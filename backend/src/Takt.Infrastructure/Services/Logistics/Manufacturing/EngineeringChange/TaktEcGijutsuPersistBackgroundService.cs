// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcGijutsuPersistBackgroundService.cs
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表新增/更新后台执行与 SignalR 完成通知
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logistics.Manufacturing;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变技术课主表新增/更新后台执行服务
/// </summary>
public sealed class TaktEcGijutsuPersistBackgroundService : ITaktEcGijutsuPersistBackgroundService
{
    /// <summary>
    /// 进行中的任务键（租户|公司|工厂|设变号）
    /// </summary>
    private static readonly ConcurrentDictionary<string, byte> RunningJobs = new();

    /// <summary>
    /// 作用域工厂（仅后台任务内新建 Scope；入队校验禁止覆盖当前请求 HttpContext）
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// 当前请求作用域的设变服务（入队前校验）
    /// </summary>
    private readonly ITaktEcGijutsuService _ecGijutsuService;

    /// <summary>
    /// 当前用户上下文
    /// </summary>
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// SignalR 推送
    /// </summary>
    private readonly ITaktSignalRDispatchService _signalRDispatchService;

    /// <summary>
    /// 租户上下文配置
    /// </summary>
    private readonly TaktTenantContextOptions _tenantOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceScopeFactory">作用域工厂</param>
    /// <param name="ecGijutsuService">当前请求设变服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="signalRDispatchService">SignalR</param>
    /// <param name="tenantOptions">租户配置</param>
    public TaktEcGijutsuPersistBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ITaktEcGijutsuService ecGijutsuService,
        ITaktUserContext userContext,
        ITaktSignalRDispatchService signalRDispatchService,
        IOptions<TaktTenantContextOptions> tenantOptions)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(ecGijutsuService);
        ArgumentNullException.ThrowIfNull(userContext);
        ArgumentNullException.ThrowIfNull(signalRDispatchService);
        ArgumentNullException.ThrowIfNull(tenantOptions);
        _serviceScopeFactory = serviceScopeFactory;
        _ecGijutsuService = ecGijutsuService;
        _userContext = userContext;
        _signalRDispatchService = signalRDispatchService;
        _tenantOptions = tenantOptions.Value;
    }

    /// <inheritdoc />
    public async Task<TaktEcGijutsuSubmittedDto> EnqueueCreateAsync(TaktEcGijutsuCreateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var context = CaptureUserContext();
        var plantCode = dto.PlantCode?.Trim() ?? string.Empty;
        var ecCode = dto.EcCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            throw new TaktBusinessException("设变单号不能为空");
        }

        // 使用当前请求 Scope 校验，禁止 CreateScope+覆盖 HttpContext（会破坏请求管道与站内消息）
        await _ecGijutsuService.EnsureEcGijutsuCreateReadyAsync(dto);

        var detailCount = dto.DetailsDeferred && dto.DeferredDetailCount > 0
            ? dto.DeferredDetailCount
            : (dto.EcDetails?.Count ?? 0);
        if (detailCount == 0
            && !string.IsNullOrWhiteSpace(dto.SourceEcId)
            && long.TryParse(dto.SourceEcId.Trim(), out _))
        {
            // 入队时尚不物化明细；回执用 DeferredDetailCount / 0，真实条数在后台物化后日志可见
            detailCount = dto.DeferredDetailCount;
        }
        var jobKey = BuildJobKey(context.TenantCode, context.CompanyCode, plantCode, ecCode);
        if (!RunningJobs.TryAdd(jobKey, 0))
        {
            throw new TaktBusinessException($"设变 {ecCode} 正在后台保存中，请稍后再试");
        }

        var submitted = new TaktEcGijutsuSubmittedDto
        {
            PlantCode = plantCode,
            EcCode = ecCode,
            IsUpdate = false,
            DetailCount = detailCount,
            EcGijutsuId = 0,
        };
        var job = new PersistJobContext(
            context.TenantCode,
            context.CompanyCode,
            context.UserId,
            context.UserName,
            plantCode,
            ecCode,
            IsUpdate: false,
            EcGijutsuId: 0,
            detailCount,
            jobKey,
            CreateDto: dto,
            UpdateDto: null);
        StartPersistJob(job);
        TaktLogger.Information(
            "[EcGijutsuPersist] 已入队后台保存 EcCode={EcCode} IsUpdate={IsUpdate} DetailCount={DetailCount} JobKey={JobKey}",
            submitted.EcCode,
            submitted.IsUpdate,
            submitted.DetailCount,
            jobKey);
        return submitted;
    }

    /// <inheritdoc />
    public async Task<TaktEcGijutsuSubmittedDto> EnqueueUpdateAsync(long id, TaktEcGijutsuUpdateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (id <= 0)
        {
            throw new TaktBusinessException("设变技术课主 ID 无效");
        }
        var context = CaptureUserContext();
        var plantCode = dto.PlantCode?.Trim() ?? string.Empty;
        var ecCode = dto.EcCode?.Trim() ?? string.Empty;

        await _ecGijutsuService.EnsureEcGijutsuUpdateReadyAsync(id, dto);
        var existing = await _ecGijutsuService.GetEcGijutsuByIdAsync(id)
            ?? throw new TaktBusinessException("设变技术课主不存在");
        if (string.IsNullOrWhiteSpace(plantCode))
        {
            plantCode = existing.PlantCode?.Trim() ?? string.Empty;
        }
        if (string.IsNullOrWhiteSpace(ecCode))
        {
            ecCode = existing.EcCode?.Trim() ?? string.Empty;
        }

        if (string.IsNullOrWhiteSpace(ecCode))
        {
            throw new TaktBusinessException("设变单号不能为空");
        }

        var detailCount = dto.EcDetails is { Count: > 0 }
            ? dto.EcDetails.Count
            : await _ecGijutsuService.CountLiveEcDetailsAsync(id);
        var jobKey = BuildJobKey(context.TenantCode, context.CompanyCode, plantCode, ecCode);
        if (!RunningJobs.TryAdd(jobKey, 0))
        {
            throw new TaktBusinessException($"设变 {ecCode} 正在后台保存中，请稍后再试");
        }

        var submitted = new TaktEcGijutsuSubmittedDto
        {
            PlantCode = plantCode,
            EcCode = ecCode,
            IsUpdate = true,
            DetailCount = detailCount,
            EcGijutsuId = id,
        };
        var job = new PersistJobContext(
            context.TenantCode,
            context.CompanyCode,
            context.UserId,
            context.UserName,
            plantCode,
            ecCode,
            IsUpdate: true,
            EcGijutsuId: id,
            detailCount,
            jobKey,
            CreateDto: null,
            UpdateDto: dto);
        StartPersistJob(job);
        TaktLogger.Information(
            "[EcGijutsuPersist] 已入队后台保存 EcCode={EcCode} IsUpdate={IsUpdate} DetailCount={DetailCount} JobKey={JobKey}",
            submitted.EcCode,
            submitted.IsUpdate,
            submitted.DetailCount,
            jobKey);
        return submitted;
    }

    /// <summary>
    /// 脱离请求 ExecutionContext / SynchronizationContext 启动后台任务。
    /// 必须 SuppressFlow：否则 Task.Run 会继承请求的 AsyncLocal HttpContext，
    /// 后台改写 IHttpContextAccessor 会污染请求线程，导致入队后站内消息「缺公司/用户名」。
    /// </summary>
    /// <param name="job">任务上下文</param>
    private void StartPersistJob(PersistJobContext job)
    {
        var flow = ExecutionContext.SuppressFlow();
        try
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await ExecutePersistJobAsync(job).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    RunningJobs.TryRemove(job.JobKey, out _);
                    TaktLogger.Error(
                        ex,
                        "[EcGijutsuPersist] 后台任务未捕获异常 EcCode={EcCode}",
                        job.EcCode);
                }
            });
        }
        finally
        {
            flow.Undo();
        }
    }

    /// <summary>
    /// 后台执行保存并推送完成事件
    /// </summary>
    /// <param name="context">任务上下文</param>
    /// <returns>任务</returns>
    private async Task ExecutePersistJobAsync(PersistJobContext context)
    {
        TaktLogger.Information(
            "[EcGijutsuPersist] 后台保存开始 EcCode={EcCode} IsUpdate={IsUpdate} DetailCount={DetailCount}",
            context.EcCode,
            context.IsUpdate,
            context.DetailCount);
        var stopwatch = Stopwatch.StartNew();
        var executeStatus = (int)TaktExecuteStatus.Success;
        string? errorMessage = null;
        string? deptExecSummary = null;
        var detailCount = context.DetailCount;
        long resultId = context.EcGijutsuId;
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            ConfigureBackgroundHttpContext(
                scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>(),
                context.TenantCode,
                context.CompanyCode,
                context.UserId,
                context.UserName);
            var service = scope.ServiceProvider.GetRequiredService<ITaktEcGijutsuService>();
            if (context.IsUpdate)
            {
                ArgumentNullException.ThrowIfNull(context.UpdateDto);
                var updated = await service.UpdateEcGijutsuAsync(context.EcGijutsuId, context.UpdateDto).ConfigureAwait(false);
                resultId = updated.EcGijutsuId;
                deptExecSummary = updated.PersistDeptExecSummary;
                detailCount = updated.PersistDetailCount;
            }
            else
            {
                ArgumentNullException.ThrowIfNull(context.CreateDto);
                var created = await service.CreateEcGijutsuAsync(context.CreateDto).ConfigureAwait(false);
                resultId = created.EcGijutsuId;
                deptExecSummary = created.PersistDeptExecSummary;
                detailCount = created.PersistDetailCount;
            }
            TaktLogger.Information(
                "[EcGijutsuPersist] 后台落库+派生完成 EcCode={EcCode} Id={Id} Summary={Summary}",
                context.EcCode,
                resultId,
                string.IsNullOrWhiteSpace(deptExecSummary) ? "(无部门写入)" : deptExecSummary);
        }
        catch (Exception ex)
        {
            executeStatus = (int)TaktExecuteStatus.Failed;
            errorMessage = ex.Message;
            TaktLogger.Error(
                ex,
                "[EcGijutsuPersist] 后台保存失败 EcCode={EcCode} IsUpdate={IsUpdate}",
                context.EcCode,
                context.IsUpdate);
        }
        finally
        {
            stopwatch.Stop();
            RunningJobs.TryRemove(context.JobKey, out _);
        }

        try
        {
            var push = new TaktSignalREcGijutsuPersistPush
            {
                TenantCode = context.TenantCode,
                CompanyCode = context.CompanyCode,
                TriggerUserName = context.UserName,
                PlantCode = context.PlantCode,
                EcCode = context.EcCode,
                IsUpdate = context.IsUpdate,
                EcGijutsuId = resultId,
                DetailCount = detailCount,
                ExecuteStatus = executeStatus,
                ExecuteDuration = stopwatch.ElapsedMilliseconds,
                ErrorMessage = errorMessage,
                CompletedAt = DateTime.Now,
            };
            await _signalRDispatchService.PushEcGijutsuPersistCompletedToUserAsync(push).ConfigureAwait(false);
            TaktLogger.Information(
                "[EcGijutsuPersist] SignalR 完成推送 EcCode={EcCode} Status={Status} DurationMs={DurationMs}",
                context.EcCode,
                executeStatus,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[EcGijutsuPersist] 完成 SignalR 推送失败 EcCode={EcCode}", context.EcCode);
        }

        try
        {
            using var notifyScope = _serviceScopeFactory.CreateScope();
            ConfigureBackgroundHttpContext(
                notifyScope.ServiceProvider.GetRequiredService<IHttpContextAccessor>(),
                context.TenantCode,
                context.CompanyCode,
                context.UserId,
                context.UserName);
            var messageService = notifyScope.ServiceProvider.GetRequiredService<ITaktMessageService>();
            var content = TaktEcGijutsuPersistMessageHelper.BuildJobCompleted(
                context.EcCode,
                context.IsUpdate,
                executeStatus == (int)TaktExecuteStatus.Success,
                stopwatch.ElapsedMilliseconds,
                detailCount,
                errorMessage,
                deptExecSummary);
            var ok = await TaktEcGijutsuPersistMessageHelper.TryNotifyAsync(messageService, content).ConfigureAwait(false);
            if (!ok)
            {
                TaktLogger.Error(
                    "[EcGijutsuPersist] 完成站内消息未落库 EcCode={EcCode} Content={Content}",
                    context.EcCode,
                    content);
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[EcGijutsuPersist] 完成消息落库失败 EcCode={EcCode}", context.EcCode);
        }
    }

    /// <summary>
    /// 抓取当前请求用户上下文
    /// </summary>
    /// <returns>用户上下文快照</returns>
    private (string TenantCode, string CompanyCode, long UserId, string UserName) CaptureUserContext()
    {
        var tenantCode = _userContext.TenantCode?.Trim() ?? string.Empty;
        var companyCode = _userContext.CompanyCode?.Trim() ?? string.Empty;
        var userId = _userContext.UserId;
        var userName = _userContext.UserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(tenantCode)
            || string.IsNullOrWhiteSpace(companyCode)
            || userId is not > 0
            || string.IsNullOrWhiteSpace(userName))
        {
            throw new TaktBusinessException("用户上下文缺失，无法提交后台保存");
        }
        return (tenantCode, companyCode, userId.Value, userName);
    }

    /// <summary>
    /// 为后台任务注入租户/公司/用户 HTTP 上下文（仅在 Task.Run 内调用）
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="userName">用户名</param>
    private void ConfigureBackgroundHttpContext(
        IHttpContextAccessor httpContextAccessor,
        string tenantCode,
        string companyCode,
        long userId,
        string userName)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        var httpContext = new DefaultHttpContext();
        TaktUserContext.ApplyRequestTenantCompanyHeaders(httpContext, tenantCode, companyCode, _tenantOptions);
        var claims = new List<Claim>
        {
            new("sub", userId.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, userId.ToString(CultureInfo.InvariantCulture)),
            new(TaktClaimNames.PreferredUsername, userName),
            new(ClaimTypes.Name, userName),
            new(TaktClaimNames.TenantCode, tenantCode),
            new(TaktClaimNames.CompanyCode, companyCode),
        };
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "BackgroundJob"));
        httpContextAccessor.HttpContext = httpContext;
    }

    /// <summary>
    /// 构建并发去重键
    /// </summary>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="ecCode">设变号</param>
    /// <returns>任务键</returns>
    private static string BuildJobKey(string tenantCode, string companyCode, string plantCode, string ecCode) =>
        $"{tenantCode}|{companyCode}|{plantCode}|{ecCode}";

    /// <summary>
    /// 后台任务上下文
    /// </summary>
    /// <param name="TenantCode">租户</param>
    /// <param name="CompanyCode">公司</param>
    /// <param name="UserId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="PlantCode">工厂</param>
    /// <param name="EcCode">设变号</param>
    /// <param name="IsUpdate">是否更新</param>
    /// <param name="EcGijutsuId">主表 ID</param>
    /// <param name="DetailCount">明细行数</param>
    /// <param name="JobKey">去重键</param>
    /// <param name="CreateDto">创建 DTO</param>
    /// <param name="UpdateDto">更新 DTO</param>
    private sealed record PersistJobContext(
        string TenantCode,
        string CompanyCode,
        long UserId,
        string UserName,
        string PlantCode,
        string EcCode,
        bool IsUpdate,
        long EcGijutsuId,
        int DetailCount,
        string JobKey,
        TaktEcGijutsuCreateDto? CreateDto,
        TaktEcGijutsuUpdateDto? UpdateDto);
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemRecalculateBackgroundService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种月平均重算后台执行与 SignalR 完成通知
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
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Logistics.Manufacturing;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本机种月平均重算后台执行服务
/// </summary>
public sealed class TaktBomMaterialCostItemRecalculateBackgroundService : ITaktBomMaterialCostItemRecalculateBackgroundService
{
    /// <summary>
    /// 进行中的任务键（租户|公司|核算月|是否重算）
    /// </summary>
    private static readonly ConcurrentDictionary<string, byte> RunningJobs = new();
    /// <summary>
    /// 作用域工厂（后台任务内解析计算服务）
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;
    /// <summary>
    /// 当前用户上下文
    /// </summary>
    private readonly ITaktUserContext _userContext;
    /// <summary>
    /// SignalR 推送服务
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
    /// <param name="userContext">当前用户上下文</param>
    /// <param name="signalRDispatchService">SignalR 推送服务</param>
    /// <param name="tenantOptions">租户上下文配置</param>
    public TaktBomMaterialCostItemRecalculateBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ITaktUserContext userContext,
        ITaktSignalRDispatchService signalRDispatchService,
        IOptions<TaktTenantContextOptions> tenantOptions)
    {
        ArgumentNullException.ThrowIfNull(serviceScopeFactory);
        ArgumentNullException.ThrowIfNull(userContext);
        ArgumentNullException.ThrowIfNull(signalRDispatchService);
        ArgumentNullException.ThrowIfNull(tenantOptions);
        _serviceScopeFactory = serviceScopeFactory;
        _userContext = userContext;
        _signalRDispatchService = signalRDispatchService;
        _tenantOptions = tenantOptions.Value;
    }

    /// <summary>
    /// 提交后台重算任务（立即返回；完成后通过 SignalR 通知触发用户）
    /// </summary>
    /// <param name="queryDto">已校验的计算查询条件</param>
    /// <param name="forceRecalculate">true=重算（归档旧成本到 ExtField 后重写）；false=计算成本</param>
    /// <returns>任务</returns>
    public Task EnqueueRecalculateAsync(
        TaktBomCalculateQueryDto queryDto,
        bool forceRecalculate = false)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        if (queryDto.ProcessRecordCount < 0)
        {
            throw new TaktBusinessException("处理记录数不能为负数（0 表示全部）");
        }
        var tenantCode = _userContext.TenantCode?.Trim() ?? string.Empty;
        var companyCode = _userContext.CompanyCode?.Trim() ?? string.Empty;
        var userId = _userContext.UserId;
        var UserName = _userContext.UserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(tenantCode)
            || string.IsNullOrWhiteSpace(companyCode)
            || userId is not > 0
            || string.IsNullOrWhiteSpace(UserName))
        {
            throw new TaktBusinessException("用户上下文缺失，无法提交后台重算");
        }

        var prepared = TaktBomCalculateService.PrepareBomCalculateQuery(queryDto);
        var jobKey = BuildJobKey(tenantCode, companyCode, prepared.ProcessedMonth, forceRecalculate);
        if (!RunningJobs.TryAdd(jobKey, 0))
        {
            throw new TaktBusinessException("该核算月份的重算任务正在执行中，请稍后再试");
        }

        var captured = new RecalculateJobContext(
            tenantCode,
            companyCode,
            userId.Value,
            UserName,
            prepared.Query,
            prepared.ProcessedMonth,
            forceRecalculate,
            jobKey);
        // SuppressFlow：避免后台改写 IHttpContextAccessor 污染请求 AsyncLocal
        var flow = ExecutionContext.SuppressFlow();
        try
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await ExecuteRecalculateJobAsync(captured).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    RunningJobs.TryRemove(captured.JobKey, out _);
                    TaktLogger.Error(
                        ex,
                        "[BomMaterialCostItem] 后台任务未捕获异常 Month={ProcessedMonth}",
                        captured.ProcessedMonth);
                }
            });
        }
        finally
        {
            flow.Undo();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 后台执行重算并在完成后推送 SignalR
    /// </summary>
    /// <param name="context">任务上下文</param>
    /// <returns>任务</returns>
    private async Task ExecuteRecalculateJobAsync(RecalculateJobContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        TaktBomCalculateCostResultDto? result = null;
        var executeStatus = (int)TaktExecuteStatus.Success;
        string? errorMessage = null;
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            ConfigureBackgroundHttpContext(
                httpContextAccessor,
                context.TenantCode,
                context.CompanyCode,
                context.UserId,
                context.UserName);
            var bomCalculateService = scope.ServiceProvider.GetRequiredService<ITaktBomCalculateService>();
            result = context.ForceRecalculate
                ? await bomCalculateService.RecalculateBomCalculateCostAsync(context.Query)
                : await bomCalculateService.SumBomCalculateCostAsync(context.Query);
        }
        catch (Exception ex)
        {
            executeStatus = (int)TaktExecuteStatus.Failed;
            errorMessage = ex.Message;
            TaktLogger.Error(ex, "[BomMaterialCostItem] 后台重算机种月平均失败 Month={ProcessedMonth}", context.ProcessedMonth);
        }
        finally
        {
            stopwatch.Stop();
            RunningJobs.TryRemove(context.JobKey, out _);
        }

        try
        {
            var push = new TaktSignalRBomMaterialCostItemRecalculatePush
            {
                TenantCode = context.TenantCode,
                CompanyCode = context.CompanyCode,
                TriggerUserName = context.UserName,
                ProcessedMonth = context.ProcessedMonth,
                ForceRecalculate = context.ForceRecalculate,
                ExecuteStatus = executeStatus,
                ExecuteDuration = stopwatch.ElapsedMilliseconds,
                ErrorMessage = errorMessage,
                ScannedRowCount = result?.ScannedRowCount ?? 0,
                RefreshedGroupCount = result?.RefreshedGroupCount ?? 0,
                SkippedGroupCount = result?.SkippedGroupCount ?? 0,
                ResetGroupCount = result?.ResetGroupCount ?? 0,
                ProcessedMonthCount = result?.ProcessedMonthCount ?? 0,
                CompletedAt = DateTime.Now,
            };
            await _signalRDispatchService.PushBomMaterialCostItemRecalculateCompletedToUserAsync(push);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[BomMaterialCostItem] 重算完成 SignalR 推送失败 Month={ProcessedMonth}", context.ProcessedMonth);
        }

        try
        {
            using var notifyScope = _serviceScopeFactory.CreateScope();
            var httpContextAccessor = notifyScope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            ConfigureBackgroundHttpContext(
                httpContextAccessor,
                context.TenantCode,
                context.CompanyCode,
                context.UserId,
                context.UserName);
            var messageService = notifyScope.ServiceProvider.GetRequiredService<ITaktMessageService>();
            var success = executeStatus == (int)TaktExecuteStatus.Success;
            await TaktBomZeroPriceOperationMessageHelper.TryNotifyAsync(
                messageService,
                TaktBomZeroPriceOperationMessageHelper.BuildCostJobCompleted(
                    context.ProcessedMonth,
                    success,
                    stopwatch.ElapsedMilliseconds,
                    result?.RefreshedGroupCount ?? 0,
                    result?.SkippedGroupCount ?? 0,
                    errorMessage));
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[BomMaterialCostItem] 重算完成消息落库失败 Month={ProcessedMonth}", context.ProcessedMonth);
        }
    }

    /// <summary>
    /// 为后台任务注入租户/公司/用户 HTTP 上下文（AsyncLocal 隔离并发任务）
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    private void ConfigureBackgroundHttpContext(
        IHttpContextAccessor httpContextAccessor,
        string tenantCode,
        string companyCode,
        long userId,
        string UserName)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        var httpContext = new DefaultHttpContext();
        TaktUserContext.ApplyRequestTenantCompanyHeaders(httpContext, tenantCode, companyCode, _tenantOptions);
        var claims = new List<Claim>
        {
            new("sub", userId.ToString(CultureInfo.InvariantCulture)),
            new(TaktClaimNames.PreferredUsername, UserName),
            new(TaktClaimNames.TenantCode, tenantCode),
            new(TaktClaimNames.CompanyCode, companyCode),
        };
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "BackgroundJob"));
        httpContextAccessor.HttpContext = httpContext;
    }

    /// <summary>
    /// 构建并发去重键
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="processedMonth">核算月份</param>
    /// <param name="forceRecalculate">是否强制重算</param>
    /// <returns>任务键</returns>
    private static string BuildJobKey(string tenantCode, string companyCode, string processedMonth, bool forceRecalculate) =>
        $"{tenantCode}|{companyCode}|{processedMonth}|{(forceRecalculate ? 1 : 0)}";

    /// <summary>
    /// 后台任务上下文快照
    /// </summary>
    /// <param name="TenantCode">租户编码</param>
    /// <param name="CompanyCode">公司编码</param>
    /// <param name="UserId">触发用户 ID</param>
    /// <param name="UserName">触发用户名</param>
    /// <param name="Query">规范化后的计算查询</param>
    /// <param name="ProcessedMonth">核算月份 yyyy-MM</param>
    /// <param name="ForceRecalculate">true=重算；false=计算成本</param>
    /// <param name="JobKey">并发去重键</param>
    private sealed record RecalculateJobContext(
        string TenantCode,
        string CompanyCode,
        long UserId,
        string UserName,
        TaktBomCalculateQueryDto Query,
        string ProcessedMonth,
        bool ForceRecalculate,
        string JobKey);
}

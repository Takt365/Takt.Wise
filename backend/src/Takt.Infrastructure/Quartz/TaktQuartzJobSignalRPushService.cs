// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobSignalRPushService.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz Job 执行完成 SignalR 推送，并落库 TaktMessage（谁执行 / 推送给谁）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Foundation;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz Job 执行完成 SignalR 推送实现（先落库在线消息，再公司组广播）
/// </summary>
public sealed class TaktQuartzJobSignalRPushService : ITaktQuartzJobSignalRPushService
{
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktMessageService _messageService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TaktTenantContextOptions _tenantOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="signalRDispatchService">SignalR 调度服务</param>
    /// <param name="messageService">在线消息服务（执行完成落库）</param>
    /// <param name="httpContextAccessor">HTTP 上下文（后台注入租户/公司）</param>
    /// <param name="tenantOptions">租户请求头配置</param>
    public TaktQuartzJobSignalRPushService(
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktMessageService messageService,
        IHttpContextAccessor httpContextAccessor,
        IOptions<TaktTenantContextOptions> tenantOptions)
    {
        ArgumentNullException.ThrowIfNull(signalRDispatchService);
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(tenantOptions);
        _signalRDispatchService = signalRDispatchService;
        _messageService = messageService;
        _httpContextAccessor = httpContextAccessor;
        _tenantOptions = tenantOptions.Value;
    }

    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="task">定时任务实体（含最新统计字段）</param>
    /// <param name="log">执行日志</param>
    /// <param name="triggerUserName">触发用户名（手动触发时有值）</param>
    /// <returns>任务</returns>
    public async Task PushTaskExecutedAsync(
        TaktQuartzTask task,
        TaktQuartzLog log,
        string? triggerUserName)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(log);
        // 幂等补强：Job 入口已注入；此处再写一次，防止中间步骤清空 HttpContext
        TaktQuartzAmbientHttpContext.Configure(
            _httpContextAccessor,
            _tenantOptions,
            task.TenantCode,
            task.CompanyCode,
            triggerUserName);
        await TryPersistExecutedMessageAsync(task, log, triggerUserName);
        var push = new TaktSignalRQuartzTaskExecutedPush
        {
            TenantCode = task.TenantCode,
            CompanyCode = task.CompanyCode,
            QuartzTaskId = task.Id,
            QuartzLogId = log.Id,
            TaskCode = task.TaskCode ?? string.Empty,
            TaskName = task.TaskName ?? string.Empty,
            ExecuteStatus = (int)log.ExecuteStatus,
            ExecuteMessage = log.ExecuteMessage,
            ErrorInfo = log.ErrorInfo,
            ExecuteDuration = log.ExecuteDuration,
            ExecuteCount = task.ExecuteCount,
            LastRunAt = task.LastRunAt,
            NextRunAt = task.NextRunAt,
            TriggerUserName = triggerUserName?.Trim(),
            ExecutedAt = log.ExecuteTime,
        };
        await _signalRDispatchService.PushQuartzTaskExecutedAsync(push);
    }

    /// <summary>
    /// 落库执行完成消息（失败仅记日志，不阻断公司组 SignalR）
    /// </summary>
    /// <param name="task">任务</param>
    /// <param name="log">日志</param>
    /// <param name="triggerUserName">触发用户</param>
    private async Task TryPersistExecutedMessageAsync(
        TaktQuartzTask task,
        TaktQuartzLog log,
        string? triggerUserName)
    {
        try
        {
            await _messageService.CreateAndSendQuartzTaskExecutedMessageAsync(task, log, triggerUserName);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(
                ex,
                "Quartz 执行消息落库/私信推送失败 TaskId={TaskId}, Code={TaskCode}, Trigger={Trigger}",
                task.Id,
                task.TaskCode,
                triggerUserName);
        }
    }
}

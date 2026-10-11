// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRDispatchService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时推送调度服务（强退、私信、广播）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Foundation;
using Takt.Shared.Models.Logistics.Manufacturing;
using Takt.Shared.Models.Workflow;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR 实时推送调度服务
/// </summary>
public class TaktSignalRDispatchService : ITaktSignalRDispatchService
{
    private readonly IHubContext<TaktConnectHub> _connectHubContext;
    private readonly IHubContext<TaktNotificationHub> _notificationHubContext;
    private readonly IHubContext<TaktEcChangeHub> _ecChangeHubContext;
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;
    private readonly ITaktOnlineService _onlineService;
    private readonly Lazy<ITaktMessageService> _messageService;
    private readonly ITaktUserContext _userContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectHubContext">连接 Hub 上下文</param>
    /// <param name="notificationHubContext">通知 Hub 上下文</param>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="messageService">在线消息服务（延迟解析，避免与 TaktMessageService 循环依赖）</param>
    /// <param name="userContext">当前登录用户上下文（强退操作者）</param>
    public TaktSignalRDispatchService(
        IHubContext<TaktConnectHub> connectHubContext,
        IHubContext<TaktNotificationHub> notificationHubContext,
        IHubContext<TaktEcChangeHub> ecChangeHubContext,
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktOnlineService onlineService,
        Lazy<ITaktMessageService> messageService,
        ITaktUserContext userContext)
    {
        _connectHubContext = connectHubContext;
        _notificationHubContext = notificationHubContext;
        _ecChangeHubContext = ecChangeHubContext;
        _onlineRepository = onlineRepository;
        _onlineService = onlineService;
        _messageService = messageService;
        _userContext = userContext;
    }

    /// <summary>
    /// 强制踢出在线用户（强退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="reason">强退原因</param>
    /// <param name="connectionId">SignalR 连接 ID（主键查无记录时回退定位）</param>
    /// <param name="delaySeconds">延迟强退秒数（0 表示立即强退）</param>
    /// <returns>任务</returns>
    public async Task ForceKickOnlineAsync(long onlineId, string? reason = null, string? connectionId = null, int delaySeconds = 0)
    {
        var online = await ResolveOnlineForForceKickAsync(onlineId, connectionId);

        if (online.OnlineStatus != 0)
        {
            throw new TaktBusinessException("该用户已不在线，无法强退");
        }

        var delay = Math.Clamp(delaySeconds, 0, TaktOnlineConstants.MaxDelayedKickSeconds);
        var (operatorUserId, operatorUserName) = ResolveKickOperator();
        if (delay > 0)
        {
            await SendForceLogoutScheduledAsync(online, reason, delay, operatorUserId, operatorUserName);
            _ = ExecuteDelayedForceKickAsync(
                online.Id,
                reason,
                online.ConnectionId,
                delay,
                operatorUserId,
                operatorUserName);
            TaktLogger.Information(
                "已安排延迟强退，用户: {UserName}, ConnectionId: {ConnectionId}, DelaySeconds: {DelaySeconds}",
                online.UserName,
                online.ConnectionId,
                delay);
            return;
        }

        var kickReason = string.IsNullOrWhiteSpace(reason) ? "您已被管理员强制下线" : reason.Trim();
        await TryPersistKickMessageAsync(
            online,
            kickReason,
            TaktOnlineConstants.KickExecuteMessageGroup,
            operatorUserId,
            operatorUserName);
        await ExecuteForceKickCoreAsync(online, reason);
    }

    /// <summary>
    /// 批量强制踢出在线用户
    /// </summary>
    /// <param name="onlineIds">在线用户记录 ID 列表</param>
    /// <param name="reason">强退原因</param>
    /// <param name="delaySeconds">延迟强退秒数（0 表示立即强退）</param>
    /// <returns>任务</returns>
    public async Task ForceKickOnlineBatchAsync(IEnumerable<long> onlineIds, string? reason = null, int delaySeconds = 0)
    {
        var idList = onlineIds?.Distinct().ToList() ?? new List<long>();
        foreach (var onlineId in idList)
        {
            try
            {
                await ForceKickOnlineAsync(onlineId, reason, null, delaySeconds);
            }
            catch (TaktBusinessException ex)
            {
                TaktLogger.Warning("批量强退跳过记录 {OnlineId}: {Message}", onlineId, ex.Message);
            }
        }
    }

    /// <summary>
    /// 推送私信到在线客户端
    /// </summary>
    /// <param name="message">私信推送模型</param>
    /// <returns>任务</returns>
    public async Task PushPrivateMessageAsync(TaktSignalRPrivateMessagePush message)
    {
        var companyCode = message.CompanyCode?.Trim() ?? string.Empty;
        var toUserName = message.ToUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(toUserName))
        {
            throw new TaktBusinessException("私信推送缺少公司编码或接收者用户名");
        }

        var payload = new
        {
            MessageId = message.MessageId.ToString(),
            message.FromUserName,
            FromUserId = message.FromUserId?.ToString(),
            message.FromUserNickName,
            ToUserName = toUserName,
            ToUserId = message.ToUserId?.ToString(),
            message.MessageTitle,
            message.MessageContent,
            message.FileName,
            message.AccessUrl,
            message.MessageType,
            message.MessageGroup,
            message.SendTime,
            message.ReadTime,
            message.ReadStatus,
        };

        var recipientUserId = message.ToUserId.HasValue && message.ToUserId.Value > 0
            ? message.ToUserId.Value.ToString()
            : null;
        var targetCompanyCodes = await ResolvePrivateMessageTargetCompanyCodesAsync(companyCode, toUserName);
        foreach (var targetCompanyCode in targetCompanyCodes)
        {
            var userGroup = TaktSignalRGroupNames.UserGroup(targetCompanyCode, toUserName);
            await _notificationHubContext.Clients.Group(userGroup).SendAsync("ReceiveMessage", payload);
            TaktSignalRLogging.LogPrivateMessagePushed(toUserName, targetCompanyCode, message.MessageId, userGroup, recipientUserId);
            await PushMessageStatisticsToUserAsync(targetCompanyCode, toUserName, message.ToUserId);
        }
    }

    /// <summary>
    /// 解析私信推送目标公司组（优先接收者当前在线会话所在公司，否则回退消息公司）
    /// </summary>
    /// <param name="messageCompanyCode">消息所属公司编码</param>
    /// <param name="toUserName">接收者登录用户名</param>
    /// <returns>去重后的公司编码列表</returns>
    private async Task<IReadOnlyList<string>> ResolvePrivateMessageTargetCompanyCodesAsync(
        string messageCompanyCode,
        string toUserName)
    {
        var onlineSessions = await _onlineRepository.GetListAsync(online =>
            online.OnlineStatus == 0
            && online.UserName == toUserName);
        var onlineCompanyCodes = onlineSessions
            .Select(online => online.CompanyCode?.Trim())
            .Where(code => !string.IsNullOrWhiteSpace(code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToList();
        if (onlineCompanyCodes.Count == 0)
        {
            return new[] { messageCompanyCode };
        }

        var matchedMessageCompany = onlineCompanyCodes.FirstOrDefault(code =>
            string.Equals(code, messageCompanyCode, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(matchedMessageCompany))
        {
            return new[] { matchedMessageCompany };
        }

        return onlineCompanyCodes;
    }

    /// <summary>
    /// 推送广播通知到公司内在线客户端
    /// </summary>
    /// <param name="broadcast">广播推送模型</param>
    /// <returns>任务</returns>
    public async Task PushBroadcastMessageAsync(TaktSignalRBroadcastPush broadcast)
    {
        if (string.IsNullOrWhiteSpace(broadcast.CompanyCode))
        {
            throw new TaktBusinessException("广播消息缺少公司编码");
        }

        var payload = new
        {
            broadcast.FromUserName,
            broadcast.MessageTitle,
            broadcast.MessageContent,
            broadcast.MessageType,
            broadcast.MessageGroup,
            SendTime = broadcast.SendTime ?? DateTime.Now,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(broadcast.CompanyCode))
            .SendAsync("ReceiveBroadcast", payload);
    }

    /// <summary>
    /// 向指定用户推送最新在线统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    public async Task PushOnlineStatisticsToUserAsync(string companyCode, string UserName, long? userId = null)
    {
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var statistics = await _onlineService.GetOnlineStatisticsAsync(new TaktOnlineStatisticsQueryDto
        {
            UserName = UserName,
        });
        var payload = ToOnlineStatisticsPayload(statistics);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);

        await _connectHubContext.Clients.Group(userGroup).SendAsync("OnlineStatisticsUpdated", payload);
        TaktSignalRLogging.LogStatisticsPushed("online", UserName, companyCode);
    }

    /// <summary>
    /// 向指定用户推送最新消息统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    public async Task PushMessageStatisticsToUserAsync(string companyCode, string UserName, long? userId = null)
    {
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var statistics = await _messageService.Value.GetMessageStatisticsByUserNameAsync(UserName, userId);
        var payload = ToMessageStatisticsPayload(statistics);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);

        await _notificationHubContext.Clients.Group(userGroup).SendAsync("MessageStatisticsUpdated", payload);
        TaktSignalRLogging.LogStatisticsPushed("message", UserName, companyCode);
    }

    /// <summary>
    /// 推送流程定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushFlowSchemeChangedAsync(TaktSignalRFlowSchemeChangedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("流程定义推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            FlowSchemeId = push.FlowSchemeId.ToString(),
            push.ProcessKey,
            push.ProcessName,
            push.ChangeType,
            push.OperatorUserName,
            ChangedAt = push.ChangedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("FlowSchemeChanged", payload);
        TaktSignalRLogging.LogWorkflowPushed("scheme-changed", companyCode, push.ProcessKey);
    }

    /// <summary>
    /// 向指定用户推送流程实例推进事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <param name="targetUserName">目标用户名</param>
    /// <returns>任务</returns>
    public async Task PushFlowInstanceProgressedToUserAsync(TaktSignalRFlowInstanceProgressedPush push, string targetUserName)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var UserName = targetUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            FlowInstanceId = push.FlowInstanceId.ToString(),
            push.InstanceCode,
            push.ProcessName,
            push.InstanceStatus,
            push.ActionType,
            push.CurrentActivityName,
            push.StartUserName,
            ProgressedAt = push.ProgressedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("FlowInstanceProgressed", payload);
        TaktSignalRLogging.LogWorkflowPushed("instance-progressed", companyCode, push.InstanceCode, UserName);
    }

    /// <summary>
    /// 向指定用户推送最新待办数量
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushFlowTodoCountToUserAsync(TaktSignalRFlowTodoCountPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var UserName = push.UserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            UserName = UserName,
            UserId = push.UserId?.ToString(),
            push.TodoCount,
            UpdatedAt = push.UpdatedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("FlowTodoCountUpdated", payload);
        TaktSignalRLogging.LogWorkflowPushed("todo-count", companyCode, push.TodoCount.ToString(), UserName);
    }

    /// <summary>
    /// 推送定时任务定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushQuartzTaskChangedAsync(TaktSignalRQuartzTaskChangedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("定时任务推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            QuartzTaskId = push.QuartzTaskId.ToString(),
            push.TaskCode,
            push.TaskName,
            push.ChangeType,
            push.OperatorUserName,
            ChangedAt = push.ChangedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("QuartzTaskChanged", payload);
        TaktSignalRLogging.LogQuartzPushed("task-changed", companyCode, push.TaskCode);
    }

    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushQuartzTaskExecutedAsync(TaktSignalRQuartzTaskExecutedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new TaktBusinessException("定时任务执行推送缺少公司编码");
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            QuartzTaskId = push.QuartzTaskId.ToString(),
            QuartzLogId = push.QuartzLogId.ToString(),
            push.TaskCode,
            push.TaskName,
            push.ExecuteStatus,
            push.ExecuteMessage,
            push.ErrorInfo,
            push.ExecuteDuration,
            push.ExecuteCount,
            push.LastRunAt,
            push.NextRunAt,
            push.TriggerUserName,
            ExecutedAt = push.ExecutedAt,
        };

        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("QuartzTaskExecuted", payload);
        TaktSignalRLogging.LogQuartzPushed("task-executed", companyCode, push.TaskCode);
    }

    /// <summary>
    /// 向触发用户推送 BOM 物料成本机种月平均重算完成事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushBomMaterialCostItemRecalculateCompletedToUserAsync(TaktSignalRBomMaterialCostItemRecalculatePush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var UserName = push.TriggerUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            TriggerUserName = UserName,
            push.ProcessedMonth,
            push.ForceRecalculate,
            push.ExecuteStatus,
            push.ExecuteDuration,
            push.ErrorMessage,
            push.ScannedRowCount,
            push.RefreshedGroupCount,
            push.SkippedGroupCount,
            push.ResetGroupCount,
            push.ProcessedMonthCount,
            CompletedAt = push.CompletedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("BomMaterialCostItemRecalculateCompleted", payload);
        TaktSignalRLogging.LogWorkflowPushed("bom-material-cost-item-recalculate", companyCode, push.ProcessedMonth, UserName);
    }

    /// <summary>
    /// 向触发用户推送设变技术课主表后台保存完成事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcGijutsuPersistCompletedToUserAsync(TaktSignalREcGijutsuPersistPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode?.Trim() ?? string.Empty;
        var UserName = push.TriggerUserName?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(companyCode) || string.IsNullOrWhiteSpace(UserName))
        {
            return;
        }

        var payload = new
        {
            TenantCode = push.TenantCode,
            CompanyCode = companyCode,
            TriggerUserName = UserName,
            push.PlantCode,
            push.EcCode,
            push.IsUpdate,
            EcGijutsuId = push.EcGijutsuId.ToString(CultureInfo.InvariantCulture),
            push.DetailCount,
            push.ExecuteStatus,
            push.ExecuteDuration,
            push.ErrorMessage,
            CompletedAt = push.CompletedAt,
        };

        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _notificationHubContext.Clients.Group(userGroup).SendAsync("EcGijutsuPersistCompleted", payload);
        TaktSignalRLogging.LogWorkflowPushed("ec-gijutsu-persist", companyCode, push.EcCode, UserName);
    }

    /// <summary>
    /// 推送工程变更通知到部门组（SendChangeNotification）
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcChangeNotificationAsync(TaktEcChangeNotificationPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode.Trim();
        var group = TaktEcHelper.DeptGroup(companyCode, push.DeptCode);
        await _ecChangeHubContext.Clients.Group(group).SendAsync("ChangeNotification", push);
    }

    /// <summary>
    /// 推送工程变更执行任务分配到部门组
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcExecutionTaskAssignedAsync(TaktEcExecutionTaskAssignedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode.Trim();
        var deptGroup = TaktEcHelper.DeptGroup(companyCode, push.DeptCode);
        var taskGroup = TaktEcHelper.TaskGroup(companyCode, push.TaskId);
        await _ecChangeHubContext.Clients.Group(deptGroup).SendAsync("TaskAssigned", push);
        await _ecChangeHubContext.Clients.Group(taskGroup).SendAsync("TaskAssigned", push);
    }

    /// <summary>
    /// 推送工程变更任务进度到任务组与部门组
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcExecutionTaskProgressAsync(TaktEcExecutionTaskProgressPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode.Trim();
        var deptGroup = TaktEcHelper.DeptGroup(companyCode, push.DeptCode);
        var taskGroup = TaktEcHelper.TaskGroup(companyCode, push.TaskId);
        await _ecChangeHubContext.Clients.Group(taskGroup).SendAsync("TaskProgress", push);
        await _ecChangeHubContext.Clients.Group(deptGroup).SendAsync("TaskProgress", push);
    }

    /// <summary>
    /// 推送工程变更闭环完成（公司广播组）
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcChangeClosedAsync(TaktEcChangeClosedPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode.Trim();
        await _ecChangeHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("ChangeClosed", push);
    }

    /// <summary>
    /// 推送工程变更任务超时/阻塞预警
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    public async Task PushEcExecutionTaskAlertAsync(TaktEcExecutionTaskAlertPush push)
    {
        ArgumentNullException.ThrowIfNull(push);
        var companyCode = push.CompanyCode.Trim();
        var deptGroup = TaktEcHelper.DeptGroup(companyCode, push.DeptCode);
        var taskGroup = TaktEcHelper.TaskGroup(companyCode, push.TaskId);
        await _ecChangeHubContext.Clients.Group(deptGroup).SendAsync("TaskAlert", push);
        await _ecChangeHubContext.Clients.Group(taskGroup).SendAsync("TaskAlert", push);
        await _notificationHubContext.Clients
            .Group(TaktSignalRGroupNames.NotificationsGroup(companyCode))
            .SendAsync("TaskAlert", push);
    }

    /// <summary>
    /// 向发起人推送部门确认通知
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">发起人用户名</param>
    /// <param name="payload">载荷</param>
    /// <returns>任务</returns>
    public async Task PushEcNotificationConfirmedToUserAsync(string companyCode, string UserName, object payload)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(UserName);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _ecChangeHubContext.Clients.Group(userGroup).SendAsync("NotificationConfirmed", payload);
    }

    /// <summary>
    /// 向发起人推送变更闭环完成
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">发起人用户名</param>
    /// <param name="push">闭环模型</param>
    /// <returns>任务</returns>
    public async Task PushEcChangeClosedToUserAsync(string companyCode, string UserName, TaktEcChangeClosedPush push)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(UserName);
        var userGroup = TaktSignalRGroupNames.UserGroup(companyCode, UserName);
        await _ecChangeHubContext.Clients.Group(userGroup).SendAsync("ChangeClosed", push);
    }

    /// <summary>
    /// 立即强退在线用户（推送 ForceLogout 并关闭会话）
    /// </summary>
    /// <param name="online">在线实体</param>
    /// <param name="reason">强退原因</param>
    /// <returns>任务</returns>
    private async Task ExecuteForceKickCoreAsync(TaktOnline online, string? reason)
    {
        var kickReason = string.IsNullOrWhiteSpace(reason) ? "您已被管理员强制下线" : reason.Trim();
        var payload = new
        {
            Message = kickReason,
            OnlineId = online.Id.ToString(),
            ConnectionId = online.ConnectionId,
            UserName = online.UserName,
            ForceKickTime = DateTime.Now,
        };

        await SendConnectHubEventToOnlineAsync(online, "ForceLogout", payload);

        var disconnectTime = DateTime.Now;
        online.ConnectLocation = TaktHttpAuditHelper.ResolveLocationFromIp(
            online.ConnectIp,
            online.ConnectLocation);
        online.Remark = $"强退: {kickReason}";
        online.UpdatedAt = disconnectTime;
        await _onlineRepository.UpdateAsync(online);
        var onlineConnectionId = online.ConnectionId?.Trim();
        if (!string.IsNullOrEmpty(onlineConnectionId))
        {
            await _onlineService.CloseOnlineSessionByConnectionIdAsync(
                onlineConnectionId,
                disconnectTime,
                onlineStatus: 2);
        }
        online = await _onlineRepository.GetByIdAsync(online.Id) ?? online;

        await PushOnlineStatisticsToUserAsync(online.CompanyCode, online.UserName, online.UserId);

        TaktLogger.Information("强退在线用户成功，用户: {UserName}, ConnectionId: {ConnectionId}", online.UserName, online.ConnectionId);
    }

    /// <summary>
    /// 推送延迟强退预告（不关闭会话）
    /// </summary>
    /// <param name="online">在线实体</param>
    /// <param name="reason">强退原因</param>
    /// <param name="delaySeconds">延迟秒数</param>
    /// <param name="fromUserId">强退操作者用户 ID</param>
    /// <param name="fromUserName">强退操作者登录名</param>
    /// <returns>任务</returns>
    private async Task SendForceLogoutScheduledAsync(
        TaktOnline online,
        string? reason,
        int delaySeconds,
        long? fromUserId,
        string? fromUserName)
    {
        var kickAt = DateTime.Now.AddSeconds(delaySeconds);
        var scheduleMessage = string.IsNullOrWhiteSpace(reason)
            ? $"您将在 {delaySeconds / 60} 分钟后被强制下线，请尽快保存工作"
            : reason.Trim();
        var payload = new
        {
            Message = scheduleMessage,
            DelaySeconds = delaySeconds,
            KickAt = kickAt,
            OnlineId = online.Id.ToString(),
            ConnectionId = online.ConnectionId,
            UserName = online.UserName,
        };

        await SendConnectHubEventToOnlineAsync(online, "ForceLogoutScheduled", payload);
        await TryPersistKickMessageAsync(
            online,
            scheduleMessage,
            TaktOnlineConstants.KickScheduleMessageGroup,
            fromUserId,
            fromUserName);
    }

    /// <summary>
    /// 延迟到期后执行强退
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="reason">强退原因</param>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <param name="delaySeconds">延迟秒数</param>
    /// <param name="fromUserId">强退操作者用户 ID</param>
    /// <param name="fromUserName">强退操作者登录名</param>
    /// <returns>任务</returns>
    private async Task ExecuteDelayedForceKickAsync(
        long onlineId,
        string? reason,
        string? connectionId,
        int delaySeconds,
        long? fromUserId,
        string? fromUserName)
    {
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            var online = await ResolveOnlineForForceKickAsync(onlineId, connectionId);
            if (online.OnlineStatus != 0)
            {
                TaktLogger.Information("延迟强退已跳过（用户已离线），OnlineId: {OnlineId}", onlineId);
                return;
            }

            var kickReason = string.IsNullOrWhiteSpace(reason) ? "您已被管理员强制下线" : reason.Trim();
            await TryPersistKickMessageAsync(
                online,
                kickReason,
                TaktOnlineConstants.KickExecuteMessageGroup,
                fromUserId,
                fromUserName);
            await ExecuteForceKickCoreAsync(online, kickReason);
        }
        catch (TaktBusinessException ex)
        {
            TaktLogger.Warning("延迟强退失败 OnlineId: {OnlineId}: {Message}", onlineId, ex.Message);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "延迟强退执行异常 OnlineId: {OnlineId}", onlineId);
        }
    }

    /// <summary>
    /// 向在线用户 ConnectHub 连接投递事件（Client 与 Group 二选一）
    /// </summary>
    /// <param name="online">在线实体</param>
    /// <param name="eventName">Hub 事件名</param>
    /// <param name="payload">载荷</param>
    /// <returns>任务</returns>
    private async Task SendConnectHubEventToOnlineAsync(TaktOnline online, string eventName, object payload)
    {
        var userGroup = TaktSignalRGroupNames.UserGroup(online.CompanyCode, online.UserName);
        var onlineConnectionId = online.ConnectionId?.Trim();
        if (!string.IsNullOrEmpty(onlineConnectionId))
        {
            await _connectHubContext.Clients.Client(onlineConnectionId).SendAsync(eventName, payload);
        }
        else
        {
            await _connectHubContext.Clients.Group(userGroup).SendAsync(eventName, payload);
        }
    }

    /// <summary>
    /// 解析强退目标在线记录（主键优先，可选按 ConnectionId 回退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <returns>在线实体</returns>
    /// <exception cref="TaktBusinessException">记录不存在时抛出</exception>
    private async Task<TaktOnline> ResolveOnlineForForceKickAsync(long onlineId, string? connectionId)
    {
        TaktOnline? online = null;
        if (onlineId > 0)
        {
            online = await _onlineRepository.GetByIdAsync(onlineId);
        }

        var normalizedConnectionId = connectionId?.Trim();
        if (online == null && !string.IsNullOrEmpty(normalizedConnectionId))
        {
            var matches = await _onlineRepository.GetListAsync(o => o.ConnectionId == normalizedConnectionId);
            online = matches.FirstOrDefault();
        }

        if (online == null)
        {
            throw new TaktBusinessException("在线用户不存在");
        }

        return online;
    }

    /// <summary>
    /// 解析当前强退操作者（HTTP 请求内有效；延迟任务须调度时传入）
    /// </summary>
    /// <returns>操作者 ID 与登录名</returns>
    private (long? UserId, string? UserName) ResolveKickOperator()
    {
        if (_userContext.IsAuthenticated
            && _userContext.UserId.HasValue
            && _userContext.UserId.Value > 0)
        {
            return (_userContext.UserId, _userContext.UserName);
        }

        return (null, null);
    }

    /// <summary>
    /// 强退通知落库并 SignalR 私信推送（失败仅记日志，不阻断强退）
    /// </summary>
    /// <param name="online">目标在线用户</param>
    /// <param name="messageContent">消息正文</param>
    /// <param name="messageGroup">消息分组 DictValue</param>
    /// <param name="fromUserId">操作者用户 ID</param>
    /// <param name="fromUserName">操作者登录名</param>
    /// <returns>任务</returns>
    private async Task TryPersistKickMessageAsync(
        TaktOnline online,
        string messageContent,
        string messageGroup,
        long? fromUserId,
        string? fromUserName)
    {
        try
        {
            await _messageService.Value.CreateAndSendOnlineKickMessageAsync(
                online,
                messageContent,
                messageGroup,
                fromUserId,
                fromUserName);
        }
        catch (Exception ex)
        {
            TaktLogger.Warning(
                ex,
                "强退消息落库/推送失败，用户: {UserName}, OnlineId: {OnlineId}",
                online.UserName,
                online.Id);
        }
    }

    /// <summary>
    /// 映射在线统计为 SignalR 推送载荷
    /// </summary>
    /// <param name="statistics">统计 DTO</param>
    /// <returns>推送载荷</returns>
    private static object ToOnlineStatisticsPayload(TaktOnlineStatisticsDto statistics)
    {
        return new
        {
            statistics.UserName,
            UserId = statistics.UserId?.ToString(),
            statistics.OnlineCount,
            statistics.CurrentDurationSeconds,
            statistics.TodayDurationSeconds,
            statistics.WeekTotalDurationSeconds,
            statistics.WeekAverageDurationSeconds,
            statistics.MonthDurationSeconds,
            statistics.MonthAverageDurationSeconds,
        };
    }

    /// <summary>
    /// 映射消息统计为 SignalR 推送载荷
    /// </summary>
    /// <param name="statistics">统计 DTO</param>
    /// <returns>推送载荷</returns>
    private static object ToMessageStatisticsPayload(TaktMessageStatisticsDto statistics)
    {
        return new
        {
            statistics.UserName,
            UserId = statistics.UserId?.ToString(),
            statistics.TotalCount,
            statistics.ReadCount,
            statistics.UnreadCount,
        };
    }
}

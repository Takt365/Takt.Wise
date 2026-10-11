// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktEcChangeHub.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知与执行监测 Hub（部门组/任务组、确认与进度上报）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Identity;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Services;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// 工程变更实时 Hub：ChangeNotification / ConfirmNotification / ReportProgress
/// </summary>
[Authorize]
public class TaktEcChangeHub : Hub
{
    private readonly ITaktEcChangeFlowService _ecChangeFlowService;
    private readonly ITaktUserContext _userContext;
    private readonly ITaktAuthService _authService;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcChangeHub(
        ITaktEcChangeFlowService ecChangeFlowService,
        ITaktUserContext userContext,
        ITaktAuthService authService)
    {
        _ecChangeFlowService = ecChangeFlowService;
        _userContext = userContext;
        _authService = authService;
    }

    /// <summary>
    /// 客户端连接时调用：写入在线用户记录、加入用户组并推送上线事件
    /// </summary>
    /// <returns>任务</returns>
    public override async Task OnConnectedAsync()
    {
        var prevPrincipal = TaktUserContext.HubInvocationPrincipal;
        TaktUserContext.HubInvocationPrincipal = Context.User;
        try
        {
            RequireResolvedLoginUser();
            var UserName = _userContext.UserName!.Trim();
            var companyCode = await ResolveCompanyCodeAsync();
            var connectionId = Context.ConnectionId ?? string.Empty;
            await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, UserName));
            await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.NotificationsGroup(companyCode));
            await base.OnConnectedAsync();
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 客户端断开连接时调用：更新离线状态、移出用户组并通知其他客户端
    /// </summary>
    /// <param name="exception">断开异常；正常断开时为 null</param>
    /// <returns>任务</returns>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var prevPrincipal = TaktUserContext.HubInvocationPrincipal;
        TaktUserContext.HubInvocationPrincipal = Context.User;
        try
        {
            RequireResolvedLoginUser();
            var UserName = _userContext.UserName!.Trim();
            var companyCode = await ResolveCompanyCodeAsync();
            var connectionId = Context.ConnectionId ?? string.Empty;
            await Groups.RemoveFromGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, UserName));
            await Groups.RemoveFromGroupAsync(connectionId, TaktSignalRGroupNames.NotificationsGroup(companyCode));
            await base.OnDisconnectedAsync(exception);
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 加入设变部门通知组（dept_xxx）
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>任务</returns>
    public async Task JoinDeptGroup(string deptCode)
    {
        RequireResolvedLoginUser();
        var companyCode = await ResolveCompanyCodeAsync();
        var group = TaktEcHelper.DeptGroup(companyCode, deptCode);
        await Groups.AddToGroupAsync(Context.ConnectionId!, group);
    }

    /// <summary>
    /// 离开设变部门通知组
    /// </summary>
    /// <param name="deptCode">部门编码</param>
    /// <returns>任务</returns>
    public async Task LeaveDeptGroup(string deptCode)
    {
        RequireResolvedLoginUser();
        var companyCode = await ResolveCompanyCodeAsync();
        var group = TaktEcHelper.DeptGroup(companyCode, deptCode);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId!, group);
    }

    /// <summary>
    /// 加入执行任务进度组（task_xxx）
    /// </summary>
    /// <param name="taskId">任务 ID（string 雪花 ID）</param>
    /// <returns>任务</returns>
    public async Task JoinTaskGroup(string taskId)
    {
        RequireResolvedLoginUser();
        if (!long.TryParse(taskId, out var parsedId) || parsedId <= 0)
        {
            throw new HubException("任务 ID 无效");
        }
        var companyCode = await ResolveCompanyCodeAsync();
        var group = TaktEcHelper.TaskGroup(companyCode, parsedId);
        await Groups.AddToGroupAsync(Context.ConnectionId!, group);
    }

    /// <summary>
    /// 确认变更通知（按投递记录 ID）
    /// </summary>
    /// <param name="deliveryId">投递记录 ID（string 雪花 ID）</param>
    /// <returns>任务</returns>
    public async Task ConfirmNotification(string deliveryId)
    {
        if (!long.TryParse(deliveryId, out var parsedId) || parsedId <= 0)
        {
            throw new HubException("投递记录 ID 无效");
        }
        await _ecChangeFlowService.ConfirmEcNotificationDeliveryAsync(new TaktEcNotificationConfirmDto
        {
            DeliveryId = parsedId
        });
        await Clients.Caller.SendAsync("NotificationConfirmAck", new { DeliveryId = deliveryId });
    }

    /// <summary>
    /// 确认变更通知（按通知单 + 部门）
    /// </summary>
    /// <param name="ecNotificationId">通知单 ID（string 雪花 ID）</param>
    /// <param name="deptCode">部门编码</param>
    /// <returns>任务</returns>
    public async Task ConfirmNotificationByDept(string ecNotificationId, string deptCode)
    {
        if (!long.TryParse(ecNotificationId, out var parsedNotificationId) || parsedNotificationId <= 0)
        {
            throw new HubException("通知单 ID 无效");
        }
        await _ecChangeFlowService.ConfirmEcNotificationDeliveryAsync(new TaktEcNotificationConfirmDto
        {
            EcNotificationId = parsedNotificationId,
            EcDeptGroupCode = deptCode
        });
        await Clients.Caller.SendAsync("NotificationConfirmAck", new
        {
            EcNotificationId = ecNotificationId,
            EcDeptGroupCode = deptCode
        });
    }

    /// <summary>
    /// 上报执行任务进度
    /// </summary>
    /// <param name="dto">进度 DTO</param>
    /// <returns>任务</returns>
    public async Task ReportProgress(TaktEcExecutionTaskProgressReportDto dto)
    {
        await _ecChangeFlowService.ReportEcExecutionTaskProgressAsync(dto);
        await Clients.Caller.SendAsync("TaskProgressAck", new { TaskId = dto.TaskId.ToString() });
    }

    /// <summary>
    /// 校验当前 Hub 调用已解析登录用户
    /// </summary>
    private void RequireResolvedLoginUser()
    {
        var (userId, UserName) = ResolveUserFromContext();
        if (!userId.HasValue || userId.Value <= 0 || string.IsNullOrWhiteSpace(UserName))
        {
            throw new HubException("无法解析当前登录用户，请重新登录后重试。");
        }
    }

    /// <summary>
    /// 解析 Hub 当前公司编码
    /// </summary>
    private async Task<string> ResolveCompanyCodeAsync()
    {
        RequireResolvedLoginUser();
        var (userId, UserName) = ResolveUserFromContext();
        return await TaktSignalRHubCompanyResolver.ResolveAsync(
            _userContext,
            _authService,
            userId!.Value,
            UserName!);
    }

    /// <summary>
    /// 从 Hub Claims 解析用户
    /// </summary>
    private (long? UserId, string? UserName) ResolveUserFromContext()
    {
        var principal = Context.User ?? TaktUserContext.HubInvocationPrincipal;
        return TaktUserContext.ResolveUserFromPrincipal(principal, _userContext.UserId, _userContext.UserName);
    }
}

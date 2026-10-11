// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktNotificationHub.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：通知 Hub，用于在线消息与广播推送
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Identity;
using Takt.Domain.Interfaces;
using Takt.Shared.Models.Foundation;
using Takt.Infrastructure.Services;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// 通知 Hub，用于在线消息与广播推送
/// </summary>
[Authorize]
public class TaktNotificationHub : Hub
{
    private readonly ITaktMessageService _messageService;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktUserContext _userContext;
    private readonly ITaktAuthService _authService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="messageService">在线消息服务</param>
    /// <param name="signalRDispatchService">SignalR 推送调度服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="authService">认证服务</param>
    public TaktNotificationHub(
        ITaktMessageService messageService,
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktUserContext userContext,
        ITaktAuthService authService)
    {
        _messageService = messageService;
        _signalRDispatchService = signalRDispatchService;
        _userContext = userContext;
        _authService = authService;
    }

    /// <summary>
    /// 客户端连接时调用：加入公司内用户组与广播组
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
            var userId = _userContext.UserId!.Value;
            var companyCode = await ResolveCompanyCodeAsync();
            var connectionId = Context.ConnectionId ?? string.Empty;
            var httpContext = Context.GetHttpContext();
            var (connectIp, connectLocation) = TaktHttpAuditHelper.ResolveClientIpAndLocation(httpContext);
            var connectTime = DateTime.Now;

            await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, UserName));
            await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.NotificationsGroup(companyCode));

            // OnlineMessage 仅由 ConnectHub 推送一次；此处只同步消息统计
            await _signalRDispatchService.PushMessageStatisticsToUserAsync(companyCode, UserName, userId);

            TaktSignalRLogging.LogHubConnected(
                nameof(TaktNotificationHub),
                connectionId,
                UserName,
                userId,
                companyCode,
                _userContext.TenantCode,
                connectIp,
                connectLocation);
            await base.OnConnectedAsync();
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 客户端断开连接时调用：移出公司内用户组与广播组
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

            TaktSignalRLogging.LogHubDisconnected(
                nameof(TaktNotificationHub),
                connectionId,
                UserName,
                _userContext.UserId,
                companyCode,
                _userContext.TenantCode,
                exception);
            await base.OnDisconnectedAsync(exception);
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 发送私信（持久化并实时推送）
    /// </summary>
    /// <param name="toUserName">接收者用户名</param>
    /// <param name="messageContent">消息内容</param>
    /// <param name="messageTitle">消息标题</param>
    /// <param name="messageType">消息类型</param>
    /// <param name="messageGroup">消息分组</param>
    /// <returns>任务</returns>
    public async Task SendMessage(
        string toUserName,
        string messageContent,
        string? messageTitle = null,
        string messageType = "text",
        string messageGroup = "message")
    {
        try
        {
            RequireResolvedLoginUser();
            var fromUserName = _userContext.UserName!.Trim();
            var fromUserId = _userContext.UserId!.Value;

            var created = await _messageService.CreateMessageAsync(new TaktMessageCreateDto
            {
                FromUserName = fromUserName,
                FromUserId = fromUserId,
                ToUserName = toUserName,
                MessageTitle = messageTitle ?? string.Empty,
                MessageContent = messageContent,
                MessageType = messageType,
                MessageGroup = messageGroup,
                ReadStatus = 0,
                SendTime = DateTime.Now,
            });

            await _messageService.SendMessageByIdAsync(created.MessageId);

            await Clients.Caller.SendAsync("MessageSent", new
            {
                ToUserName = toUserName,
                MessageId = created.MessageId.ToString(),
                SendTime = created.SendTime,
            });
        }
        catch (Exception ex)
        {
            if (ex is HubException)
            {
                throw;
            }

            TaktLogger.Error(ex, "发送私信失败");
            await Clients.Caller.SendAsync("Error", new { Message = "发送消息失败" });
        }
    }

    /// <summary>
    /// 广播消息（公司内推送）
    /// </summary>
    /// <param name="messageContent">消息内容</param>
    /// <param name="messageTitle">消息标题</param>
    /// <param name="messageType">消息类型</param>
    /// <param name="messageGroup">消息分组</param>
    /// <returns>任务</returns>
    public async Task BroadcastMessage(
        string messageContent,
        string? messageTitle = null,
        string messageType = "system",
        string messageGroup = "message")
    {
        try
        {
            RequireResolvedLoginUser();
            var fromUserName = _userContext.UserName!.Trim();
            var companyCode = await ResolveCompanyCodeAsync();
            var sendTime = DateTime.Now;

            await _signalRDispatchService.PushBroadcastMessageAsync(new TaktMessageBroadcastDto
            {
                CompanyCode = companyCode,
                FromUserName = fromUserName,
                MessageTitle = messageTitle ?? string.Empty,
                MessageContent = messageContent,
                MessageType = messageType,
                MessageGroup = messageGroup,
                SendTime = sendTime,
            }.Adapt<TaktSignalRBroadcastPush>());

            await Clients.Caller.SendAsync("MessageSent", new
            {
                ToUserName = "*",
                SendTime = sendTime,
            });
        }
        catch (Exception ex)
        {
            if (ex is HubException)
            {
                throw;
            }

            TaktLogger.Error(ex, "发送广播失败");
            await Clients.Caller.SendAsync("Error", new { Message = "发送广播消息失败" });
        }
    }

    /// <summary>
    /// 标记消息已读
    /// </summary>
    /// <param name="messageId">消息 ID</param>
    /// <returns>任务</returns>
    public async Task MarkAsRead(long messageId)
    {
        try
        {
            RequireResolvedLoginUser();
            var updated = await _messageService.MarkMessageReadAsync(new TaktMessageReadDto
            {
                MessageId = messageId,
                ReadStatus = 1,
            });

            await Clients.Caller.SendAsync("MessageRead", new
            {
                MessageId = messageId,
                ReadTime = updated.ReadTime ?? DateTime.Now,
            });

            var companyCode = await ResolveCompanyCodeAsync();
            var UserName = _userContext.UserName!.Trim();
            await _signalRDispatchService.PushMessageStatisticsToUserAsync(companyCode, UserName, _userContext.UserId);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "标记消息已读失败");
            await Clients.Caller.SendAsync("Error", new { Message = "标记消息已读失败" });
        }
    }

    /// <summary>
    /// 获取未读消息数量
    /// </summary>
    /// <returns>未读数量</returns>
    public async Task<int> GetUnreadCount()
    {
        RequireResolvedLoginUser();
        var UserName = _userContext.UserName!.Trim();
        return await _messageService.GetUnreadMessageCountAsync(UserName);
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
    /// 解析 Hub 当前公司编码（与 GET /me 一致）
    /// </summary>
    /// <returns>公司编码</returns>
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
    /// 从 Hub Claims 或用户上下文解析用户
    /// </summary>
    /// <returns>用户 ID 与用户名</returns>
    private (long? UserId, string? UserName) ResolveUserFromContext()
    {
        var principal = Context.User ?? TaktUserContext.HubInvocationPrincipal;
        return TaktUserContext.ResolveUserFromPrincipal(principal, _userContext.UserId, _userContext.UserName);
    }
}

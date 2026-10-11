// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktConnectHub.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：连接 Hub，用于管理在线用户连接
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Takt.Application.Dtos.Foundation;
using Takt.Application.Services.Foundation;
using Takt.Application.Services.Identity;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Infrastructure.Services;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// 连接 Hub，用于管理在线用户连接
/// </summary>
[Authorize]
public class TaktConnectHub : Hub
{
    private readonly ITaktOnlineService _onlineService;
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;
    private readonly ITaktSignalRDispatchService _signalRDispatchService;
    private readonly ITaktUserContext _userContext;
    private readonly ITaktAuthService _authService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineService">在线用户服务</param>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="signalRDispatchService">SignalR 推送调度服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="authService">认证服务</param>
    public TaktConnectHub(
        ITaktOnlineService onlineService,
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktSignalRDispatchService signalRDispatchService,
        ITaktUserContext userContext,
        ITaktAuthService authService)
    {
        _onlineService = onlineService;
        _onlineRepository = onlineRepository;
        _signalRDispatchService = signalRDispatchService;
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
            var connectionId = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            RequireResolvedLoginUser();

            var UserName = _userContext.UserName!.Trim();
            var userId = _userContext.UserId!.Value;
            var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
                _userContext,
                _authService,
                userId,
                UserName);
            var client = TaktHttpAuditHelper.ResolveClientLogContext(httpContext);
            var connectTime = DateTime.Now;

            await _onlineService.RegisterOnlineSessionAsync(new TaktOnlineCreateDto
            {
                ConnectionId = connectionId ?? string.Empty,
                UserName = UserName,
                UserId = userId,
                OnlineStatus = 0,
                ConnectIp = client.Ip,
                ConnectLocation = client.Location,
                UserAgent = client.UserAgent,
                BrowserType = client.Browser,
                OperatingSystem = client.OperatingSystem,
                DeviceType = client.DeviceType,
                ConnectTime = connectTime,
            });

            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.AddToGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, UserName));
            }

            var userIdStr = userId.ToString();
            await Clients.Caller.SendAsync("OnlineMessage", new
            {
                Message = $"欢迎 {UserName} 上线！连接成功，当前时间：{connectTime:yyyy-MM-dd HH:mm:ss}",
                UserName = UserName,
                UserId = userIdStr,
                ConnectTime = connectTime,
                ConnectIp = client.Ip,
                ConnectLocation = client.Location,
                DeviceType = client.DeviceType,
            });

            await Clients.Others.SendAsync("UserConnected", new
            {
                UserName = UserName,
                UserId = userIdStr,
                ConnectTime = connectTime,
                ConnectIp = client.Ip,
                ConnectLocation = client.Location,
            });

            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, UserName, userId);

            TaktSignalRLogging.LogHubConnected(
                nameof(TaktConnectHub),
                connectionId,
                UserName,
                userId,
                companyCode,
                _userContext.TenantCode,
                client.Ip,
                client.Location);
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
            var connectionId = Context.ConnectionId;
            RequireResolvedLoginUser();
            var UserName = _userContext.UserName!.Trim();
            var userId = _userContext.UserId!.Value;
            var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
                _userContext,
                _authService,
                userId,
                UserName);

            var httpContext = Context.GetHttpContext();
            var disconnectIp = TaktLocationHelper.ResolveClientIp(httpContext);

            var online = await _onlineRepository.FirstAsync(o => o.ConnectionId == connectionId);
            string? disconnectLocation = null;
            if (online != null)
            {
                var disconnectTime = DateTime.Now;
                if (string.IsNullOrWhiteSpace(online.ConnectIp) && !string.IsNullOrWhiteSpace(disconnectIp))
                {
                    online.ConnectIp = disconnectIp;
                }

                online.ConnectLocation = TaktHttpAuditHelper.ResolveLocationFromIp(
                    online.ConnectIp,
                    online.ConnectLocation);
                disconnectLocation = online.ConnectLocation;
                await _onlineRepository.UpdateAsync(online);
                await _onlineService.CloseOnlineSessionByConnectionIdAsync(connectionId ?? string.Empty, disconnectTime);
            }

            if (!string.IsNullOrEmpty(connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, TaktSignalRGroupNames.UserGroup(companyCode, UserName));
            }

            await Clients.Others.SendAsync("UserDisconnected", new
            {
                UserName = UserName,
                DisconnectTime = DateTime.Now,
            });

            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, UserName, _userContext.UserId);

            TaktSignalRLogging.LogHubDisconnected(
                nameof(TaktConnectHub),
                connectionId,
                UserName,
                _userContext.UserId,
                companyCode,
                _userContext.TenantCode,
                exception,
                online?.ConnectIp ?? disconnectIp,
                disconnectLocation);
            await base.OnDisconnectedAsync(exception);
        }
        finally
        {
            TaktUserContext.HubInvocationPrincipal = prevPrincipal;
        }
    }

    /// <summary>
    /// 心跳更新
    /// </summary>
    /// <returns>任务</returns>
    public async Task Heartbeat()
    {
        RequireResolvedLoginUser();
        var connectionId = Context.ConnectionId;
        if (!string.IsNullOrWhiteSpace(connectionId))
        {
            await _onlineService.RefreshOnlineConnectionDurationAsync(connectionId, DateTime.Now);
        }

        var UserName = _userContext.UserName!.Trim();
        var userId = _userContext.UserId!.Value;
        var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
            _userContext,
            _authService,
            userId,
            UserName);
        if (!string.IsNullOrWhiteSpace(companyCode) && !string.IsNullOrWhiteSpace(UserName))
        {
            await _signalRDispatchService.PushOnlineStatisticsToUserAsync(companyCode, UserName, _userContext.UserId);
        }
    }

    /// <summary>
    /// 获取在线用户列表
    /// </summary>
    /// <returns>在线用户列表</returns>
    public async Task<List<object>> GetOnlineUsers()
    {
        RequireResolvedLoginUser();
        var UserName = _userContext.UserName!.Trim();
        var userId = _userContext.UserId!.Value;
        var companyCode = await TaktSignalRHubCompanyResolver.ResolveAsync(
            _userContext,
            _authService,
            userId,
            UserName);
        var onlines = await _onlineRepository.GetListAsync(
            o => o.OnlineStatus == 0 && o.CompanyCode == companyCode);
        return onlines.Select(u => (object)new
        {
            UserName = u.UserName,
            UserId = u.UserId,
            ConnectTime = u.ConnectTime,
            LastActiveTime = u.LastActiveTime,
            ConnectIp = u.ConnectIp,
            ConnectLocation = TaktHttpAuditHelper.ResolveLocationFromIp(u.ConnectIp, u.ConnectLocation),
        }).ToList();
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
    /// 从 Hub Claims 或用户上下文解析用户
    /// </summary>
    /// <returns>用户 ID 与用户名</returns>
    private (long? UserId, string? UserName) ResolveUserFromContext()
    {
        var principal = Context.User ?? TaktUserContext.HubInvocationPrincipal;
        return TaktUserContext.ResolveUserFromPrincipal(principal, _userContext.UserId, _userContext.UserName);
    }
}

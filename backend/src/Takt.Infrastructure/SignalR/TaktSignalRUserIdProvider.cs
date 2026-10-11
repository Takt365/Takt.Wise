// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRUserIdProvider.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR IUserIdProvider（与 JWT sub 对齐，支持 Clients.User 定向推送）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.SignalR;
using Takt.Infrastructure.Services;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR 用户 ID 提供器（映射 JWT sub / NameIdentifier，供 IHubClients{T}.User 使用）
/// </summary>
public sealed class TaktSignalRUserIdProvider : IUserIdProvider
{
    /// <summary>
    /// 从 Hub 连接主体解析用户 ID 字符串
    /// </summary>
    /// <param name="connection">Hub 连接上下文</param>
    /// <returns>用户 ID 字符串；无法解析时返回 null</returns>
    public string? GetUserId(HubConnectionContext connection)
    {
        ArgumentNullException.ThrowIfNull(connection);
        var userId = TaktUserContext.TryResolveUserId(connection.User);
        return userId?.ToString();
    }
}

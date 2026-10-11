// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktLoginTicketService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录票据（避免 verify-password 与 signin 重复验密）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Identity;

/// <summary>
/// 登录票据服务（短时一次性，供会话 signin 复用已验密结果）
/// </summary>
public interface ITaktLoginTicketService
{
    /// <summary>
    /// 创建登录票据
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>票据字符串</returns>
    Task<string> CreateLoginTicketAsync(
        long userId,
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 消费登录票据（一次性）；租户与用户名须与签发时一致
    /// </summary>
    /// <param name="ticket">票据</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>用户 ID；无效或过期返回 null</returns>
    Task<long?> ConsumeLoginTicketAsync(
        string ticket,
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default);
}

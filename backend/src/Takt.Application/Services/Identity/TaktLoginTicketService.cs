// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：TaktLoginTicketService.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录票据缓存实现（ITaktCacheService，默认 3 分钟有效）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Identity;
using Takt.Domain.Interfaces;

namespace Takt.Application.Services.Identity;

/// <summary>
/// 登录票据服务实现
/// </summary>
public class TaktLoginTicketService : ITaktLoginTicketService
{
    private const string CacheKeyPrefix = "takt:login-ticket:";
    private static readonly TimeSpan TicketLifetime = TimeSpan.FromMinutes(3);

    private readonly ITaktCacheService _cacheService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="cacheService">统一缓存服务</param>
    public TaktLoginTicketService(ITaktCacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// 创建登录票据（缓存默认 3 分钟有效）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>票据字符串</returns>
    public async Task<string> CreateLoginTicketAsync(
        long userId,
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default)
    {
        var ticket = Guid.NewGuid().ToString("N");
        var payload = new TaktLoginTicketPayload
        {
            UserId = userId,
            TenantCode = tenantCode.Trim(),
            UserName = UserName.Trim().ToLowerInvariant(),
        };

        await _cacheService.SetAsync(
            CacheKeyPrefix + ticket,
            payload,
            TicketLifetime,
            cancellationToken);

        return ticket;
    }

    /// <summary>
    /// 消费登录票据（一次性）；租户与用户名须与签发时一致
    /// </summary>
    /// <param name="ticket">票据</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>用户 ID；无效或过期返回 null</returns>
    public async Task<long?> ConsumeLoginTicketAsync(
        string ticket,
        string tenantCode,
        string UserName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            return null;
        }

        var key = CacheKeyPrefix + ticket.Trim();
        var payload = await _cacheService.GetAsync<TaktLoginTicketPayload>(key, cancellationToken);
        if (payload == null)
        {
            return null;
        }

        var trimmedTenant = tenantCode.Trim();
        var normalizedUserName = UserName.Trim().ToLowerInvariant();
        if (!string.Equals(payload.TenantCode, trimmedTenant, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(payload.UserName, normalizedUserName, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        await _cacheService.RemoveAsync(key, cancellationToken);
        return payload.UserId;
    }
}

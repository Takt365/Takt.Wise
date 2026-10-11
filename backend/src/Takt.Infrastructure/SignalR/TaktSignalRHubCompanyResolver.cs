// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.SignalR
// 文件名称：TaktSignalRHubCompanyResolver.cs
// 创建时间：2026-06-04
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR Hub 公司编码解析（与 GET /me 及 HTTP 请求头策略一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.SignalR;
using Takt.Application.Services.Identity;
using Takt.Domain.Interfaces;

namespace Takt.Infrastructure.SignalR;

/// <summary>
/// SignalR Hub 公司编码解析器
/// </summary>
public static class TaktSignalRHubCompanyResolver
{
    /// <summary>
    /// 解析 Hub 当前连接应使用的公司编码
    /// </summary>
    /// <param name="userContext">用户上下文</param>
    /// <param name="authService">认证服务</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    /// <returns>公司编码</returns>
    public static async Task<string> ResolveAsync(
        ITaktUserContext userContext,
        ITaktAuthService authService,
        long userId,
        string UserName)
    {
        var tenantCode = userContext.TenantCode;
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new HubException("无法解析租户编码，请重新登录后重试。");
        }

        var companyCode = await authService.ResolveCurrentActiveCompanyCodeAsync(
            userId,
            tenantCode.Trim(),
            UserName.Trim());
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            throw new HubException("无法解析公司编码，请确认账号已绑定可访问公司。");
        }

        return companyCode.Trim();
    }
}

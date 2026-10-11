// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzAmbientHttpContext.cs
// 创建时间：2026-07-16
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 后台执行前注入租户/公司/触发用户 HTTP 上下文（须在解析 Scoped 服务之前调用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Takt.Infrastructure.Services;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz 后台环境 HTTP 上下文（供 ITaktUserContext / 消息落库 / 未读统计读取）
/// </summary>
public static class TaktQuartzAmbientHttpContext
{
    /// <summary>
    /// 注入租户/公司/触发用户到 IHttpContextAccessor（须在 CreateScope 后、GetRequiredService 业务服务前调用）
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantOptions">租户请求头配置</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="triggerUserName">触发用户名；空则用系统发送者</param>
    /// <param name="triggerUserId">触发用户 Id；空则用系统审计用户</param>
    public static void Configure(
        IHttpContextAccessor httpContextAccessor,
        TaktTenantContextOptions tenantOptions,
        string? tenantCode,
        string? companyCode,
        string? triggerUserName,
        long? triggerUserId = null)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(tenantOptions);
        var tenant = tenantCode?.Trim() ?? string.Empty;
        var company = companyCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(tenant) || string.IsNullOrWhiteSpace(company))
        {
            return;
        }

        var UserName = string.IsNullOrWhiteSpace(triggerUserName)
            ? TaktQuartzConstants.SystemSenderUserName
            : triggerUserName.Trim();
        var userId = triggerUserId is > 0
            ? triggerUserId.Value
            : TaktConstants.SystemAuditUser.Id;
        var httpContext = new DefaultHttpContext();
        TaktUserContext.ApplyRequestTenantCompanyHeaders(httpContext, tenant, company, tenantOptions);
        var claims = new List<Claim>
        {
            new("sub", userId.ToString(CultureInfo.InvariantCulture)),
            new(TaktClaimNames.PreferredUsername, UserName),
            new(TaktClaimNames.TenantCode, tenant),
            new(TaktClaimNames.CompanyCode, company),
        };
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "QuartzJob"));
        httpContextAccessor.HttpContext = httpContext;
    }
}

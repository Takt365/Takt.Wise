// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktUserContext.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：租户/公司/用户上下文（HTTP、Hub、Claims 全局解析入口）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Takt.Domain.Interfaces;
using Takt.Shared.Constants;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktUserContext 实现；静态方法供中间件、SqlSugar 审计等无 DI 场景复用
/// </summary>
public class TaktUserContext : ITaktUserContext
{
    /// <summary>
    /// SignalR Hub 调用链上的用户主体（WebSocket 无 HTTP 上下文时使用）
    /// </summary>
    private static readonly AsyncLocal<ClaimsPrincipal?> _hubInvocationPrincipal = new();

    /// <summary>
    /// HTTP 上下文访问器
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 租户/公司请求头配置
    /// </summary>
    private readonly TaktTenantContextOptions _tenantOptions;

    /// <summary>
    /// SignalR Hub 生命周期或客户端调用时写入的 ClaimsPrincipal
    /// </summary>
    public static ClaimsPrincipal? HubInvocationPrincipal
    {
        get => _hubInvocationPrincipal.Value;
        set => _hubInvocationPrincipal.Value = value;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="httpContextAccessor">HTTP 上下文访问器</param>
    /// <param name="tenantOptions">租户上下文配置（请求头名称）</param>
    public TaktUserContext(IHttpContextAccessor httpContextAccessor, IOptions<TaktTenantContextOptions> tenantOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _tenantOptions = tenantOptions.Value;
    }

    /// <summary>
    /// 当前用户 ID
    /// </summary>
    public long? UserId => TryResolveUserId(ResolvePrincipal(_httpContextAccessor.HttpContext));

    /// <summary>
    /// 当前用户名
    /// </summary>
    public string? UserName => TryResolveUserName(ResolvePrincipal(_httpContextAccessor.HttpContext));

    /// <summary>
    /// 当前租户编码（请求头 → Claim → Query tenantCode）
    /// </summary>
    public string? TenantCode => TryResolveTenantCode(_httpContextAccessor.HttpContext, _tenantOptions.TenantHeaderName);

    /// <summary>
    /// 当前公司编码（请求头 → Claim；不含 appsettings 默认公司回退）
    /// </summary>
    public string? CompanyCode => RequestCompanyCode;

    /// <summary>
    /// 请求头或 JWT 中的公司编码（未传则为 null）
    /// </summary>
    public string? RequestCompanyCode =>
        TryResolveCompanyCode(_httpContextAccessor.HttpContext, _tenantOptions.CompanyHeaderName);

    /// <summary>
    /// 是否已认证（HTTP 或 Hub 用户主体）
    /// </summary>
    public bool IsAuthenticated => ResolvePrincipal(_httpContextAccessor.HttpContext)?.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// 从 ClaimsPrincipal 解析租户编码
    /// </summary>
    /// <param name="principal">用户主体</param>
    /// <returns>租户编码；无法解析时返回 null</returns>
    public static string? TryResolveTenantCodeFromPrincipal(ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirst(TaktClaimNames.TenantCode)?.Value;
        return string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
    }

    /// <summary>
    /// 从 ClaimsPrincipal 解析公司编码
    /// </summary>
    /// <param name="principal">用户主体</param>
    /// <returns>公司编码；无法解析时返回 null</returns>
    public static string? TryResolveCompanyCodeFromPrincipal(ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirst(TaktClaimNames.CompanyCode)?.Value;
        return string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
    }

    /// <summary>
    /// 从 HTTP 或 Hub 调用链解析租户编码（Header → Claim → Query tenantCode）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="tenantHeaderName">租户请求头名；默认 X-Tenant-Code</param>
    /// <returns>租户编码；无法解析时返回 null</returns>
    public static string? TryResolveTenantCode(HttpContext? httpContext, string? tenantHeaderName = null)
    {
        if (httpContext == null)
        {
            return null;
        }

        var headerName = string.IsNullOrWhiteSpace(tenantHeaderName)
            ? TaktHttpHeaderNames.TenantCode
            : tenantHeaderName.Trim();
        var fromHeader = httpContext.Request.Headers[headerName].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fromHeader))
        {
            return fromHeader.Trim();
        }

        var fromClaim = TryResolveTenantCodeFromPrincipal(ResolvePrincipal(httpContext));
        if (!string.IsNullOrWhiteSpace(fromClaim))
        {
            return fromClaim;
        }

        var fromQuery = httpContext.Request.Query[TaktHttpQueryNames.TenantCode].FirstOrDefault();
        return string.IsNullOrWhiteSpace(fromQuery) ? null : fromQuery.Trim();
    }

    /// <summary>
    /// 从 HTTP 或 Hub 调用链解析公司编码（Header → Claim）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="companyHeaderName">公司请求头名；默认 X-Company-Code</param>
    /// <returns>公司编码；无法解析时返回 null</returns>
    public static string? TryResolveCompanyCode(HttpContext? httpContext, string? companyHeaderName = null)
    {
        if (httpContext == null)
        {
            return null;
        }

        var headerName = string.IsNullOrWhiteSpace(companyHeaderName)
            ? TaktHttpHeaderNames.CompanyCode
            : companyHeaderName.Trim();
        var fromHeader = httpContext.Request.Headers[headerName].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fromHeader))
        {
            return fromHeader.Trim();
        }

        return TryResolveCompanyCodeFromPrincipal(ResolvePrincipal(httpContext));
    }

    /// <summary>
    /// 将租户/公司写入当前请求头（登录成功后供仓储过滤；公司为空时移除公司头）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="options">租户上下文配置</param>
    public static void ApplyRequestTenantCompanyHeaders(
        HttpContext httpContext,
        string tenantCode,
        string? companyCode,
        TaktTenantContextOptions options)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentNullException.ThrowIfNull(options);

        httpContext.Request.Headers[options.TenantHeaderName] = tenantCode.Trim();
        if (string.IsNullOrWhiteSpace(companyCode))
        {
            httpContext.Request.Headers.Remove(options.CompanyHeaderName);
            return;
        }

        httpContext.Request.Headers[options.CompanyHeaderName] = companyCode.Trim();
    }

    /// <summary>
    /// 从 ClaimsPrincipal 解析用户 ID（sub / NameIdentifier）
    /// </summary>
    /// <param name="principal">用户主体</param>
    /// <returns>用户 ID；无法解析时返回 null</returns>
    public static long? TryResolveUserId(ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            return null;
        }

        var raw = principal.FindFirst("sub")?.Value
            ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(raw, out var userId) && userId > 0 ? userId : null;
    }

    /// <summary>
    /// 从 HTTP 或 Hub 调用链解析用户 ID（Claims → Items 暂存，不依赖 IsAuthenticated）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>用户 ID；无效时返回 null</returns>
    public static long? TryResolveUserId(HttpContext? httpContext)
    {
        var fromPrincipal = TryResolveUserId(ResolvePrincipal(httpContext));
        if (fromPrincipal is > 0)
        {
            return fromPrincipal;
        }

        return TryResolveStashedUserId(httpContext);
    }

    /// <summary>
    /// 从 ClaimsPrincipal 解析用户名（preferred_username / name / Identity.Name）
    /// </summary>
    /// <param name="principal">用户主体</param>
    /// <returns>用户名；无法解析时返回 null</returns>
    public static string? TryResolveUserName(ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            return null;
        }

        var name = principal.FindFirst(TaktClaimNames.PreferredUsername)?.Value
            ?? principal.Identity?.Name
            ?? principal.FindFirst("name")?.Value
            ?? principal.FindFirst(ClaimTypes.Name)?.Value
            ?? principal.FindFirst("unique_name")?.Value;
        return string.IsNullOrWhiteSpace(name) ? null : name.Trim();
    }

    /// <summary>
    /// 从 HTTP 或 Hub 调用链解析审计用登录名（Claims → Items 暂存；无法解析时为 unknown）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>登录名；无法解析时返回 TaktConstants.AuditUserName.Unknown</returns>
    public static string ResolveAuditUserName(HttpContext? httpContext)
    {
        var fromPrincipal = TryResolveUserName(ResolvePrincipal(httpContext));
        if (!string.IsNullOrWhiteSpace(fromPrincipal))
        {
            return fromPrincipal;
        }

        var stashed = TryResolveStashedUserName(httpContext);
        if (!string.IsNullOrWhiteSpace(stashed))
        {
            return stashed;
        }

        return TaktConstants.AuditUserName.Unknown;
    }

    /// <summary>
    /// 解析审计 Remark：Items 暂存优先；用户名为 unknown 且无暂存时用默认文案
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>Remark；有上下文且无需说明时为 null</returns>
    public static string? ResolveAuditContextRemark(HttpContext? httpContext)
    {
        var stashed = TryResolveStashedAuditContextRemark(httpContext);
        if (!string.IsNullOrWhiteSpace(stashed))
        {
            return stashed;
        }

        var UserName = ResolveAuditUserName(httpContext);
        if (string.Equals(UserName, TaktConstants.AuditUserName.Unknown, StringComparison.Ordinal))
        {
            return TaktAuditContextRemarks.DefaultUnknownOperator;
        }

        return null;
    }

    /// <summary>
    /// 将操作人写入 HttpContext.Items（登出清 Cookie 后差异/操作日志仍可读）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">登录名</param>
    /// <param name="contextRemark">审计 Remark（如登出时用户名未知的原因）</param>
    public static void StashAuditOperator(
        HttpContext? httpContext,
        long? userId,
        string? UserName,
        string? contextRemark = null)
    {
        if (httpContext == null)
        {
            return;
        }

        if (userId is > 0)
        {
            httpContext.Items[TaktHttpContextItemKeys.AuditOperatorUserId] = userId.Value;
        }

        var normalizedUserName = UserName?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedUserName))
        {
            httpContext.Items[TaktHttpContextItemKeys.AuditOperatorUserName] = normalizedUserName;
        }

        var normalizedRemark = contextRemark?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedRemark))
        {
            httpContext.Items[TaktHttpContextItemKeys.AuditContextRemark] = normalizedRemark;
        }
    }

    /// <summary>
    /// 从 Hub Claims 与用户上下文回退解析用户
    /// </summary>
    /// <param name="principal">Hub 或 HTTP 用户主体</param>
    /// <param name="fallbackUserId">回退用户 ID</param>
    /// <param name="fallbackUserName">回退用户名</param>
    /// <returns>用户 ID 与用户名</returns>
    public static (long? UserId, string? UserName) ResolveUserFromPrincipal(
        ClaimsPrincipal? principal,
        long? fallbackUserId = null,
        string? fallbackUserName = null)
    {
        var userId = TryResolveUserId(principal) ?? fallbackUserId;
        var UserName = TryResolveUserName(principal) ?? fallbackUserName;
        return (userId, UserName);
    }

    /// <summary>
    /// 解析当前 HTTP 或 Hub 调用的用户主体（Hub AsyncLocal 优先；有 sub/用户名即视为有效，不要求 IsAuthenticated）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文；可为 null</param>
    /// <returns>用户主体；无上下文时返回 null</returns>
    public static ClaimsPrincipal? ResolvePrincipal(HttpContext? httpContext)
    {
        var hubPrincipal = HubInvocationPrincipal;
        if (HasResolvableIdentity(hubPrincipal))
        {
            return hubPrincipal;
        }

        var httpPrincipal = httpContext?.User;
        if (HasResolvableIdentity(httpPrincipal))
        {
            return httpPrincipal;
        }

        return hubPrincipal ?? httpPrincipal;
    }

    /// <summary>
    /// 从 Items 读取登出等场景暂存的操作人 ID
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>用户 ID；无暂存时返回 null</returns>
    private static long? TryResolveStashedUserId(HttpContext? httpContext)
    {
        if (httpContext?.Items.TryGetValue(TaktHttpContextItemKeys.AuditOperatorUserId, out var raw) != true
            || raw == null)
        {
            return null;
        }

        return raw switch
        {
            long id when id > 0 => id,
            int intId when intId > 0 => intId,
            string text when long.TryParse(text, out var parsed) && parsed > 0 => parsed,
            _ => null,
        };
    }

    /// <summary>
    /// 从 Items 读取登出等场景暂存的操作人登录名
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>登录名；无暂存时返回 null</returns>
    private static string? TryResolveStashedUserName(HttpContext? httpContext)
    {
        if (httpContext?.Items.TryGetValue(TaktHttpContextItemKeys.AuditOperatorUserName, out var raw) != true)
        {
            return null;
        }

        var name = raw?.ToString();
        return string.IsNullOrWhiteSpace(name) ? null : name.Trim();
    }

    /// <summary>
    /// 从 Items 读取暂存的审计 Remark
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <returns>Remark；无暂存时返回 null</returns>
    private static string? TryResolveStashedAuditContextRemark(HttpContext? httpContext)
    {
        if (httpContext?.Items.TryGetValue(TaktHttpContextItemKeys.AuditContextRemark, out var raw) != true)
        {
            return null;
        }

        var remark = raw?.ToString();
        return string.IsNullOrWhiteSpace(remark) ? null : remark.Trim();
    }

    /// <summary>
    /// 主体是否携带可解析的用户标识（已认证或存在 sub/用户名 Claim）
    /// </summary>
    /// <param name="principal">用户主体</param>
    /// <returns>可解析时为 true</returns>
    private static bool HasResolvableIdentity(ClaimsPrincipal? principal)
    {
        if (principal == null)
        {
            return false;
        }

        if (principal.Identity?.IsAuthenticated == true)
        {
            return true;
        }

        return TryResolveUserId(principal) is > 0
            || !string.IsNullOrWhiteSpace(TryResolveUserName(principal));
    }
}

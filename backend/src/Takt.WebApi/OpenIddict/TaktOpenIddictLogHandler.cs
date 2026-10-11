// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.OpenIddict
// 文件名称：TaktOpenIddictLogHandler.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict 运行时处理（声明目的地、用户主体、登录日志）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Takt.Application.Services.Identity;
using Takt.Shared.Enums;
using Takt.WebApi.Logging;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Takt.WebApi.OpenIddict;

/// <summary>
/// OpenIddict 运行时处理器（主体构建、声明目的地、登录日志）
/// </summary>
public sealed class TaktOpenIddictLogHandler
{
    private readonly ITaktAuthService _authService;
    private readonly ITaktAuthLoginLogHandler _authLoginLogHandler;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="authService">身份认证服务</param>
    /// <param name="authLoginLogHandler">认证登录统一日志处理器</param>
    public TaktOpenIddictLogHandler(
        ITaktAuthService authService,
        ITaktAuthLoginLogHandler authLoginLogHandler)
    {
        _authService = authService;
        _authLoginLogHandler = authLoginLogHandler;
    }

    #region 用户主体

    /// <summary>
    /// 创建 Cookie 会话与令牌签发用的用户主体
    /// 写入 Subject/Name、角色、tenant_code、company_code 声明并设置 AccessToken 目的地
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="tenantCode">租户编码（写入 tenant_code 声明）</param>
    /// <param name="companyCode">公司编码（写入 company_code 声明）</param>
    /// <param name="scopes">请求的 scope（可为空）</param>
    /// <returns>含角色与租户/公司声明的 ClaimsPrincipal</returns>
    public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(
        long userId,
        string UserName,
        string tenantCode,
        string companyCode,
        IEnumerable<string>? scopes = null)
    {
        var roles = await _authService.GetUserRoleCodesAsync(userId, tenantCode);

        var identity = new ClaimsIdentity(
            TaktAuthCookieDefaults.AuthenticationScheme,
            Claims.Name,
            Claims.Role);

        identity.SetClaim(Claims.Subject, userId.ToString());
        identity.SetClaim(Claims.Name, UserName);
        identity.SetClaim(Claims.PreferredUsername, UserName);

        foreach (var role in roles)
        {
            var roleClaim = new Claim(Claims.Role, role);
            roleClaim.SetDestinations(Destinations.AccessToken);
            identity.AddClaim(roleClaim);
        }

        var tenantClaim = new Claim("tenant_code", tenantCode);
        tenantClaim.SetDestinations(Destinations.AccessToken);
        identity.AddClaim(tenantClaim);

        var companyClaim = new Claim("company_code", companyCode);
        companyClaim.SetDestinations(Destinations.AccessToken);
        identity.AddClaim(companyClaim);

        var principal = new ClaimsPrincipal(identity);
        if (scopes != null)
        {
            principal.SetScopes(scopes);
        }

        ApplyClaimDestinations(principal);
        return principal;
    }

    /// <summary>
    /// 将 Cookie 会话主体转换为 OpenIddict 授权码/令牌签发主体
    /// </summary>
    /// <param name="cookiePrincipal">Cookie 认证主体</param>
    /// <param name="scopes">授权请求 scope</param>
    /// <returns>OpenIddict 签发用主体</returns>
    public ClaimsPrincipal CreateOpenIddictPrincipal(ClaimsPrincipal cookiePrincipal, IEnumerable<string> scopes)
    {
        var identity = new ClaimsIdentity(
            cookiePrincipal.Claims,
            TokenValidationParameters.DefaultAuthenticationType,
            Claims.Name,
            Claims.Role);

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(scopes);
        ApplyClaimDestinations(principal);
        return principal;
    }

    #endregion

    #region 登录日志

    /// <summary>
    /// 写入认证登录日志（委托 ITaktAuthLoginLogHandler）
    /// </summary>
    /// <param name="httpContext">HTTP 上下文</param>
    /// <param name="phase">流程阶段（TaktAuthLoginPhases）</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名或 client_id</param>
    /// <param name="loginType">登录方式</param>
    /// <param name="loginResult">登录结果</param>
    /// <param name="loginMessage">结果说明</param>
    /// <param name="userId">用户 ID（可选）</param>
    /// <param name="elapsedMs">耗时毫秒（可选）</param>
    public Task CreateLoginLogAsync(
        HttpContext httpContext,
        string phase,
        string tenantCode,
        string? companyCode,
        string UserName,
        string loginType,
        string loginResult,
        string? loginMessage = null,
        long? userId = null,
        long? elapsedMs = null)
    {
        return _authLoginLogHandler.WriteAsync(
            httpContext,
            new TaktAuthLoginLogWriteRequest
            {
                Phase = phase,
                LoginType = loginType,
                LoginResult = loginResult,
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                UserName = UserName,
                Message = loginMessage,
                UserId = userId,
                ElapsedMs = elapsedMs,
            });
    }

    #endregion

    #region 声明目的地（静态）

    /// <summary>
    /// 根据 scope 与声明类型决定声明应写入的令牌类型
    /// </summary>
    /// <param name="claim">当前声明</param>
    /// <returns>目标令牌类型集合</returns>
    public static IEnumerable<string> GetClaimDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or Claims.PreferredUsername
                => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            Claims.Email when claim.Subject!.HasScope(Scopes.Email)
                => new[] { Destinations.AccessToken, Destinations.IdentityToken },

            Claims.Role
                => new[] { Destinations.AccessToken },

            "tenant_code" or "company_code"
                => new[] { Destinations.AccessToken },

            _ => new[] { Destinations.AccessToken }
        };
    }

    /// <summary>
    /// 为 Principal 内全部声明设置目的地
    /// </summary>
    /// <param name="principal">待签发的用户主体</param>
    public static void ApplyClaimDestinations(ClaimsPrincipal principal)
    {
        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(GetClaimDestinations(claim));
        }
    }

    #endregion
}

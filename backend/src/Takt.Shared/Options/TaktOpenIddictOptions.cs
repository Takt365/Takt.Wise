// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktOpenIddictOptions.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：OpenIddict OAuth2/OIDC 服务端配置（Authorization Code + PKCE）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// OpenIddict 服务端配置
/// </summary>
public class TaktOpenIddictOptions
{
    public const string SectionName = "OpenIddict";

    /// <summary>
    /// 颁发者 URI（须与对外访问地址一致）
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// SPA 公共客户端 ID（Authorization Code + PKCE）
    /// </summary>
    public string SpaClientId { get; set; } = null!;

    /// <summary>
    /// SPA 授权回调地址列表
    /// </summary>
    public string[] SpaRedirectUris { get; set; } = null!;

    /// <summary>
    /// SPA 登出后跳转地址
    /// </summary>
    public string[] SpaPostLogoutRedirectUris { get; set; } = null!;

    /// <summary>
    /// 未登录时重定向到前端登录页（完整 URL，无查询串）
    /// </summary>
    public string FrontendLoginUrl { get; set; } = null!;

    /// <summary>
    /// Access Token 有效期
    /// </summary>
    public TimeSpan AccessTokenLifetime { get; set; }

    /// <summary>
    /// Refresh Token 有效期
    /// </summary>
    public TimeSpan RefreshTokenLifetime { get; set; }

    /// <summary>
    /// API 资源标识（audience）
    /// </summary>
    public string ApiAudience { get; set; } = null!;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer))
        {
            throw new InvalidOperationException($"{SectionName}:Issuer 不能为空");
        }

        if (string.IsNullOrWhiteSpace(SpaClientId))
        {
            throw new InvalidOperationException($"{SectionName}:SpaClientId 不能为空");
        }

        if (SpaRedirectUris == null || SpaRedirectUris.Length == 0)
        {
            throw new InvalidOperationException($"{SectionName}:SpaRedirectUris 不能为空");
        }

        if (SpaPostLogoutRedirectUris == null || SpaPostLogoutRedirectUris.Length == 0)
        {
            throw new InvalidOperationException($"{SectionName}:SpaPostLogoutRedirectUris 不能为空");
        }

        if (string.IsNullOrWhiteSpace(FrontendLoginUrl))
        {
            throw new InvalidOperationException($"{SectionName}:FrontendLoginUrl 不能为空");
        }

        if (AccessTokenLifetime <= TimeSpan.Zero)
        {
            throw new InvalidOperationException($"{SectionName}:AccessTokenLifetime 必须大于 0");
        }

        if (RefreshTokenLifetime <= TimeSpan.Zero)
        {
            throw new InvalidOperationException($"{SectionName}:RefreshTokenLifetime 必须大于 0");
        }

        if (string.IsNullOrWhiteSpace(ApiAudience))
        {
            throw new InvalidOperationException($"{SectionName}:ApiAudience 不能为空");
        }
    }
}

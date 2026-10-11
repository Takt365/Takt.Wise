// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktAuthLoginPhases.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：认证流程阶段字符串常量（日志 phase 对齐；仅 string const，非枚举）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 认证登录流程阶段（与 Serilog Action、前端 auth 日志 phase 对齐）
/// </summary>
public static class TaktAuthLoginPhases
{
    /// <summary>
    /// 校验密码（session/verify-password）
    /// </summary>
    public const string VerifyPassword = "verify-password";

    /// <summary>
    /// 建立 Cookie 会话（session/signin）
    /// </summary>
    public const string SignInSession = "signin-session";

    /// <summary>
    /// 注销 Cookie 会话（session/signout）
    /// </summary>
    public const string SignOutSession = "signout-session";

    /// <summary>
    /// OAuth 授权（/connect/authorize）
    /// </summary>
    public const string OAuthAuthorize = "oauth-authorize";

    /// <summary>
    /// 授权码换令牌（grant_type=authorization_code）
    /// </summary>
    public const string AuthorizationCode = "authorization-code";

    /// <summary>
    /// 刷新令牌（grant_type=refresh_token）
    /// </summary>
    public const string RefreshToken = "refresh-token";

    /// <summary>
    /// 客户端凭证（grant_type=client_credentials）
    /// </summary>
    public const string ClientCredentials = "client-credentials";

    /// <summary>
    /// OIDC 登出（/connect/logout）
    /// </summary>
    public const string OidcLogout = "oidc-logout";

    /// <summary>
    /// 加载当前用户资料（GET api/auths/me）
    /// </summary>
    public const string UserProfile = "user-profile";

    /// <summary>
    /// 解析用户角色与功能权限码
    /// </summary>
    public const string UserPermissions = "user-permissions";

    /// <summary>
    /// 构建用户菜单树
    /// </summary>
    public const string UserMenus = "user-menus";

    /// <summary>
    /// 解析可访问路由路径
    /// </summary>
    public const string UserRoutes = "user-routes";

    /// <summary>
    /// 同步 RBAC 到前端 Store
    /// </summary>
    public const string RbacSync = "rbac-sync";

    /// <summary>
    /// 注册 Vue 动态路由
    /// </summary>
    public const string DynamicRoutes = "dynamic-routes";

    /// <summary>
    /// 登录页预览默认语言（租户→用户默认公司→公司 DefaultCulture）
    /// </summary>
    public const string LoginPreviewLocale = "login-preview-locale";

    /// <summary>
    /// Remark 中登录步骤前缀（落库区分 OAuth 多阶段）
    /// </summary>
    public const string LoginStepRemarkPrefix = "login-step:";

    /// <summary>
    /// 构建登录日志 Remark（标记认证流程阶段，便于列表/查询区分同一次登录的多条记录）
    /// </summary>
    /// <param name="phase">流程阶段常量（TaktAuthLoginPhases）</param>
    /// <returns>形如 login-step:verify-password</returns>
    public static string BuildLoginStepRemark(string phase)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(phase);
        return $"{LoginStepRemarkPrefix}{phase.Trim()}";
    }
}

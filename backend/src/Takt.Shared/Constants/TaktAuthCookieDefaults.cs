// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktAuthCookieDefaults.cs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：认证 Cookie 方案名与 Cookie 名称字面量（仅 string const，非枚举）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 授权码登录会话 Cookie 认证常量
/// </summary>
public static class TaktAuthCookieDefaults
{
    /// <summary>
    /// Cookie 认证方案名（/connect/authorize 前须已登录）
    /// </summary>
    public const string AuthenticationScheme = "Takt.Identity.Cookie";

    /// <summary>
    /// 会话 Cookie 名称
    /// </summary>
    public const string CookieName = "Takt.Auth.Session";
}

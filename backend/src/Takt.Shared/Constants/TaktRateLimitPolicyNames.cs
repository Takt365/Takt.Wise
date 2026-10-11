// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktRateLimitPolicyNames.cs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：ASP.NET Core 限流策略名称常量
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 限流策略名称（与 AddRateLimiter AddPolicy 及 EnableRateLimiting 特性一致）
/// </summary>
public static class TaktRateLimitPolicyNames
{
    /// <summary>
    /// 登录相关端点专用限流（verify-password、signin）
    /// </summary>
    public const string Login = "login";
}

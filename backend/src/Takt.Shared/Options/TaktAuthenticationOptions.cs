// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktAuthenticationOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：认证配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 认证配置选项
/// </summary>
public class TaktAuthenticationOptions
{
    public const string SectionName = "Authentication";

    /// <summary>
    /// Access Token 有效期（小时）
    /// </summary>
    public int AccessTokenLifetimeHours { get; set; } = 2;

    /// <summary>
    /// Refresh Token 有效期（天）
    /// </summary>
    public int RefreshTokenLifetimeDays { get; set; } = 7;

    /// <summary>
    /// Refresh Token 重用宽限期（分钟）
    /// </summary>
    public int RefreshTokenReuseLeewayMinutes { get; set; } = 5;

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        if (AccessTokenLifetimeHours <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:AccessTokenLifetimeHours 必须大于 0");
        }

        if (RefreshTokenLifetimeDays <= 0)
        {
            throw new InvalidOperationException($"{SectionName}:RefreshTokenLifetimeDays 必须大于 0");
        }

        if (RefreshTokenReuseLeewayMinutes < 0)
        {
            throw new InvalidOperationException($"{SectionName}:RefreshTokenReuseLeewayMinutes 不能小于 0");
        }
    }
}

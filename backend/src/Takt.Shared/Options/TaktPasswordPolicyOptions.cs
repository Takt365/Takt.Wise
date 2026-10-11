// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktPasswordPolicyOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：密码策略配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 密码策略配置选项
/// </summary>
public class TaktPasswordPolicyOptions
{
    public const string SectionName = "PasswordPolicy";

    /// <summary>
    /// 默认密码（用于初始化用户账号）
    /// </summary>
    public string DefaultPassword { get; set; } = string.Empty;

    /// <summary>
    /// 最小密码长度
    /// </summary>
    public int MinLength { get; set; } = 8;

    /// <summary>
    /// 是否要求包含数字
    /// </summary>
    public bool RequireDigit { get; set; } = true;

    /// <summary>
    /// 是否要求包含小写字母
    /// </summary>
    public bool RequireLowercase { get; set; } = true;

    /// <summary>
    /// 是否要求包含大写字母
    /// </summary>
    public bool RequireUppercase { get; set; } = true;

    /// <summary>
    /// 是否要求包含特殊字符
    /// </summary>
    public bool RequireSpecialCharacter { get; set; } = true;

    /// <summary>
    /// 登录密码 RSA 传输加密（前端密文 → 后端解密 → PBKDF2 验密）
    /// </summary>
    public TaktPasswordTransportOptions Transport { get; set; } = new();

    /// <summary>
    /// 验证配置是否有效
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(DefaultPassword))
        {
            throw new InvalidOperationException($"{SectionName}:DefaultPassword 未配置");
        }

        if (MinLength < 6)
        {
            throw new InvalidOperationException($"{SectionName}:MinLength 至少为 6 位");
        }

        Transport.Validate();
    }
}

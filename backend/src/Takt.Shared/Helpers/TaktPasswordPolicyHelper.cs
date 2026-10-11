// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktPasswordPolicyHelper.cs
// 创建时间：2026-07-07
// 创建人：Takt365(Cursor AI)
// 功能描述：密码策略校验（读取 PasswordPolicy 配置）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// 密码策略校验工具（无状态）
/// </summary>
public static class TaktPasswordPolicyHelper
{
    /// <summary>
    /// 明文密码最大长度（与登录传输解密后一致）
    /// </summary>
    public const int DefaultMaxLength = 128;

    private static readonly Regex DigitRegex = new(@"\d", RegexOptions.Compiled);

    private static readonly Regex LowerRegex = new(@"[a-z]", RegexOptions.Compiled);

    private static readonly Regex UpperRegex = new(@"[A-Z]", RegexOptions.Compiled);

    private static readonly Regex SpecialRegex = new(@"[^a-zA-Z0-9]", RegexOptions.Compiled);

    /// <summary>
    /// 校验明文密码是否满足策略
    /// </summary>
    /// <param name="password">明文密码</param>
    /// <param name="options">密码策略配置</param>
    /// <param name="i18nKey">失败时返回的翻译键</param>
    /// <returns>满足策略为 true</returns>
    public static bool TryValidate(string? password, TaktPasswordPolicyOptions options, out string i18nKey)
    {
        ArgumentNullException.ThrowIfNull(options);
        i18nKey = TaktValidationI18nKeys.ValidationPasswordWeak;

        if (string.IsNullOrWhiteSpace(password))
        {
            i18nKey = TaktValidationI18nKeys.Required;
            return false;
        }

        var minLength = Math.Max(6, options.MinLength);
        if (password.Length < minLength || password.Length > DefaultMaxLength)
        {
            return false;
        }

        if (options.RequireDigit && !DigitRegex.IsMatch(password))
        {
            return false;
        }

        if (options.RequireLowercase && !LowerRegex.IsMatch(password))
        {
            return false;
        }

        if (options.RequireUppercase && !UpperRegex.IsMatch(password))
        {
            return false;
        }

        if (options.RequireSpecialCharacter && !SpecialRegex.IsMatch(password))
        {
            return false;
        }

        return true;
    }
}

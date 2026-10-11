// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktValidationMessageHelper.cs
// 创建时间：2026-05-29
// 创建人：Takt365(Cursor AI)
// 功能描述：抽象校验文案组装（common.validation.* + entity.* / common.field.*）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;

namespace Takt.Shared.Helpers;

/// <summary>
/// 抽象校验与通用提示文案组装（<c>{field}</c>、<c>{min}</c>、<c>{max}</c>、<c>{feature}</c>、<c>{target}</c> 等）。
/// </summary>
public static class TaktValidationMessageHelper
{
    /// <summary>
    /// 组装本地化校验/提示文案。
    /// </summary>
    /// <param name="translate">翻译函数</param>
    /// <param name="messageKey">消息键（如 common.validation.required）</param>
    /// <param name="fieldKey">字段标签键（entity.* / common.field.*），可为 null</param>
    /// <param name="min">最小长度等，写入 {min}</param>
    /// <param name="max">最大长度等，写入 {max}</param>
    /// <param name="extraTokens">额外占位符</param>
    /// <param name="fieldExtras">拼在字段名后的附加值</param>
    /// <returns>完整本地化文案</returns>
    /// <exception cref="ArgumentNullException"><paramref name="translate"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="messageKey"/> 为空</exception>
    public static string Build(
        Func<string, string> translate,
        string messageKey,
        string? fieldKey = null,
        int? min = null,
        int? max = null,
        IReadOnlyDictionary<string, string>? extraTokens = null,
        object[]? fieldExtras = null)
    {
        ArgumentNullException.ThrowIfNull(translate);
        ArgumentException.ThrowIfNullOrWhiteSpace(messageKey);
        fieldExtras ??= [];
        var template = translate(messageKey);
        if (!string.IsNullOrEmpty(fieldKey))
        {
            var fieldLabel = translate(fieldKey);
            if (fieldExtras.Length > 0)
            {
                fieldLabel = $"{fieldLabel} {string.Join(' ', fieldExtras)}";
            }

            template = template.Replace("{field}", fieldLabel, StringComparison.Ordinal);
            template = template.Replace("{feature}", fieldLabel, StringComparison.Ordinal);
        }

        if (min.HasValue)
        {
            template = template.Replace("{min}", min.Value.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
        }

        if (max.HasValue)
        {
            template = template.Replace("{max}", max.Value.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
        }

        if (extraTokens != null)
        {
            foreach (var token in extraTokens)
            {
                template = template.Replace($"{{{token.Key}}}", token.Value, StringComparison.Ordinal);
            }
        }

        return template;
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDictValueHelper.cs
// 功能描述：单选字典 DictValue↔DictLabel 转换与落库规范化（数值 DictValue 存 DictLabel；与前端 takt-dict-convert.ts、TaktDictMultiValueHelper 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Foundation;

namespace Takt.Shared.Helpers;

/// <summary>
/// 单选字典值 DictValue/DictLabel 互转与落库规范化（输入可为 DictValue 或 DictLabel）
/// </summary>
public static class TaktDictValueHelper
{
    /// <summary>
    /// 规范化单选字典落库：数值 DictValue（如 logistics_manufacturing_stop_reason 的「5」）转为 DictLabel（如「停电」）
    /// </summary>
    /// <param name="raw">原始值（DictValue 或 DictLabel）</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="storeAsLabel">true 存 DictLabel；false 存 DictValue</param>
    /// <returns>规范化后的值；空输入返回 null</returns>
    public static string? NormalizeSingleDictStorage(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode,
        bool storeAsLabel)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }
        if (raw.Contains(',', StringComparison.Ordinal))
        {
            var normalized = TaktDictMultiValueHelper.NormalizeCommaSeparatedDictStorage(
                raw,
                snapshot,
                dictTypeCode,
                storeAsLabel);
            return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
        }
        if (storeAsLabel)
        {
            var label = snapshot.ResolvePartToDictLabel(dictTypeCode, raw);
            return string.IsNullOrWhiteSpace(label) ? null : label;
        }
        var value = snapshot.ResolvePartToDictValue(dictTypeCode, raw);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    /// <summary>
    /// 单选 DictValue（或已是 DictLabel）转为 DictLabel
    /// </summary>
    /// <param name="raw">原始值</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <returns>DictLabel；空输入返回 null</returns>
    public static string? ConvertToLabel(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode)
    {
        return NormalizeSingleDictStorage(raw, snapshot, dictTypeCode, storeAsLabel: true);
    }

    /// <summary>
    /// 单选 DictLabel（或已是 DictValue）转为 DictValue
    /// </summary>
    /// <param name="raw">原始值</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <returns>DictValue；空输入返回 null</returns>
    public static string? ConvertToValue(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode)
    {
        return NormalizeSingleDictStorage(raw, snapshot, dictTypeCode, storeAsLabel: false);
    }
}

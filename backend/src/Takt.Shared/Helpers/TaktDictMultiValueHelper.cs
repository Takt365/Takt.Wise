// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDictMultiValueHelper.cs
// 功能描述：多选字典逗号分隔串：sortOrder 排序、DictValue↔DictLabel 转换（单选见 TaktDictValueHelper；与前端 takt-dict-convert.ts 对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Foundation;

namespace Takt.Shared.Helpers;

/// <summary>
/// 多选字典值（逗号分隔）排序与 DictValue/DictLabel 互转辅助
/// </summary>
public static class TaktDictMultiValueHelper
{
    /// <summary>
    /// 拆分逗号分隔字典片段（去空白、去空项）
    /// </summary>
    /// <param name="raw">原始逗号分隔串</param>
    /// <returns>片段列表</returns>
    public static IReadOnlyList<string> SplitCommaSeparated(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return Array.Empty<string>();
        }
        return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    /// <summary>
    /// 拼接逗号分隔字典片段
    /// </summary>
    /// <param name="parts">片段序列</param>
    /// <returns>逗号分隔串；无有效片段时返回空串</returns>
    public static string JoinCommaSeparated(IEnumerable<string>? parts)
    {
        if (parts == null)
        {
            return string.Empty;
        }
        var list = parts
            .Select(part => part?.Trim() ?? string.Empty)
            .Where(part => !string.IsNullOrEmpty(part))
            .ToArray();
        return list.Length == 0 ? string.Empty : string.Join(",", list);
    }

    /// <summary>
    /// 将逗号分隔 DictValue 按字典项 sortOrder 升序重排后拼接
    /// </summary>
    /// <param name="raw">原始逗号分隔串</param>
    /// <param name="sortOrderByValue">DictValue → SortOrder 映射</param>
    /// <returns>排序后的逗号分隔串；空输入返回空串</returns>
    public static string SortCommaSeparatedDictValues(
        string? raw,
        IReadOnlyDictionary<string, int> sortOrderByValue)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return string.Empty;
        }
        ArgumentNullException.ThrowIfNull(sortOrderByValue);
        var parts = SplitCommaSeparated(raw);
        if (parts.Count <= 1)
        {
            return raw.Trim();
        }
        var sorted = parts
            .OrderBy(part => sortOrderByValue.TryGetValue(part, out var order) ? order : int.MaxValue)
            .ThenBy(part => part, StringComparer.Ordinal)
            .ToArray();
        return JoinCommaSeparated(sorted);
    }

    /// <summary>
    /// 逗号分隔串转为 DictLabel 串（片段可为 DictValue 或 DictLabel；按 DictValue 的 sortOrder 排序）
    /// </summary>
    /// <param name="raw">原始逗号分隔串</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="sortOrderByValue">DictValue → SortOrder；可为 null 则不排序</param>
    /// <returns>逗号分隔 DictLabel</returns>
    public static string ConvertCommaSeparatedToLabels(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode,
        IReadOnlyDictionary<string, int>? sortOrderByValue = null)
    {
        return NormalizeCommaSeparatedDictStorage(raw, snapshot, dictTypeCode, storeAsLabel: true, sortOrderByValue);
    }

    /// <summary>
    /// 逗号分隔串转为 DictValue 串（片段可为 DictValue 或 DictLabel；按 sortOrder 排序）
    /// </summary>
    /// <param name="raw">原始逗号分隔串</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="sortOrderByValue">DictValue → SortOrder；可为 null 则不排序</param>
    /// <returns>逗号分隔 DictValue</returns>
    public static string ConvertCommaSeparatedToValues(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode,
        IReadOnlyDictionary<string, int>? sortOrderByValue = null)
    {
        return NormalizeCommaSeparatedDictStorage(raw, snapshot, dictTypeCode, storeAsLabel: false, sortOrderByValue);
    }

    /// <summary>
    /// 规范化多选字典逗号分隔存储（输入可为 DictValue 或 DictLabel 混用）
    /// </summary>
    /// <param name="raw">原始逗号分隔串</param>
    /// <param name="snapshot">字典快照</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="storeAsLabel">true 存 DictLabel；false 存 DictValue</param>
    /// <param name="sortOrderByValue">DictValue → SortOrder；可为 null 则不排序</param>
    /// <returns>规范化后的逗号分隔串</returns>
    public static string NormalizeCommaSeparatedDictStorage(
        string? raw,
        TaktDictSnapshot snapshot,
        string dictTypeCode,
        bool storeAsLabel,
        IReadOnlyDictionary<string, int>? sortOrderByValue = null)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        var parts = SplitCommaSeparated(raw);
        if (parts.Count == 0)
        {
            return string.Empty;
        }
        var resolvedValues = parts
            .Select(part => snapshot.ResolvePartToDictValue(dictTypeCode, part))
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .ToList();
        if (resolvedValues.Count == 0)
        {
            return string.Empty;
        }
        IEnumerable<string> orderedValues = resolvedValues;
        if (sortOrderByValue != null && resolvedValues.Count > 1)
        {
            orderedValues = resolvedValues
                .OrderBy(part => sortOrderByValue.TryGetValue(part, out var order) ? order : int.MaxValue)
                .ThenBy(part => part, StringComparer.Ordinal);
        }
        if (storeAsLabel)
        {
            var labels = orderedValues
                .Select(value => snapshot.ResolvePartToDictLabel(dictTypeCode, value))
                .Where(label => !string.IsNullOrWhiteSpace(label));
            return JoinCommaSeparated(labels);
        }
        return JoinCommaSeparated(orderedValues);
    }
}

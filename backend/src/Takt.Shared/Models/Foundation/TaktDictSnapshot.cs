// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Models.Foundation
// 文件名称：TaktDictSnapshot.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：字典双向快照（code↔name；导入/导出 O(1) 查表，由 ITaktDictDataService 预加载）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;

namespace Takt.Shared.Models.Foundation;

/// <summary>
/// 字典双向快照（租户数据由 ITaktDictDataService.CreateDictSnapshotAsync 预加载）
/// </summary>
public sealed class TaktDictSnapshot
{
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _valueToLabel;
    private readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _labelToValue;

    private TaktDictSnapshot(
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> valueToLabelMaps,
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> labelToValueMaps)
    {
        _valueToLabel = valueToLabelMaps;
        _labelToValue = labelToValueMaps;
    }

    /// <summary>
    /// 由字典行构建双向快照
    /// </summary>
    /// <param name="rows">字典行（DictTypeCode、DictValue、DictLabel）</param>
    /// <param name="dictTypeCodes">须保证存在的类型编码（无数据时补空表）</param>
    /// <returns>快照</returns>
    public static TaktDictSnapshot CreateFromRows(
        IEnumerable<(string DictTypeCode, string DictValue, string DictLabel)> rows,
        params string[] dictTypeCodes)
    {
        ArgumentNullException.ThrowIfNull(rows);
        var valueToLabel = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
        var labelToValue = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
        foreach (var (dictTypeCode, dictValue, dictLabel) in rows)
        {
            if (string.IsNullOrWhiteSpace(dictTypeCode))
            {
                continue;
            }
            var typeKey = dictTypeCode.Trim();
            if (!valueToLabel.TryGetValue(typeKey, out var forward))
            {
                forward = new Dictionary<string, string>(StringComparer.Ordinal);
                valueToLabel[typeKey] = forward;
                labelToValue[typeKey] = new Dictionary<string, string>(StringComparer.Ordinal);
            }
            if (string.IsNullOrWhiteSpace(dictValue))
            {
                continue;
            }
            var valueKey = dictValue.Trim();
            var label = string.IsNullOrWhiteSpace(dictLabel) ? valueKey : dictLabel.Trim();
            forward[valueKey] = label;
            if (!string.IsNullOrWhiteSpace(dictLabel))
            {
                labelToValue[typeKey][label] = valueKey;
            }
        }
        if (dictTypeCodes != null)
        {
            foreach (var code in dictTypeCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c.Trim()).Distinct(StringComparer.Ordinal))
            {
                if (!valueToLabel.ContainsKey(code))
                {
                    valueToLabel[code] = new Dictionary<string, string>(StringComparer.Ordinal);
                    labelToValue[code] = new Dictionary<string, string>(StringComparer.Ordinal);
                }
            }
        }
        return new TaktDictSnapshot(
            valueToLabel.ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<string, string>)kv.Value, StringComparer.Ordinal),
            labelToValue.ToDictionary(kv => kv.Key, kv => (IReadOnlyDictionary<string, string>)kv.Value, StringComparer.Ordinal));
    }

    /// <summary>
    /// 导出：整型 code → 显示名
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="code">整型字典值</param>
    /// <param name="fallback">未命中回退</param>
    /// <returns>DictLabel 或回退</returns>
    public string GetName(string dictTypeCode, int code, string fallback)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (!_valueToLabel.TryGetValue(dictTypeCode, out var map))
        {
            return fallback;
        }
        var key = code.ToString(CultureInfo.InvariantCulture);
        return map.TryGetValue(key, out var label) ? label : fallback;
    }

    /// <summary>
    /// 导入：显示名 → 整型 code
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="name">DictLabel</param>
    /// <returns>整型值；未命中 null</returns>
    public int? GetCode(string dictTypeCode, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(name) || !_labelToValue.TryGetValue(dictTypeCode, out var map))
        {
            return null;
        }
        if (!map.TryGetValue(name.Trim(), out var valueStr))
        {
            return null;
        }
        return int.TryParse(valueStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var code)
            ? code
            : null;
    }

    /// <summary>
    /// 导入行：文本列优先，否则校验数值列
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="numericValue">数值列</param>
    /// <param name="label">文本列</param>
    /// <param name="code">解析结果</param>
    /// <param name="error">失败原因</param>
    /// <returns>是否成功</returns>
    public bool TryResolveImportCode(
        string dictTypeCode,
        int numericValue,
        string? label,
        out int code,
        out string? error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (!string.IsNullOrWhiteSpace(label))
        {
            var fromLabel = GetCode(dictTypeCode, label);
            if (!fromLabel.HasValue)
            {
                code = 0;
                error = $"字典标签「{label.Trim()}」无效";
                return false;
            }
            code = fromLabel.Value;
            error = null;
            return true;
        }
        if (_valueToLabel.TryGetValue(dictTypeCode, out var map))
        {
            var key = numericValue.ToString(CultureInfo.InvariantCulture);
            if (!map.ContainsKey(key))
            {
                code = 0;
                error = $"字典值「{numericValue}」无效";
                return false;
            }
        }
        code = numericValue;
        error = null;
        return true;
    }

    /// <summary>
    /// 导出：DictValue 字符串 → DictLabel
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="dictValue">DictValue</param>
    /// <param name="fallback">未命中回退；为空时回退为 dictValue 本身</param>
    /// <returns>DictLabel 或回退</returns>
    public string GetLabel(string dictTypeCode, string dictValue, string? fallback = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(dictValue))
        {
            return fallback ?? string.Empty;
        }
        var typeKey = dictTypeCode.Trim();
        var valueKey = dictValue.Trim();
        if (!_valueToLabel.TryGetValue(typeKey, out var map))
        {
            return fallback ?? valueKey;
        }
        return map.TryGetValue(valueKey, out var label) ? label : (fallback ?? valueKey);
    }

    /// <summary>
    /// 导入：DictLabel → DictValue 字符串
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="dictLabel">DictLabel</param>
    /// <returns>DictValue；未命中 null</returns>
    public string? GetValueByLabel(string dictTypeCode, string dictLabel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(dictLabel))
        {
            return null;
        }
        var typeKey = dictTypeCode.Trim();
        if (!_labelToValue.TryGetValue(typeKey, out var map))
        {
            return null;
        }
        return map.TryGetValue(dictLabel.Trim(), out var value) ? value : null;
    }

    /// <summary>
    /// 解析逗号分隔片段为 canonical DictValue（片段可为 DictValue 或 DictLabel）
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="part">单个片段</param>
    /// <returns>DictValue；无法解析时返回 trim 后的原片段</returns>
    public string ResolvePartToDictValue(string dictTypeCode, string part)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(part))
        {
            return string.Empty;
        }
        var typeKey = dictTypeCode.Trim();
        var trimmed = part.Trim();
        if (_valueToLabel.TryGetValue(typeKey, out var forward) && forward.ContainsKey(trimmed))
        {
            return trimmed;
        }
        var fromLabel = GetValueByLabel(typeKey, trimmed);
        return fromLabel ?? trimmed;
    }

    /// <summary>
    /// 解析逗号分隔片段为 canonical DictLabel（片段可为 DictValue 或 DictLabel）
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="part">单个片段</param>
    /// <returns>DictLabel；无法解析时返回 trim 后的原片段</returns>
    public string ResolvePartToDictLabel(string dictTypeCode, string part)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (string.IsNullOrWhiteSpace(part))
        {
            return string.Empty;
        }
        var typeKey = dictTypeCode.Trim();
        var trimmed = part.Trim();
        if (_labelToValue.TryGetValue(typeKey, out var reverse) && reverse.ContainsKey(trimmed))
        {
            return trimmed;
        }
        return GetLabel(typeKey, trimmed, trimmed);
    }
}

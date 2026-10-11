// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDictStorageHelper.cs
// 功能描述：字典落库规范化（显式 dict_type_code；UI 提交由前端 dict-type 转换，本类主要用于 Excel 导入等旁路）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using Takt.Shared.Models.Foundation;

namespace Takt.Shared.Helpers;

/// <summary>
/// 字典字段落库绑定（dict_type_code 须与前端 TaktSelect dict-type 一致）
/// </summary>
/// <param name="PropertyName">DTO/实体属性名</param>
/// <param name="DictTypeCode">字典类型编码（takt_foundation_dict_type.dict_type_code）</param>
/// <param name="MultiSelect">是否多选逗号分隔</param>
public readonly record struct TaktDictFieldStorageBinding(string PropertyName, string DictTypeCode, bool MultiSelect);

/// <summary>
/// 字典落库规范化（不依赖实体 Attribute；dict_type_code 由调用方显式传入）
/// </summary>
public static class TaktDictStorageHelper
{
    /// <summary>
    /// 从绑定项收集 dict_type_code（去重、保序）
    /// </summary>
    /// <param name="bindings">字段绑定</param>
    /// <returns>dict_type_code 列表</returns>
    public static IReadOnlyList<string> CollectDictTypeCodes(IEnumerable<TaktDictFieldStorageBinding> bindings)
    {
        ArgumentNullException.ThrowIfNull(bindings);
        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var binding in bindings)
        {
            if (string.IsNullOrWhiteSpace(binding.DictTypeCode))
            {
                continue;
            }
            var code = binding.DictTypeCode.Trim();
            if (seen.Add(code))
            {
                result.Add(code);
            }
        }
        return result;
    }

    /// <summary>
    /// 将单个字典字段规范为 DictLabel 落库值
    /// </summary>
    /// <param name="raw">原始值</param>
    /// <param name="context">字典落库上下文</param>
    /// <param name="dictTypeCode">字典类型编码</param>
    /// <param name="multiSelect">是否多选</param>
    /// <returns>DictLabel；多选空值返回 null，单选空值返回空串</returns>
    public static string? NormalizeStorageLabel(
        string? raw,
        TaktDictStorageContext context,
        string dictTypeCode,
        bool multiSelect)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        if (!multiSelect)
        {
            return TaktDictValueHelper.ConvertToLabel(raw, context.Snapshot, dictTypeCode) ?? string.Empty;
        }
        context.SortMapsByTypeCode.TryGetValue(dictTypeCode, out var sortMap);
        var normalized = TaktDictMultiValueHelper.NormalizeCommaSeparatedDictStorage(
            raw,
            context.Snapshot,
            dictTypeCode,
            storeAsLabel: true,
            sortMap);
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }

    /// <summary>
    /// 按绑定项就地写回对象上的字典字段（Excel 导入等）
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="context">字典落库上下文</param>
    /// <param name="bindings">字段绑定</param>
    public static void ApplyStorageLabels(
        object target,
        Type targetType,
        TaktDictStorageContext context,
        IReadOnlyList<TaktDictFieldStorageBinding> bindings)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(targetType);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(bindings);
        foreach (var binding in bindings)
        {
            if (string.IsNullOrWhiteSpace(binding.PropertyName))
            {
                continue;
            }
            var property = targetType.GetProperty(
                binding.PropertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                ?? throw new InvalidOperationException($"属性 {targetType.Name}.{binding.PropertyName} 不存在。");
            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException($"属性 {targetType.Name}.{binding.PropertyName} 须为 string 类型。");
            }
            var current = property.GetValue(target) as string;
            property.SetValue(
                target,
                NormalizeStorageLabel(current, context, binding.DictTypeCode, binding.MultiSelect));
        }
    }
}

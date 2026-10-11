// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktDictTypeAttributeHelper.cs
// 功能描述：读取 DTO 上 TaktDictTypeAttribute（导出/显示名等）；运行时落库转换见 TaktDictStorageHelper，UI 提交由前端 dict-type 负责
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Reflection;
using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 读取属性级 TaktDictTypeAttribute（非运行时落库权威来源；全量字典以库表 + 前端 dict-type 为准）
/// </summary>
public static class TaktDictTypeAttributeHelper
{
    /// <summary>
    /// 读取属性上的字典类型编码
    /// </summary>
    /// <typeparam name="TType">类型</typeparam>
    /// <param name="propertyName">属性名</param>
    /// <returns>dict_type_code；未标注时 null</returns>
    public static string? TryGetDictTypeCode<TType>(string propertyName)
    {
        return TryGetDictTypeCode(typeof(TType), propertyName);
    }

    /// <summary>
    /// 读取属性上的字典类型编码
    /// </summary>
    /// <param name="targetType">类型</param>
    /// <param name="propertyName">属性名</param>
    /// <returns>dict_type_code；未标注时 null</returns>
    public static string? TryGetDictTypeCode(Type targetType, string propertyName)
    {
        return GetProperty(targetType, propertyName)?.GetCustomAttribute<TaktDictTypeAttribute>()?.DictTypeCode;
    }

    /// <summary>
    /// 是否多选字典字段
    /// </summary>
    /// <param name="targetType">类型</param>
    /// <param name="propertyName">属性名</param>
    /// <returns>是否标注 TaktDictMultiSelectAttribute</returns>
    public static bool IsMultiSelect(Type targetType, string propertyName)
    {
        return GetProperty(targetType, propertyName)?.GetCustomAttribute<TaktDictMultiSelectAttribute>() != null;
    }

    private static PropertyInfo? GetProperty(Type targetType, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(targetType);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        return targetType.GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Extensions
// 文件名称：TaktEnumExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：通用枚举扩展方法（所有枚举通用）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;

namespace Takt.Shared.Extensions;

/// <summary>
/// 通用枚举扩展方法
/// </summary>
/// <remarks>
/// 为所有枚举提供通用的描述获取方法。
/// 使用方式：在枚举值上添加 [Display(Name = "描述文本")] 特性。
/// </remarks>
public static class TaktEnumExtensions
{
    /// <summary>
    /// 获取枚举的描述文本
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <returns>描述文本，如果没有 Display 特性则返回枚举名称</returns>
    public static string GetDescription<T>(this T enumValue) where T : Enum
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        
        if (fieldInfo == null)
            return enumValue.ToString();

        var displayAttribute = fieldInfo
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? enumValue.ToString();
    }

    /// <summary>
    /// 获取枚举的名称（不带描述）
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <returns>枚举名称</returns>
    public static string GetName<T>(this T enumValue) where T : Enum
    {
        return enumValue.ToString();
    }

    /// <summary>
    /// 获取枚举的值（数字）
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="enumValue">枚举值</param>
    /// <returns>枚举的整数值</returns>
    public static int GetValue<T>(this T enumValue) where T : Enum
    {
        return Convert.ToInt32(enumValue);
    }
}

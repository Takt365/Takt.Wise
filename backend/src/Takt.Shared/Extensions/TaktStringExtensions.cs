// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Extensions
// 文件名称：TaktStringExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt字符串扩展方法，提供常用的字符串操作扩展
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Extensions;

/// <summary>
/// Takt字符串扩展方法
/// </summary>
public static class TaktStringExtensions
{
    /// <summary>
    /// 判断字符串是否为空或空白
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <returns>是否为空或空白</returns>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// 判断字符串是否不为空且非空白
    /// </summary>
    /// <param name="value">字符串值</param>
    /// <returns>是否不为空且非空白</returns>
    public static bool IsNotNullOrWhiteSpace(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
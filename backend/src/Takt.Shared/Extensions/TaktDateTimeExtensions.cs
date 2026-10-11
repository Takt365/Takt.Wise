// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Extensions
// 文件名称：TaktDateTimeExtensions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt日期时间扩展方法，提供常用的日期时间操作扩展
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Extensions;

/// <summary>
/// Takt日期时间扩展方法
/// </summary>
public static class TaktDateTimeExtensions
{
    /// <summary>
    /// 转换为时间戳（秒）
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>时间戳（秒）</returns>
    public static long ToUnixTimeSeconds(this DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 转换为时间戳（毫秒）
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>时间戳（毫秒）</returns>
    public static long ToUnixTimeMilliseconds(this DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 从时间戳（秒）转换为DateTime
    /// </summary>
    /// <param name="seconds">时间戳（秒）</param>
    /// <returns>日期时间</returns>
    public static DateTime FromUnixTimeSeconds(long seconds)
    {
        return DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
    }

    /// <summary>
    /// 从时间戳（毫秒）转换为DateTime
    /// </summary>
    /// <param name="milliseconds">时间戳（毫秒）</param>
    /// <returns>日期时间</returns>
    public static DateTime FromUnixTimeMilliseconds(long milliseconds)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds).DateTime;
    }
}
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Quartz
// 文件名称：TaktQuartzJobDataMapHelper.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：安全读取 Quartz JobDataMap（缺键不抛 KeyNotFoundException）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Quartz;

namespace Takt.Infrastructure.Quartz;

/// <summary>
/// Quartz JobDataMap 安全读取
/// </summary>
public static class TaktQuartzJobDataMapHelper
{
    /// <summary>
    /// 读取字符串；键不存在或值为 null 时返回 null（不抛 KeyNotFoundException）
    /// </summary>
    /// <param name="data">JobDataMap</param>
    /// <param name="key">键</param>
    /// <returns>字符串或 null</returns>
    public static string? GetStringOrNull(JobDataMap data, string key)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (!data.ContainsKey(key))
        {
            return null;
        }
        var value = data[key];
        return value?.ToString();
    }

    /// <summary>
    /// 读取 long；键不存在或无法解析时返回 0
    /// </summary>
    /// <param name="data">JobDataMap</param>
    /// <param name="key">键</param>
    /// <returns>数值</returns>
    public static long GetLongOrDefault(JobDataMap data, string key)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (!data.ContainsKey(key))
        {
            return 0;
        }
        var value = data[key];
        return value switch
        {
            long l => l,
            int i => i,
            string s when long.TryParse(s, out var parsed) => parsed,
            _ => 0,
        };
    }

    /// <summary>
    /// 读取 int；键不存在或无法解析时返回 0
    /// </summary>
    /// <param name="data">JobDataMap</param>
    /// <param name="key">键</param>
    /// <returns>数值</returns>
    public static int GetIntOrDefault(JobDataMap data, string key)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        if (!data.ContainsKey(key))
        {
            return 0;
        }
        var value = data[key];
        return value switch
        {
            int i => i,
            long l => checked((int)l),
            string s when int.TryParse(s, out var parsed) => parsed,
            _ => 0,
        };
    }
}

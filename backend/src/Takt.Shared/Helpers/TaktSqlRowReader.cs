// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktSqlRowReader.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：SQL 动态行字典读取辅助（列名大小写不敏感）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;

namespace Takt.Shared.Helpers;

/// <summary>
/// Dictionary{TKey,TValue} 形式 SQL 结果行读取器（无状态纯函数）。
/// </summary>
public static class TaktSqlRowReader
{
    /// <summary>
    /// 读取字符串列（支持多个候选列名）
    /// </summary>
    /// <param name="row">结果行</param>
    /// <param name="columnNames">列名候选</param>
    /// <returns>去空白后的值；不存在或空则 null</returns>
    /// <exception cref="ArgumentNullException"><paramref name="row"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="columnNames"/> 为空或未提供有效列名</exception>
    public static string? GetString(IReadOnlyDictionary<string, object> row, params string[] columnNames)
    {
        ArgumentNullException.ThrowIfNull(row);
        ArgumentNullException.ThrowIfNull(columnNames);
        if (columnNames.Length == 0)
        {
            throw new ArgumentException("至少提供一个列名", nameof(columnNames));
        }

        foreach (var name in columnNames)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            if (!row.TryGetValue(name, out var raw) || raw == null)
            {
                continue;
            }

            var text = Convert.ToString(raw, CultureInfo.InvariantCulture)?.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
        }

        return null;
    }

    /// <summary>
    /// 读取 32 位整数列
    /// </summary>
    /// <param name="row">结果行</param>
    /// <param name="columnNames">列名候选</param>
    /// <returns>整数值；不存在或无法转换则 null</returns>
    /// <exception cref="ArgumentNullException"><paramref name="row"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="columnNames"/> 为空或未提供有效列名</exception>
    public static int? GetInt32(IReadOnlyDictionary<string, object> row, params string[] columnNames)
    {
        ArgumentNullException.ThrowIfNull(row);
        ArgumentNullException.ThrowIfNull(columnNames);
        if (columnNames.Length == 0)
        {
            throw new ArgumentException("至少提供一个列名", nameof(columnNames));
        }

        foreach (var name in columnNames)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            if (!row.TryGetValue(name, out var raw) || raw == null)
            {
                continue;
            }

            if (raw is int intValue)
            {
                return intValue;
            }

            if (raw is long longValue && longValue >= int.MinValue && longValue <= int.MaxValue)
            {
                return (int)longValue;
            }

            if (int.TryParse(Convert.ToString(raw, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
        }

        return null;
    }
}

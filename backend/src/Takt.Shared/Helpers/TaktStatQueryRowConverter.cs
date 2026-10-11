// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktStatQueryRowConverter.cs
// 创建时间：2026-06-19
// 创建人：Takt365(Cursor AI)
// 功能描述：SqlSugar Queryable 查询结果行转换为字典（列键对齐）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Collections;
using System.Data;

namespace Takt.Shared.Helpers;

/// <summary>
/// SqlSugar 动态查询结果行转换器（纯函数）
/// </summary>
public static class TaktStatQueryRowConverter
{
    /// <summary>
    /// 将 DataTable 转为按输出列键对齐的字典行
    /// </summary>
    /// <param name="dataTable">SqlSugar ToDataTableAsync 结果</param>
    /// <param name="outputKeys">输出列键</param>
    /// <returns>字典行列表</returns>
    public static IReadOnlyList<Dictionary<string, object?>> FromDataTable(
        DataTable? dataTable,
        IReadOnlyList<string> outputKeys)
    {
        ArgumentNullException.ThrowIfNull(outputKeys);
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            return Array.Empty<Dictionary<string, object?>>();
        }
        var rows = new List<Dictionary<string, object?>>(dataTable.Rows.Count);
        foreach (DataRow dataRow in dataTable.Rows)
        {
            var source = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn column in dataTable.Columns)
            {
                var value = dataRow[column];
                source[column.ColumnName] = value == DBNull.Value ? null : value;
            }
            rows.Add(NormalizeRow(source, outputKeys));
        }
        return rows;
    }

    /// <summary>
    /// 将 SqlSugar 动态对象列表转为字典行
    /// </summary>
    /// <param name="rows">ToPageListAsync / ToListAsync 动态结果</param>
    /// <param name="outputKeys">输出列键</param>
    /// <returns>字典行列表</returns>
    public static IReadOnlyList<Dictionary<string, object?>> FromDynamicRows(
        IEnumerable<object>? rows,
        IReadOnlyList<string> outputKeys)
    {
        ArgumentNullException.ThrowIfNull(outputKeys);
        if (rows == null)
        {
            return Array.Empty<Dictionary<string, object?>>();
        }
        var result = new List<Dictionary<string, object?>>();
        foreach (var row in rows)
        {
            if (row == null)
            {
                continue;
            }
            var source = ToDictionary(row);
            result.Add(NormalizeRow(source, outputKeys));
        }
        return result;
    }

    /// <summary>
    /// 将单行对象转为字典
    /// </summary>
    /// <param name="row">动态行</param>
    /// <returns>源字典</returns>
    private static Dictionary<string, object?> ToDictionary(object row)
    {
        if (row is IDictionary<string, object?> nullableDict)
        {
            return new Dictionary<string, object?>(nullableDict, StringComparer.OrdinalIgnoreCase);
        }
        if (row is IDictionary<string, object> dict)
        {
            return dict.ToDictionary(
                kv => kv.Key,
                kv => kv.Value is DBNull ? null : kv.Value,
                StringComparer.OrdinalIgnoreCase);
        }
        if (row is IDictionary legacyDict)
        {
            var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (DictionaryEntry entry in legacyDict)
            {
                var key = Convert.ToString(entry.Key) ?? string.Empty;
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                result[key] = entry.Value is DBNull ? null : entry.Value;
            }
            return result;
        }
        var fallback = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in row.GetType().GetProperties())
        {
            if (!property.CanRead)
            {
                continue;
            }
            var value = property.GetValue(row);
            fallback[property.Name] = value is DBNull ? null : value;
        }
        return fallback;
    }

    /// <summary>
    /// 按输出列键对齐单行
    /// </summary>
    /// <param name="source">源字典</param>
    /// <param name="outputKeys">输出列键</param>
    /// <returns>对齐后的行</returns>
    private static Dictionary<string, object?> NormalizeRow(
        IReadOnlyDictionary<string, object?> source,
        IReadOnlyList<string> outputKeys)
    {
        var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var key in outputKeys)
        {
            object? cell = null;
            if (source.TryGetValue(key, out var direct))
            {
                cell = direct;
            }
            else
            {
                foreach (var kv in source)
                {
                    if (kv.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        cell = kv.Value;
                        break;
                    }
                }
            }
            row[key] = cell is DBNull ? null : cell;
        }
        return row;
    }
}

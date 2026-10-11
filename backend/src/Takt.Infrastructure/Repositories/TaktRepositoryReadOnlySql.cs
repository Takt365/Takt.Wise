// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktRepositoryReadOnlySql.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：仓储层只读 SQL 查询内部实现（供三级仓储复用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Data;
using SqlSugar;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 只读 SQL 查询内部实现（与 TaktTenantRepository{TEntity} 等共用 Ado）
/// </summary>
internal static class TaktRepositoryReadOnlySql
{
    /// <summary>
    /// 执行只读 SQL 并返回动态行（列名不区分大小写；DBNull 转为 null）
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表；无数据时返回空列表</returns>
    /// <exception cref="OperationCanceledException">取消令牌已触发时抛出</exception>
    public static async Task<IReadOnlyList<Dictionary<string, object>>> QueryAsync(
        ISqlSugarClient db,
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var sugarParameters = BuildSugarParameters(parameters);
        var dataTable = sugarParameters.Length == 0
            ? await db.Ado.GetDataTableAsync(sql)
            : await db.Ado.GetDataTableAsync(sql, sugarParameters);

        return ConvertDataTable(dataTable);
    }

    /// <summary>
    /// 将 ADO DataTable 转为字典行列表
    /// </summary>
    /// <param name="dataTable">查询结果表</param>
    /// <returns>字典行列表；空表或 null 时返回空列表</returns>
    private static IReadOnlyList<Dictionary<string, object>> ConvertDataTable(DataTable? dataTable)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            return Array.Empty<Dictionary<string, object>>();
        }

        var rows = new List<Dictionary<string, object>>(dataTable.Rows.Count);
        foreach (DataRow dataRow in dataTable.Rows)
        {
            var row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (DataColumn column in dataTable.Columns)
            {
                var value = dataRow[column];
                row[column.ColumnName] = value == DBNull.Value ? null! : value;
            }

            rows.Add(row);
        }

        return rows;
    }

    /// <summary>
    /// 将命名参数字典转为 SqlSugar 参数数组
    /// </summary>
    /// <param name="parameters">命名参数字典（可选）</param>
    /// <returns>SqlSugar 参数数组；无参数时返回空数组</returns>
    private static SugarParameter[] BuildSugarParameters(IReadOnlyDictionary<string, object?>? parameters)
    {
        if (parameters == null || parameters.Count == 0)
        {
            return Array.Empty<SugarParameter>();
        }

        return parameters
            .Select(pair => new SugarParameter(pair.Key, pair.Value))
            .ToArray();
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktStatQueryBuilder.cs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表 SqlSugar Queryable 编译器（JOIN/筛选/分组/排序）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using SqlSugar;
using Takt.Shared.Models.Statistics;

namespace Takt.Shared.Helpers;

/// <summary>
/// 定制报表 SqlSugar Queryable 编译器（纯函数，无副作用）
/// </summary>
public static class TaktStatQueryBuilder
{
    private static readonly Regex TableNamePattern = new(@"^takt_[a-z0-9_]+$", RegexOptions.Compiled);
    private static readonly Regex ColumnNamePattern = new(@"^[a-z][a-z0-9_]*$", RegexOptions.Compiled);
    private static readonly Regex AliasPattern = new(@"^[A-Za-z][A-Za-z0-9_]{0,9}$", RegexOptions.Compiled);

    /// <summary>
    /// 将报表定义编译为 SqlSugar Queryable（无实体动态表 AS + AddJoinInfo）
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="request">编译请求</param>
    /// <returns>Queryable 与输出列元数据</returns>
    /// <exception cref="ArgumentNullException">db 或 request 为空</exception>
    /// <exception cref="ArgumentException">定义非法或缺少必填项</exception>
    public static (ISugarQueryable<object> Queryable, TaktStatQueryCompiled Metadata) Compile(
        ISqlSugarClient db,
        TaktStatQueryBuildRequest request)
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.TenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.CompanyCode);
        if (request.Sources == null || request.Sources.Count == 0)
        {
            throw new ArgumentException("报表至少需要一个数据源表", nameof(request));
        }
        var sources = request.Sources.OrderBy(x => x.SortOrder).ToList();
        var primary = sources.FirstOrDefault(x => x.IsPrimary == 1) ?? sources[0];
        var aliasToTable = BuildAliasMap(sources);
        var parameters = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["p_tenant_code"] = request.TenantCode,
            ["p_company_code"] = request.CompanyCode,
        };
        var visibleFields = (request.Fields ?? Array.Empty<TaktStatQueryFieldItem>())
            .Where(x => x.IsVisible == 1)
            .OrderBy(x => x.SortOrder)
            .ToList();
        if (visibleFields.Count == 0)
        {
            throw new ArgumentException("报表至少需要一个输出字段", nameof(request));
        }
        var groupBys = (request.GroupBys ?? Array.Empty<TaktStatQueryGroupByItem>())
            .OrderBy(x => x.SortOrder)
            .ToList();
        var hasAggregate = visibleFields.Any(x => x.AggregateFunc != 0);
        var useGroupBy = groupBys.Count > 0 || hasAggregate;
        var outputKeys = new List<string>();
        var outputLabels = new List<string>();
        var selectParts = new List<string>();
        foreach (var field in visibleFields)
        {
            ValidateAlias(field.SourceAlias);
            ValidateColumn(field.ColumnName);
            var colExpr = ColumnExpr(field.SourceAlias, field.ColumnName);
            var expr = field.AggregateFunc switch
            {
                1 => $"COUNT({colExpr})",
                2 => $"SUM({colExpr})",
                3 => $"AVG({colExpr})",
                4 => $"MIN({colExpr})",
                5 => $"MAX({colExpr})",
                _ => colExpr,
            };
            var outputKey = ResolveOutputKey(field);
            outputKeys.Add(outputKey);
            outputLabels.Add(string.IsNullOrWhiteSpace(field.DisplayName) ? outputKey : field.DisplayName);
            selectParts.Add($"{expr} AS {outputKey}");
        }
        ValidateAlias(primary.SourceAlias);
        ValidateTable(primary.TableName);
        ISugarQueryable<object> query = db.Queryable<object>().AS(primary.TableName, primary.SourceAlias);
        foreach (var join in (request.Joins ?? Array.Empty<TaktStatQueryJoinItem>()).OrderBy(x => x.SortOrder))
        {
            ValidateAlias(join.LeftSourceAlias);
            ValidateAlias(join.RightSourceAlias);
            ValidateColumn(join.LeftColumnName);
            ValidateColumn(join.RightColumnName);
            if (!aliasToTable.ContainsKey(join.LeftSourceAlias) || !aliasToTable.ContainsKey(join.RightSourceAlias))
            {
                throw new ArgumentException($"JOIN 引用了未声明的数据源别名: {join.LeftSourceAlias} / {join.RightSourceAlias}");
            }
            var rightTable = aliasToTable[join.RightSourceAlias];
            ValidateTable(rightTable);
            var onExpr = $"{join.LeftSourceAlias}.{join.LeftColumnName}={join.RightSourceAlias}.{join.RightColumnName}";
            query = query.AddJoinInfo(rightTable, join.RightSourceAlias, onExpr, MapJoinType(join.JoinType));
        }
        var whereSql = BuildWhereConditionSql(
            primary.SourceAlias,
            request.Selections ?? Array.Empty<TaktStatQuerySelectionItem>(),
            request.RuntimeSelectionValues,
            parameters);
        query = query.Where(whereSql, ToSugarParameters(parameters));
        if (useGroupBy)
        {
            var groupColumnSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var groupColumns = new List<string>();
            foreach (var groupBy in groupBys)
            {
                ValidateAlias(groupBy.SourceAlias);
                ValidateColumn(groupBy.ColumnName);
                var groupExpr = ColumnExpr(groupBy.SourceAlias, groupBy.ColumnName);
                if (groupColumnSet.Add(groupExpr))
                {
                    groupColumns.Add(groupExpr);
                }
            }
            foreach (var field in visibleFields.Where(x => x.AggregateFunc == 0))
            {
                var groupExpr = ColumnExpr(field.SourceAlias, field.ColumnName);
                if (groupColumnSet.Add(groupExpr))
                {
                    groupColumns.Add(groupExpr);
                }
            }
            if (groupColumns.Count == 0)
            {
                throw new ArgumentException("启用聚合时必须配置分组字段或非聚合输出列", nameof(request));
            }
            query = query.GroupBy(string.Join(", ", groupColumns));
        }
        var orderBys = (request.OrderBys ?? Array.Empty<TaktStatQueryOrderByItem>())
            .OrderBy(x => x.SortOrder)
            .ToList();
        if (orderBys.Count > 0)
        {
            var orderParts = orderBys.Select(x =>
            {
                ValidateAlias(x.SourceAlias);
                ValidateColumn(x.ColumnName);
                var dir = string.Equals(x.SortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
                return $"{ColumnExpr(x.SourceAlias, x.ColumnName)} {dir}";
            });
            query = query.OrderBy(string.Join(", ", orderParts));
        }
        if (request.DistinctRows == 1)
        {
            query = query.Distinct();
        }
        query = query.Select(string.Join(", ", selectParts));
        var metadata = new TaktStatQueryCompiled
        {
            OutputKeys = outputKeys,
            OutputLabels = outputLabels,
        };
        return (query, metadata);
    }

    /// <summary>
    /// 映射 JOIN 类型到 SqlSugar JoinType
    /// </summary>
    /// <param name="joinType">业务 JOIN 类型</param>
    /// <returns>SqlSugar JoinType</returns>
    private static JoinType MapJoinType(int joinType)
    {
        return joinType switch
        {
            2 => JoinType.Left,
            3 => JoinType.Right,
            4 => JoinType.Full,
            _ => JoinType.Inner,
        };
    }

    /// <summary>
    /// 构建 WHERE 条件（不含 WHERE 关键字，供 Queryable.Where 使用）
    /// </summary>
    /// <param name="primaryAlias">主表别名</param>
    /// <param name="selections">筛选字段定义</param>
    /// <param name="runtimeValues">运行时筛选值</param>
    /// <param name="parameters">输出 SQL 参数</param>
    /// <returns>WHERE 条件 SQL</returns>
    private static string BuildWhereConditionSql(
        string primaryAlias,
        IReadOnlyList<TaktStatQuerySelectionItem> selections,
        IReadOnlyDictionary<long, TaktStatQuerySelectionValue> runtimeValues,
        IDictionary<string, object?> parameters)
    {
        var clauses = new List<string>
        {
            $"{ColumnExpr(primaryAlias, "tenant_code")} = @p_tenant_code",
            $"{ColumnExpr(primaryAlias, "company_code")} = @p_company_code",
            $"{ColumnExpr(primaryAlias, "is_deleted")} = 0",
        };
        foreach (var selection in selections.OrderBy(x => x.SortOrder))
        {
            ValidateAlias(selection.SourceAlias);
            ValidateColumn(selection.ColumnName);
            var runtimeKey = ResolveSelectionRuntimeKey(selection.SelectionId, selection.SortOrder);
            runtimeValues.TryGetValue(runtimeKey, out var runtimeValue);
            var value = runtimeValue?.Value?.Trim();
            var valueTo = runtimeValue?.ValueTo?.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }
            var columnExpr = ColumnExpr(selection.SourceAlias, selection.ColumnName);
            var paramBase = $"p_sel_{runtimeKey}";
            var filterOperator = runtimeValue?.FilterOperator is >= 1 and <= 8
                ? runtimeValue.FilterOperator
                : selection.FilterOperator is >= 1 and <= 8 ? selection.FilterOperator : 7;
            switch (filterOperator)
            {
                case 1:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} = @{paramBase}");
                    break;
                case 7:
                    parameters[paramBase] = EscapeLikeLiteral(value);
                    clauses.Add($"CAST({columnExpr} AS NVARCHAR(MAX)) LIKE '%' + @{paramBase} + '%'");
                    break;
                case 2:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} <> @{paramBase}");
                    break;
                case 3:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} > @{paramBase}");
                    break;
                case 4:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} >= @{paramBase}");
                    break;
                case 5:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} < @{paramBase}");
                    break;
                case 6:
                    parameters[paramBase] = value;
                    clauses.Add($"{columnExpr} <= @{paramBase}");
                    break;
                case 8:
                    if (string.IsNullOrWhiteSpace(valueTo))
                    {
                        throw new ArgumentException($"区间筛选缺少结束值: {selection.SourceAlias}.{selection.ColumnName}");
                    }
                    parameters[$"{paramBase}_from"] = value;
                    parameters[$"{paramBase}_to"] = valueTo;
                    clauses.Add($"{columnExpr} BETWEEN @{paramBase}_from AND @{paramBase}_to");
                    break;
                case 9:
                    var inParts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    if (inParts.Length == 0)
                    {
                        throw new ArgumentException($"IN 筛选值无效: {selection.SourceAlias}.{selection.ColumnName}");
                    }
                    var inParams = new List<string>();
                    for (var i = 0; i < inParts.Length; i++)
                    {
                        var inKey = $"{paramBase}_{i}";
                        parameters[inKey] = inParts[i];
                        inParams.Add($"@{inKey}");
                    }
                    clauses.Add($"{columnExpr} IN ({string.Join(", ", inParams)})");
                    break;
                case 10:
                    clauses.Add($"{columnExpr} IS NULL");
                    break;
                case 11:
                    clauses.Add($"{columnExpr} IS NOT NULL");
                    break;
                default:
                    throw new ArgumentException($"不支持的筛选运算符: {filterOperator}");
            }
        }
        return string.Join(" AND ", clauses);
    }

    /// <summary>
    /// 运行时筛选值字典键（持久化行用 Id；预览无 Id 时用 -SortOrder）
    /// </summary>
    /// <param name="selectionId">筛选项主键</param>
    /// <param name="sortOrder">排序号</param>
    /// <returns>运行时键</returns>
    private static long ResolveSelectionRuntimeKey(long selectionId, int sortOrder) =>
        selectionId > 0 ? selectionId : -sortOrder;

    /// <summary>
    /// 转义 LIKE 模式中的通配符（%, _, [）
    /// </summary>
    /// <param name="value">用户输入</param>
    /// <returns>可安全用于 LIKE 的字面量</returns>
    private static string EscapeLikeLiteral(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return value
            .Replace("[", "[[]", StringComparison.Ordinal)
            .Replace("%", "[%]", StringComparison.Ordinal)
            .Replace("_", "[_]", StringComparison.Ordinal);
    }

    /// <summary>
    /// 将参数字典转为 SqlSugar SugarParameter 数组
    /// </summary>
    /// <param name="parameters">命名参数</param>
    /// <returns>SugarParameter 数组</returns>
    private static SugarParameter[] ToSugarParameters(IReadOnlyDictionary<string, object?> parameters)
    {
        return parameters
            .Select(pair =>
            {
                var name = pair.Key.StartsWith("@", StringComparison.Ordinal) ? pair.Key : $"@{pair.Key}";
                return new SugarParameter(name, pair.Value);
            })
            .ToArray();
    }

    /// <summary>
    /// 构建数据源别名 → 表名映射（别名唯一）
    /// </summary>
    /// <param name="sources">数据源列表</param>
    /// <returns>别名到表名的字典</returns>
    private static Dictionary<string, string> BuildAliasMap(IReadOnlyList<TaktStatQuerySourceItem> sources)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var source in sources)
        {
            ValidateAlias(source.SourceAlias);
            ValidateTable(source.TableName);
            if (!map.TryAdd(source.SourceAlias, source.TableName))
            {
                throw new ArgumentException($"数据源别名重复: {source.SourceAlias}");
            }
        }
        return map;
    }

    /// <summary>
    /// 解析输出列别名（显式 OutputAlias 或 源别名_列名）
    /// </summary>
    /// <param name="field">输出字段定义</param>
    /// <returns>输出列键名</returns>
    private static string ResolveOutputKey(TaktStatQueryFieldItem field)
    {
        if (!string.IsNullOrWhiteSpace(field.OutputAlias))
        {
            ValidateOutputAlias(field.OutputAlias);
            return field.OutputAlias;
        }
        var key = $"{field.SourceAlias}_{field.ColumnName}";
        ValidateOutputAlias(key);
        return key;
    }

    /// <summary>
    /// 构建 别名.列名 表达式（SqlSugar 字符串 API）
    /// </summary>
    /// <param name="sourceAlias">数据源别名</param>
    /// <param name="columnName">列名</param>
    /// <returns>别名.列名</returns>
    private static string ColumnExpr(string sourceAlias, string columnName)
    {
        ValidateAlias(sourceAlias);
        ValidateColumn(columnName);
        return $"{sourceAlias}.{columnName}";
    }

    /// <summary>
    /// 校验物理表名（小写蛇形，takt_ 前缀）
    /// </summary>
    /// <param name="tableName">表名</param>
    private static void ValidateTable(string tableName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tableName);
        if (!TableNamePattern.IsMatch(tableName))
        {
            throw new ArgumentException($"非法表名: {tableName}");
        }
    }

    /// <summary>
    /// 校验列名（小写蛇形）
    /// </summary>
    /// <param name="columnName">列名</param>
    private static void ValidateColumn(string columnName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(columnName);
        if (!ColumnNamePattern.IsMatch(columnName))
        {
            throw new ArgumentException($"非法列名: {columnName}");
        }
    }

    /// <summary>
    /// 校验数据源别名
    /// </summary>
    /// <param name="alias">别名</param>
    private static void ValidateAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        if (!AliasPattern.IsMatch(alias))
        {
            throw new ArgumentException($"非法数据源别名: {alias}");
        }
    }

    /// <summary>
    /// 校验 SELECT 输出列别名
    /// </summary>
    /// <param name="alias">输出别名</param>
    private static void ValidateOutputAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        if (!Regex.IsMatch(alias, @"^[A-Za-z][A-Za-z0-9_]{0,63}$"))
        {
            throw new ArgumentException($"非法输出别名: {alias}");
        }
    }
}

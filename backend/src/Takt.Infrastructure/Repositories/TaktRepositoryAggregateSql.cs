// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Repositories
// 文件名称：TaktRepositoryAggregateSql.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：仓储层聚合统计内部实现（中位数 SqlFunc + MySql/Sqlite 有序切片回退）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Repositories;

/// <summary>
/// 仓储聚合统计内部实现（供三级仓储复用）
/// </summary>
internal static class TaktRepositoryAggregateSql
{
    /// <summary>
    /// 按条件求字段中位数（库内 PERCENTILE_CONT 或 MySql/Sqlite 有序切片）
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数值字段类型</typeparam>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="query">已附加隔离与未删除过滤的查询</param>
    /// <param name="fieldSelector">聚合字段</param>
    /// <returns>中位数；无记录时为类型默认值</returns>
    public static async Task<TResult> MedianAsync<TEntity, TResult>(
        ISqlSugarClient db,
        ISugarQueryable<TEntity> query,
        Expression<Func<TEntity, TResult>> fieldSelector)
        where TEntity : class, new()
        where TResult : struct
    {
        ArgumentNullException.ThrowIfNull(db);
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(fieldSelector);

        if (await query.CountAsync() == 0)
        {
            return default;
        }

        var dbType = db.CurrentConnectionConfig.DbType;
        if (TaktSqlFuncMedian.SupportsNativePercentile(dbType))
        {
            var medianSelector = BuildMedianSelector(fieldSelector);
            var value = await query.Select(medianSelector).FirstAsync();
            return value ?? default;
        }

        return await MedianViaOrderedSliceAsync(query, fieldSelector);
    }

    /// <summary>
    /// 构建 Select(it =&gt; TaktSqlFuncMedian.Median(field)) 表达式
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">字段类型</typeparam>
    /// <param name="fieldSelector">字段选择器</param>
    /// <returns>中位数投影表达式</returns>
    private static Expression<Func<TEntity, TResult?>> BuildMedianSelector<TEntity, TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector)
        where TEntity : class, new()
        where TResult : struct
    {
        var parameter = fieldSelector.Parameters[0];
        var medianCall = Expression.Call(
            typeof(TaktSqlFuncMedian),
            nameof(TaktSqlFuncMedian.Median),
            [typeof(TResult)],
            fieldSelector.Body);
        return Expression.Lambda<Func<TEntity, TResult?>>(medianCall, parameter);
    }

    /// <summary>
    /// MySql/Sqlite 回退：排序后取中间 1～2 行再平均（与 PERCENTILE_CONT 连续型中位数语义对齐）
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">数值字段类型</typeparam>
    /// <param name="query">过滤后查询</param>
    /// <param name="fieldSelector">聚合字段</param>
    /// <returns>中位数</returns>
    private static async Task<TResult> MedianViaOrderedSliceAsync<TEntity, TResult>(
        ISugarQueryable<TEntity> query,
        Expression<Func<TEntity, TResult>> fieldSelector)
        where TEntity : class, new()
        where TResult : struct
    {
        var count = await query.CountAsync();
        if (count == 0)
        {
            return default;
        }

        var skip = checked((count - 1) / 2);
        var ordered = query.OrderBy(ToObjectExpression(fieldSelector));
        if (count % 2 == 1)
        {
            return await ordered.Skip(skip).Take(1).Select(fieldSelector).FirstAsync();
        }

        var middleValues = await ordered.Skip(skip).Take(2).Select(fieldSelector).ToListAsync();
        return AverageTwo(middleValues[0], middleValues[1]);
    }

    /// <summary>
    /// 两数平均（用于偶数行中位数）
    /// </summary>
    /// <typeparam name="TResult">数值类型</typeparam>
    /// <param name="left">左中位候选</param>
    /// <param name="right">右中位候选</param>
    /// <returns>平均值转换回 TResult</returns>
    private static TResult AverageTwo<TResult>(TResult left, TResult right) where TResult : struct
    {
        var average = (Convert.ToDecimal(left) + Convert.ToDecimal(right)) / 2m;
        return (TResult)Convert.ChangeType(average, typeof(TResult));
    }

    /// <summary>
    /// 将值类型字段选择器转为 OrderBy 可用的 object 投影
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TResult">字段类型</typeparam>
    /// <param name="fieldSelector">字段选择器</param>
    /// <returns>object 投影表达式</returns>
    private static Expression<Func<TEntity, object>> ToObjectExpression<TEntity, TResult>(
        Expression<Func<TEntity, TResult>> fieldSelector)
    {
        var body = Expression.Convert(fieldSelector.Body, typeof(object));
        return Expression.Lambda<Func<TEntity, object>>(body, fieldSelector.Parameters);
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktStatQueryExecutor.cs
// 创建时间：2026-06-19
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表 SqlSugar Queryable 原生执行器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Statistics;
using Takt.Shared.Validation;

namespace Takt.Infrastructure.Services;

/// <summary>
/// 定制报表 SqlSugar Queryable 原生执行器（ToPageListAsync / Take + ToDataTableAsync）
/// </summary>
public sealed class TaktStatQueryExecutor : ITaktStatQueryExecutor
{
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">租户 SqlSugar 上下文</param>
    public TaktStatQueryExecutor(TaktSqlSugarContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 分页查询报表数据
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="maxPageSize">pageSize 上限</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    public async Task<TaktStatQueryPageResult> ExecutePagedAsync(
        TaktStatQueryBuildRequest request,
        int pageIndex,
        int pageSize,
        int maxPageSize,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Min(maxPageSize, Math.Max(1, pageSize));
        var (queryable, metadata) = TaktStatQueryBuilder.Compile(_dbContext.Db, request);
        ValidateQueryableSql(queryable);
        RefAsync<int> totalCount = 0;
        var dynamicRows = await queryable.ToPageListAsync(pageIndex, pageSize, totalCount);
        cancellationToken.ThrowIfCancellationRequested();
        var rows = TaktStatQueryRowConverter.FromDynamicRows(dynamicRows, metadata.OutputKeys);
        return new TaktStatQueryPageResult
        {
            Total = totalCount.Value,
            Rows = rows,
            OutputKeys = metadata.OutputKeys,
            OutputLabels = metadata.OutputLabels,
        };
    }

    /// <summary>
    /// 按行数上限查询报表数据（导出）
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="maxRows">最大行数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行</returns>
    public async Task<TaktStatQueryPageResult> ExecuteTopAsync(
        TaktStatQueryBuildRequest request,
        int maxRows,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (maxRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxRows), "maxRows 必须大于 0");
        }
        cancellationToken.ThrowIfCancellationRequested();
        var (queryable, metadata) = TaktStatQueryBuilder.Compile(_dbContext.Db, request);
        ValidateQueryableSql(queryable);
        var dataTable = await queryable.Take(maxRows).ToDataTableAsync();
        cancellationToken.ThrowIfCancellationRequested();
        var rows = TaktStatQueryRowConverter.FromDataTable(dataTable, metadata.OutputKeys);
        return new TaktStatQueryPageResult
        {
            Total = rows.Count,
            Rows = rows,
            OutputKeys = metadata.OutputKeys,
            OutputLabels = metadata.OutputLabels,
        };
    }

    /// <summary>
    /// 校验 SqlSugar 生成的 SQL 符合只读策略
    /// </summary>
    /// <param name="queryable">已编译 Queryable</param>
    private static void ValidateQueryableSql(ISugarQueryable<object> queryable)
    {
        var sqlInfo = queryable.ToSql();
        TaktSqlExecutorValidator.Validate(sqlInfo.Key);
    }
}

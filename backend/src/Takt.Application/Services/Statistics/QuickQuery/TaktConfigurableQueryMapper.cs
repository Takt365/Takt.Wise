// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.QuickQuery
// 文件名称：TaktConfigurableQueryMapper.cs
// 创建时间：2026-06-13
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表实体映射为 TaktStatQueryBuildRequest
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Statistics.QuickQuery;
using Takt.Shared.Models.Statistics;

namespace Takt.Application.Services.Statistics.QuickQuery;

/// <summary>
/// 定制报表 编译请求映射（纯函数）
/// </summary>
internal static class TaktConfigurableQueryMapper
{
    /// <summary>
    /// 将定制报表定义与运行时筛选值映射为 SQL 编译请求
    /// </summary>
    /// <param name="entity">定制报表主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="sources">数据源</param>
    /// <param name="joins">关联</param>
    /// <param name="fields">输出字段</param>
    /// <param name="selections">筛选定义</param>
    /// <param name="groupBys">分组</param>
    /// <param name="orderBys">排序</param>
    /// <param name="runtimeSelectionValues">运行时筛选值</param>
    /// <returns>编译请求</returns>
    public static TaktStatQueryBuildRequest MapBuildRequest(
        TaktConfigurable entity,
        string tenantCode,
        string companyCode,
        IReadOnlyList<TaktConfigurableSource> sources,
        IReadOnlyList<TaktConfigurableJoin> joins,
        IReadOnlyList<TaktConfigurableField> fields,
        IReadOnlyList<TaktConfigurableSelection> selections,
        IReadOnlyList<TaktConfigurableGroupBy> groupBys,
        IReadOnlyList<TaktConfigurableOrderBy> orderBys,
        IReadOnlyDictionary<long, TaktStatQuerySelectionValue> runtimeSelectionValues)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        return new TaktStatQueryBuildRequest
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            DistinctRows = entity.DistinctRows,
            Sources = sources
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQuerySourceItem
                {
                    SourceAlias = x.SourceAlias,
                    TableName = x.TableName,
                    IsPrimary = x.IsPrimary,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            Joins = joins
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQueryJoinItem
                {
                    JoinType = x.JoinType,
                    LeftSourceAlias = x.LeftSourceAlias,
                    LeftColumnName = x.LeftColumnName,
                    RightSourceAlias = x.RightSourceAlias,
                    RightColumnName = x.RightColumnName,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            Fields = fields
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQueryFieldItem
                {
                    SourceAlias = x.SourceAlias,
                    ColumnName = x.ColumnName,
                    DisplayName = x.DisplayName,
                    OutputAlias = x.OutputAlias,
                    AggregateFunc = x.AggregateFunc,
                    IsVisible = x.IsVisible,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            Selections = selections
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQuerySelectionItem
                {
                    SelectionId = x.Id,
                    SourceAlias = x.SourceAlias,
                    ColumnName = x.ColumnName,
                    FilterOperator = x.FilterOperator,
                    IsRequired = x.IsRequired,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            GroupBys = groupBys
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQueryGroupByItem
                {
                    SourceAlias = x.SourceAlias,
                    ColumnName = x.ColumnName,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            OrderBys = orderBys
                .OrderBy(x => x.SortOrder)
                .Select(x => new TaktStatQueryOrderByItem
                {
                    SourceAlias = x.SourceAlias,
                    ColumnName = x.ColumnName,
                    SortDirection = x.SortDirection,
                    SortOrder = x.SortOrder,
                })
                .ToList(),
            RuntimeSelectionValues = runtimeSelectionValues,
        };
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktStatQueryExecutor.cs
// 创建时间：2026-06-19
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表 SqlSugar Queryable 原生执行器接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Statistics;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 定制报表 SqlSugar Queryable 原生执行器（非 Ado 手写 SQL）
/// </summary>
public interface ITaktStatQueryExecutor
{
    /// <summary>
    /// 分页查询报表数据
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="pageIndex">页码（从 1 开始）</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="maxPageSize">pageSize 上限</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>分页结果</returns>
    Task<TaktStatQueryPageResult> ExecutePagedAsync(
        TaktStatQueryBuildRequest request,
        int pageIndex,
        int pageSize,
        int maxPageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按行数上限查询报表数据（导出）
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="maxRows">最大行数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行</returns>
    Task<TaktStatQueryPageResult> ExecuteTopAsync(
        TaktStatQueryBuildRequest request,
        int maxRows,
        CancellationToken cancellationToken = default);
}

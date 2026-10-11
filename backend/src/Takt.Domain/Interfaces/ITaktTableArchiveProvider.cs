// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktTableArchiveProvider.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：同库按年数据归档提供者接口（Infrastructure 实现）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Code;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 同库按年数据归档提供者
/// </summary>
public interface ITaktTableArchiveProvider
{
    /// <summary>
    /// 预览将归档行数（不迁移）
    /// </summary>
    /// <param name="options">归档选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>预览结果</returns>
    Task<TaktTableArchivePreview> PreviewAsync(
        TaktTableArchiveOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 执行按年归档（基表 DELETE OUTPUT INTO 年分表 {table}_{yyyy}，分批）
    /// </summary>
    /// <param name="options">归档选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行结果</returns>
    Task<TaktTableArchiveResult> ArchiveAsync(
        TaktTableArchiveOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 按年份列表预建年分表（SELECT TOP 0 * INTO {table}_{year}，已存在则跳过）
    /// </summary>
    /// <param name="options">建表选项（含基表与租户库）</param>
    /// <param name="years">年份列表</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>已创建或已存在的年分表名</returns>
    Task<IReadOnlyList<string>> EnsureYearTablesAsync(
        TaktTableArchiveOptions options,
        IReadOnlyList<int> years,
        CancellationToken cancellationToken = default);
}

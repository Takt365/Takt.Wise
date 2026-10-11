// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktSqlExecutor.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：通用 SQL 执行器接口（只读脚本，与仓储共用 Ado 查询实现）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Options;

namespace Takt.Domain.Interfaces;

/// <summary>
/// 通用 SQL 执行器（Infrastructure 由 TaktSqlExecutor 实现；
/// 与 ITaktTenantRepository{TEntity}.QueryReadOnlySqlAsync 共用同一 Ado 查询路径）
/// </summary>
public interface ITaktSqlExecutor
{
    /// <summary>
    /// 执行 SQL，返回动态行（列名 → 值，列名大小写不敏感）
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="options">执行选项；为 null 时使用 TaktSqlExecuteOptions.ReadOnlyDefault</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    Task<IReadOnlyList<Dictionary<string, object>>> QueryAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        TaktSqlExecuteOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 执行 SQL 并返回首行；无结果时返回 null
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="options">执行选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>首行或 null</returns>
    Task<Dictionary<string, object>?> QueryFirstAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        TaktSqlExecuteOptions? options = null,
        CancellationToken cancellationToken = default);
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Services
// 文件名称：TaktSqlExecutor.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：通用 SQL 执行器（安全校验 + 与三级仓储共用 Ado 只读查询）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Interfaces;
using Takt.Infrastructure.Data.Context;
using Takt.Infrastructure.Repositories;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Infrastructure.Services;

/// <summary>
/// ITaktSqlExecutor 实现
/// 只读 SQL 经 TaktSqlExecutorValidator 校验后，由 TaktRepositoryReadOnlySql 执行（与 TaktTenantRepository{TEntity}.QueryReadOnlySqlAsync 相同路径）
/// </summary>
public sealed class TaktSqlExecutor : ITaktSqlExecutor
{
    /// <summary>
    /// 租户 SqlSugar 上下文（与现有仓储共用）
    /// </summary>
    private readonly TaktSqlSugarContext _dbContext;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dbContext">租户 SqlSugar 上下文</param>
    public TaktSqlExecutor(TaktSqlSugarContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 执行 SQL，返回动态行（列名大小写不敏感；执行前校验只读策略）
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="options">执行选项；为 null 时使用只读默认</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>结果行列表</returns>
    public async Task<IReadOnlyList<Dictionary<string, object>>> QueryAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        TaktSqlExecuteOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var script = sql?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(script))
        {
            return Array.Empty<Dictionary<string, object>>();
        }

        TaktSqlExecutorValidator.Validate(script, options);
        return await TaktRepositoryReadOnlySql.QueryAsync(_dbContext.Db, script, parameters, cancellationToken);
    }

    /// <summary>
    /// 执行 SQL 并返回首行；无结果时返回 null
    /// </summary>
    /// <param name="sql">SQL 文本</param>
    /// <param name="parameters">命名参数（可选）</param>
    /// <param name="options">执行选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>首行字典或 null</returns>
    public async Task<Dictionary<string, object>?> QueryFirstAsync(
        string sql,
        IReadOnlyDictionary<string, object?>? parameters = null,
        TaktSqlExecuteOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var rows = await QueryAsync(sql, parameters, options, cancellationToken);
        return rows.Count == 0 ? null : rows[0];
    }
}

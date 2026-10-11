// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Validation
// 文件名称：TaktSqlExecutorValidator.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：只读 SQL 脚本安全校验（供 ITaktSqlExecutor 与仓储 QueryReadOnlySqlAsync 调用前使用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Options;

namespace Takt.Shared.Validation;

/// <summary>
/// 只读 SQL 脚本安全校验器（禁止 DML/DDL 与多语句）
/// </summary>
public static class TaktSqlExecutorValidator
{
    /// <summary>
    /// 禁止出现在只读 SQL 中的关键字模式（整词匹配）
    /// </summary>
    private static readonly Regex ForbiddenSqlPattern = new(
        @"\b(INSERT|UPDATE|DELETE|DROP|ALTER|TRUNCATE|EXEC|EXECUTE|MERGE|CREATE|GRANT|REVOKE)\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    /// <summary>
    /// 按执行选项校验 SQL 文本
    /// </summary>
    /// <param name="sql">待执行的 SQL 文本</param>
    /// <param name="options">执行选项；为 null 时使用 TaktSqlExecuteOptions.ReadOnlyDefault</param>
    /// <exception cref="TaktBusinessException">SQL 为空、非只读或含禁止关键字时抛出</exception>
    public static void Validate(string sql, TaktSqlExecuteOptions? options = null)
    {
        var executeOptions = options ?? TaktSqlExecuteOptions.ReadOnlyDefault;
        var script = sql?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(script))
        {
            throw new TaktBusinessException("SQL 不能为空");
        }

        if (executeOptions.Mode == TaktSqlExecuteMode.ReadOnly)
        {
            ValidateReadOnly(script, executeOptions);
            return;
        }
        if (executeOptions.Mode == TaktSqlExecuteMode.NonQuery)
        {
            ValidateNonQuery(script);
        }
    }

    /// <summary>
    /// 非查询脚本校验：非空；禁止极其危险的语句
    /// </summary>
    /// <param name="script">已 Trim 的 SQL 文本</param>
    /// <exception cref="TaktBusinessException">含危险语句时抛出</exception>
    private static void ValidateNonQuery(string script)
    {
        if (Regex.IsMatch(
                script,
                @"\b(DROP\s+DATABASE|ALTER\s+DATABASE|SHUTDOWN)\b",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            throw new TaktBusinessException("SQL 脚本包含不允许的危险语句");
        }
    }

    /// <summary>
    /// 只读模式校验：必须以 SELECT/WITH 开头，可选禁止关键字与多语句检查
    /// </summary>
    /// <param name="script">已 Trim 的 SQL 文本</param>
    /// <param name="options">只读执行选项</param>
    /// <exception cref="TaktBusinessException">不符合只读策略时抛出</exception>
    private static void ValidateReadOnly(string script, TaktSqlExecuteOptions options)
    {
        var normalized = script.TrimStart();
        if (!normalized.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            && !normalized.StartsWith("WITH", StringComparison.OrdinalIgnoreCase))
        {
            throw new TaktBusinessException("只读 SQL 仅允许 SELECT 或 WITH 查询");
        }

        if (options.ValidateForbiddenKeywords && ForbiddenSqlPattern.IsMatch(script))
        {
            throw new TaktBusinessException("SQL 包含不允许的关键字");
        }

        if (!options.AllowMultipleStatements)
        {
            var semicolonParts = script.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (semicolonParts.Length > 1)
            {
                throw new TaktBusinessException("SQL 不允许多条语句");
            }
        }
    }
}

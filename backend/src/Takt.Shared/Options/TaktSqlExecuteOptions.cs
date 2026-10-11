// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktSqlExecuteOptions.cs
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：SQL 执行器选项（只读查询 / 脚本转数据等全局共用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Options;

/// <summary>
/// 只读 SQL 脚本执行选项（配合 ITaktSqlExecutor 与仓储 <c>QueryReadOnlySqlAsync</c>）
/// </summary>
public sealed class TaktSqlExecuteOptions
{
    /// <summary>
    /// 只读查询默认选项（单条 SELECT/WITH，校验禁止关键字）
    /// </summary>
    public static TaktSqlExecuteOptions ReadOnlyDefault { get; } = new()
    {
        Mode = TaktSqlExecuteMode.ReadOnly,
    };

    /// <summary>
    /// 非查询脚本默认选项（允许多语句；不校验只读禁止关键字）
    /// </summary>
    public static TaktSqlExecuteOptions NonQueryDefault { get; } = new()
    {
        Mode = TaktSqlExecuteMode.NonQuery,
        AllowMultipleStatements = true,
        ValidateForbiddenKeywords = false,
    };

    /// <summary>
    /// 执行模式
    /// </summary>
    public TaktSqlExecuteMode Mode { get; init; } = TaktSqlExecuteMode.ReadOnly;

    /// <summary>
    /// 是否允许多条以分号分隔的语句（默认 false）
    /// </summary>
    public bool AllowMultipleStatements { get; init; }

    /// <summary>
    /// 是否校验禁止关键字（默认 true）
    /// </summary>
    public bool ValidateForbiddenKeywords { get; init; } = true;
}

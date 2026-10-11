// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktQuartzSqlPathHelper.cs
// 创建时间：2026-07-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz SqlScript 路径格式校验（仅允许相对 wwwroot 的 .sql，禁止内联 SQL）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// Quartz SqlScript 相对 wwwroot 路径格式校验
/// </summary>
public static class TaktQuartzSqlPathHelper
{
    /// <summary>
    /// 是否为合法相对 wwwroot 的 .sql 路径（不含空白/换行/绝对路径/..；如 Quartz/sync_mat.sql）
    /// </summary>
    /// <param name="sqlScript">SqlScript 字段值</param>
    /// <returns>格式合法则为 true</returns>
    public static bool IsValidWwwRootRelativeSqlPath(string? sqlScript)
    {
        if (string.IsNullOrWhiteSpace(sqlScript))
        {
            return false;
        }
        var raw = sqlScript.Trim();
        if (raw.Length > 200
            || raw.Contains('\n', StringComparison.Ordinal)
            || raw.Contains('\r', StringComparison.Ordinal)
            || raw.Contains(' ', StringComparison.Ordinal)
            || raw.Contains("..", StringComparison.Ordinal)
            || !raw.EndsWith(".sql", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        var normalized = raw.Replace('\\', '/');
        if (Path.IsPathRooted(normalized)
            || normalized.StartsWith("/", StringComparison.Ordinal)
            || normalized.StartsWith("~/", StringComparison.Ordinal))
        {
            return false;
        }
        return normalized.IndexOfAny(Path.GetInvalidPathChars()) < 0;
    }
}

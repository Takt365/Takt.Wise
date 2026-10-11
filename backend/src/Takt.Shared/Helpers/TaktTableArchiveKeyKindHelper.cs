// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTableArchiveKeyKindHelper.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表归档键类型（字典 sys_archive_key_kind：yyyyMMddHHmmss/yyyyMM/yyyy）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Helpers;

/// <summary>
/// 归档键类型（字典 sys_archive_key_kind）标准日期格式码与物理表后缀
/// </summary>
/// <remarks>
/// 1=yyyyMMddHHmmss（例：…_20251010101000）；2=yyyyMM（例：…_202510）；3=yyyy（例：…_2025）。
/// </remarks>
public static class TaktTableArchiveKeyKindHelper
{
    /// <summary>
    /// yyyyMMddHHmmss
    /// </summary>
    public const int YyyyMmDdHhMmSs = 1;

    /// <summary>
    /// yyyyMM
    /// </summary>
    public const int YyyyMm = 2;

    /// <summary>
    /// yyyy（默认）
    /// </summary>
    public const int Yyyy = 3;

    /// <summary>
    /// 默认归档键类型
    /// </summary>
    public const int Default = Yyyy;

    /// <summary>
    /// 是否为合法字典值 1/2/3
    /// </summary>
    /// <param name="archiveKeyKind">字典 DictValue</param>
    /// <returns>是否合法</returns>
    public static bool IsKnown(int archiveKeyKind) =>
        archiveKeyKind is YyyyMmDdHhMmSs or YyyyMm or Yyyy;

    /// <summary>
    /// 字典值 → 标准日期格式码（用于归档名称后缀）
    /// </summary>
    /// <param name="archiveKeyKind">字典 DictValue</param>
    /// <returns>yyyyMMddHHmmss / yyyyMM / yyyy</returns>
    /// <exception cref="ArgumentOutOfRangeException">非法字典值</exception>
    public static string ToFormatCode(int archiveKeyKind) =>
        archiveKeyKind switch
        {
            YyyyMmDdHhMmSs => "yyyyMMddHHmmss",
            YyyyMm => "yyyyMM",
            Yyyy => "yyyy",
            _ => throw new ArgumentOutOfRangeException(nameof(archiveKeyKind), "归档键类型须为字典 sys_archive_key_kind 的 1/2/3")
        };

    /// <summary>
    /// 由时间点生成物理表后缀（标准日期格式）
    /// </summary>
    /// <param name="archiveKeyKind">字典 DictValue</param>
    /// <param name="at">业务时间</param>
    /// <returns>如 20251010101000 / 202510 / 2025</returns>
    /// <exception cref="ArgumentOutOfRangeException">非法字典值</exception>
    public static string BuildTableSuffix(int archiveKeyKind, DateTime at) =>
        archiveKeyKind switch
        {
            YyyyMmDdHhMmSs => at.ToString("yyyyMMddHHmmss"),
            YyyyMm => at.ToString("yyyyMM"),
            Yyyy => at.ToString("yyyy"),
            _ => throw new ArgumentOutOfRangeException(nameof(archiveKeyKind), "归档键类型须为字典 sys_archive_key_kind 的 1/2/3")
        };

    /// <summary>
    /// 生成归档物理表名：{base}_{suffix}
    /// </summary>
    /// <param name="baseTableName">基表名</param>
    /// <param name="archiveKeyKind">字典 DictValue</param>
    /// <param name="at">业务时间（决定后缀）</param>
    /// <returns>物理表名</returns>
    /// <exception cref="ArgumentException">表名非法</exception>
    /// <exception cref="ArgumentOutOfRangeException">类型非法</exception>
    public static string BuildArchiveTableName(string baseTableName, int archiveKeyKind, DateTime at)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseTableName);
        var baseName = baseTableName.Trim().ToLowerInvariant();
        var suffix = BuildTableSuffix(archiveKeyKind, at);
        var name = $"{baseName}_{suffix}";
        if (name.Length > 128)
        {
            throw new ArgumentException($"归档表名过长: {name}");
        }
        return name;
    }

    /// <summary>
    /// 按年归档时的物理表名（kind=yyyy 用年；其它 kind 取该年 1 月 1 日 00:00:00 生成后缀）
    /// </summary>
    /// <param name="baseTableName">基表名</param>
    /// <param name="archiveKeyKind">字典 DictValue</param>
    /// <param name="archiveYear">归档年</param>
    /// <returns>物理表名</returns>
    public static string BuildArchiveTableNameForYear(string baseTableName, int archiveKeyKind, int archiveYear)
    {
        if (archiveYear < 1970 || archiveYear > 2100)
        {
            throw new ArgumentOutOfRangeException(nameof(archiveYear), "归档年份无效");
        }
        if (archiveKeyKind == Yyyy)
        {
            return TaktYearShardTableHelper.BuildYearTableName(baseTableName, archiveYear);
        }
        return BuildArchiveTableName(baseTableName, archiveKeyKind, new DateTime(archiveYear, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }
}

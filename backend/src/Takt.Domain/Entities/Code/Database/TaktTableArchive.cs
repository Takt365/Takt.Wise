// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Code.Database
// 文件名称：TaktTableArchive.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据表归档（登记物理表与归档键列，供按年归档执行）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Code.Database;

/// <summary>
/// 数据表归档（按表登记归档键与热库保留年数）
/// </summary>
/// <remarks>
/// 产品名：数据表归档（降低主库存储与查询压力，将历史数据迁入年表；非横向分片扩展）。
/// 菜单/路由：table-archive；权限前缀 code:database:table:archive。
/// 归档表名按键类型后缀：{TableName}_{yyyyMMddHHmmss|yyyyMM|yyyy}（例：…_20251010101000 / …_202510 / …_2025）。
/// ArchiveKeyKind 默认 3=yyyy；服务层生成归档名称（如 takt_xxx_yyyy）。
/// 热库保留年数固定为 1：仅允许归档 currentYear-1 及更早（例：2026 只能归档≤2025）。
/// </remarks>
[SugarTable("takt_code_database_table_archive", "数据表归档配置表")]
[SugarIndex("ix_table_archive_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_table_archive_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_table_archive_table_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TableName), OrderByType.Asc, true)]
public class TaktTableArchive : TaktCompanyEntityBase
{
    /// <summary>
    /// 目标租户（3 位；与 ConnectionStrings:Tenant_{code} 对齐，供 Schema introspect）
    /// </summary>
    [SugarColumn(ColumnName = "target_tenant_code", ColumnDescription = "目标租户", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名（与 DatabaseInfos DisplayName 一致）
    /// </summary>
    [SugarColumn(ColumnName = "target_database_name", ColumnDescription = "目标数据库", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 物理表名（须以 takt_ 开头；长度放宽至 128，因制造域表名常超 40）
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "物理表名", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 归档键列名（如 costing_date；小写蛇形，与物理列一致）
    /// </summary>
    [SugarColumn(ColumnName = "archive_key_column", ColumnDescription = "归档键列", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string ArchiveKeyColumn { get; set; } = string.Empty;

    /// <summary>
    /// 归档键类型（字典 sys_archive_key_kind；yyyyMMddHHmmss/yyyyMM/yyyy 等）
    /// </summary>
    [SugarColumn(ColumnName = "archive_key_kind", ColumnDescription = "归档键类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "3")]
    public int ArchiveKeyKind { get; set; } = 3;

    /// <summary>
    /// 热库保留年数（固定为 1；仅允许归档 currentYear-1 及更早）
    /// </summary>
    [SugarColumn(ColumnName = "retain_hot_years", ColumnDescription = "热库保留年数", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int RetainHotYears { get; set; } = 1;

    /// <summary>
    /// 归档名称（物理表名_格式码，如 takt_xxx_yyyy；由服务层写入）
    /// </summary>
    [SugarColumn(ColumnName = "archive_name", ColumnDescription = "归档名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ArchiveName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 状态（字典 sys_normal_disable；1=启用 0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "archive_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ArchiveStatus { get; set; } = 1;
}

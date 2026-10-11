// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Code.Database
// 文件名称：TaktTableClone.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：整表克隆实体（持久化源/目标范围，一行一张表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Code.Database;

/// <summary>
/// 整表克隆（单张表一行）
/// </summary>
/// <remarks>
/// 每项落一行：源/目标范围、目标备份及克隆行数。执行选项仅存在于 Dto，不入库。
/// 数据隔离：租户 + 公司（TaktCompanyEntityBase）。
/// </remarks>
[SugarTable("takt_code_database_table_clone", "整表克隆表")]
[SugarIndex("ix_table_clone_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_table_clone_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_table_clone_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
[SugarIndex("ix_table_clone_source_table", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceTableName), OrderByType.Asc, false)]
[SugarIndex("ix_table_clone_target_table", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TargetTableName), OrderByType.Asc, false)]
[SugarIndex("ix_table_clone_backup_table", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BackupTableName), OrderByType.Asc, false)]
public class TaktTableClone : TaktCompanyEntityBase
{
    /// <summary>
    /// 源租户编码（3 位）
    /// </summary>
    [SugarColumn(ColumnName = "source_tenant_code", ColumnDescription = "源租户编码", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string SourceTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 源数据库展示名
    /// </summary>
    [SugarColumn(ColumnName = "source_database_name", ColumnDescription = "源数据库", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string SourceDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 源物理表名
    /// </summary>
    [SugarColumn(ColumnName = "source_table_name", ColumnDescription = "源数据表", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string SourceTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位）
    /// </summary>
    [SugarColumn(ColumnName = "target_tenant_code", ColumnDescription = "目标租户", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    [SugarColumn(ColumnName = "target_database_name", ColumnDescription = "目标数据库", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 目标物理表名
    /// </summary>
    [SugarColumn(ColumnName = "target_table_name", ColumnDescription = "目标数据表", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string TargetTableName { get; set; } = string.Empty;

    /// <summary>
    /// 目标备份表名（克隆前 SELECT INTO 生成的物理表）
    /// </summary>
    [SugarColumn(ColumnName = "backup_table_name", ColumnDescription = "目标备份表名", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? BackupTableName { get; set; }

    /// <summary>
    /// 目标备份所在数据库展示名（备份位置）
    /// </summary>
    [SugarColumn(ColumnName = "target_backup_database_name", ColumnDescription = "目标备份所在库", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? TargetBackupDatabaseName { get; set; }

    /// <summary>
    /// 目标备份行数
    /// </summary>
    [SugarColumn(ColumnName = "backed_up_row_count", ColumnDescription = "目标备份行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BackedUpRowCount { get; set; } = 0;

    /// <summary>
    /// 目标清空行数（备份后 TRUNCATE 的行数）
    /// </summary>
    [SugarColumn(ColumnName = "cleared_row_count", ColumnDescription = "目标清空行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClearedRowCount { get; set; } = 0;

    /// <summary>
    /// 源表行数
    /// </summary>
    [SugarColumn(ColumnName = "source_row_count", ColumnDescription = "源表行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SourceRowCount { get; set; } = 0;

    /// <summary>
    /// 实际写入目标表行数
    /// </summary>
    [SugarColumn(ColumnName = "cloned_row_count", ColumnDescription = "克隆行数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ClonedRowCount { get; set; } = 0;
}

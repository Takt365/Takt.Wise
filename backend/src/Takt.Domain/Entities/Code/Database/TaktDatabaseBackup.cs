// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Code.Database
// 文件名称：TaktDatabaseBackup.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份执行配置（可复用；每次执行结果写入 TaktBackupLog，本表仅保留最近状态摘要）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;

namespace Takt.Domain.Entities.Code.Database;

/// <summary>
/// 数据库备份执行配置
/// </summary>
/// <remarks>
/// 职责：可复用的备份目标与路径配置（执行侧）。
/// 完整审计（路径/大小/错误/起止时间）只写入 Statistics.Logging.TaktBackupLog，禁止在本表重复落结果明细。
/// BackupType：1=Full Sync 2=Delta Sync。
/// BackupPathType：1=本地(服务器端) 2=文件服务器(UNC) 3=FTP 4=客户端。
/// 执行：立即执行 / 后台调度均创建定时任务。
/// BackupStatus：0=待执行 1=执行中 2=成功 3=失败 4=已调度（最近一次执行摘要）。
/// ExecuteMode：1=立即 2=后台。
/// </remarks>
[SugarTable("takt_code_database_backup", "数据库备份表")]
[SugarIndex("ix_database_backup_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_database_backup_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_database_backup_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BackupCode), OrderByType.Asc, true)]
[SugarIndex("ix_database_backup_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
public class TaktDatabaseBackup : TaktCompanyEntityBase
{
    /// <summary>
    /// 备份编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 备份编码规则生成并展示，非手输；单据类型菜单：数据库备份）
    /// </summary>
    [SugarColumn(ColumnName = "backup_code", ColumnDescription = "备份编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string BackupCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标租户（3 位，对应 ConnectionStrings:Tenant_{code}）
    /// </summary>
    [SugarColumn(ColumnName = "target_tenant_code", ColumnDescription = "目标租户", ColumnDataType = "varchar", Length = 3, IsNullable = false)]
    public string TargetTenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库展示名
    /// </summary>
    [SugarColumn(ColumnName = "target_database_name", ColumnDescription = "目标数据库", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string TargetDatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 备份类型（1=Full Sync 2=Delta Sync）
    /// </summary>
    [SugarColumn(ColumnName = "backup_type", ColumnDescription = "备份类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int BackupType { get; set; } = 1;

    /// <summary>
    /// 执行方式（1=立即 2=后台）
    /// </summary>
    [SugarColumn(ColumnName = "execute_mode", ColumnDescription = "执行方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ExecuteMode { get; set; } = 1;

    /// <summary>
    /// 备份路径类型（1=本地服务器端 2=文件服务器 3=FTP 4=客户端）
    /// </summary>
    [SugarColumn(ColumnName = "backup_path_type", ColumnDescription = "备份路径类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "4")]
    public int BackupPathType { get; set; } = 4;

    /// <summary>
    /// 目标备份目录（服务器本地绝对路径 / UNC / FTP 远程目录 / 客户端目录标识）
    /// </summary>
    [SugarColumn(ColumnName = "backup_path", ColumnDescription = "备份目录", ColumnDataType = "nvarchar", Length = 500, IsNullable = false)]
    public string BackupPath { get; set; } = string.Empty;

    /// <summary>
    /// 网络主机或 FTP 服务器名称（本地可空）
    /// </summary>
    [SugarColumn(ColumnName = "backup_host", ColumnDescription = "备份主机", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
    public string? BackupHost { get; set; }

    /// <summary>
    /// FTP 端口（默认 21；非 FTP 可空）
    /// </summary>
    [SugarColumn(ColumnName = "backup_port", ColumnDescription = "备份端口", ColumnDataType = "int", IsNullable = true)]
    public int? BackupPort { get; set; }

    /// <summary>
    /// 网络/FTP 用户名
    /// </summary>
    [SugarColumn(ColumnName = "backup_user_name", ColumnDescription = "备份用户名", ColumnDataType = "varchar", Length = 100, IsNullable = true)]
    public string? BackupUserName { get; set; }

    /// <summary>
    /// 网络/FTP 密码密文（可逆加密）
    /// </summary>
    [SugarColumn(ColumnName = "backup_password", ColumnDescription = "备份密码密文", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? BackupPassword { get; set; }

    /// <summary>
    /// 备份文件名（不含目录，含 .bak；默认 库名_日期戳.bak）
    /// </summary>
    [SugarColumn(ColumnName = "backup_file_name", ColumnDescription = "备份文件名", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string BackupFileName { get; set; } = string.Empty;

    /// <summary>
    /// 计划执行时间（后台调度）
    /// </summary>
    [SugarColumn(ColumnName = "scheduled_at", ColumnDescription = "计划执行时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// 最近一次执行时间（摘要；明细见 TaktBackupLog）
    /// </summary>
    [SugarColumn(ColumnName = "last_run_at", ColumnDescription = "最近执行时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastRunAt { get; set; }

    /// <summary>
    /// 关联 Quartz 任务主键（后台执行时）
    /// </summary>
    [SugarColumn(ColumnName = "quartz_task_id", ColumnDescription = "Quartz任务Id", ColumnDataType = "bigint", IsNullable = true)]
    public long? QuartzTaskId { get; set; }

    /// <summary>
    /// 备份状态（0=待执行 1=执行中 2=成功 3=失败 4=已调度；最近一次摘要）
    /// </summary>
    [SugarColumn(ColumnName = "backup_status", ColumnDescription = "备份状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BackupStatus { get; set; }
}

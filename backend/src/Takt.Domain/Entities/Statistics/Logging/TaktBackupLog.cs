// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktBackupLog.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：通用备份完整审计日志（每次执行一条；与业务执行配置分离，禁止结果字段回写到配置表）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 备份日志（完整审计）
/// </summary>
/// <remarks>
/// 职责：每次备份执行的完整结果（路径/大小/状态/起止/错误等）。
/// 与业务「执行配置」分离：如数据库备份配置见 TaktDatabaseBackup（仅配置 + 最近状态摘要）。
/// BackupKind 区分场景（database / file / config）；业务专用扩展走 ExtField。
/// </remarks>
[SugarTable("takt_statistics_logging_backup_log", "备份日志表")]
[SugarIndex("ix_backup_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_backup_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_backup_log_kind", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BackupKind), OrderByType.Asc, false)]
[SugarIndex("ix_backup_log_source", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceId), OrderByType.Asc, false)]
[SugarIndex("ix_backup_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
public class TaktBackupLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 备份种类（小写，如 database / file / config）
    /// </summary>
    [SugarColumn(ColumnName = "backup_kind", ColumnDescription = "备份种类", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string BackupKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（备份配置 Id、任务号等，统一字符串）
    /// </summary>
    [SugarColumn(ColumnName = "source_id", ColumnDescription = "来源业务键", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源编码快照（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "source_code", ColumnDescription = "来源编码", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// 目标名称（库展示名、目标标签等）
    /// </summary>
    [SugarColumn(ColumnName = "target_name", ColumnDescription = "目标名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 目标范围（可选；如租户码、公司码、路径根等）
    /// </summary>
    [SugarColumn(ColumnName = "target_scope", ColumnDescription = "目标范围", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string TargetScope { get; set; } = string.Empty;

    /// <summary>
    /// 同步模式快照（字典 sys_backup_sync_mode；1=完整 2=增量）
    /// </summary>
    [SugarColumn(ColumnName = "sync_mode", ColumnDescription = "同步模式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int SyncMode { get; set; } = 1;

    /// <summary>
    /// 执行方式快照（字典 sys_backup_execute_mode；1=立即 2=后台）
    /// </summary>
    [SugarColumn(ColumnName = "execute_mode", ColumnDescription = "执行方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ExecuteMode { get; set; } = 1;

    /// <summary>
    /// 路径类型快照（字典 sys_backup_path_type；0=无 1=本地 2=网络 3=FTP）
    /// </summary>
    [SugarColumn(ColumnName = "path_type", ColumnDescription = "路径类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PathType { get; set; } = 0;

    /// <summary>
    /// 执行后结果路径
    /// </summary>
    [SugarColumn(ColumnName = "result_path", ColumnDescription = "结果路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? ResultPath { get; set; }

    /// <summary>
    /// 结果大小（字节）
    /// </summary>
    [SugarColumn(ColumnName = "file_size_bytes", ColumnDescription = "文件大小字节", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 运行状态（字典 sys_job_run_status；0=进行中 1=成功 2=失败）
    /// </summary>
    [SugarColumn(ColumnName = "run_status", ColumnDescription = "运行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int RunStatus { get; set; } = 0;

    /// <summary>
    /// 失败错误信息
    /// </summary>
    [SugarColumn(ColumnName = "error_message", ColumnDescription = "错误信息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "started_at", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "finished_at", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? FinishedAt { get; set; }
}

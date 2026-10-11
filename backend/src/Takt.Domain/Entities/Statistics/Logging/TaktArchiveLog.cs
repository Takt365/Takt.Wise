// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktArchiveLog.cs
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：通用归档完整审计日志（每次执行一条；与业务执行配置分离，策略表不落执行结果）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// 归档日志（完整审计）
/// </summary>
/// <remarks>
/// 职责：每次归档执行的完整结果（源/目标/数量/状态/起止/错误等）。
/// 与业务「执行配置」分离：表级归档配置见 TaktTableArchive（仅配置，不落结果明细）。
/// ArchiveKind 区分场景（table.year / file / attachment）；业务专用扩展走 ExtField。
/// </remarks>
[SugarTable("takt_statistics_logging_archive_log", "归档日志表")]
[SugarIndex("ix_archive_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_archive_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_archive_log_kind", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ArchiveKind), OrderByType.Asc, false)]
[SugarIndex("ix_archive_log_source", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SourceId), OrderByType.Asc, false)]
[SugarIndex("ix_archive_log_created_at", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CreatedAt), OrderByType.Desc, false)]
public class TaktArchiveLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 归档种类（小写点号分段，如 table.year / file / attachment）
    /// </summary>
    [SugarColumn(ColumnName = "archive_kind", ColumnDescription = "归档种类", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string ArchiveKind { get; set; } = string.Empty;

    /// <summary>
    /// 来源业务键（策略 Id、单据号等，统一字符串）
    /// </summary>
    [SugarColumn(ColumnName = "source_id", ColumnDescription = "来源业务键", ColumnDataType = "varchar", Length = 40, IsNullable = false)]
    public string SourceId { get; set; } = string.Empty;

    /// <summary>
    /// 来源名称（表名、路径、资源名等）
    /// </summary>
    [SugarColumn(ColumnName = "source_name", ColumnDescription = "来源名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// 归档目标名称（年分表名、归档路径等）
    /// </summary>
    [SugarColumn(ColumnName = "target_name", ColumnDescription = "目标名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TargetName { get; set; } = string.Empty;

    /// <summary>
    /// 归档年份（按年归档时填写；其它场景可空）
    /// </summary>
    [SugarColumn(ColumnName = "archive_year", ColumnDescription = "归档年份", ColumnDataType = "int", IsNullable = true)]
    public int? ArchiveYear { get; set; }

    /// <summary>
    /// 归档前匹配数量（行/文件/对象）
    /// </summary>
    [SugarColumn(ColumnName = "source_count", ColumnDescription = "源匹配数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SourceCount { get; set; } = 0;

    /// <summary>
    /// 实际归档数量
    /// </summary>
    [SugarColumn(ColumnName = "archived_count", ColumnDescription = "归档数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ArchivedCount { get; set; } = 0;

    /// <summary>
    /// 源侧删除数量（热区清理等；无删除则为 0）
    /// </summary>
    [SugarColumn(ColumnName = "deleted_count", ColumnDescription = "删除数量", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DeletedCount { get; set; } = 0;

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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktQuartzTask.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 定时任务实体（Foundation 域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Statistics.Logging;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// Quartz 定时任务实体
/// </summary>
[SugarTable("takt_foundation_quartz_task", "定时任务表")]
[SugarIndex("ix_quartz_task_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_task_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_task_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(TaskCode), OrderByType.Asc, true)]
public class TaktQuartzTask : TaktCompanyEntityBase
{
    /// <summary>
    /// 任务编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "task_code", ColumnDescription = "任务编码", ColumnDataType = "varchar", Length = 50, IsNullable = false)]
    public string TaskCode { get; set; } = string.Empty;
    /// <summary>
    /// 任务名称
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TaskName { get; set; } = string.Empty;
    /// <summary>
    /// Quartz Job 名称
    /// </summary>
    [SugarColumn(ColumnName = "job_name", ColumnDescription = "Job名称", ColumnDataType = "varchar", Length = 100, IsNullable = false)]
    public string JobName { get; set; } = string.Empty;
    /// <summary>
    /// Quartz Job 分组（字典 sys_quartz_job_group 的 DictValue）
    /// </summary>
    [SugarColumn(ColumnName = "job_group", ColumnDescription = "Job分组", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "default")]
    public string JobGroup { get; set; } = "default";
    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type 的 DictValue：assembly=程序集、http=网络请求、sql=SQL语句）
    /// </summary>
    [SugarColumn(ColumnName = "task_type", ColumnDescription = "任务类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "assembly")]
    public string TaskType { get; set; } = "assembly";
    /// <summary>
    /// 程序集名称（任务类型为程序集时使用）
    /// </summary>
    [SugarColumn(ColumnName = "assembly_name", ColumnDescription = "程序集名称", ColumnDataType = "nvarchar", Length = 255, IsNullable = false, DefaultValue = "''")]
    public string AssemblyName { get; set; } = string.Empty;
    /// <summary>
    /// 任务类名（任务类型为程序集时使用）
    /// </summary>
    [SugarColumn(ColumnName = "class_name", ColumnDescription = "任务类名", ColumnDataType = "nvarchar", Length = 255, IsNullable = false, DefaultValue = "''")]
    public string ClassName { get; set; } = string.Empty;
    /// <summary>
    /// API 执行地址（任务类型为网络请求时使用）
    /// </summary>
    [SugarColumn(ColumnName = "api_url", ColumnDescription = "API执行地址", ColumnDataType = "nvarchar", Length = 255, IsNullable = true)]
    public string? ApiUrl { get; set; }
    /// <summary>
    /// 网络请求方式（GET/POST 等）
    /// </summary>
    [SugarColumn(ColumnName = "request_method", ColumnDescription = "网络请求方式", ColumnDataType = "varchar", Length = 10, IsNullable = true)]
    public string? RequestMethod { get; set; }
    /// <summary>
    /// SQL 脚本路径（任务类型为 SQL 时使用；只可填相对 wwwroot 的 .sql 路径如 Quartz/sync_mat.sql，禁止内联 SQL）
    /// </summary>
    [SugarColumn(ColumnName = "sql_script", ColumnDescription = "SQL脚本路径", ColumnDataType = "varchar", Length = 200, IsNullable = true)]
    public string? SqlScript { get; set; }
    /// <summary>
    /// 触发器类型（字典 sys_quartz_trigger_type；0=Simple 1=Cron）
    /// </summary>
    [SugarColumn(ColumnName = "trigger_type", ColumnDescription = "触发器类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TriggerType { get; set; } = 1;
    /// <summary>
    /// Cron 表达式（触发器类型为 Cron 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "cron_expression", ColumnDescription = "Cron表达式", ColumnDataType = "varchar", Length = 100, IsNullable = false, DefaultValue = "''")]
    public string CronExpression { get; set; } = string.Empty;
    /// <summary>
    /// 执行间隔时间（秒，触发器类型为 Simple 时使用）
    /// </summary>
    [SugarColumn(ColumnName = "interval_seconds", ColumnDescription = "执行间隔时间（秒）", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IntervalSeconds { get; set; } = 0;
    /// <summary>
    /// 执行参数
    /// </summary>
    [SugarColumn(ColumnName = "execute_params", ColumnDescription = "执行参数", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? ExecuteParams { get; set; }
    /// <summary>
    /// 是否允许并发执行（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "concurrent", ColumnDescription = "并发", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Concurrent { get; set; } = 0;
    /// <summary>
    /// Misfire 策略（字典 sys_quartz_misfire_policy；0=默认 1=忽略 2=立即触发 3=不触发）
    /// </summary>
    [SugarColumn(ColumnName = "misfire_policy", ColumnDescription = "Misfire策略", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MisfirePolicy { get; set; } = 0;
    /// <summary>
    /// 首次执行（调度生效开始时间）
    /// </summary>
    [SugarColumn(ColumnName = "first_run_at", ColumnDescription = "首次执行", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? FirstRunAt { get; set; }
    /// <summary>
    /// 执行次数
    /// </summary>
    [SugarColumn(ColumnName = "execute_count", ColumnDescription = "执行次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ExecuteCount { get; set; } = 0;
    /// <summary>
    /// 上次执行
    /// </summary>
    [SugarColumn(ColumnName = "last_run_at", ColumnDescription = "上次执行", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? LastRunAt { get; set; }
    /// <summary>
    /// 下次执行
    /// </summary>
    [SugarColumn(ColumnName = "next_run_at", ColumnDescription = "下次执行", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? NextRunAt { get; set; }
    /// <summary>
    /// 任务描述
    /// </summary>
    [SugarColumn(ColumnName = "task_description", ColumnDescription = "任务描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? TaskDescription { get; set; }
    /// <summary>
    /// 任务状态（字典 sys_quartz_task_status；0=正常 1=暂停）
    /// </summary>
    [SugarColumn(ColumnName = "task_status", ColumnDescription = "任务状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int TaskStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 关联的任务执行日志列表（主子表关系：QuartzTaskId）
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToMany, nameof(TaktQuartzLog.QuartzTaskId))]
    public List<TaktQuartzLog>? QuartzLogs { get; set; }
}

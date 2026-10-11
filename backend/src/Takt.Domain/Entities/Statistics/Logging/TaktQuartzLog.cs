// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Statistics.Logging
// 文件名称：TaktQuartzLog.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务执行日志实体（Statistics.Logging 域）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities.Foundation;

namespace Takt.Domain.Entities.Statistics.Logging;

/// <summary>
/// Quartz 任务执行日志实体
/// </summary>
/// <remarks>
/// 业务字符串字段均非空：无参数/无错误/无消息时用空串。
/// JobGroup / TaskType 存字典 DictValue（sys_quartz_job_group、sys_quartz_task_type），由前端选择后写入任务并快照到日志。
/// </remarks>
[SugarTable("takt_statistics_logging_quartz_log", "任务执行日志表")]
[SugarIndex("ix_quartz_log_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_task_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QuartzTaskId), OrderByType.Asc, false)]
[SugarIndex("ix_quartz_log_execute_time", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecuteTime), OrderByType.Desc, false)]
[SugarIndex("ix_quartz_log_execute_status", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecuteStatus), OrderByType.Asc, false)]
public class TaktQuartzLog : TaktCompanyEntityBase
{
    /// <summary>
    /// 关联定时任务 ID（选项 TaktQuartzTasks/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "quartz_task_id", ColumnDescription = "定时任务ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QuartzTaskId { get; set; }

    /// <summary>
    /// 任务名称（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务组名（字典 sys_quartz_job_group；DictValue=编码）
    /// </summary>
    [SugarColumn(ColumnName = "job_group", ColumnDescription = "任务组名", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "default")]
    public string JobGroup { get; set; } = "default";

    /// <summary>
    /// 任务类型（字典 sys_quartz_task_type；DictValue=assembly/http/sql）
    /// </summary>
    [SugarColumn(ColumnName = "task_type", ColumnDescription = "任务类型", ColumnDataType = "varchar", Length = 40, IsNullable = false, DefaultValue = "assembly")]
    public string TaskType { get; set; } = "assembly";

    /// <summary>
    /// 执行时间
    /// </summary>
    [SugarColumn(ColumnName = "execute_time", ColumnDescription = "执行时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ExecuteTime { get; set; }

    /// <summary>
    /// 执行耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnName = "execute_duration", ColumnDescription = "执行耗时（毫秒）", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ExecuteDuration { get; set; }

    /// <summary>
    /// 执行参数（无参数为空串）
    /// </summary>
    [SugarColumn(ColumnName = "execute_params", ColumnDescription = "执行参数", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string ExecuteParams { get; set; } = string.Empty;

    /// <summary>
    /// 执行消息（无消息为空串）
    /// </summary>
    [SugarColumn(ColumnName = "execute_message", ColumnDescription = "执行消息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string ExecuteMessage { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息（成功为空串）
    /// </summary>
    [SugarColumn(ColumnName = "error_info", ColumnDescription = "错误信息", ColumnDataType = "nvarchar", Length = 2000, IsNullable = false)]
    public string ErrorInfo { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器 IP
    /// </summary>
    [SugarColumn(ColumnName = "execute_ip", ColumnDescription = "执行机器IP", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ExecuteIp { get; set; } = string.Empty;

    /// <summary>
    /// 执行机器名
    /// </summary>
    [SugarColumn(ColumnName = "execute_host", ColumnDescription = "执行机器名", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string ExecuteHost { get; set; } = string.Empty;

    /// <summary>
    /// 执行状态（字典 sys_success_fail_status；0=失败 1=成功）
    /// </summary>
    [SugarColumn(ColumnName = "execute_status", ColumnDescription = "执行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ExecuteStatus { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 关联的定时任务
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(QuartzTaskId))]
    public TaktQuartzTask? QuartzTask { get; set; }
}

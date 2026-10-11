#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStep.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工步执行明细（步骤确认与结果）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工步执行明细实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_exec_step", "SOP工步执行明细表")]
[SugarIndex("ix_sop_exec_step_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_exec_step_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_exec_step_exec", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecId), OrderByType.Asc, false)]
public class TaktSopExecStep : TaktCompanyEntityBase
{
    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_id", ColumnDescription = "执行追溯ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "step_id", ColumnDescription = "工步ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步序号快照
    /// </summary>
    [SugarColumn(ColumnName = "step_no", ColumnDescription = "工步序号", ColumnDataType = "int", IsNullable = false)]
    public int StepNo { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "started_at", ColumnDescription = "开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime StartedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "ended_at", ColumnDescription = "结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// 工步结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    [SugarColumn(ColumnName = "step_result", ColumnDescription = "工步结果", ColumnDataType = "int", IsNullable = true)]
    public int? StepResult { get; set; }

    /// <summary>
    /// 确认人 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by", ColumnDescription = "确认人ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ConfirmedBy { get; set; }
    /// <summary>
    /// 确认人名称（冗余：按 ConfirmedBy 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_by_name", ColumnDescription = "确认人名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ConfirmedByName { get; set; }

    /// <summary>
    /// 确认时间
    /// </summary>
    [SugarColumn(ColumnName = "confirmed_at", ColumnDescription = "确认时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// 是否禁止下一步（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "block_next_step", ColumnDescription = "是否禁止下一步", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BlockNextStep { get; set; } = 0;

    /// <summary>
    /// 执行追溯
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ExecId))]
    public TaktSopExec? Exec { get; set; }
}

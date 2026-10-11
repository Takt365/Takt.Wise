#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopArgument.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 作业实际参数
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 作业参数实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_param", "SOP作业参数表")]
[SugarIndex("ix_sop_param_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_param_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_param_exec", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ExecId), OrderByType.Asc, false)]
public class TaktSopArgument : TaktCompanyEntityBase
{
    /// <summary>
    /// 执行追溯 ID（选项 TaktSopExecs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_id", ColumnDescription = "执行追溯ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ExecId { get; set; }

    /// <summary>
    /// 工步执行明细 ID（选项 TaktSopExecSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "exec_step_id", ColumnDescription = "工步执行明细ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ExecStepId { get; set; }

    /// <summary>
    /// 工序参数定义 ID（选项 TaktRoutingItemArguments/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_parameter_id", ColumnDescription = "工序参数定义ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemParameterId { get; set; }

    /// <summary>
    /// 参数编码
    /// </summary>
    [SugarColumn(ColumnName = "param_code", ColumnDescription = "参数编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ParamCode { get; set; } = string.Empty;

    /// <summary>
    /// 实际值
    /// </summary>
    [SugarColumn(ColumnName = "actual_value", ColumnDescription = "实际值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false)]
    public decimal ActualValue { get; set; }

    /// <summary>
    /// 是否超差（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_out_of_range", ColumnDescription = "是否超差", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsOutOfRange { get; set; } = 0;

    /// <summary>
    /// 记录时间
    /// </summary>
    [SugarColumn(ColumnName = "recorded_at", ColumnDescription = "记录时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime RecordedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 执行追溯
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ExecId))]
    public TaktSopExec? Exec { get; set; }
}

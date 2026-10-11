#nullable enable
// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExec.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP 工位执行追溯（工单/SN/版本/员工/自检）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
namespace Takt.Domain.Entities.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP 工位执行追溯实体
/// </summary>
[SugarTable("takt_logistics_manufacturing_sop_exec", "SOP工位执行表")]
[SugarIndex("ix_sop_exec_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_sop_exec_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_exec_work_order", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(WorkOrderCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_exec_serial", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SerialNumber), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_sop_exec_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktSopExec : TaktCompanyEntityBase
{

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "production_order_id", ColumnDescription = "生产工单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ProductionOrderId { get; set; }

    /// <summary>
    /// MES 工单号（冗余，便于追溯查询）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品序列号 SN
    /// </summary>
    [SugarColumn(ColumnName = "serial_number", ColumnDescription = "产品序列号", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// 产品/机种物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_id", ColumnDescription = "工序ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RoutingItemId { get; set; }

    /// <summary>
    /// 工艺段类型（字典 logistics_manufacturing_process_segment_type；1=SMT，2=自插，3=手插，4=修正，5=总装）
    /// </summary>
    [SugarColumn(ColumnName = "process_segment_type", ColumnDescription = "工艺段类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ProcessSegmentType { get; set; } = 1;

    /// <summary>
    /// 工位 ID（选项 TaktSopWorkstations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "workstation_id", ColumnDescription = "工位ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long WorkstationId { get; set; }

    /// <summary>
    /// 员工 ID（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工名称（冗余：按 EmployeeId 取 TaktEmployee.EmployeeName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "sop_id", ColumnDescription = "SOP主档ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "revision_id", ColumnDescription = "SOP版本ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 版本号快照
    /// </summary>
    [SugarColumn(ColumnName = "revision", ColumnDescription = "版本号快照", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string Revision { get; set; } = string.Empty;

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
    /// 自检结果（字典 logistics_manufacturing_sop_check_result；1=合格，2=不合格，3=不适用/跳过）
    /// </summary>
    [SugarColumn(ColumnName = "self_check_result", ColumnDescription = "自检结果", ColumnDataType = "int", IsNullable = true)]
    public int? SelfCheckResult { get; set; }

    /// <summary>
    /// 执行状态（字典 logistics_manufacturing_sop_exec_status；1=进行中，2=完成，3=中断）
    /// </summary>
    [SugarColumn(ColumnName = "exec_status", ColumnDescription = "执行状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ExecStatus { get; set; } = 1;

    /// <summary>
    /// 当前工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "current_step_id", ColumnDescription = "当前工步ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? CurrentStepId { get; set; }

    /// <summary>
    /// 工位
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(WorkstationId))]
    public TaktSopWorkstation? Workstation { get; set; }

    /// <summary>
    /// 工步执行明细
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopExecStep.ExecId))]
    public List<TaktSopExecStep>? Steps { get; set; }

    /// <summary>
    /// 扫码记录
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopExecScan.ExecId))]
    public List<TaktSopExecScan>? Scans { get; set; }

    /// <summary>
    /// 作业参数
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktSopArgument.ExecId))]
    public List<TaktSopArgument>? Arguments { get; set; }
}

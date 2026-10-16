// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetail.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA明细实体，按生产时段、班组、设备、板别等记录完成数、标准产能、工数、未达成等
// 累计完成数：按工单号+班次+PCB板别+面板别汇总全部明细当日完成数（见 TaktPcbaOutputDetailDerivedFieldsHelper）
// 标准产能：人员小时产能 = DirectLabor×60÷StdMinutes×标准生产稼动率(%)；设备小时产能 = 60÷StdMinutes×设备时间稼动率(%)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA明细实体（按 PCBA日报ID + 生产班组 + 班次 + PCB板别 + 面板别 + 行号唯一）
/// </summary>
[SugarTable("takt_logistics_manufacturing_output_pcba_detail", "PCBA日报明细表")]
[SugarIndex("ix_pcba_output_detail_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pcba_output_detail_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_detail_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PcbaOutputId), OrderByType.Asc, nameof(TeamCode), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, nameof(PcbBoardType), OrderByType.Asc, nameof(PanelSide), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_detail_pcba_output_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PcbaOutputId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_output_pcba_detail_completion_bucket", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProdOrderCode), OrderByType.Asc, nameof(ShiftNo), OrderByType.Asc, nameof(PcbBoardType), OrderByType.Asc, nameof(PanelSide), OrderByType.Asc, false)]
public class TaktPcbaOutputDetail : TaktCompanyEntityBase
{
    /// <summary>
    /// PCBA日报ID（主表主键；与租户、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键；序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_output_id", ColumnDescription = "PCBA日报ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long PcbaOutputId { get; set; }

    /// <summary>
    /// 工单号（冗余字段,便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10；与租户、PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产时段（PCBA 存工作中心 WorkCenter，新增时按物料查 TaktStandardOperationTime 自动生成）
    /// </summary>
    [SugarColumn(ColumnName = "time_period", ColumnDescription = "生产时段", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TimePeriod { get; set; } = string.Empty;

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options?teamCategory=P；DictValue=TeamCode，ExtValue=PlantCode；与租户、PcbaOutputId、ShiftNo、PcbBoardType、PanelSide、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "team_code", ColumnDescription = "生产班组", Length = 8, ColumnDataType = "nvarchar", IsNullable = false)]
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产设备编码（选项 TaktProductionEquipments/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "prod_equip_code", ColumnDescription = "生产设备", Length = 18, ColumnDataType = "nvarchar", IsNullable = false)]
    public string ProdEquipCode { get; set; } = string.Empty;

    /// <summary>
    /// 直接人员
    /// </summary>
    [SugarColumn(ColumnName = "direct_labor", ColumnDescription = "直接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DirectLabor { get; set; } = 0;

    /// <summary>
    /// 间接人员
    /// </summary>
    [SugarColumn(ColumnName = "indirect_labor", ColumnDescription = "间接人员", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IndirectLabor { get; set; } = 0;

    /// <summary>
    /// 班次（字典 logistics_manufacturing_shift_category；1=早 2=中 3=晚 4=白班 5=夜班；与租户、PcbaOutputId、TeamCode、PcbBoardType、PanelSide、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "shift_no", ColumnDescription = "班次", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ShiftNo { get; set; } = 1;

    /// <summary>
    /// 标准工时(分钟)（回填：按工作中心查询 TaktStandardOperationTime 转换工时）
    /// </summary>
    [SugarColumn(ColumnName = "std_minutes", ColumnDescription = "标准工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StdMinutes { get; set; } = 0;

    /// <summary>
    /// 人员标准产能（计算结果：DirectLabor×60÷StdMinutes×标准生产稼动率）
    /// </summary>
    [SugarColumn(ColumnName = "std_labor_capacity", ColumnDescription = "人员标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal StdLaborCapacity { get; set; } = 0;

    /// <summary>
    /// 标准点数（PCBA 专用，按工作中心回填）
    /// </summary>
    [SugarColumn(ColumnName = "std_shorts", ColumnDescription = "标准点数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int StdShorts { get; set; } = 0;

    /// <summary>
    /// 设备标准产能（计算结果：60÷StdMinutes×设备时间稼动率）
    /// </summary>
    [SugarColumn(ColumnName = "std_equipment_capacity", ColumnDescription = "设备标准产能", ColumnDataType = "decimal", Length = 18, DecimalDigits = 3, IsNullable = false, DefaultValue = "0")]
    public decimal StdEquipmentCapacity { get; set; } = 0;

    /// <summary>
    /// PCB板别（字典 logistics_manufacturing_pcba_function；存 DictLabel；UI 提交由前端 dict-type 转换；与租户、PcbaOutputId、TeamCode、ShiftNo、PanelSide、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "pcb_board_type", ColumnDescription = "PCB板别", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PcbBoardType { get; set; } = string.Empty;

    /// <summary>
    /// 面板别（字典 logistics_manufacturing_pcba_side_category；存 DictValue：b= B面 t= T面；与租户、PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、LineNumber 组成唯一键）
    /// </summary>
    [SugarColumn(ColumnName = "panel_side", ColumnDescription = "面板别", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string PanelSide { get; set; } = string.Empty;

    /// <summary>
    /// 批次数量
    /// </summary>
    [SugarColumn(ColumnName = "batch_qty", ColumnDescription = "批次数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal BatchQty { get; set; } = 0;

    /// <summary>
    /// 当日完成数
    /// </summary>
    [SugarColumn(ColumnName = "daily_completed_qty", ColumnDescription = "当日完成数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal DailyCompletedQty { get; set; } = 0;

    /// <summary>
    /// 累计完成数（计算结果：同工单号+班次+PCB板别+面板别桶内全部明细当日完成数合计）
    /// </summary>
    [SugarColumn(ColumnName = "total_completed_qty", ColumnDescription = "累计完成数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 1, IsNullable = false, DefaultValue = "0")]
    public decimal TotalCompletedQty { get; set; } = 0;

    /// <summary>
    /// 完成状态（计算结果：字典 logistics_manufacturing_pcba_completed_status；0=未完成 1=部分完成 2=已完成；按累计完成数与批次数量比较）
    /// </summary>
    [SugarColumn(ColumnName = "completed_status", ColumnDescription = "完成状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CompletedStatus { get; set; } = 0;

    /// <summary>
    /// 序列号（明细级）
    /// </summary>
    [SugarColumn(ColumnName = "serial_code", ColumnDescription = "序列号", Length = 80, ColumnDataType = "nvarchar", IsNullable = false)]
    public string SerialCode { get; set; } = string.Empty;

    /// <summary>
    /// 不良台数
    /// </summary>
    [SugarColumn(ColumnName = "defect_count", ColumnDescription = "不良台数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DefectCount { get; set; } = 0;

    /// <summary>
    /// 停线时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "downtime_minutes", ColumnDescription = "停线时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DowntimeMinutes { get; set; } = 0;

    /// <summary>
    /// 停线原因（字典 logistics_manufacturing_stop_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填停线说明）
    /// </summary>
    [SugarColumn(ColumnName = "downtime_reason", ColumnDescription = "停线原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DowntimeReason { get; set; }

    /// <summary>
    /// 停线说明（停线原因选定后的补充详细说明）
    /// </summary>
    [SugarColumn(ColumnName = "downtime_description", ColumnDescription = "停线说明", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DowntimeDescription { get; set; }

    /// <summary>
    /// 投入工数(分钟)（计算结果：明细 DirectLabor×60）
    /// </summary>
    [SugarColumn(ColumnName = "input_minutes", ColumnDescription = "投入工数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InputMinutes { get; set; } = 0;

    /// <summary>
    /// 实际工时(分钟)（计算结果：MixedProd=0 时投入工时-停线时间；MixedProd≠0 时报工工时-停线时间）
    /// </summary>
    [SugarColumn(ColumnName = "actual_minutes", ColumnDescription = "实际工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ActualMinutes { get; set; } = 0;

    /// <summary>
    /// 修工数(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "repair_minutes", ColumnDescription = "修工数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal RepairMinutes { get; set; } = 0;

    /// <summary>
    /// 切换次数
    /// </summary>
    [SugarColumn(ColumnName = "switch_count", ColumnDescription = "切换次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SwitchCount { get; set; } = 0;

    /// <summary>
    /// 切换时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "switch_time", ColumnDescription = "切换时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SwitchTime { get; set; } = 0;

    /// <summary>
    /// 切停机时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "stop_time", ColumnDescription = "切停机时间", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal StopTime { get; set; } = 0;

    /// <summary>
    /// 总工数(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "total_minutes", ColumnDescription = "总工数", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalMinutes { get; set; } = 0;

    /// <summary>
    /// 未达成原因（字典 logistics_manufacturing_nonachievement_reason；多选存 DictLabel 逗号分隔；先选字典原因，再填未达成说明）
    /// </summary>
    [SugarColumn(ColumnName = "unachieved_reason", ColumnDescription = "未达成原因", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? UnachievedReason { get; set; }

    /// <summary>
    /// 未达成说明（未达成原因选定后的补充详细说明）
    /// </summary>
    [SugarColumn(ColumnName = "unachieved_description", ColumnDescription = "未达成说明", Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? UnachievedDescription { get; set; }

    /// <summary>
    /// 报工工时(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "confirm_minutes", ColumnDescription = "报工工时", ColumnDataType = "decimal", Length = 10, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ConfirmMinutes { get; set; } = 0;

    /// <summary>
    /// 混合生产（0=非混合；N=此生产时段内另有N笔报工）
    /// </summary>
    [SugarColumn(ColumnName = "mixed_prod", ColumnDescription = "混合生产", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int MixedProd { get; set; } = 0;

    /// <summary>
    /// 达成率(%)（计算结果：当日完成数÷明细人员标准产能×100%；标准产能为0时取0）
    /// </summary>
    [SugarColumn(ColumnName = "achievement_rate", ColumnDescription = "达成率", ColumnDataType = "decimal", Length = 7, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AchievementRate { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// PCBA日报（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PcbaOutputId))]
    public TaktPcbaOutput? PcbaOutput { get; set; }
}

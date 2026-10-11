// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleItem.cs
// 功能描述：APS排程明细，排程的具体工序任务
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程明细（排程的具体工序任务）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_schedule_item", "APS排程明细表")]
[SugarIndex("ix_aps_schedule_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_aps_schedule_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_item_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApsScheduleId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_item_aps_schedule_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApsScheduleId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_item_plant_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_schedule_item_process_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProcessCode), OrderByType.Asc, false)]
public class TaktApsScheduleItem : TaktCompanyEntityBase
{
    /// <summary>
    /// APS排程ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "aps_schedule_id", ColumnDescription = "APS排程ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ApsScheduleId { get; set; }


    /// <summary>
    /// APS排程编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "aps_schedule_code", ColumnDescription = "APS排程编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ApsScheduleCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 订单 ID（选项 TaktApsOrders/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "aps_order_id", ColumnDescription = "APS订单ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOrderId { get; set; }

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "aps_operation_id", ColumnDescription = "APS工序排程ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工艺路线工序 ID（选项 TaktRoutingItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "routing_item_id", ColumnDescription = "工艺路线工序ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RoutingItemId { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 生产工单编码（选项 TaktProductionOrders/options；DictValue=ProdOrderCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "生产工单编码", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "product_code", ColumnDescription = "产品编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 产品名称
    /// </summary>
    [SugarColumn(ColumnName = "product_name", ColumnDescription = "产品名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenterCode { get; set; }

    /// <summary>
    /// 工作中心描述
    /// </summary>
    [SugarColumn(ColumnName = "work_center_description", ColumnDescription = "工作中心描述", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? WorkCenterDescription { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    [SugarColumn(ColumnName = "process_code", ColumnDescription = "工序编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = false)]
    public string ProcessCode { get; set; } = string.Empty;

    /// <summary>
    /// 工序名称
    /// </summary>
    [SugarColumn(ColumnName = "process_name", ColumnDescription = "工序名称", ColumnDataType = "nvarchar", Length = 70, IsNullable = false)]
    public string ProcessName { get; set; } = string.Empty;

    /// <summary>
    /// 工序序号
    /// </summary>
    [SugarColumn(ColumnName = "process_sequence", ColumnDescription = "工序序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcessSequence { get; set; } = 0;

    /// <summary>
    /// 工序标准ST值
    /// </summary>
    [SugarColumn(ColumnName = "process_standard_st", ColumnDescription = "工序标准ST值", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ProcessStandardST { get; set; } = 0;

    /// <summary>
    /// 工序标准ST单位（0=秒/件，1=Shot/件，2=Point/件，3=分钟/件，4=小时/件）
    /// </summary>
    [SugarColumn(ColumnName = "process_standard_st_unit", ColumnDescription = "工序标准ST单位", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcessStandardSTUnit { get; set; } = 0;

    /// <summary>
    /// 额外时间（分钟），如换模、调试、清洁等准备时间
    /// </summary>
    [SugarColumn(ColumnName = "extra_minutes", ColumnDescription = "额外时间", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ExtraMinutes { get; set; } = 0;

    /// <summary>
    /// 计划数量
    /// </summary>
    [SugarColumn(ColumnName = "plan_quantity", ColumnDescription = "计划数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal PlanQuantity { get; set; } = 0;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "plan_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "plan_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime PlanEndTime { get; set; }

    /// <summary>
    /// 实际开始时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_start_time", ColumnDescription = "实际开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    [SugarColumn(ColumnName = "actual_end_time", ColumnDescription = "实际结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// 工序状态（0=未开始，1=准备中，2=加工中，3=已完工，4=已暂停，5=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "process_status", ColumnDescription = "工序状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ProcessStatus { get; set; } = 0;

    /// <summary>
    /// 优先级（0=普通，1=紧急，2=特急）
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// APS排程主表（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(ApsScheduleId))]
    public TaktApsSchedule? Schedule { get; set; }
}

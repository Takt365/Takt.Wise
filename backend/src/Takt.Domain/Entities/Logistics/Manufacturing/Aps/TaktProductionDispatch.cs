// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Aps
// 文件名称：TaktProductionDispatch.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：生产派工单，APS 排程释放至 MES 的执行指令
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产派工单（Prod_Order → Dispatch → MES 报工）
/// </summary>
[SugarTable("takt_logistics_manufacturing_aps_production_dispatch", "APS生产派工单表")]
[SugarIndex("ix_production_dispatch_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_production_dispatch_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_production_dispatch_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(DispatchCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_aps_production_dispatch_prod_order", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ProductionOrderId), OrderByType.Asc, false)]
public class TaktProductionDispatch : TaktCompanyEntityBase
{

    /// <summary>
    /// 派工单编码（前端表单选择编码规则后自动通过 TaktNumbering 派工单编码规则生成并展示，非手输；单据类型菜单：生产派工）
    /// </summary>
    [SugarColumn(ColumnName = "dispatch_code", ColumnDescription = "派工单编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DispatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工单 ID（选项 TaktProductionOrders/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "production_order_id", ColumnDescription = "生产工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionOrderId { get; set; }

    /// <summary>
    /// 工单号（关联 TaktProductionOrder.ProdOrderCode，冗余）
    /// </summary>
    [SugarColumn(ColumnName = "prod_order_code", ColumnDescription = "工单号", ColumnDataType = "nvarchar", Length = 12, IsNullable = false)]
    public string ProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// APS 工序排程 ID（选项 TaktApsOperations/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "aps_operation_id", ColumnDescription = "APS工序排程ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ApsOperationId { get; set; }

    /// <summary>
    /// 工作中心编码（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center_code", ColumnDescription = "工作中心编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenterCode { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    [SugarColumn(ColumnName = "process_code", ColumnDescription = "工序编码", ColumnDataType = "nvarchar", Length = 4, IsNullable = true)]
    public string? ProcessCode { get; set; }

    /// <summary>
    /// 派工数量
    /// </summary>
    [SugarColumn(ColumnName = "dispatch_quantity", ColumnDescription = "派工数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DispatchQuantity { get; set; } = 0;

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_start_time", ColumnDescription = "计划开始时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedStartTime { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [SugarColumn(ColumnName = "planned_end_time", ColumnDescription = "计划结束时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? PlannedEndTime { get; set; }

    /// <summary>
    /// 派工状态（字典 production_dispatch_status；0=待执行，1=执行中，2=已完成，3=已取消）
    /// </summary>
    [SugarColumn(ColumnName = "dispatch_status", ColumnDescription = "派工状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DispatchStatus { get; set; } = 0;
}

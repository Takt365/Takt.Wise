// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderMaterial.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单领料明细实体，记录维护工单物料领用
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Domain.Entities.Logistics.Maintenance;

/// <summary>
/// 维护工单领料明细实体（主子表：挂载于维护工单）
/// </summary>
[SugarTable("takt_logistics_maintenance_work_order_material", "维护工单领料表")]
[SugarIndex("ix_maintenance_work_order_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_maintenance_work_order_material_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_material_order_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceWorkOrderId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_maintenance_work_order_material_work_order_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MaintenanceWorkOrderId), OrderByType.Asc, false)]
public class TaktMaintenanceWorkOrderMaterial : TaktCompanyEntityBase
{
    /// <summary>
    /// 维护工单ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "maintenance_work_order_id", ColumnDescription = "维护工单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaintenanceWorkOrderId { get; set; }

    /// <summary>
    /// 维护工单号（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "work_order_code", ColumnDescription = "维护工单号", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string WorkOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（步长10：10/20/30…）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 物料ID（关联工厂物料主数据，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "material_id", ColumnDescription = "物料ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 需求数量
    /// </summary>
    [SugarColumn(ColumnName = "required_quantity", ColumnDescription = "需求数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal RequiredQuantity { get; set; } = 0;

    /// <summary>
    /// 已领数量
    /// </summary>
    [SugarColumn(ColumnName = "issued_quantity", ColumnDescription = "已领数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IssuedQuantity { get; set; } = 0;

    /// <summary>
    /// 单位
    /// </summary>
    [SugarColumn(ColumnName = "material_unit", ColumnDescription = "单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "个")]
    public string MaterialUnit { get; set; } = "个";

    /// <summary>
    /// 单价
    /// </summary>
    [SugarColumn(ColumnName = "unit_price", ColumnDescription = "单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal UnitPrice { get; set; } = 0;

    /// <summary>
    /// 金额
    /// </summary>
    [SugarColumn(ColumnName = "amount", ColumnDescription = "金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal Amount { get; set; } = 0;

    /// <summary>
    /// 仓库编码
    /// </summary>
    [SugarColumn(ColumnName = "warehouse_code", ColumnDescription = "仓库编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? WarehouseCode { get; set; }

    /// <summary>
    /// 库位
    /// </summary>
    [SugarColumn(ColumnName = "storage_location", ColumnDescription = "库位", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? StorageLocation { get; set; }

    /// <summary>
    /// 领料状态（0=待领料，1=部分领料，2=已领料）
    /// </summary>
    [SugarColumn(ColumnName = "issue_status", ColumnDescription = "领料状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IssueStatus { get; set; } = 0;

    /// <summary>
    /// 领料时间
    /// </summary>
    [SugarColumn(ColumnName = "issue_time", ColumnDescription = "领料时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? IssueTime { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 维护工单（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaintenanceWorkOrderId))]
    public TaktMaintenanceWorkOrder? MaintenanceWorkOrder { get; set; }

    /// <summary>
    /// 物料（工厂物料主数据）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MaterialId))]
    public TaktMaterialPlant? MaterialPlant { get; set; }
}

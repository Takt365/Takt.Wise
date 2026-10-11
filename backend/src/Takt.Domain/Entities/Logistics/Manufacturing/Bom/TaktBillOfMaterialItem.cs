// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialItem.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt物料清单明细实体（BOM_Item：扁平单层组成件，对齐 BOM_Header + BOM_Component 规范）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// Takt物料清单明细实体（扁平BOM行：一头多行，每行一个直接子件；多层BOM通过子件物料关联其BOM头递归展开）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_item", "物料清单明细表")]
[SugarIndex("ix_bill_of_material_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bill_of_material_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_item_bom_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillOfMaterialId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_item_bill_of_material_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillOfMaterialId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_item_bom_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BomCode), OrderByType.Asc, false)]
public class TaktBillOfMaterialItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料清单ID（关联BOM头，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "bill_of_material_id", ColumnDescription = "物料清单ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialId { get; set; }

    /// <summary>
    /// BOM编码（冗余，便于查询）
    /// </summary>
    [SugarColumn(ColumnName = "bom_code", ColumnDescription = "BOM编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = false)]
    public string BomCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号，步长10：10/20/30…）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 子项物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "子项物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 子项物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "子项物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? MaterialDescription { get; set; }

    /// <summary>
    /// 用量（quantity）
    /// </summary>
    [SugarColumn(ColumnName = "usage_quantity", ColumnDescription = "用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal UsageQuantity { get; set; } = 0;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [SugarColumn(ColumnName = "material_unit", ColumnDescription = "单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string MaterialUnit { get; set; } = "PC";

    /// <summary>
    /// 损耗率（0-100，scrap_rate）
    /// </summary>
    [SugarColumn(ColumnName = "scrap_rate", ColumnDescription = "损耗率", ColumnDataType = "decimal", Length = 5, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ScrapRate { get; set; } = 0;

    /// <summary>
    /// 实际用量（用量 × (1 + 损耗率/100)）
    /// </summary>
    [SugarColumn(ColumnName = "actual_usage_quantity", ColumnDescription = "实际用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal ActualUsageQuantity { get; set; } = 0;

    /// <summary>
    /// 工序号（operation_seq）
    /// </summary>
    [SugarColumn(ColumnName = "operation_seq", ColumnDescription = "工序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int OperationSeq { get; set; } = 0;

    /// <summary>
    /// 工作中心（选项 TaktWorkCenters/options；DictValue=WorkCenterCode）
    /// </summary>
    [SugarColumn(ColumnName = "work_center", ColumnDescription = "工作中心", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? WorkCenter { get; set; }

    /// <summary>
    /// 位号（position，PCB位号等）
    /// </summary>
    [SugarColumn(ColumnName = "position", ColumnDescription = "位号", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? Position { get; set; }

    /// <summary>
    /// 替代组号（substitute_group）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_group", ColumnDescription = "替代组号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SubstituteGroup { get; set; }

    /// <summary>
    /// 替代优先级（组内越小越优先）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_priority", ColumnDescription = "替代优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 是否可选件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_optional", ColumnDescription = "是否可选件", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsOptional { get; set; } = 0;

    /// <summary>
    /// 是否虚拟件（字典 sys_yes_no；0=否，1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_phantom", ColumnDescription = "是否虚拟件", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsPhantom { get; set; } = 0;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料清单（BOM头）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(BillOfMaterialId))]
    public TaktBillOfMaterial? Bom { get; set; }

    /// <summary>
    /// 替代料明细（一行主件可维护多条替代物料）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktBillOfMaterialSubstitute.BillOfMaterialItemId))]
    public List<TaktBillOfMaterialSubstitute>? Substitutes { get; set; }
}

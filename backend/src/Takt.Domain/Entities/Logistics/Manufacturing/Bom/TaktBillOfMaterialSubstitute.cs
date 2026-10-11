// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Manufacturing.Bom
// 文件名称：TaktBillOfMaterialSubstitute.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM替代料实体，记录物料清单明细行可替换的替代物料、优先级与用量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;
using Takt.Domain.Entities.Logistics.Materials;

namespace Takt.Domain.Entities.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM替代料实体（挂载于物料清单明细行，一行主件可维护多条替代物料）
/// </summary>
[SugarTable("takt_logistics_manufacturing_bom_substitute", "BOM替代料表")]
[SugarIndex("ix_bill_of_material_substitute_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bill_of_material_substitute_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_substitute_item_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillOfMaterialItemId), OrderByType.Asc, nameof(SubstituteMaterialId), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_substitute_bill_of_material_item_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillOfMaterialItemId), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_manufacturing_bom_substitute_bill_of_material_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(BillOfMaterialId), OrderByType.Asc, false)]
public class TaktBillOfMaterialSubstitute : TaktCompanyEntityBase
{
    /// <summary>
    /// 物料清单明细ID（主子表关系，序列化为string以避免Javascript精度问题）
    /// </summary>
    [SugarColumn(ColumnName = "bill_of_material_item_id", ColumnDescription = "物料清单明细ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BillOfMaterialItemId { get; set; }

    /// <summary>
    /// 物料清单ID（冗余，便于按BOM头查询，序列化为string以避免Javascript精度问题）
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
    /// 主件物料编码（冗余，对应明细行子项物料编码）
    /// </summary>
    [SugarColumn(ColumnName = "primary_material_code", ColumnDescription = "主件物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string PrimaryMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代行号（步长10：10/20/30…）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "替代行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;

    /// <summary>
    /// 替代物料ID（选项 TaktMaterialPlants/options；DictValue=Id，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_material_id", ColumnDescription = "替代物料ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SubstituteMaterialId { get; set; }

    /// <summary>
    /// 替代物料编码（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_material_code", ColumnDescription = "替代物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string SubstituteMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 替代组号（与明细行 substitute_group 对齐，便于组内检索）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_group", ColumnDescription = "替代组号", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SubstituteGroup { get; set; }

    /// <summary>
    /// 替代优先级（越小越优先）
    /// </summary>
    [SugarColumn(ColumnName = "substitute_priority", ColumnDescription = "替代优先级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SubstitutePriority { get; set; } = 0;

    /// <summary>
    /// 替代用量
    /// </summary>
    [SugarColumn(ColumnName = "usage_quantity", ColumnDescription = "替代用量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal UsageQuantity { get; set; } = 0;

    /// <summary>
    /// 单位（字典 logistics_materials_unit_of_measure_code）
    /// </summary>
    [SugarColumn(ColumnName = "material_unit", ColumnDescription = "单位", ColumnDataType = "nvarchar", Length = 20, IsNullable = false, DefaultValue = "PC")]
    public string MaterialUnit { get; set; } = "PC";

    /// <summary>
    /// 替代比例（相对主件用量，默认1表示等量替代）
    /// </summary>
    [SugarColumn(ColumnName = "usage_ratio", ColumnDescription = "替代比例", ColumnDataType = "decimal", Length = 10, DecimalDigits = 4, IsNullable = false, DefaultValue = "1")]
    public decimal UsageRatio { get; set; } = 1;

    /// <summary>
    /// 是否启用（0=否，1=是，字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_enabled", ColumnDescription = "是否启用", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsEnabled { get; set; } = 1;

    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// 失效日期（为空表示永久有效）
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 物料清单明细（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(BillOfMaterialItemId))]
    public TaktBillOfMaterialItem? BillOfMaterialItem { get; set; }

    /// <summary>
    /// 替代物料（工厂物料主数据）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(SubstituteMaterialId))]
    public TaktMaterialPlant? SubstituteMaterialPlant { get; set; }
}

// ========================================
// 项目名称:节拍智控 ·TaktWise (TDF)
// 命名空间:Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称:TaktQualityIncidentItem.cs
// 创建时间:2026-05-08
// 创建人:Takt365(Qoder AI)
// 功能描述:品质事故明细 - 废弃零件明细行
//
// 版权信息:Copyright (c) 2025 Takt  All rights reserved.
// 免责声明:此软件使用 MIT License,作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质事故明细 - 废弃零件明细行
/// </summary>
[SugarTable("takt_logistics_quality_incident_item", "品质事故明细表")]
[SugarIndex("ix_quality_incident_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_incident_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_incident_item_line_number_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QualityIncidentId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
public class TaktQualityIncidentItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 品质事故主表 ID（选项 TaktQualityIncidents/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "quality_incident_id", ColumnDescription = "品质事故主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIncidentId { get; set; }

    /// <summary>
    /// 品质事故编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "quality_incident_code", ColumnDescription = "品质事故编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityIncidentCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", Length = 70, ColumnDataType = "nvarchar", IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 废弃费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "scrap_cost", ColumnDescription = "废弃费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ScrapCost { get; set; } = 0;

    /// <summary>
    /// 废弃数量
    /// </summary>
    [SugarColumn(ColumnName = "scrap_size", ColumnDescription = "废弃数量", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ScrapSize { get; set; } = 0;

    /// <summary>
    /// 零件单价(元)
    /// </summary>
    [SugarColumn(ColumnName = "part_price", ColumnDescription = "零件单价", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PartPrice { get; set; } = 0;

    /// <summary>
    /// 废弃处理费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "scrap_reason_cost", ColumnDescription = "废弃处理费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ScrapReasonCost { get; set; } = 0;

    /// <summary>
    /// 运费(元)
    /// </summary>
    [SugarColumn(ColumnName = "freight_charges", ColumnDescription = "运费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal FreightCharges { get; set; } = 0;

    /// <summary>
    /// 其他费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "other_expenses", ColumnDescription = "其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherExpenses { get; set; } = 0;

    /// <summary>
    /// 处理作业时间(分钟)
    /// </summary>
    [SugarColumn(ColumnName = "reason_work_time_minutes", ColumnDescription = "处理作业时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ReasonWorkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// 关税(元)
    /// </summary>
    [SugarColumn(ColumnName = "tax", ColumnDescription = "关税", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal Tax { get; set; } = 0;

    /// <summary>
    /// 处理发生其他费用(元)
    /// </summary>
    [SugarColumn(ColumnName = "reason_other_expenses", ColumnDescription = "处理发生其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal ReasonOtherExpenses { get; set; } = 0;

    /// <summary>
    /// 废弃备注
    /// </summary>
    [SugarColumn(ColumnName = "scrap_note", ColumnDescription = "废弃备注", ColumnDataType = "ntext", IsNullable = true)]
    public string? ScrapNote { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 品质事故主表(导航属性)
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(QualityIncidentId))]
    public TaktQualityIncident? Incident { get; set; }
}

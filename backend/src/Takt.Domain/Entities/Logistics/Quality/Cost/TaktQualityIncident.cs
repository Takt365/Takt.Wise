// ========================================
// 项目名称:节拍智控 ·TaktWise (TDF)
// 命名空间:Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称:TaktQualityIncident.cs
// 创建时间:2026-05-07
// 创建人:Takt365(Qoder AI)
// 功能描述:品质事故主表,用于记录因品质问题导致的物料/产品报废基础信息及汇总数据
//
// 版权信息:Copyright (c) 2025 Takt  All rights reserved.
// 免责声明:此软件使用 MIT License,作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质事故主表,用于记录废弃单的基础信息(年月日、机种)及汇总数据
/// </summary>
[SugarTable("takt_logistics_quality_incident", "品质事故主表")]
[SugarIndex("ix_quality_incident_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_incident_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_incident_qi_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(QualityIncidentCode), OrderByType.Asc, nameof(IncidentDate), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_incident_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktQualityIncident : TaktCompanyEntityBase
{

    /// <summary>
    /// 品质事故编码(唯一,如:QI-2026-0001)
    /// </summary>
    [SugarColumn(ColumnName = "quality_incident_code", ColumnDescription = "品质事故编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityIncidentCode { get; set; } = string.Empty;

    // ==================== 基础日期与产品信息 ====================

    /// <summary>
    /// 事故日期
    /// </summary>
    [SugarColumn(ColumnName = "incident_date", ColumnDescription = "事故日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// 间接人员费率(元/分钟)
    /// </summary>
    [SugarColumn(ColumnName = "indirect_manpower_cost_per_minute", ColumnDescription = "间接人员费率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectManpowerCostPerMinute { get; set; } = 0;

    /// <summary>
    /// 机种/产品型号
    /// </summary>
    [SugarColumn(ColumnName = "model", ColumnDescription = "机种", Length = 40, ColumnDataType = "nvarchar", IsNullable = false)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// 事故内容(废弃原因)
    /// </summary>
    [SugarColumn(ColumnName = "incident_reason", ColumnDescription = "事故内容", ColumnDataType = "ntext", IsNullable = true)]
    public string? IncidentReason { get; set; }

    /// <summary>
    /// 废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "scrap_group_code", ColumnDescription = "废弃组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ScrapGroupCode { get; set; }

    /// <summary>
    /// 废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "scrap_group_name", ColumnDescription = "废弃组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ScrapGroupName { get; set; }

    // ==================== 汇总信息 ====================

    /// <summary>
    /// 废弃总数(自动计算 = 各子表废弃数量合计)
    /// </summary>
    [SugarColumn(ColumnName = "total_scrap_quantity", ColumnDescription = "废弃总数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalScrapQuantity { get; set; } = 0;

    /// <summary>
    /// 总废弃费用(元,自动计算 = 各子表费用合计)
    /// </summary>
    [SugarColumn(ColumnName = "total_scrap_cost", ColumnDescription = "总废弃费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalScrapCost { get; set; } = 0;

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "成本币种", Length = 3, ColumnDataType = "nvarchar", IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    // ==================== 导航关系 ====================

    /// <summary>
    /// 事故明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityIncidentItem.QualityIncidentId))]
    public List<TaktQualityIncidentItem>? IncidentItems { get; set; }
}

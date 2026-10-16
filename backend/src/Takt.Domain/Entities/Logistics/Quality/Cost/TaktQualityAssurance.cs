// ========================================
// 项目名称:节拍智控 ·TaktWise (TDF)
// 命名空间:Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称:TaktQualityAssurance.cs
// 创建时间:2026-05-07
// 创建人:Takt365(Qoder AI)
// 功能描述:品质业务主表,用于记录品质业务的基础信息(年月)及汇总数据
//
// 版权信息:Copyright (c) 2025 Takt  All rights reserved.
// 免责声明:此软件使用 MIT License,作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质业务主表,用于记录品质业务的基础信息(年月、顾客)及汇总数据
/// </summary>
[SugarTable("takt_logistics_quality_assurance", "品质业务主表")]
[SugarIndex("ix_quality_assurance_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_assurance_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_assurance_qo_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(QualityAssuranceCode), OrderByType.Asc, nameof(AssuranceMonth), OrderByType.Asc, nameof(DebitNoteCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_assurance_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktQualityAssurance : TaktCompanyEntityBase
{

    /// <summary>
    /// 品质业务编码(租户+公司+工厂内唯一，如:QO-2026-0001)（前端表单选择编码规则后自动通过 TaktNumbering 品质业务编码规则生成并展示，非手输；单据类型菜单：品质保证）
    /// </summary>
    [SugarColumn(ColumnName = "quality_assurance_code", ColumnDescription = "品质业务编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityAssuranceCode { get; set; } = string.Empty;

    // ==================== 基础日期与信息 ====================

    /// <summary>
    /// 业务年月(格式:2026-05)
    /// </summary>
    [SugarColumn(ColumnName = "assurance_month", ColumnDescription = "业务年月", Length = 7, ColumnDataType = "nvarchar", IsNullable = false)]
    public string AssuranceMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
    /// </summary>
    [SugarColumn(ColumnName = "customer_name1", ColumnDescription = "客户名称1", Length = 140, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? CustomerName1 { get; set; }

    /// <summary>
    /// Debit Note No
    /// </summary>
    [SugarColumn(ColumnName = "debit_note_code", ColumnDescription = "Debit Note No", Length = 30, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? DebitNoteCode { get; set; }

    /// <summary>
    /// 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "record_group_code", ColumnDescription = "记录组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? RecordGroupCode { get; set; }

    /// <summary>
    /// 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "record_group_name", ColumnDescription = "记录组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? RecordGroupName { get; set; }

    // ==================== 汇总信息 ====================

    /// <summary>
    /// 质量总成本(元,自动计算 = 各子表费用合计)
    /// </summary>
    [SugarColumn(ColumnName = "total_quality_cost", ColumnDescription = "质量总成本", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalQualityCost { get; set; } = 0;

    /// <summary>
    /// 成本币种(CNY/USD/JPY等)
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "成本币种", Length = 3, ColumnDataType = "nvarchar", IsNullable = false, DefaultValue = "CNY")]
    public string CurrencyCode { get; set; } = "CNY";

    // ==================== 导航关系 ====================

    /// <summary>
    /// 来料检验费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceIncoming.QualityAssuranceId))]
    public List<TaktQualityAssuranceIncoming>? IncomingItems { get; set; }

    /// <summary>
    /// 初期/定期检定费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceFirstArticle.QualityAssuranceId))]
    public List<TaktQualityAssuranceFirstArticle>? FirstArticleItems { get; set; }

    /// <summary>
    /// 设备校正费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceCalibration.QualityAssuranceId))]
    public List<TaktQualityAssuranceCalibration>? CalibrationItems { get; set; }

    /// <summary>
    /// 其他通常业务费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceOther.QualityAssuranceId))]
    public List<TaktQualityAssuranceOther>? OtherItems { get; set; }

    /// <summary>
    /// 出货检验费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceOutgoing.QualityAssuranceId))]
    public List<TaktQualityAssuranceOutgoing>? OutgoingItems { get; set; }

    /// <summary>
    /// 信赖性评价/ORT费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceReliability.QualityAssuranceId))]
    public List<TaktQualityAssuranceReliability>? ReliabilityItems { get; set; }

    /// <summary>
    /// 顾客品质要求对应费用明细列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktQualityAssuranceCustomerResponse.QualityAssuranceId))]
    public List<TaktQualityAssuranceCustomerResponse>? CustomerResponseItems { get; set; }
}

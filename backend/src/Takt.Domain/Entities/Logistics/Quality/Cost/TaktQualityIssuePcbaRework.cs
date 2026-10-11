// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuePcbaRework.cs
// 创建时间：2026-05-07
// 创建人：Takt365(Qoder AI)
// 功能描述:品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json;
using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Quality.Cost;

/// <summary>
/// 品质问题应对明细 - PCBA不良改修应对(PCBA选别・改修费用)
/// </summary>
[SugarTable("takt_logistics_quality_issue_pcba_rework", "质量问题PCBA不良改修费用明细表")]
[SugarIndex("ix_quality_issue_pcba_rework_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_quality_issue_pcba_rework_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_quality_issue_pcba_rework_line_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(QualityIssueId), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_quality_issue_pcba_rework_line_number", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(LineNumber), OrderByType.Asc, false)]
public class TaktQualityIssuePcbaRework : TaktCompanyEntityBase
{
    /// <summary>
    /// 品质问题主表 ID（选项 TaktQualityIssues/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "quality_issue_id", ColumnDescription = "品质问题主表ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long QualityIssueId { get; set; }

    /// <summary>
    /// 品质问题编码（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "quality_issue_code", ColumnDescription = "品质问题编码", Length = 20, ColumnDataType = "nvarchar", IsNullable = false)]
    public string QualityIssueCode { get; set; } = string.Empty;

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// PCBA不良内容(Parts/Components)
    /// </summary>
    [SugarColumn(ColumnName = "pcba_defect_parts", ColumnDescription = "PCBA不良内容", ColumnDataType = "ntext", IsNullable = true)]
    public string? PcbaDefectParts { get; set; }

    /// <summary>
    /// PCBA选别・改修费用（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_rework_cost", ColumnDescription = "PCBA选别改修费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaReworkCost { get; set; } = 0;

    /// <summary>
    /// PCBA选别・改修时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_rework_time_minutes", ColumnDescription = "PCBA选别改修时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PcbaReworkTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA再检查时间（分钟）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_reinspection_time_minutes", ColumnDescription = "PCBA再检查时间", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PcbaReinspectionTimeMinutes { get; set; } = 0;

    /// <summary>
    /// PCBA交通费、旅费（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_travel_cost", ColumnDescription = "PCBA交通费旅费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaTravelCost { get; set; } = 0;

    /// <summary>
    /// PCBA仓库管理费（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_warehouse_cost", ColumnDescription = "PCBA仓库管理费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaWarehouseCost { get; set; } = 0;

    /// <summary>
    /// PCBA选别・改修其他费用（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_other_expenses", ColumnDescription = "PCBA选别改修其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaOtherExpenses { get; set; } = 0;

    /// <summary>
    /// PCBA选别・改修备注
    /// </summary>
    [SugarColumn(ColumnName = "pcba_rework_note", ColumnDescription = "PCBA选别改修备注", ColumnDataType = "ntext", IsNullable = true)]
    public string? PcbaReworkNote { get; set; }

    /// <summary>
    /// PCBA向顾客的费用请求（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_scrap_cost", ColumnDescription = "PCBA向顾客费用请求", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaScrapCost { get; set; } = 0;

    /// <summary>
    /// PCBA顾客名
    /// </summary>
    [SugarColumn(ColumnName = "pcba_customer_name1", ColumnDescription = "PCBA顾客名1", Length = 140, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? PcbaCustomerName1 { get; set; }

    /// <summary>
    /// PCBA Debit Note No
    /// </summary>
    [SugarColumn(ColumnName = "pcba_debit_note_code", ColumnDescription = "PCBA Debit Note No", Length = 30, ColumnDataType = "nvarchar", IsNullable = true)]
    public string? PcbaDebitNoteCode { get; set; }

    /// <summary>
    /// PCBA其他费用（元）
    /// </summary>
    [SugarColumn(ColumnName = "pcba_other_expenses2", ColumnDescription = "PCBA其他费用", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PcbaOtherExpenses2 { get; set; } = 0;

    /// <summary>
    /// PCBA备注
    /// </summary>
    [SugarColumn(ColumnName = "pcba_note", ColumnDescription = "PCBA备注", ColumnDataType = "ntext", IsNullable = true)]
    public string? PcbaNote { get; set; }

    /// <summary>
    /// 对应组编码（选项 TaktCostGroups/options?costGroupCategory=3；DictValue=CostGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "response_group_code", ColumnDescription = "对应组编码", ColumnDataType = "varchar", Length = 6, IsNullable = true)]
    public string? ResponseGroupCode { get; set; }

    /// <summary>
    /// 对应组名称（冗余：按 ResponseGroupCode 取 TaktCostGroups.CostGroupName 联动）
    /// </summary>
    [SugarColumn(ColumnName = "response_group_name", ColumnDescription = "对应组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = true)]
    public string? ResponseGroupName { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 质量问题主表（导航属性）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(QualityIssueId))]
    public TaktQualityIssue? Issue { get; set; }
}

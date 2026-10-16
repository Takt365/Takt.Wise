// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Financial
// 文件名称：TaktTaxCondition.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税务条件实体（公司级；租户 + 公司 + 应用程序 + 国家 + 税码 + 条件记录号唯一）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Financial;

/// <summary>
/// 税务条件实体（公司级；租户内公司 + 应用程序 + 国家 + 税码 + 条件记录号唯一）。应用程序用字典 accounting_financial_application，条件类型用字典 accounting_financial_condition_type（ExtValue=应用程序）。
/// </summary>
[SugarTable("takt_accounting_financial_tax_condition", "税务条件表")]
[SugarIndex("ix_tax_condition_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_tax_condition_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_accounting_financial_tax_condition_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ApplicationCode), OrderByType.Asc, nameof(CountryCode), OrderByType.Asc, nameof(TaxCode), OrderByType.Asc, nameof(ConditionRecordCode), OrderByType.Asc, true)]
public class TaktTaxCondition : TaktCompanyEntityBase
{
    /// <summary>
    /// 应用程序（字典 accounting_financial_application；DictValue=BA/TX/V 等）
    /// </summary>
    [SugarColumn(ColumnName = "application_code", ColumnDescription = "应用程序", ColumnDataType = "varchar", Length = 2, IsNullable = false)]
    public string ApplicationCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件类型（字典 accounting_financial_condition_type；DictValue=条件类型编码；ExtValue=应用程序）
    /// </summary>
    [SugarColumn(ColumnName = "condition_type_code", ColumnDescription = "条件类型", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string ConditionTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 国家（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_code", ColumnDescription = "国家", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 税码（选项 TaktTaxCodes/options；DictValue=TaxCode）
    /// </summary>
    [SugarColumn(ColumnName = "tax_code", ColumnDescription = "税码", ColumnDataType = "varchar", Length = 2, IsNullable = false)]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// 条件记录号（长度 10；前端表单选择编码规则后自动通过 TaktNumbering 条件记录号规则生成并展示，非手输；单据类型菜单：税务条件）
    /// </summary>
    [SugarColumn(ColumnName = "condition_record_code", ColumnDescription = "条件记录号", ColumnDataType = "nvarchar", Length = 10, IsNullable = false)]
    public string ConditionRecordCode { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }

    /// <summary>
    /// 税务条件状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "tax_condition_status", ColumnDescription = "税务条件状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int TaxConditionStatus { get; set; } = 1;
}

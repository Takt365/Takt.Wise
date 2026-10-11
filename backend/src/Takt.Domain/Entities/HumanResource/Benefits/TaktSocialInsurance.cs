// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Benefits
// 文件名称：TaktSocialInsurance.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：社保公积金月度缴纳流水（非主数据；配置见 TaktBenefitItem，参保见 TaktEmpBenefitPlan）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Benefits;

/// <summary>
/// 社保与公积金月度缴纳流水（分项金额明细；福利类型配置不在此表重复建模）
/// </summary>
[SugarTable("takt_human_resource_benefits_social_insurance", "社保公积金表")]
[SugarIndex("ix_social_insurance_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_social_insurance_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_social_insurance_employee_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(PayPeriod), OrderByType.Asc, false)]
public class TaktSocialInsurance : TaktCompanyEntityBase
{
    /// <summary>
    /// 福利项目（选项 TaktBenefitItems/options；通常 benefit_type 为社保/公积金，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "benefit_item_id", ColumnDescription = "福利项目ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? BenefitItemId { get; set; }
    /// <summary>
    /// 员工（选项 TaktEmployees/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "employee_id", ColumnDescription = "员工ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long EmployeeId { get; set; }
    /// <summary>
    /// 员工姓名
    /// </summary>
    [SugarColumn(ColumnName = "employee_name", ColumnDescription = "员工姓名", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string EmployeeName { get; set; } = string.Empty;
    /// <summary>
    /// 缴纳期间（如 2026-06）
    /// </summary>
    [SugarColumn(ColumnName = "pay_period", ColumnDescription = "缴纳期间", ColumnDataType = "nvarchar", Length = 16, IsNullable = false)]
    public string PayPeriod { get; set; } = string.Empty;
    /// <summary>
    /// 社保缴纳基数（元）
    /// </summary>
    [SugarColumn(ColumnName = "social_security_base", ColumnDescription = "社保缴纳基数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SocialSecurityBase { get; set; }
    /// <summary>
    /// 养老保险（元）
    /// </summary>
    [SugarColumn(ColumnName = "pension_amount", ColumnDescription = "养老保险", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PensionAmount { get; set; }
    /// <summary>
    /// 医疗保险（元）
    /// </summary>
    [SugarColumn(ColumnName = "medical_amount", ColumnDescription = "医疗保险", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MedicalAmount { get; set; }
    /// <summary>
    /// 失业保险（元）
    /// </summary>
    [SugarColumn(ColumnName = "unemployment_amount", ColumnDescription = "失业保险", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal UnemploymentAmount { get; set; }
    /// <summary>
    /// 工伤保险（元）
    /// </summary>
    [SugarColumn(ColumnName = "injury_amount", ColumnDescription = "工伤保险", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal InjuryAmount { get; set; }
    /// <summary>
    /// 生育保险（元）
    /// </summary>
    [SugarColumn(ColumnName = "maternity_amount", ColumnDescription = "生育保险", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaternityAmount { get; set; }
    /// <summary>
    /// 公积金缴纳基数（元）
    /// </summary>
    [SugarColumn(ColumnName = "housing_fund_base", ColumnDescription = "公积金缴纳基数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HousingFundBase { get; set; }
    /// <summary>
    /// 公积金（元）
    /// </summary>
    [SugarColumn(ColumnName = "housing_fund_amount", ColumnDescription = "公积金", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HousingFundAmount { get; set; }
    /// <summary>
    /// 缴纳合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "total_amount", ColumnDescription = "缴纳合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status；0=待缴纳 1=已缴纳 2=已补缴）
    /// </summary>
    [SugarColumn(ColumnName = "pay_status", ColumnDescription = "缴纳状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int PayStatus { get; set; }
}

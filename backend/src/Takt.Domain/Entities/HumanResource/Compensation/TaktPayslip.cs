// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktPayslip.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：员工工资条（发薪期间应发/扣款/实发结果单据；由工资核算生成或手工维护）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 员工工资条（发薪结果单据，区别于 TaktEmpSalary 定薪档案）
/// </summary>
[SugarTable("takt_human_resource_compensation_payslip", "工资条表")]
[SugarIndex("ix_payslip_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_payslip_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_payslip_employee_period", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(PayPeriod), OrderByType.Asc, false)]
public class TaktPayslip : TaktCompanyEntityBase
{
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
    /// 发薪期间（如 2026-06）
    /// </summary>
    [SugarColumn(ColumnName = "pay_period", ColumnDescription = "发薪期间", ColumnDataType = "nvarchar", Length = 16, IsNullable = false)]
    public string PayPeriod { get; set; } = string.Empty;
    /// <summary>
    /// 基本工资（元）
    /// </summary>
    [SugarColumn(ColumnName = "base_salary", ColumnDescription = "基本工资", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal BaseSalary { get; set; }
    /// <summary>
    /// 岗位工资（元）
    /// </summary>
    [SugarColumn(ColumnName = "position_salary", ColumnDescription = "岗位工资", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal PositionSalary { get; set; }
    /// <summary>
    /// 绩效/奖金（元）
    /// </summary>
    [SugarColumn(ColumnName = "bonus_amount", ColumnDescription = "绩效奖金", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal BonusAmount { get; set; }
    /// <summary>
    /// 加班费（元）
    /// </summary>
    [SugarColumn(ColumnName = "overtime_pay", ColumnDescription = "加班费", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OvertimePay { get; set; }
    /// <summary>
    /// 津贴合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "allowance_total", ColumnDescription = "津贴合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AllowanceTotal { get; set; }
    /// <summary>
    /// 应发合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "gross_amount", ColumnDescription = "应发合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal GrossAmount { get; set; }
    /// <summary>
    /// 社保扣款（元）
    /// </summary>
    [SugarColumn(ColumnName = "social_security_deduction", ColumnDescription = "社保扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal SocialSecurityDeduction { get; set; }
    /// <summary>
    /// 公积金扣款（元）
    /// </summary>
    [SugarColumn(ColumnName = "housing_fund_deduction", ColumnDescription = "公积金扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal HousingFundDeduction { get; set; }
    /// <summary>
    /// 个税扣款（元）
    /// </summary>
    [SugarColumn(ColumnName = "tax_deduction", ColumnDescription = "个税扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal TaxDeduction { get; set; }
    /// <summary>
    /// 其他扣款（元）
    /// </summary>
    [SugarColumn(ColumnName = "other_deduction", ColumnDescription = "其他扣款", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal OtherDeduction { get; set; }
    /// <summary>
    /// 实发金额（元）
    /// </summary>
    [SugarColumn(ColumnName = "net_amount", ColumnDescription = "实发金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal NetAmount { get; set; }
    /// <summary>
    /// 公式方案编码（关联 TaktSalaryFormula.SetCode，核算时按同编码多行步骤顺序执行）
    /// </summary>
    [SugarColumn(ColumnName = "formula_set_code", ColumnDescription = "公式方案编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FormulaSetCode { get; set; }
    /// <summary>
    /// 发放日期
    /// </summary>
    [SugarColumn(ColumnName = "issue_date", ColumnDescription = "发放日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? IssueDate { get; set; }
    /// <summary>
    /// 发放状态（字典 humanresource_compensation_payslip_issue_status；0=待发放 1=已发放 2=已确认）
    /// </summary>
    [SugarColumn(ColumnName = "issue_status", ColumnDescription = "发放状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IssueStatus { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktEmpSalary.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：员工定薪档案；薪资项目定义见 TaktSalaryItem，体系见 TaktPayroll
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 员工薪酬档案（现金报酬定薪记录）
/// </summary>
[SugarTable("takt_human_resource_compensation_emp_salary", "员工薪酬表")]
[SugarIndex("ix_emp_salary_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_emp_salary_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_emp_salary_employee_effective", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(EffectiveDate), OrderByType.Desc, false)]
public class TaktEmpSalary : TaktCompanyEntityBase
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
    /// 薪酬体系（选项 TaktPayrolls/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "payroll_id", ColumnDescription = "薪酬体系ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }
    /// <summary>
    /// 薪级（选项 TaktPayScales/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "pay_scale_id", ColumnDescription = "薪级ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }
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
    /// 津贴合计（元）
    /// </summary>
    [SugarColumn(ColumnName = "allowance_total", ColumnDescription = "津贴合计", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal AllowanceTotal { get; set; }
    /// <summary>
    /// 薪资项目（选项 TaktSalaryItems/options；item_type=5 股权激励时使用，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "salary_item_id", ColumnDescription = "薪资项目ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryItemId { get; set; }
    /// <summary>
    /// 授予股数/份数（股权激励定薪时使用）
    /// </summary>
    [SugarColumn(ColumnName = "emp_salary_share_count", ColumnDescription = "授予股数", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal EmpSalaryShareCount { get; set; }
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "emp_salary_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EmpSalaryStatus { get; set; } = 1;
}

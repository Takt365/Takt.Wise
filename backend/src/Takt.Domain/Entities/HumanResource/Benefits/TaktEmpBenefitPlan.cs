// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Benefits
// 文件名称：TaktEmpBenefitPlan.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：员工福利方案（员工享有的福利项目配置与参保/参与记录）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Benefits;

/// <summary>
/// 员工福利方案（非现金福利参与配置）
/// </summary>
[SugarTable("takt_human_resource_benefits_emp_benefit_plan", "员工福利方案表")]
[SugarIndex("ix_emp_benefit_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_emp_benefit_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_emp_benefit_plan_employee_benefit", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(EmployeeId), OrderByType.Asc, nameof(BenefitItemId), OrderByType.Asc, false)]
public class TaktEmpBenefitPlan : TaktCompanyEntityBase
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
    /// 福利项目（选项 TaktBenefitItems/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "benefit_item_id", ColumnDescription = "福利项目ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long BenefitItemId { get; set; }
    /// <summary>
    /// 方案编码
    /// </summary>
    [SugarColumn(ColumnName = "plan_code", ColumnDescription = "方案编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlanCode { get; set; } = string.Empty;
    /// <summary>
    /// 参保/参与日期
    /// </summary>
    [SugarColumn(ColumnName = "enrollment_date", ColumnDescription = "参与日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EnrollmentDate { get; set; }
    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }
    /// <summary>
    /// 状态（字典 humanresource_benefits_emp_benefit_plan_status；0=待生效 1=生效中 2=已失效）
    /// </summary>
    [SugarColumn(ColumnName = "emp_benefit_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int EmpBenefitStatus { get; set; } = 1;
}

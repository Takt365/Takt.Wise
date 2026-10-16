// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktPayroll.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：薪酬体系主数据；具体现金报酬项由 TaktSalaryItem 配置，员工定薪见 TaktEmpSalary
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 薪酬体系（现金报酬方案头；组成项引用 TaktSalaryItem，不另建多种薪资实体）
/// </summary>
[SugarTable("takt_human_resource_payroll", "薪酬体系表")]
[SugarIndex("ix_payroll_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_payroll_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_payroll_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PayrollCode), OrderByType.Asc, true)]
public class TaktPayroll : TaktCompanyEntityBase
{
    /// <summary>
    /// 薪酬体系编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 薪酬体系编码规则生成并展示，非手输；单据类型菜单：薪酬体系）
    /// </summary>
    [SugarColumn(ColumnName = "payroll_code", ColumnDescription = "薪酬体系编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PayrollCode { get; set; } = string.Empty;
    /// <summary>
    /// 薪酬体系名称
    /// </summary>
    [SugarColumn(ColumnName = "payroll_name", ColumnDescription = "薪酬体系名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string PayrollName { get; set; } = string.Empty;
    /// <summary>
    /// 薪级表（选项 TaktPayScales/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "pay_scale_id", ColumnDescription = "薪级表ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayScaleId { get; set; }
    /// <summary>
    /// 公式方案编码（关联 TaktSalaryFormula.SetCode，同编码多行步骤；整单工资核算时使用）
    /// </summary>
    [SugarColumn(ColumnName = "formula_set_code", ColumnDescription = "公式方案编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? FormulaSetCode { get; set; }
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }
    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(ColumnName = "payroll_description", ColumnDescription = "说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? PayrollDescription { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "payroll_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PayrollStatus { get; set; } = 1;
}

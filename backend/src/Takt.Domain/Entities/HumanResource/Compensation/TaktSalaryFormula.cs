// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktSalaryFormula.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资计算公式（同一 set_code 下多行表示方案步骤；按 sort_order 顺序执行并写入 TaktPayslip）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 薪资计算公式（方案+步骤合一：set_code 分组，每行一步；标准五步：应发→社保→公积金→个税→实发）
/// 同一 set_code 示例：
/// gross_amount = base_salary + bonus_amount + overtime_pay + allowance_total
/// social_security_deduction = social_security_base * employee_ss_ratio
/// housing_fund_deduction = housing_fund_base * employee_hf_ratio
/// tax_deduction = CUMULATIVE_TAX(taxable_income)
/// net_amount = gross_amount - social_security_deduction - housing_fund_deduction - tax_deduction - other_deduction
/// </summary>
[SugarTable("takt_human_resource_compensation_salary_formula", "薪资计算公式表")]
[SugarIndex("ix_salary_formula_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_salary_formula_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_salary_formula_set_code", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SetCode), OrderByType.Asc, false)]
[SugarIndex("ix_salary_formula_set_step_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(SetCode), OrderByType.Asc, nameof(SortOrder), OrderByType.Asc, true)]
public class TaktSalaryFormula : TaktCompanyEntityBase
{
    /// <summary>
    /// 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识；前端表单选择编码规则后自动通过 TaktNumbering 公式方案编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）
    /// </summary>
    [SugarColumn(ColumnName = "set_code", ColumnDescription = "公式方案编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string SetCode { get; set; } = string.Empty;
    /// <summary>
    /// 公式方案名称
    /// </summary>
    [SugarColumn(ColumnName = "set_name", ColumnDescription = "公式方案名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string SetName { get; set; } = string.Empty;
    /// <summary>
    /// 薪酬体系（选项 TaktPayrolls/options；同 set_code 各行取值应一致，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "payroll_id", ColumnDescription = "薪酬体系ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? PayrollId { get; set; }
    /// <summary>
    /// 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET；前端表单选择编码规则后自动通过 TaktNumbering 步骤编码规则生成并展示，非手输；单据类型菜单：薪资计算公式）
    /// </summary>
    [SugarColumn(ColumnName = "formula_code", ColumnDescription = "步骤编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string FormulaCode { get; set; } = string.Empty;
    /// <summary>
    /// 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
    /// </summary>
    [SugarColumn(ColumnName = "formula_name", ColumnDescription = "步骤名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string FormulaName { get; set; } = string.Empty;
    /// <summary>
    /// 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type；1=应发 2=社保个人 3=公积金个人 4=个税 5=实发）
    /// </summary>
    [SugarColumn(ColumnName = "formula_step", ColumnDescription = "公式步骤", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int FormulaStep { get; set; } = 0;
    /// <summary>
    /// 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
    /// </summary>
    [SugarColumn(ColumnName = "target_field", ColumnDescription = "结果字段", ColumnDataType = "nvarchar", Length = 64, IsNullable = false)]
    public string TargetField { get; set; } = string.Empty;
    /// <summary>
    /// 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
    /// </summary>
    [SugarColumn(ColumnName = "formula_expression", ColumnDescription = "计算公式", ColumnDataType = "nvarchar", Length = 1000, IsNullable = false)]
    public string FormulaExpression { get; set; } = string.Empty;
    /// <summary>
    /// 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
    /// </summary>
    [SugarColumn(ColumnName = "step_description", ColumnDescription = "步骤说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? StepDescription { get; set; }
    /// <summary>
    /// 方案生效日期（同 set_code 各行应一致）
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// 方案失效日期
    /// </summary>
    [SugarColumn(ColumnName = "expiry_date", ColumnDescription = "失效日期", ColumnDataType = "date", IsNullable = true)]
    public DateTime? ExpiryDate { get; set; }
    /// <summary>
    /// 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "执行顺序", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "formula_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int FormulaStatus { get; set; } = 1;
}

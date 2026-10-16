// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktBonusPlan.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：奖金方案（规则型单据；单项奖金类型亦可在 TaktSalaryItem 字典中配置）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 奖金方案（现金奖金）
/// </summary>
[SugarTable("takt_human_resource_compensation_bonus_plan", "奖金方案表")]
[SugarIndex("ix_bonus_plan_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_bonus_plan_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_bonus_plan_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlanCode), OrderByType.Asc, true)]
public class TaktBonusPlan : TaktCompanyEntityBase
{
    /// <summary>
    /// 方案编码（租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 方案编码规则生成并展示，非手输；单据类型菜单：奖金方案）
    /// </summary>
    [SugarColumn(ColumnName = "plan_code", ColumnDescription = "方案编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string PlanCode { get; set; } = string.Empty;
    /// <summary>
    /// 方案名称
    /// </summary>
    [SugarColumn(ColumnName = "plan_name", ColumnDescription = "方案名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string PlanName { get; set; } = string.Empty;
    /// <summary>
    /// 奖金类型（字典 humanresource_compensation_bonus_type；1=绩效奖金 2=项目奖金 3=年终奖金 4=专项奖金）
    /// </summary>
    [SugarColumn(ColumnName = "bonus_type", ColumnDescription = "奖金类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int BonusType { get; set; } = 0;
    /// <summary>
    /// 计算方式（字典 humanresource_compensation_bonus_calc_method；1=固定金额 2=按比例 3=按公式）
    /// </summary>
    [SugarColumn(ColumnName = "calc_method", ColumnDescription = "计算方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CalcMethod { get; set; } = 0;
    /// <summary>
    /// 计算公式（选项 TaktSalaryFormulas/options；calc_method=3 按公式时使用，DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "salary_formula_id", ColumnDescription = "计算公式ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SalaryFormulaId { get; set; }
    /// <summary>
    /// 默认奖金金额或基数（元）
    /// </summary>
    [SugarColumn(ColumnName = "default_amount", ColumnDescription = "默认金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DefaultAmount { get; set; }
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "effective_date", ColumnDescription = "生效日期", ColumnDataType = "date", IsNullable = false)]
    public DateTime EffectiveDate { get; set; }
    /// <summary>
    /// 方案说明
    /// </summary>
    [SugarColumn(ColumnName = "bonus_plan_description", ColumnDescription = "方案说明", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? BonusPlanDescription { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "plan_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PlanStatus { get; set; } = 1;
}

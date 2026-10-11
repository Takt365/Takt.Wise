// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktSalaryItem.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资项目主数据（基本工资、岗位工资、津贴、奖金、股权激励等现金报酬项；类型由字典 humanresource_compensation_salary_item_type 区分）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 薪资项目（现金报酬可配置主数据，含股权激励；不另建 TaktStockOption 等平行实体）
/// </summary>
[SugarTable("takt_human_resource_compensation_salary_item", "薪资项目表")]
[SugarIndex("ix_salary_item_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_salary_item_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_salary_item_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ItemCode), OrderByType.Asc, true)]
public class TaktSalaryItem : TaktCompanyEntityBase
{
    /// <summary>
    /// 项目编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "item_code", ColumnDescription = "项目编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 项目名称
    /// </summary>
    [SugarColumn(ColumnName = "item_name", ColumnDescription = "项目名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ItemName { get; set; } = string.Empty;
    /// <summary>
    /// 简称
    /// </summary>
    [SugarColumn(ColumnName = "short_name", ColumnDescription = "简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ShortName { get; set; }
    /// <summary>
    /// 项目类型（字典 humanresource_compensation_salary_item_type；1=基本工资 2=岗位工资 3=津贴 4=奖金 5=股权激励）
    /// </summary>
    [SugarColumn(ColumnName = "item_type", ColumnDescription = "项目类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int ItemType { get; set; } = 0;
    /// <summary>
    /// 计算方式（字典 humanresource_compensation_salary_calc_method；1=固定金额 2=按比例 3=按公式）
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
    /// 默认金额（元）
    /// </summary>
    [SugarColumn(ColumnName = "default_amount", ColumnDescription = "默认金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal DefaultAmount { get; set; }
    /// <summary>
    /// 默认比例（%，0~100）
    /// </summary>
    [SugarColumn(ColumnName = "default_rate", ColumnDescription = "默认比例", ColumnDataType = "decimal", Length = 8, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DefaultRate { get; set; }
    /// <summary>
    /// 默认行权/授予价格（元；item_type=5 股权激励时使用）
    /// </summary>
    [SugarColumn(ColumnName = "strike_price", ColumnDescription = "默认行权价格", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal StrikePrice { get; set; }
    /// <summary>
    /// 默认归属年限（年；item_type=5 股权激励时使用）
    /// </summary>
    [SugarColumn(ColumnName = "vesting_years", ColumnDescription = "默认归属年限", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int VestingYears { get; set; }
    /// <summary>
    /// 是否扣款项（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_deduction", ColumnDescription = "是否扣款项", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsDeduction { get; set; } = 0;
    /// <summary>
    /// 是否计入应税所得（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_taxable", ColumnDescription = "是否计入应税所得", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsTaxable { get; set; } = 0;
    /// <summary>
    /// 是否计入社保基数（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "include_social_security_base", ColumnDescription = "是否计入社保基数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IncludeSocialSecurityBase { get; set; } = 0;
    /// <summary>
    /// 是否计入公积金基数（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "include_housing_fund_base", ColumnDescription = "是否计入公积金基数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IncludeHousingFundBase { get; set; } = 0;
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "item_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ItemStatus { get; set; } = 1;
}

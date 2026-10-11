// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Controlling
// 文件名称：TaktStandardWageRate.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工资率实体（按工厂、年月维护人工与加班数据）
// 计算公式：直接工资率 = (直接工资 + 直接加班总额) ÷ 销售额（销售额为 0 时取 0）
// 计算公式：间接工资率 = (间接工资 + 间接加班总额) ÷ 销售额（DirectWageRate、IndirectWageRate 可人工维护或由本式录入）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Controlling;

/// <summary>
/// 标准工资率实体
/// 直接工资率 = (直接工资 + 直接加班总额) ÷ 销售额；间接工资率 = (间接工资 + 间接加班总额) ÷ 销售额。
/// </summary>
[SugarTable("takt_accounting_controlling_standard_wage_rate", "标准工资率表")]
[SugarIndex("ix_standard_wage_rate_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_standard_wage_rate_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_standard_wage_rate_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(YearMonth), OrderByType.Asc, true)]
public class TaktStandardWageRate : TaktCompanyEntityBase
{
    /// <summary>
    /// 年月（yyyyMM）
    /// </summary>
    [SugarColumn(ColumnName = "year_month", ColumnDescription = "年月", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string YearMonth { get; set; } = string.Empty;
    /// <summary>
    /// 工作天数
    /// </summary>
    [SugarColumn(ColumnName = "working_days", ColumnDescription = "工作天数", ColumnDataType = "decimal", Length = 8, DecimalDigits = 2, IsNullable = false, DefaultValue = "21.7")]
    public decimal WorkingDays { get; set; } = 21.7m;
    /// <summary>
    /// 销售额
    /// </summary>
    [SugarColumn(ColumnName = "sales_amount", ColumnDescription = "销售额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal SalesAmount { get; set; }
    /// <summary>
    /// 直接人数
    /// </summary>
    [SugarColumn(ColumnName = "direct_labor_count", ColumnDescription = "直接人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int DirectLaborCount { get; set; }
    /// <summary>
    /// 直接工资
    /// </summary>
    [SugarColumn(ColumnName = "direct_labor_wage", ColumnDescription = "直接工资", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DirectLaborWage { get; set; }
    /// <summary>
    /// 直接加班小时
    /// </summary>
    [SugarColumn(ColumnName = "direct_overtime_hours", ColumnDescription = "直接加班小时", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DirectOvertimeHours { get; set; }
    /// <summary>
    /// 直接加班总额
    /// </summary>
    [SugarColumn(ColumnName = "direct_overtime_total", ColumnDescription = "直接加班总额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DirectOvertimeTotal { get; set; }
    /// <summary>
    /// 直接工资率
    /// </summary>
    [SugarColumn(ColumnName = "direct_wage_rate", ColumnDescription = "直接工资率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal DirectWageRate { get; set; }
    /// <summary>
    /// 间接人数
    /// </summary>
    [SugarColumn(ColumnName = "indirect_labor_count", ColumnDescription = "间接人数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IndirectLaborCount { get; set; }
    /// <summary>
    /// 间接工资
    /// </summary>
    [SugarColumn(ColumnName = "indirect_labor_wage", ColumnDescription = "间接工资", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectLaborWage { get; set; }
    /// <summary>
    /// 间接加班小时
    /// </summary>
    [SugarColumn(ColumnName = "indirect_overtime_hours", ColumnDescription = "间接加班小时", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectOvertimeHours { get; set; }
    /// <summary>
    /// 间接加班总额
    /// </summary>
    [SugarColumn(ColumnName = "indirect_overtime_total", ColumnDescription = "间接加班总额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectOvertimeTotal { get; set; }
    /// <summary>
    /// 间接工资率
    /// </summary>
    [SugarColumn(ColumnName = "indirect_wage_rate", ColumnDescription = "间接工资率", ColumnDataType = "decimal", Length = 18, DecimalDigits = 4, IsNullable = false, DefaultValue = "0")]
    public decimal IndirectWageRate { get; set; }
}

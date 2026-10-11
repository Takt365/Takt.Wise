// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.HumanResource.Compensation
// 文件名称：TaktPayScale.cs
// 创建时间：2026-06-12
// 创建人：Takt365(Cursor AI)
// 功能描述：薪级薪等表（现金报酬带宽与等级）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities;

namespace Takt.Domain.Entities.HumanResource.Compensation;

/// <summary>
/// 薪级薪等（现金报酬等级带宽）
/// </summary>
[SugarTable("takt_human_resource_compensation_pay_scale", "薪级表")]
[SugarIndex("ix_pay_scale_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_pay_scale_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_pay_scale_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ScaleCode), OrderByType.Asc, true)]
public class TaktPayScale : TaktCompanyEntityBase
{
    /// <summary>
    /// 薪级编码（租户+公司内唯一）
    /// </summary>
    [SugarColumn(ColumnName = "scale_code", ColumnDescription = "薪级编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ScaleCode { get; set; } = string.Empty;
    /// <summary>
    /// 薪级名称
    /// </summary>
    [SugarColumn(ColumnName = "scale_name", ColumnDescription = "薪级名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string ScaleName { get; set; } = string.Empty;
    /// <summary>
    /// 等级（数字越大等级越高）
    /// </summary>
    [SugarColumn(ColumnName = "grade_level", ColumnDescription = "等级", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GradeLevel { get; set; }
    /// <summary>
    /// 下限金额（元）
    /// </summary>
    [SugarColumn(ColumnName = "min_salary", ColumnDescription = "下限金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MinSalary { get; set; }
    /// <summary>
    /// 中位金额（元）
    /// </summary>
    [SugarColumn(ColumnName = "mid_salary", ColumnDescription = "中位金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MidSalary { get; set; }
    /// <summary>
    /// 上限金额（元）
    /// </summary>
    [SugarColumn(ColumnName = "max_salary", ColumnDescription = "上限金额", ColumnDataType = "decimal", Length = 18, DecimalDigits = 2, IsNullable = false, DefaultValue = "0")]
    public decimal MaxSalary { get; set; }
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 状态（字典 sys_normal_disable；0=禁用 1=启用 2=锁定）
    /// </summary>
    [SugarColumn(ColumnName = "scale_status", ColumnDescription = "状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int ScaleStatus { get; set; } = 1;
}

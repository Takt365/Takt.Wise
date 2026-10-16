// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Accounting.Controlling
// 文件名称：TaktCostElement.cs
// 创建时间：2025-03-16
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Accounting.Controlling;

/// <summary>
/// 成本要素实体
/// </summary>
[SugarTable("takt_accounting_controlling_cost_element", "成本要素表")]
[SugarIndex("ix_cost_element_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_cost_element_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_cost_element_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(CostElementCode), OrderByType.Asc, true)]
[SugarIndex("ix_cost_element_parent", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
public class TaktCostElement : TaktCompanyEntityBase
{    /// <summary>
    /// 成本要素编码（4位，租户+公司内唯一；前端表单选择编码规则后自动通过 TaktNumbering 成本要素编码规则生成并展示，非手输；单据类型菜单：成本要素）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_code", ColumnDescription = "成本要素编码", ColumnDataType = "varchar", Length = 4, IsNullable = false)]
    public string CostElementCode { get; set; } = string.Empty;
    /// <summary>
    /// 成本要素名称
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_name", ColumnDescription = "成本要素名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string CostElementName { get; set; } = string.Empty;
    /// <summary>
    /// 成本要素类型（字典 accounting_controlling_cost_element_type；0=初级，1=次级）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_type", ColumnDescription = "成本要素类型", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CostElementType { get; set; }
    /// <summary>
    /// 成本要素类别（字典 accounting_controlling_cost_element_category-KATYP，整型存 1/3/4/11…）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_category", ColumnDescription = "成本要素类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CostElementCategory { get; set; } = 1;
    /// <summary>
    /// 父级 ID
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; }
    /// <summary>
    /// 成本要素层级
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_level", ColumnDescription = "成本要素层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CostElementLevel { get; set; } = 1;
    /// <summary>
    /// 生效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_from", ColumnDescription = "生效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidFrom { get; set; } = DateTime.Today;
    /// <summary>
    /// 失效日期
    /// </summary>
    [SugarColumn(ColumnName = "valid_to", ColumnDescription = "失效日期", ColumnDataType = "datetime", IsNullable = false)]
    public DateTime ValidTo { get; set; } = new DateTime(9999, 12, 31, 23, 59, 59);
    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; }
    /// <summary>
    /// 成本要素状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "cost_element_status", ColumnDescription = "成本要素状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int CostElementStatus { get; set; } = 1;
}

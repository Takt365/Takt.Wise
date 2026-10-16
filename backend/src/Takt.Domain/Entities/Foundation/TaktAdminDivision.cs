// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Foundation
// 文件名称：TaktAdminDivision.cs
// 创建时间：2026-07-22
// 创建人：Takt365(Cursor AI)
// 功能描述：世界通用六级行政区划实体（租户级树表；国家→州省→地市→区县→乡镇街道→行政村）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Foundation;

/// <summary>
/// 行政区划实体（租户级共享；世界通用六级树）
/// 层级：1=国家，2=州省，3=地市，4=区县，5=乡镇街道，6=行政村（字典 sys_admin_division_level）
/// 编码可对齐 ISO 3166、ISO 3166-2、GB/T 2260、JIS 等；子节点 CountryCode 冗余自根国家便于过滤
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// </summary>
[SugarTable("takt_foundation_admin_division", "行政区划表")]
[SugarIndex("ix_admin_division_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_admin_division_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_admin_division_code_unique", nameof(TenantCode), OrderByType.Asc, nameof(DivisionCode), OrderByType.Asc, true)]
[SugarIndex("ix_admin_division_parent", nameof(TenantCode), OrderByType.Asc, nameof(ParentId), OrderByType.Asc, false)]
[SugarIndex("ix_admin_division_country", nameof(TenantCode), OrderByType.Asc, nameof(CountryCode), OrderByType.Asc, false)]
[SugarIndex("ix_admin_division_level", nameof(TenantCode), OrderByType.Asc, nameof(Level), OrderByType.Asc, false)]
public class TaktAdminDivision : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
    /// </summary>
    [SugarColumn(ColumnName = "country_code", ColumnDescription = "国家代码", ColumnDataType = "nvarchar", Length = 2, IsNullable = false)]
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106；前端表单选择编码规则后自动通过 TaktNumbering 区划编码规则生成并展示，非手输；单据类型菜单：行政区划）
    /// </summary>
    [SugarColumn(ColumnName = "division_code", ColumnDescription = "区划编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string DivisionCode { get; set; } = string.Empty;

    /// <summary>
    /// 区划名称（本国语言官方/本地显示名）
    /// </summary>
    [SugarColumn(ColumnName = "division_name", ColumnDescription = "区划名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string DivisionName { get; set; } = string.Empty;

    /// <summary>
    /// 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
    /// </summary>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级区划ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentId { get; set; } = 0;

    /// <summary>
    /// 层级（字典 sys_admin_division_level；1～6）
    /// </summary>
    [SugarColumn(ColumnName = "level", ColumnDescription = "层级", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int Level { get; set; } = 1;

    /// <summary>
    /// 区划路径（如 /1/3/5/，用于快速查询子孙）
    /// </summary>
    [SugarColumn(ColumnName = "division_path", ColumnDescription = "区划路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "")]
    public string DivisionPath { get; set; } = string.Empty;

    /// <summary>
    /// 是否叶子节点（字典 sys_yes_no）
    /// </summary>
    [SugarColumn(ColumnName = "is_leaf", ColumnDescription = "是否叶子节点", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsLeaf { get; set; } = 0;

    /// <summary>
    /// 邮政编码（可选；部分国家区划关联邮编）
    /// </summary>
    [SugarColumn(ColumnName = "postal_code", ColumnDescription = "邮政编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? PostalCode { get; set; }


    /// <summary>
    /// 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
    /// </summary>
    [SugarColumn(ColumnName = "currency_code", ColumnDescription = "币种", ColumnDataType = "varchar", Length = 3, IsNullable = false, DefaultValue = "")]
    public string CurrencyCode { get; set; } = string.Empty;

    /// <summary>
    /// 电话区号（国际电话区号，如 +86、+81）
    /// </summary>
    [SugarColumn(ColumnName = "phone_code", ColumnDescription = "电话区号", ColumnDataType = "varchar", Length = 16, IsNullable = false, DefaultValue = "")]
    public string PhoneCode { get; set; } = string.Empty;

    /// <summary>
    /// 内置（字典 sys_yes_no；内置项禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 区划状态（字典 sys_normal_disable）
    /// </summary>
    [SugarColumn(ColumnName = "division_status", ColumnDescription = "区划状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int DivisionStatus { get; set; } = 1;
}

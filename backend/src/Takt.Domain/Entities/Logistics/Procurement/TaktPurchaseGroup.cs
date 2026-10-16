// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktPurchaseGroup.cs
// 创建时间：2026-06-15
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt采购组主数据实体，定义采购组织分组（与物料工厂视图、采购订单 PurchaseGroup 对齐）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt采购组主数据实体（公司级；采购业务组织分组）
/// </summary>
[SugarTable("takt_logistics_procurement_purchase_group", "采购组主数据表")]
[SugarIndex("ix_purchase_group_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_purchase_group_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_purchase_group_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, nameof(PurchaseGroupCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_purchase_group_plant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(PlantCode), OrderByType.Asc, false)]
public class TaktPurchaseGroup : TaktCompanyEntityBase
{
    /// <summary>
    /// 采购组编码（与员工编码同长 6；前端表单选择编码规则后自动通过 TaktNumbering 采购组编码规则生成并展示，非手输；单据类型菜单：采购组）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group_code", ColumnDescription = "采购组编码", ColumnDataType = "varchar", Length = 6, IsNullable = false)]
    public string PurchaseGroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 采购组名称
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group_name", ColumnDescription = "采购组名称", ColumnDataType = "nvarchar", Length = 80, IsNullable = false)]
    public string PurchaseGroupName { get; set; } = string.Empty;

    /// <summary>
    /// 采购组描述
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group_description", ColumnDescription = "采购组描述", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? PurchaseGroupDescription { get; set; }

    /// <summary>
    /// 采购组类别（字典 logistics_procurement_purchase_group_category；1=通用）
    /// </summary>
    [SugarColumn(ColumnName = "purchase_group_category", ColumnDescription = "采购组类别", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int PurchaseGroupCategory { get; set; } = 1;

    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnName = "contact_phone", ColumnDescription = "联系电话", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [SugarColumn(ColumnName = "contact_email", ColumnDescription = "联系邮箱", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
    /// </summary>
    [SugarColumn(ColumnName = "is_built_in", ColumnDescription = "内置", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsBuiltIn { get; set; } = 0;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [SugarColumn(ColumnName = "sort_order", ColumnDescription = "排序号", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 采购组状态（字典 sys_normal_disable；1=启用，0=禁用）
    /// </summary>
    [SugarColumn(ColumnName = "group_status", ColumnDescription = "采购组状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int GroupStatus { get; set; } = 1;
}

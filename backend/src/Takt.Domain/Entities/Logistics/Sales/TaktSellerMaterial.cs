// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Sales
// 文件名称：TaktSellerMaterial.cs
// 创建时间：2026-08-04
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt销售商物料实体（组合4仅租户；无工厂/无语言）；以客户/客户端业务编码关联，无外键 Id、无导航
//
// 业务语义说明：
// 记录销售商侧物料编码与本厂物料的对应关系，不涉及商务价格；
// 价格和交易信息应在 TaktSalesCondition/TaktSalesConditionItem 中管理
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Sales;

/// <summary>
/// Takt销售商物料实体（租户内共享）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// </summary>
[SugarTable("takt_logistics_sales_seller_material", "销售商物料表")]
[SugarIndex("ix_takt_logistics_sales_seller_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_seller_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(InternalMaterialCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_sales_seller_material_customer_code", nameof(TenantCode), OrderByType.Asc, nameof(CustomerCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_seller_material_client_code", nameof(TenantCode), OrderByType.Asc, nameof(ClientCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_sales_seller_material_seller_material_code", nameof(TenantCode), OrderByType.Asc, nameof(SellerMaterialCode), OrderByType.Asc, false)]
public class TaktSellerMaterial : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    [SugarColumn(ColumnName = "customer_code", ColumnDescription = "客户编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "customer_short_name", ColumnDescription = "客户简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? CustomerShortName { get; set; }

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    [SugarColumn(ColumnName = "client_code", ColumnDescription = "客户端编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? ClientCode { get; set; }

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "client_short_name", ColumnDescription = "客户端简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? ClientShortName { get; set; }

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    [SugarColumn(ColumnName = "material_type", ColumnDescription = "物料类型", ColumnDataType = "nvarchar", Length = 4, IsNullable = false, DefaultValue = "HERS")]
    public string MaterialType { get; set; } = "HERS";

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_group", ColumnDescription = "物料组", ColumnDataType = "nvarchar", Length = 9, IsNullable = false)]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    [SugarColumn(ColumnName = "internal_material_code", ColumnDescription = "内部物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [SugarColumn(ColumnName = "material_code", ColumnDescription = "物料编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = false)]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（冗余：按 MaterialCode 取 TaktMaterialPlant.MaterialDescription联动）
    /// </summary>
    [SugarColumn(ColumnName = "material_description", ColumnDescription = "物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料编码（销售商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 销售商物料编码规则生成并展示，非手输；单据类型菜单：销售商物料）
    /// </summary>
    [SugarColumn(ColumnName = "seller_material_code", ColumnDescription = "销售商物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    [SugarColumn(ColumnName = "seller_material_description", ColumnDescription = "销售商物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    [SugarColumn(ColumnName = "seller_material_specification", ColumnDescription = "销售商物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? SellerMaterialSpecification { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Logistics.Procurement
// 文件名称：TaktManufacturerMaterial.cs
// 创建时间：2026-05-13
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt制造商物料实体（组合4仅租户；无工厂/无语言）；以经销商/供货商业务编码关联，无外键 Id、无导航
//
// 业务语义说明：
// 记录制造商物料编码与本厂物料的对应关系，不涉及商务价格；
// 价格和交易信息应在 TaktPurchaseInfo/TaktPurchaseInfoItem 中管理
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Logistics.Procurement;

/// <summary>
/// Takt制造商物料实体（租户内共享）
/// 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// </summary>
[SugarTable("takt_logistics_procurement_manufacturer_material", "制造商物料表")]
[SugarIndex("ix_takt_logistics_procurement_manufacturer_material_tenant", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_manufacturer_material_unique", nameof(TenantCode), OrderByType.Asc, nameof(InternalMaterialCode), OrderByType.Asc, nameof(MaterialCode), OrderByType.Asc, true)]
[SugarIndex("ix_takt_logistics_procurement_manufacturer_material_vendor_code", nameof(TenantCode), OrderByType.Asc, nameof(VendorCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_manufacturer_material_supplier_code", nameof(TenantCode), OrderByType.Asc, nameof(SupplierCode), OrderByType.Asc, false)]
[SugarIndex("ix_takt_logistics_procurement_manufacturer_material_manufacturer_material_code", nameof(TenantCode), OrderByType.Asc, nameof(ManufacturerMaterialCode), OrderByType.Asc, false)]
public class TaktManufacturerMaterial : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_code", ColumnDescription = "经销商编码", ColumnDataType = "nvarchar", Length = 20, IsNullable = true)]
    public string? VendorCode { get; set; }

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "vendor_short_name", ColumnDescription = "经销商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? VendorShortName { get; set; }

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_code", ColumnDescription = "供货商编码", ColumnDataType = "nvarchar", Length = 10, IsNullable = true)]
    public string? SupplierCode { get; set; }

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    [SugarColumn(ColumnName = "supplier_short_name", ColumnDescription = "供货商简称", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? SupplierShortName { get; set; }

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
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
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
    /// 制造商物料编码（制造商内部的物料编码；前端表单选择编码规则后自动通过 TaktNumbering 制造商物料编码规则生成并展示，非手输；单据类型菜单：制造商物料）
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_code", ColumnDescription = "制造商物料编码", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_description", ColumnDescription = "制造商物料描述", ColumnDataType = "nvarchar", Length = 40, IsNullable = false)]
    public string ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    [SugarColumn(ColumnName = "manufacturer_material_specification", ColumnDescription = "制造商物料规格", ColumnDataType = "nvarchar", Length = 70, IsNullable = true)]
    public string? ManufacturerMaterialSpecification { get; set; }
}

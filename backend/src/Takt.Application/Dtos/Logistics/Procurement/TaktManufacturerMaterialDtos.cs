// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktManufacturerMaterialDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ManufacturerMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktManufacturerMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Procurement;

// ========================================
// ManufacturerMaterial 响应 DTO
// ========================================

/// <summary>
/// Takt制造商物料实体（租户内共享）
/// 对应前端 TaktManufacturerMaterialDto
/// 继承 TaktTenantCoreDtoBase（组合 4）
/// </summary>
public class TaktManufacturerMaterialDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// ManufacturerMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerMaterialId { get; set; }

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    public string InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    public string ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    public string ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

}

// ========================================
// ManufacturerMaterial 查询 DTO
// ========================================

/// <summary>
/// ManufacturerMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktManufacturerMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    public string? InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    public string? ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ManufacturerMaterial DTO
// ========================================

/// <summary>
/// 创建ManufacturerMaterial DTO
/// </summary>
public class TaktManufacturerMaterialCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    [Required(ErrorMessage = "物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）不能为空")]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    [Required(ErrorMessage = "物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）不能为空")]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    [Required(ErrorMessage = "内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）不能为空")]
    public string InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    [Required(ErrorMessage = "物料描述（回填：随物料）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    [Required(ErrorMessage = "制造商物料编码（制造商内部的物料编码）不能为空")]
    public string ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    [Required(ErrorMessage = "制造商物料描述不能为空")]
    public string ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ManufacturerMaterial DTO
// ========================================

/// <summary>
/// 更新ManufacturerMaterial DTO
/// 继承 TaktManufacturerMaterialCreateDto，添加 ManufacturerMaterialId 字段
/// </summary>
public class TaktManufacturerMaterialUpdateDto : TaktManufacturerMaterialCreateDto
{
    /// <summary>
    /// ManufacturerMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerMaterialId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ManufacturerMaterial 导入模板行 DTO
/// </summary>
public class TaktManufacturerMaterialTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    public string? InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    public string? ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ManufacturerMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktManufacturerMaterialImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    public string? InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    public string? ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    public string? ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// ManufacturerMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktManufacturerMaterialExportDto
{
    /// <summary>
    /// ManufacturerMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ManufacturerMaterialId { get; set; }

    /// <summary>
    /// 经销商编码（选项 TaktVendors/options；DictValue=VendorCode；可空）
    /// </summary>
    public string? VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// 经销商简称（冗余）
    /// </summary>
    public string? VendorShortName { get; set; } = string.Empty;

    /// <summary>
    /// 供货商编码（选项 TaktSuppliers/options；DictValue=SupplierCode；可空）
    /// </summary>
    public string? SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// 供货商简称（冗余）
    /// </summary>
    public string? SupplierShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多制造商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    public string InternalMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：随物料）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料编码（制造商内部的物料编码）
    /// </summary>
    public string ManufacturerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料描述
    /// </summary>
    public string ManufacturerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 制造商物料规格
    /// </summary>
    public string? ManufacturerMaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

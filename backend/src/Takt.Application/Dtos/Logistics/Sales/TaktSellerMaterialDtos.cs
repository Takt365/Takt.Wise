// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSellerMaterialDtos.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Auto Generated)
// 功能描述：SellerMaterial 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSellerMaterial 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Sales;

// ========================================
// SellerMaterial 响应 DTO
// ========================================

/// <summary>
/// Takt销售商物料实体（租户内共享） 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
/// 对应前端 TaktSellerMaterialDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktSellerMaterialDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// SellerMaterialID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SellerMaterialId { get; set; }

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    public string SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    public string SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

}

// ========================================
// SellerMaterial 查询 DTO
// ========================================

/// <summary>
/// SellerMaterial 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSellerMaterialQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    public string? SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    public string? SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

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
// 创建SellerMaterial DTO
// ========================================

/// <summary>
/// 创建SellerMaterial DTO
/// </summary>
public class TaktSellerMaterialCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

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
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
    /// </summary>
    [Required(ErrorMessage = "内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）不能为空")]
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    [Required(ErrorMessage = "销售商物料编码（销售商内部的物料编码）不能为空")]
    public string SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    [Required(ErrorMessage = "销售商物料描述不能为空")]
    public string SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

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
// 更新SellerMaterial DTO
// ========================================

/// <summary>
/// 更新SellerMaterial DTO
/// 继承 TaktSellerMaterialCreateDto，添加 SellerMaterialId 字段
/// </summary>
public class TaktSellerMaterialUpdateDto : TaktSellerMaterialCreateDto
{
    /// <summary>
    /// SellerMaterialID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SellerMaterialId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SellerMaterial 导入模板行 DTO
/// </summary>
public class TaktSellerMaterialTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    public string? SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    public string? SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

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
/// SellerMaterial 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSellerMaterialImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string? MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string? MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    public string? SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    public string? SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

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
/// SellerMaterial 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSellerMaterialExportDto
{
    /// <summary>
    /// SellerMaterialID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SellerMaterialId { get; set; }

    /// <summary>
    /// 客户编码（选项 TaktCustomers/options；DictValue=CustomerCode；可空）
    /// </summary>
    public string? CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户简称（冗余）
    /// </summary>
    public string? CustomerShortName { get; set; } = string.Empty;

    /// <summary>
    /// 客户端编码（选项 TaktClients/options；DictValue=ClientCode；可空）
    /// </summary>
    public string? ClientCode { get; set; } = string.Empty;

    /// <summary>
    /// 客户端简称（冗余）
    /// </summary>
    public string? ClientShortName { get; set; } = string.Empty;

    /// <summary>
    /// 物料类型（字典 logistics_materials_material_type；DictValue=ROH/HALB/HERS 等；默认 HERS）
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// 物料组（选项 TaktMaterialGroups/options；DictValue=MaterialGroupCode）
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// 内部物料编码（物料编码后缀区分多销售商/多来源，如物料编码+1、+2、+3）
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
    /// 销售商物料编码（销售商内部的物料编码）
    /// </summary>
    public string SellerMaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料描述
    /// </summary>
    public string SellerMaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 销售商物料规格
    /// </summary>
    public string? SellerMaterialSpecification { get; set; } = string.Empty;

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

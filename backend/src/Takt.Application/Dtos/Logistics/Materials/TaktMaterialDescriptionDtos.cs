// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktMaterialDescriptionDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MaterialDescription 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMaterialDescription 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Materials;

// ========================================
// MaterialDescription 响应 DTO
// ========================================

/// <summary>
/// Takt物料多语言描述实体（租户级）
/// 对应前端 TaktMaterialDescriptionDto
/// 继承 TaktTenantDtoBase
/// </summary>
public class TaktMaterialDescriptionDto : TaktTenantCultureDtoBase
{
    /// <summary>
    /// MaterialDescriptionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDescriptionId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

}

// ========================================
// MaterialDescription 查询 DTO
// ========================================

/// <summary>
/// MaterialDescription 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMaterialDescriptionQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

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
// 创建MaterialDescription DTO
// ========================================

/// <summary>
/// 创建MaterialDescription DTO
/// </summary>
public class TaktMaterialDescriptionCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    [Required(ErrorMessage = "物料描述不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

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
// 更新MaterialDescription DTO
// ========================================

/// <summary>
/// 更新MaterialDescription DTO
/// 继承 TaktMaterialDescriptionCreateDto，添加 MaterialDescriptionId 字段
/// </summary>
public class TaktMaterialDescriptionUpdateDto : TaktMaterialDescriptionCreateDto
{
    /// <summary>
    /// MaterialDescriptionID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDescriptionId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MaterialDescription 导入模板行 DTO
/// </summary>
public class TaktMaterialDescriptionTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

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
/// MaterialDescription 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMaterialDescriptionImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? RelatedPlant { get; set; } = string.Empty;
    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

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
/// MaterialDescription 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMaterialDescriptionExportDto
{
    /// <summary>
    /// MaterialDescriptionID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MaterialDescriptionId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktGeneralMaterials/options；DictValue=MaterialCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 物料规格
    /// </summary>
    public string? MaterialSpecification { get; set; } = string.Empty;

    /// <summary>
    /// 物料型号
    /// </summary>
    public string? MaterialModel { get; set; } = string.Empty;

    /// <summary>
    /// 物料长描述
    /// </summary>
    public string? MaterialLongDescription { get; set; } = string.Empty;

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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Materials
// 文件名称：TaktModelDestinationDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ModelDestination 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktModelDestination 生成，请按需审阅）
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
// ModelDestination 响应 DTO
// ========================================

/// <summary>
/// Takt型号目的地实体（租户级；物料编码/名称、机种编码/名称、仕向地编码/名称）
/// 对应前端 TaktModelDestinationDto
/// 继承 TaktTenantCoreDtoBase（组合 4）
/// </summary>
public class TaktModelDestinationDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// ModelDestinationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ModelDestinationId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    public string DestinationName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

}

// ========================================
// ModelDestination 查询 DTO
// ========================================

/// <summary>
/// ModelDestination 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktModelDestinationQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    public string? DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    public string? DestinationName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int? SortOrder { get; set; }

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
// 创建ModelDestination DTO
// ========================================

/// <summary>
/// 创建ModelDestination DTO
/// </summary>
public class TaktModelDestinationCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    [Required(ErrorMessage = "物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）不能为空")]
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    [Required(ErrorMessage = "物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）不能为空")]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    [Required(ErrorMessage = "机种编码（40）不能为空")]
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    [Required(ErrorMessage = "机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）不能为空")]
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    [Required(ErrorMessage = "仕向地编码（40）不能为空")]
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    [Required(ErrorMessage = "仕向地名称（80）不能为空")]
    public string DestinationName { get; set; } = string.Empty;

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
// 更新ModelDestination DTO
// ========================================

/// <summary>
/// 更新ModelDestination DTO
/// 继承 TaktModelDestinationCreateDto，添加 ModelDestinationId 字段
/// </summary>
public class TaktModelDestinationUpdateDto : TaktModelDestinationCreateDto
{
    /// <summary>
    /// ModelDestinationID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ModelDestinationId { get; set; }

}

// ========================================
// ModelDestination 排序 DTO
// ========================================

/// <summary>
/// ModelDestination 排序更新 DTO
/// </summary>
public class TaktModelDestinationSortDto
{
    /// <summary>
    /// ModelDestinationID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ModelDestinationId { get; set; }

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    [Required(ErrorMessage = "排序号（越小越靠前）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ModelDestination 导入模板行 DTO
/// </summary>
public class TaktModelDestinationTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    public string? DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    public string? DestinationName { get; set; } = string.Empty;

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
/// ModelDestination 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktModelDestinationImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string? MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    public string? MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    public string? ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    public string? ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    public string? DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    public string? DestinationName { get; set; } = string.Empty;

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
/// ModelDestination 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktModelDestinationExportDto
{
    /// <summary>
    /// ModelDestinationID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ModelDestinationId { get; set; }

    /// <summary>
    /// 物料编码（选项 TaktMaterialPlants/options；DictValue=MaterialCode，ExtValue=PlantCode）
    /// </summary>
    public string MaterialCode { get; set; } = string.Empty;

    /// <summary>
    /// 物料描述（回填：按 MaterialCode 取物料描述表 culture_code=ja-JP）
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// 机种编码（40）
    /// </summary>
    public string ModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 机种名称（回填：按 ModelCode 取物料描述表 culture_code=Z1）
    /// </summary>
    public string ModelName { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地编码（40）
    /// </summary>
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// 仕向地名称（80）
    /// </summary>
    public string DestinationName { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）（越小越靠前）
    /// </summary>
    public int SortOrder { get; set; } = 0;

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

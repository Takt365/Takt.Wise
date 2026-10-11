// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopStepMediaDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopStepMedia 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopStepMedia 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Sop;

// ========================================
// SopStepMedia 响应 DTO
// ========================================

/// <summary>
/// SOP 工步多媒体实体
/// 对应前端 TaktSopStepMediaDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopStepMediaDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopStepMediaID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepMediaId { get; set; }

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 工步 名称（填充字段）
    /// </summary>
    public string? StepName { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int MediaType { get; set; } = 0;

    /// <summary>
    /// 文件 URL
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// 工步
    /// （主表：TaktSopStep）
    /// </summary>
    public TaktSopStepDto? Step { get; set; }

}

// ========================================
// SopStepMedia 查询 DTO
// ========================================

/// <summary>
/// SopStepMedia 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopStepMediaQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int? MediaType { get; set; }

    /// <summary>
    /// 文件 URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
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
// 创建SopStepMedia DTO
// ========================================

/// <summary>
/// 创建SopStepMedia DTO
/// </summary>
public class TaktSopStepMediaCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int MediaType { get; set; } = 0;

    /// <summary>
    /// 文件 URL
    /// </summary>
    [Required(ErrorMessage = "文件 URL不能为空")]
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

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
// 更新SopStepMedia DTO
// ========================================

/// <summary>
/// 更新SopStepMedia DTO
/// 继承 TaktSopStepMediaCreateDto，添加 SopStepMediaId 字段
/// </summary>
public class TaktSopStepMediaUpdateDto : TaktSopStepMediaCreateDto
{
    /// <summary>
    /// SopStepMediaID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepMediaId { get; set; }

}

// ========================================
// SopStepMedia 排序 DTO
// ========================================

/// <summary>
/// SopStepMedia 排序更新 DTO
/// </summary>
public class TaktSopStepMediaSortDto
{
    /// <summary>
    /// SopStepMediaID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepMediaId { get; set; }

    /// <summary>
    /// 排序号（回填）
    /// </summary>
    [Required(ErrorMessage = "排序号（回填）不能为空")]
    public int SortOrder { get; set; } = 0;
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopStepMedia 导入模板行 DTO
/// </summary>
public class TaktSopStepMediaTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int? MediaType { get; set; }

    /// <summary>
    /// 文件 URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

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
/// SopStepMedia 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopStepMediaImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int? MediaType { get; set; }

    /// <summary>
    /// 文件 URL
    /// </summary>
    public string? FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

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
/// SopStepMedia 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopStepMediaExportDto
{
    /// <summary>
    /// SopStepMediaID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopStepMediaId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 工步 ID（选项 TaktSopSteps/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long StepId { get; set; }

    /// <summary>
    /// 媒体类型（字典 logistics_sop_media_type；1=图片JPG/PNG，2=视频MP4，3=PDF，4=3D轻量化）
    /// </summary>
    public int MediaType { get; set; } = 0;

    /// <summary>
    /// 文件 URL
    /// </summary>
    public string FileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名（jpg/png/mp4/pdf/glb 等）
    /// </summary>
    public string? FileExt { get; set; } = string.Empty;

    /// <summary>
    /// 排序号（回填）
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

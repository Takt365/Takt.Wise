// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Sop
// 文件名称：TaktSopContentDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：SopContent 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktSopContent 生成，请按需审阅）
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
// SopContent 响应 DTO
// ========================================

/// <summary>
/// SOP 多语言正文实体
/// 对应前端 TaktSopContentDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktSopContentDto : TaktCompanyDtoBase
{
    /// <summary>
    /// SopContentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopContentId { get; set; }

    /// <summary>
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// 版本 名称（填充字段）
    /// </summary>
    public string? RevisionName { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// SOP 主档 名称（填充字段）
    /// </summary>
    public string? SopName { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 版本
    /// （主表：TaktSopRevision）
    /// </summary>
    public TaktSopRevisionDto? Revision { get; set; }

    /// <summary>
    /// 工步列表
    /// （子表：TaktSopStep）
    /// </summary>
    public List<TaktSopStepDto>? Steps { get; set; }

}

// ========================================
// SopContent 查询 DTO
// ========================================

/// <summary>
/// SopContent 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktSopContentQueryDto : TaktPagedQuery
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
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

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
// 创建SopContent DTO
// ========================================

/// <summary>
/// 创建SopContent DTO
/// </summary>
public class TaktSopContentCreateDto
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
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工步列表（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCreateDto>? Steps { get; set; }

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
// 更新SopContent DTO
// ========================================

/// <summary>
/// 更新SopContent DTO
/// 继承 TaktSopContentCreateDto，添加 SopContentId 字段
/// </summary>
public class TaktSopContentUpdateDto : TaktSopContentCreateDto
{
    /// <summary>
    /// SopContentID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopContentId { get; set; }

    /// <summary>
    /// 工步列表（子表，级联保存）
    /// </summary>
    public new List<TaktSopStepUpdateDto>? Steps { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// SopContent 导入模板行 DTO
/// </summary>
public class TaktSopContentTemplateDto
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
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工步列表（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCreateDto>? Steps { get; set; }

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
/// SopContent 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktSopContentImportDto
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
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

    /// <summary>
    /// 工步列表（子表，级联保存）
    /// </summary>
    public List<TaktSopStepCreateDto>? Steps { get; set; }

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
/// SopContent 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktSopContentExportDto
{
    /// <summary>
    /// SopContentID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopContentId { get; set; }

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
    /// 版本 ID（选项 TaktSopRevisions/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long RevisionId { get; set; }

    /// <summary>
    /// SOP 主档 ID（选项 TaktSopDocs/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long SopId { get; set; }

    /// <summary>
    /// 正文标题
    /// </summary>
    public string? ContentTitle { get; set; } = string.Empty;

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

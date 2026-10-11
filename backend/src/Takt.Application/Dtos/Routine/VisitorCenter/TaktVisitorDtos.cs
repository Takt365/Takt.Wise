// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.VisitorCenter
// 文件名称：TaktVisitorDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：Visitor 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktVisitor 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.VisitorCenter;

// ========================================
// Visitor 响应 DTO
// ========================================

/// <summary>
/// 来访接待主实体（来访公司及参访起止时间）
/// 对应前端 TaktVisitorDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktVisitorDto : TaktCompanyDtoBase
{
    /// <summary>
    /// VisitorID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 来访公司名称
    /// </summary>
    public string VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间
    /// </summary>
    public DateTime VisitStartTime { get; set; }

    /// <summary>
    /// 参访结束时间
    /// </summary>
    public DateTime VisitEndTime { get; set; }

    /// <summary>
    /// 来访人员列表
    /// （子表：TaktVisitorCompanion）
    /// </summary>
    public List<TaktVisitorCompanionDto>? Companions { get; set; }

}

// ========================================
// Visitor 查询 DTO
// ========================================

/// <summary>
/// Visitor 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktVisitorQueryDto : TaktPagedQuery
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
    /// 来访公司名称
    /// </summary>
    public string? VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间（范围查询-开始）
    /// </summary>
    public DateTime? VisitStartTimeStart { get; set; }

    /// <summary>
    /// 参访开始时间（范围查询-结束）
    /// </summary>
    public DateTime? VisitStartTimeEnd { get; set; }

    /// <summary>
    /// 参访结束时间（范围查询-开始）
    /// </summary>
    public DateTime? VisitEndTimeStart { get; set; }

    /// <summary>
    /// 参访结束时间（范围查询-结束）
    /// </summary>
    public DateTime? VisitEndTimeEnd { get; set; }

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
// 创建Visitor DTO
// ========================================

/// <summary>
/// 创建Visitor DTO
/// </summary>
public class TaktVisitorCreateDto
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
    /// 来访公司名称
    /// </summary>
    [Required(ErrorMessage = "来访公司名称不能为空")]
    public string VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间
    /// </summary>
    public DateTime VisitStartTime { get; set; }

    /// <summary>
    /// 参访结束时间
    /// </summary>
    public DateTime VisitEndTime { get; set; }

    /// <summary>
    /// 来访人员列表（子表，级联保存）
    /// </summary>
    public List<TaktVisitorCompanionCreateDto>? Companions { get; set; }

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
// 更新Visitor DTO
// ========================================

/// <summary>
/// 更新Visitor DTO
/// 继承 TaktVisitorCreateDto，添加 VisitorId 字段
/// </summary>
public class TaktVisitorUpdateDto : TaktVisitorCreateDto
{
    /// <summary>
    /// VisitorID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

    /// <summary>
    /// 来访人员列表（子表，级联保存）
    /// </summary>
    public new List<TaktVisitorCompanionUpdateDto>? Companions { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Visitor 导入模板行 DTO
/// </summary>
public class TaktVisitorTemplateDto
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
    /// 来访公司名称
    /// </summary>
    public string? VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间
    /// </summary>
    public DateTime? VisitStartTime { get; set; }

    /// <summary>
    /// 参访结束时间
    /// </summary>
    public DateTime? VisitEndTime { get; set; }

    /// <summary>
    /// 来访人员列表（子表，级联保存）
    /// </summary>
    public List<TaktVisitorCompanionCreateDto>? Companions { get; set; }

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
/// Visitor 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktVisitorImportDto
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
    /// 来访公司名称
    /// </summary>
    public string? VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间
    /// </summary>
    public DateTime? VisitStartTime { get; set; }

    /// <summary>
    /// 参访结束时间
    /// </summary>
    public DateTime? VisitEndTime { get; set; }

    /// <summary>
    /// 来访人员列表（子表，级联保存）
    /// </summary>
    public List<TaktVisitorCompanionCreateDto>? Companions { get; set; }

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
/// Visitor 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktVisitorExportDto
{
    /// <summary>
    /// VisitorID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long VisitorId { get; set; }

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
    /// 来访公司名称
    /// </summary>
    public string VisitorCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// 参访开始时间
    /// </summary>
    public DateTime VisitStartTime { get; set; }

    /// <summary>
    /// 参访结束时间
    /// </summary>
    public DateTime VisitEndTime { get; set; }

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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutesDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingMinutes 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMeetingMinutes 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.MeetingCenter;

// ========================================
// MeetingMinutes 响应 DTO
// ========================================

/// <summary>
/// 会后纪要实体，按会议维护议题分项与纪要正文
/// 对应前端 TaktMeetingMinutesDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMeetingMinutesDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MeetingMinutesID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingMinutesId { get; set; }

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

    /// <summary>
    /// 会议（主表）
    /// （主表：TaktMeeting）
    /// </summary>
    public TaktMeetingDto? Meeting { get; set; }

}

// ========================================
// MeetingMinutes 查询 DTO
// ========================================

/// <summary>
/// MeetingMinutes 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMeetingMinutesQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;


    /// <summary>
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    public string? MeetingTitle { get; set; } = string.Empty;

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
// 创建MeetingMinutes DTO
// ========================================

/// <summary>
/// 创建MeetingMinutes DTO
/// </summary>
public class TaktMeetingMinutesCreateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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
// 更新MeetingMinutes DTO
// ========================================

/// <summary>
/// 更新MeetingMinutes DTO
/// 继承 TaktMeetingMinutesCreateDto，添加 MeetingMinutesId 字段
/// </summary>
public class TaktMeetingMinutesUpdateDto : TaktMeetingMinutesCreateDto
{
    /// <summary>
    /// MeetingMinutesID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingMinutesId { get; set; }

}

// ========================================
// MeetingMinutes 作废 DTO
// ========================================

/// <summary>
/// MeetingMinutes 作废/撤销作废 DTO
/// </summary>
public class TaktMeetingMinutesObsoleteDto
{
    /// <summary>
    /// MeetingMinutesID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingMinutesId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MeetingMinutes 导入模板行 DTO
/// </summary>
public class TaktMeetingMinutesTemplateDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MeetingMinutes 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMeetingMinutesImportDto
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
    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；公司合并口径可用约定码）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int? IsObsolete { get; set; }

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
/// MeetingMinutes 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMeetingMinutesExportDto
{
    /// <summary>
    /// MeetingMinutesID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingMinutesId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    public string? MeetingMinutes { get; set; } = string.Empty;

    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    public string? MeetingSummary { get; set; } = string.Empty;

    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }

    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    public string? RecorderName { get; set; } = string.Empty;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; } = 0;

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

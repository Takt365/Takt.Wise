// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketReplyDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：TicketReply 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktTicketReply 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Routine.HelpDesk;

// ========================================
// TicketReply 响应 DTO
// ========================================

/// <summary>
/// 工单回复实体（用户与客服会话）
/// 对应前端 TaktTicketReplyDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktTicketReplyDto : TaktCompanyDtoBase
{
    /// <summary>
    /// TicketReplyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketReplyId { get; set; }

    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 工单 名称（填充字段）
    /// </summary>
    public string? TicketName { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int AuthorType { get; set; } = 0;

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    public string TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int IsInternal { get; set; } = 0;

    /// <summary>
    /// 工单（主表）
    /// （主表：TaktTicket）
    /// </summary>
    public TaktTicketDto? Ticket { get; set; }

}

// ========================================
// TicketReply 查询 DTO
// ========================================

/// <summary>
/// TicketReply 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktTicketReplyQueryDto : TaktPagedQuery
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int? AuthorType { get; set; }

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    public string? TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int? IsInternal { get; set; }

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

    /// <summary>
    /// 是否包含内部备注（默认 false 仅返回用户可见回复）
    /// </summary>
    public bool IncludeInternal { get; set; }
}

// ========================================
// 创建TicketReply DTO
// ========================================

/// <summary>
/// 创建TicketReply DTO
/// </summary>
public class TaktTicketReplyCreateDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int AuthorType { get; set; } = 0;

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    [Required(ErrorMessage = "回复内容不能为空")]
    public string TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int IsInternal { get; set; } = 0;

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
// 更新TicketReply DTO
// ========================================

/// <summary>
/// 更新TicketReply DTO
/// 继承 TaktTicketReplyCreateDto，添加 TicketReplyId 字段
/// </summary>
public class TaktTicketReplyUpdateDto : TaktTicketReplyCreateDto
{
    /// <summary>
    /// TicketReplyID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketReplyId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// TicketReply 导入模板行 DTO
/// </summary>
public class TaktTicketReplyTemplateDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int? AuthorType { get; set; }

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    public string? TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int? IsInternal { get; set; }

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
/// TicketReply 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktTicketReplyImportDto
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
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int? AuthorType { get; set; }

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    public string? TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int? IsInternal { get; set; }

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
/// TicketReply 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktTicketReplyExportDto
{
    /// <summary>
    /// TicketReplyID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketReplyId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 工单 ID（选项 TaktTickets/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long TicketId { get; set; }

    /// <summary>
    /// 作者类型（字典 routine_help_desk_ticket_reply_author_type；0=客服 1=用户 2=系统）
    /// </summary>
    public int AuthorType { get; set; } = 0;

    /// <summary>
    /// 作者 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long AuthorId { get; set; }

    /// <summary>
    /// 作者姓名
    /// </summary>
    public string? AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// 回复内容
    /// </summary>
    public string TicketReplyContent { get; set; } = string.Empty;

    /// <summary>
    /// 附件 （JSON列表形式，由TaktFile 统一上传到服务器）
    /// </summary>
    public string? Attachments { get; set; } = string.Empty;

    /// <summary>
    /// 是否内部备注（字典 sys_yes_no；1=是 0=否，仅客服可见）
    /// </summary>
    public int IsInternal { get; set; } = 0;

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

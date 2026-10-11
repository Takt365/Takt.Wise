// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Foundation
// 文件名称：TaktMessageDtos.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：Message 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMessage 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Foundation;

// ========================================
// Message 响应 DTO
// ========================================

/// <summary>
/// 在线消息实体 公司级实体：消息按租户+公司双重隔离
/// 对应前端 TaktMessageDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMessageDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MessageID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 发送者昵称（由用户表 NickName 解析，非消息表持久化字段）
    /// </summary>
    public string? FromUserNickName { get; set; }

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    public string MessageType { get; set; } = "system";

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    public string MessageGroup { get; set; } = "message";

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int IsCc { get; set; } = 0;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

}

// ========================================
// Message 查询 DTO
// ========================================

/// <summary>
/// Message 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMessageQueryDto : TaktPagedQuery
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
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string? MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string? MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    public string? MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    public string? MessageGroup { get; set; } = string.Empty;

    /// <summary>
    /// 读取时间（范围查询-开始）
    /// </summary>
    public DateTime? ReadTimeStart { get; set; }

    /// <summary>
    /// 读取时间（范围查询-结束）
    /// </summary>
    public DateTime? ReadTimeEnd { get; set; }

    /// <summary>
    /// 发送时间（范围查询-开始）
    /// </summary>
    public DateTime? SendTimeStart { get; set; }

    /// <summary>
    /// 发送时间（范围查询-结束）
    /// </summary>
    public DateTime? SendTimeEnd { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int? IsCc { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int? ReadStatus { get; set; }

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
// 创建Message DTO
// ========================================

/// <summary>
/// 创建Message DTO
/// </summary>
public class TaktMessageCreateDto
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
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（单条创建必填，由 FluentValidation 校验；批量发送由服务端按 ToUserIds 解析）
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    [Required(ErrorMessage = "消息标题不能为空")]
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    [Required(ErrorMessage = "消息内容不能为空")]
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    [Required(ErrorMessage = "消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）不能为空")]
    public string MessageType { get; set; } = "system";

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    [Required(ErrorMessage = "消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）不能为空")]
    public string MessageGroup { get; set; } = "message";

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int IsCc { get; set; } = 0;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

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
// 更新Message DTO
// ========================================

/// <summary>
/// 更新Message DTO
/// 继承 TaktMessageCreateDto，添加 MessageId 字段
/// </summary>
public class TaktMessageUpdateDto : TaktMessageCreateDto
{
    /// <summary>
    /// MessageID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

}

// ========================================
// Message 状态 DTO
// ========================================

/// <summary>
/// Message 状态更新 DTO
/// </summary>
public class TaktMessageStatusDto
{
    /// <summary>
    /// MessageID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    [Required(ErrorMessage = "读取状态（0=未读 1=已读）不能为空")]
    public int ReadStatus { get; set; } = 0;
}

// ========================================
// 收件箱 / 批量 / 已读未读 / 统计 DTO
// ========================================

/// <summary>
/// 当前用户收件箱列表查询 DTO（接收者与读取状态由服务端注入）
/// </summary>
public class TaktMessageInboxListQueryDto : TaktMessageQueryDto
{
}

/// <summary>
/// 在线消息广播推送 DTO（SignalR 公司内广播）
/// </summary>
public class TaktMessageBroadcastDto
{
    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 发送者用户名
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    public string MessageType { get; set; } = "system";

    /// <summary>
    /// 消息分组
    /// </summary>
    public string MessageGroup { get; set; } = "message";

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }
}

/// <summary>
/// 批量创建并发送在线消息 DTO
/// </summary>
public class TaktMessageBatchCreateDto : TaktMessageCreateDto
{
    /// <summary>
    /// 是否发送给当前公司全部可访问用户
    /// </summary>
    public bool SendToAll { get; set; }

    /// <summary>
    /// 指定接收者用户 ID 列表（前端 string 雪花 ID）
    /// </summary>
    public List<string>? ToUserIds { get; set; }
}

/// <summary>
/// 标记在线消息为已读 DTO
/// </summary>
public class TaktMessageReadDto
{
    /// <summary>
    /// 在线消息 ID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 1;

    /// <summary>
    /// 读取时间（为空时服务端取当前时间）
    /// </summary>
    public DateTime? ReadTime { get; set; }
}

/// <summary>
/// 标记在线消息为未读 DTO
/// </summary>
public class TaktMessageUnreadDto
{
    /// <summary>
    /// 在线消息 ID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

    /// <summary>
    /// 读取时间（未读时一般为 null）
    /// </summary>
    public DateTime? ReadTime { get; set; }
}

/// <summary>
/// 当前用户在线消息统计 DTO
/// </summary>
public class TaktMessageStatisticsDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户名（接收者）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 收件箱消息总数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 已读数量
    /// </summary>
    public int ReadCount { get; set; }

    /// <summary>
    /// 未读数量
    /// </summary>
    public int UnreadCount { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// Message 导入模板行 DTO
/// </summary>
public class TaktMessageTemplateDto
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
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string? MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string? MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    public string? MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    public string? MessageGroup { get; set; } = string.Empty;

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int? IsCc { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int? ReadStatus { get; set; }

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
/// Message 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMessageImportDto
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
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string? ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string? MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string? MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    public string? MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    public string? MessageGroup { get; set; } = string.Empty;

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int? IsCc { get; set; }

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int? ReadStatus { get; set; }

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
/// Message 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMessageExportDto
{
    /// <summary>
    /// MessageID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MessageId { get; set; }

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
    /// 发送者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long FromUserId { get; set; }

    /// <summary>
    /// 发送者用户名（冗余：按 FromUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string FromUserName { get; set; } = string.Empty;

    /// <summary>
    /// 接收者用户（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ToUserId { get; set; }

    /// <summary>
    /// 接收者用户名（冗余：按 ToUserId 取 TaktUser.UserName 联动）
    /// </summary>
    public string ToUserName { get; set; } = string.Empty;

    /// <summary>
    /// 消息标题
    /// </summary>
    public string MessageTitle { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string MessageContent { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型（字典 sys_message_type 的 DictValue，如 text、system、multimedia）
    /// </summary>
    public string MessageType { get; set; } = string.Empty;

    /// <summary>
    /// 消息分组（字典 sys_message_group 的 DictValue，如 collaboration、message、reminder）
    /// </summary>
    public string MessageGroup { get; set; } = string.Empty;

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime SendTime { get; set; }

    /// <summary>
    /// 抄送（0=否，1=是）
    /// </summary>
    public int IsCc { get; set; } = 0;

    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    public string? FileName { get; set; } = string.Empty;

    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    public string? AccessUrl { get; set; } = string.Empty;

    /// <summary>
    /// 消息扩展数据（JSON）
    /// </summary>
    public string? MessageExtData { get; set; } = string.Empty;

    /// <summary>
    /// 读取状态（0=未读 1=已读）
    /// </summary>
    public int ReadStatus { get; set; } = 0;

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

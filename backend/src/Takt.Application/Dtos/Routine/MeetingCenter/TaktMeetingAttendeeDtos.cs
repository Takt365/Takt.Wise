// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.MeetingCenter
// 文件名称：TaktMeetingAttendeeDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：MeetingAttendee 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktMeetingAttendee 生成，请按需审阅）
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
// MeetingAttendee 响应 DTO
// ========================================

/// <summary>
/// 参会人员子实体
/// 对应前端 TaktMeetingAttendeeDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktMeetingAttendeeDto : TaktCompanyDtoBase
{
    /// <summary>
    /// MeetingAttendeeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

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
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int AttendeeRole { get; set; } = 0;

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int CheckInMethod { get; set; } = 0;

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int AttendanceStatus { get; set; } = 0;

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
// MeetingAttendee 查询 DTO
// ========================================

/// <summary>
/// MeetingAttendee 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktMeetingAttendeeQueryDto : TaktPagedQuery
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
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int? AttendeeRole { get; set; }

    /// <summary>
    /// 签到时间（范围查询-开始）
    /// </summary>
    public DateTime? CheckInTimeStart { get; set; }

    /// <summary>
    /// 签到时间（范围查询-结束）
    /// </summary>
    public DateTime? CheckInTimeEnd { get; set; }

    /// <summary>
    /// 签退时间（范围查询-开始）
    /// </summary>
    public DateTime? CheckOutTimeStart { get; set; }

    /// <summary>
    /// 签退时间（范围查询-结束）
    /// </summary>
    public DateTime? CheckOutTimeEnd { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int? CheckInMethod { get; set; }

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int? AttendanceStatus { get; set; }

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
// 创建MeetingAttendee DTO
// ========================================

/// <summary>
/// 创建MeetingAttendee DTO
/// </summary>
public class TaktMeetingAttendeeCreateDto
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
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int AttendeeRole { get; set; } = 0;

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int CheckInMethod { get; set; } = 0;

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int AttendanceStatus { get; set; } = 0;

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
// 更新MeetingAttendee DTO
// ========================================

/// <summary>
/// 更新MeetingAttendee DTO
/// 继承 TaktMeetingAttendeeCreateDto，添加 MeetingAttendeeId 字段
/// </summary>
public class TaktMeetingAttendeeUpdateDto : TaktMeetingAttendeeCreateDto
{
    /// <summary>
    /// MeetingAttendeeID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

}

// ========================================
// MeetingAttendee 状态 DTO
// ========================================

/// <summary>
/// MeetingAttendee 状态更新 DTO
/// </summary>
public class TaktMeetingAttendeeStatusDto
{
    /// <summary>
    /// MeetingAttendeeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    [Required(ErrorMessage = "出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）不能为空")]
    public int AttendanceStatus { get; set; } = 0;
}

// ========================================
// MeetingAttendee 作废 DTO
// ========================================

/// <summary>
/// MeetingAttendee 作废/撤销作废 DTO
/// </summary>
public class TaktMeetingAttendeeObsoleteDto
{
    /// <summary>
    /// MeetingAttendeeID
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

    /// <summary>
    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    public int IsObsolete { get; set; }
}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// MeetingAttendee 导入模板行 DTO
/// </summary>
public class TaktMeetingAttendeeTemplateDto
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
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int? AttendeeRole { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int? CheckInMethod { get; set; }

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int? AttendanceStatus { get; set; }

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
/// MeetingAttendee 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktMeetingAttendeeImportDto
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
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? MeetingId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string? UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int? AttendeeRole { get; set; }

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int? CheckInMethod { get; set; }

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int? AttendanceStatus { get; set; }

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
/// MeetingAttendee 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktMeetingAttendeeExportDto
{
    /// <summary>
    /// MeetingAttendeeID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingAttendeeId { get; set; }

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
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }

    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    public int LineNumber { get; set; } = 0;

    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名（冗余字段，便于查询）
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    public int AttendeeRole { get; set; } = 0;

    /// <summary>
    /// 签到时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 签退时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    public int CheckInMethod { get; set; } = 0;

    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    public int AttendanceStatus { get; set; } = 0;

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

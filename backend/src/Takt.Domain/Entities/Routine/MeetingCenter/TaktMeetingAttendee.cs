// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.MeetingCenter
// 文件名称：TaktMeetingAttendee.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：参会人员子实体
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.MeetingCenter;

/// <summary>
/// 参会人员子实体
/// </summary>
[SugarTable("takt_routine_meeting_center_attendee", "参会人员表")]
[SugarIndex("ix_meeting_attendee_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_attendee_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_attendee_unique", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MeetingId), OrderByType.Asc, nameof(UserId), OrderByType.Asc, true)]
[SugarIndex("ix_meeting_attendee_meeting_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MeetingId), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_attendee_user_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(UserId), OrderByType.Asc, false)]
public class TaktMeetingAttendee : TaktCompanyEntityBase
{
    /// <summary>
    /// 会议 ID（选项 TaktMeetings/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_id", ColumnDescription = "会议ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long MeetingId { get; set; }
    /// <summary>
    /// 会议标题（冗余字段，便于查询；与主表 TaktMeeting.MeetingTitle 一致）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_title", ColumnDescription = "会议标题", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string MeetingTitle { get; set; } = string.Empty;
    /// <summary>
    /// 行号（固定步长=10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;
    /// <summary>
    /// 用户 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", ColumnDataType = "bigint", IsNullable = false)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long UserId { get; set; }
    /// <summary>
    /// 用户姓名（冗余：按对应 Id 取主数据名称联动）
    /// </summary>
    [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户姓名", ColumnDataType = "varchar", Length = 20, IsNullable = false)]
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 参与角色（字典 routine_meeting_center_attendee_role；0=参会人 1=主持人 2=记录人 3=嘉宾）
    /// </summary>
    [SugarColumn(ColumnName = "attendee_role", ColumnDescription = "参与角色", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AttendeeRole { get; set; } = 0;
    /// <summary>
    /// 签到时间
    /// </summary>
    [SugarColumn(ColumnName = "check_in_time", ColumnDescription = "签到时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CheckInTime { get; set; }
    /// <summary>
    /// 签退时间
    /// </summary>
    [SugarColumn(ColumnName = "check_out_time", ColumnDescription = "签退时间", ColumnDataType = "datetime", IsNullable = true)]
    public DateTime? CheckOutTime { get; set; }
    /// <summary>
    /// 签到方式（字典 routine_meeting_center_check_in_method；0=手动 1=扫码 2=人脸 3=门禁）
    /// </summary>
    [SugarColumn(ColumnName = "check_in_method", ColumnDescription = "签到方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int CheckInMethod { get; set; } = 0;
    /// <summary>
    /// 出席状态（字典 routine_meeting_center_attendance_status；0=待确认 1=已出席 2=缺席 3=迟到 4=请假）
    /// </summary>
    [SugarColumn(ColumnName = "attendance_status", ColumnDescription = "出席状态", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int AttendanceStatus { get; set; } = 0;
    /// <summary>
    /// 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
    /// </summary>
    [SugarColumn(ColumnName = "is_obsolete", ColumnDescription = "是否作废", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsObsolete { get; set; } = 0;

    // ========================================
    // 导航属性区域
    // ========================================
    /// <summary>
    /// 会议（主表）
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(MeetingId))]
    public TaktMeeting? Meeting { get; set; }
}

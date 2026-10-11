// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Entities.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutes.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：会后纪要实体，维护会议纪正文与摘要；会议标题冗余自主表
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Routine.MeetingCenter;

/// <summary>
/// 会后纪要实体，按会议维护纪要正文；会议标题冗余自主表 MeetingTitle
/// </summary>
[SugarTable("takt_routine_meeting_center_minutes", "会后纪要表")]
[SugarIndex("ix_meeting_minutes_tenant", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_minutes_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_meeting_minutes_meeting_id", nameof(TenantCode), OrderByType.Asc, nameof(CompanyCode), OrderByType.Asc, nameof(MeetingId), OrderByType.Asc, false)]
public class TaktMeetingMinutes : TaktCompanyEntityBase
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
    /// 行号（纪要分项序号，固定步长=10；纪要通常为 10）
    /// </summary>
    [SugarColumn(ColumnName = "line_number", ColumnDescription = "行号", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int LineNumber { get; set; } = 10;
    /// <summary>
    /// 会议纪要（会后纪要富文本 HTML）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_minutes", ColumnDescription = "会议纪要", ColumnDataType = "ntext", IsNullable = true)]
    public string? MeetingMinutes { get; set; }
    /// <summary>
    /// 摘要（纪要列表展示用）
    /// </summary>
    [SugarColumn(ColumnName = "meeting_summary", ColumnDescription = "摘要", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? MeetingSummary { get; set; }
    /// <summary>
    /// 记录 ID（选项 TaktUsers/options；DictValue=Id）
    /// </summary>
    [SugarColumn(ColumnName = "recorder_id", ColumnDescription = "记录ID", ColumnDataType = "bigint", IsNullable = true)]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? RecorderId { get; set; }
    /// <summary>
    /// 记录员（冗余字段，便于查询；与 TaktUser.UserName 一致）
    /// </summary>
    [SugarColumn(ColumnName = "recorder_name", ColumnDescription = "记录员", ColumnDataType = "nvarchar", Length = 40, IsNullable = true)]
    public string? RecorderName { get; set; }
    /// <summary>
    /// 文件名称（原始文件名，长度对齐 TaktFile.FileName）
    /// </summary>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "文件名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? FileName { get; set; }
    /// <summary>
    /// 访问地址（文件访问 URL，长度对齐 TaktFile.AccessUrl）
    /// </summary>
    [SugarColumn(ColumnName = "access_url", ColumnDescription = "访问地址", ColumnDataType = "nvarchar", Length = 1000, IsNullable = true)]
    public string? AccessUrl { get; set; }

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

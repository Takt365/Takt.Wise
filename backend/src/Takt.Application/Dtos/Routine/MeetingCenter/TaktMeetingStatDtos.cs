// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.MeetingCenter
// 文件名称：TaktMeetingStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：会议件数统计 DTO（数据看板 meeting-stat；按开始时间与状态）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Routine.MeetingCenter;

/// <summary>
/// 会议件数统计查询 DTO（按开始时间区间；与看板本月对齐）
/// </summary>
public class TaktMeetingStatQueryDto
{
    /// <summary>
    /// 统计月份（yyyy-MM；无日期区间时使用）
    /// </summary>
    public string? StatMonth { get; set; }

    /// <summary>
    /// 开始时间-起（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? StartTimeStart { get; set; }

    /// <summary>
    /// 开始时间-止（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }
}

/// <summary>
/// 会议件数统计 DTO（字典 routine_meeting_center_status：未开始=已排期 1；已完成=已结束 3）
/// </summary>
public class TaktMeetingStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 未开始件数（MeetingStatus=1 已排期）
    /// </summary>
    public int MonthNotStartedCount { get; set; }

    /// <summary>
    /// 已完成件数（MeetingStatus=3 已结束）
    /// </summary>
    public int MonthCompletedCount { get; set; }
}

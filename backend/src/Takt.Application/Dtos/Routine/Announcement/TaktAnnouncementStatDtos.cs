// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.Announcement
// 文件名称：TaktAnnouncementStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知件数统计 DTO（数据看板 announcement-stat；按发布时间）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Routine.Announcement;

/// <summary>
/// 公告通知件数统计查询 DTO（按发布时间区间；与看板本月对齐）
/// </summary>
public class TaktAnnouncementStatQueryDto
{
    /// <summary>
    /// 统计月份（yyyy-MM；无日期区间时使用）
    /// </summary>
    public string? StatMonth { get; set; }

    /// <summary>
    /// 发布时间-起（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? PublishTimeStart { get; set; }

    /// <summary>
    /// 发布时间-止（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? PublishTimeEnd { get; set; }
}

/// <summary>
/// 公告通知件数统计 DTO
/// </summary>
public class TaktAnnouncementStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 通知件数（PublishTime 落在统计月内）
    /// </summary>
    public int MonthAnnouncementCount { get; set; }
}

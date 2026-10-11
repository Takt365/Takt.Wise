// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Complaint
// 文件名称：TaktCustomerComplaintStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉件数统计 DTO（数据看板 complaint-stat；按投诉日期与状态）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Complaint;

/// <summary>
/// 客诉件数统计查询 DTO（按投诉日期区间；与看板上月对齐）
/// </summary>
public class TaktCustomerComplaintStatQueryDto
{
    /// <summary>
    /// 统计月份（yyyy-MM；无日期区间时使用，默认上月）
    /// </summary>
    public string? StatMonth { get; set; }

    /// <summary>
    /// 投诉日期-开始（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? ComplaintDateStart { get; set; }

    /// <summary>
    /// 投诉日期-结束（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? ComplaintDateEnd { get; set; }
}

/// <summary>
/// 客诉件数统计 DTO（字典 logistics_quality_complaint_status：0 待处理 / 1 处理中 / ≥2 处理完成）
/// </summary>
public class TaktCustomerComplaintStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 客诉件数（上月投诉日期内全部）
    /// </summary>
    public int MonthComplaintCount { get; set; }

    /// <summary>
    /// 未处理件数（ComplaintStatus=0 待处理）
    /// </summary>
    public int MonthPendingCount { get; set; }

    /// <summary>
    /// 处理中件数（ComplaintStatus=1）
    /// </summary>
    public int MonthInProgressCount { get; set; }

    /// <summary>
    /// 处理完成件数（ComplaintStatus≥2：已回复/已关闭/已驳回）
    /// </summary>
    public int MonthCompletedCount { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktTicketStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台工单件数统计 DTO（数据看板 ticket-stat；按创建日与状态）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Routine.HelpDesk;

/// <summary>
/// 服务台工单件数统计查询 DTO（按创建日区间；与看板上月对齐）
/// </summary>
public class TaktTicketStatQueryDto
{
    /// <summary>
    /// 统计月份（yyyy-MM；无日期区间时使用，默认上月）
    /// </summary>
    public string? StatMonth { get; set; }

    /// <summary>
    /// 创建日-开始（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建日-结束（可选，优先于 StatMonth）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }
}

/// <summary>
/// 服务台工单件数统计 DTO（字典 sys_ticket_status：未处理 0～1 / 处理中 2、3、7 / 已处理 4～6）
/// </summary>
public class TaktTicketStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 服务工单件数（上月创建日内全部）
    /// </summary>
    public int MonthTicketCount { get; set; }

    /// <summary>
    /// 未处理件数（新建/已分配）
    /// </summary>
    public int MonthPendingCount { get; set; }

    /// <summary>
    /// 处理中件数（处理中/待确认/重新打开）
    /// </summary>
    public int MonthInProgressCount { get; set; }

    /// <summary>
    /// 已处理件数（已完成/已关闭/已取消）
    /// </summary>
    public int MonthProcessedCount { get; set; }
}

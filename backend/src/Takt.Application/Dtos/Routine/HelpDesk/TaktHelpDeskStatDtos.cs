// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Routine.HelpDesk
// 文件名称：TaktHelpDeskStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台工单统计 DTO（数据看板 ticket-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Routine.HelpDesk;

/// <summary>
/// 服务台工单统计查询 DTO（按创建时间区间）
/// </summary>
public class TaktHelpDeskTicketStatQueryDto
{
    /// <summary>
    /// 创建时间（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }
}

/// <summary>
/// 服务台工单统计 DTO
/// </summary>
public class TaktHelpDeskTicketStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月工单数量
    /// </summary>
    public int MonthTicketCount { get; set; }

    /// <summary>
    /// 月进行中工单数量（状态 0～3）
    /// </summary>
    public int MonthOpenTicketCount { get; set; }

    /// <summary>
    /// 月已完成/已关闭工单数量（状态 4～5）
    /// </summary>
    public int MonthClosedTicketCount { get; set; }
}

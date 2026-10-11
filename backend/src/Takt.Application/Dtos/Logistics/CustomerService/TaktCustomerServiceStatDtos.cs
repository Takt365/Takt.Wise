// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.CustomerService
// 文件名称：TaktCustomerServiceStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：客户服务统计 DTO（数据看板 *-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.CustomerService;

/// <summary>
/// 服务请求统计查询 DTO（按请求日期区间）
/// </summary>
public class TaktServiceRequestStatQueryDto
{
    /// <summary>
    /// 请求日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? RequestDateStart { get; set; }

    /// <summary>
    /// 请求日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? RequestDateEnd { get; set; }
}

/// <summary>
/// 服务请求统计 DTO
/// </summary>
public class TaktServiceRequestStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月请求数量
    /// </summary>
    public int MonthRequestCount { get; set; }
}

/// <summary>
/// 服务订单统计查询 DTO（按订单日期区间）
/// </summary>
public class TaktServiceOrderStatQueryDto
{
    /// <summary>
    /// 订单日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }
}

/// <summary>
/// 服务订单统计 DTO
/// </summary>
public class TaktServiceOrderStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月订单数量
    /// </summary>
    public int MonthOrderCount { get; set; }

    /// <summary>
    /// 月订单金额合计（分；前端展示时 ÷100 为元）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }
}

/// <summary>
/// 服务工单统计查询 DTO（按创建时间区间）
/// </summary>
public class TaktServiceTicketStatQueryDto
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
/// 服务工单统计 DTO
/// </summary>
public class TaktServiceTicketStatDto
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

/// <summary>
/// 服务合同统计查询 DTO（按生效日期区间）
/// </summary>
public class TaktServiceContractStatQueryDto
{
    /// <summary>
    /// 生效日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? EffectiveDateStart { get; set; }

    /// <summary>
    /// 生效日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? EffectiveDateEnd { get; set; }
}

/// <summary>
/// 服务合同统计 DTO
/// </summary>
public class TaktServiceContractStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月合同数量
    /// </summary>
    public int MonthContractCount { get; set; }

    /// <summary>
    /// 月合同金额合计（分；前端展示时 ÷100 为元）
    /// </summary>
    public decimal MonthContractAmount { get; set; }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Sales
// 文件名称：TaktSalesStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：销售统计 DTO（数据看板 order-stat、invoice-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Sales;

/// <summary>
/// 销售统计查询 DTO（按订单日期区间）
/// </summary>
public class TaktSalesStatQueryDto
{
    /// <summary>
    /// 订单日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 同比基期订单日期（范围-开始；可选）
    /// </summary>
    public DateTime? CompareOrderDateStart { get; set; }

    /// <summary>
    /// 同比基期订单日期（范围-结束；可选）
    /// </summary>
    public DateTime? CompareOrderDateEnd { get; set; }
}

/// <summary>
/// 销售订单统计 DTO
/// </summary>
public class TaktSalesOrderStatDto
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
    /// 月订单金额合计（元；优先明细 SalesAmount 未作废行，回退主表 TotalAmount）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }

    /// <summary>
    /// 同比基期订单数量（未传 compare 区间时为 0）
    /// </summary>
    public int CompareOrderCount { get; set; }

    /// <summary>
    /// 订单数同比增长率（%）
    /// </summary>
    public decimal OrderCountYoYPercent { get; set; }
}

/// <summary>
/// 销售发票统计查询 DTO（按年度期间 yyyyMM 或明细过帐日期区间）
/// </summary>
public class TaktSalesInvoiceStatQueryDto
{
    /// <summary>
    /// 年度期间 yyyyMM（未传时由过帐日期区间或默认当月推导）
    /// </summary>
    public string? YearMonth { get; set; }

    /// <summary>
    /// 明细过帐日期（范围-开始；YearMonth 未传时与 PostingDateEnd 解析默认当月）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 明细过帐日期（范围-结束）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 同比基期年度期间 yyyyMM（可选；未传时为上一年同月）
    /// </summary>
    public string? CompareYearMonth { get; set; }
}

/// <summary>
/// 销售发票统计 DTO（本月销售：发票数 + 明细本位币金额合计）
/// </summary>
public class TaktSalesInvoiceStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 年度期间（yyyyMM）
    /// </summary>
    public string YearMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月发票数量
    /// </summary>
    public int MonthInvoiceCount { get; set; }

    /// <summary>
    /// 月销售额合计（本位币元；明细 LocalCurrencyAmount 合计）
    /// </summary>
    public decimal MonthSalesAmount { get; set; }

    /// <summary>
    /// 同比基期发票数量
    /// </summary>
    public int CompareInvoiceCount { get; set; }

    /// <summary>
    /// 同比基期销售额合计（本位币元）
    /// </summary>
    public decimal CompareSalesAmount { get; set; }

    /// <summary>
    /// 发票数同比增长率（%）
    /// </summary>
    public decimal InvoiceCountYoYPercent { get; set; }

    /// <summary>
    /// 销售额同比增长率（%）
    /// </summary>
    public decimal SalesAmountYoYPercent { get; set; }
}

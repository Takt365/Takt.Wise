// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Procurement
// 文件名称：TaktProcurementStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：采购统计 DTO（数据看板 order-stat / invoice-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Procurement;

/// <summary>
/// 采购统计查询 DTO（订单按 OrderDate；发票按 PostingDate）
/// </summary>
public class TaktProcurementStatQueryDto
{
    /// <summary>
    /// 订单日期（范围-开始；默认当月 1 日；order-stat）
    /// </summary>
    public DateTime? OrderDateStart { get; set; }

    /// <summary>
    /// 订单日期（范围-结束；默认当月最后一日；order-stat）
    /// </summary>
    public DateTime? OrderDateEnd { get; set; }

    /// <summary>
    /// 过帐日期（范围-开始；默认当月 1 日；invoice-stat）
    /// </summary>
    public DateTime? PostingDateStart { get; set; }

    /// <summary>
    /// 过帐日期（范围-结束；默认当月最后一日；invoice-stat）
    /// </summary>
    public DateTime? PostingDateEnd { get; set; }

    /// <summary>
    /// 申请日期（范围-开始；默认当月 1 日；request-stat）
    /// </summary>
    public DateTime? RequestDateStart { get; set; }

    /// <summary>
    /// 申请日期（范围-结束；默认当月最后一日；request-stat）
    /// </summary>
    public DateTime? RequestDateEnd { get; set; }

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
/// 采购订单统计 DTO
/// </summary>
public class TaktPurchaseOrderStatDto
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
    /// 月采购金额合计（元；优先明细 PurchaseAmount 汇总，回退主表 TotalAmount）
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
/// 采购发票统计 DTO（数据看板采购发票金额）
/// </summary>
public class TaktPurchaseInvoiceStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月发票数量（按过帐日期）
    /// </summary>
    public int MonthInvoiceCount { get; set; }

    /// <summary>
    /// 月采购金额合计（元；优先明细 Amount 未作废行，回退主表 GrossAmount）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }
}

/// <summary>
/// 采购申请统计 DTO（数据看板采购申请金额）
/// </summary>
public class TaktPurchaseRequestStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月申请数量（按申请日期）
    /// </summary>
    public int MonthRequestCount { get; set; }

    /// <summary>
    /// 月申请金额合计（元；优先明细 RequestAmount 未作废行，回退主表 TotalAmount）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }
}

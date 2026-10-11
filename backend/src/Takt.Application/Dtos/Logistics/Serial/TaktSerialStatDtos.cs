// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Serial
// 文件名称：TaktSerialStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：序列号统计 DTO（数据看板 inbound-stat / outbound-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Serial;

/// <summary>
/// 序列号入库统计查询 DTO（按入库日期区间）
/// </summary>
public class TaktSerialInboundStatQueryDto
{
    /// <summary>
    /// 入库日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? InboundDateStart { get; set; }

    /// <summary>
    /// 入库日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? InboundDateEnd { get; set; }
}

/// <summary>
/// 序列号入库统计 DTO
/// </summary>
public class TaktSerialInboundStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月入库单数量
    /// </summary>
    public int MonthInboundCount { get; set; }

    /// <summary>
    /// 月入库总数量合计
    /// </summary>
    public int MonthTotalQuantity { get; set; }
}

/// <summary>
/// 序列号出库统计查询 DTO（按装车日期区间；可选仕向地、目的地港筛选）
/// </summary>
public class TaktSerialOutboundStatQueryDto
{
    /// <summary>
    /// 装车日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? OutboundDateStart { get; set; }

    /// <summary>
    /// 装车日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? OutboundDateEnd { get; set; }

    /// <summary>
    /// 仕向地（选项 TaktModelDestinations/options，DictValue=DestinationCode；可选精确筛选）
    /// </summary>
    public string? Destination { get; set; }

    /// <summary>
    /// 目的地港（字典 logistics_serial_destination_port_code；可选精确筛选）
    /// </summary>
    public string? DestinationPort { get; set; }
}

/// <summary>
/// 序列号出库统计分组行 DTO（按装车日期 + 仕向地 + 目的地港）
/// </summary>
public class TaktSerialOutboundStatItemDto
{
    /// <summary>
    /// 装车日期（日粒度）
    /// </summary>
    public DateTime OutboundDate { get; set; }

    /// <summary>
    /// 仕向地
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// 目的地港
    /// </summary>
    public string DestinationPort { get; set; } = string.Empty;

    /// <summary>
    /// 出库单数量
    /// </summary>
    public int OutboundCount { get; set; }

    /// <summary>
    /// 出库总数量合计
    /// </summary>
    public int TotalQuantity { get; set; }
}

/// <summary>
/// 序列号出库统计 DTO
/// </summary>
public class TaktSerialOutboundStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月出库单数量
    /// </summary>
    public int MonthOutboundCount { get; set; }

    /// <summary>
    /// 月出库总数量合计
    /// </summary>
    public int MonthTotalQuantity { get; set; }

    /// <summary>
    /// 按装车日期、仕向地、目的地港分组明细
    /// </summary>
    public List<TaktSerialOutboundStatItemDto> GroupItems { get; set; } = new();
}

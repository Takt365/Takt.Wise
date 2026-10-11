// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Maintenance
// 文件名称：TaktMaintenanceStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂维护统计 DTO（数据看板 *-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Maintenance;

/// <summary>
/// 维护工单统计查询 DTO（按创建时间区间）
/// </summary>
public class TaktMaintenanceWorkOrderStatQueryDto
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
/// 维护工单统计 DTO
/// </summary>
public class TaktMaintenanceWorkOrderStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月工单数量
    /// </summary>
    public int MonthWorkOrderCount { get; set; }

    /// <summary>
    /// 月进行中工单数量（状态 0～3）
    /// </summary>
    public int MonthOpenWorkOrderCount { get; set; }

    /// <summary>
    /// 月总成本合计
    /// </summary>
    public decimal MonthTotalCost { get; set; }
}

/// <summary>
/// 维护通知单统计查询 DTO（按发现时间区间）
/// </summary>
public class TaktMaintenanceNotificationStatQueryDto
{
    /// <summary>
    /// 发现时间（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? DiscoveredAtStart { get; set; }

    /// <summary>
    /// 发现时间（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? DiscoveredAtEnd { get; set; }
}

/// <summary>
/// 维护通知单统计 DTO
/// </summary>
public class TaktMaintenanceNotificationStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月通知单数量
    /// </summary>
    public int MonthNotificationCount { get; set; }
}

/// <summary>
/// 维护履历统计查询 DTO（按维护日期区间）
/// </summary>
public class TaktMaintenanceHistoryStatQueryDto
{
    /// <summary>
    /// 维护日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? MaintenanceDateStart { get; set; }

    /// <summary>
    /// 维护日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? MaintenanceDateEnd { get; set; }
}

/// <summary>
/// 维护履历统计 DTO
/// </summary>
public class TaktMaintenanceHistoryStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月维护履历数量
    /// </summary>
    public int MonthHistoryCount { get; set; }
}

/// <summary>
/// 设备统计查询 DTO（按创建时间区间）
/// </summary>
public class TaktEquipmentStatQueryDto
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
/// 设备统计 DTO
/// </summary>
public class TaktEquipmentStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月新增设备数量
    /// </summary>
    public int MonthEquipmentCount { get; set; }
}

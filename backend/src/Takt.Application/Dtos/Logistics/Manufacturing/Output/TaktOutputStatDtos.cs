// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktOutputStatDtos.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：产出（Output）生产统计 DTO（数据看板 production-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计查询 DTO（按生产日期区间）
/// </summary>
public class TaktOutputProductionStatQueryDto
{
    /// <summary>
    /// 生产日期（范围-开始；默认当月 1 日）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围-结束；默认当月最后一日）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }
}

/// <summary>
/// 生产统计按生产班组行（数据看板 OPH：班别/计划数/生产数/达成率）
/// </summary>
public class TaktOutputProductionStatTeamItemDto
{
    /// <summary>
    /// 生产班组编码（TeamCode）
    /// </summary>
    public string TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 计划数（标准产能合计）
    /// </summary>
    public decimal StdCapacity { get; set; }

    /// <summary>
    /// 生产数（实际产量合计）
    /// </summary>
    public decimal ProdActualQty { get; set; }

    /// <summary>
    /// 达成率（%）
    /// </summary>
    public decimal AchievementRate { get; set; }
}

/// <summary>
/// 生产统计 DTO（组立/PCBA 共用结构）
/// </summary>
public class TaktOutputProductionStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 月标准产能合计（主表 StdCapacity 汇总）
    /// </summary>
    public decimal MonthStdCapacity { get; set; }

    /// <summary>
    /// 月实际产量合计（组立：明细 ProdActualQty；PCBA：明细 DailyCompletedQty）
    /// </summary>
    public decimal MonthProdActualQty { get; set; }

    /// <summary>
    /// 月达成率（%）
    /// </summary>
    public decimal MonthAchievementRate { get; set; }

    /// <summary>
    /// 月停线损失（分钟；组立：DowntimeMinutes；PCBA：StopTime + SwitchTime）
    /// </summary>
    public decimal MonthDowntimeMinutes { get; set; }

    /// <summary>
    /// 月投入工时（分钟）
    /// </summary>
    public decimal MonthInputMinutes { get; set; }

    /// <summary>
    /// 月生产工时（分钟；组立：ConfirmMinutes；PCBA：TotalMinutes）
    /// </summary>
    public decimal MonthProdMinutes { get; set; }

    /// <summary>
    /// 月实际工时（分钟；组立：ActualMinutes；PCBA：InputMinutes + RepairMinutes）
    /// </summary>
    public decimal MonthActualMinutes { get; set; }

    /// <summary>
    /// 按生产班组（TeamCode）分行汇总
    /// </summary>
    public List<TaktOutputProductionStatTeamItemDto> Teams { get; set; } = new();
}

/// <summary>
/// 组立生产统计 DTO
/// </summary>
public class TaktAssyOutputProductionStatDto : TaktOutputProductionStatDto
{
}

/// <summary>
/// PCBA 生产统计 DTO
/// </summary>
public class TaktPcbaOutputProductionStatDto : TaktOutputProductionStatDto
{
}

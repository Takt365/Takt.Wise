// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Quality.Cost
// 文件名称：TaktQualityCostStatDtos.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本金额统计 DTO（数据看板 cost-stat：业务/事故/应对）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Dtos.Logistics.Quality.Cost;

/// <summary>
/// 品质成本金额统计查询 DTO（按业务月或日期区间；与看板上月对齐）
/// </summary>
public class TaktQualityCostStatQueryDto
{
    /// <summary>
    /// 统计月份（yyyy-MM；品质业务按 AssuranceMonth；事故/应对可据此推导日期区间）
    /// </summary>
    public string? StatMonth { get; set; }

    /// <summary>
    /// 日期范围-开始（事故/应对；可选，优先于 StatMonth）
    /// </summary>
    public DateTime? DateStart { get; set; }

    /// <summary>
    /// 日期范围-结束（事故/应对；可选，优先于 StatMonth）
    /// </summary>
    public DateTime? DateEnd { get; set; }
}

/// <summary>
/// 品质成本金额统计 DTO（业务/事故/应对共用）
/// </summary>
public class TaktQualityCostStatDto
{
    /// <summary>
    /// 统计月份（yyyy-MM）
    /// </summary>
    public string StatMonth { get; set; } = string.Empty;

    /// <summary>
    /// 金额合计（元）
    /// </summary>
    public decimal MonthTotalAmount { get; set; }

    /// <summary>
    /// 单据行数
    /// </summary>
    public int MonthRowCount { get; set; }
}

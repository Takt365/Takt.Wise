// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：ITaktPurchasePriceTrendMonthlyAnalysisBuilder.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移转置分析构建器接口（列=各行第 n 笔 ValidFrom）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格推移转置分析构建器（列=各行第 n 笔 ValidFrom，不对齐全表日期）
/// </summary>
public interface ITaktPurchasePriceTrendMonthlyAnalysisBuilder
{
    /// <summary>
    /// 构建采购价格推移转置分析全量结果（列=第 n 笔）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    Task<TaktPurchasePriceTrendAnalysisBuilt> BuildAsync(TaktPurchasePriceTrendQueryDto queryDto);
}

/// <summary>
/// 采购价格月推移内存构建结果
/// </summary>
public sealed class TaktPurchasePriceTrendAnalysisBuilt
{
    /// <summary>
    /// 过滤并排序后的全量行
    /// </summary>
    public List<TaktPurchasePriceTrendDto> OrderedRows { get; init; } = new();

    /// <summary>
    /// 期间列顺序（第 n 笔序号）
    /// </summary>
    public List<string> PeriodOrder { get; init; } = new();

    /// <summary>
    /// 基准期间
    /// </summary>
    public string? BasePeriod { get; init; }

    /// <summary>
    /// 对比期间
    /// </summary>
    public string? ComparePeriod { get; init; }

    /// <summary>
    /// 上涨行数
    /// </summary>
    public int UpCount { get; init; }

    /// <summary>
    /// 下跌行数
    /// </summary>
    public int DownCount { get; init; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; init; }

    /// <summary>
    /// 无趋势行数
    /// </summary>
    public int NoneCount { get; init; }

    /// <summary>
    /// 各月采购金额 / 涨价合计 / 跌价合计（采购订单价格历史推移用；筛选后全量行）
    /// </summary>
    public List<TaktPurchasePriceTrendMonthSummaryDto> MonthSummaries { get; init; } = new();

    /// <summary>
    /// 期间采购金额合计（采购订单价格历史推移用）
    /// </summary>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// 期间涨价合计（采购订单价格历史推移用）
    /// </summary>
    public decimal TotalUpAmount { get; init; }

    /// <summary>
    /// 期间跌价合计（采购订单价格历史推移用）
    /// </summary>
    public decimal TotalDownAmount { get; init; }

    /// <summary>
    /// 空结果
    /// </summary>
    public static TaktPurchasePriceTrendAnalysisBuilt Empty() => new();
}

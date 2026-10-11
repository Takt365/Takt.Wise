// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：ITaktSalesPriceTrendMonthlyAnalysisBuilder.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移转置分析构建器接口（列=ValidFrom）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格推移转置分析构建器（列按 ValidFrom，不按自然月合并）
/// </summary>
public interface ITaktSalesPriceTrendMonthlyAnalysisBuilder
{
    /// <summary>
    /// 构建销售价格推移转置分析全量结果（列=ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    Task<TaktSalesPriceTrendAnalysisBuilt> BuildAsync(TaktSalesPriceTrendQueryDto queryDto);
}

/// <summary>
/// 销售价格推移内存构建结果
/// </summary>
public sealed class TaktSalesPriceTrendAnalysisBuilt
{
    /// <summary>
    /// 过滤并排序后的全量行
    /// </summary>
    public List<TaktSalesPriceTrendDto> OrderedRows { get; init; } = new();

    /// <summary>
    /// 有效起始日列顺序 yyyy-MM-dd
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
    /// 空结果
    /// </summary>
    public static TaktSalesPriceTrendAnalysisBuilt Empty() => new();
}

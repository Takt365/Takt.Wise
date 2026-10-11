// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostItemModelEnrichmentHelper.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种元数据回填与机种月平均材料成本计算（纯函数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 物料成本机种 enrichment 计算辅助（无 I/O）
/// </summary>
public static class TaktBomMaterialCostItemModelEnrichmentHelper
{
    /// <summary>
    /// 机种月均口径分界月（固定）：核算月 &lt; 此值用产品月成本；≥ 此值用产品月计算
    /// </summary>
    public const string ModelAverageSourceSwitchPeriod = "2026-06";

    /// <summary>
    /// 是否用产品月成本（ProductMonthlyCost）作机种月均源：核算月 &lt; 2026-06 为 true；否则用产品月计算
    /// </summary>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <returns>true=用产品月成本；false=用产品月计算</returns>
    public static bool UseProductMonthlyCostForModelAverage(string? costingPeriod)
    {
        var period = costingPeriod?.Trim() ?? string.Empty;
        if (period.Length == 0)
        {
            return false;
        }
        return string.CompareOrdinal(period, ModelAverageSourceSwitchPeriod) < 0;
    }

    /// <summary>
    /// 按核算月取机种月均用的单产品金额（&lt;2026-06→产品月成本；≥2026-06→产品月计算）
    /// </summary>
    /// <param name="header">主表行</param>
    /// <returns>参与平均的金额</returns>
    public static decimal ResolveProductAmountForModelAverage(TaktBomMaterialCost header)
    {
        ArgumentNullException.ThrowIfNull(header);
        return UseProductMonthlyCostForModelAverage(header.CostingPeriod)
            ? header.ProductMonthlyCost
            : header.ProductMonthlyCalculation;
    }

    /// <summary>
    /// 同组主表行：按产品去重后取机种月均源金额（&gt;0），供算术平均
    /// </summary>
    /// <param name="headers">同工厂+物料类型+机种+核算月主表行</param>
    /// <returns>各产品金额列表</returns>
    public static List<decimal> CollectPositiveProductAmountsForModelAverage(
        IEnumerable<TaktBomMaterialCost> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);
        return headers
            .GroupBy(
                h => (h.ProductCode ?? string.Empty).Trim(),
                StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .Select(g =>
            {
                var row = g.OrderByDescending(h => h.Id).First();
                return ResolveProductAmountForModelAverage(row);
            })
            .Where(c => c > 0m)
            .ToList();
    }

    /// <summary>
    /// 计算机种指定月份平均材料成本（生产相关=X、PCB SECT 标识为空、采购类型=F；各成品有成本的参与算术平均）
    /// </summary>
    /// <param name="catalogProductCodes">成品编码清单（须与主表产品集一致，勿用型号目的地扩编）</param>
    /// <param name="monthRows">该工厂该月全部 BOM 行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="periodKey">期间键 yyyy-MM</param>
    /// <returns>机种月成本；无有效成品成本时为 0</returns>
    public static decimal ComputeModelMonthlyAverageCost(
        IReadOnlyList<string> catalogProductCodes,
        IReadOnlyList<TaktBomMaterialCostItem> monthRows,
        string plantCode,
        string periodKey)
    {
        ArgumentNullException.ThrowIfNull(catalogProductCodes);
        ArgumentNullException.ThrowIfNull(monthRows);
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        if (catalogProductCodes.Count == 0)
        {
            return 0m;
        }
        var productGroups = monthRows
            .Where(TaktBomMaterialCostItemLineCostHelper.CountsTowardBomMaterialCostItem)
            .GroupBy(r => r.ProductCode ?? string.Empty, StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var costs = new List<decimal>();
        foreach (var catalogProductCode in catalogProductCodes)
        {
            if (string.IsNullOrWhiteSpace(catalogProductCode))
            {
                continue;
            }
            foreach (var (productCode, rows) in productGroups)
            {
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(productCode, catalogProductCode))
                {
                    continue;
                }
                var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
                    rows, plantCode, productCode, periodKey);
                if (snapshot.Count == 0)
                {
                    break;
                }
                costs.Add(TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot));
                break;
            }
        }
        if (costs.Count == 0)
        {
            return 0m;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
    }

    /// <summary>
    /// 按已收集的产品金额列表计算机种月平均（仅 &gt; 0 参与；空集返回 0）
    /// </summary>
    /// <param name="productAmounts">同工厂+物料类型+机种+月份下各产品金额（调用方宜按产品去重）</param>
    /// <returns>机种月平均成本</returns>
    public static decimal ComputeModelMonthlyAverageFromProductCosts(IReadOnlyList<decimal> productAmounts)
    {
        ArgumentNullException.ThrowIfNull(productAmounts);
        var costs = productAmounts.Where(c => c > 0m).ToList();
        if (costs.Count == 0)
        {
            return 0m;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
    }

    /// <summary>
    /// 按主表行计算机种月平均（固定口径：核算月 &lt; 2026-06 用产品月成本；≥ 2026-06 用产品月计算）
    /// </summary>
    /// <param name="headers">同工厂+物料类型+机种+核算月主表行</param>
    /// <returns>机种月平均成本</returns>
    public static decimal ComputeModelMonthlyAverageFromHeaders(IEnumerable<TaktBomMaterialCost> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);
        var amounts = CollectPositiveProductAmountsForModelAverage(headers);
        return ComputeModelMonthlyAverageFromProductCosts(amounts);
    }
}

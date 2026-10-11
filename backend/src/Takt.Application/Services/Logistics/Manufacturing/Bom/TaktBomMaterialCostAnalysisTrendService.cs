// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisTrendService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析月度涨跌服务（与转置/差异分析分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析月度涨跌服务（复用分析服务加载成本头；与 CRUD/转置控制器分离）
/// </summary>
public class TaktBomMaterialCostAnalysisTrendService : TaktServiceBase, ITaktBomMaterialCostAnalysisTrendService
{
    private readonly ITaktBomMaterialCostAnalysisService _bomMaterialCostAnalysisService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostAnalysisService">成本分析服务（加载汇总头）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostAnalysisTrendService(
        ITaktBomMaterialCostAnalysisService bomMaterialCostAnalysisService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostAnalysisService = bomMaterialCostAnalysisService;
    }

    /// <inheritdoc />
    public async Task<TaktBomMaterialCostAnalysisMonthlyTrendResultDto> GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ModelCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = queryDto.ModelCode.Trim();
        var productCode = string.IsNullOrWhiteSpace(queryDto.ProductCode) ? null : queryDto.ProductCode.Trim();
        var allMaterialsUnderModel = productCode == null;
        var (rangeStart, rangeEnd) = TaktBomMaterialCostAnalysisService.ResolvePeriodRangeBounds(
            queryDto.PeriodStart, queryDto.PeriodEnd);
        var loadQuery = new TaktBomMaterialCostAnalysisTransposedQueryDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode,
            CostingDateStart = rangeStart,
            CostingDateEnd = rangeEnd,
            PageIndex = 1,
            PageSize = TaktPagedClamp.DefaultPageSize,
        };
        var rows = await _bomMaterialCostAnalysisService.LoadBomMaterialCostAnalysisHeadersAsync(loadQuery);
        var periodOrder = TaktBomMaterialCostAnalysisService.BuildCostHeaderPeriodOrder(rows, rangeStart, rangeEnd);
        var productCodesInScope = TaktBomMaterialCostAnalysisService.ResolveProductCodesInScope(
            rows, plantCode, modelCode, productCode);
        var productDescription = allMaterialsUnderModel
            ? string.Empty
            : rows.FirstOrDefault(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode!))?.ProductDescription ?? string.Empty;
        var trendLines = new List<TaktBomMaterialCostAnalysisMonthlyTrendLineDto>();
        decimal? previousCost = null;
        string? previousPeriod = null;
        foreach (var period in periodOrder)
        {
            decimal totalCost;
            if (allMaterialsUnderModel)
            {
                var costs = productCodesInScope
                    .Select(pc => TaktBomMaterialCostAnalysisService.ResolveProductMonthlyCostFromHeaders(
                        rows, plantCode, modelCode, pc, period))
                    .Where(c => c > 0m)
                    .ToList();
                if (costs.Count == 0)
                {
                    continue;
                }
                totalCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
            }
            else
            {
                totalCost = TaktBomMaterialCostAnalysisService.ResolveProductMonthlyCostFromHeaders(
                    rows, plantCode, modelCode, productCode!, period);
                if (totalCost <= 0m)
                {
                    continue;
                }
            }
            var (varianceAmount, variancePercent, trend) =
                TaktBomMaterialCostAnalysisService.ComputeMonthOverMonthTrend(totalCost, previousCost);
            trendLines.Add(new TaktBomMaterialCostAnalysisMonthlyTrendLineDto
            {
                Period = period,
                TotalCost = totalCost,
                BasePeriod = previousPeriod,
                BaseTotalCost = previousCost,
                VarianceAmount = varianceAmount,
                VariancePercent = variancePercent,
                Trend = trend,
            });
            previousCost = totalCost;
            previousPeriod = period;
        }
        return new TaktBomMaterialCostAnalysisMonthlyTrendResultDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode ?? string.Empty,
            ProductDescription = productDescription,
            AllMaterialsUnderModel = allMaterialsUnderModel,
            Lines = trendLines,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(
        TaktBomMaterialCostAnalysisMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostAnalysisMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCode", "productDescription", "period", "totalCost",
            "basePeriod", "baseTotalCost", "varianceAmount", "variancePercent", "trend",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "产品编码", "产品描述", "年月", "材料总成本",
            "对比基准月", "基准月成本", "环比差额", "环比%", "涨跌",
        };
        var exportRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = result.PlantCode,
            ["modelCode"] = result.ModelCode,
            ["productCode"] = result.ProductCode,
            ["productDescription"] = result.ProductDescription,
            ["period"] = line.Period,
            ["totalCost"] = line.TotalCost,
            ["basePeriod"] = line.BasePeriod,
            ["baseTotalCost"] = line.BaseTotalCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(line.VariancePercent),
            ["trend"] = line.Trend,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "BOM材料月度涨跌",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(
                result.PlantCode,
                string.IsNullOrWhiteSpace(result.ModelCode)
                    ? "BOM材料月度涨跌"
                    : $"BOM材料月度涨跌_{result.ModelCode.Trim()}"));
    }
}

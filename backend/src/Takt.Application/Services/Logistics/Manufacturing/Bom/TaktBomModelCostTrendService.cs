// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomModelCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 机种成本推移：按机种对比各成品的成本组件差异（机种可空=全部）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 机种成本推移：一行一个成品。各月列为成品月计算；差额为最后月减前一月。
/// 组件差异取期间止月，只列出同一机种各成品间用量或单价不一致的成本组件（编码-用量:价格）。共通价格、差异价格为这些组件的行成本合计。
/// 机种空则范围内每个机种各自对比。
/// </summary>
public class TaktBomModelCostTrendService : TaktServiceBase, ITaktBomModelCostTrendService
{
    /// <summary>
    /// BOM 成本明细按年分表基表名（与 SugarTable 一致）
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomModelCostTrendService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktModelDestination> modelDestinationRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _modelDestinationRepository = modelDestinationRepository;
    }

    /// <summary>
    /// 机种成本推移分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    public async Task<TaktBomModelCostTrendResultDto> GetBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildModelCostTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktBomModelCostTrendResultDto
        {
            Paged = TaktPagedResult<TaktBomModelCostTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            ComparePeriod = built.ComparePeriod,
            ModelCount = built.ModelCount,
            ProductCount = built.OrderedRows.Count,
            PeriodCostTotals = SumPeriodCosts(built.OrderedRows, built.PeriodOrder),
            VarianceAmountTotal = SumVariance(built.OrderedRows),
            CommonPriceTotal = SumOptionalPrice(built.OrderedRows, row => row.CommonPrice),
            VariancePriceTotal = SumOptionalPrice(built.OrderedRows, row => row.VariancePrice),
        };
    }

    /// <summary>
    /// 导出机种成本推移分析
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildModelCostTrendAnalysisAsync(query);
        var columnKeys = new List<string>
        {
            "modelCode", "productCode", "productDescription",
        };
        var columnLabels = new List<string>
        {
            "机种", "产品", "产品描述",
        };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.Add("varianceAmount");
        columnLabels.Add("差异");
        columnKeys.Add("commonPrice");
        columnLabels.Add("共通价格");
        columnKeys.Add("variancePrice");
        columnLabels.Add("差异价格");
        columnKeys.Add("componentVarianceText");
        columnLabels.Add("组件差异");
        var exportRows = built.OrderedRows
            .Select(row =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["modelCode"] = row.ModelCode,
                    ["productCode"] = row.ProductCode,
                    ["productDescription"] = row.ProductDescription,
                    ["varianceAmount"] = row.VarianceAmount,
                    ["commonPrice"] = row.CommonPrice,
                    ["variancePrice"] = row.VariancePrice,
                    ["componentVarianceText"] = row.ComponentVarianceText,
                };
                foreach (var period in built.PeriodOrder)
                {
                    dict[$"period_{period}"] = row.PeriodCosts.TryGetValue(period, out var cost) ? cost : null;
                }
                return (IReadOnlyDictionary<string, object?>)dict;
            })
            .ToList();
        const string defaultSheet = "机种成本推移";
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? defaultSheet,
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, defaultSheet));
    }

    /// <summary>
    /// 一行一个成品：期间内各月成本；组件差异只比较期间止月、同一机种的成品
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>按机种、产品排序的全量行</returns>
    private async Task<ModelCostTrendAnalysisBuilt> BuildModelCostTrendAnalysisAsync(
        TaktBomModelCostTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var materialType = queryDto.MaterialType?.Trim();
        var (periodStart, periodEnd) = NormalizeMovingPricePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
        if (string.IsNullOrEmpty(materialType) || (!periodStart.HasValue && !periodEnd.HasValue))
        {
            return ModelCostTrendAnalysisBuilt.Empty(null, new List<string>());
        }
        var rangeStart = periodStart ?? periodEnd!.Value;
        var rangeEnd = periodEnd ?? periodStart!.Value;
        var periodOrder = BuildPeriodOrder(rangeStart, rangeEnd);
        var comparePeriod = rangeEnd.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        var modelCode = queryDto.ModelCode?.Trim();
        IReadOnlyList<string>? modelFilter = string.IsNullOrEmpty(modelCode) ? null : new[] { modelCode };
        var headers = await LoadCostHeadersAsync(plantCode, modelFilter, rangeStart, rangeEnd, materialType);
        if (headers.Count == 0)
        {
            return ModelCostTrendAnalysisBuilt.Empty(comparePeriod, periodOrder);
        }
        var destinations = await _modelDestinationRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode);
        var rows = BuildProductRows(plantCode, headers, periodOrder, destinations);
        var endMonthProducts = headers
            .Where(h => string.Equals(h.CostingPeriod?.Trim(), comparePeriod, StringComparison.Ordinal))
            .Select(h => TaktBomMaterialCostItemLineCostHelper.NormalizeProductCodeForTree(h.ProductCode))
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (endMonthProducts.Count > 0)
        {
            var items = await LoadBomCostItemsForProductsAsync(plantCode, endMonthProducts, rangeEnd, rangeEnd);
            ApplyComponentVariance(rows, items, endMonthProducts);
        }
        var ordered = rows
            .OrderBy(r => r.ModelCode, StringComparer.Ordinal)
            .ThenBy(r => r.ProductCode, StringComparer.Ordinal)
            .ToList();
        return new ModelCostTrendAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            ComparePeriod = comparePeriod,
            ModelCount = ordered.Select(r => r.ModelCode).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
        };
    }

    /// <summary>
    /// 按规范化产品码归并头表，生成成品行与各月成本、相邻月差额
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="headers">期间内成本头</param>
    /// <param name="periodOrder">月份列</param>
    /// <param name="destinations">型号目的地</param>
    /// <returns>成品行</returns>
    private static List<TaktBomModelCostTrendDto> BuildProductRows(
        string plantCode,
        IReadOnlyList<TaktBomMaterialCost> headers,
        IReadOnlyList<string> periodOrder,
        IReadOnlyList<TaktModelDestination> destinations)
    {
        var rows = new List<TaktBomModelCostTrendDto>();
        foreach (var group in headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .GroupBy(
                h => TaktBomMaterialCostItemLineCostHelper.NormalizeProductCodeForTree(h.ProductCode),
                StringComparer.OrdinalIgnoreCase))
        {
            if (string.IsNullOrEmpty(group.Key))
            {
                continue;
            }
            var latest = group
                .OrderByDescending(h => h.CostingDate)
                .ThenByDescending(h => h.Id)
                .First();
            var model = latest.ModelCode?.Trim() ?? string.Empty;
            var costs = new Dictionary<string, decimal>(StringComparer.Ordinal);
            foreach (var monthGroup in group.GroupBy(h => h.CostingPeriod?.Trim() ?? string.Empty, StringComparer.Ordinal))
            {
                if (string.IsNullOrEmpty(monthGroup.Key) || !periodOrder.Contains(monthGroup.Key))
                {
                    continue;
                }
                var monthHeader = monthGroup
                    .OrderByDescending(h => h.CostingDate)
                    .ThenByDescending(h => h.Id)
                    .First();
                costs[monthGroup.Key] = TaktBomMaterialCostItemLineCostHelper.RoundCost(monthHeader.ProductMonthlyCalculation);
            }
            rows.Add(new TaktBomModelCostTrendDto
            {
                PlantCode = plantCode,
                ModelCode = model,
                ModelName = ResolveModelName(model, destinations),
                ProductCode = latest.ProductCode.Trim(),
                ProductDescription = latest.ProductDescription?.Trim() ?? string.Empty,
                CurrencyCode = latest.CurrencyCode?.Trim() ?? string.Empty,
                PeriodCosts = costs,
                VarianceAmount = ResolveAdjacentVariance(costs, periodOrder),
            });
        }
        return rows;
    }

    /// <summary>
    /// 期间止月：同机种成品间用量或单价不一致的组件，写到各成品行（编码-用量:价格）
    /// </summary>
    /// <param name="rows">成品行</param>
    /// <param name="items">止月成本明细</param>
    /// <param name="endMonthProductKeys">止月有头表的规范化产品码</param>
    private static void ApplyComponentVariance(
        List<TaktBomModelCostTrendDto> rows,
        IReadOnlyList<TaktBomMaterialCostItem> items,
        IReadOnlyList<string> endMonthProductKeys)
    {
        var endSet = endMonthProductKeys.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var displayToKey = rows.ToDictionary(
            r => r.ProductCode,
            r => TaktBomMaterialCostItemLineCostHelper.NormalizeProductCodeForTree(r.ProductCode),
            StringComparer.OrdinalIgnoreCase);
        var canonical = BuildCanonicalProductMap(rows.Select(r => r.ProductCode).ToList());
        var usage = new Dictionary<string, Dictionary<string, ComponentUsage>>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.ComponentCode) || string.IsNullOrWhiteSpace(item.ProductCode))
            {
                continue;
            }
            if (!canonical.TryGetValue(item.ProductCode.Trim(), out var productCode))
            {
                continue;
            }
            var productKey = TaktBomMaterialCostItemLineCostHelper.NormalizeProductCodeForTree(productCode);
            if (!endSet.Contains(productKey))
            {
                continue;
            }
            var componentCode = item.ComponentCode.Trim();
            if (!usage.TryGetValue(componentCode, out var perProduct))
            {
                perProduct = new Dictionary<string, ComponentUsage>(StringComparer.OrdinalIgnoreCase);
                usage[componentCode] = perProduct;
            }
            if (!perProduct.TryGetValue(productKey, out var bucket))
            {
                bucket = new ComponentUsage();
                perProduct[productKey] = bucket;
            }
            bucket.Quantity += item.ComponentQuantity;
            bucket.LineCost += TaktBomMaterialCostItemLineCostHelper.CalculateLineCost(item);
        }
        foreach (var modelGroup in rows.GroupBy(r => r.ModelCode ?? string.Empty, StringComparer.OrdinalIgnoreCase))
        {
            var peers = modelGroup
                .Where(r => displayToKey.TryGetValue(r.ProductCode, out var key) && endSet.Contains(key))
                .ToList();
            var differing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (peers.Count >= 2)
            {
                var peerKeys = peers
                    .Select(r => displayToKey[r.ProductCode])
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                foreach (var kv in usage)
                {
                    if (ComponentDiffers(kv.Value, peerKeys))
                    {
                        differing.Add(kv.Key);
                    }
                }
            }
            foreach (var row in peers)
            {
                FillComponentPrices(row, displayToKey[row.ProductCode], usage, differing);
            }
        }
    }

    /// <summary>
    /// 把止月组件拆成共通行成本与差异行成本，并写出差异组件文本
    /// </summary>
    /// <param name="row">成品行</param>
    /// <param name="productKey">规范化产品码</param>
    /// <param name="usage">组件 → 产品用量</param>
    /// <param name="differing">差异组件编码</param>
    private static void FillComponentPrices(
        TaktBomModelCostTrendDto row,
        string productKey,
        IReadOnlyDictionary<string, Dictionary<string, ComponentUsage>> usage,
        HashSet<string> differing)
    {
        decimal common = 0m;
        decimal variance = 0m;
        var parts = new List<string>();
        foreach (var componentCode in usage.Keys.OrderBy(c => c, StringComparer.Ordinal))
        {
            if (!usage.TryGetValue(componentCode, out var perProduct)
                || !perProduct.TryGetValue(productKey, out var bucket))
            {
                continue;
            }
            var lineCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(bucket.LineCost);
            if (!differing.Contains(componentCode))
            {
                common += lineCost;
                continue;
            }
            variance += lineCost;
            var qty = TaktBomMaterialCostItemLineCostHelper.RoundCost(bucket.Quantity);
            var unitPrice = qty == 0m
                ? 0m
                : TaktBomMaterialCostItemLineCostHelper.RoundCost(bucket.LineCost / qty);
            parts.Add($"{componentCode}-{FormatQuantity(qty)}:{unitPrice.ToString("0.00000", CultureInfo.InvariantCulture)}");
        }
        row.CommonPrice = TaktBomMaterialCostItemLineCostHelper.RoundCost(common);
        row.VariancePrice = TaktBomMaterialCostItemLineCostHelper.RoundCost(variance);
        row.ComponentVarianceText = string.Join(";", parts);
    }

    /// <summary>
    /// 组件在同机种各成品上的用量或单价是否不一致（缺件视为不同）
    /// </summary>
    /// <param name="perProduct">产品码 → 用量</param>
    /// <param name="peerKeys">参与对比的成品键</param>
    /// <returns>是否为差异组件</returns>
    private static bool ComponentDiffers(
        IReadOnlyDictionary<string, ComponentUsage> perProduct,
        IReadOnlyList<string> peerKeys)
    {
        var signatures = new HashSet<string>(StringComparer.Ordinal);
        foreach (var key in peerKeys)
        {
            if (!perProduct.TryGetValue(key, out var bucket))
            {
                signatures.Add("absent");
                continue;
            }
            var qty = TaktBomMaterialCostItemLineCostHelper.RoundCost(bucket.Quantity);
            var unitPrice = qty == 0m
                ? 0m
                : TaktBomMaterialCostItemLineCostHelper.RoundCost(bucket.LineCost / qty);
            signatures.Add($"{qty.ToString("0.00000", CultureInfo.InvariantCulture)}|{unitPrice.ToString("0.00000", CultureInfo.InvariantCulture)}");
        }
        return signatures.Count > 1;
    }

    /// <summary>
    /// 最后一月成本减前一月；任一月缺失则空
    /// </summary>
    /// <param name="costs">各月成本</param>
    /// <param name="periodOrder">月份列</param>
    /// <returns>差额</returns>
    private static decimal? ResolveAdjacentVariance(
        IReadOnlyDictionary<string, decimal> costs,
        IReadOnlyList<string> periodOrder)
    {
        if (periodOrder.Count < 2)
        {
            return null;
        }
        var last = periodOrder[^1];
        var previous = periodOrder[^2];
        if (!costs.TryGetValue(last, out var lastCost) || !costs.TryGetValue(previous, out var previousCost))
        {
            return null;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(lastCost - previousCost);
    }

    /// <summary>
    /// 连续月份列
    /// </summary>
    /// <param name="start">起（月初）</param>
    /// <param name="end">止（月初）</param>
    /// <returns>yyyy-MM</returns>
    private static List<string> BuildPeriodOrder(DateTime start, DateTime end)
    {
        var order = new List<string>();
        for (var cursor = start; cursor <= end; cursor = cursor.AddMonths(1))
        {
            order.Add(cursor.ToString("yyyy-MM", CultureInfo.InvariantCulture));
        }
        return order;
    }

    /// <summary>
    /// 各月成本合计
    /// </summary>
    /// <param name="rows">成品行</param>
    /// <param name="periodOrder">月份列</param>
    /// <returns>期间合计</returns>
    private static Dictionary<string, decimal> SumPeriodCosts(
        IReadOnlyList<TaktBomModelCostTrendDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        var totals = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            decimal sum = 0m;
            var any = false;
            foreach (var row in rows)
            {
                if (row.PeriodCosts.TryGetValue(period, out var cost))
                {
                    sum += cost;
                    any = true;
                }
            }
            if (any)
            {
                totals[period] = TaktBomMaterialCostItemLineCostHelper.RoundCost(sum);
            }
        }
        return totals;
    }

    /// <summary>
    /// 数量展示（整数不带小数）
    /// </summary>
    /// <param name="quantity">数量</param>
    /// <returns>文本</returns>
    private static string FormatQuantity(decimal quantity)
    {
        return quantity == decimal.Truncate(quantity)
            ? decimal.Truncate(quantity).ToString(CultureInfo.InvariantCulture)
            : quantity.ToString("0.#####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 机种名称（型号目的地；无则回落机种码）
    /// </summary>
    /// <param name="modelCode">机种</param>
    /// <param name="destinations">型号目的地</param>
    /// <returns>名称</returns>
    private static string ResolveModelName(string modelCode, IReadOnlyList<TaktModelDestination> destinations)
    {
        if (string.IsNullOrWhiteSpace(modelCode))
        {
            return string.Empty;
        }
        var name = destinations
            .Where(d => string.Equals(d.ModelCode?.Trim(), modelCode, StringComparison.OrdinalIgnoreCase))
            .Select(d => d.ModelName?.Trim())
            .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n));
        return string.IsNullOrWhiteSpace(name) ? modelCode : name;
    }

    /// <summary>
    /// 头表产品码及其 10/18 位变体 → 头表产品码
    /// </summary>
    /// <param name="productCodes">产品编码</param>
    /// <returns>查找表</returns>
    private static Dictionary<string, string> BuildCanonicalProductMap(IReadOnlyList<string> productCodes)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in productCodes)
        {
            foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(code))
            {
                if (!map.ContainsKey(variant))
                {
                    map[variant] = code;
                }
            }
        }
        return map;
    }

    /// <summary>
    /// 极差合计；无行时 null
    /// </summary>
    /// <param name="rows">差异行</param>
    /// <returns>合计</returns>
    private static decimal? SumVariance(IReadOnlyList<TaktBomModelCostTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return null;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(
            rows.Sum(r => r.VarianceAmount ?? 0m));
    }

    /// <summary>
    /// 可选金额合计；无行时 null
    /// </summary>
    /// <param name="rows">成品行</param>
    /// <param name="selector">金额</param>
    /// <returns>合计</returns>
    private static decimal? SumOptionalPrice(
        IReadOnlyList<TaktBomModelCostTrendDto> rows,
        Func<TaktBomModelCostTrendDto, decimal?> selector)
    {
        if (rows.Count == 0)
        {
            return null;
        }
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(rows.Sum(r => selector(r) ?? 0m));
    }

    /// <summary>
    /// 加载期间内成本头（工厂 + 物料类型 + 可选机种；不折叠月份）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCodes">机种（null/空=全部）</param>
    /// <param name="costingMonthStart">核算月起</param>
    /// <param name="costingMonthEnd">核算月止</param>
    /// <param name="materialType">物料类型</param>
    /// <returns>成本头</returns>
    private async Task<List<TaktBomMaterialCost>> LoadCostHeadersAsync(
        string plantCode,
        IReadOnlyList<string>? modelCodes,
        DateTime costingMonthStart,
        DateTime costingMonthEnd,
        string materialType)
    {
        var startPeriod = costingMonthStart.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        var endPeriod = costingMonthEnd.ToString("yyyy-MM", CultureInfo.InvariantCulture);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.MaterialType == materialType
            && x.CostingPeriod != null
            && x.CostingPeriod.CompareTo(startPeriod) >= 0
            && x.CostingPeriod.CompareTo(endPeriod) <= 0);
        if (modelCodes is { Count: 1 })
        {
            var model = modelCodes[0];
            exp = exp.And(x => x.ModelCode == model);
        }
        return await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
    }

    /// <summary>
    /// 加载成品 BOM 明细（生产相关=X、PCB SECT 标识为空、采购类型=F；按核算月；不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月起</param>
    /// <param name="costingMonthEnd">核算月止</param>
    /// <returns>成本明细</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime? costingMonthStart,
        DateTime? costingMonthEnd)
    {
        var allItems = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        DateTime? costingExclusiveEnd = costingMonthEnd.HasValue
            ? costingMonthEnd.Value.AddMonths(1)
            : null;
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return allItems;
        }
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktBomMaterialCostItem>();
            exp = exp.And(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.ProductCode));
            if (costingMonthStart.HasValue)
            {
                var start = costingMonthStart.Value;
                exp = exp.And(x => x.CostingDate >= start);
            }
            if (costingExclusiveEnd.HasValue)
            {
                var endExclusive = costingExclusiveEnd.Value;
                exp = exp.And(x => x.CostingDate < endExclusive);
            }
            var part = await GetBomItemListForRangeAsync(exp.ToExpression(), costingMonthStart, costingMonthEnd);
            allItems.AddRange(TaktBomMaterialCostItemLineCostHelper.FilterBomMaterialCostItemRows(part));
        }
        return allItems;
    }

    /// <summary>
    /// 归一化移动价格期间上下界（存当月首日）
    /// </summary>
    /// <param name="periodDateStart">期间起</param>
    /// <param name="periodDateEnd">期间止</param>
    /// <returns>月初起止</returns>
    private static (DateTime? Start, DateTime? End) NormalizeMovingPricePeriodBounds(
        DateTime? periodDateStart,
        DateTime? periodDateEnd)
    {
        DateTime? start = periodDateStart.HasValue
            ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
            : null;
        DateTime? end = periodDateEnd.HasValue
            ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
            : null;
        if (start.HasValue && end.HasValue && start > end)
        {
            (start, end) = (end, start);
        }
        return (start, end);
    }

    /// <summary>
    /// 机种成本推移内存构建结果
    /// </summary>
    private sealed class ModelCostTrendAnalysisBuilt
    {
        /// <summary>
        /// 过滤并排序后的全量行
        /// </summary>
        public List<TaktBomModelCostTrendDto> OrderedRows { get; init; } = new();

        /// <summary>
        /// 月份列
        /// </summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>
        /// 组件差异月份
        /// </summary>
        public string? ComparePeriod { get; init; }

        /// <summary>
        /// 机种数
        /// </summary>
        public int ModelCount { get; init; }

        /// <summary>
        /// 空结果
        /// </summary>
        /// <param name="comparePeriod">对比月</param>
        /// <param name="periodOrder">月份列</param>
        /// <returns>空构建结果</returns>
        public static ModelCostTrendAnalysisBuilt Empty(string? comparePeriod, List<string> periodOrder) => new()
        {
            ComparePeriod = comparePeriod,
            PeriodOrder = periodOrder,
        };
    }

    /// <summary>
    /// 某一成品上某组件的汇总用量
    /// </summary>
    private sealed class ComponentUsage
    {
        /// <summary>
        /// 组件数量合计
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 行成本合计
        /// </summary>
        public decimal LineCost { get; set; }

        /// <summary>
        /// 组件描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null（回退实体基表）
    /// </summary>
    /// <param name="year">自然年</param>
    /// <returns>年分表名；不存在时为 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 按年分表查询 BOM 成本明细（可跨年合并；年分表未建时回退基表）
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="start">起</param>
    /// <param name="end">止</param>
    /// <param name="maxRows">总行上限（可选）</param>
    /// <returns>明细列表</returns>
    private async Task<List<TaktBomMaterialCostItem>> GetBomItemListForRangeAsync(
        Expression<Func<TaktBomMaterialCostItem, bool>> predicate,
        DateTime? start,
        DateTime? end,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYears(start, end);
        var result = new List<TaktBomMaterialCostItem>();
        var seenIds = new HashSet<long>();
        bool TryAppend(IEnumerable<TaktBomMaterialCostItem> rows)
        {
            foreach (var row in rows)
            {
                if (!seenIds.Add(row.Id))
                {
                    continue;
                }
                result.Add(row);
                if (maxRows.HasValue && result.Count >= maxRows.Value)
                {
                    return false;
                }
            }
            return true;
        }
        foreach (var year in years)
        {
            if (maxRows.HasValue && result.Count >= maxRows.Value)
            {
                break;
            }
            var table = await ResolveBomItemPhysicalTableAsync(year);
            if (table == null)
            {
                continue;
            }
            if (maxRows.HasValue)
            {
                var remaining = maxRows.Value - result.Count;
                var part = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining, table);
                if (!TryAppend(part))
                {
                    break;
                }
            }
            else
            {
                var part = await _bomMaterialCostItemRepository.GetListAsync(predicate, table);
                TryAppend(part);
            }
        }
        if (!maxRows.HasValue || result.Count < maxRows.Value)
        {
            List<TaktBomMaterialCostItem> basePart;
            if (maxRows.HasValue)
            {
                basePart = await _bomMaterialCostItemRepository.GetListForExportAsync(
                    predicate, maxRows.Value - result.Count);
            }
            else
            {
                basePart = await _bomMaterialCostItemRepository.GetListAsync(predicate);
            }
            var yearSet = years.ToHashSet();
            TryAppend(basePart.Where(r => yearSet.Contains(r.CostingDate.Year)));
        }
        return result;
    }
}

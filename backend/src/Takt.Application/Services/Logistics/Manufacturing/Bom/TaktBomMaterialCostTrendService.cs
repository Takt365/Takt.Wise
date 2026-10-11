// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostTrendService.cs
// 创建时间：2026-08-01
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 产品成本推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 产品成本推移分析服务（读 BOM 成本本表；与明细 CRUD 服务分离）
/// </summary>
public class TaktBomMaterialCostTrendService : TaktServiceBase, ITaktBomMaterialCostTrendService
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
    public TaktBomMaterialCostTrendService(
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
    /// 产品成本推移：单个产品下明细组件×月材料成本并算环比
    /// </summary>
    /// <param name="queryDto">查询 DTO（PlantCode + ProductCode 必填；ModelCode 可选）</param>
    /// <returns>明细组件×月材料成本结果</returns>
    public async Task<TaktBomMaterialCostTrendComponentMovingPriceResultDto> GetBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostTrendComponentMovingPriceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildComponentMovingPriceAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        var (periodCostTotals, varianceAmountTotal) = SumComponentMovingPriceRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        return new TaktBomMaterialCostTrendComponentMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostTrendComponentMovingPriceDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
            ComponentCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NewCount = built.NewCount,
            RemovedCount = built.RemovedCount,
            NoneCount = built.NoneCount,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
    }

    /// <summary>
    /// ExportBomMaterialCostTrendComponentMovingPriceAnalysisAsync
    /// </summary>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostTrendComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostTrendComponentMovingPriceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        // 导出取全量分析行，勿套用列表 MaxPageSize(100)；按条件全量不截断
        var built = await BuildComponentMovingPriceAnalysisAsync(query);
        var (periodCostTotals, varianceAmountTotal) = SumComponentMovingPriceRowGrandTotals(
            built.OrderedRows, built.PeriodOrder);
        var result = new TaktBomMaterialCostTrendComponentMovingPriceResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostTrendComponentMovingPriceDto>.Create(
                built.OrderedRows, built.OrderedRows.Count, 1, Math.Max(built.OrderedRows.Count, 1)),
            PeriodOrder = built.PeriodOrder,
            ProductCodes = built.ProductCodes,
            ComponentCount = built.OrderedRows.Count,
            BasePeriod = built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NewCount = built.NewCount,
            RemovedCount = built.RemovedCount,
            NoneCount = built.NoneCount,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
        var columnKeys = new List<string>
        {
            "plantCode", "modelCode", "productCode", "productDescription",
            "lineNumber", "bomLevel", "bomItemCode",
            "componentCode", "componentDescription", "componentQuantity",
            "productionRelated", "purchaseType", "currencyCode",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "机种编码", "产品编码", "产品描述",
            "行号", "层级", "BOM项目号",
            "组件编码", "组件描述", "组件数量",
            "生产相关", "采购类型", "币种",
        };
        foreach (var period in result.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });

        var exportRows = (result.Paged.Data ?? new List<TaktBomMaterialCostTrendComponentMovingPriceDto>())
            .Select(row =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
                {
                    ["plantCode"] = row.PlantCode,
                    ["modelCode"] = row.ModelCode,
                    ["productCode"] = row.ProductCode,
                    ["productDescription"] = row.ProductDescription,
                    ["lineNumber"] = row.LineNumber,
                    ["bomLevel"] = row.BomLevel,
                    ["bomItemCode"] = row.BomItemCode,
                    ["componentCode"] = row.ComponentCode,
                    ["componentDescription"] = row.ComponentDescription,
                    ["componentQuantity"] = row.ComponentQuantity,
                    ["productionRelated"] = row.ProductionRelated,
                    ["purchaseType"] = row.PurchaseType,
                    ["currencyCode"] = row.CurrencyCode,
                    ["basePeriod"] = row.BasePeriod,
                    ["comparePeriod"] = row.ComparePeriod,
                    ["varianceAmount"] = row.VarianceAmount,
                    ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
                    ["trend"] = row.Trend,
                };
                foreach (var period in result.PeriodOrder)
                {
                    var changeType = row.PeriodChangeTypes.TryGetValue(period, out var ct) ? ct : string.Empty;
                    if (row.PeriodMaterialCosts.TryGetValue(period, out var cost))
                    {
                        dict[$"period_{period}"] = string.IsNullOrEmpty(changeType) || changeType is "present" or "flat"
                            ? cost
                            : $"{cost} ({FormatPeriodChangeTypeLabel(changeType)})";
                    }
                    else
                    {
                        dict[$"period_{period}"] = changeType == "removed"
                            ? FormatPeriodChangeTypeLabel("removed")
                            : null;
                    }
                }
                return (IReadOnlyDictionary<string, object?>)dict;
            })
            .ToList();

        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA 产品成本推移表",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "DTA 产品成本推移表"));
    }

    /// <summary>
    /// 期间变动码导出短标签
    /// </summary>
    /// <param name="changeType">present / absent / new / removed / up / down / flat</param>
    /// <returns>中文短标签</returns>
    private static string FormatPeriodChangeTypeLabel(string changeType) => changeType switch
    {
        "new" => "新增",
        "removed" => "剔除",
        "up" => "涨",
        "down" => "跌",
        "flat" => "平",
        "present" => "有",
        "absent" => "无",
        _ => changeType,
    };

    /// <summary>
    /// 规范化物料类型筛选（空=不按类型过滤）
    /// </summary>
    /// <param name="materialType">查询传入类型</param>
    /// <returns>非空类型码；空则 null</returns>
    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    /// <summary>
    /// 从主表取机种下产品编码（可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <param name="materialType">物料类型（本表 MaterialType；空=不按类型过滤）</param>
    /// <returns>产品编码列表</returns>
    private async Task<List<string>> LoadModelProductCodesAsync(
        string plantCode,
        string modelCode,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.ModelCode == modelCode);
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        // 与唯一键 CostingPeriod（yyyy-MM）对齐，避免仅靠 CostingDate 漏月
        if (costingMonthStart.HasValue)
        {
            var startPeriod = costingMonthStart.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(startPeriod) >= 0);
        }
        if (costingMonthEnd.HasValue)
        {
            var endPeriod = costingMonthEnd.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(endPeriod) <= 0);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 加载机种产品 BOM 明细（全量展开后 Filter：生产相关=X、PCB SECT 标识为空、采购类型=F；可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月起（月初，含）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月）</param>
    /// <returns>过滤后的明细行</returns>
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
 // 10/18 位 码互认：展开查询变体后再 Contains，避免明细表空结果
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
 // 年分表 + 基表回退（与零价/移动价区间查询一致）
            var part = await GetBomItemListForRangeAsync(
                exp.ToExpression(),
                costingMonthStart,
                costingMonthEnd);
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
    /// 构建核算期间列顺序（有起止则连续月序，否则取明细 CostingDate 出现的月）
    /// </summary>
    /// <param name="costItems">BOM 成本明细</param>
    /// <param name="periodStart">期间起（月初）</param>
    /// <param name="periodEnd">期间止（月初）</param>
    /// <returns>yyyy-MM 列表</returns>
    private static List<string> BuildCostingPeriodOrder(
        IReadOnlyList<TaktBomMaterialCostItem> costItems,
        DateTime? periodStart,
        DateTime? periodEnd)
    {
        if (periodStart.HasValue && periodEnd.HasValue)
        {
            var order = new List<string>();
            for (var cursor = periodStart.Value; cursor <= periodEnd.Value; cursor = cursor.AddMonths(1))
            {
                order.Add(cursor.ToString("yyyy-MM"));
            }
            return order;
        }
        return costItems
            .Select(r => new DateTime(r.CostingDate.Year, r.CostingDate.Month, 1).ToString("yyyy-MM"))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析关注期间；未指定时取期间列最后一月
    /// </summary>
    /// <param name="focusPeriod">查询关注期间</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>yyyy-MM 或 null</returns>
    private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
    {
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            return focusPeriod.Trim();
        }
        return periodOrder.Count > 0 ? periodOrder[^1] : null;
    }

    /// <summary>
    /// 产品成本推移：单个产品下 BOM 明细行（TaktBomMaterialCostItem）× 月材料成本转置涨跌
    /// </summary>
    /// <param name="queryDto">查询条件（工厂+产品必填）</param>
    /// <returns>排序后的全量明细行与汇总</returns>
    private async Task<ComponentMovingPriceAnalysisBuilt> BuildComponentMovingPriceAnalysisAsync(
        TaktBomMaterialCostTrendComponentMovingPriceQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ProductCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var modelCode = string.IsNullOrWhiteSpace(queryDto.ModelCode) ? null : queryDto.ModelCode.Trim();
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var (periodStart, periodEnd) = NormalizeMovingPricePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
        var filterProduct = queryDto.ProductCode.Trim();

        var productMeta = await LoadProductMetaAsync(plantCode, modelCode, periodStart, periodEnd, materialType);
        var productCodesFromMeta = productMeta.Keys
            .Where(c => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(c, filterProduct))
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        var lookupCodes = TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(filterProduct)
            .Concat(productCodesFromMeta.SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            lookupCodes = new List<string> { filterProduct };
        }

        // 环比需关注月上月成本：加载区间向前扩 1 个月；展示列仍用原核算期间
        var loadStart = periodStart;
        var focusHint = !string.IsNullOrWhiteSpace(queryDto.FocusPeriod)
            ? queryDto.FocusPeriod.Trim()
            : (periodEnd.HasValue ? periodEnd.Value.ToString("yyyy-MM") : null);
        if (!string.IsNullOrWhiteSpace(focusHint)
            && DateTime.TryParseExact(
                focusHint + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var focusMonth))
        {
            var momStart = new DateTime(focusMonth.Year, focusMonth.Month, 1).AddMonths(-1);
            if (!loadStart.HasValue || loadStart.Value > momStart)
            {
                loadStart = momStart;
            }
        }

        var costItemsRaw = await LoadBomCostItemsForProductsAsync(plantCode, lookupCodes, loadStart, periodEnd);
        var costItems = costItemsRaw
            .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, filterProduct))
            .ToList();
        var productCodes = costItems
            .Select(r => r.ProductCode?.Trim() ?? string.Empty)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (productCodes.Count == 0)
        {
            productCodes = productCodesFromMeta.Count > 0
                ? productCodesFromMeta
                : new List<string> { filterProduct };
        }
        if (costItems.Count == 0)
        {
            return ComponentMovingPriceAnalysisBuilt.Empty(productCodes);
        }

        var periodOrder = BuildCostingPeriodOrder(costItems, periodStart, periodEnd);
        var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
        var productCode = productCodes[0];
        var meta = productMeta.TryGetValue(productCode, out var m)
            ? m
            : productMeta.FirstOrDefault(kv =>
                TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(kv.Key, filterProduct)).Value
                ?? new ModelProductMeta();
        var rowModelCode = !string.IsNullOrWhiteSpace(meta.ModelCode)
            ? meta.ModelCode
            : (modelCode ?? string.Empty);
        var productDescription = meta.Description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(productDescription))
        {
            productDescription = costItems
                .OrderByDescending(r => r.CostingDate)
                .Select(r => r.ProductDescription?.Trim())
                .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                ?? string.Empty;
        }

        // 按 BOM 明细行业务键展开（Sequence+Level+Item+Component+Qty…），不做机种级合并
        var lineGroups = costItems
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode))
            .GroupBy(BuildBomLineTrendKey, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .ToList();
        var allRows = lineGroups
            .Select(group => BuildProductComponentMaterialCostTrendRow(
                plantCode,
                rowModelCode,
                productCode,
                productDescription,
                group.ToList(),
                periodOrder,
                focusPeriod))
            .Where(r => r.PeriodMaterialCosts.Count > 0)
            .ToList();

        var filtered = FilterComponentMovingPriceRows(allRows, queryDto.TrendFilter);
        var ordered = OrderComponentMovingPriceRows(filtered);
        return new ComponentMovingPriceAnalysisBuilt
        {
            OrderedRows = ordered,
            PeriodOrder = periodOrder,
            ProductCodes = productCodes,
            BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
            ComparePeriod = focusPeriod,
            UpCount = allRows.Count(r => r.Trend == "up"),
            DownCount = allRows.Count(r => r.Trend == "down"),
            FlatCount = allRows.Count(r => r.Trend == "flat"),
            NewCount = allRows.Count(r => r.Trend == "new"),
            RemovedCount = allRows.Count(r => r.Trend == "removed"),
            NoneCount = allRows.Count(r => r.Trend == "none"),
        };
    }

    /// <summary>
    /// BOM 成本推移（产品×月材料成本）内存构建结果
    /// </summary>
    private sealed class ComponentMovingPriceAnalysisBuilt
    {
        /// <summary>
        /// 过滤并排序后的全量行
        /// </summary>
        public List<TaktBomMaterialCostTrendComponentMovingPriceDto> OrderedRows { get; init; } = new();

        /// <summary>
        /// 期间列顺序
        /// </summary>
        public List<string> PeriodOrder { get; init; } = new();

        /// <summary>
        /// 机种下产品编码
        /// </summary>
        public List<string> ProductCodes { get; init; } = new();

        /// <summary>
        /// 基准期间
        /// </summary>
        public string? BasePeriod { get; init; }

        /// <summary>
        /// 对比期间
        /// </summary>
        public string? ComparePeriod { get; init; }

        /// <summary>
        /// 上涨行数（过滤前全量趋势统计）
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
        /// 关注月新增行数
        /// </summary>
        public int NewCount { get; init; }

        /// <summary>
        /// 关注月剔除行数
        /// </summary>
        public int RemovedCount { get; init; }

        /// <summary>
        /// 无趋势行数
        /// </summary>
        public int NoneCount { get; init; }

        /// <summary>
        /// 空结果
        /// </summary>
        /// <param name="productCodes">产品编码</param>
        /// <returns>空构建结果</returns>
        public static ComponentMovingPriceAnalysisBuilt Empty(List<string> productCodes) => new()
        {
            ProductCodes = productCodes,
        };
    }

    /// <summary>
    /// 机种产品元数据（机种/描述/币种）
    /// </summary>
    private sealed class ModelProductMeta
    {
        /// <summary>
        /// 机种编码
        /// </summary>
        public string ModelCode { get; set; } = string.Empty;

        /// <summary>
        /// 产品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 币种
        /// </summary>
        public string CurrencyCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// 从主表加载产品编码及描述/币种/机种（可按机种、核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种（空=工厂下全部机种产品）</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <param name="materialType">物料类型（本表 MaterialType；空=默认 FERT）</param>
    /// <returns>产品编码 → 元数据</returns>
    private async Task<Dictionary<string, ModelProductMeta>> LoadProductMetaAsync(
        string plantCode,
        string? modelCode = null,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null,
        string? materialType = null)
    {
        var type = NormalizeMaterialTypeFilter(materialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode);
        if (type != null)
        {
            var mt = type;
            exp = exp.And(x => x.MaterialType == mt);
        }
        if (!string.IsNullOrWhiteSpace(modelCode))
        {
            var model = modelCode.Trim();
            exp = exp.And(x => x.ModelCode == model);
        }
        if (costingMonthStart.HasValue)
        {
            var startPeriod = costingMonthStart.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(startPeriod) >= 0);
        }
        if (costingMonthEnd.HasValue)
        {
            var endPeriod = costingMonthEnd.Value.ToString("yyyy-MM");
            exp = exp.And(x => x.CostingPeriod != null && x.CostingPeriod.CompareTo(endPeriod) <= 0);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        var map = new Dictionary<string, ModelProductMeta>(StringComparer.OrdinalIgnoreCase);
        foreach (var header in headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .OrderByDescending(h => h.CostingDate)
            .ThenByDescending(h => h.Id))
        {
            var code = header.ProductCode.Trim();
            if (map.ContainsKey(code))
            {
                continue;
            }
            map[code] = new ModelProductMeta
            {
                ModelCode = header.ModelCode?.Trim() ?? string.Empty,
                Description = header.ProductDescription?.Trim() ?? string.Empty,
                CurrencyCode = header.CurrencyCode?.Trim() ?? string.Empty,
            };
        }
        return map;
    }

    /// <summary>
    /// 从主表加载机种下产品编码及描述/币种（可按核算月过滤；按条件全量，不截断）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="costingMonthStart">核算月起（月初，含；空=不限）</param>
    /// <param name="costingMonthEnd">核算月止（月初，含该月整月；空=不限）</param>
    /// <returns>产品编码 → 元数据</returns>
    private Task<Dictionary<string, ModelProductMeta>> LoadModelProductMetaAsync(
        string plantCode,
        string modelCode,
        DateTime? costingMonthStart = null,
        DateTime? costingMonthEnd = null)
    {
        return LoadProductMetaAsync(plantCode, modelCode, costingMonthStart, costingMonthEnd);
    }

    /// <summary>
    /// BOM 明细行键（对齐表唯一键，不含 CostingDate）
    /// </summary>
    /// <param name="item">明细行</param>
    /// <returns>稳定键</returns>
    private static string BuildBomLineTrendKey(TaktBomMaterialCostItem item)
    {
        return TaktBomMaterialCostItemLineCostHelper.BuildComponentKey(item);
    }

    /// <summary>
    /// 构建单个产品下 BOM 明细行 × 月材料成本（按 BuildBomLineTrendKey 跨月对齐）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCode">机种</param>
    /// <param name="productCode">产品</param>
    /// <param name="productDescription">产品描述</param>
    /// <param name="keyItems">同 BOM 行键明细（可含环比扩窗上月）</param>
    /// <param name="periodOrder">展示期间列</param>
    /// <param name="focusPeriod">关注月</param>
    /// <returns>产品组件成本推移行</returns>
    private static TaktBomMaterialCostTrendComponentMovingPriceDto BuildProductComponentMaterialCostTrendRow(
        string plantCode,
        string modelCode,
        string productCode,
        string productDescription,
        IReadOnlyList<TaktBomMaterialCostItem> keyItems,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        var identity = keyItems
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .First();
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var currencyCode = string.Empty;
        foreach (var period in periodOrder)
        {
            var monthCost = ResolveBomLineMaterialCostForPeriod(keyItems, period);
            if (monthCost == null)
            {
                continue;
            }
            periodCosts[period] = monthCost.Value;
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                var picked = keyItems
                    .Where(r => ToPeriodKey(r.CostingDate) == period)
                    .OrderByDescending(r => r.CostingDate)
                    .ThenByDescending(r => r.Id)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(picked?.MovingPriceCurrencyCode))
                {
                    currencyCode = picked.MovingPriceCurrencyCode.Trim();
                }
            }
        }

        // 环比基准月可在展示列之外（扩窗加载），写入字典供涨跌计算后再裁剪
        if (!string.IsNullOrWhiteSpace(focusPeriod)
            && DateTime.TryParseExact(
                focusPeriod.Trim() + "-01",
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
            if (!periodCosts.ContainsKey(basePeriod))
            {
                var baseCost = ResolveBomLineMaterialCostForPeriod(keyItems, basePeriod);
                if (baseCost != null)
                {
                    periodCosts[basePeriod] = baseCost.Value;
                }
            }
        }

        var description = productDescription;
        if (string.IsNullOrWhiteSpace(description))
        {
            description = keyItems
                .OrderByDescending(r => r.CostingDate)
                .Select(r => r.ProductDescription?.Trim())
                .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                ?? string.Empty;
        }

        var row = new TaktBomMaterialCostTrendComponentMovingPriceDto
        {
            PlantCode = plantCode,
            ModelCode = modelCode,
            ProductCode = productCode,
            ProductDescription = description,
            LineNumber = identity.LineNumber,
            BomLevel = identity.BomLevel?.Trim() ?? string.Empty,
            BomItemCode = identity.BomItemCode?.Trim() ?? string.Empty,
            ComponentCode = identity.ComponentCode?.Trim() ?? string.Empty,
            ComponentDescription = identity.ComponentDescription?.Trim() ?? string.Empty,
            ComponentQuantity = identity.ComponentQuantity,
            ProductionRelated = identity.ProductionRelated?.Trim(),
            PurchaseType = identity.PurchaseType?.Trim() ?? string.Empty,
            CurrencyCode = currencyCode,
            PeriodMaterialCosts = periodCosts,
            PeriodChangeTypes = BuildPeriodChangeTypes(periodOrder, periodCosts),
        };
        ApplyMaterialCostFocusTrend(row.PeriodMaterialCosts, focusPeriod, row);
        var displaySet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
        row.PeriodMaterialCosts = row.PeriodMaterialCosts
            .Where(kv => displaySet.Contains(kv.Key))
            .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);
        return row;
    }

    /// <summary>
    /// 按展示期间顺序生成各月存在/价格变动码（先区分有无物料，再对比价格）
    /// </summary>
    /// <param name="periodOrder">展示期间列 yyyy-MM</param>
    /// <param name="periodCosts">有数据的月材料成本</param>
    /// <returns>期间 → present / absent / new / removed / up / down / flat</returns>
    private static Dictionary<string, string> BuildPeriodChangeTypes(
        IReadOnlyList<string> periodOrder,
        IReadOnlyDictionary<string, decimal> periodCosts)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 0; i < periodOrder.Count; i++)
        {
            var period = periodOrder[i];
            var hasCurrent = periodCosts.ContainsKey(period);
            var hasPrevious = i > 0 && periodCosts.ContainsKey(periodOrder[i - 1]);
            if (!hasCurrent && !hasPrevious)
            {
                result[period] = "absent";
                continue;
            }
            if (!hasCurrent && hasPrevious)
            {
                result[period] = "removed";
                continue;
            }
            if (hasCurrent && !hasPrevious)
            {
                result[period] = i == 0 ? "present" : "new";
                continue;
            }
            var currentCost = periodCosts[period];
            var previousCost = periodCosts[periodOrder[i - 1]];
            if (currentCost > previousCost)
            {
                result[period] = "up";
            }
            else if (currentCost < previousCost)
            {
                result[period] = "down";
            }
            else
            {
                result[period] = "flat";
            }
        }
        return result;
    }

    /// <summary>
    /// 同 BOM 行键在某月取最新核算日组件移动单价：移动平均价÷移动价格单位（不乘组件数量）；无数据返回 null
    /// </summary>
    /// <param name="items">同键明细</param>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>组件单价（ResolvePerBaseUnitPrice）</returns>
    private static decimal? ResolveBomLineMaterialCostForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        string periodKey)
    {
        var picked = items
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey)
            .OrderByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        if (picked == null)
        {
            return null;
        }
        // 分析/推移「移动价格」口径：仅 MAP÷价格单位，不乘组件数量
        return TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(picked);
    }

    /// <summary>
    /// 将环比结果写入产品/组件推移行
    /// </summary>
    /// <param name="periodCosts">各月材料成本</param>
    /// <param name="focusPeriod">关注月</param>
    /// <param name="row">目标行</param>
    private static void ApplyMaterialCostFocusTrend(
        IReadOnlyDictionary<string, decimal> periodCosts,
        string? focusPeriod,
        TaktBomMaterialCostTrendComponentMovingPriceDto row)
    {
        ApplyUnitPriceFocusTrend(
            periodCosts,
            focusPeriod,
            out var trend,
            out var basePeriod,
            out var comparePeriod,
            out var varianceAmount,
            out var variancePercent);
        // 关注月有、基准月无 → 新增；关注月无、基准月有 → 剔除（先于价格涨跌）
        if (!string.IsNullOrWhiteSpace(comparePeriod) && !string.IsNullOrWhiteSpace(basePeriod))
        {
            var hasCompare = periodCosts.ContainsKey(comparePeriod);
            var hasBase = periodCosts.ContainsKey(basePeriod);
            if (hasCompare && !hasBase)
            {
                trend = "new";
                varianceAmount = periodCosts[comparePeriod];
                variancePercent = null;
            }
            else if (!hasCompare && hasBase)
            {
                trend = "removed";
                varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(-periodCosts[basePeriod]);
                variancePercent = null;
            }
        }
        row.Trend = trend;
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        row.VarianceAmount = varianceAmount;
        row.VariancePercent = variancePercent;
    }

    /// <summary>
    /// 核算日 → yyyy-MM
    /// </summary>
    /// <param name="costingDate">核算日</param>
    /// <returns>期间键</returns>
    private static string ToPeriodKey(DateTime costingDate)
        => new DateTime(costingDate.Year, costingDate.Month, 1).ToString("yyyy-MM");

    /// <summary>
    /// 某月内按 BOM 行键去重后汇总组件移动单价（同键取最新一行；单价=MAP÷价格单位，不乘数量）
    /// </summary>
    /// <param name="items">明细（产品或合并键子集）</param>
    /// <param name="periodKey">yyyy-MM</param>
    /// <returns>有数据时返回单价合计，否则 null</returns>
    private static decimal? SumMaterialCostByLineKeyForPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        string periodKey)
    {
        var periodRows = items.Where(r => ToPeriodKey(r.CostingDate) == periodKey).ToList();
        if (periodRows.Count == 0)
        {
            return null;
        }
        var picked = periodRows
            .GroupBy(BuildBomLineTrendKey, StringComparer.Ordinal)
            .Select(g => g
                .OrderByDescending(r => r.CostingDate)
                .ThenByDescending(r => r.Id)
                .First())
            .ToList();
        return TaktBomMaterialCostItemLineCostHelper.RoundCost(
            picked.Sum(TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice));
    }

    /// <summary>
    /// 按涨跌筛选过滤产品行
    /// </summary>
    /// <param name="rows">全量行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomMaterialCostTrendComponentMovingPriceDto> FilterComponentMovingPriceRows(
        IReadOnlyList<TaktBomMaterialCostTrendComponentMovingPriceDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down" or "new" or "removed").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 产品成本明细全量排序（分页前）：ProductCode 升序，再 LineNumber 升序
    /// </summary>
    /// <param name="rows">已筛选行</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomMaterialCostTrendComponentMovingPriceDto> OrderComponentMovingPriceRows(
        IReadOnlyList<TaktBomMaterialCostTrendComponentMovingPriceDto> rows)
    {
        return rows
            .OrderBy(r => r, Comparer<TaktBomMaterialCostTrendComponentMovingPriceDto>.Create(
                (a, b) => TaktBomMaterialCostItemLineCostHelper.CompareProductCodeThenLineNumber(
                    a.ProductCode,
                    a.LineNumber,
                    b.ProductCode,
                    b.LineNumber)))
            .ThenBy(r => r.BomItemCode, StringComparer.Ordinal)
            .ThenBy(r => r.ComponentCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 产品组件移动价行全量合计
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumComponentMovingPriceRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostTrendComponentMovingPriceDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodMaterialCosts, r.VarianceAmount)));
    }

    /// <summary>
    /// 机种分析行全量合计
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumPeriodAndVarianceGrandTotals(
        IReadOnlyList<string> periodOrder,
        IEnumerable<(IReadOnlyDictionary<string, decimal> PeriodMap, decimal? VarianceAmount)> rows)
    {
        var rowList = rows as IReadOnlyList<(IReadOnlyDictionary<string, decimal> PeriodMap, decimal? VarianceAmount)>
            ?? rows.ToList();
        var periodCostTotals = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            decimal sum = 0m;
            var hasValue = false;
            foreach (var (periodMap, _) in rowList)
            {
                if (!periodMap.TryGetValue(period, out var value))
                {
                    continue;
                }
                sum += value;
                hasValue = true;
            }
            if (hasValue)
            {
                periodCostTotals[period] = TaktBomMaterialCostItemLineCostHelper.RoundCost(sum);
            }
        }
        decimal varianceSum = 0m;
        var hasVariance = false;
        foreach (var (_, varianceAmount) in rowList)
        {
            if (varianceAmount == null)
            {
                continue;
            }
            varianceSum += varianceAmount.Value;
            hasVariance = true;
        }
        decimal? varianceAmountTotal = hasVariance
            ? TaktBomMaterialCostItemLineCostHelper.RoundCost(varianceSum)
            : null;
        return (periodCostTotals, varianceAmountTotal);
    }

    // ========================================
    // 按年分表路由（{base}_{yyyy}）
    // ========================================

    /// <summary>
    /// 生成 BOM 成本明细年分表名
    /// </summary>
    /// <param name="year">年份</param>
    /// <returns>物理表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
 /// 解析 BOM 成本明细物理表：年分表存在则用之，否则 null（回退实体基表，兼容 同步）
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
 // 年分表与基表合并： 同步常写基表，年分表可能仅部分数据；按 Id 去重
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
    /// <summary>
    /// 按关注期间对材料成本字典应用环比涨跌
    /// </summary>
    /// <param name="periodUnitPrices">各月材料成本</param>
    /// <param name="focusPeriod">关注期间</param>
    /// <param name="trend">涨跌</param>
    /// <param name="basePeriod">基准月</param>
    /// <param name="comparePeriod">对比月</param>
    /// <param name="varianceAmount">差额</param>
    /// <param name="variancePercent">变动率（百分点）</param>
    private static void ApplyUnitPriceFocusTrend(
        IReadOnlyDictionary<string, decimal> periodUnitPrices,
        string? focusPeriod,
        out string trend,
        out string? basePeriod,
        out string? comparePeriod,
        out decimal? varianceAmount,
        out decimal? variancePercent)
    {
        trend = "none";
        basePeriod = null;
        comparePeriod = null;
        varianceAmount = null;
        variancePercent = null;
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(
                comparePeriod + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var compareMonth))
        {
            return;
        }
        basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        if (!periodUnitPrices.TryGetValue(basePeriod, out var basePrice)
            || !periodUnitPrices.TryGetValue(comparePeriod, out var comparePrice))
        {
            return;
        }
        varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(comparePrice - basePrice);
        if (basePrice != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                varianceAmount.Value / basePrice);
        }
        if (comparePrice > basePrice)
        {
            trend = "up";
        }
        else if (comparePrice < basePrice)
        {
            trend = "down";
        }
        else
        {
            trend = "flat";
        }
    }


}

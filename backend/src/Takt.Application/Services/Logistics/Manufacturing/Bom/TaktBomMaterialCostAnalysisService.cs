// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialCostAnalysisService.cs
// 创建时间：2026-07-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本分析应用服务（转置 / 差异；月度涨跌见 TaktBomMaterialCostAnalysisTrendService）
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using OfficeOpenXml;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 成本分析服务（转置 / 差异 / 月度涨跌）
/// </summary>
public class TaktBomMaterialCostAnalysisService : TaktServiceBase, ITaktBomMaterialCostAnalysisService
{
    /// <summary>
    /// BOM 成本明细按年分表基表名（与 SugarTable 一致）
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";
    /// <summary>
    /// BOM 成本明细仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    /// <summary>
    /// BOM 成本汇总仓储（转置/涨跌分析数据源）
    /// </summary>
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    /// <summary>
    /// 型号目的地仓储（机种名称）
    /// </summary>
    private readonly ITaktTenantRepository<TaktModelDestination> _modelDestinationRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 物料成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 物料成本汇总仓储（转置/涨跌分析数据源）</param>
    /// <param name="modelDestinationRepository">型号目的地仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomMaterialCostAnalysisService(
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
    /// 规范化物料类型筛选（空=不按类型过滤）
    /// </summary>
    /// <param name="materialType">查询传入类型</param>
    /// <returns>非空类型码；空则 null</returns>
    private static string? NormalizeMaterialTypeFilter(string? materialType)
    {
        var trimmed = materialType?.Trim();
        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    // ========================================
    // 转置 / 差异 / 月度涨跌分析
    // ========================================

    /// <summary>
    /// 获取成本分析转置列表（产品 × 核算月成本矩阵 + 环比涨跌）
    /// <para>有 FocusPeriod 时加载区间向前扩 1 个月取上月成本；展示列仍用原核算期间。支持 TrendFilter 与分页。</para>
    /// </summary>
    /// <param name="queryDto">转置查询（工厂、期间、可选机种/产品/物料类型/涨跌筛选）</param>
    /// <returns>分页转置行、期间列顺序、可选机种汇总、期间合计与环比差额合计</returns>
    public async Task<TaktBomMaterialCostAnalysisTransposedResultDto> GetBomMaterialCostAnalysisTransposedListAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var full = await BuildBomMaterialCostAnalysisTransposedAsync(queryDto);
        var total = full.Paged.Total;
        var transposedRows = full.Paged.Data.Skip(skip).Take(pageSize).ToList();
        return new TaktBomMaterialCostAnalysisTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostAnalysisTransposedDto>.Create(
                transposedRows, total, pageIndex, pageSize),
            PeriodOrder = full.PeriodOrder,
            ModelSummary = full.ModelSummary,
            PeriodCostTotals = full.PeriodCostTotals,
            VarianceAmountTotal = full.VarianceAmountTotal,
        };
    }

    /// <summary>
    /// 构建转置全量结果（筛选命中的全部产品行，不分页、不截断）
    /// </summary>
    /// <param name="queryDto">转置查询</param>
    /// <returns>全量转置结果（Paged.Data 为全部行）</returns>
    private async Task<TaktBomMaterialCostAnalysisTransposedResultDto> BuildBomMaterialCostAnalysisTransposedAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        var focusPeriod = string.IsNullOrWhiteSpace(queryDto.FocusPeriod) ? null : queryDto.FocusPeriod.Trim();
        var plantCode = queryDto.PlantCode?.Trim() ?? string.Empty;
        var modelCode = queryDto.ModelCode?.Trim();
        var loadQuery = CloneTransposedQueryForMomLoad(queryDto, focusPeriod);
        var rows = await LoadTransposedCostHeadersAsync(loadQuery);
        var periodOrder = BuildCostHeaderPeriodOrder(
            rows,
            queryDto.CostingDateStart,
            queryDto.CostingDateEnd,
            includeExtraPeriodsFromData: false);
        var displayPeriodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
 // 目录键：归一化产品码（10/18 位 互认），避免同一产品拆成多行
        var productGroups = rows
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductCatalogKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        // 展示区间内任一核算月有成本即进目录（默认近 3 个月）；环比扩窗上月仅用于取数，不在 periodOrder 内故不会进目录
        var productCatalog = productGroups
            .Where(kv => kv.Value.Any(r =>
                !string.IsNullOrWhiteSpace(r.CostingPeriod)
                && displayPeriodSet.Contains(r.CostingPeriod!)))
            .Select(kv =>
            {
                var latest = kv.Value.OrderByDescending(r => r.CostingDate).FirstOrDefault();
                var code = latest?.ProductCode?.Trim() ?? kv.Key;
                return (
                    ProductCode: code,
                    ProductDescription: latest?.ProductDescription?.Trim() ?? string.Empty);
            })
            .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
            .ToList();
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var trimmedProduct = queryDto.ProductCode.Trim();
            productCatalog = productCatalog
                .Where(p => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(p.ProductCode, trimmedProduct))
                .ToList();
        }
        var allTransposedRows = productCatalog
            .Select(item => BuildTransposedRowForCatalogItem(
                plantCode,
                item.ProductCode,
                item.ProductDescription,
                productGroups,
                periodOrder,
                focusPeriod))
            .ToList();
        TaktBomMaterialCostAnalysisModelSummaryDto? modelSummary = null;
        if (!string.IsNullOrWhiteSpace(modelCode) && string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var modelNameLookup = await BuildModelNameLookupAsync();
            modelNameLookup.TryGetValue(modelCode, out var modelName);
            modelSummary = BuildModelSummary(modelCode, modelName ?? modelCode, allTransposedRows, periodOrder);
        }
        var filteredRows = FilterTransposedRowsByTrend(allTransposedRows, queryDto.TrendFilter);
        var orderedRows = OrderTransposedRows(filteredRows, queryDto.SortBy);
        var total = orderedRows.Count;
        var (periodCostTotals, varianceAmountTotal) = SumTransposedRowGrandTotals(orderedRows, periodOrder);
        return new TaktBomMaterialCostAnalysisTransposedResultDto
        {
            Paged = TaktPagedResult<TaktBomMaterialCostAnalysisTransposedDto>.Create(
                orderedRows, total, 1, Math.Max(1, total)),
            PeriodOrder = periodOrder,
            ModelSummary = modelSummary,
            PeriodCostTotals = periodCostTotals,
            VarianceAmountTotal = varianceAmountTotal,
        };
    }

    /// <summary>
    /// 导出成本分析转置 Excel（机种/产品/品名 + 各月成本 + 涨跌/环比）
    /// <para>导出筛选命中的全部转置行，不分页、不截断。</para>
    /// </summary>
    /// <param name="query">查询条件；空则用默认查询 DTO</param>
    /// <param name="sheetName">工作表名称；空则默认「DTA BOM成本推移表」</param>
    /// <param name="fileName">导出文件名；空则默认 xlsx 文件名</param>
    /// <returns>实际文件名与文件字节</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisTransposedAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        query ??= new TaktBomMaterialCostAnalysisTransposedQueryDto();
        EnsureThreeLayerContext();
        var result = await BuildBomMaterialCostAnalysisTransposedAsync(query);
        var periodOrder = result.PeriodOrder;
        var columnKeys = new List<string> { "modelCode", "productCode", "productDescription" };
        var columnLabels = new List<string> { "机种", "产品", "品名" };
        foreach (var period in periodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.Add("trend");
        columnLabels.Add("涨跌");
        columnKeys.Add("varianceAmount");
        columnLabels.Add("环比差额");
        columnKeys.Add("variancePercent");
        columnLabels.Add("环比%");
        var exportRows = result.Paged.Data.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["modelCode"] = row.ModelCode,
                ["productCode"] = row.ProductCode,
                ["productDescription"] = row.ProductDescription,
                ["trend"] = row.Trend,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(row.VariancePercent),
            };
            foreach (var period in periodOrder)
            {
                row.PeriodCosts.TryGetValue(period, out var cost);
                dict[$"period_{period}"] = cost;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA BOM成本推移表",
            fileName ?? "DTA BOM成本推移表.xlsx");
    }

    /// <summary>
    /// 获取成本分析差异（单产品两核算月组件级成本/单价/数量对比）
    /// <para>须 PlantCode、ProductCode、BasePeriod、ComparePeriod；按组件键合并基准/对比快照并计算变动类型。</para>
    /// </summary>
    /// <param name="queryDto">差异查询 DTO</param>
    /// <returns>汇总总成本、总差异及按差异绝对值排序的组件明细行</returns>
    public async Task<TaktBomMaterialCostAnalysisVarianceResultDto> GetBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ProductCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.BasePeriod);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.ComparePeriod);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var productCode = queryDto.ProductCode.Trim();
        var basePeriod = queryDto.BasePeriod.Trim();
        var comparePeriod = queryDto.ComparePeriod.Trim();
        var (baseStart, baseEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(basePeriod);
        var (compareStart, compareEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(comparePeriod);
        var rangeStart = baseStart < compareStart ? baseStart : compareStart;
        var rangeEnd = baseEnd > compareEnd ? baseEnd : compareEnd;
        var rows = await LoadVarianceItemRowsAsync(plantCode, productCode, rangeStart, rangeEnd);
        var baseSnapshotRaw = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(rows, plantCode, productCode, basePeriod);
        var compareSnapshotRaw = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(rows, plantCode, productCode, comparePeriod);
        // 全量快照上按参与资格 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F），保证差异明细与合计口径一致
        var baseSnapshot = TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(baseSnapshotRaw))
            .ToList();
        var compareSnapshot = TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(compareSnapshotRaw))
            .ToList();
        var productDescription = compareSnapshotRaw.FirstOrDefault()?.ProductDescription
            ?? baseSnapshotRaw.FirstOrDefault()?.ProductDescription
            ?? string.Empty;
        var baseMap = baseSnapshot.ToDictionary(TaktBomMaterialCostItemLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var compareMap = compareSnapshot.ToDictionary(TaktBomMaterialCostItemLineCostHelper.BuildComponentKey, StringComparer.Ordinal);
        var componentKeys = baseMap.Keys.Union(compareMap.Keys, StringComparer.Ordinal).ToList();
        var lines = componentKeys
            .Select(key =>
            {
                baseMap.TryGetValue(key, out var baseRow);
                compareMap.TryGetValue(key, out var compareRow);
                return BuildVarianceLine(baseRow, compareRow);
            })
            .OrderByDescending(l => Math.Abs(l.VarianceAmount))
            .ToList();
        // base/compare 已是参与资格 Filter 后的行，直接合计
        var baseTotal = TaktBomMaterialCostItemLineCostHelper.RoundCost(
            baseSnapshot.Sum(TaktBomMaterialCostItemLineCostHelper.CalculateLineCost));
        var compareTotal = TaktBomMaterialCostItemLineCostHelper.RoundCost(
            compareSnapshot.Sum(TaktBomMaterialCostItemLineCostHelper.CalculateLineCost));
        return new TaktBomMaterialCostAnalysisVarianceResultDto
        {
            PlantCode = plantCode,
            ProductCode = productCode,
            ProductDescription = productDescription,
            BasePeriod = basePeriod,
            ComparePeriod = comparePeriod,
            BaseTotalCost = baseTotal,
            CompareTotalCost = compareTotal,
            TotalVariance = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareTotal - baseTotal),
            Lines = lines,
        };
    }

    /// <summary>
    /// 导出成本分析差异 Excel（「汇总」+「差异明细」双工作表）
    /// </summary>
    /// <param name="query">差异查询条件（与 Get 一致，必填工厂/产品/基准与对比期间）</param>
    /// <param name="sheetName">明细工作表名；空则「差异明细」</param>
    /// <param name="fileName">导出文件名；空则默认 xlsx</param>
    /// <returns>实际文件名与文件字节</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomMaterialCostAnalysisVarianceAnalysisAsync(
        TaktBomMaterialCostAnalysisVarianceQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await GetBomMaterialCostAnalysisVarianceAnalysisAsync(query);
        var summaryRows = new List<IReadOnlyDictionary<string, object?>>
        {
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "工厂代码", ["value"] = result.PlantCode },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "产品编码", ["value"] = result.ProductCode },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "产品描述", ["value"] = result.ProductDescription },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "基准期间", ["value"] = result.BasePeriod },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "对比期间", ["value"] = result.ComparePeriod },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "基准总成本", ["value"] = result.BaseTotalCost },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "对比总成本", ["value"] = result.CompareTotalCost },
            new Dictionary<string, object?>(StringComparer.Ordinal) { ["field"] = "总差异", ["value"] = result.TotalVariance },
        };
        var detailKeys = new[]
        {
            "bomItemCode", "componentCode", "componentDescription", "purchaseType", "currencyCode",
            "baseCost", "compareCost", "varianceAmount", "variancePercent",
            "baseUnitPrice", "compareUnitPrice", "unitPriceVariance",
            "baseQuantity", "compareQuantity", "quantityVariance",
            "priceEffectAmount", "quantityEffectAmount", "changeType",
        };
        var detailLabels = new[]
        {
            "BOM项目号", "组件编码", "组件描述", "采购类型", "货币",
            "基准成本", "对比成本", "成本差异", "差异率%",
            "基准单价", "对比单价", "单价差异",
            "基准数量", "对比数量", "数量差异",
            "价格影响额", "数量影响额", "变动类型",
        };
        var detailRows = result.Lines.Select(line => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["bomItemCode"] = line.BomItemCode,
            ["componentCode"] = line.ComponentCode,
            ["componentDescription"] = line.ComponentDescription,
            ["purchaseType"] = line.PurchaseType,
            ["currencyCode"] = line.CurrencyCode,
            ["baseCost"] = line.BaseCost,
            ["compareCost"] = line.CompareCost,
            ["varianceAmount"] = line.VarianceAmount,
            ["variancePercent"] = TaktBomMaterialCostItemLineCostHelper.ToExcelPercent(line.VariancePercent),
            ["baseUnitPrice"] = line.BaseUnitPrice,
            ["compareUnitPrice"] = line.CompareUnitPrice,
            ["unitPriceVariance"] = line.UnitPriceVariance,
            ["baseQuantity"] = line.BaseQuantity,
            ["compareQuantity"] = line.CompareQuantity,
            ["quantityVariance"] = line.QuantityVariance,
            ["priceEffectAmount"] = line.PriceEffectAmount,
            ["quantityEffectAmount"] = line.QuantityEffectAmount,
            ["changeType"] = line.ChangeType,
        }).ToList();
        using var package = new ExcelPackage();
        var summarySheet = package.Workbook.Worksheets.Add("汇总");
        summarySheet.Cells[1, 1].LoadFromArrays(new[] { new[] { "字段", "值" } });
        summarySheet.Cells[2, 1].LoadFromArrays(summaryRows.Select(r => new object[] { r["field"]!, r["value"]! }).ToArray());
        var detailSheet = package.Workbook.Worksheets.Add(sheetName ?? "差异明细");
        detailSheet.Cells[1, 1].LoadFromArrays(new[] { detailLabels });
        if (detailRows.Count > 0)
        {
            var dataArray = detailRows
                .Select(row => detailKeys.Select(k => row.TryGetValue(k, out var v) ? v ?? DBNull.Value : DBNull.Value).ToArray())
                .ToList();
            detailSheet.Cells[2, 1].LoadFromArrays(dataArray);
        }
        // 列宽：固定 Excel 标准列宽（不按内容 AutoFit）
        if (summarySheet.Dimension != null)
        {
            for (var i = 1; i <= summarySheet.Dimension.End.Column; i++)
            {
                summarySheet.Column(i).Width = TaktExcelHelper.ExcelStandardColumnWidth;
            }
        }
        if (detailSheet.Dimension != null)
        {
            for (var i = 1; i <= detailSheet.Dimension.End.Column; i++)
            {
                detailSheet.Column(i).Width = TaktExcelHelper.ExcelStandardColumnWidth;
            }
        }
        var actualFileName = fileName ?? "BOM材料成本差异分析.xlsx";
        var content = await package.GetAsByteArrayAsync();
        return (actualFileName, content);
    }

    /// <inheritdoc />
    public Task<List<TaktBomMaterialCost>> LoadBomMaterialCostAnalysisHeadersAsync(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
        => LoadTransposedCostHeadersAsync(queryDto);

    /// <summary>
    /// 加载转置/月度涨跌用成本汇总行（按筛选全量；MaterialType 有值才过滤；空=本表全类型）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>成本汇总行</returns>
    private async Task<List<TaktBomMaterialCost>> LoadTransposedCostHeadersAsync(TaktBomMaterialCostAnalysisTransposedQueryDto queryDto)
    {
        var materialType = NormalizeMaterialTypeFilter(queryDto.MaterialType);
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode);
        if (materialType != null)
        {
            var type = materialType;
            exp = exp.And(x => x.MaterialType == type);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            var plantCode = queryDto.PlantCode.Trim();
            exp = exp.And(x => x.PlantCode == plantCode);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var modelCode = queryDto.ModelCode.Trim();
            exp = exp.And(x => x.ModelCode == modelCode);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productCode = queryDto.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(productCode));
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords.Trim();
            exp = exp.And(x =>
                (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductDescription != null && x.ProductDescription.Contains(keywords)));
        }
        if (queryDto.CostingDateStart.HasValue)
        {
            exp = exp.And(x => x.CostingDate >= queryDto.CostingDateStart);
        }
        if (queryDto.CostingDateEnd.HasValue)
        {
            exp = exp.And(x => x.CostingDate <= queryDto.CostingDateEnd);
        }
        var rows = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productCode = queryDto.ProductCode.Trim();
            rows = rows
                .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode))
                .ToList();
        }
        return rows;
    }

    /// <summary>
    /// 加载差异分析用明细行
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="rangeStart">核算日起</param>
    /// <param name="rangeEnd">核算日止</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadVarianceItemRowsAsync(
        string plantCode,
        string productCode,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var normalizedProduct = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode);
        Expression<Func<TaktBomMaterialCostItem, bool>> predicate = x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.CostingDate >= rangeStart
            && x.CostingDate <= rangeEnd
            && (x.ProductCode == productCode
                || x.ProductCode == normalizedProduct
                || (x.ProductCode != null && x.ProductCode.Contains(productCode)));
        var rows = await GetBomItemListForRangeAsync(predicate, rangeStart, rangeEnd);
        return rows
            .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode))
            .ToList();
    }

    /// <summary>
    /// 构建机种名称查找表（型号目的地 ModelCode → ModelName，仅展示名）
    /// </summary>
    /// <returns>机种名称字典</returns>
    private async Task<Dictionary<string, string>> BuildModelNameLookupAsync()
    {
        var list = await _modelDestinationRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode);
        return list
            .Where(x => !string.IsNullOrWhiteSpace(x.ModelCode))
            .GroupBy(x => x.ModelCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.First().ModelName?.Trim() ?? g.Key,
                StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
 /// 转置产品目录分组键
    /// </summary>
    /// <param name="productCode">产品编码</param>
    /// <returns>分组键</returns>
    private static string NormalizeProductCatalogKey(string productCode)
    {
        var trimmed = productCode.Trim();
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        return string.IsNullOrEmpty(normalized) ? trimmed : normalized;
    }

    /// <summary>
    /// 克隆转置查询并将起始日提前一月，便于环比取上月成本（不改变展示用 CostingDateStart）
    /// </summary>
    /// <param name="queryDto">原查询</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <returns>加载用查询</returns>
    private static TaktBomMaterialCostAnalysisTransposedQueryDto CloneTransposedQueryForMomLoad(
        TaktBomMaterialCostAnalysisTransposedQueryDto queryDto,
        string? focusPeriod)
    {
        var loadQuery = new TaktBomMaterialCostAnalysisTransposedQueryDto
        {
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
            KeyWords = queryDto.KeyWords,
            PlantCode = queryDto.PlantCode,
            ModelCode = queryDto.ModelCode,
            ProductCode = queryDto.ProductCode,
            MaterialType = queryDto.MaterialType,
            CostingDateStart = queryDto.CostingDateStart,
            CostingDateEnd = queryDto.CostingDateEnd,
            FocusPeriod = queryDto.FocusPeriod,
            TrendFilter = queryDto.TrendFilter,
            SortBy = queryDto.SortBy,
        };
        if (string.IsNullOrWhiteSpace(focusPeriod)
            || !DateTime.TryParseExact(
                focusPeriod.Trim() + "-01",
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var focusMonth))
        {
            return loadQuery;
        }
        var momStart = new DateTime(focusMonth.Year, focusMonth.Month, 1).AddMonths(-1);
        if (!loadQuery.CostingDateStart.HasValue || loadQuery.CostingDateStart.Value > momStart)
        {
            loadQuery.CostingDateStart = momStart;
        }
        return loadQuery;
    }

    /// <summary>
    /// 构建成本汇总行的期间列顺序（yyyy-MM）；供月度涨跌独立服务复用
    /// </summary>
    /// <param name="rows">已加载汇总行</param>
    /// <param name="start">核算日起（可空）</param>
    /// <param name="end">核算日止（可空）</param>
    /// <param name="includeExtraPeriodsFromData">起止都有值时，是否把数据中落在区间外的期间并入并排序</param>
    /// <returns>升序期间键列表；无起止则仅用数据中出现的期间</returns>
    internal static List<string> BuildCostHeaderPeriodOrder(
        IReadOnlyList<TaktBomMaterialCost> rows,
        DateTime? start,
        DateTime? end,
        bool includeExtraPeriodsFromData = true)
    {
        var periods = rows
            .Select(r => r.CostingPeriod)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList();
        if (start.HasValue && end.HasValue && start.Value <= end.Value)
        {
            var cursor = new DateTime(start.Value.Year, start.Value.Month, 1);
            var endMonth = new DateTime(end.Value.Year, end.Value.Month, 1);
            var rangePeriods = new List<string>();
            while (cursor <= endMonth)
            {
                rangePeriods.Add(TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(cursor));
                cursor = cursor.AddMonths(1);
            }
            if (includeExtraPeriodsFromData)
            {
                foreach (var period in periods)
                {
                    if (!rangePeriods.Contains(period, StringComparer.Ordinal))
                    {
                        rangePeriods.Add(period);
                    }
                }
                rangePeriods.Sort(StringComparer.Ordinal);
            }
            return rangePeriods;
        }
        return periods;
    }

    /// <summary>
    /// 按主表产品目录项构建转置行（PeriodCosts 取自 TaktBomMaterialCost.ProductMonthlyCalculation）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="catalogProductCode">主表产品编码</param>
    /// <param name="catalogDescription">主表产品描述</param>
    /// <param name="productGroups">主表按产品编码分组</param>
    /// <param name="periodOrder">期间列</param>
    /// <param name="focusPeriod">关注期间（可选）</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostAnalysisTransposedDto BuildTransposedRowForCatalogItem(
        string plantCode,
        string catalogProductCode,
        string catalogDescription,
        IReadOnlyDictionary<string, List<TaktBomMaterialCost>> productGroups,
        IReadOnlyList<string> periodOrder,
        string? focusPeriod)
    {
        List<TaktBomMaterialCost>? matchedRows = null;
        foreach (var (productCode, rows) in productGroups)
        {
            if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(productCode, catalogProductCode))
            {
                continue;
            }
            matchedRows = rows;
            break;
        }
        if (matchedRows == null || matchedRows.Count == 0)
        {
            return new TaktBomMaterialCostAnalysisTransposedDto
            {
                PlantCode = plantCode,
                ModelCode = string.Empty,
                ProductCode = catalogProductCode,
                ProductDescription = catalogDescription,
                PeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal),
                Trend = "none",
            };
        }
        var description = matchedRows.OrderByDescending(r => r.CostingDate).FirstOrDefault()?.ProductDescription?.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            description = catalogDescription;
        }
        var row = BuildTransposedRowFromCostHeaders(
            plantCode,
            catalogProductCode,
            description ?? catalogProductCode,
            matchedRows,
            periodOrder);
        ApplyFocusPeriodTrend(row, focusPeriod, matchedRows);
        return row;
    }

    /// <summary>
    /// 构建单产品转置行（汇总表 ProductMonthlyCalculation）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="productDescription">产品描述</param>
    /// <param name="productRows">产品汇总行</param>
    /// <param name="periodOrder">期间顺序</param>
    /// <returns>转置行</returns>
    private static TaktBomMaterialCostAnalysisTransposedDto BuildTransposedRowFromCostHeaders(
        string plantCode,
        string productCode,
        string productDescription,
        List<TaktBomMaterialCost> productRows,
        IReadOnlyList<string> periodOrder)
    {
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var header = productRows
                .Where(r => string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
                .OrderByDescending(r => r.CostingDate)
                .FirstOrDefault();
            if (header != null && header.ProductMonthlyCalculation > 0m)
            {
                periodCosts[period] = header.ProductMonthlyCalculation;
            }
        }
        var latest = productRows.OrderByDescending(r => r.CostingDate).FirstOrDefault();
        var resolvedPlant = !string.IsNullOrWhiteSpace(plantCode)
            ? plantCode
            : latest?.PlantCode ?? string.Empty;
        return new TaktBomMaterialCostAnalysisTransposedDto
        {
            PlantCode = resolvedPlant,
            ModelCode = latest?.ModelCode?.Trim() ?? string.Empty,
            ProductCode = productCode,
            ProductDescription = productDescription,
            PeriodCosts = periodCosts,
            CurrencyCode = latest?.CurrencyCode ?? string.Empty,
            Trend = "none",
        };
    }

    /// <summary>
    /// 按 FocusPeriod 填充单行环比字段（上月成本可来自加载扩窗，不必出现在展示列）
    /// </summary>
    /// <param name="row">转置行</param>
    /// <param name="focusPeriod">关注期间 yyyy-MM</param>
    /// <param name="productRows">该产品主表行（含扩窗上月）</param>
    private static void ApplyFocusPeriodTrend(
        TaktBomMaterialCostAnalysisTransposedDto row,
        string? focusPeriod,
        IReadOnlyList<TaktBomMaterialCost> productRows)
    {
        if (string.IsNullOrWhiteSpace(focusPeriod))
        {
            return;
        }
        var comparePeriod = focusPeriod.Trim();
        if (!DateTime.TryParseExact(comparePeriod + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var compareMonth))
        {
            return;
        }
        var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
        row.BasePeriod = basePeriod;
        row.ComparePeriod = comparePeriod;
        if (!TryResolveProductMonthlyCost(productRows, basePeriod, out var baseCost)
            || !TryResolveProductMonthlyCost(productRows, comparePeriod, out var compareCost))
        {
            row.Trend = "none";
            return;
        }
        row.VarianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
        if (baseCost != 0m)
        {
            row.VariancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                row.VarianceAmount.Value / baseCost);
        }
        if (compareCost > baseCost)
        {
            row.Trend = "up";
        }
        else if (compareCost < baseCost)
        {
            row.Trend = "down";
        }
        else
        {
            row.Trend = "flat";
        }
    }

    /// <summary>
    /// 按涨跌筛选过滤转置行
    /// </summary>
    /// <param name="rows">转置行</param>
    /// <param name="trendFilter">筛选码</param>
    /// <returns>过滤后列表</returns>
    private static List<TaktBomMaterialCostAnalysisTransposedDto> FilterTransposedRowsByTrend(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        string? trendFilter)
    {
        if (string.IsNullOrWhiteSpace(trendFilter))
        {
            return rows.ToList();
        }
        var filter = trendFilter.Trim().ToLowerInvariant();
        if (filter == "changed")
        {
            return rows.Where(r => r.Trend is "up" or "down").ToList();
        }
        return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// 转置分析行全量排序（分页前）：productCode（默认）/ trend / varianceDesc
    /// </summary>
    /// <param name="rows">已筛选行</param>
    /// <param name="sortBy">排序码</param>
    /// <returns>排序后列表</returns>
    private static List<TaktBomMaterialCostAnalysisTransposedDto> OrderTransposedRows(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        string? sortBy)
    {
        static int TrendRank(string? trend) => trend switch
        {
            "up" => 0,
            "down" => 1,
            "flat" => 2,
            _ => 3,
        };
        var mode = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        IOrderedEnumerable<TaktBomMaterialCostAnalysisTransposedDto> ordered = mode switch
        {
            "trend" => rows
                .OrderBy(r => TrendRank(r.Trend))
                .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m)),
            "variancedesc" => rows
                .OrderByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
                .ThenBy(r => TrendRank(r.Trend)),
            _ => rows.OrderBy(r => r.ProductCode, StringComparer.Ordinal), // productCode 默认
        };
        return ordered
            .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
            .ThenBy(r => r.ProductCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 解析某产品某月 ProductMonthlyCalculation（取该月最新核算日）
    /// </summary>
    /// <param name="productRows">产品主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <param name="cost">成本</param>
    /// <returns>是否存在该月行</returns>
    private static bool TryResolveProductMonthlyCost(
        IReadOnlyList<TaktBomMaterialCost> productRows,
        string period,
        out decimal cost)
    {
        cost = 0m;
        var header = productRows
            .Where(r => string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault();
        if (header == null)
        {
            return false;
        }
        cost = header.ProductMonthlyCalculation;
        return true;
    }

    /// <summary>
    /// 构建机种汇总（各月成品平均成本）
    /// </summary>
    /// <param name="modelCode">机种编码</param>
    /// <param name="modelName">机种名称</param>
    /// <param name="rows">全部转置行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>机种汇总</returns>
    private static TaktBomMaterialCostAnalysisModelSummaryDto BuildModelSummary(
        string modelCode,
        string modelName,
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        var averagePeriodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            var costs = rows
                .Where(r => r.PeriodCosts.TryGetValue(period, out var cost) && cost > 0m)
                .Select(r => r.PeriodCosts[period])
                .ToList();
            if (costs.Count > 0)
            {
                averagePeriodCosts[period] = TaktBomMaterialCostItemLineCostHelper.RoundCost(costs.Sum() / costs.Count);
            }
        }
        return new TaktBomMaterialCostAnalysisModelSummaryDto
        {
            ModelCode = modelCode,
            ModelName = modelName,
            ProductCount = rows.Count,
            AveragePeriodCosts = averagePeriodCosts,
        };
    }

    /// <summary>
    /// 构建组件差异行
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <returns>差异行 DTO</returns>
    private static TaktBomMaterialCostAnalysisVarianceLineDto BuildVarianceLine(
        TaktBomMaterialCostItem? baseRow,
        TaktBomMaterialCostItem? compareRow)
    {
        var baseUnitPrice = baseRow != null
            ? TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(baseRow)
            : 0m;
        var compareUnitPrice = compareRow != null
            ? TaktBomMaterialCostItemLineCostHelper.ResolvePerBaseUnitPrice(compareRow)
            : 0m;
        var baseQty = baseRow?.ComponentQuantity ?? 0m;
        var compareQty = compareRow?.ComponentQuantity ?? 0m;
        // 移动价格口径：单价=MAP÷价格单位（已除单位）；行金额=数量×单价
        var baseCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(baseQty * baseUnitPrice);
        var compareCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareQty * compareUnitPrice);
        var unitPriceVariance = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareUnitPrice - baseUnitPrice);
        var quantityVariance = compareQty - baseQty;
        var priceEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(unitPriceVariance * baseQty);
        var quantityEffect = TaktBomMaterialCostItemLineCostHelper.RoundCost(quantityVariance * baseUnitPrice);
        var varianceAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareCost - baseCost);
        decimal? variancePercent = null;
        if (baseCost != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(varianceAmount / baseCost);
        }
        var changeType = ResolveVarianceChangeType(baseRow, compareRow, unitPriceVariance, quantityVariance);
        return new TaktBomMaterialCostAnalysisVarianceLineDto
        {
            BomItemCode = compareRow?.BomItemCode ?? baseRow?.BomItemCode ?? string.Empty,
            ComponentCode = compareRow?.ComponentCode ?? baseRow?.ComponentCode ?? string.Empty,
            ComponentDescription = compareRow?.ComponentDescription ?? baseRow?.ComponentDescription ?? string.Empty,
            PurchaseType = compareRow?.PurchaseType ?? baseRow?.PurchaseType ?? string.Empty,
            CurrencyCode = compareRow != null
                ? TaktBomMaterialCostItemLineCostHelper.ResolveCurrency(compareRow)
                : (baseRow != null ? TaktBomMaterialCostItemLineCostHelper.ResolveCurrency(baseRow) : string.Empty),
            BaseCost = baseCost,
            CompareCost = compareCost,
            VarianceAmount = varianceAmount,
            VariancePercent = variancePercent,
            BaseUnitPrice = baseUnitPrice,
            CompareUnitPrice = compareUnitPrice,
            UnitPriceVariance = unitPriceVariance,
            BaseQuantity = baseQty,
            CompareQuantity = compareQty,
            QuantityVariance = quantityVariance,
            PriceEffectAmount = priceEffect,
            QuantityEffectAmount = quantityEffect,
            ChangeType = changeType,
        };
    }

    /// <summary>
    /// 解析组件变动类型
    /// </summary>
    /// <param name="baseRow">基准行</param>
    /// <param name="compareRow">对比行</param>
    /// <param name="unitPriceVariance">单价差异</param>
    /// <param name="quantityVariance">数量差异</param>
    /// <returns>变动类型码</returns>
    private static string ResolveVarianceChangeType(
        TaktBomMaterialCostItem? baseRow,
        TaktBomMaterialCostItem? compareRow,
        decimal unitPriceVariance,
        decimal quantityVariance)
    {
        if (baseRow == null && compareRow != null)
        {
            return "new";
        }
        if (baseRow != null && compareRow == null)
        {
            return "removed";
        }
        var hasPrice = unitPriceVariance != 0m;
        var hasQty = quantityVariance != 0m;
        if (hasPrice && hasQty)
        {
            return "mixed";
        }
        if (hasPrice)
        {
            return "price";
        }
        if (hasQty)
        {
            return "quantity";
        }
        return "unchanged";
    }

    /// <summary>
    /// 解析月度涨跌分析涉及的产品编码列表
    /// </summary>
    /// <param name="rows">已加载汇总行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="productCode">单产品编码；为空表示机种下全部物料</param>
    /// <returns>产品编码列表</returns>
    internal static List<string> ResolveProductCodesInScope(
        IReadOnlyList<TaktBomMaterialCost> rows,
        string plantCode,
        string modelCode,
        string? productCode)
    {
        if (!string.IsNullOrWhiteSpace(productCode))
        {
            return new List<string> { productCode.Trim() };
        }
        return rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(r.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase))
            .Select(r => r.ProductCode)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.Ordinal)
            .ToList()!;
    }

    /// <summary>
    /// 取产品在指定期间的汇总月成本
    /// </summary>
    /// <param name="rows">汇总行</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="modelCode">机种编码</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="period">期间 yyyy-MM</param>
    /// <returns>产品月成本</returns>
    internal static decimal ResolveProductMonthlyCostFromHeaders(
        IReadOnlyList<TaktBomMaterialCost> rows,
        string plantCode,
        string modelCode,
        string productCode,
        string period)
    {
        var header = rows
            .Where(r => string.Equals(r.PlantCode, plantCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals(r.ModelCode, modelCode, StringComparison.OrdinalIgnoreCase)
                && TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode)
                && string.Equals(r.CostingPeriod, period, StringComparison.Ordinal))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault();
        return header?.ProductMonthlyCalculation ?? 0m;
    }

    /// <summary>
    /// 将 yyyy-MM 起止解析为核算日期范围
    /// </summary>
    /// <param name="periodStart">起始年月</param>
    /// <param name="periodEnd">结束年月</param>
    /// <returns>起止日期（可空）</returns>
    internal static (DateTime? Start, DateTime? End) ResolvePeriodRangeBounds(string? periodStart, string? periodEnd)
    {
        DateTime? start = null;
        DateTime? end = null;
        if (!string.IsNullOrWhiteSpace(periodStart))
        {
            var (rangeStart, _) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodStart.Trim());
            start = rangeStart;
        }
        if (!string.IsNullOrWhiteSpace(periodEnd))
        {
            var (_, rangeEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodEnd.Trim());
            end = rangeEnd;
        }
        return (start, end);
    }

    /// <summary>
    /// 计算环比涨跌指标
    /// </summary>
    /// <param name="currentCost">当月成本</param>
    /// <param name="previousCost">上月成本</param>
    /// <returns>差额、百分比、涨跌码</returns>
    internal static (decimal? VarianceAmount, decimal? VariancePercent, string Trend) ComputeMonthOverMonthTrend(
        decimal currentCost,
        decimal? previousCost)
    {
        if (!previousCost.HasValue)
        {
            return (null, null, "none");
        }
        var varianceAmount = currentCost - previousCost.Value;
        decimal? variancePercent = null;
        if (previousCost.Value != 0m)
        {
            variancePercent = TaktBomMaterialCostItemLineCostHelper.RoundPercentPoints(
                varianceAmount / previousCost.Value);
        }
        string trend;
        if (currentCost > previousCost.Value)
        {
            trend = "up";
        }
        else if (currentCost < previousCost.Value)
        {
            trend = "down";
        }
        else
        {
            trend = "flat";
        }
        return (varianceAmount, variancePercent, trend);
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
        if (costingMonthStart.HasValue)
        {
            var start = costingMonthStart.Value;
            exp = exp.And(x => x.CostingDate >= start);
        }
        if (costingMonthEnd.HasValue)
        {
            var endExclusive = costingMonthEnd.Value.AddMonths(1);
            exp = exp.And(x => x.CostingDate < endExclusive);
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
    /// 加载机种产品 BOM 明细（参与资格：生产相关=X、PCB SECT 标识为空、采购类型=F；可按核算月过滤；按条件全量，不截断）
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
                && x.IsDeleted == 0
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
            string? yearTable;
            try
            {
                yearTable = await ResolveBomItemPhysicalTableAsync(
                    TaktYearShardTableHelper.RequireSingleYear(costingMonthStart, costingMonthEnd));
            }
            catch (ArgumentException ex)
            {
                throw new TaktBusinessException(ex.Message);
            }
            var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
            allItems.AddRange(part);
        }
        return TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(allItems))
            .ToList();
    }


    /// <summary>
    /// 转置行全量合计：各期间成本 + 环比差额（分页前）
    /// </summary>
    /// <param name="rows">已筛选全量行</param>
    /// <param name="periodOrder">期间列</param>
    /// <returns>期间合计与环比差额合计</returns>
    private static (Dictionary<string, decimal> PeriodCostTotals, decimal? VarianceAmountTotal) SumTransposedRowGrandTotals(
        IReadOnlyList<TaktBomMaterialCostAnalysisTransposedDto> rows,
        IReadOnlyList<string> periodOrder)
    {
        return SumPeriodAndVarianceGrandTotals(
            periodOrder,
            rows.Select(r => (PeriodMap: (IReadOnlyDictionary<string, decimal>)r.PeriodCosts, r.VarianceAmount)));
    }


    /// <summary>
    /// 对各行期间字典与环比差额做全量求和
    /// </summary>
    /// <param name="periodOrder">期间列</param>
    /// <param name="rows">期间映射 + 环比差额</param>
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
        var yearsNeedBase = new List<int>();
        foreach (var year in years)
        {
            var table = await ResolveBomItemPhysicalTableAsync(year);
            if (table == null)
            {
                yearsNeedBase.Add(year);
                continue;
            }
            if (maxRows.HasValue)
            {
                var remaining = maxRows.Value - result.Count;
                if (remaining <= 0)
                {
                    break;
                }
                var part = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining, table);
                result.AddRange(part);
            }
            else
            {
                var part = await _bomMaterialCostItemRepository.GetListAsync(predicate, table);
                result.AddRange(part);
            }
        }
        if (yearsNeedBase.Count == 0)
        {
 // 年分表与基表合并： 同步常写基表，年分表可能仅部分数据；按 Id 去重
            if (!maxRows.HasValue || result.Count < maxRows.Value)
            {
                List<TaktBomMaterialCostItem> baseFallback;
                if (maxRows.HasValue)
                {
                    baseFallback = await _bomMaterialCostItemRepository.GetListForExportAsync(
                        predicate, maxRows.Value - result.Count);
                }
                else
                {
                    baseFallback = await _bomMaterialCostItemRepository.GetListAsync(predicate);
                }
                var yearSet = years.ToHashSet();
                var seenIds = result.Select(r => r.Id).ToHashSet();
                foreach (var row in baseFallback.Where(r => yearSet.Contains(r.CostingDate.Year)))
                {
                    if (!seenIds.Add(row.Id))
                    {
                        continue;
                    }
                    result.Add(row);
                    if (maxRows.HasValue && result.Count >= maxRows.Value)
                    {
                        break;
                    }
                }
            }
            return result;
        }
        if (maxRows.HasValue && result.Count >= maxRows.Value)
        {
            return result;
        }
        List<TaktBomMaterialCostItem> basePart;
        if (maxRows.HasValue)
        {
            var remaining = maxRows.Value - result.Count;
            basePart = await _bomMaterialCostItemRepository.GetListForExportAsync(predicate, remaining);
        }
        else
        {
            basePart = await _bomMaterialCostItemRepository.GetListAsync(predicate);
        }
        if (yearsNeedBase.Count == years.Count)
        {
            result.AddRange(basePart);
        }
        else
        {
            var yearSet = yearsNeedBase.ToHashSet();
            result.AddRange(basePart.Where(r => yearSet.Contains(r.CostingDate.Year)));
        }
        return result;
    }
}


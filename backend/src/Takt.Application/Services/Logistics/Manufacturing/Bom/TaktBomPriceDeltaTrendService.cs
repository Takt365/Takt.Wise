// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomPriceDeltaTrendService.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移应用服务（独立；产品月成本+0价格组+价格差异组）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移服务（与成本分析分离）
/// </summary>
public class TaktBomPriceDeltaTrendService : TaktServiceBase, ITaktBomPriceDeltaTrendService
{
    /// <summary>
    /// 移动价格候选查询分批上限（仅替代价查找；列表产品行数不设上限）
    /// </summary>
    private const int MovingPriceLookupMaxRows = 20000;
    /// <summary>
    /// 列表期间列上限（防横向/JSON 溢出）
    /// </summary>
    private const int MaxPeriodMonths = 24;
    /// <summary>
    /// 列表页组文字段截断长度（导出保留全文）
    /// </summary>
    private const int MaxGroupTextLength = 2000;
    /// <summary>
    /// 涨跌选项：涨幅、跌幅各前 3 条（默认）
    /// </summary>
    private const string ImpactLineOptionUp3Down3 = "up3down3";
    /// <summary>
    /// 涨跌选项：涨幅、跌幅各前 5 条
    /// </summary>
    private const string ImpactLineOptionUp5Down5 = "up5down5";
    /// <summary>
    /// 涨跌选项：全部明细
    /// </summary>
    private const string ImpactLineOptionAll = "all";
    /// <summary>
    /// 涨价逐条下限。没有达到的一侧不写明细
    /// </summary>
    private const decimal MinIncreaseDiff = 1.0000m;
    /// <summary>
    /// 跌价逐条上限。没有达到的一侧不写明细
    /// </summary>
    private const decimal MaxDecreaseDiff = -1.0000m;

    /// <summary>
    /// 涨跌明细窗口。ApplyThreshold 为 true 时只保留差额达到 1 或 -1 的行，某一侧没有则该侧为空。
    /// </summary>
    /// <param name="IncreaseTake">涨价最多条数</param>
    /// <param name="DecreaseTake">跌价最多条数</param>
    /// <param name="ApplyThreshold">是否套用 1 / -1 门槛</param>
    private readonly record struct ImpactLineWindow(int IncreaseTake, int DecreaseTake, bool ApplyThreshold);
    /// <summary>
    /// 差异选项：全部（四组文完整）
    /// </summary>
    private const string PriceDeltaOptionAll = "all";
    /// <summary>
    /// 差异选项：差异绝对值≥1 才保留产品行（界面 >=1）
    /// </summary>
    private const string PriceDeltaOptionGt1 = "gt1";
    /// <summary>
    /// 差异选项：差异绝对值≥5 才保留产品行（界面 >=5）
    /// </summary>
    private const string PriceDeltaOptionGt5 = "gt5";
    /// <summary>
    /// 差异选项：差异绝对值≥10 才保留产品行（界面 >=10）
    /// </summary>
    private const string PriceDeltaOptionGt10 = "gt10";
    /// <summary>
    /// 差异选项：差异绝对值≥50 才保留产品行（界面 >=50）
    /// </summary>
    private const string PriceDeltaOptionGt50 = "gt50";
    /// <summary>
    /// 差异选项：差异绝对值≥100 才保留产品行（界面 >=100）
    /// </summary>
    private const string PriceDeltaOptionGt100 = "gt100";
    /// <summary>
    /// 改修列工单类别（TaktProductionOrder.ProdOrderType = ZDTB）
    /// </summary>
    private const string ReworkProdOrderType = "ZDTB";
    /// <summary>
    /// 改修工单查询行数上限（当前页/导出产品集合内）
    /// </summary>
    private const int ReworkOrderLookupMaxRows = 20000;
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";
    private const string MovingPriceYearShardBaseTable = "takt_logistics_materials_material_moving_price";

    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    private readonly ITaktCompanyRepository<TaktMaterialMovingPrice> _materialMovingPriceRepository;
    private readonly ITaktCompanyRepository<TaktProductionOrder> _productionOrderRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 成本汇总仓储</param>
    /// <param name="materialMovingPriceRepository">移动价格仓储（0价格组可替代价）</param>
    /// <param name="productionOrderRepository">生产工单仓储（改修列 ZDTB）</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomPriceDeltaTrendService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktCompanyRepository<TaktMaterialMovingPrice> materialMovingPriceRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _materialMovingPriceRepository = materialMovingPriceRepository;
        _productionOrderRepository = productionOrderRepository;
    }

    /// <summary>
    /// 成本差异推移列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页结果</returns>
    public async Task<TaktBomPriceDeltaTrendResultDto> GetBomPriceDeltaTrendListAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto)
    {
        return await BuildBomPriceDeltaTrendResultAsync(
            queryDto,
            forExport: false,
            truncateGroupTexts: true);
    }

    /// <summary>
    /// 导出成本差异推移（全量产品，不分页截断）
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBomPriceDeltaTrendAsync(
        TaktBomPriceDeltaTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var result = await BuildBomPriceDeltaTrendResultAsync(
            query,
            forExport: true,
            truncateGroupTexts: false);
        var columnKeys = new List<string> { "modelCode", "productCode", "productDescription" };
        var columnLabels = new List<string> { "机种", "产品", "产品描述" };
        foreach (var period in result.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.Add("priceDelta");
        columnLabels.Add("差异");
        columnKeys.Add("zeroPriceGroup");
        columnLabels.Add("0价格组");
        columnKeys.Add("replaceComponentGroup");
        columnLabels.Add("建议替代价格");
        columnKeys.Add("priceDeltaTrend");
        columnLabels.Add("价格差异组");
        columnKeys.Add("componentDeltaGroup");
        columnLabels.Add("组件差异");
        columnKeys.Add("reworkOrderGroup");
        columnLabels.Add("改修");
        var exportRows = result.Paged.Data.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["modelCode"] = row.ModelCode,
                ["productCode"] = row.ProductCode,
                ["productDescription"] = row.ProductDescription,
                ["priceDelta"] = row.PriceDelta,
                ["zeroPriceGroup"] = row.ZeroPriceGroup,
                ["replaceComponentGroup"] = row.ReplaceComponentGroup,
                ["priceDeltaTrend"] = row.PriceDeltaTrend,
                ["componentDeltaGroup"] = row.ComponentDeltaGroup,
                ["reworkOrderGroup"] = row.ReworkOrderGroup,
            };
            foreach (var period in result.PeriodOrder)
            {
                row.PeriodCosts.TryGetValue(period, out var cost);
                dict[$"period_{period}"] = cost;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        var columnWidths = new Dictionary<string, double>(StringComparer.Ordinal)
        {
            ["zeroPriceGroup"] = 20d,
            ["replaceComponentGroup"] = 20d,
            ["priceDeltaTrend"] = 20d,
            ["componentDeltaGroup"] = 20d,
            ["reworkOrderGroup"] = 20d,
        };
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "DTA BOM成本差异推移",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "DTA BOM成本差异推移"),
            columnWidths: columnWidths,
            autoFitUnspecifiedColumns: true,
            keepNaturalRowHeight: true);
    }

    /// <summary>
    /// 构建列表/导出结果：按产品目录分页（列表）；导出取全量产品，无行数上限
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <param name="forExport">true=全量行；false=服务端分页</param>
    /// <param name="truncateGroupTexts">列表截断超长组文</param>
    /// <returns>结果</returns>
    private async Task<TaktBomPriceDeltaTrendResultDto> BuildBomPriceDeltaTrendResultAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto,
        bool forExport,
        bool truncateGroupTexts)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();

        var pageIndex = forExport ? 1 : TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = forExport
            ? TaktPagedClamp.DefaultPageSize
            : TaktPagedClamp.NormalizePageSize(
                queryDto.PageSize <= 0 ? TaktPagedClamp.DefaultPageSize : queryDto.PageSize);

        var plantCode = queryDto.PlantCode.Trim();
        var materialType = string.IsNullOrWhiteSpace(queryDto.MaterialType)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : queryDto.MaterialType.Trim();
        if (!queryDto.CostingDateStart.HasValue || !queryDto.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算期间");
        }

        // 按核算月整月归一（起=月初，止=月末 23:59:59.999），避免前端 yyyy-MM-dd 绑成 00:00:00 截断当日
        var (rangeStart, rangeEnd) = NormalizeCostingDateRange(
            queryDto.CostingDateStart.Value,
            queryDto.CostingDateEnd.Value);
        var periodOrder = BuildPeriodOrder(rangeStart, rangeEnd);
        if (periodOrder.Count == 0)
        {
            return EmptyResult(pageIndex, pageSize);
        }
        if (periodOrder.Count > MaxPeriodMonths)
        {
            throw new TaktBusinessException(
                $"核算期间最多 {MaxPeriodMonths} 个月（当前 {periodOrder.Count} 个月），请缩小期间以免列表溢出");
        }

        // 差异/组对比：基准月 vs 比较月；必须都在核算期间列内，可不相邻
        var (againstPeriod, focusPeriod) = ResolveComparePeriods(queryDto, periodOrder);
        var headers = await LoadCostHeadersAsync(
            queryDto, plantCode, materialType, rangeStart, rangeEnd);
        var productGroups = headers
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var catalog = productGroups
            .Where(kv => kv.Value.Any(r => periodOrder.Any(p => HeaderMatchesPeriod(r, p))))
            .Select(kv =>
            {
                var latest = kv.Value.OrderByDescending(r => r.CostingDate).First();
                return (
                    ProductKey: kv.Key,
                    ProductCode: latest.ProductCode?.Trim() ?? kv.Key,
                    ProductDescription: latest.ProductDescription?.Trim() ?? string.Empty,
                    Rows: kv.Value);
            })
            .OrderBy(x => x.ProductCode, StringComparer.Ordinal)
            .ToList();

        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var productFilter = queryDto.ProductCode.Trim();
            catalog = catalog
                .Where(p => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(p.ProductCode, productFilter))
                .ToList();
        }

        // 期间合集口径：主表核算日落在期间内且未软删。
        // 列表再要求比较月+基准月各自有核算日当月、未删、月计算>0；6 无 + 7/8 删 → 不应出现。
        if (!string.IsNullOrWhiteSpace(againstPeriod) && !string.IsNullOrWhiteSpace(focusPeriod))
        {
            catalog = catalog
                .Where(p =>
                    HasValidCostingDateHeader(p.Rows, againstPeriod!)
                    && HasValidCostingDateHeader(p.Rows, focusPeriod!))
                .ToList();
        }

        // 差异选项：按主表月成本差 |基准月−比较月| ≥ 阈值过滤产品行（分页总数随之变化）；全部不过滤
        var priceDeltaInclusiveMin = ResolvePriceDeltaInclusiveMin(queryDto.PriceDeltaOption);
        if (priceDeltaInclusiveMin != null
            && !string.IsNullOrWhiteSpace(againstPeriod)
            && !string.IsNullOrWhiteSpace(focusPeriod))
        {
            catalog = catalog
                .Where(p => MeetsPriceDeltaInclusiveMin(
                    p.Rows, againstPeriod!, focusPeriod!, priceDeltaInclusiveMin.Value))
                .ToList();
        }

        var total = catalog.Count;
        List<(string ProductKey, string ProductCode, string ProductDescription, List<TaktBomMaterialCost> Rows)> pageCatalog;
        if (forExport)
        {
            pageCatalog = catalog;
            pageSize = total <= 0 ? TaktPagedClamp.DefaultPageSize : total;
        }
        else
        {
            var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
            pageCatalog = catalog.Skip(skip).Take(pageSize).ToList();
        }

        var pageRows = pageCatalog
            .Select(item => BuildProductRow(
                plantCode,
                item.ProductCode,
                item.ProductDescription,
                item.Rows,
                periodOrder,
                againstPeriod,
                focusPeriod))
            .ToList();

        // 仅当前页（或导出全量页）按明细填组文与差异列，避免未展示行做 BOM 明细全量计算
        var impactWindow = ResolveImpactLineWindow(queryDto.ImpactLineOption);
        await FillComponentGroupTextsAsync(plantCode, pageRows, againstPeriod, focusPeriod, impactWindow);
        // 改修列：与差异阈值无关；按产品编码 + 基准月实际开始日期填 ZDTB 工单号
        await FillReworkOrderGroupAsync(plantCode, pageRows, focusPeriod);

        if (truncateGroupTexts)
        {
            foreach (var row in pageRows)
            {
                TruncateRowGroupTextsForDisplay(row);
            }
        }

        return new TaktBomPriceDeltaTrendResultDto
        {
            Paged = TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
                pageRows, total, pageIndex, forExport ? Math.Max(pageSize, 1) : pageSize),
            PeriodOrder = periodOrder,
            BasePeriod = focusPeriod,
            ComparePeriod = againstPeriod,
        };
    }

    /// <summary>
    /// 解析差异选项为「差异」绝对值含等下限。null=全部（不过滤）；仅允许 all / gt1 / gt5 / gt10 / gt50 / gt100。
    /// 界面选项为 >=1 / >=5 / >=10 / >=50 / >=100，业务比较为 |差异| ≥ 下限。
    /// </summary>
    /// <param name="option">查询选项</param>
    /// <returns>含等下限；全部为 null</returns>
    private static decimal? ResolvePriceDeltaInclusiveMin(string? option)
    {
        if (string.IsNullOrWhiteSpace(option)
            || string.Equals(option.Trim(), PriceDeltaOptionAll, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        var key = option.Trim();
        if (string.Equals(key, PriceDeltaOptionGt1, StringComparison.OrdinalIgnoreCase))
        {
            return 1m;
        }
        if (string.Equals(key, PriceDeltaOptionGt5, StringComparison.OrdinalIgnoreCase))
        {
            return 5m;
        }
        if (string.Equals(key, PriceDeltaOptionGt10, StringComparison.OrdinalIgnoreCase))
        {
            return 10m;
        }
        if (string.Equals(key, PriceDeltaOptionGt50, StringComparison.OrdinalIgnoreCase))
        {
            return 50m;
        }
        if (string.Equals(key, PriceDeltaOptionGt100, StringComparison.OrdinalIgnoreCase))
        {
            return 100m;
        }
        throw new TaktBusinessException("差异选项仅支持全部、>=1、>=5、>=10、>=50、>=100");
    }

    /// <summary>
    /// 解析涨跌明细窗口。空值按涨3跌3。未知值拒绝。
    /// </summary>
    /// <param name="option">up3down3 / up5down5 / all</param>
    /// <returns>涨跌条数与是否套用 1 / -1 门槛</returns>
    private static ImpactLineWindow ResolveImpactLineWindow(string? option)
    {
        if (string.IsNullOrWhiteSpace(option)
            || string.Equals(option.Trim(), ImpactLineOptionUp3Down3, StringComparison.OrdinalIgnoreCase))
        {
            return new ImpactLineWindow(3, 3, true);
        }
        var key = option.Trim();
        if (string.Equals(key, ImpactLineOptionUp5Down5, StringComparison.OrdinalIgnoreCase))
        {
            return new ImpactLineWindow(5, 5, true);
        }
        if (string.Equals(key, ImpactLineOptionAll, StringComparison.OrdinalIgnoreCase))
        {
            return new ImpactLineWindow(int.MaxValue, int.MaxValue, false);
        }
        throw new TaktBusinessException("涨跌选项仅支持涨3跌3、涨5跌5、全部");
    }

    /// <summary>
    /// 主表月成本差是否达到差异选项阈值：|基准月−比较月| ≥ inclusiveMin；任一侧成本≤0 则不达标
    /// </summary>
    /// <param name="productRows">该产品主表行</param>
    /// <param name="againstPeriod">比较月</param>
    /// <param name="focusPeriod">基准月</param>
    /// <param name="inclusiveMin">绝对值下限</param>
    /// <returns>达标则为 true</returns>
    private static bool MeetsPriceDeltaInclusiveMin(
        List<TaktBomMaterialCost> productRows,
        string againstPeriod,
        string focusPeriod,
        decimal inclusiveMin)
    {
        var againstCost = ResolveProductMonthlyCostColumn(productRows, againstPeriod);
        var focusCost = ResolveProductMonthlyCostColumn(productRows, focusPeriod);
        if (againstCost <= 0m || focusCost <= 0m)
        {
            return false;
        }
        var delta = TaktBomMaterialCostItemLineCostHelper.RoundCost(focusCost - againstCost);
        return Math.Abs(delta) >= inclusiveMin;
    }

    /// <summary>
    /// 列表页截断超长组文，防止单元格/JSON 撑爆
    /// </summary>
    private static void TruncateRowGroupTextsForDisplay(TaktBomPriceDeltaTrendDto row)
    {
        row.ZeroPriceGroup = TruncateDisplayText(row.ZeroPriceGroup);
        row.ReplaceComponentGroup = TruncateDisplayText(row.ReplaceComponentGroup);
        row.PriceDeltaTrend = TruncateDisplayText(row.PriceDeltaTrend);
        row.ComponentDeltaGroup = TruncateDisplayText(row.ComponentDeltaGroup);
        row.ReworkOrderGroup = TruncateDisplayText(row.ReworkOrderGroup);
    }

    /// <summary>
    /// 截断展示文本
    /// </summary>
    private static string TruncateDisplayText(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= MaxGroupTextLength)
        {
            return value ?? string.Empty;
        }
        return value[..MaxGroupTextLength] + "…";
    }

    /// <summary>
    /// 填充改修列：当前工厂下，物料编码匹配产品编码、工单类别 ZDTB、实际开始日期落在基准月的工单号清单（去重、按工单号排序，逗号分隔）。
    /// 仅查当前页/导出产品，避免全表。与差异选项无关。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="rows">当前页或导出行</param>
    /// <param name="basePeriod">基准月 yyyy-MM</param>
    private async Task FillReworkOrderGroupAsync(
        string plantCode,
        List<TaktBomPriceDeltaTrendDto> rows,
        string? basePeriod)
    {
        foreach (var row in rows)
        {
            row.ReworkOrderGroup = string.Empty;
        }
        if (rows.Count == 0
            || string.IsNullOrWhiteSpace(plantCode)
            || string.IsNullOrWhiteSpace(basePeriod)
            || !TryParsePeriodMonth(basePeriod, out var monthStart))
        {
            return;
        }
        var monthEndExclusive = monthStart.AddMonths(1);
        var lookupCodes = rows
            .Select(r => r.ProductCode)
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return;
        }
        var orders = new List<TaktProductionOrder>();
        const int chunkSize = 200;
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var exp = Expressionable.Create<TaktProductionOrder>()
                .And(x =>
                    x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && x.IsDeleted == 0
                    && x.ProdOrderType == ReworkProdOrderType
                    && chunk.Contains(x.MaterialCode)
                    && x.ActualStartDate != null
                    && x.ActualStartDate >= monthStart
                    && x.ActualStartDate < monthEndExclusive);
            var part = await _productionOrderRepository.GetListAsync(exp.ToExpression());
            if (part.Count > 0)
            {
                var remain = ReworkOrderLookupMaxRows - orders.Count;
                if (remain <= 0)
                {
                    break;
                }
                orders.AddRange(remain >= part.Count ? part : part.Take(remain));
            }
            if (orders.Count >= ReworkOrderLookupMaxRows)
            {
                break;
            }
        }
        var codesByProduct = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var order in orders)
        {
            if (string.IsNullOrWhiteSpace(order.ProdOrderCode) || string.IsNullOrWhiteSpace(order.MaterialCode))
            {
                continue;
            }
            var orderCode = order.ProdOrderCode.Trim();
            foreach (var row in rows)
            {
                if (!TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(order.MaterialCode, row.ProductCode))
                {
                    continue;
                }
                var key = NormalizeProductKey(row.ProductCode);
                if (!codesByProduct.TryGetValue(key, out var list))
                {
                    list = new List<string>();
                    codesByProduct[key] = list;
                }
                if (!list.Exists(c => string.Equals(c, orderCode, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(orderCode);
                }
                break;
            }
        }
        foreach (var row in rows)
        {
            var key = NormalizeProductKey(row.ProductCode);
            if (!codesByProduct.TryGetValue(key, out var list) || list.Count == 0)
            {
                continue;
            }
            row.ReworkOrderGroup = string.Join(", ", list.OrderBy(c => c, StringComparer.Ordinal));
        }
    }

    /// <summary>
    /// 核算日起止归一为整月（起日 00:00:00，止日 23:59:59.999）
    /// </summary>
    /// <param name="start">查询起</param>
    /// <param name="end">查询止</param>
    /// <returns>月初、月末末刻</returns>
    private static (DateTime RangeStart, DateTime RangeEnd) NormalizeCostingDateRange(
        DateTime start,
        DateTime end)
    {
        var rangeStart = new DateTime(start.Year, start.Month, 1);
        var endMonth = new DateTime(end.Year, end.Month, 1);
        if (endMonth < rangeStart)
        {
            endMonth = rangeStart;
        }
        var lastDay = DateTime.DaysInMonth(endMonth.Year, endMonth.Month);
        var rangeEnd = new DateTime(endMonth.Year, endMonth.Month, lastDay, 23, 59, 59, 999);
        return (rangeStart, rangeEnd);
    }

    private async Task<List<TaktBomMaterialCost>> LoadCostHeadersAsync(
        TaktBomPriceDeltaTrendQueryDto queryDto,
        string plantCode,
        string materialType,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var start = rangeStart;
        var end = rangeEnd;
        var fert = TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode;
        var exp = Expressionable.Create<TaktBomMaterialCost>();
        // 仅按核算日落在期间窗口内加载；禁止再用 CostingPeriod∈期间 OR，
        // 否则会出现「7/8 真行已软删，却被其它日期行的 CostingPeriod=7/8 顶进来」而误显示产品。
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.IsDeleted == 0
            && x.CostingDate >= start
            && x.CostingDate <= end);
        // FERT：空物料类型与 FERT 同等（避免七月行 MaterialType 空串被过滤掉）
        if (string.Equals(materialType, fert, StringComparison.OrdinalIgnoreCase))
        {
            exp = exp.And(x =>
                x.MaterialType == materialType
                || x.MaterialType == null
                || x.MaterialType == string.Empty);
        }
        else
        {
            exp = exp.And(x => x.MaterialType == materialType);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            var model = queryDto.ModelCode.Trim();
            exp = exp.And(x => x.ModelCode == model);
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            var product = queryDto.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(product));
        }
        var rows = await _bomMaterialCostRepository.GetListAsync(exp.ToExpression());
        // 防御：仅未删；月份列只允许用 ProductMonthlyCalculation
        rows = rows.Where(r => r.IsDeleted == 0).ToList();
        foreach (var row in rows)
        {
            row.ModelMonthlyAverageCost = 0m;
        }
        return rows;
    }

    private static List<string> BuildPeriodOrder(DateTime start, DateTime end)
    {
        var cursor = new DateTime(start.Year, start.Month, 1);
        var last = new DateTime(end.Year, end.Month, 1);
        var list = new List<string>();
        while (cursor <= last)
        {
            list.Add($"{cursor.Year:D4}-{cursor.Month:D2}");
            cursor = cursor.AddMonths(1);
        }
        return list;
    }

    /// <summary>
    /// 对比：关注月（基准月）成本 − 基期（比较月）成本。
    /// 查询 BasePeriod=基准月，ComparePeriod=比较月；必须都在核算期间列内，默认同列止月 vs 止减一月。
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <param name="periodOrder">展示期间列</param>
    /// <returns>against=比较月（基期），focus=基准月（关注）</returns>
    private static (string? AgainstPeriod, string? FocusPeriod) ResolveComparePeriods(
        TaktBomPriceDeltaTrendQueryDto queryDto,
        IReadOnlyList<string> periodOrder)
    {
        if (periodOrder.Count == 0)
        {
            return (null, null);
        }
        var periodSet = periodOrder.ToHashSet(StringComparer.Ordinal);
        var focusRaw = FirstNonEmptyPeriod(queryDto.BasePeriod, queryDto.FocusPeriod);
        if (string.IsNullOrWhiteSpace(focusRaw))
        {
            focusRaw = periodOrder[^1];
        }
        if (!TryParsePeriodMonth(focusRaw, out var focusMonth))
        {
            throw new TaktBusinessException("请选择基准月");
        }
        var focus = focusMonth.ToString("yyyy-MM");
        if (!periodSet.Contains(focus))
        {
            throw new TaktBusinessException("基准月必须在核算期间内");
        }
        var againstRaw = queryDto.ComparePeriod?.Trim();
        if (string.IsNullOrWhiteSpace(againstRaw))
        {
            var previous = focusMonth.AddMonths(-1).ToString("yyyy-MM");
            var against = periodSet.Contains(previous)
                ? previous
                : (periodOrder.Count >= 2 ? periodOrder[^2] : periodOrder[0]);
            return (against, focus);
        }
        if (!TryParsePeriodMonth(againstRaw, out var againstMonth))
        {
            throw new TaktBusinessException("请选择比较月");
        }
        var againstPeriod = againstMonth.ToString("yyyy-MM");
        if (!periodSet.Contains(againstPeriod))
        {
            throw new TaktBusinessException("比较月必须在核算期间内");
        }
        return (againstPeriod, focus);
    }

    /// <summary>
    /// 取首个非空期间
    /// </summary>
    private static string? FirstNonEmptyPeriod(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }
        return null;
    }

    private static TaktBomPriceDeltaTrendDto BuildProductRow(
        string plantCode,
        string productCode,
        string productDescription,
        List<TaktBomMaterialCost> productRows,
        IReadOnlyList<string> periodOrder,
        string? againstPeriod,
        string? focusPeriod)
    {
        var periodCosts = new Dictionary<string, decimal>(StringComparer.Ordinal);
        foreach (var period in periodOrder)
        {
            // 只取 product_monthly_calculation；ModelMonthlyAverageCost 在 Load 时已清零
            var productMonthlyCost = ResolveProductMonthlyCostColumn(productRows, period);
            if (productMonthlyCost > 0m)
            {
                periodCosts[period] = productMonthlyCost;
            }
        }
        EnsurePeriodCostKey(periodCosts, productRows, againstPeriod);
        EnsurePeriodCostKey(periodCosts, productRows, focusPeriod);
        // 展示用机种/描述取期间内最新核算日行（HeaderMatchesPeriod：CostingPeriod 或 CostingDate 月）
        var latestInDisplay = productRows
            .Where(r => periodOrder.Any(p => HeaderMatchesPeriod(r, p)))
            .OrderByDescending(r => r.CostingDate)
            .FirstOrDefault()
            ?? productRows.OrderByDescending(r => r.CostingDate).FirstOrDefault();
        decimal? priceDelta = null;
        if (!string.IsNullOrWhiteSpace(againstPeriod) && !string.IsNullOrWhiteSpace(focusPeriod))
        {
            var againstCost = ResolveProductMonthlyCostColumn(productRows, againstPeriod);
            var focusCost = ResolveProductMonthlyCostColumn(productRows, focusPeriod);
            // 比较月或基准月产品成本为 0：标记跳过（PriceDelta=null）；有价时差异=基准月产品成本−比较月产品成本
            if (againstCost > 0m && focusCost > 0m)
            {
                priceDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(focusCost - againstCost);
            }
        }
        return new TaktBomPriceDeltaTrendDto
        {
            PlantCode = plantCode,
            ModelCode = latestInDisplay?.ModelCode?.Trim() ?? string.Empty,
            ProductCode = productCode,
            ProductDescription = string.IsNullOrWhiteSpace(productDescription)
                ? (latestInDisplay?.ProductDescription?.Trim() ?? string.Empty)
                : productDescription,
            PeriodCosts = periodCosts,
            PriceDelta = priceDelta,
            // 建议替代价格仅由本产品明细 ExtField 填充（Fill），禁止主表机种月均履历串产品
            ReplaceComponentGroup = string.Empty,
            BasePeriod = focusPeriod,
            ComparePeriod = againstPeriod,
        };
    }

    /// <summary>
    /// 将基准月/比较月成本写入 PeriodCosts（即使不在展示列），供跳过差异判定
    /// </summary>
    private static void EnsurePeriodCostKey(
        Dictionary<string, decimal> periodCosts,
        List<TaktBomMaterialCost> productRows,
        string? period)
    {
        if (string.IsNullOrWhiteSpace(period) || periodCosts.ContainsKey(period))
        {
            return;
        }
        var cost = ResolveProductMonthlyCostColumn(productRows, period);
        if (cost > 0m)
        {
            periodCosts[period] = cost;
        }
    }

    /// <summary>
    /// 指定核算月是否有有效主表：核算日落在该月、未软删、产品月计算&gt;0（不以 CostingPeriod 字符串单独认定）
    /// </summary>
    private static bool HasValidCostingDateHeader(
        IReadOnlyList<TaktBomMaterialCost> productRows,
        string period)
    {
        if (productRows == null || string.IsNullOrWhiteSpace(period))
        {
            return false;
        }
        return productRows.Any(r =>
            r.IsDeleted == 0
            && r.ProductMonthlyCalculation > 0m
            && ToPeriodKey(r.CostingDate) == period);
    }

    /// <summary>
    /// 取指定核算月的 product_monthly_calculation（实体属性 ProductMonthlyCalculation）。
    /// ❌ 绝不读取 ModelMonthlyAverageCost / model_monthly_average_cost。
    /// 优先核算日落在该月的行；无则再回退 CostingPeriod 规范化匹配（展示列兼容）。
    /// </summary>
    /// <param name="productRows">该产品主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <returns>产品月成本；无正价为 0</returns>
    private static decimal ResolveProductMonthlyCostColumn(
        List<TaktBomMaterialCost> productRows,
        string period)
    {
        var byDate = productRows
            .Where(r => r.IsDeleted == 0 && ToPeriodKey(r.CostingDate) == period)
            .OrderByDescending(r => r.ProductMonthlyCalculation)
            .ThenByDescending(r => r.CostingDate)
            .ThenByDescending(r => r.Id)
            .FirstOrDefault();
        if (byDate != null && byDate.ProductMonthlyCalculation > 0m)
        {
            return byDate.ProductMonthlyCalculation;
        }
        return 0m;
    }

    /// <summary>
    /// 主表行是否属于核算月（仅核算日月份；与列表准入口径一致）
    /// </summary>
    /// <param name="header">主表行</param>
    /// <param name="period">yyyy-MM</param>
    /// <returns>属于该月则为 true</returns>
    private static bool HeaderMatchesPeriod(TaktBomMaterialCost header, string period)
        => ToPeriodKey(header.CostingDate) == period;

    /// <summary>
    /// 核算期间键规范化为 yyyy-MM（兼容 2026/07、202607）
    /// </summary>
    /// <param name="costingPeriod">原始期间</param>
    /// <returns>yyyy-MM；无法解析则原 Trim 或空</returns>
    private static string NormalizeCostingPeriodKey(string? costingPeriod)
    {
        if (string.IsNullOrWhiteSpace(costingPeriod))
        {
            return string.Empty;
        }
        var raw = costingPeriod.Trim().Replace('/', '-');
        if (raw.Length == 6 && raw.All(char.IsDigit))
        {
            return $"{raw[..4]}-{raw[4..6]}";
        }
        if (raw.Length >= 7 && raw[4] == '-')
        {
            return raw[..7];
        }
        return raw;
    }

    private async Task FillComponentGroupTextsAsync(
        string plantCode,
        List<TaktBomPriceDeltaTrendDto> rows,
        string? basePeriod,
        string? comparePeriod,
        ImpactLineWindow impactWindow)
    {
        if (rows.Count == 0 || string.IsNullOrWhiteSpace(comparePeriod))
        {
            return;
        }
        if (!TryParsePeriodMonth(comparePeriod, out var compareMonth))
        {
            return;
        }
        DateTime rangeStart = compareMonth;
        DateTime rangeEnd = compareMonth;
        if (!string.IsNullOrWhiteSpace(basePeriod) && TryParsePeriodMonth(basePeriod, out var baseMonth))
        {
            rangeStart = baseMonth < compareMonth ? baseMonth : compareMonth;
            rangeEnd = baseMonth > compareMonth ? baseMonth : compareMonth;
        }
        var productCodes = rows.Select(r => r.ProductCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var items = await LoadBomCostItemsForProductsAsync(plantCode, productCodes, rangeStart, rangeEnd);
        var byProduct = items
            .Where(r => !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(r => NormalizeProductKey(r.ProductCode!), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rawZeroByProduct = new Dictionary<string, List<(string Code, decimal Qty)>>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            var skipVariance = !string.IsNullOrWhiteSpace(basePeriod)
                && ShouldSkipVarianceForProduct(row, basePeriod!, comparePeriod!);

            if (!byProduct.TryGetValue(NormalizeProductKey(row.ProductCode), out var productItems))
            {
                // 无本产品明细：四列组文全空（禁止保留主表 ExtField 串产品脏文）
                row.ReplaceComponentGroup = string.Empty;
                if (skipVariance || IsZeroOrEmptyVariance(row.PriceDelta))
                {
                    ClearProductVarianceAndGroupFields(row);
                }
                continue;
            }

            // 防御：再按 ProductCode 收紧一次，杜绝串产品明细
            productItems = FilterItemsBelongingToProduct(productItems, row.ProductCode);

            // 建议替代价格：仅本产品明细行 ExtField（无 scope）且 component=本行组件
            row.ReplaceComponentGroup = BuildSuggestedSubstitutePriceGroup(
                productItems,
                comparePeriod!);

            if (skipVariance)
            {
                ClearProductVarianceAndGroupFields(row);
                continue;
            }

            if (!string.IsNullOrWhiteSpace(basePeriod))
            {
                var (priceText, _, componentText, _) = BuildDeltaGroupTexts(
                    plantCode,
                    row.ProductCode,
                    productItems,
                    basePeriod!,
                    comparePeriod!,
                    impactWindow);
                row.PriceDeltaTrend = priceText;
                row.ComponentDeltaGroup = componentText;
                // 差异列保持主表产品月成本（基准月−比较月）。价格组/组件组只说明构成，不回写差异。
            }

            // 环比差异为 0：0价格组 / 价格差异组 / 组件差异组全空
            if (IsZeroOrEmptyVariance(row.PriceDelta))
            {
                ClearProductVarianceAndGroupFields(row);
                continue;
            }

            if (HasPositivePeriodCost(row.PeriodCosts, comparePeriod!))
            {
                rawZeroByProduct[NormalizeProductKey(row.ProductCode)] =
                    CollectZeroPriceEntries(productItems, comparePeriod!);
            }
        }

        var rawZeroCodes = rawZeroByProduct.Values
            .SelectMany(list => list.Select(e => e.Code))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var headerCostByComponent = await LoadProductMonthlyCostByCodesAsync(
            plantCode, comparePeriod!, rawZeroCodes);
        var pricedComponentCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in rawZeroCodes)
        {
            if (TryGetProductMonthlyCost(headerCostByComponent, code, out _))
            {
                pricedComponentCodes.Add(code);
            }
        }

        var zeroByProduct = new Dictionary<string, List<(string Code, decimal Qty)>>(StringComparer.OrdinalIgnoreCase);
        foreach (var (productKey, list) in rawZeroByProduct)
        {
            zeroByProduct[productKey] = list
                .Where(e => !pricedComponentCodes.Contains(e.Code))
                .ToList();
        }

        var substituteByComponent = await ResolveSubstitutePricesAsync(
            plantCode,
            comparePeriod!,
            zeroByProduct.Values.SelectMany(list => list.Select(e => e.Code))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList());

        foreach (var row in rows)
        {
            var productKey = NormalizeProductKey(row.ProductCode);
            if (!zeroByProduct.TryGetValue(productKey, out var zeroEntries))
            {
                continue;
            }
            row.ZeroPriceGroup = BuildZeroPriceGroupText(zeroEntries, substituteByComponent);
        }
    }

    /// <summary>
    /// 是否跳过差异统计：比较月/基准月任一产品成本为 0。
    /// 跳过或算出差异为 0 时：差异列=0，0价格组/价格差异组/组件差异组全空。
    /// </summary>
    private static bool ShouldSkipVarianceForProduct(
        TaktBomPriceDeltaTrendDto row,
        string basePeriod,
        string comparePeriod)
    {
        // 比较月为 0 / 缺失 → 不参与
        if (!HasPositivePeriodCost(row.PeriodCosts, comparePeriod))
        {
            return true;
        }
        // 基准月为 0（有键无正价）→ 不参与
        if (row.PeriodCosts.ContainsKey(basePeriod)
            && !HasPositivePeriodCost(row.PeriodCosts, basePeriod))
        {
            return true;
        }
        // 基准月键缺失：展示期内成本为 0 时 PriceDelta 仍为 null（跳过标记）；
        // 基准月不在展示期且两侧有价时 PriceDelta 已有数值（含 0）
        if (!row.PeriodCosts.ContainsKey(basePeriod))
        {
            return row.PriceDelta == null;
        }
        return false;
    }

    private static bool HasPositivePeriodCost(
        IReadOnlyDictionary<string, decimal> periodCosts,
        string period)
        => periodCosts.TryGetValue(period, out var cost) && cost > 0m;

    /// <summary>
    /// 环比差异为空（跳过）或数值为 0
    /// </summary>
    private static bool IsZeroOrEmptyVariance(decimal? priceDelta)
        => priceDelta == null || priceDelta.Value == 0m;

    /// <summary>
    /// 差异列置 0，三组文全空（比较/基准月成本为 0，或环比差异为 0）；建议替代价格保留（本产品明细）
    /// </summary>
    private static void ClearProductVarianceAndGroupFields(TaktBomPriceDeltaTrendDto row)
    {
        row.PriceDelta = 0m;
        row.PriceDeltaTrend = string.Empty;
        row.ComponentDeltaGroup = string.Empty;
        row.ZeroPriceGroup = string.Empty;
    }

    /// <summary>
    /// 仅保留属于指定产品的明细行（ProductCodeMatches），防止串产品
    /// </summary>
    private static List<TaktBomMaterialCostItem> FilterItemsBelongingToProduct(
        IReadOnlyList<TaktBomMaterialCostItem> items,
        string productCode)
    {
        if (items == null || items.Count == 0 || string.IsNullOrWhiteSpace(productCode))
        {
            return new List<TaktBomMaterialCostItem>();
        }
        return items
            .Where(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, productCode))
            .ToList();
    }

    /// <summary>
    /// 建议替代价格：仅本产品明细 ExtField._bk.mp（无 scope 的明细回填履历），且 component_code=该行 ComponentCode；
    /// 禁止主表 product/model 履历（会串同机种其他产品组件）。
    /// </summary>
    /// <param name="productItems">本产品 Filter 明细</param>
    /// <param name="focusPeriod">基准月 yyyy-MM</param>
    /// <returns>组文</returns>
    private static string BuildSuggestedSubstitutePriceGroup(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string focusPeriod)
    {
        var map = new Dictionary<string, (string SourceCode, decimal OldUnitPrice, decimal NewUnitPrice)>(StringComparer.OrdinalIgnoreCase);
        var preferredPeriodByComponent = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in productItems)
        {
            var rowComponent = item.ComponentCode?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(rowComponent) || item.ComponentQuantity <= 0m)
            {
                continue;
            }
            foreach (var (component, source, oldUnitPrice, newUnitPrice, valuationPeriod) in EnumerateMpReplacePairsDetailed(item.ExtField))
            {
                // 只认本行组件的替换对，禁止同 ExtField 内串其它组件码
                if (!string.Equals(component, rowComponent, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                map[component] = (source, oldUnitPrice, newUnitPrice);
                if (!string.IsNullOrWhiteSpace(valuationPeriod))
                {
                    preferredPeriodByComponent[component] = valuationPeriod;
                }
            }
        }

        if (map.Count == 0)
        {
            return string.Empty;
        }

        var qtyByComponent = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        foreach (var component in map.Keys.ToList())
        {
            preferredPeriodByComponent.TryGetValue(component, out var preferred);
            var qty = ResolveComponentQuantityForSubstitute(
                productItems,
                component,
                preferred,
                focusPeriod);
            // 用量为 0：不进入建议替代价格
            if (qty <= 0m)
            {
                map.Remove(component);
                continue;
            }
            qtyByComponent[component] = qty;
        }

        if (map.Count == 0)
        {
            return string.Empty;
        }

        return FormatReplaceComponentGroup(map, qtyByComponent);
    }

    /// <summary>
    /// 解析建议替代价格用的组件用量：优先 valuation_period 月 → 基准月 → 任意月最大合计
    /// </summary>
    private static decimal ResolveComponentQuantityForSubstitute(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string componentCode,
        string? preferredPeriod,
        string focusPeriod)
    {
        if (!string.IsNullOrWhiteSpace(preferredPeriod))
        {
            var q = SumComponentQuantityInPeriod(productItems, componentCode, preferredPeriod!);
            if (q > 0m)
            {
                return q;
            }
        }
        if (!string.IsNullOrWhiteSpace(focusPeriod))
        {
            var q = SumComponentQuantityInPeriod(productItems, componentCode, focusPeriod);
            if (q > 0m)
            {
                return q;
            }
        }
        return productItems
            .Where(r => string.Equals(
                r.ComponentCode?.Trim(),
                componentCode,
                StringComparison.OrdinalIgnoreCase))
            .GroupBy(r => ToPeriodKey(r.CostingDate), StringComparer.Ordinal)
            .Select(g => SumComponentQuantityInPeriod(productItems, componentCode, g.Key))
            .Where(q => q > 0m)
            .DefaultIfEmpty(0m)
            .Max();
    }

    /// <summary>
    /// 指定月组件用量：末日快照 BuildComponentKey 去重合计；仍为 0 则该月同编码简单合计
    /// </summary>
    private static decimal SumComponentQuantityInPeriod(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string componentCode,
        string period)
    {
        var snap = ResolvePeriodSnapshotForProductItems(productItems, period);
        var fromSnap = snap
            .Where(r => string.Equals(
                r.ComponentCode?.Trim(),
                componentCode,
                StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.ComponentQuantity);
        if (fromSnap > 0m)
        {
            return fromSnap;
        }
        return productItems
            .Where(r => ToPeriodKey(r.CostingDate) == period
                && string.Equals(
                    r.ComponentCode?.Trim(),
                    componentCode,
                    StringComparison.OrdinalIgnoreCase))
            .Sum(r => r.ComponentQuantity);
    }

    /// <summary>
    /// 已按产品分组的明细：取期间最后核算日，再按 BuildComponentKey 去重（同键最大 Id）。
    /// 不再二次 ProductCodeMatches，避免 10/18 位或归一化写法不一致导致空快照、用量变 0。
    /// </summary>
    private static List<TaktBomMaterialCostItem> ResolvePeriodSnapshotForProductItems(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string period)
    {
        if (productItems == null || productItems.Count == 0 || string.IsNullOrWhiteSpace(period))
        {
            return new List<TaktBomMaterialCostItem>();
        }
        var periodKey = period.Trim();
        var periodRows = productItems
            .Where(r => ToPeriodKey(r.CostingDate) == periodKey)
            .ToList();
        if (periodRows.Count == 0)
        {
            return new List<TaktBomMaterialCostItem>();
        }
        var latestDay = periodRows.Max(r => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(r.CostingDate));
        return periodRows
            .Where(r => TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(r.CostingDate) == latestDay)
            .GroupBy(TaktBomMaterialCostItemLineCostHelper.BuildComponentKey, StringComparer.Ordinal)
            .Select(g => g.OrderByDescending(r => r.Id).First())
            .ToList();
    }

    /// <summary>
    /// 枚举单条 ExtField 中可用于「建议替代价格」的 _bk.mp 替换对（含单价与 valuation_period）
    /// </summary>
    private static IEnumerable<(string ComponentCode, string SourceCode, decimal OldUnitPrice, decimal NewUnitPrice, string ValuationPeriod)> EnumerateMpReplacePairsDetailed(
        string? extField)
    {
        if (string.IsNullOrWhiteSpace(extField))
        {
            yield break;
        }
        var root = TaktBomExtFieldBackfillHistoryHelper.ParseExtFieldObject(extField);
        if (root[TaktBomExtFieldBackfillHistoryHelper.ExtFieldBackfillRootKey] is not JsonObject bk)
        {
            yield break;
        }
        var mpNode = bk[TaktBomExtFieldBackfillHistoryHelper.ScopeMp];
        if (mpNode is JsonArray arr)
        {
            foreach (var node in arr)
            {
                if (TryReadMpReplacePair(node as JsonObject, out var pair))
                {
                    yield return pair;
                }
            }
            yield break;
        }
        if (TryReadMpReplacePair(mpNode as JsonObject, out var single))
        {
            yield return single;
        }
    }

    /// <summary>
    /// 读取 mp 履历中的建议替代对。
    /// 仅收录明细回填履历（无 scope）；主表 product_monthly_cost / model_monthly_average_cost 一律排除（防串产品）。
    /// </summary>
    private static bool TryReadMpReplacePair(
        JsonObject? entry,
        out (string ComponentCode, string SourceCode, decimal OldUnitPrice, decimal NewUnitPrice, string ValuationPeriod) pair)
    {
        pair = default;
        if (entry == null)
        {
            return false;
        }
        // 有 scope = 主表履历，禁止进入建议替代价格
        var scope = ReadJsonString(entry["scope"]);
        if (!string.IsNullOrWhiteSpace(scope))
        {
            return false;
        }
        var component = ReadJsonString(entry["component_code"]);
        var source = ReadJsonString(entry["source_component_code"]);
        if (string.IsNullOrWhiteSpace(component)
            || string.IsNullOrWhiteSpace(source)
            || string.Equals(component, source, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        var movingPrice = TryReadJsonDecimal(entry["moving_average_price"], out var mp) ? mp : 0m;
        var unit = 1;
        if (TryReadJsonInt(entry["moving_price_unit"], out var u) && u > 0)
        {
            unit = u;
        }
        var newUnitPrice = TaktBomMaterialCostItemLineCostHelper.RoundCost(movingPrice / unit);
        var oldMovingPrice = TryReadJsonDecimal(entry["old_moving_average_price"], out var oldMp) ? oldMp : 0m;
        var oldUnit = 1;
        if (TryReadJsonInt(entry["old_moving_price_unit"], out var oldU) && oldU > 0)
        {
            oldUnit = oldU;
        }
        var oldUnitPrice = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldMovingPrice / oldUnit);
        var valuationPeriod = ReadJsonString(entry["valuation_period"]);
        if (!string.IsNullOrWhiteSpace(valuationPeriod))
        {
            valuationPeriod = NormalizeCostingPeriodKey(valuationPeriod);
        }
        pair = (component, source, oldUnitPrice, newUnitPrice, valuationPeriod);
        return true;
    }

    /// <summary>
    /// 读取 JSON 字符串（兼容 JsonValue string / 非字符串 ToString）
    /// </summary>
    private static string ReadJsonString(JsonNode? node)
    {
        if (node == null)
        {
            return string.Empty;
        }
        if (node is JsonValue jv)
        {
            if (jv.TryGetValue<string>(out var s))
            {
                return s?.Trim() ?? string.Empty;
            }
            return jv.ToString().Trim().Trim('"');
        }
        return node.ToString()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 读取 JSON 数值为 decimal（兼容 number/string；JsonValue 常为 double）
    /// </summary>
    private static bool TryReadJsonDecimal(JsonNode? node, out decimal value)
    {
        value = 0m;
        if (node is not JsonValue jv)
        {
            return false;
        }
        if (jv.TryGetValue<decimal>(out value))
        {
            return true;
        }
        if (jv.TryGetValue<double>(out var d))
        {
            value = (decimal)d;
            return true;
        }
        if (jv.TryGetValue<long>(out var l))
        {
            value = l;
            return true;
        }
        if (jv.TryGetValue<int>(out var i))
        {
            value = i;
            return true;
        }
        return decimal.TryParse(
            jv.ToString(),
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out value);
    }

    /// <summary>
    /// 读取 JSON 整型（兼容 number/string）
    /// </summary>
    private static bool TryReadJsonInt(JsonNode? node, out int value)
    {
        value = 0;
        if (node is not JsonValue jv)
        {
            return false;
        }
        if (jv.TryGetValue<int>(out value))
        {
            return true;
        }
        if (jv.TryGetValue<long>(out var l) && l is >= int.MinValue and <= int.MaxValue)
        {
            value = (int)l;
            return true;
        }
        if (jv.TryGetValue<decimal>(out var dec))
        {
            value = (int)dec;
            return true;
        }
        return int.TryParse(
            jv.ToString(),
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out value);
    }

    /// <summary>
    /// 格式化建议替代价格：首行净额，明细全量列出
    /// </summary>
    private static string FormatReplaceComponentGroup(
        IReadOnlyDictionary<string, (string SourceCode, decimal OldUnitPrice, decimal NewUnitPrice)> map,
        IReadOnlyDictionary<string, decimal>? qtyByComponent)
    {
        if (map == null || map.Count == 0 || qtyByComponent == null)
        {
            return string.Empty;
        }
        var summary = 0m;
        var entries = new List<(string Text, decimal Diff)>();
        foreach (var kv in map)
        {
            if (!qtyByComponent.TryGetValue(kv.Key, out var qty) || qty <= 0m)
            {
                continue;
            }
            var oldAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(
                checked(kv.Value.OldUnitPrice * qty));
            var newAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(
                checked(kv.Value.NewUnitPrice * qty));
            var delta = TaktBomMaterialCostItemLineCostHelper.RoundCost(newAmount - oldAmount);
            summary = checked(summary + delta);
            entries.Add((
                $"{kv.Key}:{FormatQuantity(qty)}-{FormatMoney(oldAmount)}→{kv.Value.SourceCode}:{FormatQuantity(qty)}-{FormatMoney(newAmount)},Diff:{FormatMoney(delta)}",
                delta));
        }
        summary = TaktBomMaterialCostItemLineCostHelper.RoundCost(summary);
        return FormatFullGroup(entries, $"Net:{FormatSummaryCost(summary)}");
    }

    /// <summary>
    /// 关注月零价组件清单（QualifiesAsZeroPriceListLine；同一 ComponentCode 任一位置合格即入组）。
    /// 用量：期间最后核算日 + BuildComponentKey 去重后合计；用量≤0 不入组。
    /// </summary>
    /// <param name="productItems">该产品已 Filter 的明细</param>
    /// <param name="comparePeriod">关注月 yyyy-MM</param>
    /// <returns>组件编码与合并合计用量</returns>
    private static List<(string Code, decimal Qty)> CollectZeroPriceEntries(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string comparePeriod)
    {
        var snap = ResolvePeriodSnapshotForProductItems(productItems, comparePeriod);
        return snap
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode)
                && r.ComponentQuantity > 0m
                && TaktBomMaterialCostItemLineCostHelper.QualifiesAsZeroPriceListLine(r))
            .GroupBy(r => r.ComponentCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => (Code: g.Key, Qty: SumRowsQty(g.ToList())))
            .Where(x => x.Qty > 0m)
            .OrderBy(x => x.Code, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 0价格组：物料:用量:可替代物料:替代价格（末字母 Z→A 逆推；无可替代则仅物料:用量；用量≤0 跳过）
    /// </summary>
    private static string BuildZeroPriceGroupText(
        IReadOnlyList<(string Code, decimal Qty)> zeroEntries,
        IReadOnlyDictionary<string, (string SubstituteCode, decimal Price)> substituteByComponent)
    {
        var parts = new List<string>();
        foreach (var entry in zeroEntries)
        {
            if (entry.Qty <= 0m)
            {
                continue;
            }
            if (substituteByComponent.TryGetValue(entry.Code, out var sub))
            {
                parts.Add($"{entry.Code}:{FormatQuantity(entry.Qty)}:{sub.SubstituteCode}:{FormatMoney(sub.Price)}");
            }
            else
            {
                parts.Add($"{entry.Code}:{FormatQuantity(entry.Qty)}");
            }
        }
        return FormatGroup(parts);
    }

    /// <summary>
    /// 为零价组件按末字母逆推（C→B→A）：仅查移动价格表；优先关注月同 ValuationPeriod，否则取以前最近有价期间；展示价=MovingPrice÷PriceUnit；不查 cost_item
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">关注月 / 基准月 yyyy-MM</param>
    /// <param name="componentCodes">零价组件编码</param>
    /// <returns>原组件 → (可替代编码, 替代价)</returns>
    private async Task<Dictionary<string, (string SubstituteCode, decimal Price)>> ResolveSubstitutePricesAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyList<string> componentCodes)
    {
        var result = new Dictionary<string, (string SubstituteCode, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        if (componentCodes.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod))
        {
            return result;
        }
        var periodKey = costingPeriod.Trim();
        if (!TryParsePeriodMonth(periodKey, out var costingMonth))
        {
            return result;
        }
        var lookbackStart = costingMonth.AddMonths(-24).ToString("yyyy-MM");

        var candidateSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidatesByComponent = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var code in componentCodes)
        {
            var candidates = EnumeratePreviousLetterRevisions(code).ToList();
            candidatesByComponent[code] = candidates;
            foreach (var candidate in candidates)
            {
                foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(candidate))
                {
                    candidateSet.Add(variant);
                }
            }
        }
        if (candidateSet.Count == 0)
        {
            return result;
        }

        var samePeriodMoving = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        var lookbackMoving = new Dictionary<string, (string Period, decimal Price)>(StringComparer.OrdinalIgnoreCase);
        await FillMovingPriceLookupsAsync(
            plantCode, lookbackStart, periodKey, candidateSet.ToList(), samePeriodMoving, lookbackMoving);

        foreach (var code in componentCodes)
        {
            if (!candidatesByComponent.TryGetValue(code, out var list))
            {
                continue;
            }
            foreach (var candidate in list)
            {
                if (TryGetMovingPriceExactPeriod(samePeriodMoving, candidate, out var exactPrice))
                {
                    result[code] = (candidate, exactPrice);
                    break;
                }
                if (TryGetMovingPriceLookback(lookbackMoving, candidate, out var lookbackPrice))
                {
                    result[code] = (candidate, lookbackPrice);
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 加载移动价格：同期间（=关注月）与向前 24 个月回溯（仅 material_moving_price）
    /// </summary>
    private async Task FillMovingPriceLookupsAsync(
        string plantCode,
        string lookbackStart,
        string periodKey,
        IReadOnlyList<string> materialCodes,
        Dictionary<string, decimal> samePeriodMoving,
        Dictionary<string, (string Period, decimal Price)> lookbackMoving)
    {
        if (materialCodes.Count == 0)
        {
            return;
        }
        const int chunkSize = 200;
        for (var i = 0; i < materialCodes.Count; i += chunkSize)
        {
            var chunk = materialCodes.Skip(i).Take(chunkSize).ToList();
            Expression<Func<TaktMaterialMovingPrice, bool>> predicate = x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && chunk.Contains(x.MaterialCode)
                && x.MovingPrice > 0;
            var remaining = MovingPriceLookupMaxRows
                - Math.Max(samePeriodMoving.Count, lookbackMoving.Count);
            if (remaining <= 0)
            {
                break;
            }
            var part = await GetMovingPriceListForRangeAsync(predicate, lookbackStart, periodKey, remaining);
            foreach (var price in part)
            {
                if (string.IsNullOrWhiteSpace(price.MaterialCode) || price.MovingPrice <= 0m)
                {
                    continue;
                }
                var unitPrice = TaktBomMaterialCostItemLineCostHelper.ResolveMaterialMovingUnitPrice(price);
                if (unitPrice <= 0m)
                {
                    continue;
                }
                var ym = NormalizeCostingPeriodKey(price.ValuationPeriod);
                if (ym.Length == 0
                    || string.CompareOrdinal(ym, lookbackStart) < 0
                    || string.CompareOrdinal(ym, periodKey) > 0)
                {
                    continue;
                }
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(price.MaterialCode))
                {
                    if (string.Equals(ym, periodKey, StringComparison.Ordinal))
                    {
                        if (!samePeriodMoving.TryGetValue(key, out var existingSame)
                            || unitPrice > existingSame)
                        {
                            samePeriodMoving[key] = unitPrice;
                        }
                    }
                    if (!lookbackMoving.TryGetValue(key, out var existing)
                        || string.CompareOrdinal(ym, existing.Period) > 0
                        || (string.Equals(ym, existing.Period, StringComparison.Ordinal)
                            && unitPrice > existing.Price))
                    {
                        lookbackMoving[key] = (ym, unitPrice);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 取与关注月相同 ValuationPeriod 的移动价
    /// </summary>
    private static bool TryGetMovingPriceExactPeriod(
        IReadOnlyDictionary<string, decimal> samePeriodMoving,
        string materialCode,
        out decimal price)
    {
        price = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(materialCode))
        {
            if (!samePeriodMoving.TryGetValue(variant, out var hit) || hit <= 0m)
            {
                continue;
            }
            price = hit;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 取关注月及以前最近有价移动价
    /// </summary>
    private static bool TryGetMovingPriceLookback(
        IReadOnlyDictionary<string, (string Period, decimal Price)> lookbackMoving,
        string materialCode,
        out decimal price)
    {
        price = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(materialCode))
        {
            if (!lookbackMoving.TryGetValue(variant, out var hit) || hit.Price <= 0m)
            {
                continue;
            }
            price = hit.Price;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 加载指定核算月主表 ProductMonthlyCalculation（按产品编码；禁止 ModelMonthlyAverageCost）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算月 yyyy-MM</param>
    /// <param name="productCodes">产品/组件编码</param>
    /// <returns>编码变体 → ProductMonthlyCost</returns>
    private async Task<Dictionary<string, decimal>> LoadProductMonthlyCostByCodesAsync(
        string plantCode,
        string costingPeriod,
        IReadOnlyList<string> productCodes)
    {
        var result = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
        if (productCodes.Count == 0 || string.IsNullOrWhiteSpace(costingPeriod)
            || !TryParsePeriodMonth(costingPeriod, out var monthStart))
        {
            return result;
        }
        var monthEnd = monthStart.AddMonths(1);
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        const int chunkSize = 200;
        for (var i = 0; i < lookupCodes.Count; i += chunkSize)
        {
            var chunk = lookupCodes.Skip(i).Take(chunkSize).ToList();
            var part = await _bomMaterialCostRepository.GetListAsync(x =>
                x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.IsDeleted == 0
                && chunk.Contains(x.ProductCode)
                && x.ProductMonthlyCalculation > 0
                && (x.CostingPeriod == costingPeriod
                    || (x.CostingDate >= monthStart && x.CostingDate < monthEnd)));
            foreach (var header in part)
            {
                if (string.IsNullOrWhiteSpace(header.ProductCode) || header.ProductMonthlyCalculation <= 0m)
                {
                    continue;
                }
                foreach (var key in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(header.ProductCode))
                {
                    if (!result.TryGetValue(key, out var existing) || header.ProductMonthlyCalculation > existing)
                    {
                        result[key] = header.ProductMonthlyCalculation;
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 组件编码是否在主表有 ProductMonthlyCost
    /// </summary>
    /// <param name="costByCode">主表成本字典</param>
    /// <param name="componentCode">组件编码</param>
    /// <param name="cost">ProductMonthlyCost</param>
    /// <returns>有正价则为 true</returns>
    private static bool TryGetProductMonthlyCost(
        IReadOnlyDictionary<string, decimal> costByCode,
        string componentCode,
        out decimal cost)
    {
        cost = 0m;
        foreach (var variant in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(componentCode))
        {
            if (!costByCode.TryGetValue(variant, out var hit) || hit <= 0m)
            {
                continue;
            }
            cost = hit;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 构建价格差异组 + 组件差异组。
    /// 价格组按涨跌窗口截取，默认涨3跌3且差额达到 1 或 -1，某一侧没有则该侧为空。组件差异全量。单条仍是用量-行金额，Diff 为两侧行金额差。差异列不使用组内合计。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品编码</param>
    /// <param name="productItems">本产品明细</param>
    /// <param name="basePeriod">基准月</param>
    /// <param name="comparePeriod">比较月</param>
    /// <param name="impactWindow">涨跌明细窗口</param>
    /// <returns>价格组文、价格汇总、组件组文、组件汇总</returns>
    private static (string PriceText, decimal PriceSummary, string ComponentText, decimal ComponentSummary) BuildDeltaGroupTexts(
        string plantCode,
        string productCode,
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string basePeriod,
        string comparePeriod,
        ImpactLineWindow impactWindow)
    {
        // 价格/组件差异仅本产品明细，禁止串产品
        var scopedItems = FilterItemsBelongingToProduct(productItems, productCode);
        var baseMap = BuildPeriodComponentRowMap(scopedItems, basePeriod);
        var compareMap = BuildPeriodComponentRowMap(scopedItems, comparePeriod);

        var matchedBase = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var matchedCompare = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var sameCodeMatched = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // ① 编码完全相同 → 仅单价有差才进价格差异组（Diff=0 不写）
        foreach (var code in baseMap.Keys)
        {
            if (!compareMap.ContainsKey(code))
            {
                continue;
            }
            matchedBase.Add(code);
            matchedCompare.Add(code);
            sameCodeMatched.Add(code);
        }

        // 组件差异条目：(展示文案, Diff 金额=行成本差)；最终按 Diff 降序
        var componentEntries = new List<(string Text, decimal Diff)>();
        var newSum = 0m;
        var removeSum = 0m;
        var versionSum = 0m;
        // ② 末位版本字母 stem 相同、字母不同 → version
        foreach (var compareCode in compareMap.Keys.Where(c => !matchedCompare.Contains(c)).OrderBy(c => c, StringComparer.Ordinal))
        {
            if (!TrySplitComponentVersion(compareCode, out var compareStem, out var compareLetter))
            {
                continue;
            }
            var baseCandidate = baseMap.Keys
                .Where(c => !matchedBase.Contains(c))
                .Select(c =>
                {
                    var ok = TrySplitComponentVersion(c, out var stem, out var letter);
                    return (Code: c, Ok: ok, Stem: stem, Letter: letter);
                })
                .Where(x => x.Ok
                    && string.Equals(x.Stem, compareStem, StringComparison.OrdinalIgnoreCase)
                    && x.Letter != compareLetter)
                .OrderBy(x => x.Code, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();
            if (baseCandidate.Code == null)
            {
                continue;
            }
            matchedBase.Add(baseCandidate.Code);
            matchedCompare.Add(compareCode);
            var baseRows = baseMap[baseCandidate.Code];
            var compareRows = compareMap[compareCode];
            var baseQty = SumRowsQty(baseRows);
            var compareQty = SumRowsQty(compareRows);
            // 两侧用量均为 0：不进组件差异
            if (baseQty <= 0m && compareQty <= 0m)
            {
                continue;
            }
            var baseAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(baseRows));
            var compareAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(compareRows));
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareAmount - baseAmount);
            versionSum = checked(versionSum + lineDelta);
            componentEntries.Add((
                $"{baseCandidate.Code}:{FormatQuantity(baseQty)}-{FormatMoney(baseAmount)}→{compareCode}:{FormatQuantity(compareQty)}-{FormatMoney(compareAmount)}→version,Diff:{FormatMoney(lineDelta)}",
                lineDelta));
        }

        foreach (var c in baseMap.Keys.Where(x => !matchedBase.Contains(x)).OrderBy(x => x, StringComparer.Ordinal))
        {
            var rows = baseMap[c];
            var qty = SumRowsQty(rows);
            if (qty <= 0m)
            {
                continue;
            }
            var removedCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(rows));
            var lineDelta = TaktBomMaterialCostItemLineCostHelper.RoundCost(-removedCost);
            removeSum = checked(removeSum + removedCost);
            componentEntries.Add((
                $"{c}:{FormatQuantity(qty)}-{FormatMoney(removedCost)}→remove,Diff:{FormatMoney(lineDelta)}",
                lineDelta));
        }
        foreach (var c in compareMap.Keys.Where(x => !matchedCompare.Contains(x)).OrderBy(x => x, StringComparer.Ordinal))
        {
            var rows = compareMap[c];
            var qty = SumRowsQty(rows);
            if (qty <= 0m)
            {
                continue;
            }
            var addedCost = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(rows));
            var lineDelta = addedCost;
            newSum = checked(newSum + lineDelta);
            componentEntries.Add((
                $"{c}:{FormatQuantity(qty)}-{FormatMoney(addedCost)}→new,Diff:{FormatMoney(lineDelta)}",
                lineDelta));
        }

        newSum = TaktBomMaterialCostItemLineCostHelper.RoundCost(newSum);
        removeSum = TaktBomMaterialCostItemLineCostHelper.RoundCost(removeSum);
        versionSum = TaktBomMaterialCostItemLineCostHelper.RoundCost(versionSum);
        // 净值 = 新增 − 删除 + version 行差（与各条 Diff 之和一致）
        var componentSummary = TaktBomMaterialCostItemLineCostHelper.RoundCost(
            checked(newSum - removeSum + versionSum));

        // ③ 仅「编码完全相同」配对进价格差异组（不含 version 配对）。
        // 文案：编码:比较月用量-行金额→基准月用量-行金额。行金额=单价×用量；Diff=两侧行金额差（含用量变化）。
        var priceEntries = new List<(string Text, decimal Diff)>();
        var priceSummary = 0m;
        foreach (var code in sameCodeMatched)
        {
            if (!compareMap.TryGetValue(code, out var compareRows) || !baseMap.TryGetValue(code, out var baseRows))
            {
                continue;
            }
            var baseQty = SumRowsQty(baseRows);
            var compareQty = SumRowsQty(compareRows);
            if (baseQty <= 0m && compareQty <= 0m)
            {
                continue;
            }
            var baseAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(baseRows));
            var compareAmount = TaktBomMaterialCostItemLineCostHelper.RoundCost(SumRowsLineCost(compareRows));
            var deltaDisplay = TaktBomMaterialCostItemLineCostHelper.RoundCost(compareAmount - baseAmount);
            if (deltaDisplay == 0m)
            {
                continue;
            }
            priceSummary = checked(priceSummary + deltaDisplay);
            priceEntries.Add((
                $"{code}:{FormatQuantity(baseQty)}-{FormatMoney(baseAmount)}→{FormatQuantity(compareQty)}-{FormatMoney(compareAmount)},Diff:{FormatMoney(deltaDisplay)}",
                deltaDisplay));
        }
        priceSummary = TaktBomMaterialCostItemLineCostHelper.RoundCost(priceSummary);
        return (
            FormatImpactGroup(priceEntries, $"Net:{FormatSummaryCost(priceSummary)}", impactWindow),
            priceSummary,
            FormatComponentGroup(componentEntries, newSum, removeSum, componentSummary),
            componentSummary);
    }

    /// <summary>
    /// 期间最后核算日快照（BuildComponentKey 去重），按组件编码分组（多 BOM 位置保留多行）
    /// </summary>
    private static Dictionary<string, List<TaktBomMaterialCostItem>> BuildPeriodComponentRowMap(
        IReadOnlyList<TaktBomMaterialCostItem> productItems,
        string periodKey)
    {
        var snap = ResolvePeriodSnapshotForProductItems(productItems, periodKey);
        return snap
            .Where(r => !string.IsNullOrWhiteSpace(r.ComponentCode) && r.ComponentQuantity > 0m)
            .GroupBy(r => r.ComponentCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => (Code: g.Key, Rows: g.ToList()))
            .Where(x => SumRowsQty(x.Rows) > 0m)
            .ToDictionary(x => x.Code, x => x.Rows, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 参与成本统计的行成本合计（与 SumSnapshotCost 同口径：排除 PCB SECT 整树 + X + 标识空 + F）
    /// </summary>
    private static decimal SumRowsLineCost(IReadOnlyList<TaktBomMaterialCostItem> rows)
    {
        return TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(rows);
    }

    /// <summary>
    /// 成本明细合并后用量合计（调用方须已 ResolvePeriodSnapshot / BuildComponentKey 去重）
    /// </summary>
    private static decimal SumRowsQty(IReadOnlyList<TaktBomMaterialCostItem> rows)
        => rows.Sum(r => r.ComponentQuantity);

    private static TaktBomMaterialCostItem PickRepresentativeRow(IReadOnlyList<TaktBomMaterialCostItem> rows)
        => rows.OrderByDescending(r => r.CostingDate).ThenByDescending(r => r.Id).First();

    /// <summary>
    /// 拆组件版本：仅末位 A～Z 视为版本字母；无则返回 false（走 new/remove）
    /// </summary>
    private static bool TrySplitComponentVersion(string componentCode, out string stem, out char versionLetter)
    {
        stem = string.Empty;
        versionLetter = '\0';
        if (string.IsNullOrWhiteSpace(componentCode) || componentCode.Length < 2)
        {
            return false;
        }
        var code = componentCode.Trim();
        var last = char.ToUpperInvariant(code[^1]);
        if (last is < 'A' or > 'Z')
        {
            return false;
        }
        stem = code[..^1];
        versionLetter = last;
        return stem.Length > 0;
    }

    private static string FormatGroup(IReadOnlyList<string> parts)
    {
        if (parts.Count == 0)
        {
            return string.Empty;
        }
        return string.Join(", ", parts);
    }

    /// <summary>
    /// 全量组文案：首行结论，其后按差额绝对值降序列出全部明细。换行分隔。
    /// </summary>
    /// <param name="entries">条目文案与差额</param>
    /// <param name="header">首行结论</param>
    /// <returns>组展示串；无条目时空串</returns>
    private static string FormatFullGroup(
        IReadOnlyList<(string Text, decimal Diff)> entries,
        string header)
    {
        if (entries == null || entries.Count == 0 || string.IsNullOrWhiteSpace(header))
        {
            return string.Empty;
        }
        var lines = new List<string> { header };
        lines.AddRange(entries
            .OrderByDescending(e => Math.Abs(e.Diff))
            .ThenBy(e => e.Text, StringComparer.Ordinal)
            .Select(e => e.Text));
        return string.Join("\n", lines);
    }

    /// <summary>
    /// 价格差异组文案：首行净额。涨价差额降序且达到 1、跌价达到 -1，条数由涨跌窗口决定；某一侧没有则不写该侧，其余合成一行。全部则全量。换行分隔，导出自动换行。
    /// </summary>
    /// <param name="entries">条目文案与差额</param>
    /// <param name="header">首行净额</param>
    /// <param name="window">涨跌明细窗口</param>
    /// <returns>组展示串；无条目时空串</returns>
    private static string FormatImpactGroup(
        IReadOnlyList<(string Text, decimal Diff)> entries,
        string header,
        ImpactLineWindow window)
    {
        if (entries == null || entries.Count == 0 || string.IsNullOrWhiteSpace(header))
        {
            return string.Empty;
        }
        if (!window.ApplyThreshold)
        {
            return FormatFullGroup(entries, header);
        }
        var increaseIndexes = new HashSet<int>();
        foreach (var index in entries
            .Select((entry, index) => (entry, index))
            .Where(x => x.entry.Diff >= MinIncreaseDiff)
            .OrderByDescending(x => x.entry.Diff)
            .ThenBy(x => x.entry.Text, StringComparer.Ordinal)
            .Take(window.IncreaseTake)
            .Select(x => x.index))
        {
            increaseIndexes.Add(index);
        }
        var decreaseIndexes = new HashSet<int>();
        foreach (var index in entries
            .Select((entry, index) => (entry, index))
            .Where(x => x.entry.Diff <= MaxDecreaseDiff)
            .OrderBy(x => x.entry.Diff)
            .ThenBy(x => x.entry.Text, StringComparer.Ordinal)
            .Take(window.DecreaseTake)
            .Select(x => x.index))
        {
            decreaseIndexes.Add(index);
        }
        var lines = new List<string> { header };
        if (increaseIndexes.Count > 0)
        {
            lines.AddRange(entries
                .Select((entry, index) => (entry, index))
                .Where(x => increaseIndexes.Contains(x.index))
                .OrderByDescending(x => x.entry.Diff)
                .ThenBy(x => x.entry.Text, StringComparer.Ordinal)
                .Select(x => x.entry.Text));
        }
        if (decreaseIndexes.Count > 0)
        {
            lines.AddRange(entries
                .Select((entry, index) => (entry, index))
                .Where(x => decreaseIndexes.Contains(x.index))
                .OrderBy(x => x.entry.Diff)
                .ThenBy(x => x.entry.Text, StringComparer.Ordinal)
                .Select(x => x.entry.Text));
        }
        var rest = 0m;
        var restCount = 0;
        for (var i = 0; i < entries.Count; i++)
        {
            if (increaseIndexes.Contains(i) || decreaseIndexes.Contains(i))
            {
                continue;
            }
            restCount++;
            rest = checked(rest + entries[i].Diff);
        }
        if (restCount > 0)
        {
            lines.Add($"Remaining {restCount} items, Net:{FormatSummaryCost(rest)}");
        }
        return string.Join("\n", lines);
    }

    /// <summary>
    /// 组件差异：首行净值与新增、删除合计（Add / Remove），明细全量列出，不写 Remaining
    /// </summary>
    /// <param name="entries">条目文案与行差</param>
    /// <param name="newSum">→new 行成本合计</param>
    /// <param name="removeSum">→remove 行成本绝对值合计</param>
    /// <param name="netSummary">净值（新增−删除+version）</param>
    /// <returns>组展示串；无条目时空串</returns>
    private static string FormatComponentGroup(
        IReadOnlyList<(string Text, decimal Diff)> entries,
        decimal newSum,
        decimal removeSum,
        decimal netSummary)
    {
        var header =
            $"Net:{FormatSummaryCost(netSummary)},Add:{FormatSummaryCost(newSum)} Remove:{FormatSummaryCost(removeSum)}";
        return FormatFullGroup(entries, header);
    }

    private static string FormatQuantity(decimal qty)
    {
        if (qty == decimal.Truncate(qty))
        {
            return ((long)qty).ToString(CultureInfo.InvariantCulture);
        }
        return qty.ToString("0.#####", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 行金额 / Diff（两侧行金额差）/ 行成本：一律 RoundCost 5 位（禁止改成 2 位）
    /// </summary>
    private static string FormatMoney(decimal value)
        => FormatSummaryCost(value);

    /// <summary>
    /// Summary Var / 差异列展示（与 RoundCost 5 位一致）
    /// </summary>
    private static string FormatSummaryCost(decimal value)
        => TaktBomMaterialCostItemLineCostHelper.RoundCost(value)
            .ToString("0.#####", CultureInfo.InvariantCulture);

    /// <summary>
    /// 加载产品 BOM 明细：先拉全量展开，再 Filter（生产相关=X、PCB SECT 标识为空、采购类型=F）。
    /// 与零价格清单同口径；分块仅查询，Filter 合并后执行一次。
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCodes">产品编码</param>
    /// <param name="costingMonthStart">核算月初</param>
    /// <param name="costingMonthEnd">核算月末（含该月）</param>
    /// <returns>已 Filter 的明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadBomCostItemsForProductsAsync(
        string plantCode,
        IReadOnlyList<string> productCodes,
        DateTime costingMonthStart,
        DateTime costingMonthEnd)
    {
        var raw = new List<TaktBomMaterialCostItem>();
        const int chunkSize = 200;
        var costingExclusiveEnd = costingMonthEnd.AddMonths(1);
        var lookupCodes = productCodes
            .SelectMany(TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (lookupCodes.Count == 0)
        {
            return raw;
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
                && chunk.Contains(x.ProductCode)
                && x.CostingDate >= costingMonthStart
                && x.CostingDate < costingExclusiveEnd);
            var years = TaktYearShardTableHelper.ResolveYears(
                costingMonthStart,
                costingExclusiveEnd.AddDays(-1));
            foreach (var year in years)
            {
                var yearTable = await ResolveBomItemPhysicalTableAsync(year);
                var part = await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
                raw.AddRange(part);
            }
        }
        return TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(raw))
            .ToList();
    }

    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    private static string NormalizeProductKey(string productCode)
    {
        var trimmed = productCode.Trim();
        var normalized = TaktStringHelper.NormalizeSapNumericMaterialCode(trimmed);
        return string.IsNullOrEmpty(normalized) ? trimmed : normalized;
    }

    private static string ToPeriodKey(DateTime costingDate)
        => $"{costingDate.Year:D4}-{costingDate.Month:D2}";

    private static bool TryParsePeriodMonth(string period, out DateTime month)
    {
        return DateTime.TryParseExact(
            period.Trim() + "-01",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out month);
    }

    private static TaktBomPriceDeltaTrendResultDto EmptyResult(int pageIndex, int pageSize)
        => new()
        {
            Paged = TaktPagedResult<TaktBomPriceDeltaTrendDto>.Create(
                new List<TaktBomPriceDeltaTrendDto>(), 0, pageIndex, pageSize),
        };

    /// <summary>
    /// 按年分表查询移动价格（关注月及向前 24 个月）
    /// </summary>
    private async Task<List<TaktMaterialMovingPrice>> GetMovingPriceListForRangeAsync(
        Expression<Func<TaktMaterialMovingPrice, bool>> predicate,
        string? valuationPeriodStart,
        string? valuationPeriodEnd,
        int? maxRows = null)
    {
        var years = TaktYearShardTableHelper.ResolveYearsFromYyyyMmPeriod(valuationPeriodStart, valuationPeriodEnd);
        var result = new List<TaktMaterialMovingPrice>();
        var yearsNeedBase = new List<int>();
        foreach (var year in years)
        {
            var table = await ResolveMovingPricePhysicalTableAsync(year);
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
                var part = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining, table);
                result.AddRange(part);
            }
            else
            {
                var part = await _materialMovingPriceRepository.GetListAsync(predicate, table);
                result.AddRange(part);
            }
        }
        if (yearsNeedBase.Count == 0)
        {
            return result;
        }
        if (maxRows.HasValue && result.Count >= maxRows.Value)
        {
            return result;
        }
        List<TaktMaterialMovingPrice> basePart;
        if (maxRows.HasValue)
        {
            var remaining = maxRows.Value - result.Count;
            basePart = await _materialMovingPriceRepository.GetListForExportAsync(predicate, remaining);
        }
        else
        {
            basePart = await _materialMovingPriceRepository.GetListAsync(predicate);
        }
        if (yearsNeedBase.Count == years.Count)
        {
            result.AddRange(basePart);
        }
        else
        {
            var yearSet = yearsNeedBase.ToHashSet();
            result.AddRange(basePart.Where(r =>
                !string.IsNullOrWhiteSpace(r.ValuationPeriod)
                && r.ValuationPeriod.Length >= 4
                && int.TryParse(r.ValuationPeriod.AsSpan(0, 4), out var y)
                && yearSet.Contains(y)));
        }
        return result;
    }

    /// <summary>
    /// 解析移动价格物理表
    /// </summary>
    private async Task<string?> ResolveMovingPricePhysicalTableAsync(int year)
    {
        var table = TaktYearShardTableHelper.BuildYearTableName(MovingPriceYearShardBaseTable, year);
        return await _materialMovingPriceRepository.PhysicalTableExistsAsync(table) ? table : null;
    }

    /// <summary>
    /// 枚举组件编码末字母 Z→A 逆推版本（不含当前字母）
    /// </summary>
    /// <param name="componentCode">组件编码</param>
    /// <returns>按字母逆序的前一版本编码</returns>
    private static IEnumerable<string> EnumeratePreviousLetterRevisions(string componentCode)
    {
        if (string.IsNullOrWhiteSpace(componentCode))
        {
            yield break;
        }
        var code = componentCode.Trim();
        if (code.Length < 2)
        {
            yield break;
        }
        var last = code[^1];
        if (!char.IsAsciiLetter(last))
        {
            yield break;
        }
        var prefix = code[..^1];
        var min = char.IsUpper(last) ? 'A' : 'a';
        for (var c = (char)(last - 1); c >= min; c--)
        {
            yield return prefix + c;
        }
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSoPriceScrDeviationService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价在同生效周期 SCR（条件行/数量等级/价值等级）中不存在的清单
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// SO 价偏离 SCR 分析服务（读销售订单 + 销售条件记录及等级；与 CRUD 分离）
/// </summary>
public class TaktSoPriceScrDeviationService : TaktServiceBase, ITaktSoPriceScrDeviationService
{
    /// <summary>
    /// 折算单价比较容差
    /// </summary>
    private const decimal UnitPriceEpsilon = 0.00001m;

    /// <summary>
    /// 物料下拉最大返回条数（与前端大数据阈值对齐；配合 remote-search）
    /// </summary>
    private const int MaxMaterialOptions = 500;

    /// <summary>
    /// 供应商/客户下拉最大返回条数
    /// </summary>
    private const int MaxPartnerOptions = 500;

    /// <summary>
    /// 订单 Id IN 查询分片大小
    /// </summary>
    private const int OrderIdChunkSize = 500;

    private readonly ITaktCompanyRepository<TaktSalesOrder> _salesOrderRepository;
    private readonly ITaktCompanyRepository<TaktSalesOrderItem> _salesOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionRecord> _salesConditionRecordRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionRecordItem> _salesConditionRecordItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionRecordScaleQuantity> _scaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionRecordScaleValue> _scaleValueRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderRepository">销售订单仓储</param>
    /// <param name="salesOrderItemRepository">销售订单明细仓储</param>
    /// <param name="salesConditionRecordRepository">销售条件记录仓储</param>
    /// <param name="salesConditionRecordItemRepository">销售条件记录行仓储</param>
    /// <param name="scaleQuantityRepository">数量等级仓储</param>
    /// <param name="scaleValueRepository">价值等级仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSoPriceScrDeviationService(
        ITaktCompanyRepository<TaktSalesOrder> salesOrderRepository,
        ITaktCompanyRepository<TaktSalesOrderItem> salesOrderItemRepository,
        ITaktCompanyRepository<TaktSalesConditionRecord> salesConditionRecordRepository,
        ITaktCompanyRepository<TaktSalesConditionRecordItem> salesConditionRecordItemRepository,
        ITaktCompanyRepository<TaktSalesConditionRecordScaleQuantity> scaleQuantityRepository,
        ITaktCompanyRepository<TaktSalesConditionRecordScaleValue> scaleValueRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesOrderRepository = salesOrderRepository;
        _salesOrderItemRepository = salesOrderItemRepository;
        _salesConditionRecordRepository = salesConditionRecordRepository;
        _salesConditionRecordItemRepository = salesConditionRecordItemRepository;
        _scaleQuantityRepository = scaleQuantityRepository;
        _scaleValueRepository = scaleValueRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSoPriceScrDeviationPlantOptionsAsync(
        string? plantCode = null,
        string? keyword = null)
    {
        var list = await _salesOrderRepository.GetListAsync(x => true);
        var codes = list
            .Select(x => x.PlantCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (!string.IsNullOrWhiteSpace(plantCode))
        {
            codes = codes
                .Where(c => string.Equals(c, plantCode.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim();
            codes = codes
                .Where(c => c.Contains(kw, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        return codes
            .Select(c => new TaktSelectOption { DictLabel = c, DictValue = c })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSoPriceScrDeviationCustomerOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null)
    {
        if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
        {
            return new List<TaktSelectOption>();
        }
        var plant = plantCode.Trim();
        var (rangeStart, rangeEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(
            periodDateStart, periodDateEnd);
        var list = await _salesOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd
            && x.CustomerCode != null
            && x.CustomerCode != string.Empty);
        IEnumerable<TaktSalesOrder> filtered = list;
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim();
            filtered = list.Where(x =>
                x.CustomerCode.Contains(kw, StringComparison.OrdinalIgnoreCase)
                || x.CustomerName1.Contains(kw, StringComparison.OrdinalIgnoreCase));
        }
        return filtered
            .GroupBy(x => x.CustomerCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .Take(MaxPartnerOptions)
            .Select(g =>
            {
                var first = g.First();
                return new TaktSelectOption
                {
                    DictLabel = string.IsNullOrWhiteSpace(first.CustomerName1)
                        ? g.Key
                        : $"{g.Key} {first.CustomerName1}",
                    DictValue = g.Key,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetSoPriceScrDeviationMaterialOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        string? customerCode = null,
        DateTime? periodDateStart = null,
        DateTime? periodDateEnd = null)
    {
        if (string.IsNullOrWhiteSpace(plantCode) || periodDateStart == null || periodDateEnd == null)
        {
            return new List<TaktSelectOption>();
        }
        var plant = plantCode.Trim();
        var (rangeStart, rangeEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(
            periodDateStart, periodDateEnd);
        var orders = await _salesOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(customerCode))
        {
            var customer = customerCode.Trim();
            orders = orders
                .Where(x => string.Equals(x.CustomerCode, customer, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (orders.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var kw = keyword?.Trim() ?? string.Empty;
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var orderIds = orders.Select(x => x.Id).ToList();
        for (var offset = 0; offset < orderIds.Count; offset = checked(offset + OrderIdChunkSize))
        {
            var take = Math.Min(OrderIdChunkSize, orderIds.Count - offset);
            var chunkIds = orderIds.GetRange(offset, take);
            var items = await _salesOrderItemRepository.GetListAsync(
                x => chunkIds.Contains(x.SalesOrderId)
                    && x.IsObsolete == 0
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty);
            foreach (var item in items)
            {
                var code = item.MaterialCode!.Trim();
                if (string.IsNullOrEmpty(code))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(kw)
                    && !code.Contains(kw, StringComparison.OrdinalIgnoreCase)
                    && !(item.MaterialDescription?.Contains(kw, StringComparison.OrdinalIgnoreCase) ?? false))
                {
                    continue;
                }
                if (!map.ContainsKey(code))
                {
                    map[code] = item.MaterialDescription?.Trim() ?? string.Empty;
                }
                if (map.Count >= MaxMaterialOptions)
                {
                    break;
                }
            }
            if (map.Count >= MaxMaterialOptions)
            {
                break;
            }
        }
        return map
            .OrderBy(kv => kv.Key, StringComparer.OrdinalIgnoreCase)
            .Take(MaxMaterialOptions)
            .Select(kv => new TaktSelectOption
            {
                DictLabel = string.IsNullOrWhiteSpace(kv.Value) ? kv.Key : $"{kv.Key} {kv.Value}",
                DictValue = kv.Key,
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TaktSoPriceScrDeviationResultDto> GetSoPriceScrDeviationAnalysisAsync(
        TaktSoPriceScrDeviationQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildDeviationRowsAsync(queryDto);
        var pageRows = built.Rows.Skip(skip).Take(pageSize).ToList();
        return new TaktSoPriceScrDeviationResultDto
        {
            Paged = TaktPagedResult<TaktSoPriceScrDeviationDto>.Create(
                pageRows, built.Rows.Count, pageIndex, pageSize),
            DeviationCount = built.Rows.Count,
            NoScrCount = built.NoScrCount,
            PriceAbsentCount = built.PriceAbsentCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportSoPriceScrDeviationAnalysisAsync(
        TaktSoPriceScrDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildDeviationRowsAsync(query);
        var columnKeys = new[]
        {
            "plantCode", "salesOrderCode", "lineNumber", "orderDate", "pricingDate",
            "customerCode", "customerName", "materialCode", "materialDescription", "currencyCode",
            "orderQuantity", "soUnitPrice", "soPerUnit", "soNormalizedUnitPrice",
            "matchReason", "scrSalesConditionRecordCodes",
            "scrValidFrom", "scrValidTo", "scrPriceCount", "scrPricesText",
            "closestScrNormalizedUnitPrice", "deviationAmount", "deviationTrend",
        };
        var columnLabels = new[]
        {
            "工厂", "销售订单", "行号", "订单日期", "价格日期",
            "客户编码", "客户名称", "物料编码", "物料描述", "币种",
            "订购数量", "SO单价", "SO价格单位", "SO折算单价",
            "原因", "SCR定价记录号",
            "SCR有效起", "SCR有效止", "周期价目数", "周期内销售条件",
            "最近SCR折算单价", "偏离金额", "偏离方向",
        };
        var exportRows = built.Rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = row.PlantCode,
            ["salesOrderCode"] = row.SalesOrderCode,
            ["lineNumber"] = row.LineNumber,
            ["orderDate"] = row.OrderDate.Date,
            ["pricingDate"] = row.PricingDate.Date,
            ["customerCode"] = row.CustomerCode,
            ["customerName"] = row.CustomerName,
            ["materialCode"] = row.MaterialCode,
            ["materialDescription"] = row.MaterialDescription,
            ["currencyCode"] = row.CurrencyCode,
            ["orderQuantity"] = row.OrderQuantity,
            ["soUnitPrice"] = row.SoUnitPrice,
            ["soPerUnit"] = row.SoPerUnit,
            ["soNormalizedUnitPrice"] = row.SoNormalizedUnitPrice,
            ["matchReason"] = row.MatchReason switch
            {
                "no_scr" => "同周期无SCR",
                "price_absent" => "单价不在价目中",
                _ => row.MatchReason,
            },
            ["scrSalesConditionRecordCodes"] = row.ScrSalesConditionRecordCodes,
            ["scrValidFrom"] = row.ScrValidFrom?.Date,
            ["scrValidTo"] = row.ScrValidTo?.Date,
            ["scrPriceCount"] = row.ScrPriceCount,
            ["scrPricesText"] = row.ScrPricesText,
            ["closestScrNormalizedUnitPrice"] = row.ClosestScrNormalizedUnitPrice,
            ["deviationAmount"] = row.DeviationAmount,
            ["deviationTrend"] = row.DeviationTrend switch
            {
                "up" => "偏高",
                "down" => "偏低",
                _ => "—",
            },
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "SO价偏离SCR",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "SO价偏离SCR"),
            columnWidths: new Dictionary<string, double>(StringComparer.Ordinal)
            {
                ["plantCode"] = 10d,
                ["salesOrderCode"] = 14d,
                ["materialCode"] = 14d,
                ["soNormalizedUnitPrice"] = 12d,
                ["scrPricesText"] = 48d,
                ["deviationAmount"] = 12d,
            });
    }

    /// <summary>
    /// 构建命中行（SO 折算单价在同周期 SCR 价目中不存在）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>命中行与汇总</returns>
    private async Task<(List<TaktSoPriceScrDeviationDto> Rows, int NoScrCount, int PriceAbsentCount)> BuildDeviationRowsAsync(
        TaktSoPriceScrDeviationQueryDto queryDto)
    {
        if (string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            ThrowBusinessException("请选择工厂代码");
        }
        var plant = queryDto.PlantCode.Trim();
        var (rangeStart, rangeEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);

        var orders = await _salesOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            var supplier = queryDto.CustomerCode.Trim();
            orders = orders
                .Where(x => string.Equals(x.CustomerCode, supplier, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (orders.Count == 0)
        {
            return (new List<TaktSoPriceScrDeviationDto>(), 0, 0);
        }

        var orderById = orders.ToDictionary(x => x.Id);
        var orderIds = orderById.Keys.ToList();
        var items = await _salesOrderItemRepository.GetListAsync(
            x => orderIds.Contains(x.SalesOrderId) && x.IsObsolete == 0);
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            var material = queryDto.MaterialCode.Trim();
            items = items
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.MaterialCode)
                    && x.MaterialCode.Contains(material, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (items.Count == 0)
        {
            return (new List<TaktSoPriceScrDeviationDto>(), 0, 0);
        }

        var prices = await _salesConditionRecordRepository.GetListAsync(x => x.PlantCode == plant);
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            var priceType = queryDto.PriceType.Trim();
            prices = prices
                .Where(x => string.Equals(x.PriceType, priceType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var priceById = prices.ToDictionary(x => x.Id);
        var priceIds = priceById.Keys.ToList();
        var priceItems = priceIds.Count == 0
            ? new List<TaktSalesConditionRecordItem>()
            : await _salesConditionRecordItemRepository.GetListAsync(
                x => priceIds.Contains(x.SalesConditionRecordId) && x.IsObsolete == 0);
        var priceItemIds = priceItems.Select(x => x.Id).ToList();
        var scaleQuantities = priceItemIds.Count == 0
            ? new List<TaktSalesConditionRecordScaleQuantity>()
            : await _scaleQuantityRepository.GetListAsync(
                x => priceItemIds.Contains(x.SalesConditionRecordItemId) && x.IsObsolete == 0);
        var scaleValues = priceItemIds.Count == 0
            ? new List<TaktSalesConditionRecordScaleValue>()
            : await _scaleValueRepository.GetListAsync(
                x => priceItemIds.Contains(x.SalesConditionRecordItemId) && x.IsObsolete == 0);

        var priceItemsByPriceId = priceItems
            .GroupBy(x => x.SalesConditionRecordId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SalesConditionRecordSeq).ToList());
        var scaleQtyByItemId = scaleQuantities
            .GroupBy(x => x.SalesConditionRecordItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SalesScaleSeq).ToList());
        var scaleValByItemId = scaleValues
            .GroupBy(x => x.SalesConditionRecordItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.SalesScaleSeq).ToList());

        var pricesByCustomerMaterial = prices
            .GroupBy(
                x => $"{x.CustomerCode}\u001f{x.MaterialCode}",
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rows = new List<TaktSoPriceScrDeviationDto>();
        var noScrCount = 0;
        var priceAbsentCount = 0;

        foreach (var item in items)
        {
            if (!orderById.TryGetValue(item.SalesOrderId, out var order))
            {
                continue;
            }
            if (string.IsNullOrWhiteSpace(item.MaterialCode))
            {
                continue;
            }
            var pricingDate = order.OrderDate.Date;
            var soNorm = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(item.SalesUnitPrice, item.SalesPerUnit);
            var scrsInPeriod = ResolveScrsInPeriod(
                item,
                order,
                pricingDate,
                pricesByCustomerMaterial);
            var periodPrices = CollectPeriodPrices(
                scrsInPeriod,
                priceItemsByPriceId,
                scaleQtyByItemId,
                scaleValByItemId);

            if (periodPrices.Count > 0
                && periodPrices.Any(p => Math.Abs(p.NormalizedUnitPrice - soNorm) <= UnitPriceEpsilon))
            {
                // PO 折算单价已存在于同周期价目中 → 不列出
                continue;
            }

            var matchReason = periodPrices.Count == 0 ? "no_scr" : "price_absent";
            if (matchReason == "no_scr")
            {
                noScrCount = checked(noScrCount + 1);
            }
            else
            {
                priceAbsentCount = checked(priceAbsentCount + 1);
            }

            decimal? closest = null;
            decimal? deviation = null;
            var trend = "none";
            if (periodPrices.Count > 0)
            {
                closest = periodPrices
                    .OrderBy(p => Math.Abs(p.NormalizedUnitPrice - soNorm))
                    .Select(p => p.NormalizedUnitPrice)
                    .First();
                deviation = Math.Round(soNorm - closest.Value, 5, MidpointRounding.AwayFromZero);
                if (deviation > UnitPriceEpsilon)
                {
                    trend = "up";
                }
                else if (deviation < -UnitPriceEpsilon)
                {
                    trend = "down";
                }
                else
                {
                    trend = "none";
                }
            }

            rows.Add(new TaktSoPriceScrDeviationDto
            {
                PlantCode = order.PlantCode,
                SalesOrderCode = order.SalesOrderCode,
                LineNumber = item.LineNumber,
                OrderDate = order.OrderDate.Date,
                PricingDate = pricingDate,
                CustomerCode = order.CustomerCode,
                CustomerName = order.CustomerName1,
                MaterialCode = item.MaterialCode!,
                MaterialDescription = item.MaterialDescription,
                CurrencyCode = order.CurrencyCode,
                OrderQuantity = item.OrderQuantity,
                SoUnitPrice = item.SalesUnitPrice,
                SoPerUnit = item.SalesPerUnit,
                SoNormalizedUnitPrice = soNorm,
                MatchReason = matchReason,
                ScrPriceCount = periodPrices.Count,
                ScrPricesInPeriod = periodPrices,
                ScrPricesText = FormatPeriodPricesText(periodPrices),
                ScrSalesConditionRecordCodes = string.Join(
                    ";",
                    scrsInPeriod.Select(p => p.SalesConditionRecordCode).Distinct(StringComparer.OrdinalIgnoreCase)),
                ScrValidFrom = scrsInPeriod.Count == 0
                    ? null
                    : scrsInPeriod.Min(p => p.ValidFrom.Date),
                ScrValidTo = scrsInPeriod.Count == 0
                    ? null
                    : scrsInPeriod.Max(p => p.ValidTo.Date),
                ClosestScrNormalizedUnitPrice = closest,
                DeviationAmount = deviation,
                DeviationTrend = trend,
            });
        }

        rows = rows
            .OrderBy(x => x.MatchReason, StringComparer.Ordinal)
            .ThenByDescending(x => Math.Abs(x.DeviationAmount ?? 0m))
            .ThenBy(x => x.SalesOrderCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.LineNumber)
            .ToList();
        return (rows, noScrCount, priceAbsentCount);
    }

    /// <summary>
    /// 解析价格日期落在 ValidFrom～ValidTo 内的全部 SCR 头
    /// </summary>
    private static List<TaktSalesConditionRecord> ResolveScrsInPeriod(
        TaktSalesOrderItem item,
        TaktSalesOrder order,
        DateTime pricingDate,
        IReadOnlyDictionary<string, List<TaktSalesConditionRecord>> pricesByCustomerMaterial)
    {
        var key = $"{order.CustomerCode}\u001f{item.MaterialCode}";
        if (!pricesByCustomerMaterial.TryGetValue(key, out var byCm))
        {
            return new List<TaktSalesConditionRecord>();
        }
        return byCm
            .Where(p => p.ValidFrom.Date <= pricingDate && p.ValidTo.Date >= pricingDate)
            .OrderByDescending(p => p.ValidFrom)
            .ThenByDescending(p => p.Id)
            .ToList();
    }

    /// <summary>
    /// 收集同周期 SCR 的全部价目点（条件行 + 数量等级 + 价值等级）
    /// </summary>
    private static List<TaktSoPriceScrPeriodPriceDto> CollectPeriodPrices(
        IReadOnlyList<TaktSalesConditionRecord> pirs,
        IReadOnlyDictionary<long, List<TaktSalesConditionRecordItem>> priceItemsByPriceId,
        IReadOnlyDictionary<long, List<TaktSalesConditionRecordScaleQuantity>> scaleQtyByItemId,
        IReadOnlyDictionary<long, List<TaktSalesConditionRecordScaleValue>> scaleValByItemId)
    {
        var result = new List<TaktSoPriceScrPeriodPriceDto>();
        foreach (var pir in pirs)
        {
            if (!priceItemsByPriceId.TryGetValue(pir.Id, out var pirItems))
            {
                continue;
            }
            foreach (var pirItem in pirItems)
            {
                var unit = pirItem.PriceUnit <= 0 ? 1 : pirItem.PriceUnit;
                result.Add(new TaktSoPriceScrPeriodPriceDto
                {
                    SalesConditionRecordCode = pir.SalesConditionRecordCode,
                    PriceType = pir.PriceType,
                    ValidFrom = pir.ValidFrom.Date,
                    ValidTo = pir.ValidTo.Date,
                    PriceSource = "item",
                    SalesConditionRecordSeq = pirItem.SalesConditionRecordSeq,
                    Price = pirItem.Price,
                    PriceUnit = unit,
                    NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(pirItem.Price, unit),
                });
                if (scaleQtyByItemId.TryGetValue(pirItem.Id, out var qtyScales))
                {
                    foreach (var scale in qtyScales)
                    {
                        result.Add(new TaktSoPriceScrPeriodPriceDto
                        {
                            SalesConditionRecordCode = pir.SalesConditionRecordCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleQuantity",
                            SalesConditionRecordSeq = pirItem.SalesConditionRecordSeq,
                            SalesScaleSeq = scale.SalesScaleSeq,
                            ScaleQuantity = scale.ScaleQuantity,
                            Price = scale.Price,
                            PriceUnit = unit,
                            NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(scale.Price, unit),
                        });
                    }
                }
                if (scaleValByItemId.TryGetValue(pirItem.Id, out var valScales))
                {
                    foreach (var scale in valScales)
                    {
                        result.Add(new TaktSoPriceScrPeriodPriceDto
                        {
                            SalesConditionRecordCode = pir.SalesConditionRecordCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleValue",
                            SalesConditionRecordSeq = pirItem.SalesConditionRecordSeq,
                            SalesScaleSeq = scale.SalesScaleSeq,
                            ScaleValue = scale.ScaleValue,
                            Price = scale.Price,
                            PriceUnit = unit,
                            NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(scale.Price, unit),
                        });
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 周期价目展示文案
    /// </summary>
    /// <param name="prices">价目点</param>
    /// <returns>文案</returns>
    private static string FormatPeriodPricesText(IReadOnlyList<TaktSoPriceScrPeriodPriceDto> prices)
    {
        if (prices.Count == 0)
        {
            return string.Empty;
        }
        var builder = new StringBuilder();
        for (var i = 0; i < prices.Count; i++)
        {
            if (i > 0)
            {
                builder.Append(" | ");
            }
            var p = prices[i];
            var source = p.PriceSource switch
            {
                "scaleQuantity" => $"量≥{p.ScaleQuantity:0.####}",
                "scaleValue" => $"值≥{p.ScaleValue:0.####}",
                _ => "条件行",
            };
            builder.Append(CultureInfo.InvariantCulture, $"{p.SalesConditionRecordCode} {p.ValidFrom:yyyy-MM-dd}~{p.ValidTo:yyyy-MM-dd} {source} {p.NormalizedUnitPrice:0.00000}");
        }
        return builder.ToString();
    }
}

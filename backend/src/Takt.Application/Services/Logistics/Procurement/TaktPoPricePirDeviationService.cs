// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPoPricePirDeviationService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO 折算单价相对同生效周期 PIR 适用价（按订购数量取档）的偏离清单
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// PO 价偏离 PIR 分析服务（读采购订单 + 采购价格及等级；与 CRUD 分离）
/// </summary>
public class TaktPoPricePirDeviationService : TaktServiceBase, ITaktPoPricePirDeviationService
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

    private readonly ITaktCompanyRepository<TaktPurchaseOrder> _purchaseOrderRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfo> _purchaseInfoRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoItem> _purchaseInfoItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoScaleQuantity> _scaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoScaleValue> _scaleValueRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderRepository">采购订单仓储</param>
    /// <param name="purchaseOrderItemRepository">采购订单明细仓储</param>
    /// <param name="purchaseInfoRepository">采购价格仓储</param>
    /// <param name="purchaseInfoItemRepository">采购信息行仓储</param>
    /// <param name="scaleQuantityRepository">数量等级仓储</param>
    /// <param name="scaleValueRepository">价值等级仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPoPricePirDeviationService(
        ITaktCompanyRepository<TaktPurchaseOrder> purchaseOrderRepository,
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktCompanyRepository<TaktPurchaseInfo> purchaseInfoRepository,
        ITaktCompanyRepository<TaktPurchaseInfoItem> purchaseInfoItemRepository,
        ITaktCompanyRepository<TaktPurchaseInfoScaleQuantity> scaleQuantityRepository,
        ITaktCompanyRepository<TaktPurchaseInfoScaleValue> scaleValueRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _purchaseInfoRepository = purchaseInfoRepository;
        _purchaseInfoItemRepository = purchaseInfoItemRepository;
        _scaleQuantityRepository = scaleQuantityRepository;
        _scaleValueRepository = scaleValueRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPoPricePirDeviationPlantOptionsAsync(
        string? plantCode = null,
        string? keyword = null)
    {
        var list = await _purchaseOrderRepository.GetListAsync(x => true);
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
    public async Task<List<TaktSelectOption>> GetPoPricePirDeviationSupplierOptionsAsync(
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
        var list = await _purchaseOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd
            && x.SupplierCode != null
            && x.SupplierCode != string.Empty);
        IEnumerable<TaktPurchaseOrder> filtered = list;
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim();
            filtered = list.Where(x =>
                x.SupplierCode.Contains(kw, StringComparison.OrdinalIgnoreCase)
                || x.SupplierName1.Contains(kw, StringComparison.OrdinalIgnoreCase));
        }
        return filtered
            .GroupBy(x => x.SupplierCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .Take(MaxPartnerOptions)
            .Select(g =>
            {
                var first = g.First();
                return new TaktSelectOption
                {
                    DictLabel = string.IsNullOrWhiteSpace(first.SupplierName1)
                        ? g.Key
                        : $"{g.Key} {first.SupplierName1}",
                    DictValue = g.Key,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPoPricePirDeviationMaterialOptionsAsync(
        string? plantCode = null,
        string? keyword = null,
        string? supplierCode = null,
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
        var orders = await _purchaseOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(supplierCode))
        {
            var supplier = supplierCode.Trim();
            orders = orders
                .Where(x => string.Equals(x.SupplierCode, supplier, StringComparison.OrdinalIgnoreCase))
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
            var items = await _purchaseOrderItemRepository.GetListAsync(
                x => chunkIds.Contains(x.PurchaseOrderId)
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
    public async Task<TaktPoPricePirDeviationResultDto> GetPoPricePirDeviationAnalysisAsync(
        TaktPoPricePirDeviationQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildDeviationRowsAsync(queryDto);
        var pageRows = built.Rows.Skip(skip).Take(pageSize).ToList();
        return new TaktPoPricePirDeviationResultDto
        {
            Paged = TaktPagedResult<TaktPoPricePirDeviationDto>.Create(
                pageRows, built.Rows.Count, pageIndex, pageSize),
            DeviationCount = built.Rows.Count,
            NoPirCount = built.NoPirCount,
            PriceAbsentCount = built.PriceAbsentCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportPoPricePirDeviationAnalysisAsync(
        TaktPoPricePirDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildDeviationRowsAsync(query);
        var columnKeys = new[]
        {
            "plantCode", "purchaseOrderCode", "lineNumber", "orderDate",
            "supplierCode", "supplierName", "materialCode", "materialDescription",
            "orderQuantity", "poUnitPrice", "pirValidPeriod", "pirPricesText",
            "deviationAmount", "deviationPercent",
        };
        var columnLabels = new[]
        {
            "工厂", "采购订单", "行号", "订单日期",
            "供应商编码", "供应商名称", "物料编码", "物料描述",
            "数量", "PO单价", "PIR生效周期", "PIR价格",
            "价差", "价差比%",
        };
        var exportRows = built.Rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = row.PlantCode,
            ["purchaseOrderCode"] = row.PurchaseOrderCode,
            ["lineNumber"] = row.LineNumber,
            ["orderDate"] = row.OrderDate.Date,
            ["supplierCode"] = row.SupplierCode,
            ["supplierName"] = row.SupplierName,
            ["materialCode"] = row.MaterialCode,
            ["materialDescription"] = row.MaterialDescription,
            ["orderQuantity"] = row.OrderQuantity,
            ["poUnitPrice"] = row.PoUnitPrice,
            ["pirValidPeriod"] = row.PirValidPeriod,
            ["pirPricesText"] = row.PirPricesText,
            ["deviationAmount"] = row.DeviationAmount,
            ["deviationPercent"] = row.DeviationPercent.HasValue
                ? Math.Round(row.DeviationPercent.Value * 100m, 4, MidpointRounding.AwayFromZero)
                : null,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "PO价偏离PIR",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "PO价偏离PIR"),
            columnWidths: new Dictionary<string, double>(StringComparer.Ordinal)
            {
                ["plantCode"] = 10d,
                ["purchaseOrderCode"] = 14d,
                ["materialCode"] = 14d,
                ["poUnitPrice"] = 12d,
                ["pirPricesText"] = 48d,
                ["deviationAmount"] = 12d,
                ["deviationPercent"] = 12d,
            });
    }

    /// <summary>
    /// 构建命中行（PO 折算单价 ≠ 同周期 PIR 按订购数量解析的适用价）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>命中行与汇总</returns>
    private async Task<(List<TaktPoPricePirDeviationDto> Rows, int NoPirCount, int PriceAbsentCount)> BuildDeviationRowsAsync(
        TaktPoPricePirDeviationQueryDto queryDto)
    {
        if (string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            ThrowBusinessException("请选择工厂代码");
        }
        var plant = queryDto.PlantCode.Trim();
        var (rangeStart, rangeEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);

        var orders = await _purchaseOrderRepository.GetListAsync(x =>
            x.PlantCode == plant
            && x.OrderDate >= rangeStart
            && x.OrderDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            var supplier = queryDto.SupplierCode.Trim();
            orders = orders
                .Where(x => string.Equals(x.SupplierCode, supplier, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (orders.Count == 0)
        {
            return (new List<TaktPoPricePirDeviationDto>(), 0, 0);
        }

        var orderById = orders.ToDictionary(x => x.Id);
        var orderIds = orderById.Keys.ToList();
        var items = await _purchaseOrderItemRepository.GetListAsync(
            x => orderIds.Contains(x.PurchaseOrderId) && x.IsObsolete == 0);
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
            return (new List<TaktPoPricePirDeviationDto>(), 0, 0);
        }

        var prices = await _purchaseInfoRepository.GetListAsync(x => x.PlantCode == plant);
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
            ? new List<TaktPurchaseInfoItem>()
            : await _purchaseInfoItemRepository.GetListAsync(
                x => priceIds.Contains(x.PurchaseInfoId) && x.IsObsolete == 0);
        var priceItemIds = priceItems.Select(x => x.Id).ToList();
        var scaleQuantities = priceItemIds.Count == 0
            ? new List<TaktPurchaseInfoScaleQuantity>()
            : await _scaleQuantityRepository.GetListAsync(
                x => priceItemIds.Contains(x.PurchaseInfoItemId) && x.IsObsolete == 0);
        var scaleValues = priceItemIds.Count == 0
            ? new List<TaktPurchaseInfoScaleValue>()
            : await _scaleValueRepository.GetListAsync(
                x => priceItemIds.Contains(x.PurchaseInfoItemId) && x.IsObsolete == 0);

        var priceItemsByPriceId = priceItems
            .GroupBy(x => x.PurchaseInfoId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseInfoSeq).ToList());
        var scaleQtyByItemId = scaleQuantities
            .GroupBy(x => x.PurchaseInfoItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());
        var scaleValByItemId = scaleValues
            .GroupBy(x => x.PurchaseInfoItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());

        var pricesByCode = prices
            .GroupBy(x => x.PurchaseInfoCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var pricesBySupplierMaterial = prices
            .GroupBy(
                x => $"{x.SupplierCode}\u001f{x.MaterialCode}",
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rows = new List<TaktPoPricePirDeviationDto>();

        foreach (var item in items)
        {
            if (!orderById.TryGetValue(item.PurchaseOrderId, out var order))
            {
                continue;
            }
            if (string.IsNullOrWhiteSpace(item.MaterialCode))
            {
                continue;
            }
            var pricingDate = (item.PricingDate ?? order.OrderDate).Date;
            var poNorm = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(item.PurchaseUnitPrice, item.PurchasePerUnit);
            var pirsInPeriod = ResolvePirsInPeriod(
                item,
                order,
                pricingDate,
                pricesByCode,
                pricesBySupplierMaterial);
            // 与 BOM 采购价回填同规则：按订购数量取适用档位价，再折算为单位单价
            var applicablePrices = CollectApplicablePeriodPrices(
                pirsInPeriod,
                item.OrderQuantity,
                priceItemsByPriceId,
                scaleQtyByItemId,
                scaleValByItemId);
            // 展示用：生效周期内全部价目（条件行 + 数量/价值等级）
            var periodPrices = CollectPeriodPrices(
                pirsInPeriod,
                priceItemsByPriceId,
                scaleQtyByItemId,
                scaleValByItemId);

            if (applicablePrices.Count > 0
                && applicablePrices.Any(p => Math.Abs(p.NormalizedUnitPrice - poNorm) <= UnitPriceEpsilon))
            {
                continue;
            }

            var matchReason = applicablePrices.Count == 0 ? "no_pir" : "price_absent";

            decimal? closest = null;
            decimal? deviation = null;
            decimal? deviationPercent = null;
            var trend = "none";
            if (applicablePrices.Count > 0)
            {
                closest = applicablePrices
                    .OrderBy(p => Math.Abs(p.NormalizedUnitPrice - poNorm))
                    .ThenByDescending(p => p.ValidFrom)
                    .Select(p => p.NormalizedUnitPrice)
                    .First();
                deviation = Math.Round(poNorm - closest.Value, 5, MidpointRounding.AwayFromZero);
                if (Math.Abs(closest.Value) > UnitPriceEpsilon)
                {
                    deviationPercent = Math.Round(
                        (poNorm - closest.Value) / closest.Value,
                        6,
                        MidpointRounding.AwayFromZero);
                }
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

            DateTime? pirFrom = pirsInPeriod.Count == 0
                ? null
                : pirsInPeriod.Min(p => p.ValidFrom.Date);
            DateTime? pirTo = pirsInPeriod.Count == 0
                ? null
                : pirsInPeriod.Max(p => p.ValidTo.Date);
            var pirValidPeriod = pirFrom.HasValue && pirTo.HasValue
                ? $"{pirFrom.Value:yyyy-MM-dd}~{pirTo.Value:yyyy-MM-dd}"
                : string.Empty;

            rows.Add(new TaktPoPricePirDeviationDto
            {
                PlantCode = order.PlantCode,
                PurchaseOrderCode = order.PurchaseOrderCode,
                LineNumber = item.LineNumber,
                OrderDate = order.OrderDate.Date,
                PricingDate = pricingDate,
                SupplierCode = order.SupplierCode,
                SupplierName = order.SupplierName1,
                MaterialCode = item.MaterialCode!,
                MaterialDescription = item.MaterialDescription,
                CurrencyCode = order.CurrencyCode,
                OrderQuantity = item.OrderQuantity,
                PoUnitPrice = item.PurchaseUnitPrice,
                PoPerUnit = item.PurchasePerUnit,
                PoNormalizedUnitPrice = poNorm,
                PurchaseInfoCode = item.PurchaseInfoCode,
                MatchReason = matchReason,
                PirPriceCount = periodPrices.Count,
                PirPricesInPeriod = periodPrices,
                PirPricesText = FormatPeriodPricesText(periodPrices),
                PirPurchaseInfoCodes = string.Join(
                    ";",
                    pirsInPeriod.Select(p => p.PurchaseInfoCode).Distinct(StringComparer.OrdinalIgnoreCase)),
                PirValidFrom = pirFrom,
                PirValidTo = pirTo,
                PirValidPeriod = pirValidPeriod,
                ClosestPirNormalizedUnitPrice = closest,
                DeviationAmount = deviation,
                DeviationPercent = deviationPercent,
                DeviationTrend = trend,
            });
        }

        rows = rows
            .OrderBy(x => x.MatchReason, StringComparer.Ordinal)
            .ThenByDescending(x => Math.Abs(x.DeviationAmount ?? 0m))
            .ThenBy(x => x.PurchaseOrderCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.LineNumber)
            .ToList();

        rows = ApplyTrendFilter(rows, queryDto.TrendFilter);
        var filteredNoPir = rows.Count(x => string.Equals(x.MatchReason, "no_pir", StringComparison.Ordinal));
        var filteredAbsent = rows.Count(x => string.Equals(x.MatchReason, "price_absent", StringComparison.Ordinal));
        return (rows, filteredNoPir, filteredAbsent);
    }

    /// <summary>
    /// 按涨跌方向筛选（空=全部；changed=up|down；up；down）
    /// </summary>
    private static List<TaktPoPricePirDeviationDto> ApplyTrendFilter(
        List<TaktPoPricePirDeviationDto> rows,
        string? trendFilter)
    {
        var filter = trendFilter?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(filter))
        {
            return rows;
        }
        if (string.Equals(filter, "changed", StringComparison.OrdinalIgnoreCase))
        {
            return rows
                .Where(x =>
                    string.Equals(x.DeviationTrend, "up", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(x.DeviationTrend, "down", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        if (string.Equals(filter, "up", StringComparison.OrdinalIgnoreCase)
            || string.Equals(filter, "down", StringComparison.OrdinalIgnoreCase))
        {
            return rows
                .Where(x => string.Equals(x.DeviationTrend, filter, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x =>
                    string.Equals(filter, "up", StringComparison.OrdinalIgnoreCase)
                        ? (x.DeviationAmount ?? 0m)
                        : -(x.DeviationAmount ?? 0m))
                .ThenBy(x => x.PurchaseOrderCode, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.LineNumber)
                .ToList();
        }
        return rows;
    }

    /// <summary>
    /// 解析价格日期落在 ValidFrom～ValidTo 内的 PIR 头。
    /// 优先按行上采购信息编码；该编码在周期内无命中时回退供应商+物料。
    /// </summary>
    private static List<TaktPurchaseInfo> ResolvePirsInPeriod(
        TaktPurchaseOrderItem item,
        TaktPurchaseOrder order,
        DateTime pricingDate,
        IReadOnlyDictionary<string, List<TaktPurchaseInfo>> pricesByCode,
        IReadOnlyDictionary<string, List<TaktPurchaseInfo>> pricesBySupplierMaterial)
    {
        static List<TaktPurchaseInfo> FilterInPeriod(
            IEnumerable<TaktPurchaseInfo> source,
            DateTime day)
        {
            return source
                .Where(p => p.ValidFrom.Date <= day && p.ValidTo.Date >= day)
                .OrderByDescending(p => p.ValidFrom)
                .ThenByDescending(p => p.Id)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(item.PurchaseInfoCode)
            && pricesByCode.TryGetValue(item.PurchaseInfoCode.Trim(), out var byCode))
        {
            var byCodeInPeriod = FilterInPeriod(byCode, pricingDate);
            if (byCodeInPeriod.Count > 0)
            {
                return byCodeInPeriod;
            }
        }

        var key = $"{order.SupplierCode}\u001f{item.MaterialCode}";
        if (!pricesBySupplierMaterial.TryGetValue(key, out var bySm))
        {
            return new List<TaktPurchaseInfo>();
        }
        return FilterInPeriod(bySm, pricingDate);
    }

    /// <summary>
    /// 收集同周期 PIR 的全部价目点（条件行 + 数量等级 + 价值等级；用于展示）
    /// </summary>
    private static List<TaktPoPricePirPeriodPriceDto> CollectPeriodPrices(
        IReadOnlyList<TaktPurchaseInfo> pirs,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoItem>> priceItemsByPriceId,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoScaleQuantity>> scaleQtyByItemId,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoScaleValue>> scaleValByItemId)
    {
        var result = new List<TaktPoPricePirPeriodPriceDto>();
        foreach (var pir in pirs)
        {
            if (!priceItemsByPriceId.TryGetValue(pir.Id, out var pirItems))
            {
                continue;
            }
            foreach (var pirItem in pirItems)
            {
                var unit = pirItem.PriceUnit <= 0 ? 1 : pirItem.PriceUnit;
                result.Add(new TaktPoPricePirPeriodPriceDto
                {
                    PurchaseInfoCode = pir.PurchaseInfoCode,
                    PriceType = pir.PriceType,
                    ValidFrom = pir.ValidFrom.Date,
                    ValidTo = pir.ValidTo.Date,
                    PriceSource = "item",
                    PurchaseInfoSeq = pirItem.PurchaseInfoSeq,
                    Price = pirItem.Price,
                    PriceUnit = unit,
                    NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(pirItem.Price, unit),
                });
                if (scaleQtyByItemId.TryGetValue(pirItem.Id, out var qtyScales))
                {
                    foreach (var scale in qtyScales)
                    {
                        result.Add(new TaktPoPricePirPeriodPriceDto
                        {
                            PurchaseInfoCode = pir.PurchaseInfoCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleQuantity",
                            PurchaseInfoSeq = pirItem.PurchaseInfoSeq,
                            PurchaseScaleSeq = scale.PurchaseScaleSeq,
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
                        result.Add(new TaktPoPricePirPeriodPriceDto
                        {
                            PurchaseInfoCode = pir.PurchaseInfoCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleValue",
                            PurchaseInfoSeq = pirItem.PurchaseInfoSeq,
                            PurchaseScaleSeq = scale.PurchaseScaleSeq,
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
    /// 收集同周期各 PIR 按订购数量解析的适用折算单价（与 BOM 采购价回填同档规则）
    /// </summary>
    private static List<TaktPoPricePirPeriodPriceDto> CollectApplicablePeriodPrices(
        IReadOnlyList<TaktPurchaseInfo> pirs,
        decimal orderQuantity,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoItem>> priceItemsByPriceId,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoScaleQuantity>> scaleQtyByItemId,
        IReadOnlyDictionary<long, List<TaktPurchaseInfoScaleValue>> scaleValByItemId)
    {
        var emptyQty = (IReadOnlyList<TaktPurchaseInfoScaleQuantity>)Array.Empty<TaktPurchaseInfoScaleQuantity>();
        var emptyVal = (IReadOnlyList<TaktPurchaseInfoScaleValue>)Array.Empty<TaktPurchaseInfoScaleValue>();
        var result = new List<TaktPoPricePirPeriodPriceDto>();
        foreach (var pir in pirs)
        {
            if (!priceItemsByPriceId.TryGetValue(pir.Id, out var pirItems) || pirItems.Count == 0)
            {
                continue;
            }
            var pirItem = TaktBomCalculatePurchaseInfoHelper.ResolveActiveItem(pirItems);
            if (pirItem == null)
            {
                continue;
            }
            var qtyRows = scaleQtyByItemId.TryGetValue(pirItem.Id, out var qtyScales)
                ? (IReadOnlyList<TaktPurchaseInfoScaleQuantity>)qtyScales
                : emptyQty;
            var valRows = scaleValByItemId.TryGetValue(pirItem.Id, out var valScales)
                ? (IReadOnlyList<TaktPurchaseInfoScaleValue>)valScales
                : emptyVal;
            var rawPrice = TaktBomCalculatePurchaseInfoHelper.ResolveNetPrice(
                pirItem,
                qtyRows,
                valRows,
                orderQuantity);
            var unit = pirItem.PriceUnit <= 0 ? 1 : pirItem.PriceUnit;
            var (priceSource, scaleQty, scaleVal, scaleSeq) = ResolveApplicablePriceSource(
                pirItem,
                qtyRows,
                valRows,
                orderQuantity);
            result.Add(new TaktPoPricePirPeriodPriceDto
            {
                PurchaseInfoCode = pir.PurchaseInfoCode,
                PriceType = pir.PriceType,
                ValidFrom = pir.ValidFrom.Date,
                ValidTo = pir.ValidTo.Date,
                PriceSource = priceSource,
                PurchaseInfoSeq = pirItem.PurchaseInfoSeq,
                PurchaseScaleSeq = scaleSeq,
                ScaleQuantity = scaleQty,
                ScaleValue = scaleVal,
                Price = rawPrice,
                PriceUnit = unit,
                NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(rawPrice, unit),
            });
        }
        return result;
    }

    /// <summary>
    /// 标注适用价来源（条件行 / 数量等级 / 价值等级）
    /// </summary>
    private static (string Source, decimal? ScaleQuantity, decimal? ScaleValue, int? ScaleSeq) ResolveApplicablePriceSource(
        TaktPurchaseInfoItem item,
        IReadOnlyList<TaktPurchaseInfoScaleQuantity> quantityScales,
        IReadOnlyList<TaktPurchaseInfoScaleValue> valueScales,
        decimal orderQuantity)
    {
        var basis = item.ScaleBasis?.Trim() ?? string.Empty;
        var preferQuantity = string.Equals(
                basis,
                TaktBomCalculatePurchaseInfoHelper.QuantityScaleBasis,
                StringComparison.OrdinalIgnoreCase)
            || (!string.Equals(
                    basis,
                    TaktBomCalculatePurchaseInfoHelper.ValueScaleBasis,
                    StringComparison.OrdinalIgnoreCase)
                && quantityScales.Count > 0);
        if (preferQuantity && quantityScales.Count > 0)
        {
            var matched = quantityScales
                .Where(s => s.ScaleQuantity <= orderQuantity)
                .OrderByDescending(s => s.ScaleQuantity)
                .FirstOrDefault()
                ?? quantityScales.OrderBy(s => s.ScaleQuantity).First();
            return ("scaleQuantity", matched.ScaleQuantity, null, matched.PurchaseScaleSeq);
        }
        if (valueScales.Count > 0)
        {
            var matched = valueScales
                .Where(s => s.ScaleValue <= orderQuantity)
                .OrderByDescending(s => s.ScaleValue)
                .FirstOrDefault()
                ?? valueScales.OrderBy(s => s.ScaleValue).First();
            return ("scaleValue", null, matched.ScaleValue, matched.PurchaseScaleSeq);
        }
        return ("item", null, null, null);
    }

    /// <summary>
    /// 周期适用价目展示文案
    /// </summary>
    /// <param name="prices">价目点</param>
    /// <returns>文案</returns>
    private static string FormatPeriodPricesText(IReadOnlyList<TaktPoPricePirPeriodPriceDto> prices)
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
            builder.Append(CultureInfo.InvariantCulture, $"{p.PurchaseInfoCode} {p.ValidFrom:yyyy-MM-dd}~{p.ValidTo:yyyy-MM-dd} {source} {p.NormalizedUnitPrice:0.00000}");
        }
        return builder.ToString();
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceDeviationService.cs
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价偏离（PO/MIRO 折算单价相对同生效周期适用价，按数量取档）
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
/// 采购价偏离 分析服务（读采购订单/物料凭证 + 采购价格及等级；与 CRUD 分离）
/// </summary>
public class TaktPurchasePriceDeviationService : TaktServiceBase, ITaktPurchasePriceDeviationService
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
    private readonly ITaktCompanyRepository<TaktPurchaseCondition> _purchaseConditionRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseConditionItem> _purchaseConditionItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseConditionScaleQuantity> _scaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseConditionScaleValue> _scaleValueRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocument> _materialDocumentRepository;
    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _materialDocumentItemRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderRepository">采购订单仓储</param>
    /// <param name="purchaseOrderItemRepository">采购订单明细仓储</param>
    /// <param name="purchaseConditionRepository">采购价格仓储</param>
    /// <param name="purchaseConditionItemRepository">采购条件行仓储</param>
    /// <param name="scaleQuantityRepository">数量等级仓储</param>
    /// <param name="scaleValueRepository">价值等级仓储</param>
    /// <param name="materialDocumentRepository">物料凭证主表仓储</param>
    /// <param name="materialDocumentItemRepository">物料凭证行仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceDeviationService(
        ITaktCompanyRepository<TaktPurchaseOrder> purchaseOrderRepository,
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktCompanyRepository<TaktPurchaseCondition> purchaseConditionRepository,
        ITaktCompanyRepository<TaktPurchaseConditionItem> purchaseConditionItemRepository,
        ITaktCompanyRepository<TaktPurchaseConditionScaleQuantity> scaleQuantityRepository,
        ITaktCompanyRepository<TaktPurchaseConditionScaleValue> scaleValueRepository,
        ITaktCompanyRepository<TaktMaterialDocument> materialDocumentRepository,
        ITaktCompanyRepository<TaktMaterialDocumentItem> materialDocumentItemRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _purchaseConditionRepository = purchaseConditionRepository;
        _purchaseConditionItemRepository = purchaseConditionItemRepository;
        _scaleQuantityRepository = scaleQuantityRepository;
        _scaleValueRepository = scaleValueRepository;
        _materialDocumentRepository = materialDocumentRepository;
        _materialDocumentItemRepository = materialDocumentItemRepository;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchasePriceDeviationPlantOptionsAsync(
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
    public async Task<List<TaktSelectOption>> GetPurchasePriceDeviationSupplierOptionsAsync(
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
    public async Task<List<TaktSelectOption>> GetPurchasePriceDeviationMaterialOptionsAsync(
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
    public async Task<TaktPurchasePriceDeviationResultDto> GetPurchasePriceDeviationAnalysisAsync(
        TaktPurchasePriceDeviationQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildPurchasePriceDeviationRowsAsync(queryDto);
        var pageRows = built.Rows.Skip(skip).Take(pageSize).ToList();
        return new TaktPurchasePriceDeviationResultDto
        {
            Paged = TaktPagedResult<TaktPurchasePriceDeviationDto>.Create(
                pageRows, built.Rows.Count, pageIndex, pageSize),
            DeviationCount = built.Rows.Count,
            NoPirCount = built.NoPirCount,
            PriceAbsentCount = built.PriceAbsentCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceDeviationAnalysisAsync(
        TaktPurchasePriceDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildPurchasePriceDeviationRowsAsync(query);
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
            "数量", "PO单价", "PCR生效周期", "PCR价格",
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
            sheetName ?? "采购价偏离",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "采购价偏离"),
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
    private async Task<(List<TaktPurchasePriceDeviationDto> Rows, int NoPirCount, int PriceAbsentCount)> BuildPurchasePriceDeviationRowsAsync(
        TaktPurchasePriceDeviationQueryDto queryDto)
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
            return (new List<TaktPurchasePriceDeviationDto>(), 0, 0);
        }

        var orderById = orders.ToDictionary(x => x.Id);
        var orderIds = orderById.Keys.ToList();
        var items = await LoadInChunksAsync(
            orderIds,
            chunk => _purchaseOrderItemRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseOrderId) && x.IsObsolete == 0));
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
            return (new List<TaktPurchasePriceDeviationDto>(), 0, 0);
        }

        var prices = await _purchaseConditionRepository.GetListAsync(x => x.PlantCode == plant);
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            var priceType = queryDto.PriceType.Trim();
            prices = prices
                .Where(x => string.Equals(x.PriceType, priceType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var priceById = prices.ToDictionary(x => x.Id);
        var priceIds = priceById.Keys.ToList();
        var priceItems = await LoadInChunksAsync(
            priceIds,
            chunk => _purchaseConditionItemRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionId) && x.IsObsolete == 0));
        var priceItemIds = priceItems.Select(x => x.Id).ToList();
        var scaleQuantities = await LoadInChunksAsync(
            priceItemIds,
            chunk => _scaleQuantityRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionItemId) && x.IsObsolete == 0));
        var scaleValues = await LoadInChunksAsync(
            priceItemIds,
            chunk => _scaleValueRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionItemId) && x.IsObsolete == 0));

        var priceItemsByPriceId = priceItems
            .GroupBy(x => x.PurchaseConditionId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseConditionSeq).ToList());
        var scaleQtyByItemId = scaleQuantities
            .GroupBy(x => x.PurchaseConditionItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());
        var scaleValByItemId = scaleValues
            .GroupBy(x => x.PurchaseConditionItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());

        var pricesByCode = prices
            .GroupBy(x => x.PurchaseConditionCode, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);
        var pricesBySupplierMaterial = prices
            .GroupBy(
                x => $"{x.SupplierCode}\u001f{x.MaterialCode}",
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rows = new List<TaktPurchasePriceDeviationDto>();

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

            rows.Add(new TaktPurchasePriceDeviationDto
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
                PurchaseConditionCode = item.PurchaseConditionCode,
                MatchReason = matchReason,
                PirPriceCount = periodPrices.Count,
                PirPricesInPeriod = periodPrices,
                PirPricesText = FormatPeriodPricesText(periodPrices),
                PirPurchaseConditionCodes = string.Join(
                    ";",
                    pirsInPeriod.Select(p => p.PurchaseConditionCode).Distinct(StringComparer.OrdinalIgnoreCase)),
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
    private static List<TaktPurchasePriceDeviationDto> ApplyTrendFilter(
        List<TaktPurchasePriceDeviationDto> rows,
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
    /// 解析价格日期落在 ValidFrom～ValidTo 内的一条 PIR。
    /// 优先采购条件编码；该编码在周期内无命中时回退供应商+物料。
    /// 周期内多条时只取 PB00 中 ValidFrom 最晚的一条（无 PB00 则取最晚一条）。
    /// </summary>
    private static List<TaktPurchaseCondition> ResolvePirsInPeriod(
        TaktPurchaseOrderItem item,
        TaktPurchaseOrder order,
        DateTime pricingDate,
        IReadOnlyDictionary<string, List<TaktPurchaseCondition>> pricesByCode,
        IReadOnlyDictionary<string, List<TaktPurchaseCondition>> pricesBySupplierMaterial)
    {
        static List<TaktPurchaseCondition> FilterInPeriod(
            IEnumerable<TaktPurchaseCondition> source,
            DateTime day)
        {
            return source
                .Where(p => p.ValidFrom.Date <= day && p.ValidTo.Date >= day)
                .OrderByDescending(p => p.ValidFrom)
                .ThenByDescending(p => p.Id)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(item.PurchaseConditionCode)
            && pricesByCode.TryGetValue(item.PurchaseConditionCode.Trim(), out var byCode))
        {
            var byCodeInPeriod = PickApplicable(FilterInPeriod(byCode, pricingDate));
            if (byCodeInPeriod.Count > 0)
            {
                return byCodeInPeriod;
            }
        }

        var key = $"{order.SupplierCode}\u001f{item.MaterialCode}";
        if (!pricesBySupplierMaterial.TryGetValue(key, out var bySm))
        {
            return new List<TaktPurchaseCondition>();
        }
        return PickApplicable(FilterInPeriod(bySm, pricingDate));
    }

    /// <summary>
    /// 周期内多条 PIR 只保留一条适用价：PB00 优先，再取 ValidFrom、Id 最晚
    /// </summary>
    /// <param name="inPeriod">已落在价格日期内的采购条件</param>
    /// <returns>0 或 1 条</returns>
    private static List<TaktPurchaseCondition> PickApplicable(List<TaktPurchaseCondition> inPeriod)
    {
        if (inPeriod.Count == 0)
        {
            return inPeriod;
        }
        var picked = inPeriod
            .OrderBy(p => string.Equals(
                p.PriceType,
                TaktBomCalculatePurchaseConditionHelper.GrossPriceType,
                StringComparison.OrdinalIgnoreCase)
                ? 0
                : 1)
            .ThenByDescending(p => p.ValidFrom)
            .ThenByDescending(p => p.Id)
            .First();
        return new List<TaktPurchaseCondition> { picked };
    }

    /// <summary>
    /// 按 500 个 Id 分片查询，避免 SQL Server 2100 参数上限把整次分析打成失败
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <param name="ids">主键或外键</param>
    /// <param name="load">单片查询</param>
    /// <returns>合并结果</returns>
    private static async Task<List<TEntity>> LoadInChunksAsync<TEntity>(
        IReadOnlyList<long> ids,
        Func<List<long>, Task<List<TEntity>>> load)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(ids);
        ArgumentNullException.ThrowIfNull(load);
        var result = new List<TEntity>();
        if (ids.Count == 0)
        {
            return result;
        }
        for (var offset = 0; offset < ids.Count; offset = checked(offset + OrderIdChunkSize))
        {
            var take = Math.Min(OrderIdChunkSize, ids.Count - offset);
            var chunk = new List<long>(take);
            for (var i = 0; i < take; i++)
            {
                chunk.Add(ids[offset + i]);
            }
            var part = await load(chunk);
            if (part.Count > 0)
            {
                result.AddRange(part);
            }
        }
        return result;
    }

    /// <summary>
    /// 收集同周期 PIR 的全部价目点（条件行 + 数量等级 + 价值等级；用于展示）
    /// </summary>
    private static List<TaktPurchasePricePeriodPriceDto> CollectPeriodPrices(
        IReadOnlyList<TaktPurchaseCondition> pirs,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionItem>> priceItemsByPriceId,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionScaleQuantity>> scaleQtyByItemId,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionScaleValue>> scaleValByItemId)
    {
        var result = new List<TaktPurchasePricePeriodPriceDto>();
        foreach (var pir in pirs)
        {
            if (!priceItemsByPriceId.TryGetValue(pir.Id, out var pirItems))
            {
                continue;
            }
            foreach (var pirItem in pirItems)
            {
                var unit = pirItem.PriceUnit <= 0 ? 1 : pirItem.PriceUnit;
                result.Add(new TaktPurchasePricePeriodPriceDto
                {
                    PurchaseConditionCode = pir.PurchaseConditionCode,
                    PriceType = pir.PriceType,
                    ValidFrom = pir.ValidFrom.Date,
                    ValidTo = pir.ValidTo.Date,
                    PriceSource = "item",
                    PurchaseConditionSeq = pirItem.PurchaseConditionSeq,
                    Price = pirItem.Price,
                    PriceUnit = unit,
                    NormalizedUnitPrice = TaktPriceTrendAnalysisHelper.NormalizeUnitPrice(pirItem.Price, unit),
                });
                if (scaleQtyByItemId.TryGetValue(pirItem.Id, out var qtyScales))
                {
                    foreach (var scale in qtyScales)
                    {
                        result.Add(new TaktPurchasePricePeriodPriceDto
                        {
                            PurchaseConditionCode = pir.PurchaseConditionCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleQuantity",
                            PurchaseConditionSeq = pirItem.PurchaseConditionSeq,
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
                        result.Add(new TaktPurchasePricePeriodPriceDto
                        {
                            PurchaseConditionCode = pir.PurchaseConditionCode,
                            PriceType = pir.PriceType,
                            ValidFrom = pir.ValidFrom.Date,
                            ValidTo = pir.ValidTo.Date,
                            PriceSource = "scaleValue",
                            PurchaseConditionSeq = pirItem.PurchaseConditionSeq,
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
    private static List<TaktPurchasePricePeriodPriceDto> CollectApplicablePeriodPrices(
        IReadOnlyList<TaktPurchaseCondition> pirs,
        decimal orderQuantity,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionItem>> priceItemsByPriceId,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionScaleQuantity>> scaleQtyByItemId,
        IReadOnlyDictionary<long, List<TaktPurchaseConditionScaleValue>> scaleValByItemId)
    {
        var emptyQty = (IReadOnlyList<TaktPurchaseConditionScaleQuantity>)Array.Empty<TaktPurchaseConditionScaleQuantity>();
        var emptyVal = (IReadOnlyList<TaktPurchaseConditionScaleValue>)Array.Empty<TaktPurchaseConditionScaleValue>();
        var result = new List<TaktPurchasePricePeriodPriceDto>();
        foreach (var pir in pirs)
        {
            if (!priceItemsByPriceId.TryGetValue(pir.Id, out var pirItems) || pirItems.Count == 0)
            {
                continue;
            }
            var pirItem = TaktBomCalculatePurchaseConditionHelper.ResolveActiveItem(pirItems);
            if (pirItem == null)
            {
                continue;
            }
            var qtyRows = scaleQtyByItemId.TryGetValue(pirItem.Id, out var qtyScales)
                ? (IReadOnlyList<TaktPurchaseConditionScaleQuantity>)qtyScales
                : emptyQty;
            var valRows = scaleValByItemId.TryGetValue(pirItem.Id, out var valScales)
                ? (IReadOnlyList<TaktPurchaseConditionScaleValue>)valScales
                : emptyVal;
            var rawPrice = TaktBomCalculatePurchaseConditionHelper.ResolveNetPrice(
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
            result.Add(new TaktPurchasePricePeriodPriceDto
            {
                PurchaseConditionCode = pir.PurchaseConditionCode,
                PriceType = pir.PriceType,
                ValidFrom = pir.ValidFrom.Date,
                ValidTo = pir.ValidTo.Date,
                PriceSource = priceSource,
                PurchaseConditionSeq = pirItem.PurchaseConditionSeq,
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
        TaktPurchaseConditionItem item,
        IReadOnlyList<TaktPurchaseConditionScaleQuantity> quantityScales,
        IReadOnlyList<TaktPurchaseConditionScaleValue> valueScales,
        decimal orderQuantity)
    {
        var basis = item.ScaleBasis?.Trim() ?? string.Empty;
        var preferQuantity = string.Equals(
                basis,
                TaktBomCalculatePurchaseConditionHelper.QuantityScaleBasis,
                StringComparison.OrdinalIgnoreCase)
            || (!string.Equals(
                    basis,
                    TaktBomCalculatePurchaseConditionHelper.ValueScaleBasis,
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
    private static string FormatPeriodPricesText(IReadOnlyList<TaktPurchasePricePeriodPriceDto> prices)
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
            builder.Append(CultureInfo.InvariantCulture, $"{p.PurchaseConditionCode} {p.ValidFrom:yyyy-MM-dd}~{p.ValidTo:yyyy-MM-dd} {source} {p.NormalizedUnitPrice:0.00000}");
        }
        return builder.ToString();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetMiroPricePcrDeviationPlantOptionsAsync(
        string? plantCode = null,
        string? keyword = null)
    {
        var list = await _materialDocumentItemRepository.GetListAsync(x => x.IsObsolete == 0);
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
    public async Task<List<TaktSelectOption>> GetMiroPricePcrDeviationSupplierOptionsAsync(
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
        var headers = await _materialDocumentRepository.GetListAsync(x =>
            x.PostingDate >= rangeStart && x.PostingDate <= rangeEnd);
        if (headers.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var headerIds = headers.Select(x => x.Id).ToList();
        var items = await LoadInChunksAsync(
            headerIds,
            chunk => _materialDocumentItemRepository.GetListAsync(
                x => chunk.Contains(x.MaterialDocumentId)
                    && x.IsObsolete == 0
                    && x.PlantCode == plant
                    && x.SupplierCode != null
                    && x.SupplierCode != string.Empty));
        IEnumerable<TaktMaterialDocumentItem> filtered = items;
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim();
            filtered = items.Where(x =>
                x.SupplierCode!.Contains(kw, StringComparison.OrdinalIgnoreCase));
        }
        return filtered
            .GroupBy(x => x.SupplierCode!, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .Take(MaxPartnerOptions)
            .Select(g => new TaktSelectOption
            {
                DictLabel = g.Key,
                DictValue = g.Key,
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetMiroPricePcrDeviationMaterialOptionsAsync(
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
        var headers = await _materialDocumentRepository.GetListAsync(x =>
            x.PostingDate >= rangeStart && x.PostingDate <= rangeEnd);
        if (headers.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var headerIds = headers.Select(x => x.Id).ToList();
        var kw = keyword?.Trim() ?? string.Empty;
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < headerIds.Count; offset = checked(offset + OrderIdChunkSize))
        {
            var take = Math.Min(OrderIdChunkSize, headerIds.Count - offset);
            var chunkIds = headerIds.GetRange(offset, take);
            var items = await _materialDocumentItemRepository.GetListAsync(
                x => chunkIds.Contains(x.MaterialDocumentId)
                    && x.IsObsolete == 0
                    && x.PlantCode == plant
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty);
            if (!string.IsNullOrWhiteSpace(supplierCode))
            {
                var supplier = supplierCode.Trim();
                items = items
                    .Where(x => string.Equals(x.SupplierCode, supplier, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            foreach (var item in items)
            {
                var code = item.MaterialCode.Trim();
                if (string.IsNullOrEmpty(code))
                {
                    continue;
                }
                var desc = item.ItemText?.Trim() ?? string.Empty;
                if (!string.IsNullOrEmpty(kw)
                    && !code.Contains(kw, StringComparison.OrdinalIgnoreCase)
                    && !desc.Contains(kw, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (!map.ContainsKey(code))
                {
                    map[code] = desc;
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
    public async Task<TaktMiroPricePcrDeviationResultDto> GetMiroPricePcrDeviationAnalysisAsync(
        TaktMiroPricePcrDeviationQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildMiroDeviationRowsAsync(queryDto);
        var pageRows = built.Rows.Skip(skip).Take(pageSize).ToList();
        return new TaktMiroPricePcrDeviationResultDto
        {
            Paged = TaktPagedResult<TaktMiroPricePcrDeviationDto>.Create(
                pageRows, built.Rows.Count, pageIndex, pageSize),
            DeviationCount = built.Rows.Count,
            NoPirCount = built.NoPirCount,
            PriceAbsentCount = built.PriceAbsentCount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportMiroPricePcrDeviationAnalysisAsync(
        TaktMiroPricePcrDeviationQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildMiroDeviationRowsAsync(query);
        var columnKeys = new[]
        {
            "plantCode", "materialDocumentCode", "lineNumber", "postingDate",
            "supplierCode", "supplierName", "materialCode", "materialDescription",
            "quantity", "miroUnitPrice", "pirValidPeriod", "pirPricesText",
            "deviationAmount", "deviationPercent",
        };
        var columnLabels = new[]
        {
            "工厂", "物料凭证", "行号", "过账日期",
            "供应商编码", "供应商名称", "物料编码", "物料描述",
            "数量", "MIRO单价", "PCR生效周期", "PCR价格",
            "价差", "价差比%",
        };
        var exportRows = built.Rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["plantCode"] = row.PlantCode,
            ["materialDocumentCode"] = row.MaterialDocumentCode,
            ["lineNumber"] = row.LineNumber,
            ["postingDate"] = row.PostingDate.Date,
            ["supplierCode"] = row.SupplierCode,
            ["supplierName"] = row.SupplierName,
            ["materialCode"] = row.MaterialCode,
            ["materialDescription"] = row.MaterialDescription,
            ["quantity"] = row.Quantity,
            ["miroUnitPrice"] = row.MiroUnitPrice,
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
            sheetName ?? "MIRO价偏离",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "MIRO价偏离"),
            columnWidths: new Dictionary<string, double>(StringComparer.Ordinal)
            {
                ["plantCode"] = 10d,
                ["materialDocumentCode"] = 14d,
                ["materialCode"] = 14d,
                ["miroUnitPrice"] = 12d,
                ["pirPricesText"] = 48d,
                ["deviationAmount"] = 12d,
                ["deviationPercent"] = 12d,
            });
    }

    /// <summary>
    /// 构建 MIRO 命中行（本位币单价 ≠ 同周期 PCR 按数量解析的适用价）
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>命中行与汇总</returns>
    private async Task<(List<TaktMiroPricePcrDeviationDto> Rows, int NoPirCount, int PriceAbsentCount)> BuildMiroDeviationRowsAsync(
        TaktMiroPricePcrDeviationQueryDto queryDto)
    {
        if (string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            ThrowBusinessException("请选择工厂代码");
        }
        var plant = queryDto.PlantCode.Trim();
        var (rangeStart, rangeEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);

        var headers = await _materialDocumentRepository.GetListAsync(x =>
            x.PostingDate >= rangeStart && x.PostingDate <= rangeEnd);
        if (headers.Count == 0)
        {
            return (new List<TaktMiroPricePcrDeviationDto>(), 0, 0);
        }
        var headerById = headers.ToDictionary(x => x.Id);
        var headerIds = headerById.Keys.ToList();
        var items = await LoadInChunksAsync(
            headerIds,
            chunk => _materialDocumentItemRepository.GetListAsync(
                x => chunk.Contains(x.MaterialDocumentId)
                    && x.IsObsolete == 0
                    && x.PlantCode == plant));
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            var supplier = queryDto.SupplierCode.Trim();
            items = items
                .Where(x => string.Equals(x.SupplierCode, supplier, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
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
            return (new List<TaktMiroPricePcrDeviationDto>(), 0, 0);
        }

        var prices = await _purchaseConditionRepository.GetListAsync(x => x.PlantCode == plant);
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            var priceType = queryDto.PriceType.Trim();
            prices = prices
                .Where(x => string.Equals(x.PriceType, priceType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        var priceById = prices.ToDictionary(x => x.Id);
        var priceIds = priceById.Keys.ToList();
        var priceItems = await LoadInChunksAsync(
            priceIds,
            chunk => _purchaseConditionItemRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionId) && x.IsObsolete == 0));
        var priceItemIds = priceItems.Select(x => x.Id).ToList();
        var scaleQuantities = await LoadInChunksAsync(
            priceItemIds,
            chunk => _scaleQuantityRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionItemId) && x.IsObsolete == 0));
        var scaleValues = await LoadInChunksAsync(
            priceItemIds,
            chunk => _scaleValueRepository.GetListAsync(
                x => chunk.Contains(x.PurchaseConditionItemId) && x.IsObsolete == 0));

        var priceItemsByPriceId = priceItems
            .GroupBy(x => x.PurchaseConditionId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseConditionSeq).ToList());
        var scaleQtyByItemId = scaleQuantities
            .GroupBy(x => x.PurchaseConditionItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());
        var scaleValByItemId = scaleValues
            .GroupBy(x => x.PurchaseConditionItemId)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.PurchaseScaleSeq).ToList());
        var pricesBySupplierMaterial = prices
            .GroupBy(
                x => $"{x.SupplierCode}\u001f{x.MaterialCode}",
                StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

        var rows = new List<TaktMiroPricePcrDeviationDto>();
        foreach (var item in items)
        {
            if (!headerById.TryGetValue(item.MaterialDocumentId, out var header))
            {
                continue;
            }
            if (string.IsNullOrWhiteSpace(item.MaterialCode) || string.IsNullOrWhiteSpace(item.SupplierCode))
            {
                continue;
            }
            var qtyAbs = Math.Abs(item.Quantity);
            if (qtyAbs <= UnitPriceEpsilon)
            {
                continue;
            }
            var amountAbs = Math.Abs(item.LocalCurrencyAmount);
            var miroUnit = Math.Round(amountAbs / qtyAbs, 5, MidpointRounding.AwayFromZero);
            var postingDate = header.PostingDate.Date;
            var pirsInPeriod = ResolveMiroPcrsInPeriod(
                item.SupplierCode!,
                item.MaterialCode,
                postingDate,
                pricesBySupplierMaterial);
            var applicablePrices = CollectApplicablePeriodPrices(
                pirsInPeriod,
                qtyAbs,
                priceItemsByPriceId,
                scaleQtyByItemId,
                scaleValByItemId);
            var periodPrices = CollectPeriodPrices(
                pirsInPeriod,
                priceItemsByPriceId,
                scaleQtyByItemId,
                scaleValByItemId);

            if (applicablePrices.Count > 0
                && applicablePrices.Any(p => Math.Abs(p.NormalizedUnitPrice - miroUnit) <= UnitPriceEpsilon))
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
                    .OrderBy(p => Math.Abs(p.NormalizedUnitPrice - miroUnit))
                    .ThenByDescending(p => p.ValidFrom)
                    .Select(p => p.NormalizedUnitPrice)
                    .First();
                deviation = Math.Round(miroUnit - closest.Value, 5, MidpointRounding.AwayFromZero);
                if (Math.Abs(closest.Value) > UnitPriceEpsilon)
                {
                    deviationPercent = Math.Round(
                        (miroUnit - closest.Value) / closest.Value,
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

            rows.Add(new TaktMiroPricePcrDeviationDto
            {
                PlantCode = item.PlantCode,
                MaterialDocumentCode = string.IsNullOrWhiteSpace(item.MaterialDocumentCode)
                    ? header.MaterialDocumentCode
                    : item.MaterialDocumentCode,
                LineNumber = item.LineNumber,
                PostingDate = postingDate,
                SupplierCode = item.SupplierCode!,
                SupplierName = string.Empty,
                MaterialCode = item.MaterialCode,
                MaterialDescription = item.ItemText?.Trim() ?? string.Empty,
                CurrencyCode = item.CurrencyCode ?? string.Empty,
                Quantity = qtyAbs,
                MiroUnitPrice = miroUnit,
                PurchaseOrderCode = item.PurchaseOrderCode,
                MatchReason = matchReason,
                PirPriceCount = periodPrices.Count,
                PirPricesInPeriod = periodPrices,
                PirPricesText = FormatPeriodPricesText(periodPrices),
                PirPurchaseConditionCodes = string.Join(
                    ";",
                    pirsInPeriod.Select(p => p.PurchaseConditionCode).Distinct(StringComparer.OrdinalIgnoreCase)),
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
            .ThenBy(x => x.MaterialDocumentCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(x => x.LineNumber)
            .ToList();
        rows = ApplyMiroTrendFilter(rows, queryDto.TrendFilter);
        var filteredNoPir = rows.Count(x => string.Equals(x.MatchReason, "no_pir", StringComparison.Ordinal));
        var filteredAbsent = rows.Count(x => string.Equals(x.MatchReason, "price_absent", StringComparison.Ordinal));
        return (rows, filteredNoPir, filteredAbsent);
    }

    /// <summary>
    /// MIRO：按供应商+物料+过账日落在 ValidFrom～ValidTo 解析 PCR（无采购条件编码优先路径）
    /// </summary>
    private static List<TaktPurchaseCondition> ResolveMiroPcrsInPeriod(
        string supplierCode,
        string materialCode,
        DateTime postingDate,
        IReadOnlyDictionary<string, List<TaktPurchaseCondition>> pricesBySupplierMaterial)
    {
        var key = $"{supplierCode}\u001f{materialCode}";
        if (!pricesBySupplierMaterial.TryGetValue(key, out var bySm))
        {
            return new List<TaktPurchaseCondition>();
        }
        var inPeriod = bySm
            .Where(p => p.ValidFrom.Date <= postingDate && p.ValidTo.Date >= postingDate)
            .OrderByDescending(p => p.ValidFrom)
            .ThenByDescending(p => p.Id)
            .ToList();
        return PickApplicable(inPeriod);
    }

    /// <summary>
    /// MIRO 涨跌筛选
    /// </summary>
    private static List<TaktMiroPricePcrDeviationDto> ApplyMiroTrendFilter(
        List<TaktMiroPricePcrDeviationDto> rows,
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
                .ThenBy(x => x.MaterialDocumentCode, StringComparer.OrdinalIgnoreCase)
                .ThenBy(x => x.LineNumber)
                .ToList();
        }
        return rows;
    }
}

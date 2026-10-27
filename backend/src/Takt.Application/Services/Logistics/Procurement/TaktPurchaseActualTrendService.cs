// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseActualTrendService.cs
// 创建时间：2026-09-19
// 创建人：Takt365(Cursor AI)
// 功能描述：采购实绩推移分析服务实现（采购发票）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购实绩推移分析服务（读采购发票行；与采购发票 CRUD 分离）
/// </summary>
public class TaktPurchaseActualTrendService : TaktServiceBase, ITaktPurchaseActualTrendService
{
    /// <summary>
    /// 按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchaseInvoice> _invoiceRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInvoiceItem> _itemRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktPurchaseActualTrendAnalysisBuilder _analysisBuilder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="invoiceRepository">采购发票主表仓储</param>
    /// <param name="itemRepository">采购发票行仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="companyRepository">公司仓储</param>
    /// <param name="analysisBuilder">推移分析构建器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseActualTrendService(
        ITaktCompanyRepository<TaktPurchaseInvoice> invoiceRepository,
        ITaktCompanyRepository<TaktPurchaseInvoiceItem> itemRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktPurchaseActualTrendAnalysisBuilder analysisBuilder,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _invoiceRepository = invoiceRepository;
        _itemRepository = itemRepository;
        _supplierRepository = supplierRepository;
        _companyRepository = companyRepository;
        _analysisBuilder = analysisBuilder;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchaseActualTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
        var relatedPlant = companies
            .Select(c => c.RelatedPlant?.Trim() ?? string.Empty)
            .FirstOrDefault(p => !string.IsNullOrEmpty(p))
            ?? string.Empty;
        if (string.IsNullOrEmpty(relatedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var items = await _itemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == relatedPlant
                && x.IsObsolete == 0
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty);
        if (items.Count == 0)
        {
            // 回退：PlantCode 空时用 ValuationArea
            items = await _itemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.IsObsolete == 0
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty
                    && x.ValuationArea == relatedPlant);
        }
        if (items.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        return new List<TaktSelectOption>
        {
            new()
            {
                DictValue = relatedPlant,
                DictLabel = relatedPlant,
            },
        };
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchaseActualTrendSupplierOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var items = await LoadPlantInvoiceItemsAsync(plant);
        if (items.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var invoiceIds = items.Select(i => i.PurchaseInvoiceId).Distinct().ToList();
        var codes = await LoadSupplierCodesFromInvoicesAsync(invoiceIds);
        if (codes.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var plantSupplierCodes = await LoadPlantScopedSupplierCodesAsync(plant, codes);
        var scopedCodes = plantSupplierCodes.Count > 0
            ? codes.Where(plantSupplierCodes.Contains).ToList()
            : codes;
        if (scopedCodes.Count == 0)
        {
            scopedCodes = codes;
        }
        var nameLookup = await LoadSupplierNameLookupAsync(plant, scopedCodes);
        return scopedCodes
            .Select(c =>
            {
                nameLookup.TryGetValue(c, out var name);
                var label = string.IsNullOrWhiteSpace(name) ? c : $"{c} - {name}";
                return new TaktSelectOption
                {
                    DictValue = c,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetPurchaseActualTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? supplierCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var supplier = supplierCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var items = await LoadPlantInvoiceItemsAsync(plant);
        if (!string.IsNullOrEmpty(supplier))
        {
            var invoiceIds = items.Select(i => i.PurchaseInvoiceId).Distinct().ToList();
            var supplierInvoiceIds = await FilterInvoiceIdsBySupplierAsync(invoiceIds, supplier);
            items = items.Where(i => supplierInvoiceIds.Contains(i.PurchaseInvoiceId)).ToList();
        }
        return items
            .Where(e => !string.IsNullOrWhiteSpace(e.MaterialCode))
            .GroupBy(e => e.MaterialCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<TaktPurchasePriceTrendResultDto> GetPurchaseActualTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await _analysisBuilder.BuildAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktPurchasePriceTrendResultDto
        {
            Paged = TaktPagedResult<TaktPurchasePriceTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            MaterialCount = built.OrderedRows.Count,
            BasePeriod = built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
            MonthSummaries = built.MonthSummaries,
            TotalAmount = built.TotalAmount,
            TotalUpAmount = built.TotalUpAmount,
            TotalDownAmount = built.TotalDownAmount,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseActualTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(query.PlantCode);
        var built = await _analysisBuilder.BuildAsync(query);
        var plantRows = TaktPurchasePriceTrendExportHelper.WhereSelecmedPlant(built.OrderedRows, query.PlantCode);
        var summaries = built.MonthSummaries ?? new List<TaktPurchasePriceTrendMonthSummaryDto>();
        var detailRows = TaktPurchasePriceTrendExportHelper.BuildHistoryDetailExpandedSheetRows(plantRows);
        var trendRows = TaktPurchasePriceTrendExportHelper.BuildHistoryTrendSheetRows(
            summaries,
            built.TotalAmount,
            built.TotalUpAmount,
            built.TotalDownAmount);
        var allDetailKeys = new[]
        {
            "plantCode", "materialCode", "materialDescription", "supplierCode", "supplierName", "currencyCode", "unit",
            "amount", "unitPrice", "quantity", "month", "adjacent", "trend", "varianceAmount",
        };
        var allDetailLabels = new[]
        {
            "工厂代码", "物料编码", "物料描述", "供应商编码", "供应商简称", "币种", "单位",
            "金额", "单价", "数量", "月份", "单价涨跌%", "期间涨跌", "期间单价影响金额",
        };
        var (detailKeys, detailLabels) = ResolveDetailExportColumns(
            query.DetailColumnKeys,
            allDetailKeys,
            allDetailLabels);
        var detailWidths = new Dictionary<string, double>(StringComparer.Ordinal)
        {
            ["amount"] = 14d,
            ["unitPrice"] = 14d,
            ["quantity"] = 12d,
            ["month"] = 12d,
            ["adjacent"] = 16d,
        };
        var trendKeys = new[] { "month", "amount", "impact" };
        return await TaktExcelHelper.ExportDictionarySheetsAsync(
            new List<TaktExcelDictionarySheet>
            {
                new()
                {
                    SheetName = "明细",
                    Rows = detailRows,
                    ColumnKeys = detailKeys,
                    ColumnLabels = detailLabels,
                    ColumnWidths = detailWidths,
                },
                new()
                {
                    SheetName = "推移表",
                    Rows = trendRows,
                    ColumnKeys = trendKeys,
                    ColumnLabels = new[] { "月份", "采购份额（万元）", "涨跌金额（万元，单价差×数量）" },
                    ColumnWidths = new Dictionary<string, double>(StringComparer.Ordinal)
                    {
                        ["month"] = 12d,
                        ["amount"] = 16d,
                        ["impact"] = 28d,
                    },
                },
            },
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "采购实绩推移"));
    }

    /// <inheritdoc />
    public async Task<TaktPurchaseInvoiceStatDto> GetPurchaseActualTrendInvoiceStatAsync(TaktProcurementStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.PostingDateStart ?? queryDto.OrderDateStart,
            queryDto.PostingDateEnd ?? queryDto.OrderDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPurchaseInvoice, bool>> invoicePredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PostingDate >= start
            && x.PostingDate <= end;
        var monthInvoiceCount = await _invoiceRepository.CountAsync(invoicePredicate);
        Expression<Func<TaktPurchaseInvoiceItem, bool>> itemPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktPurchaseInvoice>()
                .Where(h =>
                    h.Id == x.PurchaseInvoiceId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.PostingDate >= start
                    && h.PostingDate <= end
                    && h.IsDeleted == 0)
                .Any();
        var monthTotalAmount = await _itemRepository.SumAsync(x => x.Amount ?? 0m, itemPredicate);
        if (monthTotalAmount == 0 && monthInvoiceCount > 0)
        {
            monthTotalAmount = await _invoiceRepository.SumAsync(x => x.GrossAmount, invoicePredicate);
        }
        return new TaktPurchaseInvoiceStatDto
        {
            StatMonth = statMonth,
            MonthInvoiceCount = monthInvoiceCount,
            MonthTotalAmount = monthTotalAmount,
        };
    }

    /// <summary>
    /// 加载本厂采购发票行（PlantCode 或 ValuationArea）
    /// </summary>
    /// <param name="plant">工厂</param>
    /// <returns>行列表</returns>
    private async Task<List<TaktPurchaseInvoiceItem>> LoadPlantInvoiceItemsAsync(string plant)
    {
        var list = await _itemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.IsObsolete == 0
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty);
        if (list.Count > 0)
        {
            return list;
        }
        return await _itemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.IsObsolete == 0
                && x.MaterialCode != null
                && x.MaterialCode != string.Empty
                && x.ValuationArea == plant);
    }

    /// <summary>
    /// 从发票头加载供应商编码去重列表
    /// </summary>
    /// <param name="invoiceIds">发票 Id</param>
    /// <returns>供应商编码</returns>
    private async Task<List<string>> LoadSupplierCodesFromInvoicesAsync(IReadOnlyList<long> invoiceIds)
    {
        var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (invoiceIds.Count == 0)
        {
            return new List<string>();
        }
        for (var offset = 0; offset < invoiceIds.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = invoiceIds.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var invoices = await _invoiceRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && batch.Contains(x.Id)
                    && x.SupplierCode != null
                    && x.SupplierCode != string.Empty);
            foreach (var code in invoices.Select(i => i.SupplierCode.Trim()).Where(c => !string.IsNullOrEmpty(c)))
            {
                codes.Add(code);
            }
        }
        return codes.OrderBy(c => c, StringComparer.Ordinal).ToList();
    }

    /// <summary>
    /// 按供应商过滤发票 Id
    /// </summary>
    /// <param name="invoiceIds">候选发票</param>
    /// <param name="supplierCode">供应商</param>
    /// <returns>匹配的发票 Id</returns>
    private async Task<HashSet<long>> FilterInvoiceIdsBySupplierAsync(IReadOnlyList<long> invoiceIds, string supplierCode)
    {
        var result = new HashSet<long>();
        if (invoiceIds.Count == 0)
        {
            return result;
        }
        var supplier = supplierCode.Trim();
        for (var offset = 0; offset < invoiceIds.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = invoiceIds.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var invoices = await _invoiceRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && batch.Contains(x.Id)
                    && x.SupplierCode == supplier);
            foreach (var id in invoices.Select(i => i.Id))
            {
                result.Add(id);
            }
        }
        return result;
    }

    /// <summary>
    /// 按前端可见列键裁剪明细导出列
    /// </summary>
    /// <param name="detailColumnKeys">逗号分隔列键</param>
    /// <param name="allKeys">全部列键</param>
    /// <param name="allLabels">全部列标题</param>
    /// <returns>导出列键与标题</returns>
    private static (string[] Keys, string[] Labels) ResolveDetailExportColumns(
        string? detailColumnKeys,
        IReadOnlyList<string> allKeys,
        IReadOnlyList<string> allLabels)
    {
        if (allKeys.Count != allLabels.Count)
        {
            throw new ArgumentException("明细导出列键与标题数量不一致");
        }
        var selecmed = (detailColumnKeys ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        // 兼容旧列键 points → 金额/单价/数量/月份/涨跌
        if (selecmed.Exists(k => string.Equals(k, "points", StringComparison.OrdinalIgnoreCase)))
        {
            selecmed.RemoveAll(k => string.Equals(k, "points", StringComparison.OrdinalIgnoreCase));
            selecmed.AddRange(new[] { "amount", "unitPrice", "quantity", "month", "adjacent" });
            selecmed = selecmed.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
        if (selecmed.Count > 0
            && allKeys.Contains("plantCode", StringComparer.OrdinalIgnoreCase)
            && !selecmed.Exists(k => string.Equals(k, "plantCode", StringComparison.OrdinalIgnoreCase)))
        {
            selecmed.Insert(0, "plantCode");
        }
        var selecmedSet = selecmed.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (selecmedSet.Count == 0)
        {
            return (allKeys.ToArray(), allLabels.ToArray());
        }
        var keys = new List<string>(allKeys.Count);
        var labels = new List<string>(allLabels.Count);
        for (var i = 0; i < allKeys.Count; i++)
        {
            if (!selecmedSet.Contains(allKeys[i]))
            {
                continue;
            }
            keys.Add(allKeys[i]);
            labels.Add(allLabels[i]);
        }
        if (keys.Count == 0)
        {
            return (allKeys.ToArray(), allLabels.ToArray());
        }
        return (keys.ToArray(), labels.ToArray());
    }

    /// <summary>
    /// 加载指定工厂下存在于供应商主数据的编码集合
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">候选供应商编码</param>
    /// <returns>该工厂主数据中存在的编码</returns>
    private async Task<HashSet<string>> LoadPlantScopedSupplierCodesAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(plantCode) || supplierCodes.Count == 0)
        {
            return result;
        }
        var plant = plantCode.Trim();
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = await _supplierRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && batch.Contains(x.SupplierCode));
            foreach (var code in suppliers
                .Select(s => s.SupplierCode?.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))!)
            {
                result.Add(code!);
            }
        }
        return result;
    }

    /// <summary>
    /// 加载供应商简称字典（优先同工厂主数据；无简称不回退名称）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">供应商编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadSupplierNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> supplierCodes)
    {
        if (supplierCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        var codes = supplierCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var suppliers = string.IsNullOrEmpty(plant)
                ? await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode))
                : await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && batch.Contains(x.SupplierCode));
            if (suppliers.Count == 0 && !string.IsNullOrEmpty(plant))
            {
                suppliers = await _supplierRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.SupplierCode));
            }
            foreach (var group in suppliers
                .Where(s => !string.IsNullOrWhiteSpace(s.SupplierCode))
                .GroupBy(s => s.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.SupplierShortName)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }
}

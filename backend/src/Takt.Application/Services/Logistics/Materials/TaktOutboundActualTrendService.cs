// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktOutboundActualTrendService.cs
// 创建时间：2026-09-17
// 创建人：Takt365(Cursor AI)
// 功能描述：物料出库推移分析服务实现（物料凭证行 CustomerCode 非空）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Application.Services.Logistics.Procurement;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 物料出库推移分析服务（读物料凭证 CustomerCode 非空行；与销售实绩推移分离）
/// </summary>
public class TaktOutboundActualTrendService : TaktServiceBase, ITaktOutboundActualTrendService
{
    /// <summary>
    /// 物料/客户名称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktMaterialDocumentItem> _itemRepository;
    private readonly ITaktCompanyRepository<TaktCustomer> _customerRepository;
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    private readonly ITaktOutboundActualTrendAnalysisBuilder _analysisBuilder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="itemRepository">物料凭证行项目仓储</param>
    /// <param name="customerRepository">客户仓储</param>
    /// <param name="companyRepository">公司仓储（读取 RelatedPlant）</param>
    /// <param name="analysisBuilder">推移分析构建器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOutboundActualTrendService(
        ITaktCompanyRepository<TaktMaterialDocumentItem> itemRepository,
        ITaktCompanyRepository<TaktCustomer> customerRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktOutboundActualTrendAnalysisBuilder analysisBuilder,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _itemRepository = itemRepository;
        _customerRepository = customerRepository;
        _companyRepository = companyRepository;
        _analysisBuilder = analysisBuilder;
    }

    /// <summary>
    /// 推移查询栏工厂选项：仅当前公司 RelatedPlant，且须存在交货出库凭证行
    /// </summary>
    /// <param name="plantCode">工厂代码（可选）</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOutboundActualTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
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
        var receiptItems = await _itemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == relatedPlant
                && x.IsObsolete == 0
                && x.CustomerCode != null
                && x.CustomerCode != string.Empty);
        if (receiptItems.Count == 0)
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

    /// <summary>
    /// 推移查询栏：按工厂去重客户（优先与同厂客户主数据交叉）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOutboundActualTrendCustomerOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _itemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.IsObsolete == 0
                && x.CustomerCode != null
                && x.CustomerCode != string.Empty);
        var codes = list
            .GroupBy(e => e.CustomerCode!.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.Key)
            .Where(c => !string.IsNullOrEmpty(c))
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
        if (codes.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        var plantCustomerCodes = await LoadPlantScopedCustomerCodesAsync(plant, codes);
        var scopedCodes = plantCustomerCodes.Count > 0
            ? codes.Where(plantCustomerCodes.Contains).ToList()
            : codes;
        if (scopedCodes.Count == 0)
        {
            scopedCodes = codes;
        }
        var nameLookup = await LoadCustomerNameLookupAsync(plant, scopedCodes);
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

    /// <summary>
    /// 推移查询栏：按工厂去重物料（客户可选，传入则再过滤；标签暂用编码，分析时再回填名称）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选）</param>
    /// <param name="customerCode">客户编码（可选）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOutboundActualTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? customerCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var customer = customerCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = string.IsNullOrEmpty(customer)
            ? await _itemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.IsObsolete == 0
                    && x.CustomerCode != null
                    && x.CustomerCode != string.Empty
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty)
            : await _itemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.IsObsolete == 0
                    && x.CustomerCode == customer
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty);
        return list
            .GroupBy(e => e.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 物料出库推移转置分析（工厂×物料；列=期间各月；金额浮动=单价差×当月数量）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktPurchasePriceTrendResultDto> GetOutboundActualTrendAnalysisAsync(
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

    /// <summary>
    /// 导出物料出库推移（Sheet1 明细；Sheet2 推移表：月份 / 出库份额万元 / 相比上月涨跌万元）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOutboundActualTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(query.PlantCode);
        var built = await _analysisBuilder.BuildAsync(query);
        var plantRows = TaktPurchasePriceTrendExportHelper.WhereSelectedPlant(built.OrderedRows, query.PlantCode);
        var summaries = built.MonthSummaries ?? new List<TaktPurchasePriceTrendMonthSummaryDto>();
        var detailRows = TaktPurchasePriceTrendExportHelper.BuildHistoryDetailExpandedSheetRows(plantRows);
        var trendRows = TaktPurchasePriceTrendExportHelper.BuildHistoryTrendSheetRows(
            summaries,
            built.TotalAmount,
            built.TotalUpAmount,
            built.TotalDownAmount);
        var allDetailKeys = new[]
        {
            "plantCode", "materialCode", "materialDescription", "customerCode", "customerName", "currencyCode", "unit",
            "amount", "unitPrice", "quantity", "month", "adjacent", "trend", "varianceAmount",
        };
        var allDetailLabels = new[]
        {
            "工厂代码", "物料编码", "物料描述", "客户编码", "客户名称", "币种", "单位",
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
                    ColumnLabels = new[] { "月份", "出库份额（万元）", "涨跌金额（万元，单价差×数量）" },
                    ColumnWidths = new Dictionary<string, double>(StringComparer.Ordinal)
                    {
                        ["month"] = 12d,
                        ["amount"] = 16d,
                        ["impact"] = 28d,
                    },
                },
            },
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "物料出库推移"));
    }

    /// <summary>
    /// 按前端可见列键裁剪明细导出列（保持 allKeys 顺序；空或全无效则导出全部）
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
        var selected = (detailColumnKeys ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(k => !string.IsNullOrWhiteSpace(k))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        // 兼容旧列键 points → 金额/单价/数量/月份/涨跌
        if (selected.Exists(k => string.Equals(k, "points", StringComparison.OrdinalIgnoreCase)))
        {
            selected.RemoveAll(k => string.Equals(k, "points", StringComparison.OrdinalIgnoreCase));
            selected.AddRange(new[] { "amount", "unitPrice", "quantity", "month", "adjacent" });
            selected = selected.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
        if (selected.Count > 0
            && allKeys.Contains("plantCode", StringComparer.OrdinalIgnoreCase)
            && !selected.Exists(k => string.Equals(k, "plantCode", StringComparison.OrdinalIgnoreCase)))
        {
            selected.Insert(0, "plantCode");
        }
        var selectedSet = selected.ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (selectedSet.Count == 0)
        {
            return (allKeys.ToArray(), allLabels.ToArray());
        }
        var keys = new List<string>(allKeys.Count);
        var labels = new List<string>(allLabels.Count);
        for (var i = 0; i < allKeys.Count; i++)
        {
            if (!selectedSet.Contains(allKeys[i]))
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
    /// 加载指定工厂下存在于客户主数据的编码集合
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="customerCodes">候选客户编码</param>
    /// <returns>该工厂主数据中存在的编码</returns>
    private async Task<HashSet<string>> LoadPlantScopedCustomerCodesAsync(
        string plantCode,
        IReadOnlyList<string> customerCodes)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(plantCode) || customerCodes.Count == 0)
        {
            return result;
        }
        var plant = plantCode.Trim();
        var codes = customerCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var customers = await _customerRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && batch.Contains(x.CustomerCode));
            foreach (var code in customers
                .Select(s => s.CustomerCode?.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))!)
            {
                result.Add(code!);
            }
        }
        return result;
    }

    /// <summary>
    /// 加载客户名称字典（优先同工厂主数据）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="customerCodes">客户编码</param>
    /// <returns>编码→名称</returns>
    private async Task<Dictionary<string, string>> LoadCustomerNameLookupAsync(
        string plantCode,
        IReadOnlyList<string> customerCodes)
    {
        if (customerCodes.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        var codes = customerCodes.Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var offset = 0; offset < codes.Count; offset = checked(offset + MaterialNameLookupBatchSize))
        {
            var batch = codes.Skip(offset).Take(MaterialNameLookupBatchSize).ToList();
            var customers = string.IsNullOrEmpty(plant)
                ? await _customerRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.CustomerCode))
                : await _customerRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && batch.Contains(x.CustomerCode));
            if (customers.Count == 0 && !string.IsNullOrEmpty(plant))
            {
                customers = await _customerRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && batch.Contains(x.CustomerCode));
            }
            foreach (var group in customers
                .Where(s => !string.IsNullOrWhiteSpace(s.CustomerCode))
                .GroupBy(s => s.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                if (map.ContainsKey(group.Key))
                {
                    continue;
                }
                map[group.Key] = group.Select(x => x.CustomerName1)
                    .FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))?.Trim() ?? string.Empty;
            }
        }
        return map;
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchasePriceTrendService.cs
// 创建时间：2026-08-23
// 创建人：Takt365(Cursor AI)
// 功能描述：采购价格推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购价格推移分析服务（读采购价格本表；与 CRUD 服务分离）
/// </summary>
public class TaktPurchasePriceTrendService : TaktServiceBase, ITaktPurchasePriceTrendService
{
    /// <summary>
    /// 供应商简称按编码分批查询，避免超长 IN 列表
    /// </summary>
    private const int MaterialNameLookupBatchSize = 500;

    private readonly ITaktCompanyRepository<TaktPurchaseInfo> _purchaseInfoRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;
    private readonly ITaktPurchasePriceTrendMonthlyAnalysisBuilder _monthlyAnalysisBuilder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoRepository">采购价格仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="monthlyAnalysisBuilder">月推移分析构建器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchasePriceTrendService(
        ITaktCompanyRepository<TaktPurchaseInfo> purchaseInfoRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktPurchasePriceTrendMonthlyAnalysisBuilder monthlyAnalysisBuilder,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoRepository = purchaseInfoRepository;
        _supplierRepository = supplierRepository;
        _monthlyAnalysisBuilder = monthlyAnalysisBuilder;
    }

    /// <summary>
    /// 推移查询栏工厂选项（级联第 1 级）：当前公司采购价格本表 PlantCode 去重
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项（DictValue=PlantCode）</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInfoRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode != null
                && x.PlantCode != string.Empty);
        var keywordText = keyword?.Trim() ?? string.Empty;
        var plantText = plantCode?.Trim() ?? string.Empty;
        return list
            .Select(e => e.PlantCode.Trim())
            .Where(code => !string.IsNullOrEmpty(code))
            .Where(code => string.IsNullOrEmpty(plantText) || string.Equals(code, plantText, StringComparison.OrdinalIgnoreCase))
            .Where(code => string.IsNullOrEmpty(keywordText) || code.Contains(keywordText, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(code => code, StringComparer.Ordinal)
            .Select(code => new TaktSelectOption
            {
                DictValue = code,
                DictLabel = code,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendPriceTypeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchaseInfoRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType != null
                && x.PriceType != string.Empty);
        return list
            .GroupBy(e => e.PriceType.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重供应商（级联第 3 级；优先与同厂供应商主数据交叉）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendSupplierOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _purchaseInfoRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.SupplierCode != null
                && x.SupplierCode != string.Empty);
        var codes = list
            .GroupBy(e => e.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(g => g.Key)
            .Where(c => !string.IsNullOrEmpty(c))
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
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

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重物料（供应商可选，传入则再过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="supplierCode">供应商编码（可选）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchasePriceTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null, string? supplierCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        var supplier = supplierCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type))
        {
            return new List<TaktSelectOption>();
        }
        var list = string.IsNullOrEmpty(supplier)
            ? await _purchaseInfoRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.PriceType == type
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty)
            : await _purchaseInfoRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.PriceType == type
                    && x.SupplierCode == supplier
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty);
        return list
            .GroupBy(e => e.MaterialCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var description = g.Select(x => x.MaterialDescription)
                    .FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))?.Trim();
                var label = string.IsNullOrWhiteSpace(description) ? g.Key : $"{g.Key} - {description}";
                return new TaktSelectOption
                {
                    DictValue = g.Key,
                    DictLabel = label,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 采购价格推移转置分析（工厂×物料×供应商×有效起始日）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktPurchasePriceTrendResultDto> GetPurchasePriceTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await _monthlyAnalysisBuilder.BuildAsync(queryDto);
        var lines = TaktPurchasePriceTrendExportHelper.ExpandToValidFromLines(
            TaktPurchasePriceTrendExportHelper.WhereSelectedPlant(built.OrderedRows, queryDto.PlantCode));
        var pageRows = lines.Skip(skip).Take(pageSize).ToList();
        return new TaktPurchasePriceTrendResultDto
        {
            Paged = TaktPagedResult<TaktPurchasePriceTrendDto>.Create(
                pageRows, lines.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            MaterialCount = lines.Count,
            BasePeriod = built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <summary>
    /// 导出采购价格推移（与列表相同：所选工厂、期间、条件类型、供应商、物料；每笔有效起始日一行）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchasePriceTrendAnalysisAsync(
        TaktPurchasePriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentException.ThrowIfNullOrWhiteSpace(query.PlantCode);
        var built = await _monthlyAnalysisBuilder.BuildAsync(query);
        var plantRows = TaktPurchasePriceTrendExportHelper.ExpandToValidFromLines(
            TaktPurchasePriceTrendExportHelper.WhereSelectedPlant(built.OrderedRows, query.PlantCode));
        var columnKeys = new[]
        {
            "plantCode", "purchaseInfoCode", "materialCode", "materialDescription", "supplierCode", "supplierName", "currencyCode", "unit",
            "unitPrice", "validFrom", "adjacentPercent", "trend", "varianceAmount", "variancePercent",
        };
        var columnLabels = new[]
        {
            "工厂代码", "定价记录号", "物料编码", "物料描述", "供应商编码", "供应商简称", "币种", "单位",
            "单价", "有效起始日", "涨跌%", "期间涨跌", "期间差额", "期间%",
        };
        var exportRows = TaktPurchasePriceTrendExportHelper.BuildPriceTrendDetailSheetRows(plantRows);
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "采购价格推移清单",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "采购价格推移"),
            columnWidths: new Dictionary<string, double>(StringComparer.Ordinal)
            {
                ["unitPrice"] = TaktPurchasePriceTrendExportHelper.UnitPriceColumnWidth,
                ["validFrom"] = TaktPurchasePriceTrendExportHelper.ValidFromColumnWidth,
                ["adjacentPercent"] = 12d,
            });
    }

    /// <summary>
    /// 加载指定工厂下存在于供应商主数据的编码集合（用于级联交叉过滤）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="supplierCodes">候选供应商编码</param>
    /// <returns>该工厂主数据中存在的编码；主数据无该厂记录时返回空集（调用方回退为本表全集）</returns>
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

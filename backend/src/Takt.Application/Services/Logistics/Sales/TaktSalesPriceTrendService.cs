// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesPriceTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：销售价格推移分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;
using Takt.Shared.Validation;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售价格推移分析服务（读销售价格本表；与 CRUD 服务分离）
/// </summary>
public class TaktSalesPriceTrendService : TaktServiceBase, ITaktSalesPriceTrendService
{
    private readonly ITaktCompanyRepository<TaktSalesConditionRecord> _salesConditionRecordRepository;
    private readonly ITaktSalesPriceTrendMonthlyAnalysisBuilder _monthlyAnalysisBuilder;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRecordRepository">销售价格仓储</param>
    /// <param name="monthlyAnalysisBuilder">推移分析构建器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesPriceTrendService(
        ITaktCompanyRepository<TaktSalesConditionRecord> salesConditionRecordRepository,
        ITaktSalesPriceTrendMonthlyAnalysisBuilder monthlyAnalysisBuilder,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionRecordRepository = salesConditionRecordRepository;
        _monthlyAnalysisBuilder = monthlyAnalysisBuilder;
    }

    /// <summary>
    /// 推移查询栏：销售价格本表工厂去重选项
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesConditionRecordRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode != null
                && x.PlantCode != string.Empty);
        return list
            .GroupBy(e => e.PlantCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂去重条件类型（级联第 2 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendPriceTypeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesConditionRecordRepository.GetListAsync(
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
    /// 推移查询栏：按工厂+条件类型去重客户（级联第 3 级）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendCustomerOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type))
        {
            return new List<TaktSelectOption>();
        }
        var list = await _salesConditionRecordRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.PriceType == type
                && x.CustomerCode != null
                && x.CustomerCode != string.Empty);
        return list
            .GroupBy(e => e.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 推移查询栏：按工厂+条件类型去重物料（客户可选，传入则再过滤）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="priceType">条件类型</param>
    /// <param name="customerCode">客户编码（可选）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesPriceTrendMaterialOptionsAsync(string? plantCode = null, string? keyword = null, string? priceType = null, string? customerCode = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        var type = priceType?.Trim() ?? string.Empty;
        var customer = customerCode?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(plant) || string.IsNullOrEmpty(type))
        {
            return new List<TaktSelectOption>();
        }
        var list = string.IsNullOrEmpty(customer)
            ? await _salesConditionRecordRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.PriceType == type
                    && x.MaterialCode != null
                    && x.MaterialCode != string.Empty)
            : await _salesConditionRecordRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.PriceType == type
                    && x.CustomerCode == customer
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
    /// 销售价格推移转置分析（工厂×物料×客户×ValidFrom）
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>转置分析结果</returns>
    public async Task<TaktSalesPriceTrendResultDto> GetSalesPriceTrendAnalysisAsync(
        TaktSalesPriceTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await _monthlyAnalysisBuilder.BuildAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktSalesPriceTrendResultDto
        {
            Paged = TaktPagedResult<TaktSalesPriceTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            MaterialCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <summary>
    /// 导出销售价格推移转置分析（全量）
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesPriceTrendAnalysisAsync(
        TaktSalesPriceTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await _monthlyAnalysisBuilder.BuildAsync(query);
        var columnKeys = new List<string>
        {
            "plantCode", "materialCode", "materialDescription", "customerCode", "customerName", "currencyCode", "unit",
        };
        var columnLabels = new List<string>
        {
            "工厂代码", "物料编码", "物料描述", "客户编码", "客户名称", "币种", "单位",
        };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add(period);
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准有效起始日", "对比有效起始日", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["materialCode"] = row.MaterialCode,
                ["materialDescription"] = row.MaterialDescription,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
                ["currencyCode"] = row.CurrencyCode,
                ["unit"] = row.Unit,
                ["basePeriod"] = row.BasePeriod,
                ["comparePeriod"] = row.ComparePeriod,
                ["varianceAmount"] = row.VarianceAmount,
                ["variancePercent"] = row.VariancePercent.HasValue
                    ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
                    : null,
                ["trend"] = row.Trend,
            };
            foreach (var period in built.PeriodOrder)
            {
                if (!row.PeriodUnitPrices.TryGetValue(period, out var price))
                {
                    dict[$"period_{period}"] = null;
                    continue;
                }
                var isCarried = row.PeriodPriceSourcePeriods.TryGetValue(period, out var source)
                    && !string.IsNullOrWhiteSpace(source)
                    && !string.Equals(source, period, StringComparison.Ordinal);
                dict[$"period_{period}"] = isCarried
                    ? $"{price.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture)}*"
                    : price;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "销售价格推移清单",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "销售价格推移"));
    }
}

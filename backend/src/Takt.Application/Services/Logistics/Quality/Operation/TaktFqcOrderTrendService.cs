// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktFqcOrderTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：FQC 成品检验月推移转置分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// FQC 成品检验月推移转置分析服务
/// </summary>
public class TaktFqcOrderTrendService : TaktServiceBase, ITaktFqcOrderTrendService
{
    private readonly ITaktCompanyRepository<TaktFqcOrder> _fqcOrderRepository;
    private readonly ITaktCompanyRepository<TaktCustomer> _customerRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fqcOrderRepository">出货检验单仓储</param>
    /// <param name="customerRepository">客户仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFqcOrderTrendService(
        ITaktCompanyRepository<TaktFqcOrder> fqcOrderRepository,
        ITaktCompanyRepository<TaktCustomer> customerRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _fqcOrderRepository = fqcOrderRepository;
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// FQC 成品检验月推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    public async Task<TaktQualityInspectionMonthlyTrendResultDto<TaktFqcOrderMonthlyTrendDto>> GetFqcOrderMonthlyTrendAnalysisAsync(
        TaktFqcOrderMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildFqcOrderMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktQualityInspectionMonthlyTrendResultDto<TaktFqcOrderMonthlyTrendDto>
        {
            Paged = TaktPagedResult<TaktFqcOrderMonthlyTrendDto>.Create(
                pageRows, built.OrderedRows.Count, pageIndex, pageSize),
            PeriodOrder = built.PeriodOrder,
            RowCount = built.OrderedRows.Count,
            BasePeriod = pageRows.FirstOrDefault()?.BasePeriod ?? built.BasePeriod,
            ComparePeriod = built.ComparePeriod,
            UpCount = built.UpCount,
            DownCount = built.DownCount,
            FlatCount = built.FlatCount,
            NoneCount = built.NoneCount,
        };
    }

    /// <summary>
    /// 导出 FQC 成品检验月推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFqcOrderMonthlyTrendAnalysisAsync(
        TaktFqcOrderMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildFqcOrderMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "customerCode", "customerName" };
        var columnLabels = new List<string> { "工厂代码", "客户编码", "客户名称" };
        foreach (var period in built.PeriodOrder)
        {
            columnKeys.Add($"period_{period}");
            columnLabels.Add($"{period}不良率");
        }
        columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
        columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
        var exportRows = built.OrderedRows.Select(row =>
        {
            var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
            {
                ["plantCode"] = row.PlantCode,
                ["customerCode"] = row.CustomerCode,
                ["customerName"] = row.CustomerName,
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
                dict[$"period_{period}"] = row.PeriodDefectRates.TryGetValue(period, out var rate)
                    ? rate
                    : null;
            }
            return (IReadOnlyDictionary<string, object?>)dict;
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? "成品检验推移",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "成品检验推移"));
    }

    /// <summary>
    /// 构建 FQC 检验月推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<QualityInspectionMonthlyTrendBuilt<TaktFqcOrderMonthlyTrendDto>> BuildFqcOrderMonthlyTrendAnalysisAsync(
        TaktFqcOrderMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var customerFilter = string.IsNullOrWhiteSpace(queryDto.CustomerCode) ? null : queryDto.CustomerCode.Trim();
        var (_, rangeEnd, periodOrder) = TaktInspectionTrendAnalysisHelper.ResolveTrendRange(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);
        var rangeStart = periodOrder.Count > 0
            ? DateTime.ParseExact(periodOrder[0] + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            : DateTime.Now;
        var orders = await _fqcOrderRepository.GetListAsync(
            BuildFqcOrderTrendExpression(plantCode, customerFilter, rangeStart, rangeEnd));
        if (orders.Count == 0)
        {
            return QualityInspectionMonthlyTrendBuilt<TaktFqcOrderMonthlyTrendDto>.Empty();
        }
        var snapshots = orders.Select(o => new TaktInspectionTrendOrderSnapshot
        {
            PlantCode = o.PlantCode.Trim(),
            DimensionCode = o.CustomerCode?.Trim() ?? string.Empty,
            DimensionName = string.Empty,
            InspectionDate = o.InspectionDate,
            TotalSampleQuantity = o.TotalSampleQuantity,
            TotalQualifiedQuantity = o.TotalQualifiedQuantity,
            TotalUnqualifiedQuantity = o.TotalUnqualifiedQuantity,
        }).ToList();
        var built = TaktQualityInspectionTrendAnalysisCore.Build(
            snapshots,
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd,
            queryDto.FocusPeriod,
            queryDto.TrendFilter,
            (plant, customerCode, _, _, monthly, focus) =>
            {
                var row = new TaktFqcOrderMonthlyTrendDto
                {
                    PlantCode = plant,
                    CustomerCode = customerCode,
                };
                TaktQualityInspectionTrendAnalysisCore.FillPeriodMetrics(row, monthly, periodOrder, focus);
                return row;
            },
            r => r.CustomerCode);
        await FillFqcTrendCustomerNamesAsync(plantCode, built.OrderedRows);
        return built;
    }

    /// <summary>
    /// 构建 FQC 推移源数据筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="customerFilter">客户编码</param>
    /// <param name="rangeStart">期间起</param>
    /// <param name="rangeEnd">期间止</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktFqcOrder, bool>> BuildFqcOrderTrendExpression(
        string plantCode,
        string? customerFilter,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var exp = Expressionable.Create<TaktFqcOrder>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.InspectionDate != null
            && x.InspectionDate >= rangeStart
            && x.InspectionDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(customerFilter))
        {
            exp = exp.And(x => x.CustomerCode == customerFilter);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 回填客户名称
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="rows">推移行</param>
    /// <returns>任务</returns>
    private async Task FillFqcTrendCustomerNamesAsync(
        string plantCode,
        IReadOnlyList<TaktFqcOrderMonthlyTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }
        var codes = rows
            .Select(r => r.CustomerCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (codes.Count == 0)
        {
            return;
        }
        var customers = await _customerRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && codes.Contains(x.CustomerCode));
        var map = customers
            .Where(c => !string.IsNullOrWhiteSpace(c.CustomerCode))
            .GroupBy(c => c.CustomerCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.CustomerName1?.Trim()).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            if (map.TryGetValue(row.CustomerCode, out var name))
            {
                row.CustomerName = name;
            }
        }
    }
}

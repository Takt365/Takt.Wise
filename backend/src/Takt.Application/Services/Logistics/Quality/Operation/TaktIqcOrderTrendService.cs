// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktIqcOrderTrendService.cs
// 创建时间：2026-07-23
// 创建人：Takt365(Cursor AI)
// 功能描述：IQC 进货检验月推移转置分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// IQC 进货检验月推移转置分析服务
/// </summary>
public class TaktIqcOrderTrendService : TaktServiceBase, ITaktIqcOrderTrendService
{
    private readonly ITaktCompanyRepository<TaktIqcOrder> _iqcOrderRepository;
    private readonly ITaktCompanyRepository<TaktSupplier> _supplierRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="iqcOrderRepository">进货检验单仓储</param>
    /// <param name="supplierRepository">供应商仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktIqcOrderTrendService(
        ITaktCompanyRepository<TaktIqcOrder> iqcOrderRepository,
        ITaktCompanyRepository<TaktSupplier> supplierRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _iqcOrderRepository = iqcOrderRepository;
        _supplierRepository = supplierRepository;
    }

    /// <summary>
    /// IQC 进货检验月推移转置分析
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分析结果</returns>
    public async Task<TaktQualityInspectionMonthlyTrendResultDto<TaktIqcOrderMonthlyTrendDto>> GetIqcOrderMonthlyTrendAnalysisAsync(
        TaktIqcOrderMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var built = await BuildIqcOrderMonthlyTrendAnalysisAsync(queryDto);
        var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
        return new TaktQualityInspectionMonthlyTrendResultDto<TaktIqcOrderMonthlyTrendDto>
        {
            Paged = TaktPagedResult<TaktIqcOrderMonthlyTrendDto>.Create(
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
    /// 导出 IQC 进货检验月推移
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportIqcOrderMonthlyTrendAnalysisAsync(
        TaktIqcOrderMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var built = await BuildIqcOrderMonthlyTrendAnalysisAsync(query);
        var columnKeys = new List<string> { "plantCode", "supplierCode", "supplierName" };
        var columnLabels = new List<string> { "工厂代码", "供应商编码", "供应商名称" };
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
                ["supplierCode"] = row.SupplierCode,
                ["supplierName"] = row.SupplierName,
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
            sheetName ?? "进货检验推移",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "进货检验推移"));
    }

    /// <summary>
    /// 构建 IQC 检验月推移转置分析全量结果
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns>内存构建结果</returns>
    private async Task<QualityInspectionMonthlyTrendBuilt<TaktIqcOrderMonthlyTrendDto>> BuildIqcOrderMonthlyTrendAnalysisAsync(
        TaktIqcOrderMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        EnsureThreeLayerContext();
        var plantCode = queryDto.PlantCode.Trim();
        var supplierFilter = string.IsNullOrWhiteSpace(queryDto.SupplierCode) ? null : queryDto.SupplierCode.Trim();
        var (_, rangeEnd, periodOrder) = TaktInspectionTrendAnalysisHelper.ResolveTrendRange(
            queryDto.PeriodDateStart,
            queryDto.PeriodDateEnd);
        var rangeStart = periodOrder.Count > 0
            ? DateTime.ParseExact(periodOrder[0] + "-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            : DateTime.Now;
        var orders = await _iqcOrderRepository.GetListAsync(
            BuildIqcOrderTrendExpression(plantCode, supplierFilter, rangeStart, rangeEnd));
        if (orders.Count == 0)
        {
            return QualityInspectionMonthlyTrendBuilt<TaktIqcOrderMonthlyTrendDto>.Empty();
        }
        var snapshots = orders.Select(o => new TaktInspectionTrendOrderSnapshot
        {
            PlantCode = o.PlantCode.Trim(),
            DimensionCode = o.SupplierCode?.Trim() ?? string.Empty,
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
            (plant, supplierCode, _, _, monthly, focus) =>
            {
                var row = new TaktIqcOrderMonthlyTrendDto
                {
                    PlantCode = plant,
                    SupplierCode = supplierCode,
                };
                TaktQualityInspectionTrendAnalysisCore.FillPeriodMetrics(row, monthly, periodOrder, focus);
                return row;
            },
            r => r.SupplierCode);
        await FillIqcTrendSupplierNamesAsync(plantCode, built.OrderedRows);
        return built;
    }

    /// <summary>
    /// 构建 IQC 推移源数据筛选条件
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="supplierFilter">供应商编码</param>
    /// <param name="rangeStart">期间起</param>
    /// <param name="rangeEnd">期间止</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktIqcOrder, bool>> BuildIqcOrderTrendExpression(
        string plantCode,
        string? supplierFilter,
        DateTime rangeStart,
        DateTime rangeEnd)
    {
        var exp = Expressionable.Create<TaktIqcOrder>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && x.InspectionDate != null
            && x.InspectionDate >= rangeStart
            && x.InspectionDate <= rangeEnd);
        if (!string.IsNullOrWhiteSpace(supplierFilter))
        {
            exp = exp.And(x => x.SupplierCode == supplierFilter);
        }
        return exp.ToExpression();
    }

    /// <summary>
    /// 回填供应商名称
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="rows">推移行</param>
    /// <returns>任务</returns>
    private async Task FillIqcTrendSupplierNamesAsync(
        string plantCode,
        IReadOnlyList<TaktIqcOrderMonthlyTrendDto> rows)
    {
        if (rows.Count == 0)
        {
            return;
        }
        var codes = rows
            .Select(r => r.SupplierCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (codes.Count == 0)
        {
            return;
        }
        var suppliers = await _supplierRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plantCode
            && codes.Contains(x.SupplierCode));
        var map = suppliers
            .Where(s => !string.IsNullOrWhiteSpace(s.SupplierCode))
            .GroupBy(s => s.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.SupplierName1?.Trim()).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n)) ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);
        foreach (var row in rows)
        {
            if (map.TryGetValue(row.SupplierCode, out var name))
            {
                row.SupplierName = name;
            }
        }
    }
}

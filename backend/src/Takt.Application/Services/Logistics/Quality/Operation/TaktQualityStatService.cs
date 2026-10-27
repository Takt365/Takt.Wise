// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktQualityStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：质量统计。进货检验按供应商、制程检验按工序、出货检验按客户汇总抽样与不良
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Application.Services;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 质量统计
/// </summary>
public class TaktQualityStatService : TaktServiceBase, ITaktQualityStatService
{
    private const string TabIqc = "iqc";
    private const string TabIpqc = "ipqc";
    private const string TabFqc = "fqc";
    private const int MaxRows = 100000;

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktQualityStatService(
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetQualityStatPlantOptionsAsync(string? statTab, string? keyword = null)
    {
        return DistinctAsync(statTab, null, keyword, plantOnly: true);
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetQualityStatDimensionOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, plantOnly: false);
    }

    /// <inheritdoc />
    public async Task<TaktQualityOperationStatResultDto> GetQualityStatListAsync(TaktQualityOperationStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var rows = await LoadRowsAsync(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var page = rows.Skip(skip).Take(pageSize).ToList();
        return new TaktQualityOperationStatResultDto
        {
            Paged = TaktPagedResult<TaktQualityOperationStatRowDto>.Create(page, rows.Count, pageIndex, pageSize),
            RowCount = rows.Count,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportQualityStatAsync(
        TaktQualityOperationStatQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var rows = await LoadRowsAsync(query);
        var (codeLabel, nameLabel) = DimensionLabels(query.StatTab);
        var columnKeys = new List<string>
        {
            "plantCode", "dimensionCode", "dimensionName", "orderCount",
            "sampleQty", "qualifiedQty", "unqualifiedQty", "returnQty",
            "defectRatePercent", "qualifiedRatePercent",
        };
        var columnLabels = new List<string>
        {
            "工厂", codeLabel, nameLabel, "检验单数",
            "抽样数量", "合格数量", "不合格数量", "验退数量",
            "不良率%", "合格率%",
        };
        var exportRows = rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["plantCode"] = row.PlantCode,
            ["dimensionCode"] = row.DimensionCode,
            ["dimensionName"] = row.DimensionName,
            ["orderCount"] = row.OrderCount,
            ["sampleQty"] = row.SampleQty,
            ["qualifiedQty"] = row.QualifiedQty,
            ["unqualifiedQty"] = row.UnqualifiedQty,
            ["returnQty"] = row.ReturnQty,
            ["defectRatePercent"] = row.DefectRatePercent,
            ["qualifiedRatePercent"] = row.QualifiedRatePercent,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? ResolveTitle(query.StatTab),
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "质量统计"));
    }

    /// <summary>
    /// 按页签汇总检验单
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>行</returns>
    private async Task<List<TaktQualityOperationStatRowDto>> LoadRowsAsync(TaktQualityOperationStatQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        if (!TryParseTab(queryDto.StatTab, out var tab))
        {
            ThrowBusinessException("请选择统计页签");
        }
        if (!queryDto.PeriodDateStart.HasValue || !queryDto.PeriodDateEnd.HasValue)
        {
            ThrowBusinessException("请选择期间");
        }
        EnsureThreeLayerContext();
        var start = new DateTime(queryDto.PeriodDateStart.Value.Year, queryDto.PeriodDateStart.Value.Month, 1);
        var endMonth = new DateTime(queryDto.PeriodDateEnd.Value.Year, queryDto.PeriodDateEnd.Value.Month, 1);
        if (endMonth < start)
        {
            (start, endMonth) = (endMonth, start);
        }
        var endExclusive = endMonth.AddMonths(1);
        var plant = queryDto.PlantCode.Trim();
        var dimension = BlankToNull(queryDto.DimensionCode);
        var rows = tab switch
        {
            TabIqc => await LoadIqcAsync(plant, start, endExclusive, dimension),
            TabIpqc => await LoadIpqcAsync(plant, start, endExclusive, dimension),
            _ => await LoadFqcAsync(plant, start, endExclusive, dimension),
        };
        if (rows.Count > MaxRows)
        {
            ThrowBusinessException($"统计结果超过 {MaxRows} 行，请缩小期间或指定维度后再查询");
        }
        await FillDimensionNamesAsync(tab, rows);
        return rows;
    }

    /// <summary>
    /// 进货检验按供应商汇总
    /// </summary>
    private async Task<List<TaktQualityOperationStatRowDto>> LoadIqcAsync(string plant, DateTime start, DateTime endExclusive, string? dimension)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktIqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.InspectionDate >= start && x.InspectionDate < endExclusive);
        if (dimension != null)
        {
            query = query.Where(x => x.SupplierCode == dimension);
        }
        var grouped = await query
            .GroupBy(x => x.SupplierCode)
            .Select(x => new QualityAgg
            {
                DimensionCode = x.SupplierCode,
                SampleQty = SqlFunc.AggregateSum(x.TotalSampleQuantity),
                QualifiedQty = SqlFunc.AggregateSum(x.TotalQualifiedQuantity),
                UnqualifiedQty = SqlFunc.AggregateSum(x.TotalUnqualifiedQuantity),
                ReturnQty = SqlFunc.AggregateSum(x.TotalInspectionReturnQuantity),
                OrderCount = SqlFunc.AggregateCount(x.IqcOrderCode),
            })
            .ToListAsync();
        return MapRows(plant, grouped);
    }

    /// <summary>
    /// 制程检验按工序汇总
    /// </summary>
    private async Task<List<TaktQualityOperationStatRowDto>> LoadIpqcAsync(string plant, DateTime start, DateTime endExclusive, string? dimension)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktIpqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.InspectionDate >= start && x.InspectionDate < endExclusive);
        if (dimension != null)
        {
            query = query.Where(x => x.ProcessCode == dimension);
        }
        var grouped = await query
            .GroupBy(x => x.ProcessCode)
            .Select(x => new QualityAgg
            {
                DimensionCode = x.ProcessCode,
                DimensionName = SqlFunc.AggregateMax(x.ProcessName),
                SampleQty = SqlFunc.AggregateSum(x.TotalSampleQuantity),
                QualifiedQty = SqlFunc.AggregateSum(x.TotalQualifiedQuantity),
                UnqualifiedQty = SqlFunc.AggregateSum(x.TotalUnqualifiedQuantity),
                ReturnQty = SqlFunc.AggregateSum(x.TotalInspectionReturnQuantity),
                OrderCount = SqlFunc.AggregateCount(x.IpqcOrderCode),
            })
            .ToListAsync();
        return MapRows(plant, grouped);
    }

    /// <summary>
    /// 出货检验按客户汇总
    /// </summary>
    private async Task<List<TaktQualityOperationStatRowDto>> LoadFqcAsync(string plant, DateTime start, DateTime endExclusive, string? dimension)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktFqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.InspectionDate >= start && x.InspectionDate < endExclusive
                && x.CustomerCode != null && x.CustomerCode != "");
        if (dimension != null)
        {
            query = query.Where(x => x.CustomerCode == dimension);
        }
        var grouped = await query
            .GroupBy(x => x.CustomerCode)
            .Select(x => new QualityAgg
            {
                DimensionCode = x.CustomerCode,
                SampleQty = SqlFunc.AggregateSum(x.TotalSampleQuantity),
                QualifiedQty = SqlFunc.AggregateSum(x.TotalQualifiedQuantity),
                UnqualifiedQty = SqlFunc.AggregateSum(x.TotalUnqualifiedQuantity),
                ReturnQty = SqlFunc.AggregateSum(x.TotalInspectionReturnQuantity),
                OrderCount = SqlFunc.AggregateCount(x.FqcOrderCode),
            })
            .ToListAsync();
        return MapRows(plant, grouped);
    }

    /// <summary>
    /// 汇总行转统计行
    /// </summary>
    private static List<TaktQualityOperationStatRowDto> MapRows(string plant, List<QualityAgg> grouped)
    {
        return grouped
            .Select(x =>
            {
                var sample = (decimal)x.SampleQty;
                var unqualified = (decimal)x.UnqualifiedQty;
                var qualified = (decimal)x.QualifiedQty;
                var defectRate = sample == 0m ? 0m : Math.Round(unqualified / sample * 100m, 2, MidpointRounding.AwayFromZero);
                var qualifiedRate = sample == 0m ? 0m : Math.Round(qualified / sample * 100m, 2, MidpointRounding.AwayFromZero);
                return new TaktQualityOperationStatRowDto
                {
                    PlantCode = plant,
                    DimensionCode = x.DimensionCode?.Trim() ?? string.Empty,
                    DimensionName = x.DimensionName?.Trim() ?? string.Empty,
                    OrderCount = x.OrderCount,
                    SampleQty = sample,
                    QualifiedQty = qualified,
                    UnqualifiedQty = unqualified,
                    ReturnQty = x.ReturnQty,
                    DefectRatePercent = defectRate,
                    QualifiedRatePercent = qualifiedRate,
                };
            })
            .Where(x => x.DimensionCode.Length > 0)
            .OrderBy(x => x.DimensionCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 补供应商或客户名称
    /// </summary>
    private async Task FillDimensionNamesAsync(string tab, List<TaktQualityOperationStatRowDto> rows)
    {
        if (rows.Count == 0 || tab == TabIpqc)
        {
            return;
        }
        var codes = rows.Select(x => x.DimensionCode).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        Dictionary<string, string> names;
        if (tab == TabIqc)
        {
            var list = await _db.Queryable<TaktSupplier>()
                .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && codes.Contains(x.SupplierCode))
                .Select(x => new { x.SupplierCode, x.SupplierName1 })
                .ToListAsync();
            names = list
                .GroupBy(x => x.SupplierCode.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().SupplierName1 ?? string.Empty, StringComparer.OrdinalIgnoreCase);
        }
        else
        {
            var list = await _db.Queryable<TaktCustomer>()
                .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && x.CustomerCode != null && codes.Contains(x.CustomerCode))
                .Select(x => new { x.CustomerCode, x.CustomerName1 })
                .ToListAsync();
            names = list
                .GroupBy(x => (x.CustomerCode ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().CustomerName1 ?? string.Empty, StringComparer.OrdinalIgnoreCase);
        }
        foreach (var row in rows)
        {
            if (names.TryGetValue(row.DimensionCode, out var name))
            {
                row.DimensionName = name;
            }
        }
    }

    /// <summary>
    /// 去重选项
    /// </summary>
    private async Task<List<TaktSelectOption>> DistinctAsync(string? statTab, string? plantCode, string? keyword, bool plantOnly)
    {
        EnsureThreeLayerContext();
        if (!TryParseTab(statTab, out var tab))
        {
            return [];
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        if (!plantOnly && plant.Length == 0)
        {
            return [];
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        List<string> codes = tab switch
        {
            TabIqc => await DistinctIqcAsync(tenant, company, plant, plantOnly),
            TabIpqc => await DistinctIpqcAsync(tenant, company, plant, plantOnly),
            _ => await DistinctFqcAsync(tenant, company, plant, plantOnly),
        };
        return ToOptions(codes, keyword);
    }

    /// <summary>
    /// 进货检验去重
    /// </summary>
    private async Task<List<string>> DistinctIqcAsync(string tenant, string company, string plant, bool plantOnly)
    {
        var query = _db.Queryable<TaktIqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        return plantOnly
            ? await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync()
            : await query.GroupBy(x => x.SupplierCode).Select(x => x.SupplierCode).ToListAsync();
    }

    /// <summary>
    /// 制程检验去重
    /// </summary>
    private async Task<List<string>> DistinctIpqcAsync(string tenant, string company, string plant, bool plantOnly)
    {
        var query = _db.Queryable<TaktIpqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        return plantOnly
            ? await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync()
            : await query.GroupBy(x => x.ProcessCode).Select(x => x.ProcessCode).ToListAsync();
    }

    /// <summary>
    /// 出货检验去重
    /// </summary>
    private async Task<List<string>> DistinctFqcAsync(string tenant, string company, string plant, bool plantOnly)
    {
        var query = _db.Queryable<TaktFqcOrder>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (plantOnly)
        {
            return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
        }
        return await query.Where(x => x.CustomerCode != null && x.CustomerCode != "")
            .GroupBy(x => x.CustomerCode)
            .Select(x => x.CustomerCode!)
            .ToListAsync();
    }

    /// <summary>
    /// 编码转下拉
    /// </summary>
    private static List<TaktSelectOption> ToOptions(IEnumerable<string?> codes, string? keyword)
    {
        var kw = keyword?.Trim();
        return codes
            .Select(c => c?.Trim() ?? string.Empty)
            .Where(c => c.Length > 0)
            .Where(c => string.IsNullOrEmpty(kw) || c.Contains(kw, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .Select(c => new TaktSelectOption { DictValue = c, DictLabel = c })
            .ToList();
    }

    /// <summary>
    /// 解析页签
    /// </summary>
    private static bool TryParseTab(string? statTab, out string tab)
    {
        tab = statTab?.Trim().ToLowerInvariant() ?? string.Empty;
        return tab is TabIqc or TabIpqc or TabFqc;
    }

    /// <summary>
    /// 维度列标题
    /// </summary>
    private static (string Code, string Name) DimensionLabels(string? statTab) =>
        statTab?.Trim().ToLowerInvariant() switch
        {
            TabIpqc => ("工序", "工序名称"),
            TabFqc => ("客户", "客户名称"),
            _ => ("供应商", "供应商名称"),
        };

    /// <summary>
    /// 页签名称
    /// </summary>
    private static string ResolveTitle(string? statTab) =>
        statTab?.Trim().ToLowerInvariant() switch
        {
            TabIpqc => "IPQC",
            TabFqc => "FQC",
            _ => "IQC",
        };

    /// <summary>
    /// 空白转 null
    /// </summary>
    private static string? BlankToNull(string? value)
    {
        var text = value?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// 检验单汇总行
    /// </summary>
    private sealed class QualityAgg
    {
        /// <summary>
        /// 维度编码
        /// </summary>
        public string? DimensionCode { get; set; }

        /// <summary>
        /// 维度名称
        /// </summary>
        public string? DimensionName { get; set; }

        /// <summary>
        /// 抽样数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public int QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedQty { get; set; }

        /// <summary>
        /// 验退数量
        /// </summary>
        public decimal ReturnQty { get; set; }

        /// <summary>
        /// 检验单数
        /// </summary>
        public int OrderCount { get; set; }
    }
}

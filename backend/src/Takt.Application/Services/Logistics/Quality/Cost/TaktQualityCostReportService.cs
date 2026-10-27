// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityCostReportService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告。保证按记录组、问题按机种、事故按废弃组汇总金额
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质成本报告
/// </summary>
public class TaktQualityCostReportService : TaktServiceBase, ITaktQualityCostReportService
{
    private const string TabAssurance = "assurance";
    private const string TabIssue = "issue";
    private const string TabIncident = "incident";
    private const int MaxRows = 100000;

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktQualityCostReportService(
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetQualityCostReportPlantOptionsAsync(string? statTab, string? keyword = null)
    {
        return DistinctAsync(statTab, null, keyword, "plant");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetQualityCostReportDimensionOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "dimension");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetQualityCostReportCurrencyOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "currency");
    }

    /// <inheritdoc />
    public async Task<TaktQualityCostReportResultDto> GetQualityCostReportListAsync(TaktQualityCostReportQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var rows = await LoadRowsAsync(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var page = rows.Skip(skip).Take(pageSize).ToList();
        return new TaktQualityCostReportResultDto
        {
            Paged = TaktPagedResult<TaktQualityCostReportRowDto>.Create(page, rows.Count, pageIndex, pageSize),
            RowCount = rows.Count,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportQualityCostReportAsync(
        TaktQualityCostReportQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var rows = await LoadRowsAsync(query);
        var tab = query.StatTab?.Trim().ToLowerInvariant() ?? string.Empty;
        var (codeLabel, nameLabel, qtyLabel) = ColumnLabels(tab);
        var columnKeys = new List<string> { "plantCode", "dimensionCode", "currencyCode", "orderCount", "amount" };
        var columnLabels = new List<string> { "工厂", codeLabel, "币种", "单据数", "金额" };
        if (tab != TabIssue)
        {
            columnKeys.Insert(2, "dimensionName");
            columnLabels.Insert(2, nameLabel);
        }
        if (tab != TabAssurance)
        {
            columnKeys.Insert(columnKeys.Count - 1, "quantity");
            columnLabels.Insert(columnLabels.Count - 1, qtyLabel);
        }
        var exportRows = rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["plantCode"] = row.PlantCode,
            ["dimensionCode"] = row.DimensionCode,
            ["dimensionName"] = row.DimensionName,
            ["currencyCode"] = row.CurrencyCode,
            ["orderCount"] = row.OrderCount,
            ["quantity"] = row.Quantity,
            ["amount"] = row.Amount,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? ResolveTitle(tab),
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "品质成本报告"));
    }

    /// <summary>
    /// 按页签汇总
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>行</returns>
    private async Task<List<TaktQualityCostReportRowDto>> LoadRowsAsync(TaktQualityCostReportQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        if (!TryParseTab(queryDto.StatTab, out var tab))
        {
            ThrowBusinessException("请选择报告页签");
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
        var startKey = start.ToString("yyyy-MM");
        var endKey = endMonth.ToString("yyyy-MM");
        var plant = queryDto.PlantCode.Trim();
        var dimension = BlankToNull(queryDto.DimensionCode);
        var currency = BlankToNull(queryDto.CurrencyCode);
        var rows = tab switch
        {
            TabAssurance => await LoadAssuranceAsync(plant, startKey, endKey, dimension, currency),
            TabIssue => await LoadIssueAsync(plant, start, endExclusive, dimension, currency),
            _ => await LoadIncidentAsync(plant, start, endExclusive, dimension, currency),
        };
        if (rows.Count > MaxRows)
        {
            ThrowBusinessException($"报告结果超过 {MaxRows} 行，请缩小期间或指定维度后再查询");
        }
        return rows;
    }

    /// <summary>
    /// 品质保证按记录组和币种汇总
    /// </summary>
    private async Task<List<TaktQualityCostReportRowDto>> LoadAssuranceAsync(
        string plant, string startKey, string endKey, string? dimension, string? currency)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktQualityAssurance>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.AssuranceMonth.CompareTo(startKey) >= 0
                && x.AssuranceMonth.CompareTo(endKey) <= 0);
        if (dimension != null)
        {
            query = query.Where(x => x.RecordGroupCode == dimension);
        }
        if (currency != null)
        {
            query = query.Where(x => x.CurrencyCode == currency);
        }
        var grouped = await query
            .GroupBy(x => new { x.RecordGroupCode, x.CurrencyCode })
            .Select(x => new CostAgg
            {
                DimensionCode = x.RecordGroupCode,
                DimensionName = SqlFunc.AggregateMax(x.RecordGroupName),
                CurrencyCode = x.CurrencyCode,
                OrderCount = SqlFunc.AggregateCount(x.QualityAssuranceCode),
                Amount = SqlFunc.AggregateSum(x.TotalQualityCost),
            })
            .ToListAsync();
        return MapRows(plant, grouped, useTime: false);
    }

    /// <summary>
    /// 品质问题按机种和币种汇总
    /// </summary>
    private async Task<List<TaktQualityCostReportRowDto>> LoadIssueAsync(
        string plant, DateTime start, DateTime endExclusive, string? dimension, string? currency)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktQualityIssue>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.IssueDate >= start && x.IssueDate < endExclusive);
        if (dimension != null)
        {
            query = query.Where(x => x.Model == dimension);
        }
        if (currency != null)
        {
            query = query.Where(x => x.CurrencyCode == currency);
        }
        var grouped = await query
            .GroupBy(x => new { x.Model, x.CurrencyCode })
            .Select(x => new CostAgg
            {
                DimensionCode = x.Model,
                CurrencyCode = x.CurrencyCode,
                OrderCount = SqlFunc.AggregateCount(x.QualityIssueCode),
                TimeMinutes = SqlFunc.AggregateSum(x.TotalTimeMinutes),
                Amount = SqlFunc.AggregateSum(x.TotalCost),
            })
            .ToListAsync();
        return MapRows(plant, grouped, useTime: true);
    }

    /// <summary>
    /// 品质事故按废弃组和币种汇总
    /// </summary>
    private async Task<List<TaktQualityCostReportRowDto>> LoadIncidentAsync(
        string plant, DateTime start, DateTime endExclusive, string? dimension, string? currency)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktQualityIncident>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant
                && x.IncidentDate >= start && x.IncidentDate < endExclusive);
        if (dimension != null)
        {
            query = query.Where(x => x.ScrapGroupCode == dimension);
        }
        if (currency != null)
        {
            query = query.Where(x => x.CurrencyCode == currency);
        }
        var grouped = await query
            .GroupBy(x => new { x.ScrapGroupCode, x.CurrencyCode })
            .Select(x => new CostAgg
            {
                DimensionCode = x.ScrapGroupCode,
                DimensionName = SqlFunc.AggregateMax(x.ScrapGroupName),
                CurrencyCode = x.CurrencyCode,
                OrderCount = SqlFunc.AggregateCount(x.QualityIncidentCode),
                Quantity = SqlFunc.AggregateSum(x.TotalScrapQuantity),
                Amount = SqlFunc.AggregateSum(x.TotalScrapCost),
            })
            .ToListAsync();
        return MapRows(plant, grouped, useTime: false);
    }

    /// <summary>
    /// 汇总行转报告行
    /// </summary>
    private static List<TaktQualityCostReportRowDto> MapRows(string plant, List<CostAgg> grouped, bool useTime)
    {
        return grouped
            .Select(x => new TaktQualityCostReportRowDto
            {
                PlantCode = plant,
                DimensionCode = x.DimensionCode?.Trim() ?? string.Empty,
                DimensionName = x.DimensionName?.Trim() ?? string.Empty,
                CurrencyCode = x.CurrencyCode?.Trim() ?? string.Empty,
                OrderCount = x.OrderCount,
                Amount = x.Amount,
                Quantity = useTime ? x.TimeMinutes : x.Quantity,
            })
            .OrderBy(x => x.DimensionCode, StringComparer.Ordinal)
            .ThenBy(x => x.CurrencyCode, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 去重选项
    /// </summary>
    private async Task<List<TaktSelectOption>> DistinctAsync(string? statTab, string? plantCode, string? keyword, string field)
    {
        EnsureThreeLayerContext();
        if (!TryParseTab(statTab, out var tab))
        {
            return [];
        }
        var plant = plantCode?.Trim() ?? string.Empty;
        if (field != "plant" && plant.Length == 0)
        {
            return [];
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        List<string> codes = tab switch
        {
            TabAssurance => await DistinctAssuranceAsync(tenant, company, plant, field),
            TabIssue => await DistinctIssueAsync(tenant, company, plant, field),
            _ => await DistinctIncidentAsync(tenant, company, plant, field),
        };
        return ToOptions(codes, keyword);
    }

    /// <summary>
    /// 品质保证去重
    /// </summary>
    private async Task<List<string>> DistinctAssuranceAsync(string tenant, string company, string plant, string field)
    {
        var query = _db.Queryable<TaktQualityAssurance>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (field == "plant")
        {
            return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
        }
        if (field == "currency")
        {
            return await query.GroupBy(x => x.CurrencyCode).Select(x => x.CurrencyCode).ToListAsync();
        }
        return await query.Where(x => x.RecordGroupCode != null && x.RecordGroupCode != "")
            .GroupBy(x => x.RecordGroupCode)
            .Select(x => x.RecordGroupCode ?? string.Empty)
            .ToListAsync();
    }

    /// <summary>
    /// 品质问题去重
    /// </summary>
    private async Task<List<string>> DistinctIssueAsync(string tenant, string company, string plant, string field)
    {
        var query = _db.Queryable<TaktQualityIssue>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (field == "plant")
        {
            return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
        }
        if (field == "currency")
        {
            return await query.GroupBy(x => x.CurrencyCode).Select(x => x.CurrencyCode).ToListAsync();
        }
        return await query.GroupBy(x => x.Model).Select(x => x.Model).ToListAsync();
    }

    /// <summary>
    /// 品质事故去重
    /// </summary>
    private async Task<List<string>> DistinctIncidentAsync(string tenant, string company, string plant, string field)
    {
        var query = _db.Queryable<TaktQualityIncident>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        if (field == "plant")
        {
            return await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync();
        }
        if (field == "currency")
        {
            return await query.GroupBy(x => x.CurrencyCode).Select(x => x.CurrencyCode).ToListAsync();
        }
        return await query.Where(x => x.ScrapGroupCode != null && x.ScrapGroupCode != "")
            .GroupBy(x => x.ScrapGroupCode)
            .Select(x => x.ScrapGroupCode ?? string.Empty)
            .ToListAsync();
    }

    /// <summary>
    /// 编码转下拉
    /// </summary>
    private static List<TaktSelectOption> ToOptions(IEnumerable<string> codes, string? keyword)
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
        return tab is TabAssurance or TabIssue or TabIncident;
    }

    /// <summary>
    /// 列标题
    /// </summary>
    private static (string Code, string Name, string Quantity) ColumnLabels(string tab) =>
        tab switch
        {
            TabIssue => ("机种", "机种", "总时间"),
            TabIncident => ("废弃组", "废弃组名称", "废弃数量"),
            _ => ("记录组", "记录组名称", "数量"),
        };

    /// <summary>
    /// 页签名称
    /// </summary>
    private static string ResolveTitle(string tab) =>
        tab switch
        {
            TabIssue => "品质问题",
            TabIncident => "品质事故",
            _ => "品质保证",
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
    /// 成本汇总行
    /// </summary>
    private sealed class CostAgg
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
        /// 币种
        /// </summary>
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// 单据数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 废弃数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 总时间分钟
        /// </summary>
        public int TimeMinutes { get; set; }
    }
}

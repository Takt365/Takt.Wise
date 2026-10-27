// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产推移转置分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计：按页签只查组立或 PCBA，按班组或按机种汇总产量
/// </summary>
public partial class TaktProductionMonthlyTrendService : TaktServiceBase, ITaktProductionMonthlyTrendService
{
  private const string CategoryAssy = "assy";
  private const string CategoryPcba = "pcba";
  private const string TabMfg1Team = "assy-team";
  private const string TabMfg1Model = "assy-model";
  private const string TabMfg2Team = "pcba-team";
  private const string TabMfg2Model = "pcba-model";

  private readonly ISqlSugarClient _db;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="db">当前请求的 SqlSugar 客户端</param>
  /// <param name="userContext">用户上下文</param>
  /// <param name="localizationService">本地化服务</param>
  public TaktProductionMonthlyTrendService(
      ISqlSugarClient db,
      ITaktUserContext? userContext = null,
      ITaktLocalizationService? localizationService = null)
      : base(userContext, localizationService)
  {
    _db = db;
  }

  /// <inheritdoc />
  public async Task<List<TaktSelectOption>> GetProductionMonthlyTrendPlantOptionsAsync(string? statTab, string? keyword = null)
  {
    EnsureThreeLayerContext();
    if (!TryParseStatTab(statTab, out var scope))
    {
      return [];
    }
    var tenant = CurrentTenantCode;
    var company = CurrentCompanyCode;
    List<string> codes;
    if (scope.IsAssy)
    {
      codes = await _db.Queryable<TaktAssyOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode != null && x.PlantCode != "")
          .GroupBy(x => x.PlantCode)
          .Select(x => x.PlantCode)
          .ToListAsync();
    }
    else
    {
      codes = await _db.Queryable<TaktPcbaOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode != null && x.PlantCode != "")
          .GroupBy(x => x.PlantCode)
          .Select(x => x.PlantCode)
          .ToListAsync();
    }
    return ToCodeOptions(codes, keyword);
  }

  /// <inheritdoc />
  public async Task<List<TaktSelectOption>> GetProductionMonthlyTrendTeamOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (plant.Length == 0 || !TryParseStatTab(statTab, out var scope))
    {
      return [];
    }
    var tenant = CurrentTenantCode;
    var company = CurrentCompanyCode;
    List<string> codes;
    if (scope.IsAssy)
    {
      codes = await _db.Queryable<TaktAssyOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode == plant && x.TeamCode != null && x.TeamCode != "")
          .GroupBy(x => x.TeamCode)
          .Select(x => x.TeamCode)
          .ToListAsync();
    }
    else
    {
      codes = await _db.Queryable<TaktPcbaOutput>()
          .InnerJoin<TaktPcbaOutputDetail>((h, d) => h.Id == d.PcbaOutputId)
          .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && d.IsDeleted == 0
              && h.PlantCode == plant && d.TeamCode != null && d.TeamCode != "")
          .GroupBy((h, d) => d.TeamCode)
          .Select((h, d) => d.TeamCode)
          .ToListAsync();
    }
    return ToCodeOptions(codes, keyword);
  }

  /// <inheritdoc />
  public Task<List<TaktSelectOption>> GetProductionMonthlyTrendModelOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
  {
    return DistinctHeaderCodesAsync(statTab, plantCode, keyword, model: true);
  }

  /// <inheritdoc />
  public Task<List<TaktSelectOption>> GetProductionMonthlyTrendMaterialOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
  {
    return DistinctHeaderCodesAsync(statTab, plantCode, keyword, model: false);
  }

  /// <summary>
  /// 获取月生产推移转置分析（分页）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  public async Task<TaktProductionMonthlyTrendResultDto> GetProductionMonthlyTrendAnalysisAsync(
      TaktProductionMonthlyTrendQueryDto queryDto)
  {
    ArgumentNullException.ThrowIfNull(queryDto);
    var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
    var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
    var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
    var built = await BuildProductionMonthlyTrendAnalysisAsync(queryDto);
    var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
    return new TaktProductionMonthlyTrendResultDto
    {
      Paged = TaktPagedResult<TaktProductionMonthlyTrendDto>.Create(
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
  /// 导出月生产推移转置分析 Excel
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="fileName">导出文件名</param>
  /// <returns>文件名与内容</returns>
  public async Task<(string fileName, byte[] fileContent)> ExportProductionMonthlyTrendAnalysisAsync(
      TaktProductionMonthlyTrendQueryDto query,
      string? sheetName = null,
      string? fileName = null)
  {
    ArgumentNullException.ThrowIfNull(query);
    var built = await BuildProductionMonthlyTrendAnalysisAsync(query);
    var groupByTeam = TryParseStatTab(query.StatTab, out var scope) && scope.GroupByTeam;
    var groupKey = groupByTeam ? "teamCode" : "modelCode";
    var groupLabel = groupByTeam ? "班组" : "机种";
    var columnKeys = new List<string>
    {
      "plantCode", groupKey,
    };
    var columnLabels = new List<string>
    {
      "工厂代码", groupLabel,
    };
    foreach (var period in built.PeriodOrder)
    {
      columnKeys.Add($"period_{period}");
      columnLabels.Add(period);
    }
    columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
    columnLabels.AddRange(new[] { "基准月", "对比月", "环比差额", "环比%", "涨跌" });
    var exportRows = built.OrderedRows.Select(row =>
    {
      var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
      {
        ["plantCode"] = row.PlantCode,
        ["teamCode"] = row.TeamCode,
        ["modelCode"] = row.ModelCode,
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
        dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var qty)
            ? qty
            : null;
      }
      return (IReadOnlyDictionary<string, object?>)dict;
    }).ToList();
    return await TaktExcelHelper.ExportDictionaryRowsAsync(
        exportRows,
        columnKeys,
        columnLabels,
        sheetName ?? ResolveStatTabTitle(query.StatTab),
        fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "生产统计"));
  }

  /// <summary>
  /// 构建月生产推移转置分析全量结果
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>内存构建结果</returns>
  private async Task<ProductionMonthlyTrendAnalysisBuilt> BuildProductionMonthlyTrendAnalysisAsync(
      TaktProductionMonthlyTrendQueryDto queryDto)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
    if (!TryParseStatTab(queryDto.StatTab, out var scope))
    {
      ThrowBusinessException("请选择统计页签");
    }
    EnsureThreeLayerContext();
    var plantCode = queryDto.PlantCode.Trim();
    var teamFilter = string.IsNullOrWhiteSpace(queryDto.TeamCode) ? null : queryDto.TeamCode.Trim();
    var modelFilter = string.IsNullOrWhiteSpace(queryDto.ModelCode) ? null : queryDto.ModelCode.Trim();
    var materialFilter = string.IsNullOrWhiteSpace(queryDto.MaterialCode) ? null : queryDto.MaterialCode.Trim();
    var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
    if (periodOrder.Count == 0)
    {
      return ProductionMonthlyTrendAnalysisBuilt.Empty();
    }
    var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
    var sourceRows = await LoadStatSourceRowsAsync(
        scope,
        plantCode,
        teamFilter,
        modelFilter,
        materialFilter,
        rangeStart,
        rangeEnd);
    if (sourceRows.Count == 0)
    {
      return ProductionMonthlyTrendAnalysisBuilt.Empty();
    }
    var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
    var allRows = sourceRows
        .GroupBy(
            r => new ProductionMonthlyTrendRowKey(r.PlantCode, r.TeamCode, r.ModelCode),
            ProductionMonthlyTrendRowKeyComparer.Instance)
        .Select(g => BuildProductionMonthlyTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
        .ToList();
    var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
    var ordered = OrderTrendRows(filtered);
    return new ProductionMonthlyTrendAnalysisBuilt
    {
      OrderedRows = ordered,
      PeriodOrder = periodOrder,
      BasePeriod = allRows.FirstOrDefault()?.BasePeriod,
      ComparePeriod = focusPeriod,
      UpCount = allRows.Count(r => r.Trend == "up"),
      DownCount = allRows.Count(r => r.Trend == "down"),
      FlatCount = allRows.Count(r => r.Trend == "flat"),
      NoneCount = allRows.Count(r => r.Trend == "none"),
    };
  }

  /// <summary>
  /// 按页签汇总产量：组立主表班组/机种，PCBA 明细班组或主表机种
  /// </summary>
  /// <param name="scope">页签</param>
  /// <param name="plantCode">工厂</param>
  /// <param name="teamFilter">班组</param>
  /// <param name="modelFilter">机种</param>
  /// <param name="materialFilter">物料</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>源行</returns>
  private async Task<List<ProductionMonthlyTrendSourceRow>> LoadStatSourceRowsAsync(
      StatTabScope scope,
      string plantCode,
      string? teamFilter,
      string? modelFilter,
      string? materialFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var tenant = CurrentTenantCode;
    var company = CurrentCompanyCode;
    var category = scope.IsAssy ? CategoryAssy : CategoryPcba;
    List<StatMonthQty> grouped;
    if (scope.IsAssy && scope.GroupByTeam)
    {
      var query = _db.Queryable<TaktAssyOutput>()
          .InnerJoin<TaktAssyOutputDetail>((h, d) => h.Id == d.AssyOutputId)
          .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && d.IsDeleted == 0
              && d.IsObsolete == 0 && h.PlantCode == plantCode
              && h.ProdDate >= rangeStart && h.ProdDate < rangeEndExclusive
              && h.TeamCode != null && h.TeamCode != "");
      if (teamFilter != null)
      {
        query = query.Where((h, d) => h.TeamCode == teamFilter);
      }
      if (modelFilter != null)
      {
        query = query.Where((h, d) => h.ModelCode == modelFilter);
      }
      if (materialFilter != null)
      {
        query = query.Where((h, d) => h.MaterialCode == materialFilter);
      }
      grouped = await query
          .GroupBy((h, d) => new { h.TeamCode, h.ProdDate.Year, h.ProdDate.Month })
          .Select((h, d) => new StatMonthQty
          {
            GroupCode = h.TeamCode,
            Year = h.ProdDate.Year,
            Month = h.ProdDate.Month,
            Qty = SqlFunc.AggregateSum(d.ProdActualQty),
          })
          .ToListAsync();
    }
    else if (scope.IsAssy)
    {
      var query = _db.Queryable<TaktAssyOutput>()
          .InnerJoin<TaktAssyOutputDetail>((h, d) => h.Id == d.AssyOutputId)
          .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && d.IsDeleted == 0
              && d.IsObsolete == 0 && h.PlantCode == plantCode
              && h.ProdDate >= rangeStart && h.ProdDate < rangeEndExclusive
              && h.ModelCode != null && h.ModelCode != "");
      if (teamFilter != null)
      {
        query = query.Where((h, d) => h.TeamCode == teamFilter);
      }
      if (modelFilter != null)
      {
        query = query.Where((h, d) => h.ModelCode == modelFilter);
      }
      if (materialFilter != null)
      {
        query = query.Where((h, d) => h.MaterialCode == materialFilter);
      }
      grouped = await query
          .GroupBy((h, d) => new { h.ModelCode, h.ProdDate.Year, h.ProdDate.Month })
          .Select((h, d) => new StatMonthQty
          {
            GroupCode = h.ModelCode,
            Year = h.ProdDate.Year,
            Month = h.ProdDate.Month,
            Qty = SqlFunc.AggregateSum(d.ProdActualQty),
          })
          .ToListAsync();
    }
    else if (scope.GroupByTeam)
    {
      var query = _db.Queryable<TaktPcbaOutput>()
          .InnerJoin<TaktPcbaOutputDetail>((h, d) => h.Id == d.PcbaOutputId)
          .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && d.IsDeleted == 0
              && d.IsObsolete == 0 && h.PlantCode == plantCode
              && h.ProdDate >= rangeStart && h.ProdDate < rangeEndExclusive
              && d.TeamCode != null && d.TeamCode != "");
      if (teamFilter != null)
      {
        query = query.Where((h, d) => d.TeamCode == teamFilter);
      }
      if (modelFilter != null)
      {
        query = query.Where((h, d) => h.ModelCode == modelFilter);
      }
      if (materialFilter != null)
      {
        query = query.Where((h, d) => h.MaterialCode == materialFilter);
      }
      grouped = await query
          .GroupBy((h, d) => new { d.TeamCode, h.ProdDate.Year, h.ProdDate.Month })
          .Select((h, d) => new StatMonthQty
          {
            GroupCode = d.TeamCode,
            Year = h.ProdDate.Year,
            Month = h.ProdDate.Month,
            Qty = SqlFunc.AggregateSum(d.DailyCompletedQty),
          })
          .ToListAsync();
    }
    else
    {
      var query = _db.Queryable<TaktPcbaOutput>()
          .InnerJoin<TaktPcbaOutputDetail>((h, d) => h.Id == d.PcbaOutputId)
          .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && d.IsDeleted == 0
              && d.IsObsolete == 0 && h.PlantCode == plantCode
              && h.ProdDate >= rangeStart && h.ProdDate < rangeEndExclusive
              && h.ModelCode != null && h.ModelCode != "");
      if (teamFilter != null)
      {
        query = query.Where((h, d) => d.TeamCode == teamFilter);
      }
      if (modelFilter != null)
      {
        query = query.Where((h, d) => h.ModelCode == modelFilter);
      }
      if (materialFilter != null)
      {
        query = query.Where((h, d) => h.MaterialCode == materialFilter);
      }
      grouped = await query
          .GroupBy((h, d) => new { h.ModelCode, h.ProdDate.Year, h.ProdDate.Month })
          .Select((h, d) => new StatMonthQty
          {
            GroupCode = h.ModelCode,
            Year = h.ProdDate.Year,
            Month = h.ProdDate.Month,
            Qty = SqlFunc.AggregateSum(d.DailyCompletedQty),
          })
          .ToListAsync();
    }
    var rows = new List<ProductionMonthlyTrendSourceRow>(grouped.Count);
    foreach (var item in grouped)
    {
      var code = item.GroupCode?.Trim() ?? string.Empty;
      if (code.Length == 0 || item.Month is < 1 or > 12)
      {
        continue;
      }
      rows.Add(new ProductionMonthlyTrendSourceRow
      {
        PlantCode = plantCode,
        TeamCode = scope.GroupByTeam ? code : string.Empty,
        ModelCode = scope.GroupByTeam ? string.Empty : code,
        OutputCategory = category,
        Period = new DateTime(item.Year, item.Month, 1).ToString("yyyy-MM"),
        Qty = item.Qty,
      });
    }
    return rows;
  }

  /// <summary>
  /// 解析期间列顺序
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <param name="rangeStart">区间起</param>
  /// <param name="rangeEnd">区间止</param>
  /// <returns>期间列顺序</returns>
  private static List<string> ResolvePeriodOrder(
      TaktProductionMonthlyTrendQueryDto queryDto,
      out DateTime rangeStart,
      out DateTime rangeEnd)
  {
    var (periodStart, periodEnd) = NormalizePeriodBounds(queryDto.PeriodDateStart, queryDto.PeriodDateEnd);
    if (periodStart.HasValue || periodEnd.HasValue)
    {
      var startMonth = periodStart ?? periodEnd!.Value;
      var endMonth = periodEnd ?? periodStart!.Value;
      if (startMonth > endMonth)
      {
        (startMonth, endMonth) = (endMonth, startMonth);
      }
      var monthCount = ((endMonth.Year - startMonth.Year) * 12) + endMonth.Month - startMonth.Month + 1;
      if (monthCount > TaktPriceTrendAnalysisHelper.MaxTrendMonths)
      {
        throw new ArgumentException($"分析区间不得超过 {TaktPriceTrendAnalysisHelper.MaxTrendMonths} 个月");
      }
      rangeStart = startMonth;
      rangeEnd = endMonth;
      return BuildConsecutivePeriodOrder(startMonth, endMonth);
    }
    var (resolvedStart, resolvedEnd) = TaktPriceTrendAnalysisHelper.ResolveTrendDateRange(null, null);
    rangeStart = new DateTime(resolvedStart.Year, resolvedStart.Month, 1);
    rangeEnd = new DateTime(resolvedEnd.Year, resolvedEnd.Month, 1);
    return BuildConsecutivePeriodOrder(rangeStart, rangeEnd);
  }

  /// <summary>
  /// 归一化期间起止到月初
  /// </summary>
  /// <param name="periodDateStart">期间起</param>
  /// <param name="periodDateEnd">期间止</param>
  /// <returns>月初起止</returns>
  private static (DateTime? Start, DateTime? End) NormalizePeriodBounds(
      DateTime? periodDateStart,
      DateTime? periodDateEnd)
  {
    DateTime? start = periodDateStart.HasValue
        ? new DateTime(periodDateStart.Value.Year, periodDateStart.Value.Month, 1)
        : null;
    DateTime? end = periodDateEnd.HasValue
        ? new DateTime(periodDateEnd.Value.Year, periodDateEnd.Value.Month, 1)
        : null;
    if (start.HasValue && end.HasValue && start > end)
    {
      (start, end) = (end, start);
    }
    return (start, end);
  }

  /// <summary>
  /// 构建连续 yyyy-MM 期间列
  /// </summary>
  /// <param name="periodStart">期间起（月初）</param>
  /// <param name="periodEnd">期间止（月初）</param>
  /// <returns>期间列顺序</returns>
  private static List<string> BuildConsecutivePeriodOrder(DateTime periodStart, DateTime periodEnd)
  {
    var order = new List<string>();
    for (var cursor = periodStart; cursor <= periodEnd; cursor = cursor.AddMonths(1))
    {
      order.Add(cursor.ToString("yyyy-MM"));
    }
    return order;
  }

  /// <summary>
  /// 解析关注期间
  /// </summary>
  /// <param name="focusPeriod">关注期间</param>
  /// <param name="periodOrder">期间列</param>
  /// <returns>关注期间 yyyy-MM</returns>
  private static string? ResolveFocusPeriod(string? focusPeriod, IReadOnlyList<string> periodOrder)
  {
    if (!string.IsNullOrWhiteSpace(focusPeriod))
    {
      return focusPeriod.Trim();
    }
    return periodOrder.Count > 0 ? periodOrder[^1] : null;
  }

  /// <summary>
  /// 构建单行月生产推移
  /// </summary>
  /// <param name="key">行键</param>
  /// <param name="groupRows">同键源行</param>
  /// <param name="periodSet">展示期间集合</param>
  /// <param name="focusPeriod">关注期间</param>
  /// <returns>转置行</returns>
  private static TaktProductionMonthlyTrendDto BuildProductionMonthlyTrendRow(
      ProductionMonthlyTrendRowKey key,
      IReadOnlyList<ProductionMonthlyTrendSourceRow> groupRows,
      IReadOnlySet<string> periodSet,
      string? focusPeriod)
  {
    var row = new TaktProductionMonthlyTrendDto
    {
      PlantCode = key.PlantCode,
      TeamCode = key.TeamCode,
      ModelCode = key.ModelCode,
      OutputCategory = groupRows.FirstOrDefault()?.OutputCategory ?? string.Empty,
      OutputCategoryName = ResolveCategoryName(groupRows.FirstOrDefault()?.OutputCategory ?? string.Empty),
      Trend = "none",
    };
    foreach (var period in groupRows
                 .Where(r => periodSet.Contains(r.Period))
                 .GroupBy(r => r.Period, StringComparer.Ordinal))
    {
      row.PeriodValues[period.Key] = RoundQty(period.Sum(r => r.Qty));
    }
    ApplyFocusTrend(row, focusPeriod);
    return row;
  }

  /// <summary>
  /// 解析产出类别显示名
  /// </summary>
  /// <param name="category">类别码</param>
  /// <returns>显示名</returns>
  private static string? ResolveCategoryName(string category) =>
      category switch
      {
        CategoryAssy => "组立",
        CategoryPcba => "PCBA",
        _ => null,
      };

  /// <summary>
  /// 按关注月计算环比涨跌
  /// </summary>
  /// <param name="row">转置行</param>
  /// <param name="focusPeriod">关注期间 yyyy-MM</param>
  private static void ApplyFocusTrend(TaktProductionMonthlyTrendDto row, string? focusPeriod)
  {
    if (string.IsNullOrWhiteSpace(focusPeriod))
    {
      return;
    }
    var comparePeriod = focusPeriod.Trim();
    if (!DateTime.TryParseExact(
            comparePeriod + "-01",
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out var compareMonth))
    {
      return;
    }
    var basePeriod = compareMonth.AddMonths(-1).ToString("yyyy-MM");
    row.BasePeriod = basePeriod;
    row.ComparePeriod = comparePeriod;
    if (!row.PeriodValues.TryGetValue(basePeriod, out var baseQty)
        || !row.PeriodValues.TryGetValue(comparePeriod, out var compareQty))
    {
      row.Trend = "none";
      return;
    }
    row.VarianceAmount = RoundQty(compareQty - baseQty);
    if (baseQty != 0m)
    {
      row.VariancePercent = Math.Round(
          row.VarianceAmount.Value / baseQty,
          4,
          MidpointRounding.AwayFromZero);
    }
    if (compareQty > baseQty)
    {
      row.Trend = "up";
    }
    else if (compareQty < baseQty)
    {
      row.Trend = "down";
    }
    else
    {
      row.Trend = "flat";
    }
  }

  /// <summary>
  /// 涨跌筛选
  /// </summary>
  /// <param name="rows">全量行</param>
  /// <param name="trendFilter">筛选码</param>
  /// <returns>筛选后行</returns>
  private static List<TaktProductionMonthlyTrendDto> FilterTrendRows(
      IReadOnlyList<TaktProductionMonthlyTrendDto> rows,
      string? trendFilter)
  {
    if (string.IsNullOrWhiteSpace(trendFilter))
    {
      return rows.ToList();
    }
    var filter = trendFilter.Trim().ToLowerInvariant();
    if (filter == "changed")
    {
      return rows.Where(r => r.Trend is "up" or "down").ToList();
    }
    return rows.Where(r => string.Equals(r.Trend, filter, StringComparison.OrdinalIgnoreCase)).ToList();
  }

  /// <summary>
  /// 涨跌优先排序
  /// </summary>
  /// <param name="rows">行集合</param>
  /// <returns>排序后行</returns>
  private static List<TaktProductionMonthlyTrendDto> OrderTrendRows(
      IReadOnlyList<TaktProductionMonthlyTrendDto> rows)
  {
    static int TrendRank(string? trend) => trend switch
    {
      "up" => 0,
      "down" => 1,
      "flat" => 2,
      _ => 3,
    };
    return rows
        .OrderBy(r => TrendRank(r.Trend))
        .ThenByDescending(r => Math.Abs(r.VarianceAmount ?? 0m))
        .ThenBy(r => r.TeamCode, StringComparer.Ordinal)
        .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
        .ToList();
  }

  /// <summary>
  /// 产量四舍五入至 1 位
  /// </summary>
  /// <param name="value">产量</param>
  /// <returns>四舍五入后产量</returns>
  private static decimal RoundQty(decimal value) =>
      Math.Round(value, 1, MidpointRounding.AwayFromZero);

  /// <summary>
  /// 当前页签主表机种或物料去重
  /// </summary>
  /// <param name="statTab">页签</param>
  /// <param name="plantCode">工厂</param>
  /// <param name="keyword">关键字</param>
  /// <param name="model">true 机种，false 物料</param>
  /// <returns>下拉选项</returns>
  private async Task<List<TaktSelectOption>> DistinctHeaderCodesAsync(string? statTab, string? plantCode, string? keyword, bool model)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (plant.Length == 0 || !TryParseStatTab(statTab, out var scope))
    {
      return [];
    }
    var tenant = CurrentTenantCode;
    var company = CurrentCompanyCode;
    List<string> codes;
    if (scope.IsAssy && model)
    {
      codes = await _db.Queryable<TaktAssyOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode == plant && x.ModelCode != null && x.ModelCode != "")
          .GroupBy(x => x.ModelCode)
          .Select(x => x.ModelCode)
          .ToListAsync();
    }
    else if (scope.IsAssy)
    {
      codes = await _db.Queryable<TaktAssyOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode == plant && x.MaterialCode != null && x.MaterialCode != "")
          .GroupBy(x => x.MaterialCode)
          .Select(x => x.MaterialCode)
          .ToListAsync();
    }
    else if (model)
    {
      codes = await _db.Queryable<TaktPcbaOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode == plant && x.ModelCode != null && x.ModelCode != "")
          .GroupBy(x => x.ModelCode)
          .Select(x => x.ModelCode)
          .ToListAsync();
    }
    else
    {
      codes = await _db.Queryable<TaktPcbaOutput>()
          .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
              && x.PlantCode == plant && x.MaterialCode != null && x.MaterialCode != "")
          .GroupBy(x => x.MaterialCode)
          .Select(x => x.MaterialCode)
          .ToListAsync();
    }
    return ToCodeOptions(codes, keyword);
  }

  /// <summary>
  /// 编码去重为下拉
  /// </summary>
  /// <param name="codes">编码</param>
  /// <param name="keyword">关键字</param>
  /// <returns>下拉选项</returns>
  private static List<TaktSelectOption> ToCodeOptions(IEnumerable<string?> codes, string? keyword)
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
  /// 解析统计页签
  /// </summary>
  /// <param name="statTab">页签码</param>
  /// <param name="scope">解析结果</param>
  /// <returns>是否合法</returns>
  private static bool TryParseStatTab(string? statTab, out StatTabScope scope)
  {
    switch (statTab?.Trim().ToLowerInvariant())
    {
      case TabMfg1Team:
        scope = new StatTabScope(true, true);
        return true;
      case TabMfg1Model:
        scope = new StatTabScope(true, false);
        return true;
      case TabMfg2Team:
        scope = new StatTabScope(false, true);
        return true;
      case TabMfg2Model:
        scope = new StatTabScope(false, false);
        return true;
      default:
        scope = default;
        return false;
    }
  }

  /// <summary>
  /// 页签导出工作表名
  /// </summary>
  /// <param name="statTab">页签码</param>
  /// <returns>名称</returns>
  private static string ResolveStatTabTitle(string? statTab) =>
      statTab?.Trim().ToLowerInvariant() switch
      {
        TabMfg1Team => "组立按班组",
        TabMfg1Model => "组立按机种",
        TabMfg2Team => "PCBA按班组",
        TabMfg2Model => "PCBA按机种",
        _ => "生产统计",
      };

  /// <summary>
  /// 统计页签：数据源与分组
  /// </summary>
  /// <param name="IsAssy">组立为 true，PCBA 为 false</param>
  /// <param name="GroupByTeam">按班组为 true，按机种为 false</param>
  private readonly record struct StatTabScope(bool IsAssy, bool GroupByTeam);

  /// <summary>
  /// 按分组键与年月汇总的产量
  /// </summary>
  private sealed class StatMonthQty
  {
    /// <summary>
    /// 班组或机种
    /// </summary>
    public string GroupCode { get; set; } = string.Empty;

    /// <summary>
    /// 年
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// 月
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// 产量
    /// </summary>
    public decimal Qty { get; set; }
  }

  /// <summary>
  /// 月生产推移源行
  /// </summary>
  private sealed class ProductionMonthlyTrendSourceRow
  {
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; init; } = string.Empty;

    /// <summary>
    /// 班组（按班组页签）
    /// </summary>
    public string TeamCode { get; init; } = string.Empty;

    /// <summary>
    /// 机种（按机种页签）
    /// </summary>
    public string ModelCode { get; init; } = string.Empty;

    /// <summary>
    /// 产出类别
    /// </summary>
    public string OutputCategory { get; init; } = string.Empty;

    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; init; } = string.Empty;

    /// <summary>
    /// 产量
    /// </summary>
    public decimal Qty { get; init; }
  }

  /// <summary>
  /// 月生产推移行键
  /// </summary>
  /// <param name="PlantCode">工厂代码</param>
  /// <param name="TeamCode">班组</param>
  /// <param name="ModelCode">机种</param>
  private sealed record ProductionMonthlyTrendRowKey(string PlantCode, string TeamCode, string ModelCode);

  /// <summary>
  /// 月生产推移行键比较器
  /// </summary>
  private sealed class ProductionMonthlyTrendRowKeyComparer : IEqualityComparer<ProductionMonthlyTrendRowKey>
  {
    /// <summary>
    /// 单例
    /// </summary>
    public static ProductionMonthlyTrendRowKeyComparer Instance { get; } = new();

    /// <summary>
    /// 判断两行键是否相等（工厂/班组/机种，忽略大小写）
    /// </summary>
    /// <param name="x">左值</param>
    /// <param name="y">右值</param>
    /// <returns>是否相等</returns>
    public bool Equals(ProductionMonthlyTrendRowKey? x, ProductionMonthlyTrendRowKey? y)
    {
      if (x is null || y is null)
      {
        return ReferenceEquals(x, y);
      }
      return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.TeamCode, y.TeamCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.ModelCode, y.ModelCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 计算行键哈希（工厂/班组/机种大写）
    /// </summary>
    /// <param name="obj">行键</param>
    /// <returns>哈希码</returns>
    public int GetHashCode(ProductionMonthlyTrendRowKey obj) =>
        HashCode.Combine(
            obj.PlantCode.ToUpperInvariant(),
            obj.TeamCode.ToUpperInvariant(),
            obj.ModelCode.ToUpperInvariant());
  }

  /// <summary>
  /// 月生产推移分析构建结果
  /// </summary>
  private sealed class ProductionMonthlyTrendAnalysisBuilt
  {
    /// <summary>
    /// 排序后全量行
    /// </summary>
    public List<TaktProductionMonthlyTrendDto> OrderedRows { get; init; } = new();

    /// <summary>
    /// 期间列顺序
    /// </summary>
    public List<string> PeriodOrder { get; init; } = new();

    /// <summary>
    /// 环比基准期间
    /// </summary>
    public string? BasePeriod { get; init; }

    /// <summary>
    /// 环比对比期间
    /// </summary>
    public string? ComparePeriod { get; init; }

    /// <summary>
    /// 上涨行数
    /// </summary>
    public int UpCount { get; init; }

    /// <summary>
    /// 下跌行数
    /// </summary>
    public int DownCount { get; init; }

    /// <summary>
    /// 持平行数
    /// </summary>
    public int FlatCount { get; init; }

    /// <summary>
    /// 无法比较行数
    /// </summary>
    public int NoneCount { get; init; }

    /// <summary>
    /// 空结果
    /// </summary>
    /// <returns>空构建结果</returns>
    public static ProductionMonthlyTrendAnalysisBuilt Empty() => new();
  }
}

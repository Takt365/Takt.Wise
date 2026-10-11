// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityCostTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：质量成本月推移转置分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 质量成本月推移转置分析服务
/// </summary>
public class TaktQualityCostTrendService : TaktServiceBase, ITaktQualityCostTrendService
{
  private const string CategoryAssurance = "assurance";
  private const string CategoryIssue = "issue";
  private const string CategoryIncident = "incident";

  private readonly ITaktCompanyRepository<TaktQualityAssurance> _qualityAssuranceRepository;
  private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
  private readonly ITaktCompanyRepository<TaktQualityIncident> _qualityIncidentRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="qualityAssuranceRepository">品质保证仓储</param>
  /// <param name="qualityIssueRepository">品质问题仓储</param>
  /// <param name="qualityIncidentRepository">品质事故仓储</param>
  /// <param name="userContext">用户上下文</param>
  /// <param name="localizationService">本地化服务</param>
  public TaktQualityCostTrendService(
      ITaktCompanyRepository<TaktQualityAssurance> qualityAssuranceRepository,
      ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
      ITaktCompanyRepository<TaktQualityIncident> qualityIncidentRepository,
      ITaktUserContext? userContext = null,
      ITaktLocalizationService? localizationService = null)
      : base(userContext, localizationService)
  {
    _qualityAssuranceRepository = qualityAssuranceRepository;
    _qualityIssueRepository = qualityIssueRepository;
    _qualityIncidentRepository = qualityIncidentRepository;
  }

  /// <summary>
  /// 获取质量成本月推移转置分析（分页）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  public async Task<TaktQualityCostTrendResultDto> GetQualityCostMonthlyTrendAnalysisAsync(
      TaktQualityCostTrendQueryDto queryDto)
  {
    ArgumentNullException.ThrowIfNull(queryDto);
    var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
    var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
    var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
    var built = await BuildQualityCostMonthlyTrendAnalysisAsync(queryDto);
    var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
    return new TaktQualityCostTrendResultDto
    {
      Paged = TaktPagedResult<TaktQualityCostTrendDto>.Create(
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
  /// 导出质量成本月推移转置分析 Excel
  /// </summary>
  /// <param name="query">查询条件</param>
  /// <param name="sheetName">工作表名称</param>
  /// <param name="fileName">导出文件名</param>
  /// <returns>文件名与内容</returns>
  public async Task<(string fileName, byte[] fileContent)> ExportQualityCostMonthlyTrendAnalysisAsync(
      TaktQualityCostTrendQueryDto query,
      string? sheetName = null,
      string? fileName = null)
  {
    ArgumentNullException.ThrowIfNull(query);
    var built = await BuildQualityCostMonthlyTrendAnalysisAsync(query);
    var columnKeys = new List<string>
    {
      "plantCode", "costCategory", "costCategoryName", "currencyCode",
    };
    var columnLabels = new List<string>
    {
      "工厂代码", "成本类别", "类别名称", "币种",
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
        ["costCategory"] = row.CostCategory,
        ["costCategoryName"] = row.CostCategoryName,
        ["currencyCode"] = row.CurrencyCode,
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
        dict[$"period_{period}"] = row.PeriodAmounts.TryGetValue(period, out var amount)
            ? amount
            : null;
      }
      return (IReadOnlyDictionary<string, object?>)dict;
    }).ToList();
    return await TaktExcelHelper.ExportDictionaryRowsAsync(
        exportRows,
        columnKeys,
        columnLabels,
        sheetName ?? "质量成本推移表",
        fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "质量成本推移"));
  }

  /// <summary>
  /// 构建质量成本月推移转置分析全量结果
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>内存构建结果</returns>
  private async Task<QualityCostTrendAnalysisBuilt> BuildQualityCostMonthlyTrendAnalysisAsync(
      TaktQualityCostTrendQueryDto queryDto)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
    EnsureThreeLayerContext();
    var plantCode = queryDto.PlantCode.Trim();
    var currencyFilter = string.IsNullOrWhiteSpace(queryDto.CurrencyCode)
        ? null
        : queryDto.CurrencyCode.Trim();
    var categoryFilter = NormalizeCategoryFilter(queryDto.CostCategory);
    var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
    if (periodOrder.Count == 0)
    {
      return QualityCostTrendAnalysisBuilt.Empty();
    }
    var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
    var sourceRows = await LoadQualityCostSourceRowsAsync(
        plantCode,
        categoryFilter,
        currencyFilter,
        rangeStart,
        rangeEnd);
    if (sourceRows.Count == 0)
    {
      return QualityCostTrendAnalysisBuilt.Empty();
    }
    var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
    var allRows = sourceRows
        .GroupBy(
            r => new QualityCostTrendRowKey(r.PlantCode, r.CostCategory, r.CurrencyCode),
            QualityCostTrendRowKeyComparer.Instance)
        .Select(g => BuildQualityCostTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
        .ToList();
    var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
    var ordered = OrderTrendRows(filtered);
    return new QualityCostTrendAnalysisBuilt
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
  /// 加载三类质量成本源行
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="categoryFilter">成本类别筛选</param>
  /// <param name="currencyFilter">币种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>源行列表</returns>
  private async Task<List<QualityCostTrendSourceRow>> LoadQualityCostSourceRowsAsync(
      string plantCode,
      IReadOnlySet<string>? categoryFilter,
      string? currencyFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rows = new List<QualityCostTrendSourceRow>();
    if (ShouldIncludeCategory(categoryFilter, CategoryAssurance))
    {
      var assuranceRows = await _qualityAssuranceRepository.GetListAsync(
          BuildAssuranceExpression(plantCode, currencyFilter, rangeStart, rangeEnd));
      rows.AddRange(assuranceRows.Select(MapAssuranceSourceRow));
    }
    if (ShouldIncludeCategory(categoryFilter, CategoryIssue))
    {
      var issueRows = await _qualityIssueRepository.GetListAsync(
          BuildIssueExpression(plantCode, currencyFilter, rangeStart, rangeEnd));
      rows.AddRange(issueRows.Select(MapIssueSourceRow));
    }
    if (ShouldIncludeCategory(categoryFilter, CategoryIncident))
    {
      var incidentRows = await _qualityIncidentRepository.GetListAsync(
          BuildIncidentExpression(plantCode, currencyFilter, rangeStart, rangeEnd));
      rows.AddRange(incidentRows.Select(MapIncidentSourceRow));
    }
    return rows;
  }

  /// <summary>
  /// 是否包含指定成本类别
  /// </summary>
  /// <param name="categoryFilter">类别筛选集合</param>
  /// <param name="category">类别码</param>
  /// <returns>是否包含</returns>
  private static bool ShouldIncludeCategory(IReadOnlySet<string>? categoryFilter, string category) =>
      categoryFilter == null || categoryFilter.Contains(category);

  /// <summary>
  /// 归一化成本类别筛选
  /// </summary>
  /// <param name="costCategory">成本类别</param>
  /// <returns>类别集合；空表示全部</returns>
  private static HashSet<string>? NormalizeCategoryFilter(string? costCategory)
  {
    if (string.IsNullOrWhiteSpace(costCategory))
    {
      return null;
    }
    var normalized = costCategory.Trim().ToLowerInvariant();
    return normalized switch
    {
      CategoryAssurance => new HashSet<string>(StringComparer.Ordinal) { CategoryAssurance },
      CategoryIssue => new HashSet<string>(StringComparer.Ordinal) { CategoryIssue },
      CategoryIncident => new HashSet<string>(StringComparer.Ordinal) { CategoryIncident },
      _ => throw new ArgumentException($"不支持的成本类别：{costCategory}"),
    };
  }

  /// <summary>
  /// 构建品质保证查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="currencyFilter">币种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktQualityAssurance, bool>> BuildAssuranceExpression(
      string plantCode,
      string? currencyFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var startKey = rangeStart.ToString("yyyy-MM");
    var endKey = rangeEnd.ToString("yyyy-MM");
    var exp = Expressionable.Create<TaktQualityAssurance>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && x.PlantCode == plantCode
        && x.AssuranceMonth != null
        && x.AssuranceMonth.CompareTo(startKey) >= 0
        && x.AssuranceMonth.CompareTo(endKey) <= 0);
    if (!string.IsNullOrWhiteSpace(currencyFilter))
    {
      exp = exp.And(x => x.CurrencyCode == currencyFilter);
    }
    return exp.ToExpression();
  }

  /// <summary>
  /// 构建品质问题查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="currencyFilter">币种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktQualityIssue, bool>> BuildIssueExpression(
      string plantCode,
      string? currencyFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var exp = Expressionable.Create<TaktQualityIssue>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && x.PlantCode == plantCode
        && x.IssueDate >= rangeStart
        && x.IssueDate < rangeEndExclusive);
    if (!string.IsNullOrWhiteSpace(currencyFilter))
    {
      exp = exp.And(x => x.CurrencyCode == currencyFilter);
    }
    return exp.ToExpression();
  }

  /// <summary>
  /// 构建品质事故查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="currencyFilter">币种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktQualityIncident, bool>> BuildIncidentExpression(
      string plantCode,
      string? currencyFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var exp = Expressionable.Create<TaktQualityIncident>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && x.PlantCode == plantCode
        && x.IncidentDate >= rangeStart
        && x.IncidentDate < rangeEndExclusive);
    if (!string.IsNullOrWhiteSpace(currencyFilter))
    {
      exp = exp.And(x => x.CurrencyCode == currencyFilter);
    }
    return exp.ToExpression();
  }

  /// <summary>
  /// 映射品质保证源行
  /// </summary>
  /// <param name="entity">实体</param>
  /// <returns>源行</returns>
  private static QualityCostTrendSourceRow MapAssuranceSourceRow(TaktQualityAssurance entity) =>
      new()
      {
        PlantCode = entity.PlantCode.Trim(),
        CostCategory = CategoryAssurance,
        CurrencyCode = entity.CurrencyCode?.Trim() ?? string.Empty,
        Period = entity.AssuranceMonth.Trim(),
        Amount = entity.TotalQualityCost,
      };

  /// <summary>
  /// 映射品质问题源行
  /// </summary>
  /// <param name="entity">实体</param>
  /// <returns>源行</returns>
  private static QualityCostTrendSourceRow MapIssueSourceRow(TaktQualityIssue entity) =>
      new()
      {
        PlantCode = entity.PlantCode.Trim(),
        CostCategory = CategoryIssue,
        CurrencyCode = entity.CurrencyCode?.Trim() ?? string.Empty,
        Period = new DateTime(entity.IssueDate.Year, entity.IssueDate.Month, 1).ToString("yyyy-MM"),
        Amount = entity.TotalCost,
      };

  /// <summary>
  /// 映射品质事故源行
  /// </summary>
  /// <param name="entity">实体</param>
  /// <returns>源行</returns>
  private static QualityCostTrendSourceRow MapIncidentSourceRow(TaktQualityIncident entity) =>
      new()
      {
        PlantCode = entity.PlantCode.Trim(),
        CostCategory = CategoryIncident,
        CurrencyCode = entity.CurrencyCode?.Trim() ?? string.Empty,
        Period = new DateTime(entity.IncidentDate.Year, entity.IncidentDate.Month, 1).ToString("yyyy-MM"),
        Amount = entity.TotalScrapCost,
      };

  /// <summary>
  /// 解析期间列顺序
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <param name="rangeStart">区间起</param>
  /// <param name="rangeEnd">区间止</param>
  /// <returns>期间列顺序</returns>
  private static List<string> ResolvePeriodOrder(
      TaktQualityCostTrendQueryDto queryDto,
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
  /// 构建单行质量成本月推移
  /// </summary>
  /// <param name="key">行键</param>
  /// <param name="groupRows">同键源行</param>
  /// <param name="periodSet">展示期间集合</param>
  /// <param name="focusPeriod">关注期间</param>
  /// <returns>转置行</returns>
  private static TaktQualityCostTrendDto BuildQualityCostTrendRow(
      QualityCostTrendRowKey key,
      IReadOnlyList<QualityCostTrendSourceRow> groupRows,
      IReadOnlySet<string> periodSet,
      string? focusPeriod)
  {
    var row = new TaktQualityCostTrendDto
    {
      PlantCode = key.PlantCode,
      CostCategory = key.CostCategory,
      CostCategoryName = ResolveCategoryName(key.CostCategory),
      CurrencyCode = key.CurrencyCode,
      Trend = "none",
    };
    foreach (var period in groupRows
                 .Where(r => periodSet.Contains(r.Period))
                 .GroupBy(r => r.Period, StringComparer.Ordinal))
    {
      row.PeriodAmounts[period.Key] = RoundAmount(period.Sum(r => r.Amount));
    }
    ApplyFocusTrend(row, focusPeriod);
    return row;
  }

  /// <summary>
  /// 解析成本类别显示名
  /// </summary>
  /// <param name="category">类别码</param>
  /// <returns>显示名</returns>
  private static string? ResolveCategoryName(string category) =>
      category switch
      {
        CategoryAssurance => "品质保证",
        CategoryIssue => "品质问题",
        CategoryIncident => "品质事故",
        _ => null,
      };

  /// <summary>
  /// 按关注月计算环比涨跌
  /// </summary>
  /// <param name="row">转置行</param>
  /// <param name="focusPeriod">关注期间 yyyy-MM</param>
  private static void ApplyFocusTrend(TaktQualityCostTrendDto row, string? focusPeriod)
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
    if (!row.PeriodAmounts.TryGetValue(basePeriod, out var baseAmount)
        || !row.PeriodAmounts.TryGetValue(comparePeriod, out var compareAmount))
    {
      row.Trend = "none";
      return;
    }
    row.VarianceAmount = RoundAmount(compareAmount - baseAmount);
    if (baseAmount != 0m)
    {
      row.VariancePercent = Math.Round(
          row.VarianceAmount.Value / baseAmount,
          4,
          MidpointRounding.AwayFromZero);
    }
    if (compareAmount > baseAmount)
    {
      row.Trend = "up";
    }
    else if (compareAmount < baseAmount)
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
  private static List<TaktQualityCostTrendDto> FilterTrendRows(
      IReadOnlyList<TaktQualityCostTrendDto> rows,
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
  private static List<TaktQualityCostTrendDto> OrderTrendRows(
      IReadOnlyList<TaktQualityCostTrendDto> rows)
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
        .ThenBy(r => r.CostCategory, StringComparer.Ordinal)
        .ThenBy(r => r.CurrencyCode, StringComparer.Ordinal)
        .ToList();
  }

  /// <summary>
  /// 金额四舍五入至 2 位
  /// </summary>
  /// <param name="value">金额</param>
  /// <returns>四舍五入后金额</returns>
  private static decimal RoundAmount(decimal value) =>
      Math.Round(value, 2, MidpointRounding.AwayFromZero);

  /// <summary>
  /// 质量成本推移源行
  /// </summary>
  private sealed class QualityCostTrendSourceRow
  {
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; init; } = string.Empty;

    /// <summary>
    /// 成本类别
    /// </summary>
    public string CostCategory { get; init; } = string.Empty;

    /// <summary>
    /// 成本币种
    /// </summary>
    public string CurrencyCode { get; init; } = string.Empty;

    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; init; } = string.Empty;

    /// <summary>
    /// 金额
    /// </summary>
    public decimal Amount { get; init; }
  }

  /// <summary>
  /// 质量成本推移行键
  /// </summary>
  /// <param name="PlantCode">工厂代码</param>
  /// <param name="CostCategory">成本类别</param>
  /// <param name="CurrencyCode">成本币种</param>
  private sealed record QualityCostTrendRowKey(string PlantCode, string CostCategory, string CurrencyCode);

  /// <summary>
  /// 质量成本推移行键比较器
  /// </summary>
  private sealed class QualityCostTrendRowKeyComparer : IEqualityComparer<QualityCostTrendRowKey>
  {
    /// <summary>
    /// 单例
    /// </summary>
    public static QualityCostTrendRowKeyComparer Instance { get; } = new();

    /// <summary>
    /// 判断两行键是否相等（工厂/成本类别/币种，忽略大小写）
    /// </summary>
    /// <param name="x">左值</param>
    /// <param name="y">右值</param>
    /// <returns>是否相等</returns>
    public bool Equals(QualityCostTrendRowKey? x, QualityCostTrendRowKey? y)
    {
      if (x is null || y is null)
      {
        return ReferenceEquals(x, y);
      }
      return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.CostCategory, y.CostCategory, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.CurrencyCode, y.CurrencyCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 计算行键哈希（工厂/成本类别/币种大写）
    /// </summary>
    /// <param name="obj">行键</param>
    /// <returns>哈希码</returns>
    public int GetHashCode(QualityCostTrendRowKey obj) =>
        HashCode.Combine(
            obj.PlantCode.ToUpperInvariant(),
            obj.CostCategory.ToUpperInvariant(),
            obj.CurrencyCode.ToUpperInvariant());
  }

  /// <summary>
  /// 质量成本推移分析构建结果
  /// </summary>
  private sealed class QualityCostTrendAnalysisBuilt
  {
    /// <summary>
    /// 排序后全量行
    /// </summary>
    public List<TaktQualityCostTrendDto> OrderedRows { get; init; } = new();

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
    public static QualityCostTrendAnalysisBuilt Empty() => new();
  }
}

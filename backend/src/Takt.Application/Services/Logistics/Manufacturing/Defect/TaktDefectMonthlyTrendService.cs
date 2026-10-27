// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectMonthlyTrendService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：月生产不良推移转置分析服务实现
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 月生产不良推移转置分析服务
/// </summary>
public class TaktDefectMonthlyTrendService : TaktServiceBase, ITaktDefectMonthlyTrendService
{
  private const string CategoryAssy = "assy";
  private const string CategoryPcba = "pcba";

  private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
  private readonly ITaktCompanyRepository<TaktPcbaInspection> _pcbaInspectionRepository;
  private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="assyDefectRepository">组立不良仓储</param>
  /// <param name="pcbaInspectionRepository">PCBA检查仓储</param>
  /// <param name="pcbaInspectionDetailRepository">PCBA检查明细仓储</param>
  /// <param name="userContext">用户上下文</param>
  /// <param name="localizationService">本地化服务</param>
  public TaktDefectMonthlyTrendService(
      ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
      ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
      ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
      ITaktUserContext? userContext = null,
      ITaktLocalizationService? localizationService = null)
      : base(userContext, localizationService)
  {
    _assyDefectRepository = assyDefectRepository;
    _pcbaInspectionRepository = pcbaInspectionRepository;
    _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
  }

  /// <summary>
  /// 推移查询栏：组立不良 ∪ PCBA 检查工厂去重选项
  /// </summary>
  /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <param name="statTab">页签 assy 或 pcba；空则两类工厂都返回</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetDefectMonthlyTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null, string? statTab = null)
  {
    EnsureThreeLayerContext();
    var tab = statTab?.Trim().ToLowerInvariant() ?? string.Empty;
    var includeAssy = tab.Length == 0 || tab == CategoryAssy;
    var includePcba = tab.Length == 0 || tab == CategoryPcba;
    var codes = new List<string>();
    if (includeAssy)
    {
      var assyList = await _assyDefectRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode != null
              && x.PlantCode != string.Empty);
      codes.AddRange(assyList.Select(e => e.PlantCode.Trim()));
    }
    if (includePcba)
    {
      var pcbaList = await _pcbaInspectionRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode != null
              && x.PlantCode != string.Empty);
      codes.AddRange(pcbaList.Select(e => e.PlantCode.Trim()));
    }
    return codes
        .Where(c => !string.IsNullOrWhiteSpace(c))
        .GroupBy(c => c, StringComparer.OrdinalIgnoreCase)
        .OrderBy(g => g.Key, StringComparer.Ordinal)
        .Select(g => new TaktSelectOption
        {
          DictValue = g.Key,
          DictLabel = g.Key,
        })
        .ToList();
  }

  /// <summary>
  /// 推移查询栏：按工厂可用不良类别（assy / pcba；级联第 2 级）
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetDefectMonthlyTrendDefectCategoryOptionsAsync(string? plantCode = null, string? keyword = null)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(plant))
    {
      return new List<TaktSelectOption>();
    }
    var options = new List<TaktSelectOption>();
    var assyHit = await _assyDefectRepository.FirstAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant);
    if (assyHit != null)
    {
      options.Add(new TaktSelectOption
      {
        DictValue = CategoryAssy,
        DictLabel = ResolveCategoryName(CategoryAssy) ?? CategoryAssy,
      });
    }
    var pcbaHit = await _pcbaInspectionRepository.FirstAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant);
    if (pcbaHit != null)
    {
      options.Add(new TaktSelectOption
      {
        DictValue = CategoryPcba,
        DictLabel = ResolveCategoryName(CategoryPcba) ?? CategoryPcba,
      });
    }
    return options;
  }

  /// <summary>
  /// 推移查询栏：按工厂（及可选不良类别）去重机种（级联第 3 级，查询时可空）
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="defectCategory">不良类别（assy / pcba；空则两表并集）</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetDefectMonthlyTrendModelOptionsAsync(string? plantCode = null, string? keyword = null, string? defectCategory = null)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(plant))
    {
      return new List<TaktSelectOption>();
    }
    var categoryFilter = string.IsNullOrWhiteSpace(defectCategory)
        ? null
        : NormalizeCategoryFilter(defectCategory);
    var models = new List<string>();
    if (ShouldIncludeCategory(categoryFilter, CategoryAssy))
    {
      var assyList = await _assyDefectRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode == plant
              && x.ModelCode != null
              && x.ModelCode != string.Empty);
      models.AddRange(assyList.Select(e => e.ModelCode.Trim()));
    }
    if (ShouldIncludeCategory(categoryFilter, CategoryPcba))
    {
      var pcbaList = await _pcbaInspectionRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode == plant
              && x.ModelCode != null
              && x.ModelCode != string.Empty);
      models.AddRange(pcbaList.Select(e => e.ModelCode.Trim()));
    }
    return models
        .Where(c => !string.IsNullOrWhiteSpace(c))
        .GroupBy(c => c, StringComparer.OrdinalIgnoreCase)
        .OrderBy(g => g.Key, StringComparer.Ordinal)
        .Select(g => new TaktSelectOption
        {
          DictValue = g.Key,
          DictLabel = g.Key,
        })
        .ToList();
  }

  /// <summary>
  /// 获取月生产不良推移转置分析（分页）
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <returns>转置分析结果</returns>
  public async Task<TaktDefectMonthlyTrendResultDto> GetDefectMonthlyTrendAnalysisAsync(
      TaktDefectMonthlyTrendQueryDto queryDto)
  {
    ArgumentNullException.ThrowIfNull(queryDto);
    var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
    var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
    var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
    var built = await BuildDefectMonthlyTrendAnalysisAsync(queryDto);
    var pageRows = built.OrderedRows.Skip(skip).Take(pageSize).ToList();
    return new TaktDefectMonthlyTrendResultDto
    {
      Paged = TaktPagedResult<TaktDefectMonthlyTrendDto>.Create(
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
  /// ExportDefectMonthlyTrendAnalysisAsync
  /// </summary>
  public async Task<(string fileName, byte[] fileContent)> ExportDefectMonthlyTrendAnalysisAsync(
      TaktDefectMonthlyTrendQueryDto query,
      string? sheetName = null,
      string? fileName = null)
  {
    ArgumentNullException.ThrowIfNull(query);
    var built = await BuildDefectMonthlyTrendAnalysisAsync(query);
    var columnKeys = new List<string>
    {
      "plantCode", "modelCode", "defectCategory", "defectCategoryName",
    };
    var columnLabels = new List<string>
    {
      "工厂代码", "机种", "不良类别", "类别名称",
    };
    foreach (var period in built.PeriodOrder)
    {
      columnKeys.Add($"period_{period}");
      columnLabels.Add($"{period}不良率%");
    }
    columnKeys.AddRange(new[] { "basePeriod", "comparePeriod", "varianceAmount", "variancePercent", "trend" });
    columnLabels.AddRange(new[] { "基准月", "对比月", "环比率差", "环比%", "涨跌" });
    var exportRows = built.OrderedRows.Select(row =>
    {
      var dict = new Dictionary<string, object?>(StringComparer.Ordinal)
      {
        ["plantCode"] = row.PlantCode,
        ["modelCode"] = row.ModelCode,
        ["defectCategory"] = row.DefectCategory,
        ["defectCategoryName"] = row.DefectCategoryName,
        ["basePeriod"] = row.BasePeriod,
        ["comparePeriod"] = row.ComparePeriod,
        ["varianceAmount"] = row.VarianceAmount.HasValue
            ? Math.Round(row.VarianceAmount.Value * 100m, 4, MidpointRounding.AwayFromZero)
            : null,
        ["variancePercent"] = row.VariancePercent.HasValue
            ? Math.Round(row.VariancePercent.Value, 4, MidpointRounding.AwayFromZero)
            : null,
        ["trend"] = row.Trend,
      };
      foreach (var period in built.PeriodOrder)
      {
        dict[$"period_{period}"] = row.PeriodValues.TryGetValue(period, out var rate)
            ? Math.Round(rate * 100m, 4, MidpointRounding.AwayFromZero)
            : null;
      }
      return (IReadOnlyDictionary<string, object?>)dict;
    }).ToList();
    return await TaktExcelHelper.ExportDictionaryRowsAsync(
        exportRows,
        columnKeys,
        columnLabels,
        sheetName ?? "月生产不良推移表",
        fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "月生产不良推移"));
  }

  /// <summary>
  /// 构建月生产不良推移转置分析全量结果
  /// </summary>
  /// <param name="queryDto">查询条件</param>
  /// <returns>内存构建结果</returns>
  private async Task<DefectMonthlyTrendAnalysisBuilt> BuildDefectMonthlyTrendAnalysisAsync(
      TaktDefectMonthlyTrendQueryDto queryDto)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
    EnsureThreeLayerContext();
    var plantCode = queryDto.PlantCode.Trim();
    var modelFilter = string.IsNullOrWhiteSpace(queryDto.ModelCode)
        ? null
        : queryDto.ModelCode.Trim();
    var categoryFilter = NormalizeCategoryFilter(queryDto.DefectCategory);
    var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
    if (periodOrder.Count == 0)
    {
      return DefectMonthlyTrendAnalysisBuilt.Empty();
    }
    var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var sourceRows = await LoadDefectSourceRowsAsync(
        plantCode,
        categoryFilter,
        modelFilter,
        rangeStart,
        rangeEndExclusive);
    if (sourceRows.Count == 0)
    {
      return DefectMonthlyTrendAnalysisBuilt.Empty();
    }
    var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
    var allRows = sourceRows
        .GroupBy(
            r => new DefectMonthlyTrendRowKey(r.PlantCode, r.ModelCode, r.DefectCategory),
            DefectMonthlyTrendRowKeyComparer.Instance)
        .Select(g => BuildDefectMonthlyTrendRow(g.Key, g.ToList(), periodSet, focusPeriod))
        .ToList();
    var filtered = FilterTrendRows(allRows, queryDto.TrendFilter);
    var ordered = OrderTrendRows(filtered);
    return new DefectMonthlyTrendAnalysisBuilt
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
  /// 加载组立与 PCBA 不良源行
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="categoryFilter">不良类别筛选</param>
  /// <param name="modelFilter">机种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEndExclusive">期间止（不含）</param>
  /// <returns>源行列表</returns>
  private async Task<List<DefectMonthlyTrendSourceRow>> LoadDefectSourceRowsAsync(
      string plantCode,
      IReadOnlySet<string>? categoryFilter,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEndExclusive)
  {
    var rows = new List<DefectMonthlyTrendSourceRow>();
    if (ShouldIncludeCategory(categoryFilter, CategoryAssy))
    {
      var assyRows = await _assyDefectRepository.GetListAsync(
          BuildAssyExpression(plantCode, modelFilter, rangeStart, rangeEndExclusive));
      rows.AddRange(assyRows.Select(MapAssySourceRow));
    }
    if (ShouldIncludeCategory(categoryFilter, CategoryPcba))
    {
      rows.AddRange(await LoadPcbaSourceRowsAsync(
          plantCode, modelFilter, rangeStart, rangeEndExclusive));
    }
    return rows;
  }

  /// <summary>
  /// 加载 PCBA 检查明细源行（主表机种/工厂 + 明细数量）
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEndExclusive">期间止（不含）</param>
  /// <returns>源行列表</returns>
  private async Task<List<DefectMonthlyTrendSourceRow>> LoadPcbaSourceRowsAsync(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEndExclusive)
  {
    var inspections = await _pcbaInspectionRepository.GetListAsync(
        BuildPcbaInspectionExpression(plantCode, modelFilter));
    if (inspections.Count == 0)
    {
      return new List<DefectMonthlyTrendSourceRow>();
    }
    var inspectionMap = inspections.ToDictionary(
        x => x.Id,
        x => new { x.PlantCode, x.ModelCode });
    var inspectionIds = inspectionMap.Keys.ToList();
    var details = await _pcbaInspectionDetailRepository.GetListAsync(
        d => d.TenantCode == CurrentTenantCode
            && d.CompanyCode == CurrentCompanyCode
            && inspectionIds.Contains(d.PcbaInspectionId)
            && d.IsObsolete == 0);
    var rows = new List<DefectMonthlyTrendSourceRow>();
    foreach (var detail in details)
    {
      if (!inspectionMap.TryGetValue(detail.PcbaInspectionId, out var master))
      {
        continue;
      }
      var assemblyDate = detail.BSideAssemblyDate ?? detail.TSideAssemblyDate;
      if (!assemblyDate.HasValue)
      {
        continue;
      }
      if (assemblyDate.Value < rangeStart || assemblyDate.Value >= rangeEndExclusive)
      {
        continue;
      }
      rows.Add(new DefectMonthlyTrendSourceRow
      {
        PlantCode = master.PlantCode.Trim(),
        ModelCode = master.ModelCode.Trim(),
        DefectCategory = CategoryPcba,
        Period = new DateTime(assemblyDate.Value.Year, assemblyDate.Value.Month, 1).ToString("yyyy-MM"),
        ActualQty = detail.InspectionQty,
        DefectQty = detail.DefectQty,
      });
    }
    return rows;
  }

  /// <summary>
  /// 是否包含指定不良类别
  /// </summary>
  /// <param name="categoryFilter">类别筛选集合</param>
  /// <param name="category">类别码</param>
  /// <returns>是否包含</returns>
  private static bool ShouldIncludeCategory(IReadOnlySet<string>? categoryFilter, string category) =>
      categoryFilter == null || categoryFilter.Contains(category);

  /// <summary>
  /// 归一化不良类别筛选
  /// </summary>
  /// <param name="defectCategory">不良类别</param>
  /// <returns>类别集合；空表示全部</returns>
  private static HashSet<string>? NormalizeCategoryFilter(string? defectCategory)
  {
    if (string.IsNullOrWhiteSpace(defectCategory))
    {
      return null;
    }
    var normalized = defectCategory.Trim().ToLowerInvariant();
    return normalized switch
    {
      CategoryAssy => new HashSet<string>(StringComparer.Ordinal) { CategoryAssy },
      CategoryPcba => new HashSet<string>(StringComparer.Ordinal) { CategoryPcba },
      _ => throw new ArgumentException($"不支持的不良类别：{defectCategory}"),
    };
  }

  /// <summary>
  /// 构建组立不良查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEndExclusive">期间止（不含）</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktAssyDefect, bool>> BuildAssyExpression(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEndExclusive)
  {
    var exp = Expressionable.Create<TaktAssyDefect>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && x.PlantCode == plantCode
        && x.ProdDate >= rangeStart
        && x.ProdDate < rangeEndExclusive);
    if (!string.IsNullOrWhiteSpace(modelFilter))
    {
      exp = exp.And(x => x.ModelCode == modelFilter);
    }
    return exp.ToExpression();
  }

  /// <summary>
  /// 构建 PCBA 检查主表查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktPcbaInspection, bool>> BuildPcbaInspectionExpression(
      string plantCode,
      string? modelFilter)
  {
    var exp = Expressionable.Create<TaktPcbaInspection>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && x.PlantCode == plantCode);
    if (!string.IsNullOrWhiteSpace(modelFilter))
    {
      exp = exp.And(x => x.ModelCode == modelFilter);
    }
    return exp.ToExpression();
  }

  /// <summary>
  /// 映射组立不良源行
  /// </summary>
  /// <param name="entity">实体</param>
  /// <returns>源行</returns>
  private static DefectMonthlyTrendSourceRow MapAssySourceRow(TaktAssyDefect entity)
  {
    var actual = entity.ProdActualQty;
    var defect = actual > 0m ? actual - entity.GoodQuantity : 0m;
    return new DefectMonthlyTrendSourceRow
    {
      PlantCode = entity.PlantCode.Trim(),
      ModelCode = entity.ModelCode.Trim(),
      DefectCategory = CategoryAssy,
      Period = new DateTime(entity.ProdDate.Year, entity.ProdDate.Month, 1).ToString("yyyy-MM"),
      ActualQty = actual,
      DefectQty = defect,
    };
  }

  /// <summary>
  /// 解析期间列顺序
  /// </summary>
  /// <param name="queryDto">查询 DTO</param>
  /// <param name="rangeStart">区间起</param>
  /// <param name="rangeEnd">区间止</param>
  /// <returns>期间列顺序</returns>
  private static List<string> ResolvePeriodOrder(
      TaktDefectMonthlyTrendQueryDto queryDto,
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
  /// 构建单行月生产不良推移
  /// </summary>
  /// <param name="key">行键</param>
  /// <param name="groupRows">同键源行</param>
  /// <param name="periodSet">展示期间集合</param>
  /// <param name="focusPeriod">关注期间</param>
  /// <returns>转置行</returns>
  private static TaktDefectMonthlyTrendDto BuildDefectMonthlyTrendRow(
      DefectMonthlyTrendRowKey key,
      IReadOnlyList<DefectMonthlyTrendSourceRow> groupRows,
      IReadOnlySet<string> periodSet,
      string? focusPeriod)
  {
    var row = new TaktDefectMonthlyTrendDto
    {
      PlantCode = key.PlantCode,
      ModelCode = key.ModelCode,
      DefectCategory = key.DefectCategory,
      DefectCategoryName = ResolveCategoryName(key.DefectCategory),
      Trend = "none",
    };
    foreach (var period in groupRows
                 .Where(r => periodSet.Contains(r.Period))
                 .GroupBy(r => r.Period, StringComparer.Ordinal))
    {
      var actualTotal = period.Sum(r => r.ActualQty);
      var defectTotal = period.Sum(r => r.DefectQty);
      row.PeriodActualQuantities[period.Key] = RoundQty(actualTotal);
      row.PeriodDefectQuantities[period.Key] = RoundQty(defectTotal);
      if (actualTotal > 0m)
      {
        row.PeriodValues[period.Key] = RoundRate(defectTotal / actualTotal);
      }
    }
    ApplyFocusTrend(row, focusPeriod);
    return row;
  }

  /// <summary>
  /// 解析不良类别显示名
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
  /// 按关注月计算环比涨跌（不良率）
  /// </summary>
  /// <param name="row">转置行</param>
  /// <param name="focusPeriod">关注期间 yyyy-MM</param>
  private static void ApplyFocusTrend(TaktDefectMonthlyTrendDto row, string? focusPeriod)
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
    if (!row.PeriodValues.TryGetValue(basePeriod, out var baseRate)
        || !row.PeriodValues.TryGetValue(comparePeriod, out var compareRate))
    {
      row.Trend = "none";
      return;
    }
    row.VarianceAmount = RoundRate(compareRate - baseRate);
    if (baseRate != 0m)
    {
      row.VariancePercent = Math.Round(
          row.VarianceAmount.Value / baseRate,
          4,
          MidpointRounding.AwayFromZero);
    }
    if (compareRate > baseRate)
    {
      row.Trend = "up";
    }
    else if (compareRate < baseRate)
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
  private static List<TaktDefectMonthlyTrendDto> FilterTrendRows(
      IReadOnlyList<TaktDefectMonthlyTrendDto> rows,
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
  private static List<TaktDefectMonthlyTrendDto> OrderTrendRows(
      IReadOnlyList<TaktDefectMonthlyTrendDto> rows)
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
        .ThenBy(r => r.DefectCategory, StringComparer.Ordinal)
        .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
        .ToList();
  }

  /// <summary>
  /// 不良率四舍五入至 4 位
  /// </summary>
  /// <param name="value">比率</param>
  /// <returns>四舍五入后比率</returns>
  private static decimal RoundRate(decimal value) =>
      Math.Round(value, 4, MidpointRounding.AwayFromZero);

  /// <summary>
  /// 数量四舍五入至 3 位
  /// </summary>
  /// <param name="value">数量</param>
  /// <returns>四舍五入后数量</returns>
  private static decimal RoundQty(decimal value) =>
      Math.Round(value, 3, MidpointRounding.AwayFromZero);

  /// <summary>
  /// 月生产不良推移源行
  /// </summary>
  private sealed class DefectMonthlyTrendSourceRow
  {
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; init; } = string.Empty;

    /// <summary>
    /// 机种编码
    /// </summary>
    public string ModelCode { get; init; } = string.Empty;

    /// <summary>
    /// 不良类别
    /// </summary>
    public string DefectCategory { get; init; } = string.Empty;

    /// <summary>
    /// 期间 yyyy-MM
    /// </summary>
    public string Period { get; init; } = string.Empty;

    /// <summary>
    /// 生实/检查数量
    /// </summary>
    public decimal ActualQty { get; init; }

    /// <summary>
    /// 不良数量
    /// </summary>
    public decimal DefectQty { get; init; }
  }

  /// <summary>
  /// 月生产不良推移行键
  /// </summary>
  /// <param name="PlantCode">工厂代码</param>
  /// <param name="ModelCode">机种编码</param>
  /// <param name="DefectCategory">不良类别</param>
  private sealed record DefectMonthlyTrendRowKey(string PlantCode, string ModelCode, string DefectCategory);

  /// <summary>
  /// 月生产不良推移行键比较器
  /// </summary>
  private sealed class DefectMonthlyTrendRowKeyComparer : IEqualityComparer<DefectMonthlyTrendRowKey>
  {
    /// <summary>
    /// 单例
    /// </summary>
    public static DefectMonthlyTrendRowKeyComparer Instance { get; } = new();

    /// <summary>
    /// 月生产推移行键比较器
    /// </summary>
    /// <summary>
    /// 单例
    /// </summary>
    /// <summary>
    /// 判断两行键是否相等（工厂/机种/产出类别，忽略大小写）
    /// </summary>
    /// <param name="x">左值</param>
    /// <param name="y">右值</param>
    /// <returns>是否相等</returns>
    public bool Equals(DefectMonthlyTrendRowKey? x, DefectMonthlyTrendRowKey? y)
    {
      if (x is null || y is null)
      {
        return ReferenceEquals(x, y);
      }
      return string.Equals(x.PlantCode, y.PlantCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.ModelCode, y.ModelCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.DefectCategory, y.DefectCategory, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 计算行键哈希（工厂/机种/产出类别大写）
    /// </summary>
    /// <param name="obj">行键</param>
    /// <returns>哈希码</returns>
    public int GetHashCode(DefectMonthlyTrendRowKey obj) =>
        HashCode.Combine(
            obj.PlantCode.ToUpperInvariant(),
            obj.ModelCode.ToUpperInvariant(),
            obj.DefectCategory.ToUpperInvariant());
  }

  /// <summary>
  /// 月生产不良推移分析构建结果
  /// </summary>
  private sealed class DefectMonthlyTrendAnalysisBuilt
  {
    /// <summary>
    /// 排序后全量行
    /// </summary>
    public List<TaktDefectMonthlyTrendDto> OrderedRows { get; init; } = new();

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
    public static DefectMonthlyTrendAnalysisBuilt Empty() => new();
  }
}

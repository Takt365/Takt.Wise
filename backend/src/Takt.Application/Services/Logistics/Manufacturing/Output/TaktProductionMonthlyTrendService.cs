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

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 月生产推移转置分析服务（读组立/PCBA 产出本表；与 CRUD 服务分离）
/// </summary>
public class TaktProductionMonthlyTrendService : TaktServiceBase, ITaktProductionMonthlyTrendService
{
  private const string CategoryAssy = "assy";
  private const string CategoryPcba = "pcba";

  private readonly ITaktCompanyRepository<TaktAssyOutput> _assyOutputRepository;
  private readonly ITaktCompanyRepository<TaktAssyOutputDetail> _assyOutputDetailRepository;
  private readonly ITaktCompanyRepository<TaktPcbaOutput> _pcbaOutputRepository;
  private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="assyOutputRepository">组立日报主表仓储</param>
  /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
  /// <param name="pcbaOutputRepository">PCBA日报主表仓储</param>
  /// <param name="pcbaOutputDetailRepository">PCBA日报明细仓储</param>
  /// <param name="userContext">用户上下文</param>
  /// <param name="localizationService">本地化服务</param>
  public TaktProductionMonthlyTrendService(
      ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
      ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
      ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
      ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
      ITaktUserContext? userContext = null,
      ITaktLocalizationService? localizationService = null)
      : base(userContext, localizationService)
  {
    _assyOutputRepository = assyOutputRepository;
    _assyOutputDetailRepository = assyOutputDetailRepository;
    _pcbaOutputRepository = pcbaOutputRepository;
    _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
  }

  /// <summary>
  /// 推移查询栏：组立/PCBA 产出本表工厂去重选项（并集）
  /// </summary>
  /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetProductionMonthlyTrendPlantOptionsAsync(string? plantCode = null, string? keyword = null)
  {
    EnsureThreeLayerContext();
    var assyList = await _assyOutputRepository.GetListAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode != null
            && x.PlantCode != string.Empty);
    var pcbaList = await _pcbaOutputRepository.GetListAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode != null
            && x.PlantCode != string.Empty);
    return assyList.Select(e => e.PlantCode.Trim())
        .Concat(pcbaList.Select(e => e.PlantCode.Trim()))
        .Where(c => !string.IsNullOrEmpty(c))
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
  /// 推移查询栏：按工厂返回有数据的产出类别（assy / pcba）
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetProductionMonthlyTrendOutputCategoryOptionsAsync(string? plantCode = null, string? keyword = null)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(plant))
    {
      return new List<TaktSelectOption>();
    }
    var options = new List<TaktSelectOption>();
    var assyCount = await _assyOutputRepository.CountAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant);
    if (assyCount > 0)
    {
      options.Add(new TaktSelectOption
      {
        DictValue = CategoryAssy,
        DictLabel = "组立",
      });
    }
    var pcbaCount = await _pcbaOutputRepository.CountAsync(
        x => x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant);
    if (pcbaCount > 0)
    {
      options.Add(new TaktSelectOption
      {
        DictValue = CategoryPcba,
        DictLabel = "PCBA",
      });
    }
    return options;
  }

  /// <summary>
  /// 推移查询栏：按工厂（及可选产出类别）去重机种
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="outputCategory">产出类别（assy/pcba；空则并集）</param>
  /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
  /// <returns>下拉选项</returns>
  public async Task<List<TaktSelectOption>> GetProductionMonthlyTrendModelOptionsAsync(string? plantCode = null, string? keyword = null, string? outputCategory = null)
  {
    EnsureThreeLayerContext();
    var plant = plantCode?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(plant))
    {
      return new List<TaktSelectOption>();
    }
    var category = outputCategory?.Trim().ToLowerInvariant() ?? string.Empty;
    var modelCodes = new List<string>();
    var includeAssy = string.IsNullOrEmpty(category) || category == CategoryAssy;
    var includePcba = string.IsNullOrEmpty(category) || category == CategoryPcba;
    if (includeAssy)
    {
      var assyList = await _assyOutputRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode == plant
              && x.ModelCode != null
              && x.ModelCode != string.Empty);
      modelCodes.AddRange(assyList.Select(e => e.ModelCode.Trim()));
    }
    if (includePcba)
    {
      var pcbaList = await _pcbaOutputRepository.GetListAsync(
          x => x.TenantCode == CurrentTenantCode
              && x.CompanyCode == CurrentCompanyCode
              && x.PlantCode == plant
              && x.ModelCode != null
              && x.ModelCode != string.Empty);
      modelCodes.AddRange(pcbaList.Select(e => e.ModelCode.Trim()));
    }
    return modelCodes
        .Where(c => !string.IsNullOrEmpty(c))
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
    var columnKeys = new List<string>
    {
      "plantCode", "modelCode", "outputCategory", "outputCategoryName",
    };
    var columnLabels = new List<string>
    {
      "工厂代码", "机种", "产出类别", "类别名称",
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
        ["modelCode"] = row.ModelCode,
        ["outputCategory"] = row.OutputCategory,
        ["outputCategoryName"] = row.OutputCategoryName,
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
        sheetName ?? "月生产推移表",
        fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "月生产推移"));
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
    EnsureThreeLayerContext();
    var plantCode = queryDto.PlantCode.Trim();
    var modelFilter = string.IsNullOrWhiteSpace(queryDto.ModelCode)
        ? null
        : queryDto.ModelCode.Trim();
    var categoryFilter = NormalizeCategoryFilter(queryDto.OutputCategory);
    var periodOrder = ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
    if (periodOrder.Count == 0)
    {
      return ProductionMonthlyTrendAnalysisBuilt.Empty();
    }
    var focusPeriod = ResolveFocusPeriod(queryDto.FocusPeriod, periodOrder);
    var sourceRows = await LoadProductionSourceRowsAsync(
        plantCode,
        categoryFilter,
        modelFilter,
        rangeStart,
        rangeEnd);
    if (sourceRows.Count == 0)
    {
      return ProductionMonthlyTrendAnalysisBuilt.Empty();
    }
    var periodSet = new HashSet<string>(periodOrder, StringComparer.Ordinal);
    var allRows = sourceRows
        .GroupBy(
            r => new ProductionMonthlyTrendRowKey(r.PlantCode, r.ModelCode, r.OutputCategory),
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
  /// 加载组立与 PCBA 产出源行
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="categoryFilter">产出类别筛选</param>
  /// <param name="modelFilter">机种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>源行列表</returns>
  private async Task<List<ProductionMonthlyTrendSourceRow>> LoadProductionSourceRowsAsync(
      string plantCode,
      IReadOnlySet<string>? categoryFilter,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rows = new List<ProductionMonthlyTrendSourceRow>();
    if (ShouldIncludeCategory(categoryFilter, CategoryAssy))
    {
      rows.AddRange(await LoadAssySourceRowsAsync(plantCode, modelFilter, rangeStart, rangeEnd));
    }
    if (ShouldIncludeCategory(categoryFilter, CategoryPcba))
    {
      rows.AddRange(await LoadPcbaSourceRowsAsync(plantCode, modelFilter, rangeStart, rangeEnd));
    }
    return rows;
  }

  /// <summary>
  /// 加载组立产出源行
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>源行列表</returns>
  private async Task<List<ProductionMonthlyTrendSourceRow>> LoadAssySourceRowsAsync(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var masters = await _assyOutputRepository.GetListAsync(
        BuildAssyOutputExpression(plantCode, modelFilter, rangeStart, rangeEnd));
    if (masters.Count == 0)
    {
      return new List<ProductionMonthlyTrendSourceRow>();
    }
    var masterMap = masters.ToDictionary(m => m.Id);
    var masterIds = masterMap.Keys.ToList();
    var details = await _assyOutputDetailRepository.GetListAsync(
        BuildAssyDetailExpression(masterIds));
    var rows = new List<ProductionMonthlyTrendSourceRow>(details.Count);
    foreach (var detail in details)
    {
      if (!masterMap.TryGetValue(detail.AssyOutputId, out var master))
      {
        continue;
      }
      rows.Add(new ProductionMonthlyTrendSourceRow
      {
        PlantCode = master.PlantCode.Trim(),
        ModelCode = master.ModelCode.Trim(),
        OutputCategory = CategoryAssy,
        Period = new DateTime(master.ProdDate.Year, master.ProdDate.Month, 1).ToString("yyyy-MM"),
        Qty = detail.ProdActualQty,
      });
    }
    return rows;
  }

  /// <summary>
  /// 加载 PCBA 产出源行
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种筛选</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>源行列表</returns>
  private async Task<List<ProductionMonthlyTrendSourceRow>> LoadPcbaSourceRowsAsync(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var masters = await _pcbaOutputRepository.GetListAsync(
        BuildPcbaOutputExpression(plantCode, modelFilter, rangeStart, rangeEnd));
    if (masters.Count == 0)
    {
      return new List<ProductionMonthlyTrendSourceRow>();
    }
    var masterMap = masters.ToDictionary(m => m.Id);
    var masterIds = masterMap.Keys.ToList();
    var details = await _pcbaOutputDetailRepository.GetListAsync(
        BuildPcbaDetailExpression(masterIds));
    var rows = new List<ProductionMonthlyTrendSourceRow>(details.Count);
    foreach (var detail in details)
    {
      if (!masterMap.TryGetValue(detail.PcbaOutputId, out var master))
      {
        continue;
      }
      rows.Add(new ProductionMonthlyTrendSourceRow
      {
        PlantCode = master.PlantCode.Trim(),
        ModelCode = master.ModelCode.Trim(),
        OutputCategory = CategoryPcba,
        Period = new DateTime(master.ProdDate.Year, master.ProdDate.Month, 1).ToString("yyyy-MM"),
        Qty = detail.DailyCompletedQty,
      });
    }
    return rows;
  }

  /// <summary>
  /// 是否包含指定产出类别
  /// </summary>
  /// <param name="categoryFilter">类别筛选集合</param>
  /// <param name="category">类别码</param>
  /// <returns>是否包含</returns>
  private static bool ShouldIncludeCategory(IReadOnlySet<string>? categoryFilter, string category) =>
      categoryFilter == null || categoryFilter.Contains(category);

  /// <summary>
  /// 归一化产出类别筛选
  /// </summary>
  /// <param name="outputCategory">产出类别</param>
  /// <returns>类别集合；空表示全部</returns>
  private static HashSet<string>? NormalizeCategoryFilter(string? outputCategory)
  {
    if (string.IsNullOrWhiteSpace(outputCategory))
    {
      return null;
    }
    var normalized = outputCategory.Trim().ToLowerInvariant();
    return normalized switch
    {
      CategoryAssy => new HashSet<string>(StringComparer.Ordinal) { CategoryAssy },
      CategoryPcba => new HashSet<string>(StringComparer.Ordinal) { CategoryPcba },
      _ => throw new ArgumentException($"不支持的产出类别：{outputCategory}"),
    };
  }

  /// <summary>
  /// 构建组立日报主表查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktAssyOutput, bool>> BuildAssyOutputExpression(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var exp = Expressionable.Create<TaktAssyOutput>();
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
  /// 构建组立日报明细查询条件
  /// </summary>
  /// <param name="masterIds">主表主键集合</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktAssyOutputDetail, bool>> BuildAssyDetailExpression(IReadOnlyList<long> masterIds)
  {
    var exp = Expressionable.Create<TaktAssyOutputDetail>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && masterIds.Contains(x.AssyOutputId)
        && x.IsObsolete == 0);
    return exp.ToExpression();
  }

  /// <summary>
  /// 构建 PCBA 日报主表查询条件
  /// </summary>
  /// <param name="plantCode">工厂代码</param>
  /// <param name="modelFilter">机种</param>
  /// <param name="rangeStart">期间起</param>
  /// <param name="rangeEnd">期间止</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktPcbaOutput, bool>> BuildPcbaOutputExpression(
      string plantCode,
      string? modelFilter,
      DateTime rangeStart,
      DateTime rangeEnd)
  {
    var rangeEndExclusive = rangeEnd.AddMonths(1);
    var exp = Expressionable.Create<TaktPcbaOutput>();
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
  /// 构建 PCBA 日报明细查询条件
  /// </summary>
  /// <param name="masterIds">主表主键集合</param>
  /// <returns>表达式</returns>
  private Expression<Func<TaktPcbaOutputDetail, bool>> BuildPcbaDetailExpression(IReadOnlyList<long> masterIds)
  {
    var exp = Expressionable.Create<TaktPcbaOutputDetail>();
    exp = exp.And(x =>
        x.TenantCode == CurrentTenantCode
        && x.CompanyCode == CurrentCompanyCode
        && masterIds.Contains(x.PcbaOutputId)
        && x.IsObsolete == 0);
    return exp.ToExpression();
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
      ModelCode = key.ModelCode,
      OutputCategory = key.OutputCategory,
      OutputCategoryName = ResolveCategoryName(key.OutputCategory),
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
        .ThenBy(r => r.ModelCode, StringComparer.Ordinal)
        .ThenBy(r => r.OutputCategory, StringComparer.Ordinal)
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
  /// 月生产推移源行
  /// </summary>
  private sealed class ProductionMonthlyTrendSourceRow
  {
    /// <summary>
    /// 工厂代码
    /// </summary>
    public string PlantCode { get; init; } = string.Empty;

    /// <summary>
    /// 机种
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
  /// <param name="ModelCode">机种</param>
  /// <param name="OutputCategory">产出类别</param>
  private sealed record ProductionMonthlyTrendRowKey(string PlantCode, string ModelCode, string OutputCategory);

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
    /// 判断两行键是否相等（工厂/机种/产出类别，忽略大小写）
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
          && string.Equals(x.ModelCode, y.ModelCode, StringComparison.OrdinalIgnoreCase)
          && string.Equals(x.OutputCategory, y.OutputCategory, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 计算行键哈希（工厂/机种/产出类别大写）
    /// </summary>
    /// <param name="obj">行键</param>
    /// <returns>哈希码</returns>
    public int GetHashCode(ProductionMonthlyTrendRowKey obj) =>
        HashCode.Combine(
            obj.PlantCode.ToUpperInvariant(),
            obj.ModelCode.ToUpperInvariant(),
            obj.OutputCategory.ToUpperInvariant());
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

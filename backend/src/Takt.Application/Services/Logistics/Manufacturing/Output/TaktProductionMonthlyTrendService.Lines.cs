// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktProductionMonthlyTrendService.Lines.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：生产统计明细。组立一行一张日报，PCBA 一行一条明细
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 生产统计明细行
/// </summary>
public partial class TaktProductionMonthlyTrendService
{
    private const int MaxStatLines = 100000;

    /// <inheritdoc />
    public async Task<TaktProductionStatLineResultDto> GetProductionStatLineListAsync(TaktProductionMonthlyTrendQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var (scope, plant, start, endExclusive) = PrepareStatLine(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var (page, total) = scope.IsAssy
            ? await LoadAssyLinesAsync(plant, start, endExclusive, queryDto, skip, pageSize, pageOnly: true)
            : await LoadPcbaLinesAsync(plant, start, endExclusive, queryDto, skip, pageSize, pageOnly: true);
        return new TaktProductionStatLineResultDto
        {
            Paged = TaktPagedResult<TaktProductionStatLineDto>.Create(page, total, pageIndex, pageSize),
            RowCount = total,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportProductionStatLineAsync(
        TaktProductionMonthlyTrendQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var (scope, plant, start, endExclusive) = PrepareStatLine(query);
        var (rows, _) = scope.IsAssy
            ? await LoadAssyLinesAsync(plant, start, endExclusive, query, 0, MaxStatLines, pageOnly: false)
            : await LoadPcbaLinesAsync(plant, start, endExclusive, query, 0, MaxStatLines, pageOnly: false);
        var (keys, labels) = LineExportColumns(scope);
        var exportRows = rows.Select(row => (IReadOnlyDictionary<string, object?>)ToExportRow(row, keys)).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            keys,
            labels,
            sheetName ?? ResolveStatTabTitle(query.StatTab),
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(plant, "生产统计"));
    }

    /// <summary>
    /// 校验页签、工厂、期间
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>页签、工厂、期间起、期间止（不含）</returns>
    private (StatTabScope Scope, string Plant, DateTime Start, DateTime EndExclusive) PrepareStatLine(TaktProductionMonthlyTrendQueryDto queryDto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        if (!TryParseStatTab(queryDto.StatTab, out var scope))
        {
            ThrowBusinessException("请选择统计页签");
        }
        if (!queryDto.PeriodDateStart.HasValue || !queryDto.PeriodDateEnd.HasValue)
        {
            ThrowBusinessException("请选择期间");
        }
        EnsureThreeLayerContext();
        ResolvePeriodOrder(queryDto, out var rangeStart, out var rangeEnd);
        return (scope, queryDto.PlantCode.Trim(), rangeStart, rangeEnd.AddMonths(1));
    }

    /// <summary>
    /// 组立日报行。计划台数=明细标准产能合计，生产工数=投入工时合计，实绩 ST=工数/实绩台数
    /// </summary>
    /// <param name="plant">工厂</param>
    /// <param name="start">期间起</param>
    /// <param name="endExclusive">期间止（不含）</param>
    /// <param name="query">查询</param>
    /// <param name="skip">跳过行</param>
    /// <param name="take">取行</param>
    /// <param name="pageOnly">true 分页，false 导出</param>
    /// <returns>行与总数</returns>
    private async Task<(List<TaktProductionStatLineDto> Rows, int Total)> LoadAssyLinesAsync(
        string plant,
        DateTime start,
        DateTime endExclusive,
        TaktProductionMonthlyTrendQueryDto query,
        int skip,
        int take,
        bool pageOnly)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var team = Blank(query.TeamCode);
        var model = Blank(query.ModelCode);
        var material = Blank(query.MaterialCode);
        var category = Blank(query.ProdCategory);
        var headersQuery = _db.Queryable<TaktAssyOutput>()
            .Where(h => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0 && h.PlantCode == plant
                && h.ProdDate >= start && h.ProdDate < endExclusive)
            .WhereIF(team != null, h => h.TeamCode == team)
            .WhereIF(model != null, h => h.ModelCode == model)
            .WhereIF(material != null, h => h.MaterialCode == material)
            .WhereIF(category != null, h => h.ProdCategory == category);
        var total = await headersQuery.CountAsync();
        if (!pageOnly && total > MaxStatLines)
        {
            ThrowBusinessException($"统计结果超过 {MaxStatLines} 行，请缩小期间或增加筛选后再导出");
        }
        var headers = await headersQuery
            .OrderBy(h => h.ProdDate, OrderByType.Desc)
            .OrderBy(h => h.TeamCode, OrderByType.Asc)
            .OrderBy(h => h.BatchCode, OrderByType.Asc)
            .Skip(pageOnly ? skip : 0)
            .Take(pageOnly ? take : MaxStatLines)
            .ToListAsync();
        var sums = await SumAssyDetailsAsync(headers.Select(h => h.Id).ToList());
        var rows = headers.Select(h =>
        {
            sums.TryGetValue(h.Id, out var sum);
            var plan = sum?.PlanQty ?? 0m;
            var actual = sum?.ActualQty ?? 0m;
            var labor = sum?.LaborMinutes ?? 0m;
            var actualSt = actual > 0 ? Math.Round(labor / actual, 2, MidpointRounding.AwayFromZero) : 0m;
            return new TaktProductionStatLineDto
            {
                RowKey = h.Id.ToString(),
                PlantCode = h.PlantCode,
                TeamCode = h.TeamCode,
                ProdDate = h.ProdDate,
                BatchCode = h.BatchCode ?? string.Empty,
                MaterialCode = h.MaterialCode,
                ModelCode = h.ModelCode,
                StdMinutes = h.StdMinutes,
                PlanQty = plan,
                DirectLabor = h.DirectLabor,
                IndirectLabor = h.IndirectLabor,
                LaborMinutes = labor,
                ActualQty = actual,
                ActualStdMinutes = actualSt,
                StdDiff = Math.Round(actualSt - h.StdMinutes, 2, MidpointRounding.AwayFromZero),
                QtyDiff = Math.Round(actual - plan, 2, MidpointRounding.AwayFromZero),
                AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(actual, plan),
            };
        }).ToList();
        return (rows, total);
    }

    /// <summary>
    /// 按组立日报汇总未作废明细
    /// </summary>
    /// <param name="headerIds">主表主键</param>
    /// <returns>主表主键到合计</returns>
    private async Task<Dictionary<long, AssyDetailSum>> SumAssyDetailsAsync(List<long> headerIds)
    {
        var result = new Dictionary<long, AssyDetailSum>();
        if (headerIds.Count == 0)
        {
            return result;
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        const int batchSize = 400;
        for (var offset = 0; offset < headerIds.Count; offset += batchSize)
        {
            var batch = headerIds.Skip(offset).Take(batchSize).ToList();
            var sums = await _db.Queryable<TaktAssyOutputDetail>()
                .Where(d => d.TenantCode == tenant && d.CompanyCode == company && d.IsDeleted == 0 && d.IsObsolete == 0
                    && batch.Contains(d.AssyOutputId))
                .GroupBy(d => d.AssyOutputId)
                .Select(d => new AssyDetailSum
                {
                    AssyOutputId = d.AssyOutputId,
                    PlanQty = SqlFunc.AggregateSum(d.StdCapacity),
                    ActualQty = SqlFunc.AggregateSum(d.ProdActualQty),
                    LaborMinutes = SqlFunc.AggregateSum(d.InputMinutes),
                })
                .ToListAsync();
            foreach (var sum in sums)
            {
                result[sum.AssyOutputId] = sum;
            }
        }
        return result;
    }

    /// <summary>
    /// PCBA 明细行
    /// </summary>
    /// <param name="plant">工厂</param>
    /// <param name="start">期间起</param>
    /// <param name="endExclusive">期间止（不含）</param>
    /// <param name="query">查询</param>
    /// <param name="skip">跳过行</param>
    /// <param name="take">取行</param>
    /// <param name="pageOnly">true 分页，false 导出</param>
    /// <returns>行与总数</returns>
    private async Task<(List<TaktProductionStatLineDto> Rows, int Total)> LoadPcbaLinesAsync(
        string plant,
        DateTime start,
        DateTime endExclusive,
        TaktProductionMonthlyTrendQueryDto query,
        int skip,
        int take,
        bool pageOnly)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var team = Blank(query.TeamCode);
        var model = Blank(query.ModelCode);
        var material = Blank(query.MaterialCode);
        var category = Blank(query.ProdCategory);
        var lineQuery = _db.Queryable<TaktPcbaOutputDetail, TaktPcbaOutput>((d, h) => d.PcbaOutputId == h.Id)
            .Where((d, h) => d.TenantCode == tenant && d.CompanyCode == company && d.IsDeleted == 0 && d.IsObsolete == 0
                && h.IsDeleted == 0 && h.PlantCode == plant
                && h.ProdDate >= start && h.ProdDate < endExclusive)
            .WhereIF(team != null, (d, h) => d.TeamCode == team)
            .WhereIF(model != null, (d, h) => h.ModelCode == model)
            .WhereIF(material != null, (d, h) => h.MaterialCode == material)
            .WhereIF(category != null, (d, h) => h.ProdCategory == category);
        var total = await lineQuery.CountAsync();
        if (!pageOnly && total > MaxStatLines)
        {
            ThrowBusinessException($"统计结果超过 {MaxStatLines} 行，请缩小期间或增加筛选后再导出");
        }
        var rows = await lineQuery
            .OrderBy((d, h) => h.ProdDate, OrderByType.Desc)
            .OrderBy((d, h) => d.TeamCode, OrderByType.Asc)
            .OrderBy((d, h) => h.BatchCode, OrderByType.Asc)
            .Select((d, h) => new TaktProductionStatLineDto
            {
                RowKey = SqlFunc.ToString(d.Id),
                PlantCode = h.PlantCode,
                TeamCode = d.TeamCode,
                ProdDate = h.ProdDate,
                BatchCode = h.BatchCode ?? string.Empty,
                MaterialCode = h.MaterialCode,
                ModelCode = h.ModelCode,
                LotQty = d.BatchQty,
                PcbBoardType = d.PcbBoardType,
                PanelSide = d.PanelSide,
                ActualQty = d.DailyCompletedQty,
                CumulativeQty = d.TotalCompletedQty,
                CompletedStatus = d.CompletedStatus,
                DefectQty = d.DefectCount,
            })
            .Skip(pageOnly ? skip : 0)
            .Take(pageOnly ? take : MaxStatLines)
            .ToListAsync();
        return (rows, total);
    }

    /// <summary>
    /// 按页签导出列
    /// </summary>
    /// <param name="scope">页签范围</param>
    /// <returns>键与标题</returns>
    private static (List<string> Keys, List<string> Labels) LineExportColumns(StatTabScope scope)
    {
        if (!scope.IsAssy)
        {
            var keys = new List<string>();
            var labels = new List<string>();
            if (scope.GroupByTeam)
            {
                keys.Add("teamCode");
                labels.Add("班组");
            }
            keys.AddRange(["prodDate", "batchCode", "lotQty", "modelCode", "pcbBoardType", "panelSide", "actualQty", "cumulativeQty", "completedStatus", "defectQty"]);
            labels.AddRange(["生产日期", "生产批次", "LOT数量", "机种", "Pcb类别", "板面", "生产实绩", "累计生产", "完成状态", scope.GroupByTeam ? "不良台数" : "不良数量"]);
            return (keys, labels);
        }
        if (scope.GroupByTeam)
        {
            return (
                ["teamCode", "prodDate", "batchCode", "materialCode", "modelCode", "stdMinutes", "planQty", "directLabor", "indirectLabor", "laborMinutes", "actualQty", "actualStdMinutes", "stdDiff", "qtyDiff", "achievementRate"],
                ["生产班组", "生产日期", "生产批次", "物料", "机种", "ST", "计划台数", "直接", "间接", "工数", "实绩台数", "实绩ST", "ST差异", "台数差异", "达成率"]);
        }
        return (
            ["prodDate", "modelCode", "stdMinutes", "planQty", "laborMinutes", "actualQty", "directLabor", "actualStdMinutes", "stdDiff", "qtyDiff", "achievementRate"],
            ["生产日期", "机种编码", "ST", "计划台数", "生产工数", "实绩台数", "直接人数", "实绩ST", "ST差异", "台数差异", "达成率"]);
    }

    /// <summary>
    /// 导出行
    /// </summary>
    /// <param name="row">明细</param>
    /// <param name="keys">列键</param>
    /// <returns>字典</returns>
    private static Dictionary<string, object?> ToExportRow(TaktProductionStatLineDto row, IReadOnlyList<string> keys)
    {
        var map = new Dictionary<string, object?>
        {
            ["teamCode"] = row.TeamCode,
            ["prodDate"] = row.ProdDate.ToString("yyyy-MM-dd"),
            ["batchCode"] = row.BatchCode,
            ["materialCode"] = row.MaterialCode,
            ["modelCode"] = row.ModelCode,
            ["stdMinutes"] = row.StdMinutes,
            ["planQty"] = row.PlanQty,
            ["directLabor"] = row.DirectLabor,
            ["indirectLabor"] = row.IndirectLabor,
            ["laborMinutes"] = row.LaborMinutes,
            ["actualQty"] = row.ActualQty,
            ["actualStdMinutes"] = row.ActualStdMinutes,
            ["stdDiff"] = row.StdDiff,
            ["qtyDiff"] = row.QtyDiff,
            ["achievementRate"] = row.AchievementRate,
            ["lotQty"] = row.LotQty,
            ["pcbBoardType"] = row.PcbBoardType,
            ["panelSide"] = row.PanelSide,
            ["cumulativeQty"] = row.CumulativeQty,
            ["completedStatus"] = row.CompletedStatus,
            ["defectQty"] = row.DefectQty,
        };
        return keys.ToDictionary(key => key, key => map[key]);
    }

    /// <summary>
    /// 空白转 null
    /// </summary>
    /// <param name="value">原值</param>
    /// <returns>去空白后的值</returns>
    private static string? Blank(string? value)
    {
        var text = value?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// 组立明细合计
    /// </summary>
    private sealed class AssyDetailSum
    {
        /// <summary>
        /// 组立日报主键
        /// </summary>
        public long AssyOutputId { get; set; }

        /// <summary>
        /// 计划台数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实绩台数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 生产工数
        /// </summary>
        public decimal LaborMinutes { get; set; }
    }
}

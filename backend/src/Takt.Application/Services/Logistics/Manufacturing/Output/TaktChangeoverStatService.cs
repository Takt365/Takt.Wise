// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktChangeoverStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：切换统计。按工厂与期间列出生产切换记录，可导出
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 切换统计
/// </summary>
public class TaktChangeoverStatService : TaktServiceBase, ITaktChangeoverStatService
{
    private const int MaxRows = 100000;
    private const int MaxMonths = 12;

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktChangeoverStatService(
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetChangeoverStatPlantOptionsAsync(string? keyword = null)
    {
        EnsureThreeLayerContext();
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var codes = await _db.Queryable<TaktProductionChangeover>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0 && x.PlantCode != "")
            .GroupBy(x => x.PlantCode)
            .Select(x => x.PlantCode)
            .ToListAsync();
        return ToOptions(codes, keyword);
    }

    /// <inheritdoc />
    public async Task<List<TaktSelectOption>> GetChangeoverStatTeamOptionsAsync(string? plantCode, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var plant = plantCode?.Trim() ?? string.Empty;
        if (plant.Length == 0)
        {
            return [];
        }
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var codes = await _db.Queryable<TaktProductionChangeover>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant && x.TeamCode != "")
            .GroupBy(x => x.TeamCode)
            .Select(x => x.TeamCode)
            .ToListAsync();
        return ToOptions(codes, keyword);
    }

    /// <inheritdoc />
    public async Task<TaktChangeoverStatResultDto> GetChangeoverStatListAsync(TaktChangeoverStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var (rows, total) = await LoadAsync(queryDto, skip, pageSize, pageOnly: true);
        return new TaktChangeoverStatResultDto
        {
            Paged = TaktPagedResult<TaktChangeoverStatRowDto>.Create(rows, total, pageIndex, pageSize),
            RowCount = total,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportChangeoverStatAsync(
        TaktChangeoverStatQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var (rows, _) = await LoadAsync(query, 0, MaxRows, pageOnly: false);
        var keys = new List<string>
        {
            "plantCode", "prodCategory", "changeoverCategory", "prodDate", "teamCode",
            "currentProdOrderCode", "currentModelCode", "changeoverProdOrderCode", "changeoverModelCode",
            "changeoverCount", "changeoverTime", "instrumentSetupTime", "totalChangeoverTime",
            "readSopTime", "learningTime", "personCount", "totalLearningTime", "totalSopTime",
        };
        var labels = new List<string>
        {
            "工厂", "生产类别", "切换类别", "生产日期", "生产班组",
            "当前工单", "当前机种", "切换后工单", "切换后机种",
            "切换次数", "切换时间", "仪设时间", "切换总时间",
            "读取SOP时间", "学习时间", "人数", "学习总时间", "SOP总时间",
        };
        var exportRows = rows.Select(row => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["plantCode"] = row.PlantCode,
            ["prodCategory"] = row.ProdCategory,
            ["changeoverCategory"] = row.ChangeoverCategory,
            ["prodDate"] = row.ProdDate.ToString("yyyy-MM-dd"),
            ["teamCode"] = row.TeamCode,
            ["currentProdOrderCode"] = row.CurrentProdOrderCode,
            ["currentModelCode"] = row.CurrentModelCode,
            ["changeoverProdOrderCode"] = row.ChangeoverProdOrderCode,
            ["changeoverModelCode"] = row.ChangeoverModelCode,
            ["changeoverCount"] = row.ChangeoverCount,
            ["changeoverTime"] = row.ChangeoverTime,
            ["instrumentSetupTime"] = row.InstrumentSetupTime,
            ["totalChangeoverTime"] = row.TotalChangeoverTime,
            ["readSopTime"] = row.ReadSopTime,
            ["learningTime"] = row.LearningTime,
            ["personCount"] = row.PersonCount,
            ["totalLearningTime"] = row.TotalLearningTime,
            ["totalSopTime"] = row.TotalSopTime,
        }).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            keys,
            labels,
            sheetName ?? "切换统计",
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode?.Trim() ?? string.Empty, "切换统计"));
    }

    /// <summary>
    /// 按工厂与期间加载切换记录
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <param name="skip">跳过</param>
    /// <param name="take">取行</param>
    /// <param name="pageOnly">true 分页，false 导出</param>
    /// <returns>行与总数</returns>
    private async Task<(List<TaktChangeoverStatRowDto> Rows, int Total)> LoadAsync(
        TaktChangeoverStatQueryDto queryDto,
        int skip,
        int take,
        bool pageOnly)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
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
        var monthCount = ((endMonth.Year - start.Year) * 12) + endMonth.Month - start.Month + 1;
        if (monthCount > MaxMonths)
        {
            ThrowBusinessException($"分析区间不得超过 {MaxMonths} 个月");
        }
        var endExclusive = endMonth.AddMonths(1);
        var plant = queryDto.PlantCode.Trim();
        var category = Blank(queryDto.ProdCategory);
        var changeover = Blank(queryDto.ChangeoverCategory);
        var team = Blank(queryDto.TeamCode);
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktProductionChangeover>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant && x.ProdDate >= start && x.ProdDate < endExclusive)
            .WhereIF(category != null, x => x.ProdCategory == category)
            .WhereIF(changeover != null, x => x.ChangeoverCategory == changeover)
            .WhereIF(team != null, x => x.TeamCode == team);
        var total = await query.CountAsync();
        if (!pageOnly && total > MaxRows)
        {
            ThrowBusinessException($"统计结果超过 {MaxRows} 行，请缩小期间或增加筛选后再导出");
        }
        var list = await query
            .OrderBy(x => x.ProdDate, OrderByType.Desc)
            .OrderBy(x => x.TeamCode, OrderByType.Asc)
            .OrderBy(x => x.CurrentProdOrderCode, OrderByType.Asc)
            .Skip(pageOnly ? skip : 0)
            .Take(pageOnly ? take : MaxRows)
            .ToListAsync();
        var rows = list.Select(x => new TaktChangeoverStatRowDto
        {
            RowKey = x.Id.ToString(),
            PlantCode = x.PlantCode,
            ProdCategory = x.ProdCategory,
            ChangeoverCategory = x.ChangeoverCategory,
            ProdDate = x.ProdDate,
            TeamCode = x.TeamCode,
            CurrentProdOrderCode = x.CurrentProdOrderCode,
            CurrentModelCode = x.CurrentModelCode,
            ChangeoverProdOrderCode = x.ChangeoverProdOrderCode,
            ChangeoverModelCode = x.ChangeoverModelCode,
            ChangeoverCount = x.ChangeoverCount,
            ChangeoverTime = x.ChangeoverTime,
            InstrumentSetupTime = x.InstrumentSetupTime,
            TotalChangeoverTime = x.TotalChangeoverTime,
            ReadSopTime = x.ReadSopTime,
            LearningTime = x.LearningTime,
            PersonCount = x.PersonCount,
            TotalLearningTime = x.TotalLearningTime,
            TotalSopTime = x.TotalSopTime,
        }).ToList();
        return (rows, total);
    }

    /// <summary>
    /// 去空白
    /// </summary>
    /// <param name="value">原值</param>
    /// <returns>空则 null</returns>
    private static string? Blank(string? value)
    {
        var text = value?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// 编码下拉
    /// </summary>
    /// <param name="codes">编码</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
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
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktDefectStatService.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：不良统计。组立读工单/批次统计表全列，SMT 检查与 PCBA 修理按明细行出列
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 不良统计
/// </summary>
public class TaktDefectStatService : TaktServiceBase, ITaktDefectStatService
{
    private const string TabMfg1Order = "assy-order";
    private const string TabMfg1Batch = "assy-batch";
    private const string TabMtlInspection = "smt-inspection";
    private const string TabMfg2Repair = "pcba-repair";
    private const int MaxRows = 100000;

    private readonly ISqlSugarClient _db;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="db">SqlSugar 客户端</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化</param>
    public TaktDefectStatService(
        ISqlSugarClient db,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _db = db;
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetDefectStatPlantOptionsAsync(string? statTab, string? keyword = null)
    {
        return DistinctAsync(statTab, null, keyword, "plant");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetDefectStatOrderOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "order");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetDefectStatBatchOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "batch");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetDefectStatModelOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "model");
    }

    /// <inheritdoc />
    public Task<List<TaktSelectOption>> GetDefectStatMaterialOptionsAsync(string? statTab, string? plantCode, string? keyword = null)
    {
        return DistinctAsync(statTab, plantCode, keyword, "material");
    }

    /// <inheritdoc />
    public async Task<TaktDefectOrderStatResultDto> GetDefectStatListAsync(TaktDefectOrderStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        var rows = await LoadRowsAsync(queryDto);
        var pageIndex = TaktPagedClamp.NormalizePageIndex(queryDto.PageIndex);
        var pageSize = TaktPagedClamp.NormalizePageSize(queryDto.PageSize);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var page = rows.Skip(skip).Take(pageSize).ToList();
        return new TaktDefectOrderStatResultDto
        {
            Paged = TaktPagedResult<TaktDefectOrderStatRowDto>.Create(page, rows.Count, pageIndex, pageSize),
            RowCount = rows.Count,
        };
    }

    /// <inheritdoc />
    public async Task<(string fileName, byte[] fileContent)> ExportDefectStatAsync(
        TaktDefectOrderStatQueryDto query,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(query);
        var rows = await LoadRowsAsync(query);
        var (columnKeys, columnLabels) = ExportColumns(query.StatTab);
        var exportRows = rows.Select(ToExportRow).ToList();
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            exportRows,
            columnKeys,
            columnLabels,
            sheetName ?? ResolveTitle(query.StatTab),
            fileName ?? TaktNamingHelper.PlantPrefixedTrendExportBase(query.PlantCode, "不良统计"));
    }

    /// <summary>
    /// 按页签加载统计行
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>行</returns>
    private async Task<List<TaktDefectOrderStatRowDto>> LoadRowsAsync(TaktDefectOrderStatQueryDto queryDto)
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
        var order = BlankToNull(queryDto.ProdOrderCode);
        var batch = BlankToNull(queryDto.BatchCode);
        var model = BlankToNull(queryDto.ModelCode);
        var material = BlankToNull(queryDto.MaterialCode);
        var category = BlankToNull(queryDto.ProdCategory);
        var rows = tab switch
        {
            TabMfg1Order => await LoadAssyOrderAsync(plant, start, endExclusive, order, batch, model, material, category),
            TabMfg1Batch => await LoadAssyBatchAsync(plant, start, endExclusive, batch, model, material, category),
            TabMtlInspection => await LoadSmtInspectionAsync(plant, start, endExclusive, order, batch, model, material, category),
            _ => await LoadPcbaRepairAsync(plant, start, endExclusive, order, batch, model, material, category),
        };
        if (rows.Count > MaxRows)
        {
            ThrowBusinessException($"统计结果超过 {MaxRows} 行，请缩小期间或指定生产类别、工单、批次、机种、物料");
        }
        return rows;
    }

    /// <summary>
    /// 组立工单不良
    /// </summary>
    private async Task<List<TaktDefectOrderStatRowDto>> LoadAssyOrderAsync(
        string plant, DateTime start, DateTime endExclusive, string? order, string? batch, string? model, string? material, string? category)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktAssyOrderDefect>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant && x.LastProdDate >= start && x.LastProdDate < endExclusive);
        if (order != null)
        {
            query = query.Where(x => x.ProdOrderCode == order);
        }
        if (batch != null)
        {
            query = query.Where(x => x.BatchCode == batch);
        }
        if (model != null)
        {
            query = query.Where(x => x.ModelCode == model);
        }
        if (material != null)
        {
            query = query.Where(x => x.MaterialCode == material);
        }
        if (category != null)
        {
            query = query.Where(x => x.ProdCategory == category);
        }
        var list = await query.ToListAsync();
        return list
            .OrderBy(x => x.ProdOrderCode, StringComparer.Ordinal)
            .Select(x => new TaktDefectOrderStatRowDto
            {
                RowKey = $"{x.ProdCategory}|{x.ProdOrderCode}",
                PlantCode = x.PlantCode,
                ProdCategory = x.ProdCategory,
                ProdOrderCode = x.ProdOrderCode,
                ProdDateGroup = x.ProdDateGroup ?? string.Empty,
                ModelCode = x.ModelCode,
                MaterialCode = x.MaterialCode,
                BatchCode = x.BatchCode ?? string.Empty,
                ProdOrderQty = x.ProdOrderQty,
                ProdActualQty = x.ProdActualQty,
                ActualQty = x.ProdActualQty,
                GoodQty = x.GoodQuantity,
                DefectQty = x.DefectQty,
                DefectRatePercent = x.DefectRatePercent,
                YieldRatePercent = x.YieldRatePercent,
                LastProdDate = x.LastProdDate,
                ReportCount = x.ReportCount,
                OrderStatus = x.OrderStatus,
            })
            .ToList();
    }

    /// <summary>
    /// 组立批次不良
    /// </summary>
    private async Task<List<TaktDefectOrderStatRowDto>> LoadAssyBatchAsync(
        string plant, DateTime start, DateTime endExclusive, string? batch, string? model, string? material, string? category)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktAssyBatchDefect>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0
                && x.PlantCode == plant && x.LastProdDate >= start && x.LastProdDate < endExclusive);
        if (batch != null)
        {
            query = query.Where(x => x.BatchCode == batch);
        }
        if (model != null)
        {
            query = query.Where(x => x.ModelCode == model);
        }
        if (material != null)
        {
            query = query.Where(x => x.MaterialGroup != null && x.MaterialGroup.Contains(material));
        }
        if (category != null)
        {
            query = query.Where(x => x.ProdCategory == category);
        }
        var list = await query.ToListAsync();
        return list
            .OrderBy(x => x.BatchCode, StringComparer.Ordinal)
            .Select(x => new TaktDefectOrderStatRowDto
            {
                RowKey = $"{x.ProdCategory}|{x.BatchCode}",
                PlantCode = x.PlantCode,
                ProdCategory = x.ProdCategory,
                BatchCode = x.BatchCode,
                ProdDateGroup = x.ProdDateGroup ?? string.Empty,
                ProdOrderGroup = x.ProdOrderGroup ?? string.Empty,
                ModelCode = x.ModelCode,
                MaterialGroup = x.MaterialGroup ?? string.Empty,
                MaterialCode = x.MaterialGroup ?? string.Empty,
                BatchOrderQty = x.BatchOrderQty,
                ProdOrderQtyGroup = x.ProdOrderQtyGroup ?? string.Empty,
                ProdActualQty = x.ProdActualQty,
                ActualQty = x.ProdActualQty,
                GoodQty = x.GoodQuantity,
                DefectQty = x.DefectQty,
                DefectRatePercent = x.DefectRatePercent,
                YieldRatePercent = x.YieldRatePercent,
                LastProdDate = x.LastProdDate,
                ReportCount = x.ReportCount,
                BatchStatus = x.BatchStatus,
            })
            .ToList();
    }

    /// <summary>
    /// SMT 检查明细。一行一条检查明细，期间按 B 面或 T 面实装日期
    /// </summary>
    private async Task<List<TaktDefectOrderStatRowDto>> LoadSmtInspectionAsync(
        string plant, DateTime start, DateTime endExclusive, string? order, string? batch, string? model, string? material, string? category)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktPcbaInspection>()
            .InnerJoin<TaktPcbaInspectionDetail>((h, d) => h.Id == d.PcbaInspectionId)
            .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0
                && d.IsDeleted == 0 && d.IsObsolete == 0 && h.PlantCode == plant
                && ((d.BSideAssemblyDate >= start && d.BSideAssemblyDate < endExclusive)
                    || (d.TSideAssemblyDate >= start && d.TSideAssemblyDate < endExclusive)));
        if (order != null)
        {
            query = query.Where((h, d) => h.ProdOrderCode == order);
        }
        if (batch != null)
        {
            query = query.Where((h, d) => h.BatchCode == batch);
        }
        if (model != null)
        {
            query = query.Where((h, d) => h.ModelCode == model);
        }
        if (material != null)
        {
            query = query.Where((h, d) => h.MaterialCode == material);
        }
        if (category != null)
        {
            query = query.Where((h, d) => h.ProdCategory == category);
        }
#pragma warning disable CS8601
        var list = await query
            .OrderBy((h, d) => d.TSideAssemblyDate, OrderByType.Desc)
            .OrderBy((h, d) => d.BSideAssemblyDate, OrderByType.Desc)
            .OrderBy((h, d) => h.ProdOrderCode, OrderByType.Asc)
            .Select((h, d) => new TaktDefectOrderStatRowDto
            {
                RowKey = SqlFunc.ToString(d.Id),
                PlantCode = h.PlantCode,
                ProdCategory = h.ProdCategory,
                ProdOrderType = h.ProdOrderType,
                ProdOrderCode = h.ProdOrderCode,
                ProdOrderQty = h.ProdOrderQty,
                ModelCode = h.ModelCode,
                BatchCode = h.BatchCode,
                MaterialCode = h.MaterialCode,
                Remark = h.Remark,
                LineNumber = d.LineNumber,
                PcbaBoardType = d.PcbaBoardType,
                VisualInspectionLine = d.VisualInspectionLine,
                AoiLine = d.AoiLine,
                BSideAssemblyDate = d.BSideAssemblyDate,
                TSideAssemblyDate = d.TSideAssemblyDate,
                ShiftNo = d.ShiftNo,
                InspectorName = d.InspectorName,
                DailyCompletedQty = d.DailyCompletedQty,
                InspectionQty = d.InspectionQty,
                InspectionStatus = d.InspectionStatus,
                TeamCode = d.TeamCode,
                InspectionWorkHours = d.InspectionWorkHours,
                AoiWorkHours = d.AoiWorkHours,
                DefectQty = d.DefectQty,
                HandPlacement = d.HandPlacement,
                SerialNumber = d.SerialNumber,
                Content = d.Content,
                DefectLocation = d.DefectLocation,
            })
            .ToListAsync();
#pragma warning restore CS8601
        foreach (var row in list)
        {
            FillBlank(row);
            row.InspectionDate = ResolveInspectionDate(row.TSideAssemblyDate, row.BSideAssemblyDate, start, endExclusive);
        }
        return list;
    }

    /// <summary>
    /// PCBA 修理明细。一行一条改修明细，期间按生产日期
    /// </summary>
    private async Task<List<TaktDefectOrderStatRowDto>> LoadPcbaRepairAsync(
        string plant, DateTime start, DateTime endExclusive, string? order, string? batch, string? model, string? material, string? category)
    {
        var tenant = CurrentTenantCode;
        var company = CurrentCompanyCode;
        var query = _db.Queryable<TaktPcbaRepair>()
            .InnerJoin<TaktPcbaRepairDetail>((h, d) => h.Id == d.PcbaRepairId)
            .Where((h, d) => h.TenantCode == tenant && h.CompanyCode == company && h.IsDeleted == 0
                && d.IsDeleted == 0 && d.IsObsolete == 0 && h.PlantCode == plant
                && h.ProdDate >= start && h.ProdDate < endExclusive);
        if (order != null)
        {
            query = query.Where((h, d) => h.ProdOrderCode == order);
        }
        if (batch != null)
        {
            query = query.Where((h, d) => h.BatchCode == batch);
        }
        if (model != null)
        {
            query = query.Where((h, d) => h.ModelCode == model);
        }
        if (material != null)
        {
            query = query.Where((h, d) => h.MaterialCode == material);
        }
        if (category != null)
        {
            query = query.Where((h, d) => h.ProdCategory == category);
        }
#pragma warning disable CS8601
        var list = await query
            .OrderBy((h, d) => h.ProdDate, OrderByType.Desc)
            .OrderBy((h, d) => h.ProdOrderCode, OrderByType.Asc)
            .Select((h, d) => new TaktDefectOrderStatRowDto
            {
                RowKey = SqlFunc.ToString(d.Id),
                PlantCode = h.PlantCode,
                ProdCategory = h.ProdCategory,
                ProdDate = h.ProdDate,
                TeamCode = h.TeamCode,
                ShiftNo = h.ShiftNo,
                ProdOrderType = h.ProdOrderType,
                ProdOrderCode = h.ProdOrderCode,
                ProdOrderQty = h.ProdOrderQty,
                ModelCode = h.ModelCode,
                BatchCode = h.BatchCode,
                MaterialCode = h.MaterialCode,
                Remark = h.Remark,
                LineNumber = d.LineNumber,
                PcbaBoardType = d.PcbaBoardType,
                ProdActualQty = d.ProdActualQty,
                DetailTeamCode = d.TeamCode,
                CardCode = d.CardCode,
                DefectSymptom = d.DefectSymptom,
                DefectEngineering = d.DefectEngineering,
                DefectReason = d.DefectReason,
                DefectQty = d.DefectQty,
                DefectResponsibility = d.DefectResponsibility,
                DefectNature = d.DefectNature,
                RepairOperatorName = d.RepairOperatorName,
            })
            .ToListAsync();
#pragma warning restore CS8601
        foreach (var row in list)
        {
            FillBlank(row);
        }
        return list;
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
        List<string> codes;
        if (tab is TabMfg1Order or TabMfg1Batch)
        {
            codes = await DistinctAssyAsync(tab, tenant, company, plant, field);
        }
        else if (tab == TabMtlInspection)
        {
            codes = await DistinctInspectionAsync(tenant, company, plant, field);
        }
        else
        {
            codes = await DistinctRepairAsync(tenant, company, plant, field);
        }
        return ToOptions(codes, keyword);
    }

    /// <summary>
    /// 组立汇总表去重
    /// </summary>
    private async Task<List<string>> DistinctAssyAsync(string tab, string tenant, string company, string plant, string field)
    {
        if (tab == TabMfg1Order)
        {
            var query = _db.Queryable<TaktAssyOrderDefect>()
                .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
            if (plant.Length > 0)
            {
                query = query.Where(x => x.PlantCode == plant);
            }
            return field switch
            {
                "plant" => await query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync(),
                "order" => await query.GroupBy(x => x.ProdOrderCode).Select(x => x.ProdOrderCode).ToListAsync(),
                "batch" => await query.Where(x => x.BatchCode != null && x.BatchCode != "").GroupBy(x => x.BatchCode).Select(x => x.BatchCode!).ToListAsync(),
                "model" => await query.GroupBy(x => x.ModelCode).Select(x => x.ModelCode).ToListAsync(),
                _ => await query.GroupBy(x => x.MaterialCode).Select(x => x.MaterialCode).ToListAsync(),
            };
        }
        var batchQuery = _db.Queryable<TaktAssyBatchDefect>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            batchQuery = batchQuery.Where(x => x.PlantCode == plant);
        }
        if (field == "material")
        {
            var groups = await batchQuery.Where(x => x.MaterialGroup != null && x.MaterialGroup != "").Select(x => x.MaterialGroup).ToListAsync();
            return groups.SelectMany(g => (g ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)).ToList();
        }
        return field switch
        {
            "plant" => await batchQuery.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync(),
            "batch" => await batchQuery.GroupBy(x => x.BatchCode).Select(x => x.BatchCode).ToListAsync(),
            "order" => [],
            _ => await batchQuery.GroupBy(x => x.ModelCode).Select(x => x.ModelCode).ToListAsync(),
        };
    }

    /// <summary>
    /// SMT 检查主表去重
    /// </summary>
    private Task<List<string>> DistinctInspectionAsync(string tenant, string company, string plant, string field)
    {
        var query = _db.Queryable<TaktPcbaInspection>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        return field switch
        {
            "plant" => query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync(),
            "order" => query.GroupBy(x => x.ProdOrderCode).Select(x => x.ProdOrderCode).ToListAsync(),
            "batch" => query.Where(x => x.BatchCode != null && x.BatchCode != "").GroupBy(x => x.BatchCode).Select(x => x.BatchCode!).ToListAsync(),
            "model" => query.GroupBy(x => x.ModelCode).Select(x => x.ModelCode).ToListAsync(),
            _ => query.GroupBy(x => x.MaterialCode).Select(x => x.MaterialCode).ToListAsync(),
        };
    }

    /// <summary>
    /// PCBA 修理主表去重
    /// </summary>
    private Task<List<string>> DistinctRepairAsync(string tenant, string company, string plant, string field)
    {
        var query = _db.Queryable<TaktPcbaRepair>()
            .Where(x => x.TenantCode == tenant && x.CompanyCode == company && x.IsDeleted == 0);
        if (plant.Length > 0)
        {
            query = query.Where(x => x.PlantCode == plant);
        }
        return field switch
        {
            "plant" => query.GroupBy(x => x.PlantCode).Select(x => x.PlantCode).ToListAsync(),
            "order" => query.GroupBy(x => x.ProdOrderCode).Select(x => x.ProdOrderCode).ToListAsync(),
            "batch" => query.Where(x => x.BatchCode != null && x.BatchCode != "").GroupBy(x => x.BatchCode).Select(x => x.BatchCode!).ToListAsync(),
            "model" => query.GroupBy(x => x.ModelCode).Select(x => x.ModelCode).ToListAsync(),
            _ => query.GroupBy(x => x.MaterialCode).Select(x => x.MaterialCode).ToListAsync(),
        };
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
        return tab is TabMfg1Order or TabMfg1Batch or TabMtlInspection or TabMfg2Repair;
    }

    /// <summary>
    /// 页签名称
    /// </summary>
    private static string ResolveTitle(string? statTab) =>
        statTab?.Trim().ToLowerInvariant() switch
        {
            TabMfg1Order => "组立按工单",
            TabMfg1Batch => "组立按批次",
            TabMtlInspection => "SMT检查",
            TabMfg2Repair => "PCBA修理",
            _ => "不良统计",
        };

    /// <summary>
    /// 导出列
    /// </summary>
    private static (List<string> Keys, List<string> Labels) ExportColumns(string? statTab)
    {
        var tab = statTab?.Trim().ToLowerInvariant();
        if (tab == TabMfg1Order)
        {
            return (
                ["plantCode", "prodCategory", "prodOrderCode", "prodDateGroup", "modelCode", "materialCode", "batchCode", "prodOrderQty", "prodActualQty", "goodQty", "defectQty", "defectRatePercent", "yieldRatePercent", "lastProdDate", "reportCount", "orderStatus"],
                ["工厂", "生产类别", "工单号", "生产日期组", "机种", "物料编码", "批次", "工单数量", "累计生实实绩", "累计无不良数量", "累计不良数量", "不良率", "直行率", "最近生产日期", "日报笔数", "工单状态"]);
        }
        if (tab == TabMfg1Batch)
        {
            return (
                ["plantCode", "prodCategory", "batchCode", "prodDateGroup", "prodOrderGroup", "modelCode", "materialGroup", "batchOrderQty", "prodOrderQtyGroup", "prodActualQty", "goodQty", "defectQty", "defectRatePercent", "yieldRatePercent", "lastProdDate", "reportCount", "batchStatus"],
                ["工厂", "生产类别", "批次", "生产日期组", "生产工单组", "机种", "生产物料组", "批次工单总数量", "订单数量组", "累计生实实绩", "累计无不良数量", "累计不良数量", "不良率", "直行率", "最近生产日期", "日报笔数", "批次状态"]);
        }
        if (tab == TabMtlInspection)
        {
            return (
                ["plantCode", "prodCategory", "prodOrderType", "prodOrderCode", "prodOrderQty", "modelCode", "batchCode", "materialCode", "remark", "lineNumber", "pcbaBoardType", "visualInspectionLine", "aoiLine", "bSideAssemblyDate", "tSideAssemblyDate", "shiftNo", "inspectorName", "dailyCompletedQty", "inspectionQty", "inspectionStatus", "teamCode", "inspectionWorkHours", "aoiWorkHours", "defectQty", "handPlacement", "serialNumber", "content", "defectLocation"],
                ["工厂", "生产类别", "工单类别", "工单号", "工单数量", "机种", "批次", "物料编码", "备注", "行号", "PCBA板别", "目视线别", "AOI线别", "B面实装日期", "T面实装日期", "班次", "检查员名称", "当日完成数量", "检查数量", "检查状态", "生产班组", "检查工数", "AOI工数", "不良数量", "手贴", "流水号", "内容", "不良个所"]);
        }
        return (
            ["plantCode", "prodCategory", "prodDate", "teamCode", "shiftNo", "prodOrderType", "prodOrderCode", "prodOrderQty", "modelCode", "batchCode", "materialCode", "remark", "lineNumber", "pcbaBoardType", "prodActualQty", "detailTeamCode", "cardCode", "defectSymptom", "defectEngineering", "defectReason", "defectQty", "defectResponsibility", "defectNature", "repairOperatorName"],
            ["工厂", "生产类别", "生产日期", "生产班组", "班次", "工单类别", "工单号", "工单数量", "机种", "批次", "物料编码", "备注", "行号", "PCBA板别", "生产实绩", "明细生产班组", "卡号", "不良症状", "检出工程", "不良原因", "不良数量", "责任归属", "不良性质", "修理员名称"]);
    }

    /// <summary>
    /// 导出行
    /// </summary>
    private static IReadOnlyDictionary<string, object?> ToExportRow(TaktDefectOrderStatRowDto row)
    {
        return new Dictionary<string, object?>
        {
            ["plantCode"] = row.PlantCode,
            ["prodCategory"] = row.ProdCategory,
            ["prodOrderCode"] = row.ProdOrderCode,
            ["prodDateGroup"] = row.ProdDateGroup,
            ["modelCode"] = row.ModelCode,
            ["materialCode"] = row.MaterialCode,
            ["batchCode"] = row.BatchCode,
            ["prodOrderQty"] = row.ProdOrderQty,
            ["prodActualQty"] = row.ProdActualQty,
            ["goodQty"] = row.GoodQty,
            ["defectQty"] = row.DefectQty,
            ["defectRatePercent"] = row.DefectRatePercent,
            ["yieldRatePercent"] = row.YieldRatePercent,
            ["lastProdDate"] = row.LastProdDate?.ToString("yyyy-MM-dd"),
            ["reportCount"] = row.ReportCount,
            ["orderStatus"] = row.OrderStatus,
            ["prodOrderGroup"] = row.ProdOrderGroup,
            ["materialGroup"] = row.MaterialGroup,
            ["batchOrderQty"] = row.BatchOrderQty,
            ["prodOrderQtyGroup"] = row.ProdOrderQtyGroup,
            ["batchStatus"] = row.BatchStatus,
            ["inspectionDate"] = row.InspectionDate?.ToString("yyyy-MM-dd"),
            ["pcbaBoardType"] = row.PcbaBoardType,
            ["visualInspectionLine"] = row.VisualInspectionLine,
            ["aoiLine"] = row.AoiLine,
            ["bSideAssemblyDate"] = row.BSideAssemblyDate?.ToString("yyyy-MM-dd"),
            ["tSideAssemblyDate"] = row.TSideAssemblyDate?.ToString("yyyy-MM-dd"),
            ["shiftNo"] = row.ShiftNo,
            ["inspectorName"] = row.InspectorName,
            ["dailyCompletedQty"] = row.DailyCompletedQty,
            ["inspectionQty"] = row.InspectionQty,
            ["inspectionStatus"] = row.InspectionStatus,
            ["teamCode"] = row.TeamCode,
            ["inspectionWorkHours"] = row.InspectionWorkHours,
            ["aoiWorkHours"] = row.AoiWorkHours,
            ["handPlacement"] = row.HandPlacement,
            ["serialNumber"] = row.SerialNumber,
            ["content"] = row.Content,
            ["defectLocation"] = row.DefectLocation,
            ["prodDate"] = row.ProdDate?.ToString("yyyy-MM-dd"),
            ["cardCode"] = row.CardCode,
            ["defectSymptom"] = row.DefectSymptom,
            ["defectEngineering"] = row.DefectEngineering,
            ["defectReason"] = row.DefectReason,
            ["defectResponsibility"] = row.DefectResponsibility,
            ["defectNature"] = row.DefectNature,
            ["repairOperatorName"] = row.RepairOperatorName,
            ["prodOrderType"] = row.ProdOrderType,
            ["remark"] = row.Remark,
            ["lineNumber"] = row.LineNumber,
            ["detailTeamCode"] = row.DetailTeamCode,
        };
    }

    /// <summary>
    /// 检查日期：期间内的实装日期，T 面优先
    /// </summary>
    private static DateTime? ResolveInspectionDate(DateTime? tSide, DateTime? bSide, DateTime start, DateTime endExclusive)
    {
        if (tSide >= start && tSide < endExclusive)
        {
            return tSide;
        }
        if (bSide >= start && bSide < endExclusive)
        {
            return bSide;
        }
        return tSide ?? bSide;
    }

    /// <summary>
    /// 可空文本补成空串，避免前端空值
    /// </summary>
    private static void FillBlank(TaktDefectOrderStatRowDto row)
    {
        row.PcbaBoardType ??= string.Empty;
        row.VisualInspectionLine ??= string.Empty;
        row.AoiLine ??= string.Empty;
        row.InspectorName ??= string.Empty;
        row.BatchCode ??= string.Empty;
        row.TeamCode ??= string.Empty;
        row.HandPlacement ??= string.Empty;
        row.SerialNumber ??= string.Empty;
        row.Content ??= string.Empty;
        row.DefectLocation ??= string.Empty;
        row.CardCode ??= string.Empty;
        row.DefectSymptom ??= string.Empty;
        row.DefectEngineering ??= string.Empty;
        row.DefectReason ??= string.Empty;
        row.DefectResponsibility ??= string.Empty;
        row.DefectNature ??= string.Empty;
        row.RepairOperatorName ??= string.Empty;
        row.ProdOrderType ??= string.Empty;
        row.Remark ??= string.Empty;
        row.DetailTeamCode ??= string.Empty;
        row.MaterialCode ??= string.Empty;
    }

    /// <summary>
    /// 空白转 null
    /// </summary>
    private static string? BlankToNull(string? value)
    {
        var text = value?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

}

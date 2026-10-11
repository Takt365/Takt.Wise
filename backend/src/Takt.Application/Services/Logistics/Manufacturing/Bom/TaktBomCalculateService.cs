// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktBomCalculateService.cs
// 创建时间：2026-08-14
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 计算应用服务（计算成本 / 重算成本 / 计算平均成本 / 回填采购价）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// BOM 计算服务（计算成本 / 重算成本 / 计算平均成本；与成本分析分离）
/// </summary>
public class TaktBomCalculateService : TaktServiceBase, ITaktBomCalculateService
{
    /// <summary>
    /// 明细年表基表名（按年分表时拼接年份）
    /// </summary>
    private const string BomItemYearShardBaseTable = "takt_logistics_manufacturing_bom_material_cost_item";
    /// <summary>
    /// 与 TaktCompanyEntityBase.ExtField nvarchar(4000) 对齐
    /// </summary>
    private const int BomMaterialCostExtFieldMaxLength = 4000;
    /// <summary>
    /// 旧成本 JSON 键格式：核算日，如 2026/6/30
    /// </summary>
    private const string OldCostDateFormat = "yyyy/M/d";
    /// <summary>
    /// BOM 成本明细仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktBomMaterialCostItem> _bomMaterialCostItemRepository;
    /// <summary>
    /// BOM 成本汇总仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktBomMaterialCost> _bomMaterialCostRepository;
    /// <summary>
    /// 公司仓储（RelatedPlant）
    /// </summary>
    private readonly ITaktTenantRepository<TaktCompany> _companyRepository;
    /// <summary>
    /// 采购价格主表仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecord> _purchaseInfoRecordRepository;
    /// <summary>
    /// 采购价格条件行仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordItem> _purchaseInfoRecordItemRepository;
    /// <summary>
    /// 采购阶梯数量仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordScaleQuantity> _purchaseInfoRecordScaleQuantityRepository;
    /// <summary>
    /// 采购阶梯价格仓储
    /// </summary>
    private readonly ITaktCompanyRepository<TaktPurchaseInfoRecordScaleValue> _purchaseInfoRecordScaleValueRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialCostItemRepository">BOM 成本明细仓储</param>
    /// <param name="bomMaterialCostRepository">BOM 成本汇总仓储</param>
    /// <param name="companyRepository">公司仓储（RelatedPlant）</param>
    /// <param name="purchaseInfoRecordRepository">采购价格主表仓储</param>
    /// <param name="purchaseInfoRecordItemRepository">采购价格条件行仓储</param>
    /// <param name="purchaseInfoRecordScaleQuantityRepository">采购阶梯数量仓储</param>
    /// <param name="purchaseInfoRecordScaleValueRepository">采购阶梯价格仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBomCalculateService(
        ITaktCompanyRepository<TaktBomMaterialCostItem> bomMaterialCostItemRepository,
        ITaktCompanyRepository<TaktBomMaterialCost> bomMaterialCostRepository,
        ITaktTenantRepository<TaktCompany> companyRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecord> purchaseInfoRecordRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecordItem> purchaseInfoRecordItemRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecordScaleQuantity> purchaseInfoRecordScaleQuantityRepository,
        ITaktCompanyRepository<TaktPurchaseInfoRecordScaleValue> purchaseInfoRecordScaleValueRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _bomMaterialCostItemRepository = bomMaterialCostItemRepository;
        _bomMaterialCostRepository = bomMaterialCostRepository;
        _companyRepository = companyRepository;
        _purchaseInfoRecordRepository = purchaseInfoRecordRepository;
        _purchaseInfoRecordItemRepository = purchaseInfoRecordItemRepository;
        _purchaseInfoRecordScaleQuantityRepository = purchaseInfoRecordScaleQuantityRepository;
        _purchaseInfoRecordScaleValueRepository = purchaseInfoRecordScaleValueRepository;
    }

    /// <summary>
    /// 规范化计算查询为单个核算月（供后台任务与入口共用）
    /// </summary>
    /// <param name="queryDto">计算/重算查询（须含核算日起止；起止须同月）</param>
    /// <returns>规范化查询与核算月份标签</returns>
    public static TaktBomCalculatePreparedQueryDto PrepareBomCalculateQuery(TaktBomCalculateQueryDto queryDto)
    {
        queryDto ??= new TaktBomCalculateQueryDto();
        var normalized = new TaktBomCalculateQueryDto
        {
            PlantCode = queryDto.PlantCode,
            MaterialType = queryDto.MaterialType,
            ModelCode = queryDto.ModelCode,
            ProductCode = queryDto.ProductCode,
            ProductCodes = queryDto.ProductCodes,
            CostingDateStart = queryDto.CostingDateStart,
            CostingDateEnd = queryDto.CostingDateEnd,
            ProcessRecordCount = queryDto.ProcessRecordCount,
        };
        if (!normalized.CostingDateStart.HasValue || !normalized.CostingDateEnd.HasValue)
        {
            throw new TaktBusinessException("请选择核算月份后再计算");
        }
        var startMonth = new DateTime(
            normalized.CostingDateStart.Value.Year,
            normalized.CostingDateStart.Value.Month,
            1);
        var endMonth = new DateTime(
            normalized.CostingDateEnd.Value.Year,
            normalized.CostingDateEnd.Value.Month,
            1);
        if (startMonth != endMonth)
        {
            throw new TaktBusinessException("计算仅支持单个核算月份，请缩小日期范围");
        }
        var lastDay = DateTime.DaysInMonth(startMonth.Year, startMonth.Month);
        normalized.CostingDateStart = startMonth;
        normalized.CostingDateEnd = new DateTime(startMonth.Year, startMonth.Month, lastDay, 23, 59, 59, 999);
        return new TaktBomCalculatePreparedQueryDto
        {
            Query = normalized,
            ProcessedMonth = $"{startMonth.Year:D4}-{startMonth.Month:D2}",
        };
    }

    /// <summary>
    /// 查询栏工厂选项：当前公司 RelatedPlant ∩ 成本主表 PlantCode
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBomCalculatePlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var companies = await _companyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode);
        var relatedPlant = companies
            .Select(c => c.RelatedPlant?.Trim() ?? string.Empty)
            .FirstOrDefault(p => !string.IsNullOrEmpty(p))
            ?? string.Empty;
        if (string.IsNullOrEmpty(relatedPlant))
        {
            return new List<TaktSelectOption>();
        }
        var costPlants = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == relatedPlant);
        if (costPlants.Count == 0)
        {
            return new List<TaktSelectOption>();
        }
        return new List<TaktSelectOption>
        {
            new()
            {
                DictValue = relatedPlant,
                DictLabel = relatedPlant,
            },
        };
    }

    /// <summary>
    /// 计算成本：明细按工厂+产品+核算月合计写入主表（有则更新；明细有而主表无则回填新建；按查询所选物料类型，空=全部类型），再刷新机种月均
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>合计统计</returns>
    public Task<TaktBomCalculateCostResultDto> SumBomCalculateCostAsync(TaktBomCalculateQueryDto queryDto)
    {
        return ExecuteBomCalculateCostAsync(queryDto, forceRecalculate: false);
    }

    /// <summary>
    /// 重算成本：将主表旧产品月计算追加到 ExtField JSON（核算日 yyyy/M/d → 计算值）后按明细重写（有则更新；明细有而主表无则回填新建；按查询所选物料类型，空=全部类型），再刷新机种月均
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>重算统计</returns>
    public Task<TaktBomCalculateCostResultDto> RecalculateBomCalculateCostAsync(TaktBomCalculateQueryDto queryDto)
    {
        return ExecuteBomCalculateCostAsync(queryDto, forceRecalculate: true);
    }

    /// <summary>
    /// 计算最近采购成本：与产品月计算同一快照口径，行金额=组件数量×(净价÷采购价格单位)，写入主表 LatestPurchaseCost
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>合计统计</returns>
    public Task<TaktBomCalculateCostResultDto> SumBomCalculateLatestPurchaseCostAsync(
        TaktBomCalculateQueryDto queryDto)
    {
        return ExecuteBomCalculateCostAsync(
            queryDto,
            forceRecalculate: false,
            writeLatestPurchaseCost: true);
    }

    /// <summary>
    /// 按组件编码+核算日回填 BOM 明细采购组织（主表工厂编码）、采购组、供应商、净价、采购货币、采购价格单位
    /// </summary>
    /// <param name="queryDto">工厂/物料类型/机种可选；须单个核算月</param>
    /// <returns>回填统计</returns>
    public async Task<TaktBomCalculatePurchaseInfoRecordBackfillResultDto> BackfillBomCalculatePurchaseInfoRecordAsync(
        TaktBomCalculateQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(queryDto);
        if (queryDto.ProcessRecordCount < 0)
        {
            throw new TaktBusinessException("处理记录数不能为负数（0 表示全部）");
        }
        var prepared = PrepareBomCalculateQuery(queryDto);
        await ApplyModelCodeProductScopeAsync(prepared.Query);
        var itemRows = await LoadItemsForCalculateQueryAsync(prepared.Query);
        var scopedRows = await FilterBomCalculateItemRowsForPurchaseBackfillAsync(
            itemRows,
            prepared.Query);
        var yearTable = await ResolveBomItemPhysicalTableAsync(prepared.Query.CostingDateStart!.Value.Year);
        var lookup = await LoadPurchaseInfoRecordLookupAsync(scopedRows);
        var updated = 0;
        var skippedNoPrice = 0;
        var unchanged = 0;
        foreach (var row in scopedRows)
        {
            if (string.IsNullOrWhiteSpace(row.ComponentCode))
            {
                skippedNoPrice = checked(skippedNoPrice + 1);
                continue;
            }
            var headers = lookup.FindHeaders(row.ComponentCode);
            var header = TaktBomCalculatePurchaseInfoRecordHelper.ResolveNearestHeader(headers, row.CostingDate);
            if (header == null)
            {
                skippedNoPrice = checked(skippedNoPrice + 1);
                continue;
            }
            var item = TaktBomCalculatePurchaseInfoRecordHelper.ResolveActiveItem(lookup.FindItems(header.Id));
            if (item == null)
            {
                skippedNoPrice = checked(skippedNoPrice + 1);
                continue;
            }
            var netPrice = TaktBomCalculatePurchaseInfoRecordHelper.ResolveNetPrice(
                item,
                lookup.FindQuantityScales(item.Id),
                lookup.FindValueScales(item.Id),
                row.ComponentQuantity);
            if (!TaktBomCalculatePurchaseInfoRecordHelper.ApplyPurchaseFields(row, header, item, netPrice))
            {
                unchanged = checked(unchanged + 1);
                continue;
            }
            await _bomMaterialCostItemRepository.UpdateAsync(row, yearTable);
            updated = checked(updated + 1);
        }
        return new TaktBomCalculatePurchaseInfoRecordBackfillResultDto
        {
            ScannedRowCount = scopedRows.Count,
            UpdatedRowCount = updated,
            SkippedNoPriceCount = skippedNoPrice,
            UnchangedRowCount = unchanged,
            ProcessedMonth = prepared.ProcessedMonth,
        };
    }

    /// <summary>
    /// Quartz 计算成本：判定日所在自然月（不传工厂；按明细表行内 PlantCode 分组）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>合计统计；当月无明细时仍返回统计（扫描 0）</returns>
    public async Task<TaktBomCalculateCostResultDto?> RunScheduledBomCalculateSumAsync(DateTime? asOfDate = null)
    {
        return await ExecuteBomCalculateCostAsync(
            BuildScheduledCurrentMonthQuery(asOfDate),
            forceRecalculate: false);
    }

    /// <summary>
    /// Quartz 重算成本：判定日所在自然月（不传工厂；按明细表行内 PlantCode 分组）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>重算统计</returns>
    public async Task<TaktBomCalculateCostResultDto?> RunScheduledBomCalculateRecalculateAsync(DateTime? asOfDate = null)
    {
        return await ExecuteBomCalculateCostAsync(
            BuildScheduledCurrentMonthQuery(asOfDate),
            forceRecalculate: true);
    }

    /// <summary>
    /// 计算平均成本：按主表已有机种/物料类型分组重算机种月均（空机种/空类型由 QT_SYNC_BV_BK 回填，此处不写）
    /// （固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；比较后有变化才落库并写 ExtField；忽略查询栏 MaterialType）
    /// </summary>
    /// <param name="queryDto">工厂 + 核算期间；机种可选；MaterialType 忽略</param>
    /// <returns>平均结果</returns>
    public async Task<TaktBomCalculateAverageResultDto> CalculateBomCalculateAverageAsync(
        TaktBomCalculateAverageQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(queryDto);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.PlantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(queryDto.CostingPeriod);
        var plant = queryDto.PlantCode.Trim();
        var periodKey = queryDto.CostingPeriod.Trim();
        var modelFilters = ParseModelCodes(null, queryDto.ModelCode);
        // 平均成本固定扫该工厂+期间下全部 MaterialType（ROH/HALB/FERT/空类型等），不按查询栏类型过滤
        var headerExp = Expressionable.Create<TaktBomMaterialCost>();
        headerExp = headerExp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.PlantCode == plant
            && x.CostingPeriod == periodKey);
        if (modelFilters.Count == 1)
        {
            var model = modelFilters[0];
            headerExp = headerExp.And(x => x.ModelCode == model);
        }
        else if (modelFilters.Count > 1)
        {
            var models = modelFilters.ToList();
            headerExp = headerExp.And(x => models.Contains(x.ModelCode));
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(headerExp.ToExpression(), includeSoftDeleted: true);
        if (modelFilters.Count > 1)
        {
            var modelSet = new HashSet<string>(modelFilters, StringComparer.OrdinalIgnoreCase);
            headers = headers
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        var modelCodeUpdated = 0;
        var materialTypeUpdated = 0;
        var groups = headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode))
            .GroupBy(h => BuildModelAverageGroupKey(h.MaterialType, h.ModelCode), StringComparer.OrdinalIgnoreCase)
            .Where(g => !string.IsNullOrWhiteSpace(g.Key))
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var averageUpdated = 0;
        var groupsWithProductCost = 0;
        var groupsWithoutProductCost = 0;
        var positiveProductCostRows = headers.Count(h =>
            TaktBomMaterialCostItemModelEnrichmentHelper.ResolveProductAmountForModelAverage(h) > 0m);
        foreach (var grp in groups)
        {
            var list = grp.ToList();
            if (list.Exists(h =>
                TaktBomMaterialCostItemModelEnrichmentHelper.ResolveProductAmountForModelAverage(h) > 0m))
            {
                groupsWithProductCost = checked(groupsWithProductCost + 1);
            }
            else
            {
                groupsWithoutProductCost = checked(groupsWithoutProductCost + 1);
            }
            averageUpdated = checked(averageUpdated + await ApplyModelMonthlyAverageAndSaveAsync(list));
        }
        return new TaktBomCalculateAverageResultDto
        {
            ScannedRowCount = headers.Count,
            ModelCodeUpdatedCount = modelCodeUpdated,
            MaterialTypeUpdatedCount = materialTypeUpdated,
            AverageUpdatedCount = averageUpdated,
            ModelGroupCount = groups.Count,
            PositiveProductCostRowCount = positiveProductCostRows,
            GroupsWithProductCostCount = groupsWithProductCost,
            GroupsWithoutProductCostCount = groupsWithoutProductCost,
            CostingPeriod = periodKey,
        };
    }

    /// <summary>
    /// Quartz 计算平均成本：判定日所在自然月（按主表行 PlantCode 去重工厂）
    /// </summary>
    /// <param name="asOfDate">判定日；默认今天</param>
    /// <returns>各工厂汇总；当月无主表行时返回 null</returns>
    public async Task<TaktBomCalculateAverageResultDto?> RunScheduledBomCalculateAverageAsync(DateTime? asOfDate = null)
    {
        EnsureThreeLayerContext();
        var asOf = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(asOfDate ?? DateTime.Today);
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(asOf);
        var periodHeaders = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.CostingPeriod == periodKey,
            includeSoftDeleted: true);
        if (periodHeaders.Count == 0)
        {
            return null;
        }
        var plantCodes = periodHeaders
            .Select(x => x.PlantCode?.Trim() ?? string.Empty)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (plantCodes.Count == 0)
        {
            return null;
        }
        var scanned = 0;
        var modelUpdated = 0;
        var typeUpdated = 0;
        var averageUpdated = 0;
        var modelGroups = 0;
        var positiveProductCostRows = 0;
        var groupsWithProductCost = 0;
        var groupsWithoutProductCost = 0;
        foreach (var plant in plantCodes)
        {
            var result = await CalculateBomCalculateAverageAsync(
                new TaktBomCalculateAverageQueryDto
                {
                    PlantCode = plant,
                    CostingPeriod = periodKey,
                });
            scanned = checked(scanned + result.ScannedRowCount);
            modelUpdated = checked(modelUpdated + result.ModelCodeUpdatedCount);
            typeUpdated = checked(typeUpdated + result.MaterialTypeUpdatedCount);
            averageUpdated = checked(averageUpdated + result.AverageUpdatedCount);
            modelGroups = checked(modelGroups + result.ModelGroupCount);
            positiveProductCostRows = checked(positiveProductCostRows + result.PositiveProductCostRowCount);
            groupsWithProductCost = checked(groupsWithProductCost + result.GroupsWithProductCostCount);
            groupsWithoutProductCost = checked(groupsWithoutProductCost + result.GroupsWithoutProductCostCount);
        }
        return new TaktBomCalculateAverageResultDto
        {
            ScannedRowCount = scanned,
            ModelCodeUpdatedCount = modelUpdated,
            MaterialTypeUpdatedCount = typeUpdated,
            AverageUpdatedCount = averageUpdated,
            ModelGroupCount = modelGroups,
            PositiveProductCostRowCount = positiveProductCostRows,
            GroupsWithProductCostCount = groupsWithProductCost,
            GroupsWithoutProductCostCount = groupsWithoutProductCost,
            CostingPeriod = periodKey,
        };
    }

    /// <summary>
    /// 计算/重算成本或最近采购成本核心（按查询所选物料类型过滤，空=全部类型；产品月计算路径再刷新机种月均）
    /// </summary>
    /// <param name="queryDto">计算/重算查询（工厂/物料类型/机种可选；须单个核算月）</param>
    /// <param name="forceRecalculate">为 true 时先把旧产品月计算写入 ExtField，ResetGroupCount 计入已同步组</param>
    /// <param name="writeLatestPurchaseCost">为 true 时只写最近采购成本，不改产品月计算、不刷新机种月均</param>
    /// <returns>统计</returns>
    private async Task<TaktBomCalculateCostResultDto> ExecuteBomCalculateCostAsync(
        TaktBomCalculateQueryDto queryDto,
        bool forceRecalculate,
        bool writeLatestPurchaseCost = false)
    {
        EnsureThreeLayerContext();
        ArgumentNullException.ThrowIfNull(queryDto);
        if (queryDto.ProcessRecordCount < 0)
        {
            throw new TaktBusinessException("处理记录数不能为负数（0 表示全部）");
        }
        var prepared = PrepareBomCalculateQuery(queryDto);
        await ApplyModelCodeProductScopeAsync(prepared.Query);
        var periodKey = prepared.ProcessedMonth;
        var modelFilterSet = new HashSet<string>(
            ParseModelCodes(null, prepared.Query.ModelCode),
            StringComparer.OrdinalIgnoreCase);
        var filterMaterialType = string.IsNullOrWhiteSpace(prepared.Query.MaterialType)
            ? null
            : prepared.Query.MaterialType.Trim();
        var processRecordCount = prepared.Query.ProcessRecordCount;
        var itemRows = await LoadItemsForCalculateQueryAsync(prepared.Query);
        TaktLogger.Information(
            "[BomMaterialCost] 明细已加载 Month={Month} Plant={Plant} Rows={Rows} Tenant={Tenant} Company={Company}",
            periodKey,
            prepared.Query.PlantCode ?? string.Empty,
            itemRows.Count,
            CurrentTenantCode,
            CurrentCompanyCode);
        var groupedKeys = itemRows
            .Where(r => !string.IsNullOrWhiteSpace(r.PlantCode) && !string.IsNullOrWhiteSpace(r.ProductCode))
            .GroupBy(
                r =>
                {
                    var plant = r.PlantCode!.Trim();
                    var product = r.ProductCode!.Trim();
                    var period = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(r.CostingDate);
                    return $"{plant}|{product}|{period}";
                },
                StringComparer.OrdinalIgnoreCase)
            .Select(g =>
            {
                var latest = g.OrderByDescending(x => x.CostingDate).First();
                return (
                    PlantCode: latest.PlantCode!.Trim(),
                    ProductCode: latest.ProductCode!.Trim(),
                    CostingDate: latest.CostingDate,
                    Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(latest.CostingDate));
            })
            .Where(k => string.Equals(k.Period, periodKey, StringComparison.Ordinal))
            .OrderBy(k => k.PlantCode, StringComparer.OrdinalIgnoreCase)
            .ThenBy(k => k.ProductCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
        var plantCodes = groupedKeys
            .Select(k => k.PlantCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var headerScope = await LoadBomHeaderScopeLookupAsync(periodKey, plantCodes);
        var syncKeys = new List<(string PlantCode, string ProductCode, DateTime CostingDate)>();
        var skippedCount = 0;
        foreach (var key in groupedKeys)
        {
            var (headerType, headerModel) = ResolveBomHeaderScope(headerScope, key.PlantCode, key.ProductCode);
            if (!MatchesSelectedMaterialType(headerType, filterMaterialType))
            {
                skippedCount = checked(skippedCount + 1);
                continue;
            }
            if (modelFilterSet.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(headerModel) || !modelFilterSet.Contains(headerModel))
                {
                    skippedCount = checked(skippedCount + 1);
                    continue;
                }
            }
            syncKeys.Add((key.PlantCode, key.ProductCode, key.CostingDate));
        }
        var totalMatchedGroupCount = syncKeys.Count;
        if (processRecordCount > 0 && syncKeys.Count > processRecordCount)
        {
            syncKeys = syncKeys.Take(processRecordCount).ToList();
        }
        if (syncKeys.Count > 0)
        {
            if (writeLatestPurchaseCost)
            {
                await SyncLatestPurchaseCostFromItemsBatchAsync(
                    syncKeys,
                    Array.Empty<TaktModelDestination>(),
                    Array.Empty<TaktMaterialPlant>(),
                    Array.Empty<TaktGeneralMaterial>(),
                    filterMaterialType);
            }
            else
            {
                await SyncBomMaterialCostFromItemsBatchAsync(
                    syncKeys,
                    Array.Empty<TaktModelDestination>(),
                    Array.Empty<TaktMaterialPlant>(),
                    Array.Empty<TaktGeneralMaterial>(),
                    archiveOldCost: forceRecalculate,
                    filterMaterialType);
            }
        }
        return new TaktBomCalculateCostResultDto
        {
            ScannedRowCount = itemRows.Count,
            RefreshedGroupCount = syncKeys.Count,
            SkippedGroupCount = skippedCount + Math.Max(0, totalMatchedGroupCount - syncKeys.Count),
            ResetGroupCount = forceRecalculate && !writeLatestPurchaseCost ? syncKeys.Count : 0,
            ProcessedMonthCount = 1,
            ProcessedMonth = prepared.ProcessedMonth,
        };
    }

    /// <summary>
    /// Quartz：构造判定日所在自然月的计算查询（不带物料类型）
    /// </summary>
    /// <param name="asOfDate">判定日；空则今天</param>
    /// <returns>查询</returns>
    private TaktBomCalculateQueryDto BuildScheduledCurrentMonthQuery(DateTime? asOfDate)
    {
        EnsureThreeLayerContext();
        var asOf = TaktBomMaterialCostItemLineCostHelper.NormalizeCostingDate(asOfDate ?? DateTime.Today);
        var costingStart = new DateTime(asOf.Year, asOf.Month, 1);
        var costingEnd = new DateTime(
            asOf.Year,
            asOf.Month,
            DateTime.DaysInMonth(asOf.Year, asOf.Month),
            23,
            59,
            59,
            999);
        return new TaktBomCalculateQueryDto
        {
            CostingDateStart = costingStart,
            CostingDateEnd = costingEnd,
            ProcessRecordCount = 0,
        };
    }

    /// <summary>
    /// 仅选机种未选产品时，把该工厂该机种下产品编码写入 ProductCodes
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>任务</returns>
    private async Task ApplyModelCodeProductScopeAsync(TaktBomCalculateQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        if (string.IsNullOrWhiteSpace(queryDto.ModelCode)
            || !string.IsNullOrWhiteSpace(queryDto.ProductCode)
            || (queryDto.ProductCodes != null && queryDto.ProductCodes.Count > 0))
        {
            return;
        }
        var plantCode = queryDto.PlantCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(plantCode))
        {
            return;
        }
        var modelCodes = ParseModelCodes(null, queryDto.ModelCode);
        if (modelCodes.Count == 0)
        {
            return;
        }
        var productCodes = await LoadModelProductCodesAsync(plantCode, modelCodes);
        queryDto.ProductCodes = productCodes.Count > 0
            ? productCodes
            : new List<string> { "__no_model_product__" };
    }

    /// <summary>
    /// 按工厂+机种（可多选）从主表取产品编码列表
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="modelCodes">机种列表</param>
    /// <returns>产品编码</returns>
    private async Task<List<string>> LoadModelProductCodesAsync(
        string plantCode,
        IReadOnlyList<string> modelCodes)
    {
        if (modelCodes.Count == 0)
        {
            return new List<string>();
        }
        List<TaktBomMaterialCost> headers;
        if (modelCodes.Count == 1)
        {
            var modelCode = modelCodes[0];
            headers = await _bomMaterialCostRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && x.ModelCode == modelCode,
                includeSoftDeleted: true);
        }
        else
        {
            var models = modelCodes.ToList();
            headers = await _bomMaterialCostRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plantCode
                    && models.Contains(x.ModelCode),
                includeSoftDeleted: true);
            var modelSet = new HashSet<string>(modelCodes, StringComparer.OrdinalIgnoreCase);
            headers = headers
                .Where(h => !string.IsNullOrWhiteSpace(h.ModelCode)
                    && modelSet.Contains(h.ModelCode.Trim()))
                .ToList();
        }
        return headers
            .Where(h => !string.IsNullOrWhiteSpace(h.ProductCode))
            .Select(h => h.ProductCode.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// 按核算月加载明细（可按工厂/产品过滤；明细无软删业务）
    /// </summary>
    /// <param name="query">查询</param>
    /// <returns>明细行</returns>
    private async Task<List<TaktBomMaterialCostItem>> LoadItemsForCalculateQueryAsync(TaktBomCalculateQueryDto query)
    {
        ArgumentNullException.ThrowIfNull(query);
        var start = query.CostingDateStart
            ?? throw new TaktBusinessException("请选择核算月份后再计算");
        var end = query.CostingDateEnd
            ?? throw new TaktBusinessException("请选择核算月份后再计算");
        var yearTable = await ResolveBomItemPhysicalTableAsync(start.Year);
        var exp = Expressionable.Create<TaktBomMaterialCostItem>();
        exp = exp.And(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.CostingDate >= start
            && x.CostingDate <= end);
        if (!string.IsNullOrWhiteSpace(query.PlantCode))
        {
            var plant = query.PlantCode.Trim();
            exp = exp.And(x => x.PlantCode == plant);
        }
        if (!string.IsNullOrWhiteSpace(query.ProductCode))
        {
            var product = query.ProductCode.Trim();
            exp = exp.And(x => x.ProductCode == product);
        }
        else if (query.ProductCodes != null && query.ProductCodes.Count > 0)
        {
            var codes = query.ProductCodes
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (codes.Count > 0)
            {
                exp = exp.And(x => codes.Contains(x.ProductCode));
            }
        }
        // 明细无软删业务（IsDeleted 恒为 0）；读隔离默认即可
        return await _bomMaterialCostItemRepository.GetListAsync(exp.ToExpression(), yearTable);
    }

    /// <summary>
    /// 按工厂+产品+核算月批量同步主表（同工厂同月仅加载一次明细与主表缓存；机种月均去重后刷新）
    /// </summary>
    /// <param name="keys">待同步键</param>
    /// <param name="destinations">型号目的地</param>
    /// <param name="materialPlants">工厂物料</param>
    /// <param name="generalMaterials">通用物料</param>
    /// <param name="archiveOldCost">为 true 时将旧产品月计算写入 ExtField</param>
    /// <param name="filterMaterialType">所选物料类型；空=不按类型改写</param>
    /// <returns>任务</returns>
    private async Task SyncBomMaterialCostFromItemsBatchAsync(
        IEnumerable<(string PlantCode, string ProductCode, DateTime CostingDate)> keys,
        IReadOnlyList<TaktModelDestination>? destinations,
        IReadOnlyList<TaktMaterialPlant>? materialPlants,
        IReadOnlyList<TaktGeneralMaterial>? generalMaterials,
        bool archiveOldCost,
        string? filterMaterialType)
    {
        ArgumentNullException.ThrowIfNull(keys);
        var distinct = keys
            .Where(k => !string.IsNullOrWhiteSpace(k.PlantCode) && !string.IsNullOrWhiteSpace(k.ProductCode))
            .Select(k => (
                PlantCode: k.PlantCode.Trim(),
                ProductCode: k.ProductCode.Trim(),
                Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(k.CostingDate),
                CostingDate: k.CostingDate))
            .GroupBy(k => $"{k.PlantCode}|{k.ProductCode}|{k.Period}", StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.CostingDate).First())
            .ToList();
        var destList = destinations ?? Array.Empty<TaktModelDestination>();
        var generalList = generalMaterials ?? Array.Empty<TaktGeneralMaterial>();
        foreach (var plantPeriodGroup in distinct.GroupBy(
                     k => $"{k.PlantCode}|{k.Period}",
                     StringComparer.OrdinalIgnoreCase))
        {
            var sample = plantPeriodGroup.First();
            var plant = sample.PlantCode;
            var periodKey = sample.Period;
            var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
            var yearTable = await ResolveBomItemPhysicalTableAsync(monthStart.Year);
            var monthItems = await _bomMaterialCostItemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.CostingDate >= monthStart
                    && x.CostingDate <= monthEnd
                    && x.ProductCode != null,
                yearTable);
            var headerCache = await _bomMaterialCostRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.CostingPeriod == periodKey
                    && x.ProductCode != null,
                includeSoftDeleted: true);
            var averageKeys = new HashSet<(string MaterialType, string ModelCode)>();
            var done = 0;
            var total = plantPeriodGroup.Count();
            TaktLogger.Information(
                "[BomMaterialCost] 合计批次开始 Plant={Plant} Period={Period} Products={Products} ItemRows={ItemRows} Headers={Headers}",
                plant,
                periodKey,
                total,
                monthItems.Count,
                headerCache.Count);
            foreach (var key in plantPeriodGroup)
            {
                var product = TaktStringHelper.NormalizeSapNumericMaterialCode(key.ProductCode);
                if (string.IsNullOrWhiteSpace(product))
                {
                    product = key.ProductCode;
                }
                var productItems = monthItems
                    .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
                    .ToList();
                var avgKey = await SyncBomMaterialCostFromItemsAsync(
                    plant,
                    product,
                    key.CostingDate,
                    destList,
                    materialPlants,
                    generalList,
                    archiveOldCost,
                    filterMaterialType,
                    productItems,
                    headerCache,
                    skipAverageRefresh: true);
                if (avgKey.HasValue)
                {
                    averageKeys.Add(avgKey.Value);
                }
                done = checked(done + 1);
                if (done == 1 || done == total || done % 50 == 0)
                {
                    TaktLogger.Information(
                        "[BomMaterialCost] 合计进度 Plant={Plant} Period={Period} Done={Done}/{Total}",
                        plant,
                        periodKey,
                        done,
                        total);
                }
            }
            foreach (var (materialType, modelCode) in averageKeys)
            {
                await RefreshModelMonthlyAverageForPeriodAsync(plant, materialType, modelCode, periodKey);
            }
            TaktLogger.Information(
                "[BomMaterialCost] 合计批次完成 Plant={Plant} Period={Period} Products={Products} ModelAvgGroups={AvgGroups}",
                plant,
                periodKey,
                total,
                averageKeys.Count);
        }
    }

    /// <summary>
    /// 按工厂+产品+核算月批量写入最近采购成本
    /// </summary>
    /// <param name="keys">待同步键</param>
    /// <param name="destinations">型号目的地</param>
    /// <param name="materialPlants">工厂物料</param>
    /// <param name="generalMaterials">通用物料</param>
    /// <param name="filterMaterialType">所选物料类型；空=不按类型改写</param>
    /// <returns>任务</returns>
    private async Task SyncLatestPurchaseCostFromItemsBatchAsync(
        IEnumerable<(string PlantCode, string ProductCode, DateTime CostingDate)> keys,
        IReadOnlyList<TaktModelDestination>? destinations,
        IReadOnlyList<TaktMaterialPlant>? materialPlants,
        IReadOnlyList<TaktGeneralMaterial>? generalMaterials,
        string? filterMaterialType)
    {
        ArgumentNullException.ThrowIfNull(keys);
        var distinct = keys
            .Where(k => !string.IsNullOrWhiteSpace(k.PlantCode) && !string.IsNullOrWhiteSpace(k.ProductCode))
            .Select(k => (
                PlantCode: k.PlantCode.Trim(),
                ProductCode: k.ProductCode.Trim(),
                Period: TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(k.CostingDate),
                CostingDate: k.CostingDate))
            .GroupBy(k => $"{k.PlantCode}|{k.ProductCode}|{k.Period}", StringComparer.OrdinalIgnoreCase)
            .Select(g => g.OrderByDescending(x => x.CostingDate).First())
            .ToList();
        var destList = destinations ?? Array.Empty<TaktModelDestination>();
        var generalList = generalMaterials ?? Array.Empty<TaktGeneralMaterial>();
        foreach (var plantPeriodGroup in distinct.GroupBy(
                     k => $"{k.PlantCode}|{k.Period}",
                     StringComparer.OrdinalIgnoreCase))
        {
            var sample = plantPeriodGroup.First();
            var plant = sample.PlantCode;
            var periodKey = sample.Period;
            var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
            var yearTable = await ResolveBomItemPhysicalTableAsync(monthStart.Year);
            var monthItems = await _bomMaterialCostItemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.CostingDate >= monthStart
                    && x.CostingDate <= monthEnd
                    && x.ProductCode != null,
                yearTable);
            var headerCache = await _bomMaterialCostRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && x.PlantCode == plant
                    && x.CostingPeriod == periodKey
                    && x.ProductCode != null,
                includeSoftDeleted: true);
            foreach (var key in plantPeriodGroup)
            {
                var product = TaktStringHelper.NormalizeSapNumericMaterialCode(key.ProductCode);
                if (string.IsNullOrWhiteSpace(product))
                {
                    product = key.ProductCode;
                }
                var productItems = monthItems
                    .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
                    .ToList();
                await SyncLatestPurchaseCostFromItemsAsync(
                    plant,
                    product,
                    key.CostingDate,
                    destList,
                    materialPlants,
                    generalList,
                    filterMaterialType,
                    productItems,
                    headerCache);
            }
        }
    }

    /// <summary>
    /// 按工厂+产品+核算月把明细最近采购成本写入主表（不改产品月计算、不刷新机种月均）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <param name="costingDate">核算日</param>
    /// <param name="destinations">型号目的地</param>
    /// <param name="materialPlants">工厂物料</param>
    /// <param name="generalMaterials">通用物料</param>
    /// <param name="filterMaterialType">所选物料类型；空则用解析结果，解析不到再保留已有值</param>
    /// <param name="preloadedProductItems">已按产品筛好的明细；空则按产品变体查库</param>
    /// <param name="headerCache">同工厂同月主表缓存；空则按产品查库</param>
    /// <returns>任务</returns>
    private async Task SyncLatestPurchaseCostFromItemsAsync(
        string plantCode,
        string productCode,
        DateTime costingDate,
        IReadOnlyList<TaktModelDestination> destinations,
        IReadOnlyList<TaktMaterialPlant>? materialPlants,
        IReadOnlyList<TaktGeneralMaterial>? generalMaterials,
        string? filterMaterialType,
        IReadOnlyList<TaktBomMaterialCostItem>? preloadedProductItems = null,
        List<TaktBomMaterialCost>? headerCache = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentNullException.ThrowIfNull(destinations);
        var plant = plantCode.Trim();
        var product = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode.Trim());
        if (string.IsNullOrWhiteSpace(product))
        {
            product = productCode.Trim();
        }
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(costingDate);
        List<TaktBomMaterialCostItem> productItems;
        if (preloadedProductItems != null)
        {
            productItems = preloadedProductItems.ToList();
        }
        else
        {
            var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
            var yearTable = await ResolveBomItemPhysicalTableAsync(monthStart.Year);
            var variants = TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(product).ToList();
            var monthItems = variants.Count == 0
                ? new List<TaktBomMaterialCostItem>()
                : await _bomMaterialCostItemRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && x.CostingDate >= monthStart
                        && x.CostingDate <= monthEnd
                        && x.ProductCode != null
                        && variants.Contains(x.ProductCode),
                    yearTable);
            productItems = monthItems
                .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
                .ToList();
        }
        // 主表匹配键：工厂+产品+核算日（有明细时取期间最后核算日）
        var matchCostingDate = costingDate;
        if (productItems.Count > 0)
        {
            matchCostingDate = TaktBomMaterialCostItemLineCostHelper.ResolveLatestCostingDate(
                productItems, plant, product, periodKey) ?? costingDate;
        }
        var existing = headerCache != null
            ? FindHeaderInCache(headerCache, plant, product, matchCostingDate)
            : await FindHeaderByProductCostingDateAsync(plant, product, matchCostingDate);
        var modelCode = existing != null && !string.IsNullOrWhiteSpace(existing.ModelCode)
            ? existing.ModelCode.Trim()
            : string.Empty;
        var materialType = existing != null && !string.IsNullOrWhiteSpace(existing.MaterialType)
            ? existing.MaterialType.Trim()
            : string.Empty;
        if (string.IsNullOrWhiteSpace(materialType) && !string.IsNullOrWhiteSpace(filterMaterialType))
        {
            materialType = filterMaterialType.Trim() ?? string.Empty;
        }
        if (productItems.Count == 0)
        {
            if (existing == null)
            {
                return;
            }
            var oldLpc = existing.LatestPurchaseCost;
            existing.LatestPurchaseCost = 0;
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                existing.ModelCode = modelCode;
            }
            if (!string.IsNullOrWhiteSpace(materialType))
            {
                existing.MaterialType = materialType;
            }
            existing.ExtField = AppendLatestPurchaseOperationHistory(
                existing.ExtField,
                oldLpc,
                0m,
                periodKey,
                plant,
                product,
                existing.ModelCode);
            await _bomMaterialCostRepository.UpdateAsync(existing);
            return;
        }
        var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
            productItems, plant, product, periodKey);
        var latestCostingDate = matchCostingDate;
        var latestPurchaseCost = TaktBomMaterialCostItemLineCostHelper.SumSnapshotPurchaseCost(snapshot);
        var productDescription = snapshot.FirstOrDefault()?.ProductDescription
            ?? productItems.OrderByDescending(x => x.CostingDate).First().ProductDescription
            ?? string.Empty;
        var purchaseCurrency = TaktBomMaterialCostItemLineCostHelper
            .FilterBomMaterialCostItemRows(
                TaktBomMaterialCostItemLineCostHelper.ExcludePcbSectHierarchyRows(snapshot))
            .Select(x => x.PurchaseCurrencyCode)
            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? string.Empty;
        // 明细有而主表无：回填新建主表行
        if (existing == null)
        {
            existing = new TaktBomMaterialCost
            {
                PlantCode = plant,
                ModelCode = modelCode,
                MaterialType = materialType,
                ProductCode = product,
                ProductDescription = productDescription,
                ProductMonthlyCost = 0,
                ProductMonthlyCalculation = 0,
                LatestPurchaseCost = latestPurchaseCost,
                ModelMonthlyAverageCost = 0,
                CurrencyCode = purchaseCurrency,
                CostingPeriod = periodKey,
                CostingDate = latestCostingDate.Date,
            };
            existing.ExtField = AppendLatestPurchaseOperationHistory(
                null,
                0m,
                latestPurchaseCost,
                periodKey,
                plant,
                product,
                modelCode);
            existing = await _bomMaterialCostRepository.CreateAsync(existing);
            headerCache?.Add(existing);
            return;
        }
        if (string.IsNullOrWhiteSpace(existing.ModelCode) && !string.IsNullOrWhiteSpace(modelCode))
        {
            existing.ModelCode = modelCode;
        }
        if (string.IsNullOrWhiteSpace(existing.MaterialType) && !string.IsNullOrWhiteSpace(materialType))
        {
            existing.MaterialType = materialType;
        }
        existing.ProductCode = product;
        if (!string.IsNullOrWhiteSpace(productDescription))
        {
            existing.ProductDescription = productDescription;
        }
        var oldLatest = existing.LatestPurchaseCost;
        existing.LatestPurchaseCost = latestPurchaseCost;
        existing.CostingPeriod = periodKey;
        existing.CostingDate = latestCostingDate.Date;
        existing.ExtField = AppendLatestPurchaseOperationHistory(
            existing.ExtField,
            oldLatest,
            latestPurchaseCost,
            periodKey,
            plant,
            product,
            existing.ModelCode);
        await _bomMaterialCostRepository.UpdateAsync(existing);
    }

    /// <summary>
    /// 按工厂+产品+核算月把明细合计写入主表一行（有则更新；明细有而主表无则回填新建）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <param name="costingDate">核算日</param>
    /// <param name="destinations">型号目的地</param>
    /// <param name="materialPlants">工厂物料</param>
    /// <param name="generalMaterials">通用物料</param>
    /// <param name="archiveOldCost">为 true 时将旧产品月计算写入 ExtField</param>
    /// <param name="filterMaterialType">所选物料类型；空则用解析结果，解析不到再保留已有值</param>
    /// <param name="preloadedProductItems">已按产品筛好的明细；空则按产品变体查库</param>
    /// <param name="headerCache">同工厂同月主表缓存（批量路径）；空则按产品查库</param>
    /// <param name="skipAverageRefresh">为 true 时不刷新机种月均（由批量末尾去重刷新）</param>
    /// <returns>需刷新机种月均的 (物料类型, 机种)；无需则 null</returns>
    private async Task<(string MaterialType, string ModelCode)?> SyncBomMaterialCostFromItemsAsync(
        string plantCode,
        string productCode,
        DateTime costingDate,
        IReadOnlyList<TaktModelDestination> destinations,
        IReadOnlyList<TaktMaterialPlant>? materialPlants,
        IReadOnlyList<TaktGeneralMaterial>? generalMaterials,
        bool archiveOldCost,
        string? filterMaterialType,
        IReadOnlyList<TaktBomMaterialCostItem>? preloadedProductItems = null,
        List<TaktBomMaterialCost>? headerCache = null,
        bool skipAverageRefresh = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(productCode);
        ArgumentNullException.ThrowIfNull(destinations);
        var plant = plantCode.Trim();
        var product = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode.Trim());
        if (string.IsNullOrWhiteSpace(product))
        {
            product = productCode.Trim();
        }
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(costingDate);
        List<TaktBomMaterialCostItem> productItems;
        if (preloadedProductItems != null)
        {
            productItems = preloadedProductItems.ToList();
        }
        else
        {
            var (monthStart, monthEnd) = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodDateRange(periodKey);
            var yearTable = await ResolveBomItemPhysicalTableAsync(monthStart.Year);
            var variants = TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(product).ToList();
            var monthItems = variants.Count == 0
                ? new List<TaktBomMaterialCostItem>()
                : await _bomMaterialCostItemRepository.GetListAsync(
                    x => x.TenantCode == CurrentTenantCode
                        && x.CompanyCode == CurrentCompanyCode
                        && x.PlantCode == plant
                        && x.CostingDate >= monthStart
                        && x.CostingDate <= monthEnd
                        && x.ProductCode != null
                        && variants.Contains(x.ProductCode),
                    yearTable);
            productItems = monthItems
                .Where(x => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, product))
                .ToList();
        }
        // 主表匹配键：工厂+产品+核算日（有明细时取期间最后核算日）
        var matchCostingDate = costingDate;
        if (productItems.Count > 0)
        {
            matchCostingDate = TaktBomMaterialCostItemLineCostHelper.ResolveLatestCostingDate(
                productItems, plant, product, periodKey) ?? costingDate;
        }
        var existing = headerCache != null
            ? FindHeaderInCache(headerCache, plant, product, matchCostingDate)
            : await FindHeaderByProductCostingDateAsync(plant, product, matchCostingDate);
        var modelCode = existing != null && !string.IsNullOrWhiteSpace(existing.ModelCode)
            ? existing.ModelCode.Trim()
            : string.Empty;
        var materialType = existing != null && !string.IsNullOrWhiteSpace(existing.MaterialType)
            ? existing.MaterialType.Trim()
            : string.Empty;
        if (string.IsNullOrWhiteSpace(materialType) && !string.IsNullOrWhiteSpace(filterMaterialType))
        {
            materialType = filterMaterialType.Trim();
        }
        if (productItems.Count == 0)
        {
            if (existing == null)
            {
                return null;
            }
            var oldZero = existing.ProductMonthlyCalculation;
            if (archiveOldCost)
            {
                ArchiveOldProductMonthlyCost(existing);
            }
            existing.ProductMonthlyCalculation = 0;
            existing.ModelMonthlyAverageCost = 0;
            existing.CostingDate = matchCostingDate.Date;
            existing.CostingPeriod = periodKey;
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                existing.ModelCode = modelCode;
            }
            existing.MaterialType = materialType;
            existing.ExtField = AppendProductCostOperationHistory(
                existing.ExtField,
                archiveOldCost,
                oldZero,
                0m,
                periodKey,
                plant,
                product,
                existing.ModelCode);
            await _bomMaterialCostRepository.UpdateAsync(existing);
            var zeroType = existing.MaterialType?.Trim() ?? string.Empty;
            var zeroModel = existing.ModelCode?.Trim() ?? string.Empty;
            if (!skipAverageRefresh)
            {
                await RefreshModelMonthlyAverageForPeriodAsync(plant, zeroType, zeroModel, periodKey);
                return null;
            }
            return string.IsNullOrWhiteSpace(zeroType) || string.IsNullOrWhiteSpace(zeroModel)
                ? null
                : (zeroType, zeroModel);
        }
        var snapshot = TaktBomMaterialCostItemLineCostHelper.ResolvePeriodSnapshot(
            productItems, plant, product, periodKey);
        var latestCostingDate = matchCostingDate;
        var productMonthlyCost = TaktBomMaterialCostItemLineCostHelper.SumSnapshotCost(snapshot);
        var currency = snapshot
            .Select(TaktBomMaterialCostItemLineCostHelper.ResolveCurrency)
            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? productItems
                .Select(x => x.MovingPriceCurrencyCode)
                .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? string.Empty;
        var productDescription = snapshot.FirstOrDefault()?.ProductDescription
            ?? productItems.OrderByDescending(x => x.CostingDate).First().ProductDescription
            ?? string.Empty;
        // 明细有而主表无：回填新建主表行（补齐机种/物料类型后写入产品月计算）
        if (existing == null)
        {
            existing = new TaktBomMaterialCost
            {
                PlantCode = plant,
                ModelCode = modelCode,
                MaterialType = materialType,
                ProductCode = product,
                ProductDescription = productDescription,
                ProductMonthlyCost = 0,
                ProductMonthlyCalculation = productMonthlyCost,
                ModelMonthlyAverageCost = 0,
                CurrencyCode = currency,
                CostingPeriod = periodKey,
                CostingDate = latestCostingDate.Date,
            };
            existing.ExtField = AppendProductCostOperationHistory(
                null,
                archiveOldCost,
                0m,
                productMonthlyCost,
                periodKey,
                plant,
                product,
                modelCode);
            existing = await _bomMaterialCostRepository.CreateAsync(existing);
            headerCache?.Add(existing);
        }
        else
        {
            var oldCalc = existing.ProductMonthlyCalculation;
            if (archiveOldCost)
            {
                ArchiveOldProductMonthlyCost(existing);
            }
            existing.ModelCode = string.IsNullOrWhiteSpace(existing.ModelCode)
                ? (modelCode ?? string.Empty)
                : existing.ModelCode;
            if (string.IsNullOrWhiteSpace(existing.MaterialType) && !string.IsNullOrWhiteSpace(materialType))
            {
                existing.MaterialType = materialType;
            }
            existing.ProductCode = product;
            existing.ProductDescription = productDescription;
            existing.ProductMonthlyCalculation = productMonthlyCost;
            existing.CurrencyCode = currency;
            existing.CostingPeriod = periodKey;
            existing.CostingDate = latestCostingDate.Date;
            existing.ExtField = AppendProductCostOperationHistory(
                existing.ExtField,
                archiveOldCost,
                oldCalc,
                productMonthlyCost,
                periodKey,
                plant,
                product,
                existing.ModelCode);
            await _bomMaterialCostRepository.UpdateAsync(existing);
        }
        var effectiveModel = string.IsNullOrWhiteSpace(existing.ModelCode)
            ? (modelCode ?? string.Empty)
            : existing.ModelCode.Trim();
        var effectiveType = string.IsNullOrWhiteSpace(existing.MaterialType)
            ? (materialType ?? string.Empty)
            : existing.MaterialType.Trim();
        if (!skipAverageRefresh)
        {
            await RefreshModelMonthlyAverageForPeriodAsync(plant, effectiveType, effectiveModel, periodKey);
            return null;
        }
        return string.IsNullOrWhiteSpace(effectiveType) || string.IsNullOrWhiteSpace(effectiveModel)
            ? null
            : (effectiveType, effectiveModel);
    }

    /// <summary>
    /// 在主表缓存中按工厂+产品+核算日查找（与业务唯一键一致；ModelCode 不参与匹配）
    /// </summary>
    private static TaktBomMaterialCost? FindHeaderInCache(
        List<TaktBomMaterialCost> headers,
        string plantCode,
        string productCode,
        DateTime costingDate)
    {
        ArgumentNullException.ThrowIfNull(headers);
        var day = costingDate.Date;
        return headers.FirstOrDefault(x =>
            string.Equals((x.PlantCode ?? string.Empty).Trim(), plantCode, StringComparison.OrdinalIgnoreCase)
            && x.CostingDate.Date == day
            && TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, productCode));
    }

    /// <summary>
    /// 按工厂+产品+核算日查找主表行（与业务唯一键一致）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <param name="costingDate">核算日</param>
    /// <returns>主表行；没有则 null</returns>
    private async Task<TaktBomMaterialCost?> FindHeaderByProductCostingDateAsync(
        string plantCode,
        string productCode,
        DateTime costingDate)
    {
        var variants = TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(productCode).ToList();
        if (variants.Count == 0)
        {
            return null;
        }
        var dayStart = costingDate.Date;
        var dayEnd = dayStart.AddDays(1);
        var list = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plantCode
                && x.CostingDate >= dayStart
                && x.CostingDate < dayEnd
                && x.ProductCode != null
                && variants.Contains(x.ProductCode),
            includeSoftDeleted: true);
        return list.FirstOrDefault(x =>
            TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(x.ProductCode, productCode));
    }

    /// <summary>
    /// 刷新同工厂+物料类型+机种+核算月的机种月均
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <param name="periodKey">核算月</param>
    /// <returns>更新行数</returns>
    private async Task<int> RefreshModelMonthlyAverageForPeriodAsync(
        string plantCode,
        string materialType,
        string modelCode,
        string periodKey)
    {
        if (string.IsNullOrWhiteSpace(modelCode) || string.IsNullOrWhiteSpace(materialType))
        {
            return 0;
        }
        var mt = materialType.Trim();
        var model = modelCode.Trim();
        var plant = plantCode.Trim();
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.PlantCode == plant
                && x.CostingPeriod == periodKey,
            includeSoftDeleted: true);
        var matched = headers
            .Where(h =>
                string.Equals((h.ModelCode ?? string.Empty).Trim(), model, StringComparison.OrdinalIgnoreCase)
                && MaterialTypeMatchesModelAverageGroup(h.MaterialType, mt))
            .ToList();
        return await ApplyModelMonthlyAverageAndSaveAsync(matched);
    }

    /// <summary>
    /// 按产品金额算术平均写入机种月均并保存。
    /// 固定口径：核算月 &lt; 2026-06 用产品月成本，≥ 2026-06 用产品月计算；
    /// 流程：全量算出新均值 → 与行上原机种月成本比较 → 仅「新值≠旧值」才落库并写 ExtField。
    /// </summary>
    /// <param name="headers">同组主表行</param>
    /// <returns>实际更新行数</returns>
    private async Task<int> ApplyModelMonthlyAverageAndSaveAsync(List<TaktBomMaterialCost> headers)
    {
        ArgumentNullException.ThrowIfNull(headers);
        if (headers.Count == 0)
        {
            return 0;
        }
        // 按产品去重；核算月&lt;2026-06 用产品月成本，≥2026-06 用产品月计算 → 算术平均
        var average = TaktBomMaterialCostItemModelEnrichmentHelper.ComputeModelMonthlyAverageFromHeaders(headers);
        var updated = 0;
        var now = DateTime.Now;
        var operatorUserId = CurrentUserId ?? 0L;
        foreach (var header in headers)
        {
            var current = TaktBomMaterialCostItemLineCostHelper.RoundCost(header.ModelMonthlyAverageCost);
            // 先全量算 average，再比较：仅新旧相等才跳过（不是「原值非 0 就跳过」）
            if (current == average)
            {
                continue;
            }
            var nextExt = AppendModelAvgOperationHistory(
                header.ExtField,
                current,
                average,
                header.CostingPeriod,
                header.PlantCode,
                header.ProductCode,
                header.ModelCode,
                header.MaterialType);
            // 全量写回：不要求 IsDeleted=0（软删主表同样更新机种月均）
            var rows = await _bomMaterialCostRepository.UpdateAsync(
                x => x.Id == header.Id,
                x => new TaktBomMaterialCost
                {
                    ModelMonthlyAverageCost = average,
                    ExtField = nextExt,
                    UpdatedAt = now,
                    UpdatedBy = operatorUserId,
                });
            if (rows <= 0)
            {
                continue;
            }
            header.ModelMonthlyAverageCost = average;
            header.ExtField = nextExt;
            updated = checked(updated + 1);
        }
        return updated;
    }

    /// <summary>
    /// 是否匹配查询栏所选物料类型（空筛选=全部类型）
    /// </summary>
    /// <param name="resolvedType">从通用物料/工厂物料解析出的类型</param>
    /// <param name="filterType">查询所选类型；空则不过滤</param>
    /// <returns>是否纳入本次计算</returns>
    private static bool MatchesSelectedMaterialType(string? resolvedType, string? filterType)
    {
        if (string.IsNullOrWhiteSpace(filterType))
        {
            return true;
        }
        return string.Equals(
            resolvedType?.Trim(),
            filterType.Trim(),
            StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 机种月均分组键（物料类型|机种；类型空时用 FERT 仅用于分组，不改变筛选）
    /// </summary>
    /// <param name="materialType">物料类型</param>
    /// <param name="modelCode">机种</param>
    /// <returns>分组键；机种空则空串</returns>
    private static string BuildModelAverageGroupKey(string? materialType, string? modelCode)
    {
        var model = modelCode?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(model))
        {
            return string.Empty;
        }
        var mt = string.IsNullOrWhiteSpace(materialType)
            ? TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode
            : materialType.Trim();
        return $"{mt}|{model}";
    }

    /// <summary>
    /// 空物料类型是否与分组期望类型视为同组（仅空值对齐 FERT 分组，不把筛选锁成 FERT）
    /// </summary>
    /// <param name="stored">主表已存类型</param>
    /// <param name="expected">分组期望类型</param>
    /// <returns>是否同组</returns>
    private static bool MaterialTypeMatchesModelAverageGroup(string? stored, string expected)
    {
        var actual = stored?.Trim() ?? string.Empty;
        if (string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return string.IsNullOrEmpty(actual)
            && string.Equals(
                expected,
                TaktBomMaterialCostItemLineCostHelper.FertMaterialTypeCode,
                StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 解析机种多选（ModelCode 支持逗号/分号分隔，与零价格对齐）
    /// </summary>
    /// <param name="multiCodes">多选串（当前计算 DTO 无独立字段时传 null）</param>
    /// <param name="singleCode">ModelCode（可含逗号）</param>
    /// <returns>去重机种列表</returns>
    private static List<string> ParseModelCodes(string? multiCodes, string? singleCode)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void AddRaw(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return;
            }
            foreach (var part in raw.Split(
                         new[] { ',', ';' },
                         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    set.Add(part);
                }
            }
        }
        AddRaw(multiCodes);
        AddRaw(singleCode);
        return set.OrderBy(c => c, StringComparer.Ordinal).ToList();
    }

    /// <summary>
    /// 按期间+工厂加载主表已有机种/物料类型（供筛选；不回填）
    /// </summary>
    /// <param name="periodKey">核算月 yyyy-MM</param>
    /// <param name="plantCodes">工厂</param>
    /// <returns>键为工厂|产品</returns>
    private async Task<Dictionary<string, (string MaterialType, string ModelCode)>> LoadBomHeaderScopeLookupAsync(
        string periodKey,
        IReadOnlyCollection<string> plantCodes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(periodKey);
        ArgumentNullException.ThrowIfNull(plantCodes);
        var plants = plantCodes
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Select(p => p.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var map = new Dictionary<string, (string MaterialType, string ModelCode, DateTime CostingDate)>(
            StringComparer.OrdinalIgnoreCase);
        if (plants.Count == 0)
        {
            return new Dictionary<string, (string MaterialType, string ModelCode)>(StringComparer.OrdinalIgnoreCase);
        }
        var headers = await _bomMaterialCostRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.CostingPeriod == periodKey
                && plants.Contains(x.PlantCode),
            includeSoftDeleted: true);
        foreach (var header in headers)
        {
            var plant = header.PlantCode?.Trim() ?? string.Empty;
            var product = TaktStringHelper.NormalizeSapNumericMaterialCode(header.ProductCode?.Trim() ?? string.Empty);
            if (string.IsNullOrWhiteSpace(product))
            {
                product = header.ProductCode?.Trim() ?? string.Empty;
            }
            if (string.IsNullOrWhiteSpace(plant) || string.IsNullOrWhiteSpace(product))
            {
                continue;
            }
            var key = $"{plant}|{product}";
            if (!map.TryGetValue(key, out var previous) || header.CostingDate > previous.CostingDate)
            {
                map[key] = (
                    header.MaterialType?.Trim() ?? string.Empty,
                    header.ModelCode?.Trim() ?? string.Empty,
                    header.CostingDate);
            }
        }
        return map.ToDictionary(
            p => p.Key,
            p => (p.Value.MaterialType, p.Value.ModelCode),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 取主表已有物料类型/机种（无主表行为空）
    /// </summary>
    /// <param name="lookup">LoadBomHeaderScopeLookupAsync 结果</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <returns>物料类型、机种</returns>
    private static (string MaterialType, string ModelCode) ResolveBomHeaderScope(
        IReadOnlyDictionary<string, (string MaterialType, string ModelCode)> lookup,
        string plantCode,
        string? productCode)
    {
        ArgumentNullException.ThrowIfNull(lookup);
        var plant = plantCode?.Trim() ?? string.Empty;
        var product = TaktStringHelper.NormalizeSapNumericMaterialCode(productCode?.Trim() ?? string.Empty);
        if (string.IsNullOrWhiteSpace(product))
        {
            product = productCode?.Trim() ?? string.Empty;
        }
        if (string.IsNullOrWhiteSpace(plant) || string.IsNullOrWhiteSpace(product))
        {
            return (string.Empty, string.Empty);
        }
        var key = $"{plant}|{product}";
        return lookup.TryGetValue(key, out var hit) ? hit : (string.Empty, string.Empty);
    }

    /// <summary>
    /// 按所选物料类型/机种/处理上限过滤待回填明细（与计算成本同一产品范围）
    /// </summary>
    /// <param name="itemRows">核算月明细</param>
    /// <param name="query">规范化查询</param>
    /// <returns>待回填行</returns>
    private async Task<List<TaktBomMaterialCostItem>> FilterBomCalculateItemRowsForPurchaseBackfillAsync(
        List<TaktBomMaterialCostItem> itemRows,
        TaktBomCalculateQueryDto query)
    {
        ArgumentNullException.ThrowIfNull(itemRows);
        ArgumentNullException.ThrowIfNull(query);
        var modelFilterSet = new HashSet<string>(
            ParseModelCodes(null, query.ModelCode),
            StringComparer.OrdinalIgnoreCase);
        var filterMaterialType = string.IsNullOrWhiteSpace(query.MaterialType)
            ? null
            : query.MaterialType.Trim();
        var processRecordCount = query.ProcessRecordCount;
        var productCodes = itemRows
            .Select(r => r.ProductCode)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var plantCodes = itemRows
            .Select(r => r.PlantCode?.Trim() ?? string.Empty)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        var periodKey = TaktBomMaterialCostItemLineCostHelper.ToPeriodKey(
            query.CostingDateStart ?? DateTime.Today);
        var headerScope = await LoadBomHeaderScopeLookupAsync(periodKey, plantCodes);
        var allowedProducts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var product in productCodes)
        {
            var plant = itemRows
                .First(r => TaktBomMaterialCostItemLineCostHelper.ProductCodeMatches(r.ProductCode, product))
                .PlantCode?.Trim() ?? string.Empty;
            var (headerType, headerModel) = ResolveBomHeaderScope(headerScope, plant, product);
            if (!MatchesSelectedMaterialType(headerType, filterMaterialType))
            {
                continue;
            }
            if (modelFilterSet.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(headerModel) || !modelFilterSet.Contains(headerModel))
                {
                    continue;
                }
            }
            allowedProducts.Add(product);
        }
        var grouped = itemRows
            .Where(r =>
                !string.IsNullOrWhiteSpace(r.PlantCode)
                && !string.IsNullOrWhiteSpace(r.ProductCode)
                && allowedProducts.Contains(r.ProductCode))
            .GroupBy(
                r => $"{r.PlantCode!.Trim()}|{r.ProductCode!.Trim()}",
                StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (processRecordCount > 0 && grouped.Count > processRecordCount)
        {
            grouped = grouped.Take(processRecordCount).ToList();
        }
        return grouped.SelectMany(g => g).ToList();
    }

    /// <summary>
    /// 按组件编码批量加载采购价格主表、条件行与等级（不按工厂过滤；选价按核算日对照 ValidFrom）
    /// </summary>
    /// <param name="itemRows">待回填明细</param>
    /// <returns>查找表</returns>
    private async Task<BomCalculatePurchaseInfoRecordLookup> LoadPurchaseInfoRecordLookupAsync(
        IReadOnlyList<TaktBomMaterialCostItem> itemRows)
    {
        ArgumentNullException.ThrowIfNull(itemRows);
        var lookup = new BomCalculatePurchaseInfoRecordLookup();
        var materialCodes = itemRows
            .SelectMany(r => TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(r.ComponentCode))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (materialCodes.Count == 0)
        {
            return lookup;
        }
        const int chunkSize = 200;
        var headers = new List<TaktPurchaseInfoRecord>();
        for (var i = 0; i < materialCodes.Count; i += chunkSize)
        {
            var chunk = materialCodes.Skip(i).Take(chunkSize).ToList();
            var part = await _purchaseInfoRecordRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && chunk.Contains(x.MaterialCode));
            if (part.Count > 0)
            {
                headers.AddRange(part);
            }
        }
        lookup.AddHeaders(headers);
        var headerIds = headers.Select(h => h.Id).Distinct().ToList();
        if (headerIds.Count == 0)
        {
            return lookup;
        }
        var items = new List<TaktPurchaseInfoRecordItem>();
        for (var i = 0; i < headerIds.Count; i += chunkSize)
        {
            var chunk = headerIds.Skip(i).Take(chunkSize).ToList();
            var part = await _purchaseInfoRecordItemRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && chunk.Contains(x.PurchaseInfoRecordId)
                    && x.IsObsolete == 0);
            if (part.Count > 0)
            {
                items.AddRange(part);
            }
        }
        lookup.AddItems(items);
        var itemIds = items.Select(it => it.Id).Distinct().ToList();
        if (itemIds.Count == 0)
        {
            return lookup;
        }
        for (var i = 0; i < itemIds.Count; i += chunkSize)
        {
            var chunk = itemIds.Skip(i).Take(chunkSize).ToList();
            var qtyPart = await _purchaseInfoRecordScaleQuantityRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && chunk.Contains(x.PurchaseInfoRecordItemId)
                    && x.IsObsolete == 0);
            lookup.AddQuantityScales(qtyPart);
            var valuePart = await _purchaseInfoRecordScaleValueRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode
                    && x.CompanyCode == CurrentCompanyCode
                    && chunk.Contains(x.PurchaseInfoRecordItemId)
                    && x.IsObsolete == 0);
            lookup.AddValueScales(valuePart);
        }
        return lookup;
    }

    /// <summary>
    /// 采购价格内存查找（按物料 / 主表 Id / 条件行 Id）
    /// </summary>
    private sealed class BomCalculatePurchaseInfoRecordLookup
    {
        /// <summary>
        /// 物料编码 → 主表
        /// </summary>
        private readonly Dictionary<string, List<TaktPurchaseInfoRecord>> _headersByMaterial = new(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 主表 Id → 条件行
        /// </summary>
        private readonly Dictionary<long, List<TaktPurchaseInfoRecordItem>> _itemsByHeaderId = new();
        /// <summary>
        /// 条件行 Id → 数量等级
        /// </summary>
        private readonly Dictionary<long, List<TaktPurchaseInfoRecordScaleQuantity>> _quantityByItemId = new();
        /// <summary>
        /// 条件行 Id → 价值等级
        /// </summary>
        private readonly Dictionary<long, List<TaktPurchaseInfoRecordScaleValue>> _valueByItemId = new();

        /// <summary>
        /// 登记主表（同一物料的 10/18 位编码都可命中）
        /// </summary>
        /// <param name="headers">主表</param>
        public void AddHeaders(IReadOnlyList<TaktPurchaseInfoRecord> headers)
        {
            ArgumentNullException.ThrowIfNull(headers);
            foreach (var header in headers)
            {
                foreach (var code in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(header.MaterialCode))
                {
                    if (!_headersByMaterial.TryGetValue(code, out var list))
                    {
                        list = new List<TaktPurchaseInfoRecord>();
                        _headersByMaterial[code] = list;
                    }
                    if (!list.Exists(h => h.Id == header.Id))
                    {
                        list.Add(header);
                    }
                }
            }
        }

        /// <summary>
        /// 登记条件行
        /// </summary>
        /// <param name="items">条件行</param>
        public void AddItems(IReadOnlyList<TaktPurchaseInfoRecordItem> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            foreach (var item in items)
            {
                if (!_itemsByHeaderId.TryGetValue(item.PurchaseInfoRecordId, out var list))
                {
                    list = new List<TaktPurchaseInfoRecordItem>();
                    _itemsByHeaderId[item.PurchaseInfoRecordId] = list;
                }
                list.Add(item);
            }
        }

        /// <summary>
        /// 登记数量等级
        /// </summary>
        /// <param name="rows">数量等级</param>
        public void AddQuantityScales(IReadOnlyList<TaktPurchaseInfoRecordScaleQuantity> rows)
        {
            ArgumentNullException.ThrowIfNull(rows);
            foreach (var row in rows)
            {
                if (!_quantityByItemId.TryGetValue(row.PurchaseInfoRecordItemId, out var list))
                {
                    list = new List<TaktPurchaseInfoRecordScaleQuantity>();
                    _quantityByItemId[row.PurchaseInfoRecordItemId] = list;
                }
                list.Add(row);
            }
        }

        /// <summary>
        /// 登记价值等级
        /// </summary>
        /// <param name="rows">价值等级</param>
        public void AddValueScales(IReadOnlyList<TaktPurchaseInfoRecordScaleValue> rows)
        {
            ArgumentNullException.ThrowIfNull(rows);
            foreach (var row in rows)
            {
                if (!_valueByItemId.TryGetValue(row.PurchaseInfoRecordItemId, out var list))
                {
                    list = new List<TaktPurchaseInfoRecordScaleValue>();
                    _valueByItemId[row.PurchaseInfoRecordItemId] = list;
                }
                list.Add(row);
            }
        }

        /// <summary>
        /// 按组件编码取主表（物料键；含 10/18 位互认）
        /// </summary>
        /// <param name="componentCode">组件编码</param>
        /// <returns>主表列表</returns>
        public IReadOnlyList<TaktPurchaseInfoRecord> FindHeaders(string componentCode)
        {
            var seen = new HashSet<long>();
            var result = new List<TaktPurchaseInfoRecord>();
            foreach (var code in TaktBomMaterialCostItemLineCostHelper.ExpandProductCodeLookupVariants(componentCode))
            {
                if (!_headersByMaterial.TryGetValue(code, out var list))
                {
                    continue;
                }
                foreach (var header in list)
                {
                    if (seen.Add(header.Id))
                    {
                        result.Add(header);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 按主表 Id 取条件行
        /// </summary>
        /// <param name="headerId">主表 Id</param>
        /// <returns>条件行</returns>
        public IReadOnlyList<TaktPurchaseInfoRecordItem> FindItems(long headerId)
        {
            return _itemsByHeaderId.TryGetValue(headerId, out var list)
                ? list
                : Array.Empty<TaktPurchaseInfoRecordItem>();
        }

        /// <summary>
        /// 按条件行 Id 取数量等级
        /// </summary>
        /// <param name="itemId">条件行 Id</param>
        /// <returns>数量等级</returns>
        public IReadOnlyList<TaktPurchaseInfoRecordScaleQuantity> FindQuantityScales(long itemId)
        {
            return _quantityByItemId.TryGetValue(itemId, out var list)
                ? list
                : Array.Empty<TaktPurchaseInfoRecordScaleQuantity>();
        }

        /// <summary>
        /// 按条件行 Id 取价值等级
        /// </summary>
        /// <param name="itemId">条件行 Id</param>
        /// <returns>价值等级</returns>
        public IReadOnlyList<TaktPurchaseInfoRecordScaleValue> FindValueScales(long itemId)
        {
            return _valueByItemId.TryGetValue(itemId, out var list)
                ? list
                : Array.Empty<TaktPurchaseInfoRecordScaleValue>();
        }
    }

    /// <summary>
    /// 追加合计/重算履历到 ExtField._bk.sum 或 _bk.recalc（仅追加）
    /// </summary>
    /// <param name="extField">原扩展字段</param>
    /// <param name="isRecalc">是否重算</param>
    /// <param name="oldCalc">旧产品月计算</param>
    /// <param name="newCalc">新产品月计算</param>
    /// <param name="periodKey">核算月</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="productCode">产品</param>
    /// <param name="modelCode">机种</param>
    /// <returns>合并后 ExtField</returns>
    private static string? AppendProductCostOperationHistory(
        string? extField,
        bool isRecalc,
        decimal oldCalc,
        decimal newCalc,
        string periodKey,
        string plantCode,
        string productCode,
        string? modelCode)
    {
        var scope = isRecalc
            ? TaktBomExtFieldBackfillHistoryHelper.ScopeRecalc
            : TaktBomExtFieldBackfillHistoryHelper.ScopeSum;
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["costing_period"] = periodKey?.Trim() ?? string.Empty,
            ["plant_code"] = plantCode?.Trim() ?? string.Empty,
            ["product_code"] = productCode?.Trim() ?? string.Empty,
            ["model_code"] = modelCode?.Trim() ?? string.Empty,
            ["old_product_monthly_calculation"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldCalc),
            ["new_product_monthly_calculation"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(newCalc),
        };
        return TaktBomExtFieldBackfillHistoryHelper.Append(extField, scope, entry);
    }

    /// <summary>
    /// 追加机种月均履历到 ExtField._bk.model_avg（仅追加；含计算前/后机种月成本）
    /// </summary>
    private static string? AppendModelAvgOperationHistory(
        string? extField,
        decimal oldAvg,
        decimal newAvg,
        string? periodKey,
        string? plantCode,
        string? productCode,
        string? modelCode,
        string? materialType = null)
    {
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["costing_period"] = periodKey?.Trim() ?? string.Empty,
            ["plant_code"] = plantCode?.Trim() ?? string.Empty,
            ["product_code"] = productCode?.Trim() ?? string.Empty,
            ["model_code"] = modelCode?.Trim() ?? string.Empty,
            ["material_type"] = materialType?.Trim() ?? string.Empty,
            ["old_model_monthly_average_cost"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldAvg),
            ["new_model_monthly_average_cost"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(newAvg),
        };
        return TaktBomExtFieldBackfillHistoryHelper.Append(
            extField,
            TaktBomExtFieldBackfillHistoryHelper.ScopeModelAvg,
            entry);
    }

    /// <summary>
    /// 追加最近采购成本履历到 ExtField._bk.lpc（仅追加）
    /// </summary>
    private static string? AppendLatestPurchaseOperationHistory(
        string? extField,
        decimal oldCost,
        decimal newCost,
        string periodKey,
        string plantCode,
        string productCode,
        string? modelCode)
    {
        var entry = new JsonObject
        {
            ["at"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            ["costing_period"] = periodKey?.Trim() ?? string.Empty,
            ["plant_code"] = plantCode?.Trim() ?? string.Empty,
            ["product_code"] = productCode?.Trim() ?? string.Empty,
            ["model_code"] = modelCode?.Trim() ?? string.Empty,
            ["old_latest_purchase_cost"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(oldCost),
            ["new_latest_purchase_cost"] = TaktBomMaterialCostItemLineCostHelper.RoundCost(newCost),
        };
        return TaktBomExtFieldBackfillHistoryHelper.Append(
            extField,
            TaktBomExtFieldBackfillHistoryHelper.ScopeLatestPurchase,
            entry);
    }

    /// <summary>
    /// 重算前把当前产品月计算写入 ExtField JSON（键=旧核算日 yyyy/M/d，值=旧计算）
    /// </summary>
    /// <param name="existing">已有主表行</param>
    private static void ArchiveOldProductMonthlyCost(TaktBomMaterialCost existing)
    {
        ArgumentNullException.ThrowIfNull(existing);
        existing.ExtField = AppendOldCostToExtField(
            existing.ExtField,
            existing.CostingDate,
            existing.ProductMonthlyCalculation);
    }

    /// <summary>
    /// 追加一条旧成本到 ExtField JSON 对象；超过 nvarchar(4000) 时丢弃最早核算日（保留 _bk）
    /// </summary>
    /// <param name="extField">当前扩展字段</param>
    /// <param name="oldCostingDate">旧核算日期</param>
    /// <param name="oldCost">旧产品月计算</param>
    /// <returns>序列化后的 JSON</returns>
    private static string AppendOldCostToExtField(string? extField, DateTime oldCostingDate, decimal oldCost)
    {
        var obj = ParseExtFieldObject(extField);
        var previous = extField?.Trim() ?? string.Empty;
        var key = oldCostingDate.ToString(OldCostDateFormat, CultureInfo.InvariantCulture);
        obj[key] = JsonValue.Create(decimal.Round(oldCost, 5, MidpointRounding.AwayFromZero));
        var json = obj.ToJsonString();
        while (json.Length > BomMaterialCostExtFieldMaxLength && obj.Count > 1)
        {
            var oldestKey = obj
                .Select(p => p.Key)
                .Where(k => !string.Equals(
                    k,
                    TaktBomExtFieldBackfillHistoryHelper.ExtFieldBackfillRootKey,
                    StringComparison.Ordinal))
                .OrderBy(ParseOldCostDateKey)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(oldestKey))
            {
                break;
            }
            obj.Remove(oldestKey);
            json = obj.ToJsonString();
        }
        if (json.Length > BomMaterialCostExtFieldMaxLength)
        {
            return previous;
        }
        return json;
    }

    /// <summary>
    /// 将 ExtField 解析为 JSON 对象；空或非法时返回空对象
    /// </summary>
    /// <param name="extField">扩展字段</param>
    /// <returns>JSON 对象</returns>
    private static JsonObject ParseExtFieldObject(string? extField)
    {
        if (string.IsNullOrWhiteSpace(extField))
        {
            return new JsonObject();
        }
        try
        {
            return JsonNode.Parse(extField) as JsonObject ?? new JsonObject();
        }
        catch (System.Text.Json.JsonException)
        {
            return new JsonObject();
        }
    }

    /// <summary>
    /// 解析旧成本 JSON 键为日期；无法解析则排到最早（优先丢弃非法键）
    /// </summary>
    /// <param name="key">JSON 键</param>
    /// <returns>日期</returns>
    private static DateTime ParseOldCostDateKey(string key)
    {
        if (DateTime.TryParseExact(
                key,
                OldCostDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsed))
        {
            return parsed;
        }
        return DateTime.MinValue;
    }

    /// <summary>
    /// 明细年表物理名
    /// </summary>
    /// <param name="year">年</param>
    /// <returns>表名</returns>
    private static string BuildBomItemYearTable(int year) =>
        TaktYearShardTableHelper.BuildYearTableName(BomItemYearShardBaseTable, year);

    /// <summary>
    /// 解析当年明细物理表；不存在则 null（走基表）
    /// </summary>
    /// <param name="year">年</param>
    /// <returns>物理表名或 null</returns>
    private async Task<string?> ResolveBomItemPhysicalTableAsync(int year)
    {
        var table = BuildBomItemYearTable(year);
        return await _bomMaterialCostItemRepository.PhysicalTableExistsAsync(table) ? table : null;
    }
}

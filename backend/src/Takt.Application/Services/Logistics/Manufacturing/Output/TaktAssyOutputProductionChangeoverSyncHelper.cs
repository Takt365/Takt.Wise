// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputProductionChangeoverSyncHelper.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细无产出但有报工工时时自动 upsert 生产切换记录（切换类别 ASSY）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立产出明细 → 生产切换记录 级联同步辅助
/// </summary>
internal static class TaktAssyOutputProductionChangeoverSyncHelper
{
    private const string SyncExtFieldPrefix = "takt-assy-out:";
    private const string AssyChangeoverCategory = "ASSY";

    /// <summary>
    /// 刷新同一生产日期、生产班组、生产时段桶内的自动同步切换记录
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="productionChangeoverRepository">生产切换记录仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="TeamCode">生产班组</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="timePeriod">生产时段</param>
    /// <returns>任务</returns>
    public static async Task RefreshBucketAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        string TeamCode,
        DateTime prodDate,
        string timePeriod)
    {
        ArgumentNullException.ThrowIfNull(assyOutputRepository);
        ArgumentNullException.ThrowIfNull(assyOutputDetailRepository);
        ArgumentNullException.ThrowIfNull(productionChangeoverRepository);
        ArgumentNullException.ThrowIfNull(productionOrderRepository);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(TeamCode) || string.IsNullOrWhiteSpace(timePeriod))
        {
            return;
        }
        var prodDateOnly = prodDate.Date;
        var trimmedPeriod = timePeriod.Trim();
        var dayContext = await LoadAssyDayContextAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            tenantCode,
            companyCode,
            TeamCode,
            prodDateOnly);
        if (dayContext.Masters.Count == 0)
        {
            return;
        }
        var bucketLines = dayContext.Lines
            .Where(x => string.Equals(x.Detail.TimePeriod?.Trim(), trimmedPeriod, StringComparison.Ordinal))
            .ToList();
        if (bucketLines.Count == 0)
        {
            return;
        }
        var bucketDetailSyncKeys = bucketLines
            .Select(x => BuildSyncExtField(x.Master.Id, x.Detail.LineNumber))
            .ToHashSet(StringComparer.Ordinal);
        await DeleteAutoSyncChangeoversForBucketAsync(
            productionChangeoverRepository,
            tenantCode,
            companyCode,
            TeamCode,
            prodDateOnly,
            bucketDetailSyncKeys);
        var candidates = bucketLines
            .Where(x => TaktProductionStatHelper.IsAssyChangeoverCandidate(x.Detail.ProdActualQty, x.Detail.ConfirmMinutes))
            .ToList();
        if (candidates.Count == 0)
        {
            return;
        }
        var earlierProducer = FindLatestEarlierProducer(dayContext, trimmedPeriod);
        var groups = new Dictionary<string, ChangeoverAggregateGroup>(StringComparer.Ordinal);
        foreach (var candidate in candidates)
        {
            var changeoverMaster = candidate.Master;
            var currentMaster = ResolveCurrentMaster(changeoverMaster, bucketLines, earlierProducer);
            var groupKey = BuildNaturalKey(
                changeoverMaster.ProdCategory,
                prodDateOnly,
                TeamCode,
                currentMaster.ProdOrderCode,
                changeoverMaster.ProdOrderCode);
            if (!groups.TryGetValue(groupKey, out var group))
            {
                group = new ChangeoverAggregateGroup(
                    changeoverMaster.PlantCode,
                    changeoverMaster.ProdCategory,
                    prodDateOnly,
                    TeamCode,
                    currentMaster.ProdOrderCode,
                    currentMaster.ModelCode,
                    changeoverMaster.ProdOrderCode,
                    changeoverMaster.ModelCode,
                    changeoverMaster.DirectLabor + changeoverMaster.IndirectLabor);
                groups[groupKey] = group;
            }
            group.AddContributor(candidate.Detail, changeoverMaster);
        }
        foreach (var group in groups.Values)
        {
            await UpsertAutoChangeoverAsync(
                productionChangeoverRepository,
                productionOrderRepository,
                tenantCode,
                companyCode,
                group);
        }
    }

    /// <summary>
    /// 删除组立日报时刷新其明细涉及的生产时段切换桶
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="productionChangeoverRepository">生产切换记录仓储</param>
    /// <param name="productionOrderRepository">生产工单仓储</param>
    /// <param name="output">组立日报快照</param>
    /// <param name="details">组立日报明细快照</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task RefreshBucketsForDeletedOutputAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        TaktAssyOutput output,
        IReadOnlyList<TaktAssyOutputDetail> details,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(details);
        if (string.IsNullOrWhiteSpace(output.TeamCode))
        {
            return;
        }
        var buckets = details
            .Where(d => !string.IsNullOrWhiteSpace(d.TimePeriod))
            .Select(d => d.TimePeriod.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToList();
        foreach (var timePeriod in buckets)
        {
            await RefreshBucketAsync(
                assyOutputRepository,
                assyOutputDetailRepository,
                productionChangeoverRepository,
                productionOrderRepository,
                tenantCode,
                companyCode,
                output.TeamCode,
                output.ProdDate,
                timePeriod);
        }
    }

    /// <summary>
    /// 加载同一班组、生产日期下全部组立日报与明细
    /// </summary>
    private static async Task<AssyDayContext> LoadAssyDayContextAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        string tenantCode,
        string companyCode,
        string TeamCode,
        DateTime prodDateOnly)
    {
        var masters = await assyOutputRepository.GetListAsync(m =>
            m.TenantCode == tenantCode
            && m.CompanyCode == companyCode
            && m.TeamCode == TeamCode
            && m.ProdDate == prodDateOnly);
        if (masters.Count == 0)
        {
            return new AssyDayContext([], []);
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masterById.Keys.ToList();
        var details = await assyOutputDetailRepository.GetListAsync(d => masterIds.Contains(d.AssyOutputId));
        var lines = details
            .Where(d => masterById.ContainsKey(d.AssyOutputId))
            .Select(d => (Master: masterById[d.AssyOutputId], Detail: d))
            .ToList();
        return new AssyDayContext(masters, lines);
    }

    /// <summary>
    /// 解析切换前工单主表：优先同时段其他有产量工单，其次更早时段最近一笔有产量工单，否则取切换目标工单
    /// </summary>
    private static TaktAssyOutput ResolveCurrentMaster(
        TaktAssyOutput changeoverMaster,
        IReadOnlyList<(TaktAssyOutput Master, TaktAssyOutputDetail Detail)> bucketLines,
        (TaktAssyOutput Master, TaktAssyOutputDetail Detail)? earlierProducer)
    {
        var producersInBucket = bucketLines
            .Where(x => x.Detail.ProdActualQty > 0 && x.Master.Id != changeoverMaster.Id)
            .OrderByDescending(x => x.Detail.ProdActualQty)
            .ThenBy(x => x.Master.ProdOrderCode, StringComparer.OrdinalIgnoreCase)
            .ToList();
        if (producersInBucket.Count > 0)
        {
            return producersInBucket[0].Master;
        }
        if (earlierProducer.HasValue)
        {
            return earlierProducer.Value.Master;
        }
        return changeoverMaster;
    }

    /// <summary>
    /// 查找当前时段之前最近一笔有产量的组立日报明细
    /// </summary>
    private static (TaktAssyOutput Master, TaktAssyOutputDetail Detail)? FindLatestEarlierProducer(
        AssyDayContext dayContext,
        string timePeriod)
    {
        var periodIndex = Array.IndexOf(TaktAssyOutputTimePeriodConstants.DefaultTimePeriods, timePeriod);
        if (periodIndex <= 0)
        {
            return null;
        }
        for (var i = periodIndex - 1; i >= 0; i--)
        {
            var period = TaktAssyOutputTimePeriodConstants.DefaultTimePeriods[i];
            var producer = dayContext.Lines
                .Where(x => string.Equals(x.Detail.TimePeriod?.Trim(), period, StringComparison.Ordinal)
                    && x.Detail.ProdActualQty > 0)
                .OrderByDescending(x => x.Detail.ProdActualQty)
                .ThenBy(x => x.Master.ProdOrderCode, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault();
            if (producer.Master != null)
            {
                return producer;
            }
        }
        return null;
    }

    /// <summary>
    /// 删除本桶内由组立日报自动同步且不再有效的切换记录
    /// </summary>
    private static async Task DeleteAutoSyncChangeoversForBucketAsync(
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        string tenantCode,
        string companyCode,
        string TeamCode,
        DateTime prodDateOnly,
        HashSet<string> bucketDetailSyncKeys)
    {
        var existing = await productionChangeoverRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.TeamCode == TeamCode
            && x.ProdDate == prodDateOnly
            && x.ChangeoverCategory == AssyChangeoverCategory
            && x.ExtField != null
            && x.ExtField.StartsWith(SyncExtFieldPrefix, StringComparison.Ordinal));
        foreach (var record in existing)
        {
            if (ExtFieldIntersectsBucket(record.ExtField, bucketDetailSyncKeys))
            {
                await productionChangeoverRepository.DeleteAsync(record.Id);
            }
        }
    }

    /// <summary>
    /// 按自然键 upsert 自动同步的生产切换记录
    /// </summary>
    private static async Task UpsertAutoChangeoverAsync(
        ITaktCompanyRepository<TaktProductionChangeover> productionChangeoverRepository,
        ITaktCompanyRepository<TaktProductionOrder> productionOrderRepository,
        string tenantCode,
        string companyCode,
        ChangeoverAggregateGroup group)
    {
        var existing = await productionChangeoverRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == group.ProdCategory
            && x.ChangeoverCategory == AssyChangeoverCategory
            && x.ProdDate == group.ProdDate
            && x.TeamCode == group.TeamCode
            && x.CurrentProdOrderCode == group.CurrentProdOrderCode
            && x.ChangeoverProdOrderCode == group.ChangeoverProdOrderCode);
        if (existing != null && !IsAutoSyncRecord(existing))
        {
            return;
        }
        var entity = existing ?? new TaktProductionChangeover
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
        };
        entity.PlantCode = group.PlantCode;
        entity.ProdCategory = group.ProdCategory;
        entity.ChangeoverCategory = AssyChangeoverCategory;
        entity.ProdDate = group.ProdDate;
        entity.TeamCode = group.TeamCode;
        entity.CurrentProdOrderCode = group.CurrentProdOrderCode;
        entity.CurrentModelCode = group.CurrentModelCode;
        entity.ChangeoverProdOrderCode = group.ChangeoverProdOrderCode;
        entity.ChangeoverModelCode = group.ChangeoverModelCode;
        entity.ChangeoverCount = group.ContributorCount;
        entity.TotalChangeoverTime = group.TotalMinutes;
        entity.ChangeoverTime = group.ContributorCount > 0
            ? (int)Math.Round((decimal)group.TotalMinutes / group.ContributorCount, MidpointRounding.AwayFromZero)
            : 0;
        entity.PersonCount = group.PersonCount;
        entity.ExtField = string.Join(';', group.SyncKeys.OrderBy(k => k, StringComparer.Ordinal));
        await TaktProductionOrderBackfillHelper.ApplyPlantCodeAsync(
            productionOrderRepository,
            tenantCode,
            companyCode,
            entity.CurrentProdOrderCode,
            v => entity.PlantCode = v);
        if (existing == null)
        {
            await productionChangeoverRepository.CreateAsync(entity);
        }
        else
        {
            await productionChangeoverRepository.UpdateAsync(entity);
        }
    }

    /// <summary>
    /// 构建产出明细同步 ExtField
    /// </summary>
    private static string BuildSyncExtField(long assyOutputId, int lineNumber)
    {
        return $"{SyncExtFieldPrefix}{assyOutputId}:{lineNumber}";
    }

    /// <summary>
    /// 判断 ExtField 是否关联本桶内任一组立日报明细
    /// </summary>
    private static bool ExtFieldIntersectsBucket(string? extField, HashSet<string> bucketDetailSyncKeys)
    {
        if (string.IsNullOrWhiteSpace(extField))
        {
            return false;
        }
        foreach (var token in extField.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (bucketDetailSyncKeys.Contains(token))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否为组立日报自动同步记录
    /// </summary>
    private static bool IsAutoSyncRecord(TaktProductionChangeover entity)
    {
        return !string.IsNullOrWhiteSpace(entity.ExtField)
            && entity.ExtField.StartsWith(SyncExtFieldPrefix, StringComparison.Ordinal);
    }

    /// <summary>
    /// 构建切换记录自然键（与库唯一键对齐：ProdCategory+ChangeoverCategory+ProdDate+TeamCode+CurrentProdOrderCode+ChangeoverProdOrderCode）
    /// </summary>
    private static string BuildNaturalKey(
        string prodCategory,
        DateTime prodDate,
        string teamCode,
        string currentProdOrderCode,
        string changeoverProdOrderCode)
    {
        return string.Join('|',
            prodCategory,
            AssyChangeoverCategory,
            prodDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
            teamCode,
            currentProdOrderCode,
            changeoverProdOrderCode);
    }

    private sealed class AssyDayContext
    {
        public AssyDayContext(
            IReadOnlyList<TaktAssyOutput> masters,
            IReadOnlyList<(TaktAssyOutput Master, TaktAssyOutputDetail Detail)> lines)
        {
            Masters = masters;
            Lines = lines;
        }

        public IReadOnlyList<TaktAssyOutput> Masters { get; }
        public IReadOnlyList<(TaktAssyOutput Master, TaktAssyOutputDetail Detail)> Lines { get; }
    }

    private sealed class ChangeoverAggregateGroup
    {
        private readonly List<string> _syncKeys = [];

        public ChangeoverAggregateGroup(
            string plantCode,
            string prodCategory,
            DateTime prodDate,
            string teamCode,
            string currentProdOrderCode,
            string currentModelCode,
            string changeoverProdOrderCode,
            string changeoverModelCode,
            int personCount)
        {
            PlantCode = plantCode;
            ProdCategory = prodCategory;
            ProdDate = prodDate;
            TeamCode = teamCode;
            CurrentProdOrderCode = currentProdOrderCode;
            CurrentModelCode = currentModelCode;
            ChangeoverProdOrderCode = changeoverProdOrderCode;
            ChangeoverModelCode = changeoverModelCode;
            PersonCount = personCount;
        }

        public string PlantCode { get; }
        public string ProdCategory { get; }
        public DateTime ProdDate { get; }
        public string TeamCode { get; }
        public string CurrentProdOrderCode { get; }
        public string CurrentModelCode { get; }
        public string ChangeoverProdOrderCode { get; }
        public string ChangeoverModelCode { get; }
        public int PersonCount { get; }
        public int ContributorCount { get; private set; }
        public int TotalMinutes { get; private set; }
        public IReadOnlyList<string> SyncKeys => _syncKeys;

        public void AddContributor(TaktAssyOutputDetail detail, TaktAssyOutput master)
        {
            ContributorCount += 1;
            TotalMinutes += (int)Math.Round(detail.ConfirmMinutes, MidpointRounding.AwayFromZero);
            _syncKeys.Add(BuildSyncExtField(master.Id, detail.LineNumber));
        }
    }
}

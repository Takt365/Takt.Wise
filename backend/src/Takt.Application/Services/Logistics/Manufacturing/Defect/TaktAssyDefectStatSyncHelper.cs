// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectStatSyncHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立产出/不良日报变更后刷新工单不良/批量不良统计表
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立工单/批量不良统计同步辅助
/// </summary>
internal static class TaktAssyDefectStatSyncHelper
{
    /// <summary>
    /// 统计快照（按组立日报一行；无不良日报时无不良数量=生实实绩）
    /// </summary>
    private sealed class AssyStatReportSnapshot
    {
        public long Id { get; init; }
        public string PlantCode { get; init; } = string.Empty;
        public string ProdCategory { get; init; } = string.Empty;
        public DateTime ProdDate { get; init; }
        public string ProdOrderCode { get; init; } = string.Empty;
        public string ModelCode { get; init; } = string.Empty;
        public string? BatchCode { get; init; }
        public string MaterialCode { get; init; } = string.Empty;
        public decimal ProdOrderQty { get; init; }
        public decimal ProdActualQty { get; init; }
        public decimal GoodQuantity { get; init; }
    }

    /// <summary>
    /// 按组立不良日报汇总刷新工单与批量不良统计（统计维度：生产类别+工单号 / 生产类别+批次）
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="batchCode">批次（为空时跳过批量统计）</param>
    /// <returns>任务</returns>
    public static async Task RefreshDefectStatsFromAssyDefectAsync(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string? prodCategory,
        string prodOrderCode,
        string? batchCode)
    {
        var normalizedProdCategory = prodCategory?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedProdCategory))
        {
            return;
        }
        await RefreshOrderDefectStatFromAssyDefectAsync(
            assyDefectRepository,
            assyOrderDefectRepository,
            tenantCode,
            companyCode,
            normalizedProdCategory,
            prodOrderCode);
        if (!string.IsNullOrWhiteSpace(batchCode))
        {
            await RefreshBatchDefectStatFromAssyDefectAsync(
                assyDefectRepository,
                assyBatchDefectRepository,
                tenantCode,
                companyCode,
                normalizedProdCategory,
                batchCode);
        }
    }

    /// <summary>
    /// 刷新指定工单号的工单不良统计（统计维度：生产类别+工单号；汇总 TaktAssyDefect）
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>任务</returns>
    public static async Task RefreshOrderDefectStatFromAssyDefectAsync(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodCategory)
            || string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        var defects = await assyDefectRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.ProdOrderCode == prodOrderCode);
        var reports = BuildStatSnapshotsFromDefects(defects);
        await UpsertOrderDefectStatAsync(
            assyOrderDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            prodOrderCode,
            reports);
    }

    /// <summary>
    /// 刷新指定批次的批量不良统计（统计维度：生产类别+批次；汇总 TaktAssyDefect）
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="batchCode">批次</param>
    /// <returns>任务</returns>
    public static async Task RefreshBatchDefectStatFromAssyDefectAsync(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string batchCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodCategory)
            || string.IsNullOrWhiteSpace(batchCode))
        {
            return;
        }
        var defects = await assyDefectRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.BatchCode == batchCode);
        var reports = BuildStatSnapshotsFromDefects(defects);
        await UpsertBatchDefectStatAsync(
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            batchCode,
            reports);
    }

    /// <summary>
    /// 刷新指定工单号的工单不良统计（统计维度：生产类别+工单号；产出日报+不良日报混合口径，供组立产出同步）
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储（仅用于已手工录入的不良日报无不良数量）</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>任务</returns>
    public static async Task RefreshOrderDefectStatAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodCategory)
            || string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        var reports = await BuildStatSnapshotsForOrderAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            assyDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            prodOrderCode);
        await UpsertOrderDefectStatAsync(
            assyOrderDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            prodOrderCode,
            reports);
    }

    /// <summary>
    /// 刷新指定批次的批量不良统计（统计维度：生产类别+批次）
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储（仅用于已手工录入的不良日报无不良数量）</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="batchCode">批次</param>
    /// <returns>任务</returns>
    public static async Task RefreshBatchDefectStatAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string batchCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodCategory)
            || string.IsNullOrWhiteSpace(batchCode))
        {
            return;
        }
        var reports = await BuildStatSnapshotsForBatchAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            assyDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            batchCode);
        await UpsertBatchDefectStatAsync(
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            batchCode,
            reports);
    }

    /// <summary>
    /// 按组立产出维度刷新工单与批量不良统计
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="batchCode">批次</param>
    /// <returns>任务</returns>
    public static async Task RefreshDefectStatsAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string? prodCategory,
        string prodOrderCode,
        string? batchCode)
    {
        var normalizedProdCategory = prodCategory?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedProdCategory))
        {
            return;
        }
        await RefreshOrderDefectStatAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            assyDefectRepository,
            assyOrderDefectRepository,
            tenantCode,
            companyCode,
            normalizedProdCategory,
            prodOrderCode);
        if (!string.IsNullOrWhiteSpace(batchCode))
        {
            await RefreshBatchDefectStatAsync(
                assyOutputRepository,
                assyOutputDetailRepository,
                assyDefectRepository,
                assyBatchDefectRepository,
                tenantCode,
                companyCode,
                normalizedProdCategory,
                batchCode);
        }
    }

    /// <summary>
    /// 按同工单组立日报构建统计快照
    /// </summary>
    private static async Task<List<AssyStatReportSnapshot>> BuildStatSnapshotsForOrderAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string prodOrderCode)
    {
        var outputs = await assyOutputRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.ProdOrderCode == prodOrderCode);
        return await BuildStatSnapshotsAsync(
            assyOutputDetailRepository,
            assyDefectRepository,
            tenantCode,
            companyCode,
            outputs);
    }

    /// <summary>
    /// 按同批次组立日报构建统计快照
    /// </summary>
    private static async Task<List<AssyStatReportSnapshot>> BuildStatSnapshotsForBatchAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string batchCode)
    {
        var outputs = await assyOutputRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.BatchCode == batchCode);
        return await BuildStatSnapshotsAsync(
            assyOutputDetailRepository,
            assyDefectRepository,
            tenantCode,
            companyCode,
            outputs);
    }

    /// <summary>
    /// 将组立日报列表转为统计快照（生实实绩取自产出明细；无不良日报时无不良数量=生实实绩）
    /// </summary>
    private static async Task<List<AssyStatReportSnapshot>> BuildStatSnapshotsAsync(
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        string tenantCode,
        string companyCode,
        IReadOnlyList<TaktAssyOutput> outputs)
    {
        ArgumentNullException.ThrowIfNull(outputs);
        if (outputs.Count == 0)
        {
            return [];
        }
        var outputIds = outputs.Select(x => x.Id).ToList();
        var allDetails = await assyOutputDetailRepository.GetListAsync(x => outputIds.Contains(x.AssyOutputId));
        var detailsByOutputId = allDetails
            .GroupBy(x => x.AssyOutputId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var prodOrderCodes = outputs
            .Select(x => x.ProdOrderCode)
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var defects = await assyDefectRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && prodOrderCodes.Contains(x.ProdOrderCode));
        var defectByDailyOrder = defects.ToDictionary(
            x => BuildDailyOrderKey(x.ProdDate, x.ProdOrderCode),
            x => x);
        var snapshots = new List<AssyStatReportSnapshot>(outputs.Count);
        foreach (var output in outputs)
        {
            var details = detailsByOutputId.GetValueOrDefault(output.Id);
            var prodActualQty = details?.Sum(x => x.ProdActualQty) ?? 0;
            var dailyKey = BuildDailyOrderKey(output.ProdDate, output.ProdOrderCode);
            var goodQuantity = defectByDailyOrder.TryGetValue(dailyKey, out var defect)
                ? defect.GoodQuantity
                : prodActualQty;
            snapshots.Add(new AssyStatReportSnapshot
            {
                Id = output.Id,
                PlantCode = output.PlantCode,
                ProdCategory = output.ProdCategory,
                ProdDate = output.ProdDate,
                ProdOrderCode = output.ProdOrderCode,
                ModelCode = output.ModelCode,
                BatchCode = output.BatchCode,
                MaterialCode = output.MaterialCode,
                ProdOrderQty = output.ProdOrderQty,
                ProdActualQty = prodActualQty,
                GoodQuantity = goodQuantity,
            });
        }
        return snapshots;
    }

    /// <summary>
    /// 生产日期+工单号复合键（yyyy-MM-dd|工单号）
    /// </summary>
    private static string BuildDailyOrderKey(DateTime prodDate, string prodOrderCode)
    {
        return $"{prodDate.Date:yyyy-MM-dd}|{prodOrderCode}";
    }

    /// <summary>
    /// 计算批次工单总数量（同工单取最大订单数量，再按工单合计）
    /// </summary>
    /// <param name="reports">同批次组立日报快照列表</param>
    /// <returns>批次工单总数量</returns>
    private static decimal CalculateBatchOrderTotalQty(IReadOnlyList<AssyStatReportSnapshot> reports)
    {
        ArgumentNullException.ThrowIfNull(reports);
        if (reports.Count == 0)
        {
            return 0;
        }
        return reports
            .GroupBy(x => x.ProdOrderCode, StringComparer.Ordinal)
            .Sum(g => g.Max(x => x.ProdOrderQty));
    }

    /// <summary>
    /// 构建工单生产日期组（日报去重生产日期，yyyy-MM-dd 逗号分隔升序）
    /// </summary>
    /// <param name="reports">同工单组立日报快照列表</param>
    /// <returns>生产日期组</returns>
    private static string BuildOrderProdDateGroup(IReadOnlyList<AssyStatReportSnapshot> reports)
    {
        ArgumentNullException.ThrowIfNull(reports);
        if (reports.Count == 0)
        {
            return string.Empty;
        }
        return string.Join(",", reports
            .Select(x => x.ProdDate.Date)
            .Distinct()
            .OrderBy(d => d)
            .Select(d => d.ToString("yyyy-MM-dd")));
    }

    /// <summary>
    /// 构建批次组快照（生产日期组、生产工单组、生产物料组、订单数量组，四组逗号分隔且位置一一对应）
    /// </summary>
    /// <param name="reports">同批次组立日报快照列表</param>
    /// <returns>生产日期组、生产工单组、生产物料组、订单数量组</returns>
    private static (string ProdDateGroup, string ProdOrderGroup, string MaterialGroup, string ProdOrderQtyGroup) BuildBatchGroupSnapshot(
        IReadOnlyList<AssyStatReportSnapshot> reports)
    {
        ArgumentNullException.ThrowIfNull(reports);
        if (reports.Count == 0)
        {
            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }
        var items = reports
            .GroupBy(x => x.ProdOrderCode, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var latest = g.OrderByDescending(x => x.ProdDate).ThenByDescending(x => x.Id).First();
                return new
                {
                    ProdOrderCode = g.Key,
                    ProdDate = g.Min(x => x.ProdDate),
                    ProdOrderQty = g.Max(x => x.ProdOrderQty),
                    MaterialCode = latest.MaterialCode,
                };
            })
            .ToList();
        var prodDateGroup = string.Join(",", items.Select(x => x.ProdDate.ToString("yyyy-MM-dd")));
        var prodOrderGroup = string.Join(",", items.Select(x => x.ProdOrderCode));
        var materialGroup = string.Join(",", items.Select(x => x.MaterialCode));
        var prodOrderQtyGroup = string.Join(",", items.Select(x => x.ProdOrderQty.ToString("0.###")));
        return (prodDateGroup, prodOrderGroup, materialGroup, prodOrderQtyGroup);
    }

    /// <summary>
    /// 将组立不良日报列表转为统计快照
    /// </summary>
    /// <param name="defects">组立不良日报列表</param>
    /// <returns>统计快照列表</returns>
    private static List<AssyStatReportSnapshot> BuildStatSnapshotsFromDefects(IReadOnlyList<TaktAssyDefect> defects)
    {
        ArgumentNullException.ThrowIfNull(defects);
        if (defects.Count == 0)
        {
            return [];
        }
        return defects.Select(defect => new AssyStatReportSnapshot
        {
            Id = defect.Id,
            PlantCode = defect.PlantCode,
            ProdCategory = defect.ProdCategory,
            ProdDate = defect.ProdDate,
            ProdOrderCode = defect.ProdOrderCode,
            ModelCode = defect.ModelCode,
            BatchCode = defect.BatchCode,
            MaterialCode = defect.MaterialCode,
            ProdOrderQty = defect.ProdOrderQty,
            ProdActualQty = defect.ProdActualQty,
            GoodQuantity = defect.GoodQuantity,
        }).ToList();
    }

    /// <summary>
    /// 写入或更新工单不良统计行
    /// </summary>
    private static async Task UpsertOrderDefectStatAsync(
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string prodOrderCode,
        List<AssyStatReportSnapshot> reports)
    {
        var existing = await assyOrderDefectRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.ProdOrderCode == prodOrderCode);
        if (reports.Count == 0)
        {
            if (existing != null)
            {
                await assyOrderDefectRepository.DeleteAsync(existing.Id);
            }
            return;
        }
        var latest = reports.OrderByDescending(x => x.ProdDate).ThenByDescending(x => x.Id).First();
        var prodActualQty = reports.Sum(x => x.ProdActualQty);
        var goodQuantity = reports.Sum(x => x.GoodQuantity);
        var defectQty = Math.Max(0, prodActualQty - goodQuantity);
        var stat = existing ?? new TaktAssyOrderDefect
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            ProdCategory = prodCategory,
            ProdOrderCode = prodOrderCode,
        };
        stat.PlantCode = latest.PlantCode;
        stat.ProdDateGroup = BuildOrderProdDateGroup(reports);
        stat.ModelCode = latest.ModelCode;
        stat.MaterialCode = latest.MaterialCode;
        stat.BatchCode = latest.BatchCode;
        stat.ProdOrderQty = latest.ProdOrderQty;
        stat.ProdActualQty = prodActualQty;
        stat.GoodQuantity = goodQuantity;
        stat.DefectQty = defectQty;
        stat.DefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(defectQty, prodActualQty);
        stat.YieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(goodQuantity, prodActualQty);
        stat.LastProdDate = reports.Max(x => x.ProdDate);
        stat.ReportCount = reports.Count;
        stat.OrderStatus = TaktDefectStatHelper.ResolveOrderProdStatus(latest.ProdOrderQty, prodActualQty);
        if (existing == null)
        {
            await assyOrderDefectRepository.CreateAsync(stat);
        }
        else
        {
            await assyOrderDefectRepository.UpdateAsync(stat);
        }
    }

    /// <summary>
    /// 写入或更新批量不良统计行
    /// </summary>
    private static async Task UpsertBatchDefectStatAsync(
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string prodCategory,
        string batchCode,
        List<AssyStatReportSnapshot> reports)
    {
        var existing = await assyBatchDefectRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdCategory == prodCategory
            && x.BatchCode == batchCode);
        if (reports.Count == 0)
        {
            if (existing != null)
            {
                await assyBatchDefectRepository.DeleteAsync(existing.Id);
            }
            return;
        }
        var latest = reports.OrderByDescending(x => x.ProdDate).ThenByDescending(x => x.Id).First();
        var batchOrderQty = CalculateBatchOrderTotalQty(reports);
        var (prodDateGroup, prodOrderGroup, materialGroup, prodOrderQtyGroup) = BuildBatchGroupSnapshot(reports);
        var prodActualQty = reports.Sum(x => x.ProdActualQty);
        var goodQuantity = reports.Sum(x => x.GoodQuantity);
        var defectQty = Math.Max(0, prodActualQty - goodQuantity);
        var stat = existing ?? new TaktAssyBatchDefect
        {
            TenantCode = tenantCode,
            CompanyCode = companyCode,
            ProdCategory = prodCategory,
            BatchCode = batchCode,
        };
        stat.PlantCode = latest.PlantCode;
        stat.ProdDateGroup = prodDateGroup;
        stat.ProdOrderGroup = prodOrderGroup;
        stat.ModelCode = latest.ModelCode;
        stat.MaterialGroup = materialGroup;
        stat.BatchOrderQty = batchOrderQty;
        stat.ProdOrderQtyGroup = prodOrderQtyGroup;
        stat.ProdActualQty = prodActualQty;
        stat.GoodQuantity = goodQuantity;
        stat.DefectQty = defectQty;
        stat.DefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(defectQty, prodActualQty);
        stat.YieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(goodQuantity, prodActualQty);
        stat.LastProdDate = reports.Max(x => x.ProdDate);
        stat.ReportCount = reports.Count;
        stat.BatchStatus = TaktDefectStatHelper.ResolveBatchProdStatus(batchOrderQty, prodActualQty);
        if (existing == null)
        {
            await assyBatchDefectRepository.CreateAsync(stat);
        }
        else
        {
            await assyBatchDefectRepository.UpdateAsync(stat);
        }
    }
}

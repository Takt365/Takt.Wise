// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyOutputDefectSyncHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报（产出）与工单·批量不良统计级联同步（不自动生成组立不良日报）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立产出 → 工单/批量不良统计 级联同步辅助（组立不良日报须手工维护）
/// </summary>
internal static class TaktAssyOutputDefectSyncHelper
{
    /// <summary>
    /// 组立不良日报保存/删除后刷新工单与批量不良统计（汇总 TaktAssyDefect）
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="defect">组立不良日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task SyncFromAssyDefectAsync(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        TaktAssyDefect defect,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(defect);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        await TaktAssyDefectStatSyncHelper.RefreshDefectStatsFromAssyDefectAsync(
            assyDefectRepository,
            assyOrderDefectRepository,
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            defect.ProdCategory,
            defect.ProdOrderCode,
            defect.BatchCode);
    }

    /// <summary>
    /// 按统计维度刷新工单与批量不良统计（汇总 TaktAssyDefect；主表维度变更时刷新旧键）
    /// </summary>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="batchCode">批次</param>
    /// <returns>任务</returns>
    public static async Task SyncDefectStatsForDimensionAsync(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        string tenantCode,
        string companyCode,
        string? prodCategory,
        string? prodOrderCode,
        string? batchCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodCategory) || string.IsNullOrWhiteSpace(prodOrderCode))
        {
            return;
        }
        await TaktAssyDefectStatSyncHelper.RefreshDefectStatsFromAssyDefectAsync(
            assyDefectRepository,
            assyOrderDefectRepository,
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            prodCategory,
            prodOrderCode,
            batchCode);
    }

    /// <summary>
    /// 按组立日报主表刷新工单/批量不良统计（不 upsert 组立不良日报）
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储（保留参数以兼容调用方，产出同步不使用）</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="output">组立日报主表</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task SyncFromAssyOutputAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        TaktAssyOutput output,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        _ = assyDefectDetailRepository;
        await TaktAssyDefectStatSyncHelper.RefreshDefectStatsAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            assyDefectRepository,
            assyOrderDefectRepository,
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            output.ProdCategory,
            output.ProdOrderCode,
            output.BatchCode);
    }

    /// <summary>
    /// 删除组立日报时仅刷新工单/批量不良统计（不删除手工维护的组立不良日报）
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储（保留参数以兼容调用方，产出同步不使用）</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="output">已删除或待删除的组立日报快照</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task DeleteDefectForAssyOutputAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        TaktAssyOutput output,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        _ = assyDefectDetailRepository;
        await TaktAssyDefectStatSyncHelper.RefreshDefectStatsAsync(
            assyOutputRepository,
            assyOutputDetailRepository,
            assyDefectRepository,
            assyOrderDefectRepository,
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            output.ProdCategory,
            output.ProdOrderCode,
            output.BatchCode);
    }

    /// <summary>
    /// 组立不良明细变更后重算主表无不良数量并刷新工单/批量统计
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="assyOutputDetailRepository">组立日报明细仓储</param>
    /// <param name="assyDefectRepository">组立不良日报仓储</param>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="assyOrderDefectRepository">工单不良统计仓储</param>
    /// <param name="assyBatchDefectRepository">批量不良统计仓储</param>
    /// <param name="assyDefectId">组立不良日报 ID</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task SyncFromAssyDefectDetailChangeAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktCompanyRepository<TaktAssyOrderDefect> assyOrderDefectRepository,
        ITaktCompanyRepository<TaktAssyBatchDefect> assyBatchDefectRepository,
        long assyDefectId,
        string tenantCode,
        string companyCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (assyDefectId <= 0)
        {
            return;
        }
        var defect = await assyDefectRepository.GetByIdAsync(assyDefectId);
        if (defect == null
            || defect.TenantCode != tenantCode
            || defect.CompanyCode != companyCode)
        {
            return;
        }
        var output = await FindAssyOutputByDailyOrderAsync(
            assyOutputRepository,
            tenantCode,
            companyCode,
            defect.ProdDate,
            defect.ProdOrderCode);
        if (output != null)
        {
            await RecalculateAndPersistAssyDefectQuantitiesAsync(
                assyOutputDetailRepository,
                assyDefectDetailRepository,
                defect,
                output.Id);
        }
        else
        {
            await SyncAssyDefectDetailRedundantQuantitiesAsync(
                assyDefectDetailRepository,
                defect);
        }
        await assyDefectRepository.UpdateAsync(defect);
        await TaktAssyDefectStatSyncHelper.RefreshDefectStatsFromAssyDefectAsync(
            assyDefectRepository,
            assyOrderDefectRepository,
            assyBatchDefectRepository,
            tenantCode,
            companyCode,
            defect.ProdCategory,
            defect.ProdOrderCode,
            defect.BatchCode);
    }

    /// <summary>
    /// 按「生产日期 + 工单号」查找组立日报
    /// </summary>
    private static Task<TaktAssyOutput?> FindAssyOutputByDailyOrderAsync(
        ITaktCompanyRepository<TaktAssyOutput> repository,
        string tenantCode,
        string companyCode,
        DateTime prodDate,
        string prodOrderCode)
    {
        var dateOnly = prodDate.Date;
        return repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate == dateOnly
            && x.ProdOrderCode == prodOrderCode);
    }

    /// <summary>
    /// 按组立产出明细合计重写生实实绩，并将主表生实/无不良冗余同步到不良明细（不根据子表不良合计改写无不良数量）
    /// </summary>
    private static async Task RecalculateAndPersistAssyDefectQuantitiesAsync(
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        TaktAssyDefect defect,
        long assyOutputId)
    {
        ArgumentNullException.ThrowIfNull(defect);
        var outputDetails = await assyOutputDetailRepository.GetListAsync(x => x.AssyOutputId == assyOutputId);
        defect.ProdActualQty = outputDetails.Sum(x => x.ProdActualQty);
        await SyncAssyDefectDetailRedundantQuantitiesAsync(assyDefectDetailRepository, defect);
    }

    /// <summary>
    /// 将主表生实实绩、无不良数量同步到全部不良明细冗余字段
    /// </summary>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="defect">组立不良日报主表</param>
    /// <returns>任务</returns>
    private static async Task SyncAssyDefectDetailRedundantQuantitiesAsync(
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        TaktAssyDefect defect)
    {
        ArgumentNullException.ThrowIfNull(defect);
        if (defect.Id <= 0)
        {
            return;
        }
        var details = await assyDefectDetailRepository.GetListAsync(
            x => x.AssyDefectId == defect.Id && x.IsObsolete == 0);
        if (details.Count == 0)
        {
            return;
        }
        foreach (var detail in details)
        {
            detail.ProdActualQty = defect.ProdActualQty;
            detail.GoodQuantity = defect.GoodQuantity;
        }
        await assyDefectDetailRepository.UpdateRangeAsync(details);
    }
}

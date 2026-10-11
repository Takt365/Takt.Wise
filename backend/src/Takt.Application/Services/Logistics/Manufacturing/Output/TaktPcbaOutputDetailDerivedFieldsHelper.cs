// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailDerivedFieldsHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报明细派生字段计算与累计完成数桶刷新（按工单号+班次+PCB板别+面板别）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报明细派生字段计算辅助（服务层编排，含仓储查询）
/// </summary>
internal static class TaktPcbaOutputDetailDerivedFieldsHelper
{
    /// <summary>
    /// 写入明细投入工数、实际工时与达成率（基于明细人员标准产能）
    /// </summary>
    /// <param name="detail">PCBA日报明细</param>
    public static void ApplyLaborDerivedFields(TaktPcbaOutputDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        detail.InputMinutes = detail.DirectLabor > 0 ? detail.DirectLabor * 60m : 0m;
        if (detail.MixedProd == 0)
        {
            detail.ActualMinutes = Math.Max(0m, detail.InputMinutes - detail.DowntimeMinutes);
        }
        else
        {
            detail.ActualMinutes = Math.Max(0m, detail.ConfirmMinutes - detail.DowntimeMinutes);
        }
        detail.AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(
            detail.DailyCompletedQty,
            detail.StdLaborCapacity);
    }

    /// <summary>
    /// 按桶内当日完成数合计写入单条明细累计完成数与完成状态
    /// </summary>
    /// <param name="detail">PCBA日报明细</param>
    /// <param name="bucketTotalCompletedQty">桶内累计完成数</param>
    public static void ApplyTotalCompletedFields(TaktPcbaOutputDetail detail, decimal bucketTotalCompletedQty)
    {
        ArgumentNullException.ThrowIfNull(detail);
        detail.TotalCompletedQty = bucketTotalCompletedQty;
        detail.CompletedStatus = TaktProductionStatHelper.ResolvePcbaCompletedStatus(
            bucketTotalCompletedQty,
            detail.BatchQty);
    }

    /// <summary>
    /// 刷新同一工单号、班次、PCB板别、面板别桶内全部明细的累计完成数与完成状态
    /// </summary>
    /// <param name="detailRepository">PCBA日报明细仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <param name="shiftNo">班次</param>
    /// <param name="pcbBoardType">PCB板别</param>
    /// <param name="panelSide">面板别</param>
    /// <returns>任务</returns>
    public static async Task RefreshTotalCompletedBucketAsync(
        ITaktCompanyRepository<TaktPcbaOutputDetail> detailRepository,
        string tenantCode,
        string companyCode,
        string prodOrderCode,
        int shiftNo,
        string pcbBoardType,
        string panelSide)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(prodOrderCode)
            || string.IsNullOrWhiteSpace(pcbBoardType)
            || string.IsNullOrWhiteSpace(panelSide))
        {
            return;
        }
        var orderCode = prodOrderCode.Trim();
        var boardType = pcbBoardType.Trim();
        var side = panelSide.Trim();
        var details = await detailRepository.GetListAsync(d =>
            d.TenantCode == tenantCode
            && d.CompanyCode == companyCode
            && d.ProdOrderCode == orderCode
            && d.ShiftNo == shiftNo
            && d.PcbBoardType == boardType
            && d.PanelSide == side);
        if (details.Count == 0)
        {
            return;
        }
        var bucketTotal = TaktProductionStatHelper.CalculatePcbaTotalCompletedQty(
            details.Select(d => d.DailyCompletedQty));
        foreach (var detail in details)
        {
            ApplyTotalCompletedFields(detail, bucketTotal);
        }
        await detailRepository.UpdateRangeAsync(details);
    }

    /// <summary>
    /// 按明细集合去重刷新多个累计完成数统计桶
    /// </summary>
    /// <param name="detailRepository">PCBA日报明细仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="details">明细集合</param>
    /// <returns>任务</returns>
    public static async Task RefreshTotalCompletedBucketsForDetailsAsync(
        ITaktCompanyRepository<TaktPcbaOutputDetail> detailRepository,
        string tenantCode,
        string companyCode,
        IEnumerable<TaktPcbaOutputDetail> details)
    {
        ArgumentNullException.ThrowIfNull(details);
        var seenBuckets = new HashSet<string>(StringComparer.Ordinal);
        foreach (var detail in details)
        {
            var bucketKey = TryGetCompletionBucketKey(detail);
            if (bucketKey == null)
            {
                continue;
            }
            var key = bucketKey.Value;
            var token = $"{key.ProdOrderCode}|{key.ShiftNo}|{key.PcbBoardType}|{key.PanelSide}";
            if (!seenBuckets.Add(token))
            {
                continue;
            }
            await RefreshTotalCompletedBucketAsync(
                detailRepository,
                tenantCode,
                companyCode,
                key.ProdOrderCode,
                key.ShiftNo,
                key.PcbBoardType,
                key.PanelSide);
        }
    }

    /// <summary>
    /// 从明细行解析累计完成数统计桶键
    /// </summary>
    /// <param name="detail">PCBA日报明细</param>
    /// <returns>桶键；工单号或板别为空时返回 null</returns>
    public static PcbaCompletionBucketKey? TryGetCompletionBucketKey(TaktPcbaOutputDetail detail)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (string.IsNullOrWhiteSpace(detail.ProdOrderCode)
            || string.IsNullOrWhiteSpace(detail.PcbBoardType)
            || string.IsNullOrWhiteSpace(detail.PanelSide))
        {
            return null;
        }
        return new PcbaCompletionBucketKey(
            detail.ProdOrderCode.Trim(),
            detail.ShiftNo,
            detail.PcbBoardType.Trim(),
            detail.PanelSide.Trim());
    }

    /// <summary>
    /// PCBA 累计完成数统计桶键（工单号+班次+PCB板别+面板别）
    /// </summary>
    /// <param name="ProdOrderCode">工单号</param>
    /// <param name="ShiftNo">班次</param>
    /// <param name="PcbBoardType">PCB板别</param>
    /// <param name="PanelSide">面板别</param>
    internal readonly record struct PcbaCompletionBucketKey(
        string ProdOrderCode,
        int ShiftNo,
        string PcbBoardType,
        string PanelSide);
}

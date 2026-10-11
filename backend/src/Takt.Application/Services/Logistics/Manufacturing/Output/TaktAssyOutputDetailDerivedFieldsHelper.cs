// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputDetailDerivedFieldsHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：组立日报明细派生字段计算与混合生产桶刷新（投入/实际工时、MixedProd、混合生产备注、达成率）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立日报明细派生字段计算辅助（服务层编排，含仓储查询）
/// </summary>
internal static class TaktAssyOutputDetailDerivedFieldsHelper
{
    /// <summary>
    /// 按主表与混合生产笔数写入明细派生字段（投入工时、标准产能、实际工时、间接工时、MixedProd、达成率）
    /// </summary>
    /// <param name="detail">组立日报明细</param>
    /// <param name="master">组立日报主表</param>
    /// <param name="mixedProdCount">混合生产笔数（同时段有产量/报工总数，仅对有产量/报工行生效）</param>
    /// <param name="operationRatePercent">标准生产稼动率(%)</param>
    /// <param name="mixedProdBucketRemark">混合生产桶备注；传入时仅对有产量/报工行写入 Remark</param>
    public static void ApplyCalculatedFields(
        TaktAssyOutputDetail detail,
        TaktAssyOutput master,
        int mixedProdCount,
        decimal operationRatePercent,
        string? mixedProdBucketRemark = null)
    {
        ArgumentNullException.ThrowIfNull(detail);
        ArgumentNullException.ThrowIfNull(master);
        ApplyCleaningTimePeriodDefaults(detail, master.DirectLabor);
        detail.InputMinutes = TaktProductionStatHelper.CalculateAssyInputMinutes(
            master.DirectLabor,
            detail.ConfirmMinutes,
            detail.ProdActualQty);
        detail.StdCapacity = TaktProductionStatHelper.CalculateAssyDetailStdCapacity(
            master.StdMinutes,
            master.StdCapacity,
            detail.ConfirmMinutes,
            operationRatePercent,
            detail.ProdActualQty);
        detail.ActualMinutes = TaktProductionStatHelper.CalculateAssyActualMinutes(
            detail.InputMinutes,
            detail.ConfirmMinutes,
            detail.DowntimeMinutes,
            detail.ProdActualQty);
        detail.IndirectMinutes = TaktProductionStatHelper.CalculateAssyIndirectMinutes(
            master.IndirectLabor,
            master.DirectLabor,
            detail.ActualMinutes,
            detail.ConfirmMinutes,
            detail.ProdActualQty);
        detail.AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(
            detail.ProdActualQty,
            detail.StdCapacity);
        ApplyMixedProdFields(detail, mixedProdCount, mixedProdBucketRemark);
    }

    /// <summary>
    /// 清洁时段：仅当实际生产数量 > 0 时写入停线原因=清洁、停线时间=直接人员×4分钟
    /// </summary>
    /// <param name="detail">组立日报明细</param>
    /// <param name="directLabor">主表直接人员</param>
    public static void ApplyCleaningTimePeriodDefaults(TaktAssyOutputDetail detail, int directLabor)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (!TaktProductionStatHelper.IsAssyCleaningTimePeriod(detail.TimePeriod))
        {
            return;
        }
        if (detail.ProdActualQty <= 0)
        {
            detail.DowntimeMinutes = 0;
            detail.DowntimeReason = string.Empty;
            return;
        }
        detail.DowntimeReason = TaktAssyOutputTimePeriodConstants.CleaningStopReasonDictLabel;
        detail.DowntimeMinutes = TaktProductionStatHelper.CalculateAssyCleaningDowntimeMinutes(directLabor);
    }

    /// <summary>
    /// 仅写入混合生产笔数与混合生产备注（不改动投入/实际工时等其他派生字段）
    /// </summary>
    /// <param name="detail">组立日报明细</param>
    /// <param name="mixedProdCount">同桶内有产量/报工明细总数（≥2 为混合）</param>
    /// <param name="mixedProdBucketRemark">混合生产桶备注；null 表示不改动 Remark</param>
    public static void ApplyMixedProdFields(
        TaktAssyOutputDetail detail,
        int mixedProdCount,
        string? mixedProdBucketRemark = null)
    {
        ArgumentNullException.ThrowIfNull(detail);
        if (TaktProductionStatHelper.IsAssyDetailWithoutProduction(detail.ProdActualQty, detail.ConfirmMinutes))
        {
            detail.MixedProd = 0;
            if (TaktProductionStatHelper.IsAssyMixedProdAutoRemark(detail.Remark))
            {
                detail.Remark = string.Empty;
            }
            return;
        }
        detail.MixedProd = mixedProdCount;
        if (mixedProdBucketRemark != null)
        {
            detail.Remark = mixedProdCount >= 2 ? mixedProdBucketRemark : string.Empty;
        }
    }

    /// <summary>
    /// 刷新同一生产日期、生产班组、生产时段桶内有产量/报工明细的 MixedProd 与混合生产备注（不重算工时/达成率）
    /// </summary>
    /// <param name="assyOutputRepository">组立日报仓储</param>
    /// <param name="detailRepository">组立日报明细仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="TeamCode">生产班组</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="timePeriod">生产时段</param>
    /// <returns>任务</returns>
    public static async Task RefreshMixedProdBucketAsync(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> detailRepository,
        string tenantCode,
        string companyCode,
        string TeamCode,
        DateTime prodDate,
        string timePeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(TeamCode) || string.IsNullOrWhiteSpace(timePeriod))
        {
            return;
        }
        var prodDateOnly = prodDate.Date;
        var masters = await assyOutputRepository.GetListAsync(m =>
            m.TenantCode == tenantCode
            && m.CompanyCode == companyCode
            && m.TeamCode == TeamCode
            && m.ProdDate == prodDateOnly);
        if (masters.Count == 0)
        {
            return;
        }
        var masterById = masters.ToDictionary(m => m.Id);
        var masterIds = masterById.Keys.ToList();
        var details = await detailRepository.GetListAsync(d =>
            masterIds.Contains(d.AssyOutputId)
            && d.TimePeriod == timePeriod);
        if (details.Count == 0)
        {
            return;
        }
        var activeDetails = details
            .Where(d => !TaktProductionStatHelper.IsAssyDetailWithoutProduction(d.ProdActualQty, d.ConfirmMinutes))
            .ToList();
        var mixedProdCount = TaktProductionStatHelper.CalculateAssyMixedProdCount(activeDetails.Count);
        var mixedProdBucketRemark = TaktProductionStatHelper.BuildAssyMixedProdBucketRemark(
            activeDetails.Select(d =>
            {
                var orderCode = masterById.TryGetValue(d.AssyOutputId, out var masterEntry)
                    ? masterEntry.ProdOrderCode
                    : d.ProdOrderCode;
                return (orderCode, d.TimePeriod);
            }));
        var detailsToUpdate = new List<TaktAssyOutputDetail>();
        foreach (var detail in details)
        {
            if (TaktProductionStatHelper.IsAssyDetailWithoutProduction(detail.ProdActualQty, detail.ConfirmMinutes))
            {
                if (detail.MixedProd != 0
                    || TaktProductionStatHelper.IsAssyMixedProdAutoRemark(detail.Remark))
                {
                    detail.MixedProd = 0;
                    detail.Remark = string.Empty;
                    detailsToUpdate.Add(detail);
                }
                continue;
            }
            var previousMixedProd = detail.MixedProd;
            var previousRemark = detail.Remark;
            ApplyMixedProdFields(detail, mixedProdCount, mixedProdBucketRemark);
            if (detail.MixedProd != previousMixedProd || detail.Remark != previousRemark)
            {
                detailsToUpdate.Add(detail);
            }
        }
        if (detailsToUpdate.Count > 0)
        {
            await detailRepository.UpdateRangeAsync(detailsToUpdate);
        }
    }
}

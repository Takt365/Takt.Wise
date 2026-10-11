// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 产出看板统计服务（与 TaktPcbaOutput CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA 产出看板统计服务（读产出明细；与 CRUD 分离）
/// </summary>
public class TaktPcbaOutputStatService : TaktServiceBase, ITaktPcbaOutputStatService
{
    private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputDetailRepository">PCBA 产出明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaOutputStatService(
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktPcbaOutputProductionStatDto> GetPcbaOutputProductionStatAsync(TaktOutputProductionStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPcbaOutputDetail, bool>> detailPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktPcbaOutput>()
                .Where(h =>
                    h.Id == x.PcbaOutputId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.ProdDate >= start
                    && h.ProdDate <= end
                    && h.IsDeleted == 0)
                .Any();
        var details = await _pcbaOutputDetailRepository.GetListAsync(detailPredicate);
        var teams = details
            .GroupBy(d => d.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var plan = g.Sum(d => d.StdLaborCapacity);
                var actual = g.Sum(d => d.DailyCompletedQty);
                return new TaktOutputProductionStatTeamItemDto
                {
                    TeamCode = g.Key,
                    StdCapacity = plan,
                    ProdActualQty = actual,
                    AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(actual, plan),
                };
            })
            .ToList();
        var monthStdCapacity = teams.Sum(t => t.StdCapacity);
        var monthProdActualQty = teams.Sum(t => t.ProdActualQty);
        var monthStopTime = details.Sum(d => d.StopTime);
        var monthSwitchTime = details.Sum(d => d.SwitchTime);
        var monthInputMinutes = details.Sum(d => d.InputMinutes);
        var monthProdMinutes = details.Sum(d => d.TotalMinutes);
        var monthRepairMinutes = details.Sum(d => d.RepairMinutes);
        return new TaktPcbaOutputProductionStatDto
        {
            StatMonth = statMonth,
            MonthStdCapacity = monthStdCapacity,
            MonthProdActualQty = monthProdActualQty,
            MonthAchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(monthProdActualQty, monthStdCapacity),
            MonthDowntimeMinutes = monthStopTime + monthSwitchTime,
            MonthInputMinutes = monthInputMinutes,
            MonthProdMinutes = monthProdMinutes,
            MonthActualMinutes = monthInputMinutes + monthRepairMinutes,
            Teams = teams,
        };
    }
}

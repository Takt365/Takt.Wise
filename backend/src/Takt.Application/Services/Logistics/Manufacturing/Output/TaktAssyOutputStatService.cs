// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：组立产出看板统计服务（与 TaktAssyOutput CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 组立产出看板统计服务（读产出主从表；与 CRUD 分离）
/// </summary>
public class TaktAssyOutputStatService : TaktServiceBase, ITaktAssyOutputStatService
{
    private readonly ITaktCompanyRepository<TaktAssyOutput> _assyOutputRepository;
    private readonly ITaktCompanyRepository<TaktAssyOutputDetail> _assyOutputDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputRepository">组立产出主表仓储</param>
    /// <param name="assyOutputDetailRepository">组立产出明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyOutputStatService(
        ITaktCompanyRepository<TaktAssyOutput> assyOutputRepository,
        ITaktCompanyRepository<TaktAssyOutputDetail> assyOutputDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyOutputRepository = assyOutputRepository;
        _assyOutputDetailRepository = assyOutputDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktAssyOutputProductionStatDto> GetAssyOutputProductionStatAsync(TaktOutputProductionStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktAssyOutput, bool>> headerPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var headers = await _assyOutputRepository.GetListAsync(headerPredicate);
        var outputIds = headers.Select(h => h.Id).ToList();
        var details = outputIds.Count == 0
            ? new List<TaktAssyOutputDetail>()
            : await _assyOutputDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && outputIds.Contains(x.AssyOutputId));
        var qtyByOutputId = details
            .GroupBy(d => d.AssyOutputId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.ProdActualQty));
        var teams = headers
            .GroupBy(h => h.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var plan = g.Sum(h => h.StdCapacity);
                var actual = g.Sum(h => qtyByOutputId.TryGetValue(h.Id, out var qty) ? qty : 0m);
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
        var monthDowntimeMinutes = details.Sum(d => (decimal)d.DowntimeMinutes);
        var monthInputMinutes = details.Sum(d => d.InputMinutes);
        var monthProdMinutes = details.Sum(d => d.ConfirmMinutes);
        var monthActualMinutes = details.Sum(d => d.ActualMinutes);
        return new TaktAssyOutputProductionStatDto
        {
            StatMonth = statMonth,
            MonthStdCapacity = monthStdCapacity,
            MonthProdActualQty = monthProdActualQty,
            MonthAchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(monthProdActualQty, monthStdCapacity),
            MonthDowntimeMinutes = monthDowntimeMinutes,
            MonthInputMinutes = monthInputMinutes,
            MonthProdMinutes = monthProdMinutes,
            MonthActualMinutes = monthActualMinutes,
            Teams = teams,
        };
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktAssyDefectStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：组立不良看板统计服务（与 TaktAssyDefect CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// 组立不良看板统计服务（读不良主从表；与 CRUD 分离）
/// </summary>
public class TaktAssyDefectStatService : TaktServiceBase, ITaktAssyDefectStatService
{
    private readonly ITaktCompanyRepository<TaktAssyDefect> _assyDefectRepository;
    private readonly ITaktCompanyRepository<TaktAssyDefectDetail> _assyDefectDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyDefectRepository">组立不良主表仓储</param>
    /// <param name="assyDefectDetailRepository">组立不良明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAssyDefectStatService(
        ITaktCompanyRepository<TaktAssyDefect> assyDefectRepository,
        ITaktCompanyRepository<TaktAssyDefectDetail> assyDefectDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _assyDefectRepository = assyDefectRepository;
        _assyDefectDetailRepository = assyDefectDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktAssyDefectStatDto> GetAssyDefectStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktAssyDefect, bool>> headerPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var headers = await _assyDefectRepository.GetListAsync(headerPredicate);
        var defectIds = headers.Select(h => h.Id).ToList();
        var details = defectIds.Count == 0
            ? new List<TaktAssyDefectDetail>()
            : await _assyDefectDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && defectIds.Contains(x.AssyDefectId));
        var defectQtyByHeaderId = details
            .GroupBy(d => d.AssyDefectId)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.DefectQty));
        var teams = headers
            .GroupBy(h => h.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var baseQty = g.Sum(h => h.ProdActualQty);
                var goodQty = g.Sum(h => h.GoodQuantity);
                var defectQty = g.Sum(h => defectQtyByHeaderId.TryGetValue(h.Id, out var qty) ? qty : 0m);
                return new TaktDefectStatTeamItemDto
                {
                    TeamCode = g.Key,
                    BaseQty = baseQty,
                    GoodQty = goodQty,
                    DefectQty = defectQty,
                    DefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(defectQty, baseQty),
                    YieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(goodQty, baseQty),
                };
            })
            .ToList();
        var monthBaseQty = teams.Sum(t => t.BaseQty);
        var monthGoodQty = teams.Sum(t => t.GoodQty);
        var monthDefectQty = teams.Sum(t => t.DefectQty);
        return new TaktAssyDefectStatDto
        {
            StatMonth = statMonth,
            MonthBaseQty = monthBaseQty,
            MonthGoodQty = monthGoodQty,
            MonthDefectQty = monthDefectQty,
            MonthDefectRatePercent = TaktDefectStatHelper.CalculateDefectRatePercent(monthDefectQty, monthBaseQty),
            MonthYieldRatePercent = TaktDefectStatHelper.CalculateYieldRatePercent(monthGoodQty, monthBaseQty),
            Teams = teams,
        };
    }
}

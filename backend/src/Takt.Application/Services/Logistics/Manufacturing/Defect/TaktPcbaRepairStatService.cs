// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 改修不良看板统计服务（与 TaktPcbaRepair CRUD 分离）
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
/// PCBA 改修不良看板统计服务（读改修主从表；与 CRUD 分离）
/// </summary>
public class TaktPcbaRepairStatService : TaktServiceBase, ITaktPcbaRepairStatService
{
    private readonly ITaktCompanyRepository<TaktPcbaRepair> _pcbaRepairRepository;
    private readonly ITaktCompanyRepository<TaktPcbaRepairDetail> _pcbaRepairDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairRepository">PCBA 改修主表仓储</param>
    /// <param name="pcbaRepairDetailRepository">PCBA 改修明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaRepairStatService(
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaRepairRepository = pcbaRepairRepository;
        _pcbaRepairDetailRepository = pcbaRepairDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktPcbaRepairStatDto> GetPcbaRepairStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPcbaRepair, bool>> headerPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var headers = await _pcbaRepairRepository.GetListAsync(headerPredicate);
        var repairIds = headers.Select(h => h.Id).ToList();
        var details = repairIds.Count == 0
            ? new List<TaktPcbaRepairDetail>()
            : await _pcbaRepairDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && repairIds.Contains(x.PcbaRepairId));
        var detailsByHeaderId = details
            .GroupBy(d => d.PcbaRepairId)
            .ToDictionary(g => g.Key, g => g.ToList());
        var teams = headers
            .GroupBy(h => h.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var teamDetails = g
                    .SelectMany(h => detailsByHeaderId.TryGetValue(h.Id, out var rows) ? rows : Enumerable.Empty<TaktPcbaRepairDetail>())
                    .ToList();
                var baseQty = teamDetails.Sum(d => d.ProdActualQty);
                var defectQty = teamDetails.Sum(d => d.DefectQty);
                var goodQty = Math.Max(0m, baseQty - defectQty);
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
        var monthDefectQty = teams.Sum(t => t.DefectQty);
        var monthGoodQty = Math.Max(0m, monthBaseQty - monthDefectQty);
        return new TaktPcbaRepairStatDto
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

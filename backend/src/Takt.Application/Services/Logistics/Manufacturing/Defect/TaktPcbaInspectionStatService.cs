// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 检查不良看板统计服务（与 TaktPcbaInspection CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA 检查不良看板统计服务（读检查明细；与 CRUD 分离）
/// </summary>
public class TaktPcbaInspectionStatService : TaktServiceBase, ITaktPcbaInspectionStatService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionDetailRepository">PCBA 检查明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaInspectionStatService(
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktPcbaInspectionStatDto> GetPcbaInspectionStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPcbaInspectionDetail, bool>> detailPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && (
                (x.TSideAssemblyDate != null && x.TSideAssemblyDate >= start && x.TSideAssemblyDate <= end)
                || (x.BSideAssemblyDate != null && x.BSideAssemblyDate >= start && x.BSideAssemblyDate <= end))
            && SqlFunc.Subqueryable<TaktPcbaInspection>()
                .Where(h =>
                    h.Id == x.PcbaInspectionId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.IsDeleted == 0)
                .Any();
        var details = await _pcbaInspectionDetailRepository.GetListAsync(detailPredicate);
        var teams = details
            .GroupBy(d => d.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var baseQty = g.Sum(d => d.InspectionQty);
                var defectQty = g.Sum(d => d.DefectQty);
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
        return new TaktPcbaInspectionStatDto
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

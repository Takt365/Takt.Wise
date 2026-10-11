// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaDefectBoardStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 不良看板统计服务（检查当日完成数量 + 修理不良数量）
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
/// PCBA 不良看板统计服务（读检查/修理明细；与 CRUD 分离）
/// </summary>
public class TaktPcbaDefectBoardStatService : TaktServiceBase, ITaktPcbaDefectBoardStatService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaRepair> _pcbaRepairRepository;
    private readonly ITaktCompanyRepository<TaktPcbaRepairDetail> _pcbaRepairDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionDetailRepository">PCBA 检查明细仓储</param>
    /// <param name="pcbaRepairRepository">PCBA 修理主表仓储</param>
    /// <param name="pcbaRepairDetailRepository">PCBA 修理明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaDefectBoardStatService(
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
        _pcbaRepairRepository = pcbaRepairRepository;
        _pcbaRepairDetailRepository = pcbaRepairDetailRepository;
    }

    /// <inheritdoc />
    public async Task<TaktPcbaDefectBoardStatDto> GetPcbaDefectBoardStatAsync(TaktDefectStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;

        Expression<Func<TaktPcbaInspectionDetail, bool>> inspectionPredicate = x =>
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
        var inspectionDetails = await _pcbaInspectionDetailRepository.GetListAsync(inspectionPredicate);
        var inspectionQty = inspectionDetails.Sum(d => d.DailyCompletedQty);

        Expression<Func<TaktPcbaRepair, bool>> repairHeaderPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.ProdDate >= start
            && x.ProdDate <= end;
        var repairHeaders = await _pcbaRepairRepository.GetListAsync(repairHeaderPredicate);
        var repairIds = repairHeaders.Select(h => h.Id).ToList();
        var repairDetails = repairIds.Count == 0
            ? new List<TaktPcbaRepairDetail>()
            : await _pcbaRepairDetailRepository.GetListAsync(x =>
                x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.IsObsolete == 0
                && repairIds.Contains(x.PcbaRepairId));
        var repairQty = repairDetails.Sum(d => d.DefectQty);

        return new TaktPcbaDefectBoardStatDto
        {
            StatMonth = statMonth,
            InspectionQty = inspectionQty,
            RepairQty = repairQty,
        };
    }
}

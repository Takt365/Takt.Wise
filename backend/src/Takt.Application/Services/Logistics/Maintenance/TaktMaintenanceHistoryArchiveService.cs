// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoryArchiveService.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单完工后将完整维护记录归档至 TaktMaintenanceHistory 履历
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Mapster;
using Newtonsoft.Json;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Domain.Entities.Logistics.Maintenance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 维护工单完工归档至设备维护履历
/// </summary>
public class TaktMaintenanceHistoryArchiveService : TaktServiceBase, ITaktMaintenanceHistoryArchiveService
{
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _workOrderRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> _materialRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> _laborRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceHistory> _maintenanceHistoryRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="workOrderRepository">维护工单仓储</param>
    /// <param name="materialRepository">领料明细仓储</param>
    /// <param name="laborRepository">报工明细仓储</param>
    /// <param name="maintenanceHistoryRepository">维护履历仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceHistoryArchiveService(
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> workOrderRepository,
        ITaktCompanyRepository<TaktMaintenanceWorkOrderMaterial> materialRepository,
        ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> laborRepository,
        ITaktCompanyRepository<TaktMaintenanceHistory> maintenanceHistoryRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _workOrderRepository = workOrderRepository;
        _materialRepository = materialRepository;
        _laborRepository = laborRepository;
        _maintenanceHistoryRepository = maintenanceHistoryRepository;
    }

    /// <summary>
    /// 维护工单状态变更后尝试归档（已完工/已结算/已关闭且未归档时写入履历）
    /// </summary>
    /// <param name="workOrderId">维护工单ID</param>
    /// <returns>归档后的履历 DTO；不满足条件时返回 null</returns>
    public async Task<TaktMaintenanceHistoryDto?> TryArchiveFromWorkOrderAsync(long workOrderId)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
        if (workOrder == null
            || workOrder.TenantCode != CurrentTenantCode
            || workOrder.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }

        var materials = await _materialRepository.GetListAsync(x => x.MaintenanceWorkOrderId == workOrderId);
        var labors = await _laborRepository.GetListAsync(x => x.MaintenanceWorkOrderId == workOrderId);
        return await TryArchiveFromWorkOrderAsync(workOrder, materials, labors);
    }

    /// <summary>
    /// 根据内存中的工单实体归档（含子表数据已加载时使用）
    /// </summary>
    /// <param name="workOrder">维护工单实体</param>
    /// <param name="materials">领料明细</param>
    /// <param name="labors">报工明细</param>
    /// <returns>归档后的履历 DTO；不满足条件时返回 null</returns>
    public async Task<TaktMaintenanceHistoryDto?> TryArchiveFromWorkOrderAsync(
        TaktMaintenanceWorkOrder workOrder,
        IReadOnlyList<TaktMaintenanceWorkOrderMaterial>? materials,
        IReadOnlyList<TaktMaintenanceWorkOrderLabor>? labors)
    {
        ArgumentNullException.ThrowIfNull(workOrder);

        if (!TaktMaintenanceConstants.ShouldArchiveWorkOrderToHistory(workOrder.WorkOrderStatus))
        {
            return null;
        }

        if (workOrder.CompletedAt == null && workOrder.ActualEndTime == null)
        {
            throw new TaktBusinessException("维护工单尚未填写完工时间，无法归档维护履历");
        }

        var existing = await _maintenanceHistoryRepository.FirstAsync(
            x => x.MaintenanceWorkOrderId == workOrder.Id
                && x.TenantCode == workOrder.TenantCode
                && x.CompanyCode == workOrder.CompanyCode);

        var history = existing ?? new TaktMaintenanceHistory
        {
            MaintenanceWorkOrderId = workOrder.Id,
            EquipmentId = workOrder.EquipmentId,
        };

        history.WorkOrderCode = workOrder.WorkOrderCode;
        history.EquipCode = workOrder.EquipCode;
        history.MaintenanceType = workOrder.MaintenanceType;
        history.MaintenanceCategory = workOrder.MaintenanceCategory;
        history.MaintenanceCompany = workOrder.MaintenanceCompany;
        history.MaintenanceTechnician = ResolveTechnician(workOrder, labors);
        history.MaintenanceDate = workOrder.CompletedAt ?? workOrder.ActualEndTime ?? DateTime.Now;
        history.MaintenanceStartTime = workOrder.ActualStartTime;
        history.MaintenanceEndTime = workOrder.ActualEndTime ?? workOrder.CompletedAt;
        history.MaintenanceContent = workOrder.MaintenanceContent;
        history.FaultDescription = workOrder.FaultDescription;
        history.Solution = workOrder.Solution;
        history.UsedParts = BuildUsedPartsJson(materials);
        history.MaintenanceCost = workOrder.TotalCost;
        history.MaintenanceResult = workOrder.MaintenanceResult;
        history.MaintenanceStatus = 2;
        history.NextMaintenanceDate = workOrder.NextMaintenanceDate;
        history.MaintenanceCycleDays = workOrder.MaintenanceCycleDays;
        history.MaintenanceDocuments = workOrder.MaintenanceDocuments;
        history.MaintenanceImages = workOrder.MaintenanceImages;
        history.AcceptedSummary = workOrder.AcceptedSummary;
        history.AcceptedByEmployeeName = workOrder.AcceptedByEmployeeName;
        history.AcceptedAt = workOrder.AcceptedAt;
        history.ArchivedAt = DateTime.Now;

        if (existing == null)
        {
            history = await _maintenanceHistoryRepository.CreateAsync(history);
        }
        else
        {
            await _maintenanceHistoryRepository.UpdateAsync(history);
        }

        if (workOrder.IsHistoryArchived != 1)
        {
            workOrder.IsHistoryArchived = 1;
            await _workOrderRepository.UpdateAsync(workOrder);
        }

        return history.Adapt<TaktMaintenanceHistoryDto>();
    }

    /// <summary>
    /// 从领料明细汇总配件 JSON
    /// </summary>
    /// <param name="materials">领料明细</param>
    /// <returns>JSON 字符串</returns>
    private static string? BuildUsedPartsJson(IReadOnlyList<TaktMaintenanceWorkOrderMaterial>? materials)
    {
        if (materials == null || materials.Count == 0)
        {
            return null;
        }

        var items = materials
            .Where(m => m.IssuedQuantity > 0)
            .Select(m => new
            {
                materialCode = m.MaterialCode,
                materialDescription = m.MaterialDescription,
                quantity = m.IssuedQuantity,
                unit = m.MaterialUnit,
                amount = m.Amount,
            })
            .ToList();

        return items.Count == 0 ? null : JsonConvert.SerializeObject(items);
    }

    /// <summary>
    /// 解析维护技师：优先工单指派，否则取首条报工员工编码
    /// </summary>
    /// <param name="workOrder">工单</param>
    /// <param name="labors">报工明细</param>
    /// <returns>技师编码</returns>
    private static string? ResolveTechnician(
        TaktMaintenanceWorkOrder workOrder,
        IReadOnlyList<TaktMaintenanceWorkOrderLabor>? labors)
    {
        if (!string.IsNullOrWhiteSpace(workOrder.AssignedTechnician))
        {
            return workOrder.AssignedTechnician;
        }

        return labors?.FirstOrDefault(l => !string.IsNullOrWhiteSpace(l.EmployeeCode))?.EmployeeCode;
    }
}

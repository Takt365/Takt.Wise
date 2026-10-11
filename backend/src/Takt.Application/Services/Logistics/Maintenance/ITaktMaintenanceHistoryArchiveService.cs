// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：ITaktMaintenanceHistoryArchiveService.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单完工归档至设备维护履历
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Domain.Entities.Logistics.Maintenance;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 维护工单 → 维护履历归档服务
/// </summary>
public interface ITaktMaintenanceHistoryArchiveService
{
    /// <summary>
    /// 维护工单状态变更后尝试归档（已完工/已结算/已关闭且未归档时写入履历）
    /// </summary>
    /// <param name="workOrderId">维护工单ID</param>
    /// <returns>归档后的履历 DTO；不满足条件时返回 null</returns>
    Task<TaktMaintenanceHistoryDto?> TryArchiveFromWorkOrderAsync(long workOrderId);

    /// <summary>
    /// 根据内存中的工单实体归档（含子表数据已加载时使用）
    /// </summary>
    /// <param name="workOrder">维护工单实体</param>
    /// <param name="materials">领料明细</param>
    /// <param name="labors">报工明细</param>
    /// <returns>归档后的履历 DTO；不满足条件时返回 null</returns>
    Task<TaktMaintenanceHistoryDto?> TryArchiveFromWorkOrderAsync(
        TaktMaintenanceWorkOrder workOrder,
        IReadOnlyList<TaktMaintenanceWorkOrderMaterial>? materials,
        IReadOnlyList<TaktMaintenanceWorkOrderLabor>? labors);
}

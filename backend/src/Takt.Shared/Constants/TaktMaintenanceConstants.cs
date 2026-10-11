// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktMaintenanceConstants.cs
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂维护模块常量（工单状态对齐字典 sys_ticket_status；与实体 WorkOrderStatus 列 int 一致）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 工厂维护模块常量
/// </summary>
public static class TaktMaintenanceConstants
{
    /// <summary>
    /// 维护工单：已创建
    /// </summary>
    public const int WorkOrderCreated = 0;

    /// <summary>
    /// 维护工单：已下达
    /// </summary>
    public const int WorkOrderReleased = 1;

    /// <summary>
    /// 维护工单：执行中
    /// </summary>
    public const int WorkOrderInProgress = 2;

    /// <summary>
    /// 维护工单：已完工
    /// </summary>
    public const int WorkOrderCompleted = 3;

    /// <summary>
    /// 维护工单：已结算
    /// </summary>
    public const int WorkOrderSettled = 4;

    /// <summary>
    /// 维护工单：已关闭
    /// </summary>
    public const int WorkOrderClosed = 5;

    /// <summary>
    /// 维护工单：已取消
    /// </summary>
    public const int WorkOrderCancelled = 6;

    /// <summary>
    /// 维护工单是否应触发履历归档（已完工/已结算/已关闭）
    /// </summary>
    /// <param name="workOrderStatus">工单状态</param>
    /// <returns>是否应归档</returns>
    public static bool ShouldArchiveWorkOrderToHistory(int workOrderStatus)
    {
        return workOrderStatus is WorkOrderCompleted or WorkOrderSettled or WorkOrderClosed;
    }
}

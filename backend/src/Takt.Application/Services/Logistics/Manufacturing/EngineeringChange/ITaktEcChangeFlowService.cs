// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcChangeFlowService.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知与执行监测流程接口
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 工程变更通知派发、确认与执行任务监测
/// </summary>
public interface ITaktEcChangeFlowService
{
    /// <summary>
    /// 派发变更通知：解析部门、持久化投递记录、SignalR 推送
    /// </summary>
    /// <param name="ecNotificationId">通知单 ID</param>
    /// <returns>异步任务</returns>
    Task DispatchEcNotificationAsync(long ecNotificationId);

    /// <summary>
    /// 确认通知投递（部门用户点击确认）
    /// </summary>
    /// <param name="dto">确认参数</param>
    /// <returns>异步任务</returns>
    Task ConfirmEcNotificationDeliveryAsync(TaktEcNotificationConfirmDto dto);

    /// <summary>
    /// 上报执行任务进度
    /// </summary>
    /// <param name="dto">进度参数</param>
    /// <returns>异步任务</returns>
    Task ReportEcExecutionTaskProgressAsync(TaktEcExecutionTaskProgressReportDto dto);

    /// <summary>
    /// 扫描超时/阻塞任务并推送预警（定时任务入口）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>处理任务数</returns>
    Task<int> ScanOverdueEcExecutionTasksAsync(string tenantCode, string companyCode);
}

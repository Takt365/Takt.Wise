// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktSignalRDispatchService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：SignalR 实时推送调度服务接口（强退、消息推送）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Models.Foundation;
using Takt.Shared.Models.Logistics.Manufacturing;
using Takt.Shared.Models.Workflow;

namespace Takt.Domain.Interfaces;

/// <summary>
/// SignalR 实时推送调度服务接口
/// </summary>
public interface ITaktSignalRDispatchService
{
    /// <summary>
    /// 强制踢出在线用户（强退）
    /// </summary>
    /// <param name="onlineId">在线用户记录 ID</param>
    /// <param name="reason">强退原因</param>
    /// <param name="connectionId">SignalR 连接 ID（主键查无记录时回退定位）</param>
    /// <param name="delaySeconds">延迟强退秒数（0 表示立即强退）</param>
    /// <returns>任务</returns>
    Task ForceKickOnlineAsync(long onlineId, string? reason = null, string? connectionId = null, int delaySeconds = 0);

    /// <summary>
    /// 批量强制踢出在线用户
    /// </summary>
    /// <param name="onlineIds">在线用户记录 ID 列表</param>
    /// <param name="reason">强退原因</param>
    /// <param name="delaySeconds">延迟强退秒数（0 表示立即强退）</param>
    /// <returns>任务</returns>
    Task ForceKickOnlineBatchAsync(IEnumerable<long> onlineIds, string? reason = null, int delaySeconds = 0);

    /// <summary>
    /// 推送私信到在线客户端
    /// </summary>
    /// <param name="message">私信推送模型</param>
    /// <returns>任务</returns>
    Task PushPrivateMessageAsync(TaktSignalRPrivateMessagePush message);

    /// <summary>
    /// 推送广播通知到公司内在线客户端
    /// </summary>
    /// <param name="broadcast">广播推送模型</param>
    /// <returns>任务</returns>
    Task PushBroadcastMessageAsync(TaktSignalRBroadcastPush broadcast);

    /// <summary>
    /// 向指定用户推送最新在线统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    Task PushOnlineStatisticsToUserAsync(string companyCode, string UserName, long? userId = null);

    /// <summary>
    /// 向指定用户推送最新消息统计（多终端同步）
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">用户名（接收者）</param>
    /// <param name="userId">用户 ID</param>
    /// <returns>任务</returns>
    Task PushMessageStatisticsToUserAsync(string companyCode, string UserName, long? userId = null);

    /// <summary>
    /// 推送流程定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushFlowSchemeChangedAsync(TaktSignalRFlowSchemeChangedPush push);

    /// <summary>
    /// 向指定用户推送流程实例推进事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <param name="targetUserName">目标用户名</param>
    /// <returns>任务</returns>
    Task PushFlowInstanceProgressedToUserAsync(TaktSignalRFlowInstanceProgressedPush push, string targetUserName);

    /// <summary>
    /// 向指定用户推送最新待办数量
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushFlowTodoCountToUserAsync(TaktSignalRFlowTodoCountPush push);

    /// <summary>
    /// 推送定时任务定义变更到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushQuartzTaskChangedAsync(TaktSignalRQuartzTaskChangedPush push);

    /// <summary>
    /// 推送定时任务执行完成到公司内在线客户端
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushQuartzTaskExecutedAsync(TaktSignalRQuartzTaskExecutedPush push);

    /// <summary>
    /// 推送工程变更通知到部门组（SendChangeNotification）
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcChangeNotificationAsync(TaktEcChangeNotificationPush push);

    /// <summary>
    /// 推送工程变更执行任务分配到部门组
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcExecutionTaskAssignedAsync(TaktEcExecutionTaskAssignedPush push);

    /// <summary>
    /// 推送工程变更任务进度到任务组与部门组
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcExecutionTaskProgressAsync(TaktEcExecutionTaskProgressPush push);

    /// <summary>
    /// 推送工程变更闭环完成（公司广播组）
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcChangeClosedAsync(TaktEcChangeClosedPush push);

    /// <summary>
    /// 推送工程变更任务超时/阻塞预警
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcExecutionTaskAlertAsync(TaktEcExecutionTaskAlertPush push);

    /// <summary>
    /// 向触发用户推送 BOM 物料成本机种月平均重算完成事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushBomMaterialCostItemRecalculateCompletedToUserAsync(TaktSignalRBomMaterialCostItemRecalculatePush push);

    /// <summary>
    /// 向触发用户推送设变技术课主表后台保存完成事件
    /// </summary>
    /// <param name="push">推送模型</param>
    /// <returns>任务</returns>
    Task PushEcTePersistCompletedToUserAsync(TaktSignalREcTePersistPush push);

    /// <summary>
    /// 向发起人推送部门确认通知
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">发起人用户名</param>
    /// <param name="payload">载荷</param>
    /// <returns>任务</returns>
    Task PushEcNotificationConfirmedToUserAsync(string companyCode, string UserName, object payload);

    /// <summary>
    /// 向发起人推送变更闭环完成
    /// </summary>
    /// <param name="companyCode">公司编码</param>
    /// <param name="UserName">发起人用户名</param>
    /// <param name="push">闭环模型</param>
    /// <returns>任务</returns>
    Task PushEcChangeClosedToUserAsync(string companyCode, string UserName, TaktEcChangeClosedPush push);
}

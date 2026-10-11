// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktEcFlowConstants.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：工程变更通知与执行监测流程常量
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 工程变更通知与执行监测流程常量
/// </summary>
public static class TaktEcFlowConstants
{
    /// <summary>
    /// 通知投递：待发送
    /// </summary>
    public const int DeliveryStatusPending = 0;

    /// <summary>
    /// 通知投递：已发送（已 SignalR 推送）
    /// </summary>
    public const int DeliveryStatusSent = 1;

    /// <summary>
    /// 通知投递：已确认
    /// </summary>
    public const int DeliveryStatusConfirmed = 2;

    /// <summary>
    /// 执行任务：待执行
    /// </summary>
    public const int TaskStatusPending = 0;

    /// <summary>
    /// 执行任务：执行中
    /// </summary>
    public const int TaskStatusInProgress = 1;

    /// <summary>
    /// 执行任务：已完成
    /// </summary>
    public const int TaskStatusCompleted = 2;

    /// <summary>
    /// 执行任务：阻塞
    /// </summary>
    public const int TaskStatusBlocked = 3;

    /// <summary>
    /// 执行任务：超时
    /// </summary>
    public const int TaskStatusOverdue = 4;

    /// <summary>
    /// 优先级：普通
    /// </summary>
    public const int PriorityNormal = 1;

    /// <summary>
    /// 优先级：高
    /// </summary>
    public const int PriorityHigh = 2;

    /// <summary>
    /// 优先级：紧急
    /// </summary>
    public const int PriorityUrgent = 3;

    /// <summary>
    /// Quartz 扫描超时任务 HandlerKey
    /// </summary>
    public const string QuartzHandlerEcTaskOverdueScan = "TaktEcExecutionTaskOverdueScanJobHandler";

    /// <summary>
    /// Quartz：BOM 生效日前 30 天、后 30 天提醒未实施部门
    /// </summary>
    public const string QuartzHandlerEcBomDateReminderScan = "TaktEcBomDateReminderScanJobHandler";

    /// <summary>
    /// 系统通知编码规则（EcNotificationMethod=1）
    /// </summary>
    public const string NotificationRuleSystem = "LG-ECN1";

    /// <summary>
    /// 邮件通知编码规则（EcNotificationMethod=2）
    /// </summary>
    public const string NotificationRuleEmail = "LG-ECN2";

    /// <summary>
    /// 纸质通知编码规则（EcNotificationMethod=3）
    /// </summary>
    public const string NotificationRulePaper = "LG-ECN3";

    /// <summary>
    /// 会议通知编码规则（EcNotificationMethod=4）
    /// </summary>
    public const string NotificationRuleMeeting = "LG-ECN4";

    /// <summary>
    /// 按通知方式取编码规则
    /// </summary>
    /// <param name="notificationMethod">通知方式（1=系统通知 2=邮件 3=纸质 4=会议）</param>
    /// <returns>编码规则 RuleCode</returns>
    public static string ResolveNotificationRuleCode(int notificationMethod) => notificationMethod switch
    {
        1 => NotificationRuleSystem,
        2 => NotificationRuleEmail,
        3 => NotificationRulePaper,
        4 => NotificationRuleMeeting,
        _ => NotificationRuleEmail,
    };
}

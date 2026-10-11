// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktTicketWorkflowHelper.cs
// 创建时间：2026-06-10
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台工单 ITSM 状态机流转校验（纯函数，无 I/O；状态值对齐字典 sys_ticket_status）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// 工单工作流状态机辅助类
/// </summary>
public static class TaktTicketWorkflowHelper
{
    /// <summary>
    /// 编码规则编码（对接 TaktNumbering.RuleCode）
    /// </summary>
    public const string TicketNumberRuleCode = "HD-TICKET";

    /// <summary>
    /// 将库内 int 规范化为合法服务台工单状态（含旧版 6=重新打开 → 7）
    /// </summary>
    /// <param name="status">库内状态值</param>
    /// <returns>合法状态 int</returns>
    public static int NormalizeLegacyStatus(int status) => TaktTicketConstants.NormalizeHelpDeskStatus(status);

    /// <summary>
    /// 导入/迁移旧版 4 态（0→Open，1→InProgress，2→Resolved，3→Closed）
    /// </summary>
    /// <param name="legacyStatus">旧版状态 int</param>
    /// <returns>新状态 int</returns>
    public static int MapLegacyImportStatus(int legacyStatus)
    {
        return legacyStatus switch
        {
            0 => TaktTicketConstants.New,
            1 => TaktTicketConstants.InProgress,
            2 => TaktTicketConstants.Completed,
            3 => TaktTicketConstants.Closed,
            _ => NormalizeLegacyStatus(legacyStatus),
        };
    }

    /// <summary>
    /// 校验状态流转是否合法
    /// </summary>
    /// <param name="current">当前状态</param>
    /// <param name="target">目标状态</param>
    /// <returns>是否允许流转</returns>
    public static bool CanTransition(int current, int target)
    {
        current = NormalizeLegacyStatus(current);
        target = NormalizeLegacyStatus(target);
        if (current == target)
        {
            return false;
        }
        return (current, target) switch
        {
            (TaktTicketConstants.New, TaktTicketConstants.Assigned) => true,
            (TaktTicketConstants.New, TaktTicketConstants.InProgress) => true,
            (TaktTicketConstants.Reopened, TaktTicketConstants.Assigned) => true,
            (TaktTicketConstants.Reopened, TaktTicketConstants.InProgress) => true,
            (TaktTicketConstants.Assigned, TaktTicketConstants.InProgress) => true,
            (TaktTicketConstants.InProgress, TaktTicketConstants.PendingConfirm) => true,
            (TaktTicketConstants.InProgress, TaktTicketConstants.Completed) => true,
            (TaktTicketConstants.PendingConfirm, TaktTicketConstants.InProgress) => true,
            (TaktTicketConstants.Completed, TaktTicketConstants.Closed) => true,
            (TaktTicketConstants.Completed, TaktTicketConstants.Reopened) => true,
            (TaktTicketConstants.Closed, TaktTicketConstants.Reopened) => true,
            _ => false,
        };
    }

    /// <summary>
    /// 是否允许客服领取/指派
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可领取</returns>
    public static bool CanPickOrAssign(int status)
    {
        status = NormalizeLegacyStatus(status);
        return status is TaktTicketConstants.New or TaktTicketConstants.Reopened;
    }

    /// <summary>
    /// 是否允许开始处理
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可开始</returns>
    public static bool CanStartProgress(int status)
    {
        return NormalizeLegacyStatus(status) == TaktTicketConstants.Assigned;
    }

    /// <summary>
    /// 是否允许请求用户补充信息
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可等待用户</returns>
    public static bool CanWaitForRequester(int status)
    {
        return NormalizeLegacyStatus(status) == TaktTicketConstants.InProgress;
    }

    /// <summary>
    /// 是否允许标记已解决
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可解决</returns>
    public static bool CanResolve(int status)
    {
        return NormalizeLegacyStatus(status) == TaktTicketConstants.InProgress;
    }

    /// <summary>
    /// 是否允许用户确认关闭
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可确认关闭</returns>
    public static bool CanConfirmClose(int status)
    {
        return NormalizeLegacyStatus(status) == TaktTicketConstants.Completed;
    }

    /// <summary>
    /// 是否允许重新打开
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <returns>是否可重开</returns>
    public static bool CanReopen(int status)
    {
        status = NormalizeLegacyStatus(status);
        return status is TaktTicketConstants.Completed or TaktTicketConstants.Closed;
    }

    /// <summary>
    /// 用户回复后是否应回到处理中
    /// </summary>
    /// <param name="status">当前状态</param>
    /// <param name="authorType">回复作者类型</param>
    /// <returns>是否自动回到处理中</returns>
    public static bool ShouldResumeAfterReply(int status, int authorType)
    {
        return NormalizeLegacyStatus(status) == TaktTicketConstants.PendingConfirm && authorType == 1;
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktTicketConstants.cs
// 创建时间：2026-06-18
// 创建人：Takt365(Cursor AI)
// 功能描述：通用工单状态常量（字典 sys_ticket_status；与 TaktTicket、TaktCustomerServiceTicket、TaktMaintenanceWorkOrder 等共用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 通用工单状态（字典 sys_ticket_status）
/// </summary>
public static class TaktTicketConstants
{
    /// <summary>
    /// 新建
    /// </summary>
    public const int New = 0;

    /// <summary>
    /// 已分配
    /// </summary>
    public const int Assigned = 1;

    /// <summary>
    /// 处理中
    /// </summary>
    public const int InProgress = 2;

    /// <summary>
    /// 待确认
    /// </summary>
    public const int PendingConfirm = 3;

    /// <summary>
    /// 已完成
    /// </summary>
    public const int Completed = 4;

    /// <summary>
    /// 已关闭
    /// </summary>
    public const int Closed = 5;

    /// <summary>
    /// 已取消
    /// </summary>
    public const int Cancelled = 6;

    /// <summary>
    /// 重新打开（服务台 ITSM 专用）
    /// </summary>
    public const int Reopened = 7;

    /// <summary>
    /// 服务台工单：旧版 6=重新打开 迁移为 7（不影响客户服务/维护工单 6=已取消）
    /// </summary>
    /// <param name="status">库内状态值</param>
    /// <returns>规范化后的状态</returns>
    public static int NormalizeHelpDeskStatus(int status)
    {
        if (status == Cancelled)
        {
            return Reopened;
        }
        return status is >= New and <= Reopened ? status : New;
    }
}

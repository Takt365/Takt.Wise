// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/routine/help-desk
// 文件名称：ticketStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：服务台工单件数统计（数据看板 ticket-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 服务台工单件数统计（按创建日与状态）
 * @description 对应后端 TaktTicketStatDto
 */
export interface HelpDeskTicketStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 服务工单件数 */
  monthTicketCount: number
  /** 未处理件数 */
  monthPendingCount: number
  /** 处理中件数 */
  monthInProgressCount: number
  /** 已处理件数 */
  monthProcessedCount: number
}

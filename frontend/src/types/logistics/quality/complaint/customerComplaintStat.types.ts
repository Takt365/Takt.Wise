// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：customerComplaintStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：客诉件数统计（数据看板 complaint-stat）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 客诉件数统计（按投诉日期与状态）
 * @description 对应后端 TaktCustomerComplaintStatDto
 */
export interface CustomerComplaintStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 客诉件数 */
  monthComplaintCount: number
  /** 未处理件数（待处理） */
  monthPendingCount: number
  /** 处理中件数 */
  monthInProgressCount: number
  /** 处理完成件数 */
  monthCompletedCount: number
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：pcbaDefectBoardStat.types.ts
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 不良看板统计 DTO（SMT 检查数 + 修理数）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * PCBA 不良看板统计
 * @description 对应后端 TaktPcbaDefectBoardStatDto
 */
export interface PcbaDefectBoardStat {
  /** 统计月份 yyyy-MM */
  statMonth: string
  /** 检查数（当日完成数量合计） */
  inspectionQty: number
  /** 修理数（不良数量合计） */
  repairQty: number
}

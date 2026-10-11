// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecImplementationStatus.constants.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变实施路径状态常量，与后端 TaktEcImplementationStatusConstants 对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 设变实施路径状态 */
export const TaktEcImplementationStatus = {
  /** 未开始 */
  NotStarted: 0,
  /** 实施中 */
  InProgress: 1,
  /** 正式完成（品管课全部明细已实施） */
  OfficiallyCompleted: 2,
  /** 全部完成（含制技） */
  FullyCompleted: 3,
} as const;

export type TaktEcImplementationStatusValue =
  (typeof TaktEcImplementationStatus)[keyof typeof TaktEcImplementationStatus];

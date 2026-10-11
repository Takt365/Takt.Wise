// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：materialCostItemSignalR.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 物料成本机种月平均重算 SignalR 事件类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * BOM 物料成本机种月平均重算完成事件
 */
export interface BomMaterialCostItemRecalculateCompletedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 触发用户名 */
  triggerUserName: string;
  /** 核算月份 YYYY-MM */
  processedMonth: string;
  /** 是否重置并重算 */
  forceRecalculate: boolean;
  /** 执行状态（1 成功 / 2 失败） */
  executeStatus: number;
  /** 执行耗时（毫秒） */
  executeDuration: number;
  /** 失败时的错误摘要 */
  errorMessage?: string;
  /** 扫描 BOM 行数 */
  scannedRowCount: number;
  /** 重算维度组数 */
  refreshedGroupCount: number;
  /** 跳过维度组数 */
  skippedGroupCount: number;
  /** 重置维度组数 */
  resetGroupCount: number;
  /** 涉及核算月份数 */
  processedMonthCount: number;
  /** 完成时间 */
  completedAt: string;
}

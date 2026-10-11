// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecGijutsuPersistSignalR.types.ts
// 创建时间：2026-09-08
// 创建人：Takt365(Cursor AI)
// 功能描述：设变技术课主表后台保存完成 SignalR 事件类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 设变技术课主表后台保存完成事件
 */
export interface EcGijutsuPersistCompletedEvent {
  /** 租户编码 */
  tenantCode: string;
  /** 公司编码 */
  companyCode: string;
  /** 触发用户名 */
  triggerUserName: string;
  /** 工厂代码 */
  plantCode: string;
  /** 设变单号 */
  ecCode: string;
  /** 是否为更新 */
  isUpdate: boolean;
  /** 主表主键 */
  ecGijutsuId: string;
  /** 明细行数 */
  detailCount: number;
  /** 执行状态（1 成功 / 2 失败） */
  executeStatus: number;
  /** 执行耗时（毫秒） */
  executeDuration: number;
  /** 失败时的错误摘要 */
  errorMessage?: string;
  /** 完成时间 */
  completedAt: string;
}

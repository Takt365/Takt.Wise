// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecBatchStageCodes.constants.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变批次转置阶段编码，与后端 TaktEcBatchStageCodes 对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 设变批次转置阶段编码
 */
export const TaktEcBatchStageCodes = {
  PurchaseOrder: 'PurchaseOrder',
  Scheduled: 'Scheduled',
  IncomingInspection: 'IncomingInspection',
  Outbound: 'Outbound',
  ElectronicOutbound: 'ElectronicOutbound',
  PcbaProduction: 'PcbaProduction',
  AssyProduction: 'AssyProduction',
  SampleInspection: 'SampleInspection',
  TechSop: 'TechSop',
} as const;

/** 转置表列顺序（与后端 TaktEcBatchStageCodes.TransposedOrder 一致） */
export const TaktEcBatchStageTransposedOrder: readonly string[] = [
  TaktEcBatchStageCodes.Scheduled,
  TaktEcBatchStageCodes.PurchaseOrder,
  TaktEcBatchStageCodes.IncomingInspection,
  TaktEcBatchStageCodes.Outbound,
  TaktEcBatchStageCodes.ElectronicOutbound,
  TaktEcBatchStageCodes.Mfg2Production,
  TaktEcBatchStageCodes.Mfg1Production,
  TaktEcBatchStageCodes.SampleInspection,
  TaktEcBatchStageCodes.TechSop,
];

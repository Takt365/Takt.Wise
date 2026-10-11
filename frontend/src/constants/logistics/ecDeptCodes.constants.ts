// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecDeptCodes.constants.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门编码常量，与后端 TaktEcDeptCodes / TaktDeptSeedData 对齐
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 设变责任部门编码（TaktDept.DeptCode，5 位；与 TaktEcExec.DeptCode、SignalR dept 分组一致）
 */
export const TaktEcDeptCodes = {
  /** 技术课 D0710 */
  Eng: 'D0710',
  /** 生管课 D0420 */
  Pmc: 'D0420',
  /** 采购课 D0510 */
  Mp: 'D0510',
  /** 受检课 D0810 */
  Iqc: 'D0810',
  /** 部管课 D0430 */
  Mc: 'D0430',
  /** 制造2课-物料 / SMT D0625 */
  Smt: 'D0625',
  /** 制造2课 D0620（看板「制二」桶；TaktEcSeizounika） */
  Pcba: 'D0620',
  /** 制造1课 D0610 */
  Assy: 'D0610',
  /** 品管课 D0820 */
  Qa: 'D0820',
  /** 制造技术课 D0630 */
  Te: 'D0630',
} as const;

/** 部门转置列顺序（与后端 TaktEcDeptCodes.TransposedOrder 一致；不含 Te） */
export const TaktEcDeptTransposedOrder: readonly string[] = [
  TaktEcDeptCodes.Mp,
  TaktEcDeptCodes.Pmc,
  TaktEcDeptCodes.Iqc,
  TaktEcDeptCodes.Mc,
  TaktEcDeptCodes.Pcba,
  TaktEcDeptCodes.Assy,
  TaktEcDeptCodes.Qa,
];

export type TaktEcDeptCode = (typeof TaktEcDeptCodes)[keyof typeof TaktEcDeptCodes];

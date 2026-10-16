// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/constants/logistics
// 文件名称：ecExecCodes.constants.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行编码别名导出（与 ecDeptCodes 同源，对齐后端 TaktEcDeptCodes）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TaktEcDeptCodes } from './ecDeptCodes.constants';

export {
  TaktEcDeptCodes as TaktEcExecCodes,
  TaktEcDeptTransposedOrder as TaktEcExecTransposedOrder,
  type TaktEcDeptCode as TaktEcExecCode,
} from './ecDeptCodes.constants';

/** 看板列顺序（与后端 TaktEcDeptCodes.KanbanOrder 一致；不含技术课） */
export const TaktEcKanbanOrder: readonly string[] = [
  TaktEcDeptCodes.Pmc,
  TaktEcDeptCodes.Mp,
  TaktEcDeptCodes.Iqc,
  TaktEcDeptCodes.Mc,
  TaktEcDeptCodes.Smt,
  TaktEcDeptCodes.Pcba,
  TaktEcDeptCodes.Assy,
  TaktEcDeptCodes.Qa,
  TaktEcDeptCodes.Te,
];

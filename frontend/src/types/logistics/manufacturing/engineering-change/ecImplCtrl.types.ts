// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecImplCtrl.types.ts
// 创建时间：2026-06-22
// 功能描述：设变实施管控类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

export interface EcImplCtrl extends CompanyDtoBase {
  ecDetailId: string;
  ecCode: string;
  lineNumber: number;
  ecModelCode: string;
  /** 机种编码组（制一、制二、品管执行行） */
  ecModelCodeGroup?: string;
  /** 根物料编码组（生管、制一、制二、品管执行行） */
  ecRootMaterialCodeGroup?: string;
  ecOldMaterialCode?: string;
  ecOldMaterialDescription?: string;
  ecOldPurchaseType?: string;
  ecNewMaterialCode?: string;
  ecNewMaterialDescription?: string;
  ecNewPurchaseType?: string;
  ecOldRequiresInspection: number;
  ecNewRequiresInspection: number;
  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理），只记录生管/制二/制一/品管；全仕向时生管与此相同。采购/受检/部管/SMT 固定按设变，制技固定按机种
   */
  ecControlScope?: number;
}

export interface EcImplCtrlQuery extends TaktPagedQuery {
  ecCode?: string;
  cultureCode?: string;
  ecModelCode?: string;
  ecOldMaterialCode?: string;
  ecNewMaterialCode?: string;
  ecOldPurchaseType?: string;
  ecNewPurchaseType?: string;
  ecOldRequiresInspection?: number;
  ecNewRequiresInspection?: number;
  /** pc 生管、assy 制一、pcba 制二、qa 品管 */
  deptTab?: string;
  /** 录入日期起，yyyy-MM-dd */
  ecEntryDateStart?: string;
  /** 录入日期止，yyyy-MM-dd */
  ecEntryDateEnd?: string;
}

export interface EcImplCtrlUpdate {
  ecDetailId: string;
  /**
   * 控制范围（字典 logistics_manufacturing_ec_control_scope_category；1=按成品，2=按机种，3=按设变，4=不管理）
   */
  ecControlScope: number;
  /** 当前页签，只更新该部门执行表 */
  deptTab?: string;
}

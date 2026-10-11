// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecDeptView.types.ts
// 创建时间：2026-06-22
// 功能描述：设变部门视图共用类型；引用键 logistics.manufacturing.engineering-change
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CompanyDtoBase, TaktPagedQuery } from '@/types/common.types';

export interface EcDeptView extends CompanyDtoBase {
  ecDeptId?: string;
  ecDetailId: string;
  ecGijutsuId: string;
  ecCode: string;
  lineNumber: number;
  ecModelCode: string;
  ecOldMaterialCode?: string;
  ecNewMaterialCode?: string;
  ecOldMaterialDescription?: string;
  ecNewMaterialDescription?: string;
  deptCode: string;
  isImplemented: number;
  content?: string;
  /** 执行内容（与 content 同源；实体 API 字段） */
  execContent?: string;
  /** 停产状态（字典 logistics_materials_material_discontinued_status） */
  discontinuedStatus?: string;
  /** 录入日期（技术） */
  entryDate?: string;
  /** 担当（EcLeader） */
  ecLeader?: string;
  scheduledDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderCode?: string;
  iqcOrderCode?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderCode?: string;
  productionTeam?: string;
  implementationDate?: string;
  /** 实施批次（制一） */
  implementationBatch?: string;
  inspectionBatch?: string;
  samplingCode?: string;
  /** SOP日期（制技） */
  sopDate?: string;
  isSopUpdated: number;
}

export interface EcDeptViewQuery extends TaktPagedQuery {
  ecCode?: string;
  ecModelCode?: string;
  isImplemented?: number;
  ecOldMaterialCode?: string;
  ecNewMaterialCode?: string;
}

export interface EcDeptViewUpdate {
  ecDetailId: string;
  isImplemented: number;
  content?: string;
  /** 录入日期（技术） */
  entryDate?: string;
  /** 担当（EcLeader） */
  ecLeader?: string;
  scheduledDate?: string;
  scheduledBatch?: string;
  poRemainder?: string;
  balance?: string;
  oldProductHandling?: string;
  purchaseOrderIssueDate?: string;
  supplier?: string;
  purchaseOrderCode?: string;
  iqcOrderCode?: string;
  inspectionDate?: string;
  outboundBatch?: string;
  outboundDate?: string;
  productionDate?: string;
  productionBatch?: string;
  outboundOrderCode?: string;
  productionTeam?: string;
  implementationDate?: string;
  /** 实施批次（制一） */
  implementationBatch?: string;
  inspectionBatch?: string;
  samplingCode?: string;
  /** SOP日期（制技） */
  sopDate?: string;
  isSopUpdated: number;
  remark?: string;
}

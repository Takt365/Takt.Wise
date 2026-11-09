// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecPur.types.ts
// 创建时间：2026-09-08
// 创建人：Takt365(Auto Generated)
// 功能描述：设变采购课类型（按设变一行；新/旧物料编码组聚合 F 类明细）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';
import type { EcDetail } from './ecDetail.types';

/**
 * 设变采购课（D0510）部门执行表
 * @description 对应后端 TaktEcPurDto
 */
export interface EcPur extends CompanyDtoBase {
  /** EcPurID（string 避免精度丢失） */
  ecPurId: string;
  /** 执行行去重键 */
  ecGrainKey?: string;
  /** 设变明细 ID */
  ecDetailId?: string;
  /** 设变单号 */
  ecCode: string;
  /** 行号 */
  lineNumber: number;
  /** 机种编码组 */
  ecModelCodeGroup?: string;
  /** 根物料编码组 */
  ecRootMaterialCodeGroup?: string;
  /** 新物料编码组 */
  ecNewMaterialCodeGroup?: string;
  /** 旧物料编码组（供旧品结余按行拆分） */
  ecOldMaterialCodeGroup?: string;
  /** 停产状态 */
  discontinuedStatus?: string;
  /** 实施范围 */
  ecImplScope?: number;
  /** 控制范围 */
  ecControlScope?: number;
  /** 新采购类型 */
  ecNewPurchaseType?: string;
  /** 部门编码 */
  deptCode: string;
  /** 部门名称 */
  deptName?: string;
  /** 是否实施 */
  isImplemented: number;
  /** 进货日期 */
  inboundDate?: string;
  /** 执行内容（物料:采购单10位:序号4位:供应商; 多行） */
  execContent?: string;
  /**
   * 旧品结余（可空；非空须按旧物料编码组逐行「旧物料-采购单10位:序号4位:数量;结余;处理方式;」；ntext）
   */
  ecOldPartBalance?: string;
  /** 是否作废 */
  isObsolete: number;
  /** 关联明细（同设变 + 采购类型 F） */
  ecDetails?: EcDetail[];
}

/**
 * EcPur 分页查询
 */
export interface EcPurQuery extends TaktPagedQuery {
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecGrainKey?: string;
  ecCode?: string;
  lineNumber?: number;
  ecNewMaterialCodeGroup?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewPurchaseType?: string;
  deptCode?: string;
  deptName?: string;
  isImplemented?: number;
  inboundDateStart?: string;
  inboundDateEnd?: string;
  execContent?: string;
  ecOldPartBalance?: string;
  isObsolete?: number;
  createdAtStart?: string;
  createdAtEnd?: string;
  extField?: string;
  remark?: string;
}

/**
 * 创建 EcPur
 */
export interface EcPurCreate {
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecGrainKey?: string;
  ecCode: string;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  lineNumber?: number;
  discontinuedStatus?: string;
  ecImplScope?: number;
  ecControlScope?: number;
  ecNewMaterialCodeGroup?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewPurchaseType?: string;
  deptCode: string;
  deptName?: string;
  isImplemented?: number;
  inboundDate?: string;
  execContent?: string;
  ecOldPartBalance?: string;
  isObsolete?: number;
  extField?: string;
  remark?: string;
}

/**
 * 更新 EcPur
 */
export interface EcPurUpdate extends EcPurCreate {
  ecPurId: string;
}

/**
 * 停产状态
 */
export interface EcPurDiscontinuedStatus {
  ecPurId: string;
  discontinuedStatus: string;
  ecImplScope?: number;
  ecControlScope?: number;
}

/**
 * 作废
 */
export interface EcPurObsolete {
  ecPurId: string;
  isObsolete: number;
}

/**
 * 导入模板行
 */
export interface EcPurTemplate {
  tenantCode?: string;
  companyCode?: string;
  cultureCode?: string;
  plantCode?: string;
  ecGrainKey?: string;
  ecCode?: string;
  lineNumber?: number;
  ecNewMaterialCodeGroup?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewPurchaseType?: string;
  deptCode?: string;
  deptName?: string;
  isImplemented?: number;
  inboundDate?: string;
  execContent?: string;
  ecOldPartBalance?: string;
  isObsolete?: number;
  extField?: string;
  remark?: string;
}

/**
 * 导入
 */
export interface EcPurImport extends EcPurTemplate {}

/**
 * 导出
 */
export interface EcPurExport {
  ecPurId: string;
  companyCode: string;
  plantCode: string;
  cultureCode: string;
  ecGrainKey: string;
  ecCode: string;
  ecModelCodeGroup?: string;
  ecRootMaterialCodeGroup?: string;
  lineNumber: number;
  discontinuedStatus: string;
  ecImplScope: number;
  ecControlScope: number;
  ecNewMaterialCodeGroup?: string;
  ecOldMaterialCodeGroup?: string;
  ecNewPurchaseType?: string;
  deptCode: string;
  deptName?: string;
  isImplemented: number;
  inboundDate?: string;
  execContent?: string;
  ecOldPartBalance?: string;
  isObsolete: number;
  extField?: string;
  remark?: string;
  createdAt: string;
}

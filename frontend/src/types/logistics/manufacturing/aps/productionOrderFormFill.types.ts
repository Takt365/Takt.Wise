// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/types/logistics/manufacturing/aps
// 文件名称：productionOrderFormFill.types.ts
// 功能描述：生产工单表单回填类型（独立非实体 CRUD；generate-from-backend 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 生产工单表单回填查询
 */
export interface ProductionOrderFormFillQuery {
  /**
   * 生产工单号（必填）
   */
  prodOrderCode: string
  /**
   * 工厂代码（可选）
   */
  plantCode?: string
  /**
   * 生产日期 YYYY-MM-DD（可选）
   */
  prodDate?: string
  /**
   * 直接作业人数（可选；用于计算 stdCapacity）
   */
  directLabor?: number
  /**
   * 是否附带默认明细预览（PCBA）
   */
  includeDefaultDetails?: boolean
}

/**
 * 默认明细预览行（PCBA 子表 timePeriod = workCenter）
 */
export interface ProductionOrderFormFillDefaultDetail {
  /**
   * 行号
   */
  lineNumber: number
  /**
   * 工作中心
   */
  workCenter: string
  /**
   * 工序描述
   */
  operationDesc?: string
  /**
   * 标准点数
   */
  standardShorts: number
  /**
   * 标准工时（分钟）
   */
  standardMinutes?: number
}

/**
 * 生产工单表单回填结果
 */
export interface ProductionOrderFormFill {
  /**
   * 生产工单号
   */
  prodOrderCode: string
  /**
   * 工厂代码
   */
  plantCode: string
  /**
   * 工单类别
   */
  prodOrderType: string
  /**
   * 机种编码
   */
  modelCode?: string
  /**
   * 物料编码
   */
  materialCode: string
  /**
   * 批次
   */
  batchCode?: string
  /**
   * 工单数量
   */
  prodOrderQty: number
  /**
   * 序列号
   */
  serialCode?: string
  /**
   * 标准工时（分钟）
   */
  stdMinutes: number
  /**
   * 人员标准生产稼动率（%）
   */
  operationRatePercent: number
  /**
   * 标准产能（小时产能）
   */
  stdCapacity?: number | null
  /**
   * 默认明细预览
   */
  defaultDetails?: ProductionOrderFormFillDefaultDetail[] | null
}

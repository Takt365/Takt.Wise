// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mrp
// 文件名称：manufacturingPlanningFlow.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路流程 DTO 类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 从 MDS 生成 MPS 请求 */
export interface MpsRunFromMds {
  /** 来源 MDS 头表 ID */
  masterDemandScheduleId: string;
  /** 时间桶粒度 */
  bucketType?: number;
  /** 已存在 MPS 头表 ID（可选，刷新行） */
  masterProductionScheduleId?: string;
}

/** MRP 运算选项 */
export interface MrpRunOptions {
  /** BOM 类型 */
  bomType?: number;
  /** BOM 最大展开层级 */
  maxBomLevel?: number;
  /** 是否计入开放采购订单 */
  includeOpenPurchaseOrders?: boolean;
  /** 是否计入已确认计划订单 */
  includePlannedOrders?: boolean;
}

/** MRP 运算请求 */
export interface MrpRun {
  /** MRP 头表 ID */
  materialRequirementsPlanningId: string;
  /** 运算选项 */
  options?: MrpRunOptions;
}

/** 计划订单释放到 APS */
export interface ReleasePlannedOrdersToAps {
  /** 计划订单 ID 列表 */
  plannedOrderIds: string[];
}

/** APS 排程请求 */
export interface ApsScheduleRun {
  /** APS 订单 ID 列表 */
  apsOrderIds: string[];
  /** 已有 APS 排程批次 ID */
  apsScheduleId?: string;
  /** 排程名称 */
  scheduleName?: string;
}

/** APS 释放生产工单 */
export interface ReleaseApsToProduction {
  /** APS 订单 ID 列表 */
  apsOrderIds: string[];
}

/** 采购计划转 PR */
export interface ConvertPurchasePlanToPr {
  /** 是否自动提交会签 */
  submitForCountersign?: boolean;
}

/** 制造计划编排结果 */
export interface ManufacturingPlanningFlowResult {
  /** 主实体 ID */
  entityId: string;
  /** 业务编码 */
  entityCode?: string;
  /** 处理行数 */
  processedCount: number;
  /** 产出子实体 ID 列表 */
  createdEntityIds?: string[];
}

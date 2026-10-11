// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/manufacturing/mrp
// 文件名称：manufacturingPlanningFlow.api.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：制造计划全链路流程 API（MDS→MPS→MRP→APS→工单 / 采购计划→PR）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  ApsScheduleRun,
  ConvertPurchasePlanToPr,
  ManufacturingPlanningFlowResult,
  MrpRun,
  MpsRunFromMds,
  ReleaseApsToProduction,
  ReleasePlannedOrdersToAps,
} from '@/types/logistics/manufacturing/mrp/manufacturingPlanningFlow.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 TaktManufacturingPlanningFlowController）
 * @description TaktManufacturingPlanningFlow
 */
const MANUFACTURING_PLANNING_FLOW_API_BASE = 'TaktManufacturingPlanningFlow';

/**
 * 从 MDS 生成或刷新 MPS
 * @param dto 下推参数
 * @returns 编排结果
 */
export function runMpsFromMds(dto: MpsRunFromMds): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/mps/run-from-mds`,
    method: 'post',
    data: dto,
  });
}

/**
 * 执行 MRP 运算
 * @param dto 运算参数
 * @returns 编排结果
 */
export function runMrp(dto: MrpRun): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/mrp/run`,
    method: 'post',
    data: dto,
  });
}

/**
 * 发布 MRP 运算结果
 * @param materialRequirementsPlanningId MRP 头表 ID
 * @returns 编排结果
 */
export function publishMrp(materialRequirementsPlanningId: string): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/mrp/${materialRequirementsPlanningId}/publish`,
    method: 'post',
  });
}

/**
 * 计划订单释放到 APS
 * @param dto 计划订单 ID 列表
 * @returns 编排结果
 */
export function releasePlannedOrdersToAps(dto: ReleasePlannedOrdersToAps): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/planned-orders/release-to-aps`,
    method: 'post',
    data: dto,
  });
}

/**
 * APS 排程
 * @param dto 排程参数
 * @returns 编排结果
 */
export function runApsScheduling(dto: ApsScheduleRun): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/aps/schedule`,
    method: 'post',
    data: dto,
  });
}

/**
 * APS 释放为生产工单
 * @param dto APS 订单 ID 列表
 * @returns 编排结果
 */
export function releaseApsToProductionOrders(dto: ReleaseApsToProduction): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/aps/release-to-production`,
    method: 'post',
    data: dto,
  });
}

/**
 * 采购计划转采购申请
 * @param purchasePlanId 采购计划 ID
 * @param dto 转 PR 选项
 * @returns 编排结果
 */
export function convertPurchasePlanToPurchaseRequest(
  purchasePlanId: string,
  dto?: ConvertPurchasePlanToPr,
): Promise<ManufacturingPlanningFlowResult> {
  return request<ManufacturingPlanningFlowResult>({
    url: `${MANUFACTURING_PLANNING_FLOW_API_BASE}/purchase-plans/${purchasePlanId}/convert-to-pr`,
    method: 'post',
    data: dto ?? {},
  });
}

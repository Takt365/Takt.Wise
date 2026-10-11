// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/logistics/customer-service
// 文件名称：stat.api.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块 API（自动生成，请勿手改路由常量）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type {
  ServiceContractStat,
  ServiceContractStatQuery,
  ServiceOrderStat,
  ServiceOrderStatQuery,
  ServiceRequestStat,
  ServiceRequestStatQuery,
  ServiceTicketStat,
  ServiceTicketStatQuery
} from '@/types/logistics/customer-service/stat.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktCustomerServiceStats
 */
const CUSTOMER_SERVICE_STAT_API_BASE = 'TaktCustomerServiceStats';

// ========================================
// 基础 CRUD
// ========================================

/**
 * 服务请求统计（数据看板）
 * @param {ServiceRequestStatQuery} queryDto 查询 DTO
 * @returns {Promise<ServiceRequestStat>} 请求统计
 */
export function getServiceRequestStat(queryDto: ServiceRequestStatQuery): Promise<ServiceRequestStat> {
  return request<ServiceRequestStat>({
    url: `${CUSTOMER_SERVICE_STAT_API_BASE}/request-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 服务订单统计（数据看板）
 * @param {ServiceOrderStatQuery} queryDto 查询 DTO
 * @returns {Promise<ServiceOrderStat>} 订单统计
 */
export function getServiceOrderStat(queryDto: ServiceOrderStatQuery): Promise<ServiceOrderStat> {
  return request<ServiceOrderStat>({
    url: `${CUSTOMER_SERVICE_STAT_API_BASE}/order-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 服务工单统计（数据看板）
 * @param {ServiceTicketStatQuery} queryDto 查询 DTO
 * @returns {Promise<ServiceTicketStat>} 工单统计
 */
export function getServiceTicketStat(queryDto: ServiceTicketStatQuery): Promise<ServiceTicketStat> {
  return request<ServiceTicketStat>({
    url: `${CUSTOMER_SERVICE_STAT_API_BASE}/ticket-stat`,
    method: 'get',
    params: queryDto,
  });
}

/**
 * 服务合同统计（数据看板）
 * @param {ServiceContractStatQuery} queryDto 查询 DTO
 * @returns {Promise<ServiceContractStat>} 合同统计
 */
export function getServiceContractStat(queryDto: ServiceContractStatQuery): Promise<ServiceContractStat> {
  return request<ServiceContractStat>({
    url: `${CUSTOMER_SERVICE_STAT_API_BASE}/contract-stat`,
    method: 'get',
    params: queryDto,
  });
}

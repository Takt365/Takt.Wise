// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/views/dashboard/dataBoard.types/utils
// 文件名称：statsQuery.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：数据看板统计查询辅助（静默请求、分页 total、有界聚合）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import dayjs from 'dayjs';
import request from '@/api/request';
import type { TaktPagedResult } from '@/types/common.types';
import {
  ensureTaktPaginationConfigAsync,
  getTaktDefaultPageIndex,
  getTaktMaxPageSize,
} from '@/utils/taktPaged';
import { createLogger } from '@/utils/logger';

const statsQueryLogger = createLogger('statsQuery');

/** 看板统计 list / GET 路径（相对 /api） */
export const DASHBOARD_STATS_API = {
  flowTodoList: 'TaktFlowEngine/todo/list',
  messageStatistics: 'TaktMessages/statistics',
  messageUnreadList: 'TaktMessages/unread-list',
  onlineDashboard: 'TaktOnlines/statistics/dashboard',
  /** 在线用户 list（TaktOnlines） */
  onlineList: 'TaktOnlines/list',
  /** 会议通知 list（TaktMeetingNotifications） */
  meetingNotificationList: 'TaktMeetingNotifications/list',
  /** 公告通知 list（TaktAnnouncements） */
  announcementList: 'TaktAnnouncements/list',
  /** 公告通知件数统计（TaktAnnouncementStats/announcement-stat；独立于 CRUD） */
  announcementStat: 'TaktAnnouncementStats/announcement-stat',
  /** 会议中心 list（TaktMeetings） */
  meetingList: 'TaktMeetings/list',
  /** 会议件数统计（TaktMeetings/meeting-stat） */
  meetingStat: 'TaktMeetings/meeting-stat',
  /** 服务台工单 list（TaktTickets） */
  helpDeskTicketList: 'TaktTickets/list',
  /** 服务台工单件数统计（TaktTickets/ticket-stat） */
  helpDeskTicketStat: 'TaktTickets/ticket-stat',
  /** 销售订单列表 */
  salesOrderList: 'TaktSalesOrders/list',
  /** 销售订单统计（TaktSalesOrders/order-stat） */
  salesOrderStat: 'TaktSalesOrders/order-stat',
  /** 销售发票统计（销售实绩推移；TaktSalesActualTrends/invoice-stat） */
  salesInvoiceStat: 'TaktSalesActualTrends/invoice-stat',
  /** 设变部门执行行数统计（8 张部门表；TaktEcKanbans/dept-execution-count） */
  ecDeptExecutionCount: 'TaktEcKanbans/dept-execution-count',
  /** 设变实施路径 / 卡点部门（TaktEcKanban / ITaktEcKanbanService） */
  ecKanbanList: 'TaktEcKanbans/list',
  /** 设变主表 + 子表明细数量统计（TaktEcGijutsu / ITaktEcGijutsuService） */
  ecStat: 'TaktEcGijutsus/stat',
  productionOrderList: 'TaktProductionOrders/list',
  personnelOperationRateList: 'TaktPersonnelOperationRates/list',
  assyOutputProductionStat: 'TaktAssyOutputStats/production-stat',
  pcbaOutputProductionStat: 'TaktPcbaOutputStats/production-stat',
  /** 组立不良统计（TaktAssyDefects/defect-stat） */
  assyDefectStat: 'TaktAssyDefects/defect-stat',
  /** PCBA 不良看板统计（检查数 + 修理数；TaktPcbaDefectBoardStats/board-stat） */
  pcbaDefectBoardStat: 'TaktPcbaDefectBoardStats/board-stat',
  /** 月生产不良推移（TaktDefectMonthlyTrends/monthly-trend-analysis） */
  defectMonthlyTrend: 'TaktDefectMonthlyTrends/monthly-trend-analysis',
  /** IQC 检验统计（TaktIqcOrders/inspection-stat） */
  iqcOrderStat: 'TaktIqcOrders/inspection-stat',
  /** IPQC 检验统计（TaktIpqcOrders/inspection-stat） */
  ipqcOrderStat: 'TaktIpqcOrders/inspection-stat',
  /** FQC 检验统计（TaktFqcOrders/inspection-stat） */
  fqcOrderStat: 'TaktFqcOrders/inspection-stat',
  /** 品质业务金额统计（TaktQualityAssurances/cost-stat） */
  qualityAssuranceCostStat: 'TaktQualityAssurances/cost-stat',
  /** 品质事故金额统计（TaktQualityIncidents/cost-stat） */
  qualityIncidentCostStat: 'TaktQualityIncidents/cost-stat',
  /** 品质应对金额统计（TaktQualityIssues/cost-stat） */
  qualityIssueCostStat: 'TaktQualityIssues/cost-stat',
  /** 客诉件数统计（TaktCustomerComplaints/complaint-stat） */
  customerComplaintStat: 'TaktCustomerComplaints/complaint-stat',
  /** 客户服务请求统计 */
  serviceRequestStat: 'TaktCustomerServiceStats/request-stat',
  /** 客户服务订单统计 */
  serviceOrderStat: 'TaktCustomerServiceStats/order-stat',
  /** 客户服务工单统计 */
  serviceTicketStat: 'TaktCustomerServiceStats/ticket-stat',
  /** 客户服务合同统计 */
  serviceContractStat: 'TaktCustomerServiceStats/contract-stat',
  /** 采购订单统计（TaktPurchaseOrders/order-stat） */
  purchaseOrderStat: 'TaktPurchaseOrders/order-stat',
  /** 采购发票统计（采购实绩推移；TaktPurchaseActualTrends/invoice-stat） */
  purchaseInvoiceStat: 'TaktPurchaseActualTrends/invoice-stat',
  /** 采购申请统计（TaktPurchaseRequests/request-stat） */
  purchaseRequestStat: 'TaktPurchaseRequests/request-stat',
  /** 在库金额统计（TaktMaterialMovingPrices/stock-stat） */
  materialMovingPriceStockStat: 'TaktMaterialMovingPrices/stock-stat',
} as const;

/** 看板指标对应 list 权限（无权限时不发请求） */
export const DASHBOARD_STATS_PERMISSION = {
  flowTodoList: 'workflow:todo:list',
  messageStatistics: 'foundation:message:query',
  messageUnreadList: 'foundation:message:unread',
  onlineDashboard: 'foundation:online:list',
  meetingList: 'routine:meeting:center:list',
  meetingNotificationList: 'routine:meeting:center:notification:list',
  announcementList: 'routine:announcement:list',
  helpDeskTicketList: 'routine:help:desk:ticket:list',
  helpDeskMyTicketList: 'routine:help:desk:my:ticket:list',
  salesOrderList: 'logistics:sales:order:list',
  salesOrderStat: 'logistics:sales:order:list',
  salesInvoiceStat: 'logistics:sales:actual:trend:list',
  ecDeptExecutionCount: 'logistics:manufacturing:engineering:change:kanban:list',
  ecKanbanList: 'logistics:manufacturing:engineering:change:kanban:list',
  ecList: 'logistics:manufacturing:engineering:change:gijutsu:list',
  productionOrderList: 'logistics:manufacturing:output:production:order:list',
  personnelOperationRateList: 'logistics:manufacturing:output:personnel:operation:rate:list',
  assyOutputList: 'logistics:manufacturing:output:assy:list',
  pcbaOutputList: 'logistics:manufacturing:output:pcba:list',
  assyDefectList: 'logistics:manufacturing:defect:assy:list',
  pcbaInspectionList: 'logistics:manufacturing:defect:pcba:inspection:list',
  pcbaRepairList: 'logistics:manufacturing:defect:pcba:repair:list',
  defectMonthlyTrendList: 'logistics:manufacturing:defect:monthly:list',
  iqcOrderList: 'logistics:quality:operation:iqc:order:list',
  ipqcOrderList: 'logistics:quality:operation:ipqc:order:list',
  fqcOrderList: 'logistics:quality:operation:fqc:order:list',
  qualityAssuranceList: 'logistics:quality:cost:assurance:list',
  qualityIncidentList: 'logistics:quality:cost:incident:list',
  qualityIssueList: 'logistics:quality:cost:issue:list',
  customerComplaintList: 'logistics:quality:complaint:customer:list',
  serviceRequestStat: 'logistics:customer:service:request:query',
  serviceOrderStat: 'logistics:customer:service:order:query',
  serviceTicketStat: 'logistics:customer:service:ticket:query',
  serviceContractStat: 'logistics:customer:service:contract:query',
  purchaseOrderStat: 'logistics:procurement:purchase:order:list',
  purchaseInvoiceStat: 'logistics:procurement:purchase:actual:trend:list',
  purchaseRequestStat: 'logistics:procurement:purchase:request:list',
  materialMovingPriceList: 'logistics:materials:material:moving:price:list',
} as const;

/** 看板质量/不良/客服用指标跳转路由 */
export const DASHBOARD_STATS_ROUTE = {
  flowTodo: '/workflow/todo',
  message: '/foundation/message',
  meeting: '/routine/meeting-center/meeting',
  announcement: '/routine/announcement',
  helpDeskTicket: '/routine/help-desk/ticket',
  helpDeskMyTicket: '/routine/help-desk/my-ticket',
  online: '/foundation/online',
  assyOutput: '/logistics/manufacturing/output/assy-output',
  pcbaOutput: '/logistics/manufacturing/output/pcba-output',
  assyDefect: '/logistics/manufacturing/defect/assy-defect',
  pcbaInspection: '/logistics/manufacturing/defect/pcba-inspection',
  pcbaRepair: '/logistics/manufacturing/defect/pcba-repair',
  defectMonthlyTrend: '/logistics/manufacturing/defect/defect-monthly',
  iqcOrder: '/logistics/quality/operation/iqc-order',
  ipqcOrder: '/logistics/quality/operation/ipqc-order',
  fqcOrder: '/logistics/quality/operation/fqc-order',
  qualityAssurance: '/logistics/quality/cost/assurance',
  qualityIncident: '/logistics/quality/cost/incident',
  qualityIssue: '/logistics/quality/cost/issue',
  customerComplaint: '/logistics/quality/complaint/customer-complaint',
  serviceRequest: '/logistics/customer-service/request',
  serviceOrder: '/logistics/customer-service/order',
  serviceTicket: '/logistics/customer-service/ticket',
  serviceContract: '/logistics/customer-service/contract',
  purchaseOrder: '/logistics/procurement/purchase-order',
  purchaseInvoice: '/logistics/procurement/purchase-invoice',
  purchaseRequest: '/logistics/procurement/purchase-request',
  salesOrder: '/logistics/sales/order',
  salesInvoice: '/logistics/sales/sales-invoice',
  materialMovingPrice: '/logistics/materials/material-moving-price',
} as const;

/** 日期区间 */
export interface TaktDateRange {
  start: string;
  end: string;
}

/**
 * 获取当前自然月区间（含当日）
 * @returns {TaktDateRange} YYYY-MM-DD HH:mm:ss
 */
export function getCurrentMonthRange(): TaktDateRange {
  const start = dayjs().startOf('month').format('YYYY-MM-DD 00:00:00');
  const end = dayjs().endOf('month').format('YYYY-MM-DD 23:59:59');
  return { start, end };
}

/**
 * 获取上月自然月区间
 * @returns {TaktDateRange} YYYY-MM-DD HH:mm:ss
 */
export function getLastMonthRange(): TaktDateRange {
  const anchor = dayjs().subtract(1, 'month');
  return {
    start: anchor.startOf('month').format('YYYY-MM-DD 00:00:00'),
    end: anchor.endOf('month').format('YYYY-MM-DD 23:59:59'),
  };
}

/**
 * 获取去年同期自然月区间
 * @returns {TaktDateRange} YYYY-MM-DD HH:mm:ss
 */
export function getSameMonthLastYearRange(): TaktDateRange {
  const anchor = dayjs().subtract(1, 'year');
  return {
    start: anchor.startOf('month').format('YYYY-MM-DD 00:00:00'),
    end: anchor.endOf('month').format('YYYY-MM-DD 23:59:59'),
  };
}

/**
 * 计算同比增幅（百分比）
 * @param current 本期值
 * @param previous 同期值
 * @returns {number} 增幅百分比
 */
export function calcYoYPercent(current: number, previous: number): number {
  if (previous <= 0) {
    return current > 0 ? 100 : 0;
  }
  return ((current - previous) / previous) * 100;
}

/**
 * 延迟执行看板数据加载，避免路由切换时与首屏渲染争抢主线程
 * @param task 加载任务
 */
export function scheduleDashboardLoad(task: () => void | Promise<void>): void {
  const run = (): void => {
    void task();
  };
  if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
    window.requestIdleCallback(run, { timeout: 1500 });
    return;
  }
  setTimeout(run, 16);
}

/**
 * 看板专用：部门执行行数（TaktEcKanbans/dept-execution-count）
 * @param isImplemented 是否实施（0/1；省略=全部）
 * @returns {Promise<number>} 行数
 */
export async function fetchDashboardDeptExecutionCount(isImplemented?: number): Promise<number> {
  const params = isImplemented === undefined ? undefined : { isImplemented };
  const data = await fetchDashboardGet<{ count: number }>(DASHBOARD_STATS_API.ecDeptExecutionCount, params);
  return data?.count ?? 0;
}

/** 看板 Popover 预览列表条数上限 */
export const DASHBOARD_POPOVER_LIST_SIZE = 6;

/**
 * 看板专用：分页 list 预览（静默、有界条数）
 * @param path list API 路径
 * @param query 额外查询条件
 * @returns {Promise<{ rows: T[]; total: number }>} 预览行与总数
 */
export async function fetchDashboardListPreview<T>(
  path: string,
  query: Record<string, unknown> = {},
): Promise<{ rows: T[]; total: number }> {
  await ensureTaktPaginationConfigAsync();
  const res = await fetchDashboardGet<TaktPagedResult<T>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: DASHBOARD_POPOVER_LIST_SIZE,
  });
  return {
    rows: res?.data ?? [],
    total: res?.total ?? 0,
  };
}

/**
 * 看板静默 GET（失败不弹全局通知）
 * @param path API 路径（相对 /api）
 * @param params 查询参数
 * @returns {Promise<T | null>} 业务 data 或 null
 */
export async function fetchDashboardGet<T>(
  path: string,
  params?: Record<string, unknown>,
): Promise<T | null> {
  try {
    return await request<T>({
      url: path,
      method: 'get',
      params,
      skipErrorNotification: true,
    });
  } catch (error: unknown) {
    statsQueryLogger.warn('看板 GET 失败', { action: 'fetchDashboardGet', path }, error);
    return null;
  }
}

/**
 * 有权限时执行看板指标加载
 * @param permitted 是否具备权限
 * @param label 指标标识
 * @param fetcher 加载函数
 * @param fallback 失败默认值
 * @returns {Promise<T>} 指标值
 */
export async function fetchDashboardMetricIfPermitted<T>(
  permitted: boolean,
  label: string,
  fetcher: () => Promise<T>,
  fallback: T,
): Promise<T> {
  if (!permitted) {
    return fallback;
  }
  return fetchMetricSafely(label, fetcher, fallback);
}

/**
 * 看板单项指标安全加载（失败时返回默认值，不影响其他指标）
 * @param label 指标标识（日志用）
 * @param fetcher 加载函数
 * @param fallback 失败时的默认值
 * @returns {Promise<T>} 指标值
 */
export async function fetchMetricSafely<T>(
  label: string,
  fetcher: () => Promise<T>,
  fallback: T,
): Promise<T> {
  try {
    return await fetcher();
  } catch (error: unknown) {
    statsQueryLogger.warn(`指标 ${label} 加载失败`, { action: 'fetchMetricSafely', label }, error);
    return fallback;
  }
}

/**
 * 看板专用：分页 list 仅取 total（静默、pageSize=1）
 * @param path list API 路径
 * @param query 额外查询条件
 * @returns {Promise<number>} 总条数
 */
export async function fetchDashboardPagedTotal(
  path: string,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const res = await fetchDashboardGet<TaktPagedResult<unknown>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: 1,
  });
  return res?.total ?? 0;
}

/**
 * 分页列表仅取 total（pageSize=1，避免全量拉数）
 * @param fetcher 列表 API
 * @param query 额外查询条件
 * @returns {Promise<number>} 总条数
 */
export async function fetchPagedTotal<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize: 1,
  });
  return res.total ?? 0;
}

/** 有界求和结果 */
export interface TaktBoundedSumResult {
  sum: number;
  total: number;
  sampled: number;
}

/**
 * 看板专用：有界行数内求和（静默）
 * @param path list API 路径
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<TaktBoundedSumResult>} 求和结果
 */
export async function fetchDashboardBoundedSum<T>(
  path: string,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<TaktBoundedSumResult> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetchDashboardGet<TaktPagedResult<T>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res?.data ?? [];
  let sum = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
    }
  });
  return {
    sum,
    total: res?.total ?? rows.length,
    sampled: rows.length,
  };
}

/**
 * 在有界行数内对数值字段求和（用于看板聚合，非全量统计）
 * @param fetcher 列表 API
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<TaktBoundedSumResult>} 求和结果
 */
export async function fetchBoundedSum<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<TaktBoundedSumResult> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res.data ?? [];
  let sum = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
    }
  });
  return {
    sum,
    total: res.total ?? rows.length,
    sampled: rows.length,
  };
}

/**
 * 看板专用：有界行数内求平均（静默）
 * @param path list API 路径
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<number>} 平均值
 */
export async function fetchDashboardBoundedAverage<T>(
  path: string,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetchDashboardGet<TaktPagedResult<T>>(path, {
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res?.data ?? [];
  if (rows.length === 0) {
    return 0;
  }
  let sum = 0;
  let count = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
      count += 1;
    }
  });
  return count > 0 ? sum / count : 0;
}

/**
 * 在有界行数内对数值字段求平均
 * @param fetcher 列表 API
 * @param pickValue 取值函数
 * @param query 额外查询条件
 * @returns {Promise<number>} 平均值（无数据时 0）
 */
export async function fetchBoundedAverage<T>(
  fetcher: (query: Record<string, unknown>) => Promise<TaktPagedResult<T>>,
  pickValue: (item: T) => number,
  query: Record<string, unknown> = {},
): Promise<number> {
  await ensureTaktPaginationConfigAsync();
  const pageSize = getTaktMaxPageSize();
  const res = await fetcher({
    ...query,
    pageIndex: getTaktDefaultPageIndex(),
    pageSize,
  });
  const rows = res.data ?? [];
  if (rows.length === 0) {
    return 0;
  }
  let sum = 0;
  let count = 0;
  rows.forEach((item) => {
    const value = pickValue(item);
    if (Number.isFinite(value)) {
      sum += value;
      count += 1;
    }
  });
  return count > 0 ? sum / count : 0;
}

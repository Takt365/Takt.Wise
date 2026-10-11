// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：stat.types.ts
// 创建时间：2026-08-28
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================


/**
 * 服务请求统计查询 DTO（按请求日期区间）
 * 对应前端 ServiceRequestStatQuery
 * @description 对应后端 TaktServiceRequestStatQueryDto
 */
export interface ServiceRequestStatQuery {
  /**
   * 请求日期（范围-开始；默认当月 1 日）
   */
  requestDateStart?: string;

  /**
   * 请求日期（范围-结束；默认当月最后一日）
   */
  requestDateEnd?: string;

}


/**
 * 服务请求统计 DTO
 * 对应前端 ServiceRequestStat
 * @description 对应后端 TaktServiceRequestStatDto
 */
export interface ServiceRequestStat {
  /**
   * 统计月份（yyyy-MM）
   */
  statMonth: string;

  /**
   * 月请求数量
   */
  monthRequestCount: number;

}


/**
 * 服务订单统计查询 DTO（按订单日期区间）
 * 对应前端 ServiceOrderStatQuery
 * @description 对应后端 TaktServiceOrderStatQueryDto
 */
export interface ServiceOrderStatQuery {
  /**
   * 订单日期（范围-开始；默认当月 1 日）
   */
  orderDateStart?: string;

  /**
   * 订单日期（范围-结束；默认当月最后一日）
   */
  orderDateEnd?: string;

}


/**
 * 服务订单统计 DTO
 * 对应前端 ServiceOrderStat
 * @description 对应后端 TaktServiceOrderStatDto
 */
export interface ServiceOrderStat {
  /**
   * 统计月份（yyyy-MM）
   */
  statMonth: string;

  /**
   * 月订单数量
   */
  monthOrderCount: number;

  /**
   * 月订单金额合计（分；前端展示时 ÷100 为元）
   */
  monthTotalAmount: number;

}


/**
 * 服务工单统计查询 DTO（按创建时间区间）
 * 对应前端 ServiceTicketStatQuery
 * @description 对应后端 TaktServiceTicketStatQueryDto
 */
export interface ServiceTicketStatQuery {
  /**
   * 创建时间（范围-开始；默认当月 1 日）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围-结束；默认当月最后一日）
   */
  createdAtEnd?: string;

}


/**
 * 服务工单统计 DTO
 * 对应前端 ServiceTicketStat
 * @description 对应后端 TaktServiceTicketStatDto
 */
export interface ServiceTicketStat {
  /**
   * 统计月份（yyyy-MM）
   */
  statMonth: string;

  /**
   * 月工单数量
   */
  monthTicketCount: number;

  /**
   * 月进行中工单数量（状态 0～3）
   */
  monthOpenTicketCount: number;

  /**
   * 月已完成/已关闭工单数量（状态 4～5）
   */
  monthClosedTicketCount: number;

}


/**
 * 服务合同统计查询 DTO（按生效日期区间）
 * 对应前端 ServiceContractStatQuery
 * @description 对应后端 TaktServiceContractStatQueryDto
 */
export interface ServiceContractStatQuery {
  /**
   * 生效日期（范围-开始；默认当月 1 日）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围-结束；默认当月最后一日）
   */
  effectiveDateEnd?: string;

}


/**
 * 服务合同统计 DTO
 * 对应前端 ServiceContractStat
 * @description 对应后端 TaktServiceContractStatDto
 */
export interface ServiceContractStat {
  /**
   * 统计月份（yyyy-MM）
   */
  statMonth: string;

  /**
   * 月合同数量
   */
  monthContractCount: number;

  /**
   * 月合同金额合计（分；前端展示时 ÷100 为元）
   */
  monthContractAmount: number;

}


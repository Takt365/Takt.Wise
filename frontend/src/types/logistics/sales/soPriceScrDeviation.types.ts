// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：soPriceScrDeviation.types.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：SO 价在同生效周期 SCR 价目中不存在的分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * SO 价偏离 SCR 查询
 * @description 对应后端 TaktSoPriceScrDeviationQueryDto
 */
export interface SoPriceScrDeviationQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 订单期间起 */
  periodDateStart?: string;
  /** 订单期间止 */
  periodDateEnd?: string;
  /** 客户编码 */
  customerCode?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** SCR 条件类型 */
  priceType?: string;
}

/**
 * 同周期 SCR 价目点
 * @description 对应后端 TaktSoPriceScrPeriodPriceDto
 */
export interface SoPriceScrPeriodPrice {
  /** 定价记录号 */
  salesConditionRecordCode: string;
  /** 条件类型 */
  priceType: string;
  /** 有效起始日 */
  validFrom: string;
  /** 有效截至日 */
  validTo: string;
  /** item / scaleQuantity / scaleValue */
  priceSource: string;
  /** 定价序号 */
  salesConditionRecordSeq: number;
  /** 等级序号 */
  salesScaleSeq?: number | null;
  /** 数量门槛 */
  scaleQuantity?: number | null;
  /** 价值门槛 */
  scaleValue?: number | null;
  /** 原始价格 */
  price: number;
  /** 定价单位 */
  priceUnit: number;
  /** 折算单价 */
  normalizedUnitPrice: number;
}

/**
 * SO 价在 SCR 中不存在的明细行
 * @description 对应后端 TaktSoPriceScrDeviationDto
 */
export interface SoPriceScrDeviationRow {
  /** 工厂代码 */
  plantCode: string;
  /** 销售订单编码 */
  salesOrderCode: string;
  /** 行号 */
  lineNumber: number;
  /** 订单日期 */
  orderDate: string;
  /** 价格日期 */
  pricingDate: string;
  /** 客户编码 */
  customerCode: string;
  /** 客户名称 */
  customerName: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料描述 */
  materialDescription: string;
  /** 币种 */
  currencyCode: string;
  /** 订购数量 */
  orderQuantity: number;
  /** SO 原始单价 */
  soUnitPrice: number;
  /** SO 价格单位 */
  soPerUnit: number;
  /** SO 折算单价 */
  soNormalizedUnitPrice: number;
  /** no_scr / price_absent */
  matchReason: string;
  /** 周期价目数 */
  scrPriceCount: number;
  /** 周期价目明细 */
  scrPricesInPeriod: SoPriceScrPeriodPrice[];
  /** 周期价目文案 */
  scrPricesText: string;
  /** SCR 记录号（分号拼接） */
  scrSalesConditionRecordCodes: string;
  /** SCR 有效起 */
  scrValidFrom?: string | null;
  /** SCR 有效止 */
  scrValidTo?: string | null;
  /** 最近 SCR 折算单价 */
  closestScrNormalizedUnitPrice?: number | null;
  /** 偏离金额 */
  deviationAmount?: number | null;
  /** up / down / none */
  deviationTrend: string;
}

/**
 * SO 价偏离 SCR 分析结果
 * @description 对应后端 TaktSoPriceScrDeviationResultDto
 */
export interface SoPriceScrDeviationResult {
  /** 分页明细 */
  paged: TaktPagedResult<SoPriceScrDeviationRow>;
  /** 命中行总数 */
  deviationCount: number;
  /** 同周期无 SCR */
  noScrCount: number;
  /** 有 SCR 但单价不在价目中 */
  priceAbsentCount: number;
}

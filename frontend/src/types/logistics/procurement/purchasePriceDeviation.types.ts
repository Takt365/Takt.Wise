// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchasePriceDeviation.types.ts
// 创建时间：2026-09-20
// 创建人：Takt365(Cursor AI)
// 功能描述：PO 价在同生效周期 PIR 价目中不存在的分析类型
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { TaktPagedQuery, TaktPagedResult } from '@/types/common.types';

/**
 * PO 价偏离 PCR 查询
 * @description 对应后端 TaktPurchasePriceDeviationQueryDto
 */
export interface PurchasePriceDeviationQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 订单期间起 */
  periodDateStart?: string;
  /** 订单期间止 */
  periodDateEnd?: string;
  /** 供应商编码 */
  supplierCode?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** PIR 条件类型 */
  priceType?: string;
  /** 涨跌筛选：空=全部；changed；up；down */
  trendFilter?: string;
}

/**
 * 同周期 PIR 价目点
 * @description 对应后端 TaktPurchasePricePeriodPriceDto
 */
export interface PurchasePricePeriodPrice {
  /** 定价记录号 */
  purchaseConditionCode: string;
  /** 条件类型 */
  priceType: string;
  /** 有效起始日 */
  validFrom: string;
  /** 有效截至日 */
  validTo: string;
  /** item / scaleQuantity / scaleValue */
  priceSource: string;
  /** 定价序号 */
  purchaseConditionSeq: number;
  /** 等级序号 */
  purchaseScaleSeq?: number | null;
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
 * PO 价在 PIR 中不存在的明细行
 * @description 对应后端 TaktPurchasePriceDeviationDto
 */
export interface PurchasePriceDeviationRow {
  /** 工厂代码 */
  plantCode: string;
  /** 采购订单编码 */
  purchaseOrderCode: string;
  /** 行号 */
  lineNumber: number;
  /** 订单日期 */
  orderDate: string;
  /** 价格日期 */
  pricingDate: string;
  /** 供应商编码 */
  supplierCode: string;
  /** 供应商名称 */
  supplierName: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料描述 */
  materialDescription: string;
  /** 币种 */
  currencyCode: string;
  /** 订购数量 */
  orderQuantity: number;
  /** PO 原始单价 */
  poUnitPrice: number;
  /** PO 价格单位 */
  poPerUnit: number;
  /** PO 折算单价 */
  poNormalizedUnitPrice: number;
  /** 信息记录编码 */
  purchaseConditionCode?: string | null;
  /** no_pir / price_absent */
  matchReason: string;
  /** 周期价目数 */
  pirPriceCount: number;
  /** 周期价目明细 */
  pirPricesInPeriod: PurchasePricePeriodPrice[];
  /** 周期价目文案 */
  pirPricesText: string;
  /** PIR 记录号（分号拼接） */
  pirPurchaseConditionCodes: string;
  /** PIR 有效起 */
  pirValidFrom?: string | null;
  /** PIR 有效止 */
  pirValidTo?: string | null;
  /** PIR 生效周期展示 ValidFrom~ValidTo */
  pirValidPeriod?: string;
  /** 适用 PIR 折算单价 */
  closestPirNormalizedUnitPrice?: number | null;
  /** 偏离金额（PO − 适用 PIR） */
  deviationAmount?: number | null;
  /** 涨跌比（小数，如 0.05=5%） */
  deviationPercent?: number | null;
  /** up / down / none */
  deviationTrend: string;
}

/**
 * PO 价偏离 PCR 分析结果
 * @description 对应后端 TaktPurchasePriceDeviationResultDto
 */
export interface PurchasePriceDeviationResult {
  /** 分页明细 */
  paged: TaktPagedResult<PurchasePriceDeviationRow>;
  /** 命中行总数 */
  deviationCount: number;
  /** 同周期无 PIR */
  noPirCount: number;
  /** 有 PIR 但单价不在价目中 */
  priceAbsentCount: number;
}

/**
 * MIRO 价偏离 PCR 查询
 * @description 对应后端 TaktMiroPricePcrDeviationQueryDto
 */
export interface MiroPricePcrDeviationQuery extends TaktPagedQuery {
  /** 工厂代码（必填） */
  plantCode: string;
  /** 过账期间起 */
  periodDateStart?: string;
  /** 过账期间止 */
  periodDateEnd?: string;
  /** 供应商编码 */
  supplierCode?: string;
  /** 物料编码（模糊） */
  materialCode?: string;
  /** PCR 条件类型 */
  priceType?: string;
  /** 涨跌筛选：空=全部；changed；up；down */
  trendFilter?: string;
}

/**
 * MIRO 价偏离 PCR 明细行
 * @description 对应后端 TaktMiroPricePcrDeviationDto
 */
export interface MiroPricePcrDeviationRow {
  /** 工厂代码 */
  plantCode: string;
  /** 物料凭证编码 */
  materialDocumentCode: string;
  /** 行号 */
  lineNumber: number;
  /** 过账日期 */
  postingDate: string;
  /** 供应商编码 */
  supplierCode: string;
  /** 供应商名称 */
  supplierName: string;
  /** 物料编码 */
  materialCode: string;
  /** 物料描述 */
  materialDescription: string;
  /** 币种 */
  currencyCode: string;
  /** 数量 */
  quantity: number;
  /** MIRO 折算单价 */
  miroUnitPrice: number;
  /** 关联采购订单 */
  purchaseOrderCode?: string | null;
  /** no_pir / price_absent */
  matchReason: string;
  /** 周期价目数 */
  pirPriceCount: number;
  /** 周期价目明细 */
  pirPricesInPeriod: PurchasePricePeriodPrice[];
  /** 周期价目文案 */
  pirPricesText: string;
  /** PCR 记录号（分号拼接） */
  pirPurchaseConditionCodes: string;
  /** PCR 有效起 */
  pirValidFrom?: string | null;
  /** PCR 有效止 */
  pirValidTo?: string | null;
  /** PCR 生效周期展示 */
  pirValidPeriod?: string;
  /** 适用 PCR 折算单价 */
  closestPirNormalizedUnitPrice?: number | null;
  /** 偏离金额（MIRO − 适用 PCR） */
  deviationAmount?: number | null;
  /** 涨跌比（小数，如 0.05=5%） */
  deviationPercent?: number | null;
  /** up / down / none */
  deviationTrend: string;
}

/**
 * MIRO 价偏离 PCR 分析结果
 * @description 对应后端 TaktMiroPricePcrDeviationResultDto
 */
export interface MiroPricePcrDeviationResult {
  /** 分页明细 */
  paged: TaktPagedResult<MiroPricePcrDeviationRow>;
  /** 命中行总数 */
  deviationCount: number;
  /** 同周期无 PCR */
  noPirCount: number;
  /** 有 PCR 但单价不在价目中 */
  priceAbsentCount: number;
}

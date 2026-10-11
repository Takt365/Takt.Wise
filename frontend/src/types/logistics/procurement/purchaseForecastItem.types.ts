// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/procurement
// 文件名称：purchaseForecastItem.types.ts
// 创建时间：2026-08-06
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/procurement 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * Takt采购预测明细（一行 = 主表物料在某财年某月的 001/002 计划量；产品/类别/利润中心/机种/物料在主表）
 * 对应前端 TaktPurchaseForecastItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PurchaseForecastItem
 * @description 对应后端 TaktPurchaseForecastItemDto
 */
export interface PurchaseForecastItem extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseForecastId?: string;

  /**
   * 采购预测编码（冗余字段，便于查询）
   */
  purchaseForecastCode?: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber?: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear?: string;

  /**
   * 计划月份（1～12）
   */
  planMonth?: number;

  /**
   * 计划数量版本001
   */
  planQuantity001?: number;

  /**
   * 计划数量版本002
   */
  planQuantity002?: number;

  /**
   * 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
   */
  planQuantityDelta?: number;

  /**
   * 已转采购数量（基本单位数量）
   */
  convertedQuantity?: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice?: number;

  /**
   * 预计金额
   */
  estimatedAmount?: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete?: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * PurchaseForecastItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PurchaseForecastItemExport
 * @description 对应后端 TaktPurchaseForecastItemExportDto
 */
export interface PurchaseForecastItemExport {
  /**
   * PurchaseForecastItemID
   */
  purchaseForecastItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 采购预测ID（主子表关系，序列化为 string 以避免 Javascript 精度问题）
   */
  purchaseForecastId: string;

  /**
   * 采购预测编码（冗余字段，便于查询）
   */
  purchaseForecastCode: string;

  /**
   * 行号（固定步长=10）
   */
  lineNumber: number;

  /**
   * 财年（选项 TaktFinancialPeriods/options；DictValue=FinancialYearCode，如 FY2027）
   */
  fiscalYear: string;

  /**
   * 计划月份（1～12）
   */
  planMonth: number;

  /**
   * 计划数量版本001
   */
  planQuantity001: number;

  /**
   * 计划数量版本002
   */
  planQuantity002: number;

  /**
   * 计划增减（版本002 − 版本001；可为负表示减量；服务层写入，禁止手改）
   */
  planQuantityDelta: number;

  /**
   * 已转采购数量（基本单位数量）
   */
  convertedQuantity: number;

  /**
   * 预计单价
   */
  estimatedUnitPrice: number;

  /**
   * 预计金额
   */
  estimatedAmount: number;

  /**
   * 是否作废（字典 sys_yes_no；0=否 1=是；编辑移除子行时标记作废）
   */
  isObsolete: number;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}


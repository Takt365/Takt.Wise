// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：assurance.types.ts
// 创建时间：2026-07-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 品质业务主表,用于记录品质业务的基础信息(年月、顾客)及汇总数据
 * 对应前端 TaktQualityAssuranceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityAssurance
 * @description 对应后端 TaktQualityAssuranceDto
 */
export interface QualityAssurance extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode?: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth?: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * Debit Note No
   */
  debitNoteCode?: string;

  /**
   * 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
   */
  recordGroupCode?: string;

  /**
   * 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  recordGroupName?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost?: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  currencyCode?: string;

  /**
   * 来料检验费用明细列表（子表，级联保存）
   */
  incomingItems?: QualityAssuranceIncomingCreate[];

  /**
   * 初期/定期检定费用明细列表（子表，级联保存）
   */
  firstArticleItems?: QualityAssuranceFirstArticleCreate[];

  /**
   * 设备校正费用明细列表（子表，级联保存）
   */
  calibrationItems?: QualityAssuranceCalibrationCreate[];

  /**
   * 其他通常业务费用明细列表（子表，级联保存）
   */
  otherItems?: QualityAssuranceOtherCreate[];

  /**
   * 出货检验费用明细列表（子表，级联保存）
   */
  outgoingItems?: QualityAssuranceOutgoingCreate[];

  /**
   * 信赖性评价/ORT费用明细列表（子表，级联保存）
   */
  reliabilityItems?: QualityAssuranceReliabilityCreate[];

  /**
   * 顾客品质要求对应费用明细列表（子表，级联保存）
   */
  customerResponseItems?: QualityAssuranceCustomerResponseCreate[];

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
 * QualityAssurance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityAssuranceExport
 * @description 对应后端 TaktQualityAssuranceExportDto
 */
export interface QualityAssuranceExport {
  /**
   * QualityAssuranceID
   */
  qualityAssuranceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质业务编码(唯一,如:QO-2026-0001)
   */
  qualityAssuranceCode: string;

  /**
   * 业务年月(格式:2026-05)
   */
  assuranceMonth: string;

  /**
   * 客户名称1（冗余，与 TaktCustomer.CustomerName1 对齐）
   */
  customerName1?: string;

  /**
   * Debit Note No
   */
  debitNoteCode?: string;

  /**
   * 记录组编码（选项 TaktCostGroups/options?costGroupCategory=1；DictValue=CostGroupCode）
   */
  recordGroupCode?: string;

  /**
   * 记录组名称（冗余：按 RecordGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  recordGroupName?: string;

  /**
   * 质量总成本(元,自动计算 = 各子表费用合计)
   */
  totalQualityCost: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  currencyCode: string;

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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：incident.types.ts
// 创建时间：2026-06-30
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
 * 品质事故主表,用于记录废弃单的基础信息(年月日、机种)及汇总数据
 * 对应前端 TaktQualityIncidentDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityIncident
 * @description 对应后端 TaktQualityIncidentDto
 */
export interface QualityIncident extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode?: string;

  /**
   * 事故日期
   */
  incidentDate?: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute?: number;

  /**
   * 机种/产品型号
   */
  model?: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）
   */
  scrapGroupCode?: string;

  /**
   * 废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  scrapGroupName?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity?: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost?: number;

  /**
   * 成本币种(CNY/USD/JPY等)
   */
  currencyCode?: string;

  /**
   * 事故明细列表（子表，级联保存）
   */
  incidentItems?: QualityIncidentItemCreate[];

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
 * QualityIncident 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityIncidentExport
 * @description 对应后端 TaktQualityIncidentExportDto
 */
export interface QualityIncidentExport {
  /**
   * QualityIncidentID
   */
  qualityIncidentId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 品质事故编码(唯一,如:QI-2026-0001)
   */
  qualityIncidentCode: string;

  /**
   * 事故日期
   */
  incidentDate: string;

  /**
   * 间接人员费率(元/分钟)
   */
  indirectManpowerCostPerMinute: number;

  /**
   * 机种/产品型号
   */
  model: string;

  /**
   * 事故内容(废弃原因)
   */
  incidentReason?: string;

  /**
   * 废弃组编码（选项 TaktCostGroups/options?costGroupCategory=2；DictValue=CostGroupCode）
   */
  scrapGroupCode?: string;

  /**
   * 废弃组名称（冗余：按 ScrapGroupCode 取 TaktCostGroups.CostGroupName 联动）
   */
  scrapGroupName?: string;

  /**
   * 废弃总数(自动计算 = 各子表废弃数量合计)
   */
  totalScrapQuantity: number;

  /**
   * 总废弃费用(元,自动计算 = 各子表费用合计)
   */
  totalScrapCost: number;

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


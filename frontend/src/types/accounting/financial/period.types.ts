// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/financial
// 文件名称：period.types.ts
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/financial 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 财务期间（租户级主数据；按 CountryCode / sys_country_code 区分各国财年规则）
 * 对应前端 TaktFinancialPeriodDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 FinancialPeriod
 * @description 对应后端 TaktFinancialPeriodDto
 */
export interface FinancialPeriod extends TenantCoreDtoBase {
  /**
   * FinancialPeriodID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  financialPeriodId: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}

/**
 * FinancialPeriod 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 FinancialPeriodQuery
 * @description 对应后端 TaktFinancialPeriodQueryDto
 */
export interface FinancialPeriodQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode?: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode?: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear?: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth?: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode?: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}

/**
 * 创建FinancialPeriod DTO
 * 对应前端 FinancialPeriodCreate
 * @description 对应后端 TaktFinancialPeriodCreateDto
 */
export interface FinancialPeriodCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

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
 * 更新FinancialPeriod DTO
 * 继承 TaktFinancialPeriodCreateDto，添加 FinancialPeriodId 字段
 * 对应前端 FinancialPeriodUpdate
 * @description 对应后端 TaktFinancialPeriodUpdateDto
 */
export interface FinancialPeriodUpdate extends FinancialPeriodCreate {
  /**
   * FinancialPeriodID（标识要更新的实体）
   */
  financialPeriodId: string;

}

/**
 * FinancialPeriod 导入模板行 DTO
 * 对应前端 FinancialPeriodTemplate
 * @description 对应后端 TaktFinancialPeriodTemplateDto
 */
export interface FinancialPeriodTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode?: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode?: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear?: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth?: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode?: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn?: number;

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
 * FinancialPeriod 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 FinancialPeriodImport
 * @description 对应后端 TaktFinancialPeriodImportDto
 */
export interface FinancialPeriodImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode?: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode?: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear?: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth?: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode?: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn?: number;

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
 * FinancialPeriod 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 FinancialPeriodExport
 * @description 对应后端 TaktFinancialPeriodExportDto
 */
export interface FinancialPeriodExport {
  /**
   * FinancialPeriodID
   */
  financialPeriodId: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31；中国 FY2027=2027/1/1～2027/12/31）
   */
  financialYearCode: string;

  /**
   * 会计期间编码（YYYYMM，如 201101、202704）
   */
  periodCode: string;

  /**
   * 自然年（日历年份）
   */
  calendarYear: number;

  /**
   * 自然月（1～12）
   */
  calendarMonth: number;

  /**
   * 财季编码（随财年类别变化；日本/香港 Q1=4～6 月；中国 Q1=1～3 月；美国 Q1=10～12 月）
   */
  financialQuarterCode: string;

  /**
   * 是否内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

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


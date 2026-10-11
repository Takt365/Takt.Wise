// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/accounting/controlling
// 文件名称：standardWageRate.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：accounting/controlling 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 标准工资率实体
 * 对应前端 TaktStandardWageRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StandardWageRate
 * @description 对应后端 TaktStandardWageRateDto
 */
export interface StandardWageRate extends CompanyDtoBase {

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
 * StandardWageRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardWageRateExport
 * @description 对应后端 TaktStandardWageRateExportDto
 */
export interface StandardWageRateExport {
  /**
   * StandardWageRateID
   */
  standardWageRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 年月（yyyyMM）
   */
  yearMonth: string;

  /**
   * 工作天数
   */
  workingDays: number;

  /**
   * 销售额
   */
  salesAmount: number;

  /**
   * 直接人数
   */
  directLaborCount: number;

  /**
   * 直接工资
   */
  directLaborWage: number;

  /**
   * 直接加班小时
   */
  directOvertimeHours: number;

  /**
   * 直接加班总额
   */
  directOvertimeTotal: number;

  /**
   * 直接工资率
   */
  directWageRate: number;

  /**
   * 间接人数
   */
  indirectLaborCount: number;

  /**
   * 间接工资
   */
  indirectLaborWage: number;

  /**
   * 间接加班小时
   */
  indirectOvertimeHours: number;

  /**
   * 间接加班总额
   */
  indirectOvertimeTotal: number;

  /**
   * 间接工资率
   */
  indirectWageRate: number;

  /**
   * 关联工厂
   */
  plantCode?: string;

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


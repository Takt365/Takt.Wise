// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：payroll.types.ts
// 创建时间：2026-06-24
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/compensation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 薪酬体系（现金报酬方案头；组成项引用 TaktSalaryItem，不另建多种薪资实体）
 * 对应前端 TaktPayrollDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Payroll
 * @description 对应后端 TaktPayrollDto
 */
export interface Payroll extends CompanyDtoBase {

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
 * Payroll 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayrollExport
 * @description 对应后端 TaktPayrollExportDto
 */
export interface PayrollExport {
  /**
   * PayrollID
   */
  payrollId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 薪酬体系编码（租户+公司内唯一）
   */
  payrollCode: string;

  /**
   * 薪酬体系名称
   */
  payrollName: string;

  /**
   * 关联薪级表 ID
   */
  payScaleId?: string;

  /**
   * 默认公式方案编码（整单工资核算，见 TaktSalaryFormula.set_code）
   */
  formulaSetCode?: string;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  payrollStatus: number;

  /**
   * 说明
   */
  payrollDescription?: string;

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


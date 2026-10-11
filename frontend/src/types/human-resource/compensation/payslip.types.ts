// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：payslip.types.ts
// 创建时间：2026-06-23
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
 * 员工工资条（发薪结果单据，区别于 TaktEmpSalary 定薪档案）
 * 对应前端 TaktPayslipDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Payslip
 * @description 对应后端 TaktPayslipDto
 */
export interface Payslip extends CompanyDtoBase {

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
 * Payslip 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayslipExport
 * @description 对应后端 TaktPayslipExportDto
 */
export interface PayslipExport {
  /**
   * PayslipID
   */
  payslipId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 发薪期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 基本工资（元）
   */
  baseSalary: number;

  /**
   * 岗位工资（元）
   */
  positionSalary: number;

  /**
   * 绩效/奖金（元）
   */
  bonusAmount: number;

  /**
   * 加班费（元）
   */
  overtimePay: number;

  /**
   * 津贴合计（元）
   */
  allowanceTotal: number;

  /**
   * 应发合计（元）
   */
  grossAmount: number;

  /**
   * 社保扣款（元）
   */
  socialSecurityDeduction: number;

  /**
   * 公积金扣款（元）
   */
  housingFundDeduction: number;

  /**
   * 个税扣款（元）
   */
  taxDeduction: number;

  /**
   * 其他扣款（元）
   */
  otherDeduction: number;

  /**
   * 实发金额（元）
   */
  netAmount: number;

  /**
   * 关联计算公式方案编码（核算时按 TaktSalaryFormula.set_code 加载步骤并执行）
   */
  formulaSetCode?: string;

  /**
   * 发放状态（字典 humanresource_compensation_payslip_issue_status：0=待发放 1=已发放 2=已确认）
   */
  issueStatus: number;

  /**
   * 发放日期
   */
  issueDate?: string;

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


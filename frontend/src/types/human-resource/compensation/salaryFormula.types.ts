// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：salaryFormula.types.ts
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
 * 薪资计算公式（方案+步骤合一：set_code 分组，每行一步；标准五步：应发→社保→公积金→个税→实发） 同一 set_code 示例： gross_amount = base_salary + bonus_amount + overtime_pay + allowance_total social_security_deduction = social_security_base * employee_ss_ratio housing_fund_deduction = housing_fund_base * employee_hf_ratio tax_deduction = CUMULATIVE_TAX(taxable_income) net_amount = gross_amount - social_security_deduction - housing_fund_deduction - tax_deduction - other_deduction
 * 对应前端 TaktSalaryFormulaDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalaryFormula
 * @description 对应后端 TaktSalaryFormulaDto
 */
export interface SalaryFormula extends CompanyDtoBase {

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
 * SalaryFormula 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalaryFormulaExport
 * @description 对应后端 TaktSalaryFormulaExportDto
 */
export interface SalaryFormulaExport {
  /**
   * SalaryFormulaID
   */
  salaryFormulaId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 公式方案编码（同编码多行=一套完整核算步骤，租户+公司内业务唯一标识）
   */
  setCode: string;

  /**
   * 公式方案名称
   */
  setName: string;

  /**
   * 关联薪酬体系 ID（可选；同 set_code 各行取值应一致）
   */
  payrollId?: string;

  /**
   * 步骤编码（同方案内唯一，如 GROSS、SS_EMP、HF_EMP、TAX、NET）
   */
  formulaCode: string;

  /**
   * 步骤名称（如：应发合计、社保个人、公积金个人、个税、实发）
   */
  formulaName: string;

  /**
   * 公式步骤类型（字典 humanresource_compensation_salary_formula_step_type：应发/社保个人/公积金个人/个税/实发）
   */
  formulaStep: number;

  /**
   * 执行顺序（同一 set_code 内从小到大；应发=1 … 实发=5）
   */
  sortOrder: number;

  /**
   * 结果写入字段（与 TaktPayslip 列名一致，如 gross_amount、net_amount）
   */
  targetField: string;

  /**
   * 计算公式表达式（引擎解析；支持 + - * / 及 CUMULATIVE_TAX 等内置函数）
   */
  formulaExpression: string;

  /**
   * 步骤说明（可读描述，如「应发=基本+绩效+加班费+补贴」）
   */
  stepDescription?: string;

  /**
   * 方案生效日期（同 set_code 各行应一致）
   */
  effectiveDate: string;

  /**
   * 方案失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  formulaStatus: number;

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


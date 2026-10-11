// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：socialInsurance.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/benefits 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 社保与公积金月度缴纳流水（分项金额明细；福利类型配置不在此表重复建模）
 * 对应前端 TaktSocialInsuranceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SocialInsurance
 * @description 对应后端 TaktSocialInsuranceDto
 */
export interface SocialInsurance extends CompanyDtoBase {

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
 * SocialInsurance 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SocialInsuranceExport
 * @description 对应后端 TaktSocialInsuranceExportDto
 */
export interface SocialInsuranceExport {
  /**
   * SocialInsuranceID
   */
  socialInsuranceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 关联福利项目 ID（通常对应 humanresource_benefits_benefit_type 为社保/公积金的 TaktBenefitItem）
   */
  benefitItemId?: string;

  /**
   * 员工 ID
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 缴纳期间（如 2026-06）
   */
  payPeriod: string;

  /**
   * 社保缴纳基数（元）
   */
  socialSecurityBase: number;

  /**
   * 养老保险（元）
   */
  pensionAmount: number;

  /**
   * 医疗保险（元）
   */
  medicalAmount: number;

  /**
   * 失业保险（元）
   */
  unemploymentAmount: number;

  /**
   * 工伤保险（元）
   */
  injuryAmount: number;

  /**
   * 生育保险（元）
   */
  maternityAmount: number;

  /**
   * 公积金缴纳基数（元）
   */
  housingFundBase: number;

  /**
   * 公积金（元）
   */
  housingFundAmount: number;

  /**
   * 缴纳合计（元）
   */
  totalAmount: number;

  /**
   * 缴纳状态（字典 humanresource_benefits_social_insurance_pay_status：0=待缴纳 1=已缴纳 2=已补缴）
   */
  payStatus: number;

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


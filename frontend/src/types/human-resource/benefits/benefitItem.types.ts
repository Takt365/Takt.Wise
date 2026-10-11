// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/benefits
// 文件名称：benefitItem.types.ts
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
 * 福利项目（非直接现金福利主数据；年假请假走考勤模块，培训实施走培训模块，此处仅配置福利项）
 * 对应前端 TaktBenefitItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BenefitItem
 * @description 对应后端 TaktBenefitItemDto
 */
export interface BenefitItem extends CompanyDtoBase {

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
 * BenefitItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BenefitItemExport
 * @description 对应后端 TaktBenefitItemExportDto
 */
export interface BenefitItemExport {
  /**
   * BenefitItemID
   */
  benefitItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 福利项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 福利项目名称
   */
  itemName: string;

  /**
   * 福利大类（字典 humanresource_benefits_benefit_category：保险/补贴/休假/其他）
   */
  benefitCategory: number;

  /**
   * 福利类型（字典 humanresource_benefits_benefit_type：社保/公积金/商业保险/年假额度/餐补/培训补贴/员工折扣等）
   */
  benefitType: number;

  /**
   * 发放周期（字典 humanresource_benefits_benefit_payment_cycle）
   */
  paymentCycle: number;

  /**
   * 默认金额或补贴标准（元）
   */
  defaultAmount: number;

  /**
   * 金额上限（元，0 表示不限制）
   */
  maxAmount: number;

  /**
   * 公司承担比例（%，如公积金单位缴存比例）
   */
  employerRatio: number;

  /**
   * 个人承担比例（%，如公积金个人缴存比例）
   */
  employeeRatio: number;

  /**
   * 是否强制福利（字典 sys_yes_no）
   */
  isMandatory: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  itemStatus: number;

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


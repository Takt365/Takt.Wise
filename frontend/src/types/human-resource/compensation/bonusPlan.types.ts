// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：bonusPlan.types.ts
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
 * 奖金方案（现金奖金）
 * 对应前端 TaktBonusPlanDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BonusPlan
 * @description 对应后端 TaktBonusPlanDto
 */
export interface BonusPlan extends CompanyDtoBase {

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
 * BonusPlan 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BonusPlanExport
 * @description 对应后端 TaktBonusPlanExportDto
 */
export interface BonusPlanExport {
  /**
   * BonusPlanID
   */
  bonusPlanId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 方案编码（租户+公司内唯一）
   */
  planCode: string;

  /**
   * 方案名称
   */
  planName: string;

  /**
   * 奖金类型（字典 humanresource_compensation_bonus_type）
   */
  bonusType: number;

  /**
   * 计算方式（字典 humanresource_compensation_bonus_calc_method）
   */
  calcMethod: number;

  /**
   * 关联计算公式 ID（按公式计算时引用 TaktSalaryFormula）
   */
  salaryFormulaId?: string;

  /**
   * 默认奖金金额或基数（元）
   */
  defaultAmount: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 状态（字典 sys_normal_disable）
   */
  planStatus: number;

  /**
   * 方案说明
   */
  bonusPlanDescription?: string;

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


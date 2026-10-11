// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：salaryItem.types.ts
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
 * 薪资项目（现金报酬可配置主数据，含股权激励；不另建 TaktStockOption 等平行实体）
 * 对应前端 TaktSalaryItemDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalaryItem
 * @description 对应后端 TaktSalaryItemDto
 */
export interface SalaryItem extends CompanyDtoBase {

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
 * SalaryItem 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalaryItemExport
 * @description 对应后端 TaktSalaryItemExportDto
 */
export interface SalaryItemExport {
  /**
   * SalaryItemID
   */
  salaryItemId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 项目编码（租户+公司内唯一）
   */
  itemCode: string;

  /**
   * 项目名称
   */
  itemName: string;

  /**
   * 简称
   */
  shortName?: string;

  /**
   * 项目类型（字典 humanresource_compensation_salary_item_type：基本工资/岗位工资/津贴/奖金/股权激励等）
   */
  itemType: number;

  /**
   * 计算方式（字典 humanresource_compensation_salary_calc_method：固定金额/按比例/按公式）
   */
  calcMethod: number;

  /**
   * 关联计算公式步骤 ID（calc_method 为按公式时引用 TaktSalaryFormula 单行；整单核算用 formula_set_code）
   */
  salaryFormulaId?: string;

  /**
   * 默认金额（元）
   */
  defaultAmount: number;

  /**
   * 默认比例（%，0~100）
   */
  defaultRate: number;

  /**
   * 默认行权/授予价格（元；item_type 为股权激励时使用）
   */
  strikePrice: number;

  /**
   * 默认归属年限（年；item_type 为股权激励时使用）
   */
  vestingYears: number;

  /**
   * 是否扣款项（字典 sys_yes_no）
   */
  isDeduction: number;

  /**
   * 是否计入应税所得（字典 sys_yes_no）
   */
  isTaxable: number;

  /**
   * 是否计入社保基数（字典 sys_yes_no）
   */
  includeSocialSecurityBase: number;

  /**
   * 是否计入公积金基数（字典 sys_yes_no）
   */
  includeHousingFundBase: number;

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


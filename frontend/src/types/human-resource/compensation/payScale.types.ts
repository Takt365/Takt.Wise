// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/compensation
// 文件名称：payScale.types.ts
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
 * 薪级薪等（现金报酬等级带宽）
 * 对应前端 TaktPayScaleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 PayScale
 * @description 对应后端 TaktPayScaleDto
 */
export interface PayScale extends CompanyDtoBase {

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
 * PayScale 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 PayScaleExport
 * @description 对应后端 TaktPayScaleExportDto
 */
export interface PayScaleExport {
  /**
   * PayScaleID
   */
  payScaleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 薪级编码（租户+公司内唯一）
   */
  scaleCode: string;

  /**
   * 薪级名称
   */
  scaleName: string;

  /**
   * 等级（数字越大等级越高）
   */
  gradeLevel: number;

  /**
   * 下限金额（元）
   */
  minSalary: number;

  /**
   * 中位金额（元）
   */
  midSalary: number;

  /**
   * 上限金额（元）
   */
  maxSalary: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable）
   */
  scaleStatus: number;

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


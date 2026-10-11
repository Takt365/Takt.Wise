// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/mps
// 文件名称：standardOperationRate.types.ts
// 创建时间：2026-07-13
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/mps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 标准生产稼动率实体 OperationRate 为标准对标目标值；对比参考：达成率(%) = 实际稼动率 ÷ 标准稼动率 × 100%。
 * 对应前端 TaktStandardOperationRateDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 StandardOperationRate
 * @description 对应后端 TaktStandardOperationRateDto
 */
export interface StandardOperationRate extends CompanyDtoBase {

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear?: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType?: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate?: number;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用，1=启用）
   */
  rateStatus?: number;

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
 * StandardOperationRate 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 StandardOperationRateExport
 * @description 对应后端 TaktStandardOperationRateExportDto
 */
export interface StandardOperationRateExport {
  /**
   * StandardOperationRateID
   */
  standardOperationRateId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode: string;

  /**
   * 财务年度编码（如 FY2000、FY2027；日本/香港 FY2027=2026/4/1～2027/3/31）
   */
  financialYear: string;

  /**
   * 稼动率类型（1=人员，2=SMT设备，3=测试设备，4=包装设备，5=其他）
   */
  operationType: number;

  /**
   * 稼动率（比例，如 0.85 表示 85%）
   */
  operationRate: number;

  /**
   * 生效日期
   */
  effectiveDate: string;

  /**
   * 失效日期
   */
  expiryDate?: string;

  /**
   * 状态（字典 sys_normal_disable；0=禁用，1=启用）
   */
  rateStatus: number;

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


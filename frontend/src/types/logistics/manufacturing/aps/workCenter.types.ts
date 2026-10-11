// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：workCenter.types.ts
// 创建时间：2026-07-24
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/aps 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 工作中心（WC；PlantCode 对齐 TaktCalendar.RelatedPlant）
 * 对应前端 TaktWorkCenterDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 WorkCenter
 * @description 对应后端 TaktWorkCenterDto
 */
export interface WorkCenter extends CompanyDtoBase {

  /**
   * 工作中心编码
   */
  workCenterCode?: string;

  /**
   * 工作中心描述
   */
  workCenterDescription?: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus?: number;

  /**
   * 工作中心资源列表（子表，级联保存）
   */
  resources?: WorkCenterResourceCreate[];

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
 * WorkCenter 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WorkCenterExport
 * @description 对应后端 TaktWorkCenterExportDto
 */
export interface WorkCenterExport {
  /**
   * WorkCenterID
   */
  workCenterId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 工作中心编码
   */
  workCenterCode: string;

  /**
   * 工作中心描述
   */
  workCenterDescription: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  workCenterStatus: number;

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


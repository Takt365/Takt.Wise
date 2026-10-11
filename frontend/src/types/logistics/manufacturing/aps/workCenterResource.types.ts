// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：workCenterResource.types.ts
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
 * 工作中心资源（设备/人员/模具等）
 * 对应前端 TaktWorkCenterResourceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 WorkCenterResource
 * @description 对应后端 TaktWorkCenterResourceDto
 */
export interface WorkCenterResource extends CompanyDtoBase {
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）
   */
  workCenterId?: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode?: string;

  /**
   * 资源编码
   */
  resourceCode?: string;

  /**
   * 资源名称
   */
  resourceName?: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType?: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity?: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus?: number;

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
 * WorkCenterResource 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 WorkCenterResourceExport
 * @description 对应后端 TaktWorkCenterResourceExportDto
 */
export interface WorkCenterResourceExport {
  /**
   * WorkCenterResourceID
   */
  workCenterResourceId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工作中心 ID（主子表关系，关联 TaktWorkCenter.Id，选项 TaktWorkCenters/options）
   */
  workCenterId: string;

  /**
   * 工作中心编码（关联 TaktWorkCenter.WorkCenterCode，冗余；选项 TaktWorkCenters/options，DictValue=WorkCenterCode）
   */
  workCenterCode: string;

  /**
   * 资源编码
   */
  resourceCode: string;

  /**
   * 资源名称
   */
  resourceName: string;

  /**
   * 资源类型（字典 work_center_resource_type；0=设备，1=人员，2=模具）
   */
  resourceType: number;

  /**
   * 并行能力（可同时加工任务数）
   */
  parallelCapacity: number;

  /**
   * 效率系数（1.0=标准）
   */
  efficiencyRate: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  resourceStatus: number;

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


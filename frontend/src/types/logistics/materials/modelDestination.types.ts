// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：modelDestination.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/materials 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * Takt型号目的地实体（租户级；物料编码/名称、机种编码/名称、仕向地编码/名称）
 * 对应前端 TaktModelDestinationDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 ModelDestination
 * @description 对应后端 TaktModelDestinationDto
 */
export interface ModelDestination extends TenantCoreDtoBase {
  /**
   * ModelDestinationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  modelDestinationId: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 机种编码
   */
  modelCode: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地编码
   */
  destinationCode: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}

/**
 * ModelDestination 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ModelDestinationQuery
 * @description 对应后端 TaktModelDestinationQueryDto
 */
export interface ModelDestinationQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 机种编码
   */
  modelCode?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地编码
   */
  destinationCode?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 创建时间（范围查询-开始）
   */
  createdAtStart?: string;

  /**
   * 创建时间（范围查询-结束）
   */
  createdAtEnd?: string;

  /**
   * 扩展字段JSON
   */
  extField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}

/**
 * 创建ModelDestination DTO
 * 对应前端 ModelDestinationCreate
 * @description 对应后端 TaktModelDestinationCreateDto
 */
export interface ModelDestinationCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 机种编码
   */
  modelCode: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地编码
   */
  destinationCode: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

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
 * 更新ModelDestination DTO
 * 继承 TaktModelDestinationCreateDto，添加 ModelDestinationId 字段
 * 对应前端 ModelDestinationUpdate
 * @description 对应后端 TaktModelDestinationUpdateDto
 */
export interface ModelDestinationUpdate extends ModelDestinationCreate {
  /**
   * ModelDestinationID（标识要更新的实体）
   */
  modelDestinationId: string;

}

/**
 * ModelDestination 排序更新 DTO
 * 对应前端 ModelDestinationSort
 * @description 对应后端 TaktModelDestinationSortDto
 */
export interface ModelDestinationSort {
  /**
   * ModelDestinationID
   */
  modelDestinationId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}

/**
 * ModelDestination 导入模板行 DTO
 * 对应前端 ModelDestinationTemplate
 * @description 对应后端 TaktModelDestinationTemplateDto
 */
export interface ModelDestinationTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 机种编码
   */
  modelCode?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地编码
   */
  destinationCode?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

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
 * ModelDestination 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ModelDestinationImport
 * @description 对应后端 TaktModelDestinationImportDto
 */
export interface ModelDestinationImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料编码
   */
  materialCode?: string;

  /**
   * 物料描述
   */
  materialDescription?: string;

  /**
   * 机种编码
   */
  modelCode?: string;

  /**
   * 机种名称
   */
  modelName?: string;

  /**
   * 仕向地编码
   */
  destinationCode?: string;

  /**
   * 仕向地名称
   */
  destinationName?: string;

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
 * ModelDestination 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ModelDestinationExport
 * @description 对应后端 TaktModelDestinationExportDto
 */
export interface ModelDestinationExport {
  /**
   * ModelDestinationID
   */
  modelDestinationId: string;

  /**
   * 物料编码
   */
  materialCode: string;

  /**
   * 物料描述
   */
  materialDescription: string;

  /**
   * 机种编码
   */
  modelCode: string;

  /**
   * 机种名称
   */
  modelName: string;

  /**
   * 仕向地编码
   */
  destinationCode: string;

  /**
   * 仕向地名称
   */
  destinationName: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/materials
// 文件名称：materialGroup.types.ts
// 创建时间：2026-08-13
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
 * Takt物料组主数据实体（租户级） 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
 * 对应前端 TaktMaterialGroupDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 MaterialGroup
 * @description 对应后端 TaktMaterialGroupDto
 */
export interface MaterialGroup extends TenantCoreDtoBase {
  /**
   * MaterialGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  materialGroupId: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName: string;

  /**
   * 排序号（sort；越小越靠前）
   */
  sortOrder: number;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

}


/**
 * MaterialGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaterialGroupQuery
 * @description 对应后端 TaktMaterialGroupQueryDto
 */
export interface MaterialGroupQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode?: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName?: string;

  /**
   * 排序号（sort；越小越靠前）
   */
  sortOrder?: number;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

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
 * 创建MaterialGroup DTO
 * 对应前端 MaterialGroupCreate
 * @description 对应后端 TaktMaterialGroupCreateDto
 */
export interface MaterialGroupCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName: string;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

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
 * 更新MaterialGroup DTO
 * 继承 TaktMaterialGroupCreateDto，添加 MaterialGroupId 字段
 * 对应前端 MaterialGroupUpdate
 * @description 对应后端 TaktMaterialGroupUpdateDto
 */
export interface MaterialGroupUpdate extends MaterialGroupCreate {
  /**
   * MaterialGroupID（标识要更新的实体）
   */
  materialGroupId: string;

}


/**
 * MaterialGroup 排序更新 DTO
 * 对应前端 MaterialGroupSort
 * @description 对应后端 TaktMaterialGroupSortDto
 */
export interface MaterialGroupSort {
  /**
   * MaterialGroupID
   */
  materialGroupId: string;

  /**
   * 排序号（sort；越小越靠前）
   */
  sortOrder: number;

}


/**
 * MaterialGroup 导入模板行 DTO
 * 对应前端 MaterialGroupTemplate
 * @description 对应后端 TaktMaterialGroupTemplateDto
 */
export interface MaterialGroupTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode?: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName?: string;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

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
 * MaterialGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaterialGroupImport
 * @description 对应后端 TaktMaterialGroupImportDto
 */
export interface MaterialGroupImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode?: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName?: string;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

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
 * MaterialGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaterialGroupExport
 * @description 对应后端 TaktMaterialGroupExportDto
 */
export interface MaterialGroupExport {
  /**
   * MaterialGroupID
   */
  materialGroupId: string;

  /**
   * 物料组编码（group_code；租户内唯一；与物料 material_group 对齐）
   */
  materialGroupCode: string;

  /**
   * 物料组名称（group_name）
   */
  materialGroupName: string;

  /**
   * 排序号（sort；越小越靠前）
   */
  sortOrder: number;

  /**
   * 物料组描述（description）
   */
  materialGroupDescription?: string;

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


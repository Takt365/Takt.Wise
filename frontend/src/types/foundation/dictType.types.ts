// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：dictType.types.ts
// 创建时间：2026-07-20
// 创建人：Takt365(Auto Generated)
// 功能描述：foundation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 字典类型实体 用于定义系统中使用的各种字典分类，如：订单状态、用户类型、审批状态等 租户级实体：字典类型在租户内共享，不需要公司隔离
 * 对应前端 TaktDictTypeDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 DictType
 * @description 对应后端 TaktDictTypeDto
 */
export interface DictType extends TenantCoreDtoBase {
  /**
   * DictTypeID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  dictTypeId: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus: number;

  /**
   * 字典数据列表（一对多关联） （子表：TaktDictData）
   */
  dictDataList?: DictData[];

}

/**
 * DictType 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DictTypeQuery
 * @description 对应后端 TaktDictTypeQueryDto
 */
export interface DictTypeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode?: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName?: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource?: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 排序号
   */
  sortOrder?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus?: number;

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
 * 创建DictType DTO
 * 对应前端 DictTypeCreate
 * @description 对应后端 TaktDictTypeCreateDto
 */
export interface DictTypeCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus: number;

  /**
   * 字典数据列表（一对多关联）（子表，级联保存）
   */
  dictDataList?: DictDataCreate[];

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
 * 更新DictType DTO
 * 继承 TaktDictTypeCreateDto，添加 DictTypeId 字段
 * 对应前端 DictTypeUpdate
 * @description 对应后端 TaktDictTypeUpdateDto
 */
export interface DictTypeUpdate extends DictTypeCreate {
  /**
   * DictTypeID（标识要更新的实体）
   */
  dictTypeId: string;

}

/**
 * DictType 状态更新 DTO
 * 对应前端 DictTypeStatus
 * @description 对应后端 TaktDictTypeStatusDto
 */
export interface DictTypeStatus {
  /**
   * DictTypeID
   */
  dictTypeId: string;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus: number;

}

/**
 * DictType 内置更新 DTO
 * 对应前端 DictTypeBuiltIn
 * @description 对应后端 TaktDictTypeBuiltInDto
 */
export interface DictTypeBuiltIn {
  /**
   * DictTypeID
   */
  dictTypeId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}

/**
 * DictType 排序更新 DTO
 * 对应前端 DictTypeSort
 * @description 对应后端 TaktDictTypeSortDto
 */
export interface DictTypeSort {
  /**
   * DictTypeID
   */
  dictTypeId: string;

  /**
   * 排序号
   */
  sortOrder: number;

}

/**
 * DictType 导入模板行 DTO
 * 对应前端 DictTypeTemplate
 * @description 对应后端 TaktDictTypeTemplateDto
 */
export interface DictTypeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode?: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName?: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource?: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus?: number;

  /**
   * 字典数据列表（一对多关联）（子表，级联保存）
   */
  dictDataList?: DictDataCreate[];

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
 * DictType 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DictTypeImport
 * @description 对应后端 TaktDictTypeImportDto
 */
export interface DictTypeImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode?: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName?: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource?: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn?: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus?: number;

  /**
   * 字典数据列表（一对多关联）（子表，级联保存）
   */
  dictDataList?: DictDataCreate[];

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
 * DictType 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DictTypeExport
 * @description 对应后端 TaktDictTypeExportDto
 */
export interface DictTypeExport {
  /**
   * DictTypeID
   */
  dictTypeId: string;

  /**
   * 字典类型编码（租户内唯一；命名 {领域}_{业务}_{项}，Length=140；如 accounting_controlling_cost_center_type）
   */
  dictTypeCode: string;

  /**
   * 字典类型名称（如：订单状态、用户类型）
   */
  dictTypeName: string;

  /**
   * 数据源（字典 sys_data_source；0=系统表 1=SQL查询）
   */
  dataSource: number;

  /**
   * SQL脚本（仅当DataSource=SqlScript时使用） SQL必须返回DictValue和DictLabel列，可选返回ListClass、CssClass、SortOrder
   */
  dictScript?: string;

  /**
   * 内置（字典 sys_yes_no；0=否 1=是）
   */
  isBuiltIn: number;

  /**
   * 排序号
   */
  sortOrder: number;

  /**
   * 状态（字典 sys_normal_disable；1=启用 0=禁用）
   */
  dictStatus: number;

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


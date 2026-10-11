// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：tenant.types.ts
// 创建时间：2026-06-09
// 创建人：Takt365(Auto Generated)
// 功能描述：identity 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  TaktPagedQuery,
  TenantCoreDtoBase
} from '@/types/common.types';

/**
 * 租户实体 代表系统中的独立租户（第一层数据隔离）
 * 对应前端 TaktTenantDto
 * 继承 TaktTenantCoreDtoBase（组合 4）
 * 对应前端 Tenant
 * @description 对应后端 TaktTenantDto
 */
export interface Tenant extends TenantCoreDtoBase {
  /**
   * TenantID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  tenantId: string;

  /**
   * 租户名称
   */
  tenantName: string;

  /**
   * 订阅开始时间
   */
  subscriptionStartTime: string;

  /**
   * 订阅结束时间（9999/12/31 23:59:59表示长期有效）
   */
  subscriptionEndTime: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus: number;

  /**
   * 可访问该租户的用户关联（RBAC，表 takt_identity_user_tenant） （子表：TaktUserTenant）
   */
  userTenants?: UserTenant[];

}

/**
 * Tenant 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 TenantQuery
 * @description 对应后端 TaktTenantQueryDto
 */
export interface TenantQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 租户名称
   */
  tenantName?: string;

  /**
   * 订阅开始时间（范围查询-开始）
   */
  subscriptionStartTimeStart?: string;

  /**
   * 订阅开始时间（范围查询-结束）
   */
  subscriptionStartTimeEnd?: string;

  /**
   * 订阅结束时间（9999/12/31 23:59:59表示长期有效）（范围查询-开始）
   */
  subscriptionEndTimeStart?: string;

  /**
   * 订阅结束时间（9999/12/31 23:59:59表示长期有效）（范围查询-结束）
   */
  subscriptionEndTimeEnd?: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus?: number;

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
  ExtField?: string;

  /**
   * 备注（模糊查询）
   */
  remark?: string;

}

/**
 * 创建Tenant DTO
 * 对应前端 TenantCreate
 * @description 对应后端 TaktTenantCreateDto
 */
export interface TenantCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant: string;

  /**
   * 租户名称
   */
  tenantName: string;

  /**
   * 订阅开始时间
   */
  subscriptionStartTime: string;

  /**
   * 订阅结束时间（9999/12/31 23:59:59表示长期有效）
   */
  subscriptionEndTime: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus: number;

  /**
   * 可访问该租户的用户 ID 列表（RBAC 反向合并，分配走 ITaktRbacService）
   */
  userIds?: any;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * 更新Tenant DTO
 * 继承 TaktTenantCreateDto，添加 TenantId 字段
 * 对应前端 TenantUpdate
 * @description 对应后端 TaktTenantUpdateDto
 */
export interface TenantUpdate extends TenantCreate {
  /**
   * TenantID（标识要更新的实体）
   */
  tenantId: string;

}

/**
 * Tenant 状态更新 DTO
 * 对应前端 TenantStatus
 * @description 对应后端 TaktTenantStatusDto
 */
export interface TenantStatus {
  /**
   * TenantID
   */
  tenantId: string;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus: number;

}

/**
 * Tenant 导入模板行 DTO
 * 对应前端 TenantTemplate
 * @description 对应后端 TaktTenantTemplateDto
 */
export interface TenantTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 租户名称
   */
  tenantName?: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * Tenant 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 TenantImport
 * @description 对应后端 TaktTenantImportDto
 */
export interface TenantImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
   */
  relatedPlant?: string;

  /**
   * 租户名称
   */
  tenantName?: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn?: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus?: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

}

/**
 * Tenant 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 TenantExport
 * @description 对应后端 TaktTenantExportDto
 */
export interface TenantExport {
  /**
   * TenantID
   */
  tenantId: string;

  /**
   * 租户名称
   */
  tenantName: string;

  /**
   * 订阅开始时间
   */
  subscriptionStartTime: string;

  /**
   * 订阅结束时间（9999/12/31 23:59:59表示长期有效）
   */
  subscriptionEndTime: string;

  /**
   * 联系人姓名
   */
  contactName?: string;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail: string;

  /**
   * 内置（1=是，0=否） 种子租户（000/500/100）为内置，不允许删除
   */
  isBuiltIn: number;

  /**
   * 状态（1=启用，0=禁用）
   */
  tenantStatus: number;

  /**
   * 扩展字段JSON
   */
  ExtField?: string;

  /**
   * 备注
   */
  remark?: string;

  /**
   * 创建时间
   */
  createdAt: string;

}


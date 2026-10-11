// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：adminDivision.types.ts
// 创建时间：2026-08-21
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
 * 行政区划实体（租户级共享；世界通用六级树） 层级：1=国家，2=州省，3=地市，4=区县，5=乡镇街道，6=行政村（字典 sys_admin_division_level） 编码可对齐 ISO 3166、ISO 3166-2、GB/T 2260、JIS 等；子节点 CountryCode 冗余自根国家便于过滤 组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase；仅租户）
 * 对应前端 TaktAdminDivisionDto
 * 继承 TaktTenantCoreDtoBase
 * 对应前端 AdminDivision
 * @description 对应后端 TaktAdminDivisionDto
 */
export interface AdminDivision extends TenantCoreDtoBase {
  /**
   * AdminDivisionID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  adminDivisionId: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId: string;

  /**
   * 层级（字典 sys_admin_division_level；1～6）
   */
  level: number;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath: string;

  /**
   * 是否叶子节点（字典 sys_yes_no）
   */
  isLeaf: number;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus: number;

}


/**
 * AdminDivision 树形列表/树选择 DTO（含子节点）
 * 对应 GetAdminDivisionTreeAsync 等接口
 * 对应前端 AdminDivisionTree
 * @description 对应后端 TaktAdminDivisionTreeDto
 */
export interface AdminDivisionTree extends AdminDivision {
  /**
   * 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）
   */
  children?: AdminDivisionTree[];

}


/**
 * AdminDivision 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 AdminDivisionQuery
 * @description 对应后端 TaktAdminDivisionQueryDto
 */
export interface AdminDivisionQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode?: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName?: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId?: string;

  /**
   * 层级（字典 sys_admin_division_level；1～6）
   */
  level?: number;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath?: string;

  /**
   * 是否叶子节点（字典 sys_yes_no）
   */
  isLeaf?: number;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode?: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 排序号（回填）
   */
  sortOrder?: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus?: number;

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
 * 创建AdminDivision DTO
 * 对应前端 AdminDivisionCreate
 * @description 对应后端 TaktAdminDivisionCreateDto
 */
export interface AdminDivisionCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId: string;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath: string;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus: number;

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
 * 更新AdminDivision DTO
 * 继承 TaktAdminDivisionCreateDto，添加 AdminDivisionId 字段
 * 对应前端 AdminDivisionUpdate
 * @description 对应后端 TaktAdminDivisionUpdateDto
 */
export interface AdminDivisionUpdate extends AdminDivisionCreate {
  /**
   * AdminDivisionID（标识要更新的实体）
   */
  adminDivisionId: string;

}


/**
 * AdminDivision 状态更新 DTO
 * 对应前端 AdminDivisionStatus
 * @description 对应后端 TaktAdminDivisionStatusDto
 */
export interface AdminDivisionStatus {
  /**
   * AdminDivisionID
   */
  adminDivisionId: string;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus: number;

}


/**
 * AdminDivision 排序更新 DTO
 * 对应前端 AdminDivisionSort
 * @description 对应后端 TaktAdminDivisionSortDto
 */
export interface AdminDivisionSort {
  /**
   * AdminDivisionID
   */
  adminDivisionId: string;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

}


/**
 * AdminDivision 导入模板行 DTO
 * 对应前端 AdminDivisionTemplate
 * @description 对应后端 TaktAdminDivisionTemplateDto
 */
export interface AdminDivisionTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode?: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName?: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId?: string;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath?: string;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode?: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus?: number;

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
 * AdminDivision 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 AdminDivisionImport
 * @description 对应后端 TaktAdminDivisionImportDto
 */
export interface AdminDivisionImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode?: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode?: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName?: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId?: string;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath?: string;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode?: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode?: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus?: number;

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
 * AdminDivision 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 AdminDivisionExport
 * @description 对应后端 TaktAdminDivisionExportDto
 */
export interface AdminDivisionExport {
  /**
   * AdminDivisionID
   */
  adminDivisionId: string;

  /**
   * 国家代码（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  countryCode: string;

  /**
   * 区划编码（唯一索引：租户内唯一；即标准代码，如 CN、CN-44、440100、440106）
   */
  divisionCode: string;

  /**
   * 区划名称（本国语言官方/本地显示名）
   */
  divisionName: string;

  /**
   * 父级区划ID（关联 TaktAdminDivision.Id；不可为空；根/国家必须为 0）
   */
  parentId: string;

  /**
   * 层级（字典 sys_admin_division_level；1～6）
   */
  level: number;

  /**
   * 区划路径（如 /1/3/5/，用于快速查询子孙）
   */
  divisionPath: string;

  /**
   * 是否叶子节点（字典 sys_yes_no）
   */
  isLeaf: number;

  /**
   * 邮政编码（可选；部分国家区划关联邮编）
   */
  postalCode?: string;

  /**
   * 币种（字典 accounting_financial_currency_code；ISO 4217，如 CNY/USD）
   */
  currencyCode: string;

  /**
   * 电话区号（国际电话区号，如 +86、+81）
   */
  phoneCode: string;

  /**
   * 内置（字典 sys_yes_no；内置项禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（回填）
   */
  sortOrder: number;

  /**
   * 区划状态（字典 sys_normal_disable）
   */
  divisionStatus: number;

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


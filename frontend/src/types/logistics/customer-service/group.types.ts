// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/customer-service
// 文件名称：group.types.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/customer-service 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 客户服务组主数据实体（公司级；按类别区分的客服业务组织分组）
 * 对应前端 TaktCustomerServiceGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CustomerServiceGroup
 * @description 对应后端 TaktCustomerServiceGroupDto
 */
export interface CustomerServiceGroup extends CompanyDtoBase {
  /**
   * CustomerServiceGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  customerServiceGroupId: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * CustomerServiceGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CustomerServiceGroupQuery
 * @description 对应后端 TaktCustomerServiceGroupQueryDto
 */
export interface CustomerServiceGroupQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode?: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName?: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory?: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus?: number;

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
 * 创建CustomerServiceGroup DTO
 * 对应前端 CustomerServiceGroupCreate
 * @description 对应后端 TaktCustomerServiceGroupCreateDto
 */
export interface CustomerServiceGroupCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

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
 * 更新CustomerServiceGroup DTO
 * 继承 TaktCustomerServiceGroupCreateDto，添加 CustomerServiceGroupId 字段
 * 对应前端 CustomerServiceGroupUpdate
 * @description 对应后端 TaktCustomerServiceGroupUpdateDto
 */
export interface CustomerServiceGroupUpdate extends CustomerServiceGroupCreate {
  /**
   * CustomerServiceGroupID（标识要更新的实体）
   */
  customerServiceGroupId: string;

}


/**
 * CustomerServiceGroup 状态更新 DTO
 * 对应前端 CustomerServiceGroupStatus
 * @description 对应后端 TaktCustomerServiceGroupStatusDto
 */
export interface CustomerServiceGroupStatus {
  /**
   * CustomerServiceGroupID
   */
  customerServiceGroupId: string;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * CustomerServiceGroup 排序更新 DTO
 * 对应前端 CustomerServiceGroupSort
 * @description 对应后端 TaktCustomerServiceGroupSortDto
 */
export interface CustomerServiceGroupSort {
  /**
   * CustomerServiceGroupID
   */
  customerServiceGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CustomerServiceGroup 内置更新 DTO
 * 对应前端 CustomerServiceGroupBuiltIn
 * @description 对应后端 TaktCustomerServiceGroupBuiltInDto
 */
export interface CustomerServiceGroupBuiltIn {
  /**
   * CustomerServiceGroupID
   */
  customerServiceGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * CustomerServiceGroup 导入模板行 DTO
 * 对应前端 CustomerServiceGroupTemplate
 * @description 对应后端 TaktCustomerServiceGroupTemplateDto
 */
export interface CustomerServiceGroupTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode?: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName?: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory?: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus?: number;

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
 * CustomerServiceGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CustomerServiceGroupImport
 * @description 对应后端 TaktCustomerServiceGroupImportDto
 */
export interface CustomerServiceGroupImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）
   */
  plantCode?: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode?: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName?: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory?: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn?: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus?: number;

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
 * CustomerServiceGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CustomerServiceGroupExport
 * @description 对应后端 TaktCustomerServiceGroupExportDto
 */
export interface CustomerServiceGroupExport {
  /**
   * CustomerServiceGroupID
   */
  customerServiceGroupId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string;

  /**
   * 客户服务组编码（与员工编码同长 6）
   */
  customerServiceGroupCode: string;

  /**
   * 客户服务组名称
   */
  customerServiceGroupName: string;

  /**
   * 客户服务组描述
   */
  customerServiceGroupDescription?: string;

  /**
   * 客户服务组类别（字典 logistics_customer_service_group_category；1=服务 2=工单）
   */
  customerServiceGroupCategory: number;

  /**
   * 联系电话
   */
  contactPhone?: string;

  /**
   * 联系邮箱
   */
  contactEmail?: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否；内置记录禁止删除）
   */
  isBuiltIn: number;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

  /**
   * 客户服务组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

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


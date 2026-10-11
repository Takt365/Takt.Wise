// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/sales
// 文件名称：group.types.ts
// 创建时间：2026-08-23
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/sales 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 销售组主数据实体（公司级；销售业务组织分组）
 * 对应前端 TaktSalesGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SalesGroup
 * @description 对应后端 TaktSalesGroupDto
 */
export interface SalesGroup extends CompanyDtoBase {
  /**
   * SalesGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  salesGroupId: string;

  /**
   * 销售组编码（3）
   */
  salesGroupCode: string;

  /**
   * 销售组名称
   */
  salesGroupName: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * SalesGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SalesGroupQuery
 * @description 对应后端 TaktSalesGroupQueryDto
 */
export interface SalesGroupQuery extends TaktPagedQuery {
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
   * 销售组编码（3）
   */
  salesGroupCode?: string;

  /**
   * 销售组名称
   */
  salesGroupName?: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory?: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建SalesGroup DTO
 * 对应前端 SalesGroupCreate
 * @description 对应后端 TaktSalesGroupCreateDto
 */
export interface SalesGroupCreate {
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
   * 销售组编码（3）
   */
  salesGroupCode: string;

  /**
   * 销售组名称
   */
  salesGroupName: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新SalesGroup DTO
 * 继承 TaktSalesGroupCreateDto，添加 SalesGroupId 字段
 * 对应前端 SalesGroupUpdate
 * @description 对应后端 TaktSalesGroupUpdateDto
 */
export interface SalesGroupUpdate extends SalesGroupCreate {
  /**
   * SalesGroupID（标识要更新的实体）
   */
  salesGroupId: string;

}

/**
 * SalesGroup 状态更新 DTO
 * 对应前端 SalesGroupStatus
 * @description 对应后端 TaktSalesGroupStatusDto
 */
export interface SalesGroupStatus {
  /**
   * SalesGroupID
   */
  salesGroupId: string;

  /**
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * SalesGroup 排序更新 DTO
 * 对应前端 SalesGroupSort
 * @description 对应后端 TaktSalesGroupSortDto
 */
export interface SalesGroupSort {
  /**
   * SalesGroupID
   */
  salesGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}

/**
 * SalesGroup 内置更新 DTO
 * 对应前端 SalesGroupBuiltIn
 * @description 对应后端 TaktSalesGroupBuiltInDto
 */
export interface SalesGroupBuiltIn {
  /**
   * SalesGroupID
   */
  salesGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}

/**
 * SalesGroup 导入模板行 DTO
 * 对应前端 SalesGroupTemplate
 * @description 对应后端 TaktSalesGroupTemplateDto
 */
export interface SalesGroupTemplate {
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
   * 销售组编码（3）
   */
  salesGroupCode?: string;

  /**
   * 销售组名称
   */
  salesGroupName?: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory?: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * SalesGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SalesGroupImport
 * @description 对应后端 TaktSalesGroupImportDto
 */
export interface SalesGroupImport {
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
   * 销售组编码（3）
   */
  salesGroupCode?: string;

  /**
   * 销售组名称
   */
  salesGroupName?: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory?: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * SalesGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SalesGroupExport
 * @description 对应后端 TaktSalesGroupExportDto
 */
export interface SalesGroupExport {
  /**
   * SalesGroupID
   */
  salesGroupId: string;

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
   * 销售组编码（3）
   */
  salesGroupCode: string;

  /**
   * 销售组名称
   */
  salesGroupName: string;

  /**
   * 销售组描述
   */
  salesGroupDescription?: string;

  /**
   * 销售组类别（字典 logistics_sales_sales_group_category；1=通用）
   */
  salesGroupCategory: number;

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
   * 销售组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


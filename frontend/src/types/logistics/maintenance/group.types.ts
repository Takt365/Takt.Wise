// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/maintenance
// 文件名称：group.types.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/maintenance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 设备维护组主数据实体（公司级；按类别区分的维护业务组织分组）
 * 对应前端 TaktMaintenanceGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 MaintenanceGroup
 * @description 对应后端 TaktMaintenanceGroupDto
 */
export interface MaintenanceGroup extends CompanyDtoBase {
  /**
   * MaintenanceGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  maintenanceGroupId: string;

  /**
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * MaintenanceGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 MaintenanceGroupQuery
 * @description 对应后端 TaktMaintenanceGroupQueryDto
 */
export interface MaintenanceGroupQuery extends TaktPagedQuery {
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
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode?: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName?: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory?: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建MaintenanceGroup DTO
 * 对应前端 MaintenanceGroupCreate
 * @description 对应后端 TaktMaintenanceGroupCreateDto
 */
export interface MaintenanceGroupCreate {
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
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新MaintenanceGroup DTO
 * 继承 TaktMaintenanceGroupCreateDto，添加 MaintenanceGroupId 字段
 * 对应前端 MaintenanceGroupUpdate
 * @description 对应后端 TaktMaintenanceGroupUpdateDto
 */
export interface MaintenanceGroupUpdate extends MaintenanceGroupCreate {
  /**
   * MaintenanceGroupID（标识要更新的实体）
   */
  maintenanceGroupId: string;

}


/**
 * MaintenanceGroup 状态更新 DTO
 * 对应前端 MaintenanceGroupStatus
 * @description 对应后端 TaktMaintenanceGroupStatusDto
 */
export interface MaintenanceGroupStatus {
  /**
   * MaintenanceGroupID
   */
  maintenanceGroupId: string;

  /**
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * MaintenanceGroup 排序更新 DTO
 * 对应前端 MaintenanceGroupSort
 * @description 对应后端 TaktMaintenanceGroupSortDto
 */
export interface MaintenanceGroupSort {
  /**
   * MaintenanceGroupID
   */
  maintenanceGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * MaintenanceGroup 内置更新 DTO
 * 对应前端 MaintenanceGroupBuiltIn
 * @description 对应后端 TaktMaintenanceGroupBuiltInDto
 */
export interface MaintenanceGroupBuiltIn {
  /**
   * MaintenanceGroupID
   */
  maintenanceGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * MaintenanceGroup 导入模板行 DTO
 * 对应前端 MaintenanceGroupTemplate
 * @description 对应后端 TaktMaintenanceGroupTemplateDto
 */
export interface MaintenanceGroupTemplate {
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
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode?: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName?: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory?: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * MaintenanceGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 MaintenanceGroupImport
 * @description 对应后端 TaktMaintenanceGroupImportDto
 */
export interface MaintenanceGroupImport {
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
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode?: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName?: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory?: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * MaintenanceGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 MaintenanceGroupExport
 * @description 对应后端 TaktMaintenanceGroupExportDto
 */
export interface MaintenanceGroupExport {
  /**
   * MaintenanceGroupID
   */
  maintenanceGroupId: string;

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
   * 设备维护组编码（与员工编码同长 6）
   */
  maintenanceGroupCode: string;

  /**
   * 设备维护组名称
   */
  maintenanceGroupName: string;

  /**
   * 设备维护组描述
   */
  maintenanceGroupDescription?: string;

  /**
   * 设备维护组类别（字典 logistics_maintenance_group_category；1=设备 2=维护）
   */
  maintenanceGroupCategory: number;

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
   * 设备维护组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


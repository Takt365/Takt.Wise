// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/aps
// 文件名称：group.types.ts
// 创建时间：2026-10-08
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
 * APS组主数据实体（公司级；按计划类别区分的业务组织分组）
 * 对应前端 TaktApsGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ApsGroup
 * @description 对应后端 TaktApsGroupDto
 */
export interface ApsGroup extends CompanyDtoBase {
  /**
   * ApsGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  apsGroupId: string;

  /**
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode: string;

  /**
   * APS组名称
   */
  apsGroupName: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * ApsGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ApsGroupQuery
 * @description 对应后端 TaktApsGroupQueryDto
 */
export interface ApsGroupQuery extends TaktPagedQuery {
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
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode?: string;

  /**
   * APS组名称
   */
  apsGroupName?: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory?: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建ApsGroup DTO
 * 对应前端 ApsGroupCreate
 * @description 对应后端 TaktApsGroupCreateDto
 */
export interface ApsGroupCreate {
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
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode: string;

  /**
   * APS组名称
   */
  apsGroupName: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新ApsGroup DTO
 * 继承 TaktApsGroupCreateDto，添加 ApsGroupId 字段
 * 对应前端 ApsGroupUpdate
 * @description 对应后端 TaktApsGroupUpdateDto
 */
export interface ApsGroupUpdate extends ApsGroupCreate {
  /**
   * ApsGroupID（标识要更新的实体）
   */
  apsGroupId: string;

}


/**
 * ApsGroup 状态更新 DTO
 * 对应前端 ApsGroupStatus
 * @description 对应后端 TaktApsGroupStatusDto
 */
export interface ApsGroupStatus {
  /**
   * ApsGroupID
   */
  apsGroupId: string;

  /**
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * ApsGroup 排序更新 DTO
 * 对应前端 ApsGroupSort
 * @description 对应后端 TaktApsGroupSortDto
 */
export interface ApsGroupSort {
  /**
   * ApsGroupID
   */
  apsGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ApsGroup 内置更新 DTO
 * 对应前端 ApsGroupBuiltIn
 * @description 对应后端 TaktApsGroupBuiltInDto
 */
export interface ApsGroupBuiltIn {
  /**
   * ApsGroupID
   */
  apsGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * ApsGroup 导入模板行 DTO
 * 对应前端 ApsGroupTemplate
 * @description 对应后端 TaktApsGroupTemplateDto
 */
export interface ApsGroupTemplate {
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
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode?: string;

  /**
   * APS组名称
   */
  apsGroupName?: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory?: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * ApsGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ApsGroupImport
 * @description 对应后端 TaktApsGroupImportDto
 */
export interface ApsGroupImport {
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
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode?: string;

  /**
   * APS组名称
   */
  apsGroupName?: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory?: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * ApsGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ApsGroupExport
 * @description 对应后端 TaktApsGroupExportDto
 */
export interface ApsGroupExport {
  /**
   * ApsGroupID
   */
  apsGroupId: string;

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
   * APS组编码（与员工编码同长 6）
   */
  apsGroupCode: string;

  /**
   * APS组名称
   */
  apsGroupName: string;

  /**
   * APS组描述
   */
  apsGroupDescription?: string;

  /**
   * APS组类别（字典 logistics_manufacturing_aps_group_category；1=计划员 2=发布员 3=设备操作员 4=设备维护员）
   */
  apsGroupCategory: number;

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
   * APS组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


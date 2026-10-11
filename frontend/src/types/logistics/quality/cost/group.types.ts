// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/cost
// 文件名称：group.types.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/cost 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 成本组主数据实体（公司级；按品质成本业务类别区分的组织分组）
 * 对应前端 TaktCostGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 CostGroup
 * @description 对应后端 TaktCostGroupDto
 */
export interface CostGroup extends CompanyDtoBase {
  /**
   * CostGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  costGroupId: string;

  /**
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode: string;

  /**
   * 成本组名称
   */
  costGroupName: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * CostGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CostGroupQuery
 * @description 对应后端 TaktCostGroupQueryDto
 */
export interface CostGroupQuery extends TaktPagedQuery {
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
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode?: string;

  /**
   * 成本组名称
   */
  costGroupName?: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory?: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建CostGroup DTO
 * 对应前端 CostGroupCreate
 * @description 对应后端 TaktCostGroupCreateDto
 */
export interface CostGroupCreate {
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
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode: string;

  /**
   * 成本组名称
   */
  costGroupName: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新CostGroup DTO
 * 继承 TaktCostGroupCreateDto，添加 CostGroupId 字段
 * 对应前端 CostGroupUpdate
 * @description 对应后端 TaktCostGroupUpdateDto
 */
export interface CostGroupUpdate extends CostGroupCreate {
  /**
   * CostGroupID（标识要更新的实体）
   */
  costGroupId: string;

}


/**
 * CostGroup 状态更新 DTO
 * 对应前端 CostGroupStatus
 * @description 对应后端 TaktCostGroupStatusDto
 */
export interface CostGroupStatus {
  /**
   * CostGroupID
   */
  costGroupId: string;

  /**
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * CostGroup 排序更新 DTO
 * 对应前端 CostGroupSort
 * @description 对应后端 TaktCostGroupSortDto
 */
export interface CostGroupSort {
  /**
   * CostGroupID
   */
  costGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * CostGroup 内置更新 DTO
 * 对应前端 CostGroupBuiltIn
 * @description 对应后端 TaktCostGroupBuiltInDto
 */
export interface CostGroupBuiltIn {
  /**
   * CostGroupID
   */
  costGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * CostGroup 导入模板行 DTO
 * 对应前端 CostGroupTemplate
 * @description 对应后端 TaktCostGroupTemplateDto
 */
export interface CostGroupTemplate {
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
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode?: string;

  /**
   * 成本组名称
   */
  costGroupName?: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory?: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * CostGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CostGroupImport
 * @description 对应后端 TaktCostGroupImportDto
 */
export interface CostGroupImport {
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
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode?: string;

  /**
   * 成本组名称
   */
  costGroupName?: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory?: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * CostGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CostGroupExport
 * @description 对应后端 TaktCostGroupExportDto
 */
export interface CostGroupExport {
  /**
   * CostGroupID
   */
  costGroupId: string;

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
   * 成本组编码（与员工编码同长 6）
   */
  costGroupCode: string;

  /**
   * 成本组名称
   */
  costGroupName: string;

  /**
   * 成本组描述
   */
  costGroupDescription?: string;

  /**
   * 成本组类别（字典 logistics_quality_cost_group_category；1=记录组 2=废弃组 3=对应组）
   */
  costGroupCategory: number;

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
   * 成本组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


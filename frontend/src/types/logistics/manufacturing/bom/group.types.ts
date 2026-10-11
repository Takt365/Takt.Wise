// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：group.types.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/bom 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * BOM组主数据实体（公司级；按类别区分的 BOM/工艺业务组织分组）
 * 对应前端 TaktBomGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 BomGroup
 * @description 对应后端 TaktBomGroupDto
 */
export interface BomGroup extends CompanyDtoBase {
  /**
   * BomGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  bomGroupId: string;

  /**
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode: string;

  /**
   * BOM组名称
   */
  bomGroupName: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * BomGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 BomGroupQuery
 * @description 对应后端 TaktBomGroupQueryDto
 */
export interface BomGroupQuery extends TaktPagedQuery {
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
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称
   */
  bomGroupName?: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory?: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建BomGroup DTO
 * 对应前端 BomGroupCreate
 * @description 对应后端 TaktBomGroupCreateDto
 */
export interface BomGroupCreate {
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
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode: string;

  /**
   * BOM组名称
   */
  bomGroupName: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新BomGroup DTO
 * 继承 TaktBomGroupCreateDto，添加 BomGroupId 字段
 * 对应前端 BomGroupUpdate
 * @description 对应后端 TaktBomGroupUpdateDto
 */
export interface BomGroupUpdate extends BomGroupCreate {
  /**
   * BomGroupID（标识要更新的实体）
   */
  bomGroupId: string;

}


/**
 * BomGroup 状态更新 DTO
 * 对应前端 BomGroupStatus
 * @description 对应后端 TaktBomGroupStatusDto
 */
export interface BomGroupStatus {
  /**
   * BomGroupID
   */
  bomGroupId: string;

  /**
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * BomGroup 排序更新 DTO
 * 对应前端 BomGroupSort
 * @description 对应后端 TaktBomGroupSortDto
 */
export interface BomGroupSort {
  /**
   * BomGroupID
   */
  bomGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * BomGroup 内置更新 DTO
 * 对应前端 BomGroupBuiltIn
 * @description 对应后端 TaktBomGroupBuiltInDto
 */
export interface BomGroupBuiltIn {
  /**
   * BomGroupID
   */
  bomGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * BomGroup 导入模板行 DTO
 * 对应前端 BomGroupTemplate
 * @description 对应后端 TaktBomGroupTemplateDto
 */
export interface BomGroupTemplate {
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
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称
   */
  bomGroupName?: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory?: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * BomGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 BomGroupImport
 * @description 对应后端 TaktBomGroupImportDto
 */
export interface BomGroupImport {
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
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode?: string;

  /**
   * BOM组名称
   */
  bomGroupName?: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory?: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * BomGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 BomGroupExport
 * @description 对应后端 TaktBomGroupExportDto
 */
export interface BomGroupExport {
  /**
   * BomGroupID
   */
  bomGroupId: string;

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
   * BOM组编码（与员工编码同长 6）
   */
  bomGroupCode: string;

  /**
   * BOM组名称
   */
  bomGroupName: string;

  /**
   * BOM组描述
   */
  bomGroupDescription?: string;

  /**
   * BOM组类别（字典 logistics_manufacturing_bom_group_category；1=BOM 2=工艺）
   */
  bomGroupCategory: number;

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
   * BOM组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


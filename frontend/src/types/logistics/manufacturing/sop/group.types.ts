// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/sop
// 文件名称：group.types.ts
// 创建时间：2026-10-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/sop 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * SOP组主数据实体（公司级；按 SOP 业务类别区分的组织分组）
 * 对应前端 TaktSopGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 SopGroup
 * @description 对应后端 TaktSopGroupDto
 */
export interface SopGroup extends CompanyDtoBase {
  /**
   * SopGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  sopGroupId: string;

  /**
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode: string;

  /**
   * SOP组名称
   */
  sopGroupName: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * SopGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 SopGroupQuery
 * @description 对应后端 TaktSopGroupQueryDto
 */
export interface SopGroupQuery extends TaktPagedQuery {
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
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode?: string;

  /**
   * SOP组名称
   */
  sopGroupName?: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory?: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建SopGroup DTO
 * 对应前端 SopGroupCreate
 * @description 对应后端 TaktSopGroupCreateDto
 */
export interface SopGroupCreate {
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
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode: string;

  /**
   * SOP组名称
   */
  sopGroupName: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新SopGroup DTO
 * 继承 TaktSopGroupCreateDto，添加 SopGroupId 字段
 * 对应前端 SopGroupUpdate
 * @description 对应后端 TaktSopGroupUpdateDto
 */
export interface SopGroupUpdate extends SopGroupCreate {
  /**
   * SopGroupID（标识要更新的实体）
   */
  sopGroupId: string;

}


/**
 * SopGroup 状态更新 DTO
 * 对应前端 SopGroupStatus
 * @description 对应后端 TaktSopGroupStatusDto
 */
export interface SopGroupStatus {
  /**
   * SopGroupID
   */
  sopGroupId: string;

  /**
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * SopGroup 排序更新 DTO
 * 对应前端 SopGroupSort
 * @description 对应后端 TaktSopGroupSortDto
 */
export interface SopGroupSort {
  /**
   * SopGroupID
   */
  sopGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * SopGroup 内置更新 DTO
 * 对应前端 SopGroupBuiltIn
 * @description 对应后端 TaktSopGroupBuiltInDto
 */
export interface SopGroupBuiltIn {
  /**
   * SopGroupID
   */
  sopGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * SopGroup 导入模板行 DTO
 * 对应前端 SopGroupTemplate
 * @description 对应后端 TaktSopGroupTemplateDto
 */
export interface SopGroupTemplate {
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
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode?: string;

  /**
   * SOP组名称
   */
  sopGroupName?: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory?: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * SopGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 SopGroupImport
 * @description 对应后端 TaktSopGroupImportDto
 */
export interface SopGroupImport {
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
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode?: string;

  /**
   * SOP组名称
   */
  sopGroupName?: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory?: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * SopGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 SopGroupExport
 * @description 对应后端 TaktSopGroupExportDto
 */
export interface SopGroupExport {
  /**
   * SopGroupID
   */
  sopGroupId: string;

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
   * SOP组编码（与员工编码同长 6）
   */
  sopGroupCode: string;

  /**
   * SOP组名称
   */
  sopGroupName: string;

  /**
   * SOP组描述
   */
  sopGroupDescription?: string;

  /**
   * SOP组类别（字典 logistics_manufacturing_sop_group_category；1=工位 2=文档 3=版本 4=确认 5=执行 6=扫码 7=ESD 8=安灯）
   */
  sopGroupCategory: number;

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
   * SOP组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


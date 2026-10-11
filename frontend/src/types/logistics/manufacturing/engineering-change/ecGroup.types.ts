// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/engineering-change
// 文件名称：ecGroup.types.ts
// 创建时间：2026-07-08
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/engineering-change 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 设变组主数据实体（公司级；设变业务组织分组）
 * 对应前端 TaktEcGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EcGroup
 * @description 对应后端 TaktEcGroupDto
 */
export interface EcGroup extends CompanyDtoBase {

  /**
   * 设变组编码（3）
   */
  ecGroupCode: string;

  /**
   * 设变组名称
   */
  ecGroupName: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory: string;

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
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * EcGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EcGroupQuery
 * @description 对应后端 TaktEcGroupQueryDto
 */
export interface EcGroupQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 设变组编码（3）
   */
  ecGroupCode?: string;

  /**
   * 设变组名称
   */
  ecGroupName?: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory?: string;

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
   * 排序号（越小越靠前）
   */
  sortOrder?: number;

  /**
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建EcGroup DTO
 * 对应前端 EcGroupCreate
 * @description 对应后端 TaktEcGroupCreateDto
 */
export interface EcGroupCreate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 设变组编码（3）
   */
  ecGroupCode: string;

  /**
   * 设变组名称
   */
  ecGroupName: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory: string;

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
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新EcGroup DTO
 * 继承 TaktEcGroupCreateDto，添加 EcGroupId 字段
 * 对应前端 EcGroupUpdate
 * @description 对应后端 TaktEcGroupUpdateDto
 */
export interface EcGroupUpdate extends EcGroupCreate {
  /**
   * EcGroupID（标识要更新的实体）
   */
  ecGroupId: string;

}

/**
 * EcGroup 状态更新 DTO
 * 对应前端 EcGroupStatus
 * @description 对应后端 TaktEcGroupStatusDto
 */
export interface EcGroupStatus {
  /**
   * EcGroupID
   */
  ecGroupId: string;

  /**
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * EcGroup 排序更新 DTO
 * 对应前端 EcGroupSort
 * @description 对应后端 TaktEcGroupSortDto
 */
export interface EcGroupSort {
  /**
   * EcGroupID
   */
  ecGroupId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}

/**
 * EcGroup 导入模板行 DTO
 * 对应前端 EcGroupTemplate
 * @description 对应后端 TaktEcGroupTemplateDto
 */
export interface EcGroupTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 设变组编码（3）
   */
  ecGroupCode?: string;

  /**
   * 设变组名称
   */
  ecGroupName?: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory?: string;

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
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * EcGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EcGroupImport
 * @description 对应后端 TaktEcGroupImportDto
 */
export interface EcGroupImport {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 当前公司区域文化 BCP47（登录或公司切换注入，须与 takt_company.default_culture 一致，用于写入校验）
   */
  /**
   * 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
   */
  cultureCode?: string

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode?: string;

  /**
   * 设变组编码（3）
   */
  ecGroupCode?: string;

  /**
   * 设变组名称
   */
  ecGroupName?: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory?: string;

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
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * EcGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EcGroupExport
 * @description 对应后端 TaktEcGroupExportDto
 */
export interface EcGroupExport {
  /**
   * EcGroupID
   */
  ecGroupId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 设变组编码（3）
   */
  ecGroupCode: string;

  /**
   * 设变组名称
   */
  ecGroupName: string;

  /**
   * 设变组描述
   */
  ecGroupDescription?: string;

  /**
   * 设变组类别（字典 logistics_manufacturing_ec_group_category；1=EC技术担当 2=SOP制技担当 10=EC品管担当）
   */
  ecGroupCategory: string;

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
   * 排序号（越小越靠前）
   */
  sortOrder: number;

  /**
   * 设变组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


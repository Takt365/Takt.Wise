// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/complaint
// 文件名称：group.types.ts
// 创建时间：2026-10-11
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/complaint 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 客诉组主数据实体（公司级；按客诉业务类别区分的组织分组）
 * 对应前端 TaktComplaintGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ComplaintGroup
 * @description 对应后端 TaktComplaintGroupDto
 */
export interface ComplaintGroup extends CompanyDtoBase {
  /**
   * ComplaintGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  complaintGroupId: string;

  /**
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode: string;

  /**
   * 客诉组名称
   */
  complaintGroupName: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * ComplaintGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ComplaintGroupQuery
 * @description 对应后端 TaktComplaintGroupQueryDto
 */
export interface ComplaintGroupQuery extends TaktPagedQuery {
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
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode?: string;

  /**
   * 客诉组名称
   */
  complaintGroupName?: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory?: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建ComplaintGroup DTO
 * 对应前端 ComplaintGroupCreate
 * @description 对应后端 TaktComplaintGroupCreateDto
 */
export interface ComplaintGroupCreate {
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
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode: string;

  /**
   * 客诉组名称
   */
  complaintGroupName: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新ComplaintGroup DTO
 * 继承 TaktComplaintGroupCreateDto，添加 ComplaintGroupId 字段
 * 对应前端 ComplaintGroupUpdate
 * @description 对应后端 TaktComplaintGroupUpdateDto
 */
export interface ComplaintGroupUpdate extends ComplaintGroupCreate {
  /**
   * ComplaintGroupID（标识要更新的实体）
   */
  complaintGroupId: string;

}


/**
 * ComplaintGroup 状态更新 DTO
 * 对应前端 ComplaintGroupStatus
 * @description 对应后端 TaktComplaintGroupStatusDto
 */
export interface ComplaintGroupStatus {
  /**
   * ComplaintGroupID
   */
  complaintGroupId: string;

  /**
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}


/**
 * ComplaintGroup 排序更新 DTO
 * 对应前端 ComplaintGroupSort
 * @description 对应后端 TaktComplaintGroupSortDto
 */
export interface ComplaintGroupSort {
  /**
   * ComplaintGroupID
   */
  complaintGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}


/**
 * ComplaintGroup 内置更新 DTO
 * 对应前端 ComplaintGroupBuiltIn
 * @description 对应后端 TaktComplaintGroupBuiltInDto
 */
export interface ComplaintGroupBuiltIn {
  /**
   * ComplaintGroupID
   */
  complaintGroupId: string;

  /**
   * 内置（字典 sys_yes_no；1=是，0=否）
   */
  isBuiltIn: number;

}


/**
 * ComplaintGroup 导入模板行 DTO
 * 对应前端 ComplaintGroupTemplate
 * @description 对应后端 TaktComplaintGroupTemplateDto
 */
export interface ComplaintGroupTemplate {
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
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode?: string;

  /**
   * 客诉组名称
   */
  complaintGroupName?: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory?: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * ComplaintGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ComplaintGroupImport
 * @description 对应后端 TaktComplaintGroupImportDto
 */
export interface ComplaintGroupImport {
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
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode?: string;

  /**
   * 客诉组名称
   */
  complaintGroupName?: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory?: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * ComplaintGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ComplaintGroupExport
 * @description 对应后端 TaktComplaintGroupExportDto
 */
export interface ComplaintGroupExport {
  /**
   * ComplaintGroupID
   */
  complaintGroupId: string;

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
   * 客诉组编码（与员工编码同长 6）
   */
  complaintGroupCode: string;

  /**
   * 客诉组名称
   */
  complaintGroupName: string;

  /**
   * 客诉组描述
   */
  complaintGroupDescription?: string;

  /**
   * 客诉组类别（字典 logistics_quality_complaint_group_category；1=责任组 2=处理组 3=改善组 4=评价组 5=调查组）
   */
  complaintGroupCategory: number;

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
   * 客诉组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/defect
// 文件名称：group.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/manufacturing/defect 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 不良组主数据实体（公司级；按不良组类别区分的不良业务组织分组）
 * 对应前端 TaktDefectGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 DefectGroup
 * @description 对应后端 TaktDefectGroupDto
 */
export interface DefectGroup extends CompanyDtoBase {
  /**
   * DefectGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  defectGroupId: string;

  /**
   * 不良组编码（3）
   */
  defectGroupCode: string;

  /**
   * 不良组名称
   */
  defectGroupName: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

  /**
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory: number;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * DefectGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 DefectGroupQuery
 * @description 对应后端 TaktDefectGroupQueryDto
 */
export interface DefectGroupQuery extends TaktPagedQuery {
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
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory?: number;

  /**
   * 不良组编码（3）
   */
  defectGroupCode?: string;

  /**
   * 不良组名称
   */
  defectGroupName?: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建DefectGroup DTO
 * 对应前端 DefectGroupCreate
 * @description 对应后端 TaktDefectGroupCreateDto
 */
export interface DefectGroupCreate {
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
   * 不良组编码（3）
   */
  defectGroupCode: string;

  /**
   * 不良组名称
   */
  defectGroupName: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

  /**
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory: number;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新DefectGroup DTO
 * 继承 TaktDefectGroupCreateDto，添加 DefectGroupId 字段
 * 对应前端 DefectGroupUpdate
 * @description 对应后端 TaktDefectGroupUpdateDto
 */
export interface DefectGroupUpdate extends DefectGroupCreate {
  /**
   * DefectGroupID（标识要更新的实体）
   */
  defectGroupId: string;

}

/**
 * DefectGroup 状态更新 DTO
 * 对应前端 DefectGroupStatus
 * @description 对应后端 TaktDefectGroupStatusDto
 */
export interface DefectGroupStatus {
  /**
   * DefectGroupID
   */
  defectGroupId: string;

  /**
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * DefectGroup 排序更新 DTO
 * 对应前端 DefectGroupSort
 * @description 对应后端 TaktDefectGroupSortDto
 */
export interface DefectGroupSort {
  /**
   * DefectGroupID
   */
  defectGroupId: string;

  /**
   * 排序号（回填）（越小越靠前）
   */
  sortOrder: number;

}

/**
 * DefectGroup 导入模板行 DTO
 * 对应前端 DefectGroupTemplate
 * @description 对应后端 TaktDefectGroupTemplateDto
 */
export interface DefectGroupTemplate {
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
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory?: number;

  /**
   * 不良组编码（3）
   */
  defectGroupCode?: string;

  /**
   * 不良组名称
   */
  defectGroupName?: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * DefectGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 DefectGroupImport
 * @description 对应后端 TaktDefectGroupImportDto
 */
export interface DefectGroupImport {
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
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory?: number;

  /**
   * 不良组编码（3）
   */
  defectGroupCode?: string;

  /**
   * 不良组名称
   */
  defectGroupName?: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * DefectGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 DefectGroupExport
 * @description 对应后端 TaktDefectGroupExportDto
 */
export interface DefectGroupExport {
  /**
   * DefectGroupID
   */
  defectGroupId: string;

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
   * 不良组编码（3）
   */
  defectGroupCode: string;

  /**
   * 不良组名称
   */
  defectGroupName: string;

  /**
   * 不良组描述
   */
  defectGroupDescription?: string;

  /**
   * 不良组类别（字典 logistics_manufacturing_defect_group_category；0=Assy，1=Inspection，2=Repair）
   */
  defectGroupCategory: number;

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
   * 不良组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


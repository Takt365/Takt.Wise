// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/quality/operation
// 文件名称：group.types.ts
// 创建时间：2026-07-09
// 创建人：Takt365(Auto Generated)
// 功能描述：logistics/quality/operation 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 质量组主数据实体（公司级；按质量组类别区分的质量业务组织分组）
 * 对应前端 TaktQualityGroupDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 QualityGroup
 * @description 对应后端 TaktQualityGroupDto
 */
export interface QualityGroup extends CompanyDtoBase {
  /**
   * QualityGroupID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  qualityGroupId: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 质量组编码（3）
   */
  qualityGroupCode: string;

  /**
   * 质量组名称
   */
  qualityGroupName: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

  /**
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory: number;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * QualityGroup 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 QualityGroupQuery
 * @description 对应后端 TaktQualityGroupQueryDto
 */
export interface QualityGroupQuery extends TaktPagedQuery {
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
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory?: number;

  /**
   * 质量组编码（3）
   */
  qualityGroupCode?: string;

  /**
   * 质量组名称
   */
  qualityGroupName?: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 创建QualityGroup DTO
 * 对应前端 QualityGroupCreate
 * @description 对应后端 TaktQualityGroupCreateDto
 */
export interface QualityGroupCreate {
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
   * 质量组编码（3）
   */
  qualityGroupCode: string;

  /**
   * 质量组名称
   */
  qualityGroupName: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

  /**
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory: number;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * 更新QualityGroup DTO
 * 继承 TaktQualityGroupCreateDto，添加 QualityGroupId 字段
 * 对应前端 QualityGroupUpdate
 * @description 对应后端 TaktQualityGroupUpdateDto
 */
export interface QualityGroupUpdate extends QualityGroupCreate {
  /**
   * QualityGroupID（标识要更新的实体）
   */
  qualityGroupId: string;

}

/**
 * QualityGroup 状态更新 DTO
 * 对应前端 QualityGroupStatus
 * @description 对应后端 TaktQualityGroupStatusDto
 */
export interface QualityGroupStatus {
  /**
   * QualityGroupID
   */
  qualityGroupId: string;

  /**
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
   */
  groupStatus: number;

}

/**
 * QualityGroup 排序更新 DTO
 * 对应前端 QualityGroupSort
 * @description 对应后端 TaktQualityGroupSortDto
 */
export interface QualityGroupSort {
  /**
   * QualityGroupID
   */
  qualityGroupId: string;

  /**
   * 排序号（越小越靠前）
   */
  sortOrder: number;

}

/**
 * QualityGroup 导入模板行 DTO
 * 对应前端 QualityGroupTemplate
 * @description 对应后端 TaktQualityGroupTemplateDto
 */
export interface QualityGroupTemplate {
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
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory?: number;

  /**
   * 质量组编码（3）
   */
  qualityGroupCode?: string;

  /**
   * 质量组名称
   */
  qualityGroupName?: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * QualityGroup 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 QualityGroupImport
 * @description 对应后端 TaktQualityGroupImportDto
 */
export interface QualityGroupImport {
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
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory?: number;

  /**
   * 质量组编码（3）
   */
  qualityGroupCode?: string;

  /**
   * 质量组名称
   */
  qualityGroupName?: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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
 * QualityGroup 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 QualityGroupExport
 * @description 对应后端 TaktQualityGroupExportDto
 */
export interface QualityGroupExport {
  /**
   * QualityGroupID
   */
  qualityGroupId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 工厂代码（选项 TaktPlants/options，DictValue=PlantCode）
   */
  plantCode: string;

  /**
   * 质量组编码（3）
   */
  qualityGroupCode: string;

  /**
   * 质量组名称
   */
  qualityGroupName: string;

  /**
   * 质量组描述
   */
  qualityGroupDescription?: string;

  /**
   * 质量组类别（字典 logistics_quality_group_category；0=IQC组 1=QA组 2=IPQC组 3=责任组 4=处理组 5=质量负责人组）
   */
  qualityGroupCategory: number;

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
   * 质量组状态（字典 sys_normal_disable；1=启用，0=禁用）
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


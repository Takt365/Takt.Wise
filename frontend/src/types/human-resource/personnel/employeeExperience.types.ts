// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeExperience.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工外部工作经历
 * 对应前端 TaktEmployeeExperienceDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeExperience
 * @description 对应后端 TaktEmployeeExperienceDto
 */
export interface EmployeeExperience extends CompanyDtoBase {
  /**
   * EmployeeExperienceID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeExperienceId: string;

  /**
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 工作单位名称
   */
  companyName: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeExperience 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeExperienceQuery
 * @description 对应后端 TaktEmployeeExperienceQueryDto
 */
export interface EmployeeExperienceQuery extends TaktPagedQuery {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 工作单位名称
   */
  companyName?: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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
 * 创建EmployeeExperience DTO
 * 对应前端 EmployeeExperienceCreate
 * @description 对应后端 TaktEmployeeExperienceCreateDto
 */
export interface EmployeeExperienceCreate {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 工作单位名称
   */
  companyName: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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
 * 更新EmployeeExperience DTO
 * 继承 TaktEmployeeExperienceCreateDto，添加 EmployeeExperienceId 字段
 * 对应前端 EmployeeExperienceUpdate
 * @description 对应后端 TaktEmployeeExperienceUpdateDto
 */
export interface EmployeeExperienceUpdate extends EmployeeExperienceCreate {
  /**
   * EmployeeExperienceID（标识要更新的实体）
   */
  employeeExperienceId: string;

}


/**
 * EmployeeExperience 导入模板行 DTO
 * 对应前端 EmployeeExperienceTemplate
 * @description 对应后端 TaktEmployeeExperienceTemplateDto
 */
export interface EmployeeExperienceTemplate {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 工作单位名称
   */
  companyName?: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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
 * EmployeeExperience 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeExperienceImport
 * @description 对应后端 TaktEmployeeExperienceImportDto
 */
export interface EmployeeExperienceImport {
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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId?: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode?: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName?: string;

  /**
   * 工作单位名称
   */
  companyName?: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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
 * EmployeeExperience 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeExperienceExport
 * @description 对应后端 TaktEmployeeExperienceExportDto
 */
export interface EmployeeExperienceExport {
  /**
   * EmployeeExperienceID
   */
  employeeExperienceId: string;

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
   * 员工（选项 TaktEmployees/options；DictValue=Id）
   */
  employeeId: string;

  /**
   * 员工编码（冗余，与 TaktEmployee.EmployeeCode 对齐）
   */
  employeeCode: string;

  /**
   * 员工姓名（冗余，与 TaktEmployee.EmployeeName 对齐）
   */
  employeeName: string;

  /**
   * 工作单位名称
   */
  companyName: string;

  /**
   * 职位名称
   */
  positionName?: string;

  /**
   * 工作内容
   */
  jobContent?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 证明人姓名
   */
  witnessName?: string;

  /**
   * 证明人电话
   */
  witnessPhone?: string;

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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeEducation.types.ts
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
 * 员工教育经历
 * 对应前端 TaktEmployeeEducationDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeEducation
 * @description 对应后端 TaktEmployeeEducationDto
 */
export interface EmployeeEducation extends CompanyDtoBase {
  /**
   * EmployeeEducationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeEducationId: string;

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
   * 学校名称
   */
  schoolName: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest: number;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeEducation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeEducationQuery
 * @description 对应后端 TaktEmployeeEducationQueryDto
 */
export interface EmployeeEducationQuery extends TaktPagedQuery {
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
   * 学校名称
   */
  schoolName?: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

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
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest?: number;

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
 * 创建EmployeeEducation DTO
 * 对应前端 EmployeeEducationCreate
 * @description 对应后端 TaktEmployeeEducationCreateDto
 */
export interface EmployeeEducationCreate {
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
   * 学校名称
   */
  schoolName: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest: number;

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
 * 更新EmployeeEducation DTO
 * 继承 TaktEmployeeEducationCreateDto，添加 EmployeeEducationId 字段
 * 对应前端 EmployeeEducationUpdate
 * @description 对应后端 TaktEmployeeEducationUpdateDto
 */
export interface EmployeeEducationUpdate extends EmployeeEducationCreate {
  /**
   * EmployeeEducationID（标识要更新的实体）
   */
  employeeEducationId: string;

}


/**
 * EmployeeEducation 导入模板行 DTO
 * 对应前端 EmployeeEducationTemplate
 * @description 对应后端 TaktEmployeeEducationTemplateDto
 */
export interface EmployeeEducationTemplate {
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
   * 学校名称
   */
  schoolName?: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest?: number;

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
 * EmployeeEducation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeEducationImport
 * @description 对应后端 TaktEmployeeEducationImportDto
 */
export interface EmployeeEducationImport {
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
   * 学校名称
   */
  schoolName?: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest?: number;

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
 * EmployeeEducation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeEducationExport
 * @description 对应后端 TaktEmployeeEducationExportDto
 */
export interface EmployeeEducationExport {
  /**
   * EmployeeEducationID
   */
  employeeEducationId: string;

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
   * 学校名称
   */
  schoolName: string;

  /**
   * 学历层次（字典 humanresource_personnel_education_level；1=高中及以下 2=大专 3=本科 4=硕士 5=博士）
   */
  educationLevel?: number;

  /**
   * 学位层次（字典 humanresource_personnel_degree_level；0=无 1=学士 2=硕士 3=博士）
   */
  degreeLevel?: number;

  /**
   * 专业名称
   */
  majorName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 是否最高学历（字典 sys_yes_no；0=否 1=是）
   */
  isHighest: number;

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


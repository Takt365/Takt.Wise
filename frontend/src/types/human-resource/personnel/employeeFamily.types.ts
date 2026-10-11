// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeFamily.types.ts
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
 * 员工家庭成员
 * 对应前端 TaktEmployeeFamilyDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeFamily
 * @description 对应后端 TaktEmployeeFamilyDto
 */
export interface EmployeeFamily extends CompanyDtoBase {
  /**
   * EmployeeFamilyID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeFamilyId: string;

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
   * 成员姓名
   */
  memberName: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact: number;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeFamily 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeFamilyQuery
 * @description 对应后端 TaktEmployeeFamilyQueryDto
 */
export interface EmployeeFamilyQuery extends TaktPagedQuery {
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
   * 成员姓名
   */
  memberName?: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType?: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期（范围查询-开始）
   */
  birthDateStart?: string;

  /**
   * 出生日期（范围查询-结束）
   */
  birthDateEnd?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact?: number;

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
 * 创建EmployeeFamily DTO
 * 对应前端 EmployeeFamilyCreate
 * @description 对应后端 TaktEmployeeFamilyCreateDto
 */
export interface EmployeeFamilyCreate {
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
   * 成员姓名
   */
  memberName: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact: number;

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
 * 更新EmployeeFamily DTO
 * 继承 TaktEmployeeFamilyCreateDto，添加 EmployeeFamilyId 字段
 * 对应前端 EmployeeFamilyUpdate
 * @description 对应后端 TaktEmployeeFamilyUpdateDto
 */
export interface EmployeeFamilyUpdate extends EmployeeFamilyCreate {
  /**
   * EmployeeFamilyID（标识要更新的实体）
   */
  employeeFamilyId: string;

}


/**
 * EmployeeFamily 导入模板行 DTO
 * 对应前端 EmployeeFamilyTemplate
 * @description 对应后端 TaktEmployeeFamilyTemplateDto
 */
export interface EmployeeFamilyTemplate {
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
   * 成员姓名
   */
  memberName?: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType?: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact?: number;

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
 * EmployeeFamily 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeFamilyImport
 * @description 对应后端 TaktEmployeeFamilyImportDto
 */
export interface EmployeeFamilyImport {
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
   * 成员姓名
   */
  memberName?: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType?: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact?: number;

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
 * EmployeeFamily 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeFamilyExport
 * @description 对应后端 TaktEmployeeFamilyExportDto
 */
export interface EmployeeFamilyExport {
  /**
   * EmployeeFamilyID
   */
  employeeFamilyId: string;

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
   * 成员姓名
   */
  memberName: string;

  /**
   * 与员工关系（字典 humanresource_personnel_employee_family_relation_type；0=配偶 1=子女 2=父母 3=兄弟姐妹 9=其他）
   */
  relationType: number;

  /**
   * 联系电话
   */
  phoneNumber?: string;

  /**
   * 工作单位
   */
  workUnit?: string;

  /**
   * 职务
   */
  jobTitle?: string;

  /**
   * 出生日期
   */
  birthDate?: string;

  /**
   * 是否紧急联系人（字典 sys_yes_no；0=否 1=是）
   */
  isEmergencyContact: number;

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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeContract.types.ts
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
 * 员工劳动合同
 * 对应前端 TaktEmployeeContractDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeContract
 * @description 对应后端 TaktEmployeeContractDto
 */
export interface EmployeeContract extends CompanyDtoBase {
  /**
   * EmployeeContractID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeContractId: string;

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
   * 合同编码
   */
  contractCode: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType: number;

  /**
   * 合同开始日期
   */
  startDate: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus: number;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeContract 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeContractQuery
 * @description 对应后端 TaktEmployeeContractQueryDto
 */
export interface EmployeeContractQuery extends TaktPagedQuery {
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
   * 合同编码
   */
  contractCode?: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType?: number;

  /**
   * 合同开始日期（范围查询-开始）
   */
  startDateStart?: string;

  /**
   * 合同开始日期（范围查询-结束）
   */
  startDateEnd?: string;

  /**
   * 合同结束日期（范围查询-开始）
   */
  endDateStart?: string;

  /**
   * 合同结束日期（范围查询-结束）
   */
  endDateEnd?: string;

  /**
   * 试用期结束日期（范围查询-开始）
   */
  probationEndDateStart?: string;

  /**
   * 试用期结束日期（范围查询-结束）
   */
  probationEndDateEnd?: string;

  /**
   * 签订日期（范围查询-开始）
   */
  signDateStart?: string;

  /**
   * 签订日期（范围查询-结束）
   */
  signDateEnd?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus?: number;

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
 * 创建EmployeeContract DTO
 * 对应前端 EmployeeContractCreate
 * @description 对应后端 TaktEmployeeContractCreateDto
 */
export interface EmployeeContractCreate {
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
   * 合同编码
   */
  contractCode: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType: number;

  /**
   * 合同开始日期
   */
  startDate: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus: number;

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
 * 更新EmployeeContract DTO
 * 继承 TaktEmployeeContractCreateDto，添加 EmployeeContractId 字段
 * 对应前端 EmployeeContractUpdate
 * @description 对应后端 TaktEmployeeContractUpdateDto
 */
export interface EmployeeContractUpdate extends EmployeeContractCreate {
  /**
   * EmployeeContractID（标识要更新的实体）
   */
  employeeContractId: string;

}


/**
 * EmployeeContract 状态更新 DTO
 * 对应前端 EmployeeContractStatus
 * @description 对应后端 TaktEmployeeContractStatusDto
 */
export interface EmployeeContractStatus {
  /**
   * EmployeeContractID
   */
  employeeContractId: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus: number;

}


/**
 * EmployeeContract 导入模板行 DTO
 * 对应前端 EmployeeContractTemplate
 * @description 对应后端 TaktEmployeeContractTemplateDto
 */
export interface EmployeeContractTemplate {
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
   * 合同编码
   */
  contractCode?: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType?: number;

  /**
   * 合同开始日期
   */
  startDate?: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus?: number;

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
 * EmployeeContract 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeContractImport
 * @description 对应后端 TaktEmployeeContractImportDto
 */
export interface EmployeeContractImport {
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
   * 合同编码
   */
  contractCode?: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType?: number;

  /**
   * 合同开始日期
   */
  startDate?: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus?: number;

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
 * EmployeeContract 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeContractExport
 * @description 对应后端 TaktEmployeeContractExportDto
 */
export interface EmployeeContractExport {
  /**
   * EmployeeContractID
   */
  employeeContractId: string;

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
   * 合同编码
   */
  contractCode: string;

  /**
   * 合同类型（字典 humanresource_personnel_employee_contract_type；0=固定期限 1=无固定期限 2=以完成一定工作任务为期限 3=实习）
   */
  contractType: number;

  /**
   * 合同开始日期
   */
  startDate: string;

  /**
   * 合同结束日期
   */
  endDate?: string;

  /**
   * 试用期结束日期
   */
  probationEndDate?: string;

  /**
   * 签订日期
   */
  signDate?: string;

  /**
   * 签约单位
   */
  signCompany?: string;

  /**
   * 合同状态（字典 humanresource_personnel_employee_contract_status；0=草稿 1=生效 2=到期 3=终止）
   */
  contractStatus: number;

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


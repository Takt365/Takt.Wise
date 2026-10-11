// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeAddress.types.ts
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
 * 员工地址（主档子表；同一员工每种地址类型至多一条）
 * 对应前端 TaktEmployeeAddressDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeAddress
 * @description 对应后端 TaktEmployeeAddressDto
 */
export interface EmployeeAddress extends CompanyDtoBase {
  /**
   * EmployeeAddressID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeAddressId: string;

  /**
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district: string;

  /**
   * 地址1（详细地址行1）
   */
  address1: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeAddress 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeAddressQuery
 * @description 对应后端 TaktEmployeeAddressQueryDto
 */
export interface EmployeeAddressQuery extends TaktPagedQuery {
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
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country?: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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
 * 创建EmployeeAddress DTO
 * 对应前端 EmployeeAddressCreate
 * @description 对应后端 TaktEmployeeAddressCreateDto
 */
export interface EmployeeAddressCreate {
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
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district: string;

  /**
   * 地址1（详细地址行1）
   */
  address1: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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
 * 更新EmployeeAddress DTO
 * 继承 TaktEmployeeAddressCreateDto，添加 EmployeeAddressId 字段
 * 对应前端 EmployeeAddressUpdate
 * @description 对应后端 TaktEmployeeAddressUpdateDto
 */
export interface EmployeeAddressUpdate extends EmployeeAddressCreate {
  /**
   * EmployeeAddressID（标识要更新的实体）
   */
  employeeAddressId: string;

}


/**
 * EmployeeAddress 导入模板行 DTO
 * 对应前端 EmployeeAddressTemplate
 * @description 对应后端 TaktEmployeeAddressTemplateDto
 */
export interface EmployeeAddressTemplate {
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
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country?: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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
 * EmployeeAddress 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeAddressImport
 * @description 对应后端 TaktEmployeeAddressImportDto
 */
export interface EmployeeAddressImport {
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
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType?: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country?: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province?: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city?: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district?: string;

  /**
   * 地址1（详细地址行1）
   */
  address1?: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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
 * EmployeeAddress 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeAddressExport
 * @description 对应后端 TaktEmployeeAddressExportDto
 */
export interface EmployeeAddressExport {
  /**
   * EmployeeAddressID
   */
  employeeAddressId: string;

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
   * 员工（主子表关系；选项 TaktEmployees/options；DictValue=Id）
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
   * 地址类型（字典 humanresource_personnel_employee_address_type；1=家庭 2=工作 3=常住）
   */
  addressType: number;

  /**
   * 国家（字典 sys_country_code；DictValue=ISO alpha-2）
   */
  country: string;

  /**
   * 省（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=2）
   */
  province: string;

  /**
   * 市（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=3）
   */
  city: string;

  /**
   * 区县（选项 TaktAdminDivisions/options；DictValue=DivisionCode；建议 Level=4）
   */
  district: string;

  /**
   * 地址1（详细地址行1）
   */
  address1: string;

  /**
   * 地址2（详细地址行2）
   */
  address2?: string;

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


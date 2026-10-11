// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeSkill.types.ts
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
 * 员工技能与证书
 * 对应前端 TaktEmployeeSkillDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 EmployeeSkill
 * @description 对应后端 TaktEmployeeSkillDto
 */
export interface EmployeeSkill extends CompanyDtoBase {
  /**
   * EmployeeSkillID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeSkillId: string;

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
   * 技能名称
   */
  skillName: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeSkill 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeSkillQuery
 * @description 对应后端 TaktEmployeeSkillQueryDto
 */
export interface EmployeeSkillQuery extends TaktPagedQuery {
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
   * 技能名称
   */
  skillName?: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel?: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期（范围查询-开始）
   */
  obtainedDateStart?: string;

  /**
   * 取得日期（范围查询-结束）
   */
  obtainedDateEnd?: string;

  /**
   * 到期日期（范围查询-开始）
   */
  expiryDateStart?: string;

  /**
   * 到期日期（范围查询-结束）
   */
  expiryDateEnd?: string;

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
 * 创建EmployeeSkill DTO
 * 对应前端 EmployeeSkillCreate
 * @description 对应后端 TaktEmployeeSkillCreateDto
 */
export interface EmployeeSkillCreate {
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
   * 技能名称
   */
  skillName: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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
 * 更新EmployeeSkill DTO
 * 继承 TaktEmployeeSkillCreateDto，添加 EmployeeSkillId 字段
 * 对应前端 EmployeeSkillUpdate
 * @description 对应后端 TaktEmployeeSkillUpdateDto
 */
export interface EmployeeSkillUpdate extends EmployeeSkillCreate {
  /**
   * EmployeeSkillID（标识要更新的实体）
   */
  employeeSkillId: string;

}


/**
 * EmployeeSkill 导入模板行 DTO
 * 对应前端 EmployeeSkillTemplate
 * @description 对应后端 TaktEmployeeSkillTemplateDto
 */
export interface EmployeeSkillTemplate {
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
   * 技能名称
   */
  skillName?: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel?: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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
 * EmployeeSkill 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeSkillImport
 * @description 对应后端 TaktEmployeeSkillImportDto
 */
export interface EmployeeSkillImport {
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
   * 技能名称
   */
  skillName?: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel?: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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
 * EmployeeSkill 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeSkillExport
 * @description 对应后端 TaktEmployeeSkillExportDto
 */
export interface EmployeeSkillExport {
  /**
   * EmployeeSkillID
   */
  employeeSkillId: string;

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
   * 技能名称
   */
  skillName: string;

  /**
   * 技能等级（字典 humanresource_personnel_employee_skill_level；0=入门 1=熟练 2=精通 3=专家）
   */
  skillLevel: number;

  /**
   * 证书名称
   */
  certificateName?: string;

  /**
   * 证书编码
   */
  certificateCode?: string;

  /**
   * 取得日期
   */
  obtainedDate?: string;

  /**
   * 到期日期
   */
  expiryDate?: string;

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


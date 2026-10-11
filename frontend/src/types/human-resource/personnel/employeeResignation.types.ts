// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeResignation.types.ts
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/personnel 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 员工离职办理记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeResignationDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeResignation
 * @description 对应后端 TaktEmployeeResignationDto
 */
export interface EmployeeResignation extends ApprovalDtoBase {
  /**
   * EmployeeResignationID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeResignationId: string;

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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeResignation 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeResignationQuery
 * @description 对应后端 TaktEmployeeResignationQueryDto
 */
export interface EmployeeResignationQuery extends TaktPagedQuery {
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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType?: number;

  /**
   * 申请日期（范围查询-开始）
   */
  applyDateStart?: string;

  /**
   * 申请日期（范围查询-结束）
   */
  applyDateEnd?: string;

  /**
   * 最后工作日（范围查询-开始）
   */
  lastWorkDateStart?: string;

  /**
   * 最后工作日（范围查询-结束）
   */
  lastWorkDateEnd?: string;

  /**
   * 实际离职日期（范围查询-开始）
   */
  terminationDateStart?: string;

  /**
   * 实际离职日期（范围查询-结束）
   */
  terminationDateEnd?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

  /**
   * 审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）
   */
  approvalStatus?: number;

  /**
   * 发起人ID
   */
  initiatorId?: string;

  /**
   * 发起时间（范围查询-开始）
   */
  initiatedAtStart?: string;

  /**
   * 发起时间（范围查询-结束）
   */
  initiatedAtEnd?: string;

  /**
   * 最终审批人ID
   */
  approvedBy?: string;

  /**
   * 最终审批时间（范围查询-开始）
   */
  approvedAtStart?: string;

  /**
   * 最终审批时间（范围查询-结束）
   */
  approvedAtEnd?: string;

  /**
   * 流程实例 ID
   */
  flowInstanceId?: string;

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
 * 创建EmployeeResignation DTO
 * 对应前端 EmployeeResignationCreate
 * @description 对应后端 TaktEmployeeResignationCreateDto
 */
export interface EmployeeResignationCreate {
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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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
 * 更新EmployeeResignation DTO
 * 继承 TaktEmployeeResignationCreateDto，添加 EmployeeResignationId 字段
 * 对应前端 EmployeeResignationUpdate
 * @description 对应后端 TaktEmployeeResignationUpdateDto
 */
export interface EmployeeResignationUpdate extends EmployeeResignationCreate {
  /**
   * EmployeeResignationID（标识要更新的实体）
   */
  employeeResignationId: string;

}


/**
 * EmployeeResignation 导入模板行 DTO
 * 对应前端 EmployeeResignationTemplate
 * @description 对应后端 TaktEmployeeResignationTemplateDto
 */
export interface EmployeeResignationTemplate {
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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType?: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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
 * EmployeeResignation 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeResignationImport
 * @description 对应后端 TaktEmployeeResignationImportDto
 */
export interface EmployeeResignationImport {
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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType?: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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
 * EmployeeResignation 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeResignationExport
 * @description 对应后端 TaktEmployeeResignationExportDto
 */
export interface EmployeeResignationExport {
  /**
   * EmployeeResignationID
   */
  employeeResignationId: string;

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
   * 离职类型（字典 humanresource_personnel_resignation_category；0=主动辞职 1=公司辞退 2=合同到期 3=退休 9=其他）
   */
  resignationType: number;

  /**
   * 申请日期
   */
  applyDate?: string;

  /**
   * 最后工作日
   */
  lastWorkDate?: string;

  /**
   * 实际离职日期
   */
  terminationDate?: string;

  /**
   * 离职原因
   */
  reason?: string;

  /**
   * 工作交接说明
   */
  handoverNotes?: string;

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


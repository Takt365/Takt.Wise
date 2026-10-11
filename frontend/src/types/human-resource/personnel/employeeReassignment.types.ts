// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/personnel
// 文件名称：employeeReassignment.types.ts
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
 * 员工调动记录（审批单；审批态见基类 ApprovalStatus，字典 sys_approval_status）
 * 对应前端 TaktEmployeeReassignmentDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 EmployeeReassignment
 * @description 对应后端 TaktEmployeeReassignmentDto
 */
export interface EmployeeReassignment extends ApprovalDtoBase {
  /**
   * EmployeeReassignmentID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
   */
  employeeReassignmentId: string;

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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

  /**
   * 员工主档（多对一） （主表：TaktEmployee）
   */
  employee?: Employee;

}


/**
 * EmployeeReassignment 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 EmployeeReassignmentQuery
 * @description 对应后端 TaktEmployeeReassignmentQueryDto
 */
export interface EmployeeReassignmentQuery extends TaktPagedQuery {
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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期（范围查询-开始）
   */
  effectiveDateStart?: string;

  /**
   * 生效日期（范围查询-结束）
   */
  effectiveDateEnd?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * 创建EmployeeReassignment DTO
 * 对应前端 EmployeeReassignmentCreate
 * @description 对应后端 TaktEmployeeReassignmentCreateDto
 */
export interface EmployeeReassignmentCreate {
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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * 更新EmployeeReassignment DTO
 * 继承 TaktEmployeeReassignmentCreateDto，添加 EmployeeReassignmentId 字段
 * 对应前端 EmployeeReassignmentUpdate
 * @description 对应后端 TaktEmployeeReassignmentUpdateDto
 */
export interface EmployeeReassignmentUpdate extends EmployeeReassignmentCreate {
  /**
   * EmployeeReassignmentID（标识要更新的实体）
   */
  employeeReassignmentId: string;

}


/**
 * EmployeeReassignment 导入模板行 DTO
 * 对应前端 EmployeeReassignmentTemplate
 * @description 对应后端 TaktEmployeeReassignmentTemplateDto
 */
export interface EmployeeReassignmentTemplate {
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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * EmployeeReassignment 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 EmployeeReassignmentImport
 * @description 对应后端 TaktEmployeeReassignmentImportDto
 */
export interface EmployeeReassignmentImport {
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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType?: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId?: string;

  /**
   * 调出部门名称
   */
  fromDeptName?: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId?: string;

  /**
   * 调入部门名称
   */
  toDeptName?: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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
 * EmployeeReassignment 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 EmployeeReassignmentExport
 * @description 对应后端 TaktEmployeeReassignmentExportDto
 */
export interface EmployeeReassignmentExport {
  /**
   * EmployeeReassignmentID
   */
  employeeReassignmentId: string;

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
   * 调动类型（字典 humanresource_personnel_reassignment_type；0=转岗 1=调岗）
   */
  reassignmentType: number;

  /**
   * 调出部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  fromDeptId: string;

  /**
   * 调出部门名称
   */
  fromDeptName: string;

  /**
   * 调出岗位（选项 TaktPosts/options；DictValue=Id）
   */
  fromPostId?: string;

  /**
   * 调出岗位名称
   */
  fromPostName?: string;

  /**
   * 调入部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  toDeptId: string;

  /**
   * 调入部门名称
   */
  toDeptName: string;

  /**
   * 调入岗位（选项 TaktPosts/options；DictValue=Id）
   */
  toPostId?: string;

  /**
   * 调入岗位名称
   */
  toPostName?: string;

  /**
   * 生效日期
   */
  effectiveDate?: string;

  /**
   * 调动原因
   */
  reason?: string;

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


// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：leave.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  ApprovalDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 请假实体。FlowInstanceId 由业务在发起流程后写入；流程引擎通过 BusinessKey/BusinessType 与请假模块对接。
 * 对应前端 TaktLeaveDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Leave
 * @description 对应后端 TaktLeaveDto
 */
export interface Leave extends ApprovalDtoBase {

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy: string;

  /**
   * 经办时间
   */
  handlingAt?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus: number;

}


/**
 * Leave 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 LeaveQuery
 * @description 对应后端 TaktLeaveQueryDto
 */
export interface LeaveQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 请假类型（字典 sys_leave_type；列存 DictValue）
   */
  leaveType?: string;

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
   * 请假事由
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy?: string;

  /**
   * 经办时间（范围查询-开始）
   */
  handlingAtStart?: string;

  /**
   * 经办时间（范围查询-结束）
   */
  handlingAtEnd?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus?: number;

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
 * 创建Leave DTO
 * 对应前端 LeaveCreate
 * @description 对应后端 TaktLeaveCreateDto
 */
export interface LeaveCreate {
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
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 请假类型（字典 sys_leave_type；列存 DictValue）
   */
  leaveType: string;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 请假事由
   */
  reason: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy: string;

  /**
   * 经办时间
   */
  handlingAt?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus: number;

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
 * 更新Leave DTO
 * 继承 TaktLeaveCreateDto，添加 LeaveId 字段
 * 对应前端 LeaveUpdate
 * @description 对应后端 TaktLeaveUpdateDto
 */
export interface LeaveUpdate extends LeaveCreate {
  /**
   * LeaveID（标识要更新的实体）
   */
  leaveId: string;

}


/**
 * Leave 状态更新 DTO
 * 对应前端 LeaveStatus
 * @description 对应后端 TaktLeaveStatusDto
 */
export interface LeaveStatus {
  /**
   * LeaveID
   */
  leaveId: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus: number;

}


/**
 * Leave 导入模板行 DTO
 * 对应前端 LeaveTemplate
 * @description 对应后端 TaktLeaveTemplateDto
 */
export interface LeaveTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 请假类型（字典 sys_leave_type；列存 DictValue）
   */
  leaveType?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 请假事由
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy?: string;

  /**
   * 经办时间
   */
  handlingAt?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus?: number;

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
 * Leave 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 LeaveImport
 * @description 对应后端 TaktLeaveImportDto
 */
export interface LeaveImport {
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
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId?: string;

  /**
   * 员工姓名
   */
  employeeName?: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 请假类型（字典 sys_leave_type；列存 DictValue）
   */
  leaveType?: string;

  /**
   * 开始日期
   */
  startDate?: string;

  /**
   * 结束日期
   */
  endDate?: string;

  /**
   * 请假事由
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy?: string;

  /**
   * 经办时间
   */
  handlingAt?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus?: number;

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
 * Leave 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 LeaveExport
 * @description 对应后端 TaktLeaveExportDto
 */
export interface LeaveExport {
  /**
   * LeaveID
   */
  leaveId: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  employeeId: string;

  /**
   * 员工姓名
   */
  employeeName: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 请假类型（字典 sys_leave_type；列存 DictValue）
   */
  leaveType: string;

  /**
   * 开始日期
   */
  startDate: string;

  /**
   * 结束日期
   */
  endDate: string;

  /**
   * 请假事由
   */
  reason: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

  /**
   * 证明附件 JSON（列表形式，由TaktFile 统一上传到服务器）
   */
  Attachments?: string;

  /**
   * 经办人（关联 TaktEmployee.Id，选项 TaktEmployees/options）
   */
  handlingBy: string;

  /**
   * 经办时间
   */
  handlingAt?: string;

  /**
   * 经办备注
   */
  handlingComment?: string;

  /**
   * 请假状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  leaveStatus: number;

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


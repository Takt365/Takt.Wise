// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：overtime.types.ts
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
 * 加班申请（时长与状态由业务维护，可与工作流扩展对接）
 * 对应前端 TaktOvertimeDto
 * 继承 TaktApprovalDtoBase
 * 对应前端 Overtime
 * @description 对应后端 TaktOvertimeDto
 */
export interface Overtime extends ApprovalDtoBase {

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus: number;

  /**
   * 加班明细列表 （子表：TaktOvertimeItem）
   */
  items?: OvertimeItem[];

}


/**
 * Overtime 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 OvertimeQuery
 * @description 对应后端 TaktOvertimeQueryDto
 */
export interface OvertimeQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 加班归属日期（范围查询-开始）
   */
  overtimeDateStart?: string;

  /**
   * 加班归属日期（范围查询-结束）
   */
  overtimeDateEnd?: string;

  /**
   * 计划加班开始时间（范围查询-开始）
   */
  plannedStartTimeStart?: string;

  /**
   * 计划加班开始时间（范围查询-结束）
   */
  plannedStartTimeEnd?: string;

  /**
   * 计划加班结束时间（范围查询-开始）
   */
  plannedEndTimeStart?: string;

  /**
   * 计划加班结束时间（范围查询-结束）
   */
  plannedEndTimeEnd?: string;

  /**
   * 加班总人数
   */
  totalEmployees?: number;

  /**
   * 计划加班总小时数
   */
  totalPlannedHours?: number;

  /**
   * 实际加班总小时数
   */
  totalActualHours?: number;

  /**
   * 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
   */
  overtimeType?: number;

  /**
   * 加班原因
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus?: number;

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
 * 创建Overtime DTO
 * 对应前端 OvertimeCreate
 * @description 对应后端 TaktOvertimeCreateDto
 */
export interface OvertimeCreate {
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
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 加班归属日期
   */
  overtimeDate: string;

  /**
   * 计划加班开始时间
   */
  plannedStartTime: string;

  /**
   * 计划加班结束时间
   */
  plannedEndTime: string;

  /**
   * 加班总人数
   */
  totalEmployees: number;

  /**
   * 计划加班总小时数
   */
  totalPlannedHours: number;

  /**
   * 实际加班总小时数
   */
  totalActualHours: number;

  /**
   * 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
   */
  overtimeType: number;

  /**
   * 加班原因
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus: number;

  /**
   * 加班明细列表（子表，级联保存）
   */
  items?: OvertimeItemCreate[];

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
 * 更新Overtime DTO
 * 继承 TaktOvertimeCreateDto，添加 OvertimeId 字段
 * 对应前端 OvertimeUpdate
 * @description 对应后端 TaktOvertimeUpdateDto
 */
export interface OvertimeUpdate extends OvertimeCreate {
  /**
   * OvertimeID（标识要更新的实体）
   */
  overtimeId: string;

}


/**
 * Overtime 状态更新 DTO
 * 对应前端 OvertimeStatus
 * @description 对应后端 TaktOvertimeStatusDto
 */
export interface OvertimeStatus {
  /**
   * OvertimeID
   */
  overtimeId: string;

  /**
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus: number;

}


/**
 * Overtime 导入模板行 DTO
 * 对应前端 OvertimeTemplate
 * @description 对应后端 TaktOvertimeTemplateDto
 */
export interface OvertimeTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 加班归属日期
   */
  overtimeDate?: string;

  /**
   * 计划加班开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划加班结束时间
   */
  plannedEndTime?: string;

  /**
   * 加班总人数
   */
  totalEmployees?: number;

  /**
   * 计划加班总小时数
   */
  totalPlannedHours?: number;

  /**
   * 实际加班总小时数
   */
  totalActualHours?: number;

  /**
   * 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
   */
  overtimeType?: number;

  /**
   * 加班原因
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus?: number;

  /**
   * 加班明细列表（子表，级联保存）
   */
  items?: OvertimeItemCreate[];

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
 * Overtime 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 OvertimeImport
 * @description 对应后端 TaktOvertimeImportDto
 */
export interface OvertimeImport {
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
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId?: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 加班归属日期
   */
  overtimeDate?: string;

  /**
   * 计划加班开始时间
   */
  plannedStartTime?: string;

  /**
   * 计划加班结束时间
   */
  plannedEndTime?: string;

  /**
   * 加班总人数
   */
  totalEmployees?: number;

  /**
   * 计划加班总小时数
   */
  totalPlannedHours?: number;

  /**
   * 实际加班总小时数
   */
  totalActualHours?: number;

  /**
   * 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
   */
  overtimeType?: number;

  /**
   * 加班原因
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus?: number;

  /**
   * 加班明细列表（子表，级联保存）
   */
  items?: OvertimeItemCreate[];

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
 * Overtime 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 OvertimeExport
 * @description 对应后端 TaktOvertimeExportDto
 */
export interface OvertimeExport {
  /**
   * OvertimeID
   */
  overtimeId: string;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options）
   */
  deptId: string;

  /**
   * 部门名称
   */
  deptName?: string;

  /**
   * 加班归属日期
   */
  overtimeDate: string;

  /**
   * 计划加班开始时间
   */
  plannedStartTime: string;

  /**
   * 计划加班结束时间
   */
  plannedEndTime: string;

  /**
   * 加班总人数
   */
  totalEmployees: number;

  /**
   * 计划加班总小时数
   */
  totalPlannedHours: number;

  /**
   * 实际加班总小时数
   */
  totalActualHours: number;

  /**
   * 加班类型（字典 humanresource_attendance_overtime_type；0=工作日加班 1=休息日加班 2=法定节假日加班）
   */
  overtimeType: number;

  /**
   * 加班原因
   */
  reason?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
   * 加班状态（字典 sys_approval_status；0=待审批 1=审批中 2=已通过 3=已驳回 4=已撤回 5=已终止）
   */
  overtimeStatus: number;

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


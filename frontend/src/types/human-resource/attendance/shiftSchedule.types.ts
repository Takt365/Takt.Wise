// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：shiftSchedule.types.ts
// 创建时间：2026-06-23
// 创建人：Takt365(Auto Generated)
// 功能描述：human-resource/attendance 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type {
  CompanyDtoBase,
  TaktPagedQuery
} from '@/types/common.types';

/**
 * 排班计划（ScheduleType=0 部门排班时 DeptId 必填；ScheduleType=1 人员排班时 EmployeeId 必填）
 * 对应前端 TaktShiftScheduleDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 ShiftSchedule
 * @description 对应后端 TaktShiftScheduleDto
 */
export interface ShiftSchedule extends CompanyDtoBase {

}


/**
 * ShiftSchedule 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 ShiftScheduleQuery
 * @description 对应后端 TaktShiftScheduleQueryDto
 */
export interface ShiftScheduleQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
   */
  scheduleType?: number;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
   */
  deptId?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
   */
  employeeId?: string;

  /**
   * 排班日期（范围查询-开始）
   */
  scheduleDateStart?: string;

  /**
   * 排班日期（范围查询-结束）
   */
  scheduleDateEnd?: string;

  /**
   * 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
   */
  shiftId?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * 创建ShiftSchedule DTO
 * 对应前端 ShiftScheduleCreate
 * @description 对应后端 TaktShiftScheduleCreateDto
 */
export interface ShiftScheduleCreate {
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
   * 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
   */
  scheduleType: number;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
   */
  deptId?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
   */
  employeeId?: string;

  /**
   * 排班日期
   */
  scheduleDate: string;

  /**
   * 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
   */
  shiftId: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * 更新ShiftSchedule DTO
 * 继承 TaktShiftScheduleCreateDto，添加 ShiftScheduleId 字段
 * 对应前端 ShiftScheduleUpdate
 * @description 对应后端 TaktShiftScheduleUpdateDto
 */
export interface ShiftScheduleUpdate extends ShiftScheduleCreate {
  /**
   * ShiftScheduleID（标识要更新的实体）
   */
  shiftScheduleId: string;

}


/**
 * ShiftSchedule 导入模板行 DTO
 * 对应前端 ShiftScheduleTemplate
 * @description 对应后端 TaktShiftScheduleTemplateDto
 */
export interface ShiftScheduleTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
   */
  scheduleType?: number;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
   */
  deptId?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
   */
  employeeId?: string;

  /**
   * 排班日期
   */
  scheduleDate?: string;

  /**
   * 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
   */
  shiftId?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * ShiftSchedule 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 ShiftScheduleImport
 * @description 对应后端 TaktShiftScheduleImportDto
 */
export interface ShiftScheduleImport {
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
   * 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
   */
  scheduleType?: number;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
   */
  deptId?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
   */
  employeeId?: string;

  /**
   * 排班日期
   */
  scheduleDate?: string;

  /**
   * 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
   */
  shiftId?: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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
 * ShiftSchedule 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 ShiftScheduleExport
 * @description 对应后端 TaktShiftScheduleExportDto
 */
export interface ShiftScheduleExport {
  /**
   * ShiftScheduleID
   */
  shiftScheduleId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 排班类别（字典 humanresource_attendance_schedule_type；0=部门 1=人员）
   */
  scheduleType: number;

  /**
   * 部门（关联 TaktDept.Id，选项 TaktDepts/tree-options；ScheduleType=0 时必填）
   */
  deptId?: string;

  /**
   * 员工（关联 TaktEmployee.Id，选项 TaktEmployees/options；ScheduleType=1 时必填）
   */
  employeeId?: string;

  /**
   * 排班日期
   */
  scheduleDate: string;

  /**
   * 班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
   */
  shiftId: string;

  /**
   * 关联工厂（关联 TaktPlant.PlantCode，选项 TaktPlants/options）
   */
  plantCode?: string;

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


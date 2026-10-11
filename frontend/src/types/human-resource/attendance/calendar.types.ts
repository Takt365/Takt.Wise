// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：calendar.types.ts
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
 * 工厂日历（公司级；按 PlantCode 区分工厂维度）
 * 对应前端 TaktCalendarDto
 * 继承 TaktCompanyDtoBase
 * 对应前端 Calendar
 * @description 对应后端 TaktCalendarDto
 */
export interface Calendar extends CompanyDtoBase {

}


/**
 * Calendar 分页查询 DTO
 * 继承 TaktPagedQuery
 * 对应前端 CalendarQuery
 * @description 对应后端 TaktCalendarQueryDto
 */
export interface CalendarQuery extends TaktPagedQuery {
  /**
   * 租户编码
   */
  tenantCode?: string;

  /**
   * 公司代码
   */
  companyCode?: string;

  /**
   * 日历日期（范围查询-开始）
   */
  calendarDateStart?: string;

  /**
   * 日历日期（范围查询-结束）
   */
  calendarDateEnd?: string;

  /**
   * 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
   */
  isWorkingDay?: number;

  /**
   * 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
   */
  holidayId?: string;

  /**
   * 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
 * 创建Calendar DTO
 * 对应前端 CalendarCreate
 * @description 对应后端 TaktCalendarCreateDto
 */
export interface CalendarCreate {
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
   * 日历日期
   */
  calendarDate: string;

  /**
   * 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
   */
  isWorkingDay: number;

  /**
   * 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
   */
  holidayId?: string;

  /**
   * 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
 * 更新Calendar DTO
 * 继承 TaktCalendarCreateDto，添加 CalendarId 字段
 * 对应前端 CalendarUpdate
 * @description 对应后端 TaktCalendarUpdateDto
 */
export interface CalendarUpdate extends CalendarCreate {
  /**
   * CalendarID（标识要更新的实体）
   */
  calendarId: string;

}


/**
 * Calendar 导入模板行 DTO
 * 对应前端 CalendarTemplate
 * @description 对应后端 TaktCalendarTemplateDto
 */
export interface CalendarTemplate {
  /**
   * 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
   */
  tenantCode?: string;

  /**
   * 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
   */
  companyCode?: string;

  /**
   * 日历日期
   */
  calendarDate?: string;

  /**
   * 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
   */
  isWorkingDay?: number;

  /**
   * 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
   */
  holidayId?: string;

  /**
   * 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
 * Calendar 导入 DTO（独立实现，不继承 TemplateDto）
 * 对应前端 CalendarImport
 * @description 对应后端 TaktCalendarImportDto
 */
export interface CalendarImport {
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
   * 日历日期
   */
  calendarDate?: string;

  /**
   * 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
   */
  isWorkingDay?: number;

  /**
   * 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
   */
  holidayId?: string;

  /**
   * 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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
 * Calendar 导出 DTO（独立实现，不继承响应 Dto）
 * 对应前端 CalendarExport
 * @description 对应后端 TaktCalendarExportDto
 */
export interface CalendarExport {
  /**
   * CalendarID
   */
  calendarId: string;

  /**
   * 公司代码
   */
  companyCode: string;

  /**
   * 日历日期
   */
  calendarDate: string;

  /**
   * 是否工作日（字典 humanresource_attendance_holiday_working_day_type；0=非工作日 1=工作日 2=半天等）
   */
  isWorkingDay: number;

  /**
   * 关联假日（关联 TaktHoliday.Id，选项 TaktHolidays/options）
   */
  holidayId?: string;

  /**
   * 关联班次（关联 TaktWorkShift.Id，选项 TaktWorkShifts/options）
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

  /**
   * 创建时间
   */
  createdAt: string;

}


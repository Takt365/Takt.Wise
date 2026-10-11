// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/human-resource/attendance
// 文件名称：holiday.types.ts
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
 * 假日实体
 * @description 对应后端 TaktHolidayDto
 */
export interface Holiday extends CompanyDtoBase {
  /**
   * 区域文化编码
   */
  cultureCode?: string

  /**
   * 假日名称
   */
  holidayName?: string

  /**
   * 假日类型（字典 humanresource_attendance_holiday_category；0=法定 1=调休 2=公司）
   */
  holidayType?: number

  /**
   * 假日开始日期
   */
  startDate?: string

  /**
   * 假日结束日期
   */
  endDate?: string

  /**
   * 假期天数（含起止日）
   */
  daysCount?: number

  /**
   * 调休对应（上班日=>所补放假日；多对分号分隔）
   */
  compensatoryWorkDates?: string

  /**
   * 是否带薪假（字典 sys_yes_no；0=否 1=是）
   */
  isPaid?: number

  /**
   * 假日问候语
   */
  holidayGreeting?: string

  /**
   * 假日引用/诗句
   */
  holidayQuote?: string

  /**
   * 假日主题
   */
  holidayTheme?: string

  /**
   * 扩展字段JSON
   */
  extField?: string

  /**
   * 备注
   */
  remark?: string
}

/**
 * Holiday 分页查询
 * @description 对应后端 TaktHolidayQueryDto
 */
export interface HolidayQuery extends TaktPagedQuery {
  tenantCode?: string
  companyCode?: string
  cultureCode?: string
  plantCode?: string
  holidayName?: string
  holidayType?: number
  startDateStart?: string
  startDateEnd?: string
  endDateStart?: string
  endDateEnd?: string
  daysCount?: number
  compensatoryWorkDates?: string
  isPaid?: number
  holidayGreeting?: string
  holidayQuote?: string
  holidayTheme?: string
  createdAtStart?: string
  createdAtEnd?: string
  extField?: string
  remark?: string
}

/**
 * Holiday 创建 DTO
 * @description 对应后端 TaktHolidayCreateDto
 */
export interface HolidayCreate {
  tenantCode?: string
  companyCode?: string
  cultureCode?: string
  plantCode?: string
  holidayName: string
  holidayType: number
  startDate: string
  endDate: string
  daysCount?: number
  compensatoryWorkDates?: string
  isPaid?: number
  holidayGreeting: string
  holidayQuote: string
  holidayTheme: string
  extField?: string
  remark?: string
}

/**
 * Holiday 更新 DTO
 * @description 对应后端 TaktHolidayUpdateDto
 */
export interface HolidayUpdate extends HolidayCreate {
  holidayId: string
}

/**
 * Holiday 导出 DTO
 * @description 对应后端 TaktHolidayExportDto
 */
export interface HolidayExport {
  holidayId: string
  companyCode: string
  holidayName: string
  holidayType: number
  startDate: string
  endDate: string
  daysCount: number
  compensatoryWorkDates: string
  isPaid: number
  holidayGreeting: string
  holidayQuote: string
  holidayTheme: string
  extField?: string
  remark?: string
  createdAt: string
}

/**
 * 服务器当日假日主题响应
 * @description 对应后端 TaktHolidayThemeDto
 */
export interface HolidayTheme {
  companyCode: string
  holidayName: string
  holidayType: number
  startDate: string
  endDate: string
  daysCount: number
  compensatoryWorkDates: string
  isPaid: number
  holidayGreeting: string
  holidayQuote: string
  holidayTheme: string
  /**
   * 服务器当日是否处于假日放假区间（无匹配记录时为 false）
   */
  isHolidayToday: boolean
}

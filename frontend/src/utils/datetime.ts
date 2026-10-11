// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/datetime
// 文件名称：datetime.ts
// 创建时间：2025-01-21
// 创建人：Takt365(Cursor AI)
// 功能描述：日期时间格式化工具类，提供各种日期时间格式化方法
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 日期时间格式化工具类
 */
export class DateTimeHelper {
  /**
   * 格式化日期时间
   * @param date 日期时间（Date对象、时间戳或日期字符串）
   * @param format 格式化模板（默认：'YYYY-MM-DD HH:mm:ss'）
   * @returns 格式化后的日期时间字符串
   * 
   * @example
   * format(new Date(), 'YYYY-MM-DD HH:mm:ss') // '2025-01-21 12:30:45'
   * format(new Date(), 'YYYY年MM月DD日') // '2025年01月21日'
   * format(1642752000000, 'YYYY-MM-DD') // '2022-01-21'
   */
  static format(date: Date | number | string | null | undefined, format: string = 'YYYY-MM-DD HH:mm:ss'): string {
    if (!date) {
      return ''
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return ''
    }

    const year = dateObj.getFullYear()
    const month = dateObj.getMonth() + 1
    const day = dateObj.getDate()
    const hour = dateObj.getHours()
    const minute = dateObj.getMinutes()
    const second = dateObj.getSeconds()
    const millisecond = dateObj.getMilliseconds()

    const formatMap: Record<string, string> = {
      YYYY: String(year),
      YY: String(year).slice(-2),
      MM: String(month).padStart(2, '0'),
      M: String(month),
      DD: String(day).padStart(2, '0'),
      D: String(day),
      HH: String(hour).padStart(2, '0'),
      H: String(hour),
      mm: String(minute).padStart(2, '0'),
      m: String(minute),
      ss: String(second).padStart(2, '0'),
      s: String(second),
      SSS: String(millisecond).padStart(3, '0'),
      S: String(millisecond)
    }

    return format.replace(/YYYY|YY|MM|M|DD|D|HH|H|mm|m|ss|s|SSS|S/g, (match) => formatMap[match] || match)
  }

  /**
   * 转换为Date对象
   * @param date 日期时间（Date对象、时间戳或日期字符串）
   * @returns Date对象
   */
  static toDate(date: Date | number | string): Date | null {
    if (date instanceof Date) {
      return date
    }

    if (typeof date === 'number') {
      // 时间戳（毫秒）
      if (date > 10000000000) {
        return new Date(date)
      }
      // 时间戳（秒）
      return new Date(date * 1000)
    }

    if (typeof date === 'string') {
      // 尝试解析日期字符串
      const parsed = new Date(date)
      if (!isNaN(parsed.getTime())) {
        return parsed
      }
    }

    return null
  }

  /**
   * 格式化日期（YYYY-MM-DD）
   * @param date 日期时间
   * @returns 格式化后的日期字符串
   */
  static formatDate(date: Date | number | string | null | undefined): string {
    return this.format(date, 'YYYY-MM-DD')
  }

  /**
   * 格式化时间（HH:mm:ss）
   * @param date 日期时间
   * @returns 格式化后的时间字符串
   */
  static formatTime(date: Date | number | string | null | undefined): string {
    return this.format(date, 'HH:mm:ss')
  }

  /**
   * 格式化日期时间（YYYY-MM-DD HH:mm:ss）
   * @param date 日期时间
   * @returns 格式化后的日期时间字符串
   */
  static formatDateTime(date: Date | number | string | null | undefined): string {
    return this.format(date, 'YYYY-MM-DD HH:mm:ss')
  }

  /**
   * 格式化日期时间（YYYY-MM-DD HH:mm）
   * @param date 日期时间
   * @returns 格式化后的日期时间字符串
   */
  static formatDateTimeShort(date: Date | number | string | null | undefined): string {
    return this.format(date, 'YYYY-MM-DD HH:mm')
  }

  /**
   * 格式化日期（中文格式：YYYY年MM月DD日）
   * @param date 日期时间
   * @returns 格式化后的日期字符串
   */
  static formatDateCN(date: Date | number | string | null | undefined): string {
    return this.format(date, 'YYYY年MM月DD日')
  }

  /**
   * 格式化日期时间（中文格式：YYYY年MM月DD日 HH时mm分ss秒）
   * @param date 日期时间
   * @returns 格式化后的日期时间字符串
   */
  static formatDateTimeCN(date: Date | number | string | null | undefined): string {
    return this.format(date, 'YYYY年MM月DD日 HH时mm分ss秒')
  }

  /**
   * 格式化相对时间（如：刚刚、1分钟前、2小时前、3天前）
   * @param date 日期时间
   * @returns 相对时间字符串
   */
  static formatRelative(date: Date | number | string | null | undefined): string {
    if (!date) {
      return ''
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return ''
    }

    const now = new Date()
    const diff = now.getTime() - dateObj.getTime()
    const seconds = Math.floor(diff / 1000)
    const minutes = Math.floor(seconds / 60)
    const hours = Math.floor(minutes / 60)
    const days = Math.floor(hours / 24)
    const months = Math.floor(days / 30)
    const years = Math.floor(days / 365)

    if (seconds < 0) {
      return '未来'
    }

    if (seconds < 60) {
      return '刚刚'
    }

    if (minutes < 60) {
      return `${minutes}分钟前`
    }

    if (hours < 24) {
      return `${hours}小时前`
    }

    if (days < 30) {
      return `${days}天前`
    }

    if (months < 12) {
      return `${months}个月前`
    }

    return `${years}年前`
  }

  /**
   * 格式化友好时间（智能选择格式）
   * @param date 日期时间
   * @param showTime 是否显示时间（默认：true）
   * @returns 友好时间字符串
   */
  static formatFriendly(date: Date | number | string | null | undefined, showTime: boolean = true): string {
    if (!date) {
      return ''
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return ''
    }

    const now = new Date()
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate())
    const yesterday = new Date(today.getTime() - 24 * 60 * 60 * 1000)
    const dateOnly = new Date(dateObj.getFullYear(), dateObj.getMonth(), dateObj.getDate())

    if (dateOnly.getTime() === today.getTime()) {
      // 今天
      return showTime ? `今天 ${this.formatTime(dateObj)}` : '今天'
    }

    if (dateOnly.getTime() === yesterday.getTime()) {
      // 昨天
      return showTime ? `昨天 ${this.formatTime(dateObj)}` : '昨天'
    }

    const diffDays = Math.floor((now.getTime() - dateObj.getTime()) / (24 * 60 * 60 * 1000))
    if (diffDays < 7) {
      // 一周内
      const weekdays = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六']
      const weekday = weekdays[dateObj.getDay()]
      return showTime ? `${weekday} ${this.formatTime(dateObj)}` : weekday
    }

    // 超过一周，显示完整日期
    return showTime ? this.formatDateTime(dateObj) : this.formatDate(dateObj)
  }

  /**
   * 获取时间戳（毫秒）
   * @param date 日期时间（可选，默认当前时间）
   * @returns 时间戳（毫秒）
   */
  static getTimestamp(date?: Date | number | string): number {
    if (date) {
      const dateObj = this.toDate(date)
      return dateObj ? dateObj.getTime() : Date.now()
    }
    return Date.now()
  }

  /**
   * 获取时间戳（秒）
   * @param date 日期时间（可选，默认当前时间）
   * @returns 时间戳（秒）
   */
  static getTimestampSeconds(date?: Date | number | string): number {
    return Math.floor(this.getTimestamp(date) / 1000)
  }

  /**
   * 判断是否为今天
   * @param date 日期时间
   * @returns 是否为今天
   */
  static isToday(date: Date | number | string | null | undefined): boolean {
    if (!date) {
      return false
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return false
    }

    const now = new Date()
    return (
      dateObj.getFullYear() === now.getFullYear() &&
      dateObj.getMonth() === now.getMonth() &&
      dateObj.getDate() === now.getDate()
    )
  }

  /**
   * 判断是否为昨天
   * @param date 日期时间
   * @returns 是否为昨天
   */
  static isYesterday(date: Date | number | string | null | undefined): boolean {
    if (!date) {
      return false
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return false
    }

    const yesterday = new Date()
    yesterday.setDate(yesterday.getDate() - 1)

    return (
      dateObj.getFullYear() === yesterday.getFullYear() &&
      dateObj.getMonth() === yesterday.getMonth() &&
      dateObj.getDate() === yesterday.getDate()
    )
  }

  /**
   * 判断是否为本周
   * @param date 日期时间
   * @returns 是否为本周
   */
  static isThisWeek(date: Date | number | string | null | undefined): boolean {
    if (!date) {
      return false
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return false
    }

    const now = new Date()
    const weekStart = new Date(now.setDate(now.getDate() - now.getDay()))
    weekStart.setHours(0, 0, 0, 0)

    const weekEnd = new Date(weekStart)
    weekEnd.setDate(weekEnd.getDate() + 6)
    weekEnd.setHours(23, 59, 59, 999)

    return dateObj >= weekStart && dateObj <= weekEnd
  }

  /**
   * 判断是否为今年
   * @param date 日期时间
   * @returns 是否为今年
   */
  static isThisYear(date: Date | number | string | null | undefined): boolean {
    if (!date) {
      return false
    }

    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return false
    }

    const now = new Date()
    return dateObj.getFullYear() === now.getFullYear()
  }

  /**
   * 计算两个日期之间的差值
   * @param date1 第一个日期
   * @param date2 第二个日期（可选，默认当前时间）
   * @returns 差值对象（包含年、月、日、时、分、秒、毫秒）
   */
  static diff(
    date1: Date | number | string,
    date2?: Date | number | string
  ): {
    years: number
    months: number
    days: number
    hours: number
    minutes: number
    seconds: number
    milliseconds: number
  } {
    const dateObj1 = this.toDate(date1)
    const dateObj2 = this.toDate(date2 || new Date())

    if (!dateObj1 || !dateObj2) {
      return {
        years: 0,
        months: 0,
        days: 0,
        hours: 0,
        minutes: 0,
        seconds: 0,
        milliseconds: 0
      }
    }

    const diff = Math.abs(dateObj2.getTime() - dateObj1.getTime())

    return {
      years: Math.floor(diff / (365 * 24 * 60 * 60 * 1000)),
      months: Math.floor(diff / (30 * 24 * 60 * 60 * 1000)),
      days: Math.floor(diff / (24 * 60 * 60 * 1000)),
      hours: Math.floor(diff / (60 * 60 * 1000)),
      minutes: Math.floor(diff / (60 * 1000)),
      seconds: Math.floor(diff / 1000),
      milliseconds: diff
    }
  }

  /**
   * 添加时间
   * @param date 日期时间
   * @param amount 数量
   * @param unit 单位（'year' | 'month' | 'day' | 'hour' | 'minute' | 'second'）
   * @returns 新的Date对象
   */
  static add(
    date: Date | number | string,
    amount: number,
    unit: 'year' | 'month' | 'day' | 'hour' | 'minute' | 'second'
  ): Date | null {
    const dateObj = this.toDate(date)
    if (!dateObj || isNaN(dateObj.getTime())) {
      return null
    }

    const newDate = new Date(dateObj)

    switch (unit) {
      case 'year':
        newDate.setFullYear(newDate.getFullYear() + amount)
        break
      case 'month':
        newDate.setMonth(newDate.getMonth() + amount)
        break
      case 'day':
        newDate.setDate(newDate.getDate() + amount)
        break
      case 'hour':
        newDate.setHours(newDate.getHours() + amount)
        break
      case 'minute':
        newDate.setMinutes(newDate.getMinutes() + amount)
        break
      case 'second':
        newDate.setSeconds(newDate.getSeconds() + amount)
        break
    }

    return newDate
  }

  /**
   * 获取开始时间（当天的00:00:00）
   * @param date 日期时间（可选，默认当前时间）
   * @returns 开始时间的Date对象
   */
  static startOfDay(date?: Date | number | string): Date | null {
    const dateObj = this.toDate(date || new Date())
    if (!dateObj || isNaN(dateObj.getTime())) {
      return null
    }

    const newDate = new Date(dateObj)
    newDate.setHours(0, 0, 0, 0)
    return newDate
  }

  /**
   * 获取结束时间（当天的23:59:59.999）
   * @param date 日期时间（可选，默认当前时间）
   * @returns 结束时间的Date对象
   */
  static endOfDay(date?: Date | number | string): Date | null {
    const dateObj = this.toDate(date || new Date())
    if (!dateObj || isNaN(dateObj.getTime())) {
      return null
    }

    const newDate = new Date(dateObj)
    newDate.setHours(23, 59, 59, 999)
    return newDate
  }
}

/**
 * 便捷导出：直接使用函数形式
 */
export const formatDateTime = DateTimeHelper.format.bind(DateTimeHelper)
export const formatDate = DateTimeHelper.formatDate.bind(DateTimeHelper)
export const formatTime = DateTimeHelper.formatTime.bind(DateTimeHelper)
export const formatDateTimeShort = DateTimeHelper.formatDateTimeShort.bind(DateTimeHelper)
export const formatDateCN = DateTimeHelper.formatDateCN.bind(DateTimeHelper)
export const formatDateTimeCN = DateTimeHelper.formatDateTimeCN.bind(DateTimeHelper)
export const formatRelative = DateTimeHelper.formatRelative.bind(DateTimeHelper)
export const formatFriendly = DateTimeHelper.formatFriendly.bind(DateTimeHelper)
export const getTimestamp = DateTimeHelper.getTimestamp.bind(DateTimeHelper)
export const getTimestampSeconds = DateTimeHelper.getTimestampSeconds.bind(DateTimeHelper)
export const isToday = DateTimeHelper.isToday.bind(DateTimeHelper)
export const isYesterday = DateTimeHelper.isYesterday.bind(DateTimeHelper)
export const isThisWeek = DateTimeHelper.isThisWeek.bind(DateTimeHelper)
export const isThisYear = DateTimeHelper.isThisYear.bind(DateTimeHelper)
export const diffDateTime = DateTimeHelper.diff.bind(DateTimeHelper)
export const addDateTime = DateTimeHelper.add.bind(DateTimeHelper)
export const startOfDay = DateTimeHelper.startOfDay.bind(DateTimeHelper)
export const endOfDay = DateTimeHelper.endOfDay.bind(DateTimeHelper)

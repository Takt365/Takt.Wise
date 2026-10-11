// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/components/business/TaktCronEditor
// 文件名称：quartzCronCore.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 6 字段 Cron 构建/反解析与最近运行时间预览（参照博客园 BlackCatFish 自定义 cron 组件）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 单段 Cron 配置（秒/分/时/天/周/月/年 Tab 内 radio 状态） */
export interface QuartzCronFieldState {
  cronEvery: string
  incrementStart: number | string
  incrementIncrement: number | string
  rangeStart: number | string
  rangeEnd: number | string
  specificSpecific: number[]
  cronLastSpecificDomDay?: number
  cronDaysBeforeEomMinus?: number
  cronDaysNearestWeekday?: number
  cronNthDayDay?: number
  cronNthDayNth?: number | string
}

/** Cron 弹窗完整编辑状态 */
export interface QuartzCronEditorState {
  second: QuartzCronFieldState
  minute: QuartzCronFieldState
  hour: QuartzCronFieldState
  day: QuartzCronFieldState
  week: QuartzCronFieldState
  month: QuartzCronFieldState
  year: QuartzCronFieldState
}

/** 解析后的 Quartz Cron 六段文本 */
export interface QuartzCronSegments {
  second: string
  minute: string
  hour: string
  day: string
  month: string
  week: string
}

const currentYear = new Date().getFullYear()

/**
 * 创建默认 Cron 编辑状态
 * @returns 默认 state（各 Tab 选中「每一*」）
 */
export function createDefaultQuartzCronEditorState(): QuartzCronEditorState {
  return {
    second: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 5,
      rangeStart: 0,
      rangeEnd: 59,
      specificSpecific: [],
    },
    minute: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 5,
      rangeStart: 0,
      rangeEnd: 59,
      specificSpecific: [],
    },
    hour: {
      cronEvery: '1',
      incrementStart: 0,
      incrementIncrement: 1,
      rangeStart: 0,
      rangeEnd: 23,
      specificSpecific: [],
    },
    day: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 31,
      specificSpecific: [],
      cronLastSpecificDomDay: 1,
      cronDaysBeforeEomMinus: 1,
      cronDaysNearestWeekday: 1,
    },
    week: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 7,
      specificSpecific: [],
      cronNthDayDay: 1,
      cronNthDayNth: 1,
    },
    month: {
      cronEvery: '1',
      incrementStart: 1,
      incrementIncrement: 1,
      rangeStart: 1,
      rangeEnd: 12,
      specificSpecific: [],
    },
    year: {
      cronEvery: '1',
      incrementStart: currentYear,
      incrementIncrement: 1,
      rangeStart: currentYear,
      rangeEnd: currentYear,
      specificSpecific: [],
    },
  }
}

function buildSecondText(state: QuartzCronEditorState): string {
  const field = state.second
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '0'
  }
}

function buildMinuteText(state: QuartzCronEditorState): string {
  const field = state.minute
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

function buildHourText(state: QuartzCronEditorState): string {
  const field = state.hour
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '0'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

function buildDayText(state: QuartzCronEditorState): string {
  const field = state.day
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
    case '4':
    case '11':
      return '?'
    case '3':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '5':
      return field.specificSpecific.join(',') || '1'
    case '6':
      return 'L'
    case '7':
      return 'LW'
    case '8':
      return '?'
    case '9':
      return `L-${field.cronDaysBeforeEomMinus ?? 1}`
    case '10':
      return `${field.cronDaysNearestWeekday ?? 1}W`
    default:
      return '*'
  }
}

function buildWeekText(state: QuartzCronEditorState): string {
  const dayEvery = String(state.day.cronEvery)
  const field = state.week
  switch (dayEvery) {
    case '1':
    case '3':
    case '5':
      return '?'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '4':
      return field.specificSpecific.join(',') || '1'
    case '6':
    case '7':
    case '9':
    case '10':
      return '?'
    case '8':
      return `${state.day.cronLastSpecificDomDay ?? 1}L`
    case '11':
      return `${field.cronNthDayDay ?? 1}#${field.cronNthDayNth ?? 1}`
    default:
      return '?'
  }
}

function buildMonthText(state: QuartzCronEditorState): string {
  const field = state.month
  switch (String(field.cronEvery)) {
    case '1':
      return '*'
    case '2':
      return `${field.incrementStart}/${field.incrementIncrement}`
    case '3':
      return field.specificSpecific.join(',') || '1'
    case '4':
      return `${field.rangeStart}-${field.rangeEnd}`
    default:
      return '*'
  }
}

/**
 * 由编辑状态生成 Quartz 六段 Cron
 * @param state 弹窗编辑 state
 * @returns 秒 分 时 日 月 周 六段文本
 */
export function buildQuartzCronSegments(state: QuartzCronEditorState): QuartzCronSegments {
  return {
    second: buildSecondText(state) || '0',
    minute: buildMinuteText(state) || '*',
    hour: buildHourText(state) || '*',
    day: buildDayText(state) || '*',
    month: buildMonthText(state) || '*',
    week: buildWeekText(state) || '?',
  }
}

/**
 * 由编辑状态生成 Quartz Cron 字符串（对齐 Quartz.NET / 种子数据 6 字段）
 * @param state 弹窗编辑 state
 * @returns Cron 表达式
 */
export function buildQuartzCronExpression(state: QuartzCronEditorState): string {
  const seg = buildQuartzCronSegments(state)
  return `${seg.second} ${seg.minute} ${seg.hour} ${seg.day} ${seg.month} ${seg.week}`
}

function parseSecondPart(value: string): QuartzCronFieldState {
  const second = createDefaultQuartzCronEditorState().second
  if (value.includes('*')) {
    second.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    second.cronEvery = '2'
    second.incrementStart = Number(start)
    second.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    second.cronEvery = '3'
    second.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    second.cronEvery = '4'
    second.rangeStart = Number(start)
    second.rangeEnd = Number(end)
  } else if (/^\d+$/.test(value)) {
    second.cronEvery = '3'
    second.specificSpecific = [Number(value)]
  } else {
    second.cronEvery = '1'
  }
  return second
}

function parseMinutePart(value: string): QuartzCronFieldState {
  const minute = createDefaultQuartzCronEditorState().minute
  if (value.includes('*')) {
    minute.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    minute.cronEvery = '2'
    minute.incrementStart = Number(start)
    minute.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    minute.cronEvery = '3'
    minute.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    minute.cronEvery = '4'
    minute.rangeStart = Number(start)
    minute.rangeEnd = Number(end)
  } else if (/^\d+$/.test(value)) {
    minute.cronEvery = '3'
    minute.specificSpecific = [Number(value)]
  } else {
    minute.cronEvery = '1'
  }
  return minute
}

function parseHourPart(value: string): QuartzCronFieldState {
  const hour = createDefaultQuartzCronEditorState().hour
  if (value.includes('*')) {
    hour.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    hour.cronEvery = '2'
    hour.incrementStart = Number(start)
    hour.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    hour.cronEvery = '3'
    hour.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    hour.cronEvery = '4'
    hour.rangeStart = Number(start)
    hour.rangeEnd = Number(end)
  } else if (/^\d+$/.test(value)) {
    hour.cronEvery = '3'
    hour.specificSpecific = [Number(value)]
  } else {
    hour.cronEvery = '1'
  }
  return hour
}

function parseDayWeekParts(dayValue: string, weekValue: string): Pick<QuartzCronEditorState, 'day' | 'week'> {
  const day = createDefaultQuartzCronEditorState().day
  const week = createDefaultQuartzCronEditorState().week
  if (!dayValue.includes('?')) {
    switch (true) {
      case dayValue.includes('*'):
        day.cronEvery = '1'
        break
      case dayValue.includes('/'):
        day.cronEvery = '3'
        day.incrementStart = Number(dayValue.split('/')[0])
        day.incrementIncrement = Number(dayValue.split('/')[1])
        break
      case dayValue.includes(','):
        day.cronEvery = '5'
        day.specificSpecific = dayValue.split(',').map(Number).sort((a, b) => a - b)
        break
      case dayValue.includes('LW'):
        day.cronEvery = '7'
        break
      case dayValue.includes('L-'):
        day.cronEvery = '9'
        day.cronDaysBeforeEomMinus = Number(dayValue.split('L-')[1])
        break
      case dayValue.includes('L'):
        if (dayValue.length === 1) {
          day.cronEvery = '6'
        } else {
          day.cronEvery = '8'
          day.cronLastSpecificDomDay = Number(dayValue.split('L')[0])
        }
        break
      case dayValue.includes('W'):
        day.cronEvery = '10'
        day.cronDaysNearestWeekday = Number(dayValue.split('W')[0])
        break
      case /^\d+$/.test(dayValue):
        day.cronEvery = '5'
        day.specificSpecific = [Number(dayValue)]
        break
      default:
        day.cronEvery = '1'
    }
  } else {
    switch (true) {
      case weekValue.includes('/'):
        day.cronEvery = '2'
        week.incrementStart = Number(weekValue.split('/')[0])
        week.incrementIncrement = Number(weekValue.split('/')[1])
        break
      case weekValue.includes(','):
        day.cronEvery = '4'
        week.specificSpecific = weekValue.split(',').map(Number).sort((a, b) => a - b)
        break
      case weekValue.includes('#'):
        day.cronEvery = '11'
        week.cronNthDayDay = Number(weekValue.split('#')[0])
        week.cronNthDayNth = Number(weekValue.split('#')[1])
        break
      case /^\d+L$/.test(weekValue):
        day.cronEvery = '8'
        day.cronLastSpecificDomDay = Number(weekValue.replace('L', ''))
        break
      case /^\d+$/.test(weekValue):
        day.cronEvery = '4'
        week.specificSpecific = [Number(weekValue)]
        break
      default:
        day.cronEvery = '1'
    }
  }
  return { day, week }
}

function parseMonthPart(value: string): QuartzCronFieldState {
  const month = createDefaultQuartzCronEditorState().month
  if (value.includes('*')) {
    month.cronEvery = '1'
  } else if (value.includes('/')) {
    const [start, inc] = value.split('/')
    month.cronEvery = '2'
    month.incrementStart = Number(start)
    month.incrementIncrement = Number(inc)
  } else if (value.includes(',')) {
    month.cronEvery = '3'
    month.specificSpecific = value.split(',').map(Number).sort((a, b) => a - b)
  } else if (value.includes('-')) {
    const [start, end] = value.split('-')
    month.cronEvery = '4'
    month.rangeStart = Number(start)
    month.rangeEnd = Number(end)
  } else if (/^\d+$/.test(value)) {
    month.cronEvery = '3'
    month.specificSpecific = [Number(value)]
  } else {
    month.cronEvery = '1'
  }
  return month
}

/**
 * 将 Quartz Cron 字符串反解析为编辑状态
 * @param expression Cron 表达式
 * @returns 弹窗 state；空串返回默认 state
 */
export function parseQuartzCronExpression(expression: string): QuartzCronEditorState {
  const trimmed = String(expression ?? '').trim()
  if (!trimmed) {
    return createDefaultQuartzCronEditorState()
  }
  const parts = trimmed.split(/\s+/)
  if (parts.length < 6) {
    return createDefaultQuartzCronEditorState()
  }
  const [second, minute, hour, day, month, week] = parts
  const dayWeek = parseDayWeekParts(day, week)
  return {
    second: parseSecondPart(second),
    minute: parseMinutePart(minute),
    hour: parseHourPart(hour),
    day: dayWeek.day,
    week: dayWeek.week,
    month: parseMonthPart(month),
    year: createDefaultQuartzCronEditorState().year,
  }
}

/** 人类可读说明用的字段标签（由调用方 i18n 注入） */
export interface QuartzCronDescribeLabels {
  everySecond: string
  everyMinute: string
  everyHour: string
  everyDay: string
  everyMonth: string
  atTime: (h: string, m: string, s: string) => string
  intervalSeconds: (start: string, step: string) => string
  intervalMinutes: (start: string, step: string) => string
  intervalHours: (start: string, step: string) => string
  specificSeconds: (values: string) => string
  specificMinutes: (values: string) => string
  specificHours: (values: string) => string
  specificDays: (values: string) => string
  specificMonths: (values: string) => string
  specificWeeks: (values: string) => string
  weekday: (n: number) => string
  unknown: string
  join: string
}

/**
 * 格式化两位时间片段
 * @param n 数字
 * @returns 两位字符串
 */
function pad2(n: number): string {
  return String(Math.max(0, Math.trunc(n))).padStart(2, '0')
}

/**
 * 将字段段简述为可读文案（无法识别时返回空串，由上层拼装）
 * @param field 段原始文本
 * @param kind second|minute|hour|day|month|week
 * @param labels 文案
 * @returns 简述；空表示该段用通配可不单独强调
 */
function describeFieldPart(
  field: string,
  kind: 'second' | 'minute' | 'hour' | 'day' | 'month' | 'week',
  labels: QuartzCronDescribeLabels,
): string {
  const v = String(field ?? '').trim()
  if (!v || v === '*' || v === '?') {
    return ''
  }
  if (v.includes('/')) {
    const [start, step] = v.split('/')
    if (kind === 'second') return labels.intervalSeconds(start, step)
    if (kind === 'minute') return labels.intervalMinutes(start, step)
    if (kind === 'hour') return labels.intervalHours(start, step)
  }
  if (/^[\d,]+$/.test(v)) {
    if (kind === 'second') return labels.specificSeconds(v)
    if (kind === 'minute') return labels.specificMinutes(v)
    if (kind === 'hour') return labels.specificHours(v)
    if (kind === 'day') return labels.specificDays(v)
    if (kind === 'month') return labels.specificMonths(v)
    if (kind === 'week') {
      const names = v.split(',').map((x) => labels.weekday(Number(x))).join(',')
      return labels.specificWeeks(names)
    }
  }
  return ''
}

/**
 * 生成 Cron 人类可读说明（打开弹窗时展示原参数含义）
 * @param expression Quartz 六段表达式
 * @param labels i18n 文案
 * @returns 说明文本；无法解析时返回 unknown
 */
export function describeQuartzCronExpression(expression: string, labels: QuartzCronDescribeLabels): string {
  const trimmed = String(expression ?? '').trim()
  if (!trimmed) {
    return labels.unknown
  }
  const parts = trimmed.split(/\s+/)
  if (parts.length < 6) {
    return labels.unknown
  }
  const [second, minute, hour, day, month, week] = parts
  // 高频：0 0 H * * ? → 每天 HH:00:00
  if (/^\d+$/.test(second) && /^\d+$/.test(minute) && /^\d+$/.test(hour) && day === '*' && month === '*' && week === '?') {
    return labels.atTime(pad2(Number(hour)), pad2(Number(minute)), pad2(Number(second)))
  }
  const chunks = [
    describeFieldPart(second, 'second', labels),
    describeFieldPart(minute, 'minute', labels),
    describeFieldPart(hour, 'hour', labels),
    describeFieldPart(day, 'day', labels),
    describeFieldPart(month, 'month', labels),
    describeFieldPart(week, 'week', labels),
  ].filter(Boolean)
  if (chunks.length === 0) {
    if (second === '*' && minute === '*' && hour === '*') {
      return labels.everySecond
    }
    return labels.unknown
  }
  return chunks.join(labels.join)
}

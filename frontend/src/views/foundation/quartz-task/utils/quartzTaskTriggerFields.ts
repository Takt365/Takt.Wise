// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/quartz-task/utils
// 文件名称：quartzTaskTriggerFields.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 触发器 / Cron / Misfire 表单字段 i18n 与互斥字段清理（对齐 TaktQuartzSchedulerManager、sys_quartz_trigger_type / sys_quartz_misfire_policy）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TaktQuartzTriggerType } from '@/constants/takt.constants'

/** 触发器类型字典码 */
export const QUARTZ_TRIGGER_TYPE_DICT_TYPE = 'sys_quartz_trigger_type' as const

/** Misfire 策略字典码 */
export const QUARTZ_MISFIRE_POLICY_DICT_TYPE = 'sys_quartz_misfire_policy' as const

/** 随触发器类型切换显隐的字段 */
export const QUARTZ_TRIGGER_TYPE_FIELDS = ['cronExpression', 'intervalSeconds'] as const

export type QuartzTriggerTypeField = (typeof QUARTZ_TRIGGER_TYPE_FIELDS)[number]

/** 触发器相关字段 → 标签 / 问号提示 / 占位符 i18n 键（对齐 sqlScript extField 模式） */
export const QUARTZ_TASK_TRIGGER_FIELD_I18N: Readonly<
  Record<
    'triggerType' | QuartzTriggerTypeField | 'misfirePolicy',
    { label: string; hint: string; placeholder: string }
  >
> = {
  triggerType: {
    label: 'entity.quartztask.triggertype',
    hint: 'entity.quartztask.triggertypehint',
    placeholder: 'common.page.form.placeholder.quartztask.triggertype',
  },
  cronExpression: {
    label: 'entity.quartztask.cronexpression',
    hint: 'entity.quartztask.cronexpressionhint',
    placeholder: 'common.page.form.placeholder.quartztask.cronexpression',
  },
  intervalSeconds: {
    label: 'entity.quartztask.intervalseconds',
    hint: 'entity.quartztask.intervalsecondshint',
    placeholder: 'common.page.form.placeholder.quartztask.intervalseconds',
  },
  misfirePolicy: {
    label: 'entity.quartztask.misfirepolicy',
    hint: 'entity.quartztask.misfirepolicyhint',
    placeholder: 'common.page.form.placeholder.quartztask.misfirepolicy',
  },
}

/**
 * 规范化触发器类型（0=Simple 1=Cron）
 * @param value 表单 triggerType
 * @returns 0 或 1；非法值回退 Cron
 */
export function normalizeQuartzTriggerTypeValue(value: unknown): number {
  const num = typeof value === 'number' ? value : Number(value)
  if (num === TaktQuartzTriggerType.Simple) {
    return TaktQuartzTriggerType.Simple
  }
  return TaktQuartzTriggerType.Cron
}

/**
 * 提交前清理与当前触发器类型互斥的字段，避免 Simple 仍携带 Cron 表达式
 * @param target 表单 state 或 getValues payload
 * @param triggerType 触发器类型
 */
export function clearQuartzTriggerHiddenFields(
  target: Record<string, unknown>,
  triggerType: unknown,
): void {
  const normalized = normalizeQuartzTriggerTypeValue(triggerType)
  if (normalized === TaktQuartzTriggerType.Cron) {
    target.intervalSeconds = 0
  } else {
    target.cronExpression = ''
  }
}

// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/quartz-task/utils
// 文件名称：quartzTaskTypeFields.ts
// 创建时间：2026-06-28
// 创建人：Takt365(Cursor AI)
// 功能描述：sys_quartz_task_type 字典 DictValue 与 Quartz 任务执行参数字段显隐映射（对齐 TaktConstants.QuartzTaskType、TaktQuartzTask 实体注释）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { TaktQuartzTaskType } from '@/constants/takt.constants'

/** 任务类型字典码（与 TaktSelect dict-type、TaktDictTypeSeedData 一致） */
export const QUARTZ_TASK_TYPE_DICT_TYPE = 'sys_quartz_task_type' as const

/** 随任务类型切换显隐的执行参数字段 */
export const QUARTZ_TASK_TYPE_EXEC_FIELDS = [
  'assemblyName',
  'className',
  'apiUrl',
  'requestMethod',
  'sqlScript',
] as const

export type QuartzTaskTypeExecField = (typeof QUARTZ_TASK_TYPE_EXEC_FIELDS)[number]

/** 执行参数字段 → 标签 / 问号提示 / 占位符 i18n 键（对齐 extField 模式） */
export const QUARTZ_TASK_EXEC_FIELD_I18N: Readonly<
  Record<QuartzTaskTypeExecField, { label: string; hint: string; placeholder: string }>
> = {
  assemblyName: {
    label: 'entity.quartztask.assemblyname',
    hint: 'common.page.form.placeholder.quartztask.assemblyname',
    placeholder: 'common.page.form.placeholder.quartztask.assemblyname',
  },
  className: {
    label: 'entity.quartztask.classname',
    hint: 'common.page.form.placeholder.quartztask.classname',
    placeholder: 'common.page.form.placeholder.quartztask.classname',
  },
  apiUrl: {
    label: 'entity.quartztask.apiurl',
    hint: 'common.page.form.placeholder.quartztask.apiurl',
    placeholder: 'common.page.form.placeholder.quartztask.apiurl',
  },
  requestMethod: {
    label: 'entity.quartztask.requestmethod',
    hint: 'common.page.form.placeholder.quartztask.requestmethod',
    placeholder: 'common.page.form.placeholder.quartztask.requestmethod',
  },
  sqlScript: {
    label: 'entity.quartztask.sqlscript',
    hint: 'common.page.form.placeholder.quartztask.sqlscript',
    placeholder: 'common.page.form.placeholder.quartztask.sqlscript',
  },
}

/**
 * sys_quartz_task_type 的 DictValue → 当前类型应展示的表单字段
 * assembly → 程序集名称 + 类名；http → API + 方法；sql → SQL 脚本
 */
export const QUARTZ_TASK_TYPE_VISIBLE_FIELD_MAP: Readonly<Record<string, readonly QuartzTaskTypeExecField[]>> = {
  [TaktQuartzTaskType.Assembly]: ['assemblyName', 'className'],
  [TaktQuartzTaskType.Http]: ['apiUrl', 'requestMethod'],
  [TaktQuartzTaskType.Sql]: ['sqlScript'],
}

/**
 * 规范化任务类型 DictValue（TaktSelect v-model / 后端 TaskType 均为 dictValue 字符串）
 * @param value 表单 taskType
 * @returns 小写 trim 后的 dictValue
 */
export function normalizeQuartzTaskTypeValue(value: unknown): string {
  return String(value ?? '').trim().toLowerCase()
}

/**
 * 当前任务类型应展示的执行参数字段列表
 * @param taskType sys_quartz_task_type 选中项 DictValue
 * @returns 可见字段 key 列表；未知类型返回空数组
 */
export function getQuartzTaskTypeVisibleFields(taskType: unknown): readonly QuartzTaskTypeExecField[] {
  const normalized = normalizeQuartzTaskTypeValue(taskType)
  return QUARTZ_TASK_TYPE_VISIBLE_FIELD_MAP[normalized] ?? []
}

/**
 * 判断某执行参数字段在当前任务类型下是否应展示
 * @param taskType sys_quartz_task_type 选中项 DictValue
 * @param fieldKey 执行参数字段名
 * @returns 是否可见
 */
export function isQuartzTaskTypeFieldVisible(taskType: unknown, fieldKey: QuartzTaskTypeExecField): boolean {
  return getQuartzTaskTypeVisibleFields(taskType).includes(fieldKey)
}

/**
 * 生成五种执行参数字段的显隐布尔表（供模板 v-if 使用）
 * @param taskType sys_quartz_task_type 选中项 DictValue
 * @returns 字段 → 是否展示
 */
export function buildQuartzTaskTypeFieldVisibility(taskType: unknown): Record<QuartzTaskTypeExecField, boolean> {
  const visible = new Set(getQuartzTaskTypeVisibleFields(taskType))
  return {
    assemblyName: visible.has('assemblyName'),
    className: visible.has('className'),
    apiUrl: visible.has('apiUrl'),
    requestMethod: visible.has('requestMethod'),
    sqlScript: visible.has('sqlScript'),
  }
}

/**
 * 切换任务类型后清空当前类型不可见的执行参数字段，避免隐藏项残留提交
 * @param target 表单 state
 * @param taskType sys_quartz_task_type 选中项 DictValue
 */
export function clearQuartzTaskTypeHiddenFields(
  target: Record<string, unknown>,
  taskType: unknown,
): void {
  const visible = new Set(getQuartzTaskTypeVisibleFields(taskType))
  for (const key of QUARTZ_TASK_TYPE_EXEC_FIELDS) {
    if (!visible.has(key)) {
      target[key] = ''
    }
  }
}

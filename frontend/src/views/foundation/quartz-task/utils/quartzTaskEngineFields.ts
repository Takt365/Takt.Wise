// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/foundation/quartz-task/utils
// 文件名称：quartzTaskEngineFields.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：Quartz 任务由调度引擎维护的运行时字段（禁止 create/update 表单编辑或提交）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 引擎维护字段：首次/上次/下次执行与执行次数（列表只读展示，表单不可维护） */
export const QUARTZ_TASK_ENGINE_MANAGED_FIELDS = [
  'firstRunAt',
  'executeCount',
  'lastRunAt',
  'nextRunAt',
] as const

export type QuartzTaskEngineManagedField = (typeof QUARTZ_TASK_ENGINE_MANAGED_FIELDS)[number]

/**
 * 从 create/update payload 剔除引擎维护字段，避免覆盖调度器写入的运行时数据
 * @param target 表单 getValues 结果
 */
export function stripQuartzTaskEngineManagedFields(target: Record<string, unknown>): void {
  for (const key of QUARTZ_TASK_ENGINE_MANAGED_FIELDS) {
    delete target[key]
  }
}

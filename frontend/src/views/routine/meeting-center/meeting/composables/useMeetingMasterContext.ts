// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/routine/meeting-center/meeting/composables
// 文件名称：useMeetingMasterContext.ts
// 功能描述：会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Meeting } from '@/types/routine/meeting-center/meeting.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type MeetingRowRecord = Meeting | Record<string, unknown>

/** 主表选中行上下文 */
export interface MeetingMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<MeetingRowRecord | null>
}

const meetingMasterContextKey: InjectionKey<MeetingMasterContext> = Symbol('meetingMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {MeetingMasterContext} 主表上下文
 */
export function provideMeetingMasterContext(): MeetingMasterContext {
  const selecmedMasterRow = ref<MeetingRowRecord | null>(null)
  const ctx: MeetingMasterContext = { selecmedMasterRow }
  provide(meetingMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {MeetingMasterContext} 主表上下文
 */
export function useMeetingMasterContext(): MeetingMasterContext {
  const ctx = inject(meetingMasterContextKey)
  if (!ctx) {
    throw new Error('useMeetingMasterContext must be used within meeting index')
  }
  return ctx
}

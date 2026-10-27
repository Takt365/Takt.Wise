// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/human-resource/personnel/employee-delegation/composables
// 文件名称：useEmployeeMasterContext.ts
// 功能描述：员工实体主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { Employee } from '@/types/human-resource/personnel/employee.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type EmployeeRowRecord = Employee | Record<string, unknown>

/** 主表选中行上下文 */
export interface EmployeeMasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<EmployeeRowRecord | null>
}

const employeeMasterContextKey: InjectionKey<EmployeeMasterContext> = Symbol('employeeMasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {EmployeeMasterContext} 主表上下文
 */
export function provideEmployeeMasterContext(): EmployeeMasterContext {
  const selecmedMasterRow = ref<EmployeeRowRecord | null>(null)
  const ctx: EmployeeMasterContext = { selecmedMasterRow }
  provide(employeeMasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {EmployeeMasterContext} 主表上下文
 */
export function useEmployeeMasterContext(): EmployeeMasterContext {
  const ctx = inject(employeeMasterContextKey)
  if (!ctx) {
    throw new Error('useEmployeeMasterContext must be used within employee index')
  }
  return ctx
}

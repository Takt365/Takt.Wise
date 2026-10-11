// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/components/business/TaktFlowAntDesigner/config
// 文件名称：useFlowDrawer.ts
// 创建时间：2026-04-07
// 创建人：Takt365(Cursor AI)
// 功能描述：流程设计器各抽屉共享状态（open/close/save）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { reactive } from 'vue'
import type { FlowTreeNode } from './taktFlowTree'

export type DrawerType = 'promoter' | 'approver' | 'copyer' | 'condition' | null

export interface FlowDrawerState {
  visible: boolean
  type: DrawerType
  config: FlowTreeNode | null
  /** 条件分支时当前编辑的条件下标 */
  conditionIndex: number | null
  /** 确定时回写：由调用方传入，用于把表单数据写回树节点 */
  onSave: ((updated: FlowTreeNode) => void) | null
}

const state = reactive<FlowDrawerState>({
  visible: false,
  type: null,
  config: null,
  conditionIndex: null,
  onSave: null
})

export function useFlowDrawer() {
  function open(type: DrawerType, config: FlowTreeNode | null, conditionIndex: number | null, onSave: (updated: FlowTreeNode) => void) {
    state.type = type
    state.config = config ? JSON.parse(JSON.stringify(config)) : null
    state.conditionIndex = conditionIndex
    state.onSave = onSave
    state.visible = true
  }
  function close() {
    state.visible = false
    state.type = null
    state.config = null
    state.conditionIndex = null
    state.onSave = null
  }
  function save(updated: FlowTreeNode) {
    state.onSave?.(updated)
    close()
  }
  return { state, open, close, save }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：taktTransfer.ts
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：分配弹窗 Transfer 共用 filter、视口样式、滚动与单击/双击穿梭交互；UI 见 components/business/TaktTransfer。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { CSSProperties } from 'vue'
import type { TransferProps } from 'ant-design-vue'

/** 分配弹窗 dataSource 行（字段与官方 TransferItem 一致，key 可选） */
export type TaktTransferRecord = NonNullable<TransferProps['dataSource']>[number]

/** Transfer filter-option（与 a-transfer filterOption 类型完全一致） */
export type TaktTransferFilterOption = NonNullable<TransferProps['filterOption']>

/** Transfer 列表方向 */
export type TaktAssignTransferDirection = 'left' | 'right'

/**
 * Transfer filter-option：按 title / description 模糊匹配（对齐官方示例）
 * @param inputValue 搜索框输入
 * @param option 当前行
 * @returns 是否保留
 */
export const taktTransferFilterOption: TaktTransferFilterOption = (inputValue, option) => {
  const q = inputValue.trim()
  if (!q) return true
  const lower = q.toLowerCase()
  const title = String(option.title ?? '')
  const description = String(option.description ?? '')
  return title.toLowerCase().includes(lower) || description.toLowerCase().includes(lower)
}

/** 分配弹窗列表区高度（相对当前视口；配合 global.css 内 flex + .takt-transfer-panel-scroll 滚动） */
export const TAKT_ASSIGN_TRANSFER_LIST_STYLE: CSSProperties = {
  height: '50vh',
}

/** 操作列 ← → 在列表高度内垂直居中（官方 operationStyle） */
export const TAKT_TRANSFER_OPERATION_STYLE: CSSProperties = {
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'center',
}

/**
 * Transfer 列表滚动回调（对齐官方 @scroll 示例，预留 lazy load）
 * @param _direction 左/右列
 * @param _event 滚动事件
 */
export function taktAssignTransferHandleScroll(
  _direction: TaktAssignTransferDirection,
  _event: Event,
): void {
  // 当前无分页/lazy load；保留钩子供后续扩展
}

/**
 * 单击切换行选中（配合 Transfer children 槽 onItemSelect）
 * @param key 行 key
 * @param selectedKeys 当前列已选 key
 * @param onItemSelect Transfer 选中回调
 * @param listDisabled 整列禁用
 * @param itemDisabled 行禁用
 */
export function taktAssignTransferHandleItemClick(
  key: string | number,
  selectedKeys: readonly (string | number)[],
  onItemSelect: (itemKey: string, selected: boolean) => void,
  listDisabled?: boolean,
  itemDisabled?: boolean,
): void {
  if (listDisabled || itemDisabled) return
  const itemKey = String(key)
  const isSelected = selectedKeys.some(k => String(k) === itemKey)
  onItemSelect(itemKey, !isSelected)
}

/**
 * 双击自动穿梭到对侧（左→右加入 targetKeys，右→左移除）
 * @param key 行 key
 * @param direction 当前列方向
 * @param getTargetKeys 读取 targetKeys
 * @param setTargetKeys 写回 targetKeys
 * @param onItemSelect 可选：穿梭后取消选中
 * @param listDisabled 整列禁用
 * @param itemDisabled 行禁用
 */
export function taktAssignTransferShuttleByDblClick(
  key: string | number,
  direction: TaktAssignTransferDirection,
  getTargetKeys: () => string[],
  setTargetKeys: (keys: string[]) => void,
  onItemSelect?: (itemKey: string, selected: boolean) => void,
  listDisabled?: boolean,
  itemDisabled?: boolean,
): void {
  if (listDisabled || itemDisabled) return
  const itemKey = String(key)
  const current = getTargetKeys()
  if (direction === 'left') {
    if (!current.includes(itemKey)) {
      setTargetKeys([...current, itemKey])
    }
  } else {
    setTargetKeys(current.filter(id => id !== itemKey))
  }
  onItemSelect?.(itemKey, false)
}

/**
 * 判断 Transfer 行是否选中
 * @param key 行 key
 * @param selectedKeys 当前列已选 key
 * @returns 是否选中
 */
export function taktAssignTransferIsItemSelected(
  key: string | number,
  selectedKeys: readonly (string | number)[],
): boolean {
  const itemKey = String(key)
  return selectedKeys.some(k => String(k) === itemKey)
}

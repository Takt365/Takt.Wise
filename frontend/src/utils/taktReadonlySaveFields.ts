// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/utils/taktReadonlySaveFields
// 文件名称：taktReadonlySaveFields.ts
// 创建时间：2026-10-12
// 创建人：Takt365(Cursor AI)
// 功能描述：运行时网关。弹窗或抽屉点确定时记录表单禁用项，供创建/更新的全角检查跳过
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/** 请求头：本次保存时处于禁用状态的表单项名称 */
export const TAKT_READONLY_FIELDS_HEADER = 'X-Takt-Readonly-Fields'

/** 快照有效期。校验失败后未发出保存请求时，避免把旧名单套到下一次无关保存 */
const SNAPSHOT_TTL_MS = 120_000

/** 确定按钮（点击时表单尚未进入提交 loading，禁用项仍是真正的只读项） */
const SUBMIT_BUTTON_SELECTOR = '.ant-modal-footer .ant-btn-primary, .ant-drawer-footer .ant-btn-primary'

/** 禁用控件。提交 loading 尚未加上，因此不含仅因 loading 而禁用的可编辑项 */
const DISABLED_CONTROL_SELECTOR = [
  'input.ant-input-disabled',
  'textarea.ant-input-disabled',
  '.ant-input-number-disabled',
  '.ant-select-disabled',
  '.ant-picker-disabled',
  '.ant-radio-group-disabled',
  '.ant-checkbox-wrapper-disabled',
].join(', ')

/** 待交给下一次创建/更新请求的只读项 */
let pendingReadonlyFields: { at: number; names: Set<string>; root: Element } | null = null

/** 是否已注册点击捕获 */
let captureInstalled = false

/**
 * 注册确定按钮捕获。重复调用无效果。
 * @returns {void}
 */
export function installReadonlySaveFieldCapture(): void {
  if (captureInstalled || typeof document === 'undefined') {
    return
  }
  captureInstalled = true
  document.addEventListener('click', onFooterSubmitClick, true)
}

/**
 * 取出并清空最近一次确定时的只读项。过期则丢弃。
 * @returns {ReadonlySet<string>} 只读项名称；没有时为空集
 */
export function consumeReadonlySaveFieldNames(): ReadonlySet<string> {
  const pending = pendingReadonlyFields
  pendingReadonlyFields = null
  if (!pending || Date.now() - pending.at > SNAPSHOT_TTL_MS || !pending.root.isConnected) {
    return new Set()
  }
  return pending.names
}

/**
 * 确定按钮点击：在提交 loading 禁用全部控件之前，记下当前禁用项
 * @param event 点击事件
 * @returns {void}
 */
function onFooterSubmitClick(event: Event): void {
  const target = event.target
  if (!(target instanceof Element)) {
    return
  }
  const button = target.closest(SUBMIT_BUTTON_SELECTOR)
  if (!(button instanceof HTMLElement) || button.classList.contains('ant-btn-loading')) {
    return
  }
  const root = button.closest('.ant-modal, .ant-drawer')
  if (!root) {
    return
  }
  pendingReadonlyFields = { at: Date.now(), names: collectReadonlyFieldNames(root), root }
}

/**
 * 收集容器内禁用表单项的字段名
 * @param root 弹窗或抽屉
 * @returns {Set<string>} 字段名
 */
function collectReadonlyFieldNames(root: Element): Set<string> {
  const names = new Set<string>()
  root.querySelectorAll('.ant-form-item').forEach((item) => {
    if (!item.querySelector(DISABLED_CONTROL_SELECTOR)) {
      return
    }
    const controlId = item.querySelector('label[for]')?.getAttribute('for') ?? ''
    const name = fieldNameFromControlId(controlId)
    if (name) {
      names.add(name)
    }
  })
  return names
}

/**
 * 从表单项控件 id 取出字段名。Ant Design Vue 默认 id 为 form_item_{字段名}
 * @param rawId 控件 id
 * @returns {string} 字段名；无法识别时为空串
 */
function fieldNameFromControlId(rawId: string): string {
  const id = rawId.trim()
  if (!id) {
    return ''
  }
  const marker = 'form_item_'
  const index = id.lastIndexOf(marker)
  const name = index >= 0 ? id.slice(index + marker.length) : id
  return /^[A-Za-z0-9_.]+$/.test(name) ? name : ''
}

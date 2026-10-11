// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables
// 文件名称：useEcAttachmentPreview.ts
// 功能描述：设变附件文件编码超链接：走 TaktEcAttachments/{id}/preview 鉴权流，禁止把 /uploads 拼到前端源（会进 Vue 403）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { TaktApiError } from '@/api/request'
import { previewEcAttachment } from '@/api/logistics/manufacturing/engineering-change/ecAttachment.api'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'

/** 技术部门附件预览权限（与菜单按钮 Generic preview、控制器 GET {id}/preview 一致） */
export const EC_GIJUTSU_PREVIEW_PERMISSION = 'logistics:manufacturing:engineering:change:gijutsu:preview'

/** blob URL 释放延迟（毫秒） */
const PREVIEW_BLOB_REVOKE_MS = 60_000

/**
 * 设变附件预览：已保存行调鉴权 preview API；未保存行仅打开绝对 https AccessUrl
 * @returns 预览权限与打开方法
 */
export function useEcAttachmentPreview() {
  const { t } = useI18n()
  const permissionStore = usePermissionStore()
  /** 当前用户是否具备技术部门预览权限 */
  const canPreviewAttachment = computed(() => permissionStore.hasPermission(EC_GIJUTSU_PREVIEW_PERMISSION))

  /**
   * 访问地址是否可预览（排除空值与危险 scheme）
   * @param {unknown} accessUrl 访问地址
   * @returns {boolean} 可预览
   */
  function hasPreviewableAccessUrl(accessUrl: unknown): boolean {
    const raw = String(accessUrl ?? '').trim()
    if (!raw || raw === '-') {
      return false
    }
    if (/^(javascript|data|vbscript):/i.test(raw)) {
      return false
    }
    return true
  }

  /**
   * 解析已持久化附件主键（排除表单临时行）
   * @param {Record<string, unknown>} record 附件行
   * @returns {string} 主键，临时行为空串
   */
  function resolvePersistedAttachmentId(record: Record<string, unknown>): string {
    const id = String(record.ecAttachmentId ?? record.id ?? '').trim()
    if (!id || id.startsWith('client-') || id.startsWith('row-')) {
      return ''
    }
    return id
  }

  /**
   * 在已打开的空白页中展示 blob，避免 await 后弹窗被拦截
   * @param {Blob} blob 文件流
   * @param {Window | null} previewWindow 同步打开的空白窗
   * @returns {void}
   */
  function openBlobInWindow(blob: Blob, previewWindow: Window | null): void {
    if (blob.type.includes('text/html')) {
      previewWindow?.close()
      message.error(t('common.feedback.failed'))
      return
    }
    const url = URL.createObjectURL(blob)
    if (previewWindow && !previewWindow.closed) {
      previewWindow.location.replace(url)
    } else {
      window.open(url, '_blank', 'noopener,noreferrer')
    }
    window.setTimeout(() => URL.revokeObjectURL(url), PREVIEW_BLOB_REVOKE_MS)
  }

  /**
   * 打开附件预览（禁止把相对 AccessUrl 拼到 VITE_APP_ORIGIN，否则新标签走 Vue 路由 403）
   * @param {Record<string, unknown>} record 附件行
   * @param {Window | null} previewWindow 点击时同步 window.open 的空白页
   * @returns {Promise<void>}
   */
  async function openAttachmentPreview(
    record: Record<string, unknown>,
    previewWindow: Window | null,
  ): Promise<void> {
    const id = resolvePersistedAttachmentId(record)
    try {
      if (id) {
        const blob = await previewEcAttachment(id)
        openBlobInWindow(blob, previewWindow)
        return
      }
      const raw = String(record.accessUrl ?? '').trim()
      if (!hasPreviewableAccessUrl(raw)) {
        previewWindow?.close()
        return
      }
      if (/^https?:\/\//i.test(raw)) {
        if (previewWindow && !previewWindow.closed) {
          previewWindow.location.replace(raw)
        } else {
          window.open(raw, '_blank', 'noopener,noreferrer')
        }
        return
      }
      previewWindow?.close()
      message.error(t('common.feedback.failed'))
    } catch (error: unknown) {
      previewWindow?.close()
      if (error instanceof TaktApiError) {
        return
      }
      const status = (error as { response?: { status?: number } })?.response?.status
      if (typeof status === 'number') {
        return
      }
      message.error(t('common.feedback.failed'))
    }
  }

  /**
   * 文件编码超链接点击：先同步开空白页，再拉鉴权文件流
   * @param {Record<string, unknown>} record 附件行
   * @param {MouseEvent} event 点击事件
   * @returns {void}
   */
  function handleAttachmentDocCodeClick(record: Record<string, unknown>, event: MouseEvent): void {
    event.preventDefault()
    event.stopPropagation()
    const previewWindow = window.open('about:blank', '_blank')
    void openAttachmentPreview(record, previewWindow)
  }

  return {
    previewPermission: EC_GIJUTSU_PREVIEW_PERMISSION,
    canPreviewAttachment,
    hasPreviewableAccessUrl,
    handleAttachmentDocCodeClick,
    openAttachmentPreview,
  }
}

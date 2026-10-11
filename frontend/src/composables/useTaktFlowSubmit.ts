// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktFlowSubmit.ts
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：通用「有后台表」提交审批（调用引擎 submit-by-table，无需每实体专用 API）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { ref } from 'vue'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { submitFlowApprovalByTable } from '@/api/workflow/flowEngine.api'
import type { FlowInstanceDetail } from '@/types/workflow/flowEngine.types'

/**
 * 业务列表页「提交审批」组合式（有后台表流程）
 * @param relatedTableName 表单 RelatedTableName / 实体物理表名
 * @returns loading 与 submitApproval 方法
 */
export function useTaktFlowSubmit(relatedTableName: string) {
  const { t } = useI18n()
  /** 提交审批 loading */
  const submitApprovalLoading = ref(false)

  /**
   * 按业务主键提交审批
   * @param entityId 业务主键（string，对齐雪花 ID）
   * @param processKey 可选流程键
   * @returns 流程实例详情
   */
  async function submitApproval(entityId: string, processKey?: string): Promise<FlowInstanceDetail | null> {
    if (!relatedTableName?.trim() || !entityId?.trim()) {
      message.warning(t('common.feedback.failed'))
      return null
    }
    submitApprovalLoading.value = true
    try {
      const detail = await submitFlowApprovalByTable({
        relatedTableName: relatedTableName.trim(),
        entityId: entityId.trim(),
        processKey: processKey?.trim() || undefined,
      })
      message.success(t('common.feedback.success'))
      return detail
    } catch (error: unknown) {
      const msg = error instanceof Error ? error.message : String(error)
      message.error(msg || t('common.feedback.failed'))
      return null
    } finally {
      submitApprovalLoading.value = false
    }
  }

  return {
    submitApprovalLoading,
    submitApproval,
  }
}

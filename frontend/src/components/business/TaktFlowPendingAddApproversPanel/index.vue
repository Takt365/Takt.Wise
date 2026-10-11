<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/components/business/TaktFlowPendingAddApproversPanel -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：流程实例未处理加签列表；可选减签（pendingAddApprovers、reduce-sign API） -->
<!-- ======================================== -->
<template>
  <div
    v-if="pendingAddApproverItems.length"
    class="flow-pending-add"
  >
    <div class="flow-pending-add__title">
      {{ t('entity.flowinstance.addsigns') }}
    </div>
    <div
      v-for="p in pendingAddApproverItems"
      :key="p.flowAddSignId"
      class="flow-pending-add__row"
    >
      <span class="flow-pending-add__name">{{ p.approverUserName }}</span>
      <a-button
        v-if="allowReduce"
        v-permission="'workflow:todo:reducesign'"
        type="link"
        size="small"
        :loading="loadingId === p.flowAddSignId"
        @click="onReduce(p.flowAddSignId)"
      >
        {{ t('common.page.button.reducesign') }}
      </a-button>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 未处理加签列表；allowReduce 为 true 时显示减签（需 detail.canVerify 由父级控制）。
 */
import { computed, ref } from 'vue'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { reduceFlowEngineSign } from '@/api/workflow/flowEngine.api'
import type { FlowInstanceDetail, FlowPendingAddApprover } from '@/types/workflow/flowEngine.types'

const { t } = useI18n()

const props = withDefaults(
  defineProps<{ detail: FlowInstanceDetail | null; allowReduce?: boolean }>(),
  { allowReduce: false }
)
const emit = defineEmits<{ refresh: [] }>()

const loadingId = ref<string | null>(null)

/** 校验未处理加签项结构（兼容 API 偶发脏数据） */
function isFlowPendingAddApprover(item: unknown): item is FlowPendingAddApprover {
  if (item == null || typeof item !== 'object') return false
  const row = item as Partial<FlowPendingAddApprover>
  return typeof row.flowAddSignId === 'string' && typeof row.approverUserName === 'string'
}

const pendingAddApproverItems = computed<FlowPendingAddApprover[]>(() => {
  const list = props.detail?.pendingAddApprovers
  if (!Array.isArray(list)) return []
  return list.filter(isFlowPendingAddApprover)
})

/**
 * 减签
 * @param flowAddSignId 加签记录 ID
 */
async function onReduce(flowAddSignId: string) {
  const d = props.detail
  const flowInstanceId = d?.flowInstanceId
  if (!flowInstanceId) return
  loadingId.value = flowAddSignId
  try {
    await reduceFlowEngineSign({
      flowInstanceId,
      instanceCode: d?.instanceCode,
      flowAddSignId
    })
    message.success(t('workflow.instance.page.msg.reduce.sign.success'))
    emit('refresh')
  } catch (e: unknown) {
    const err = e as { message?: string }
    message.error(err?.message || t('workflow.instance.page.msg.reduce.sign.fail'))
  } finally {
    loadingId.value = null
  }
}
</script>

<style scoped lang="css">
.flow-pending-add {
  margin-top: 12px;
  padding-top: 12px;
  border-top: 1px solid var(--ant-color-border-secondary);
}
.flow-pending-add__title {
  font-weight: 600;
  margin-bottom: 8px;
  color: var(--ant-color-text);
}
.flow-pending-add__row {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 4px;
  font-size: 13px;
}
.flow-pending-add__name {
  min-width: 0;
}
</style>

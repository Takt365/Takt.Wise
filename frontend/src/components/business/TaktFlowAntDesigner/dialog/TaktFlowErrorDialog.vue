<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/dialog -->
<!-- 文件名称：TaktFlowErrorDialog.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程设计校验错误列表弹窗（与 AntFlow errorDialog 语义对齐） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-modal
    v-model:open="openProxy"
    :title="t('workflow.designer.page.errordialogtitle')"
    width="640px"
    :footer="null"
    destroy-on-close
    @cancel="() => emit('update:open', false)"
  >
    <a-table
      size="small"
      :columns="columns"
      :data-source="rows"
      :pagination="false"
      row-key="key"
      :scroll="{ y: 360 }"
    />
  </a-modal>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

export interface TaktFlowErrorRow {
  nodeName: string
  message: string
}

const props = defineProps<{
  open: boolean
  rows: TaktFlowErrorRow[]
}>()

const emit = defineEmits<{
  'update:open': [open: boolean]
}>()

const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v)
})

const columns = computed(() => [
  { title: t('workflow.designer.page.errorcolindex'), dataIndex: 'index', width: 64 },
  { title: t('workflow.designer.page.errorcolnode'), dataIndex: 'nodeName', ellipsis: true },
  { title: t('workflow.designer.page.errorcolmessage'), dataIndex: 'message', ellipsis: true }
])

const rows = computed(() =>
  (props.rows ?? []).map((r, i) => ({
    key: `${i}-${r.nodeName}-${r.message}`,
    index: i + 1,
    nodeName: r.nodeName,
    message: r.message
  }))
)
</script>

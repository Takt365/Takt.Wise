<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：EcSubPanels.vue -->
<!-- 功能描述：设变技术部门右栏子表面板（附件 TaktEcAttachment + 明细 TaktEcDetail） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-sub-panels flex h-full min-h-0 flex-col overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-sub-panels-tabs flex min-h-0 flex-1 flex-col"
    >
      <a-tab-pane
        key="attachment"
        :tab="ai.self()"
        force-render
      >
        <EcAttachmentPanel
          ref="attachmentPanelRef"
          class="h-full min-h-0"
        />
      </a-tab-pane>
      <a-tab-pane
        key="detail"
        :tab="pi.self()"
        force-render
      >
        <EcDetailPanel
          ref="detailPanelRef"
          class="h-full min-h-0"
        />
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变技术部门右栏：附件 + 明细子表 Tab
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import EcAttachmentPanel from './EcAttachmentPanel.vue'
import EcDetailPanel from './EcDetailPanel.vue'
import { useEcAttachmentI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcAttachmentI18n'
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n'

const pi = useEcDetailI18n()
const ai = useEcAttachmentI18n()
/** 当前激活子表 Tab（流程：主表 → 附件 → 明细） */
const activeTab = ref('attachment')
/** 附件子表面板 ref */
const attachmentPanelRef = ref<InstanceType<typeof EcAttachmentPanel> | null>(null)
/** 明细子表面板 ref */
const detailPanelRef = ref<InstanceType<typeof EcDetailPanel> | null>(null)

/** 主表选中变更后刷新两个子表 */
function reload() {
  attachmentPanelRef.value?.reload?.()
  detailPanelRef.value?.reload?.()
}

defineExpose({ reload })
</script>

<style scoped lang="css">
:deep(.ec-sub-panels-tabs .ant-tabs-content-holder) {
  flex: 1;
  min-height: 0;
  overflow: hidden;
}

:deep(.ec-sub-panels-tabs .ant-tabs-content) {
  height: 100%;
}

:deep(.ec-sub-panels-tabs .ant-tabs-tabpane) {
  height: 100%;
  min-height: 0;
}
</style>

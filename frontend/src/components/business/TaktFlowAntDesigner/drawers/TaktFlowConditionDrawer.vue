<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/drawers -->
<!-- 文件名称：TaktFlowConditionDrawer.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：条件分支条件表达式编辑抽屉 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-drawer
    v-model:open="visible"
    :title="t('workflow.designer.page.conditiontitle')"
    width="480"
    :footer-style="{ textAlign: 'right' }"
    @close="handleClose"
  >
    <template v-if="config">
      <a-form layout="vertical">
        <a-form-item :label="t('workflow.designer.page.conditionname')">
          <a-input
            v-model:value="form.nodeName"
            :placeholder="t('workflow.designer.page.conditionnameplaceholder')"
          />
        </a-form-item>
        <a-form-item :label="t('workflow.designer.page.edgecondition')">
          <a-textarea
            v-model:value="form.conditionExpr"
            :placeholder="t('workflow.designer.page.placeholderedgecondition')"
            :rows="3"
          />
        </a-form-item>
      </a-form>
    </template>
    <template #footer>
      <a-button
        style="margin-right: 8px"
        @click="handleClose"
      >
        {{ t('common.page.button.cancel') }}
      </a-button>
      <a-button
        type="primary"
        @click="handleSave"
      >
        {{ t('common.page.button.ok') }}
      </a-button>
    </template>
  </a-drawer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useFlowDrawer } from '../config/useFlowDrawer'
import type { FlowTreeNode } from '../config/taktFlowTree'

const { t } = useI18n()
const { state, save: saveDrawer, close } = useFlowDrawer()

const visible = computed({
  get: () => !!(state.visible && state.type === 'condition'),
  set: (v) => { if (!v) close() }
})
const config = ref<FlowTreeNode | null>(null)
const form = ref({ nodeName: '', conditionExpr: '' })

function conditionListFromExpr(expr: string): { showName?: string; optType?: string; zdy1?: string }[] {
  if (!expr?.trim()) return []
  const m = expr.trim().match(/^(\S+)\s*(<|>|<=|>=|==)\s*(.+)$/)
  if (!m) return []
  const [, key, op, val] = m
  const optMap: Record<string, string> = { '<': '1', '>': '2', '>=': '4', '<=': '5', '==': '3' }
  const result: { showName?: string; optType?: string; zdy1?: string } = {}
  const showName = key?.replace(/_/g, ' ')
  if (showName) result.showName = showName
  result.optType = optMap[op ?? ''] ?? '3'
  const zdy1 = val?.trim()
  if (zdy1) result.zdy1 = zdy1
  return [result]
}

watch(
  () => state.visible && state.type === 'condition',
  (v) => {
    if (v && state.config) {
      config.value = state.config
      const list = state.config.conditionList ?? []
      const first = list[0]
      const expr = first?.zdy1 != null && first?.showName
        ? `${first.showName.replace(/\s/g, '_')} ${(first.optType ?? '') === '1' ? '<' : (first.optType ?? '') === '2' ? '>' : (first.optType ?? '') === '4' ? '>=' : (first.optType ?? '') === '5' ? '<=' : '=='} ${first.zdy1}`
        : ''
      form.value = { nodeName: state.config.nodeName ?? '', conditionExpr: expr }
    } else {
      config.value = null
    }
  },
  { immediate: true }
)

function handleSave() {
  if (!config.value) return
  const conditionList = conditionListFromExpr(form.value.conditionExpr)
  const updated: FlowTreeNode = {
    ...config.value,
    nodeName: form.value.nodeName || config.value.nodeName,
    conditionList: conditionList.length ? conditionList : (config.value.conditionList ?? [])
  }
  saveDrawer(updated)
}

function handleClose() {
  close()
}
</script>

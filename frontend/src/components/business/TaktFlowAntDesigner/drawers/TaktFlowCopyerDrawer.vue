<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/drawers -->
<!-- 文件名称：TaktFlowCopyerDrawer.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：抄送人节点属性编辑抽屉 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-drawer
    v-model:open="visible"
    :title="t('workflow.designer.page.copyer')"
    width="480"
    :footer-style="{ textAlign: 'right' }"
    @close="handleClose"
  >
    <template v-if="config && config.nodeType === 6">
      <a-form layout="vertical">
        <a-form-item :label="t('workflow.designer.page.propcopyusers')">
          <div class="takt-flow-drawer-pick-row">
            <a-select
              v-model:value="form.copyUserIds"
              mode="multiple"
              show-search
              :filter-option="filterOption"
              :options="userOptions"
              :placeholder="t('workflow.designer.page.placeholderselectcopyusers')"
              class="takt-flow-drawer-pick-row__select"
              :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            />
            <a-button
              type="primary"
              ghost
              @click="selectUserOpen = true"
            >
              {{ t('workflow.designer.page.openselectuserlist') }}
            </a-button>
          </div>
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
    <TaktFlowSelectUserDialog
      v-model:open="selectUserOpen"
      :selected-ids="form.copyUserIds"
      @confirm="onSelectUserConfirm"
    />
  </a-drawer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useFlowDrawer } from '../config/useFlowDrawer'
import { getUserOptions } from '@/api/identity/user.api'
import type { TaktSelectOption } from '@/types/common.types'
import type { FlowTreeNode } from '../config/taktFlowTree'

const { t } = useI18n()
const { state, save: saveDrawer, close } = useFlowDrawer()

const visible = computed({
  get: () => !!(state.visible && state.type === 'copyer'),
  set: (v) => { if (!v) close() }
})
const config = ref<FlowTreeNode | null>(null)
const form = ref({ copyUserIds: [] as string[] })
const userOptions = ref<TaktSelectOption[]>([])
const selectUserOpen = ref(false)

function onSelectUserConfirm(payload: { ids: string[]; items: { targetId: string; name: string }[] }) {
  form.value.copyUserIds = payload.ids
  const list = userOptions.value
  for (const it of payload.items) {
    if (!list.some((o) => String(o.dictValue) === it.targetId)) {
      list.push({ dictLabel: it.name, dictValue: it.targetId, sortOrder: 0 })
    }
  }
}

function filterOption(
  input: string,
  option?: { label?: string; dictLabel?: string }
) {
  const label = option?.dictLabel ?? option?.label ?? ''
  return String(label).toLowerCase().includes((input ?? '').toLowerCase())
}

watch(
  () => state.visible && state.type === 'copyer',
  (v) => {
    if (v && state.config) {
      config.value = state.config
      form.value.copyUserIds = (state.config.nodeApproveList ?? []).map((x) => x.targetId)
    } else {
      config.value = null
    }
  },
  { immediate: true }
)

onMounted(() => {
  getUserOptions().then((list) => { userOptions.value = list ?? [] })
})

function handleSave() {
  if (!config.value) return
  const list = (form.value.copyUserIds ?? []).map((id) => {
    const o = userOptions.value.find((u) => String(u.dictValue) === id)
    return { targetId: id, name: o?.dictLabel ?? id }
  })
  const updated: FlowTreeNode = { ...config.value, nodeApproveList: list }
  saveDrawer(updated)
}

function handleClose() {
  close()
}
</script>

<style scoped>
.takt-flow-drawer-pick-row {
  display: flex;
  gap: 8px;
  align-items: flex-start;
  width: 100%;
}
.takt-flow-drawer-pick-row__select {
  flex: 1;
  min-width: 0;
}
</style>

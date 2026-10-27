<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/drawers -->
<!-- 文件名称：TaktFlowApproverDrawer.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：审批人节点属性编辑抽屉 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-drawer
    v-model:open="visible"
    :title="t('workflow.designer.page.approver')"
    width="480"
    :footer-style="{ textAlign: 'right' }"
    @close="handleClose"
  >
    <template v-if="config && config.nodeType === 4">
      <a-form layout="vertical">
        <a-form-item :label="t('workflow.designer.page.propassigneetype')">
          <a-radio-group v-model:value="form.setType">
            <a-radio :value="1">
              {{ t('workflow.designer.page.assigneeassignee') }}
            </a-radio>
            <a-radio :value="2">
              {{ t('workflow.designer.page.directorleveldirect') }}
            </a-radio>
            <a-radio :value="3">
              {{ t('workflow.designer.page.assigneerole') }}
            </a-radio>
            <a-radio :value="4">
              {{ t('workflow.designer.page.assigneedept') }}
            </a-radio>
            <a-radio :value="5">
              {{ t('workflow.designer.page.assigneeselfselect') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
        <a-form-item
          v-if="form.setType === 1"
          :label="t('workflow.designer.page.propassignees')"
        >
          <div class="takt-flow-drawer-pick-row">
            <a-select
              v-model:value="form.assigneeUserIds"
              mode="multiple"
              show-search
              :filter-option="filterOption"
              :options="userOptions"
              :placeholder="t('workflow.designer.page.placeholderselectusers')"
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
        <a-form-item
          v-if="form.setType === 3"
          :label="t('workflow.designer.page.proproles')"
        >
          <div class="takt-flow-drawer-pick-row">
            <a-select
              v-model:value="form.roleIds"
              mode="multiple"
              show-search
              :filter-option="filterOption"
              :options="roleOptions"
              :placeholder="t('workflow.designer.page.placeholderselectroles')"
              class="takt-flow-drawer-pick-row__select"
              :field-names="{ label: 'dictLabel', value: 'dictValue' }"
            />
            <a-button
              type="primary"
              ghost
              @click="selectRoleOpen = true"
            >
              {{ t('workflow.designer.page.openselectrolelist') }}
            </a-button>
          </div>
        </a-form-item>
        <a-form-item
          v-if="form.setType === 4"
          :label="t('workflow.designer.page.propdepartments')"
        >
          <TaktTreeSelect
            v-model:value="form.deptIds"
            api-url="TaktDepts/tree-options"
            :lazy="true"
            multiple
            :placeholder="t('workflow.designer.page.placeholderselectdepts')"
            style="width: 100%"
            :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          />
        </a-form-item>
        <a-form-item
          v-if="form.setType === 2"
          :label="t('workflow.designer.page.directorlevel')"
        >
          <a-select
            v-model:value="form.directorLevel"
            style="width: 100%"
          >
            <a-select-option :value="1">
              {{ t('workflow.designer.page.directorleveldirect') }}
            </a-select-option>
            <a-select-option :value="2">
              {{ t('workflow.designer.page.directorlevelsecond') }}
            </a-select-option>
            <a-select-option :value="3">
              {{ t('workflow.designer.page.directorlevelthird') }}
            </a-select-option>
          </a-select>
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
      :selecmed-ids="form.assigneeUserIds"
      @confirm="onSelectUserConfirm"
    />
    <TaktFlowSelectRoleDialog
      v-model:open="selectRoleOpen"
      :selecmed-ids="form.roleIds"
      @confirm="onSelectRoleConfirm"
    />
  </a-drawer>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useFlowDrawer } from '../config/useFlowDrawer'
import { getRoleOptions } from '@/api/identity/role.api'
import { getUserOptions } from '@/api/identity/user.api'
import type { TaktSelectOption } from '@/types/common.types'
import type { FlowTreeNode } from '../config/taktFlowTree'

const { t } = useI18n()
const { state, save: saveDrawer, close } = useFlowDrawer()

const visible = computed({
  get: () => !!(state.visible && state.type === 'approver'),
  set: (v) => { if (!v) close() }
})
const config = ref<FlowTreeNode | null>(null)
const form = ref({
  setType: 1,
  directorLevel: 1,
  signType: 1,
  assigneeUserIds: [] as string[],
  roleIds: [] as string[],
  deptIds: [] as string[],
  nodeApproveList: [] as { targetId: string; name: string }[]
})

const roleOptions = ref<TaktSelectOption[]>([])
const userOptions = ref<TaktSelectOption[]>([])

const selectUserOpen = ref(false)
const selectRoleOpen = ref(false)

function mergeUserOptions(items: { targetId: string; name: string }[]) {
  const list = userOptions.value
  for (const it of items) {
    if (!list.some((o) => String(o.dictValue) === it.targetId)) {
      list.push({ dictLabel: it.name, dictValue: it.targetId, sortOrder: 0 })
    }
  }
}

function mergeRoleOptions(items: { targetId: string; name: string }[]) {
  const list = roleOptions.value
  for (const it of items) {
    if (!list.some((o) => String(o.dictValue) === it.targetId)) {
      list.push({ dictLabel: it.name, dictValue: it.targetId, sortOrder: 0 })
    }
  }
}

function onSelectUserConfirm(payload: { ids: string[]; items: { targetId: string; name: string }[] }) {
  form.value.assigneeUserIds = payload.ids
  mergeUserOptions(payload.items)
}

function onSelectRoleConfirm(payload: { ids: string[]; items: { targetId: string; name: string }[] }) {
  form.value.roleIds = payload.ids
  mergeRoleOptions(payload.items)
}

function filterOption(
  input: string,
  option?: { label?: string; dictLabel?: string }
) {
  const label = option?.dictLabel ?? option?.label ?? ''
  return String(label).toLowerCase().includes((input ?? '').toLowerCase())
}

watch(
  () => state.visible && state.type === 'approver',
  (v) => {
    if (v && state.config) {
      config.value = state.config
      form.value = {
        setType: state.config.setType ?? 1,
        directorLevel: state.config.directorLevel ?? 1,
        signType: state.config.signType ?? 1,
        assigneeUserIds: (state.config.nodeApproveList ?? []).map((x) => x.targetId),
        roleIds: (state.config.nodeApproveList ?? []).map((x) => x.targetId),
        deptIds: [],
        nodeApproveList: state.config.nodeApproveList ? [...state.config.nodeApproveList] : []
      }
      if (form.value.setType === 1) {
        form.value.nodeApproveList = state.config.nodeApproveList ? [...state.config.nodeApproveList] : []
      } else if (form.value.setType === 3) {
        form.value.nodeApproveList = state.config.nodeApproveList ? [...state.config.nodeApproveList] : []
      }
    } else {
      config.value = null
    }
  },
  { immediate: true }
)

onMounted(() => {
  getRoleOptions().then((list) => { roleOptions.value = list ?? [] })
  getUserOptions().then((list) => { userOptions.value = list ?? [] })
})

function buildNodeApproveList(): { targetId: string; name: string }[] {
  const st = form.value.setType
  if (st === 1 && form.value.assigneeUserIds?.length) {
    return form.value.assigneeUserIds.map((id) => {
      const o = userOptions.value.find((u) => String(u.dictValue) === id)
      return { targetId: id, name: o?.dictLabel ?? id }
    })
  }
  if (st === 3 && form.value.roleIds?.length) {
    return form.value.roleIds.map((id) => {
      const o = roleOptions.value.find((r) => String(r.dictValue) === id)
      return { targetId: id, name: o?.dictLabel ?? id }
    })
  }
  if (st === 4 && form.value.deptIds?.length) {
    return form.value.deptIds.map((id) => ({
      targetId: id,
      name: id,
    }))
  }
  return form.value.nodeApproveList ?? []
}

function handleSave() {
  if (!config.value) return
  const updated: FlowTreeNode = {
    ...config.value,
    setType: form.value.setType,
    directorLevel: form.value.directorLevel,
    signType: form.value.signType,
    nodeApproveList: buildNodeApproveList()
  }
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

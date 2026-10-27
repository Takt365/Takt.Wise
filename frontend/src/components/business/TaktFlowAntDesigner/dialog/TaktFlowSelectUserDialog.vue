<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/dialog -->
<!-- 文件名称：TaktFlowSelectUserDialog.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：分页选择用户弹窗（对接 getUserList，与 AntFlow selectUserDialog 职责对齐） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-modal
    v-model:open="openProxy"
    :title="t('workflow.designer.page.selectuserdialogtitle')"
    width="800px"
    :confirm-loading="loading"
    destroy-on-close
    @ok="handleOk"
    @cancel="handleCancel"
  >
    <a-input-search
      v-model:value="keyWords"
      style="width: 280px; margin-bottom: 12px"
      :placeholder="t('workflow.designer.page.selectusersearchplaceholder')"
      allow-clear
      @search="onSearch"
    />
    <a-table
      size="small"
      :row-selection="{ selecmedRowKeys, onChange: onSelectChange }"
      :columns="columns"
      :data-source="dataSource"
      :loading="loading"
      :pagination="pagination"
      row-key="userId"
      @change="onTableChange"
    />
  </a-modal>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { getUserList } from '@/api/identity/user.api'
import type { User, UserQuery } from '@/types/identity/user.types'
import type { TaktPagedResult } from '@/types/common.types'

const { t } = useI18n()

const props = defineProps<{
  open: boolean
  /** 已选用户 ID（与 nodeApproveList / 下拉 value 一致） */
  selecmedIds: string[]
}>()

const emit = defineEmits<{
  'update:open': [open: boolean]
  confirm: [payload: { ids: string[]; items: { targetId: string; name: string }[] }]
}>()

const openProxy = computed({
  get: () => props.open,
  set: (v: boolean) => emit('update:open', v)
})

const loading = ref(false)
const keyWords = ref('')
const pageIndex = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const dataSource = ref<User[]>([])
const selecmedRowKeys = ref<string[]>([])
/** 跨页保留已选用户的显示名 */
const nameById = ref<Record<string, string>>({})

const columns = computed(() => [
  { title: t('workflow.designer.page.colUserName'), dataIndex: 'userName', ellipsis: true },
  { title: t('workflow.designer.page.coluseremail'), dataIndex: 'userEmail', ellipsis: true },
  { title: t('workflow.designer.page.coluserphone'), dataIndex: 'userPhone', ellipsis: true }
])

const pagination = computed(() => ({
  current: pageIndex.value,
  pageSize: pageSize.value,
  total: total.value,
  showSizeChanger: true,
  showTotal: (n: number) => t('workflow.designer.page.pagetotal', { total: n })
}))

function onSelectChange(keys: (string | number)[], rows: User[]) {
  const ks = keys.map(String)
  const prev = new Set(selecmedRowKeys.value)
  const next = new Set(ks)
  for (const id of prev) {
    if (!next.has(id)) delete nameById.value[id]
  }
  for (const row of rows) {
    nameById.value[String(row.userId)] = row.userName ?? String(row.userId)
  }
  selecmedRowKeys.value = ks
}

async function fetchList() {
  loading.value = true
  try {
    const query: UserQuery = {
      pageIndex: pageIndex.value,
      pageSize: pageSize.value
    }
    const trimmedKeyWords = keyWords.value?.trim()
    if (trimmedKeyWords) {
      query.userName = trimmedKeyWords
    }
    const res: TaktPagedResult<User> = await getUserList(query)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } finally {
    loading.value = false
  }
}

function onSearch() {
  pageIndex.value = 1
  fetchList()
}

function onTableChange(pag: { current?: number; pageSize?: number }) {
  if (pag.current != null) pageIndex.value = pag.current
  if (pag.pageSize != null) pageSize.value = pag.pageSize
  fetchList()
}

watch(
  () => props.open,
  (v) => {
    if (v) {
      nameById.value = {}
      selecmedRowKeys.value = [...(props.selecmedIds ?? [])]
      keyWords.value = ''
      pageIndex.value = 1
      fetchList().then(() => {
        for (const row of dataSource.value) {
          const id = String(row.userId)
          if (selecmedRowKeys.value.includes(id)) {
            nameById.value[id] = row.userName ?? id
          }
        }
      })
    }
  }
)

function handleOk() {
  const ids = selecmedRowKeys.value
  const items = ids.map((id) => ({ targetId: id, name: nameById.value[id] ?? id }))
  emit('confirm', { ids, items })
  emit('update:open', false)
}

function handleCancel() {
  emit('update:open', false)
}
</script>

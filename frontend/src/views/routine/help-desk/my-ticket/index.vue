<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/my-ticket -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台「我的工单」门户页，仅展示当前用户提交的工单 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <div class="mb-2">
      <a-button
        v-permission="'routine:help:desk:my:ticket:list'"
        type="primary"
        class="takt-button-create"
        @click="handleCreate"
      >
        {{ t('common.page.button.create') }}
      </a-button>
      <a-button
        v-permission="'routine:help:desk:my:ticket:list'"
        class="ml-2 takt-button-query"
        :disabled="!selecmedRow"
        @click="handleOpenWorkflow"
      >
        {{ t('routine.help-desk.ticket.page.workflow.title') }}
      </a-button>
    </div>
    <TaktSingleTable
      :columns="columns"
      entity-scope="company"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTicketId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ticketStatus'">
          <TaktDictTag
            :value="record.ticketStatus"
            dict-type="sys_ticket_status"
          />
        </template>
        <template v-else-if="column.key === 'priority'">
          <TaktDictTag
            :value="record.priority"
            dict-type="sys_priority_level"
          />
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    <TaktModal
      v-model:open="formVisible"
      :title="t('common.dialog.title.create', { entity: t('entity.ticket._self') })"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
    >
      <a-form ref="formRef" :model="formState" :rules="rules" layout="horizontal" label-align="right">
        <a-form-item :label="t('entity.ticket.title')" name="title">
          <a-input v-model:value="formState.title" size="small" allow-clear />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.content')" name="content">
          <a-textarea v-model:value="formState.content" :rows="3" size="small" />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.urgency')" name="urgency">
          <TaktSelect
            v-model:value="formState.urgency"
            dict-type="sys_urgency_level"
            size="small"
          />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.impact')" name="impact">
          <TaktSelect
            v-model:value="formState.impact"
            dict-type="sys_impact_level"
            size="small"
          />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.priority')" name="priority">
          <TaktSelect
            v-model:value="computedPriority"
            dict-type="sys_priority_level"
            size="small"
            disabled
          />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.categorycode')" name="categoryCode">
          <a-input v-model:value="formState.categoryCode" size="small" allow-clear />
        </a-form-item>
        <a-form-item :label="t('entity.ticket.assetcode')" name="assetCode">
          <TaktSelect
            v-model:value="formState.assetCode"
            :options="assetOptions"
            :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ticket.assetcode') })"
            size="small"
            allow-clear
          />
        </a-form-item>
      </a-form>
    </TaktModal>
    <TicketWorkflowDrawer
      v-model:open="workflowVisible"
      :ticket-id="workflowTicketId"
      workflow-permission="routine:help:desk:my:ticket:list"
      :show-internal-note="false"
      portal-mode
      @changed="loadData"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 服务台我的工单门户页
 * @module views/routine/help-desk/my-ticket
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import type { Rule } from 'ant-design-vue/es/form'
import { getMyTicketList, submitTicket } from '@/api/routine/help-desk/ticket.api'
import { getAssetList } from '@/api/accounting/financial/asset.api'
import type { Ticket, TicketSubmit } from '@/types/routine/help-desk/ticket.types'
import { resolveTicketPriority } from '@/utils/taktTicketPriority'
import TicketWorkflowDrawer from '../ticket/components/TicketWorkflowDrawer.vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 快捷查询占位 */
const searchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', { keyword: t('entity.ticket._self') })
)
/** 查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 列表数据 */
const dataSource = ref<Ticket[]>([])
/** 当前页 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 总数 */
const total = ref(0)
/** 选中行 */
const selecmedRow = ref<Ticket | null>(null)
/** 选中 keys */
const selecmedRowKeys = ref<(string | number)[]>([])
/** 新建弹窗 */
const formVisible = ref(false)
/** 表单 loading */
const formLoading = ref(false)
/** 工作流抽屉 */
const workflowVisible = ref(false)
/** 工作流工单 ID */
const workflowTicketId = ref<string | null>(null)
/** 新建表单 */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 新建表单模型 */
const formState = ref<TicketSubmit>({
  title: '',
  content: '',
  urgency: 3,
  impact: 3,
  categoryCode: '',
  assetCode: '',
})
/** 由紧急度×影响范围自动计算的优先级预览 */
const computedPriority = computed(() =>
  resolveTicketPriority(formState.value.urgency, formState.value.impact),
)
/** 资产号码下拉选项 */
const assetOptions = ref<Array<{ label: string; value: string }>>([])
/** 新建表单规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  title: [{ required: true, message: t('common.page.form.placeholder.required', { field: t('entity.ticket.title') }), trigger: 'blur' }],
}))
/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  { title: t('entity.ticket.no'), dataIndex: 'ticketCode', key: 'ticketCode', width: 140, ellipsis: true },
  { title: t('entity.ticket.title'), dataIndex: 'title', key: 'title', width: 200, ellipsis: true },
  { title: t('entity.ticket.status'), dataIndex: 'ticketStatus', key: 'ticketStatus', width: 120 },
  { title: t('entity.ticket.priority'), dataIndex: 'priority', key: 'priority', width: 100 },
  { title: t('entity.ticket.categorycode'), dataIndex: 'categoryCode', key: 'categoryCode', width: 120, ellipsis: true },
  { title: t('entity.ticket.assetcode'), dataIndex: 'assetCode', key: 'assetCode', width: 120, ellipsis: true },
  { title: t('common.page.entity.createdat'), dataIndex: 'createdAt', key: 'createdAt', width: 160, ellipsis: true }])
/** row-key */
const getTicketId = (record: Ticket): string => record.ticketId ?? ''
/** 行选择 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Ticket[]) => {
    selecmedRowKeys.value = keys
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
}))
/** 行点击 */
const onClickRow = (record: Ticket) => ({
  onClick: () => {
    const key = getTicketId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value = [key]
    }
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? record : null
  },
})
/** 加载我的工单列表 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: Record<string, unknown> = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
    }
    if (kw) {
      params.keyWords = kw
    }
    const res = await getMyTicketList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}
/** 重置 */
function handleReset() {
  queryKeyword.value = ''
  currentPage.value = 1
  loadData()
}

/** 外置分页：翻页 */
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}

/** 外置分页：改每页条数时回到第 1 页 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = 1
  pageSize.value = size
  loadData()
}

/** 新建工单 */
function handleCreate() {
  formState.value = { title: '', content: '', urgency: 3, impact: 3, categoryCode: '', assetCode: '' }
  formVisible.value = true
}
/** 加载资产选项 */
async function loadAssetOptions() {
  try {
    const res = await getAssetList({ pageIndex: 1, pageSize: 500 })
    assetOptions.value = (res.data ?? [])
      .filter((item) => item.assetCode)
      .map((item) => ({
        label: item.assetName ? `${item.assetName} (${item.assetCode})` : (item.assetCode ?? ''),
        value: item.assetCode ?? '',
      }))
  } catch {
    assetOptions.value = []
  }
}
/** 提交新建 */
async function handleFormSubmit() {
  try {
    await formRef.value?.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    await submitTicket(formState.value)
    message.success(t('common.feedback.created', { target: t('entity.ticket._self') }))
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}
/** 打开工作流抽屉 */
function handleOpenWorkflow() {
  if (!selecmedRow.value) {
    return
  }
  workflowTicketId.value = getTicketId(selecmedRow.value)
  workflowVisible.value = true
}

onMounted(() => {
  loadData()
  loadAssetOptions()
})
</script>

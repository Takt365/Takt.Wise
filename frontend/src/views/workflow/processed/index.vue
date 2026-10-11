<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/processed -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：已办列表页面，包含我已处理的流程查询与导出（可与我的流程共用接口，后续可扩展“我参与办结的”接口） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-processed">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-refresh="true"
      :show-export="false"
      :show-fullscreen="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :refresh-loading="loading"
      @refresh="handleRefresh"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getInstanceId"
      :large-screen-column-count="6"
      :small-screen-column-count="4"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'instanceStatus'">
          <TaktDictTag
            :value="(record as FlowInstanceListItem).instanceStatus"
            dict-type="sys_flow_status"
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
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('entity.flowinstance._self') })"
      width="640px"
      :footer="null"
      :cancel-text="t('common.page.button.cancel')"
      @cancel="detailVisible = false"
    >
      <FlowInstanceDetailForm
        :detail="detail"
        @refresh="reloadInstanceDetail"
      />
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.flowinstance.instancecode')">
        <a-input v-model:value="advancedQueryForm.instanceCode" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.processkey')">
        <a-input v-model:value="advancedQueryForm.processKey" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.processname')">
        <a-input v-model:value="advancedQueryForm.processName" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.processtitle')">
        <a-input v-model:value="advancedQueryForm.processTitle" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.currentactivityname')">
        <a-input v-model:value="advancedQueryForm.taskName" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.startUserName')">
        <a-input v-model:value="advancedQueryForm.startUserName" allow-clear />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.starttime')">
        <a-range-picker
          v-model:value="startTimeRange"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 已办列表页：我已处理的流程实例列表、分页、详情、导出。
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEyeLine } from '@remixicon/vue'
import {
  getFlowEngineProcessedList,
  getFlowEngineProcessedById
} from '@/api/workflow/flowEngine.api'
import FlowInstanceDetailForm from '@/views/workflow/processed/components/FlowInstanceDetailForm.vue'
import type { FlowInstanceListItem, FlowInstanceDetail, FlowTodoQuery } from '@/types/workflow/flowEngine.types'
import { useWorkflowSignalRRefresh, WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'

const { t } = useI18n()
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.flowinstance._self') })
)

/** 与 `TaktSingleTable` 的 `@resize-column` 第二参数一致（`ResizableColumn`） */
type TaktResizeColumn = { width?: string | number } & Record<string, unknown>

const loading = ref(false)
const queryKeyword = ref('')
const advancedQueryVisible = ref(false)
const columnSettingVisible = ref(false)
const advancedQueryForm = ref({
  instanceCode: '',
  processKey: '',
  processName: '',
  processTitle: '',
  taskName: '',
  startUserName: ''
})
/** 发起时间范围（高级查询） */
const startTimeRange = ref<[string, string] | undefined>(undefined)
const dataSource = ref<FlowInstanceListItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const detailVisible = ref(false)
const detail = ref<FlowInstanceDetail | null>(null)
/** 列可见键（未启用列设置时保持空数组，展示全部列） */
const visibleColumnKeys = ref<string[]>([])

const columns = computed<TableColumnsType>(() => [
  { title: t('entity.flowinstance.instancecode'), dataIndex: 'instanceCode', key: 'instanceCode', width: 200, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processname'), dataIndex: 'processName', key: 'processName', width: 120, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processtitle'), dataIndex: 'processTitle', key: 'processTitle', ellipsis: true, resizable: true },
  { title: t('entity.flowinstance.instancestatus'), dataIndex: 'instanceStatus', key: 'instanceStatus', width: 90 },
  { title: t('entity.flowinstance.startUserName'), dataIndex: 'startUserName', key: 'startUserName', width: 90, resizable: true },
  { title: t('entity.flowinstance.starttime'), dataIndex: 'startTime', key: 'startTime', width: 170, resizable: true },
  CreateActionColumn<FlowInstanceListItem>({
    width: 80,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'workflow:processed:query',
        onClick: (record) => showDetail(record)
      }
    ]
  })
])

/** 表格列 key（列宽拖拽） */
function getColumnKey(col: TaktResizeColumn): string {
  const key = col.key ?? col.dataIndex ?? col.title
  return key != null ? String(key) : ''
}

/** 组装列表查询 DTO（关键词 + 高级查询，对齐 FlowTodoQuery） */
function buildFlowTodoQuery(): FlowTodoQuery {
  const query: FlowTodoQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value
  }
  const kw = queryKeyword.value.trim()
  if (kw) query.keyWords = kw
  const f = advancedQueryForm.value
  if (f.instanceCode.trim()) query.instanceCode = f.instanceCode.trim()
  if (f.processKey.trim()) query.processKey = f.processKey.trim()
  if (f.processName.trim()) query.processName = f.processName.trim()
  if (f.processTitle.trim()) query.processTitle = f.processTitle.trim()
  if (f.taskName.trim()) query.taskName = f.taskName.trim()
  if (f.startUserName.trim()) query.startUserName = f.startUserName.trim()
  if (startTimeRange.value?.[0]) query.startTimeStart = startTimeRange.value[0]
  if (startTimeRange.value?.[1]) query.startTimeEnd = startTimeRange.value[1]
  return query
}

/** 将 a-table bodyCell 的 record 断言为 FlowInstanceListItem */
function asFlowInstance(r: Record<string, unknown>): FlowInstanceListItem {
  return r as unknown as FlowInstanceListItem
}

/** 实例行 key：取 flowInstanceId 字符串 */
function getInstanceId(record: unknown): string {
  if (!record || typeof record !== 'object' || !('flowInstanceId' in record)) return ''
  const flowInstanceId = (record as { flowInstanceId?: unknown }).flowInstanceId
  return flowInstanceId != null ? String(flowInstanceId) : ''
}

/** 拉取已办列表（分页），结果写入 dataSource 与 total */
async function loadList() {
  loading.value = true
  try {
    const res = await getFlowEngineProcessedList(buildFlowTodoQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    loading.value = false
  }
}

/** 查询：页码置 1 并重新拉取 */
function handleSearch() {
  currentPage.value = 1
  loadList()
}

/** 重置关键词、高级查询、页码并重新拉取 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
    instanceCode: '',
    processKey: '',
    processName: '',
    processTitle: '',
    taskName: '',
    startUserName: ''
  }
  startTimeRange.value = undefined
  currentPage.value = 1
  loadList()
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadList()
  advancedQueryVisible.value = false
}

/** 高级查询重置 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
    instanceCode: '',
    processKey: '',
    processName: '',
    processTitle: '',
    taskName: '',
    startUserName: ''
  }
  startTimeRange.value = undefined
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置勾选变化 */
function handleColumnKeysChange(keys: (string | number)[]) {
  visibleColumnKeys.value = keys.map((k) => String(k))
}

/** 列设置重置 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadList()
}

/** 列宽拖拽 */
function handleResizeColumn(w: number, col: TaktResizeColumn) {
  const column = columns.value.find((c) => getColumnKey(c as TaktResizeColumn) === getColumnKey(col))
  if (column && 'width' in column) {
    ;(column as { width?: number }).width = w
  }
}

/** 表格变化占位（分页由 TaktPagination 处理） */
function handleTableChange(_pag: { current?: number; pageSize?: number }) {
  // 分页由 TaktPagination 处理
}

/** 分页变化时更新并拉取 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadList()
}

/** 每页条数变化时更新并拉取 */
function handlePaginationSizeChange(current: number, size: number) {
  currentPage.value = current
  pageSize.value = size
  loadList()
}

/** 拉取实例详情并打开详情弹窗 */
async function showDetail(record: FlowInstanceListItem) {
  try {
    detail.value = await getFlowEngineProcessedById(record.flowInstanceId)
    detailVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 加签/减签后刷新当前详情（保持弹窗打开） */
async function reloadInstanceDetail() {
  if (!detail.value?.flowInstanceId) return
  try {
    detail.value = await getFlowEngineProcessedById(detail.value.flowInstanceId)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

onMounted(() => loadList())
useWorkflowSignalRRefresh(loadList, WORKFLOW_TABLE_NAMES.processed)
</script>

<style scoped lang="css">
.workflow-processed {
  padding: 16px;
}
.history-item {
  font-size: 12px;
  margin-bottom: 4px;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/instance -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程实例管理页面，包含全部实例列表、查询、导出、详情、更新与删除 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-instance">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <TaktToolsBar
      update-permission="workflow:instance:update"
      delete-permission="workflow:instance:delete"
      export-permission="workflow:instance:export"
      :show-create="false"
      :show-update="true"
      :show-delete="true"
      :show-refresh="true"
      :show-export="true"
      :show-fullscreen="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :update-disabled="!selecmedRow || selecmedRows.length !== 1"
      :delete-disabled="selecmedRows.length === 0"
      :refresh-loading="loading"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
      @export="handleExport"
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
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'instanceStatus'">
          <TaktDictTag
            :value="(record as FlowInstanceTableRow).instanceStatus"
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
      <InstanceForm
        :detail="detail"
        @refresh="reloadInstanceDetail"
      />
    </TaktModal>

    <!-- 挂起 -->
    <TaktModal
      v-model:open="suspendVisible"
      :title="t('common.dialog.title.suspend')"
      @ok="submitSuspend"
      @cancel="currentSuspendInstance = null; suspendReason = ''"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('workflow.instance.page.suspend.reason.label')">
          <a-textarea
            v-model:value="suspendReason"
            :rows="3"
            :placeholder="t('workflow.instance.page.suspend.reason.placeholder')"
          />
        </a-form-item>
      </a-form>
    </TaktModal>
    <!-- 终止 -->
    <TaktModal
      v-model:open="terminateVisible"
      :title="t('common.dialog.title.terminate')"
      @ok="submitTerminate"
      @cancel="currentTerminateInstance = null; terminateReason = ''"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('entity.flowinstance.deletereason')">
          <a-textarea
            v-model:value="terminateReason"
            :rows="3"
            :placeholder="t('workflow.instance.page.terminate.reason.placeholder')"
          />
        </a-form-item>
      </a-form>
    </TaktModal>
    <!-- 编辑实例（流程标题与表单数据） -->
    <TaktModal
      v-model:open="updateVisible"
      :title="t('common.dialog.title.edit', { entity: t('entity.flowinstance._self') })"
      :confirm-loading="updateLoading"
      @ok="handleUpdateSubmit"
      @cancel="updateVisible = false"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('entity.flowinstance.processtitle')">
          <a-input
            v-model:value="updateProcessTitle"
            :placeholder="t('common.page.form.placeholder.required', { field: t('entity.flowinstance.processtitle') })"
          />
        </a-form-item>
        <a-form-item :label="t('entity.flowinstance.frmdata')">
          <a-textarea
            v-model:value="updateFrmData"
            :rows="6"
            :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.flowinstance.frmdata') })"
          />
        </a-form-item>
      </a-form>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.flowinstance.instancecode')">
        <a-input v-model:value="advancedQueryForm.instanceCode" />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.processkey')">
        <a-input v-model:value="advancedQueryForm.processKey" />
      </a-form-item>
      <a-form-item :label="t('entity.flowinstance.instancestatus')">
        <TaktSelect
          v-model="advancedQueryForm.instanceStatus"
          dict-type="sys_flow_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowinstance.instancestatus') })"
          allow-clear
          :show-search="true"
        />
      </a-form-item>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'id'"
      :action-column-key="'action'"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 流程实例列表页：全部实例查询、分页、详情、更新、撤回、挂起、恢复、终止、删除、导出。
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import {
  getFlowEngineDetailById,
  withdrawFlowEngineInstance,
  suspendFlowEngineInstance,
  resumeFlowEngineInstance,
  terminateFlowEngineInstance
} from '@/api/workflow/flowEngine.api'
import {
  getFlowInstanceList,
  updateFlowInstance,
  deleteFlowInstanceById,
  deleteFlowInstanceBatch,
  exportFlowInstance
} from '@/api/workflow/flowInstance.api'
import { useUserStore } from '@/stores/identity/useUserStore'
import InstanceForm from './components/InstanceForm.vue'
import type { FlowInstance, FlowInstanceQuery, FlowInstanceEditPayload } from '@/types/workflow/flowInstance.types'
import type { FlowInstanceDetail } from '@/types/workflow/flowEngine.types'
import { useWorkflowSignalRRefresh, WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh'

type FlowInstanceTableRow = FlowInstance & { instanceId: string; currentNodeName?: string }
import { RiEyeLine, RiArrowGoBackLine, RiEditLine, RiDeleteBinLine, RiPauseLine, RiPlayLine, RiStopLine } from '@remixicon/vue'

const { t } = useI18n()
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.flowinstance._self') })
)
const loading = ref(false)
const queryKeyword = ref('')
const queryStatus = ref<number | undefined>(undefined)
const dataSource = ref<FlowInstanceTableRow[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selecmedRow = ref<FlowInstanceTableRow | null>(null)
const selecmedRows = ref<FlowInstanceTableRow[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const detailVisible = ref(false)
const detail = ref<FlowInstanceDetail | null>(null)
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref<{ instanceCode: string; processKey: string; instanceStatus: number | undefined }>({
  instanceCode: '',
  processKey: '',
  instanceStatus: undefined
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const updateVisible = ref(false)
const updateProcessTitle = ref('')
const updateFrmData = ref('')
const updateLoading = ref(false)
const currentEditInstance = ref<FlowInstanceTableRow | null>(null)
const suspendVisible = ref(false)
const suspendReason = ref('')
const currentSuspendInstance = ref<FlowInstanceTableRow | null>(null)
const terminateVisible = ref(false)
const terminateReason = ref('')
const currentTerminateInstance = ref<FlowInstanceTableRow | null>(null)

type FlowInstanceTableRowColumn = {
  key?: string | number
  dataIndex?: string | number
  title?: string | number
  width?: string | number
}

type TableSorterInfo = {
  field?: string
  order?: string
}

function getErrorMessage(error: unknown, fallback: string): string {
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const message = (error as { message?: unknown }).message
    if (typeof message === 'string' && message.trim()) return message
  }
  return fallback
}

function getColumnKey(column: FlowInstanceTableRowColumn): string {
  const key = column.key ?? column.dataIndex ?? column.title
  return key != null ? String(key) : ''
}

function getSorterInfo(sorter: unknown): TableSorterInfo {
  if (typeof sorter !== 'object' || sorter === null) return {}
  const sorterObj = sorter as { field?: unknown; order?: unknown }
  const result: TableSorterInfo = {}
  if (typeof sorterObj.field === 'string') result.field = sorterObj.field
  if (typeof sorterObj.order === 'string') result.order = sorterObj.order
  return result
}

const userStore = useUserStore()
const currentUserId = computed(() => userStore.userInfo?.userId ?? '')

const getInstanceId = (record: unknown): string => {
  if (!record || typeof record !== 'object') return ''
  const row = record as { flowInstanceId?: unknown; instanceId?: unknown }
  const id = row.flowInstanceId ?? row.instanceId
  return id != null ? String(id) : ''
}

/** 表格列：与 @/types/workflow/instance FlowInstanceTableRow 字段一致（列表展示用，不含 frmData 等大字段） */
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'instanceId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left'
  },
  {
    title: t('entity.flowinstance.instancecode'),
    dataIndex: 'instanceCode',
    key: 'instanceCode',
    width: 160,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowinstance.processkey'),
    dataIndex: 'processKey',
    key: 'processKey',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowinstance.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowinstance.processtitle'),
    dataIndex: 'processTitle',
    key: 'processTitle',
    width: 140,
    ellipsis: true,
    resizable: true
  },
  {
    title: t('entity.flowinstance.instancestatus'),
    dataIndex: 'instanceStatus',
    key: 'instanceStatus',
    width: 90
  },
  {
    title: t('entity.flowinstance.currentactivityname'),
    dataIndex: 'currentNodeName',
    key: 'currentNodeName',
    width: 100,
    ellipsis: true
  },
  {
    title: t('entity.flowinstance.startUserName'),
    dataIndex: 'startUserName',
    key: 'startUserName',
    width: 100
  },
  {
    title: t('entity.flowinstance.starttime'),
    dataIndex: 'startTime',
    key: 'startTime',
    width: 160
  },
  {
    title: t('entity.flowinstance.endtime'),
    dataIndex: 'endTime',
    key: 'endTime',
    width: 160
  },
  CreateActionColumn({
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'workflow:instance:detail',
        onClick: (_record: FlowInstanceTableRow) => showDetail(_record)
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'workflow:instance:update',
        visible: (record: FlowInstanceTableRow) => (record.instanceStatus === 0 || record.instanceStatus === 5) && isStarter(record),
        onClick: (_record: FlowInstanceTableRow) => handleEditInstance(_record)
      },
      {
        key: 'withdraw',
        label: t('common.page.button.withdraw'),
        shape: 'plain',
        icon: RiArrowGoBackLine,
        permission: 'workflow:instance:withdraw',
        visible: (record: FlowInstanceTableRow) => record.instanceStatus === 0 && isStarter(record),
        onClick: (_record: FlowInstanceTableRow) => handleWithdraw(_record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'workflow:instance:delete',
        onClick: (_record: FlowInstanceTableRow) => handleDeleteOne(_record)
      },
      {
        key: 'suspend',
        label: t('common.page.button.suspend'),
        shape: 'plain',
        icon: RiPauseLine,
        permission: 'workflow:instance:suspend',
        visible: (record: FlowInstanceTableRow) => record.instanceStatus === 0,
        onClick: (_record: FlowInstanceTableRow) => openSuspendModal(_record)
      },
      {
        key: 'resume',
        label: t('common.page.button.resume'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'workflow:instance:resume',
        visible: (record: FlowInstanceTableRow) => record.instanceStatus === 3,
        onClick: (_record: FlowInstanceTableRow) => handleResume(_record)
      },
      {
        key: 'terminate',
        label: t('common.page.button.terminate'),
        shape: 'plain',
        icon: RiStopLine,
        permission: 'workflow:instance:terminate',
        visible: (record: FlowInstanceTableRow) => record.instanceStatus === 0 || record.instanceStatus === 3,
        onClick: (_record: FlowInstanceTableRow) => openTerminateModal(_record)
      }
    ]
  })
])


const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: FlowInstanceTableRow[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: FlowInstanceTableRow, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getInstanceId(selecmedRow.value) === getInstanceId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: FlowInstanceTableRow[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: FlowInstanceTableRow) => ({
  onClick: () => {
    const key = getInstanceId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) selecmedRowKeys.value.splice(index, 1)
    else selecmedRowKeys.value.push(key)
    selecmedRows.value = dataSource.value.filter(item => selecmedRowKeys.value.includes(getInstanceId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
  }
})

/** 判断当前用户是否为该实例发起人 */
function isStarter(r: FlowInstanceTableRow): boolean {
  return String(r.startUserId) === String(currentUserId.value)
}

/** 拉取流程实例列表（分页），结果写入 dataSource 与 total */
async function loadData() {
  try {
    loading.value = true
    const query: FlowInstanceQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    const processKey = queryKeyword.value || advancedQueryForm.value.processKey
    const instanceCode = queryKeyword.value || advancedQueryForm.value.instanceCode
    if (processKey) query.processKey = processKey
    if (instanceCode) query.instanceCode = instanceCode
    if (advancedQueryForm.value.instanceStatus != null) query.instanceStatus = advancedQueryForm.value.instanceStatus
    const res = await getFlowInstanceList(query)
    dataSource.value = (res.data ?? []).map((row) => ({
      ...row,
      instanceId: row.flowInstanceId,
      currentNodeName: row.currentActivityName
    }))
    total.value = res.total ?? 0
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换与工作流 SignalR 推送时自动重载列表 */
useWorkflowSignalRRefresh(loadData, WORKFLOW_TABLE_NAMES.instance)
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置关键词、状态、高级查询并重新拉取 */
function handleReset() {
  queryKeyword.value = ''
  queryStatus.value = undefined
  advancedQueryForm.value = { instanceCode: '', processKey: '', instanceStatus: undefined }
  currentPage.value = 1
  loadData()
}

/** 打开高级查询弹窗 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：应用条件、拉取并关闭弹窗 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadData()
  advancedQueryVisible.value = false
}

/** 高级查询重置：清空高级查询表单 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = { instanceCode: '', processKey: '', instanceStatus: undefined }
}

/** 打开列设置弹窗 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置勾选变化时同步 visibleColumnKeys */
function handleColumnKeysChange(keys: (string | number)[]) {
  visibleColumnKeys.value = keys.map(k => String(k))
}

/** 列设置重置：清空可见列 key */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新：重新拉取列表 */
function handleRefresh() {
  loadData()
}

/** 导出流程实例为 Excel 并触发下载 */
async function handleExport() {
  try {
    loading.value = true
    const query: FlowInstanceQuery = {
      pageIndex: 1,
      pageSize: 99999
    }
    const processKey = queryKeyword.value || advancedQueryForm.value.processKey
    const instanceCode = queryKeyword.value || advancedQueryForm.value.instanceCode
    if (processKey) query.processKey = processKey
    if (instanceCode) query.instanceCode = instanceCode
    if (advancedQueryForm.value.instanceStatus != null) query.instanceStatus = advancedQueryForm.value.instanceStatus
    const blob = await exportFlowInstance(query)
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fileName = `${t('entity.flowinstance._self')}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}.xlsx`
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success'))
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.export.failed')))
  } finally {
    loading.value = false
  }
}

/** 表格变化（排序等）占位 */
function handleTableChange(_pagination: unknown, _filters: unknown, sorter: unknown) {
  const sorterInfo = getSorterInfo(sorter)
  if (sorterInfo.order) {
    // 如需服务端排序可在此处理
  }
}

/** 分页变化时更新并拉取 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 每页条数变化：重置到第 1 页并拉取 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = 1
  pageSize.value = size
  loadData()
}

/** 列宽拖拽后更新对应列的 width */
function handleResizeColumn(w: number, col: FlowInstanceTableRowColumn) {
  const column = columns.value.find((c) => getColumnKey(c as FlowInstanceTableRowColumn) === getColumnKey(col))
  if (column) (column as FlowInstanceTableRowColumn).width = w
}

/** 拉取实例详情并打开详情弹窗 */
async function showDetail(record: FlowInstanceTableRow) {
  try {
    detail.value = await getFlowEngineDetailById(record.instanceId)
    detailVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 加签/减签后刷新当前详情（保持弹窗打开） */
async function reloadInstanceDetail() {
  if (!detail.value?.flowInstanceId) return
  try {
    detail.value = await getFlowEngineDetailById(detail.value.flowInstanceId)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 打开挂起弹窗并设置当前实例 */
function openSuspendModal(record: FlowInstanceTableRow) {
  currentSuspendInstance.value = record
  suspendReason.value = ''
  suspendVisible.value = true
}

/** 提交挂起：调用 suspend 接口后关闭弹窗并刷新列表 */
async function submitSuspend() {
  if (!currentSuspendInstance.value) return
  try {
    loading.value = true
    const payload: { flowInstanceId: string; reason?: string } = { flowInstanceId: currentSuspendInstance.value.instanceId }
    const reason = suspendReason.value.trim()
    if (reason) payload.reason = reason
    await suspendFlowEngineInstance(payload)
    message.success(t('workflow.instance.page.msg.suspend.success'))
    suspendVisible.value = false
    currentSuspendInstance.value = null
    suspendReason.value = ''
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    loading.value = false
  }
}

/** 恢复：二次确认后调用 resume 接口并刷新列表 */
function handleResume(record: FlowInstanceTableRow) {
  const name = record.processTitle || record.instanceCode
  Modal.confirm({
    centered: true,
    title: t('common.page.button.resume'),
    content: t('workflow.instance.page.confirm.resume', { name }),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await resumeFlowEngineInstance({ flowInstanceId: record.instanceId })
        message.success(t('workflow.instance.page.msg.resume.success'))
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, t('common.feedback.failed')))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 打开终止弹窗并设置当前实例 */
function openTerminateModal(record: FlowInstanceTableRow) {
  currentTerminateInstance.value = record
  terminateReason.value = ''
  terminateVisible.value = true
}

/** 提交终止：调用 terminate 接口后关闭弹窗并刷新列表 */
async function submitTerminate() {
  if (!currentTerminateInstance.value) return
  try {
    loading.value = true
    const payload: { flowInstanceId: string; reason?: string } = { flowInstanceId: currentTerminateInstance.value.instanceId }
    const reason = terminateReason.value.trim()
    if (reason) payload.reason = reason
    await terminateFlowEngineInstance(payload)
    message.success(t('workflow.instance.page.msg.terminate.success'))
    terminateVisible.value = false
    currentTerminateInstance.value = null
    terminateReason.value = ''
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    loading.value = false
  }
}

/** 撤回：二次确认后调用 withdraw 接口并刷新列表 */
function handleWithdraw(record: FlowInstanceTableRow) {
  const name = record.processTitle || record.instanceCode
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.title', { action: t('common.page.button.withdraw') }),
    content: t('workflow.instance.page.confirm.revoke', { name }),
    okText: t('common.page.button.ok'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await withdrawFlowEngineInstance(record.instanceCode)
        message.success(t('common.feedback.action.success', { action: t('common.page.button.withdraw') }))
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, t('common.feedback.failed')))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 更新：若有选中行则打编辑弹窗，否则提示请选择 */
function handleUpdate() {
  if (selecmedRow.value) handleEditInstance(selecmedRow.value)
  else message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.flowinstance._self') }))
}

/** 打开编辑弹窗：回填标题与 frmData，拉取最新详情 */
async function handleEditInstance(record: FlowInstanceTableRow) {
  currentEditInstance.value = record
  updateProcessTitle.value = record.processTitle ?? ''
  updateFrmData.value = record.frmData ?? ''
  try {
    const d = await getFlowEngineDetailById(record.instanceId)
    if (d) {
      updateProcessTitle.value = d.processTitle ?? ''
      updateFrmData.value = d.frmData ?? ''
    }
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
  }
  updateVisible.value = true
}

/** 编辑提交：调用 update 接口后关闭弹窗并刷新列表 */
async function handleUpdateSubmit() {
  if (!currentEditInstance.value) return
  try {
    updateLoading.value = true
    const id = currentEditInstance.value.instanceId
    const updatePayload: FlowInstanceEditPayload = { flowInstanceId: id }
    const processTitle = updateProcessTitle.value?.trim()
    const frmData = updateFrmData.value?.trim()
    if (processTitle) updatePayload.processTitle = processTitle
    if (frmData) updatePayload.frmData = frmData
    await updateFlowInstance(id, updatePayload)
    message.success(t('common.feedback.updated'))
    updateVisible.value = false
    currentEditInstance.value = null
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    updateLoading.value = false
  }
}

/** 单条删除：二次确认后 deleteById 并刷新列表 */
function handleDeleteOne(record: FlowInstanceTableRow) {
  const name = record.processTitle || record.instanceCode
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.flowinstance._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteFlowInstanceById(record.instanceId)
        message.success(t('common.feedback.deleted'))
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, t('common.feedback.delete.failed')))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 批量删除：无选中则提示；有选中则二次确认后 deleteBatch 并刷新 */
function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.flowinstance._self') }))
    return
  }
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.flowinstance._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteFlowInstanceBatch(selecmedRows.value.map(r => r.instanceId))
        message.success(t('common.feedback.deleted'))
        selecmedRows.value = []
        selecmedRowKeys.value = []
        selecmedRow.value = null
        loadData()
      } catch (error: unknown) {
        message.error(getErrorMessage(error, t('common.feedback.delete.failed')))
      } finally {
        loading.value = false
      }
    }
  })
}

onMounted(() => loadData())
</script>

<style scoped lang="css">
.workflow-instance {
  padding: 16px;

  &__toolbar {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 12px;
  }

  &__status-select {
    width: 120px;
  }
}
</style>

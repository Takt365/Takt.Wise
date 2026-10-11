<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/todo -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：待办列表页面，包含查询、办结（通过/驳回）、转办、加签、导出等 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-todo">
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
      :row-key="getTodoRowKey"
      :large-screen-column-count="7"
      :small-screen-column-count="4"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
    <TaktModal
      v-model:open="modalVisible"
      :title="t('common.dialog.title.approve')"
      :confirm-loading="loading"
      :ok-text="t('common.page.button.submit')"
      :cancel-text="t('common.page.button.cancel')"
      width="720px"
      @ok="handleApproveOk"
      @cancel="closeApproveModal"
    >
      <div class="todo-modal__sections">
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.instance.page.task.form.content') }}
          </div>
          <TaskFormContent :detail="taskDetail" />
          <TaktFlowPendingAddApproversPanel
            :detail="taskDetail"
            :allow-reduce="!!taskDetail?.canVerify"
            @refresh="reloadTaskDetailInModal"
          />
        </div>
        <div
          v-if="getTaskNodeId(taskDetail) === CASHIER_ROUTE_NODE_ID"
          class="todo-modal__section"
        >
          <div class="todo-modal__section-title">
            {{ t('workflow.todo.page.cashier.payout.method') }}
          </div>
          <a-select
            v-model:value="cashierPayoutChannel"
            :options="cashierPayoutOptions"
            allow-clear
            :placeholder="t('workflow.todo.page.cashier.payout.required')"
            style="width: 100%"
          />
        </div>
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.todo.page.task.approve.action') }}
          </div>
          <ApproveForm
            ref="approveFormRef"
            :form="completeForm"
          />
        </div>
      </div>
    </TaktModal>
    <TaktModal
      v-model:open="transferVisible"
      :title="t('common.dialog.title.transfer')"
      :confirm-loading="loading"
      :ok-text="t('common.page.button.submit')"
      :cancel-text="t('common.page.button.cancel')"
      width="720px"
      @ok="handleTransferOk"
      @cancel="closeTransferModal"
    >
      <div class="todo-modal__sections">
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.instance.page.task.form.content') }}
          </div>
          <TaskFormContent :detail="taskDetail" />
          <TaktFlowPendingAddApproversPanel
            :detail="taskDetail"
            :allow-reduce="!!taskDetail?.canVerify"
            @refresh="reloadTaskDetailInModal"
          />
        </div>
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.todo.page.task.approve.action') }}
          </div>
          <TransferForm
            ref="transferFormRef"
            :form="transferForm"
            :user-options="userOptions"
          />
        </div>
      </div>
    </TaktModal>
    <TaktModal
      v-model:open="addSignVisible"
      :title="t('common.dialog.title.addsign')"
      :confirm-loading="loading"
      :ok-text="t('common.page.button.submit')"
      :cancel-text="t('common.page.button.cancel')"
      width="720px"
      @ok="handleAddSignOk"
      @cancel="closeAddSignModal"
    >
      <div class="todo-modal__sections">
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.instance.page.task.form.content') }}
          </div>
          <TaskFormContent :detail="taskDetail" />
          <TaktFlowPendingAddApproversPanel
            :detail="taskDetail"
            :allow-reduce="!!taskDetail?.canVerify"
            @refresh="reloadTaskDetailInModal"
          />
        </div>
        <div class="todo-modal__section">
          <div class="todo-modal__section-title">
            {{ t('workflow.todo.page.task.approve.action') }}
          </div>
          <AddSignForm
            ref="addSignFormRef"
            :form="addSignForm"
            :user-options="userOptions"
          />
        </div>
      </div>
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
      <a-form-item :label="t('entity.flowtask.taskname')">
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
 * 待办列表页：拉取待办、分页、办结（通过/驳回）、转办、加签；弹窗内展示任务内容与审批/转办/加签表单。
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import {
  RiCheckboxCircleLine,
  RiCloseCircleLine,
  RiUserShared2Line,
  RiUserAddLine
} from '@remixicon/vue'
import { getFlowEngineTodoList, getFlowEngineTodoById, completeFlowEngineTask, transferFlowEngineTask, addFlowEngineApprovers } from '@/api/workflow/flowEngine.api'
import { getUserOptions } from '@/api/identity/user.api'
import ApproveForm from './components/FlowApproveForm.vue'
import TransferForm from './components/FlowTransferForm.vue'
import AddSignForm from './components/FlowAddSignForm.vue'
import TaskFormContent from './components/FlowTaskFormContent.vue'
import type { FlowTodoItem, FlowInstanceDetail, FlowAddApproverItem, FlowTodoQuery } from '@/types/workflow/flowEngine.types'
import type { TaktSelectOption } from '@/types/common.types'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useWorkflowSignalRRefresh, WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh'
import { useWorkflowTodoCountStore } from '@/stores/workflow/useTodoCountStore'
const toErrorMessage = (error: unknown): string => (error instanceof Error ? error.message : String(error))

const { t } = useI18n()
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.flowtask._self') })
)

/** 与 `TaktSingleTable` 的 `@resize-column` 第二参数一致（`ResizableColumn`） */
type TaktResizeColumn = { width?: string | number } & Record<string, unknown>

/** 与种子 ProcessContent 节点 id 一致：出纳确认付款方式 */
const CASHIER_ROUTE_NODE_ID = 'cashier_route'

/** 当前任务节点 ID */
function getTaskNodeId(detail: FlowInstanceDetail | null): string | undefined {
  return detail?.currentActivityId
}

const cashierPayoutChannel = ref<number | undefined>(undefined)
const cashierPayoutOptions = computed(() => [
  { value: 1, label: t('workflow.todo.page.cashier.payout.bank') },
  { value: 2, label: t('workflow.todo.page.cashier.payout.cash') },
  { value: 3, label: t('workflow.todo.page.cashier.payout.repay') }
])

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
const dataSource = ref<FlowTodoItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
/** 列可见键（未启用列设置时保持空数组，展示全部列） */
const visibleColumnKeys = ref<string[]>([])

const columns = computed<TableColumnsType>(() => [
  { title: t('entity.flowinstance.instancecode'), dataIndex: 'instanceCode', key: 'instanceCode', width: 200, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processname'), dataIndex: 'processName', key: 'processName', width: 120, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processtitle'), dataIndex: 'processTitle', key: 'processTitle', ellipsis: true, resizable: true },
  { title: t('entity.flowtask.taskname'), dataIndex: 'taskName', key: 'taskName', width: 100, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.startUserName'), dataIndex: 'startUserName', key: 'startUserName', width: 90, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.starttime'), dataIndex: 'startTime', key: 'startTime', width: 170, resizable: true },
  CreateActionColumn<FlowTodoItem>({
    width: 148,
    actions: [
      {
        key: 'pass',
        label: t('common.page.button.pass'),
        shape: 'plain',
        icon: RiCheckboxCircleLine,
        permission: 'workflow:todo:approve',
        onClick: (record) => handleApprove(record, true)
      },
      {
        key: 'reject',
        label: t('common.page.button.reject'),
        shape: 'plain',
        icon: RiCloseCircleLine,
        permission: 'workflow:todo:approve',
        onClick: (record) => handleApprove(record, false)
      },
      {
        key: 'transfer',
        label: t('common.page.button.transfer'),
        shape: 'plain',
        icon: RiUserShared2Line,
        permission: 'workflow:todo:transfer',
        onClick: (record) => openTransfer(record)
      },
      {
        key: 'addsign',
        label: t('common.page.button.addsign'),
        shape: 'plain',
        icon: RiUserAddLine,
        permission: 'workflow:todo:addsign',
        onClick: (record) => openAddSign(record)
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

/** 待办行 key：取 flowInstanceId 字符串 */
function getTodoRowKey(record: unknown): string {
  if (!record || typeof record !== 'object' || !('flowInstanceId' in record)) return ''
  const row = record as { flowInstanceId?: unknown }
  return row.flowInstanceId != null ? String(row.flowInstanceId) : ''
}

const modalVisible = ref(false)
const currentTask = ref<FlowTodoItem | null>(null)
const taskDetail = ref<FlowInstanceDetail | null>(null)
const approveFormRef = ref<InstanceType<typeof ApproveForm> | null>(null)
const completeForm = reactive({
  comment: '',
  approved: true,
  nodeRejectStep: undefined as string | undefined
})

const userOptions = ref<TaktSelectOption[]>([])
const transferVisible = ref(false)
const addSignVisible = ref(false)
const currentTransferTask = ref<FlowTodoItem | null>(null)
const currentAddSignTask = ref<FlowTodoItem | null>(null)
const transferFormRef = ref<InstanceType<typeof TransferForm> | null>(null)
const transferForm = reactive<{
  toUserId?: string
  toUserName: string
  comment?: string
}>({
  toUserName: ''
})
const addSignFormRef = ref<InstanceType<typeof AddSignForm> | null>(null)
const addSignForm = reactive({
  approverIds: [] as string[],
  approveType: 'sequential',
  reason: '',
  returnToSignNode: false
})

/** 拉取待办列表（分页），结果写入 dataSource 与 total */
async function loadTodo() {
  loading.value = true
  try {
    const res = await getFlowEngineTodoList(buildFlowTodoQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } finally {
    loading.value = false
  }
}

/** 查询：页码置 1 并重新拉取 */
function handleSearch() {
  currentPage.value = 1
  loadTodo()
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
  loadTodo()
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadTodo()
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
  loadTodo()
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

/** 分页变化时更新页码、每页条数并拉取 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadTodo()
}

/** 每页条数变化时更新并拉取 */
function handlePaginationSizeChange(current: number, size: number) {
  currentPage.value = current
  pageSize.value = size
  loadTodo()
}

/** 减签后刷新弹窗内实例详情并刷新待办列表 */
async function reloadTaskDetailInModal() {
  const id = taskDetail.value?.flowInstanceId
  if (id == null) return
  try {
    taskDetail.value = await getFlowEngineTodoById(id)
    await loadTodo()
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 关闭审批弹窗并清空任务详情 */
async function closeApproveModal() {
  modalVisible.value = false
  taskDetail.value = null
}

/** 打开审批弹窗：设置当前任务、办结表单、拉取实例详情 */
async function handleApprove(record: FlowTodoItem, pass: boolean) {
  currentTask.value = record
  completeForm.comment = ''
  completeForm.approved = pass
  completeForm.nodeRejectStep = undefined
  cashierPayoutChannel.value = undefined
  taskDetail.value = null
  try {
    taskDetail.value = await getFlowEngineTodoById(record.flowInstanceId)
    const fd = taskDetail.value?.frmData?.trim()
    if (fd) {
      try {
        const j = JSON.parse(fd) as { payoutChannel?: number }
        if (typeof j.payoutChannel === 'number' && [1, 2, 3].includes(j.payoutChannel)) {
          cashierPayoutChannel.value = j.payoutChannel
        }
      } catch {
        /* ignore */
      }
    }
  } catch {
    taskDetail.value = null
  }
  modalVisible.value = true
}

/** 审批提交：校验表单、调用 complete 接口后关闭弹窗并刷新待办 */
async function handleApproveOk() {
  const ok = await approveFormRef.value?.validate()
  if (!ok || !currentTask.value) return
  const detail = taskDetail.value
  if (
    completeForm.approved &&
    getTaskNodeId(detail) === CASHIER_ROUTE_NODE_ID &&
    cashierPayoutChannel.value == null
  ) {
    message.warning(t('workflow.todo.page.cashier.payout.required'))
    return
  }
  let frmDataPayload: string | undefined
  if (completeForm.approved && getTaskNodeId(detail) === CASHIER_ROUTE_NODE_ID && detail) {
    try {
      const base = detail.frmData?.trim()
        ? (JSON.parse(detail.frmData) as Record<string, unknown>)
        : {}
      base.payoutChannel = cashierPayoutChannel.value as number
      frmDataPayload = JSON.stringify(base)
    } catch {
      message.error(t('common.feedback.load.data.failed'))
      return
    }
  }
  loading.value = true
  try {
    const payload: {
      flowInstanceId: string
      instanceCode: string
      approved: boolean
      comment?: string
      nodeRejectStep?: string
      frmData?: string
    } = {
      flowInstanceId: currentTask.value.flowInstanceId,
      instanceCode: currentTask.value.instanceCode,
      approved: completeForm.approved
    }
    if (completeForm.comment) payload.comment = completeForm.comment
    if (completeForm.nodeRejectStep) payload.nodeRejectStep = completeForm.nodeRejectStep
    if (frmDataPayload) payload.frmData = frmDataPayload
    await completeFlowEngineTask(payload)
    message.success(t('common.feedback.success'))
    modalVisible.value = false
    taskDetail.value = null
    loadTodo()
  } catch (error: unknown) {
    message.error(toErrorMessage(error) || t('common.feedback.failed'))
  } finally {
    loading.value = false
  }
}

/** 若用户选项为空则拉取用户列表供转办/加签下拉使用 */
async function ensureUserOptions() {
  if (userOptions.value.length === 0) {
    try {
      userOptions.value = await getUserOptions()
    } catch {
      message.error(t('common.feedback.load.data.failed'))
    }
  }
}

/** 关闭转办弹窗并清空任务详情 */
function closeTransferModal() {
  transferVisible.value = false
  taskDetail.value = null
}

/** 关闭加签弹窗并清空任务详情 */
function closeAddSignModal() {
  addSignVisible.value = false
  taskDetail.value = null
}

/** 打开转办弹窗：设置当前任务、拉取实例详情、拉取用户选项 */
async function openTransfer(record: FlowTodoItem) {
  currentTransferTask.value = record
  delete transferForm.toUserId
  transferForm.toUserName = ''
  delete transferForm.comment
  taskDetail.value = null
  try {
    taskDetail.value = await getFlowEngineTodoById(record.flowInstanceId)
  } catch {
    taskDetail.value = null
  }
  ensureUserOptions()
  transferVisible.value = true
}

/** 转办提交：校验表单、调用 transfer 接口后关闭弹窗并刷新待办 */
async function handleTransferOk() {
  const ok = await transferFormRef.value?.validate()
  if (!ok || !currentTransferTask.value || !transferForm.toUserId || !transferForm.toUserName) return
  loading.value = true
  try {
    await transferFlowEngineTask({
      flowInstanceId: currentTransferTask.value.flowInstanceId,
      instanceCode: currentTransferTask.value.instanceCode,
      toUserId: transferForm.toUserId,
      toUserName: transferForm.toUserName,
      comment: transferForm.comment || undefined
    })
    message.success(t('common.feedback.action.success', { action: t('common.page.button.transfer') }))
    transferVisible.value = false
    currentTransferTask.value = null
    taskDetail.value = null
    loadTodo()
  } catch (error: unknown) {
    message.error(toErrorMessage(error) || t('common.feedback.failed'))
  } finally {
    loading.value = false
  }
}

/** 打开加签弹窗：设置当前任务、拉取实例详情、拉取用户选项 */
async function openAddSign(record: FlowTodoItem) {
  currentAddSignTask.value = record
  addSignForm.approverIds = []
  addSignForm.approveType = 'sequential'
  addSignForm.reason = ''
  addSignForm.returnToSignNode = false
  taskDetail.value = null
  try {
    taskDetail.value = await getFlowEngineTodoById(record.flowInstanceId)
  } catch {
    taskDetail.value = null
  }
  ensureUserOptions()
  addSignVisible.value = true
}

/** 加签提交：校验表单、调用 addApprovers 接口后关闭弹窗并刷新待办 */
async function handleAddSignOk() {
  const ok = await addSignFormRef.value?.validate()
  if (!ok || !currentAddSignTask.value || !addSignForm.approverIds?.length) return
  const approvers: FlowAddApproverItem[] = addSignForm.approverIds.map(id => {
    const opt = userOptions.value.find(o => String(o.dictValue) === String(id))
    return { approverUserId: id, approverUserName: opt?.dictLabel ?? id }
  })
  loading.value = true
  try {
    const payload: {
      flowInstanceId: string
      instanceCode: string
      approvers: FlowAddApproverItem[]
      approveType: string
      returnToSignNode: boolean
      reason?: string
    } = {
      flowInstanceId: currentAddSignTask.value.flowInstanceId,
      instanceCode: currentAddSignTask.value.instanceCode,
      approvers,
      approveType: addSignForm.approveType,
      returnToSignNode: addSignForm.returnToSignNode
    }
    if (addSignForm.reason) payload.reason = addSignForm.reason
    await addFlowEngineApprovers(payload)
    message.success(t('common.feedback.action.success', { action: t('common.page.button.addsign') }))
    addSignVisible.value = false
    currentAddSignTask.value = null
    taskDetail.value = null
    loadTodo()
  } catch (error: unknown) {
    message.error(toErrorMessage(error) || t('common.feedback.failed'))
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  void useWorkflowTodoCountStore().refreshTodoCountAsync()
  loadTodo()
})
useWorkflowSignalRRefresh(loadTodo, WORKFLOW_TABLE_NAMES.todo)
</script>

<style scoped lang="css">
.workflow-todo {
  padding: 16px;
}
.workflow-todo__toolbar {
  margin-bottom: 12px;
}

.todo-modal__sections {
  display: flex;
  flex-direction: column;
  gap: 20px;
}
.todo-modal__section-title {
  font-weight: 600;
  margin-bottom: 8px;
  padding-bottom: 6px;
  border-bottom: 1px solid var(--ant-color-border-secondary);
  color: var(--ant-color-text);
}
</style>

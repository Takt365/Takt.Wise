<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/my -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：我的流程列表页面，包含我发起的流程、发起流程、草稿、详情与导出 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-my">
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
      :show-start-flow="true"
      start-flow-permission="workflow:instance:initiate"
      :show-send-message="false"
      :show-refresh="true"
      :show-export="false"
      :show-fullscreen="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :refresh-loading="loading"
      @refresh="handleRefresh"
      @start-flow="goStartFlow"
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
    <TaktModal
      v-model:open="editVisible"
      :title="t('common.dialog.title.edit', { entity: t('entity.flowinstance._self') })"
      :confirm-loading="editLoading"
      :ok-text="t('common.page.button.ok')"
      :cancel-text="t('common.page.button.cancel')"
      @ok="handleEditSubmit"
      @cancel="editVisible = false"
    >
      <FlowInstanceEditForm
        ref="editFormRef"
        :form="editForm"
      />
    </TaktModal>
    <TaktModal
      v-model:open="startFlowVisible"
      :title="t('common.dialog.title.initiate')"
      width="900px"
      @cancel="closeStartFlowModal"
    >
      <FlowStartForm
        :key="startFlowFormKey"
        ref="startFormRef"
        :form="startFlowForm"
        :scheme-options="schemeOptions"
        :scheme-loading="schemeLoading"
      />
      <template #footer>
        <a-button @click="closeStartFlowModal">
          {{ t('common.page.button.cancel') }}
        </a-button>
        <a-button
          v-permission="'workflow:instance:initiate'"
          :loading="startDraftLoading"
          @click="handleStartFlowDraft"
        >
          {{ t('workflow.my.page.start.flow.form.save.draft.label') }}
        </a-button>
        <a-button
          v-permission="'workflow:instance:initiate'"
          type="primary"
          :loading="startFlowLoading"
          @click="handleStartFlowSubmit"
        >
          {{ t('workflow.my.page.start.flow.form.submit.label') }}
        </a-button>
      </template>
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
 * 我的流程页：我发起的实例列表、分页、发起流程、草稿、从草稿启动、详情、撤回、编辑、导出。
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEyeLine, RiEditLine, RiPlayLine, RiArrowGoBackLine } from '@remixicon/vue'
import {
  getFlowEngineMyList,
  getFlowEngineMyById,
  withdrawFlowEngineInstance,
  startFlowEngineFromDraft,
  startFlowEngineInstance,
  createFlowEngineDraft,
  getFlowEngineStartableSchemes
} from '@/api/workflow/flowEngine.api'
import { updateFlowInstance } from '@/api/workflow/flowInstance.api'
import { useUserStore } from '@/stores/identity/useUserStore'
import FlowInstanceDetailForm from '../instance/components/FlowInstanceDetailForm.vue'
import FlowInstanceEditForm from './components/FlowInstanceEditForm.vue'
import FlowStartForm from './components/FlowStartForm.vue'
import type { FlowStart, FlowInstanceListItem, FlowInstanceDetail, FlowTodoQuery } from '@/types/workflow/flowEngine.types'
import type { FlowInstanceEditPayload } from '@/types/workflow/flowInstance.types'
import { useWorkflowSignalRRefresh, WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
const toErrorMessage = (error: unknown): string => (error instanceof Error ? error.message : String(error))

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
const editVisible = ref(false)
const editFormRef = ref<InstanceType<typeof FlowInstanceEditForm> | null>(null)
const editForm = reactive({ processTitle: '', frmData: '' })
const editLoading = ref(false)
const currentEditRecord = ref<FlowInstanceListItem | null>(null)
const startFlowVisible = ref(false)
const startFlowForm = reactive<{ processKey: string; processTitle: string; frmData: string }>({ processKey: '', processTitle: '', frmData: '' })
const schemeOptions = ref<{ label: string; value: string }[]>([])
const schemeLoading = ref(false)
const startFormRef = ref<InstanceType<typeof FlowStartForm> | null>(null)
/** 每次打开发起弹窗递增，子组件重挂以便「申请人」重新默认当前登录用户 */
const startFlowFormKey = ref(0)
const startFlowLoading = ref(false)
const startDraftLoading = ref(false)
/** 列可见键（未启用列设置时保持空数组，展示全部列） */
const visibleColumnKeys = ref<string[]>([])

const userStore = useUserStore()
const currentUserId = computed(() => String(userStore.userInfo?.userId ?? ''))

const columns = computed<TableColumnsType>(() => [
  { title: t('entity.flowinstance.instancecode'), dataIndex: 'instanceCode', key: 'instanceCode', width: 200, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processname'), dataIndex: 'processName', key: 'processName', width: 120, resizable: true, ellipsis: true },
  { title: t('entity.flowinstance.processtitle'), dataIndex: 'processTitle', key: 'processTitle', ellipsis: true, resizable: true },
  { title: t('entity.flowinstance.instancestatus'), dataIndex: 'instanceStatus', key: 'instanceStatus', width: 90 },
  { title: t('entity.flowinstance.starttime'), dataIndex: 'startTime', key: 'startTime', width: 170, resizable: true },
  CreateActionColumn<FlowInstanceListItem>({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'workflow:my:query',
        onClick: (record) => showDetail(record)
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'workflow:my:update',
        visible: (record) => (record.instanceStatus === 0 || record.instanceStatus === 5) && isStarter(record),
        onClick: (record) => handleEdit(record)
      },
      {
        key: 'start',
        label: t('workflow.my.page.start.from.draft'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'workflow:instance:initiate',
        visible: (record) => record.instanceStatus === 5 && isStarter(record),
        onClick: (record) => handleStartFromDraft(record)
      },
      {
        key: 'withdraw',
        label: t('common.page.button.withdraw'),
        shape: 'plain',
        icon: RiArrowGoBackLine,
        permission: 'workflow:instance:withdraw',
        visible: (record) => record.instanceStatus === 0 && isStarter(record),
        onClick: (record) => handleWithdraw(record)
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

/** 判断当前用户是否为该实例发起人 */
function isStarter(r: FlowInstanceListItem) {
  return String(r.startUserId) === String(currentUserId.value)
}

/** 打开发起流程弹窗并拉取已发布方案列表填充 schemeOptions */
async function goStartFlow() {
  startFlowFormKey.value += 1
  startFlowForm.processKey = ''
  startFlowForm.processTitle = ''
  startFlowForm.frmData = ''
  startFlowVisible.value = true
  schemeLoading.value = true
  try {
    const list = await getFlowEngineStartableSchemes()
    schemeOptions.value = list.map((s) => ({
      label: `${s.processName}（${s.processKey}）`,
      value: s.processKey
    }))
    if (list.length === 1) startFlowForm.processKey = list[0].processKey
  } finally {
    schemeLoading.value = false
  }
}

/** 关闭发起流程弹窗 */
function closeStartFlowModal() {
  startFlowVisible.value = false
}

/** 发起流程提交：校验后调用 start 接口并刷新列表 */
async function handleStartFlowSubmit() {
  const ok = await startFormRef.value?.validate()
  if (!ok || !startFlowForm.processKey?.trim()) return
  startFlowLoading.value = true
  try {
    const processKey = startFlowForm.processKey.trim()
    const payload: FlowStart = { processKey }
    const processTitle = startFlowForm.processTitle.trim()
    const frmData = startFlowForm.frmData.trim()
    if (processTitle) payload.processTitle = processTitle
    if (frmData) payload.frmData = frmData
    payload.businessType = processKey
    const res = await startFlowEngineInstance(payload)
    message.success(t('workflow.my.page.start.flow.form.submit.success', { code: res.instanceCode }))
    closeStartFlowModal()
    loadList()
  } catch (err: unknown) {
    message.error(toErrorMessage(err) || t('common.feedback.failed'))
  } finally {
    startFlowLoading.value = false
  }
}

/** 保存草稿：校验后调用 createDraft 接口并刷新列表 */
async function handleStartFlowDraft() {
  const ok = await startFormRef.value?.validate()
  if (!ok || !startFlowForm.processKey?.trim()) return
  startDraftLoading.value = true
  try {
    const processKey = startFlowForm.processKey.trim()
    const payload: FlowStart = { processKey }
    const processTitle = startFlowForm.processTitle.trim()
    const frmData = startFlowForm.frmData.trim()
    if (processTitle) payload.processTitle = processTitle
    if (frmData) payload.frmData = frmData
    payload.businessType = processKey
    const res = await createFlowEngineDraft(payload)
    message.success(t('workflow.my.page.start.flow.form.save.draft.success', { code: res.instanceCode }))
    closeStartFlowModal()
    loadList()
  } catch (err: unknown) {
    message.error(toErrorMessage(err) || t('common.feedback.failed'))
  } finally {
    startDraftLoading.value = false
  }
}

/** 实例行 key：取 flowInstanceId 字符串 */
function getInstanceId(record: unknown): string {
  if (!record || typeof record !== 'object' || !('flowInstanceId' in record)) return ''
  const flowInstanceId = (record as { flowInstanceId?: unknown }).flowInstanceId
  return flowInstanceId != null ? String(flowInstanceId) : ''
}

/** 拉取我发起的流程列表（分页），结果写入 dataSource 与 total */
async function loadList() {
  loading.value = true
  try {
    const res = await getFlowEngineMyList(buildFlowTodoQuery())
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
  // TaktSingleTable 分页由 TaktPagination 处理，此处仅保留以兼容 @change
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
    detail.value = await getFlowEngineMyById(record.flowInstanceId)
    detailVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 加签/减签后刷新当前详情（保持弹窗打开） */
async function reloadInstanceDetail() {
  if (!detail.value?.flowInstanceId) return
  try {
    detail.value = await getFlowEngineMyById(detail.value.flowInstanceId)
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  }
}

/** 打开编辑弹窗：回填标题与 frmData，拉取最新详情 */
async function handleEdit(record: FlowInstanceListItem) {
  currentEditRecord.value = record
  editForm.processTitle = record.processTitle ?? ''
  editForm.frmData = record.frmData ?? ''
  try {
    const d = await getFlowEngineMyById(record.flowInstanceId)
    if (d) {
      editForm.processTitle = d.processTitle ?? ''
      editForm.frmData = d.frmData ?? ''
    }
  } catch {
    // 忽略详情回填失败，保留当前行数据进入编辑
  }
  editVisible.value = true
}

/** 编辑提交：校验后调用 update 接口并关闭弹窗、刷新列表 */
async function handleEditSubmit() {
  const ok = await editFormRef.value?.validate()
  if (!ok || !currentEditRecord.value) return
  try {
    editLoading.value = true
    const id = currentEditRecord.value.flowInstanceId
    const processTitle = editForm.processTitle?.trim()
    const frmData = editForm.frmData?.trim()
    const dto: FlowInstanceEditPayload = {
      flowInstanceId: id,
      ...(processTitle ? { processTitle } : {}),
      ...(frmData ? { frmData } : {})
    }
    await updateFlowInstance(id, dto)
    message.success(t('common.feedback.updated'))
    editVisible.value = false
    currentEditRecord.value = null
    loadList()
  } catch (error: unknown) {
    message.error(toErrorMessage(error) || t('common.feedback.failed'))
  } finally {
    editLoading.value = false
  }
}

/** 从草稿启动：二次确认后调用 startFromDraft 并刷新列表 */
function handleStartFromDraft(record: FlowInstanceListItem) {
  Modal.confirm({
    centered: true,
    title: t('workflow.my.page.start.from.draft'),
    content: t('workflow.my.page.confirm.start.from.draft', { name: record.processTitle || record.instanceCode }),
    onOk: async () => {
      try {
        loading.value = true
        await startFlowEngineFromDraft(record.flowInstanceId)
        message.success(t('common.feedback.success'))
        loadList()
      } catch (error: unknown) {
        message.error(toErrorMessage(error) || t('common.feedback.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

/** 撤回：二次确认后调用 withdraw 接口并刷新列表 */
function handleWithdraw(record: FlowInstanceListItem) {
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.title', { action: t('common.page.button.withdraw') }),
    content: t('workflow.instance.page.confirm.revoke', { name: record.processTitle || record.instanceCode }),
    onOk: async () => {
      try {
        loading.value = true
        await withdrawFlowEngineInstance(record.instanceCode)
        message.success(t('common.feedback.action.success', { action: t('common.page.button.withdraw') }))
        loadList()
      } catch (error: unknown) {
        message.error(toErrorMessage(error) || t('common.feedback.failed'))
      } finally {
        loading.value = false
      }
    }
  })
}

onMounted(() => loadList())
useWorkflowSignalRRefresh(loadList, WORKFLOW_TABLE_NAMES.my)
</script>

<style scoped lang="css">
.workflow-my {
  padding: 16px;
}
.history-item {
  font-size: 12px;
  margin-bottom: 4px;
}
</style>

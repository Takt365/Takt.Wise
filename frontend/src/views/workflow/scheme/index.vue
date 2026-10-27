<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/scheme -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：流程方案管理页面，包含列表、查询、导出、新增、编辑、删除及 ProcessContent 设计 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-scheme">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <TaktToolsBar
      create-permission="workflow:scheme:create"
      update-permission="workflow:scheme:update"
      delete-permission="workflow:scheme:delete"
      import-permission="workflow:scheme:import"
      export-permission="workflow:scheme:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-refresh="true"
      :show-fullscreen="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :update-disabled="!selecmedRow"
      :delete-disabled="selecmedRows.length === 0"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :export-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
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
      :row-key="getSchemeId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'processStatus'">
          <TaktDictTag
            :value="record.processStatus"
            dict-type="sys_scheme_status"
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
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <SchemeForm
        ref="schemeFormRef"
        :form="form"
      />
    </TaktModal>

    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.flowscheme._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.flowscheme._self"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.flowscheme.processkey')">
        <a-input v-model:value="advancedQueryForm.processKey" />
      </a-form-item>
      <a-form-item :label="t('entity.flowscheme.processname')">
        <a-input v-model:value="advancedQueryForm.processName" />
      </a-form-item>
      <a-form-item :label="t('entity.flowscheme.processstatus')">
        <TaktSelect
          v-model="advancedQueryForm.processStatus"
          dict-type="sys_scheme_status"
          style="width: 100%"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowscheme.processstatus') })"
          allow-clear
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
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 流程方案列表页：查询、分页、导出、新增、编辑、删除；弹窗内使用 SchemeForm 编辑方案与 ProcessContent。
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { validateProcessContentForSave } from '@/utils/workflow/validateProcessContent'
import { DEFAULT_PROCESS_CONTENT } from '@/utils/workflow/defaultProcessContent'
import SchemeForm from './components/SchemeForm.vue'
import {
  getFlowSchemeList,
  getFlowSchemeById,
  createFlowScheme,
  updateFlowScheme,
  deleteFlowSchemeById,
  updateFlowSchemeStatus,
  exportFlowScheme,
  getFlowSchemeTemplate,
  importFlowScheme
} from '@/api/workflow/flowScheme.api'
import type { FlowScheme, FlowSchemeCreate, FlowSchemeUpdate, FlowSchemeFormModel } from '@/types/workflow/flowScheme.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { useWorkflowSignalRRefresh, WORKFLOW_TABLE_NAMES } from '@/composables/useWorkflowSignalrRefresh'
import { RiEditLine, RiDeleteBinLine, RiPlayLine, RiStopLine } from '@remixicon/vue'

const { t } = useI18n()
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.flowscheme._self') })
)
const tenantStore = useTenantStore()
const userStore = useUserStore()
const loading = ref(false)
const queryKeyword = ref('')
const dataSource = ref<FlowScheme[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selecmedRow = ref<FlowScheme | null>(null)
const selecmedRows = ref<FlowScheme[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
const formTitle = ref('')
const formLoading = ref(false)
const schemeFormRef = ref<InstanceType<typeof SchemeForm> | null>(null)
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref<{ processKey: string; processName: string; processStatus: number | undefined }>({ processKey: '', processName: '', processStatus: undefined })
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
/** 导入弹窗可见 */
const importVisible = ref(false)
/** Excel 导入导出文件名 */
const excelNames = taktExcelEntityNames('TaktFlowScheme')

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
function getColumnKey(col: any): string {
  const key = col.key || col.dataIndex || col.title
  return key ? String(key) : ''
}
function getSorterInfo(sorter: unknown): TableSorterInfo {
  if (typeof sorter !== 'object' || sorter === null) return {}
  const sorterObj = sorter as { field?: unknown; order?: unknown }
  const info: TableSorterInfo = {}
  if (typeof sorterObj.field === 'string') info.field = sorterObj.field
  if (typeof sorterObj.order === 'string') info.order = sorterObj.order
  return info
}
const form = reactive<FlowSchemeFormModel>({
  tenantCode: '',
  companyCode: '',
  companyDefaultCulture: '',
  processKey: '',
  processName: '',
  definitionVersion: 1,
  processVersion: 'v1.0.0',
  isLatest: 1,
  processCategory: 0,
  processStatus: 0,
  suspensionState: 1,
  sortOrder: 0,
  processContent: ''
})
const defaultProcessContent = DEFAULT_PROCESS_CONTENT
const getSchemeId = (record: unknown): string => {
  if (!record || typeof record !== 'object' || !('flowSchemeId' in record)) return ''
  const flowSchemeId = (record as { flowSchemeId?: unknown }).flowSchemeId
  return flowSchemeId != null ? String(flowSchemeId) : ''
}

/** 表格列：与 @/types/workflow/flowScheme.types FlowScheme 字段一致（列表展示用，不含 processContent 大字段） */
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'flowSchemeId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left'
  },
  {
    title: t('entity.flowscheme.processkey'),
    dataIndex: 'processKey',
    key: 'processKey',
    width: 120,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowscheme.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowscheme.processcategory'),
    dataIndex: 'processCategory',
    key: 'processCategory',
    width: 100
  },
  {
    title: t('entity.flowscheme.processversion'),
    dataIndex: 'processVersion',
    key: 'processVersion',
    width: 80
  },
  {
    title: t('entity.flowscheme.processdescription'),
    dataIndex: 'processDescription',
    key: 'processDescription',
    width: 140,
    ellipsis: true
  },
  {
    title: t('entity.flowscheme.formcode'),
    dataIndex: 'formCode',
    key: 'formCode',
    width: 100,
    ellipsis: true
  },
  {
    title: t('entity.flowscheme.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 80
  },
  {
    title: t('entity.flowscheme.processstatus'),
    dataIndex: 'processStatus',
    key: 'processStatus',
    width: 90
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'workflow:scheme:update',
        onClick: (_record: FlowScheme) => handleEdit(_record)
      },
      {
        key: 'publish',
        label: t('common.page.button.publish'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'workflow:scheme:update',
        visible: (record: FlowScheme) => record.processStatus !== 1,
        onClick: (record: FlowScheme) => handleSchemeStatusChange(record, 1)
      },
      {
        key: 'disable',
        label: t('common.page.button.disable'),
        shape: 'plain',
        icon: RiStopLine,
        permission: 'workflow:scheme:update',
        visible: (record: FlowScheme) => record.processStatus === 1,
        onClick: (record: FlowScheme) => handleSchemeStatusChange(record, 2)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'workflow:scheme:delete',
        onClick: (_record: FlowScheme) => handleDeleteOne(_record)
      }
    ]
  })
])

// 合并列配置（包含审计字段）- 使用 any 避免 TypeScript「类型实例化过深」错误

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: FlowScheme[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: FlowScheme, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getSchemeId(selecmedRow.value) === getSchemeId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: FlowScheme[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: FlowScheme) => ({
  onClick: () => {
    const key = getSchemeId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) selecmedRowKeys.value.splice(index, 1)
    else selecmedRowKeys.value.push(key)
    selecmedRows.value = dataSource.value.filter(item => selecmedRowKeys.value.includes(getSchemeId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
  }
})

/** 流程状态数字转展示文案（0 草稿 / 1 已发布 / 2 已停用） */
/** 拉取流程方案列表（分页），结果写入 dataSource 与 total */
async function loadData() {
  try {
    loading.value = true
    const params: any = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    if (queryKeyword.value || advancedQueryForm.value.processKey) {
      params.processKey = queryKeyword.value || advancedQueryForm.value.processKey
    }
    if (queryKeyword.value || advancedQueryForm.value.processName) {
      params.processName = queryKeyword.value || advancedQueryForm.value.processName
    }
    if (advancedQueryForm.value.processStatus !== undefined) {
      params.processStatus = advancedQueryForm.value.processStatus
    }
    const res = (await getFlowSchemeList(params))
    dataSource.value = res.data ?? []
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
useWorkflowSignalRRefresh(loadData, WORKFLOW_TABLE_NAMES.scheme)
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置：清空关键词与高级查询、页码置 1 并重新拉取 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = { processKey: '', processName: '', processStatus: undefined }
  currentPage.value = 1
  loadData()
}

/** 打开高级查询弹窗 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：应用条件、页码置 1、拉取并关闭弹窗 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadData()
  advancedQueryVisible.value = false
}

/** 高级查询重置：清空高级查询表单 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = { processKey: '', processName: '', processStatus: undefined }
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

/** 表格变化（排序等），当前仅占位 */
function handleTableChange(_pagination: unknown, _filters: unknown, sorter: unknown) {
  const sorterInfo = getSorterInfo(sorter)
  if (sorterInfo.order) {
    // 如需服务端排序可在此处理
  }
}

/** 分页页码或每页条数变化时重新拉取 */
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

/** 刷新：重新拉取列表 */
function handleRefresh() {
  loadData()
}

/** 同步租户/公司隔离字段 */
function syncFormScopeDefaults(force = false) {
  if (force || !form.tenantCode) form.tenantCode = tenantStore.tenantCode
  if (force || !form.companyCode) form.companyCode = tenantStore.companyCode
  if (force || !form.companyDefaultCulture) {
    form.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}

/** 打开导入弹窗 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getFlowSchemeTemplate(sheetName, fileName)
  return (res as { data?: Blob })?.data ?? res
}

/** 上传并导入 Excel */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importFlowScheme(file, sheetName)
}

/** 导入成功回调 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入弹窗 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出：与列表查询条件一致 */
async function handleExport() {
  try {
    loading.value = true
    const query: Record<string, unknown> = {
      pageIndex: 1,
      pageSize: 10000
    }
    if (queryKeyword.value || advancedQueryForm.value.processKey) {
      query.processKey = queryKeyword.value || advancedQueryForm.value.processKey
    }
    if (queryKeyword.value || advancedQueryForm.value.processName) {
      query.processName = queryKeyword.value || advancedQueryForm.value.processName
    }
    if (advancedQueryForm.value.processStatus !== undefined) {
      query.processStatus = advancedQueryForm.value.processStatus
    }
    const blob = await exportFlowScheme(query, excelNames.sheet, excelNames.fileBase)
    const fileName = resolveExportDownloadFileName(blob, excelNames.fileBase)
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url)
    message.success(t('common.feedback.export.success'))
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.export.failed')))
  } finally {
    loading.value = false
  }
}

/**
 * 更新方案发布状态
 * @param record 方案行
 * @param processStatus 目标状态（1 发布 / 2 停用）
 */
async function handleSchemeStatusChange(record: FlowScheme, processStatus: number) {
  try {
    loading.value = true
    await updateFlowSchemeStatus({ flowSchemeId: record.flowSchemeId, processStatus })
    message.success(processStatus === 1 ? t('workflow.scheme.page.publish.success') : t('workflow.scheme.page.disable.success'))
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    loading.value = false
  }
}

/** 列宽拖拽后更新对应列的 width */
function handleResizeColumn(w: number, col: any) {
  const column = columns.value.find((c: any) => {
    const colKey = col.key || col.dataIndex || col.title
    const cKey = c.key || c.dataIndex || c.title
    return colKey && cKey && String(colKey) === String(cKey)
  })
  if (column) {
    column.width = w
  }
}

/** 新增：清空 form、设置标题、打开弹窗并重置步骤 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.flowscheme._self') })
  delete form.flowSchemeId
  form.processKey = ''
  form.processName = ''
  form.processCategory = 0
  form.processStatus = 0
  form.definitionVersion = 1
  form.processVersion = 'v1.0.0'
  form.isLatest = 1
  form.suspensionState = 1
  form.sortOrder = 0
  form.processDescription = ''
  form.processContent = defaultProcessContent
  delete form.formId
  delete form.formCode
  syncFormScopeDefaults(true)
  formVisible.value = true
  setTimeout(() => schemeFormRef.value?.resetSteps?.(), 0)
}

/** 编辑：调用 getFlowSchemeById（后端需 workflow:scheme:query）；保存需 create/update */
async function handleEdit(record: FlowScheme) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.flowscheme._self') })
  formLoading.value = true
  try {
    const detail = await getFlowSchemeById(String(record.flowSchemeId))
    form.flowSchemeId = detail.flowSchemeId
    form.tenantCode = detail.tenantCode
    form.companyCode = detail.companyCode
    form.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
    form.processKey = detail.processKey
    form.processName = detail.processName
    form.definitionVersion = detail.definitionVersion ?? 1
    form.processVersion = detail.processVersion ?? 'v1.0.0'
    form.isLatest = detail.isLatest ?? 1
    form.processCategory = detail.processCategory ?? 0
    form.processDescription = detail.processDescription ?? ''
    form.processStatus = detail.processStatus
    form.suspensionState = detail.suspensionState ?? 1
    form.sortOrder = detail.sortOrder ?? 0
    const rawContent = (detail as { processContent?: string; ProcessContent?: string }).processContent ?? (detail as { processContent?: string; ProcessContent?: string }).ProcessContent
    const contentStr = typeof rawContent === 'string' ? rawContent.trim() : (rawContent != null ? JSON.stringify(rawContent) : '')
    form.processContent = contentStr || defaultProcessContent

    /** 与库表/接口一致的 ProcessContent：拉取后立即校验，非法时提示（条数无关，仅校验当前编辑这一条） */
    if (contentStr) {
      const pv = validateProcessContentForSave(contentStr)
      if (!pv.ok) message.warning(t('workflow.scheme.page.invalid.process.content'))
    }
    if (detail.formId != null) {
      form.formId = String(detail.formId)
    } else {
      delete form.formId
    }
    if (detail.formCode) {
      form.formCode = detail.formCode
    } else {
      delete form.formCode
    }
    formVisible.value = true
    setTimeout(() => schemeFormRef.value?.resetSteps?.(), 0)
  } catch {
    message.error(t('workflow.scheme.page.load.detail.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 更新：若有选中行则编辑该行，否则提示请选择 */
function handleUpdate() {
  if (selecmedRow.value) handleEdit(selecmedRow.value)
  else message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.flowscheme._self') }))
}

/** 单条删除：二次确认后 deleteById 并刷新列表 */
function handleDeleteOne(record: FlowScheme) {
  const name = record.processName || getSchemeId(record)
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.flowscheme._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteFlowSchemeById(record.flowSchemeId)
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

/** 批量删除：无选中则提示；有选中则二次确认后逐条 deleteById 并刷新 */
function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.flowscheme._self') }))
    return
  }
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { count: selecmedRows.value.length, entity: t('entity.flowscheme._self') }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        for (const row of selecmedRows.value) {
          await deleteFlowSchemeById(row.flowSchemeId)
        }
        message.success(t('common.feedback.deleted'))
        selecmedRowKeys.value = []
        selecmedRows.value = []
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

/** 关闭表单弹窗 */
function handleFormCancel() {
  formVisible.value = false
}

/** 提交：校验步骤、调用 create/update 后关闭弹窗并刷新列表 */
async function handleFormSubmit() {
  const valid = await schemeFormRef.value?.validateAllSteps?.()
  if (valid === false) {
    message.warning(t('workflow.scheme.page.step.complete.required'))
    return
  }
  const persisted = await schemeFormRef.value?.persistFormBeforeSchemeSave?.()
  if (persisted === false) return
  const pcCheck = validateProcessContentForSave(form.processContent)
  if (!pcCheck.ok) {
    message.warning(t('workflow.scheme.page.invalid.process.content'))
    return
  }
  try {
    formLoading.value = true
    syncFormScopeDefaults(true)
    const payload: FlowSchemeCreate = {
      tenantCode: form.tenantCode ?? tenantStore.tenantCode,
      companyCode: form.companyCode ?? tenantStore.companyCode,
      companyDefaultCulture: form.companyDefaultCulture ?? userStore.userInfo?.companyDefaultCulture ?? '',
      processKey: form.processKey?.trim() ?? '',
      processName: form.processName?.trim() ?? '',
      definitionVersion: form.definitionVersion ?? 1,
      processVersion: form.processVersion?.trim() || 'v1.0.0',
      isLatest: form.isLatest ?? 1,
      processCategory: form.processCategory ?? 0,
      processDescription: form.processDescription?.trim() || undefined,
      processStatus: form.processStatus ?? 0,
      suspensionState: form.suspensionState ?? 1,
      sortOrder: form.sortOrder ?? 0
    }
    if (form.processContent?.trim()) payload.processContent = form.processContent.trim()
    if (form.formId) payload.formId = form.formId
    if (form.formCode) payload.formCode = form.formCode
    if (form.flowSchemeId) {
      await updateFlowScheme(form.flowSchemeId, { ...payload, flowSchemeId: form.flowSchemeId } as FlowSchemeUpdate)
      message.success(t('common.feedback.updated'))
    } else {
      await createFlowScheme(payload)
      message.success(t('common.feedback.created'))
    }
    formVisible.value = false
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    formLoading.value = false
  }
}

onMounted(() => loadData())
</script>

<style scoped lang="css">
.workflow-scheme {
  padding: 16px;
}
</style>

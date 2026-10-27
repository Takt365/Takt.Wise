<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/workflow/form -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程表单管理页面，包含列表、查询、新增、编辑、删除及表单设计 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="workflow-form">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <TaktToolsBar
      create-permission="workflow:form:create"
      update-permission="workflow:form:update"
      delete-permission="workflow:form:delete"
      import-permission="workflow:form:import"
      export-permission="workflow:form:export"
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
      :row-key="getFormId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :large-screen-column-count="9"
      :small-screen-column-count="5"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'formCategory'">
          <TaktDictTag
            :value="record.formCategory"
            dict-type="sys_form_category"
          />
        </template>
        <template v-else-if="column.key === 'formType'">
          <TaktDictTag
            :value="record.formType"
            dict-type="sys_form_type"
          />
        </template>
        <template v-else-if="column.key === 'isDatasource'">
          <TaktDictTag
            :value="record.isDatasource"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'formStatus'">
          <TaktDictTag
            :value="record.formStatus"
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
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <FormForm
        ref="formFormRef"
        :form="form"
      />
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <a-form-item :label="t('entity.flowform.formcode')">
        <a-input v-model:value="advancedQueryForm.formCode" />
      </a-form-item>
      <a-form-item :label="t('entity.flowform.formname')">
        <a-input v-model:value="advancedQueryForm.formName" />
      </a-form-item>
      <a-form-item :label="t('entity.flowform.formstatus')">
        <TaktSelect
          v-model="advancedQueryForm.formStatus"
          dict-type="sys_scheme_status"
          style="width: 100%"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowform.formstatus') })"
          allow-clear
        />
      </a-form-item>
    </TaktQueryDrawer>

    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.flowform._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.flowform._self"
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
 * 流程表单列表页：查询、分页、新增、编辑、删除；弹窗内使用 FormForm 编辑表单与 formConfig。
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import TaktQueryBar from '@/components/business/TaktQueryBar/index.vue'
import FormForm from './components/FormForm.vue'
import {
  getFlowFormList,
  getFlowFormById,
  createFlowForm,
  updateFlowForm,
  deleteFlowFormById,
  deleteFlowFormBatch,
  updateFlowFormStatus,
  getFlowFormTemplate,
  importFlowForm,
  exportFlowForm
} from '@/api/workflow/flowForm.api'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import type {
  FlowForm,
  FlowFormQuery,
  FlowFormCreate,
  FlowFormUpdate
} from '@/types/workflow/flowForm.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { RiEditLine, RiDeleteBinLine, RiBrushLine, RiPlayLine, RiStopLine } from '@remixicon/vue'

const { t } = useI18n()
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.flowform._self') })
)
const tenantStore = useTenantStore()
const userStore = useUserStore()
const loading = ref(false)
const queryKeyword = ref('')
const dataSource = ref<FlowForm[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selecmedRow = ref<FlowForm | null>(null)
const selecmedRows = ref<FlowForm[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formLoading = ref(false)
const formFormRef = ref<InstanceType<typeof FormForm> | null>(null)
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref<{ formCode: string; formName: string; formStatus: number | undefined }>({
  formCode: '',
  formName: '',
  formStatus: undefined
})
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
/** 导入弹窗可见 */
const importVisible = ref(false)
/** Excel 导入导出文件名 */
const excelNames = taktExcelEntityNames('TaktFlowForm')

type FlowFormColumn = {
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

function getColumnKey(column: FlowFormColumn): string {
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

const form = reactive<FlowFormCreate & { flowFormId?: string }>({
  tenantCode: '',
  companyCode: '',
  companyDefaultCulture: '',
  formCode: '',
  numberingRuleCode: '',
  formName: '',
  formCategory: 0,
  formType: 0,
  formConfig: '',
  formTemplate: '',
  formVersion: 'v1.0.0',
  isDatasource: 0,
  relatedDataBaseName: '',
  relatedTableName: '',
  relatedFormField: '',
  sortOrder: 0,
  formStatus: 0
})

/** 同步租户/公司隔离字段（与生成表单 applyScopeDefaults 一致） */
function syncFormScopeDefaults(force = false) {
  if (force || !form.tenantCode) form.tenantCode = tenantStore.tenantCode
  if (force || !form.companyCode) form.companyCode = tenantStore.companyCode
  if (force || !form.companyDefaultCulture) {
    form.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
  }
}

const getFormId = (record: unknown): string => {
  if (!record || typeof record !== 'object' || !('flowFormId' in record)) return ''
  const flowFormId = (record as { flowFormId?: unknown }).flowFormId
  return flowFormId != null ? String(flowFormId) : ''
}

/** 表格列：与 @/types/workflow/form FlowForm 字段一致（列表展示用，不含 formConfig/formTemplate 等大字段） */
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'flowFormId',
    key: 'id',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left'
  },
  {
    title: t('entity.flowform.formcode'),
    dataIndex: 'formCode',
    key: 'formCode',
    width: 140,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowform.formname'),
    dataIndex: 'formName',
    key: 'formName',
    width: 160,
    resizable: true,
    ellipsis: true
  },
  {
    title: t('entity.flowform.formcategory'),
    dataIndex: 'formCategory',
    key: 'formCategory',
    width: 100
  },
  {
    title: t('entity.flowform.formtype'),
    dataIndex: 'formType',
    key: 'formType',
    width: 120
  },
  {
    title: t('entity.flowform.formversion'),
    dataIndex: 'formVersion',
    key: 'formVersion',
    width: 90
  },
  {
    title: t('entity.flowform.isdatasource'),
    dataIndex: 'isDatasource',
    key: 'isDatasource',
    width: 80
  },
  {
    title: t('entity.flowform.sortorder'),
    dataIndex: 'sortOrder',
    key: 'sortOrder',
    width: 80
  },
  {
    title: t('entity.flowform.formstatus'),
    dataIndex: 'formStatus',
    key: 'formStatus',
    width: 90
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'workflow:form:update',
        onClick: (_record: FlowForm, _index: number) => handleEdit(_record)
      },
      {
        key: 'design',
        label: t('common.page.button.design'),
        shape: 'plain',
        icon: RiBrushLine,
        permission: 'workflow:form:design',
        onClick: (_record: FlowForm, _index: number) => handleDesign(_record)
      },
      {
        key: 'publish',
        label: t('common.page.button.publish'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'workflow:form:update',
        visible: (record: FlowForm) => record.formStatus !== 1,
        onClick: (record: FlowForm) => handleFormStatusChange(record, 1)
      },
      {
        key: 'disable',
        label: t('common.page.button.disable'),
        shape: 'plain',
        icon: RiStopLine,
        permission: 'workflow:form:update',
        visible: (record: FlowForm) => record.formStatus === 1,
        onClick: (record: FlowForm) => handleFormStatusChange(record, 2)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'workflow:form:delete',
        onClick: (_record: FlowForm, _index: number) => handleDeleteOne(_record)
      }
    ]
  })
])


const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: FlowForm[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: FlowForm, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getFormId(selecmedRow.value) === getFormId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: FlowForm[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: FlowForm) => ({
  onClick: () => {
    const key = getFormId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) selecmedRowKeys.value.splice(index, 1)
    else selecmedRowKeys.value.push(key)
    selecmedRows.value = dataSource.value.filter(item => selecmedRowKeys.value.includes(getFormId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
  }
})

/** 拉取流程表单列表（分页），结果写入 dataSource 与 total */
async function loadData() {
  try {
    loading.value = true
    const query: FlowFormQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value
    }
    const formCode = queryKeyword.value || advancedQueryForm.value.formCode
    const formName = queryKeyword.value || advancedQueryForm.value.formName
    if (formCode) query.formCode = formCode
    if (formName) query.formName = formName
    if (advancedQueryForm.value.formStatus != null) query.formStatus = advancedQueryForm.value.formStatus
    const res = (await getFlowFormList(query))
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

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置：清空关键词与高级查询条件，页码置 1 并重新拉取 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = { formCode: '', formName: '', formStatus: undefined }
  currentPage.value = 1
  loadData()
}

/** 打开高级查询弹窗 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：应用条件、页码置 1、拉取列表并关闭弹窗 */
function handleAdvancedQuerySubmit() {
  currentPage.value = 1
  loadData()
  advancedQueryVisible.value = false
}

/** 高级查询重置：清空高级查询表单 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = { formCode: '', formName: '', formStatus: undefined }
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

/** 打开导入弹窗 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getFlowFormTemplate(sheetName, fileName)
  return (res as { data?: Blob })?.data ?? res
}

/** 上传并导入 Excel */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importFlowForm(file, sheetName)
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

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const query: FlowFormQuery = {
      pageIndex: 1,
      pageSize: 10000
    }
    const formCode = queryKeyword.value || advancedQueryForm.value.formCode
    const formName = queryKeyword.value || advancedQueryForm.value.formName
    if (formCode) query.formCode = formCode
    if (formName) query.formName = formName
    if (advancedQueryForm.value.formStatus != null) query.formStatus = advancedQueryForm.value.formStatus
    const blob = await exportFlowForm(query, excelNames.sheet, excelNames.fileBase)
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
 * 更新表单发布状态
 * @param record 表单行
 * @param formStatus 目标状态（1 发布 / 2 停用）
 */
async function handleFormStatusChange(record: FlowForm, formStatus: number) {
  try {
    loading.value = true
    await updateFlowFormStatus({ flowFormId: record.flowFormId, formStatus })
    message.success(formStatus === 1 ? t('workflow.form.page.publish.success') : t('workflow.form.page.disable.success'))
    loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.failed')))
  } finally {
    loading.value = false
  }
}

/** 列宽拖拽后更新对应列的 width */
function handleResizeColumn(w: number, col: FlowFormColumn) {
  const column = columns.value.find((c) => getColumnKey(c as FlowFormColumn) === getColumnKey(col))
  if (column) (column as FlowFormColumn).width = w
}

/** 重置表单对象为初始值。仅用于「打开新增」前，不在关闭弹窗时调用。 */
function resetForm() {
  delete form.flowFormId
  form.formCode = ''
  form.numberingRuleCode = ''
  form.formName = ''
  form.formCategory = 0
  form.formType = 0
  form.formConfig = ''
  form.formTemplate = ''
  form.formVersion = 'v1.0.0'
  form.isDatasource = 0
  form.relatedDataBaseName = ''
  form.relatedTableName = ''
  form.relatedFormField = ''
  form.sortOrder = 0
  form.formStatus = 0
  syncFormScopeDefaults(true)
}

/** 新增：仅此时重置表单数据；打开弹窗后 nextTick 再重置子组件步骤与内部状态。 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.flowform._self') })
  resetForm()
  formVisible.value = true
  nextTick(() => formFormRef.value?.resetSteps?.())
}

/** 编辑：拉取详情回填 form，再打开弹窗，nextTick 后重置子组件步骤与内部状态（子组件会按 form 重新拉取数据源/表/列）。 */
async function handleEdit(record: FlowForm) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.flowform._self') })
  formLoading.value = true
  try {
    const detail = await getFlowFormById(String(record.flowFormId))
    form.flowFormId = detail.flowFormId
    form.tenantCode = detail.tenantCode
    form.companyCode = detail.companyCode
    form.companyDefaultCulture = userStore.userInfo?.companyDefaultCulture ?? ''
    form.formCode = detail.formCode
    form.formName = detail.formName
    form.formCategory = detail.formCategory
    form.formType = detail.formType
    form.formConfig = detail.formConfig?.trim() ? detail.formConfig : ''
    form.formTemplate = detail.formTemplate ?? ''
    form.formVersion = detail.formVersion
    form.isDatasource = detail.isDatasource
    form.relatedDataBaseName = detail.relatedDataBaseName ?? ''
    form.relatedTableName = detail.relatedTableName ?? ''
    form.relatedFormField = detail.relatedFormField ?? ''
    form.sortOrder = detail.sortOrder
    form.formStatus = detail.formStatus
    formVisible.value = true
    nextTick(() => formFormRef.value?.resetSteps?.())
  } catch {
    message.error(t('workflow.form.page.load.detail.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 设计：复用编辑逻辑打开表单设计 */
function handleDesign(record: FlowForm) {
  handleEdit(record)
}

/** 更新：若有选中行则编辑该行，否则提示请选择 */
function handleUpdate() {
  if (selecmedRow.value) handleEdit(selecmedRow.value)
  else message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.flowform._self') }))
}

/** 单条删除：二次确认后调用 deleteById 并刷新列表 */
function handleDeleteOne(record: FlowForm) {
  const name = record.formName || getFormId(record)
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.flowform._self'), name }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteFlowFormById(record.flowFormId)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.flowform._self') }))
    return
  }
  Modal.confirm({
    centered: true,
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { count: selecmedRows.value.length, entity: t('entity.flowform._self') }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      try {
        loading.value = true
        await deleteFlowFormBatch(selecmedRowKeys.value.map(k => String(k)))
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

/** 关闭表单弹窗：不在此处重置表单或步骤，下次打开为新增会 resetForm + resetSteps，为编辑会回填 + resetSteps。 */
function handleFormCancel() {
  formVisible.value = false
}

/** 提交：校验步骤、同步设计器、调用 create/update 后关闭弹窗并刷新列表 */
async function handleFormSubmit() {
  const valid = await formFormRef.value?.validateAllSteps?.()
  if (valid === false) {
    message.warning(t('workflow.form.page.step.complete.required'))
    return
  }
  try {
    formLoading.value = true
    formFormRef.value?.syncDesignerToModel()
    syncFormScopeDefaults(true)
    const payload: FlowFormCreate = {
      tenantCode: form.tenantCode,
      companyCode: form.companyCode,
      companyDefaultCulture: form.companyDefaultCulture,
      formCode: form.formCode.trim(),
      numberingRuleCode: form.numberingRuleCode?.trim() || undefined,
      formName: form.formName.trim(),
      formCategory: form.formCategory,
      formType: form.formType,
      formConfig: form.formConfig?.trim() || undefined,
      formTemplate: form.formTemplate?.trim() || undefined,
      formVersion: form.formVersion?.trim() || 'v1.0.0',
      isDatasource: form.isDatasource,
      relatedDataBaseName: form.relatedDataBaseName?.trim() || undefined,
      relatedTableName: form.relatedTableName?.trim() || undefined,
      relatedFormField: form.relatedFormField?.trim() || undefined,
      sortOrder: form.sortOrder ?? 0,
      formStatus: form.formStatus
    }
    if (form.flowFormId) {
      const { numberingRuleCode: _omitRule, ...updateBody } = payload
      await updateFlowForm(form.flowFormId, { ...updateBody, flowFormId: form.flowFormId } as FlowFormUpdate)
      message.success(t('common.feedback.updated'))
    } else {
      await createFlowForm(payload)
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

onMounted(() => {
  loadData()
})
</script>

<style scoped lang="css">
.workflow-form {
  padding: 16px;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/backup-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：备份日志只读审计页，含查询、详情、删除、导出（无新增/编辑），对齐 delta-log -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      delete-permission="statistics:logging:backup:log:delete"
      export-permission="statistics:logging:backup:log:export"
      :show-create="false"
      :show-update="false"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'backupLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getBackupLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 详情对话框 -->
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: pi.self() })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <BackupLogDetail :detail="detailData" />
      </a-spin>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-statistics-logging-backup-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('backupKind')">
      <a-form-item :label="pi.queryLabel('backupKind')">
        <a-input
          v-model:value="advancedQueryForm.backupKind"
          :placeholder="pi.queryPh('backupKind', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceId')">
      <a-form-item :label="pi.queryLabel('sourceId')">
        <a-input
          v-model:value="advancedQueryForm.sourceId"
          :placeholder="pi.queryPh('sourceId', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCode')">
      <a-form-item :label="pi.queryLabel('sourceCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceCode"
          :placeholder="pi.queryPh('sourceCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetName')">
      <a-form-item :label="pi.queryLabel('targetName')">
        <a-input
          v-model:value="advancedQueryForm.targetName"
          :placeholder="pi.queryPh('targetName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetScope')">
      <a-form-item :label="pi.queryLabel('targetScope')">
        <a-textarea
          v-model:value="advancedQueryForm.targetScope"
          :placeholder="pi.queryPh('targetScope', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('syncMode')">
      <a-form-item :label="pi.queryLabel('syncMode')">
        <a-input-number
          v-model:value="advancedQueryForm.syncMode"
          :placeholder="pi.queryPh('syncMode', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executeMode')">
      <a-form-item :label="pi.queryLabel('executeMode')">
        <a-input-number
          v-model:value="advancedQueryForm.executeMode"
          :placeholder="pi.queryPh('executeMode', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pathType')">
      <a-form-item :label="pi.queryLabel('pathType')">
        <a-input-number
          v-model:value="advancedQueryForm.pathType"
          :placeholder="pi.queryPh('pathType', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resultPath')">
      <a-form-item :label="pi.queryLabel('resultPath')">
        <a-input
          v-model:value="advancedQueryForm.resultPath"
          :placeholder="pi.queryPh('resultPath', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileSizeBytes')">
      <a-form-item :label="pi.queryLabel('fileSizeBytes')">
        <a-input
          v-model:value="advancedQueryForm.fileSizeBytes"
          :placeholder="pi.queryPh('fileSizeBytes', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('runStatus')">
      <a-form-item :label="pi.queryLabel('runStatus')">
        <a-input-number
          v-model:value="advancedQueryForm.runStatus"
          :placeholder="pi.queryPh('runStatus', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('errorMessage')">
      <a-form-item :label="pi.queryLabel('errorMessage')">
        <a-input
          v-model:value="advancedQueryForm.errorMessage"
          :placeholder="pi.queryPh('errorMessage', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startedAtStart')">
      <a-form-item :label="pi.queryLabel('startedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.startedAtStart"
          :placeholder="pi.queryPh('startedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startedAtEnd')">
      <a-form-item :label="pi.queryLabel('startedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.startedAtEnd"
          :placeholder="pi.queryPh('startedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('finishedAtStart')">
      <a-form-item :label="pi.queryLabel('finishedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.finishedAtStart"
          :placeholder="pi.queryPh('finishedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('finishedAtEnd')">
      <a-form-item :label="pi.queryLabel('finishedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.finishedAtEnd"
          :placeholder="pi.queryPh('finishedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'backupLogId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 备份日志管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/backup-log
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import BackupLogDetail from './components/BackupLogDetail.vue'
import { getBackupLogList, getBackupLogById, deleteBackupLogById, deleteBackupLogBatch, exportBackupLog } from '@/api/statistics/logging/backupLog.api'
import type { BackupLog, BackupLogQuery } from '@/types/statistics/logging/backupLog.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEyeLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useBackupLogI18n,
  BACKUPLOG_LIST_FIELDS,
  BACKUPLOG_QUERY_STRING_FIELDS,
  BACKUPLOG_QUERY_FIELDS,
  BACKUPLOG_SELF_I18N_KEY,
} from './composables/useBackupLogI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBackupLogI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type BackupLogRowRecord = BackupLog | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBackupLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BackupLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BackupLogRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<BackupLogRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据 */
const detailData = ref<BackupLog | null>(null)

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 创建空的高级查询表单
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(BACKUPLOG_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof BACKUPLOG_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    syncMode: undefined as number | undefined,
    executeMode: undefined as number | undefined,
    pathType: undefined as number | undefined,
    runStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  BACKUPLOG_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'backupLogId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BackupLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BackupLogQuery>): BackupLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BackupLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BackupLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of BACKUPLOG_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.syncMode !== undefined && form.syncMode !== null) {
    query.syncMode = form.syncMode
  }
  if (form.executeMode !== undefined && form.executeMode !== null) {
    query.executeMode = form.executeMode
  }
  if (form.pathType !== undefined && form.pathType !== null) {
    query.pathType = form.pathType
  }
  if (form.runStatus !== undefined && form.runStatus !== null) {
    query.runStatus = form.runStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildBackupLogListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  buildBackupLogListColumn('backupLogId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...BACKUPLOG_LIST_FIELDS.map((key) => buildBackupLogListColumn(key, pi.label(key))),
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'statistics:logging:backup:log:query',
        onClick: (record: BackupLogRowRecord) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:backup:log:delete',
        onClick: (record: BackupLogRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getBackupLogId = (record: BackupLogRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BackupLogRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BackupLogRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getBackupLogId(selectedRow.value) === getBackupLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BackupLogRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: BackupLogRowRecord) => ({
  onClick: () => {
    const key = getBackupLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getBackupLogId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getBackupLogList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[BackupLog] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开详情弹窗 */
async function handleShowDetail(record: BackupLogRowRecord) {
  const id = getBackupLogId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getBackupLogById(id)
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    detailVisible.value = false
  } finally {
    detailLoading.value = false
  }
}

/** 关闭详情弹窗 */
function handleDetailClose() {
  detailVisible.value = false
  detailData.value = null
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportBackupLog(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[BackupLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BackupLogRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBackupLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteBackupLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/database/databaseBackup.types -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：标准 CRUD 列表；工具栏与操作列均含更新/删除；操作列另含立即/后台执行 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
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
      create-permission="code:database:backup:create"
      update-permission="code:database:backup:update"
      delete-permission="code:database:backup:delete"
      export-permission="code:database:backup:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
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
      :id-column-key="entityIdName"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getDatabaseBackupId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      :pagination="false"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'backupType'">
          {{ formatBackupType(record.backupType) }}
        </template>
        <template v-else-if="column.key === 'backupPathType'">
          {{ formatBackupPathType(record.backupPathType) }}
        </template>
        <template v-else-if="column.key === 'executeMode'">
          {{ formatExecuteMode(record.executeMode) }}
        </template>
        <template v-else-if="column.key === 'backupStatus'">
          {{ formatBackupStatus(record.backupStatus) }}
        </template>
      </template>
    </TaktSingleTable>

    <!-- 分页 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <BackupForm
        v-if="formVisible"
        :key="formData?.databaseBackupId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 高级查询 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-code-database-database-backup'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('backupCode')">
          <a-form-item :label="pi.queryLabel('backupCode')">
            <a-input
              v-model:value="advancedQueryForm.backupCode"
              :placeholder="pi.queryPh('backupCode', 'required')"
              show-count
              :maxlength="40"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('targetTenantCode')">
          <a-form-item :label="pi.queryLabel('targetTenantCode')">
            <a-input
              v-model:value="advancedQueryForm.targetTenantCode"
              :placeholder="pi.queryPh('targetTenantCode', 'required')"
              show-count
              :maxlength="3"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('targetDatabaseName')">
          <a-form-item :label="pi.queryLabel('targetDatabaseName')">
            <a-input
              v-model:value="advancedQueryForm.targetDatabaseName"
              :placeholder="pi.queryPh('targetDatabaseName', 'required')"
              show-count
              :maxlength="40"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('backupType')">
          <a-form-item :label="pi.queryLabel('backupType')">
            <a-input-number
              v-model:value="advancedQueryForm.backupType"
              :placeholder="pi.queryPh('backupType', 'select')"
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('executeMode')">
          <a-form-item :label="pi.queryLabel('executeMode')">
            <a-input-number
              v-model:value="advancedQueryForm.executeMode"
              :placeholder="pi.queryPh('executeMode', 'select')"
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('backupPath')">
          <a-form-item :label="pi.queryLabel('backupPath')">
            <a-input
              v-model:value="advancedQueryForm.backupPath"
              :placeholder="pi.queryPh('backupPath', 'required')"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('backupStatus')">
          <a-form-item :label="pi.queryLabel('backupStatus')">
            <a-input-number
              v-model:value="advancedQueryForm.backupStatus"
              :placeholder="pi.queryPh('backupStatus', 'select')"
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('scheduledAtStart')">
          <a-form-item :label="pi.queryLabel('scheduledAtStart')">
            <a-date-picker
              v-model:value="advancedQueryForm.scheduledAtStart"
              :placeholder="pi.queryPh('scheduledAtStart', 'select')"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('scheduledAtEnd')">
          <a-form-item :label="pi.queryLabel('scheduledAtEnd')">
            <a-date-picker
              v-model:value="advancedQueryForm.scheduledAtEnd"
              :placeholder="pi.queryPh('scheduledAtEnd', 'select')"
              value-format="YYYY-MM-DD HH:mm:ss"
              show-time
              class="w-full"
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('remark')">
          <a-form-item :label="pi.queryLabel('remark')">
            <a-textarea
              v-model:value="advancedQueryForm.remark"
              :placeholder="pi.queryPh('remark', 'optional')"
              :rows="3"
              show-count
              :maxlength="400"
              allow-clear
            />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="entityIdName"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 后台执行 -->
    <TaktModal
      v-model:open="scheduleModalVisible"
      :title="t('code.database.database-backup.page.button.schedule')"
      :confirm-loading="scheduleSubmitLoading"
      @ok="handleScheduleSubmit"
      @cancel="handleScheduleCancel"
    >
      <a-form layout="vertical">
        <a-form-item :label="t('code.database.database-backup.page.field.scheduledat')" required>
          <a-date-picker
            v-model:value="scheduleAtValue"
            show-time
            class="w-full"
            format="YYYY-MM-DD HH:mm:ss"
            value-format="YYYY-MM-DD HH:mm:ss"
          />
        </a-form-item>
        <a-typography-text type="secondary">
          {{ t('code.database.database-backup.page.tip.schedule') }}
        </a-typography-text>
      </a-form>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 数据库备份标准 CRUD 列表页
 * @module views/code/database/databaseBackup.types
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { RiEditLine, RiDeleteBinLine, RiPlayLine, RiTimerLine } from '@remixicon/vue'
import {
  createDatabaseBackup,
  deleteDatabaseBackupById,
  deleteDatabaseBackupBatch,
  exportDatabaseBackup,
  getDatabaseBackupById,
  getDatabaseBackupList,
  runDatabaseBackupById,
  scheduleDatabaseBackupById,
  updateDatabaseBackup,
} from '@/api/code/database/databaseBackup.api'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import type { DatabaseBackup, DatabaseBackupQuery } from '@/types/code/database/backup.types'
import BackupForm from './components/BackupForm.vue'
import {
  useDatabaseBackupI18n,
  DATABASEBACKUP_LIST_FIELDS,
  DATABASEBACKUP_QUERY_FIELDS,
  DATABASEBACKUP_QUERY_STRING_FIELDS,
} from './composables/useBackupI18n'

/** 实体字段 i18n */
const pi = useDatabaseBackupI18n()
/** 表格行类型 */
type DatabaseBackupRowRecord = DatabaseBackup | Record<string, unknown>
/** i18n */
const { t } = useI18n()
/** Excel 导出默认命名 */
const excelNames = taktExcelEntityNames('TaktDatabaseBackup')
/** 列表快捷查询占位 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)
/** 实体主键字段名 */
const entityIdName = 'databaseBackupId'

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<DatabaseBackup[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选行 */
const selectedRow = ref<DatabaseBackupRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<DatabaseBackupRowRecord[]>([])
/** 表格多选 row-key */
const selectedRowKeys = ref<(string | number)[]>([])
/** 新增/编辑弹窗 */
const formVisible = ref(false)
/** 弹窗标题 */
const formTitle = ref('')
/** 传入表单的数据 */
const formData = ref<Partial<DatabaseBackup> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 表单 ref */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false)
/** 列设置抽屉 */
const columnSettingVisible = ref(false)
/** 表格可见列 */
const visibleColumnKeys = ref<string[]>([])
/** 高级查询可见字段 */
const visibleQueryFieldKeys = ref<string[]>([])
/** 后台执行弹窗 */
const scheduleModalVisible = ref(false)
/** 后台执行时间 */
const scheduleAtValue = ref<string | null>(null)
/** 后台执行目标 Id */
const scheduleTargetId = ref<string | null>(null)
/** 后台执行提交 loading */
const scheduleSubmitLoading = ref(false)
/** 行内立即执行 loading Id */
const runLoadingId = ref<string | null>(null)

/**
 * 创建空高级查询表单
 * @returns 初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(
    DATABASEBACKUP_QUERY_STRING_FIELDS.map((key) => [key, ''])
  ) as Record<(typeof DATABASEBACKUP_QUERY_STRING_FIELDS)[number], string>
  return {
    ...form,
    backupType: undefined as number | undefined,
    executeMode: undefined as number | undefined,
    backupStatus: undefined as number | undefined,
  }
}

/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  DATABASEBACKUP_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) }))
)
/** 工具栏编辑禁用 */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏删除禁用 */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询
 * @param overrides 覆盖项
 * @returns 查询 DTO
 */
function buildListQuery(overrides?: Partial<DatabaseBackupQuery>): DatabaseBackupQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: DatabaseBackupQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  for (const key of DATABASEBACKUP_QUERY_STRING_FIELDS) {
    const v = (form[key] ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  if (form.backupType !== undefined && form.backupType !== null) {
    query.backupType = form.backupType
  }
  if (form.backupPathType !== undefined && form.backupPathType !== null) {
    query.backupPathType = form.backupPathType
  }
  if (form.executeMode !== undefined && form.executeMode !== null) {
    query.executeMode = form.executeMode
  }
  if (form.backupStatus !== undefined && form.backupStatus !== null) {
    query.backupStatus = form.backupStatus
  }
  return query
}

/**
 * 构建标准文本列
 * @param key 列 key
 * @param title 标题
 * @param options 宽度等
 */
function buildListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' }
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

/** 表格列 */
const columns = computed<TableColumnsType>(() => [
  buildListColumn(entityIdName, t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...DATABASEBACKUP_LIST_FIELDS.map((key) => buildListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'code:database:backup:update',
        onClick: (record: DatabaseBackupRowRecord) => handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'code:database:backup:delete',
        onClick: (record: DatabaseBackupRowRecord) => handleDeleteOne(record),
      },
      {
        key: 'run-now',
        label: t('code.database.database-backup.page.button.runnow'),
        shape: 'plain',
        icon: RiPlayLine,
        permission: 'code:database:backup:run',
        loadingFn: (record: DatabaseBackupRowRecord) =>
          runLoadingId.value === getDatabaseBackupId(record),
        onClick: (record: DatabaseBackupRowRecord) => handleRunNow(record),
      },
      {
        key: 'schedule',
        label: t('code.database.database-backup.page.button.schedule'),
        shape: 'plain',
        icon: RiTimerLine,
        permission: 'code:database:backup:schedule',
        onClick: (record: DatabaseBackupRowRecord) => openScheduleModal(record),
      }],
  })])

/**
 * 表格 row-key
 * @param record 行数据
 */
function getDatabaseBackupId(record: DatabaseBackupRowRecord): string {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}

/**
 * 备份类型文案
 * @param value 类型码
 */
function formatBackupType(value: unknown): string {
  return Number(value) === 2
    ? t('code.database.database-backup.page.backuptype.delta')
    : t('code.database.database-backup.page.backuptype.full')
}

/**
 * 路径类型文案（1=本地服务器 4=客户端 2=文件服务器 3=FTP）
 * @param value 类型码
 * @returns {string} 文案
 */
function formatBackupPathType(value: unknown): string {
  switch (Number(value)) {
    case 4:
      return t('code.database.database-backup.page.pathtype.client')
    case 2:
      return t('code.database.database-backup.page.pathtype.network')
    case 3:
      return t('code.database.database-backup.page.pathtype.ftp')
    default:
      return t('code.database.database-backup.page.pathtype.local')
  }
}

/**
 * 执行方式文案
 * @param value 方式码
 */
function formatExecuteMode(value: unknown): string {
  return Number(value) === 2
    ? t('code.database.database-backup.page.executemode.background')
    : t('code.database.database-backup.page.executemode.immediate')
}

/**
 * 备份状态文案
 * @param value 状态码
 */
function formatBackupStatus(value: unknown): string {
  switch (Number(value)) {
    case 1:
      return t('code.database.database-backup.page.status.running')
    case 2:
      return t('code.database.database-backup.page.status.success')
    case 3:
      return t('code.database.database-backup.page.status.failed')
    case 4:
      return t('code.database.database-backup.page.status.scheduled')
    default:
      return t('code.database.database-backup.page.status.pending')
  }
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DatabaseBackupRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DatabaseBackupRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (
      selectedRow.value &&
      getDatabaseBackupId(selectedRow.value) === getDatabaseBackupId(record)
    ) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: DatabaseBackupRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/** 行点击切换选中 */
const onClickRow = (record: DatabaseBackupRowRecord) => ({
  onClick: () => {
    const key = getDatabaseBackupId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) =>
      selectedRowKeys.value.includes(getDatabaseBackupId(item))
    )
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  },
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getDatabaseBackupList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[DatabaseBackup] load failed', { error })
    message.error(t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/**
 * 打开编辑
 * @param record 当前行
 */
async function handleEdit(record: DatabaseBackupRowRecord) {
  const id = getDatabaseBackupId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getDatabaseBackupById(id)
    formData.value = detail ?? ({ ...record } as Partial<DatabaseBackup>)
    formVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() })
    )
  }
}

/** 提交新增/编辑 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) {
    return
  }
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.databaseBackupId
    if (id) {
      await updateDatabaseBackup(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createDatabaseBackup(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
    nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭表单弹窗 */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

/** 导出 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportDatabaseBackup(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? exportMeta
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
  } catch (error: unknown) {
    logger.error('[DatabaseBackup] export failed', { error })
    message.error(t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}

/** 批量删除 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(
      t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() })
    )
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value
        .map((row) => getDatabaseBackupId(row))
        .filter(Boolean)
      await deleteDatabaseBackupBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    },
  })
}

/**
 * 行内单条删除
 * @param record 当前行
 */
function handleDeleteOne(record: DatabaseBackupRowRecord) {
  const id = getDatabaseBackupId(record)
  if (!id) {
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDatabaseBackupById(id)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    },
  })
}

/**
 * 立即执行
 * @param record 当前行
 */
async function handleRunNow(record: DatabaseBackupRowRecord) {
  const id = getDatabaseBackupId(record)
  if (!id) {
    return
  }
  runLoadingId.value = id
  try {
    await runDatabaseBackupById(id)
    message.success(t('code.database.database-backup.page.message.runsuccess'))
    loadData()
  } catch (error: unknown) {
    logger.error('[DatabaseBackup] run failed', { error, id })
  } finally {
    runLoadingId.value = null
  }
}

/**
 * 打开后台执行弹窗
 * @param record 当前行
 */
function openScheduleModal(record: DatabaseBackupRowRecord) {
  const id = getDatabaseBackupId(record)
  if (!id) {
    return
  }
  scheduleTargetId.value = id
  scheduleAtValue.value = null
  scheduleModalVisible.value = true
}

/** 关闭后台执行弹窗 */
function handleScheduleCancel() {
  scheduleModalVisible.value = false
  scheduleTargetId.value = null
  scheduleAtValue.value = null
}

/**
 * 校验调度时间
 * @returns 是否通过
 */
function validateScheduleAt(): boolean {
  if (!scheduleAtValue.value) {
    message.warning(t('code.database.database-backup.page.message.schedulerequired'))
    return false
  }
  const when = new Date(scheduleAtValue.value.replace(/-/g, '/'))
  if (Number.isNaN(when.getTime()) || when.getTime() <= Date.now()) {
    message.warning(t('code.database.database-backup.page.message.schedulefuture'))
    return false
  }
  return true
}

/** 提交后台执行 */
async function handleScheduleSubmit() {
  if (!scheduleTargetId.value || !validateScheduleAt()) {
    return
  }
  scheduleSubmitLoading.value = true
  try {
    await scheduleDatabaseBackupById(scheduleTargetId.value, {
      scheduledAt: scheduleAtValue.value!,
    })
    message.success(t('code.database.database-backup.page.message.schedulesuccess'))
    scheduleModalVisible.value = false
    scheduleTargetId.value = null
    scheduleAtValue.value = null
    loadData()
  } catch (error: unknown) {
    logger.error('[DatabaseBackup] schedule failed', { error })
  } finally {
    scheduleSubmitLoading.value = false
  }
}

/** 打开高级查询 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 高级查询重置 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/** 打开列设置 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 更新可见列 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置恢复默认 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}

/** 列宽拖拽占位 */
function handleResizeColumn() {}

/**
 * 分页变更
 * @param page 页码
 * @param size 页大小
 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/**
 * 页大小变更
 * @param _current 当前页
 * @param size 页大小
 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})
</script>

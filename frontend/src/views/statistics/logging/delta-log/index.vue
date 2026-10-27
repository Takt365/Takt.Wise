<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/delta-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：差异日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      delete-permission="statistics:logging:delta:log:delete"
      export-permission="statistics:logging:delta:log:export"
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
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'deltaLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getDeltaLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'operType'">
          <TaktConstTag
            category="operType"
            :value="getDeltaLogField(record, 'operType')"
          />
        </template>
      </template>
    </TaktSingleTable>

    <!-- 分页组件 -->
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
      :title="t('common.dialog.title.detail', { entity: t('entity.deltalog._self') })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <DeltaLogDetail :detail="detailData" />
      </a-spin>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-statistics-logging-delta-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.deltalog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.username') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operType')">
      <a-form-item :label="t('entity.deltalog.opertype')">
        <TaktSelect
          v-model:value="advancedQueryForm.operType"
          :options="operTypeOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.deltalog.opertype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tableName')">
      <a-form-item :label="t('entity.deltalog.tablename')">
        <a-input
          v-model:value="advancedQueryForm.tableName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.tablename') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('primaryKeyId')">
      <a-form-item :label="t('entity.deltalog.primarykeyid')">
        <a-input
          v-model:value="advancedQueryForm.primaryKeyId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.primarykeyid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('beforeData')">
      <a-form-item :label="t('entity.deltalog.beforedata')">
        <a-input
          v-model:value="advancedQueryForm.beforeData"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.beforedata') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('afterData')">
      <a-form-item :label="t('entity.deltalog.afterdata')">
        <a-input
          v-model:value="advancedQueryForm.afterData"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.afterdata') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('diffData')">
      <a-form-item :label="t('entity.deltalog.diffdata')">
        <a-input
          v-model:value="advancedQueryForm.diffData"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.diffdata') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sqlStatement')">
      <a-form-item :label="t('entity.deltalog.sqlstatement')">
        <a-input
          v-model:value="advancedQueryForm.sqlStatement"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.sqlstatement') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operIp')">
      <a-form-item :label="t('entity.deltalog.operip')">
        <a-input
          v-model:value="advancedQueryForm.operIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.operip') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operLocation')">
      <a-form-item :label="t('entity.deltalog.operlocation')">
        <a-input
          v-model:value="advancedQueryForm.operLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.operlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operTimeStart')">
      <a-form-item :label="t('entity.deltalog.opertimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.operTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.deltalog.opertimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operTimeEnd')">
      <a-form-item :label="t('entity.deltalog.opertimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.operTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.deltalog.opertimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('elapsedTime')">
      <a-form-item :label="t('entity.deltalog.elapsedtime')">
        <a-input-number
          v-model:value="advancedQueryForm.elapsedTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.deltalog.elapsedtime') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.extfield')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfield') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
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
      :id-column-key="'deltaLogId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { operTypeOptions } from '@/constants/takt.constants'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 差异日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/delta-log
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import DeltaLogDetail from './components/DeltaLogDetail.vue'
import { getDeltaLogList, getDeltaLogById, deleteDeltaLogById, deleteDeltaLogBatch, exportDeltaLog } from '@/api/statistics/logging/deltaLog.api'
import type { DeltaLog, DeltaLogQuery } from '@/types/statistics/logging/deltaLog.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEyeLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktDeltaLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.deltalog._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<DeltaLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<DeltaLog | null>(null)
/** 表格多选行 */
const selecmedRows = ref<DeltaLog[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  userName: '',
  operType: undefined as string | undefined,
  tableName: '',
  primaryKeyId: '',
  beforeData: '',
  afterData: '',
  diffData: '',
  sqlStatement: '',
  operIp: '',
  operLocation: '',
  operTimeStart: '',
  operTimeEnd: '',
  elapsedTime: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'userName', label: t('entity.deltalog.username') },
  { key: 'operType', label: t('entity.deltalog.opertype') },
  { key: 'tableName', label: t('entity.deltalog.tablename') },
  { key: 'primaryKeyId', label: t('entity.deltalog.primarykeyid') },
  { key: 'beforeData', label: t('entity.deltalog.beforedata') },
  { key: 'afterData', label: t('entity.deltalog.afterdata') },
  { key: 'diffData', label: t('entity.deltalog.diffdata') },
  { key: 'sqlStatement', label: t('entity.deltalog.sqlstatement') },
  { key: 'operIp', label: t('entity.deltalog.operip') },
  { key: 'operLocation', label: t('entity.deltalog.operlocation') },
  { key: 'operTimeStart', label: t('entity.deltalog.opertimestart') },
  { key: 'operTimeEnd', label: t('entity.deltalog.opertimeend') },
  { key: 'elapsedTime', label: t('entity.deltalog.elapsedtime') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'deltaLogId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)
/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据 */
const detailData = ref<DeltaLog | null>(null)

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'deltaLogId',
    key: 'deltaLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'deltaLogId') ?? ''
  },
  {
    title: t('entity.deltalog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.deltalog.opertype'),
    dataIndex: 'operType',
    key: 'operType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.deltalog.tablename'),
    dataIndex: 'tableName',
    key: 'tableName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'tableName') ?? ''
  },
  {
    title: t('entity.deltalog.primarykeyid'),
    dataIndex: 'primaryKeyId',
    key: 'primaryKeyId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'primaryKeyId') ?? ''
  },
  {
    title: t('entity.deltalog.beforedata'),
    dataIndex: 'beforeData',
    key: 'beforeData',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'beforeData') ?? ''
  },
  {
    title: t('entity.deltalog.afterdata'),
    dataIndex: 'afterData',
    key: 'afterData',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'afterData') ?? ''
  },
  {
    title: t('entity.deltalog.diffdata'),
    dataIndex: 'diffData',
    key: 'diffData',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'diffData') ?? ''
  },
  {
    title: t('entity.deltalog.sqlstatement'),
    dataIndex: 'sqlStatement',
    key: 'sqlStatement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'sqlStatement') ?? ''
  },
  {
    title: t('entity.deltalog.operip'),
    dataIndex: 'operIp',
    key: 'operIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'operIp') ?? ''
  },
  {
    title: t('entity.deltalog.operlocation'),
    dataIndex: 'operLocation',
    key: 'operLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'operLocation') ?? ''
  },
  {
    title: t('entity.deltalog.opertime'),
    dataIndex: 'operTime',
    key: 'operTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'operTime') ?? ''
  },
  {
    title: t('entity.deltalog.elapsedtime'),
    dataIndex: 'elapsedTime',
    key: 'elapsedTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDeltaLogField(record, 'elapsedTime') ?? ''
  },
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'statistics:logging:delta:log:query',
        onClick: (record: DeltaLog) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:delta:log:delete',
        onClick: (record: DeltaLog) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getDeltaLogId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getDeltaLogField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: DeltaLog[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: DeltaLog, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getDeltaLogId(selecmedRow.value) === getDeltaLogId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: DeltaLog[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: DeltaLog) => ({
  onClick: () => {
    const key = getDeltaLogId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getDeltaLogId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: DeltaLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getDeltaLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[DeltaLog] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 打开详情弹窗 */
async function handleShowDetail(record: DeltaLog) {
  const id = getDeltaLogId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getDeltaLogById(id)
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

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  userName: '',
  operType: undefined as string | undefined,
  tableName: '',
  primaryKeyId: '',
  beforeData: '',
  afterData: '',
  diffData: '',
  sqlStatement: '',
  operIp: '',
  operLocation: '',
  operTimeStart: '',
  operTimeEnd: '',
  elapsedTime: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: DeltaLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportDeltaLog(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.deltalog._self') }))
  } catch (error: any) {
    logger.error('[DeltaLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.deltalog._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: DeltaLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.deltalog._self'), name: t('common.tip.this.target', { target: t('entity.deltalog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDeltaLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.deltalog._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.deltalog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.deltalog._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteDeltaLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.deltalog._self') }))
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
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  userName: '',
  operType: undefined as string | undefined,
  tableName: '',
  primaryKeyId: '',
  beforeData: '',
  afterData: '',
  diffData: '',
  sqlStatement: '',
  operIp: '',
  operLocation: '',
  operTimeStart: '',
  operTimeEnd: '',
  elapsedTime: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
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
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

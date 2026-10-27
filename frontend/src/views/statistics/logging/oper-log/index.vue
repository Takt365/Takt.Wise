<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/oper-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：操作日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      delete-permission="statistics:logging:oper:log:delete"
      export-permission="statistics:logging:oper:log:export"
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
      :id-column-key="'operLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getOperLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'operType'">
          <TaktConstTag
            category="operType"
            :value="getOperLogField(record, 'operType')"
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
      :title="t('common.dialog.title.detail', { entity: t('entity.operlog._self') })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <OperLogDetail :detail="detailData" />
      </a-spin>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-statistics-logging-oper-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.operlog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.username') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operModule')">
      <a-form-item :label="t('entity.operlog.opermodule')">
        <a-input
          v-model:value="advancedQueryForm.operModule"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.opermodule') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operType')">
      <a-form-item :label="t('entity.operlog.opertype')">
        <TaktSelect
          v-model:value="advancedQueryForm.operType"
          :options="operTypeOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operMethod')">
      <a-form-item :label="t('entity.operlog.opermethod')">
        <a-input
          v-model:value="advancedQueryForm.operMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.opermethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestMethod')">
      <a-form-item :label="t('entity.operlog.requestmethod')">
        <a-input
          v-model:value="advancedQueryForm.requestMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.requestmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operUrl')">
      <a-form-item :label="t('entity.operlog.operurl')">
        <a-input
          v-model:value="advancedQueryForm.operUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operurl') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestParam')">
      <a-form-item :label="t('entity.operlog.requestparam')">
        <a-input
          v-model:value="advancedQueryForm.requestParam"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.requestparam') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jsonResult')">
      <a-form-item :label="t('entity.operlog.jsonresult')">
        <a-input
          v-model:value="advancedQueryForm.jsonResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.jsonresult') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operStatus')">
      <a-form-item :label="t('entity.operlog.operstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.operStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('errorMsg')">
      <a-form-item :label="t('entity.operlog.errormsg')">
        <a-input
          v-model:value="advancedQueryForm.errorMsg"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.errormsg') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operIp')">
      <a-form-item :label="t('entity.operlog.operip')">
        <a-input
          v-model:value="advancedQueryForm.operIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operip') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operLocation')">
      <a-form-item :label="t('entity.operlog.operlocation')">
        <a-input
          v-model:value="advancedQueryForm.operLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.operlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operTimeStart')">
      <a-form-item :label="t('entity.operlog.opertimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.operTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operTimeEnd')">
      <a-form-item :label="t('entity.operlog.opertimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.operTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.operlog.opertimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('elapsedTime')">
      <a-form-item :label="t('entity.operlog.elapsedtime')">
        <a-input-number
          v-model:value="advancedQueryForm.elapsedTime"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.operlog.elapsedtime') })"
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
      :id-column-key="'operLogId'"
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
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
/**
 * 操作日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/oper-log
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import OperLogDetail from './components/OperLogDetail.vue'
import { getOperLogList, getOperLogById, deleteOperLogById, deleteOperLogBatch, exportOperLog } from '@/api/statistics/logging/operLog.api'
import type { OperLog, OperLogQuery } from '@/types/statistics/logging/operLog.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEyeLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktOperLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.operlog._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<OperLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<OperLog | null>(null)
/** 表格多选行 */
const selecmedRows = ref<OperLog[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  userName: '',
  operModule: '',
  operType: undefined as string | undefined,
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
  operStatus: undefined as number | undefined,
  errorMsg: '',
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
  { key: 'userName', label: t('entity.operlog.username') },
  { key: 'operModule', label: t('entity.operlog.opermodule') },
  { key: 'operType', label: t('entity.operlog.opertype') },
  { key: 'operMethod', label: t('entity.operlog.opermethod') },
  { key: 'requestMethod', label: t('entity.operlog.requestmethod') },
  { key: 'operUrl', label: t('entity.operlog.operurl') },
  { key: 'requestParam', label: t('entity.operlog.requestparam') },
  { key: 'jsonResult', label: t('entity.operlog.jsonresult') },
  { key: 'operStatus', label: t('entity.operlog.operstatus') },
  { key: 'errorMsg', label: t('entity.operlog.errormsg') },
  { key: 'operIp', label: t('entity.operlog.operip') },
  { key: 'operLocation', label: t('entity.operlog.operlocation') },
  { key: 'operTimeStart', label: t('entity.operlog.opertimestart') },
  { key: 'operTimeEnd', label: t('entity.operlog.opertimeend') },
  { key: 'elapsedTime', label: t('entity.operlog.elapsedtime') },
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
const entityIdName = 'operLogId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)
/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据 */
const detailData = ref<OperLog | null>(null)

/** 高级查询中可 trim 赋值的字符串字段 */
type OperLogQueryTrimmedKey =
  | 'userName'
  | 'operModule'
  | 'operMethod'
  | 'requestMethod'
  | 'operUrl'
  | 'requestParam'
  | 'jsonResult'
  | 'errorMsg'
  | 'operIp'
  | 'operLocation'
  | 'operTimeStart'
  | 'operTimeEnd'
  | 'createdAtStart'
  | 'createdAtEnd'
  | 'ExtField'
  | 'remark'

/** 组装列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端模型绑定 400） */
function buildListQuery(overrides?: Partial<OperLogQuery>): OperLogQuery {
  const form = advancedQueryForm.value
  const query: OperLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: OperLogQueryTrimmedKey, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v
    }
  }
  assignTrimmed('userName', form.userName)
  assignTrimmed('operModule', form.operModule)
  assignTrimmed('operMethod', form.operMethod)
  assignTrimmed('requestMethod', form.requestMethod)
  assignTrimmed('operUrl', form.operUrl)
  assignTrimmed('requestParam', form.requestParam)
  assignTrimmed('jsonResult', form.jsonResult)
  assignTrimmed('errorMsg', form.errorMsg)
  assignTrimmed('operIp', form.operIp)
  assignTrimmed('operLocation', form.operLocation)
  assignTrimmed('operTimeStart', form.operTimeStart)
  assignTrimmed('operTimeEnd', form.operTimeEnd)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('ExtField', form.ExtField)
  assignTrimmed('remark', form.remark)
  if (form.operType?.trim()) {
    query.operType = form.operType.trim()
  }
  if (form.operStatus !== undefined && form.operStatus !== null) {
    query.operStatus = form.operStatus
  }
  if (form.elapsedTime !== undefined && form.elapsedTime !== null) {
    query.elapsedTime = form.elapsedTime
  }
  return query
}

/** 页面挂载后加载分页列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'operLogId',
    key: 'operLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operLogId') ?? ''
  },
  {
    title: t('entity.operlog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.operlog.opermodule'),
    dataIndex: 'operModule',
    key: 'operModule',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operModule') ?? ''
  },
  {
    title: t('entity.operlog.opertype'),
    dataIndex: 'operType',
    key: 'operType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.operlog.opermethod'),
    dataIndex: 'operMethod',
    key: 'operMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operMethod') ?? ''
  },
  {
    title: t('entity.operlog.requestmethod'),
    dataIndex: 'requestMethod',
    key: 'requestMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'requestMethod') ?? ''
  },
  {
    title: t('entity.operlog.operurl'),
    dataIndex: 'operUrl',
    key: 'operUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operUrl') ?? ''
  },
  {
    title: t('entity.operlog.requestparam'),
    dataIndex: 'requestParam',
    key: 'requestParam',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'requestParam') ?? ''
  },
  {
    title: t('entity.operlog.jsonresult'),
    dataIndex: 'jsonResult',
    key: 'jsonResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'jsonResult') ?? ''
  },
  {
    title: t('entity.operlog.operstatus'),
    dataIndex: 'operStatus',
    key: 'operStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operStatus') ?? ''
  },
  {
    title: t('entity.operlog.errormsg'),
    dataIndex: 'errorMsg',
    key: 'errorMsg',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'errorMsg') ?? ''
  },
  {
    title: t('entity.operlog.operip'),
    dataIndex: 'operIp',
    key: 'operIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operIp') ?? ''
  },
  {
    title: t('entity.operlog.operlocation'),
    dataIndex: 'operLocation',
    key: 'operLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operLocation') ?? ''
  },
  {
    title: t('entity.operlog.opertime'),
    dataIndex: 'operTime',
    key: 'operTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'operTime') ?? ''
  },
  {
    title: t('entity.operlog.elapsedtime'),
    dataIndex: 'elapsedTime',
    key: 'elapsedTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOperLogField(record, 'elapsedTime') ?? ''
  },
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'statistics:logging:oper:log:query',
        onClick: (record: OperLog) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:oper:log:delete',
        onClick: (record: OperLog) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getOperLogId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getOperLogField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: OperLog[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: OperLog, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getOperLogId(selecmedRow.value) === getOperLogId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: OperLog[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: OperLog) => ({
  onClick: () => {
    const key = getOperLogId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getOperLogId(item)))
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
    const res = await getOperLogList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[OperLog] 加载数据失败', { error })
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
async function handleShowDetail(record: OperLog) {
  const id = getOperLogId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getOperLogById(id)
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
  operModule: '',
  operType: undefined as string | undefined,
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
  operStatus: undefined as number | undefined,
  errorMsg: '',
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
    const exportMeta = await exportOperLog(
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
    message.success(t('common.feedback.export.success', { target: t('entity.operlog._self') }))
  } catch (error: any) {
    logger.error('[OperLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.operlog._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: OperLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.operlog._self'), name: t('common.tip.this.target', { target: t('entity.operlog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOperLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.operlog._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.operlog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.operlog._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteOperLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.operlog._self') }))
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
  operModule: '',
  operType: undefined as string | undefined,
  operMethod: '',
  requestMethod: '',
  operUrl: '',
  requestParam: '',
  jsonResult: '',
  operStatus: undefined as number | undefined,
  errorMsg: '',
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/login-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：登录日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      delete-permission="statistics:logging:login:log:delete"
      export-permission="statistics:logging:login:log:export"
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
      :id-column-key="'loginLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getLoginLogId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'browser'">
          <TaktConstTag
            category="browserType"
            :value="getLoginLogField(record, 'browser')"
          />
        </template>
        <template v-else-if="column.key === 'os'">
          <TaktConstTag
            category="operatingSystem"
            :value="getLoginLogField(record, 'os')"
          />
        </template>
        <template v-else-if="column.key === 'loginType'">
          <TaktConstTag
            category="loginType"
            :value="getLoginLogField(record, 'loginType')"
          />
        </template>
        <template v-else-if="column.key === 'loginResult'">
          <TaktConstTag
            category="loginResult"
            :value="getLoginLogField(record, 'loginResult')"
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
      :title="t('common.dialog.title.detail', { entity: t('entity.loginlog._self') })"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleDetailClose"
    >
      <a-spin :spinning="detailLoading">
        <LoginLogDetail :detail="detailData" />
      </a-spin>
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-statistics-logging-login-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.loginlog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.username') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('loginType')">
      <a-form-item :label="t('entity.loginlog.logintype')">
        <TaktSelect
          v-model:value="advancedQueryForm.loginType"
          :options="loginTypeOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.logintype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('browser')">
      <a-form-item :label="t('entity.loginlog.browser')">
        <TaktSelect
          v-model:value="advancedQueryForm.browser"
          :options="browserTypeOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.browser') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('os')">
      <a-form-item :label="t('entity.loginlog.os')">
        <TaktSelect
          v-model:value="advancedQueryForm.os"
          :options="operatingSystemOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.os') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userAgent')">
      <a-form-item :label="t('entity.loginlog.useragent')">
        <a-input
          v-model:value="advancedQueryForm.userAgent"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.useragent') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('loginResult')">
      <a-form-item :label="t('entity.loginlog.loginresult')">
        <TaktSelect
          v-model:value="advancedQueryForm.loginResult"
          :options="loginResultOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.loginlog.loginresult') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('loginMessage')">
      <a-form-item :label="t('entity.loginlog.loginmessage')">
        <a-input
          v-model:value="advancedQueryForm.loginMessage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginmessage') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('loginIp')">
      <a-form-item :label="t('entity.loginlog.loginip')">
        <a-input
          v-model:value="advancedQueryForm.loginIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginip') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('loginLocation')">
      <a-form-item :label="t('entity.loginlog.loginlocation')">
        <a-input
          v-model:value="advancedQueryForm.loginLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.loginlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logoutAtStart')">
      <a-form-item :label="t('entity.loginlog.logoutatstart')">
        <a-input
          v-model:value="advancedQueryForm.logoutAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.logoutatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logoutAtEnd')">
      <a-form-item :label="t('entity.loginlog.logoutatend')">
        <a-input
          v-model:value="advancedQueryForm.logoutAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.loginlog.logoutatend') })"
          allow-clear
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
      :id-column-key="'loginLogId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import {
  browserTypeOptions,
  loginResultOptions,
  loginTypeOptions,
  operatingSystemOptions,
} from '@/constants/takt.constants'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 登录日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/login-log
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import LoginLogDetail from './components/LoginLogDetail.vue'
import { getLoginLogList, getLoginLogById, deleteLoginLogById, deleteLoginLogBatch, exportLoginLogData } from '@/api/statistics/logging/loginLog.api'
import type { LoginLog, LoginLogQuery } from '@/types/statistics/logging/loginLog.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEyeLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktLoginLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.loginlog._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<LoginLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<LoginLog | null>(null)
/** 表格多选行 */
const selectedRows = ref<LoginLog[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  userName: '',
  loginType: undefined as string | undefined,
  browser: undefined as string | undefined,
  os: undefined as string | undefined,
  userAgent: '',
  loginResult: undefined as string | undefined,
  loginMessage: '',
  loginIp: '',
  loginLocation: '',
  logoutAtStart: '',
  logoutAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'userName', label: t('entity.loginlog.username') },
  { key: 'loginType', label: t('entity.loginlog.logintype') },
  { key: 'browser', label: t('entity.loginlog.browser') },
  { key: 'os', label: t('entity.loginlog.os') },
  { key: 'userAgent', label: t('entity.loginlog.useragent') },
  { key: 'loginResult', label: t('entity.loginlog.loginresult') },
  { key: 'loginMessage', label: t('entity.loginlog.loginmessage') },
  { key: 'loginIp', label: t('entity.loginlog.loginip') },
  { key: 'loginLocation', label: t('entity.loginlog.loginlocation') },
  { key: 'logoutAtStart', label: t('entity.loginlog.logoutatstart') },
  { key: 'logoutAtEnd', label: t('entity.loginlog.logoutatend') },
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
const entityIdName = 'loginLogId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)
/** 详情弹窗是否打开 */
const detailVisible = ref(false)
/** 详情加载中 */
const detailLoading = ref(false)
/** 详情数据 */
const detailData = ref<LoginLog | null>(null)

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'loginLogId',
    key: 'loginLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginLogId') ?? ''
  },
  {
    title: t('entity.loginlog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.loginlog.logintype'),
    dataIndex: 'loginType',
    key: 'loginType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.browser'),
    dataIndex: 'browser',
    key: 'browser',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.os'),
    dataIndex: 'os',
    key: 'os',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.useragent'),
    dataIndex: 'userAgent',
    key: 'userAgent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'userAgent') ?? ''
  },
  {
    title: t('entity.loginlog.loginresult'),
    dataIndex: 'loginResult',
    key: 'loginResult',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.loginlog.loginmessage'),
    dataIndex: 'loginMessage',
    key: 'loginMessage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginMessage') ?? ''
  },
  {
    title: t('common.page.entity.remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 180,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'remark') ?? ''
  },
  {
    title: t('entity.loginlog.loginip'),
    dataIndex: 'loginIp',
    key: 'loginIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginIp') ?? ''
  },
  {
    title: t('entity.loginlog.loginlocation'),
    dataIndex: 'loginLocation',
    key: 'loginLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'loginLocation') ?? ''
  },
  {
    title: t('entity.loginlog.logoutat'),
    dataIndex: 'logoutAt',
    key: 'logoutAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getLoginLogField(record, 'logoutAt') ?? ''
  },
  CreateActionColumn({
    width: 148,
    actions: [
      {
        key: 'detail',
        label: t('common.page.button.detail'),
        shape: 'plain',
        icon: RiEyeLine,
        permission: 'statistics:logging:login:log:query',
        onClick: (record: LoginLog) => handleShowDetail(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:login:log:delete',
        onClick: (record: LoginLog) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getLoginLogId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getLoginLogField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: LoginLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: LoginLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getLoginLogId(selectedRow.value) === getLoginLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: LoginLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: LoginLog) => ({
  onClick: () => {
    const key = getLoginLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getLoginLogId(item)))
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
    const kw = (queryKeyword.value ?? '').trim()
    const params: LoginLogQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getLoginLogList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[LoginLog] 加载数据失败', { error })
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
async function handleShowDetail(record: LoginLog) {
  const id = getLoginLogId(record)
  if (!id) {
    return
  }
  detailVisible.value = true
  detailLoading.value = true
  detailData.value = null
  try {
    detailData.value = await getLoginLogById(id)
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
  loginType: undefined as string | undefined,
  browser: undefined as string | undefined,
  os: undefined as string | undefined,
  userAgent: '',
  loginResult: undefined as string | undefined,
  loginMessage: '',
  loginIp: '',
  loginLocation: '',
  logoutAtStart: '',
  logoutAtEnd: '',
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
    const exportQuery: LoginLogQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportLoginLogData(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.loginlog._self') }))
  } catch (error: any) {
    logger.error('[LoginLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.loginlog._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: LoginLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.loginlog._self'), name: t('common.tip.this.target', { target: t('entity.loginlog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteLoginLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.loginlog._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.loginlog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.loginlog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteLoginLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.loginlog._self') }))
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
  loginType: undefined as string | undefined,
  browser: undefined as string | undefined,
  os: undefined as string | undefined,
  userAgent: '',
  loginResult: undefined as string | undefined,
  loginMessage: '',
  loginIp: '',
  loginLocation: '',
  logoutAtStart: '',
  logoutAtEnd: '',
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

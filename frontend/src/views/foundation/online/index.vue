<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/online -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：在线用户管理页面（查询、删除、导出、强退；会话由 SignalR 自动注册） -->
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
      delete-permission="foundation:online:delete"
      export-permission="foundation:online:export"
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
      :export-disabled="false"
      :export-loading="loading"
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
      :id-column-key="'onlineId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getOnlineId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'onlineStatus'">
          <TaktDictTag
            :value="getOnlineField(record, 'onlineStatus')"
            dict-type="sys_online_status"
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

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-foundation-online'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('connectionId')">
      <a-form-item :label="t('entity.online.connectionid')">
        <a-input
          v-model:value="advancedQueryForm.connectionId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.online.connectionid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.online.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.online.username') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userId')">
      <a-form-item :label="t('entity.online.userid')">
        <a-input
          v-model:value="advancedQueryForm.userId"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.online.userid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('onlineStatus')">
      <a-form-item :label="t('entity.online.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.onlineStatus"
          dict-type="sys_online_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.online.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('connectIp')">
      <a-form-item :label="t('entity.online.connectip')">
        <a-input
          v-model:value="advancedQueryForm.connectIp"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.online.connectip') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('connectLocation')">
      <a-form-item :label="t('entity.online.connectlocation')">
        <a-input
          v-model:value="advancedQueryForm.connectLocation"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.online.connectlocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('connectTimeStart')">
      <a-form-item :label="t('entity.online.connecttime') + ' (' + t('common.page.entity.createdatstart') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.connectTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.online.connecttime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('connectTimeEnd')">
      <a-form-item :label="t('entity.online.connecttime') + ' (' + t('common.page.entity.createdatend') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.connectTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.online.connecttime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
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
      :id-column-key="'onlineId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

    <!-- 强退弹窗 -->
    <TaktModal
      v-model:open="kickVisible"
      :title="kickModalTitle"
      :confirm-loading="kickLoading"
      :use-viewport-size="false"
      width="640px"
      @ok="handleKickSubmit"
      @cancel="handleKickCancel"
    >
      <OnlineKickForm
        ref="kickFormRef"
        :form-data="kickTarget"
        :default-kick-mode="kickDefaultMode"
        :loading="kickLoading"
      />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 在线用户实体管理页
 * @module views/foundation/online
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
import { getOnlineList, deleteOnlineById, deleteOnlineBatch, exportOnline, forceKickOnlineById } from '@/api/foundation/online.api'
import type { Online, OnlineQuery } from '@/types/foundation/online.types'
import OnlineKickForm from './components/OnlineKickForm.vue'
import type { OnlineKickMode } from './components/OnlineKickForm.vue'
import { useUserStore } from '@/stores/identity/useUserStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { TaktOnlineStatus } from '@/utils/foundationEnums'
import { RiDeleteBinLine, RiLogoutBoxRLine, RiTimerLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktOnline')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.online._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Online[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Online | null>(null)
/** 表格多选行 */
const selectedRows = ref<Online[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 未选中行时禁用工具栏批量删除 */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  connectionId: '',
  userName: '',
  userId: '',
  onlineStatus: undefined as number | undefined,
  connectIp: '',
  connectLocation: '',
  connectTimeStart: '',
  connectTimeEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'connectionId', label: t('entity.online.connectionid') },
  { key: 'userName', label: t('entity.online.username') },
  { key: 'userId', label: t('entity.online.userid') },
  { key: 'onlineStatus', label: t('entity.online.status') },
  { key: 'connectIp', label: t('entity.online.connectip') },
  { key: 'connectLocation', label: t('entity.online.connectlocation') },
  { key: 'connectTimeStart', label: t('entity.online.connecttime') + ' (' + t('common.page.entity.createdatstart') + ')' },
  { key: 'connectTimeEnd', label: t('entity.online.connecttime') + ' (' + t('common.page.entity.createdatend') + ')' },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 强退弹窗是否打开 */
const kickVisible = ref(false)
/** 强退提交 loading */
const kickLoading = ref(false)
/** 强退目标行 */
const kickTarget = ref<Online | null>(null)
/** 强退弹窗默认方式 */
const kickDefaultMode = ref<OnlineKickMode>('immediate')
/** 强退表单 ref */
const kickFormRef = ref<InstanceType<typeof OnlineKickForm> | null>(null)

/** 强退弹窗标题（随入口默认方式变化） */
const kickModalTitle = computed(() =>
  kickDefaultMode.value === 'immediate'
    ? t('common.page.button.kick.immediate')
    : t('common.page.button.kick.delayed'),
)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'onlineId'

/**
 * 判断在线用户是否处于在线状态（可强退）
 * @param record 行数据
 * @returns {boolean} 是否在线
 */
function isOnlineActive(record: Online): boolean {
  const status = getOnlineField(record, 'onlineStatus')
  return status === TaktOnlineStatus.Online || status === '0'
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
    dataIndex: 'onlineId',
    key: 'onlineId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'onlineId') ?? ''
  },
  {
    title: t('entity.online.connectionid'),
    dataIndex: 'connectionId',
    key: 'connectionId',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'connectionId') ?? ''
  },
  {
    title: t('entity.online.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'userName') ?? ''
  },
  {
    title: t('entity.online.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'userId') ?? ''
  },
  {
    title: t('entity.online.status'),
    dataIndex: 'onlineStatus',
    key: 'onlineStatus',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.online.connectip'),
    dataIndex: 'connectIp',
    key: 'connectIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'connectIp') ?? ''
  },
  {
    title: t('entity.online.connectlocation'),
    dataIndex: 'connectLocation',
    key: 'connectLocation',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'connectLocation') ?? ''
  },
  {
    title: t('entity.online.connecttime'),
    dataIndex: 'connectTime',
    key: 'connectTime',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'connectTime') ?? ''
  },
  {
    title: t('entity.online.lastactivetime'),
    dataIndex: 'lastActiveTime',
    key: 'lastActiveTime',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'lastActiveTime') ?? ''
  },
  {
    title: t('entity.online.disconnecttime'),
    dataIndex: 'disconnectTime',
    key: 'disconnectTime',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'disconnectTime') ?? ''
  },
  {
    title: t('entity.online.connectionduration'),
    dataIndex: 'connectionDuration',
    key: 'connectionDuration',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getOnlineField(record, 'connectionDuration') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'kick-immediate',
        label: t('common.page.button.kick.immediate'),
        shape: 'plain',
        icon: RiLogoutBoxRLine,
        permission: 'foundation:online:kick',
        disabled: (record: Online) => !isOnlineActive(record),
        onClick: (record: Online) => openKickModal(record, 'immediate')
      },
      {
        key: 'kick-delayed',
        label: t('common.page.button.kick.delayed'),
        shape: 'plain',
        icon: RiTimerLine,
        permission: 'foundation:online:kick',
        disabled: (record: Online) => !isOnlineActive(record),
        onClick: (record: Online) => openKickModal(record, 'delayed')
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:online:delete',
        onClick: (record: Online) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key */
const getOnlineId = (record: any): string => {
  const raw = record?.[entityIdName] ?? record?.onlineId ?? record?.id
  if (raw == null || raw === '') return ''
  return String(raw)
}

/**
 * 列表行 ID 字段归一为 string（避免 JSON 大整数精度丢失）
 * @param row 原始行
 * @returns 归一化后的行
 */
function normalizeOnlineRow(row: Online): Online {
  const onlineId = row.onlineId != null && row.onlineId !== '' ? String(row.onlineId) : ''
  const userId = row.userId != null && row.userId !== '' ? String(row.userId) : row.userId
  return { ...row, onlineId, userId }
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getOnlineField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Online[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Online, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getOnlineId(selectedRow.value) === getOnlineId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Online[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Online) => ({
  onClick: () => {
    const key = getOnlineId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getOnlineId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  if (!useUserStore().isLoggedIn) {
    return
  }
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: OnlineQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const q = advancedQueryForm.value
    if (q.connectionId) params.connectionId = q.connectionId
    if (q.userName) params.userName = q.userName
    if (q.userId) params.userId = q.userId
    if (q.onlineStatus !== undefined) params.onlineStatus = q.onlineStatus
    if (q.connectIp) params.connectIp = q.connectIp
    if (q.connectLocation) params.connectLocation = q.connectLocation
    if (q.connectTimeStart) params.connectTimeStart = q.connectTimeStart
    if (q.connectTimeEnd) params.connectTimeEnd = q.connectTimeEnd
    if (q.createdAtStart) params.createdAtStart = q.createdAtStart
    if (q.createdAtEnd) params.createdAtEnd = q.createdAtEnd
    if (q.remark) params.remark = q.remark
    const res = await getOnlineList(params)
    dataSource.value = (res.data ?? []).map((row) => normalizeOnlineRow(row as Online))
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Online] 加载数据失败', { error })
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
  advancedQueryForm.value = {
    connectionId: '',
    userName: '',
    userId: '',
    onlineStatus: undefined as number | undefined,
    connectIp: '',
    connectLocation: '',
    connectTimeStart: '',
    connectTimeEnd: '',
    createdAtStart: '',
    createdAtEnd: '',
    remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: OnlineQuery = {
      pageIndex: 1,
      pageSize: 100000,
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const q = advancedQueryForm.value
    if (q.connectionId) exportQuery.connectionId = q.connectionId
    if (q.userName) exportQuery.userName = q.userName
    if (q.userId) exportQuery.userId = q.userId
    if (q.onlineStatus !== undefined) exportQuery.onlineStatus = q.onlineStatus
    if (q.connectIp) exportQuery.connectIp = q.connectIp
    if (q.connectLocation) exportQuery.connectLocation = q.connectLocation
    if (q.connectTimeStart) exportQuery.connectTimeStart = q.connectTimeStart
    if (q.connectTimeEnd) exportQuery.connectTimeEnd = q.connectTimeEnd
    if (q.createdAtStart) exportQuery.createdAtStart = q.createdAtStart
    if (q.createdAtEnd) exportQuery.createdAtEnd = q.createdAtEnd
    if (q.remark) exportQuery.remark = q.remark
    const exportMeta = await exportOnline(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.online._self') }))
  } catch (error: any) {
    logger.error('[Online] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.online._self') }))
  } finally {
    loading.value = false
  }
}

/** 打开强退弹窗 */
function openKickModal(record: Online, defaultMode: OnlineKickMode = 'immediate') {
  kickTarget.value = record
  kickDefaultMode.value = defaultMode
  kickVisible.value = true
  nextTick(() => {
    kickFormRef.value?.resetFields()
  })
}

/** 强退弹窗取消 */
function handleKickCancel() {
  kickVisible.value = false
  kickFormRef.value?.resetFields()
  kickTarget.value = null
}

/** 强退弹窗提交 */
async function handleKickSubmit() {
  if (!kickFormRef.value) {
    return
  }
  try {
    await kickFormRef.value.validate()
    kickLoading.value = true
    const onlineId = kickFormRef.value.getTargetOnlineId()
    const dto = kickFormRef.value.getValues()
    await forceKickOnlineById(onlineId, dto)
    message.success(t('common.feedback.success'))
    kickVisible.value = false
    kickFormRef.value.resetFields()
    kickTarget.value = null
    await loadData()
  } catch (error: any) {
    if (error?.errorFields) {
      return
    }
    logger.error('[Online] 强退失败', { error })
    message.error(error?.message || t('common.feedback.failed'))
  } finally {
    kickLoading.value = false
  }
}

/** 删除单行 */
async function handleDeleteOne(record: Online) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.online._self'), name: t('common.tip.this.target', { target: t('entity.online._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteOnlineById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.online._self') }))
      loadData()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.online._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.online._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteOnlineBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.online._self') }))
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

/** 高级查询重置 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
    connectionId: '',
    userName: '',
    userId: '',
    onlineStatus: undefined as number | undefined,
    connectIp: '',
    connectLocation: '',
    connectTimeStart: '',
    connectTimeEnd: '',
    createdAtStart: '',
    createdAtEnd: '',
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
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}
/** 分页每页条数变更（重置到默认页码） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
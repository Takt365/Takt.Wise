<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/message -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：在线消息实体管理页面，含查询、增删改、导出 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-message">
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
      send-message-permission="foundation:message:send"
      delete-permission="foundation:message:delete"
      export-permission="foundation:message:export"
      :show-send-message="true"
      :show-update="false"
      :show-delete="true"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :send-message-disabled="false"
      :send-message-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @send-message="handleSendMessage"
      @delete="handleDelete"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <div class="foundation-message-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'messageId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMessageId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'messageType'">
          <TaktDictTag
            :value="getMessageField(record, 'messageType')"
            dict-type="sys_message_type"
          />
        </template>
        <template v-else-if="column.key === 'messageGroup'">
          <TaktDictTag
            :value="getMessageField(record, 'messageGroup')"
            dict-type="sys_message_group"
          />
        </template>
        <template v-else-if="column.key === 'isCc'">
          <TaktDictTag
            :value="getMessageField(record, 'isCc')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'readStatus'">
          <TaktDictTag
            :value="getMessageField(record, 'readStatus')"
            dict-type="sys_read_status"
          />
        </template>
      </template>
    </TaktSingleTable>
    </div>

    <!-- 分页组件 -->
    <TaktPagination
      v-if="paginationReady"
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MessageForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-foundation-message'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('fromUserId')">
      <a-form-item :label="t('entity.message.fromuserid')">
        <TaktSelect
          v-model:value="advancedQueryForm.fromUserId"
          api-url="TaktUsers/options"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.fromuserid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fromUserName')">
      <a-form-item :label="t('entity.message.fromusername')">
        <a-input
          v-model:value="advancedQueryForm.fromUserName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.message.fromusername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toUserId')">
      <a-form-item :label="t('entity.message.touserid')">
        <TaktSelect
          v-model:value="advancedQueryForm.toUserId"
          api-url="TaktUsers/options"
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.touserid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('toUserName')">
      <a-form-item :label="t('entity.message.tousername')">
        <a-input
          v-model:value="advancedQueryForm.toUserName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.message.tousername') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('messageType')">
      <a-form-item :label="t('entity.message.type')">
        <TaktSelect
          v-model:value="advancedQueryForm.messageType"
          dict-type="sys_message_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.type') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('messageGroup')">
      <a-form-item :label="t('entity.message.group')">
        <TaktSelect
          v-model:value="advancedQueryForm.messageGroup"
          dict-type="sys_message_group"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.group') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('messageTitle')">
      <a-form-item :label="t('entity.message.title')">
        <a-input
          v-model:value="advancedQueryForm.messageTitle"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('messageContent')">
      <a-form-item :label="t('entity.message.content')">
        <a-input
          v-model:value="advancedQueryForm.messageContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.message.content') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sendTimeStart')">
      <a-form-item :label="t('entity.message.sendtime') + ' (' + t('common.page.entity.createdatstart') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.sendTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.sendtime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sendTimeEnd')">
      <a-form-item :label="t('entity.message.sendtime') + ' (' + t('common.page.entity.createdatend') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.sendTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.sendtime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('readTimeStart')">
      <a-form-item :label="t('entity.message.readtime') + ' (' + t('common.page.entity.createdatstart') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.readTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.readtime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('readTimeEnd')">
      <a-form-item :label="t('entity.message.readtime') + ' (' + t('common.page.entity.createdatend') + ')'">
        <a-date-picker
          v-model:value="advancedQueryForm.readTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.readtime') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('readStatus')">
      <a-form-item :label="t('entity.message.readstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.readStatus"
          dict-type="sys_read_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.message.readstatus') })"
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
      :id-column-key="'messageId'"
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
 * 在线消息实体管理页
 * @module views/foundation/message
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize, ensureTaktPaginationConfigAsync } from '@/utils/taktPaged'
import MessageForm from './components/MessageForm.vue'
import {
  getMessageList,
  deleteMessageById,
  deleteMessageBatch,
  exportMessage,
} from '@/api/foundation/message.api'
import type {
  Message,
  MessageQuery,
} from '@/types/foundation/message.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { useUserStore } from '@/stores/identity/useUserStore'
import { isLogoutInProgress } from '@/bootstrap/taktLogoutFlow'
import { useEventBus } from '@/utils/eventBus'
import type { SignalRMessage } from '@/types/foundation/signalR.types'
import { RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMessage')

/**
 * 高级查询字典 DictValue：非空 trim 后传 API
 * @param value 表单值
 * @returns {string | undefined}
 */
function trimDictQueryValue(value: unknown): string | undefined {
  if (value === undefined || value === null || value === '') {
    return undefined
  }
  const text = String(value).trim()
  return text || undefined
}
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.message._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Message[]>([])
/** 当前页码（挂载后由 ensureTaktPaginationConfigAsync 同步为运维配置） */
const currentPage = ref(1)
/** 每页条数（挂载后由 ensureTaktPaginationConfigAsync 同步为运维配置） */
const pageSize = ref(20)
/** 分页配置已加载（避免 HMR/首屏在 TaktPagination 默认 props 中触发 assertPaginationConfigured） */
const paginationReady = ref(false)
/** 分页 total */
const total = ref(0)
/** 表格 scroll.y（服务端分页固定视口高度） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const
/** 工具栏单选时当前行 */
const selecmedRow = ref<Message | null>(null)
/** 表格多选行 */
const selecmedRows = ref<Message[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Message>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  fromUserId: undefined as string | undefined,
  fromUserName: '',
  toUserId: undefined as string | undefined,
  toUserName: '',
  messageTitle: '',
  messageContent: '',
  messageType: undefined as string | undefined,
  messageGroup: undefined as string | undefined,
  sendTimeStart: '',
  sendTimeEnd: '',
  readTimeStart: '',
  readTimeEnd: '',
  readStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'fromUserId', label: t('entity.message.fromuserid') },
  { key: 'fromUserName', label: t('entity.message.fromusername') },
  { key: 'toUserId', label: t('entity.message.touserid') },
  { key: 'toUserName', label: t('entity.message.tousername') },
  { key: 'messageType', label: t('entity.message.type') },
  { key: 'messageGroup', label: t('entity.message.group') },
  { key: 'messageTitle', label: t('entity.message.title') },
  { key: 'messageContent', label: t('entity.message.content') },
  { key: 'sendTimeStart', label: t('entity.message.sendtime') + ' (' + t('common.page.entity.createdatstart') + ')' },
  { key: 'sendTimeEnd', label: t('entity.message.sendtime') + ' (' + t('common.page.entity.createdatend') + ')' },
  { key: 'readTimeStart', label: t('entity.message.readtime') + ' (' + t('common.page.entity.createdatstart') + ')' },
  { key: 'readTimeEnd', label: t('entity.message.readtime') + ' (' + t('common.page.entity.createdatend') + ')' },
  { key: 'readStatus', label: t('entity.message.readstatus') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'messageId'
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** 全局事件总线（实时消息刷新列表） */
const { on: onEventBus, off: offEventBus } = useEventBus()

/**
 * SignalR 收到私信后刷新当前页列表
 * @param _msg 私信载荷
 */
function handleFoundationMessageReceived(_msg: SignalRMessage): void {
  void loadData()
}

/** 页面挂载后加载分页列表并订阅实时消息 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = getTaktDefaultPageSize()
  paginationReady.value = true
  loadData()
  onEventBus('foundation:message:received', handleFoundationMessageReceived)
})

/** 卸载时取消订阅 */
onUnmounted(() => {
  offEventBus('foundation:message:received', handleFoundationMessageReceived)
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'messageId',
    key: 'messageId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMessageField(record, 'messageId') ?? ''
  },
  {
    title: t('entity.message.fromusername'),
    dataIndex: 'fromUserName',
    key: 'fromUserName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'fromUserName') ?? ''
  },
  {
    title: t('entity.message.tousername'),
    dataIndex: 'toUserName',
    key: 'toUserName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'toUserName') ?? ''
  },
  {
    title: t('entity.message.iscc'),
    dataIndex: 'isCc',
    key: 'isCc',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.message.type'),
    dataIndex: 'messageType',
    key: 'messageType',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.message.group'),
    dataIndex: 'messageGroup',
    key: 'messageGroup',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.message.title'),
    dataIndex: 'messageTitle',
    key: 'messageTitle',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'messageTitle') ?? ''
  },
  {
    title: t('entity.message.content'),
    dataIndex: 'messageContent',
    key: 'messageContent',
    width: 200,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'messageContent') ?? ''
  },
  {
    title: t('entity.message.sendtime'),
    dataIndex: 'sendTime',
    key: 'sendTime',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'sendTime') ?? ''
  },
  {
    title: t('entity.message.readtime'),
    dataIndex: 'readTime',
    key: 'readTime',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMessageField(record, 'readTime') ?? ''
  },
  {
    title: t('entity.message.readstatus'),
    dataIndex: 'readStatus',
    key: 'readStatus',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:message:delete',
        onClick: (record: Message) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key */
const getMessageId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMessageField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Message[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Message, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getMessageId(selecmedRow.value) === getMessageId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: Message[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Message) => ({
  onClick: () => {
    const key = getMessageId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getMessageId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  if (!useUserStore().isLoggedIn || isLogoutInProgress()) {
    return
  }
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: MessageQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const q = advancedQueryForm.value
    if (q.fromUserId) params.fromUserId = q.fromUserId
    if (q.fromUserName) params.fromUserName = q.fromUserName
    if (q.toUserId) params.toUserId = q.toUserId
    if (q.toUserName) params.toUserName = q.toUserName
    if (q.messageTitle) params.messageTitle = q.messageTitle
    if (q.messageContent) params.messageContent = q.messageContent
    const messageType = trimDictQueryValue(q.messageType)
    if (messageType !== undefined) params.messageType = messageType
    const messageGroup = trimDictQueryValue(q.messageGroup)
    if (messageGroup !== undefined) params.messageGroup = messageGroup
    if (q.sendTimeStart) params.sendTimeStart = q.sendTimeStart
    if (q.sendTimeEnd) params.sendTimeEnd = q.sendTimeEnd
    if (q.readTimeStart) params.readTimeStart = q.readTimeStart
    if (q.readTimeEnd) params.readTimeEnd = q.readTimeEnd
    if (q.readStatus !== undefined) params.readStatus = q.readStatus
    if (q.createdAtStart) params.createdAtStart = q.createdAtStart
    if (q.createdAtEnd) params.createdAtEnd = q.createdAtEnd
    if (q.remark) params.remark = q.remark
    const res = await getMessageList(params)

    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    if (isLogoutInProgress() || !useUserStore().isLoggedIn) {
      return
    }
    logger.error('[Message] 加载数据失败', { error })
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
    fromUserId: undefined as string | undefined,
    fromUserName: '',
    toUserId: undefined as string | undefined,
    toUserName: '',
    messageTitle: '',
    messageContent: '',
    messageType: undefined as string | undefined,
    messageGroup: undefined as string | undefined,
    sendTimeStart: '',
    sendTimeEnd: '',
    readTimeStart: '',
    readTimeEnd: '',
    readStatus: undefined as number | undefined,
    createdAtStart: '',
    createdAtEnd: '',
    remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开发送消息弹窗 */
function handleSendMessage() {
  formTitle.value = t('common.page.button.sendmessage')
  formData.value = {}
  formVisible.value = true
}

/** 提交新增表单：表单内两步（落库 → SignalR 推送） */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.submitCreateAndPushAsync) {
    return
  }
  formLoading.value = true
  try {
    const result = await refInst.submitCreateAndPushAsync()
    if (result.pushFailed) {
      message.warning(t('common.feedback.failed'))
      return
    }
    message.success(t('common.feedback.created', { target: t('entity.message._self') }))
    formVisible.value = false
    loadData()
  } catch {
    /* validate 或落库失败：表单/请求拦截器已提示 */
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: MessageQuery = {
      pageIndex: 1,
      pageSize: 100000,
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const q = advancedQueryForm.value
    if (q.fromUserId) exportQuery.fromUserId = q.fromUserId
    if (q.fromUserName) exportQuery.fromUserName = q.fromUserName
    if (q.toUserId) exportQuery.toUserId = q.toUserId
    if (q.toUserName) exportQuery.toUserName = q.toUserName
    if (q.messageTitle) exportQuery.messageTitle = q.messageTitle
    if (q.messageContent) exportQuery.messageContent = q.messageContent
    const exportMessageType = trimDictQueryValue(q.messageType)
    if (exportMessageType !== undefined) exportQuery.messageType = exportMessageType
    const exportMessageGroup = trimDictQueryValue(q.messageGroup)
    if (exportMessageGroup !== undefined) exportQuery.messageGroup = exportMessageGroup
    if (q.sendTimeStart) exportQuery.sendTimeStart = q.sendTimeStart
    if (q.sendTimeEnd) exportQuery.sendTimeEnd = q.sendTimeEnd
    if (q.readTimeStart) exportQuery.readTimeStart = q.readTimeStart
    if (q.readTimeEnd) exportQuery.readTimeEnd = q.readTimeEnd
    if (q.readStatus !== undefined) exportQuery.readStatus = q.readStatus
    if (q.createdAtStart) exportQuery.createdAtStart = q.createdAtStart
    if (q.createdAtEnd) exportQuery.createdAtEnd = q.createdAtEnd
    if (q.remark) exportQuery.remark = q.remark
    const exportMeta = await exportMessage(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.message._self') }))
  } catch (error: any) {
    logger.error('[Message] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.message._self') }))
  } finally {
    loading.value = false
  }
}

/** 删除单行 */
async function handleDeleteOne(record: Message) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.message._self'), name: t('common.tip.this.target', { target: t('entity.message._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMessageById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.message._self') }))
      loadData()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.message._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.message._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMessageBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.message._self') }))
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
    fromUserId: undefined as string | undefined,
    fromUserName: '',
    toUserId: undefined as string | undefined,
    toUserName: '',
    messageTitle: '',
    messageContent: '',
    messageType: undefined as string | undefined,
    messageGroup: undefined as string | undefined,
    sendTimeStart: '',
    sendTimeEnd: '',
    readTimeStart: '',
    readTimeEnd: '',
    readStatus: undefined as number | undefined,
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

<style scoped lang="css">
.foundation-message {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}
.foundation-message-table-wrap {
  flex: 1;
  min-height: 0;
}
</style>
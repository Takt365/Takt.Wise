<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting/components -->
<!-- 文件名称：MeetingNotificationPanel.vue -->
<!-- 功能描述：会议通知投递记录只读面板（按主表 meetingId 分页；系统派发；支持收件人确认回执） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="meeting-notification-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      export-permission="routine:meeting:center:notification:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-expand="false"
      :show-refresh="true"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :export-disabled="!hasMasterSelection"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="meeting-notification-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getMeetingNotificationId"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="meetingNotificationId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="false"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="meetingNotificationId"
      action-column-key="action"
      entity-scope="approval"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议通知子表面板（系统派发记录 + 回执状态）
 * @module views/routine/meeting-center/meeting/components
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { RiCheckLine } from '@remixicon/vue'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useMeetingMasterContext } from '../composables/useMeetingMasterContext'
import {
  useMeetingNotificationI18n,
  MEETINGNOTIFICATION_DEFAULT_VISIBLE_COLUMN_KEYS,
  MEETING_NOTIFICATION_DELIVERY_SENT,
} from '../composables/useMeetingNotificationI18n'
import {
  getMeetingNotificationList,
  exportMeetingNotification,
  confirmMeetingNotificationReceipt,
} from '@/api/routine/meeting-center/meetingNotification.api'
import type { MeetingNotification } from '@/types/routine/meeting-center/meetingNotification.types'
import { useUserStore } from '@/stores/identity/useUserStore'

/** 实体字段 i18n */
const pi = useMeetingNotificationI18n()
const { t } = useI18n()
const userStore = useUserStore()
const { selectedMasterRow } = useMeetingMasterContext()

/** Excel 导出默认命名 */
const excelNames = taktExcelEntityNames('TaktMeetingNotification')
/** 快捷查询占位 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
const detailTableWrapRef = ref<HTMLElement | null>(null)
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 重算子表 scroll.y */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: false })
}

function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}

const dataSource = ref<MeetingNotification[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([...MEETINGNOTIFICATION_DEFAULT_VISIBLE_COLUMN_KEYS])

const entityIdName = 'meetingNotificationId'
const masterMeetingId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['meetingId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterMeetingId.value !== '')
const currentUserId = computed(() => userStore.userId ?? '')

function getMeetingNotificationId(record: MeetingNotification | Record<string, unknown>): string {
  return String((record as MeetingNotification)?.[entityIdName] ?? '')
}

function getField(record: MeetingNotification | Record<string, unknown>, field: string): unknown {
  return (record as MeetingNotification)?.[field as keyof MeetingNotification]
}

/**
 * 是否可对本行执行回执确认
 * @param {MeetingNotification} record 行
 * @returns {boolean}
 */
function canConfirmReceipt(record: MeetingNotification): boolean {
  if (record.deliveryStatus !== MEETING_NOTIFICATION_DELIVERY_SENT) {
    return false
  }
  const uid = currentUserId.value
  return uid.length > 0 && String(record.userId) === uid
}

/**
 * 确认回执
 * @param {MeetingNotification} record 行
 * @returns {Promise<void>}
 */
async function handleConfirmReceipt(record: MeetingNotification): Promise<void> {
  const id = getMeetingNotificationId(record)
  if (!id) {
    return
  }
  loading.value = true
  try {
    const result = await confirmMeetingNotificationReceipt(id)
    message.success(
      result.alreadyConfirmed
        ? t('routine.meeting-center.meeting-notification.page.confirm.alreadyConfirmed')
        : t('routine.meeting-center.meeting-notification.page.confirm.success'),
    )
    await loadData()
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message ?? t('routine.meeting-center.meeting-notification.page.confirm.failed'))
  } finally {
    loading.value = false
  }
}

const columns = computed<TableColumnsType>(() => [
  {
    title: pi.label('userName'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'userName') ?? ''),
  },
  {
    title: pi.label('recipientEmail'),
    dataIndex: 'recipientEmail',
    key: 'recipientEmail',
    width: 180,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'recipientEmail') ?? ''),
  },
  {
    title: pi.label('notificationType'),
    dataIndex: 'notificationType',
    key: 'notificationType',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) =>
      h(TaktDictTag, {
        dictType: 'routine_meeting_center_notification_type',
        value: getField(record, 'notificationType'),
      }),
  },
  {
    title: pi.label('deliveryStatus'),
    dataIndex: 'deliveryStatus',
    key: 'deliveryStatus',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) =>
      h(TaktDictTag, {
        dictType: 'routine_meeting_center_notification_status',
        value: getField(record, 'deliveryStatus'),
      }),
  },
  {
    title: pi.label('notificationSubject'),
    dataIndex: 'notificationSubject',
    key: 'notificationSubject',
    width: 200,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'notificationSubject') ?? ''),
  },
  {
    title: pi.label('sentAt'),
    dataIndex: 'sentAt',
    key: 'sentAt',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'sentAt') ?? ''),
  },
  {
    title: pi.label('confirmedAt'),
    dataIndex: 'confirmedAt',
    key: 'confirmedAt',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'confirmedAt') ?? ''),
  },
  {
    title: pi.label('sendErrorMessage'),
    dataIndex: 'sendErrorMessage',
    key: 'sendErrorMessage',
    width: 200,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }) => String(getField(record, 'sendErrorMessage') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'confirmReceipt',
        label: t('common.page.button.confirmReceipt'),
        icon: RiCheckLine,
        permission: 'routine:meeting:center:notification:update',
        visible: (record) => canConfirmReceipt(record as MeetingNotification),
        onClick: (record) => handleConfirmReceipt(record as MeetingNotification),
      },
    ],
    actionColumnKey: 'action',
  }),
])

function hasAnyListQueryFilter(): boolean {
  if (hasMasterSelection.value) {
    return true
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  return false
}

/**
 * 构建列表查询参数
 * @param {object} [overrides] 覆盖分页
 * @returns {Record<string, unknown>}
 */
function buildListQuery(overrides?: { pageIndex?: number; pageSize?: number }): Record<string, unknown> {
  const query: Record<string, unknown> = {
    pageIndex: overrides?.pageIndex ?? currentPage.value,
    pageSize: overrides?.pageSize ?? pageSize.value,
  }
  if (masterMeetingId.value) {
    query.meetingId = masterMeetingId.value
  }
  const kw = (queryKeyword.value ?? '').trim()
  if (kw) {
    query.keyWords = kw
  }
  return query
}

/** 加载列表 */
async function loadData(): Promise<void> {
  if (!hasAnyListQueryFilter()) {
    dataSource.value = []
    total.value = 0
    return
  }
  loading.value = true
  try {
    const result = await getMeetingNotificationList(buildListQuery())
    dataSource.value = result.data ?? []
    total.value = result.total ?? 0
  } finally {
    loading.value = false
  }
}

function handleSearch(): void {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset(): void {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleRefresh(): void {
  void loadData()
}

function handleColumnSetting(): void {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]): void {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset(): void {
  visibleColumnKeys.value = [...MEETINGNOTIFICATION_DEFAULT_VISIBLE_COLUMN_KEYS]
}

function handleMasterDetailPaginationChange(page: number, size: number): void {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

function handleTableChange(): void {}

function handleResizeColumn(): void {}

/** 导出当前会议通知 */
async function handleExport(): Promise<void> {
  if (!hasMasterSelection.value) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.export'),
      entity: pi.self(),
    }))
    return
  }
  loading.value = true
  try {
    const exportMeta = await exportMeetingNotification(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase,
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
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
    const err = error as { message?: string }
    message.error(err?.message ?? t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}

watch(masterMeetingId, () => {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
})

onMounted(() => {
  void nextTick(() => {
    startDetailTableScrollObserve()
    void loadData()
  })
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

defineExpose({
  loadData,
  refresh: handleRefresh,
})
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会议中心管理页（工具栏含会议室/纪要/出席人窗口；右栏出席人明细） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selecmed-master-key="selecmedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getMeetingId"
      :master-row-selection="rowSelection"
      master-id-column-key="meetingId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="approval"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      create-permission="routine:meeting:center:create"
      update-permission="routine:meeting:center:update"
      delete-permission="routine:meeting:center:delete"
      import-permission="routine:meeting:center:import"
      export-permission="routine:meeting:center:export"
      :left-actions="toolbarLeftActions"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
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
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
        />
      </template>
      <template #detail>
        <MeetingAttendeePanel
          ref="meetingAttendeePanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

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
      <MeetingForm
        :key="formData?.meetingId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 纪要窗口（须选中会议） -->
    <TaktModal
      v-model:open="minutesWindowVisible"
      :title="minutesWindowTitle"
      width="1200px"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleAgendaWindowClose"
    >
      <div
        v-if="minutesWindowVisible"
        class="h-[70vh] min-h-[420px] flex flex-col overflow-hidden"
      >
        <MeetingMinutesPanel
          ref="meetingMinutesWindowPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </div>
    </TaktModal>
    <!-- 参与人窗口（须选中会议；右侧明细同组件） -->
    <TaktModal
      v-model:open="attendeeWindowVisible"
      :title="attendeeWindowTitle"
      width="1200px"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleAttendeeWindowClose"
    >
      <div
        v-if="attendeeWindowVisible"
        class="h-[70vh] min-h-[420px] flex flex-col overflow-hidden"
      >
        <MeetingAttendeePanel
          ref="meetingAttendeeWindowPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </div>
    </TaktModal>
    <!-- 会议通知窗口（须选中会议；系统派发记录） -->
    <TaktModal
      v-model:open="notificationWindowVisible"
      :title="notificationWindowTitle"
      width="1200px"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleNotificationWindowClose"
    >
      <div
        v-if="notificationWindowVisible"
        class="h-[70vh] min-h-[420px] flex flex-col overflow-hidden"
      >
        <MeetingNotificationPanel
          ref="meetingNotificationWindowPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </div>
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-routine-meeting-center-meeting'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">

      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="MEETING_SELF_I18N_KEY"
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
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'meetingId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议中心主实体 支持内部/外部/视频/混合会议排期、议程及参与人管理管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/meeting-center/meeting
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import MeetingForm from './components/MeetingForm.vue'
import MeetingAttendeePanel from './components/MeetingAttendeePanel.vue'
import MeetingNotificationPanel from './components/MeetingNotificationPanel.vue'
import MeetingMinutesPanel from './components/MeetingMinutesPanel.vue'
import { provideMeetingMasterContext, type MeetingRowRecord } from './composables/useMeetingMasterContext'
import { getMeetingList, getMeetingById, createMeeting, updateMeeting, deleteMeetingById, deleteMeetingBatch, getMeetingTemplate, importMeeting, exportMeeting, updateMeetingStatus } from '@/api/routine/meeting-center/meeting.api'
import type { Meeting, MeetingQuery } from '@/types/routine/meeting-center/meeting.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiFileList3Line, RiGroupLine, RiMailLine } from '@remixicon/vue'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'

import {
  useMeetingI18n,
  MEETING_LIST_FIELDS,
  MEETING_QUERY_STRING_FIELDS,
  MEETING_QUERY_FIELDS,
  MEETING_SELF_I18N_KEY,
} from './composables/useMeetingI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMeetingI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMeeting')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Meeting[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<MeetingRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<MeetingRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Meeting> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of MEETING_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }

  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(MEETING_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof MEETING_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  MEETING_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'meetingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provideMeetingMasterContext()
const meetingAttendeePanelRef = ref<InstanceType<typeof MeetingAttendeePanel> | null>(null)
/** 参与人窗口内面板 ref */
const meetingAttendeeWindowPanelRef = ref<InstanceType<typeof MeetingAttendeePanel> | null>(null)
/** 纪要窗口内面板 ref */
const meetingMinutesWindowPanelRef = ref<InstanceType<typeof MeetingMinutesPanel> | null>(null)
/** 纪要窗口是否打开 */
const minutesWindowVisible = ref(false)
/** 参与人窗口是否打开 */
const attendeeWindowVisible = ref(false)
/** 会议通知窗口是否打开 */
const notificationWindowVisible = ref(false)
/** 会议通知窗口内面板 ref */
const meetingNotificationWindowPanelRef = ref<InstanceType<typeof MeetingNotificationPanel> | null>(null)

/** 纪要窗口标题 */
const minutesWindowTitle = computed(() => {
  const row = selecmedMasterRow.value as Record<string, unknown> | null
  const title = row?.meetingTitle != null ? String(row.meetingTitle) : ''
  const code = row?.meetingCode != null ? String(row.meetingCode) : ''
  const base = t('common.page.button.minutes')
  if (title || code) {
    return `${base} — ${title || code}`
  }
  return base
})

/** 参与人窗口标题 */
const attendeeWindowTitle = computed(() => {
  const row = selecmedMasterRow.value as Record<string, unknown> | null
  const title = row?.meetingTitle != null ? String(row.meetingTitle) : ''
  const code = row?.meetingCode != null ? String(row.meetingCode) : ''
  const base = t('common.page.button.attendee')
  if (title || code) {
    return `${base} — ${title || code}`
  }
  return base
})

/** 会议通知窗口标题 */
const notificationWindowTitle = computed(() => {
  const row = selecmedMasterRow.value as Record<string, unknown> | null
  const title = row?.meetingTitle != null ? String(row.meetingTitle) : ''
  const code = row?.meetingCode != null ? String(row.meetingCode) : ''
  const base = t('common.page.button.notification')
  if (title || code) {
    return `${base} — ${title || code}`
  }
  return base
})

/** 工具栏扩展：纪要 / 参与人（会议室为独立菜单页） */
const toolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'minutes',
    label: t('common.page.button.minutes'),
    icon: RiFileList3Line,
    buttonClass: 'takt-button-minutes',
    permission: 'routine:meeting:center:minutes',
    disabled: selecmedRows.value.length !== 1 && !selecmedMasterKey.value,
    onClick: () => handleOpenMinutesWindow(),
  },
  {
    key: 'attendee',
    label: t('common.page.button.attendee'),
    icon: RiGroupLine,
    buttonClass: 'takt-button-attendee',
    permission: 'routine:meeting:center:attendee',
    disabled: selecmedRows.value.length !== 1 && !selecmedMasterKey.value,
    onClick: () => handleOpenAttendeeWindow(),
  },
  {
    key: 'notification',
    label: t('common.page.button.notification'),
    icon: RiMailLine,
    buttonClass: 'takt-button-notification',
    permission: 'routine:meeting:center:notification:list',
    disabled: selecmedRows.value.length !== 1 && !selecmedMasterKey.value,
    onClick: () => handleOpenNotificationWindow(),
  },
])

/**
 * 打开纪要窗口（须选中当前会议）
 * @param {MeetingRowRecord} [record] 行；省略时用工具栏当前选中
 * @returns {void}
 */
function handleOpenMinutesWindow(record?: MeetingRowRecord): void {
  const target = record
    ?? (selecmedRows.value.length === 1 ? selecmedRows.value[0] : null)
    ?? (selecmedMasterRow.value as MeetingRowRecord | null)
  if (!target) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.minutes'),
      entity: pi.self(),
    }))
    return
  }
  selecmedMasterRow.value = target
  selecmedMasterKey.value = getMeetingId(target)
  minutesWindowVisible.value = true
}

/**
 * 关闭纪要窗口
 * @returns {void}
 */
function handleAgendaWindowClose(): void {
  minutesWindowVisible.value = false
}

/**
 * 打开参与人窗口（须选中当前会议）
 * @param {MeetingRowRecord} [record] 行；省略时用工具栏当前选中
 * @returns {void}
 */
function handleOpenAttendeeWindow(record?: MeetingRowRecord): void {
  const target = record
    ?? (selecmedRows.value.length === 1 ? selecmedRows.value[0] : null)
    ?? (selecmedMasterRow.value as MeetingRowRecord | null)
  if (!target) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.attendee'),
      entity: pi.self(),
    }))
    return
  }
  selecmedMasterRow.value = target
  selecmedMasterKey.value = getMeetingId(target)
  attendeeWindowVisible.value = true
}

/**
 * 关闭参与人窗口
 * @returns {void}
 */
function handleAttendeeWindowClose(): void {
  attendeeWindowVisible.value = false
}

/**
 * 打开会议通知窗口（须选中当前会议）
 * @param {MeetingRowRecord} [record] 行；省略时用工具栏当前选中
 * @returns {void}
 */
function handleOpenNotificationWindow(record?: MeetingRowRecord): void {
  const target = record
    ?? (selecmedRows.value.length === 1 ? selecmedRows.value[0] : null)
    ?? (selecmedMasterRow.value as MeetingRowRecord | null)
  if (!target) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.notification'),
      entity: pi.self(),
    }))
    return
  }
  selecmedMasterRow.value = target
  selecmedMasterKey.value = getMeetingId(target)
  notificationWindowVisible.value = true
}

/**
 * 关闭会议通知窗口
 * @returns {void}
 */
function handleNotificationWindowClose(): void {
  notificationWindowVisible.value = false
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MeetingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MeetingQuery>): MeetingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MeetingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MeetingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MEETING_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: MeetingRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getMeetingId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as MeetingRowRecord
  const key = getMeetingId(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 加载主表详情并回填当前页 dataSource */
async function loadMeetingDetail(record: MeetingRowRecord): Promise<Meeting | null> {
  const id = getMeetingId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getMeetingById(id)
    const index = dataSource.value.findIndex((row) => getMeetingId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Meeting
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'meetingId',
    key: 'meetingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingId') ?? ''
  },
  {
    title: pi.label('meetingTitle'),
    dataIndex: 'meetingTitle',
    key: 'meetingTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingTitle') ?? ''
  },
  {
    title: pi.label('meetingType'),
    dataIndex: 'meetingType',
    key: 'meetingType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingType') ?? ''
  },
  {
    title: pi.label('meetingStatus'),
    dataIndex: 'meetingStatus',
    key: 'meetingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingStatus') ?? ''
  },
  {
    title: pi.label('startTime'),
    dataIndex: 'startTime',
    key: 'startTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'startTime') ?? ''
  },
  {
    title: pi.label('endTime'),
    dataIndex: 'endTime',
    key: 'endTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'endTime') ?? ''
  },
  {
    title: pi.label('location'),
    dataIndex: 'location',
    key: 'location',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'location') ?? ''
  },
  {
    title: pi.label('meetingLink'),
    dataIndex: 'meetingLink',
    key: 'meetingLink',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingLink') ?? ''
  },
  {
    title: pi.label('meetingAgenda'),
    dataIndex: 'meetingAgenda',
    key: 'meetingAgenda',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingAgenda') ?? ''
  },
  {
    title: pi.label('meetingTags'),
    dataIndex: 'meetingTags',
    key: 'meetingTags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingTags') ?? ''
  },
  {
    title: pi.label('organizerId'),
    dataIndex: 'organizerId',
    key: 'organizerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'organizerId') ?? ''
  },
  {
    title: pi.label('organizerName'),
    dataIndex: 'organizerName',
    key: 'organizerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'organizerName') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('maxAttendees'),
    dataIndex: 'maxAttendees',
    key: 'maxAttendees',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'maxAttendees') ?? ''
  },
  {
    title: pi.label('reminderMinutes'),
    dataIndex: 'reminderMinutes',
    key: 'reminderMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'reminderMinutes') ?? ''
  },
  {
    title: pi.label('meetingRoomId'),
    dataIndex: 'meetingRoomId',
    key: 'meetingRoomId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingRoomId') ?? ''
  },
  {
    title: pi.label('meetingRoomName'),
    dataIndex: 'meetingRoomName',
    key: 'meetingRoomName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'meetingRoomName') ?? ''
  },
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingField(record, 'remark') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:meeting:center:update',
        onClick: (record: MeetingRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:meeting:center:delete',
        onClick: (record: MeetingRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMeetingId = (record: MeetingRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMeetingField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MeetingRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: MeetingRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getMeetingId(selecmedRow.value) === getMeetingId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: MeetingRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    syncMasterSelection(selecmedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getMeetingList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Meeting] 加载数据失败', { error })
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

  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: MeetingRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadMeetingDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateMeeting(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMeeting(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
  meetingAttendeePanelRef.value?.reload?.()
    }
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMeetingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMeeting(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selecmedMasterKey.value) {
    meetingAttendeePanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportMeeting(
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
    logger.error('[Meeting] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MeetingRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMeetingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMeetingBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
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
  advancedQueryForm.value = {

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
</script>

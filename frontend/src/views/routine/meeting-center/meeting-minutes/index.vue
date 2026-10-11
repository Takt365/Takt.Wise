<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting-minutes -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会后纪要实体 RecordType=纪要分项时多行维护议题管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="routine:meeting:center:minutes:create"
      update-permission="routine:meeting:center:minutes:update"
      delete-permission="routine:meeting:center:minutes:delete"
      import-permission="routine:meeting:center:minutes:import"
      export-permission="routine:meeting:center:minutes:export"
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

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'meetingMinutesId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMeetingMinutesId"
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
      <MeetingMinutesForm
        :key="formData?.meetingMinutesId ?? 'create'"
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
      :storage-key="'takt-query-fields-routine-meeting-center-meeting-minutes'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('meetingId')">
      <a-form-item :label="t('entity.meetingminutes.meetingid')">
        <a-input
          v-model:value="advancedQueryForm.meetingId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.meetingid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recordType')">
      <a-form-item :label="t('entity.meetingminutes.recordtype')">
        <a-input-number
          v-model:value="advancedQueryForm.recordType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.recordtype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.meetingminutes.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.meetingminutes.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.title') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('content')">
      <a-form-item :label="t('entity.meetingminutes.content')">
        <a-textarea
          v-model:value="advancedQueryForm.content"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.content') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('summary')">
      <a-form-item :label="t('entity.meetingminutes.summary')">
        <a-input
          v-model:value="advancedQueryForm.summary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.summary') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('presenterId')">
      <a-form-item :label="t('entity.meetingminutes.presenterid')">
        <a-input
          v-model:value="advancedQueryForm.presenterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.presenterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('presenterName')">
      <a-form-item :label="t('entity.meetingminutes.presentername')">
        <a-input
          v-model:value="advancedQueryForm.presenterName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.presentername') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeStart')">
      <a-form-item :label="t('entity.meetingminutes.plannedstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.meetingminutes.plannedstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedStartTimeEnd')">
      <a-form-item :label="t('entity.meetingminutes.plannedstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.meetingminutes.plannedstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('durationMinutes')">
      <a-form-item :label="t('entity.meetingminutes.durationminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.durationMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.durationminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recorderId')">
      <a-form-item :label="t('entity.meetingminutes.recorderid')">
        <a-input
          v-model:value="advancedQueryForm.recorderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.recorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('recorderName')">
      <a-form-item :label="t('entity.meetingminutes.recordername')">
        <a-input
          v-model:value="advancedQueryForm.recorderName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.meetingminutes.recordername') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.meetingminutes.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.meetingminutes.attachments') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.meetingminutes._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.meetingminutes._self"
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
      :id-column-key="'meetingMinutesId'"
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
 * 会后纪要实体 RecordType=纪要分项时多行维护议题管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/meeting-center/meeting-minutes
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import MeetingMinutesForm from './components/MeetingMinutesForm.vue'
import { getMeetingMinutesList, getMeetingMinutesById, createMeetingMinutes, updateMeetingMinutes, deleteMeetingMinutesById, deleteMeetingMinutesBatch, getMeetingMinutesTemplate, importMeetingMinutes, exportMeetingMinutes } from '@/api/routine/meeting-center/meetingMinutes.api'
import type { MeetingMinutes, MeetingMinutesQuery } from '@/types/routine/meeting-center/meetingMinutes.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMeetingMinutes')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.meetingminutes._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MeetingMinutes[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MeetingMinutes | null>(null)
/** 表格多选行 */
const selectedRows = ref<MeetingMinutes[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MeetingMinutes> | null>(null)
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
  meetingId: '',
  recordType: undefined as number | undefined,
  lineNumber: undefined as number | undefined,
  title: '',
  content: '',
  summary: '',
  presenterId: '',
  presenterName: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  durationMinutes: undefined as number | undefined,
  recorderId: '',
  recorderName: '',
  attachments: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'meetingId', label: t('entity.meetingminutes.meetingid') },
  { key: 'recordType', label: t('entity.meetingminutes.recordtype') },
  { key: 'lineNumber', label: t('entity.meetingminutes.linenumber') },
  { key: 'title', label: t('entity.meetingminutes.title') },
  { key: 'content', label: t('entity.meetingminutes.content') },
  { key: 'summary', label: t('entity.meetingminutes.summary') },
  { key: 'presenterId', label: t('entity.meetingminutes.presenterid') },
  { key: 'presenterName', label: t('entity.meetingminutes.presentername') },
  { key: 'plannedStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.meetingminutes.plannedstarttime')) },
  { key: 'plannedStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.meetingminutes.plannedstarttime')) },
  { key: 'durationMinutes', label: t('entity.meetingminutes.durationminutes') },
  { key: 'recorderId', label: t('entity.meetingminutes.recorderid') },
  { key: 'recorderName', label: t('entity.meetingminutes.recordername') },
  { key: 'attachments', label: t('entity.meetingminutes.attachments') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'meetingMinutesId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MeetingMinutesQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MeetingMinutesQuery>): MeetingMinutesQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MeetingMinutesQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MeetingMinutesQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('meetingId', form.meetingId)
  if (form.recordType !== undefined && form.recordType !== null) {
    query.recordType = form.recordType
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('title', form.title)
  assignTrimmed('content', form.content)
  assignTrimmed('summary', form.summary)
  assignTrimmed('presenterId', form.presenterId)
  assignTrimmed('presenterName', form.presenterName)
  assignTrimmed('plannedStartTimeStart', form.plannedStartTimeStart)
  assignTrimmed('plannedStartTimeEnd', form.plannedStartTimeEnd)
  if (form.durationMinutes !== undefined && form.durationMinutes !== null) {
    query.durationMinutes = form.durationMinutes
  }
  assignTrimmed('recorderId', form.recorderId)
  assignTrimmed('recorderName', form.recorderName)
  assignTrimmed('attachments', form.attachments)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'meetingMinutesId',
    key: 'meetingMinutesId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'meetingMinutesId') ?? ''
  },
  {
    title: t('entity.meetingminutes.meetingid'),
    dataIndex: 'meetingId',
    key: 'meetingId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'meetingId') ?? ''
  },
  {
    title: t('entity.meetingminutes.recordtype'),
    dataIndex: 'recordType',
    key: 'recordType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'recordType') ?? ''
  },
  {
    title: t('entity.meetingminutes.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.meetingminutes.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'title') ?? ''
  },
  {
    title: t('entity.meetingminutes.content'),
    dataIndex: 'content',
    key: 'content',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'content') ?? ''
  },
  {
    title: t('entity.meetingminutes.summary'),
    dataIndex: 'summary',
    key: 'summary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'summary') ?? ''
  },
  {
    title: t('entity.meetingminutes.presenterid'),
    dataIndex: 'presenterId',
    key: 'presenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'presenterId') ?? ''
  },
  {
    title: t('entity.meetingminutes.presentername'),
    dataIndex: 'presenterName',
    key: 'presenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'presenterName') ?? ''
  },
  {
    title: t('entity.meetingminutes.plannedstarttime'),
    dataIndex: 'plannedStartTime',
    key: 'plannedStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'plannedStartTime') ?? ''
  },
  {
    title: t('entity.meetingminutes.durationminutes'),
    dataIndex: 'durationMinutes',
    key: 'durationMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'durationMinutes') ?? ''
  },
  {
    title: t('entity.meetingminutes.recorderid'),
    dataIndex: 'recorderId',
    key: 'recorderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'recorderId') ?? ''
  },
  {
    title: t('entity.meetingminutes.recordername'),
    dataIndex: 'recorderName',
    key: 'recorderName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'recorderName') ?? ''
  },
  {
    title: t('entity.meetingminutes.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'attachments') ?? ''
  },
  {
    title: t('entity.meetingminutes.meeting'),
    dataIndex: 'meeting',
    key: 'meeting',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMeetingMinutesField(record, 'meeting') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:meeting:center:minutes:update',
        onClick: (record: MeetingMinutes) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:meeting:center:minutes:delete',
        onClick: (record: MeetingMinutes) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMeetingMinutesId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMeetingMinutesField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MeetingMinutes[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MeetingMinutes, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMeetingMinutesId(selectedRow.value) === getMeetingMinutesId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MeetingMinutes[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: MeetingMinutes) => ({
  onClick: () => {
    const key = getMeetingMinutesId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getMeetingMinutesId(item)))
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
    const res = await getMeetingMinutesList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MeetingMinutes] 加载数据失败', { error })
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
  meetingId: '',
  recordType: undefined as number | undefined,
  lineNumber: undefined as number | undefined,
  title: '',
  content: '',
  summary: '',
  presenterId: '',
  presenterName: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  durationMinutes: undefined as number | undefined,
  recorderId: '',
  recorderName: '',
  attachments: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.meetingminutes._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: MeetingMinutes) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.meetingminutes._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.meetingminutes._self') }))
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
      await updateMeetingMinutes(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.meetingminutes._self') }))
    } else {
      await createMeetingMinutes(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.meetingminutes._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getMeetingMinutesTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMeetingMinutes(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportMeetingMinutes(
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
    message.success(t('common.feedback.export.success', { target: t('entity.meetingminutes._self') }))
  } catch (error: any) {
    logger.error('[MeetingMinutes] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.meetingminutes._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MeetingMinutes) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.meetingminutes._self'), name: t('common.tip.this.target', { target: t('entity.meetingminutes._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMeetingMinutesById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.meetingminutes._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.meetingminutes._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.meetingminutes._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMeetingMinutesBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.meetingminutes._self') }))
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
  meetingId: '',
  recordType: undefined as number | undefined,
  lineNumber: undefined as number | undefined,
  title: '',
  content: '',
  summary: '',
  presenterId: '',
  presenterName: '',
  plannedStartTimeStart: '',
  plannedStartTimeEnd: '',
  durationMinutes: undefined as number | undefined,
  recorderId: '',
  recorderName: '',
  attachments: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
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

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

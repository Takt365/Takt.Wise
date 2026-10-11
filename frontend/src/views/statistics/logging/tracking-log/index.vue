<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/logging/tracking-log -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：前端交互日志实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="statistics:logging:tracking:log:create"
      update-permission="statistics:logging:tracking:log:update"
      delete-permission="statistics:logging:tracking:log:delete"

      export-permission="statistics:logging:tracking:log:export"
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
      :id-column-key="'trackingLogId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTrackingLogId"
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
      <TrackingLogForm
        :key="formData?.trackingLogId ?? 'create'"
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
      :storage-key="'takt-query-fields-statistics-logging-tracking-log'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('userName')">
      <a-form-item :label="t('entity.trackinglog.username')">
        <a-input
          v-model:value="advancedQueryForm.userName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.username') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userId')">
      <a-form-item :label="t('entity.trackinglog.userid')">
        <a-input
          v-model:value="advancedQueryForm.userId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.userid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eventTrackingType')">
      <a-form-item :label="t('entity.trackinglog.eventtrackingtype')">
        <a-input
          v-model:value="advancedQueryForm.eventTrackingType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingtype') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eventTrackingCategory')">
      <a-form-item :label="t('entity.trackinglog.eventtrackingcategory')">
        <a-input
          v-model:value="advancedQueryForm.eventTrackingCategory"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.eventtrackingcategory') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eventTimeStart')">
      <a-form-item :label="t('entity.trackinglog.eventtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.eventTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trackinglog.eventtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eventTimeEnd')">
      <a-form-item :label="t('entity.trackinglog.eventtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.eventTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trackinglog.eventtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('durationMs')">
      <a-form-item :label="t('entity.trackinglog.durationms')">
        <a-input-number
          v-model:value="advancedQueryForm.durationMs"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.durationms') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('performanceStartMs')">
      <a-form-item :label="t('entity.trackinglog.performancestartms')">
        <a-input-number
          v-model:value="advancedQueryForm.performanceStartMs"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.performancestartms') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('entryName')">
      <a-form-item :label="t('entity.trackinglog.entryname')">
        <a-input
          v-model:value="advancedQueryForm.entryName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.entryname') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trackingLevel')">
      <a-form-item :label="t('entity.trackinglog.trackinglevel')">
        <a-input-number
          v-model:value="advancedQueryForm.trackingLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.trackinglevel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routePath')">
      <a-form-item :label="t('entity.trackinglog.routepath')">
        <a-input
          v-model:value="advancedQueryForm.routePath"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.routepath') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pageUrl')">
      <a-form-item :label="t('entity.trackinglog.pageurl')">
        <a-input
          v-model:value="advancedQueryForm.pageUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.pageurl') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerType')">
      <a-form-item :label="t('entity.trackinglog.containertype')">
        <a-input
          v-model:value="advancedQueryForm.containerType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containertype') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerName')">
      <a-form-item :label="t('entity.trackinglog.containername')">
        <a-input
          v-model:value="advancedQueryForm.containerName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containername') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerSrc')">
      <a-form-item :label="t('entity.trackinglog.containersrc')">
        <a-input
          v-model:value="advancedQueryForm.containerSrc"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containersrc') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerId')">
      <a-form-item :label="t('entity.trackinglog.containerid')">
        <a-input
          v-model:value="advancedQueryForm.containerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.containerid') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attributionJson')">
      <a-form-item :label="t('entity.trackinglog.attributionjson')">
        <a-input
          v-model:value="advancedQueryForm.attributionJson"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.attributionjson') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('userAgent')">
      <a-form-item :label="t('entity.trackinglog.useragent')">
        <a-input
          v-model:value="advancedQueryForm.userAgent"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.useragent') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientIp')">
      <a-form-item :label="t('entity.trackinglog.clientip')">
        <a-input
          v-model:value="advancedQueryForm.clientIp"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trackinglog.clientip') })"
          show-count
          :maxlength="50"
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

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'trackingLogId'"
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
 * 前端交互日志实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/statistics/logging/tracking-log
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import TrackingLogForm from './components/TrackingLogForm.vue'
import { getTrackingLogList, getTrackingLogById, createTrackingLog, updateTrackingLog, deleteTrackingLogById, deleteTrackingLogBatch, exportTrackingLog } from '@/api/statistics/logging/trackingLog.api'
import type { TrackingLog, TrackingLogQuery } from '@/types/statistics/logging/trackingLog.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { parseEventTrackingDiagnosis } from '@/utils/taktEventTrackingDiagnosis'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTrackingLog')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.trackinglog._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<TrackingLog[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<TrackingLog | null>(null)
/** 表格多选行 */
const selectedRows = ref<TrackingLog[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<TrackingLog> | null>(null)
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
  userName: '',
  userId: '',
  eventTrackingType: '',
  eventTrackingCategory: '',
  eventTimeStart: '',
  eventTimeEnd: '',
  durationMs: undefined as number | undefined,
  performanceStartMs: undefined as number | undefined,
  entryName: '',
  trackingLevel: undefined as number | undefined,
  routePath: '',
  pageUrl: '',
  containerType: '',
  containerName: '',
  containerSrc: '',
  containerId: '',
  attributionJson: '',
  userAgent: '',
  clientIp: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'userName', label: t('entity.trackinglog.username') },
  { key: 'userId', label: t('entity.trackinglog.userid') },
  { key: 'eventTrackingType', label: t('entity.trackinglog.eventtrackingtype') },
  { key: 'eventTrackingCategory', label: t('entity.trackinglog.eventtrackingcategory') },
  { key: 'eventTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.trackinglog.eventtime')) },
  { key: 'eventTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.trackinglog.eventtime')) },
  { key: 'durationMs', label: t('entity.trackinglog.durationms') },
  { key: 'performanceStartMs', label: t('entity.trackinglog.performancestartms') },
  { key: 'entryName', label: t('entity.trackinglog.entryname') },
  { key: 'trackingLevel', label: t('entity.trackinglog.trackinglevel') },
  { key: 'routePath', label: t('entity.trackinglog.routepath') },
  { key: 'pageUrl', label: t('entity.trackinglog.pageurl') },
  { key: 'containerType', label: t('entity.trackinglog.containertype') },
  { key: 'containerName', label: t('entity.trackinglog.containername') },
  { key: 'containerSrc', label: t('entity.trackinglog.containersrc') },
  { key: 'containerId', label: t('entity.trackinglog.containerid') },
  { key: 'attributionJson', label: t('entity.trackinglog.attributionjson') },
  { key: 'userAgent', label: t('entity.trackinglog.useragent') },
  { key: 'clientIp', label: t('entity.trackinglog.clientip') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'trackingLogId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TrackingLogQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<TrackingLogQuery>): TrackingLogQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TrackingLogQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof TrackingLogQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('userName', form.userName)
  assignTrimmed('userId', form.userId)
  assignTrimmed('eventTrackingType', form.eventTrackingType)
  assignTrimmed('eventTrackingCategory', form.eventTrackingCategory)
  assignTrimmed('eventTimeStart', form.eventTimeStart)
  assignTrimmed('eventTimeEnd', form.eventTimeEnd)
  if (form.durationMs !== undefined && form.durationMs !== null) {
    query.durationMs = form.durationMs
  }
  if (form.performanceStartMs !== undefined && form.performanceStartMs !== null) {
    query.performanceStartMs = form.performanceStartMs
  }
  assignTrimmed('entryName', form.entryName)
  if (form.trackingLevel !== undefined && form.trackingLevel !== null) {
    query.trackingLevel = form.trackingLevel
  }
  assignTrimmed('routePath', form.routePath)
  assignTrimmed('pageUrl', form.pageUrl)
  assignTrimmed('containerType', form.containerType)
  assignTrimmed('containerName', form.containerName)
  assignTrimmed('containerSrc', form.containerSrc)
  assignTrimmed('containerId', form.containerId)
  assignTrimmed('attributionJson', form.attributionJson)
  assignTrimmed('userAgent', form.userAgent)
  assignTrimmed('clientIp', form.clientIp)
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
    dataIndex: 'trackingLogId',
    key: 'trackingLogId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'trackingLogId') ?? ''
  },
  {
    title: t('entity.trackinglog.username'),
    dataIndex: 'userName',
    key: 'userName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'userName') ?? ''
  },
  {
    title: t('entity.trackinglog.userid'),
    dataIndex: 'userId',
    key: 'userId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'userId') ?? ''
  },
  {
    title: t('entity.trackinglog.eventtrackingtype'),
    dataIndex: 'eventTrackingType',
    key: 'eventTrackingType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'eventTrackingType') ?? ''
  },
  {
    title: t('entity.trackinglog.eventtrackingcategory'),
    dataIndex: 'eventTrackingCategory',
    key: 'eventTrackingCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'eventTrackingCategory') ?? ''
  },
  {
    title: t('entity.trackinglog.eventtime'),
    dataIndex: 'eventTime',
    key: 'eventTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'eventTime') ?? ''
  },
  {
    title: t('entity.trackinglog.durationms'),
    dataIndex: 'durationMs',
    key: 'durationMs',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'durationMs') ?? ''
  },
  {
    title: t('statistics.logging.tracking-log.page.diagnosis.reportResult'),
    key: 'diagnosisReportResult',
    width: 220,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) =>
      parseEventTrackingDiagnosis(getTrackingLogField(record, 'attributionJson'))?.reportResult ?? ''
  },
  {
    title: t('statistics.logging.tracking-log.page.diagnosis.problemLocation'),
    key: 'diagnosisProblemLocation',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) =>
      parseEventTrackingDiagnosis(getTrackingLogField(record, 'attributionJson'))?.problemLocation ?? ''
  },
  {
    title: t('statistics.logging.tracking-log.page.diagnosis.action'),
    key: 'diagnosisAction',
    width: 200,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) =>
      parseEventTrackingDiagnosis(getTrackingLogField(record, 'attributionJson'))?.action ?? ''
  },
  {
    title: t('entity.trackinglog.performancestartms'),
    dataIndex: 'performanceStartMs',
    key: 'performanceStartMs',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'performanceStartMs') ?? ''
  },
  {
    title: t('entity.trackinglog.entryname'),
    dataIndex: 'entryName',
    key: 'entryName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'entryName') ?? ''
  },
  {
    title: t('entity.trackinglog.trackinglevel'),
    dataIndex: 'trackingLevel',
    key: 'trackingLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'trackingLevel') ?? ''
  },
  {
    title: t('entity.trackinglog.routepath'),
    dataIndex: 'routePath',
    key: 'routePath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'routePath') ?? ''
  },
  {
    title: t('entity.trackinglog.pageurl'),
    dataIndex: 'pageUrl',
    key: 'pageUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'pageUrl') ?? ''
  },
  {
    title: t('entity.trackinglog.containertype'),
    dataIndex: 'containerType',
    key: 'containerType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'containerType') ?? ''
  },
  {
    title: t('entity.trackinglog.containername'),
    dataIndex: 'containerName',
    key: 'containerName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'containerName') ?? ''
  },
  {
    title: t('entity.trackinglog.containersrc'),
    dataIndex: 'containerSrc',
    key: 'containerSrc',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'containerSrc') ?? ''
  },
  {
    title: t('entity.trackinglog.containerid'),
    dataIndex: 'containerId',
    key: 'containerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'containerId') ?? ''
  },
  {
    title: t('entity.trackinglog.attributionjson'),
    dataIndex: 'attributionJson',
    key: 'attributionJson',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'attributionJson') ?? ''
  },
  {
    title: t('entity.trackinglog.useragent'),
    dataIndex: 'userAgent',
    key: 'userAgent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'userAgent') ?? ''
  },
  {
    title: t('entity.trackinglog.clientip'),
    dataIndex: 'clientIp',
    key: 'clientIp',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrackingLogField(record, 'clientIp') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:logging:tracking:log:update',
        onClick: (record: TrackingLog) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:logging:tracking:log:delete',
        onClick: (record: TrackingLog) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTrackingLogId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTrackingLogField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TrackingLog[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TrackingLog, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getTrackingLogId(selectedRow.value) === getTrackingLogId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TrackingLog[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: TrackingLog) => ({
  onClick: () => {
    const key = getTrackingLogId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTrackingLogId(item)))
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
    const res = await getTrackingLogList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TrackingLog] 加载数据失败', { error })
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
  userName: '',
  userId: '',
  eventTrackingType: '',
  eventTrackingCategory: '',
  eventTimeStart: '',
  eventTimeEnd: '',
  durationMs: undefined as number | undefined,
  performanceStartMs: undefined as number | undefined,
  entryName: '',
  trackingLevel: undefined as number | undefined,
  routePath: '',
  pageUrl: '',
  containerType: '',
  containerName: '',
  containerSrc: '',
  containerId: '',
  attributionJson: '',
  userAgent: '',
  clientIp: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.trackinglog._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: TrackingLog) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.trackinglog._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.trackinglog._self') }))
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
      await updateTrackingLog(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.trackinglog._self') }))
    } else {
      await createTrackingLog(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.trackinglog._self') }))
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
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportTrackingLog(
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
    message.success(t('common.feedback.export.success', { target: t('entity.trackinglog._self') }))
  } catch (error: any) {
    logger.error('[TrackingLog] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.trackinglog._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: TrackingLog) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.trackinglog._self'), name: t('common.tip.this.target', { target: t('entity.trackinglog._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTrackingLogById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.trackinglog._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.trackinglog._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.trackinglog._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTrackingLogBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.trackinglog._self') }))
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
  userName: '',
  userId: '',
  eventTrackingType: '',
  eventTrackingCategory: '',
  eventTimeStart: '',
  eventTimeEnd: '',
  durationMs: undefined as number | undefined,
  performanceStartMs: undefined as number | undefined,
  entryName: '',
  trackingLevel: undefined as number | undefined,
  routePath: '',
  pageUrl: '',
  containerType: '',
  containerName: '',
  containerSrc: '',
  containerId: '',
  attributionJson: '',
  userAgent: '',
  clientIp: '',
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

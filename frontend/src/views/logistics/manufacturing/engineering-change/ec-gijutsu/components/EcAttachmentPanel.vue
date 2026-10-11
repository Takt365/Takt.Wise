<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：EcAttachmentPanel.vue -->
<!-- 功能描述：设变主表实体右侧明细 ecAttachment 独立 CRUD（按主表选中 ecGijutsuId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ec-attachment-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:engineering:change:gijutsu:create"
      update-permission="logistics:manufacturing:engineering:change:gijutsu:update"
      delete-permission="logistics:manufacturing:engineering:change:gijutsu:delete"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="false"
      :show-export="false"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="ec-attachment-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getEcAttachmentId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecAttachmentId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="ecGijutsuModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <EcAttachmentForm
        :key="formData?.ecAttachmentId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :master-id="masterEcId"
        :current-ec-code="currentMasterEcCode"
        :sibling-doc-codes="attachmentSiblingDocCodes"
        :sibling-file-names="attachmentSiblingFileNames"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-ec-attachment-ec-attachment"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecCode')">
      <a-form-item :label="ai.label('ecCode')">
        <a-input
          v-model:value="advancedQueryForm.ecCode"
          :placeholder="t('common.page.form.placeholder.required', { field: ai.label('ecCode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="ai.label('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: ai.label('lineNumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachmentType')">
      <a-form-item :label="ai.label('attachmentType')">
        <TaktSelect
          v-model:value="advancedQueryForm.attachmentType"
          dict-type="logistics_manufacturing_ec_attachment_type"
          :placeholder="t('common.page.form.placeholder.select', { field: ai.label('attachmentType') })"
          allow-clear
          class="w-full"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('docCode')">
      <a-form-item :label="ai.label('docCode')">
        <a-input
          v-model:value="advancedQueryForm.docCode"
          :placeholder="t('common.page.form.placeholder.required', { field: ai.label('docCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="ai.label('fileName')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: ai.label('fileName') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accessUrl')">
      <a-form-item :label="ai.label('accessUrl')">
        <a-input
          v-model:value="advancedQueryForm.accessUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: ai.label('accessUrl') })"
          show-count
          :maxlength="20"
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
      <a-form-item :label="t('common.page.entity.extfield')">
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') })"
          :rows="2"
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
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: ai.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecattachment._self"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecAttachmentId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变子表 ecAttachment 右栏面板
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import EcAttachmentForm from './EcAttachmentForm.vue'
import TaktDictTag from '@/components/common/TaktDictTag/index.vue'
import { useEcMasterContext } from '../composables/useEcMasterContext'
import { useEcAttachmentPreview } from '../composables/useEcAttachmentPreview'
import {
  getEcAttachmentList,
  getEcAttachmentById,
  createEcAttachment,
  updateEcAttachment,
  deleteEcAttachmentById,
  deleteEcAttachmentBatch,
  getEcAttachmentTemplate,
  importEcAttachment,
  exportEcAttachment,
} from '@/api/logistics/manufacturing/engineering-change/ecAttachment.api'
import type { EcAttachment, EcAttachmentQuery } from '@/types/logistics/manufacturing/engineering-change/ecAttachment.types'
import { useEcAttachmentI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcAttachmentI18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'

const { t } = useI18n()
const ai = useEcAttachmentI18n()
/** 附件弹窗宽度：与来源设变导入弹窗相同，（视口 − 左侧菜单）× 80% */
const ecGijutsuModalWidthPx = useTaktContentModalWidth()
const { selectedMasterRow } = useEcMasterContext()
const {
  canPreviewAttachment,
  hasPreviewableAccessUrl,
  handleAttachmentDocCodeClick,
} = useEcAttachmentPreview()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcAttachment')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: ai.self() }),
)

const loading = ref(false)
/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度裁掉横向滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: false })
}

/** 监听子表容器尺寸变化 */
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

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
const dataSource = ref<EcAttachment[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<EcAttachment | null>(null)
const selectedRows = ref<EcAttachment[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EcAttachment>>({})
const formLoading = ref(false)
const formRef = ref()

/** 当前编辑行以外的文件编码（当前页列表查重；最终以后端为准） */
const attachmentSiblingDocCodes = computed(() => {
  const editingId = String(formData.value?.ecAttachmentId ?? '')
  return dataSource.value
    .filter((row) => String(row.ecAttachmentId ?? '') !== editingId)
    .map((row) => String(row.docCode ?? '').trim())
    .filter(Boolean)
})

/** 当前编辑行以外的文件名称（当前页列表查重；最终以后端为准） */
const attachmentSiblingFileNames = computed(() => {
  const editingId = String(formData.value?.ecAttachmentId ?? '')
  return dataSource.value
    .filter((row) => String(row.ecAttachmentId ?? '') !== editingId)
    .map((row) => String(row.fileName ?? '').trim())
    .filter(Boolean)
})

/** 主表当前设变号码 */
const currentMasterEcCode = computed(() =>
  String((selectedMasterRow.value?.ecCode ?? selectedMasterRow.value?.ecNo) ?? '').trim(),
)

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ecCode: '',
  lineNumber: undefined as number | undefined,
  attachmentType: '',
  docCode: '',
  fileName: '',
  accessUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ecCode', label: ai.label('ecCode') },
  { key: 'lineNumber', label: ai.label('lineNumber') },
  { key: 'attachmentType', label: ai.label('attachmentType') },
  { key: 'docCode', label: ai.label('docCode') },
  { key: 'fileName', label: ai.label('fileName') },
  { key: 'accessUrl', label: ai.label('accessUrl') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  ecCode: '',
  lineNumber: undefined as number | undefined,
  attachmentType: '',
  docCode: '',
  fileName: '',
  accessUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'ecAttachmentId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.ecGijutsuId)
const masterEcId = computed(() => selectedMasterRow.value?.ecGijutsuId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getEcAttachmentId(record: EcAttachment | Record<string, unknown>): string {
  return String((record as EcAttachment)?.[entityIdName] ?? '')
}

function getEcAttachmentField(record: EcAttachment | Record<string, unknown>, field: string): unknown {
  return (record as EcAttachment)?.[field as keyof EcAttachment]
}

/**
 * 文件编码列：有访问地址且具备 preview 权限时渲染为超链接（鉴权 preview API，不跳转 /uploads 页面）
 * @param record 附件行
 */
function renderAttachmentDocCodeCell(record: EcAttachment): ReturnType<typeof h> | string {
  const code = String(getEcAttachmentField(record, 'docCode') ?? '').trim()
  const canOpen =
    canPreviewAttachment.value &&
    hasPreviewableAccessUrl(getEcAttachmentField(record, 'accessUrl')) &&
    !!code
  if (canOpen) {
    return h(
      'a',
      {
        href: '#',
        class: 'text-primary hover:underline',
        title: t('common.page.button.preview'),
        onClick: (e: MouseEvent) => {
          handleAttachmentDocCodeClick(record as unknown as Record<string, unknown>, e)
        },
      },
      code,
    )
  }
  return code
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecAttachmentId',
    key: 'ecAttachmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'ecAttachmentId') ?? ''),
  },
  {
    title: ai.label('ecCode'),
    dataIndex: 'ecCode',
    key: 'ecCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'ecCode') ?? ''),
  },
  {
    title: ai.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'lineNumber') ?? ''),
  },
  {
    title: ai.label('attachmentType'),
    dataIndex: 'attachmentType',
    key: 'attachmentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) => h(TaktDictTag, {
      dictType: 'logistics_manufacturing_ec_attachment_type',
      value: String(getEcAttachmentField(record, 'attachmentType') ?? ''),
    }),
  },
  {
    title: ai.label('docCode'),
    dataIndex: 'docCode',
    key: 'docCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) => renderAttachmentDocCodeCell(record),
  },
  {
    title: ai.label('fileName'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'fileName') ?? ''),
  },
  {
    title: ai.label('accessUrl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) =>
      String(getEcAttachmentField(record, 'accessUrl') ?? ''),
  },
  {
    title: ai.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: EcAttachment }) => h(TaktDictTag, {
      dictType: 'sys_yes_no',
      value: String(getEcAttachmentField(record, 'isObsolete') ?? ''),
    }),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:update',
        onClick: (record: EcAttachment) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:engineering:change:gijutsu:delete',
        onClick: (record: EcAttachment) => void handleDeleteOne(record),
      },
    ],
  }),
])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcAttachment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcAttachment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (
      selectedRow.value != null
      && getEcAttachmentId(selectedRow.value) === getEcAttachmentId(record)
    ) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: EcAttachment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EcAttachment) {
  const key = getEcAttachmentId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcAttachmentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcAttachmentQuery>): EcAttachmentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcAttachmentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ecGijutsuId: masterEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcAttachmentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecCode', form.ecCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('attachmentType', form.attachmentType)
  assignTrimmed('docCode', form.docCode)
  assignTrimmed('fileName', form.fileName)
  assignTrimmed('accessUrl', form.accessUrl)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getEcAttachmentList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterEcId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selected) => {
  if (selected) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: ai.self() })
  const ecCode = String((selectedMasterRow.value?.ecCode ?? selectedMasterRow.value?.ecNo) ?? '').trim()
  formData.value = {
    ecCode,
    attachmentType: 'EC',
    docCode: ecCode,
  }
  formVisible.value = true
}

async function handleEdit(record: EcAttachment) {
  formTitle.value = t('common.dialog.title.edit', { entity: ai.self() })
  formLoading.value = true
  try {
    const detail = await getEcAttachmentById(getEcAttachmentId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: ai.self(),
    }))
  }
}

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
    const payload = refInst.getValues?.()
    const id = formData.value?.ecAttachmentId
    if (id) {
      await updateEcAttachment(id, payload)
      message.success(t('common.feedback.updated', { target: ai.self() }))
    } else {
      await createEcAttachment(payload)
      message.success(t('common.feedback.created', { target: ai.self() }))
    }
    formVisible.value = false
    formData.value = {}
    nextTick(() => formRef.value?.resetFields?.())
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
  formData.value = {}
  nextTick(() => formRef.value?.resetFields?.())
}

async function handleDeleteOne(record: EcAttachment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: ai.self(),
      name: t('common.tip.this.target', { target: ai.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcAttachmentById(getEcAttachmentId(record))
      message.success(t('common.feedback.deleted', { target: ai.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: ai.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: ai.self(),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getEcAttachmentId(r)).filter(Boolean)
      await deleteEcAttachmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: ai.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEcAttachmentTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcAttachment(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    const exportMeta = await exportEcAttachment(
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
    message.success(t('common.feedback.export.success', { target: ai.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: ai.self() }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>

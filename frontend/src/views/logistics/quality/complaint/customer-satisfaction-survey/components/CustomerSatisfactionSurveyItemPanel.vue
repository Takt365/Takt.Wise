<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-satisfaction-survey/components -->
<!-- 文件名称：CustomerSatisfactionSurveyItemPanel.vue -->
<!-- 功能描述：客户满意度调查表主表实体主表实体右侧明细 customerSatisfactionSurveyItem 独立 CRUD（按主表选中 customerSatisfactionSurveyId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="customer-satisfaction-survey-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:complaint:customer:satisfaction:survey:create"
      update-permission="logistics:quality:complaint:customer:satisfaction:survey:update"
      delete-permission="logistics:quality:complaint:customer:satisfaction:survey:delete"
      import-permission="logistics:quality:complaint:customer:satisfaction:survey:import"
      export-permission="logistics:quality:complaint:customer:satisfaction:survey:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="false"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @column-setting="handleColumnSetting"
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
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="customer-satisfaction-survey-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getCustomerSatisfactionSurveyItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="customerSatisfactionSurveyItemId"
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
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <CustomerSatisfactionSurveyItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterCustomerSatisfactionSurveyId"
        :master-row="selecmedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

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
        :entity-i18n-key="CUSTOMERSATISFACTIONSURVEYITEM_SELF_I18N_KEY"
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
      id-column-key="customerSatisfactionSurveyItemId"
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
 * 客户满意度调查表主表实体子表 customerSatisfactionSurveyItem 右栏面板
 * @module views/logistics/quality/complaint/customer-satisfaction-survey/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'
import CustomerSatisfactionSurveyItemForm from './CustomerSatisfactionSurveyItemForm.vue'
import { useCustomerSatisfactionSurveyMasterContext } from '../composables/useCustomerSatisfactionSurveyMasterContext'
import {
  getCustomerSatisfactionSurveyItemList,
  getCustomerSatisfactionSurveyItemById,
  createCustomerSatisfactionSurveyItem,
  updateCustomerSatisfactionSurveyItem,
  deleteCustomerSatisfactionSurveyItemById,
  deleteCustomerSatisfactionSurveyItemBatch,
  getCustomerSatisfactionSurveyItemTemplate,
  importCustomerSatisfactionSurveyItem,
  exportCustomerSatisfactionSurveyItem,
} from '@/api/logistics/quality/complaint/customerSatisfactionSurveyItem.api'
import type { CustomerSatisfactionSurveyItem, CustomerSatisfactionSurveyItemQuery } from '@/types/logistics/quality/complaint/customerSatisfactionSurveyItem.types'

import {
  useCustomerSatisfactionSurveyItemI18n,
  CUSTOMERSATISFACTIONSURVEYITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS,
  CUSTOMERSATISFACTIONSURVEYITEM_QUERY_STRING_FIELDS,
  CUSTOMERSATISFACTIONSURVEYITEM_QUERY_FIELDS,
  CUSTOMERSATISFACTIONSURVEYITEM_SELF_I18N_KEY,
} from '../composables/useCustomerSatisfactionSurveyItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCustomerSatisfactionSurveyItemI18n()

const { t } = useI18n()
const { selecmedMasterRow } = useCustomerSatisfactionSurveyMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerSatisfactionSurveyItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
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
const dataSource = ref<CustomerSatisfactionSurveyItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<CustomerSatisfactionSurveyItem | null>(null)
const selecmedRows = ref<CustomerSatisfactionSurveyItem[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<CustomerSatisfactionSurveyItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...CUSTOMERSATISFACTIONSURVEYITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...CUSTOMERSATISFACTIONSURVEYITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'customerSatisfactionSurveyItemId'
const masterCustomerSatisfactionSurveyId = computed((): string => {
  const id = (selecmedMasterRow.value as Record<string, unknown> | null)?.['customerSatisfactionSurveyId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterCustomerSatisfactionSurveyId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getCustomerSatisfactionSurveyItemId(record: CustomerSatisfactionSurveyItem | Record<string, unknown>): string {
  return String((record as CustomerSatisfactionSurveyItem)?.[entityIdName] ?? '')
}

function getCustomerSatisfactionSurveyItemField(record: CustomerSatisfactionSurveyItem | Record<string, unknown>, field: string): unknown {
  return (record as CustomerSatisfactionSurveyItem)?.[field as keyof CustomerSatisfactionSurveyItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerSatisfactionSurveyItemId',
    key: 'customerSatisfactionSurveyItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'customerSatisfactionSurveyItemId') ?? ''),
  },
  {
    title: pi.label('customerSatisfactionSurveyCode'),
    dataIndex: 'customerSatisfactionSurveyCode',
    key: 'customerSatisfactionSurveyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'customerSatisfactionSurveyCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('categoryType'),
    dataIndex: 'categoryType',
    key: 'categoryType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'categoryType') ?? ''),
  },
  {
    title: pi.label('itemName'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'itemName') ?? ''),
  },
  {
    title: pi.label('itemDescription'),
    dataIndex: 'itemDescription',
    key: 'itemDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'itemDescription') ?? ''),
  },
  {
    title: pi.label('weight'),
    dataIndex: 'weight',
    key: 'weight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'weight') ?? ''),
  },
  {
    title: pi.label('score'),
    dataIndex: 'score',
    key: 'score',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'score') ?? ''),
  },
  {
    title: pi.label('satisfactionLevel'),
    dataIndex: 'satisfactionLevel',
    key: 'satisfactionLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'satisfactionLevel') ?? ''),
  },
  {
    title: pi.label('customerFeedback'),
    dataIndex: 'customerFeedback',
    key: 'customerFeedback',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'customerFeedback') ?? ''),
  },
  {
    title: pi.label('improvementSuggestion'),
    dataIndex: 'improvementSuggestion',
    key: 'improvementSuggestion',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'improvementSuggestion') ?? ''),
  },
  {
    title: pi.label('followUpAction'),
    dataIndex: 'followUpAction',
    key: 'followUpAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'followUpAction') ?? ''),
  },
  {
    title: pi.label('followUpStatus'),
    dataIndex: 'followUpStatus',
    key: 'followUpStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'followUpStatus') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'isObsolete') ?? ''),
  },
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: CustomerSatisfactionSurveyItem }) =>
      String(getCustomerSatisfactionSurveyItemField(record, 'remark') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customer:satisfaction:survey:update',
        onClick: (record: CustomerSatisfactionSurveyItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:satisfaction:survey:delete',
        onClick: (record: CustomerSatisfactionSurveyItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'customerSatisfactionSurveyItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of CUSTOMERSATISFACTIONSURVEYITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getCustomerSatisfactionSurveyItemField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerSatisfactionSurveyItem[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerSatisfactionSurveyItem, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getCustomerSatisfactionSurveyItemId(selecmedRow.value) === getCustomerSatisfactionSurveyItemId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: CustomerSatisfactionSurveyItem[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: CustomerSatisfactionSurveyItem) {
  const key = getCustomerSatisfactionSurveyItemId(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CustomerSatisfactionSurveyItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerSatisfactionSurveyItemQuery>): CustomerSatisfactionSurveyItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerSatisfactionSurveyItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    customerSatisfactionSurveyId: masterCustomerSatisfactionSurveyId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerSatisfactionSurveyItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of CUSTOMERSATISFACTIONSURVEYITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getCustomerSatisfactionSurveyItemList(buildListQuery())
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
watch(masterCustomerSatisfactionSurveyId, () => {
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

watch(hasMasterSelection, (selecmed) => {
  if (selecmed) {
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: CustomerSatisfactionSurveyItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getCustomerSatisfactionSurveyItemById(getCustomerSatisfactionSurveyItemId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
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
    const id = formData.value?.customerSatisfactionSurveyItemId
    if (id) {
      await updateCustomerSatisfactionSurveyItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCustomerSatisfactionSurveyItem(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: CustomerSatisfactionSurveyItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerSatisfactionSurveyItemById(getCustomerSatisfactionSurveyItemId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getCustomerSatisfactionSurveyItemId(r)).filter(Boolean)
      await deleteCustomerSatisfactionSurveyItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getCustomerSatisfactionSurveyItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCustomerSatisfactionSurveyItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
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
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportCustomerSatisfactionSurveyItem(
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
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

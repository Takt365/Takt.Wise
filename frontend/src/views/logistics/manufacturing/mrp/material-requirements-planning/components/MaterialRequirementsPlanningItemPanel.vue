<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/components -->
<!-- 文件名称：MaterialRequirementsPlanningItemPanel.vue -->
<!-- 功能描述：物料需求计划 MRP 头表主表实体右侧明细 materialRequirementsPlanningItem 独立 CRUD（按主表选中 materialRequirementsPlanningId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-requirements-planning-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:mrp:material:requirements:planning:create"
      update-permission="logistics:manufacturing:mrp:material:requirements:planning:update"
      delete-permission="logistics:manufacturing:mrp:material:requirements:planning:delete"
      import-permission="logistics:manufacturing:mrp:material:requirements:planning:import"
      export-permission="logistics:manufacturing:mrp:material:requirements:planning:export"
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
      class="material-requirements-planning-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="approval"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getMaterialRequirementsPlanningItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="materialRequirementsPlanningItemId"
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
      <MaterialRequirementsPlanningItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterMaterialRequirementsPlanningId"
        :master-row="selectedMasterRow"
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
        :entity-i18n-key="MATERIALREQUIREMENTSPLANNINGITEM_SELF_I18N_KEY"
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
      id-column-key="materialRequirementsPlanningItemId"
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
 * 物料需求计划 MRP 头表子表 materialRequirementsPlanningItem 右栏面板
 * @module views/logistics/manufacturing/mrp/material-requirements-planning/components
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
import MaterialRequirementsPlanningItemForm from './MaterialRequirementsPlanningItemForm.vue'
import { useMaterialRequirementsPlanningMasterContext } from '../composables/useMaterialRequirementsPlanningMasterContext'
import {
  getMaterialRequirementsPlanningItemList,
  getMaterialRequirementsPlanningItemById,
  createMaterialRequirementsPlanningItem,
  updateMaterialRequirementsPlanningItem,
  deleteMaterialRequirementsPlanningItemById,
  deleteMaterialRequirementsPlanningItemBatch,
  getMaterialRequirementsPlanningItemTemplate,
  importMaterialRequirementsPlanningItem,
  exportMaterialRequirementsPlanningItem,
} from '@/api/logistics/manufacturing/mrp/materialRequirementsPlanningItem.api'
import type { MaterialRequirementsPlanningItem, MaterialRequirementsPlanningItemQuery } from '@/types/logistics/manufacturing/mrp/materialRequirementsPlanningItem.types'

import {
  useMaterialRequirementsPlanningItemI18n,
  MATERIALREQUIREMENTSPLANNINGITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS,
  MATERIALREQUIREMENTSPLANNINGITEM_QUERY_STRING_FIELDS,
  MATERIALREQUIREMENTSPLANNINGITEM_QUERY_FIELDS,
  MATERIALREQUIREMENTSPLANNINGITEM_SELF_I18N_KEY,
} from '../composables/useMaterialRequirementsPlanningItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useMaterialRequirementsPlanningItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useMaterialRequirementsPlanningMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaterialRequirementsPlanningItem')
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
const dataSource = ref<MaterialRequirementsPlanningItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaterialRequirementsPlanningItem | null>(null)
const selectedRows = ref<MaterialRequirementsPlanningItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaterialRequirementsPlanningItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...MATERIALREQUIREMENTSPLANNINGITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...MATERIALREQUIREMENTSPLANNINGITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'materialRequirementsPlanningItemId'
const masterMaterialRequirementsPlanningId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['materialRequirementsPlanningId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterMaterialRequirementsPlanningId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaterialRequirementsPlanningItemId(record: MaterialRequirementsPlanningItem | Record<string, unknown>): string {
  return String((record as MaterialRequirementsPlanningItem)?.[entityIdName] ?? '')
}

function getMaterialRequirementsPlanningItemField(record: MaterialRequirementsPlanningItem | Record<string, unknown>, field: string): unknown {
  return (record as MaterialRequirementsPlanningItem)?.[field as keyof MaterialRequirementsPlanningItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'materialRequirementsPlanningItemId',
    key: 'materialRequirementsPlanningItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'materialRequirementsPlanningItemId') ?? ''),
  },
  {
    title: pi.label('materialRequirementsPlanningCode'),
    dataIndex: 'materialRequirementsPlanningCode',
    key: 'materialRequirementsPlanningCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'materialRequirementsPlanningCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('materialDescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'materialDescription') ?? ''),
  },
  {
    title: pi.label('materialSpecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: pi.label('modelCode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'modelCode') ?? ''),
  },
  {
    title: pi.label('modelName'),
    dataIndex: 'modelName',
    key: 'modelName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'modelName') ?? ''),
  },
  {
    title: pi.label('parentMaterialCode'),
    dataIndex: 'parentMaterialCode',
    key: 'parentMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'parentMaterialCode') ?? ''),
  },
  {
    title: pi.label('bomLevel'),
    dataIndex: 'bomLevel',
    key: 'bomLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'bomLevel') ?? ''),
  },
  {
    title: pi.label('requirementDate'),
    dataIndex: 'requirementDate',
    key: 'requirementDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'requirementDate') ?? ''),
  },
  {
    title: pi.label('planUnit'),
    dataIndex: 'planUnit',
    key: 'planUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'planUnit') ?? ''),
  },
  {
    title: pi.label('grossRequirement'),
    dataIndex: 'grossRequirement',
    key: 'grossRequirement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'grossRequirement') ?? ''),
  },
  {
    title: pi.label('scheduledReceipts'),
    dataIndex: 'scheduledReceipts',
    key: 'scheduledReceipts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'scheduledReceipts') ?? ''),
  },
  {
    title: pi.label('onHandQuantity'),
    dataIndex: 'onHandQuantity',
    key: 'onHandQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'onHandQuantity') ?? ''),
  },
  {
    title: pi.label('projectedOnHand'),
    dataIndex: 'projectedOnHand',
    key: 'projectedOnHand',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'projectedOnHand') ?? ''),
  },
  {
    title: pi.label('netRequirement'),
    dataIndex: 'netRequirement',
    key: 'netRequirement',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'netRequirement') ?? ''),
  },
  {
    title: pi.label('procurementType'),
    dataIndex: 'procurementType',
    key: 'procurementType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'procurementType') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'isObsolete') ?? ''),
  },
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaterialRequirementsPlanningItem }) =>
      String(getMaterialRequirementsPlanningItemField(record, 'remark') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:mrp:material:requirements:planning:update',
        onClick: (record: MaterialRequirementsPlanningItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:mrp:material:requirements:planning:delete',
        onClick: (record: MaterialRequirementsPlanningItem) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'approval')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'materialRequirementsPlanningItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'approval',
  })
})

const summarySumFieldSet = new Set<string>(MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS)

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
    MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of MATERIALREQUIREMENTSPLANNINGITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getMaterialRequirementsPlanningItemField(row, field))
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
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaterialRequirementsPlanningItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaterialRequirementsPlanningItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getMaterialRequirementsPlanningItemId(selectedRow.value) === getMaterialRequirementsPlanningItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaterialRequirementsPlanningItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaterialRequirementsPlanningItem) {
  const key = getMaterialRequirementsPlanningItemId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaterialRequirementsPlanningItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaterialRequirementsPlanningItemQuery>): MaterialRequirementsPlanningItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaterialRequirementsPlanningItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    materialRequirementsPlanningId: masterMaterialRequirementsPlanningId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaterialRequirementsPlanningItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of MATERIALREQUIREMENTSPLANNINGITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
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
    const res = await getMaterialRequirementsPlanningItemList(buildListQuery())
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
watch(masterMaterialRequirementsPlanningId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaterialRequirementsPlanningItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getMaterialRequirementsPlanningItemById(getMaterialRequirementsPlanningItemId(record))
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
    const id = formData.value?.materialRequirementsPlanningItemId
    if (id) {
      await updateMaterialRequirementsPlanningItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createMaterialRequirementsPlanningItem(payload)
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

async function handleDeleteOne(record: MaterialRequirementsPlanningItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaterialRequirementsPlanningItemById(getMaterialRequirementsPlanningItemId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
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
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaterialRequirementsPlanningItemId(r)).filter(Boolean)
      await deleteMaterialRequirementsPlanningItemBatch(ids)
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
  const res = await getMaterialRequirementsPlanningItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importMaterialRequirementsPlanningItem(file, sheetName)
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
    const exportMeta = await exportMaterialRequirementsPlanningItem(
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

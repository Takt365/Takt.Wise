<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：MaterialCostItemPanel.vue -->
<!-- 功能描述：右栏 BOM 明细浏览表（无查询/工具栏；默认 X/F；按选中产品行过滤） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="material-cost-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <!-- 浏览只读：无查询栏/工具栏；默认过滤 ProductionRelated=X、PurchaseType=F -->
    <div
      ref="detailTableWrapRef"
      class="material-cost-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        table-mode="single"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getBomMaterialCostItemId"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="bomMaterialCostItemId"
        :pagination="false"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="false"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
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
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      :disabled="loading"
      @change="handleMasterDetailPaginationChange"
    />
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <BomMaterialCostItemForm
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-material-cost-material-cost-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('bomLevel')">
      <a-form-item :label="pi.queryLabel('bomLevel')">
        <a-input
          v-model:value="advancedQueryForm.bomLevel"
          :placeholder="pi.queryPh('bomLevel', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomItemCode')">
      <a-form-item :label="pi.queryLabel('bomItemCode')">
        <a-input
          v-model:value="advancedQueryForm.bomItemCode"
          :placeholder="pi.queryPh('bomItemCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCode')">
      <a-form-item :label="pi.queryLabel('productCode')">
        <a-input
          v-model:value="advancedQueryForm.productCode"
          :placeholder="pi.queryPh('productCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          :min="1"
          :step="10"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productDescription')">
      <a-form-item :label="pi.queryLabel('productDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.productDescription"
          :placeholder="pi.queryPh('productDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('componentCode')">
      <a-form-item :label="pi.queryLabel('componentCode')">
        <a-input
          v-model:value="advancedQueryForm.componentCode"
          :placeholder="pi.queryPh('componentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('componentDescription')">
      <a-form-item :label="pi.queryLabel('componentDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.componentDescription"
          :placeholder="pi.queryPh('componentDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('componentQuantity')">
      <a-form-item :label="pi.queryLabel('componentQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.componentQuantity"
          :placeholder="pi.queryPh('componentQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchIndicator')">
      <a-form-item :label="pi.queryLabel('batchIndicator')">
        <a-input
          v-model:value="advancedQueryForm.batchIndicator"
          :placeholder="pi.queryPh('batchIndicator', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionRelated')">
      <a-form-item :label="pi.queryLabel('productionRelated')">
        <a-input
          v-model:value="advancedQueryForm.productionRelated"
          :placeholder="pi.queryPh('productionRelated', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseType')">
      <a-form-item :label="pi.queryLabel('purchaseType')">
        <a-input
          v-model:value="advancedQueryForm.purchaseType"
          :placeholder="pi.queryPh('purchaseType', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('specialProcurementType')">
      <a-form-item :label="pi.queryLabel('specialProcurementType')">
        <a-input
          v-model:value="advancedQueryForm.specialProcurementType"
          :placeholder="pi.queryPh('specialProcurementType', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('profitCenterCode')">
      <a-form-item :label="pi.queryLabel('profitCenterCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.profitCenterCode"
          api-url="TaktProfitCenters/options"
          :placeholder="pi.queryPh('profitCenterCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movingAveragePrice')">
      <a-form-item :label="pi.queryLabel('movingAveragePrice')">
        <a-input-number
          v-model:value="advancedQueryForm.movingAveragePrice"
          :placeholder="pi.queryPh('movingAveragePrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movingPriceUnit')">
      <a-form-item :label="pi.queryLabel('movingPriceUnit')">
        <a-input-number
          v-model:value="advancedQueryForm.movingPriceUnit"
          :placeholder="pi.queryPh('movingPriceUnit', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('movingPriceCurrencyCode')">
      <a-form-item :label="pi.queryLabel('movingPriceCurrencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.movingPriceCurrencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('movingPriceCurrencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrganization')">
      <a-form-item :label="pi.queryLabel('purchaseOrganization')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrganization"
          :placeholder="pi.queryPh('purchaseOrganization', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseGroup')">
      <a-form-item :label="pi.queryLabel('purchaseGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseGroup"
          api-url="TaktPurchaseGroups/options"
          :placeholder="pi.queryPh('purchaseGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supplierCode')">
      <a-form-item :label="pi.queryLabel('supplierCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.supplierCode"
          api-url="TaktSuppliers/options"
          :placeholder="pi.queryPh('supplierCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netPurchasePrice')">
      <a-form-item :label="pi.queryLabel('netPurchasePrice')">
        <a-input-number
          v-model:value="advancedQueryForm.netPurchasePrice"
          :placeholder="pi.queryPh('netPurchasePrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasePriceUnit')">
      <a-form-item :label="pi.queryLabel('purchasePriceUnit')">
        <a-input-number
          v-model:value="advancedQueryForm.purchasePriceUnit"
          :placeholder="pi.queryPh('purchasePriceUnit', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseCurrencyCode')">
      <a-form-item :label="pi.queryLabel('purchaseCurrencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseCurrencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('purchaseCurrencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costingDateStart')">
      <a-form-item :label="pi.queryLabel('costingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.costingDateStart"
          :placeholder="pi.queryPh('costingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costingDateEnd')">
      <a-form-item :label="pi.queryLabel('costingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.costingDateEnd"
          :placeholder="pi.queryPh('costingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="BOMMATERIALCOSTITEM_SELF_I18N_KEY"
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
      id-column-key="bomMaterialCostItemId"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本主表子表 bomMaterialCostItem 右栏面板
 * @module views/logistics/manufacturing/bom/material-cost/components
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
  resolveTableSummaryLabelColumnKey,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { RiQuestionLine } from '@remixicon/vue'
import BomMaterialCostItemForm from './MaterialCostItemForm.vue'
import { useBomMaterialCostMasterContext } from '../composables/useMaterialCostMasterContext'
import {
  formatBomMaterialCostAmount,
  sumBomMaterialCostItemLineCosts,
} from '../utils/bomMaterialCostItemLineCost'
import { sortBomMaterialCostItemRowsByProductCodeLineNumber } from '../utils/bomMaterialCostItemSort'
import {
  getBomMaterialCostItemList,
  getBomMaterialCostItemById,
  createBomMaterialCostItem,
  updateBomMaterialCostItem,
  deleteBomMaterialCostItemById,
  deleteBomMaterialCostItemBatch,
  getBomMaterialCostItemTemplate,
  importBomMaterialCostItem,
  exportBomMaterialCostItem,
} from '@/api/logistics/manufacturing/bom/materialCostItem.api'
import type { BomMaterialCostItem, BomMaterialCostItemQuery } from '@/types/logistics/manufacturing/bom/materialCostItem.types'

import {
  useBomMaterialCostItemI18n,
  BOMMATERIALCOSTITEM_LIST_FIELDS,
  BOMMATERIALCOSTITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS,
  BOMMATERIALCOSTITEM_QUERY_FIELDS,
  BOMMATERIALCOSTITEM_SELF_I18N_KEY,
} from '../composables/useMaterialCostItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBomMaterialCostItemI18n()

const { t } = useI18n()
const { selecmedProductRow } = useBomMaterialCostMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBomMaterialCostItem')
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
const dataSource = ref<BomMaterialCostItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<BomMaterialCostItem | null>(null)
const selecmedRows = ref<BomMaterialCostItem[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<BomMaterialCostItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
/** 明细列表默认过滤：生产相关=X、采购类型=F（可清空后看全部） */
const DEFAULT_ITEM_PRODUCTION_RELATED = 'X'
const DEFAULT_ITEM_PURCHASE_TYPE = 'F'

/**
 * 创建高级查询表单（默认带 X/F；清空对应输入即不过滤该字段）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    productionRelated: DEFAULT_ITEM_PRODUCTION_RELATED,
    purchaseType: DEFAULT_ITEM_PURCHASE_TYPE,
    lineNumber: undefined as number | undefined,
    componentQuantity: undefined as number | undefined,
    movingAveragePrice: undefined as number | undefined,
    movingPriceUnit: undefined as number | undefined,
    netPurchasePrice: undefined as number | undefined,
    purchasePriceUnit: undefined as number | undefined,
  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  BOMMATERIALCOSTITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...BOMMATERIALCOSTITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...BOMMATERIALCOSTITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'bomMaterialCostItemId'

/**
 * 由选中产品行推导 Item 查询范围（工厂 + 产品 + 核算月）
 * @returns 业务键与月起止；未选中或期间非法时为 null
 */
function resolveProductItemScope(
  row: Record<string, unknown> | null,
): {
  plantCode: string
  productCode: string
  productDescription: string
  costingPeriod: string
  costingDate: string
  costingDateStart: string
  costingDateEnd: string
  scopeKey: string
} | null {
  if (!row) return null
  const plantCode = String(row.plantCode ?? '').trim()
  const productCode = String(row.productCode ?? '').trim()
  const costingPeriod = String(row.costingPeriod ?? '').trim()
  if (!plantCode || !productCode || !costingPeriod) return null
  const match = /^(\d{4})-(\d{2})$/.exec(costingPeriod)
  if (!match) return null
  const year = Number(match[1])
  const month = Number(match[2])
  if (!Number.isFinite(year) || month < 1 || month > 12) return null
  const pad = (n: number) => String(n).padStart(2, '0')
  const lastDay = new Date(year, month, 0).getDate()
  const costingDateRaw = row.costingDate != null ? String(row.costingDate) : ''
  const costingDate =
    costingDateRaw.length >= 10 ? costingDateRaw.slice(0, 10) : `${year}-${pad(month)}-01`
  return {
    plantCode,
    productCode,
    productDescription: String(row.productDescription ?? ''),
    costingPeriod,
    costingDate,
    costingDateStart: `${year}-${pad(month)}-01`,
    costingDateEnd: `${year}-${pad(month)}-${pad(lastDay)}`,
    scopeKey: `${plantCode}|${productCode}|${costingPeriod}`,
  }
}

/** 当前选中产品行对应的 Item 业务范围 */
const masterItemScope = computed(() =>
  resolveProductItemScope(selecmedProductRow.value as Record<string, unknown> | null),
)
const hasMasterSelection = computed(() => masterItemScope.value != null)
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getBomMaterialCostItemId(record: BomMaterialCostItem | Record<string, unknown>): string {
  return String((record as BomMaterialCostItem)?.[entityIdName] ?? '')
}

function getBomMaterialCostItemField(record: BomMaterialCostItem | Record<string, unknown>, field: string): unknown {
  return (record as BomMaterialCostItem)?.[field as keyof BomMaterialCostItem]
}

/** 列表业务列宽度（描述类略宽） */
const ITEM_FIELD_COLUMN_WIDTH: Partial<Record<(typeof BOMMATERIALCOSTITEM_LIST_FIELDS)[number], number>> = {
  productDescription: 140,
  componentDescription: 140,
  componentCode: 120,
  productCode: 120,
  costingDate: 110,
}

/** 右表明细列（浏览只读，无操作列；单价/净价固定 5 位小数） */
const columns = computed<TableColumnsType>(() =>
  BOMMATERIALCOSTITEM_LIST_FIELDS.map((field) => ({
    title: pi.label(field),
    dataIndex: field,
    key: field,
    width: ITEM_FIELD_COLUMN_WIDTH[field] ?? 100,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BomMaterialCostItem }) => {
      const raw = getBomMaterialCostItemField(record, field)
      if (field === 'movingAveragePrice' || field === 'netPurchasePrice') {
        return formatBomMaterialCostAmount(raw)
      }
      return String(raw ?? '')
    },
  })),
)

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'bomMaterialCostItemId',
    actionColumnKey: 'action',
    tableMode: 'single',
    entityScope: 'company',
  })
})

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/**
 * 当前页移动价格合计：Σ componentQuantity×(movingAveragePrice÷movingPriceUnit)，
 * 仅 productionRelated=X、pcbSectIndicator 为空、purchaseType=F（与后端 LineCostHelper 一致）
 */
const movingPriceCostTotal = computed(() => sumBomMaterialCostItemLineCosts(dataSource.value))

/** 汇总行单元格（无行选择列；文案在第一个非序号业务列；移动平均价列显示公式合计） */
const summaryCells = computed(() => {
  const labelKey = resolveTableSummaryLabelColumnKey(resolvedSummaryColumns.value)
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (labelKey && key === labelKey) {
      text = summaryLabel.value
    } else if (key === 'movingAveragePrice') {
      text = formatSummaryValue(movingPriceCostTotal.value, 5)
    }
    cells.push({
      key,
      text,
      index: columnIndex,
    })
  })
  return cells
})
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BomMaterialCostItem[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BomMaterialCostItem, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getBomMaterialCostItemId(selecmedRow.value) === getBomMaterialCostItemId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: BomMaterialCostItem[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: BomMaterialCostItem) {
  const key = getBomMaterialCostItemId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BomMaterialCostItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BomMaterialCostItemQuery>): BomMaterialCostItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BomMaterialCostItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BomMaterialCostItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of BOMMATERIALCOSTITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.componentQuantity !== undefined && form.componentQuantity !== null) {
    query.componentQuantity = form.componentQuantity
  }
  if (form.movingAveragePrice !== undefined && form.movingAveragePrice !== null) {
    query.movingAveragePrice = form.movingAveragePrice
  }
  if (form.movingPriceUnit !== undefined && form.movingPriceUnit !== null) {
    query.movingPriceUnit = form.movingPriceUnit
  }
  if (form.netPurchasePrice !== undefined && form.netPurchasePrice !== null) {
    query.netPurchasePrice = form.netPurchasePrice
  }
  if (form.purchasePriceUnit !== undefined && form.purchasePriceUnit !== null) {
    query.purchasePriceUnit = form.purchasePriceUnit
  }
  const scope = masterItemScope.value
  if (scope) {
    query.plantCode = scope.plantCode
    query.productCode = scope.productCode
    query.costingDateStart = scope.costingDateStart
    query.costingDateEnd = scope.costingDateEnd
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
    const res = await getBomMaterialCostItemList(buildListQuery())
    dataSource.value = sortBomMaterialCostItemRowsByProductCodeLineNumber(res.data ?? [])
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
    await nextTick()
    startDetailTableScrollObserve()
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 汇总行业务键变更时重新加载相关明细（含首次挂载后选中） */
watch(
  () => masterItemScope.value?.scopeKey ?? '',
  () => {
    reload()
  },
  { immediate: true },
)

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

/** 可选过滤变更后立即刷新（清空 productionRelated/purchaseType 即查全部） */
function handleItemFilterChange() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  const scope = masterItemScope.value
  if (!scope) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {
    plantCode: scope.plantCode,
    productCode: scope.productCode,
    productDescription: scope.productDescription,
    costingDate: scope.costingDate,
  }
  formVisible.value = true
}

async function handleEdit(record: BomMaterialCostItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getBomMaterialCostItemById(getBomMaterialCostItemId(record))
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
    const id = formData.value?.bomMaterialCostItemId
    if (id) {
      await updateBomMaterialCostItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createBomMaterialCostItem(payload)
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

async function handleDeleteOne(record: BomMaterialCostItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBomMaterialCostItemById(getBomMaterialCostItemId(record))
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
      const ids = selecmedRows.value.map((r) => getBomMaterialCostItemId(r)).filter(Boolean)
      await deleteBomMaterialCostItemBatch(ids)
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
  const res = await getBomMaterialCostItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importBomMaterialCostItem(file, sheetName)
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
    const exportMeta = await exportBomMaterialCostItem(
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
 * 明细面板分页变更
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

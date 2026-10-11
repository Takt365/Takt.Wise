<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/quotation/components -->
<!-- 文件名称：QuotationItemPanel.vue -->
<!-- 功能描述：Takt销售报价实体主表实体右侧明细 salesQuotationItem 独立 CRUD（按主表选中 salesQuotationId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="quotation-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:sales:quotation:create"
      update-permission="logistics:sales:quotation:update"
      delete-permission="logistics:sales:quotation:delete"
      import-permission="logistics:sales:quotation:import"
      export-permission="logistics:sales:quotation:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
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
      class="quotation-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getSalesQuotationItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="salesQuotationItemId"
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
      <SalesQuotationItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterSalesQuotationId"
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-sales-quotation-quotation-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesQuotationCode')">
      <a-form-item :label="pi.queryLabel('salesQuotationCode')">
        <a-input
          v-model:value="advancedQueryForm.salesQuotationCode"
          :placeholder="pi.queryPh('salesQuotationCode', 'required')"
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
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktMaterialPlants/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="pi.queryLabel('materialDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="pi.queryPh('materialDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialSpecification')">
      <a-form-item :label="pi.queryLabel('materialSpecification')">
        <a-input
          v-model:value="advancedQueryForm.materialSpecification"
          :placeholder="pi.queryPh('materialSpecification', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesUnit')">
      <a-form-item :label="pi.queryLabel('salesUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('salesUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationQuantity')">
      <a-form-item :label="pi.queryLabel('quotationQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quotationQuantity"
          :placeholder="pi.queryPh('quotationQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesPerUnit')">
      <a-form-item :label="pi.queryLabel('salesPerUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesPerUnit"
          dict-type="logistics_materials_price_unit_param"
          :placeholder="pi.queryPh('salesPerUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationUnitPrice')">
      <a-form-item :label="pi.queryLabel('quotationUnitPrice')">
        <a-input-number
          v-model:value="advancedQueryForm.quotationUnitPrice"
          :placeholder="pi.queryPh('quotationUnitPrice', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountRate')">
      <a-form-item :label="pi.queryLabel('discountRate')">
        <TaktSelect
          v-model:value="advancedQueryForm.discountRate"
          dict-type="logistics_sales_discount_rate_param"
          :placeholder="pi.queryPh('discountRate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="pi.queryLabel('discountAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="pi.queryPh('discountAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxCode')">
      <a-form-item :label="pi.queryLabel('taxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxCode"
          dict-type="accounting_financial_tax_code"
          :placeholder="pi.queryPh('taxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxIncludedAmount')">
      <a-form-item :label="pi.queryLabel('taxIncludedAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxIncludedAmount"
          :placeholder="pi.queryPh('taxIncludedAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('untaxedAmount')">
      <a-form-item :label="pi.queryLabel('untaxedAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.untaxedAmount"
          :placeholder="pi.queryPh('untaxedAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="pi.queryLabel('taxAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="pi.queryPh('taxAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationAmount')">
      <a-form-item :label="pi.queryLabel('quotationAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.quotationAmount"
          :placeholder="pi.queryPh('quotationAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingDateStart')">
      <a-form-item :label="pi.queryLabel('pricingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.pricingDateStart"
          :placeholder="pi.queryPh('pricingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingDateEnd')">
      <a-form-item :label="pi.queryLabel('pricingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.pricingDateEnd"
          :placeholder="pi.queryPh('pricingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossWeight')">
      <a-form-item :label="pi.queryLabel('grossWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.grossWeight"
          :placeholder="pi.queryPh('grossWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netWeight')">
      <a-form-item :label="pi.queryLabel('netWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.netWeight"
          :placeholder="pi.queryPh('netWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weightUnit')">
      <a-form-item :label="pi.queryLabel('weightUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.weightUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('weightUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volume')">
      <a-form-item :label="pi.queryLabel('volume')">
        <a-input-number
          v-model:value="advancedQueryForm.volume"
          :placeholder="pi.queryPh('volume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volumeUnit')">
      <a-form-item :label="pi.queryLabel('volumeUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.volumeUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('volumeUnit', 'select')"
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
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isObsolete', 'select')"
          allow-clear
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
        :entity-i18n-key="SALESQUOTATIONITEM_SELF_I18N_KEY"
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
      id-column-key="salesQuotationItemId"
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
 * Takt销售报价实体子表 salesQuotationItem 右栏面板
 * @module views/logistics/sales/quotation/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
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
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import SalesQuotationItemForm from './QuotationItemForm.vue'
import { useSalesQuotationMasterContext } from '../composables/useQuotationMasterContext'
import {
  getSalesQuotationItemList,
  getSalesQuotationItemById,
  createSalesQuotationItem,
  updateSalesQuotationItem,
  deleteSalesQuotationItemById,
  deleteSalesQuotationItemBatch,
  getSalesQuotationItemTemplate,
  importSalesQuotationItem,
  exportSalesQuotationItem,
} from '@/api/logistics/sales/quotationItem.api'
import type { SalesQuotationItem, SalesQuotationItemQuery } from '@/types/logistics/sales/quotationItem.types'

import {
  useSalesQuotationItemI18n,
  SALESQUOTATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS,
  SALESQUOTATIONITEM_QUERY_STRING_FIELDS,
  SALESQUOTATIONITEM_QUERY_FIELDS,
  SALESQUOTATIONITEM_SELF_I18N_KEY,
} from '../composables/useQuotationItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesQuotationItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = useSalesQuotationMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesQuotationItem')
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
const dataSource = ref<SalesQuotationItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<SalesQuotationItem | null>(null)
const selectedRows = ref<SalesQuotationItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<SalesQuotationItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

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
  for (const key of SALESQUOTATIONITEM_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.quotationQuantity !== undefined && form.quotationQuantity !== null) {
    return true
  }
  if (form.salesPerUnit !== undefined && form.salesPerUnit !== null) {
    return true
  }
  if (form.quotationUnitPrice !== undefined && form.quotationUnitPrice !== null) {
    return true
  }
  if (form.discountRate !== undefined && form.discountRate !== null) {
    return true
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    return true
  }
  if (form.taxIncludedAmount !== undefined && form.taxIncludedAmount !== null) {
    return true
  }
  if (form.untaxedAmount !== undefined && form.untaxedAmount !== null) {
    return true
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    return true
  }
  if (form.quotationAmount !== undefined && form.quotationAmount !== null) {
    return true
  }
  if (form.grossWeight !== undefined && form.grossWeight !== null) {
    return true
  }
  if (form.netWeight !== undefined && form.netWeight !== null) {
    return true
  }
  if (form.volume !== undefined && form.volume !== null) {
    return true
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESQUOTATIONITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESQUOTATIONITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    quotationQuantity: undefined as number | undefined,
    salesPerUnit: undefined as number | undefined,
    quotationUnitPrice: undefined as number | undefined,
    discountRate: undefined as number | undefined,
    discountAmount: undefined as number | undefined,
    taxIncludedAmount: undefined as number | undefined,
    untaxedAmount: undefined as number | undefined,
    taxAmount: undefined as number | undefined,
    quotationAmount: undefined as number | undefined,
    grossWeight: undefined as number | undefined,
    netWeight: undefined as number | undefined,
    volume: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  SALESQUOTATIONITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...SALESQUOTATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...SALESQUOTATIONITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'salesQuotationItemId'
const masterSalesQuotationId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['salesQuotationId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterSalesQuotationId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getSalesQuotationItemId(record: SalesQuotationItem | Record<string, unknown>): string {
  return String((record as SalesQuotationItem)?.[entityIdName] ?? '')
}

function getSalesQuotationItemField(record: SalesQuotationItem | Record<string, unknown>, field: string): unknown {
  return (record as SalesQuotationItem)?.[field as keyof SalesQuotationItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salesQuotationItemId',
    key: 'salesQuotationItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationItemId') ?? ''),
  },
  {
    title: pi.label('salesQuotationId'),
    dataIndex: 'salesQuotationId',
    key: 'salesQuotationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationId') ?? ''),
  },
  {
    title: pi.label('salesQuotationName'),
    dataIndex: 'salesQuotationName',
    key: 'salesQuotationName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationName') ?? ''),
  },
  {
    title: pi.label('salesQuotationCode'),
    dataIndex: 'salesQuotationCode',
    key: 'salesQuotationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesQuotationCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('materialDescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialDescription') ?? ''),
  },
  {
    title: pi.label('materialSpecification'),
    dataIndex: 'materialSpecification',
    key: 'materialSpecification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'materialSpecification') ?? ''),
  },
  {
    title: pi.label('salesUnit'),
    dataIndex: 'salesUnit',
    key: 'salesUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesUnit') ?? ''),
  },
  {
    title: pi.label('quotationQuantity'),
    dataIndex: 'quotationQuantity',
    key: 'quotationQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'quotationQuantity') ?? ''),
  },
  {
    title: pi.label('salesPerUnit'),
    dataIndex: 'salesPerUnit',
    key: 'salesPerUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'salesPerUnit') ?? ''),
  },
  {
    title: pi.label('quotationUnitPrice'),
    dataIndex: 'quotationUnitPrice',
    key: 'quotationUnitPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'quotationUnitPrice') ?? ''),
  },
  {
    title: pi.label('discountRate'),
    dataIndex: 'discountRate',
    key: 'discountRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'discountRate') ?? ''),
  },
  {
    title: pi.label('discountAmount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'discountAmount') ?? ''),
  },
  {
    title: pi.label('taxCode'),
    dataIndex: 'taxCode',
    key: 'taxCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'taxCode') ?? ''),
  },
  {
    title: pi.label('taxIncludedAmount'),
    dataIndex: 'taxIncludedAmount',
    key: 'taxIncludedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'taxIncludedAmount') ?? ''),
  },
  {
    title: pi.label('untaxedAmount'),
    dataIndex: 'untaxedAmount',
    key: 'untaxedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'untaxedAmount') ?? ''),
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'taxAmount') ?? ''),
  },
  {
    title: pi.label('quotationAmount'),
    dataIndex: 'quotationAmount',
    key: 'quotationAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'quotationAmount') ?? ''),
  },
  {
    title: pi.label('pricingDate'),
    dataIndex: 'pricingDate',
    key: 'pricingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'pricingDate') ?? ''),
  },
  {
    title: pi.label('grossWeight'),
    dataIndex: 'grossWeight',
    key: 'grossWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'grossWeight') ?? ''),
  },
  {
    title: pi.label('netWeight'),
    dataIndex: 'netWeight',
    key: 'netWeight',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'netWeight') ?? ''),
  },
  {
    title: pi.label('weightUnit'),
    dataIndex: 'weightUnit',
    key: 'weightUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'weightUnit') ?? ''),
  },
  {
    title: pi.label('volume'),
    dataIndex: 'volume',
    key: 'volume',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'volume') ?? ''),
  },
  {
    title: pi.label('volumeUnit'),
    dataIndex: 'volumeUnit',
    key: 'volumeUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'volumeUnit') ?? ''),
  },
  {
    title: pi.label('profitCenterCode'),
    dataIndex: 'profitCenterCode',
    key: 'profitCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'profitCenterCode') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: SalesQuotationItem }) =>
      String(getSalesQuotationItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:quotation:update',
        onClick: (record: SalesQuotationItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:quotation:delete',
        onClick: (record: SalesQuotationItem) => void handleDeleteOne(record),
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
    idColumnKey: 'salesQuotationItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS)

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
    SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of SALESQUOTATIONITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getSalesQuotationItemField(row, field))
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
  onChange: (keys: (string | number)[], rows: SalesQuotationItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalesQuotationItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSalesQuotationItemId(selectedRow.value) === getSalesQuotationItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesQuotationItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: SalesQuotationItem) {
  const key = getSalesQuotationItemId(record)
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
 * @returns {SalesQuotationItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesQuotationItemQuery>): SalesQuotationItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesQuotationItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    salesQuotationId: masterSalesQuotationId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesQuotationItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESQUOTATIONITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.quotationQuantity !== undefined && form.quotationQuantity !== null) {
    query.quotationQuantity = form.quotationQuantity
  }
  if (form.salesPerUnit !== undefined && form.salesPerUnit !== null) {
    query.salesPerUnit = form.salesPerUnit
  }
  if (form.quotationUnitPrice !== undefined && form.quotationUnitPrice !== null) {
    query.quotationUnitPrice = form.quotationUnitPrice
  }
  if (form.discountRate !== undefined && form.discountRate !== null) {
    query.discountRate = form.discountRate
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.taxIncludedAmount !== undefined && form.taxIncludedAmount !== null) {
    query.taxIncludedAmount = form.taxIncludedAmount
  }
  if (form.untaxedAmount !== undefined && form.untaxedAmount !== null) {
    query.untaxedAmount = form.untaxedAmount
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.quotationAmount !== undefined && form.quotationAmount !== null) {
    query.quotationAmount = form.quotationAmount
  }
  if (form.grossWeight !== undefined && form.grossWeight !== null) {
    query.grossWeight = form.grossWeight
  }
  if (form.netWeight !== undefined && form.netWeight !== null) {
    query.netWeight = form.netWeight
  }
  if (form.volume !== undefined && form.volume !== null) {
    query.volume = form.volume
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
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
    const res = await getSalesQuotationItemList(buildListQuery())
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
watch(masterSalesQuotationId, () => {
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

async function handleEdit(record: SalesQuotationItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getSalesQuotationItemById(getSalesQuotationItemId(record))
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
    const id = formData.value?.salesQuotationItemId
    if (id) {
      await updateSalesQuotationItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesQuotationItem(payload)
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

async function handleDeleteOne(record: SalesQuotationItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesQuotationItemById(getSalesQuotationItemId(record))
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
      const ids = selectedRows.value.map((r) => getSalesQuotationItemId(r)).filter(Boolean)
      await deleteSalesQuotationItemBatch(ids)
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
  const res = await getSalesQuotationItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesQuotationItem(file, sheetName)
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
    const exportMeta = await exportSalesQuotationItem(
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

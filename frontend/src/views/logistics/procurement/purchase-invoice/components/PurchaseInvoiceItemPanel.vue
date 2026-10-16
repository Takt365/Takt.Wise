<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-invoice/components -->
<!-- 文件名称：PurchaseInvoiceItemPanel.vue -->
<!-- 功能描述：Takt采购发票主表实体主表实体右侧明细 purchaseInvoiceItem 独立 CRUD（按主表选中 purchaseInvoiceId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="purchase-invoice-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:procurement:purchase:invoice:create"
      update-permission="logistics:procurement:purchase:invoice:update"
      delete-permission="logistics:procurement:purchase:invoice:delete"
      import-permission="logistics:procurement:purchase:invoice:import"
      export-permission="logistics:procurement:purchase:invoice:export"
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
      class="purchase-invoice-item-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getPurchaseInvoiceItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="purchaseInvoiceItemId"
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
      <PurchaseInvoiceItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPurchaseInvoiceId"
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-procurement-purchase-invoice-purchase-invoice-item"
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
      <div v-show="isFieldVisible('purchaseInvoiceCode')">
      <a-form-item :label="pi.queryLabel('purchaseInvoiceCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInvoiceCode"
          :placeholder="pi.queryPh('purchaseInvoiceCode', 'required')"
          show-count
          :maxlength="10"
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
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="pi.queryLabel('purchaseOrderCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseOrderCode"
          api-url="TaktPurchaseOrders/options"
          :placeholder="pi.queryPh('purchaseOrderCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseOrderItem')">
      <a-form-item :label="pi.queryLabel('purchaseOrderItem')">
        <a-input-number
          v-model:value="advancedQueryForm.purchaseOrderItem"
          :placeholder="pi.queryPh('purchaseOrderItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountAssignmentSeq')">
      <a-form-item :label="pi.queryLabel('accountAssignmentSeq')">
        <a-input
          v-model:value="advancedQueryForm.accountAssignmentSeq"
          :placeholder="pi.queryPh('accountAssignmentSeq', 'required')"
          show-count
          :maxlength="20"
          allow-clear
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
      <div v-show="isFieldVisible('valuationArea')">
      <a-form-item :label="pi.queryLabel('valuationArea')">
        <a-input
          v-model:value="advancedQueryForm.valuationArea"
          :placeholder="pi.queryPh('valuationArea', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('amount')">
      <a-form-item :label="pi.queryLabel('amount')">
        <a-input-number
          v-model:value="advancedQueryForm.amount"
          :placeholder="pi.queryPh('amount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('debitCreditIndicator')">
      <a-form-item :label="pi.queryLabel('debitCreditIndicator')">
        <a-input
          v-model:value="advancedQueryForm.debitCreditIndicator"
          :placeholder="pi.queryPh('debitCreditIndicator', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxCode')">
      <a-form-item :label="pi.queryLabel('taxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxCode"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('taxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantity')">
      <a-form-item :label="pi.queryLabel('quantity')">
        <a-input-number
          v-model:value="advancedQueryForm.quantity"
          :placeholder="pi.queryPh('quantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderUnit')">
      <a-form-item :label="pi.queryLabel('orderUnit')">
        <a-input
          v-model:value="advancedQueryForm.orderUnit"
          :placeholder="pi.queryPh('orderUnit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('poPriceQuantity')">
      <a-form-item :label="pi.queryLabel('poPriceQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.poPriceQuantity"
          :placeholder="pi.queryPh('poPriceQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('poPriceUnit')">
      <a-form-item :label="pi.queryLabel('poPriceUnit')">
        <a-input
          v-model:value="advancedQueryForm.poPriceUnit"
          :placeholder="pi.queryPh('poPriceUnit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuatedStockQuantity')">
      <a-form-item :label="pi.queryLabel('valuatedStockQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.valuatedStockQuantity"
          :placeholder="pi.queryPh('valuatedStockQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('previousPeriodStock')">
      <a-form-item :label="pi.queryLabel('previousPeriodStock')">
        <a-input-number
          v-model:value="advancedQueryForm.previousPeriodStock"
          :placeholder="pi.queryPh('previousPeriodStock', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="pi.queryLabel('baseUnit')">
        <a-input
          v-model:value="advancedQueryForm.baseUnit"
          :placeholder="pi.queryPh('baseUnit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valuationClass')">
      <a-form-item :label="pi.queryLabel('valuationClass')">
        <a-input
          v-model:value="advancedQueryForm.valuationClass"
          :placeholder="pi.queryPh('valuationClass', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('updatePoHistoryFlag')">
      <a-form-item :label="pi.queryLabel('updatePoHistoryFlag')">
        <a-date-picker
          v-model:value="advancedQueryForm.updatePoHistoryFlag"
          :placeholder="pi.queryPh('updatePoHistoryFlag', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('subsequentDebitCredit')">
      <a-form-item :label="pi.queryLabel('subsequentDebitCredit')">
        <a-input
          v-model:value="advancedQueryForm.subsequentDebitCredit"
          :placeholder="pi.queryPh('subsequentDebitCredit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('blockReasonPrice')">
      <a-form-item :label="pi.queryLabel('blockReasonPrice')">
        <a-input
          v-model:value="advancedQueryForm.blockReasonPrice"
          :placeholder="pi.queryPh('blockReasonPrice', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('blockReasonQuantity')">
      <a-form-item :label="pi.queryLabel('blockReasonQuantity')">
        <a-input
          v-model:value="advancedQueryForm.blockReasonQuantity"
          :placeholder="pi.queryPh('blockReasonQuantity', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('blockReasonQuality')">
      <a-form-item :label="pi.queryLabel('blockReasonQuality')">
        <a-input
          v-model:value="advancedQueryForm.blockReasonQuality"
          :placeholder="pi.queryPh('blockReasonQuality', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('blockReasonEnhanced')">
      <a-form-item :label="pi.queryLabel('blockReasonEnhanced')">
        <a-input
          v-model:value="advancedQueryForm.blockReasonEnhanced"
          :placeholder="pi.queryPh('blockReasonEnhanced', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('valueString')">
      <a-form-item :label="pi.queryLabel('valueString')">
        <a-input
          v-model:value="advancedQueryForm.valueString"
          :placeholder="pi.queryPh('valueString', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceCode')">
      <a-form-item :label="pi.queryLabel('referenceCode')">
        <a-input
          v-model:value="advancedQueryForm.referenceCode"
          :placeholder="pi.queryPh('referenceCode', 'required')"
          show-count
          :maxlength="16"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conditionType')">
      <a-form-item :label="pi.queryLabel('conditionType')">
        <a-input
          v-model:value="advancedQueryForm.conditionType"
          :placeholder="pi.queryPh('conditionType', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalValuatedStockValue')">
      <a-form-item :label="pi.queryLabel('totalValuatedStockValue')">
        <a-input-number
          v-model:value="advancedQueryForm.totalValuatedStockValue"
          :placeholder="pi.queryPh('totalValuatedStockValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('previousPeriodValue')">
      <a-form-item :label="pi.queryLabel('previousPeriodValue')">
        <a-input-number
          v-model:value="advancedQueryForm.previousPeriodValue"
          :placeholder="pi.queryPh('previousPeriodValue', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentCode')">
      <a-form-item :label="pi.queryLabel('referenceDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.referenceDocumentCode"
          :placeholder="pi.queryPh('referenceDocumentCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentYear')">
      <a-form-item :label="pi.queryLabel('referenceDocumentYear')">
        <a-input
          v-model:value="advancedQueryForm.referenceDocumentYear"
          :placeholder="pi.queryPh('referenceDocumentYear', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('referenceDocumentItem')">
      <a-form-item :label="pi.queryLabel('referenceDocumentItem')">
        <a-input-number
          v-model:value="advancedQueryForm.referenceDocumentItem"
          :placeholder="pi.queryPh('referenceDocumentItem', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stockManagedMaterialCode')">
      <a-form-item :label="pi.queryLabel('stockManagedMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.stockManagedMaterialCode"
          :placeholder="pi.queryPh('stockManagedMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemText')">
      <a-form-item :label="pi.queryLabel('itemText')">
        <a-input
          v-model:value="advancedQueryForm.itemText"
          :placeholder="pi.queryPh('itemText', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDocumentItem')">
      <a-form-item :label="pi.queryLabel('materialDocumentItem')">
        <a-input-number
          v-model:value="advancedQueryForm.materialDocumentItem"
          :placeholder="pi.queryPh('materialDocumentItem', 'required')"
          style="width: 100%"
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
        :entity-i18n-key="PURCHASEINVOICEITEM_SELF_I18N_KEY"
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
      id-column-key="purchaseInvoiceItemId"
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
 * Takt采购发票主表实体子表 purchaseInvoiceItem 右栏面板
 * @module views/logistics/procurement/purchase-invoice/components
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
import PurchaseInvoiceItemForm from './PurchaseInvoiceItemForm.vue'
import { usePurchaseInvoiceMasterContext } from '../composables/usePurchaseInvoiceMasterContext'
import {
  getPurchaseInvoiceItemList,
  getPurchaseInvoiceItemById,
  createPurchaseInvoiceItem,
  updatePurchaseInvoiceItem,
  deletePurchaseInvoiceItemById,
  deletePurchaseInvoiceItemBatch,
  getPurchaseInvoiceItemTemplate,
  importPurchaseInvoiceItem,
  exportPurchaseInvoiceItem,
} from '@/api/logistics/procurement/purchaseInvoiceItem.api'
import type { PurchaseInvoiceItem, PurchaseInvoiceItemQuery } from '@/types/logistics/procurement/purchaseInvoiceItem.types'

import {
  usePurchaseInvoiceItemI18n,
  PURCHASEINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS,
  PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS,
  PURCHASEINVOICEITEM_QUERY_STRING_FIELDS,
  PURCHASEINVOICEITEM_QUERY_FIELDS,
  PURCHASEINVOICEITEM_SELF_I18N_KEY,
} from '../composables/usePurchaseInvoiceItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = usePurchaseInvoiceItemI18n()

const { t } = useI18n()
const { selectedMasterRow } = usePurchaseInvoiceMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseInvoiceItem')
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
const dataSource = ref<PurchaseInvoiceItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PurchaseInvoiceItem | null>(null)
const selectedRows = ref<PurchaseInvoiceItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PurchaseInvoiceItem>>({})
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
  for (const key of PURCHASEINVOICEITEM_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.purchaseOrderItem !== undefined && form.purchaseOrderItem !== null) {
    return true
  }
  if (form.amount !== undefined && form.amount !== null) {
    return true
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    return true
  }
  if (form.poPriceQuantity !== undefined && form.poPriceQuantity !== null) {
    return true
  }
  if (form.valuatedStockQuantity !== undefined && form.valuatedStockQuantity !== null) {
    return true
  }
  if (form.previousPeriodStock !== undefined && form.previousPeriodStock !== null) {
    return true
  }
  if (form.totalValuatedStockValue !== undefined && form.totalValuatedStockValue !== null) {
    return true
  }
  if (form.previousPeriodValue !== undefined && form.previousPeriodValue !== null) {
    return true
  }
  if (form.referenceDocumentItem !== undefined && form.referenceDocumentItem !== null) {
    return true
  }
  if (form.materialDocumentItem !== undefined && form.materialDocumentItem !== null) {
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
  const form = Object.fromEntries(PURCHASEINVOICEITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PURCHASEINVOICEITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    purchaseOrderItem: undefined as number | undefined,
    amount: undefined as number | undefined,
    quantity: undefined as number | undefined,
    poPriceQuantity: undefined as number | undefined,
    valuatedStockQuantity: undefined as number | undefined,
    previousPeriodStock: undefined as number | undefined,
    totalValuatedStockValue: undefined as number | undefined,
    previousPeriodValue: undefined as number | undefined,
    referenceDocumentItem: undefined as number | undefined,
    materialDocumentItem: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  PURCHASEINVOICEITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...PURCHASEINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...PURCHASEINVOICEITEM_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'purchaseInvoiceItemId'
const masterPurchaseInvoiceId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['purchaseInvoiceId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterPurchaseInvoiceId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPurchaseInvoiceItemId(record: PurchaseInvoiceItem | Record<string, unknown>): string {
  return String((record as PurchaseInvoiceItem)?.[entityIdName] ?? '')
}

function getPurchaseInvoiceItemField(record: PurchaseInvoiceItem | Record<string, unknown>, field: string): unknown {
  return (record as PurchaseInvoiceItem)?.[field as keyof PurchaseInvoiceItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'purchaseInvoiceItemId',
    key: 'purchaseInvoiceItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'purchaseInvoiceItemId') ?? ''),
  },
  {
    title: pi.label('purchaseInvoiceId'),
    dataIndex: 'purchaseInvoiceId',
    key: 'purchaseInvoiceId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'purchaseInvoiceId') ?? ''),
  },
  {
    title: pi.label('purchaseInvoiceCode'),
    dataIndex: 'purchaseInvoiceCode',
    key: 'purchaseInvoiceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'purchaseInvoiceCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('purchaseOrderCode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'purchaseOrderCode') ?? ''),
  },
  {
    title: pi.label('purchaseOrderItem'),
    dataIndex: 'purchaseOrderItem',
    key: 'purchaseOrderItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'purchaseOrderItem') ?? ''),
  },
  {
    title: pi.label('accountAssignmentSeq'),
    dataIndex: 'accountAssignmentSeq',
    key: 'accountAssignmentSeq',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'accountAssignmentSeq') ?? ''),
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'materialCode') ?? ''),
  },
  {
    title: pi.label('valuationArea'),
    dataIndex: 'valuationArea',
    key: 'valuationArea',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'valuationArea') ?? ''),
  },
  {
    title: pi.label('amount'),
    dataIndex: 'amount',
    key: 'amount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'amount') ?? ''),
  },
  {
    title: pi.label('debitCreditIndicator'),
    dataIndex: 'debitCreditIndicator',
    key: 'debitCreditIndicator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'debitCreditIndicator') ?? ''),
  },
  {
    title: pi.label('taxCode'),
    dataIndex: 'taxCode',
    key: 'taxCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'taxCode') ?? ''),
  },
  {
    title: pi.label('quantity'),
    dataIndex: 'quantity',
    key: 'quantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'quantity') ?? ''),
  },
  {
    title: pi.label('orderUnit'),
    dataIndex: 'orderUnit',
    key: 'orderUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'orderUnit') ?? ''),
  },
  {
    title: pi.label('poPriceQuantity'),
    dataIndex: 'poPriceQuantity',
    key: 'poPriceQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'poPriceQuantity') ?? ''),
  },
  {
    title: pi.label('poPriceUnit'),
    dataIndex: 'poPriceUnit',
    key: 'poPriceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'poPriceUnit') ?? ''),
  },
  {
    title: pi.label('valuatedStockQuantity'),
    dataIndex: 'valuatedStockQuantity',
    key: 'valuatedStockQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'valuatedStockQuantity') ?? ''),
  },
  {
    title: pi.label('previousPeriodStock'),
    dataIndex: 'previousPeriodStock',
    key: 'previousPeriodStock',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'previousPeriodStock') ?? ''),
  },
  {
    title: pi.label('baseUnit'),
    dataIndex: 'baseUnit',
    key: 'baseUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'baseUnit') ?? ''),
  },
  {
    title: pi.label('valuationClass'),
    dataIndex: 'valuationClass',
    key: 'valuationClass',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'valuationClass') ?? ''),
  },
  {
    title: pi.label('updatePoHistoryFlag'),
    dataIndex: 'updatePoHistoryFlag',
    key: 'updatePoHistoryFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'updatePoHistoryFlag') ?? ''),
  },
  {
    title: pi.label('subsequentDebitCredit'),
    dataIndex: 'subsequentDebitCredit',
    key: 'subsequentDebitCredit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'subsequentDebitCredit') ?? ''),
  },
  {
    title: pi.label('blockReasonPrice'),
    dataIndex: 'blockReasonPrice',
    key: 'blockReasonPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'blockReasonPrice') ?? ''),
  },
  {
    title: pi.label('blockReasonQuantity'),
    dataIndex: 'blockReasonQuantity',
    key: 'blockReasonQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'blockReasonQuantity') ?? ''),
  },
  {
    title: pi.label('blockReasonQuality'),
    dataIndex: 'blockReasonQuality',
    key: 'blockReasonQuality',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'blockReasonQuality') ?? ''),
  },
  {
    title: pi.label('blockReasonEnhanced'),
    dataIndex: 'blockReasonEnhanced',
    key: 'blockReasonEnhanced',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'blockReasonEnhanced') ?? ''),
  },
  {
    title: pi.label('valueString'),
    dataIndex: 'valueString',
    key: 'valueString',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'valueString') ?? ''),
  },
  {
    title: pi.label('referenceCode'),
    dataIndex: 'referenceCode',
    key: 'referenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'referenceCode') ?? ''),
  },
  {
    title: pi.label('conditionType'),
    dataIndex: 'conditionType',
    key: 'conditionType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'conditionType') ?? ''),
  },
  {
    title: pi.label('totalValuatedStockValue'),
    dataIndex: 'totalValuatedStockValue',
    key: 'totalValuatedStockValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'totalValuatedStockValue') ?? ''),
  },
  {
    title: pi.label('previousPeriodValue'),
    dataIndex: 'previousPeriodValue',
    key: 'previousPeriodValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'previousPeriodValue') ?? ''),
  },
  {
    title: pi.label('referenceDocumentCode'),
    dataIndex: 'referenceDocumentCode',
    key: 'referenceDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'referenceDocumentCode') ?? ''),
  },
  {
    title: pi.label('referenceDocumentYear'),
    dataIndex: 'referenceDocumentYear',
    key: 'referenceDocumentYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'referenceDocumentYear') ?? ''),
  },
  {
    title: pi.label('referenceDocumentItem'),
    dataIndex: 'referenceDocumentItem',
    key: 'referenceDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'referenceDocumentItem') ?? ''),
  },
  {
    title: pi.label('stockManagedMaterialCode'),
    dataIndex: 'stockManagedMaterialCode',
    key: 'stockManagedMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'stockManagedMaterialCode') ?? ''),
  },
  {
    title: pi.label('itemText'),
    dataIndex: 'itemText',
    key: 'itemText',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'itemText') ?? ''),
  },
  {
    title: pi.label('materialDocumentItem'),
    dataIndex: 'materialDocumentItem',
    key: 'materialDocumentItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'materialDocumentItem') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PurchaseInvoiceItem }) =>
      String(getPurchaseInvoiceItemField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:invoice:update',
        onClick: (record: PurchaseInvoiceItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:invoice:delete',
        onClick: (record: PurchaseInvoiceItem) => void handleDeleteOne(record),
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
    idColumnKey: 'purchaseInvoiceItemId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS)

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
    PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of PURCHASEINVOICEITEM_SUMMARY_SUM_FIELDS) {
      const num = Number(getPurchaseInvoiceItemField(row, field))
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
  onChange: (keys: (string | number)[], rows: PurchaseInvoiceItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PurchaseInvoiceItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPurchaseInvoiceItemId(selectedRow.value) === getPurchaseInvoiceItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PurchaseInvoiceItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PurchaseInvoiceItem) {
  const key = getPurchaseInvoiceItemId(record)
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
 * @returns {PurchaseInvoiceItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseInvoiceItemQuery>): PurchaseInvoiceItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseInvoiceItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    purchaseInvoiceId: masterPurchaseInvoiceId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseInvoiceItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PURCHASEINVOICEITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.purchaseOrderItem !== undefined && form.purchaseOrderItem !== null) {
    query.purchaseOrderItem = form.purchaseOrderItem
  }
  if (form.amount !== undefined && form.amount !== null) {
    query.amount = form.amount
  }
  if (form.quantity !== undefined && form.quantity !== null) {
    query.quantity = form.quantity
  }
  if (form.poPriceQuantity !== undefined && form.poPriceQuantity !== null) {
    query.poPriceQuantity = form.poPriceQuantity
  }
  if (form.valuatedStockQuantity !== undefined && form.valuatedStockQuantity !== null) {
    query.valuatedStockQuantity = form.valuatedStockQuantity
  }
  if (form.previousPeriodStock !== undefined && form.previousPeriodStock !== null) {
    query.previousPeriodStock = form.previousPeriodStock
  }
  if (form.totalValuatedStockValue !== undefined && form.totalValuatedStockValue !== null) {
    query.totalValuatedStockValue = form.totalValuatedStockValue
  }
  if (form.previousPeriodValue !== undefined && form.previousPeriodValue !== null) {
    query.previousPeriodValue = form.previousPeriodValue
  }
  if (form.referenceDocumentItem !== undefined && form.referenceDocumentItem !== null) {
    query.referenceDocumentItem = form.referenceDocumentItem
  }
  if (form.materialDocumentItem !== undefined && form.materialDocumentItem !== null) {
    query.materialDocumentItem = form.materialDocumentItem
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
    const res = await getPurchaseInvoiceItemList(buildListQuery())
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
watch(masterPurchaseInvoiceId, () => {
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

async function handleEdit(record: PurchaseInvoiceItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getPurchaseInvoiceItemById(getPurchaseInvoiceItemId(record))
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
    const id = formData.value?.purchaseInvoiceItemId
    if (id) {
      await updatePurchaseInvoiceItem(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createPurchaseInvoiceItem(payload)
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

async function handleDeleteOne(record: PurchaseInvoiceItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseInvoiceItemById(getPurchaseInvoiceItemId(record))
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
      const ids = selectedRows.value.map((r) => getPurchaseInvoiceItemId(r)).filter(Boolean)
      await deletePurchaseInvoiceItemBatch(ids)
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
  const res = await getPurchaseInvoiceItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importPurchaseInvoiceItem(file, sheetName)
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
    const exportMeta = await exportPurchaseInvoiceItem(
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

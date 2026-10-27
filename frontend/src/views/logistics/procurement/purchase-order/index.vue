<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-order -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt采购订单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getPurchaseOrderId"
      :master-row-selection="rowSelection"
      master-id-column-key="purchaseOrderId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
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
      create-permission="logistics:procurement:purchase:order:create"
      update-permission="logistics:procurement:purchase:order:update"
      delete-permission="logistics:procurement:purchase:order:delete"
      import-permission="logistics:procurement:purchase:order:import"
      export-permission="logistics:procurement:purchase:order:export"
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
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'orderStatus'">
          <a-switch
            :checked="getPurchaseOrderDictValue(record, 'orderStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleOrderStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'purchaseOrderType'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'purchaseOrderType')"
            dict-type="logistics_procurement_purchase_order_type"
          />
        </template>
        <template v-else-if="column.key === 'paymentTerms'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'paymentTerms')"
            dict-type="accounting_financial_payment_terms_param"
          />
        </template>
        <template v-else-if="column.key === 'pricingProcedure'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'pricingProcedure')"
            dict-type="logistics_procurement_pricing_procedure"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'taxCode'">
          {{ record.taxCode }}
        </template>
        <template v-else-if="column.key === 'taxRate'">
          {{ record.taxRate }}
        </template>
        <template v-else-if="column.key === 'paymentMethod'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'paymentMethod')"
            dict-type="accounting_financial_payment_method"
          />
        </template>
        <template v-else-if="column.key === 'deliveryMethod'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'deliveryMethod')"
            dict-type="logistics_sales_delivery_method"
          />
        </template>
        <template v-else-if="column.key === 'deliveryStatus'">
          <TaktDictTag
            :value="getPurchaseOrderDictValue(record, 'deliveryStatus')"
            dict-type="logistics_sales_delivery_status"
          />
        </template>
      </template>
      <template #detail>
        <PurchaseOrderItemPanel
          ref="purchaseOrderItemPanelRef"
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
      <PurchaseOrderForm
        :key="formData?.purchaseOrderId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-procurement-purchase-order'"
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
      <div v-show="isFieldVisible('purchaseOrderCode')">
      <a-form-item :label="pi.queryLabel('purchaseOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseOrderCode"
          :placeholder="pi.queryPh('purchaseOrderCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestId')">
      <a-form-item :label="pi.queryLabel('purchaseRequestId')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseRequestId"
          api-url="TaktPurchaseRequests/options"
          :placeholder="pi.queryPh('purchaseRequestId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseRequestCode')">
      <a-form-item :label="pi.queryLabel('purchaseRequestCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseRequestCode"
          :placeholder="pi.queryPh('purchaseRequestCode', 'required')"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('supplierName1')">
      <a-form-item :label="pi.queryLabel('supplierName1')">
        <a-input
          v-model:value="advancedQueryForm.supplierName1"
          :placeholder="pi.queryPh('supplierName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateStart')">
      <a-form-item :label="pi.queryLabel('orderDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateStart"
          :placeholder="pi.queryPh('orderDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderDateEnd')">
      <a-form-item :label="pi.queryLabel('orderDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.orderDateEnd"
          :placeholder="pi.queryPh('orderDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateStart')">
      <a-form-item :label="pi.queryLabel('requiredArrivalDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateStart"
          :placeholder="pi.queryPh('requiredArrivalDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredArrivalDateEnd')">
      <a-form-item :label="pi.queryLabel('requiredArrivalDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredArrivalDateEnd"
          :placeholder="pi.queryPh('requiredArrivalDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateStart')">
      <a-form-item :label="pi.queryLabel('actualArrivalDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateStart"
          :placeholder="pi.queryPh('actualArrivalDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualArrivalDateEnd')">
      <a-form-item :label="pi.queryLabel('actualArrivalDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualArrivalDateEnd"
          :placeholder="pi.queryPh('actualArrivalDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
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
      <div v-show="isFieldVisible('purchaseOrderType')">
      <a-form-item :label="pi.queryLabel('purchaseOrderType')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseOrderType"
          dict-type="logistics_procurement_purchase_order_type"
          :placeholder="pi.queryPh('purchaseOrderType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="pi.queryLabel('paymentTerms')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentTerms"
          dict-type="accounting_financial_payment_terms_param"
          :placeholder="pi.queryPh('paymentTerms', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingProcedure')">
      <a-form-item :label="pi.queryLabel('pricingProcedure')">
        <TaktSelect
          v-model:value="advancedQueryForm.pricingProcedure"
          dict-type="logistics_procurement_pricing_procedure"
          :placeholder="pi.queryPh('pricingProcedure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingConditionCode')">
      <a-form-item :label="pi.queryLabel('pricingConditionCode')">
        <a-input
          v-model:value="advancedQueryForm.pricingConditionCode"
          :placeholder="pi.queryPh('pricingConditionCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="pi.queryLabel('totalQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="pi.queryPh('totalQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="pi.queryLabel('totalAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="pi.queryPh('totalAmount', 'required')"
          style="width: 100%"
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
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('exchangeRate')">
      <a-form-item :label="pi.queryLabel('exchangeRate')">
        <a-input-number
          v-model:value="advancedQueryForm.exchangeRate"
          :placeholder="pi.queryPh('exchangeRate', 'required')"
          style="width: 100%"
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
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="pi.queryLabel('taxRate')">
        <a-input-number
          v-model:value="advancedQueryForm.taxRate"
          :placeholder="pi.queryPh('taxRate', 'required')"
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
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="pi.queryLabel('actualAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="pi.queryPh('actualAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedQuantity')">
      <a-form-item :label="pi.queryLabel('receivedQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedQuantity"
          :placeholder="pi.queryPh('receivedQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('receivedAmount')">
      <a-form-item :label="pi.queryLabel('receivedAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.receivedAmount"
          :placeholder="pi.queryPh('receivedAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paidAmount')">
      <a-form-item :label="pi.queryLabel('paidAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.paidAmount"
          :placeholder="pi.queryPh('paidAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentMethod')">
      <a-form-item :label="pi.queryLabel('paymentMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentMethod"
          dict-type="accounting_financial_payment_method"
          :placeholder="pi.queryPh('paymentMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryMethod')">
      <a-form-item :label="pi.queryLabel('deliveryMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.deliveryMethod"
          dict-type="logistics_sales_delivery_method"
          :placeholder="pi.queryPh('deliveryMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryAddress')">
      <a-form-item :label="pi.queryLabel('deliveryAddress')">
        <a-textarea
          v-model:value="advancedQueryForm.deliveryAddress"
          :placeholder="pi.queryPh('deliveryAddress', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderStatus')">
      <a-form-item :label="pi.queryLabel('orderStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('orderStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deliveryStatus')">
      <a-form-item :label="pi.queryLabel('deliveryStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.deliveryStatus"
          dict-type="logistics_sales_delivery_status"
          :placeholder="pi.queryPh('deliveryStatus', 'select')"
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
        :entity-i18n-key="PURCHASEORDER_SELF_I18N_KEY"
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
      :id-column-key="'purchaseOrderId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt采购订单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/purchase-order
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import PurchaseOrderForm from './components/PurchaseOrderForm.vue'
import PurchaseOrderItemPanel from './components/PurchaseOrderItemPanel.vue'
import { providePurchaseOrderMasterContext, type PurchaseOrderRowRecord } from './composables/usePurchaseOrderMasterContext'
import { getPurchaseOrderList, getPurchaseOrderById, createPurchaseOrder, updatePurchaseOrder, deletePurchaseOrderById, deletePurchaseOrderBatch, getPurchaseOrderTemplate, importPurchaseOrder, exportPurchaseOrder, updatePurchaseOrderStatus } from '@/api/logistics/procurement/purchaseOrder.api'
import type { PurchaseOrder, PurchaseOrderQuery } from '@/types/logistics/procurement/purchaseOrder.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  usePurchaseOrderI18n,
  PURCHASEORDER_LIST_FIELDS,
  PURCHASEORDER_QUERY_STRING_FIELDS,
  PURCHASEORDER_QUERY_FIELDS,
  PURCHASEORDER_SELF_I18N_KEY,
} from './composables/usePurchaseOrderI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = usePurchaseOrderI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPurchaseOrder')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PurchaseOrder[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<PurchaseOrderRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<PurchaseOrderRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PurchaseOrder> | null>(null)
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
  for (const key of PURCHASEORDER_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    return true
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    return true
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    return true
  }
  if (form.exchangeRate !== undefined && form.exchangeRate !== null) {
    return true
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    return true
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    return true
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    return true
  }
  if (form.receivedQuantity !== undefined && form.receivedQuantity !== null) {
    return true
  }
  if (form.receivedAmount !== undefined && form.receivedAmount !== null) {
    return true
  }
  if (form.paidAmount !== undefined && form.paidAmount !== null) {
    return true
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    return true
  }
  if (form.deliveryMethod !== undefined && form.deliveryMethod !== null) {
    return true
  }
  if (form.orderStatus !== undefined && form.orderStatus !== null) {
    return true
  }
  if (form.deliveryStatus !== undefined && form.deliveryStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(PURCHASEORDER_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PURCHASEORDER_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    totalQuantity: undefined as number | undefined,
    totalAmount: undefined as number | undefined,
    discountAmount: undefined as number | undefined,
    exchangeRate: undefined as number | undefined,
    taxRate: undefined as number | undefined,
    taxAmount: undefined as number | undefined,
    actualAmount: undefined as number | undefined,
    receivedQuantity: undefined as number | undefined,
    receivedAmount: undefined as number | undefined,
    paidAmount: undefined as number | undefined,
    paymentMethod: undefined as number | undefined,
    deliveryMethod: undefined as number | undefined,
    orderStatus: undefined as number | undefined,
    deliveryStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  PURCHASEORDER_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'purchaseOrderId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = providePurchaseOrderMasterContext()
const purchaseOrderItemPanelRef = ref<InstanceType<typeof PurchaseOrderItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PurchaseOrderQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PurchaseOrderQuery>): PurchaseOrderQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PurchaseOrderQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PurchaseOrderQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PURCHASEORDER_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    query.totalQuantity = form.totalQuantity
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    query.totalAmount = form.totalAmount
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.exchangeRate !== undefined && form.exchangeRate !== null) {
    query.exchangeRate = form.exchangeRate
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    query.actualAmount = form.actualAmount
  }
  if (form.receivedQuantity !== undefined && form.receivedQuantity !== null) {
    query.receivedQuantity = form.receivedQuantity
  }
  if (form.receivedAmount !== undefined && form.receivedAmount !== null) {
    query.receivedAmount = form.receivedAmount
  }
  if (form.paidAmount !== undefined && form.paidAmount !== null) {
    query.paidAmount = form.paidAmount
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    query.paymentMethod = form.paymentMethod
  }
  if (form.deliveryMethod !== undefined && form.deliveryMethod !== null) {
    query.deliveryMethod = form.deliveryMethod
  }
  if (form.orderStatus !== undefined && form.orderStatus !== null) {
    query.orderStatus = form.orderStatus
  }
  if (form.deliveryStatus !== undefined && form.deliveryStatus !== null) {
    query.deliveryStatus = form.deliveryStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: PurchaseOrderRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getPurchaseOrderId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as PurchaseOrderRowRecord
  const key = getPurchaseOrderId(row)
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
async function loadPurchaseOrderDetail(record: PurchaseOrderRowRecord): Promise<PurchaseOrder | null> {
  const id = getPurchaseOrderId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getPurchaseOrderById(id)
    const index = dataSource.value.findIndex((row) => getPurchaseOrderId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as PurchaseOrder
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
    dataIndex: 'purchaseOrderId',
    key: 'purchaseOrderId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseOrderId') ?? ''
  },
  {
    title: pi.label('purchaseOrderCode'),
    dataIndex: 'purchaseOrderCode',
    key: 'purchaseOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseOrderCode') ?? ''
  },
  {
    title: pi.label('purchaseRequestId'),
    dataIndex: 'purchaseRequestId',
    key: 'purchaseRequestId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseRequestId') ?? ''
  },
  {
    title: pi.label('purchaseRequestCode'),
    dataIndex: 'purchaseRequestCode',
    key: 'purchaseRequestCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseRequestCode') ?? ''
  },
  {
    title: pi.label('supplierCode'),
    dataIndex: 'supplierCode',
    key: 'supplierCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierCode') ?? ''
  },
  {
    title: pi.label('supplierName1'),
    dataIndex: 'supplierName1',
    key: 'supplierName1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'supplierName1') ?? ''
  },
  {
    title: pi.label('orderDate'),
    dataIndex: 'orderDate',
    key: 'orderDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'orderDate') ?? ''
  },
  {
    title: pi.label('requiredArrivalDate'),
    dataIndex: 'requiredArrivalDate',
    key: 'requiredArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'requiredArrivalDate') ?? ''
  },
  {
    title: pi.label('actualArrivalDate'),
    dataIndex: 'actualArrivalDate',
    key: 'actualArrivalDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualArrivalDate') ?? ''
  },
  {
    title: pi.label('purchaseGroup'),
    dataIndex: 'purchaseGroup',
    key: 'purchaseGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'purchaseGroup') ?? ''
  },
  {
    title: pi.label('purchaseOrderType'),
    dataIndex: 'purchaseOrderType',
    key: 'purchaseOrderType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('paymentTerms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('pricingProcedure'),
    dataIndex: 'pricingProcedure',
    key: 'pricingProcedure',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('pricingConditionCode'),
    dataIndex: 'pricingConditionCode',
    key: 'pricingConditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'pricingConditionCode') ?? ''
  },
  {
    title: pi.label('totalQuantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalQuantity') ?? ''
  },
  {
    title: pi.label('totalAmount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'totalAmount') ?? ''
  },
  {
    title: pi.label('discountAmount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'discountAmount') ?? ''
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('exchangeRate'),
    dataIndex: 'exchangeRate',
    key: 'exchangeRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'exchangeRate') ?? ''
  },
  {
    title: pi.label('taxCode'),
    dataIndex: 'taxCode',
    key: 'taxCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('taxRate'),
    dataIndex: 'taxRate',
    key: 'taxRate',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'taxAmount') ?? ''
  },
  {
    title: pi.label('actualAmount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'actualAmount') ?? ''
  },
  {
    title: pi.label('receivedQuantity'),
    dataIndex: 'receivedQuantity',
    key: 'receivedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedQuantity') ?? ''
  },
  {
    title: pi.label('receivedAmount'),
    dataIndex: 'receivedAmount',
    key: 'receivedAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'receivedAmount') ?? ''
  },
  {
    title: pi.label('paidAmount'),
    dataIndex: 'paidAmount',
    key: 'paidAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'paidAmount') ?? ''
  },
  {
    title: pi.label('paymentMethod'),
    dataIndex: 'paymentMethod',
    key: 'paymentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('deliveryMethod'),
    dataIndex: 'deliveryMethod',
    key: 'deliveryMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('deliveryAddress'),
    dataIndex: 'deliveryAddress',
    key: 'deliveryAddress',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPurchaseOrderField(record, 'deliveryAddress') ?? ''
  },
  {
    title: pi.label('orderStatus'),
    dataIndex: 'orderStatus',
    key: 'orderStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('deliveryStatus'),
    dataIndex: 'deliveryStatus',
    key: 'deliveryStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:purchase:order:update',
        onClick: (record: PurchaseOrderRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:purchase:order:delete',
        onClick: (record: PurchaseOrderRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPurchaseOrderId = (record: PurchaseOrderRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseOrderField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getPurchaseOrderDictValue = (
  record: PurchaseOrderRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toPurchaseOrderNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PurchaseOrderRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: PurchaseOrderRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getPurchaseOrderId(selecmedRow.value) === getPurchaseOrderId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: PurchaseOrderRowRecord[]) => {
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
    const res = await getPurchaseOrderList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PurchaseOrder] 加载数据失败', { error })
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
  cultureCode: '',
  plantCode: '',
  purchaseOrderCode: '',
  purchaseRequestId: '',
  purchaseRequestCode: '',
  supplierCode: '',
  supplierName1: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  actualArrivalDateStart: '',
  actualArrivalDateEnd: '',
  purchaseGroup: '',
  purchaseOrderType: '',
  paymentTerms: '',
  pricingProcedure: '',
  pricingConditionCode: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  currencyCode: '',
  exchangeRate: undefined as number | undefined,
  taxCode: '',
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: PurchaseOrderRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadPurchaseOrderDetail(record)
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
      await updatePurchaseOrder(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createPurchaseOrder(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
  purchaseOrderItemPanelRef.value?.reload?.()
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
  const res = await getPurchaseOrderTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importPurchaseOrder(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selecmedMasterKey.value) {
    purchaseOrderItemPanelRef.value?.reload?.()
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
    const exportMeta = await exportPurchaseOrder(
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
    logger.error('[PurchaseOrder] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PurchaseOrderRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePurchaseOrderById((record as any)[entityIdName])
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
      await deletePurchaseOrderBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleOrderStatusChange(record: PurchaseOrderRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toPurchaseOrderNumber(getPurchaseOrderDictValue(record, 'orderStatus'))
  const id = getPurchaseOrderId(record)
  const row = dataSource.value.find((item) => getPurchaseOrderId(item) === id)
  if (row) {
    row.orderStatus = newVal
  }
  try {
    await updatePurchaseOrderStatus({ purchaseOrderId: id, orderStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.orderStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  cultureCode: '',
  plantCode: '',
  purchaseOrderCode: '',
  purchaseRequestId: '',
  purchaseRequestCode: '',
  supplierCode: '',
  supplierName1: '',
  orderDateStart: '',
  orderDateEnd: '',
  requiredArrivalDateStart: '',
  requiredArrivalDateEnd: '',
  actualArrivalDateStart: '',
  actualArrivalDateEnd: '',
  purchaseGroup: '',
  purchaseOrderType: '',
  paymentTerms: '',
  pricingProcedure: '',
  pricingConditionCode: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  currencyCode: '',
  exchangeRate: undefined as number | undefined,
  taxCode: '',
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  receivedQuantity: undefined as number | undefined,
  receivedAmount: undefined as number | undefined,
  paidAmount: undefined as number | undefined,
  paymentMethod: undefined as number | undefined,
  deliveryMethod: undefined as number | undefined,
  deliveryAddress: '',
  orderStatus: undefined as number | undefined,
  deliveryStatus: undefined as number | undefined,
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
</script>

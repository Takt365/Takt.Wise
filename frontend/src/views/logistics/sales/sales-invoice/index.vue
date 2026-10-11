<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售发票主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getSalesInvoiceId"
      :master-row-selection="rowSelection"
      master-id-column-key="salesInvoiceId"
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
      create-permission="logistics:sales:invoice:create"
      update-permission="logistics:sales:invoice:update"
      delete-permission="logistics:sales:invoice:delete"
      import-permission="logistics:sales:invoice:import"
      export-permission="logistics:sales:invoice:export"
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
        <template v-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getSalesInvoiceDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'shippingConditions'">
          <TaktDictTag
            :value="getSalesInvoiceDictValue(record, 'shippingConditions')"
            dict-type="logistics_sales_shipping_conditions"
          />
        </template>
        <template v-else-if="column.key === 'countryCode'">
          <TaktDictTag
            :value="getSalesInvoiceDictValue(record, 'countryCode')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'statisticsCurrencyCode'">
          <TaktDictTag
            :value="getSalesInvoiceDictValue(record, 'statisticsCurrencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'taxDepartureCountry'">
          <TaktDictTag
            :value="getSalesInvoiceDictValue(record, 'taxDepartureCountry')"
            dict-type="sys_country_code"
          />
        </template>
      </template>
      <template #detail>
        <SalesInvoiceItemPanel
          ref="salesInvoiceItemPanelRef"
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
      <SalesInvoiceForm
        :key="formData?.salesInvoiceId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-sales-sales-invoice'"
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
      <div v-show="isFieldVisible('billingDocumentCode')">
      <a-form-item :label="pi.queryLabel('billingDocumentCode')">
        <a-input
          v-model:value="advancedQueryForm.billingDocumentCode"
          :placeholder="pi.queryPh('billingDocumentCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('billingType')">
      <a-form-item :label="pi.queryLabel('billingType')">
        <a-input
          v-model:value="advancedQueryForm.billingType"
          :placeholder="pi.queryPh('billingType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('billingCategory')">
      <a-form-item :label="pi.queryLabel('billingCategory')">
        <a-input
          v-model:value="advancedQueryForm.billingCategory"
          :placeholder="pi.queryPh('billingCategory', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentCategory')">
      <a-form-item :label="pi.queryLabel('documentCategory')">
        <a-input
          v-model:value="advancedQueryForm.documentCategory"
          :placeholder="pi.queryPh('documentCategory', 'required')"
          show-count
          :maxlength="1"
          allow-clear
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
      <div v-show="isFieldVisible('salesOrganization')">
      <a-form-item :label="pi.queryLabel('salesOrganization')">
        <a-input
          v-model:value="advancedQueryForm.salesOrganization"
          :placeholder="pi.queryPh('salesOrganization', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('distributionChannel')">
      <a-form-item :label="pi.queryLabel('distributionChannel')">
        <a-input
          v-model:value="advancedQueryForm.distributionChannel"
          :placeholder="pi.queryPh('distributionChannel', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingProcedure')">
      <a-form-item :label="pi.queryLabel('pricingProcedure')">
        <a-input
          v-model:value="advancedQueryForm.pricingProcedure"
          :placeholder="pi.queryPh('pricingProcedure', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('conditionCode')">
      <a-form-item :label="pi.queryLabel('conditionCode')">
        <a-input
          v-model:value="advancedQueryForm.conditionCode"
          :placeholder="pi.queryPh('conditionCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shippingConditions')">
      <a-form-item :label="pi.queryLabel('shippingConditions')">
        <TaktSelect
          v-model:value="advancedQueryForm.shippingConditions"
          dict-type="logistics_sales_shipping_conditions"
          :placeholder="pi.queryPh('shippingConditions', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('billingDateStart')">
      <a-form-item :label="pi.queryLabel('billingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.billingDateStart"
          :placeholder="pi.queryPh('billingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('billingDateEnd')">
      <a-form-item :label="pi.queryLabel('billingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.billingDateEnd"
          :placeholder="pi.queryPh('billingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerGroup')">
      <a-form-item :label="pi.queryLabel('customerGroup')">
        <a-input
          v-model:value="advancedQueryForm.customerGroup"
          :placeholder="pi.queryPh('customerGroup', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incoterms1')">
      <a-form-item :label="pi.queryLabel('incoterms1')">
        <a-input
          v-model:value="advancedQueryForm.incoterms1"
          :placeholder="pi.queryPh('incoterms1', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incoterms2')">
      <a-form-item :label="pi.queryLabel('incoterms2')">
        <a-input
          v-model:value="advancedQueryForm.incoterms2"
          :placeholder="pi.queryPh('incoterms2', 'required')"
          show-count
          :maxlength="28"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingStatus')">
      <a-form-item :label="pi.queryLabel('postingStatus')">
        <a-input
          v-model:value="advancedQueryForm.postingStatus"
          :placeholder="pi.queryPh('postingStatus', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountingExchangeRate')">
      <a-form-item :label="pi.queryLabel('accountingExchangeRate')">
        <a-input-number
          v-model:value="advancedQueryForm.accountingExchangeRate"
          :placeholder="pi.queryPh('accountingExchangeRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentTerms')">
      <a-form-item :label="pi.queryLabel('paymentTerms')">
        <a-input
          v-model:value="advancedQueryForm.paymentTerms"
          :placeholder="pi.queryPh('paymentTerms', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountAssignmentGroup')">
      <a-form-item :label="pi.queryLabel('accountAssignmentGroup')">
        <a-input
          v-model:value="advancedQueryForm.accountAssignmentGroup"
          :placeholder="pi.queryPh('accountAssignmentGroup', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countryCode')">
      <a-form-item :label="pi.queryLabel('countryCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.countryCode"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('countryCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netAmount')">
      <a-form-item :label="pi.queryLabel('netAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.netAmount"
          :placeholder="pi.queryPh('netAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('payerCode')">
      <a-form-item :label="pi.queryLabel('payerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.payerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('payerCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="pi.queryLabel('customerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('statisticsCurrencyCode')">
      <a-form-item :label="pi.queryLabel('statisticsCurrencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.statisticsCurrencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('statisticsCurrencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('foreignTradeCode')">
      <a-form-item :label="pi.queryLabel('foreignTradeCode')">
        <a-input
          v-model:value="advancedQueryForm.foreignTradeCode"
          :placeholder="pi.queryPh('foreignTradeCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cancelledBillingDocument')">
      <a-form-item :label="pi.queryLabel('cancelledBillingDocument')">
        <a-input
          v-model:value="advancedQueryForm.cancelledBillingDocument"
          :placeholder="pi.queryPh('cancelledBillingDocument', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('invoiceListType')">
      <a-form-item :label="pi.queryLabel('invoiceListType')">
        <a-input
          v-model:value="advancedQueryForm.invoiceListType"
          :placeholder="pi.queryPh('invoiceListType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('division')">
      <a-form-item :label="pi.queryLabel('division')">
        <a-input
          v-model:value="advancedQueryForm.division"
          :placeholder="pi.queryPh('division', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hierarchyTypePricing')">
      <a-form-item :label="pi.queryLabel('hierarchyTypePricing')">
        <a-input
          v-model:value="advancedQueryForm.hierarchyTypePricing"
          :placeholder="pi.queryPh('hierarchyTypePricing', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tradingPartner')">
      <a-form-item :label="pi.queryLabel('tradingPartner')">
        <a-input
          v-model:value="advancedQueryForm.tradingPartner"
          :placeholder="pi.queryPh('tradingPartner', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxDepartureCountry')">
      <a-form-item :label="pi.queryLabel('taxDepartureCountry')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxDepartureCountry"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('taxDepartureCountry', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('organizationSalesTaxNumber')">
      <a-form-item :label="pi.queryLabel('organizationSalesTaxNumber')">
        <a-input
          v-model:value="advancedQueryForm.organizationSalesTaxNumber"
          :placeholder="pi.queryPh('organizationSalesTaxNumber', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countrySalesTaxNumber')">
      <a-form-item :label="pi.queryLabel('countrySalesTaxNumber')">
        <a-input
          v-model:value="advancedQueryForm.countrySalesTaxNumber"
          :placeholder="pi.queryPh('countrySalesTaxNumber', 'required')"
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
      <div v-show="isFieldVisible('cancelledFlag')">
      <a-form-item :label="pi.queryLabel('cancelledFlag')">
        <a-input
          v-model:value="advancedQueryForm.cancelledFlag"
          :placeholder="pi.queryPh('cancelledFlag', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('exchangeRateDateStart')">
      <a-form-item :label="pi.queryLabel('exchangeRateDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.exchangeRateDateStart"
          :placeholder="pi.queryPh('exchangeRateDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('exchangeRateDateEnd')">
      <a-form-item :label="pi.queryLabel('exchangeRateDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.exchangeRateDateEnd"
          :placeholder="pi.queryPh('exchangeRateDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('paymentReference')">
      <a-form-item :label="pi.queryLabel('paymentReference')">
        <a-input
          v-model:value="advancedQueryForm.paymentReference"
          :placeholder="pi.queryPh('paymentReference', 'required')"
          show-count
          :maxlength="30"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reversalReason')">
      <a-form-item :label="pi.queryLabel('reversalReason')">
        <a-input
          v-model:value="advancedQueryForm.reversalReason"
          :placeholder="pi.queryPh('reversalReason', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postedBy')">
      <a-form-item :label="pi.queryLabel('postedBy')">
        <a-input
          v-model:value="advancedQueryForm.postedBy"
          :placeholder="pi.queryPh('postedBy', 'required')"
          show-count
          :maxlength="6"
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
        :entity-i18n-key="SALESINVOICE_SELF_I18N_KEY"
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
      :id-column-key="'salesInvoiceId'"
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
 * Takt销售发票主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-invoice
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import SalesInvoiceForm from './components/InvoiceForm.vue'
import SalesInvoiceItemPanel from './components/InvoiceItemPanel.vue'
import { provideSalesInvoiceMasterContext, type SalesInvoiceRowRecord } from './composables/useInvoiceMasterContext'
import { getSalesInvoiceList, getSalesInvoiceById, createSalesInvoice, updateSalesInvoice, deleteSalesInvoiceById, deleteSalesInvoiceBatch, getSalesInvoiceTemplate, importSalesInvoice, exportSalesInvoice, updateSalesInvoiceStatus } from '@/api/logistics/sales/invoice.api'
import type { SalesInvoice, SalesInvoiceQuery } from '@/types/logistics/sales/invoice.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useSalesInvoiceI18n,
  SALESINVOICE_LIST_FIELDS,
  SALESINVOICE_QUERY_STRING_FIELDS,
  SALESINVOICE_QUERY_FIELDS,
  SALESINVOICE_SELF_I18N_KEY,
} from './composables/useInvoiceI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesInvoiceI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesInvoice')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesInvoice[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesInvoiceRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesInvoiceRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesInvoice> | null>(null)
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
  for (const key of SALESINVOICE_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.accountingExchangeRate !== undefined && form.accountingExchangeRate !== null) {
    return true
  }
  if (form.netAmount !== undefined && form.netAmount !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESINVOICE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESINVOICE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    accountingExchangeRate: undefined as number | undefined,
    netAmount: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  SALESINVOICE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'salesInvoiceId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSalesInvoiceMasterContext()
const salesInvoiceItemPanelRef = ref<InstanceType<typeof SalesInvoiceItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SalesInvoiceQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesInvoiceQuery>): SalesInvoiceQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesInvoiceQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesInvoiceQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESINVOICE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.accountingExchangeRate !== undefined && form.accountingExchangeRate !== null) {
    query.accountingExchangeRate = form.accountingExchangeRate
  }
  if (form.netAmount !== undefined && form.netAmount !== null) {
    query.netAmount = form.netAmount
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
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: SalesInvoiceRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSalesInvoiceId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SalesInvoiceRowRecord
  const key = getSalesInvoiceId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
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
async function loadSalesInvoiceDetail(record: SalesInvoiceRowRecord): Promise<SalesInvoice | null> {
  const id = getSalesInvoiceId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesInvoiceById(id)
    const index = dataSource.value.findIndex((row) => getSalesInvoiceId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesInvoice
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
    dataIndex: 'salesInvoiceId',
    key: 'salesInvoiceId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'salesInvoiceId') ?? ''
  },
  {
    title: pi.label('billingDocumentCode'),
    dataIndex: 'billingDocumentCode',
    key: 'billingDocumentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'billingDocumentCode') ?? ''
  },
  {
    title: pi.label('billingType'),
    dataIndex: 'billingType',
    key: 'billingType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'billingType') ?? ''
  },
  {
    title: pi.label('billingCategory'),
    dataIndex: 'billingCategory',
    key: 'billingCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'billingCategory') ?? ''
  },
  {
    title: pi.label('documentCategory'),
    dataIndex: 'documentCategory',
    key: 'documentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'documentCategory') ?? ''
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
    title: pi.label('salesOrganization'),
    dataIndex: 'salesOrganization',
    key: 'salesOrganization',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'salesOrganization') ?? ''
  },
  {
    title: pi.label('distributionChannel'),
    dataIndex: 'distributionChannel',
    key: 'distributionChannel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'distributionChannel') ?? ''
  },
  {
    title: pi.label('pricingProcedure'),
    dataIndex: 'pricingProcedure',
    key: 'pricingProcedure',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'pricingProcedure') ?? ''
  },
  {
    title: pi.label('conditionCode'),
    dataIndex: 'conditionCode',
    key: 'conditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'conditionCode') ?? ''
  },
  {
    title: pi.label('shippingConditions'),
    dataIndex: 'shippingConditions',
    key: 'shippingConditions',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('billingDate'),
    dataIndex: 'billingDate',
    key: 'billingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'billingDate') ?? ''
  },
  {
    title: pi.label('customerGroup'),
    dataIndex: 'customerGroup',
    key: 'customerGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'customerGroup') ?? ''
  },
  {
    title: pi.label('incoterms1'),
    dataIndex: 'incoterms1',
    key: 'incoterms1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'incoterms1') ?? ''
  },
  {
    title: pi.label('incoterms2'),
    dataIndex: 'incoterms2',
    key: 'incoterms2',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'incoterms2') ?? ''
  },
  {
    title: pi.label('postingStatus'),
    dataIndex: 'postingStatus',
    key: 'postingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'postingStatus') ?? ''
  },
  {
    title: pi.label('accountingExchangeRate'),
    dataIndex: 'accountingExchangeRate',
    key: 'accountingExchangeRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'accountingExchangeRate') ?? ''
  },
  {
    title: pi.label('paymentTerms'),
    dataIndex: 'paymentTerms',
    key: 'paymentTerms',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'paymentTerms') ?? ''
  },
  {
    title: pi.label('accountAssignmentGroup'),
    dataIndex: 'accountAssignmentGroup',
    key: 'accountAssignmentGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'accountAssignmentGroup') ?? ''
  },
  {
    title: pi.label('countryCode'),
    dataIndex: 'countryCode',
    key: 'countryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('netAmount'),
    dataIndex: 'netAmount',
    key: 'netAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'netAmount') ?? ''
  },
  {
    title: pi.label('payerCode'),
    dataIndex: 'payerCode',
    key: 'payerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'payerCode') ?? ''
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'customerCode') ?? ''
  },
  {
    title: pi.label('statisticsCurrencyCode'),
    dataIndex: 'statisticsCurrencyCode',
    key: 'statisticsCurrencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('foreignTradeCode'),
    dataIndex: 'foreignTradeCode',
    key: 'foreignTradeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'foreignTradeCode') ?? ''
  },
  {
    title: pi.label('cancelledBillingDocument'),
    dataIndex: 'cancelledBillingDocument',
    key: 'cancelledBillingDocument',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'cancelledBillingDocument') ?? ''
  },
  {
    title: pi.label('invoiceListType'),
    dataIndex: 'invoiceListType',
    key: 'invoiceListType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'invoiceListType') ?? ''
  },
  {
    title: pi.label('division'),
    dataIndex: 'division',
    key: 'division',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'division') ?? ''
  },
  {
    title: pi.label('hierarchyTypePricing'),
    dataIndex: 'hierarchyTypePricing',
    key: 'hierarchyTypePricing',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'hierarchyTypePricing') ?? ''
  },
  {
    title: pi.label('tradingPartner'),
    dataIndex: 'tradingPartner',
    key: 'tradingPartner',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'tradingPartner') ?? ''
  },
  {
    title: pi.label('taxDepartureCountry'),
    dataIndex: 'taxDepartureCountry',
    key: 'taxDepartureCountry',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('organizationSalesTaxNumber'),
    dataIndex: 'organizationSalesTaxNumber',
    key: 'organizationSalesTaxNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'organizationSalesTaxNumber') ?? ''
  },
  {
    title: pi.label('countrySalesTaxNumber'),
    dataIndex: 'countrySalesTaxNumber',
    key: 'countrySalesTaxNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'countrySalesTaxNumber') ?? ''
  },
  {
    title: pi.label('referenceCode'),
    dataIndex: 'referenceCode',
    key: 'referenceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'referenceCode') ?? ''
  },
  {
    title: pi.label('cancelledFlag'),
    dataIndex: 'cancelledFlag',
    key: 'cancelledFlag',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'cancelledFlag') ?? ''
  },
  {
    title: pi.label('exchangeRateDate'),
    dataIndex: 'exchangeRateDate',
    key: 'exchangeRateDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'exchangeRateDate') ?? ''
  },
  {
    title: pi.label('paymentReference'),
    dataIndex: 'paymentReference',
    key: 'paymentReference',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'paymentReference') ?? ''
  },
  {
    title: pi.label('reversalReason'),
    dataIndex: 'reversalReason',
    key: 'reversalReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'reversalReason') ?? ''
  },
  {
    title: pi.label('postedBy'),
    dataIndex: 'postedBy',
    key: 'postedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesInvoiceField(record, 'postedBy') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:invoice:update',
        onClick: (record: SalesInvoiceRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:invoice:delete',
        onClick: (record: SalesInvoiceRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesInvoiceId = (record: SalesInvoiceRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesInvoiceField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesInvoiceDictValue = (
  record: SalesInvoiceRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesInvoiceRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SalesInvoiceRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSalesInvoiceId(selectedRow.value) === getSalesInvoiceId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesInvoiceRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
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
    const res = await getSalesInvoiceList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesInvoice] 加载数据失败', { error })
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
  billingDocumentCode: '',
  billingType: '',
  billingCategory: '',
  documentCategory: '',
  currencyCode: '',
  salesOrganization: '',
  distributionChannel: '',
  pricingProcedure: '',
  conditionCode: '',
  shippingConditions: '',
  billingDateStart: '',
  billingDateEnd: '',
  customerGroup: '',
  incoterms1: '',
  incoterms2: '',
  postingStatus: '',
  accountingExchangeRate: undefined as number | undefined,
  paymentTerms: '',
  accountAssignmentGroup: '',
  countryCode: '',
  netAmount: undefined as number | undefined,
  payerCode: '',
  customerCode: '',
  statisticsCurrencyCode: '',
  foreignTradeCode: '',
  cancelledBillingDocument: '',
  invoiceListType: '',
  division: '',
  hierarchyTypePricing: '',
  tradingPartner: '',
  taxDepartureCountry: '',
  organizationSalesTaxNumber: '',
  countrySalesTaxNumber: '',
  referenceCode: '',
  cancelledFlag: '',
  exchangeRateDateStart: '',
  exchangeRateDateEnd: '',
  paymentReference: '',
  reversalReason: '',
  postedBy: '',
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
async function handleEdit(record: SalesInvoiceRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadSalesInvoiceDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
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
      await updateSalesInvoice(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesInvoice(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  salesInvoiceItemPanelRef.value?.reload?.()
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
  const res = await getSalesInvoiceTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesInvoice(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    salesInvoiceItemPanelRef.value?.reload?.()
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
    const exportMeta = await exportSalesInvoice(
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
    logger.error('[SalesInvoice] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesInvoiceRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesInvoiceById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalesInvoiceBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
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
  cultureCode: '',
  plantCode: '',
  billingDocumentCode: '',
  billingType: '',
  billingCategory: '',
  documentCategory: '',
  currencyCode: '',
  salesOrganization: '',
  distributionChannel: '',
  pricingProcedure: '',
  conditionCode: '',
  shippingConditions: '',
  billingDateStart: '',
  billingDateEnd: '',
  customerGroup: '',
  incoterms1: '',
  incoterms2: '',
  postingStatus: '',
  accountingExchangeRate: undefined as number | undefined,
  paymentTerms: '',
  accountAssignmentGroup: '',
  countryCode: '',
  netAmount: undefined as number | undefined,
  payerCode: '',
  customerCode: '',
  statisticsCurrencyCode: '',
  foreignTradeCode: '',
  cancelledBillingDocument: '',
  invoiceListType: '',
  division: '',
  hierarchyTypePricing: '',
  tradingPartner: '',
  taxDepartureCountry: '',
  organizationSalesTaxNumber: '',
  countrySalesTaxNumber: '',
  referenceCode: '',
  cancelledFlag: '',
  exchangeRateDateStart: '',
  exchangeRateDateEnd: '',
  paymentReference: '',
  reversalReason: '',
  postedBy: '',
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

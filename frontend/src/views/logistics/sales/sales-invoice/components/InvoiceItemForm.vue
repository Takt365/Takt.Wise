<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：InvoiceItemForm.vue -->
<!-- 功能描述：Takt销售发票主表实体子表 salesInvoiceItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form invoice-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="invoice-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingQuantity')"
                name="billingQuantity"
              >
                <a-input-number
                  v-model:value="formState.billingQuantity"
                  :placeholder="pi.ph('billingQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesUnit')"
                name="salesUnit"
              >
                <a-input
                  v-model:value="formState.salesUnit"
                  :placeholder="pi.ph('salesUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="pi.ph('baseUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantity')"
                name="scaleQuantity"
              >
                <a-input-number
                  v-model:value="formState.scaleQuantity"
                  :placeholder="pi.ph('scaleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingQuantitySku')"
                name="billingQuantitySku"
              >
                <a-input-number
                  v-model:value="formState.billingQuantitySku"
                  :placeholder="pi.ph('billingQuantitySku')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <a-input
                  v-model:value="formState.weightUnit"
                  :placeholder="pi.ph('weightUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessAreaCode')"
                name="businessAreaCode"
              >
                <a-input
                  v-model:value="formState.businessAreaCode"
                  :placeholder="pi.ph('businessAreaCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingDate')"
                name="pricingDate"
              >
                <a-date-picker
                  v-model:value="formState.pricingDate"
                  :placeholder="pi.ph('pricingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceRenderedDate')"
                name="serviceRenderedDate"
              >
                <a-date-picker
                  v-model:value="formState.serviceRenderedDate"
                  :placeholder="pi.ph('serviceRenderedDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingExchangeRate')"
                name="pricingExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.pricingExchangeRate"
                  :placeholder="pi.ph('pricingExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netAmount')"
                name="netAmount"
              >
                <a-input-number
                  v-model:value="formState.netAmount"
                  :placeholder="pi.ph('netAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCode')"
                name="referenceDocumentCode"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCode"
                  :placeholder="pi.ph('referenceDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentItem')"
                name="referenceDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.referenceDocumentItem"
                  :placeholder="pi.ph('referenceDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCategory')"
                name="referenceDocumentCategory"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCategory"
                  :placeholder="pi.ph('referenceDocumentCategory')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentCode')"
                name="salesDocumentCode"
              >
                <a-input
                  v-model:value="formState.salesDocumentCode"
                  :placeholder="pi.ph('salesDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentItem')"
                name="salesDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.salesDocumentItem"
                  :placeholder="pi.ph('salesDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDocumentReferenceFlag')"
                name="salesDocumentReferenceFlag"
              >
                <a-input
                  v-model:value="formState.salesDocumentReferenceFlag"
                  :placeholder="pi.ph('salesDocumentReferenceFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingReferenceMaterialCode')"
                name="pricingReferenceMaterialCode"
              >
                <a-input
                  v-model:value="formState.pricingReferenceMaterialCode"
                  :placeholder="pi.ph('pricingReferenceMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="pi.ph('batchCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialGroup')"
                name="materialGroup"
              >
                <a-input
                  v-model:value="formState.materialGroup"
                  :placeholder="pi.ph('materialGroup')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesItemCategory')"
                name="salesItemCategory"
              >
                <a-input
                  v-model:value="formState.salesItemCategory"
                  :placeholder="pi.ph('salesItemCategory')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productHierarchy')"
                name="productHierarchy"
              >
                <a-input
                  v-model:value="formState.productHierarchy"
                  :placeholder="pi.ph('productHierarchy')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippingPoint')"
                name="shippingPoint"
              >
                <a-input
                  v-model:value="formState.shippingPoint"
                  :placeholder="pi.ph('shippingPoint')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('division')"
                name="division"
              >
                <a-input
                  v-model:value="formState.division"
                  :placeholder="pi.ph('division')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('partnerItem')"
                name="partnerItem"
              >
                <a-input-number
                  v-model:value="formState.partnerItem"
                  :placeholder="pi.ph('partnerItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('departureCountry')"
                name="departureCountry"
              >
                <TaktSelect
                  v-model:value="formState.departureCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('departureCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantRegion')"
                name="plantRegion"
              >
                <a-input
                  v-model:value="formState.plantRegion"
                  :placeholder="pi.ph('plantRegion')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingFlag')"
                name="pricingFlag"
              >
                <a-input
                  v-model:value="formState.pricingFlag"
                  :placeholder="pi.ph('pricingFlag')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('warehouseCode')"
                name="warehouseCode"
              >
                <TaktSelect
                  v-model:value="formState.warehouseCode"
                  api-url="TaktWarehouses/options"
                  :placeholder="pi.ph('warehouseCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costAmount')"
                name="costAmount"
              >
                <a-input-number
                  v-model:value="formState.costAmount"
                  :placeholder="pi.ph('costAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal1')"
                name="subtotal1"
              >
                <a-input-number
                  v-model:value="formState.subtotal1"
                  :placeholder="pi.ph('subtotal1')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal2')"
                name="subtotal2"
              >
                <a-input-number
                  v-model:value="formState.subtotal2"
                  :placeholder="pi.ph('subtotal2')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal3')"
                name="subtotal3"
              >
                <a-input-number
                  v-model:value="formState.subtotal3"
                  :placeholder="pi.ph('subtotal3')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal4')"
                name="subtotal4"
              >
                <a-input-number
                  v-model:value="formState.subtotal4"
                  :placeholder="pi.ph('subtotal4')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal5')"
                name="subtotal5"
              >
                <a-input-number
                  v-model:value="formState.subtotal5"
                  :placeholder="pi.ph('subtotal5')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subtotal6')"
                name="subtotal6"
              >
                <a-input-number
                  v-model:value="formState.subtotal6"
                  :placeholder="pi.ph('subtotal6')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('statisticsExchangeRate')"
                name="statisticsExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.statisticsExchangeRate"
                  :placeholder="pi.ph('statisticsExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('profitCenterCode')"
                name="profitCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.profitCenterCode"
                  api-url="TaktProfitCenters/options"
                  :placeholder="pi.ph('profitCenterCode')"
                  :disabled="!!formData?.salesInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('creditPrice')"
                name="creditPrice"
              >
                <a-input-number
                  v-model:value="formState.creditPrice"
                  :placeholder="pi.ph('creditPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerGroupSalesOrder')"
                name="customerGroupSalesOrder"
              >
                <a-input
                  v-model:value="formState.customerGroupSalesOrder"
                  :placeholder="pi.ph('customerGroupSalesOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('destinationCountryOrder')"
                name="destinationCountryOrder"
              >
                <TaktSelect
                  v-model:value="formState.destinationCountryOrder"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('destinationCountryOrder')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('regionOrder')"
                name="regionOrder"
              >
                <a-input
                  v-model:value="formState.regionOrder"
                  :placeholder="pi.ph('regionOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrganizationOrder')"
                name="salesOrganizationOrder"
              >
                <a-input
                  v-model:value="formState.salesOrganizationOrder"
                  :placeholder="pi.ph('salesOrganizationOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('distributionChannelOrder')"
                name="distributionChannelOrder"
              >
                <a-input
                  v-model:value="formState.distributionChannelOrder"
                  :placeholder="pi.ph('distributionChannelOrder')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('documentCategory')"
                name="documentCategory"
              >
                <a-input
                  v-model:value="formState.documentCategory"
                  :placeholder="pi.ph('documentCategory')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossAmount')"
                name="grossAmount"
              >
                <a-input-number
                  v-model:value="formState.grossAmount"
                  :placeholder="pi.ph('grossAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('exchangeRateDate')"
                name="exchangeRateDate"
              >
                <a-date-picker
                  v-model:value="formState.exchangeRateDate"
                  :placeholder="pi.ph('exchangeRateDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isObsolete')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售发票主表实体子表 salesInvoiceItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesInvoiceItemI18n } from '../composables/useInvoiceItemI18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceItemI18n()

import type { SalesInvoiceItemCreate } from '@/types/logistics/sales/invoiceItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","billingQuantity","salesUnit","baseUnit","scaleQuantity","billingQuantitySku","netWeight","grossWeight","weightUnit","businessAreaCode","pricingDate","serviceRenderedDate","pricingExchangeRate","netAmount","referenceDocumentCode","referenceDocumentItem","referenceDocumentCategory","salesDocumentCode","salesDocumentItem","salesDocumentReferenceFlag","materialCode","pricingReferenceMaterialCode","batchCode","materialGroup","salesItemCategory","productHierarchy","shippingPoint","division","partnerItem","departureCountry","plantRegion","pricingFlag","warehouseCode","costAmount","subtotal1","subtotal2","subtotal3","subtotal4","subtotal5","subtotal6","statisticsExchangeRate","profitCenterCode","creditPrice","customerGroupSalesOrder","destinationCountryOrder","regionOrder","salesOrganizationOrder","distributionChannelOrder","documentCategory","taxAmount","grossAmount","exchangeRateDate","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceItemCreate & { salesInvoiceItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  departureCountry: "CN",
  destinationCountryOrder: "CN"
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesInvoiceItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesInvoiceItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.salesInvoiceItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 salesInvoiceId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    if (rawlineNumber === undefined || rawlineNumber === null || rawlineNumber === '') {
      delete payload.lineNumber
    } else {
      const numlineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
      if (Number.isFinite(numlineNumber)) payload.lineNumber = numlineNumber
      else delete payload.lineNumber
    }
  }
  if ('billingQuantity' in payload) {
    const rawbillingQuantity = payload.billingQuantity
    if (rawbillingQuantity === undefined || rawbillingQuantity === null || rawbillingQuantity === '') {
      delete payload.billingQuantity
    } else {
      const numbillingQuantity = typeof rawbillingQuantity === 'number' ? rawbillingQuantity : Number(rawbillingQuantity)
      if (Number.isFinite(numbillingQuantity)) payload.billingQuantity = numbillingQuantity
      else delete payload.billingQuantity
    }
  }
  if ('scaleQuantity' in payload) {
    const rawscaleQuantity = payload.scaleQuantity
    if (rawscaleQuantity === undefined || rawscaleQuantity === null || rawscaleQuantity === '') {
      delete payload.scaleQuantity
    } else {
      const numscaleQuantity = typeof rawscaleQuantity === 'number' ? rawscaleQuantity : Number(rawscaleQuantity)
      if (Number.isFinite(numscaleQuantity)) payload.scaleQuantity = numscaleQuantity
      else delete payload.scaleQuantity
    }
  }
  if ('billingQuantitySku' in payload) {
    const rawbillingQuantitySku = payload.billingQuantitySku
    if (rawbillingQuantitySku === undefined || rawbillingQuantitySku === null || rawbillingQuantitySku === '') {
      delete payload.billingQuantitySku
    } else {
      const numbillingQuantitySku = typeof rawbillingQuantitySku === 'number' ? rawbillingQuantitySku : Number(rawbillingQuantitySku)
      if (Number.isFinite(numbillingQuantitySku)) payload.billingQuantitySku = numbillingQuantitySku
      else delete payload.billingQuantitySku
    }
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    if (rawnetWeight === undefined || rawnetWeight === null || rawnetWeight === '') {
      delete payload.netWeight
    } else {
      const numnetWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
      if (Number.isFinite(numnetWeight)) payload.netWeight = numnetWeight
      else delete payload.netWeight
    }
  }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    if (rawgrossWeight === undefined || rawgrossWeight === null || rawgrossWeight === '') {
      delete payload.grossWeight
    } else {
      const numgrossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
      if (Number.isFinite(numgrossWeight)) payload.grossWeight = numgrossWeight
      else delete payload.grossWeight
    }
  }
  if ('pricingExchangeRate' in payload) {
    const rawpricingExchangeRate = payload.pricingExchangeRate
    if (rawpricingExchangeRate === undefined || rawpricingExchangeRate === null || rawpricingExchangeRate === '') {
      delete payload.pricingExchangeRate
    } else {
      const numpricingExchangeRate = typeof rawpricingExchangeRate === 'number' ? rawpricingExchangeRate : Number(rawpricingExchangeRate)
      if (Number.isFinite(numpricingExchangeRate)) payload.pricingExchangeRate = numpricingExchangeRate
      else delete payload.pricingExchangeRate
    }
  }
  if ('netAmount' in payload) {
    const rawnetAmount = payload.netAmount
    if (rawnetAmount === undefined || rawnetAmount === null || rawnetAmount === '') {
      delete payload.netAmount
    } else {
      const numnetAmount = typeof rawnetAmount === 'number' ? rawnetAmount : Number(rawnetAmount)
      if (Number.isFinite(numnetAmount)) payload.netAmount = numnetAmount
      else delete payload.netAmount
    }
  }
  if ('referenceDocumentItem' in payload) {
    const rawreferenceDocumentItem = payload.referenceDocumentItem
    if (rawreferenceDocumentItem === undefined || rawreferenceDocumentItem === null || rawreferenceDocumentItem === '') {
      delete payload.referenceDocumentItem
    } else {
      const numreferenceDocumentItem = typeof rawreferenceDocumentItem === 'number' ? rawreferenceDocumentItem : Number(rawreferenceDocumentItem)
      if (Number.isFinite(numreferenceDocumentItem)) payload.referenceDocumentItem = numreferenceDocumentItem
      else delete payload.referenceDocumentItem
    }
  }
  if ('salesDocumentItem' in payload) {
    const rawsalesDocumentItem = payload.salesDocumentItem
    if (rawsalesDocumentItem === undefined || rawsalesDocumentItem === null || rawsalesDocumentItem === '') {
      delete payload.salesDocumentItem
    } else {
      const numsalesDocumentItem = typeof rawsalesDocumentItem === 'number' ? rawsalesDocumentItem : Number(rawsalesDocumentItem)
      if (Number.isFinite(numsalesDocumentItem)) payload.salesDocumentItem = numsalesDocumentItem
      else delete payload.salesDocumentItem
    }
  }
  if ('partnerItem' in payload) {
    const rawpartnerItem = payload.partnerItem
    if (rawpartnerItem === undefined || rawpartnerItem === null || rawpartnerItem === '') {
      delete payload.partnerItem
    } else {
      const numpartnerItem = typeof rawpartnerItem === 'number' ? rawpartnerItem : Number(rawpartnerItem)
      if (Number.isFinite(numpartnerItem)) payload.partnerItem = numpartnerItem
      else delete payload.partnerItem
    }
  }
  if ('costAmount' in payload) {
    const rawcostAmount = payload.costAmount
    if (rawcostAmount === undefined || rawcostAmount === null || rawcostAmount === '') {
      delete payload.costAmount
    } else {
      const numcostAmount = typeof rawcostAmount === 'number' ? rawcostAmount : Number(rawcostAmount)
      if (Number.isFinite(numcostAmount)) payload.costAmount = numcostAmount
      else delete payload.costAmount
    }
  }
  if ('subtotal1' in payload) {
    const rawsubtotal1 = payload.subtotal1
    if (rawsubtotal1 === undefined || rawsubtotal1 === null || rawsubtotal1 === '') {
      delete payload.subtotal1
    } else {
      const numsubtotal1 = typeof rawsubtotal1 === 'number' ? rawsubtotal1 : Number(rawsubtotal1)
      if (Number.isFinite(numsubtotal1)) payload.subtotal1 = numsubtotal1
      else delete payload.subtotal1
    }
  }
  if ('subtotal2' in payload) {
    const rawsubtotal2 = payload.subtotal2
    if (rawsubtotal2 === undefined || rawsubtotal2 === null || rawsubtotal2 === '') {
      delete payload.subtotal2
    } else {
      const numsubtotal2 = typeof rawsubtotal2 === 'number' ? rawsubtotal2 : Number(rawsubtotal2)
      if (Number.isFinite(numsubtotal2)) payload.subtotal2 = numsubtotal2
      else delete payload.subtotal2
    }
  }
  if ('subtotal3' in payload) {
    const rawsubtotal3 = payload.subtotal3
    if (rawsubtotal3 === undefined || rawsubtotal3 === null || rawsubtotal3 === '') {
      delete payload.subtotal3
    } else {
      const numsubtotal3 = typeof rawsubtotal3 === 'number' ? rawsubtotal3 : Number(rawsubtotal3)
      if (Number.isFinite(numsubtotal3)) payload.subtotal3 = numsubtotal3
      else delete payload.subtotal3
    }
  }
  if ('subtotal4' in payload) {
    const rawsubtotal4 = payload.subtotal4
    if (rawsubtotal4 === undefined || rawsubtotal4 === null || rawsubtotal4 === '') {
      delete payload.subtotal4
    } else {
      const numsubtotal4 = typeof rawsubtotal4 === 'number' ? rawsubtotal4 : Number(rawsubtotal4)
      if (Number.isFinite(numsubtotal4)) payload.subtotal4 = numsubtotal4
      else delete payload.subtotal4
    }
  }
  if ('subtotal5' in payload) {
    const rawsubtotal5 = payload.subtotal5
    if (rawsubtotal5 === undefined || rawsubtotal5 === null || rawsubtotal5 === '') {
      delete payload.subtotal5
    } else {
      const numsubtotal5 = typeof rawsubtotal5 === 'number' ? rawsubtotal5 : Number(rawsubtotal5)
      if (Number.isFinite(numsubtotal5)) payload.subtotal5 = numsubtotal5
      else delete payload.subtotal5
    }
  }
  if ('subtotal6' in payload) {
    const rawsubtotal6 = payload.subtotal6
    if (rawsubtotal6 === undefined || rawsubtotal6 === null || rawsubtotal6 === '') {
      delete payload.subtotal6
    } else {
      const numsubtotal6 = typeof rawsubtotal6 === 'number' ? rawsubtotal6 : Number(rawsubtotal6)
      if (Number.isFinite(numsubtotal6)) payload.subtotal6 = numsubtotal6
      else delete payload.subtotal6
    }
  }
  if ('statisticsExchangeRate' in payload) {
    const rawstatisticsExchangeRate = payload.statisticsExchangeRate
    if (rawstatisticsExchangeRate === undefined || rawstatisticsExchangeRate === null || rawstatisticsExchangeRate === '') {
      delete payload.statisticsExchangeRate
    } else {
      const numstatisticsExchangeRate = typeof rawstatisticsExchangeRate === 'number' ? rawstatisticsExchangeRate : Number(rawstatisticsExchangeRate)
      if (Number.isFinite(numstatisticsExchangeRate)) payload.statisticsExchangeRate = numstatisticsExchangeRate
      else delete payload.statisticsExchangeRate
    }
  }
  if ('creditPrice' in payload) {
    const rawcreditPrice = payload.creditPrice
    if (rawcreditPrice === undefined || rawcreditPrice === null || rawcreditPrice === '') {
      delete payload.creditPrice
    } else {
      const numcreditPrice = typeof rawcreditPrice === 'number' ? rawcreditPrice : Number(rawcreditPrice)
      if (Number.isFinite(numcreditPrice)) payload.creditPrice = numcreditPrice
      else delete payload.creditPrice
    }
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    if (rawtaxAmount === undefined || rawtaxAmount === null || rawtaxAmount === '') {
      delete payload.taxAmount
    } else {
      const numtaxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
      if (Number.isFinite(numtaxAmount)) payload.taxAmount = numtaxAmount
      else delete payload.taxAmount
    }
  }
  if ('grossAmount' in payload) {
    const rawgrossAmount = payload.grossAmount
    if (rawgrossAmount === undefined || rawgrossAmount === null || rawgrossAmount === '') {
      delete payload.grossAmount
    } else {
      const numgrossAmount = typeof rawgrossAmount === 'number' ? rawgrossAmount : Number(rawgrossAmount)
      if (Number.isFinite(numgrossAmount)) payload.grossAmount = numgrossAmount
      else delete payload.grossAmount
    }
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    if (rawisObsolete === undefined || rawisObsolete === null || rawisObsolete === '') {
      delete payload.isObsolete
    } else {
      const numisObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
      if (Number.isFinite(numisObsolete)) payload.isObsolete = numisObsolete
      else delete payload.isObsolete
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.salesInvoiceItemId) {
    payload.salesInvoiceItemId = props.formData.salesInvoiceItemId
    delete payload.numberingRuleCode
  }
  payload.salesInvoiceId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.salesInvoiceCode ?? masterRow.SalesInvoiceCode
    const masterName = masterRow.salesInvoiceName ?? masterRow.SalesInvoiceName
    if (masterCode != null && masterCode !== '' && !payload.salesInvoiceCode) {
      payload.salesInvoiceCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.salesInvoiceName) {
      payload.salesInvoiceName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesInvoiceItemId)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>

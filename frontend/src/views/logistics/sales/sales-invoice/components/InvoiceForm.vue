<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/sales-invoice/components -->
<!-- 文件名称：InvoiceForm.vue -->
<!-- 功能描述：Takt销售发票主表实体维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form invoice-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="invoice-form__master min-h-0 flex-1 overflow-hidden">
    <a-tabs
      v-model:active-key="activeTab"
      class="invoice-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
                :label="pi.label('billingDocumentCode')"
                name="billingDocumentCode"
              >
                <a-input
                  v-model:value="formState.billingDocumentCode"
                  :placeholder="pi.ph('billingDocumentCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingType')"
                name="billingType"
              >
                <a-input
                  v-model:value="formState.billingType"
                  :placeholder="pi.ph('billingType')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingCategory')"
                name="billingCategory"
              >
                <a-input
                  v-model:value="formState.billingCategory"
                  :placeholder="pi.ph('billingCategory')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
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
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrganization')"
                name="salesOrganization"
              >
                <a-input
                  v-model:value="formState.salesOrganization"
                  :placeholder="pi.ph('salesOrganization')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('distributionChannel')"
                name="distributionChannel"
              >
                <a-input
                  v-model:value="formState.distributionChannel"
                  :placeholder="pi.ph('distributionChannel')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingProcedure')"
                name="pricingProcedure"
              >
                <a-input
                  v-model:value="formState.pricingProcedure"
                  :placeholder="pi.ph('pricingProcedure')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conditionCode')"
                name="conditionCode"
              >
                <a-input
                  v-model:value="formState.conditionCode"
                  :placeholder="pi.ph('conditionCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shippingConditions')"
                name="shippingConditions"
              >
                <TaktSelect
                  v-model:value="formState.shippingConditions"
                  dict-type="logistics_sales_shipping_conditions"
                  :placeholder="pi.ph('shippingConditions')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('billingDate')"
                name="billingDate"
              >
                <a-date-picker
                  v-model:value="formState.billingDate"
                  :placeholder="pi.ph('billingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerGroup')"
                name="customerGroup"
              >
                <a-input
                  v-model:value="formState.customerGroup"
                  :placeholder="pi.ph('customerGroup')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('incoterms1')"
                name="incoterms1"
              >
                <a-input
                  v-model:value="formState.incoterms1"
                  :placeholder="pi.ph('incoterms1')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('incoterms2')"
                name="incoterms2"
              >
                <a-input
                  v-model:value="formState.incoterms2"
                  :placeholder="pi.ph('incoterms2')"
                  show-count
                  :maxlength="28"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postingStatus')"
                name="postingStatus"
              >
                <a-input
                  v-model:value="formState.postingStatus"
                  :placeholder="pi.ph('postingStatus')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountingExchangeRate')"
                name="accountingExchangeRate"
              >
                <a-input-number
                  v-model:value="formState.accountingExchangeRate"
                  :placeholder="pi.ph('accountingExchangeRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentTerms')"
                name="paymentTerms"
              >
                <a-input
                  v-model:value="formState.paymentTerms"
                  :placeholder="pi.ph('paymentTerms')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountAssignmentGroup')"
                name="accountAssignmentGroup"
              >
                <a-input
                  v-model:value="formState.accountAssignmentGroup"
                  :placeholder="pi.ph('accountAssignmentGroup')"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countryCode')"
                name="countryCode"
              >
                <TaktSelect
                  v-model:value="formState.countryCode"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('countryCode')"
                  :disabled="!!formData?.salesInvoiceId"
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
                :label="pi.label('payerCode')"
                name="payerCode"
              >
                <TaktSelect
                  v-model:value="formState.payerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('payerCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <TaktSelect
                  v-model:value="formState.customerCode"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('customerCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('statisticsCurrencyCode')"
                name="statisticsCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.statisticsCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('statisticsCurrencyCode')"
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('foreignTradeCode')"
                name="foreignTradeCode"
              >
                <a-input
                  v-model:value="formState.foreignTradeCode"
                  :placeholder="pi.ph('foreignTradeCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cancelledBillingDocument')"
                name="cancelledBillingDocument"
              >
                <a-input
                  v-model:value="formState.cancelledBillingDocument"
                  :placeholder="pi.ph('cancelledBillingDocument')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('invoiceListType')"
                name="invoiceListType"
              >
                <a-input
                  v-model:value="formState.invoiceListType"
                  :placeholder="pi.ph('invoiceListType')"
                  show-count
                  :maxlength="4"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('hierarchyTypePricing')"
                name="hierarchyTypePricing"
              >
                <a-input
                  v-model:value="formState.hierarchyTypePricing"
                  :placeholder="pi.ph('hierarchyTypePricing')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tradingPartner')"
                name="tradingPartner"
              >
                <a-input
                  v-model:value="formState.tradingPartner"
                  :placeholder="pi.ph('tradingPartner')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxDepartureCountry')"
                name="taxDepartureCountry"
              >
                <TaktSelect
                  v-model:value="formState.taxDepartureCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('taxDepartureCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('organizationSalesTaxNumber')"
                name="organizationSalesTaxNumber"
              >
                <a-input
                  v-model:value="formState.organizationSalesTaxNumber"
                  :placeholder="pi.ph('organizationSalesTaxNumber')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('countrySalesTaxNumber')"
                name="countrySalesTaxNumber"
              >
                <a-input
                  v-model:value="formState.countrySalesTaxNumber"
                  :placeholder="pi.ph('countrySalesTaxNumber')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceCode')"
                name="referenceCode"
              >
                <a-input
                  v-model:value="formState.referenceCode"
                  :placeholder="pi.ph('referenceCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.salesInvoiceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cancelledFlag')"
                name="cancelledFlag"
              >
                <a-input
                  v-model:value="formState.cancelledFlag"
                  :placeholder="pi.ph('cancelledFlag')"
                  show-count
                  :maxlength="1"
                  allow-clear
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
                :label="pi.label('paymentReference')"
                name="paymentReference"
              >
                <a-input
                  v-model:value="formState.paymentReference"
                  :placeholder="pi.ph('paymentReference')"
                  show-count
                  :maxlength="30"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reversalReason')"
                name="reversalReason"
              >
                <a-input
                  v-model:value="formState.reversalReason"
                  :placeholder="pi.ph('reversalReason')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postedBy')"
                name="postedBy"
              >
                <a-input
                  v-model:value="formState.postedBy"
                  :placeholder="pi.ph('postedBy')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
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
            <a-col :span="24">
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
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    </div>
    <!-- 下：子表（弹窗视口约 1/2） -->
    <div
      ref="detailHostRef"
      class="invoice-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
    <TaktEditableTable
      ref="salesInvoiceItemTableRef"
      v-model="childSalesInvoiceItemRows"
      :columns="salesInvoiceItemFormColumns"
      :title="salesInvoiceItemPi.self()"
      :add-button-entity="salesInvoiceItemPi.self()"
      id-field="salesInvoiceItemId"
      :default-row="createDefaultSalesInvoiceItemRow"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-departureCountry="{ record }">
        <TaktSelect
          v-model:value="record.departureCountry"
          dict-type="sys_country_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('departureCountry')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-warehouseCode="{ record }">
        <TaktSelect
          v-model:value="record.warehouseCode"
          api-url="TaktWarehouses/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('warehouseCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-profitCenterCode="{ record }">
        <TaktSelect
          v-model:value="record.profitCenterCode"
          api-url="TaktProfitCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.queryPh('profitCenterCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-destinationCountryOrder="{ record }">
        <TaktSelect
          v-model:value="record.destinationCountryOrder"
          dict-type="sys_country_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('destinationCountryOrder')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="salesInvoiceItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
    </div>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt销售发票主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/sales-invoice/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSalesInvoiceI18n } from '../composables/useInvoiceI18n'

/** 实体字段 i18n */
const pi = useSalesInvoiceI18n()

import type { SalesInvoiceCreate } from '@/types/logistics/sales/invoice.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","billingDocumentCode","billingType","billingCategory","documentCategory","currencyCode","salesOrganization","distributionChannel","pricingProcedure","conditionCode","shippingConditions","billingDate","customerGroup","incoterms1","incoterms2","postingStatus","accountingExchangeRate","paymentTerms","accountAssignmentGroup","countryCode","netAmount","payerCode","customerCode","statisticsCurrencyCode","foreignTradeCode","cancelledBillingDocument","invoiceListType","division","hierarchyTypePricing","tradingPartner","taxDepartureCountry","organizationSalesTaxNumber","countrySalesTaxNumber","referenceCode","cancelledFlag","exchangeRateDate","paymentReference","reversalReason","postedBy","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useSalesInvoiceItemI18n } from '../composables/useInvoiceItemI18n'

const salesInvoiceItemPi = useSalesInvoiceItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSalesInvoiceItemRows = ref<Record<string, unknown>[]>([])
const salesInvoiceItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedSalesInvoiceItemRow(row: Record<string, unknown>): boolean {
  const id = row.salesInvoiceItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextSalesInvoiceItemLineNumber(): number {
  const rows = salesInvoiceItemTableRef.value?.getRows?.() ?? childSalesInvoiceItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 salesInvoiceItem 可编辑列 */
const salesInvoiceItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: salesInvoiceItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'billingQuantity',
    title: salesInvoiceItemPi.label('billingQuantity'),
    width: 140,
  },
  {
    key: 'salesUnit',
    title: salesInvoiceItemPi.label('salesUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesUnit'),
  },
  {
    key: 'baseUnit',
    title: salesInvoiceItemPi.label('baseUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('baseUnit'),
  },
  {
    key: 'scaleQuantity',
    title: salesInvoiceItemPi.label('scaleQuantity'),
    width: 140,
  },
  {
    key: 'billingQuantitySku',
    title: salesInvoiceItemPi.label('billingQuantitySku'),
    width: 140,
  },
  {
    key: 'netWeight',
    title: salesInvoiceItemPi.label('netWeight'),
    width: 140,
  },
  {
    key: 'grossWeight',
    title: salesInvoiceItemPi.label('grossWeight'),
    width: 140,
  },
  {
    key: 'weightUnit',
    title: salesInvoiceItemPi.label('weightUnit'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('weightUnit'),
  },
  {
    key: 'businessAreaCode',
    title: salesInvoiceItemPi.label('businessAreaCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('businessAreaCode'),
  },
  {
    key: 'pricingDate',
    title: salesInvoiceItemPi.label('pricingDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'serviceRenderedDate',
    title: salesInvoiceItemPi.label('serviceRenderedDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'pricingExchangeRate',
    title: salesInvoiceItemPi.label('pricingExchangeRate'),
    width: 140,
  },
  {
    key: 'netAmount',
    title: salesInvoiceItemPi.label('netAmount'),
    width: 140,
  },
  {
    key: 'referenceDocumentCode',
    title: salesInvoiceItemPi.label('referenceDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('referenceDocumentCode'),
  },
  {
    key: 'referenceDocumentItem',
    title: salesInvoiceItemPi.label('referenceDocumentItem'),
    width: 140,
  },
  {
    key: 'referenceDocumentCategory',
    title: salesInvoiceItemPi.label('referenceDocumentCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('referenceDocumentCategory'),
  },
  {
    key: 'salesDocumentCode',
    title: salesInvoiceItemPi.label('salesDocumentCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesDocumentCode'),
  },
  {
    key: 'salesDocumentItem',
    title: salesInvoiceItemPi.label('salesDocumentItem'),
    width: 140,
  },
  {
    key: 'salesDocumentReferenceFlag',
    title: salesInvoiceItemPi.label('salesDocumentReferenceFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesDocumentReferenceFlag'),
  },
  {
    key: 'materialCode',
    title: salesInvoiceItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'pricingReferenceMaterialCode',
    title: salesInvoiceItemPi.label('pricingReferenceMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('pricingReferenceMaterialCode'),
  },
  {
    key: 'batchCode',
    title: salesInvoiceItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('batchCode'),
  },
  {
    key: 'materialGroup',
    title: salesInvoiceItemPi.label('materialGroup'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('materialGroup'),
  },
  {
    key: 'salesItemCategory',
    title: salesInvoiceItemPi.label('salesItemCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesItemCategory'),
  },
  {
    key: 'productHierarchy',
    title: salesInvoiceItemPi.label('productHierarchy'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('productHierarchy'),
  },
  {
    key: 'shippingPoint',
    title: salesInvoiceItemPi.label('shippingPoint'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('shippingPoint'),
  },
  {
    key: 'division',
    title: salesInvoiceItemPi.label('division'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('division'),
  },
  {
    key: 'partnerItem',
    title: salesInvoiceItemPi.label('partnerItem'),
    width: 140,
  },
  {
    key: 'departureCountry',
    title: salesInvoiceItemPi.label('departureCountry'),
    width: 140,
  },
  {
    key: 'plantRegion',
    title: salesInvoiceItemPi.label('plantRegion'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('plantRegion'),
  },
  {
    key: 'pricingFlag',
    title: salesInvoiceItemPi.label('pricingFlag'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('pricingFlag'),
  },
  {
    key: 'warehouseCode',
    title: salesInvoiceItemPi.label('warehouseCode'),
    width: 140,
  },
  {
    key: 'costAmount',
    title: salesInvoiceItemPi.label('costAmount'),
    width: 140,
  },
  {
    key: 'subtotal1',
    title: salesInvoiceItemPi.label('subtotal1'),
    width: 140,
  },
  {
    key: 'subtotal2',
    title: salesInvoiceItemPi.label('subtotal2'),
    width: 140,
  },
  {
    key: 'subtotal3',
    title: salesInvoiceItemPi.label('subtotal3'),
    width: 140,
  },
  {
    key: 'subtotal4',
    title: salesInvoiceItemPi.label('subtotal4'),
    width: 140,
  },
  {
    key: 'subtotal5',
    title: salesInvoiceItemPi.label('subtotal5'),
    width: 140,
  },
  {
    key: 'subtotal6',
    title: salesInvoiceItemPi.label('subtotal6'),
    width: 140,
  },
  {
    key: 'statisticsExchangeRate',
    title: salesInvoiceItemPi.label('statisticsExchangeRate'),
    width: 140,
  },
  {
    key: 'profitCenterCode',
    title: salesInvoiceItemPi.label('profitCenterCode'),
    width: 140,
  },
  {
    key: 'creditPrice',
    title: salesInvoiceItemPi.label('creditPrice'),
    width: 140,
  },
  {
    key: 'customerGroupSalesOrder',
    title: salesInvoiceItemPi.label('customerGroupSalesOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('customerGroupSalesOrder'),
  },
  {
    key: 'destinationCountryOrder',
    title: salesInvoiceItemPi.label('destinationCountryOrder'),
    width: 140,
  },
  {
    key: 'regionOrder',
    title: salesInvoiceItemPi.label('regionOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('regionOrder'),
  },
  {
    key: 'salesOrganizationOrder',
    title: salesInvoiceItemPi.label('salesOrganizationOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('salesOrganizationOrder'),
  },
  {
    key: 'distributionChannelOrder',
    title: salesInvoiceItemPi.label('distributionChannelOrder'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('distributionChannelOrder'),
  },
  {
    key: 'documentCategory',
    title: salesInvoiceItemPi.label('documentCategory'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: salesInvoiceItemPi.ph('documentCategory'),
  },
  {
    key: 'taxAmount',
    title: salesInvoiceItemPi.label('taxAmount'),
    width: 140,
  },
  {
    key: 'grossAmount',
    title: salesInvoiceItemPi.label('grossAmount'),
    width: 140,
  },
  {
    key: 'exchangeRateDate',
    title: salesInvoiceItemPi.label('exchangeRateDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'isObsolete',
    title: salesInvoiceItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null | undefined) {
  const rows_salesInvoiceItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childSalesInvoiceItemRows.value = rows_salesInvoiceItem
}

function createDefaultSalesInvoiceItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextSalesInvoiceItemLineNumber(),
    billingQuantity: 0,
    salesUnit: '',
    baseUnit: '',
    scaleQuantity: 0,
    billingQuantitySku: 0,
    netWeight: 0,
    grossWeight: 0,
    weightUnit: '',
    businessAreaCode: '',
    pricingDate: '',
    serviceRenderedDate: '',
    pricingExchangeRate: 0,
    netAmount: 0,
    referenceDocumentCode: '',
    referenceDocumentItem: 0,
    referenceDocumentCategory: '',
    salesDocumentCode: '',
    salesDocumentItem: 0,
    salesDocumentReferenceFlag: '',
    materialCode: '',
    pricingReferenceMaterialCode: '',
    batchCode: '',
    materialGroup: '',
    salesItemCategory: '',
    productHierarchy: '',
    shippingPoint: '',
    division: '',
    partnerItem: 0,
    departureCountry: '',
    plantRegion: '',
    pricingFlag: '',
    warehouseCode: '',
    costAmount: 0,
    subtotal1: 0,
    subtotal2: 0,
    subtotal3: 0,
    subtotal4: 0,
    subtotal5: 0,
    subtotal6: 0,
    statisticsExchangeRate: 0,
    profitCenterCode: '',
    creditPrice: 0,
    customerGroupSalesOrder: '',
    destinationCountryOrder: '',
    regionOrder: '',
    salesOrganizationOrder: '',
    distributionChannelOrder: '',
    documentCategory: '',
    taxAmount: 0,
    grossAmount: 0,
    exchangeRateDate: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.salesInvoiceId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: salesInvoiceItemTableRef.value?.getRows?.() ?? childSalesInvoiceItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        salesInvoiceId: isUpdate ? masterId : 0,
      }
      if (isUpdate && isPersistedSalesInvoiceItemRow(row)) {
        normalized.salesInvoiceItemId = row.salesInvoiceItemId
      } else {
        delete normalized.salesInvoiceItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalesInvoiceCreate & { salesInvoiceId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
} from '@/utils/tableScroll'

/** 子表半区宿主（弹窗视口约 1/2） */
const detailHostRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（半区内扣除标题/表头/汇总） */
const detailScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailHostResizeObserver: ResizeObserver | null = null

/**
 * 按子表半区实测 scroll.y
 */
function recalcDetailScrollYPx(): void {
  const host = detailHostRef.value
  if (host == null || host.clientHeight <= 0) {
    return
  }
  const tableRoot = host.querySelector('.takt-editable-table') as HTMLElement | null
  const toolbar = tableRoot?.querySelector(':scope > .mb-2') as HTMLElement | null
  const toolbarH = toolbar?.offsetHeight ?? 0
  const sectionPad = 12
  const next = Math.floor(
    host.clientHeight
      - toolbarH
      - sectionPad
      - TAKT_TABLE_HEADER_FALLBACK_PX
      - TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
  )
  detailScrollYPx.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, next)
}

/** 监听弹窗/半区尺寸变化 */
function bindDetailHostResizeObserver(): void {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  const host = detailHostRef.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const target =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
    ?? host
  detailHostResizeObserver = new ResizeObserver(() => {
    recalcDetailScrollYPx()
  })
  detailHostResizeObserver.observe(target)
  detailHostResizeObserver.observe(host)
}

onMounted(() => {
  void nextTick(() => {
    recalcDetailScrollYPx()
    bindDetailHostResizeObserver()
  })
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', recalcDetailScrollYPx)
  }
})

onBeforeUnmount(() => {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', recalcDetailScrollYPx)
  }
})

/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  currencyCode: "CNY",
  shippingConditions: "Z1",
  countryCode: "CN",
  statisticsCurrencyCode: "CNY",
  taxDepartureCountry: "CN"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salesInvoiceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salesInvoiceId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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

watch(
  () => props.formData,
  () => {
    void nextTick(() => {
      recalcDetailScrollYPx()
      bindDetailHostResizeObserver()
    })
  },
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.salesInvoiceId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billingDocumentCode: [
    {
      required: true,
      message: pi.ph('billingDocumentCode'),
      trigger: 'blur'
    }
  ],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  billingDate: [
    {
      required: true,
      message: pi.ph('billingDate'),
      trigger: 'change'
    }
  ],
  netAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('netAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('netAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await salesInvoiceItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('accountingExchangeRate' in payload) {
    const rawaccountingExchangeRate = payload.accountingExchangeRate
    if (rawaccountingExchangeRate === undefined || rawaccountingExchangeRate === null || rawaccountingExchangeRate === '') {
      delete payload.accountingExchangeRate
    } else {
      const numaccountingExchangeRate = typeof rawaccountingExchangeRate === 'number' ? rawaccountingExchangeRate : Number(rawaccountingExchangeRate)
      if (Number.isFinite(numaccountingExchangeRate)) payload.accountingExchangeRate = numaccountingExchangeRate
      else delete payload.accountingExchangeRate
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
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.salesInvoiceId) {
    payload.salesInvoiceId = props.formData.salesInvoiceId
    delete payload.numberingRuleCode
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salesInvoiceId)
  childSalesInvoiceItemRows.value = []
  salesInvoiceItemTableRef.value?.resetRows?.()
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.invoice-form__master {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.invoice-form__master > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.invoice-form__master :deep(.invoice-form-tabs.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.invoice-form__master :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.invoice-form__master :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.invoice-form__master :deep(.ant-tabs-content),
.invoice-form__master :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/vendor/components -->
<!-- 文件名称：VendorForm.vue -->
<!-- 功能描述：Takt经销商实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="vendor-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/6)'"
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
                :label="pi.label('vendorCode')"
                name="vendorCode"
              >
                <a-input
                  v-model:value="formState.vendorCode"
                  :placeholder="pi.ph('vendorCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorName1')"
                name="vendorName1"
              >
                <a-input
                  v-model:value="formState.vendorName1"
                  :placeholder="pi.ph('vendorName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorName2')"
                name="vendorName2"
              >
                <a-input
                  v-model:value="formState.vendorName2"
                  :placeholder="pi.ph('vendorName2')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorShortName')"
                name="vendorShortName"
              >
                <a-input
                  v-model:value="formState.vendorShortName"
                  :placeholder="pi.ph('vendorShortName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorType')"
                name="vendorType"
              >
                <TaktSelect
                  v-model:value="formState.vendorType"
                  dict-type="logistics_sales_vendor_category"
                  :placeholder="pi.ph('vendorType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('enterpriseNature')"
                name="enterpriseNature"
              >
                <TaktSelect
                  v-model:value="formState.enterpriseNature"
                  dict-type="sys_enterprise_nature"
                  :placeholder="pi.ph('enterpriseNature')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('industryAttribute')"
                name="industryAttribute"
              >
                <TaktSelect
                  v-model:value="formState.industryAttribute"
                  dict-type="sys_industry_attribute"
                  :placeholder="pi.ph('industryAttribute')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorTaxNumber')"
                name="vendorTaxNumber"
              >
                <a-input
                  v-model:value="formState.vendorTaxNumber"
                  :placeholder="pi.ph('vendorTaxNumber')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <TaktSelect
                  v-model:value="formState.taxCode"
                  dict-type="accounting_financial_tax_code"
                  :placeholder="pi.ph('taxCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRate')"
                name="taxRate"
              >
                <TaktSelect
                  v-model:value="formState.taxRate"
                  dict-type="accounting_financial_tax_code"
                  :placeholder="pi.ph('taxRate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationCountry')"
                name="registrationCountry"
              >
                <TaktSelect
                  v-model:value="formState.registrationCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('registrationCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationProvince')"
                name="registrationProvince"
              >
                <TaktSelect
                  v-model:value="formState.registrationProvince"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 2 }"
                  :placeholder="pi.ph('registrationProvince')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registrationCity')"
                name="registrationCity"
              >
                <TaktSelect
                  v-model:value="formState.registrationCity"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 3 }"
                  :placeholder="pi.ph('registrationCity')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress1')"
                name="registrationAddress1"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress1"
                  :placeholder="pi.ph('registrationAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('registrationAddress2')"
                name="registrationAddress2"
              >
                <a-textarea
                  v-model:value="formState.registrationAddress2"
                  :placeholder="pi.ph('registrationAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorPhone')"
                name="vendorPhone"
              >
                <a-input
                  v-model:value="formState.vendorPhone"
                  :placeholder="pi.ph('vendorPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorFax')"
                name="vendorFax"
              >
                <a-input
                  v-model:value="formState.vendorFax"
                  :placeholder="pi.ph('vendorFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorEmail')"
                name="vendorEmail"
              >
                <a-input
                  v-model:value="formState.vendorEmail"
                  :placeholder="pi.ph('vendorEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorWebsite')"
                name="vendorWebsite"
              >
                <a-input
                  v-model:value="formState.vendorWebsite"
                  :placeholder="pi.ph('vendorWebsite')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPerson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="pi.ph('contactPerson')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPhone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="pi.ph('contactPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactEmail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="pi.ph('contactEmail')"
                  show-count
                  :maxlength="100"
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
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reconciliationAccount')"
                name="reconciliationAccount"
              >
                <TaktSelect
                  v-model:value="formState.reconciliationAccount"
                  api-url="TaktAccountTitles/options"
                  :api-params="{ reconciliationOnly: true, auxiliaryType: 'K' }"
                  :placeholder="pi.ph('reconciliationAccount')"
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
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clearingWithCustomer')"
                name="clearingWithCustomer"
              >
                <TaktSelect
                  v-model:value="formState.clearingWithCustomer"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('clearingWithCustomer')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentMethod')"
                name="paymentMethod"
              >
                <TaktSelect
                  v-model:value="formState.paymentMethod"
                  dict-type="accounting_financial_payment_method"
                  :placeholder="pi.ph('paymentMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('paymentTerms')"
                name="paymentTerms"
              >
                <TaktSelect
                  v-model:value="formState.paymentTerms"
                  dict-type="accounting_financial_payment_terms_param"
                  :placeholder="pi.ph('paymentTerms')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankCode')"
                name="bankCode"
              >
                <TaktSelect
                  v-model:value="formState.bankCode"
                  api-url="TaktBanks/options"
                  :placeholder="pi.ph('bankCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankAccount')"
                name="bankAccount"
              >
                <a-input
                  v-model:value="formState.bankAccount"
                  :placeholder="pi.ph('bankAccount')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountHolder')"
                name="accountHolder"
              >
                <a-input
                  v-model:value="formState.accountHolder"
                  :placeholder="pi.ph('accountHolder')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grBasedInvoiceInspection')"
                name="grBasedInvoiceInspection"
              >
                <TaktSelect
                  v-model:value="formState.grBasedInvoiceInspection"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('grBasedInvoiceInspection')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('incoterms1')"
                name="incoterms1"
              >
                <TaktSelect
                  v-model:value="formState.incoterms1"
                  dict-type="logistics_sales_incoterms1"
                  :placeholder="pi.ph('incoterms1')"
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
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('automaticPurchaseOrder')"
                name="automaticPurchaseOrder"
              >
                <TaktSelect
                  v-model:value="formState.automaticPurchaseOrder"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('automaticPurchaseOrder')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingDateControl')"
                name="pricingDateControl"
              >
                <TaktSelect
                  v-model:value="formState.pricingDateControl"
                  dict-type="logistics_procurement_pricing_date_control"
                  :placeholder="pi.ph('pricingDateControl')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseGroup')"
                name="purchaseGroup"
              >
                <TaktSelect
                  v-model:value="formState.purchaseGroup"
                  api-url="TaktPurchaseGroups/options"
                  :placeholder="pi.ph('purchaseGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedDeliveryTimeDays')"
                name="plannedDeliveryTimeDays"
              >
                <a-input-number
                  v-model:value="formState.plannedDeliveryTimeDays"
                  :placeholder="pi.ph('plannedDeliveryTimeDays')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('evaluatedReceiptSettlement')"
                name="evaluatedReceiptSettlement"
              >
                <TaktSelect
                  v-model:value="formState.evaluatedReceiptSettlement"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('evaluatedReceiptSettlement')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('purchasingOrganization')"
                name="purchasingOrganization"
              >
                <TaktSelect
                  v-model:value="formState.purchasingOrganization"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('purchasingOrganization')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('creditLevel')"
                name="creditLevel"
              >
                <TaktSelect
                  v-model:value="formState.creditLevel"
                  dict-type="logistics_sales_credit_rating"
                  :placeholder="pi.ph('creditLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('creditAmount')"
                name="creditAmount"
              >
                <a-input-number
                  v-model:value="formState.creditAmount"
                  :placeholder="pi.ph('creditAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('authorizedBrand')"
                name="authorizedBrand"
              >
                <a-input
                  v-model:value="formState.authorizedBrand"
                  :placeholder="pi.ph('authorizedBrand')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('agentRegion')"
                name="agentRegion"
              >
                <a-input
                  v-model:value="formState.agentRegion"
                  :placeholder="pi.ph('agentRegion')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('vendorLevel')"
                name="vendorLevel"
              >
                <TaktSelect
                  v-model:value="formState.vendorLevel"
                  dict-type="logistics_sales_grade"
                  :placeholder="pi.ph('vendorLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('evaluationScore')"
                name="evaluationScore"
              >
                <a-input-number
                  v-model:value="formState.evaluationScore"
                  :placeholder="pi.ph('evaluationScore')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('vendorStatus')"
                name="vendorStatus"
              >
                <TaktSelect
                  v-model:value="formState.vendorStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('vendorStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/6)'"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt经销商实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/vendor/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useVendorI18n } from '../composables/useVendorI18n'

/** 实体字段 i18n */
const pi = useVendorI18n()
import type { VendorCreate } from '@/types/logistics/procurement/vendor.types'
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
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<VendorCreate & { vendorId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  vendorType: 0,
  enterpriseNature: "150",
  industryAttribute: "C",
  taxCode: "J2",
  registrationCountry: "CN",
  currencyCode: "CNY",
  paymentMethod: 0,
  paymentTerms: "PREPAYSHIP",
  incoterms1: "FOB",
  creditLevel: 0,
  vendorLevel: 0,
  vendorStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 vendorId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.vendorId) {
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
    if (!props.formData?.vendorId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  vendorCode: [
    {
      required: true,
      message: pi.ph('vendorCode'),
      trigger: 'blur'
    }
  ],
  vendorName1: [
    {
      required: true,
      message: pi.ph('vendorName1'),
      trigger: 'blur'
    }
  ],
  vendorType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('vendorType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('vendorType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  enterpriseNature: [
    {
      required: true,
      message: pi.ph('enterpriseNature'),
      trigger: 'change'
    }
  ],
  industryAttribute: [
    {
      required: true,
      message: pi.ph('industryAttribute'),
      trigger: 'change'
    }
  ],
  taxRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  reconciliationAccount: [
    {
      required: true,
      message: pi.ph('reconciliationAccount'),
      trigger: 'change'
    }
  ],
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'change'
    }
  ],
  clearingWithCustomer: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clearingWithCustomer'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clearingWithCustomer'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paymentMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paymentMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paymentTerms: [
    {
      required: true,
      message: pi.ph('paymentTerms'),
      trigger: 'change'
    }
  ],
  bankCode: [
    {
      required: true,
      message: pi.ph('bankCode'),
      trigger: 'change'
    }
  ],
  bankAccount: [
    {
      required: true,
      message: pi.ph('bankAccount'),
      trigger: 'blur'
    }
  ],
  accountHolder: [
    {
      required: true,
      message: pi.ph('accountHolder'),
      trigger: 'blur'
    }
  ],
  grBasedInvoiceInspection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grBasedInvoiceInspection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  incoterms1: [
    {
      required: true,
      message: pi.ph('incoterms1'),
      trigger: 'change'
    }
  ],
  incoterms2: [
    {
      required: true,
      message: pi.ph('incoterms2'),
      trigger: 'blur'
    }
  ],
  automaticPurchaseOrder: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('automaticPurchaseOrder'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('automaticPurchaseOrder'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pricingDateControl: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('pricingDateControl'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseGroup: [
    {
      required: true,
      message: pi.ph('purchaseGroup'),
      trigger: 'change'
    }
  ],
  plannedDeliveryTimeDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedDeliveryTimeDays'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedDeliveryTimeDays'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluatedReceiptSettlement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluatedReceiptSettlement'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluatedReceiptSettlement'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchasingOrganization: [
    {
      required: true,
      message: pi.ph('purchasingOrganization'),
      trigger: 'change'
    }
  ],
  creditLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('creditLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('creditLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  creditAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('creditAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('creditAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  vendorLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('vendorLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('vendorLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  evaluationScore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('evaluationScore'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  vendorStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('vendorStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('vendorStatus'))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('vendorType' in payload) {
    const rawvendorType = payload.vendorType
    if (rawvendorType === undefined || rawvendorType === null || rawvendorType === '') {
      delete payload.vendorType
    } else {
      const numvendorType = typeof rawvendorType === 'number' ? rawvendorType : Number(rawvendorType)
      if (Number.isFinite(numvendorType)) payload.vendorType = numvendorType
      else delete payload.vendorType
    }
  }
  if ('taxRate' in payload) {
    const rawtaxRate = payload.taxRate
    if (rawtaxRate === undefined || rawtaxRate === null || rawtaxRate === '') {
      delete payload.taxRate
    } else {
      const numtaxRate = typeof rawtaxRate === 'number' ? rawtaxRate : Number(rawtaxRate)
      if (Number.isFinite(numtaxRate)) payload.taxRate = numtaxRate
      else delete payload.taxRate
    }
  }
  if ('clearingWithCustomer' in payload) {
    const rawclearingWithCustomer = payload.clearingWithCustomer
    if (rawclearingWithCustomer === undefined || rawclearingWithCustomer === null || rawclearingWithCustomer === '') {
      delete payload.clearingWithCustomer
    } else {
      const numclearingWithCustomer = typeof rawclearingWithCustomer === 'number' ? rawclearingWithCustomer : Number(rawclearingWithCustomer)
      if (Number.isFinite(numclearingWithCustomer)) payload.clearingWithCustomer = numclearingWithCustomer
      else delete payload.clearingWithCustomer
    }
  }
  if ('paymentMethod' in payload) {
    const rawpaymentMethod = payload.paymentMethod
    if (rawpaymentMethod === undefined || rawpaymentMethod === null || rawpaymentMethod === '') {
      delete payload.paymentMethod
    } else {
      const numpaymentMethod = typeof rawpaymentMethod === 'number' ? rawpaymentMethod : Number(rawpaymentMethod)
      if (Number.isFinite(numpaymentMethod)) payload.paymentMethod = numpaymentMethod
      else delete payload.paymentMethod
    }
  }
  if ('grBasedInvoiceInspection' in payload) {
    const rawgrBasedInvoiceInspection = payload.grBasedInvoiceInspection
    if (rawgrBasedInvoiceInspection === undefined || rawgrBasedInvoiceInspection === null || rawgrBasedInvoiceInspection === '') {
      delete payload.grBasedInvoiceInspection
    } else {
      const numgrBasedInvoiceInspection = typeof rawgrBasedInvoiceInspection === 'number' ? rawgrBasedInvoiceInspection : Number(rawgrBasedInvoiceInspection)
      if (Number.isFinite(numgrBasedInvoiceInspection)) payload.grBasedInvoiceInspection = numgrBasedInvoiceInspection
      else delete payload.grBasedInvoiceInspection
    }
  }
  if ('automaticPurchaseOrder' in payload) {
    const rawautomaticPurchaseOrder = payload.automaticPurchaseOrder
    if (rawautomaticPurchaseOrder === undefined || rawautomaticPurchaseOrder === null || rawautomaticPurchaseOrder === '') {
      delete payload.automaticPurchaseOrder
    } else {
      const numautomaticPurchaseOrder = typeof rawautomaticPurchaseOrder === 'number' ? rawautomaticPurchaseOrder : Number(rawautomaticPurchaseOrder)
      if (Number.isFinite(numautomaticPurchaseOrder)) payload.automaticPurchaseOrder = numautomaticPurchaseOrder
      else delete payload.automaticPurchaseOrder
    }
  }
  if ('pricingDateControl' in payload) {
    const rawpricingDateControl = payload.pricingDateControl
    if (rawpricingDateControl === undefined || rawpricingDateControl === null || rawpricingDateControl === '') {
      delete payload.pricingDateControl
    } else {
      const numpricingDateControl = typeof rawpricingDateControl === 'number' ? rawpricingDateControl : Number(rawpricingDateControl)
      if (Number.isFinite(numpricingDateControl)) payload.pricingDateControl = numpricingDateControl
      else delete payload.pricingDateControl
    }
  }
  if ('plannedDeliveryTimeDays' in payload) {
    const rawplannedDeliveryTimeDays = payload.plannedDeliveryTimeDays
    if (rawplannedDeliveryTimeDays === undefined || rawplannedDeliveryTimeDays === null || rawplannedDeliveryTimeDays === '') {
      delete payload.plannedDeliveryTimeDays
    } else {
      const numplannedDeliveryTimeDays = typeof rawplannedDeliveryTimeDays === 'number' ? rawplannedDeliveryTimeDays : Number(rawplannedDeliveryTimeDays)
      if (Number.isFinite(numplannedDeliveryTimeDays)) payload.plannedDeliveryTimeDays = numplannedDeliveryTimeDays
      else delete payload.plannedDeliveryTimeDays
    }
  }
  if ('evaluatedReceiptSettlement' in payload) {
    const rawevaluatedReceiptSettlement = payload.evaluatedReceiptSettlement
    if (rawevaluatedReceiptSettlement === undefined || rawevaluatedReceiptSettlement === null || rawevaluatedReceiptSettlement === '') {
      delete payload.evaluatedReceiptSettlement
    } else {
      const numevaluatedReceiptSettlement = typeof rawevaluatedReceiptSettlement === 'number' ? rawevaluatedReceiptSettlement : Number(rawevaluatedReceiptSettlement)
      if (Number.isFinite(numevaluatedReceiptSettlement)) payload.evaluatedReceiptSettlement = numevaluatedReceiptSettlement
      else delete payload.evaluatedReceiptSettlement
    }
  }
  if ('creditLevel' in payload) {
    const rawcreditLevel = payload.creditLevel
    if (rawcreditLevel === undefined || rawcreditLevel === null || rawcreditLevel === '') {
      delete payload.creditLevel
    } else {
      const numcreditLevel = typeof rawcreditLevel === 'number' ? rawcreditLevel : Number(rawcreditLevel)
      if (Number.isFinite(numcreditLevel)) payload.creditLevel = numcreditLevel
      else delete payload.creditLevel
    }
  }
  if ('creditAmount' in payload) {
    const rawcreditAmount = payload.creditAmount
    if (rawcreditAmount === undefined || rawcreditAmount === null || rawcreditAmount === '') {
      delete payload.creditAmount
    } else {
      const numcreditAmount = typeof rawcreditAmount === 'number' ? rawcreditAmount : Number(rawcreditAmount)
      if (Number.isFinite(numcreditAmount)) payload.creditAmount = numcreditAmount
      else delete payload.creditAmount
    }
  }
  if ('vendorLevel' in payload) {
    const rawvendorLevel = payload.vendorLevel
    if (rawvendorLevel === undefined || rawvendorLevel === null || rawvendorLevel === '') {
      delete payload.vendorLevel
    } else {
      const numvendorLevel = typeof rawvendorLevel === 'number' ? rawvendorLevel : Number(rawvendorLevel)
      if (Number.isFinite(numvendorLevel)) payload.vendorLevel = numvendorLevel
      else delete payload.vendorLevel
    }
  }
  if ('evaluationScore' in payload) {
    const rawevaluationScore = payload.evaluationScore
    if (rawevaluationScore === undefined || rawevaluationScore === null || rawevaluationScore === '') {
      delete payload.evaluationScore
    } else {
      const numevaluationScore = typeof rawevaluationScore === 'number' ? rawevaluationScore : Number(rawevaluationScore)
      if (Number.isFinite(numevaluationScore)) payload.evaluationScore = numevaluationScore
      else delete payload.evaluationScore
    }
  }
  if ('vendorStatus' in payload) {
    const rawvendorStatus = payload.vendorStatus
    if (rawvendorStatus === undefined || rawvendorStatus === null || rawvendorStatus === '') {
      delete payload.vendorStatus
    } else {
      const numvendorStatus = typeof rawvendorStatus === 'number' ? rawvendorStatus : Number(rawvendorStatus)
      if (Number.isFinite(numvendorStatus)) payload.vendorStatus = numvendorStatus
      else delete payload.vendorStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.vendorId) {
    payload.vendorId = props.formData.vendorId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.vendorId)

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

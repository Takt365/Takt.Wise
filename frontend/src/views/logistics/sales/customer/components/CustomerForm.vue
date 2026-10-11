<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/customer/components -->
<!-- 文件名称：CustomerForm.vue -->
<!-- 功能描述：Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+CustomerCode维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="customer-form-tabs"
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
                :label="pi.label('customerCode')"
                name="customerCode"
              >
                <a-input
                  v-model:value="formState.customerCode"
                  :placeholder="pi.ph('customerCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerName1')"
                name="customerName1"
              >
                <a-input
                  v-model:value="formState.customerName1"
                  :placeholder="pi.ph('customerName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerName2')"
                name="customerName2"
              >
                <a-input
                  v-model:value="formState.customerName2"
                  :placeholder="pi.ph('customerName2')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerShortName')"
                name="customerShortName"
              >
                <a-input
                  v-model:value="formState.customerShortName"
                  :placeholder="pi.ph('customerShortName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerType')"
                name="customerType"
              >
                <TaktSelect
                  v-model:value="formState.customerType"
                  dict-type="logistics_sales_customer_category"
                  :placeholder="pi.ph('customerType')"
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
                :label="pi.label('customerTaxNumber')"
                name="customerTaxNumber"
              >
                <a-input
                  v-model:value="formState.customerTaxNumber"
                  :placeholder="pi.ph('customerTaxNumber')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/7)'"
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
                  :disabled="!!formData?.customerId"
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
                :label="pi.label('customerPhone')"
                name="customerPhone"
              >
                <a-input
                  v-model:value="formState.customerPhone"
                  :placeholder="pi.ph('customerPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerFax')"
                name="customerFax"
              >
                <a-input
                  v-model:value="formState.customerFax"
                  :placeholder="pi.ph('customerFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerEmail')"
                name="customerEmail"
              >
                <a-input
                  v-model:value="formState.customerEmail"
                  :placeholder="pi.ph('customerEmail')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerWebsite')"
                name="customerWebsite"
              >
                <a-input
                  v-model:value="formState.customerWebsite"
                  :placeholder="pi.ph('customerWebsite')"
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
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesOrganization')"
                name="salesOrganization"
              >
                <TaktSelect
                  v-model:value="formState.salesOrganization"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('salesOrganization')"
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
                :label="pi.label('productGroup')"
                name="productGroup"
              >
                <a-input
                  v-model:value="formState.productGroup"
                  :placeholder="pi.ph('productGroup')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerGroup')"
                name="customerGroup"
              >
                <TaktSelect
                  v-model:value="formState.customerGroup"
                  dict-type="logistics_sales_customer_group"
                  :placeholder="pi.ph('customerGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tradingPartner')"
                name="tradingPartner"
              >
                <TaktSelect
                  v-model:value="formState.tradingPartner"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('tradingPartner')"
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
                :label="pi.label('accountAssignmentGroup')"
                name="accountAssignmentGroup"
              >
                <TaktSelect
                  v-model:value="formState.accountAssignmentGroup"
                  dict-type="logistics_sales_account_assignment_group"
                  :placeholder="pi.ph('accountAssignmentGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.customerId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('nielsenIndicator')"
                name="nielsenIndicator"
              >
                <a-input
                  v-model:value="formState.nielsenIndicator"
                  :placeholder="pi.ph('nielsenIndicator')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('centralPostingBlock')"
                name="centralPostingBlock"
              >
                <TaktSelect
                  v-model:value="formState.centralPostingBlock"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('centralPostingBlock')"
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
                  :api-params="{ reconciliationOnly: true, auxiliaryType: 'D' }"
                  :placeholder="pi.ph('reconciliationAccount')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('headquarters')"
                name="headquarters"
              >
                <TaktSelect
                  v-model:value="formState.headquarters"
                  api-url="TaktCustomers/options"
                  :placeholder="pi.ph('headquarters')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clearingWithVendor')"
                name="clearingWithVendor"
              >
                <TaktSelect
                  v-model:value="formState.clearingWithVendor"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('clearingWithVendor')"
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
                :label="pi.label('deliveringPlant')"
                name="deliveringPlant"
              >
                <TaktSelect
                  v-model:value="formState.deliveringPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('deliveringPlant')"
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
                :label="pi.label('customerPricingProcedure')"
                name="customerPricingProcedure"
              >
                <TaktSelect
                  v-model:value="formState.customerPricingProcedure"
                  dict-type="logistics_sales_customer_pricing_procedure"
                  :placeholder="pi.ph('customerPricingProcedure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountRate')"
                name="discountRate"
              >
                <TaktSelect
                  v-model:value="formState.discountRate"
                  dict-type="logistics_sales_discount_rate_param"
                  :placeholder="pi.ph('discountRate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('customerLevel')"
                name="customerLevel"
              >
                <TaktSelect
                  v-model:value="formState.customerLevel"
                  dict-type="logistics_sales_customer_level"
                  :placeholder="pi.ph('customerLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('customerStatus')"
                name="customerStatus"
              >
                <TaktSelect
                  v-model:value="formState.customerStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('customerStatus')"
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
 * Takt客户信息实体 <para>业务唯一键：TenantCode+CompanyCode+CustomerCode维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/customer/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerI18n } from '../composables/useCustomerI18n'

/** 实体字段 i18n */
const pi = useCustomerI18n()
import type { CustomerCreate } from '@/types/logistics/sales/customer.types'
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
  formData?: Partial<CustomerCreate & { customerId?: string }> | null
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
  customerType: 0,
  enterpriseNature: "150",
  industryAttribute: "C",
  taxCode: "J2",
  registrationCountry: "CN",
  currencyCode: "CNY",
  customerGroup: "Z1",
  accountAssignmentGroup: "01",
  paymentTerms: "PREPAYSHIP",
  paymentMethod: 0,
  incoterms1: "FOB",
  shippingConditions: "Z1",
  customerPricingProcedure: "1",
  creditLevel: 0,
  discountRate: 0,
  customerLevel: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerId) {
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
    if (!props.formData?.customerId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  customerCode: [
    {
      required: true,
      message: pi.ph('customerCode'),
      trigger: 'blur'
    }
  ],
  customerName1: [
    {
      required: true,
      message: pi.ph('customerName1'),
      trigger: 'blur'
    }
  ],
  customerType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerType'))
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
  salesOrganization: [
    {
      required: true,
      message: pi.ph('salesOrganization'),
      trigger: 'change'
    }
  ],
  distributionChannel: [
    {
      required: true,
      message: pi.ph('distributionChannel'),
      trigger: 'blur'
    }
  ],
  productGroup: [
    {
      required: true,
      message: pi.ph('productGroup'),
      trigger: 'blur'
    }
  ],
  customerGroup: [
    {
      required: true,
      message: pi.ph('customerGroup'),
      trigger: 'change'
    }
  ],
  tradingPartner: [
    {
      required: true,
      message: pi.ph('tradingPartner'),
      trigger: 'change'
    }
  ],
  accountAssignmentGroup: [
    {
      required: true,
      message: pi.ph('accountAssignmentGroup'),
      trigger: 'change'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  nielsenIndicator: [
    {
      required: true,
      message: pi.ph('nielsenIndicator'),
      trigger: 'blur'
    }
  ],
  centralPostingBlock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('centralPostingBlock'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('centralPostingBlock'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  reconciliationAccount: [
    {
      required: true,
      message: pi.ph('reconciliationAccount'),
      trigger: 'change'
    }
  ],
  headquarters: [
    {
      required: true,
      message: pi.ph('headquarters'),
      trigger: 'change'
    }
  ],
  clearingWithVendor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clearingWithVendor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clearingWithVendor'))
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
  deliveringPlant: [
    {
      required: true,
      message: pi.ph('deliveringPlant'),
      trigger: 'change'
    }
  ],
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
  shippingConditions: [
    {
      required: true,
      message: pi.ph('shippingConditions'),
      trigger: 'change'
    }
  ],
  customerPricingProcedure: [
    {
      required: true,
      message: pi.ph('customerPricingProcedure'),
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
  discountRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  customerLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerLevel'))
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
  customerStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('customerStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('customerStatus'))
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
  if ('customerType' in payload) {
    const rawcustomerType = payload.customerType
    if (rawcustomerType === undefined || rawcustomerType === null || rawcustomerType === '') {
      delete payload.customerType
    } else {
      const numcustomerType = typeof rawcustomerType === 'number' ? rawcustomerType : Number(rawcustomerType)
      if (Number.isFinite(numcustomerType)) payload.customerType = numcustomerType
      else delete payload.customerType
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
  if ('centralPostingBlock' in payload) {
    const rawcentralPostingBlock = payload.centralPostingBlock
    if (rawcentralPostingBlock === undefined || rawcentralPostingBlock === null || rawcentralPostingBlock === '') {
      delete payload.centralPostingBlock
    } else {
      const numcentralPostingBlock = typeof rawcentralPostingBlock === 'number' ? rawcentralPostingBlock : Number(rawcentralPostingBlock)
      if (Number.isFinite(numcentralPostingBlock)) payload.centralPostingBlock = numcentralPostingBlock
      else delete payload.centralPostingBlock
    }
  }
  if ('clearingWithVendor' in payload) {
    const rawclearingWithVendor = payload.clearingWithVendor
    if (rawclearingWithVendor === undefined || rawclearingWithVendor === null || rawclearingWithVendor === '') {
      delete payload.clearingWithVendor
    } else {
      const numclearingWithVendor = typeof rawclearingWithVendor === 'number' ? rawclearingWithVendor : Number(rawclearingWithVendor)
      if (Number.isFinite(numclearingWithVendor)) payload.clearingWithVendor = numclearingWithVendor
      else delete payload.clearingWithVendor
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
  if ('discountRate' in payload) {
    const rawdiscountRate = payload.discountRate
    if (rawdiscountRate === undefined || rawdiscountRate === null || rawdiscountRate === '') {
      delete payload.discountRate
    } else {
      const numdiscountRate = typeof rawdiscountRate === 'number' ? rawdiscountRate : Number(rawdiscountRate)
      if (Number.isFinite(numdiscountRate)) payload.discountRate = numdiscountRate
      else delete payload.discountRate
    }
  }
  if ('customerLevel' in payload) {
    const rawcustomerLevel = payload.customerLevel
    if (rawcustomerLevel === undefined || rawcustomerLevel === null || rawcustomerLevel === '') {
      delete payload.customerLevel
    } else {
      const numcustomerLevel = typeof rawcustomerLevel === 'number' ? rawcustomerLevel : Number(rawcustomerLevel)
      if (Number.isFinite(numcustomerLevel)) payload.customerLevel = numcustomerLevel
      else delete payload.customerLevel
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
  if ('customerStatus' in payload) {
    const rawcustomerStatus = payload.customerStatus
    if (rawcustomerStatus === undefined || rawcustomerStatus === null || rawcustomerStatus === '') {
      delete payload.customerStatus
    } else {
      const numcustomerStatus = typeof rawcustomerStatus === 'number' ? rawcustomerStatus : Number(rawcustomerStatus)
      if (Number.isFinite(numcustomerStatus)) payload.customerStatus = numcustomerStatus
      else delete payload.customerStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.customerId) {
    payload.customerId = props.formData.customerId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerId)

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

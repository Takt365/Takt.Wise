<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/vendor/components -->
<!-- 文件名称：VendorForm.vue -->
<!-- 功能描述：Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/8)'"
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
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountGroup')"
                name="accountGroup"
              >
                <a-input
                  v-model:value="formState.accountGroup"
                  :placeholder="pi.ph('accountGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorTitle')"
                name="vendorTitle"
              >
                <TaktSelect
                  v-model:value="formState.vendorTitle"
                  dict-type="logistics_sales_customer_title"
                  :placeholder="pi.ph('vendorTitle')"
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
                :label="pi.label('vendorName3')"
                name="vendorName3"
              >
                <a-input
                  v-model:value="formState.vendorName3"
                  :placeholder="pi.ph('vendorName3')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('vendorName4')"
                name="vendorName4"
              >
                <a-input
                  v-model:value="formState.vendorName4"
                  :placeholder="pi.ph('vendorName4')"
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/8)'"
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
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('regionCode')"
                name="regionCode"
              >
                <TaktSelect
                  v-model:value="formState.regionCode"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 2 }"
                  :placeholder="pi.ph('regionCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cityCode')"
                name="cityCode"
              >
                <TaktSelect
                  v-model:value="formState.cityCode"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 3 }"
                  :placeholder="pi.ph('cityCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('addr1')"
                name="addr1"
              >
                <a-input
                  v-model:value="formState.addr1"
                  :placeholder="pi.ph('addr1')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('addr2')"
                name="addr2"
              >
                <a-input
                  v-model:value="formState.addr2"
                  :placeholder="pi.ph('addr2')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postalCode')"
                name="postalCode"
              >
                <a-input
                  v-model:value="formState.postalCode"
                  :placeholder="pi.ph('postalCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('phone1')"
                name="phone1"
              >
                <a-input
                  v-model:value="formState.phone1"
                  :placeholder="pi.ph('phone1')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('phone2')"
                name="phone2"
              >
                <a-input
                  v-model:value="formState.phone2"
                  :placeholder="pi.ph('phone2')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('faxNumber')"
                name="faxNumber"
              >
                <a-input
                  v-model:value="formState.faxNumber"
                  :placeholder="pi.ph('faxNumber')"
                  show-count
                  :maxlength="40"
                  allow-clear
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/8)'"
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
                :label="pi.label('authorizationGroup')"
                name="authorizationGroup"
              >
                <a-input
                  v-model:value="formState.authorizationGroup"
                  :placeholder="pi.ph('authorizationGroup')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('groupKey')"
                name="groupKey"
              >
                <a-input
                  v-model:value="formState.groupKey"
                  :placeholder="pi.ph('groupKey')"
                  show-count
                  :maxlength="10"
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
                  dict-type="logistics_sales_customer_category"
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
                :label="pi.label('creditInfoNumber')"
                name="creditInfoNumber"
              >
                <a-input
                  v-model:value="formState.creditInfoNumber"
                  :placeholder="pi.ph('creditInfoNumber')"
                  show-count
                  :maxlength="11"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankCountryCode')"
                name="bankCountryCode"
              >
                <TaktSelect
                  v-model:value="formState.bankCountryCode"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('bankCountryCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/8)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountHolder')"
                name="accountHolder"
              >
                <a-input
                  v-model:value="formState.accountHolder"
                  :placeholder="pi.ph('accountHolder')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankChargeBearerCode')"
                name="bankChargeBearerCode"
              >
                <a-input
                  v-model:value="formState.bankChargeBearerCode"
                  :placeholder="pi.ph('bankChargeBearerCode')"
                  show-count
                  :maxlength="2"
                  allow-clear
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
                :label="pi.label('headOfficeAccountCode')"
                name="headOfficeAccountCode"
              >
                <a-input
                  v-model:value="formState.headOfficeAccountCode"
                  :placeholder="pi.ph('headOfficeAccountCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planningGroup')"
                name="planningGroup"
              >
                <a-input
                  v-model:value="formState.planningGroup"
                  :placeholder="pi.ph('planningGroup')"
                  show-count
                  :maxlength="10"
                  allow-clear
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
                :label="pi.label('doubleInvoiceCheck')"
                name="doubleInvoiceCheck"
              >
                <TaktSelect
                  v-model:value="formState.doubleInvoiceCheck"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('doubleInvoiceCheck')"
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
                :label="pi.label('paymentBlock')"
                name="paymentBlock"
              >
                <TaktSelect
                  v-model:value="formState.paymentBlock"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('paymentBlock')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('houseBank')"
                name="houseBank"
              >
                <TaktSelect
                  v-model:value="formState.houseBank"
                  api-url="TaktBanks/options"
                  :placeholder="pi.ph('houseBank')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/8)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
                :label="pi.label('accountingClerk')"
                name="accountingClerk"
              >
                <a-input
                  v-model:value="formState.accountingClerk"
                  :placeholder="pi.ph('accountingClerk')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ourAccountAtVendor')"
                name="ourAccountAtVendor"
              >
                <a-input
                  v-model:value="formState.ourAccountAtVendor"
                  :placeholder="pi.ph('ourAccountAtVendor')"
                  show-count
                  :maxlength="12"
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
                  :maxlength="80"
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
                  :maxlength="40"
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
                :label="pi.label('withholdingTaxCountry')"
                name="withholdingTaxCountry"
              >
                <TaktSelect
                  v-model:value="formState.withholdingTaxCountry"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('withholdingTaxCountry')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('withholdingTaxType')"
                name="withholdingTaxType"
              >
                <TaktSelect
                  v-model:value="formState.withholdingTaxType"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('withholdingTaxType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('withholdingTaxCode')"
                name="withholdingTaxCode"
              >
                <TaktSelect
                  v-model:value="formState.withholdingTaxCode"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('withholdingTaxCode')"
                  :disabled="!!formData?.vendorId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-5"
        :tab="t('common.page.form.tabs.basicinfo') + ' (6/8)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderCurrencyCode')"
                name="orderCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.orderCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('orderCurrencyCode')"
                  :disabled="!!formData?.vendorId"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('returnErsFlag')"
                name="returnErsFlag"
              >
                <TaktSelect
                  v-model:value="formState.returnErsFlag"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('returnErsFlag')"
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
                :label="pi.label('taxNumber1')"
                name="taxNumber1"
              >
                <a-input
                  v-model:value="formState.taxNumber1"
                  :placeholder="pi.ph('taxNumber1')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-6"
        :tab="t('common.page.form.tabs.basicinfo') + ' (7/8)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxNumber2')"
                name="taxNumber2"
              >
                <a-input
                  v-model:value="formState.taxNumber2"
                  :placeholder="pi.ph('taxNumber2')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('taxNumber3')"
                name="taxNumber3"
              >
                <a-input
                  v-model:value="formState.taxNumber3"
                  :placeholder="pi.ph('taxNumber3')"
                  show-count
                  :maxlength="60"
                  allow-clear
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
                :label="pi.label('vendorLevel')"
                name="vendorLevel"
              >
                <TaktSelect
                  v-model:value="formState.vendorLevel"
                  dict-type="logistics_sales_customer_level"
                  :placeholder="pi.ph('vendorLevel')"
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
        key="tab-7"
        :tab="t('common.page.form.tabs.basicinfo') + ' (8/8)'"
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
 * Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
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
  vendorTitle: 1,
  countryCode: "CN",
  vendorType: 0,
  enterpriseNature: "150",
  industryAttribute: "C",
  bankCountryCode: "CN",
  paymentTerms: "PREPAYSHIP",
  paymentMethod: 0,
  withholdingTaxCountry: "CN",
  orderCurrencyCode: "CNY",
  incoterms1: "FOB",
  pricingDateControl: 1,
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
  accountGroup: [
    {
      required: true,
      message: pi.ph('accountGroup'),
      trigger: 'blur'
    }
  ],
  vendorTitle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('vendorTitle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('vendorTitle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  reconciliationAccount: [
    {
      required: true,
      message: pi.ph('reconciliationAccount'),
      trigger: 'change'
    }
  ],
  paymentTerms: [
    {
      required: true,
      message: pi.ph('paymentTerms'),
      trigger: 'change'
    }
  ],
  doubleInvoiceCheck: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('doubleInvoiceCheck'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('doubleInvoiceCheck'))
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
  paymentBlock: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paymentBlock'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paymentBlock'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  purchasingOrganization: [
    {
      required: true,
      message: pi.ph('purchasingOrganization'),
      trigger: 'change'
    }
  ],
  orderCurrencyCode: [
    {
      required: true,
      message: pi.ph('orderCurrencyCode'),
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
  returnErsFlag: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('returnErsFlag'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('returnErsFlag'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
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
  purchaseGroup: [
    {
      required: true,
      message: pi.ph('purchaseGroup'),
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
  if ('vendorTitle' in payload) {
    const rawvendorTitle = payload.vendorTitle
    if (rawvendorTitle === undefined || rawvendorTitle === null || rawvendorTitle === '') {
      delete payload.vendorTitle
    } else {
      const numvendorTitle = typeof rawvendorTitle === 'number' ? rawvendorTitle : Number(rawvendorTitle)
      if (Number.isFinite(numvendorTitle)) payload.vendorTitle = numvendorTitle
      else delete payload.vendorTitle
    }
  }
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
  if ('doubleInvoiceCheck' in payload) {
    const rawdoubleInvoiceCheck = payload.doubleInvoiceCheck
    if (rawdoubleInvoiceCheck === undefined || rawdoubleInvoiceCheck === null || rawdoubleInvoiceCheck === '') {
      delete payload.doubleInvoiceCheck
    } else {
      const numdoubleInvoiceCheck = typeof rawdoubleInvoiceCheck === 'number' ? rawdoubleInvoiceCheck : Number(rawdoubleInvoiceCheck)
      if (Number.isFinite(numdoubleInvoiceCheck)) payload.doubleInvoiceCheck = numdoubleInvoiceCheck
      else delete payload.doubleInvoiceCheck
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
  if ('paymentBlock' in payload) {
    const rawpaymentBlock = payload.paymentBlock
    if (rawpaymentBlock === undefined || rawpaymentBlock === null || rawpaymentBlock === '') {
      delete payload.paymentBlock
    } else {
      const numpaymentBlock = typeof rawpaymentBlock === 'number' ? rawpaymentBlock : Number(rawpaymentBlock)
      if (Number.isFinite(numpaymentBlock)) payload.paymentBlock = numpaymentBlock
      else delete payload.paymentBlock
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
  if ('returnErsFlag' in payload) {
    const rawreturnErsFlag = payload.returnErsFlag
    if (rawreturnErsFlag === undefined || rawreturnErsFlag === null || rawreturnErsFlag === '') {
      delete payload.returnErsFlag
    } else {
      const numreturnErsFlag = typeof rawreturnErsFlag === 'number' ? rawreturnErsFlag : Number(rawreturnErsFlag)
      if (Number.isFinite(numreturnErsFlag)) payload.returnErsFlag = numreturnErsFlag
      else delete payload.returnErsFlag
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

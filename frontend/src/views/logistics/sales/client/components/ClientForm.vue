<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/client/components -->
<!-- 文件名称：ClientForm.vue -->
<!-- 功能描述：Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="client-form-tabs"
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
                :label="t('common.page.form.numberingRule')"
                name="numberingRuleCode"
              >
                <TaktSelect
                  v-model:value="formState.numberingRuleCode"
                  api-url="TaktNumberings/options"
                  :api-params="{ documentType: '顾客信息' }"
                  :placeholder="t('common.page.form.placeholder.selectonly')"
                  :disabled="!!formData?.clientId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientCode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="t('common.page.form.numberingCodePreview')"
                  disabled
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
                :label="pi.label('clientTitle')"
                name="clientTitle"
              >
                <TaktSelect
                  v-model:value="formState.clientTitle"
                  dict-type="logistics_sales_client_title"
                  :placeholder="pi.ph('clientTitle')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientName1')"
                name="clientName1"
              >
                <a-input
                  v-model:value="formState.clientName1"
                  :placeholder="pi.ph('clientName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientName2')"
                name="clientName2"
              >
                <a-input
                  v-model:value="formState.clientName2"
                  :placeholder="pi.ph('clientName2')"
                  show-count
                  :maxlength="140"
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
                :label="pi.label('clientName3')"
                name="clientName3"
              >
                <a-input
                  v-model:value="formState.clientName3"
                  :placeholder="pi.ph('clientName3')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientName4')"
                name="clientName4"
              >
                <a-input
                  v-model:value="formState.clientName4"
                  :placeholder="pi.ph('clientName4')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientShortName')"
                name="clientShortName"
              >
                <a-input
                  v-model:value="formState.clientShortName"
                  :placeholder="pi.ph('clientShortName')"
                  show-count
                  :maxlength="40"
                  allow-clear
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
                :label="pi.label('registrationRegion')"
                name="registrationRegion"
              >
                <TaktSelect
                  v-model:value="formState.registrationRegion"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 2 }"
                  :placeholder="pi.ph('registrationRegion')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('addr1')"
                name="addr1"
              >
                <a-input
                  v-model:value="formState.addr1"
                  :placeholder="pi.ph('addr1')"
                  show-count
                  :maxlength="140"
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
                :label="pi.label('addr3')"
                name="addr3"
              >
                <a-input
                  v-model:value="formState.addr3"
                  :placeholder="pi.ph('addr3')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('addr4')"
                name="addr4"
              >
                <a-input
                  v-model:value="formState.addr4"
                  :placeholder="pi.ph('addr4')"
                  show-count
                  :maxlength="140"
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
                :label="pi.label('addr5')"
                name="addr5"
              >
                <a-input
                  v-model:value="formState.addr5"
                  :placeholder="pi.ph('addr5')"
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
                  :disabled="!!formData?.clientId"
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
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.clientId"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
                :label="pi.label('bankCountryCode')"
                name="bankCountryCode"
              >
                <TaktSelect
                  v-model:value="formState.bankCountryCode"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('bankCountryCode')"
                  :disabled="!!formData?.clientId"
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
                  allow-clear
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
                :label="pi.label('bankAccountHolder')"
                name="bankAccountHolder"
              >
                <a-input
                  v-model:value="formState.bankAccountHolder"
                  :placeholder="pi.ph('bankAccountHolder')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientCategory')"
                name="clientCategory"
              >
                <TaktSelect
                  v-model:value="formState.clientCategory"
                  dict-type="logistics_sales_client_category"
                  :placeholder="pi.ph('clientCategory')"
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
                :label="pi.label('salesDistrict')"
                name="salesDistrict"
              >
                <a-input
                  v-model:value="formState.salesDistrict"
                  :placeholder="pi.ph('salesDistrict')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/7)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesDepartment')"
                name="salesDepartment"
              >
                <TaktSelect
                  v-model:value="formState.salesDepartment"
                  api-url="TaktDepts/options"
                  :placeholder="pi.ph('salesDepartment')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('salesGroup')"
                name="salesGroup"
              >
                <TaktSelect
                  v-model:value="formState.salesGroup"
                  api-url="TaktSalesGroups/options"
                  :placeholder="pi.ph('salesGroup')"
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
                  :disabled="!!formData?.clientId"
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
                :label="pi.label('taxClassification')"
                name="taxClassification"
              >
                <TaktSelect
                  v-model:value="formState.taxClassification"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('taxClassification')"
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
                :label="pi.label('clientContact')"
                name="clientContact"
              >
                <a-input
                  v-model:value="formState.clientContact"
                  :placeholder="pi.ph('clientContact')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('clientPhone')"
                name="clientPhone"
              >
                <a-input
                  v-model:value="formState.clientPhone"
                  :placeholder="pi.ph('clientPhone')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('clientEmail')"
                name="clientEmail"
              >
                <a-input
                  v-model:value="formState.clientEmail"
                  :placeholder="pi.ph('clientEmail')"
                  show-count
                  :maxlength="100"
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
                :label="pi.label('clientLevel')"
                name="clientLevel"
              >
                <TaktSelect
                  v-model:value="formState.clientLevel"
                  dict-type="logistics_sales_client_level"
                  :placeholder="pi.ph('clientLevel')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('clientStatus')"
                name="clientStatus"
              >
                <TaktSelect
                  v-model:value="formState.clientStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('clientStatus')"
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
 * Takt客户端信息实体 <para>业务唯一键：TenantCode+CompanyCode+SalesOrganization+ClientCode维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/client/components
 */

import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useClientI18n } from '../composables/useClientI18n'
import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'

/** 实体字段 i18n */
const pi = useClientI18n()
import type { ClientCreate } from '@/types/logistics/sales/client.types'
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
  formData?: Partial<ClientCreate & { clientId?: string }> | null
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
  registrationCountry: "CN",
  bankCountryCode: "CN",
  clientCategory: 0,
  enterpriseNature: "150",
  industryAttribute: "C",
  paymentTerms: "PREPAYSHIP",
  paymentMethod: 0,
  currencyCode: "CNY",
  customerPricingProcedure: "1",
  shippingConditions: "Z1",
  incoterms1: "FOB",
  accountAssignmentGroup: "01",
  creditLevel: 0
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



/** 编辑态灌入 formData；新增态恢复默认值（须含 clientId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.clientId) {
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
    if (!props.formData?.clientId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 是否编辑态 */
const isEditMode = computed(() => !!props.formData?.clientId)

useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: 'clientCode',
})

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  clientCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],
  accountGroup: [
    {
      required: true,
      message: pi.ph('accountGroup'),
      trigger: 'blur'
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
  clientTitle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clientTitle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clientTitle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  clientName1: [
    {
      required: true,
      message: pi.ph('clientName1'),
      trigger: 'blur'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
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
  clientCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clientCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clientCategory'))
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
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
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
  shippingConditions: [
    {
      required: true,
      message: pi.ph('shippingConditions'),
      trigger: 'change'
    }
  ],
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
  accountAssignmentGroup: [
    {
      required: true,
      message: pi.ph('accountAssignmentGroup'),
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
  clientLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clientLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clientLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  clientStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('clientStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('clientStatus'))
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
  if ('clientTitle' in payload) {
    const rawclientTitle = payload.clientTitle
    if (rawclientTitle === undefined || rawclientTitle === null || rawclientTitle === '') {
      delete payload.clientTitle
    } else {
      const numclientTitle = typeof rawclientTitle === 'number' ? rawclientTitle : Number(rawclientTitle)
      if (Number.isFinite(numclientTitle)) payload.clientTitle = numclientTitle
      else delete payload.clientTitle
    }
  }
  if ('clientCategory' in payload) {
    const rawclientCategory = payload.clientCategory
    if (rawclientCategory === undefined || rawclientCategory === null || rawclientCategory === '') {
      delete payload.clientCategory
    } else {
      const numclientCategory = typeof rawclientCategory === 'number' ? rawclientCategory : Number(rawclientCategory)
      if (Number.isFinite(numclientCategory)) payload.clientCategory = numclientCategory
      else delete payload.clientCategory
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
  if ('clientLevel' in payload) {
    const rawclientLevel = payload.clientLevel
    if (rawclientLevel === undefined || rawclientLevel === null || rawclientLevel === '') {
      delete payload.clientLevel
    } else {
      const numclientLevel = typeof rawclientLevel === 'number' ? rawclientLevel : Number(rawclientLevel)
      if (Number.isFinite(numclientLevel)) payload.clientLevel = numclientLevel
      else delete payload.clientLevel
    }
  }
  if ('clientStatus' in payload) {
    const rawclientStatus = payload.clientStatus
    if (rawclientStatus === undefined || rawclientStatus === null || rawclientStatus === '') {
      delete payload.clientStatus
    } else {
      const numclientStatus = typeof rawclientStatus === 'number' ? rawclientStatus : Number(rawclientStatus)
      if (Number.isFinite(numclientStatus)) payload.clientStatus = numclientStatus
      else delete payload.clientStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.clientId) {
    payload.clientId = props.formData.clientId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.clientId)

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

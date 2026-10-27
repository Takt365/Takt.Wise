<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/vendor -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:procurement:vendor:create"
      update-permission="logistics:procurement:vendor:update"
      delete-permission="logistics:procurement:vendor:delete"
      import-permission="logistics:procurement:vendor:import"
      export-permission="logistics:procurement:vendor:export"
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

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'vendorId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getVendorId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'vendorStatus'">
          <a-switch
            :checked="getVendorDictValue(record, 'vendorStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleVendorStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'vendorTitle'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'vendorTitle')"
            dict-type="logistics_sales_customer_title"
          />
        </template>
        <template v-else-if="column.key === 'countryCode'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'countryCode')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'vendorType'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'vendorType')"
            dict-type="logistics_sales_customer_category"
          />
        </template>
        <template v-else-if="column.key === 'enterpriseNature'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'enterpriseNature')"
            dict-type="sys_enterprise_nature"
          />
        </template>
        <template v-else-if="column.key === 'industryAttribute'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'industryAttribute')"
            dict-type="sys_industry_attribute"
          />
        </template>
        <template v-else-if="column.key === 'bankCountryCode'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'bankCountryCode')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'paymentTerms'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'paymentTerms')"
            dict-type="accounting_financial_payment_terms_param"
          />
        </template>
        <template v-else-if="column.key === 'doubleInvoiceCheck'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'doubleInvoiceCheck')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'paymentMethod'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'paymentMethod')"
            dict-type="accounting_financial_payment_method"
          />
        </template>
        <template v-else-if="column.key === 'paymentBlock'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'paymentBlock')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'clearingWithCustomer'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'clearingWithCustomer')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'withholdingTaxCountry'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'withholdingTaxCountry')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'orderCurrencyCode'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'orderCurrencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'incoterms1'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'incoterms1')"
            dict-type="logistics_sales_incoterms1"
          />
        </template>
        <template v-else-if="column.key === 'pricingDateControl'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'pricingDateControl')"
            dict-type="logistics_procurement_pricing_date_control"
          />
        </template>
        <template v-else-if="column.key === 'grBasedInvoiceInspection'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'grBasedInvoiceInspection')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'evaluatedReceiptSettlement'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'evaluatedReceiptSettlement')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'returnErsFlag'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'returnErsFlag')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'automaticPurchaseOrder'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'automaticPurchaseOrder')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'creditLevel'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'creditLevel')"
            dict-type="logistics_sales_credit_rating"
          />
        </template>
        <template v-else-if="column.key === 'vendorLevel'">
          <TaktDictTag
            :value="getVendorDictValue(record, 'vendorLevel')"
            dict-type="logistics_sales_customer_level"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

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
      <VendorForm
        :key="formData?.vendorId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-procurement-vendor'"
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
      <div v-show="isFieldVisible('vendorCode')">
      <a-form-item :label="pi.queryLabel('vendorCode')">
        <a-input
          v-model:value="advancedQueryForm.vendorCode"
          :placeholder="pi.queryPh('vendorCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountGroup')">
      <a-form-item :label="pi.queryLabel('accountGroup')">
        <a-input
          v-model:value="advancedQueryForm.accountGroup"
          :placeholder="pi.queryPh('accountGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorTitle')">
      <a-form-item :label="pi.queryLabel('vendorTitle')">
        <TaktSelect
          v-model:value="advancedQueryForm.vendorTitle"
          dict-type="logistics_sales_customer_title"
          :placeholder="pi.queryPh('vendorTitle', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorName1')">
      <a-form-item :label="pi.queryLabel('vendorName1')">
        <a-input
          v-model:value="advancedQueryForm.vendorName1"
          :placeholder="pi.queryPh('vendorName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorName2')">
      <a-form-item :label="pi.queryLabel('vendorName2')">
        <a-input
          v-model:value="advancedQueryForm.vendorName2"
          :placeholder="pi.queryPh('vendorName2', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorName3')">
      <a-form-item :label="pi.queryLabel('vendorName3')">
        <a-input
          v-model:value="advancedQueryForm.vendorName3"
          :placeholder="pi.queryPh('vendorName3', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorName4')">
      <a-form-item :label="pi.queryLabel('vendorName4')">
        <a-input
          v-model:value="advancedQueryForm.vendorName4"
          :placeholder="pi.queryPh('vendorName4', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorShortName')">
      <a-form-item :label="pi.queryLabel('vendorShortName')">
        <a-input
          v-model:value="advancedQueryForm.vendorShortName"
          :placeholder="pi.queryPh('vendorShortName', 'required')"
          show-count
          :maxlength="40"
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
      <div v-show="isFieldVisible('regionCode')">
      <a-form-item :label="pi.queryLabel('regionCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.regionCode"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 2 }"
          :placeholder="pi.queryPh('regionCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cityCode')">
      <a-form-item :label="pi.queryLabel('cityCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cityCode"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 3 }"
          :placeholder="pi.queryPh('cityCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('addr1')">
      <a-form-item :label="pi.queryLabel('addr1')">
        <a-input
          v-model:value="advancedQueryForm.addr1"
          :placeholder="pi.queryPh('addr1', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('addr2')">
      <a-form-item :label="pi.queryLabel('addr2')">
        <a-input
          v-model:value="advancedQueryForm.addr2"
          :placeholder="pi.queryPh('addr2', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postalCode')">
      <a-form-item :label="pi.queryLabel('postalCode')">
        <a-input
          v-model:value="advancedQueryForm.postalCode"
          :placeholder="pi.queryPh('postalCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('phone1')">
      <a-form-item :label="pi.queryLabel('phone1')">
        <a-input
          v-model:value="advancedQueryForm.phone1"
          :placeholder="pi.queryPh('phone1', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('phone2')">
      <a-form-item :label="pi.queryLabel('phone2')">
        <a-input
          v-model:value="advancedQueryForm.phone2"
          :placeholder="pi.queryPh('phone2', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faxNumber')">
      <a-form-item :label="pi.queryLabel('faxNumber')">
        <a-input
          v-model:value="advancedQueryForm.faxNumber"
          :placeholder="pi.queryPh('faxNumber', 'required')"
          show-count
          :maxlength="40"
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
      <div v-show="isFieldVisible('authorizationGroup')">
      <a-form-item :label="pi.queryLabel('authorizationGroup')">
        <a-input
          v-model:value="advancedQueryForm.authorizationGroup"
          :placeholder="pi.queryPh('authorizationGroup', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('groupKey')">
      <a-form-item :label="pi.queryLabel('groupKey')">
        <a-input
          v-model:value="advancedQueryForm.groupKey"
          :placeholder="pi.queryPh('groupKey', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorType')">
      <a-form-item :label="pi.queryLabel('vendorType')">
        <TaktSelect
          v-model:value="advancedQueryForm.vendorType"
          dict-type="logistics_sales_customer_category"
          :placeholder="pi.queryPh('vendorType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('enterpriseNature')">
      <a-form-item :label="pi.queryLabel('enterpriseNature')">
        <TaktSelect
          v-model:value="advancedQueryForm.enterpriseNature"
          dict-type="sys_enterprise_nature"
          :placeholder="pi.queryPh('enterpriseNature', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industryAttribute')">
      <a-form-item :label="pi.queryLabel('industryAttribute')">
        <TaktSelect
          v-model:value="advancedQueryForm.industryAttribute"
          dict-type="sys_industry_attribute"
          :placeholder="pi.queryPh('industryAttribute', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('creditInfoNumber')">
      <a-form-item :label="pi.queryLabel('creditInfoNumber')">
        <a-input
          v-model:value="advancedQueryForm.creditInfoNumber"
          :placeholder="pi.queryPh('creditInfoNumber', 'required')"
          show-count
          :maxlength="11"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bankCountryCode')">
      <a-form-item :label="pi.queryLabel('bankCountryCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.bankCountryCode"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('bankCountryCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bankCode')">
      <a-form-item :label="pi.queryLabel('bankCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.bankCode"
          api-url="TaktBanks/options"
          :placeholder="pi.queryPh('bankCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bankAccount')">
      <a-form-item :label="pi.queryLabel('bankAccount')">
        <a-input
          v-model:value="advancedQueryForm.bankAccount"
          :placeholder="pi.queryPh('bankAccount', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountHolder')">
      <a-form-item :label="pi.queryLabel('accountHolder')">
        <a-input
          v-model:value="advancedQueryForm.accountHolder"
          :placeholder="pi.queryPh('accountHolder', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bankChargeBearerCode')">
      <a-form-item :label="pi.queryLabel('bankChargeBearerCode')">
        <a-input
          v-model:value="advancedQueryForm.bankChargeBearerCode"
          :placeholder="pi.queryPh('bankChargeBearerCode', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reconciliationAccount')">
      <a-form-item :label="pi.queryLabel('reconciliationAccount')">
        <TaktSelect
          v-model:value="advancedQueryForm.reconciliationAccount"
          api-url="TaktAccountTitles/options"
          :api-params="{ reconciliationOnly: true, auxiliaryType: 'K' }"
          :placeholder="pi.queryPh('reconciliationAccount', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headOfficeAccountCode')">
      <a-form-item :label="pi.queryLabel('headOfficeAccountCode')">
        <a-input
          v-model:value="advancedQueryForm.headOfficeAccountCode"
          :placeholder="pi.queryPh('headOfficeAccountCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planningGroup')">
      <a-form-item :label="pi.queryLabel('planningGroup')">
        <a-input
          v-model:value="advancedQueryForm.planningGroup"
          :placeholder="pi.queryPh('planningGroup', 'required')"
          show-count
          :maxlength="10"
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
      <div v-show="isFieldVisible('doubleInvoiceCheck')">
      <a-form-item :label="pi.queryLabel('doubleInvoiceCheck')">
        <TaktSelect
          v-model:value="advancedQueryForm.doubleInvoiceCheck"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('doubleInvoiceCheck', 'select')"
          allow-clear
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
      <div v-show="isFieldVisible('paymentBlock')">
      <a-form-item :label="pi.queryLabel('paymentBlock')">
        <TaktSelect
          v-model:value="advancedQueryForm.paymentBlock"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('paymentBlock', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('houseBank')">
      <a-form-item :label="pi.queryLabel('houseBank')">
        <TaktSelect
          v-model:value="advancedQueryForm.houseBank"
          api-url="TaktBanks/options"
          :placeholder="pi.queryPh('houseBank', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clearingWithCustomer')">
      <a-form-item :label="pi.queryLabel('clearingWithCustomer')">
        <TaktSelect
          v-model:value="advancedQueryForm.clearingWithCustomer"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('clearingWithCustomer', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountingClerk')">
      <a-form-item :label="pi.queryLabel('accountingClerk')">
        <a-input
          v-model:value="advancedQueryForm.accountingClerk"
          :placeholder="pi.queryPh('accountingClerk', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ourAccountAtVendor')">
      <a-form-item :label="pi.queryLabel('ourAccountAtVendor')">
        <a-input
          v-model:value="advancedQueryForm.ourAccountAtVendor"
          :placeholder="pi.queryPh('ourAccountAtVendor', 'required')"
          show-count
          :maxlength="12"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPerson')">
      <a-form-item :label="pi.queryLabel('contactPerson')">
        <a-input
          v-model:value="advancedQueryForm.contactPerson"
          :placeholder="pi.queryPh('contactPerson', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactPhone')">
      <a-form-item :label="pi.queryLabel('contactPhone')">
        <a-input
          v-model:value="advancedQueryForm.contactPhone"
          :placeholder="pi.queryPh('contactPhone', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contactEmail')">
      <a-form-item :label="pi.queryLabel('contactEmail')">
        <a-input
          v-model:value="advancedQueryForm.contactEmail"
          :placeholder="pi.queryPh('contactEmail', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('withholdingTaxCountry')">
      <a-form-item :label="pi.queryLabel('withholdingTaxCountry')">
        <TaktSelect
          v-model:value="advancedQueryForm.withholdingTaxCountry"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('withholdingTaxCountry', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('withholdingTaxType')">
      <a-form-item :label="pi.queryLabel('withholdingTaxType')">
        <TaktSelect
          v-model:value="advancedQueryForm.withholdingTaxType"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('withholdingTaxType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('withholdingTaxCode')">
      <a-form-item :label="pi.queryLabel('withholdingTaxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.withholdingTaxCode"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('withholdingTaxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingOrganization')">
      <a-form-item :label="pi.queryLabel('purchasingOrganization')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchasingOrganization"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('purchasingOrganization', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderCurrencyCode')">
      <a-form-item :label="pi.queryLabel('orderCurrencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderCurrencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('orderCurrencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('incoterms1')">
      <a-form-item :label="pi.queryLabel('incoterms1')">
        <TaktSelect
          v-model:value="advancedQueryForm.incoterms1"
          dict-type="logistics_sales_incoterms1"
          :placeholder="pi.queryPh('incoterms1', 'select')"
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
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingDateControl')">
      <a-form-item :label="pi.queryLabel('pricingDateControl')">
        <TaktSelect
          v-model:value="advancedQueryForm.pricingDateControl"
          dict-type="logistics_procurement_pricing_date_control"
          :placeholder="pi.queryPh('pricingDateControl', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grBasedInvoiceInspection')">
      <a-form-item :label="pi.queryLabel('grBasedInvoiceInspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.grBasedInvoiceInspection"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('grBasedInvoiceInspection', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluatedReceiptSettlement')">
      <a-form-item :label="pi.queryLabel('evaluatedReceiptSettlement')">
        <TaktSelect
          v-model:value="advancedQueryForm.evaluatedReceiptSettlement"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('evaluatedReceiptSettlement', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('returnErsFlag')">
      <a-form-item :label="pi.queryLabel('returnErsFlag')">
        <TaktSelect
          v-model:value="advancedQueryForm.returnErsFlag"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('returnErsFlag', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('automaticPurchaseOrder')">
      <a-form-item :label="pi.queryLabel('automaticPurchaseOrder')">
        <TaktSelect
          v-model:value="advancedQueryForm.automaticPurchaseOrder"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('automaticPurchaseOrder', 'select')"
          allow-clear
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
      <div v-show="isFieldVisible('taxNumber1')">
      <a-form-item :label="pi.queryLabel('taxNumber1')">
        <a-input
          v-model:value="advancedQueryForm.taxNumber1"
          :placeholder="pi.queryPh('taxNumber1', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxNumber2')">
      <a-form-item :label="pi.queryLabel('taxNumber2')">
        <a-input
          v-model:value="advancedQueryForm.taxNumber2"
          :placeholder="pi.queryPh('taxNumber2', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxNumber3')">
      <a-form-item :label="pi.queryLabel('taxNumber3')">
        <a-input
          v-model:value="advancedQueryForm.taxNumber3"
          :placeholder="pi.queryPh('taxNumber3', 'required')"
          show-count
          :maxlength="60"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('creditLevel')">
      <a-form-item :label="pi.queryLabel('creditLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.creditLevel"
          dict-type="logistics_sales_credit_rating"
          :placeholder="pi.queryPh('creditLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorLevel')">
      <a-form-item :label="pi.queryLabel('vendorLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.vendorLevel"
          dict-type="logistics_sales_customer_level"
          :placeholder="pi.queryPh('vendorLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vendorStatus')">
      <a-form-item :label="pi.queryLabel('vendorStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.vendorStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('vendorStatus', 'select')"
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
        :entity-i18n-key="VENDOR_SELF_I18N_KEY"
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
      :id-column-key="'vendorId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt经销商实体 <para>业务唯一键：TenantCode+CompanyCode+PurchasingOrganization+VendorCode管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/vendor
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import VendorForm from './components/VendorForm.vue'
import { getVendorList, getVendorById, createVendor, updateVendor, deleteVendorById, deleteVendorBatch, getVendorTemplate, importVendor, exportVendor, updateVendorStatus } from '@/api/logistics/procurement/vendor.api'
import type { Vendor, VendorQuery } from '@/types/logistics/procurement/vendor.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useVendorI18n,
  VENDOR_LIST_FIELDS,
  VENDOR_QUERY_STRING_FIELDS,
  VENDOR_QUERY_FIELDS,
  VENDOR_SELF_I18N_KEY,
} from './composables/useVendorI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useVendorI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type VendorRowRecord = Vendor | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktVendor')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Vendor[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<VendorRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<VendorRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Vendor> | null>(null)
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
  for (const key of VENDOR_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.vendorTitle !== undefined && form.vendorTitle !== null) {
    return true
  }
  if (form.vendorType !== undefined && form.vendorType !== null) {
    return true
  }
  if (form.doubleInvoiceCheck !== undefined && form.doubleInvoiceCheck !== null) {
    return true
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    return true
  }
  if (form.paymentBlock !== undefined && form.paymentBlock !== null) {
    return true
  }
  if (form.clearingWithCustomer !== undefined && form.clearingWithCustomer !== null) {
    return true
  }
  if (form.pricingDateControl !== undefined && form.pricingDateControl !== null) {
    return true
  }
  if (form.grBasedInvoiceInspection !== undefined && form.grBasedInvoiceInspection !== null) {
    return true
  }
  if (form.evaluatedReceiptSettlement !== undefined && form.evaluatedReceiptSettlement !== null) {
    return true
  }
  if (form.returnErsFlag !== undefined && form.returnErsFlag !== null) {
    return true
  }
  if (form.automaticPurchaseOrder !== undefined && form.automaticPurchaseOrder !== null) {
    return true
  }
  if (form.creditLevel !== undefined && form.creditLevel !== null) {
    return true
  }
  if (form.vendorLevel !== undefined && form.vendorLevel !== null) {
    return true
  }
  if (form.vendorStatus !== undefined && form.vendorStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(VENDOR_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof VENDOR_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    vendorTitle: undefined as number | undefined,
    vendorType: undefined as number | undefined,
    doubleInvoiceCheck: undefined as number | undefined,
    paymentMethod: undefined as number | undefined,
    paymentBlock: undefined as number | undefined,
    clearingWithCustomer: undefined as number | undefined,
    pricingDateControl: undefined as number | undefined,
    grBasedInvoiceInspection: undefined as number | undefined,
    evaluatedReceiptSettlement: undefined as number | undefined,
    returnErsFlag: undefined as number | undefined,
    automaticPurchaseOrder: undefined as number | undefined,
    creditLevel: undefined as number | undefined,
    vendorLevel: undefined as number | undefined,
    vendorStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  VENDOR_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'vendorId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {VendorQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<VendorQuery>): VendorQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: VendorQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof VendorQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of VENDOR_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.vendorTitle !== undefined && form.vendorTitle !== null) {
    query.vendorTitle = form.vendorTitle
  }
  if (form.vendorType !== undefined && form.vendorType !== null) {
    query.vendorType = form.vendorType
  }
  if (form.doubleInvoiceCheck !== undefined && form.doubleInvoiceCheck !== null) {
    query.doubleInvoiceCheck = form.doubleInvoiceCheck
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    query.paymentMethod = form.paymentMethod
  }
  if (form.paymentBlock !== undefined && form.paymentBlock !== null) {
    query.paymentBlock = form.paymentBlock
  }
  if (form.clearingWithCustomer !== undefined && form.clearingWithCustomer !== null) {
    query.clearingWithCustomer = form.clearingWithCustomer
  }
  if (form.pricingDateControl !== undefined && form.pricingDateControl !== null) {
    query.pricingDateControl = form.pricingDateControl
  }
  if (form.grBasedInvoiceInspection !== undefined && form.grBasedInvoiceInspection !== null) {
    query.grBasedInvoiceInspection = form.grBasedInvoiceInspection
  }
  if (form.evaluatedReceiptSettlement !== undefined && form.evaluatedReceiptSettlement !== null) {
    query.evaluatedReceiptSettlement = form.evaluatedReceiptSettlement
  }
  if (form.returnErsFlag !== undefined && form.returnErsFlag !== null) {
    query.returnErsFlag = form.returnErsFlag
  }
  if (form.automaticPurchaseOrder !== undefined && form.automaticPurchaseOrder !== null) {
    query.automaticPurchaseOrder = form.automaticPurchaseOrder
  }
  if (form.creditLevel !== undefined && form.creditLevel !== null) {
    query.creditLevel = form.creditLevel
  }
  if (form.vendorLevel !== undefined && form.vendorLevel !== null) {
    query.vendorLevel = form.vendorLevel
  }
  if (form.vendorStatus !== undefined && form.vendorStatus !== null) {
    query.vendorStatus = form.vendorStatus
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildVendorListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  buildVendorListColumn('vendorId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...VENDOR_LIST_FIELDS.map((key) => buildVendorListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:procurement:vendor:update',
        onClick: (record: VendorRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:procurement:vendor:delete',
        onClick: (record: VendorRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getVendorId = (record: VendorRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getVendorDictValue = (
  record: VendorRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toVendorNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: VendorRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: VendorRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getVendorId(selecmedRow.value) === getVendorId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: VendorRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: VendorRowRecord) => ({
  onClick: () => {
    const key = getVendorId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getVendorId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getVendorList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Vendor] 加载数据失败', { error })
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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
/** 打开编辑弹窗（拉取详情，避免列表列裁剪字段） */
async function handleEdit(record: VendorRowRecord) {
  const id = getVendorId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getVendorById(id)
    formData.value = detail ?? ({ ...record } as Partial<Vendor>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
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
      await updateVendor(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createVendor(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getVendorTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importVendor(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
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
    const exportMeta = await exportVendor(
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
    logger.error('[Vendor] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: VendorRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteVendorById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
      await deleteVendorBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleVendorStatusChange(record: VendorRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toVendorNumber(getVendorDictValue(record, 'vendorStatus'))
  const id = getVendorId(record)
  const row = dataSource.value.find((item) => getVendorId(item) === id)
  if (row) {
    row.vendorStatus = newVal
  }
  try {
    await updateVendorStatus({ vendorId: id, vendorStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.vendorStatus = oldVal
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

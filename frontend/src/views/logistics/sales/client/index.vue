<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/client -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt客户端信息实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:sales:client:create"
      update-permission="logistics:sales:client:update"
      delete-permission="logistics:sales:client:delete"
      import-permission="logistics:sales:client:import"
      export-permission="logistics:sales:client:export"
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
      :id-column-key="'clientId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getClientId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'clientStatus'">
          <a-switch
            :checked="getClientDictValue(record, 'clientStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleClientStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'clientType'">
          <TaktDictTag
            :value="getClientDictValue(record, 'clientType')"
            dict-type="logistics_sales_client_category"
          />
        </template>
        <template v-else-if="column.key === 'enterpriseNature'">
          <TaktDictTag
            :value="getClientDictValue(record, 'enterpriseNature')"
            dict-type="sys_enterprise_nature"
          />
        </template>
        <template v-else-if="column.key === 'industryAttribute'">
          <TaktDictTag
            :value="getClientDictValue(record, 'industryAttribute')"
            dict-type="sys_industry_attribute"
          />
        </template>
        <template v-else-if="column.key === 'taxCode'">
          <TaktDictTag
            :value="getClientDictValue(record, 'taxCode')"
            dict-type="accounting_financial_tax_code"
          />
        </template>
        <template v-else-if="column.key === 'taxRate'">
          <TaktDictTag
            :value="getClientDictValue(record, 'taxRate')"
            dict-type="accounting_financial_tax_code"
          />
        </template>
        <template v-else-if="column.key === 'registrationCountry'">
          <TaktDictTag
            :value="getClientDictValue(record, 'registrationCountry')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getClientDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'customerGroup'">
          <TaktDictTag
            :value="getClientDictValue(record, 'customerGroup')"
            dict-type="logistics_sales_customer_group"
          />
        </template>
        <template v-else-if="column.key === 'accountAssignmentGroup'">
          <TaktDictTag
            :value="getClientDictValue(record, 'accountAssignmentGroup')"
            dict-type="logistics_sales_account_assignment_group"
          />
        </template>
        <template v-else-if="column.key === 'centralPostingBlock'">
          <TaktDictTag
            :value="getClientDictValue(record, 'centralPostingBlock')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'clearingWithVendor'">
          <TaktDictTag
            :value="getClientDictValue(record, 'clearingWithVendor')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'paymentTerms'">
          <TaktDictTag
            :value="getClientDictValue(record, 'paymentTerms')"
            dict-type="accounting_financial_payment_terms_param"
          />
        </template>
        <template v-else-if="column.key === 'paymentMethod'">
          <TaktDictTag
            :value="getClientDictValue(record, 'paymentMethod')"
            dict-type="accounting_financial_payment_method"
          />
        </template>
        <template v-else-if="column.key === 'incoterms1'">
          <TaktDictTag
            :value="getClientDictValue(record, 'incoterms1')"
            dict-type="logistics_sales_incoterms1"
          />
        </template>
        <template v-else-if="column.key === 'shippingConditions'">
          <TaktDictTag
            :value="getClientDictValue(record, 'shippingConditions')"
            dict-type="logistics_sales_shipping_conditions"
          />
        </template>
        <template v-else-if="column.key === 'customerPricingProcedure'">
          <TaktDictTag
            :value="getClientDictValue(record, 'customerPricingProcedure')"
            dict-type="logistics_sales_customer_pricing_procedure"
          />
        </template>
        <template v-else-if="column.key === 'salesChannel'">
          <TaktDictTag
            :value="getClientDictValue(record, 'salesChannel')"
            dict-type="logistics_sales_channel"
          />
        </template>
        <template v-else-if="column.key === 'clientLevel'">
          <TaktDictTag
            :value="getClientDictValue(record, 'clientLevel')"
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
      <ClientForm
        :key="formData?.clientId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-sales-client'"
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
      <div v-show="isFieldVisible('clientCode')">
      <a-form-item :label="pi.queryLabel('clientCode')">
        <a-input
          v-model:value="advancedQueryForm.clientCode"
          :placeholder="pi.queryPh('clientCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName1')">
      <a-form-item :label="pi.queryLabel('clientName1')">
        <a-input
          v-model:value="advancedQueryForm.clientName1"
          :placeholder="pi.queryPh('clientName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientName2')">
      <a-form-item :label="pi.queryLabel('clientName2')">
        <a-input
          v-model:value="advancedQueryForm.clientName2"
          :placeholder="pi.queryPh('clientName2', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientShortName')">
      <a-form-item :label="pi.queryLabel('clientShortName')">
        <a-input
          v-model:value="advancedQueryForm.clientShortName"
          :placeholder="pi.queryPh('clientShortName', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientType')">
      <a-form-item :label="pi.queryLabel('clientType')">
        <TaktSelect
          v-model:value="advancedQueryForm.clientType"
          dict-type="logistics_sales_client_category"
          :placeholder="pi.queryPh('clientType', 'select')"
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
      <div v-show="isFieldVisible('clientTaxNumber')">
      <a-form-item :label="pi.queryLabel('clientTaxNumber')">
        <a-input
          v-model:value="advancedQueryForm.clientTaxNumber"
          :placeholder="pi.queryPh('clientTaxNumber', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxCode')">
      <a-form-item :label="pi.queryLabel('taxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxCode"
          dict-type="accounting_financial_tax_code"
          :placeholder="pi.queryPh('taxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="pi.queryLabel('taxRate')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxRate"
          dict-type="accounting_financial_tax_code"
          :placeholder="pi.queryPh('taxRate', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCountry')">
      <a-form-item :label="pi.queryLabel('registrationCountry')">
        <TaktSelect
          v-model:value="advancedQueryForm.registrationCountry"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('registrationCountry', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationProvince')">
      <a-form-item :label="pi.queryLabel('registrationProvince')">
        <TaktSelect
          v-model:value="advancedQueryForm.registrationProvince"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 2 }"
          :placeholder="pi.queryPh('registrationProvince', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationCity')">
      <a-form-item :label="pi.queryLabel('registrationCity')">
        <TaktSelect
          v-model:value="advancedQueryForm.registrationCity"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 3 }"
          :placeholder="pi.queryPh('registrationCity', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress1')">
      <a-form-item :label="pi.queryLabel('registrationAddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress1"
          :placeholder="pi.queryPh('registrationAddress1', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registrationAddress2')">
      <a-form-item :label="pi.queryLabel('registrationAddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.registrationAddress2"
          :placeholder="pi.queryPh('registrationAddress2', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientPhone')">
      <a-form-item :label="pi.queryLabel('clientPhone')">
        <a-input
          v-model:value="advancedQueryForm.clientPhone"
          :placeholder="pi.queryPh('clientPhone', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientFax')">
      <a-form-item :label="pi.queryLabel('clientFax')">
        <a-input
          v-model:value="advancedQueryForm.clientFax"
          :placeholder="pi.queryPh('clientFax', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientEmail')">
      <a-form-item :label="pi.queryLabel('clientEmail')">
        <a-input
          v-model:value="advancedQueryForm.clientEmail"
          :placeholder="pi.queryPh('clientEmail', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientWebsite')">
      <a-form-item :label="pi.queryLabel('clientWebsite')">
        <a-input
          v-model:value="advancedQueryForm.clientWebsite"
          :placeholder="pi.queryPh('clientWebsite', 'required')"
          show-count
          :maxlength="200"
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
          :maxlength="50"
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
          :maxlength="50"
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
        <TaktSelect
          v-model:value="advancedQueryForm.salesOrganization"
          api-url="TaktCompanies/options"
          :placeholder="pi.queryPh('salesOrganization', 'select')"
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
      <div v-show="isFieldVisible('productGroup')">
      <a-form-item :label="pi.queryLabel('productGroup')">
        <a-input
          v-model:value="advancedQueryForm.productGroup"
          :placeholder="pi.queryPh('productGroup', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerGroup')">
      <a-form-item :label="pi.queryLabel('customerGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerGroup"
          dict-type="logistics_sales_customer_group"
          :placeholder="pi.queryPh('customerGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('tradingPartner')">
      <a-form-item :label="pi.queryLabel('tradingPartner')">
        <TaktSelect
          v-model:value="advancedQueryForm.tradingPartner"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('tradingPartner', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountAssignmentGroup')">
      <a-form-item :label="pi.queryLabel('accountAssignmentGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountAssignmentGroup"
          dict-type="logistics_sales_account_assignment_group"
          :placeholder="pi.queryPh('accountAssignmentGroup', 'select')"
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
      <div v-show="isFieldVisible('nielsenIndicator')">
      <a-form-item :label="pi.queryLabel('nielsenIndicator')">
        <a-input
          v-model:value="advancedQueryForm.nielsenIndicator"
          :placeholder="pi.queryPh('nielsenIndicator', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('centralPostingBlock')">
      <a-form-item :label="pi.queryLabel('centralPostingBlock')">
        <TaktSelect
          v-model:value="advancedQueryForm.centralPostingBlock"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('centralPostingBlock', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reconciliationAccount')">
      <a-form-item :label="pi.queryLabel('reconciliationAccount')">
        <TaktSelect
          v-model:value="advancedQueryForm.reconciliationAccount"
          api-url="TaktAccountTitles/options"
          :api-params="{ reconciliationOnly: true, auxiliaryType: 'D' }"
          :placeholder="pi.queryPh('reconciliationAccount', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headquarters')">
      <a-form-item :label="pi.queryLabel('headquarters')">
        <TaktSelect
          v-model:value="advancedQueryForm.headquarters"
          api-url="TaktClients/options"
          :placeholder="pi.queryPh('headquarters', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clearingWithVendor')">
      <a-form-item :label="pi.queryLabel('clearingWithVendor')">
        <TaktSelect
          v-model:value="advancedQueryForm.clearingWithVendor"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('clearingWithVendor', 'select')"
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
      <div v-show="isFieldVisible('deliveringPlant')">
      <a-form-item :label="pi.queryLabel('deliveringPlant')">
        <TaktSelect
          v-model:value="advancedQueryForm.deliveringPlant"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('deliveringPlant', 'select')"
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
      <div v-show="isFieldVisible('customerPricingProcedure')">
      <a-form-item :label="pi.queryLabel('customerPricingProcedure')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerPricingProcedure"
          dict-type="logistics_sales_customer_pricing_procedure"
          :placeholder="pi.queryPh('customerPricingProcedure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesChannel')">
      <a-form-item :label="pi.queryLabel('salesChannel')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesChannel"
          dict-type="logistics_sales_channel"
          :placeholder="pi.queryPh('salesChannel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('platformName')">
      <a-form-item :label="pi.queryLabel('platformName')">
        <a-input
          v-model:value="advancedQueryForm.platformName"
          :placeholder="pi.queryPh('platformName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storeName')">
      <a-form-item :label="pi.queryLabel('storeName')">
        <a-input
          v-model:value="advancedQueryForm.storeName"
          :placeholder="pi.queryPh('storeName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientLevel')">
      <a-form-item :label="pi.queryLabel('clientLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.clientLevel"
          dict-type="logistics_sales_customer_level"
          :placeholder="pi.queryPh('clientLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationScore')">
      <a-form-item :label="pi.queryLabel('evaluationScore')">
        <a-input-number
          v-model:value="advancedQueryForm.evaluationScore"
          :placeholder="pi.queryPh('evaluationScore', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientStatus')">
      <a-form-item :label="pi.queryLabel('clientStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.clientStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('clientStatus', 'select')"
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
        :entity-i18n-key="CLIENT_SELF_I18N_KEY"
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
      :id-column-key="'clientId'"
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
 * Takt客户端信息实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/client
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import ClientForm from './components/ClientForm.vue'
import { getClientList, getClientById, createClient, updateClient, deleteClientById, deleteClientBatch, getClientTemplate, importClient, exportClient, updateClientStatus } from '@/api/logistics/sales/client.api'
import type { Client, ClientQuery } from '@/types/logistics/sales/client.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useClientI18n,
  CLIENT_LIST_FIELDS,
  CLIENT_QUERY_STRING_FIELDS,
  CLIENT_QUERY_FIELDS,
  CLIENT_SELF_I18N_KEY,
} from './composables/useClientI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useClientI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type ClientRowRecord = Client | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktClient')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Client[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<ClientRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<ClientRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Client> | null>(null)
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
  for (const key of CLIENT_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.clientType !== undefined && form.clientType !== null) {
    return true
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    return true
  }
  if (form.centralPostingBlock !== undefined && form.centralPostingBlock !== null) {
    return true
  }
  if (form.clearingWithVendor !== undefined && form.clearingWithVendor !== null) {
    return true
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    return true
  }
  if (form.salesChannel !== undefined && form.salesChannel !== null) {
    return true
  }
  if (form.clientLevel !== undefined && form.clientLevel !== null) {
    return true
  }
  if (form.evaluationScore !== undefined && form.evaluationScore !== null) {
    return true
  }
  if (form.clientStatus !== undefined && form.clientStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(CLIENT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof CLIENT_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    clientType: undefined as number | undefined,
    taxRate: undefined as number | undefined,
    centralPostingBlock: undefined as number | undefined,
    clearingWithVendor: undefined as number | undefined,
    paymentMethod: undefined as number | undefined,
    salesChannel: undefined as number | undefined,
    clientLevel: undefined as number | undefined,
    evaluationScore: undefined as number | undefined,
    clientStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  CLIENT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'clientId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ClientQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ClientQuery>): ClientQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ClientQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ClientQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of CLIENT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.clientType !== undefined && form.clientType !== null) {
    query.clientType = form.clientType
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.centralPostingBlock !== undefined && form.centralPostingBlock !== null) {
    query.centralPostingBlock = form.centralPostingBlock
  }
  if (form.clearingWithVendor !== undefined && form.clearingWithVendor !== null) {
    query.clearingWithVendor = form.clearingWithVendor
  }
  if (form.paymentMethod !== undefined && form.paymentMethod !== null) {
    query.paymentMethod = form.paymentMethod
  }
  if (form.salesChannel !== undefined && form.salesChannel !== null) {
    query.salesChannel = form.salesChannel
  }
  if (form.clientLevel !== undefined && form.clientLevel !== null) {
    query.clientLevel = form.clientLevel
  }
  if (form.evaluationScore !== undefined && form.evaluationScore !== null) {
    query.evaluationScore = form.evaluationScore
  }
  if (form.clientStatus !== undefined && form.clientStatus !== null) {
    query.clientStatus = form.clientStatus
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
function buildClientListColumn(
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
  buildClientListColumn('clientId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...CLIENT_LIST_FIELDS.map((key) => buildClientListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:client:update',
        onClick: (record: ClientRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:client:delete',
        onClick: (record: ClientRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getClientId = (record: ClientRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getClientDictValue = (
  record: ClientRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toClientNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ClientRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ClientRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getClientId(selectedRow.value) === getClientId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ClientRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ClientRowRecord) => ({
  onClick: () => {
    const key = getClientId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getClientId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
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
    const res = await getClientList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Client] 加载数据失败', { error })
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
async function handleEdit(record: ClientRowRecord) {
  const id = getClientId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getClientById(id)
    formData.value = detail ?? ({ ...record } as Partial<Client>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
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
      await updateClient(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createClient(payload as any)
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
  const res = await getClientTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importClient(file, sheetName)
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
    const exportMeta = await exportClient(
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
    logger.error('[Client] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ClientRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteClientById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
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
      await deleteClientBatch(ids)
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
async function handleClientStatusChange(record: ClientRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toClientNumber(getClientDictValue(record, 'clientStatus'))
  const id = getClientId(record)
  const row = dataSource.value.find((item) => getClientId(item) === id)
  if (row) {
    row.clientStatus = newVal
  }
  try {
    await updateClientStatus({ clientId: id, clientStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.clientStatus = oldVal
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

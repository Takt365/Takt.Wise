<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/company -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公司实体 代表租户下的独立公司管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:financial:company:create"
      update-permission="accounting:financial:company:update"
      delete-permission="accounting:financial:company:delete"
      import-permission="accounting:financial:company:import"
      export-permission="accounting:financial:company:export"
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
      entity-scope="tenant"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'companyId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getCompanyId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'companyStatus'">
          <a-switch
            :checked="getCompanyDictValue(record, 'companyStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleCompanyStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'enterpriseNature'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'enterpriseNature')"
            dict-type="sys_enterprise_nature"
          />
        </template>
        <template v-else-if="column.key === 'industryAttribute'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'industryAttribute')"
            dict-type="sys_industry_attribute"
          />
        </template>
        <template v-else-if="column.key === 'enterpriseScale'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'enterpriseScale')"
            dict-type="sys_enterprise_scale"
          />
        </template>
        <template v-else-if="column.key === 'registrationRegion'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'registrationRegion')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'businessRegion'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'businessRegion')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'companyExistence'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'companyExistence')"
            dict-type="sys_entity_existence_status"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'chartOfAccounts'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'chartOfAccounts')"
            dict-type="accounting_financial_chart_of_accounts"
          />
        </template>
        <template v-else-if="column.key === 'inputTaxCode'">
          {{ record.inputTaxCode }}
        </template>
        <template v-else-if="column.key === 'outputTaxCode'">
          {{ record.outputTaxCode }}
        </template>
        <template v-else-if="column.key === 'postingPeriodVariant'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'postingPeriodVariant')"
            dict-type="accounting_financial_posting_period_variant"
          />
        </template>
        <template v-else-if="column.key === 'fiscalYearVariant'">
          <TaktDictTag
            :value="getCompanyDictValue(record, 'fiscalYearVariant')"
            dict-type="accounting_financial_fiscal_year_variant"
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
      <CompanyForm
        :key="formData?.companyId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-company'"
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
      <div v-show="isFieldVisible('companyName1')">
      <a-form-item :label="pi.queryLabel('companyName1')">
        <a-input
          v-model:value="advancedQueryForm.companyName1"
          :placeholder="pi.queryPh('companyName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyName2')">
      <a-form-item :label="pi.queryLabel('companyName2')">
        <a-input
          v-model:value="advancedQueryForm.companyName2"
          :placeholder="pi.queryPh('companyName2', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyShortName')">
      <a-form-item :label="pi.queryLabel('companyShortName')">
        <a-input
          v-model:value="advancedQueryForm.companyShortName"
          :placeholder="pi.queryPh('companyShortName', 'required')"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('enterpriseScale')">
      <a-form-item :label="pi.queryLabel('enterpriseScale')">
        <TaktSelect
          v-model:value="advancedQueryForm.enterpriseScale"
          dict-type="sys_enterprise_scale"
          :placeholder="pi.queryPh('enterpriseScale', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessScope')">
      <a-form-item :label="pi.queryLabel('businessScope')">
        <a-textarea
          v-model:value="advancedQueryForm.businessScope"
          :placeholder="pi.queryPh('businessScope', 'optional')"
          :rows="2"
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
      <div v-show="isFieldVisible('registrationRegion')">
      <a-form-item :label="pi.queryLabel('registrationRegion')">
        <TaktSelect
          v-model:value="advancedQueryForm.registrationRegion"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('registrationRegion', 'select')"
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
      <div v-show="isFieldVisible('businessRegion')">
      <a-form-item :label="pi.queryLabel('businessRegion')">
        <TaktSelect
          v-model:value="advancedQueryForm.businessRegion"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('businessRegion', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessProvince')">
      <a-form-item :label="pi.queryLabel('businessProvince')">
        <TaktSelect
          v-model:value="advancedQueryForm.businessProvince"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 2 }"
          :placeholder="pi.queryPh('businessProvince', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessCity')">
      <a-form-item :label="pi.queryLabel('businessCity')">
        <TaktSelect
          v-model:value="advancedQueryForm.businessCity"
          api-url="TaktAdminDivisions/options"
          :api-params="{ level: 3 }"
          :placeholder="pi.queryPh('businessCity', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAddress1')">
      <a-form-item :label="pi.queryLabel('businessAddress1')">
        <a-textarea
          v-model:value="advancedQueryForm.businessAddress1"
          :placeholder="pi.queryPh('businessAddress1', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessAddress2')">
      <a-form-item :label="pi.queryLabel('businessAddress2')">
        <a-textarea
          v-model:value="advancedQueryForm.businessAddress2"
          :placeholder="pi.queryPh('businessAddress2', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyPhone')">
      <a-form-item :label="pi.queryLabel('companyPhone')">
        <a-input
          v-model:value="advancedQueryForm.companyPhone"
          :placeholder="pi.queryPh('companyPhone', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyEmail')">
      <a-form-item :label="pi.queryLabel('companyEmail')">
        <a-input
          v-model:value="advancedQueryForm.companyEmail"
          :placeholder="pi.queryPh('companyEmail', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyFax')">
      <a-form-item :label="pi.queryLabel('companyFax')">
        <a-input
          v-model:value="advancedQueryForm.companyFax"
          :placeholder="pi.queryPh('companyFax', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyWebsite')">
      <a-form-item :label="pi.queryLabel('companyWebsite')">
        <a-input
          v-model:value="advancedQueryForm.companyWebsite"
          :placeholder="pi.queryPh('companyWebsite', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unifiedSocialCreditCode')">
      <a-form-item :label="pi.queryLabel('unifiedSocialCreditCode')">
        <a-input
          v-model:value="advancedQueryForm.unifiedSocialCreditCode"
          :placeholder="pi.queryPh('unifiedSocialCreditCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRegistrationNumber')">
      <a-form-item :label="pi.queryLabel('taxRegistrationNumber')">
        <a-input
          v-model:value="advancedQueryForm.taxRegistrationNumber"
          :placeholder="pi.queryPh('taxRegistrationNumber', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('legalRepresentative')">
      <a-form-item :label="pi.queryLabel('legalRepresentative')">
        <a-input
          v-model:value="advancedQueryForm.legalRepresentative"
          :placeholder="pi.queryPh('legalRepresentative', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyManager')">
      <a-form-item :label="pi.queryLabel('companyManager')">
        <a-input
          v-model:value="advancedQueryForm.companyManager"
          :placeholder="pi.queryPh('companyManager', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('registeredCapital')">
      <a-form-item :label="pi.queryLabel('registeredCapital')">
        <a-input-number
          v-model:value="advancedQueryForm.registeredCapital"
          :placeholder="pi.queryPh('registeredCapital', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('establishmentDateStart')">
      <a-form-item :label="pi.queryLabel('establishmentDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.establishmentDateStart"
          :placeholder="pi.queryPh('establishmentDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('establishmentDateEnd')">
      <a-form-item :label="pi.queryLabel('establishmentDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.establishmentDateEnd"
          :placeholder="pi.queryPh('establishmentDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closingDateStart')">
      <a-form-item :label="pi.queryLabel('closingDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.closingDateStart"
          :placeholder="pi.queryPh('closingDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closingDateEnd')">
      <a-form-item :label="pi.queryLabel('closingDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.closingDateEnd"
          :placeholder="pi.queryPh('closingDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyExistence')">
      <a-form-item :label="pi.queryLabel('companyExistence')">
        <TaktSelect
          v-model:value="advancedQueryForm.companyExistence"
          dict-type="sys_entity_existence_status"
          :placeholder="pi.queryPh('companyExistence', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('codeAlias')">
      <a-form-item :label="pi.queryLabel('codeAlias')">
        <a-input
          v-model:value="advancedQueryForm.codeAlias"
          :placeholder="pi.queryPh('codeAlias', 'required')"
          show-count
          :maxlength="3"
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
      <div v-show="isFieldVisible('chartOfAccounts')">
      <a-form-item :label="pi.queryLabel('chartOfAccounts')">
        <TaktSelect
          v-model:value="advancedQueryForm.chartOfAccounts"
          dict-type="accounting_financial_chart_of_accounts"
          :placeholder="pi.queryPh('chartOfAccounts', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inputTaxCode')">
      <a-form-item :label="pi.queryLabel('inputTaxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.inputTaxCode"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('inputTaxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('outputTaxCode')">
      <a-form-item :label="pi.queryLabel('outputTaxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.outputTaxCode"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('outputTaxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessPlace')">
      <a-form-item :label="pi.queryLabel('businessPlace')">
        <a-input
          v-model:value="advancedQueryForm.businessPlace"
          :placeholder="pi.queryPh('businessPlace', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingPeriodVariant')">
      <a-form-item :label="pi.queryLabel('postingPeriodVariant')">
        <TaktSelect
          v-model:value="advancedQueryForm.postingPeriodVariant"
          dict-type="accounting_financial_posting_period_variant"
          :placeholder="pi.queryPh('postingPeriodVariant', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiscalYearVariant')">
      <a-form-item :label="pi.queryLabel('fiscalYearVariant')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiscalYearVariant"
          dict-type="accounting_financial_fiscal_year_variant"
          :placeholder="pi.queryPh('fiscalYearVariant', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('creditControlArea')">
      <a-form-item :label="pi.queryLabel('creditControlArea')">
        <TaktSelect
          v-model:value="advancedQueryForm.creditControlArea"
          api-url="TaktCompanies/options"
          :placeholder="pi.queryPh('creditControlArea', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('financialManagementArea')">
      <a-form-item :label="pi.queryLabel('financialManagementArea')">
        <TaktSelect
          v-model:value="advancedQueryForm.financialManagementArea"
          api-url="TaktCompanies/options"
          :placeholder="pi.queryPh('financialManagementArea', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('companyStatus')">
      <a-form-item :label="pi.queryLabel('companyStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.companyStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('companyStatus', 'select')"
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
        :entity-i18n-key="COMPANY_SELF_I18N_KEY"
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
      :id-column-key="'companyId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 公司实体 代表租户下的独立公司管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/company
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import CompanyForm from './components/CompanyForm.vue'
import { getCompanyList, getCompanyById, createCompany, updateCompany, deleteCompanyById, deleteCompanyBatch, getCompanyTemplate, importCompany, exportCompany, updateCompanyStatus } from '@/api/accounting/financial/company.api'
import type { Company, CompanyQuery } from '@/types/accounting/financial/company.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useCompanyI18n,
  COMPANY_LIST_FIELDS,
  COMPANY_QUERY_STRING_FIELDS,
  COMPANY_QUERY_FIELDS,
  COMPANY_SELF_I18N_KEY,
} from './composables/useCompanyI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCompanyI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type CompanyRowRecord = Company | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCompany')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Company[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<CompanyRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<CompanyRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Company> | null>(null)
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
  for (const key of COMPANY_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.registeredCapital !== undefined && form.registeredCapital !== null) {
    return true
  }
  if (form.companyExistence !== undefined && form.companyExistence !== null) {
    return true
  }
  if (form.companyStatus !== undefined && form.companyStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(COMPANY_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof COMPANY_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    registeredCapital: undefined as number | undefined,
    companyExistence: undefined as number | undefined,
    companyStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  COMPANY_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'companyId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CompanyQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CompanyQuery>): CompanyQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CompanyQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CompanyQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of COMPANY_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.registeredCapital !== undefined && form.registeredCapital !== null) {
    query.registeredCapital = form.registeredCapital
  }
  if (form.companyExistence !== undefined && form.companyExistence !== null) {
    query.companyExistence = form.companyExistence
  }
  if (form.companyStatus !== undefined && form.companyStatus !== null) {
    query.companyStatus = form.companyStatus
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
function buildCompanyListColumn(
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
  buildCompanyListColumn('companyId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...COMPANY_LIST_FIELDS.map((key) => buildCompanyListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:company:update',
        onClick: (record: CompanyRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:company:delete',
        onClick: (record: CompanyRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCompanyId = (record: CompanyRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getCompanyDictValue = (
  record: CompanyRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 将行字段/字典值转为有限 number */
const toCompanyNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CompanyRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CompanyRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getCompanyId(selecmedRow.value) === getCompanyId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: CompanyRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CompanyRowRecord) => ({
  onClick: () => {
    const key = getCompanyId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getCompanyId(item)))
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
    const res = await getCompanyList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Company] 加载数据失败', { error })
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
async function handleEdit(record: CompanyRowRecord) {
  const id = getCompanyId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getCompanyById(id)
    formData.value = detail ?? ({ ...record } as Partial<Company>)
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
      await updateCompany(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCompany(payload as any)
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
  const res = await getCompanyTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCompany(file, sheetName)
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
    const exportMeta = await exportCompany(
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
    logger.error('[Company] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CompanyRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCompanyById((record as any)[entityIdName])
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
      await deleteCompanyBatch(ids)
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
async function handleCompanyStatusChange(record: CompanyRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toCompanyNumber(getCompanyDictValue(record, 'companyStatus'))
  const id = getCompanyId(record)
  const row = dataSource.value.find((item) => getCompanyId(item) === id)
  if (row) {
    row.companyStatus = newVal
  }
  try {
    await updateCompanyStatus({ companyId: id, companyStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.companyStatus = oldVal
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

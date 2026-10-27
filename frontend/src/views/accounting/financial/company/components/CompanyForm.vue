<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/company/components -->
<!-- 文件名称：CompanyForm.vue -->
<!-- 功能描述：公司实体 代表租户下的独立公司维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="company-form-tabs"
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
                :label="pi.label('relatedPlant')"
                name="relatedPlant"
              >
                <TaktSelect
                  v-model:value="formState.relatedPlant"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('relatedPlant')"
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
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyName1')"
                name="companyName1"
              >
                <a-input
                  v-model:value="formState.companyName1"
                  :placeholder="pi.ph('companyName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyName2')"
                name="companyName2"
              >
                <a-input
                  v-model:value="formState.companyName2"
                  :placeholder="pi.ph('companyName2')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyShortName')"
                name="companyShortName"
              >
                <a-input
                  v-model:value="formState.companyShortName"
                  :placeholder="pi.ph('companyShortName')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                :label="pi.label('enterpriseScale')"
                name="enterpriseScale"
              >
                <TaktSelect
                  v-model:value="formState.enterpriseScale"
                  dict-type="sys_enterprise_scale"
                  :placeholder="pi.ph('enterpriseScale')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessScope')"
                name="businessScope"
              >
                <a-textarea
                  v-model:value="formState.businessScope"
                  :placeholder="pi.ph('businessScope')"
                  :rows="2"
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
                :label="pi.label('registrationRegion')"
                name="registrationRegion"
              >
                <TaktSelect
                  v-model:value="formState.registrationRegion"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('registrationRegion')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessRegion')"
                name="businessRegion"
              >
                <TaktSelect
                  v-model:value="formState.businessRegion"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('businessRegion')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessProvince')"
                name="businessProvince"
              >
                <TaktSelect
                  v-model:value="formState.businessProvince"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 2 }"
                  :placeholder="pi.ph('businessProvince')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessCity')"
                name="businessCity"
              >
                <TaktSelect
                  v-model:value="formState.businessCity"
                  api-url="TaktAdminDivisions/options"
                  :api-params="{ level: 3 }"
                  :placeholder="pi.ph('businessCity')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessAddress1')"
                name="businessAddress1"
              >
                <a-textarea
                  v-model:value="formState.businessAddress1"
                  :placeholder="pi.ph('businessAddress1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('businessAddress2')"
                name="businessAddress2"
              >
                <a-textarea
                  v-model:value="formState.businessAddress2"
                  :placeholder="pi.ph('businessAddress2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyPhone')"
                name="companyPhone"
              >
                <a-input
                  v-model:value="formState.companyPhone"
                  :placeholder="pi.ph('companyPhone')"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/6)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyEmail')"
                name="companyEmail"
              >
                <a-input
                  v-model:value="formState.companyEmail"
                  :placeholder="pi.ph('companyEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyFax')"
                name="companyFax"
              >
                <a-input
                  v-model:value="formState.companyFax"
                  :placeholder="pi.ph('companyFax')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyWebsite')"
                name="companyWebsite"
              >
                <a-input
                  v-model:value="formState.companyWebsite"
                  :placeholder="pi.ph('companyWebsite')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unifiedSocialCreditCode')"
                name="unifiedSocialCreditCode"
              >
                <a-input
                  v-model:value="formState.unifiedSocialCreditCode"
                  :placeholder="pi.ph('unifiedSocialCreditCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.companyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxRegistrationNumber')"
                name="taxRegistrationNumber"
              >
                <a-input
                  v-model:value="formState.taxRegistrationNumber"
                  :placeholder="pi.ph('taxRegistrationNumber')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('legalRepresentative')"
                name="legalRepresentative"
              >
                <a-input
                  v-model:value="formState.legalRepresentative"
                  :placeholder="pi.ph('legalRepresentative')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyManager')"
                name="companyManager"
              >
                <a-input
                  v-model:value="formState.companyManager"
                  :placeholder="pi.ph('companyManager')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('registeredCapital')"
                name="registeredCapital"
              >
                <a-input-number
                  v-model:value="formState.registeredCapital"
                  :placeholder="pi.ph('registeredCapital')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('establishmentDate')"
                name="establishmentDate"
              >
                <a-date-picker
                  v-model:value="formState.establishmentDate"
                  :placeholder="pi.ph('establishmentDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('closingDate')"
                name="closingDate"
              >
                <a-date-picker
                  v-model:value="formState.closingDate"
                  :placeholder="pi.ph('closingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                :label="pi.label('companyExistence')"
                name="companyExistence"
              >
                <TaktSelect
                  v-model:value="formState.companyExistence"
                  dict-type="sys_entity_existence_status"
                  :placeholder="pi.ph('companyExistence')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('codeAlias')"
                name="codeAlias"
              >
                <a-input
                  v-model:value="formState.codeAlias"
                  :placeholder="pi.ph('codeAlias')"
                  show-count
                  :maxlength="3"
                  allow-clear
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
                  :disabled="!!formData?.companyId"
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
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <TaktSelect
                  v-model:value="formState.currencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('currencyCode')"
                  :disabled="!!formData?.companyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('chartOfAccounts')"
                name="chartOfAccounts"
              >
                <TaktSelect
                  v-model:value="formState.chartOfAccounts"
                  dict-type="accounting_financial_chart_of_accounts"
                  :placeholder="pi.ph('chartOfAccounts')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inputTaxCode')"
                name="inputTaxCode"
              >
                <TaktSelect
                  v-model:value="formState.inputTaxCode"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('inputTaxCode')"
                  :disabled="!!formData?.companyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('outputTaxCode')"
                name="outputTaxCode"
              >
                <TaktSelect
                  v-model:value="formState.outputTaxCode"
                  api-url="TaktTaxCodes/options"
                  :placeholder="pi.ph('outputTaxCode')"
                  :disabled="!!formData?.companyId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('businessPlace')"
                name="businessPlace"
              >
                <a-input
                  v-model:value="formState.businessPlace"
                  :placeholder="pi.ph('businessPlace')"
                  show-count
                  :maxlength="4"
                  allow-clear
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
                :label="pi.label('postingPeriodVariant')"
                name="postingPeriodVariant"
              >
                <TaktSelect
                  v-model:value="formState.postingPeriodVariant"
                  dict-type="accounting_financial_posting_period_variant"
                  :placeholder="pi.ph('postingPeriodVariant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('fiscalYearVariant')"
                name="fiscalYearVariant"
              >
                <TaktSelect
                  v-model:value="formState.fiscalYearVariant"
                  dict-type="accounting_financial_fiscal_year_variant"
                  :placeholder="pi.ph('fiscalYearVariant')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('creditControlArea')"
                name="creditControlArea"
              >
                <TaktSelect
                  v-model:value="formState.creditControlArea"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('creditControlArea')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('financialManagementArea')"
                name="financialManagementArea"
              >
                <TaktSelect
                  v-model:value="formState.financialManagementArea"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('financialManagementArea')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyStatus')"
                name="companyStatus"
              >
                <TaktSelect
                  v-model:value="formState.companyStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('companyStatus')"
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
                  allow-clear
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
 * 公司实体 代表租户下的独立公司维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/company/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCompanyI18n } from '../composables/useCompanyI18n'

/** 实体字段 i18n */
const pi = useCompanyI18n()
import type { CompanyCreate } from '@/types/accounting/financial/company.types'
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
 * 上下文隔离字段：租户 / RelatedPlant（登录或公司切换注入；关联工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.relatedPlant) {
    target.relatedPlant = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CompanyCreate & { companyId?: string }> | null
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
  cultureCode: "ZH-CN",
  enterpriseNature: "150",
  industryAttribute: "C",
  enterpriseScale: "M",
  registrationRegion: "CN",
  businessRegion: "CN",
  companyExistence: 1,
  currencyCode: "CNY",
  chartOfAccounts: "INT",
  inputTaxCode: "J2",
  outputTaxCode: "J2",
  postingPeriodVariant: "0001",
  fiscalYearVariant: "K4"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 companyId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.companyId) {
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

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.companyId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  relatedPlant: [
    {
      required: true,
      message: pi.ph('relatedPlant'),
      trigger: 'change'
    }
  ],
  companyName1: [
    {
      required: true,
      message: pi.ph('companyName1'),
      trigger: 'blur'
    }
  ],
  companyShortName: [
    {
      required: true,
      message: pi.ph('companyShortName'),
      trigger: 'blur'
    }
  ],
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
  enterpriseScale: [
    {
      required: true,
      message: pi.ph('enterpriseScale'),
      trigger: 'change'
    }
  ],
  businessScope: [
    {
      required: true,
      message: pi.ph('businessScope'),
      trigger: 'blur'
    }
  ],
  registrationAddress1: [
    {
      required: true,
      message: pi.ph('registrationAddress1'),
      trigger: 'blur'
    }
  ],
  registrationRegion: [
    {
      required: true,
      message: pi.ph('registrationRegion'),
      trigger: 'change'
    }
  ],
  registrationProvince: [
    {
      required: true,
      message: pi.ph('registrationProvince'),
      trigger: 'change'
    }
  ],
  registrationCity: [
    {
      required: true,
      message: pi.ph('registrationCity'),
      trigger: 'change'
    }
  ],
  businessRegion: [
    {
      required: true,
      message: pi.ph('businessRegion'),
      trigger: 'change'
    }
  ],
  businessProvince: [
    {
      required: true,
      message: pi.ph('businessProvince'),
      trigger: 'change'
    }
  ],
  businessCity: [
    {
      required: true,
      message: pi.ph('businessCity'),
      trigger: 'change'
    }
  ],
  businessAddress1: [
    {
      required: true,
      message: pi.ph('businessAddress1'),
      trigger: 'blur'
    }
  ],
  companyPhone: [
    {
      required: true,
      message: pi.ph('companyPhone'),
      trigger: 'blur'
    }
  ],
  companyEmail: [
    {
      required: true,
      message: pi.ph('companyEmail'),
      trigger: 'blur'
    }
  ],
  companyFax: [
    {
      required: true,
      message: pi.ph('companyFax'),
      trigger: 'blur'
    }
  ],
  companyWebsite: [
    {
      required: true,
      message: pi.ph('companyWebsite'),
      trigger: 'blur'
    }
  ],
  unifiedSocialCreditCode: [
    {
      required: true,
      message: pi.ph('unifiedSocialCreditCode'),
      trigger: 'blur'
    }
  ],
  taxRegistrationNumber: [
    {
      required: true,
      message: pi.ph('taxRegistrationNumber'),
      trigger: 'blur'
    }
  ],
  legalRepresentative: [
    {
      required: true,
      message: pi.ph('legalRepresentative'),
      trigger: 'blur'
    }
  ],
  companyManager: [
    {
      required: true,
      message: pi.ph('companyManager'),
      trigger: 'blur'
    }
  ],
  registeredCapital: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('registeredCapital'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('registeredCapital'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  establishmentDate: [
    {
      required: true,
      message: pi.ph('establishmentDate'),
      trigger: 'change'
    }
  ],
  companyExistence: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('companyExistence'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('companyExistence'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  codeAlias: [
    {
      required: true,
      message: pi.ph('codeAlias'),
      trigger: 'blur'
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
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'change'
    }
  ],
  chartOfAccounts: [
    {
      required: true,
      message: pi.ph('chartOfAccounts'),
      trigger: 'change'
    }
  ],
  inputTaxCode: [
    {
      required: true,
      message: pi.ph('inputTaxCode'),
      trigger: 'change'
    }
  ],
  outputTaxCode: [
    {
      required: true,
      message: pi.ph('outputTaxCode'),
      trigger: 'change'
    }
  ],
  businessPlace: [
    {
      required: true,
      message: pi.ph('businessPlace'),
      trigger: 'blur'
    }
  ],
  postingPeriodVariant: [
    {
      required: true,
      message: pi.ph('postingPeriodVariant'),
      trigger: 'change'
    }
  ],
  fiscalYearVariant: [
    {
      required: true,
      message: pi.ph('fiscalYearVariant'),
      trigger: 'change'
    }
  ],
  creditControlArea: [
    {
      required: true,
      message: pi.ph('creditControlArea'),
      trigger: 'change'
    }
  ],
  financialManagementArea: [
    {
      required: true,
      message: pi.ph('financialManagementArea'),
      trigger: 'change'
    }
  ],
  companyStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('companyStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('companyStatus'))
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
  if ('registeredCapital' in payload) {
    const rawregisteredCapital = payload.registeredCapital
    payload.registeredCapital = typeof rawregisteredCapital === 'number' ? rawregisteredCapital : Number(rawregisteredCapital)
  }
  if ('companyExistence' in payload) {
    const rawcompanyExistence = payload.companyExistence
    payload.companyExistence = typeof rawcompanyExistence === 'number' ? rawcompanyExistence : Number(rawcompanyExistence)
  }
  if ('companyStatus' in payload) {
    const rawcompanyStatus = payload.companyStatus
    payload.companyStatus = typeof rawcompanyStatus === 'number' ? rawcompanyStatus : Number(rawcompanyStatus)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.companyId)

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

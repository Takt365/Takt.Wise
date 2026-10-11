<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/payslip/components -->
<!-- 文件名称：PayslipForm.vue -->
<!-- 功能描述：员工工资条维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="payslip-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.employeename') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.payperiod')"
                name="payPeriod"
              >
                <a-input
                  v-model:value="formState.payPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.payperiod') })"
                  show-count
                  :maxlength="16"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.basesalary')"
                name="baseSalary"
              >
                <a-input-number
                  v-model:value="formState.baseSalary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.basesalary') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.positionsalary')"
                name="positionSalary"
              >
                <a-input-number
                  v-model:value="formState.positionSalary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.positionsalary') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.bonusamount')"
                name="bonusAmount"
              >
                <a-input-number
                  v-model:value="formState.bonusAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.bonusamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.overtimepay')"
                name="overtimePay"
              >
                <a-input-number
                  v-model:value="formState.overtimePay"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.overtimepay') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.allowancetotal')"
                name="allowanceTotal"
              >
                <a-input-number
                  v-model:value="formState.allowanceTotal"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.allowancetotal') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.grossamount')"
                name="grossAmount"
              >
                <a-input-number
                  v-model:value="formState.grossAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.grossamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.socialsecuritydeduction')"
                name="socialSecurityDeduction"
              >
                <a-input-number
                  v-model:value="formState.socialSecurityDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.socialsecuritydeduction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.housingfunddeduction')"
                name="housingFundDeduction"
              >
                <a-input-number
                  v-model:value="formState.housingFundDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.housingfunddeduction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.taxdeduction')"
                name="taxDeduction"
              >
                <a-input-number
                  v-model:value="formState.taxDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.taxdeduction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.otherdeduction')"
                name="otherDeduction"
              >
                <a-input-number
                  v-model:value="formState.otherDeduction"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.otherdeduction') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.netamount')"
                name="netAmount"
              >
                <a-input-number
                  v-model:value="formState.netAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.netamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.formulasetcode')"
                name="formulaSetCode"
              >
                <a-input
                  v-model:value="formState.formulaSetCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.formulasetcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.payslipId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.issuestatus')"
                name="issueStatus"
              >
                <TaktSelect
                  v-model:value="formState.issueStatus"
                  dict-type="humanresource_compensation_payslip_issue_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.payslip.issuestatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.payslip.issuedate')"
                name="issueDate"
              >
                <a-date-picker
                  v-model:value="formState.issueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.payslip.issuedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.payslip.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.payslip.relatedplant') })"
                  show-count
                  :maxlength="4"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
 * 员工工资条维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/payslip/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PayslipCreate } from '@/types/human-resource/compensation/payslip.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","employeeId","employeeName","payPeriod","baseSalary","positionSalary","bonusAmount","overtimePay","allowanceTotal","grossAmount","socialSecurityDeduction","housingFundDeduction","taxDeduction","otherDeduction","netAmount","formulaSetCode","issueStatus","issueDate","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PayslipCreate & { payslipId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 payslipId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.payslipId) {
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
    const isCreate = !props.formData?.payslipId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.employeename') }),
      trigger: 'blur'
    }
  ],
  payPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.payslip.payperiod') }),
      trigger: 'blur'
    }
  ],
  baseSalary: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.basesalary') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.basesalary') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  positionSalary: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.positionsalary') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.positionsalary') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  bonusAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.bonusamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.bonusamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimePay: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.overtimepay') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.overtimepay') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  allowanceTotal: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.allowancetotal') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.allowancetotal') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  grossAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.grossamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.grossamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  socialSecurityDeduction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.socialsecuritydeduction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.socialsecuritydeduction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  housingFundDeduction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.housingfunddeduction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.housingfunddeduction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxDeduction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.taxdeduction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.taxdeduction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherDeduction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.otherdeduction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.otherdeduction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  netAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.netamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.netamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issueStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.issuestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.payslip.issuestatus') }))
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
  if ('baseSalary' in payload) {
    const rawbaseSalary = payload.baseSalary
    payload.baseSalary = typeof rawbaseSalary === 'number' ? rawbaseSalary : Number(rawbaseSalary)
  }
  if ('positionSalary' in payload) {
    const rawpositionSalary = payload.positionSalary
    payload.positionSalary = typeof rawpositionSalary === 'number' ? rawpositionSalary : Number(rawpositionSalary)
  }
  if ('bonusAmount' in payload) {
    const rawbonusAmount = payload.bonusAmount
    payload.bonusAmount = typeof rawbonusAmount === 'number' ? rawbonusAmount : Number(rawbonusAmount)
  }
  if ('overtimePay' in payload) {
    const rawovertimePay = payload.overtimePay
    payload.overtimePay = typeof rawovertimePay === 'number' ? rawovertimePay : Number(rawovertimePay)
  }
  if ('allowanceTotal' in payload) {
    const rawallowanceTotal = payload.allowanceTotal
    payload.allowanceTotal = typeof rawallowanceTotal === 'number' ? rawallowanceTotal : Number(rawallowanceTotal)
  }
  if ('grossAmount' in payload) {
    const rawgrossAmount = payload.grossAmount
    payload.grossAmount = typeof rawgrossAmount === 'number' ? rawgrossAmount : Number(rawgrossAmount)
  }
  if ('socialSecurityDeduction' in payload) {
    const rawsocialSecurityDeduction = payload.socialSecurityDeduction
    payload.socialSecurityDeduction = typeof rawsocialSecurityDeduction === 'number' ? rawsocialSecurityDeduction : Number(rawsocialSecurityDeduction)
  }
  if ('housingFundDeduction' in payload) {
    const rawhousingFundDeduction = payload.housingFundDeduction
    payload.housingFundDeduction = typeof rawhousingFundDeduction === 'number' ? rawhousingFundDeduction : Number(rawhousingFundDeduction)
  }
  if ('taxDeduction' in payload) {
    const rawtaxDeduction = payload.taxDeduction
    payload.taxDeduction = typeof rawtaxDeduction === 'number' ? rawtaxDeduction : Number(rawtaxDeduction)
  }
  if ('otherDeduction' in payload) {
    const rawotherDeduction = payload.otherDeduction
    payload.otherDeduction = typeof rawotherDeduction === 'number' ? rawotherDeduction : Number(rawotherDeduction)
  }
  if ('netAmount' in payload) {
    const rawnetAmount = payload.netAmount
    payload.netAmount = typeof rawnetAmount === 'number' ? rawnetAmount : Number(rawnetAmount)
  }
  if ('issueStatus' in payload) {
    const rawissueStatus = payload.issueStatus
    payload.issueStatus = typeof rawissueStatus === 'number' ? rawissueStatus : Number(rawissueStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.payslipId)

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

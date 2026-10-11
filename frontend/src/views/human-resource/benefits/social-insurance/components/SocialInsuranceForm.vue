<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/benefits/social-insurance/components -->
<!-- 文件名称：SocialInsuranceForm.vue -->
<!-- 功能描述：社保与公积金月度缴纳流水维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="social-insurance-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.socialinsurance.benefititemid')"
                name="benefitItemId"
              >
                <a-input
                  v-model:value="formState.benefitItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.benefititemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeename') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.payperiod')"
                name="payPeriod"
              >
                <a-input
                  v-model:value="formState.payPeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.payperiod') })"
                  show-count
                  :maxlength="16"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.socialsecuritybase')"
                name="socialSecurityBase"
              >
                <a-input-number
                  v-model:value="formState.socialSecurityBase"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.socialsecuritybase') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.pensionamount')"
                name="pensionAmount"
              >
                <a-input-number
                  v-model:value="formState.pensionAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.pensionamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.medicalamount')"
                name="medicalAmount"
              >
                <a-input-number
                  v-model:value="formState.medicalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.medicalamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.unemploymentamount')"
                name="unemploymentAmount"
              >
                <a-input-number
                  v-model:value="formState.unemploymentAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.unemploymentamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.injuryamount')"
                name="injuryAmount"
              >
                <a-input-number
                  v-model:value="formState.injuryAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.injuryamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.maternityamount')"
                name="maternityAmount"
              >
                <a-input-number
                  v-model:value="formState.maternityAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.maternityamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.housingfundbase')"
                name="housingFundBase"
              >
                <a-input-number
                  v-model:value="formState.housingFundBase"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.housingfundbase') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.housingfundamount')"
                name="housingFundAmount"
              >
                <a-input-number
                  v-model:value="formState.housingFundAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.housingfundamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.totalamount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.totalamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.paystatus')"
                name="payStatus"
              >
                <TaktSelect
                  v-model:value="formState.payStatus"
                  dict-type="humanresource_benefits_social_insurance_pay_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.paystatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.socialinsurance.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.relatedplant') })"
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
 * 社保与公积金月度缴纳流水维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/benefits/social-insurance/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SocialInsuranceCreate } from '@/types/human-resource/benefits/socialInsurance.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","benefitItemId","employeeId","employeeName","payPeriod","socialSecurityBase","pensionAmount","medicalAmount","unemploymentAmount","injuryAmount","maternityAmount","housingFundBase","housingFundAmount","totalAmount","payStatus","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SocialInsuranceCreate & { socialInsuranceId?: string }> | null
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
  payStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 socialInsuranceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.socialInsuranceId) {
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
    const isCreate = !props.formData?.socialInsuranceId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.employeename') }),
      trigger: 'blur'
    }
  ],
  payPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.socialinsurance.payperiod') }),
      trigger: 'blur'
    }
  ],
  socialSecurityBase: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.socialsecuritybase') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.socialsecuritybase') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pensionAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.pensionamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.pensionamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  medicalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.medicalamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.medicalamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unemploymentAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.unemploymentamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.unemploymentamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  injuryAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.injuryamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.injuryamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maternityAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.maternityamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.maternityamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  housingFundBase: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.housingfundbase') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.housingfundbase') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  housingFundAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.housingfundamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.housingfundamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.totalamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.totalamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  payStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.paystatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.socialinsurance.paystatus') }))
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
  if ('socialSecurityBase' in payload) {
    const rawsocialSecurityBase = payload.socialSecurityBase
    payload.socialSecurityBase = typeof rawsocialSecurityBase === 'number' ? rawsocialSecurityBase : Number(rawsocialSecurityBase)
  }
  if ('pensionAmount' in payload) {
    const rawpensionAmount = payload.pensionAmount
    payload.pensionAmount = typeof rawpensionAmount === 'number' ? rawpensionAmount : Number(rawpensionAmount)
  }
  if ('medicalAmount' in payload) {
    const rawmedicalAmount = payload.medicalAmount
    payload.medicalAmount = typeof rawmedicalAmount === 'number' ? rawmedicalAmount : Number(rawmedicalAmount)
  }
  if ('unemploymentAmount' in payload) {
    const rawunemploymentAmount = payload.unemploymentAmount
    payload.unemploymentAmount = typeof rawunemploymentAmount === 'number' ? rawunemploymentAmount : Number(rawunemploymentAmount)
  }
  if ('injuryAmount' in payload) {
    const rawinjuryAmount = payload.injuryAmount
    payload.injuryAmount = typeof rawinjuryAmount === 'number' ? rawinjuryAmount : Number(rawinjuryAmount)
  }
  if ('maternityAmount' in payload) {
    const rawmaternityAmount = payload.maternityAmount
    payload.maternityAmount = typeof rawmaternityAmount === 'number' ? rawmaternityAmount : Number(rawmaternityAmount)
  }
  if ('housingFundBase' in payload) {
    const rawhousingFundBase = payload.housingFundBase
    payload.housingFundBase = typeof rawhousingFundBase === 'number' ? rawhousingFundBase : Number(rawhousingFundBase)
  }
  if ('housingFundAmount' in payload) {
    const rawhousingFundAmount = payload.housingFundAmount
    payload.housingFundAmount = typeof rawhousingFundAmount === 'number' ? rawhousingFundAmount : Number(rawhousingFundAmount)
  }
  if ('totalAmount' in payload) {
    const rawtotalAmount = payload.totalAmount
    payload.totalAmount = typeof rawtotalAmount === 'number' ? rawtotalAmount : Number(rawtotalAmount)
  }
  if ('payStatus' in payload) {
    const rawpayStatus = payload.payStatus
    payload.payStatus = typeof rawpayStatus === 'number' ? rawpayStatus : Number(rawpayStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.socialInsuranceId)

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

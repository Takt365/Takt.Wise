<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/customer-service/contract/components -->
<!-- 文件名称：ContractForm.vue -->
<!-- 功能描述：服务合同实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="contract-form-tabs"
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
                :label="pi.label('serviceContractCode')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="pi.ph('serviceContractCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerServiceContractId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contractName')"
                name="contractName"
              >
                <a-input
                  v-model:value="formState.contractName"
                  :placeholder="pi.ph('contractName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientId')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="pi.ph('clientId')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :placeholder="pi.ph('clientCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.customerServiceContractId"
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
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contractType')"
                name="contractType"
              >
                <a-input-number
                  v-model:value="formState.contractType"
                  :placeholder="pi.ph('contractType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contractStatus')"
                name="contractStatus"
              >
                <a-input-number
                  v-model:value="formState.contractStatus"
                  :placeholder="pi.ph('contractStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('signDate')"
                name="signDate"
              >
                <a-date-picker
                  v-model:value="formState.signDate"
                  :placeholder="pi.ph('signDate')"
                  value-format="YYYY-MM-DD"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('effectiveDate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="pi.ph('effectiveDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('expiryDate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="pi.ph('expiryDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('contractAmount')"
                name="contractAmount"
              >
                <a-input-number
                  v-model:value="formState.contractAmount"
                  :placeholder="pi.ph('contractAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('currencyCode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="pi.ph('currencyCode')"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.customerServiceContractId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('paymentTerms')"
                name="paymentTerms"
              >
                <a-input-number
                  v-model:value="formState.paymentTerms"
                  :placeholder="pi.ph('paymentTerms')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('serviceScope')"
                name="serviceScope"
              >
                <a-textarea
                  v-model:value="formState.serviceScope"
                  :placeholder="pi.ph('serviceScope')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('slaResponseHours')"
                name="slaResponseHours"
              >
                <a-input-number
                  v-model:value="formState.slaResponseHours"
                  :placeholder="pi.ph('slaResponseHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('slaResolveHours')"
                name="slaResolveHours"
              >
                <a-input-number
                  v-model:value="formState.slaResolveHours"
                  :placeholder="pi.ph('slaResolveHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('accountManager')"
                name="accountManager"
              >
                <a-input
                  v-model:value="formState.accountManager"
                  :placeholder="pi.ph('accountManager')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
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
 * 服务合同实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/customer-service/contract/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerServiceContractI18n } from '../composables/useContractI18n'

/** 实体字段 i18n */
const pi = useCustomerServiceContractI18n()
import type { CustomerServiceContractCreate } from '@/types/logistics/customer-service/contract.types'
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
  formData?: Partial<CustomerServiceContractCreate & { customerServiceContractId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerServiceContractId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerServiceContractId) {
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
    if (!props.formData?.customerServiceContractId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  serviceContractCode: [
    {
      required: true,
      message: pi.ph('serviceContractCode'),
      trigger: 'blur'
    }
  ],
  contractName: [
    {
      required: true,
      message: pi.ph('contractName'),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: pi.ph('clientId'),
      trigger: 'blur'
    }
  ],
  contractType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('contractType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('contractType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  contractStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('contractStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('contractStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  effectiveDate: [
    {
      required: true,
      message: pi.ph('effectiveDate'),
      trigger: 'change'
    }
  ],
  contractAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('contractAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('contractAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: pi.ph('currencyCode'),
      trigger: 'blur'
    }
  ],
  paymentTerms: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('paymentTerms'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('paymentTerms'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  slaResponseHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('slaResponseHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('slaResponseHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  slaResolveHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('slaResolveHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('slaResolveHours'))
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
  if ('contractType' in payload) {
    const rawcontractType = payload.contractType
    if (rawcontractType === undefined || rawcontractType === null || rawcontractType === '') {
      delete payload.contractType
    } else {
      const numcontractType = typeof rawcontractType === 'number' ? rawcontractType : Number(rawcontractType)
      if (Number.isFinite(numcontractType)) payload.contractType = numcontractType
      else delete payload.contractType
    }
  }
  if ('contractStatus' in payload) {
    const rawcontractStatus = payload.contractStatus
    if (rawcontractStatus === undefined || rawcontractStatus === null || rawcontractStatus === '') {
      delete payload.contractStatus
    } else {
      const numcontractStatus = typeof rawcontractStatus === 'number' ? rawcontractStatus : Number(rawcontractStatus)
      if (Number.isFinite(numcontractStatus)) payload.contractStatus = numcontractStatus
      else delete payload.contractStatus
    }
  }
  if ('contractAmount' in payload) {
    const rawcontractAmount = payload.contractAmount
    if (rawcontractAmount === undefined || rawcontractAmount === null || rawcontractAmount === '') {
      delete payload.contractAmount
    } else {
      const numcontractAmount = typeof rawcontractAmount === 'number' ? rawcontractAmount : Number(rawcontractAmount)
      if (Number.isFinite(numcontractAmount)) payload.contractAmount = numcontractAmount
      else delete payload.contractAmount
    }
  }
  if ('paymentTerms' in payload) {
    const rawpaymentTerms = payload.paymentTerms
    if (rawpaymentTerms === undefined || rawpaymentTerms === null || rawpaymentTerms === '') {
      delete payload.paymentTerms
    } else {
      const numpaymentTerms = typeof rawpaymentTerms === 'number' ? rawpaymentTerms : Number(rawpaymentTerms)
      if (Number.isFinite(numpaymentTerms)) payload.paymentTerms = numpaymentTerms
      else delete payload.paymentTerms
    }
  }
  if ('slaResponseHours' in payload) {
    const rawslaResponseHours = payload.slaResponseHours
    if (rawslaResponseHours === undefined || rawslaResponseHours === null || rawslaResponseHours === '') {
      delete payload.slaResponseHours
    } else {
      const numslaResponseHours = typeof rawslaResponseHours === 'number' ? rawslaResponseHours : Number(rawslaResponseHours)
      if (Number.isFinite(numslaResponseHours)) payload.slaResponseHours = numslaResponseHours
      else delete payload.slaResponseHours
    }
  }
  if ('slaResolveHours' in payload) {
    const rawslaResolveHours = payload.slaResolveHours
    if (rawslaResolveHours === undefined || rawslaResolveHours === null || rawslaResolveHours === '') {
      delete payload.slaResolveHours
    } else {
      const numslaResolveHours = typeof rawslaResolveHours === 'number' ? rawslaResolveHours : Number(rawslaResolveHours)
      if (Number.isFinite(numslaResolveHours)) payload.slaResolveHours = numslaResolveHours
      else delete payload.slaResolveHours
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.customerServiceContractId) {
    payload.customerServiceContractId = props.formData.customerServiceContractId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerServiceContractId)

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

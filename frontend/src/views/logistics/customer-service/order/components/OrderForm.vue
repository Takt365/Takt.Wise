<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/customer-service/order/components -->
<!-- 文件名称：OrderForm.vue -->
<!-- 功能描述：服务订单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="order-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
                :label="pi.label('serviceOrderCode')"
                name="serviceOrderCode"
              >
                <a-input
                  v-model:value="formState.serviceOrderCode"
                  :placeholder="pi.ph('serviceOrderCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerServiceOrderId"
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
                  disabled
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
                :label="pi.label('serviceContractId')"
                name="serviceContractId"
              >
                <a-input
                  v-model:value="formState.serviceContractId"
                  :placeholder="pi.ph('serviceContractId')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceRequestId')"
                name="serviceRequestId"
              >
                <a-input
                  v-model:value="formState.serviceRequestId"
                  :placeholder="pi.ph('serviceRequestId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceRequestCode')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="pi.ph('serviceRequestCode')"
                  show-count
                  :maxlength="50"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderDate')"
                name="orderDate"
              >
                <a-date-picker
                  v-model:value="formState.orderDate"
                  :placeholder="pi.ph('orderDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderType')"
                name="orderType"
              >
                <a-input-number
                  v-model:value="formState.orderType"
                  :placeholder="pi.ph('orderType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderStatus')"
                name="orderStatus"
              >
                <a-input-number
                  v-model:value="formState.orderStatus"
                  :placeholder="pi.ph('orderStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalAmount')"
                name="totalAmount"
              >
                <a-input-number
                  v-model:value="formState.totalAmount"
                  :placeholder="pi.ph('totalAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountAmount')"
                name="discountAmount"
              >
                <a-input-number
                  v-model:value="formState.discountAmount"
                  :placeholder="pi.ph('discountAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxAmount')"
                name="taxAmount"
              >
                <a-input-number
                  v-model:value="formState.taxAmount"
                  :placeholder="pi.ph('taxAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualAmount')"
                name="actualAmount"
              >
                <a-input-number
                  v-model:value="formState.actualAmount"
                  :placeholder="pi.ph('actualAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
                  :disabled="!!formData?.customerServiceOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedStartDate')"
                name="plannedStartDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartDate"
                  :placeholder="pi.ph('plannedStartDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedEndDate')"
                name="plannedEndDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndDate"
                  :placeholder="pi.ph('plannedEndDate')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('actualStartDate')"
                name="actualStartDate"
              >
                <a-date-picker
                  v-model:value="formState.actualStartDate"
                  :placeholder="pi.ph('actualStartDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('actualEndDate')"
                name="actualEndDate"
              >
                <a-date-picker
                  v-model:value="formState.actualEndDate"
                  :placeholder="pi.ph('actualEndDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('serviceEmployeeId')"
                name="serviceEmployeeId"
              >
                <TaktSelect
                  v-model:value="formState.serviceEmployeeId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('serviceEmployeeId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('serviceEmployeeName')"
                name="serviceEmployeeName"
              >
                <a-input
                  v-model:value="formState.serviceEmployeeName"
                  :placeholder="pi.ph('serviceEmployeeName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
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
 * 服务订单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/customer-service/order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerServiceOrderI18n } from '../composables/useOrderI18n'

/** 实体字段 i18n */
const pi = useCustomerServiceOrderI18n()
import type { CustomerServiceOrderCreate } from '@/types/logistics/customer-service/order.types'
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
  formData?: Partial<CustomerServiceOrderCreate & { customerServiceOrderId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerServiceOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerServiceOrderId) {
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
    if (!props.formData?.customerServiceOrderId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  serviceOrderCode: [
    {
      required: true,
      message: pi.ph('serviceOrderCode'),
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
  orderDate: [
    {
      required: true,
      message: pi.ph('orderDate'),
      trigger: 'change'
    }
  ],
  orderType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  orderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  discountAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualAmount'))
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
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('orderType' in payload) {
    const raworderType = payload.orderType
    if (raworderType === undefined || raworderType === null || raworderType === '') {
      delete payload.orderType
    } else {
      const numorderType = typeof raworderType === 'number' ? raworderType : Number(raworderType)
      if (Number.isFinite(numorderType)) payload.orderType = numorderType
      else delete payload.orderType
    }
  }
  if ('orderStatus' in payload) {
    const raworderStatus = payload.orderStatus
    if (raworderStatus === undefined || raworderStatus === null || raworderStatus === '') {
      delete payload.orderStatus
    } else {
      const numorderStatus = typeof raworderStatus === 'number' ? raworderStatus : Number(raworderStatus)
      if (Number.isFinite(numorderStatus)) payload.orderStatus = numorderStatus
      else delete payload.orderStatus
    }
  }
  if ('totalAmount' in payload) {
    const rawtotalAmount = payload.totalAmount
    if (rawtotalAmount === undefined || rawtotalAmount === null || rawtotalAmount === '') {
      delete payload.totalAmount
    } else {
      const numtotalAmount = typeof rawtotalAmount === 'number' ? rawtotalAmount : Number(rawtotalAmount)
      if (Number.isFinite(numtotalAmount)) payload.totalAmount = numtotalAmount
      else delete payload.totalAmount
    }
  }
  if ('discountAmount' in payload) {
    const rawdiscountAmount = payload.discountAmount
    if (rawdiscountAmount === undefined || rawdiscountAmount === null || rawdiscountAmount === '') {
      delete payload.discountAmount
    } else {
      const numdiscountAmount = typeof rawdiscountAmount === 'number' ? rawdiscountAmount : Number(rawdiscountAmount)
      if (Number.isFinite(numdiscountAmount)) payload.discountAmount = numdiscountAmount
      else delete payload.discountAmount
    }
  }
  if ('taxAmount' in payload) {
    const rawtaxAmount = payload.taxAmount
    if (rawtaxAmount === undefined || rawtaxAmount === null || rawtaxAmount === '') {
      delete payload.taxAmount
    } else {
      const numtaxAmount = typeof rawtaxAmount === 'number' ? rawtaxAmount : Number(rawtaxAmount)
      if (Number.isFinite(numtaxAmount)) payload.taxAmount = numtaxAmount
      else delete payload.taxAmount
    }
  }
  if ('actualAmount' in payload) {
    const rawactualAmount = payload.actualAmount
    if (rawactualAmount === undefined || rawactualAmount === null || rawactualAmount === '') {
      delete payload.actualAmount
    } else {
      const numactualAmount = typeof rawactualAmount === 'number' ? rawactualAmount : Number(rawactualAmount)
      if (Number.isFinite(numactualAmount)) payload.actualAmount = numactualAmount
      else delete payload.actualAmount
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.customerServiceOrderId) {
    payload.customerServiceOrderId = props.formData.customerServiceOrderId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerServiceOrderId)

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

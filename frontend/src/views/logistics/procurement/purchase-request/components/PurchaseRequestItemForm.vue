<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-request/components -->
<!-- 文件名称：PurchaseRequestItemForm.vue -->
<!-- 功能描述：Takt采购申请实体子表 purchaseRequestItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-request-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-request-item-form-tabs"
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
                :label="pi.label('purchasePlanItemId')"
                name="purchasePlanItemId"
              >
                <a-input
                  v-model:value="formState.purchasePlanItemId"
                  :placeholder="pi.ph('purchasePlanItemId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('allocationCategory')"
                name="allocationCategory"
              >
                <TaktSelect
                  v-model:value="formState.allocationCategory"
                  dict-type="logistics_sales_allocation_category"
                  :placeholder="pi.ph('allocationCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.purchaseRequestItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestUnit')"
                name="requestUnit"
              >
                <TaktSelect
                  v-model:value="formState.requestUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('requestUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestQuantity')"
                name="requestQuantity"
              >
                <a-input-number
                  v-model:value="formState.requestQuantity"
                  :placeholder="pi.ph('requestQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('convertedQuantity')"
                name="convertedQuantity"
              >
                <a-input-number
                  v-model:value="formState.convertedQuantity"
                  :placeholder="pi.ph('convertedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchasePerUnit')"
                name="purchasePerUnit"
              >
                <TaktSelect
                  v-model:value="formState.purchasePerUnit"
                  dict-type="logistics_materials_price_unit_param"
                  :placeholder="pi.ph('purchasePerUnit')"
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
                :label="pi.label('purchaseRequestUnitPrice')"
                name="purchaseRequestUnitPrice"
              >
                <a-input-number
                  v-model:value="formState.purchaseRequestUnitPrice"
                  :placeholder="pi.ph('purchaseRequestUnitPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxIncludedAmount')"
                name="taxIncludedAmount"
              >
                <a-input-number
                  v-model:value="formState.taxIncludedAmount"
                  :placeholder="pi.ph('taxIncludedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('untaxedAmount')"
                name="untaxedAmount"
              >
                <a-input-number
                  v-model:value="formState.untaxedAmount"
                  :placeholder="pi.ph('untaxedAmount')"
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
                :label="pi.label('requestAmount')"
                name="requestAmount"
              >
                <a-input-number
                  v-model:value="formState.requestAmount"
                  :placeholder="pi.ph('requestAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pricingDate')"
                name="pricingDate"
              >
                <a-date-picker
                  v-model:value="formState.pricingDate"
                  :placeholder="pi.ph('pricingDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossWeight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="pi.ph('grossWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netWeight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="pi.ph('netWeight')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('weightUnit')"
                name="weightUnit"
              >
                <TaktSelect
                  v-model:value="formState.weightUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('weightUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volume')"
                name="volume"
              >
                <a-input-number
                  v-model:value="formState.volume"
                  :placeholder="pi.ph('volume')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('volumeUnit')"
                name="volumeUnit"
              >
                <TaktSelect
                  v-model:value="formState.volumeUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('volumeUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('profitCenterCode')"
                name="profitCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.profitCenterCode"
                  api-url="TaktProfitCenters/options"
                  :placeholder="pi.ph('profitCenterCode')"
                  :disabled="!!formData?.purchaseRequestItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isObsolete')"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt采购申请实体子表 purchaseRequestItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/procurement/purchase-request/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseRequestItemI18n } from '../composables/usePurchaseRequestItemI18n'

/** 实体字段 i18n */
const pi = usePurchaseRequestItemI18n()

import type { PurchaseRequestItemCreate } from '@/types/logistics/procurement/purchaseRequestItem.types'
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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchasePlanItemId","lineNumber","allocationCategory","materialCode","requestUnit","requestQuantity","convertedQuantity","purchasePerUnit","purchaseRequestUnitPrice","taxIncludedAmount","untaxedAmount","taxAmount","requestAmount","pricingDate","grossWeight","netWeight","weightUnit","volume","volumeUnit","profitCenterCode","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseRequestItemCreate & { purchaseRequestItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  purchasePerUnit: 1000
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseRequestItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseRequestItemId) {
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
    if (!props.formData?.purchaseRequestItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  allocationCategory: [
    {
      required: true,
      message: pi.ph('allocationCategory'),
      trigger: 'change'
    }
  ],
  requestUnit: [
    {
      required: true,
      message: pi.ph('requestUnit'),
      trigger: 'change'
    }
  ],
  requestQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  convertedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchasePerUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchasePerUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchasePerUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseRequestUnitPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseRequestUnitPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseRequestUnitPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxIncludedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxIncludedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxIncludedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  untaxedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('untaxedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('untaxedAmount'))
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
  requestAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
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

/** 映射为 Create/Update DTO（含主表外键 purchaseRequestId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    if (rawlineNumber === undefined || rawlineNumber === null || rawlineNumber === '') {
      delete payload.lineNumber
    } else {
      const numlineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
      if (Number.isFinite(numlineNumber)) payload.lineNumber = numlineNumber
      else delete payload.lineNumber
    }
  }
  if ('requestQuantity' in payload) {
    const rawrequestQuantity = payload.requestQuantity
    if (rawrequestQuantity === undefined || rawrequestQuantity === null || rawrequestQuantity === '') {
      delete payload.requestQuantity
    } else {
      const numrequestQuantity = typeof rawrequestQuantity === 'number' ? rawrequestQuantity : Number(rawrequestQuantity)
      if (Number.isFinite(numrequestQuantity)) payload.requestQuantity = numrequestQuantity
      else delete payload.requestQuantity
    }
  }
  if ('convertedQuantity' in payload) {
    const rawconvertedQuantity = payload.convertedQuantity
    if (rawconvertedQuantity === undefined || rawconvertedQuantity === null || rawconvertedQuantity === '') {
      delete payload.convertedQuantity
    } else {
      const numconvertedQuantity = typeof rawconvertedQuantity === 'number' ? rawconvertedQuantity : Number(rawconvertedQuantity)
      if (Number.isFinite(numconvertedQuantity)) payload.convertedQuantity = numconvertedQuantity
      else delete payload.convertedQuantity
    }
  }
  if ('purchasePerUnit' in payload) {
    const rawpurchasePerUnit = payload.purchasePerUnit
    if (rawpurchasePerUnit === undefined || rawpurchasePerUnit === null || rawpurchasePerUnit === '') {
      delete payload.purchasePerUnit
    } else {
      const numpurchasePerUnit = typeof rawpurchasePerUnit === 'number' ? rawpurchasePerUnit : Number(rawpurchasePerUnit)
      if (Number.isFinite(numpurchasePerUnit)) payload.purchasePerUnit = numpurchasePerUnit
      else delete payload.purchasePerUnit
    }
  }
  if ('purchaseRequestUnitPrice' in payload) {
    const rawpurchaseRequestUnitPrice = payload.purchaseRequestUnitPrice
    if (rawpurchaseRequestUnitPrice === undefined || rawpurchaseRequestUnitPrice === null || rawpurchaseRequestUnitPrice === '') {
      delete payload.purchaseRequestUnitPrice
    } else {
      const numpurchaseRequestUnitPrice = typeof rawpurchaseRequestUnitPrice === 'number' ? rawpurchaseRequestUnitPrice : Number(rawpurchaseRequestUnitPrice)
      if (Number.isFinite(numpurchaseRequestUnitPrice)) payload.purchaseRequestUnitPrice = numpurchaseRequestUnitPrice
      else delete payload.purchaseRequestUnitPrice
    }
  }
  if ('taxIncludedAmount' in payload) {
    const rawtaxIncludedAmount = payload.taxIncludedAmount
    if (rawtaxIncludedAmount === undefined || rawtaxIncludedAmount === null || rawtaxIncludedAmount === '') {
      delete payload.taxIncludedAmount
    } else {
      const numtaxIncludedAmount = typeof rawtaxIncludedAmount === 'number' ? rawtaxIncludedAmount : Number(rawtaxIncludedAmount)
      if (Number.isFinite(numtaxIncludedAmount)) payload.taxIncludedAmount = numtaxIncludedAmount
      else delete payload.taxIncludedAmount
    }
  }
  if ('untaxedAmount' in payload) {
    const rawuntaxedAmount = payload.untaxedAmount
    if (rawuntaxedAmount === undefined || rawuntaxedAmount === null || rawuntaxedAmount === '') {
      delete payload.untaxedAmount
    } else {
      const numuntaxedAmount = typeof rawuntaxedAmount === 'number' ? rawuntaxedAmount : Number(rawuntaxedAmount)
      if (Number.isFinite(numuntaxedAmount)) payload.untaxedAmount = numuntaxedAmount
      else delete payload.untaxedAmount
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
  if ('requestAmount' in payload) {
    const rawrequestAmount = payload.requestAmount
    if (rawrequestAmount === undefined || rawrequestAmount === null || rawrequestAmount === '') {
      delete payload.requestAmount
    } else {
      const numrequestAmount = typeof rawrequestAmount === 'number' ? rawrequestAmount : Number(rawrequestAmount)
      if (Number.isFinite(numrequestAmount)) payload.requestAmount = numrequestAmount
      else delete payload.requestAmount
    }
  }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    if (rawgrossWeight === undefined || rawgrossWeight === null || rawgrossWeight === '') {
      delete payload.grossWeight
    } else {
      const numgrossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
      if (Number.isFinite(numgrossWeight)) payload.grossWeight = numgrossWeight
      else delete payload.grossWeight
    }
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    if (rawnetWeight === undefined || rawnetWeight === null || rawnetWeight === '') {
      delete payload.netWeight
    } else {
      const numnetWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
      if (Number.isFinite(numnetWeight)) payload.netWeight = numnetWeight
      else delete payload.netWeight
    }
  }
  if ('volume' in payload) {
    const rawvolume = payload.volume
    if (rawvolume === undefined || rawvolume === null || rawvolume === '') {
      delete payload.volume
    } else {
      const numvolume = typeof rawvolume === 'number' ? rawvolume : Number(rawvolume)
      if (Number.isFinite(numvolume)) payload.volume = numvolume
      else delete payload.volume
    }
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    if (rawisObsolete === undefined || rawisObsolete === null || rawisObsolete === '') {
      delete payload.isObsolete
    } else {
      const numisObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
      if (Number.isFinite(numisObsolete)) payload.isObsolete = numisObsolete
      else delete payload.isObsolete
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.purchaseRequestItemId) {
    payload.purchaseRequestItemId = props.formData.purchaseRequestItemId
    delete payload.numberingRuleCode
  }
  payload.purchaseRequestId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.purchaseRequestCode ?? masterRow.PurchaseRequestCode
    const masterName = masterRow.purchaseRequestName ?? masterRow.PurchaseRequestName
    if (masterCode != null && masterCode !== '' && !payload.purchaseRequestCode) {
      payload.purchaseRequestCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.purchaseRequestName) {
      payload.purchaseRequestName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseRequestItemId)
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

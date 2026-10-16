<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-order/components -->
<!-- 文件名称：PurchaseOrderItemForm.vue -->
<!-- 功能描述：Takt采购订单实体子表 purchaseOrderItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-order-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-order-item-form-tabs"
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
                :label="pi.label('requestCode')"
                name="requestCode"
              >
                <a-input
                  v-model:value="formState.requestCode"
                  :placeholder="pi.ph('requestCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestLineNumber')"
                name="requestLineNumber"
              >
                <a-input-number
                  v-model:value="formState.requestLineNumber"
                  :placeholder="pi.ph('requestLineNumber')"
                  style="width: 100%"
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
                  :disabled="!!formData?.purchaseOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseInfoCode')"
                name="purchaseInfoCode"
              >
                <a-input
                  v-model:value="formState.purchaseInfoCode"
                  :placeholder="pi.ph('purchaseInfoCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierMaterialCode')"
                name="supplierMaterialCode"
              >
                <a-input
                  v-model:value="formState.supplierMaterialCode"
                  :placeholder="pi.ph('supplierMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseUnit')"
                name="purchaseUnit"
              >
                <TaktSelect
                  v-model:value="formState.purchaseUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('purchaseUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderQuantity')"
                name="orderQuantity"
              >
                <a-input-number
                  v-model:value="formState.orderQuantity"
                  :placeholder="pi.ph('orderQuantity')"
                  style="width: 100%"
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
                :label="pi.label('receivedQuantity')"
                name="receivedQuantity"
              >
                <a-input-number
                  v-model:value="formState.receivedQuantity"
                  :placeholder="pi.ph('receivedQuantity')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseUnitPrice')"
                name="purchaseUnitPrice"
              >
                <a-input-number
                  v-model:value="formState.purchaseUnitPrice"
                  :placeholder="pi.ph('purchaseUnitPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discountRate')"
                name="discountRate"
              >
                <TaktSelect
                  v-model:value="formState.discountRate"
                  dict-type="logistics_sales_discount_rate_param"
                  :placeholder="pi.ph('discountRate')"
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
                :label="pi.label('purchaseAmount')"
                name="purchaseAmount"
              >
                <a-input-number
                  v-model:value="formState.purchaseAmount"
                  :placeholder="pi.ph('purchaseAmount')"
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
                  :disabled="!!formData?.purchaseOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deliveryStatus')"
                name="deliveryStatus"
              >
                <TaktSelect
                  v-model:value="formState.deliveryStatus"
                  dict-type="logistics_sales_delivery_status"
                  :placeholder="pi.ph('deliveryStatus')"
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
 * Takt采购订单实体子表 purchaseOrderItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/procurement/purchase-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseOrderItemI18n } from '../composables/usePurchaseOrderItemI18n'

/** 实体字段 i18n */
const pi = usePurchaseOrderItemI18n()

import type { PurchaseOrderItemCreate } from '@/types/logistics/procurement/purchaseOrderItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","requestCode","requestLineNumber","materialCode","purchaseInfoCode","supplierMaterialCode","purchaseUnit","orderQuantity","receivedQuantity","purchasePerUnit","purchaseUnitPrice","discountRate","discountAmount","taxIncludedAmount","untaxedAmount","taxAmount","purchaseAmount","pricingDate","grossWeight","netWeight","weightUnit","volume","volumeUnit","profitCenterCode","deliveryStatus","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseOrderItemCreate & { purchaseOrderItemId?: string }> | null
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
  purchasePerUnit: 1000,
  discountRate: 0,
  deliveryStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseOrderItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseOrderItemId) {
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
    if (!props.formData?.purchaseOrderItemId) {
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
  purchaseUnit: [
    {
      required: true,
      message: pi.ph('purchaseUnit'),
      trigger: 'change'
    }
  ],
  orderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('orderQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('orderQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  receivedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('receivedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('receivedQuantity'))
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
  purchaseUnitPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseUnitPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseUnitPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  discountRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('discountRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('discountRate'))
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
  purchaseAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  deliveryStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('deliveryStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('deliveryStatus'))
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

/** 映射为 Create/Update DTO（含主表外键 purchaseOrderId） */
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
  if ('requestLineNumber' in payload) {
    const rawrequestLineNumber = payload.requestLineNumber
    if (rawrequestLineNumber === undefined || rawrequestLineNumber === null || rawrequestLineNumber === '') {
      delete payload.requestLineNumber
    } else {
      const numrequestLineNumber = typeof rawrequestLineNumber === 'number' ? rawrequestLineNumber : Number(rawrequestLineNumber)
      if (Number.isFinite(numrequestLineNumber)) payload.requestLineNumber = numrequestLineNumber
      else delete payload.requestLineNumber
    }
  }
  if ('orderQuantity' in payload) {
    const raworderQuantity = payload.orderQuantity
    if (raworderQuantity === undefined || raworderQuantity === null || raworderQuantity === '') {
      delete payload.orderQuantity
    } else {
      const numorderQuantity = typeof raworderQuantity === 'number' ? raworderQuantity : Number(raworderQuantity)
      if (Number.isFinite(numorderQuantity)) payload.orderQuantity = numorderQuantity
      else delete payload.orderQuantity
    }
  }
  if ('receivedQuantity' in payload) {
    const rawreceivedQuantity = payload.receivedQuantity
    if (rawreceivedQuantity === undefined || rawreceivedQuantity === null || rawreceivedQuantity === '') {
      delete payload.receivedQuantity
    } else {
      const numreceivedQuantity = typeof rawreceivedQuantity === 'number' ? rawreceivedQuantity : Number(rawreceivedQuantity)
      if (Number.isFinite(numreceivedQuantity)) payload.receivedQuantity = numreceivedQuantity
      else delete payload.receivedQuantity
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
  if ('purchaseUnitPrice' in payload) {
    const rawpurchaseUnitPrice = payload.purchaseUnitPrice
    if (rawpurchaseUnitPrice === undefined || rawpurchaseUnitPrice === null || rawpurchaseUnitPrice === '') {
      delete payload.purchaseUnitPrice
    } else {
      const numpurchaseUnitPrice = typeof rawpurchaseUnitPrice === 'number' ? rawpurchaseUnitPrice : Number(rawpurchaseUnitPrice)
      if (Number.isFinite(numpurchaseUnitPrice)) payload.purchaseUnitPrice = numpurchaseUnitPrice
      else delete payload.purchaseUnitPrice
    }
  }
  if ('discountRate' in payload) {
    const rawdiscountRate = payload.discountRate
    if (rawdiscountRate === undefined || rawdiscountRate === null || rawdiscountRate === '') {
      delete payload.discountRate
    } else {
      const numdiscountRate = typeof rawdiscountRate === 'number' ? rawdiscountRate : Number(rawdiscountRate)
      if (Number.isFinite(numdiscountRate)) payload.discountRate = numdiscountRate
      else delete payload.discountRate
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
  if ('purchaseAmount' in payload) {
    const rawpurchaseAmount = payload.purchaseAmount
    if (rawpurchaseAmount === undefined || rawpurchaseAmount === null || rawpurchaseAmount === '') {
      delete payload.purchaseAmount
    } else {
      const numpurchaseAmount = typeof rawpurchaseAmount === 'number' ? rawpurchaseAmount : Number(rawpurchaseAmount)
      if (Number.isFinite(numpurchaseAmount)) payload.purchaseAmount = numpurchaseAmount
      else delete payload.purchaseAmount
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
  if ('deliveryStatus' in payload) {
    const rawdeliveryStatus = payload.deliveryStatus
    if (rawdeliveryStatus === undefined || rawdeliveryStatus === null || rawdeliveryStatus === '') {
      delete payload.deliveryStatus
    } else {
      const numdeliveryStatus = typeof rawdeliveryStatus === 'number' ? rawdeliveryStatus : Number(rawdeliveryStatus)
      if (Number.isFinite(numdeliveryStatus)) payload.deliveryStatus = numdeliveryStatus
      else delete payload.deliveryStatus
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

  if (props.formData?.purchaseOrderItemId) {
    payload.purchaseOrderItemId = props.formData.purchaseOrderItemId
    delete payload.numberingRuleCode
  }
  payload.purchaseOrderId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.purchaseOrderCode ?? masterRow.PurchaseOrderCode
    const masterName = masterRow.purchaseOrderName ?? masterRow.PurchaseOrderName
    if (masterCode != null && masterCode !== '' && !payload.purchaseOrderCode) {
      payload.purchaseOrderCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.purchaseOrderName) {
      payload.purchaseOrderName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseOrderItemId)
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

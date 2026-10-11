<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-info-record/components -->
<!-- 文件名称：PurchaseInfoRecordItemForm.vue -->
<!-- 功能描述：Takt采购信息记录实体子表 purchaseInfoRecordItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *Form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-info-record-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-info-record-item-form-tabs"
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
                :label="pi.label('purchaseInfoRecordSeq')"
                name="purchaseInfoRecordSeq"
              >
                <a-input-number
                  v-model:value="formState.purchaseInfoRecordSeq"
                  :placeholder="pi.ph('purchaseInfoRecordSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleType')"
                name="scaleType"
              >
                <TaktSelect
                  v-model:value="formState.scaleType"
                  dict-type="logistics_procurement_scale"
                  :placeholder="pi.ph('scaleType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleBasis')"
                name="scaleBasis"
              >
                <TaktSelect
                  v-model:value="formState.scaleBasis"
                  dict-type="logistics_procurement_scale_basis"
                  :placeholder="pi.ph('scaleBasis')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantity')"
                name="scaleQuantity"
              >
                <a-input-number
                  v-model:value="formState.scaleQuantity"
                  :placeholder="pi.ph('scaleQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleUnit')"
                name="scaleUnit"
              >
                <TaktSelect
                  v-model:value="formState.scaleUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('scaleUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleValue')"
                name="scaleValue"
              >
                <a-input-number
                  v-model:value="formState.scaleValue"
                  :placeholder="pi.ph('scaleValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleCurrencyCode')"
                name="scaleCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.scaleCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('scaleCurrencyCode')"
                  :disabled="!!formData?.purchaseInfoRecordItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('calculationType')"
                name="calculationType"
              >
                <TaktSelect
                  v-model:value="formState.calculationType"
                  dict-type="logistics_procurement_calculation_type"
                  :placeholder="pi.ph('calculationType')"
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
                :label="pi.label('price')"
                name="price"
              >
                <a-input-number
                  v-model:value="formState.price"
                  :placeholder="pi.ph('price')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conditionCurrencyCode')"
                name="conditionCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.conditionCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('conditionCurrencyCode')"
                  :disabled="!!formData?.purchaseInfoRecordItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priceUnit')"
                name="priceUnit"
              >
                <TaktSelect
                  v-model:value="formState.priceUnit"
                  dict-type="logistics_materials_price_unit_param"
                  :placeholder="pi.ph('priceUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitOfMeasure')"
                name="unitOfMeasure"
              >
                <TaktSelect
                  v-model:value="formState.unitOfMeasure"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('unitOfMeasure')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('minOrderQuantity')"
                name="minOrderQuantity"
              >
                <a-input-number
                  v-model:value="formState.minOrderQuantity"
                  :placeholder="pi.ph('minOrderQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('roundingValue')"
                name="roundingValue"
              >
                <a-input-number
                  v-model:value="formState.roundingValue"
                  :placeholder="pi.ph('roundingValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedDeliveryTimeDays')"
                name="plannedDeliveryTimeDays"
              >
                <a-input-number
                  v-model:value="formState.plannedDeliveryTimeDays"
                  :placeholder="pi.ph('plannedDeliveryTimeDays')"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleQuantities')"
                name="scaleQuantities"
              >
                <a-input
                  v-model:value="formState.scaleQuantities"
                  :placeholder="pi.ph('scaleQuantities')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scaleValues')"
                name="scaleValues"
              >
                <a-input
                  v-model:value="formState.scaleValues"
                  :placeholder="pi.ph('scaleValues')"
                  show-count
                  :maxlength="20"
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
 * Takt采购信息记录实体子表 purchaseInfoRecordItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/procurement/purchase-info-record/components
 */

import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseInfoRecordItemI18n } from '../composables/usePurchaseInfoRecordItemI18n'

/** 实体字段 i18n */
const pi = usePurchaseInfoRecordItemI18n()

import type { PurchaseInfoRecordItemCreate } from '@/types/logistics/procurement/purchaseInfoRecordItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","purchaseInfoRecordSeq","scaleType","scaleBasis","scaleQuantity","scaleUnit","scaleValue","scaleCurrencyCode","calculationType","price","conditionCurrencyCode","priceUnit","unitOfMeasure","minOrderQuantity","roundingValue","plannedDeliveryTimeDays","isObsolete","scaleQuantities","scaleValues"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInfoRecordItemCreate & { purchaseInfoRecordItemId?: string }> | null
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
  scaleType: "A",
  scaleBasis: "C",
  scaleCurrencyCode: "CNY",
  calculationType: "A",
  conditionCurrencyCode: "CNY",
  priceUnit: 1000
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



/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInfoRecordItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInfoRecordItemId) {
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
    if (!props.formData?.purchaseInfoRecordItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  purchaseInfoRecordSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchaseInfoRecordSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchaseInfoRecordSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scaleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scaleQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scaleValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scaleValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scaleValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  calculationType: [
    {
      required: true,
      message: pi.ph('calculationType'),
      trigger: 'change'
    }
  ],
  price: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('price'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('price'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  conditionCurrencyCode: [
    {
      required: true,
      message: pi.ph('conditionCurrencyCode'),
      trigger: 'change'
    }
  ],
  priceUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priceUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priceUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitOfMeasure: [
    {
      required: true,
      message: pi.ph('unitOfMeasure'),
      trigger: 'change'
    }
  ],
  minOrderQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('minOrderQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('minOrderQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  roundingValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('roundingValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('roundingValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedDeliveryTimeDays: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedDeliveryTimeDays'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedDeliveryTimeDays'))
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

/** 映射为 Create/Update DTO（含主表外键 purchaseInfoRecordId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('purchaseInfoRecordSeq' in payload) {
    const rawpurchaseInfoRecordSeq = payload.purchaseInfoRecordSeq
    if (rawpurchaseInfoRecordSeq === undefined || rawpurchaseInfoRecordSeq === null || rawpurchaseInfoRecordSeq === '') {
      delete payload.purchaseInfoRecordSeq
    } else {
      const numpurchaseInfoRecordSeq = typeof rawpurchaseInfoRecordSeq === 'number' ? rawpurchaseInfoRecordSeq : Number(rawpurchaseInfoRecordSeq)
      if (Number.isFinite(numpurchaseInfoRecordSeq)) payload.purchaseInfoRecordSeq = numpurchaseInfoRecordSeq
      else delete payload.purchaseInfoRecordSeq
    }
  }
  if ('scaleQuantity' in payload) {
    const rawscaleQuantity = payload.scaleQuantity
    if (rawscaleQuantity === undefined || rawscaleQuantity === null || rawscaleQuantity === '') {
      delete payload.scaleQuantity
    } else {
      const numscaleQuantity = typeof rawscaleQuantity === 'number' ? rawscaleQuantity : Number(rawscaleQuantity)
      if (Number.isFinite(numscaleQuantity)) payload.scaleQuantity = numscaleQuantity
      else delete payload.scaleQuantity
    }
  }
  if ('scaleValue' in payload) {
    const rawscaleValue = payload.scaleValue
    if (rawscaleValue === undefined || rawscaleValue === null || rawscaleValue === '') {
      delete payload.scaleValue
    } else {
      const numscaleValue = typeof rawscaleValue === 'number' ? rawscaleValue : Number(rawscaleValue)
      if (Number.isFinite(numscaleValue)) payload.scaleValue = numscaleValue
      else delete payload.scaleValue
    }
  }
  if ('price' in payload) {
    const rawprice = payload.price
    if (rawprice === undefined || rawprice === null || rawprice === '') {
      delete payload.price
    } else {
      const numprice = typeof rawprice === 'number' ? rawprice : Number(rawprice)
      if (Number.isFinite(numprice)) payload.price = numprice
      else delete payload.price
    }
  }
  if ('priceUnit' in payload) {
    const rawpriceUnit = payload.priceUnit
    if (rawpriceUnit === undefined || rawpriceUnit === null || rawpriceUnit === '') {
      delete payload.priceUnit
    } else {
      const numpriceUnit = typeof rawpriceUnit === 'number' ? rawpriceUnit : Number(rawpriceUnit)
      if (Number.isFinite(numpriceUnit)) payload.priceUnit = numpriceUnit
      else delete payload.priceUnit
    }
  }
  if ('minOrderQuantity' in payload) {
    const rawminOrderQuantity = payload.minOrderQuantity
    if (rawminOrderQuantity === undefined || rawminOrderQuantity === null || rawminOrderQuantity === '') {
      delete payload.minOrderQuantity
    } else {
      const numminOrderQuantity = typeof rawminOrderQuantity === 'number' ? rawminOrderQuantity : Number(rawminOrderQuantity)
      if (Number.isFinite(numminOrderQuantity)) payload.minOrderQuantity = numminOrderQuantity
      else delete payload.minOrderQuantity
    }
  }
  if ('roundingValue' in payload) {
    const rawroundingValue = payload.roundingValue
    if (rawroundingValue === undefined || rawroundingValue === null || rawroundingValue === '') {
      delete payload.roundingValue
    } else {
      const numroundingValue = typeof rawroundingValue === 'number' ? rawroundingValue : Number(rawroundingValue)
      if (Number.isFinite(numroundingValue)) payload.roundingValue = numroundingValue
      else delete payload.roundingValue
    }
  }
  if ('plannedDeliveryTimeDays' in payload) {
    const rawplannedDeliveryTimeDays = payload.plannedDeliveryTimeDays
    if (rawplannedDeliveryTimeDays === undefined || rawplannedDeliveryTimeDays === null || rawplannedDeliveryTimeDays === '') {
      delete payload.plannedDeliveryTimeDays
    } else {
      const numplannedDeliveryTimeDays = typeof rawplannedDeliveryTimeDays === 'number' ? rawplannedDeliveryTimeDays : Number(rawplannedDeliveryTimeDays)
      if (Number.isFinite(numplannedDeliveryTimeDays)) payload.plannedDeliveryTimeDays = numplannedDeliveryTimeDays
      else delete payload.plannedDeliveryTimeDays
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

  if (props.formData?.purchaseInfoRecordItemId) {
    payload.purchaseInfoRecordItemId = props.formData.purchaseInfoRecordItemId
    delete payload.numberingRuleCode
  }
  payload.purchaseInfoRecordId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.purchaseInfoRecordCode ?? masterRow.PurchaseInfoRecordCode
    const masterName = masterRow.purchaseInfoRecordName ?? masterRow.PurchaseInfoRecordName
    if (masterCode != null && masterCode !== '' && !payload.purchaseInfoRecordCode) {
      payload.purchaseInfoRecordCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.purchaseInfoRecordName) {
      payload.purchaseInfoRecordName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseInfoRecordItemId)
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

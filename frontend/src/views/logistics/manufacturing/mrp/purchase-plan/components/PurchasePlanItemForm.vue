<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/purchase-plan/components -->
<!-- 文件名称：PurchasePlanItemForm.vue -->
<!-- 功能描述：Takt采购计划实体子表 purchasePlanItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-plan-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-plan-item-form-tabs"
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
                :label="pi.label('productionPlanId')"
                name="productionPlanId"
              >
                <a-input
                  v-model:value="formState.productionPlanId"
                  :placeholder="pi.ph('productionPlanId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionPlanCode')"
                name="productionPlanCode"
              >
                <a-input
                  v-model:value="formState.productionPlanCode"
                  :placeholder="pi.ph('productionPlanCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.purchasePlanItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionPlanLineNumber')"
                name="productionPlanLineNumber"
              >
                <a-input-number
                  v-model:value="formState.productionPlanLineNumber"
                  :placeholder="pi.ph('productionPlanLineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialRequirementsPlanningItemId')"
                name="materialRequirementsPlanningItemId"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningItemId"
                  :placeholder="pi.ph('materialRequirementsPlanningItemId')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :disabled="!!formData?.purchasePlanItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planUnit')"
                name="planUnit"
              >
                <TaktSelect
                  v-model:value="formState.planUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('planUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planQuantity')"
                name="planQuantity"
              >
                <a-input-number
                  v-model:value="formState.planQuantity"
                  :placeholder="pi.ph('planQuantity')"
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
                :label="pi.label('plannedArrivalDate')"
                name="plannedArrivalDate"
              >
                <a-date-picker
                  v-model:value="formState.plannedArrivalDate"
                  :placeholder="pi.ph('plannedArrivalDate')"
                  value-format="YYYY-MM-DD"
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
                :label="pi.label('estimatedUnitPrice')"
                name="estimatedUnitPrice"
              >
                <a-input-number
                  v-model:value="formState.estimatedUnitPrice"
                  :placeholder="pi.ph('estimatedUnitPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('estimatedAmount')"
                name="estimatedAmount"
              >
                <a-input-number
                  v-model:value="formState.estimatedAmount"
                  :placeholder="pi.ph('estimatedAmount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxIncludedPrice')"
                name="taxIncludedPrice"
              >
                <a-input-number
                  v-model:value="formState.taxIncludedPrice"
                  :placeholder="pi.ph('taxIncludedPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('untaxedPrice')"
                name="untaxedPrice"
              >
                <a-input-number
                  v-model:value="formState.untaxedPrice"
                  :placeholder="pi.ph('untaxedPrice')"
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
                :label="pi.label('referenceSupplierCode')"
                name="referenceSupplierCode"
              >
                <TaktSelect
                  v-model:value="formState.referenceSupplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('referenceSupplierCode')"
                  :disabled="!!formData?.purchasePlanItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceSupplierName1')"
                name="referenceSupplierName1"
              >
                <a-input
                  v-model:value="formState.referenceSupplierName1"
                  :placeholder="pi.ph('referenceSupplierName1')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
 * Takt采购计划实体子表 purchasePlanItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/mrp/purchase-plan/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchasePlanItemI18n } from '../composables/usePurchasePlanItemI18n'

/** 实体字段 i18n */
const pi = usePurchasePlanItemI18n()

import type { PurchasePlanItemCreate } from '@/types/logistics/manufacturing/mrp/purchasePlanItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","productionPlanId","productionPlanCode","productionPlanLineNumber","materialRequirementsPlanningItemId","materialCode","planUnit","planQuantity","plannedArrivalDate","convertedQuantity","estimatedUnitPrice","estimatedAmount","taxIncludedPrice","untaxedPrice","taxAmount","referenceSupplierCode","referenceSupplierName1","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchasePlanItemCreate & { purchasePlanItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchasePlanItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchasePlanItemId) {
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
    if (!props.formData?.purchasePlanItemId) {
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
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  planUnit: [
    {
      required: true,
      message: pi.ph('planUnit'),
      trigger: 'change'
    }
  ],
  planQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('planQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('planQuantity'))
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
  estimatedUnitPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('estimatedUnitPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('estimatedUnitPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  estimatedAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('estimatedAmount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('estimatedAmount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  taxIncludedPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('taxIncludedPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('taxIncludedPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  untaxedPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('untaxedPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('untaxedPrice'))
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

/** 映射为 Create/Update DTO（含主表外键 purchasePlanId） */
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
  if ('productionPlanLineNumber' in payload) {
    const rawproductionPlanLineNumber = payload.productionPlanLineNumber
    if (rawproductionPlanLineNumber === undefined || rawproductionPlanLineNumber === null || rawproductionPlanLineNumber === '') {
      delete payload.productionPlanLineNumber
    } else {
      const numproductionPlanLineNumber = typeof rawproductionPlanLineNumber === 'number' ? rawproductionPlanLineNumber : Number(rawproductionPlanLineNumber)
      if (Number.isFinite(numproductionPlanLineNumber)) payload.productionPlanLineNumber = numproductionPlanLineNumber
      else delete payload.productionPlanLineNumber
    }
  }
  if ('planQuantity' in payload) {
    const rawplanQuantity = payload.planQuantity
    if (rawplanQuantity === undefined || rawplanQuantity === null || rawplanQuantity === '') {
      delete payload.planQuantity
    } else {
      const numplanQuantity = typeof rawplanQuantity === 'number' ? rawplanQuantity : Number(rawplanQuantity)
      if (Number.isFinite(numplanQuantity)) payload.planQuantity = numplanQuantity
      else delete payload.planQuantity
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
  if ('estimatedUnitPrice' in payload) {
    const rawestimatedUnitPrice = payload.estimatedUnitPrice
    if (rawestimatedUnitPrice === undefined || rawestimatedUnitPrice === null || rawestimatedUnitPrice === '') {
      delete payload.estimatedUnitPrice
    } else {
      const numestimatedUnitPrice = typeof rawestimatedUnitPrice === 'number' ? rawestimatedUnitPrice : Number(rawestimatedUnitPrice)
      if (Number.isFinite(numestimatedUnitPrice)) payload.estimatedUnitPrice = numestimatedUnitPrice
      else delete payload.estimatedUnitPrice
    }
  }
  if ('estimatedAmount' in payload) {
    const rawestimatedAmount = payload.estimatedAmount
    if (rawestimatedAmount === undefined || rawestimatedAmount === null || rawestimatedAmount === '') {
      delete payload.estimatedAmount
    } else {
      const numestimatedAmount = typeof rawestimatedAmount === 'number' ? rawestimatedAmount : Number(rawestimatedAmount)
      if (Number.isFinite(numestimatedAmount)) payload.estimatedAmount = numestimatedAmount
      else delete payload.estimatedAmount
    }
  }
  if ('taxIncludedPrice' in payload) {
    const rawtaxIncludedPrice = payload.taxIncludedPrice
    if (rawtaxIncludedPrice === undefined || rawtaxIncludedPrice === null || rawtaxIncludedPrice === '') {
      delete payload.taxIncludedPrice
    } else {
      const numtaxIncludedPrice = typeof rawtaxIncludedPrice === 'number' ? rawtaxIncludedPrice : Number(rawtaxIncludedPrice)
      if (Number.isFinite(numtaxIncludedPrice)) payload.taxIncludedPrice = numtaxIncludedPrice
      else delete payload.taxIncludedPrice
    }
  }
  if ('untaxedPrice' in payload) {
    const rawuntaxedPrice = payload.untaxedPrice
    if (rawuntaxedPrice === undefined || rawuntaxedPrice === null || rawuntaxedPrice === '') {
      delete payload.untaxedPrice
    } else {
      const numuntaxedPrice = typeof rawuntaxedPrice === 'number' ? rawuntaxedPrice : Number(rawuntaxedPrice)
      if (Number.isFinite(numuntaxedPrice)) payload.untaxedPrice = numuntaxedPrice
      else delete payload.untaxedPrice
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

  if (props.formData?.purchasePlanItemId) {
    payload.purchasePlanItemId = props.formData.purchasePlanItemId
    delete payload.numberingRuleCode
  }
  payload.purchasePlanId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.purchasePlanCode ?? masterRow.PurchasePlanCode
    const masterName = masterRow.purchasePlanName ?? masterRow.PurchasePlanName
    if (masterCode != null && masterCode !== '' && !payload.purchasePlanCode) {
      payload.purchasePlanCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.purchasePlanName) {
      payload.purchasePlanName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchasePlanItemId)
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

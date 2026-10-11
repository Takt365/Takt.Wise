<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-item/components -->
<!-- 文件名称：MaterialCostItemForm.vue -->
<!-- 功能描述：BOM 物料成本明细行维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="material-cost-item-form-tabs"
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
                :label="pi.label('bomLevel')"
                name="bomLevel"
              >
                <a-input
                  v-model:value="formState.bomLevel"
                  :placeholder="pi.ph('bomLevel')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomItemCode')"
                name="bomItemCode"
              >
                <a-input
                  v-model:value="formState.bomItemCode"
                  :placeholder="pi.ph('bomItemCode')"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productCode')"
                name="productCode"
              >
                <TaktSelect
                  v-model:value="formState.productCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('productCode')"
                  :disabled="!!formData?.bomMaterialCostItemId"
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
                  :min="1"
                  :step="10"
                  style="width: 100%"
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('productDescription')"
                name="productDescription"
              >
                <a-textarea
                  v-model:value="formState.productDescription"
                  :placeholder="pi.ph('productDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('componentCode')"
                name="componentCode"
              >
                <TaktSelect
                  v-model:value="formState.componentCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('componentCode')"
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('componentDescription')"
                name="componentDescription"
              >
                <a-textarea
                  v-model:value="formState.componentDescription"
                  :placeholder="pi.ph('componentDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('componentQuantity')"
                name="componentQuantity"
              >
                <a-input-number
                  v-model:value="formState.componentQuantity"
                  :placeholder="pi.ph('componentQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchIndicator')"
                name="batchIndicator"
              >
                <a-input
                  v-model:value="formState.batchIndicator"
                  :placeholder="pi.ph('batchIndicator')"
                  show-count
                  :maxlength="1"
                  allow-clear
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
                :label="pi.label('productionRelated')"
                name="productionRelated"
              >
                <a-input
                  v-model:value="formState.productionRelated"
                  :placeholder="pi.ph('productionRelated')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseType')"
                name="purchaseType"
              >
                <a-input
                  v-model:value="formState.purchaseType"
                  :placeholder="pi.ph('purchaseType')"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('specialProcurementType')"
                name="specialProcurementType"
              >
                <a-input
                  v-model:value="formState.specialProcurementType"
                  :placeholder="pi.ph('specialProcurementType')"
                  show-count
                  :maxlength="50"
                  allow-clear
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
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('movingAveragePrice')"
                name="movingAveragePrice"
              >
                <a-input-number
                  v-model:value="formState.movingAveragePrice"
                  :placeholder="pi.ph('movingAveragePrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('movingPriceUnit')"
                name="movingPriceUnit"
              >
                <a-input-number
                  v-model:value="formState.movingPriceUnit"
                  :placeholder="pi.ph('movingPriceUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('movingPriceCurrencyCode')"
                name="movingPriceCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.movingPriceCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('movingPriceCurrencyCode')"
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrganization')"
                name="purchaseOrganization"
              >
                <a-input
                  v-model:value="formState.purchaseOrganization"
                  :placeholder="pi.ph('purchaseOrganization')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseGroup')"
                name="purchaseGroup"
              >
                <TaktSelect
                  v-model:value="formState.purchaseGroup"
                  api-url="TaktPurchaseGroups/options"
                  :placeholder="pi.ph('purchaseGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('supplierCode')"
                name="supplierCode"
              >
                <TaktSelect
                  v-model:value="formState.supplierCode"
                  api-url="TaktSuppliers/options"
                  :placeholder="pi.ph('supplierCode')"
                  :disabled="!!formData?.bomMaterialCostItemId"
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
                :label="pi.label('netPurchasePrice')"
                name="netPurchasePrice"
              >
                <a-input-number
                  v-model:value="formState.netPurchasePrice"
                  :placeholder="pi.ph('netPurchasePrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('purchasePriceUnit')"
                name="purchasePriceUnit"
              >
                <a-input-number
                  v-model:value="formState.purchasePriceUnit"
                  :placeholder="pi.ph('purchasePriceUnit')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('purchaseCurrencyCode')"
                name="purchaseCurrencyCode"
              >
                <TaktSelect
                  v-model:value="formState.purchaseCurrencyCode"
                  dict-type="accounting_financial_currency_code"
                  :placeholder="pi.ph('purchaseCurrencyCode')"
                  :disabled="!!formData?.bomMaterialCostItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('costingDate')"
                name="costingDate"
              >
                <a-date-picker
                  v-model:value="formState.costingDate"
                  :placeholder="pi.ph('costingDate')"
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
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="pi.ph('cultureCode')"
                  show-count
                  :maxlength="20"
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
 * BOM 物料成本明细行维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/material-cost-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBomMaterialCostItemI18n } from '../composables/useMaterialCostItemI18n'

/** 实体字段 i18n */
const pi = useBomMaterialCostItemI18n()
import type { BomMaterialCostItemCreate } from '@/types/logistics/manufacturing/bom/materialCostItem.types'
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
 * 上下文隔离字段：租户 / 公司 / CultureCode（登录或公司切换注入，表单只读）
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
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BomMaterialCostItemCreate & { bomMaterialCostItemId?: string }> | null
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
  lineNumber: 10,
  movingPriceCurrencyCode: "CNY",
  purchaseCurrencyCode: "CNY"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 bomMaterialCostItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.bomMaterialCostItemId) {
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.bomMaterialCostItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  bomLevel: [
    {
      required: true,
      message: pi.ph('bomLevel'),
      trigger: 'blur'
    }
  ],
  bomItemCode: [
    {
      required: true,
      message: pi.ph('bomItemCode'),
      trigger: 'blur'
    }
  ],
  productCode: [
    {
      required: true,
      message: pi.ph('productCode'),
      trigger: 'change'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num) || num <= 0) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  productDescription: [
    {
      required: true,
      message: pi.ph('productDescription'),
      trigger: 'blur'
    }
  ],
  componentCode: [
    {
      required: true,
      message: pi.ph('componentCode'),
      trigger: 'change'
    }
  ],
  componentDescription: [
    {
      required: true,
      message: pi.ph('componentDescription'),
      trigger: 'blur'
    }
  ],
  componentQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('componentQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('componentQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseType: [
    {
      required: true,
      message: pi.ph('purchaseType'),
      trigger: 'blur'
    }
  ],
  profitCenterCode: [
    {
      required: true,
      message: pi.ph('profitCenterCode'),
      trigger: 'change'
    }
  ],
  movingAveragePrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('movingAveragePrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('movingAveragePrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  movingPriceUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('movingPriceUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('movingPriceUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  movingPriceCurrencyCode: [
    {
      required: true,
      message: pi.ph('movingPriceCurrencyCode'),
      trigger: 'change'
    }
  ],
  purchaseOrganization: [
    {
      required: true,
      message: pi.ph('purchaseOrganization'),
      trigger: 'blur'
    }
  ],
  purchaseGroup: [
    {
      required: true,
      message: pi.ph('purchaseGroup'),
      trigger: 'change'
    }
  ],
  supplierCode: [
    {
      required: true,
      message: pi.ph('supplierCode'),
      trigger: 'change'
    }
  ],
  netPurchasePrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('netPurchasePrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('netPurchasePrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchasePriceUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purchasePriceUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purchasePriceUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  purchaseCurrencyCode: [
    {
      required: true,
      message: pi.ph('purchaseCurrencyCode'),
      trigger: 'change'
    }
  ],
  costingDate: [
    {
      required: true,
      message: pi.ph('costingDate'),
      trigger: 'change'
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
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('componentQuantity' in payload) {
    const rawcomponentQuantity = payload.componentQuantity
    payload.componentQuantity = typeof rawcomponentQuantity === 'number' ? rawcomponentQuantity : Number(rawcomponentQuantity)
  }
  if ('movingAveragePrice' in payload) {
    const rawmovingAveragePrice = payload.movingAveragePrice
    payload.movingAveragePrice = typeof rawmovingAveragePrice === 'number' ? rawmovingAveragePrice : Number(rawmovingAveragePrice)
  }
  if ('movingPriceUnit' in payload) {
    const rawmovingPriceUnit = payload.movingPriceUnit
    payload.movingPriceUnit = typeof rawmovingPriceUnit === 'number' ? rawmovingPriceUnit : Number(rawmovingPriceUnit)
  }
  if ('netPurchasePrice' in payload) {
    const rawnetPurchasePrice = payload.netPurchasePrice
    payload.netPurchasePrice = typeof rawnetPurchasePrice === 'number' ? rawnetPurchasePrice : Number(rawnetPurchasePrice)
  }
  if ('purchasePriceUnit' in payload) {
    const rawpurchasePriceUnit = payload.purchasePriceUnit
    payload.purchasePriceUnit = typeof rawpurchasePriceUnit === 'number' ? rawpurchasePriceUnit : Number(rawpurchasePriceUnit)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.bomMaterialCostItemId)

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

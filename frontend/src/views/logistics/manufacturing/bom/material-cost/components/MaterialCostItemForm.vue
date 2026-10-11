<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：MaterialCostItemForm.vue -->
<!-- 功能描述：BOM 物料成本主表子表 bomMaterialCostItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-cost-item-form flex flex-col min-h-0"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本主表子表 bomMaterialCostItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/material-cost/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBomMaterialCostItemI18n } from '../composables/useMaterialCostItemI18n'

/** 实体字段 i18n */
const pi = useBomMaterialCostItemI18n()

import type { BomMaterialCostItemCreate } from '@/types/logistics/manufacturing/bom/materialCostItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["plantCode","bomLevel","bomItemCode","productCode","lineNumber","productDescription","componentCode","componentDescription","componentQuantity","batchIndicator","productionRelated","purchaseType","specialProcurementType","profitCenterCode","movingAveragePrice","movingPriceUnit","movingPriceCurrencyCode","purchaseOrganization","purchaseGroup","supplierCode","netPurchasePrice","purchasePriceUnit","purchaseCurrencyCode","costingDate"]

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

import { useTenantStore } from '@/stores/identity/useTenantStore'

/** Pinia：租户上下文（工厂默认取当前公司 RelatedPlant） */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
}

/** 公司切换时，新增态同步工厂默认值 */
watch(
  () => tenantStore.currentCompanyRelatedPlant,
  () => {
    if (!props.formData?.bomMaterialCostItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

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

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
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
      trigger: 'blur'
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
      trigger: 'blur'
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

/** 映射为 Create/Update DTO（无汇总表外键；由明细业务键再 Sync 汇总） */
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
  if ('bomMaterialCostId' in payload) delete payload.bomMaterialCostId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState, !props.formData?.bomMaterialCostItemId)
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

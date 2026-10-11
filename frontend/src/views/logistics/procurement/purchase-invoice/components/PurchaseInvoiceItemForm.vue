<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/purchase-invoice/components -->
<!-- 文件名称：PurchaseInvoiceItemForm.vue -->
<!-- 功能描述：Takt采购发票主表实体子表 purchaseInvoiceItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-invoice-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="purchase-invoice-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/5)'"
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
                :label="pi.label('purchaseOrderCode')"
                name="purchaseOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.purchaseOrderCode"
                  api-url="TaktPurchaseOrders/options"
                  :placeholder="pi.ph('purchaseOrderCode')"
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseOrderItem')"
                name="purchaseOrderItem"
              >
                <a-input-number
                  v-model:value="formState.purchaseOrderItem"
                  :placeholder="pi.ph('purchaseOrderItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountAssignmentSeq')"
                name="accountAssignmentSeq"
              >
                <a-input
                  v-model:value="formState.accountAssignmentSeq"
                  :placeholder="pi.ph('accountAssignmentSeq')"
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
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valuationArea')"
                name="valuationArea"
              >
                <a-input
                  v-model:value="formState.valuationArea"
                  :placeholder="pi.ph('valuationArea')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('amount')"
                name="amount"
              >
                <a-input-number
                  v-model:value="formState.amount"
                  :placeholder="pi.ph('amount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('debitCreditIndicator')"
                name="debitCreditIndicator"
              >
                <a-input
                  v-model:value="formState.debitCreditIndicator"
                  :placeholder="pi.ph('debitCreditIndicator')"
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
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('taxCode')"
                name="taxCode"
              >
                <a-input
                  v-model:value="formState.taxCode"
                  :placeholder="pi.ph('taxCode')"
                  show-count
                  :maxlength="2"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('quantity')"
                name="quantity"
              >
                <a-input-number
                  v-model:value="formState.quantity"
                  :placeholder="pi.ph('quantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('orderUnit')"
                name="orderUnit"
              >
                <a-input
                  v-model:value="formState.orderUnit"
                  :placeholder="pi.ph('orderUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('poPriceQuantity')"
                name="poPriceQuantity"
              >
                <a-input-number
                  v-model:value="formState.poPriceQuantity"
                  :placeholder="pi.ph('poPriceQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('poPriceUnit')"
                name="poPriceUnit"
              >
                <a-input
                  v-model:value="formState.poPriceUnit"
                  :placeholder="pi.ph('poPriceUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valuatedStockQuantity')"
                name="valuatedStockQuantity"
              >
                <a-input-number
                  v-model:value="formState.valuatedStockQuantity"
                  :placeholder="pi.ph('valuatedStockQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('previousPeriodStock')"
                name="previousPeriodStock"
              >
                <a-input-number
                  v-model:value="formState.previousPeriodStock"
                  :placeholder="pi.ph('previousPeriodStock')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <a-input
                  v-model:value="formState.baseUnit"
                  :placeholder="pi.ph('baseUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valuationClass')"
                name="valuationClass"
              >
                <a-input
                  v-model:value="formState.valuationClass"
                  :placeholder="pi.ph('valuationClass')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('updatePoHistoryFlag')"
                name="updatePoHistoryFlag"
              >
                <a-date-picker
                  v-model:value="formState.updatePoHistoryFlag"
                  :placeholder="pi.ph('updatePoHistoryFlag')"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('subsequentDebitCredit')"
                name="subsequentDebitCredit"
              >
                <a-input
                  v-model:value="formState.subsequentDebitCredit"
                  :placeholder="pi.ph('subsequentDebitCredit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('blockReasonPrice')"
                name="blockReasonPrice"
              >
                <a-input
                  v-model:value="formState.blockReasonPrice"
                  :placeholder="pi.ph('blockReasonPrice')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('blockReasonQuantity')"
                name="blockReasonQuantity"
              >
                <a-input
                  v-model:value="formState.blockReasonQuantity"
                  :placeholder="pi.ph('blockReasonQuantity')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('blockReasonQuality')"
                name="blockReasonQuality"
              >
                <a-input
                  v-model:value="formState.blockReasonQuality"
                  :placeholder="pi.ph('blockReasonQuality')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('blockReasonEnhanced')"
                name="blockReasonEnhanced"
              >
                <a-input
                  v-model:value="formState.blockReasonEnhanced"
                  :placeholder="pi.ph('blockReasonEnhanced')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('valueString')"
                name="valueString"
              >
                <a-input
                  v-model:value="formState.valueString"
                  :placeholder="pi.ph('valueString')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceCode')"
                name="referenceCode"
              >
                <a-input
                  v-model:value="formState.referenceCode"
                  :placeholder="pi.ph('referenceCode')"
                  show-count
                  :maxlength="16"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('conditionType')"
                name="conditionType"
              >
                <a-input
                  v-model:value="formState.conditionType"
                  :placeholder="pi.ph('conditionType')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalValuatedStockValue')"
                name="totalValuatedStockValue"
              >
                <a-input-number
                  v-model:value="formState.totalValuatedStockValue"
                  :placeholder="pi.ph('totalValuatedStockValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('previousPeriodValue')"
                name="previousPeriodValue"
              >
                <a-input-number
                  v-model:value="formState.previousPeriodValue"
                  :placeholder="pi.ph('previousPeriodValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/5)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentCode')"
                name="referenceDocumentCode"
              >
                <a-input
                  v-model:value="formState.referenceDocumentCode"
                  :placeholder="pi.ph('referenceDocumentCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentYear')"
                name="referenceDocumentYear"
              >
                <a-input
                  v-model:value="formState.referenceDocumentYear"
                  :placeholder="pi.ph('referenceDocumentYear')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('referenceDocumentItem')"
                name="referenceDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.referenceDocumentItem"
                  :placeholder="pi.ph('referenceDocumentItem')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stockManagedMaterialCode')"
                name="stockManagedMaterialCode"
              >
                <a-input
                  v-model:value="formState.stockManagedMaterialCode"
                  :placeholder="pi.ph('stockManagedMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.purchaseInvoiceItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('itemText')"
                name="itemText"
              >
                <a-input
                  v-model:value="formState.itemText"
                  :placeholder="pi.ph('itemText')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialDocumentItem')"
                name="materialDocumentItem"
              >
                <a-input-number
                  v-model:value="formState.materialDocumentItem"
                  :placeholder="pi.ph('materialDocumentItem')"
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
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-4"
        :tab="t('common.page.form.tabs.basicinfo') + ' (5/5)'"
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
 * Takt采购发票主表实体子表 purchaseInvoiceItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/procurement/purchase-invoice/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseInvoiceItemI18n } from '../composables/usePurchaseInvoiceItemI18n'

/** 实体字段 i18n */
const pi = usePurchaseInvoiceItemI18n()

import type { PurchaseInvoiceItemCreate } from '@/types/logistics/procurement/purchaseInvoiceItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","purchaseOrderCode","purchaseOrderItem","accountAssignmentSeq","materialCode","valuationArea","amount","debitCreditIndicator","taxCode","quantity","orderUnit","poPriceQuantity","poPriceUnit","valuatedStockQuantity","previousPeriodStock","baseUnit","valuationClass","updatePoHistoryFlag","subsequentDebitCredit","blockReasonPrice","blockReasonQuantity","blockReasonQuality","blockReasonEnhanced","valueString","referenceCode","conditionType","totalValuatedStockValue","previousPeriodValue","referenceDocumentCode","referenceDocumentYear","referenceDocumentItem","stockManagedMaterialCode","itemText","materialDocumentItem","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseInvoiceItemCreate & { purchaseInvoiceItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseInvoiceItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseInvoiceItemId) {
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
    if (!props.formData?.purchaseInvoiceItemId) {
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

/** 映射为 Create/Update DTO（含主表外键 purchaseInvoiceId） */
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
  if ('purchaseOrderItem' in payload) {
    const rawpurchaseOrderItem = payload.purchaseOrderItem
    if (rawpurchaseOrderItem === undefined || rawpurchaseOrderItem === null || rawpurchaseOrderItem === '') {
      delete payload.purchaseOrderItem
    } else {
      const numpurchaseOrderItem = typeof rawpurchaseOrderItem === 'number' ? rawpurchaseOrderItem : Number(rawpurchaseOrderItem)
      if (Number.isFinite(numpurchaseOrderItem)) payload.purchaseOrderItem = numpurchaseOrderItem
      else delete payload.purchaseOrderItem
    }
  }
  if ('amount' in payload) {
    const rawamount = payload.amount
    if (rawamount === undefined || rawamount === null || rawamount === '') {
      delete payload.amount
    } else {
      const numamount = typeof rawamount === 'number' ? rawamount : Number(rawamount)
      if (Number.isFinite(numamount)) payload.amount = numamount
      else delete payload.amount
    }
  }
  if ('quantity' in payload) {
    const rawquantity = payload.quantity
    if (rawquantity === undefined || rawquantity === null || rawquantity === '') {
      delete payload.quantity
    } else {
      const numquantity = typeof rawquantity === 'number' ? rawquantity : Number(rawquantity)
      if (Number.isFinite(numquantity)) payload.quantity = numquantity
      else delete payload.quantity
    }
  }
  if ('poPriceQuantity' in payload) {
    const rawpoPriceQuantity = payload.poPriceQuantity
    if (rawpoPriceQuantity === undefined || rawpoPriceQuantity === null || rawpoPriceQuantity === '') {
      delete payload.poPriceQuantity
    } else {
      const numpoPriceQuantity = typeof rawpoPriceQuantity === 'number' ? rawpoPriceQuantity : Number(rawpoPriceQuantity)
      if (Number.isFinite(numpoPriceQuantity)) payload.poPriceQuantity = numpoPriceQuantity
      else delete payload.poPriceQuantity
    }
  }
  if ('valuatedStockQuantity' in payload) {
    const rawvaluatedStockQuantity = payload.valuatedStockQuantity
    if (rawvaluatedStockQuantity === undefined || rawvaluatedStockQuantity === null || rawvaluatedStockQuantity === '') {
      delete payload.valuatedStockQuantity
    } else {
      const numvaluatedStockQuantity = typeof rawvaluatedStockQuantity === 'number' ? rawvaluatedStockQuantity : Number(rawvaluatedStockQuantity)
      if (Number.isFinite(numvaluatedStockQuantity)) payload.valuatedStockQuantity = numvaluatedStockQuantity
      else delete payload.valuatedStockQuantity
    }
  }
  if ('previousPeriodStock' in payload) {
    const rawpreviousPeriodStock = payload.previousPeriodStock
    if (rawpreviousPeriodStock === undefined || rawpreviousPeriodStock === null || rawpreviousPeriodStock === '') {
      delete payload.previousPeriodStock
    } else {
      const numpreviousPeriodStock = typeof rawpreviousPeriodStock === 'number' ? rawpreviousPeriodStock : Number(rawpreviousPeriodStock)
      if (Number.isFinite(numpreviousPeriodStock)) payload.previousPeriodStock = numpreviousPeriodStock
      else delete payload.previousPeriodStock
    }
  }
  if ('totalValuatedStockValue' in payload) {
    const rawtotalValuatedStockValue = payload.totalValuatedStockValue
    if (rawtotalValuatedStockValue === undefined || rawtotalValuatedStockValue === null || rawtotalValuatedStockValue === '') {
      delete payload.totalValuatedStockValue
    } else {
      const numtotalValuatedStockValue = typeof rawtotalValuatedStockValue === 'number' ? rawtotalValuatedStockValue : Number(rawtotalValuatedStockValue)
      if (Number.isFinite(numtotalValuatedStockValue)) payload.totalValuatedStockValue = numtotalValuatedStockValue
      else delete payload.totalValuatedStockValue
    }
  }
  if ('previousPeriodValue' in payload) {
    const rawpreviousPeriodValue = payload.previousPeriodValue
    if (rawpreviousPeriodValue === undefined || rawpreviousPeriodValue === null || rawpreviousPeriodValue === '') {
      delete payload.previousPeriodValue
    } else {
      const numpreviousPeriodValue = typeof rawpreviousPeriodValue === 'number' ? rawpreviousPeriodValue : Number(rawpreviousPeriodValue)
      if (Number.isFinite(numpreviousPeriodValue)) payload.previousPeriodValue = numpreviousPeriodValue
      else delete payload.previousPeriodValue
    }
  }
  if ('referenceDocumentItem' in payload) {
    const rawreferenceDocumentItem = payload.referenceDocumentItem
    if (rawreferenceDocumentItem === undefined || rawreferenceDocumentItem === null || rawreferenceDocumentItem === '') {
      delete payload.referenceDocumentItem
    } else {
      const numreferenceDocumentItem = typeof rawreferenceDocumentItem === 'number' ? rawreferenceDocumentItem : Number(rawreferenceDocumentItem)
      if (Number.isFinite(numreferenceDocumentItem)) payload.referenceDocumentItem = numreferenceDocumentItem
      else delete payload.referenceDocumentItem
    }
  }
  if ('materialDocumentItem' in payload) {
    const rawmaterialDocumentItem = payload.materialDocumentItem
    if (rawmaterialDocumentItem === undefined || rawmaterialDocumentItem === null || rawmaterialDocumentItem === '') {
      delete payload.materialDocumentItem
    } else {
      const nummaterialDocumentItem = typeof rawmaterialDocumentItem === 'number' ? rawmaterialDocumentItem : Number(rawmaterialDocumentItem)
      if (Number.isFinite(nummaterialDocumentItem)) payload.materialDocumentItem = nummaterialDocumentItem
      else delete payload.materialDocumentItem
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

  if (props.formData?.purchaseInvoiceItemId) {
    payload.purchaseInvoiceItemId = props.formData.purchaseInvoiceItemId
    delete payload.numberingRuleCode
  }
  payload.purchaseInvoiceId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.purchaseInvoiceCode ?? masterRow.PurchaseInvoiceCode
    const masterName = masterRow.purchaseInvoiceName ?? masterRow.PurchaseInvoiceName
    if (masterCode != null && masterCode !== '' && !payload.purchaseInvoiceCode) {
      payload.purchaseInvoiceCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.purchaseInvoiceName) {
      payload.purchaseInvoiceName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseInvoiceItemId)
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

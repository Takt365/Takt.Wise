<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/procurement/condition-item/components -->
<!-- 文件名称：PurchaseConditionItemForm.vue -->
<!-- 功能描述：Takt采购条件行实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form purchase-price-item-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 scaleQuantities -->
    <TaktEditableTable
      ref="purchaseConditionScaleQuantityTableRef"
      v-model="childPurchaseConditionScaleQuantityRows"
      :columns="purchaseConditionScaleQuantityFormColumns"
      :title="purchaseConditionScaleQuantityPi.self()"
      :add-button-entity="purchaseConditionScaleQuantityPi.self()"
      id-field="purchaseConditionScaleQuantityId"
      :default-row="createDefaultPurchaseConditionScaleQuantityRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt采购条件行实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/procurement/condition-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePurchaseConditionItemI18n } from '../composables/usePurchaseConditionItemI18n'

/** 实体字段 i18n */
const pi = usePurchaseConditionItemI18n()

import type { PurchaseConditionItemCreate } from '@/types/logistics/procurement/conditionItem.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = []

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { usePurchaseConditionScaleQuantityI18n } from '../composables/usePurchaseConditionScaleQuantityI18n'

const purchaseConditionScaleQuantityPi = usePurchaseConditionScaleQuantityI18n()

const childPurchaseConditionScaleQuantityRows = ref<Record<string, unknown>[]>([])
const purchaseConditionScaleQuantityTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 purchaseConditionScaleQuantity 可编辑列 */
const purchaseConditionScaleQuantityFormColumns = computed<TaktEditableTableColumn[]>(() => [
,
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PurchaseConditionItemCreate & { purchaseConditionItemId?: string }> | null | undefined) {
  const rows_purchaseConditionScaleQuantity = ((val as any)?.scaleQuantities ?? []) as Record<string, unknown>[]
  childPurchaseConditionScaleQuantityRows.value = rows_purchaseConditionScaleQuantity
}

function createDefaultPurchaseConditionScaleQuantityRow(): Record<string, unknown> {
  return {

  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.purchaseConditionItemId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    scaleQuantities: purchaseConditionScaleQuantityTableRef.value?.getRows?.() ?? childPurchaseConditionScaleQuantityRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      purchaseConditionItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PurchaseConditionItemCreate & { purchaseConditionItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 purchaseConditionItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.purchaseConditionItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).scaleQuantities
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({

}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await purchaseConditionScaleQuantityTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.purchaseConditionItemId) {
    payload.purchaseConditionItemId = props.formData.purchaseConditionItemId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.purchaseConditionItemId)
  childPurchaseConditionScaleQuantityRows.value = []
  purchaseConditionScaleQuantityTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


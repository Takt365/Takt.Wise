<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order/components -->
<!-- 文件名称：IqcOrderForm.vue -->
<!-- 功能描述：IQC进货检验单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form iqc-order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="iqcOrderItemTableRef"
      v-model="childIqcOrderItemRows"
      :columns="iqcOrderItemFormColumns"
      :title="iqcOrderItemPi.self()"
      :add-button-entity="iqcOrderItemPi.self()"
      id-field="iqcOrderItemId"
      :default-row="createDefaultIqcOrderItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktGeneralMaterials/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="iqcOrderItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-standardCode="{ record }">
        <TaktSelect
          v-model:value="record.standardCode"
          api-url="TaktInspectionStandards/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="iqcOrderItemPi.queryPh('standardCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-samplingSchemeCode="{ record }">
        <TaktSelect
          v-model:value="record.samplingSchemeCode"
          api-url="TaktSamplingSchemes/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="iqcOrderItemPi.queryPh('samplingSchemeCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="iqcOrderItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * IQC进货检验单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/iqc-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useIqcOrderI18n } from '../composables/useIqcOrderI18n'

/** 实体字段 i18n */
const pi = useIqcOrderI18n()

import type { IqcOrderCreate } from '@/types/logistics/quality/operation/iqcOrder.types'
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
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = []

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useIqcOrderItemI18n } from '../composables/useIqcOrderItemI18n'

const iqcOrderItemPi = useIqcOrderItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childIqcOrderItemRows = ref<Record<string, unknown>[]>([])
const iqcOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedIqcOrderItemRow(row: Record<string, unknown>): boolean {
  const id = row.iqcOrderItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextIqcOrderItemLineNumber(): number {
  const rows = iqcOrderItemTableRef.value?.getRows?.() ?? childIqcOrderItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 iqcOrderItem 可编辑列 */
const iqcOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: iqcOrderItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: iqcOrderItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialDescription',
    title: iqcOrderItemPi.label('materialDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: iqcOrderItemPi.ph('materialDescription'),
    width: 180,
  },
  {
    key: 'batchCode',
    title: iqcOrderItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: iqcOrderItemPi.ph('batchCode'),
  },
  {
    key: 'purchaseQuantity',
    title: iqcOrderItemPi.label('purchaseQuantity'),
    width: 140,
  },
  {
    key: 'standardCode',
    title: iqcOrderItemPi.label('standardCode'),
    width: 140,
  },
  {
    key: 'samplingSchemeCode',
    title: iqcOrderItemPi.label('samplingSchemeCode'),
    width: 140,
  },
  {
    key: 'inspectionMethod',
    title: iqcOrderItemPi.label('inspectionMethod'),
    width: 140,
  },
  {
    key: 'sampleQuantity',
    title: iqcOrderItemPi.label('sampleQuantity'),
    width: 140,
  },
  {
    key: 'qualifiedQuantity',
    title: iqcOrderItemPi.label('qualifiedQuantity'),
    width: 140,
  },
  {
    key: 'unqualifiedQuantity',
    title: iqcOrderItemPi.label('unqualifiedQuantity'),
    width: 140,
  },
  {
    key: 'inspectionReturnQuantity',
    title: iqcOrderItemPi.label('inspectionReturnQuantity'),
    width: 140,
  },
  {
    key: 'sampleSerialCode',
    title: iqcOrderItemPi.label('sampleSerialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: iqcOrderItemPi.ph('sampleSerialCode'),
  },
  {
    key: 'inspectionDescription',
    title: iqcOrderItemPi.label('inspectionDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: iqcOrderItemPi.ph('inspectionDescription'),
    width: 180,
  },
  {
    key: 'inspectorBy',
    title: iqcOrderItemPi.label('inspectorBy'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inspectionDate',
    title: iqcOrderItemPi.label('inspectionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'judgeStatus',
    title: iqcOrderItemPi.label('judgeStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: iqcOrderItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'defectHandlings',
    title: iqcOrderItemPi.label('defectHandlings'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: iqcOrderItemPi.ph('defectHandlings'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<IqcOrderCreate & { iqcOrderId?: string }> | null | undefined) {
  const rows_iqcOrderItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childIqcOrderItemRows.value = rows_iqcOrderItem
}

function createDefaultIqcOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextIqcOrderItemLineNumber(),
    materialCode: '',
    materialDescription: '',
    batchCode: '',
    purchaseQuantity: 0,
    standardCode: '',
    samplingSchemeCode: '',
    inspectionMethod: 0,
    sampleQuantity: 0,
    qualifiedQuantity: 0,
    unqualifiedQuantity: 0,
    inspectionReturnQuantity: 0,
    sampleSerialCode: '',
    inspectionDescription: '',
    inspectorBy: '',
    inspectionDate: '',
    judgeStatus: 0,
    isObsolete: 0,
    defectHandlings: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.iqcOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: iqcOrderItemTableRef.value?.getRows?.() ?? childIqcOrderItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        iqcOrderId: masterId,
      }
      if (isUpdate && isPersistedIqcOrderItemRow(row)) {
        normalized.iqcOrderItemId = row.iqcOrderItemId
      } else {
        delete normalized.iqcOrderItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IqcOrderCreate & { iqcOrderId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 iqcOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.iqcOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.iqcOrderId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({

}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await iqcOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.iqcOrderId) {
    payload.iqcOrderId = props.formData.iqcOrderId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.iqcOrderId)
  childIqcOrderItemRows.value = []
  iqcOrderItemTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


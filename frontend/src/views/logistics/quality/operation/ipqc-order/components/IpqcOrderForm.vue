<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order/components -->
<!-- 文件名称：IpqcOrderForm.vue -->
<!-- 功能描述：IPQC制程检验单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ipqc-order-form flex flex-col min-h-0 overflow-visible"
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
      ref="ipqcOrderItemTableRef"
      v-model="childIpqcOrderItemRows"
      :columns="ipqcOrderItemFormColumns"
      :title="ipqcOrderItemPi.self()"
      :add-button-entity="ipqcOrderItemPi.self()"
      id-field="ipqcOrderItemId"
      :default-row="createDefaultIpqcOrderItemRow"
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
          :placeholder="ipqcOrderItemPi.queryPh('materialCode', 'select')"
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
          :placeholder="ipqcOrderItemPi.queryPh('standardCode', 'select')"
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
          :placeholder="ipqcOrderItemPi.queryPh('samplingSchemeCode', 'select')"
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
          :placeholder="ipqcOrderItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * IPQC制程检验单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useIpqcOrderI18n } from '../composables/useIpqcOrderI18n'

/** 实体字段 i18n */
const pi = useIpqcOrderI18n()

import type { IpqcOrderCreate } from '@/types/logistics/quality/operation/ipqcOrder.types'
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
import { useIpqcOrderItemI18n } from '../composables/useIpqcOrderItemI18n'

const ipqcOrderItemPi = useIpqcOrderItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childIpqcOrderItemRows = ref<Record<string, unknown>[]>([])
const ipqcOrderItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedIpqcOrderItemRow(row: Record<string, unknown>): boolean {
  const id = row.ipqcOrderItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextIpqcOrderItemLineNumber(): number {
  const rows = ipqcOrderItemTableRef.value?.getRows?.() ?? childIpqcOrderItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 ipqcOrderItem 可编辑列 */
const ipqcOrderItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: ipqcOrderItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: ipqcOrderItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialDescription',
    title: ipqcOrderItemPi.label('materialDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: ipqcOrderItemPi.ph('materialDescription'),
    width: 180,
  },
  {
    key: 'batchCode',
    title: ipqcOrderItemPi.label('batchCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: ipqcOrderItemPi.ph('batchCode'),
  },
  {
    key: 'productionQuantity',
    title: ipqcOrderItemPi.label('productionQuantity'),
    width: 140,
  },
  {
    key: 'standardCode',
    title: ipqcOrderItemPi.label('standardCode'),
    width: 140,
  },
  {
    key: 'samplingSchemeCode',
    title: ipqcOrderItemPi.label('samplingSchemeCode'),
    width: 140,
  },
  {
    key: 'inspectionMethod',
    title: ipqcOrderItemPi.label('inspectionMethod'),
    width: 140,
  },
  {
    key: 'sampleQuantity',
    title: ipqcOrderItemPi.label('sampleQuantity'),
    width: 140,
  },
  {
    key: 'qualifiedQuantity',
    title: ipqcOrderItemPi.label('qualifiedQuantity'),
    width: 140,
  },
  {
    key: 'unqualifiedQuantity',
    title: ipqcOrderItemPi.label('unqualifiedQuantity'),
    width: 140,
  },
  {
    key: 'inspectionReturnQuantity',
    title: ipqcOrderItemPi.label('inspectionReturnQuantity'),
    width: 140,
  },
  {
    key: 'sampleSerialCode',
    title: ipqcOrderItemPi.label('sampleSerialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: ipqcOrderItemPi.ph('sampleSerialCode'),
  },
  {
    key: 'inspectionDescription',
    title: ipqcOrderItemPi.label('inspectionDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: ipqcOrderItemPi.ph('inspectionDescription'),
    width: 180,
  },
  {
    key: 'inspectorBy',
    title: ipqcOrderItemPi.label('inspectorBy'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inspectionDate',
    title: ipqcOrderItemPi.label('inspectionDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'judgeStatus',
    title: ipqcOrderItemPi.label('judgeStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: ipqcOrderItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'defectHandlings',
    title: ipqcOrderItemPi.label('defectHandlings'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: ipqcOrderItemPi.ph('defectHandlings'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<IpqcOrderCreate & { ipqcOrderId?: string }> | null | undefined) {
  const rows_ipqcOrderItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childIpqcOrderItemRows.value = rows_ipqcOrderItem
}

function createDefaultIpqcOrderItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextIpqcOrderItemLineNumber(),
    materialCode: '',
    materialDescription: '',
    batchCode: '',
    productionQuantity: 0,
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
  const masterId = props.formData?.ipqcOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: ipqcOrderItemTableRef.value?.getRows?.() ?? childIpqcOrderItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        ipqcOrderId: masterId,
      }
      if (isUpdate && isPersistedIpqcOrderItemRow(row)) {
        normalized.ipqcOrderItemId = row.ipqcOrderItemId
      } else {
        delete normalized.ipqcOrderItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IpqcOrderCreate & { ipqcOrderId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ipqcOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ipqcOrderId) {
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
    if (!props.formData?.ipqcOrderId) {
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
  await ipqcOrderItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.ipqcOrderId) {
    payload.ipqcOrderId = props.formData.ipqcOrderId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ipqcOrderId)
  childIpqcOrderItemRows.value = []
  ipqcOrderItemTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


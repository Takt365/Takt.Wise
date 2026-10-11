<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/incident/components -->
<!-- 文件名称：IncidentForm.vue -->
<!-- 功能描述：品质事故主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form incident-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 incidentItems -->
    <TaktEditableTable
      ref="qualityIncidentItemTableRef"
      v-model="childQualityIncidentItemRows"
      :columns="qualityIncidentItemFormColumns"
      :title="qualityIncidentItemPi.self()"
      :add-button-entity="qualityIncidentItemPi.self()"
      id-field="qualityIncidentItemId"
      :default-row="createDefaultQualityIncidentItemRow"
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
          :placeholder="qualityIncidentItemPi.queryPh('materialCode', 'select')"
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
          :placeholder="qualityIncidentItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质事故主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/incident/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useQualityIncidentI18n } from '../composables/useIncidentI18n'

/** 实体字段 i18n */
const pi = useQualityIncidentI18n()

import type { QualityIncidentCreate } from '@/types/logistics/quality/cost/incident.types'
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
}
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = []

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useQualityIncidentItemI18n } from '../composables/useIncidentItemI18n'

const qualityIncidentItemPi = useQualityIncidentItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childQualityIncidentItemRows = ref<Record<string, unknown>[]>([])
const qualityIncidentItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedQualityIncidentItemRow(row: Record<string, unknown>): boolean {
  const id = row.qualityIncidentItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextQualityIncidentItemLineNumber(): number {
  const rows = qualityIncidentItemTableRef.value?.getRows?.() ?? childQualityIncidentItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 qualityIncidentItem 可编辑列 */
const qualityIncidentItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: qualityIncidentItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: qualityIncidentItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialDescription',
    title: qualityIncidentItemPi.label('materialDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: qualityIncidentItemPi.ph('materialDescription'),
    width: 180,
  },
  {
    key: 'scrapCost',
    title: qualityIncidentItemPi.label('scrapCost'),
    width: 140,
  },
  {
    key: 'scrapSize',
    title: qualityIncidentItemPi.label('scrapSize'),
    width: 140,
  },
  {
    key: 'partPrice',
    title: qualityIncidentItemPi.label('partPrice'),
    width: 140,
  },
  {
    key: 'scrapReasonCost',
    title: qualityIncidentItemPi.label('scrapReasonCost'),
    width: 140,
  },
  {
    key: 'freightCharges',
    title: qualityIncidentItemPi.label('freightCharges'),
    width: 140,
  },
  {
    key: 'otherExpenses',
    title: qualityIncidentItemPi.label('otherExpenses'),
    width: 140,
  },
  {
    key: 'reasonWorkTimeMinutes',
    title: qualityIncidentItemPi.label('reasonWorkTimeMinutes'),
    width: 140,
  },
  {
    key: 'tax',
    title: qualityIncidentItemPi.label('tax'),
    width: 140,
  },
  {
    key: 'reasonOtherExpenses',
    title: qualityIncidentItemPi.label('reasonOtherExpenses'),
    width: 140,
  },
  {
    key: 'scrapNote',
    title: qualityIncidentItemPi.label('scrapNote'),
    editor: 'textarea',
    rows: 1,
    placeholder: qualityIncidentItemPi.ph('scrapNote'),
    width: 180,
  },
  {
    key: 'isObsolete',
    title: qualityIncidentItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<QualityIncidentCreate & { qualityIncidentId?: string }> | null | undefined) {
  const rows_qualityIncidentItem = ((val as any)?.incidentItems ?? []) as Record<string, unknown>[]
  childQualityIncidentItemRows.value = rows_qualityIncidentItem
}

function createDefaultQualityIncidentItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextQualityIncidentItemLineNumber(),
    materialCode: '',
    materialDescription: '',
    scrapCost: 0,
    scrapSize: 0,
    partPrice: 0,
    scrapReasonCost: 0,
    freightCharges: 0,
    otherExpenses: 0,
    reasonWorkTimeMinutes: 0,
    tax: 0,
    reasonOtherExpenses: 0,
    scrapNote: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.qualityIncidentId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    incidentItems: qualityIncidentItemTableRef.value?.getRows?.() ?? childQualityIncidentItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        qualityIncidentId: masterId,
      }
      if (isUpdate && isPersistedQualityIncidentItemRow(row)) {
        normalized.qualityIncidentItemId = row.qualityIncidentItemId
      } else {
        delete normalized.qualityIncidentItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIncidentCreate & { qualityIncidentId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIncidentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIncidentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).incidentItems
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.qualityIncidentId) {
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
  await qualityIncidentItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.qualityIncidentId) {
    payload.qualityIncidentId = props.formData.qualityIncidentId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.qualityIncidentId)
  childQualityIncidentItemRows.value = []
  qualityIncidentItemTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


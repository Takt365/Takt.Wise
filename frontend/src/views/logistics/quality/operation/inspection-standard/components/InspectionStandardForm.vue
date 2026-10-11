<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/inspection-standard/components -->
<!-- 文件名称：InspectionStandardForm.vue -->
<!-- 功能描述：检验标准实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form inspection-standard-form flex flex-col min-h-0 overflow-visible"
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
      ref="inspectionStandardItemTableRef"
      v-model="childInspectionStandardItemRows"
      :columns="inspectionStandardItemFormColumns"
      :title="inspectionStandardItemPi.self()"
      :add-button-entity="inspectionStandardItemPi.self()"
      id-field="inspectionStandardItemId"
      :default-row="createDefaultInspectionStandardItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-itemType="{ record }">
        <TaktSelect
          v-model:value="record.itemType"
          dict-type="logistics_quality_inspection_item_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="inspectionStandardItemPi.ph('itemType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectLevel="{ record }">
        <TaktSelect
          v-model:value="record.defectLevel"
          dict-type="logistics_quality_defect_severity_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="inspectionStandardItemPi.ph('defectLevel')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-inspectionMode="{ record }">
        <TaktSelect
          v-model:value="record.inspectionMode"
          dict-type="logistics_quality_inspection_mode"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="inspectionStandardItemPi.ph('inspectionMode')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isQualifiedBasis="{ record }">
        <TaktSelect
          v-model:value="record.isQualifiedBasis"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="inspectionStandardItemPi.ph('isQualifiedBasis')"
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
          :placeholder="inspectionStandardItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 检验标准实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/inspection-standard/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useInspectionStandardI18n } from '../composables/useInspectionStandardI18n'

/** 实体字段 i18n */
const pi = useInspectionStandardI18n()

import type { InspectionStandardCreate } from '@/types/logistics/quality/operation/inspectionStandard.types'
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
import { useInspectionStandardItemI18n } from '../composables/useInspectionStandardItemI18n'

const inspectionStandardItemPi = useInspectionStandardItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childInspectionStandardItemRows = ref<Record<string, unknown>[]>([])
const inspectionStandardItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedInspectionStandardItemRow(row: Record<string, unknown>): boolean {
  const id = row.inspectionStandardItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextInspectionStandardItemLineNumber(): number {
  const rows = inspectionStandardItemTableRef.value?.getRows?.() ?? childInspectionStandardItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 inspectionStandardItem 可编辑列 */
const inspectionStandardItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: inspectionStandardItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'itemCode',
    title: inspectionStandardItemPi.label('itemCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemName',
    title: inspectionStandardItemPi.label('itemName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'itemType',
    title: inspectionStandardItemPi.label('itemType'),
    width: 140,
  },
  {
    key: 'defectLevel',
    title: inspectionStandardItemPi.label('defectLevel'),
    width: 140,
  },
  {
    key: 'inspectionMode',
    title: inspectionStandardItemPi.label('inspectionMode'),
    width: 140,
  },
  {
    key: 'standardValue',
    title: inspectionStandardItemPi.label('standardValue'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'upperLimit',
    title: inspectionStandardItemPi.label('upperLimit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lowerLimit',
    title: inspectionStandardItemPi.label('lowerLimit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inspectionTool',
    title: inspectionStandardItemPi.label('inspectionTool'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'inspectionMethodDescription',
    title: inspectionStandardItemPi.label('inspectionMethodDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: inspectionStandardItemPi.ph('inspectionMethodDescription'),
    width: 180,
  },
  {
    key: 'acceptanceCriteria',
    title: inspectionStandardItemPi.label('acceptanceCriteria'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'rejectionCriteria',
    title: inspectionStandardItemPi.label('rejectionCriteria'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'isQualifiedBasis',
    title: inspectionStandardItemPi.label('isQualifiedBasis'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: inspectionStandardItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<InspectionStandardCreate & { inspectionStandardId?: string }> | null | undefined) {
  const rows_inspectionStandardItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childInspectionStandardItemRows.value = rows_inspectionStandardItem
}

function createDefaultInspectionStandardItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextInspectionStandardItemLineNumber(),
    itemCode: '',
    itemName: '',
    itemType: 0,
    defectLevel: '',
    inspectionMode: 0,
    standardValue: '',
    upperLimit: '',
    lowerLimit: '',
    inspectionTool: '',
    inspectionMethodDescription: '',
    acceptanceCriteria: '',
    rejectionCriteria: '',
    isQualifiedBasis: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.inspectionStandardId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: inspectionStandardItemTableRef.value?.getRows?.() ?? childInspectionStandardItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        inspectionStandardId: masterId,
      }
      if (isUpdate && isPersistedInspectionStandardItemRow(row)) {
        normalized.inspectionStandardItemId = row.inspectionStandardItemId
      } else {
        delete normalized.inspectionStandardItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<InspectionStandardCreate & { inspectionStandardId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 inspectionStandardId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.inspectionStandardId) {
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
    if (!props.formData?.inspectionStandardId) {
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
  await inspectionStandardItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.inspectionStandardId) {
    payload.inspectionStandardId = props.formData.inspectionStandardId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.inspectionStandardId)
  childInspectionStandardItemRows.value = []
  inspectionStandardItemTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


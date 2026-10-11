<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：WorkOrderForm.vue -->
<!-- 功能描述：维护工单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-order-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 materials -->
    <TaktEditableTable
      ref="maintenanceWorkOrderMaterialTableRef"
      v-model="childMaintenanceWorkOrderMaterialRows"
      :columns="maintenanceWorkOrderMaterialFormColumns"
      :title="maintenanceWorkOrderMaterialPi.self()"
      :add-button-entity="maintenanceWorkOrderMaterialPi.self()"
      id-field="maintenanceWorkOrderMaterialId"
      :default-row="createDefaultMaintenanceWorkOrderMaterialRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="maintenanceWorkOrderMaterialPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 维护工单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/work-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaintenanceWorkOrderI18n } from '../composables/useWorkOrderI18n'

/** 实体字段 i18n */
const pi = useMaintenanceWorkOrderI18n()

import type { MaintenanceWorkOrderCreate } from '@/types/logistics/maintenance/workOrder.types'
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
import { useMaintenanceWorkOrderMaterialI18n } from '../composables/useWorkOrderMaterialI18n'

const maintenanceWorkOrderMaterialPi = useMaintenanceWorkOrderMaterialI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaintenanceWorkOrderMaterialRows = ref<Record<string, unknown>[]>([])
const maintenanceWorkOrderMaterialTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaintenanceWorkOrderMaterialRow(row: Record<string, unknown>): boolean {
  const id = row.maintenanceWorkOrderMaterialId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaintenanceWorkOrderMaterialLineNumber(): number {
  const rows = maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 maintenanceWorkOrderMaterial 可编辑列 */
const maintenanceWorkOrderMaterialFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: maintenanceWorkOrderMaterialPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialId',
    title: maintenanceWorkOrderMaterialPi.label('materialId'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialCode',
    title: maintenanceWorkOrderMaterialPi.label('materialCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialDescription',
    title: maintenanceWorkOrderMaterialPi.label('materialDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: maintenanceWorkOrderMaterialPi.ph('materialDescription'),
    width: 180,
  },
  {
    key: 'requiredQuantity',
    title: maintenanceWorkOrderMaterialPi.label('requiredQuantity'),
    width: 140,
  },
  {
    key: 'issuedQuantity',
    title: maintenanceWorkOrderMaterialPi.label('issuedQuantity'),
    width: 140,
  },
  {
    key: 'materialUnit',
    title: maintenanceWorkOrderMaterialPi.label('materialUnit'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'unitPrice',
    title: maintenanceWorkOrderMaterialPi.label('unitPrice'),
    width: 140,
  },
  {
    key: 'amount',
    title: maintenanceWorkOrderMaterialPi.label('amount'),
    width: 140,
  },
  {
    key: 'warehouseCode',
    title: maintenanceWorkOrderMaterialPi.label('warehouseCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('warehouseCode'),
  },
  {
    key: 'storageLocation',
    title: maintenanceWorkOrderMaterialPi.label('storageLocation'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: maintenanceWorkOrderMaterialPi.ph('storageLocation'),
  },
  {
    key: 'issueStatus',
    title: maintenanceWorkOrderMaterialPi.label('issueStatus'),
    width: 140,
  },
  {
    key: 'issueTime',
    title: maintenanceWorkOrderMaterialPi.label('issueTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'isObsolete',
    title: maintenanceWorkOrderMaterialPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null | undefined) {
  const rows_maintenanceWorkOrderMaterial = ((val as any)?.materials ?? []) as Record<string, unknown>[]
  childMaintenanceWorkOrderMaterialRows.value = rows_maintenanceWorkOrderMaterial
}

function createDefaultMaintenanceWorkOrderMaterialRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMaintenanceWorkOrderMaterialLineNumber(),
    materialId: '',
    materialCode: '',
    materialDescription: '',
    requiredQuantity: 0,
    issuedQuantity: 0,
    materialUnit: '',
    unitPrice: 0,
    amount: 0,
    warehouseCode: '',
    storageLocation: '',
    issueStatus: 0,
    issueTime: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.maintenanceWorkOrderId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    materials: maintenanceWorkOrderMaterialTableRef.value?.getRows?.() ?? childMaintenanceWorkOrderMaterialRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        maintenanceWorkOrderId: masterId,
      }
      if (isUpdate && isPersistedMaintenanceWorkOrderMaterialRow(row)) {
        normalized.maintenanceWorkOrderMaterialId = row.maintenanceWorkOrderMaterialId
      } else {
        delete normalized.maintenanceWorkOrderMaterialId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceWorkOrderCreate & { maintenanceWorkOrderId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceWorkOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceWorkOrderId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).materials
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
    if (!props.formData?.maintenanceWorkOrderId) {
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
  await maintenanceWorkOrderMaterialTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.maintenanceWorkOrderId) {
    payload.maintenanceWorkOrderId = props.formData.maintenanceWorkOrderId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.maintenanceWorkOrderId)
  childMaintenanceWorkOrderMaterialRows.value = []
  maintenanceWorkOrderMaterialTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


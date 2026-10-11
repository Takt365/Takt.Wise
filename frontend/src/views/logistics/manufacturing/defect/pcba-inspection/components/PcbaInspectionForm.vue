<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/components -->
<!-- 文件名称：PcbaInspectionForm.vue -->
<!-- 功能描述：PCBA检查日报实体 不良率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-inspection-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 pcbaInspectionDetails -->
    <TaktEditableTable
      ref="pcbaInspectionDetailTableRef"
      v-model="childPcbaInspectionDetailRows"
      :columns="pcbaInspectionDetailFormColumns"
      :title="pcbaInspectionDetailPi.self()"
      :add-button-entity="pcbaInspectionDetailPi.self()"
      id-field="pcbaInspectionDetailId"
      :default-row="createDefaultPcbaInspectionDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-pcbaBoardType="{ record }">
        <TaktSelect
          v-model:value="record.pcbaBoardType"
          dict-type="logistics_manufacturing_pcba_function"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('pcbaBoardType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-visualInspectionLine="{ record }">
        <TaktSelect
          v-model:value="record.visualInspectionLine"
          dict-type="logistics_manufacturing_visual_inspection_line_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('visualInspectionLine')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-aoiLine="{ record }">
        <TaktSelect
          v-model:value="record.aoiLine"
          dict-type="logistics_manufacturing_aoi_inspection_line_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('aoiLine')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-shiftNo="{ record }">
        <TaktSelect
          v-model:value="record.shiftNo"
          dict-type="logistics_manufacturing_shift_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('shiftNo')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-inspectorName="{ record }">
        <TaktSelect
          v-model:value="record.inspectorName"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.queryPh('inspectorName', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-inspectionStatus="{ record }">
        <TaktSelect
          v-model:value="record.inspectionStatus"
          dict-type="logistics_manufacturing_pcba_inspection_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('inspectionStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-TeamCode="{ record }">
        <TaktSelect
          v-model:value="record.TeamCode"
          api-url="TaktProductionTeams/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.queryPh('TeamCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectLocation="{ record }">
        <TaktSelect
          v-model:value="record.defectLocation"
          dict-type="logistics_manufacturing_pcb_location_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaInspectionDetailPi.ph('defectLocation')"
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
          :placeholder="pcbaInspectionDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA检查日报实体 不良率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaInspectionI18n } from '../composables/usePcbaInspectionI18n'

/** 实体字段 i18n */
const pi = usePcbaInspectionI18n()

import type { PcbaInspectionCreate } from '@/types/logistics/manufacturing/defect/pcbaInspection.types'
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
import { usePcbaInspectionDetailI18n } from '../composables/usePcbaInspectionDetailI18n'

const pcbaInspectionDetailPi = usePcbaInspectionDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPcbaInspectionDetailRows = ref<Record<string, unknown>[]>([])
const pcbaInspectionDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPcbaInspectionDetailRow(row: Record<string, unknown>): boolean {
  const id = row.pcbaInspectionDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPcbaInspectionDetailLineNumber(): number {
  const rows = pcbaInspectionDetailTableRef.value?.getRows?.() ?? childPcbaInspectionDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 pcbaInspectionDetail 可编辑列 */
const pcbaInspectionDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: pcbaInspectionDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'pcbaBoardType',
    title: pcbaInspectionDetailPi.label('pcbaBoardType'),
    width: 140,
  },
  {
    key: 'visualInspectionLine',
    title: pcbaInspectionDetailPi.label('visualInspectionLine'),
    width: 140,
  },
  {
    key: 'aoiLine',
    title: pcbaInspectionDetailPi.label('aoiLine'),
    width: 140,
  },
  {
    key: 'bSideAssemblyDate',
    title: pcbaInspectionDetailPi.label('bSideAssemblyDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'tSideAssemblyDate',
    title: pcbaInspectionDetailPi.label('tSideAssemblyDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'shiftNo',
    title: pcbaInspectionDetailPi.label('shiftNo'),
    width: 140,
  },
  {
    key: 'inspectorName',
    title: pcbaInspectionDetailPi.label('inspectorName'),
    width: 140,
  },
  {
    key: 'dailyCompletedQty',
    title: pcbaInspectionDetailPi.label('dailyCompletedQty'),
    width: 140,
  },
  {
    key: 'inspectionQty',
    title: pcbaInspectionDetailPi.label('inspectionQty'),
    width: 140,
  },
  {
    key: 'inspectionStatus',
    title: pcbaInspectionDetailPi.label('inspectionStatus'),
    width: 140,
  },
  {
    key: 'TeamCode',
    title: pcbaInspectionDetailPi.label('TeamCode'),
    width: 140,
  },
  {
    key: 'inspectionWorkHours',
    title: pcbaInspectionDetailPi.label('inspectionWorkHours'),
    width: 140,
  },
  {
    key: 'aoiWorkHours',
    title: pcbaInspectionDetailPi.label('aoiWorkHours'),
    width: 140,
  },
  {
    key: 'defectQty',
    title: pcbaInspectionDetailPi.label('defectQty'),
    width: 140,
  },
  {
    key: 'handPlacement',
    title: pcbaInspectionDetailPi.label('handPlacement'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaInspectionDetailPi.ph('handPlacement'),
  },
  {
    key: 'serialNumber',
    title: pcbaInspectionDetailPi.label('serialNumber'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaInspectionDetailPi.ph('serialNumber'),
  },
  {
    key: 'content',
    title: pcbaInspectionDetailPi.label('content'),
    editor: 'textarea',
    rows: 1,
    placeholder: pcbaInspectionDetailPi.ph('content'),
    width: 180,
  },
  {
    key: 'defectLocation',
    title: pcbaInspectionDetailPi.label('defectLocation'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: pcbaInspectionDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaInspectionCreate & { pcbaInspectionId?: string }> | null | undefined) {
  const rows_pcbaInspectionDetail = ((val as any)?.pcbaInspectionDetails ?? []) as Record<string, unknown>[]
  childPcbaInspectionDetailRows.value = rows_pcbaInspectionDetail
}

function createDefaultPcbaInspectionDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextPcbaInspectionDetailLineNumber(),
    pcbaBoardType: '',
    visualInspectionLine: '',
    aoiLine: '',
    bSideAssemblyDate: '',
    tSideAssemblyDate: '',
    shiftNo: 0,
    inspectorName: '',
    dailyCompletedQty: 0,
    inspectionQty: 0,
    inspectionStatus: 0,
    TeamCode: '',
    inspectionWorkHours: 0,
    aoiWorkHours: 0,
    defectQty: 0,
    handPlacement: '',
    serialNumber: '',
    content: '',
    defectLocation: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaInspectionId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    pcbaInspectionDetails: pcbaInspectionDetailTableRef.value?.getRows?.() ?? childPcbaInspectionDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        pcbaInspectionId: masterId,
      }
      if (isUpdate && isPersistedPcbaInspectionDetailRow(row)) {
        normalized.pcbaInspectionDetailId = row.pcbaInspectionDetailId
      } else {
        delete normalized.pcbaInspectionDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaInspectionCreate & { pcbaInspectionId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaInspectionId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaInspectionId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaInspectionDetails
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
    if (!props.formData?.pcbaInspectionId) {
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
  await pcbaInspectionDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.pcbaInspectionId) {
    payload.pcbaInspectionId = props.formData.pcbaInspectionId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaInspectionId)
  childPcbaInspectionDetailRows.value = []
  pcbaInspectionDetailTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-repair/components -->
<!-- 文件名称：PcbaRepairForm.vue -->
<!-- 功能描述：PCBA改修日报实体 不良率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-repair-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <div :class="formContentClass">
      <a-row :gutter="24">

      </a-row>
    </div>
    <!-- 下：子表 pcbaRepairDetails -->
    <TaktEditableTable
      ref="pcbaRepairDetailTableRef"
      v-model="childPcbaRepairDetailRows"
      :columns="pcbaRepairDetailFormColumns"
      :title="pcbaRepairDetailPi.self()"
      :add-button-entity="pcbaRepairDetailPi.self()"
      id-field="pcbaRepairDetailId"
      :default-row="createDefaultPcbaRepairDetailRow"
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
          :placeholder="pcbaRepairDetailPi.ph('pcbaBoardType')"
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
          :placeholder="pcbaRepairDetailPi.queryPh('TeamCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectEngineering="{ record }">
        <TaktSelect
          v-model:value="record.defectEngineering"
          dict-type="logistics_manufacturing_defect_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectEngineering')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectResponsibility="{ record }">
        <TaktSelect
          v-model:value="record.defectResponsibility"
          dict-type="logistics_manufacturing_defect_responsibility_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectResponsibility')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectNature="{ record }">
        <TaktSelect
          v-model:value="record.defectNature"
          dict-type="logistics_manufacturing_defect_nature"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.ph('defectNature')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-repairOperator="{ record }">
        <TaktSelect
          v-model:value="record.repairOperator"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="pcbaRepairDetailPi.queryPh('repairOperator', 'select')"
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
          :placeholder="pcbaRepairDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA改修日报实体 不良率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/pcba-repair/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaRepairI18n } from '../composables/usePcbaRepairI18n'

/** 实体字段 i18n */
const pi = usePcbaRepairI18n()

import type { PcbaRepairCreate } from '@/types/logistics/manufacturing/defect/pcbaRepair.types'
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
import { usePcbaRepairDetailI18n } from '../composables/usePcbaRepairDetailI18n'

const pcbaRepairDetailPi = usePcbaRepairDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childPcbaRepairDetailRows = ref<Record<string, unknown>[]>([])
const pcbaRepairDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedPcbaRepairDetailRow(row: Record<string, unknown>): boolean {
  const id = row.pcbaRepairDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextPcbaRepairDetailLineNumber(): number {
  const rows = pcbaRepairDetailTableRef.value?.getRows?.() ?? childPcbaRepairDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 pcbaRepairDetail 可编辑列 */
const pcbaRepairDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: pcbaRepairDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'pcbaBoardType',
    title: pcbaRepairDetailPi.label('pcbaBoardType'),
    width: 140,
  },
  {
    key: 'prodActualQty',
    title: pcbaRepairDetailPi.label('prodActualQty'),
    width: 140,
  },
  {
    key: 'TeamCode',
    title: pcbaRepairDetailPi.label('TeamCode'),
    width: 140,
  },
  {
    key: 'cardCode',
    title: pcbaRepairDetailPi.label('cardCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('cardCode'),
  },
  {
    key: 'defectSymptom',
    title: pcbaRepairDetailPi.label('defectSymptom'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('defectSymptom'),
  },
  {
    key: 'defectEngineering',
    title: pcbaRepairDetailPi.label('defectEngineering'),
    width: 140,
  },
  {
    key: 'defectReason',
    title: pcbaRepairDetailPi.label('defectReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: pcbaRepairDetailPi.ph('defectReason'),
  },
  {
    key: 'defectQty',
    title: pcbaRepairDetailPi.label('defectQty'),
    width: 140,
  },
  {
    key: 'defectResponsibility',
    title: pcbaRepairDetailPi.label('defectResponsibility'),
    width: 140,
  },
  {
    key: 'defectNature',
    title: pcbaRepairDetailPi.label('defectNature'),
    width: 140,
  },
  {
    key: 'repairOperator',
    title: pcbaRepairDetailPi.label('repairOperator'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: pcbaRepairDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<PcbaRepairCreate & { pcbaRepairId?: string }> | null | undefined) {
  const rows_pcbaRepairDetail = ((val as any)?.pcbaRepairDetails ?? []) as Record<string, unknown>[]
  childPcbaRepairDetailRows.value = rows_pcbaRepairDetail
}

function createDefaultPcbaRepairDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextPcbaRepairDetailLineNumber(),
    pcbaBoardType: '',
    prodActualQty: 0,
    TeamCode: '',
    cardCode: '',
    defectSymptom: '',
    defectEngineering: '',
    defectReason: '',
    defectQty: 0,
    defectResponsibility: '',
    defectNature: '',
    repairOperator: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.pcbaRepairId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    pcbaRepairDetails: pcbaRepairDetailTableRef.value?.getRows?.() ?? childPcbaRepairDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        pcbaRepairId: masterId,
      }
      if (isUpdate && isPersistedPcbaRepairDetailRow(row)) {
        normalized.pcbaRepairDetailId = row.pcbaRepairDetailId
      } else {
        delete normalized.pcbaRepairDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaRepairCreate & { pcbaRepairId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaRepairId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaRepairId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaRepairDetails
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
    if (!props.formData?.pcbaRepairId) {
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
  await pcbaRepairDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.pcbaRepairId) {
    payload.pcbaRepairId = props.formData.pcbaRepairId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaRepairId)
  childPcbaRepairDetailRows.value = []
  pcbaRepairDetailTableRef.value?.resetRows?.()
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


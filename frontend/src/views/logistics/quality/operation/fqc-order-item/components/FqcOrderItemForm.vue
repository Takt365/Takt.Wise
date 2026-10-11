<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order-item/components -->
<!-- 文件名称：FqcOrderItemForm.vue -->
<!-- 功能描述：FQC出货检验单明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form fqc-order-item-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="fqc-order-item-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
              <a-col :span="12">
                <a-form-item
                  :label="t('common.page.entity.culturecode')"
                  name="cultureCode"
                >
                  <a-input
                    v-model:value="formState.cultureCode"
                    disabled
                    :placeholder="t('common.page.form.placeholder.input')"
                  />
                </a-form-item>
              </a-col>
            <a-col :span="24">
              <a-form-item
                name="extField"
                class="takt-form-item-ext-field"
              >
                <template #label>
                  <span class="takt-form-ext-field-label">
                    <a-tooltip
                      :title="t('common.page.entity.extfieldhint')"
                      placement="top"
                    >
                      <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
                    </a-tooltip>
                    <span>{{ pi.label('extField') }}</span>
                  </span>
                </template>
                <a-textarea
                  v-model:value="formState.extField"
                  :placeholder="t('common.page.form.placeholder.extfield')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
                  :rows="4"
                  show-count
                  :maxlength="400"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 defectHandlings -->
    <TaktEditableTable
      ref="fqcDefectHandlingTableRef"
      v-model="childFqcDefectHandlingRows"
      :columns="fqcDefectHandlingFormColumns"
      :title="fqcDefectHandlingPi.self()"
      :add-button-entity="fqcDefectHandlingPi.self()"
      id-field="fqcDefectHandlingId"
      :default-row="createDefaultFqcDefectHandlingRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-defectType="{ record }">
        <TaktSelect
          v-model:value="record.defectType"
          dict-type="logistics_quality_defect_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.ph('defectType')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-handlingMethod="{ record }">
        <TaktSelect
          v-model:value="record.handlingMethod"
          dict-type="logistics_quality_defect_handling_method"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.ph('handlingMethod')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-responsibleDept="{ record }">
        <TaktSelect
          v-model:value="record.responsibleDept"
          api-url="TaktDepts/tree-options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.queryPh('responsibleDept', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-responsibleGroupCode="{ record }">
        <TaktSelect
          v-model:value="record.responsibleGroupCode"
          api-url="TaktQualityGroups/options"
          :api-params="{ qualityGroupCategory: 3 }"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.queryPh('responsibleGroupCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-handlerGroupCode="{ record }">
        <TaktSelect
          v-model:value="record.handlerGroupCode"
          api-url="TaktQualityGroups/options"
          :api-params="{ qualityGroupCategory: 4 }"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.queryPh('handlerGroupCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-verifyDeptId="{ record }">
        <TaktSelect
          v-model:value="record.verifyDeptId"
          api-url="TaktDepts/tree-options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.queryPh('verifyDeptId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-qualityOwnerGroupCode="{ record }">
        <TaktSelect
          v-model:value="record.qualityOwnerGroupCode"
          api-url="TaktQualityGroups/options"
          :api-params="{ qualityGroupCategory: 5 }"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.queryPh('qualityOwnerGroupCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-verifyAt="{ record }">
        <a-date-picker
          v-model:value="record.verifyAt"
          class="w-full"
          value-format="YYYY-MM-DD"
          :placeholder="fqcDefectHandlingPi.ph('verifyAt')"
          :disabled="loading"
        />
      </template>
      <template #cell-verifyStatus="{ record }">
        <TaktSelect
          v-model:value="record.verifyStatus"
          dict-type="logistics_quality_defect_verify_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.ph('verifyStatus')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-handlingStatus="{ record }">
        <TaktSelect
          v-model:value="record.handlingStatus"
          dict-type="logistics_quality_defect_handling_status"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="fqcDefectHandlingPi.ph('handlingStatus')"
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
          :placeholder="fqcDefectHandlingPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * FQC出货检验单明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/fqc-order-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useFqcOrderItemI18n } from '../composables/useFqcOrderItemI18n'

/** 实体字段 i18n */
const pi = useFqcOrderItemI18n()

import type { FqcOrderItemCreate } from '@/types/logistics/quality/operation/fqcOrderItem.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","fqcOrderId","fqcOrderCode","lineNumber","materialCode","materialDescription","batchCode","warehouseQuantity","standardCode","samplingSchemeCode","inspectionMethod","sampleQuantity","qualifiedQuantity","unqualifiedQuantity","inspectionReturnQuantity","sampleSerialCode","inspectionDescription","inspectorBy","inspectionDate","judgeStatus","isObsolete","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useFqcDefectHandlingI18n } from '../composables/useFqcDefectHandlingI18n'

const fqcDefectHandlingPi = useFqcDefectHandlingI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childFqcDefectHandlingRows = ref<Record<string, unknown>[]>([])
const fqcDefectHandlingTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedFqcDefectHandlingRow(row: Record<string, unknown>): boolean {
  const id = row.fqcDefectHandlingId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextFqcDefectHandlingLineNumber(): number {
  const rows = fqcDefectHandlingTableRef.value?.getRows?.() ?? childFqcDefectHandlingRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 fqcDefectHandling 可编辑列 */
const fqcDefectHandlingFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'fqcDefectHandlingCode',
    title: fqcDefectHandlingPi.label('fqcDefectHandlingCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'fqcOrderCode',
    title: fqcDefectHandlingPi.label('fqcOrderCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: fqcDefectHandlingPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'defectType',
    title: fqcDefectHandlingPi.label('defectType'),
    width: 140,
  },
  {
    key: 'defectCode',
    title: fqcDefectHandlingPi.label('defectCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'defectDescription',
    title: fqcDefectHandlingPi.label('defectDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: fqcDefectHandlingPi.ph('defectDescription'),
    width: 180,
  },
  {
    key: 'defectQuantity',
    title: fqcDefectHandlingPi.label('defectQuantity'),
    width: 140,
  },
  {
    key: 'handlingMethod',
    title: fqcDefectHandlingPi.label('handlingMethod'),
    width: 140,
  },
  {
    key: 'handlingDescription',
    title: fqcDefectHandlingPi.label('handlingDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: fqcDefectHandlingPi.ph('handlingDescription'),
    width: 180,
  },
  {
    key: 'responsibleDept',
    title: fqcDefectHandlingPi.label('responsibleDept'),
    width: 140,
  },
  {
    key: 'responsibleGroupCode',
    title: fqcDefectHandlingPi.label('responsibleGroupCode'),
    width: 140,
  },
  {
    key: 'handlerGroupCode',
    title: fqcDefectHandlingPi.label('handlerGroupCode'),
    width: 140,
  },
  {
    key: 'handlingAt',
    title: fqcDefectHandlingPi.label('handlingAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'verifyDeptId',
    title: fqcDefectHandlingPi.label('verifyDeptId'),
    width: 160,
  },
  {
    key: 'qualityOwnerGroupCode',
    title: fqcDefectHandlingPi.label('qualityOwnerGroupCode'),
    width: 160,
  },
  {
    key: 'verifyAt',
    title: fqcDefectHandlingPi.label('verifyAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'verifyStatus',
    title: fqcDefectHandlingPi.label('verifyStatus'),
    width: 140,
  },
  {
    key: 'correctiveAction',
    title: fqcDefectHandlingPi.label('correctiveAction'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: fqcDefectHandlingPi.ph('correctiveAction'),
  },
  {
    key: 'defectImages',
    title: fqcDefectHandlingPi.label('defectImages'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: fqcDefectHandlingPi.ph('defectImages'),
  },
  {
    key: 'attachments',
    title: fqcDefectHandlingPi.label('attachments'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: fqcDefectHandlingPi.ph('attachments'),
  },
  {
    key: 'handlingStatus',
    title: fqcDefectHandlingPi.label('handlingStatus'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: fqcDefectHandlingPi.label('isObsolete'),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<FqcOrderItemCreate & { fqcOrderItemId?: string }> | null | undefined) {
  const rows_fqcDefectHandling = ((val as any)?.defectHandlings ?? []) as Record<string, unknown>[]
  childFqcDefectHandlingRows.value = rows_fqcDefectHandling
}

function createDefaultFqcDefectHandlingRow(): Record<string, unknown> {
  return {
    fqcDefectHandlingCode: '',
    fqcOrderCode: '',
    lineNumber: allocateNextFqcDefectHandlingLineNumber(),
    defectType: 0,
    defectCode: '',
    defectDescription: '',
    defectQuantity: 0,
    handlingMethod: 0,
    handlingDescription: '',
    responsibleDept: '',
    responsibleGroupCode: '',
    handlerGroupCode: '',
    handlingAt: '',
    verifyDeptId: '',
    qualityOwnerGroupCode: '',
    verifyAt: '',
    verifyStatus: 0,
    correctiveAction: '',
    defectImages: '',
    attachments: '',
    handlingStatus: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.fqcOrderItemId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    defectHandlings: fqcDefectHandlingTableRef.value?.getRows?.() ?? childFqcDefectHandlingRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        fqcOrderItemId: masterId,
      }
      if (isUpdate && isPersistedFqcDefectHandlingRow(row)) {
        normalized.fqcDefectHandlingId = row.fqcDefectHandlingId
      } else {
        delete normalized.fqcDefectHandlingId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<FqcOrderItemCreate & { fqcOrderItemId?: string }> | null
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
  inspectionMethod: 2,
  judgeStatus: 0
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 fqcOrderItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.fqcOrderItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).defectHandlings
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
    const isCreate = !props.formData?.fqcOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  fqcOrderId: [
    {
      required: true,
      message: pi.ph('fqcOrderId'),
      trigger: 'change'
    }
  ],
  fqcOrderCode: [
    {
      required: true,
      message: pi.ph('fqcOrderCode'),
      trigger: 'blur'
    }
  ],
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
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  warehouseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('warehouseQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('warehouseQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardCode: [
    {
      required: true,
      message: pi.ph('standardCode'),
      trigger: 'change'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: pi.ph('samplingSchemeCode'),
      trigger: 'change'
    }
  ],
  inspectionMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sampleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sampleQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sampleQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('qualifiedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('qualifiedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unqualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('unqualifiedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('unqualifiedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionReturnQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionReturnQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionReturnQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectorBy: [
    {
      required: true,
      message: pi.ph('inspectorBy'),
      trigger: 'change'
    }
  ],
  inspectionDate: [
    {
      required: true,
      message: pi.ph('inspectionDate'),
      trigger: 'change'
    }
  ],
  judgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('judgeStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('judgeStatus'))
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
  await fqcDefectHandlingTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('warehouseQuantity' in payload) {
    const rawwarehouseQuantity = payload.warehouseQuantity
    payload.warehouseQuantity = typeof rawwarehouseQuantity === 'number' ? rawwarehouseQuantity : Number(rawwarehouseQuantity)
  }
  if ('inspectionMethod' in payload) {
    const rawinspectionMethod = payload.inspectionMethod
    payload.inspectionMethod = typeof rawinspectionMethod === 'number' ? rawinspectionMethod : Number(rawinspectionMethod)
  }
  if ('sampleQuantity' in payload) {
    const rawsampleQuantity = payload.sampleQuantity
    payload.sampleQuantity = typeof rawsampleQuantity === 'number' ? rawsampleQuantity : Number(rawsampleQuantity)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('unqualifiedQuantity' in payload) {
    const rawunqualifiedQuantity = payload.unqualifiedQuantity
    payload.unqualifiedQuantity = typeof rawunqualifiedQuantity === 'number' ? rawunqualifiedQuantity : Number(rawunqualifiedQuantity)
  }
  if ('inspectionReturnQuantity' in payload) {
    const rawinspectionReturnQuantity = payload.inspectionReturnQuantity
    payload.inspectionReturnQuantity = typeof rawinspectionReturnQuantity === 'number' ? rawinspectionReturnQuantity : Number(rawinspectionReturnQuantity)
  }
  if ('judgeStatus' in payload) {
    const rawjudgeStatus = payload.judgeStatus
    payload.judgeStatus = typeof rawjudgeStatus === 'number' ? rawjudgeStatus : Number(rawjudgeStatus)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.fqcOrderItemId)
  childFqcDefectHandlingRows.value = []
  fqcDefectHandlingTableRef.value?.resetRows?.()
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

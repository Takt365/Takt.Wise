<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：AssyDefectForm.vue -->
<!-- 功能描述：组立不良日报实体 不良率维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-defect-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-defect-form-tabs"
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
                :label="pi.label('prodCategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_manufacturing_prod_category"
                  :placeholder="pi.ph('prodCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('teamCode')"
                name="teamCode"
              >
                <TaktSelect
                  v-model:value="formState.teamCode"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('teamCode')"
                  :disabled="!!formData?.assyDefectId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_manufacturing_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderType')"
                name="prodOrderType"
              >
                <a-input
                  v-model:value="formState.prodOrderType"
                  :placeholder="pi.ph('prodOrderType')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="pi.ph('prodOrderCode')"
                  show-count
                  :maxlength="12"
                  allow-clear
                  :disabled="!!formData?.assyDefectId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodOrderQty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="pi.ph('prodOrderQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelCode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="pi.ph('modelCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.assyDefectId"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="pi.ph('batchCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.assyDefectId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.assyDefectId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('prodActualQty')"
                name="prodActualQty"
              >
                <a-input-number
                  v-model:value="formState.prodActualQty"
                  :placeholder="pi.ph('prodActualQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('goodQuantity')"
                name="goodQuantity"
              >
                <a-input-number
                  v-model:value="formState.goodQuantity"
                  :placeholder="pi.ph('goodQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
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
            <a-col :span="24">
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
    <!-- 下：子表 assyDefectDetails -->
    <TaktEditableTable
      ref="assyDefectDetailTableRef"
      v-model="childAssyDefectDetailRows"
      :columns="assyDefectDetailFormColumns"
      :title="assyDefectDetailPi.self()"
      :add-button-entity="assyDefectDetailPi.self()"
      id-field="assyDefectDetailId"
      :default-row="createDefaultAssyDefectDetailRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-defectCategory="{ record }">
        <TaktSelect
          v-model:value="record.defectCategory"
          dict-type="logistics_manufacturing_defect_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyDefectDetailPi.ph('defectCategory')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-defectLocation="{ record }">
        <TaktSelect
          v-model:value="record.defectLocation"
          dict-type="logistics_manufacturing_assy_location_category"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="assyDefectDetailPi.ph('defectLocation')"
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
          :placeholder="assyDefectDetailPi.queryPh('repairOperator', 'select')"
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
          :placeholder="assyDefectDetailPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 组立不良日报实体 不良率维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyDefectI18n } from '../composables/useAssyDefectI18n'

/** 实体字段 i18n */
const pi = useAssyDefectI18n()

import type { AssyDefectCreate } from '@/types/logistics/manufacturing/defect/assyDefect.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","prodCategory","prodDate","teamCode","shiftNo","prodOrderType","prodOrderCode","prodOrderQty","modelCode","batchCode","materialCode","prodActualQty","goodQuantity","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useAssyDefectDetailI18n } from '../composables/useAssyDefectDetailI18n'

const assyDefectDetailPi = useAssyDefectDetailI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childAssyDefectDetailRows = ref<Record<string, unknown>[]>([])
const assyDefectDetailTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedAssyDefectDetailRow(row: Record<string, unknown>): boolean {
  const id = row.assyDefectDetailId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextAssyDefectDetailLineNumber(): number {
  const rows = assyDefectDetailTableRef.value?.getRows?.() ?? childAssyDefectDetailRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 assyDefectDetail 可编辑列 */
const assyDefectDetailFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: assyDefectDetailPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'defectCategory',
    title: assyDefectDetailPi.label('defectCategory'),
    width: 140,
  },
  {
    key: 'defectQty',
    title: assyDefectDetailPi.label('defectQty'),
    width: 140,
  },
  {
    key: 'cumulativeDefectQty',
    title: assyDefectDetailPi.label('cumulativeDefectQty'),
    width: 140,
  },
  {
    key: 'randomCardCode',
    title: assyDefectDetailPi.label('randomCardCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('randomCardCode'),
  },
  {
    key: 'occurrenceEngineering',
    title: assyDefectDetailPi.label('occurrenceEngineering'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('occurrenceEngineering'),
  },
  {
    key: 'testStep',
    title: assyDefectDetailPi.label('testStep'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('testStep'),
  },
  {
    key: 'defectSymptom',
    title: assyDefectDetailPi.label('defectSymptom'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('defectSymptom'),
  },
  {
    key: 'defectLocation',
    title: assyDefectDetailPi.label('defectLocation'),
    width: 140,
  },
  {
    key: 'defectReason',
    title: assyDefectDetailPi.label('defectReason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: assyDefectDetailPi.ph('defectReason'),
  },
  {
    key: 'repairOperator',
    title: assyDefectDetailPi.label('repairOperator'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: assyDefectDetailPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<AssyDefectCreate & { assyDefectId?: string }> | null | undefined) {
  const rows_assyDefectDetail = ((val as any)?.assyDefectDetails ?? []) as Record<string, unknown>[]
  childAssyDefectDetailRows.value = rows_assyDefectDetail
}

function createDefaultAssyDefectDetailRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextAssyDefectDetailLineNumber(),
    defectCategory: '',
    defectQty: 0,
    cumulativeDefectQty: 0,
    randomCardCode: '',
    occurrenceEngineering: '',
    testStep: '',
    defectSymptom: '',
    defectLocation: '',
    defectReason: '',
    repairOperator: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.assyDefectId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    assyDefectDetails: assyDefectDetailTableRef.value?.getRows?.() ?? childAssyDefectDetailRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        assyDefectId: masterId,
      }
      if (isUpdate && isPersistedAssyDefectDetailRow(row)) {
        normalized.assyDefectDetailId = row.assyDefectDetailId
      } else {
        delete normalized.assyDefectDetailId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyDefectCreate & { assyDefectId?: string }> | null
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
  prodCategory: "FPP"
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyDefectId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyDefectId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).assyDefectDetails
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
    if (!props.formData?.assyDefectId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodCategory: [
    {
      required: true,
      message: pi.ph('prodCategory'),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change'
    }
  ],
  teamCode: [
    {
      required: true,
      message: pi.ph('teamCode'),
      trigger: 'change'
    }
  ],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodOrderQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  modelCode: [
    {
      required: true,
      message: pi.ph('modelCode'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur'
    }
  ],
  prodActualQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodActualQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  goodQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('goodQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('goodQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await assyDefectDetailTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    if (rawshiftNo === undefined || rawshiftNo === null || rawshiftNo === '') {
      delete payload.shiftNo
    } else {
      const numshiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
      if (Number.isFinite(numshiftNo)) payload.shiftNo = numshiftNo
      else delete payload.shiftNo
    }
  }
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    if (rawprodOrderQty === undefined || rawprodOrderQty === null || rawprodOrderQty === '') {
      delete payload.prodOrderQty
    } else {
      const numprodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
      if (Number.isFinite(numprodOrderQty)) payload.prodOrderQty = numprodOrderQty
      else delete payload.prodOrderQty
    }
  }
  if ('prodActualQty' in payload) {
    const rawprodActualQty = payload.prodActualQty
    if (rawprodActualQty === undefined || rawprodActualQty === null || rawprodActualQty === '') {
      delete payload.prodActualQty
    } else {
      const numprodActualQty = typeof rawprodActualQty === 'number' ? rawprodActualQty : Number(rawprodActualQty)
      if (Number.isFinite(numprodActualQty)) payload.prodActualQty = numprodActualQty
      else delete payload.prodActualQty
    }
  }
  if ('goodQuantity' in payload) {
    const rawgoodQuantity = payload.goodQuantity
    if (rawgoodQuantity === undefined || rawgoodQuantity === null || rawgoodQuantity === '') {
      delete payload.goodQuantity
    } else {
      const numgoodQuantity = typeof rawgoodQuantity === 'number' ? rawgoodQuantity : Number(rawgoodQuantity)
      if (Number.isFinite(numgoodQuantity)) payload.goodQuantity = numgoodQuantity
      else delete payload.goodQuantity
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.assyDefectId) {
    payload.assyDefectId = props.formData.assyDefectId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assyDefectId)
  childAssyDefectDetailRows.value = []
  assyDefectDetailTableRef.value?.resetRows?.()
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

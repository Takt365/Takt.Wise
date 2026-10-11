<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：BillOfMaterialItemForm.vue -->
<!-- 功能描述：Takt物料清单明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form bill-of-material-item-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="bill-of-material-item-form-tabs"
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
                :label="pi.label('billOfMaterialId')"
                name="billOfMaterialId"
              >
                <a-input
                  v-model:value="formState.billOfMaterialId"
                  :placeholder="pi.ph('billOfMaterialId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomCode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="pi.ph('bomCode')"
                  show-count
                  :maxlength="50"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.billOfMaterialItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="pi.ph('materialDescription')"
                  :rows="2"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('usageQuantity')"
                name="usageQuantity"
              >
                <a-input-number
                  v-model:value="formState.usageQuantity"
                  :placeholder="pi.ph('usageQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialUnit')"
                name="materialUnit"
              >
                <TaktSelect
                  v-model:value="formState.materialUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('materialUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapRate')"
                name="scrapRate"
              >
                <a-input-number
                  v-model:value="formState.scrapRate"
                  :placeholder="pi.ph('scrapRate')"
                  style="width: 100%"
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
                :label="pi.label('actualUsageQuantity')"
                name="actualUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.actualUsageQuantity"
                  :placeholder="pi.ph('actualUsageQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('operationSeq')"
                name="operationSeq"
              >
                <a-input-number
                  v-model:value="formState.operationSeq"
                  :placeholder="pi.ph('operationSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('workCenter')"
                name="workCenter"
              >
                <TaktSelect
                  v-model:value="formState.workCenter"
                  api-url="TaktWorkCenters/options"
                  :placeholder="pi.ph('workCenter')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('position')"
                name="position"
              >
                <a-input
                  v-model:value="formState.position"
                  :placeholder="pi.ph('position')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('substituteGroup')"
                name="substituteGroup"
              >
                <a-input
                  v-model:value="formState.substituteGroup"
                  :placeholder="pi.ph('substituteGroup')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('substitutePriority')"
                name="substitutePriority"
              >
                <a-input-number
                  v-model:value="formState.substitutePriority"
                  :placeholder="pi.ph('substitutePriority')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isOptional')"
                name="isOptional"
              >
                <TaktSelect
                  v-model:value="formState.isOptional"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isOptional')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isPhantom')"
                name="isPhantom"
              >
                <TaktSelect
                  v-model:value="formState.isPhantom"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isPhantom')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isObsolete')"
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
    <!-- 下：子表 substitutes -->
    <TaktEditableTable
      ref="billOfMaterialSubstituteTableRef"
      v-model="childBillOfMaterialSubstituteRows"
      :columns="billOfMaterialSubstituteFormColumns"
      :title="billOfMaterialSubstitutePi.self()"
      :add-button-entity="billOfMaterialSubstitutePi.self()"
      id-field="billOfMaterialSubstituteId"
      :default-row="createDefaultBillOfMaterialSubstituteRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-substituteMaterialId="{ record }">
        <TaktSelect
          v-model:value="record.substituteMaterialId"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialSubstitutePi.queryPh('substituteMaterialId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-materialUnit="{ record }">
        <TaktSelect
          v-model:value="record.materialUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialSubstitutePi.ph('materialUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isEnabled="{ record }">
        <TaktSelect
          v-model:value="record.isEnabled"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialSubstitutePi.ph('isEnabled')"
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
          :placeholder="billOfMaterialSubstitutePi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料清单明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBillOfMaterialItemI18n } from '../composables/useBillOfMaterialItemI18n'

/** 实体字段 i18n */
const pi = useBillOfMaterialItemI18n()

import type { BillOfMaterialItemCreate } from '@/types/logistics/manufacturing/bom/billOfMaterialItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","billOfMaterialId","bomCode","lineNumber","materialCode","materialDescription","usageQuantity","materialUnit","scrapRate","actualUsageQuantity","operationSeq","workCenter","position","substituteGroup","substitutePriority","isOptional","isPhantom","isObsolete","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useBillOfMaterialSubstituteI18n } from '../composables/useBillOfMaterialSubstituteI18n'

const billOfMaterialSubstitutePi = useBillOfMaterialSubstituteI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childBillOfMaterialSubstituteRows = ref<Record<string, unknown>[]>([])
const billOfMaterialSubstituteTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedBillOfMaterialSubstituteRow(row: Record<string, unknown>): boolean {
  const id = row.billOfMaterialSubstituteId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextBillOfMaterialSubstituteLineNumber(): number {
  const rows = billOfMaterialSubstituteTableRef.value?.getRows?.() ?? childBillOfMaterialSubstituteRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 billOfMaterialSubstitute 可编辑列 */
const billOfMaterialSubstituteFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: billOfMaterialSubstitutePi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'substituteMaterialId',
    title: billOfMaterialSubstitutePi.label('substituteMaterialId'),
    width: 140,
  },
  {
    key: 'substituteGroup',
    title: billOfMaterialSubstitutePi.label('substituteGroup'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: billOfMaterialSubstitutePi.ph('substituteGroup'),
  },
  {
    key: 'substitutePriority',
    title: billOfMaterialSubstitutePi.label('substitutePriority'),
    width: 140,
  },
  {
    key: 'usageQuantity',
    title: billOfMaterialSubstitutePi.label('usageQuantity'),
    width: 140,
  },
  {
    key: 'materialUnit',
    title: billOfMaterialSubstitutePi.label('materialUnit'),
    width: 140,
  },
  {
    key: 'usageRatio',
    title: billOfMaterialSubstitutePi.label('usageRatio'),
    width: 140,
  },
  {
    key: 'isEnabled',
    title: billOfMaterialSubstitutePi.label('isEnabled'),
    width: 140,
  },
  {
    key: 'effectiveDate',
    title: billOfMaterialSubstitutePi.label('effectiveDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'expiryDate',
    title: billOfMaterialSubstitutePi.label('expiryDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'isObsolete',
    title: billOfMaterialSubstitutePi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null | undefined) {
  const rows_billOfMaterialSubstitute = ((val as any)?.substitutes ?? []) as Record<string, unknown>[]
  childBillOfMaterialSubstituteRows.value = rows_billOfMaterialSubstitute
}

function createDefaultBillOfMaterialSubstituteRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextBillOfMaterialSubstituteLineNumber(),
    substituteMaterialId: '',
    substituteGroup: '',
    substitutePriority: 0,
    usageQuantity: 0,
    materialUnit: '',
    usageRatio: 0,
    isEnabled: 0,
    effectiveDate: '',
    expiryDate: '',
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.billOfMaterialItemId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    substitutes: billOfMaterialSubstituteTableRef.value?.getRows?.() ?? childBillOfMaterialSubstituteRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        billOfMaterialItemId: masterId,
      }
      if (isUpdate && isPersistedBillOfMaterialSubstituteRow(row)) {
        normalized.billOfMaterialSubstituteId = row.billOfMaterialSubstituteId
      } else {
        delete normalized.billOfMaterialSubstituteId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null
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

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 billOfMaterialItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.billOfMaterialItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).substitutes
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
    if (!props.formData?.billOfMaterialItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billOfMaterialId: [
    {
      required: true,
      message: pi.ph('billOfMaterialId'),
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
  usageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('usageQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('usageQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialUnit: [
    {
      required: true,
      message: pi.ph('materialUnit'),
      trigger: 'change'
    }
  ],
  scrapRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scrapRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scrapRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualUsageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualUsageQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualUsageQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operationSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('operationSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('operationSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  substitutePriority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('substitutePriority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('substitutePriority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isOptional: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isOptional'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isOptional'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPhantom: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isPhantom'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isPhantom'))
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
  await billOfMaterialSubstituteTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    if (rawlineNumber === undefined || rawlineNumber === null || rawlineNumber === '') {
      delete payload.lineNumber
    } else {
      const numlineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
      if (Number.isFinite(numlineNumber)) payload.lineNumber = numlineNumber
      else delete payload.lineNumber
    }
  }
  if ('usageQuantity' in payload) {
    const rawusageQuantity = payload.usageQuantity
    if (rawusageQuantity === undefined || rawusageQuantity === null || rawusageQuantity === '') {
      delete payload.usageQuantity
    } else {
      const numusageQuantity = typeof rawusageQuantity === 'number' ? rawusageQuantity : Number(rawusageQuantity)
      if (Number.isFinite(numusageQuantity)) payload.usageQuantity = numusageQuantity
      else delete payload.usageQuantity
    }
  }
  if ('scrapRate' in payload) {
    const rawscrapRate = payload.scrapRate
    if (rawscrapRate === undefined || rawscrapRate === null || rawscrapRate === '') {
      delete payload.scrapRate
    } else {
      const numscrapRate = typeof rawscrapRate === 'number' ? rawscrapRate : Number(rawscrapRate)
      if (Number.isFinite(numscrapRate)) payload.scrapRate = numscrapRate
      else delete payload.scrapRate
    }
  }
  if ('actualUsageQuantity' in payload) {
    const rawactualUsageQuantity = payload.actualUsageQuantity
    if (rawactualUsageQuantity === undefined || rawactualUsageQuantity === null || rawactualUsageQuantity === '') {
      delete payload.actualUsageQuantity
    } else {
      const numactualUsageQuantity = typeof rawactualUsageQuantity === 'number' ? rawactualUsageQuantity : Number(rawactualUsageQuantity)
      if (Number.isFinite(numactualUsageQuantity)) payload.actualUsageQuantity = numactualUsageQuantity
      else delete payload.actualUsageQuantity
    }
  }
  if ('operationSeq' in payload) {
    const rawoperationSeq = payload.operationSeq
    if (rawoperationSeq === undefined || rawoperationSeq === null || rawoperationSeq === '') {
      delete payload.operationSeq
    } else {
      const numoperationSeq = typeof rawoperationSeq === 'number' ? rawoperationSeq : Number(rawoperationSeq)
      if (Number.isFinite(numoperationSeq)) payload.operationSeq = numoperationSeq
      else delete payload.operationSeq
    }
  }
  if ('substitutePriority' in payload) {
    const rawsubstitutePriority = payload.substitutePriority
    if (rawsubstitutePriority === undefined || rawsubstitutePriority === null || rawsubstitutePriority === '') {
      delete payload.substitutePriority
    } else {
      const numsubstitutePriority = typeof rawsubstitutePriority === 'number' ? rawsubstitutePriority : Number(rawsubstitutePriority)
      if (Number.isFinite(numsubstitutePriority)) payload.substitutePriority = numsubstitutePriority
      else delete payload.substitutePriority
    }
  }
  if ('isOptional' in payload) {
    const rawisOptional = payload.isOptional
    if (rawisOptional === undefined || rawisOptional === null || rawisOptional === '') {
      delete payload.isOptional
    } else {
      const numisOptional = typeof rawisOptional === 'number' ? rawisOptional : Number(rawisOptional)
      if (Number.isFinite(numisOptional)) payload.isOptional = numisOptional
      else delete payload.isOptional
    }
  }
  if ('isPhantom' in payload) {
    const rawisPhantom = payload.isPhantom
    if (rawisPhantom === undefined || rawisPhantom === null || rawisPhantom === '') {
      delete payload.isPhantom
    } else {
      const numisPhantom = typeof rawisPhantom === 'number' ? rawisPhantom : Number(rawisPhantom)
      if (Number.isFinite(numisPhantom)) payload.isPhantom = numisPhantom
      else delete payload.isPhantom
    }
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    if (rawisObsolete === undefined || rawisObsolete === null || rawisObsolete === '') {
      delete payload.isObsolete
    } else {
      const numisObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
      if (Number.isFinite(numisObsolete)) payload.isObsolete = numisObsolete
      else delete payload.isObsolete
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.billOfMaterialItemId) {
    payload.billOfMaterialItemId = props.formData.billOfMaterialItemId
    delete payload.numberingRuleCode
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.billOfMaterialItemId)
  childBillOfMaterialSubstituteRows.value = []
  billOfMaterialSubstituteTableRef.value?.resetRows?.()
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

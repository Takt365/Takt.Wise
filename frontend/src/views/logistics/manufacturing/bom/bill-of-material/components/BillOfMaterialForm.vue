<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：BillOfMaterialForm.vue -->
<!-- 功能描述：Takt物料清单实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form bill-of-material-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="bill-of-material-form-tabs"
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
                :label="pi.label('bomCode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="pi.ph('bomCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomName')"
                name="bomName"
              >
                <a-input
                  v-model:value="formState.bomName"
                  :placeholder="pi.ph('bomName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parentMaterialCode')"
                name="parentMaterialCode"
              >
                <TaktSelect
                  v-model:value="formState.parentMaterialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('parentMaterialCode')"
                  :disabled="!!formData?.billOfMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentMaterialDescription')"
                name="parentMaterialDescription"
              >
                <a-textarea
                  v-model:value="formState.parentMaterialDescription"
                  :placeholder="pi.ph('parentMaterialDescription')"
                  :rows="2"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomVersion')"
                name="bomVersion"
              >
                <a-input
                  v-model:value="formState.bomVersion"
                  :placeholder="pi.ph('bomVersion')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomType')"
                name="bomType"
              >
                <TaktSelect
                  v-model:value="formState.bomType"
                  dict-type="logistics_manufacturing_bom_type"
                  :placeholder="pi.ph('bomType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('alternativeBomNumber')"
                name="alternativeBomNumber"
              >
                <a-input
                  v-model:value="formState.alternativeBomNumber"
                  :placeholder="pi.ph('alternativeBomNumber')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('effectiveDate')"
                name="effectiveDate"
              >
                <a-date-picker
                  v-model:value="formState.effectiveDate"
                  :placeholder="pi.ph('effectiveDate')"
                  value-format="YYYY-MM-DD"
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
                :label="pi.label('expiryDate')"
                name="expiryDate"
              >
                <a-date-picker
                  v-model:value="formState.expiryDate"
                  :placeholder="pi.ph('expiryDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentMaterialUnit')"
                name="parentMaterialUnit"
              >
                <TaktSelect
                  v-model:value="formState.parentMaterialUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('parentMaterialUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentMaterialQuantity')"
                name="parentMaterialQuantity"
              >
                <a-input-number
                  v-model:value="formState.parentMaterialQuantity"
                  :placeholder="pi.ph('parentMaterialQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomGroupCode')"
                name="bomGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.bomGroupCode"
                  api-url="TaktBomGroups/options"
                  :api-params="{ bomGroupCategory: 1 }"
                  :placeholder="pi.ph('bomGroupCode')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomGroupName')"
                name="bomGroupName"
              >
                <a-input
                  v-model:value="formState.bomGroupName"
                  :placeholder="pi.ph('bomGroupName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('bomDescription')"
                name="bomDescription"
              >
                <a-textarea
                  v-model:value="formState.bomDescription"
                  :placeholder="pi.ph('bomDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('bomStatus')"
                name="bomStatus"
              >
                <TaktSelect
                  v-model:value="formState.bomStatus"
                  dict-type="logistics_manufacturing_bom_status"
                  :placeholder="pi.ph('bomStatus')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="billOfMaterialItemTableRef"
      v-model="childBillOfMaterialItemRows"
      :columns="billOfMaterialItemFormColumns"
      :title="billOfMaterialItemPi.self()"
      :add-button-entity="billOfMaterialItemPi.self()"
      id-field="billOfMaterialItemId"
      :default-row="createDefaultBillOfMaterialItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterialPlants/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialItemPi.queryPh('materialCode', 'select')"
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
          :placeholder="billOfMaterialItemPi.ph('materialUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-workCenter="{ record }">
        <TaktSelect
          v-model:value="record.workCenter"
          api-url="TaktWorkCenters/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialItemPi.queryPh('workCenter', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isOptional="{ record }">
        <TaktSelect
          v-model:value="record.isOptional"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialItemPi.ph('isOptional')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isPhantom="{ record }">
        <TaktSelect
          v-model:value="record.isPhantom"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="billOfMaterialItemPi.ph('isPhantom')"
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
          :placeholder="billOfMaterialItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料清单实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBillOfMaterialI18n } from '../composables/useBillOfMaterialI18n'

/** 实体字段 i18n */
const pi = useBillOfMaterialI18n()

import type { BillOfMaterialCreate } from '@/types/logistics/manufacturing/bom/billOfMaterial.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","bomCode","bomName","parentMaterialCode","parentMaterialDescription","bomVersion","bomType","alternativeBomNumber","effectiveDate","expiryDate","parentMaterialUnit","parentMaterialQuantity","bomGroupCode","bomGroupName","bomDescription","bomStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useBillOfMaterialItemI18n } from '../composables/useBillOfMaterialItemI18n'

const billOfMaterialItemPi = useBillOfMaterialItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childBillOfMaterialItemRows = ref<Record<string, unknown>[]>([])
const billOfMaterialItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedBillOfMaterialItemRow(row: Record<string, unknown>): boolean {
  const id = row.billOfMaterialItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextBillOfMaterialItemLineNumber(): number {
  const rows = billOfMaterialItemTableRef.value?.getRows?.() ?? childBillOfMaterialItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 billOfMaterialItem 可编辑列 */
const billOfMaterialItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: billOfMaterialItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: billOfMaterialItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'usageQuantity',
    title: billOfMaterialItemPi.label('usageQuantity'),
    width: 140,
  },
  {
    key: 'materialUnit',
    title: billOfMaterialItemPi.label('materialUnit'),
    width: 140,
  },
  {
    key: 'scrapRate',
    title: billOfMaterialItemPi.label('scrapRate'),
    width: 140,
  },
  {
    key: 'actualUsageQuantity',
    title: billOfMaterialItemPi.label('actualUsageQuantity'),
    width: 140,
  },
  {
    key: 'operationSeq',
    title: billOfMaterialItemPi.label('operationSeq'),
    width: 140,
  },
  {
    key: 'workCenter',
    title: billOfMaterialItemPi.label('workCenter'),
    width: 140,
  },
  {
    key: 'position',
    title: billOfMaterialItemPi.label('position'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: billOfMaterialItemPi.ph('position'),
  },
  {
    key: 'substituteGroup',
    title: billOfMaterialItemPi.label('substituteGroup'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: billOfMaterialItemPi.ph('substituteGroup'),
  },
  {
    key: 'substitutePriority',
    title: billOfMaterialItemPi.label('substitutePriority'),
    width: 140,
  },
  {
    key: 'isOptional',
    title: billOfMaterialItemPi.label('isOptional'),
    width: 140,
  },
  {
    key: 'isPhantom',
    title: billOfMaterialItemPi.label('isPhantom'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: billOfMaterialItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'substitutes',
    title: billOfMaterialItemPi.label('substitutes'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: billOfMaterialItemPi.ph('substitutes'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<BillOfMaterialCreate & { billOfMaterialId?: string }> | null | undefined) {
  const rows_billOfMaterialItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childBillOfMaterialItemRows.value = rows_billOfMaterialItem
}

function createDefaultBillOfMaterialItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextBillOfMaterialItemLineNumber(),
    materialCode: '',
    usageQuantity: 0,
    materialUnit: '',
    scrapRate: 0,
    actualUsageQuantity: 0,
    operationSeq: 0,
    workCenter: '',
    position: '',
    substituteGroup: '',
    substitutePriority: 0,
    isOptional: 0,
    isPhantom: 0,
    isObsolete: 0,
    substitutes: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.billOfMaterialId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: billOfMaterialItemTableRef.value?.getRows?.() ?? childBillOfMaterialItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        billOfMaterialId: masterId,
      }
      if (isUpdate && isPersistedBillOfMaterialItemRow(row)) {
        normalized.billOfMaterialItemId = row.billOfMaterialItemId
      } else {
        delete normalized.billOfMaterialItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialCreate & { billOfMaterialId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 billOfMaterialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.billOfMaterialId) {
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
    if (!props.formData?.billOfMaterialId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  bomCode: [
    {
      required: true,
      message: pi.ph('bomCode'),
      trigger: 'blur'
    }
  ],
  bomName: [
    {
      required: true,
      message: pi.ph('bomName'),
      trigger: 'blur'
    }
  ],
  parentMaterialCode: [
    {
      required: true,
      message: pi.ph('parentMaterialCode'),
      trigger: 'change'
    }
  ],
  bomVersion: [
    {
      required: true,
      message: pi.ph('bomVersion'),
      trigger: 'blur'
    }
  ],
  bomType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('bomType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('bomType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  alternativeBomNumber: [
    {
      required: true,
      message: pi.ph('alternativeBomNumber'),
      trigger: 'blur'
    }
  ],
  effectiveDate: [
    {
      required: true,
      message: pi.ph('effectiveDate'),
      trigger: 'change'
    }
  ],
  parentMaterialUnit: [
    {
      required: true,
      message: pi.ph('parentMaterialUnit'),
      trigger: 'change'
    }
  ],
  parentMaterialQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('parentMaterialQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('parentMaterialQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  bomStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('bomStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('bomStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await billOfMaterialItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('bomType' in payload) {
    const rawbomType = payload.bomType
    if (rawbomType === undefined || rawbomType === null || rawbomType === '') {
      delete payload.bomType
    } else {
      const numbomType = typeof rawbomType === 'number' ? rawbomType : Number(rawbomType)
      if (Number.isFinite(numbomType)) payload.bomType = numbomType
      else delete payload.bomType
    }
  }
  if ('parentMaterialQuantity' in payload) {
    const rawparentMaterialQuantity = payload.parentMaterialQuantity
    if (rawparentMaterialQuantity === undefined || rawparentMaterialQuantity === null || rawparentMaterialQuantity === '') {
      delete payload.parentMaterialQuantity
    } else {
      const numparentMaterialQuantity = typeof rawparentMaterialQuantity === 'number' ? rawparentMaterialQuantity : Number(rawparentMaterialQuantity)
      if (Number.isFinite(numparentMaterialQuantity)) payload.parentMaterialQuantity = numparentMaterialQuantity
      else delete payload.parentMaterialQuantity
    }
  }
  if ('bomStatus' in payload) {
    const rawbomStatus = payload.bomStatus
    if (rawbomStatus === undefined || rawbomStatus === null || rawbomStatus === '') {
      delete payload.bomStatus
    } else {
      const numbomStatus = typeof rawbomStatus === 'number' ? rawbomStatus : Number(rawbomStatus)
      if (Number.isFinite(numbomStatus)) payload.bomStatus = numbomStatus
      else delete payload.bomStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.billOfMaterialId) {
    payload.billOfMaterialId = props.formData.billOfMaterialId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.billOfMaterialId)
  childBillOfMaterialItemRows.value = []
  billOfMaterialItemTableRef.value?.resetRows?.()
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

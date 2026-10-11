<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing/components -->
<!-- 文件名称：RoutingForm.vue -->
<!-- 功能描述：工艺路线主表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="routing-form-tabs"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingCode')"
                name="routingCode"
              >
                <a-input
                  v-model:value="formState.routingCode"
                  :placeholder="pi.ph('routingCode')"
                  show-count
                  :maxlength="8"
                  allow-clear
                  :disabled="!!formData?.routingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingName')"
                name="routingName"
              >
                <a-input
                  v-model:value="formState.routingName"
                  :placeholder="pi.ph('routingName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingGroupCode')"
                name="routingGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.routingGroupCode"
                  api-url="TaktBomGroups/options"
                  :api-params="{ bomGroupCategory: 2 }"
                  :placeholder="pi.ph('routingGroupCode')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingGroupName')"
                name="routingGroupName"
              >
                <a-input
                  v-model:value="formState.routingGroupName"
                  :placeholder="pi.ph('routingGroupName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purpose')"
                name="purpose"
              >
                <TaktSelect
                  v-model:value="formState.purpose"
                  dict-type="logistics_manufacturing_routing_purpose"
                  :placeholder="pi.ph('purpose')"
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
                  :disabled="!!formData?.routingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('version')"
                name="version"
              >
                <a-input
                  v-model:value="formState.version"
                  :placeholder="pi.ph('version')"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingStatus')"
                name="routingStatus"
              >
                <TaktSelect
                  v-model:value="formState.routingStatus"
                  dict-type="logistics_manufacturing_routing_status"
                  :placeholder="pi.ph('routingStatus')"
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
                :label="pi.label('routingDescription')"
                name="routingDescription"
              >
                <a-textarea
                  v-model:value="formState.routingDescription"
                  :placeholder="pi.ph('routingDescription')"
                  :rows="2"
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
      ref="routingItemTableRef"
      v-model="childRoutingItemRows"
      :columns="routingItemFormColumns"
      :title="routingItemPi.self()"
      :add-button-entity="routingItemPi.self()"
      id-field="routingItemId"
      :default-row="createDefaultRoutingItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-baseUnit="{ record }">
        <TaktSelect
          v-model:value="record.baseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('baseUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-timeUnit="{ record }">
        <TaktSelect
          v-model:value="record.timeUnit"
          dict-type="logistics_manufacturing_time_unit"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('timeUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-pointsUnit="{ record }">
        <TaktSelect
          v-model:value="record.pointsUnit"
          dict-type="logistics_manufacturing_points_unit"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('pointsUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-pointsToMinutesRate="{ record }">
        <TaktSelect
          v-model:value="record.pointsToMinutesRate"
          dict-type="logistics_manufacturing_points_to_minutes_rate"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('pointsToMinutesRate')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isInspection="{ record }">
        <TaktSelect
          v-model:value="record.isInspection"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('isInspection')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-processSegmentType="{ record }">
        <TaktSelect
          v-model:value="record.processSegmentType"
          dict-type="logistics_manufacturing_process_segment_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemPi.ph('processSegmentType')"
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
          :placeholder="routingItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工艺路线主表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useRoutingI18n } from '../composables/useRoutingI18n'

/** 实体字段 i18n */
const pi = useRoutingI18n()

import type { RoutingCreate } from '@/types/logistics/manufacturing/bom/routing.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","workCenter","routingCode","routingName","routingGroupCode","routingGroupName","purpose","materialCode","version","routingStatus","effectiveDate","expiryDate","routingDescription","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useRoutingItemI18n } from '../composables/useRoutingItemI18n'

const routingItemPi = useRoutingItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childRoutingItemRows = ref<Record<string, unknown>[]>([])
const routingItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedRoutingItemRow(row: Record<string, unknown>): boolean {
  const id = row.routingItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextRoutingItemLineNumber(): number {
  const rows = routingItemTableRef.value?.getRows?.() ?? childRoutingItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 routingItem 可编辑列 */
const routingItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: routingItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'baseUnit',
    title: routingItemPi.label('baseUnit'),
    width: 140,
  },
  {
    key: 'baseQuantity',
    title: routingItemPi.label('baseQuantity'),
    width: 140,
  },
  {
    key: 'standardMinutes',
    title: routingItemPi.label('standardMinutes'),
    width: 140,
  },
  {
    key: 'timeUnit',
    title: routingItemPi.label('timeUnit'),
    width: 140,
  },
  {
    key: 'standardShorts',
    title: routingItemPi.label('standardShorts'),
    width: 140,
  },
  {
    key: 'pointsUnit',
    title: routingItemPi.label('pointsUnit'),
    width: 140,
  },
  {
    key: 'pointsToMinutesRate',
    title: routingItemPi.label('pointsToMinutesRate'),
    width: 140,
  },
  {
    key: 'convertedMinutes',
    title: routingItemPi.label('convertedMinutes'),
    width: 140,
  },
  {
    key: 'setupMinutes',
    title: routingItemPi.label('setupMinutes'),
    width: 140,
  },
  {
    key: 'teardownMinutes',
    title: routingItemPi.label('teardownMinutes'),
    width: 140,
  },
  {
    key: 'isInspection',
    title: routingItemPi.label('isInspection'),
    width: 140,
  },
  {
    key: 'processDescription',
    title: routingItemPi.label('processDescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: routingItemPi.ph('processDescription'),
    width: 180,
  },
  {
    key: 'processSegmentType',
    title: routingItemPi.label('processSegmentType'),
    width: 140,
  },
  {
    key: 'extJson',
    title: routingItemPi.label('extJson'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: routingItemPi.ph('extJson'),
  },
  {
    key: 'isObsolete',
    title: routingItemPi.label('isObsolete'),
    width: 140,
  },
  {
    key: 'arguments',
    title: routingItemPi.label('arguments'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: routingItemPi.ph('arguments'),
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<RoutingCreate & { routingId?: string }> | null | undefined) {
  const rows_routingItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childRoutingItemRows.value = rows_routingItem
}

function createDefaultRoutingItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextRoutingItemLineNumber(),
    baseUnit: '',
    baseQuantity: 0,
    standardMinutes: 0,
    timeUnit: '',
    standardShorts: 0,
    pointsUnit: '',
    pointsToMinutesRate: '',
    convertedMinutes: 0,
    setupMinutes: 0,
    teardownMinutes: 0,
    isInspection: 0,
    processDescription: '',
    processSegmentType: 0,
    extJson: '',
    isObsolete: 0,
    arguments: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.routingId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: routingItemTableRef.value?.getRows?.() ?? childRoutingItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        routingId: masterId,
      }
      if (isUpdate && isPersistedRoutingItemRow(row)) {
        normalized.routingItemId = row.routingItemId
      } else {
        delete normalized.routingItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingCreate & { routingId?: string }> | null
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
  purpose: 1,
  routingStatus: 4
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 routingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.routingId) {
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
    if (!props.formData?.routingId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  workCenter: [
    {
      required: true,
      message: pi.ph('workCenter'),
      trigger: 'change'
    }
  ],
  routingCode: [
    {
      required: true,
      message: pi.ph('routingCode'),
      trigger: 'blur'
    }
  ],
  routingName: [
    {
      required: true,
      message: pi.ph('routingName'),
      trigger: 'blur'
    }
  ],
  purpose: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('purpose'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('purpose'))
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
  version: [
    {
      required: true,
      message: pi.ph('version'),
      trigger: 'blur'
    }
  ],
  routingStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('routingStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('routingStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await routingItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('purpose' in payload) {
    const rawpurpose = payload.purpose
    if (rawpurpose === undefined || rawpurpose === null || rawpurpose === '') {
      delete payload.purpose
    } else {
      const numpurpose = typeof rawpurpose === 'number' ? rawpurpose : Number(rawpurpose)
      if (Number.isFinite(numpurpose)) payload.purpose = numpurpose
      else delete payload.purpose
    }
  }
  if ('routingStatus' in payload) {
    const rawroutingStatus = payload.routingStatus
    if (rawroutingStatus === undefined || rawroutingStatus === null || rawroutingStatus === '') {
      delete payload.routingStatus
    } else {
      const numroutingStatus = typeof rawroutingStatus === 'number' ? rawroutingStatus : Number(rawroutingStatus)
      if (Number.isFinite(numroutingStatus)) payload.routingStatus = numroutingStatus
      else delete payload.routingStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.routingId) {
    payload.routingId = props.formData.routingId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.routingId)
  childRoutingItemRows.value = []
  routingItemTableRef.value?.resetRows?.()
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

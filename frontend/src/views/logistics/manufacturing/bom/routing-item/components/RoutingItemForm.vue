<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing-item/components -->
<!-- 文件名称：RoutingItemForm.vue -->
<!-- 功能描述：工艺路线明细表实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-item-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="routing-item-form-tabs"
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
                :label="pi.label('routingId')"
                name="routingId"
              >
                <a-input
                  v-model:value="formState.routingId"
                  :placeholder="pi.ph('routingId')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :disabled="!!formData?.routingItemId"
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
                :label="pi.label('baseUnit')"
                name="baseUnit"
              >
                <TaktSelect
                  v-model:value="formState.baseUnit"
                  dict-type="logistics_materials_unit_of_measure_code"
                  :placeholder="pi.ph('baseUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('baseQuantity')"
                name="baseQuantity"
              >
                <a-input-number
                  v-model:value="formState.baseQuantity"
                  :placeholder="pi.ph('baseQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('standardMinutes')"
                name="standardMinutes"
              >
                <a-input-number
                  v-model:value="formState.standardMinutes"
                  :placeholder="pi.ph('standardMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('timeUnit')"
                name="timeUnit"
              >
                <TaktSelect
                  v-model:value="formState.timeUnit"
                  dict-type="logistics_manufacturing_time_unit"
                  :placeholder="pi.ph('timeUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('standardShorts')"
                name="standardShorts"
              >
                <a-input-number
                  v-model:value="formState.standardShorts"
                  :placeholder="pi.ph('standardShorts')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pointsUnit')"
                name="pointsUnit"
              >
                <TaktSelect
                  v-model:value="formState.pointsUnit"
                  dict-type="logistics_manufacturing_points_unit"
                  :placeholder="pi.ph('pointsUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pointsToMinutesRate')"
                name="pointsToMinutesRate"
              >
                <TaktSelect
                  v-model:value="formState.pointsToMinutesRate"
                  dict-type="logistics_manufacturing_points_to_minutes_rate"
                  :placeholder="pi.ph('pointsToMinutesRate')"
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
                :label="pi.label('convertedMinutes')"
                name="convertedMinutes"
              >
                <a-input-number
                  v-model:value="formState.convertedMinutes"
                  :placeholder="pi.ph('convertedMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('setupMinutes')"
                name="setupMinutes"
              >
                <a-input-number
                  v-model:value="formState.setupMinutes"
                  :placeholder="pi.ph('setupMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('teardownMinutes')"
                name="teardownMinutes"
              >
                <a-input-number
                  v-model:value="formState.teardownMinutes"
                  :placeholder="pi.ph('teardownMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isInspection')"
                name="isInspection"
              >
                <TaktSelect
                  v-model:value="formState.isInspection"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isInspection')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('processDescription')"
                name="processDescription"
              >
                <a-textarea
                  v-model:value="formState.processDescription"
                  :placeholder="pi.ph('processDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('processSegmentType')"
                name="processSegmentType"
              >
                <TaktSelect
                  v-model:value="formState.processSegmentType"
                  dict-type="logistics_manufacturing_process_segment_type"
                  :placeholder="pi.ph('processSegmentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('extJson')"
                name="extJson"
              >
                <a-input
                  v-model:value="formState.extJson"
                  :placeholder="pi.ph('extJson')"
                  show-count
                  :maxlength="4000"
                  allow-clear
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
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="pi.ph('cultureCode')"
                  show-count
                  :maxlength="20"
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
    <!-- 下：子表 arguments -->
    <TaktEditableTable
      ref="routingItemArgumentTableRef"
      v-model="childRoutingItemArgumentRows"
      :columns="routingItemArgumentFormColumns"
      :title="routingItemArgumentPi.self()"
      :add-button-entity="routingItemArgumentPi.self()"
      id-field="routingItemArgumentId"
      :default-row="createDefaultRoutingItemArgumentRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-paramUnit="{ record }">
        <TaktSelect
          v-model:value="record.paramUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="routingItemArgumentPi.ph('paramUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工艺路线明细表实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/routing-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useRoutingItemI18n } from '../composables/useRoutingItemI18n'

/** 实体字段 i18n */
const pi = useRoutingItemI18n()

import type { RoutingItemCreate } from '@/types/logistics/manufacturing/bom/routingItem.types'
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
 * 上下文隔离字段：租户 / 公司 / CultureCode（登录或公司切换注入，表单只读）
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
const formFields = ["tenantCode","companyCode","cultureCode","routingId","routingCode","lineNumber","baseUnit","baseQuantity","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate","convertedMinutes","setupMinutes","teardownMinutes","isInspection","processDescription","processSegmentType","extJson","isObsolete","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useRoutingItemArgumentI18n } from '../composables/useRoutingItemArgumentI18n'

const routingItemArgumentPi = useRoutingItemArgumentI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childRoutingItemArgumentRows = ref<Record<string, unknown>[]>([])
const routingItemArgumentTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 routingItemArgument 可编辑列 */
const routingItemArgumentFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'paramCode',
    title: routingItemArgumentPi.label('paramCode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'paramName',
    title: routingItemArgumentPi.label('paramName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'paramUnit',
    title: routingItemArgumentPi.label('paramUnit'),
    width: 140,
  },
  {
    key: 'standardValue',
    title: routingItemArgumentPi.label('standardValue'),
    width: 140,
  },
  {
    key: 'lowerLimit',
    title: routingItemArgumentPi.label('lowerLimit'),
    width: 140,
  },
  {
    key: 'upperLimit',
    title: routingItemArgumentPi.label('upperLimit'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<RoutingItemCreate & { routingItemId?: string }> | null | undefined) {
  const rows_routingItemArgument = ((val as any)?.arguments ?? []) as Record<string, unknown>[]
  childRoutingItemArgumentRows.value = rows_routingItemArgument
}

function createDefaultRoutingItemArgumentRow(): Record<string, unknown> {
  return {
    paramCode: '',
    paramName: '',
    paramUnit: '',
    standardValue: 0,
    lowerLimit: 0,
    upperLimit: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.routingItemId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    arguments: routingItemArgumentTableRef.value?.getRows?.() ?? childRoutingItemArgumentRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      routingItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingItemCreate & { routingItemId?: string }> | null
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
  timeUnit: "MIN",
  pointsUnit: "SHORT",
  pointsToMinutesRate: "1",
  processSegmentType: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 routingItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.routingItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).arguments
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
    const isCreate = !props.formData?.routingItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  routingId: [
    {
      required: true,
      message: pi.ph('routingId'),
      trigger: 'blur'
    }
  ],
  routingCode: [
    {
      required: true,
      message: pi.ph('routingCode'),
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
  baseUnit: [
    {
      required: true,
      message: pi.ph('baseUnit'),
      trigger: 'change'
    }
  ],
  baseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('baseQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('baseQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('standardMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('standardMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  timeUnit: [
    {
      required: true,
      message: pi.ph('timeUnit'),
      trigger: 'change'
    }
  ],
  standardShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('standardShorts'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('standardShorts'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pointsUnit: [
    {
      required: true,
      message: pi.ph('pointsUnit'),
      trigger: 'change'
    }
  ],
  pointsToMinutesRate: [
    {
      required: true,
      message: pi.ph('pointsToMinutesRate'),
      trigger: 'change'
    }
  ],
  convertedMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('convertedMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('convertedMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  setupMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('setupMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('setupMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  teardownMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('teardownMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('teardownMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isInspection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isInspection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isInspection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  processSegmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processSegmentType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processSegmentType'))
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
  await routingItemArgumentTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('baseQuantity' in payload) {
    const rawbaseQuantity = payload.baseQuantity
    payload.baseQuantity = typeof rawbaseQuantity === 'number' ? rawbaseQuantity : Number(rawbaseQuantity)
  }
  if ('standardMinutes' in payload) {
    const rawstandardMinutes = payload.standardMinutes
    payload.standardMinutes = typeof rawstandardMinutes === 'number' ? rawstandardMinutes : Number(rawstandardMinutes)
  }
  if ('standardShorts' in payload) {
    const rawstandardShorts = payload.standardShorts
    payload.standardShorts = typeof rawstandardShorts === 'number' ? rawstandardShorts : Number(rawstandardShorts)
  }
  if ('convertedMinutes' in payload) {
    const rawconvertedMinutes = payload.convertedMinutes
    payload.convertedMinutes = typeof rawconvertedMinutes === 'number' ? rawconvertedMinutes : Number(rawconvertedMinutes)
  }
  if ('setupMinutes' in payload) {
    const rawsetupMinutes = payload.setupMinutes
    payload.setupMinutes = typeof rawsetupMinutes === 'number' ? rawsetupMinutes : Number(rawsetupMinutes)
  }
  if ('teardownMinutes' in payload) {
    const rawteardownMinutes = payload.teardownMinutes
    payload.teardownMinutes = typeof rawteardownMinutes === 'number' ? rawteardownMinutes : Number(rawteardownMinutes)
  }
  if ('isInspection' in payload) {
    const rawisInspection = payload.isInspection
    payload.isInspection = typeof rawisInspection === 'number' ? rawisInspection : Number(rawisInspection)
  }
  if ('processSegmentType' in payload) {
    const rawprocessSegmentType = payload.processSegmentType
    payload.processSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.routingItemId)
  childRoutingItemArgumentRows.value = []
  routingItemArgumentTableRef.value?.resetRows?.()
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

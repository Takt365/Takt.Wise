<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/routing/components -->
<!-- 文件名称：RoutingItemForm.vue -->
<!-- 功能描述：工艺路线主表实体子表 routingItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form routing-item-form flex flex-col min-h-0"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('extJson')"
                name="extJson"
              >
                <a-input
                  v-model:value="formState.extJson"
                  :placeholder="pi.ph('extJson')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('arguments')"
                name="arguments"
              >
                <a-input
                  v-model:value="formState.arguments"
                  :placeholder="pi.ph('arguments')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
            <a-col :span="12">
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
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工艺路线主表实体子表 routingItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/routing/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useRoutingItemI18n } from '../composables/useRoutingItemI18n'

/** 实体字段 i18n */
const pi = useRoutingItemI18n()

import type { RoutingItemCreate } from '@/types/logistics/manufacturing/bom/routingItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","baseUnit","baseQuantity","standardMinutes","timeUnit","standardShorts","pointsUnit","pointsToMinutesRate","convertedMinutes","setupMinutes","teardownMinutes","isInspection","processDescription","processSegmentType","extJson","isObsolete","arguments"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<RoutingItemCreate & { routingItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
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

      applyScopeDefaults(next)
      Object.assign(formState, next)
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
    if (!props.formData?.routingItemId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
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
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 routingId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
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
  if ('baseQuantity' in payload) {
    const rawbaseQuantity = payload.baseQuantity
    if (rawbaseQuantity === undefined || rawbaseQuantity === null || rawbaseQuantity === '') {
      delete payload.baseQuantity
    } else {
      const numbaseQuantity = typeof rawbaseQuantity === 'number' ? rawbaseQuantity : Number(rawbaseQuantity)
      if (Number.isFinite(numbaseQuantity)) payload.baseQuantity = numbaseQuantity
      else delete payload.baseQuantity
    }
  }
  if ('standardMinutes' in payload) {
    const rawstandardMinutes = payload.standardMinutes
    if (rawstandardMinutes === undefined || rawstandardMinutes === null || rawstandardMinutes === '') {
      delete payload.standardMinutes
    } else {
      const numstandardMinutes = typeof rawstandardMinutes === 'number' ? rawstandardMinutes : Number(rawstandardMinutes)
      if (Number.isFinite(numstandardMinutes)) payload.standardMinutes = numstandardMinutes
      else delete payload.standardMinutes
    }
  }
  if ('standardShorts' in payload) {
    const rawstandardShorts = payload.standardShorts
    if (rawstandardShorts === undefined || rawstandardShorts === null || rawstandardShorts === '') {
      delete payload.standardShorts
    } else {
      const numstandardShorts = typeof rawstandardShorts === 'number' ? rawstandardShorts : Number(rawstandardShorts)
      if (Number.isFinite(numstandardShorts)) payload.standardShorts = numstandardShorts
      else delete payload.standardShorts
    }
  }
  if ('convertedMinutes' in payload) {
    const rawconvertedMinutes = payload.convertedMinutes
    if (rawconvertedMinutes === undefined || rawconvertedMinutes === null || rawconvertedMinutes === '') {
      delete payload.convertedMinutes
    } else {
      const numconvertedMinutes = typeof rawconvertedMinutes === 'number' ? rawconvertedMinutes : Number(rawconvertedMinutes)
      if (Number.isFinite(numconvertedMinutes)) payload.convertedMinutes = numconvertedMinutes
      else delete payload.convertedMinutes
    }
  }
  if ('setupMinutes' in payload) {
    const rawsetupMinutes = payload.setupMinutes
    if (rawsetupMinutes === undefined || rawsetupMinutes === null || rawsetupMinutes === '') {
      delete payload.setupMinutes
    } else {
      const numsetupMinutes = typeof rawsetupMinutes === 'number' ? rawsetupMinutes : Number(rawsetupMinutes)
      if (Number.isFinite(numsetupMinutes)) payload.setupMinutes = numsetupMinutes
      else delete payload.setupMinutes
    }
  }
  if ('teardownMinutes' in payload) {
    const rawteardownMinutes = payload.teardownMinutes
    if (rawteardownMinutes === undefined || rawteardownMinutes === null || rawteardownMinutes === '') {
      delete payload.teardownMinutes
    } else {
      const numteardownMinutes = typeof rawteardownMinutes === 'number' ? rawteardownMinutes : Number(rawteardownMinutes)
      if (Number.isFinite(numteardownMinutes)) payload.teardownMinutes = numteardownMinutes
      else delete payload.teardownMinutes
    }
  }
  if ('isInspection' in payload) {
    const rawisInspection = payload.isInspection
    if (rawisInspection === undefined || rawisInspection === null || rawisInspection === '') {
      delete payload.isInspection
    } else {
      const numisInspection = typeof rawisInspection === 'number' ? rawisInspection : Number(rawisInspection)
      if (Number.isFinite(numisInspection)) payload.isInspection = numisInspection
      else delete payload.isInspection
    }
  }
  if ('processSegmentType' in payload) {
    const rawprocessSegmentType = payload.processSegmentType
    if (rawprocessSegmentType === undefined || rawprocessSegmentType === null || rawprocessSegmentType === '') {
      delete payload.processSegmentType
    } else {
      const numprocessSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
      if (Number.isFinite(numprocessSegmentType)) payload.processSegmentType = numprocessSegmentType
      else delete payload.processSegmentType
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
  if (props.formData?.routingItemId) {
    payload.routingItemId = props.formData.routingItemId
  }
  payload.routingId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.routingCode ?? masterRow.RoutingCode
    const masterName = masterRow.routingName ?? masterRow.RoutingName
    if (masterCode != null && masterCode !== '' && !payload.routingCode) {
      payload.routingCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.routingName) {
      payload.routingName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.routingItemId)
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

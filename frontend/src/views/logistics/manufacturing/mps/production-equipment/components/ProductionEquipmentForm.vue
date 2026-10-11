<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/production-equipment/components -->
<!-- 文件名称：ProductionEquipmentForm.vue -->
<!-- 功能描述：生产设备主数据维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="production-equipment-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/8)'"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 生产设备主数据维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/production-equipment/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useProductionEquipmentI18n } from '../composables/useProductionEquipmentI18n'

/** 实体字段 i18n */
const pi = useProductionEquipmentI18n()
import type { ProductionEquipmentCreate } from '@/types/logistics/manufacturing/mps/productionEquipment.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
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
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProductionEquipmentCreate & { productionEquipmentId?: string }> | null
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
  prodEquipStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 productionEquipmentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.productionEquipmentId) {
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.productionEquipmentId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  equipCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('equipCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('equipCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  prodEquipCode: [
    {
      required: true,
      message: pi.ph('prodEquipCode'),
      trigger: 'blur'
    }
  ],
  prodEquipName: [
    {
      required: true,
      message: pi.ph('prodEquipName'),
      trigger: 'blur'
    }
  ],
  machineType: [
    {
      required: true,
      message: pi.ph('machineType'),
      trigger: 'blur'
    }
  ],
  stdCycleTimeSeconds: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdCycleTimeSeconds'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdCycleTimeSeconds'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdMinutesPerUnit: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdMinutesPerUnit'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdMinutesPerUnit'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdMinutesPerCycle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdMinutesPerCycle'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdMinutesPerCycle'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  theoreticalSpm: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('theoreticalSpm'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('theoreticalSpm'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  theoreticalCycleTimeSeconds: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('theoreticalCycleTimeSeconds'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('theoreticalCycleTimeSeconds'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdEquipHourlyCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdEquipHourlyCapacity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdEquipHourlyCapacity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  availabilityRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('availabilityRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('availabilityRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  performanceRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('performanceRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('performanceRate'))
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
  moldChangeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('moldChangeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('moldChangeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialChangeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('materialChangeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('materialChangeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mtbfHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mtbfHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mtbfHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mttrHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mttrHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mttrHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cavityCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('cavityCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('cavityCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  quickMoldChange: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('quickMoldChange'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('quickMoldChange'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operatorCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('operatorCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('operatorCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCriticalResource: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCriticalResource'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCriticalResource'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  parallelCapacity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('parallelCapacity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  allowRushOrder: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('allowRushOrder'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('allowRushOrder'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  warmupMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('warmupMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('warmupMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  equipmentRunStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('equipmentRunStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('equipmentRunStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maintenanceIntervalHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('maintenanceIntervalHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('maintenanceIntervalHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  cumulativeRunHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('cumulativeRunHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('cumulativeRunHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  storageLocation: [
    {
      required: true,
      message: pi.ph('storageLocation'),
      trigger: 'blur'
    }
  ],
  prodEquipStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('prodEquipStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('prodEquipStatus'))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('equipCategory' in payload) {
    const rawequipCategory = payload.equipCategory
    payload.equipCategory = typeof rawequipCategory === 'number' ? rawequipCategory : Number(rawequipCategory)
  }
  if ('stdCycleTimeSeconds' in payload) {
    const rawstdCycleTimeSeconds = payload.stdCycleTimeSeconds
    payload.stdCycleTimeSeconds = typeof rawstdCycleTimeSeconds === 'number' ? rawstdCycleTimeSeconds : Number(rawstdCycleTimeSeconds)
  }
  if ('stdMinutesPerUnit' in payload) {
    const rawstdMinutesPerUnit = payload.stdMinutesPerUnit
    payload.stdMinutesPerUnit = typeof rawstdMinutesPerUnit === 'number' ? rawstdMinutesPerUnit : Number(rawstdMinutesPerUnit)
  }
  if ('stdMinutesPerCycle' in payload) {
    const rawstdMinutesPerCycle = payload.stdMinutesPerCycle
    payload.stdMinutesPerCycle = typeof rawstdMinutesPerCycle === 'number' ? rawstdMinutesPerCycle : Number(rawstdMinutesPerCycle)
  }
  if ('theoreticalSpm' in payload) {
    const rawtheoreticalSpm = payload.theoreticalSpm
    payload.theoreticalSpm = typeof rawtheoreticalSpm === 'number' ? rawtheoreticalSpm : Number(rawtheoreticalSpm)
  }
  if ('theoreticalCycleTimeSeconds' in payload) {
    const rawtheoreticalCycleTimeSeconds = payload.theoreticalCycleTimeSeconds
    payload.theoreticalCycleTimeSeconds = typeof rawtheoreticalCycleTimeSeconds === 'number' ? rawtheoreticalCycleTimeSeconds : Number(rawtheoreticalCycleTimeSeconds)
  }
  if ('stdEquipHourlyCapacity' in payload) {
    const rawstdEquipHourlyCapacity = payload.stdEquipHourlyCapacity
    payload.stdEquipHourlyCapacity = typeof rawstdEquipHourlyCapacity === 'number' ? rawstdEquipHourlyCapacity : Number(rawstdEquipHourlyCapacity)
  }
  if ('availabilityRate' in payload) {
    const rawavailabilityRate = payload.availabilityRate
    payload.availabilityRate = typeof rawavailabilityRate === 'number' ? rawavailabilityRate : Number(rawavailabilityRate)
  }
  if ('performanceRate' in payload) {
    const rawperformanceRate = payload.performanceRate
    payload.performanceRate = typeof rawperformanceRate === 'number' ? rawperformanceRate : Number(rawperformanceRate)
  }
  if ('setupMinutes' in payload) {
    const rawsetupMinutes = payload.setupMinutes
    payload.setupMinutes = typeof rawsetupMinutes === 'number' ? rawsetupMinutes : Number(rawsetupMinutes)
  }
  if ('moldChangeMinutes' in payload) {
    const rawmoldChangeMinutes = payload.moldChangeMinutes
    payload.moldChangeMinutes = typeof rawmoldChangeMinutes === 'number' ? rawmoldChangeMinutes : Number(rawmoldChangeMinutes)
  }
  if ('materialChangeMinutes' in payload) {
    const rawmaterialChangeMinutes = payload.materialChangeMinutes
    payload.materialChangeMinutes = typeof rawmaterialChangeMinutes === 'number' ? rawmaterialChangeMinutes : Number(rawmaterialChangeMinutes)
  }
  if ('mtbfHours' in payload) {
    const rawmtbfHours = payload.mtbfHours
    payload.mtbfHours = typeof rawmtbfHours === 'number' ? rawmtbfHours : Number(rawmtbfHours)
  }
  if ('mttrHours' in payload) {
    const rawmttrHours = payload.mttrHours
    payload.mttrHours = typeof rawmttrHours === 'number' ? rawmttrHours : Number(rawmttrHours)
  }
  if ('repeatabilityAccuracy' in payload) {
    const rawrepeatabilityAccuracy = payload.repeatabilityAccuracy
    payload.repeatabilityAccuracy = typeof rawrepeatabilityAccuracy === 'number' ? rawrepeatabilityAccuracy : Number(rawrepeatabilityAccuracy)
  }
  if ('shutHeightAccuracy' in payload) {
    const rawshutHeightAccuracy = payload.shutHeightAccuracy
    payload.shutHeightAccuracy = typeof rawshutHeightAccuracy === 'number' ? rawshutHeightAccuracy : Number(rawshutHeightAccuracy)
  }
  if ('injectionAccuracy' in payload) {
    const rawinjectionAccuracy = payload.injectionAccuracy
    payload.injectionAccuracy = typeof rawinjectionAccuracy === 'number' ? rawinjectionAccuracy : Number(rawinjectionAccuracy)
  }
  if ('temperatureControlAccuracy' in payload) {
    const rawtemperatureControlAccuracy = payload.temperatureControlAccuracy
    payload.temperatureControlAccuracy = typeof rawtemperatureControlAccuracy === 'number' ? rawtemperatureControlAccuracy : Number(rawtemperatureControlAccuracy)
  }
  if ('pressureControlAccuracy' in payload) {
    const rawpressureControlAccuracy = payload.pressureControlAccuracy
    payload.pressureControlAccuracy = typeof rawpressureControlAccuracy === 'number' ? rawpressureControlAccuracy : Number(rawpressureControlAccuracy)
  }
  if ('processCapabilityCpk' in payload) {
    const rawprocessCapabilityCpk = payload.processCapabilityCpk
    payload.processCapabilityCpk = typeof rawprocessCapabilityCpk === 'number' ? rawprocessCapabilityCpk : Number(rawprocessCapabilityCpk)
  }
  if ('maxDimensionalTolerance' in payload) {
    const rawmaxDimensionalTolerance = payload.maxDimensionalTolerance
    payload.maxDimensionalTolerance = typeof rawmaxDimensionalTolerance === 'number' ? rawmaxDimensionalTolerance : Number(rawmaxDimensionalTolerance)
  }
  if ('maxMoldWeightTon' in payload) {
    const rawmaxMoldWeightTon = payload.maxMoldWeightTon
    payload.maxMoldWeightTon = typeof rawmaxMoldWeightTon === 'number' ? rawmaxMoldWeightTon : Number(rawmaxMoldWeightTon)
  }
  if ('ejectionType' in payload) {
    const rawejectionType = payload.ejectionType
    payload.ejectionType = typeof rawejectionType === 'number' ? rawejectionType : Number(rawejectionType)
  }
  if ('ejectionStrokeMm' in payload) {
    const rawejectionStrokeMm = payload.ejectionStrokeMm
    payload.ejectionStrokeMm = typeof rawejectionStrokeMm === 'number' ? rawejectionStrokeMm : Number(rawejectionStrokeMm)
  }
  if ('cavityCount' in payload) {
    const rawcavityCount = payload.cavityCount
    payload.cavityCount = typeof rawcavityCount === 'number' ? rawcavityCount : Number(rawcavityCount)
  }
  if ('quickMoldChange' in payload) {
    const rawquickMoldChange = payload.quickMoldChange
    payload.quickMoldChange = typeof rawquickMoldChange === 'number' ? rawquickMoldChange : Number(rawquickMoldChange)
  }
  if ('ratedTonnage' in payload) {
    const rawratedTonnage = payload.ratedTonnage
    payload.ratedTonnage = typeof rawratedTonnage === 'number' ? rawratedTonnage : Number(rawratedTonnage)
  }
  if ('clampingForceKn' in payload) {
    const rawclampingForceKn = payload.clampingForceKn
    payload.clampingForceKn = typeof rawclampingForceKn === 'number' ? rawclampingForceKn : Number(rawclampingForceKn)
  }
  if ('maxStrokeMm' in payload) {
    const rawmaxStrokeMm = payload.maxStrokeMm
    payload.maxStrokeMm = typeof rawmaxStrokeMm === 'number' ? rawmaxStrokeMm : Number(rawmaxStrokeMm)
  }
  if ('openStrokeMm' in payload) {
    const rawopenStrokeMm = payload.openStrokeMm
    payload.openStrokeMm = typeof rawopenStrokeMm === 'number' ? rawopenStrokeMm : Number(rawopenStrokeMm)
  }
  if ('ratedVoltage' in payload) {
    const rawratedVoltage = payload.ratedVoltage
    payload.ratedVoltage = typeof rawratedVoltage === 'number' ? rawratedVoltage : Number(rawratedVoltage)
  }
  if ('ratedPowerKw' in payload) {
    const rawratedPowerKw = payload.ratedPowerKw
    payload.ratedPowerKw = typeof rawratedPowerKw === 'number' ? rawratedPowerKw : Number(rawratedPowerKw)
  }
  if ('airConsumptionLpm' in payload) {
    const rawairConsumptionLpm = payload.airConsumptionLpm
    payload.airConsumptionLpm = typeof rawairConsumptionLpm === 'number' ? rawairConsumptionLpm : Number(rawairConsumptionLpm)
  }
  if ('coolingWaterFlowLpm' in payload) {
    const rawcoolingWaterFlowLpm = payload.coolingWaterFlowLpm
    payload.coolingWaterFlowLpm = typeof rawcoolingWaterFlowLpm === 'number' ? rawcoolingWaterFlowLpm : Number(rawcoolingWaterFlowLpm)
  }
  if ('operatorCount' in payload) {
    const rawoperatorCount = payload.operatorCount
    payload.operatorCount = typeof rawoperatorCount === 'number' ? rawoperatorCount : Number(rawoperatorCount)
  }
  if ('isCriticalResource' in payload) {
    const rawisCriticalResource = payload.isCriticalResource
    payload.isCriticalResource = typeof rawisCriticalResource === 'number' ? rawisCriticalResource : Number(rawisCriticalResource)
  }
  if ('parallelCapacity' in payload) {
    const rawparallelCapacity = payload.parallelCapacity
    payload.parallelCapacity = typeof rawparallelCapacity === 'number' ? rawparallelCapacity : Number(rawparallelCapacity)
  }
  if ('allowRushOrder' in payload) {
    const rawallowRushOrder = payload.allowRushOrder
    payload.allowRushOrder = typeof rawallowRushOrder === 'number' ? rawallowRushOrder : Number(rawallowRushOrder)
  }
  if ('warmupMinutes' in payload) {
    const rawwarmupMinutes = payload.warmupMinutes
    payload.warmupMinutes = typeof rawwarmupMinutes === 'number' ? rawwarmupMinutes : Number(rawwarmupMinutes)
  }
  if ('noiseLevelDb' in payload) {
    const rawnoiseLevelDb = payload.noiseLevelDb
    payload.noiseLevelDb = typeof rawnoiseLevelDb === 'number' ? rawnoiseLevelDb : Number(rawnoiseLevelDb)
  }
  if ('equipmentRunStatus' in payload) {
    const rawequipmentRunStatus = payload.equipmentRunStatus
    payload.equipmentRunStatus = typeof rawequipmentRunStatus === 'number' ? rawequipmentRunStatus : Number(rawequipmentRunStatus)
  }
  if ('maintenanceIntervalHours' in payload) {
    const rawmaintenanceIntervalHours = payload.maintenanceIntervalHours
    payload.maintenanceIntervalHours = typeof rawmaintenanceIntervalHours === 'number' ? rawmaintenanceIntervalHours : Number(rawmaintenanceIntervalHours)
  }
  if ('cumulativeRunHours' in payload) {
    const rawcumulativeRunHours = payload.cumulativeRunHours
    payload.cumulativeRunHours = typeof rawcumulativeRunHours === 'number' ? rawcumulativeRunHours : Number(rawcumulativeRunHours)
  }
  if ('prodEquipStatus' in payload) {
    const rawprodEquipStatus = payload.prodEquipStatus
    payload.prodEquipStatus = typeof rawprodEquipStatus === 'number' ? rawprodEquipStatus : Number(rawprodEquipStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.productionEquipmentId)

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

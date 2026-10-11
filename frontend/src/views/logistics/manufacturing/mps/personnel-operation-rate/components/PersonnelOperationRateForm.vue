<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mps/personnel-operation-rate/components -->
<!-- 文件名称：PersonnelOperationRateForm.vue -->
<!-- 功能描述：人员稼动率实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="personnel-operation-rate-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
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
 * 人员稼动率实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mps/personnel-operation-rate/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePersonnelOperationRateI18n } from '../composables/usePersonnelOperationRateI18n'

/** 实体字段 i18n */
const pi = usePersonnelOperationRateI18n()
import type { PersonnelOperationRateCreate } from '@/types/logistics/manufacturing/mps/personnelOperationRate.types'
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
  formData?: Partial<PersonnelOperationRateCreate & { personnelOperationRateId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 personnelOperationRateId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.personnelOperationRateId) {
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
    if (!props.formData?.personnelOperationRateId) {
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
  timeCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('timeCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('timeCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startDate: [
    {
      required: true,
      message: pi.ph('startDate'),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: pi.ph('endDate'),
      trigger: 'change'
    }
  ],
  TeamCode: [
    {
      required: true,
      message: pi.ph('TeamCode'),
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
  plannedDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedDirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedDirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualDirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualDirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualDirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedIndirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedIndirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualIndirectPersonnelCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualIndirectPersonnelCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualIndirectPersonnelCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedWorkTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedWorkTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualWorkTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualWorkTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualWorkTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  breakTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('breakTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('breakTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  idleTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('idleTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('idleTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  personnelOperationRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('personnelOperationRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('personnelOperationRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedOutput'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedOutput'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualOutput: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualOutput'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualOutput'))
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
  defectiveQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectiveQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectiveQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  yieldRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('yieldRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('yieldRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workEfficiency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('workEfficiency'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('workEfficiency'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimeHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overtimeHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overtimeHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  rateStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('rateStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('rateStatus'))
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
  if ('timeCategory' in payload) {
    const rawtimeCategory = payload.timeCategory
    payload.timeCategory = typeof rawtimeCategory === 'number' ? rawtimeCategory : Number(rawtimeCategory)
  }
  if ('weekNumber' in payload) {
    const rawweekNumber = payload.weekNumber
    payload.weekNumber = typeof rawweekNumber === 'number' ? rawweekNumber : Number(rawweekNumber)
  }
  if ('monthNumber' in payload) {
    const rawmonthNumber = payload.monthNumber
    payload.monthNumber = typeof rawmonthNumber === 'number' ? rawmonthNumber : Number(rawmonthNumber)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('plannedDirectPersonnelCount' in payload) {
    const rawplannedDirectPersonnelCount = payload.plannedDirectPersonnelCount
    payload.plannedDirectPersonnelCount = typeof rawplannedDirectPersonnelCount === 'number' ? rawplannedDirectPersonnelCount : Number(rawplannedDirectPersonnelCount)
  }
  if ('actualDirectPersonnelCount' in payload) {
    const rawactualDirectPersonnelCount = payload.actualDirectPersonnelCount
    payload.actualDirectPersonnelCount = typeof rawactualDirectPersonnelCount === 'number' ? rawactualDirectPersonnelCount : Number(rawactualDirectPersonnelCount)
  }
  if ('plannedIndirectPersonnelCount' in payload) {
    const rawplannedIndirectPersonnelCount = payload.plannedIndirectPersonnelCount
    payload.plannedIndirectPersonnelCount = typeof rawplannedIndirectPersonnelCount === 'number' ? rawplannedIndirectPersonnelCount : Number(rawplannedIndirectPersonnelCount)
  }
  if ('actualIndirectPersonnelCount' in payload) {
    const rawactualIndirectPersonnelCount = payload.actualIndirectPersonnelCount
    payload.actualIndirectPersonnelCount = typeof rawactualIndirectPersonnelCount === 'number' ? rawactualIndirectPersonnelCount : Number(rawactualIndirectPersonnelCount)
  }
  if ('plannedWorkTime' in payload) {
    const rawplannedWorkTime = payload.plannedWorkTime
    payload.plannedWorkTime = typeof rawplannedWorkTime === 'number' ? rawplannedWorkTime : Number(rawplannedWorkTime)
  }
  if ('actualWorkTime' in payload) {
    const rawactualWorkTime = payload.actualWorkTime
    payload.actualWorkTime = typeof rawactualWorkTime === 'number' ? rawactualWorkTime : Number(rawactualWorkTime)
  }
  if ('breakTime' in payload) {
    const rawbreakTime = payload.breakTime
    payload.breakTime = typeof rawbreakTime === 'number' ? rawbreakTime : Number(rawbreakTime)
  }
  if ('idleTime' in payload) {
    const rawidleTime = payload.idleTime
    payload.idleTime = typeof rawidleTime === 'number' ? rawidleTime : Number(rawidleTime)
  }
  if ('personnelOperationRate' in payload) {
    const rawpersonnelOperationRate = payload.personnelOperationRate
    payload.personnelOperationRate = typeof rawpersonnelOperationRate === 'number' ? rawpersonnelOperationRate : Number(rawpersonnelOperationRate)
  }
  if ('plannedOutput' in payload) {
    const rawplannedOutput = payload.plannedOutput
    payload.plannedOutput = typeof rawplannedOutput === 'number' ? rawplannedOutput : Number(rawplannedOutput)
  }
  if ('actualOutput' in payload) {
    const rawactualOutput = payload.actualOutput
    payload.actualOutput = typeof rawactualOutput === 'number' ? rawactualOutput : Number(rawactualOutput)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('defectiveQuantity' in payload) {
    const rawdefectiveQuantity = payload.defectiveQuantity
    payload.defectiveQuantity = typeof rawdefectiveQuantity === 'number' ? rawdefectiveQuantity : Number(rawdefectiveQuantity)
  }
  if ('yieldRate' in payload) {
    const rawyieldRate = payload.yieldRate
    payload.yieldRate = typeof rawyieldRate === 'number' ? rawyieldRate : Number(rawyieldRate)
  }
  if ('workEfficiency' in payload) {
    const rawworkEfficiency = payload.workEfficiency
    payload.workEfficiency = typeof rawworkEfficiency === 'number' ? rawworkEfficiency : Number(rawworkEfficiency)
  }
  if ('idleReasonType' in payload) {
    const rawidleReasonType = payload.idleReasonType
    payload.idleReasonType = typeof rawidleReasonType === 'number' ? rawidleReasonType : Number(rawidleReasonType)
  }
  if ('overtimeHours' in payload) {
    const rawovertimeHours = payload.overtimeHours
    payload.overtimeHours = typeof rawovertimeHours === 'number' ? rawovertimeHours : Number(rawovertimeHours)
  }
  if ('rateStatus' in payload) {
    const rawrateStatus = payload.rateStatus
    payload.rateStatus = typeof rawrateStatus === 'number' ? rawrateStatus : Number(rawrateStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.personnelOperationRateId)

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

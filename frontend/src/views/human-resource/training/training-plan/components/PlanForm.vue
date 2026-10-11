<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-plan/components -->
<!-- 文件名称：PlanForm.vue -->
<!-- 功能描述：培训计划维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="plan-form-tabs"
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
                :label="pi.label('planCode')"
                name="planCode"
              >
                <a-input
                  v-model:value="formState.planCode"
                  :placeholder="pi.ph('planCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.trainingPlanId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planName')"
                name="planName"
              >
                <a-input
                  v-model:value="formState.planName"
                  :placeholder="pi.ph('planName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planYear')"
                name="planYear"
              >
                <a-input-number
                  v-model:value="formState.planYear"
                  :placeholder="pi.ph('planYear')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planType')"
                name="planType"
              >
                <TaktSelect
                  v-model:value="formState.planType"
                  dict-type="humanresource_training_plan_type"
                  :placeholder="pi.ph('planType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicableDepartment')"
                name="applicableDepartment"
              >
                <a-input
                  v-model:value="formState.applicableDepartment"
                  :placeholder="pi.ph('applicableDepartment')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('startDate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="pi.ph('startDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('endDate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="pi.ph('endDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('trainingObjectives')"
                name="trainingObjectives"
              >
                <a-input
                  v-model:value="formState.trainingObjectives"
                  :placeholder="pi.ph('trainingObjectives')"
                  show-count
                  :maxlength="1000"
                  allow-clear
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
                :label="pi.label('plannedHeadcount')"
                name="plannedHeadcount"
              >
                <a-input-number
                  v-model:value="formState.plannedHeadcount"
                  :placeholder="pi.ph('plannedHeadcount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('trainingBudget')"
                name="trainingBudget"
              >
                <a-input-number
                  v-model:value="formState.trainingBudget"
                  :placeholder="pi.ph('trainingBudget')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('trainingPlanDescription')"
                name="trainingPlanDescription"
              >
                <a-textarea
                  v-model:value="formState.trainingPlanDescription"
                  :placeholder="pi.ph('trainingPlanDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('trainingPlanStatus')"
                name="trainingPlanStatus"
              >
                <TaktSelect
                  v-model:value="formState.trainingPlanStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('trainingPlanStatus')"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 培训计划维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training/training-plan/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useTrainingPlanI18n } from '../composables/usePlanI18n'

/** 实体字段 i18n */
const pi = useTrainingPlanI18n()
import type { TrainingPlanCreate } from '@/types/human-resource/training/plan.types'
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
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TrainingPlanCreate & { trainingPlanId?: string }> | null
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
  trainingPlanStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 trainingPlanId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.trainingPlanId) {
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
    if (!props.formData?.trainingPlanId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  planCode: [
    {
      required: true,
      message: pi.ph('planCode'),
      trigger: 'blur'
    }
  ],
  planName: [
    {
      required: true,
      message: pi.ph('planName'),
      trigger: 'blur'
    }
  ],
  planYear: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('planYear'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('planYear'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  planType: [
    {
      required: true,
      message: pi.ph('planType'),
      trigger: 'change'
    }
  ],
  applicableDepartment: [
    {
      required: true,
      message: pi.ph('applicableDepartment'),
      trigger: 'blur'
    }
  ],
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
  trainingObjectives: [
    {
      required: true,
      message: pi.ph('trainingObjectives'),
      trigger: 'blur'
    }
  ],
  plannedHeadcount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedHeadcount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedHeadcount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  trainingBudget: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('trainingBudget'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('trainingBudget'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  trainingPlanDescription: [
    {
      required: true,
      message: pi.ph('trainingPlanDescription'),
      trigger: 'blur'
    }
  ],
  trainingPlanStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('trainingPlanStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('trainingPlanStatus'))
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
  if ('planYear' in payload) {
    const rawplanYear = payload.planYear
    if (rawplanYear === undefined || rawplanYear === null || rawplanYear === '') {
      delete payload.planYear
    } else {
      const numplanYear = typeof rawplanYear === 'number' ? rawplanYear : Number(rawplanYear)
      if (Number.isFinite(numplanYear)) payload.planYear = numplanYear
      else delete payload.planYear
    }
  }
  if ('plannedHeadcount' in payload) {
    const rawplannedHeadcount = payload.plannedHeadcount
    if (rawplannedHeadcount === undefined || rawplannedHeadcount === null || rawplannedHeadcount === '') {
      delete payload.plannedHeadcount
    } else {
      const numplannedHeadcount = typeof rawplannedHeadcount === 'number' ? rawplannedHeadcount : Number(rawplannedHeadcount)
      if (Number.isFinite(numplannedHeadcount)) payload.plannedHeadcount = numplannedHeadcount
      else delete payload.plannedHeadcount
    }
  }
  if ('trainingBudget' in payload) {
    const rawtrainingBudget = payload.trainingBudget
    if (rawtrainingBudget === undefined || rawtrainingBudget === null || rawtrainingBudget === '') {
      delete payload.trainingBudget
    } else {
      const numtrainingBudget = typeof rawtrainingBudget === 'number' ? rawtrainingBudget : Number(rawtrainingBudget)
      if (Number.isFinite(numtrainingBudget)) payload.trainingBudget = numtrainingBudget
      else delete payload.trainingBudget
    }
  }
  if ('trainingPlanStatus' in payload) {
    const rawtrainingPlanStatus = payload.trainingPlanStatus
    if (rawtrainingPlanStatus === undefined || rawtrainingPlanStatus === null || rawtrainingPlanStatus === '') {
      delete payload.trainingPlanStatus
    } else {
      const numtrainingPlanStatus = typeof rawtrainingPlanStatus === 'number' ? rawtrainingPlanStatus : Number(rawtrainingPlanStatus)
      if (Number.isFinite(numtrainingPlanStatus)) payload.trainingPlanStatus = numtrainingPlanStatus
      else delete payload.trainingPlanStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.trainingPlanId) {
    payload.trainingPlanId = props.formData.trainingPlanId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.trainingPlanId)

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

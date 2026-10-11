<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-objective/components -->
<!-- 文件名称：PerfObjectiveForm.vue -->
<!-- 功能描述：员工绩效目标维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="perf-objective-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.employeename') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.schememetricid')"
                name="schemeMetricId"
              >
                <a-input
                  v-model:value="formState.schemeMetricId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.schememetricid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.objectiveperiod')"
                name="objectivePeriod"
              >
                <a-input
                  v-model:value="formState.objectivePeriod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.objectiveperiod') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.objectivedescription')"
                name="objectiveDescription"
              >
                <a-textarea
                  v-model:value="formState.objectiveDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfobjective.objectivedescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.targetvalue')"
                name="targetValue"
              >
                <a-input-number
                  v-model:value="formState.targetValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.targetvalue') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfobjective.actualvalue')"
                name="actualValue"
              >
                <a-input-number
                  v-model:value="formState.actualValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.actualvalue') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.completionpercentage')"
                name="completionPercentage"
              >
                <a-input-number
                  v-model:value="formState.completionPercentage"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.completionpercentage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.objectiveweight')"
                name="objectiveWeight"
              >
                <a-input-number
                  v-model:value="formState.objectiveWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.objectiveweight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfobjective.startdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.duedate')"
                name="dueDate"
              >
                <a-date-picker
                  v-model:value="formState.dueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfobjective.duedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.achievementnotes')"
                name="achievementNotes"
              >
                <a-textarea
                  v-model:value="formState.achievementNotes"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfobjective.achievementnotes') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.objectivestatus')"
                name="objectiveStatus"
              >
                <a-input-number
                  v-model:value="formState.objectiveStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.objectivestatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.perfobjective.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfobjective.relatedplant') })"
                  show-count
                  :maxlength="4"
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
                    <span>{{ t('common.page.entity.extfield') }}</span>
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
                :label="t('common.page.entity.remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
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
 * 员工绩效目标维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-objective/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PerfObjectiveCreate } from '@/types/human-resource/performance/perfObjective.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","cultureCode","employeeId","employeeName","schemeMetricId","objectivePeriod","objectiveDescription","targetValue","actualValue","completionPercentage","objectiveWeight","startDate","dueDate","achievementNotes","objectiveStatus","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PerfObjectiveCreate & { perfObjectiveId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 perfObjectiveId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.perfObjectiveId) {
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
    const isCreate = !props.formData?.perfObjectiveId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.employeename') }),
      trigger: 'blur'
    }
  ],
  schemeMetricId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.schememetricid') }),
      trigger: 'blur'
    }
  ],
  objectivePeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.objectiveperiod') }),
      trigger: 'blur'
    }
  ],
  objectiveDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.objectivedescription') }),
      trigger: 'blur'
    }
  ],
  targetValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.targetvalue') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.targetvalue') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.actualvalue') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.actualvalue') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  completionPercentage: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.completionpercentage') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.completionpercentage') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  objectiveWeight: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.objectiveweight') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.objectiveweight') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfobjective.startdate') }),
      trigger: 'change'
    }
  ],
  dueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.perfobjective.duedate') }),
      trigger: 'change'
    }
  ],
  achievementNotes: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfobjective.achievementnotes') }),
      trigger: 'blur'
    }
  ],
  objectiveStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.objectivestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfobjective.objectivestatus') }))
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
  if ('targetValue' in payload) {
    const rawtargetValue = payload.targetValue
    payload.targetValue = typeof rawtargetValue === 'number' ? rawtargetValue : Number(rawtargetValue)
  }
  if ('actualValue' in payload) {
    const rawactualValue = payload.actualValue
    payload.actualValue = typeof rawactualValue === 'number' ? rawactualValue : Number(rawactualValue)
  }
  if ('completionPercentage' in payload) {
    const rawcompletionPercentage = payload.completionPercentage
    payload.completionPercentage = typeof rawcompletionPercentage === 'number' ? rawcompletionPercentage : Number(rawcompletionPercentage)
  }
  if ('objectiveWeight' in payload) {
    const rawobjectiveWeight = payload.objectiveWeight
    payload.objectiveWeight = typeof rawobjectiveWeight === 'number' ? rawobjectiveWeight : Number(rawobjectiveWeight)
  }
  if ('objectiveStatus' in payload) {
    const rawobjectiveStatus = payload.objectiveStatus
    payload.objectiveStatus = typeof rawobjectiveStatus === 'number' ? rawobjectiveStatus : Number(rawobjectiveStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.perfObjectiveId)

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

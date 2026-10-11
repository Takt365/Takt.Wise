<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-scheme/components -->
<!-- 文件名称：PerfSchemeForm.vue -->
<!-- 功能描述：绩效方案指标维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="perf-scheme-form-tabs"
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
                :label="t('entity.perfscheme.schemecode')"
                name="schemeCode"
              >
                <a-input
                  v-model:value="formState.schemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemecode') })"
                  show-count
                  :maxlength="64"
                  allow-clear
                  :disabled="!!formData?.perfSchemeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.schemename')"
                name="schemeName"
              >
                <a-input
                  v-model:value="formState.schemeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemename') })"
                  show-count
                  :maxlength="128"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.applicabledepartment')"
                name="applicableDepartment"
              >
                <a-input
                  v-model:value="formState.applicableDepartment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.applicabledepartment') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.cycletype')"
                name="cycleType"
              >
                <a-input
                  v-model:value="formState.cycleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.cycletype') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.scoringstandard')"
                name="scoringStandard"
              >
                <a-input
                  v-model:value="formState.scoringStandard"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringstandard') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.selfevaluationweight')"
                name="selfEvaluationWeight"
              >
                <a-input-number
                  v-model:value="formState.selfEvaluationWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.selfevaluationweight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.supervisorweight')"
                name="supervisorWeight"
              >
                <a-input-number
                  v-model:value="formState.supervisorWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.supervisorweight') })"
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
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.metriccode')"
                name="metricCode"
              >
                <a-input
                  v-model:value="formState.metricCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metriccode') })"
                  show-count
                  :maxlength="64"
                  allow-clear
                  :disabled="!!formData?.perfSchemeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.metricname')"
                name="metricName"
              >
                <a-input
                  v-model:value="formState.metricName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metricname') })"
                  show-count
                  :maxlength="128"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.category')"
                name="category"
              >
                <a-input
                  v-model:value="formState.category"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.category') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.metrictype')"
                name="metricType"
              >
                <a-input
                  v-model:value="formState.metricType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metrictype') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.scoringcriteria')"
                name="scoringCriteria"
              >
                <a-input
                  v-model:value="formState.scoringCriteria"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringcriteria') })"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.standardweight')"
                name="standardWeight"
              >
                <a-input-number
                  v-model:value="formState.standardWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.standardweight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.schememetricstatus')"
                name="schemeMetricStatus"
              >
                <a-input-number
                  v-model:value="formState.schemeMetricStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schememetricstatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.perfscheme.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfscheme.relatedplant') })"
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
 * 绩效方案指标维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-scheme/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PerfSchemeCreate } from '@/types/human-resource/performance/perfScheme.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","schemeCode","schemeName","applicableDepartment","cycleType","scoringStandard","selfEvaluationWeight","supervisorWeight","metricCode","metricName","category","metricType","scoringCriteria","standardWeight","schemeMetricStatus","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PerfSchemeCreate & { perfSchemeId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 perfSchemeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.perfSchemeId) {
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
    const isCreate = !props.formData?.perfSchemeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  schemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemecode') }),
      trigger: 'blur'
    }
  ],
  schemeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.schemename') }),
      trigger: 'blur'
    }
  ],
  applicableDepartment: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.applicabledepartment') }),
      trigger: 'blur'
    }
  ],
  cycleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.cycletype') }),
      trigger: 'blur'
    }
  ],
  scoringStandard: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringstandard') }),
      trigger: 'blur'
    }
  ],
  selfEvaluationWeight: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.selfevaluationweight') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.selfevaluationweight') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  supervisorWeight: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.supervisorweight') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.supervisorweight') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  metricCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metriccode') }),
      trigger: 'blur'
    }
  ],
  metricName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metricname') }),
      trigger: 'blur'
    }
  ],
  category: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.category') }),
      trigger: 'blur'
    }
  ],
  metricType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.metrictype') }),
      trigger: 'blur'
    }
  ],
  scoringCriteria: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.perfscheme.scoringcriteria') }),
      trigger: 'blur'
    }
  ],
  standardWeight: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.standardweight') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.standardweight') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  schemeMetricStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.schememetricstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.perfscheme.schememetricstatus') }))
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
  if ('selfEvaluationWeight' in payload) {
    const rawselfEvaluationWeight = payload.selfEvaluationWeight
    payload.selfEvaluationWeight = typeof rawselfEvaluationWeight === 'number' ? rawselfEvaluationWeight : Number(rawselfEvaluationWeight)
  }
  if ('supervisorWeight' in payload) {
    const rawsupervisorWeight = payload.supervisorWeight
    payload.supervisorWeight = typeof rawsupervisorWeight === 'number' ? rawsupervisorWeight : Number(rawsupervisorWeight)
  }
  if ('standardWeight' in payload) {
    const rawstandardWeight = payload.standardWeight
    payload.standardWeight = typeof rawstandardWeight === 'number' ? rawstandardWeight : Number(rawstandardWeight)
  }
  if ('schemeMetricStatus' in payload) {
    const rawschemeMetricStatus = payload.schemeMetricStatus
    payload.schemeMetricStatus = typeof rawschemeMetricStatus === 'number' ? rawschemeMetricStatus : Number(rawschemeMetricStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.perfSchemeId)

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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-plan/components -->
<!-- 文件名称：TrainingPlanForm.vue -->
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
      class="training-plan-form-tabs"
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
                :label="t('entity.trainingplan.plancode')"
                name="planCode"
              >
                <a-input
                  v-model:value="formState.planCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.plancode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.planname')"
                name="planName"
              >
                <a-input
                  v-model:value="formState.planName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.planname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.planyear')"
                name="planYear"
              >
                <a-input-number
                  v-model:value="formState.planYear"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.planyear') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.plantype')"
                name="planType"
              >
                <a-input
                  v-model:value="formState.planType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.plantype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.applicabledepartment')"
                name="applicableDepartment"
              >
                <a-input
                  v-model:value="formState.applicableDepartment"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.applicabledepartment') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingplan.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.enddate')"
                name="endDate"
              >
                <a-date-picker
                  v-model:value="formState.endDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingplan.enddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
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
                :label="t('entity.trainingplan.trainingobjectives')"
                name="trainingObjectives"
              >
                <a-input
                  v-model:value="formState.trainingObjectives"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.trainingobjectives') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.plannedheadcount')"
                name="plannedHeadcount"
              >
                <a-input-number
                  v-model:value="formState.plannedHeadcount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.plannedheadcount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.trainingbudget')"
                name="trainingBudget"
              >
                <a-input-number
                  v-model:value="formState.trainingBudget"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.trainingbudget') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.trainingplan.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.trainingplan.description') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.status')"
                name="trainingPlanStatus"
              >
                <TaktSelect
                  v-model:value="formState.trainingPlanStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingplan.status') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingplan.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingplan.relatedplant') })"
                  size="small"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfield')"
                name="ExtField"
              >
                <a-input
                  v-model:value="formState.ExtField"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfield') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
import type { TrainingPlanCreate } from '@/types/human-resource/training/trainingPlan.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","planCode","planName","planYear","planType","applicableDepartment","startDate","endDate","trainingObjectives","plannedHeadcount","trainingBudget","description","trainingPlanStatus","plantCode","ExtField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TrainingPlanCreate & { trainingPlanId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    const isCreate = !props.formData?.trainingPlanId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  planCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.plancode') }),
      trigger: 'blur'
    }
  ],
  planName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.planname') }),
      trigger: 'blur'
    }
  ],
  planYear: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.planyear') }),
      trigger: 'change'
    }
  ],
  planType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.plantype') }),
      trigger: 'blur'
    }
  ],
  applicableDepartment: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.applicabledepartment') }),
      trigger: 'blur'
    }
  ],
  startDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.startdate') }),
      trigger: 'change'
    }
  ],
  endDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.enddate') }),
      trigger: 'change'
    }
  ],
  trainingObjectives: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.trainingobjectives') }),
      trigger: 'blur'
    }
  ],
  plannedHeadcount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.plannedheadcount') }),
      trigger: 'change'
    }
  ],
  trainingBudget: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.trainingbudget') }),
      trigger: 'change'
    }
  ],
  description: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingplan.description') }),
      trigger: 'blur'
    }
  ],
  trainingPlanStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingplan.status') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return { ...formState }
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-attendee/components -->
<!-- 文件名称：TrainingAttendeeForm.vue -->
<!-- 功能描述：员工培训结果记录维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="training-attendee-form-tabs"
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
                :label="t('entity.trainingattendee.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeeid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.employeename')"
                name="employeeName"
              >
                <a-input
                  v-model:value="formState.employeeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingcourseid')"
                name="trainingCourseId"
              >
                <a-input
                  v-model:value="formState.trainingCourseId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingcourseid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.coursename')"
                name="courseName"
              >
                <a-input
                  v-model:value="formState.courseName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.coursename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingtype')"
                name="trainingType"
              >
                <a-input
                  v-model:value="formState.trainingType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingtype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.instructor')"
                name="instructor"
              >
                <a-input
                  v-model:value="formState.instructor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.instructor') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingstartdate')"
                name="trainingStartDate"
              >
                <a-date-picker
                  v-model:value="formState.trainingStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingstartdate') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingenddate')"
                name="trainingEndDate"
              >
                <a-date-picker
                  v-model:value="formState.trainingEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingenddate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingdate')"
                name="trainingDate"
              >
                <a-date-picker
                  v-model:value="formState.trainingDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.traininghours')"
                name="trainingHours"
              >
                <a-input-number
                  v-model:value="formState.trainingHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.traininghours') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingscore')"
                name="trainingScore"
              >
                <a-input-number
                  v-model:value="formState.trainingScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.ispassed')"
                name="isPassed"
              >
                <a-input-number
                  v-model:value="formState.isPassed"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.ispassed') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.CertificateCode')"
                name="CertificateCode"
              >
                <a-input
                  v-model:value="formState.CertificateCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.CertificateCode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingevaluation')"
                name="trainingEvaluation"
              >
                <a-input
                  v-model:value="formState.trainingEvaluation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingevaluation') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.trainingresultstatus')"
                name="trainingResultStatus"
              >
                <a-input-number
                  v-model:value="formState.trainingResultStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingresultstatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingattendee.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.relatedplant') })"
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
 * 员工培训结果记录维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training/training-attendee/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TrainingAttendeeCreate } from '@/types/human-resource/training/trainingAttendee.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","employeeId","employeeName","trainingCourseId","courseName","trainingType","instructor","trainingStartDate","trainingEndDate","trainingDate","trainingHours","trainingScore","isPassed","CertificateCode","trainingEvaluation","trainingResultStatus","plantCode","ExtField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TrainingAttendeeCreate & { trainingAttendeeId?: string }> | null
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
    const isCreate = !props.formData?.trainingAttendeeId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeeid') }),
      trigger: 'blur'
    }
  ],
  employeeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeename') }),
      trigger: 'blur'
    }
  ],
  trainingCourseId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingcourseid') }),
      trigger: 'blur'
    }
  ],
  courseName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.coursename') }),
      trigger: 'blur'
    }
  ],
  trainingType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingtype') }),
      trigger: 'blur'
    }
  ],
  instructor: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.instructor') }),
      trigger: 'blur'
    }
  ],
  trainingStartDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingstartdate') }),
      trigger: 'change'
    }
  ],
  trainingEndDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingenddate') }),
      trigger: 'change'
    }
  ],
  trainingDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingdate') }),
      trigger: 'change'
    }
  ],
  trainingHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.traininghours') }),
      trigger: 'change'
    }
  ],
  trainingScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingscore') }),
      trigger: 'change'
    }
  ],
  isPassed: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.ispassed') }),
      trigger: 'change'
    }
  ],
  CertificateCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.CertificateCode') }),
      trigger: 'blur'
    }
  ],
  trainingEvaluation: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingevaluation') }),
      trigger: 'blur'
    }
  ],
  trainingResultStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingresultstatus') }),
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

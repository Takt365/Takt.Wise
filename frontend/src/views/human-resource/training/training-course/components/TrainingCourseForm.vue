<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-course/components -->
<!-- 文件名称：TrainingCourseForm.vue -->
<!-- 功能描述：培训课程定义维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="training-course-form-tabs"
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
                :label="t('entity.trainingcourse.coursecode')"
                name="courseCode"
              >
                <a-input
                  v-model:value="formState.courseCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.coursename')"
                name="courseName"
              >
                <a-input
                  v-model:value="formState.courseName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursename') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.coursetype')"
                name="courseType"
              >
                <a-input
                  v-model:value="formState.courseType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursetype') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.courselevel')"
                name="courseLevel"
              >
                <a-input
                  v-model:value="formState.courseLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courselevel') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.trainingcourse.coursedescription')"
                name="courseDescription"
              >
                <a-textarea
                  v-model:value="formState.courseDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.trainingcourse.coursedescription') })"
                  :rows="2"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.courseobjectives')"
                name="courseObjectives"
              >
                <a-input
                  v-model:value="formState.courseObjectives"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courseobjectives') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.traininghours')"
                name="trainingHours"
              >
                <a-input-number
                  v-model:value="formState.trainingHours"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.traininghours') })"
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
                :label="t('entity.trainingcourse.maininstructor')"
                name="mainInstructor"
              >
                <a-input
                  v-model:value="formState.mainInstructor"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.maininstructor') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.trainingmethod')"
                name="trainingMethod"
              >
                <a-input
                  v-model:value="formState.trainingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.trainingmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.assessmentmethod')"
                name="assessmentMethod"
              >
                <a-input
                  v-model:value="formState.assessmentMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.assessmentmethod') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.passingscore')"
                name="passingScore"
              >
                <a-input-number
                  v-model:value="formState.passingScore"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.passingscore') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.status')"
                name="trainingCourseStatus"
              >
                <TaktSelect
                  v-model:value="formState.trainingCourseStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingcourse.status') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.trainingcourse.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.relatedplant') })"
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
 * 培训课程定义维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training/training-course/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TrainingCourseCreate } from '@/types/human-resource/training/trainingCourse.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","courseCode","courseName","courseType","courseLevel","courseDescription","courseObjectives","trainingHours","mainInstructor","trainingMethod","assessmentMethod","passingScore","trainingCourseStatus","plantCode","ExtField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TrainingCourseCreate & { trainingCourseId?: string }> | null
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
    const isCreate = !props.formData?.trainingCourseId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  courseCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursecode') }),
      trigger: 'blur'
    }
  ],
  courseName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursename') }),
      trigger: 'blur'
    }
  ],
  courseType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursetype') }),
      trigger: 'blur'
    }
  ],
  courseLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courselevel') }),
      trigger: 'blur'
    }
  ],
  courseDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursedescription') }),
      trigger: 'blur'
    }
  ],
  courseObjectives: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courseobjectives') }),
      trigger: 'blur'
    }
  ],
  trainingHours: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingcourse.traininghours') }),
      trigger: 'change'
    }
  ],
  mainInstructor: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.maininstructor') }),
      trigger: 'blur'
    }
  ],
  trainingMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.trainingmethod') }),
      trigger: 'blur'
    }
  ],
  assessmentMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.assessmentmethod') }),
      trigger: 'blur'
    }
  ],
  passingScore: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingcourse.passingscore') }),
      trigger: 'change'
    }
  ],
  trainingCourseStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.trainingcourse.status') }),
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
  const payload = { ...formState }
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
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

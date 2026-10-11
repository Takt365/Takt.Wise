<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/model-destination/components -->
<!-- 文件名称：ModelDestinationForm.vue -->
<!-- 功能描述：Takt型号目的地实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.modelDestinationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.materialdescription')"
                name="materialDescription"
              >
                <a-input
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.materialdescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.modelcode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.modelcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.modelDestinationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.modelname')"
                name="modelName"
              >
                <a-input
                  v-model:value="formState.modelName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.modelname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.destinationcode')"
                name="destinationCode"
              >
                <a-input
                  v-model:value="formState.destinationCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.destinationcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.modelDestinationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.modeldestination.destinationname')"
                name="destinationName"
              >
                <a-input
                  v-model:value="formState.destinationName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.modeldestination.destinationname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt型号目的地实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/model-destination/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ModelDestinationCreate } from '@/types/logistics/materials/modelDestination.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：租户级实体仅注入 tenantCode，表单只读
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
}
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","materialCode","materialDescription","modelCode","modelName","destinationCode","destinationName","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ModelDestinationCreate & { modelDestinationId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 modelDestinationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.modelDestinationId) {
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

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    const isCreate = !props.formData?.modelDestinationId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.materialdescription') }),
      trigger: 'blur'
    }
  ],
  modelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.modelcode') }),
      trigger: 'blur'
    }
  ],
  modelName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.modelname') }),
      trigger: 'blur'
    }
  ],
  destinationCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.destinationcode') }),
      trigger: 'blur'
    }
  ],
  destinationName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.modeldestination.destinationname') }),
      trigger: 'blur'
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

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.modelDestinationId)

  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>


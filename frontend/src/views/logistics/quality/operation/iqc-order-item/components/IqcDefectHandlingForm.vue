<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order-item/components -->
<!-- 文件名称：IqcDefectHandlingForm.vue -->
<!-- 功能描述：IQC进货检验单明细实体子表 iqcDefectHandling 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form iqc-defect-handling-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="iqc-defect-handling-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.code')"
                name="iqcDefectHandlingCode"
              >
                <a-input
                  v-model:value="formState.iqcDefectHandlingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.iqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.iqcordercode')"
                name="iqcOrderCode"
              >
                <a-input
                  v-model:value="formState.iqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.iqcordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.defecttype')"
                name="defectType"
              >
                <a-input-number
                  v-model:value="formState.defectType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defecttype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.defectcode')"
                name="defectCode"
              >
                <a-input
                  v-model:value="formState.defectCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defectcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.iqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcdefecthandling.defectdescription')"
                name="defectDescription"
              >
                <a-textarea
                  v-model:value="formState.defectDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcdefecthandling.defectdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.defectquantity')"
                name="defectQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defectquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcdefecthandling.handlingmethod')"
                name="handlingMethod"
              >
                <a-input-number
                  v-model:value="formState.handlingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.handlingmethod') })"
                  style="width: 100%"
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
 * IQC进货检验单明细实体子表 iqcDefectHandling 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/operation/iqc-order-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IqcDefectHandlingCreate } from '@/types/logistics/quality/operation/iqcDefectHandling.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["iqcDefectHandlingCode","iqcOrderCode","lineNumber","defectType","defectCode","defectDescription","defectQuantity","handlingMethod"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IqcDefectHandlingCreate & { iqcDefectHandlingId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 iqcDefectHandlingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.iqcDefectHandlingId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  iqcDefectHandlingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.code') }),
      trigger: 'blur'
    }
  ],
  iqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.iqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.defecttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.defecttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defectcode') }),
      trigger: 'blur'
    }
  ],
  defectDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defectdescription') }),
      trigger: 'blur'
    }
  ],
  defectQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.defectquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.defectquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.handlingmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcdefecthandling.handlingmethod') }))
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

/** 映射为 Create/Update DTO（含主表外键 iqcOrderItemId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('defectType' in payload) {
    const rawdefectType = payload.defectType
    payload.defectType = typeof rawdefectType === 'number' ? rawdefectType : Number(rawdefectType)
  }
  if ('defectQuantity' in payload) {
    const rawdefectQuantity = payload.defectQuantity
    payload.defectQuantity = typeof rawdefectQuantity === 'number' ? rawdefectQuantity : Number(rawdefectQuantity)
  }
  if ('handlingMethod' in payload) {
    const rawhandlingMethod = payload.handlingMethod
    payload.handlingMethod = typeof rawhandlingMethod === 'number' ? rawhandlingMethod : Number(rawhandlingMethod)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.iqcOrderItemId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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

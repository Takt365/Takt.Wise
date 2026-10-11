<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order-item/components -->
<!-- 文件名称：IpqcDefectHandlingForm.vue -->
<!-- 功能描述：IPQC制程检验单明细实体子表 ipqcDefectHandling 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ipqc-defect-handling-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ipqc-defect-handling-form-tabs"
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
                :label="t('entity.ipqcdefecthandling.code')"
                name="ipqcDefectHandlingCode"
              >
                <a-input
                  v-model:value="formState.ipqcDefectHandlingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.ipqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.ipqcordercode')"
                name="ipqcOrderCode"
              >
                <a-input
                  v-model:value="formState.ipqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.ipqcordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.ipqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.defecttype')"
                name="defectType"
              >
                <a-input-number
                  v-model:value="formState.defectType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defecttype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.defectcode')"
                name="defectCode"
              >
                <a-input
                  v-model:value="formState.defectCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.ipqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.defectdescription')"
                name="defectDescription"
              >
                <a-textarea
                  v-model:value="formState.defectDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcdefecthandling.defectdescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.defectquantity')"
                name="defectQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcdefecthandling.handlingmethod')"
                name="handlingMethod"
              >
                <a-input-number
                  v-model:value="formState.handlingMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlingmethod') })"
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
 * IPQC制程检验单明细实体子表 ipqcDefectHandling 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/operation/ipqc-order-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IpqcDefectHandlingCreate } from '@/types/logistics/quality/operation/ipqcDefectHandling.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["ipqcDefectHandlingCode","ipqcOrderCode","lineNumber","defectType","defectCode","defectDescription","defectQuantity","handlingMethod"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IpqcDefectHandlingCreate & { ipqcDefectHandlingId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ipqcDefectHandlingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ipqcDefectHandlingId) {
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
  ipqcDefectHandlingCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.code') }),
      trigger: 'blur'
    }
  ],
  ipqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.ipqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.defecttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.defecttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectcode') }),
      trigger: 'blur'
    }
  ],
  defectDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectdescription') }),
      trigger: 'blur'
    }
  ],
  defectQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.defectquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.defectquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.handlingmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.handlingmethod') }))
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

/** 映射为 Create/Update DTO（含主表外键 ipqcOrderItemId） */
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
  payload.ipqcOrderItemId = props.masterId
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

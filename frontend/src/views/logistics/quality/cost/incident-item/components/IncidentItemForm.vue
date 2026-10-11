<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/incident-item/components -->
<!-- 文件名称：IncidentItemForm.vue -->
<!-- 功能描述：品质事故主表子表 qualityIncidentItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form incident-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="incident-item-form-tabs"
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
                :label="t('entity.qualityincidentitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.qualityIncidentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.materialdescription')"
                name="materialDescription"
              >
                <a-input
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialdescription') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.scrapcost')"
                name="scrapCost"
              >
                <a-input-number
                  v-model:value="formState.scrapCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.scrapsize')"
                name="scrapSize"
              >
                <a-input-number
                  v-model:value="formState.scrapSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapsize') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.partprice')"
                name="partPrice"
              >
                <a-input-number
                  v-model:value="formState.partPrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.partprice') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.scrapreasoncost')"
                name="scrapReasonCost"
              >
                <a-input-number
                  v-model:value="formState.scrapReasonCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.scrapreasoncost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincidentitem.freightcharges')"
                name="freightCharges"
              >
                <a-input-number
                  v-model:value="formState.freightCharges"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.freightcharges') })"
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
 * 品质事故主表子表 qualityIncidentItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/incident-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIncidentItemCreate } from '@/types/logistics/quality/cost/incidentItem.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","materialCode","materialDescription","scrapCost","scrapSize","partPrice","scrapReasonCost","freightCharges"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIncidentItemCreate & { qualityIncidentItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIncidentItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIncidentItemId) {
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
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityincidentitem.materialdescription') }),
      trigger: 'blur'
    }
  ],
  scrapCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scrapSize: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapsize') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapsize') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  partPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.partprice') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.partprice') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scrapReasonCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapreasoncost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.scrapreasoncost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  freightCharges: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.freightcharges') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincidentitem.freightcharges') }))
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

/** 映射为 Create/Update DTO（含主表外键 qualityIncidentId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('scrapCost' in payload) {
    const rawscrapCost = payload.scrapCost
    payload.scrapCost = typeof rawscrapCost === 'number' ? rawscrapCost : Number(rawscrapCost)
  }
  if ('scrapSize' in payload) {
    const rawscrapSize = payload.scrapSize
    payload.scrapSize = typeof rawscrapSize === 'number' ? rawscrapSize : Number(rawscrapSize)
  }
  if ('partPrice' in payload) {
    const rawpartPrice = payload.partPrice
    payload.partPrice = typeof rawpartPrice === 'number' ? rawpartPrice : Number(rawpartPrice)
  }
  if ('scrapReasonCost' in payload) {
    const rawscrapReasonCost = payload.scrapReasonCost
    payload.scrapReasonCost = typeof rawscrapReasonCost === 'number' ? rawscrapReasonCost : Number(rawscrapReasonCost)
  }
  if ('freightCharges' in payload) {
    const rawfreightCharges = payload.freightCharges
    payload.freightCharges = typeof rawfreightCharges === 'number' ? rawfreightCharges : Number(rawfreightCharges)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.qualityIncidentId = props.masterId
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

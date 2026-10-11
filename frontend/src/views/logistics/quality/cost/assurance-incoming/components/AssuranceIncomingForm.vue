<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/assurance-incoming/components -->
<!-- 文件名称：AssuranceIncomingForm.vue -->
<!-- 功能描述：品质业务主表子表 qualityAssuranceIncoming 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assurance-incoming-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assurance-incoming-form-tabs"
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
                :label="t('entity.qualityassuranceincoming.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.directmanpowercostperminute')"
                name="directManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.directManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.directmanpowercostperminute') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.incominginspectioncost')"
                name="incomingInspectionCost"
              >
                <a-input-number
                  v-model:value="formState.incomingInspectionCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.incominginspectioncost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.inspectiontimeminutes')"
                name="inspectionTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.inspectionTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.inspectiontimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.travelcost')"
                name="travelCost"
              >
                <a-input-number
                  v-model:value="formState.travelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.travelcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.otherexpenses')"
                name="otherExpenses"
              >
                <a-input-number
                  v-model:value="formState.otherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassuranceincoming.otherexpenses') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityassuranceincoming.incomingnote')"
                name="incomingNote"
              >
                <a-textarea
                  v-model:value="formState.incomingNote"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityassuranceincoming.incomingnote') })"
                  :rows="2"
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质业务主表子表 qualityAssuranceIncoming 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/assurance-incoming/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityAssuranceIncomingCreate } from '@/types/logistics/quality/cost/assuranceIncoming.types'
import { RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","directManpowerCostPerMinute","incomingInspectionCost","inspectionTimeMinutes","travelCost","otherExpenses","incomingNote","extField"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityAssuranceIncomingCreate & { qualityAssuranceIncomingId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityAssuranceIncomingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityAssuranceIncomingId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  directManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.directmanpowercostperminute') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.directmanpowercostperminute') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  incomingInspectionCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.incominginspectioncost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.incominginspectioncost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.inspectiontimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.inspectiontimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  travelCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.travelcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.travelcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.otherexpenses') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassuranceincoming.otherexpenses') }))
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

/** 映射为 Create/Update DTO（含主表外键 qualityAssuranceId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('directManpowerCostPerMinute' in payload) {
    const rawdirectManpowerCostPerMinute = payload.directManpowerCostPerMinute
    payload.directManpowerCostPerMinute = typeof rawdirectManpowerCostPerMinute === 'number' ? rawdirectManpowerCostPerMinute : Number(rawdirectManpowerCostPerMinute)
  }
  if ('incomingInspectionCost' in payload) {
    const rawincomingInspectionCost = payload.incomingInspectionCost
    payload.incomingInspectionCost = typeof rawincomingInspectionCost === 'number' ? rawincomingInspectionCost : Number(rawincomingInspectionCost)
  }
  if ('inspectionTimeMinutes' in payload) {
    const rawinspectionTimeMinutes = payload.inspectionTimeMinutes
    payload.inspectionTimeMinutes = typeof rawinspectionTimeMinutes === 'number' ? rawinspectionTimeMinutes : Number(rawinspectionTimeMinutes)
  }
  if ('travelCost' in payload) {
    const rawtravelCost = payload.travelCost
    payload.travelCost = typeof rawtravelCost === 'number' ? rawtravelCost : Number(rawtravelCost)
  }
  if ('otherExpenses' in payload) {
    const rawotherExpenses = payload.otherExpenses
    payload.otherExpenses = typeof rawotherExpenses === 'number' ? rawotherExpenses : Number(rawotherExpenses)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.qualityAssuranceId = props.masterId
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

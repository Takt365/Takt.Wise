<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-assy-rework/components -->
<!-- 文件名称：IssueAssyReworkForm.vue -->
<!-- 功能描述：品质问题应对主表子表 qualityIssueAssyRework 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form issue-assy-rework-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="issue-assy-rework-form-tabs"
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
                :label="t('entity.qualityissueassyrework.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assydefectparts')"
                name="assyDefectParts"
              >
                <a-input
                  v-model:value="formState.assyDefectParts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assydefectparts') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assyreworkcost')"
                name="assyReworkCost"
              >
                <a-input-number
                  v-model:value="formState.assyReworkCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assyreworkcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assyreworktimeminutes')"
                name="assyReworkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.assyReworkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assyreworktimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assyreinspectiontimeminutes')"
                name="assyReinspectionTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.assyReinspectionTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assyreinspectiontimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assytravelcost')"
                name="assyTravelCost"
              >
                <a-input-number
                  v-model:value="formState.assyTravelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assytravelcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assywarehousecost')"
                name="assyWarehouseCost"
              >
                <a-input-number
                  v-model:value="formState.assyWarehouseCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assywarehousecost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissueassyrework.assyotherexpenses')"
                name="assyOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.assyOtherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissueassyrework.assyotherexpenses') })"
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
 * 品质问题应对主表子表 qualityIssueAssyRework 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/issue-assy-rework/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssueAssyReworkCreate } from '@/types/logistics/quality/cost/issueAssyRework.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","assyDefectParts","assyReworkCost","assyReworkTimeMinutes","assyReinspectionTimeMinutes","assyTravelCost","assyWarehouseCost","assyOtherExpenses"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIssueAssyReworkCreate & { qualityIssueAssyReworkId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIssueAssyReworkId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIssueAssyReworkId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyReworkCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreworkcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreworkcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyReworkTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreworktimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreworktimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyReinspectionTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreinspectiontimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyreinspectiontimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyTravelCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assytravelcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assytravelcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyWarehouseCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assywarehousecost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assywarehousecost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assyOtherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyotherexpenses') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissueassyrework.assyotherexpenses') }))
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

/** 映射为 Create/Update DTO（含主表外键 qualityIssueId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('assyReworkCost' in payload) {
    const rawassyReworkCost = payload.assyReworkCost
    payload.assyReworkCost = typeof rawassyReworkCost === 'number' ? rawassyReworkCost : Number(rawassyReworkCost)
  }
  if ('assyReworkTimeMinutes' in payload) {
    const rawassyReworkTimeMinutes = payload.assyReworkTimeMinutes
    payload.assyReworkTimeMinutes = typeof rawassyReworkTimeMinutes === 'number' ? rawassyReworkTimeMinutes : Number(rawassyReworkTimeMinutes)
  }
  if ('assyReinspectionTimeMinutes' in payload) {
    const rawassyReinspectionTimeMinutes = payload.assyReinspectionTimeMinutes
    payload.assyReinspectionTimeMinutes = typeof rawassyReinspectionTimeMinutes === 'number' ? rawassyReinspectionTimeMinutes : Number(rawassyReinspectionTimeMinutes)
  }
  if ('assyTravelCost' in payload) {
    const rawassyTravelCost = payload.assyTravelCost
    payload.assyTravelCost = typeof rawassyTravelCost === 'number' ? rawassyTravelCost : Number(rawassyTravelCost)
  }
  if ('assyWarehouseCost' in payload) {
    const rawassyWarehouseCost = payload.assyWarehouseCost
    payload.assyWarehouseCost = typeof rawassyWarehouseCost === 'number' ? rawassyWarehouseCost : Number(rawassyWarehouseCost)
  }
  if ('assyOtherExpenses' in payload) {
    const rawassyOtherExpenses = payload.assyOtherExpenses
    payload.assyOtherExpenses = typeof rawassyOtherExpenses === 'number' ? rawassyOtherExpenses : Number(rawassyOtherExpenses)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.qualityIssueId = props.masterId
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

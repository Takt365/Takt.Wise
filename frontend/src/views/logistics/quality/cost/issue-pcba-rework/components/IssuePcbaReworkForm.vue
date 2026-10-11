<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-pcba-rework/components -->
<!-- 文件名称：IssuePcbaReworkForm.vue -->
<!-- 功能描述：品质问题应对主表子表 qualityIssuePcbaRework 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form issue-pcba-rework-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="issue-pcba-rework-form-tabs"
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
                :label="t('entity.qualityissuepcbarework.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbadefectparts')"
                name="pcbaDefectParts"
              >
                <a-input
                  v-model:value="formState.pcbaDefectParts"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbadefectparts') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbareworkcost')"
                name="pcbaReworkCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaReworkCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareworkcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbareworktimeminutes')"
                name="pcbaReworkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.pcbaReworkTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareworktimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes')"
                name="pcbaReinspectionTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.pcbaReinspectionTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbatravelcost')"
                name="pcbaTravelCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaTravelCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbatravelcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbawarehousecost')"
                name="pcbaWarehouseCost"
              >
                <a-input-number
                  v-model:value="formState.pcbaWarehouseCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbawarehousecost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuepcbarework.pcbaotherexpenses')"
                name="pcbaOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.pcbaOtherExpenses"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuepcbarework.pcbaotherexpenses') })"
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
 * 品质问题应对主表子表 qualityIssuePcbaRework 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/issue-pcba-rework/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssuePcbaReworkCreate } from '@/types/logistics/quality/cost/issuePcbaRework.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","pcbaDefectParts","pcbaReworkCost","pcbaReworkTimeMinutes","pcbaReinspectionTimeMinutes","pcbaTravelCost","pcbaWarehouseCost","pcbaOtherExpenses"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIssuePcbaReworkCreate & { qualityIssuePcbaReworkId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIssuePcbaReworkId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIssuePcbaReworkId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaReworkCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareworkcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareworkcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaReworkTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareworktimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareworktimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaReinspectionTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaTravelCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbatravelcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbatravelcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaWarehouseCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbawarehousecost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbawarehousecost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbaOtherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbaotherexpenses') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuepcbarework.pcbaotherexpenses') }))
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
  if ('pcbaReworkCost' in payload) {
    const rawpcbaReworkCost = payload.pcbaReworkCost
    payload.pcbaReworkCost = typeof rawpcbaReworkCost === 'number' ? rawpcbaReworkCost : Number(rawpcbaReworkCost)
  }
  if ('pcbaReworkTimeMinutes' in payload) {
    const rawpcbaReworkTimeMinutes = payload.pcbaReworkTimeMinutes
    payload.pcbaReworkTimeMinutes = typeof rawpcbaReworkTimeMinutes === 'number' ? rawpcbaReworkTimeMinutes : Number(rawpcbaReworkTimeMinutes)
  }
  if ('pcbaReinspectionTimeMinutes' in payload) {
    const rawpcbaReinspectionTimeMinutes = payload.pcbaReinspectionTimeMinutes
    payload.pcbaReinspectionTimeMinutes = typeof rawpcbaReinspectionTimeMinutes === 'number' ? rawpcbaReinspectionTimeMinutes : Number(rawpcbaReinspectionTimeMinutes)
  }
  if ('pcbaTravelCost' in payload) {
    const rawpcbaTravelCost = payload.pcbaTravelCost
    payload.pcbaTravelCost = typeof rawpcbaTravelCost === 'number' ? rawpcbaTravelCost : Number(rawpcbaTravelCost)
  }
  if ('pcbaWarehouseCost' in payload) {
    const rawpcbaWarehouseCost = payload.pcbaWarehouseCost
    payload.pcbaWarehouseCost = typeof rawpcbaWarehouseCost === 'number' ? rawpcbaWarehouseCost : Number(rawpcbaWarehouseCost)
  }
  if ('pcbaOtherExpenses' in payload) {
    const rawpcbaOtherExpenses = payload.pcbaOtherExpenses
    payload.pcbaOtherExpenses = typeof rawpcbaOtherExpenses === 'number' ? rawpcbaOtherExpenses : Number(rawpcbaOtherExpenses)
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

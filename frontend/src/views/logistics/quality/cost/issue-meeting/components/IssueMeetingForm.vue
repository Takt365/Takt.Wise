<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-meeting/components -->
<!-- 文件名称：IssueMeetingForm.vue -->
<!-- 功能描述：品质问题应对主表子表 qualityIssueMeeting 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form issue-meeting-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="issue-meeting-form-tabs"
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
                :label="t('entity.qualityissuemeeting.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.directmanpowercostperminute')"
                name="directManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.directManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.directmanpowercostperminute') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.indirectmanpowercostperminute')"
                name="indirectManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.indirectManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.indirectmanpowercostperminute') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityissuemeeting.meetinginvestigationcontent')"
                name="meetingInvestigationContent"
              >
                <a-textarea
                  v-model:value="formState.meetingInvestigationContent"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityissuemeeting.meetinginvestigationcontent') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.meetinginvestigationcost')"
                name="meetingInvestigationCost"
              >
                <a-input-number
                  v-model:value="formState.meetingInvestigationCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.meetinginvestigationcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.meetingtimeminutes')"
                name="meetingTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.meetingTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.meetingtimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.directparticipantcount')"
                name="directParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.directParticipantCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.directparticipantcount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissuemeeting.indirectparticipantcount')"
                name="indirectParticipantCount"
              >
                <a-input-number
                  v-model:value="formState.indirectParticipantCount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissuemeeting.indirectparticipantcount') })"
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
 * 品质问题应对主表子表 qualityIssueMeeting 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/issue-meeting/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssueMeetingCreate } from '@/types/logistics/quality/cost/issueMeeting.types'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","directManpowerCostPerMinute","indirectManpowerCostPerMinute","meetingInvestigationContent","meetingInvestigationCost","meetingTimeMinutes","directParticipantCount","indirectParticipantCount"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIssueMeetingCreate & { qualityIssueMeetingId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIssueMeetingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIssueMeetingId) {
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
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  directManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.directmanpowercostperminute') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.directmanpowercostperminute') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.indirectmanpowercostperminute') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.indirectmanpowercostperminute') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  meetingInvestigationCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.meetinginvestigationcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.meetinginvestigationcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  meetingTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.meetingtimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.meetingtimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  directParticipantCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.directparticipantcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.directparticipantcount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectParticipantCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.indirectparticipantcount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissuemeeting.indirectparticipantcount') }))
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
  if ('directManpowerCostPerMinute' in payload) {
    const rawdirectManpowerCostPerMinute = payload.directManpowerCostPerMinute
    payload.directManpowerCostPerMinute = typeof rawdirectManpowerCostPerMinute === 'number' ? rawdirectManpowerCostPerMinute : Number(rawdirectManpowerCostPerMinute)
  }
  if ('indirectManpowerCostPerMinute' in payload) {
    const rawindirectManpowerCostPerMinute = payload.indirectManpowerCostPerMinute
    payload.indirectManpowerCostPerMinute = typeof rawindirectManpowerCostPerMinute === 'number' ? rawindirectManpowerCostPerMinute : Number(rawindirectManpowerCostPerMinute)
  }
  if ('meetingInvestigationCost' in payload) {
    const rawmeetingInvestigationCost = payload.meetingInvestigationCost
    payload.meetingInvestigationCost = typeof rawmeetingInvestigationCost === 'number' ? rawmeetingInvestigationCost : Number(rawmeetingInvestigationCost)
  }
  if ('meetingTimeMinutes' in payload) {
    const rawmeetingTimeMinutes = payload.meetingTimeMinutes
    payload.meetingTimeMinutes = typeof rawmeetingTimeMinutes === 'number' ? rawmeetingTimeMinutes : Number(rawmeetingTimeMinutes)
  }
  if ('directParticipantCount' in payload) {
    const rawdirectParticipantCount = payload.directParticipantCount
    payload.directParticipantCount = typeof rawdirectParticipantCount === 'number' ? rawdirectParticipantCount : Number(rawdirectParticipantCount)
  }
  if ('indirectParticipantCount' in payload) {
    const rawindirectParticipantCount = payload.indirectParticipantCount
    payload.indirectParticipantCount = typeof rawindirectParticipantCount === 'number' ? rawindirectParticipantCount : Number(rawindirectParticipantCount)
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

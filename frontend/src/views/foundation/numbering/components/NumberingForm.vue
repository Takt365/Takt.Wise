<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/numbering/components -->
<!-- 文件名称：NumberingForm.vue -->
<!-- 功能描述：编码规则实体 定义系统中各类业务单据的编码生成规则维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="numbering-form-tabs"
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
                :label="t('entity.numbering.rulecode')"
                name="ruleCode"
              >
                <a-input
                  v-model:value="formState.ruleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.numberingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.rulename')"
                name="ruleName"
              >
                <a-input
                  v-model:value="formState.ruleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulename') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.documenttype')"
                name="documentType"
              >
                <TaktTreeSelect
                  v-model:value="formState.documentType"
                  api-url="TaktMenus/tree-options?valueBy=name"
                  :lazy="true"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.documenttype') })"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :disabled="props.loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.deptcode')"
                name="deptCode"
              >
                <TaktTreeSelect
                  v-model:value="formState.deptCode"
                  api-url="TaktDepts/iso-tree-options"
                  :lazy="true"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.deptcode') })"
                  allow-clear
                  :disabled="props.loading"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.prefixcode')"
                name="prefixCode"
              >
                <a-input
                  v-model:value="formState.prefixCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.prefixcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.numberingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.dateformat')"
                name="dateFormat"
              >
                <TaktSelect
                  v-model:value="formState.dateFormat"
                  dict-type="sys_numbering_date_format"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.dateformat') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.sequencelength')"
                name="sequenceLength"
              >
                <a-input-number
                  v-model:value="formState.sequenceLength"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencelength') })"
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
                :label="t('entity.numbering.sequencestep')"
                name="sequenceStep"
              >
                <a-input-number
                  v-model:value="formState.sequenceStep"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencestep') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.suffixcode')"
                name="suffixCode"
              >
                <a-input
                  v-model:value="formState.suffixCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.suffixcode') })"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.numberingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.resetperiod')"
                name="resetPeriod"
              >
                <TaktSelect
                  v-model:value="formState.resetPeriod"
                  dict-type="sys_reset_period"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.resetperiod') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.currentsequence')"
                name="currentSequence"
              >
                <a-input-number
                  v-model:value="formState.currentSequence"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.currentsequence') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.examplecode')"
                name="exampleCode"
              >
                <a-input
                  v-model:value="formState.exampleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.examplecode') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                  :disabled="!!formData?.numberingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.separator')"
                name="separator"
              >
                <a-input
                  v-model:value="formState.separator"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.separator') })"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.isbuiltin')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.numbering.status')"
                name="status"
              >
                <TaktSelect
                  v-model:value="formState.status"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.status') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.numbering.description')"
                name="description"
              >
                <a-textarea
                  v-model:value="formState.description"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.numbering.description') })"
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
 * 编码规则实体 定义系统中各类业务单据的编码生成规则维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/numbering/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { NumberingCreate } from '@/types/foundation/numbering.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
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
const formFields = ["tenantCode","companyCode","cultureCode","ruleCode","ruleName","documentType","deptCode","prefixCode","dateFormat","sequenceLength","sequenceStep","suffixCode","resetPeriod","currentSequence","exampleCode","separator","isBuiltIn","status","description","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<NumberingCreate & { numberingId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  dateFormat: "yyyyMMddHH",
  resetPeriod: "None",
  isBuiltIn: 0,
  status: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}
/** resetPeriod：后端 legacy 与字典 dictValue 归一化（None|Annually|Monthly|Daily） */
const RESET_PERIOD_TO_DICT: Record<string, string> = {
  none: 'None',
  annually: 'Annually',
  year: 'Annually',
  yearly: 'Annually',
  monthly: 'Monthly',
  month: 'Monthly',
  daily: 'Daily',
  day: 'Daily',
  hour: 'Daily',
  hourly: 'Daily',
}

/** 编辑回填：归一化为 sys_reset_period dictValue */
function normalizeResetPeriodForForm(value: unknown): string {
  const fallback = String(FORM_FIELD_DEFAULTS.resetPeriod ?? 'None')
  const key = String(value ?? fallback).trim().toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? (typeof value === 'string' && value.trim() ? value.trim() : fallback)
}

/** 提交：与实体 reset_period、字典 sys_reset_period 一致 */
function normalizeResetPeriodForSubmit(value: unknown): string {
  const fallback = String(FORM_FIELD_DEFAULTS.resetPeriod ?? 'None')
  const key = String(value ?? '').trim().toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? (typeof value === 'string' && value.trim() ? value.trim() : fallback)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 numberingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.numberingId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      if ('resetPeriod' in next) next.resetPeriod = normalizeResetPeriodForForm((next as Record<string, unknown>).resetPeriod)
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.numberingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ruleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.numbering.rulecode') }),
      trigger: 'blur'
    }
  ],
  ruleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.numbering.rulename') }),
      trigger: 'blur'
    }
  ],
  documentType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.numbering.documenttype') }),
      trigger: 'change'
    }
  ],
  deptCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.numbering.deptcode') }),
      trigger: 'change'
    }
  ],
  sequenceLength: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.sequencelength') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.sequencelength') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sequenceStep: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.sequencestep') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.sequencestep') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  resetPeriod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.numbering.resetperiod') }),
      trigger: 'change'
    }
  ],
  currentSequence: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.currentsequence') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.currentsequence') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  exampleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.numbering.examplecode') }),
      trigger: 'blur'
    }
  ],
  isBuiltIn: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  status: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.numbering.status') }))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('resetPeriod' in payload) payload.resetPeriod = normalizeResetPeriodForSubmit(payload.resetPeriod)
  if ('sequenceLength' in payload) {
    const rawsequenceLength = payload.sequenceLength
    payload.sequenceLength = typeof rawsequenceLength === 'number' ? rawsequenceLength : Number(rawsequenceLength)
  }
  if ('sequenceStep' in payload) {
    const rawsequenceStep = payload.sequenceStep
    payload.sequenceStep = typeof rawsequenceStep === 'number' ? rawsequenceStep : Number(rawsequenceStep)
  }
  if ('currentSequence' in payload) {
    const rawcurrentSequence = payload.currentSequence
    payload.currentSequence = typeof rawcurrentSequence === 'number' ? rawcurrentSequence : Number(rawcurrentSequence)
  }
  if ('isBuiltIn' in payload) {
    const rawisBuiltIn = payload.isBuiltIn
    payload.isBuiltIn = typeof rawisBuiltIn === 'number' ? rawisBuiltIn : Number(rawisBuiltIn)
  }
  if ('status' in payload) {
    const rawstatus = payload.status
    payload.status = typeof rawstatus === 'number' ? rawstatus : Number(rawstatus)
  }
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.numberingId)

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

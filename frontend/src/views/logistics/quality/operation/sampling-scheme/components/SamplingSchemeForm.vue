<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/sampling-scheme/components -->
<!-- 文件名称：SamplingSchemeForm.vue -->
<!-- 功能描述：Takt抽样方案实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="sampling-scheme-form-tabs"
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
                :label="t('common.page.entity.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.code')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.code') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.samplingSchemeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.name')"
                name="samplingSchemeName"
              >
                <a-input
                  v-model:value="formState.samplingSchemeName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.name') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.type')"
                name="samplingSchemeType"
              >
                <TaktSelect
                  v-model:value="formState.samplingSchemeType"
                  dict-type="logistics_quality_sampling_scheme_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.type') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.samplingstandard')"
                name="samplingStandard"
              >
                <TaktSelect
                  v-model:value="formState.samplingStandard"
                  dict-type="logistics_quality_sampling_standard"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.samplingstandard') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.inspectionlevel')"
                name="inspectionLevel"
              >
                <TaktSelect
                  v-model:value="formState.inspectionLevel"
                  dict-type="logistics_quality_inspection_level"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionlevel') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.aqlvalue')"
                name="aqlValue"
              >
                <a-input-number
                  v-model:value="formState.aqlValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.aqlvalue') })"
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
                :label="t('entity.samplingscheme.lotsizemin')"
                name="lotSizeMin"
              >
                <a-input-number
                  v-model:value="formState.lotSizeMin"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.lotsizemin') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.lotsizemax')"
                name="lotSizeMax"
              >
                <a-input-number
                  v-model:value="formState.lotSizeMax"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.lotsizemax') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.samplesize')"
                name="sampleSize"
              >
                <a-input-number
                  v-model:value="formState.sampleSize"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.samplesize') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.acceptancenumber')"
                name="acceptanceNumber"
              >
                <a-input-number
                  v-model:value="formState.acceptanceNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.acceptancenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.rejectionnumber')"
                name="rejectionNumber"
              >
                <a-input-number
                  v-model:value="formState.rejectionNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.rejectionnumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.inspectionstrictness')"
                name="inspectionStrictness"
              >
                <TaktSelect
                  v-model:value="formState.inspectionStrictness"
                  dict-type="logistics_quality_inspection_strictness"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionstrictness') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.istransferruleenabled')"
                name="isTransferRuleEnabled"
              >
                <a-input-number
                  v-model:value="formState.isTransferRuleEnabled"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.istransferruleenabled') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.transferruleconfig')"
                name="transferRuleConfig"
              >
                <a-input
                  v-model:value="formState.transferRuleConfig"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.transferruleconfig') })"
                  show-count
                  :maxlength="2000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.samplingscheme.status')"
                name="samplingSchemeStatus"
              >
                <TaktSelect
                  v-model:value="formState.samplingSchemeStatus"
                  dict-type="logistics_quality_standard_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.status') })"
                  :disabled="loading"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.samplingscheme.schemedescription')"
                name="schemeDescription"
              >
                <a-textarea
                  v-model:value="formState.schemeDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.samplingscheme.schemedescription') })"
                  :rows="2"
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
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt抽样方案实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/sampling-scheme/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SamplingSchemeCreate } from '@/types/logistics/quality/operation/samplingScheme.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","samplingSchemeCode","samplingSchemeName","samplingSchemeType","samplingStandard","inspectionLevel","aqlValue","lotSizeMin","lotSizeMax","sampleSize","acceptanceNumber","rejectionNumber","inspectionStrictness","isTransferRuleEnabled","transferRuleConfig","samplingSchemeStatus","schemeDescription","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SamplingSchemeCreate & { samplingSchemeId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 samplingSchemeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.samplingSchemeId) {
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

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    const isCreate = !props.formData?.samplingSchemeId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.code') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.samplingscheme.name') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.type') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.type') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  samplingStandard: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.samplingstandard') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.samplingstandard') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionlevel') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionlevel') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  aqlValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.aqlvalue') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.aqlvalue') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  lotSizeMin: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.lotsizemin') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.lotsizemin') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  lotSizeMax: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.lotsizemax') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.lotsizemax') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sampleSize: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.samplesize') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.samplesize') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  acceptanceNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.acceptancenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.acceptancenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  rejectionNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.rejectionnumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.rejectionnumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionStrictness: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionstrictness') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.inspectionstrictness') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isTransferRuleEnabled: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.istransferruleenabled') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.istransferruleenabled') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  samplingSchemeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.samplingscheme.status') }))
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
  if ('samplingSchemeType' in payload) {
    const rawsamplingSchemeType = payload.samplingSchemeType
    payload.samplingSchemeType = typeof rawsamplingSchemeType === 'number' ? rawsamplingSchemeType : Number(rawsamplingSchemeType)
  }
  if ('samplingStandard' in payload) {
    const rawsamplingStandard = payload.samplingStandard
    payload.samplingStandard = typeof rawsamplingStandard === 'number' ? rawsamplingStandard : Number(rawsamplingStandard)
  }
  if ('inspectionLevel' in payload) {
    const rawinspectionLevel = payload.inspectionLevel
    payload.inspectionLevel = typeof rawinspectionLevel === 'number' ? rawinspectionLevel : Number(rawinspectionLevel)
  }
  if ('aqlValue' in payload) {
    const rawaqlValue = payload.aqlValue
    payload.aqlValue = typeof rawaqlValue === 'number' ? rawaqlValue : Number(rawaqlValue)
  }
  if ('lotSizeMin' in payload) {
    const rawlotSizeMin = payload.lotSizeMin
    payload.lotSizeMin = typeof rawlotSizeMin === 'number' ? rawlotSizeMin : Number(rawlotSizeMin)
  }
  if ('lotSizeMax' in payload) {
    const rawlotSizeMax = payload.lotSizeMax
    payload.lotSizeMax = typeof rawlotSizeMax === 'number' ? rawlotSizeMax : Number(rawlotSizeMax)
  }
  if ('sampleSize' in payload) {
    const rawsampleSize = payload.sampleSize
    payload.sampleSize = typeof rawsampleSize === 'number' ? rawsampleSize : Number(rawsampleSize)
  }
  if ('acceptanceNumber' in payload) {
    const rawacceptanceNumber = payload.acceptanceNumber
    payload.acceptanceNumber = typeof rawacceptanceNumber === 'number' ? rawacceptanceNumber : Number(rawacceptanceNumber)
  }
  if ('rejectionNumber' in payload) {
    const rawrejectionNumber = payload.rejectionNumber
    payload.rejectionNumber = typeof rawrejectionNumber === 'number' ? rawrejectionNumber : Number(rawrejectionNumber)
  }
  if ('inspectionStrictness' in payload) {
    const rawinspectionStrictness = payload.inspectionStrictness
    payload.inspectionStrictness = typeof rawinspectionStrictness === 'number' ? rawinspectionStrictness : Number(rawinspectionStrictness)
  }
  if ('isTransferRuleEnabled' in payload) {
    const rawisTransferRuleEnabled = payload.isTransferRuleEnabled
    payload.isTransferRuleEnabled = typeof rawisTransferRuleEnabled === 'number' ? rawisTransferRuleEnabled : Number(rawisTransferRuleEnabled)
  }
  if ('samplingSchemeStatus' in payload) {
    const rawsamplingSchemeStatus = payload.samplingSchemeStatus
    payload.samplingSchemeStatus = typeof rawsamplingSchemeStatus === 'number' ? rawsamplingSchemeStatus : Number(rawsamplingSchemeStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.samplingSchemeId)

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

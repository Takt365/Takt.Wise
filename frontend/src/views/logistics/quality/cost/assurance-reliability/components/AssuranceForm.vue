<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/assurance-reliability/components -->
<!-- 文件名称：AssuranceForm.vue -->
<!-- 功能描述：品质业务主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assurance-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assurance-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
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
                :label="t('entity.qualityassurance.code')"
                name="qualityAssuranceCode"
              >
                <a-input
                  v-model:value="formState.qualityAssuranceCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.code') })"
                  show-count
                  :maxlength="30"
                  allow-clear
                  :disabled="!!formData?.qualityAssuranceId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassurance.assurancemonth')"
                name="assuranceMonth"
              >
                <a-input
                  v-model:value="formState.assuranceMonth"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.assurancemonth') })"
                  show-count
                  :maxlength="7"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassurance.customername')"
                name="customerName"
              >
                <a-input
                  v-model:value="formState.customerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.customername') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityassurance.debitnoteCode')"
                name="debitNoteCode"
              >
                <a-textarea
                  v-model:value="formState.debitNoteCode"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityassurance.debitnoteCode') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassurance.recordgroupcode')"
                name="recordGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.recordGroupCode"
                  api-url="TaktCostGroups/options"
                  :api-params="{ costGroupCategory: 1 }"
                  class="w-full"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.qualityassurance.recordgroupcode') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassurance.recordgroupname')"
                name="recordGroupName"
              >
                <a-input
                  v-model:value="formState.recordGroupName"
                  disabled
                  :maxlength="80"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityassurance.totalqualitycost')"
                name="totalQualityCost"
              >
                <a-input-number
                  v-model:value="formState.totalQualityCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.totalqualitycost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityassurance.currencyCode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.currencyCode') })"
                  show-count
                  :maxlength="3"
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
        </div>
      </a-tab-pane>
    </a-tabs>
    <!-- 下：子表 reliabilityItems -->
    <TaktEditableTable
      ref="qualityAssuranceReliabilityTableRef"
      v-model="childQualityAssuranceReliabilityRows"
      :columns="qualityAssuranceReliabilityFormColumns"
      :title="t('entity.qualityassurancereliability._self')"
      :add-button-entity="t('entity.qualityassurancereliability._self')"
      id-field="qualityAssuranceReliabilityId"
      :default-row="createDefaultQualityAssuranceReliabilityRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质业务主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/assurance-reliability/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityAssuranceCreate } from '@/types/logistics/quality/cost/assurance.types'
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
  if (formFields.includes('plantCode') && (force || !target.plantCode)) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
  if (formFields.includes('relatedPlant') && (force || !target.relatedPlant)) {
    target.relatedPlant = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","qualityAssuranceCode","assuranceMonth","customerName","debitNoteCode","recordGroupCode","recordGroupName","totalQualityCost","currencyCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childQualityAssuranceReliabilityRows = ref<Record<string, unknown>[]>([])
const qualityAssuranceReliabilityTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 qualityAssuranceReliability 可编辑列 */
const qualityAssuranceReliabilityFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.qualityassurancereliability.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'testCost',
    title: t('entity.qualityassurancereliability.testcost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'workTimeMinutes',
    title: t('entity.qualityassurancereliability.worktimeminutes'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'otherExpenses',
    title: t('entity.qualityassurancereliability.otherexpenses'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'reliabilityNote',
    title: t('entity.qualityassurancereliability.reliabilitynote'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.optional', { field: t('entity.qualityassurancereliability.reliabilitynote') }),
    width: 140,
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<QualityAssuranceCreate & { qualityAssuranceId?: string }> | null | undefined) {
  childQualityAssuranceReliabilityRows.value = ((val as any)?.reliabilityItems ?? []) as Record<string, unknown>[]
}

function createDefaultQualityAssuranceReliabilityRow(): Record<string, unknown> {
  return {
    lineNumber: (childQualityAssuranceReliabilityRows.value.length + 1) * 10,
    testCost: 0,
    workTimeMinutes: 0,
    otherExpenses: 0,
    reliabilityNote: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.qualityAssuranceId ?? ''
  return {
    ...formState,
    reliabilityItems: qualityAssuranceReliabilityTableRef.value?.getRows?.() ?? childQualityAssuranceReliabilityRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      qualityAssuranceId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityAssuranceCreate & { qualityAssuranceId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityAssuranceId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityAssuranceId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).reliabilityItems
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
    const isCreate = !props.formData?.qualityAssuranceId
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
  qualityAssuranceCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.code') }),
      trigger: 'blur'
    }
  ],
  assuranceMonth: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.assurancemonth') }),
      trigger: 'blur'
    }
  ],
  totalQualityCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassurance.totalqualitycost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityassurance.totalqualitycost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityassurance.currencyCode') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await qualityAssuranceReliabilityTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalQualityCost' in payload) {
    const rawtotalQualityCost = payload.totalQualityCost
    payload.totalQualityCost = typeof rawtotalQualityCost === 'number' ? rawtotalQualityCost : Number(rawtotalQualityCost)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.qualityAssuranceId)
  childQualityAssuranceReliabilityRows.value = []
  qualityAssuranceReliabilityTableRef.value?.resetRows?.()
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/issue-pcba-rework/components -->
<!-- 文件名称：IssueForm.vue -->
<!-- 功能描述：品质问题应对主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form issue-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="issue-form-tabs"
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
                :label="t('entity.qualityissue.code')"
                name="qualityIssueCode"
              >
                <a-input
                  v-model:value="formState.qualityIssueCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.code') })"
                  show-count
                  :maxlength="30"
                  allow-clear
                  :disabled="!!formData?.qualityIssueId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissue.issuedate')"
                name="issueDate"
              >
                <a-date-picker
                  v-model:value="formState.issueDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityissue.issuedate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissue.model')"
                name="model"
              >
                <a-input
                  v-model:value="formState.model"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.model') })"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissue.lot')"
                name="lot"
              >
                <a-input
                  v-model:value="formState.lot"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.lot') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissue.qualityproblemsresponse')"
                name="qualityProblemsResponse"
              >
                <a-input
                  v-model:value="formState.qualityProblemsResponse"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.qualityproblemsresponse') })"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityissue.reworkduetodefects')"
                name="reworkDueToDefects"
              >
                <a-input
                  v-model:value="formState.reworkDueToDefects"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.reworkduetodefects') })"
                  show-count
                  :maxlength="255"
                  allow-clear
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
                :label="t('entity.qualityissue.needrework')"
                name="needRework"
              >
                <a-input
                  v-model:value="formState.needRework"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.needrework') })"
                  show-count
                  :maxlength="1"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityissue.totaltimeminutes')"
                name="totalTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalTimeMinutes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.totaltimeminutes') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityissue.totalcost')"
                name="totalCost"
              >
                <a-input-number
                  v-model:value="formState.totalCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.totalcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityissue.currencyCode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityissue.currencyCode') })"
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
    <!-- 下：子表 pcbaReworkItems -->
    <TaktEditableTable
      ref="qualityIssuePcbaReworkTableRef"
      v-model="childQualityIssuePcbaReworkRows"
      :columns="qualityIssuePcbaReworkFormColumns"
      :title="t('entity.qualityissuepcbarework._self')"
      :add-button-entity="t('entity.qualityissuepcbarework._self')"
      id-field="qualityIssuePcbaReworkId"
      :default-row="createDefaultQualityIssuePcbaReworkRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质问题应对主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/issue-pcba-rework/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIssueCreate } from '@/types/logistics/quality/cost/issue.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","qualityIssueCode","issueDate","model","lot","qualityProblemsResponse","reworkDueToDefects","needRework","totalTimeMinutes","totalCost","currencyCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childQualityIssuePcbaReworkRows = ref<Record<string, unknown>[]>([])
const qualityIssuePcbaReworkTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 qualityIssuePcbaRework 可编辑列 */
const qualityIssuePcbaReworkFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.qualityissuepcbarework.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'pcbaDefectParts',
    title: t('entity.qualityissuepcbarework.pcbadefectparts'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.qualityissuepcbarework.pcbadefectparts') }),
  },
  {
    key: 'pcbaReworkCost',
    title: t('entity.qualityissuepcbarework.pcbareworkcost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbaReworkTimeMinutes',
    title: t('entity.qualityissuepcbarework.pcbareworktimeminutes'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbaReinspectionTimeMinutes',
    title: t('entity.qualityissuepcbarework.pcbareinspectiontimeminutes'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbaTravelCost',
    title: t('entity.qualityissuepcbarework.pcbatravelcost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbaWarehouseCost',
    title: t('entity.qualityissuepcbarework.pcbawarehousecost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'pcbaOtherExpenses',
    title: t('entity.qualityissuepcbarework.pcbaotherexpenses'),
    editor: 'inputNumber',
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<QualityIssueCreate & { qualityIssueId?: string }> | null | undefined) {
  childQualityIssuePcbaReworkRows.value = ((val as any)?.pcbaReworkItems ?? []) as Record<string, unknown>[]
}

function createDefaultQualityIssuePcbaReworkRow(): Record<string, unknown> {
  return {
    lineNumber: (childQualityIssuePcbaReworkRows.value.length + 1) * 10,
    pcbaDefectParts: '',
    pcbaReworkCost: 0,
    pcbaReworkTimeMinutes: 0,
    pcbaReinspectionTimeMinutes: 0,
    pcbaTravelCost: 0,
    pcbaWarehouseCost: 0,
    pcbaOtherExpenses: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.qualityIssueId ?? ''
  return {
    ...formState,
    pcbaReworkItems: qualityIssuePcbaReworkTableRef.value?.getRows?.() ?? childQualityIssuePcbaReworkRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      qualityIssueId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIssueCreate & { qualityIssueId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIssueId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIssueId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).pcbaReworkItems
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
    const isCreate = !props.formData?.qualityIssueId
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
  qualityIssueCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityissue.code') }),
      trigger: 'blur'
    }
  ],
  issueDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityissue.issuedate') }),
      trigger: 'change'
    }
  ],
  model: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityissue.model') }),
      trigger: 'blur'
    }
  ],
  lot: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityissue.lot') }),
      trigger: 'blur'
    }
  ],
  totalTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissue.totaltimeminutes') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissue.totaltimeminutes') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissue.totalcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityissue.totalcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityissue.currencyCode') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await qualityIssuePcbaReworkTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalTimeMinutes' in payload) {
    const rawtotalTimeMinutes = payload.totalTimeMinutes
    payload.totalTimeMinutes = typeof rawtotalTimeMinutes === 'number' ? rawtotalTimeMinutes : Number(rawtotalTimeMinutes)
  }
  if ('totalCost' in payload) {
    const rawtotalCost = payload.totalCost
    payload.totalCost = typeof rawtotalCost === 'number' ? rawtotalCost : Number(rawtotalCost)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.qualityIssueId)
  childQualityIssuePcbaReworkRows.value = []
  qualityIssuePcbaReworkTableRef.value?.resetRows?.()
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

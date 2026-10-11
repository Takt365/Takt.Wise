<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/incident-item/components -->
<!-- 文件名称：IncidentForm.vue -->
<!-- 功能描述：品质事故主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form incident-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="incident-form-tabs"
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
                :label="t('entity.qualityincident.code')"
                name="qualityIncidentCode"
              >
                <a-input
                  v-model:value="formState.qualityIncidentCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.code') })"
                  show-count
                  :maxlength="30"
                  allow-clear
                  :disabled="!!formData?.qualityIncidentId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincident.incidentdate')"
                name="incidentDate"
              >
                <a-date-picker
                  v-model:value="formState.incidentDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.qualityincident.incidentdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincident.indirectmanpowercostperminute')"
                name="indirectManpowerCostPerMinute"
              >
                <a-input-number
                  v-model:value="formState.indirectManpowerCostPerMinute"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.indirectmanpowercostperminute') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincident.model')"
                name="model"
              >
                <a-input
                  v-model:value="formState.model"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.model') })"
                  show-count
                  :maxlength="255"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincident.incidentreason')"
                name="incidentReason"
              >
                <a-input
                  v-model:value="formState.incidentReason"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.incidentreason') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.qualityincident.totalscrapquantity')"
                name="totalScrapQuantity"
              >
                <a-input-number
                  v-model:value="formState.totalScrapQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.totalscrapquantity') })"
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
                :label="t('entity.qualityincident.totalscrapcost')"
                name="totalScrapCost"
              >
                <a-input-number
                  v-model:value="formState.totalScrapCost"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.totalscrapcost') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.qualityincident.currencyCode')"
                name="currencyCode"
              >
                <a-input
                  v-model:value="formState.currencyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.qualityincident.currencyCode') })"
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
    <!-- 下：子表 incidentItems -->
    <TaktEditableTable
      ref="qualityIncidentItemTableRef"
      v-model="childQualityIncidentItemRows"
      :columns="qualityIncidentItemFormColumns"
      :title="t('entity.qualityincidentitem._self')"
      :add-button-entity="t('entity.qualityincidentitem._self')"
      id-field="qualityIncidentItemId"
      :default-row="createDefaultQualityIncidentItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * 品质事故主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/cost/incident-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { QualityIncidentCreate } from '@/types/logistics/quality/cost/incident.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","qualityIncidentCode","incidentDate","indirectManpowerCostPerMinute","model","incidentReason","totalScrapQuantity","totalScrapCost","currencyCode","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childQualityIncidentItemRows = ref<Record<string, unknown>[]>([])
const qualityIncidentItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 qualityIncidentItem 可编辑列 */
const qualityIncidentItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: t('entity.qualityincidentitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'materialCode',
    title: t('entity.qualityincidentitem.materialcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialDescription',
    title: t('entity.qualityincidentitem.materialdescription'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'scrapCost',
    title: t('entity.qualityincidentitem.scrapcost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'scrapSize',
    title: t('entity.qualityincidentitem.scrapsize'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'partPrice',
    title: t('entity.qualityincidentitem.partprice'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'scrapReasonCost',
    title: t('entity.qualityincidentitem.scrapreasoncost'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'freightCharges',
    title: t('entity.qualityincidentitem.freightcharges'),
    editor: 'inputNumber',
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<QualityIncidentCreate & { qualityIncidentId?: string }> | null | undefined) {
  childQualityIncidentItemRows.value = ((val as any)?.incidentItems ?? []) as Record<string, unknown>[]
}

function createDefaultQualityIncidentItemRow(): Record<string, unknown> {
  return {
    lineNumber: (childQualityIncidentItemRows.value.length + 1) * 10,
    materialCode: '',
    materialDescription: '',
    scrapCost: 0,
    scrapSize: 0,
    partPrice: 0,
    scrapReasonCost: 0,
    freightCharges: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.qualityIncidentId ?? ''
  return {
    ...formState,
    incidentItems: qualityIncidentItemTableRef.value?.getRows?.() ?? childQualityIncidentItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      qualityIncidentId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIncidentCreate & { qualityIncidentId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIncidentId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIncidentId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).incidentItems
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
    const isCreate = !props.formData?.qualityIncidentId
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
  qualityIncidentCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityincident.code') }),
      trigger: 'blur'
    }
  ],
  incidentDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.qualityincident.incidentdate') }),
      trigger: 'change'
    }
  ],
  indirectManpowerCostPerMinute: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.indirectmanpowercostperminute') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.indirectmanpowercostperminute') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  model: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityincident.model') }),
      trigger: 'blur'
    }
  ],
  totalScrapQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.totalscrapquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.totalscrapquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalScrapCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.totalscrapcost') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.qualityincident.totalscrapcost') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  currencyCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.qualityincident.currencyCode') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await qualityIncidentItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('indirectManpowerCostPerMinute' in payload) {
    const rawindirectManpowerCostPerMinute = payload.indirectManpowerCostPerMinute
    payload.indirectManpowerCostPerMinute = typeof rawindirectManpowerCostPerMinute === 'number' ? rawindirectManpowerCostPerMinute : Number(rawindirectManpowerCostPerMinute)
  }
  if ('totalScrapQuantity' in payload) {
    const rawtotalScrapQuantity = payload.totalScrapQuantity
    payload.totalScrapQuantity = typeof rawtotalScrapQuantity === 'number' ? rawtotalScrapQuantity : Number(rawtotalScrapQuantity)
  }
  if ('totalScrapCost' in payload) {
    const rawtotalScrapCost = payload.totalScrapCost
    payload.totalScrapCost = typeof rawtotalScrapCost === 'number' ? rawtotalScrapCost : Number(rawtotalScrapCost)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.qualityIncidentId)
  childQualityIncidentItemRows.value = []
  qualityIncidentItemTableRef.value?.resetRows?.()
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order-item/components -->
<!-- 文件名称：IqcOrderItemForm.vue -->
<!-- 功能描述：IQC进货检验单明细实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form iqc-order-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="iqc-order-item-form-tabs"
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
                :label="t('entity.iqcorderitem.iqcorderid')"
                name="iqcOrderId"
              >
                <a-input
                  v-model:value="formState.iqcOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.iqcorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.iqcordercode')"
                name="iqcOrderCode"
              >
                <a-input
                  v-model:value="formState.iqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.iqcordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.iqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.materialdescription')"
                name="materialDescription"
              >
                <a-input
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialdescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.batchCode')"
                name="batchCode"
              >
                <a-input
                  v-model:value="formState.batchCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.batchCode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.purchasequantity')"
                name="purchaseQuantity"
              >
                <a-input-number
                  v-model:value="formState.purchaseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.purchasequantity') })"
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
                :label="t('entity.iqcorderitem.standardcode')"
                name="standardCode"
              >
                <a-input
                  v-model:value="formState.standardCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.standardcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.samplingschemecode')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.samplingschemecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.iqcOrderItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.inspectionmethod')"
                name="inspectionMethod"
              >
                <a-input-number
                  v-model:value="formState.inspectionMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectionmethod') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.samplequantity')"
                name="sampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.sampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.samplequantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.qualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.unqualifiedquantity')"
                name="unqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.unqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.unqualifiedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.inspectionreturnquantity')"
                name="inspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.inspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectionreturnquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.sampleserialCode')"
                name="sampleSerialCode"
              >
                <a-input
                  v-model:value="formState.sampleSerialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.sampleserialCode') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorderitem.inspectiondescription')"
                name="inspectionDescription"
              >
                <a-textarea
                  v-model:value="formState.inspectionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcorderitem.inspectiondescription') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcorderitem.inspectorby')"
                name="inspectorBy"
              >
                <a-input
                  v-model:value="formState.inspectorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectorby') })"
                  show-count
                  :maxlength="50"
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
                :label="t('entity.iqcorderitem.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcorderitem.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.judgestatus') })"
                  style="width: 100%"
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
    <!-- 下：子表 defectHandlings -->
    <TaktEditableTable
      ref="iqcDefectHandlingTableRef"
      v-model="childIqcDefectHandlingRows"
      :columns="iqcDefectHandlingFormColumns"
      :title="t('entity.iqcdefecthandling._self')"
      :add-button-entity="t('entity.iqcdefecthandling._self')"
      id-field="iqcDefectHandlingId"
      :default-row="createDefaultIqcDefectHandlingRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * IQC进货检验单明细实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/iqc-order-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IqcOrderItemCreate } from '@/types/logistics/quality/operation/iqcOrderItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","iqcOrderId","iqcOrderCode","lineNumber","materialCode","materialDescription","batchCode","purchaseQuantity","standardCode","samplingSchemeCode","inspectionMethod","sampleQuantity","qualifiedQuantity","unqualifiedQuantity","inspectionReturnQuantity","sampleSerialCode","inspectionDescription","inspectorBy","inspectionDate","judgeStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childIqcDefectHandlingRows = ref<Record<string, unknown>[]>([])
const iqcDefectHandlingTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 iqcDefectHandling 可编辑列 */
const iqcDefectHandlingFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'iqcDefectHandlingCode',
    title: t('entity.iqcdefecthandling.code'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'iqcOrderCode',
    title: t('entity.iqcdefecthandling.iqcordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'lineNumber',
    title: t('entity.iqcdefecthandling.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'defectType',
    title: t('entity.iqcdefecthandling.defecttype'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'defectCode',
    title: t('entity.iqcdefecthandling.defectcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'defectDescription',
    title: t('entity.iqcdefecthandling.defectdescription'),
    editor: 'textarea',
    rows: 1,
    placeholder: t('common.page.form.placeholder.required', { field: t('entity.iqcdefecthandling.defectdescription') }),
    width: 140,
  },
  {
    key: 'defectQuantity',
    title: t('entity.iqcdefecthandling.defectquantity'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'handlingMethod',
    title: t('entity.iqcdefecthandling.handlingmethod'),
    editor: 'inputNumber',
    width: 140,
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<IqcOrderItemCreate & { iqcOrderItemId?: string }> | null | undefined) {
  childIqcDefectHandlingRows.value = ((val as any)?.defectHandlings ?? []) as Record<string, unknown>[]
}

function createDefaultIqcDefectHandlingRow(): Record<string, unknown> {
  return {
    iqcDefectHandlingCode: '',
    iqcOrderCode: '',
    lineNumber: (childIqcDefectHandlingRows.value.length + 1) * 10,
    defectType: 0,
    defectCode: '',
    defectDescription: '',
    defectQuantity: 0,
    handlingMethod: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.iqcOrderItemId ?? ''
  return {
    ...formState,
    defectHandlings: iqcDefectHandlingTableRef.value?.getRows?.() ?? childIqcDefectHandlingRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      iqcOrderItemId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<IqcOrderItemCreate & { iqcOrderItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 iqcOrderItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.iqcOrderItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).defectHandlings
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.iqcOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  iqcOrderId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.iqcorderid') }),
      trigger: 'blur'
    }
  ],
  iqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.iqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialDescription: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.materialdescription') }),
      trigger: 'blur'
    }
  ],
  purchaseQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.purchasequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.purchasequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  standardCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.standardcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.samplingschemecode') }),
      trigger: 'blur'
    }
  ],
  inspectionMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectionmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectionmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sampleQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.samplequantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.samplequantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  qualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.qualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.qualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unqualifiedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.unqualifiedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.unqualifiedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionReturnQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectionreturnquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectionreturnquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectorBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcorderitem.inspectorby') }),
      trigger: 'blur'
    }
  ],
  inspectionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.inspectiondate') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.judgestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.iqcorderitem.judgestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await iqcDefectHandlingTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('purchaseQuantity' in payload) {
    const rawpurchaseQuantity = payload.purchaseQuantity
    payload.purchaseQuantity = typeof rawpurchaseQuantity === 'number' ? rawpurchaseQuantity : Number(rawpurchaseQuantity)
  }
  if ('inspectionMethod' in payload) {
    const rawinspectionMethod = payload.inspectionMethod
    payload.inspectionMethod = typeof rawinspectionMethod === 'number' ? rawinspectionMethod : Number(rawinspectionMethod)
  }
  if ('sampleQuantity' in payload) {
    const rawsampleQuantity = payload.sampleQuantity
    payload.sampleQuantity = typeof rawsampleQuantity === 'number' ? rawsampleQuantity : Number(rawsampleQuantity)
  }
  if ('qualifiedQuantity' in payload) {
    const rawqualifiedQuantity = payload.qualifiedQuantity
    payload.qualifiedQuantity = typeof rawqualifiedQuantity === 'number' ? rawqualifiedQuantity : Number(rawqualifiedQuantity)
  }
  if ('unqualifiedQuantity' in payload) {
    const rawunqualifiedQuantity = payload.unqualifiedQuantity
    payload.unqualifiedQuantity = typeof rawunqualifiedQuantity === 'number' ? rawunqualifiedQuantity : Number(rawunqualifiedQuantity)
  }
  if ('inspectionReturnQuantity' in payload) {
    const rawinspectionReturnQuantity = payload.inspectionReturnQuantity
    payload.inspectionReturnQuantity = typeof rawinspectionReturnQuantity === 'number' ? rawinspectionReturnQuantity : Number(rawinspectionReturnQuantity)
  }
  if ('judgeStatus' in payload) {
    const rawjudgeStatus = payload.judgeStatus
    payload.judgeStatus = typeof rawjudgeStatus === 'number' ? rawjudgeStatus : Number(rawjudgeStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.iqcOrderItemId)
  childIqcDefectHandlingRows.value = []
  iqcDefectHandlingTableRef.value?.resetRows?.()
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

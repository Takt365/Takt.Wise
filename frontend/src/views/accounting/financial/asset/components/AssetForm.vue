<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/asset/components -->
<!-- 文件名称：AssetForm.vue -->
<!-- 功能描述：资产实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="asset-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="pi.ph('plantCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetCode')"
                name="assetCode"
              >
                <a-input
                  v-model:value="formState.assetCode"
                  :placeholder="pi.ph('assetCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.assetId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetName')"
                name="assetName"
              >
                <a-input
                  v-model:value="formState.assetName"
                  :placeholder="pi.ph('assetName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetCategory')"
                name="assetCategory"
              >
                <TaktSelect
                  v-model:value="formState.assetCategory"
                  dict-type="accounting_financial_asset_category"
                  :placeholder="pi.ph('assetCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetType')"
                name="assetType"
              >
                <TaktSelect
                  v-model:value="formState.assetType"
                  dict-type="accounting_financial_asset_type"
                  :placeholder="pi.ph('assetType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetOriginalValue')"
                name="assetOriginalValue"
              >
                <a-input-number
                  v-model:value="formState.assetOriginalValue"
                  :placeholder="pi.ph('assetOriginalValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetNetValue')"
                name="assetNetValue"
              >
                <a-input-number
                  v-model:value="formState.assetNetValue"
                  :placeholder="pi.ph('assetNetValue')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accumulatedDepreciation')"
                name="accumulatedDepreciation"
              >
                <a-input-number
                  v-model:value="formState.accumulatedDepreciation"
                  :placeholder="pi.ph('accumulatedDepreciation')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCenterId')"
                name="costCenterId"
              >
                <TaktSelect
                  v-model:value="formState.costCenterId"
                  api-url="TaktCostCenters/tree-options"
                  :placeholder="pi.ph('costCenterId')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costCenterName')"
                name="costCenterName"
              >
                <a-input
                  v-model:value="formState.costCenterName"
                  :placeholder="pi.ph('costCenterName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptId')"
                name="deptId"
              >
                <TaktSelect
                  v-model:value="formState.deptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('deptId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('userId')"
                name="userId"
              >
                <TaktSelect
                  v-model:value="formState.userId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('userId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('userName')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="pi.ph('userName')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assetLocation')"
                name="assetLocation"
              >
                <a-input
                  v-model:value="formState.assetLocation"
                  :placeholder="pi.ph('assetLocation')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('purchaseDate')"
                name="purchaseDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseDate"
                  :placeholder="pi.ph('purchaseDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('startDate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="pi.ph('startDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapDate')"
                name="scrapDate"
              >
                <a-date-picker
                  v-model:value="formState.scrapDate"
                  :placeholder="pi.ph('scrapDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('disposalDate')"
                name="disposalDate"
              >
                <a-date-picker
                  v-model:value="formState.disposalDate"
                  :placeholder="pi.ph('disposalDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('expectedLifeMonths')"
                name="expectedLifeMonths"
              >
                <a-input-number
                  v-model:value="formState.expectedLifeMonths"
                  :placeholder="pi.ph('expectedLifeMonths')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('depreciationMethod')"
                name="depreciationMethod"
              >
                <TaktSelect
                  v-model:value="formState.depreciationMethod"
                  dict-type="accounting_financial_depreciation_method"
                  :placeholder="pi.ph('depreciationMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('monthlyDepreciation')"
                name="monthlyDepreciation"
              >
                <a-input-number
                  v-model:value="formState.monthlyDepreciation"
                  :placeholder="pi.ph('monthlyDepreciation')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('assetStatus')"
                name="assetStatus"
              >
                <TaktSelect
                  v-model:value="formState.assetStatus"
                  dict-type="accounting_financial_asset_status"
                  :placeholder="pi.ph('assetStatus')"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-3"
        :tab="t('common.page.form.tabs.basicinfo') + ' (4/4)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <TaktSelect
                  v-model:value="formState.companyCode"
                  api-url="TaktCompanies/options"
                  :placeholder="pi.ph('companyCode')"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
 * 资产实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/asset/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssetI18n } from '../composables/useAssetI18n'

/** 实体字段 i18n */
const pi = useAssetI18n()
import type { AssetCreate } from '@/types/accounting/financial/asset.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文（当前公司 CultureCode 注入源） */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssetCreate & { assetId?: string }> | null
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
  assetType: "NORM",
  assetStatus: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 assetId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assetId) {
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
    if (!props.formData?.assetId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  assetCode: [
    {
      required: true,
      message: pi.ph('assetCode'),
      trigger: 'blur'
    }
  ],
  assetName: [
    {
      required: true,
      message: pi.ph('assetName'),
      trigger: 'blur'
    }
  ],
  assetCategory: [
    {
      required: true,
      message: pi.ph('assetCategory'),
      trigger: 'change'
    }
  ],
  assetType: [
    {
      required: true,
      message: pi.ph('assetType'),
      trigger: 'change'
    }
  ],
  assetOriginalValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('assetOriginalValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('assetOriginalValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assetNetValue: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('assetNetValue'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('assetNetValue'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  accumulatedDepreciation: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('accumulatedDepreciation'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('accumulatedDepreciation'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  expectedLifeMonths: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('expectedLifeMonths'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('expectedLifeMonths'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  depreciationMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('depreciationMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('depreciationMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  monthlyDepreciation: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('monthlyDepreciation'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('monthlyDepreciation'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  assetStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('assetStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('assetStatus'))
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
  if ('assetOriginalValue' in payload) {
    const rawassetOriginalValue = payload.assetOriginalValue
    if (rawassetOriginalValue === undefined || rawassetOriginalValue === null || rawassetOriginalValue === '') {
      delete payload.assetOriginalValue
    } else {
      const numassetOriginalValue = typeof rawassetOriginalValue === 'number' ? rawassetOriginalValue : Number(rawassetOriginalValue)
      if (Number.isFinite(numassetOriginalValue)) payload.assetOriginalValue = numassetOriginalValue
      else delete payload.assetOriginalValue
    }
  }
  if ('assetNetValue' in payload) {
    const rawassetNetValue = payload.assetNetValue
    if (rawassetNetValue === undefined || rawassetNetValue === null || rawassetNetValue === '') {
      delete payload.assetNetValue
    } else {
      const numassetNetValue = typeof rawassetNetValue === 'number' ? rawassetNetValue : Number(rawassetNetValue)
      if (Number.isFinite(numassetNetValue)) payload.assetNetValue = numassetNetValue
      else delete payload.assetNetValue
    }
  }
  if ('accumulatedDepreciation' in payload) {
    const rawaccumulatedDepreciation = payload.accumulatedDepreciation
    if (rawaccumulatedDepreciation === undefined || rawaccumulatedDepreciation === null || rawaccumulatedDepreciation === '') {
      delete payload.accumulatedDepreciation
    } else {
      const numaccumulatedDepreciation = typeof rawaccumulatedDepreciation === 'number' ? rawaccumulatedDepreciation : Number(rawaccumulatedDepreciation)
      if (Number.isFinite(numaccumulatedDepreciation)) payload.accumulatedDepreciation = numaccumulatedDepreciation
      else delete payload.accumulatedDepreciation
    }
  }
  if ('expectedLifeMonths' in payload) {
    const rawexpectedLifeMonths = payload.expectedLifeMonths
    if (rawexpectedLifeMonths === undefined || rawexpectedLifeMonths === null || rawexpectedLifeMonths === '') {
      delete payload.expectedLifeMonths
    } else {
      const numexpectedLifeMonths = typeof rawexpectedLifeMonths === 'number' ? rawexpectedLifeMonths : Number(rawexpectedLifeMonths)
      if (Number.isFinite(numexpectedLifeMonths)) payload.expectedLifeMonths = numexpectedLifeMonths
      else delete payload.expectedLifeMonths
    }
  }
  if ('depreciationMethod' in payload) {
    const rawdepreciationMethod = payload.depreciationMethod
    if (rawdepreciationMethod === undefined || rawdepreciationMethod === null || rawdepreciationMethod === '') {
      delete payload.depreciationMethod
    } else {
      const numdepreciationMethod = typeof rawdepreciationMethod === 'number' ? rawdepreciationMethod : Number(rawdepreciationMethod)
      if (Number.isFinite(numdepreciationMethod)) payload.depreciationMethod = numdepreciationMethod
      else delete payload.depreciationMethod
    }
  }
  if ('monthlyDepreciation' in payload) {
    const rawmonthlyDepreciation = payload.monthlyDepreciation
    if (rawmonthlyDepreciation === undefined || rawmonthlyDepreciation === null || rawmonthlyDepreciation === '') {
      delete payload.monthlyDepreciation
    } else {
      const nummonthlyDepreciation = typeof rawmonthlyDepreciation === 'number' ? rawmonthlyDepreciation : Number(rawmonthlyDepreciation)
      if (Number.isFinite(nummonthlyDepreciation)) payload.monthlyDepreciation = nummonthlyDepreciation
      else delete payload.monthlyDepreciation
    }
  }
  if ('assetStatus' in payload) {
    const rawassetStatus = payload.assetStatus
    if (rawassetStatus === undefined || rawassetStatus === null || rawassetStatus === '') {
      delete payload.assetStatus
    } else {
      const numassetStatus = typeof rawassetStatus === 'number' ? rawassetStatus : Number(rawassetStatus)
      if (Number.isFinite(numassetStatus)) payload.assetStatus = numassetStatus
      else delete payload.assetStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.assetId) {
    payload.assetId = props.formData.assetId
    delete payload.numberingRuleCode
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assetId)

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

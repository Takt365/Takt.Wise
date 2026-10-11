<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/salary-item/components -->
<!-- 文件名称：SalaryItemForm.vue -->
<!-- 功能描述：薪资项目维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="salary-item-form-tabs"
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
                :label="t('entity.salaryitem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.salaryItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.shortname')"
                name="shortName"
              >
                <a-input
                  v-model:value="formState.shortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.shortname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemtype')"
                name="itemType"
              >
                <TaktSelect
                  v-model:value="formState.itemType"
                  dict-type="humanresource_compensation_salary_item_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.calcmethod')"
                name="calcMethod"
              >
                <TaktSelect
                  v-model:value="formState.calcMethod"
                  dict-type="humanresource_compensation_salary_calc_method"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.salaryformulaid')"
                name="salaryFormulaId"
              >
                <a-input
                  v-model:value="formState.salaryFormulaId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.salaryformulaid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.defaultamount')"
                name="defaultAmount"
              >
                <a-input-number
                  v-model:value="formState.defaultAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultamount') })"
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
                :label="t('entity.salaryitem.defaultrate')"
                name="defaultRate"
              >
                <a-input-number
                  v-model:value="formState.defaultRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultrate') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.strikeprice')"
                name="strikePrice"
              >
                <a-input-number
                  v-model:value="formState.strikePrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.strikeprice') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.vestingyears')"
                name="vestingYears"
              >
                <a-input-number
                  v-model:value="formState.vestingYears"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.vestingyears') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.isdeduction')"
                name="isDeduction"
              >
                <TaktSelect
                  v-model:value="formState.isDeduction"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.istaxable')"
                name="isTaxable"
              >
                <TaktSelect
                  v-model:value="formState.isTaxable"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.includesocialsecuritybase')"
                name="includeSocialSecurityBase"
              >
                <TaktSelect
                  v-model:value="formState.includeSocialSecurityBase"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.includehousingfundbase')"
                name="includeHousingFundBase"
              >
                <TaktSelect
                  v-model:value="formState.includeHousingFundBase"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemstatus')"
                name="itemStatus"
              >
                <TaktSelect
                  v-model:value="formState.itemStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.relatedplant') })"
                  show-count
                  :maxlength="4"
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
 * 薪资项目维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/salary-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SalaryItemCreate } from '@/types/human-resource/compensation/salaryItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","itemCode","itemName","shortName","itemType","calcMethod","salaryFormulaId","defaultAmount","defaultRate","strikePrice","vestingYears","isDeduction","isTaxable","includeSocialSecurityBase","includeHousingFundBase","itemStatus","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalaryItemCreate & { salaryItemId?: string }> | null
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
  itemType: 1,
  calcMethod: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 salaryItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.salaryItemId) {
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
    const isCreate = !props.formData?.salaryItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  itemCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemname') }),
      trigger: 'blur'
    }
  ],
  itemType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  calcMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defaultAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defaultRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultrate') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultrate') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  strikePrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.strikeprice') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.strikeprice') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  vestingYears: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.vestingyears') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.vestingyears') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isDeduction: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isTaxable: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  includeSocialSecurityBase: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  includeHousingFundBase: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  itemStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') }))
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
  if ('itemType' in payload) {
    const rawitemType = payload.itemType
    payload.itemType = typeof rawitemType === 'number' ? rawitemType : Number(rawitemType)
  }
  if ('calcMethod' in payload) {
    const rawcalcMethod = payload.calcMethod
    payload.calcMethod = typeof rawcalcMethod === 'number' ? rawcalcMethod : Number(rawcalcMethod)
  }
  if ('defaultAmount' in payload) {
    const rawdefaultAmount = payload.defaultAmount
    payload.defaultAmount = typeof rawdefaultAmount === 'number' ? rawdefaultAmount : Number(rawdefaultAmount)
  }
  if ('defaultRate' in payload) {
    const rawdefaultRate = payload.defaultRate
    payload.defaultRate = typeof rawdefaultRate === 'number' ? rawdefaultRate : Number(rawdefaultRate)
  }
  if ('strikePrice' in payload) {
    const rawstrikePrice = payload.strikePrice
    payload.strikePrice = typeof rawstrikePrice === 'number' ? rawstrikePrice : Number(rawstrikePrice)
  }
  if ('vestingYears' in payload) {
    const rawvestingYears = payload.vestingYears
    payload.vestingYears = typeof rawvestingYears === 'number' ? rawvestingYears : Number(rawvestingYears)
  }
  if ('isDeduction' in payload) {
    const rawisDeduction = payload.isDeduction
    payload.isDeduction = typeof rawisDeduction === 'number' ? rawisDeduction : Number(rawisDeduction)
  }
  if ('isTaxable' in payload) {
    const rawisTaxable = payload.isTaxable
    payload.isTaxable = typeof rawisTaxable === 'number' ? rawisTaxable : Number(rawisTaxable)
  }
  if ('includeSocialSecurityBase' in payload) {
    const rawincludeSocialSecurityBase = payload.includeSocialSecurityBase
    payload.includeSocialSecurityBase = typeof rawincludeSocialSecurityBase === 'number' ? rawincludeSocialSecurityBase : Number(rawincludeSocialSecurityBase)
  }
  if ('includeHousingFundBase' in payload) {
    const rawincludeHousingFundBase = payload.includeHousingFundBase
    payload.includeHousingFundBase = typeof rawincludeHousingFundBase === 'number' ? rawincludeHousingFundBase : Number(rawincludeHousingFundBase)
  }
  if ('itemStatus' in payload) {
    const rawitemStatus = payload.itemStatus
    payload.itemStatus = typeof rawitemStatus === 'number' ? rawitemStatus : Number(rawitemStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.salaryItemId)

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

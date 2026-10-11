<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/benefits/benefit-item/components -->
<!-- 文件名称：BenefitItemForm.vue -->
<!-- 功能描述：福利项目维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="benefit-item-form-tabs"
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
                :label="t('entity.benefititem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.benefitItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemname') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.benefitcategory')"
                name="benefitCategory"
              >
                <TaktSelect
                  v-model:value="formState.benefitCategory"
                  dict-type="humanresource_benefits_benefit_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefitcategory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.benefittype')"
                name="benefitType"
              >
                <TaktSelect
                  v-model:value="formState.benefitType"
                  dict-type="humanresource_benefits_benefit_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefittype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.paymentcycle')"
                name="paymentCycle"
              >
                <TaktSelect
                  v-model:value="formState.paymentCycle"
                  dict-type="humanresource_benefits_benefit_payment_cycle"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.paymentcycle') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.defaultamount')"
                name="defaultAmount"
              >
                <a-input-number
                  v-model:value="formState.defaultAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.defaultamount') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.maxamount')"
                name="maxAmount"
              >
                <a-input-number
                  v-model:value="formState.maxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.maxamount') })"
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
                :label="t('entity.benefititem.employerratio')"
                name="employerRatio"
              >
                <a-input-number
                  v-model:value="formState.employerRatio"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.employerratio') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.benefititem.employeeratio')"
                name="employeeRatio"
              >
                <a-input-number
                  v-model:value="formState.employeeRatio"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.employeeratio') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.benefititem.ismandatory')"
                name="isMandatory"
              >
                <TaktSelect
                  v-model:value="formState.isMandatory"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.ismandatory') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.benefititem.itemstatus')"
                name="itemStatus"
              >
                <TaktSelect
                  v-model:value="formState.itemStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.itemstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.benefititem.relatedplant')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.relatedplant') })"
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
 * 福利项目维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/benefits/benefit-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BenefitItemCreate } from '@/types/human-resource/benefits/benefitItem.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","itemCode","itemName","benefitCategory","benefitType","paymentCycle","defaultAmount","maxAmount","employerRatio","employeeRatio","isMandatory","itemStatus","plantCode","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BenefitItemCreate & { benefitItemId?: string }> | null
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
  benefitCategory: 1,
  benefitType: 1,
  paymentCycle: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 benefitItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.benefitItemId) {
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
    const isCreate = !props.formData?.benefitItemId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemname') }),
      trigger: 'blur'
    }
  ],
  benefitCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefitcategory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefitcategory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  benefitType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefittype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.benefittype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  paymentCycle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.paymentcycle') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.paymentcycle') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defaultAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.defaultamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.defaultamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  maxAmount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.maxamount') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.maxamount') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  employerRatio: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.employerratio') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.employerratio') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  employeeRatio: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.employeeratio') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.employeeratio') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isMandatory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.ismandatory') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.ismandatory') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  itemStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.itemstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.benefititem.itemstatus') }))
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
  if ('benefitCategory' in payload) {
    const rawbenefitCategory = payload.benefitCategory
    payload.benefitCategory = typeof rawbenefitCategory === 'number' ? rawbenefitCategory : Number(rawbenefitCategory)
  }
  if ('benefitType' in payload) {
    const rawbenefitType = payload.benefitType
    payload.benefitType = typeof rawbenefitType === 'number' ? rawbenefitType : Number(rawbenefitType)
  }
  if ('paymentCycle' in payload) {
    const rawpaymentCycle = payload.paymentCycle
    payload.paymentCycle = typeof rawpaymentCycle === 'number' ? rawpaymentCycle : Number(rawpaymentCycle)
  }
  if ('defaultAmount' in payload) {
    const rawdefaultAmount = payload.defaultAmount
    payload.defaultAmount = typeof rawdefaultAmount === 'number' ? rawdefaultAmount : Number(rawdefaultAmount)
  }
  if ('maxAmount' in payload) {
    const rawmaxAmount = payload.maxAmount
    payload.maxAmount = typeof rawmaxAmount === 'number' ? rawmaxAmount : Number(rawmaxAmount)
  }
  if ('employerRatio' in payload) {
    const rawemployerRatio = payload.employerRatio
    payload.employerRatio = typeof rawemployerRatio === 'number' ? rawemployerRatio : Number(rawemployerRatio)
  }
  if ('employeeRatio' in payload) {
    const rawemployeeRatio = payload.employeeRatio
    payload.employeeRatio = typeof rawemployeeRatio === 'number' ? rawemployeeRatio : Number(rawemployeeRatio)
  }
  if ('isMandatory' in payload) {
    const rawisMandatory = payload.isMandatory
    payload.isMandatory = typeof rawisMandatory === 'number' ? rawisMandatory : Number(rawisMandatory)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.benefitItemId)

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

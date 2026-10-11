<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title/components -->
<!-- 文件名称：AccountTitleForm.vue -->
<!-- 功能描述：会计科目实体树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="account-title-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentId')"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="TaktAccountTitles/tree-options"
                  :lazy="true"
                  :placeholder="pi.ph('parentId')"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
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
                :label="pi.label('accountTitleCode')"
                name="accountTitleCode"
              >
                <a-input
                  v-model:value="formState.accountTitleCode"
                  :placeholder="pi.ph('accountTitleCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.accountTitleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitleName')"
                name="accountTitleName"
              >
                <a-input
                  v-model:value="formState.accountTitleName"
                  :placeholder="pi.ph('accountTitleName')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitleType')"
                name="accountTitleType"
              >
                <TaktSelect
                  v-model:value="formState.accountTitleType"
                  dict-type="accounting_financial_account_title_type"
                  :placeholder="pi.ph('accountTitleType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('balanceDirection')"
                name="balanceDirection"
              >
                <a-input-number
                  v-model:value="formState.balanceDirection"
                  :placeholder="pi.ph('balanceDirection')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('accountTitleLevel')"
                name="accountTitleLevel"
              >
                <a-input-number
                  v-model:value="formState.accountTitleLevel"
                  :placeholder="pi.ph('accountTitleLevel')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isAuxiliary')"
                name="isAuxiliary"
              >
                <TaktSelect
                  v-model:value="formState.isAuxiliary"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isAuxiliary')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('auxiliaryType')"
                name="auxiliaryType"
              >
                <TaktSelect
                  v-model:value="formState.auxiliaryType"
                  dict-type="accounting_financial_auxiliary_type"
                  :placeholder="pi.ph('auxiliaryType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isQuantity')"
                name="isQuantity"
              >
                <TaktSelect
                  v-model:value="formState.isQuantity"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isQuantity')"
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
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isCurrency')"
                name="isCurrency"
              >
                <TaktSelect
                  v-model:value="formState.isCurrency"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isCurrency')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isCash')"
                name="isCash"
              >
                <TaktSelect
                  v-model:value="formState.isCash"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isCash')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isBank')"
                name="isBank"
              >
                <TaktSelect
                  v-model:value="formState.isBank"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isBank')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('validFrom')"
                name="validFrom"
              >
                <a-date-picker
                  v-model:value="formState.validFrom"
                  :placeholder="pi.ph('validFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('validTo')"
                name="validTo"
              >
                <a-date-picker
                  v-model:value="formState.validTo"
                  :placeholder="pi.ph('validTo')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('accountTitleStatus')"
                name="accountTitleStatus"
              >
                <TaktSelect
                  v-model:value="formState.accountTitleStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('accountTitleStatus')"
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
 * 会计科目实体维护表单 · 由 generate-vue-tree-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/account-title/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAccountTitleI18n } from '../composables/useAccountTitleI18n'

/** 实体字段 i18n */
const pi = useAccountTitleI18n()

import type { AccountTitleCreate } from '@/types/accounting/financial/accountTitle.types'
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
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","accountTitleCode","accountTitleName","accountTitleType","balanceDirection","accountTitleLevel","isAuxiliary","auxiliaryType","isQuantity","isCurrency","isCash","isBank","validFrom","validTo","accountTitleStatus","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AccountTitleCreate & { accountTitleId?: string }> | null
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
const formState = reactive<Record<string, any>>({ parentId: '0' })
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  auxiliaryType: "S",
  accountTitleStatus: 1
}

/** 树表 parentId：空值归一为根节点 0（string，与后端 ParentId=0 一致） */
function normalizeTreeParentId(target: Record<string, unknown>) {
  const raw = target.parentId
  target.parentId = raw === '' || raw === undefined || raw === null ? '0' : String(raw)
}
/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 accountTitleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.accountTitleId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      normalizeTreeParentId(formState)
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
    const isCreate = !props.formData?.accountTitleId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  parentId: [
    {
      required: true,
      message: pi.ph('parentId'),
      trigger: 'change'
    }
  ],
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  accountTitleCode: [
    {
      required: true,
      message: pi.ph('accountTitleCode'),
      trigger: 'blur'
    }
  ],
  accountTitleName: [
    {
      required: true,
      message: pi.ph('accountTitleName'),
      trigger: 'blur'
    }
  ],
  accountTitleType: [
    {
      required: true,
      message: pi.ph('accountTitleType'),
      trigger: 'change'
    }
  ],
  balanceDirection: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('balanceDirection'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('balanceDirection'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  accountTitleLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('accountTitleLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('accountTitleLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isAuxiliary: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isAuxiliary'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isAuxiliary'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  auxiliaryType: [
    {
      required: true,
      message: pi.ph('auxiliaryType'),
      trigger: 'change'
    }
  ],
  isQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCurrency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCurrency'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCurrency'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCash: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCash'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCash'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBank: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBank'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBank'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  validFrom: [
    {
      required: true,
      message: pi.ph('validFrom'),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: pi.ph('validTo'),
      trigger: 'change'
    }
  ],
  accountTitleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('accountTitleStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('accountTitleStatus'))
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
  if ('balanceDirection' in payload) {
    const rawbalanceDirection = payload.balanceDirection
    payload.balanceDirection = typeof rawbalanceDirection === 'number' ? rawbalanceDirection : Number(rawbalanceDirection)
  }
  if ('accountTitleLevel' in payload) {
    const rawaccountTitleLevel = payload.accountTitleLevel
    payload.accountTitleLevel = typeof rawaccountTitleLevel === 'number' ? rawaccountTitleLevel : Number(rawaccountTitleLevel)
  }
  if ('isAuxiliary' in payload) {
    const rawisAuxiliary = payload.isAuxiliary
    payload.isAuxiliary = typeof rawisAuxiliary === 'number' ? rawisAuxiliary : Number(rawisAuxiliary)
  }
  if ('isQuantity' in payload) {
    const rawisQuantity = payload.isQuantity
    payload.isQuantity = typeof rawisQuantity === 'number' ? rawisQuantity : Number(rawisQuantity)
  }
  if ('isCurrency' in payload) {
    const rawisCurrency = payload.isCurrency
    payload.isCurrency = typeof rawisCurrency === 'number' ? rawisCurrency : Number(rawisCurrency)
  }
  if ('isCash' in payload) {
    const rawisCash = payload.isCash
    payload.isCash = typeof rawisCash === 'number' ? rawisCash : Number(rawisCash)
  }
  if ('isBank' in payload) {
    const rawisBank = payload.isBank
    payload.isBank = typeof rawisBank === 'number' ? rawisBank : Number(rawisBank)
  }
  if ('accountTitleStatus' in payload) {
    const rawaccountTitleStatus = payload.accountTitleStatus
    payload.accountTitleStatus = typeof rawaccountTitleStatus === 'number' ? rawaccountTitleStatus : Number(rawaccountTitleStatus)
  }
  const parentRaw = payload.parentId
  const parentId = parentRaw === '' || parentRaw === undefined || parentRaw === null ? '0' : String(parentRaw)
  payload.parentId = parentId
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行 */
/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.accountTitleId)

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

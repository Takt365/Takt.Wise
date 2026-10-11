<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/bank/components -->
<!-- 文件名称：BankForm.vue -->
<!-- 功能描述：银行信息实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="bank-form-tabs"
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
                :label="pi.label('countryRegion')"
                name="countryRegion"
              >
                <TaktSelect
                  v-model:value="formState.countryRegion"
                  dict-type="sys_country_code"
                  :placeholder="pi.ph('countryRegion')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankCode')"
                name="bankCode"
              >
                <a-input
                  v-model:value="formState.bankCode"
                  :placeholder="pi.ph('bankCode')"
                  show-count
                  :maxlength="15"
                  allow-clear
                  :disabled="!!formData?.bankId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankName1')"
                name="bankName1"
              >
                <a-input
                  v-model:value="formState.bankName1"
                  :placeholder="pi.ph('bankName1')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankName2')"
                name="bankName2"
              >
                <a-input
                  v-model:value="formState.bankName2"
                  :placeholder="pi.ph('bankName2')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('province')"
                name="province"
              >
                <TaktSelect
                  v-model:value="formState.province"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('province')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prefecture')"
                name="prefecture"
              >
                <TaktSelect
                  v-model:value="formState.prefecture"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('prefecture')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('district')"
                name="district"
              >
                <TaktSelect
                  v-model:value="formState.district"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('district')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('township')"
                name="township"
              >
                <TaktSelect
                  v-model:value="formState.township"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('township')"
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
                :label="pi.label('village')"
                name="village"
              >
                <TaktSelect
                  v-model:value="formState.village"
                  api-url="TaktAdminDivisions/options"
                  :placeholder="pi.ph('village')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address1')"
                name="address1"
              >
                <a-textarea
                  v-model:value="formState.address1"
                  :placeholder="pi.ph('address1')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('address2')"
                name="address2"
              >
                <a-textarea
                  v-model:value="formState.address2"
                  :placeholder="pi.ph('address2')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('swiftBic')"
                name="swiftBic"
              >
                <a-input
                  v-model:value="formState.swiftBic"
                  :placeholder="pi.ph('swiftBic')"
                  show-count
                  :maxlength="11"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankGroup')"
                name="bankGroup"
              >
                <a-input
                  v-model:value="formState.bankGroup"
                  :placeholder="pi.ph('bankGroup')"
                  show-count
                  :maxlength="2"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pobkCurAc')"
                name="pobkCurAc"
              >
                <TaktSelect
                  v-model:value="formState.pobkCurAc"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('pobkCurAc')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bankNumber')"
                name="bankNumber"
              >
                <a-input
                  v-model:value="formState.bankNumber"
                  :placeholder="pi.ph('bankNumber')"
                  show-count
                  :maxlength="15"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('postalBank')"
                name="postalBank"
              >
                <a-input
                  v-model:value="formState.postalBank"
                  :placeholder="pi.ph('postalBank')"
                  show-count
                  :maxlength="16"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('addressNumber')"
                name="addressNumber"
              >
                <a-textarea
                  v-model:value="formState.addressNumber"
                  :placeholder="pi.ph('addressNumber')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('branch')"
                name="branch"
              >
                <a-input
                  v-model:value="formState.branch"
                  :placeholder="pi.ph('branch')"
                  show-count
                  :maxlength="40"
                  allow-clear
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
                :label="pi.label('bankMethod')"
                name="bankMethod"
              >
                <a-input
                  v-model:value="formState.bankMethod"
                  :placeholder="pi.ph('bankMethod')"
                  show-count
                  :maxlength="4"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('bankFormat')"
                name="bankFormat"
              >
                <a-input
                  v-model:value="formState.bankFormat"
                  :placeholder="pi.ph('bankFormat')"
                  show-count
                  :maxlength="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ibanRule')"
                name="ibanRule"
              >
                <a-input
                  v-model:value="formState.ibanRule"
                  :placeholder="pi.ph('ibanRule')"
                  show-count
                  :maxlength="6"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sddB2b')"
                name="sddB2b"
              >
                <TaktSelect
                  v-model:value="formState.sddB2b"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('sddB2b')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sddCore')"
                name="sddCore"
              >
                <TaktSelect
                  v-model:value="formState.sddCore"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('sddCore')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('sddRtrans')"
                name="sddRtrans"
              >
                <TaktSelect
                  v-model:value="formState.sddRtrans"
                  dict-type="accounting_financial_sepa_rtrans_type"
                  :placeholder="pi.ph('sddRtrans')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('bicPlusNumber')"
                name="bicPlusNumber"
              >
                <a-input
                  v-model:value="formState.bicPlusNumber"
                  :placeholder="pi.ph('bicPlusNumber')"
                  show-count
                  :maxlength="12"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('pathCode')"
                name="pathCode"
              >
                <a-input
                  v-model:value="formState.pathCode"
                  :placeholder="pi.ph('pathCode')"
                  show-count
                  :maxlength="15"
                  allow-clear
                  :disabled="!!formData?.bankId"
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
 * 银行信息实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/bank/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBankI18n } from '../composables/useBankI18n'

/** 实体字段 i18n */
const pi = useBankI18n()
import type { BankCreate } from '@/types/accounting/financial/bank.types'
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
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BankCreate & { bankId?: string }> | null
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
  countryRegion: "CN",
  sddRtrans: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 bankId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.bankId) {
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
    if (!props.formData?.bankId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  countryRegion: [
    {
      required: true,
      message: pi.ph('countryRegion'),
      trigger: 'change'
    }
  ],
  bankCode: [
    {
      required: true,
      message: pi.ph('bankCode'),
      trigger: 'blur'
    }
  ],
  bankName1: [
    {
      required: true,
      message: pi.ph('bankName1'),
      trigger: 'blur'
    }
  ],
  pobkCurAc: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('pobkCurAc'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('pobkCurAc'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sddB2b: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sddB2b'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sddB2b'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sddCore: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sddCore'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sddCore'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sddRtrans: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sddRtrans'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sddRtrans'))
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
  if ('pobkCurAc' in payload) {
    const rawpobkCurAc = payload.pobkCurAc
    payload.pobkCurAc = typeof rawpobkCurAc === 'number' ? rawpobkCurAc : Number(rawpobkCurAc)
  }
  if ('sddB2b' in payload) {
    const rawsddB2b = payload.sddB2b
    payload.sddB2b = typeof rawsddB2b === 'number' ? rawsddB2b : Number(rawsddB2b)
  }
  if ('sddCore' in payload) {
    const rawsddCore = payload.sddCore
    payload.sddCore = typeof rawsddCore === 'number' ? rawsddCore : Number(rawsddCore)
  }
  if ('sddRtrans' in payload) {
    const rawsddRtrans = payload.sddRtrans
    payload.sddRtrans = typeof rawsddRtrans === 'number' ? rawsddRtrans : Number(rawsddRtrans)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.bankId)

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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/identity/tenant/components -->
<!-- 文件名称：TenantForm.vue -->
<!-- 功能描述：租户实体 代表系统中的独立租户维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="tenant-form-tabs"
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
                :label="t('common.page.entity.tenantcode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
                  show-count
                  :maxlength="3"
                  allow-clear
                  :disabled="!!formData?.tenantId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.name')"
                name="tenantName"
              >
                <a-input
                  v-model:value="formState.tenantName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.name') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.subscriptionstarttime')"
                name="subscriptionStartTime"
              >
                <a-date-picker
                  v-model:value="formState.subscriptionStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionstarttime') })"
                  value-format="YYYY-MM-DD HH:mm:ss"
                  show-time
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.subscriptionendtime')"
                name="subscriptionEndTime"
              >
                <a-date-picker
                  v-model:value="formState.subscriptionEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionendtime') })"
                  value-format="YYYY-MM-DD HH:mm:ss"
                  show-time
                  style="width: 100%"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactname')"
                name="contactName"
              >
                <a-input
                  v-model:value="formState.contactName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.tenant.contactname') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactphone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.tenant.contactphone') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.contactemail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.tenant.contactemail') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.isbuiltin')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.isbuiltin') })"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.tenant.status')"
                name="tenantStatus"
              >
                <TaktSelect
                  v-model:value="formState.tenantStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.tenant.status') })"
                  disabled
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
                name="ExtField"
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
                  v-model:value="formState.ExtField"
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
 * 租户实体 代表系统中的独立租户维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/identity/tenant/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { TenantCreate } from '@/types/identity/tenant.types'
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
function applyScopeDefaults(target: Record<string, unknown>, _force = false) {
  if (formFields.includes('companyCode') && !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'tenantName', 'subscriptionStartTime', 'subscriptionEndTime', 'contactName', 'contactPhone', 'contactEmail', 'isBuiltIn', 'tenantStatus', 'ExtField', 'remark']

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TenantCreate & { tenantId?: string }> | null
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
  isBuiltIn: 0,
  tenantStatus: 1,
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

/** 字典整型字段校验器 */
function dictIntRule(fieldLabelKey: string): Rule {
  return {
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldLabelKey) }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t(fieldLabelKey) }))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 tenantId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.tenantId) {
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
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段（租户实体无登录上下文 tenantCode 注入） */
watch(
  () => [tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.tenantId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  tenantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') }),
      trigger: 'blur',
    }],
  tenantName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.name') }),
      trigger: 'blur',
    }],
  subscriptionStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionstarttime') }),
      trigger: 'change',
    }],
  subscriptionEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.tenant.subscriptionendtime') }),
      trigger: 'change',
    }],
  contactEmail: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.tenant.contactemail') }),
      trigger: 'blur',
    }],
  isBuiltIn: [dictIntRule('entity.tenant.isbuiltin')],
  tenantStatus: [dictIntRule('entity.tenant.status')],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  for (const key of ['isBuiltIn', 'tenantStatus'] as const) {
    if (key in payload) {
      const raw = payload[key]
      payload[key] = typeof raw === 'number' ? raw : Number(raw)
    }
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
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

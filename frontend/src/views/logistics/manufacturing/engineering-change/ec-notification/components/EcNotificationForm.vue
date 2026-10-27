<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-notification/components -->
<!-- 文件名称：EcNotificationForm.vue -->
<!-- 功能描述：工程变更通知单维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
    <TaktFormTabs
      v-model:active-key="activeTab"
      class="ec-notification-form-tabs"
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  disabled
                  :placeholder="pi.ph('tenantCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  disabled
                  :placeholder="pi.ph('companyCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  disabled
                  :placeholder="pi.ph('cultureCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNotificationCode')"
                name="ecNotificationCode"
              >
                <a-input
                  v-model:value="formState.ecNotificationCode"
                  :placeholder="pi.ph('ecNotificationCode')"
                  show-count
                  :maxlength="30"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecTeId')"
                name="ecTeId"
              >
                <a-input
                  v-model:value="formState.ecTeId"
                  :placeholder="pi.ph('ecTeId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecCode')"
                name="ecCode"
              >
                <a-input
                  v-model:value="formState.ecCode"
                  :placeholder="pi.ph('ecCode')"
                  show-count
                  :maxlength="30"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecTitle')"
                name="ecTitle"
              >
                <a-input
                  v-model:value="formState.ecTitle"
                  :placeholder="pi.ph('ecTitle')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNotificationDate')"
                name="ecNotificationDate"
              >
                <a-date-picker
                  v-model:value="formState.ecNotificationDate"
                  :placeholder="pi.ph('ecNotificationDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNotificationDeptCodes')"
                name="ecNotificationDeptCodes"
              >
                <TaktSelect
                  v-model:value="deptGroupCodes"
                  api-url="TaktDepts/options"
                  multiple
                  :placeholder="pi.ph('ecNotificationDeptCodes')"
                  :disabled="loading"
                  allow-clear
                  @change="onDeptGroupsChange"
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
                :label="pi.label('ecNotificationDeptNames')"
                name="ecNotificationDeptNames"
              >
                <a-input
                  v-model:value="formState.ecNotificationDeptNames"
                  :placeholder="pi.ph('ecNotificationDeptNames')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecNotificationNotifierGroupCode')"
                name="ecNotificationNotifierGroupCode"
              >
                <TaktSelect
                  v-model:value="notifierGroupCodes"
                  api-url="TaktEcGroups/options"
                  multiple
                  :api-params="notifierGroupApiParams"
                  :placeholder="pi.ph('ecNotificationNotifierGroupCode')"
                  :disabled="loading || deptGroupCodes.length === 0"
                  allow-clear
                  @change="onNotifierGroupsChange"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecNotificationNotifierGroupName')"
                name="ecNotificationNotifierGroupName"
              >
                <a-input
                  v-model:value="formState.ecNotificationNotifierGroupName"
                  :placeholder="pi.ph('ecNotificationNotifierGroupName')"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecNotificationMethod')"
                name="ecNotificationMethod"
              >
                <a-input-number
                  v-model:value="formState.ecNotificationMethod"
                  :placeholder="pi.ph('ecNotificationMethod')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ecNotificationStatus')"
                name="ecNotificationStatus"
              >
                <a-input-number
                  v-model:value="formState.ecNotificationStatus"
                  :placeholder="pi.ph('ecNotificationStatus')"
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
    </TaktFormTabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 工程变更通知单维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/engineering-change/ec-notification/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcNotificationCreate } from '@/types/logistics/manufacturing/engineering-change/ecNotification.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

import { useEcNotificationI18n } from '../composables/useEcNotificationI18n'

/** 实体字段 i18n（标签/占位符传小驼峰字段名） */
const pi = useEcNotificationI18n()
/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 区域文化 / 工厂（登录或公司切换注入，表单只读）
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
/** 已选部门编码 */
const deptGroupCodes = ref<string[]>([])
/** 已选通知组编码（仅已选部门下的设变组） */
const notifierGroupCodes = ref<string[]>([])
/** 通知组选项查询：按已选部门编码过滤设变组类别 */
const notifierGroupApiParams = computed(() => ({
  ecGroupCategory: deptGroupCodes.value.join(','),
}))

/**
 * 逗号串拆成编码数组
 * @param raw 表单或接口中的编码串
 * @returns 编码列表
 */
function splitCodes(raw: unknown): string[] {
  if (Array.isArray(raw)) {
    return raw.map((item) => String(item).trim()).filter((item) => item.length > 0)
  }
  return String(raw ?? '')
    .split(',')
    .map((item) => item.trim())
    .filter((item) => item.length > 0)
}

/**
 * 多选选项的显示名按选择顺序拼接
 * @param option TaktSelect change 的选项
 * @returns 逗号分隔名称
 */
function joinOptionLabels(option: unknown): string {
  const list = Array.isArray(option) ? option : option ? [option] : []
  return list
    .map((item) => {
      const row = item as { label?: unknown }
      return typeof row?.label === 'string' ? row.label.trim() : ''
    })
    .filter((item) => item.length > 0)
    .join(',')
}

/**
 * 部门组变更后回填名称，并清空不再属于这些部门组的通知组
 * @param _value 已选编码
 * @param option 已选选项
 */
function onDeptGroupsChange(_value: unknown, option: unknown) {
  formState.ecNotificationDeptNames = joinOptionLabels(option)
  notifierGroupCodes.value = []
  formState.ecNotificationNotifierGroupCode = ''
  formState.ecNotificationNotifierGroupName = ''
}

/**
 * 通知组变更后回填名称
 * @param _value 已选编码
 * @param option 已选选项
 */
function onNotifierGroupsChange(_value: unknown, option: unknown) {
  formState.ecNotificationNotifierGroupName = joinOptionLabels(option)
}

watch(deptGroupCodes, (codes) => {
  formState.ecNotificationDeptCodes = codes.join(',')
})
watch(notifierGroupCodes, (codes) => {
  formState.ecNotificationNotifierGroupCode = codes.join(',')
})

/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","ecNotificationCode","ecTeId","ecCode","ecTitle","ecNotificationDate","ecNotificationDeptCodes","ecNotificationDeptNames","ecNotificationNotifierGroupCode","ecNotificationNotifierGroupName","ecNotificationMethod","ecNotificationStatus","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcNotificationCreate & { ecNotificationId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecNotificationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecNotificationId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      deptGroupCodes.value = splitCodes(next.ecNotificationDeptCodes)
      notifierGroupCodes.value = splitCodes(next.ecNotificationNotifierGroupCode)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      applyScopeDefaults(formState as Record<string, unknown>, true)
      deptGroupCodes.value = splitCodes(formState.ecNotificationDeptCodes)
      notifierGroupCodes.value = splitCodes(formState.ecNotificationNotifierGroupCode)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    const isCreate = !props.formData?.ecNotificationId
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
      message: pi.ph('plantCode'),
      trigger: 'blur'
    }
  ],
  ecNotificationCode: [
    {
      required: true,
      message: pi.ph('ecNotificationCode'),
      trigger: 'blur'
    }
  ],
  ecTeId: [
    {
      required: true,
      message: pi.ph('ecTeId'),
      trigger: 'blur'
    }
  ],
  ecCode: [
    {
      required: true,
      message: pi.ph('ecCode'),
      trigger: 'blur'
    }
  ],
  ecNotificationDate: [
    {
      required: true,
      message: pi.ph('ecNotificationDate'),
      trigger: 'change'
    }
  ],
  ecNotificationMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ecNotificationMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ecNotificationMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecNotificationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ecNotificationStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ecNotificationStatus'))
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
  if ('ecNotificationMethod' in payload) {
    const rawecNotificationMethod = payload.ecNotificationMethod
    payload.ecNotificationMethod = typeof rawecNotificationMethod === 'number' ? rawecNotificationMethod : Number(rawecNotificationMethod)
  }
  if ('ecNotificationStatus' in payload) {
    const rawecNotificationStatus = payload.ecNotificationStatus
    payload.ecNotificationStatus = typeof rawecNotificationStatus === 'number' ? rawecNotificationStatus : Number(rawecNotificationStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecNotificationId)
  deptGroupCodes.value = splitCodes(formState.ecNotificationDeptCodes)
  notifierGroupCodes.value = splitCodes(formState.ecNotificationNotifierGroupCode)

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

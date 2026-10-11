<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/customer-service/request/components -->
<!-- 文件名称：RequestForm.vue -->
<!-- 功能描述：服务请求实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="request-form-tabs"
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
                :label="pi.label('serviceRequestCode')"
                name="serviceRequestCode"
              >
                <a-input
                  v-model:value="formState.serviceRequestCode"
                  :placeholder="pi.ph('serviceRequestCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerServiceRequestId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientId')"
                name="clientId"
              >
                <a-input
                  v-model:value="formState.clientId"
                  :placeholder="pi.ph('clientId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientCode')"
                name="clientCode"
              >
                <a-input
                  v-model:value="formState.clientCode"
                  :placeholder="pi.ph('clientCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('clientName1')"
                name="clientName1"
              >
                <a-input
                  v-model:value="formState.clientName1"
                  :placeholder="pi.ph('clientName1')"
                  show-count
                  :maxlength="140"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceContractId')"
                name="serviceContractId"
              >
                <a-input
                  v-model:value="formState.serviceContractId"
                  :placeholder="pi.ph('serviceContractId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceContractCode')"
                name="serviceContractCode"
              >
                <a-input
                  v-model:value="formState.serviceContractCode"
                  :placeholder="pi.ph('serviceContractCode')"
                  show-count
                  :maxlength="50"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestDate')"
                name="requestDate"
              >
                <a-date-picker
                  v-model:value="formState.requestDate"
                  :placeholder="pi.ph('requestDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('expectedServiceDate')"
                name="expectedServiceDate"
              >
                <a-date-picker
                  v-model:value="formState.expectedServiceDate"
                  :placeholder="pi.ph('expectedServiceDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
                :label="pi.label('requestType')"
                name="requestType"
              >
                <a-input-number
                  v-model:value="formState.requestType"
                  :placeholder="pi.ph('requestType')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('sourceChannel')"
                name="sourceChannel"
              >
                <a-input-number
                  v-model:value="formState.sourceChannel"
                  :placeholder="pi.ph('sourceChannel')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('priority')"
                name="priority"
              >
                <TaktSelect
                  v-model:value="formState.priority"
                  dict-type="sys_priority_level"
                  :placeholder="pi.ph('priority')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestStatus')"
                name="requestStatus"
              >
                <a-input-number
                  v-model:value="formState.requestStatus"
                  :placeholder="pi.ph('requestStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requestSubject')"
                name="requestSubject"
              >
                <a-input
                  v-model:value="formState.requestSubject"
                  :placeholder="pi.ph('requestSubject')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('requestDescription')"
                name="requestDescription"
              >
                <a-textarea
                  v-model:value="formState.requestDescription"
                  :placeholder="pi.ph('requestDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPerson')"
                name="contactPerson"
              >
                <a-input
                  v-model:value="formState.contactPerson"
                  :placeholder="pi.ph('contactPerson')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactPhone')"
                name="contactPhone"
              >
                <a-input
                  v-model:value="formState.contactPhone"
                  :placeholder="pi.ph('contactPhone')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('contactEmail')"
                name="contactEmail"
              >
                <a-input
                  v-model:value="formState.contactEmail"
                  :placeholder="pi.ph('contactEmail')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('serviceAddress')"
                name="serviceAddress"
              >
                <a-textarea
                  v-model:value="formState.serviceAddress"
                  :placeholder="pi.ph('serviceAddress')"
                  :rows="2"
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
                :label="pi.label('assignedEmployeeId')"
                name="assignedEmployeeId"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeId"
                  :placeholder="pi.ph('assignedEmployeeId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('assignedEmployeeName')"
                name="assignedEmployeeName"
              >
                <a-input
                  v-model:value="formState.assignedEmployeeName"
                  :placeholder="pi.ph('assignedEmployeeName')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('assignedAt')"
                name="assignedAt"
              >
                <a-date-picker
                  v-model:value="formState.assignedAt"
                  :placeholder="pi.ph('assignedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('closedAt')"
                name="closedAt"
              >
                <a-date-picker
                  v-model:value="formState.closedAt"
                  :placeholder="pi.ph('closedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
 * 服务请求实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/customer-service/request/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerServiceRequestI18n } from '../composables/useRequestI18n'

/** 实体字段 i18n */
const pi = useCustomerServiceRequestI18n()
import type { CustomerServiceRequestCreate } from '@/types/logistics/customer-service/request.types'
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
  formData?: Partial<CustomerServiceRequestCreate & { customerServiceRequestId?: string }> | null
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
  priority: 3
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerServiceRequestId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerServiceRequestId) {
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
    if (!props.formData?.customerServiceRequestId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  serviceRequestCode: [
    {
      required: true,
      message: pi.ph('serviceRequestCode'),
      trigger: 'blur'
    }
  ],
  clientId: [
    {
      required: true,
      message: pi.ph('clientId'),
      trigger: 'blur'
    }
  ],
  requestDate: [
    {
      required: true,
      message: pi.ph('requestDate'),
      trigger: 'change'
    }
  ],
  requestType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  sourceChannel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('sourceChannel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('sourceChannel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('priority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('priority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requestStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requestStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requestSubject: [
    {
      required: true,
      message: pi.ph('requestSubject'),
      trigger: 'blur'
    }
  ],
  requestDescription: [
    {
      required: true,
      message: pi.ph('requestDescription'),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('requestType' in payload) {
    const rawrequestType = payload.requestType
    if (rawrequestType === undefined || rawrequestType === null || rawrequestType === '') {
      delete payload.requestType
    } else {
      const numrequestType = typeof rawrequestType === 'number' ? rawrequestType : Number(rawrequestType)
      if (Number.isFinite(numrequestType)) payload.requestType = numrequestType
      else delete payload.requestType
    }
  }
  if ('sourceChannel' in payload) {
    const rawsourceChannel = payload.sourceChannel
    if (rawsourceChannel === undefined || rawsourceChannel === null || rawsourceChannel === '') {
      delete payload.sourceChannel
    } else {
      const numsourceChannel = typeof rawsourceChannel === 'number' ? rawsourceChannel : Number(rawsourceChannel)
      if (Number.isFinite(numsourceChannel)) payload.sourceChannel = numsourceChannel
      else delete payload.sourceChannel
    }
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    if (rawpriority === undefined || rawpriority === null || rawpriority === '') {
      delete payload.priority
    } else {
      const numpriority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
      if (Number.isFinite(numpriority)) payload.priority = numpriority
      else delete payload.priority
    }
  }
  if ('requestStatus' in payload) {
    const rawrequestStatus = payload.requestStatus
    if (rawrequestStatus === undefined || rawrequestStatus === null || rawrequestStatus === '') {
      delete payload.requestStatus
    } else {
      const numrequestStatus = typeof rawrequestStatus === 'number' ? rawrequestStatus : Number(rawrequestStatus)
      if (Number.isFinite(numrequestStatus)) payload.requestStatus = numrequestStatus
      else delete payload.requestStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.customerServiceRequestId) {
    payload.customerServiceRequestId = props.formData.customerServiceRequestId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerServiceRequestId)

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

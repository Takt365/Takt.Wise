<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/customer-service/ticket/components -->
<!-- 文件名称：TicketForm.vue -->
<!-- 功能描述：服务工单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ticket-form-tabs"
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
                :label="pi.label('serviceTicketCode')"
                name="serviceTicketCode"
              >
                <a-input
                  v-model:value="formState.serviceTicketCode"
                  :placeholder="pi.ph('serviceTicketCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.customerServiceTicketId"
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
                :label="pi.label('serviceRequestId')"
                name="serviceRequestId"
              >
                <a-input
                  v-model:value="formState.serviceRequestId"
                  :placeholder="pi.ph('serviceRequestId')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceOrderId')"
                name="serviceOrderId"
              >
                <a-input
                  v-model:value="formState.serviceOrderId"
                  :placeholder="pi.ph('serviceOrderId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceOrderCode')"
                name="serviceOrderCode"
              >
                <a-input
                  v-model:value="formState.serviceOrderCode"
                  :placeholder="pi.ph('serviceOrderCode')"
                  show-count
                  :maxlength="50"
                  disabled
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
                :label="pi.label('ticketType')"
                name="ticketType"
              >
                <a-input-number
                  v-model:value="formState.ticketType"
                  :placeholder="pi.ph('ticketType')"
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
                :label="pi.label('ticketStatus')"
                name="ticketStatus"
              >
                <TaktSelect
                  v-model:value="formState.ticketStatus"
                  dict-type="sys_ticket_status"
                  :placeholder="pi.ph('ticketStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ticketSubject')"
                name="ticketSubject"
              >
                <a-input
                  v-model:value="formState.ticketSubject"
                  :placeholder="pi.ph('ticketSubject')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('faultDescription')"
                name="faultDescription"
              >
                <a-textarea
                  v-model:value="formState.faultDescription"
                  :placeholder="pi.ph('faultDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('solutionDescription')"
                name="solutionDescription"
              >
                <a-textarea
                  v-model:value="formState.solutionDescription"
                  :placeholder="pi.ph('solutionDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serviceLocation')"
                name="serviceLocation"
              >
                <a-input
                  v-model:value="formState.serviceLocation"
                  :placeholder="pi.ph('serviceLocation')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
                :label="pi.label('scheduledStartTime')"
                name="scheduledStartTime"
              >
                <a-date-picker
                  v-model:value="formState.scheduledStartTime"
                  :placeholder="pi.ph('scheduledStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('scheduledEndTime')"
                name="scheduledEndTime"
              >
                <a-date-picker
                  v-model:value="formState.scheduledEndTime"
                  :placeholder="pi.ph('scheduledEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('actualStartTime')"
                name="actualStartTime"
              >
                <a-date-picker
                  v-model:value="formState.actualStartTime"
                  :placeholder="pi.ph('actualStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('actualEndTime')"
                name="actualEndTime"
              >
                <a-date-picker
                  v-model:value="formState.actualEndTime"
                  :placeholder="pi.ph('actualEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('acceptanceResult')"
                name="acceptanceResult"
              >
                <a-input-number
                  v-model:value="formState.acceptanceResult"
                  :placeholder="pi.ph('acceptanceResult')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('acceptedByEmployeeId')"
                name="acceptedByEmployeeId"
              >
                <TaktSelect
                  v-model:value="formState.acceptedByEmployeeId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('acceptedByEmployeeId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('acceptedByEmployeeName')"
                name="acceptedByEmployeeName"
              >
                <a-input
                  v-model:value="formState.acceptedByEmployeeName"
                  :placeholder="pi.ph('acceptedByEmployeeName')"
                  show-count
                  :maxlength="80"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('acceptedAt')"
                name="acceptedAt"
              >
                <a-date-picker
                  v-model:value="formState.acceptedAt"
                  :placeholder="pi.ph('acceptedAt')"
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
 * 服务工单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/customer-service/ticket/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCustomerServiceTicketI18n } from '../composables/useTicketI18n'

/** 实体字段 i18n */
const pi = useCustomerServiceTicketI18n()
import type { CustomerServiceTicketCreate } from '@/types/logistics/customer-service/ticket.types'
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
  formData?: Partial<CustomerServiceTicketCreate & { customerServiceTicketId?: string }> | null
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
  priority: 3,
  ticketStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 customerServiceTicketId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.customerServiceTicketId) {
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
    if (!props.formData?.customerServiceTicketId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  serviceTicketCode: [
    {
      required: true,
      message: pi.ph('serviceTicketCode'),
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
  ticketType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ticketType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ticketType'))
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
  ticketStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ticketStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ticketStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ticketSubject: [
    {
      required: true,
      message: pi.ph('ticketSubject'),
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
  if ('ticketType' in payload) {
    const rawticketType = payload.ticketType
    if (rawticketType === undefined || rawticketType === null || rawticketType === '') {
      delete payload.ticketType
    } else {
      const numticketType = typeof rawticketType === 'number' ? rawticketType : Number(rawticketType)
      if (Number.isFinite(numticketType)) payload.ticketType = numticketType
      else delete payload.ticketType
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
  if ('ticketStatus' in payload) {
    const rawticketStatus = payload.ticketStatus
    if (rawticketStatus === undefined || rawticketStatus === null || rawticketStatus === '') {
      delete payload.ticketStatus
    } else {
      const numticketStatus = typeof rawticketStatus === 'number' ? rawticketStatus : Number(rawticketStatus)
      if (Number.isFinite(numticketStatus)) payload.ticketStatus = numticketStatus
      else delete payload.ticketStatus
    }
  }
  if ('acceptanceResult' in payload) {
    const rawacceptanceResult = payload.acceptanceResult
    if (rawacceptanceResult === undefined || rawacceptanceResult === null || rawacceptanceResult === '') {
      delete payload.acceptanceResult
    } else {
      const numacceptanceResult = typeof rawacceptanceResult === 'number' ? rawacceptanceResult : Number(rawacceptanceResult)
      if (Number.isFinite(numacceptanceResult)) payload.acceptanceResult = numacceptanceResult
      else delete payload.acceptanceResult
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.customerServiceTicketId) {
    payload.customerServiceTicketId = props.formData.customerServiceTicketId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.customerServiceTicketId)

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

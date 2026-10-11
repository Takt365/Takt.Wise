<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/ticket/components -->
<!-- 文件名称：TicketForm.vue -->
<!-- 功能描述：服务台工单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
                :label="pi.label('ticketCode')"
                name="ticketCode"
              >
                <a-input
                  v-model:value="formState.ticketCode"
                  :placeholder="pi.ph('ticketCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.ticketId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ticketTitle')"
                name="ticketTitle"
              >
                <a-input
                  v-model:value="formState.ticketTitle"
                  :placeholder="pi.ph('ticketTitle')"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('ticketContent')"
                name="ticketContent"
              >
                <TaktRichEditor
                  v-model:value="formState.ticketContent"
                  :placeholder="pi.ph('ticketContent')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('attachments')"
                name="attachments"
              >
                <a-input
                  v-model:value="formState.attachments"
                  :placeholder="pi.ph('attachments')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                :label="pi.label('urgency')"
                name="urgency"
              >
                <TaktSelect
                  v-model:value="formState.urgency"
                  dict-type="sys_urgency_level"
                  :placeholder="pi.ph('urgency')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('impact')"
                name="impact"
              >
                <TaktSelect
                  v-model:value="formState.impact"
                  dict-type="sys_impact_level"
                  :placeholder="pi.ph('impact')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('categoryCode')"
                name="categoryCode"
              >
                <a-input
                  v-model:value="formState.categoryCode"
                  :placeholder="pi.ph('categoryCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.ticketId"
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
                :label="pi.label('ticketSource')"
                name="ticketSource"
              >
                <TaktSelect
                  v-model:value="formState.ticketSource"
                  dict-type="routine_help_desk_ticket_source"
                  :placeholder="pi.ph('ticketSource')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('submitterId')"
                name="submitterId"
              >
                <TaktSelect
                  v-model:value="formState.submitterId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('submitterId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('submitterName')"
                name="submitterName"
              >
                <a-input
                  v-model:value="formState.submitterName"
                  :placeholder="pi.ph('submitterName')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assigneeId')"
                name="assigneeId"
              >
                <TaktSelect
                  v-model:value="formState.assigneeId"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('assigneeId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('assigneeName')"
                name="assigneeName"
              >
                <a-input
                  v-model:value="formState.assigneeName"
                  :placeholder="pi.ph('assigneeName')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('knowledgeId')"
                name="knowledgeId"
              >
                <TaktSelect
                  v-model:value="formState.knowledgeId"
                  api-url="TaktKnowledges/options"
                  :placeholder="pi.ph('knowledgeId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parentTicketId')"
                name="parentTicketId"
              >
                <TaktSelect
                  v-model:value="formState.parentTicketId"
                  api-url="TaktTickets/options"
                  :placeholder="pi.ph('parentTicketId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('firstResponseAt')"
                name="firstResponseAt"
              >
                <a-date-picker
                  v-model:value="formState.firstResponseAt"
                  :placeholder="pi.ph('firstResponseAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('firstResponseDueBy')"
                name="firstResponseDueBy"
              >
                <a-date-picker
                  v-model:value="formState.firstResponseDueBy"
                  :placeholder="pi.ph('firstResponseDueBy')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resolvedAt')"
                name="resolvedAt"
              >
                <a-date-picker
                  v-model:value="formState.resolvedAt"
                  :placeholder="pi.ph('resolvedAt')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resolutionDueBy')"
                name="resolutionDueBy"
              >
                <a-date-picker
                  v-model:value="formState.resolutionDueBy"
                  :placeholder="pi.ph('resolutionDueBy')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('itAssetId')"
                name="itAssetId"
              >
                <TaktSelect
                  v-model:value="formState.itAssetId"
                  api-url="TaktItAssets/options"
                  :placeholder="pi.ph('itAssetId')"
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
                  :maxlength="40"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantDeptId')"
                name="applicantDeptId"
              >
                <TaktSelect
                  v-model:value="formState.applicantDeptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('applicantDeptId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantDeptName')"
                name="applicantDeptName"
              >
                <a-input
                  v-model:value="formState.applicantDeptName"
                  :placeholder="pi.ph('applicantDeptName')"
                  show-count
                  :maxlength="100"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantBy')"
                name="applicantBy"
              >
                <TaktSelect
                  v-model:value="formState.applicantBy"
                  api-url="TaktUsers/options"
                  :placeholder="pi.ph('applicantBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('applicantName')"
                name="applicantName"
              >
                <a-input
                  v-model:value="formState.applicantName"
                  :placeholder="pi.ph('applicantName')"
                  show-count
                  :maxlength="20"
                  disabled
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
                :label="pi.label('childTickets')"
                name="childTickets"
              >
                <a-input
                  v-model:value="formState.childTickets"
                  :placeholder="pi.ph('childTickets')"
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
 * 服务台工单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/ticket/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useTicketI18n } from '../composables/useTicketI18n'

/** 实体字段 i18n */
const pi = useTicketI18n()
import type { TicketCreate } from '@/types/routine/help-desk/ticket.types'
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
  formData?: Partial<TicketCreate & { ticketId?: string }> | null
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
  urgency: 3,
  impact: 3,
  ticketSource: 0,
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ticketId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ticketId) {
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
    if (!props.formData?.ticketId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ticketCode: [
    {
      required: true,
      message: pi.ph('ticketCode'),
      trigger: 'blur'
    }
  ],
  ticketTitle: [
    {
      required: true,
      message: pi.ph('ticketTitle'),
      trigger: 'blur'
    }
  ],
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
  urgency: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('urgency'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('urgency'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  impact: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('impact'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('impact'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ticketSource: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('ticketSource'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('ticketSource'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  submitterId: [
    {
      required: true,
      message: pi.ph('submitterId'),
      trigger: 'change'
    }
  ],
  applicantBy: [
    {
      required: true,
      message: pi.ph('applicantBy'),
      trigger: 'change'
    }
  ],
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
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
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
  if ('urgency' in payload) {
    const rawurgency = payload.urgency
    if (rawurgency === undefined || rawurgency === null || rawurgency === '') {
      delete payload.urgency
    } else {
      const numurgency = typeof rawurgency === 'number' ? rawurgency : Number(rawurgency)
      if (Number.isFinite(numurgency)) payload.urgency = numurgency
      else delete payload.urgency
    }
  }
  if ('impact' in payload) {
    const rawimpact = payload.impact
    if (rawimpact === undefined || rawimpact === null || rawimpact === '') {
      delete payload.impact
    } else {
      const numimpact = typeof rawimpact === 'number' ? rawimpact : Number(rawimpact)
      if (Number.isFinite(numimpact)) payload.impact = numimpact
      else delete payload.impact
    }
  }
  if ('ticketSource' in payload) {
    const rawticketSource = payload.ticketSource
    if (rawticketSource === undefined || rawticketSource === null || rawticketSource === '') {
      delete payload.ticketSource
    } else {
      const numticketSource = typeof rawticketSource === 'number' ? rawticketSource : Number(rawticketSource)
      if (Number.isFinite(numticketSource)) payload.ticketSource = numticketSource
      else delete payload.ticketSource
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
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }

  if (props.formData?.ticketId) {
    payload.ticketId = props.formData.ticketId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ticketId)

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

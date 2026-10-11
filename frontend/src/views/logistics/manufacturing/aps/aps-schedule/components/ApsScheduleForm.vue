<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/components -->
<!-- 文件名称：ApsScheduleForm.vue -->
<!-- 功能描述：APS排程主表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form aps-schedule-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="aps-schedule-form-tabs"
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
                :label="t('entity.apsschedule.materialrequirementsplanningid')"
                name="materialRequirementsPlanningId"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningId"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsschedule.materialrequirementsplanningid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.materialrequirementsplanningcode')"
                name="materialRequirementsPlanningCode"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningCode"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsschedule.materialrequirementsplanningcode') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.schedulecode')"
                name="scheduleCode"
              >
                <a-input
                  v-model:value="formState.scheduleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.schedulename')"
                name="scheduleName"
              >
                <a-input
                  v-model:value="formState.scheduleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulename') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.scheduletype')"
                name="scheduleType"
              >
                <a-input-number
                  v-model:value="formState.scheduleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.scheduletype') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.plandate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.plandate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.planstarttime')"
                name="planStartTime"
              >
                <a-date-picker
                  v-model:value="formState.planStartTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.planstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.planendtime')"
                name="planEndTime"
              >
                <a-date-picker
                  v-model:value="formState.planEndTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.planendtime') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.plancycle')"
                name="planCycle"
              >
                <a-input-number
                  v-model:value="formState.planCycle"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.plancycle') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.workshopcode')"
                name="workshopCode"
              >
                <a-input
                  v-model:value="formState.workshopCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.workshopcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.workshopname')"
                name="workshopName"
              >
                <a-input
                  v-model:value="formState.workshopName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.workshopname') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.productionlinecode')"
                name="productionLineCode"
              >
                <a-input
                  v-model:value="formState.productionLineCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.productionlinecode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.apsScheduleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.productionlinename')"
                name="productionLineName"
              >
                <a-input
                  v-model:value="formState.productionLineName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.productionlinename') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.schedulestrategy')"
                name="scheduleStrategy"
              >
                <a-input-number
                  v-model:value="formState.scheduleStrategy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulestrategy') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.schedulealgorithm')"
                name="scheduleAlgorithm"
              >
                <a-input-number
                  v-model:value="formState.scheduleAlgorithm"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulealgorithm') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.optimizationobjective')"
                name="optimizationObjective"
              >
                <a-input-number
                  v-model:value="formState.optimizationObjective"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.optimizationobjective') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.schedulestatus')"
                name="scheduleStatus"
              >
                <a-input-number
                  v-model:value="formState.scheduleStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulestatus') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.apsschedule.plannergroupcode')"
                name="plannerGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.plannerGroupCode"
                  api-url="TaktApsGroups/options"
                  :api-params="{ apsGroupCategory: 1 }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.plannergroupcode') })"
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
                :label="t('entity.apsschedule.plannergroupname')"
                name="plannerGroupName"
              >
                <a-input
                  v-model:value="formState.plannerGroupName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsschedule.plannergroupname') })"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsschedule.publishtime')"
                name="publishTime"
              >
                <a-date-picker
                  v-model:value="formState.publishTime"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.publishtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsschedule.publishuserid')"
                name="publishUserId"
              >
                <TaktSelect
                  v-model:value="formState.publishUserId"
                  api-url="TaktApsGroups/options"
                  :api-params="{ apsGroupCategory: 2 }"
                  :field-names="{ label: 'dictLabel', value: 'extValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsschedule.publishuserid') })"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsschedule.publishUserName')"
                name="publishUserName"
              >
                <a-input
                  v-model:value="formState.publishUserName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsschedule.publishUserName') })"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.apsschedule.scheduledescription')"
                name="scheduleDescription"
              >
                <a-textarea
                  v-model:value="formState.scheduleDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.apsschedule.scheduledescription') })"
                  :rows="2"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="apsScheduleItemTableRef"
      v-model="childApsScheduleItemRows"
      :columns="apsScheduleItemFormColumns"
      :title="t('entity.apsscheduleitem._self')"
      :add-button-entity="t('entity.apsscheduleitem._self')"
      id-field="apsScheduleItemId"
      :default-row="createDefaultApsScheduleItemRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * APS排程主表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/aps/aps-schedule/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ApsScheduleCreate } from '@/types/logistics/manufacturing/aps/apsSchedule.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","cultureCode","materialRequirementsPlanningId","materialRequirementsPlanningCode","plantCode","scheduleCode","scheduleName","scheduleType","planDate","planStartTime","planEndTime","planCycle","workshopCode","workshopName","productionLineCode","productionLineName","scheduleStrategy","scheduleAlgorithm","optimizationObjective","scheduleStatus","plannerGroupCode","plannerGroupName","publishTime","publishUserId","publishUserName","scheduleDescription","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childApsScheduleItemRows = ref<Record<string, unknown>[]>([])
const apsScheduleItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 apsScheduleItem 可编辑列 */
const apsScheduleItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'apsOrderId',
    title: t('entity.apsscheduleitem.apsorderid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.apsscheduleitem.apsorderid') }),
  },
  {
    key: 'apsOperationId',
    title: t('entity.apsscheduleitem.apsoperationid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.apsscheduleitem.apsoperationid') }),
  },
  {
    key: 'routingItemId',
    title: t('entity.apsscheduleitem.routingitemid'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.apsscheduleitem.routingitemid') }),
  },
  {
    key: 'lineNumber',
    title: t('entity.apsscheduleitem.linenumber'),
    editor: 'inputNumber',
    width: 140, summary: 'sum',
  },
  {
    key: 'workOrderCode',
    title: t('entity.apsscheduleitem.workordercode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'productCode',
    title: t('entity.apsscheduleitem.productcode'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'productName',
    title: t('entity.apsscheduleitem.productname'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'workCenterCode',
    title: t('entity.apsscheduleitem.workcentercode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.apsscheduleitem.workcentercode') }),
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null | undefined) {
  childApsScheduleItemRows.value = ((val as any)?.items ?? []) as Record<string, unknown>[]
}

function createDefaultApsScheduleItemRow(): Record<string, unknown> {
  return {
    apsOrderId: '',
    apsOperationId: '',
    routingItemId: '',
    lineNumber: (childApsScheduleItemRows.value.length + 1) * 10,
    workOrderCode: '',
    productCode: '',
    productName: '',
    workCenterCode: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.apsScheduleId ?? ''
  return {
    ...formState,
    items: apsScheduleItemTableRef.value?.getRows?.() ?? childApsScheduleItemRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      apsScheduleId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsScheduleCreate & { apsScheduleId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 apsScheduleId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsScheduleId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).items
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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
    const isCreate = !props.formData?.apsScheduleId
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
      message: t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') }),
      trigger: 'blur'
    }
  ],
  scheduleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulecode') }),
      trigger: 'blur'
    }
  ],
  scheduleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.apsschedule.schedulename') }),
      trigger: 'blur'
    }
  ],
  scheduleType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.scheduletype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.scheduletype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  planDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsschedule.plandate') }),
      trigger: 'change'
    }
  ],
  planStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsschedule.planstarttime') }),
      trigger: 'change'
    }
  ],
  planEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.apsschedule.planendtime') }),
      trigger: 'change'
    }
  ],
  planCycle: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.plancycle') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.plancycle') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStrategy: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulestrategy') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulestrategy') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleAlgorithm: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulealgorithm') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulealgorithm') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  optimizationObjective: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.optimizationobjective') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.optimizationobjective') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduleStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulestatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.apsschedule.schedulestatus') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await apsScheduleItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('scheduleType' in payload) {
    const rawscheduleType = payload.scheduleType
    payload.scheduleType = typeof rawscheduleType === 'number' ? rawscheduleType : Number(rawscheduleType)
  }
  if ('planCycle' in payload) {
    const rawplanCycle = payload.planCycle
    payload.planCycle = typeof rawplanCycle === 'number' ? rawplanCycle : Number(rawplanCycle)
  }
  if ('scheduleStrategy' in payload) {
    const rawscheduleStrategy = payload.scheduleStrategy
    payload.scheduleStrategy = typeof rawscheduleStrategy === 'number' ? rawscheduleStrategy : Number(rawscheduleStrategy)
  }
  if ('scheduleAlgorithm' in payload) {
    const rawscheduleAlgorithm = payload.scheduleAlgorithm
    payload.scheduleAlgorithm = typeof rawscheduleAlgorithm === 'number' ? rawscheduleAlgorithm : Number(rawscheduleAlgorithm)
  }
  if ('optimizationObjective' in payload) {
    const rawoptimizationObjective = payload.optimizationObjective
    payload.optimizationObjective = typeof rawoptimizationObjective === 'number' ? rawoptimizationObjective : Number(rawoptimizationObjective)
  }
  if ('scheduleStatus' in payload) {
    const rawscheduleStatus = payload.scheduleStatus
    payload.scheduleStatus = typeof rawscheduleStatus === 'number' ? rawscheduleStatus : Number(rawscheduleStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.apsScheduleId)
  childApsScheduleItemRows.value = []
  apsScheduleItemTableRef.value?.resetRows?.()
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

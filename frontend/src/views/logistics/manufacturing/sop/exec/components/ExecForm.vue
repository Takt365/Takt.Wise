<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/exec/components -->
<!-- 文件名称：ExecForm.vue -->
<!-- 功能描述：SOP 工位执行追溯实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form exec-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="exec-form-tabs"
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
                :label="pi.label('productionOrderId')"
                name="productionOrderId"
              >
                <TaktSelect
                  v-model:value="formState.productionOrderId"
                  api-url="TaktProductionOrders/options"
                  :placeholder="pi.ph('productionOrderId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workOrderCode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="pi.ph('workOrderCode')"
                  show-count
                  :maxlength="50"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="pi.ph('serialNumber')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.sopExecId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('routingItemId')"
                name="routingItemId"
              >
                <TaktSelect
                  v-model:value="formState.routingItemId"
                  api-url="TaktRoutingItems/options"
                  :placeholder="pi.ph('routingItemId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processSegmentType')"
                name="processSegmentType"
              >
                <TaktSelect
                  v-model:value="formState.processSegmentType"
                  dict-type="logistics_manufacturing_process_segment_type"
                  :placeholder="pi.ph('processSegmentType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workstationId')"
                name="workstationId"
              >
                <TaktSelect
                  v-model:value="formState.workstationId"
                  api-url="TaktSopWorkstations/options"
                  :placeholder="pi.ph('workstationId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('employeeId')"
                name="employeeId"
              >
                <TaktSelect
                  v-model:value="formState.employeeId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('employeeId')"
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
                :label="pi.label('sopId')"
                name="sopId"
              >
                <TaktSelect
                  v-model:value="formState.sopId"
                  api-url="TaktSopDocs/options"
                  :placeholder="pi.ph('sopId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('revisionId')"
                name="revisionId"
              >
                <TaktSelect
                  v-model:value="formState.revisionId"
                  api-url="TaktSopRevisions/options"
                  :placeholder="pi.ph('revisionId')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('revision')"
                name="revision"
              >
                <a-input
                  v-model:value="formState.revision"
                  :placeholder="pi.ph('revision')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('startedAt')"
                name="startedAt"
              >
                <a-date-picker
                  v-model:value="formState.startedAt"
                  :placeholder="pi.ph('startedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('endedAt')"
                name="endedAt"
              >
                <a-date-picker
                  v-model:value="formState.endedAt"
                  :placeholder="pi.ph('endedAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('selfCheckResult')"
                name="selfCheckResult"
              >
                <TaktSelect
                  v-model:value="formState.selfCheckResult"
                  dict-type="logistics_manufacturing_sop_check_result"
                  :placeholder="pi.ph('selfCheckResult')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('execStatus')"
                name="execStatus"
              >
                <TaktSelect
                  v-model:value="formState.execStatus"
                  dict-type="logistics_manufacturing_sop_exec_status"
                  :placeholder="pi.ph('execStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('currentStepId')"
                name="currentStepId"
              >
                <TaktSelect
                  v-model:value="formState.currentStepId"
                  api-url="TaktSopSteps/options"
                  :placeholder="pi.ph('currentStepId')"
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
    <!-- 下：子表 steps -->
    <TaktEditableTable
      ref="sopExecStepTableRef"
      v-model="childSopExecStepRows"
      :columns="sopExecStepFormColumns"
      :title="sopExecStepPi.self()"
      :add-button-entity="sopExecStepPi.self()"
      id-field="sopExecStepId"
      :default-row="createDefaultSopExecStepRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-stepId="{ record }">
        <TaktSelect
          v-model:value="record.stepId"
          api-url="TaktSopSteps/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sopExecStepPi.queryPh('stepId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-stepResult="{ record }">
        <TaktSelect
          v-model:value="record.stepResult"
          dict-type="logistics_manufacturing_sop_check_result"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sopExecStepPi.ph('stepResult')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-confirmedBy="{ record }">
        <TaktSelect
          v-model:value="record.confirmedBy"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sopExecStepPi.queryPh('confirmedBy', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-blockNextStep="{ record }">
        <TaktSelect
          v-model:value="record.blockNextStep"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="sopExecStepPi.ph('blockNextStep')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 工位执行追溯实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/exec/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useSopExecI18n } from '../composables/useExecI18n'

/** 实体字段 i18n */
const pi = useSopExecI18n()

import type { SopExecCreate } from '@/types/logistics/manufacturing/sop/exec.types'
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
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","productionOrderId","workOrderCode","serialNumber","materialCode","routingItemId","processSegmentType","workstationId","employeeId","sopId","revisionId","revision","startedAt","endedAt","selfCheckResult","execStatus","currentStepId","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useSopExecStepI18n } from '../composables/useExecStepI18n'

const sopExecStepPi = useSopExecStepI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childSopExecStepRows = ref<Record<string, unknown>[]>([])
const sopExecStepTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 sopExecStep 可编辑列 */
const sopExecStepFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'stepId',
    title: sopExecStepPi.label('stepId'),
    width: 140,
  },
  {
    key: 'stepNo',
    title: sopExecStepPi.label('stepNo'),
    width: 140,
  },
  {
    key: 'startedAt',
    title: sopExecStepPi.label('startedAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'endedAt',
    title: sopExecStepPi.label('endedAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'stepResult',
    title: sopExecStepPi.label('stepResult'),
    width: 140,
  },
  {
    key: 'confirmedBy',
    title: sopExecStepPi.label('confirmedBy'),
    width: 140,
  },
  {
    key: 'confirmedAt',
    title: sopExecStepPi.label('confirmedAt'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'blockNextStep',
    title: sopExecStepPi.label('blockNextStep'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SopExecCreate & { sopExecId?: string }> | null | undefined) {
  const rows_sopExecStep = ((val as any)?.steps ?? []) as Record<string, unknown>[]
  childSopExecStepRows.value = rows_sopExecStep
}

function createDefaultSopExecStepRow(): Record<string, unknown> {
  return {
    stepId: '',
    stepNo: 0,
    startedAt: '',
    endedAt: '',
    stepResult: 0,
    confirmedBy: '',
    confirmedAt: '',
    blockNextStep: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sopExecId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    steps: sopExecStepTableRef.value?.getRows?.() ?? childSopExecStepRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      execId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SopExecCreate & { sopExecId?: string }> | null
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
  processSegmentType: 1,
  selfCheckResult: 1,
  execStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 sopExecId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.sopExecId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).steps
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
    if (!props.formData?.sopExecId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  routingItemId: [
    {
      required: true,
      message: pi.ph('routingItemId'),
      trigger: 'change'
    }
  ],
  processSegmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('processSegmentType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('processSegmentType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workstationId: [
    {
      required: true,
      message: pi.ph('workstationId'),
      trigger: 'change'
    }
  ],
  employeeId: [
    {
      required: true,
      message: pi.ph('employeeId'),
      trigger: 'change'
    }
  ],
  sopId: [
    {
      required: true,
      message: pi.ph('sopId'),
      trigger: 'change'
    }
  ],
  revisionId: [
    {
      required: true,
      message: pi.ph('revisionId'),
      trigger: 'change'
    }
  ],
  revision: [
    {
      required: true,
      message: pi.ph('revision'),
      trigger: 'blur'
    }
  ],
  startedAt: [
    {
      required: true,
      message: pi.ph('startedAt'),
      trigger: 'change'
    }
  ],
  execStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('execStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('execStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await sopExecStepTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('processSegmentType' in payload) {
    const rawprocessSegmentType = payload.processSegmentType
    if (rawprocessSegmentType === undefined || rawprocessSegmentType === null || rawprocessSegmentType === '') {
      delete payload.processSegmentType
    } else {
      const numprocessSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
      if (Number.isFinite(numprocessSegmentType)) payload.processSegmentType = numprocessSegmentType
      else delete payload.processSegmentType
    }
  }
  if ('selfCheckResult' in payload) {
    const rawselfCheckResult = payload.selfCheckResult
    if (rawselfCheckResult === undefined || rawselfCheckResult === null || rawselfCheckResult === '') {
      delete payload.selfCheckResult
    } else {
      const numselfCheckResult = typeof rawselfCheckResult === 'number' ? rawselfCheckResult : Number(rawselfCheckResult)
      if (Number.isFinite(numselfCheckResult)) payload.selfCheckResult = numselfCheckResult
      else delete payload.selfCheckResult
    }
  }
  if ('execStatus' in payload) {
    const rawexecStatus = payload.execStatus
    if (rawexecStatus === undefined || rawexecStatus === null || rawexecStatus === '') {
      delete payload.execStatus
    } else {
      const numexecStatus = typeof rawexecStatus === 'number' ? rawexecStatus : Number(rawexecStatus)
      if (Number.isFinite(numexecStatus)) payload.execStatus = numexecStatus
      else delete payload.execStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.sopExecId) {
    payload.sopExecId = props.formData.sopExecId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.sopExecId)
  childSopExecStepRows.value = []
  sopExecStepTableRef.value?.resetRows?.()
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

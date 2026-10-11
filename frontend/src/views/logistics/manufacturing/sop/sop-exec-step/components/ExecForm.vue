<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-exec-step/components -->
<!-- 文件名称：ExecForm.vue -->
<!-- 功能描述：SOP 工位执行追溯实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form exec-form flex flex-col min-h-0"
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
                :label="t('entity.sopexec.productionorderid')"
                name="productionOrderId"
              >
                <a-input
                  v-model:value="formState.productionOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.productionorderid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.workorderCode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.workorderCode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.serialnumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.serialnumber') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.materialcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.sopExecId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.routingitemid')"
                name="routingItemId"
              >
                <a-input
                  v-model:value="formState.routingItemId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.routingitemid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.processsegmenttype')"
                name="processSegmentType"
              >
                <TaktSelect
                  v-model:value="formState.processSegmentType"
                  dict-type="logistics_manufacturing_process_segment_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.workstationid')"
                name="workstationId"
              >
                <a-input
                  v-model:value="formState.workstationId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.workstationid') })"
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
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.employeeid')"
                name="employeeId"
              >
                <a-input
                  v-model:value="formState.employeeId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.employeeid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.sopid')"
                name="sopId"
              >
                <a-input
                  v-model:value="formState.sopId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.sopid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.revisionid')"
                name="revisionId"
              >
                <a-input
                  v-model:value="formState.revisionId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.revisionid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.revision')"
                name="revision"
              >
                <a-input
                  v-model:value="formState.revision"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.revision') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.startedat')"
                name="startedAt"
              >
                <a-input
                  v-model:value="formState.startedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.startedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.endedat')"
                name="endedAt"
              >
                <a-input
                  v-model:value="formState.endedAt"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.endedat') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.selfcheckresult')"
                name="selfCheckResult"
              >
                <TaktSelect
                  v-model:value="formState.selfCheckResult"
                  dict-type="logistics_manufacturing_sop_check_result"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.selfcheckresult') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.execstatus')"
                name="execStatus"
              >
                <TaktSelect
                  v-model:value="formState.execStatus"
                  dict-type="logistics_manufacturing_sop_exec_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.sopexec.currentstepid')"
                name="currentStepId"
              >
                <a-input
                  v-model:value="formState.currentStepId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopexec.currentstepid') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
    <!-- 下：子表 steps -->
    <TaktEditableTable
      ref="sopExecStepTableRef"
      v-model="childSopExecStepRows"
      :columns="sopExecStepFormColumns"
      :title="t('entity.sopexecstep._self')"
      :add-button-entity="t('entity.sopexecstep._self')"
      id-field="sopExecStepId"
      :default-row="createDefaultSopExecStepRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * SOP 工位执行追溯实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/sop-exec-step/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SopExecCreate } from '@/types/logistics/manufacturing/sop/exec.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","productionOrderId","workOrderCode","serialNumber","materialCode","routingItemId","processSegmentType","workstationId","employeeId","sopId","revisionId","revision","startedAt","endedAt","selfCheckResult","execStatus","currentStepId","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'

const childSopExecStepRows = ref<Record<string, unknown>[]>([])
const sopExecStepTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 sopExecStep 可编辑列 */
const sopExecStepFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'execId',
    title: t('entity.sopexecstep.execid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'stepId',
    title: t('entity.sopexecstep.stepid'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'stepNo',
    title: t('entity.sopexecstep.stepno'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'startedAt',
    title: t('entity.sopexecstep.startedat'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'endedAt',
    title: t('entity.sopexecstep.endedat'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.sopexecstep.endedat') }),
  },
  {
    key: 'stepResult',
    title: t('entity.sopexecstep.stepresult'),
    editor: 'inputNumber',
    width: 140,
  },
  {
    key: 'confirmedBy',
    title: t('entity.sopexecstep.confirmedby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.sopexecstep.confirmedby') }),
  },
  {
    key: 'confirmedAt',
    title: t('entity.sopexecstep.confirmedat'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.sopexecstep.confirmedat') }),
  }])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<SopExecCreate & { sopExecId?: string }> | null | undefined) {
  childSopExecStepRows.value = ((val as any)?.steps ?? []) as Record<string, unknown>[]
}

function createDefaultSopExecStepRow(): Record<string, unknown> {
  return {
    execId: '',
    stepId: '',
    stepNo: 0,
    startedAt: '',
    endedAt: '',
    stepResult: 0,
    confirmedBy: '',
    confirmedAt: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.sopExecId ?? ''
  return {
    ...formState,
    steps: sopExecStepTableRef.value?.getRows?.() ?? childSopExecStepRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      sopExecId: masterId,
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.sopExecId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.workorderCode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.materialcode') }),
      trigger: 'blur'
    }
  ],
  routingItemId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.routingitemid') }),
      trigger: 'blur'
    }
  ],
  processSegmentType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.processsegmenttype') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  workstationId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.workstationid') }),
      trigger: 'blur'
    }
  ],
  employeeId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.employeeid') }),
      trigger: 'blur'
    }
  ],
  sopId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.sopid') }),
      trigger: 'blur'
    }
  ],
  revisionId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.revisionid') }),
      trigger: 'blur'
    }
  ],
  revision: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.revision') }),
      trigger: 'blur'
    }
  ],
  cultureCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.culturecode') }),
      trigger: 'blur'
    }
  ],
  startedAt: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.sopexec.startedat') }),
      trigger: 'blur'
    }
  ],
  execStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.sopexec.execstatus') }))
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
    payload.processSegmentType = typeof rawprocessSegmentType === 'number' ? rawprocessSegmentType : Number(rawprocessSegmentType)
  }
  if ('selfCheckResult' in payload) {
    const rawselfCheckResult = payload.selfCheckResult
    payload.selfCheckResult = typeof rawselfCheckResult === 'number' ? rawselfCheckResult : Number(rawselfCheckResult)
  }
  if ('execStatus' in payload) {
    const rawexecStatus = payload.execStatus
    payload.execStatus = typeof rawexecStatus === 'number' ? rawexecStatus : Number(rawexecStatus)
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

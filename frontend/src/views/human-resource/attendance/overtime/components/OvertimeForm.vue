<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/attendance/overtime/components -->
<!-- 文件名称：OvertimeForm.vue -->
<!-- 功能描述：加班申请维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form overtime-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="overtime-form-tabs"
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
                :label="pi.label('deptName')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="pi.ph('deptName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overtimeDate')"
                name="overtimeDate"
              >
                <a-date-picker
                  v-model:value="formState.overtimeDate"
                  :placeholder="pi.ph('overtimeDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedStartTime')"
                name="plannedStartTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartTime"
                  :placeholder="pi.ph('plannedStartTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannedEndTime')"
                name="plannedEndTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndTime"
                  :placeholder="pi.ph('plannedEndTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalEmployees')"
                name="totalEmployees"
              >
                <a-input-number
                  v-model:value="formState.totalEmployees"
                  :placeholder="pi.ph('totalEmployees')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalPlannedHours')"
                name="totalPlannedHours"
              >
                <a-input-number
                  v-model:value="formState.totalPlannedHours"
                  :placeholder="pi.ph('totalPlannedHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalActualHours')"
                name="totalActualHours"
              >
                <a-input-number
                  v-model:value="formState.totalActualHours"
                  :placeholder="pi.ph('totalActualHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('overtimeType')"
                name="overtimeType"
              >
                <TaktSelect
                  v-model:value="formState.overtimeType"
                  dict-type="humanresource_attendance_overtime_type"
                  :placeholder="pi.ph('overtimeType')"
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
                :label="pi.label('reason')"
                name="reason"
              >
                <a-input
                  v-model:value="formState.reason"
                  :placeholder="pi.ph('reason')"
                  show-count
                  :maxlength="1000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('handlingBy')"
                name="handlingBy"
              >
                <TaktSelect
                  v-model:value="formState.handlingBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('handlingBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('handlingAt')"
                name="handlingAt"
              >
                <a-date-picker
                  v-model:value="formState.handlingAt"
                  :placeholder="pi.ph('handlingAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('handlingComment')"
                name="handlingComment"
              >
                <a-input
                  v-model:value="formState.handlingComment"
                  :placeholder="pi.ph('handlingComment')"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('overtimeStatus')"
                name="overtimeStatus"
              >
                <TaktSelect
                  v-model:value="formState.overtimeStatus"
                  dict-type="sys_approval_status"
                  :placeholder="pi.ph('overtimeStatus')"
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
    <!-- 下：子表 items -->
    <TaktEditableTable
      ref="overtimeItemTableRef"
      v-model="childOvertimeItemRows"
      :columns="overtimeItemFormColumns"
      :title="overtimeItemPi.self()"
      :add-button-entity="overtimeItemPi.self()"
      id-field="overtimeItemId"
      :default-row="createDefaultOvertimeItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-employeeId="{ record }">
        <TaktSelect
          v-model:value="record.employeeId"
          api-url="TaktEmployees/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="overtimeItemPi.queryPh('employeeId', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-isObsolete="{ record }">
        <TaktSelect
          v-model:value="record.isObsolete"
          dict-type="sys_yes_no"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="overtimeItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 加班申请维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/attendance/overtime/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useOvertimeI18n } from '../composables/useOvertimeI18n'

/** 实体字段 i18n */
const pi = useOvertimeI18n()

import type { OvertimeCreate } from '@/types/human-resource/attendance/overtime.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","deptName","overtimeDate","plannedStartTime","plannedEndTime","totalEmployees","totalPlannedHours","totalActualHours","overtimeType","reason","handlingBy","handlingAt","handlingComment","overtimeStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { resolveNextDetailLineNumber } from '@/utils/taktSequence'
import { useOvertimeItemI18n } from '../composables/useOvertimeItemI18n'

const overtimeItemPi = useOvertimeItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childOvertimeItemRows = ref<Record<string, unknown>[]>([])
const overtimeItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedOvertimeItemRow(row: Record<string, unknown>): boolean {
  const id = row.overtimeItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextOvertimeItemLineNumber(): number {
  const rows = overtimeItemTableRef.value?.getRows?.() ?? childOvertimeItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 overtimeItem 可编辑列 */
const overtimeItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: overtimeItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'employeeId',
    title: overtimeItemPi.label('employeeId'),
    width: 140,
  },
  {
    key: 'employeeName',
    title: overtimeItemPi.label('employeeName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'plannedHours',
    title: overtimeItemPi.label('plannedHours'),
    width: 140,
  },
  {
    key: 'actualStartTime',
    title: overtimeItemPi.label('actualStartTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'actualEndTime',
    title: overtimeItemPi.label('actualEndTime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'actualHours',
    title: overtimeItemPi.label('actualHours'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: overtimeItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<OvertimeCreate & { overtimeId?: string }> | null | undefined) {
  const rows_overtimeItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childOvertimeItemRows.value = rows_overtimeItem
}

function createDefaultOvertimeItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextOvertimeItemLineNumber(),
    employeeId: '',
    employeeName: '',
    plannedHours: 0,
    actualStartTime: '',
    actualEndTime: '',
    actualHours: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.overtimeId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: overtimeItemTableRef.value?.getRows?.() ?? childOvertimeItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        overtimeId: masterId,
      }
      if (isUpdate && isPersistedOvertimeItemRow(row)) {
        normalized.overtimeItemId = row.overtimeItemId
      } else {
        delete normalized.overtimeItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<OvertimeCreate & { overtimeId?: string }> | null
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
  overtimeStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 overtimeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.overtimeId) {
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
    if (!props.formData?.overtimeId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  overtimeDate: [
    {
      required: true,
      message: pi.ph('overtimeDate'),
      trigger: 'change'
    }
  ],
  plannedStartTime: [
    {
      required: true,
      message: pi.ph('plannedStartTime'),
      trigger: 'change'
    }
  ],
  plannedEndTime: [
    {
      required: true,
      message: pi.ph('plannedEndTime'),
      trigger: 'change'
    }
  ],
  totalEmployees: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalEmployees'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalEmployees'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalPlannedHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalPlannedHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalPlannedHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalActualHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalActualHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalActualHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  overtimeType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overtimeType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overtimeType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingBy: [
    {
      required: true,
      message: pi.ph('handlingBy'),
      trigger: 'change'
    }
  ],
  overtimeStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('overtimeStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('overtimeStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await overtimeItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('totalEmployees' in payload) {
    const rawtotalEmployees = payload.totalEmployees
    if (rawtotalEmployees === undefined || rawtotalEmployees === null || rawtotalEmployees === '') {
      delete payload.totalEmployees
    } else {
      const numtotalEmployees = typeof rawtotalEmployees === 'number' ? rawtotalEmployees : Number(rawtotalEmployees)
      if (Number.isFinite(numtotalEmployees)) payload.totalEmployees = numtotalEmployees
      else delete payload.totalEmployees
    }
  }
  if ('totalPlannedHours' in payload) {
    const rawtotalPlannedHours = payload.totalPlannedHours
    if (rawtotalPlannedHours === undefined || rawtotalPlannedHours === null || rawtotalPlannedHours === '') {
      delete payload.totalPlannedHours
    } else {
      const numtotalPlannedHours = typeof rawtotalPlannedHours === 'number' ? rawtotalPlannedHours : Number(rawtotalPlannedHours)
      if (Number.isFinite(numtotalPlannedHours)) payload.totalPlannedHours = numtotalPlannedHours
      else delete payload.totalPlannedHours
    }
  }
  if ('totalActualHours' in payload) {
    const rawtotalActualHours = payload.totalActualHours
    if (rawtotalActualHours === undefined || rawtotalActualHours === null || rawtotalActualHours === '') {
      delete payload.totalActualHours
    } else {
      const numtotalActualHours = typeof rawtotalActualHours === 'number' ? rawtotalActualHours : Number(rawtotalActualHours)
      if (Number.isFinite(numtotalActualHours)) payload.totalActualHours = numtotalActualHours
      else delete payload.totalActualHours
    }
  }
  if ('overtimeType' in payload) {
    const rawovertimeType = payload.overtimeType
    if (rawovertimeType === undefined || rawovertimeType === null || rawovertimeType === '') {
      delete payload.overtimeType
    } else {
      const numovertimeType = typeof rawovertimeType === 'number' ? rawovertimeType : Number(rawovertimeType)
      if (Number.isFinite(numovertimeType)) payload.overtimeType = numovertimeType
      else delete payload.overtimeType
    }
  }
  if ('overtimeStatus' in payload) {
    const rawovertimeStatus = payload.overtimeStatus
    if (rawovertimeStatus === undefined || rawovertimeStatus === null || rawovertimeStatus === '') {
      delete payload.overtimeStatus
    } else {
      const numovertimeStatus = typeof rawovertimeStatus === 'number' ? rawovertimeStatus : Number(rawovertimeStatus)
      if (Number.isFinite(numovertimeStatus)) payload.overtimeStatus = numovertimeStatus
      else delete payload.overtimeStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.overtimeId) {
    payload.overtimeId = props.formData.overtimeId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.overtimeId)
  childOvertimeItemRows.value = []
  overtimeItemTableRef.value?.resetRows?.()
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

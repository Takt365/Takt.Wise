<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/components -->
<!-- 文件名称：PcbaInspectionDetailForm.vue -->
<!-- 功能描述：PCBA检查日报实体 不良率子表 pcbaInspectionDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-inspection-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-inspection-detail-form-tabs"
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
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pcbaBoardType')"
                name="pcbaBoardType"
              >
                <TaktSelect
                  v-model:value="formState.pcbaBoardType"
                  dict-type="logistics_manufacturing_pcba_function"
                  :placeholder="pi.ph('pcbaBoardType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('visualInspectionLine')"
                name="visualInspectionLine"
              >
                <TaktSelect
                  v-model:value="formState.visualInspectionLine"
                  dict-type="logistics_manufacturing_visual_inspection_line_category"
                  :placeholder="pi.ph('visualInspectionLine')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('aoiLine')"
                name="aoiLine"
              >
                <TaktSelect
                  v-model:value="formState.aoiLine"
                  dict-type="logistics_manufacturing_aoi_inspection_line_category"
                  :placeholder="pi.ph('aoiLine')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bSideAssemblyDate')"
                name="bSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.bSideAssemblyDate"
                  :placeholder="pi.ph('bSideAssemblyDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tSideAssemblyDate')"
                name="tSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.tSideAssemblyDate"
                  :placeholder="pi.ph('tSideAssemblyDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_manufacturing_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectorName')"
                name="inspectorName"
              >
                <TaktSelect
                  v-model:value="formState.inspectorName"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('inspectorName')"
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
                :label="pi.label('dailyCompletedQty')"
                name="dailyCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.dailyCompletedQty"
                  :placeholder="pi.ph('dailyCompletedQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectionQty')"
                name="inspectionQty"
              >
                <a-input-number
                  v-model:value="formState.inspectionQty"
                  :placeholder="pi.ph('inspectionQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectionStatus')"
                name="inspectionStatus"
              >
                <TaktSelect
                  v-model:value="formState.inspectionStatus"
                  dict-type="logistics_manufacturing_pcba_inspection_status"
                  :placeholder="pi.ph('inspectionStatus')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('TeamCode')"
                name="TeamCode"
              >
                <TaktSelect
                  v-model:value="formState.TeamCode"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('TeamCode')"
                  :disabled="!!formData?.pcbaInspectionDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectionWorkHours')"
                name="inspectionWorkHours"
              >
                <a-input-number
                  v-model:value="formState.inspectionWorkHours"
                  :placeholder="pi.ph('inspectionWorkHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('aoiWorkHours')"
                name="aoiWorkHours"
              >
                <a-input-number
                  v-model:value="formState.aoiWorkHours"
                  :placeholder="pi.ph('aoiWorkHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectQty')"
                name="defectQty"
              >
                <a-input-number
                  v-model:value="formState.defectQty"
                  :placeholder="pi.ph('defectQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handPlacement')"
                name="handPlacement"
              >
                <a-input
                  v-model:value="formState.handPlacement"
                  :placeholder="pi.ph('handPlacement')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="pi.ph('content')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectLocation')"
                name="defectLocation"
              >
                <TaktSelect
                  v-model:value="formState.defectLocation"
                  dict-type="logistics_manufacturing_pcb_location_category"
                  :placeholder="pi.ph('defectLocation')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isObsolete')"
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
            <a-col :span="12">
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
            <a-col :span="12">
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * PCBA检查日报实体 不良率子表 pcbaInspectionDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaInspectionDetailI18n } from '../composables/usePcbaInspectionDetailI18n'

/** 实体字段 i18n */
const pi = usePcbaInspectionDetailI18n()

import type { PcbaInspectionDetailCreate } from '@/types/logistics/manufacturing/defect/pcbaInspectionDetail.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","pcbaBoardType","visualInspectionLine","aoiLine","bSideAssemblyDate","tSideAssemblyDate","shiftNo","inspectorName","dailyCompletedQty","inspectionQty","inspectionStatus","TeamCode","inspectionWorkHours","aoiWorkHours","defectQty","handPlacement","serialNumber","content","defectLocation","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaInspectionDetailCreate & { pcbaInspectionDetailId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
  masterRow: null,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  inspectionStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaInspectionDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaInspectionDetailId) {
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
    if (!props.formData?.pcbaInspectionDetailId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  dailyCompletedQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('dailyCompletedQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('dailyCompletedQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionWorkHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionWorkHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionWorkHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  aoiWorkHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('aoiWorkHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('aoiWorkHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
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

/** 映射为 Create/Update DTO（含主表外键 pcbaInspectionId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    if (rawlineNumber === undefined || rawlineNumber === null || rawlineNumber === '') {
      delete payload.lineNumber
    } else {
      const numlineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
      if (Number.isFinite(numlineNumber)) payload.lineNumber = numlineNumber
      else delete payload.lineNumber
    }
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    if (rawshiftNo === undefined || rawshiftNo === null || rawshiftNo === '') {
      delete payload.shiftNo
    } else {
      const numshiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
      if (Number.isFinite(numshiftNo)) payload.shiftNo = numshiftNo
      else delete payload.shiftNo
    }
  }
  if ('dailyCompletedQty' in payload) {
    const rawdailyCompletedQty = payload.dailyCompletedQty
    if (rawdailyCompletedQty === undefined || rawdailyCompletedQty === null || rawdailyCompletedQty === '') {
      delete payload.dailyCompletedQty
    } else {
      const numdailyCompletedQty = typeof rawdailyCompletedQty === 'number' ? rawdailyCompletedQty : Number(rawdailyCompletedQty)
      if (Number.isFinite(numdailyCompletedQty)) payload.dailyCompletedQty = numdailyCompletedQty
      else delete payload.dailyCompletedQty
    }
  }
  if ('inspectionQty' in payload) {
    const rawinspectionQty = payload.inspectionQty
    if (rawinspectionQty === undefined || rawinspectionQty === null || rawinspectionQty === '') {
      delete payload.inspectionQty
    } else {
      const numinspectionQty = typeof rawinspectionQty === 'number' ? rawinspectionQty : Number(rawinspectionQty)
      if (Number.isFinite(numinspectionQty)) payload.inspectionQty = numinspectionQty
      else delete payload.inspectionQty
    }
  }
  if ('inspectionStatus' in payload) {
    const rawinspectionStatus = payload.inspectionStatus
    if (rawinspectionStatus === undefined || rawinspectionStatus === null || rawinspectionStatus === '') {
      delete payload.inspectionStatus
    } else {
      const numinspectionStatus = typeof rawinspectionStatus === 'number' ? rawinspectionStatus : Number(rawinspectionStatus)
      if (Number.isFinite(numinspectionStatus)) payload.inspectionStatus = numinspectionStatus
      else delete payload.inspectionStatus
    }
  }
  if ('inspectionWorkHours' in payload) {
    const rawinspectionWorkHours = payload.inspectionWorkHours
    if (rawinspectionWorkHours === undefined || rawinspectionWorkHours === null || rawinspectionWorkHours === '') {
      delete payload.inspectionWorkHours
    } else {
      const numinspectionWorkHours = typeof rawinspectionWorkHours === 'number' ? rawinspectionWorkHours : Number(rawinspectionWorkHours)
      if (Number.isFinite(numinspectionWorkHours)) payload.inspectionWorkHours = numinspectionWorkHours
      else delete payload.inspectionWorkHours
    }
  }
  if ('aoiWorkHours' in payload) {
    const rawaoiWorkHours = payload.aoiWorkHours
    if (rawaoiWorkHours === undefined || rawaoiWorkHours === null || rawaoiWorkHours === '') {
      delete payload.aoiWorkHours
    } else {
      const numaoiWorkHours = typeof rawaoiWorkHours === 'number' ? rawaoiWorkHours : Number(rawaoiWorkHours)
      if (Number.isFinite(numaoiWorkHours)) payload.aoiWorkHours = numaoiWorkHours
      else delete payload.aoiWorkHours
    }
  }
  if ('defectQty' in payload) {
    const rawdefectQty = payload.defectQty
    if (rawdefectQty === undefined || rawdefectQty === null || rawdefectQty === '') {
      delete payload.defectQty
    } else {
      const numdefectQty = typeof rawdefectQty === 'number' ? rawdefectQty : Number(rawdefectQty)
      if (Number.isFinite(numdefectQty)) payload.defectQty = numdefectQty
      else delete payload.defectQty
    }
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    if (rawisObsolete === undefined || rawisObsolete === null || rawisObsolete === '') {
      delete payload.isObsolete
    } else {
      const numisObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
      if (Number.isFinite(numisObsolete)) payload.isObsolete = numisObsolete
      else delete payload.isObsolete
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }
  if (props.formData?.pcbaInspectionDetailId) {
    payload.pcbaInspectionDetailId = props.formData.pcbaInspectionDetailId
  }
  payload.pcbaInspectionId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.pcbaInspectionCode ?? masterRow.PcbaInspectionCode
    const masterName = masterRow.pcbaInspectionName ?? masterRow.PcbaInspectionName
    if (masterCode != null && masterCode !== '' && !payload.pcbaInspectionCode) {
      payload.pcbaInspectionCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.pcbaInspectionName) {
      payload.pcbaInspectionName = masterName
    }
    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaInspectionDetailId)
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

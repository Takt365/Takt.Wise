<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/order/components -->
<!-- 文件名称：OperationForm.vue -->
<!-- 功能描述：APS 排程订单子表 apsOperation 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form operation-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="operation-form-tabs"
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
                :label="pi.label('processCode')"
                name="processCode"
              >
                <a-input
                  v-model:value="formState.processCode"
                  :placeholder="pi.ph('processCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.apsOperationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('processName')"
                name="processName"
              >
                <a-input
                  v-model:value="formState.processName"
                  :placeholder="pi.ph('processName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenterCode')"
                name="workCenterCode"
              >
                <TaktSelect
                  v-model:value="formState.workCenterCode"
                  api-url="TaktWorkCenters/options"
                  :placeholder="pi.ph('workCenterCode')"
                  :disabled="!!formData?.apsOperationId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenterResourceId')"
                name="workCenterResourceId"
              >
                <TaktSelect
                  v-model:value="formState.workCenterResourceId"
                  api-url="TaktWorkCenterResources/options"
                  :placeholder="pi.ph('workCenterResourceId')"
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
                :label="pi.label('plannedDurationMinutes')"
                name="plannedDurationMinutes"
              >
                <a-input-number
                  v-model:value="formState.plannedDurationMinutes"
                  :placeholder="pi.ph('plannedDurationMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeoverMinutes')"
                name="changeoverMinutes"
              >
                <a-input-number
                  v-model:value="formState.changeoverMinutes"
                  :placeholder="pi.ph('changeoverMinutes')"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('operationStatus')"
                name="operationStatus"
              >
                <TaktSelect
                  v-model:value="formState.operationStatus"
                  dict-type="aps_operation_status"
                  :placeholder="pi.ph('operationStatus')"
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
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * APS 排程订单子表 apsOperation 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/sales/order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useApsOperationI18n } from '../composables/useOperationI18n'

/** 实体字段 i18n */
const pi = useApsOperationI18n()

import type { ApsOperationCreate } from '@/types/logistics/manufacturing/aps/operation.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","routingItemId","processCode","processName","workCenterCode","workCenterResourceId","plannedStartTime","plannedEndTime","plannedDurationMinutes","changeoverMinutes","operationStatus","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ApsOperationCreate & { apsOperationId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 apsOperationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.apsOperationId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

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
  processCode: [
    {
      required: true,
      message: pi.ph('processCode'),
      trigger: 'blur'
    }
  ],
  plannedDurationMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('plannedDurationMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('plannedDurationMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  changeoverMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('changeoverMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('changeoverMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operationStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('operationStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('operationStatus'))
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

/** 映射为 Create/Update DTO（含主表外键 apsOrderId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('plannedDurationMinutes' in payload) {
    const rawplannedDurationMinutes = payload.plannedDurationMinutes
    payload.plannedDurationMinutes = typeof rawplannedDurationMinutes === 'number' ? rawplannedDurationMinutes : Number(rawplannedDurationMinutes)
  }
  if ('changeoverMinutes' in payload) {
    const rawchangeoverMinutes = payload.changeoverMinutes
    payload.changeoverMinutes = typeof rawchangeoverMinutes === 'number' ? rawchangeoverMinutes : Number(rawchangeoverMinutes)
  }
  if ('operationStatus' in payload) {
    const rawoperationStatus = payload.operationStatus
    payload.operationStatus = typeof rawoperationStatus === 'number' ? rawoperationStatus : Number(rawoperationStatus)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.apsOrderId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
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

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>

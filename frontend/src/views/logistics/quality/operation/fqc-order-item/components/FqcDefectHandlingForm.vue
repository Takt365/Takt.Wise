<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/fqc-order-item/components -->
<!-- 文件名称：FqcDefectHandlingForm.vue -->
<!-- 功能描述：FQC出货检验单明细实体子表 fqcDefectHandling 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form fqc-defect-handling-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="fqc-defect-handling-form-tabs"
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
                :label="pi.label('fqcDefectHandlingCode')"
                name="fqcDefectHandlingCode"
              >
                <a-input
                  v-model:value="formState.fqcDefectHandlingCode"
                  :placeholder="pi.ph('fqcDefectHandlingCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.fqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('fqcOrderCode')"
                name="fqcOrderCode"
              >
                <a-input
                  v-model:value="formState.fqcOrderCode"
                  :placeholder="pi.ph('fqcOrderCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.fqcDefectHandlingId"
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
                :label="pi.label('defectType')"
                name="defectType"
              >
                <TaktSelect
                  v-model:value="formState.defectType"
                  dict-type="logistics_quality_defect_type"
                  :placeholder="pi.ph('defectType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectCode')"
                name="defectCode"
              >
                <a-input
                  v-model:value="formState.defectCode"
                  :placeholder="pi.ph('defectCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.fqcDefectHandlingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('defectDescription')"
                name="defectDescription"
              >
                <a-textarea
                  v-model:value="formState.defectDescription"
                  :placeholder="pi.ph('defectDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectQuantity')"
                name="defectQuantity"
              >
                <a-input-number
                  v-model:value="formState.defectQuantity"
                  :placeholder="pi.ph('defectQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlingMethod')"
                name="handlingMethod"
              >
                <TaktSelect
                  v-model:value="formState.handlingMethod"
                  dict-type="logistics_quality_defect_handling_method"
                  :placeholder="pi.ph('handlingMethod')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('handlingDescription')"
                name="handlingDescription"
              >
                <a-textarea
                  v-model:value="formState.handlingDescription"
                  :placeholder="pi.ph('handlingDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('responsibleDept')"
                name="responsibleDept"
              >
                <TaktSelect
                  v-model:value="formState.responsibleDept"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('responsibleDept')"
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
                :label="pi.label('responsibleGroupCode')"
                name="responsibleGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.responsibleGroupCode"
                  api-url="TaktQualityGroups/options"
                  :api-params="{ qualityGroupCategory: 3 }"
                  :placeholder="pi.ph('responsibleGroupCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handlerGroupCode')"
                name="handlerGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.handlerGroupCode"
                  api-url="TaktQualityGroups/options"
                  :api-params="{ qualityGroupCategory: 4 }"
                  :placeholder="pi.ph('handlerGroupCode')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('verifyDeptId')"
                name="verifyDeptId"
              >
                <TaktSelect
                  v-model:value="formState.verifyDeptId"
                  api-url="TaktDepts/tree-options"
                  :placeholder="pi.ph('verifyDeptId')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('qualityOwnerGroupCode')"
                name="qualityOwnerGroupCode"
              >
                <TaktSelect
                  v-model:value="formState.qualityOwnerGroupCode"
                  api-url="TaktQualityGroups/options"
                  :api-params="{ qualityGroupCategory: 5 }"
                  :placeholder="pi.ph('qualityOwnerGroupCode')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('verifyAt')"
                name="verifyAt"
              >
                <a-date-picker
                  v-model:value="formState.verifyAt"
                  :placeholder="pi.ph('verifyAt')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('verifyStatus')"
                name="verifyStatus"
              >
                <TaktSelect
                  v-model:value="formState.verifyStatus"
                  dict-type="logistics_quality_defect_verify_status"
                  :placeholder="pi.ph('verifyStatus')"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('correctiveAction')"
                name="correctiveAction"
              >
                <a-input
                  v-model:value="formState.correctiveAction"
                  :placeholder="pi.ph('correctiveAction')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectImages')"
                name="defectImages"
              >
                <a-input
                  v-model:value="formState.defectImages"
                  :placeholder="pi.ph('defectImages')"
                  show-count
                  :maxlength="20"
                  allow-clear
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
                :label="pi.label('handlingStatus')"
                name="handlingStatus"
              >
                <TaktSelect
                  v-model:value="formState.handlingStatus"
                  dict-type="logistics_quality_defect_handling_status"
                  :placeholder="pi.ph('handlingStatus')"
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
 * FQC出货检验单明细实体子表 fqcDefectHandling 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/operation/fqc-order-item/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useFqcDefectHandlingI18n } from '../composables/useFqcDefectHandlingI18n'

/** 实体字段 i18n */
const pi = useFqcDefectHandlingI18n()

import type { FqcDefectHandlingCreate } from '@/types/logistics/quality/operation/fqcDefectHandling.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["fqcDefectHandlingCode","fqcOrderCode","lineNumber","defectType","defectCode","defectDescription","defectQuantity","handlingMethod","handlingDescription","responsibleDept","responsibleGroupCode","handlerGroupCode","handlingAt","verifyDeptId","qualityOwnerGroupCode","verifyAt","verifyStatus","correctiveAction","defectImages","attachments","handlingStatus","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<FqcDefectHandlingCreate & { fqcDefectHandlingId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  defectType: 0,
  handlingMethod: 0,
  handlingStatus: 0,
  verifyStatus: 0
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 fqcDefectHandlingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.fqcDefectHandlingId) {
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
  fqcDefectHandlingCode: [
    {
      required: true,
      message: pi.ph('fqcDefectHandlingCode'),
      trigger: 'blur'
    }
  ],
  fqcOrderCode: [
    {
      required: true,
      message: pi.ph('fqcOrderCode'),
      trigger: 'blur'
    }
  ],
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
  defectType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectCode: [
    {
      required: true,
      message: pi.ph('defectCode'),
      trigger: 'blur'
    }
  ],
  defectDescription: [
    {
      required: true,
      message: pi.ph('defectDescription'),
      trigger: 'blur'
    }
  ],
  defectQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingMethod: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('handlingMethod'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('handlingMethod'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  handlingStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('handlingStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('handlingStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  verifyStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('verifyStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('verifyStatus'))
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

/** 映射为 Create/Update DTO（含主表外键 fqcOrderItemId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('defectType' in payload) {
    const rawdefectType = payload.defectType
    payload.defectType = typeof rawdefectType === 'number' ? rawdefectType : Number(rawdefectType)
  }
  if ('defectQuantity' in payload) {
    const rawdefectQuantity = payload.defectQuantity
    payload.defectQuantity = typeof rawdefectQuantity === 'number' ? rawdefectQuantity : Number(rawdefectQuantity)
  }
  if ('handlingMethod' in payload) {
    const rawhandlingMethod = payload.handlingMethod
    payload.handlingMethod = typeof rawhandlingMethod === 'number' ? rawhandlingMethod : Number(rawhandlingMethod)
  }
  if ('handlingStatus' in payload) {
    const rawhandlingStatus = payload.handlingStatus
    payload.handlingStatus = typeof rawhandlingStatus === 'number' ? rawhandlingStatus : Number(rawhandlingStatus)
  }
  if ('verifyStatus' in payload) {
    const rawverifyStatus = payload.verifyStatus
    payload.verifyStatus = typeof rawverifyStatus === 'number' ? rawverifyStatus : Number(rawverifyStatus)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.fqcOrderItemId = props.masterId
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

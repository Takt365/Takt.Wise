<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/incident/components -->
<!-- 文件名称：IncidentItemForm.vue -->
<!-- 功能描述：品质事故主表子表 qualityIncidentItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form incident-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="incident-item-form-tabs"
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
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktGeneralMaterials/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.qualityIncidentItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('materialDescription')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="pi.ph('materialDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapCost')"
                name="scrapCost"
              >
                <a-input-number
                  v-model:value="formState.scrapCost"
                  :placeholder="pi.ph('scrapCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapSize')"
                name="scrapSize"
              >
                <a-input-number
                  v-model:value="formState.scrapSize"
                  :placeholder="pi.ph('scrapSize')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('partPrice')"
                name="partPrice"
              >
                <a-input-number
                  v-model:value="formState.partPrice"
                  :placeholder="pi.ph('partPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapReasonCost')"
                name="scrapReasonCost"
              >
                <a-input-number
                  v-model:value="formState.scrapReasonCost"
                  :placeholder="pi.ph('scrapReasonCost')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('freightCharges')"
                name="freightCharges"
              >
                <a-input-number
                  v-model:value="formState.freightCharges"
                  :placeholder="pi.ph('freightCharges')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('otherExpenses')"
                name="otherExpenses"
              >
                <a-input-number
                  v-model:value="formState.otherExpenses"
                  :placeholder="pi.ph('otherExpenses')"
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
                :label="pi.label('reasonWorkTimeMinutes')"
                name="reasonWorkTimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.reasonWorkTimeMinutes"
                  :placeholder="pi.ph('reasonWorkTimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tax')"
                name="tax"
              >
                <a-input-number
                  v-model:value="formState.tax"
                  :placeholder="pi.ph('tax')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('reasonOtherExpenses')"
                name="reasonOtherExpenses"
              >
                <a-input-number
                  v-model:value="formState.reasonOtherExpenses"
                  :placeholder="pi.ph('reasonOtherExpenses')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('scrapNote')"
                name="scrapNote"
              >
                <a-textarea
                  v-model:value="formState.scrapNote"
                  :placeholder="pi.ph('scrapNote')"
                  :rows="2"
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
        key="tab-2"
        :tab="t('common.page.form.tabs.basicinfo') + ' (3/3)'"
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
 * 品质事故主表子表 qualityIncidentItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/quality/cost/incident/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useQualityIncidentItemI18n } from '../composables/useIncidentItemI18n'

/** 实体字段 i18n */
const pi = useQualityIncidentItemI18n()

import type { QualityIncidentItemCreate } from '@/types/logistics/quality/cost/incidentItem.types'
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
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","lineNumber","materialCode","materialDescription","scrapCost","scrapSize","partPrice","scrapReasonCost","freightCharges","otherExpenses","reasonWorkTimeMinutes","tax","reasonOtherExpenses","scrapNote","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<QualityIncidentItemCreate & { qualityIncidentItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 qualityIncidentItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.qualityIncidentItemId) {
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
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.qualityIncidentItemId) {
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
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  materialDescription: [
    {
      required: true,
      message: pi.ph('materialDescription'),
      trigger: 'blur'
    }
  ],
  scrapCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scrapCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scrapCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scrapSize: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scrapSize'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scrapSize'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  partPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('partPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('partPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scrapReasonCost: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scrapReasonCost'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scrapReasonCost'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  freightCharges: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('freightCharges'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('freightCharges'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  otherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('otherExpenses'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('otherExpenses'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  reasonWorkTimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('reasonWorkTimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('reasonWorkTimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  tax: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('tax'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('tax'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  reasonOtherExpenses: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('reasonOtherExpenses'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('reasonOtherExpenses'))
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

/** 映射为 Create/Update DTO（含主表外键 qualityIncidentId） */
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
  if ('scrapCost' in payload) {
    const rawscrapCost = payload.scrapCost
    if (rawscrapCost === undefined || rawscrapCost === null || rawscrapCost === '') {
      delete payload.scrapCost
    } else {
      const numscrapCost = typeof rawscrapCost === 'number' ? rawscrapCost : Number(rawscrapCost)
      if (Number.isFinite(numscrapCost)) payload.scrapCost = numscrapCost
      else delete payload.scrapCost
    }
  }
  if ('scrapSize' in payload) {
    const rawscrapSize = payload.scrapSize
    if (rawscrapSize === undefined || rawscrapSize === null || rawscrapSize === '') {
      delete payload.scrapSize
    } else {
      const numscrapSize = typeof rawscrapSize === 'number' ? rawscrapSize : Number(rawscrapSize)
      if (Number.isFinite(numscrapSize)) payload.scrapSize = numscrapSize
      else delete payload.scrapSize
    }
  }
  if ('partPrice' in payload) {
    const rawpartPrice = payload.partPrice
    if (rawpartPrice === undefined || rawpartPrice === null || rawpartPrice === '') {
      delete payload.partPrice
    } else {
      const numpartPrice = typeof rawpartPrice === 'number' ? rawpartPrice : Number(rawpartPrice)
      if (Number.isFinite(numpartPrice)) payload.partPrice = numpartPrice
      else delete payload.partPrice
    }
  }
  if ('scrapReasonCost' in payload) {
    const rawscrapReasonCost = payload.scrapReasonCost
    if (rawscrapReasonCost === undefined || rawscrapReasonCost === null || rawscrapReasonCost === '') {
      delete payload.scrapReasonCost
    } else {
      const numscrapReasonCost = typeof rawscrapReasonCost === 'number' ? rawscrapReasonCost : Number(rawscrapReasonCost)
      if (Number.isFinite(numscrapReasonCost)) payload.scrapReasonCost = numscrapReasonCost
      else delete payload.scrapReasonCost
    }
  }
  if ('freightCharges' in payload) {
    const rawfreightCharges = payload.freightCharges
    if (rawfreightCharges === undefined || rawfreightCharges === null || rawfreightCharges === '') {
      delete payload.freightCharges
    } else {
      const numfreightCharges = typeof rawfreightCharges === 'number' ? rawfreightCharges : Number(rawfreightCharges)
      if (Number.isFinite(numfreightCharges)) payload.freightCharges = numfreightCharges
      else delete payload.freightCharges
    }
  }
  if ('otherExpenses' in payload) {
    const rawotherExpenses = payload.otherExpenses
    if (rawotherExpenses === undefined || rawotherExpenses === null || rawotherExpenses === '') {
      delete payload.otherExpenses
    } else {
      const numotherExpenses = typeof rawotherExpenses === 'number' ? rawotherExpenses : Number(rawotherExpenses)
      if (Number.isFinite(numotherExpenses)) payload.otherExpenses = numotherExpenses
      else delete payload.otherExpenses
    }
  }
  if ('reasonWorkTimeMinutes' in payload) {
    const rawreasonWorkTimeMinutes = payload.reasonWorkTimeMinutes
    if (rawreasonWorkTimeMinutes === undefined || rawreasonWorkTimeMinutes === null || rawreasonWorkTimeMinutes === '') {
      delete payload.reasonWorkTimeMinutes
    } else {
      const numreasonWorkTimeMinutes = typeof rawreasonWorkTimeMinutes === 'number' ? rawreasonWorkTimeMinutes : Number(rawreasonWorkTimeMinutes)
      if (Number.isFinite(numreasonWorkTimeMinutes)) payload.reasonWorkTimeMinutes = numreasonWorkTimeMinutes
      else delete payload.reasonWorkTimeMinutes
    }
  }
  if ('tax' in payload) {
    const rawtax = payload.tax
    if (rawtax === undefined || rawtax === null || rawtax === '') {
      delete payload.tax
    } else {
      const numtax = typeof rawtax === 'number' ? rawtax : Number(rawtax)
      if (Number.isFinite(numtax)) payload.tax = numtax
      else delete payload.tax
    }
  }
  if ('reasonOtherExpenses' in payload) {
    const rawreasonOtherExpenses = payload.reasonOtherExpenses
    if (rawreasonOtherExpenses === undefined || rawreasonOtherExpenses === null || rawreasonOtherExpenses === '') {
      delete payload.reasonOtherExpenses
    } else {
      const numreasonOtherExpenses = typeof rawreasonOtherExpenses === 'number' ? rawreasonOtherExpenses : Number(rawreasonOtherExpenses)
      if (Number.isFinite(numreasonOtherExpenses)) payload.reasonOtherExpenses = numreasonOtherExpenses
      else delete payload.reasonOtherExpenses
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

  if (props.formData?.qualityIncidentItemId) {
    payload.qualityIncidentItemId = props.formData.qualityIncidentItemId
  }
  payload.qualityIncidentId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.qualityIncidentCode ?? masterRow.QualityIncidentCode
    const masterName = masterRow.qualityIncidentName ?? masterRow.QualityIncidentName
    if (masterCode != null && masterCode !== '' && !payload.qualityIncidentCode) {
      payload.qualityIncidentCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.qualityIncidentName) {
      payload.qualityIncidentName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.qualityIncidentItemId)
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

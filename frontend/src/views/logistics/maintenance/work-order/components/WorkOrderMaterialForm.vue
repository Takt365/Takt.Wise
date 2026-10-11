<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：WorkOrderMaterialForm.vue -->
<!-- 功能描述：维护工单实体子表 maintenanceWorkOrderMaterial 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-order-material-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="work-order-material-form-tabs"
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
                :label="pi.label('materialId')"
                name="materialId"
              >
                <a-input
                  v-model:value="formState.materialId"
                  :placeholder="pi.ph('materialId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialCode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="pi.ph('materialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderMaterialId"
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
                :label="pi.label('requiredQuantity')"
                name="requiredQuantity"
              >
                <a-input-number
                  v-model:value="formState.requiredQuantity"
                  :placeholder="pi.ph('requiredQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('issuedQuantity')"
                name="issuedQuantity"
              >
                <a-input-number
                  v-model:value="formState.issuedQuantity"
                  :placeholder="pi.ph('issuedQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialUnit')"
                name="materialUnit"
              >
                <a-input
                  v-model:value="formState.materialUnit"
                  :placeholder="pi.ph('materialUnit')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unitPrice')"
                name="unitPrice"
              >
                <a-input-number
                  v-model:value="formState.unitPrice"
                  :placeholder="pi.ph('unitPrice')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('amount')"
                name="amount"
              >
                <a-input-number
                  v-model:value="formState.amount"
                  :placeholder="pi.ph('amount')"
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
                :label="pi.label('warehouseCode')"
                name="warehouseCode"
              >
                <a-input
                  v-model:value="formState.warehouseCode"
                  :placeholder="pi.ph('warehouseCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('storageLocation')"
                name="storageLocation"
              >
                <a-input
                  v-model:value="formState.storageLocation"
                  :placeholder="pi.ph('storageLocation')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('issueStatus')"
                name="issueStatus"
              >
                <a-input-number
                  v-model:value="formState.issueStatus"
                  :placeholder="pi.ph('issueStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('issueTime')"
                name="issueTime"
              >
                <a-date-picker
                  v-model:value="formState.issueTime"
                  :placeholder="pi.ph('issueTime')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
 * 维护工单实体子表 maintenanceWorkOrderMaterial 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/maintenance/work-order/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaintenanceWorkOrderMaterialI18n } from '../composables/useWorkOrderMaterialI18n'

/** 实体字段 i18n */
const pi = useMaintenanceWorkOrderMaterialI18n()

import type { MaintenanceWorkOrderMaterialCreate } from '@/types/logistics/maintenance/workOrderMaterial.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","lineNumber","materialId","materialCode","materialDescription","requiredQuantity","issuedQuantity","materialUnit","unitPrice","amount","warehouseCode","storageLocation","issueStatus","issueTime","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceWorkOrderMaterialCreate & { maintenanceWorkOrderMaterialId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceWorkOrderMaterialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceWorkOrderMaterialId) {
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
    if (!props.formData?.maintenanceWorkOrderMaterialId) {
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
  materialId: [
    {
      required: true,
      message: pi.ph('materialId'),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'blur'
    }
  ],
  materialDescription: [
    {
      required: true,
      message: pi.ph('materialDescription'),
      trigger: 'blur'
    }
  ],
  requiredQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('requiredQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('requiredQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issuedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('issuedQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('issuedQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialUnit: [
    {
      required: true,
      message: pi.ph('materialUnit'),
      trigger: 'blur'
    }
  ],
  unitPrice: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('unitPrice'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('unitPrice'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  amount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('amount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('amount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issueStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('issueStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('issueStatus'))
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

/** 映射为 Create/Update DTO（含主表外键 maintenanceWorkOrderId） */
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
  if ('requiredQuantity' in payload) {
    const rawrequiredQuantity = payload.requiredQuantity
    if (rawrequiredQuantity === undefined || rawrequiredQuantity === null || rawrequiredQuantity === '') {
      delete payload.requiredQuantity
    } else {
      const numrequiredQuantity = typeof rawrequiredQuantity === 'number' ? rawrequiredQuantity : Number(rawrequiredQuantity)
      if (Number.isFinite(numrequiredQuantity)) payload.requiredQuantity = numrequiredQuantity
      else delete payload.requiredQuantity
    }
  }
  if ('issuedQuantity' in payload) {
    const rawissuedQuantity = payload.issuedQuantity
    if (rawissuedQuantity === undefined || rawissuedQuantity === null || rawissuedQuantity === '') {
      delete payload.issuedQuantity
    } else {
      const numissuedQuantity = typeof rawissuedQuantity === 'number' ? rawissuedQuantity : Number(rawissuedQuantity)
      if (Number.isFinite(numissuedQuantity)) payload.issuedQuantity = numissuedQuantity
      else delete payload.issuedQuantity
    }
  }
  if ('unitPrice' in payload) {
    const rawunitPrice = payload.unitPrice
    if (rawunitPrice === undefined || rawunitPrice === null || rawunitPrice === '') {
      delete payload.unitPrice
    } else {
      const numunitPrice = typeof rawunitPrice === 'number' ? rawunitPrice : Number(rawunitPrice)
      if (Number.isFinite(numunitPrice)) payload.unitPrice = numunitPrice
      else delete payload.unitPrice
    }
  }
  if ('amount' in payload) {
    const rawamount = payload.amount
    if (rawamount === undefined || rawamount === null || rawamount === '') {
      delete payload.amount
    } else {
      const numamount = typeof rawamount === 'number' ? rawamount : Number(rawamount)
      if (Number.isFinite(numamount)) payload.amount = numamount
      else delete payload.amount
    }
  }
  if ('issueStatus' in payload) {
    const rawissueStatus = payload.issueStatus
    if (rawissueStatus === undefined || rawissueStatus === null || rawissueStatus === '') {
      delete payload.issueStatus
    } else {
      const numissueStatus = typeof rawissueStatus === 'number' ? rawissueStatus : Number(rawissueStatus)
      if (Number.isFinite(numissueStatus)) payload.issueStatus = numissueStatus
      else delete payload.issueStatus
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

  if (props.formData?.maintenanceWorkOrderMaterialId) {
    payload.maintenanceWorkOrderMaterialId = props.formData.maintenanceWorkOrderMaterialId
  }
  payload.maintenanceWorkOrderId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.maintenanceWorkOrderCode ?? masterRow.MaintenanceWorkOrderCode
    const masterName = masterRow.maintenanceWorkOrderName ?? masterRow.MaintenanceWorkOrderName
    if (masterCode != null && masterCode !== '' && !payload.maintenanceWorkOrderCode) {
      payload.maintenanceWorkOrderCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.maintenanceWorkOrderName) {
      payload.maintenanceWorkOrderName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.maintenanceWorkOrderMaterialId)
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

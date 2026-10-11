<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/assy-defect/components -->
<!-- 文件名称：AssyDefectDetailForm.vue -->
<!-- 功能描述：组立不良日报实体 不良率子表 assyDefectDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form assy-defect-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="assy-defect-detail-form-tabs"
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
                :label="pi.label('defectCategory')"
                name="defectCategory"
              >
                <TaktSelect
                  v-model:value="formState.defectCategory"
                  dict-type="logistics_manufacturing_defect_category"
                  :placeholder="pi.ph('defectCategory')"
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
                :label="pi.label('cumulativeDefectQty')"
                name="cumulativeDefectQty"
              >
                <a-input-number
                  v-model:value="formState.cumulativeDefectQty"
                  :placeholder="pi.ph('cumulativeDefectQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('randomCardCode')"
                name="randomCardCode"
              >
                <a-input
                  v-model:value="formState.randomCardCode"
                  :placeholder="pi.ph('randomCardCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.assyDefectDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('occurrenceEngineering')"
                name="occurrenceEngineering"
              >
                <a-input
                  v-model:value="formState.occurrenceEngineering"
                  :placeholder="pi.ph('occurrenceEngineering')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('testStep')"
                name="testStep"
              >
                <a-input
                  v-model:value="formState.testStep"
                  :placeholder="pi.ph('testStep')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectSymptom')"
                name="defectSymptom"
              >
                <a-input
                  v-model:value="formState.defectSymptom"
                  :placeholder="pi.ph('defectSymptom')"
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
                :label="pi.label('defectLocation')"
                name="defectLocation"
              >
                <TaktSelect
                  v-model:value="formState.defectLocation"
                  dict-type="logistics_manufacturing_assy_location_category"
                  :placeholder="pi.ph('defectLocation')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectReason')"
                name="defectReason"
              >
                <a-input
                  v-model:value="formState.defectReason"
                  :placeholder="pi.ph('defectReason')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repairOperator')"
                name="repairOperator"
              >
                <TaktSelect
                  v-model:value="formState.repairOperator"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('repairOperator')"
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
 * 组立不良日报实体 不良率子表 assyDefectDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/defect/assy-defect/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useAssyDefectDetailI18n } from '../composables/useAssyDefectDetailI18n'

/** 实体字段 i18n */
const pi = useAssyDefectDetailI18n()

import type { AssyDefectDetailCreate } from '@/types/logistics/manufacturing/defect/assyDefectDetail.types'
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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","defectCategory","defectQty","cumulativeDefectQty","randomCardCode","occurrenceEngineering","testStep","defectSymptom","defectLocation","defectReason","repairOperator","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssyDefectDetailCreate & { assyDefectDetailId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 assyDefectDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.assyDefectDetailId) {
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
    if (!props.formData?.assyDefectDetailId) {
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
  cumulativeDefectQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('cumulativeDefectQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('cumulativeDefectQty'))
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

/** 映射为 Create/Update DTO（含主表外键 assyDefectId） */
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
  if ('cumulativeDefectQty' in payload) {
    const rawcumulativeDefectQty = payload.cumulativeDefectQty
    if (rawcumulativeDefectQty === undefined || rawcumulativeDefectQty === null || rawcumulativeDefectQty === '') {
      delete payload.cumulativeDefectQty
    } else {
      const numcumulativeDefectQty = typeof rawcumulativeDefectQty === 'number' ? rawcumulativeDefectQty : Number(rawcumulativeDefectQty)
      if (Number.isFinite(numcumulativeDefectQty)) payload.cumulativeDefectQty = numcumulativeDefectQty
      else delete payload.cumulativeDefectQty
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
  if (props.formData?.assyDefectDetailId) {
    payload.assyDefectDetailId = props.formData.assyDefectDetailId
  }
  payload.assyDefectId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.assyDefectCode ?? masterRow.AssyDefectCode
    const masterName = masterRow.assyDefectName ?? masterRow.AssyDefectName
    if (masterCode != null && masterCode !== '' && !payload.assyDefectCode) {
      payload.assyDefectCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.assyDefectName) {
      payload.assyDefectName = masterName
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.assyDefectDetailId)
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

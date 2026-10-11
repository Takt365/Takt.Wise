<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：PcbaOutputDetailForm.vue -->
<!-- 功能描述：PCBA日报实体 达成率子表 pcbaOutputDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-detail-form-tabs"
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
                :label="pi.label('timePeriod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="pi.ph('timePeriod')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('teamCode')"
                name="teamCode"
              >
                <TaktSelect
                  v-model:value="formState.teamCode"
                  api-url="TaktProductionTeams/options"
                  :api-params="pcbaTeamOptionsParams"
                  :placeholder="pi.ph('teamCode')"
                  :disabled="!!formData?.pcbaOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodEquipCode')"
                name="prodEquipCode"
              >
                <TaktSelect
                  v-model:value="formState.prodEquipCode"
                  api-url="TaktProductionEquipments/options"
                  :placeholder="pi.ph('prodEquipCode')"
                  :disabled="!!formData?.pcbaOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('directLabor')"
                name="directLabor"
              >
                <a-input-number
                  v-model:value="formState.directLabor"
                  :placeholder="pi.ph('directLabor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectLabor')"
                name="indirectLabor"
              >
                <a-input-number
                  v-model:value="formState.indirectLabor"
                  :placeholder="pi.ph('indirectLabor')"
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
                :label="pi.label('stdShorts')"
                name="stdShorts"
              >
                <a-input-number
                  v-model:value="formState.stdShorts"
                  :placeholder="pi.ph('stdShorts')"
                  style="width: 100%"
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
                :label="pi.label('pcbBoardType')"
                name="pcbBoardType"
              >
                <TaktSelect
                  v-model:value="formState.pcbBoardType"
                  dict-type="logistics_manufacturing_pcba_function"
                  :placeholder="pi.ph('pcbBoardType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('panelSide')"
                name="panelSide"
              >
                <TaktSelect
                  v-model:value="formState.panelSide"
                  dict-type="logistics_manufacturing_pcba_side_category"
                  :placeholder="pi.ph('panelSide')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchQty')"
                name="batchQty"
              >
                <a-input-number
                  v-model:value="formState.batchQty"
                  :placeholder="pi.ph('batchQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
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
                :label="pi.label('serialCode')"
                name="serialCode"
              >
                <a-input
                  v-model:value="formState.serialCode"
                  :placeholder="pi.ph('serialCode')"
                  show-count
                  :maxlength="80"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectCount')"
                name="defectCount"
              >
                <a-input-number
                  v-model:value="formState.defectCount"
                  :placeholder="pi.ph('defectCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('downtimeMinutes')"
                name="downtimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.downtimeMinutes"
                  :placeholder="pi.ph('downtimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('downtimeReason')"
                name="downtimeReason"
              >
                <a-input
                  v-model:value="formState.downtimeReason"
                  :placeholder="pi.ph('downtimeReason')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('downtimeDescription')"
                name="downtimeDescription"
              >
                <a-textarea
                  v-model:value="formState.downtimeDescription"
                  :placeholder="pi.ph('downtimeDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repairMinutes')"
                name="repairMinutes"
              >
                <a-input-number
                  v-model:value="formState.repairMinutes"
                  :placeholder="pi.ph('repairMinutes')"
                  style="width: 100%"
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
                :label="pi.label('switchCount')"
                name="switchCount"
              >
                <a-input-number
                  v-model:value="formState.switchCount"
                  :placeholder="pi.ph('switchCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('switchTime')"
                name="switchTime"
              >
                <a-input-number
                  v-model:value="formState.switchTime"
                  :placeholder="pi.ph('switchTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stopTime')"
                name="stopTime"
              >
                <a-input-number
                  v-model:value="formState.stopTime"
                  :placeholder="pi.ph('stopTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalMinutes')"
                name="totalMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalMinutes"
                  :placeholder="pi.ph('totalMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unachievedReason')"
                name="unachievedReason"
              >
                <a-input
                  v-model:value="formState.unachievedReason"
                  :placeholder="pi.ph('unachievedReason')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('unachievedDescription')"
                name="unachievedDescription"
              >
                <a-textarea
                  v-model:value="formState.unachievedDescription"
                  :placeholder="pi.ph('unachievedDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('confirmMinutes')"
                name="confirmMinutes"
              >
                <a-input-number
                  v-model:value="formState.confirmMinutes"
                  :placeholder="pi.ph('confirmMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mixedProd')"
                name="mixedProd"
              >
                <a-input-number
                  v-model:value="formState.mixedProd"
                  :placeholder="pi.ph('mixedProd')"
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
 * PCBA日报实体 达成率子表 pcbaOutputDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaOutputDetailI18n } from '../composables/usePcbaOutputDetailI18n'
import type { PcbaOutputDetailCreate } from '@/types/logistics/manufacturing/output/pcbaOutputDetail.types'
import { buildPcbaProductionTeamOptionsParams } from '../../composables/productionTeamCategory'
import { usePcbaOutputDetailDictFormat } from '../composables/usePcbaOutputDetailDictFormat'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** 实体字段 i18n */
const pi = usePcbaOutputDetailI18n()
/** PCB板别等字典 Label/Value 转换 */
const { hydrateDetailDictFields, formatDetailDictFieldsForSubmit } = usePcbaOutputDetailDictFormat()

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
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","timePeriod","teamCode","prodEquipCode","directLabor","indirectLabor","shiftNo","stdShorts","pcbBoardType","panelSide","batchQty","dailyCompletedQty","serialCode","defectCount","downtimeMinutes","downtimeReason","downtimeDescription","repairMinutes","switchCount","switchTime","stopTime","totalMinutes","unachievedReason","unachievedDescription","confirmMinutes","mixedProd","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputDetailCreate & { pcbaOutputDetailId?: string }> | null
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

/** 班组下拉：按工厂 + PCBA 分类 */
const pcbaTeamOptionsParams = computed(() =>
  buildPcbaProductionTeamOptionsParams(String(formState.plantCode ?? '').trim()),
)
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputDetailId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      hydrateDetailDictFields(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
        hydrateDetailDictFields(formState)
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
    if (!props.formData?.pcbaOutputDetailId) {
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
  timePeriod: [
    {
      required: true,
      message: pi.ph('timePeriod'),
      trigger: 'blur'
    }
  ],
  teamCode: [
    {
      required: true,
      message: pi.ph('teamCode'),
      trigger: 'change'
    }
  ],
  prodEquipCode: [
    {
      required: true,
      message: pi.ph('prodEquipCode'),
      trigger: 'change'
    }
  ],
  directLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('directLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('directLabor'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('indirectLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('indirectLabor'))
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
  stdShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdShorts'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdShorts'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbBoardType: [
    {
      required: true,
      message: pi.ph('pcbBoardType'),
      trigger: 'blur'
    }
  ],
  panelSide: [
    {
      required: true,
      message: pi.ph('panelSide'),
      trigger: 'change'
    }
  ],
  batchQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('batchQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('batchQty'))
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
  serialCode: [
    {
      required: true,
      message: pi.ph('serialCode'),
      trigger: 'blur'
    }
  ],
  defectCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downtimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  repairMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('repairMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('repairMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  switchCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('switchCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('switchCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  switchTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('switchTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('switchTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stopTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stopTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stopTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  confirmMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('confirmMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('confirmMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mixedProd: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mixedProd'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mixedProd'))
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

/** 映射为 Create/Update DTO（含主表外键 pcbaOutputId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  formatDetailDictFieldsForSubmit(payload)
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
  if ('directLabor' in payload) {
    const rawdirectLabor = payload.directLabor
    if (rawdirectLabor === undefined || rawdirectLabor === null || rawdirectLabor === '') {
      delete payload.directLabor
    } else {
      const numdirectLabor = typeof rawdirectLabor === 'number' ? rawdirectLabor : Number(rawdirectLabor)
      if (Number.isFinite(numdirectLabor)) payload.directLabor = numdirectLabor
      else delete payload.directLabor
    }
  }
  if ('indirectLabor' in payload) {
    const rawindirectLabor = payload.indirectLabor
    if (rawindirectLabor === undefined || rawindirectLabor === null || rawindirectLabor === '') {
      delete payload.indirectLabor
    } else {
      const numindirectLabor = typeof rawindirectLabor === 'number' ? rawindirectLabor : Number(rawindirectLabor)
      if (Number.isFinite(numindirectLabor)) payload.indirectLabor = numindirectLabor
      else delete payload.indirectLabor
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
  if ('stdShorts' in payload) {
    const rawstdShorts = payload.stdShorts
    if (rawstdShorts === undefined || rawstdShorts === null || rawstdShorts === '') {
      delete payload.stdShorts
    } else {
      const numstdShorts = typeof rawstdShorts === 'number' ? rawstdShorts : Number(rawstdShorts)
      if (Number.isFinite(numstdShorts)) payload.stdShorts = numstdShorts
      else delete payload.stdShorts
    }
  }
  if ('batchQty' in payload) {
    const rawbatchQty = payload.batchQty
    if (rawbatchQty === undefined || rawbatchQty === null || rawbatchQty === '') {
      delete payload.batchQty
    } else {
      const numbatchQty = typeof rawbatchQty === 'number' ? rawbatchQty : Number(rawbatchQty)
      if (Number.isFinite(numbatchQty)) payload.batchQty = numbatchQty
      else delete payload.batchQty
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
  if ('defectCount' in payload) {
    const rawdefectCount = payload.defectCount
    if (rawdefectCount === undefined || rawdefectCount === null || rawdefectCount === '') {
      delete payload.defectCount
    } else {
      const numdefectCount = typeof rawdefectCount === 'number' ? rawdefectCount : Number(rawdefectCount)
      if (Number.isFinite(numdefectCount)) payload.defectCount = numdefectCount
      else delete payload.defectCount
    }
  }
  if ('downtimeMinutes' in payload) {
    const rawdowntimeMinutes = payload.downtimeMinutes
    if (rawdowntimeMinutes === undefined || rawdowntimeMinutes === null || rawdowntimeMinutes === '') {
      delete payload.downtimeMinutes
    } else {
      const numdowntimeMinutes = typeof rawdowntimeMinutes === 'number' ? rawdowntimeMinutes : Number(rawdowntimeMinutes)
      if (Number.isFinite(numdowntimeMinutes)) payload.downtimeMinutes = numdowntimeMinutes
      else delete payload.downtimeMinutes
    }
  }
  if ('repairMinutes' in payload) {
    const rawrepairMinutes = payload.repairMinutes
    if (rawrepairMinutes === undefined || rawrepairMinutes === null || rawrepairMinutes === '') {
      delete payload.repairMinutes
    } else {
      const numrepairMinutes = typeof rawrepairMinutes === 'number' ? rawrepairMinutes : Number(rawrepairMinutes)
      if (Number.isFinite(numrepairMinutes)) payload.repairMinutes = numrepairMinutes
      else delete payload.repairMinutes
    }
  }
  if ('switchCount' in payload) {
    const rawswitchCount = payload.switchCount
    if (rawswitchCount === undefined || rawswitchCount === null || rawswitchCount === '') {
      delete payload.switchCount
    } else {
      const numswitchCount = typeof rawswitchCount === 'number' ? rawswitchCount : Number(rawswitchCount)
      if (Number.isFinite(numswitchCount)) payload.switchCount = numswitchCount
      else delete payload.switchCount
    }
  }
  if ('switchTime' in payload) {
    const rawswitchTime = payload.switchTime
    if (rawswitchTime === undefined || rawswitchTime === null || rawswitchTime === '') {
      delete payload.switchTime
    } else {
      const numswitchTime = typeof rawswitchTime === 'number' ? rawswitchTime : Number(rawswitchTime)
      if (Number.isFinite(numswitchTime)) payload.switchTime = numswitchTime
      else delete payload.switchTime
    }
  }
  if ('stopTime' in payload) {
    const rawstopTime = payload.stopTime
    if (rawstopTime === undefined || rawstopTime === null || rawstopTime === '') {
      delete payload.stopTime
    } else {
      const numstopTime = typeof rawstopTime === 'number' ? rawstopTime : Number(rawstopTime)
      if (Number.isFinite(numstopTime)) payload.stopTime = numstopTime
      else delete payload.stopTime
    }
  }
  if ('totalMinutes' in payload) {
    const rawtotalMinutes = payload.totalMinutes
    if (rawtotalMinutes === undefined || rawtotalMinutes === null || rawtotalMinutes === '') {
      delete payload.totalMinutes
    } else {
      const numtotalMinutes = typeof rawtotalMinutes === 'number' ? rawtotalMinutes : Number(rawtotalMinutes)
      if (Number.isFinite(numtotalMinutes)) payload.totalMinutes = numtotalMinutes
      else delete payload.totalMinutes
    }
  }
  if ('confirmMinutes' in payload) {
    const rawconfirmMinutes = payload.confirmMinutes
    if (rawconfirmMinutes === undefined || rawconfirmMinutes === null || rawconfirmMinutes === '') {
      delete payload.confirmMinutes
    } else {
      const numconfirmMinutes = typeof rawconfirmMinutes === 'number' ? rawconfirmMinutes : Number(rawconfirmMinutes)
      if (Number.isFinite(numconfirmMinutes)) payload.confirmMinutes = numconfirmMinutes
      else delete payload.confirmMinutes
    }
  }
  if ('mixedProd' in payload) {
    const rawmixedProd = payload.mixedProd
    if (rawmixedProd === undefined || rawmixedProd === null || rawmixedProd === '') {
      delete payload.mixedProd
    } else {
      const nummixedProd = typeof rawmixedProd === 'number' ? rawmixedProd : Number(rawmixedProd)
      if (Number.isFinite(nummixedProd)) payload.mixedProd = nummixedProd
      else delete payload.mixedProd
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
  if (props.formData?.pcbaOutputDetailId) {
    payload.pcbaOutputDetailId = props.formData.pcbaOutputDetailId
  }
  payload.pcbaOutputId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.pcbaOutputCode ?? masterRow.PcbaOutputCode
    const masterName = masterRow.pcbaOutputName ?? masterRow.PcbaOutputName
    if (masterCode != null && masterCode !== '' && !payload.pcbaOutputCode) {
      payload.pcbaOutputCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.pcbaOutputName) {
      payload.pcbaOutputName = masterName
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
    hydrateDetailDictFields(formState)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.pcbaOutputDetailId)
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

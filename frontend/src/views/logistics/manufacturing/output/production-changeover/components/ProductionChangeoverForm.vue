<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-changeover/components -->
<!-- 文件名称：ProductionChangeoverForm.vue -->
<!-- 功能描述：生产切换记录实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
    :disabled="loading || isMasterProdDateLocked"
  >
    <a-alert
      v-if="isMasterProdDateLocked"
      type="warning"
      show-icon
      class="mb-3 shrink-0"
      :message="prodDateLockedAlertMessage"
    />
    <a-tabs
      v-model:active-key="activeTab"
      class="production-changeover-form-tabs"
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
                :label="pi.label('plantCode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="pi.ph('plantCode')"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodCategory')"
                name="prodCategory"
              >
                <TaktSelect
                  v-model:value="formState.prodCategory"
                  dict-type="logistics_manufacturing_prod_category"
                  :placeholder="pi.ph('prodCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeoverCategory')"
                name="changeoverCategory"
              >
                <TaktSelect
                  v-model:value="formState.changeoverCategory"
                  dict-type="logistics_manufacturing_changeover_category"
                  :placeholder="pi.ph('changeoverCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodDate')"
                name="prodDate"
              >
                <a-date-picker
                  v-model:value="formState.prodDate"
                  :placeholder="pi.ph('prodDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                  :disabled-date="prodDatePickerDisabledDate"
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
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('currentProdOrderCode')"
                name="currentProdOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.currentProdOrderCode"
                  api-url="TaktProductionOrders/options"
                  :placeholder="pi.ph('currentProdOrderCode')"
                  :disabled="!!formData?.productionChangeoverId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('currentModelCode')"
                name="currentModelCode"
              >
                <a-input
                  v-model:value="formState.currentModelCode"
                  :placeholder="pi.ph('currentModelCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeoverProdOrderCode')"
                name="changeoverProdOrderCode"
              >
                <TaktSelect
                  v-model:value="formState.changeoverProdOrderCode"
                  api-url="TaktProductionOrders/options"
                  :placeholder="pi.ph('changeoverProdOrderCode')"
                  :disabled="!!formData?.productionChangeoverId"
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
                :label="pi.label('changeoverModelCode')"
                name="changeoverModelCode"
              >
                <a-input
                  v-model:value="formState.changeoverModelCode"
                  :placeholder="pi.ph('changeoverModelCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeoverCount')"
                name="changeoverCount"
              >
                <a-input-number
                  v-model:value="formState.changeoverCount"
                  :placeholder="pi.ph('changeoverCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('changeoverTime')"
                name="changeoverTime"
              >
                <a-input-number
                  v-model:value="formState.changeoverTime"
                  :placeholder="pi.ph('changeoverTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('instrumentSetupTime')"
                name="instrumentSetupTime"
              >
                <a-input-number
                  v-model:value="formState.instrumentSetupTime"
                  :placeholder="pi.ph('instrumentSetupTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalChangeoverTime')"
                name="totalChangeoverTime"
              >
                <a-input-number
                  v-model:value="formState.totalChangeoverTime"
                  :placeholder="pi.ph('totalChangeoverTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('readSopTime')"
                name="readSopTime"
              >
                <a-input-number
                  v-model:value="formState.readSopTime"
                  :placeholder="pi.ph('readSopTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('learningTime')"
                name="learningTime"
              >
                <a-input-number
                  v-model:value="formState.learningTime"
                  :placeholder="pi.ph('learningTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('personCount')"
                name="personCount"
              >
                <a-input-number
                  v-model:value="formState.personCount"
                  :placeholder="pi.ph('personCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalLearningTime')"
                name="totalLearningTime"
              >
                <a-input-number
                  v-model:value="formState.totalLearningTime"
                  :placeholder="pi.ph('totalLearningTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalSopTime')"
                name="totalSopTime"
              >
                <a-input-number
                  v-model:value="formState.totalSopTime"
                  :placeholder="pi.ph('totalSopTime')"
                  style="width: 100%"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 生产切换记录实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/production-changeover/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useProductionChangeoverI18n } from '../composables/useProductionChangeoverI18n'
import type { ProductionChangeoverCreate } from '@/types/logistics/manufacturing/output/productionChangeover.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'
import { getProductionOrderByCode } from '@/api/logistics/manufacturing/aps/productionOrder.api'
import { getModelDestinationByMaterial } from '@/api/logistics/materials/modelDestination.api'
import {
  isOutputProdDateLocked,
  isOutputProdDateSelectable,
  outputProdDatePickerDisabledDate,
  resolveDefaultOutputProdDateYmd,
} from '../../composables/taktOutputProdDateEditLock'
import { useOutputProdDateI18n } from '../../composables/useOutputProdDateI18n'

/** 实体字段 i18n */
const pi = useProductionChangeoverI18n()
const prodDateI18n = useOutputProdDateI18n()

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言（登录或公司切换注入，表单只读）
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
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }

}
/** 表单内容区高度 class（多 Tab 大表单固定 10 行高度） */
const formContentClass = 'takt-form-content-rows-10'
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProductionChangeoverCreate & { productionChangeoverId?: string }> | null
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

/** 主表生产日期是否已锁定 */
const isMasterProdDateLocked = computed(() =>
  isOutputProdDateLocked(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 锁定提示文案 */
const prodDateLockedAlertMessage = computed(() =>
  prodDateI18n.prodDateLockedMessage(String(formState.prodDate ?? '').trim().slice(0, 10)),
)
/** 生产日期不可选已锁定/跨月/未来日期 */
function prodDatePickerDisabledDate(current: Parameters<typeof outputProdDatePickerDisabledDate>[0]) {
  return outputProdDatePickerDisabledDate(current)
}

/** 表单字段默认值 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (!target.prodDate) {
    target.prodDate = resolveDefaultOutputProdDateYmd()
  }
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 productionChangeoverId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.productionChangeoverId) {
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

/** 按当前/切换后工单回填工厂与机种（仅新增态） */
async function backfillFromProductionOrders() {
  if (props.formData?.productionChangeoverId) {
    return
  }
  const currentCode = String(formState.currentProdOrderCode ?? '').trim()
  if (currentCode) {
    try {
      const currentOrder = await getProductionOrderByCode(currentCode)
      if (currentOrder.plantCode) {
        formState.plantCode = currentOrder.plantCode
      }
      if (currentOrder.materialCode) {
        const model = await getModelDestinationByMaterial(currentOrder.materialCode)
        if (model?.modelCode) {
          formState.currentModelCode = model.modelCode
        }
      }
    } catch {
      // 工单不存在时保留用户已填内容
    }
  }
  const changeoverCode = String(formState.changeoverProdOrderCode ?? '').trim()
  if (changeoverCode) {
    try {
      const changeoverOrder = await getProductionOrderByCode(changeoverCode)
      if (changeoverOrder.materialCode) {
        const model = await getModelDestinationByMaterial(changeoverOrder.materialCode)
        if (model?.modelCode) {
          formState.changeoverModelCode = model.modelCode
        }
      }
    } catch {
      // 工单不存在时保留用户已填内容
    }
  }
}

watch(
  () => [formState.currentProdOrderCode, formState.changeoverProdOrderCode] as const,
  () => {
    void backfillFromProductionOrders()
  }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.productionChangeoverId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  changeoverCategory: [
    {
      required: true,
      message: pi.ph('changeoverCategory'),
      trigger: 'change'
    }
  ],
  prodDate: [
    {
      required: true,
      message: pi.ph('prodDate'),
      trigger: 'change'
    },
    {
      validator: async (_rule, value) => {
        const ymd = String(value ?? '').trim().slice(0, 10)
        if (!ymd) {
          return Promise.resolve()
        }
        if (isOutputProdDateLocked(ymd)) {
          return Promise.reject(prodDateI18n.prodDateLockedMessage(ymd))
        }
        if (!isOutputProdDateSelectable(ymd)) {
          return Promise.reject(prodDateI18n.prodDateOutOfRangeMessage())
        }
        return Promise.resolve()
      },
      trigger: 'change',
    }],
  currentProdOrderCode: [
    {
      required: true,
      message: pi.ph('currentProdOrderCode'),
      trigger: 'change'
    }
  ],
  changeoverProdOrderCode: [
    {
      required: true,
      message: pi.ph('changeoverProdOrderCode'),
      trigger: 'change'
    }
  ],
  changeoverCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('changeoverCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('changeoverCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  changeoverTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('changeoverTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('changeoverTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  instrumentSetupTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('instrumentSetupTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('instrumentSetupTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalChangeoverTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalChangeoverTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalChangeoverTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  readSopTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('readSopTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('readSopTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  learningTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('learningTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('learningTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  personCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('personCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('personCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalLearningTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalLearningTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalLearningTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalSopTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalSopTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalSopTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  if (isMasterProdDateLocked.value) {
    throw new Error(prodDateLockedAlertMessage.value)
  }
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('changeoverCount' in payload) {
    const rawchangeoverCount = payload.changeoverCount
    payload.changeoverCount = typeof rawchangeoverCount === 'number' ? rawchangeoverCount : Number(rawchangeoverCount)
  }
  if ('changeoverTime' in payload) {
    const rawchangeoverTime = payload.changeoverTime
    payload.changeoverTime = typeof rawchangeoverTime === 'number' ? rawchangeoverTime : Number(rawchangeoverTime)
  }
  if ('instrumentSetupTime' in payload) {
    const rawinstrumentSetupTime = payload.instrumentSetupTime
    payload.instrumentSetupTime = typeof rawinstrumentSetupTime === 'number' ? rawinstrumentSetupTime : Number(rawinstrumentSetupTime)
  }
  if ('totalChangeoverTime' in payload) {
    const rawtotalChangeoverTime = payload.totalChangeoverTime
    payload.totalChangeoverTime = typeof rawtotalChangeoverTime === 'number' ? rawtotalChangeoverTime : Number(rawtotalChangeoverTime)
  }
  if ('readSopTime' in payload) {
    const rawreadSopTime = payload.readSopTime
    payload.readSopTime = typeof rawreadSopTime === 'number' ? rawreadSopTime : Number(rawreadSopTime)
  }
  if ('learningTime' in payload) {
    const rawlearningTime = payload.learningTime
    payload.learningTime = typeof rawlearningTime === 'number' ? rawlearningTime : Number(rawlearningTime)
  }
  if ('personCount' in payload) {
    const rawpersonCount = payload.personCount
    payload.personCount = typeof rawpersonCount === 'number' ? rawpersonCount : Number(rawpersonCount)
  }
  if ('totalLearningTime' in payload) {
    const rawtotalLearningTime = payload.totalLearningTime
    payload.totalLearningTime = typeof rawtotalLearningTime === 'number' ? rawtotalLearningTime : Number(rawtotalLearningTime)
  }
  if ('totalSopTime' in payload) {
    const rawtotalSopTime = payload.totalSopTime
    payload.totalSopTime = typeof rawtotalSopTime === 'number' ? rawtotalSopTime : Number(rawtotalSopTime)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.productionChangeoverId)

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

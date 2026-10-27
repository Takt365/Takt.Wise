<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/components -->
<!-- 文件名称：EcDetailForm.vue -->
<!-- 功能描述：设变子表 ecDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <TaktFormTabs
      v-model:active-key="activeTab"
      class="ec-detail-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('tenantCode') })"
                  show-count
                  :maxlength="3"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('companyCode') })"
                  show-count
                  :maxlength="4"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <a-input
                  v-model:value="formState.cultureCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('cultureCode') })"
                  show-count
                  :maxlength="5"
                  disabled
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
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('plantCode') })"
                  show-count
                  :maxlength="4"
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
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('lineNumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecCode')"
                name="ecCode"
              >
                <a-input
                  v-model:value="formState.ecCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecCode') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecModelCode')"
                name="ecModelCode"
              >
                <a-input
                  v-model:value="formState.ecModelCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecModelCode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecRootMaterialType')"
                name="ecRootMaterialType"
              >
                <TaktSelect
                  v-model:value="formState.ecRootMaterialType"
                  dict-type="logistics_materials_material_type"
                  disabled
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecRootMaterialType') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecRootMaterialCode')"
                name="ecRootMaterialCode"
              >
                <a-input
                  v-model:value="formState.ecRootMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecRootMaterialCode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecRootMaterialDescription')"
                name="ecRootMaterialDescription"
              >
                <a-input
                  v-model:value="formState.ecRootMaterialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecRootMaterialDescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('discontinuedStatus')"
                name="discontinuedStatus"
              >
                <TaktSelect
                  v-model:value="formState.discontinuedStatus"
                  dict-type="logistics_materials_material_discontinued_status"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('discontinuedStatus') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecParentMaterialCode')"
                name="ecParentMaterialCode"
              >
                <a-input
                  v-model:value="formState.ecParentMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecParentMaterialCode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecParentMaterialDescription')"
                name="ecParentMaterialDescription"
              >
                <a-input
                  v-model:value="formState.ecParentMaterialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecParentMaterialDescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('logistics.manufacturing.engineering-change.ec-te.page.tabs.oldNewMaterial')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldMaterialCode')"
                name="ecOldMaterialCode"
              >
                <a-input
                  v-model:value="formState.ecOldMaterialCode"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldMaterialCode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldMaterialDescription')"
                name="ecOldMaterialDescription"
              >
                <a-input
                  v-model:value="formState.ecOldMaterialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldMaterialDescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldUsageQuantity')"
                name="ecOldUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.ecOldUsageQuantity"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldUsageQuantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldItemPosition')"
                name="ecOldItemPosition"
              >
                <a-input
                  v-model:value="formState.ecOldItemPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldItemPosition') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldStock')"
                name="ecOldStock"
              >
                <a-input-number
                  v-model:value="formState.ecOldStock"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldStock') })"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldWarehouse')"
                name="ecOldWarehouse"
              >
                <TaktSelect
                  v-model:value="formState.ecOldWarehouse"
                  api-url="TaktWarehouses/options"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldWarehouse') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldPurchaseType')"
                name="ecOldPurchaseType"
              >
                <TaktSelect
                  v-model:value="formState.ecOldPurchaseType"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldPurchaseType') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecOldRequiresInspection')"
                name="ecOldRequiresInspection"
              >
                <TaktSelect
                  v-model:value="formState.ecOldRequiresInspection"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldRequiresInspection') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewMaterialCode')"
                name="ecNewMaterialCode"
              >
                <a-input
                  v-model:value="formState.ecNewMaterialCode"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewMaterialCode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewMaterialDescription')"
                name="ecNewMaterialDescription"
              >
                <a-input
                  v-model:value="formState.ecNewMaterialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewMaterialDescription') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewUsageQuantity')"
                name="ecNewUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.ecNewUsageQuantity"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewUsageQuantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewItemPosition')"
                name="ecNewItemPosition"
              >
                <a-input
                  v-model:value="formState.ecNewItemPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewItemPosition') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewStock')"
                name="ecNewStock"
              >
                <a-input-number
                  v-model:value="formState.ecNewStock"
                  :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewStock') })"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewWarehouse')"
                name="ecNewWarehouse"
              >
                <TaktSelect
                  v-model:value="formState.ecNewWarehouse"
                  api-url="TaktWarehouses/options"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewWarehouse') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewPurchaseType')"
                name="ecNewPurchaseType"
              >
                <TaktSelect
                  v-model:value="formState.ecNewPurchaseType"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewPurchaseType') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecNewRequiresInspection')"
                name="ecNewRequiresInspection"
              >
                <TaktSelect
                  v-model:value="formState.ecNewRequiresInspection"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewRequiresInspection') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('ecBomDate')"
                name="ecBomDate"
              >
                <a-date-picker
                  v-model:value="formState.ecBomDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecBomDate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
    </TaktFormTabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 设变子表 ecDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/engineering-change/ec-te/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcDetailCreate } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types'
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcDetailI18n'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()
const pi = useEcDetailI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","lineNumber","ecCode","ecModelCode","ecRootMaterialType","ecRootMaterialCode","ecRootMaterialDescription","discontinuedStatus","ecParentMaterialCode","ecParentMaterialDescription","ecOldMaterialCode","ecOldMaterialDescription","ecOldUsageQuantity","ecOldItemPosition","ecOldStock","ecOldWarehouse","ecOldPurchaseType","ecOldRequiresInspection","ecNewMaterialCode","ecNewMaterialDescription","ecNewUsageQuantity","ecNewItemPosition","ecNewStock","ecNewWarehouse","ecNewPurchaseType","ecNewRequiresInspection","ecBomDate"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcDetailCreate & { ecDetailId?: string }> | null
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

/**
 * 上下文隔离字段：租户 / 公司 / 区域文化 / 工厂
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecDetailId) {
      const next = { ...val } as Record<string, unknown>
      if (!next.ecCode && next.ecNo) {
        next.ecCode = next.ecNo
      }
      Object.keys(formState).forEach((k) => delete formState[k])
      applyScopeDefaults(next)
      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        if (!next.ecCode && next.ecNo) {
          next.ecCode = next.ecNo
        }
        Object.assign(formState, next)
      }
      applyScopeDefaults(formState as Record<string, unknown>, true)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture, userStore.userInfo?.cultureCode, tenantStore.currentCompanyRelatedPlant] as const,
  () => {
    if (!props.formData?.ecDetailId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: pi.label('ecCode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: pi.label('lineNumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: pi.label('lineNumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecModelCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: pi.label('ecModelCode') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 ecTeId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('ecOldUsageQuantity' in payload && payload.ecOldUsageQuantity !== undefined && payload.ecOldUsageQuantity !== null && payload.ecOldUsageQuantity !== '') {
    const raw = payload.ecOldUsageQuantity
    payload.ecOldUsageQuantity = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecNewUsageQuantity' in payload && payload.ecNewUsageQuantity !== undefined && payload.ecNewUsageQuantity !== null && payload.ecNewUsageQuantity !== '') {
    const raw = payload.ecNewUsageQuantity
    payload.ecNewUsageQuantity = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecOldStock' in payload && payload.ecOldStock !== undefined && payload.ecOldStock !== null && payload.ecOldStock !== '') {
    const raw = payload.ecOldStock
    payload.ecOldStock = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecNewStock' in payload && payload.ecNewStock !== undefined && payload.ecNewStock !== null && payload.ecNewStock !== '') {
    const raw = payload.ecNewStock
    payload.ecNewStock = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.ecTeId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.ecDetailId)
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

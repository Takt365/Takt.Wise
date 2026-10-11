<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/cost-element/components -->
<!-- 文件名称：CostElementForm.vue -->
<!-- 功能描述：成本要素实体树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="cost-element-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="pi.label('parentId')"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="TaktCostElements/tree-options"
                  :lazy="true"
                  :placeholder="pi.ph('parentId')"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
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
                :label="pi.label('costElementCode')"
                name="costElementCode"
              >
                <a-input
                  v-model:value="formState.costElementCode"
                  :placeholder="pi.ph('costElementCode')"
                  show-count
                  :maxlength="4"
                  allow-clear
                  :disabled="!!formData?.costElementId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementName')"
                name="costElementName"
              >
                <a-input
                  v-model:value="formState.costElementName"
                  :placeholder="pi.ph('costElementName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementType')"
                name="costElementType"
              >
                <TaktSelect
                  v-model:value="formState.costElementType"
                  dict-type="accounting_controlling_cost_element_type"
                  :placeholder="pi.ph('costElementType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementCategory')"
                name="costElementCategory"
              >
                <TaktSelect
                  v-model:value="formState.costElementCategory"
                  dict-type="accounting_controlling_cost_element_category"
                  :placeholder="pi.ph('costElementCategory')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementLevel')"
                name="costElementLevel"
              >
                <a-input-number
                  v-model:value="formState.costElementLevel"
                  :placeholder="pi.ph('costElementLevel')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('validFrom')"
                name="validFrom"
              >
                <a-date-picker
                  v-model:value="formState.validFrom"
                  :placeholder="pi.ph('validFrom')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('validTo')"
                name="validTo"
              >
                <a-date-picker
                  v-model:value="formState.validTo"
                  :placeholder="pi.ph('validTo')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('costElementStatus')"
                name="costElementStatus"
              >
                <TaktSelect
                  v-model:value="formState.costElementStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('costElementStatus')"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 成本要素实体维护表单 · 由 generate-vue-tree-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/cost-element/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCostElementI18n } from '../composables/useCostElementI18n'

/** 实体字段 i18n */
const pi = useCostElementI18n()

import type { CostElementCreate } from '@/types/accounting/controlling/costElement.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { useUserStore } from '@/stores/identity/useUserStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/**
 * 上下文隔离字段：租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或公司切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (formFields.includes('tenantCode') && (force || !target.tenantCode)) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (formFields.includes('companyCode') && (force || !target.companyCode)) {
    target.companyCode = tenantStore.companyCode
  }
  if (formFields.includes('cultureCode') && (force || !target.cultureCode)) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }
  if (formFields.includes('plantCode') && (force || !target.plantCode)) {
    target.plantCode = tenantStore.currentCompanyRelatedPlant || ''
  }
  if (formFields.includes('relatedPlant') && (force || !target.relatedPlant)) {
    target.relatedPlant = tenantStore.currentCompanyRelatedPlant || ''
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","companyCode","cultureCode","plantCode","costElementCode","costElementName","costElementType","costElementCategory","costElementLevel","validFrom","validTo","costElementStatus","extField","remark"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CostElementCreate & { costElementId?: string }> | null
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
const formState = reactive<Record<string, any>>({ parentId: '0' })
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  costElementType: 0,
  costElementCategory: 1,
  costElementStatus: 1
}

/** 树表 parentId：空值归一为根节点 0（string，与后端 ParentId=0 一致） */
function normalizeTreeParentId(target: Record<string, unknown>) {
  const raw = target.parentId
  target.parentId = raw === '' || raw === undefined || raw === null ? '0' : String(raw)
}
/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 costElementId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.costElementId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      applyScopeDefaults(next)
      Object.assign(formState, next)
      normalizeTreeParentId(formState)
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
    const isCreate = !props.formData?.costElementId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  parentId: [
    {
      required: true,
      message: pi.ph('parentId'),
      trigger: 'change'
    }
  ],
  plantCode: [
    {
      required: true,
      message: pi.ph('plantCode'),
      trigger: 'change'
    }
  ],
  costElementCode: [
    {
      required: true,
      message: pi.ph('costElementCode'),
      trigger: 'blur'
    }
  ],
  costElementName: [
    {
      required: true,
      message: pi.ph('costElementName'),
      trigger: 'blur'
    }
  ],
  costElementType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('costElementType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('costElementType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  costElementCategory: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('costElementCategory'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('costElementCategory'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  costElementLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('costElementLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('costElementLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  validFrom: [
    {
      required: true,
      message: pi.ph('validFrom'),
      trigger: 'change'
    }
  ],
  validTo: [
    {
      required: true,
      message: pi.ph('validTo'),
      trigger: 'change'
    }
  ],
  costElementStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('costElementStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('costElementStatus'))
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

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('costElementType' in payload) {
    const rawcostElementType = payload.costElementType
    payload.costElementType = typeof rawcostElementType === 'number' ? rawcostElementType : Number(rawcostElementType)
  }
  if ('costElementCategory' in payload) {
    const rawcostElementCategory = payload.costElementCategory
    payload.costElementCategory = typeof rawcostElementCategory === 'number' ? rawcostElementCategory : Number(rawcostElementCategory)
  }
  if ('costElementLevel' in payload) {
    const rawcostElementLevel = payload.costElementLevel
    payload.costElementLevel = typeof rawcostElementLevel === 'number' ? rawcostElementLevel : Number(rawcostElementLevel)
  }
  if ('costElementStatus' in payload) {
    const rawcostElementStatus = payload.costElementStatus
    payload.costElementStatus = typeof rawcostElementStatus === 'number' ? rawcostElementStatus : Number(rawcostElementStatus)
  }
  const parentRaw = payload.parentId
  const parentId = parentRaw === '' || parentRaw === undefined || parentRaw === null ? '0' : String(parentRaw)
  payload.parentId = parentId
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload
}

/** 重置表单与子表行 */
/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.costElementId)

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

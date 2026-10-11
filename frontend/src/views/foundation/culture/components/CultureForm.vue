<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/culture/components -->
<!-- 文件名称：CultureForm.vue -->
<!-- 功能描述：区域文化实体 定义系统支持的多语言区域文化维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form culture-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="culture-form-tabs"
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
                :label="pi.label('cultureCode')"
                name="cultureCode"
              >
                <TaktSelect
                  v-model:value="formState.cultureCode"
                  dict-type="sys_culture_code"
                  :placeholder="pi.ph('cultureCode')"
                  :disabled="!!formData?.cultureId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('nativeName')"
                name="nativeName"
              >
                <a-input
                  v-model:value="formState.nativeName"
                  :placeholder="pi.ph('nativeName')"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('icon')"
                name="icon"
              >
                <a-input
                  v-model:value="formState.icon"
                  :placeholder="pi.ph('icon')"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isDefault')"
                name="isDefault"
              >
                <TaktSelect
                  v-model:value="formState.isDefault"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isDefault')"
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
    <!-- 下：子表 translationList -->
    <TaktEditableTable
      ref="translationTableRef"
      v-model="childTranslationRows"
      :columns="translationFormColumns"
      :title="translationPi.self()"
      :add-button-entity="translationPi.self()"
      id-field="translationId"
      :default-row="createDefaultTranslationRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-resourceGroup="{ record }">
        <TaktSelect
          v-model:value="record.resourceGroup"
          api-url="TaktMenus/tree-options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="translationPi.queryPh('resourceGroup', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-resourceType="{ record }">
        <TaktSelect
          v-model:value="record.resourceType"
          dict-type="sys_resource_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="translationPi.ph('resourceType')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 区域文化实体 定义系统支持的多语言区域文化维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/foundation/culture/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useCultureI18n } from '../composables/useCultureI18n'

/** 实体字段 i18n */
const pi = useCultureI18n()

import type { CultureCreate } from '@/types/foundation/culture.types'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：仅租户（TaktTenantCoreEntityBase，无工厂/无语言隔离）
 * cultureCode 为业务字段（本条区域文化编码），不从当前用户语言注入
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
}
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["tenantCode","cultureCode","nativeName","icon","isDefault","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useTranslationI18n } from '../composables/useTranslationI18n'

const translationPi = useTranslationI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childTranslationRows = ref<Record<string, unknown>[]>([])
const translationTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 translation 可编辑列 */
const translationFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'i18nKey',
    title: translationPi.label('i18nKey'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'translationText',
    title: translationPi.label('translationText'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'resourceGroup',
    title: translationPi.label('resourceGroup'),
    width: 140,
  },
  {
    key: 'resourceType',
    title: translationPi.label('resourceType'),
    width: 140,
  },
  {
    key: 'contextNote',
    title: translationPi.label('contextNote'),
    editor: 'textarea',
    rows: 1,
    placeholder: translationPi.ph('contextNote'),
    width: 180,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<CultureCreate & { cultureId?: string }> | null | undefined) {
  const rows_translation = ((val as any)?.translationList ?? []) as Record<string, unknown>[]
  childTranslationRows.value = rows_translation
}

function createDefaultTranslationRow(): Record<string, unknown> {
  return {
    tenantCode: tenantStore.tenantCode,
    i18nKey: '',
    translationText: '',
    resourceGroup: '',
    resourceType: '',
    contextNote: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.cultureId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    translationList: translationTableRef.value?.getRows?.() ?? childTranslationRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      cultureId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<CultureCreate & { cultureId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 cultureId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.cultureId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).translationList
      applyScopeDefaults(next)
      Object.assign(formState, next)
    syncChildRowsFromFormData(val)
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

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.cultureId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  cultureCode: [
    {
      required: true,
      message: pi.ph('cultureCode'),
      trigger: 'change'
    }
  ],
  nativeName: [
    {
      required: true,
      message: pi.ph('nativeName'),
      trigger: 'blur'
    }
  ],
  isDefault: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isDefault'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isDefault'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await translationTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('isDefault' in payload) {
    const rawisDefault = payload.isDefault
    if (rawisDefault === undefined || rawisDefault === null || rawisDefault === '') {
      delete payload.isDefault
    } else {
      const numisDefault = typeof rawisDefault === 'number' ? rawisDefault : Number(rawisDefault)
      if (Number.isFinite(numisDefault)) payload.isDefault = numisDefault
      else delete payload.isDefault
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.cultureId) {
    payload.cultureId = props.formData.cultureId
  }
  return payload
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.cultureId)
  childTranslationRows.value = []
  translationTableRef.value?.resetRows?.()
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

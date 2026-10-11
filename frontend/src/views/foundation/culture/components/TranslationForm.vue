<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/culture/components -->
<!-- 文件名称：TranslationForm.vue -->
<!-- 功能描述：区域文化实体 定义系统支持的多语言区域文化子表 translation 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form translation-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="translation-form-tabs"
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
                :label="pi.label('i18nKey')"
                name="i18nKey"
              >
                <a-input
                  v-model:value="formState.i18nKey"
                  :placeholder="pi.ph('i18nKey')"
                  show-count
                  :maxlength="140"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('translationText')"
                name="translationText"
              >
                <a-input
                  v-model:value="formState.translationText"
                  :placeholder="pi.ph('translationText')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resourceGroup')"
                name="resourceGroup"
              >
                <TaktSelect
                  v-model:value="formState.resourceGroup"
                  api-url="TaktMenus/tree-options"
                  :placeholder="pi.ph('resourceGroup')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('resourceType')"
                name="resourceType"
              >
                <TaktSelect
                  v-model:value="formState.resourceType"
                  dict-type="sys_resource_type"
                  :placeholder="pi.ph('resourceType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('contextNote')"
                name="contextNote"
              >
                <a-textarea
                  v-model:value="formState.contextNote"
                  :placeholder="pi.ph('contextNote')"
                  :rows="2"
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 区域文化实体 定义系统支持的多语言区域文化子表 translation 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/foundation/culture/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useTranslationI18n } from '../composables/useTranslationI18n'

/** 实体字段 i18n */
const pi = useTranslationI18n()

import type { TranslationCreate } from '@/types/foundation/translation.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：仅租户（TaktTenantCoreEntityBase，无工厂/无语言隔离）
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
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
const formFields = ["tenantCode","i18nKey","translationText","resourceGroup","resourceType","contextNote"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<TranslationCreate & { translationId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（回填 cultureId；翻译为 TenantCore，无工厂隔离） */
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 translationId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.translationId) {
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

/** 租户切换时，新增态表单同步隔离字段 */
watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.translationId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  i18nKey: [
    {
      required: true,
      message: pi.ph('i18nKey'),
      trigger: 'blur'
    }
  ],
  translationText: [
    {
      required: true,
      message: pi.ph('translationText'),
      trigger: 'blur'
    }
  ],
  resourceGroup: [
    {
      required: true,
      message: pi.ph('resourceGroup'),
      trigger: 'change'
    }
  ],
  resourceType: [
    {
      required: true,
      message: pi.ph('resourceType'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（外键 cultureId；租户由上下文注入） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (props.formData?.translationId) {
    payload.translationId = props.formData.translationId
  }
  payload.cultureId = props.masterId
  payload.tenantCode = tenantStore.tenantCode
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCode != null && masterCode !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCode
    }
  }
  delete payload.relatedPlant
  delete payload.plantCode
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.translationId)
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

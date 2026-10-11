<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/dict-type/components -->
<!-- 文件名称：DictDataForm.vue -->
<!-- 功能描述：字典数据子表独立 CRUD 弹窗表单（TaktTenantCultureEntityBase：租户 + cultureCode，无工厂）。defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form dict-data-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="dict-data-form-tabs"
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
                  :placeholder="pi.ph('tenantCode')"
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
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dictLabel')"
                name="dictLabel"
              >
                <a-input
                  v-model:value="formState.dictLabel"
                  :placeholder="pi.ph('dictLabel')"
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dictValue')"
                name="dictValue"
              >
                <a-input
                  v-model:value="formState.dictValue"
                  :placeholder="pi.ph('dictValue')"
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('i18nKey')"
                name="i18nKey"
              >
                <a-input
                  v-model:value="formState.i18nKey"
                  :placeholder="pi.ph('i18nKey')"
                  :maxlength="140"
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
 * 字典数据子表维护表单（租户 + 语言隔离；CultureCode 默认 mul，不回填公司 UI 语言）
 * @module views/foundation/dict-type/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useDictDataI18n } from '../composables/useDictDataI18n'
import type { DictDataCreate } from '@/types/foundation/dictData.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** 实体字段 i18n */
const pi = useDictDataI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** 租户上下文 */
const tenantStore = useTenantStore()
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode', 'cultureCode', 'dictLabel', 'dictValue', 'i18nKey']
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DictDataCreate & { dictDataId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（回填 dictTypeCode） */
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

/**
 * 新增默认值：租户只读注入；语言隔离默认 mul（多种语言内容）
 * @param target 表单模型
 */
function applyFormDefaults(target: Record<string, unknown>) {
  if (!target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }
  if (!target.cultureCode) {
    target.cultureCode = 'mul'
  }
}

/** 编辑态灌入 formData；新增态恢复默认值（须含 dictDataId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.dictDataId) {
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

watch(
  () => tenantStore.tenantCode,
  () => {
    if (!props.formData?.dictDataId) {
      applyFormDefaults(formState)
    }
  }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  cultureCode: [{ required: true, message: pi.ph('cultureCode'), trigger: 'change' }],
  dictLabel: [{ required: true, message: pi.ph('dictLabel'), trigger: 'blur' }],
  dictValue: [{ required: true, message: pi.ph('dictValue'), trigger: 'blur' }],
  i18nKey: [{ required: true, message: pi.ph('i18nKey'), trigger: 'blur' }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（外键 dictTypeId；语言默认 mul） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('sortOrder' in payload) delete payload.sortOrder
  if (props.formData?.dictDataId) {
    payload.dictDataId = props.formData.dictDataId
  }
  payload.dictTypeId = props.masterId
  payload.tenantCode = payload.tenantCode || tenantStore.tenantCode
  payload.cultureCode = payload.cultureCode || 'mul'
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.dictTypeCode ?? masterRow.DictTypeCode
    if (masterCode != null && masterCode !== '' && !payload.dictTypeCode) {
      payload.dictTypeCode = masterCode
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

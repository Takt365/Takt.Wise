<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise  -->
<!-- 命名空间：@/views/routine/dict/components -->
<!-- 文件名称：DictDataForm.vue -->
<!-- 创建时间：2025-01-20 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：字典数据表单组件 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="dict-data-form">
    <a-form
      ref="formRef"
      class="takt-generated-form"
      :model="formState"
      :rules="formRulesComputed"
      layout="horizontal"
      label-align="right"
    >
      <!-- 表单字段顺序与 DictData 接口字段顺序一致 -->
      <a-form-item
        :label="t('entity.dictdata.dicttypecode')"
        name="dictTypeCode"
      >
        <a-input
          v-model:value="formState.dictTypeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dicttype.code') })"
          :disabled="true"
        />
      </a-form-item>

      <a-form-item
        :label="t('common.page.entity.tenantcode')"
        name="tenantCode"
      >
        <a-input
          v-model:value="formState.tenantCode"
          disabled
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.culturecode')"
        name="cultureCode"
      >
        <TaktSelect
          v-model:value="formState.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.dictdata.culturecode') })"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.dictlabel')"
        name="dictLabel"
      >
        <a-input
          v-model:value="formState.dictLabel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dictdata.dictlabel') })"
          :maxlength="40"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.i18nkey')"
        name="i18nKey"
      >
        <a-input
          v-model:value="formState.i18nKey"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.dictdata.i18nkey') })"
          :maxlength="140"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.dictvalue')"
        name="dictValue"
      >
        <a-input
          v-model:value="formState.dictValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.dictdata.dictvalue') })"
          :maxlength="40"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.cssclass')"
        name="cssClass"
      >
        <a-input-number
          v-model:value="formState.cssClass"
          :min="0"
          :placeholder="t('common.page.form.placeholder.input', { field: t('entity.dictdata.cssclass') })"
          style="width: 100%"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.listclass')"
        name="listClass"
      >
        <a-input-number
          v-model:value="formState.listClass"
          :min="0"
          :placeholder="t('common.page.form.placeholder.input', { field: t('entity.dictdata.listclass') })"
          style="width: 100%"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.extlabel')"
        name="extLabel"
      >
        <a-input
          v-model:value="formState.extLabel"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.dictdata.extlabel') })"
          :maxlength="140"
        />
      </a-form-item>

      <a-form-item
        :label="t('entity.dictdata.extvalue')"
        name="extValue"
      >
        <a-input
          v-model:value="formState.extValue"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.dictdata.extvalue') })"
          :maxlength="140"
        />
      </a-form-item>

      <a-form-item
        :label="t('common.page.entity.remark')"
        name="remark"
      >
        <a-textarea
          v-model:value="formState.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="3"
        />
      </a-form-item>
    </a-form>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { DictData, DictDataCreate, DictDataUpdate } from '@/types/foundation/dictData.types'
import { useTenantStore } from '@/stores/identity/useTenantStore'

const { t } = useI18n()
const tenantStore = useTenantStore()

/** 弹窗表单：`DictDataCreate` 中 `extLabel`/`extValue`/`remark` 为可选时与 `a-input`/`a-textarea` 在 exactOptionalPropertyTypes 下不兼容，此处收窄为必填 `string`（空串表示未填）。 */
type DictDataFormState = Omit<DictDataCreate, 'extLabel' | 'extValue' | 'remark'> & {
  extLabel: string
  extValue: string
  remark: string
  dictDataId?: string
}

// ========================================
// Props & Emits
// ========================================

interface Props {
  formData?: DictData | null
  dictTypeCode?: string
  dictTypeId?: string
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  dictTypeCode: '',
  dictTypeId: '',
  loading: false
})

// ========================================
// 数据定义
// ========================================

const formRef = ref()

/** 表单状态（与 DictData 接口字段顺序一致） */
const formState = reactive<DictDataFormState>({
  tenantCode: '',
  cultureCode: 'mul',
  dictTypeId: '',
  dictTypeCode: '',
  dictLabel: '',
  dictValue: '',
  i18nKey: '',
  extLabel: '',
  extValue: '',
  cssClass: 0,
  listClass: 0,
  isDefault: 0,
  remark: ''
})

const formRulesComputed = computed<Record<string, Rule[]>>(() => ({
  cultureCode: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dictdata.culturecode') }), trigger: 'change' }
  ],
  dictLabel: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dictdata.dictlabel') }), trigger: 'blur' }
  ],
  dictValue: [
    { required: true, message: t('common.page.form.placeholder.required', { field: t('entity.dictdata.dictvalue') }), trigger: 'blur' }
  ]
}))

// ========================================
// 方法定义
// ========================================

/** 监听 formData 变化，同步编辑/新增表单 */
watch(
  () => props.formData,
  (newData) => {
    if (newData) {
      Object.assign(formState, {
        dictDataId: newData.dictDataId,
        tenantCode: newData.tenantCode || tenantStore.tenantCode,
        cultureCode: newData.cultureCode || 'mul',
        dictTypeId: newData.dictTypeId || props.dictTypeId,
        dictTypeCode: newData.dictTypeCode || props.dictTypeCode,
        dictLabel: newData.dictLabel || '',
        i18nKey: newData.i18nKey || '',
        dictValue: newData.dictValue || '',
        cssClass: newData.cssClass ?? 0,
        listClass: newData.listClass ?? 0,
        isDefault: newData.isDefault ?? 0,
        extLabel: newData.extLabel || '',
        extValue: newData.extValue || '',
        remark: newData.remark || ''
      })
    } else {
      Object.assign(formState, {
        dictDataId: undefined,
        tenantCode: tenantStore.tenantCode,
        cultureCode: 'mul',
        dictTypeId: props.dictTypeId || '',
        dictTypeCode: props.dictTypeCode || '',
        dictLabel: '',
        i18nKey: '',
        dictValue: '',
        cssClass: 0,
        listClass: 0,
        isDefault: 0,
        extLabel: '',
        extValue: '',
        remark: ''
      })
    }
  },
  { immediate: true, deep: true }
)

/** 监听 dictTypeCode 和 dictTypeId 变化（新增模式） */
watch(
  () => [props.dictTypeCode, props.dictTypeId],
  ([newCode, newId]) => {
    if (!props.formData) {
      formState.dictTypeCode = newCode || ''
      formState.dictTypeId = newId || ''
    }
  },
  { immediate: true }
)

/** 表单验证 */
const validate = async () => {
  await formRef.value?.validate()
}

/**
 * 获取表单数据（按 DictData 接口字段顺序）
 * @returns {DictDataCreate | DictDataUpdate} 提交载荷
 */
const getFormData = (): DictDataCreate | DictDataUpdate => {
  const baseData: DictDataCreate & { dictDataId?: string } = {
    tenantCode: formState.tenantCode || tenantStore.tenantCode,
    cultureCode: formState.cultureCode || 'mul',
    dictTypeId: formState.dictTypeId,
    dictTypeCode: formState.dictTypeCode,
    dictLabel: formState.dictLabel,
    i18nKey: formState.i18nKey,
    dictValue: formState.dictValue,
    cssClass: formState.cssClass,
    listClass: formState.listClass,
    isDefault: formState.isDefault,
    extLabel: formState.extLabel || undefined,
    extValue: formState.extValue || undefined,
    remark: formState.remark || undefined
  }
  if (formState.dictDataId) {
    baseData.dictDataId = formState.dictDataId
  }
  return baseData
}

// ========================================
// 暴露方法
// ========================================

defineExpose({
  validate,
  getFormData
})
</script>

<style scoped lang="css">
.dict-data-form {
  padding: 16px 0;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktFlowAntDesigner/BasicSetting -->
<!-- 文件名称：TaktFlowBasicSetting.vue -->
<!-- 创建时间：2026-04-07 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：流程方案「基础信息」表单片段（与 AntFlow BasicSetting 职责对齐，绑定 FlowSchemeCreateOrUpdate） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->
<template>
  <a-row :gutter="16">
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processkey')"
        name="processKey"
        required
      >
        <a-input
          v-model:value="processKeyModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.flowscheme.processkey') })"
          :disabled="!!props.form.flowSchemeId"
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processname')"
        name="processName"
        required
      >
        <a-input
          v-model:value="processNameModel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.flowscheme.processname') })"
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.definitionversion')"
        name="definitionVersion"
      >
        <a-input-number
          v-model:value="definitionVersionModel"
          :min="1"
          style="width: 100%"
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processversion')"
        name="processVersion"
      >
        <a-input
          v-model:value="processVersionModel"
          placeholder="v1.0.0"
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.islatest')"
        name="isLatest"
      >
        <a-select
          v-model:value="isLatestModel"
          style="width: 100%"
        >
          <a-select-option :value="1">
            {{ t('common.status.yes') }}
          </a-select-option>
          <a-select-option :value="0">
            {{ t('common.status.no') }}
          </a-select-option>
        </a-select>
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.suspensionstate')"
        name="suspensionState"
      >
        <a-select
          v-model:value="suspensionStateModel"
          style="width: 100%"
        >
          <a-select-option :value="1">
            {{ t('workflow.scheme.page.suspension.active') }}
          </a-select-option>
          <a-select-option :value="2">
            {{ t('workflow.scheme.page.suspension.suspended') }}
          </a-select-option>
        </a-select>
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processcategory')"
        name="processCategory"
      >
        <a-input-number
          v-model:value="processCategoryModel"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processdescription')"
        name="processDescription"
      >
        <a-input
          v-model:value="processDescriptionModel"
          placeholder=""
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.processstatus')"
        name="processStatus"
      >
        <TaktSelect
          v-model="processStatusModel"
          dict-type="sys_scheme_status"
          style="width: 100%"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.flowscheme.processstatus') })"
          allow-clear
        />
      </a-form-item>
    </a-col>
    <a-col :span="12">
      <a-form-item
        :label="t('entity.flowscheme.sortorder')"
        name="sortOrder"
      >
        <a-input-number
          v-model:value="sortOrderModel"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
    </a-col>
  </a-row>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'
import type { FlowSchemeFormModel } from '@/types/workflow/flowScheme.types'

const { t } = useI18n()

const props = defineProps<{
  form: FlowSchemeFormModel
}>()

/**
 * 更新父级 reactive form 字段（与后端 TaktFlowSchemeCreateDto 字段名一致）
 * @param key 字段名
 * @param value 新值
 */
function updateFormField<K extends keyof FlowSchemeFormModel>(key: K, value: FlowSchemeFormModel[K]) {
  props.form[key] = value
}

const processKeyModel = computed({
  get: () => props.form.processKey ?? '',
  set: (value: string) => updateFormField('processKey', value)
})

const processNameModel = computed({
  get: () => props.form.processName ?? '',
  set: (value: string) => updateFormField('processName', value)
})

const processCategoryModel = computed({
  get: () => props.form.processCategory ?? 0,
  set: (value: number | null) => updateFormField('processCategory', value ?? 0)
})

const processDescriptionModel = computed({
  get: () => props.form.processDescription ?? '',
  set: (value: string) => updateFormField('processDescription', value)
})

const processStatusModel = computed({
  get: () => props.form.processStatus ?? 0,
  set: (value: number | null) => updateFormField('processStatus', value ?? 0)
})

const sortOrderModel = computed({
  get: () => props.form.sortOrder ?? 0,
  set: (value: number | null) => updateFormField('sortOrder', value ?? 0)
})

const definitionVersionModel = computed({
  get: () => props.form.definitionVersion ?? 1,
  set: (value: number | null) => updateFormField('definitionVersion', value ?? 1)
})

const processVersionModel = computed({
  get: () => props.form.processVersion ?? 'v1.0.0',
  set: (value: string) => updateFormField('processVersion', value)
})

const isLatestModel = computed({
  get: () => props.form.isLatest ?? 1,
  set: (value: number | null) => updateFormField('isLatest', value ?? 1)
})

const suspensionStateModel = computed({
  get: () => props.form.suspensionState ?? 1,
  set: (value: number | null) => updateFormField('suspensionState', value ?? 1)
})
</script>

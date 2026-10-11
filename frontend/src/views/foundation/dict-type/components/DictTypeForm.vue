<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/dict-type/components -->
<!-- 文件名称：DictTypeForm.vue -->
<!-- 功能描述：字典类型实体 用于定义系统中使用的各种字典分类维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form dict-type-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="dict-type-form-tabs"
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
                :label="pi.label('dictTypeCode')"
                name="dictTypeCode"
              >
                <a-input
                  v-model:value="formState.dictTypeCode"
                  :placeholder="pi.ph('dictTypeCode')"
                  show-count
                  :maxlength="140"
                  allow-clear
                  :disabled="!!formData?.dictTypeId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('dictTypeName')"
                name="dictTypeName"
              >
                <a-input
                  v-model:value="formState.dictTypeName"
                  :placeholder="pi.ph('dictTypeName')"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('dataSource')"
                name="dataSource"
              >
                <TaktSelect
                  v-model:value="formState.dataSource"
                  dict-type="sys_data_source"
                  :placeholder="pi.ph('dataSource')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('dictScript')"
                name="dictScript"
              >
                <a-input
                  v-model:value="formState.dictScript"
                  :placeholder="pi.ph('dictScript')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('isBuiltIn')"
                name="isBuiltIn"
              >
                <TaktSelect
                  v-model:value="formState.isBuiltIn"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isBuiltIn')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('dictStatus')"
                name="dictStatus"
              >
                <TaktSelect
                  v-model:value="formState.dictStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="pi.ph('dictStatus')"
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
    <!-- 下：子表 dictDataList -->
    <TaktEditableTable
      ref="dictDataTableRef"
      v-model="childDictDataRows"
      :columns="dictDataFormColumns"
      :title="dictDataPi.self()"
      :add-button-entity="dictDataPi.self()"
      id-field="dictDataId"
      :default-row="createDefaultDictDataRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 字典类型实体 用于定义系统中使用的各种字典分类维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/foundation/dict-type/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useDictTypeI18n } from '../composables/useDictTypeI18n'

/** 实体字段 i18n */
const pi = useDictTypeI18n()

import type { DictTypeCreate } from '@/types/foundation/dictType.types'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","dictTypeCode","dictTypeName","dataSource","dictScript","isBuiltIn","dictStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
import { useDictDataI18n } from '../composables/useDictDataI18n'

const dictDataPi = useDictDataI18n()

const childDictDataRows = ref<Record<string, unknown>[]>([])
const dictDataTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 dictData 可编辑列 */
const dictDataFormColumns = computed<TaktEditableTableColumn[]>(() => [
,
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<DictTypeCreate & { dictTypeId?: string }> | null | undefined) {
  const rows_dictData = ((val as any)?.dictDataList ?? []) as Record<string, unknown>[]
  childDictDataRows.value = rows_dictData
}

function createDefaultDictDataRow(): Record<string, unknown> {
  return {
    tenantCode: tenantStore.tenantCode,
    cultureCode: 'mul',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.dictTypeId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    dictDataList: dictDataTableRef.value?.getRows?.() ?? childDictDataRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      cultureCode: 'mul',
      dictTypeId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<DictTypeCreate & { dictTypeId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  isBuiltIn: 0,
  dictStatus: 1
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

/** 编辑态灌入 formData；新增态恢复默认值（须含 dictTypeId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.dictTypeId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).dictDataList
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
    if (!props.formData?.dictTypeId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  dictTypeCode: [
    {
      required: true,
      message: pi.ph('dictTypeCode'),
      trigger: 'blur'
    }
  ],
  dictTypeName: [
    {
      required: true,
      message: pi.ph('dictTypeName'),
      trigger: 'blur'
    }
  ],
  dataSource: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('dataSource'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('dataSource'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isBuiltIn: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isBuiltIn'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  dictStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('dictStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('dictStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await dictDataTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('dataSource' in payload) {
    const rawdataSource = payload.dataSource
    if (rawdataSource === undefined || rawdataSource === null || rawdataSource === '') {
      delete payload.dataSource
    } else {
      const numdataSource = typeof rawdataSource === 'number' ? rawdataSource : Number(rawdataSource)
      if (Number.isFinite(numdataSource)) payload.dataSource = numdataSource
      else delete payload.dataSource
    }
  }
  if ('isBuiltIn' in payload) {
    const rawisBuiltIn = payload.isBuiltIn
    if (rawisBuiltIn === undefined || rawisBuiltIn === null || rawisBuiltIn === '') {
      delete payload.isBuiltIn
    } else {
      const numisBuiltIn = typeof rawisBuiltIn === 'number' ? rawisBuiltIn : Number(rawisBuiltIn)
      if (Number.isFinite(numisBuiltIn)) payload.isBuiltIn = numisBuiltIn
      else delete payload.isBuiltIn
    }
  }
  if ('dictStatus' in payload) {
    const rawdictStatus = payload.dictStatus
    if (rawdictStatus === undefined || rawdictStatus === null || rawdictStatus === '') {
      delete payload.dictStatus
    } else {
      const numdictStatus = typeof rawdictStatus === 'number' ? rawdictStatus : Number(rawdictStatus)
      if (Number.isFinite(numdictStatus)) payload.dictStatus = numdictStatus
      else delete payload.dictStatus
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.dictTypeId) {
    payload.dictTypeId = props.formData.dictTypeId
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.dictTypeId)
  childDictDataRows.value = []
  dictDataTableRef.value?.resetRows?.()
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

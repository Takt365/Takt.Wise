<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/code/generator/gen-table/components -->
<!-- 文件名称：GenTableColumnForm.vue -->
<!-- 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言子表 genTableColumn 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form gen-table-column-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="gen-table-column-form-tabs"
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
                :label="pi.label('databaseColumnName')"
                name="databaseColumnName"
              >
                <a-input
                  v-model:value="formState.databaseColumnName"
                  :placeholder="pi.ph('databaseColumnName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('columnComment')"
                name="columnComment"
              >
                <a-input
                  v-model:value="formState.columnComment"
                  :placeholder="pi.ph('columnComment')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('databaseDataType')"
                name="databaseDataType"
              >
                <TaktSelect
                  v-model:value="formState.databaseDataType"
                  dict-type="sys_db_data_type"
                  :placeholder="pi.ph('databaseDataType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('csharpDataType')"
                name="csharpDataType"
              >
                <TaktSelect
                  v-model:value="formState.csharpDataType"
                  dict-type="code_generator_csharp_data_type"
                  :placeholder="pi.ph('csharpDataType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('csharpColumnName')"
                name="csharpColumnName"
              >
                <a-input
                  v-model:value="formState.csharpColumnName"
                  :placeholder="pi.ph('csharpColumnName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('length')"
                name="length"
              >
                <a-input-number
                  v-model:value="formState.length"
                  :placeholder="pi.ph('length')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('decimalDigits')"
                name="decimalDigits"
              >
                <a-input-number
                  v-model:value="formState.decimalDigits"
                  :placeholder="pi.ph('decimalDigits')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isPk')"
                name="isPk"
              >
                <TaktSelect
                  v-model:value="formState.isPk"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isPk')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isIncrement')"
                name="isIncrement"
              >
                <TaktSelect
                  v-model:value="formState.isIncrement"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isIncrement')"
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
                :label="pi.label('isRequired')"
                name="isRequired"
              >
                <TaktSelect
                  v-model:value="formState.isRequired"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isRequired')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isCreate')"
                name="isCreate"
              >
                <TaktSelect
                  v-model:value="formState.isCreate"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isCreate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isUpdate')"
                name="isUpdate"
              >
                <TaktSelect
                  v-model:value="formState.isUpdate"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isUpdate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isUnique')"
                name="isUnique"
              >
                <TaktSelect
                  v-model:value="formState.isUnique"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isUnique')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isList')"
                name="isList"
              >
                <TaktSelect
                  v-model:value="formState.isList"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isList')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isExport')"
                name="isExport"
              >
                <TaktSelect
                  v-model:value="formState.isExport"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isExport')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isSort')"
                name="isSort"
              >
                <TaktSelect
                  v-model:value="formState.isSort"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isSort')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isQuery')"
                name="isQuery"
              >
                <TaktSelect
                  v-model:value="formState.isQuery"
                  dict-type="sys_yes_no"
                  :placeholder="pi.ph('isQuery')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('queryType')"
                name="queryType"
              >
                <TaktSelect
                  v-model:value="formState.queryType"
                  dict-type="code_generator_query_type"
                  :placeholder="pi.ph('queryType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('htmlType')"
                name="htmlType"
              >
                <TaktSelect
                  v-model:value="formState.htmlType"
                  dict-type="code_generator_display_type"
                  :placeholder="pi.ph('htmlType')"
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
                :label="pi.label('dictType')"
                name="dictType"
              >
                <TaktSelect
                  v-model:value="formState.dictType"
                  api-url="TaktDictTypes/options"
                  :placeholder="pi.ph('dictType')"
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
          </a-row>
        </div>
      </a-tab-pane>
    </a-tabs>
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言子表 genTableColumn 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/code/generator/gen-table/components
 */
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useGenTableColumnI18n } from '../composables/useGenTableColumnI18n'

/** 实体字段 i18n */
const pi = useGenTableColumnI18n()

import type { GenTableColumnCreate } from '@/types/code/generator/genTableColumn.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { useTenantStore } from '@/stores/identity/useTenantStore'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：租户上下文 */
const tenantStore = useTenantStore()

/**
 * 上下文隔离字段：租户级实体仅注入 tenantCode，表单只读
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
const formFields = ["tenantCode","lineNumber","databaseColumnName","columnComment","databaseDataType","csharpDataType","csharpColumnName","length","decimalDigits","isPk","isIncrement","isRequired","isCreate","isUpdate","isUnique","isList","isExport","isSort","isQuery","queryType","htmlType","dictType"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<GenTableColumnCreate & { genTableColumnId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 genTableColumnId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.genTableColumnId) {
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
    if (!props.formData?.genTableColumnId) {
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
  databaseColumnName: [
    {
      required: true,
      message: pi.ph('databaseColumnName'),
      trigger: 'blur'
    }
  ],
  databaseDataType: [
    {
      required: true,
      message: pi.ph('databaseDataType'),
      trigger: 'change'
    }
  ],
  csharpDataType: [
    {
      required: true,
      message: pi.ph('csharpDataType'),
      trigger: 'change'
    }
  ],
  csharpColumnName: [
    {
      required: true,
      message: pi.ph('csharpColumnName'),
      trigger: 'blur'
    }
  ],
  length: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('length'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('length'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  decimalDigits: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('decimalDigits'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('decimalDigits'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPk: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isPk'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isPk'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isIncrement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isIncrement'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isIncrement'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isRequired: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isRequired'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCreate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCreate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCreate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isUpdate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isUpdate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isUpdate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isUnique: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isUnique'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isUnique'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isList: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isList'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isList'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isExport: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isExport'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isExport'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isSort: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isSort'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isSort'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isQuery: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isQuery'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isQuery'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  queryType: [
    {
      required: true,
      message: pi.ph('queryType'),
      trigger: 'change'
    }
  ],
  htmlType: [
    {
      required: true,
      message: pi.ph('htmlType'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 genTableId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
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
  if ('length' in payload) {
    const rawlength = payload.length
    if (rawlength === undefined || rawlength === null || rawlength === '') {
      delete payload.length
    } else {
      const numlength = typeof rawlength === 'number' ? rawlength : Number(rawlength)
      if (Number.isFinite(numlength)) payload.length = numlength
      else delete payload.length
    }
  }
  if ('decimalDigits' in payload) {
    const rawdecimalDigits = payload.decimalDigits
    if (rawdecimalDigits === undefined || rawdecimalDigits === null || rawdecimalDigits === '') {
      delete payload.decimalDigits
    } else {
      const numdecimalDigits = typeof rawdecimalDigits === 'number' ? rawdecimalDigits : Number(rawdecimalDigits)
      if (Number.isFinite(numdecimalDigits)) payload.decimalDigits = numdecimalDigits
      else delete payload.decimalDigits
    }
  }
  if ('isPk' in payload) {
    const rawisPk = payload.isPk
    if (rawisPk === undefined || rawisPk === null || rawisPk === '') {
      delete payload.isPk
    } else {
      const numisPk = typeof rawisPk === 'number' ? rawisPk : Number(rawisPk)
      if (Number.isFinite(numisPk)) payload.isPk = numisPk
      else delete payload.isPk
    }
  }
  if ('isIncrement' in payload) {
    const rawisIncrement = payload.isIncrement
    if (rawisIncrement === undefined || rawisIncrement === null || rawisIncrement === '') {
      delete payload.isIncrement
    } else {
      const numisIncrement = typeof rawisIncrement === 'number' ? rawisIncrement : Number(rawisIncrement)
      if (Number.isFinite(numisIncrement)) payload.isIncrement = numisIncrement
      else delete payload.isIncrement
    }
  }
  if ('isRequired' in payload) {
    const rawisRequired = payload.isRequired
    if (rawisRequired === undefined || rawisRequired === null || rawisRequired === '') {
      delete payload.isRequired
    } else {
      const numisRequired = typeof rawisRequired === 'number' ? rawisRequired : Number(rawisRequired)
      if (Number.isFinite(numisRequired)) payload.isRequired = numisRequired
      else delete payload.isRequired
    }
  }
  if ('isCreate' in payload) {
    const rawisCreate = payload.isCreate
    if (rawisCreate === undefined || rawisCreate === null || rawisCreate === '') {
      delete payload.isCreate
    } else {
      const numisCreate = typeof rawisCreate === 'number' ? rawisCreate : Number(rawisCreate)
      if (Number.isFinite(numisCreate)) payload.isCreate = numisCreate
      else delete payload.isCreate
    }
  }
  if ('isUpdate' in payload) {
    const rawisUpdate = payload.isUpdate
    if (rawisUpdate === undefined || rawisUpdate === null || rawisUpdate === '') {
      delete payload.isUpdate
    } else {
      const numisUpdate = typeof rawisUpdate === 'number' ? rawisUpdate : Number(rawisUpdate)
      if (Number.isFinite(numisUpdate)) payload.isUpdate = numisUpdate
      else delete payload.isUpdate
    }
  }
  if ('isUnique' in payload) {
    const rawisUnique = payload.isUnique
    if (rawisUnique === undefined || rawisUnique === null || rawisUnique === '') {
      delete payload.isUnique
    } else {
      const numisUnique = typeof rawisUnique === 'number' ? rawisUnique : Number(rawisUnique)
      if (Number.isFinite(numisUnique)) payload.isUnique = numisUnique
      else delete payload.isUnique
    }
  }
  if ('isList' in payload) {
    const rawisList = payload.isList
    if (rawisList === undefined || rawisList === null || rawisList === '') {
      delete payload.isList
    } else {
      const numisList = typeof rawisList === 'number' ? rawisList : Number(rawisList)
      if (Number.isFinite(numisList)) payload.isList = numisList
      else delete payload.isList
    }
  }
  if ('isExport' in payload) {
    const rawisExport = payload.isExport
    if (rawisExport === undefined || rawisExport === null || rawisExport === '') {
      delete payload.isExport
    } else {
      const numisExport = typeof rawisExport === 'number' ? rawisExport : Number(rawisExport)
      if (Number.isFinite(numisExport)) payload.isExport = numisExport
      else delete payload.isExport
    }
  }
  if ('isSort' in payload) {
    const rawisSort = payload.isSort
    if (rawisSort === undefined || rawisSort === null || rawisSort === '') {
      delete payload.isSort
    } else {
      const numisSort = typeof rawisSort === 'number' ? rawisSort : Number(rawisSort)
      if (Number.isFinite(numisSort)) payload.isSort = numisSort
      else delete payload.isSort
    }
  }
  if ('isQuery' in payload) {
    const rawisQuery = payload.isQuery
    if (rawisQuery === undefined || rawisQuery === null || rawisQuery === '') {
      delete payload.isQuery
    } else {
      const numisQuery = typeof rawisQuery === 'number' ? rawisQuery : Number(rawisQuery)
      if (Number.isFinite(numisQuery)) payload.isQuery = numisQuery
      else delete payload.isQuery
    }
  }
  if ('sortOrder' in payload) delete payload.sortOrder

  if (props.formData?.genTableColumnId) {
    payload.genTableColumnId = props.formData.genTableColumnId
  }
  payload.genTableId = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.genTableCode ?? masterRow.GenTableCode
    const masterName = masterRow.genTableName ?? masterRow.GenTableName
    if (masterCode != null && masterCode !== '' && !payload.genTableCode) {
      payload.genTableCode = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.genTableName) {
      payload.genTableName = masterName
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
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.genTableColumnId)
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

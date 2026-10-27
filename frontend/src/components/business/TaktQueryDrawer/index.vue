<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktQueryDrawer -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-21 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 高级查询抽屉；默认展示前 N 个查询字段，支持字段显隐自定义与 localStorage 持久化 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-drawer
    v-model:open="internalOpen"
    v-bind="drawerProps"
    :title="title !== '' ? title : t('common.page.button.advancedquery')"
    :placement="placement"
    :width="width"
    class="takt-query-drawer"
    @close="handleClose"
  >
    <template #extra>
      <a-button
        v-if="fields.length > 0"
        size="small"
        @click="fieldSettingExpanded = !fieldSettingExpanded"
      >
        {{ t('components.business.page.querydrawer.fieldsetting') }}
      </a-button>
    </template>
    <div
      v-if="fields.length > 0 && fieldSettingExpanded"
      class="mb-4 rounded border border-border p-3"
    >
      <div class="mb-2 text-sm font-medium text-text">
        {{ t('components.business.page.querydrawer.fieldsetting') }}
      </div>
      <a-checkbox-group
        v-model:value="selecmedFieldKeys"
        class="flex w-full flex-col gap-2"
      >
        <a-checkbox
          v-for="field in fields"
          :key="field.key"
          :value="field.key"
        >
          {{ field.label }}
        </a-checkbox>
      </a-checkbox-group>
      <div class="mt-3">
        <a-button
          size="small"
          @click="handleFieldSettingReset"
        >
          {{ t('common.page.button.reset') }}
        </a-button>
      </div>
    </div>
    <a-form
      ref="formRef"
      :class="formLayout === 'horizontal' ? 'takt-generated-form' : undefined"
      :model="formModel"
      :layout="formLayout"
      label-align="right"
      @finish="handleSubmit"
    >
      <slot :is-field-visible="isFieldVisible" />
      <a-form-item>
        <a-space>
          <a-button
            type="primary"
            html-type="submit"
            :loading="submitLoading"
          >
            {{ submitText !== '' ? submitText : t('common.page.button.query') }}
          </a-button>
          <a-button @click="handleReset">
            {{ resetText !== '' ? resetText : t('common.page.button.reset') }}
          </a-button>
        </a-space>
      </a-form-item>
    </a-form>
  </a-drawer>
</template>

<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

/** 高级查询字段元数据 */
export interface TaktQueryFieldItem {
  /** 字段键，与表单 model 属性名一致 */
  key: string
  /** 字段显示标签 */
  label: string
}

interface Props {
  /** 是否显示抽屉 */
  open?: boolean
  /** 抽屉标题，默认为"高级查询" */
  title?: string
  /** 抽屉位置，默认为"right" */
  placement?: 'top' | 'right' | 'bottom' | 'left'
  /** 抽屉宽度，默认为 400 */
  width?: string | number
  /** 表单数据模型 */
  formModel?: Record<string, unknown>
  /** 表单布局，默认为"vertical" */
  formLayout?: 'horizontal' | 'vertical' | 'inline'
  /** 查询按钮文本，默认为"查询" */
  submitText?: string
  /** 重置按钮文本，默认为"重置" */
  resetText?: string
  /** 查询按钮加载状态 */
  submitLoading?: boolean
  /** 全部高级查询字段（用于显隐配置） */
  fields?: TaktQueryFieldItem[]
  /** 当前可见字段键列表 */
  visibleFieldKeys?: string[]
  /** 默认可见字段数量（未持久化且父级未传入 visibleFieldKeys 时） */
  defaultVisibleCount?: number
  /** localStorage 键；传入则持久化用户字段显隐偏好 */
  storageKey?: string
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  title: '',
  placement: 'right',
  width: 400,
  formModel: () => ({}),
  formLayout: 'vertical',
  submitText: '',
  resetText: '',
  submitLoading: false,
  fields: () => [],
  visibleFieldKeys: () => [],
  defaultVisibleCount: 5,
  storageKey: '',
})

const emit = defineEmits<{
  'update:open': [open: boolean]
  'update:visibleFieldKeys': [keys: string[]]
  'submit': [values: Record<string, unknown>]
  'reset': []
  'close': []
}>()

const attrs = useAttrs()
const formRef = ref<FormInstance>()
/** 字段显隐配置面板展开态 */
const fieldSettingExpanded = ref(false)
/** 内部可见字段键 */
const internalVisibleFieldKeys = ref<string[]>([])

type FormInstanceWithSetFieldsValue = FormInstance & {
  setFieldsValue?: (values: Record<string, unknown>) => void
}

/** 全部字段键（按 fields 顺序） */
const allFieldKeys = computed(() => props.fields.map((f) => f.key).filter(Boolean))

/** 默认可见字段键（前 defaultVisibleCount 个） */
function getDefaultVisibleFieldKeys(): string[] {
  const keys = allFieldKeys.value
  if (keys.length === 0) {
    return []
  }
  const count = Math.max(1, Math.min(props.defaultVisibleCount, keys.length))
  return keys.slice(0, count)
}

/** 从 localStorage 读取已保存的可见字段键 */
function readStoredVisibleFieldKeys(): string[] | null {
  if (!props.storageKey) {
    return null
  }
  try {
    const raw = localStorage.getItem(props.storageKey)
    if (!raw) {
      return null
    }
    const parsed = JSON.parse(raw)
    if (!Array.isArray(parsed)) {
      return null
    }
    const valid = parsed.map((k) => String(k)).filter((k) => allFieldKeys.value.includes(k))
    return valid.length > 0 ? valid : null
  } catch {
    return null
  }
}

/** 将可见字段键写入 localStorage */
function writeStoredVisibleFieldKeys(keys: string[]): void {
  if (!props.storageKey) {
    return
  }
  try {
    localStorage.setItem(props.storageKey, JSON.stringify(keys))
  } catch {
    // 忽略存储失败（隐私模式等）
  }
}

/** 同步可见字段键到内部状态并通知父级 */
function applyVisibleFieldKeys(keys: string[]): void {
  const normalized = keys.filter((k) => allFieldKeys.value.includes(k))
  const finalKeys = normalized.length > 0 ? normalized : getDefaultVisibleFieldKeys()
  internalVisibleFieldKeys.value = finalKeys
  emit('update:visibleFieldKeys', finalKeys)
  writeStoredVisibleFieldKeys(finalKeys)
}

/** 初始化可见字段键 */
function initVisibleFieldKeys(): void {
  if (props.visibleFieldKeys.length > 0) {
    applyVisibleFieldKeys([...props.visibleFieldKeys])
    return
  }
  const stored = readStoredVisibleFieldKeys()
  if (stored) {
    applyVisibleFieldKeys(stored)
    return
  }
  applyVisibleFieldKeys(getDefaultVisibleFieldKeys())
}

initVisibleFieldKeys()

watch(
  () => props.fields,
  () => {
    const current = internalVisibleFieldKeys.value.filter((k) => allFieldKeys.value.includes(k))
    if (current.length === 0) {
      initVisibleFieldKeys()
      return
    }
    applyVisibleFieldKeys(current)
  },
  { deep: true }
)

watch(
  () => props.visibleFieldKeys,
  (newKeys) => {
    if (!newKeys || newKeys.length === 0) {
      return
    }
    const normalized = newKeys.filter((k) => allFieldKeys.value.includes(k))
    const currentSorted = [...internalVisibleFieldKeys.value].sort()
    const newSorted = [...normalized].sort()
    if (JSON.stringify(currentSorted) !== JSON.stringify(newSorted)) {
      internalVisibleFieldKeys.value = normalized
    }
  },
  { deep: true }
)

const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => {
    emit('update:open', value)
  }
})

const drawerProps = computed(() => attrs)

/** checkbox-group 双向绑定 */
const selecmedFieldKeys = computed({
  get: () => internalVisibleFieldKeys.value,
  set: (val: string[]) => {
    applyVisibleFieldKeys(val.map(String))
  }
})

/**
 * 判断高级查询字段是否可见
 * @param {string} key 字段键
 * @returns {boolean} 是否显示
 */
function isFieldVisible(key: string): boolean {
  if (props.fields.length === 0) {
    return true
  }
  return internalVisibleFieldKeys.value.includes(key)
}

function handleSubmit(values: Record<string, unknown>): void {
  emit('submit', values)
}

function handleReset(): void {
  formRef.value?.resetFields()
  emit('reset')
}

function handleFieldSettingReset(): void {
  applyVisibleFieldKeys(getDefaultVisibleFieldKeys())
}

function handleClose(): void {
  emit('close')
  emit('update:open', false)
}

defineExpose({
  resetFields: () => formRef.value?.resetFields(),
  validate: () => formRef.value?.validate(),
  validateFields: (nameList?: string[]) => formRef.value?.validateFields(nameList),
  getFieldsValue: () => formRef.value?.getFieldsValue(),
  setFieldsValue: (values: Record<string, unknown>) => {
    const form = formRef.value as FormInstanceWithSetFieldsValue | undefined
    form?.setFieldsValue?.(values)
  },
  isFieldVisible,
  resetVisibleFieldKeys: handleFieldSettingReset,
})
</script>

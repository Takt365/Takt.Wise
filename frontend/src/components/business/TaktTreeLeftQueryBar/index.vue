<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktTreeLeftQueryBar
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:左树查询栏,栏宽对齐左表；无查询/重置按钮时关键字输入框占满左表栏宽

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->

<template>
  <div
    v-if="show"
    class="takt-tree-left-query-bar"
  >
    <a-input
      v-model:value="keyword"
      :placeholder="placeholder ?? defaultPlaceholder"
      :size="size"
      :allow-clear="allowClear"
      :max-length="maxLength"
      @press-enter="handleSearch"
      @change="handleChange"
    >
      <template #prefix>
        <RiSearchLine class="takt-remix-icon" />
      </template>
    </a-input>
  </div>
</template>

<script setup lang="ts">
import { RiSearchLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

/** 默认占位：tree.search 模板 + common.page.form.keyword */
const defaultPlaceholder = computed(() =>
  t('common.page.form.placeholder.tree.search', { keyword: t('common.page.form.keyword') }),
)

interface Props {
  /** 是否显示 */
  show?: boolean
  /** 关键字值(v-model) */
  modelValue?: string
  /** 占位符 */
  placeholder?: string | undefined
  /** 输入框尺寸 */
  size?: 'small' | 'middle' | 'large'
  /** 是否显示清除按钮 */
  allowClear?: boolean
  /** 加载状态 */
  loading?: boolean
  /** 最大长度 */
  maxLength?: number | undefined
}

const props = withDefaults(defineProps<Props>(), {
  show: true,
  modelValue: '',
  placeholder: undefined,
  size: 'middle',
  allowClear: true,
  loading: false,
  maxLength: undefined,
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
  'search': [keyword: string]
  'reset': []
  'change': [value: string]
}>()

const keyword = ref(props.modelValue)

watch(() => props.modelValue, (val) => {
  keyword.value = val
})

watch(keyword, (val) => {
  emit('update:modelValue', val)
})

const handleSearch = () => {
  emit('search', keyword.value)
}

const handleReset = () => {
  keyword.value = ''
  emit('update:modelValue', '')
  emit('reset')
}

const handleChange = (e: Event) => {
  const value = (e.target as HTMLInputElement).value
  emit('change', value)
}

defineExpose({
  keyword,
  clear: handleReset,
  search: handleSearch
})
</script>

<style scoped>
/* 栏宽：与左侧树一致（20%）；无查询/重置按钮，输入框占满左表栏宽 */
.takt-tree-left-query-bar {
  flex: 0 0 20%;
  width: 20%;
  min-width: 160px;
  max-width: 20%;
  padding: 0;
  display: flex;
  align-items: center;
  gap: 8px;
  box-sizing: border-box;

  :deep(.ant-input-affix-wrapper) {
    flex: 1 1 auto;
    width: 100%;
    max-width: none;
    min-width: 0;

    .ant-input-prefix {
      margin-inline-end: 4px;

      svg {
        color: var(--ant-color-text-secondary);
        fill: currentColor;
      }
    }
  }
}
</style>

<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktTreeRightQueryBar
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:右树查询栏,栏宽对齐右表；关键字输入框宽度=右表栏宽减去查询/重置按钮

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div
    v-if="show"
    class="takt-query-bar"
  >
    <a-input
      v-model:value="keyword"
      class="takt-tree-right-query-bar__keyword"
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
    <a-space class="query-actions">
      <a-button
        class="takt-button-query"
        :loading="loading"
        @click="handleSearch"
      >
        <template #icon>
          <RiSearchLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.query') }}
      </a-button>
      <a-button
        class="takt-button-reset"
        @click="handleReset"
      >
        <template #icon>
          <RiRefreshLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.reset') }}
      </a-button>
    </a-space>
  </div>
</template>

<script setup lang="ts">
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

/** 默认占位：search + common.page.form.keyword */
const defaultPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', { keyword: t('common.page.form.keyword') }),
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
/* 栏宽：与右表一致（80%）；关键字 flex:1 = 右表栏宽 − 查询/重置按钮 */
.takt-query-bar {
  flex: 0 0 80%;
  width: 80%;
  min-width: 200px;
  max-width: 80%;
  padding: 4px;
  box-sizing: border-box;
  display: flex;
  align-items: center;
  gap: 8px;

  :deep(.takt-tree-right-query-bar__keyword.ant-input-affix-wrapper),
  :deep(.ant-input-affix-wrapper) {
    flex: 1 1 auto;
    width: auto;
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

  .query-actions {
    flex: 0 0 auto;
    flex-shrink: 0;

    :deep(.ant-btn) {
      display: inline-flex;
      align-items: center;
      gap: 4px;

      .anticon {
        margin-inline-end: 0 !important;
      }
    }
  }
}
</style>

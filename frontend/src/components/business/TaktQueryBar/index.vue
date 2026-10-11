<!-- ========================================
项目名称:TaktWise
命名空间:@/components/business/TaktQueryBar
文件名称:index.vue
创建时间:2025-01-20
创建人:Takt365(Cursor AI)
功能描述:查询栏组件；默认关键字占满剩余宽；有 #fields 时关键字 50%，扩展条件+查询/重置共 50%

版权信息:Copyright (c) 2025 Takt  All rights reserved.
免责声明:此软件使用 MIT License,作者不承担任何使用风险。
======================================== -->
<template>
  <div
    v-if="show"
    class="takt-query-bar"
    :class="{ 'takt-query-bar--custom-fields': hasFieldsSlot }"
  >
    <a-input
      v-if="showKeyword"
      v-model:value="keyword"
      class="takt-query-bar__keyword"
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
    <div class="takt-query-bar__trailing">
      <div v-if="hasFieldsSlot" class="takt-query-bar__fields">
        <slot name="fields" />
      </div>
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
  </div>
</template>

<script setup lang="ts">
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import { useSlots } from 'vue'

const { t } = useI18n()
const slots = useSlots()

/** 是否使用了 #fields 自定义条件 */
const hasFieldsSlot = computed(() => Boolean(slots.fields))

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
  /**
   * 是否显示关键字框
   * @description 仅用 #fields 时建议 false；默认 true 保持原 CRUD 页行为
   */
  showKeyword?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  show: true,
  modelValue: '',
  placeholder: undefined,
  size: 'middle',
  allowClear: true,
  loading: false,
  maxLength: undefined,
  showKeyword: true,
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
  search: handleSearch,
})
</script>

<style scoped>
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
}

.takt-query-bar__keyword {
  flex: 1 1 auto;
  width: auto;
  min-width: 0;
  max-width: none;
}

.takt-query-bar__trailing {
  flex: 0 0 auto;
  display: flex;
  align-items: center;
  gap: 8px;
  min-width: 0;
}

.takt-query-bar__fields {
  flex: 1 1 auto;
  min-width: 0;
  display: flex;
  align-items: center;
  gap: 8px;
}

.takt-query-bar--custom-fields .takt-query-bar__keyword {
  flex: 0 0 calc(50% - 4px);
  width: calc(50% - 4px);
  max-width: calc(50% - 4px);
}

.takt-query-bar--custom-fields .takt-query-bar__trailing {
  flex: 0 0 calc(50% - 4px);
  width: calc(50% - 4px);
  max-width: calc(50% - 4px);
}

.takt-query-bar--custom-fields .takt-query-bar__fields > * {
  flex: 1 1 0;
  min-width: 0;
}

.takt-query-bar--custom-fields .takt-query-bar__fields :deep(.ant-select) {
  width: 100%;
}

:deep(.ant-input-affix-wrapper) {
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
</style>

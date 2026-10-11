<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/components/business/TaktIconPicker -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Remix 图标选择器（只读输入 + 弹窗网格）；v-model 绑定图标组件名；依赖 @remixicon/vue 与 takt-remix-icon -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-icon-picker w-full min-w-0">
    <a-input
      :value="displayValue"
      readonly
      :disabled="disabled"
      :placeholder="placeholder || t('common.page.icon.picker.placeholder')"
      class="cursor-pointer"
      :allow-clear="allowClear && !!modelValue"
      @click="openModal"
      @clear="handleClear"
    >
      <template #prefix>
        <TaktRemixIcon
          :name="modelValue"
          :size="18"
          :show-placeholder="!!modelValue"
        />
      </template>
      <template #suffix>
        <span
          role="button"
          tabindex="0"
          class="inline-flex items-center text-text-secondary hover:text-primary cursor-pointer"
          :aria-label="t('common.page.icon.picker.title')"
          @click.stop="openModal"
          @keydown.enter.prevent="openModal"
        >
          <RiApps2Line class="takt-remix-icon" />
        </span>
      </template>
    </a-input>

    <TaktModal
      v-model:open="modalOpen"
      :title="t('common.page.icon.picker.title')"
      :use-viewport-size="false"
      width="720px"
      :confirm-loading="false"
      @ok="handleConfirm"
    >
      <div class="flex flex-col gap-3">
        <div class="flex flex-wrap items-center gap-2">
          <a-input
            v-model:value="keyword"
            allow-clear
            class="min-w-0 flex-1"
            :placeholder="t('common.page.icon.picker.search')"
          >
            <template #prefix>
              <RiSearchLine class="takt-remix-icon text-text-secondary" />
            </template>
          </a-input>
          <a-radio-group
            v-model:value="variant"
            button-style="solid"
            option-type="button"
            :options="variantOptions"
          />
        </div>

        <div
          v-if="selectedPreview"
          class="flex items-center gap-2 text-sm text-text-secondary"
        >
          <span>{{ t('common.page.icon.picker.selected') }}</span>
          <TaktRemixIcon
            :name="selectedPreview"
            :size="20"
          />
          <span class="font-mono text-text">{{ selectedPreview }}</span>
        </div>

        <a-spin :spinning="listLoading">
          <div
            class="grid max-h-[360px] grid-cols-6 gap-2 overflow-y-auto sm:grid-cols-8"
          >
            <button
              v-for="name in pagedNames"
              :key="name"
              type="button"
              class="flex flex-col items-center gap-1 rounded border border-border px-1 py-2 text-center transition-colors hover:border-primary hover:text-primary"
              :class="name === selectedPreview ? 'border-primary bg-primary/5 text-primary' : ''"
              :title="name"
              @click="selectedPreview = name"
            >
              <TaktRemixIcon
                :name="name"
                :size="22"
                :show-placeholder="false"
              />
              <span class="w-full truncate text-[10px] leading-tight">{{ shortLabel(name) }}</span>
            </button>
          </div>
          <a-empty
            v-if="!listLoading && filteredNames.length === 0"
            :description="t('common.page.icon.picker.empty')"
          />
        </a-spin>

        <div class="flex items-center justify-between gap-2">
          <a-button
            v-if="allowClear"
            type="link"
            class="px-0"
            @click="selectedPreview = ''"
          >
            {{ t('common.page.icon.picker.clear') }}
          </a-button>
          <span
            v-else
            class="flex-1"
          />
          <a-pagination
            v-model:current="currentPage"
            size="small"
            :total="filteredNames.length"
            :page-size="pageSize"
            :show-size-changer="false"
            :show-total="(total: number) => t('common.page.icon.picker.total', { total })"
          />
        </div>
      </div>
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * Remix 图标选择器：表单控件 + 弹窗分页网格
 */
import { useI18n } from 'vue-i18n'
import { RiApps2Line, RiSearchLine } from '@remixicon/vue'
import {
  filterRemixIconNames,
  listRemixIconNames,
  preloadRemixIcons,
  type TaktRemixIconVariant,
} from '@/utils/taktRemixIcon'

/** 每页图标数（避免一次渲染千级 DOM） */
const PAGE_SIZE = 80

interface Props {
  /** 绑定 Remix 组件名 */
  modelValue?: string
  /** 禁用 */
  disabled?: boolean
  /** 占位文案 */
  placeholder?: string
  /** 是否允许清空 */
  allowClear?: boolean
}

interface Emits {
  (event: 'update:modelValue', value: string): void
  (event: 'change', value: string): void
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: '',
  disabled: false,
  placeholder: undefined,
  allowClear: true,
})

const emit = defineEmits<Emits>()

const { t } = useI18n()

/** 弹窗可见 */
const modalOpen = ref(false)
/** 名称列表加载中 */
const listLoading = ref(false)
/** 搜索关键字 */
const keyword = ref('')
/** 图标变体 */
const variant = ref<TaktRemixIconVariant>('line')
/** 当前变体下全量名称 */
const allNames = ref<string[]>([])
/** 弹窗内临时选中 */
const selectedPreview = ref('')
/** 当前页 */
const currentPage = ref(1)
/** 每页条数 */
const pageSize = PAGE_SIZE

/** 变体选项 */
const variantOptions = computed(() => [
  { label: t('common.page.icon.picker.variant.line'), value: 'line' },
  { label: t('common.page.icon.picker.variant.fill'), value: 'fill' },
  { label: t('common.page.icon.picker.variant.all'), value: 'all' },
])

/** 输入框展示值 */
const displayValue = computed(() => String(props.modelValue ?? '').trim())

/** 过滤后的名称 */
const filteredNames = computed(() => filterRemixIconNames(allNames.value, keyword.value))

/** 当前页名称切片 */
const pagedNames = computed(() => {
  const start = checkedPageStart(currentPage.value, pageSize, filteredNames.value.length)
  return filteredNames.value.slice(start, start + pageSize)
})

/**
 * 计算分页起始下标（防越界）
 * @param page 页码（从 1）
 * @param size 每页大小
 * @param total 总数
 * @returns {number} slice 起始
 */
function checkedPageStart(page: number, size: number, total: number): number {
  const safePage = Math.max(1, page)
  const safeSize = Math.max(1, size)
  const maxPage = Math.max(1, Math.ceil(total / safeSize) || 1)
  const clamped = Math.min(safePage, maxPage)
  return (clamped - 1) * safeSize
}

/**
 * 缩短网格标签（去掉 Ri / Line|Fill 后缀）
 * @param name 完整组件名
 * @returns {string} 短标签
 */
function shortLabel(name: string): string {
  return name
    .replace(/^Ri/, '')
    .replace(/(Line|Fill)$/, '')
}

/**
 * 加载当前变体图标名列表
 * @returns {Promise<void>}
 */
async function loadIconNames(): Promise<void> {
  listLoading.value = true
  try {
    allNames.value = await listRemixIconNames({ variant: variant.value })
  } finally {
    listLoading.value = false
  }
}

/**
 * 预加载当前页图标组件
 * @returns {Promise<void>}
 */
async function preloadPageIcons(): Promise<void> {
  if (!pagedNames.value.length) {
    return
  }
  await preloadRemixIcons(pagedNames.value)
}

/** 打开选择弹窗 */
async function openModal(): Promise<void> {
  if (props.disabled) {
    return
  }
  selectedPreview.value = String(props.modelValue ?? '').trim()
  keyword.value = ''
  currentPage.value = 1
  modalOpen.value = true
  await loadIconNames()
  await preloadPageIcons()
}

/** 清空绑定值 */
function handleClear(): void {
  emit('update:modelValue', '')
  emit('change', '')
}

/** 确认选择 */
function handleConfirm(): void {
  const value = String(selectedPreview.value ?? '').trim()
  emit('update:modelValue', value)
  emit('change', value)
  modalOpen.value = false
}

watch(variant, async () => {
  currentPage.value = 1
  await loadIconNames()
  await preloadPageIcons()
})

watch(keyword, () => {
  currentPage.value = 1
})

watch(filteredNames, (list) => {
  const maxPage = Math.max(1, Math.ceil(list.length / pageSize) || 1)
  if (currentPage.value > maxPage) {
    currentPage.value = maxPage
  }
})

watch(pagedNames, () => {
  void preloadPageIcons()
})
</script>

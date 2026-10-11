<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：frontend/src/components/common/TaktTagColor -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：预设色标签（color-base.css + tag-base.css；单标签 / 逗号列表） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div v-if="isListMode" class="flex flex-wrap items-center gap-1 w-full">
    <template v-if="resolvedTags.length">
      <template v-for="(tag, tagIndex) in resolvedTags" :key="`${tagIndex}-${tag}`">
        <a-tooltip v-if="shouldTruncateTagLabel(tag, displayMax)" :title="tag">
          <a-tag :class="buildTagColorClass(tagIndex)">
            {{ truncateTagLabel(tag, displayMax) }}
          </a-tag>
        </a-tooltip>
        <a-tag v-else :class="buildTagColorClass(tagIndex)">
          {{ tag }}
        </a-tag>
      </template>
    </template>
    <span v-else class="text-text-secondary">{{ emptyText }}</span>
  </div>
  <template v-else-if="singleLabel">
    <a-tooltip v-if="shouldTruncateTagLabel(singleLabel, displayMax)" :title="singleLabel">
      <a-tag
        :class="buildSingleTagClass()"
        :closable="closable"
        @close="handleClose"
      >
        <slot>{{ truncateTagLabel(singleLabel, displayMax) }}</slot>
      </a-tag>
    </a-tooltip>
    <a-tag
      v-else
      :class="buildSingleTagClass()"
      :closable="closable"
      @close="handleClose"
    >
      <slot>{{ singleLabel }}</slot>
    </a-tag>
  </template>
</template>

<script setup lang="ts">
/**
 * 预设色标签：列表模式（value/tags）或单标签模式（label + index）
 */
import { computed } from 'vue'
import {
  TAKT_TAG_COLOR_BASE_CLASS,
  TAKT_TAG_COLOR_DISPLAY_MAX,
  normalizeTagList,
  parseCommaSeparatedTags,
  resolveTaktTagColorClass,
  shouldTruncateTagLabel,
  truncateTagLabel,
} from '@/utils/taktTagColor'

interface Props {
  /** 逗号分隔标签（列表模式） */
  value?: string | null
  /** 标签数组（列表模式，优先于 value） */
  tags?: string[] | null
  /** 单标签文案（非列表模式） */
  label?: string
  /** 单标签配色序号 */
  index?: number
  /** 列表解析分隔符 */
  separator?: string
  /** 列表最大展示数量 */
  maxCount?: number
  /** 展示截断长度 */
  displayMax?: number
  /** 无标签占位 */
  emptyText?: string
  /** 单标签可关闭 */
  closable?: boolean
  /** 单标签禁用态（降低透明度） */
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  value: undefined,
  tags: undefined,
  label: undefined,
  index: 0,
  separator: ',',
  maxCount: undefined,
  displayMax: TAKT_TAG_COLOR_DISPLAY_MAX,
  emptyText: '-',
  closable: false,
  disabled: false,
})

const emit = defineEmits<{
  /** 单标签 closable 关闭 */
  close: []
}>()

/** 是否列表模式 */
const isListMode = computed(() => {
  if (props.tags != null && props.tags.length > 0) {
    return true
  }
  const raw = props.value
  return raw != null && typeof raw === 'string' && raw.trim() !== ''
})

/** 列表模式标签 */
const resolvedTags = computed(() => {
  if (props.tags != null && props.tags.length > 0) {
    return normalizeTagList(props.tags, props.maxCount)
  }
  const parsed = parseCommaSeparatedTags(props.value, props.separator)
  return normalizeTagList(parsed, props.maxCount)
})

/** 单标签文案 */
const singleLabel = computed(() => {
  const text = props.label?.trim()
  return text || ''
})

/** 单标签配色序号 */
const colorIndex = computed(() => {
  const num = props.index
  return Number.isFinite(num) ? Math.trunc(num) : 0
})

/**
 * 列表项标签 class（color-base 色板）
 * @param index 序号
 * @returns class 数组
 */
function buildTagColorClass(index: number): string[] {
  return [TAKT_TAG_COLOR_BASE_CLASS, resolveTaktTagColorClass(index), '!m-0']
}

/**
 * 单标签 class
 * @returns class 数组
 */
function buildSingleTagClass(): string[] {
  const classes = buildTagColorClass(colorIndex.value)
  if (props.disabled) {
    classes.push('opacity-60')
  }
  return classes
}

/**
 * 单标签关闭
 */
function handleClose() {
  if (props.disabled) {
    return
  }
  emit('close')
}
</script>

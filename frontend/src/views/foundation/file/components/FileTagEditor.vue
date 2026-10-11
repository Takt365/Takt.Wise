<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/file/components -->
<!-- 文件名称：FileTagEditor.vue -->
<!-- 功能描述：文件标签编辑（takt-tag-color + 虚线新增）；v-model 为逗号分隔字符串 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex flex-wrap items-center gap-2 w-full">
    <TaktTagColor
      v-for="(tag, tagIndex) in tagList"
      :key="tag"
      :label="tag"
      :index="tagIndex"
      closable
      :disabled="disabled"
      @close="handleClose(tag)"
    />
    <a-input
      v-if="tagInputState.inputVisible"
      ref="tagInputRef"
      v-model:value="tagInputState.inputValue"
      type="text"
      size="small"
      class="!w-20"
      :disabled="disabled"
      @blur="handleInputConfirm"
      @keyup.enter="handleInputConfirm"
    />
    <a-tag
      v-else-if="canAddTag"
      class="cursor-pointer bg-container border border-dashed border-border"
      @click="showInput"
    >
      <plus-outlined />
      {{ t('foundation.file.page.tags.add') }}
    </a-tag>
  </div>
</template>

<script setup lang="ts">
/**
 * 文件标签编辑器：takt-tag-color 预设色 + Ant Design 虚线新增交互
 */
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import { PlusOutlined } from '@ant-design/icons-vue'
import {
  FILE_TAG_MAX_COUNT,
  joinFileTags,
  parseFileTags,
} from '@/utils/taktFileTags'

/** i18n 翻译函数 */
const { t } = useI18n()

/** v-model：逗号分隔标签字符串 */
const modelValue = defineModel<string>({ default: '' })

/** 组件 Props */
interface Props {
  /** 禁用交互 */
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  disabled: false,
})

/** 新增标签输入态 */
const tagInputState = reactive({
  inputVisible: false,
  inputValue: '',
})
/** 新增标签输入框 ref */
const tagInputRef = ref<{ focus: () => void } | null>(null)

/** 当前标签列表 */
const tagList = computed(() => parseFileTags(modelValue.value))

/** 是否还可添加标签 */
const canAddTag = computed(() => !props.disabled && tagList.value.length < FILE_TAG_MAX_COUNT)

/**
 * 关闭输入态并清空草稿
 */
function resetTagInputState() {
  tagInputState.inputVisible = false
  tagInputState.inputValue = ''
}

/** 外部清空 model 时同步关闭输入态 */
watch(modelValue, (value) => {
  if (!value?.trim()) {
    resetTagInputState()
  }
})

/**
 * 删除标签
 * @param removedTag 待移除标签
 */
function handleClose(removedTag: string) {
  if (props.disabled) {
    return
  }
  modelValue.value = joinFileTags(tagList.value.filter((item) => item !== removedTag))
}

/**
 * 显示新增标签输入框
 */
function showInput() {
  if (!canAddTag.value) {
    return
  }
  tagInputState.inputVisible = true
  void nextTick(() => {
    tagInputRef.value?.focus()
  })
}

/**
 * 确认新增标签（blur / Enter）
 */
function handleInputConfirm() {
  const inputValue = tagInputState.inputValue.trim()
  if (!inputValue) {
    resetTagInputState()
    return
  }
  if (tagList.value.length >= FILE_TAG_MAX_COUNT) {
    message.warning(t('foundation.file.page.tags.max.limit', { max: FILE_TAG_MAX_COUNT }))
    resetTagInputState()
    return
  }
  if (tagList.value.includes(inputValue)) {
    resetTagInputState()
    return
  }
  modelValue.value = joinFileTags([...tagList.value, inputValue])
  resetTagInputState()
}
</script>
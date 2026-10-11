<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktCronEditor -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2026-06-28 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Quartz Cron 输入框 + 可视化弹窗（参照博客园 BlackCatFish 自定义 cron 组件） -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="takt-cron-editor w-full min-w-0"
    @click="openModal"
  >
    <a-input
      :value="displayExpression"
      readonly
      :disabled="disabled"
      :placeholder="props.placeholder || t('foundation.quartz-task.page.cron.inputPlaceholder')"
      class="cursor-pointer"
    >
      <template #suffix>
        <span
          role="button"
          tabindex="0"
          class="inline-flex items-center text-text-secondary hover:text-primary cursor-pointer"
          :aria-label="t('foundation.quartz-task.page.cron.modalTitle')"
          @click.stop="openModal"
          @keydown.enter.prevent="openModal"
        >
          <RiEditLine class="takt-remix-icon" />
        </span>
      </template>
    </a-input>
    <Teleport to="body">
      <TaktCronModal
        v-if="modalOpen"
        v-model:open="modalOpen"
        :expression="modelValue ?? ''"
        @confirm="handleConfirm"
      />
    </Teleport>
  </div>
</template>

<script setup lang="ts">
/**
 * Quartz Cron 表单控件：只读输入框 + 配置弹窗
 * @module components/business/TaktCronEditor
 */
import { computed, ref, defineAsyncComponent } from 'vue'
import { useI18n } from 'vue-i18n'
import { RiEditLine } from '@remixicon/vue'

const TaktCronModal = defineAsyncComponent(() =>
  import('@/components/business/TaktCronEditor/TaktCronModal.vue'),
)

interface Props {
  /** 绑定 Quartz Cron 表达式 */
  modelValue?: string
  /** 禁用 */
  disabled?: boolean
  /** 输入框占位符；未传时使用 foundation.quartz-task.page.cron.inputPlaceholder */
  placeholder?: string
}

interface Emits {
  (event: 'update:modelValue', value: string): void
  (event: 'error', value: string): void
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: '',
  disabled: false,
})

const emit = defineEmits<Emits>()

const { t } = useI18n()

/** Cron 配置弹窗可见 */
const modalOpen = ref(false)

/** 输入框展示文本 */
const displayExpression = computed(() => String(props.modelValue ?? '').trim())

/** 打开 Cron 配置弹窗 */
function openModal() {
  if (props.disabled) {
    return
  }
  modalOpen.value = true
}

/**
 * 弹窗确定：回写表达式
 * @param value Quartz Cron
 */
function handleConfirm(value: string) {
  emit('update:modelValue', value)
  emit('error', '')
}
</script>

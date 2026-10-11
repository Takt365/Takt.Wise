<!-- ======================================== -->
<!-- 项目名称：TaktWise -->
<!-- 命名空间：@/components/business/TaktModal -->
<!-- 文件名称：index.vue -->
<!-- 创建时间：2025-01-21 -->
<!-- 创建人：Takt365(Cursor AI) -->
<!-- 功能描述：Takt 对话框组件，封装 a-modal；右上角全屏与关闭始终显示，无需额外参数 -->
<!--  -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-modal
    v-model:open="internalOpen"
    v-bind="modalProps"
    @cancel="handleCancel"
  >
    <template
      v-if="$slots.default"
      #default
    >
      <slot />
    </template>
    <template
      v-if="$slots.title"
      #title
    >
      <slot name="title" />
    </template>
    <template #closeIcon>
      <span
        class="takt-modal-close-area"
        @click.stop
      >
        <span
          role="button"
          tabindex="0"
          class="takt-modal-header-tool-btn"
          :aria-label="isFs ? t('common.page.button.exitfullscreen') : t('common.page.button.fullscreen')"
          @click.stop="toggleFs"
          @keydown="handleHeaderToolKeydown($event, toggleFs)"
        >
          <RiFullscreenExitLine
            v-if="isFs"
            class="takt-remix-icon"
          />
          <RiFullscreenLine
            v-else
            class="takt-remix-icon"
          />
        </span>
        <span
          role="button"
          tabindex="0"
          class="takt-modal-header-tool-btn"
          :aria-label="t('common.page.button.close')"
          @click.stop="handleHeaderCloseClick"
          @keydown="handleHeaderToolKeydown($event, handleHeaderCloseClick)"
        >
          <RiCloseLine class="takt-remix-icon" />
        </span>
      </span>
    </template>
    <template
      v-if="!hideFooter"
      #footer
    >
      <slot name="footer">
        <div class="flex justify-end gap-2 [&_.ant-btn]:inline-flex [&_.ant-btn]:items-center [&_.ant-btn]:gap-1 [&_.anticon]:!me-0">
          <a-button
            @click="handleCancel"
          >
            <template #icon>
              <RiCloseLine class="takt-remix-icon" />
            </template>
            {{ cancelText ?? t('common.page.button.cancel') }}
          </a-button>
          <a-button
            type="primary"
            :loading="props.confirmLoading"
            @click="handleOk"
          >
            <template #icon>
              <RiCheckLine class="takt-remix-icon" />
            </template>
            {{ okText ?? t('common.page.button.submit') }}
          </a-button>
        </div>
      </slot>
    </template>
  </a-modal>
</template>

<script setup lang="ts">
import { RiCloseLine, RiCheckLine, RiFullscreenLine, RiFullscreenExitLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import type { CSSProperties } from 'vue'

defineOptions({
  inheritAttrs: false,
})

const { t } = useI18n()

interface Props {
  /** 确定按钮文本,默认为"提交" */
  okText?: string | undefined
  /** 取消按钮文本,默认为"取消" */
  cancelText?: string | undefined
  /** 确定按钮 loading（与 a-modal confirmLoading 对齐） */
  confirmLoading?: boolean
  /** 是否显示对话框 */
  open?: boolean
  /** 是否使用视口默认尺寸（70vw×85vh）；紧凑弹窗（如验证码）设为 false */
  useViewportSize?: boolean
  /** 是否隐藏底栏（仅保留标题栏关闭按钮，如验证码弹窗） */
  hideFooter?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  okText: undefined,
  cancelText: undefined,
  confirmLoading: false,
  open: false,
  useViewportSize: true,
  hideFooter: false,
})

const emit = defineEmits<{
  'update:open': [open: boolean]
  'ok': [e: MouseEvent]
  'cancel': [e: MouseEvent]
  'fullscreen': [isFullscreen: boolean]
}>()

const attrs = useAttrs()

/** 弹窗是否铺满视口（非浏览器 Fullscreen API） */
const isFs = ref(false)

/**
 * 合并行内样式对象
 * @param base 透传样式
 * @param override 全屏态覆盖
 * @returns {Record<string, unknown>} 合并结果
 */
function mergeStyleRecord(
  base: unknown,
  override: CSSProperties,
): CSSProperties {
  const normalized = base && typeof base === 'object' && !Array.isArray(base)
    ? base as CSSProperties
    : {}
  return { ...normalized, ...override }
}

/**
 * 标题栏工具按钮键盘激活（Enter / Space）
 * @param e 键盘事件
 * @param action 点击回调
 */
function handleHeaderToolKeydown(e: KeyboardEvent, action: () => void) {
  if (e.key !== 'Enter' && e.key !== ' ') {
    return
  }
  e.preventDefault()
  action()
}

/**
 * 切换视口全屏
 */
function toggleFs() {
  isFs.value = !isFs.value
  emit('fullscreen', isFs.value)
}

/**
 * 标题栏关闭
 */
function handleHeaderCloseClick() {
  handleCancel(new MouseEvent('click'))
}

/** 内部 open 状态 */
const internalOpen = computed({
  get: () => props.open,
  set: (value: boolean) => {
    emit('update:open', value)
  },
})

/** 计算 modal 属性：默认 70vw×85vh、居中；全屏态铺满视口 */
const modalProps = computed(() => {
  const {
    centered,
    closable: _closable,
    style,
    bodyStyle,
    'show-fullscreen': _showFullscreenKebab,
    showFullscreen: _showFullscreen,
    ...rest
  } = attrs
  const wrapClassName = [
    rest.wrapClassName,
    'takt-modal',
    props.useViewportSize && !isFs.value ? 'takt-modal-viewport-size' : null,
    isFs.value ? 'takt-modal-is-fs' : null,
  ]
    .filter(Boolean)
    .join(' ')
  const defaultWidth = rest.width !== undefined && rest.width !== null ? rest.width : '70vw'
  const normalizedCentered = typeof centered === 'boolean' ? centered : true
  const fsBodyOffsetPx = props.hideFooter ? 56 : 108
  const base = {
    ...rest,
    wrapClassName,
    centered: isFs.value ? false : normalizedCentered,
    closable: true,
    ...(props.hideFooter ? { footer: null } : {}),
  }
  if (!isFs.value) {
    return {
      ...base,
      width: defaultWidth as string | number,
      style: style as CSSProperties | undefined,
      bodyStyle: bodyStyle as CSSProperties | undefined,
    }
  }
  return {
    ...base,
    width: '100vw',
    style: mergeStyleRecord(style, { top: 0, paddingBottom: 0 }),
    bodyStyle: mergeStyleRecord(bodyStyle, {
      height: `calc(100vh - ${fsBodyOffsetPx}px)`,
      overflow: 'auto',
    }),
  }
})

/** 处理确定按钮点击 */
const handleOk = (e: MouseEvent) => {
  emit('ok', e)
}

/** 处理取消按钮点击 */
const handleCancel = (e: MouseEvent) => {
  isFs.value = false
  emit('cancel', e)
  emit('update:open', false)
}

watch(
  () => props.open,
  (open) => {
    if (!open) {
      isFs.value = false
    }
  },
)
</script>

<!-- 不 scoped：弹窗 teleport 到 body；宽度由内层 .ant-modal 的内联 width 决定 -->
<style>
.takt-modal.ant-modal-wrap {
  --takt-modal-header-tools-border-gap: 48px;
}
.takt-modal-viewport-size.ant-modal-wrap {
  display: flex;
  align-items: center;
  justify-content: center;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal,
.takt-modal-is-fs.ant-modal-wrap .ant-modal {
  top: 0;
  margin: 0;
  padding-bottom: 0;
}
.takt-modal-is-fs.ant-modal-wrap .ant-modal {
  max-width: 100vw;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-content {
  width: 100%;
  max-width: 100%;
  box-sizing: border-box;
  height: 85vh;
  min-width: 0;
  min-height: 360px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-body {
  flex: 1;
  min-height: 0;
  overflow: auto;
}
.takt-modal-viewport-size.ant-modal-wrap .ant-modal-footer {
  flex-shrink: 0;
}
.takt-modal-is-fs.ant-modal-wrap .ant-modal-content {
  width: 100%;
  max-width: 100vw;
  box-sizing: border-box;
}
.takt-modal.ant-modal-wrap .ant-modal-close {
  width: auto !important;
  height: auto !important;
  line-height: 1;
  padding: 0 !important;
  inset-inline-end: calc(var(--takt-modal-header-tools-border-gap) - var(--ant-padding-content-horizontal-lg, 24px)) !important;
  right: calc(var(--takt-modal-header-tools-border-gap) - var(--ant-padding-content-horizontal-lg, 24px)) !important;
  color: transparent !important;
  background-color: transparent !important;
}
.takt-modal.ant-modal-wrap .ant-modal-close:is(:hover, :active, :focus) {
  color: transparent !important;
  background-color: transparent !important;
}
.takt-modal.ant-modal-wrap .ant-modal-close-x {
  display: inline-flex !important;
  align-items: center;
  gap: 8px;
  width: auto !important;
  height: auto !important;
  line-height: 1 !important;
  font-size: inherit;
  text-align: inherit;
  color: inherit;
  background: transparent;
}
.takt-modal-close-area {
  display: contents;
}
.takt-modal-header-tool-btn {
  box-sizing: border-box;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: var(--ant-modal-close-btn-size, 22px);
  height: var(--ant-modal-close-btn-size, 22px);
  padding: 0;
  margin: 0;
  border: 0;
  outline: 0;
  background: transparent;
  border-radius: var(--ant-border-radius-sm, 4px);
  cursor: pointer;
  color: var(--ant-color-text-quaternary, rgba(0, 0, 0, 0.45));
  font-size: var(--ant-font-size-lg, 16px);
  line-height: 1;
  transition: color 0.2s, background-color 0.2s;
}
.takt-modal-header-tool-btn .takt-remix-icon {
  width: 1em;
  height: 1em;
  flex-shrink: 0;
  color: inherit;
}
.takt-modal-header-tool-btn:hover,
.takt-modal-header-tool-btn:focus-visible {
  color: var(--ant-color-text, rgba(0, 0, 0, 0.85));
  background-color: var(--ant-color-fill-content, rgba(0, 0, 0, 0.06));
}
.takt-modal-header-tool-btn:active {
  color: var(--ant-color-text, rgba(0, 0, 0, 0.85));
  background-color: var(--ant-color-fill-content-hover, rgba(0, 0, 0, 0.1));
}
</style>

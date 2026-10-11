<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktCaptchaSlider -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：滑块拼图验证码弹窗（仅标题栏关闭；验证通过后自动提交） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <component
    :is="embedded ? 'div' : TaktModal"
    v-model:open="open"
    v-bind="wrapperProps"
    @cancel="handleModalCancel"
  >
    <a-spin :spinning="loading">
      <div
        v-if="challenge"
        :ref="slider.containerRef"
        class="mx-auto mb-4 flex w-full justify-center"
      >
        <div
          class="flex flex-col items-center overflow-hidden"
          :style="scaledStyle"
        >
          <div class="shrink-0">
            <div class="relative flex w-full flex-col items-center gap-2">
              <div
                class="shrink-0 overflow-hidden"
                :style="imageWrapStyle"
              >
                <div
                  class="shrink-0"
                  :style="imageScaledStyle"
                >
                  <div
                    :ref="slider.imageRef"
                    class="relative overflow-hidden rounded border border-border"
                    :style="imageAreaStyle"
                  >
                    <img
                      v-if="backgroundSrc"
                      :src="backgroundSrc"
                      class="block h-full w-full rounded object-none transition-opacity duration-200"
                      :class="!dragCompleted && 'cursor-pointer hover:opacity-85'"
                      alt=""
                      draggable="false"
                      @click="handleBackgroundClick"
                    >
                    <div
                      v-else
                      class="block min-h-full w-full rounded bg-gradient-to-br from-[#4f6ef7] to-[#7b4fd6]"
                      :class="!dragCompleted && 'cursor-pointer hover:opacity-85'"
                      @click="handleBackgroundClick"
                    />

                    <img
                      v-if="sliderSrc"
                      :src="sliderSrc"
                      class="pointer-events-none absolute top-1/2 z-10 -translate-y-1/2 rounded transition-[left] duration-75 will-change-[left]"
                      :style="puzzlePieceStyle"
                      alt=""
                      draggable="false"
                    >
                    <div
                      v-else
                      class="pointer-events-none absolute top-1/2 z-10 -translate-y-1/2 rounded bg-white/95 shadow-md transition-[left] duration-75 will-change-[left]"
                      :style="puzzlePieceStyle"
                    />
                  </div>
                </div>
              </div>

              <div
                class="shrink-0 overflow-hidden"
                :style="trackWrapStyle"
              >
                <div
                  class="shrink-0"
                  :style="trackScaledStyle"
                >
                  <div
                    :ref="slider.trackRef"
                    class="relative h-10 rounded border border-border bg-[var(--ant-color-fill-quaternary)] dark:bg-[#262626]"
                    :style="trackBarStyle"
                  >
                    <div
                      class="pointer-events-none absolute inset-0 z-0 rounded bg-gradient-to-r from-[var(--ant-color-fill-quaternary)] to-[var(--ant-color-bg-container)] dark:from-[#262626] dark:to-[#1f1f1f]"
                    />

                    <div
                      class="absolute -top-px z-10 box-border flex items-center justify-center rounded border border-primary bg-primary hover:opacity-90"
                      :class="[
                        isDragging ? 'cursor-no-drop' : dragCompleted ? 'cursor-default' : 'cursor-ew-resize',
                        isDragging && '!transition-none',
                      ]"
                      :style="{ ...thumbStyle, width: handleWidthPx, height: '40px' }"
                      @mousedown="handleDragStart"
                      @touchstart="handleDragStart"
                    >
                      <RiArrowRightDoubleLine
                        v-if="!dragCompleted"
                        class="pointer-events-none text-white takt-remix-icon"
                      />
                      <RiCheckLine
                        v-else
                        class="pointer-events-none text-white takt-remix-icon"
                      />
                    </div>

                    <div
                      class="absolute left-0 top-0 z-[1] h-full rounded-l bg-primary transition-[width] duration-75 ease-[cubic-bezier(0.4,0,0.2,1)] will-change-[width]"
                      :class="isDragging && '!transition-none'"
                      :style="{ width: progressBarWidth }"
                    />

                    <div
                      class="pointer-events-none absolute inset-0 z-[2] flex select-none items-center text-xs"
                      :class="[
                        dragCompleted ? 'takt-captcha-success text-white' : 'justify-center',
                        dragCompleted && successTextPosition === 'left' && 'justify-start pl-3',
                        dragCompleted && successTextPosition === 'right' && 'justify-end pr-3',
                        (!dragCompleted || successTextPosition === 'center') && 'justify-center',
                      ]"
                    >
                      <a-typography-text
                        v-if="!dragCompleted"
                        class="takt-captcha-shine"
                      >
                        {{ t('login.page.captcha.drag.hint') }}
                      </a-typography-text>
                      <a-typography-text
                        v-else
                        class="font-medium opacity-95"
                      >
                        {{ t('login.page.captcha.success') }}
                      </a-typography-text>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </a-spin>
  </component>
</template>

<script setup lang="ts">
/**
 * 滑块拼图验证码（弹窗或步骤条内嵌；验证通过后由父级 buildCaptchaCode）
 */
import { RiArrowRightDoubleLine, RiCheckLine } from '@remixicon/vue';
import { useI18n } from 'vue-i18n';
import TaktModal from '@/components/business/TaktModal/index.vue';
import { useTaktCaptchaSlider } from '@/composables/useTaktCaptchaSlider';
import type { TaktCaptchaChallengeDto } from '@/types/identity/captcha.types';

interface Props {
  /**
   * 验证码挑战数据（由登录页拉取后传入）
   */
  challenge: TaktCaptchaChallengeDto | null;
  /**
   * 是否正在拉取挑战
   */
  loading?: boolean;
  /**
   * 为 true 时内嵌于步骤条，不渲染 takt-modal
   */
  embedded?: boolean;
}

interface Emits {
  /**
   * 是否可提交（拖动完成后为 true）
   */
  (event: 'can-submit-change', value: boolean): void;
  /**
   * 请求父级重新拉取挑战（点击背景图刷新）
   */
  (event: 'request-refresh'): void;
  /**
   * 用户点击标题栏关闭
   */
  (event: 'cancel'): void;
}

const open = defineModel<boolean>('open', { default: false });

const props = withDefaults(defineProps<Props>(), {
  loading: false,
  embedded: false,
});

const emit = defineEmits<Emits>();

const { t } = useI18n();

/** 弹窗模式下的 takt-modal 透传属性 */
const wrapperProps = computed(() => {
  if (props.embedded) {
    return { class: 'w-full' };
  }

  return {
    title: t('login.page.captcha.title'),
    useViewportSize: false,
    hideFooter: true,
    width: '460px',
    maskClosable: false,
    destroyOnClose: true,
  };
});

const challengeRef = toRef(props, 'challenge');
const slider = useTaktCaptchaSlider(challengeRef);
const {
  isDragging,
  dragCompleted,
  canSubmit,
  scaledStyle,
  imageWrapStyle,
  imageScaledStyle,
  imageAreaStyle,
  trackWrapStyle,
  trackScaledStyle,
  trackBarStyle,
  puzzlePieceStyle,
  progressBarWidth,
  thumbStyle,
  successTextPosition,
  handleDragStart,
  resetDragState,
  updateScale,
} = slider;

const handleWidthPx = computed(() => `${props.challenge?.sliderWidth ?? 48}px`);

/**
 * 背景图 data URL
 */
const backgroundSrc = computed(() => {
  const src = props.challenge?.backgroundImage;
  if (!src) {
    return undefined;
  }
  return src.startsWith('data:') ? src : `data:image/jpeg;base64,${src}`;
});

/**
 * 滑块拼图 data URL
 */
const sliderSrc = computed(() => {
  const src = props.challenge?.sliderImage;
  if (!src) {
    return undefined;
  }
  return src.startsWith('data:') ? src : `data:image/png;base64,${src}`;
});

watch(
  () => props.challenge?.captchaId,
  () => {
    resetDragState();
    updateScale();
  },
);

watch(canSubmit, (value) => {
  emit('can-submit-change', value);
}, { immediate: true });

/**
 * 标题栏关闭
 */
function handleModalCancel(): void {
  emit('cancel');
}

/**
 * 点击背景图请求刷新挑战
 */
function handleBackgroundClick(): void {
  if (dragCompleted.value) {
    return;
  }
  resetDragState();
  emit('request-refresh');
}

/**
 * 构建并返回提交 JSON 字符串
 * @returns {string} captchaCode
 */
function buildCaptchaCode(): string {
  return JSON.stringify(slider.buildSubmissionPayload());
}

defineExpose({
  buildCaptchaCode,
  canSubmit,
  resetDragState,
});
</script>

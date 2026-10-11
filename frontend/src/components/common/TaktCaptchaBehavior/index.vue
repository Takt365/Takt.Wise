<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktCaptchaBehavior -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：行为验证码弹窗（仅标题栏关闭；验证通过后自动提交） -->
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
        :ref="behavior.containerRef"
        class="mx-auto mb-4 flex w-full justify-center"
      >
        <div
          class="flex flex-col items-center overflow-hidden"
          :style="scaledStyle"
        >
          <div
            class="shrink-0"
            :style="innerScaledStyle"
          >
            <div class="relative flex w-full flex-col items-center gap-2">
              <div
                :ref="behavior.wrapperRef"
                class="relative overflow-hidden rounded border border-border bg-[var(--ant-color-fill-quaternary)] text-center dark:bg-[#262626]"
                :class="[
                  verified && 'pointer-events-none',
                  isDragging && !verified && '[&_.takt-captcha-progress]:transition-none',
                ]"
                :style="sliderTrackStyle"
                @mouseleave="handleDragOver"
                @mousemove="handleDragMoving"
                @mouseup="handleDragOver"
                @touchend="handleDragOver"
                @touchmove="handleDragMoving"
              >
                <div
                  :ref="behavior.barRef"
                  class="takt-captcha-progress absolute left-0 top-0 h-full w-0 bg-feicui transition-[width] duration-200 ease-[cubic-bezier(0.4,0,0.2,1)]"
                  :class="toLeft && '!w-0 !transition-[width] duration-300 ease-in-out'"
                />

                <div
                  :ref="behavior.contentRef"
                  class="pointer-events-none absolute inset-0 z-[2] flex select-none items-center justify-center text-xs"
                  :class="verified && 'takt-captcha-success text-white'"
                >
                  <a-typography-text
                    v-if="!verified"
                    class="takt-captcha-shine"
                  >
                    {{
                      hasTargetIndicator
                        ? t('login.page.captcha.slide.to.target', { position: targetPosition })
                        : t('login.page.captcha.behavior.hint')
                    }}
                  </a-typography-text>
                  <a-typography-text
                    v-else
                    class="font-medium opacity-95"
                  >
                    {{ t('login.page.captcha.success') }}
                  </a-typography-text>
                </div>

                <div
                  v-if="!verified && hasTargetIndicator"
                  class="pointer-events-none absolute top-0 bottom-0 z-[1] -translate-x-1/2"
                  :style="targetIndicatorStyle"
                >
                  <div class="mx-auto h-full w-0.5 bg-[var(--ant-color-error)]" />
                </div>

                <div
                  :ref="behavior.actionRef"
                  class="absolute left-0 top-0 z-10 box-border flex h-full cursor-ew-resize select-none items-center justify-center rounded border border-primary bg-primary hover:opacity-90"
                  :style="{ width: handleWidthPx }"
                  :class="{
                    'cursor-no-drop !transition-none': isDragging && !verified,
                    '!left-0 !transition-[left] duration-300 ease-in-out': toLeft,
                    'cursor-default': verified,
                  }"
                  @mousedown.stop="handleDragStart"
                  @touchstart.stop="handleDragStart"
                >
                  <RiArrowRightDoubleLine
                    v-if="!verified"
                    class="pointer-events-none size-4 text-white takt-remix-icon"
                  />
                  <RiCheckLine
                    v-else
                    class="pointer-events-none size-4 text-white takt-remix-icon"
                  />
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
 * 行为验证码（弹窗或步骤条内嵌；验证通过后由父级 buildCaptchaCode）
 */
import { RiArrowRightDoubleLine, RiCheckLine } from '@remixicon/vue';
import { useI18n } from 'vue-i18n';
import TaktModal from '@/components/business/TaktModal/index.vue';
import { useTaktCaptchaBehavior } from '@/composables/useTaktCaptchaBehavior';
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
   * 是否可提交（拖到目标附近后为 true）
   */
  (event: 'can-submit-change', value: boolean): void;
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
const behavior = useTaktCaptchaBehavior(challengeRef);
const {
  verified,
  isDragging,
  toLeft,
  canSubmit,
  targetPosition,
  scaledStyle,
  innerScaledStyle,
  sliderTrackStyle,
  targetIndicatorStyle,
  handleDragStart,
  handleDragMoving,
  handleDragOver,
  resetDragState,
  updateScale,
} = behavior;

const hasTargetIndicator = computed(() => props.challenge?.targetPosition != null);

const handleWidthPx = computed(() => `${props.challenge?.sliderWidth ?? 48}px`);

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
 * 构建并返回提交 JSON 字符串
 * @returns {string} captchaCode
 */
function buildCaptchaCode(): string {
  const payload = behavior.buildSubmissionPayload();
  if (props.challenge?.requireBehaviorData && !payload.mouseTrajectory?.length) {
    const finalLeft = Number.parseFloat(behavior.actionRef.value?.style.left.replace('px', '') || '0');
    payload.mouseTrajectory = [{ x: finalLeft, y: 0, t: 0 }];
  }
  return JSON.stringify(payload);
}

defineExpose({
  buildCaptchaCode,
  canSubmit,
  resetDragState,
});
</script>

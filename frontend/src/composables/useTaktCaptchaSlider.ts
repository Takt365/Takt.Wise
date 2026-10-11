// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktCaptchaSlider.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：滑块拼图验证码拖动、图片区联动与提交载荷（与后端 Slider 校验一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed, nextTick, onMounted, onUnmounted, ref, watch, type Ref } from 'vue';
import type {
  TaktCaptchaChallengeDto,
  TaktCaptchaSubmissionDto,
  TaktCaptchaTrajectoryPointDto,
} from '@/types/identity/captcha.types';
import { TAKT_CAPTCHA_SLIDER_TRACK_HEIGHT } from '@/utils/common';

export { TAKT_CAPTCHA_SLIDER_TRACK_HEIGHT };

/**
 * 滑块拼图验证码交互（轨道手柄驱动拼图块，position 为拼图 left 占画布宽度百分比）
 * @param challenge 当前挑战数据
 * @returns 拖动状态、缩放样式与提交构建
 */
export function useTaktCaptchaSlider(challenge: Ref<TaktCaptchaChallengeDto | null>) {
  const containerRef = ref<HTMLDivElement | null>(null);
  const imageRef = ref<HTMLElement | null>(null);
  const trackRef = ref<HTMLElement | null>(null);

  const scale = ref(1);
  const isDragging = ref(false);
  const dragCompleted = ref(false);
  const thumbLeft = ref(0);
  const maxLeft = ref(0);
  const dragStartX = ref(0);
  const startThumbLeft = ref(0);
  const startTime = ref(0);
  const mouseTrajectory = ref<TaktCaptchaTrajectoryPointDto[]>([]);

  const designWidth = computed(() => challenge.value?.width ?? 400);
  const designImageHeight = computed(() => challenge.value?.height ?? 100);
  const thumbWidth = computed(() => challenge.value?.sliderWidth ?? 48);

  const canSubmit = computed(
    () => challenge.value != null && dragCompleted.value && thumbLeft.value > 0 && !isDragging.value,
  );

  const scaledStyle = computed(() => ({
    width: `${designWidth.value * scale.value}px`,
    overflow: 'hidden',
    margin: '0 auto',
  }));

  const imageWrapStyle = computed(() => ({
    width: `${designWidth.value * scale.value}px`,
    height: `${designImageHeight.value * scale.value}px`,
    overflow: 'hidden',
  }));

  const imageScaledStyle = computed(() => ({
    transform: `scale(${scale.value})`,
    transformOrigin: 'top center',
    width: `${designWidth.value}px`,
    height: `${designImageHeight.value}px`,
  }));

  const imageAreaStyle = computed(() => ({
    width: `${designWidth.value}px`,
    height: `${designImageHeight.value}px`,
  }));

  const trackWrapStyle = computed(() => ({
    width: `${designWidth.value * scale.value}px`,
    height: `${TAKT_CAPTCHA_SLIDER_TRACK_HEIGHT}px`,
    overflow: 'hidden',
  }));

  const trackScaledStyle = computed(() => ({
    transform: `scaleX(${scale.value})`,
    transformOrigin: 'top center',
    width: `${designWidth.value}px`,
    height: `${TAKT_CAPTCHA_SLIDER_TRACK_HEIGHT}px`,
  }));

  const trackBarStyle = computed(() => ({
    width: `${designWidth.value}px`,
  }));

  /**
   * 拼图块在背景图上的 left（像素）
   */
  const puzzleLeft = computed(() => {
    if (maxLeft.value <= 0) {
      return 0;
    }
    const imageWidth = designWidth.value;
    const pieceWidth = thumbWidth.value;
    return Math.round((thumbLeft.value / maxLeft.value) * (imageWidth - pieceWidth));
  });

  const puzzlePieceStyle = computed(() => ({
    left: `${puzzleLeft.value}px`,
    width: `${challenge.value?.sliderWidth ?? 48}px`,
    height: `${challenge.value?.sliderHeight ?? 48}px`,
  }));

  const progressBarWidth = computed(() => `${thumbLeft.value + thumbWidth.value}px`);

  const thumbStyle = computed(() => ({
    left: `${thumbLeft.value}px`,
  }));

  /**
   * 验证成功文案对齐（避免被手柄遮挡）
   */
  const successTextPosition = computed((): 'left' | 'right' | 'center' => {
    if (!dragCompleted.value || maxLeft.value <= 0) {
      return 'center';
    }
    const handleW = thumbWidth.value;
    const thumbRight = thumbLeft.value + handleW;
    const trackWidth = designWidth.value;
    const trackCenter = trackWidth / 2;
    const thumbCenter = thumbLeft.value + handleW / 2;

    if (thumbCenter < trackCenter) {
      return thumbRight < trackWidth - 80 ? 'right' : 'center';
    }
    return thumbLeft.value > 80 ? 'left' : 'center';
  });

  /**
   * 初始化轨道可拖最大 left
   */
  async function initMaxLeft(): Promise<void> {
    await nextTick();
    if (trackRef.value) {
      maxLeft.value = Math.max(0, trackRef.value.clientWidth - thumbWidth.value);
    } else {
      maxLeft.value = Math.max(0, designWidth.value - thumbWidth.value);
    }
  }

  /**
   * 更新容器横向缩放
   */
  function updateScale(): void {
    const el = containerRef.value;
    if (!el) {
      return;
    }
    const w = el.offsetWidth || el.clientWidth;
    if (w > 0) {
      scale.value = Math.min(1, w / designWidth.value);
    }
  }

  let resizeObserver: ResizeObserver | null = null;

  /**
   * 获取 clientX
   * @param e 鼠标或触摸事件
   */
  function getEventX(e: MouseEvent | TouchEvent): number {
    if ('clientX' in e) {
      return e.clientX;
    }
    if ('touches' in e && e.touches[0]) {
      return e.touches[0].clientX;
    }
    return 0;
  }

  /**
   * 获取 clientY
   * @param e 鼠标或触摸事件
   */
  function getEventY(e: MouseEvent | TouchEvent): number {
    if ('clientY' in e) {
      return e.clientY;
    }
    if ('touches' in e && e.touches[0]) {
      return e.touches[0].clientY;
    }
    return 0;
  }

  /**
   * 记录相对图片区的轨迹点
   * @param e 指针事件
   */
  function appendTrajectoryFromEvent(e: MouseEvent | TouchEvent): void {
    if (!imageRef.value) {
      return;
    }
    const rect = imageRef.value.getBoundingClientRect();
    mouseTrajectory.value.push({
      x: getEventX(e) - rect.left,
      y: getEventY(e) - rect.top,
      t: Date.now() - startTime.value,
    });
  }

  let moveHandler: ((ev: MouseEvent | TouchEvent) => void) | null = null;
  let upHandler: ((ev: MouseEvent | TouchEvent) => void) | null = null;

  /**
   * 移除 document 级拖动监听
   */
  function detachDocumentListeners(): void {
    if (moveHandler) {
      document.removeEventListener('mousemove', moveHandler);
      document.removeEventListener('touchmove', moveHandler);
    }
    if (upHandler) {
      document.removeEventListener('mouseup', upHandler);
      document.removeEventListener('touchend', upHandler);
    }
    moveHandler = null;
    upHandler = null;
  }

  /**
   * 拖拽开始
   * @param e 鼠标或触摸事件
   */
  function handleDragStart(e: MouseEvent | TouchEvent): void {
    if (dragCompleted.value || !challenge.value || maxLeft.value <= 0) {
      return;
    }

    e.preventDefault();
    e.stopPropagation();

    isDragging.value = true;
    dragStartX.value = getEventX(e);
    startThumbLeft.value = thumbLeft.value;
    startTime.value = Date.now();
    mouseTrajectory.value = [];

    if (imageRef.value) {
      const rect = imageRef.value.getBoundingClientRect();
      mouseTrajectory.value.push({
        x: getEventX(e) - rect.left,
        y: getEventY(e) - rect.top,
        t: 0,
      });
    }

    moveHandler = (ev: MouseEvent | TouchEvent) => {
      if (!isDragging.value) {
        return;
      }
      ev.preventDefault();
      const moveX = getEventX(ev);
      let left = startThumbLeft.value + (moveX - dragStartX.value);
      left = Math.max(0, Math.min(left, maxLeft.value));
      thumbLeft.value = left;
      appendTrajectoryFromEvent(ev);
    };

    upHandler = (ev: MouseEvent | TouchEvent) => {
      if (!isDragging.value) {
        return;
      }
      isDragging.value = false;
      detachDocumentListeners();
      dragCompleted.value = thumbLeft.value > 0;
      appendTrajectoryFromEvent(ev);
    };

    document.addEventListener('mousemove', moveHandler);
    document.addEventListener('mouseup', upHandler);
    document.addEventListener('touchmove', moveHandler, { passive: false });
    document.addEventListener('touchend', upHandler);
  }

  /**
   * 计算提交 position 百分比（拼图块 left / 画布宽度）
   */
  function computePositionPercent(): number {
    const imageWidth = designWidth.value;
    const pieceWidth = thumbWidth.value;
    const sliderLeftPosition =
      maxLeft.value > 0 ? (thumbLeft.value / maxLeft.value) * (imageWidth - pieceWidth) : 0;
    return Math.round((sliderLeftPosition / imageWidth) * 100);
  }

  /**
   * 构建提交载荷
   */
  function buildSubmissionPayload(): TaktCaptchaSubmissionDto {
    const elapsedSeconds = startTime.value > 0 ? (Date.now() - startTime.value) / 1000 : 0;
    const payload: TaktCaptchaSubmissionDto = {
      position: computePositionPercent(),
      timeSpent: Number(elapsedSeconds.toFixed(2)),
    };

    if (challenge.value?.requireBehaviorData && mouseTrajectory.value.length > 0) {
      payload.mouseTrajectory = mouseTrajectory.value.map((p) => ({
        x: Math.round(p.x),
        y: Math.round(p.y),
        ...(p.t != null ? { t: p.t } : {}),
      }));
    }

    return payload;
  }

  /**
   * 重置拖动状态
   */
  function resetDragState(): void {
    detachDocumentListeners();
    isDragging.value = false;
    dragCompleted.value = false;
    thumbLeft.value = 0;
    dragStartX.value = 0;
    startThumbLeft.value = 0;
    startTime.value = 0;
    mouseTrajectory.value = [];
  }

  /**
   * 带动画复位滑块
   */
  function animateResetThumb(): void {
    const startLeft = thumbLeft.value;
    const animStart = Date.now();
    const duration = 300;

    const animate = () => {
      const elapsed = Date.now() - animStart;
      const progress = Math.min(elapsed / duration, 1);
      const easeOut = 1 - (1 - progress) ** 3;
      thumbLeft.value = startLeft * (1 - easeOut);

      if (progress < 1) {
        requestAnimationFrame(animate);
      } else {
        dragCompleted.value = false;
      }
    };

    requestAnimationFrame(animate);
  }

  watch(
    () => challenge.value?.captchaId,
    async () => {
      resetDragState();
      await initMaxLeft();
      updateScale();
    },
  );

  onMounted(async () => {
    updateScale();
    resizeObserver = new ResizeObserver(() => {
      updateScale();
      void initMaxLeft();
    });
    if (containerRef.value) {
      resizeObserver.observe(containerRef.value);
    }
    await initMaxLeft();
  });

  onUnmounted(() => {
    detachDocumentListeners();
    if (resizeObserver && containerRef.value) {
      resizeObserver.unobserve(containerRef.value);
      resizeObserver = null;
    }
  });

  return {
    containerRef,
    imageRef,
    trackRef,
    scale,
    isDragging,
    dragCompleted,
    thumbLeft,
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
    animateResetThumb,
    buildSubmissionPayload,
    updateScale,
    initMaxLeft,
  };
}

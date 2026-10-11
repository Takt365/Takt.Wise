// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/composables
// 文件名称：useTaktCaptchaBehavior.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：行为验证码滑轨拖动、位置百分比与提交载荷（与后端 Behavior 评分一致）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import { computed, onMounted, onUnmounted, ref, type Ref } from 'vue';
import type {
  TaktCaptchaChallengeDto,
  TaktCaptchaSubmissionDto,
  TaktCaptchaTrajectoryPointDto,
} from '@/types/identity/captcha.types';
import {
  TAKT_CAPTCHA_BEHAVIOR_TRACK_HEIGHT,
  TAKT_CAPTCHA_POSITION_TOLERANCE,
} from '@/utils/common';

export { TAKT_CAPTCHA_BEHAVIOR_TRACK_HEIGHT };

/**
 * 行为验证码滑轨交互（手柄右缘映射 0–100%，轨迹含相对耗时 t）
 * @param challenge 当前挑战数据
 * @returns 拖动状态、DOM 操作与提交构建
 */
export function useTaktCaptchaBehavior(challenge: Ref<TaktCaptchaChallengeDto | null>) {
  const containerRef = ref<HTMLDivElement | null>(null);
  const wrapperRef = ref<HTMLDivElement | null>(null);
  const actionRef = ref<HTMLDivElement | null>(null);
  const barRef = ref<HTMLDivElement | null>(null);
  const contentRef = ref<HTMLDivElement | null>(null);

  const verified = ref(false);
  const isDragging = ref(false);
  const toLeft = ref(false);
  const moveDistance = ref(0);
  const startTime = ref(0);
  const mouseTrajectory = ref<TaktCaptchaTrajectoryPointDto[]>([]);
  const scale = ref(1);

  const designWidth = computed(() => challenge.value?.width ?? 400);
  const actionWidth = computed(() => challenge.value?.sliderWidth ?? 48);
  const targetPosition = computed(() => challenge.value?.targetPosition ?? 0);

  const canSubmit = computed(() => verified.value && !isDragging.value);

  const scaledStyle = computed(() => ({
    width: `${designWidth.value * scale.value}px`,
    height: `${TAKT_CAPTCHA_BEHAVIOR_TRACK_HEIGHT}px`,
    overflow: 'hidden',
    margin: '0 auto',
  }));

  const innerScaledStyle = computed(() => ({
    transform: `scale(${scale.value}, 1)`,
    transformOrigin: 'top center',
    width: `${designWidth.value}px`,
  }));

  const sliderTrackStyle = computed(() => ({
    width: `${designWidth.value}px`,
    height: `${TAKT_CAPTCHA_BEHAVIOR_TRACK_HEIGHT}px`,
  }));

  /**
   * 滑轨尺寸（手柄右缘 0–100% 映射与红线位置共用）
   */
  function getTrackMetrics() {
    const wrapperWidth = wrapperRef.value?.offsetWidth ?? designWidth.value;
    const handleW = actionWidth.value;
    const minRight = handleW;
    const maxRight = Math.max(minRight, wrapperWidth);
    const effectiveWidth = Math.max(1, maxRight - minRight);
    return { handleW, wrapperWidth, minRight, maxRight, effectiveWidth };
  }

  /**
   * 目标红线对应的手柄 left（px）；无目标时返回 null
   */
  function getTargetLeftPx(): number | null {
    if (challenge.value?.targetPosition == null) {
      return null;
    }
    const { effectiveWidth } = getTrackMetrics();
    const clampedTarget = Math.min(Math.max(targetPosition.value, 0), 100);
    return (clampedTarget / 100) * effectiveWidth;
  }

  /**
   * 手柄可拖动的最大 left：有目标时止于红线，否则为滑轨右端
   */
  function getMaxDragLeft(): number {
    const { handleW, wrapperWidth } = getTrackMetrics();
    const trackEndLeft = Math.max(0, wrapperWidth - handleW - 6);
    const targetLeft = getTargetLeftPx();
    if (targetLeft != null) {
      return Math.min(trackEndLeft, targetLeft);
    }
    return trackEndLeft;
  }

  const targetIndicatorStyle = computed(() => {
    const { minRight, effectiveWidth } = getTrackMetrics();
    const clampedTarget = Math.min(Math.max(targetPosition.value, 0), 100);
    const rightX = minRight + (clampedTarget / 100) * effectiveWidth;

    return {
      left: `${rightX}px`,
    };
  });

  /**
   * 更新容器横向缩放（仅缩小不放大）
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
   * 获取事件 pageX
   * @param e 鼠标或触摸事件
   */
  function getEventPageX(e: MouseEvent | TouchEvent): number {
    if ('pageX' in e) {
      return e.pageX;
    }
    if ('touches' in e && e.touches[0]) {
      return e.touches[0].pageX;
    }
    return 0;
  }

  /**
   * 获取事件 pageY
   * @param e 鼠标或触摸事件
   */
  function getEventPageY(e: MouseEvent | TouchEvent): number {
    if ('pageY' in e) {
      return e.pageY;
    }
    if ('touches' in e && e.touches[0]) {
      return e.touches[0].pageY;
    }
    return 0;
  }

  /**
   * 滑轨可达偏移量（offset 为有目标时的红线 left，否则为滑轨右端）
   */
  function getOffset() {
    const { handleW, wrapperWidth } = getTrackMetrics();
    return { actionWidth: handleW, offset: getMaxDragLeft(), wrapperWidth };
  }

  /**
   * 设置手柄 left
   * @param val CSS left
   */
  function setActionLeft(val: string): void {
    if (actionRef.value) {
      actionRef.value.style.left = val;
    }
  }

  /**
   * 设置进度条宽度
   * @param val CSS width
   */
  function setBarWidth(val: string): void {
    if (barRef.value) {
      barRef.value.style.width = val;
    }
  }

  /**
   * 根据手柄右缘计算位置百分比 0–100
   * @param finalLeft 手柄 left（像素）
   */
  function computePositionPercent(finalLeft: number): number {
    const wrapperWidth = wrapperRef.value?.offsetWidth ?? designWidth.value;
    const handleW = actionWidth.value;
    const actionRight = finalLeft + handleW;
    const minRight = handleW;
    const maxRight = Math.max(minRight, wrapperWidth);
    const effectiveWidth = Math.max(1, maxRight - minRight);
    const clampedRight = Math.min(Math.max(actionRight, minRight), maxRight);
    return Math.round(((clampedRight - minRight) / effectiveWidth) * 100);
  }

  /**
   * 开始拖拽
   * @param e 鼠标或触摸事件
   */
  function handleDragStart(e: MouseEvent | TouchEvent): void {
    if (verified.value || !challenge.value || !actionRef.value) {
      return;
    }

    e.preventDefault();
    e.stopPropagation();

    const currentLeft = Number.parseInt(actionRef.value.style.left.replace('px', '') || '0', 10);
    moveDistance.value = getEventPageX(e) - currentLeft;
    startTime.value = Date.now();
    isDragging.value = true;
    mouseTrajectory.value = [];

    if (wrapperRef.value) {
      const rect = wrapperRef.value.getBoundingClientRect();
      mouseTrajectory.value.push({
        x: getEventPageX(e) - rect.left,
        y: getEventPageY(e) - rect.top,
        t: 0,
      });
    }
  }

  /**
   * 拖拽中
   * @param e 鼠标或触摸事件
   */
  function handleDragMoving(e: MouseEvent | TouchEvent): void {
    if (!isDragging.value || verified.value || !actionRef.value || !barRef.value) {
      return;
    }

    e.preventDefault();
    e.stopPropagation();

    const { actionWidth: handleW, offset } = getOffset();
    const moveX = getEventPageX(e) - moveDistance.value;

    if (wrapperRef.value) {
      const rect = wrapperRef.value.getBoundingClientRect();
      mouseTrajectory.value.push({
        x: getEventPageX(e) - rect.left,
        y: getEventPageY(e) - rect.top,
        t: Date.now() - startTime.value,
      });
    }

    const clampedLeft = Math.max(0, Math.min(moveX, offset));
    setActionLeft(`${clampedLeft}px`);
    setBarWidth(`${clampedLeft + handleW}px`);
  }

  /**
   * 标记验证通过并将滑块停在目标红线（无目标时停于当前位置）
   */
  function markVerified(): void {
    verified.value = true;
    isDragging.value = false;

    if (!actionRef.value) {
      return;
    }

    const { handleW } = getTrackMetrics();
    const targetLeft = getTargetLeftPx();
    const currentLeft = Number.parseFloat(actionRef.value.style.left.replace('px', '') || '0');
    const finalLeft = targetLeft ?? currentLeft;
    setActionLeft(`${finalLeft}px`);
    setBarWidth(`${finalLeft + handleW}px`);
  }

  /**
   * 拖拽结束：接近目标则通过，否则复位
   * @param e 鼠标或触摸事件
   */
  function handleDragOver(e: MouseEvent | TouchEvent): void {
    if (!isDragging.value || verified.value) {
      return;
    }

    e.preventDefault();
    e.stopPropagation();
    isDragging.value = false;

    if (!actionRef.value || !barRef.value) {
      return;
    }

    const moveX = getEventPageX(e) - moveDistance.value;
    const { actionWidth: handleW, offset } = getOffset();

    const finalLeft = Math.max(0, Math.min(moveX, offset));

    setActionLeft(`${finalLeft}px`);
    setBarWidth(`${finalLeft + handleW}px`);

    const currentPosition = computePositionPercent(finalLeft);
    const targetPos = targetPosition.value;
    const hasTarget = challenge.value?.targetPosition != null;

    if (!hasTarget) {
      if (finalLeft > 0) {
        markVerified();
      } else {
        resumeUi();
      }
      return;
    }

    if (Math.abs(currentPosition - targetPos) <= TAKT_CAPTCHA_POSITION_TOLERANCE) {
      markVerified();
    } else {
      resumeUi();
    }
  }

  /**
   * 复位滑轨 UI（不重新拉取挑战）
   */
  function resumeUi(): void {
    verified.value = false;
    isDragging.value = false;
    moveDistance.value = 0;
    toLeft.value = false;
    startTime.value = 0;
    mouseTrajectory.value = [];

    if (!actionRef.value || !barRef.value) {
      return;
    }

    toLeft.value = true;
    setTimeout(() => {
      toLeft.value = false;
      setActionLeft('0');
      setBarWidth('0');
    }, 300);
  }

  /**
   * 重置全部状态（挑战切换时）
   */
  function resetDragState(): void {
    verified.value = false;
    isDragging.value = false;
    moveDistance.value = 0;
    toLeft.value = false;
    startTime.value = 0;
    mouseTrajectory.value = [];
    setActionLeft('0');
    setBarWidth('0');
  }

  /**
   * 构建提交载荷
   * @returns position、timeSpent、mouseTrajectory
   */
  function buildSubmissionPayload(): TaktCaptchaSubmissionDto {
    const finalLeft = Number.parseFloat(actionRef.value?.style.left.replace('px', '') || '0');
    const position = computePositionPercent(finalLeft);
    const elapsedSeconds = startTime.value > 0 ? (Date.now() - startTime.value) / 1000 : 0;
    const payload: TaktCaptchaSubmissionDto = {
      position,
      timeSpent: Number(elapsedSeconds.toFixed(2)),
    };

    if (challenge.value?.requireBehaviorData && mouseTrajectory.value.length > 0) {
      payload.mouseTrajectory = mouseTrajectory.value.map((p) => ({
        x: p.x,
        y: p.y,
        ...(p.t != null ? { t: p.t } : {}),
      }));
    }

    return payload;
  }

  onMounted(() => {
    updateScale();
    resizeObserver = new ResizeObserver(() => updateScale());
    if (containerRef.value) {
      resizeObserver.observe(containerRef.value);
    }
  });

  onUnmounted(() => {
    if (resizeObserver && containerRef.value) {
      resizeObserver.unobserve(containerRef.value);
      resizeObserver = null;
    }
  });

  return {
    containerRef,
    wrapperRef,
    actionRef,
    barRef,
    contentRef,
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
    buildSubmissionPayload,
    updateScale,
  };
}

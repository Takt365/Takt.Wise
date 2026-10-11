<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/business/TaktDrawSvg -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：登录展示插画：左右面板浮动动画（无文字） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="scopeRef"
    class="takt-draw-svg-root"
  >
    <div class="takt-draw-svg-stage">
      <div class="takt-draw-svg-panels">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 800 600"
          aria-hidden="true"
        >
          <g id="right-display">
            <g
              id="right-display-display"
              class="takt-draw-svg-right-display"
            >
              <path
                fill="#282e39"
                stroke="#0ff"
                stroke-miterlimit="10"
                stroke-width="5"
                d="M654.7 461H508.6c-10.5 0-15.8-8.5-12-19l26.2-72a29.9 29.9 0 0125.8-18.9h146c10.5 0 15.8 8.5 12 19l-26.2 72a29.9 29.9 0 01-25.7 18.8z"
                opacity=".8"
              />
              <g id="bars">
                <polygon
                  id="bar-3-btm"
                  fill="#34496a"
                  points="656.9 441.2 642.4 441.2 667.6 371.7 682.2 371.7 656.9 441.2"
                />
                <polygon
                  id="bar-3-top"
                  class="takt-draw-svg-bar-top"
                  fill="#0ff"
                  points="656.9 441.2 642.4 441.2 653 412 667.5 412 656.9 441.2"
                />
                <polygon
                  id="bar-2-btm"
                  fill="#34496a"
                  points="633.7 441.2 619.2 441.2 644.5 371.7 659 371.7 633.7 441.2"
                />
                <polygon
                  id="bar-2-top"
                  class="takt-draw-svg-bar-top"
                  fill="#0ff"
                  points="633.7 441.2 619.2 441.2 636 395.1 650.5 395.1 633.7 441.2"
                />
                <polygon
                  id="bar-1-btm"
                  fill="#34496a"
                  points="610.6 441.2 596.1 441.2 621.4 371.7 635.9 371.7 610.6 441.2"
                />
                <polygon
                  id="bar-1-top"
                  class="takt-draw-svg-bar-top"
                  fill="#0ff"
                  points="610.6 441.2 596.1 441.2 604 419.5 618.5 419.5 610.6 441.2"
                />
              </g>
              <g
                id="btns"
                fill="#0ff"
              >
                <ellipse
                  cx="546.8"
                  cy="379.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 546.8 379.3)"
                />
                <ellipse
                  cx="562.7"
                  cy="379.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 562.7 379.3)"
                />
                <ellipse
                  cx="578.6"
                  cy="379.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 578.6 379.3)"
                />
                <ellipse
                  cx="594.5"
                  cy="379.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 594.5 379.3)"
                />
                <ellipse
                  cx="540.6"
                  cy="396.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 540.6 396.3)"
                />
                <ellipse
                  cx="556.5"
                  cy="396.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 556.5 396.3)"
                />
                <ellipse
                  cx="572.4"
                  cy="396.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 572.4 396.3)"
                />
                <ellipse
                  cx="588.3"
                  cy="396.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 588.4 396.3)"
                />
                <ellipse
                  cx="534.4"
                  cy="413.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 534.4 413.3)"
                />
                <ellipse
                  cx="550.3"
                  cy="413.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 550.3 413.3)"
                />
                <ellipse
                  cx="566.2"
                  cy="413.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 566.2 413.3)"
                />
                <ellipse
                  cx="582.1"
                  cy="413.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 582.2 413.3)"
                />
                <ellipse
                  cx="528.2"
                  cy="430.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 528.2 430.3)"
                />
                <ellipse
                  cx="544.1"
                  cy="430.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 544.1 430.3)"
                />
                <ellipse
                  cx="560"
                  cy="430.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.6 562.3 429.7)"
                />
                <ellipse
                  cx="575.9"
                  cy="430.3"
                  rx="6.5"
                  ry="4.6"
                  transform="rotate(-39.8 576 430.3)"
                />
              </g>
            </g>
          </g>
          <g id="left-display">
            <g
              id="left-display-display"
              class="takt-draw-svg-left-display"
            >
              <path
                fill="#282e39"
                stroke="#0ff"
                stroke-miterlimit="10"
                stroke-width="5"
                d="M299 461H153c-10.4 0-22-8.5-25.8-19L101 370c-3.8-10.4 1.6-18.9 12-18.9h146c10.5 0 22 8.5 25.9 18.9l26.2 72c3.8 10.4-1.6 19-12 19z"
                opacity=".8"
              />
              <polygon
                fill="#0ff"
                points="153.1 433.3 155.7 440.3 158.2 447.3 153.6 443.8 148.9 440.3 151 436.8 153.1 433.3"
              />
              <polygon
                fill="#0ff"
                points="143 433.3 146.4 433.3 151.9 448.4 148.5 448.4 143 433.3"
              />
              <polygon
                fill="#0ff"
                points="193.8 448.4 191.3 441.4 188.7 434.4 193.4 437.9 198 441.4 195.9 444.9 193.8 448.4"
              />
              <polygon
                fill="#0ff"
                points="203.9 448.4 200.6 448.4 195.1 433.3 198.4 433.3 203.9 448.4"
              />
              <polygon
                fill="#0ff"
                points="164.4 433.3 167.8 433.3 173.3 448.4 169.9 448.4 164.4 433.3"
              />
              <polygon
                fill="#0ff"
                points="174 433.3 177.4 433.3 182.9 448.4 179.5 448.4 174 433.3"
              />
              <ellipse
                cx="199"
                cy="377.7"
                fill="#34496a"
                rx="5.4"
                ry="7.7"
                transform="rotate(-50.2 199 377.7)"
              />
              <polygon
                fill="#0ff"
                points="198.2 380.9 197 377.7 195.9 374.6 199.2 376.1 202.6 377.7 200.4 379.3 198.2 380.9"
              />
              <line
                class="takt-draw-svg-eq-line"
                x1="217.3"
                x2="267.5"
                y1="377.7"
                y2="377.7"
                fill="#282e39"
                stroke="#0ff"
                stroke-linecap="round"
                stroke-miterlimit="10"
                stroke-width="5"
                opacity=".8"
              />
              <ellipse
                cx="206.2"
                cy="397.6"
                fill="#34496a"
                rx="5.4"
                ry="7.7"
                transform="rotate(-50.2 206.2 397.6)"
              />
              <polygon
                fill="#0ff"
                points="205.4 400.7 204.2 397.6 203.1 394.4 206.4 396 209.8 397.6 207.6 399.2 205.4 400.7"
              />
              <line
                class="takt-draw-svg-eq-line"
                x1="224.5"
                x2="274.8"
                y1="397.6"
                y2="397.6"
                fill="#282e39"
                stroke="#0ff"
                stroke-linecap="round"
                stroke-miterlimit="10"
                stroke-width="5"
                opacity=".8"
              />
              <ellipse
                cx="213.5"
                cy="417.5"
                fill="#34496a"
                rx="5.4"
                ry="7.7"
                transform="rotate(-50.2 213.4 417.4)"
              />
              <polygon
                fill="#0ff"
                points="212.6 420.6 211.5 417.5 210.3 414.3 213.7 415.9 217 417.5 214.8 419 212.6 420.6"
              />
              <line
                class="takt-draw-svg-eq-line"
                x1="231.8"
                x2="282"
                y1="417.5"
                y2="417.5"
                fill="#282e39"
                stroke="#0ff"
                stroke-linecap="round"
                stroke-miterlimit="10"
                stroke-width="5"
                opacity=".8"
              />
              <ellipse
                cx="220.7"
                cy="437.3"
                fill="#34496a"
                rx="5.4"
                ry="7.7"
                transform="rotate(-50.2 220.7 437.3)"
              />
              <polygon
                fill="#0ff"
                points="219.8 440.5 218.7 437.3 217.6 434.2 220.9 435.8 224.3 437.3 222.1 438.9 219.8 440.5"
              />
              <line
                class="takt-draw-svg-eq-line"
                x1="239"
                x2="289.2"
                y1="437.3"
                y2="437.3"
                fill="#282e39"
                stroke="#0ff"
                stroke-linecap="round"
                stroke-miterlimit="10"
                stroke-width="5"
                opacity=".8"
              />
              <path
                fill="#34496a"
                d="M190.5 424.4h-46a7.4 7.4 0 01-6.5-4.7l-15.8-43.5c-1-2.6.4-4.7 3-4.7h46a7.4 7.4 0 016.5 4.7l15.8 43.5c1 2.6-.4 4.7-3 4.7z"
              />
              <ellipse
                cx="157.8"
                cy="398"
                fill="#282e39"
                rx="17.5"
                ry="25.1"
                transform="rotate(-50.2 157.8 398)"
              />
              <ellipse
                cx="157.8"
                cy="398"
                fill="#282e39"
                rx="5.1"
                ry="7.3"
                transform="rotate(-50.2 157.8 398)"
              />
              <path
                fill="#0ff"
                d="M159.8 405a10 10 0 01-8.8-6.4 5.8 5.8 0 01.5-5.4 5.3 5.3 0 014.4-2.2 10 10 0 018.8 6.4 5.8 5.8 0 01-.5 5.4 5.3 5.3 0 01-4.4 2.1zm-3.9-10.6a2 2 0 00-1.6.7 2.5 2.5 0 000 2.3 6.6 6.6 0 005.4 4 2 2 0 001.7-.6 2.5 2.5 0 000-2.3 6.6 6.6 0 00-5.5-4zM173.6 405h14.5l-5.1-14h-14.4a1.8 1.8 0 00-1.7 2.6l3.2 8.7a4.1 4.1 0 003.5 2.6z"
              />
            </g>
          </g>
        </svg>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 登录展示插画：左右面板浮动 + 均衡器 / 音量条动画（无文字）
 */
import { gsap } from '@/bootstrap/taktGsap';
import { useGsap } from '@/composables/useGsap';

/** 作用域 */
const scopeRef = ref<HTMLElement | null>(null);

/**
 * 左右面板浮动与指示灯动画
 */
useGsap(() => {
  const root = scopeRef.value;
  if (!root) {
    return;
  }

  const leftDisplay = root.querySelector('.takt-draw-svg-left-display');
  const rightDisplay = root.querySelector('.takt-draw-svg-right-display');
  const eqLines = root.querySelectorAll('.takt-draw-svg-eq-line');
  const barTops = root.querySelectorAll('.takt-draw-svg-bar-top');

  if (leftDisplay) {
    gsap.to(leftDisplay, {
      y: 10,
      ease: 'sine.inOut',
      repeat: -1,
      yoyo: true,
      duration: 2,
    });
  }
  if (rightDisplay) {
    gsap.to(rightDisplay, {
      y: 10,
      ease: 'sine.inOut',
      repeat: -1,
      yoyo: true,
      duration: 2,
      delay: 0.3,
    });
  }
  if (eqLines.length) {
    gsap.timeline({ repeat: -1 })
      .to(eqLines, {
        stroke: '#34496a',
        stagger: { each: 0.5 },
        duration: 0.01,
      })
      .to(eqLines, {
        stroke: '#0ff',
        stagger: { each: 0.5 },
        duration: 0.01,
      });
  }
  if (barTops.length) {
    gsap.to(barTops, {
      scaleY: 0.35,
      transformOrigin: '50% 100%',
      ease: 'sine.inOut',
      repeat: -1,
      yoyo: true,
      stagger: 0.2,
      duration: 0.8,
    });
  }
}, {
  scope: scopeRef,
});
</script>

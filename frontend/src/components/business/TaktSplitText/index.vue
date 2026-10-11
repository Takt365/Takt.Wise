<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/business/TaktSplitText -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：GSAP SplitText 词下落旋转动画（对齐 CodePen jEEbzJb；可循环） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    ref="scopeRef"
    class="takt-split-text-root"
  >
    <h1
      ref="splitTextRef"
      class="takt-split-text"
    >{{ text }}</h1>
  </div>
</template>

<script setup lang="ts">
/**
 * GSAP SplitText 词动画封装
 * @see https://codepen.io/GreenSock/pen/jEEbzJb
 */
import { gsap, SplitText } from '@/bootstrap/taktGsap';
import { useGsap } from '@/composables/useGsap';

interface Props {
  /**
   * 待拆分文案；用 wordDelimiter 分隔词（默认 ZWJ \\u200D）
   * 例：`Practical\u200DSimple\u200DFlexible`
   */
  text: string;
  /** 词分隔符；默认 ZWJ(\\u200D) */
  wordDelimiter?: string;
  /** 入场结束后是否淡出并循环；默认 true */
  loop?: boolean;
  /** 入场完成后停留秒数（仅 loop）；默认 1.5 */
  holdSeconds?: number;
  /** 淡出时长秒（仅 loop）；默认 0.3 */
  fadeSeconds?: number;
}

const props = withDefaults(defineProps<Props>(), {
  wordDelimiter: '\u200D',
  loop: true,
  holdSeconds: 1.5,
  fadeSeconds: 0.3,
});

/** SplitText / gsap.context 作用域 */
const scopeRef = ref<HTMLElement | null>(null);

/** 被拆分的标题 */
const splitTextRef = ref<HTMLElement | null>(null);

/** 字体就绪后再跑 SplitText（与 Pen document.fonts.ready 一致） */
const fontsReady = ref(typeof document !== 'undefined' && document.fonts.status === 'loaded');

if (!fontsReady.value) {
  void document.fonts.ready.then(() => {
    fontsReady.value = true;
  });
}

/**
 * 对齐 Pen 入场参数；loop 时用 fromTo 重播（淡出后不可再用 from，否则 0→0）
 */
useGsap(() => {
  if (!fontsReady.value) {
    return;
  }
  const el = splitTextRef.value;
  if (!el?.textContent?.trim()) {
    return;
  }

  gsap.set(el, { opacity: 1 });

  const split = SplitText.create(el, {
    type: 'words',
    wordsClass: 'takt-split-text-word',
    wordDelimiter: props.wordDelimiter,
  });

  if (!split.words?.length) {
    return;
  }

  const play = (): void => {
    gsap.fromTo(
      split.words,
      {
        y: -100,
        opacity: 0,
        rotation: () => gsap.utils.random(-80, 80),
      },
      {
        y: 0,
        opacity: 1,
        rotation: 0,
        stagger: 0.1,
        duration: 1,
        ease: 'back',
        onComplete: () => {
          if (!props.loop) {
            return;
          }
          gsap.delayedCall(props.holdSeconds, () => {
            gsap.to(split.words, {
              opacity: 0,
              duration: props.fadeSeconds,
              onComplete: play,
            });
          });
        },
      },
    );
  };

  play();
}, {
  scope: scopeRef,
  dependencies: [() => fontsReady.value, () => props.text, () => props.wordDelimiter, () => props.loop],
});
</script>

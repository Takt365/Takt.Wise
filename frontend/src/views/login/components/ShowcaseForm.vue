<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：ShowcaseForm.vue -->
<!-- 功能描述：登录页展示区（插画 takt-draw-svg + slogan + tagline takt-split-text） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-login-showcase flex min-h-screen w-full flex-col text-center text-white">
    <!-- 插画区：视口高度 2/3 → takt-draw-svg -->
    <div class="takt-login-showcase-demo flex h-[66.67vh] w-full shrink-0 items-center justify-center overflow-hidden px-6">
      <TaktDrawSvg />
    </div>

    <!-- 文案区：视口高度 1/3 → slogan + tagline(takt-split-text) -->
    <div class="takt-login-showcase flex h-[33.33vh] w-full shrink-0 flex-col items-center justify-center gap-2 px-6">
      <p class="text-[48px] font-semibold leading-tight">
        {{ t('common.page.app.slogan') }}
      </p>
      <!-- tagline：字号随 text-[32px] 适配 -->
      <div class="w-full max-w-full text-[32px] leading-snug text-white/75">
        <TaktSplitText
          :key="locale"
          :text="splitText"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 登录页左侧展示区：takt-draw-svg + slogan + takt-split-text（随 locale）
 * @see https://codepen.io/GreenSock/pen/jEEbzJb
 */
import { useI18n } from 'vue-i18n';

/** i18n */
const { t, locale } = useI18n();

/** SplitText 词分隔符（ZWJ） */
const ZWJ = '\u200D';

/**
 * 三词文案：Practical / Simple / Flexible（按当前语言）
 * @returns {string} ZWJ 拼接后的拆分文本
 */
const splitText = computed(() =>
  [
    t('login.page.showcase.split.practical'),
    t('login.page.showcase.split.simple'),
    t('login.page.showcase.split.flexible'),
  ].join(ZWJ),
);
</script>

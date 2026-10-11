<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：LoginBrand.vue -->
<!-- 功能描述：登录相关页左上角 Logo + 应用标题、产品编码与 package.json 版本 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="absolute top-6 left-6 z-10 flex items-center gap-3">
    <img
      v-if="logoVisible && !logoError"
      :src="logoUrl!"
      :alt="t('common.page.app.title')"
      class="size-10 shrink-0 object-contain"
      @error="handleLogoError"
    >
    <div class="flex flex-col leading-tight text-white">
      <span class="text-base font-semibold">{{ t('common.page.app.title') }}</span>
      <span class="text-xs text-white/65">
        {{ t('common.page.app.productcode') }} · V{{ appPackageInfo.version }}
      </span>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 登录页品牌区（Logo、common.page.app.title、common.page.app.productcode、package.json 版本）
 */
import { useI18n } from 'vue-i18n';
import { appInfo, type AppPackageInfo } from '@/utils/appMeta';
import { defaultSetting, useSettingStore } from '@/stores/common/useSettingStore';

/** i18n 翻译函数 */
const { t } = useI18n();

/** package.json 摘要（virtual:app-info 注入） */
const appPackageInfo: AppPackageInfo = appInfo.pkg;

/** 全局偏好（Logo 路径与显隐） */
const { setting } = storeToRefs(useSettingStore());

/** 安全读取的设置 */
const settingSafe = computed(() => setting.value ?? defaultSetting);

/** Logo 加载失败标记 */
const logoError = ref(false);

/** Logo 图片 URL */
const logoUrl = computed(() => {
  const logoPath = settingSafe.value.logo?.trim();
  if (!logoPath) {
    return null;
  }

  if (logoPath.startsWith('@/')) {
    return logoPath.replace('@/', '/src/');
  }

  if (logoPath.startsWith('/')) {
    return logoPath;
  }

  return `/src/${logoPath}`;
});

/** 是否展示 Logo 图 */
const logoVisible = computed(() => settingSafe.value.showLogo !== false && !!logoUrl.value);

/**
 * Logo 加载失败时隐藏图片，保留标题与产品编码
 */
function handleLogoError(): void {
  logoError.value = true;
}
</script>

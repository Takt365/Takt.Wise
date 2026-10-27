<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktThemeToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：浅色 / 深色 / 跟随系统（默认图标收缩，悬停下拉；可选单按钮切换） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-tooltip v-if="type === 'button'" :title="tooltipTitle">
    <a-button type="text" :size="size" :aria-label="tooltipTitle" @click="themeStore.toggleThemeMode()">
      <template #icon>
        <ri-moon-line class="takt-remix-icon" v-if="themeStore.resolvedTheme === 'light'" />
        <ri-sun-line class="takt-remix-icon" v-else />
      </template>
    </a-button>
  </a-tooltip>

  <a-dropdown
    v-else-if="type === 'dropdown' || type === 'icon'"
    :trigger="trigger"
    placement="bottomRight"
    :mouse-enter-delay="0.15"
    v-bind="$attrs"
  >
    <a-button type="text" :size="size" :aria-label="t('common.page.theme.switch.label')">
      <template #icon>
        <ri-moon-line class="takt-remix-icon" v-if="themeStore.resolvedTheme === 'light'" />
        <ri-sun-line class="takt-remix-icon" v-else />
      </template>
    </a-button>

    <template #overlay>
      <a-menu :selecmed-keys="[themeStore.mode]" @click="handleMenuClick">
        <a-menu-item key="light">
          <span class="takt-toggle-menu-row">
            <ri-sun-line class="takt-remix-icon" />
            {{ t('common.page.theme.light') }}
          </span>
        </a-menu-item>
        <a-menu-item key="dark">
          <span class="takt-toggle-menu-row">
            <ri-moon-line class="takt-remix-icon" />
            {{ t('common.page.theme.dark') }}
          </span>
        </a-menu-item>
        <a-menu-item key="system">
          <span class="takt-toggle-menu-row">
            <ri-computer-line class="takt-remix-icon" />
            {{ t('common.page.theme.system') }}
          </span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
/**
 * 主题模式切换（默认悬停下拉；type=button 时为单按钮亮暗切换）
 */
import { useI18n } from 'vue-i18n';
import type { MenuProps } from 'ant-design-vue';
import { RiMoonLine, RiSunLine, RiComputerLine } from '@remixicon/vue';
import { useThemeStore } from '@/stores/common/useThemeStore';
import type { TaktThemeMode } from '@/utils/theme';

/** 展示形态 */
type TaktThemeToggleType = 'dropdown' | 'icon' | 'button';

interface Props {
  /** 展示形态（icon 与 dropdown 相同：图标 + 悬停下拉） */
  type?: TaktThemeToggleType;
  /** 下拉触发方式（type=dropdown / icon） */
  trigger?: ('click' | 'hover' | 'contextmenu')[];
  /** 控件尺寸 */
  size?: 'small' | 'middle' | 'large';
}

withDefaults(defineProps<Props>(), {
  type: 'dropdown',
  trigger: () => ['hover'],
  size: 'middle',
});

const { t } = useI18n();
const themeStore = useThemeStore();

/**
 * 单按钮模式提示文案
 */
const tooltipTitle = computed(() =>
  themeStore.resolvedTheme === 'dark'
    ? t('common.page.theme.switch.to.light')
    : t('common.page.theme.switch.to.dark')
);

/**
 * 下拉菜单选中主题模式
 */
const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
  if (key === 'light' || key === 'dark' || key === 'system') {
    themeStore.setThemeMode(key as TaktThemeMode);
  }
};
</script>

<style scoped>
.takt-toggle-menu-row {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  line-height: 1;
}

:deep(svg) {
  color: var(--ant-color-text);
  fill: currentColor;
}
</style>

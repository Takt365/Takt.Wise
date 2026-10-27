<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktColorToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：主题色预设切换（按钮 / 图标展开 / 下拉 / 单选） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-button
    v-if="type === 'button'"
    :type="buttonType"
    :size="size"
    v-bind="$attrs"
    @click="themeColorStore.toggleColorPreset()"
  >
    <template #icon>
      <ri-palette-line class="mr-2 takt-remix-icon" />
    </template>
    <slot>{{ t('common.page.color.switch') }}</slot>
  </a-button>

  <div
    v-else-if="type === 'icon'"
    class="color-toggle-group"
    :class="{ expanded: isExpanded }"
    @mouseenter="onGroupEnter"
    @mouseleave="onGroupLeave"
  >
    <div class="color-circles-container" :class="{ expanded: isExpanded }">
      <a-tooltip
        v-for="presetKey in themeColorPresetKeys"
        :key="presetKey"
        :title="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)"
        placement="top"
      >
        <button
          type="button"
          class="color-circle-button"
          :class="{ active: themeColorStore.preset === presetKey }"
          :style="{ backgroundColor: themeColorMap[presetKey] }"
          :aria-label="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)"
          @click.stop="selectPreset(presetKey)"
        >
          <ri-check-line v-if="themeColorStore.preset === presetKey" class="text-white takt-remix-icon" />
        </button>
      </a-tooltip>
    </div>

    <a-button type="text" :size="size" class="color-palette-button" v-bind="$attrs">
      <template #icon>
        <ri-palette-line class="takt-remix-icon" :style="{ color: themeColorStore.colorPrimary }" />
      </template>
    </a-button>
  </div>

  <a-dropdown v-else-if="type === 'dropdown'" :trigger="['click']" v-bind="$attrs">
    <a-button type="text" :size="size">
      <template #icon>
        <ri-palette-line class="mr-2 takt-remix-icon" />
      </template>
      <slot>{{ t('common.page.color.title') }}</slot>
    </a-button>

    <template #overlay>
      <a-menu :selecmed-keys="[themeColorStore.preset]" @click="handleMenuClick">
        <a-menu-item v-for="presetKey in themeColorPresetKeys" :key="presetKey">
          <span class="color-item">
            <span class="color-dot" :style="{ backgroundColor: themeColorMap[presetKey] }" />
            {{ t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`) }}
          </span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>

  <a-radio-group
    v-else-if="type === 'radio' || type === 'radio-icon'"
    :value="themeColorStore.preset"
    :size="radioSize"
    v-bind="$attrs"
    @change="handleRadioChange"
  >
    <a-radio-button v-for="presetKey in themeColorPresetKeys" :key="presetKey" :value="presetKey">
      <a-tooltip :title="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)">
        <span
          v-if="type === 'radio-icon'"
          class="takt-toggle-radio-item color-dot-only"
          :style="{ backgroundColor: themeColorMap[presetKey] }"
          :aria-label="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)"
        />
        <span v-else class="color-item">
          <span class="color-dot" :style="{ backgroundColor: themeColorMap[presetKey] }" />
          {{ t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`) }}
        </span>
      </a-tooltip>
    </a-radio-button>
  </a-radio-group>

  <template v-else-if="type === 'radio-item'">
    <a-radio-button v-for="presetKey in themeColorPresetKeys" :key="presetKey" :value="presetKey">
      <a-tooltip :title="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)">
        <span
          v-if="radioVariant === 'icon'"
          class="takt-toggle-radio-item color-dot-only"
          :style="{ backgroundColor: themeColorMap[presetKey] }"
          :aria-label="t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`)"
        />
        <span v-else class="color-item">
          <span class="color-dot" :style="{ backgroundColor: themeColorMap[presetKey] }" />
          {{ t(`common.page.color.${themeColorPresetI18nKeyMap[presetKey]}`) }}
        </span>
      </a-tooltip>
    </a-radio-button>
  </template>
</template>

<script setup lang="ts">
/**
 * 主题色预设切换
 */
import { useI18n } from 'vue-i18n';
import type { MenuProps } from 'ant-design-vue';
import type { RadioChangeEvent } from 'ant-design-vue/es/radio';
import { RiPaletteLine, RiCheckLine } from '@remixicon/vue';
import { useThemeColorStore } from '@/stores/common/useThemeColorStore';
import { themeColorMap, themeColorPresetKeys, themeColorPresetI18nKeyMap, type TaktThemeColorPreset } from '@/utils/theme';

/** 组件展示形态 */
type TaktColorToggleType = 'button' | 'icon' | 'dropdown' | 'radio' | 'radio-icon' | 'radio-item';

/** radio / radio-item 子项展示 */
type TaktColorRadioVariant = 'icon' | 'text';

interface Props {
  /** 展示形态 */
  type?: TaktColorToggleType;
  /** 单选项展示（type=radio-icon / radio-item 且 radioVariant=icon） */
  radioVariant?: TaktColorRadioVariant;
  /** 按钮 type（仅 type=button 时生效） */
  buttonType?: 'default' | 'primary' | 'dashed' | 'link' | 'text';
  /** 控件尺寸 */
  size?: 'small' | 'middle' | 'large';
}

const props = withDefaults(defineProps<Props>(), {
  type: 'icon',
  radioVariant: 'text',
  buttonType: 'default',
  size: 'middle',
});

const { t } = useI18n();
const themeColorStore = useThemeColorStore();

const isExpanded = ref(false);
const leaveTimer = ref<ReturnType<typeof setTimeout> | null>(null);

const COLLAPSE_DELAY_MS = 200;

/**
 * Radio 组尺寸映射
 */
const radioSize = computed<'default' | 'small' | 'large'>(() =>
  props.size === 'middle' ? 'default' : props.size
);

/**
 * 鼠标移入展开色板
 */
function onGroupEnter(): void {
  if (leaveTimer.value != null) {
    clearTimeout(leaveTimer.value);
    leaveTimer.value = null;
  }

  isExpanded.value = true;
}

/**
 * 鼠标移出延迟收起
 */
function onGroupLeave(): void {
  if (leaveTimer.value != null) {
    clearTimeout(leaveTimer.value);
  }

  leaveTimer.value = setTimeout(() => {
    isExpanded.value = false;
    leaveTimer.value = null;
  }, COLLAPSE_DELAY_MS);
}

onUnmounted(() => {
  if (leaveTimer.value != null) {
    clearTimeout(leaveTimer.value);
  }
});

/**
 * 选中预设色
 * @param preset 预设键名
 */
function selectPreset(preset: TaktThemeColorPreset): void {
  themeColorStore.setColorPreset(preset);

  if (leaveTimer.value != null) {
    clearTimeout(leaveTimer.value);
    leaveTimer.value = null;
  }

  isExpanded.value = false;
}

/**
 * 下拉菜单选中
 */
const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
  if (typeof key === 'string' && key in themeColorMap) {
    themeColorStore.setColorPreset(key as TaktThemeColorPreset);
  }
};

/**
 * 单选组变更
 * @param event Radio 变更事件
 */
function handleRadioChange(event: RadioChangeEvent): void {
  const value = event.target?.value;

  if (typeof value === 'string' && value in themeColorMap) {
    themeColorStore.setColorPreset(value as TaktThemeColorPreset);
  }
}
</script>

<style scoped>
.color-item {
  display: inline-flex;
  align-items: center;
  gap: 8px;
}

.color-dot {
  display: inline-block;
  width: 12px;
  height: 12px;
  border-radius: 50%;
}

.color-toggle-group {
  position: relative;
  display: inline-flex;
  align-items: center;
}

.color-toggle-group.expanded::before {
  content: '';
  position: absolute;
  right: 100%;
  top: 0;
  bottom: 0;
  width: 360px;
  margin-right: 8px;
  z-index: 0;
}

.color-circles-container {
  position: absolute;
  right: 100%;
  top: 50%;
  transform: translateY(-50%);
  display: inline-flex;
  align-items: center;
  gap: 8px;
  margin-right: 8px;
  z-index: 10;
  max-width: 0;
  opacity: 0;
  overflow: hidden;
  pointer-events: none;
  transition:
    max-width 0.25s ease-out,
    opacity 0.2s ease-out;
}

.color-circles-container.expanded {
  max-width: 360px;
  opacity: 1;
  pointer-events: auto;
}

.color-circle-button {
  width: 24px;
  height: 24px;
  border: none;
  border-radius: 50%;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  cursor: pointer;
  padding: 0;
  transition: opacity 0.15s ease-out;
}

.color-circles-container:not(.expanded) .color-circle-button {
  opacity: 0;
}

.color-palette-button {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.takt-toggle-radio-item {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  line-height: 1;
}

.color-dot-only {
  width: 14px;
  height: 14px;
  border-radius: 50%;
}

:deep(svg) {
  color: var(--ant-color-text);
  fill: currentColor;
}
</style>

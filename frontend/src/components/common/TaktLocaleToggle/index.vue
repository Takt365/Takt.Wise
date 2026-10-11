<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktLocaleToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：语言切换（GetCultureOptionsAsync / TaktSelectOption；默认图标收缩，悬停下拉） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-dropdown
    v-if="type === 'dropdown' || type === 'icon'"
    :trigger="trigger"
    placement="bottomRight"
    :mouse-enter-delay="0.15"
    v-bind="$attrs"
  >
    <a-button type="text" :size="size" :aria-label="t('common.page.locale.switch')" :loading="localeStore.loading">
      <template #icon>
        <ri-global-line class="takt-remix-icon" />
      </template>
    </a-button>

    <template #overlay>
      <a-menu :selected-keys="[localeStore.currentLocale]" @click="handleMenuClick">
        <a-menu-item
          v-for="option in localeStore.cultureOptions"
          :key="resolveCultureCode(option)"
        >
          <span class="takt-toggle-menu-row">
            <span
              v-if="isCultureEmojiIcon(resolveCultureIcon(option))"
              class="text-base leading-none"
            >
              {{ resolveCultureIcon(option) }}
            </span>
            <span
              v-else-if="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
              :class="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
              aria-hidden="true"
            />
            <span>{{ option.dictLabel }}</span>
          </span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>

  <a-radio-group
    v-else-if="type === 'radio'"
    :value="localeStore.currentLocale"
    :size="radioSize"
    :disabled="localeStore.loading || localeStore.cultureOptions.length === 0"
    v-bind="$attrs"
    @change="handleRadioChange"
  >
    <a-radio-button
      v-for="option in localeStore.cultureOptions"
      :key="resolveCultureCode(option)"
      :value="resolveCultureCode(option)"
    >
      <a-tooltip :title="option.dictLabel">
        <span
          class="takt-toggle-radio-item"
          :aria-label="option.dictLabel"
        >
          <span
            v-if="isCultureEmojiIcon(resolveCultureIcon(option))"
            class="text-base leading-none"
          >
            {{ resolveCultureIcon(option) }}
          </span>
          <span
            v-else-if="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
            :class="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
            aria-hidden="true"
          />
          <ri-global-line class="takt-remix-icon" v-else />
        </span>
      </a-tooltip>
    </a-radio-button>
  </a-radio-group>

  <template v-else-if="type === 'radio-item'">
    <a-radio-button
      v-for="option in localeStore.cultureOptions"
      :key="resolveCultureCode(option)"
      :value="resolveCultureCode(option)"
    >
      <a-tooltip :title="option.dictLabel">
        <span
          class="takt-toggle-radio-item"
          :aria-label="option.dictLabel"
        >
          <span
            v-if="isCultureEmojiIcon(resolveCultureIcon(option))"
            class="text-base leading-none"
          >
            {{ resolveCultureIcon(option) }}
          </span>
          <span
            v-else-if="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
            :class="resolveCultureFlagClass(resolveCultureCode(option), resolveCultureIcon(option))"
            aria-hidden="true"
          />
          <ri-global-line class="takt-remix-icon" v-else />
        </span>
      </a-tooltip>
    </a-radio-button>
  </template>
</template>

<script setup lang="ts">
/**
 * 语言切换（已登录 GetCultureOptions；登录页 SessionCultureOptions，结构为 TaktSelectOption）
 */
import { useI18n } from 'vue-i18n';
import type { MenuProps } from 'ant-design-vue';
import type { RadioChangeEvent } from 'ant-design-vue/es/radio';
import { RiGlobalLine } from '@remixicon/vue';
import {
  resolveCultureCode,
  resolveCultureIcon,
  useLocaleStore,
} from '@/stores/foundation/useLocaleStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import { isCultureEmojiIcon, resolveCultureFlagClass } from '@/utils/taktLocaleFlag';

/** 展示形态 */
type TaktLocaleToggleType = 'dropdown' | 'icon' | 'radio' | 'radio-item';

interface Props {
  /** 展示形态 */
  type?: TaktLocaleToggleType;
  /** 下拉触发方式（type=dropdown / icon） */
  trigger?: ('click' | 'hover' | 'contextmenu')[];
  /** 控件尺寸 */
  size?: 'small' | 'middle' | 'large';
}

const props = withDefaults(defineProps<Props>(), {
  type: 'dropdown',
  trigger: () => ['hover'],
  size: 'middle',
});

const { t } = useI18n();
const localeStore = useLocaleStore();
const userStore = useUserStore();
const tenantStore = useTenantStore();

/**
 * Radio 组尺寸映射
 */
const radioSize = computed<'default' | 'small' | 'large'>(() =>
  props.size === 'middle' ? 'default' : props.size
);

/**
 * 下拉菜单选中语言
 */
const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
  if (typeof key === 'string') {
    localeStore.setLocale(key);
  }
};

/**
 * 单选组变更
 * @param event Radio 变更事件
 */
function handleRadioChange(event: RadioChangeEvent): void {
  const value = event.target?.value;

  if (typeof value === 'string') {
    localeStore.setLocale(value);
  }
}

onMounted(() => {
  if (userStore.isLoggedIn || tenantStore.tenantCode?.trim()) {
    void localeStore.loadCultureOptionsAsync().catch(() => undefined);
  }
});
</script>

<style scoped>
.takt-toggle-menu-row,
.takt-toggle-radio-item {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  line-height: 1;
  min-width: 1.25rem;
}

:deep(svg) {
  color: var(--ant-color-text);
  fill: currentColor;
}
</style>

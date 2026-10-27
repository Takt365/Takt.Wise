<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktCompanyToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：公司切换（已登录；select 下拉 / icon 仅图标悬停下拉） -->
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
    <a-button
      type="text"
      :size="size"
      :aria-label="companyAriaLabel"
      :loading="tenantStore.companyLoading"
      :disabled="isSelectDisabled"
    >
      <template #icon>
        <ri-community-line class="takt-remix-icon" />
      </template>
    </a-button>

    <template #overlay>
      <a-menu
        :selecmed-keys="[tenantStore.companyCode]"
        @click="handleMenuClick"
      >
        <a-menu-item
          v-for="option in tenantStore.companyOptions"
          :key="resolveSelectOptionValue(option)"
        >
          <span class="takt-toggle-menu-row">{{ option.dictLabel }}</span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>

  <TaktSelect
    v-else
    :model-value="tenantStore.companyCode"
    :options="tenantStore.companyOptions"
    :loading="tenantStore.companyLoading"
    :placeholder="placeholder ?? t('common.page.company.switch')"
    :disabled="isSelectDisabled"
    :size="size"
    :allow-clear="false"
    :show-search="showSearch"
    class="takt-company-toggle__select"
    @update:model-value="handleValueChange"
  />
</template>

<script setup lang="ts">
/**
 * 公司切换（TaktSelect 或图标 + 下拉菜单）
 */
import { useI18n } from 'vue-i18n';
import { RiCommunityLine } from '@remixicon/vue';
import type { MenuProps } from 'ant-design-vue';
import type { SelectValue } from 'ant-design-vue/es/select';
import { useUserStore } from '@/stores/identity/useUserStore';
import { resolveSelectOptionValue, useTenantStore } from '@/stores/identity/useTenantStore';

/** 展示形态 */
type TaktCompanyToggleType = 'select' | 'dropdown' | 'icon';

interface Props {
  /** 展示形态（icon 与 dropdown 相同：图标 + 悬停下拉） */
  type?: TaktCompanyToggleType;
  /** 下拉触发方式（type=dropdown / icon） */
  trigger?: ('click' | 'hover' | 'contextmenu')[];
  /** 尺寸（透传控件） */
  size?: 'small' | 'middle' | 'large';
  /** 占位符（type=select） */
  placeholder?: string;
  /** 是否可搜索（type=select） */
  showSearch?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  type: 'select',
  trigger: () => ['hover'],
  size: 'middle',
  showSearch: true,
});

const { t } = useI18n();
const userStore = useUserStore();
const tenantStore = useTenantStore();

/** 是否禁用选择 */
const isSelectDisabled = computed(
  () => !tenantStore.tenantCode || tenantStore.companyOptions.length === 0,
);

/** 图标按钮无障碍标签 */
const companyAriaLabel = computed(() => {
  const label = tenantStore.currentCompanyOption?.dictLabel;
  return label ? `${t('common.page.company.switch')}: ${label}` : t('common.page.company.switch');
});

/**
 * 下拉菜单选中公司
 * @param param 菜单点击参数
 */
const handleMenuClick: MenuProps['onClick'] = ({ key }) => {
  if (typeof key === 'string' && key) {
    tenantStore.setCompany(key, { rememberSession: true });
  }
};

/**
 * 下拉选择变更（type=select）
 * @param value 选中值
 */
function handleValueChange(value: SelectValue): void {
  if (typeof value === 'string' && value) {
    tenantStore.setCompany(value, { rememberSession: true });
  }
}

watch(
  [() => userStore.isLoggedIn, () => tenantStore.tenantCode, () => userStore.profileLoaded],
  ([loggedIn, tenant, profileLoaded]) => {
    if (loggedIn && tenant && profileLoaded) {
      if (tenantStore.companyOptions.length === 0) {
        void tenantStore.loadCompanyOptionsAsync().catch(() => undefined);
      }
      return;
    }

    tenantStore.clearCompanyOptions();
  },
  { immediate: true },
);
</script>

<style scoped>
.takt-company-toggle__select {
  min-width: 10rem;
}

.takt-toggle-menu-row {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  line-height: 1;
}
</style>

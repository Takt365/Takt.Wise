<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/components/common/TaktTenantToggle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：租户选择（登录前 takt-select）/ 已登录只读展示 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <span
    v-if="showReadonly"
    class="takt-tenant-toggle__readonly"
    :title="tenantDisplayLabel"
    :aria-label="tenantDisplayLabel"
  >
    <ri-building-line class="takt-remix-icon" />
    <span>{{ tenantDisplayLabel }}</span>
  </span>

  <TaktSelect
    v-else
    :model-value="tenantStore.tenantCode"
    :options="tenantStore.tenantOptions"
    :loading="tenantStore.tenantLoading"
    :placeholder="placeholder ?? t('common.page.form.placeholder.required', { field: t('common.page.entity.tenantcode') })"
    :disabled="isSelectDisabled"
    :size="size"
    :allow-clear="false"
    :show-search="showSearch"
    class="takt-tenant-toggle__select"
    @update:model-value="handleValueChange"
  />
</template>

<script setup lang="ts">
/**
 * 租户：未登录/登录页用 TaktSelect；已登录仅展示当前租户（禁止在线切换租户）
 */
import { useI18n } from 'vue-i18n';
import { message } from 'ant-design-vue';
import type { SelectValue } from 'ant-design-vue/es/select';
import { RiBuildingLine } from '@remixicon/vue';
import { resolveHttpErrorMessage } from '@/utils/taktHttpErrorMessage';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';

interface Props {
  /** 尺寸（透传 takt-select） */
  size?: 'small' | 'middle' | 'large';
  /** 占位符 */
  placeholder?: string;
  /** 是否可搜索 */
  showSearch?: boolean;
  /** 登录页：始终下拉可选，不因残留 Token 变为只读 */
  loginMode?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  size: 'middle',
  showSearch: true,
  loginMode: false,
});

const { t } = useI18n();
const userStore = useUserStore();
const tenantStore = useTenantStore();

/** 已登录且非登录页 → 只读展示 */
const showReadonly = computed(() => userStore.isLoggedIn && !props.loginMode);

const isSelectDisabled = computed(() => tenantStore.tenantOptions.length === 0);

/** 已登录时展示的租户文案 */
const tenantDisplayLabel = computed(() => {
  const option = tenantStore.currentTenantOption;
  if (option?.dictLabel) {
    return option.dictLabel;
  }
  return tenantStore.tenantCode || '—';
});

/**
 * 租户选择变更
 * @param value 选中值
 */
function handleValueChange(value: SelectValue): void {
  if (typeof value === 'string' && value) {
    tenantStore.setTenant(value);
  }
}

/**
 * 加载租户下拉；失败时提示（须用户手动选择租户）
 * @param loginMode 是否登录页模式
 */
async function loadTenantOptionsWithNotifyAsync(loginMode: boolean): Promise<void> {
  try {
    await tenantStore.loadTenantOptionsAsync(loginMode);
  } catch (error) {
    message.error(resolveHttpErrorMessage(error) || t('login.page.message.tenant.options.fail'));
  }
}

watch(
  () => [userStore.isLoggedIn, props.loginMode] as const,
  ([loggedIn, loginMode]) => {
    if (loginMode) {
      void loadTenantOptionsWithNotifyAsync(true);
      return;
    }
    if (loggedIn) {
      void loadTenantOptionsWithNotifyAsync(false);
    }
  },
  { immediate: true },
);
</script>

<style scoped>
.takt-tenant-toggle__readonly {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  max-width: 12rem;
  overflow: hidden;
  color: var(--ant-color-text-secondary);
  font-size: 14px;
  text-overflow: ellipsis;
  white-space: nowrap;
  line-height: 1;
}

.takt-tenant-toggle__select {
  min-width: 10rem;
}

:deep(svg) {
  color: var(--ant-color-text);
  fill: currentColor;
}
</style>

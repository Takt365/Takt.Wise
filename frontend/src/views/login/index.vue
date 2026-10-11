<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：登录页壳（品牌、展示区、布局切换、视图路由） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <RegisterForm v-if="viewMode === 'register'" embedded @back="setViewModeLogin" />
  <ForgotForm v-else-if="viewMode === 'forget'" embedded @back="setViewModeLogin" />
  <div
    v-else
    class="relative min-h-screen w-full bg-gradient-to-br from-[#667eea] to-[#764ba2]"
    style="background-color: #667eea"
  >
    <LoginBrand />

    <div class="takt-login-toolbar absolute top-4 right-4 z-10">
      <a-button-group size="small">
        <a-radio-button value="color">
          <TaktColorToggle type="icon" size="small" />
        </a-radio-button>
        <a-radio-button value="layout">
          <TaktLayoutToggle v-model:position="layoutPosition" size="small" />
        </a-radio-button>
        <a-radio-button value="theme">
          <TaktThemeToggle type="icon" size="small" />
        </a-radio-button>
      </a-button-group>
    </div>

    <!-- 分栏：表单 1/3 与展示区 2/3 相斥排列（左表单则右展示，右表单则左展示） -->
    <div
      class="relative z-[1] flex min-h-screen w-full"
      :class="loginPageSplitClass"
    >
      <LoginForm
        :layout-position="layoutPosition"
        @register="openRegisterView"
        @forgot="openForgotView"
      />
      <aside
        v-if="layoutPosition !== 'center'"
        class="takt-login-showcase hidden h-auto min-h-screen w-full shrink-0 items-center justify-center px-6 lg:flex lg:w-2/3 xl:px-12"
      >
        <ShowcaseForm class="takt-login-showcase h-full w-full" />
      </aside>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 登录页壳：视图切换、布局与展示区；主表单见 `LoginForm.vue`
 */
import { message } from 'ant-design-vue';
import { TAKT_LOGOUT_FLASH_STORAGE_KEY } from '@/bootstrap/taktEventHandlers';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import {
  readStoredLoginLayoutPosition,
  type TaktLoginLayoutPosition,
} from '@/utils/taktLoginLayoutDom';
import RegisterForm from '@/views/login/components/RegisterForm.vue';
import ForgotForm from '@/views/login/components/ForgotForm.vue';
import LoginBrand from '@/views/login/components/LoginBrand.vue';
import ShowcaseForm from '@/views/login/components/ShowcaseForm.vue';
import LoginForm from '@/views/login/components/LoginForm.vue';

/** 登录页视图：登录表单 / 注册 / 忘记密码 */
type TaktLoginViewMode = 'login' | 'register' | 'forget';

/** 当前登录壳视图 */
const viewMode = ref<TaktLoginViewMode>('login');

/**
 * 切回登录主表单
 */
function setViewModeLogin(): void {
  viewMode.value = 'login';
}

/**
 * 打开注册视图
 */
function openRegisterView(): void {
  viewMode.value = 'register';
}

/**
 * 打开忘记密码视图
 */
function openForgotView(): void {
  viewMode.value = 'forget';
}

/** 租户 Pinia */
const tenantStore = useTenantStore();

/** 用户 Pinia */
const userStore = useUserStore();

/** 登录卡片在视口中的水平对齐 */
const layoutPosition = ref<TaktLoginLayoutPosition>(readStoredLoginLayoutPosition('center'));

/**
 * 登录页分栏方向：左对齐时表单在左、展示在右；右对齐时相反
 */
const loginPageSplitClass = computed(() => {
  if (layoutPosition.value === 'center') {
    return 'justify-center';
  }
  if (layoutPosition.value === 'right') {
    return 'flex-row-reverse';
  }
  return 'flex-row';
});

onBeforeMount(() => {
  if (userStore.isLoggedIn) {
    userStore.logout();
  }

  tenantStore.clearTenant();
  useLocaleStore().resetLocaleForLoginPage();
});

onMounted(() => {
  if (typeof sessionStorage === 'undefined') {
    return;
  }
  const raw = sessionStorage.getItem(TAKT_LOGOUT_FLASH_STORAGE_KEY);
  if (!raw) {
    return;
  }
  sessionStorage.removeItem(TAKT_LOGOUT_FLASH_STORAGE_KEY);
  try {
    const parsed = JSON.parse(raw) as { type?: string; message?: string };
    const content = parsed.message?.trim();
    if (!content) {
      return;
    }
    const type = parsed.type === 'warning' ? 'warning' : 'error';
    message[type](content);
  } catch {
    // 非法 flash 数据忽略
  }
});

</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login -->
<!-- 文件名称：Callback.vue -->
<!-- 功能描述：OAuth2 Authorization Code + PKCE 回调页，用 code 换 access_token -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="callback-container">
    <a-spin :tip="tip" size="large" />
  </div>
</template>

<script setup lang="ts">
/**
 * OAuth 授权回调页
 * @description 校验 state/PKCE → 换 token → 加载用户资料 → 跳转业务页；vue API 由 auto-import 注入
 */
import { useI18n } from 'vue-i18n';
import { message } from 'ant-design-vue';
import { exchangeAuthorizationCode } from '@/api/identity/oauth.api';
import { consumeOAuthState, consumePkceVerifier } from '@/utils/oauth';
import { useUserStore } from '@/stores/identity/useUserStore';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useMenuStore } from '@/stores/identity/useMenuStore';
import { useTranslationStore } from '@/stores/foundation/useTranslationStore';
import i18n from '@/locales';
import { EventBus } from '@/utils/eventBus';
import { ensureMenuAndRoutesLoaded } from '@/router';
import { resolveDefaultMenuPath } from '@/router/menuRoutes';

/** i18n 翻译函数 */
const { t } = useI18n();

/** 当前路由（读取 code、state、error 等查询参数） */
const route = useRoute();

/** 编程式导航（失败回登录、成功进 dashboard） */
const router = useRouter();

/** 用户 Pinia（写入 token、加载 profile） */
const userStore = useUserStore();
const tenantStore = useTenantStore();
const menuStore = useMenuStore();

/** 加载中提示文案 */
const tip = computed(() => t('login.page.callback.processing'));

/**
 * 挂载后处理 OAuth 回调：错误码 → state/PKCE 校验 → 换 token → 拉用户 → 跳转
 */
onMounted(async () => {
  const error = route.query.error as string | undefined;
  if (error) {
    const errorDescription = (route.query.error_description as string) || error;
    message.error(errorDescription);
    userStore.logout();
    await router.replace('/login');
    return;
  }

  const code = route.query.code as string | undefined;
  const state = route.query.state as string | undefined;
  const savedState = consumeOAuthState();

  if (!code) {
    message.error(t('login.page.callback.missing.code'));
    await router.replace('/login');
    return;
  }

  if (!state || !savedState || state !== savedState) {
    message.error(t('login.page.callback.state.mismatch'));
    userStore.logout();
    await router.replace('/login');
    return;
  }

  const verifier = consumePkceVerifier();
  if (!verifier) {
    message.error(t('login.page.callback.pkce.missing'));
    userStore.logout();
    await router.replace('/login');
    return;
  }

  try {
    userStore.logout();

    const token = await exchangeAuthorizationCode(code, verifier);
    userStore.setOAuthTokens({
      accessToken: token.access_token,
      refreshToken: token.refresh_token,
      expiresIn: token.expires_in,
    });

    tenantStore.restoreTenantCodeFromStorage();
    tenantStore.resetCompanySelection();
    await userStore.loadUserProfile(true);

    await useTranslationStore().loadTranslationMessagesAsync(String(i18n.global.locale.value));
    menuStore.syncMenusFromUserProfile();
    await ensureMenuAndRoutesLoaded();

    EventBus.emit('user:login', {
      userId: userStore.userId,
      userName: userStore.userName,
    });

    const saved = sessionStorage.getItem('takt.oauth.return_after_login');
    sessionStorage.removeItem('takt.oauth.return_after_login');
    const defaultPath = resolveDefaultMenuPath(userStore.menus) ?? '/dashboard/workspace';
    const redirect = saved || (route.query.redirect as string) || defaultPath;
    await router.replace(redirect);
  } catch (e) {
    userStore.logout();
    message.error(e instanceof Error ? e.message : t('login.page.callback.fail'));
    await router.replace('/login');
  }
});
</script>

<style scoped>
.callback-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
}
</style>

<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：LoginForm.vue -->
<!-- 功能描述：登录主表单（租户/用户/密码、验证码弹窗、OAuth 登录流程） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="relative z-[1] flex min-h-screen items-center p-6"
    :class="loginFormShellClass"
  >
    <a-card
      class="w-full max-w-[400px] shrink-0 shadow-md"
      :title="t('login.page.login.title')"
    >
      <p
        v-if="loginHolidayHint"
        class="mb-4 text-sm text-text-secondary"
      >
        {{ loginHolidayHint }}
      </p>

      <a-form
        ref="formRef"
        :model="formData"
        :rules="rules"
        layout="vertical"
        @finish="handleLogin"
      >
        <a-form-item
          :label="t('login.page.tenant.code')"
          name="tenantCode"
          :auto-link="false"
        >
          <a-input
            v-model:value="formData.tenantCode"
            :placeholder="t('login.page.field.tenant.code.placeholder')"
            size="large"
            show-count
            :maxlength="TAKT_TENANT_CODE_LENGTH"
            inputmode="numeric"
            autocomplete="organization"
            :disabled="tenantValidating"
            @blur="handleTenantBlur"
            @press-enter="handleTenantPressEnter"
          >
            <template #prefix>
              <RiBuildingLine class="text-text-secondary takt-remix-icon" />
            </template>
          </a-input>
        </a-form-item>

        <a-form-item
          :label="t('login.page.field.userName.label')"
          name="userName"
          :auto-link="false"
        >
          <a-input
            ref="UserNameInputRef"
            v-model:value="formData.userName"
            :placeholder="t('login.page.field.userName.placeholder')"
            size="large"
            show-count
            :maxlength="LOGIN_USER_NAME_MAX_LENGTH"
            autocomplete="username"
            @blur="handleUserNameBlur"
          >
            <template #prefix>
              <RiUserLine class="text-text-secondary takt-remix-icon" />
            </template>
          </a-input>
        </a-form-item>

        <a-form-item
          :label="t('login.page.field.password.label')"
          name="password"
          :auto-link="false"
        >
          <a-input-password
            v-model:value="formData.password"
            :placeholder="t('login.page.field.password.placeholder')"
            size="large"
            show-count
            :maxlength="LOGIN_PASSWORD_MAX_LENGTH"
            autocomplete="current-password"
          >
            <template #prefix>
              <RiLockPasswordLine class="text-text-secondary takt-remix-icon" />
            </template>
          </a-input-password>
        </a-form-item>

        <a-form-item
          :label="t('login.page.field.culture.label')"
          name="cultureCode"
        >
          <TaktSelect
            :model-value="localeStore.currentLocale"
            :options="loginCultureSelectOptions"
            :loading="localeStore.loading"
            :disabled="!tenantValidated || loginCultureSelectOptions.length === 0"
            :placeholder="t('login.page.field.culture.placeholder')"
            size="large"
            :allow-clear="false"
            @update:model-value="handleCultureChange"
          />
        </a-form-item>

        <a-form-item class="!mb-2">
          <div class="flex w-full items-center justify-between gap-2">
            <a-checkbox v-model:checked="formData.rememberMe">
              {{ t('login.page.login.rememberme') }}
            </a-checkbox>
            <a
              v-if="showForgotPassword"
              class="shrink-0 text-sm text-primary"
              href="#"
              @click.prevent="emit('forgot')"
            >
              {{ t('login.page.forgot.password') }}
            </a>
          </div>
        </a-form-item>

        <a-form-item class="!mb-2">
          <a-button type="primary" html-type="submit" size="large" block :loading="loading">
            <template #icon>
              <RiLoginBoxLine class="takt-remix-icon" />
            </template>
            {{ t('login.page.login.login') }}
          </a-button>
        </a-form-item>

        <a-form-item v-if="showRegister" class="!mb-0 text-center">
          <i18n-t
            keypath="login.page.login.noaccountregister"
            scope="global"
            tag="span"
            class="text-sm text-text-secondary"
          >
            <template #register>
              <a
                class="text-primary"
                href="#"
                @click.prevent="emit('register')"
              >
                {{ t('login.page.sign.title') }}
              </a>
            </template>
          </i18n-t>
        </a-form-item>
      </a-form>
    </a-card>
  </div>

  <template v-if="captchaModalOpen">
    <TaktCaptchaSlider
      v-if="captchaIsSlider"
      v-model:open="captchaModalOpen"
      :ref="bindCaptchaPanelRef"
      :challenge="captchaChallenge"
      :loading="captchaLoading"
      @can-submit-change="handleCaptchaCanSubmitChange"
      @request-refresh="loadCaptchaChallengeAsync"
      @cancel="handleCaptchaCancel"
    />
    <TaktCaptchaBehavior
      v-else
      v-model:open="captchaModalOpen"
      :ref="bindCaptchaPanelRef"
      :challenge="captchaChallenge"
      :loading="captchaLoading"
      @can-submit-change="handleCaptchaCanSubmitChange"
      @cancel="handleCaptchaCancel"
    />
  </template>
</template>

<script setup lang="ts">
/**
 * 登录主表单：验密、验证码、Cookie 会话与 OAuth 授权跳转
 */
import { I18nT, useI18n } from 'vue-i18n';
import { message } from 'ant-design-vue';
import { RiBuildingLine, RiLoginBoxLine, RiLockPasswordLine, RiUserLine } from '@remixicon/vue';
import type { FormInstance } from 'ant-design-vue';
import type { Rule } from 'ant-design-vue/es/form';
import { useLoginFieldSync } from '@/composables/useLoginFieldSync';
import {
  probeSessionCaptchaRequiredAsync,
  useTaktLoginCaptcha,
  type TaktCaptchaPanelExpose,
} from '@/composables/useTaktLoginCaptcha';
import { normalizeSessionVerifyPasswordResponse } from '@/utils/taktSessionVerifyPassword';
import type { TaktCaptchaSubmitPayload } from '@/types/identity/captcha.types';
import { useTenantStore } from '@/stores/identity/useTenantStore';
import { useUserStore } from '@/stores/identity/useUserStore';
import { getSessionLoginPublicKey, signInSession, verifySessionPassword } from '@/api/identity/auth.api';
import { encryptLoginPassword } from '@/utils/crypto';
import { redirectToAuthorize } from '@/utils/oauth';
import { resolveRequestLocale, resolveCultureCode, useLocaleStore } from '@/stores/foundation/useLocaleStore';
import { useSettingStore } from '@/stores/common/useSettingStore';
import type { TaktLoginLayoutPosition } from '@/utils/taktLoginLayoutDom';
import { isValidLoginUserName, isValidPassword, isValidTenantCode, LOGIN_PASSWORD_MAX_LENGTH, LOGIN_USER_NAME_MAX_LENGTH } from '@/utils/regex';
import { TAKT_TENANT_CODE_LENGTH } from '@/utils/common';

interface Props {
  /** 登录卡片在视口中的水平对齐（由父级 layout 切换传入） */
  layoutPosition: TaktLoginLayoutPosition;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  register: [];
  forgot: [];
}>();

/** i18n 翻译函数 */
const { t } = useI18n();

/** 全局偏好（控制注册、忘记密码入口显隐） */
const settingStore = useSettingStore();

/** 是否显示注册入口 */
const showRegister = computed(() => settingStore.setting.showRegister);

/** 是否显示忘记密码入口 */
const showForgotPassword = computed(() => settingStore.setting.showForgotPassword);

/** 当前路由（读取 OAuth 回跳 returnUrl） */
const route = useRoute();

/** 租户 Pinia */
const tenantStore = useTenantStore();

/** 用户 Pinia */
const userStore = useUserStore();

/** 主表单提交 loading */
const loading = ref(false);

/** 登录表单实例 */
const formRef = ref<FormInstance>();

/** 用户名输入框（租户校验后聚焦） */
const UserNameInputRef = ref<{ focus?: () => void } | null>(null);

/** 登录表单模型 */
const formData = reactive({
  tenantCode: '000',
  userName: 'admin',
  password: 'Takt@123456',
  rememberMe: false,
});

/**
 * 登录字段同步：租户校验 → 登录预览
 */
const {
  tenantValidated,
  tenantValidating,
  onTenantInputChange,
  onUserNameInputChange,
  commitTenantAsync,
  commitUserNamePreviewAsync,
  dispose: disposeLoginFieldSync,
} = useLoginFieldSync({
  getTenantCode: () => formData.tenantCode,
  setTenantCode: (code) => {
    formData.tenantCode = code;
  },
  getUserName: () => formData.userName,
  setUserName: (userName) => {
    formData.userName = userName;
  },
  formRef,
});

/** 区域文化 Pinia（租户校验通过后加载该租户可选语言；用户名预览后写入用户默认语言） */
const localeStore = useLocaleStore();

/** 登录页语言下拉选项（租户库 TaktCulture） */
const loginCultureSelectOptions = computed(() =>
  localeStore.cultureOptions.map((item) => ({
    label: String(item.dictLabel ?? resolveCultureCode(item)),
    value: resolveCultureCode(item),
  })),
);

/**
 * 登录页语言切换（用户手动选择后不再被预览覆盖）
 * @param value 选中的 CultureCode
 */
function handleCultureChange(value: string | number | (string | number)[] | undefined): void {
  if (typeof value === 'string' && value.trim()) {
    localeStore.setLocale(value);
  }
}

/** 登录卡片假日提示（问候语或假日名称） */
const loginHolidayHint = computed(() => {
  void localeStore.currentLocale;
  const holiday = userStore.holidayFromToken;
  if (!holiday?.isHolidayToday) {
    return '';
  }
  const greeting = holiday.holidayGreeting?.trim();
  if (greeting) {
    return greeting;
  }
  return holiday.holidayName?.trim() ?? '';
});

/** 验证码弹窗是否打开 */
const captchaModalOpen = ref(false);

/** verify-password 签发的登录票据 */
const loginTicket = ref<string | null>(null);

/** 验密成功时绑定的租户+用户名（signin 须与签发时一致） */
const loginSessionContext = ref<{ tenantCode: string; userName: string } | null>(null);

/** 启用验证码时，待验密的租户/用户名/密文密码 */
const pendingVerifyPayload = ref<{
  tenantCode: string;
  userName: string;
  password: string;
} | null>(null);

/** 防止验证码通过后重复触发 signin */
const signInSubmitting = ref(false);

/** 登录密码 RSA 公钥 PEM */
const loginPublicKeyPem = ref('');

/**
 * 登录表单列宽：居中占满视口；左右分栏时占视口 1/3（小屏回退为满宽）
 */
const loginFormShellClass = computed(() => {
  if (props.layoutPosition === 'center') {
    return 'w-full justify-center';
  }
  return 'w-full shrink-0 justify-center lg:w-1/3';
});

/** 登录验证码组合式 */
const {
  loading: captchaLoading,
  challenge: captchaChallenge,
  panelRef,
  isSlider: captchaIsSlider,
  loadChallengeAsync: loadCaptchaChallengeAsync,
  handleCanSubmitChange: handleCaptchaCanSubmitChange,
  registerOnVerified: registerCaptchaOnVerified,
  registerOnCaptchaSkipped: registerCaptchaOnCaptchaSkipped,
  confirmCaptcha,
  cancelCaptcha,
} = useTaktLoginCaptcha(captchaModalOpen);

/**
 * 绑定验证码面板实例
 * @param el 子组件实例或 null
 */
function bindCaptchaPanelRef(el: Element | ComponentPublicInstance | null): void {
  panelRef.value = el as TaktCaptchaPanelExpose | null;
}

watch(
  () => formData.tenantCode,
  () => {
    onTenantInputChange();
  },
);

watch(
  () => formData.userName,
  () => {
    onUserNameInputChange();
  },
);

/**
 * 租户输入框失焦：立即提交租户校验
 */
function handleTenantBlur(): void {
  void commitTenantAsync();
}

/**
 * 租户输入框回车：立即校验并聚焦用户名
 * @param event 键盘事件
 */
function handleTenantPressEnter(event: KeyboardEvent): void {
  event.preventDefault();
  void commitTenantAsync().then(() => {
    UserNameInputRef.value?.focus?.();
  });
}

/**
 * 用户名输入框失焦：立即拉取登录预览
 */
function handleUserNameBlur(): void {
  void commitUserNamePreviewAsync();
}

onMounted(() => {
  void loadLoginPublicKeyAsync().catch(() => undefined);
});

onUnmounted(() => {
  disposeLoginFieldSync();
});

/**
 * 拉取登录密码 RSA 公钥
 */
async function loadLoginPublicKeyAsync(): Promise<void> {
  const keyDto = await getSessionLoginPublicKey();
  loginPublicKeyPem.value = keyDto.publicKeyPem;
}

/**
 * 将表单明文密码加密为 API 所需 RSA 密文
 */
async function buildCipherPasswordAsync(): Promise<string> {
  if (!loginPublicKeyPem.value) {
    await loadLoginPublicKeyAsync();
  }
  if (!loginPublicKeyPem.value) {
    throw new Error(t('login.page.message.public.key.missing'));
  }

  const plain = formData.password.trim();
  const cipher = encryptLoginPassword(plain, loginPublicKeyPem.value);
  if (!cipher) {
    throw new Error(t('login.page.message.encrypt.fail'));
  }

  return cipher;
}

/** Ant Design Form 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  tenantCode: [
    {
      required: true,
      message: t('login.page.validate.tenant.required'),
      trigger: ['blur', 'change'],
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(formData.tenantCode ?? value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        if (!isValidTenantCode(trimmed)) {
          return Promise.reject(t('login.page.validate.tenant.invalid'));
        }
        if (tenantValidating.value) {
          return Promise.resolve();
        }
        if (tenantValidated.value && tenantStore.tenantCode === trimmed) {
          return Promise.resolve();
        }
        if (!tenantValidated.value) {
          return Promise.reject(t('login.page.message.tenant.not.found'));
        }
        return Promise.resolve();
      },
      trigger: ['blur', 'change'],
    }],
  userName: [
    {
      required: true,
      message: t('login.page.validate.userName.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(formData.userName ?? value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        if (!isValidLoginUserName(trimmed)) {
          return Promise.reject(t('login.page.validate.userName.invalid'));
        }
        return Promise.resolve();
      },
      trigger: 'blur',
    }],
  password: [
    {
      required: true,
      message: t('login.page.validate.password.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(formData.password ?? value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        if (!isValidPassword(trimmed)) {
          return Promise.reject(t('login.page.validate.password.weak'));
        }
        return Promise.resolve();
      },
      trigger: 'blur',
    }],
}));

/**
 * 解析当前登录凭据（租户 + 用户名，与后端 NormalizeLogin* 一致）
 * @returns {object} 租户编码与用户名
 */
function resolveLoginCredentials(): { tenantCode: string; userName: string } {
  const tenantCode = (tenantStore.tenantCode || formData.tenantCode.trim());
  const userName = formData.userName.trim().toLowerCase();
  return { tenantCode, userName };
}

/**
 * 建立 Cookie 会话并跳转 OpenIddict 授权
 * @param captcha 验证码提交载荷
 */
async function completeSignInAsync(captcha?: TaktCaptchaSubmitPayload): Promise<void> {
  const ctx = loginSessionContext.value;
  if (!ctx?.tenantCode || !ctx.userName) {
    throw new Error(t('login.page.message.fail'));
  }
  const ticket = loginTicket.value?.trim();
  if (!ticket) {
    throw new Error(t('login.page.message.fail'));
  }
  const cultureCode = resolveRequestLocale();

  await signInSession({
    userName: ctx.userName,
    tenantCode: ctx.tenantCode,
    cultureCode,
    rememberMe: formData.rememberMe,
    loginTicket: ticket,
    captchaId: captcha?.captchaId,
    captchaCode: captcha?.captchaCode,
  });

  loginTicket.value = null;
  loginSessionContext.value = null;

  const returnUrl = route.query.returnUrl as string | undefined;
  if (returnUrl) {
    sessionStorage.setItem('takt.oauth.return_after_login', returnUrl);
  }

  tenantStore.persistOAuthTenantCode(ctx.tenantCode);
  await redirectToAuthorize();
}

/**
 * 解析登录接口错误文案
 * @param error 捕获的异常
 * @param fallback 默认文案
 */
function resolveLoginErrorMessage(error: unknown, fallback: string): string {
  if (!(error instanceof Error) || !error.message) {
    return fallback;
  }

  const msg = error.message.trim();
  const tenantNoAccessHints = [
    'common.permission.tenant.no.access',
    '当前用户无权登录所选租户',
    'You do not have permission to sign in to the selecmed tenant',
    '選択したテナントにログインする権限がありません'] as const;

  if (tenantNoAccessHints.some((hint) => msg.includes(hint))) {
    return t('login.page.message.tenant.no.access');
  }

  const passwordCipherHints = [
    'common.field.password.cipher',
    'common.validation.invalidformat',
    '密码传输密文',
    'Encrypted password payload',
    'パスワード暗号文',
    '密碼傳輸密文'] as const;

  if (passwordCipherHints.some((hint) => msg.toLowerCase().includes(hint.toLowerCase()))) {
    return t('login.page.message.passwordCipherInvalid');
  }

  const invalidCredentialsHints = [
    'login.page.message.credentialsIncorrect',
    'common.validation.incorrect',
    'common.field.login.credentials',
    '用户或密码错误',
    '用户名或密码错误',
    'Incorrect UserName or password',
    'Incorrect username or password',
    'Incorrect user or password',
    'ユーザーまたはパスワードが正しくありません',
    'ユーザー名またはパスワードが正しくありません'] as const;

  if (invalidCredentialsHints.some((hint) => msg.includes(hint))) {
    return t('login.page.message.credentials.incorrect');
  }

  const accountLockedHints = [
    'common.status.account.locked',
    'login.page.message.account.locked',
    '账户已锁定',
    'account is locked',
    'アカウントがロック'] as const;

  if (accountLockedHints.some((hint) => msg.toLowerCase().includes(hint.toLowerCase()))) {
    return t('login.page.message.account.locked');
  }

  return msg;
}

/**
 * 验密并建立会话（verify-password → signin；有票据时 signin 跳过验证码）
 * @param payload 验密请求载荷
 */
async function runVerifyAndSignInAsync(payload: {
  tenantCode: string;
  userName: string;
  password: string;
  captchaId?: string;
  captchaCode?: string;
}): Promise<void> {
  const verifyResult = normalizeSessionVerifyPasswordResponse(
    await verifySessionPassword({
      userName: payload.userName,
      password: payload.password,
      tenantCode: payload.tenantCode,
      captchaId: payload.captchaId,
      captchaCode: payload.captchaCode,
    }),
  );

  const ticket = verifyResult.loginTicket?.trim();
  if (!ticket) {
    throw new Error(t('login.page.message.fail'));
  }

  loginTicket.value = ticket;
  loginSessionContext.value = { tenantCode: payload.tenantCode, userName: payload.userName };
  pendingVerifyPayload.value = null;

  await completeSignInAsync();
}

/**
 * 提交登录表单
 */
async function handleLogin(): Promise<void> {
  loading.value = true;
  try {
    const tenantOk = await commitTenantAsync();
    if (!tenantOk) {
      await formRef.value?.validateFields(['tenantCode']).catch(() => undefined);
      return;
    }

    await commitUserNamePreviewAsync();
    await formRef.value?.validate();

    const { tenantCode, userName } = resolveLoginCredentials();
    const cipherPassword = await buildCipherPasswordAsync();

    const needCaptchaUpfront = await probeSessionCaptchaRequiredAsync();
    if (needCaptchaUpfront) {
      pendingVerifyPayload.value = { tenantCode, userName, password: cipherPassword };
      captchaModalOpen.value = true;
      return;
    }

    await runVerifyAndSignInAsync({ tenantCode, userName, password: cipherPassword });
  } catch (error) {
    loginTicket.value = null;
    loginSessionContext.value = null;
    pendingVerifyPayload.value = null;
    message.error(resolveLoginErrorMessage(error, t('login.page.message.fail')));
  } finally {
    loading.value = false;
  }
}

/**
 * 验证码弹窗确认
 */
async function handleCaptchaConfirm(): Promise<void> {
  if (loading.value || signInSubmitting.value) {
    return;
  }

  const captchaPayload = confirmCaptcha();
  if (!captchaPayload) {
    return;
  }

  const pending = pendingVerifyPayload.value;
  if (!pending) {
    const ctx = loginSessionContext.value;
    if (!ctx?.tenantCode || !loginTicket.value?.trim()) {
      message.error(t('login.page.message.fail'));
      captchaModalOpen.value = true;
      return;
    }

    loading.value = true;
    signInSubmitting.value = true;
    try {
      if (tenantStore.tenantCode !== ctx.tenantCode) {
        tenantStore.setTenant(ctx.tenantCode);
      }
      await completeSignInAsync(captchaPayload);
    } catch (error) {
      loginTicket.value = null;
      loginSessionContext.value = null;
      message.error(resolveLoginErrorMessage(error, t('login.page.message.fail')));
      captchaModalOpen.value = true;
    } finally {
      loading.value = false;
      signInSubmitting.value = false;
    }
    return;
  }

  loading.value = true;
  signInSubmitting.value = true;
  try {
    if (tenantStore.tenantCode !== pending.tenantCode) {
      tenantStore.setTenant(pending.tenantCode);
    }
    await runVerifyAndSignInAsync({
      ...pending,
      captchaId: captchaPayload.captchaId,
      captchaCode: captchaPayload.captchaCode,
    });
  } catch (error) {
    loginTicket.value = null;
    loginSessionContext.value = null;
    pendingVerifyPayload.value = null;
    message.error(resolveLoginErrorMessage(error, t('login.page.message.fail')));
    captchaModalOpen.value = true;
  } finally {
    loading.value = false;
    signInSubmitting.value = false;
  }
}

/**
 * 验证码弹窗取消
 */
function handleCaptchaCancel(): void {
  loginTicket.value = null;
  loginSessionContext.value = null;
  pendingVerifyPayload.value = null;
  cancelCaptcha();
}

registerCaptchaOnVerified(handleCaptchaConfirm);

registerCaptchaOnCaptchaSkipped(async () => {
  const pending = pendingVerifyPayload.value;
  if (!pending) {
    loading.value = true;
    signInSubmitting.value = true;
    try {
      await completeSignInAsync();
    } catch (error) {
      loginTicket.value = null;
      loginSessionContext.value = null;
      message.error(resolveLoginErrorMessage(error, t('login.page.message.fail')));
    } finally {
      loading.value = false;
      signInSubmitting.value = false;
    }
    return;
  }

  loading.value = true;
  signInSubmitting.value = true;
  try {
    await runVerifyAndSignInAsync(pending);
  } catch (error) {
    loginTicket.value = null;
    loginSessionContext.value = null;
    pendingVerifyPayload.value = null;
    message.error(resolveLoginErrorMessage(error, t('login.page.message.fail')));
  } finally {
    loading.value = false;
    signInSubmitting.value = false;
  }
});
</script>

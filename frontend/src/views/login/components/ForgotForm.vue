<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：ForgotForm.vue -->
<!-- 功能描述：忘记密码（a-steps 三步；验证码内嵌于步骤条） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div
    class="relative flex min-h-screen items-center bg-gradient-to-br from-[#667eea] to-[#764ba2] p-6"
    :class="{
      'justify-start': layoutPosition === 'left',
      'justify-center': layoutPosition === 'center',
      'justify-end': layoutPosition === 'right',
    }"
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
        <a-radio-button value="locale">
          <TaktLocaleToggle type="icon" size="small" />
        </a-radio-button>
        <a-radio-button value="theme">
          <TaktThemeToggle type="icon" size="small" />
        </a-radio-button>
      </a-button-group>
    </div>

    <a-card
      class="w-full max-w-[460px] shadow-md"
      :title="t('login.page.forgot.title')"
    >
      <a-steps
        :current="currentStep"
        size="small"
        class="mb-6"
      >
        <a-step :title="t('login.page.forgot.steps.email')" />
        <a-step :title="t('login.page.forgot.steps.captcha')" />
        <a-step :title="t('login.page.forgot.steps.done')" />
      </a-steps>

      <!-- 步骤 1：输入邮箱 -->
      <div v-if="currentStep === FORGOT_STEP_EMAIL">
        <p class="mb-4 text-sm text-text-secondary">
          {{ t('login.page.forgot.subtitle') }}
        </p>

        <a-form
          ref="formRef"
          :model="formState"
          :rules="rules"
          layout="vertical"
          @finish="handleEmailStepSubmit"
        >
          <a-form-item :label="t('login.page.field.userName.or.email.label')" name="userEmail">
            <a-input
              v-model:value="formState.userEmail"
              :placeholder="t('login.page.field.userName.or.email.placeholder')"
              size="large"
              show-count
              :maxlength="EMAIL_MAX_LENGTH"
              autocomplete="email"
            >
              <template #prefix>
                <RiMailLine class="text-text-secondary takt-remix-icon" />
              </template>
            </a-input>
          </a-form-item>

          <a-form-item class="!mb-2">
            <a-button
              type="primary"
              html-type="submit"
              size="large"
              block
              :loading="loading"
              class="!inline-flex items-center justify-center"
            >
              <span class="inline-flex items-center justify-center gap-2">
                <RiLockPasswordLine class="takt-remix-icon shrink-0" />
                <span>{{ t('common.page.button.next') }}</span>
              </span>
            </a-button>
          </a-form-item>

          <a-form-item class="!mb-0 text-center">
            <a class="text-sm text-primary" href="#" @click.prevent="goToLogin">
              {{ t('login.page.forgot.backtologin') }}
            </a>
          </a-form-item>
        </a-form>
      </div>

      <!-- 步骤 2：安全验证 -->
      <div v-else-if="currentStep === FORGOT_STEP_CAPTCHA">
        <p class="mb-4 text-sm text-text-secondary">
          {{ t('login.page.captcha.title') }}
        </p>

        <TaktCaptchaSlider
          v-if="captchaIsSlider"
          :ref="bindCaptchaPanelRef"
          embedded
          :challenge="captchaChallenge"
          :loading="captchaLoading"
          @can-submit-change="handleCaptchaCanSubmitChange"
          @request-refresh="loadCaptchaChallengeAsync"
        />
        <TaktCaptchaBehavior
          v-else
          :ref="bindCaptchaPanelRef"
          embedded
          :challenge="captchaChallenge"
          :loading="captchaLoading"
          @can-submit-change="handleCaptchaCanSubmitChange"
        />

        <a-button
          class="mt-4"
          block
          size="large"
          @click="handleCaptchaStepBack"
        >
          {{ t('common.page.button.prev') }}
        </a-button>
      </div>

      <!-- 步骤 3：完成 -->
      <div v-else-if="currentStep === FORGOT_STEP_DONE">
        <a-result
          status="success"
          :title="t('login.page.forgot.email.sent')"
        >
          <template #extra>
            <a-button type="primary" @click="goToLogin">
              {{ t('login.page.forgot.backtologin') }}
            </a-button>
          </template>
        </a-result>
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
/**
 * 忘记密码：a-steps 三步（邮箱 → 验证码 → 完成）；验证码内嵌于步骤条。
 */
import { useI18n } from 'vue-i18n';
import { message } from 'ant-design-vue';
import { RiMailLine, RiLockPasswordLine } from '@remixicon/vue';
import type { Rule } from 'ant-design-vue/es/form';
import LoginBrand from '@/views/login/components/LoginBrand.vue';
import {
  isTaktCaptchaDisabledError,
  probeSessionCaptchaRequiredAsync,
  useTaktLoginCaptcha,
  type TaktCaptchaPanelExpose,
} from '@/composables/useTaktLoginCaptcha';
import { forgotPassword } from '@/api/identity/user.api';
import { TaktApiError } from '@/api/request';
import { probeHealthAsync } from '@/api/health.api';
import { EMAIL_MAX_LENGTH, EMAIL_MIN_LENGTH, isValidEmail } from '@/utils/regex';
import {
  readStoredLoginLayoutPosition,
  type TaktLoginLayoutPosition,
} from '@/utils/taktLoginLayoutDom';

/** 内嵌模式：由 `login/index.vue` 切换展示 */
interface Props {
  /** 为 true 时成功或返回登录由 `emit('back')` 切回主表单 */
  embedded?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  embedded: false,
});

const emit = defineEmits<{
  (e: 'back'): void;
}>();

/** i18n */
const { t } = useI18n();

/** 路由（非内嵌时跳转登录） */
const router = useRouter();

/** 忘记密码页日志 */
const forgotLogger = createLogger('ForgotPassword');

/** 表单区水平对齐 */
const layoutPosition = ref<TaktLoginLayoutPosition>(readStoredLoginLayoutPosition('center'));

/** 忘记密码步骤：输入邮箱 */
const FORGOT_STEP_EMAIL = 0;

/** 忘记密码步骤：安全验证 */
const FORGOT_STEP_CAPTCHA = 1;

/** 忘记密码步骤：完成 */
const FORGOT_STEP_DONE = 2;

/** 当前步骤（与 a-steps :current 对齐） */
const currentStep = ref(FORGOT_STEP_EMAIL);

/** 验证码会话是否激活（拉取挑战） */
const captchaSessionActive = ref(false);

/** 登录验证码组合式 */
const {
  loading: captchaLoading,
  challenge: captchaChallenge,
  panelRef,
  registerOnVerified: registerCaptchaOnVerified,
  registerOnCaptchaSkipped: registerCaptchaOnCaptchaSkipped,
  isSlider: captchaIsSlider,
  loadChallengeAsync: loadCaptchaChallengeAsync,
  handleCanSubmitChange: handleCaptchaCanSubmitChange,
  confirmCaptcha,
  cancelCaptcha,
} = useTaktLoginCaptcha(captchaSessionActive);

/**
 * 绑定验证码面板实例
 * @param el 子组件实例或 null
 */
function bindCaptchaPanelRef(el: Element | ComponentPublicInstance | null): void {
  panelRef.value = el as TaktCaptchaPanelExpose | null;
}

/** 忘记密码表单 */
const formState = reactive({
  userEmail: '',
});

/** 提交 loading */
const loading = ref(false);

/** 表单实例 */
const formRef = ref();

/** 邮箱校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  userEmail: [
    {
      required: true,
      message: t('login.page.validate.userName.or.email.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        if (!isValidEmail(trimmed)) {
          return Promise.reject(t('login.page.validate.userName.or.email.invalid'));
        }
        if (trimmed.length < EMAIL_MIN_LENGTH) {
          return Promise.reject(
            t('login.page.validate.userName.or.email.too.short', { min: EMAIL_MIN_LENGTH })
          );
        }
        if (trimmed.length > EMAIL_MAX_LENGTH) {
          return Promise.reject(
            t('login.page.validate.userName.or.email.too.long', { max: EMAIL_MAX_LENGTH })
          );
        }
        return Promise.resolve();
      },
      trigger: 'blur',
    }],
}));

/**
 * 解析忘记密码接口业务错误文案
 * @param error 捕获的异常
 * @returns {string} 展示用错误信息
 */
function resolveForgotPasswordErrorMessage(error: unknown): string {
  if (error instanceof TaktApiError) {
    const code =
      typeof error.data === 'object' && error.data !== null && 'code' in error.data
        ? String((error.data as { code?: string }).code)
        : '';
    if (code === 'ProtectedUser') {
      return t('login.page.forgot.reset.unavailable');
    }
    if (code === 'EmailNotFound') {
      return t('login.page.forgot.emailnotregistered');
    }
    if (error.message) {
      return error.message;
    }
  }
  if (error instanceof Error && error.message) {
    return error.message;
  }
  return t('login.page.forgot.fail');
}

/**
 * 调用忘记密码接口
 * @returns {Promise<void>}
 */
async function doForgotPasswordAsync(): Promise<void> {
  try {
    loading.value = true;
    await forgotPassword({ UserNameOrEmail: formState.userEmail.trim() });
    message.success(t('login.page.forgot.email.sent'));
    formState.userEmail = '';
    currentStep.value = FORGOT_STEP_DONE;
    captchaSessionActive.value = false;
    setTimeout(() => {
      if (props.embedded) {
        emit('back');
      } else {
        router.push('/login');
      }
    }, 1500);
  } catch (error: unknown) {
    forgotLogger.error('发送密码重置邮件失败', { action: 'forgotPassword' }, error);
    message.error(resolveForgotPasswordErrorMessage(error));
    captchaSessionActive.value = false;
    if (currentStep.value === FORGOT_STEP_CAPTCHA) {
      currentStep.value = FORGOT_STEP_EMAIL;
    }
  } finally {
    loading.value = false;
  }
}

/**
 * 验证码弹窗确认
 * @returns {Promise<void>}
 */
async function handleCaptchaConfirm(): Promise<void> {
  if (loading.value) {
    return;
  }

  const payload = confirmCaptcha();
  if (!payload) {
    return;
  }
  await doForgotPasswordAsync();
}

/** 验证码步骤返回上一步 */
function handleCaptchaStepBack(): void {
  currentStep.value = FORGOT_STEP_EMAIL;
  captchaSessionActive.value = false;
  cancelCaptcha();
}

/**
 * 步骤 1 提交：探测验证码后进入步骤 2 或直接发信
 * @returns {Promise<void>}
 */
async function handleEmailStepSubmit(): Promise<void> {
  try {
    await formRef.value?.validateFields(['userEmail']);
  } catch {
    return;
  }

  loading.value = true;
  try {
    const captchaRequired = await probeSessionCaptchaRequiredAsync();
    if (captchaRequired) {
      currentStep.value = FORGOT_STEP_CAPTCHA;
      captchaSessionActive.value = true;
      return;
    }
    await doForgotPasswordAsync();
  } catch (error: unknown) {
    if (isTaktCaptchaDisabledError(error)) {
      await doForgotPasswordAsync();
      return;
    }
    forgotLogger.error('获取验证码挑战失败', { action: 'probeCaptcha' }, error);
    message.error(
      error instanceof Error && error.message
        ? error.message
        : t('login.page.validate.captcha.required'),
    );
  } finally {
    loading.value = false;
  }
}

registerCaptchaOnVerified(handleCaptchaConfirm);

registerCaptchaOnCaptchaSkipped(async () => {
  await doForgotPasswordAsync();
});

/** 返回登录主表单或路由 */
function goToLogin(): void {
  if (props.embedded) {
    emit('back');
  } else {
    router.push('/login');
  }
}

/** 预热 Cookie */
onMounted(async () => {
  try {
    await probeHealthAsync();
  } catch (error: unknown) {
    forgotLogger.warn('预热健康检查失败', { action: 'health' }, error);
  }

});
</script>

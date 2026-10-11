<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/login/components -->
<!-- 文件名称：RegisterForm.vue -->
<!-- 功能描述：用户自助注册（a-steps 三步；验证码内嵌于步骤条） -->
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
      :title="t('login.page.sign.title')"
    >
      <a-steps
        :current="currentStep"
        size="small"
        class="mb-6"
      >
        <a-step :title="t('login.page.sign.steps.info')" />
        <a-step :title="t('login.page.sign.steps.captcha')" />
        <a-step :title="t('login.page.sign.steps.done')" />
      </a-steps>

      <!-- 步骤 1：填写资料 -->
      <div v-if="currentStep === REGISTER_STEP_INFO">
        <p class="mb-4 text-sm text-text-secondary">
          {{ t('login.page.sign.subtitle') }}
        </p>

        <a-form
          ref="formRef"
          :model="formState"
          :rules="rules"
          layout="vertical"
          @finish="handleInfoStepSubmit"
        >
          <a-form-item :label="t('login.page.field.userName.label')" name="userName">
            <a-input
              v-model:value="formState.userName"
              :placeholder="t('login.page.field.userName.placeholder')"
              size="large"
              show-count
              :maxlength="LOGIN_USER_NAME_MAX_LENGTH"
              autocomplete="username"
            >
              <template #prefix>
                <RiUserLine class="text-text-secondary takt-remix-icon" />
              </template>
            </a-input>
          </a-form-item>

          <a-form-item :label="t('login.page.field.email.label')" name="userEmail">
            <a-input
              v-model:value="formState.userEmail"
              :placeholder="t('login.page.field.email.placeholder')"
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

          <a-form-item :label="t('login.page.field.phone.label')" name="userPhone">
            <a-input
              v-model:value="formState.userPhone"
              :placeholder="t('login.page.field.phone.placeholder')"
              size="large"
              show-count
              :maxlength="phoneMaxLength"
              inputmode="numeric"
              autocomplete="tel"
            >
              <template #prefix>
                <RiPhoneLine class="text-text-secondary takt-remix-icon" />
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
                <RiUserAddLine class="takt-remix-icon shrink-0" />
                <span>{{ t('common.page.button.next') }}</span>
              </span>
            </a-button>
          </a-form-item>

          <a-form-item class="!mb-0 text-center">
            <a class="text-sm text-primary" href="#" @click.prevent="goToLogin">
              {{ t('login.page.sign.hasaccount') }}
            </a>
          </a-form-item>
        </a-form>
      </div>

      <!-- 步骤 2：安全验证 -->
      <div v-else-if="currentStep === REGISTER_STEP_CAPTCHA">
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
      <div v-else-if="currentStep === REGISTER_STEP_DONE">
        <a-result
          status="success"
          :title="t('login.page.sign.successinitialpassword')"
        >
          <template #extra>
            <a-button type="primary" @click="goToLogin">
              {{ t('login.page.sign.hasaccount') }}
            </a-button>
          </template>
        </a-result>
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
/**
 * 自助注册：a-steps 三步（资料 → 验证码 → 完成）；验证码内嵌于步骤条。
 */
import { useI18n } from 'vue-i18n';
import { message } from 'ant-design-vue';
import { RiUserLine, RiUserAddLine, RiMailLine, RiPhoneLine } from '@remixicon/vue';
import type { Rule } from 'ant-design-vue/es/form';
import LoginBrand from '@/views/login/components/LoginBrand.vue';
import {
  isTaktCaptchaDisabledError,
  probeSessionCaptchaRequiredAsync,
  useTaktLoginCaptcha,
  type TaktCaptchaPanelExpose,
} from '@/composables/useTaktLoginCaptcha';
import { createUser } from '@/api/identity/user.api';
import { probeHealthAsync } from '@/api/health.api';
import type { CreateUser } from '@/types/identity/user.types';
import {
  readStoredLoginLayoutPosition,
  type TaktLoginLayoutPosition,
} from '@/utils/taktLoginLayoutDom';
import {
  EMAIL_MAX_LENGTH,
  EMAIL_MIN_LENGTH,
  getPhoneMaxLengthByCulture,
  isValidEmail,
  isValidLoginUserName,
  isValidPhoneByCulture,
  LOGIN_USER_NAME_MAX_LENGTH,
} from '@/utils/regex';
import { useLocaleStore } from '@/stores/foundation/useLocaleStore';

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

/** 区域文化（登录预览已同步公司 default_culture） */
const localeStore = useLocaleStore();

/** 按公司默认文化限制手机号长度 */
const phoneMaxLength = computed(() => getPhoneMaxLengthByCulture(localeStore.currentLocale));

/** 路由（非内嵌时跳转登录） */
const router = useRouter();

/** 注册页日志 */
const registerLogger = createLogger('Register');

/** 表单区水平对齐 */
const layoutPosition = ref<TaktLoginLayoutPosition>(readStoredLoginLayoutPosition('center'));

/** 注册步骤：填写资料 */
const REGISTER_STEP_INFO = 0;

/** 注册步骤：安全验证 */
const REGISTER_STEP_CAPTCHA = 1;

/** 注册步骤：完成 */
const REGISTER_STEP_DONE = 2;

/** 当前步骤（与 a-steps :current 对齐） */
const currentStep = ref(REGISTER_STEP_INFO);

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

/** 注册表单字段 */
const formState = reactive({
  userName: '',
  userEmail: '',
  userPhone: '',
});

/** 提交 loading */
const loading = ref(false);

/** 表单实例 */
const formRef = ref();

/** 表单校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  userName: [
    {
      required: true,
      message: t('login.page.validate.userName.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(value ?? '').trim();
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
  userEmail: [
    {
      required: true,
      message: t('login.page.validate.email.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        if (!isValidEmail(trimmed)) {
          return Promise.reject(t('login.page.validate.email.invalid'));
        }
        if (trimmed.length < EMAIL_MIN_LENGTH) {
          return Promise.reject(
            t('login.page.validate.email.too.short', { min: EMAIL_MIN_LENGTH })
          );
        }
        if (trimmed.length > EMAIL_MAX_LENGTH) {
          return Promise.reject(
            t('login.page.validate.email.too.long', { max: EMAIL_MAX_LENGTH })
          );
        }
        return Promise.resolve();
      },
      trigger: 'blur',
    }],
  userPhone: [
    {
      required: true,
      message: t('login.page.validate.phone.required'),
      trigger: 'blur',
    },
    {
      validator: async (_rule, value) => {
        const trimmed = String(value ?? '').trim();
        if (!trimmed) {
          return Promise.resolve();
        }
        const culture = localeStore.currentLocale;
        if (!isValidPhoneByCulture(trimmed, culture)) {
          return Promise.reject(t('login.page.validate.phone.invalid'));
        }
        return Promise.resolve();
      },
      trigger: 'blur',
    }],
}));

/**
 * 提交注册（开放注册默认档案字段，密码由后端策略处理）
 * @returns {Promise<void>}
 */
async function doRegisterAsync(): Promise<void> {
  try {
    loading.value = true;
    const userName = formState.userName.trim();
    const registerData = {
      employeeId: '0',
      userName: userName,
      nickName: userName,
      userType: 0,
      passwordHash: '',
      userStatus: 0,
      remark: `email:${formState.userEmail.trim()};phone:${formState.userPhone.trim()}`,
    } as CreateUser;
    await createUser(registerData);
    message.success(t('login.page.sign.successinitialpassword'));
    currentStep.value = REGISTER_STEP_DONE;
    captchaSessionActive.value = false;
    setTimeout(() => {
      if (props.embedded) {
        emit('back');
      } else {
        router.push('/login');
      }
    }, 1500);
  } catch (error: unknown) {
    registerLogger.error('注册失败', { action: 'createUser' }, error);
    message.error(error instanceof Error && error.message ? error.message : t('login.page.sign.fail'));
    captchaSessionActive.value = false;
    if (currentStep.value === REGISTER_STEP_CAPTCHA) {
      currentStep.value = REGISTER_STEP_INFO;
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
  await doRegisterAsync();
}

/** 验证码步骤返回上一步 */
function handleCaptchaStepBack(): void {
  currentStep.value = REGISTER_STEP_INFO;
  captchaSessionActive.value = false;
  cancelCaptcha();
}

/**
 * 步骤 1 提交：探测验证码后进入步骤 2 或直接注册
 * @returns {Promise<void>}
 */
async function handleInfoStepSubmit(): Promise<void> {
  try {
    await formRef.value?.validateFields(['userName', 'userEmail', 'userPhone']);
  } catch {
    return;
  }

  loading.value = true;
  try {
    const captchaRequired = await probeSessionCaptchaRequiredAsync();
    if (captchaRequired) {
      currentStep.value = REGISTER_STEP_CAPTCHA;
      captchaSessionActive.value = true;
      return;
    }
    await doRegisterAsync();
  } catch (error: unknown) {
    if (isTaktCaptchaDisabledError(error)) {
      await doRegisterAsync();
      return;
    }
    registerLogger.error('获取验证码挑战失败', { action: 'probeCaptcha' }, error);
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
  await doRegisterAsync();
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
    registerLogger.warn('预热健康检查失败', { action: 'health' }, error);
  }

});
</script>

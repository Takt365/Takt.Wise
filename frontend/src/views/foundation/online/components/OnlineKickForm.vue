<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/online/components -->
<!-- 文件名称：OnlineKickForm.vue -->
<!-- 功能描述：在线用户强退弹窗表单（立即强退 / 3 分钟后倒计时强退）；defineExpose validate/getValues/resetFields -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-row :gutter="24">
      <a-col :span="12">
        <a-form-item :label="t('entity.online.username')">
          <a-input :value="displayUserName" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.online.connectionid')">
          <a-input :value="displayConnectionId" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.online.connectip')">
          <a-input :value="displayConnectIp" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item :label="t('entity.online.connectlocation')">
          <a-input :value="displayConnectLocation" disabled />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('foundation.online.page.kick.mode')"
          name="kickMode"
        >
          <a-radio-group
            v-model:value="formState.kickMode"
            :disabled="loading"
          >
            <a-radio value="immediate">
              {{ t('common.page.button.kick.immediate') }}
            </a-radio>
            <a-radio value="delayed">
              {{ t('common.page.button.kick.delayed') }}
            </a-radio>
          </a-radio-group>
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-form-item
          :label="t('foundation.online.page.kick.reason')"
          name="reason"
        >
          <a-textarea
            v-model:value="formState.reason"
            :placeholder="t('foundation.online.page.kick.reason.placeholder')"
            :disabled="loading"
            :rows="3"
            allow-clear
          />
        </a-form-item>
      </a-col>
      <a-col :span="24">
        <a-alert
          v-if="formState.kickMode === 'delayed'"
          type="warning"
          show-icon
          :message="t('foundation.online.page.kick.delayed.hint')"
        />
      </a-col>
    </a-row>
  </a-form>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';
import type { FormInstance } from 'ant-design-vue';
import type { Rule } from 'ant-design-vue/es/form';
import { ONLINE_FORCE_KICK_DELAY_SECONDS } from '@/constants/online.constants';
import type { Online, OnlineForceKick } from '@/types/foundation/online.types';

/** 强退方式 */
export type OnlineKickMode = 'immediate' | 'delayed';

const props = withDefaults(
  defineProps<{
    /** 目标在线用户行 */
    formData?: Online | null;
    /** 打开弹窗时默认强退方式 */
    defaultKickMode?: OnlineKickMode;
    /** 父级提交 loading */
    loading?: boolean;
  }>(),
  {
    formData: null,
    defaultKickMode: 'immediate',
    loading: false,
  },
);

const { t } = useI18n();
/** 表单实例 */
const formRef = ref<FormInstance>();
/** 表单状态 */
const formState = reactive({
  kickMode: 'immediate' as OnlineKickMode,
  reason: '',
});

/** 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  kickMode: [{ required: true, message: t('foundation.online.page.kick.mode.required') }],
}));

/** 展示用户名 */
const displayUserName = computed(() => props.formData?.userName?.trim() || '—');
/** 展示连接 ID */
const displayConnectionId = computed(() => props.formData?.connectionId?.trim() || '—');
/** 展示连接 IP */
const displayConnectIp = computed(() => props.formData?.connectIp?.trim() || '—');
/** 展示连接地点 */
const displayConnectLocation = computed(() => props.formData?.connectLocation?.trim() || '—');

/** 目标在线用户主键 */
const targetOnlineId = computed(() => {
  const raw = props.formData?.onlineId;
  if (raw == null || raw === '') {
    return '';
  }
  return String(raw);
});

/** 目标 SignalR 连接 ID */
const targetConnectionId = computed(() => props.formData?.connectionId?.trim() || '');

watch(
  () => [props.formData, props.defaultKickMode] as const,
  () => {
    resetFields();
  },
);

/**
 * 校验表单
 * @returns {Promise<void>}
 */
async function validate(): Promise<void> {
  await formRef.value?.validate();
  if (!targetOnlineId.value && !targetConnectionId.value) {
    throw new Error(t('common.feedback.failed'));
  }
}

/**
 * 获取强退 API 入参
 * @returns {OnlineForceKick} 强退 DTO
 */
function getValues(): OnlineForceKick {
  const reason = formState.reason.trim();
  return {
    ...(targetConnectionId.value ? { connectionId: targetConnectionId.value } : {}),
    ...(reason ? { reason } : {}),
    delaySeconds: formState.kickMode === 'immediate' ? 0 : ONLINE_FORCE_KICK_DELAY_SECONDS,
  };
}

/**
 * 获取目标在线用户 ID（路径参数）
 * @returns {string} onlineId
 */
function getTargetOnlineId(): string {
  return targetOnlineId.value || '0';
}

/**
 * 重置表单
 */
function resetFields(): void {
  formState.kickMode = props.defaultKickMode ?? 'immediate';
  formState.reason = '';
  formRef.value?.clearValidate();
}

defineExpose({
  validate,
  getValues,
  getTargetOnlineId,
  resetFields,
});
</script>

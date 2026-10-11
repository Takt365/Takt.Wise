<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/meeting-center/meeting-notification -->
<!-- 文件名称：confirm.vue -->
<!-- 功能描述：会议通知邮件回执确认页（匿名；query token） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="flex min-h-screen items-center justify-center bg-page p-6">
    <div class="w-full max-w-lg rounded-lg bg-container p-8 shadow-sm border border-border">
      <a-spin :spinning="loading">
        <div class="text-center">
          <h1 class="text-xl font-semibold text-text mb-4">
            {{ t('routine.meeting-center.meeting-notification.page.confirm.title') }}
          </h1>
          <template v-if="status === 'success'">
            <a-result status="success" :title="successTitle" :sub-title="successSubTitle" />
          </template>
          <template v-else-if="status === 'error'">
            <a-result status="error" :title="t('routine.meeting-center.meeting-notification.page.confirm.failed')" :sub-title="errorMessage" />
          </template>
          <template v-else-if="status === 'missing'">
            <a-result status="warning" :title="t('routine.meeting-center.meeting-notification.page.confirm.missingToken')" />
          </template>
        </div>
      </a-spin>
    </div>
  </div>
</template>

<script setup lang="ts">
/**
 * 会议通知邮件回执确认（公开页）
 */
import { useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { confirmMeetingNotificationReceiptByToken } from '@/api/routine/meeting-center/meetingNotification.api'

const { t } = useI18n()
const route = useRoute()

/** 页面状态 */
const loading = ref(false)
/** success | error | missing | idle */
const status = ref<'success' | 'error' | 'missing' | 'idle'>('idle')
/** 错误文案 */
const errorMessage = ref('')
/** 会议标题 */
const meetingTitle = ref('')
/** 是否此前已确认 */
const alreadyConfirmed = ref(false)

/** 成功主标题 */
const successTitle = computed(() =>
  alreadyConfirmed.value
    ? t('routine.meeting-center.meeting-notification.page.confirm.alreadyConfirmed')
    : t('routine.meeting-center.meeting-notification.page.confirm.success'),
)

/** 成功副标题 */
const successSubTitle = computed(() =>
  meetingTitle.value
    ? t('routine.meeting-center.meeting-notification.page.confirm.meetingTitle', { title: meetingTitle.value })
    : '',
)

/** 提交回执确认 */
async function submitConfirm(token: string): Promise<void> {
  loading.value = true
  try {
    const result = await confirmMeetingNotificationReceiptByToken(token)
    meetingTitle.value = result.meetingTitle ?? ''
    alreadyConfirmed.value = !!result.alreadyConfirmed
    status.value = 'success'
  } catch (error: unknown) {
    status.value = 'error'
    const err = error as { message?: string }
    errorMessage.value = err?.message ?? t('routine.meeting-center.meeting-notification.page.confirm.failed')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  const raw = route.query.token
  const token = typeof raw === 'string' ? raw.trim() : ''
  if (!token) {
    status.value = 'missing'
    return
  }
  void submitConfirm(token)
})
</script>

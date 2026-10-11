<!-- ========================================
项目名称：节拍智控·TaktWise
命名空间：frontend/src/components/common/TaktForceLogoutBanner
文件名称：index.vue
创建时间：2026-07-01
创建人：Takt365(Cursor AI)
功能描述：延迟强退全局倒计时条（不阻塞页面操作）

版权信息：Copyright (c) 2026 Takt  All rights reserved.
免责声明：此软件使用 MIT License，作者不承担任何使用风险。
======================================== -->
<template>
  <div
    v-if="scheduleStore.active"
    class="pointer-events-none fixed inset-x-0 top-0 z-[2000] flex justify-center px-4 pt-2"
  >
    <a-alert
      type="warning"
      show-icon
      banner
      class="pointer-events-auto w-full max-w-3xl shadow-md"
      :message="scheduleStore.message"
      :description="countdownDescription"
    />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { useForceLogoutScheduleStore } from '@/stores/foundation/useForceLogoutScheduleStore';

const { t } = useI18n();
/** 延迟强退倒计时状态 */
const scheduleStore = useForceLogoutScheduleStore();

/** 倒计时描述文案 */
const countdownDescription = computed(() =>
  t('common.tip.force.logout.countdown', { time: scheduleStore.formattedCountdown }),
);
</script>

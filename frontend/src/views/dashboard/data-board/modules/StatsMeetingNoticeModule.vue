<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsMeetingNoticeModule.vue -->
<!-- 功能描述：数据看板会议/通知（本月：未开始/已完成/通知件数） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.meetingnotice.summaryTitle') }}
    </p>
    <StatsMetricGrid
      card-variant="dashboard-compact"
      :loading="loading"
      :period-label="t('dashboard.data-board.page.periodmonth')"
      :items="metricItems"
      :col-xs="12"
      :col-sm="12"
      :col-md="8"
      :col-lg="8"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会议通知：本月会议未开始/已完成 + 通知件数
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { MeetingStat } from '@/types/routine/meeting-center/meetingStat.types'
import type { AnnouncementStat } from '@/types/routine/announcementStat.types'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getCurrentMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

/** 空会议统计 */
const EMPTY_MEETING: MeetingStat = {
  statMonth: '',
  monthNotStartedCount: 0,
  monthCompletedCount: 0,
}

/** 空通知统计 */
const EMPTY_ANNOUNCEMENT: AnnouncementStat = {
  statMonth: '',
  monthAnnouncementCount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 会议统计 */
const meetingStat = ref<MeetingStat>({ ...EMPTY_MEETING })
/** 通知统计 */
const announcementStat = ref<AnnouncementStat>({ ...EMPTY_ANNOUNCEMENT })

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'meetingNotStarted',
      title: t('dashboard.data-board.page.meetingnotice.notstarted'),
      value: Number(meetingStat.value.monthNotStartedCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.meeting,
    },
    {
      key: 'meetingCompleted',
      title: t('dashboard.data-board.page.meetingnotice.completed'),
      value: Number(meetingStat.value.monthCompletedCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.meeting,
    },
    {
      key: 'announcementCount',
      title: t('dashboard.data-board.page.meetingnotice.announcementcount'),
      value: Number(announcementStat.value.monthAnnouncementCount) || 0,
      routePath: DASHBOARD_STATS_ROUTE.announcement,
    },
  ]),
)

/**
 * 加载本月会议与通知统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const canMeeting = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.meetingList)
  const canAnnouncement = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.announcementList)
  const range = getCurrentMonthRange()
  loading.value = true
  try {
    const [meeting, announcement] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canMeeting,
        'meetingStat',
        async () => {
          const res = await fetchDashboardGet<MeetingStat>(DASHBOARD_STATS_API.meetingStat, {
            startTimeStart: range.start,
            startTimeEnd: range.end,
          })
          return res ?? { ...EMPTY_MEETING }
        },
        { ...EMPTY_MEETING },
      ),
      fetchDashboardMetricIfPermitted(
        canAnnouncement,
        'announcementStat',
        async () => {
          const res = await fetchDashboardGet<AnnouncementStat>(DASHBOARD_STATS_API.announcementStat, {
            publishTimeStart: range.start,
            publishTimeEnd: range.end,
          })
          return res ?? { ...EMPTY_ANNOUNCEMENT }
        },
        { ...EMPTY_ANNOUNCEMENT },
      ),
    ])
    meetingStat.value = meeting
    announcementStat.value = announcement
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  scheduleDashboardLoad(loadData)
})

useTableRefresh(() => {
  scheduleDashboardLoad(loadData)
})
</script>

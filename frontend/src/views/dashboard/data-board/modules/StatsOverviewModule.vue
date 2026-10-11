<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsOverviewModule.vue -->
<!-- 功能描述：数据看板统计概览（著名色 Popover：待办/消息/会议/通知/帮助台/在线） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <StatsMetricGrid
    use-popover
    card-variant="dashboard"
    :loading="loading"
    :items="metricItems"
    :col-xs="12"
    :col-sm="12"
    :col-md="8"
    :col-lg="4"
    @popover-open-change="handlePopoverOpenChange"
  >
    <template #popover="{ item }">
      <StatsMetricPopoverPanel
        :loading="!!popoverLoading[item.key]"
        :rows="popoverPreview[item.key]?.rows ?? []"
        :total="popoverPreview[item.key]?.total ?? 0"
        :empty-text="resolvePopoverEmptyText(item.key)"
        :view-all-text="t('dashboard.workspace.page.viewall')"
        :show-view-all="isMetricNavigable(item)"
        :accent-color="item.accentColor"
        @view-all="goOverviewRoute(item.key)"
      />
    </template>
  </StatsMetricGrid>
</template>

<script setup lang="ts">
/**
 * 统计概览：六项指标著名色 Popover 预览 + 查看全部跳转
 */
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import type { StatsMetricItem } from '../components/StatsMetricGrid.vue'
import StatsMetricPopoverPanel from '../components/StatsMetricPopoverPanel.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type { MessageStatistics } from '@/types/foundation/message.types'
import type { OnlineDashboardStatistics } from '@/types/foundation/online.types'
import {
  loadOverviewPopoverPreview,
  type StatsOverviewPopoverPreview,
} from '../utils/statsOverviewPopover'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  fetchDashboardPagedTotal,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

const router = useRouter()
const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** Popover 预览 loading（按指标键） */
const popoverLoading = ref<Record<string, boolean>>({})
/** Popover 预览缓存（按指标键） */
const popoverPreview = ref<Record<string, StatsOverviewPopoverPreview>>({})
/** 概览指标 */
const overview = ref({
  todoCount: 0,
  messageCount: 0,
  meetingCount: 0,
  notificationCount: 0,
  helpDeskCount: 0,
  onlineUsers: 0,
})

/**
 * 解析概览指标跳转路由（与加载权限及菜单 RoutePath 对齐）
 * @param key 指标键
 * @returns {string | undefined} 目标路由
 */
function resolveOverviewRoutePath(key: string): string | undefined {
  switch (key) {
    case 'todo':
      return permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.flowTodoList)
        ? DASHBOARD_STATS_ROUTE.flowTodo
        : undefined
    case 'message':
      return permissionStore.hasAny([
        DASHBOARD_STATS_PERMISSION.messageStatistics,
        DASHBOARD_STATS_PERMISSION.messageUnreadList,
      ])
        ? DASHBOARD_STATS_ROUTE.message
        : undefined
    case 'meeting':
      return permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.meetingList)
        ? DASHBOARD_STATS_ROUTE.meeting
        : undefined
    case 'notification':
      if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.announcementList)) {
        return DASHBOARD_STATS_ROUTE.announcement
      }
      if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.meetingNotificationList)) {
        return DASHBOARD_STATS_ROUTE.meeting
      }
      return undefined
    case 'helpdesk':
      if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskTicketList)) {
        return DASHBOARD_STATS_ROUTE.helpDeskTicket
      }
      if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskMyTicketList)) {
        return DASHBOARD_STATS_ROUTE.helpDeskMyTicket
      }
      return undefined
    case 'online':
      return permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard)
        ? DASHBOARD_STATS_ROUTE.online
        : undefined
    default:
      return undefined
  }
}

/**
 * Popover 空态文案
 * @param key 指标键
 * @returns {string} 空态文案
 */
function resolvePopoverEmptyText(key: string): string {
  const map: Record<string, string> = {
    todo: 'dashboard.workspace.page.todoplaceholder',
    message: 'dashboard.data-board.page.overview.popover.messageempty',
    meeting: 'dashboard.data-board.page.overview.popover.meetingempty',
    notification: 'dashboard.workspace.page.noticeplaceholder',
    helpdesk: 'dashboard.data-board.page.overview.popover.helpdeskempty',
    online: 'dashboard.data-board.page.overview.popover.onlineempty',
  }
  return t(map[key] ?? 'dashboard.data-board.page.overview.popover.empty')
}

/**
 * 指标是否可跳转
 * @param item 指标项
 * @returns {boolean} 是否可导航
 */
function isMetricNavigable(item: StatsMetricItem): boolean {
  const path = item.routePath?.trim()
  if (!path) {
    return false
  }
  return permissionStore.canAccess(path)
}

/** a-statistic 指标（dashboard KPI 卡） */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'todo',
      title: t('dashboard.data-board.page.overview.todo'),
      value: overview.value.todoCount,
      routePath: resolveOverviewRoutePath('todo'),
    },
    {
      key: 'message',
      title: t('dashboard.data-board.page.overview.message'),
      value: overview.value.messageCount,
      routePath: resolveOverviewRoutePath('message'),
    },
    {
      key: 'meeting',
      title: t('dashboard.data-board.page.overview.meeting'),
      value: overview.value.meetingCount,
      routePath: resolveOverviewRoutePath('meeting'),
    },
    {
      key: 'notification',
      title: t('dashboard.data-board.page.overview.notification'),
      value: overview.value.notificationCount,
      routePath: resolveOverviewRoutePath('notification'),
    },
    {
      key: 'helpdesk',
      title: t('dashboard.data-board.page.overview.helpdesk'),
      value: overview.value.helpDeskCount,
      routePath: resolveOverviewRoutePath('helpdesk'),
    },
    {
      key: 'online',
      title: t('dashboard.data-board.page.overview.online'),
      value: overview.value.onlineUsers,
      routePath: resolveOverviewRoutePath('online'),
    },
  ]),
)

/**
 * 清空 Popover 预览缓存
 * @returns {void}
 */
function clearPopoverPreviewCache(): void {
  popoverPreview.value = {}
}

/**
 * 打开 Popover 时懒加载预览
 * @param key 指标键
 * @param open 是否打开
 * @returns {Promise<void>}
 */
async function handlePopoverOpenChange(key: string, open: boolean): Promise<void> {
  if (!open || popoverPreview.value[key]) {
    return
  }
  popoverLoading.value[key] = true
  try {
    popoverPreview.value[key] = await loadOverviewPopoverPreview(
      key,
      permissionStore.hasPermission.bind(permissionStore),
      permissionStore.hasAny.bind(permissionStore),
    )
  } finally {
    popoverLoading.value[key] = false
  }
}

/**
 * 跳转概览指标关联视图
 * @param key 指标键
 * @returns {void}
 */
function goOverviewRoute(key: string): void {
  const path = resolveOverviewRoutePath(key)?.trim()
  if (!path || !permissionStore.canAccess(path)) {
    return
  }
  void router.push(path)
}

/**
 * 加载未读消息数
 * @returns {Promise<number>} 未读条数
 */
async function loadMessageCount(): Promise<number> {
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.messageStatistics)) {
    const stats = await fetchDashboardGet<MessageStatistics>(DASHBOARD_STATS_API.messageStatistics)
    if (stats) {
      return stats.unreadCount ?? 0
    }
  }
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.messageUnreadList)) {
    return fetchDashboardPagedTotal(DASHBOARD_STATS_API.messageUnreadList)
  }
  return 0
}

/**
 * 加载公告通知数（无公告 list 权限时回退会议通知 list）
 * @returns {Promise<number>} 通知条数
 */
async function loadNotificationCount(): Promise<number> {
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.announcementList)) {
    return fetchDashboardPagedTotal(DASHBOARD_STATS_API.announcementList)
  }
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.meetingNotificationList)) {
    return fetchDashboardPagedTotal(DASHBOARD_STATS_API.meetingNotificationList)
  }
  return 0
}

/**
 * 加载服务台工单数
 * @returns {Promise<number>} 工单条数
 */
async function loadHelpDeskCount(): Promise<number> {
  if (permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskTicketList)) {
    return fetchDashboardPagedTotal(DASHBOARD_STATS_API.helpDeskTicketList)
  }
  return 0
}

/**
 * 加载概览六项指标
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  loading.value = true
  clearPopoverPreviewCache()
  try {
    const [
      todoCount,
      messageCount,
      meetingCount,
      notificationCount,
      helpDeskCount,
      onlineUsers,
    ] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.flowTodoList),
        'todo',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.flowTodoList),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasAny([
          DASHBOARD_STATS_PERMISSION.messageStatistics,
          DASHBOARD_STATS_PERMISSION.messageUnreadList,
        ]),
        'message',
        loadMessageCount,
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.meetingList),
        'meeting',
        () => fetchDashboardPagedTotal(DASHBOARD_STATS_API.meetingList),
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasAny([
          DASHBOARD_STATS_PERMISSION.announcementList,
          DASHBOARD_STATS_PERMISSION.meetingNotificationList,
        ]),
        'notification',
        loadNotificationCount,
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.helpDeskTicketList),
        'helpdesk',
        loadHelpDeskCount,
        0,
      ),
      fetchDashboardMetricIfPermitted(
        permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.onlineDashboard),
        'online',
        async () => {
          const stats = await fetchDashboardGet<OnlineDashboardStatistics>(DASHBOARD_STATS_API.onlineDashboard)
          return stats?.onlineUserCount ?? 0
        },
        0,
      ),
    ])
    overview.value = {
      todoCount,
      messageCount,
      meetingCount,
      notificationCount,
      helpDeskCount,
      onlineUsers,
    }
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

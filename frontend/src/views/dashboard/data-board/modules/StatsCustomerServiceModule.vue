<!-- ======================================== -->
<!-- 项目名称：节拍智控·TaktWise -->
<!-- 命名空间：frontend/src/views/dashboard/dataBoard.types/modules -->
<!-- 文件名称：StatsCustomerServiceModule.vue -->
<!-- 功能描述：数据看板客户服务统计（请求/订单/工单/合同本月数量） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="min-h-[80px]">
    <p class="mb-3 text-base font-medium leading-relaxed text-text">
      {{ t('dashboard.data-board.page.customerservice.summaryTitle') }}
    </p>
    <StatsMetricGrid
    card-variant="dashboard-compact"
    :loading="loading"
    :period-label="t('dashboard.data-board.page.periodlastmonth')"
    :items="metricItems"
    :col-xs="12"
    :col-sm="12"
    :col-md="8"
    :col-lg="4"
  />
  </div>
</template>

<script setup lang="ts">
/**
 * 客户服务统计：request/order/ticket/contract *-stat
 */
import { useI18n } from 'vue-i18n'
import StatsMetricGrid from '../components/StatsMetricGrid.vue'
import { usePermissionStore } from '@/stores/identity/usePermissionStore'
import type {
  ServiceContractStat,
  ServiceOrderStat,
  ServiceRequestStat,
  ServiceTicketStat,
} from '@/types/logistics/customer-service/stat.types'
import { enrichDashboardMetricItems } from '../utils/statsMetricDashboard'
import {
  DASHBOARD_STATS_API,
  DASHBOARD_STATS_PERMISSION,
  DASHBOARD_STATS_ROUTE,
  fetchDashboardGet,
  fetchDashboardMetricIfPermitted,
  getLastMonthRange,
  scheduleDashboardLoad,
} from '../utils/statsQuery'

/** 空请求统计 */
const EMPTY_REQUEST_STAT: ServiceRequestStat = { statMonth: '', monthRequestCount: 0 }
/** 空订单统计 */
const EMPTY_ORDER_STAT: ServiceOrderStat = { statMonth: '', monthOrderCount: 0, monthTotalAmount: 0 }
/** 空工单统计 */
const EMPTY_TICKET_STAT: ServiceTicketStat = {
  statMonth: '',
  monthTicketCount: 0,
  monthOpenTicketCount: 0,
  monthClosedTicketCount: 0,
}
/** 空合同统计 */
const EMPTY_CONTRACT_STAT: ServiceContractStat = {
  statMonth: '',
  monthContractCount: 0,
  monthContractAmount: 0,
}

const { t } = useI18n()
const permissionStore = usePermissionStore()

/** 列表 loading */
const loading = ref(false)
/** 请求统计 */
const requestStat = ref<ServiceRequestStat>({ ...EMPTY_REQUEST_STAT })
/** 订单统计 */
const orderStat = ref<ServiceOrderStat>({ ...EMPTY_ORDER_STAT })
/** 工单统计 */
const ticketStat = ref<ServiceTicketStat>({ ...EMPTY_TICKET_STAT })
/** 合同统计 */
const contractStat = ref<ServiceContractStat>({ ...EMPTY_CONTRACT_STAT })

/** dashboard KPI 指标 */
const metricItems = computed(() =>
  enrichDashboardMetricItems([
    {
      key: 'requests',
      title: t('dashboard.data-board.page.customerservice.requests'),
      value: requestStat.value.monthRequestCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceRequest,
    },
    {
      key: 'orders',
      title: t('dashboard.data-board.page.customerservice.orders'),
      value: orderStat.value.monthOrderCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceOrder,
    },
    {
      key: 'tickets',
      title: t('dashboard.data-board.page.customerservice.tickets'),
      value: ticketStat.value.monthTicketCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceTicket,
    },
    {
      key: 'openTickets',
      title: t('dashboard.data-board.page.customerservice.opentickets'),
      value: ticketStat.value.monthOpenTicketCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceTicket,
    },
    {
      key: 'closedTickets',
      title: t('dashboard.data-board.page.customerservice.closedtickets'),
      value: ticketStat.value.monthClosedTicketCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceTicket,
    },
    {
      key: 'contracts',
      title: t('dashboard.data-board.page.customerservice.contracts'),
      value: contractStat.value.monthContractCount,
      routePath: DASHBOARD_STATS_ROUTE.serviceContract,
    },
  ]),
)

/**
 * 加载单项客户服务统计
 * @param path API 路径
 * @param dateQuery 日期查询字段
 * @returns {Promise<T | null>} 统计
 */
async function loadServiceStat<T>(
  path: string,
  dateQuery: Record<string, string>,
): Promise<T | null> {
  return fetchDashboardGet<T>(path, dateQuery)
}

/**
 * 加载本月客户服务统计
 * @returns {Promise<void>}
 */
async function loadData(): Promise<void> {
  const month = getLastMonthRange()
  const canRequest = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.serviceRequestStat)
  const canOrder = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.serviceOrderStat)
  const canTicket = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.serviceTicketStat)
  const canContract = permissionStore.hasPermission(DASHBOARD_STATS_PERMISSION.serviceContractStat)
  loading.value = true
  try {
    const [request, order, ticket, contract] = await Promise.all([
      fetchDashboardMetricIfPermitted(
        canRequest,
        'serviceRequestStat',
        () => loadServiceStat<ServiceRequestStat>(DASHBOARD_STATS_API.serviceRequestStat, {
          requestDateStart: month.start,
          requestDateEnd: month.end,
        }).then((res) => res ?? { ...EMPTY_REQUEST_STAT }),
        { ...EMPTY_REQUEST_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canOrder,
        'serviceOrderStat',
        () => loadServiceStat<ServiceOrderStat>(DASHBOARD_STATS_API.serviceOrderStat, {
          orderDateStart: month.start,
          orderDateEnd: month.end,
        }).then((res) => res ?? { ...EMPTY_ORDER_STAT }),
        { ...EMPTY_ORDER_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canTicket,
        'serviceTicketStat',
        () => loadServiceStat<ServiceTicketStat>(DASHBOARD_STATS_API.serviceTicketStat, {
          createdAtStart: month.start,
          createdAtEnd: month.end,
        }).then((res) => res ?? { ...EMPTY_TICKET_STAT }),
        { ...EMPTY_TICKET_STAT },
      ),
      fetchDashboardMetricIfPermitted(
        canContract,
        'serviceContractStat',
        () => loadServiceStat<ServiceContractStat>(DASHBOARD_STATS_API.serviceContractStat, {
          effectiveDateStart: month.start,
          effectiveDateEnd: month.end,
        }).then((res) => res ?? { ...EMPTY_CONTRACT_STAT }),
        { ...EMPTY_CONTRACT_STAT },
      ),
    ])
    requestStat.value = request
    orderStat.value = order
    ticketStat.value = ticket
    contractStat.value = contract
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-monthly-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：月设变推移（工厂×设变号×部门×月份转置表） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 flex-col p-4">
    <EcMonthlyTrendQueryForm
      v-model:plant-code="plantCode"
      v-model:period-range="periodRange"
      v-model:ec-code="ecCode"
      v-model:ec-scope="ecScope"
      v-model:change-status="changeStatus"
      v-model:ec-status="ecStatus"
      v-model:dept-code="deptCode"
      :active-tab="activeTab"
      :loading="panelLoading"
      @search="handleSearch"
      @reset="handleReset"
    />
    <a-tabs
      v-model:activeKey="activeTab"
      class="ec-monthly-trend-tabs mb-1 shrink-0"
    >
      <a-tab-pane
        key="issue"
        :tab="t(`${localePrefix}.tabs.issue`)"
      />
      <a-tab-pane
        key="implement"
        :tab="t(`${localePrefix}.tabs.implement`)"
      />
    </a-tabs>
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="false"
      :show-column-setting="false"
      :show-fullscreen="false"
      :show-refresh="true"
      :export-disabled="!plantCode?.trim() || !hasRows"
      :export-loading="exportLoading"
      :refresh-loading="panelLoading"
      :right-actions="trendFilterActions"
      export-permission="logistics:manufacturing:engineering:change:monthly:trend:export"
      @export="handleExport"
      @refresh="handleRefresh"
    />
    <EcMonthlyTrendPanel
      ref="panelRef"
      v-model:loading="panelLoading"
      v-model:has-rows="hasRows"
      class="min-h-0 flex-1"
      :trend-filter="trendFilter"
      :active-tab="activeTab"
      :plant-code="plantCode"
      :period-range="periodRange"
      :ec-code="ecCode"
      :ec-scope="ecScope"
      :change-status="changeStatus"
      :ec-status="ecStatus"
      :dept-code="deptCode"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 月设变推移转置分析
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'
import {
  RiArrowDownLine,
  RiArrowUpDownLine,
  RiArrowUpLine,
  RiListCheck,
} from '@remixicon/vue'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import EcMonthlyTrendQueryForm from './components/EcMonthlyTrendQueryForm.vue'
import EcMonthlyTrendPanel from './components/EcMonthlyTrendPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.engineering-change.ec-monthly-trend.page'
const tenantStore = useTenantStore()

/** 当前 Tab：issue=月设变推移；implement=月实施推移 */
const activeTab = ref<'issue' | 'implement'>('issue')
/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)

/** 默认工厂=公司 RelatedPlant；期间=前三月（对齐成本差异推移） */
const { applyDefaultPeriodRange, applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 设变单号 */
const ecCode = ref<string | undefined>()
/** 区分 */
const ecScope = ref<number | undefined>()
/** 变更状态 */
const changeStatus = ref<number | undefined>()
/** 设变状态 */
const ecStatus = ref<number | undefined>()
/** 部门编码（设变推移 / 实施推移） */
const deptCode = ref<string | undefined>()
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 涨跌筛选 */
const trendFilter = ref('')
/** 右侧涨跌筛选：仅图标 + tooltip */
const trendFilterActions = computed<ToolBarAction[]>(() => [
  {
    key: 'trend-all',
    icon: RiListCheck,
    tooltip: t(`${localePrefix}.filter.all`),
    active: trendFilter.value === '',
    onClick: () => setTrendFilter(''),
  },
  {
    key: 'trend-changed',
    icon: RiArrowUpDownLine,
    tooltip: t(`${localePrefix}.filter.changed`),
    active: trendFilter.value === 'changed',
    onClick: () => setTrendFilter('changed'),
  },
  {
    key: 'trend-up',
    icon: RiArrowUpLine,
    tooltip: t(`${localePrefix}.trend.up`),
    active: trendFilter.value === 'up',
    onClick: () => setTrendFilter('up'),
  },
  {
    key: 'trend-down',
    icon: RiArrowDownLine,
    tooltip: t(`${localePrefix}.trend.down`),
    active: trendFilter.value === 'down',
    onClick: () => setTrendFilter('down'),
  }])
/** 明细面板 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | null>(null)

/** 查询 */
function handleSearch() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!periodRange.value?.[0]) {
    message.warning(t(`${localePrefix}.selectPeriodRequired`))
    return
  }
  if (!assertPeriodRangeOk(periodRange.value)) {
    return
  }
  void panelRef.value?.reload?.()
}

/** 涨跌筛选 */
function setTrendFilter(value: string) {
  if (trendFilter.value === value) {
    return
  }
  trendFilter.value = value
}

/** 刷新 */
function handleRefresh() {
  void panelRef.value?.reload?.()
}


/** 清空工厂级联与结果 */
function clearPlantCascade() {
  deptCode.value = undefined
  ecCode.value = undefined
  hasRows.value = false
  panelRef.value?.clear?.()
}

/** 重置 */
async function handleReset() {
  clearPlantCascade()
  ecScope.value = undefined
  changeStatus.value = undefined
  ecStatus.value = undefined
  await applyAnalysisQueryDefaults()
  trendFilter.value = ''
}

/** 导出 */
async function handleExport() {
  if (!plantCode.value?.trim()) {
    message.warning(t(`${localePrefix}.selectPlantRequired`))
    return
  }
  if (!hasRows.value) {
    message.warning(t(`${localePrefix}.exportEmpty`))
    return
  }
  exportLoading.value = true
  try {
    await panelRef.value?.handleExport?.()
  } finally {
    exportLoading.value = false
  }
}

watch(
  () => tenantStore.companyCode,
  () => {
    clearPlantCascade()
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  await applyAnalysisQueryDefaults()
  void getTaktDefaultPageSize()
})
</script>

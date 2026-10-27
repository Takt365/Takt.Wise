<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/defect-monthly -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：不良推移（组立、PCBA 两个页签） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-col p-4">
    <a-tabs
      v-model:activeKey="statTab"
      class="defect-trend-tabs flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content]:w-full [&_.ant-tabs-content]:min-w-0 [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:w-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:min-w-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content-holder]:flex-col [&_.ant-tabs-tabpane]:flex [&_.ant-tabs-tabpane]:h-full [&_.ant-tabs-tabpane]:w-full [&_.ant-tabs-tabpane]:min-h-0 [&_.ant-tabs-tabpane]:min-w-0 [&_.ant-tabs-tabpane]:flex-col"
      @change="handleStatTabChange"
    >
      <a-tab-pane
        v-for="tab in statTabs"
        :key="tab.key"
        :tab="t(`${localePrefix}.tabs.${tab.label}`)"
        class="h-full min-h-0 min-w-0"
      >
        <div
          v-if="statTab === tab.key"
          class="flex h-full min-h-0 min-w-0 flex-col"
        >
          <DefectMonthlyQueryForm
            v-model:plant-code="plantCode"
            v-model:period-range="periodRange"
            v-model:model-code="modelCode"
            :stat-tab="tab.key"
            :loading="panelLoading"
            @search="handleSearch"
            @reset="handleReset"
          />
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
            export-permission="logistics:manufacturing:defect:monthly:export"
            @export="handleExport"
            @refresh="handleRefresh"
          />
          <DefectMonthlyPanel
            ref="panelRef"
            v-model:loading="panelLoading"
            v-model:has-rows="hasRows"
            class="min-h-0 min-w-0 flex-1"
            :trend-filter="trendFilter"
            :plant-code="plantCode"
            :period-range="periodRange"
            :model-code="modelCode"
            :defect-category="tab.key"
          />
        </div>
      </a-tab-pane>
    </a-tabs>
  </div>
</template>

<script setup lang="ts">
/**
 * 不良推移：组立与 PCBA 分开按机种汇总月份不良率
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
import DefectMonthlyQueryForm from './components/DefectMonthlyQueryForm.vue'
import DefectMonthlyPanel from './components/DefectMonthlyPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.defect.defect-monthly.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)

/** 默认工厂=公司 RelatedPlant；期间=前三月（对齐成本差异推移） */
const { applyDefaultPeriodRange, applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 页签（每个页签自带查询、工具栏和表格） */
const statTabs = [
  { key: 'assy', label: 'assy' },
  { key: 'pcba', label: 'pcba' },
] as const
/** 当前页签 */
const statTab = ref<string>(statTabs[0].key)
/** 机种 */
const modelCode = ref<string | undefined>()
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
/** 明细面板。写在 v-for 里时 Vue 会收成数组 */
const panelRef = ref<DefectMonthlyPanelExpose | DefectMonthlyPanelExpose[] | null>(null)

interface DefectMonthlyPanelExpose {
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
}

/**
 * 当前页签里的表格
 * @returns 面板实例
 */
function activePanel() {
  const value = panelRef.value
  if (Array.isArray(value)) {
    return value.find((item) => item != null) ?? null
  }
  return value
}

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
  void activePanel()?.reload?.()
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
  void activePanel()?.reload?.()
}

/** 切换页签后清空机种，等重新查询 */
function handleStatTabChange() {
  modelCode.value = undefined
  hasRows.value = false
  activePanel()?.clear?.()
}

/** 清空机种与结果 */
function clearPlantCascade() {
  modelCode.value = undefined
  hasRows.value = false
  activePanel()?.clear?.()
}

/** 重置 */
async function handleReset() {
  clearPlantCascade()
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
    await activePanel()?.handleExport?.()
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

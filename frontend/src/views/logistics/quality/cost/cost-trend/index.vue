<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/cost-trend -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：质量成本推移（品质保证、品质问题、品质事故） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-col p-4">
    <a-tabs
      v-model:active-key="statTab"
      class="cost-trend-tabs flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content]:w-full [&_.ant-tabs-content]:min-w-0 [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:w-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:min-w-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content-holder]:flex-col [&_.ant-tabs-tabpane]:flex [&_.ant-tabs-tabpane]:h-full [&_.ant-tabs-tabpane]:w-full [&_.ant-tabs-tabpane]:min-h-0 [&_.ant-tabs-tabpane]:min-w-0 [&_.ant-tabs-tabpane]:flex-col"
      @change="handleStatTabChange"
    >
      <a-tab-pane
        v-for="tab in statTabs"
        :key="tab.key"
        :tab="t(`${localePrefix}.costCategoryOptions.${tab.label}`)"
      >
        <div
          v-if="statTab === tab.key"
          class="flex h-full min-h-0 min-w-0 flex-col"
        >
          <CostTrendQueryForm
            v-model:plant-code="plantCode"
            v-model:period-range="periodRange"
            v-model:currency-code="currencyCode"
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
            :show-advanced-query="true"
            :show-column-setting="true"
            :show-fullscreen="false"
            :show-refresh="true"
            :export-disabled="!plantCode?.trim() || !hasRows"
            :export-loading="exportLoading"
            :refresh-loading="panelLoading"
            :right-actions="trendFilterActions"
            export-permission="logistics:quality:cost:trend:export"
            @export="handleExport"
            @refresh="handleRefresh"
            @advanced-query="advancedQueryVisible = true"
            @column-setting="columnSettingOpen = true"
          />
          <CostTrendPanel
            ref="panelRef"
            v-model:loading="panelLoading"
            v-model:has-rows="hasRows"
            v-model:column-setting-open="columnSettingOpen"
            class="min-h-0 min-w-0 flex-1"
            :trend-filter="trendFilter"
            :plant-code="plantCode"
            :period-range="periodRange"
            :cost-category="tab.key"
            :currency-code="currencyCode"
            :hidden-column-keys="hiddenColumnKeysByTab[tab.key]"
            @update:hidden-column-keys="handleHiddenColumnKeysChange"
          />
        </div>
      </a-tab-pane>
    </a-tabs>
    <TaktQueryDrawer
      :key="statTab"
      v-model:open="advancedQueryVisible"
      :title="t('common.page.advancedQuery.title')"
      :fields="advancedQueryFields"
      :default-visible-count="3"
      :storage-key="`takt-query-fields-quality-cost-trend-${statTab}`"
      @submit="handleAdvancedSubmit"
      @reset="handleReset"
    >
      <template #default="{ isFieldVisible }">
        <a-form-item
          v-show="isFieldVisible('periodRange')"
          :label="t(`${localePrefix}.periodRange`)"
        >
          <a-range-picker
            v-model:value="periodRange"
            picker="month"
            format="YYYY-MM"
            value-format="YYYY-MM"
            class="w-full"
            :placeholder="[t(`${localePrefix}.periodRange`), t(`${localePrefix}.periodRange`)]"
            :disabled-date="disabledDate"
            @calendar-change="onCalendarChange"
            @open-change="onOpenChange"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('currencyCode')"
          :label="t(`${localePrefix}.currencyCode`)"
        >
          <TaktSelect
            v-model:value="currencyCode"
            dict-type="accounting_financial_currency_code"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
      </template>
    </TaktQueryDrawer>
  </div>
</template>

<script setup lang="ts">
/**
 * 质量成本推移：品质保证、品质问题、品质事故三个页签
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
import { resolveCurrentCompanyRelatedPlantCode } from '@/composables/useCompanyRelatedPlant'
import { useTenantStore } from '@/stores/identity/useTenantStore'
import { buildDefaultCostingPeriodRange } from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod'
import CostTrendQueryForm from './components/CostTrendQueryForm.vue'
import CostTrendPanel from './components/CostTrendPanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

const { t } = useI18n()

/** 推移表面板方法 */
interface CostTrendPanelExpose {
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
}

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk, disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()
/** 三个成本类别页签 */
const statTabs = [
  { key: 'assurance', label: 'assurance' },
  { key: 'issue', label: 'issue' },
  { key: 'incident', label: 'incident' },
] as const
/** 当前页签，同时作为成本类别 */
const statTab = ref<(typeof statTabs)[number]['key']>('assurance')
/** 静态 locales 前缀 */
const localePrefix = 'logistics.quality.cost.cost-trend.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)
/** 成本币种 */
const currencyCode = ref<string | undefined>()
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
/** 高级查询 */
const advancedQueryVisible = ref(false)
/** 列设置 */
const columnSettingOpen = ref(false)
/** 各页签隐藏列 */
const hiddenColumnKeysByTab = reactive<Record<string, string[]>>({})
/** 高级查询字段 */
const advancedQueryFields = computed(() => [
  { key: 'periodRange', label: t(`${localePrefix}.periodRange`) },
  { key: 'currencyCode', label: t(`${localePrefix}.currencyCode`) },
])
/** 明细面板。v-if 下 ref 可能是数组 */
const panelRef = ref<CostTrendPanelExpose | CostTrendPanelExpose[] | null>(null)

/** 当前页签面板 */
function activePanel(): CostTrendPanelExpose | null {
  const value = panelRef.value
  if (Array.isArray(value)) {
    return value[0] ?? null
  }
  return value
}

/**
 * 保存当前页签隐藏列
 * @param {string[]} keys 隐藏列键
 */
function handleHiddenColumnKeysChange(keys: string[]) {
  hiddenColumnKeysByTab[statTab.value] = keys
}

/** 切换页签后清空当前表，避免沿用上一类别的行 */
function handleStatTabChange() {
  hasRows.value = false
  columnSettingOpen.value = false
  advancedQueryVisible.value = false
  activePanel()?.clear?.()
}

/** 高级查询提交 */
function handleAdvancedSubmit() {
  advancedQueryVisible.value = false
  handleSearch()
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

/** 默认核算年月 */
function applyDefaultPeriodRange() {
  periodRange.value = buildDefaultCostingPeriodRange(3)
}

/** 默认工厂 */
async function applyDefaultPlantFromCompany(): Promise<void> {
  const plant = await resolveCurrentCompanyRelatedPlantCode()
  plantCode.value = plant || undefined
}

/** 重置 */
async function handleReset() {
  await applyDefaultPlantFromCompany()
  currencyCode.value = undefined
  applyDefaultPeriodRange()
  trendFilter.value = ''
  hasRows.value = false
  activePanel()?.clear?.()
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
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  applyDefaultPeriodRange()
  await applyDefaultPlantFromCompany()
  void getTaktDefaultPageSize()
})
</script>

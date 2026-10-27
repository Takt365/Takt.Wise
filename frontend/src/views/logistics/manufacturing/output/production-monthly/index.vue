<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：生产统计（组立按班组、组立按机种、PCBA按班组、PCBA按机种的日报明细列） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="flex h-full min-h-0 min-w-0 flex-col p-4">
    <a-tabs
      v-model:activeKey="statTab"
      class="production-stat-tabs flex min-h-0 min-w-0 flex-1 flex-col overflow-hidden [&_.ant-tabs-content]:h-full [&_.ant-tabs-content]:w-full [&_.ant-tabs-content]:min-w-0 [&_.ant-tabs-content-holder]:flex [&_.ant-tabs-content-holder]:w-full [&_.ant-tabs-content-holder]:min-h-0 [&_.ant-tabs-content-holder]:min-w-0 [&_.ant-tabs-content-holder]:flex-1 [&_.ant-tabs-content-holder]:flex-col [&_.ant-tabs-tabpane]:flex [&_.ant-tabs-tabpane]:h-full [&_.ant-tabs-tabpane]:w-full [&_.ant-tabs-tabpane]:min-h-0 [&_.ant-tabs-tabpane]:min-w-0 [&_.ant-tabs-tabpane]:flex-col"
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
          <ProductionMonthlyQueryForm
            v-model:plant-code="plantCode"
            v-model:period-range="periodRange"
            v-model:team-code="teamCode"
            v-model:model-code="modelCode"
            v-model:material-code="materialCode"
            v-model:prod-category="prodCategory"
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
            :show-advanced-query="true"
            :show-column-setting="true"
            :show-fullscreen="false"
            :show-refresh="true"
            :export-disabled="!plantCode?.trim() || !hasRows"
            :export-loading="exportLoading"
            :refresh-loading="panelLoading"
            export-permission="logistics:manufacturing:output:production:monthly:export"
            @export="handleExport"
            @advanced-query="advancedQueryVisible = true"
            @column-setting="columnSettingOpen = true"
            @refresh="handleRefresh"
          />
          <ProductionStatLinePanel
            ref="panelRef"
            v-model:loading="panelLoading"
            v-model:has-rows="hasRows"
            class="min-h-0 min-w-0 flex-1"
            :stat-tab="tab.key"
            v-model:column-setting-open="columnSettingOpen"
            :saved-visible-keys="visibleColumnKeysByTab[tab.key]"
            @update:saved-visible-keys="(keys: string[]) => { visibleColumnKeysByTab[tab.key] = keys }"
            :plant-code="plantCode"
            :period-range="periodRange"
            :team-code="teamCode"
            :model-code="modelCode"
            :material-code="materialCode"
            :prod-category="prodCategory"
          />
        </div>
      </a-tab-pane>
    </a-tabs>
    <TaktQueryDrawer
      :key="statTab"
      v-model:open="advancedQueryVisible"
      :fields="queryFieldsMeta"
      :default-visible-count="6"
      :storage-key="`takt-query-fields-production-stat-${statTab}`"
      :submit-loading="panelLoading"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
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
            :disabled-date="disabledDate"
            @calendar-change="onCalendarChange"
            @open-change="onOpenChange"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('prodCategory')"
          :label="t(`${localePrefix}.prodCategory`)"
        >
          <TaktSelect
            v-model:value="prodCategory"
            dict-type="logistics_manufacturing_prod_category"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('teamCode')"
          :label="t(`${localePrefix}.teamCode`)"
        >
          <TaktSelect
            v-model:value="teamCode"
            :api-url="teamOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('modelCode')"
          :label="t(`${localePrefix}.modelCode`)"
        >
          <TaktSelect
            v-model:value="modelCode"
            :api-url="modelOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </a-form-item>
        <a-form-item
          v-show="isFieldVisible('materialCode')"
          :label="t(`${localePrefix}.materialCode`)"
        >
          <TaktSelect
            v-model:value="materialCode"
            :api-url="materialOptionsUrl"
            :api-params="scopedApiParams"
            :disabled="!plantCode?.trim()"
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
 * 生产统计：四个页签列出组立日报或 PCBA 明细，列随页签变化
 */
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import {
  getProductionMonthlyTrendMaterialOptionsUrl,
  getProductionMonthlyTrendModelOptionsUrl,
  getProductionMonthlyTrendPlantOptionsUrl,
  getProductionMonthlyTrendTeamOptionsUrl,
} from '@/api/logistics/manufacturing/output/productionMonthly.api'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageSize } from '@/utils/taktPaged'
import ProductionMonthlyQueryForm from './components/ProductionMonthlyQueryForm.vue'
import ProductionStatLinePanel from './components/ProductionStatLinePanel.vue'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'
import { useAnalysisQueryDefaults } from '@/composables/useAnalysisQueryDefaults'
import { useTenantStore } from '@/stores/identity/useTenantStore'

const { t } = useI18n()

/** 期间最多 12 个月（查询前校验） */
const { assertPeriodRangeOk, disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.output.production-monthly.page'
const tenantStore = useTenantStore()

/** 工厂 */
const plantCode = ref<string | undefined>()
/** 期间年月 */
const periodRange = ref<[string, string] | null>(null)

/** 默认工厂=公司 RelatedPlant；期间=前三月（对齐成本差异推移） */
const { applyDefaultPeriodRange, applyDefaultPlantFromCompany, applyAnalysisQueryDefaults } = useAnalysisQueryDefaults(plantCode, periodRange)
/** 页签（每个页签自带查询、工具栏和表格） */
const statTabs = [
  { key: 'assy-team', label: 'assyTeam' },
  { key: 'assy-model', label: 'assyModel' },
  { key: 'pcba-team', label: 'pcbaTeam' },
  { key: 'pcba-model', label: 'pcbaModel' },
] as const
/** 当前页签 */
const statTab = ref<string>(statTabs[0].key)
/** 班组 */
const teamCode = ref<string | undefined>()
/** 机种 */
const modelCode = ref<string | undefined>()
/** 物料 */
const materialCode = ref<string | undefined>()
/** 生产类别 */
const prodCategory = ref<string | undefined>()
/** 明细面板 loading */
const panelLoading = ref(false)
/** 导出 loading */
const exportLoading = ref(false)
/** 是否有数据行 */
const hasRows = ref(false)
/** 高级查询 */
const advancedQueryVisible = ref(false)
/** 列设置抽屉 */
const columnSettingOpen = ref(false)
/** 各页签用户选择的可见列。空表示全部列 */
const visibleColumnKeysByTab = reactive<Record<string, string[]>>({})
/** 工厂选项 */
const plantOptionsUrl = getProductionMonthlyTrendPlantOptionsUrl()
/** 班组选项 */
const teamOptionsUrl = getProductionMonthlyTrendTeamOptionsUrl()
/** 机种选项 */
const modelOptionsUrl = getProductionMonthlyTrendModelOptionsUrl()
/** 物料选项 */
const materialOptionsUrl = getProductionMonthlyTrendMaterialOptionsUrl()
/** 工厂选项参数 */
const plantApiParams = computed(() => ({ statTab: statTab.value }))
/** 班组 / 机种 / 物料选项参数 */
const scopedApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { statTab: statTab.value, plantCode: plant }
})
/** 高级查询字段 */
const queryFieldsMeta = computed(() => [
  { key: 'periodRange', label: t(`${localePrefix}.periodRange`) },
  { key: 'prodCategory', label: t(`${localePrefix}.prodCategory`) },
  { key: 'teamCode', label: t(`${localePrefix}.teamCode`) },
  { key: 'modelCode', label: t(`${localePrefix}.modelCode`) },
  { key: 'materialCode', label: t(`${localePrefix}.materialCode`) },
])
/** 明细面板。写在 v-for 里时 Vue 会收成数组 */
const panelRef = ref<{
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
} | {
  reload?: () => Promise<void>
  handleExport?: () => Promise<void>
  clear?: () => void
}[] | null>(null)

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

/** 刷新 */
function handleRefresh() {
  void activePanel()?.reload?.()
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  handleSearch()
}

/** 高级查询清空选填条件 */
function handleAdvancedQueryReset() {
  teamCode.value = undefined
  modelCode.value = undefined
  materialCode.value = undefined
  prodCategory.value = undefined
}

/** 切换页签：清空选填条件，等用户重新查询 */
function handleStatTabChange() {
  teamCode.value = undefined
  modelCode.value = undefined
  materialCode.value = undefined
  prodCategory.value = undefined
  hasRows.value = false
  activePanel()?.clear?.()
}

/** 重置：清空选填条件，保留默认工厂与期间 */
async function handleReset() {
  teamCode.value = undefined
  modelCode.value = undefined
  materialCode.value = undefined
  prodCategory.value = undefined
  await applyAnalysisQueryDefaults()
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
    teamCode.value = undefined
    modelCode.value = undefined
    materialCode.value = undefined
    prodCategory.value = undefined
    hasRows.value = false
    activePanel()?.clear?.()
    void applyDefaultPlantFromCompany()
  },
)

onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  await applyAnalysisQueryDefaults()
  void getTaktDefaultPageSize()
})
</script>

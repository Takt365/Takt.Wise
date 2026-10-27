<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly/components -->
<!-- 文件名称：ProductionMonthlyQueryForm.vue -->
<!-- 功能描述：生产统计查询栏（工厂、期间必填；班组、机种、物料取自当前页签产出表） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar production-monthly-query-bar">
    <div class="production-monthly-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div
        v-if="!trendMode"
        class="flex items-center gap-1"
      >
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.prodCategory`) }}</span>
        <TaktSelect
          :key="`${props.statTab}-category`"
          v-model:value="prodCategory"
          dict-type="logistics_manufacturing_prod_category"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--model"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div
        v-if="!trendMode"
        class="flex items-center gap-1"
      >
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.teamCode`) }}</span>
        <TaktSelect
          :key="`${props.statTab}-team`"
          v-model:value="teamCode"
          :api-url="teamOptionsUrl"
          :api-params="scopedApiParams"
          :disabled="!plantCode?.trim()"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--model"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.modelCode`) }}</span>
        <TaktSelect
          :key="`${props.statTab}-model`"
          v-model:value="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="scopedApiParams"
          :disabled="!plantCode?.trim()"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--model"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div
        v-if="!trendMode"
        class="flex items-center gap-1"
      >
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.materialCode`) }}</span>
        <TaktSelect
          :key="`${props.statTab}-material`"
          v-model:value="materialCode"
          :api-url="materialOptionsUrl"
          :api-params="scopedApiParams"
          :disabled="!plantCode?.trim()"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--model"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
    </div>
    <a-space class="query-actions">
      <a-button
        class="takt-button-query"
        :loading="props.loading"
        @click="emit('search')"
      >
        <template #icon>
          <RiSearchLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.query') }}
      </a-button>
      <a-button
        class="takt-button-reset"
        :disabled="props.loading"
        @click="emit('reset')"
      >
        <template #icon>
          <RiRefreshLine class="takt-remix-icon" />
        </template>
        {{ t('common.page.button.reset') }}
      </a-button>
    </a-space>
  </div>
</template>

<script setup lang="ts">
/**
 * 生产统计查询栏：工厂、期间必填；班组、机种、物料按当前页签取自产出表
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getProductionMonthlyTrendMaterialOptionsUrl,
  getProductionMonthlyTrendModelOptionsUrl,
  getProductionMonthlyTrendPlantOptionsUrl,
  getProductionMonthlyTrendTeamOptionsUrl,
} from '@/api/logistics/manufacturing/output/productionMonthly.api'
import {
  getProductionTrendModelOptionsUrl,
  getProductionTrendPlantOptionsUrl,
} from '@/api/logistics/manufacturing/output/productionTrend.api'

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 班组（可空） */
const teamCode = defineModel<string | undefined>('teamCode')
/** 机种（可空） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 物料（可空） */
const materialCode = defineModel<string | undefined>('materialCode')
/** 生产类别（可空） */
const prodCategory = defineModel<string | undefined>('prodCategory')
const props = defineProps<{
  /** 当前页签 */
  statTab: string
  /** 生产推移只保留工厂、期间、机种 */
  trendMode?: boolean
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.output.production-monthly.page'
/** 当前页签产出表选项 URL */
const plantOptionsUrl = computed(() => props.trendMode
  ? getProductionTrendPlantOptionsUrl()
  : getProductionMonthlyTrendPlantOptionsUrl())
const teamOptionsUrl = getProductionMonthlyTrendTeamOptionsUrl()
const modelOptionsUrl = computed(() => props.trendMode
  ? getProductionTrendModelOptionsUrl()
  : getProductionMonthlyTrendModelOptionsUrl())
const materialOptionsUrl = getProductionMonthlyTrendMaterialOptionsUrl()

/** 工厂选项：只取当前页签的产出表 */
const plantApiParams = computed(() => ({ statTab: props.statTab }))

/** 班组 / 机种 / 物料：当前页签 + 工厂 */
const scopedApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { statTab: props.statTab, plantCode: plant }
})

/** 工厂变更：清空选填项 */
watch(
  () => plantCode.value,
  () => {
    teamCode.value = undefined
    modelCode.value = undefined
    materialCode.value = undefined
    prodCategory.value = undefined
  },
)
</script>

<style scoped>
.takt-query-bar {
  margin: 4px;
  padding: 4px;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  box-sizing: border-box;
}

.production-monthly-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.production-monthly-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.production-monthly-query-bar__control--model {
  width: 12rem;
  min-width: 9rem;
}

.query-actions {
  flex-shrink: 0;
}

.query-actions :deep(.ant-btn) {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.query-actions :deep(.ant-btn .anticon) {
  margin-inline-end: 0 !important;
}
</style>

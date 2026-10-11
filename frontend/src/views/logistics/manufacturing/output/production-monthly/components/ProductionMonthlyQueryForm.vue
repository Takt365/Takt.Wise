<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-monthly/components -->
<!-- 文件名称：ProductionMonthlyQueryForm.vue -->
<!-- 功能描述：月生产推移查询栏（工厂→产出类别→机种 + 期间） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar production-monthly-query-bar">
    <div class="production-monthly-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          :api-url="plantOptionsUrl"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--plant"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
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
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.outputCategory`) }}</span>
        <TaktSelect
          v-model:value="outputCategory"
          :api-url="outputCategoryOptionsUrl"
          :api-params="outputCategoryApiParams"
          :disabled="!plantCode?.trim()"
          class="production-monthly-query-bar__control production-monthly-query-bar__control--category"
          allow-clear
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.modelCode`) }}</span>
        <TaktSelect
          :key="modelSelectKey"
          v-model:value="modelCode"
          :api-url="modelOptionsUrl"
          :api-params="modelApiParams"
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
 * 月生产推移查询栏：工厂 → 产出类别 → 机种（机种可空）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getProductionMonthlyTrendModelOptionsUrl,
  getProductionMonthlyTrendOutputCategoryOptionsUrl,
  getProductionMonthlyTrendPlantOptionsUrl,
} from '@/api/logistics/manufacturing/output/productionMonthly.api'

/** 工厂代码（第 1 级，必选） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 机种（第 3 级，可空） */
const modelCode = defineModel<string | undefined>('modelCode')
/** 产出类别（第 2 级，可空） */
const outputCategory = defineModel<string | undefined>('outputCategory')
const props = defineProps<{
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
/** 机种下拉刷新键 */
const modelSelectKey = ref(0)
/** 推移本表级联选项 URL（TaktProductionMonthlyTrends） */
const plantOptionsUrl = getProductionMonthlyTrendPlantOptionsUrl()
const outputCategoryOptionsUrl = getProductionMonthlyTrendOutputCategoryOptionsUrl()
const modelOptionsUrl = getProductionMonthlyTrendModelOptionsUrl()

/** 第 2 级：工厂 → 产出类别 */
const outputCategoryApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 第 3 级：工厂 + 产出类别 → 机种（类别可空并集） */
const modelApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  const category = outputCategory.value?.trim()
  return {
    plantCode: plant,
    ...(category ? { outputCategory: category } : {}),
  }
})

/** 工厂变更：清空第 2～3 级 */
watch(
  () => plantCode.value,
  () => {
    outputCategory.value = undefined
    modelCode.value = undefined
    modelSelectKey.value += 1
  },
)

/** 产出类别变更：清空第 3 级 */
watch(
  () => outputCategory.value,
  () => {
    modelCode.value = undefined
    modelSelectKey.value += 1
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

.production-monthly-query-bar__control--category {
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

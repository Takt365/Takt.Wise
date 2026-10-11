<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/cost/cost-trend/components -->
<!-- 文件名称：CostTrendQueryForm.vue -->
<!-- 功能描述：质量成本推移查询栏（工厂/期间/成本类别/币种） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar cost-trend-query-bar">
    <div class="cost-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          api-url="TaktPlants/options"
          class="cost-trend-query-bar__control cost-trend-query-bar__control--plant"
          allow-clear
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
          class="cost-trend-query-bar__control cost-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.costCategory`) }}</span>
        <a-select
          v-model:value="costCategory"
          class="cost-trend-query-bar__control cost-trend-query-bar__control--category"
          allow-clear
          :placeholder="t('common.page.form.placeholder.selectonly')"
          :options="costCategoryOptions"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.currencyCode`) }}</span>
        <TaktSelect
          v-model:value="currencyCode"
          dict-type="accounting_financial_currency_code"
          class="cost-trend-query-bar__control cost-trend-query-bar__control--currencyCode"
          allow-clear
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
 * 质量成本推移查询栏：工厂 + 期间 + 成本类别 + 币种
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 成本类别 */
const costCategory = defineModel<string | undefined>('costCategory')
/** 成本币种 */
const currencyCode = defineModel<string | undefined>('currencyCode')
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
const localePrefix = 'logistics.quality.cost.cost-trend.page'

/** 成本类别选项 */
const costCategoryOptions = computed(() => [
  { value: 'assurance', label: t(`${localePrefix}.costCategoryOptions.assurance`) },
  { value: 'issue', label: t(`${localePrefix}.costCategoryOptions.issue`) },
  { value: 'incident', label: t(`${localePrefix}.costCategoryOptions.incident`) }])
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

.cost-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.cost-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.cost-trend-query-bar__control--category {
  width: 12rem;
  min-width: 9rem;
}

.cost-trend-query-bar__control--currencyCode {
  width: 10rem;
  min-width: 8rem;
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

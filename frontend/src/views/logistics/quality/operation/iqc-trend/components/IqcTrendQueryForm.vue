<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-trend/components -->
<!-- 文件名称：IqcTrendQueryForm.vue -->
<!-- 功能描述：进货检验推移查询栏（工厂/期间/供应商） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar inspection-trend-query-bar">
    <div class="inspection-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          api-url="TaktPlants/options"
          class="inspection-trend-query-bar__control inspection-trend-query-bar__control--plant"
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
          class="inspection-trend-query-bar__control inspection-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.supplierCode`) }}</span>
        <TaktSelect
          v-model:value="supplierCode"
          api-url="TaktSuppliers/options"
          class="inspection-trend-query-bar__control inspection-trend-query-bar__control--dimension"
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
 * 进货检验推移查询栏
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'

/** 工厂代码 */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 供应商编码 */
const supplierCode = defineModel<string | undefined>('supplierCode')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
const localePrefix = 'logistics.quality.operation.iqc-trend.page'
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

.inspection-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.inspection-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.inspection-trend-query-bar__control--dimension {
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

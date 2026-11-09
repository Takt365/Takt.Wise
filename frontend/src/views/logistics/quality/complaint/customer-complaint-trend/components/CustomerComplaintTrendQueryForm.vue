<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-trend/components -->
<!-- 文件名称：CustomerComplaintTrendQueryForm.vue -->
<!-- 功能描述：顾客投诉推移查询栏（工厂/期间/客户/投诉类型） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktQueryBar
    :show-keyword="false"
    :loading="props.loading"
    @search="emit('search')"
    @reset="emit('reset')"
  >
    <template #leading>
<div class="customer-complaint-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.customercomplaint.customercode') }}</span>
        <TaktSelect
          v-model:value="customerCode"
          api-url="TaktCustomers/options"
          class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--customer"
          allow-clear
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.customercomplaint.complainttype') }}</span>
        <TaktSelect
          v-model:value="complaintType"
          dict-type="logistics_quality_complaint_type"
          class="customer-complaint-trend-query-bar__control customer-complaint-trend-query-bar__control--complaint-type"
          allow-clear
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
    </div>
    </template>
  </TaktQueryBar>
</template>

<script setup lang="ts">
/**
 * 顾客投诉推移查询栏：工厂 + 期间 + 客户 + 投诉类型
 */
import { useI18n } from 'vue-i18n'

/** 工厂代码 */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 客户编码 */
const customerCode = defineModel<string | undefined>('customerCode')
/** 投诉类型 */
const complaintType = defineModel<number | undefined>('complaintType')
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
const localePrefix = 'logistics.quality.complaint.customer-complaint-trend.page'
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

.customer-complaint-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.customer-complaint-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.customer-complaint-trend-query-bar__control--customer {
  width: 12rem;
  min-width: 9rem;
}

.customer-complaint-trend-query-bar__control--complaint-type {
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-monthly-trend/components -->
<!-- 文件名称：EcMonthlyTrendQueryForm.vue -->
<!-- 功能描述：月设变/月实施推移查询栏（工厂→部门→设变号级联 + Tab 条件筛选） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar ec-monthly-trend-query-bar">
    <div class="ec-monthly-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ gi.label('plantCode') }}</span>
        <TaktSelect
          v-model:value="plantCode"
          :api-url="plantOptionsUrl"
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--plant"
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
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          :disabled-date="disabledDate"
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.deptCode`) }}</span>
        <TaktSelect
          v-model:value="deptCode"
          :api-url="deptOptionsUrl"
          :api-params="deptApiParams"
          :disabled="!plantCode?.trim()"
          class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--dept"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
        />
      </div>
      <template v-if="props.activeTab === 'issue'">
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.ecCode`) }}</span>
          <TaktSelect
            v-model:value="ecCode"
            :api-url="ecCodeOptionsUrl"
            :api-params="ecCodeApiParams"
            :disabled="!plantCode?.trim()"
            class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--ec-no"
            allow-clear
            show-search
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ gi.label('ecScope') }}</span>
          <TaktSelect
            v-model:value="ecScope"
            dict-type="logistics_manufacturing_ec_scope_category"
            class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--scope"
            allow-clear
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ gi.label('changeStatus') }}</span>
          <TaktSelect
            v-model:value="changeStatus"
            dict-type="logistics_manufacturing_ec_status"
            class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--status"
            allow-clear
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
        <div class="flex items-center gap-1">
          <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ gi.label('ecStatus') }}</span>
          <TaktSelect
            v-model:value="ecStatus"
            dict-type="logistics_manufacturing_ec_gijutsu_status"
            class="ec-monthly-trend-query-bar__control ec-monthly-trend-query-bar__control--ec-status"
            allow-clear
            :placeholder="t('common.page.form.placeholder.selectonly')"
          />
        </div>
      </template>
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
 * 月设变/月实施推移查询栏：工厂 → 部门 → 设变单号（本表级联）
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getEcMonthlyTrendDeptOptionsUrl,
  getEcMonthlyTrendEcCodeOptionsUrl,
  getEcMonthlyTrendPlantOptionsUrl,
} from '@/api/logistics/manufacturing/engineering-change/ecMonthlyTrend.api'
import { useEcGijutsuI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcGijutsuI18n'
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit'

/** 工厂代码（第 1 级） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 部门编码（第 2 级，可空） */
const deptCode = defineModel<string | undefined>('deptCode')
/** 设变单号（第 3 级，可空；仅 issue Tab） */
const ecCode = defineModel<string | undefined>('ecCode')
/** 实施范围（EcScope） */
const ecScope = defineModel<number | undefined>('ecScope')
/** 变更状态 */
const changeStatus = defineModel<number | undefined>('changeStatus')
/** 设变状态 */
const ecStatus = defineModel<number | undefined>('ecStatus')
const props = defineProps<{
  /** 当前 Tab */
  activeTab: 'issue' | 'implement'
  /** 查询 loading */
  loading?: boolean
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
const gi = useEcGijutsuI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.manufacturing.engineering-change.ec-monthly-trend.page'
/** 推移本表级联选项 URL（TaktEcMonthlyTrends） */
const plantOptionsUrl = getEcMonthlyTrendPlantOptionsUrl()
const deptOptionsUrl = getEcMonthlyTrendDeptOptionsUrl()
const ecCodeOptionsUrl = getEcMonthlyTrendEcCodeOptionsUrl()

/** 第 2 级：工厂 → 部门 */
const deptApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 第 3 级：工厂 + 部门 → 设变单号（部门可空） */
const ecCodeApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  const dept = deptCode.value?.trim()
  return dept ? { plantCode: plant, deptCode: dept } : { plantCode: plant }
})

/** 工厂变更：清空第 2～3 级 */
watch(
  () => plantCode.value,
  () => {
    deptCode.value = undefined
    ecCode.value = undefined
  },
)

/** 部门变更：清空第 3 级 */
watch(
  () => deptCode.value,
  () => {
    ecCode.value = undefined
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

.ec-monthly-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.ec-monthly-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.ec-monthly-trend-query-bar__control--scope,
.ec-monthly-trend-query-bar__control--status,
.ec-monthly-trend-query-bar__control--ec-status,
.ec-monthly-trend-query-bar__control--dept,
.ec-monthly-trend-query-bar__control--ec-no {
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

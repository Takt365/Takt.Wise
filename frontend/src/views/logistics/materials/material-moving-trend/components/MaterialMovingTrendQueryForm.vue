<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-moving-trend/components -->
<!-- 文件名称：MaterialMovingTrendQueryForm.vue -->
<!-- 功能描述：物料移动价格推移查询栏（工厂/期间必选；评估/物料可空并级联） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="takt-query-bar material-moving-trend-query-bar">
    <div class="material-moving-trend-query-bar__fields min-w-0 flex flex-1 flex-wrap items-center gap-x-3 gap-y-2">
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('common.page.entity.plantcode') }}</span>
        <TaktSelect
          :key="`plant-${props.plantSelectKey ?? 0}`"
          v-model="plantCode"
          :api-url="plantOptionsUrl"
          class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--plant"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
          @change="handlePlantChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.periodRange`) }}</span>
        <a-range-picker
          v-model:value="periodRange"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          :disabled-date="disabledDate"
          class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--period"
          :placeholder="[
            t(`${localePrefix}.periodRange`),
            t(`${localePrefix}.periodRange`)]"
        
          @calendar-change="onCalendarChange"
          @open-change="onOpenChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t('entity.materialmovingprice.valuation') }}</span>
        <TaktSelect
          :key="`valuation-${plantCode || ''}`"
          v-model:value="valuation"
          :api-url="valuationOptionsUrl"
          :api-params="valuationApiParams"
          :disabled="!plantCode?.trim()"
          class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--valuation"
          allow-clear
          show-search
          :placeholder="t('common.page.form.placeholder.selectonly')"
          @change="handleValuationChange"
        />
      </div>
      <div class="flex items-center gap-1">
        <span class="shrink-0 text-sm text-text-secondary whitespace-nowrap">{{ t(`${localePrefix}.materialCode`) }}</span>
        <TaktSelect
          :key="`material-${plantCode || ''}-${valuation || ''}`"
          v-model:value="materialCode"
          :api-url="materialOptionsUrl"
          :api-params="materialApiParams"
          :disabled="!plantCode?.trim()"
          class="material-moving-trend-query-bar__control material-moving-trend-query-bar__control--material"
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
 * 物料移动价格推移查询栏：工厂 → 评估类别（可空）→ 物料（可空）；选项随上级联动
 */
import { RiSearchLine, RiRefreshLine } from '@remixicon/vue'
import { useI18n } from 'vue-i18n'
import {
  getMaterialMovingTrendMaterialOptionsUrl,
  getMaterialMovingTrendPlantOptionsUrl,
  getMaterialMovingTrendValuationOptionsUrl,
} from '@/api/logistics/materials/materialMovingTrend.api'

/** 工厂代码（第 1 级，必选） */

/** 期间最多 12 个月 */
const { disabledDate, onCalendarChange, onOpenChange } = useAnalysisPeriodRangeLimit()

const plantCode = defineModel<string | undefined>('plantCode')
/** 年月区间 */
const periodRange = defineModel<[string, string] | null>('periodRange')
/** 评估类别（可空；选项随工厂联动） */
const valuation = defineModel<string | undefined>('valuation')
/** 物料编码（可空；选项随工厂+评估联动） */
const materialCode = defineModel<string | undefined>('materialCode')
const props = defineProps<{
  /** 查询 loading */
  loading?: boolean
  /** 工厂下拉重挂载键（默认选中后刷新选项） */
  plantSelectKey?: number
}>()
const emit = defineEmits<{
  search: []
  reset: []
}>()

const { t } = useI18n()
/** 静态 locales 前缀 */
const localePrefix = 'logistics.materials.material-moving-trend.page'
/** 推移本表级联选项 URL（TaktMaterialMovingTrends） */
const plantOptionsUrl = getMaterialMovingTrendPlantOptionsUrl()
const valuationOptionsUrl = getMaterialMovingTrendValuationOptionsUrl()
const materialOptionsUrl = getMaterialMovingTrendMaterialOptionsUrl()

/** 工厂 → 评估类别 */
const valuationApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  return { plantCode: plant }
})

/** 工厂 → 物料；有评估类别时一并传入收窄列表 */
const materialApiParams = computed(() => {
  const plant = plantCode.value?.trim()
  if (!plant) {
    return undefined
  }
  const val = valuation.value?.trim()
  return val ? { plantCode: plant, valuation: val } : { plantCode: plant }
})

/** 工厂变更：清空下游评估/物料 */
function handlePlantChange() {
  valuation.value = undefined
  materialCode.value = undefined
}

/** 评估类别变更：清空物料（选项随 :key 重载） */
function handleValuationChange() {
  materialCode.value = undefined
}
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

.material-moving-trend-query-bar__control--plant {
  width: 10rem;
  min-width: 8rem;
}

.material-moving-trend-query-bar__control--period {
  width: 16rem;
  min-width: 14rem;
}

.material-moving-trend-query-bar__control--type {
  width: 7rem;
  min-width: 6rem;
}

.material-moving-trend-query-bar__control--valuation {
  width: 10rem;
  min-width: 8rem;
}

.material-moving-trend-query-bar__control--material {
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

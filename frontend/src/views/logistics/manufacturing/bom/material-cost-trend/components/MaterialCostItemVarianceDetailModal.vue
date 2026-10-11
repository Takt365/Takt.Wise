<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：MaterialCostItemVarianceDetailModal.vue -->
<!-- 功能描述：BOM 物料成本两期间组件差异下钻弹窗（汇总 + 明细表 + 导出） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <TaktModal
    v-model:open="open"
    :title="t(`${localePrefix}.variance.title`)"
    :use-viewport-size="true"
    :hide-footer="true"
    @cancel="open = false"
  >
    <a-spin :spinning="loading">
      <div v-if="varianceData" class="flex flex-col gap-4">
        <div class="flex flex-wrap items-center justify-between gap-2">
          <div class="text-sm text-text-secondary">
            {{
              t(`${localePrefix}.variance.summary`, {
                productCode: varianceData.productCode,
                basePeriod: varianceData.basePeriod,
                comparePeriod: varianceData.comparePeriod,
              })
            }}
          </div>
          <a-button
            v-permission="'logistics:manufacturing:bom:material:cost:trend:export'"
            type="primary"
            :loading="exportLoading"
            @click="handleExport"
          >
            {{ t(`${localePrefix}.variance.export`) }}
          </a-button>
        </div>
        <a-descriptions bordered size="small" :column="3">
          <a-descriptions-item :label="t(`${localePrefix}.totals.baseTotal`)">
            {{ formatCost(varianceData.baseTotalCost) }}
          </a-descriptions-item>
          <a-descriptions-item :label="t(`${localePrefix}.totals.compareTotal`)">
            {{ formatCost(varianceData.compareTotalCost) }}
          </a-descriptions-item>
          <a-descriptions-item :label="t(`${localePrefix}.totals.totalVariance`)">
            <span :class="varianceClass(varianceData.totalVariance)">
              {{ formatCost(varianceData.totalVariance) }}
            </span>
          </a-descriptions-item>
        </a-descriptions>
        <a-table
          class="takt-analysis-table"
          size="small"
          :columns="varianceColumns"
          :data-source="varianceData.lines"
          :row-key="getVarianceLineKey"
          :pagination="{ pageSize: 20, showSizeChanger: true }"
          :scroll="{ x: 'max-content' }"
        >
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'changeType'">
              {{ changeTypeLabel((record as BomMaterialCostItemVarianceLine).changeType) }}
            </template>
            <template v-else-if="column.key === 'baseCost' || column.key === 'compareCost'">
              {{ formatCost((record as Record<string, number | null | undefined>)[String(column.key)]) }}
            </template>
            <template v-else-if="column.key === 'varianceAmount'">
              <span :class="varianceClass((record as BomMaterialCostItemVarianceLine).varianceAmount)">
                {{ formatCost((record as BomMaterialCostItemVarianceLine).varianceAmount) }}
              </span>
            </template>
          </template>
        </a-table>
      </div>
    </a-spin>
  </TaktModal>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本组件差异下钻弹窗
 */
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import {
  exportBomMaterialCostItemVarianceAnalysis,
  getBomMaterialCostItemVarianceAnalysis,
} from '@/api/logistics/manufacturing/bom/materialCostAnalysis.api';
import type {
  BomMaterialCostItemVarianceLine,
  BomMaterialCostItemVarianceResult,
} from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types';
import {
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  useMaterialCostAnalysis,
  type BomMaterialCostItemVarianceQueryContext,
} from '../composables/useMaterialCostItemAnalysis';

/** 弹窗开关 */
const open = defineModel<boolean>('open', { default: false });
const props = defineProps<{
  /** 下钻查询条件；打开弹窗时据此拉取差异 */
  query?: BomMaterialCostItemVarianceQueryContext | null;
}>();

const { t } = useI18n();
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX;
const { formatCost, varianceClass, changeTypeLabel } = useMaterialCostAnalysis();
/** 差异 loading */
const loading = ref(false);
/** 差异导出 loading */
const exportLoading = ref(false);
/** 差异数据 */
const varianceData = ref<BomMaterialCostItemVarianceResult | null>(null);

/** 差异明细列 */
const varianceColumns = computed<TableColumnsType>(() => [
  { title: t('entity.bommaterialcostitem.bomitemcode'), dataIndex: 'bomItemCode', key: 'bomItemCode', width: 90 },
  { title: t('entity.bommaterialcostitem.componentcode'), dataIndex: 'componentCode', key: 'componentCode', width: 120 },
  { title: t('entity.bommaterialcostitem.componentdescription'), dataIndex: 'componentDescription', key: 'componentDescription', width: 160 },
  { title: t(`${localePrefix}.columns.changeType`), dataIndex: 'changeType', key: 'changeType', width: 100 },
  { title: t(`${localePrefix}.variance.baseCost`), dataIndex: 'baseCost', key: 'baseCost', width: 100, align: 'right' },
  { title: t(`${localePrefix}.variance.compareCost`), dataIndex: 'compareCost', key: 'compareCost', width: 100, align: 'right' },
  { title: t(`${localePrefix}.columns.varianceAmount`), dataIndex: 'varianceAmount', key: 'varianceAmount', width: 100, align: 'right' }]);

/**
 * 差异行主键
 * @param record 差异行
 * @returns 组件键
 */
function getVarianceLineKey(record: BomMaterialCostItemVarianceLine) {
  return `${record.bomItemCode}|${record.componentCode}`;
}

/**
 * 加载差异分析数据
 * @param query 查询上下文
 */
async function loadVariance(query: BomMaterialCostItemVarianceQueryContext) {
  loading.value = true;
  varianceData.value = null;
  try {
    varianceData.value = await getBomMaterialCostItemVarianceAnalysis(query);
  } finally {
    loading.value = false;
  }
}

/** 导出差异明细 */
async function handleExport() {
  if (!props.query) return;
  exportLoading.value = true;
  try {
    const blob = await exportBomMaterialCostItemVarianceAnalysis(props.query);
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `BOM_${props.query.productCode}_${props.query.comparePeriod}.xlsx`;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  } finally {
    exportLoading.value = false;
  }
}

watch(
  () => [open.value, props.query] as const,
  ([isOpen, query]) => {
    if (isOpen && query) {
      void loadVariance(query);
    }
    if (!isOpen) {
      varianceData.value = null;
    }
  },
);
</script>

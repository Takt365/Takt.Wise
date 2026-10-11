<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost-trend/components -->
<!-- 文件名称：MaterialCostItemTransposedTable.vue -->
<!-- 功能描述：机种下各成品材料成本转置表（行=成品，列=月份，表尾=机种平均） -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div>
    <TaktSingleTable
      analysis-table
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'productCode'"
      table-mode="single"
      :data-source="rows"
      :loading="loading"
      :stripe="true"
      :row-key="getRowKey"
      :pagination="false"
      :scroll="{ x: 'max-content' }"
      :footer-remark="summaryText"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="String(column.key).startsWith('period_')">
          {{ formatPeriodCost(record as BomMaterialCostItemTransposed, String(column.key)) }}
        </template>
      </template>
      <template v-if="modelSummary" #summary>
        <a-table-summary fixed>
          <a-table-summary-row>
            <a-table-summary-cell :index="0">
              <span class="font-medium text-text">{{ t(`${localePrefix}.modelAverageLabel`) }}</span>
            </a-table-summary-cell>
            <a-table-summary-cell :index="1">
              <span class="text-text-secondary">{{ modelSummarySubtitle }}</span>
            </a-table-summary-cell>
            <a-table-summary-cell
              v-for="(period, idx) in periodOrder"
              :key="`avg_${period}`"
              :index="2 + idx"
              align="right"
            >
              <span class="font-medium text-primary">{{ formatAverageCost(period) }}</span>
            </a-table-summary-cell>
          </a-table-summary-row>
        </a-table-summary>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-if="total > 0"
      class="mt-4"
      :current="pageIndex"
      :page-size="pageSize"
      :total="total"
      @change="handlePageChange"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 机种成品材料成本转置表（含机种平均成本汇总行）
 */
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import type {
  BomMaterialCostItemModelSummary,
  BomMaterialCostItemTransposed,
} from '@/types/logistics/manufacturing/bom/materialCostAnalysis.types';
import {
  MATERIAL_COST_ANALYSIS_LOCALE_PREFIX,
  useMaterialCostAnalysis,
} from '../composables/useMaterialCostItemAnalysis';

const props = defineProps<{
  /** 机种编码 */
  modelCode?: string;
  /** 机种平均成本汇总（全量成品，不受分页影响） */
  modelSummary?: BomMaterialCostItemModelSummary | null;
  /** 表格行 */
  rows: BomMaterialCostItemTransposed[];
  /** 期间列顺序 */
  periodOrder: string[];
  /** 加载态 */
  loading?: boolean;
  /** 当前页 */
  pageIndex: number;
  /** 页大小 */
  pageSize: number;
  /** 总条数 */
  total: number;
}>();

const emit = defineEmits<{
  /** 分页变更 */
  'page-change': [page: number, pageSize: number];
}>();

const { t } = useI18n();
const { formatCost } = useMaterialCostAnalysis();
const localePrefix = MATERIAL_COST_ANALYSIS_LOCALE_PREFIX;

/** 页头摘要 */
const summaryText = computed(() => {
  if (!props.modelCode) {
    return '';
  }
  const count = props.modelSummary?.productCount ?? props.total;
  return t(`${localePrefix}.modelProductListSummary`, {
    modelCode: props.modelCode,
    count,
  });
});

/** 汇总行副标题 */
const modelSummarySubtitle = computed(() => {
  if (!props.modelSummary) {
    return '';
  }
  const name = props.modelSummary.modelName?.trim();
  return t(`${localePrefix}.modelAverageSubtitle`, {
    modelName: name || props.modelSummary.modelCode,
    count: props.modelSummary.productCount,
  });
});

/** 转置列 */
const columns = computed<TableColumnsType>(() => {
  const fixed: TableColumnsType = [
    {
      title: t('entity.bommaterialcostitem.productcode'),
      dataIndex: 'productCode',
      key: 'productCode',
      width: 130,
      fixed: 'left',
    },
    {
      title: t('entity.bommaterialcostitem.productdescription'),
      dataIndex: 'productDescription',
      key: 'productDescription',
      width: 200,
      fixed: 'left',
    }];
  const periodCols: TableColumnsType = props.periodOrder.map((period) => ({
    title: period,
    key: `period_${period}`,
    align: 'right' as const,
    width: 108,
  }));
  return [...fixed, ...periodCols];
});

/** 可见列 */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)));

/**
 * 行主键
 * @param record 转置行
 * @returns 产品编码
 */
function getRowKey(record: BomMaterialCostItemTransposed) {
  return record.productCode;
}

/**
 * 格式化期间成本单元格
 * @param record 转置行
 * @param columnKey 列键 period_yyyy-MM
 * @returns 展示文本
 */
function formatPeriodCost(record: BomMaterialCostItemTransposed, columnKey: string) {
  const period = columnKey.replace(/^period_/, '');
  const value = record.periodCosts?.[period];
  if (value == null) {
    return '—';
  }
  return formatCost(value);
}

/**
 * 格式化机种平均成本
 * @param period 期间 yyyy-MM
 * @returns 展示文本
 */
function formatAverageCost(period: string) {
  const value = props.modelSummary?.averagePeriodCosts?.[period];
  if (value == null) {
    return '—';
  }
  return formatCost(value);
}

/**
 * 分页变更
 * @param page 页码
 * @param size 页大小
 */
function handlePageChange(page: number, size: number) {
  emit('page-change', page, size);
}
</script>

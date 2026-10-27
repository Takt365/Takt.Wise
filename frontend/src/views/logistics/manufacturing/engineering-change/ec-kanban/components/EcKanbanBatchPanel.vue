<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanBatchPanel.vue -->
<!-- 功能描述：设变看板批次页签：一个设变号一组，各部门按可见明细行数展开成表格行 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="box-border flex w-full min-w-0 max-w-full flex-col overflow-x-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :loading="loading"
      :placeholder="t('common.page.form.placeholder.search')"
      @search="handleSearch"
      @reset="handleReset"
    >
      <template #leading>
        <span class="whitespace-nowrap text-sm text-text-secondary">{{ t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange') }}</span>
        <a-range-picker
          v-model:value="entryPeriod"
          picker="month"
          format="YYYY-MM"
          value-format="YYYY-MM"
          class="w-64 min-w-56"
          :placeholder="[
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
            t('logistics.manufacturing.engineering-change.ec-stat.page.entryPeriodRange'),
          ]"
          :disabled-date="entryPeriodLimit.disabledDate"
          @calendar-change="entryPeriodLimit.onCalendarChange"
          @open-change="entryPeriodLimit.onOpenChange"
        />
      </template>
    </TaktQueryBar>
    <TaktToolsBar
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      export-permission="logistics:manufacturing:engineering:change:kanban:export"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="loadData"
    />
    <TaktSingleTable
      analysis-table
      entity-scope="company"
      :show-row-selection="false"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecDetailId'"
      table-mode="single"
      :data-source="tableRows"
      :loading="loading"
      :stripe="true"
      :virtual="false"
      :row-key="getEcDetailId"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'ecCode'">
            <a-typography-link @click.stop="handleDetail(record as BatchTableRow)">
              {{ record.ecCode }}
            </a-typography-link>
          </template>
          <template v-else-if="column.key === 'ecIssueDate' || column.key === 'ecEntryDate'">
            <span>{{ formatCalendarDate((record as Record<string, unknown>)[String(column.key)]) }}</span>
          </template>
          <template v-else-if="isStageColumn(column.key)">
            <span>{{ (record as BatchTableRow)[String(column.key)] }}</span>
          </template>
      </template>
    </TaktSingleTable>
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
    />
    <TaktModal
      v-model:open="detailVisible"
      :title="t('common.dialog.title.detail', { entity: t('logistics.manufacturing.engineering-change.ec-kanban.page.tabs.batch') })"
      width="960px"
      :hide-footer="true"
      @cancel="detailVisible = false"
    >
      <a-spin :spinning="detailLoading">
        <a-descriptions v-if="detailData" bordered :column="2" size="small">
          <a-descriptions-item :label="gi.label('ecCode')">{{ detailData.ecCode }}</a-descriptions-item>
          <a-descriptions-item :label="pi.label('lineNumber')">{{ detailData.lineNumber }}</a-descriptions-item>
          <a-descriptions-item :label="pi.label('ecModelCode')">{{ detailData.ecModelCode }}</a-descriptions-item>
          <a-descriptions-item :label="pi.label('ecNewMaterialCode')">{{ detailData.ecNewMaterialCode ?? '—' }}</a-descriptions-item>
          <a-descriptions-item :label="pi.label('ecOldMaterialCode')">{{ detailData.ecOldMaterialCode ?? '—' }}</a-descriptions-item>
          <a-descriptions-item :label="gi.label('ecEntryDate')">{{ formatCalendarDate(detailData.ecEntryDate) || '—' }}</a-descriptions-item>
        </a-descriptions>
      </a-spin>
    </TaktModal>
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-kanban-batch"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('ecCode')">
          <a-form-item :label="gi.label('ecCode')">
            <a-input v-model:value="advancedQueryForm.ecCode" allow-clear :maxlength="10" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecModelCode')">
          <a-form-item :label="pi.label('ecModelCode')">
            <a-input v-model:value="advancedQueryForm.ecModelCode" allow-clear :maxlength="40" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecNewMaterialCode')">
          <a-form-item :label="pi.label('ecNewMaterialCode')">
            <a-input v-model:value="advancedQueryForm.ecNewMaterialCode" allow-clear :maxlength="20" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecIssueDateStart')">
          <a-form-item :label="gi.queryLabel('ecIssueDateStart')">
            <a-date-picker v-model:value="advancedQueryForm.ecIssueDateStart" value-format="YYYY-MM-DD" class="w-full" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecIssueDateEnd')">
          <a-form-item :label="gi.queryLabel('ecIssueDateEnd')">
            <a-date-picker v-model:value="advancedQueryForm.ecIssueDateEnd" value-format="YYYY-MM-DD" class="w-full" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('batchCode')">
          <a-form-item :label="pc.label('scheduledBatch')">
            <a-input :value="advancedQueryForm.batchCode" allow-clear :maxlength="20" @update:value="(value) => { advancedQueryForm.batchCode = taktDeptBatchInput(value) }" />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecDetailId"
      action-column-key="action"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板批次页签：一个设变号一组。部门有几条可见明细就展开几行，设变信息每行重复
 */
import '../../ec-list-layout.css'
import { RiEyeLine } from '@remixicon/vue';
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit';
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod';
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n';
import { getEcBatchTransposedList } from '@/api/logistics/manufacturing/engineering-change/ecBatch.api';
import { getEcDetailById } from '@/api/logistics/manufacturing/engineering-change/ecDetail.api';
import { exportEcBatchData } from '@/api/logistics/manufacturing/engineering-change/ecBatch.api';
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index';
import { TaktEcBatchStageCodes, TaktEcBatchStageTransposedOrder } from '@/constants/logistics/ecBatchStageCodes.constants';
import { taktDeptBatchInput } from '@/constants/logistics/ecDeptExecLineFields.constants';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types';
import type {
  EcExecBatchTransposed,
  EcExecBatchTransposedQuery,
  EcExecBatchTransposedResult,
  EcExecBatchTransposedStageLine,
} from '@/types/logistics/manufacturing/engineering-change/ecExecTransposed.types';
import { formatCalendarDate } from '@/utils/datetime';
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcDetailI18n';
import { useEcTeI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcTeI18n';

const { t } = useI18n();
const pi = useEcDetailI18n();
const gi = useEcTeI18n();
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page.batch';

/** 展开后的一行：同一设变号按各部门最大行数展开，阶段列只放这一行 */
interface BatchTableRow {
  rowKey: string;
  lineIndex: number;
  lineCount: number;
  ecDetailId: string;
  ecCode: string;
  technicalLiaisonNo: string;
  pNo: string;
  tcjLiaisonNo: string;
  ecIssueDate: string;
  ecModelCode: string;
  ecNewMaterialCode: string;
  ecEntryDate: string;
  [key: string]: string | number;
}
/** 生管 entity.ecpc.* */
const pc = useEntityFieldI18n('ecpc');
/** 列表 loading */
const loading = ref(false);
/** 转置结果 */
const transposedResult = ref<EcExecBatchTransposedResult | null>(null);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词，放在查询按钮前面 */
const queryKeyword = ref('');
/** 录入期间，截止本月 */
const entryPeriod = ref<[string, string] | null>(buildDefaultThroughCurrentPeriodRange(3));
/** 录入期间可选到本月，最多 12 个月 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true });
/** 详情弹窗 */
const detailVisible = ref(false);
/** 详情 loading */
const detailLoading = ref(false);
/** 详情数据 */
const detailData = ref<EcDetail | null>(null);
/** 列设置抽屉 */
const columnSettingVisible = ref(false);
/** 用户勾选的列；空表示跟随当前全部列 */
const userVisibleColumnKeys = ref<string[] | null>(null);
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false);
/** 高级查询字段 */
const BATCH_QUERY_FIELDS = [
  'ecCode',
  'ecModelCode',
  'ecNewMaterialCode',
  'ecIssueDateStart',
  'ecIssueDateEnd',
  'batchCode',
] as const;
/** 高级查询可见字段 */
const visibleQueryFieldKeys = ref<string[]>([]);
/**
 * 空的高级查询表单
 */
function createEmptyAdvancedQueryForm() {
  return {
    ecCode: '',
    ecModelCode: '',
    ecNewMaterialCode: '',
    ecIssueDateStart: '',
    ecIssueDateEnd: '',
    batchCode: '',
  };
}
/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm());
/** 高级查询字段标题 */
const queryFieldsMeta = computed(() => BATCH_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'ecCode' || key === 'ecIssueDateStart' || key === 'ecIssueDateEnd'
    ? gi.queryLabel(key)
    : key === 'batchCode'
      ? pc.label('scheduledBatch')
      : pi.label(key),
})));

/**
 * 设变信息跨行。第一行占满该设变号的全部展开行
 * @param record 展开行
 * @returns 单元格跨行
 */
function identityCell(record: BatchTableRow) {
  return { rowSpan: record.lineIndex === 0 ? record.lineCount : 0 };
}

/**
 * 叶子列
 * @param titleKey 页面列文案键
 * @param key 列键
 * @param width 列宽
 * @param spanGroup 是否按设变号跨行
 * @returns 列定义
 */
function leafColumn(titleKey: string, key: string, width: number, spanGroup = false) {
  return {
    title: t(`${localePrefix}.column.${titleKey}`),
    dataIndex: key,
    key,
    width,
    align: 'center' as const,
    customCell: spanGroup ? identityCell : undefined,
  };
}

/**
 * 分组表头
 * @param titleKey 分组文案键
 * @param key 分组键
 * @param children 子列
 * @returns 分组列
 */
function groupColumn(titleKey: string, key: string, children: TableColumnsType) {
  return {
    title: t(`${localePrefix}.group.${titleKey}`),
    key,
    align: 'center' as const,
    children,
  };
}

/** 两级表头：设变信息，以及生管、采购、受检、部管、物料、制二、制一、品管、制技 */
const columns = computed(() => {
  const base: TableColumnsType = [
    groupColumn('ecInfo', 'ecInfo', [
      leafColumn('ecCode', 'ecCode', 120, true),
      leafColumn('liaisonNo', 'technicalLiaisonNo', 120, true),
      leafColumn('pNo', 'pNo', 110, true),
      leafColumn('tcjDate', 'tcjLiaisonNo', 130, true),
      leafColumn('issueDate', 'ecIssueDate', 110, true),
      leafColumn('model', 'ecModelCode', 160, true),
      leafColumn('product', 'ecNewMaterialCode', 180, true),
      leafColumn('entryDate', 'ecEntryDate', 110, true),
    ]),
    groupColumn('pc', 'pc', [
      leafColumn('scheduledInput', `stageDate_${TaktEcBatchStageCodes.Scheduled}`, 110),
      leafColumn('productionSchedule', `stageBatch_${TaktEcBatchStageCodes.Scheduled}`, 110),
    ]),
    groupColumn('pur', 'pur', [
      leafColumn('orderDate', `stageDate_${TaktEcBatchStageCodes.PurchaseOrder}`, 110),
      leafColumn('orderCode', `stageBatch_${TaktEcBatchStageCodes.PurchaseOrder}`, 130),
    ]),
    groupColumn('iqc', 'iqc', [
      leafColumn('inspectionDate', `stageDate_${TaktEcBatchStageCodes.IncomingInspection}`, 110),
      leafColumn('iqcOrderCode', `stageBatch_${TaktEcBatchStageCodes.IncomingInspection}`, 130),
    ]),
    groupColumn('mc', 'mc', [
      leafColumn('outboundDate', `stageDate_${TaktEcBatchStageCodes.Outbound}`, 110),
      leafColumn('outboundBatch', `stageBatch_${TaktEcBatchStageCodes.Outbound}`, 120),
    ]),
    groupColumn('mtl', 'mtl', [
      leafColumn('outboundDate', `stageDate_${TaktEcBatchStageCodes.ElectronicOutbound}`, 110),
      leafColumn('outboundBatch', `stageBatch_${TaktEcBatchStageCodes.ElectronicOutbound}`, 120),
    ]),
    groupColumn('mfg2', 'mfg2', [
      leafColumn('productionDate', `stageDate_${TaktEcBatchStageCodes.Mfg2Production}`, 110),
      leafColumn('productionBatch', `stageBatch_${TaktEcBatchStageCodes.Mfg2Production}`, 120),
    ]),
    groupColumn('mfg1', 'mfg1', [
      leafColumn('productionDate', `stageDate_${TaktEcBatchStageCodes.Mfg1Production}`, 110),
      leafColumn('productionBatch', `stageBatch_${TaktEcBatchStageCodes.Mfg1Production}`, 120),
    ]),
    groupColumn('qc', 'qc', [
      leafColumn('sampleDate', `stageDate_${TaktEcBatchStageCodes.SampleInspection}`, 110),
    ]),
    groupColumn('me', 'me', [
      leafColumn('sopDate', `stageDate_${TaktEcBatchStageCodes.TechSop}`, 110),
    ]),
  ];
  base.push({
    ...CreateActionColumn<BatchTableRow>({
      actions: [
        {
          key: 'detail',
          label: t('common.page.button.detail'),
          shape: 'plain',
          icon: RiEyeLine,
          permission: 'logistics:manufacturing:engineering:change:kanban:list',
          buttonClass: 'takt-button-detail',
          onClick: (record) => handleDetail(record),
        }],
    }),
    customCell: identityCell,
  });
  return base;
});

/** 可见列 keys */
const visibleColumnKeys = computed(() => {
  const all = columns.value.map((column) => String(column.key ?? '')).filter((key) => key.length > 0);
  if (userVisibleColumnKeys.value == null) {
    return all;
  }
  const allowed = new Set(all);
  const kept = userVisibleColumnKeys.value.filter((key) => allowed.has(key));
  const known = new Set(userVisibleColumnKeys.value);
  return [...kept, ...all.filter((key) => !known.has(key))];
});

/** 表格行：按各部门执行行数展开 */
const tableRows = computed(() => expandBatchRows(transposedResult.value?.paged?.data ?? []));

/**
 * 行主键：设变号码加展开行号
 * @param record 行数据
 * @returns 行主键
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.rowKey ?? record.ecCode ?? record.ecDetailId ?? '');
}

/**
 * 是否为部门日期或批次列
 * @param key 列键
 * @returns 是否阶段列
 */
function isStageColumn(key: unknown): boolean {
  const text = String(key ?? '');
  return text.startsWith('stageDate_') || text.startsWith('stageBatch_');
}

/**
 * 一条执行行的日期或批次。该部门没有这一行时留空
 * @param line 执行行
 * @param isDate 是否日期列
 * @returns 展示文本
 */
function stageLineText(line: EcExecBatchTransposedStageLine | undefined, isDate: boolean): string {
  if (!line) {
    return '';
  }
  if (isDate) {
    if (line.dateDisplayText) {
      return line.dateDisplayText;
    }
    return line.stageDate ? String(line.stageDate).slice(0, 10).replace(/-/g, '') : '—';
  }
  return line.batchCode?.trim() ? line.batchCode : '—';
}

/**
 * 一个设变号按各部门执行行数展开。行数取最大的那个部门
 * @param rows 接口行
 * @returns 表格行
 */
function expandBatchRows(rows: EcExecBatchTransposed[]): BatchTableRow[] {
  const lines: BatchTableRow[] = [];
  for (const row of rows) {
    const groups = TaktEcBatchStageTransposedOrder.map((code) => row.stageCells?.[code]?.lines ?? []);
    const lineCount = Math.max(1, ...groups.map((group) => group.length));
    for (let index = 0; index < lineCount; index += 1) {
      const line: BatchTableRow = {
        rowKey: `${row.ecCode}-${index}`,
        lineIndex: index,
        lineCount,
        ecDetailId: row.ecDetailId,
        ecCode: row.ecCode,
        technicalLiaisonNo: row.technicalLiaisonNo ?? '',
        pNo: row.pNo ?? '',
        tcjLiaisonNo: row.tcjLiaisonNo ?? '',
        ecIssueDate: row.ecIssueDate,
        ecModelCode: row.ecModelCode,
        ecNewMaterialCode: row.ecNewMaterialCode ?? '',
        ecEntryDate: row.ecEntryDate,
      };
      TaktEcBatchStageTransposedOrder.forEach((code, deptIndex) => {
        const item = groups[deptIndex]?.[index];
        line[`stageDate_${code}`] = stageLineText(item, true);
        line[`stageBatch_${code}`] = stageLineText(item, false);
      });
      lines.push(line);
    }
  }
  return lines;
}

/**
 * 构建查询参数
 * @returns 查询 DTO
 */
function buildQueryParams(): EcExecBatchTransposedQuery {
  const form = advancedQueryForm.value;
  const query: EcExecBatchTransposedQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
  };
  const keyword = queryKeyword.value.trim();
  if (keyword) {
    query.keyWords = keyword;
  }
  const textKeys = ['ecCode', 'ecModelCode', 'ecNewMaterialCode', 'batchCode'] as const;
  for (const key of textKeys) {
    const value = form[key].trim();
    if (value) {
      query[key] = value;
    }
  }
  if (form.ecIssueDateStart) {
    query.ecIssueDateStart = form.ecIssueDateStart;
  }
  if (form.ecIssueDateEnd) {
    query.ecIssueDateEnd = form.ecIssueDateEnd;
  }
  const entryDates = entryPeriodToQuery(entryPeriod.value);
  if (entryDates.ecEntryDateStart) {
    query.ecEntryDateStart = entryDates.ecEntryDateStart;
  }
  if (entryDates.ecEntryDateEnd) {
    query.ecEntryDateEnd = entryDates.ecEntryDateEnd;
  }
  return query;
}

/**
 * 录入期间转成技术主表日期起止。结束日取该月最后一天
 * @param range 年月区间
 * @returns 录入日期起止
 */
function entryPeriodToQuery(range: [string, string] | null): { ecEntryDateStart?: string; ecEntryDateEnd?: string } {
  if (!range?.[0] || !range?.[1]) {
    return {};
  }
  const parts = range[1].split('-').map(Number);
  const year = parts[0];
  const month = parts[1];
  if (!year || !month) {
    return { ecEntryDateStart: `${range[0]}-01` };
  }
  const lastDay = new Date(year, month, 0).getDate();
  return {
    ecEntryDateStart: `${range[0]}-01`,
    ecEntryDateEnd: `${range[1]}-${String(lastDay).padStart(2, '0')}`,
  };
}

/** 打开高级查询 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true;
}

/** 提交高级查询 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false;
  currentPage.value = 1;
  loadData();
}

/** 重置高级查询 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm();
  currentPage.value = 1;
  loadData();
}

/** 打开列设置 */
function handleColumnSetting() {
  columnSettingVisible.value = true;
}

/**
 * 列设置勾选变更
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  userVisibleColumnKeys.value = keys;
}

/** 列设置恢复默认 */
function handleColumnSettingReset() {
  userVisibleColumnKeys.value = null;
}

/** 加载转置列表 */
async function loadData() {
  loading.value = true;
  try {
    const result = await getEcBatchTransposedList(buildQueryParams());
    transposedResult.value = result;
    total.value = result?.paged?.total ?? 0;
  } finally {
    loading.value = false;
  }
}

/** 搜索 */
function handleSearch() {
  currentPage.value = 1;
  loadData();
}

/** 重置 */
function handleReset() {
  queryKeyword.value = '';
  entryPeriod.value = buildDefaultThroughCurrentPeriodRange(3);
  advancedQueryForm.value = createEmptyAdvancedQueryForm();
  currentPage.value = 1;
  loadData();
}

/** 分页变化 */
function handlePaginationChange() {
  loadData();
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽变化 */
function handleResizeColumn() {}

/**
 * 打开设变明细详情
 * @param record 展开行
 */
async function handleDetail(record: { ecDetailId: string }) {
  detailVisible.value = true;
  detailLoading.value = true;
  detailData.value = null;
  try {
    detailData.value = await getEcDetailById(record.ecDetailId);
  } finally {
    detailLoading.value = false;
  }
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcBatchData(buildQueryParams());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('logistics.manufacturing.engineering-change.ec-kanban.page.tabs.batch')}.xlsx`;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    setTimeout(() => window.URL.revokeObjectURL(url), 100);
  } finally {
    loading.value = false;
  }
}

useTableRefresh(loadData);
onMounted(loadData);
</script>

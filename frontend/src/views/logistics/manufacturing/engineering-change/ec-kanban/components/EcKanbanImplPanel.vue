<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban/components -->
<!-- 文件名称：EcKanbanImplPanel.vue -->
<!-- 功能描述：设变看板·实施页签：按各部门执行行展开，已实施的格显示勾 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="box-border flex w-full min-w-0 max-w-full flex-col overflow-x-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
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
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecTeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="false"
      :row-key="getEcId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ecIssueDate' || column.key === 'ecEntryDate'">
          {{ formatIssueDate(record[column.key]) }}
        </template>
        <template v-else-if="column.key === 'ecLeader'">
          <TaktDictTag
            v-if="record.ecLeader"
            :value="record.ecLeader"
            :option="ecGroupOption(record.ecLeader)"
          />
        </template>
        <template v-else-if="isDeptColumn(column.key)">
          <span v-if="lineChecked(record, column.key)" class="text-lg leading-none">√</span>
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-kanban"
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
        <div v-show="isFieldVisible('changeStatus')">
          <a-form-item :label="gi.label('changeStatus')">
            <TaktSelect v-model:value="advancedQueryForm.changeStatus" dict-type="logistics_manufacturing_ec_status" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecStatus')">
          <a-form-item :label="gi.label('ecStatus')">
            <TaktSelect v-model:value="advancedQueryForm.ecStatus" dict-type="logistics_manufacturing_ec_te_status" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('cultureCode')">
          <a-form-item :label="t('common.page.entity.culturecode')">
            <TaktSelect v-model:value="advancedQueryForm.cultureCode" dict-type="sys_culture_code" allow-clear />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="ecTeId"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变看板实施页签：按各部门执行行展开，已实施的格显示勾
 */
import '../../ec-list-layout.css'
import { useI18n } from 'vue-i18n';
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit';
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod';
import { getEcKanbanList, exportEcKanbanData } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api';
import { getEcGroupOptions } from '@/api/logistics/manufacturing/engineering-change/ecGroup.api';
import type { TaktSelectOption } from '@/types/common.types';
import type { EcKanban } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types';
import { useEcTeI18n } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcTeI18n';

const { t } = useI18n();
const gi = useEcTeI18n();
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page';
/** 列表 loading */
const loading = ref(false);
/** 单张设变最多展开的行数，避免超大设变把页面撑爆 */
const KANBAN_MAX_EXPAND_LINES = 6000
/** 机种、成品单元格最长字符，避免把整张明细拼进每一行 */
const KANBAN_LABEL_MAX_CHARS = 240
/** 展开后的一行：同一设变号按各部门最大执行行数展开 */
interface KanbanTableRow {
  rowKey: string
  lineIndex: number
  lineCount: number
  ecIssueDate: string
  ecLeader: string
  ecCode: string
  ecModelCode: string
  ecRootMaterialCode: string
  ecEntryDate: string
  [key: string]: string | number
}
/** 展开后的看板行 */
const dataSource = ref<KanbanTableRow[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词，放在查询和重置之前 */
const queryKeyword = ref('');
/** 录入期间，截止本月 */
const entryPeriod = ref<[string, string] | null>(buildDefaultThroughCurrentPeriodRange(3));
/** 录入期间可选到本月，最多 12 个月 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true });
/** 列设置抽屉 */
const columnSettingVisible = ref(false);
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false);
/** 高级查询字段 */
const KANBAN_QUERY_FIELDS = [
  'ecCode',
  'changeStatus',
  'ecStatus',
  'cultureCode',
] as const;
/** 执行部门列。该部门这一条明细已实施时显示勾 */
const KANBAN_DEPT_COLUMNS = [
  { key: 'pcImplemented', title: 'pc' },
  { key: 'purImplemented', title: 'pur' },
  { key: 'iqcImplemented', title: 'iqc' },
  { key: 'mcImplemented', title: 'mc' },
  { key: 'mtlImplemented', title: 'mtl' },
  { key: 'mfg2Implemented', title: 'mfg2' },
  { key: 'mfg1Implemented', title: 'mfg1' },
  { key: 'qcImplemented', title: 'qc' },
  { key: 'meImplemented', title: 'me' },
] as const;
/** 高级查询可见字段 */
const visibleQueryFieldKeys = ref<string[]>([]);
/**
 * 空的高级查询表单
 */
function createEmptyAdvancedQueryForm() {
  return {
    ecCode: '',
    cultureCode: '',
    changeStatus: undefined as number | undefined,
    ecStatus: undefined as number | undefined,
  };
}
/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm());
/** 高级查询字段标题 */
const queryFieldsMeta = computed(() => KANBAN_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'cultureCode'
        ? t('common.page.entity.culturecode')
        : gi.label(key),
})));
/** 选中行 keys */
const selecmedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selecmedRows = ref<KanbanTableRow[]>([]);
/** 列定义：设变信息在每一行重复，部门列按执行行逐行打勾 */
const columns = computed(() => [
  { title: t(`${localePrefix}.column.issueDate`), dataIndex: 'ecIssueDate', key: 'ecIssueDate', width: 120 },
  { title: t(`${localePrefix}.column.leader`), dataIndex: 'ecLeader', key: 'ecLeader', width: 110 },
  { title: t(`${localePrefix}.column.ecCode`), dataIndex: 'ecCode', key: 'ecCode', width: 130 },
  { title: t(`${localePrefix}.column.model`), dataIndex: 'ecModelCode', key: 'ecModelCode', width: 120 },
  { title: t(`${localePrefix}.column.finishedProduct`), dataIndex: 'ecRootMaterialCode', key: 'ecRootMaterialCode', width: 150 },
  { title: t(`${localePrefix}.column.tech`), dataIndex: 'ecEntryDate', key: 'ecEntryDate', width: 120 },
  ...KANBAN_DEPT_COLUMNS.map((column) => ({
    title: t(`${localePrefix}.column.${column.title}`),
    dataIndex: column.key,
    key: column.key,
    width: 88,
    align: 'center' as const,
  })),
]);
/** 可见列 keys */
const KANBAN_DEFAULT_COLUMN_KEYS = [
  'ecIssueDate',
  'ecLeader',
  'ecCode',
  'ecModelCode',
  'ecRootMaterialCode',
  'ecEntryDate',
  ...KANBAN_DEPT_COLUMNS.map((column) => column.key),
];
const visibleColumnKeys = ref<string[]>([...KANBAN_DEFAULT_COLUMN_KEYS]);
/** 行选择 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcKanban[]) => {
    selecmedRowKeys.value = keys;
    selecmedRows.value = rows;
  },
}));

/** 设变组编码 → 选项（设变担当 EcLeader 显示名称） */
const ecGroupOptionByCode = ref<Record<string, TaktSelectOption>>({});

/**
 * 设变担当对应的设变组选项
 * @param value 设变组编码
 * @returns 选项；无映射时 undefined，标签回退编码
 */
function ecGroupOption(value: unknown): TaktSelectOption | undefined {
  if (value == null || value === '') {
    return undefined;
  }
  return ecGroupOptionByCode.value[String(value)];
}

/** 预加载设变组选项，供设变担当列编码转名称 */
async function loadEcGroupOptions(): Promise<void> {
  try {
    const list = await getEcGroupOptions();
    const map: Record<string, TaktSelectOption> = {};
    for (const item of list ?? []) {
      if (item.dictValue != null && String(item.dictValue) !== '') {
        map[String(item.dictValue)] = item;
      }
    }
    ecGroupOptionByCode.value = map;
  } catch {
    ecGroupOptionByCode.value = {};
  }
}

/**
 * 是否为部门实施列
 * @param key 列键
 * @returns 是否部门列
 */
function isDeptColumn(key: unknown): boolean {
  return KANBAN_DEPT_COLUMNS.some((column) => column.key === key);
}

/**
 * 取出一个部门的实施标记
 * @param value 接口返回的标记列表
 * @returns 0 或 1
 */
function deptFlags(value: unknown): number[] {
  if (!Array.isArray(value)) {
    return [];
  }
  return value.filter((item): item is number => item === 0 || item === 1);
}

/**
 * 这一条部门明细是否打勾
 * @param record 展开行
 * @param key 列键
 * @returns 是否打勾
 */
function lineChecked(record: Record<string, unknown>, key: unknown): boolean {
  return record[String(key)] === 1
}

/**
 * 截断机种或成品文本
 * @param value 接口文本
 * @returns 不超过上限的文本
 */
function boundedLabel(value: unknown): string {
  const text = String(value ?? '')
  return text.length > KANBAN_LABEL_MAX_CHARS ? `${text.slice(0, KANBAN_LABEL_MAX_CHARS)}…` : text
}

/**
 * 一个设变号按各部门执行行数展开。行数取最多的那个部门，并限制上限
 * @param rows 接口行
 * @returns 表格行
 */
function expandKanbanRows(rows: EcKanban[]): KanbanTableRow[] {
  const lines: KanbanTableRow[] = []
  for (const row of rows) {
    const groups = KANBAN_DEPT_COLUMNS.map((column) => deptFlags(row[column.key]))
    const lineCount = Math.min(KANBAN_MAX_EXPAND_LINES, Math.max(1, ...groups.map((group) => group.length)))
    const modelCode = boundedLabel(row.ecModelCode)
    const rootMaterialCode = boundedLabel(row.ecRootMaterialCode)
    for (let index = 0; index < lineCount; index += 1) {
      const line: KanbanTableRow = {
        rowKey: `${row.ecCode}-${index}`,
        lineIndex: index,
        lineCount,
        ecIssueDate: row.ecIssueDate ?? '',
        ecLeader: row.ecLeader ?? '',
        ecCode: row.ecCode ?? '',
        ecModelCode: modelCode,
        ecRootMaterialCode: rootMaterialCode,
        ecEntryDate: row.ecEntryDate ?? '',
      }
      KANBAN_DEPT_COLUMNS.forEach((column, groupIndex) => {
        const flags = groups[groupIndex] ?? []
        line[column.key] = index < flags.length ? flags[index] : ''
      })
      lines.push(line)
    }
  }
  return lines
}

/**
 * 发行日期只显示年月日
 * @param value 发行日期
 * @returns yyyy-MM-dd
 */
function formatIssueDate(value: unknown): string {
  const text = String(value ?? '');
  return text.length >= 10 ? text.slice(0, 10) : text;
}

/**
 * 行主键：设变号加展开行序号
 * @param record 行数据
 * @returns 行主键
 */
function getEcId(record: Record<string, unknown>) {
  return String(record.rowKey ?? record.ecCode ?? '');
}

/**
 * 构建列表查询参数
 * @returns {object} 查询 DTO
 */
function buildQueryParams() {
  const form = advancedQueryForm.value;
  return {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    keyWords: queryKeyword.value || undefined,
    ...entryPeriodToQuery(entryPeriod.value),
    ecCode: form.ecCode.trim() || undefined,
    cultureCode: form.cultureCode.trim() || undefined,
    changeStatus: form.changeStatus === undefined || form.changeStatus === null ? undefined : Number(form.changeStatus),
    ecStatus: form.ecStatus === undefined || form.ecStatus === null ? undefined : Number(form.ecStatus),
  };
}

/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKanbanList(buildQueryParams());
    dataSource.value = expandKanbanRows(res.data ?? []);
    total.value = res.total ?? 0;
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

/** 重置（关键字由 TaktQueryBar 清空） */
function handleReset() {
  entryPeriod.value = buildDefaultThroughCurrentPeriodRange(3);
  advancedQueryForm.value = createEmptyAdvancedQueryForm();
  currentPage.value = 1;
  loadData();
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
  visibleColumnKeys.value = keys;
}

/** 列设置恢复默认 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [...KANBAN_DEFAULT_COLUMN_KEYS];
}

/** 分页变化 */
function handlePaginationChange() {
  loadData();
}

/** 表格变化 */
function handleTableChange() {}

/** 列宽变化 */
function handleResizeColumn() {}

/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return {
    title: String(record.ecCode ?? ''),
    onClick: () => {
      const id = getEcId(record);
      selecmedRowKeys.value = [id];
      selecmedRows.value = [record as KanbanTableRow];
    },
  };
}

/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcKanbanData(buildQueryParams());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.kanban')}.xlsx`;
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
onMounted(() => {
  void loadEcGroupOptions();
  void loadData();
});
</script>

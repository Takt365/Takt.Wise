<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kanban -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变实施跟踪看板：执行路径、当前卡点部门、实施状态筛选 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="t('common.page.form.placeholder.search')"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    >
      <template #fields>
        <a-select
          v-model:value="advancedQueryForm.currentDeptCode"
          allow-clear
          :placeholder="execI18n.label('deptCode')"
          :options="deptFilterOptions"
          @change="handleFilterChange"
        />
        <a-select
          v-model:value="advancedQueryForm.implementationStatus"
          allow-clear
          :placeholder="t(`${localePrefix}.filter.implementationStatus`)"
          :options="statusFilterOptions"
          @change="handleFilterChange"
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
      :id-column-key="'ecGijutsuId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'implementationStatus'">
          <a-tag :color="implementationStatusColor(record.implementationStatus)">
            {{ implementationStatusLabel(record.implementationStatus) }}
          </a-tag>
        </template>
        <template v-else-if="column.key === 'deptImplementation'">
          <span class="inline-flex flex-wrap items-center gap-1" :title="formatDeptImplementation(record as EcKanban)">
            <template v-for="(part, index) in buildDeptImplementationParts(record as EcKanban)" :key="part.key">
              <span v-if="index > 0">-></span>
              <a-tag class="!mr-0" :class="part.colorClass">
                {{ part.label }}(RC:<span :class="part.matched ? 'text-[var(--ant-color-success)]' : 'text-[var(--ant-color-error)]'">{{ part.total }}</span>-EC:<span :class="part.matched ? 'text-[var(--ant-color-success)]' : 'text-[var(--ant-color-error)]'">{{ part.done }}</span>)
              </a-tag>
            </template>
          </span>
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
            <TaktSelect v-model:value="advancedQueryForm.ecStatus" dict-type="logistics_manufacturing_ec_gijutsu_status" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('currentDeptCode')">
          <a-form-item :label="execI18n.label('deptCode')">
            <a-select v-model:value="advancedQueryForm.currentDeptCode" allow-clear :options="deptFilterOptions" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('implementationStatus')">
          <a-form-item :label="t(`${localePrefix}.filter.implementationStatus`)">
            <a-select v-model:value="advancedQueryForm.implementationStatus" allow-clear :options="statusFilterOptions" />
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
      id-column-key="ecGijutsuId"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变实施跟踪看板：按 8 张部门执行表汇总路径，品管课完成即正式完成
 */
import { useI18n } from 'vue-i18n';
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n';
import { taktOrgDeptI18nKey } from '@/utils/naming';
import { resolveTaktTagColorClass } from '@/utils/taktTagColor';
import { getEcKanbanList, exportEcKanbanData } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api';
import { TaktEcKanbanOrder } from '@/constants/logistics/ecExecCodes.constants';
import { TaktEcDeptCodes } from '@/constants/logistics/ecDeptCodes.constants';
import { TaktEcImplementationStatus } from '@/constants/logistics/ecImplementationStatus.constants';
import type { EcKanban } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types';
import { useEcGijutsuI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcGijutsuI18n';

const { t } = useI18n();
const gi = useEcGijutsuI18n();
/** 公共执行字段 entity.ecexec.* */
const execI18n = useEntityFieldI18n('ecexec');
const localePrefix = 'logistics.manufacturing.engineering-change.ec-kanban.page';
/** 列表 loading */
const loading = ref(false);
/** 数据源 */
const dataSource = ref<EcKanban[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词 */
const queryKeyword = ref('');
/** 列设置抽屉 */
const columnSettingVisible = ref(false);
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false);
/** 高级查询字段 */
const KANBAN_QUERY_FIELDS = [
  'ecCode',
  'changeStatus',
  'ecStatus',
  'currentDeptCode',
  'implementationStatus',
  'cultureCode',
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
    currentDeptCode: undefined as string | undefined,
    implementationStatus: undefined as number | undefined,
  };
}
/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm());
/** 高级查询字段标题 */
const queryFieldsMeta = computed(() => KANBAN_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'currentDeptCode'
    ? execI18n.label('deptCode')
    : key === 'implementationStatus'
      ? t(`${localePrefix}.filter.implementationStatus`)
      : key === 'cultureCode'
        ? t('common.page.entity.culturecode')
        : gi.label(key),
})));
/** 选中行 keys */
const selectedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selectedRows = ref<EcKanban[]>([]);
/** 部门筛选项 */
const deptFilterOptions = computed(() =>
  TaktEcKanbanOrder.map((code) => ({ value: code, label: deptLabel(code) })));
/** 状态筛选项 */
const statusFilterOptions = computed(() => [
  { value: TaktEcImplementationStatus.NotStarted, label: t(`${localePrefix}.implementationStatus.notStarted`) },
  { value: TaktEcImplementationStatus.InProgress, label: t(`${localePrefix}.implementationStatus.inProgress`) },
  { value: TaktEcImplementationStatus.OfficiallyCompleted, label: t(`${localePrefix}.implementationStatus.officiallyCompleted`) },
  { value: TaktEcImplementationStatus.FullyCompleted, label: t(`${localePrefix}.implementationStatus.fullyCompleted`) }]);
/** 列定义 */
const columns = computed(() => [
  { title: gi.label('ecCode'), dataIndex: 'ecCode', key: 'ecCode', width: 120 },
  { title: gi.label('ecTitle'), dataIndex: 'ecTitle', key: 'ecTitle', width: 200 },
  { title: t(`${localePrefix}.column.implementationStatus`), dataIndex: 'implementationStatus', key: 'implementationStatus', width: 110 },
  { title: t(`${localePrefix}.column.pendingCount`), dataIndex: 'pendingAtCurrentDeptCount', key: 'pendingAtCurrentDeptCount', width: 90 },
  { title: t(`${localePrefix}.column.detailCount`), dataIndex: 'detailCount', key: 'detailCount', width: 80 },
  { title: t(`${localePrefix}.column.implementation`), dataIndex: 'deptImplementation', key: 'deptImplementation', width: 720, ellipsis: true },
  { title: gi.label('ecLeader'), dataIndex: 'ecLeader', key: 'ecLeader', width: 100 }]);
/** 可见列 keys */
const KANBAN_DEFAULT_COLUMN_KEYS = [
  'ecCode',
  'ecTitle',
  'implementationStatus',
  'pendingAtCurrentDeptCount',
  'detailCount',
  'deptImplementation',
  'ecLeader',
];
const visibleColumnKeys = ref<string[]>([...KANBAN_DEFAULT_COLUMN_KEYS]);
/** 行选择 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcKanban[]) => {
    selectedRowKeys.value = keys;
    selectedRows.value = rows;
  },
}));

/**
 * 部门显示名
 * @param code 部门编码
 * @returns {string} 文案
 */
function deptLabel(code: string): string {
  const key = taktOrgDeptI18nKey(code);
  return key ? t(key) : '';
}

/** 路径与实施列顺序（生管->采购->受检->部管->物料->制二课->制一课->品管课） */
const KANBAN_PATH_STEPS = [
  { code: TaktEcDeptCodes.Pmc, key: 'pmc' },
  { code: TaktEcDeptCodes.Mp, key: 'mp' },
  { code: TaktEcDeptCodes.Iqc, key: 'iqc' },
  { code: TaktEcDeptCodes.Mc, key: 'mc' },
  { code: TaktEcDeptCodes.Smt, key: 'smt' },
  { code: TaktEcDeptCodes.Pcba, key: 'pcba' },
  { code: TaktEcDeptCodes.Assy, key: 'assy' },
  { code: TaktEcDeptCodes.Qa, key: 'qa' },
] as const;

/**
 * 路径：固定为截图顺序
 * @returns {string} 路径文本
 */
function formatDeptPath(): string {
  return KANBAN_PATH_STEPS.map((step) => t(`${localePrefix}.pathLabel.${step.key}`)).join('->');
}

/**
 * 实施列分段。行数与实施数只给数字上色：相等为绿，不相等为红
 * @param record 看板行
 * @returns {Array<{ key: string, label: string, total: number, done: number, matched: boolean, colorClass: string }>} 各部门片段
 */
function buildDeptImplementationParts(record: EcKanban): Array<{
  key: string
  label: string
  total: number
  done: number
  matched: boolean
  colorClass: string
}> {
  return KANBAN_PATH_STEPS.map((step, index) => {
    const stage = record.deptStages?.find((item) => item.deptCode === step.code);
    const total = stage?.totalCount ?? 0;
    const done = stage?.implementedCount ?? 0;
    return {
      key: step.key,
      label: t(`${localePrefix}.implLabel.${step.key}`),
      total,
      done,
      matched: total === done,
      colorClass: resolveTaktTagColorClass(index),
    };
  });
}

/**
 * 实施：生管课(RC:1-EC:1)->采购课(RC:3-EC:0)
 * @param record 看板行
 * @returns {string} 实施文本
 */
function formatDeptImplementation(record: EcKanban): string {
  return buildDeptImplementationParts(record)
    .map((part) => `${part.label}(RC:${part.total}-EC:${part.done})`)
    .join('->');
}

/**
 * 实施状态文案
 * @param status 状态值
 * @returns {string} 文案
 */
function implementationStatusLabel(status: number): string {
  switch (status) {
    case TaktEcImplementationStatus.NotStarted:
      return t(`${localePrefix}.implementationStatus.notStarted`);
    case TaktEcImplementationStatus.InProgress:
      return t(`${localePrefix}.implementationStatus.inProgress`);
    case TaktEcImplementationStatus.OfficiallyCompleted:
      return t(`${localePrefix}.implementationStatus.officiallyCompleted`);
    case TaktEcImplementationStatus.FullyCompleted:
      return t(`${localePrefix}.implementationStatus.fullyCompleted`);
    default:
      return String(status);
  }
}

/**
 * 实施状态标签色
 * @param status 状态值
 * @returns {string} Ant Design 颜色
 */
function implementationStatusColor(status: number): string {
  switch (status) {
    case TaktEcImplementationStatus.NotStarted:
      return 'default';
    case TaktEcImplementationStatus.InProgress:
      return 'processing';
    case TaktEcImplementationStatus.OfficiallyCompleted:
      return 'success';
    case TaktEcImplementationStatus.FullyCompleted:
      return 'green';
    default:
      return 'default';
  }
}

/**
 * 行主键
 * @param record 行数据
 * @returns {string} ecGijutsuId
 */
function getEcId(record: Record<string, unknown>) {
  return String(record.ecGijutsuId ?? '');
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
    ecCode: form.ecCode.trim() || undefined,
    cultureCode: form.cultureCode.trim() || undefined,
    changeStatus: form.changeStatus === undefined || form.changeStatus === null ? undefined : Number(form.changeStatus),
    ecStatus: form.ecStatus === undefined || form.ecStatus === null ? undefined : Number(form.ecStatus),
    currentDeptCode: form.currentDeptCode || undefined,
    implementationStatus: form.implementationStatus === undefined || form.implementationStatus === null
      ? undefined
      : Number(form.implementationStatus),
  };
}

/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKanbanList(buildQueryParams());
    dataSource.value = res.data ?? [];
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
function handleReset() {
  queryKeyword.value = '';
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

/** 筛选变化 */
function handleFilterChange() {
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

/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return {
    title: `${t(`${localePrefix}.column.path`)}：${formatDeptPath()}`,
    onClick: () => {
      const id = getEcId(record);
      selectedRowKeys.value = [id];
      selectedRows.value = [record as unknown as EcKanban];
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
onMounted(loadData);
</script>

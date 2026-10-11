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
          v-model:value="filterCurrentDeptCode"
          allow-clear
          :placeholder="execI18n.label('deptCode')"
          :options="deptFilterOptions"
          @change="handleFilterChange"
        />
        <a-select
          v-model:value="filterImplementationStatus"
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
      export-permission="logistics:manufacturing:engineering:change:kanban:export"
      :refresh-loading="loading"
      @export="handleExport"
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
        <template v-else-if="column.key === 'currentDeptCode'">
          <span v-if="record.currentDeptCode">{{ deptLabel(record.currentDeptCode) }}</span>
          <span v-else class="text-text-secondary">—</span>
        </template>
        <template v-else-if="column.key === 'deptPath'">
          <EcKanbanStageStrip
            :dept-stages="record.deptStages ?? []"
            :current-dept-code="record.currentDeptCode"
          />
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变实施跟踪看板：按 8 张部门执行表汇总路径，品管课完成即正式完成
 */
import { useI18n } from 'vue-i18n';
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n';
import { taktOrgDeptI18nKey } from '@/utils/naming';
import { getEcKanbanList, exportEcKanbanData } from '@/api/logistics/manufacturing/engineering-change/ecKanban.api';
import { TaktEcKanbanOrder } from '@/constants/logistics/ecExecCodes.constants';
import { TaktEcImplementationStatus } from '@/constants/logistics/ecImplementationStatus.constants';
import type { EcKanban } from '@/types/logistics/manufacturing/engineering-change/ecKanban.types';
import { useEcGijutsuI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcGijutsuI18n';
import EcKanbanStageStrip from './components/EcKanbanStageStrip.vue';

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
/** 当前卡点部门筛选 */
const filterCurrentDeptCode = ref<string | undefined>(undefined);
/** 实施状态筛选 */
const filterImplementationStatus = ref<number | undefined>(undefined);
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
  { title: execI18n.label('deptCode'), dataIndex: 'currentDeptCode', key: 'currentDeptCode', width: 100 },
  { title: t(`${localePrefix}.column.pendingCount`), dataIndex: 'pendingAtCurrentDeptCount', key: 'pendingAtCurrentDeptCount', width: 90 },
  { title: t(`${localePrefix}.column.detailCount`), dataIndex: 'detailCount', key: 'detailCount', width: 80 },
  { title: t(`${localePrefix}.column.path`), dataIndex: 'deptPath', key: 'deptPath', width: 420 },
  { title: gi.label('ecLeader'), dataIndex: 'ecLeader', key: 'ecLeader', width: 100 }]);
/** 可见列 keys */
const visibleColumnKeys = ref([
  'ecCode', 'ecTitle', 'implementationStatus', 'currentDeptCode', 'pendingAtCurrentDeptCount', 'detailCount', 'deptPath', 'ecLeader']);
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
  return {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    keyWords: queryKeyword.value || undefined,
    currentDeptCode: filterCurrentDeptCode.value || undefined,
    implementationStatus: filterImplementationStatus.value,
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
  filterCurrentDeptCode.value = undefined;
  filterImplementationStatus.value = undefined;
  currentPage.value = 1;
  loadData();
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

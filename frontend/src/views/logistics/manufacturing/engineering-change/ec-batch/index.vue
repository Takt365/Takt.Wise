<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-batch -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变投入批次转置列表（行=设变明细，列=各阶段日期+批次） -->
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
    />
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
      export-permission="logistics:manufacturing:engineering:change:batch:export"
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
            <a-typography-link @click.stop="handleDetail(record as EcExecBatchTransposed)">
              {{ record.ecCode }}
            </a-typography-link>
          </template>
          <template v-else-if="String(column.key ?? '').startsWith('stageDate_')">
            <span>{{ formatStageDate(record, String(column.key)) }}</span>
          </template>
          <template v-else-if="String(column.key ?? '').startsWith('stageBatch_')">
            <span>{{ formatStageBatch(record, String(column.key)) }}</span>
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
      :title="t('common.dialog.title.detail', { entity: t('menu.logistics.manufacturing.engineering.change.batch') })"
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
          <a-descriptions-item :label="gi.label('ecEntryDate')">{{ detailData.ecEntryDate ?? '—' }}</a-descriptions-item>
        </a-descriptions>
      </a-spin>
    </TaktModal>
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-batch"
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
          <a-form-item :label="seikan.label('scheduledBatch')">
            <a-input v-model:value="advancedQueryForm.batchCode" allow-clear :maxlength="40" />
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
 * 设变投入批次转置列表页
 */
import { RiEyeLine } from '@remixicon/vue';
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { useEntityFieldI18n } from '@/composables/useEntityFieldI18n';
import { getEcBatchTransposedList } from '@/api/logistics/manufacturing/engineering-change/ecBatch.api';
import { getEcDetailById } from '@/api/logistics/manufacturing/engineering-change/ecDetail.api';
import { exportEcBatchData } from '@/api/logistics/manufacturing/engineering-change/ecBatch.api';
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index';
import { TaktEcBatchStageCodes } from '@/constants/logistics/ecBatchStageCodes.constants';
import { TaktEcDeptCodes } from '@/constants/logistics/ecDeptCodes.constants';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types';
import type {
  EcExecBatchTransposed,
  EcExecBatchTransposedQuery,
  EcExecBatchTransposedResult,
} from '@/types/logistics/manufacturing/engineering-change/ecExecTransposed.types';
import { taktOrgDeptI18nKey } from '@/utils/naming';
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n';
import { useEcGijutsuI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcGijutsuI18n';

const { t } = useI18n();
const pi = useEcDetailI18n();
const gi = useEcGijutsuI18n();
/** 生管 entity.ecseikan.* */
const seikan = useEntityFieldI18n('ecseikan');
/** 部管 entity.ecbukan.* */
const bukan = useEntityFieldI18n('ecbukan');
/** 制二 entity.ecseizounika.* */
const nika = useEntityFieldI18n('ecseizounika');
/** 制一 entity.ecseizouikka.* */
const ikka = useEntityFieldI18n('ecseizouikka');
/** 品管 entity.echinkan.* */
const hinkan = useEntityFieldI18n('echinkan');
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
/** 关键词 */
const queryKeyword = ref('');
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
      ? seikan.label('scheduledBatch')
      : pi.label(key),
})));

/**
 * 部门显示名（org.dept.{编码}）
 * @param code 部门编码
 * @returns {string} 文案
 */
function deptLabel(code: string): string {
  const key = taktOrgDeptI18nKey(code);
  return key ? t(key) : '';
}

/**
 * 附件类别列标题（字典 dict.logistics.manufacturing.ec.attachment.type.*）
 * @param typeKey 字典末段 tl / fpp / tcj
 * @returns {string} 文案
 */
function attachmentTypeTitle(typeKey: 'tl' | 'fpp' | 'tcj'): string {
  return t(`dict.logistics.manufacturing.ec.attachment.type.${typeKey}`);
}

/**
 * 部门名 + 实体字段标签
 * @param deptCode 部门编码
 * @param fieldLabel entity.* 字段文案
 * @returns {string} 列标题
 */
function deptFieldTitle(deptCode: string, fieldLabel: string): string {
  return `${deptLabel(deptCode)} ${fieldLabel}`.trim();
}

/**
 * 阶段列标题（org.dept.* + entity.{deptSlug}.*）
 * @param stageCode 阶段编码
 * @param kind 日期或批次
 * @returns {string} 列标题
 */
function stageColumnTitle(stageCode: string, kind: 'date' | 'batch'): string {
  const map: Record<string, { date: string; batch: string }> = {
    [TaktEcBatchStageCodes.Scheduled]: {
      date: deptFieldTitle(TaktEcDeptCodes.Pmc, seikan.label('scheduledDate')),
      batch: deptFieldTitle(TaktEcDeptCodes.Pmc, seikan.label('scheduledBatch')),
    },
    [TaktEcBatchStageCodes.Outbound]: {
      date: deptFieldTitle(TaktEcDeptCodes.Mc, bukan.label('outboundDate')),
      batch: deptFieldTitle(TaktEcDeptCodes.Mc, bukan.label('outboundBatch')),
    },
    [TaktEcBatchStageCodes.PcbaProduction]: {
      date: deptFieldTitle(TaktEcDeptCodes.Pcba, nika.label('productionDate')),
      batch: deptFieldTitle(TaktEcDeptCodes.Pcba, nika.label('productionBatch')),
    },
    [TaktEcBatchStageCodes.AssyProduction]: {
      date: deptFieldTitle(TaktEcDeptCodes.Assy, ikka.label('productionDate')),
      batch: deptFieldTitle(TaktEcDeptCodes.Assy, ikka.label('implementationBatch')),
    },
    [TaktEcBatchStageCodes.SampleInspection]: {
      date: deptFieldTitle(TaktEcDeptCodes.Qa, hinkan.label('inspectionDate')),
      batch: '',
    },
  };
  const item = map[stageCode];
  if (!item) return stageCode;
  return kind === 'date' ? item.date : item.batch;
}

/** 动态列 */
const columns = computed(() => {
  const order = transposedResult.value?.stageCodeOrder ?? [];
  const base: TableColumnsType = [
    { title: gi.label('ecCode'), dataIndex: 'ecCode', key: 'ecCode', width: 110, fixed: 'left' as const },
    { title: attachmentTypeTitle('tl'), dataIndex: 'technicalLiaisonNo', key: 'technicalLiaisonNo', width: 110 },
    { title: attachmentTypeTitle('fpp'), dataIndex: 'pNo', key: 'pNo', width: 100 },
    { title: attachmentTypeTitle('tcj'), dataIndex: 'tcjLiaisonNo', key: 'tcjLiaisonNo', width: 120 },
    { title: gi.label('ecIssueDate'), dataIndex: 'ecIssueDate', key: 'ecIssueDate', width: 100 },
    { title: pi.label('ecModelCode'), dataIndex: 'ecModelCode', key: 'ecModelCode', width: 100 },
    { title: pi.label('ecNewMaterialCode'), dataIndex: 'ecNewMaterialCode', key: 'ecNewMaterialCode', width: 120 },
    { title: gi.label('ecEntryDate'), dataIndex: 'ecEntryDate', key: 'ecEntryDate', width: 100 }];
  order.forEach((stageCode) => {
    base.push({
      title: stageColumnTitle(stageCode, 'date'),
      dataIndex: `stageDate_${stageCode}`,
      key: `stageDate_${stageCode}`,
      width: 100,
    });
    if (stageCode !== TaktEcBatchStageCodes.SampleInspection) {
      base.push({
        title: stageColumnTitle(stageCode, 'batch'),
        dataIndex: `stageBatch_${stageCode}`,
        key: `stageBatch_${stageCode}`,
        width: 120,
      });
    }
  });
  base.push(
    CreateActionColumn<EcExecBatchTransposed>({
      actions: [
        {
          key: 'detail',
          label: t('common.page.button.detail'),
          shape: 'plain',
          icon: RiEyeLine,
          permission: 'logistics:manufacturing:engineering:change:batch:detail',
          buttonClass: 'takt-button-detail',
          onClick: (record) => handleDetail(record),
        }],
    }),
  );
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

/** 表格行（扁平化阶段列） */
const tableRows = computed(() => transposedResult.value?.paged?.data ?? []);

/**
 * 行主键
 * @param record 行数据
 * @returns ecDetailId
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}

/**
 * 阶段日期展示
 * @param record 行数据
 * @param columnKey 列 key
 * @returns 展示文本
 */
function formatStageDate(record: Record<string, unknown>, columnKey: string): string {
  const stageCode = columnKey.replace('stageDate_', '');
  const row = record as unknown as EcExecBatchTransposed;
  const cell = row.stageCells?.[stageCode];
  if (cell?.dateDisplayText) return cell.dateDisplayText;
  return cell?.stageDate ? String(cell.stageDate).slice(0, 10).replace(/-/g, '') : '';
}

/**
 * 阶段批次展示
 * @param record 行数据
 * @param columnKey 列 key
 * @returns 展示文本
 */
function formatStageBatch(record: Record<string, unknown>, columnKey: string): string {
  const stageCode = columnKey.replace('stageBatch_', '');
  const row = record as unknown as EcExecBatchTransposed;
  return row.stageCells?.[stageCode]?.batchCode ?? '';
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
  return query;
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
 * @param record 转置行
 */
async function handleDetail(record: EcExecBatchTransposed) {
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
    link.download = `${t('menu.logistics.manufacturing.engineering.change.batch')}.xlsx`;
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

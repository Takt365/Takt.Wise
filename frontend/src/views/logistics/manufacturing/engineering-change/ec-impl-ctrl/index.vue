<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-impl-ctrl -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变实施管控；页签为生管、制一、制二、品管 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-list-layout p-4">
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
      :show-update="true"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="false"
      :show-fullscreen="true"
      :show-refresh="true"
      update-permission="logistics:manufacturing:engineering:change:implctrl:update"
      export-permission="logistics:manufacturing:engineering:change:implctrl:export"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @refresh="loadData"
    />
    <TaktPaneTabs
      v-model:active-key="implCtrlTab"
      class="mb-3"
      hide-content
      :items="implCtrlTabItems"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="tableVisibleColumnKeys"
      :id-column-key="''"
      table-mode="single"
      scroll-layout="pageWithInnerTabs"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcDetailId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    <TaktModal v-model:open="formVisible" :title="t('common.dialog.title.edit', { entity: t('menu.logistics.manufacturing.engineering.change.implctrl') })" :width="formModalWidthPx" :confirm-loading="formLoading" @ok="handleFormSubmit">
      <ImplCtrlForm ref="formRef" :form-data="formData" :loading="formLoading" :dept-tab="implCtrlTab" />
    </TaktModal>
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-impl-ctrl"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
        <div v-show="isFieldVisible('ecCode')">
          <a-form-item :label="pi.queryLabel('ecCode')">
            <a-input v-model:value="advancedQueryForm.ecCode" :placeholder="pi.queryPh('ecCode', 'required')" allow-clear :maxlength="10" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecModelCode')">
          <a-form-item :label="pi.queryLabel('ecModelCode')">
            <a-input v-model:value="advancedQueryForm.ecModelCode" :placeholder="pi.queryPh('ecModelCode', 'required')" allow-clear :maxlength="40" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecOldMaterialCode')">
          <a-form-item :label="pi.queryLabel('ecOldMaterialCode')">
            <a-input v-model:value="advancedQueryForm.ecOldMaterialCode" :placeholder="pi.queryPh('ecOldMaterialCode', 'required')" allow-clear :maxlength="20" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecNewMaterialCode')">
          <a-form-item :label="pi.queryLabel('ecNewMaterialCode')">
            <a-input v-model:value="advancedQueryForm.ecNewMaterialCode" :placeholder="pi.queryPh('ecNewMaterialCode', 'required')" allow-clear :maxlength="20" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecOldPurchaseType')">
          <a-form-item :label="pi.queryLabel('ecOldPurchaseType')">
            <a-input v-model:value="advancedQueryForm.ecOldPurchaseType" :placeholder="pi.queryPh('ecOldPurchaseType', 'required')" allow-clear :maxlength="4" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecNewPurchaseType')">
          <a-form-item :label="pi.queryLabel('ecNewPurchaseType')">
            <a-input v-model:value="advancedQueryForm.ecNewPurchaseType" :placeholder="pi.queryPh('ecNewPurchaseType', 'required')" allow-clear :maxlength="4" />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecOldRequiresInspection')">
          <a-form-item :label="pi.queryLabel('ecOldRequiresInspection')">
            <TaktSelect v-model:value="advancedQueryForm.ecOldRequiresInspection" dict-type="sys_yes_no" :placeholder="pi.queryPh('ecOldRequiresInspection', 'select')" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecNewRequiresInspection')">
          <a-form-item :label="pi.queryLabel('ecNewRequiresInspection')">
            <TaktSelect v-model:value="advancedQueryForm.ecNewRequiresInspection" dict-type="sys_yes_no" :placeholder="pi.queryPh('ecNewRequiresInspection', 'select')" allow-clear />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('cultureCode')">
          <a-form-item :label="t('common.page.entity.culturecode')">
            <TaktSelect v-model:value="advancedQueryForm.cultureCode" dict-type="sys_culture_code" :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.culturecode') })" allow-clear />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变实施管控列表页
 */
import '../ec-list-layout.css'
import { message } from 'ant-design-vue';
import type { TableColumnsType } from 'ant-design-vue';
import { RiEditLine } from '@remixicon/vue';
import type { TaktPaneTabItem } from '@/components/business/TaktPaneTabs/index.vue';
import { useAnalysisPeriodRangeLimit } from '@/composables/useAnalysisPeriodRangeLimit';
import {
  TAKT_ANALYSIS_PERIOD_MAX_MONTHS,
  buildDefaultThroughCurrentPeriodRange,
} from '@/views/logistics/manufacturing/bom/material-cost/utils/bomMaterialCostPeriod';
import { useI18n } from 'vue-i18n';
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index';
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth';
import { getEcImplCtrlList, updateEcImplCtrl, exportEcImplCtrlData } from '@/api/logistics/manufacturing/engineering-change/ecImplCtrl.api';
import type { EcImplCtrl, EcImplCtrlQuery, EcImplCtrlUpdate } from '@/types/logistics/manufacturing/engineering-change/ecImplCtrl.types';
import { useEcDetailI18n, ECDETAIL_COLUMN_WIDTH } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcDetailI18n';
import TaktDictTag from '@/components/common/TaktDictTag/index.vue';
import ImplCtrlForm from './components/ImplCtrlForm.vue';

const { t } = useI18n();
const pi = useEcDetailI18n();
/** 列表 loading */
const loading = ref(false);
/** 当前页签执行表行 */
const dataSource = ref<EcImplCtrl[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词，放在查询栏最后 */
const queryKeyword = ref('');
/** 录入期间，截止本月 */
const entryPeriod = ref<[string, string] | null>(buildDefaultThroughCurrentPeriodRange(3));
/** 录入期间可选到本月，最多 12 个月 */
const entryPeriodLimit = useAnalysisPeriodRangeLimit(TAKT_ANALYSIS_PERIOD_MAX_MONTHS, { allowCurrentMonth: true });
/** 选中行 keys */
const selecmedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selecmedRows = ref<EcImplCtrl[]>([]);
/** 表单可见 */
const formVisible = ref(false);
/** 表单 loading */
const formLoading = ref(false);
/** 编辑数据 */
const formData = ref<EcImplCtrl | null>(null);
/** 表单 ref */
const formRef = ref<InstanceType<typeof ImplCtrlForm> | null>(null);
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 实施管控页签 */
type ImplCtrlTab = 'pc' | 'mfg2' | 'mfg1' | 'qc'
/** 部门页签列字段 */
type ImplCtrlField = 'ecCode' | 'ecModelCodeGroup' | 'ecRootMaterialCodeGroup' | 'ecControlScope'
/** 实施管控静态文案前缀 */
const IMPL_CTRL_PAGE = 'logistics.manufacturing.engineering-change.ec-impl-ctrl.page'
/** 当前页签，默认制二 */
const implCtrlTab = ref<ImplCtrlTab>('mfg2')
/** 四个页签：生管、制一、制二、品管（可按 show/permission 删减） */
const implCtrlTabItems = computed<TaktPaneTabItem[]>(() => [
  { key: 'pc', label: t(`${IMPL_CTRL_PAGE}.pc`) },
  { key: 'mfg1', label: t(`${IMPL_CTRL_PAGE}.mfg1`) },
  { key: 'mfg2', label: t(`${IMPL_CTRL_PAGE}.mfg2`) },
  { key: 'qc', label: t(`${IMPL_CTRL_PAGE}.qc`) },
])
watch(implCtrlTab, () => {
  currentPage.value = 1
  selecmedRowKeys.value = []
  selecmedRows.value = []
  if (hasListQuery()) {
    void loadData()
  } else {
    clearList()
  }
})
/** 四个页签都是设变单号、机种编码组、根物料编码组、控制范围 */
const DEPT_VIEW_FIELDS: Record<ImplCtrlTab, ImplCtrlField[]> = {
  pc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecControlScope'],
  mfg1: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecControlScope'],
  mfg2: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecControlScope'],
  qc: ['ecCode', 'ecModelCodeGroup', 'ecRootMaterialCodeGroup', 'ecControlScope'],
}
/** 列标题 */
const IMPL_CTRL_REQUIRED_TITLE: Partial<Record<ImplCtrlField, string>> = {
  ecModelCodeGroup: 'entity.ecqc.ecmodelcodegroup',
  ecRootMaterialCodeGroup: 'entity.ecqc.ecrootmaterialcodegroup',
  ecControlScope: 'entity.ecte.eccontrolscope',
}
/** 四个页签都有行内修改 */
const ACTION_TABS = new Set<ImplCtrlTab>(['pc', 'mfg1', 'mfg2', 'qc'])
/** 当前页签列：设变号、机种、根物料和该实施管控 */
const activeFields = computed<ImplCtrlField[]>(() => DEPT_VIEW_FIELDS[implCtrlTab.value])
/** 列定义（实施管控各表不显示 ID） */
const columns = computed<TableColumnsType>(() => [
  ...activeFields.value.map((field) => {
    return {
      title: IMPL_CTRL_REQUIRED_TITLE[field] ? t(IMPL_CTRL_REQUIRED_TITLE[field]) : pi.label(field),
      dataIndex: field,
      key: field,
      width: field === 'ecRootMaterialCodeGroup' || field === 'ecModelCodeGroup' ? 280 : (ECDETAIL_COLUMN_WIDTH[field as keyof typeof ECDETAIL_COLUMN_WIDTH] ?? 140),
      ellipsis: true,
      resizable: true,
      customRender: ({ record }: { record: Record<string, unknown> }) => {
        const value = record[field];
        if (field === 'ecControlScope') {
          return h(TaktDictTag, {
            dictType: 'logistics_manufacturing_ec_control_scope_category',
            value: value as string | number | undefined,
          });
        }
        if (value === undefined || value === null || value === '') {
          return '';
        }
        return String(value);
      },
    };
  }),
  ...(ACTION_TABS.has(implCtrlTab.value)
    ? [CreateActionColumn<EcImplCtrl>({
        width: 88,
        actions: [
          {
            key: 'update',
            label: t('common.page.button.update'),
            shape: 'plain',
            icon: RiEditLine,
            permission: 'logistics:manufacturing:engineering:change:implctrl:update',
            onClick: (record) => openEdit(record),
          },
        ],
      })]
    : []),
]);
/** 表格实际可见列：设变号、机种、根物料、实施管控；生管制一制二含修改 */
const tableVisibleColumnKeys = computed(() => {
  const keys = [...DEPT_VIEW_FIELDS[implCtrlTab.value]]
  if (ACTION_TABS.has(implCtrlTab.value)) {
    keys.push('action')
  }
  return keys
})
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false);
/** 高级查询字段 */
const IMPL_CTRL_QUERY_FIELDS = [
  'ecCode',
  'ecModelCode',
  'ecOldMaterialCode',
  'ecNewMaterialCode',
  'ecOldPurchaseType',
  'ecNewPurchaseType',
  'ecOldRequiresInspection',
  'ecNewRequiresInspection',
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
    ecModelCode: '',
    ecOldMaterialCode: '',
    ecNewMaterialCode: '',
    ecOldPurchaseType: '',
    ecNewPurchaseType: '',
    cultureCode: '',
    ecOldRequiresInspection: undefined as number | undefined,
    ecNewRequiresInspection: undefined as number | undefined,
  };
}
/** 高级查询表单 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm());
/** 高级查询字段标题 */
const queryFieldsMeta = computed(() => IMPL_CTRL_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'cultureCode' ? t('common.page.entity.culturecode') : pi.queryLabel(key),
})));
/** 行选择 */
const rowSelection = computed(() => ({ selecmedRowKeys: selecmedRowKeys.value, onChange: (keys: (string | number)[], rows: EcImplCtrl[]) => { selecmedRowKeys.value = keys; selecmedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selecmedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}
/**
 * 列表与导出共用的查询条件
 */
function buildListQuery(): EcImplCtrlQuery {
  const form = advancedQueryForm.value;
  const query: EcImplCtrlQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    deptTab: implCtrlTab.value,
  };
  const keyword = queryKeyword.value.trim();
  if (keyword) {
    query.keyWords = keyword;
  }
  const entryDates = entryPeriodToQuery(entryPeriod.value);
  if (entryDates.ecEntryDateStart) {
    query.ecEntryDateStart = entryDates.ecEntryDateStart;
  }
  if (entryDates.ecEntryDateEnd) {
    query.ecEntryDateEnd = entryDates.ecEntryDateEnd;
  }
  const textKeys = ['ecCode', 'ecModelCode', 'ecOldMaterialCode', 'ecNewMaterialCode', 'ecOldPurchaseType', 'ecNewPurchaseType', 'cultureCode'] as const;
  for (const key of textKeys) {
    const value = form[key].trim();
    if (value) {
      query[key] = value;
    }
  }
  if (form.ecOldRequiresInspection !== undefined && form.ecOldRequiresInspection !== null) {
    query.ecOldRequiresInspection = Number(form.ecOldRequiresInspection);
  }
  if (form.ecNewRequiresInspection !== undefined && form.ecNewRequiresInspection !== null) {
    query.ecNewRequiresInspection = Number(form.ecNewRequiresInspection);
  }
  return query;
}
/**
 * 是否已填写关键字或高级查询条件
 * @returns {boolean} 有任一条件时为 true
 */
function hasListQuery(): boolean {
  if (queryKeyword.value.trim() || entryPeriod.value?.[0]) {
    return true;
  }
  const form = advancedQueryForm.value;
  const textKeys = ['ecCode', 'ecModelCode', 'ecOldMaterialCode', 'ecNewMaterialCode', 'ecOldPurchaseType', 'ecNewPurchaseType', 'cultureCode'] as const;
  for (const key of textKeys) {
    if (form[key].trim()) {
      return true;
    }
  }
  if (form.ecOldRequiresInspection !== undefined && form.ecOldRequiresInspection !== null) {
    return true;
  }
  if (form.ecNewRequiresInspection !== undefined && form.ecNewRequiresInspection !== null) {
    return true;
  }
  return false;
}
/** 清空列表，不请求后端 */
function clearList(): void {
  dataSource.value = [];
  total.value = 0;
  selecmedRowKeys.value = [];
  selecmedRows.value = [];
}
/** 有查询条件时才加载列表 */
async function loadData() {
  if (!hasListQuery()) {
    clearList();
    return;
  }
  loading.value = true;
  try {
    const res = await getEcImplCtrlList(buildListQuery());
    dataSource.value = res.data ?? [];
    total.value = res.total ?? 0;
  } finally {
    loading.value = false;
  }
}
/** 搜索 */
function handleSearch() { currentPage.value = 1; loadData(); }
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
/** 分页变化 */
function handlePaginationChange() { loadData(); }
/** 表格变化 */
function handleTableChange() {}
/** 列宽变化 */
function handleResizeColumn() {}
/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return { onClick: () => { const id = getEcDetailId(record); selecmedRowKeys.value = [id]; selecmedRows.value = [record as unknown as EcImplCtrl]; } };
}
/**
 * 打开当前行的实施管控编辑
 * @param row 当前部门执行行
 */
function openEdit(row: EcImplCtrl) {
  const id = getEcDetailId(row as unknown as Record<string, unknown>);
  selecmedRowKeys.value = [id];
  selecmedRows.value = [row];
  formData.value = { ...row } as EcImplCtrl;
  formVisible.value = true;
}
/** 工具栏修改：编辑当前选中行 */
async function handleUpdate() {
  const row = selecmedRows.value[0];
  if (!row) return;
  openEdit(row);
}
/** 提交表单 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) return;
  await formRef.value.validate();
  const dto: EcImplCtrlUpdate = formRef.value.getValues();
  formLoading.value = true;
  try {
    await updateEcImplCtrl(String(formData.value.ecDetailId), dto);
    message.success(t('common.feedback.updated', { target: t('menu.logistics.manufacturing.engineering.change.implctrl') }));
    formVisible.value = false;
    await loadData();
  } finally {
    formLoading.value = false;
  }
}
/** 导出 */
async function handleExport() {
  if (!hasListQuery()) {
    return;
  }
  try {
    loading.value = true;
    const blob = await exportEcImplCtrlData(buildListQuery());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.implctrl')}.xlsx`;
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
</script>

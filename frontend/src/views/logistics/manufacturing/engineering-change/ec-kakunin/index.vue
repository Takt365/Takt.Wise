<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-kakunin -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变物料确认页面 -->
<!-- 版权信息：Copyright (c) 2026 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <TaktQueryBar v-model="queryKeyword" :placeholder="t('common.page.form.placeholder.search')" :loading="loading" @search="handleSearch" @reset="handleReset" />
    <TaktToolsBar
      :show-create="false"
      :show-update="true"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      update-permission="logistics:manufacturing:engineering:change:kakunin:update"
      export-permission="logistics:manufacturing:engineering:change:kakunin:export"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="loadData"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'id'"
      table-mode="single"
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
    <TaktModal v-model:open="formVisible" :title="t('common.dialog.title.edit', { entity: t('menu.logistics.manufacturing.engineering.change.kakunin') })" :width="formModalWidthPx" :confirm-loading="formLoading" @ok="handleFormSubmit">
      <KakuninForm ref="formRef" :form-data="formData" :loading="formLoading" />
    </TaktModal>
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-kakunin"
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
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="id"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变物料确认列表页
 */
import { message } from 'ant-design-vue';
import type { TableColumnsType } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth';
import { getEcKakuninList, updateEcKakunin, exportEcKakuninData } from '@/api/logistics/manufacturing/engineering-change/ecKakunin.api';
import type { EcKakunin, EcKakuninQuery, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/ecKakunin.types';
import type { EcDetail } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types';
import { resolveEntityScopeBaseFieldKeys } from '@/utils/tableColumns';
import TaktDictTag from '@/components/common/TaktDictTag/index.vue';
import { useEcDetailI18n, ECDETAIL_COLUMN_WIDTH } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n';
import KakuninForm from './components/KakuninForm.vue';

const { t } = useI18n();
const pi = useEcDetailI18n();
/** 列表 loading */
const loading = ref(false);
/** 数据源（TaktEcDetail 全列） */
const dataSource = ref<EcDetail[]>([]);
/** 当前页 */
const currentPage = ref(1);
/** 每页条数 */
const pageSize = ref(20);
/** 总数 */
const total = ref(0);
/** 关键词 */
const queryKeyword = ref('');
/** 选中行 keys */
const selectedRowKeys = ref<(string | number)[]>([]);
/** 选中行 */
const selectedRows = ref<EcDetail[]>([]);
/** 表单可见 */
const formVisible = ref(false);
/** 表单 loading */
const formLoading = ref(false);
/** 编辑数据 */
const formData = ref<EcKakunin | null>(null);
/** 表单 ref */
const formRef = ref<InstanceType<typeof KakuninForm> | null>(null);
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** TaktEcDetail 标量列（声明顺序；导航属性不进表格） */
const KAKUNIN_DETAIL_FIELDS = [
  'lineNumber',
  'ecCode',
  'ecBomLineCode',
  'ecModelCode',
  'ecRootMaterialCode',
  'ecRootMaterialDescription',
  'discontinuedStatus',
  'ecParentMaterialCode',
  'ecParentMaterialDescription',
  'ecOldMaterialCode',
  'ecOldMaterialDescription',
  'ecOldUsageQuantity',
  'ecOldItemPosition',
  'ecOldStock',
  'ecOldWarehouse',
  'ecOldPurchaseType',
  'ecOldRequiresInspection',
  'ecNewMaterialCode',
  'ecNewMaterialDescription',
  'ecNewUsageQuantity',
  'ecNewItemPosition',
  'ecNewStock',
  'ecNewWarehouse',
  'ecNewPurchaseType',
  'ecNewRequiresInspection',
  'ecBomDate',
  'ecIsCompatible',
  'ec2ndVendor',
  'ecInstruction',
  'ecOldPartDisposition',
  'ecScope',
  'ecDeptScope',
  'ecGijutsuId',
  'isObsolete',
] as const;
/** 字典列 */
const KAKUNIN_DICT_TYPE: Partial<Record<(typeof KAKUNIN_DETAIL_FIELDS)[number], string>> = {
  discontinuedStatus: 'logistics_materials_material_discontinued_status',
  ecOldPurchaseType: 'logistics_procurement_type',
  ecOldRequiresInspection: 'sys_yes_no',
  ecNewPurchaseType: 'logistics_procurement_type',
  ecNewRequiresInspection: 'sys_yes_no',
  ec2ndVendor: 'logistics_manufacturing_ec_2nd_vendor',
  ecInstruction: 'logistics_manufacturing_ec_source_instruction',
  ecOldPartDisposition: 'logistics_manufacturing_ec_old_part_disposition',
  ecScope: 'logistics_manufacturing_ec_scope_category',
  ecDeptScope: 'logistics_manufacturing_ec_dept_scope_category',
  isObsolete: 'sys_yes_no',
};
/**
 * 默认可见列：主键 + 公司基类列 + 明细全列（列设置只做显隐）
 */
function buildDefaultVisibleColumnKeys(): string[] {
  const base = resolveEntityScopeBaseFieldKeys('company');
  const rest = base.filter((key) => key !== 'plantCode');
  return ['id', 'plantCode', ...KAKUNIN_DETAIL_FIELDS, ...rest];
}
/** 列定义（与 TaktEcDetail 声明顺序一致） */
const columns = computed<TableColumnsType>(() => [
  {
    title: 'ID',
    dataIndex: 'ecDetailId',
    key: 'id',
    width: 180,
    ellipsis: true,
    resizable: true,
    fixed: 'left',
  },
  ...KAKUNIN_DETAIL_FIELDS.map((field) => {
    const dictType = KAKUNIN_DICT_TYPE[field];
    return {
      title: pi.label(field),
      dataIndex: field,
      key: field,
      width: ECDETAIL_COLUMN_WIDTH[field as keyof typeof ECDETAIL_COLUMN_WIDTH] ?? 120,
      ellipsis: true,
      resizable: true,
      customRender: ({ record }: { record: Record<string, unknown> }) => {
        const value = record[field];
        if (dictType) {
          return h(TaktDictTag, { dictType, value: value == null || value === '' ? '' : String(value) });
        }
        if (field === 'ecBomDate' && value) {
          return String(value).slice(0, 10);
        }
        if (value === undefined || value === null || value === '') {
          return '';
        }
        return String(value);
      },
    };
  }),
]);
/** 列设置抽屉 */
const columnSettingVisible = ref(false);
/** 可见列 keys */
const visibleColumnKeys = ref<string[]>(buildDefaultVisibleColumnKeys());
/** 高级查询抽屉 */
const advancedQueryVisible = ref(false);
/** 高级查询字段 */
const KAKUNIN_QUERY_FIELDS = [
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
const queryFieldsMeta = computed(() => KAKUNIN_QUERY_FIELDS.map((key) => ({
  key,
  label: key === 'cultureCode' ? t('common.page.entity.culturecode') : pi.queryLabel(key),
})));
/** 行选择 */
const rowSelection = computed(() => ({ selectedRowKeys: selectedRowKeys.value, onChange: (keys: (string | number)[], rows: EcDetail[]) => { selectedRowKeys.value = keys; selectedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}
/**
 * 列表与导出共用的查询条件
 */
function buildListQuery(): EcKakuninQuery {
  const form = advancedQueryForm.value;
  const query: EcKakuninQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
  };
  const keyword = queryKeyword.value.trim();
  if (keyword) {
    query.keyWords = keyword;
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
/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKakuninList(buildListQuery());
    dataSource.value = res.data ?? [];
    total.value = res.total ?? 0;
  } finally {
    loading.value = false;
  }
}
/** 搜索 */
function handleSearch() { currentPage.value = 1; loadData(); }
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
  visibleColumnKeys.value = buildDefaultVisibleColumnKeys();
}
/** 分页变化 */
function handlePaginationChange() { loadData(); }
/** 表格变化 */
function handleTableChange() {}
/** 列宽变化 */
function handleResizeColumn() {}
/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return { onClick: () => { const id = getEcDetailId(record); selectedRowKeys.value = [id]; selectedRows.value = [record as unknown as EcDetail]; } };
}
/** 编辑 */
async function handleUpdate() {
  const row = selectedRows.value[0];
  if (!row) return;
  formData.value = { ...row } as EcKakunin;
  formVisible.value = true;
}
/** 提交表单 */
async function handleFormSubmit() {
  if (!formRef.value || !formData.value) return;
  await formRef.value.validate();
  const dto: EcKakuninUpdate = formRef.value.getValues();
  formLoading.value = true;
  try {
    await updateEcKakunin(String(formData.value.ecDetailId), dto);
    message.success(t('common.feedback.updated', { target: t('menu.logistics.manufacturing.engineering.change.kakunin') }));
    formVisible.value = false;
    await loadData();
  } finally {
    formLoading.value = false;
  }
}
/** 导出 */
async function handleExport() {
  try {
    loading.value = true;
    const blob = await exportEcKakuninData(buildListQuery());
    const url = window.URL.createObjectURL(blob as Blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `${t('menu.logistics.manufacturing.engineering.change.kakunin')}.xlsx`;
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

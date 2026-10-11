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
      update-permission="logistics:manufacturing:engineering:change:kakunin:update"
      export-permission="logistics:manufacturing:engineering:change:kakunin:export"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
      @export="handleExport"
      @refresh="loadData"
    />
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'ecDetailId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcDetailId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'ecOldRequiresInspection'">
          <TaktDictTag dict-type="sys_yes_no" :value="record.ecOldRequiresInspection" />
        </template>
        <template v-else-if="column.key === 'ecNewRequiresInspection'">
          <TaktDictTag dict-type="sys_yes_no" :value="record.ecNewRequiresInspection" />
        </template>
      </template>
    </TaktSingleTable>
    <TaktPagination v-model:current="currentPage" v-model:page-size="pageSize" :total="total" @change="handlePaginationChange" />
    <TaktModal v-model:open="formVisible" :title="t('common.dialog.title.edit', { entity: t('menu.logistics.manufacturing.engineering.change.kakunin') })" :width="formModalWidthPx" :confirm-loading="formLoading" @ok="handleFormSubmit">
      <KakuninForm ref="formRef" :form-data="formData" :loading="formLoading" />
    </TaktModal>
  </div>
</template>

<script setup lang="ts">
/**
 * 设变物料确认列表页
 */
import { message } from 'ant-design-vue';
import { useI18n } from 'vue-i18n';
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth';
import { getEcKakuninList, updateEcKakunin, exportEcKakuninData } from '@/api/logistics/manufacturing/engineering-change/ecKakunin.api';
import type { EcKakunin, EcKakuninUpdate } from '@/types/logistics/manufacturing/engineering-change/ecKakunin.types';
import { useEcDetailI18n } from '@/views/logistics/manufacturing/engineering-change/ec-gijutsu/composables/useEcDetailI18n';
import KakuninForm from './components/KakuninForm.vue';

const { t } = useI18n();
const pi = useEcDetailI18n();
/** 列表 loading */
const loading = ref(false);
/** 数据源 */
const dataSource = ref<EcKakunin[]>([]);
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
const selectedRows = ref<EcKakunin[]>([]);
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
/** 列定义（entity.ecdetail.*） */
const columns = computed(() => [
  { title: pi.label('ecCode'), dataIndex: 'ecCode', key: 'ecCode', width: 120 },
  { title: pi.label('lineNumber'), dataIndex: 'lineNumber', key: 'lineNumber', width: 80 },
  { title: pi.label('ecModelCode'), dataIndex: 'ecModelCode', key: 'ecModelCode', width: 140 },
  { title: pi.label('ecOldMaterialCode'), dataIndex: 'ecOldMaterialCode', key: 'ecOldMaterialCode', width: 140 },
  { title: pi.label('ecNewMaterialCode'), dataIndex: 'ecNewMaterialCode', key: 'ecNewMaterialCode', width: 140 },
  { title: pi.label('ecOldPurchaseType'), dataIndex: 'ecOldPurchaseType', key: 'ecOldPurchaseType', width: 120 },
  { title: pi.label('ecOldRequiresInspection'), dataIndex: 'ecOldRequiresInspection', key: 'ecOldRequiresInspection', width: 120 },
  { title: pi.label('ecNewPurchaseType'), dataIndex: 'ecNewPurchaseType', key: 'ecNewPurchaseType', width: 120 },
  { title: pi.label('ecNewRequiresInspection'), dataIndex: 'ecNewRequiresInspection', key: 'ecNewRequiresInspection', width: 120 },
]);
/** 可见列 keys */
const visibleColumnKeys = computed(() => columns.value.map((c) => String(c.key)));
/** 行选择 */
const rowSelection = computed(() => ({ selectedRowKeys: selectedRowKeys.value, onChange: (keys: (string | number)[], rows: EcKakunin[]) => { selectedRowKeys.value = keys; selectedRows.value = rows; } }));
/** 更新按钮禁用 */
const updateDisabled = computed(() => selectedRowKeys.value.length !== 1);
/**
 * 行主键
 */
function getEcDetailId(record: Record<string, unknown>) {
  return String(record.ecDetailId ?? '');
}
/** 加载列表 */
async function loadData() {
  loading.value = true;
  try {
    const res = await getEcKakuninList({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
    dataSource.value = res.data ?? [];
    total.value = res.total ?? 0;
  } finally {
    loading.value = false;
  }
}
/** 搜索 */
function handleSearch() { currentPage.value = 1; loadData(); }
/** 重置 */
function handleReset() { queryKeyword.value = ''; currentPage.value = 1; loadData(); }
/** 分页变化 */
function handlePaginationChange() { loadData(); }
/** 表格变化 */
function handleTableChange() {}
/** 列宽变化 */
function handleResizeColumn() {}
/** 行点击 */
function onClickRow(record: Record<string, unknown>) {
  return { onClick: () => { const id = getEcDetailId(record); selectedRowKeys.value = [id]; selectedRows.value = [record as unknown as EcKakunin]; } };
}
/** 编辑 */
async function handleUpdate() {
  const row = selectedRows.value[0];
  if (!row) return;
  formData.value = { ...row };
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
    const blob = await exportEcKakuninData({ pageIndex: currentPage.value, pageSize: pageSize.value, keyWords: queryKeyword.value || undefined });
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

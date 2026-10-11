<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/source-ec -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变来源只读列表页，含查询、导出（无新增/编辑/删除/导入） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getSourceEcId"
      :master-row-selection="rowSelection"
      master-id-column-key="sourceEcId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="company"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
        <TaktToolsBar
      export-permission="logistics:manufacturing:engineering:change:source:ec:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
        />
      </template>
      <template #detail>
        <SourceEcDetailPanel class="h-full min-h-0 flex-1" />
      </template>
    </TaktMasterDetailTableLr>

    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-source-ec'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="pi.queryLabel('plantCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.plantCode"
          api-url="TaktPlants/options"
          :placeholder="pi.queryPh('plantCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcCode')">
      <a-form-item :label="pi.queryLabel('sourceEcCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceEcCode"
          :placeholder="pi.queryPh('sourceEcCode', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceModel')">
      <a-form-item :label="pi.queryLabel('sourceModel')">
        <a-input
          v-model:value="advancedQueryForm.sourceModel"
          :placeholder="pi.queryPh('sourceModel', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTitle')">
      <a-form-item :label="pi.queryLabel('sourceTitle')">
        <a-input
          v-model:value="advancedQueryForm.sourceTitle"
          :placeholder="pi.queryPh('sourceTitle', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceStatus')">
      <a-form-item :label="pi.queryLabel('sourceStatus')">
        <a-input
          v-model:value="advancedQueryForm.sourceStatus"
          :placeholder="pi.queryPh('sourceStatus', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceIssueDateStart')">
      <a-form-item :label="pi.queryLabel('sourceIssueDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceIssueDateStart"
          :placeholder="pi.queryPh('sourceIssueDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceIssueDateEnd')">
      <a-form-item :label="pi.queryLabel('sourceIssueDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.sourceIssueDateEnd"
          :placeholder="pi.queryPh('sourceIssueDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTcjOwner')">
      <a-form-item :label="pi.queryLabel('sourceTcjOwner')">
        <a-input
          v-model:value="advancedQueryForm.sourceTcjOwner"
          :placeholder="pi.queryPh('sourceTcjOwner', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTcjDependency')">
      <a-form-item :label="pi.queryLabel('sourceTcjDependency')">
        <a-input
          v-model:value="advancedQueryForm.sourceTcjDependency"
          :placeholder="pi.queryPh('sourceTcjDependency', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcMeeting')">
      <a-form-item :label="pi.queryLabel('sourceEcMeeting')">
        <a-input
          v-model:value="advancedQueryForm.sourceEcMeeting"
          :placeholder="pi.queryPh('sourceEcMeeting', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourcePpCode')">
      <a-form-item :label="pi.queryLabel('sourcePpCode')">
        <a-input
          v-model:value="advancedQueryForm.sourcePpCode"
          :placeholder="pi.queryPh('sourcePpCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceTechnicalNoticeCode')">
      <a-form-item :label="pi.queryLabel('sourceTechnicalNoticeCode')">
        <a-input
          v-model:value="advancedQueryForm.sourceTechnicalNoticeCode"
          :placeholder="pi.queryPh('sourceTechnicalNoticeCode', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceImplementation')">
      <a-form-item :label="pi.queryLabel('sourceImplementation')">
        <a-input
          v-model:value="advancedQueryForm.sourceImplementation"
          :placeholder="pi.queryPh('sourceImplementation', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceMainChangeReason')">
      <a-form-item :label="pi.queryLabel('sourceMainChangeReason')">
        <a-input
          v-model:value="advancedQueryForm.sourceMainChangeReason"
          :placeholder="pi.queryPh('sourceMainChangeReason', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSecondaryChangeReason')">
      <a-form-item :label="pi.queryLabel('sourceSecondaryChangeReason')">
        <a-input
          v-model:value="advancedQueryForm.sourceSecondaryChangeReason"
          :placeholder="pi.queryPh('sourceSecondaryChangeReason', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSafetyRegulation')">
      <a-form-item :label="pi.queryLabel('sourceSafetyRegulation')">
        <a-input
          v-model:value="advancedQueryForm.sourceSafetyRegulation"
          :placeholder="pi.queryPh('sourceSafetyRegulation', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceProgressStatus')">
      <a-form-item :label="pi.queryLabel('sourceProgressStatus')">
        <a-input
          v-model:value="advancedQueryForm.sourceProgressStatus"
          :placeholder="pi.queryPh('sourceProgressStatus', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceSerialNumberControl')">
      <a-form-item :label="pi.queryLabel('sourceSerialNumberControl')">
        <a-input
          v-model:value="advancedQueryForm.sourceSerialNumberControl"
          :placeholder="pi.queryPh('sourceSerialNumberControl', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCustomerApproval')">
      <a-form-item :label="pi.queryLabel('sourceCustomerApproval')">
        <a-input
          v-model:value="advancedQueryForm.sourceCustomerApproval"
          :placeholder="pi.queryPh('sourceCustomerApproval', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceServiceManualRevision')">
      <a-form-item :label="pi.queryLabel('sourceServiceManualRevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceServiceManualRevision"
          :placeholder="pi.queryPh('sourceServiceManualRevision', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceUserManualRevision')">
      <a-form-item :label="pi.queryLabel('sourceUserManualRevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceUserManualRevision"
          :placeholder="pi.queryPh('sourceUserManualRevision', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourcePromotionManualRevision')">
      <a-form-item :label="pi.queryLabel('sourcePromotionManualRevision')">
        <a-input
          v-model:value="advancedQueryForm.sourcePromotionManualRevision"
          :placeholder="pi.queryPh('sourcePromotionManualRevision', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceStandardDocumentRevision')">
      <a-form-item :label="pi.queryLabel('sourceStandardDocumentRevision')">
        <a-input
          v-model:value="advancedQueryForm.sourceStandardDocumentRevision"
          :placeholder="pi.queryPh('sourceStandardDocumentRevision', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceInformationRelease')">
      <a-form-item :label="pi.queryLabel('sourceInformationRelease')">
        <a-input
          v-model:value="advancedQueryForm.sourceInformationRelease"
          :placeholder="pi.queryPh('sourceInformationRelease', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceCostChange')">
      <a-form-item :label="pi.queryLabel('sourceCostChange')">
        <a-input
          v-model:value="advancedQueryForm.sourceCostChange"
          :placeholder="pi.queryPh('sourceCostChange', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceUnitCost')">
      <a-form-item :label="pi.queryLabel('sourceUnitCost')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceUnitCost"
          :placeholder="pi.queryPh('sourceUnitCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceMoldModificationCost')">
      <a-form-item :label="pi.queryLabel('sourceMoldModificationCost')">
        <a-input-number
          v-model:value="advancedQueryForm.sourceMoldModificationCost"
          :placeholder="pi.queryPh('sourceMoldModificationCost', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceRelatedDrawing')">
      <a-form-item :label="pi.queryLabel('sourceRelatedDrawing')">
        <a-input
          v-model:value="advancedQueryForm.sourceRelatedDrawing"
          :placeholder="pi.queryPh('sourceRelatedDrawing', 'required')"
          show-count
          :maxlength="210"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceEcContent')">
      <a-form-item :label="pi.queryLabel('sourceEcContent')">
        <a-textarea
          v-model:value="advancedQueryForm.sourceEcContent"
          :placeholder="pi.queryPh('sourceEcContent', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'sourceEcId'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变来源只读列表页（查询/导出）
 * @module views/logistics/manufacturing/engineering-change/source-ec
 */
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SourceEcDetailPanel from './components/SourceEcDetailPanel.vue'
import { provideSourceEcMasterContext, type SourceEcRowRecord } from './composables/useSourceEcMasterContext'
import { getSourceEcList, exportSourceEc } from '@/api/logistics/manufacturing/engineering-change/sourceEc.api'
import type { SourceEc, SourceEcQuery } from '@/types/logistics/manufacturing/engineering-change/sourceEc.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiQuestionLine } from '@remixicon/vue'

import {
  useSourceEcI18n,
  SOURCEEC_QUERY_STRING_FIELDS,
  SOURCEEC_QUERY_FIELDS,
} from './composables/useSourceEcI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSourceEcI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSourceEc')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SourceEc[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SourceEcRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<SourceEcRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of SOURCEEC_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.sourceUnitCost !== undefined && form.sourceUnitCost !== null) {
    return true
  }
  if (form.sourceMoldModificationCost !== undefined && form.sourceMoldModificationCost !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SOURCEEC_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SOURCEEC_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    sourceUnitCost: undefined as number | undefined,
    sourceMoldModificationCost: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  SOURCEEC_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'sourceEcId'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSourceEcMasterContext()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SourceEcQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SourceEcQuery>): SourceEcQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SourceEcQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SourceEcQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SOURCEEC_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.sourceUnitCost !== undefined && form.sourceUnitCost !== null) {
    query.sourceUnitCost = form.sourceUnitCost
  }
  if (form.sourceMoldModificationCost !== undefined && form.sourceMoldModificationCost !== null) {
    query.sourceMoldModificationCost = form.sourceMoldModificationCost
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: SourceEcRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSourceEcId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SourceEcRowRecord
  const key = getSourceEcId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sourceEcId',
    key: 'sourceEcId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcId') ?? ''
  },
  {
    title: pi.label('sourceEcCode'),
    dataIndex: 'sourceEcCode',
    key: 'sourceEcCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcCode') ?? ''
  },
  {
    title: pi.label('sourceModel'),
    dataIndex: 'sourceModel',
    key: 'sourceModel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceModel') ?? ''
  },
  {
    title: pi.label('sourceTitle'),
    dataIndex: 'sourceTitle',
    key: 'sourceTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTitle') ?? ''
  },
  {
    title: pi.label('sourceStatus'),
    dataIndex: 'sourceStatus',
    key: 'sourceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceStatus') ?? ''
  },
  {
    title: pi.label('sourceIssueDate'),
    dataIndex: 'sourceIssueDate',
    key: 'sourceIssueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceIssueDate') ?? ''
  },
  {
    title: pi.label('sourceTcjOwner'),
    dataIndex: 'sourceTcjOwner',
    key: 'sourceTcjOwner',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTcjOwner') ?? ''
  },
  {
    title: pi.label('sourceTcjDependency'),
    dataIndex: 'sourceTcjDependency',
    key: 'sourceTcjDependency',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTcjDependency') ?? ''
  },
  {
    title: pi.label('sourceEcMeeting'),
    dataIndex: 'sourceEcMeeting',
    key: 'sourceEcMeeting',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcMeeting') ?? ''
  },
  {
    title: pi.label('sourcePpCode'),
    dataIndex: 'sourcePpCode',
    key: 'sourcePpCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourcePpCode') ?? ''
  },
  {
    title: pi.label('sourceTechnicalNoticeCode'),
    dataIndex: 'sourceTechnicalNoticeCode',
    key: 'sourceTechnicalNoticeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceTechnicalNoticeCode') ?? ''
  },
  {
    title: pi.label('sourceImplementation'),
    dataIndex: 'sourceImplementation',
    key: 'sourceImplementation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceImplementation') ?? ''
  },
  {
    title: pi.label('sourceMainChangeReason'),
    dataIndex: 'sourceMainChangeReason',
    key: 'sourceMainChangeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceMainChangeReason') ?? ''
  },
  {
    title: pi.label('sourceSecondaryChangeReason'),
    dataIndex: 'sourceSecondaryChangeReason',
    key: 'sourceSecondaryChangeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSecondaryChangeReason') ?? ''
  },
  {
    title: pi.label('sourceSafetyRegulation'),
    dataIndex: 'sourceSafetyRegulation',
    key: 'sourceSafetyRegulation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSafetyRegulation') ?? ''
  },
  {
    title: pi.label('sourceProgressStatus'),
    dataIndex: 'sourceProgressStatus',
    key: 'sourceProgressStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceProgressStatus') ?? ''
  },
  {
    title: pi.label('sourceSerialNumberControl'),
    dataIndex: 'sourceSerialNumberControl',
    key: 'sourceSerialNumberControl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceSerialNumberControl') ?? ''
  },
  {
    title: pi.label('sourceCustomerApproval'),
    dataIndex: 'sourceCustomerApproval',
    key: 'sourceCustomerApproval',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceCustomerApproval') ?? ''
  },
  {
    title: pi.label('sourceServiceManualRevision'),
    dataIndex: 'sourceServiceManualRevision',
    key: 'sourceServiceManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceServiceManualRevision') ?? ''
  },
  {
    title: pi.label('sourceUserManualRevision'),
    dataIndex: 'sourceUserManualRevision',
    key: 'sourceUserManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceUserManualRevision') ?? ''
  },
  {
    title: pi.label('sourcePromotionManualRevision'),
    dataIndex: 'sourcePromotionManualRevision',
    key: 'sourcePromotionManualRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourcePromotionManualRevision') ?? ''
  },
  {
    title: pi.label('sourceStandardDocumentRevision'),
    dataIndex: 'sourceStandardDocumentRevision',
    key: 'sourceStandardDocumentRevision',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceStandardDocumentRevision') ?? ''
  },
  {
    title: pi.label('sourceInformationRelease'),
    dataIndex: 'sourceInformationRelease',
    key: 'sourceInformationRelease',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceInformationRelease') ?? ''
  },
  {
    title: pi.label('sourceCostChange'),
    dataIndex: 'sourceCostChange',
    key: 'sourceCostChange',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceCostChange') ?? ''
  },
  {
    title: pi.label('sourceUnitCost'),
    dataIndex: 'sourceUnitCost',
    key: 'sourceUnitCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceUnitCost') ?? ''
  },
  {
    title: pi.label('sourceMoldModificationCost'),
    dataIndex: 'sourceMoldModificationCost',
    key: 'sourceMoldModificationCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceMoldModificationCost') ?? ''
  },
  {
    title: pi.label('sourceRelatedDrawing'),
    dataIndex: 'sourceRelatedDrawing',
    key: 'sourceRelatedDrawing',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceRelatedDrawing') ?? ''
  },
  {
    title: pi.label('sourceEcContent'),
    dataIndex: 'sourceEcContent',
    key: 'sourceEcContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSourceEcField(record, 'sourceEcContent') ?? ''
  },
])

/** 表格 row-key（优先实体主键字段） */
const getSourceEcId = (record: SourceEcRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSourceEcField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SourceEcRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SourceEcRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSourceEcId(selectedRow.value) === getSourceEcId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SourceEcRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getSourceEcList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SourceEc] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  cultureCode: '',
  plantCode: '',
  sourceEcCode: '',
  sourceModel: '',
  sourceTitle: '',
  sourceStatus: '',
  sourceIssueDateStart: '',
  sourceIssueDateEnd: '',
  sourceTcjOwner: '',
  sourceTcjDependency: '',
  sourceEcMeeting: '',
  sourcePpCode: '',
  sourceTechnicalNoticeCode: '',
  sourceImplementation: '',
  sourceMainChangeReason: '',
  sourceSecondaryChangeReason: '',
  sourceSafetyRegulation: '',
  sourceProgressStatus: '',
  sourceSerialNumberControl: '',
  sourceCustomerApproval: '',
  sourceServiceManualRevision: '',
  sourceUserManualRevision: '',
  sourcePromotionManualRevision: '',
  sourceStandardDocumentRevision: '',
  sourceInformationRelease: '',
  sourceCostChange: '',
  sourceUnitCost: undefined as number | undefined,
  sourceMoldModificationCost: undefined as number | undefined,
  sourceRelatedDrawing: '',
  sourceEcContent: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportSourceEc(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[SourceEc] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  cultureCode: '',
  plantCode: '',
  sourceEcCode: '',
  sourceModel: '',
  sourceTitle: '',
  sourceStatus: '',
  sourceIssueDateStart: '',
  sourceIssueDateEnd: '',
  sourceTcjOwner: '',
  sourceTcjDependency: '',
  sourceEcMeeting: '',
  sourcePpCode: '',
  sourceTechnicalNoticeCode: '',
  sourceImplementation: '',
  sourceMainChangeReason: '',
  sourceSecondaryChangeReason: '',
  sourceSafetyRegulation: '',
  sourceProgressStatus: '',
  sourceSerialNumberControl: '',
  sourceCustomerApproval: '',
  sourceServiceManualRevision: '',
  sourceUserManualRevision: '',
  sourcePromotionManualRevision: '',
  sourceStandardDocumentRevision: '',
  sourceInformationRelease: '',
  sourceCostChange: '',
  sourceUnitCost: undefined as number | undefined,
  sourceMoldModificationCost: undefined as number | undefined,
  sourceRelatedDrawing: '',
  sourceEcContent: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：BillOfMaterialSubstitutePanel.vue -->
<!-- 功能描述：Takt物料清单明细实体主表实体右侧明细 billOfMaterialSubstitute 独立 CRUD（按主表选中 billOfMaterialItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="bill-of-material-substitute-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:bom:bill:of:material:create"
      update-permission="logistics:manufacturing:bom:bill:of:material:update"
      delete-permission="logistics:manufacturing:bom:bill:of:material:delete"
      import-permission="logistics:manufacturing:bom:bill:of:material:import"
      export-permission="logistics:manufacturing:bom:bill:of:material:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"

      :show-import="true"
      :show-export="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="bill-of-material-substitute-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getBillOfMaterialSubstituteId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="billOfMaterialSubstituteId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <BillOfMaterialSubstituteForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterBillOfMaterialItemId"
        :master-row="selecmedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-bom-bill-of-material-item-bill-of-material-substitute"
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
      <div v-show="isFieldVisible('billOfMaterialId')">
      <a-form-item :label="pi.queryLabel('billOfMaterialId')">
        <a-input
          v-model:value="advancedQueryForm.billOfMaterialId"
          :placeholder="pi.queryPh('billOfMaterialId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="pi.queryLabel('bomCode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="pi.queryPh('bomCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('primaryMaterialCode')">
      <a-form-item :label="pi.queryLabel('primaryMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.primaryMaterialCode"
          :placeholder="pi.queryPh('primaryMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteMaterialId')">
      <a-form-item :label="pi.queryLabel('substituteMaterialId')">
        <TaktSelect
          v-model:value="advancedQueryForm.substituteMaterialId"
          api-url="TaktMaterialPlants/options"
          :placeholder="pi.queryPh('substituteMaterialId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteMaterialCode')">
      <a-form-item :label="pi.queryLabel('substituteMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.substituteMaterialCode"
          :placeholder="pi.queryPh('substituteMaterialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteGroup')">
      <a-form-item :label="pi.queryLabel('substituteGroup')">
        <a-input
          v-model:value="advancedQueryForm.substituteGroup"
          :placeholder="pi.queryPh('substituteGroup', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substitutePriority')">
      <a-form-item :label="pi.queryLabel('substitutePriority')">
        <a-input-number
          v-model:value="advancedQueryForm.substitutePriority"
          :placeholder="pi.queryPh('substitutePriority', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageQuantity')">
      <a-form-item :label="pi.queryLabel('usageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.usageQuantity"
          :placeholder="pi.queryPh('usageQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="pi.queryLabel('materialUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('materialUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageRatio')">
      <a-form-item :label="pi.queryLabel('usageRatio')">
        <a-input-number
          v-model:value="advancedQueryForm.usageRatio"
          :placeholder="pi.queryPh('usageRatio', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isEnabled')">
      <a-form-item :label="pi.queryLabel('isEnabled')">
        <TaktSelect
          v-model:value="advancedQueryForm.isEnabled"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isEnabled', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="pi.queryLabel('effectiveDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="pi.queryPh('effectiveDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="pi.queryLabel('effectiveDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="pi.queryPh('effectiveDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="pi.queryLabel('expiryDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="pi.queryPh('expiryDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="pi.queryLabel('expiryDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="pi.queryPh('expiryDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isObsolete', 'select')"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="BILLOFMATERIALSUBSTITUTE_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="billOfMaterialSubstituteId"
      action-column-key="action"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt物料清单明细实体子表 billOfMaterialSubstitute 右栏面板
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import BillOfMaterialSubstituteForm from './BillOfMaterialSubstituteForm.vue'
import { useBillOfMaterialItemMasterContext } from '../composables/useBillOfMaterialItemMasterContext'
import {
  getBillOfMaterialSubstituteList,
  getBillOfMaterialSubstituteById,
  createBillOfMaterialSubstitute,
  updateBillOfMaterialSubstitute,
  deleteBillOfMaterialSubstituteById,
  deleteBillOfMaterialSubstituteBatch,
  getBillOfMaterialSubstituteTemplate,
  importBillOfMaterialSubstitute,
  exportBillOfMaterialSubstitute,
} from '@/api/logistics/manufacturing/bom/billOfMaterialSubstitute.api'
import type { BillOfMaterialSubstitute, BillOfMaterialSubstituteQuery } from '@/types/logistics/manufacturing/bom/billOfMaterialSubstitute.types'

import {
  useBillOfMaterialSubstituteI18n,
  BILLOFMATERIALSUBSTITUTE_DEFAULT_VISIBLE_COLUMN_KEYS,
  BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS,
  BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS,
  BILLOFMATERIALSUBSTITUTE_QUERY_FIELDS,
  BILLOFMATERIALSUBSTITUTE_SELF_I18N_KEY,
} from '../composables/useBillOfMaterialSubstituteI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBillOfMaterialSubstituteI18n()

const { t } = useI18n()
const { selecmedMasterRow } = useBillOfMaterialItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterialSubstitute')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)

/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: true })
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}
const dataSource = ref<BillOfMaterialSubstitute[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<BillOfMaterialSubstitute | null>(null)
const selecmedRows = ref<BillOfMaterialSubstitute[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<BillOfMaterialSubstitute>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

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
  for (const key of BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    return true
  }
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    return true
  }
  if (form.usageRatio !== undefined && form.usageRatio !== null) {
    return true
  }
  if (form.isEnabled !== undefined && form.isEnabled !== null) {
    return true
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    substitutePriority: undefined as number | undefined,
    usageQuantity: undefined as number | undefined,
    usageRatio: undefined as number | undefined,
    isEnabled: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  BILLOFMATERIALSUBSTITUTE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...BILLOFMATERIALSUBSTITUTE_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...BILLOFMATERIALSUBSTITUTE_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'billOfMaterialSubstituteId'
const masterBillOfMaterialItemId = computed((): string => {
  const id = (selecmedMasterRow.value as Record<string, unknown> | null)?.['billOfMaterialItemId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterBillOfMaterialItemId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getBillOfMaterialSubstituteId(record: BillOfMaterialSubstitute | Record<string, unknown>): string {
  return String((record as BillOfMaterialSubstitute)?.[entityIdName] ?? '')
}

function getBillOfMaterialSubstituteField(record: BillOfMaterialSubstitute | Record<string, unknown>, field: string): unknown {
  return (record as BillOfMaterialSubstitute)?.[field as keyof BillOfMaterialSubstitute]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'billOfMaterialSubstituteId',
    key: 'billOfMaterialSubstituteId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'billOfMaterialSubstituteId') ?? ''),
  },
  {
    title: pi.label('billOfMaterialItemId'),
    dataIndex: 'billOfMaterialItemId',
    key: 'billOfMaterialItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'billOfMaterialItemId') ?? ''),
  },
  {
    title: pi.label('billOfMaterialId'),
    dataIndex: 'billOfMaterialId',
    key: 'billOfMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'billOfMaterialId') ?? ''),
  },
  {
    title: pi.label('bomCode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'bomCode') ?? ''),
  },
  {
    title: pi.label('primaryMaterialCode'),
    dataIndex: 'primaryMaterialCode',
    key: 'primaryMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'primaryMaterialCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('substituteMaterialId'),
    dataIndex: 'substituteMaterialId',
    key: 'substituteMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteMaterialId') ?? ''),
  },
  {
    title: pi.label('substituteMaterialCode'),
    dataIndex: 'substituteMaterialCode',
    key: 'substituteMaterialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteMaterialCode') ?? ''),
  },
  {
    title: pi.label('substituteGroup'),
    dataIndex: 'substituteGroup',
    key: 'substituteGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substituteGroup') ?? ''),
  },
  {
    title: pi.label('substitutePriority'),
    dataIndex: 'substitutePriority',
    key: 'substitutePriority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'substitutePriority') ?? ''),
  },
  {
    title: pi.label('usageQuantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'usageQuantity') ?? ''),
  },
  {
    title: pi.label('materialUnit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'materialUnit') ?? ''),
  },
  {
    title: pi.label('usageRatio'),
    dataIndex: 'usageRatio',
    key: 'usageRatio',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'usageRatio') ?? ''),
  },
  {
    title: pi.label('isEnabled'),
    dataIndex: 'isEnabled',
    key: 'isEnabled',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'isEnabled') ?? ''),
  },
  {
    title: pi.label('effectiveDate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'effectiveDate') ?? ''),
  },
  {
    title: pi.label('expiryDate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'expiryDate') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: BillOfMaterialSubstitute }) =>
      String(getBillOfMaterialSubstituteField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:update',
        onClick: (record: BillOfMaterialSubstitute) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:delete',
        onClick: (record: BillOfMaterialSubstitute) => void handleDeleteOne(record),
      },
    ],
  }),
])

/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, 'company')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: 'billOfMaterialSubstituteId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of BILLOFMATERIALSUBSTITUTE_SUMMARY_SUM_FIELDS) {
      const num = Number(getBillOfMaterialSubstituteField(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: BillOfMaterialSubstitute[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: BillOfMaterialSubstitute, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getBillOfMaterialSubstituteId(selecmedRow.value) === getBillOfMaterialSubstituteId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: BillOfMaterialSubstitute[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: BillOfMaterialSubstitute) {
  const key = getBillOfMaterialSubstituteId(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BillOfMaterialSubstituteQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialSubstituteQuery>): BillOfMaterialSubstituteQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialSubstituteQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    billOfMaterialItemId: masterBillOfMaterialItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialSubstituteQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of BILLOFMATERIALSUBSTITUTE_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    query.substitutePriority = form.substitutePriority
  }
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    query.usageQuantity = form.usageQuantity
  }
  if (form.usageRatio !== undefined && form.usageRatio !== null) {
    query.usageRatio = form.usageRatio
  }
  if (form.isEnabled !== undefined && form.isEnabled !== null) {
    query.isEnabled = form.isEnabled
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
  }
  return query
}

async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getBillOfMaterialSubstituteList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(masterBillOfMaterialItemId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selecmed) => {
  if (selecmed) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: BillOfMaterialSubstitute) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getBillOfMaterialSubstituteById(getBillOfMaterialSubstituteId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
  }
}

async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    const id = formData.value?.billOfMaterialSubstituteId
    if (id) {
      await updateBillOfMaterialSubstitute(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createBillOfMaterialSubstitute(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: BillOfMaterialSubstitute) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialSubstituteById(getBillOfMaterialSubstituteId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getBillOfMaterialSubstituteId(r)).filter(Boolean)
      await deleteBillOfMaterialSubstituteBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

/** 打开导入对话框 */
function handleImport() {
  if (!hasMasterSelection.value) {
      message.warning(t('common.status.empty'))
      return
    }
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getBillOfMaterialSubstituteTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importBillOfMaterialSubstitute(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportBillOfMaterialSubstitute(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
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
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>

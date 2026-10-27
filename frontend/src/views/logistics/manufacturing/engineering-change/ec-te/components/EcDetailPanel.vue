<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-te/components -->
<!-- 文件名称：EcDetailPanel.vue -->
<!-- 功能描述：设变主表实体右侧明细 ecDetail 独立 CRUD（按主表选中 ecTeId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ec-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      export-permission="logistics:manufacturing:engineering:change:te:export"
      :show-create="false"
      :show-update="false"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-refresh="true"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :export-disabled="!hasMasterSelection"
      :export-loading="loading"
      :refresh-loading="loading"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="ec-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getEcDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ecDetailId"
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
      />
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <EcDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEcId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-engineering-change-ec-ec-detail"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ecCode')">
      <a-form-item :label="pi.label('ecCode')">
        <a-input
          v-model:value="advancedQueryForm.ecCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecCode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.label('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('lineNumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecModelCode')">
      <a-form-item :label="pi.label('ecModelCode')">
        <a-input
          v-model:value="advancedQueryForm.ecModelCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecModelCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecRootMaterialCode')">
      <a-form-item :label="pi.label('ecRootMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.ecRootMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecRootMaterialCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecRootMaterialDescription')">
      <a-form-item :label="pi.label('ecRootMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.ecRootMaterialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecRootMaterialDescription') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecParentMaterialCode')">
      <a-form-item :label="pi.label('ecParentMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.ecParentMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecParentMaterialCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecParentMaterialDescription')">
      <a-form-item :label="pi.label('ecParentMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.ecParentMaterialDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecParentMaterialDescription') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discontinuedStatus')">
      <a-form-item :label="pi.label('discontinuedStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.discontinuedStatus"
          dict-type="logistics_materials_material_discontinued_status"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('discontinuedStatus') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldMaterialCode')">
      <a-form-item :label="pi.label('ecOldMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.ecOldMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecOldMaterialCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldMaterialDescription')">
      <a-form-item :label="pi.label('ecOldMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.ecOldMaterialDescription"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecOldMaterialDescription') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldUsageQuantity')">
      <a-form-item :label="pi.label('ecOldUsageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldUsageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecOldUsageQuantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldItemPosition')">
      <a-form-item :label="pi.label('ecOldItemPosition')">
        <a-input
          v-model:value="advancedQueryForm.ecOldItemPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecOldItemPosition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldStock')">
      <a-form-item :label="pi.label('ecOldStock')">
        <a-input-number
          v-model:value="advancedQueryForm.ecOldStock"
          :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecOldStock') })"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldWarehouse')">
      <a-form-item :label="pi.label('ecOldWarehouse')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecOldWarehouse"
          api-url="TaktWarehouses/options"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldWarehouse') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldPurchaseType')">
      <a-form-item :label="pi.label('ecOldPurchaseType')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecOldPurchaseType"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldPurchaseType') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecOldRequiresInspection')">
      <a-form-item :label="pi.label('ecOldRequiresInspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecOldRequiresInspection"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecOldRequiresInspection') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewMaterialCode')">
      <a-form-item :label="pi.label('ecNewMaterialCode')">
        <a-input
          v-model:value="advancedQueryForm.ecNewMaterialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecNewMaterialCode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewMaterialDescription')">
      <a-form-item :label="pi.label('ecNewMaterialDescription')">
        <a-input
          v-model:value="advancedQueryForm.ecNewMaterialDescription"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecNewMaterialDescription') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewUsageQuantity')">
      <a-form-item :label="pi.label('ecNewUsageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewUsageQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecNewUsageQuantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewItemPosition')">
      <a-form-item :label="pi.label('ecNewItemPosition')">
        <a-input
          v-model:value="advancedQueryForm.ecNewItemPosition"
          :placeholder="t('common.page.form.placeholder.required', { field: pi.label('ecNewItemPosition') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewStock')">
      <a-form-item :label="pi.label('ecNewStock')">
        <a-input-number
          v-model:value="advancedQueryForm.ecNewStock"
          :placeholder="t('common.page.form.placeholder.optional', { field: pi.label('ecNewStock') })"
          :min="0"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewWarehouse')">
      <a-form-item :label="pi.label('ecNewWarehouse')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecNewWarehouse"
          api-url="TaktWarehouses/options"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewWarehouse') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewPurchaseType')">
      <a-form-item :label="pi.label('ecNewPurchaseType')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecNewPurchaseType"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewPurchaseType') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecNewRequiresInspection')">
      <a-form-item :label="pi.label('ecNewRequiresInspection')">
        <TaktSelect
          v-model:value="advancedQueryForm.ecNewRequiresInspection"
          dict-type="sys_yes_no"
          allow-clear
          :placeholder="t('common.page.form.placeholder.select', { field: pi.label('ecNewRequiresInspection') })"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateStart')">
      <a-form-item :label="pi.queryLabel('ecBomDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateStart"
          :placeholder="pi.queryPh('ecBomDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ecBomDateEnd')">
      <a-form-item :label="pi.queryLabel('ecBomDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.ecBomDateEnd"
          :placeholder="pi.queryPh('ecBomDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="t('common.page.entity.createdatstart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatstart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="t('common.page.entity.createdatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('common.page.entity.createdatend') })"
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
            <span>{{ t('common.page.entity.extfield') }}</span>
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
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.ecdetail._self"
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
      id-column-key="ecDetailId"
      entity-scope="company"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设变子表 ecDetail 右栏面板
 * @module views/logistics/manufacturing/engineering-change/ec-te/components
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
import { RiQuestionLine } from '@remixicon/vue'
import EcDetailForm from './EcDetailForm.vue'
import { useEcMasterContext } from '../composables/useEcMasterContext'
import {
  getEcDetailList,
  getEcDetailById,
  createEcDetail,
  updateEcDetail,
  deleteEcDetailById,
  deleteEcDetailBatch,
  getEcDetailTemplate,
  importEcDetail,
  exportEcDetail,
} from '@/api/logistics/manufacturing/engineering-change/ecDetail.api'
import type { EcDetail, EcDetailQuery } from '@/types/logistics/manufacturing/engineering-change/ecDetail.types'
import { useEcDetailI18n, buildEcDetailTableColumns, ECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS } from '@/views/logistics/manufacturing/engineering-change/ec-te/composables/useEcDetailI18n'

const { t } = useI18n()
const pi = useEcDetailI18n()
const { selecmedMasterRow } = useEcMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcDetail')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度裁掉横向滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

/** 按子表容器重算 scroll.y */
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap, { reserveSummaryRow: false })
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
const dataSource = ref<EcDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<EcDetail | null>(null)
const selecmedRows = ref<EcDetail[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<EcDetail>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ecCode: '',
  lineNumber: undefined as number | undefined,
  ecModelCode: '',
  ecRootMaterialCode: '',
  ecRootMaterialDescription: '',
  ecParentMaterialCode: '',
  ecParentMaterialDescription: '',
  ecOldMaterialCode: '',
  ecOldMaterialDescription: '',
  ecOldUsageQuantity: undefined as number | undefined,
  ecOldItemPosition: '',
  ecOldStock: undefined as number | undefined,
  ecOldWarehouse: '',
  ecOldPurchaseType: undefined as number | undefined,
  ecOldRequiresInspection: undefined as number | undefined,
  ecNewMaterialCode: '',
  ecNewMaterialDescription: '',
  ecNewUsageQuantity: undefined as number | undefined,
  ecNewItemPosition: '',
  ecNewStock: undefined as number | undefined,
  ecNewWarehouse: '',
  ecNewPurchaseType: undefined as number | undefined,
  ecNewRequiresInspection: undefined as number | undefined,
  discontinuedStatus: undefined as string | undefined,
  ecBomDateStart: '',
  ecBomDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ecCode', label: pi.label('ecCode') },
  { key: 'lineNumber', label: pi.label('lineNumber') },
  { key: 'ecModelCode', label: pi.label('ecModelCode') },
  { key: 'ecRootMaterialCode', label: pi.label('ecRootMaterialCode') },
  { key: 'ecRootMaterialDescription', label: pi.label('ecRootMaterialDescription') },
  { key: 'ecParentMaterialCode', label: pi.label('ecParentMaterialCode') },
  { key: 'ecParentMaterialDescription', label: pi.label('ecParentMaterialDescription') },
  { key: 'discontinuedStatus', label: pi.label('discontinuedStatus') },
  { key: 'ecOldMaterialCode', label: pi.label('ecOldMaterialCode') },
  { key: 'ecOldMaterialDescription', label: pi.label('ecOldMaterialDescription') },
  { key: 'ecOldUsageQuantity', label: pi.label('ecOldUsageQuantity') },
  { key: 'ecOldItemPosition', label: pi.label('ecOldItemPosition') },
  { key: 'ecOldStock', label: pi.label('ecOldStock') },
  { key: 'ecOldWarehouse', label: pi.label('ecOldWarehouse') },
  { key: 'ecOldPurchaseType', label: pi.label('ecOldPurchaseType') },
  { key: 'ecOldRequiresInspection', label: pi.label('ecOldRequiresInspection') },
  { key: 'ecNewMaterialCode', label: pi.label('ecNewMaterialCode') },
  { key: 'ecNewMaterialDescription', label: pi.label('ecNewMaterialDescription') },
  { key: 'ecNewUsageQuantity', label: pi.label('ecNewUsageQuantity') },
  { key: 'ecNewItemPosition', label: pi.label('ecNewItemPosition') },
  { key: 'ecNewStock', label: pi.label('ecNewStock') },
  { key: 'ecNewWarehouse', label: pi.label('ecNewWarehouse') },
  { key: 'ecNewPurchaseType', label: pi.label('ecNewPurchaseType') },
  { key: 'ecNewRequiresInspection', label: pi.label('ecNewRequiresInspection') },
  { key: 'ecBomDateStart', label: pi.queryLabel('ecBomDateStart') },
  { key: 'ecBomDateEnd', label: pi.queryLabel('ecBomDateEnd') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') },
])

/**
 * 高级查询字段标签
 * @param key 字段 key
 */
function fieldLabel(key: string): string {
  return queryFieldsMeta.value.find((f) => f.key === key)?.label ?? key
}

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  ecCode: '',
  lineNumber: undefined as number | undefined,
  ecModelCode: '',
  ecRootMaterialCode: '',
  ecRootMaterialDescription: '',
  ecParentMaterialCode: '',
  ecParentMaterialDescription: '',
  ecOldMaterialCode: '',
  ecOldMaterialDescription: '',
  ecOldUsageQuantity: undefined as number | undefined,
  ecOldItemPosition: '',
  ecOldStock: undefined as number | undefined,
  ecOldWarehouse: '',
  ecOldPurchaseType: undefined as number | undefined,
  ecOldRequiresInspection: undefined as number | undefined,
  ecNewMaterialCode: '',
  ecNewMaterialDescription: '',
  ecNewUsageQuantity: undefined as number | undefined,
  ecNewItemPosition: '',
  ecNewStock: undefined as number | undefined,
  ecNewWarehouse: '',
  ecNewPurchaseType: undefined as number | undefined,
  ecNewRequiresInspection: undefined as number | undefined,
  discontinuedStatus: undefined as string | undefined,
  ecBomDateStart: '',
  ecBomDateEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>(['ecDetailId', 'plantCode', ...ECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = ['ecDetailId', 'plantCode', ...ECDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'ecDetailId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.ecTeId)
const masterEcId = computed(() => selecmedMasterRow.value?.ecTeId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getEcDetailId(record: EcDetail | Record<string, unknown>): string {
  return String((record as EcDetail)?.[entityIdName] ?? '')
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ecDetailId',
    key: 'ecDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
  },
  ...buildEcDetailTableColumns((field) => pi.columnLabel(field)),
])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcDetail[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcDetail, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getEcDetailId(selecmedRow.value) === getEcDetailId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: EcDetail[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: EcDetail) {
  const key = getEcDetailId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcDetailQuery>): EcDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ecTeId: masterEcId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ecCode', form.ecCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('ecModelCode', form.ecModelCode)
  assignTrimmed('ecRootMaterialCode', form.ecRootMaterialCode)
  assignTrimmed('ecRootMaterialDescription', form.ecRootMaterialDescription)
  assignTrimmed('ecParentMaterialCode', form.ecParentMaterialCode)
  assignTrimmed('ecParentMaterialDescription', form.ecParentMaterialDescription)
  assignTrimmed('discontinuedStatus', form.discontinuedStatus)
  assignTrimmed('ecOldMaterialCode', form.ecOldMaterialCode)
  assignTrimmed('ecOldMaterialDescription', form.ecOldMaterialDescription)
  if (form.ecOldUsageQuantity !== undefined && form.ecOldUsageQuantity !== null) {
    query.ecOldUsageQuantity = form.ecOldUsageQuantity
  }
  assignTrimmed('ecOldItemPosition', form.ecOldItemPosition)
  if (form.ecOldStock !== undefined && form.ecOldStock !== null) {
    query.ecOldStock = form.ecOldStock
  }
  assignTrimmed('ecOldWarehouse', form.ecOldWarehouse)
  if (form.ecOldPurchaseType !== undefined && form.ecOldPurchaseType !== null) {
    query.ecOldPurchaseType = form.ecOldPurchaseType
  }
  if (form.ecOldRequiresInspection !== undefined && form.ecOldRequiresInspection !== null) {
    query.ecOldRequiresInspection = form.ecOldRequiresInspection
  }
  assignTrimmed('ecNewMaterialCode', form.ecNewMaterialCode)
  assignTrimmed('ecNewMaterialDescription', form.ecNewMaterialDescription)
  if (form.ecNewUsageQuantity !== undefined && form.ecNewUsageQuantity !== null) {
    query.ecNewUsageQuantity = form.ecNewUsageQuantity
  }
  assignTrimmed('ecNewItemPosition', form.ecNewItemPosition)
  if (form.ecNewStock !== undefined && form.ecNewStock !== null) {
    query.ecNewStock = form.ecNewStock
  }
  assignTrimmed('ecNewWarehouse', form.ecNewWarehouse)
  if (form.ecNewPurchaseType !== undefined && form.ecNewPurchaseType !== null) {
    query.ecNewPurchaseType = form.ecNewPurchaseType
  }
  if (form.ecNewRequiresInspection !== undefined && form.ecNewRequiresInspection !== null) {
    query.ecNewRequiresInspection = form.ecNewRequiresInspection
  }
  assignTrimmed('ecBomDateStart', form.ecBomDateStart)
  assignTrimmed('ecBomDateEnd', form.ecBomDateEnd)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
    const res = await getEcDetailList(buildListQuery())
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
watch(masterEcId, () => {
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

async function handleEdit(record: EcDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getEcDetailById(getEcDetailId(record))
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
    const id = formData.value?.ecDetailId
    if (id) {
      await updateEcDetail(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createEcDetail(payload)
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

async function handleDeleteOne(record: EcDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteEcDetailById(getEcDetailId(record))
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
      const ids = selecmedRows.value.map((r) => getEcDetailId(r)).filter(Boolean)
      await deleteEcDetailBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}

function handleImport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getEcDetailTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importEcDetail(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  void loadData()
  if (result.fail === 0) {
    setTimeout(() => {
      importVisible.value = false
    }, 2000)
  }
}

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
    const exportMeta = await exportEcDetail(
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

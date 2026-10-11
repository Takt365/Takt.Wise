<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：PcbaOutputDetailPanel.vue -->
<!-- 功能描述：PCBA日报实体 达成率主表实体右侧明细 pcbaOutputDetail 独立 CRUD（按主表选中 pcbaOutputId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="pcba-output-detail-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:output:pcba:create"
      update-permission="logistics:manufacturing:output:pcba:update"
      delete-permission="logistics:manufacturing:output:pcba:delete"
      import-permission="logistics:manufacturing:output:pcba:import"
      export-permission="logistics:manufacturing:output:pcba:export"
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
      class="pcba-output-detail-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="getPcbaOutputDetailId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="pcbaOutputDetailId"
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
      <PcbaOutputDetailForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterPcbaOutputId"
        :master-row="selectedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-output-pcba-output-pcba-output-detail"
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
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="pi.queryLabel('prodOrderCode')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderCode"
          :placeholder="pi.queryPh('prodOrderCode', 'required')"
          show-count
          :maxlength="12"
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
      <div v-show="isFieldVisible('timePeriod')">
      <a-form-item :label="pi.queryLabel('timePeriod')">
        <a-input
          v-model:value="advancedQueryForm.timePeriod"
          :placeholder="pi.queryPh('timePeriod', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamCode')">
      <a-form-item :label="pi.queryLabel('teamCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.teamCode"
          api-url="TaktProductionTeams/options"
          :api-params="pcbaTeamQueryOptionsParams"
          :placeholder="pi.queryPh('teamCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodEquipCode')">
      <a-form-item :label="pi.queryLabel('prodEquipCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.prodEquipCode"
          api-url="TaktProductionEquipments/options"
          :placeholder="pi.queryPh('prodEquipCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directLabor')">
      <a-form-item :label="pi.queryLabel('directLabor')">
        <a-input-number
          v-model:value="advancedQueryForm.directLabor"
          :placeholder="pi.queryPh('directLabor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectLabor')">
      <a-form-item :label="pi.queryLabel('indirectLabor')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectLabor"
          :placeholder="pi.queryPh('indirectLabor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="pi.queryLabel('shiftNo')">
        <TaktSelect
          v-model:value="advancedQueryForm.shiftNo"
          dict-type="logistics_manufacturing_shift_category"
          :placeholder="pi.queryPh('shiftNo', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdMinutes')">
      <a-form-item :label="pi.queryLabel('stdMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.stdMinutes"
          :placeholder="pi.queryPh('stdMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdLaborCapacity')">
      <a-form-item :label="pi.queryLabel('stdLaborCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdLaborCapacity"
          :placeholder="pi.queryPh('stdLaborCapacity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdShorts')">
      <a-form-item :label="pi.queryLabel('stdShorts')">
        <a-input-number
          v-model:value="advancedQueryForm.stdShorts"
          :placeholder="pi.queryPh('stdShorts', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdEquipmentCapacity')">
      <a-form-item :label="pi.queryLabel('stdEquipmentCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdEquipmentCapacity"
          :placeholder="pi.queryPh('stdEquipmentCapacity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pcbBoardType')">
      <a-form-item :label="pi.queryLabel('pcbBoardType')">
        <a-input
          v-model:value="advancedQueryForm.pcbBoardType"
          :placeholder="pi.queryPh('pcbBoardType', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('panelSide')">
      <a-form-item :label="pi.queryLabel('panelSide')">
        <TaktSelect
          v-model:value="advancedQueryForm.panelSide"
          dict-type="logistics_manufacturing_pcba_side_category"
          :placeholder="pi.queryPh('panelSide', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchQty')">
      <a-form-item :label="pi.queryLabel('batchQty')">
        <a-input-number
          v-model:value="advancedQueryForm.batchQty"
          :placeholder="pi.queryPh('batchQty', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dailyCompletedQty')">
      <a-form-item :label="pi.queryLabel('dailyCompletedQty')">
        <a-input-number
          v-model:value="advancedQueryForm.dailyCompletedQty"
          :placeholder="pi.queryPh('dailyCompletedQty', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalCompletedQty')">
      <a-form-item :label="pi.queryLabel('totalCompletedQty')">
        <a-input-number
          v-model:value="advancedQueryForm.totalCompletedQty"
          :placeholder="pi.queryPh('totalCompletedQty', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completedStatus')">
      <a-form-item :label="pi.queryLabel('completedStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.completedStatus"
          dict-type="logistics_manufacturing_pcba_completed_status"
          :placeholder="pi.queryPh('completedStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialCode')">
      <a-form-item :label="pi.queryLabel('serialCode')">
        <a-input
          v-model:value="advancedQueryForm.serialCode"
          :placeholder="pi.queryPh('serialCode', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectCount')">
      <a-form-item :label="pi.queryLabel('defectCount')">
        <a-input-number
          v-model:value="advancedQueryForm.defectCount"
          :placeholder="pi.queryPh('defectCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeMinutes')">
      <a-form-item :label="pi.queryLabel('downtimeMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.downtimeMinutes"
          :placeholder="pi.queryPh('downtimeMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeReason')">
      <a-form-item :label="pi.queryLabel('downtimeReason')">
        <a-input
          v-model:value="advancedQueryForm.downtimeReason"
          :placeholder="pi.queryPh('downtimeReason', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('downtimeDescription')">
      <a-form-item :label="pi.queryLabel('downtimeDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.downtimeDescription"
          :placeholder="pi.queryPh('downtimeDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inputMinutes')">
      <a-form-item :label="pi.queryLabel('inputMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.inputMinutes"
          :placeholder="pi.queryPh('inputMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualMinutes')">
      <a-form-item :label="pi.queryLabel('actualMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.actualMinutes"
          :placeholder="pi.queryPh('actualMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('repairMinutes')">
      <a-form-item :label="pi.queryLabel('repairMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.repairMinutes"
          :placeholder="pi.queryPh('repairMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('switchCount')">
      <a-form-item :label="pi.queryLabel('switchCount')">
        <a-input-number
          v-model:value="advancedQueryForm.switchCount"
          :placeholder="pi.queryPh('switchCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('switchTime')">
      <a-form-item :label="pi.queryLabel('switchTime')">
        <a-input-number
          v-model:value="advancedQueryForm.switchTime"
          :placeholder="pi.queryPh('switchTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stopTime')">
      <a-form-item :label="pi.queryLabel('stopTime')">
        <a-input-number
          v-model:value="advancedQueryForm.stopTime"
          :placeholder="pi.queryPh('stopTime', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalMinutes')">
      <a-form-item :label="pi.queryLabel('totalMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.totalMinutes"
          :placeholder="pi.queryPh('totalMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedReason')">
      <a-form-item :label="pi.queryLabel('unachievedReason')">
        <a-input
          v-model:value="advancedQueryForm.unachievedReason"
          :placeholder="pi.queryPh('unachievedReason', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unachievedDescription')">
      <a-form-item :label="pi.queryLabel('unachievedDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.unachievedDescription"
          :placeholder="pi.queryPh('unachievedDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('confirmMinutes')">
      <a-form-item :label="pi.queryLabel('confirmMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.confirmMinutes"
          :placeholder="pi.queryPh('confirmMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mixedProd')">
      <a-form-item :label="pi.queryLabel('mixedProd')">
        <a-input-number
          v-model:value="advancedQueryForm.mixedProd"
          :placeholder="pi.queryPh('mixedProd', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('achievementRate')">
      <a-form-item :label="pi.queryLabel('achievementRate')">
        <a-input-number
          v-model:value="advancedQueryForm.achievementRate"
          :placeholder="pi.queryPh('achievementRate', 'required')"
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
        :entity-i18n-key="PCBAOUTPUTDETAIL_SELF_I18N_KEY"
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
      id-column-key="pcbaOutputDetailId"
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
 * PCBA日报实体 达成率子表 pcbaOutputDetail 右栏面板
 * @module views/logistics/manufacturing/output/pcba-output/components
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
import PcbaOutputDetailForm from './PcbaOutputDetailForm.vue'
import { buildPcbaProductionTeamOptionsParams } from '../../composables/productionTeamCategory'
import { usePcbaOutputMasterContext } from '../composables/usePcbaOutputMasterContext'
import {
  getPcbaOutputDetailList,
  getPcbaOutputDetailById,
  createPcbaOutputDetail,
  updatePcbaOutputDetail,
  deletePcbaOutputDetailById,
  deletePcbaOutputDetailBatch,
  getPcbaOutputDetailTemplate,
  importPcbaOutputDetail,
  exportPcbaOutputDetail,
} from '@/api/logistics/manufacturing/output/pcbaOutputDetail.api'
import type { PcbaOutputDetail, PcbaOutputDetailQuery } from '@/types/logistics/manufacturing/output/pcbaOutputDetail.types'

import {
  usePcbaOutputDetailI18n,
  PCBAOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS,
  PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS,
  PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS,
  PCBAOUTPUTDETAIL_QUERY_FIELDS,
  PCBAOUTPUTDETAIL_SELF_I18N_KEY,
} from '../composables/usePcbaOutputDetailI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = usePcbaOutputDetailI18n()

const { t } = useI18n()
const { selectedMasterRow } = usePcbaOutputMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPcbaOutputDetail')
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
const dataSource = ref<PcbaOutputDetail[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<PcbaOutputDetail | null>(null)
const selectedRows = ref<PcbaOutputDetail[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<PcbaOutputDetail>>({})
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
  for (const key of PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.directLabor !== undefined && form.directLabor !== null) {
    return true
  }
  if (form.indirectLabor !== undefined && form.indirectLabor !== null) {
    return true
  }
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    return true
  }
  if (form.stdMinutes !== undefined && form.stdMinutes !== null) {
    return true
  }
  if (form.stdLaborCapacity !== undefined && form.stdLaborCapacity !== null) {
    return true
  }
  if (form.stdShorts !== undefined && form.stdShorts !== null) {
    return true
  }
  if (form.stdEquipmentCapacity !== undefined && form.stdEquipmentCapacity !== null) {
    return true
  }
  if (form.batchQty !== undefined && form.batchQty !== null) {
    return true
  }
  if (form.dailyCompletedQty !== undefined && form.dailyCompletedQty !== null) {
    return true
  }
  if (form.totalCompletedQty !== undefined && form.totalCompletedQty !== null) {
    return true
  }
  if (form.completedStatus !== undefined && form.completedStatus !== null) {
    return true
  }
  if (form.defectCount !== undefined && form.defectCount !== null) {
    return true
  }
  if (form.downtimeMinutes !== undefined && form.downtimeMinutes !== null) {
    return true
  }
  if (form.inputMinutes !== undefined && form.inputMinutes !== null) {
    return true
  }
  if (form.actualMinutes !== undefined && form.actualMinutes !== null) {
    return true
  }
  if (form.repairMinutes !== undefined && form.repairMinutes !== null) {
    return true
  }
  if (form.switchCount !== undefined && form.switchCount !== null) {
    return true
  }
  if (form.switchTime !== undefined && form.switchTime !== null) {
    return true
  }
  if (form.stopTime !== undefined && form.stopTime !== null) {
    return true
  }
  if (form.totalMinutes !== undefined && form.totalMinutes !== null) {
    return true
  }
  if (form.confirmMinutes !== undefined && form.confirmMinutes !== null) {
    return true
  }
  if (form.mixedProd !== undefined && form.mixedProd !== null) {
    return true
  }
  if (form.achievementRate !== undefined && form.achievementRate !== null) {
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
  const form = Object.fromEntries(PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    directLabor: undefined as number | undefined,
    indirectLabor: undefined as number | undefined,
    shiftNo: undefined as number | undefined,
    stdMinutes: undefined as number | undefined,
    stdLaborCapacity: undefined as number | undefined,
    stdShorts: undefined as number | undefined,
    stdEquipmentCapacity: undefined as number | undefined,
    batchQty: undefined as number | undefined,
    dailyCompletedQty: undefined as number | undefined,
    totalCompletedQty: undefined as number | undefined,
    completedStatus: undefined as number | undefined,
    defectCount: undefined as number | undefined,
    downtimeMinutes: undefined as number | undefined,
    inputMinutes: undefined as number | undefined,
    actualMinutes: undefined as number | undefined,
    repairMinutes: undefined as number | undefined,
    switchCount: undefined as number | undefined,
    switchTime: undefined as number | undefined,
    stopTime: undefined as number | undefined,
    totalMinutes: undefined as number | undefined,
    confirmMinutes: undefined as number | undefined,
    mixedProd: undefined as number | undefined,
    achievementRate: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())

/** 高级查询班组下拉（按工厂 + PCBA 分类） */
const pcbaTeamQueryOptionsParams = computed(() =>
  buildPcbaProductionTeamOptionsParams(advancedQueryForm.value.plantCode),
)
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  PCBAOUTPUTDETAIL_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const visibleColumnKeys = ref<string[]>([...PCBAOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...PCBAOUTPUTDETAIL_DEFAULT_VISIBLE_COLUMN_KEYS]
}
const importVisible = ref(false)

const entityIdName = 'pcbaOutputDetailId'
const masterPcbaOutputId = computed((): string => {
  const id = (selectedMasterRow.value as Record<string, unknown> | null)?.['pcbaOutputId']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => masterPcbaOutputId.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getPcbaOutputDetailId(record: PcbaOutputDetail | Record<string, unknown>): string {
  return String((record as PcbaOutputDetail)?.[entityIdName] ?? '')
}

function getPcbaOutputDetailField(record: PcbaOutputDetail | Record<string, unknown>, field: string): unknown {
  return (record as PcbaOutputDetail)?.[field as keyof PcbaOutputDetail]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'pcbaOutputDetailId',
    key: 'pcbaOutputDetailId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'pcbaOutputDetailId') ?? ''),
  },
  {
    title: pi.label('pcbaOutputId'),
    dataIndex: 'pcbaOutputId',
    key: 'pcbaOutputId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'pcbaOutputId') ?? ''),
  },
  {
    title: pi.label('prodOrderCode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'prodOrderCode') ?? ''),
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'lineNumber') ?? ''),
  },
  {
    title: pi.label('timePeriod'),
    dataIndex: 'timePeriod',
    key: 'timePeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'timePeriod') ?? ''),
  },
  {
    title: pi.label('teamCode'),
    dataIndex: 'teamCode',
    key: 'teamCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'teamCode') ?? ''),
  },
  {
    title: pi.label('prodEquipCode'),
    dataIndex: 'prodEquipCode',
    key: 'prodEquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'prodEquipCode') ?? ''),
  },
  {
    title: pi.label('directLabor'),
    dataIndex: 'directLabor',
    key: 'directLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'directLabor') ?? ''),
  },
  {
    title: pi.label('indirectLabor'),
    dataIndex: 'indirectLabor',
    key: 'indirectLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'indirectLabor') ?? ''),
  },
  {
    title: pi.label('shiftNo'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'shiftNo') ?? ''),
  },
  {
    title: pi.label('stdMinutes'),
    dataIndex: 'stdMinutes',
    key: 'stdMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'stdMinutes') ?? ''),
  },
  {
    title: pi.label('stdLaborCapacity'),
    dataIndex: 'stdLaborCapacity',
    key: 'stdLaborCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'stdLaborCapacity') ?? ''),
  },
  {
    title: pi.label('stdShorts'),
    dataIndex: 'stdShorts',
    key: 'stdShorts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'stdShorts') ?? ''),
  },
  {
    title: pi.label('stdEquipmentCapacity'),
    dataIndex: 'stdEquipmentCapacity',
    key: 'stdEquipmentCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'stdEquipmentCapacity') ?? ''),
  },
  {
    title: pi.label('pcbBoardType'),
    dataIndex: 'pcbBoardType',
    key: 'pcbBoardType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'pcbBoardType') ?? ''),
  },
  {
    title: pi.label('panelSide'),
    dataIndex: 'panelSide',
    key: 'panelSide',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'panelSide') ?? ''),
  },
  {
    title: pi.label('batchQty'),
    dataIndex: 'batchQty',
    key: 'batchQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'batchQty') ?? ''),
  },
  {
    title: pi.label('dailyCompletedQty'),
    dataIndex: 'dailyCompletedQty',
    key: 'dailyCompletedQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'dailyCompletedQty') ?? ''),
  },
  {
    title: pi.label('totalCompletedQty'),
    dataIndex: 'totalCompletedQty',
    key: 'totalCompletedQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'totalCompletedQty') ?? ''),
  },
  {
    title: pi.label('completedStatus'),
    dataIndex: 'completedStatus',
    key: 'completedStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'completedStatus') ?? ''),
  },
  {
    title: pi.label('serialCode'),
    dataIndex: 'serialCode',
    key: 'serialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'serialCode') ?? ''),
  },
  {
    title: pi.label('defectCount'),
    dataIndex: 'defectCount',
    key: 'defectCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'defectCount') ?? ''),
  },
  {
    title: pi.label('downtimeMinutes'),
    dataIndex: 'downtimeMinutes',
    key: 'downtimeMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'downtimeMinutes') ?? ''),
  },
  {
    title: pi.label('downtimeReason'),
    dataIndex: 'downtimeReason',
    key: 'downtimeReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'downtimeReason') ?? ''),
  },
  {
    title: pi.label('downtimeDescription'),
    dataIndex: 'downtimeDescription',
    key: 'downtimeDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'downtimeDescription') ?? ''),
  },
  {
    title: pi.label('inputMinutes'),
    dataIndex: 'inputMinutes',
    key: 'inputMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'inputMinutes') ?? ''),
  },
  {
    title: pi.label('actualMinutes'),
    dataIndex: 'actualMinutes',
    key: 'actualMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'actualMinutes') ?? ''),
  },
  {
    title: pi.label('repairMinutes'),
    dataIndex: 'repairMinutes',
    key: 'repairMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'repairMinutes') ?? ''),
  },
  {
    title: pi.label('switchCount'),
    dataIndex: 'switchCount',
    key: 'switchCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'switchCount') ?? ''),
  },
  {
    title: pi.label('switchTime'),
    dataIndex: 'switchTime',
    key: 'switchTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'switchTime') ?? ''),
  },
  {
    title: pi.label('stopTime'),
    dataIndex: 'stopTime',
    key: 'stopTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'stopTime') ?? ''),
  },
  {
    title: pi.label('totalMinutes'),
    dataIndex: 'totalMinutes',
    key: 'totalMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'totalMinutes') ?? ''),
  },
  {
    title: pi.label('unachievedReason'),
    dataIndex: 'unachievedReason',
    key: 'unachievedReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'unachievedReason') ?? ''),
  },
  {
    title: pi.label('unachievedDescription'),
    dataIndex: 'unachievedDescription',
    key: 'unachievedDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'unachievedDescription') ?? ''),
  },
  {
    title: pi.label('confirmMinutes'),
    dataIndex: 'confirmMinutes',
    key: 'confirmMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'confirmMinutes') ?? ''),
  },
  {
    title: pi.label('mixedProd'),
    dataIndex: 'mixedProd',
    key: 'mixedProd',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'mixedProd') ?? ''),
  },
  {
    title: pi.label('achievementRate'),
    dataIndex: 'achievementRate',
    key: 'achievementRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'achievementRate') ?? ''),
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: PcbaOutputDetail }) =>
      String(getPcbaOutputDetailField(record, 'isObsolete') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:pcba:update',
        onClick: (record: PcbaOutputDetail) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:pcba:delete',
        onClick: (record: PcbaOutputDetail) => void handleDeleteOne(record),
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
    idColumnKey: 'pcbaOutputDetailId',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: 'company',
  })
})

const summarySumFieldSet = new Set<string>(PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS)

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
    PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of PCBAOUTPUTDETAIL_SUMMARY_SUM_FIELDS) {
      const num = Number(getPcbaOutputDetailField(row, field))
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
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PcbaOutputDetail[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PcbaOutputDetail, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPcbaOutputDetailId(selectedRow.value) === getPcbaOutputDetailId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PcbaOutputDetail[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: PcbaOutputDetail) {
  const key = getPcbaOutputDetailId(record)
  return {
    onClick: () => {
      selectedRowKeys.value = [key]
      selectedRows.value = [record]
      selectedRow.value = record
    },
    class: selectedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selected cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PcbaOutputDetailQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PcbaOutputDetailQuery>): PcbaOutputDetailQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PcbaOutputDetailQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    pcbaOutputId: masterPcbaOutputId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PcbaOutputDetailQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of PCBAOUTPUTDETAIL_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.directLabor !== undefined && form.directLabor !== null) {
    query.directLabor = form.directLabor
  }
  if (form.indirectLabor !== undefined && form.indirectLabor !== null) {
    query.indirectLabor = form.indirectLabor
  }
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  if (form.stdMinutes !== undefined && form.stdMinutes !== null) {
    query.stdMinutes = form.stdMinutes
  }
  if (form.stdLaborCapacity !== undefined && form.stdLaborCapacity !== null) {
    query.stdLaborCapacity = form.stdLaborCapacity
  }
  if (form.stdShorts !== undefined && form.stdShorts !== null) {
    query.stdShorts = form.stdShorts
  }
  if (form.stdEquipmentCapacity !== undefined && form.stdEquipmentCapacity !== null) {
    query.stdEquipmentCapacity = form.stdEquipmentCapacity
  }
  if (form.batchQty !== undefined && form.batchQty !== null) {
    query.batchQty = form.batchQty
  }
  if (form.dailyCompletedQty !== undefined && form.dailyCompletedQty !== null) {
    query.dailyCompletedQty = form.dailyCompletedQty
  }
  if (form.totalCompletedQty !== undefined && form.totalCompletedQty !== null) {
    query.totalCompletedQty = form.totalCompletedQty
  }
  if (form.completedStatus !== undefined && form.completedStatus !== null) {
    query.completedStatus = form.completedStatus
  }
  if (form.defectCount !== undefined && form.defectCount !== null) {
    query.defectCount = form.defectCount
  }
  if (form.downtimeMinutes !== undefined && form.downtimeMinutes !== null) {
    query.downtimeMinutes = form.downtimeMinutes
  }
  if (form.inputMinutes !== undefined && form.inputMinutes !== null) {
    query.inputMinutes = form.inputMinutes
  }
  if (form.actualMinutes !== undefined && form.actualMinutes !== null) {
    query.actualMinutes = form.actualMinutes
  }
  if (form.repairMinutes !== undefined && form.repairMinutes !== null) {
    query.repairMinutes = form.repairMinutes
  }
  if (form.switchCount !== undefined && form.switchCount !== null) {
    query.switchCount = form.switchCount
  }
  if (form.switchTime !== undefined && form.switchTime !== null) {
    query.switchTime = form.switchTime
  }
  if (form.stopTime !== undefined && form.stopTime !== null) {
    query.stopTime = form.stopTime
  }
  if (form.totalMinutes !== undefined && form.totalMinutes !== null) {
    query.totalMinutes = form.totalMinutes
  }
  if (form.confirmMinutes !== undefined && form.confirmMinutes !== null) {
    query.confirmMinutes = form.confirmMinutes
  }
  if (form.mixedProd !== undefined && form.mixedProd !== null) {
    query.mixedProd = form.mixedProd
  }
  if (form.achievementRate !== undefined && form.achievementRate !== null) {
    query.achievementRate = form.achievementRate
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
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getPcbaOutputDetailList(buildListQuery())
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
watch(masterPcbaOutputId, () => {
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

watch(hasMasterSelection, (selected) => {
  if (selected) {
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

async function handleEdit(record: PcbaOutputDetail) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getPcbaOutputDetailById(getPcbaOutputDetailId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
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
    const id = formData.value?.pcbaOutputDetailId
    if (id) {
      await updatePcbaOutputDetail(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createPcbaOutputDetail(payload)
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

async function handleDeleteOne(record: PcbaOutputDetail) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePcbaOutputDetailById(getPcbaOutputDetailId(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
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
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getPcbaOutputDetailId(r)).filter(Boolean)
      await deletePcbaOutputDetailBatch(ids)
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
  const res = await getPcbaOutputDetailTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importPcbaOutputDetail(file, sheetName)
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
    const exportMeta = await exportPcbaOutputDetail(
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

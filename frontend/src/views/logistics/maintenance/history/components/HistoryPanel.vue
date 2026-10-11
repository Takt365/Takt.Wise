<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/history/components -->
<!-- 文件名称：HistoryPanel.vue -->
<!-- 功能描述：Takt工厂设备实体主表实体右侧明细 maintenanceHistory 独立 CRUD（按主表选中 equipmentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="history-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.maintenancehistory._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:maintenance:equipment:create"
      update-permission="logistics:maintenance:equipment:update"
      delete-permission="logistics:maintenance:equipment:delete"
      import-permission="logistics:maintenance:equipment:import"
      export-permission="logistics:maintenance:equipment:export"
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
    <div class="history-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaintenanceHistoryId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="maintenanceHistoryId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="single"
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
      <MaintenanceHistoryForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterEquipmentId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-maintenance-history-history"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('maintenanceWorkOrderId')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceworkorderid')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceWorkOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenanceworkorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="t('entity.maintenancehistory.workordercode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.workordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipCode')">
      <a-form-item :label="t('entity.maintenancehistory.EquipCode')">
        <a-input
          v-model:value="advancedQueryForm.EquipCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.EquipCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceType')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancetype')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceType"
          dict-type="logistics_maintenance_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancetype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCategory')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancecategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceCategory"
          dict-type="logistics_maintenance_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancecategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCompany')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancecompany')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceCompany"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancecompany') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceTechnician')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancetechnician')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceTechnician"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancetechnician') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDateStart')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStartTimeStart')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancestarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancestarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStartTimeEnd')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancestarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancestarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeStart')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenanceendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeEnd')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenanceendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceContent')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancecontent')">
        <a-textarea
          v-model:value="advancedQueryForm.maintenanceContent"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenancehistory.maintenancecontent') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.maintenancehistory.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenancehistory.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('solution')">
      <a-form-item :label="t('entity.maintenancehistory.solution')">
        <a-input
          v-model:value="advancedQueryForm.solution"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.solution') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usedParts')">
      <a-form-item :label="t('entity.maintenancehistory.usedparts')">
        <a-input
          v-model:value="advancedQueryForm.usedParts"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.usedparts') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCost')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancecost')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancecost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceResult')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceresult')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceResult"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenanceresult') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStatus')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancestatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.maintenancehistory.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.maintenancehistory.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCycleDays')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancecycledays')">
        <a-input-number
          v-model:value="advancedQueryForm.maintenanceCycleDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancecycledays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceDocuments')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancedocuments')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceDocuments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenancedocuments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceImages')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceimages')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.maintenanceimages') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedSummary')">
      <a-form-item :label="t('entity.maintenancehistory.acceptedsummary')">
        <a-input
          v-model:value="advancedQueryForm.acceptedSummary"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.acceptedsummary') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedBy')">
      <a-form-item :label="t('entity.maintenancehistory.acceptedby')">
        <a-input
          v-model:value="advancedQueryForm.acceptedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.acceptedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtStart')">
      <a-form-item :label="t('entity.maintenancehistory.acceptedatstart')">
        <a-input
          v-model:value="advancedQueryForm.acceptedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.acceptedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptedAtEnd')">
      <a-form-item :label="t('entity.maintenancehistory.acceptedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.acceptedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.acceptedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('archivedAtStart')">
      <a-form-item :label="t('entity.maintenancehistory.archivedatstart')">
        <a-input
          v-model:value="advancedQueryForm.archivedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.archivedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('archivedAtEnd')">
      <a-form-item :label="t('entity.maintenancehistory.archivedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.archivedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.archivedatend') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.maintenancehistory._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenancehistory._self"
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
      id-column-key="maintenanceHistoryId"
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
 * Takt工厂设备实体子表 maintenanceHistory 右栏面板
 * @module views/logistics/maintenance/history/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import MaintenanceHistoryForm from './HistoryForm.vue'
import { useEquipmentMasterContext } from '../composables/useEquipmentMasterContext'
import {
  getMaintenanceHistoryList,
  getMaintenanceHistoryById,
  createMaintenanceHistory,
  updateMaintenanceHistory,
  deleteMaintenanceHistoryById,
  deleteMaintenanceHistoryBatch,
  getMaintenanceHistoryTemplate,
  importMaintenanceHistory,
  exportMaintenanceHistory,
} from '@/api/logistics/maintenance/history.api'
import type { MaintenanceHistory, MaintenanceHistoryQuery } from '@/types/logistics/maintenance/history.types'

const { t } = useI18n()
const { selectedMasterRow } = useEquipmentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceHistory')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenancehistory._self') }),
)

const loading = ref(false)
const dataSource = ref<MaintenanceHistory[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaintenanceHistory | null>(null)
const selectedRows = ref<MaintenanceHistory[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaintenanceHistory>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  maintenanceWorkOrderId: '',
  workOrderCode: '',
  EquipCode: '',
  maintenanceType: undefined as number | undefined,
  maintenanceCategory: undefined as number | undefined,
  maintenanceCompany: '',
  maintenanceTechnician: '',
  maintenanceDateStart: '',
  maintenanceDateEnd: '',
  maintenanceStartTimeStart: '',
  maintenanceStartTimeEnd: '',
  maintenanceEndTimeStart: '',
  maintenanceEndTimeEnd: '',
  maintenanceContent: '',
  faultDescription: '',
  solution: '',
  usedParts: '',
  maintenanceCost: undefined as number | undefined,
  maintenanceResult: undefined as number | undefined,
  maintenanceStatus: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceDocuments: '',
  maintenanceImages: '',
  acceptedSummary: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  archivedAtStart: '',
  archivedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'maintenanceWorkOrderId', label: t('entity.maintenancehistory.maintenanceworkorderid') },
  { key: 'workOrderCode', label: t('entity.maintenancehistory.workordercode') },
  { key: 'EquipCode', label: t('entity.maintenancehistory.EquipCode') },
  { key: 'maintenanceType', label: t('entity.maintenancehistory.maintenancetype') },
  { key: 'maintenanceCategory', label: t('entity.maintenancehistory.maintenancecategory') },
  { key: 'maintenanceCompany', label: t('entity.maintenancehistory.maintenancecompany') },
  { key: 'maintenanceTechnician', label: t('entity.maintenancehistory.maintenancetechnician') },
  { key: 'maintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenancedate')) },
  { key: 'maintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenancedate')) },
  { key: 'maintenanceStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenancestarttime')) },
  { key: 'maintenanceStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenancestarttime')) },
  { key: 'maintenanceEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenanceendtime')) },
  { key: 'maintenanceEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.maintenanceendtime')) },
  { key: 'maintenanceContent', label: t('entity.maintenancehistory.maintenancecontent') },
  { key: 'faultDescription', label: t('entity.maintenancehistory.faultdescription') },
  { key: 'solution', label: t('entity.maintenancehistory.solution') },
  { key: 'usedParts', label: t('entity.maintenancehistory.usedparts') },
  { key: 'maintenanceCost', label: t('entity.maintenancehistory.maintenancecost') },
  { key: 'maintenanceResult', label: t('entity.maintenancehistory.maintenanceresult') },
  { key: 'maintenanceStatus', label: t('entity.maintenancehistory.maintenancestatus') },
  { key: 'nextMaintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.nextmaintenancedate')) },
  { key: 'nextMaintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancehistory.nextmaintenancedate')) },
  { key: 'maintenanceCycleDays', label: t('entity.maintenancehistory.maintenancecycledays') },
  { key: 'maintenanceDocuments', label: t('entity.maintenancehistory.maintenancedocuments') },
  { key: 'maintenanceImages', label: t('entity.maintenancehistory.maintenanceimages') },
  { key: 'acceptedSummary', label: t('entity.maintenancehistory.acceptedsummary') },
  { key: 'acceptedBy', label: t('entity.maintenancehistory.acceptedby') },
  { key: 'acceptedAtStart', label: t('entity.maintenancehistory.acceptedatstart') },
  { key: 'acceptedAtEnd', label: t('entity.maintenancehistory.acceptedatend') },
  { key: 'archivedAtStart', label: t('entity.maintenancehistory.archivedatstart') },
  { key: 'archivedAtEnd', label: t('entity.maintenancehistory.archivedatend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'extField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])

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
  maintenanceWorkOrderId: '',
  workOrderCode: '',
  EquipCode: '',
  maintenanceType: undefined as number | undefined,
  maintenanceCategory: undefined as number | undefined,
  maintenanceCompany: '',
  maintenanceTechnician: '',
  maintenanceDateStart: '',
  maintenanceDateEnd: '',
  maintenanceStartTimeStart: '',
  maintenanceStartTimeEnd: '',
  maintenanceEndTimeStart: '',
  maintenanceEndTimeEnd: '',
  maintenanceContent: '',
  faultDescription: '',
  solution: '',
  usedParts: '',
  maintenanceCost: undefined as number | undefined,
  maintenanceResult: undefined as number | undefined,
  maintenanceStatus: undefined as number | undefined,
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  maintenanceCycleDays: undefined as number | undefined,
  maintenanceDocuments: '',
  maintenanceImages: '',
  acceptedSummary: '',
  acceptedBy: '',
  acceptedAtStart: '',
  acceptedAtEnd: '',
  archivedAtStart: '',
  archivedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}
const importVisible = ref(false)

const entityIdName = 'maintenanceHistoryId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.equipmentId)
const masterEquipmentId = computed(() => selectedMasterRow.value?.equipmentId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaintenanceHistoryId(record: MaintenanceHistory | Record<string, unknown>): string {
  return String((record as MaintenanceHistory)?.[entityIdName] ?? '')
}

function getMaintenanceHistoryField(record: MaintenanceHistory | Record<string, unknown>, field: string): unknown {
  return (record as MaintenanceHistory)?.[field as keyof MaintenanceHistory]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceHistoryId',
    key: 'maintenanceHistoryId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceHistoryId') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenanceworkorderid'),
    dataIndex: 'maintenanceWorkOrderId',
    key: 'maintenanceWorkOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceWorkOrderId') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'workOrderCode') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.EquipCode'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'EquipCode') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenancetype'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceType') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceCategory') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenancecompany'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceCompany') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenancetechnician'),
    dataIndex: 'maintenanceTechnician',
    key: 'maintenanceTechnician',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceTechnician') ?? ''),
  },
  {
    title: t('entity.maintenancehistory.maintenancedate'),
    dataIndex: 'maintenanceDate',
    key: 'maintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceHistory }) =>
      String(getMaintenanceHistoryField(record, 'maintenanceDate') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:equipment:update',
        onClick: (record: MaintenanceHistory) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:equipment:delete',
        onClick: (record: MaintenanceHistory) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceHistory[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaintenanceHistory, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMaintenanceHistoryId(selectedRow.value) === getMaintenanceHistoryId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceHistory[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaintenanceHistory) {
  const key = getMaintenanceHistoryId(record)
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
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {MaintenanceHistoryQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaintenanceHistoryQuery>): MaintenanceHistoryQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaintenanceHistoryQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    equipmentId: masterEquipmentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaintenanceHistoryQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('maintenanceWorkOrderId', form.maintenanceWorkOrderId)
  assignTrimmed('workOrderCode', form.workOrderCode)
  assignTrimmed('EquipCode', form.EquipCode)
  if (form.maintenanceType !== undefined && form.maintenanceType !== null) {
    query.maintenanceType = form.maintenanceType
  }
  if (form.maintenanceCategory !== undefined && form.maintenanceCategory !== null) {
    query.maintenanceCategory = form.maintenanceCategory
  }
  assignTrimmed('maintenanceCompany', form.maintenanceCompany)
  assignTrimmed('maintenanceTechnician', form.maintenanceTechnician)
  assignTrimmed('maintenanceDateStart', form.maintenanceDateStart)
  assignTrimmed('maintenanceDateEnd', form.maintenanceDateEnd)
  assignTrimmed('maintenanceStartTimeStart', form.maintenanceStartTimeStart)
  assignTrimmed('maintenanceStartTimeEnd', form.maintenanceStartTimeEnd)
  assignTrimmed('maintenanceEndTimeStart', form.maintenanceEndTimeStart)
  assignTrimmed('maintenanceEndTimeEnd', form.maintenanceEndTimeEnd)
  assignTrimmed('maintenanceContent', form.maintenanceContent)
  assignTrimmed('faultDescription', form.faultDescription)
  assignTrimmed('solution', form.solution)
  assignTrimmed('usedParts', form.usedParts)
  if (form.maintenanceCost !== undefined && form.maintenanceCost !== null) {
    query.maintenanceCost = form.maintenanceCost
  }
  if (form.maintenanceResult !== undefined && form.maintenanceResult !== null) {
    query.maintenanceResult = form.maintenanceResult
  }
  if (form.maintenanceStatus !== undefined && form.maintenanceStatus !== null) {
    query.maintenanceStatus = form.maintenanceStatus
  }
  assignTrimmed('nextMaintenanceDateStart', form.nextMaintenanceDateStart)
  assignTrimmed('nextMaintenanceDateEnd', form.nextMaintenanceDateEnd)
  if (form.maintenanceCycleDays !== undefined && form.maintenanceCycleDays !== null) {
    query.maintenanceCycleDays = form.maintenanceCycleDays
  }
  assignTrimmed('maintenanceDocuments', form.maintenanceDocuments)
  assignTrimmed('maintenanceImages', form.maintenanceImages)
  assignTrimmed('acceptedSummary', form.acceptedSummary)
  assignTrimmed('acceptedBy', form.acceptedBy)
  assignTrimmed('acceptedAtStart', form.acceptedAtStart)
  assignTrimmed('acceptedAtEnd', form.acceptedAtEnd)
  assignTrimmed('archivedAtStart', form.archivedAtStart)
  assignTrimmed('archivedAtEnd', form.archivedAtEnd)
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
    selectedRowKeys.value = []
    selectedRows.value = []
    selectedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getMaintenanceHistoryList(buildListQuery())
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
watch(masterEquipmentId, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenancehistory._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaintenanceHistory) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenancehistory._self') })
  formLoading.value = true
  try {
    const detail = await getMaintenanceHistoryById(getMaintenanceHistoryId(record))
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
      entity: t('entity.maintenancehistory._self'),
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
    const id = formData.value?.maintenanceHistoryId
    if (id) {
      await updateMaintenanceHistory(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.maintenancehistory._self') }))
    } else {
      await createMaintenanceHistory(payload)
      message.success(t('common.feedback.created', { target: t('entity.maintenancehistory._self') }))
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

async function handleDeleteOne(record: MaintenanceHistory) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.maintenancehistory._self'),
      name: t('common.tip.this.target', { target: t('entity.maintenancehistory._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceHistoryById(getMaintenanceHistoryId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancehistory._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.maintenancehistory._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.maintenancehistory._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaintenanceHistoryId(r)).filter(Boolean)
      await deleteMaintenanceHistoryBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancehistory._self') }))
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
  const res = await getMaintenanceHistoryTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceHistory(file, sheetName)
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
    const exportMeta = await exportMaintenanceHistory(
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenancehistory._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.maintenancehistory._self') }))
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

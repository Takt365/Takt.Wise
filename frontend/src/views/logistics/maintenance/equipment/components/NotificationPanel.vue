<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/equipment/components -->
<!-- 文件名称：NotificationPanel.vue -->
<!-- 功能描述：Takt工厂设备实体主表实体右侧明细 maintenanceNotification 独立 CRUD（按主表选中 equipmentId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="notification-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.maintenancenotification._self') }}
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
    <div class="notification-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getMaintenanceNotificationId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="maintenanceNotificationId"
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
      <MaintenanceNotificationForm
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
      storage-key="takt-query-fields-logistics-maintenance-equipment-notification"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('common.page.entity.plantcode')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.plantcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationCode')">
      <a-form-item :label="t('entity.maintenancenotification.notificationcode')">
        <a-input
          v-model:value="advancedQueryForm.notificationCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('EquipCode')">
      <a-form-item :label="t('entity.maintenancenotification.EquipCode')">
        <a-input
          v-model:value="advancedQueryForm.EquipCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.EquipCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentName')">
      <a-form-item :label="t('entity.maintenancenotification.equipmentname')">
        <a-input
          v-model:value="advancedQueryForm.equipmentName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceCategory')">
      <a-form-item :label="t('entity.maintenancenotification.maintenancecategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.maintenanceCategory"
          dict-type="logistics_maintenance_category"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.maintenancecategory') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.maintenancenotification.priority')">
        <a-input-number
          v-model:value="advancedQueryForm.priority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.priority') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationStatus')">
      <a-form-item :label="t('entity.maintenancenotification.notificationstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.notificationStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('faultDescription')">
      <a-form-item :label="t('entity.maintenancenotification.faultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.faultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.maintenancenotification.faultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discoveredAtStart')">
      <a-form-item :label="t('entity.maintenancenotification.discoveredatstart')">
        <a-input
          v-model:value="advancedQueryForm.discoveredAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.discoveredatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discoveredAtEnd')">
      <a-form-item :label="t('entity.maintenancenotification.discoveredatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.discoveredAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.discoveredatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownStartTimeStart')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownstarttimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownStartTimeEnd')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownstarttimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownEndTimeStart')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownendtimestart') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownEndTimeEnd')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownendtimeend') })"
          value-format="YYYY-MM-DD HH:mm:ss"
          show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reportedBy')">
      <a-form-item :label="t('entity.maintenancenotification.reportedby')">
        <a-input
          v-model:value="advancedQueryForm.reportedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.reportedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterId')">
      <a-form-item :label="t('entity.maintenancenotification.costcenterid')">
        <a-input
          v-model:value="advancedQueryForm.costCenterId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.costcenterid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costCenterCode')">
      <a-form-item :label="t('entity.maintenancenotification.costcentercode')">
        <a-input
          v-model:value="advancedQueryForm.costCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.costcentercode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceWorkOrderId')">
      <a-form-item :label="t('entity.maintenancenotification.maintenanceworkorderid')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceWorkOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.maintenanceworkorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceWorkOrderCode')">
      <a-form-item :label="t('entity.maintenancenotification.maintenanceworkordercode')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceWorkOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.maintenanceworkordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('notificationImages')">
      <a-form-item :label="t('entity.maintenancenotification.notificationimages')">
        <a-input
          v-model:value="advancedQueryForm.notificationImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.notificationimages') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.maintenancenotification.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.maintenancenotification.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.maintenancenotification.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.maintenancenotification.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.maintenancenotification.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.maintenancenotification.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.maintenancenotification.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.maintenancenotification.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.flowinstanceid') })"
          show-count
          :maxlength="20"
          allow-clear
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
      :title="t('common.dialog.title.import', { entity: t('entity.maintenancenotification._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.maintenancenotification._self"
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
      id-column-key="maintenanceNotificationId"
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
 * Takt工厂设备实体子表 maintenanceNotification 右栏面板
 * @module views/logistics/maintenance/equipment/components
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
import MaintenanceNotificationForm from './NotificationForm.vue'
import { useEquipmentMasterContext } from '../composables/useEquipmentMasterContext'
import {
  getMaintenanceNotificationList,
  getMaintenanceNotificationById,
  createMaintenanceNotification,
  updateMaintenanceNotification,
  deleteMaintenanceNotificationById,
  deleteMaintenanceNotificationBatch,
  getMaintenanceNotificationTemplate,
  importMaintenanceNotification,
  exportMaintenanceNotification,
} from '@/api/logistics/maintenance/notification.api'
import type { MaintenanceNotification, MaintenanceNotificationQuery } from '@/types/logistics/maintenance/notification.types'

const { t } = useI18n()
const { selectedMasterRow } = useEquipmentMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceNotification')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenancenotification._self') }),
)

const loading = ref(false)
const dataSource = ref<MaintenanceNotification[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<MaintenanceNotification | null>(null)
const selectedRows = ref<MaintenanceNotification[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<MaintenanceNotification>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  plantCode: '',
  notificationCode: '',
  EquipCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  priority: undefined as number | undefined,
  notificationStatus: undefined as number | undefined,
  faultDescription: '',
  discoveredAtStart: '',
  discoveredAtEnd: '',
  breakdownStartTimeStart: '',
  breakdownStartTimeEnd: '',
  breakdownEndTimeStart: '',
  breakdownEndTimeEnd: '',
  reportedBy: '',
  costCenterId: '',
  costCenterCode: '',
  maintenanceWorkOrderId: '',
  maintenanceWorkOrderCode: '',
  notificationImages: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('common.page.entity.plantcode') },
  { key: 'notificationCode', label: t('entity.maintenancenotification.notificationcode') },
  { key: 'EquipCode', label: t('entity.maintenancenotification.EquipCode') },
  { key: 'equipmentName', label: t('entity.maintenancenotification.equipmentname') },
  { key: 'maintenanceCategory', label: t('entity.maintenancenotification.maintenancecategory') },
  { key: 'priority', label: t('entity.maintenancenotification.priority') },
  { key: 'notificationStatus', label: t('entity.maintenancenotification.notificationstatus') },
  { key: 'faultDescription', label: t('entity.maintenancenotification.faultdescription') },
  { key: 'discoveredAtStart', label: t('entity.maintenancenotification.discoveredatstart') },
  { key: 'discoveredAtEnd', label: t('entity.maintenancenotification.discoveredatend') },
  { key: 'breakdownStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancenotification.breakdownstarttime')) },
  { key: 'breakdownStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancenotification.breakdownstarttime')) },
  { key: 'breakdownEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.maintenancenotification.breakdownendtime')) },
  { key: 'breakdownEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.maintenancenotification.breakdownendtime')) },
  { key: 'reportedBy', label: t('entity.maintenancenotification.reportedby') },
  { key: 'costCenterId', label: t('entity.maintenancenotification.costcenterid') },
  { key: 'costCenterCode', label: t('entity.maintenancenotification.costcentercode') },
  { key: 'maintenanceWorkOrderId', label: t('entity.maintenancenotification.maintenanceworkorderid') },
  { key: 'maintenanceWorkOrderCode', label: t('entity.maintenancenotification.maintenanceworkordercode') },
  { key: 'notificationImages', label: t('entity.maintenancenotification.notificationimages') },
  { key: 'approvalStatus', label: t('entity.maintenancenotification.approvalstatus') },
  { key: 'initiatorId', label: t('entity.maintenancenotification.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.maintenancenotification.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.maintenancenotification.initiatedatend') },
  { key: 'approvedBy', label: t('entity.maintenancenotification.approvedby') },
  { key: 'approvedAtStart', label: t('entity.maintenancenotification.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.maintenancenotification.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.maintenancenotification.flowinstanceid') },
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
  plantCode: '',
  notificationCode: '',
  EquipCode: '',
  equipmentName: '',
  maintenanceCategory: undefined as number | undefined,
  priority: undefined as number | undefined,
  notificationStatus: undefined as number | undefined,
  faultDescription: '',
  discoveredAtStart: '',
  discoveredAtEnd: '',
  breakdownStartTimeStart: '',
  breakdownStartTimeEnd: '',
  breakdownEndTimeStart: '',
  breakdownEndTimeEnd: '',
  reportedBy: '',
  costCenterId: '',
  costCenterCode: '',
  maintenanceWorkOrderId: '',
  maintenanceWorkOrderCode: '',
  notificationImages: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  flowInstanceId: '',
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

const entityIdName = 'maintenanceNotificationId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.equipmentId)
const masterEquipmentId = computed(() => selectedMasterRow.value?.equipmentId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getMaintenanceNotificationId(record: MaintenanceNotification | Record<string, unknown>): string {
  return String((record as MaintenanceNotification)?.[entityIdName] ?? '')
}

function getMaintenanceNotificationField(record: MaintenanceNotification | Record<string, unknown>, field: string): unknown {
  return (record as MaintenanceNotification)?.[field as keyof MaintenanceNotification]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceNotificationId',
    key: 'maintenanceNotificationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'maintenanceNotificationId') ?? ''),
  },
  {
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'plantCode') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.notificationcode'),
    dataIndex: 'notificationCode',
    key: 'notificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'notificationCode') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.EquipCode'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'EquipCode') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'equipmentName') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'maintenanceCategory') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'priority') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.notificationstatus'),
    dataIndex: 'notificationStatus',
    key: 'notificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'notificationStatus') ?? ''),
  },
  {
    title: t('entity.maintenancenotification.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: MaintenanceNotification }) =>
      String(getMaintenanceNotificationField(record, 'faultDescription') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:equipment:update',
        onClick: (record: MaintenanceNotification) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:equipment:delete',
        onClick: (record: MaintenanceNotification) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceNotification[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaintenanceNotification, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getMaintenanceNotificationId(selectedRow.value) === getMaintenanceNotificationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceNotification[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: MaintenanceNotification) {
  const key = getMaintenanceNotificationId(record)
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
 * @returns {MaintenanceNotificationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<MaintenanceNotificationQuery>): MaintenanceNotificationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: MaintenanceNotificationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    equipmentId: masterEquipmentId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof MaintenanceNotificationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('plantCode', form.plantCode)
  assignTrimmed('notificationCode', form.notificationCode)
  assignTrimmed('EquipCode', form.EquipCode)
  assignTrimmed('equipmentName', form.equipmentName)
  if (form.maintenanceCategory !== undefined && form.maintenanceCategory !== null) {
    query.maintenanceCategory = form.maintenanceCategory
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
  if (form.notificationStatus !== undefined && form.notificationStatus !== null) {
    query.notificationStatus = form.notificationStatus
  }
  assignTrimmed('faultDescription', form.faultDescription)
  assignTrimmed('discoveredAtStart', form.discoveredAtStart)
  assignTrimmed('discoveredAtEnd', form.discoveredAtEnd)
  assignTrimmed('breakdownStartTimeStart', form.breakdownStartTimeStart)
  assignTrimmed('breakdownStartTimeEnd', form.breakdownStartTimeEnd)
  assignTrimmed('breakdownEndTimeStart', form.breakdownEndTimeStart)
  assignTrimmed('breakdownEndTimeEnd', form.breakdownEndTimeEnd)
  assignTrimmed('reportedBy', form.reportedBy)
  assignTrimmed('costCenterId', form.costCenterId)
  assignTrimmed('costCenterCode', form.costCenterCode)
  assignTrimmed('maintenanceWorkOrderId', form.maintenanceWorkOrderId)
  assignTrimmed('maintenanceWorkOrderCode', form.maintenanceWorkOrderCode)
  assignTrimmed('notificationImages', form.notificationImages)
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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
    const res = await getMaintenanceNotificationList(buildListQuery())
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenancenotification._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: MaintenanceNotification) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenancenotification._self') })
  formLoading.value = true
  try {
    const detail = await getMaintenanceNotificationById(getMaintenanceNotificationId(record))
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
      entity: t('entity.maintenancenotification._self'),
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
    const id = formData.value?.maintenanceNotificationId
    if (id) {
      await updateMaintenanceNotification(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.maintenancenotification._self') }))
    } else {
      await createMaintenanceNotification(payload)
      message.success(t('common.feedback.created', { target: t('entity.maintenancenotification._self') }))
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

async function handleDeleteOne(record: MaintenanceNotification) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.maintenancenotification._self'),
      name: t('common.tip.this.target', { target: t('entity.maintenancenotification._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceNotificationById(getMaintenanceNotificationId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancenotification._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.maintenancenotification._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.maintenancenotification._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getMaintenanceNotificationId(r)).filter(Boolean)
      await deleteMaintenanceNotificationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancenotification._self') }))
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
  const res = await getMaintenanceNotificationTemplate(sheetName, fileName)
  return (res as { data?: Blob }).data ?? (res as Blob)
}

async function handleImportFile(
  file: File,
  sheetName?: string,
): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceNotification(file, sheetName)
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
    const exportMeta = await exportMaintenanceNotification(
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenancenotification._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.maintenancenotification._self') }))
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

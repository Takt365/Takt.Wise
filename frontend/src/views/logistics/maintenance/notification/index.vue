<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/notification -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：维护通知单实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:maintenance:notification:create"
      update-permission="logistics:maintenance:notification:update"
      delete-permission="logistics:maintenance:notification:delete"
      import-permission="logistics:maintenance:notification:import"
      export-permission="logistics:maintenance:notification:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="approval"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'maintenanceNotificationId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMaintenanceNotificationId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'maintenanceCategory'">
          <TaktDictTag
            :value="getMaintenanceNotificationField(record, 'maintenanceCategory')"
            dict-type="logistics_maintenance_category"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <MaintenanceNotificationForm
        :key="formData?.maintenanceNotificationId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-maintenance-notification'"
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
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentId')">
      <a-form-item :label="t('entity.maintenancenotification.equipmentid')">
        <a-input
          v-model:value="advancedQueryForm.equipmentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancenotification.equipmentid') })"
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
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownStartTimeEnd')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownEndTimeStart')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('breakdownEndTimeEnd')">
      <a-form-item :label="t('entity.maintenancenotification.breakdownendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.breakdownEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancenotification.breakdownendtimeend') })"
          value-format="YYYY-MM-DD"
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
          :maxlength="50"
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
          :maxlength="50"
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
          :maxlength="2000"
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

    <!-- 导入对话框 -->
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'maintenanceNotificationId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 维护通知单实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/notification
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import MaintenanceNotificationForm from './components/NotificationForm.vue'
import { getMaintenanceNotificationList, getMaintenanceNotificationById, createMaintenanceNotification, updateMaintenanceNotification, deleteMaintenanceNotificationById, deleteMaintenanceNotificationBatch, getMaintenanceNotificationTemplate, importMaintenanceNotification, exportMaintenanceNotification, updateMaintenanceNotificationStatus } from '@/api/logistics/maintenance/notification.api'
import type { MaintenanceNotification, MaintenanceNotificationQuery } from '@/types/logistics/maintenance/notification.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceNotification')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenancenotification._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaintenanceNotification[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<MaintenanceNotification | null>(null)
/** 表格多选行 */
const selectedRows = ref<MaintenanceNotification[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaintenanceNotification> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  plantCode: '',
  notificationCode: '',
  equipmentId: '',
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
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'plantCode', label: t('common.page.entity.plantcode') },
  { key: 'notificationCode', label: t('entity.maintenancenotification.notificationcode') },
  { key: 'equipmentId', label: t('entity.maintenancenotification.equipmentid') },
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
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'maintenanceNotificationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

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
  assignTrimmed('equipmentId', form.equipmentId)
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
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'maintenanceNotificationId',
    key: 'maintenanceNotificationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'maintenanceNotificationId') ?? ''
  },
  {
    title: t('common.page.entity.plantcode'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'plantCode') ?? ''
  },
  {
    title: t('entity.maintenancenotification.notificationcode'),
    dataIndex: 'notificationCode',
    key: 'notificationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'notificationCode') ?? ''
  },
  {
    title: t('entity.maintenancenotification.equipmentid'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'equipmentId') ?? ''
  },
  {
    title: t('entity.maintenancenotification.EquipCode'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'EquipCode') ?? ''
  },
  {
    title: t('entity.maintenancenotification.equipmentname'),
    dataIndex: 'equipmentName',
    key: 'equipmentName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'equipmentName') ?? ''
  },
  {
    title: t('entity.maintenancenotification.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenancenotification.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'priority') ?? ''
  },
  {
    title: t('entity.maintenancenotification.notificationstatus'),
    dataIndex: 'notificationStatus',
    key: 'notificationStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'notificationStatus') ?? ''
  },
  {
    title: t('entity.maintenancenotification.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'faultDescription') ?? ''
  },
  {
    title: t('entity.maintenancenotification.discoveredat'),
    dataIndex: 'discoveredAt',
    key: 'discoveredAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'discoveredAt') ?? ''
  },
  {
    title: t('entity.maintenancenotification.breakdownstarttime'),
    dataIndex: 'breakdownStartTime',
    key: 'breakdownStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'breakdownStartTime') ?? ''
  },
  {
    title: t('entity.maintenancenotification.breakdownendtime'),
    dataIndex: 'breakdownEndTime',
    key: 'breakdownEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'breakdownEndTime') ?? ''
  },
  {
    title: t('entity.maintenancenotification.reportedby'),
    dataIndex: 'reportedBy',
    key: 'reportedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'reportedBy') ?? ''
  },
  {
    title: t('entity.maintenancenotification.costcenterid'),
    dataIndex: 'costCenterId',
    key: 'costCenterId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'costCenterId') ?? ''
  },
  {
    title: t('entity.maintenancenotification.costcentercode'),
    dataIndex: 'costCenterCode',
    key: 'costCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'costCenterCode') ?? ''
  },
  {
    title: t('entity.maintenancenotification.maintenanceworkorderid'),
    dataIndex: 'maintenanceWorkOrderId',
    key: 'maintenanceWorkOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'maintenanceWorkOrderId') ?? ''
  },
  {
    title: t('entity.maintenancenotification.maintenanceworkordercode'),
    dataIndex: 'maintenanceWorkOrderCode',
    key: 'maintenanceWorkOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'maintenanceWorkOrderCode') ?? ''
  },
  {
    title: t('entity.maintenancenotification.notificationimages'),
    dataIndex: 'notificationImages',
    key: 'notificationImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'notificationImages') ?? ''
  },
  {
    title: t('entity.maintenancenotification.equipment'),
    dataIndex: 'equipment',
    key: 'equipment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'equipment') ?? ''
  },
  {
    title: t('entity.maintenancenotification.maintenanceworkorder'),
    dataIndex: 'maintenanceWorkOrder',
    key: 'maintenanceWorkOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceNotificationField(record, 'maintenanceWorkOrder') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:notification:update',
        onClick: (record: MaintenanceNotification) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:notification:delete',
        onClick: (record: MaintenanceNotification) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaintenanceNotificationId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaintenanceNotificationField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
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
    } else if (selectedRow.value && getMaintenanceNotificationId(selectedRow.value) === getMaintenanceNotificationId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: MaintenanceNotification[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: MaintenanceNotification) => ({
  onClick: () => {
    const key = getMaintenanceNotificationId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getMaintenanceNotificationId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaintenanceNotificationList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaintenanceNotification] 加载数据失败', { error })
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
  plantCode: '',
  notificationCode: '',
  equipmentId: '',
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenancenotification._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: MaintenanceNotification) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenancenotification._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.maintenancenotification._self') }))
  }
}
/** 提交新增/编辑表单 */
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
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateMaintenanceNotification(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.maintenancenotification._self') }))
    } else {
      await createMaintenanceNotification(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.maintenancenotification._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getMaintenanceNotificationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceNotification(file, sheetName)
}

/** 导入完成回调：刷新列表并可选关闭对话框 */
function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenancenotification._self') }))
  } catch (error: any) {
    logger.error('[MaintenanceNotification] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.maintenancenotification._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaintenanceNotification) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.maintenancenotification._self'), name: t('common.tip.this.target', { target: t('entity.maintenancenotification._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceNotificationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancenotification._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.maintenancenotification._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.maintenancenotification._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaintenanceNotificationBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancenotification._self') }))
      loadData()
    }
  })
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
  plantCode: '',
  notificationCode: '',
  equipmentId: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

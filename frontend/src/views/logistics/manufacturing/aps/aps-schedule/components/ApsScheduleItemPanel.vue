<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/aps/aps-schedule/components -->
<!-- 文件名称：ApsScheduleItemPanel.vue -->
<!-- 功能描述：APS排程主表主表实体右侧明细 apsScheduleItem 独立 CRUD（按主表选中 apsScheduleId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="aps-schedule-item-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.apsscheduleitem._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:manufacturing:aps:schedule:create"
      update-permission="logistics:manufacturing:aps:schedule:update"
      delete-permission="logistics:manufacturing:aps:schedule:delete"
      import-permission="logistics:manufacturing:aps:schedule:import"
      export-permission="logistics:manufacturing:aps:schedule:export"
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
    <div class="aps-schedule-item-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getApsScheduleItemId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="apsScheduleItemId"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
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
      <ApsScheduleItemForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterApsScheduleId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-manufacturing-scheduling-aps-schedule-aps-schedule-item"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('apsScheduleCode')">
      <a-form-item :label="t('entity.apsscheduleitem.apsschedulecode')">
        <a-input
          v-model:value="advancedQueryForm.apsScheduleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.apsschedulecode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('apsOrderId')">
      <a-form-item :label="t('entity.apsscheduleitem.apsorderid')">
        <a-input
          v-model:value="advancedQueryForm.apsOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.apsorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('apsOperationId')">
      <a-form-item :label="t('entity.apsscheduleitem.apsoperationid')">
        <a-input
          v-model:value="advancedQueryForm.apsOperationId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.apsoperationid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routingItemId')">
      <a-form-item :label="t('entity.apsscheduleitem.routingitemid')">
        <a-input
          v-model:value="advancedQueryForm.routingItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.routingitemid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.apsscheduleitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workOrderCode')">
      <a-form-item :label="t('entity.apsscheduleitem.workordercode')">
        <a-input
          v-model:value="advancedQueryForm.workOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workordercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCode')">
      <a-form-item :label="t('entity.apsscheduleitem.productcode')">
        <a-input
          v-model:value="advancedQueryForm.productCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productName')">
      <a-form-item :label="t('entity.apsscheduleitem.productname')">
        <a-input
          v-model:value="advancedQueryForm.productName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.productname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenterCode')">
      <a-form-item :label="t('entity.apsscheduleitem.workcentercode')">
        <a-input
          v-model:value="advancedQueryForm.workCenterCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workcentercode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenterName')">
      <a-form-item :label="t('entity.apsscheduleitem.workcentername')">
        <a-input
          v-model:value="advancedQueryForm.workCenterName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.workcentername') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processCode')">
      <a-form-item :label="t('entity.apsscheduleitem.processcode')">
        <a-input
          v-model:value="advancedQueryForm.processCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processName')">
      <a-form-item :label="t('entity.apsscheduleitem.processname')">
        <a-input
          v-model:value="advancedQueryForm.processName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processSequence')">
      <a-form-item :label="t('entity.apsscheduleitem.processsequence')">
        <a-input-number
          v-model:value="advancedQueryForm.processSequence"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processsequence') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processStandardST')">
      <a-form-item :label="t('entity.apsscheduleitem.processstandardst')">
        <a-input-number
          v-model:value="advancedQueryForm.processStandardST"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processstandardst') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processStandardSTUnit')">
      <a-form-item :label="t('entity.apsscheduleitem.processstandardstunit')">
        <a-input-number
          v-model:value="advancedQueryForm.processStandardSTUnit"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processstandardstunit') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extraMinutes')">
      <a-form-item :label="t('entity.apsscheduleitem.extraminutes')">
        <a-input-number
          v-model:value="advancedQueryForm.extraMinutes"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.extraminutes') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planQuantity')">
      <a-form-item :label="t('entity.apsscheduleitem.planquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.planQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.planquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeStart')">
      <a-form-item :label="t('entity.apsscheduleitem.planstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.planstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planStartTimeEnd')">
      <a-form-item :label="t('entity.apsscheduleitem.planstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.planstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeStart')">
      <a-form-item :label="t('entity.apsscheduleitem.planendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.planendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planEndTimeEnd')">
      <a-form-item :label="t('entity.apsscheduleitem.planendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.planendtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeStart')">
      <a-form-item :label="t('entity.apsscheduleitem.actualstarttimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.actualstarttimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualStartTimeEnd')">
      <a-form-item :label="t('entity.apsscheduleitem.actualstarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.actualstarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeStart')">
      <a-form-item :label="t('entity.apsscheduleitem.actualendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.actualendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualEndTimeEnd')">
      <a-form-item :label="t('entity.apsscheduleitem.actualendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.apsscheduleitem.actualendtimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('processStatus')">
      <a-form-item :label="t('entity.apsscheduleitem.processstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.processStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.processstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priority')">
      <a-form-item :label="t('entity.apsscheduleitem.priority')">
        <a-input-number
          v-model:value="advancedQueryForm.priority"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.apsscheduleitem.priority') })"
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
    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.apsscheduleitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.apsscheduleitem._self"
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
      id-column-key="apsScheduleItemId"
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
 * APS排程主表子表 apsScheduleItem 右栏面板
 * @module views/logistics/manufacturing/aps/aps-schedule/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import ApsScheduleItemForm from './ApsScheduleItemForm.vue'
import { useApsScheduleMasterContext } from '../composables/useApsScheduleMasterContext'
import {
  getApsScheduleItemList,
  getApsScheduleItemById,
  createApsScheduleItem,
  updateApsScheduleItem,
  deleteApsScheduleItemById,
  deleteApsScheduleItemBatch,
  getApsScheduleItemTemplate,
  importApsScheduleItem,
  exportApsScheduleItem,
} from '@/api/logistics/manufacturing/aps/apsScheduleItem.api'
import type { ApsScheduleItem, ApsScheduleItemQuery } from '@/types/logistics/manufacturing/aps/apsScheduleItem.types'

const { t } = useI18n()
const { selectedMasterRow } = useApsScheduleMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktApsScheduleItem')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.apsscheduleitem._self') }),
)

const loading = ref(false)
const dataSource = ref<ApsScheduleItem[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selectedRow = ref<ApsScheduleItem | null>(null)
const selectedRows = ref<ApsScheduleItem[]>([])
const selectedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<ApsScheduleItem>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  apsScheduleCode: '',
  apsOrderId: '',
  apsOperationId: '',
  routingItemId: '',
  lineNumber: undefined as number | undefined,
  workOrderCode: '',
  productCode: '',
  productName: '',
  workCenterCode: '',
  workCenterName: '',
  processCode: '',
  processName: '',
  processSequence: undefined as number | undefined,
  processStandardST: undefined as number | undefined,
  processStandardSTUnit: undefined as number | undefined,
  extraMinutes: undefined as number | undefined,
  planQuantity: undefined as number | undefined,
  planStartTimeStart: '',
  planStartTimeEnd: '',
  planEndTimeStart: '',
  planEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  processStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'apsScheduleCode', label: t('entity.apsscheduleitem.apsschedulecode') },
  { key: 'apsOrderId', label: t('entity.apsscheduleitem.apsorderid') },
  { key: 'apsOperationId', label: t('entity.apsscheduleitem.apsoperationid') },
  { key: 'routingItemId', label: t('entity.apsscheduleitem.routingitemid') },
  { key: 'lineNumber', label: t('entity.apsscheduleitem.linenumber') },
  { key: 'workOrderCode', label: t('entity.apsscheduleitem.workordercode') },
  { key: 'productCode', label: t('entity.apsscheduleitem.productcode') },
  { key: 'productName', label: t('entity.apsscheduleitem.productname') },
  { key: 'workCenterCode', label: t('entity.apsscheduleitem.workcentercode') },
  { key: 'workCenterName', label: t('entity.apsscheduleitem.workcentername') },
  { key: 'processCode', label: t('entity.apsscheduleitem.processcode') },
  { key: 'processName', label: t('entity.apsscheduleitem.processname') },
  { key: 'processSequence', label: t('entity.apsscheduleitem.processsequence') },
  { key: 'processStandardST', label: t('entity.apsscheduleitem.processstandardst') },
  { key: 'processStandardSTUnit', label: t('entity.apsscheduleitem.processstandardstunit') },
  { key: 'extraMinutes', label: t('entity.apsscheduleitem.extraminutes') },
  { key: 'planQuantity', label: t('entity.apsscheduleitem.planquantity') },
  { key: 'planStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.planstarttime')) },
  { key: 'planStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.planstarttime')) },
  { key: 'planEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.planendtime')) },
  { key: 'planEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.planendtime')) },
  { key: 'actualStartTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.actualstarttime')) },
  { key: 'actualStartTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.actualstarttime')) },
  { key: 'actualEndTimeStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.actualendtime')) },
  { key: 'actualEndTimeEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.apsscheduleitem.actualendtime')) },
  { key: 'processStatus', label: t('entity.apsscheduleitem.processstatus') },
  { key: 'priority', label: t('entity.apsscheduleitem.priority') },
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
  apsScheduleCode: '',
  apsOrderId: '',
  apsOperationId: '',
  routingItemId: '',
  lineNumber: undefined as number | undefined,
  workOrderCode: '',
  productCode: '',
  productName: '',
  workCenterCode: '',
  workCenterName: '',
  processCode: '',
  processName: '',
  processSequence: undefined as number | undefined,
  processStandardST: undefined as number | undefined,
  processStandardSTUnit: undefined as number | undefined,
  extraMinutes: undefined as number | undefined,
  planQuantity: undefined as number | undefined,
  planStartTimeStart: '',
  planStartTimeEnd: '',
  planEndTimeStart: '',
  planEndTimeEnd: '',
  actualStartTimeStart: '',
  actualStartTimeEnd: '',
  actualEndTimeStart: '',
  actualEndTimeEnd: '',
  processStatus: undefined as number | undefined,
  priority: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
  }
}
const columnSettingVisible = ref(false)
/** 表格当前可见列 key（空数组时按 tableMode=masterDetailDetail 默认 id+4 业务列） */
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

const entityIdName = 'apsScheduleItemId'
const hasMasterSelection = computed(() => !!selectedMasterRow.value?.apsScheduleId)
const masterApsScheduleId = computed(() => selectedMasterRow.value?.apsScheduleId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selectedRows.value.length === 0)

function getApsScheduleItemId(record: ApsScheduleItem | Record<string, unknown>): string {
  return String((record as ApsScheduleItem)?.[entityIdName] ?? '')
}

function getApsScheduleItemField(record: ApsScheduleItem | Record<string, unknown>, field: string): unknown {
  return (record as ApsScheduleItem)?.[field as keyof ApsScheduleItem]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'apsScheduleItemId',
    key: 'apsScheduleItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'apsScheduleItemId') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.apsschedulecode'),
    dataIndex: 'apsScheduleCode',
    key: 'apsScheduleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'apsScheduleCode') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.apsorderid'),
    dataIndex: 'apsOrderId',
    key: 'apsOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'apsOrderId') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.apsoperationid'),
    dataIndex: 'apsOperationId',
    key: 'apsOperationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'apsOperationId') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.routingitemid'),
    dataIndex: 'routingItemId',
    key: 'routingItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'routingItemId') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'workOrderCode') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.productcode'),
    dataIndex: 'productCode',
    key: 'productCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'productCode') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.productname'),
    dataIndex: 'productName',
    key: 'productName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'productName') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.workcentercode'),
    dataIndex: 'workCenterCode',
    key: 'workCenterCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'workCenterCode') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.workcentername'),
    dataIndex: 'workCenterName',
    key: 'workCenterName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'workCenterName') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processcode'),
    dataIndex: 'processCode',
    key: 'processCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processCode') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processname'),
    dataIndex: 'processName',
    key: 'processName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processName') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processsequence'),
    dataIndex: 'processSequence',
    key: 'processSequence',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processSequence') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processstandardst'),
    dataIndex: 'processStandardST',
    key: 'processStandardST',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processStandardST') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processstandardstunit'),
    dataIndex: 'processStandardSTUnit',
    key: 'processStandardSTUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processStandardSTUnit') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.extraminutes'),
    dataIndex: 'extraMinutes',
    key: 'extraMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'extraMinutes') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.planquantity'),
    dataIndex: 'planQuantity',
    key: 'planQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'planQuantity') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.planstarttime'),
    dataIndex: 'planStartTime',
    key: 'planStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'planStartTime') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.planendtime'),
    dataIndex: 'planEndTime',
    key: 'planEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'planEndTime') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.actualstarttime'),
    dataIndex: 'actualStartTime',
    key: 'actualStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'actualStartTime') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.actualendtime'),
    dataIndex: 'actualEndTime',
    key: 'actualEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'actualEndTime') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.processstatus'),
    dataIndex: 'processStatus',
    key: 'processStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'processStatus') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.priority'),
    dataIndex: 'priority',
    key: 'priority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'priority') ?? ''),
  },
  {
    title: t('entity.apsscheduleitem.schedule'),
    dataIndex: 'schedule',
    key: 'schedule',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ApsScheduleItem }) =>
      String(getApsScheduleItemField(record, 'schedule') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:aps:schedule:update',
        onClick: (record: ApsScheduleItem) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:aps:schedule:delete',
        onClick: (record: ApsScheduleItem) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ApsScheduleItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ApsScheduleItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getApsScheduleItemId(selectedRow.value) === getApsScheduleItemId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ApsScheduleItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ApsScheduleItem) {
  const key = getApsScheduleItemId(record)
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
 * @returns {ApsScheduleItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ApsScheduleItemQuery>): ApsScheduleItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ApsScheduleItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    apsScheduleId: masterApsScheduleId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ApsScheduleItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('apsScheduleCode', form.apsScheduleCode)
  assignTrimmed('apsOrderId', form.apsOrderId)
  assignTrimmed('apsOperationId', form.apsOperationId)
  assignTrimmed('routingItemId', form.routingItemId)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('workOrderCode', form.workOrderCode)
  assignTrimmed('productCode', form.productCode)
  assignTrimmed('productName', form.productName)
  assignTrimmed('workCenterCode', form.workCenterCode)
  assignTrimmed('workCenterName', form.workCenterName)
  assignTrimmed('processCode', form.processCode)
  assignTrimmed('processName', form.processName)
  if (form.processSequence !== undefined && form.processSequence !== null) {
    query.processSequence = form.processSequence
  }
  if (form.processStandardST !== undefined && form.processStandardST !== null) {
    query.processStandardST = form.processStandardST
  }
  if (form.processStandardSTUnit !== undefined && form.processStandardSTUnit !== null) {
    query.processStandardSTUnit = form.processStandardSTUnit
  }
  if (form.extraMinutes !== undefined && form.extraMinutes !== null) {
    query.extraMinutes = form.extraMinutes
  }
  if (form.planQuantity !== undefined && form.planQuantity !== null) {
    query.planQuantity = form.planQuantity
  }
  assignTrimmed('planStartTimeStart', form.planStartTimeStart)
  assignTrimmed('planStartTimeEnd', form.planStartTimeEnd)
  assignTrimmed('planEndTimeStart', form.planEndTimeStart)
  assignTrimmed('planEndTimeEnd', form.planEndTimeEnd)
  assignTrimmed('actualStartTimeStart', form.actualStartTimeStart)
  assignTrimmed('actualStartTimeEnd', form.actualStartTimeEnd)
  assignTrimmed('actualEndTimeStart', form.actualEndTimeStart)
  assignTrimmed('actualEndTimeEnd', form.actualEndTimeEnd)
  if (form.processStatus !== undefined && form.processStatus !== null) {
    query.processStatus = form.processStatus
  }
  if (form.priority !== undefined && form.priority !== null) {
    query.priority = form.priority
  }
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
    const res = await getApsScheduleItemList(buildListQuery())
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
watch(masterApsScheduleId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.apsscheduleitem._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ApsScheduleItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.apsscheduleitem._self') })
  formLoading.value = true
  try {
    const detail = await getApsScheduleItemById(getApsScheduleItemId(record))
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
      entity: t('entity.apsscheduleitem._self'),
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
    const id = formData.value?.apsScheduleItemId
    if (id) {
      await updateApsScheduleItem(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.apsscheduleitem._self') }))
    } else {
      await createApsScheduleItem(payload)
      message.success(t('common.feedback.created', { target: t('entity.apsscheduleitem._self') }))
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

async function handleDeleteOne(record: ApsScheduleItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.apsscheduleitem._self'),
      name: t('common.tip.this.target', { target: t('entity.apsscheduleitem._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteApsScheduleItemById(getApsScheduleItemId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.apsscheduleitem._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.apsscheduleitem._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.apsscheduleitem._self'),
      count: selectedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r) => getApsScheduleItemId(r)).filter(Boolean)
      await deleteApsScheduleItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.apsscheduleitem._self') }))
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
  const res = await getApsScheduleItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importApsScheduleItem(file, sheetName)
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
    const exportMeta = await exportApsScheduleItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.apsscheduleitem._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.apsscheduleitem._self') }))
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

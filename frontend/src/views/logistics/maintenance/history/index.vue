<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/history -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设备维护履历实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:maintenance:history:create"
      update-permission="logistics:maintenance:history:update"
      delete-permission="logistics:maintenance:history:delete"
      import-permission="logistics:maintenance:history:import"
      export-permission="logistics:maintenance:history:export"
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
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'maintenanceHistoryId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getMaintenanceHistoryId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'maintenanceType'">
          <TaktDictTag
            :value="getMaintenanceHistoryField(record, 'maintenanceType')"
            dict-type="logistics_maintenance_type"
          />
        </template>
        <template v-else-if="column.key === 'maintenanceCategory'">
          <TaktDictTag
            :value="getMaintenanceHistoryField(record, 'maintenanceCategory')"
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
      <MaintenanceHistoryForm
        :key="formData?.maintenanceHistoryId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-maintenance-history'"
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
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('equipmentId')">
      <a-form-item :label="t('entity.maintenancehistory.equipmentid')">
        <a-input
          v-model:value="advancedQueryForm.equipmentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenancehistory.equipmentid') })"
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
          :maxlength="200"
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
          :maxlength="50"
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
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStartTimeEnd')">
      <a-form-item :label="t('entity.maintenancehistory.maintenancestarttimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceStartTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenancestarttimeend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeStart')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceendtimestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenanceendtimestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceEndTimeEnd')">
      <a-form-item :label="t('entity.maintenancehistory.maintenanceendtimeend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceEndTimeEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.maintenancehistory.maintenanceendtimeend') })"
          value-format="YYYY-MM-DD"
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
          :maxlength="2000"
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
          :maxlength="4000"
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
          :maxlength="2000"
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
          :maxlength="2000"
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
          :maxlength="500"
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
          :maxlength="50"
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

    <!-- 导入对话框 -->
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
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'maintenanceHistoryId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 设备维护履历实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/maintenance/history
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import MaintenanceHistoryForm from './components/HistoryForm.vue'
import { getMaintenanceHistoryList, getMaintenanceHistoryById, createMaintenanceHistory, updateMaintenanceHistory, deleteMaintenanceHistoryById, deleteMaintenanceHistoryBatch, getMaintenanceHistoryTemplate, importMaintenanceHistory, exportMaintenanceHistory, updateMaintenanceHistoryStatus } from '@/api/logistics/maintenance/history.api'
import type { MaintenanceHistory, MaintenanceHistoryQuery } from '@/types/logistics/maintenance/history.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktMaintenanceHistory')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.maintenancehistory._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<MaintenanceHistory[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<MaintenanceHistory | null>(null)
/** 表格多选行 */
const selecmedRows = ref<MaintenanceHistory[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<MaintenanceHistory> | null>(null)
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
  maintenanceWorkOrderId: '',
  workOrderCode: '',
  equipmentId: '',
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
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'maintenanceWorkOrderId', label: t('entity.maintenancehistory.maintenanceworkorderid') },
  { key: 'workOrderCode', label: t('entity.maintenancehistory.workordercode') },
  { key: 'equipmentId', label: t('entity.maintenancehistory.equipmentid') },
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
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'maintenanceHistoryId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

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
  assignTrimmed('equipmentId', form.equipmentId)
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
    dataIndex: 'maintenanceHistoryId',
    key: 'maintenanceHistoryId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceHistoryId') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenanceworkorderid'),
    dataIndex: 'maintenanceWorkOrderId',
    key: 'maintenanceWorkOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceWorkOrderId') ?? ''
  },
  {
    title: t('entity.maintenancehistory.workordercode'),
    dataIndex: 'workOrderCode',
    key: 'workOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'workOrderCode') ?? ''
  },
  {
    title: t('entity.maintenancehistory.equipmentid'),
    dataIndex: 'equipmentId',
    key: 'equipmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'equipmentId') ?? ''
  },
  {
    title: t('entity.maintenancehistory.EquipCode'),
    dataIndex: 'EquipCode',
    key: 'EquipCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'EquipCode') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancetype'),
    dataIndex: 'maintenanceType',
    key: 'maintenanceType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenancehistory.maintenancecategory'),
    dataIndex: 'maintenanceCategory',
    key: 'maintenanceCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.maintenancehistory.maintenancecompany'),
    dataIndex: 'maintenanceCompany',
    key: 'maintenanceCompany',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceCompany') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancetechnician'),
    dataIndex: 'maintenanceTechnician',
    key: 'maintenanceTechnician',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceTechnician') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancedate'),
    dataIndex: 'maintenanceDate',
    key: 'maintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceDate') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancestarttime'),
    dataIndex: 'maintenanceStartTime',
    key: 'maintenanceStartTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceStartTime') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenanceendtime'),
    dataIndex: 'maintenanceEndTime',
    key: 'maintenanceEndTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceEndTime') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancecontent'),
    dataIndex: 'maintenanceContent',
    key: 'maintenanceContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceContent') ?? ''
  },
  {
    title: t('entity.maintenancehistory.faultdescription'),
    dataIndex: 'faultDescription',
    key: 'faultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'faultDescription') ?? ''
  },
  {
    title: t('entity.maintenancehistory.solution'),
    dataIndex: 'solution',
    key: 'solution',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'solution') ?? ''
  },
  {
    title: t('entity.maintenancehistory.usedparts'),
    dataIndex: 'usedParts',
    key: 'usedParts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'usedParts') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancecost'),
    dataIndex: 'maintenanceCost',
    key: 'maintenanceCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceCost') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenanceresult'),
    dataIndex: 'maintenanceResult',
    key: 'maintenanceResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceResult') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancestatus'),
    dataIndex: 'maintenanceStatus',
    key: 'maintenanceStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceStatus') ?? ''
  },
  {
    title: t('entity.maintenancehistory.nextmaintenancedate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancecycledays'),
    dataIndex: 'maintenanceCycleDays',
    key: 'maintenanceCycleDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceCycleDays') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenancedocuments'),
    dataIndex: 'maintenanceDocuments',
    key: 'maintenanceDocuments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceDocuments') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenanceimages'),
    dataIndex: 'maintenanceImages',
    key: 'maintenanceImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceImages') ?? ''
  },
  {
    title: t('entity.maintenancehistory.acceptedsummary'),
    dataIndex: 'acceptedSummary',
    key: 'acceptedSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'acceptedSummary') ?? ''
  },
  {
    title: t('entity.maintenancehistory.acceptedby'),
    dataIndex: 'acceptedBy',
    key: 'acceptedBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'acceptedBy') ?? ''
  },
  {
    title: t('entity.maintenancehistory.acceptedat'),
    dataIndex: 'acceptedAt',
    key: 'acceptedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'acceptedAt') ?? ''
  },
  {
    title: t('entity.maintenancehistory.archivedat'),
    dataIndex: 'archivedAt',
    key: 'archivedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'archivedAt') ?? ''
  },
  {
    title: t('entity.maintenancehistory.equipment'),
    dataIndex: 'equipment',
    key: 'equipment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'equipment') ?? ''
  },
  {
    title: t('entity.maintenancehistory.maintenanceworkorder'),
    dataIndex: 'maintenanceWorkOrder',
    key: 'maintenanceWorkOrder',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getMaintenanceHistoryField(record, 'maintenanceWorkOrder') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:maintenance:history:update',
        onClick: (record: MaintenanceHistory) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:maintenance:history:delete',
        onClick: (record: MaintenanceHistory) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getMaintenanceHistoryId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getMaintenanceHistoryField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: MaintenanceHistory[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: MaintenanceHistory, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getMaintenanceHistoryId(selecmedRow.value) === getMaintenanceHistoryId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: MaintenanceHistory[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: MaintenanceHistory) => ({
  onClick: () => {
    const key = getMaintenanceHistoryId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getMaintenanceHistoryId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getMaintenanceHistoryList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[MaintenanceHistory] 加载数据失败', { error })
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
  maintenanceWorkOrderId: '',
  workOrderCode: '',
  equipmentId: '',
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
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.maintenancehistory._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: MaintenanceHistory) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.maintenancehistory._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.maintenancehistory._self') }))
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
      await updateMaintenanceHistory(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.maintenancehistory._self') }))
    } else {
      await createMaintenanceHistory(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.maintenancehistory._self') }))
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
  const res = await getMaintenanceHistoryTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importMaintenanceHistory(file, sheetName)
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
    const exportMeta = await exportMaintenanceHistory(
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
    message.success(t('common.feedback.export.success', { target: t('entity.maintenancehistory._self') }))
  } catch (error: any) {
    logger.error('[MaintenanceHistory] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.maintenancehistory._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: MaintenanceHistory) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.maintenancehistory._self'), name: t('common.tip.this.target', { target: t('entity.maintenancehistory._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteMaintenanceHistoryById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancehistory._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.maintenancehistory._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.maintenancehistory._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteMaintenanceHistoryBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.maintenancehistory._self') }))
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
  maintenanceWorkOrderId: '',
  workOrderCode: '',
  equipmentId: '',
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

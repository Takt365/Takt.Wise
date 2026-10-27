<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint-handling -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客诉处理记录实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="logistics:quality:complaint:customer:handling:create"
      update-permission="logistics:quality:complaint:customer:handling:update"
      delete-permission="logistics:quality:complaint:customer:handling:delete"
      import-permission="logistics:quality:complaint:customer:handling:import"
      export-permission="logistics:quality:complaint:customer:handling:export"
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
      :id-column-key="'customerComplaintHandlingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getCustomerComplaintHandlingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <CustomerComplaintHandlingForm
        :key="formData?.customerComplaintHandlingId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-customer-complaint-handling'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('complaintHandlingCode')">
      <a-form-item :label="t('entity.customercomplainthandling.complainthandlingcode')">
        <a-input
          v-model:value="advancedQueryForm.complaintHandlingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complainthandlingcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintId')">
      <a-form-item :label="t('entity.customercomplainthandling.complaintid')">
        <a-input
          v-model:value="advancedQueryForm.complaintId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintCode')">
      <a-form-item :label="t('entity.customercomplainthandling.complaintCode')">
        <a-input
          v-model:value="advancedQueryForm.complaintCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintItemId')">
      <a-form-item :label="t('entity.customercomplainthandling.complaintitemid')">
        <a-input
          v-model:value="advancedQueryForm.complaintItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.complaintitemid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStage')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingstage')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingStage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingstage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingMethod')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingDescription')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.handlingDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.customercomplainthandling.handlingdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('causeAnalysis')">
      <a-form-item :label="t('entity.customercomplainthandling.causeanalysis')">
        <a-input
          v-model:value="advancedQueryForm.causeAnalysis"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.causeanalysis') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('correctiveAction')">
      <a-form-item :label="t('entity.customercomplainthandling.correctiveaction')">
        <a-input
          v-model:value="advancedQueryForm.correctiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.correctiveaction') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('preventiveAction')">
      <a-form-item :label="t('entity.customercomplainthandling.preventiveaction')">
        <a-input
          v-model:value="advancedQueryForm.preventiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.preventiveaction') })"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDept')">
      <a-form-item :label="t('entity.customercomplainthandling.responsibledept')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.responsibledept') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleBy')">
      <a-form-item :label="t('entity.customercomplainthandling.responsibleby')">
        <a-input
          v-model:value="advancedQueryForm.responsibleBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.responsibleby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlerBy')">
      <a-form-item :label="t('entity.customercomplainthandling.handlerby')">
        <a-input
          v-model:value="advancedQueryForm.handlerBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlerby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateStart')">
      <a-form-item :label="t('entity.customercomplainthandling.plannedcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.plannedcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plannedCompletionDateEnd')">
      <a-form-item :label="t('entity.customercomplainthandling.plannedcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.plannedCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.plannedcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateStart')">
      <a-form-item :label="t('entity.customercomplainthandling.actualcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.actualcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualCompletionDateEnd')">
      <a-form-item :label="t('entity.customercomplainthandling.actualcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.customercomplainthandling.actualcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStatus')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingCost')">
      <a-form-item :label="t('entity.customercomplainthandling.handlingcost')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingCost"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.handlingcost') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerFeedback')">
      <a-form-item :label="t('entity.customercomplainthandling.customerfeedback')">
        <a-input
          v-model:value="advancedQueryForm.customerFeedback"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.customerfeedback') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerSatisfaction')">
      <a-form-item :label="t('entity.customercomplainthandling.customersatisfaction')">
        <a-input-number
          v-model:value="advancedQueryForm.customerSatisfaction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.customersatisfaction') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="t('entity.customercomplainthandling.filename')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.filename') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accessUrl')">
      <a-form-item :label="t('entity.customercomplainthandling.accessurl')">
        <a-input
          v-model:value="advancedQueryForm.accessUrl"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.customercomplainthandling.accessurl') })"
          show-count
          :maxlength="1000"
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
      :title="t('common.dialog.title.import', { entity: t('entity.customercomplainthandling._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.customercomplainthandling._self"
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
      :id-column-key="'customerComplaintHandlingId'"
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
 * 客诉处理记录实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint-handling
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import CustomerComplaintHandlingForm from './components/CustomerComplaintHandlingForm.vue'
import { getCustomerComplaintHandlingList, getCustomerComplaintHandlingById, createCustomerComplaintHandling, updateCustomerComplaintHandling, deleteCustomerComplaintHandlingById, deleteCustomerComplaintHandlingBatch, getCustomerComplaintHandlingTemplate, importCustomerComplaintHandling, exportCustomerComplaintHandling, updateCustomerComplaintHandlingStatus } from '@/api/logistics/quality/complaint/customerComplaintHandling.api'
import type { CustomerComplaintHandling, CustomerComplaintHandlingQuery } from '@/types/logistics/quality/complaint/customerComplaintHandling.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaintHandling')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.customercomplainthandling._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerComplaintHandling[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<CustomerComplaintHandling | null>(null)
/** 表格多选行 */
const selecmedRows = ref<CustomerComplaintHandling[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerComplaintHandling> | null>(null)
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
  complaintHandlingCode: '',
  complaintId: '',
  complaintCode: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  fileName: '',
  accessUrl: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'complaintHandlingCode', label: t('entity.customercomplainthandling.complainthandlingcode') },
  { key: 'complaintId', label: t('entity.customercomplainthandling.complaintid') },
  { key: 'complaintCode', label: t('entity.customercomplainthandling.complaintCode') },
  { key: 'complaintItemId', label: t('entity.customercomplainthandling.complaintitemid') },
  { key: 'handlingStage', label: t('entity.customercomplainthandling.handlingstage') },
  { key: 'handlingMethod', label: t('entity.customercomplainthandling.handlingmethod') },
  { key: 'handlingDescription', label: t('entity.customercomplainthandling.handlingdescription') },
  { key: 'causeAnalysis', label: t('entity.customercomplainthandling.causeanalysis') },
  { key: 'correctiveAction', label: t('entity.customercomplainthandling.correctiveaction') },
  { key: 'preventiveAction', label: t('entity.customercomplainthandling.preventiveaction') },
  { key: 'responsibleDept', label: t('entity.customercomplainthandling.responsibledept') },
  { key: 'responsibleBy', label: t('entity.customercomplainthandling.responsibleby') },
  { key: 'handlerBy', label: t('entity.customercomplainthandling.handlerby') },
  { key: 'handlingAtStart', label: t('entity.customercomplainthandling.handlingatstart') },
  { key: 'handlingAtEnd', label: t('entity.customercomplainthandling.handlingatend') },
  { key: 'plannedCompletionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplainthandling.plannedcompletiondate')) },
  { key: 'plannedCompletionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplainthandling.plannedcompletiondate')) },
  { key: 'actualCompletionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.customercomplainthandling.actualcompletiondate')) },
  { key: 'actualCompletionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.customercomplainthandling.actualcompletiondate')) },
  { key: 'handlingStatus', label: t('entity.customercomplainthandling.handlingstatus') },
  { key: 'handlingCost', label: t('entity.customercomplainthandling.handlingcost') },
  { key: 'customerFeedback', label: t('entity.customercomplainthandling.customerfeedback') },
  { key: 'customerSatisfaction', label: t('entity.customercomplainthandling.customersatisfaction') },
  { key: 'fileName', label: t('entity.customercomplainthandling.filename') },
  { key: 'accessUrl', label: t('entity.customercomplainthandling.accessurl') },
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
const entityIdName = 'customerComplaintHandlingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CustomerComplaintHandlingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerComplaintHandlingQuery>): CustomerComplaintHandlingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerComplaintHandlingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerComplaintHandlingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('complaintHandlingCode', form.complaintHandlingCode)
  assignTrimmed('complaintId', form.complaintId)
  assignTrimmed('complaintCode', form.complaintCode)
  assignTrimmed('complaintItemId', form.complaintItemId)
  if (form.handlingStage !== undefined && form.handlingStage !== null) {
    query.handlingStage = form.handlingStage
  }
  if (form.handlingMethod !== undefined && form.handlingMethod !== null) {
    query.handlingMethod = form.handlingMethod
  }
  assignTrimmed('handlingDescription', form.handlingDescription)
  assignTrimmed('causeAnalysis', form.causeAnalysis)
  assignTrimmed('correctiveAction', form.correctiveAction)
  assignTrimmed('preventiveAction', form.preventiveAction)
  assignTrimmed('responsibleDept', form.responsibleDept)
  assignTrimmed('responsibleBy', form.responsibleBy)
  assignTrimmed('handlerBy', form.handlerBy)
  assignTrimmed('handlingAtStart', form.handlingAtStart)
  assignTrimmed('handlingAtEnd', form.handlingAtEnd)
  assignTrimmed('plannedCompletionDateStart', form.plannedCompletionDateStart)
  assignTrimmed('plannedCompletionDateEnd', form.plannedCompletionDateEnd)
  assignTrimmed('actualCompletionDateStart', form.actualCompletionDateStart)
  assignTrimmed('actualCompletionDateEnd', form.actualCompletionDateEnd)
  if (form.handlingStatus !== undefined && form.handlingStatus !== null) {
    query.handlingStatus = form.handlingStatus
  }
  if (form.handlingCost !== undefined && form.handlingCost !== null) {
    query.handlingCost = form.handlingCost
  }
  assignTrimmed('customerFeedback', form.customerFeedback)
  if (form.customerSatisfaction !== undefined && form.customerSatisfaction !== null) {
    query.customerSatisfaction = form.customerSatisfaction
  }
  assignTrimmed('fileName', form.fileName)
  assignTrimmed('accessUrl', form.accessUrl)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerComplaintHandlingId',
    key: 'customerComplaintHandlingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerComplaintHandlingId') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.complainthandlingcode'),
    dataIndex: 'complaintHandlingCode',
    key: 'complaintHandlingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintHandlingCode') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.complaintid'),
    dataIndex: 'complaintId',
    key: 'complaintId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintId') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.complaintCode'),
    dataIndex: 'complaintCode',
    key: 'complaintCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintCode') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.complaintitemid'),
    dataIndex: 'complaintItemId',
    key: 'complaintItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaintItemId') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingstage'),
    dataIndex: 'handlingStage',
    key: 'handlingStage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingStage') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingmethod'),
    dataIndex: 'handlingMethod',
    key: 'handlingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingMethod') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingdescription'),
    dataIndex: 'handlingDescription',
    key: 'handlingDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingDescription') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.causeanalysis'),
    dataIndex: 'causeAnalysis',
    key: 'causeAnalysis',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'causeAnalysis') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.correctiveaction'),
    dataIndex: 'correctiveAction',
    key: 'correctiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'correctiveAction') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.preventiveaction'),
    dataIndex: 'preventiveAction',
    key: 'preventiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'preventiveAction') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.responsibledept'),
    dataIndex: 'responsibleDept',
    key: 'responsibleDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'responsibleDept') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.responsibleby'),
    dataIndex: 'responsibleBy',
    key: 'responsibleBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'responsibleBy') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlerby'),
    dataIndex: 'handlerBy',
    key: 'handlerBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlerBy') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingat'),
    dataIndex: 'handlingAt',
    key: 'handlingAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingAt') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.plannedcompletiondate'),
    dataIndex: 'plannedCompletionDate',
    key: 'plannedCompletionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'plannedCompletionDate') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.actualcompletiondate'),
    dataIndex: 'actualCompletionDate',
    key: 'actualCompletionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'actualCompletionDate') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingstatus'),
    dataIndex: 'handlingStatus',
    key: 'handlingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingStatus') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.handlingcost'),
    dataIndex: 'handlingCost',
    key: 'handlingCost',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'handlingCost') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.customerfeedback'),
    dataIndex: 'customerFeedback',
    key: 'customerFeedback',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerFeedback') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.customersatisfaction'),
    dataIndex: 'customerSatisfaction',
    key: 'customerSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'customerSatisfaction') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.filename'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'fileName') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.accessurl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'accessUrl') ?? ''
  },
  {
    title: t('entity.customercomplainthandling.complaint'),
    dataIndex: 'complaint',
    key: 'complaint',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintHandlingField(record, 'complaint') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customer:handling:update',
        onClick: (record: CustomerComplaintHandling) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:handling:delete',
        onClick: (record: CustomerComplaintHandling) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerComplaintHandlingId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerComplaintHandlingField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerComplaintHandling[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: CustomerComplaintHandling, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getCustomerComplaintHandlingId(selecmedRow.value) === getCustomerComplaintHandlingId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: CustomerComplaintHandling[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: CustomerComplaintHandling) => ({
  onClick: () => {
    const key = getCustomerComplaintHandlingId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getCustomerComplaintHandlingId(item)))
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
    const res = await getCustomerComplaintHandlingList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CustomerComplaintHandling] 加载数据失败', { error })
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
  complaintHandlingCode: '',
  complaintId: '',
  complaintCode: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  fileName: '',
  accessUrl: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.customercomplainthandling._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: CustomerComplaintHandling) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.customercomplainthandling._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.customercomplainthandling._self') }))
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
      await updateCustomerComplaintHandling(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.customercomplainthandling._self') }))
    } else {
      await createCustomerComplaintHandling(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.customercomplainthandling._self') }))
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
  const res = await getCustomerComplaintHandlingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importCustomerComplaintHandling(file, sheetName)
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
    const exportMeta = await exportCustomerComplaintHandling(
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
    message.success(t('common.feedback.export.success', { target: t('entity.customercomplainthandling._self') }))
  } catch (error: any) {
    logger.error('[CustomerComplaintHandling] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.customercomplainthandling._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerComplaintHandling) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.customercomplainthandling._self'), name: t('common.tip.this.target', { target: t('entity.customercomplainthandling._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintHandlingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplainthandling._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.customercomplainthandling._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.customercomplainthandling._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerComplaintHandlingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.customercomplainthandling._self') }))
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
  complaintHandlingCode: '',
  complaintId: '',
  complaintCode: '',
  complaintItemId: '',
  handlingStage: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  causeAnalysis: '',
  correctiveAction: '',
  preventiveAction: '',
  responsibleDept: '',
  responsibleBy: '',
  handlerBy: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  plannedCompletionDateStart: '',
  plannedCompletionDateEnd: '',
  actualCompletionDateStart: '',
  actualCompletionDateEnd: '',
  handlingStatus: undefined as number | undefined,
  handlingCost: undefined as number | undefined,
  customerFeedback: '',
  customerSatisfaction: undefined as number | undefined,
  fileName: '',
  accessUrl: '',
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

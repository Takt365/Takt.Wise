<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/complaint/customer-complaint -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：客诉主表实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selecmed-master-key="selecmedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getCustomerComplaintId"
      :master-row-selection="rowSelection"
      master-id-column-key="customerComplaintId"
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
      create-permission="logistics:quality:complaint:customer:create"
      update-permission="logistics:quality:complaint:customer:update"
      delete-permission="logistics:quality:complaint:customer:delete"
      import-permission="logistics:quality:complaint:customer:import"
      export-permission="logistics:quality:complaint:customer:export"
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
      </template>
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'complaintMethod'">
          <TaktDictTag
            :value="getCustomerComplaintDictValue(record, 'complaintMethod')"
            dict-type="logistics_quality_complaint_method"
          />
        </template>
        <template v-else-if="column.key === 'complaintType'">
          <TaktDictTag
            :value="getCustomerComplaintDictValue(record, 'complaintType')"
            dict-type="logistics_quality_complaint_type"
          />
        </template>
        <template v-else-if="column.key === 'complaintLevel'">
          <TaktDictTag
            :value="getCustomerComplaintDictValue(record, 'complaintLevel')"
            dict-type="logistics_quality_complaint_level"
          />
        </template>
        <template v-else-if="column.key === 'customerSatisfaction'">
          <TaktDictTag
            :value="getCustomerComplaintDictValue(record, 'customerSatisfaction')"
            dict-type="logistics_quality_customer_satisfaction"
          />
        </template>
        <template v-else-if="column.key === 'complaintStatus'">
          <TaktDictTag
            :value="getCustomerComplaintDictValue(record, 'complaintStatus')"
            dict-type="logistics_quality_complaint_status"
          />
        </template>
      </template>
      <template #detail>
        <CustomerComplaintItemPanel
          ref="customerComplaintItemPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>
    </TaktMasterDetailTableLr>

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
      <CustomerComplaintForm
        :key="formData?.customerComplaintId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-quality-complaint-customer-complaint'"
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
      <div v-show="isFieldVisible('customerComplaintCode')">
      <a-form-item :label="pi.queryLabel('customerComplaintCode')">
        <a-input
          v-model:value="advancedQueryForm.customerComplaintCode"
          :placeholder="pi.queryPh('customerComplaintCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerId')">
      <a-form-item :label="pi.queryLabel('customerId')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerId"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerName1')">
      <a-form-item :label="pi.queryLabel('customerName1')">
        <a-input
          v-model:value="advancedQueryForm.customerName1"
          :placeholder="pi.queryPh('customerName1', 'required')"
          show-count
          :maxlength="140"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerCode')">
      <a-form-item :label="pi.queryLabel('customerCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerCode"
          api-url="TaktCustomers/options"
          :placeholder="pi.queryPh('customerCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateStart')">
      <a-form-item :label="pi.queryLabel('complaintDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateStart"
          :placeholder="pi.queryPh('complaintDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDateEnd')">
      <a-form-item :label="pi.queryLabel('complaintDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.complaintDateEnd"
          :placeholder="pi.queryPh('complaintDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintMethod')">
      <a-form-item :label="pi.queryLabel('complaintMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.complaintMethod"
          dict-type="logistics_quality_complaint_method"
          :placeholder="pi.queryPh('complaintMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintType')">
      <a-form-item :label="pi.queryLabel('complaintType')">
        <TaktSelect
          v-model:value="advancedQueryForm.complaintType"
          dict-type="logistics_quality_complaint_type"
          :placeholder="pi.queryPh('complaintType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintLevel')">
      <a-form-item :label="pi.queryLabel('complaintLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.complaintLevel"
          dict-type="logistics_quality_complaint_level"
          :placeholder="pi.queryPh('complaintLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptId')">
      <a-form-item :label="pi.queryLabel('responsibleDeptId')">
        <TaktSelect
          v-model:value="advancedQueryForm.responsibleDeptId"
          api-url="TaktDepts/tree-options"
          :placeholder="pi.queryPh('responsibleDeptId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDeptName')">
      <a-form-item :label="pi.queryLabel('responsibleDeptName')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDeptName"
          :placeholder="pi.queryPh('responsibleDeptName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleGroupCode')">
      <a-form-item :label="pi.queryLabel('responsibleGroupCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.responsibleGroupCode"
          api-url="TaktComplaintGroups/options"
          :api-params="{ complaintGroupCategory: 1 }"
          :placeholder="pi.queryPh('responsibleGroupCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleGroupName')">
      <a-form-item :label="pi.queryLabel('responsibleGroupName')">
        <a-input
          v-model:value="advancedQueryForm.responsibleGroupName"
          :placeholder="pi.queryPh('responsibleGroupName', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateStart')">
      <a-form-item :label="pi.queryLabel('requiredReplyDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateStart"
          :placeholder="pi.queryPh('requiredReplyDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requiredReplyDateEnd')">
      <a-form-item :label="pi.queryLabel('requiredReplyDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.requiredReplyDateEnd"
          :placeholder="pi.queryPh('requiredReplyDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateStart')">
      <a-form-item :label="pi.queryLabel('actualReplyDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateStart"
          :placeholder="pi.queryPh('actualReplyDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualReplyDateEnd')">
      <a-form-item :label="pi.queryLabel('actualReplyDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.actualReplyDateEnd"
          :placeholder="pi.queryPh('actualReplyDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintDescription')">
      <a-form-item :label="pi.queryLabel('complaintDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.complaintDescription"
          :placeholder="pi.queryPh('complaintDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingResult')">
      <a-form-item :label="pi.queryLabel('handlingResult')">
        <a-textarea
          v-model:value="advancedQueryForm.handlingResult"
          :placeholder="pi.queryPh('handlingResult', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('customerSatisfaction')">
      <a-form-item :label="pi.queryLabel('customerSatisfaction')">
        <TaktSelect
          v-model:value="advancedQueryForm.customerSatisfaction"
          dict-type="logistics_quality_customer_satisfaction"
          :placeholder="pi.queryPh('customerSatisfaction', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="pi.queryLabel('attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="pi.queryPh('attachments', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('complaintStatus')">
      <a-form-item :label="pi.queryLabel('complaintStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.complaintStatus"
          dict-type="logistics_quality_complaint_status"
          :placeholder="pi.queryPh('complaintStatus', 'select')"
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
        :entity-i18n-key="CUSTOMERCOMPLAINT_SELF_I18N_KEY"
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
      :id-column-key="'customerComplaintId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 客诉主表实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/complaint/customer-complaint
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import CustomerComplaintForm from './components/CustomerComplaintForm.vue'
import CustomerComplaintItemPanel from './components/CustomerComplaintItemPanel.vue'
import { provideCustomerComplaintMasterContext, type CustomerComplaintRowRecord } from './composables/useCustomerComplaintMasterContext'
import { getCustomerComplaintList, getCustomerComplaintById, createCustomerComplaint, updateCustomerComplaint, deleteCustomerComplaintById, deleteCustomerComplaintBatch, getCustomerComplaintTemplate, importCustomerComplaint, exportCustomerComplaint, updateCustomerComplaintStatus } from '@/api/logistics/quality/complaint/customerComplaint.api'
import type { CustomerComplaint, CustomerComplaintQuery } from '@/types/logistics/quality/complaint/customerComplaint.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useCustomerComplaintI18n,
  CUSTOMERCOMPLAINT_LIST_FIELDS,
  CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS,
  CUSTOMERCOMPLAINT_QUERY_FIELDS,
  CUSTOMERCOMPLAINT_SELF_I18N_KEY,
} from './composables/useCustomerComplaintI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCustomerComplaintI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCustomerComplaint')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<CustomerComplaint[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<CustomerComplaintRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<CustomerComplaintRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<CustomerComplaint> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

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
  for (const key of CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.complaintMethod !== undefined && form.complaintMethod !== null) {
    return true
  }
  if (form.complaintType !== undefined && form.complaintType !== null) {
    return true
  }
  if (form.complaintLevel !== undefined && form.complaintLevel !== null) {
    return true
  }
  if (form.customerSatisfaction !== undefined && form.customerSatisfaction !== null) {
    return true
  }
  if (form.complaintStatus !== undefined && form.complaintStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    complaintMethod: undefined as number | undefined,
    complaintType: undefined as number | undefined,
    complaintLevel: undefined as number | undefined,
    customerSatisfaction: undefined as number | undefined,
    complaintStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  CUSTOMERCOMPLAINT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'customerComplaintId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provideCustomerComplaintMasterContext()
const customerComplaintItemPanelRef = ref<InstanceType<typeof CustomerComplaintItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CustomerComplaintQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CustomerComplaintQuery>): CustomerComplaintQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CustomerComplaintQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CustomerComplaintQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of CUSTOMERCOMPLAINT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.complaintMethod !== undefined && form.complaintMethod !== null) {
    query.complaintMethod = form.complaintMethod
  }
  if (form.complaintType !== undefined && form.complaintType !== null) {
    query.complaintType = form.complaintType
  }
  if (form.complaintLevel !== undefined && form.complaintLevel !== null) {
    query.complaintLevel = form.complaintLevel
  }
  if (form.customerSatisfaction !== undefined && form.customerSatisfaction !== null) {
    query.customerSatisfaction = form.customerSatisfaction
  }
  if (form.complaintStatus !== undefined && form.complaintStatus !== null) {
    query.complaintStatus = form.complaintStatus
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
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: CustomerComplaintRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getCustomerComplaintId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CustomerComplaintRowRecord
  const key = getCustomerComplaintId(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
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

/** 加载主表详情并回填当前页 dataSource */
async function loadCustomerComplaintDetail(record: CustomerComplaintRowRecord): Promise<CustomerComplaint | null> {
  const id = getCustomerComplaintId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCustomerComplaintById(id)
    const index = dataSource.value.findIndex((row) => getCustomerComplaintId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as CustomerComplaint
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'customerComplaintId',
    key: 'customerComplaintId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerComplaintId') ?? ''
  },
  {
    title: pi.label('customerComplaintCode'),
    dataIndex: 'customerComplaintCode',
    key: 'customerComplaintCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerComplaintCode') ?? ''
  },
  {
    title: pi.label('customerId'),
    dataIndex: 'customerId',
    key: 'customerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerId') ?? ''
  },
  {
    title: pi.label('customerName1'),
    dataIndex: 'customerName1',
    key: 'customerName1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerName1') ?? ''
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'customerCode') ?? ''
  },
  {
    title: pi.label('complaintDate'),
    dataIndex: 'complaintDate',
    key: 'complaintDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDate') ?? ''
  },
  {
    title: pi.label('complaintMethod'),
    dataIndex: 'complaintMethod',
    key: 'complaintMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('complaintType'),
    dataIndex: 'complaintType',
    key: 'complaintType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('complaintLevel'),
    dataIndex: 'complaintLevel',
    key: 'complaintLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('responsibleDeptId'),
    dataIndex: 'responsibleDeptId',
    key: 'responsibleDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptId') ?? ''
  },
  {
    title: pi.label('responsibleDeptName'),
    dataIndex: 'responsibleDeptName',
    key: 'responsibleDeptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleDeptName') ?? ''
  },
  {
    title: pi.label('responsibleGroupCode'),
    dataIndex: 'responsibleGroupCode',
    key: 'responsibleGroupCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleGroupCode') ?? ''
  },
  {
    title: pi.label('responsibleGroupName'),
    dataIndex: 'responsibleGroupName',
    key: 'responsibleGroupName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'responsibleGroupName') ?? ''
  },
  {
    title: pi.label('requiredReplyDate'),
    dataIndex: 'requiredReplyDate',
    key: 'requiredReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'requiredReplyDate') ?? ''
  },
  {
    title: pi.label('actualReplyDate'),
    dataIndex: 'actualReplyDate',
    key: 'actualReplyDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'actualReplyDate') ?? ''
  },
  {
    title: pi.label('complaintDescription'),
    dataIndex: 'complaintDescription',
    key: 'complaintDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'complaintDescription') ?? ''
  },
  {
    title: pi.label('handlingResult'),
    dataIndex: 'handlingResult',
    key: 'handlingResult',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'handlingResult') ?? ''
  },
  {
    title: pi.label('customerSatisfaction'),
    dataIndex: 'customerSatisfaction',
    key: 'customerSatisfaction',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCustomerComplaintField(record, 'attachments') ?? ''
  },
  {
    title: pi.label('complaintStatus'),
    dataIndex: 'complaintStatus',
    key: 'complaintStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:complaint:customer:update',
        onClick: (record: CustomerComplaintRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:complaint:customer:delete',
        onClick: (record: CustomerComplaintRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCustomerComplaintId = (record: CustomerComplaintRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerComplaintField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getCustomerComplaintDictValue = (
  record: CustomerComplaintRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: CustomerComplaintRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CustomerComplaintRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getCustomerComplaintId(selecmedRow.value) === getCustomerComplaintId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: CustomerComplaintRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    syncMasterSelection(selecmedRow.value)
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
    const res = await getCustomerComplaintList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[CustomerComplaint] 加载数据失败', { error })
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
  customerComplaintCode: '',
  customerId: '',
  customerName1: '',
  customerCode: '',
  complaintDateStart: '',
  complaintDateEnd: '',
  complaintMethod: undefined as number | undefined,
  complaintType: undefined as number | undefined,
  complaintLevel: undefined as number | undefined,
  responsibleDeptId: '',
  responsibleDeptName: '',
  responsibleGroupCode: '',
  responsibleGroupName: '',
  requiredReplyDateStart: '',
  requiredReplyDateEnd: '',
  actualReplyDateStart: '',
  actualReplyDateEnd: '',
  complaintDescription: '',
  handlingResult: '',
  customerSatisfaction: undefined as number | undefined,
  attachments: '',
  complaintStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: CustomerComplaintRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadCustomerComplaintDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      await updateCustomerComplaint(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCustomerComplaint(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
  customerComplaintItemPanelRef.value?.reload?.()
    }
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
  const res = await getCustomerComplaintTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCustomerComplaint(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selecmedMasterKey.value) {
    customerComplaintItemPanelRef.value?.reload?.()
      }
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportCustomerComplaint(
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
    logger.error('[CustomerComplaint] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CustomerComplaintRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCustomerComplaintById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteCustomerComplaintBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
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
  cultureCode: '',
  plantCode: '',
  customerComplaintCode: '',
  customerId: '',
  customerName1: '',
  customerCode: '',
  complaintDateStart: '',
  complaintDateEnd: '',
  complaintMethod: undefined as number | undefined,
  complaintType: undefined as number | undefined,
  complaintLevel: undefined as number | undefined,
  responsibleDeptId: '',
  responsibleDeptName: '',
  responsibleGroupCode: '',
  responsibleGroupName: '',
  requiredReplyDateStart: '',
  requiredReplyDateEnd: '',
  actualReplyDateStart: '',
  actualReplyDateEnd: '',
  complaintDescription: '',
  handlingResult: '',
  customerSatisfaction: undefined as number | undefined,
  attachments: '',
  complaintStatus: undefined as number | undefined,
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

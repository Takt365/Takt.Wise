<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/quotation -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售报价实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getSalesQuotationId"
      :master-row-selection="rowSelection"
      master-id-column-key="salesQuotationId"
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
      create-permission="logistics:sales:quotation:create"
      update-permission="logistics:sales:quotation:update"
      delete-permission="logistics:sales:quotation:delete"
      import-permission="logistics:sales:quotation:import"
      export-permission="logistics:sales:quotation:export"
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
        <template v-if="column.key === 'salesQuotationType'">
          <TaktDictTag
            :value="getSalesQuotationDictValue(record, 'salesQuotationType')"
            dict-type="logistics_sales_order_type"
          />
        </template>
        <template v-else-if="column.key === 'pricingProcedure'">
          <TaktDictTag
            :value="getSalesQuotationDictValue(record, 'pricingProcedure')"
            dict-type="logistics_sales_pricing_procedure"
          />
        </template>
        <template v-else-if="column.key === 'currencyCode'">
          <TaktDictTag
            :value="getSalesQuotationDictValue(record, 'currencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'taxCode'">
          {{ record.taxCode }}
        </template>
        <template v-else-if="column.key === 'taxRate'">
          {{ record.taxRate }}
        </template>
        <template v-else-if="column.key === 'quotationStatus'">
          <TaktDictTag
            :value="getSalesQuotationDictValue(record, 'quotationStatus')"
            dict-type="logistics_sales_quotation_status"
          />
        </template>
      </template>
      <template #detail>
        <SalesQuotationItemPanel
          ref="salesQuotationItemPanelRef"
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
      <SalesQuotationForm
        :key="formData?.salesQuotationId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-sales-quotation'"
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
      <div v-show="isFieldVisible('salesQuotationCode')">
      <a-form-item :label="pi.queryLabel('salesQuotationCode')">
        <a-input
          v-model:value="advancedQueryForm.salesQuotationCode"
          :placeholder="pi.queryPh('salesQuotationCode', 'required')"
          show-count
          :maxlength="20"
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
      <div v-show="isFieldVisible('quotationDateStart')">
      <a-form-item :label="pi.queryLabel('quotationDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.quotationDateStart"
          :placeholder="pi.queryPh('quotationDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationDateEnd')">
      <a-form-item :label="pi.queryLabel('quotationDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.quotationDateEnd"
          :placeholder="pi.queryPh('quotationDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validUntilDateStart')">
      <a-form-item :label="pi.queryLabel('validUntilDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validUntilDateStart"
          :placeholder="pi.queryPh('validUntilDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validUntilDateEnd')">
      <a-form-item :label="pi.queryLabel('validUntilDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validUntilDateEnd"
          :placeholder="pi.queryPh('validUntilDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesGroup')">
      <a-form-item :label="pi.queryLabel('salesGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesGroup"
          api-url="TaktSalesGroups/options"
          :placeholder="pi.queryPh('salesGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesQuotationType')">
      <a-form-item :label="pi.queryLabel('salesQuotationType')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesQuotationType"
          dict-type="logistics_sales_order_type"
          :placeholder="pi.queryPh('salesQuotationType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingProcedure')">
      <a-form-item :label="pi.queryLabel('pricingProcedure')">
        <TaktSelect
          v-model:value="advancedQueryForm.pricingProcedure"
          dict-type="logistics_sales_pricing_procedure"
          :placeholder="pi.queryPh('pricingProcedure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingConditionCode')">
      <a-form-item :label="pi.queryLabel('pricingConditionCode')">
        <a-input
          v-model:value="advancedQueryForm.pricingConditionCode"
          :placeholder="pi.queryPh('pricingConditionCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalQuantity')">
      <a-form-item :label="pi.queryLabel('totalQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.totalQuantity"
          :placeholder="pi.queryPh('totalQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalAmount')">
      <a-form-item :label="pi.queryLabel('totalAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.totalAmount"
          :placeholder="pi.queryPh('totalAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountAmount')">
      <a-form-item :label="pi.queryLabel('discountAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.discountAmount"
          :placeholder="pi.queryPh('discountAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxCode')">
      <a-form-item :label="pi.queryLabel('taxCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxCode"
          api-url="TaktTaxCodes/options"
          :placeholder="pi.queryPh('taxCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxRate')">
      <a-form-item :label="pi.queryLabel('taxRate')">
        <a-input-number
          v-model:value="advancedQueryForm.taxRate"
          :placeholder="pi.queryPh('taxRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxAmount')">
      <a-form-item :label="pi.queryLabel('taxAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.taxAmount"
          :placeholder="pi.queryPh('taxAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualAmount')">
      <a-form-item :label="pi.queryLabel('actualAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.actualAmount"
          :placeholder="pi.queryPh('actualAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesOrderCode')">
      <a-form-item :label="pi.queryLabel('salesOrderCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.salesOrderCode"
          api-url="TaktSalesOrders/options"
          :placeholder="pi.queryPh('salesOrderCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quotationStatus')">
      <a-form-item :label="pi.queryLabel('quotationStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.quotationStatus"
          dict-type="logistics_sales_quotation_status"
          :placeholder="pi.queryPh('quotationStatus', 'select')"
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
        :entity-i18n-key="SALESQUOTATION_SELF_I18N_KEY"
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
      :id-column-key="'salesQuotationId'"
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
 * Takt销售报价实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/quotation
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import SalesQuotationForm from './components/QuotationForm.vue'
import SalesQuotationItemPanel from './components/QuotationItemPanel.vue'
import { provideSalesQuotationMasterContext, type SalesQuotationRowRecord } from './composables/useQuotationMasterContext'
import { getSalesQuotationList, getSalesQuotationById, createSalesQuotation, updateSalesQuotation, deleteSalesQuotationById, deleteSalesQuotationBatch, getSalesQuotationTemplate, importSalesQuotation, exportSalesQuotation, updateSalesQuotationStatus } from '@/api/logistics/sales/quotation.api'
import type { SalesQuotation, SalesQuotationQuery } from '@/types/logistics/sales/quotation.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useSalesQuotationI18n,
  SALESQUOTATION_LIST_FIELDS,
  SALESQUOTATION_QUERY_STRING_FIELDS,
  SALESQUOTATION_QUERY_FIELDS,
  SALESQUOTATION_SELF_I18N_KEY,
} from './composables/useQuotationI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesQuotationI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesQuotation')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesQuotation[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesQuotationRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesQuotationRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesQuotation> | null>(null)
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
  for (const key of SALESQUOTATION_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    return true
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    return true
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    return true
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    return true
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    return true
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    return true
  }
  if (form.quotationStatus !== undefined && form.quotationStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESQUOTATION_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESQUOTATION_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    totalQuantity: undefined as number | undefined,
    totalAmount: undefined as number | undefined,
    discountAmount: undefined as number | undefined,
    taxRate: undefined as number | undefined,
    taxAmount: undefined as number | undefined,
    actualAmount: undefined as number | undefined,
    quotationStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  SALESQUOTATION_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'salesQuotationId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSalesQuotationMasterContext()
const salesQuotationItemPanelRef = ref<InstanceType<typeof SalesQuotationItemPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SalesQuotationQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesQuotationQuery>): SalesQuotationQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesQuotationQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesQuotationQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESQUOTATION_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.totalQuantity !== undefined && form.totalQuantity !== null) {
    query.totalQuantity = form.totalQuantity
  }
  if (form.totalAmount !== undefined && form.totalAmount !== null) {
    query.totalAmount = form.totalAmount
  }
  if (form.discountAmount !== undefined && form.discountAmount !== null) {
    query.discountAmount = form.discountAmount
  }
  if (form.taxRate !== undefined && form.taxRate !== null) {
    query.taxRate = form.taxRate
  }
  if (form.taxAmount !== undefined && form.taxAmount !== null) {
    query.taxAmount = form.taxAmount
  }
  if (form.actualAmount !== undefined && form.actualAmount !== null) {
    query.actualAmount = form.actualAmount
  }
  if (form.quotationStatus !== undefined && form.quotationStatus !== null) {
    query.quotationStatus = form.quotationStatus
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
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: SalesQuotationRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSalesQuotationId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SalesQuotationRowRecord
  const key = getSalesQuotationId(row)
  selectedRowKeys.value = [key]
  selectedRows.value = [row]
  selectedRow.value = row
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
async function loadSalesQuotationDetail(record: SalesQuotationRowRecord): Promise<SalesQuotation | null> {
  const id = getSalesQuotationId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesQuotationById(id)
    const index = dataSource.value.findIndex((row) => getSalesQuotationId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesQuotation
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
    dataIndex: 'salesQuotationId',
    key: 'salesQuotationId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesQuotationId') ?? ''
  },
  {
    title: pi.label('salesQuotationCode'),
    dataIndex: 'salesQuotationCode',
    key: 'salesQuotationCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesQuotationCode') ?? ''
  },
  {
    title: pi.label('customerCode'),
    dataIndex: 'customerCode',
    key: 'customerCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'customerCode') ?? ''
  },
  {
    title: pi.label('customerName1'),
    dataIndex: 'customerName1',
    key: 'customerName1',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'customerName1') ?? ''
  },
  {
    title: pi.label('quotationDate'),
    dataIndex: 'quotationDate',
    key: 'quotationDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'quotationDate') ?? ''
  },
  {
    title: pi.label('validUntilDate'),
    dataIndex: 'validUntilDate',
    key: 'validUntilDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'validUntilDate') ?? ''
  },
  {
    title: pi.label('salesGroup'),
    dataIndex: 'salesGroup',
    key: 'salesGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesGroup') ?? ''
  },
  {
    title: pi.label('salesQuotationType'),
    dataIndex: 'salesQuotationType',
    key: 'salesQuotationType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('pricingProcedure'),
    dataIndex: 'pricingProcedure',
    key: 'pricingProcedure',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('pricingConditionCode'),
    dataIndex: 'pricingConditionCode',
    key: 'pricingConditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'pricingConditionCode') ?? ''
  },
  {
    title: pi.label('totalQuantity'),
    dataIndex: 'totalQuantity',
    key: 'totalQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'totalQuantity') ?? ''
  },
  {
    title: pi.label('totalAmount'),
    dataIndex: 'totalAmount',
    key: 'totalAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'totalAmount') ?? ''
  },
  {
    title: pi.label('discountAmount'),
    dataIndex: 'discountAmount',
    key: 'discountAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'discountAmount') ?? ''
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('taxCode'),
    dataIndex: 'taxCode',
    key: 'taxCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('taxRate'),
    dataIndex: 'taxRate',
    key: 'taxRate',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'taxAmount') ?? ''
  },
  {
    title: pi.label('actualAmount'),
    dataIndex: 'actualAmount',
    key: 'actualAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'actualAmount') ?? ''
  },
  {
    title: pi.label('salesOrderCode'),
    dataIndex: 'salesOrderCode',
    key: 'salesOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesQuotationField(record, 'salesOrderCode') ?? ''
  },
  {
    title: pi.label('quotationStatus'),
    dataIndex: 'quotationStatus',
    key: 'quotationStatus',
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
        permission: 'logistics:sales:quotation:update',
        onClick: (record: SalesQuotationRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:quotation:delete',
        onClick: (record: SalesQuotationRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesQuotationId = (record: SalesQuotationRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesQuotationField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesQuotationDictValue = (
  record: SalesQuotationRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalesQuotationRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SalesQuotationRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSalesQuotationId(selectedRow.value) === getSalesQuotationId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesQuotationRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
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
    const res = await getSalesQuotationList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesQuotation] 加载数据失败', { error })
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
  salesQuotationCode: '',
  customerCode: '',
  customerName1: '',
  quotationDateStart: '',
  quotationDateEnd: '',
  validUntilDateStart: '',
  validUntilDateEnd: '',
  salesGroup: '',
  salesQuotationType: '',
  pricingProcedure: '',
  pricingConditionCode: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  currencyCode: '',
  taxCode: '',
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  salesOrderCode: '',
  quotationStatus: undefined as number | undefined,
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
async function handleEdit(record: SalesQuotationRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadSalesQuotationDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
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
      await updateSalesQuotation(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesQuotation(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  salesQuotationItemPanelRef.value?.reload?.()
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
  const res = await getSalesQuotationTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesQuotation(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    salesQuotationItemPanelRef.value?.reload?.()
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
    const exportMeta = await exportSalesQuotation(
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
    logger.error('[SalesQuotation] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesQuotationRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesQuotationById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalesQuotationBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selectedRowKeys.value = []
      selectedRows.value = []
      selectedRow.value = null
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
  salesQuotationCode: '',
  customerCode: '',
  customerName1: '',
  quotationDateStart: '',
  quotationDateEnd: '',
  validUntilDateStart: '',
  validUntilDateEnd: '',
  salesGroup: '',
  salesQuotationType: '',
  pricingProcedure: '',
  pricingConditionCode: '',
  totalQuantity: undefined as number | undefined,
  totalAmount: undefined as number | undefined,
  discountAmount: undefined as number | undefined,
  currencyCode: '',
  taxCode: '',
  taxRate: undefined as number | undefined,
  taxAmount: undefined as number | undefined,
  actualAmount: undefined as number | undefined,
  salesOrderCode: '',
  quotationStatus: undefined as number | undefined,
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

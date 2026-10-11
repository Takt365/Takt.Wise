<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/countersign -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会签单实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getCountersignId"
      :master-row-selection="rowSelection"
      master-id-column-key="countersignId"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="approval"
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
      create-permission="accounting:financial:countersign:create"
      update-permission="accounting:financial:countersign:update"
      delete-permission="accounting:financial:countersign:delete"
      import-permission="accounting:financial:countersign:import"
      export-permission="accounting:financial:countersign:export"
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
        <template v-if="column.key === 'businessType'">
          <TaktDictTag
            :value="getCountersignDictValue(record, 'businessType')"
            dict-type="accounting_financial_countersign_business_type"
          />
        </template>
        <template v-else-if="column.key === 'isBudget'">
          <TaktDictTag
            :value="getCountersignDictValue(record, 'isBudget')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'countersignStatus'">
          <TaktDictTag
            :value="getCountersignDictValue(record, 'countersignStatus')"
            dict-type="sys_approval_status"
          />
        </template>
      </template>
      <template #detail>
        <CountersignDetailPanel
          ref="countersignDetailPanelRef"
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
      <CountersignForm
        :key="formData?.countersignId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-countersign'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
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
      <div v-show="isFieldVisible('countersignCode')">
      <a-form-item :label="pi.queryLabel('countersignCode')">
        <a-input
          v-model:value="advancedQueryForm.countersignCode"
          :placeholder="pi.queryPh('countersignCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInquiryId')">
      <a-form-item :label="pi.queryLabel('purchaseInquiryId')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchaseInquiryId"
          api-url="TaktPurchaseInquiries/options"
          :placeholder="pi.queryPh('purchaseInquiryId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchaseInquiryCode')">
      <a-form-item :label="pi.queryLabel('purchaseInquiryCode')">
        <a-input
          v-model:value="advancedQueryForm.purchaseInquiryCode"
          :placeholder="pi.queryPh('purchaseInquiryCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessType')">
      <a-form-item :label="pi.queryLabel('businessType')">
        <TaktSelect
          v-model:value="advancedQueryForm.businessType"
          dict-type="accounting_financial_countersign_business_type"
          :placeholder="pi.queryPh('businessType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('businessKey')">
      <a-form-item :label="pi.queryLabel('businessKey')">
        <a-input
          v-model:value="advancedQueryForm.businessKey"
          :placeholder="pi.queryPh('businessKey', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stepNo')">
      <a-form-item :label="pi.queryLabel('stepNo')">
        <a-input-number
          v-model:value="advancedQueryForm.stepNo"
          :placeholder="pi.queryPh('stepNo', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignDepts')">
      <a-form-item :label="pi.queryLabel('countersignDepts')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.countersignDepts"
          api-url="TaktDepts/tree-options"
          :lazy="true"
          multiple
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="pi.queryPh('countersignDepts', 'select')"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('financeDept')">
      <a-form-item :label="pi.queryLabel('financeDept')">
        <a-input
          v-model:value="advancedQueryForm.financeDept"
          :placeholder="pi.queryPh('financeDept', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetReviewComment')">
      <a-form-item :label="pi.queryLabel('budgetReviewComment')">
        <a-input
          v-model:value="advancedQueryForm.budgetReviewComment"
          :placeholder="pi.queryPh('budgetReviewComment', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('executiveOffice')">
      <a-form-item :label="pi.queryLabel('executiveOffice')">
        <a-input
          v-model:value="advancedQueryForm.executiveOffice"
          :placeholder="pi.queryPh('executiveOffice', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantBy')">
      <a-form-item :label="pi.queryLabel('applicantBy')">
        <TaktSelect
          v-model:value="advancedQueryForm.applicantBy"
          api-url="TaktEmployees/options"
          :placeholder="pi.queryPh('applicantBy', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicantName')">
      <a-form-item :label="pi.queryLabel('applicantName')">
        <a-input
          v-model:value="advancedQueryForm.applicantName"
          :placeholder="pi.queryPh('applicantName', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationDeptId')">
      <a-form-item :label="pi.queryLabel('applicationDeptId')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.applicationDeptId"
          api-url="TaktDepts/tree-options"
          :lazy="true"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="pi.queryPh('applicationDeptId', 'select')"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationDeptName')">
      <a-form-item :label="pi.queryLabel('applicationDeptName')">
        <a-input
          v-model:value="advancedQueryForm.applicationDeptName"
          :placeholder="pi.queryPh('applicationDeptName', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costBearerDeptId')">
      <a-form-item :label="pi.queryLabel('costBearerDeptId')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.costBearerDeptId"
          api-url="TaktDepts/tree-options"
          :lazy="true"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
          :placeholder="pi.queryPh('costBearerDeptId', 'select')"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('costBearerDeptName')">
      <a-form-item :label="pi.queryLabel('costBearerDeptName')">
        <a-input
          v-model:value="advancedQueryForm.costBearerDeptName"
          :placeholder="pi.queryPh('costBearerDeptName', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBudget')">
      <a-form-item :label="pi.queryLabel('isBudget')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBudget"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBudget', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetItemId')">
      <a-form-item :label="pi.queryLabel('budgetItemId')">
        <TaktSelect
          v-model:value="advancedQueryForm.budgetItemId"
          api-url="TaktBudgetActuals/options"
          :placeholder="pi.queryPh('budgetItemId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetItem')">
      <a-form-item :label="pi.queryLabel('budgetItem')">
        <a-input
          v-model:value="advancedQueryForm.budgetItem"
          :placeholder="pi.queryPh('budgetItem', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetAmount')">
      <a-form-item :label="pi.queryLabel('budgetAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.budgetAmount"
          :placeholder="pi.queryPh('budgetAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationAmount')">
      <a-form-item :label="pi.queryLabel('applicationAmount')">
        <a-input-number
          v-model:value="advancedQueryForm.applicationAmount"
          :placeholder="pi.queryPh('applicationAmount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignTitle')">
      <a-form-item :label="pi.queryLabel('countersignTitle')">
        <a-input
          v-model:value="advancedQueryForm.countersignTitle"
          :placeholder="pi.queryPh('countersignTitle', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicationReason')">
      <a-form-item :label="pi.queryLabel('applicationReason')">
        <a-input
          v-model:value="advancedQueryForm.applicationReason"
          :placeholder="pi.queryPh('applicationReason', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetUsageDescription')">
      <a-form-item :label="pi.queryLabel('budgetUsageDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.budgetUsageDescription"
          :placeholder="pi.queryPh('budgetUsageDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetAndExpectedBenefit')">
      <a-form-item :label="pi.queryLabel('targetAndExpectedBenefit')">
        <a-input
          v-model:value="advancedQueryForm.targetAndExpectedBenefit"
          :placeholder="pi.queryPh('targetAndExpectedBenefit', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fileName')">
      <a-form-item :label="pi.queryLabel('fileName')">
        <a-input
          v-model:value="advancedQueryForm.fileName"
          :placeholder="pi.queryPh('fileName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accessUrl')">
      <a-form-item :label="pi.queryLabel('accessUrl')">
        <a-input
          v-model:value="advancedQueryForm.accessUrl"
          :placeholder="pi.queryPh('accessUrl', 'required')"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countersignStatus')">
      <a-form-item :label="pi.queryLabel('countersignStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.countersignStatus"
          dict-type="sys_approval_status"
          :placeholder="pi.queryPh('countersignStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="pi.queryLabel('approvalStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="pi.queryPh('approvalStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="pi.queryLabel('initiatorId')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="pi.queryPh('initiatorId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="pi.queryLabel('initiatedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="pi.queryPh('initiatedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="pi.queryLabel('initiatedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="pi.queryPh('initiatedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="pi.queryLabel('approvedBy')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="pi.queryPh('approvedBy', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="pi.queryLabel('approvedAtStart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="pi.queryPh('approvedAtStart', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="pi.queryLabel('approvedAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="pi.queryPh('approvedAtEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="pi.queryLabel('flowInstanceId')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="pi.queryPh('flowInstanceId', 'required')"
          show-count
          :maxlength="20"
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
        :entity-i18n-key="COUNTERSIGN_SELF_I18N_KEY"
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
      :id-column-key="'countersignId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="masterDetailMaster"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会签单实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/countersign
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import CountersignForm from './components/CountersignForm.vue'
import CountersignDetailPanel from './components/CountersignDetailPanel.vue'
import { provideCountersignMasterContext, type CountersignRowRecord } from './composables/useCountersignMasterContext'
import { getCountersignList, getCountersignById, createCountersign, updateCountersign, deleteCountersignById, deleteCountersignBatch, getCountersignTemplate, importCountersign, exportCountersign, updateCountersignStatus } from '@/api/accounting/financial/countersign.api'
import type { Countersign, CountersignQuery } from '@/types/accounting/financial/countersign.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useCountersignI18n,
  COUNTERSIGN_LIST_FIELDS,
  COUNTERSIGN_QUERY_STRING_FIELDS,
  COUNTERSIGN_QUERY_FIELDS,
  COUNTERSIGN_SELF_I18N_KEY,
} from './composables/useCountersignI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useCountersignI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktCountersign')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Countersign[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<CountersignRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<CountersignRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Countersign> | null>(null)
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
  for (const key of COUNTERSIGN_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  const countersignDepts = (form as { countersignDepts?: unknown }).countersignDepts
  if (Array.isArray(countersignDepts) && countersignDepts.length > 0) {
    return true
  }
  if (typeof countersignDepts === 'string' && countersignDepts.trim().length > 0) {
    return true
  }
  if (form.stepNo !== undefined && form.stepNo !== null) {
    return true
  }
  if (form.isBudget !== undefined && form.isBudget !== null) {
    return true
  }
  if (form.budgetAmount !== undefined && form.budgetAmount !== null) {
    return true
  }
  if (form.applicationAmount !== undefined && form.applicationAmount !== null) {
    return true
  }
  if (form.countersignStatus !== undefined && form.countersignStatus !== null) {
    return true
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(COUNTERSIGN_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof COUNTERSIGN_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    countersignDepts: [] as string[],
    stepNo: undefined as number | undefined,
    isBudget: undefined as number | undefined,
    budgetAmount: undefined as number | undefined,
    applicationAmount: undefined as number | undefined,
    countersignStatus: undefined as number | undefined,
    approvalStatus: undefined as number | undefined,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  COUNTERSIGN_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'countersignId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideCountersignMasterContext()
const countersignDetailPanelRef = ref<InstanceType<typeof CountersignDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {CountersignQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<CountersignQuery>): CountersignQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: CountersignQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof CountersignQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of COUNTERSIGN_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  const countersignDepts = (form as { countersignDepts?: unknown }).countersignDepts
  if (Array.isArray(countersignDepts) && countersignDepts.length > 0) {
    query.countersignDepts = JSON.stringify(countersignDepts.map((item) => String(item)))
  } else if (typeof countersignDepts === 'string' && countersignDepts.trim().length > 0) {
    query.countersignDepts = countersignDepts.trim()
  }
  if (form.stepNo !== undefined && form.stepNo !== null) {
    query.stepNo = form.stepNo
  }
  if (form.isBudget !== undefined && form.isBudget !== null) {
    query.isBudget = form.isBudget
  }
  if (form.budgetAmount !== undefined && form.budgetAmount !== null) {
    query.budgetAmount = form.budgetAmount
  }
  if (form.applicationAmount !== undefined && form.applicationAmount !== null) {
    query.applicationAmount = form.applicationAmount
  }
  if (form.countersignStatus !== undefined && form.countersignStatus !== null) {
    query.countersignStatus = form.countersignStatus
  }
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
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
function syncMasterSelection(record: CountersignRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getCountersignId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as CountersignRowRecord
  const key = getCountersignId(row)
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
async function loadCountersignDetail(record: CountersignRowRecord): Promise<Countersign | null> {
  const id = getCountersignId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getCountersignById(id)
    const index = dataSource.value.findIndex((row) => getCountersignId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Countersign
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
    dataIndex: 'countersignId',
    key: 'countersignId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignId') ?? ''
  },
  {
    title: pi.label('countersignCode'),
    dataIndex: 'countersignCode',
    key: 'countersignCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignCode') ?? ''
  },
  {
    title: pi.label('purchaseInquiryId'),
    dataIndex: 'purchaseInquiryId',
    key: 'purchaseInquiryId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'purchaseInquiryId') ?? ''
  },
  {
    title: pi.label('purchaseInquiryCode'),
    dataIndex: 'purchaseInquiryCode',
    key: 'purchaseInquiryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'purchaseInquiryCode') ?? ''
  },
  {
    title: pi.label('businessType'),
    dataIndex: 'businessType',
    key: 'businessType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('businessKey'),
    dataIndex: 'businessKey',
    key: 'businessKey',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'businessKey') ?? ''
  },
  {
    title: pi.label('stepNo'),
    dataIndex: 'stepNo',
    key: 'stepNo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'stepNo') ?? ''
  },
  {
    title: pi.label('countersignDepts'),
    dataIndex: 'countersignDepts',
    key: 'countersignDepts',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignDepts') ?? ''
  },
  {
    title: pi.label('financeDept'),
    dataIndex: 'financeDept',
    key: 'financeDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'financeDept') ?? ''
  },
  {
    title: pi.label('budgetReviewComment'),
    dataIndex: 'budgetReviewComment',
    key: 'budgetReviewComment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetReviewComment') ?? ''
  },
  {
    title: pi.label('executiveOffice'),
    dataIndex: 'executiveOffice',
    key: 'executiveOffice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'executiveOffice') ?? ''
  },
  {
    title: pi.label('applicantBy'),
    dataIndex: 'applicantBy',
    key: 'applicantBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicantBy') ?? ''
  },
  {
    title: pi.label('applicantName'),
    dataIndex: 'applicantName',
    key: 'applicantName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicantName') ?? ''
  },
  {
    title: pi.label('applicationDeptId'),
    dataIndex: 'applicationDeptId',
    key: 'applicationDeptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationDeptId') ?? ''
  },
  {
    title: pi.label('applicationDeptName'),
    dataIndex: 'applicationDeptName',
    key: 'applicationDeptName',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationDeptName') ?? ''
  },
  {
    title: pi.label('costBearerDeptId'),
    dataIndex: 'costBearerDeptId',
    key: 'costBearerDeptId',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'costBearerDeptId') ?? ''
  },
  {
    title: pi.label('costBearerDeptName'),
    dataIndex: 'costBearerDeptName',
    key: 'costBearerDeptName',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'costBearerDeptName') ?? ''
  },
  {
    title: pi.label('isBudget'),
    dataIndex: 'isBudget',
    key: 'isBudget',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('budgetItemId'),
    dataIndex: 'budgetItemId',
    key: 'budgetItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetItemId') ?? ''
  },
  {
    title: pi.label('budgetItem'),
    dataIndex: 'budgetItem',
    key: 'budgetItem',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetItem') ?? ''
  },
  {
    title: pi.label('budgetAmount'),
    dataIndex: 'budgetAmount',
    key: 'budgetAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetAmount') ?? ''
  },
  {
    title: pi.label('applicationAmount'),
    dataIndex: 'applicationAmount',
    key: 'applicationAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationAmount') ?? ''
  },
  {
    title: pi.label('countersignTitle'),
    dataIndex: 'countersignTitle',
    key: 'countersignTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'countersignTitle') ?? ''
  },
  {
    title: pi.label('applicationReason'),
    dataIndex: 'applicationReason',
    key: 'applicationReason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'applicationReason') ?? ''
  },
  {
    title: pi.label('budgetUsageDescription'),
    dataIndex: 'budgetUsageDescription',
    key: 'budgetUsageDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'budgetUsageDescription') ?? ''
  },
  {
    title: pi.label('targetAndExpectedBenefit'),
    dataIndex: 'targetAndExpectedBenefit',
    key: 'targetAndExpectedBenefit',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'targetAndExpectedBenefit') ?? ''
  },
  {
    title: pi.label('fileName'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'fileName') ?? ''
  },
  {
    title: pi.label('accessUrl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 160,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getCountersignField(record, 'accessUrl') ?? ''
  },
  {
    title: pi.label('countersignStatus'),
    dataIndex: 'countersignStatus',
    key: 'countersignStatus',
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
        permission: 'accounting:financial:countersign:update',
        onClick: (record: CountersignRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:countersign:delete',
        onClick: (record: CountersignRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getCountersignId = (record: CountersignRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getCountersignField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getCountersignDictValue = (
  record: CountersignRowRecord,
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
  onChange: (keys: (string | number)[], rows: CountersignRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: CountersignRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getCountersignId(selectedRow.value) === getCountersignId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: CountersignRowRecord[]) => {
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
    const res = await getCountersignList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Countersign] 加载数据失败', { error })
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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
async function handleEdit(record: CountersignRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadCountersignDetail(record)
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
      await updateCountersign(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createCountersign(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  countersignDetailPanelRef.value?.reload?.()
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
  const res = await getCountersignTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importCountersign(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    countersignDetailPanelRef.value?.reload?.()
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
    const exportMeta = await exportCountersign(
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
    logger.error('[Countersign] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: CountersignRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteCountersignById((record as any)[entityIdName])
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
      await deleteCountersignBatch(ids)
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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

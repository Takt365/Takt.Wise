<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/document-center/document -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getDocumentId"
      :master-row-selection="rowSelection"
      master-id-column-key="documentId"
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
      create-permission="routine:document:center:create"
      update-permission="routine:document:center:update"
      delete-permission="routine:document:center:delete"
      import-permission="routine:document:center:import"
      export-permission="routine:document:center:export"
      :left-actions="toolbarLeftActions"
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
        <template v-if="column.key === 'documentCategory'">
          <TaktDictTag
            :value="getDocumentDictValue(record, 'documentCategory')"
            dict-type="routine_document_center_category"
          />
        </template>
        <template v-else-if="column.key === 'confidentialLevel'">
          <TaktDictTag
            :value="getDocumentDictValue(record, 'confidentialLevel')"
            dict-type="routine_document_center_confidential_level"
          />
        </template>
        <template v-else-if="column.key === 'documentIsTop'">
          <TaktDictTag
            :value="getDocumentDictValue(record, 'documentIsTop')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'targetScope'">
          <TaktDictTag
            :value="getDocumentDictValue(record, 'targetScope')"
            dict-type="sys_publish_scope"
          />
        </template>
        <template v-else-if="column.key === 'documentStatus'">
          <TaktDictTag
            :value="getDocumentDictValue(record, 'documentStatus')"
            dict-type="sys_publish_status"
          />
        </template>
      </template>
      <template #detail>
        <DocumentVersionPanel
          ref="documentVersionPanelRef"
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
      <DocumentForm
        :key="formData?.documentId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 版本控制窗口（选中文档后工具栏/行操作打开；含版本 CRUD） -->
    <TaktModal
      v-model:open="versionWindowVisible"
      :title="versionWindowTitle"
      width="1200px"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleVersionWindowClose"
    >
      <div
        v-if="versionWindowVisible"
        class="h-[70vh] min-h-[420px] flex flex-col overflow-hidden"
      >
        <DocumentVersionPanel
          ref="documentVersionWindowPanelRef"
          class="h-full min-h-0 flex-1"
        />
      </div>
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-routine-document-center-document'"
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
      <div v-show="isFieldVisible('documentCode')">
      <a-form-item :label="pi.queryLabel('documentCode')">
        <a-input
          v-model:value="advancedQueryForm.documentCode"
          :placeholder="pi.queryPh('documentCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentTitle')">
      <a-form-item :label="pi.queryLabel('documentTitle')">
        <a-input
          v-model:value="advancedQueryForm.documentTitle"
          :placeholder="pi.queryPh('documentTitle', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentCategory')">
      <a-form-item :label="pi.queryLabel('documentCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentCategory"
          dict-type="routine_document_center_category"
          :placeholder="pi.queryPh('documentCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('confidentialLevel')">
      <a-form-item :label="pi.queryLabel('confidentialLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.confidentialLevel"
          dict-type="routine_document_center_confidential_level"
          :placeholder="pi.queryPh('confidentialLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('version')">
      <a-form-item :label="pi.queryLabel('version')">
        <a-input-number
          v-model:value="advancedQueryForm.version"
          :placeholder="pi.queryPh('version', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentContent')">
      <a-form-item :label="pi.queryLabel('documentContent')">
        <a-textarea
          v-model:value="advancedQueryForm.documentContent"
          :placeholder="pi.queryPh('documentContent', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentSummary')">
      <a-form-item :label="pi.queryLabel('documentSummary')">
        <a-input
          v-model:value="advancedQueryForm.documentSummary"
          :placeholder="pi.queryPh('documentSummary', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentTags')">
      <a-form-item :label="pi.queryLabel('documentTags')">
        <a-input
          v-model:value="advancedQueryForm.documentTags"
          :placeholder="pi.queryPh('documentTags', 'required')"
          show-count
          :maxlength="500"
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
      <div v-show="isFieldVisible('documentEffectiveTimeStart')">
      <a-form-item :label="pi.queryLabel('documentEffectiveTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentEffectiveTimeStart"
          :placeholder="pi.queryPh('documentEffectiveTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentEffectiveTimeEnd')">
      <a-form-item :label="pi.queryLabel('documentEffectiveTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentEffectiveTimeEnd"
          :placeholder="pi.queryPh('documentEffectiveTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentExpireTimeStart')">
      <a-form-item :label="pi.queryLabel('documentExpireTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentExpireTimeStart"
          :placeholder="pi.queryPh('documentExpireTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentExpireTimeEnd')">
      <a-form-item :label="pi.queryLabel('documentExpireTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentExpireTimeEnd"
          :placeholder="pi.queryPh('documentExpireTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPublishTimeStart')">
      <a-form-item :label="pi.queryLabel('documentPublishTimeStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentPublishTimeStart"
          :placeholder="pi.queryPh('documentPublishTimeStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPublishTimeEnd')">
      <a-form-item :label="pi.queryLabel('documentPublishTimeEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.documentPublishTimeEnd"
          :placeholder="pi.queryPh('documentPublishTimeEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherId')">
      <a-form-item :label="pi.queryLabel('publisherId')">
        <TaktSelect
          v-model:value="advancedQueryForm.publisherId"
          api-url="TaktUsers/options"
          :placeholder="pi.queryPh('publisherId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publisherName')">
      <a-form-item :label="pi.queryLabel('publisherName')">
        <a-input
          v-model:value="advancedQueryForm.publisherName"
          :placeholder="pi.queryPh('publisherName', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="pi.queryLabel('deptId')">
        <TaktSelect
          v-model:value="advancedQueryForm.deptId"
          api-url="TaktDepts/tree-options"
          :placeholder="pi.queryPh('deptId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptName')">
      <a-form-item :label="pi.queryLabel('deptName')">
        <a-input
          v-model:value="advancedQueryForm.deptName"
          :placeholder="pi.queryPh('deptName', 'required')"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentIsTop')">
      <a-form-item :label="pi.queryLabel('documentIsTop')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentIsTop"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('documentIsTop', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentViewCount')">
      <a-form-item :label="pi.queryLabel('documentViewCount')">
        <a-input-number
          v-model:value="advancedQueryForm.documentViewCount"
          :placeholder="pi.queryPh('documentViewCount', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetScope')">
      <a-form-item :label="pi.queryLabel('targetScope')">
        <TaktSelect
          v-model:value="advancedQueryForm.targetScope"
          dict-type="sys_publish_scope"
          :placeholder="pi.queryPh('targetScope', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetDepartments')">
      <a-form-item :label="pi.queryLabel('targetDepartments')">
        <a-input
          v-model:value="advancedQueryForm.targetDepartments"
          :placeholder="pi.queryPh('targetDepartments', 'required')"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetUsers')">
      <a-form-item :label="pi.queryLabel('targetUsers')">
        <a-input
          v-model:value="advancedQueryForm.targetUsers"
          :placeholder="pi.queryPh('targetUsers', 'required')"
          show-count
          :maxlength="2000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentStatus')">
      <a-form-item :label="pi.queryLabel('documentStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentStatus"
          dict-type="sys_publish_status"
          :placeholder="pi.queryPh('documentStatus', 'select')"
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
        :entity-i18n-key="DOCUMENT_SELF_I18N_KEY"
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
      :id-column-key="'documentId'"
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
 * 文管中心主实体 支持制度、流程、模板等文档的分类、版本与权限控制管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/routine/document-center/document
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import DocumentForm from './components/DocumentForm.vue'
import DocumentVersionPanel from './components/DocumentVersionPanel.vue'
import { provideDocumentMasterContext, type DocumentRowRecord } from './composables/useDocumentMasterContext'
import { getDocumentList, getDocumentById, createDocument, updateDocument, deleteDocumentById, deleteDocumentBatch, getDocumentTemplate, importDocument, exportDocument, updateDocumentStatus } from '@/api/routine/document-center/document.api'
import type { Document, DocumentQuery } from '@/types/routine/document-center/document.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine, RiGitBranchLine } from '@remixicon/vue'
import type { ToolBarAction } from '@/components/business/TaktToolsBar/index.vue'

import {
  useDocumentI18n,
  DOCUMENT_LIST_FIELDS,
  DOCUMENT_QUERY_STRING_FIELDS,
  DOCUMENT_QUERY_FIELDS,
  DOCUMENT_SELF_I18N_KEY,
} from './composables/useDocumentI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useDocumentI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktDocument')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Document[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<DocumentRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<DocumentRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Document> | null>(null)
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
  for (const key of DOCUMENT_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.documentCategory !== undefined && form.documentCategory !== null) {
    return true
  }
  if (form.confidentialLevel !== undefined && form.confidentialLevel !== null) {
    return true
  }
  if (form.version !== undefined && form.version !== null) {
    return true
  }
  if (form.documentIsTop !== undefined && form.documentIsTop !== null) {
    return true
  }
  if (form.documentViewCount !== undefined && form.documentViewCount !== null) {
    return true
  }
  if (form.targetScope !== undefined && form.targetScope !== null) {
    return true
  }
  if (form.documentStatus !== undefined && form.documentStatus !== null) {
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
  const form = Object.fromEntries(DOCUMENT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof DOCUMENT_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    documentCategory: undefined as number | undefined,
    confidentialLevel: undefined as number | undefined,
    version: undefined as number | undefined,
    documentIsTop: undefined as number | undefined,
    documentViewCount: undefined as number | undefined,
    targetScope: undefined as number | undefined,
    documentStatus: undefined as number | undefined,
    approvalStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  DOCUMENT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'documentId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideDocumentMasterContext()
const documentVersionPanelRef = ref<InstanceType<typeof DocumentVersionPanel> | null>(null)
/** 版本窗口内面板 ref */
const documentVersionWindowPanelRef = ref<InstanceType<typeof DocumentVersionPanel> | null>(null)
/** 版本控制窗口是否打开 */
const versionWindowVisible = ref(false)

/** 版本窗口标题 */
const versionWindowTitle = computed(() => {
  const row = selectedMasterRow.value as Record<string, unknown> | null
  const title = row?.documentTitle != null ? String(row.documentTitle) : ''
  const code = row?.documentCode != null ? String(row.documentCode) : ''
  const base = t('common.page.button.version')
  if (title || code) {
    return `${base} — ${title || code}`
  }
  return base
})

/** 工具栏扩展：版本控制（须选中一行） */
const toolbarLeftActions = computed<ToolBarAction[]>(() => [
  {
    key: 'version',
    label: t('common.page.button.version'),
    icon: RiGitBranchLine,
    buttonClass: 'takt-button-version',
    permission: 'routine:document:center:version',
    disabled: selectedRows.value.length !== 1 && !selectedMasterKey.value,
    onClick: () => handleOpenVersionWindow(),
  },
])

/**
 * 打开版本控制窗口（选中当前文档）
 * @param {DocumentRowRecord} [record] 行；省略时用工具栏当前选中
 * @returns {void}
 */
function handleOpenVersionWindow(record?: DocumentRowRecord): void {
  const target = record
    ?? (selectedRows.value.length === 1 ? selectedRows.value[0] : null)
    ?? (selectedMasterRow.value as DocumentRowRecord | null)
  if (!target) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.version'),
      entity: pi.self(),
    }))
    return
  }
  selectedMasterRow.value = target
  selectedMasterKey.value = getDocumentId(target)
  versionWindowVisible.value = true
  nextTick(() => {
    documentVersionWindowPanelRef.value?.reload?.()
  })
}

/**
 * 关闭版本控制窗口
 * @returns {void}
 */
function handleVersionWindowClose(): void {
  versionWindowVisible.value = false
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {DocumentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<DocumentQuery>): DocumentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: DocumentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof DocumentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of DOCUMENT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.documentCategory !== undefined && form.documentCategory !== null) {
    query.documentCategory = form.documentCategory
  }
  if (form.confidentialLevel !== undefined && form.confidentialLevel !== null) {
    query.confidentialLevel = form.confidentialLevel
  }
  if (form.version !== undefined && form.version !== null) {
    query.version = form.version
  }
  if (form.documentIsTop !== undefined && form.documentIsTop !== null) {
    query.documentIsTop = form.documentIsTop
  }
  if (form.documentViewCount !== undefined && form.documentViewCount !== null) {
    query.documentViewCount = form.documentViewCount
  }
  if (form.targetScope !== undefined && form.targetScope !== null) {
    query.targetScope = form.targetScope
  }
  if (form.documentStatus !== undefined && form.documentStatus !== null) {
    query.documentStatus = form.documentStatus
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
function syncMasterSelection(record: DocumentRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getDocumentId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as DocumentRowRecord
  const key = getDocumentId(row)
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
async function loadDocumentDetail(record: DocumentRowRecord): Promise<Document | null> {
  const id = getDocumentId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getDocumentById(id)
    const index = dataSource.value.findIndex((row) => getDocumentId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Document
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
    dataIndex: 'documentId',
    key: 'documentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentId') ?? ''
  },
  {
    title: pi.label('documentCode'),
    dataIndex: 'documentCode',
    key: 'documentCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentCode') ?? ''
  },
  {
    title: pi.label('documentTitle'),
    dataIndex: 'documentTitle',
    key: 'documentTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentTitle') ?? ''
  },
  {
    title: pi.label('documentCategory'),
    dataIndex: 'documentCategory',
    key: 'documentCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('confidentialLevel'),
    dataIndex: 'confidentialLevel',
    key: 'confidentialLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('version'),
    dataIndex: 'version',
    key: 'version',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'version') ?? ''
  },
  {
    title: pi.label('documentContent'),
    dataIndex: 'documentContent',
    key: 'documentContent',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentContent') ?? ''
  },
  {
    title: pi.label('documentSummary'),
    dataIndex: 'documentSummary',
    key: 'documentSummary',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentSummary') ?? ''
  },
  {
    title: pi.label('documentTags'),
    dataIndex: 'documentTags',
    key: 'documentTags',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentTags') ?? ''
  },
  {
    title: pi.label('fileName'),
    dataIndex: 'fileName',
    key: 'fileName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'fileName') ?? ''
  },
  {
    title: pi.label('accessUrl'),
    dataIndex: 'accessUrl',
    key: 'accessUrl',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'accessUrl') ?? ''
  },
  {
    title: pi.label('documentEffectiveTime'),
    dataIndex: 'documentEffectiveTime',
    key: 'documentEffectiveTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentEffectiveTime') ?? ''
  },
  {
    title: pi.label('documentExpireTime'),
    dataIndex: 'documentExpireTime',
    key: 'documentExpireTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentExpireTime') ?? ''
  },
  {
    title: pi.label('documentPublishTime'),
    dataIndex: 'documentPublishTime',
    key: 'documentPublishTime',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentPublishTime') ?? ''
  },
  {
    title: pi.label('publisherId'),
    dataIndex: 'publisherId',
    key: 'publisherId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'publisherId') ?? ''
  },
  {
    title: pi.label('publisherName'),
    dataIndex: 'publisherName',
    key: 'publisherName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'publisherName') ?? ''
  },
  {
    title: pi.label('deptId'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'deptId') ?? ''
  },
  {
    title: pi.label('deptName'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'deptName') ?? ''
  },
  {
    title: pi.label('documentIsTop'),
    dataIndex: 'documentIsTop',
    key: 'documentIsTop',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('documentViewCount'),
    dataIndex: 'documentViewCount',
    key: 'documentViewCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'documentViewCount') ?? ''
  },
  {
    title: pi.label('targetScope'),
    dataIndex: 'targetScope',
    key: 'targetScope',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('targetDepartments'),
    dataIndex: 'targetDepartments',
    key: 'targetDepartments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'targetDepartments') ?? ''
  },
  {
    title: pi.label('targetUsers'),
    dataIndex: 'targetUsers',
    key: 'targetUsers',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getDocumentField(record, 'targetUsers') ?? ''
  },
  {
    title: pi.label('documentStatus'),
    dataIndex: 'documentStatus',
    key: 'documentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn({
    actions: [
      {
        key: 'version',
        label: t('common.page.button.version'),
        shape: 'plain',
        icon: RiGitBranchLine,
        permission: 'routine:document:center:version',
        onClick: (record: DocumentRowRecord) => handleOpenVersionWindow(record)
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:document:center:update',
        onClick: (record: DocumentRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:document:center:delete',
        onClick: (record: DocumentRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getDocumentId = (record: DocumentRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getDocumentField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getDocumentDictValue = (
  record: DocumentRowRecord,
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
  onChange: (keys: (string | number)[], rows: DocumentRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: DocumentRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getDocumentId(selectedRow.value) === getDocumentId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: DocumentRowRecord[]) => {
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
    const res = await getDocumentList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Document] 加载数据失败', { error })
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
  documentCode: '',
  documentTitle: '',
  documentCategory: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  documentContent: '',
  documentSummary: '',
  documentTags: '',
  fileName: '',
  accessUrl: '',
  documentEffectiveTimeStart: '',
  documentEffectiveTimeEnd: '',
  documentExpireTimeStart: '',
  documentExpireTimeEnd: '',
  documentPublishTimeStart: '',
  documentPublishTimeEnd: '',
  publisherId: '',
  publisherName: '',
  deptId: '',
  deptName: '',
  documentIsTop: undefined as number | undefined,
  documentViewCount: undefined as number | undefined,
  targetScope: undefined as number | undefined,
  targetDepartments: '',
  targetUsers: '',
  documentStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: DocumentRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadDocumentDetail(record)
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
      await updateDocument(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createDocument(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  documentVersionPanelRef.value?.reload?.()
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
  const res = await getDocumentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importDocument(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    documentVersionPanelRef.value?.reload?.()
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
    const exportMeta = await exportDocument(
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
    logger.error('[Document] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: DocumentRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteDocumentById((record as any)[entityIdName])
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
      await deleteDocumentBatch(ids)
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
  documentCode: '',
  documentTitle: '',
  documentCategory: undefined as number | undefined,
  confidentialLevel: undefined as number | undefined,
  version: undefined as number | undefined,
  documentContent: '',
  documentSummary: '',
  documentTags: '',
  fileName: '',
  accessUrl: '',
  documentEffectiveTimeStart: '',
  documentEffectiveTimeEnd: '',
  documentExpireTimeStart: '',
  documentExpireTimeEnd: '',
  documentPublishTimeStart: '',
  documentPublishTimeEnd: '',
  publisherId: '',
  publisherName: '',
  deptId: '',
  deptName: '',
  documentIsTop: undefined as number | undefined,
  documentViewCount: undefined as number | undefined,
  targetScope: undefined as number | undefined,
  targetDepartments: '',
  targetUsers: '',
  documentStatus: undefined as number | undefined,
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
</script>

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order-item/components -->
<!-- 文件名称：IpqcDefectHandlingPanel.vue -->
<!-- 功能描述：IPQC制程检验单明细实体主表实体右侧明细 ipqcDefectHandling 独立 CRUD（按主表选中 ipqcOrderItemId 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="ipqc-defect-handling-panel flex h-full min-h-0 flex-col overflow-hidden">
    <div class="mb-2 text-sm font-medium text-text">
      {{ t('entity.ipqcdefecthandling._self') }}
    </div>
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="logistics:quality:operation:ipqc:order:create"
      update-permission="logistics:quality:operation:ipqc:order:update"
      delete-permission="logistics:quality:operation:ipqc:order:delete"
      import-permission="logistics:quality:operation:ipqc:order:import"
      export-permission="logistics:quality:operation:ipqc:order:export"
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
    <div class="ipqc-defect-handling-panel__table-wrap min-h-0 flex-1 overflow-hidden">
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="company"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getIpqcDefectHandlingId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="ipqcDefectHandlingId"
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
      <IpqcDefectHandlingForm
        ref="formRef"
        :form-data="formData"
        :master-id="masterIpqcOrderItemId"
        :loading="formLoading"
      />
    </TaktModal>

    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="takt-query-fields-logistics-quality-operation-ipqc-order-item-ipqc-defect-handling"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ipqcDefectHandlingCode')">
      <a-form-item :label="t('entity.ipqcdefecthandling.code')">
        <a-input
          v-model:value="advancedQueryForm.ipqcDefectHandlingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.code') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ipqcOrderCode')">
      <a-form-item :label="t('entity.ipqcdefecthandling.ipqcordercode')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.ipqcordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ipqcdefecthandling.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectType')">
      <a-form-item :label="t('entity.ipqcdefecthandling.defecttype')">
        <a-input-number
          v-model:value="advancedQueryForm.defectType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defecttype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectCode')">
      <a-form-item :label="t('entity.ipqcdefecthandling.defectcode')">
        <a-input
          v-model:value="advancedQueryForm.defectCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectDescription')">
      <a-form-item :label="t('entity.ipqcdefecthandling.defectdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.defectDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcdefecthandling.defectdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectQuantity')">
      <a-form-item :label="t('entity.ipqcdefecthandling.defectquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.defectQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingMethod')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlingmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlingmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingDescription')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlingdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.handlingDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcdefecthandling.handlingdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleDept')">
      <a-form-item :label="t('entity.ipqcdefecthandling.responsibledept')">
        <a-input
          v-model:value="advancedQueryForm.responsibleDept"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.responsibledept') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('responsibleGroupName')">
      <a-form-item :label="t('entity.ipqcdefecthandling.responsiblegroupname')">
        <a-input
          v-model:value="advancedQueryForm.responsibleGroupName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.responsiblegroupname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlerGroupName')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlergroupname')">
        <a-input
          v-model:value="advancedQueryForm.handlerGroupName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlergroupname') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtStart')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlingatstart')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlingatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingAtEnd')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlingatend')">
        <a-input
          v-model:value="advancedQueryForm.handlingAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlingatend') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('verifyDeptName')">
      <a-form-item :label="t('entity.ipqcdefecthandling.verifydeptname')">
        <a-input
          v-model:value="advancedQueryForm.verifyDeptName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.verifydeptname') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityOwnerGroupName')">
      <a-form-item :label="t('entity.ipqcdefecthandling.qualityownergroupname')">
        <a-input
          v-model:value="advancedQueryForm.qualityOwnerGroupName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.qualityownergroupname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('verifyAtStart')">
      <a-form-item :label="t('entity.ipqcdefecthandling.verifyat')">
        <a-input
          v-model:value="advancedQueryForm.verifyAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.verifyat') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('verifyAtEnd')">
      <a-form-item :label="t('entity.ipqcdefecthandling.verifyat')">
        <a-input
          v-model:value="advancedQueryForm.verifyAtEnd"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.verifyat') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('verifyStatus')">
      <a-form-item :label="t('entity.ipqcdefecthandling.verifystatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.verifyStatus"
          dict-type="logistics_quality_defect_verify_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcdefecthandling.verifystatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('correctiveAction')">
      <a-form-item :label="t('entity.ipqcdefecthandling.correctiveaction')">
        <a-input
          v-model:value="advancedQueryForm.correctiveAction"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.correctiveaction') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defectImages')">
      <a-form-item :label="t('entity.ipqcdefecthandling.defectimages')">
        <a-input
          v-model:value="advancedQueryForm.defectImages"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.defectimages') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('attachments')">
      <a-form-item :label="t('entity.ipqcdefecthandling.attachments')">
        <a-input
          v-model:value="advancedQueryForm.attachments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.attachments') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingStatus')">
      <a-form-item :label="t('entity.ipqcdefecthandling.handlingstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.handlingStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcdefecthandling.handlingstatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ipqcdefecthandling._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.ipqcdefecthandling._self"
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
      id-column-key="ipqcDefectHandlingId"
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
 * IPQC制程检验单明细实体子表 ipqcDefectHandling 右栏面板
 * @module views/logistics/quality/operation/ipqc-order-item/components
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
import IpqcDefectHandlingForm from './IpqcDefectHandlingForm.vue'
import { useIpqcOrderItemMasterContext } from '../composables/useIpqcOrderItemMasterContext'
import {
  getIpqcDefectHandlingList,
  getIpqcDefectHandlingById,
  createIpqcDefectHandling,
  updateIpqcDefectHandling,
  deleteIpqcDefectHandlingById,
  deleteIpqcDefectHandlingBatch,
  getIpqcDefectHandlingTemplate,
  importIpqcDefectHandling,
  exportIpqcDefectHandling,
} from '@/api/logistics/quality/operation/ipqcDefectHandling.api'
import type { IpqcDefectHandling, IpqcDefectHandlingQuery } from '@/types/logistics/quality/operation/ipqcDefectHandling.types'

const { t } = useI18n()
const { selecmedMasterRow } = useIpqcOrderItemMasterContext()

/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIpqcDefectHandling')
/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcdefecthandling._self') }),
)

const loading = ref(false)
const dataSource = ref<IpqcDefectHandling[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<IpqcDefectHandling | null>(null)
const selecmedRows = ref<IpqcDefectHandling[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<IpqcDefectHandling>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  ipqcDefectHandlingCode: '',
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectType: undefined as number | undefined,
  defectCode: '',
  defectDescription: '',
  defectQuantity: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  responsibleDept: '',
  responsibleGroupName: '',
  handlerGroupName: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  verifyDeptName: '',
  qualityOwnerGroupName: '',
  verifyAtStart: '',
  verifyAtEnd: '',
  verifyStatus: undefined as number | undefined,
  correctiveAction: '',
  defectImages: '',
  attachments: '',
  handlingStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() => [
  { key: 'ipqcDefectHandlingCode', label: t('entity.ipqcdefecthandling.code') },
  { key: 'ipqcOrderCode', label: t('entity.ipqcdefecthandling.ipqcordercode') },
  { key: 'lineNumber', label: t('entity.ipqcdefecthandling.linenumber') },
  { key: 'defectType', label: t('entity.ipqcdefecthandling.defecttype') },
  { key: 'defectCode', label: t('entity.ipqcdefecthandling.defectcode') },
  { key: 'defectDescription', label: t('entity.ipqcdefecthandling.defectdescription') },
  { key: 'defectQuantity', label: t('entity.ipqcdefecthandling.defectquantity') },
  { key: 'handlingMethod', label: t('entity.ipqcdefecthandling.handlingmethod') },
  { key: 'handlingDescription', label: t('entity.ipqcdefecthandling.handlingdescription') },
  { key: 'responsibleDept', label: t('entity.ipqcdefecthandling.responsibledept') },
  { key: 'responsibleGroupName', label: t('entity.ipqcdefecthandling.responsiblegroupname') },
  { key: 'handlerGroupName', label: t('entity.ipqcdefecthandling.handlergroupname') },
  { key: 'handlingAtStart', label: t('entity.ipqcdefecthandling.handlingatstart') },
  { key: 'handlingAtEnd', label: t('entity.ipqcdefecthandling.handlingatend') },
  { key: 'verifyDeptName', label: t('entity.ipqcdefecthandling.verifydeptname') },
  { key: 'qualityOwnerGroupName', label: t('entity.ipqcdefecthandling.qualityownergroupname') },
  { key: 'verifyAtStart', label: t('entity.ipqcdefecthandling.verifyat') },
  { key: 'verifyAtEnd', label: t('entity.ipqcdefecthandling.verifyat') },
  { key: 'verifyStatus', label: t('entity.ipqcdefecthandling.verifystatus') },
  { key: 'correctiveAction', label: t('entity.ipqcdefecthandling.correctiveaction') },
  { key: 'defectImages', label: t('entity.ipqcdefecthandling.defectimages') },
  { key: 'attachments', label: t('entity.ipqcdefecthandling.attachments') },
  { key: 'handlingStatus', label: t('entity.ipqcdefecthandling.handlingstatus') },
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
  ipqcDefectHandlingCode: '',
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  defectType: undefined as number | undefined,
  defectCode: '',
  defectDescription: '',
  defectQuantity: undefined as number | undefined,
  handlingMethod: undefined as number | undefined,
  handlingDescription: '',
  responsibleDept: '',
  responsibleGroupName: '',
  handlerGroupName: '',
  handlingAtStart: '',
  handlingAtEnd: '',
  verifyDeptName: '',
  qualityOwnerGroupName: '',
  verifyAtStart: '',
  verifyAtEnd: '',
  verifyStatus: undefined as number | undefined,
  correctiveAction: '',
  defectImages: '',
  attachments: '',
  handlingStatus: undefined as number | undefined,
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

const entityIdName = 'ipqcDefectHandlingId'
const hasMasterSelection = computed(() => !!selecmedMasterRow.value?.ipqcOrderItemId)
const masterIpqcOrderItemId = computed(() => selecmedMasterRow.value?.ipqcOrderItemId ?? '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function getIpqcDefectHandlingId(record: IpqcDefectHandling | Record<string, unknown>): string {
  return String((record as IpqcDefectHandling)?.[entityIdName] ?? '')
}

function getIpqcDefectHandlingField(record: IpqcDefectHandling | Record<string, unknown>, field: string): unknown {
  return (record as IpqcDefectHandling)?.[field as keyof IpqcDefectHandling]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'ipqcDefectHandlingId',
    key: 'ipqcDefectHandlingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'ipqcDefectHandlingId') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.code'),
    dataIndex: 'ipqcDefectHandlingCode',
    key: 'ipqcDefectHandlingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'ipqcDefectHandlingCode') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.ipqcordercode'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'ipqcOrderCode') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'lineNumber') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.defecttype'),
    dataIndex: 'defectType',
    key: 'defectType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'defectType') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.defectcode'),
    dataIndex: 'defectCode',
    key: 'defectCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'defectCode') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.defectdescription'),
    dataIndex: 'defectDescription',
    key: 'defectDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'defectDescription') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.defectquantity'),
    dataIndex: 'defectQuantity',
    key: 'defectQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'defectQuantity') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.handlingmethod'),
    dataIndex: 'handlingMethod',
    key: 'handlingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'handlingMethod') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.handlingdescription'),
    dataIndex: 'handlingDescription',
    key: 'handlingDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'handlingDescription') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.responsibledept'),
    dataIndex: 'responsibleDept',
    key: 'responsibleDept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'responsibleDept') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.responsiblegroupname'),
    dataIndex: 'responsibleGroupName',
    key: 'responsibleGroupName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'responsibleGroupName') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.handlergroupname'),
    dataIndex: 'handlerGroupName',
    key: 'handlerGroupName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'handlerGroupName') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.handlingat'),
    dataIndex: 'handlingAt',
    key: 'handlingAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'handlingAt') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.verifydeptname'),
    dataIndex: 'verifyDeptName',
    key: 'verifyDeptName',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'verifyDeptName') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.qualityownergroupname'),
    dataIndex: 'qualityOwnerGroupName',
    key: 'qualityOwnerGroupName',
    width: 140,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'qualityOwnerGroupName') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.verifyat'),
    dataIndex: 'verifyAt',
    key: 'verifyAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'verifyAt') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.verifystatus'),
    dataIndex: 'verifyStatus',
    key: 'verifyStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'verifyStatus') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.correctiveaction'),
    dataIndex: 'correctiveAction',
    key: 'correctiveAction',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'correctiveAction') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.defectimages'),
    dataIndex: 'defectImages',
    key: 'defectImages',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'defectImages') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.attachments'),
    dataIndex: 'attachments',
    key: 'attachments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'attachments') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.handlingstatus'),
    dataIndex: 'handlingStatus',
    key: 'handlingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'handlingStatus') ?? ''),
  },
  {
    title: t('entity.ipqcdefecthandling.orderitem'),
    dataIndex: 'orderItem',
    key: 'orderItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: IpqcDefectHandling }) =>
      String(getIpqcDefectHandlingField(record, 'orderItem') ?? ''),
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:ipqc:order:update',
        onClick: (record: IpqcDefectHandling) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqc:order:delete',
        onClick: (record: IpqcDefectHandling) => void handleDeleteOne(record),
      }],
  })])

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IpqcDefectHandling[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: IpqcDefectHandling, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getIpqcDefectHandlingId(selecmedRow.value) === getIpqcDefectHandlingId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: IpqcDefectHandling[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: IpqcDefectHandling) {
  const key = getIpqcDefectHandlingId(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {IpqcDefectHandlingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IpqcDefectHandlingQuery>): IpqcDefectHandlingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IpqcDefectHandlingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ipqcOrderItemId: masterIpqcOrderItemId.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IpqcDefectHandlingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ipqcDefectHandlingCode', form.ipqcDefectHandlingCode)
  assignTrimmed('ipqcOrderCode', form.ipqcOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.defectType !== undefined && form.defectType !== null) {
    query.defectType = form.defectType
  }
  assignTrimmed('defectCode', form.defectCode)
  assignTrimmed('defectDescription', form.defectDescription)
  if (form.defectQuantity !== undefined && form.defectQuantity !== null) {
    query.defectQuantity = form.defectQuantity
  }
  if (form.handlingMethod !== undefined && form.handlingMethod !== null) {
    query.handlingMethod = form.handlingMethod
  }
  assignTrimmed('handlingDescription', form.handlingDescription)
  assignTrimmed('responsibleDept', form.responsibleDept)
  assignTrimmed('responsibleGroupName', form.responsibleGroupName)
  assignTrimmed('handlerGroupName', form.handlerGroupName)
  assignTrimmed('handlingAtStart', form.handlingAtStart)
  assignTrimmed('handlingAtEnd', form.handlingAtEnd)
  assignTrimmed('verifyDeptName', form.verifyDeptName)
  assignTrimmed('qualityOwnerGroupName', form.qualityOwnerGroupName)
  assignTrimmed('verifyAtStart', form.verifyAtStart)
  assignTrimmed('verifyAtEnd', form.verifyAtEnd)
  if (form.verifyStatus !== undefined && form.verifyStatus !== null) {
    query.verifyStatus = form.verifyStatus
  }
  assignTrimmed('correctiveAction', form.correctiveAction)
  assignTrimmed('defectImages', form.defectImages)
  assignTrimmed('attachments', form.attachments)
  if (form.handlingStatus !== undefined && form.handlingStatus !== null) {
    query.handlingStatus = form.handlingStatus
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
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await getIpqcDefectHandlingList(buildListQuery())
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
watch(masterIpqcOrderItemId, () => {
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ipqcdefecthandling._self') })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: IpqcDefectHandling) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ipqcdefecthandling._self') })
  formLoading.value = true
  try {
    const detail = await getIpqcDefectHandlingById(getIpqcDefectHandlingId(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: t('entity.ipqcdefecthandling._self'),
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
    const id = formData.value?.ipqcDefectHandlingId
    if (id) {
      await updateIpqcDefectHandling(id, payload)
      message.success(t('common.feedback.updated', { target: t('entity.ipqcdefecthandling._self') }))
    } else {
      await createIpqcDefectHandling(payload)
      message.success(t('common.feedback.created', { target: t('entity.ipqcdefecthandling._self') }))
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

async function handleDeleteOne(record: IpqcDefectHandling) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: t('entity.ipqcdefecthandling._self'),
      name: t('common.tip.this.target', { target: t('entity.ipqcdefecthandling._self') }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcDefectHandlingById(getIpqcDefectHandlingId(record))
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcdefecthandling._self') }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: t('entity.ipqcdefecthandling._self'),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: t('entity.ipqcdefecthandling._self'),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => getIpqcDefectHandlingId(r)).filter(Boolean)
      await deleteIpqcDefectHandlingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcdefecthandling._self') }))
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
  const res = await getIpqcDefectHandlingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importIpqcDefectHandling(file, sheetName)
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
    const exportMeta = await exportIpqcDefectHandling(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcdefecthandling._self') }))
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: t('entity.ipqcdefecthandling._self') }))
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

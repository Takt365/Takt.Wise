<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：IPQC制程检验单明细实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
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
      create-permission="logistics:quality:operation:ipqc:order:create"
      update-permission="logistics:quality:operation:ipqc:order:update"
      delete-permission="logistics:quality:operation:ipqc:order:delete"
      import-permission="logistics:quality:operation:ipqc:order:import"
      export-permission="logistics:quality:operation:ipqc:order:export"
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

    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getIpqcOrderItemId"
      :master-row-selection="rowSelection"
      master-id-column-key="ipqcOrderItemId"
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
      <template #detail>
        <IpqcDefectHandlingPanel
          ref="ipqcDefectHandlingPanelRef"
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
      <IpqcOrderItemForm
        :key="formData?.ipqcOrderItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-quality-operation-ipqc-order-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ipqcOrderId')">
      <a-form-item :label="t('entity.ipqcorderitem.ipqcorderid')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.ipqcorderid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ipqcOrderCode')">
      <a-form-item :label="t('entity.ipqcorderitem.ipqcordercode')">
        <a-input
          v-model:value="advancedQueryForm.ipqcOrderCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.ipqcordercode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="t('entity.ipqcorderitem.linenumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.linenumber') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.ipqcorderitem.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.materialcode') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="t('entity.ipqcorderitem.materialdescription')">
        <a-input
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.materialdescription') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchCode')">
      <a-form-item :label="t('entity.ipqcorderitem.batchCode')">
        <a-input
          v-model:value="advancedQueryForm.batchCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.batchCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.productionquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.productionQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.productionquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardCode')">
      <a-form-item :label="t('entity.ipqcorderitem.standardcode')">
        <a-input
          v-model:value="advancedQueryForm.standardCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.standardcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samplingSchemeCode')">
      <a-form-item :label="t('entity.ipqcorderitem.samplingschemecode')">
        <a-input
          v-model:value="advancedQueryForm.samplingSchemeCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.samplingschemecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionMethod')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectionmethod')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectionmethod') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.samplequantity')">
        <a-input-number
          v-model:value="advancedQueryForm.sampleQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.samplequantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.qualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.qualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.qualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unqualifiedQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.unqualifiedquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.unqualifiedQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.unqualifiedquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionReturnQuantity')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectionreturnquantity')">
        <a-input-number
          v-model:value="advancedQueryForm.inspectionReturnQuantity"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectionreturnquantity') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sampleSerialCode')">
      <a-form-item :label="t('entity.ipqcorderitem.sampleserialCode')">
        <a-input
          v-model:value="advancedQueryForm.sampleSerialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.sampleserialCode') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDescription')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondescription')">
        <a-textarea
          v-model:value="advancedQueryForm.inspectionDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcorderitem.inspectiondescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectorBy')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectorby')">
        <a-input
          v-model:value="advancedQueryForm.inspectorBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.inspectorby') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateStart')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorderitem.inspectiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inspectionDateEnd')">
      <a-form-item :label="t('entity.ipqcorderitem.inspectiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.inspectionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcorderitem.inspectiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('judgeStatus')">
      <a-form-item :label="t('entity.ipqcorderitem.judgestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.judgeStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcorderitem.judgestatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.ipqcorderitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        entity-i18n-key="entity.ipqcorderitem._self"
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
      :id-column-key="'ipqcOrderItemId'"
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
 * IPQC制程检验单明细实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order-item
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import IpqcOrderItemForm from './components/IpqcOrderItemForm.vue'
import IpqcDefectHandlingPanel from './components/IpqcDefectHandlingPanel.vue'
import { provideIpqcOrderItemMasterContext } from './composables/useIpqcOrderItemMasterContext'
import { getIpqcOrderItemList, getIpqcOrderItemById, createIpqcOrderItem, updateIpqcOrderItem, deleteIpqcOrderItemById, deleteIpqcOrderItemBatch, getIpqcOrderItemTemplate, importIpqcOrderItem, exportIpqcOrderItem, updateIpqcOrderItemStatus } from '@/api/logistics/quality/operation/ipqcOrderItem.api'
import type { IpqcOrderItem, IpqcOrderItemQuery } from '@/types/logistics/quality/operation/ipqcOrderItem.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktIpqcOrderItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.ipqcorderitem._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<IpqcOrderItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<IpqcOrderItem | null>(null)
/** 表格多选行 */
const selectedRows = ref<IpqcOrderItem[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<IpqcOrderItem> | null>(null)
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
  ipqcOrderId: '',
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  batchCode: '',
  productionQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  sampleSerialCode: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  judgeStatus: undefined as number | undefined,
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'ipqcOrderId', label: t('entity.ipqcorderitem.ipqcorderid') },
  { key: 'ipqcOrderCode', label: t('entity.ipqcorderitem.ipqcordercode') },
  { key: 'lineNumber', label: t('entity.ipqcorderitem.linenumber') },
  { key: 'materialCode', label: t('entity.ipqcorderitem.materialcode') },
  { key: 'materialDescription', label: t('entity.ipqcorderitem.materialdescription') },
  { key: 'batchCode', label: t('entity.ipqcorderitem.batchCode') },
  { key: 'productionQuantity', label: t('entity.ipqcorderitem.productionquantity') },
  { key: 'standardCode', label: t('entity.ipqcorderitem.standardcode') },
  { key: 'samplingSchemeCode', label: t('entity.ipqcorderitem.samplingschemecode') },
  { key: 'inspectionMethod', label: t('entity.ipqcorderitem.inspectionmethod') },
  { key: 'sampleQuantity', label: t('entity.ipqcorderitem.samplequantity') },
  { key: 'qualifiedQuantity', label: t('entity.ipqcorderitem.qualifiedquantity') },
  { key: 'unqualifiedQuantity', label: t('entity.ipqcorderitem.unqualifiedquantity') },
  { key: 'inspectionReturnQuantity', label: t('entity.ipqcorderitem.inspectionreturnquantity') },
  { key: 'sampleSerialCode', label: t('entity.ipqcorderitem.sampleserialCode') },
  { key: 'inspectionDescription', label: t('entity.ipqcorderitem.inspectiondescription') },
  { key: 'inspectorBy', label: t('entity.ipqcorderitem.inspectorby') },
  { key: 'inspectionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.ipqcorderitem.inspectiondate')) },
  { key: 'inspectionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.ipqcorderitem.inspectiondate')) },
  { key: 'judgeStatus', label: t('entity.ipqcorderitem.judgestatus') },
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
const entityIdName = 'ipqcOrderItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideIpqcOrderItemMasterContext()
const ipqcDefectHandlingPanelRef = ref<InstanceType<typeof IpqcDefectHandlingPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {IpqcOrderItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<IpqcOrderItemQuery>): IpqcOrderItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: IpqcOrderItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof IpqcOrderItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ipqcOrderId', form.ipqcOrderId)
  assignTrimmed('ipqcOrderCode', form.ipqcOrderCode)
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  assignTrimmed('materialCode', form.materialCode)
  assignTrimmed('materialDescription', form.materialDescription)
  assignTrimmed('batchCode', form.batchCode)
  if (form.productionQuantity !== undefined && form.productionQuantity !== null) {
    query.productionQuantity = form.productionQuantity
  }
  assignTrimmed('standardCode', form.standardCode)
  assignTrimmed('samplingSchemeCode', form.samplingSchemeCode)
  if (form.inspectionMethod !== undefined && form.inspectionMethod !== null) {
    query.inspectionMethod = form.inspectionMethod
  }
  if (form.sampleQuantity !== undefined && form.sampleQuantity !== null) {
    query.sampleQuantity = form.sampleQuantity
  }
  if (form.qualifiedQuantity !== undefined && form.qualifiedQuantity !== null) {
    query.qualifiedQuantity = form.qualifiedQuantity
  }
  if (form.unqualifiedQuantity !== undefined && form.unqualifiedQuantity !== null) {
    query.unqualifiedQuantity = form.unqualifiedQuantity
  }
  if (form.inspectionReturnQuantity !== undefined && form.inspectionReturnQuantity !== null) {
    query.inspectionReturnQuantity = form.inspectionReturnQuantity
  }
  assignTrimmed('sampleSerialCode', form.sampleSerialCode)
  assignTrimmed('inspectionDescription', form.inspectionDescription)
  assignTrimmed('inspectorBy', form.inspectorBy)
  assignTrimmed('inspectionDateStart', form.inspectionDateStart)
  assignTrimmed('inspectionDateEnd', form.inspectionDateEnd)
  if (form.judgeStatus !== undefined && form.judgeStatus !== null) {
    query.judgeStatus = form.judgeStatus
  }
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

/** 主表行点击选中 key（左右主子表高亮） */
const selectedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: IpqcOrderItem | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getIpqcOrderItemId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as IpqcOrderItem
  const key = getIpqcOrderItemId(row)
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
async function loadIpqcOrderItemDetail(record: IpqcOrderItem): Promise<IpqcOrderItem | null> {
  const id = getIpqcOrderItemId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getIpqcOrderItemById(id)
    const index = dataSource.value.findIndex((row) => getIpqcOrderItemId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as IpqcOrderItem
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
    dataIndex: 'ipqcOrderItemId',
    key: 'ipqcOrderItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'ipqcOrderItemId') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.ipqcorderid'),
    dataIndex: 'ipqcOrderId',
    key: 'ipqcOrderId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'ipqcOrderId') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.ipqcordercode'),
    dataIndex: 'ipqcOrderCode',
    key: 'ipqcOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'ipqcOrderCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.linenumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'lineNumber') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.materialdescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'materialDescription') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.batchCode'),
    dataIndex: 'batchCode',
    key: 'batchCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'batchCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.productionquantity'),
    dataIndex: 'productionQuantity',
    key: 'productionQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'productionQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.standardcode'),
    dataIndex: 'standardCode',
    key: 'standardCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'standardCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.samplingschemecode'),
    dataIndex: 'samplingSchemeCode',
    key: 'samplingSchemeCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'samplingSchemeCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.inspectionmethod'),
    dataIndex: 'inspectionMethod',
    key: 'inspectionMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'inspectionMethod') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.samplequantity'),
    dataIndex: 'sampleQuantity',
    key: 'sampleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'sampleQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.qualifiedquantity'),
    dataIndex: 'qualifiedQuantity',
    key: 'qualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'qualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.unqualifiedquantity'),
    dataIndex: 'unqualifiedQuantity',
    key: 'unqualifiedQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'unqualifiedQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.inspectionreturnquantity'),
    dataIndex: 'inspectionReturnQuantity',
    key: 'inspectionReturnQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'inspectionReturnQuantity') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.sampleserialCode'),
    dataIndex: 'sampleSerialCode',
    key: 'sampleSerialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'sampleSerialCode') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.inspectiondescription'),
    dataIndex: 'inspectionDescription',
    key: 'inspectionDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'inspectionDescription') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.inspectorby'),
    dataIndex: 'inspectorBy',
    key: 'inspectorBy',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'inspectorBy') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.inspectiondate'),
    dataIndex: 'inspectionDate',
    key: 'inspectionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'inspectionDate') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.judgestatus'),
    dataIndex: 'judgeStatus',
    key: 'judgeStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'judgeStatus') ?? ''
  },
  {
    title: t('entity.ipqcorderitem.order'),
    dataIndex: 'order',
    key: 'order',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getIpqcOrderItemField(record, 'order') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:quality:operation:ipqc:order:update',
        onClick: (record: IpqcOrderItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:quality:operation:ipqc:order:delete',
        onClick: (record: IpqcOrderItem) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getIpqcOrderItemId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getIpqcOrderItemField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: IpqcOrderItem[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: IpqcOrderItem, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getIpqcOrderItemId(selectedRow.value) === getIpqcOrderItemId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: IpqcOrderItem[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getIpqcOrderItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[IpqcOrderItem] 加载数据失败', { error })
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
  ipqcOrderId: '',
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  batchCode: '',
  productionQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  sampleSerialCode: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  judgeStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.ipqcorderitem._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: IpqcOrderItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.ipqcorderitem._self') })
  formLoading.value = true
  try {
    const detail = await loadIpqcOrderItemDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.ipqcorderitem._self') }))
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
      await updateIpqcOrderItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.ipqcorderitem._self') }))
    } else {
      await createIpqcOrderItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.ipqcorderitem._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  ipqcDefectHandlingPanelRef.value?.reload?.()
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
  const res = await getIpqcOrderItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importIpqcOrderItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    ipqcDefectHandlingPanelRef.value?.reload?.()
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
    const exportMeta = await exportIpqcOrderItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.ipqcorderitem._self') }))
  } catch (error: any) {
    logger.error('[IpqcOrderItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.ipqcorderitem._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: IpqcOrderItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.ipqcorderitem._self'), name: t('common.tip.this.target', { target: t('entity.ipqcorderitem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteIpqcOrderItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorderitem._self') }))
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.ipqcorderitem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.ipqcorderitem._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteIpqcOrderItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.ipqcorderitem._self') }))
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
  ipqcOrderId: '',
  ipqcOrderCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  batchCode: '',
  productionQuantity: undefined as number | undefined,
  standardCode: '',
  samplingSchemeCode: '',
  inspectionMethod: undefined as number | undefined,
  sampleQuantity: undefined as number | undefined,
  qualifiedQuantity: undefined as number | undefined,
  unqualifiedQuantity: undefined as number | undefined,
  inspectionReturnQuantity: undefined as number | undefined,
  sampleSerialCode: '',
  inspectionDescription: '',
  inspectorBy: '',
  inspectionDateStart: '',
  inspectionDateEnd: '',
  judgeStatus: undefined as number | undefined,
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt物料清单明细实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getBillOfMaterialItemId"
      :master-row-selection="rowSelection"
      master-id-column-key="billOfMaterialItemId"
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
      create-permission="logistics:manufacturing:bom:bill:of:material:create"
      update-permission="logistics:manufacturing:bom:bill:of:material:update"
      delete-permission="logistics:manufacturing:bom:bill:of:material:delete"
      import-permission="logistics:manufacturing:bom:bill:of:material:import"
      export-permission="logistics:manufacturing:bom:bill:of:material:export"
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
        <template v-if="column.key === 'materialUnit'">
          <TaktDictTag
            :value="getBillOfMaterialItemDictValue(record, 'materialUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'isOptional'">
          <TaktDictTag
            :value="getBillOfMaterialItemDictValue(record, 'isOptional')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isPhantom'">
          <TaktDictTag
            :value="getBillOfMaterialItemDictValue(record, 'isPhantom')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isObsolete'">
          <TaktDictTag
            :value="getBillOfMaterialItemDictValue(record, 'isObsolete')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>
      <template #detail>
        <BillOfMaterialSubstitutePanel
          ref="billOfMaterialSubstitutePanelRef"
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
      <BillOfMaterialItemForm
        :key="formData?.billOfMaterialItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-bom-bill-of-material-item'"
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
      <div v-show="isFieldVisible('billOfMaterialId')">
      <a-form-item :label="pi.queryLabel('billOfMaterialId')">
        <a-input
          v-model:value="advancedQueryForm.billOfMaterialId"
          :placeholder="pi.queryPh('billOfMaterialId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('bomCode')">
      <a-form-item :label="pi.queryLabel('bomCode')">
        <a-input
          v-model:value="advancedQueryForm.bomCode"
          :placeholder="pi.queryPh('bomCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lineNumber')">
      <a-form-item :label="pi.queryLabel('lineNumber')">
        <a-input-number
          v-model:value="advancedQueryForm.lineNumber"
          :placeholder="pi.queryPh('lineNumber', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCode"
          api-url="TaktMaterialPlants/options"
          :placeholder="pi.queryPh('materialCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialDescription')">
      <a-form-item :label="pi.queryLabel('materialDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.materialDescription"
          :placeholder="pi.queryPh('materialDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('usageQuantity')">
      <a-form-item :label="pi.queryLabel('usageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.usageQuantity"
          :placeholder="pi.queryPh('usageQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialUnit')">
      <a-form-item :label="pi.queryLabel('materialUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('materialUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('scrapRate')">
      <a-form-item :label="pi.queryLabel('scrapRate')">
        <a-input-number
          v-model:value="advancedQueryForm.scrapRate"
          :placeholder="pi.queryPh('scrapRate', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('actualUsageQuantity')">
      <a-form-item :label="pi.queryLabel('actualUsageQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.actualUsageQuantity"
          :placeholder="pi.queryPh('actualUsageQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('operationSeq')">
      <a-form-item :label="pi.queryLabel('operationSeq')">
        <a-input-number
          v-model:value="advancedQueryForm.operationSeq"
          :placeholder="pi.queryPh('operationSeq', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workCenter')">
      <a-form-item :label="pi.queryLabel('workCenter')">
        <TaktSelect
          v-model:value="advancedQueryForm.workCenter"
          api-url="TaktWorkCenters/options"
          :placeholder="pi.queryPh('workCenter', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('position')">
      <a-form-item :label="pi.queryLabel('position')">
        <a-input
          v-model:value="advancedQueryForm.position"
          :placeholder="pi.queryPh('position', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substituteGroup')">
      <a-form-item :label="pi.queryLabel('substituteGroup')">
        <a-input
          v-model:value="advancedQueryForm.substituteGroup"
          :placeholder="pi.queryPh('substituteGroup', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('substitutePriority')">
      <a-form-item :label="pi.queryLabel('substitutePriority')">
        <a-input-number
          v-model:value="advancedQueryForm.substitutePriority"
          :placeholder="pi.queryPh('substitutePriority', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isOptional')">
      <a-form-item :label="pi.queryLabel('isOptional')">
        <TaktSelect
          v-model:value="advancedQueryForm.isOptional"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isOptional', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPhantom')">
      <a-form-item :label="pi.queryLabel('isPhantom')">
        <TaktSelect
          v-model:value="advancedQueryForm.isPhantom"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isPhantom', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isObsolete')">
      <a-form-item :label="pi.queryLabel('isObsolete')">
        <TaktSelect
          v-model:value="advancedQueryForm.isObsolete"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isObsolete', 'select')"
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
        :entity-i18n-key="BILLOFMATERIALITEM_SELF_I18N_KEY"
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
      :id-column-key="'billOfMaterialItemId'"
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
 * Takt物料清单明细实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import BillOfMaterialItemForm from './components/BillOfMaterialItemForm.vue'
import BillOfMaterialSubstitutePanel from './components/BillOfMaterialSubstitutePanel.vue'
import { provideBillOfMaterialItemMasterContext, type BillOfMaterialItemRowRecord } from './composables/useBillOfMaterialItemMasterContext'
import { getBillOfMaterialItemList, getBillOfMaterialItemById, createBillOfMaterialItem, updateBillOfMaterialItem, deleteBillOfMaterialItemById, deleteBillOfMaterialItemBatch, getBillOfMaterialItemTemplate, importBillOfMaterialItem, exportBillOfMaterialItem } from '@/api/logistics/manufacturing/bom/billOfMaterialItem.api'
import type { BillOfMaterialItem, BillOfMaterialItemQuery } from '@/types/logistics/manufacturing/bom/billOfMaterialItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useBillOfMaterialItemI18n,
  BILLOFMATERIALITEM_LIST_FIELDS,
  BILLOFMATERIALITEM_QUERY_STRING_FIELDS,
  BILLOFMATERIALITEM_QUERY_FIELDS,
  BILLOFMATERIALITEM_SELF_I18N_KEY,
} from './composables/useBillOfMaterialItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useBillOfMaterialItemI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktBillOfMaterialItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<BillOfMaterialItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<BillOfMaterialItemRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<BillOfMaterialItemRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<BillOfMaterialItem> | null>(null)
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
  for (const key of BILLOFMATERIALITEM_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    return true
  }
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    return true
  }
  if (form.scrapRate !== undefined && form.scrapRate !== null) {
    return true
  }
  if (form.actualUsageQuantity !== undefined && form.actualUsageQuantity !== null) {
    return true
  }
  if (form.operationSeq !== undefined && form.operationSeq !== null) {
    return true
  }
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    return true
  }
  if (form.isOptional !== undefined && form.isOptional !== null) {
    return true
  }
  if (form.isPhantom !== undefined && form.isPhantom !== null) {
    return true
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(BILLOFMATERIALITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof BILLOFMATERIALITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    lineNumber: undefined as number | undefined,
    usageQuantity: undefined as number | undefined,
    scrapRate: undefined as number | undefined,
    actualUsageQuantity: undefined as number | undefined,
    operationSeq: undefined as number | undefined,
    substitutePriority: undefined as number | undefined,
    isOptional: undefined as number | undefined,
    isPhantom: undefined as number | undefined,
    isObsolete: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  BILLOFMATERIALITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'billOfMaterialItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideBillOfMaterialItemMasterContext()
const billOfMaterialSubstitutePanelRef = ref<InstanceType<typeof BillOfMaterialSubstitutePanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {BillOfMaterialItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<BillOfMaterialItemQuery>): BillOfMaterialItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: BillOfMaterialItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof BillOfMaterialItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of BILLOFMATERIALITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.lineNumber !== undefined && form.lineNumber !== null) {
    query.lineNumber = form.lineNumber
  }
  if (form.usageQuantity !== undefined && form.usageQuantity !== null) {
    query.usageQuantity = form.usageQuantity
  }
  if (form.scrapRate !== undefined && form.scrapRate !== null) {
    query.scrapRate = form.scrapRate
  }
  if (form.actualUsageQuantity !== undefined && form.actualUsageQuantity !== null) {
    query.actualUsageQuantity = form.actualUsageQuantity
  }
  if (form.operationSeq !== undefined && form.operationSeq !== null) {
    query.operationSeq = form.operationSeq
  }
  if (form.substitutePriority !== undefined && form.substitutePriority !== null) {
    query.substitutePriority = form.substitutePriority
  }
  if (form.isOptional !== undefined && form.isOptional !== null) {
    query.isOptional = form.isOptional
  }
  if (form.isPhantom !== undefined && form.isPhantom !== null) {
    query.isPhantom = form.isPhantom
  }
  if (form.isObsolete !== undefined && form.isObsolete !== null) {
    query.isObsolete = form.isObsolete
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
function syncMasterSelection(record: BillOfMaterialItemRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getBillOfMaterialItemId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as BillOfMaterialItemRowRecord
  const key = getBillOfMaterialItemId(row)
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
async function loadBillOfMaterialItemDetail(record: BillOfMaterialItemRowRecord): Promise<BillOfMaterialItem | null> {
  const id = getBillOfMaterialItemId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getBillOfMaterialItemById(id)
    const index = dataSource.value.findIndex((row) => getBillOfMaterialItemId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as BillOfMaterialItem
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
    dataIndex: 'billOfMaterialItemId',
    key: 'billOfMaterialItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'billOfMaterialItemId') ?? ''
  },
  {
    title: pi.label('billOfMaterialId'),
    dataIndex: 'billOfMaterialId',
    key: 'billOfMaterialId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'billOfMaterialId') ?? ''
  },
  {
    title: pi.label('bomCode'),
    dataIndex: 'bomCode',
    key: 'bomCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'bomCode') ?? ''
  },
  {
    title: pi.label('lineNumber'),
    dataIndex: 'lineNumber',
    key: 'lineNumber',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'lineNumber') ?? ''
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'materialCode') ?? ''
  },
  {
    title: pi.label('materialDescription'),
    dataIndex: 'materialDescription',
    key: 'materialDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'materialDescription') ?? ''
  },
  {
    title: pi.label('usageQuantity'),
    dataIndex: 'usageQuantity',
    key: 'usageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'usageQuantity') ?? ''
  },
  {
    title: pi.label('materialUnit'),
    dataIndex: 'materialUnit',
    key: 'materialUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('scrapRate'),
    dataIndex: 'scrapRate',
    key: 'scrapRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'scrapRate') ?? ''
  },
  {
    title: pi.label('actualUsageQuantity'),
    dataIndex: 'actualUsageQuantity',
    key: 'actualUsageQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'actualUsageQuantity') ?? ''
  },
  {
    title: pi.label('operationSeq'),
    dataIndex: 'operationSeq',
    key: 'operationSeq',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'operationSeq') ?? ''
  },
  {
    title: pi.label('workCenter'),
    dataIndex: 'workCenter',
    key: 'workCenter',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'workCenter') ?? ''
  },
  {
    title: pi.label('position'),
    dataIndex: 'position',
    key: 'position',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'position') ?? ''
  },
  {
    title: pi.label('substituteGroup'),
    dataIndex: 'substituteGroup',
    key: 'substituteGroup',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'substituteGroup') ?? ''
  },
  {
    title: pi.label('substitutePriority'),
    dataIndex: 'substitutePriority',
    key: 'substitutePriority',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getBillOfMaterialItemField(record, 'substitutePriority') ?? ''
  },
  {
    title: pi.label('isOptional'),
    dataIndex: 'isOptional',
    key: 'isOptional',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isPhantom'),
    dataIndex: 'isPhantom',
    key: 'isPhantom',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
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
        permission: 'logistics:manufacturing:bom:bill:of:material:update',
        onClick: (record: BillOfMaterialItemRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:bom:bill:of:material:delete',
        onClick: (record: BillOfMaterialItemRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getBillOfMaterialItemId = (record: BillOfMaterialItemRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getBillOfMaterialItemField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getBillOfMaterialItemDictValue = (
  record: BillOfMaterialItemRowRecord,
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
  onChange: (keys: (string | number)[], rows: BillOfMaterialItemRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: BillOfMaterialItemRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getBillOfMaterialItemId(selectedRow.value) === getBillOfMaterialItemId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: BillOfMaterialItemRowRecord[]) => {
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
    const res = await getBillOfMaterialItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[BillOfMaterialItem] 加载数据失败', { error })
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
  billOfMaterialId: '',
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
  isObsolete: undefined as number | undefined,
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
async function handleEdit(record: BillOfMaterialItemRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadBillOfMaterialItemDetail(record)
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
      await updateBillOfMaterialItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createBillOfMaterialItem(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  billOfMaterialSubstitutePanelRef.value?.reload?.()
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
  const res = await getBillOfMaterialItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importBillOfMaterialItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    billOfMaterialSubstitutePanelRef.value?.reload?.()
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
    const exportMeta = await exportBillOfMaterialItem(
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
    logger.error('[BillOfMaterialItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: BillOfMaterialItemRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteBillOfMaterialItemById((record as any)[entityIdName])
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
      await deleteBillOfMaterialItemBatch(ids)
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
  billOfMaterialId: '',
  bomCode: '',
  lineNumber: undefined as number | undefined,
  materialCode: '',
  materialDescription: '',
  usageQuantity: undefined as number | undefined,
  materialUnit: '',
  scrapRate: undefined as number | undefined,
  actualUsageQuantity: undefined as number | undefined,
  operationSeq: undefined as number | undefined,
  workCenter: '',
  position: '',
  substituteGroup: '',
  substitutePriority: undefined as number | undefined,
  isOptional: undefined as number | undefined,
  isPhantom: undefined as number | undefined,
  isObsolete: undefined as number | undefined,
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

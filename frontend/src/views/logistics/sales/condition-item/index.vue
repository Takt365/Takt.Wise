<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/sales/condition-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt销售条件行实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
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
      :master-row-key="getSalesConditionItemId"
      :master-row-selection="rowSelection"
      master-id-column-key="salesConditionItemId"
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
      create-permission="logistics:sales:condition:create"
      update-permission="logistics:sales:condition:update"
      delete-permission="logistics:sales:condition:delete"
      import-permission="logistics:sales:condition:import"
      export-permission="logistics:sales:condition:export"
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
        <template v-if="column.key === 'priceType'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'priceType')"
            dict-type="logistics_procurement_price_type"
          />
        </template>
        <template v-else-if="column.key === 'scaleType'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'scaleType')"
            dict-type="logistics_procurement_scale"
          />
        </template>
        <template v-else-if="column.key === 'scaleBasis'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'scaleBasis')"
            dict-type="logistics_procurement_scale_basis"
          />
        </template>
        <template v-else-if="column.key === 'scaleUnit'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'scaleUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'scaleCurrencyCode'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'scaleCurrencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'calculationType'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'calculationType')"
            dict-type="logistics_procurement_calculation_type"
          />
        </template>
        <template v-else-if="column.key === 'conditionCurrencyCode'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'conditionCurrencyCode')"
            dict-type="accounting_financial_currency_code"
          />
        </template>
        <template v-else-if="column.key === 'priceUnit'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'priceUnit')"
            dict-type="logistics_materials_price_unit_param"
          />
        </template>
        <template v-else-if="column.key === 'unitOfMeasure'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'unitOfMeasure')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'isObsolete'">
          <TaktDictTag
            :value="getSalesConditionItemDictValue(record, 'isObsolete')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>
      <template #detail>
        <SalesConditionScaleQuantityPanel
          ref="salesConditionScaleQuantityPanelRef"
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
      <SalesConditionItemForm
        :key="formData?.salesConditionItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-sales-price-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">

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
        :entity-i18n-key="SALESPRICEITEM_SELF_I18N_KEY"
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
      :id-column-key="'salesConditionItemId'"
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
 * Takt销售条件行实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/sales/condition-item
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SalesConditionItemForm from './components/ConditionItemForm.vue'
import SalesConditionScaleQuantityPanel from './components/ConditionScaleQuantityPanel.vue'
import { provideSalesConditionItemMasterContext, type SalesConditionItemRowRecord } from './composables/useConditionItemMasterContext'
import { getSalesConditionItemList, getSalesConditionItemById, createSalesConditionItem, updateSalesConditionItem, deleteSalesConditionItemById, deleteSalesConditionItemBatch, getSalesConditionItemTemplate, importSalesConditionItem, exportSalesConditionItem } from '@/api/logistics/sales/conditionItem.api'
import type { SalesConditionItem, SalesConditionItemQuery } from '@/types/logistics/sales/conditionItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

import {
  useSalesConditionItemI18n,
  SALESPRICEITEM_LIST_FIELDS,
  SALESPRICEITEM_QUERY_STRING_FIELDS,
  SALESPRICEITEM_QUERY_FIELDS,
  SALESPRICEITEM_SELF_I18N_KEY,
} from './composables/useConditionItemI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useSalesConditionItemI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalesConditionItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalesConditionItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalesConditionItemRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalesConditionItemRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalesConditionItem> | null>(null)
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
  for (const key of SALESPRICEITEM_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }

  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(SALESPRICEITEM_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof SALESPRICEITEM_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  SALESPRICEITEM_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'salesConditionItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selectedMasterRow } = provideSalesConditionItemMasterContext()
const salesConditionScaleQuantityPanelRef = ref<InstanceType<typeof SalesConditionScaleQuantityPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SalesConditionItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalesConditionItemQuery>): SalesConditionItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalesConditionItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalesConditionItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of SALESPRICEITEM_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
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
function syncMasterSelection(record: SalesConditionItemRowRecord | null) {
  selectedMasterRow.value = record
  selectedMasterKey.value = record ? getSalesConditionItemId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as SalesConditionItemRowRecord
  const key = getSalesConditionItemId(row)
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
async function loadSalesConditionItemDetail(record: SalesConditionItemRowRecord): Promise<SalesConditionItem | null> {
  const id = getSalesConditionItemId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSalesConditionItemById(id)
    const index = dataSource.value.findIndex((row) => getSalesConditionItemId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SalesConditionItem
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
    dataIndex: 'salesConditionItemId',
    key: 'salesConditionItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'salesConditionItemId') ?? ''
  },
  {
    title: pi.label('salesConditionCode'),
    dataIndex: 'salesConditionCode',
    key: 'salesConditionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'salesConditionCode') ?? ''
  },
  {
    title: pi.label('salesConditionSeq'),
    dataIndex: 'salesConditionSeq',
    key: 'salesConditionSeq',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'salesConditionSeq') ?? ''
  },
  {
    title: pi.label('priceType'),
    dataIndex: 'priceType',
    key: 'priceType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('scaleType'),
    dataIndex: 'scaleType',
    key: 'scaleType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('scaleBasis'),
    dataIndex: 'scaleBasis',
    key: 'scaleBasis',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('scaleQuantity'),
    dataIndex: 'scaleQuantity',
    key: 'scaleQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'scaleQuantity') ?? ''
  },
  {
    title: pi.label('scaleUnit'),
    dataIndex: 'scaleUnit',
    key: 'scaleUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('scaleValue'),
    dataIndex: 'scaleValue',
    key: 'scaleValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'scaleValue') ?? ''
  },
  {
    title: pi.label('scaleCurrencyCode'),
    dataIndex: 'scaleCurrencyCode',
    key: 'scaleCurrencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('calculationType'),
    dataIndex: 'calculationType',
    key: 'calculationType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('price'),
    dataIndex: 'price',
    key: 'price',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'price') ?? ''
  },
  {
    title: pi.label('untaxedPrice'),
    dataIndex: 'untaxedPrice',
    key: 'untaxedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'untaxedPrice') ?? ''
  },
  {
    title: pi.label('taxIncludedPrice'),
    dataIndex: 'taxIncludedPrice',
    key: 'taxIncludedPrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'taxIncludedPrice') ?? ''
  },
  {
    title: pi.label('taxAmount'),
    dataIndex: 'taxAmount',
    key: 'taxAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'taxAmount') ?? ''
  },
  {
    title: pi.label('conditionCurrencyCode'),
    dataIndex: 'conditionCurrencyCode',
    key: 'conditionCurrencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('priceUnit'),
    dataIndex: 'priceUnit',
    key: 'priceUnit',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('unitOfMeasure'),
    dataIndex: 'unitOfMeasure',
    key: 'unitOfMeasure',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('minOrderQuantity'),
    dataIndex: 'minOrderQuantity',
    key: 'minOrderQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'minOrderQuantity') ?? ''
  },
  {
    title: pi.label('roundingValue'),
    dataIndex: 'roundingValue',
    key: 'roundingValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'roundingValue') ?? ''
  },
  {
    title: pi.label('plannedDeliveryTimeDays'),
    dataIndex: 'plannedDeliveryTimeDays',
    key: 'plannedDeliveryTimeDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'plannedDeliveryTimeDays') ?? ''
  },
  {
    title: pi.label('isObsolete'),
    dataIndex: 'isObsolete',
    key: 'isObsolete',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('remark'),
    dataIndex: 'remark',
    key: 'remark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalesConditionItemField(record, 'remark') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:sales:condition:update',
        onClick: (record: SalesConditionItemRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:sales:condition:delete',
        onClick: (record: SalesConditionItemRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalesConditionItemId = (record: SalesConditionItemRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesConditionItemField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getSalesConditionItemDictValue = (
  record: SalesConditionItemRowRecord,
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
  onChange: (keys: (string | number)[], rows: SalesConditionItemRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: SalesConditionItemRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && getSalesConditionItemId(selectedRow.value) === getSalesConditionItemId(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalesConditionItemRowRecord[]) => {
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
    const res = await getSalesConditionItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalesConditionItem] 加载数据失败', { error })
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
async function handleEdit(record: SalesConditionItemRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadSalesConditionItemDetail(record)
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
      await updateSalesConditionItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createSalesConditionItem(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    if (selectedMasterKey.value) {
  salesConditionScaleQuantityPanelRef.value?.reload?.()
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
  const res = await getSalesConditionItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importSalesConditionItem(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selectedMasterKey.value) {
    salesConditionScaleQuantityPanelRef.value?.reload?.()
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
    const exportMeta = await exportSalesConditionItem(
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
    logger.error('[SalesConditionItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalesConditionItemRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalesConditionItemById((record as any)[entityIdName])
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
      await deleteSalesConditionItemBatch(ids)
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

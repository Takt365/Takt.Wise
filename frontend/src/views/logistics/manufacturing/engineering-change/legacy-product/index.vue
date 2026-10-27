<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/legacy-product -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：设变旧品管制列表页，含查询、编辑、导出（无新增/删除/导入） -->
<!-- 版权信息：Copyright (c) 2026 Takt All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="ec-list-layout p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏：仅编辑 / 导出，其余壳与标准 CRUD 一致 -->
    <TaktToolsBar
      update-permission="logistics:manufacturing:engineering:change:legacy:product:update"
      export-permission="logistics:manufacturing:engineering:change:legacy:product:export"
      :show-create="false"
      :show-update="true"
      :show-delete="false"
      :show-import="false"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :refresh-loading="loading"
      @update="handleUpdate"
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
      :id-column-key="'ecDetailId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getEcLegacyProductId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <template #bodyCell="slotData">
        <template v-if="isLegacyProductColumn(slotData.column, 'discontinuedStatus')">
          <TaktDictTag
            dict-type="logistics_materials_material_discontinued_status"
            :value="legacyProductDictCellValue(slotData.record, 'discontinuedStatus', slotData.text)"
          />
        </template>
        <template v-else-if="isLegacyProductColumn(slotData.column, 'ec2ndVendor')">
          <TaktDictTag
            dict-type="logistics_manufacturing_ec_2nd_vendor"
            :value="legacyProductDictCellValue(slotData.record, 'ec2ndVendor', slotData.text)"
          />
        </template>
        <template v-else-if="isLegacyProductColumn(slotData.column, 'ecInstruction')">
          <TaktDictTag
            dict-type="logistics_manufacturing_ec_source_instruction"
            :value="legacyProductDictCellValue(slotData.record, 'ecInstruction', slotData.text)"
          />
        </template>
        <template v-else-if="isLegacyProductColumn(slotData.column, 'oldProductHandling')">
          <TaktDictTag
            dict-type="logistics_manufacturing_ec_old_part_disposition"
            :value="legacyProductDictCellValue(slotData.record, 'oldProductHandling', slotData.text)"
          />
        </template>
        <template v-else-if="isLegacyProductColumn(slotData.column, 'ecOldPartDisposition')">
          <TaktDictTag
            dict-type="logistics_manufacturing_ec_old_part_disposition"
            :value="legacyProductDictCellValue(slotData.record, 'ecOldPartDisposition', slotData.text)"
          />
        </template>
        <TaktTableBodyCellFallback
          v-else
          :slot-data="slotData"
        />
      </template>
    </TaktSingleTable>

    <!-- 分页 -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <LegacyProductForm
        :key="formData?.ecDetailId ?? 'edit'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-engineering-change-legacy-product'"
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
        <div v-show="isFieldVisible('ecCode')">
          <a-form-item :label="pi.queryLabel('ecCode')">
            <a-input
              v-model:value="advancedQueryForm.ecCode"
              :placeholder="pi.queryPh('ecCode', 'required')"
              show-count
              :maxlength="10"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecModelCode')">
          <a-form-item :label="pi.queryLabel('ecModelCode')">
            <a-input
              v-model:value="advancedQueryForm.ecModelCode"
              :placeholder="pi.queryPh('ecModelCode', 'required')"
              show-count
              :maxlength="40"
              allow-clear
            />
          </a-form-item>
        </div>
        <div v-show="isFieldVisible('ecOldMaterialCode')">
          <a-form-item :label="pi.queryLabel('ecOldMaterialCode')">
            <a-input
              v-model:value="advancedQueryForm.ecOldMaterialCode"
              :placeholder="pi.queryPh('ecOldMaterialCode', 'required')"
              show-count
              :maxlength="20"
              allow-clear
            />
          </a-form-item>
        </div>
      </template>
    </TaktQueryDrawer>

    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'ecDetailId'"
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
 * 设变旧品管制列表页（编辑 + 导出）
 * @module views/logistics/manufacturing/engineering-change/legacy-product
 */
import '../ec-list-layout.css'
import { message } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import TaktTableBodyCellFallback from '@/components/business/TaktTableBodyCellFallback/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import LegacyProductForm from './components/LegacyProductForm.vue'
import {
  getEcLegacyProductList,
  getEcLegacyProductByEcDetailId,
  updateEcLegacyProduct,
  exportEcLegacyProduct,
} from '@/api/logistics/manufacturing/engineering-change/legacyProduct.api'
import type { EcLegacyProduct, EcLegacyProductQuery } from '@/types/logistics/manufacturing/engineering-change/legacyProduct.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine } from '@remixicon/vue'
import {
  useEcLegacyProductI18n,
  ECLEGACYPRODUCT_LIST_FIELDS,
  ECLEGACYPRODUCT_DEFAULT_VISIBLE_COLUMN_KEYS,
  ECLEGACYPRODUCT_QUERY_STRING_FIELDS,
  ECLEGACYPRODUCT_QUERY_FIELDS,
} from './composables/useEcLegacyProductI18n'

/** 实体字段 i18n */
const pi = useEcLegacyProductI18n()
/** 表格行类型 */
type EcLegacyProductRowRecord = EcLegacyProduct | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktEcLegacyProduct')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<EcLegacyProduct[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<EcLegacyProductRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<EcLegacyProductRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])
/** 编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题 */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<EcLegacyProduct> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)

/**
 * 创建空的高级查询表单
 * @returns 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  return Object.fromEntries(ECLEGACYPRODUCT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ECLEGACYPRODUCT_QUERY_STRING_FIELDS)[number],
    string
  >
}

/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  ECLEGACYPRODUCT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...ECLEGACYPRODUCT_DEFAULT_VISIBLE_COLUMN_KEYS])
/** 实体主键字段名（row-key、详情路径） */
const entityIdName = 'ecDetailId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {EcLegacyProductQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<EcLegacyProductQuery>): EcLegacyProductQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: EcLegacyProductQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof EcLegacyProductQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of ECLEGACYPRODUCT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  return query
}

/** 页面挂载：分页配置 + 字典 + 列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildEcLegacyProductListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义 */
const columns = computed<TableColumnsType>(() => [
  buildEcLegacyProductListColumn('ecDetailId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...ECLEGACYPRODUCT_LIST_FIELDS.map((key) => buildEcLegacyProductListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:engineering:change:legacy:product:update',
        onClick: (record: EcLegacyProductRowRecord) => handleEdit(record),
      },
    ],
  }),
])

/**
 * 表格 row-key
 * @param record 行数据
 * @returns {string} 明细 ID
 */
const getEcLegacyProductId = (record: EcLegacyProductRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}

/**
 * 读取行字段
 * @param record 行数据
 * @param field 字段名
 */
const getEcLegacyProductField = (record: EcLegacyProductRowRecord, field: string): unknown => {
  const row = record as Record<string, unknown>
  if (row[field] != null && row[field] !== '') {
    return row[field]
  }
  const pascal = field.charAt(0).toUpperCase() + field.slice(1)
  return row[pascal]
}

/**
 * 字典列单元格值：优先 Ant Design 已解析的 text，其次行字段（含 PascalCase）
 * @param record 行数据
 * @param field 字段名
 * @param text bodyCell text
 * @returns {string} 字典 DictValue
 */
function legacyProductDictCellValue(
  record: EcLegacyProductRowRecord,
  field: string,
  text: unknown,
): string {
  if (text != null && text !== '') {
    return String(text)
  }
  const raw = getEcLegacyProductField(record, field)
  return raw != null && raw !== '' ? String(raw) : ''
}

/**
 * 判断 bodyCell 列是否为目标字段（Ant Design 可能给 key 或 dataIndex）
 * @param column 表格列
 * @param field 字段名
 * @returns {boolean} 是否匹配
 */
function isLegacyProductColumn(
  column: { key?: string | number; dataIndex?: string | number | readonly (string | number)[] },
  field: string,
): boolean {
  const key = column.key != null ? String(column.key) : ''
  const dataIndex = Array.isArray(column.dataIndex)
    ? String(column.dataIndex[0] ?? '')
    : column.dataIndex != null
      ? String(column.dataIndex)
      : ''
  return key === field || dataIndex === field
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: EcLegacyProductRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: EcLegacyProductRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getEcLegacyProductId(selecmedRow.value) === getEcLegacyProductId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: EcLegacyProductRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击切换选中
 * @param record 当前行
 */
const onClickRow = (record: EcLegacyProductRowRecord) => ({
  onClick: () => {
    const key = getEcLegacyProductId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getEcLegacyProductId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  },
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getEcLegacyProductList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    logger.error('[EcLegacyProduct] 加载数据失败', { error })
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时重载列表 */
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

/**
 * 打开编辑弹窗（拉取详情）
 * @param record 当前行
 */
async function handleEdit(record: EcLegacyProductRowRecord) {
  const id = getEcLegacyProductId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getEcLegacyProductByEcDetailId(id)
    formData.value = detail ?? ({ ...record } as Partial<EcLegacyProduct>)
    formVisible.value = true
  } catch {
    message.error(t('common.feedback.load.data.failed'))
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

/** 提交编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) {
    return
  }
  try {
    await refInst.validate()
  } catch {
    return
  }
  const id = formData.value?.ecDetailId
  if (!id) {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.()
    await updateEcLegacyProduct(String(id), payload)
    message.success(t('common.feedback.updated', { target: pi.self() }))
    formVisible.value = false
    formData.value = null
    nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭编辑弹窗 */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportEcLegacyProduct(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase,
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob as Blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    logger.error('[EcLegacyProduct] 导出失败', { error })
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 高级查询重置 */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/**
 * 列设置：更新可见列 key
 * @param keys 可见列
 */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = [...ECLEGACYPRODUCT_DEFAULT_VISIBLE_COLUMN_KEYS]
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}

/** 列宽拖拽回调占位 */
function handleResizeColumn() {}

/**
 * 分页页码变更
 * @param page 页码
 * @param size 每页条数
 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/**
 * 分页每页条数变更
 * @param _current 当前页
 * @param size 每页条数
 */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

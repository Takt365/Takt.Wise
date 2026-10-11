<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/statistics/quick-query/configurable -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：定制报表主实体管理页面，含查询、增删改、设计器与运行时 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="statistics-quick-query-configurable">
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
      create-permission="statistics:quickquery:configurable:create"
      update-permission="statistics:quickquery:configurable:update"
      delete-permission="statistics:quickquery:configurable:delete"
      import-permission="statistics:quickquery:configurable:import"
      export-permission="statistics:quickquery:configurable:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
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
      :columns="columns"
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'configurableId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getConfigurableId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'configurableStatus'">
          <TaktDictTag
            :value="getConfigurableField(record, 'configurableStatus')"
            dict-type="sys_normal_disable"
          />
        </template>
        <template v-else-if="column.key === 'configurableDomain'">
          {{ resolveDomainLabel(getConfigurableField(record, 'configurableDomain') as number | undefined) }}
        </template>
        <template v-else-if="column.key === 'isPublic'">
          <TaktDictTag
            :value="getConfigurableField(record, 'isPublic')"
            dict-type="sys_public_type"
          />
        </template>
      </template>
    </TaktSingleTable>

    <!-- 分页组件 -->
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
      width="85%"
      :use-viewport-size="true"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <ConfigurableDesigner
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
      :storage-key="'takt-query-fields-statistics-quick-query-configurable'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('configurableCode')">
      <a-form-item :label="t('entity.configurable.code')">
        <a-input
          v-model:value="advancedQueryForm.configurableCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.code') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableName')">
      <a-form-item :label="t('entity.configurable.name')">
        <a-input
          v-model:value="advancedQueryForm.configurableName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.name') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableDomain')">
      <a-form-item :label="t('entity.configurable.domain')">
        <a-select
          v-model:value="advancedQueryForm.configurableDomain"
          :options="moduleOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.domain') })"
          allow-clear
          size="small"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableSubCategory')">
      <a-form-item :label="t('entity.configurable.subcategory')">
        <a-select
          v-model:value="advancedQueryForm.configurableSubCategory"
          :options="subCategoryOptions"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.subcategory') })"
          :disabled="!advancedQueryForm.configurableDomain || subCategoryOptions.length === 0"
          allow-clear
          size="small"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('distinctRows')">
      <a-form-item :label="t('entity.configurable.distinctrows')">
        <a-input-number
          v-model:value="advancedQueryForm.distinctRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.distinctrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxExportRows')">
      <a-form-item :label="t('entity.configurable.maxexportrows')">
        <a-input-number
          v-model:value="advancedQueryForm.maxExportRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxexportrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maxQueryRows')">
      <a-form-item :label="t('entity.configurable.maxqueryrows')">
        <a-input-number
          v-model:value="advancedQueryForm.maxQueryRows"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.maxqueryrows') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPublic')">
      <a-form-item :label="t('entity.configurable.ispublic')">
        <TaktSelect
          v-model:value="advancedQueryForm.isPublic"
          dict-type="sys_public_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.ispublic') })"
          allow-clear
          size="small"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sortOrder')">
      <a-form-item :label="t('entity.configurable.sortorder')">
        <a-input-number
          v-model:value="advancedQueryForm.sortOrder"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.configurable.sortorder') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableStatus')">
      <a-form-item :label="t('entity.configurable.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.configurableStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.configurable.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableDescription')">
      <a-form-item :label="t('entity.configurable.description')">
        <a-textarea
          v-model:value="advancedQueryForm.configurableDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.configurable.description') })"
          :rows="2"
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
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.configurable._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.configurable._self"
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
      :id-column-key="'configurableId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />

  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 定制报表主实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/statistics/quick-query/configurable
 */
import { useRouter } from 'vue-router'
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import ConfigurableDesigner from './components/ConfigurableDesigner.vue'
import { getConfigurableList, getConfigurableById, createConfigurable, updateConfigurable, deleteConfigurableById, deleteConfigurableBatch, getConfigurableTemplate, importConfigurable, exportConfigurable } from '@/api/statistics/quick-query/configurable.api'
import type { Configurable, ConfigurableQuery } from '@/types/statistics/quick-query/configurable.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiPlayLine } from '@remixicon/vue'
import { useConfigurableCategory } from './composables/useConfigurableCategory'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktConfigurable')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.configurable._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Configurable[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Configurable | null>(null)
/** 表格多选行 */
const selectedRows = ref<Configurable[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Configurable>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  configurableCode: '',
  configurableName: '',
  configurableDomain: undefined as number | undefined,
  configurableSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  configurableStatus: undefined as number | undefined,
  configurableDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  remark: '',
})
/** 高级查询：业务域 / 子分类菜单联动 */
const advancedConfigurableDomain = computed({
  get: () => advancedQueryForm.value.configurableDomain,
  set: (value: number | undefined) => {
    advancedQueryForm.value.configurableDomain = value
  },
})
const advancedConfigurableSubCategory = computed({
  get: () => advancedQueryForm.value.configurableSubCategory,
  set: (value: string) => {
    advancedQueryForm.value.configurableSubCategory = value
  },
})
const { moduleOptions, subCategoryOptions, resolveSubCategoryLabel, resolveDomainLabel } = useConfigurableCategory(
  advancedConfigurableDomain,
  advancedConfigurableSubCategory
)
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'configurableCode', label: t('entity.configurable.code') },
  { key: 'configurableName', label: t('entity.configurable.name') },
  { key: 'configurableDomain', label: t('entity.configurable.domain') },
  { key: 'configurableSubCategory', label: t('entity.configurable.subcategory') },
  { key: 'distinctRows', label: t('entity.configurable.distinctrows') },
  { key: 'maxExportRows', label: t('entity.configurable.maxexportrows') },
  { key: 'maxQueryRows', label: t('entity.configurable.maxqueryrows') },
  { key: 'isPublic', label: t('entity.configurable.ispublic') },
  { key: 'sortOrder', label: t('entity.configurable.sortorder') },
  { key: 'configurableStatus', label: t('entity.configurable.status') },
  { key: 'configurableDescription', label: t('entity.configurable.description') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
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
const entityIdName = 'configurableId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)
/** 路由导航（跳转执行页） */
const router = useRouter()

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 加载主表详情并回填当前页 dataSource */
async function loadConfigurableDetail(record: Configurable): Promise<Configurable | null> {
  const id = getConfigurableId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getConfigurableById(id)
    const index = dataSource.value.findIndex((row) => getConfigurableId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as Configurable
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
    dataIndex: 'configurableId',
    key: 'configurableId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'configurableId') ?? ''
  },
  {
    title: t('entity.configurable.code'),
    dataIndex: 'configurableCode',
    key: 'configurableCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'configurableCode') ?? ''
  },
  {
    title: t('entity.configurable.name'),
    dataIndex: 'configurableName',
    key: 'configurableName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'configurableName') ?? ''
  },
  {
    title: t('entity.configurable.domain'),
    dataIndex: 'configurableDomain',
    key: 'configurableDomain',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.configurable.subcategory'),
    dataIndex: 'configurableSubCategory',
    key: 'configurableSubCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) =>
      resolveSubCategoryLabel(
        getConfigurableField(record, 'configurableDomain') as number | undefined,
        getConfigurableField(record, 'configurableSubCategory') as string | undefined
      )
  },
  {
    title: t('entity.configurable.distinctrows'),
    dataIndex: 'distinctRows',
    key: 'distinctRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'distinctRows') ?? ''
  },
  {
    title: t('entity.configurable.maxexportrows'),
    dataIndex: 'maxExportRows',
    key: 'maxExportRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'maxExportRows') ?? ''
  },
  {
    title: t('entity.configurable.maxqueryrows'),
    dataIndex: 'maxQueryRows',
    key: 'maxQueryRows',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'maxQueryRows') ?? ''
  },
  {
    title: t('entity.configurable.ispublic'),
    dataIndex: 'isPublic',
    key: 'isPublic',
    width: 100,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.configurable.status'),
    dataIndex: 'configurableStatus',
    key: 'configurableStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.configurable.description'),
    dataIndex: 'configurableDescription',
    key: 'configurableDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getConfigurableField(record, 'configurableDescription') ?? ''
  },
  CreateActionColumn({
    width: 200,
    actions: [
      {
        key: 'run',
        label: t('statistics.quickquery.configurable.page.run'),
        shape: 'plain',
        icon: RiPlayLine,
        buttonClass: 'takt-button-run',
        permission: 'statistics:quickquery:configurable:run',
        disabled: (record: Configurable) => getConfigurableField(record, 'configurableStatus') !== 1,
        onClick: (record: Configurable) => handleRunConfigurable(record),
      },
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'statistics:quickquery:configurable:update',
        onClick: (record: Configurable) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'statistics:quickquery:configurable:delete',
        onClick: (record: Configurable) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getConfigurableId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getConfigurableField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Configurable[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Configurable, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getConfigurableId(selectedRow.value) === getConfigurableId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Configurable[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Configurable) => ({
  onClick: () => {
    const key = getConfigurableId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getConfigurableId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: ConfigurableQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getConfigurableList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Configurable] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 跳转 定制报表独立执行页 */
function handleRunConfigurable(record: Configurable) {
  const id = getConfigurableId(record)
  if (!id) {
    return
  }
  const name = getConfigurableField(record, 'configurableName') ?? ''
  router.push({
    path: '/statistics/quick-query/configurable/run',
    query: {
      id,
      ...(name ? { name } : {}),
    },
  })
}

/** 快捷查询 */
function handleSearch() {
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  configurableCode: '',
  configurableName: '',
  configurableDomain: undefined as number | undefined,
  configurableSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  configurableStatus: undefined as number | undefined,
  configurableDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.configurable._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: Configurable) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.configurable._self') })
  formLoading.value = true
  try {
    const detail = await loadConfigurableDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.configurable._self') }))
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
      await updateConfigurable(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.configurable._self') }))
    } else {
      await createConfigurable(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.configurable._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formRef.value?.resetFields?.()
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getConfigurableTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importConfigurable(file, sheetName)
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: ConfigurableQuery = {
      pageIndex: 1,
      pageSize: 50000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportConfigurable(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.configurable._self') }))
  } catch (error: any) {
    logger.error('[Configurable] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.configurable._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Configurable) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.configurable._self'), name: t('common.tip.this.target', { target: t('entity.configurable._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteConfigurableById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.configurable._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.configurable._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.configurable._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteConfigurableBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.configurable._self') }))
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
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  configurableCode: '',
  configurableName: '',
  configurableDomain: undefined as number | undefined,
  configurableSubCategory: '',
  distinctRows: undefined as number | undefined,
  maxExportRows: undefined as number | undefined,
  maxQueryRows: undefined as number | undefined,
  isPublic: undefined as number | undefined,
  sortOrder: undefined as number | undefined,
  configurableStatus: undefined as number | undefined,
  configurableDescription: '',
  createdAtStart: '',
  createdAtEnd: '',
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
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.statistics-quick-query-configurable {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>

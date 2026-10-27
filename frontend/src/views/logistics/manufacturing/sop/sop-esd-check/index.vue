<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-esd-check -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：SOP ESD 检查实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-sop-sop-esd-check">
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
      create-permission="logistics:manufacturing:sop:esdcheck:create"
      update-permission="logistics:manufacturing:sop:esdcheck:update"
      delete-permission="logistics:manufacturing:sop:esdcheck:delete"
      import-permission="logistics:manufacturing:sop:esdcheck:import"
      export-permission="logistics:manufacturing:sop:esdcheck:export"
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

    <!-- 表格 -->
    <div class="logistics-manufacturing-sop-sop-esd-check-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        :columns="columns"
        entity-scope="company"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'sopEsdCheckId'"
        table-mode="single"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSopEsdCheckId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"

        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isCompliant'">
          <TaktDictTag
            :value="getSopEsdCheckField(record, 'isCompliant')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'lockScreenTriggered'">
          <TaktDictTag
            :value="getSopEsdCheckField(record, 'lockScreenTriggered')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>

      </TaktSingleTable>
    </div>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
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
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <SopEsdCheckForm
        :key="formData?.sopEsdCheckId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-sop-sop-esd-check'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('workstationId')">
      <a-form-item :label="t('entity.sopesdcheck.workstationid')">
        <a-input
          v-model:value="advancedQueryForm.workstationId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.workstationid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('execId')">
      <a-form-item :label="t('entity.sopesdcheck.execid')">
        <a-input
          v-model:value="advancedQueryForm.execId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.execid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.sopesdcheck.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deviceCode')">
      <a-form-item :label="t('entity.sopesdcheck.devicecode')">
        <a-input
          v-model:value="advancedQueryForm.deviceCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.devicecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resistanceValue')">
      <a-form-item :label="t('entity.sopesdcheck.resistancevalue')">
        <a-input-number
          v-model:value="advancedQueryForm.resistanceValue"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.resistancevalue') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCompliant')">
      <a-form-item :label="t('entity.sopesdcheck.iscompliant')">
        <TaktSelect
          v-model:value="advancedQueryForm.isCompliant"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopesdcheck.iscompliant') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lockScreenTriggered')">
      <a-form-item :label="t('entity.sopesdcheck.lockscreentriggered')">
        <TaktSelect
          v-model:value="advancedQueryForm.lockScreenTriggered"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopesdcheck.lockscreentriggered') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkedAtStart')">
      <a-form-item :label="t('entity.sopesdcheck.checkedatstart')">
        <a-input
          v-model:value="advancedQueryForm.checkedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopesdcheck.checkedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('checkedAtEnd')">
      <a-form-item :label="t('entity.sopesdcheck.checkedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.checkedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopesdcheck.checkedatend') })"
          value-format="YYYY-MM-DD"
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
      :title="t('common.dialog.title.import', { entity: t('entity.sopesdcheck._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.sopesdcheck._self"
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
      :id-column-key="'sopEsdCheckId'"
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
 * SOP ESD 检查实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/sop-esd-check
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SopEsdCheckForm from './components/SopEsdCheckForm.vue'
import { getSopEsdCheckList, getSopEsdCheckById, createSopEsdCheck, updateSopEsdCheck, deleteSopEsdCheckById, deleteSopEsdCheckBatch, getSopEsdCheckTemplate, importSopEsdCheck, exportSopEsdCheck } from '@/api/logistics/manufacturing/sop/sopEsdCheck.api'
import type { SopEsdCheck, SopEsdCheckQuery, SopEsdCheckCreate, SopEsdCheckUpdate } from '@/types/logistics/manufacturing/sop/sopEsdCheck.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSopEsdCheck')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sopesdcheck._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SopEsdCheck[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<SopEsdCheck | null>(null)
/** 表格多选行 */
const selecmedRows = ref<SopEsdCheck[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SopEsdCheck> | null>(null)
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
  workstationId: '',
  execId: '',
  employeeId: '',
  deviceCode: '',
  resistanceValue: undefined as number | undefined,
  isCompliant: undefined as number | undefined,
  lockScreenTriggered: undefined as number | undefined,
  checkedAtStart: '',
  checkedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'workstationId', label: t('entity.sopesdcheck.workstationid') },
  { key: 'execId', label: t('entity.sopesdcheck.execid') },
  { key: 'employeeId', label: t('entity.sopesdcheck.employeeid') },
  { key: 'deviceCode', label: t('entity.sopesdcheck.devicecode') },
  { key: 'resistanceValue', label: t('entity.sopesdcheck.resistancevalue') },
  { key: 'isCompliant', label: t('entity.sopesdcheck.iscompliant') },
  { key: 'lockScreenTriggered', label: t('entity.sopesdcheck.lockscreentriggered') },
  { key: 'checkedAtStart', label: t('entity.sopesdcheck.checkedatstart') },
  { key: 'checkedAtEnd', label: t('entity.sopesdcheck.checkedatend') },
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
const entityIdName = 'sopEsdCheckId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 表格 scroll.y（服务端分页固定视口高度；scroll.x 由 TaktSingleTable 按列宽累计） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const

/**
 * 构建列表/导出查询参数
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SopEsdCheckQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SopEsdCheckQuery>): SopEsdCheckQuery {
  const kw = (queryKeyword.value ?? '').trim()
  const query: SopEsdCheckQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...advancedQueryForm.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置，再拉列表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sopEsdCheckId',
    key: 'sopEsdCheckId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'sopEsdCheckId') ?? ''
  },
  {
    title: t('entity.sopesdcheck.workstationid'),
    dataIndex: 'workstationId',
    key: 'workstationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'workstationId') ?? ''
  },
  {
    title: t('entity.sopesdcheck.execid'),
    dataIndex: 'execId',
    key: 'execId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'execId') ?? ''
  },
  {
    title: t('entity.sopesdcheck.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.sopesdcheck.devicecode'),
    dataIndex: 'deviceCode',
    key: 'deviceCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'deviceCode') ?? ''
  },
  {
    title: t('entity.sopesdcheck.resistancevalue'),
    dataIndex: 'resistanceValue',
    key: 'resistanceValue',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'resistanceValue') ?? ''
  },
  {
    title: t('entity.sopesdcheck.iscompliant'),
    dataIndex: 'isCompliant',
    key: 'isCompliant',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.sopesdcheck.lockscreentriggered'),
    dataIndex: 'lockScreenTriggered',
    key: 'lockScreenTriggered',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.sopesdcheck.checkedat'),
    dataIndex: 'checkedAt',
    key: 'checkedAt',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'checkedAt') ?? ''
  },
  {
    title: t('entity.sopesdcheck.workstation'),
    dataIndex: 'workstation',
    key: 'workstation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopEsdCheckField(record, 'workstation') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:sop:esdcheck:update',
        onClick: (record: SopEsdCheck) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:sop:esdcheck:delete',
        onClick: (record: SopEsdCheck) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSopEsdCheckId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSopEsdCheckField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SopEsdCheck[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SopEsdCheck, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getSopEsdCheckId(selecmedRow.value) === getSopEsdCheckId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: SopEsdCheck[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SopEsdCheck) => ({
  onClick: () => {
    const key = getSopEsdCheckId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getSopEsdCheckId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    const res = await getSopEsdCheckList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SopEsdCheck] 加载数据失败', { error })
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
  workstationId: '',
  execId: '',
  employeeId: '',
  deviceCode: '',
  resistanceValue: undefined as number | undefined,
  isCompliant: undefined as number | undefined,
  lockScreenTriggered: undefined as number | undefined,
  checkedAtStart: '',
  checkedAtEnd: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.sopesdcheck._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: SopEsdCheck) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.sopesdcheck._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.sopesdcheck._self') }))
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
      await updateSopEsdCheck(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.sopesdcheck._self') }))
    } else {
      await createSopEsdCheck(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.sopesdcheck._self') }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
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
  const res = await getSopEsdCheckTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSopEsdCheck(file, sheetName)
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
    const exportMeta = await exportSopEsdCheck(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sopesdcheck._self') }))
  } catch (error: any) {
    logger.error('[SopEsdCheck] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.sopesdcheck._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SopEsdCheck) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.sopesdcheck._self'), name: t('common.tip.this.target', { target: t('entity.sopesdcheck._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSopEsdCheckById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.sopesdcheck._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.sopesdcheck._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.sopesdcheck._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSopEsdCheckBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.sopesdcheck._self') }))
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
  workstationId: '',
  execId: '',
  employeeId: '',
  deviceCode: '',
  resistanceValue: undefined as number | undefined,
  isCompliant: undefined as number | undefined,
  lockScreenTriggered: undefined as number | undefined,
  checkedAtStart: '',
  checkedAtEnd: '',
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
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>

<style scoped lang="css">
.logistics-manufacturing-sop-sop-esd-check {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.logistics-manufacturing-sop-sop-esd-check-table-wrap {
  flex: 1;
  min-height: 0;
  min-width: 0;
}
</style>

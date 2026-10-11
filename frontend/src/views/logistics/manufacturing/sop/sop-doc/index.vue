<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/sop/sop-doc -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：SOP 文档头实体管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="logistics-manufacturing-sop-sop-doc">
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
      create-permission="logistics:manufacturing:sop:doc:create"
      update-permission="logistics:manufacturing:sop:doc:update"
      delete-permission="logistics:manufacturing:sop:doc:delete"
      import-permission="logistics:manufacturing:sop:doc:import"
      export-permission="logistics:manufacturing:sop:doc:export"
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
    <div class="logistics-manufacturing-sop-sop-doc-table-wrap">
      <TaktSingleTable
        :scroll="tableScroll"
        :columns="columns"
        entity-scope="approval"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'sopDocId'"
        table-mode="single"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :row-key="getSopDocId"
        :row-selection="rowSelection"
        :custom-row="onClickRow"

      :expanded-row-keys="expandedRowKeys"
      @expand="handleExpand"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'sopStatus'">
          <a-switch
            :checked="getSopDocField(record, 'sopStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleSopStatusChange(record, Boolean(checked))"
          />
        </template>
      </template>
      <!-- 展开行渲染 -->
      <template #expandedRowRender="{ record }">
        <div class="p-4">
          <div class="mb-2 text-sm font-medium">{{ t('entity.soprevision._self') }}</div>
          <a-table
            v-if="hasSopRevisionRows(record)"
            :columns="sopRevisionExpandColumns"
            :data-source="getSopRevisionRows(record)"
            :row-key="(row: SopRevision, index?: number) => row?.sopRevisionId || String(index ?? 0)"
            :pagination="false"
            size="small"
            bordered
            class="mb-4"
          />
          <a-empty v-else class="mb-4" />
        </div>
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
      <SopDocForm
        :key="formData?.sopDocId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-sop-sop-doc'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('sopCode')">
      <a-form-item :label="t('entity.sopdoc.sopcode')">
        <a-input
          v-model:value="advancedQueryForm.sopCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sopName')">
      <a-form-item :label="t('entity.sopdoc.sopname')">
        <a-input
          v-model:value="advancedQueryForm.sopName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.sopname') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="t('entity.sopdoc.materialcode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.materialcode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('routingItemId')">
      <a-form-item :label="t('entity.sopdoc.routingitemid')">
        <a-input
          v-model:value="advancedQueryForm.routingItemId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.routingitemid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workstationId')">
      <a-form-item :label="t('entity.sopdoc.workstationid')">
        <a-input
          v-model:value="advancedQueryForm.workstationId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.workstationid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentRevisionId')">
      <a-form-item :label="t('entity.sopdoc.currentrevisionid')">
        <a-input
          v-model:value="advancedQueryForm.currentRevisionId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.currentrevisionid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="t('entity.sopdoc.culturecode')">
        <a-input
          v-model:value="advancedQueryForm.cultureCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.culturecode') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sopStatus')">
      <a-form-item :label="t('entity.sopdoc.sopstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.sopStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopdoc.sopstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.sopdoc.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopdoc.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.sopdoc.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.sopdoc.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.sopdoc.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopdoc.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.sopdoc.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.sopdoc.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.sopdoc.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.sopdoc.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.sopdoc.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.sopdoc.flowinstanceid') })"
          show-count
          :maxlength="20"
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
      :title="t('common.dialog.title.import', { entity: t('entity.sopdoc._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.sopdoc._self"
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
      :id-column-key="'sopDocId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * SOP 文档头实体管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/sop/sop-doc
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SopDocForm from './components/SopDocForm.vue'
import { getSopDocList, getSopDocById, createSopDoc, updateSopDoc, deleteSopDocById, deleteSopDocBatch, getSopDocTemplate, importSopDoc, exportSopDoc, updateSopDocStatus } from '@/api/logistics/manufacturing/sop/sopDoc.api'
import type { SopRevision } from '@/types/logistics/manufacturing/sop/sopRevision.types'
import type { SopDoc, SopDocQuery, SopDocCreate, SopDocUpdate } from '@/types/logistics/manufacturing/sop/sopDoc.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSopDoc')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.sopdoc._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SopDoc[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SopDoc | null>(null)
/** 表格多选行 */
const selectedRows = ref<SopDoc[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SopDoc> | null>(null)
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
  sopCode: '',
  sopName: '',
  materialCode: '',
  routingItemId: '',
  workstationId: '',
  currentRevisionId: '',
  cultureCode: '',
  sopStatus: undefined as number | undefined,
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
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'sopCode', label: t('entity.sopdoc.sopcode') },
  { key: 'sopName', label: t('entity.sopdoc.sopname') },
  { key: 'materialCode', label: t('entity.sopdoc.materialcode') },
  { key: 'routingItemId', label: t('entity.sopdoc.routingitemid') },
  { key: 'workstationId', label: t('entity.sopdoc.workstationid') },
  { key: 'currentRevisionId', label: t('entity.sopdoc.currentrevisionid') },
  { key: 'cultureCode', label: t('entity.sopdoc.culturecode') },
  { key: 'sopStatus', label: t('entity.sopdoc.sopstatus') },
  { key: 'approvalStatus', label: t('entity.sopdoc.approvalstatus') },
  { key: 'initiatorId', label: t('entity.sopdoc.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.sopdoc.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.sopdoc.initiatedatend') },
  { key: 'approvedBy', label: t('entity.sopdoc.approvedby') },
  { key: 'approvedAtStart', label: t('entity.sopdoc.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.sopdoc.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.sopdoc.flowinstanceid') },
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
const entityIdName = 'sopDocId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主子表展开行 keys（手风琴，仅一行展开） */
const expandedRowKeys = ref<string[]>([])

/** 表格 scroll.y（服务端分页固定视口高度；scroll.x 由 TaktSingleTable 按列宽累计） */
const tableScroll = { y: 'calc(100vh - 300px)' } as const

/**
 * 构建列表/导出查询参数
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SopDocQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SopDocQuery>): SopDocQuery {
  const kw = (queryKeyword.value ?? '').trim()
  const query: SopDocQuery = {
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

/** 展开行预览：sopRevision 列 */
const sopRevisionExpandColumns = computed(() => [

])

/** 读取主表行上的 sopRevision 子表缓存 */
function getSopRevisionRows(record: SopDoc): SopRevision[] {
  return (record as any)?.revisions ?? []
}

/** 主表行是否已加载 sopRevision 子表 */
function hasSopRevisionRows(record: SopDoc): boolean {
  return getSopRevisionRows(record).length > 0
}

/** 加载主表详情并回填当前页 dataSource */
async function loadSopDocDetail(record: SopDoc): Promise<SopDoc | null> {
  const id = getSopDocId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getSopDocById(id)
    const index = dataSource.value.findIndex((row) => getSopDocId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as SopDoc
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}
/** 通过主表详情接口加载 sopRevision 子表 */
async function loadSopRevisionForSopDoc(record: SopDoc): Promise<SopRevision[]> {
  const detail = await loadSopDocDetail(record)
  return detail?.revisions ?? []
}

/** 展开前确保各子表已懒加载 */
async function ensureSopDocChildrenLoaded(record: SopDoc) {
  if (!hasSopRevisionRows(record)) {
    await loadSopRevisionForSopDoc(record)
  }
}

/** 主表展开行：手风琴懒加载子表 */
async function handleExpand(expanded: boolean, record: SopDoc) {
  const key = getSopDocId(record)
  if (!expanded || !key) {
    expandedRowKeys.value = []
    return
  }
  if (expandedRowKeys.value.length > 0 && expandedRowKeys.value[0] !== key) {
    expandedRowKeys.value = []
  }
  await ensureSopDocChildrenLoaded(record)
  expandedRowKeys.value = [key]
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'sopDocId',
    key: 'sopDocId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'sopDocId') ?? ''
  },
  {
    title: t('entity.sopdoc.sopcode'),
    dataIndex: 'sopCode',
    key: 'sopCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'sopCode') ?? ''
  },
  {
    title: t('entity.sopdoc.sopname'),
    dataIndex: 'sopName',
    key: 'sopName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'sopName') ?? ''
  },
  {
    title: t('entity.sopdoc.materialcode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'materialCode') ?? ''
  },
  {
    title: t('entity.sopdoc.routingitemid'),
    dataIndex: 'routingItemId',
    key: 'routingItemId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'routingItemId') ?? ''
  },
  {
    title: t('entity.sopdoc.workstationid'),
    dataIndex: 'workstationId',
    key: 'workstationId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'workstationId') ?? ''
  },
  {
    title: t('entity.sopdoc.currentrevisionid'),
    dataIndex: 'currentRevisionId',
    key: 'currentRevisionId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'currentRevisionId') ?? ''
  },
  {
    title: t('entity.sopdoc.culturecode'),
    dataIndex: 'cultureCode',
    key: 'cultureCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'cultureCode') ?? ''
  },
  {
    title: t('entity.sopdoc.sopstatus'),
    dataIndex: 'sopStatus',
    key: 'sopStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.sopdoc.routingitem'),
    dataIndex: 'routingItem',
    key: 'routingItem',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'routingItem') ?? ''
  },
  {
    title: t('entity.sopdoc.workstation'),
    dataIndex: 'workstation',
    key: 'workstation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSopDocField(record, 'workstation') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:sop:doc:update',
        onClick: (record: SopDoc) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:sop:doc:delete',
        onClick: (record: SopDoc) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSopDocId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSopDocField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SopDoc[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SopDoc, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (getSopDocId(selectedRow.value) === getSopDocId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SopDoc[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SopDoc) => ({
  onClick: () => {
    const key = getSopDocId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSopDocId(item)))
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
    const res = await getSopDocList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SopDoc] 加载数据失败', { error })
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
  sopCode: '',
  sopName: '',
  materialCode: '',
  routingItemId: '',
  workstationId: '',
  currentRevisionId: '',
  cultureCode: '',
  sopStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.sopdoc._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: SopDoc) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.sopdoc._self') })
  formLoading.value = true
  try {
    const detail = await loadSopDocDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.sopdoc._self') }))
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
      await updateSopDoc(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.sopdoc._self') }))
    } else {
      await createSopDoc(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.sopdoc._self') }))
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
  const res = await getSopDocTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSopDoc(file, sheetName)
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
    const exportMeta = await exportSopDoc(
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
    message.success(t('common.feedback.export.success', { target: t('entity.sopdoc._self') }))
  } catch (error: any) {
    logger.error('[SopDoc] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.sopdoc._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SopDoc) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.sopdoc._self'), name: t('common.tip.this.target', { target: t('entity.sopdoc._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSopDocById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.sopdoc._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.sopdoc._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.sopdoc._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSopDocBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.sopdoc._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleSopStatusChange(record: SopDoc, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getSopDocField(record, 'sopStatus')
  const id = getSopDocId(record)
  const row = dataSource.value.find((item) => getSopDocId(item) === id)
  if (row) {
    row.sopStatus = newVal
  }
  try {
    await updateSopDocStatus({ sopDocId: id, sopStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.sopStatus = oldVal
    }
    message.error(t('common.feedback.failed'))
  }
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
  sopCode: '',
  sopName: '',
  materialCode: '',
  routingItemId: '',
  workstationId: '',
  currentRevisionId: '',
  cultureCode: '',
  sopStatus: undefined as number | undefined,
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
.logistics-manufacturing-sop-sop-doc {
  padding: 0 4px 0 0;
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.logistics-manufacturing-sop-sop-doc-table-wrap {
  flex: 1;
  min-height: 0;
  min-width: 0;
}
</style>

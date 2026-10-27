<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/assy-output -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：组立日报管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selecmed-master-key="selecmedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="getAssyOutputId"
      :master-row-selection="rowSelection"
      master-id-column-key="assyOutputId"
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
      create-permission="logistics:manufacturing:output:mfg1:create"
      update-permission="logistics:manufacturing:output:mfg1:update"
      delete-permission="logistics:manufacturing:output:mfg1:delete"
      import-permission="logistics:manufacturing:output:mfg1:import"
      export-permission="logistics:manufacturing:output:mfg1:export"
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
        <template v-if="column.key === 'prodCategory'">
          <TaktDictTag
            :value="getAssyOutputDictValue(record, 'prodCategory')"
            dict-type="logistics_manufacturing_prod_category"
          />
        </template>
        <template v-else-if="column.key === 'shiftNo'">
          <TaktDictTag
            :value="getAssyOutputDictValue(record, 'shiftNo')"
            dict-type="logistics_manufacturing_shift_category"
          />
        </template>
      </template>
      <template #detail>
        <AssyOutputDetailPanel
          ref="assyOutputDetailPanelRef"
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
      <AssyOutputForm
        :key="formData?.assyOutputId ?? 'create'"
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
      :storage-key="'takt-query-fields-logistics-manufacturing-output-assy-output'"
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
      <div v-show="isFieldVisible('prodCategory')">
      <a-form-item :label="pi.queryLabel('prodCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.prodCategory"
          dict-type="logistics_manufacturing_prod_category"
          :placeholder="pi.queryPh('prodCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateStart')">
      <a-form-item :label="pi.queryLabel('prodDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateStart"
          :placeholder="pi.queryPh('prodDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodDateEnd')">
      <a-form-item :label="pi.queryLabel('prodDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.prodDateEnd"
          :placeholder="pi.queryPh('prodDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('teamCode')">
      <a-form-item :label="pi.queryLabel('teamCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.teamCode"
          api-url="TaktProductionTeams/options"
          :api-params="assyTeamQueryOptionsParams"
          :placeholder="pi.queryPh('teamCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directLabor')">
      <a-form-item :label="pi.queryLabel('directLabor')">
        <a-input-number
          v-model:value="advancedQueryForm.directLabor"
          :placeholder="pi.queryPh('directLabor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectLabor')">
      <a-form-item :label="pi.queryLabel('indirectLabor')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectLabor"
          :placeholder="pi.queryPh('indirectLabor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shiftNo')">
      <a-form-item :label="pi.queryLabel('shiftNo')">
        <TaktSelect
          v-model:value="advancedQueryForm.shiftNo"
          dict-type="logistics_manufacturing_shift_category"
          :placeholder="pi.queryPh('shiftNo', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderType')">
      <a-form-item :label="pi.queryLabel('prodOrderType')">
        <a-input
          v-model:value="advancedQueryForm.prodOrderType"
          :placeholder="pi.queryPh('prodOrderType', 'required')"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderCode')">
      <a-form-item :label="pi.queryLabel('prodOrderCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.prodOrderCode"
          api-url="TaktProductionOrders/options"
          :api-params="prodOrderQueryOptionsParams"
          :placeholder="pi.queryPh('prodOrderCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('modelCode')">
      <a-form-item :label="pi.queryLabel('modelCode')">
        <a-input
          v-model:value="advancedQueryForm.modelCode"
          :placeholder="pi.queryPh('modelCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="pi.queryPh('materialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchCode')">
      <a-form-item :label="pi.queryLabel('batchCode')">
        <a-input
          v-model:value="advancedQueryForm.batchCode"
          :placeholder="pi.queryPh('batchCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prodOrderQty')">
      <a-form-item :label="pi.queryLabel('prodOrderQty')">
        <a-input-number
          v-model:value="advancedQueryForm.prodOrderQty"
          :placeholder="pi.queryPh('prodOrderQty', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialCode')">
      <a-form-item :label="pi.queryLabel('serialCode')">
        <a-input
          v-model:value="advancedQueryForm.serialCode"
          :placeholder="pi.queryPh('serialCode', 'required')"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdMinutes')">
      <a-form-item :label="pi.queryLabel('stdMinutes')">
        <a-input-number
          v-model:value="advancedQueryForm.stdMinutes"
          :placeholder="pi.queryPh('stdMinutes', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stdCapacity')">
      <a-form-item :label="pi.queryLabel('stdCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.stdCapacity"
          :placeholder="pi.queryPh('stdCapacity', 'required')"
          style="width: 100%"
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
        :entity-i18n-key="ASSYOUTPUT_SELF_I18N_KEY"
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
      :id-column-key="'assyOutputId'"
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
 * 组立日报管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/assy-output
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import AssyOutputForm from './components/AssyOutputForm.vue'
import AssyOutputDetailPanel from './components/AssyOutputDetailPanel.vue'
import { buildAssyProductionTeamOptionsParams } from '../composables/productionTeamCategory'
import { provideAssyOutputMasterContext, type AssyOutputRowRecord } from './composables/useAssyOutputMasterContext'
import { getAssyOutputList, getAssyOutputById, createAssyOutput, updateAssyOutput, deleteAssyOutputById, deleteAssyOutputBatch, getAssyOutputTemplate, importAssyOutput, exportAssyOutput } from '@/api/logistics/manufacturing/output/assyOutput.api'
import type { AssyOutput, AssyOutputQuery } from '@/types/logistics/manufacturing/output/assyOutput.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useAssyOutputI18n,
  ASSYOUTPUT_LIST_FIELDS,
  ASSYOUTPUT_QUERY_STRING_FIELDS,
  ASSYOUTPUT_QUERY_FIELDS,
  ASSYOUTPUT_SELF_I18N_KEY,
} from './composables/useAssyOutputI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useAssyOutputI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAssyOutput')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<AssyOutput[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<AssyOutputRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<AssyOutputRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AssyOutput> | null>(null)
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
  for (const key of ASSYOUTPUT_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.directLabor !== undefined && form.directLabor !== null) {
    return true
  }
  if (form.indirectLabor !== undefined && form.indirectLabor !== null) {
    return true
  }
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    return true
  }
  if (form.prodOrderQty !== undefined && form.prodOrderQty !== null) {
    return true
  }
  if (form.stdMinutes !== undefined && form.stdMinutes !== null) {
    return true
  }
  if (form.stdCapacity !== undefined && form.stdCapacity !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ASSYOUTPUT_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ASSYOUTPUT_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    directLabor: undefined as number | undefined,
    indirectLabor: undefined as number | undefined,
    shiftNo: undefined as number | undefined,
    prodOrderQty: undefined as number | undefined,
    stdMinutes: undefined as number | undefined,
    stdCapacity: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())

/** 高级查询班组下拉（按工厂 + 组立分类） */
const assyTeamQueryOptionsParams = computed(() =>
  buildAssyProductionTeamOptionsParams(advancedQueryForm.value.plantCode),
)

/** 高级查询工单下拉（按工厂） */
const prodOrderQueryOptionsParams = computed(() => {
  const plant = String(advancedQueryForm.value.plantCode ?? '').trim()
  return plant ? { plantCode: plant } : {}
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ASSYOUTPUT_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'assyOutputId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provideAssyOutputMasterContext()
const assyOutputDetailPanelRef = ref<InstanceType<typeof AssyOutputDetailPanel> | null>(null)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {AssyOutputQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<AssyOutputQuery>): AssyOutputQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: AssyOutputQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof AssyOutputQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of ASSYOUTPUT_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.directLabor !== undefined && form.directLabor !== null) {
    query.directLabor = form.directLabor
  }
  if (form.indirectLabor !== undefined && form.indirectLabor !== null) {
    query.indirectLabor = form.indirectLabor
  }
  if (form.shiftNo !== undefined && form.shiftNo !== null) {
    query.shiftNo = form.shiftNo
  }
  if (form.prodOrderQty !== undefined && form.prodOrderQty !== null) {
    query.prodOrderQty = form.prodOrderQty
  }
  if (form.stdMinutes !== undefined && form.stdMinutes !== null) {
    query.stdMinutes = form.stdMinutes
  }
  if (form.stdCapacity !== undefined && form.stdCapacity !== null) {
    query.stdCapacity = form.stdCapacity
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
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: AssyOutputRowRecord | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? getAssyOutputId(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as AssyOutputRowRecord
  const key = getAssyOutputId(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
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
async function loadAssyOutputDetail(record: AssyOutputRowRecord): Promise<AssyOutput | null> {
  const id = getAssyOutputId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getAssyOutputById(id)
    const index = dataSource.value.findIndex((row) => getAssyOutputId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as AssyOutput
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
    dataIndex: 'assyOutputId',
    key: 'assyOutputId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'assyOutputId') ?? ''
  },
  {
    title: pi.label('prodCategory'),
    dataIndex: 'prodCategory',
    key: 'prodCategory',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('prodDate'),
    dataIndex: 'prodDate',
    key: 'prodDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodDate') ?? ''
  },
  {
    title: pi.label('teamCode'),
    dataIndex: 'teamCode',
    key: 'teamCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'teamCode') ?? ''
  },
  {
    title: pi.label('directLabor'),
    dataIndex: 'directLabor',
    key: 'directLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'directLabor') ?? ''
  },
  {
    title: pi.label('indirectLabor'),
    dataIndex: 'indirectLabor',
    key: 'indirectLabor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'indirectLabor') ?? ''
  },
  {
    title: pi.label('shiftNo'),
    dataIndex: 'shiftNo',
    key: 'shiftNo',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('prodOrderType'),
    dataIndex: 'prodOrderType',
    key: 'prodOrderType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderType') ?? ''
  },
  {
    title: pi.label('prodOrderCode'),
    dataIndex: 'prodOrderCode',
    key: 'prodOrderCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderCode') ?? ''
  },
  {
    title: pi.label('modelCode'),
    dataIndex: 'modelCode',
    key: 'modelCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'modelCode') ?? ''
  },
  {
    title: pi.label('materialCode'),
    dataIndex: 'materialCode',
    key: 'materialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'materialCode') ?? ''
  },
  {
    title: pi.label('batchCode'),
    dataIndex: 'batchCode',
    key: 'batchCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'batchCode') ?? ''
  },
  {
    title: pi.label('prodOrderQty'),
    dataIndex: 'prodOrderQty',
    key: 'prodOrderQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'prodOrderQty') ?? ''
  },
  {
    title: pi.label('serialCode'),
    dataIndex: 'serialCode',
    key: 'serialCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'serialCode') ?? ''
  },
  {
    title: pi.label('stdMinutes'),
    dataIndex: 'stdMinutes',
    key: 'stdMinutes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'stdMinutes') ?? ''
  },
  {
    title: pi.label('stdCapacity'),
    dataIndex: 'stdCapacity',
    key: 'stdCapacity',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getAssyOutputField(record, 'stdCapacity') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:manufacturing:output:mfg1:update',
        onClick: (record: AssyOutputRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:manufacturing:output:mfg1:delete',
        onClick: (record: AssyOutputRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getAssyOutputId = (record: AssyOutputRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getAssyOutputField = (record: any, field: string): any => record?.[field]
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getAssyOutputDictValue = (
  record: AssyOutputRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AssyOutputRowRecord[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }
  },
  onSelect: (record: AssyOutputRowRecord, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
      syncMasterSelection(record)
    } else if (selecmedRow.value && getAssyOutputId(selecmedRow.value) === getAssyOutputId(record)) {
      selecmedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: AssyOutputRowRecord[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
    syncMasterSelection(selecmedRow.value)
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
    const res = await getAssyOutputList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[AssyOutput] 加载数据失败', { error })
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  teamCode: '',
  directLabor: undefined as number | undefined,
  indirectLabor: undefined as number | undefined,
  shiftNo: undefined as number | undefined,
  prodOrderType: '',
  prodOrderCode: '',
  modelCode: '',
  materialCode: '',
  batchCode: '',
  prodOrderQty: undefined as number | undefined,
  serialCode: '',
  stdMinutes: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
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
async function handleEdit(record: AssyOutputRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await loadAssyOutputDetail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
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
      await updateAssyOutput(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createAssyOutput(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
    nextTick(() => formRef.value?.resetFields())
    if (selecmedMasterKey.value) {
      assyOutputDetailPanelRef.value?.reload?.()
    }
    loadData()
  } catch {
    // 400/业务错误已由 request 拦截器 toast；此处吞掉避免 Vue 再报 component event handler
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
  const res = await getAssyOutputTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAssyOutput(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()

      if (selecmedMasterKey.value) {
    assyOutputDetailPanelRef.value?.reload?.()
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
    const exportMeta = await exportAssyOutput(
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
    logger.error('[AssyOutput] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: AssyOutputRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAssyOutputById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAssyOutputBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
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
  prodCategory: '',
  prodDateStart: '',
  prodDateEnd: '',
  teamCode: '',
  directLabor: undefined as number | undefined,
  indirectLabor: undefined as number | undefined,
  shiftNo: undefined as number | undefined,
  prodOrderType: '',
  prodOrderCode: '',
  modelCode: '',
  materialCode: '',
  batchCode: '',
  prodOrderQty: undefined as number | undefined,
  serialCode: '',
  stdMinutes: undefined as number | undefined,
  stdCapacity: undefined as number | undefined,
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

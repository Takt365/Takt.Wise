<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/numbering -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：编码规则实体 定义系统中各类业务单据的编码生成规则管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
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
      create-permission="foundation:numbering:create"
      update-permission="foundation:numbering:update"
      delete-permission="foundation:numbering:delete"
      import-permission="foundation:numbering:import"
      export-permission="foundation:numbering:export"
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
    <TaktSingleTable
      entity-scope="company"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'numberingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getNumberingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'status'">
          <a-switch
            :checked="getNumberingField(record, 'status') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'deptCode'">
          <span>{{ getNumberingField(record, 'deptCode') }}</span>
        </template>
        <template v-else-if="column.key === 'dateFormat'">
          <TaktDictTag
            :value="getNumberingField(record, 'dateFormat')"
            dict-type="sys_numbering_date_format"
          />
        </template>
        <template v-else-if="column.key === 'resetPeriod'">
          <TaktDictTag
            :value="mapResetPeriodDictValue(getNumberingField(record, 'resetPeriod') as string | number | undefined)"
            dict-type="sys_reset_period"
          />
        </template>
        <template v-else-if="column.key === 'isBuiltIn'">
          <TaktDictTag
            :value="getNumberingField(record, 'isBuiltIn')"
            dict-type="sys_yes_no"
          />
        </template>
      </template>

    </TaktSingleTable>

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
      <NumberingForm
        :key="formData?.numberingId ?? 'create'"
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
      :storage-key="'takt-query-fields-foundation-numbering'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('ruleCode')">
      <a-form-item :label="t('entity.numbering.rulecode')">
        <a-input
          v-model:value="advancedQueryForm.ruleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulecode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('ruleName')">
      <a-form-item :label="t('entity.numbering.rulename')">
        <a-input
          v-model:value="advancedQueryForm.ruleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.rulename') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentType')">
      <a-form-item :label="t('entity.numbering.documenttype')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.documentType"
          api-url="TaktMenus/tree-options?valueBy=name"
          :lazy="true"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.documenttype') })"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptCode')">
      <a-form-item :label="t('entity.numbering.deptcode')">
        <TaktTreeSelect
          v-model:value="advancedQueryForm.deptCode"
          api-url="TaktDepts/iso-tree-options"
          :lazy="true"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.deptcode') })"
          allow-clear
          :field-names="{ label: 'dictLabel', value: 'dictValue' }"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('prefixCode')">
      <a-form-item :label="t('entity.numbering.prefixcode')">
        <a-input
          v-model:value="advancedQueryForm.prefixCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.prefixcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dateFormat')">
      <a-form-item :label="t('entity.numbering.dateformat')">
        <TaktSelect
          v-model:value="advancedQueryForm.dateFormat"
          dict-type="sys_numbering_date_format"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.dateformat') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sequenceLength')">
      <a-form-item :label="t('entity.numbering.sequencelength')">
        <a-input-number
          v-model:value="advancedQueryForm.sequenceLength"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencelength') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sequenceStep')">
      <a-form-item :label="t('entity.numbering.sequencestep')">
        <a-input-number
          v-model:value="advancedQueryForm.sequenceStep"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.sequencestep') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('suffixCode')">
      <a-form-item :label="t('entity.numbering.suffixcode')">
        <a-input
          v-model:value="advancedQueryForm.suffixCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.suffixcode') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resetPeriod')">
      <a-form-item :label="t('entity.numbering.resetperiod')">
        <TaktSelect
          v-model:value="advancedQueryForm.resetPeriod"
          dict-type="sys_reset_period"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.resetperiod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentSequence')">
      <a-form-item :label="t('entity.numbering.currentsequence')">
        <a-input-number
          v-model:value="advancedQueryForm.currentSequence"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.currentsequence') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('exampleCode')">
      <a-form-item :label="t('entity.numbering.examplecode')">
        <a-input
          v-model:value="advancedQueryForm.exampleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.examplecode') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('separator')">
      <a-form-item :label="t('entity.numbering.separator')">
        <a-input
          v-model:value="advancedQueryForm.separator"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.numbering.separator') })"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="t('entity.numbering.isbuiltin')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.isbuiltin') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('status')">
      <a-form-item :label="t('entity.numbering.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.status"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.numbering.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.numbering.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.numbering.description') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.numbering._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.numbering._self"
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
      :id-column-key="'numberingId'"
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
 * 编码规则实体 定义系统中各类业务单据的编码生成规则管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/foundation/numbering
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import NumberingForm from './components/NumberingForm.vue'
import { getNumberingList, getNumberingById, createNumbering, updateNumbering, deleteNumberingById, deleteNumberingBatch, getNumberingTemplate, importNumbering, exportNumbering, updateNumberingStatus } from '@/api/foundation/numbering.api'
import type { Numbering, NumberingQuery } from '@/types/foundation/numbering.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktNumbering')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.numbering._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<Numbering[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<Numbering | null>(null)
/** 表格多选行 */
const selectedRows = ref<Numbering[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<Numbering> | null>(null)
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
  ruleCode: '',
  ruleName: '',
  documentType: '',
  deptCode: '',
  prefixCode: '',
  dateFormat: '',
  sequenceLength: undefined as number | undefined,
  sequenceStep: undefined as number | undefined,
  suffixCode: '',
  resetPeriod: '',
  currentSequence: undefined as number | undefined,
  exampleCode: '',
  separator: '',
  isBuiltIn: undefined as number | undefined,
  status: undefined as number | undefined,
  description: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'ruleCode', label: t('entity.numbering.rulecode') },
  { key: 'ruleName', label: t('entity.numbering.rulename') },
  { key: 'documentType', label: t('entity.numbering.documenttype') },
  { key: 'deptCode', label: t('entity.numbering.deptcode') },
  { key: 'prefixCode', label: t('entity.numbering.prefixcode') },
  { key: 'dateFormat', label: t('entity.numbering.dateformat') },
  { key: 'sequenceLength', label: t('entity.numbering.sequencelength') },
  { key: 'sequenceStep', label: t('entity.numbering.sequencestep') },
  { key: 'suffixCode', label: t('entity.numbering.suffixcode') },
  { key: 'resetPeriod', label: t('entity.numbering.resetperiod') },
  { key: 'currentSequence', label: t('entity.numbering.currentsequence') },
  { key: 'exampleCode', label: t('entity.numbering.examplecode') },
  { key: 'separator', label: t('entity.numbering.separator') },
  { key: 'isBuiltIn', label: t('entity.numbering.isbuiltin') },
  { key: 'status', label: t('entity.numbering.status') },
  { key: 'description', label: t('entity.numbering.description') },
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
const entityIdName = 'numberingId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {NumberingQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<NumberingQuery>): NumberingQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: NumberingQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof NumberingQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('ruleCode', form.ruleCode)
  assignTrimmed('ruleName', form.ruleName)
  assignTrimmed('documentType', form.documentType)
  assignTrimmed('deptCode', form.deptCode)
  assignTrimmed('prefixCode', form.prefixCode)
  assignTrimmed('dateFormat', form.dateFormat)
  if (form.sequenceLength !== undefined && form.sequenceLength !== null) {
    query.sequenceLength = form.sequenceLength
  }
  if (form.sequenceStep !== undefined && form.sequenceStep !== null) {
    query.sequenceStep = form.sequenceStep
  }
  assignTrimmed('suffixCode', form.suffixCode)
  assignTrimmed('resetPeriod', form.resetPeriod)
  if (form.currentSequence !== undefined && form.currentSequence !== null) {
    query.currentSequence = form.currentSequence
  }
  assignTrimmed('exampleCode', form.exampleCode)
  assignTrimmed('separator', form.separator)
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    query.isBuiltIn = form.isBuiltIn
  }
  if (form.status !== undefined && form.status !== null) {
    query.status = form.status
  }
  assignTrimmed('description', form.description)
  assignTrimmed('createdAtStart', form.createdAtStart)
  assignTrimmed('createdAtEnd', form.createdAtEnd)
  assignTrimmed('extField', form.extField)
  assignTrimmed('remark', form.remark)
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
    dataIndex: 'numberingId',
    key: 'numberingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'numberingId') ?? ''
  },
  {
    title: t('entity.numbering.rulecode'),
    dataIndex: 'ruleCode',
    key: 'ruleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'ruleCode') ?? ''
  },
  {
    title: t('entity.numbering.rulename'),
    dataIndex: 'ruleName',
    key: 'ruleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'ruleName') ?? ''
  },
  {
    title: t('entity.numbering.documenttype'),
    dataIndex: 'documentType',
    key: 'documentType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'documentType') ?? ''
  },
  {
    title: t('entity.numbering.deptcode'),
    dataIndex: 'deptCode',
    key: 'deptCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'deptCode') ?? ''
  },
  {
    title: t('entity.numbering.prefixcode'),
    dataIndex: 'prefixCode',
    key: 'prefixCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'prefixCode') ?? ''
  },
  {
    title: t('entity.numbering.dateformat'),
    dataIndex: 'dateFormat',
    key: 'dateFormat',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.numbering.sequencelength'),
    dataIndex: 'sequenceLength',
    key: 'sequenceLength',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'sequenceLength') ?? ''
  },
  {
    title: t('entity.numbering.sequencestep'),
    dataIndex: 'sequenceStep',
    key: 'sequenceStep',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'sequenceStep') ?? ''
  },
  {
    title: t('entity.numbering.suffixcode'),
    dataIndex: 'suffixCode',
    key: 'suffixCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'suffixCode') ?? ''
  },
  {
    title: t('entity.numbering.resetperiod'),
    dataIndex: 'resetPeriod',
    key: 'resetPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.numbering.currentsequence'),
    dataIndex: 'currentSequence',
    key: 'currentSequence',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'currentSequence') ?? ''
  },
  {
    title: t('entity.numbering.examplecode'),
    dataIndex: 'exampleCode',
    key: 'exampleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'exampleCode') ?? ''
  },
  {
    title: t('entity.numbering.separator'),
    dataIndex: 'separator',
    key: 'separator',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'separator') ?? ''
  },
  {
    title: t('entity.numbering.isbuiltin'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.numbering.status'),
    dataIndex: 'status',
    key: 'status',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.numbering.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getNumberingField(record, 'description') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:numbering:update',
        onClick: (record: Numbering) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:numbering:delete',
        onClick: (record: Numbering) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getNumberingId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getNumberingField = (record: any, field: string): any => record?.[field]
/** 列表 TaktDictTag：resetPeriod 归一化为 sys_reset_period dictValue */
const RESET_PERIOD_TO_DICT: Record<string, string> = {
  none: 'None',
  annually: 'Annually',
  year: 'Annually',
  yearly: 'Annually',
  monthly: 'Monthly',
  month: 'Monthly',
  daily: 'Daily',
  day: 'Daily',
  hour: 'Daily',
  hourly: 'Daily',
}

/** @param value 后端 resetPeriod */
function mapResetPeriodDictValue(value?: string | number | null): string {
  const raw = String(value ?? 'None').trim()
  const key = raw.toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? raw
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: Numbering[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: Numbering, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getNumberingId(selectedRow.value) === getNumberingId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: Numbering[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: Numbering) => ({
  onClick: () => {
    const key = getNumberingId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getNumberingId(item)))
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
    const res = await getNumberingList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[Numbering] 加载数据失败', { error })
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
  ruleCode: '',
  ruleName: '',
  documentType: '',
  deptCode: '',
  prefixCode: '',
  dateFormat: '',
  sequenceLength: undefined as number | undefined,
  sequenceStep: undefined as number | undefined,
  suffixCode: '',
  resetPeriod: '',
  currentSequence: undefined as number | undefined,
  exampleCode: '',
  separator: '',
  isBuiltIn: undefined as number | undefined,
  status: undefined as number | undefined,
  description: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.numbering._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: Numbering) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.numbering._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.numbering._self') }))
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
      await updateNumbering(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.numbering._self') }))
    } else {
      await createNumbering(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.numbering._self') }))
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
  const res = await getNumberingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importNumbering(file, sheetName)
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
    const exportMeta = await exportNumbering(
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
    message.success(t('common.feedback.export.success', { target: t('entity.numbering._self') }))
  } catch (error: any) {
    logger.error('[Numbering] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.numbering._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: Numbering) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.numbering._self'), name: t('common.tip.this.target', { target: t('entity.numbering._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteNumberingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.numbering._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.numbering._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.numbering._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteNumberingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.numbering._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleStatusChange(record: Numbering, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getNumberingField(record, 'status')
  const id = getNumberingId(record)
  const row = dataSource.value.find((item) => getNumberingId(item) === id)
  if (row) {
    row.status = newVal
  }
  try {
    await updateNumberingStatus({ numberingId: id, status: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.status = oldVal
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
  ruleCode: '',
  ruleName: '',
  documentType: '',
  deptCode: '',
  prefixCode: '',
  dateFormat: '',
  sequenceLength: undefined as number | undefined,
  sequenceStep: undefined as number | undefined,
  suffixCode: '',
  resetPeriod: '',
  currentSequence: undefined as number | undefined,
  exampleCode: '',
  separator: '',
  isBuiltIn: undefined as number | undefined,
  status: undefined as number | undefined,
  description: '',
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

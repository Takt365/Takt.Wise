<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/standard-wage-rate -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：标准工资率实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="accounting:controlling:standard:wage:rate:create"
      update-permission="accounting:controlling:standard:wage:rate:update"
      delete-permission="accounting:controlling:standard:wage:rate:delete"
      import-permission="accounting:controlling:standard:wage:rate:import"
      export-permission="accounting:controlling:standard:wage:rate:export"
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
      :id-column-key="'standardWageRateId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getStandardWageRateId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

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
      <StandardWageRateForm
        :key="formData?.standardWageRateId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-controlling-standard-wage-rate'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('yearMonth')">
      <a-form-item :label="t('entity.standardwagerate.yearmonth')">
        <a-input
          v-model:value="advancedQueryForm.yearMonth"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.yearmonth') })"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workingDays')">
      <a-form-item :label="t('entity.standardwagerate.workingdays')">
        <a-input-number
          v-model:value="advancedQueryForm.workingDays"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.workingdays') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salesAmount')">
      <a-form-item :label="t('entity.standardwagerate.salesamount')">
        <a-input-number
          v-model:value="advancedQueryForm.salesAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.salesamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directLaborCount')">
      <a-form-item :label="t('entity.standardwagerate.directlaborcount')">
        <a-input-number
          v-model:value="advancedQueryForm.directLaborCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.directlaborcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directLaborWage')">
      <a-form-item :label="t('entity.standardwagerate.directlaborwage')">
        <a-input-number
          v-model:value="advancedQueryForm.directLaborWage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.directlaborwage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directOvertimeHours')">
      <a-form-item :label="t('entity.standardwagerate.directovertimehours')">
        <a-input-number
          v-model:value="advancedQueryForm.directOvertimeHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.directovertimehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directOvertimeTotal')">
      <a-form-item :label="t('entity.standardwagerate.directovertimetotal')">
        <a-input-number
          v-model:value="advancedQueryForm.directOvertimeTotal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.directovertimetotal') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('directWageRate')">
      <a-form-item :label="t('entity.standardwagerate.directwagerate')">
        <a-input-number
          v-model:value="advancedQueryForm.directWageRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.directwagerate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectLaborCount')">
      <a-form-item :label="t('entity.standardwagerate.indirectlaborcount')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectLaborCount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.indirectlaborcount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectLaborWage')">
      <a-form-item :label="t('entity.standardwagerate.indirectlaborwage')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectLaborWage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.indirectlaborwage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectOvertimeHours')">
      <a-form-item :label="t('entity.standardwagerate.indirectovertimehours')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectOvertimeHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.indirectovertimehours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectOvertimeTotal')">
      <a-form-item :label="t('entity.standardwagerate.indirectovertimetotal')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectOvertimeTotal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.indirectovertimetotal') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('indirectWageRate')">
      <a-form-item :label="t('entity.standardwagerate.indirectwagerate')">
        <a-input-number
          v-model:value="advancedQueryForm.indirectWageRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.standardwagerate.indirectwagerate') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.standardwagerate._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.standardwagerate._self"
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
      :id-column-key="'standardWageRateId'"
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
 * 标准工资率实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/standard-wage-rate
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import StandardWageRateForm from './components/StandardWageRateForm.vue'
import { getStandardWageRateList, getStandardWageRateById, createStandardWageRate, updateStandardWageRate, deleteStandardWageRateById, deleteStandardWageRateBatch, getStandardWageRateTemplate, importStandardWageRate, exportStandardWageRate } from '@/api/accounting/controlling/standardWageRate.api'
import type { StandardWageRate, StandardWageRateQuery } from '@/types/accounting/controlling/standardWageRate.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktStandardWageRate')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.standardwagerate._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<StandardWageRate[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<StandardWageRate | null>(null)
/** 表格多选行 */
const selecmedRows = ref<StandardWageRate[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<StandardWageRate> | null>(null)
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
  yearMonth: '',
  workingDays: undefined as number | undefined,
  salesAmount: undefined as number | undefined,
  directLaborCount: undefined as number | undefined,
  directLaborWage: undefined as number | undefined,
  directOvertimeHours: undefined as number | undefined,
  directOvertimeTotal: undefined as number | undefined,
  directWageRate: undefined as number | undefined,
  indirectLaborCount: undefined as number | undefined,
  indirectLaborWage: undefined as number | undefined,
  indirectOvertimeHours: undefined as number | undefined,
  indirectOvertimeTotal: undefined as number | undefined,
  indirectWageRate: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'yearMonth', label: t('entity.standardwagerate.yearmonth') },
  { key: 'workingDays', label: t('entity.standardwagerate.workingdays') },
  { key: 'salesAmount', label: t('entity.standardwagerate.salesamount') },
  { key: 'directLaborCount', label: t('entity.standardwagerate.directlaborcount') },
  { key: 'directLaborWage', label: t('entity.standardwagerate.directlaborwage') },
  { key: 'directOvertimeHours', label: t('entity.standardwagerate.directovertimehours') },
  { key: 'directOvertimeTotal', label: t('entity.standardwagerate.directovertimetotal') },
  { key: 'directWageRate', label: t('entity.standardwagerate.directwagerate') },
  { key: 'indirectLaborCount', label: t('entity.standardwagerate.indirectlaborcount') },
  { key: 'indirectLaborWage', label: t('entity.standardwagerate.indirectlaborwage') },
  { key: 'indirectOvertimeHours', label: t('entity.standardwagerate.indirectovertimehours') },
  { key: 'indirectOvertimeTotal', label: t('entity.standardwagerate.indirectovertimetotal') },
  { key: 'indirectWageRate', label: t('entity.standardwagerate.indirectwagerate') },
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
const entityIdName = 'standardWageRateId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {StandardWageRateQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<StandardWageRateQuery>): StandardWageRateQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: StandardWageRateQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof StandardWageRateQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('yearMonth', form.yearMonth)
  if (form.workingDays !== undefined && form.workingDays !== null) {
    query.workingDays = form.workingDays
  }
  if (form.salesAmount !== undefined && form.salesAmount !== null) {
    query.salesAmount = form.salesAmount
  }
  if (form.directLaborCount !== undefined && form.directLaborCount !== null) {
    query.directLaborCount = form.directLaborCount
  }
  if (form.directLaborWage !== undefined && form.directLaborWage !== null) {
    query.directLaborWage = form.directLaborWage
  }
  if (form.directOvertimeHours !== undefined && form.directOvertimeHours !== null) {
    query.directOvertimeHours = form.directOvertimeHours
  }
  if (form.directOvertimeTotal !== undefined && form.directOvertimeTotal !== null) {
    query.directOvertimeTotal = form.directOvertimeTotal
  }
  if (form.directWageRate !== undefined && form.directWageRate !== null) {
    query.directWageRate = form.directWageRate
  }
  if (form.indirectLaborCount !== undefined && form.indirectLaborCount !== null) {
    query.indirectLaborCount = form.indirectLaborCount
  }
  if (form.indirectLaborWage !== undefined && form.indirectLaborWage !== null) {
    query.indirectLaborWage = form.indirectLaborWage
  }
  if (form.indirectOvertimeHours !== undefined && form.indirectOvertimeHours !== null) {
    query.indirectOvertimeHours = form.indirectOvertimeHours
  }
  if (form.indirectOvertimeTotal !== undefined && form.indirectOvertimeTotal !== null) {
    query.indirectOvertimeTotal = form.indirectOvertimeTotal
  }
  if (form.indirectWageRate !== undefined && form.indirectWageRate !== null) {
    query.indirectWageRate = form.indirectWageRate
  }
  assignTrimmed('plantCode', form.plantCode)
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

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'standardWageRateId',
    key: 'standardWageRateId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'standardWageRateId') ?? ''
  },
  {
    title: t('entity.standardwagerate.yearmonth'),
    dataIndex: 'yearMonth',
    key: 'yearMonth',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'yearMonth') ?? ''
  },
  {
    title: t('entity.standardwagerate.workingdays'),
    dataIndex: 'workingDays',
    key: 'workingDays',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'workingDays') ?? ''
  },
  {
    title: t('entity.standardwagerate.salesamount'),
    dataIndex: 'salesAmount',
    key: 'salesAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'salesAmount') ?? ''
  },
  {
    title: t('entity.standardwagerate.directlaborcount'),
    dataIndex: 'directLaborCount',
    key: 'directLaborCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'directLaborCount') ?? ''
  },
  {
    title: t('entity.standardwagerate.directlaborwage'),
    dataIndex: 'directLaborWage',
    key: 'directLaborWage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'directLaborWage') ?? ''
  },
  {
    title: t('entity.standardwagerate.directovertimehours'),
    dataIndex: 'directOvertimeHours',
    key: 'directOvertimeHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'directOvertimeHours') ?? ''
  },
  {
    title: t('entity.standardwagerate.directovertimetotal'),
    dataIndex: 'directOvertimeTotal',
    key: 'directOvertimeTotal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'directOvertimeTotal') ?? ''
  },
  {
    title: t('entity.standardwagerate.directwagerate'),
    dataIndex: 'directWageRate',
    key: 'directWageRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'directWageRate') ?? ''
  },
  {
    title: t('entity.standardwagerate.indirectlaborcount'),
    dataIndex: 'indirectLaborCount',
    key: 'indirectLaborCount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'indirectLaborCount') ?? ''
  },
  {
    title: t('entity.standardwagerate.indirectlaborwage'),
    dataIndex: 'indirectLaborWage',
    key: 'indirectLaborWage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'indirectLaborWage') ?? ''
  },
  {
    title: t('entity.standardwagerate.indirectovertimehours'),
    dataIndex: 'indirectOvertimeHours',
    key: 'indirectOvertimeHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'indirectOvertimeHours') ?? ''
  },
  {
    title: t('entity.standardwagerate.indirectovertimetotal'),
    dataIndex: 'indirectOvertimeTotal',
    key: 'indirectOvertimeTotal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'indirectOvertimeTotal') ?? ''
  },
  {
    title: t('entity.standardwagerate.indirectwagerate'),
    dataIndex: 'indirectWageRate',
    key: 'indirectWageRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'indirectWageRate') ?? ''
  },
  {
    title: t('entity.standardwagerate.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getStandardWageRateField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:controlling:standard:wage:rate:update',
        onClick: (record: StandardWageRate) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:controlling:standard:wage:rate:delete',
        onClick: (record: StandardWageRate) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getStandardWageRateId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getStandardWageRateField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: StandardWageRate[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: StandardWageRate, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getStandardWageRateId(selecmedRow.value) === getStandardWageRateId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: StandardWageRate[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: StandardWageRate) => ({
  onClick: () => {
    const key = getStandardWageRateId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getStandardWageRateId(item)))
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
    const res = await getStandardWageRateList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[StandardWageRate] 加载数据失败', { error })
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
  yearMonth: '',
  workingDays: undefined as number | undefined,
  salesAmount: undefined as number | undefined,
  directLaborCount: undefined as number | undefined,
  directLaborWage: undefined as number | undefined,
  directOvertimeHours: undefined as number | undefined,
  directOvertimeTotal: undefined as number | undefined,
  directWageRate: undefined as number | undefined,
  indirectLaborCount: undefined as number | undefined,
  indirectLaborWage: undefined as number | undefined,
  indirectOvertimeHours: undefined as number | undefined,
  indirectOvertimeTotal: undefined as number | undefined,
  indirectWageRate: undefined as number | undefined,
  plantCode: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.standardwagerate._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: StandardWageRate) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.standardwagerate._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.standardwagerate._self') }))
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
      await updateStandardWageRate(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.standardwagerate._self') }))
    } else {
      await createStandardWageRate(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.standardwagerate._self') }))
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
  const res = await getStandardWageRateTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importStandardWageRate(file, sheetName)
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
    const exportMeta = await exportStandardWageRate(
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
    message.success(t('common.feedback.export.success', { target: t('entity.standardwagerate._self') }))
  } catch (error: any) {
    logger.error('[StandardWageRate] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.standardwagerate._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: StandardWageRate) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.standardwagerate._self'), name: t('common.tip.this.target', { target: t('entity.standardwagerate._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteStandardWageRateById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.standardwagerate._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.standardwagerate._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.standardwagerate._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteStandardWageRateBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.standardwagerate._self') }))
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
  yearMonth: '',
  workingDays: undefined as number | undefined,
  salesAmount: undefined as number | undefined,
  directLaborCount: undefined as number | undefined,
  directLaborWage: undefined as number | undefined,
  directOvertimeHours: undefined as number | undefined,
  directOvertimeTotal: undefined as number | undefined,
  directWageRate: undefined as number | undefined,
  indirectLaborCount: undefined as number | undefined,
  indirectLaborWage: undefined as number | undefined,
  indirectOvertimeHours: undefined as number | undefined,
  indirectOvertimeTotal: undefined as number | undefined,
  indirectWageRate: undefined as number | undefined,
  plantCode: '',
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

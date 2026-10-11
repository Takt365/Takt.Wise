<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/salary-formula -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：薪资计算公式管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:compensation:salary:formula:create"
      update-permission="human:resource:compensation:salary:formula:update"
      delete-permission="human:resource:compensation:salary:formula:delete"
      import-permission="human:resource:compensation:salary:formula:import"
      export-permission="human:resource:compensation:salary:formula:export"
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
      :id-column-key="'salaryFormulaId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalaryFormulaId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'formulaStatus'">
          <a-switch
            :checked="getSalaryFormulaField(record, 'formulaStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleFormulaStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'formulaStep'">
          <TaktDictTag
            :value="getSalaryFormulaField(record, 'formulaStep')"
            dict-type="humanresource_compensation_salary_formula_step_type"
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
      <SalaryFormulaForm
        :key="formData?.salaryFormulaId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-compensation-salary-formula'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('setCode')">
      <a-form-item :label="t('entity.salaryformula.setcode')">
        <a-input
          v-model:value="advancedQueryForm.setCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.setcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('setName')">
      <a-form-item :label="t('entity.salaryformula.setname')">
        <a-input
          v-model:value="advancedQueryForm.setName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.setname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('payrollId')">
      <a-form-item :label="t('entity.salaryformula.payrollid')">
        <a-input
          v-model:value="advancedQueryForm.payrollId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.payrollid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaCode')">
      <a-form-item :label="t('entity.salaryformula.formulacode')">
        <a-input
          v-model:value="advancedQueryForm.formulaCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulacode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaName')">
      <a-form-item :label="t('entity.salaryformula.formulaname')">
        <a-input
          v-model:value="advancedQueryForm.formulaName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulaname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaStep')">
      <a-form-item :label="t('entity.salaryformula.formulastep')">
        <TaktSelect
          v-model:value="advancedQueryForm.formulaStep"
          dict-type="humanresource_compensation_salary_formula_step_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.formulastep') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetField')">
      <a-form-item :label="t('entity.salaryformula.targetfield')">
        <a-input
          v-model:value="advancedQueryForm.targetField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.targetfield') })"
          show-count
          :maxlength="64"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaExpression')">
      <a-form-item :label="t('entity.salaryformula.formulaexpression')">
        <a-input
          v-model:value="advancedQueryForm.formulaExpression"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.formulaexpression') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stepDescription')">
      <a-form-item :label="t('entity.salaryformula.stepdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.stepDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.salaryformula.stepdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateStart')">
      <a-form-item :label="t('entity.salaryformula.effectivedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.effectivedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectiveDateEnd')">
      <a-form-item :label="t('entity.salaryformula.effectivedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.effectiveDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.effectivedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateStart')">
      <a-form-item :label="t('entity.salaryformula.expirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.expirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expiryDateEnd')">
      <a-form-item :label="t('entity.salaryformula.expirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.expirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formulaStatus')">
      <a-form-item :label="t('entity.salaryformula.formulastatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.formulaStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryformula.formulastatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.salaryformula.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryformula.relatedplant') })"
          show-count
          :maxlength="4"
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
      :title="t('common.dialog.title.import', { entity: t('entity.salaryformula._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salaryformula._self"
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
      :id-column-key="'salaryFormulaId'"
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
 * 薪资计算公式管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/salary-formula
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SalaryFormulaForm from './components/SalaryFormulaForm.vue'
import { getSalaryFormulaList, getSalaryFormulaById, createSalaryFormula, updateSalaryFormula, deleteSalaryFormulaById, deleteSalaryFormulaBatch, getSalaryFormulaTemplate, importSalaryFormula, exportSalaryFormula, updateSalaryFormulaStatus } from '@/api/human-resource/compensation/salaryFormula.api'
import type { SalaryFormula, SalaryFormulaQuery } from '@/types/human-resource/compensation/salaryFormula.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalaryFormula')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salaryformula._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalaryFormula[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<SalaryFormula | null>(null)
/** 表格多选行 */
const selectedRows = ref<SalaryFormula[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalaryFormula> | null>(null)
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
  setCode: '',
  setName: '',
  payrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'setCode', label: t('entity.salaryformula.setcode') },
  { key: 'setName', label: t('entity.salaryformula.setname') },
  { key: 'payrollId', label: t('entity.salaryformula.payrollid') },
  { key: 'formulaCode', label: t('entity.salaryformula.formulacode') },
  { key: 'formulaName', label: t('entity.salaryformula.formulaname') },
  { key: 'formulaStep', label: t('entity.salaryformula.formulastep') },
  { key: 'targetField', label: t('entity.salaryformula.targetfield') },
  { key: 'formulaExpression', label: t('entity.salaryformula.formulaexpression') },
  { key: 'stepDescription', label: t('entity.salaryformula.stepdescription') },
  { key: 'effectiveDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.salaryformula.effectivedate')) },
  { key: 'effectiveDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.salaryformula.effectivedate')) },
  { key: 'expiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.salaryformula.expirydate')) },
  { key: 'expiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.salaryformula.expirydate')) },
  { key: 'formulaStatus', label: t('entity.salaryformula.formulastatus') },
  { key: 'plantCode', label: t('entity.salaryformula.relatedplant') },
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
const entityIdName = 'salaryFormulaId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SalaryFormulaQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalaryFormulaQuery>): SalaryFormulaQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalaryFormulaQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalaryFormulaQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('setCode', form.setCode)
  assignTrimmed('setName', form.setName)
  assignTrimmed('payrollId', form.payrollId)
  assignTrimmed('formulaCode', form.formulaCode)
  assignTrimmed('formulaName', form.formulaName)
  if (form.formulaStep !== undefined && form.formulaStep !== null) {
    query.formulaStep = form.formulaStep
  }
  assignTrimmed('targetField', form.targetField)
  assignTrimmed('formulaExpression', form.formulaExpression)
  assignTrimmed('stepDescription', form.stepDescription)
  assignTrimmed('effectiveDateStart', form.effectiveDateStart)
  assignTrimmed('effectiveDateEnd', form.effectiveDateEnd)
  assignTrimmed('expiryDateStart', form.expiryDateStart)
  assignTrimmed('expiryDateEnd', form.expiryDateEnd)
  if (form.formulaStatus !== undefined && form.formulaStatus !== null) {
    query.formulaStatus = form.formulaStatus
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
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'salaryFormulaId',
    key: 'salaryFormulaId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'salaryFormulaId') ?? ''
  },
  {
    title: t('entity.salaryformula.setcode'),
    dataIndex: 'setCode',
    key: 'setCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'setCode') ?? ''
  },
  {
    title: t('entity.salaryformula.setname'),
    dataIndex: 'setName',
    key: 'setName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'setName') ?? ''
  },
  {
    title: t('entity.salaryformula.payrollid'),
    dataIndex: 'payrollId',
    key: 'payrollId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'payrollId') ?? ''
  },
  {
    title: t('entity.salaryformula.formulacode'),
    dataIndex: 'formulaCode',
    key: 'formulaCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaCode') ?? ''
  },
  {
    title: t('entity.salaryformula.formulaname'),
    dataIndex: 'formulaName',
    key: 'formulaName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaName') ?? ''
  },
  {
    title: t('entity.salaryformula.formulastep'),
    dataIndex: 'formulaStep',
    key: 'formulaStep',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryformula.targetfield'),
    dataIndex: 'targetField',
    key: 'targetField',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'targetField') ?? ''
  },
  {
    title: t('entity.salaryformula.formulaexpression'),
    dataIndex: 'formulaExpression',
    key: 'formulaExpression',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'formulaExpression') ?? ''
  },
  {
    title: t('entity.salaryformula.stepdescription'),
    dataIndex: 'stepDescription',
    key: 'stepDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'stepDescription') ?? ''
  },
  {
    title: t('entity.salaryformula.effectivedate'),
    dataIndex: 'effectiveDate',
    key: 'effectiveDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'effectiveDate') ?? ''
  },
  {
    title: t('entity.salaryformula.expirydate'),
    dataIndex: 'expiryDate',
    key: 'expiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'expiryDate') ?? ''
  },
  {
    title: t('entity.salaryformula.formulastatus'),
    dataIndex: 'formulaStatus',
    key: 'formulaStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryformula.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryFormulaField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:compensation:salary:formula:update',
        onClick: (record: SalaryFormula) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:compensation:salary:formula:delete',
        onClick: (record: SalaryFormula) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalaryFormulaId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalaryFormulaField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalaryFormula[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalaryFormula, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getSalaryFormulaId(selectedRow.value) === getSalaryFormulaId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: SalaryFormula[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalaryFormula) => ({
  onClick: () => {
    const key = getSalaryFormulaId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getSalaryFormulaId(item)))
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
    const res = await getSalaryFormulaList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalaryFormula] 加载数据失败', { error })
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
  setCode: '',
  setName: '',
  payrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salaryformula._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: SalaryFormula) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salaryformula._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salaryformula._self') }))
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
      await updateSalaryFormula(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salaryformula._self') }))
    } else {
      await createSalaryFormula(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salaryformula._self') }))
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
  const res = await getSalaryFormulaTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalaryFormula(file, sheetName)
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
    const exportMeta = await exportSalaryFormula(
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
    message.success(t('common.feedback.export.success', { target: t('entity.salaryformula._self') }))
  } catch (error: any) {
    logger.error('[SalaryFormula] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salaryformula._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalaryFormula) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salaryformula._self'), name: t('common.tip.this.target', { target: t('entity.salaryformula._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalaryFormulaById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salaryformula._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salaryformula._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salaryformula._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalaryFormulaBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salaryformula._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleFormulaStatusChange(record: SalaryFormula, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getSalaryFormulaField(record, 'formulaStatus')
  const id = getSalaryFormulaId(record)
  const row = dataSource.value.find((item) => getSalaryFormulaId(item) === id)
  if (row) {
    row.formulaStatus = newVal
  }
  try {
    await updateSalaryFormulaStatus({ salaryFormulaId: id, formulaStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.formulaStatus = oldVal
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
  setCode: '',
  setName: '',
  payrollId: '',
  formulaCode: '',
  formulaName: '',
  formulaStep: undefined as number | undefined,
  targetField: '',
  formulaExpression: '',
  stepDescription: '',
  effectiveDateStart: '',
  effectiveDateEnd: '',
  expiryDateStart: '',
  expiryDateEnd: '',
  formulaStatus: undefined as number | undefined,
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

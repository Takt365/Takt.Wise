<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-assessment -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工绩效考核管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:performance:perf:assessment:create"
      update-permission="human:resource:performance:perf:assessment:update"
      delete-permission="human:resource:performance:perf:assessment:delete"
      import-permission="human:resource:performance:perf:assessment:import"
      export-permission="human:resource:performance:perf:assessment:export"
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
      :id-column-key="'perfAssessmentId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPerfAssessmentId"
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
      <PerfAssessmentForm
        :key="formData?.perfAssessmentId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-performance-perf-assessment'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.perfassessment.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.perfassessment.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.employeename') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentPeriod')">
      <a-form-item :label="t('entity.perfassessment.assessmentperiod')">
        <a-input
          v-model:value="advancedQueryForm.assessmentPeriod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.assessmentperiod') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateStart')">
      <a-form-item :label="t('entity.perfassessment.assessmentdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfassessment.assessmentdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentDateEnd')">
      <a-form-item :label="t('entity.perfassessment.assessmentdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.assessmentDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfassessment.assessmentdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('schemeMetricId')">
      <a-form-item :label="t('entity.perfassessment.schememetricid')">
        <a-input
          v-model:value="advancedQueryForm.schemeMetricId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.schememetricid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfScore')">
      <a-form-item :label="t('entity.perfassessment.selfscore')">
        <a-input-number
          v-model:value="advancedQueryForm.selfScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.selfscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationNotes')">
      <a-form-item :label="t('entity.perfassessment.selfevaluationnotes')">
        <a-textarea
          v-model:value="advancedQueryForm.selfEvaluationNotes"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfassessment.selfevaluationnotes') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorScore')">
      <a-form-item :label="t('entity.perfassessment.supervisorscore')">
        <a-input-number
          v-model:value="advancedQueryForm.supervisorScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.supervisorscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorComments')">
      <a-form-item :label="t('entity.perfassessment.supervisorcomments')">
        <a-input
          v-model:value="advancedQueryForm.supervisorComments"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.supervisorcomments') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('finalScore')">
      <a-form-item :label="t('entity.perfassessment.finalscore')">
        <a-input-number
          v-model:value="advancedQueryForm.finalScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.finalscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('performanceGrade')">
      <a-form-item :label="t('entity.perfassessment.performancegrade')">
        <a-input
          v-model:value="advancedQueryForm.performanceGrade"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.performancegrade') })"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reviewerId')">
      <a-form-item :label="t('entity.perfassessment.reviewerid')">
        <a-input
          v-model:value="advancedQueryForm.reviewerId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.reviewerid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateStart')">
      <a-form-item :label="t('entity.perfassessment.interviewdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfassessment.interviewdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDateEnd')">
      <a-form-item :label="t('entity.perfassessment.interviewdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfassessment.interviewdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewNotes')">
      <a-form-item :label="t('entity.perfassessment.interviewnotes')">
        <a-textarea
          v-model:value="advancedQueryForm.interviewNotes"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfassessment.interviewnotes') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentStatus')">
      <a-form-item :label="t('entity.perfassessment.assessmentstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.assessmentStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.assessmentstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.perfassessment.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfassessment.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.perfassessment._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.perfassessment._self"
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
      :id-column-key="'perfAssessmentId'"
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
 * 员工绩效考核管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-assessment
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import PerfAssessmentForm from './components/PerfAssessmentForm.vue'
import { getPerfAssessmentList, getPerfAssessmentById, createPerfAssessment, updatePerfAssessment, deletePerfAssessmentById, deletePerfAssessmentBatch, getPerfAssessmentTemplate, importPerfAssessment, exportPerfAssessment, updatePerfAssessmentStatus } from '@/api/human-resource/performance/perfAssessment.api'
import type { PerfAssessment, PerfAssessmentQuery } from '@/types/human-resource/performance/perfAssessment.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPerfAssessment')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.perfassessment._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PerfAssessment[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PerfAssessment | null>(null)
/** 表格多选行 */
const selectedRows = ref<PerfAssessment[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PerfAssessment> | null>(null)
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
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.perfassessment.employeeid') },
  { key: 'employeeName', label: t('entity.perfassessment.employeename') },
  { key: 'assessmentPeriod', label: t('entity.perfassessment.assessmentperiod') },
  { key: 'assessmentDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfassessment.assessmentdate')) },
  { key: 'assessmentDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfassessment.assessmentdate')) },
  { key: 'schemeMetricId', label: t('entity.perfassessment.schememetricid') },
  { key: 'selfScore', label: t('entity.perfassessment.selfscore') },
  { key: 'selfEvaluationNotes', label: t('entity.perfassessment.selfevaluationnotes') },
  { key: 'supervisorScore', label: t('entity.perfassessment.supervisorscore') },
  { key: 'supervisorComments', label: t('entity.perfassessment.supervisorcomments') },
  { key: 'finalScore', label: t('entity.perfassessment.finalscore') },
  { key: 'performanceGrade', label: t('entity.perfassessment.performancegrade') },
  { key: 'reviewerId', label: t('entity.perfassessment.reviewerid') },
  { key: 'interviewDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfassessment.interviewdate')) },
  { key: 'interviewDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfassessment.interviewdate')) },
  { key: 'interviewNotes', label: t('entity.perfassessment.interviewnotes') },
  { key: 'assessmentStatus', label: t('entity.perfassessment.assessmentstatus') },
  { key: 'plantCode', label: t('entity.perfassessment.relatedplant') },
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
const entityIdName = 'perfAssessmentId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PerfAssessmentQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PerfAssessmentQuery>): PerfAssessmentQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PerfAssessmentQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PerfAssessmentQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('employeeName', form.employeeName)
  assignTrimmed('assessmentPeriod', form.assessmentPeriod)
  assignTrimmed('assessmentDateStart', form.assessmentDateStart)
  assignTrimmed('assessmentDateEnd', form.assessmentDateEnd)
  assignTrimmed('schemeMetricId', form.schemeMetricId)
  if (form.selfScore !== undefined && form.selfScore !== null) {
    query.selfScore = form.selfScore
  }
  assignTrimmed('selfEvaluationNotes', form.selfEvaluationNotes)
  if (form.supervisorScore !== undefined && form.supervisorScore !== null) {
    query.supervisorScore = form.supervisorScore
  }
  assignTrimmed('supervisorComments', form.supervisorComments)
  if (form.finalScore !== undefined && form.finalScore !== null) {
    query.finalScore = form.finalScore
  }
  assignTrimmed('performanceGrade', form.performanceGrade)
  assignTrimmed('reviewerId', form.reviewerId)
  assignTrimmed('interviewDateStart', form.interviewDateStart)
  assignTrimmed('interviewDateEnd', form.interviewDateEnd)
  assignTrimmed('interviewNotes', form.interviewNotes)
  if (form.assessmentStatus !== undefined && form.assessmentStatus !== null) {
    query.assessmentStatus = form.assessmentStatus
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
    dataIndex: 'perfAssessmentId',
    key: 'perfAssessmentId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'perfAssessmentId') ?? ''
  },
  {
    title: t('entity.perfassessment.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.perfassessment.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.perfassessment.assessmentperiod'),
    dataIndex: 'assessmentPeriod',
    key: 'assessmentPeriod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'assessmentPeriod') ?? ''
  },
  {
    title: t('entity.perfassessment.assessmentdate'),
    dataIndex: 'assessmentDate',
    key: 'assessmentDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'assessmentDate') ?? ''
  },
  {
    title: t('entity.perfassessment.schememetricid'),
    dataIndex: 'schemeMetricId',
    key: 'schemeMetricId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'schemeMetricId') ?? ''
  },
  {
    title: t('entity.perfassessment.selfscore'),
    dataIndex: 'selfScore',
    key: 'selfScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'selfScore') ?? ''
  },
  {
    title: t('entity.perfassessment.selfevaluationnotes'),
    dataIndex: 'selfEvaluationNotes',
    key: 'selfEvaluationNotes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'selfEvaluationNotes') ?? ''
  },
  {
    title: t('entity.perfassessment.supervisorscore'),
    dataIndex: 'supervisorScore',
    key: 'supervisorScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'supervisorScore') ?? ''
  },
  {
    title: t('entity.perfassessment.supervisorcomments'),
    dataIndex: 'supervisorComments',
    key: 'supervisorComments',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'supervisorComments') ?? ''
  },
  {
    title: t('entity.perfassessment.finalscore'),
    dataIndex: 'finalScore',
    key: 'finalScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'finalScore') ?? ''
  },
  {
    title: t('entity.perfassessment.performancegrade'),
    dataIndex: 'performanceGrade',
    key: 'performanceGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'performanceGrade') ?? ''
  },
  {
    title: t('entity.perfassessment.reviewerid'),
    dataIndex: 'reviewerId',
    key: 'reviewerId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'reviewerId') ?? ''
  },
  {
    title: t('entity.perfassessment.interviewdate'),
    dataIndex: 'interviewDate',
    key: 'interviewDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'interviewDate') ?? ''
  },
  {
    title: t('entity.perfassessment.interviewnotes'),
    dataIndex: 'interviewNotes',
    key: 'interviewNotes',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'interviewNotes') ?? ''
  },
  {
    title: t('entity.perfassessment.assessmentstatus'),
    dataIndex: 'assessmentStatus',
    key: 'assessmentStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'assessmentStatus') ?? ''
  },
  {
    title: t('entity.perfassessment.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAssessmentField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:performance:perf:assessment:update',
        onClick: (record: PerfAssessment) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:performance:perf:assessment:delete',
        onClick: (record: PerfAssessment) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPerfAssessmentId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPerfAssessmentField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PerfAssessment[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PerfAssessment, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPerfAssessmentId(selectedRow.value) === getPerfAssessmentId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PerfAssessment[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PerfAssessment) => ({
  onClick: () => {
    const key = getPerfAssessmentId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPerfAssessmentId(item)))
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
    const res = await getPerfAssessmentList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PerfAssessment] 加载数据失败', { error })
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
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.perfassessment._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: PerfAssessment) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.perfassessment._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.perfassessment._self') }))
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
      await updatePerfAssessment(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.perfassessment._self') }))
    } else {
      await createPerfAssessment(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.perfassessment._self') }))
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
  const res = await getPerfAssessmentTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPerfAssessment(file, sheetName)
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
    const exportMeta = await exportPerfAssessment(
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
    message.success(t('common.feedback.export.success', { target: t('entity.perfassessment._self') }))
  } catch (error: any) {
    logger.error('[PerfAssessment] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.perfassessment._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PerfAssessment) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.perfassessment._self'), name: t('common.tip.this.target', { target: t('entity.perfassessment._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePerfAssessmentById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.perfassessment._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.perfassessment._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.perfassessment._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePerfAssessmentBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.perfassessment._self') }))
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
  employeeId: '',
  employeeName: '',
  assessmentPeriod: '',
  assessmentDateStart: '',
  assessmentDateEnd: '',
  schemeMetricId: '',
  selfScore: undefined as number | undefined,
  selfEvaluationNotes: '',
  supervisorScore: undefined as number | undefined,
  supervisorComments: '',
  finalScore: undefined as number | undefined,
  performanceGrade: '',
  reviewerId: '',
  interviewDateStart: '',
  interviewDateEnd: '',
  interviewNotes: '',
  assessmentStatus: undefined as number | undefined,
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

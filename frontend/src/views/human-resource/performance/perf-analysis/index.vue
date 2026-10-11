<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-analysis -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：分析改进管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:performance:perf:analysis:create"
      update-permission="human:resource:performance:perf:analysis:update"
      delete-permission="human:resource:performance:perf:analysis:delete"
      import-permission="human:resource:performance:perf:analysis:import"
      export-permission="human:resource:performance:perf:analysis:export"
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
      entity-scope="approval"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'perfAnalysisId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPerfAnalysisId"
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
      <PerfAnalysisForm
        :key="formData?.perfAnalysisId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-performance-perf-analysis'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.perfanalysis.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.perfanalysis.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.employeename') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentId')">
      <a-form-item :label="t('entity.perfanalysis.assessmentid')">
        <a-input
          v-model:value="advancedQueryForm.assessmentId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.assessmentid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planTitle')">
      <a-form-item :label="t('entity.perfanalysis.plantitle')">
        <a-input
          v-model:value="advancedQueryForm.planTitle"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.plantitle') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementArea')">
      <a-form-item :label="t('entity.perfanalysis.improvementarea')">
        <a-input
          v-model:value="advancedQueryForm.improvementArea"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementarea') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currentSituation')">
      <a-form-item :label="t('entity.perfanalysis.currentsituation')">
        <a-input
          v-model:value="advancedQueryForm.currentSituation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.currentsituation') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementGoal')">
      <a-form-item :label="t('entity.perfanalysis.improvementgoal')">
        <a-input
          v-model:value="advancedQueryForm.improvementGoal"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementgoal') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementActions')">
      <a-form-item :label="t('entity.perfanalysis.improvementactions')">
        <a-input
          v-model:value="advancedQueryForm.improvementActions"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementactions') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateStart')">
      <a-form-item :label="t('entity.perfanalysis.plandatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.plandatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('planDateEnd')">
      <a-form-item :label="t('entity.perfanalysis.plandateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.planDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.plandateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetCompletionDateStart')">
      <a-form-item :label="t('entity.perfanalysis.targetcompletiondatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.targetCompletionDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.targetcompletiondatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('targetCompletionDateEnd')">
      <a-form-item :label="t('entity.perfanalysis.targetcompletiondateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.targetCompletionDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.targetcompletiondateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('progressPercentage')">
      <a-form-item :label="t('entity.perfanalysis.progresspercentage')">
        <a-input-number
          v-model:value="advancedQueryForm.progressPercentage"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.progresspercentage') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resultDescription')">
      <a-form-item :label="t('entity.perfanalysis.resultdescription')">
        <a-textarea
          v-model:value="advancedQueryForm.resultDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfanalysis.resultdescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mentorId')">
      <a-form-item :label="t('entity.perfanalysis.mentorid')">
        <a-input
          v-model:value="advancedQueryForm.mentorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.mentorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('improvementStatus')">
      <a-form-item :label="t('entity.perfanalysis.improvementstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.improvementStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.improvementstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.perfanalysis.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.relatedplant') })"
          show-count
          :maxlength="4"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.perfanalysis.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.perfanalysis.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.initiatorid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.perfanalysis.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.initiatedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.perfanalysis.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.perfanalysis.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.approvedby') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.perfanalysis.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.approvedatstart') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.perfanalysis.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfanalysis.approvedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('flowInstanceId')">
      <a-form-item :label="t('entity.perfanalysis.flowinstanceid')">
        <a-input
          v-model:value="advancedQueryForm.flowInstanceId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfanalysis.flowinstanceid') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.perfanalysis._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.perfanalysis._self"
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
      :id-column-key="'perfAnalysisId'"
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
 * 分析改进管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-analysis
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import PerfAnalysisForm from './components/PerfAnalysisForm.vue'
import { getPerfAnalysisList, getPerfAnalysisById, createPerfAnalysis, updatePerfAnalysis, deletePerfAnalysisById, deletePerfAnalysisBatch, getPerfAnalysisTemplate, importPerfAnalysis, exportPerfAnalysis, updatePerfAnalysisStatus } from '@/api/human-resource/performance/perfAnalysis.api'
import type { PerfAnalysis, PerfAnalysisQuery } from '@/types/human-resource/performance/perfAnalysis.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPerfAnalysis')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.perfanalysis._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PerfAnalysis[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<PerfAnalysis | null>(null)
/** 表格多选行 */
const selectedRows = ref<PerfAnalysis[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PerfAnalysis> | null>(null)
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
  assessmentId: '',
  planTitle: '',
  improvementArea: '',
  currentSituation: '',
  improvementGoal: '',
  improvementActions: '',
  planDateStart: '',
  planDateEnd: '',
  targetCompletionDateStart: '',
  targetCompletionDateEnd: '',
  progressPercentage: undefined as number | undefined,
  resultDescription: '',
  mentorId: '',
  improvementStatus: undefined as number | undefined,
  plantCode: '',
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
  { key: 'employeeId', label: t('entity.perfanalysis.employeeid') },
  { key: 'employeeName', label: t('entity.perfanalysis.employeename') },
  { key: 'assessmentId', label: t('entity.perfanalysis.assessmentid') },
  { key: 'planTitle', label: t('entity.perfanalysis.plantitle') },
  { key: 'improvementArea', label: t('entity.perfanalysis.improvementarea') },
  { key: 'currentSituation', label: t('entity.perfanalysis.currentsituation') },
  { key: 'improvementGoal', label: t('entity.perfanalysis.improvementgoal') },
  { key: 'improvementActions', label: t('entity.perfanalysis.improvementactions') },
  { key: 'planDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfanalysis.plandate')) },
  { key: 'planDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfanalysis.plandate')) },
  { key: 'targetCompletionDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfanalysis.targetcompletiondate')) },
  { key: 'targetCompletionDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfanalysis.targetcompletiondate')) },
  { key: 'progressPercentage', label: t('entity.perfanalysis.progresspercentage') },
  { key: 'resultDescription', label: t('entity.perfanalysis.resultdescription') },
  { key: 'mentorId', label: t('entity.perfanalysis.mentorid') },
  { key: 'improvementStatus', label: t('entity.perfanalysis.improvementstatus') },
  { key: 'plantCode', label: t('entity.perfanalysis.relatedplant') },
  { key: 'approvalStatus', label: t('entity.perfanalysis.approvalstatus') },
  { key: 'initiatorId', label: t('entity.perfanalysis.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.perfanalysis.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.perfanalysis.initiatedatend') },
  { key: 'approvedBy', label: t('entity.perfanalysis.approvedby') },
  { key: 'approvedAtStart', label: t('entity.perfanalysis.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.perfanalysis.approvedatend') },
  { key: 'flowInstanceId', label: t('entity.perfanalysis.flowinstanceid') },
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
const entityIdName = 'perfAnalysisId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PerfAnalysisQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PerfAnalysisQuery>): PerfAnalysisQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PerfAnalysisQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PerfAnalysisQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('employeeName', form.employeeName)
  assignTrimmed('assessmentId', form.assessmentId)
  assignTrimmed('planTitle', form.planTitle)
  assignTrimmed('improvementArea', form.improvementArea)
  assignTrimmed('currentSituation', form.currentSituation)
  assignTrimmed('improvementGoal', form.improvementGoal)
  assignTrimmed('improvementActions', form.improvementActions)
  assignTrimmed('planDateStart', form.planDateStart)
  assignTrimmed('planDateEnd', form.planDateEnd)
  assignTrimmed('targetCompletionDateStart', form.targetCompletionDateStart)
  assignTrimmed('targetCompletionDateEnd', form.targetCompletionDateEnd)
  if (form.progressPercentage !== undefined && form.progressPercentage !== null) {
    query.progressPercentage = form.progressPercentage
  }
  assignTrimmed('resultDescription', form.resultDescription)
  assignTrimmed('mentorId', form.mentorId)
  if (form.improvementStatus !== undefined && form.improvementStatus !== null) {
    query.improvementStatus = form.improvementStatus
  }
  assignTrimmed('plantCode', form.plantCode)
  if (form.approvalStatus !== undefined && form.approvalStatus !== null) {
    query.approvalStatus = form.approvalStatus
  }
  assignTrimmed('initiatorId', form.initiatorId)
  assignTrimmed('initiatedAtStart', form.initiatedAtStart)
  assignTrimmed('initiatedAtEnd', form.initiatedAtEnd)
  assignTrimmed('approvedBy', form.approvedBy)
  assignTrimmed('approvedAtStart', form.approvedAtStart)
  assignTrimmed('approvedAtEnd', form.approvedAtEnd)
  assignTrimmed('flowInstanceId', form.flowInstanceId)
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
    dataIndex: 'perfAnalysisId',
    key: 'perfAnalysisId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'perfAnalysisId') ?? ''
  },
  {
    title: t('entity.perfanalysis.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.perfanalysis.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.perfanalysis.assessmentid'),
    dataIndex: 'assessmentId',
    key: 'assessmentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'assessmentId') ?? ''
  },
  {
    title: t('entity.perfanalysis.plantitle'),
    dataIndex: 'planTitle',
    key: 'planTitle',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'planTitle') ?? ''
  },
  {
    title: t('entity.perfanalysis.improvementarea'),
    dataIndex: 'improvementArea',
    key: 'improvementArea',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'improvementArea') ?? ''
  },
  {
    title: t('entity.perfanalysis.currentsituation'),
    dataIndex: 'currentSituation',
    key: 'currentSituation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'currentSituation') ?? ''
  },
  {
    title: t('entity.perfanalysis.improvementgoal'),
    dataIndex: 'improvementGoal',
    key: 'improvementGoal',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'improvementGoal') ?? ''
  },
  {
    title: t('entity.perfanalysis.improvementactions'),
    dataIndex: 'improvementActions',
    key: 'improvementActions',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'improvementActions') ?? ''
  },
  {
    title: t('entity.perfanalysis.plandate'),
    dataIndex: 'planDate',
    key: 'planDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'planDate') ?? ''
  },
  {
    title: t('entity.perfanalysis.targetcompletiondate'),
    dataIndex: 'targetCompletionDate',
    key: 'targetCompletionDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'targetCompletionDate') ?? ''
  },
  {
    title: t('entity.perfanalysis.progresspercentage'),
    dataIndex: 'progressPercentage',
    key: 'progressPercentage',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'progressPercentage') ?? ''
  },
  {
    title: t('entity.perfanalysis.resultdescription'),
    dataIndex: 'resultDescription',
    key: 'resultDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'resultDescription') ?? ''
  },
  {
    title: t('entity.perfanalysis.mentorid'),
    dataIndex: 'mentorId',
    key: 'mentorId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'mentorId') ?? ''
  },
  {
    title: t('entity.perfanalysis.improvementstatus'),
    dataIndex: 'improvementStatus',
    key: 'improvementStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'improvementStatus') ?? ''
  },
  {
    title: t('entity.perfanalysis.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfAnalysisField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:performance:perf:analysis:update',
        onClick: (record: PerfAnalysis) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:performance:perf:analysis:delete',
        onClick: (record: PerfAnalysis) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPerfAnalysisId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPerfAnalysisField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PerfAnalysis[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PerfAnalysis, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getPerfAnalysisId(selectedRow.value) === getPerfAnalysisId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: PerfAnalysis[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PerfAnalysis) => ({
  onClick: () => {
    const key = getPerfAnalysisId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getPerfAnalysisId(item)))
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
    const res = await getPerfAnalysisList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PerfAnalysis] 加载数据失败', { error })
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
  assessmentId: '',
  planTitle: '',
  improvementArea: '',
  currentSituation: '',
  improvementGoal: '',
  improvementActions: '',
  planDateStart: '',
  planDateEnd: '',
  targetCompletionDateStart: '',
  targetCompletionDateEnd: '',
  progressPercentage: undefined as number | undefined,
  resultDescription: '',
  mentorId: '',
  improvementStatus: undefined as number | undefined,
  plantCode: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.perfanalysis._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: PerfAnalysis) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.perfanalysis._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.perfanalysis._self') }))
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
      await updatePerfAnalysis(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.perfanalysis._self') }))
    } else {
      await createPerfAnalysis(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.perfanalysis._self') }))
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
  const res = await getPerfAnalysisTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPerfAnalysis(file, sheetName)
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
    const exportMeta = await exportPerfAnalysis(
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
    message.success(t('common.feedback.export.success', { target: t('entity.perfanalysis._self') }))
  } catch (error: any) {
    logger.error('[PerfAnalysis] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.perfanalysis._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PerfAnalysis) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.perfanalysis._self'), name: t('common.tip.this.target', { target: t('entity.perfanalysis._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePerfAnalysisById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.perfanalysis._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.perfanalysis._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.perfanalysis._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePerfAnalysisBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.perfanalysis._self') }))
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
  assessmentId: '',
  planTitle: '',
  improvementArea: '',
  currentSituation: '',
  improvementGoal: '',
  improvementActions: '',
  planDateStart: '',
  planDateEnd: '',
  targetCompletionDateStart: '',
  targetCompletionDateEnd: '',
  progressPercentage: undefined as number | undefined,
  resultDescription: '',
  mentorId: '',
  improvementStatus: undefined as number | undefined,
  plantCode: '',
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

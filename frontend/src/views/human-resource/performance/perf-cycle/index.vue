<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/performance/perf-cycle -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：绩效考核周期日程安排管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:performance:perf:cycle:create"
      update-permission="human:resource:performance:perf:cycle:update"
      delete-permission="human:resource:performance:perf:cycle:delete"
      import-permission="human:resource:performance:perf:cycle:import"
      export-permission="human:resource:performance:perf:cycle:export"
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
      :id-column-key="'perfCycleId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getPerfCycleId"
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
      <PerfCycleForm
        :key="formData?.perfCycleId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-performance-perf-cycle'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cycleCode')">
      <a-form-item :label="t('entity.perfcycle.cyclecode')">
        <a-input
          v-model:value="advancedQueryForm.cycleCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cyclecode') })"
          show-count
          :maxlength="64"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleName')">
      <a-form-item :label="t('entity.perfcycle.cyclename')">
        <a-input
          v-model:value="advancedQueryForm.cycleName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cyclename') })"
          show-count
          :maxlength="128"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleType')">
      <a-form-item :label="t('entity.perfcycle.cycletype')">
        <a-input
          v-model:value="advancedQueryForm.cycleType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cycletype') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleYear')">
      <a-form-item :label="t('entity.perfcycle.cycleyear')">
        <a-input-number
          v-model:value="advancedQueryForm.cycleYear"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cycleyear') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleSequence')">
      <a-form-item :label="t('entity.perfcycle.cyclesequence')">
        <a-input-number
          v-model:value="advancedQueryForm.cycleSequence"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cyclesequence') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateStart')">
      <a-form-item :label="t('entity.perfcycle.startdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.startdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('startDateEnd')">
      <a-form-item :label="t('entity.perfcycle.startdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.startDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.startdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateStart')">
      <a-form-item :label="t('entity.perfcycle.enddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.enddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('endDateEnd')">
      <a-form-item :label="t('entity.perfcycle.enddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.endDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.enddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('goalSettingDueDateStart')">
      <a-form-item :label="t('entity.perfcycle.goalsettingduedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.goalSettingDueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.goalsettingduedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('goalSettingDueDateEnd')">
      <a-form-item :label="t('entity.perfcycle.goalsettingduedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.goalSettingDueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.goalsettingduedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationDueDateStart')">
      <a-form-item :label="t('entity.perfcycle.selfevaluationduedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.selfEvaluationDueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.selfevaluationduedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('selfEvaluationDueDateEnd')">
      <a-form-item :label="t('entity.perfcycle.selfevaluationduedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.selfEvaluationDueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.selfevaluationduedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorReviewDueDateStart')">
      <a-form-item :label="t('entity.perfcycle.supervisorreviewduedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.supervisorReviewDueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.supervisorreviewduedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supervisorReviewDueDateEnd')">
      <a-form-item :label="t('entity.perfcycle.supervisorreviewduedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.supervisorReviewDueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.supervisorreviewduedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDueDateStart')">
      <a-form-item :label="t('entity.perfcycle.interviewduedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.interviewduedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('interviewDueDateEnd')">
      <a-form-item :label="t('entity.perfcycle.interviewduedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.interviewDueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.interviewduedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resultConfirmationDueDateStart')">
      <a-form-item :label="t('entity.perfcycle.resultconfirmationduedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.resultConfirmationDueDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.resultconfirmationduedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('resultConfirmationDueDateEnd')">
      <a-form-item :label="t('entity.perfcycle.resultconfirmationduedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.resultConfirmationDueDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.perfcycle.resultconfirmationduedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('applicableDepartment')">
      <a-form-item :label="t('entity.perfcycle.applicabledepartment')">
        <a-input
          v-model:value="advancedQueryForm.applicableDepartment"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.applicabledepartment') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('description')">
      <a-form-item :label="t('entity.perfcycle.description')">
        <a-textarea
          v-model:value="advancedQueryForm.description"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.perfcycle.description') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cycleScheduleStatus')">
      <a-form-item :label="t('entity.perfcycle.cycleschedulestatus')">
        <a-input-number
          v-model:value="advancedQueryForm.cycleScheduleStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.perfcycle.cycleschedulestatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.perfcycle._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.perfcycle._self"
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
      :id-column-key="'perfCycleId'"
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
 * 绩效考核周期日程安排管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/performance/perf-cycle
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import PerfCycleForm from './components/PerfCycleForm.vue'
import { getPerfCycleList, getPerfCycleById, createPerfCycle, updatePerfCycle, deletePerfCycleById, deletePerfCycleBatch, getPerfCycleTemplate, importPerfCycle, exportPerfCycle, updatePerfCycleStatus } from '@/api/human-resource/performance/perfCycle.api'
import type { PerfCycle, PerfCycleQuery } from '@/types/human-resource/performance/perfCycle.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktPerfCycle')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.perfcycle._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<PerfCycle[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<PerfCycle | null>(null)
/** 表格多选行 */
const selecmedRows = ref<PerfCycle[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<PerfCycle> | null>(null)
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
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  goalSettingDueDateStart: '',
  goalSettingDueDateEnd: '',
  selfEvaluationDueDateStart: '',
  selfEvaluationDueDateEnd: '',
  supervisorReviewDueDateStart: '',
  supervisorReviewDueDateEnd: '',
  interviewDueDateStart: '',
  interviewDueDateEnd: '',
  resultConfirmationDueDateStart: '',
  resultConfirmationDueDateEnd: '',
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'cycleCode', label: t('entity.perfcycle.cyclecode') },
  { key: 'cycleName', label: t('entity.perfcycle.cyclename') },
  { key: 'cycleType', label: t('entity.perfcycle.cycletype') },
  { key: 'cycleYear', label: t('entity.perfcycle.cycleyear') },
  { key: 'cycleSequence', label: t('entity.perfcycle.cyclesequence') },
  { key: 'startDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.startdate')) },
  { key: 'startDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.startdate')) },
  { key: 'endDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.enddate')) },
  { key: 'endDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.enddate')) },
  { key: 'goalSettingDueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.goalsettingduedate')) },
  { key: 'goalSettingDueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.goalsettingduedate')) },
  { key: 'selfEvaluationDueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.selfevaluationduedate')) },
  { key: 'selfEvaluationDueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.selfevaluationduedate')) },
  { key: 'supervisorReviewDueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.supervisorreviewduedate')) },
  { key: 'supervisorReviewDueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.supervisorreviewduedate')) },
  { key: 'interviewDueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.interviewduedate')) },
  { key: 'interviewDueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.interviewduedate')) },
  { key: 'resultConfirmationDueDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.perfcycle.resultconfirmationduedate')) },
  { key: 'resultConfirmationDueDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.perfcycle.resultconfirmationduedate')) },
  { key: 'applicableDepartment', label: t('entity.perfcycle.applicabledepartment') },
  { key: 'description', label: t('entity.perfcycle.description') },
  { key: 'cycleScheduleStatus', label: t('entity.perfcycle.cycleschedulestatus') },
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
const entityIdName = 'perfCycleId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {PerfCycleQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<PerfCycleQuery>): PerfCycleQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: PerfCycleQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof PerfCycleQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('cycleCode', form.cycleCode)
  assignTrimmed('cycleName', form.cycleName)
  assignTrimmed('cycleType', form.cycleType)
  if (form.cycleYear !== undefined && form.cycleYear !== null) {
    query.cycleYear = form.cycleYear
  }
  if (form.cycleSequence !== undefined && form.cycleSequence !== null) {
    query.cycleSequence = form.cycleSequence
  }
  assignTrimmed('startDateStart', form.startDateStart)
  assignTrimmed('startDateEnd', form.startDateEnd)
  assignTrimmed('endDateStart', form.endDateStart)
  assignTrimmed('endDateEnd', form.endDateEnd)
  assignTrimmed('goalSettingDueDateStart', form.goalSettingDueDateStart)
  assignTrimmed('goalSettingDueDateEnd', form.goalSettingDueDateEnd)
  assignTrimmed('selfEvaluationDueDateStart', form.selfEvaluationDueDateStart)
  assignTrimmed('selfEvaluationDueDateEnd', form.selfEvaluationDueDateEnd)
  assignTrimmed('supervisorReviewDueDateStart', form.supervisorReviewDueDateStart)
  assignTrimmed('supervisorReviewDueDateEnd', form.supervisorReviewDueDateEnd)
  assignTrimmed('interviewDueDateStart', form.interviewDueDateStart)
  assignTrimmed('interviewDueDateEnd', form.interviewDueDateEnd)
  assignTrimmed('resultConfirmationDueDateStart', form.resultConfirmationDueDateStart)
  assignTrimmed('resultConfirmationDueDateEnd', form.resultConfirmationDueDateEnd)
  assignTrimmed('applicableDepartment', form.applicableDepartment)
  assignTrimmed('description', form.description)
  if (form.cycleScheduleStatus !== undefined && form.cycleScheduleStatus !== null) {
    query.cycleScheduleStatus = form.cycleScheduleStatus
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
    dataIndex: 'perfCycleId',
    key: 'perfCycleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'perfCycleId') ?? ''
  },
  {
    title: t('entity.perfcycle.cyclecode'),
    dataIndex: 'cycleCode',
    key: 'cycleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleCode') ?? ''
  },
  {
    title: t('entity.perfcycle.cyclename'),
    dataIndex: 'cycleName',
    key: 'cycleName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleName') ?? ''
  },
  {
    title: t('entity.perfcycle.cycletype'),
    dataIndex: 'cycleType',
    key: 'cycleType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleType') ?? ''
  },
  {
    title: t('entity.perfcycle.cycleyear'),
    dataIndex: 'cycleYear',
    key: 'cycleYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleYear') ?? ''
  },
  {
    title: t('entity.perfcycle.cyclesequence'),
    dataIndex: 'cycleSequence',
    key: 'cycleSequence',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleSequence') ?? ''
  },
  {
    title: t('entity.perfcycle.startdate'),
    dataIndex: 'startDate',
    key: 'startDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'startDate') ?? ''
  },
  {
    title: t('entity.perfcycle.enddate'),
    dataIndex: 'endDate',
    key: 'endDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'endDate') ?? ''
  },
  {
    title: t('entity.perfcycle.goalsettingduedate'),
    dataIndex: 'goalSettingDueDate',
    key: 'goalSettingDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'goalSettingDueDate') ?? ''
  },
  {
    title: t('entity.perfcycle.selfevaluationduedate'),
    dataIndex: 'selfEvaluationDueDate',
    key: 'selfEvaluationDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'selfEvaluationDueDate') ?? ''
  },
  {
    title: t('entity.perfcycle.supervisorreviewduedate'),
    dataIndex: 'supervisorReviewDueDate',
    key: 'supervisorReviewDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'supervisorReviewDueDate') ?? ''
  },
  {
    title: t('entity.perfcycle.interviewduedate'),
    dataIndex: 'interviewDueDate',
    key: 'interviewDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'interviewDueDate') ?? ''
  },
  {
    title: t('entity.perfcycle.resultconfirmationduedate'),
    dataIndex: 'resultConfirmationDueDate',
    key: 'resultConfirmationDueDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'resultConfirmationDueDate') ?? ''
  },
  {
    title: t('entity.perfcycle.applicabledepartment'),
    dataIndex: 'applicableDepartment',
    key: 'applicableDepartment',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'applicableDepartment') ?? ''
  },
  {
    title: t('entity.perfcycle.description'),
    dataIndex: 'description',
    key: 'description',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'description') ?? ''
  },
  {
    title: t('entity.perfcycle.cycleschedulestatus'),
    dataIndex: 'cycleScheduleStatus',
    key: 'cycleScheduleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'cycleScheduleStatus') ?? ''
  },
  {
    title: t('entity.perfcycle.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getPerfCycleField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:performance:perf:cycle:update',
        onClick: (record: PerfCycle) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:performance:perf:cycle:delete',
        onClick: (record: PerfCycle) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getPerfCycleId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getPerfCycleField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: PerfCycle[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: PerfCycle, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getPerfCycleId(selecmedRow.value) === getPerfCycleId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: PerfCycle[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: PerfCycle) => ({
  onClick: () => {
    const key = getPerfCycleId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getPerfCycleId(item)))
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
    const res = await getPerfCycleList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[PerfCycle] 加载数据失败', { error })
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
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  goalSettingDueDateStart: '',
  goalSettingDueDateEnd: '',
  selfEvaluationDueDateStart: '',
  selfEvaluationDueDateEnd: '',
  supervisorReviewDueDateStart: '',
  supervisorReviewDueDateEnd: '',
  interviewDueDateStart: '',
  interviewDueDateEnd: '',
  resultConfirmationDueDateStart: '',
  resultConfirmationDueDateEnd: '',
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.perfcycle._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: PerfCycle) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.perfcycle._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.perfcycle._self') }))
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
      await updatePerfCycle(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.perfcycle._self') }))
    } else {
      await createPerfCycle(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.perfcycle._self') }))
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
  const res = await getPerfCycleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importPerfCycle(file, sheetName)
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
    const exportMeta = await exportPerfCycle(
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
    message.success(t('common.feedback.export.success', { target: t('entity.perfcycle._self') }))
  } catch (error: any) {
    logger.error('[PerfCycle] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.perfcycle._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: PerfCycle) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.perfcycle._self'), name: t('common.tip.this.target', { target: t('entity.perfcycle._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deletePerfCycleById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.perfcycle._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.perfcycle._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.perfcycle._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deletePerfCycleBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.perfcycle._self') }))
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
  cycleCode: '',
  cycleName: '',
  cycleType: '',
  cycleYear: undefined as number | undefined,
  cycleSequence: undefined as number | undefined,
  startDateStart: '',
  startDateEnd: '',
  endDateStart: '',
  endDateEnd: '',
  goalSettingDueDateStart: '',
  goalSettingDueDateEnd: '',
  selfEvaluationDueDateStart: '',
  selfEvaluationDueDateEnd: '',
  supervisorReviewDueDateStart: '',
  supervisorReviewDueDateEnd: '',
  interviewDueDateStart: '',
  interviewDueDateEnd: '',
  resultConfirmationDueDateStart: '',
  resultConfirmationDueDateEnd: '',
  applicableDepartment: '',
  description: '',
  cycleScheduleStatus: undefined as number | undefined,
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

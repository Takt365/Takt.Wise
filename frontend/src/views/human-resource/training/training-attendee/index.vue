<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-attendee -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：员工培训结果记录管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:training:attendee:create"
      update-permission="human:resource:training:attendee:update"
      delete-permission="human:resource:training:attendee:delete"
      import-permission="human:resource:training:attendee:import"
      export-permission="human:resource:training:attendee:export"
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
      :id-column-key="'trainingAttendeeId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTrainingAttendeeId"
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
      <TrainingAttendeeForm
        :key="formData?.trainingAttendeeId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-training-training-attendee'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('employeeId')">
      <a-form-item :label="t('entity.trainingattendee.employeeid')">
        <a-input
          v-model:value="advancedQueryForm.employeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeeid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('employeeName')">
      <a-form-item :label="t('entity.trainingattendee.employeename')">
        <a-input
          v-model:value="advancedQueryForm.employeeName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.employeename') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingCourseId')">
      <a-form-item :label="t('entity.trainingattendee.trainingcourseid')">
        <a-input
          v-model:value="advancedQueryForm.trainingCourseId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingcourseid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseName')">
      <a-form-item :label="t('entity.trainingattendee.coursename')">
        <a-input
          v-model:value="advancedQueryForm.courseName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.coursename') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingType')">
      <a-form-item :label="t('entity.trainingattendee.trainingtype')">
        <a-input
          v-model:value="advancedQueryForm.trainingType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingtype') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('instructor')">
      <a-form-item :label="t('entity.trainingattendee.instructor')">
        <a-input
          v-model:value="advancedQueryForm.instructor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.instructor') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingStartDateStart')">
      <a-form-item :label="t('entity.trainingattendee.trainingstartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingstartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingStartDateEnd')">
      <a-form-item :label="t('entity.trainingattendee.trainingstartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingstartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingEndDateStart')">
      <a-form-item :label="t('entity.trainingattendee.trainingenddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingEndDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingenddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingEndDateEnd')">
      <a-form-item :label="t('entity.trainingattendee.trainingenddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingEndDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingenddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingDateStart')">
      <a-form-item :label="t('entity.trainingattendee.trainingdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingDateEnd')">
      <a-form-item :label="t('entity.trainingattendee.trainingdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.trainingDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingattendee.trainingdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingHours')">
      <a-form-item :label="t('entity.trainingattendee.traininghours')">
        <a-input-number
          v-model:value="advancedQueryForm.trainingHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.traininghours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingScore')">
      <a-form-item :label="t('entity.trainingattendee.trainingscore')">
        <a-input-number
          v-model:value="advancedQueryForm.trainingScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isPassed')">
      <a-form-item :label="t('entity.trainingattendee.ispassed')">
        <a-input-number
          v-model:value="advancedQueryForm.isPassed"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.ispassed') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('CertificateCode')">
      <a-form-item :label="t('entity.trainingattendee.CertificateCode')">
        <a-input
          v-model:value="advancedQueryForm.CertificateCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.CertificateCode') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingEvaluation')">
      <a-form-item :label="t('entity.trainingattendee.trainingevaluation')">
        <a-input
          v-model:value="advancedQueryForm.trainingEvaluation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingevaluation') })"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingResultStatus')">
      <a-form-item :label="t('entity.trainingattendee.trainingresultstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.trainingResultStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.trainingresultstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.trainingattendee.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingattendee.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.trainingattendee._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.trainingattendee._self"
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
      :id-column-key="'trainingAttendeeId'"
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
 * 员工培训结果记录管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training/training-attendee
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import TrainingAttendeeForm from './components/AttendeeForm.vue'
import { getTrainingAttendeeList, getTrainingAttendeeById, createTrainingAttendee, updateTrainingAttendee, deleteTrainingAttendeeById, deleteTrainingAttendeeBatch, getTrainingAttendeeTemplate, importTrainingAttendee, exportTrainingAttendee, updateTrainingAttendeeStatus } from '@/api/human-resource/training/attendee.api'
import type { TrainingAttendee, TrainingAttendeeQuery } from '@/types/human-resource/training/attendee.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTrainingAttendee')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.trainingattendee._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<TrainingAttendee[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<TrainingAttendee | null>(null)
/** 表格多选行 */
const selectedRows = ref<TrainingAttendee[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<TrainingAttendee> | null>(null)
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
  trainingCourseId: '',
  courseName: '',
  trainingType: '',
  instructor: '',
  trainingStartDateStart: '',
  trainingStartDateEnd: '',
  trainingEndDateStart: '',
  trainingEndDateEnd: '',
  trainingDateStart: '',
  trainingDateEnd: '',
  trainingHours: undefined as number | undefined,
  trainingScore: undefined as number | undefined,
  isPassed: undefined as number | undefined,
  CertificateCode: '',
  trainingEvaluation: '',
  trainingResultStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'employeeId', label: t('entity.trainingattendee.employeeid') },
  { key: 'employeeName', label: t('entity.trainingattendee.employeename') },
  { key: 'trainingCourseId', label: t('entity.trainingattendee.trainingcourseid') },
  { key: 'courseName', label: t('entity.trainingattendee.coursename') },
  { key: 'trainingType', label: t('entity.trainingattendee.trainingtype') },
  { key: 'instructor', label: t('entity.trainingattendee.instructor') },
  { key: 'trainingStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingstartdate')) },
  { key: 'trainingStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingstartdate')) },
  { key: 'trainingEndDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingenddate')) },
  { key: 'trainingEndDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingenddate')) },
  { key: 'trainingDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingdate')) },
  { key: 'trainingDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.trainingattendee.trainingdate')) },
  { key: 'trainingHours', label: t('entity.trainingattendee.traininghours') },
  { key: 'trainingScore', label: t('entity.trainingattendee.trainingscore') },
  { key: 'isPassed', label: t('entity.trainingattendee.ispassed') },
  { key: 'CertificateCode', label: t('entity.trainingattendee.CertificateCode') },
  { key: 'trainingEvaluation', label: t('entity.trainingattendee.trainingevaluation') },
  { key: 'trainingResultStatus', label: t('entity.trainingattendee.trainingresultstatus') },
  { key: 'plantCode', label: t('entity.trainingattendee.relatedplant') },
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
const entityIdName = 'trainingAttendeeId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TrainingAttendeeQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<TrainingAttendeeQuery>): TrainingAttendeeQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TrainingAttendeeQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof TrainingAttendeeQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('employeeId', form.employeeId)
  assignTrimmed('employeeName', form.employeeName)
  assignTrimmed('trainingCourseId', form.trainingCourseId)
  assignTrimmed('courseName', form.courseName)
  assignTrimmed('trainingType', form.trainingType)
  assignTrimmed('instructor', form.instructor)
  assignTrimmed('trainingStartDateStart', form.trainingStartDateStart)
  assignTrimmed('trainingStartDateEnd', form.trainingStartDateEnd)
  assignTrimmed('trainingEndDateStart', form.trainingEndDateStart)
  assignTrimmed('trainingEndDateEnd', form.trainingEndDateEnd)
  assignTrimmed('trainingDateStart', form.trainingDateStart)
  assignTrimmed('trainingDateEnd', form.trainingDateEnd)
  if (form.trainingHours !== undefined && form.trainingHours !== null) {
    query.trainingHours = form.trainingHours
  }
  if (form.trainingScore !== undefined && form.trainingScore !== null) {
    query.trainingScore = form.trainingScore
  }
  if (form.isPassed !== undefined && form.isPassed !== null) {
    query.isPassed = form.isPassed
  }
  assignTrimmed('CertificateCode', form.CertificateCode)
  assignTrimmed('trainingEvaluation', form.trainingEvaluation)
  if (form.trainingResultStatus !== undefined && form.trainingResultStatus !== null) {
    query.trainingResultStatus = form.trainingResultStatus
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
    dataIndex: 'trainingAttendeeId',
    key: 'trainingAttendeeId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingAttendeeId') ?? ''
  },
  {
    title: t('entity.trainingattendee.employeeid'),
    dataIndex: 'employeeId',
    key: 'employeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'employeeId') ?? ''
  },
  {
    title: t('entity.trainingattendee.employeename'),
    dataIndex: 'employeeName',
    key: 'employeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'employeeName') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingcourseid'),
    dataIndex: 'trainingCourseId',
    key: 'trainingCourseId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingCourseId') ?? ''
  },
  {
    title: t('entity.trainingattendee.coursename'),
    dataIndex: 'courseName',
    key: 'courseName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'courseName') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingtype'),
    dataIndex: 'trainingType',
    key: 'trainingType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingType') ?? ''
  },
  {
    title: t('entity.trainingattendee.instructor'),
    dataIndex: 'instructor',
    key: 'instructor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'instructor') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingstartdate'),
    dataIndex: 'trainingStartDate',
    key: 'trainingStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingStartDate') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingenddate'),
    dataIndex: 'trainingEndDate',
    key: 'trainingEndDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingEndDate') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingdate'),
    dataIndex: 'trainingDate',
    key: 'trainingDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingDate') ?? ''
  },
  {
    title: t('entity.trainingattendee.traininghours'),
    dataIndex: 'trainingHours',
    key: 'trainingHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingHours') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingscore'),
    dataIndex: 'trainingScore',
    key: 'trainingScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingScore') ?? ''
  },
  {
    title: t('entity.trainingattendee.ispassed'),
    dataIndex: 'isPassed',
    key: 'isPassed',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'isPassed') ?? ''
  },
  {
    title: t('entity.trainingattendee.CertificateCode'),
    dataIndex: 'CertificateCode',
    key: 'CertificateCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'CertificateCode') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingevaluation'),
    dataIndex: 'trainingEvaluation',
    key: 'trainingEvaluation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingEvaluation') ?? ''
  },
  {
    title: t('entity.trainingattendee.trainingresultstatus'),
    dataIndex: 'trainingResultStatus',
    key: 'trainingResultStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'trainingResultStatus') ?? ''
  },
  {
    title: t('entity.trainingattendee.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingAttendeeField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:training:attendee:update',
        onClick: (record: TrainingAttendee) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:training:attendee:delete',
        onClick: (record: TrainingAttendee) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTrainingAttendeeId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTrainingAttendeeField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TrainingAttendee[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TrainingAttendee, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getTrainingAttendeeId(selectedRow.value) === getTrainingAttendeeId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TrainingAttendee[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: TrainingAttendee) => ({
  onClick: () => {
    const key = getTrainingAttendeeId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTrainingAttendeeId(item)))
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
    const res = await getTrainingAttendeeList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TrainingAttendee] 加载数据失败', { error })
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
  trainingCourseId: '',
  courseName: '',
  trainingType: '',
  instructor: '',
  trainingStartDateStart: '',
  trainingStartDateEnd: '',
  trainingEndDateStart: '',
  trainingEndDateEnd: '',
  trainingDateStart: '',
  trainingDateEnd: '',
  trainingHours: undefined as number | undefined,
  trainingScore: undefined as number | undefined,
  isPassed: undefined as number | undefined,
  CertificateCode: '',
  trainingEvaluation: '',
  trainingResultStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.trainingattendee._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: TrainingAttendee) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.trainingattendee._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.trainingattendee._self') }))
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
      await updateTrainingAttendee(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.trainingattendee._self') }))
    } else {
      await createTrainingAttendee(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.trainingattendee._self') }))
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
  const res = await getTrainingAttendeeTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTrainingAttendee(file, sheetName)
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
    const exportMeta = await exportTrainingAttendee(
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
    message.success(t('common.feedback.export.success', { target: t('entity.trainingattendee._self') }))
  } catch (error: any) {
    logger.error('[TrainingAttendee] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.trainingattendee._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: TrainingAttendee) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.trainingattendee._self'), name: t('common.tip.this.target', { target: t('entity.trainingattendee._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTrainingAttendeeById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.trainingattendee._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.trainingattendee._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.trainingattendee._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTrainingAttendeeBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.trainingattendee._self') }))
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
  trainingCourseId: '',
  courseName: '',
  trainingType: '',
  instructor: '',
  trainingStartDateStart: '',
  trainingStartDateEnd: '',
  trainingEndDateStart: '',
  trainingEndDateEnd: '',
  trainingDateStart: '',
  trainingDateEnd: '',
  trainingHours: undefined as number | undefined,
  trainingScore: undefined as number | undefined,
  isPassed: undefined as number | undefined,
  CertificateCode: '',
  trainingEvaluation: '',
  trainingResultStatus: undefined as number | undefined,
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/training/training-course -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：培训课程定义管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:training:course:create"
      update-permission="human:resource:training:course:update"
      delete-permission="human:resource:training:course:delete"
      import-permission="human:resource:training:course:import"
      export-permission="human:resource:training:course:export"
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
      :id-column-key="'trainingCourseId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTrainingCourseId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'trainingCourseStatus'">
          <a-switch
            :checked="getTrainingCourseField(record, 'trainingCourseStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleTrainingCourseStatusChange(record, Boolean(checked))"
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
      <TrainingCourseForm
        :key="formData?.trainingCourseId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-training-training-course'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('courseCode')">
      <a-form-item :label="t('entity.trainingcourse.coursecode')">
        <a-input
          v-model:value="advancedQueryForm.courseCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursecode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseName')">
      <a-form-item :label="t('entity.trainingcourse.coursename')">
        <a-input
          v-model:value="advancedQueryForm.courseName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursename') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseType')">
      <a-form-item :label="t('entity.trainingcourse.coursetype')">
        <a-input
          v-model:value="advancedQueryForm.courseType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.coursetype') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseLevel')">
      <a-form-item :label="t('entity.trainingcourse.courselevel')">
        <a-input
          v-model:value="advancedQueryForm.courseLevel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courselevel') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseDescription')">
      <a-form-item :label="t('entity.trainingcourse.coursedescription')">
        <a-textarea
          v-model:value="advancedQueryForm.courseDescription"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.trainingcourse.coursedescription') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('courseObjectives')">
      <a-form-item :label="t('entity.trainingcourse.courseobjectives')">
        <a-input
          v-model:value="advancedQueryForm.courseObjectives"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.courseobjectives') })"
          show-count
          :maxlength="1000"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingHours')">
      <a-form-item :label="t('entity.trainingcourse.traininghours')">
        <a-input-number
          v-model:value="advancedQueryForm.trainingHours"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.traininghours') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainInstructor')">
      <a-form-item :label="t('entity.trainingcourse.maininstructor')">
        <a-input
          v-model:value="advancedQueryForm.mainInstructor"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.maininstructor') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingMethod')">
      <a-form-item :label="t('entity.trainingcourse.trainingmethod')">
        <a-input
          v-model:value="advancedQueryForm.trainingMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.trainingmethod') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assessmentMethod')">
      <a-form-item :label="t('entity.trainingcourse.assessmentmethod')">
        <a-input
          v-model:value="advancedQueryForm.assessmentMethod"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.assessmentmethod') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('passingScore')">
      <a-form-item :label="t('entity.trainingcourse.passingscore')">
        <a-input-number
          v-model:value="advancedQueryForm.passingScore"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.passingscore') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('trainingCourseStatus')">
      <a-form-item :label="t('entity.trainingcourse.status')">
        <TaktSelect
          v-model:value="advancedQueryForm.trainingCourseStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.trainingcourse.status') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('plantCode')">
      <a-form-item :label="t('entity.trainingcourse.relatedplant')">
        <a-input
          v-model:value="advancedQueryForm.plantCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.trainingcourse.relatedplant') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.trainingcourse._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.trainingcourse._self"
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
      :id-column-key="'trainingCourseId'"
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
 * 培训课程定义管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/training/training-course
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import TrainingCourseForm from './components/CourseForm.vue'
import { getTrainingCourseList, getTrainingCourseById, createTrainingCourse, updateTrainingCourse, deleteTrainingCourseById, deleteTrainingCourseBatch, getTrainingCourseTemplate, importTrainingCourse, exportTrainingCourse, updateTrainingCourseStatus } from '@/api/human-resource/training/course.api'
import type { TrainingCourse, TrainingCourseQuery } from '@/types/human-resource/training/course.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTrainingCourse')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.trainingcourse._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<TrainingCourse[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<TrainingCourse | null>(null)
/** 表格多选行 */
const selectedRows = ref<TrainingCourse[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<TrainingCourse> | null>(null)
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
  courseCode: '',
  courseName: '',
  courseType: '',
  courseLevel: '',
  courseDescription: '',
  courseObjectives: '',
  trainingHours: undefined as number | undefined,
  mainInstructor: '',
  trainingMethod: '',
  assessmentMethod: '',
  passingScore: undefined as number | undefined,
  trainingCourseStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'courseCode', label: t('entity.trainingcourse.coursecode') },
  { key: 'courseName', label: t('entity.trainingcourse.coursename') },
  { key: 'courseType', label: t('entity.trainingcourse.coursetype') },
  { key: 'courseLevel', label: t('entity.trainingcourse.courselevel') },
  { key: 'courseDescription', label: t('entity.trainingcourse.coursedescription') },
  { key: 'courseObjectives', label: t('entity.trainingcourse.courseobjectives') },
  { key: 'trainingHours', label: t('entity.trainingcourse.traininghours') },
  { key: 'mainInstructor', label: t('entity.trainingcourse.maininstructor') },
  { key: 'trainingMethod', label: t('entity.trainingcourse.trainingmethod') },
  { key: 'assessmentMethod', label: t('entity.trainingcourse.assessmentmethod') },
  { key: 'passingScore', label: t('entity.trainingcourse.passingscore') },
  { key: 'trainingCourseStatus', label: t('entity.trainingcourse.status') },
  { key: 'plantCode', label: t('entity.trainingcourse.relatedplant') },
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
const entityIdName = 'trainingCourseId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {TrainingCourseQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<TrainingCourseQuery>): TrainingCourseQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: TrainingCourseQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof TrainingCourseQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('courseCode', form.courseCode)
  assignTrimmed('courseName', form.courseName)
  assignTrimmed('courseType', form.courseType)
  assignTrimmed('courseLevel', form.courseLevel)
  assignTrimmed('courseDescription', form.courseDescription)
  assignTrimmed('courseObjectives', form.courseObjectives)
  if (form.trainingHours !== undefined && form.trainingHours !== null) {
    query.trainingHours = form.trainingHours
  }
  assignTrimmed('mainInstructor', form.mainInstructor)
  assignTrimmed('trainingMethod', form.trainingMethod)
  assignTrimmed('assessmentMethod', form.assessmentMethod)
  if (form.passingScore !== undefined && form.passingScore !== null) {
    query.passingScore = form.passingScore
  }
  if (form.trainingCourseStatus !== undefined && form.trainingCourseStatus !== null) {
    query.trainingCourseStatus = form.trainingCourseStatus
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
    dataIndex: 'trainingCourseId',
    key: 'trainingCourseId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'trainingCourseId') ?? ''
  },
  {
    title: t('entity.trainingcourse.coursecode'),
    dataIndex: 'courseCode',
    key: 'courseCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseCode') ?? ''
  },
  {
    title: t('entity.trainingcourse.coursename'),
    dataIndex: 'courseName',
    key: 'courseName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseName') ?? ''
  },
  {
    title: t('entity.trainingcourse.coursetype'),
    dataIndex: 'courseType',
    key: 'courseType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseType') ?? ''
  },
  {
    title: t('entity.trainingcourse.courselevel'),
    dataIndex: 'courseLevel',
    key: 'courseLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseLevel') ?? ''
  },
  {
    title: t('entity.trainingcourse.coursedescription'),
    dataIndex: 'courseDescription',
    key: 'courseDescription',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseDescription') ?? ''
  },
  {
    title: t('entity.trainingcourse.courseobjectives'),
    dataIndex: 'courseObjectives',
    key: 'courseObjectives',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'courseObjectives') ?? ''
  },
  {
    title: t('entity.trainingcourse.traininghours'),
    dataIndex: 'trainingHours',
    key: 'trainingHours',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'trainingHours') ?? ''
  },
  {
    title: t('entity.trainingcourse.maininstructor'),
    dataIndex: 'mainInstructor',
    key: 'mainInstructor',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'mainInstructor') ?? ''
  },
  {
    title: t('entity.trainingcourse.trainingmethod'),
    dataIndex: 'trainingMethod',
    key: 'trainingMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'trainingMethod') ?? ''
  },
  {
    title: t('entity.trainingcourse.assessmentmethod'),
    dataIndex: 'assessmentMethod',
    key: 'assessmentMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'assessmentMethod') ?? ''
  },
  {
    title: t('entity.trainingcourse.passingscore'),
    dataIndex: 'passingScore',
    key: 'passingScore',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'passingScore') ?? ''
  },
  {
    title: t('entity.trainingcourse.status'),
    dataIndex: 'trainingCourseStatus',
    key: 'trainingCourseStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.trainingcourse.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTrainingCourseField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:training:course:update',
        onClick: (record: TrainingCourse) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:training:course:delete',
        onClick: (record: TrainingCourse) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTrainingCourseId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTrainingCourseField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TrainingCourse[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TrainingCourse, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getTrainingCourseId(selectedRow.value) === getTrainingCourseId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: TrainingCourse[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: TrainingCourse) => ({
  onClick: () => {
    const key = getTrainingCourseId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getTrainingCourseId(item)))
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
    const res = await getTrainingCourseList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TrainingCourse] 加载数据失败', { error })
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
  courseCode: '',
  courseName: '',
  courseType: '',
  courseLevel: '',
  courseDescription: '',
  courseObjectives: '',
  trainingHours: undefined as number | undefined,
  mainInstructor: '',
  trainingMethod: '',
  assessmentMethod: '',
  passingScore: undefined as number | undefined,
  trainingCourseStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.trainingcourse._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: TrainingCourse) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.trainingcourse._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.trainingcourse._self') }))
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
      await updateTrainingCourse(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.trainingcourse._self') }))
    } else {
      await createTrainingCourse(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.trainingcourse._self') }))
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
  const res = await getTrainingCourseTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTrainingCourse(file, sheetName)
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
    const exportMeta = await exportTrainingCourse(
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
    message.success(t('common.feedback.export.success', { target: t('entity.trainingcourse._self') }))
  } catch (error: any) {
    logger.error('[TrainingCourse] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.trainingcourse._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: TrainingCourse) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.trainingcourse._self'), name: t('common.tip.this.target', { target: t('entity.trainingcourse._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTrainingCourseById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.trainingcourse._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.trainingcourse._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.trainingcourse._self'), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTrainingCourseBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.trainingcourse._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleTrainingCourseStatusChange(record: TrainingCourse, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getTrainingCourseField(record, 'trainingCourseStatus')
  const id = getTrainingCourseId(record)
  const row = dataSource.value.find((item) => getTrainingCourseId(item) === id)
  if (row) {
    row.trainingCourseStatus = newVal
  }
  try {
    await updateTrainingCourseStatus({ trainingCourseId: id, trainingCourseStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.trainingCourseStatus = oldVal
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
  courseCode: '',
  courseName: '',
  courseType: '',
  courseLevel: '',
  courseDescription: '',
  courseObjectives: '',
  trainingHours: undefined as number | undefined,
  mainInstructor: '',
  trainingMethod: '',
  assessmentMethod: '',
  passingScore: undefined as number | undefined,
  trainingCourseStatus: undefined as number | undefined,
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

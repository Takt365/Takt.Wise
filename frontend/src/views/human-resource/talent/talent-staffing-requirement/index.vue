<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/talent-staffing-requirement -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：用人需求管理页面，含查询、增删改，由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-talent-talent-staffing-requirement">
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
      create-permission="human:resource:talent:talentstaffingrequirement:create"
      update-permission="human:resource:talent:talentstaffingrequirement:update"
      delete-permission="human:resource:talent:talentstaffingrequirement:delete"
      import-permission="human:resource:talent:talentstaffingrequirement:import"
      export-permission="human:resource:talent:talentstaffingrequirement:export"
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
      :columns="columns"
      entity-scope="approval"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'talentStaffingRequirementId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTalentStaffingRequirementId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    />

    <!-- 分页组件 -->
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
      <TalentStaffingRequirementForm
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
      :storage-key="'takt-query-fields-human-resource-talent-talent-staffing-requirement'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('reqCode')">
      <a-form-item :label="t('entity.talentStaffingRequirement.reqno')">
        <a-input
          v-model:value="advancedQueryForm.reqCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reqno') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deptId')">
      <a-form-item :label="t('entity.talentStaffingRequirement.deptid')">
        <a-input
          v-model:value="advancedQueryForm.deptId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.deptid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postId')">
      <a-form-item :label="t('entity.talentStaffingRequirement.postid')">
        <a-input
          v-model:value="advancedQueryForm.postId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.postid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobGrade')">
      <a-form-item :label="t('entity.talentStaffingRequirement.jobgrade')">
        <a-input
          v-model:value="advancedQueryForm.jobGrade"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.jobgrade') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('requestQty')">
      <a-form-item :label="t('entity.talentStaffingRequirement.requestqty')">
        <a-input-number
          v-model:value="advancedQueryForm.requestQty"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.requestqty') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('headcountType')">
      <a-form-item :label="t('entity.talentStaffingRequirement.headcounttype')">
        <a-input
          v-model:value="advancedQueryForm.headcountType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.headcounttype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reasonCode')">
      <a-form-item :label="t('entity.talentStaffingRequirement.reasoncode')">
        <a-input
          v-model:value="advancedQueryForm.reasonCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.reasoncode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('replaceEmployeeId')">
      <a-form-item :label="t('entity.talentStaffingRequirement.replaceemployeeid')">
        <a-input
          v-model:value="advancedQueryForm.replaceEmployeeId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.replaceemployeeid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedOnboardDateStart')">
      <a-form-item :label="t('entity.talentStaffingRequirement.expectedonboarddatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedOnboardDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.expectedonboarddatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expectedOnboardDateEnd')">
      <a-form-item :label="t('entity.talentStaffingRequirement.expectedonboarddateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.expectedOnboardDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.expectedonboarddateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contractType')">
      <a-form-item :label="t('entity.talentStaffingRequirement.contracttype')">
        <a-input
          v-model:value="advancedQueryForm.contractType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.contracttype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('workLocation')">
      <a-form-item :label="t('entity.talentStaffingRequirement.worklocation')">
        <a-input
          v-model:value="advancedQueryForm.workLocation"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.worklocation') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('jobDesc')">
      <a-form-item :label="t('entity.talentStaffingRequirement.jobdesc')">
        <a-input
          v-model:value="advancedQueryForm.jobDesc"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.jobdesc') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualification')">
      <a-form-item :label="t('entity.talentStaffingRequirement.qualification')">
        <a-input
          v-model:value="advancedQueryForm.qualification"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.qualification') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('budgetYear')">
      <a-form-item :label="t('entity.talentStaffingRequirement.budgetyear')">
        <a-input
          v-model:value="advancedQueryForm.budgetYear"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.budgetyear') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvalStatus')">
      <a-form-item :label="t('entity.talentStaffingRequirement.approvalstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.approvalStatus"
          dict-type="sys_approval_status"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.approvalstatus') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatorId')">
      <a-form-item :label="t('entity.talentStaffingRequirement.initiatorid')">
        <a-input
          v-model:value="advancedQueryForm.initiatorId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.initiatorid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtStart')">
      <a-form-item :label="t('entity.talentStaffingRequirement.initiatedatstart')">
        <a-input
          v-model:value="advancedQueryForm.initiatedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.initiatedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('initiatedAtEnd')">
      <a-form-item :label="t('entity.talentStaffingRequirement.initiatedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.initiatedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.initiatedatend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBy')">
      <a-form-item :label="t('entity.talentStaffingRequirement.approvedby')">
        <a-input
          v-model:value="advancedQueryForm.approvedBy"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.approvedby') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtStart')">
      <a-form-item :label="t('entity.talentStaffingRequirement.approvedatstart')">
        <a-input
          v-model:value="advancedQueryForm.approvedAtStart"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentStaffingRequirement.approvedatstart') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedAtEnd')">
      <a-form-item :label="t('entity.talentStaffingRequirement.approvedatend')">
        <a-date-picker
          v-model:value="advancedQueryForm.approvedAtEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentStaffingRequirement.approvedatend') })"
          value-format="YYYY-MM-DD"
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
      <div v-show="isFieldVisible('ExtField')">
      <a-form-item :label="t('common.page.entity.extfield')">
        <a-input
          v-model:value="advancedQueryForm.ExtField"
          :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfield') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="t('common.page.entity.remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') })"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: t('entity.talentStaffingRequirement._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.talentStaffingRequirement._self"
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
      :id-column-key="'talentStaffingRequirementId'"
      :action-column-key="'action'"
      entity-scope="approval"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 用人需求管理页 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/talent/talent-staffing-requirement
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import TalentStaffingRequirementForm from './components/TalentStaffingRequirementForm.vue'
import { getTalentStaffingRequirementList, getTalentStaffingRequirementById, createTalentStaffingRequirement, updateTalentStaffingRequirement, deleteTalentStaffingRequirementById, deleteTalentStaffingRequirementBatch, getTalentStaffingRequirementTemplate, importTalentStaffingRequirement, exportTalentStaffingRequirement } from '@/api/human-resource/talent/talentStaffingRequirement.api'
import type { TalentStaffingRequirement, TalentStaffingRequirementQuery, TalentStaffingRequirementCreate, TalentStaffingRequirementUpdate } from '@/types/human-resource/talent/talentStaffingRequirement.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktTalentStaffingRequirement')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.talentStaffingRequirement._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<TalentStaffingRequirement[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<TalentStaffingRequirement | null>(null)
/** 表格多选行 */
const selecmedRows = ref<TalentStaffingRequirement[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<TalentStaffingRequirement>>({})
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/** 高级查询表单模型 */
const advancedQueryForm = ref({
  reqCode: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'reqCode', label: t('entity.talentStaffingRequirement.reqno') },
  { key: 'deptId', label: t('entity.talentStaffingRequirement.deptid') },
  { key: 'postId', label: t('entity.talentStaffingRequirement.postid') },
  { key: 'jobGrade', label: t('entity.talentStaffingRequirement.jobgrade') },
  { key: 'requestQty', label: t('entity.talentStaffingRequirement.requestqty') },
  { key: 'headcountType', label: t('entity.talentStaffingRequirement.headcounttype') },
  { key: 'reasonCode', label: t('entity.talentStaffingRequirement.reasoncode') },
  { key: 'replaceEmployeeId', label: t('entity.talentStaffingRequirement.replaceemployeeid') },
  { key: 'expectedOnboardDateStart', label: t('entity.talentStaffingRequirement.expectedonboarddatestart') },
  { key: 'expectedOnboardDateEnd', label: t('entity.talentStaffingRequirement.expectedonboarddateend') },
  { key: 'contractType', label: t('entity.talentStaffingRequirement.contracttype') },
  { key: 'workLocation', label: t('entity.talentStaffingRequirement.worklocation') },
  { key: 'jobDesc', label: t('entity.talentStaffingRequirement.jobdesc') },
  { key: 'qualification', label: t('entity.talentStaffingRequirement.qualification') },
  { key: 'budgetYear', label: t('entity.talentStaffingRequirement.budgetyear') },
  { key: 'approvalStatus', label: t('entity.talentStaffingRequirement.approvalstatus') },
  { key: 'initiatorId', label: t('entity.talentStaffingRequirement.initiatorid') },
  { key: 'initiatedAtStart', label: t('entity.talentStaffingRequirement.initiatedatstart') },
  { key: 'initiatedAtEnd', label: t('entity.talentStaffingRequirement.initiatedatend') },
  { key: 'approvedBy', label: t('entity.talentStaffingRequirement.approvedby') },
  { key: 'approvedAtStart', label: t('entity.talentStaffingRequirement.approvedatstart') },
  { key: 'approvedAtEnd', label: t('entity.talentStaffingRequirement.approvedatend') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.extfield') },
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
const entityIdName = 'talentStaffingRequirementId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** 页面挂载后加载分页列表 */
onMounted(() => {
  loadData()
})

/** 加载主表详情并回填当前页 dataSource */
async function loadTalentStaffingRequirementDetail(record: TalentStaffingRequirement): Promise<TalentStaffingRequirement | null> {
  const id = getTalentStaffingRequirementId(record)
  if (!id) {
    return null
  }
  try {
    const detail = await getTalentStaffingRequirementById(id)
    const index = dataSource.value.findIndex((row) => getTalentStaffingRequirementId(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as TalentStaffingRequirement
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
    dataIndex: 'talentStaffingRequirementId',
    key: 'talentStaffingRequirementId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'talentStaffingRequirementId') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.reqno'),
    dataIndex: 'reqCode',
    key: 'reqCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'reqCode') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.deptid'),
    dataIndex: 'deptId',
    key: 'deptId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'deptId') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.deptname'),
    dataIndex: 'deptName',
    key: 'deptName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'deptName') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.postid'),
    dataIndex: 'postId',
    key: 'postId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'postId') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.postname'),
    dataIndex: 'postName',
    key: 'postName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'postName') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.jobgrade'),
    dataIndex: 'jobGrade',
    key: 'jobGrade',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'jobGrade') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.requestqty'),
    dataIndex: 'requestQty',
    key: 'requestQty',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'requestQty') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.headcounttype'),
    dataIndex: 'headcountType',
    key: 'headcountType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'headcountType') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.reasoncode'),
    dataIndex: 'reasonCode',
    key: 'reasonCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'reasonCode') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.replaceemployeeid'),
    dataIndex: 'replaceEmployeeId',
    key: 'replaceEmployeeId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'replaceEmployeeId') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.replaceemployeename'),
    dataIndex: 'replaceEmployeeName',
    key: 'replaceEmployeeName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'replaceEmployeeName') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.expectedonboarddate'),
    dataIndex: 'expectedOnboardDate',
    key: 'expectedOnboardDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'expectedOnboardDate') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.contracttype'),
    dataIndex: 'contractType',
    key: 'contractType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'contractType') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.worklocation'),
    dataIndex: 'workLocation',
    key: 'workLocation',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'workLocation') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.jobdesc'),
    dataIndex: 'jobDesc',
    key: 'jobDesc',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'jobDesc') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.qualification'),
    dataIndex: 'qualification',
    key: 'qualification',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'qualification') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.budgetyear'),
    dataIndex: 'budgetYear',
    key: 'budgetYear',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'budgetYear') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.dept'),
    dataIndex: 'dept',
    key: 'dept',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'dept') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.post'),
    dataIndex: 'post',
    key: 'post',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'post') ?? ''
  },
  {
    title: t('entity.talentStaffingRequirement.replaceemployee'),
    dataIndex: 'replaceEmployee',
    key: 'replaceEmployee',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentStaffingRequirementField(record, 'replaceEmployee') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:talent:talentstaffingrequirement:update',
        onClick: (record: TalentStaffingRequirement) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:talent:talentstaffingrequirement:delete',
        onClick: (record: TalentStaffingRequirement) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getTalentStaffingRequirementId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getTalentStaffingRequirementField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TalentStaffingRequirement[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TalentStaffingRequirement, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getTalentStaffingRequirementId(selecmedRow.value) === getTalentStaffingRequirementId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: TalentStaffingRequirement[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: TalentStaffingRequirement) => ({
  onClick: () => {
    const key = getTalentStaffingRequirementId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getTalentStaffingRequirementId(item)))
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
    const kw = (queryKeyword.value ?? '').trim()
    const params: TalentStaffingRequirementQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getTalentStaffingRequirementList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TalentStaffingRequirement] 加载数据失败', { error })
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
  currentPage.value = 1
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  reqCode: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.talentStaffingRequirement._self') })
  formData.value = {}
  formVisible.value = true
}
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: TalentStaffingRequirement) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.talentStaffingRequirement._self') })
  formLoading.value = true
  try {
    const detail = await loadTalentStaffingRequirementDetail(record)
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
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.talentStaffingRequirement._self') }))
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
      await updateTalentStaffingRequirement(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.talentStaffingRequirement._self') }))
    } else {
      await createTalentStaffingRequirement(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.talentStaffingRequirement._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getTalentStaffingRequirementTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTalentStaffingRequirement(file, sheetName)
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
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: TalentStaffingRequirementQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportTalentStaffingRequirement(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.talentStaffingRequirement._self') }))
  } catch (error: any) {
    logger.error('[TalentStaffingRequirement] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.talentStaffingRequirement._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: TalentStaffingRequirement) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.talentStaffingRequirement._self'), name: t('common.tip.this.target', { target: t('entity.talentStaffingRequirement._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTalentStaffingRequirementById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.talentStaffingRequirement._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.talentStaffingRequirement._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.talentStaffingRequirement._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTalentStaffingRequirementBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.talentStaffingRequirement._self') }))
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
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  reqCode: '',
  deptId: '',
  postId: '',
  jobGrade: '',
  requestQty: undefined as number | undefined,
  headcountType: '',
  reasonCode: '',
  replaceEmployeeId: '',
  expectedOnboardDateStart: '',
  expectedOnboardDateEnd: '',
  contractType: '',
  workLocation: '',
  jobDesc: '',
  qualification: '',
  budgetYear: '',
  approvalStatus: undefined as number | undefined,
  initiatorId: '',
  initiatedAtStart: '',
  initiatedAtEnd: '',
  approvedBy: '',
  approvedAtStart: '',
  approvedAtEnd: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
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
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
/** 分页每页条数变更 */
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.human-resource-talent-talent-staffing-requirement {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>

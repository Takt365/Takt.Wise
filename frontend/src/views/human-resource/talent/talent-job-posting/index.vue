<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/talent/talent-job-posting -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：职位发布管理页面，含查询、增删改，由 generate-vue-from-api 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="human-resource-talent-talent-job-posting">
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
      create-permission="human:resource:talent:talentjobposting:create"
      update-permission="human:resource:talent:talentjobposting:update"
      delete-permission="human:resource:talent:talentjobposting:delete"
      import-permission="human:resource:talent:talentjobposting:import"
      export-permission="human:resource:talent:talentjobposting:export"
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
      entity-scope="company"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'talentJobPostingId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getTalentJobPostingId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >

    </TaktSingleTable>

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
      <TalentJobPostingForm
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
      :storage-key="'takt-query-fields-human-resource-talent-talent-job-posting'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('staffingRequirementId')">
      <a-form-item :label="t('entity.talentJobPosting.staffingrequirementid')">
        <a-input
          v-model:value="advancedQueryForm.staffingRequirementId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.staffingrequirementid') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingCode')">
      <a-form-item :label="t('entity.talentJobPosting.postingcode')">
        <a-input
          v-model:value="advancedQueryForm.postingCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.postingcode') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('title')">
      <a-form-item :label="t('entity.talentJobPosting.title')">
        <a-input
          v-model:value="advancedQueryForm.title"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.title') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postingStatus')">
      <a-form-item :label="t('entity.talentJobPosting.postingstatus')">
        <a-input-number
          v-model:value="advancedQueryForm.postingStatus"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.postingstatus') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishDateStart')">
      <a-form-item :label="t('entity.talentJobPosting.publishdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.publishdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishDateEnd')">
      <a-form-item :label="t('entity.talentJobPosting.publishdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.publishDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.publishdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('openDateStart')">
      <a-form-item :label="t('entity.talentJobPosting.opendatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.openDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.opendatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('openDateEnd')">
      <a-form-item :label="t('entity.talentJobPosting.opendateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.openDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.opendateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closeDateStart')">
      <a-form-item :label="t('entity.talentJobPosting.closedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.closeDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.closedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closeDateEnd')">
      <a-form-item :label="t('entity.talentJobPosting.closedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.closeDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.talentJobPosting.closedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('publishChannel')">
      <a-form-item :label="t('entity.talentJobPosting.publishchannel')">
        <a-input-number
          v-model:value="advancedQueryForm.publishChannel"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.publishchannel') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('reason')">
      <a-form-item :label="t('entity.talentJobPosting.reason')">
        <a-input
          v-model:value="advancedQueryForm.reason"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.talentJobPosting.reason') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.talentJobPosting._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.talentJobPosting._self"
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
      :id-column-key="'talentJobPostingId'"
      :action-column-key="'action'"
      entity-scope="company"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
/**
 * 职位发布管理页 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/human-resource/talent/talent-job-posting
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import TalentJobPostingForm from './components/TalentJobPostingForm.vue'
import { getTalentJobPostingList, getTalentJobPostingById, createTalentJobPosting, updateTalentJobPosting, deleteTalentJobPostingById, deleteTalentJobPostingBatch, getTalentJobPostingTemplate, importTalentJobPosting, exportTalentJobPosting } from '@/api/human-resource/talent/talentJobPosting.api'
import type { TalentJobPosting, TalentJobPostingQuery, TalentJobPostingCreate, TalentJobPostingUpdate } from '@/types/human-resource/talent/talentJobPosting.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine } from '@remixicon/vue'

const { t } = useI18n()
const excelNames = taktExcelEntityNames('TaktTalentJobPosting')
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.talentJobPosting._self') })
)

const queryKeyword = ref('')
const loading = ref(false)
const dataSource = ref<TalentJobPosting[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const selecmedRow = ref<TalentJobPosting | null>(null)
const selecmedRows = ref<TalentJobPosting[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])

const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<TalentJobPosting>>({})
const formLoading = ref(false)
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
const advancedQueryVisible = ref(false)
const advancedQueryForm = ref({
  staffingRequirementId: '',
  postingCode: '',
  title: '',
  postingStatus: undefined as number | undefined,
  publishDateStart: '',
  publishDateEnd: '',
  openDateStart: '',
  openDateEnd: '',
  closeDateStart: '',
  closeDateEnd: '',
  publishChannel: undefined as number | undefined,
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
})
/** 高级查询字段元数据（显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'staffingRequirementId', label: t('entity.talentJobPosting.staffingrequirementid') },
  { key: 'postingCode', label: t('entity.talentJobPosting.postingcode') },
  { key: 'title', label: t('entity.talentJobPosting.title') },
  { key: 'postingStatus', label: t('entity.talentJobPosting.postingstatus') },
  { key: 'publishDateStart', label: t('entity.talentJobPosting.publishdatestart') },
  { key: 'publishDateEnd', label: t('entity.talentJobPosting.publishdateend') },
  { key: 'openDateStart', label: t('entity.talentJobPosting.opendatestart') },
  { key: 'openDateEnd', label: t('entity.talentJobPosting.opendateend') },
  { key: 'closeDateStart', label: t('entity.talentJobPosting.closedatestart') },
  { key: 'closeDateEnd', label: t('entity.talentJobPosting.closedateend') },
  { key: 'publishChannel', label: t('entity.talentJobPosting.publishchannel') },
  { key: 'reason', label: t('entity.talentJobPosting.reason') },
  { key: 'createdAtStart', label: t('common.page.entity.createdatstart') },
  { key: 'createdAtEnd', label: t('common.page.entity.createdatend') },
  { key: 'ExtField', label: t('common.page.entity.extfield') },
  { key: 'remark', label: t('common.page.entity.remark') }])
const visibleQueryFieldKeys = ref<string[]>([])
const columnSettingVisible = ref(false)
const importVisible = ref(false)
const visibleColumnKeys = ref<string[]>([])
const entityIdName = 'talentJobPostingId'
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

onMounted(() => {
  loadData()
})

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'talentJobPostingId',
    key: 'talentJobPostingId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'talentJobPostingId') ?? ''
  },
  {
    title: t('entity.talentJobPosting.staffingrequirementid'),
    dataIndex: 'staffingRequirementId',
    key: 'staffingRequirementId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'staffingRequirementId') ?? ''
  },
  {
    title: t('entity.talentJobPosting.staffingrequirementname'),
    dataIndex: 'staffingRequirementName',
    key: 'staffingRequirementName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'staffingRequirementName') ?? ''
  },
  {
    title: t('entity.talentJobPosting.postingcode'),
    dataIndex: 'postingCode',
    key: 'postingCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'postingCode') ?? ''
  },
  {
    title: t('entity.talentJobPosting.title'),
    dataIndex: 'title',
    key: 'title',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'title') ?? ''
  },
  {
    title: t('entity.talentJobPosting.postingstatus'),
    dataIndex: 'postingStatus',
    key: 'postingStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'postingStatus') ?? ''
  },
  {
    title: t('entity.talentJobPosting.publishdate'),
    dataIndex: 'publishDate',
    key: 'publishDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'publishDate') ?? ''
  },
  {
    title: t('entity.talentJobPosting.opendate'),
    dataIndex: 'openDate',
    key: 'openDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'openDate') ?? ''
  },
  {
    title: t('entity.talentJobPosting.closedate'),
    dataIndex: 'closeDate',
    key: 'closeDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'closeDate') ?? ''
  },
  {
    title: t('entity.talentJobPosting.publishchannel'),
    dataIndex: 'publishChannel',
    key: 'publishChannel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'publishChannel') ?? ''
  },
  {
    title: t('entity.talentJobPosting.reason'),
    dataIndex: 'reason',
    key: 'reason',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getTalentJobPostingField(record, 'reason') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:talent:talentjobposting:update',
        onClick: (record: TalentJobPosting) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:talent:talentjobposting:delete',
        onClick: (record: TalentJobPosting) => handleDeleteOne(record)
      }
    ]
  })
])

const getTalentJobPostingId = (record: any): string => record?.[entityIdName] ?? ''
const getTalentJobPostingField = (record: any, field: string): any => record?.[field]

const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: TalentJobPosting[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: TalentJobPosting, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (getTalentJobPostingId(selecmedRow.value) === getTalentJobPostingId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: TalentJobPosting[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

const onClickRow = (record: TalentJobPosting) => ({
  onClick: () => {
    const key = getTalentJobPostingId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getTalentJobPostingId(item)))
    selecmedRow.value = selecmedRowKeys.value.length === 1 ? (selecmedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selecmedRowKeys.value, selecmedRows.value)
    }
  }
})

async function loadData() {
  loading.value = true
  try {
    const kw = (queryKeyword.value ?? '').trim()
    const params: TalentJobPostingQuery = {
      pageIndex: currentPage.value,
      pageSize: pageSize.value,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      params.keyWords = kw
    }
    const res = await getTalentJobPostingList(params)
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[TalentJobPosting] 加载数据失败', { error })
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

function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = {
  staffingRequirementId: '',
  postingCode: '',
  title: '',
  postingStatus: undefined as number | undefined,
  publishDateStart: '',
  publishDateEnd: '',
  openDateStart: '',
  openDateEnd: '',
  closeDateStart: '',
  closeDateEnd: '',
  publishChannel: undefined as number | undefined,
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
  currentPage.value = 1
  loadData()
}

function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.talentJobPosting._self') })
  formData.value = {}
  formVisible.value = true
}
function handleEdit(record: TalentJobPosting) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.talentJobPosting._self') })
  formData.value = { ...record }
  formVisible.value = true
}

function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.talentJobPosting._self') }))
  }
}
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
      await updateTalentJobPosting(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.talentJobPosting._self') }))
    } else {
      await createTalentJobPosting(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.talentJobPosting._self') }))
    }
    formVisible.value = false
    loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}
function handleImport() {
  importVisible.value = true
}

async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getTalentJobPostingTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importTalentJobPosting(file, sheetName)
}

function handleImportSuccess(result: { success: number; fail: number; errors: string[] }) {
  loadData()
  if (result.fail === 0) setTimeout(() => { importVisible.value = false }, 2000)
}

function handleImportCancel() {
  importVisible.value = false
}
async function handleExport() {
  try {
    loading.value = true
    const kw = (queryKeyword.value ?? '').trim()
    const exportQuery: TalentJobPostingQuery = {
      pageIndex: 1,
      pageSize: 100000,
      ...advancedQueryForm.value
    }
    if (kw.length > 0) {
      exportQuery.keyWords = kw
    }
    const exportMeta = await exportTalentJobPosting(exportQuery, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: t('entity.talentJobPosting._self') }))
  } catch (error: any) {
    logger.error('[TalentJobPosting] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.talentJobPosting._self') }))
  } finally {
    loading.value = false
  }
}
async function handleDeleteOne(record: TalentJobPosting) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.talentJobPosting._self'), name: t('common.tip.this.target', { target: t('entity.talentJobPosting._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteTalentJobPostingById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.talentJobPosting._self') }))
      loadData()
    }
  })
}
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.talentJobPosting._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.talentJobPosting._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteTalentJobPostingBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.talentJobPosting._self') }))
      loadData()
    }
  })
}
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = 1
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = {
  staffingRequirementId: '',
  postingCode: '',
  title: '',
  postingStatus: undefined as number | undefined,
  publishDateStart: '',
  publishDateEnd: '',
  openDateStart: '',
  openDateEnd: '',
  closeDateStart: '',
  closeDateEnd: '',
  publishChannel: undefined as number | undefined,
  reason: '',
  createdAtStart: '',
  createdAtEnd: '',
  ExtField: '',
  remark: '',
  }
}

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

function handleRefresh() {
  loadData()
}

function handleTableChange() {}
function handleResizeColumn() {}
function handlePaginationChange(page: number) {
  currentPage.value = page
  loadData()
}
function handlePaginationSizeChange(_current: number, size: number) {
  pageSize.value = size
  currentPage.value = 1
  loadData()
}
</script>

<style scoped lang="css">
.human-resource-talent-talent-job-posting {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
</style>

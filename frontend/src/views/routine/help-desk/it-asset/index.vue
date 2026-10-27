<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/routine/help-desk/it-asset -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：服务台 IT 设备保修扩展实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="routine:help:desk:it:asset:create"
      update-permission="routine:help:desk:it:asset:update"
      delete-permission="routine:help:desk:it:asset:delete"
      import-permission="routine:help:desk:it:asset:import"
      export-permission="routine:help:desk:it:asset:export"
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
      :id-column-key="'itAssetId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getItAssetId"
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
      <ItAssetForm
        :key="formData?.itAssetId ?? 'create'"
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
      :storage-key="'takt-query-fields-routine-help-desk-it-asset'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('assetCode')">
      <a-form-item :label="t('entity.itasset.assetcode')">
        <a-input
          v-model:value="advancedQueryForm.assetCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.assetcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyType')">
      <a-form-item :label="t('entity.itasset.warrantytype')">
        <a-input-number
          v-model:value="advancedQueryForm.warrantyType"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.warrantytype') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateStart')">
      <a-form-item :label="t('entity.itasset.warrantystartdatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.warrantystartdatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyStartDateEnd')">
      <a-form-item :label="t('entity.itasset.warrantystartdateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyStartDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.warrantystartdateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyExpiryDateStart')">
      <a-form-item :label="t('entity.itasset.warrantyexpirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyExpiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.warrantyexpirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyExpiryDateEnd')">
      <a-form-item :label="t('entity.itasset.warrantyexpirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.warrantyExpiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.warrantyexpirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyProvider')">
      <a-form-item :label="t('entity.itasset.warrantyprovider')">
        <a-input
          v-model:value="advancedQueryForm.warrantyProvider"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.warrantyprovider') })"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyContractCode')">
      <a-form-item :label="t('entity.itasset.warrantyContractCode')">
        <a-input
          v-model:value="advancedQueryForm.warrantyContractCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.warrantyContractCode') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceHotline')">
      <a-form-item :label="t('entity.itasset.servicehotline')">
        <a-input
          v-model:value="advancedQueryForm.serviceHotline"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.servicehotline') })"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serviceEmail')">
      <a-form-item :label="t('entity.itasset.serviceemail')">
        <a-input
          v-model:value="advancedQueryForm.serviceEmail"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.itasset.serviceemail') })"
          show-count
          :maxlength="100"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceExpiryDateStart')">
      <a-form-item :label="t('entity.itasset.maintenanceexpirydatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceExpiryDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.maintenanceexpirydatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceExpiryDateEnd')">
      <a-form-item :label="t('entity.itasset.maintenanceexpirydateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.maintenanceExpiryDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.maintenanceexpirydateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastMaintenanceDateStart')">
      <a-form-item :label="t('entity.itasset.lastmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.lastmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lastMaintenanceDateEnd')">
      <a-form-item :label="t('entity.itasset.lastmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.lastMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.lastmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateStart')">
      <a-form-item :label="t('entity.itasset.nextmaintenancedatestart')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateStart"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.nextmaintenancedatestart') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('nextMaintenanceDateEnd')">
      <a-form-item :label="t('entity.itasset.nextmaintenancedateend')">
        <a-date-picker
          v-model:value="advancedQueryForm.nextMaintenanceDateEnd"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.itasset.nextmaintenancedateend') })"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warrantyRemark')">
      <a-form-item :label="t('entity.itasset.warrantyremark')">
        <a-textarea
          v-model:value="advancedQueryForm.warrantyRemark"
          :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.itasset.warrantyremark') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.itasset._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.itasset._self"
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
      :id-column-key="'itAssetId'"
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
 * 服务台 IT 设备保修扩展实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/routine/help-desk/it-asset
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import ItAssetForm from './components/ItAssetForm.vue'
import { getItAssetList, getItAssetById, createItAsset, updateItAsset, deleteItAssetById, deleteItAssetBatch, getItAssetTemplate, importItAsset, exportItAsset } from '@/api/routine/help-desk/itAsset.api'
import type { ItAsset, ItAssetQuery } from '@/types/routine/help-desk/itAsset.types'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktItAsset')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.itasset._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<ItAsset[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<ItAsset | null>(null)
/** 表格多选行 */
const selecmedRows = ref<ItAsset[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<ItAsset> | null>(null)
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
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractCode: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'assetCode', label: t('entity.itasset.assetcode') },
  { key: 'warrantyType', label: t('entity.itasset.warrantytype') },
  { key: 'warrantyStartDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.itasset.warrantystartdate')) },
  { key: 'warrantyStartDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.itasset.warrantystartdate')) },
  { key: 'warrantyExpiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.itasset.warrantyexpirydate')) },
  { key: 'warrantyExpiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.itasset.warrantyexpirydate')) },
  { key: 'warrantyProvider', label: t('entity.itasset.warrantyprovider') },
  { key: 'warrantyContractCode', label: t('entity.itasset.warrantyContractCode') },
  { key: 'serviceHotline', label: t('entity.itasset.servicehotline') },
  { key: 'serviceEmail', label: t('entity.itasset.serviceemail') },
  { key: 'maintenanceExpiryDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.itasset.maintenanceexpirydate')) },
  { key: 'maintenanceExpiryDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.itasset.maintenanceexpirydate')) },
  { key: 'lastMaintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.itasset.lastmaintenancedate')) },
  { key: 'lastMaintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.itasset.lastmaintenancedate')) },
  { key: 'nextMaintenanceDateStart', label: t('common.page.entity.createdatstart').replace(t('common.page.entity.createdat'), t('entity.itasset.nextmaintenancedate')) },
  { key: 'nextMaintenanceDateEnd', label: t('common.page.entity.createdatend').replace(t('common.page.entity.createdat'), t('entity.itasset.nextmaintenancedate')) },
  { key: 'warrantyRemark', label: t('entity.itasset.warrantyremark') },
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
const entityIdName = 'itAssetId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {ItAssetQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<ItAssetQuery>): ItAssetQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ItAssetQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ItAssetQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('assetCode', form.assetCode)
  if (form.warrantyType !== undefined && form.warrantyType !== null) {
    query.warrantyType = form.warrantyType
  }
  assignTrimmed('warrantyStartDateStart', form.warrantyStartDateStart)
  assignTrimmed('warrantyStartDateEnd', form.warrantyStartDateEnd)
  assignTrimmed('warrantyExpiryDateStart', form.warrantyExpiryDateStart)
  assignTrimmed('warrantyExpiryDateEnd', form.warrantyExpiryDateEnd)
  assignTrimmed('warrantyProvider', form.warrantyProvider)
  assignTrimmed('warrantyContractCode', form.warrantyContractCode)
  assignTrimmed('serviceHotline', form.serviceHotline)
  assignTrimmed('serviceEmail', form.serviceEmail)
  assignTrimmed('maintenanceExpiryDateStart', form.maintenanceExpiryDateStart)
  assignTrimmed('maintenanceExpiryDateEnd', form.maintenanceExpiryDateEnd)
  assignTrimmed('lastMaintenanceDateStart', form.lastMaintenanceDateStart)
  assignTrimmed('lastMaintenanceDateEnd', form.lastMaintenanceDateEnd)
  assignTrimmed('nextMaintenanceDateStart', form.nextMaintenanceDateStart)
  assignTrimmed('nextMaintenanceDateEnd', form.nextMaintenanceDateEnd)
  assignTrimmed('warrantyRemark', form.warrantyRemark)
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
    dataIndex: 'itAssetId',
    key: 'itAssetId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'itAssetId') ?? ''
  },
  {
    title: t('entity.itasset.assetcode'),
    dataIndex: 'assetCode',
    key: 'assetCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'assetCode') ?? ''
  },
  {
    title: t('entity.itasset.warrantytype'),
    dataIndex: 'warrantyType',
    key: 'warrantyType',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyType') ?? ''
  },
  {
    title: t('entity.itasset.warrantystartdate'),
    dataIndex: 'warrantyStartDate',
    key: 'warrantyStartDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyStartDate') ?? ''
  },
  {
    title: t('entity.itasset.warrantyexpirydate'),
    dataIndex: 'warrantyExpiryDate',
    key: 'warrantyExpiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyExpiryDate') ?? ''
  },
  {
    title: t('entity.itasset.warrantyprovider'),
    dataIndex: 'warrantyProvider',
    key: 'warrantyProvider',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyProvider') ?? ''
  },
  {
    title: t('entity.itasset.warrantyContractCode'),
    dataIndex: 'warrantyContractCode',
    key: 'warrantyContractCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyContractCode') ?? ''
  },
  {
    title: t('entity.itasset.servicehotline'),
    dataIndex: 'serviceHotline',
    key: 'serviceHotline',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'serviceHotline') ?? ''
  },
  {
    title: t('entity.itasset.serviceemail'),
    dataIndex: 'serviceEmail',
    key: 'serviceEmail',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'serviceEmail') ?? ''
  },
  {
    title: t('entity.itasset.maintenanceexpirydate'),
    dataIndex: 'maintenanceExpiryDate',
    key: 'maintenanceExpiryDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'maintenanceExpiryDate') ?? ''
  },
  {
    title: t('entity.itasset.lastmaintenancedate'),
    dataIndex: 'lastMaintenanceDate',
    key: 'lastMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'lastMaintenanceDate') ?? ''
  },
  {
    title: t('entity.itasset.nextmaintenancedate'),
    dataIndex: 'nextMaintenanceDate',
    key: 'nextMaintenanceDate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'nextMaintenanceDate') ?? ''
  },
  {
    title: t('entity.itasset.warrantyremark'),
    dataIndex: 'warrantyRemark',
    key: 'warrantyRemark',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getItAssetField(record, 'warrantyRemark') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'routine:help:desk:it:asset:update',
        onClick: (record: ItAsset) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'routine:help:desk:it:asset:delete',
        onClick: (record: ItAsset) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getItAssetId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getItAssetField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ItAsset[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ItAsset, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getItAssetId(selecmedRow.value) === getItAssetId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: ItAsset[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ItAsset) => ({
  onClick: () => {
    const key = getItAssetId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getItAssetId(item)))
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
    const res = await getItAssetList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[ItAsset] 加载数据失败', { error })
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
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractCode: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.itasset._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: ItAsset) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.itasset._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.itasset._self') }))
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
      await updateItAsset(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.itasset._self') }))
    } else {
      await createItAsset(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.itasset._self') }))
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
  const res = await getItAssetTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importItAsset(file, sheetName)
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
    const exportMeta = await exportItAsset(
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
    message.success(t('common.feedback.export.success', { target: t('entity.itasset._self') }))
  } catch (error: any) {
    logger.error('[ItAsset] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.itasset._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: ItAsset) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.itasset._self'), name: t('common.tip.this.target', { target: t('entity.itasset._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteItAssetById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.itasset._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.itasset._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.itasset._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteItAssetBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.itasset._self') }))
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
  assetCode: '',
  warrantyType: undefined as number | undefined,
  warrantyStartDateStart: '',
  warrantyStartDateEnd: '',
  warrantyExpiryDateStart: '',
  warrantyExpiryDateEnd: '',
  warrantyProvider: '',
  warrantyContractCode: '',
  serviceHotline: '',
  serviceEmail: '',
  maintenanceExpiryDateStart: '',
  maintenanceExpiryDateEnd: '',
  lastMaintenanceDateStart: '',
  lastMaintenanceDateEnd: '',
  nextMaintenanceDateStart: '',
  nextMaintenanceDateEnd: '',
  warrantyRemark: '',
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

<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/salary-item -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：薪资项目管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
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
      create-permission="human:resource:compensation:salary:item:create"
      update-permission="human:resource:compensation:salary:item:update"
      delete-permission="human:resource:compensation:salary:item:delete"
      import-permission="human:resource:compensation:salary:item:import"
      export-permission="human:resource:compensation:salary:item:export"
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
      :id-column-key="'salaryItemId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :row-key="getSalaryItemId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'itemStatus'">
          <a-switch
            :checked="getSalaryItemField(record, 'itemStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleItemStatusChange(record, Boolean(checked))"
          />
        </template>
        <template v-else-if="column.key === 'itemType'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'itemType')"
            dict-type="humanresource_compensation_salary_item_type"
          />
        </template>
        <template v-else-if="column.key === 'calcMethod'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'calcMethod')"
            dict-type="humanresource_compensation_salary_calc_method"
          />
        </template>
        <template v-else-if="column.key === 'isDeduction'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'isDeduction')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'isTaxable'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'isTaxable')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'includeSocialSecurityBase'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'includeSocialSecurityBase')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'includeHousingFundBase'">
          <TaktDictTag
            :value="getSalaryItemField(record, 'includeHousingFundBase')"
            dict-type="sys_yes_no"
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
      <SalaryItemForm
        :key="formData?.salaryItemId ?? 'create'"
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
      :storage-key="'takt-query-fields-human-resource-compensation-salary-item'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('itemCode')">
      <a-form-item :label="t('entity.salaryitem.itemcode')">
        <a-input
          v-model:value="advancedQueryForm.itemCode"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemcode') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemName')">
      <a-form-item :label="t('entity.salaryitem.itemname')">
        <a-input
          v-model:value="advancedQueryForm.itemName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemname') })"
          show-count
          :maxlength="80"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shortName')">
      <a-form-item :label="t('entity.salaryitem.shortname')">
        <a-input
          v-model:value="advancedQueryForm.shortName"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.shortname') })"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemType')">
      <a-form-item :label="t('entity.salaryitem.itemtype')">
        <TaktSelect
          v-model:value="advancedQueryForm.itemType"
          dict-type="humanresource_compensation_salary_item_type"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('calcMethod')">
      <a-form-item :label="t('entity.salaryitem.calcmethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.calcMethod"
          dict-type="humanresource_compensation_salary_calc_method"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('salaryFormulaId')">
      <a-form-item :label="t('entity.salaryitem.salaryformulaid')">
        <a-input
          v-model:value="advancedQueryForm.salaryFormulaId"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.salaryformulaid') })"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defaultAmount')">
      <a-form-item :label="t('entity.salaryitem.defaultamount')">
        <a-input-number
          v-model:value="advancedQueryForm.defaultAmount"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultamount') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('defaultRate')">
      <a-form-item :label="t('entity.salaryitem.defaultrate')">
        <a-input-number
          v-model:value="advancedQueryForm.defaultRate"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultrate') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('strikePrice')">
      <a-form-item :label="t('entity.salaryitem.strikeprice')">
        <a-input-number
          v-model:value="advancedQueryForm.strikePrice"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.strikeprice') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('vestingYears')">
      <a-form-item :label="t('entity.salaryitem.vestingyears')">
        <a-input-number
          v-model:value="advancedQueryForm.vestingYears"
          :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.vestingyears') })"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isDeduction')">
      <a-form-item :label="t('entity.salaryitem.isdeduction')">
        <TaktSelect
          v-model:value="advancedQueryForm.isDeduction"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isTaxable')">
      <a-form-item :label="t('entity.salaryitem.istaxable')">
        <TaktSelect
          v-model:value="advancedQueryForm.isTaxable"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('includeSocialSecurityBase')">
      <a-form-item :label="t('entity.salaryitem.includesocialsecuritybase')">
        <TaktSelect
          v-model:value="advancedQueryForm.includeSocialSecurityBase"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('includeHousingFundBase')">
      <a-form-item :label="t('entity.salaryitem.includehousingfundbase')">
        <TaktSelect
          v-model:value="advancedQueryForm.includeHousingFundBase"
          dict-type="sys_yes_no"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') })"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('itemStatus')">
      <a-form-item :label="t('entity.salaryitem.itemstatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.itemStatus"
          dict-type="sys_normal_disable"
          :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') })"
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
      :title="t('common.dialog.title.import', { entity: t('entity.salaryitem._self') })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        entity-i18n-key="entity.salaryitem._self"
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
      :id-column-key="'salaryItemId'"
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
 * 薪资项目管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/salary-item
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import SalaryItemForm from './components/SalaryItemForm.vue'
import { getSalaryItemList, getSalaryItemById, createSalaryItem, updateSalaryItem, deleteSalaryItemById, deleteSalaryItemBatch, getSalaryItemTemplate, importSalaryItem, exportSalaryItem, updateSalaryItemStatus } from '@/api/human-resource/compensation/salaryItem.api'
import type { SalaryItem, SalaryItemQuery } from '@/types/human-resource/compensation/salaryItem.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktSalaryItem')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: t('entity.salaryitem._self') })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<SalaryItem[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selecmedRow = ref<SalaryItem | null>(null)
/** 表格多选行 */
const selecmedRows = ref<SalaryItem[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<SalaryItem> | null>(null)
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
  itemCode: '',
  itemName: '',
  shortName: '',
  itemType: undefined as number | undefined,
  calcMethod: undefined as number | undefined,
  salaryFormulaId: '',
  defaultAmount: undefined as number | undefined,
  defaultRate: undefined as number | undefined,
  strikePrice: undefined as number | undefined,
  vestingYears: undefined as number | undefined,
  isDeduction: undefined as number | undefined,
  isTaxable: undefined as number | undefined,
  includeSocialSecurityBase: undefined as number | undefined,
  includeHousingFundBase: undefined as number | undefined,
  itemStatus: undefined as number | undefined,
  plantCode: '',
  createdAtStart: '',
  createdAtEnd: '',
  extField: '',
  remark: '',
})
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() => [
  { key: 'itemCode', label: t('entity.salaryitem.itemcode') },
  { key: 'itemName', label: t('entity.salaryitem.itemname') },
  { key: 'shortName', label: t('entity.salaryitem.shortname') },
  { key: 'itemType', label: t('entity.salaryitem.itemtype') },
  { key: 'calcMethod', label: t('entity.salaryitem.calcmethod') },
  { key: 'salaryFormulaId', label: t('entity.salaryitem.salaryformulaid') },
  { key: 'defaultAmount', label: t('entity.salaryitem.defaultamount') },
  { key: 'defaultRate', label: t('entity.salaryitem.defaultrate') },
  { key: 'strikePrice', label: t('entity.salaryitem.strikeprice') },
  { key: 'vestingYears', label: t('entity.salaryitem.vestingyears') },
  { key: 'isDeduction', label: t('entity.salaryitem.isdeduction') },
  { key: 'isTaxable', label: t('entity.salaryitem.istaxable') },
  { key: 'includeSocialSecurityBase', label: t('entity.salaryitem.includesocialsecuritybase') },
  { key: 'includeHousingFundBase', label: t('entity.salaryitem.includehousingfundbase') },
  { key: 'itemStatus', label: t('entity.salaryitem.itemstatus') },
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
const entityIdName = 'salaryItemId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selecmedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selecmedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {SalaryItemQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<SalaryItemQuery>): SalaryItemQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: SalaryItemQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof SalaryItemQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  assignTrimmed('itemCode', form.itemCode)
  assignTrimmed('itemName', form.itemName)
  assignTrimmed('shortName', form.shortName)
  if (form.itemType !== undefined && form.itemType !== null) {
    query.itemType = form.itemType
  }
  if (form.calcMethod !== undefined && form.calcMethod !== null) {
    query.calcMethod = form.calcMethod
  }
  assignTrimmed('salaryFormulaId', form.salaryFormulaId)
  if (form.defaultAmount !== undefined && form.defaultAmount !== null) {
    query.defaultAmount = form.defaultAmount
  }
  if (form.defaultRate !== undefined && form.defaultRate !== null) {
    query.defaultRate = form.defaultRate
  }
  if (form.strikePrice !== undefined && form.strikePrice !== null) {
    query.strikePrice = form.strikePrice
  }
  if (form.vestingYears !== undefined && form.vestingYears !== null) {
    query.vestingYears = form.vestingYears
  }
  if (form.isDeduction !== undefined && form.isDeduction !== null) {
    query.isDeduction = form.isDeduction
  }
  if (form.isTaxable !== undefined && form.isTaxable !== null) {
    query.isTaxable = form.isTaxable
  }
  if (form.includeSocialSecurityBase !== undefined && form.includeSocialSecurityBase !== null) {
    query.includeSocialSecurityBase = form.includeSocialSecurityBase
  }
  if (form.includeHousingFundBase !== undefined && form.includeHousingFundBase !== null) {
    query.includeHousingFundBase = form.includeHousingFundBase
  }
  if (form.itemStatus !== undefined && form.itemStatus !== null) {
    query.itemStatus = form.itemStatus
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
    dataIndex: 'salaryItemId',
    key: 'salaryItemId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'salaryItemId') ?? ''
  },
  {
    title: t('entity.salaryitem.itemcode'),
    dataIndex: 'itemCode',
    key: 'itemCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'itemCode') ?? ''
  },
  {
    title: t('entity.salaryitem.itemname'),
    dataIndex: 'itemName',
    key: 'itemName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'itemName') ?? ''
  },
  {
    title: t('entity.salaryitem.shortname'),
    dataIndex: 'shortName',
    key: 'shortName',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'shortName') ?? ''
  },
  {
    title: t('entity.salaryitem.itemtype'),
    dataIndex: 'itemType',
    key: 'itemType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.calcmethod'),
    dataIndex: 'calcMethod',
    key: 'calcMethod',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.salaryformulaid'),
    dataIndex: 'salaryFormulaId',
    key: 'salaryFormulaId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'salaryFormulaId') ?? ''
  },
  {
    title: t('entity.salaryitem.defaultamount'),
    dataIndex: 'defaultAmount',
    key: 'defaultAmount',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'defaultAmount') ?? ''
  },
  {
    title: t('entity.salaryitem.defaultrate'),
    dataIndex: 'defaultRate',
    key: 'defaultRate',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'defaultRate') ?? ''
  },
  {
    title: t('entity.salaryitem.strikeprice'),
    dataIndex: 'strikePrice',
    key: 'strikePrice',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'strikePrice') ?? ''
  },
  {
    title: t('entity.salaryitem.vestingyears'),
    dataIndex: 'vestingYears',
    key: 'vestingYears',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'vestingYears') ?? ''
  },
  {
    title: t('entity.salaryitem.isdeduction'),
    dataIndex: 'isDeduction',
    key: 'isDeduction',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.istaxable'),
    dataIndex: 'isTaxable',
    key: 'isTaxable',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.includesocialsecuritybase'),
    dataIndex: 'includeSocialSecurityBase',
    key: 'includeSocialSecurityBase',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.includehousingfundbase'),
    dataIndex: 'includeHousingFundBase',
    key: 'includeHousingFundBase',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.itemstatus'),
    dataIndex: 'itemStatus',
    key: 'itemStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: t('entity.salaryitem.relatedplant'),
    dataIndex: 'plantCode',
    key: 'plantCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => getSalaryItemField(record, 'plantCode') ?? ''
  },
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'human:resource:compensation:salary:item:update',
        onClick: (record: SalaryItem) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'human:resource:compensation:salary:item:delete',
        onClick: (record: SalaryItem) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getSalaryItemId = (record: any): string => record?.[entityIdName] ?? ''
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const getSalaryItemField = (record: any, field: string): any => record?.[field]

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: SalaryItem[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: SalaryItem, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && getSalaryItemId(selecmedRow.value) === getSalaryItemId(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: SalaryItem[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: SalaryItem) => ({
  onClick: () => {
    const key = getSalaryItemId(record)
    const index = selecmedRowKeys.value.indexOf(key)
    if (index > -1) {
      selecmedRowKeys.value.splice(index, 1)
    } else {
      selecmedRowKeys.value.push(key)
    }
    selecmedRows.value = dataSource.value.filter((item) => selecmedRowKeys.value.includes(getSalaryItemId(item)))
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
    const res = await getSalaryItemList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[SalaryItem] 加载数据失败', { error })
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
  itemCode: '',
  itemName: '',
  shortName: '',
  itemType: undefined as number | undefined,
  calcMethod: undefined as number | undefined,
  salaryFormulaId: '',
  defaultAmount: undefined as number | undefined,
  defaultRate: undefined as number | undefined,
  strikePrice: undefined as number | undefined,
  vestingYears: undefined as number | undefined,
  isDeduction: undefined as number | undefined,
  isTaxable: undefined as number | undefined,
  includeSocialSecurityBase: undefined as number | undefined,
  includeHousingFundBase: undefined as number | undefined,
  itemStatus: undefined as number | undefined,
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
  formTitle.value = t('common.dialog.title.create', { entity: t('entity.salaryitem._self') })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗 */
function handleEdit(record: SalaryItem) {
  formTitle.value = t('common.dialog.title.edit', { entity: t('entity.salaryitem._self') })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: t('entity.salaryitem._self') }))
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
      await updateSalaryItem(id, payload as any)
      message.success(t('common.feedback.updated', { target: t('entity.salaryitem._self') }))
    } else {
      await createSalaryItem(payload as any)
      message.success(t('common.feedback.created', { target: t('entity.salaryitem._self') }))
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
  const res = await getSalaryItemTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件 */
async function handleImportFile(file: File, sheetName?: string): Promise<{ success: number; fail: number; errors: string[] }> {
  return await importSalaryItem(file, sheetName)
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
    const exportMeta = await exportSalaryItem(
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
    message.success(t('common.feedback.export.success', { target: t('entity.salaryitem._self') }))
  } catch (error: any) {
    logger.error('[SalaryItem] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: t('entity.salaryitem._self') }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: SalaryItem) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: t('entity.salaryitem._self'), name: t('common.tip.this.target', { target: t('entity.salaryitem._self') }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteSalaryItemById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: t('entity.salaryitem._self') }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: t('entity.salaryitem._self') }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: t('entity.salaryitem._self'), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteSalaryItemBatch(ids)
      message.success(t('common.feedback.deleted', { target: t('entity.salaryitem._self') }))
      loadData()
    }
  })
}
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleItemStatusChange(record: SalaryItem, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = getSalaryItemField(record, 'itemStatus')
  const id = getSalaryItemId(record)
  const row = dataSource.value.find((item) => getSalaryItemId(item) === id)
  if (row) {
    row.itemStatus = newVal
  }
  try {
    await updateSalaryItemStatus({ salaryItemId: id, itemStatus: newVal })
    message.success(t('common.feedback.updated'))
    
  } catch (error: unknown) {
    if (row) {
      row.itemStatus = oldVal
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
  itemCode: '',
  itemName: '',
  shortName: '',
  itemType: undefined as number | undefined,
  calcMethod: undefined as number | undefined,
  salaryFormulaId: '',
  defaultAmount: undefined as number | undefined,
  defaultRate: undefined as number | undefined,
  strikePrice: undefined as number | undefined,
  vestingYears: undefined as number | undefined,
  isDeduction: undefined as number | undefined,
  isTaxable: undefined as number | undefined,
  includeSocialSecurityBase: undefined as number | undefined,
  includeHousingFundBase: undefined as number | undefined,
  itemStatus: undefined as number | undefined,
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

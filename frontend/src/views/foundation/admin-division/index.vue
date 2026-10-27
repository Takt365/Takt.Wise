<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/foundation/admin-division -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：行政区划实体树表管理页（仅 tree API；右表选中后展示子孙），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="foundation-admin-division">
    <!-- 查询栏 -->
    <div class="foundation-admin-division-query-row">
      <TaktTreeLeftQueryBar
        v-model="treeQueryKeyword"
        @search="handleTreeQuerySearch"
      />
      <TaktTreeRightQueryBar
        v-model="queryKeyword"
        :placeholder="tableSearchPlaceholder"
        :loading="loading"
        @search="handleSearch"
        @reset="handleReset"
      />
    </div>

    <!-- 工具栏 -->
    <div class="foundation-admin-division-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadData"
      />
      <TaktTreeRightToolsBar
        create-permission="foundation:admin:division:create"
        update-permission="foundation:admin:division:update"
        delete-permission="foundation:admin:division:delete"
        import-permission="foundation:admin:division:import"
        export-permission="foundation:admin:division:export"
        :show-create="true"
        :show-update="true"
        :show-delete="true"
        :show-import="true"
        :show-export="true"
        :show-advanced-query="true"
        :show-column-setting="true"
        :show-fullscreen="true"
        :show-refresh="true"
        :show-expand="true"
        :update-disabled="!selecmedRow"
        :delete-disabled="!selecmedRow && selecmedRows.length === 0"
        :create-loading="loading"
        :update-loading="loading"
        :delete-loading="loading"
        :refresh-loading="loading"
        :expanded="tableExpanded"
        @create="handleCreate"
        @update="handleUpdate"
        @delete="handleDelete"
        @import="handleImport"
        @export="handleExport"
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 左树右表：左导航树；右表仅在选中后展示该节点全部子孙树 -->
    <div class="foundation-admin-division-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selecmed-keys="selecmedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        :accordion="false"
        :expand-action="false"
        :load-data="useLazyTree ? handleLeftTreeLoadData : undefined"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="tenant"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'adminDivisionId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :load-children="useLazyTree ? handleRightTreeLoadChildren : undefined"
        :loading="listLoading"
        :row-key="getAdminDivisionId"
        :stripe="true"
        :row-selection="rowSelection"
        :virtual="true"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'divisionName'">
            <span>{{ getAdminDivisionField(record, 'divisionName') }}</span>
          </template>
        <template v-else-if="column.key === 'divisionStatus'">
          <a-switch
            :checked="getAdminDivisionDictValue(record, 'divisionStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleDivisionStatusChange(record, Boolean(checked))"
          />
        </template>
          <template v-else-if="column.key === 'countryCode'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'countryCode')"
              dict-type="sys_country_code"
            />
          </template>
          <template v-else-if="column.key === 'level'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'level')"
              dict-type="sys_admin_division_level"
            />
          </template>
          <template v-else-if="column.key === 'isLeaf'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'isLeaf')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'currencyCode'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'currencyCode')"
              dict-type="accounting_financial_currency_code"
            />
          </template>
          <template v-else-if="column.key === 'isBuiltIn'">
            <TaktDictTag
              :value="getAdminDivisionDictValue(record, 'isBuiltIn')"
              dict-type="sys_yes_no"
            />
          </template>
        </template>
      </TaktTreeRightTable>
    </div>

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
      <AdminDivisionForm
        :key="formData?.adminDivisionId ?? 'create'"
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
      :storage-key="'takt-query-fields-foundation-admin-division'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('countryCode')">
      <a-form-item :label="pi.queryLabel('countryCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.countryCode"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('countryCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionCode')">
      <a-form-item :label="pi.queryLabel('divisionCode')">
        <a-input
          v-model:value="advancedQueryForm.divisionCode"
          :placeholder="pi.queryPh('divisionCode', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionName')">
      <a-form-item :label="pi.queryLabel('divisionName')">
        <a-input
          v-model:value="advancedQueryForm.divisionName"
          :placeholder="pi.queryPh('divisionName', 'required')"
          show-count
          :maxlength="200"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('parentId')">
      <a-form-item :label="pi.queryLabel('parentId')">
        <a-input
          v-model:value="advancedQueryForm.parentId"
          :placeholder="pi.queryPh('parentId', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('level')">
      <a-form-item :label="pi.queryLabel('level')">
        <TaktSelect
          v-model:value="advancedQueryForm.level"
          dict-type="sys_admin_division_level"
          :placeholder="pi.queryPh('level', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionPath')">
      <a-form-item :label="pi.queryLabel('divisionPath')">
        <a-input
          v-model:value="advancedQueryForm.divisionPath"
          :placeholder="pi.queryPh('divisionPath', 'required')"
          show-count
          :maxlength="500"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isLeaf')">
      <a-form-item :label="pi.queryLabel('isLeaf')">
        <TaktSelect
          v-model:value="advancedQueryForm.isLeaf"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isLeaf', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('postalCode')">
      <a-form-item :label="pi.queryLabel('postalCode')">
        <a-input
          v-model:value="advancedQueryForm.postalCode"
          :placeholder="pi.queryPh('postalCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('currencyCode')">
      <a-form-item :label="pi.queryLabel('currencyCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.currencyCode"
          dict-type="accounting_financial_currency_code"
          :placeholder="pi.queryPh('currencyCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('phoneCode')">
      <a-form-item :label="pi.queryLabel('phoneCode')">
        <a-input
          v-model:value="advancedQueryForm.phoneCode"
          :placeholder="pi.queryPh('phoneCode', 'required')"
          show-count
          :maxlength="16"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBuiltIn')">
      <a-form-item :label="pi.queryLabel('isBuiltIn')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBuiltIn"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBuiltIn', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('divisionStatus')">
      <a-form-item :label="pi.queryLabel('divisionStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.divisionStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('divisionStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
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
            <span>{{ pi.queryLabel('extField') }}</span>
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
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
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
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="ADMINDIVISION_SELF_I18N_KEY"
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
      entity-scope="tenant"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'adminDivisionId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 行政区划实体树表管理页 · 左树选中后右表展示该节点+直接子级（更深展开懒加载）；默认右表为空
 * @module views/foundation/admin-division
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import {
  filterTaktTreeTableNodes,
  collectTaktTreeTableExpandableKeys,
  expandTaktLazyTreeFully,
  runWithTaktTreeLoadConcurrency,
  taktTreeExpandedKeysEqual,
  taktTreeTableNodeKey,
  type TaktTreeTableNode,
} from '@/utils/taktTreeTable'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
  type TaktLazyTreeNode,
} from '@/composables/useLazyTree'
import AdminDivisionForm from './components/AdminDivisionForm.vue'
import { getAdminDivisionTree, getAdminDivisionById, createAdminDivision, updateAdminDivision, deleteAdminDivisionById, deleteAdminDivisionBatch, getAdminDivisionTemplate, importAdminDivision, exportAdminDivision, updateAdminDivisionStatus, updateAdminDivisionSort } from '@/api/foundation/adminDivision.api'
import type { AdminDivision, AdminDivisionTree, AdminDivisionUpdate } from '@/types/foundation/adminDivision.types'
import type { TreeDropPayload } from '@/components/business/TaktTreeLeftTable/index.vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useAdminDivisionI18n,
  ADMINDIVISION_QUERY_STRING_FIELDS,
  ADMINDIVISION_QUERY_FIELDS,
  ADMINDIVISION_SELF_I18N_KEY,
} from './composables/useAdminDivisionI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useAdminDivisionI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAdminDivision')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [pi.label('divisionName'), pi.label('divisionCode')].join(' / '),
  })
)

/** 左侧树关键字（仅过滤已加载节点，不重复请求 API） */
const treeQueryKeyword = ref('')
/** 右侧树表快捷查询关键字 */
const queryKeyword = ref('')
/** 左侧树工具栏「展开/收缩」状态（仅已加载层） */
const treeExpanded = ref(false)
/** 左侧树当前展开的节点 key 列表 */
const treeExpandedKeys = ref<(string | number)[]>([])
/** 右侧树表工具栏「全部展开/收缩」 */
const tableExpanded = ref(false)
/** 右侧 a-table 树表当前展开行 key */
const tableExpandedRowKeys = ref<(string | number)[]>([])
/** 达到阈值后左右树均按 parentId 一层懒加载 */
const useLazyTree = ref(true)
/** 左侧树 loading */
const loading = ref(false)
/** 右侧树 loading */
const listLoading = ref(false)
/** 左侧 a-tree 数据（懒加载仅已展开路径；低于阈值时为全量树） */
const entityTreeData = ref<TaktLazyTreeNode[]>([])
/** 右侧树表数据源（带 children / _hasChildren，组件内拍平 virtual） */
const tableTreeData = ref<Record<string, unknown>[]>([])
/** 左侧树当前选中的节点 key 列表 */
const selecmedTreeKeys = ref<(string | number)[]>([])
/** 工具栏单选时当前行（编辑/删除） */
const selecmedRow = ref<AdminDivisionRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<AdminDivisionRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AdminDivision> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of ADMINDIVISION_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.level !== undefined && form.level !== null) {
    return true
  }
  if (form.isLeaf !== undefined && form.isLeaf !== null) {
    return true
  }
  if (form.isBuiltIn !== undefined && form.isBuiltIn !== null) {
    return true
  }
  if (form.divisionStatus !== undefined && form.divisionStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ADMINDIVISION_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ADMINDIVISION_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    level: undefined as number | undefined,
    isLeaf: undefined as number | undefined,
    isBuiltIn: undefined as number | undefined,
    divisionStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ADMINDIVISION_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'adminDivisionId'
/** 树节点标题字段名（左侧树 title） */
const treeTitleField = 'divisionName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matchesAdminDivisionRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.divisionName ?? '').toLowerCase().includes(k) && !String(record.divisionCode ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.countryCode && !String(record.countryCode ?? '').includes(String(advancedQueryForm.value.countryCode))) return false
  if (advancedQueryForm.value.divisionCode && !String(record.divisionCode ?? '').includes(String(advancedQueryForm.value.divisionCode))) return false
  if (advancedQueryForm.value.divisionName && !String(record.divisionName ?? '').includes(String(advancedQueryForm.value.divisionName))) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.level !== undefined && record.level !== advancedQueryForm.value.level) return false
  if (advancedQueryForm.value.divisionPath && !String(record.divisionPath ?? '').includes(String(advancedQueryForm.value.divisionPath))) return false
  if (advancedQueryForm.value.isLeaf !== undefined && record.isLeaf !== advancedQueryForm.value.isLeaf) return false
  if (advancedQueryForm.value.postalCode && !String(record.postalCode ?? '').includes(String(advancedQueryForm.value.postalCode))) return false
  if (advancedQueryForm.value.currencyCode && !String(record.currencyCode ?? '').includes(String(advancedQueryForm.value.currencyCode))) return false
  if (advancedQueryForm.value.phoneCode && !String(record.phoneCode ?? '').includes(String(advancedQueryForm.value.phoneCode))) return false
  if (advancedQueryForm.value.isBuiltIn !== undefined && record.isBuiltIn !== advancedQueryForm.value.isBuiltIn) return false
  if (advancedQueryForm.value.divisionStatus !== undefined && record.divisionStatus !== advancedQueryForm.value.divisionStatus) return false
  if (advancedQueryForm.value.createdAtStart && !String(record.createdAtStart ?? '').includes(String(advancedQueryForm.value.createdAtStart))) return false
  if (advancedQueryForm.value.createdAtEnd && !String(record.createdAtEnd ?? '').includes(String(advancedQueryForm.value.createdAtEnd))) return false
  if (advancedQueryForm.value.extField && !String(record.extField ?? '').includes(String(advancedQueryForm.value.extField))) return false
  if (advancedQueryForm.value.remark && !String(record.remark ?? '').includes(String(advancedQueryForm.value.remark))) return false
  return true
}

/**
 * 将树 API 一层 DTO 映射为左侧懒加载节点
 * @param rows 一层子节点
 */
function mapAdminDivisionLazyNodes(rows: AdminDivisionTree[]): TaktLazyTreeNode[] {
  return mapLazyTreeNodes(rows, {
    getKey: (n) => String(n.adminDivisionId ?? ''),
    getTitle: (n) => String(n.divisionName || n.adminDivisionCode || n.adminDivisionId || ''),
    isLeaf: (n) => taktIsLeafFlag((n as { isLeaf?: unknown }).isLeaf),
  })
}

/**
 * 将一层 DTO 映射为右侧树表节点（未加载子级用 _hasChildren 显示展开箭头）
 * @param rows 一层子节点
 */
function mapAdminDivisionRightTreeNodes(rows: AdminDivisionTree[]): Record<string, unknown>[] {
  return (rows ?? []).map((row) => {
    const rec = row as Record<string, unknown>
    const id = String(rec.adminDivisionId ?? '')
    const rawChildren = Array.isArray(rec.children) ? rec.children as AdminDivisionTree[] : []
    const children = rawChildren.length > 0 ? mapAdminDivisionRightTreeNodes(rawChildren) : undefined
    return {
      ...rec,
      key: id,
      children,
      _hasChildren: (children != null && children.length > 0) || !taktIsLeafFlag(rec.isLeaf),
    }
  })
}

/**
 * 按关键字过滤左侧树：仅过滤已加载节点（大规模树不做全量搜索）
 * @param nodes 树节点
 * @param keyword 关键字
 */
function filterTreeByKeyword(nodes: TaktLazyTreeNode[], keyword: string): TaktLazyTreeNode[] {
  const k = (keyword ?? '').trim().toLowerCase()
  if (!k) return nodes
  /** 递归过滤子树 */
  function filter(list: TaktLazyTreeNode[]): TaktLazyTreeNode[] {
    if (!list?.length) return []
    return list
      .map((node) => {
        const title = String(node.title ?? '').toLowerCase()
        const matched = title.includes(k)
        const filteredChildren = node.children?.length ? filter(node.children as TaktLazyTreeNode[]) : undefined
        const hasMatchInChildren = filteredChildren != null && filteredChildren.length > 0
        if (matched || hasMatchInChildren) {
          if (filteredChildren != null && filteredChildren.length > 0) {
            return { ...node, children: filteredChildren }
          }
          const { children: _omitChildren, ...rest } = node
          return rest as TaktLazyTreeNode
        }
        return null
      })
      .filter(Boolean) as TaktLazyTreeNode[]
  }
  return filter(nodes)
}

/** 左侧树绑定数据（按 treeQueryKeyword 客户端过滤已加载节点） */
const filteredTreeData = computed(() =>
  filterTreeByKeyword(entityTreeData.value, treeQueryKeyword.value)
)

/**
 * 收集左侧可展开 key（含尚未拉子的非叶子，供工具栏一次展开逐层 loadData）
 * @param nodes 树节点
 */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  return collectTaktTreeTableExpandableKeys(
    nodes,
    (node) => taktTreeTableNodeKey(node, 'adminDivisionId'),
    { includeUnloaded: true },
  )
}

/**
 * 展开态下把「当前树中全部可展开节点」写入 expandedKeys（子节点 load 完后会再触发，直至拉齐）
 */
function applyLeftTreeExpandKeys() {
  const next = collectTreeExpandableKeys(filteredTreeData.value)
  if (taktTreeExpandedKeysEqual(treeExpandedKeys.value, next)) return
  treeExpandedKeys.value = next
}

/**
 * 当前左侧选中节点 Id；未选中返回 null（右表必须为空）
 * @returns {string | null} 选中节点 Id
 */
function getSelecmedTreeNodeId(): string | null {
  const keys = selecmedTreeKeys.value
  if (keys.length > 0 && keys[keys.length - 1] != null) {
    return String(keys[keys.length - 1])
  }
  return null
}

/** 从树 API 响应中取出一层节点 */
function unwrapAdminDivisionTree(res: unknown): AdminDivisionTree[] {
  const resAny = res as { data?: AdminDivisionTree[]; Data?: AdminDivisionTree[] }
  if (Array.isArray(res)) return res as AdminDivisionTree[]
  return resAny?.data ?? resAny?.Data ?? []
}

/**
 * 加载右侧树：仅 tree API；未选中则空；选中则该节点 + 直接子级一层（更深靠展开懒加载）
 */
async function loadRightTree() {
  const selecmedId = getSelecmedTreeNodeId()
  if (!selecmedId) {
    tableTreeData.value = []
    tableExpandedRowKeys.value = []
    tableExpanded.value = false
    return
  }
  listLoading.value = true
  try {
    const detail = await getAdminDivisionById(selecmedId) as Record<string, unknown>
    const childRows = unwrapAdminDivisionTree(await getAdminDivisionTree(selecmedId, true))
    const mappedChildren = mapAdminDivisionRightTreeNodes(childRows)
    const children = mappedChildren.length > 0 ? mappedChildren : undefined
    const root: Record<string, unknown> = {
      ...detail,
      key: selecmedId,
      children,
      _hasChildren: (children?.length ?? 0) > 0 || !taktIsLeafFlag(detail.isLeaf),
    }
    tableTreeData.value = [root]
    // 选中仅加载一层：取消「全部展开」任务；不触发全量子树请求（更深靠行内懒加载 / 工具栏）
    rightExpandEpoch += 1
    if (tableExpanded.value) {
      tableExpanded.value = false
      await nextTick()
    }
    tableExpandedRowKeys.value =
      (children?.length ?? 0) > 0 || root._hasChildren === true ? [selecmedId] : []
  } catch (error: unknown) {
    logger.error('[AdminDivision] 加载右侧树失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    tableTreeData.value = []
  } finally {
    listLoading.value = false
  }
}

/**
 * 按父级 Id 拉取并合并右侧一层子节点
 * @param parentId 父节点 Id
 */
async function loadRightChildrenByParentId(parentId: string) {
  if (!parentId) return
  const trees = unwrapAdminDivisionTree(await getAdminDivisionTree(parentId, true))
  const children = mapAdminDivisionRightTreeNodes(trees)
  tableTreeData.value = mergeLoadedChildren(
    tableTreeData.value as TaktLazyTreeNode[],
    parentId,
    children as TaktLazyTreeNode[],
    { keyField: 'adminDivisionId' },
  )
}

/**
 * 右侧树展开：再拉一层子节点（懒加载，并发受限）
 * @param record 当前行
 */
async function handleRightTreeLoadChildren(record: Record<string, unknown>) {
  const id = getAdminDivisionId(record)
  if (!id) return
  await runWithTaktTreeLoadConcurrency(async () => {
    await loadRightChildrenByParentId(id)
  })
}

/**
 * 加载左侧树根节点（仅 GET tree?parentId=0，一层）
 */
async function reloadLeftTreeRoots() {
  const trees = unwrapAdminDivisionTree(await getAdminDivisionTree('0', true))
  entityTreeData.value = mapAdminDivisionLazyNodes(trees)
  treeExpandedKeys.value = []
  treeExpanded.value = false
}

/**
 * 重新加载指定父节点下已展开的子节点（CRUD 后局部刷新）
 * @param parentKey 父节点 key
 */
async function reloadLeftTreeChildren(parentKey: string) {
  if (!parentKey || parentKey === '0') {
    await reloadLeftTreeRoots()
    return
  }
  const trees = unwrapAdminDivisionTree(await getAdminDivisionTree(parentKey, true))
  const children = mapAdminDivisionLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentKey,
    children,
  ) as TaktLazyTreeNode[]
}

/**
 * 按父级 Id 拉取并合并左侧一层子节点
 * @param parentId 父节点 Id
 * @returns {Promise<TaktLazyTreeNode[]>} 子节点
 */
async function loadLeftChildrenByParentId(parentId: string): Promise<TaktLazyTreeNode[]> {
  if (!parentId) return []
  const trees = unwrapAdminDivisionTree(await getAdminDivisionTree(parentId, true))
  const children = mapAdminDivisionLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentId,
    children,
  ) as TaktLazyTreeNode[]
  return children
}

/**
 * 左侧树懒加载子节点（Ant Design loadData；并发受限）
 * @param treeNode Ant Design Tree 节点
 */
async function handleLeftTreeLoadData(treeNode: Record<string, unknown>) {
  const dataRef = (treeNode.dataRef ?? treeNode) as Record<string, unknown>
  const key = dataRef.key ?? treeNode.key
  if (key == null) return
  if (Array.isArray(dataRef.children) && dataRef.children.length > 0) return
  await runWithTaktTreeLoadConcurrency(async () => {
    dataRef.children = await loadLeftChildrenByParentId(String(key))
  })
}

/** 右侧展示树：未选中为空；选中后为该节点+直接子级（仅 tree API） */
const tableDisplayTree = computed(() => {
  if (!getSelecmedTreeNodeId()) return []
  return tableTreeData.value
})

/** 右侧过滤后的树（保留 children，供组件按展开路径拍平） */
const tableFilteredTree = computed(() =>
  filterTaktTreeTableNodes(tableDisplayTree.value, matchesAdminDivisionRightQuery)
)

/**
 * 同步右侧树表 expandable keys（展开态；含未加载非叶子）
 */
function applyAdminDivisionTableExpandState() {
  if (!tableExpanded.value) {
    tableExpandedRowKeys.value = []
    return
  }
  const next = collectTaktTreeTableExpandableKeys(
    tableFilteredTree.value,
    (node) => taktTreeTableNodeKey(node, 'adminDivisionId'),
    { includeUnloaded: true },
  )
  if (!taktTreeExpandedKeysEqual(tableExpandedRowKeys.value, next)) {
    tableExpandedRowKeys.value = next
  }
}

/** 右侧工具栏展开任务世代 */
let rightExpandEpoch = 0

/**
 * 右侧一次全部展开：仅工具栏触发；按层拉齐当前右表子树（选中节点不会自动走此路径）
 */
async function expandRightTreeFully() {
  const epoch = (rightExpandEpoch += 1)
  await expandTaktLazyTreeFully({
    getNodes: () => tableFilteredTree.value as TaktTreeTableNode[],
    getKey: (node) => taktTreeTableNodeKey(node, 'adminDivisionId'),
    setExpandedKeys: (keys) => {
      if (epoch !== rightExpandEpoch) return
      if (!taktTreeExpandedKeysEqual(tableExpandedRowKeys.value, keys)) {
        tableExpandedRowKeys.value = keys
      }
    },
    loadChildren: async (parentId) => {
      await loadRightChildrenByParentId(parentId)
    },
    isActive: () => tableExpanded.value && epoch === rightExpandEpoch,
  })
}

/** 右侧工具栏展开/收缩：展开才全量拉齐；选中节点不经过此 watch */
watch(tableExpanded, async (expanded) => {
  if (!expanded) {
    rightExpandEpoch += 1
    tableExpandedRowKeys.value = []
    return
  }
  await expandRightTreeFully()
})

watch(tableFilteredTree, () => {
  if (tableExpanded.value) {
    applyAdminDivisionTableExpandState()
  }
})

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param adminDivision 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {AdminDivisionUpdate} 更新载荷
 */
function buildAdminDivisionUpdateDto(
  adminDivision: AdminDivision,
  overrides: Pick<AdminDivisionUpdate, 'parentId'> & { sortOrder: number },
): AdminDivisionUpdate {
  return {
    adminDivisionId: String(adminDivision.adminDivisionId),
    tenantCode: adminDivision.tenantCode,
    countryCode: adminDivision.countryCode,
    divisionCode: adminDivision.divisionCode,
    divisionName: adminDivision.divisionName,
    parentId: overrides.parentId,
    divisionPath: adminDivision.divisionPath,
    postalCode: adminDivision.postalCode,
    currencyCode: adminDivision.currencyCode,
    phoneCode: adminDivision.phoneCode,
    isBuiltIn: adminDivision.isBuiltIn,
    divisionStatus: adminDivision.divisionStatus,
    extField: adminDivision.extField,
    remark: adminDivision.remark,
  }
}

/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.adminDivisionId ?? '')
    if (k === keyStr) {
      return { parentId: parentKey, sortOrder: i }
    }
    const children = (node?.children as Array<Record<string, unknown>> | undefined) ?? []
    if (children.length) {
      const found = findParentAndOrderNum(children, targetKey, k)
      if (found) return found
    }
  }
  return null
}

/**
 * 左侧树拖拽完成后更新 parentId 与 sortOrder
 * @param payload 新树数据与被拖拽节点 key
 */
const handleTreeDrop = async (payload: TreeDropPayload) => {
  const { newTreeData, dragKey } = payload
  const pos = findParentAndOrderNum(newTreeData, dragKey)
  if (!pos) return
  try {
    loading.value = true
    entityTreeData.value = newTreeData as TaktLazyTreeNode[]
    const full = await getAdminDivisionById(String(dragKey))
    await updateAdminDivision(String(dragKey), buildAdminDivisionUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    await updateAdminDivisionSort({ adminDivisionId: String(dragKey), sortOrder: pos.sortOrder })
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await reloadLeftTreeRoots().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

/** 左侧工具栏展开任务世代（收缩或再次展开时作废上一次全量展开） */
let leftExpandEpoch = 0

/** 左侧树关键字搜索（仅过滤已加载节点） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    applyLeftTreeExpandKeys()
  }
}

/** 左侧工具栏展开/收缩：一次展开主动按层拉齐全部非叶子（不再依赖多次点击） */
watch(treeExpanded, async (expanded) => {
  if (!expanded) {
    leftExpandEpoch += 1
    treeExpandedKeys.value = []
    return
  }
  const epoch = (leftExpandEpoch += 1)
  await nextTick()
  if (epoch !== leftExpandEpoch || !treeExpanded.value) return
  await expandTaktLazyTreeFully({
    getNodes: () => filteredTreeData.value as TaktTreeTableNode[],
    getKey: (node) => taktTreeTableNodeKey(node, 'adminDivisionId'),
    setExpandedKeys: (keys) => {
      if (epoch !== leftExpandEpoch) return
      if (!taktTreeExpandedKeysEqual(treeExpandedKeys.value, keys)) {
        treeExpandedKeys.value = keys
      }
    },
    loadChildren: async (parentId) => {
      await loadLeftChildrenByParentId(parentId)
    },
    isActive: () => treeExpanded.value && epoch === leftExpandEpoch,
  })
})

/** 三角展开 loadData 后：若工具栏仍为展开态，补齐 expandable keys */
watch(filteredTreeData, () => {
  if (treeExpanded.value) {
    applyLeftTreeExpandKeys()
  }
})

/** 左侧树选中：右表展示该节点+直接子级；取消选中则右表清空 */
const handleTreeSelect = (selecmedKeys: (string | number)[]) => {
  selecmedTreeKeys.value = selecmedKeys
  void loadRightTree()
}

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type AdminDivisionRowRecord = AdminDivision | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getAdminDivisionId = (record: AdminDivisionRowRecord): string => {
  if (record != null && 'adminDivisionId' in record && (record as Record<string, unknown>).adminDivisionId != null) {
    return String((record as Record<string, unknown>).adminDivisionId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getAdminDivisionField = (record: AdminDivisionRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getAdminDivisionDictValue = (
  record: AdminDivisionRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
/** 将行字段/字典值转为有限 number */
const toAdminDivisionNumber = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}

/** 从异常对象提取用户可见消息 */
const getErrorMessage = (error: unknown, fallback: string): string => {
  if (typeof error === 'object' && error !== null && 'message' in error) {
    const messageText = (error as { message?: unknown }).message
    if (typeof messageText === 'string' && messageText.trim()) return messageText
  }
  return fallback
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = ref<TableColumnsType>([])
watchEffect(() => {
  columns.value = [
  {
    title: t('common.page.entity.id'),
    dataIndex: 'adminDivisionId',
    key: 'adminDivisionId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getAdminDivisionField(record, 'adminDivisionId') ?? getAdminDivisionField(record, 'id') ?? '',
  },
  {
    title: pi.label('countryCode'),
    dataIndex: 'countryCode',
    key: 'countryCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionCode'),
    dataIndex: 'divisionCode',
    key: 'divisionCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'divisionCode') ?? ''
  },
  {
    title: pi.label('divisionName'),
    dataIndex: 'divisionName',
    key: 'divisionName',
    width: 160,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('parentId'),
    dataIndex: 'parentId',
    key: 'parentId',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('level'),
    dataIndex: 'level',
    key: 'level',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionPath'),
    dataIndex: 'divisionPath',
    key: 'divisionPath',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'divisionPath') ?? ''
  },
  {
    title: pi.label('isLeaf'),
    dataIndex: 'isLeaf',
    key: 'isLeaf',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('postalCode'),
    dataIndex: 'postalCode',
    key: 'postalCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'postalCode') ?? ''
  },
  {
    title: pi.label('currencyCode'),
    dataIndex: 'currencyCode',
    key: 'currencyCode',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('phoneCode'),
    dataIndex: 'phoneCode',
    key: 'phoneCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAdminDivisionField(record, 'phoneCode') ?? ''
  },
  {
    title: pi.label('isBuiltIn'),
    dataIndex: 'isBuiltIn',
    key: 'isBuiltIn',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('divisionStatus'),
    dataIndex: 'divisionStatus',
    key: 'divisionStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn<AdminDivision>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'foundation:admin:division:update',
        onClick: (record: AdminDivisionRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'foundation:admin:division:delete',
        onClick: (record: AdminDivisionRowRecord) => handleDeleteOne(record)
      }
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AdminDivision[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: AdminDivisionRowRecord, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getAdminDivisionId(selecmedRow.value) === getAdminDivisionId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: AdminDivision[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? selecmedRowsData[0] : null
  },
}))

/** 加载左树：仅 GET …/tree?parentId=0；默认不选中、右表为空（禁止走 list） */
async function loadData() {
  loading.value = true
  try {
    useLazyTree.value = true
    selecmedTreeKeys.value = []
    tableTreeData.value = []
    tableExpandedRowKeys.value = []
    tableExpanded.value = false
    await reloadLeftTreeRoots()
  } catch (error: unknown) {
    logger.error('[AdminDivision] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    entityTreeData.value = []
    tableTreeData.value = []
  } finally {
    loading.value = false
  }
}

/**
 * CRUD / 状态变更后：刷新左树当前层 + 按选中重载右表（仅 tree）
 */
async function refreshAfterMutation() {
  loading.value = true
  try {
    const selecmedId = getSelecmedTreeNodeId()
    await reloadLeftTreeChildren(selecmedId ?? '0')
    await loadRightTree()
  } catch (error: unknown) {
    logger.error('[AdminDivision] 刷新失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
  } finally {
    loading.value = false
  }
}

/** 右侧查询（客户端过滤已加载树节点） */
const handleSearch = () => {}

/** 右侧重置（客户端过滤，不重建树） */
const handleReset = () => {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function handleDivisionStatusChange(record: AdminDivisionRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toAdminDivisionNumber(getAdminDivisionDictValue(record, 'divisionStatus'))
  const id = getAdminDivisionId(record)
  try {
    await updateAdminDivisionStatus({ adminDivisionId: id, divisionStatus: newVal })
    message.success(t('common.feedback.updated'))
    await loadData()
  } catch (error: unknown) {
    message.error(t('common.feedback.failed'))
  }
}

/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  const keys = selecmedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}

/** 打开编辑弹窗 */
function handleEdit(record: AdminDivisionRowRecord) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selecmedRow.value) {
    handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
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
      await updateAdminDivision(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createAdminDivision(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    await refreshAfterMutation()
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

/** 删除单行 */
async function handleDeleteOne(record: AdminDivisionRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAdminDivisionById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}

/** 批量删除选中行 */
async function handleDelete() {
  if (selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selecmedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteAdminDivisionBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}

/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getAdminDivisionTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAdminDivision(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  void refreshAfterMutation()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}

/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await exportAdminDivision({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: unknown) {
    logger.error('[AdminDivision] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: pi.self() })))
  } finally {
    loading.value = false
  }
}

/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉，客户端过滤右侧树 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
}

/** 重置高级查询表单（不自动查询） */
function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
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

/** 刷新树数据 */
function handleRefresh() {
  void loadData()
}

/** 表格 change / 列宽拖拽占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}

/** 页面挂载：仅拉左树根（tree API）；右表待选中 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
useTableRefresh(loadData)
</script>

<style scoped lang="css">
.foundation-admin-division {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.foundation-admin-division-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.foundation-admin-division-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.foundation-admin-division-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>

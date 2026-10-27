<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：会计科目实体树表管理页（左右树表，超阈值懒加载+virtual，无分页），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="accounting-financial-account-title">
    <!-- 查询栏 -->
    <div class="accounting-financial-account-title-query-row">
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
    <div class="accounting-financial-account-title-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="reloadLeftTreeRoots"
      />
      <TaktTreeRightToolsBar
        create-permission="accounting:financial:account:title:create"
        update-permission="accounting:financial:account:title:update"
        delete-permission="accounting:financial:account:title:delete"
        import-permission="accounting:financial:account:title:import"
        export-permission="accounting:financial:account:title:export"
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

    <!-- 左树右表（均为树表：懒加载+virtual，无分页） -->
    <div class="accounting-financial-account-title-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selecmed-keys="selecmedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="true"
        :load-data="useLazyTree ? handleLeftTreeLoadData : undefined"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="company"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'accountTitleId'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :load-children="useLazyTree ? handleRightTreeLoadChildren : undefined"
        :loading="listLoading"
        :row-key="getAccountTitleId"
        :stripe="true"
        :row-selection="rowSelection"
        :virtual="true"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'accountTitleName'">
            <span>{{ getAccountTitleField(record, 'accountTitleName') }}</span>
          </template>
        <template v-else-if="column.key === 'accountTitleStatus'">
          <a-switch
            :checked="getAccountTitleDictValue(record, 'accountTitleStatus') === 1"
            :checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"
            @change="(checked: unknown) => handleAccountTitleStatusChange(record, Boolean(checked))"
          />
        </template>
          <template v-else-if="column.key === 'accountTitleType'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'accountTitleType')"
              dict-type="accounting_financial_account_title_type"
            />
          </template>
          <template v-else-if="column.key === 'isLeaf'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isLeaf')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'isAuxiliary'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isAuxiliary')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'auxiliaryType'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'auxiliaryType')"
              dict-type="accounting_financial_auxiliary_type"
            />
          </template>
          <template v-else-if="column.key === 'isQuantity'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isQuantity')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'isCurrency'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isCurrency')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'isCash'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isCash')"
              dict-type="sys_yes_no"
            />
          </template>
          <template v-else-if="column.key === 'isBank'">
            <TaktDictTag
              :value="getAccountTitleDictValue(record, 'isBank')"
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
      <AccountTitleForm
        :key="formData?.accountTitleId ?? 'create'"
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
      :storage-key="'takt-query-fields-accounting-financial-account-title'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('cultureCode')">
      <a-form-item :label="pi.queryLabel('cultureCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.cultureCode"
          dict-type="sys_culture_code"
          :placeholder="pi.queryPh('cultureCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleCode')">
      <a-form-item :label="pi.queryLabel('accountTitleCode')">
        <a-input
          v-model:value="advancedQueryForm.accountTitleCode"
          :placeholder="pi.queryPh('accountTitleCode', 'required')"
          show-count
          :maxlength="50"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleName')">
      <a-form-item :label="pi.queryLabel('accountTitleName')">
        <a-input
          v-model:value="advancedQueryForm.accountTitleName"
          :placeholder="pi.queryPh('accountTitleName', 'required')"
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
      <div v-show="isFieldVisible('accountTitleType')">
      <a-form-item :label="pi.queryLabel('accountTitleType')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitleType"
          dict-type="accounting_financial_account_title_type"
          :placeholder="pi.queryPh('accountTitleType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('balanceDirection')">
      <a-form-item :label="pi.queryLabel('balanceDirection')">
        <a-input-number
          v-model:value="advancedQueryForm.balanceDirection"
          :placeholder="pi.queryPh('balanceDirection', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleLevel')">
      <a-form-item :label="pi.queryLabel('accountTitleLevel')">
        <a-input-number
          v-model:value="advancedQueryForm.accountTitleLevel"
          :placeholder="pi.queryPh('accountTitleLevel', 'required')"
          style="width: 100%"
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
      <div v-show="isFieldVisible('isAuxiliary')">
      <a-form-item :label="pi.queryLabel('isAuxiliary')">
        <TaktSelect
          v-model:value="advancedQueryForm.isAuxiliary"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isAuxiliary', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('auxiliaryType')">
      <a-form-item :label="pi.queryLabel('auxiliaryType')">
        <TaktSelect
          v-model:value="advancedQueryForm.auxiliaryType"
          dict-type="accounting_financial_auxiliary_type"
          :placeholder="pi.queryPh('auxiliaryType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isQuantity')">
      <a-form-item :label="pi.queryLabel('isQuantity')">
        <TaktSelect
          v-model:value="advancedQueryForm.isQuantity"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isQuantity', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCurrency')">
      <a-form-item :label="pi.queryLabel('isCurrency')">
        <TaktSelect
          v-model:value="advancedQueryForm.isCurrency"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isCurrency', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isCash')">
      <a-form-item :label="pi.queryLabel('isCash')">
        <TaktSelect
          v-model:value="advancedQueryForm.isCash"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isCash', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('isBank')">
      <a-form-item :label="pi.queryLabel('isBank')">
        <TaktSelect
          v-model:value="advancedQueryForm.isBank"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('isBank', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromStart')">
      <a-form-item :label="pi.queryLabel('validFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromStart"
          :placeholder="pi.queryPh('validFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromEnd')">
      <a-form-item :label="pi.queryLabel('validFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromEnd"
          :placeholder="pi.queryPh('validFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToStart')">
      <a-form-item :label="pi.queryLabel('validToStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToStart"
          :placeholder="pi.queryPh('validToStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToEnd')">
      <a-form-item :label="pi.queryLabel('validToEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToEnd"
          :placeholder="pi.queryPh('validToEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('accountTitleStatus')">
      <a-form-item :label="pi.queryLabel('accountTitleStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.accountTitleStatus"
          dict-type="sys_normal_disable"
          :placeholder="pi.queryPh('accountTitleStatus', 'select')"
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
        :entity-i18n-key="ACCOUNTTITLE_SELF_I18N_KEY"
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
      entity-scope="company"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'accountTitleId'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * 会计科目实体树表管理页 · 左右均为树表：超阈值懒加载+virtual，无分页
 * @module views/accounting/financial/account-title
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktMaxPageSize } from '@/utils/taktPaged'
import { shouldUseTreeLazyLoad } from '@/utils/taktLargeData'
import {
  buildTaktTreeFromFlat,
  filterTaktTreeTableNodes,
  collectTaktTreeTableExpandableKeys,
  findTaktTreeTableSubtree,
  taktTreeTableNodeKey,
} from '@/utils/taktTreeTable'
import { useTableRefresh } from '@/composables/useTableRefresh'
import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
  type TaktLazyTreeNode,
} from '@/composables/useLazyTree'
import AccountTitleForm from './components/AccountTitleForm.vue'
import { getAccountTitleList, getAccountTitleTree, getAccountTitleById, createAccountTitle, updateAccountTitle, deleteAccountTitleById, deleteAccountTitleBatch, getAccountTitleTemplate, importAccountTitle, exportAccountTitle, updateAccountTitleStatus, updateAccountTitleSort } from '@/api/accounting/financial/accountTitle.api'
import type { AccountTitle, AccountTitleTree, AccountTitleUpdate } from '@/types/accounting/financial/accountTitle.types'
import type { TreeDropPayload } from '@/components/business/TaktTreeLeftTable/index.vue'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'
import { useUserStore } from '@/stores/identity/useUserStore'

import {
  useAccountTitleI18n,
  ACCOUNTTITLE_QUERY_STRING_FIELDS,
  ACCOUNTTITLE_QUERY_FIELDS,
  ACCOUNTTITLE_SELF_I18N_KEY,
} from './composables/useAccountTitleI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useAccountTitleI18n()

/** i18n 翻译函数 */
const { t } = useI18n()
/** 用户上下文（companyDefaultCulture 等） */
const userStore = useUserStore()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktAccountTitle')
/** 右侧树表快捷查询占位文案 */
const tableSearchPlaceholder = computed(() =>
  t('common.page.form.placeholder.search', {
    keyword: [pi.label('accountTitleName'), pi.label('accountTitleCode')].join(' / '),
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
const selecmedRow = ref<AccountTitleRowRecord | null>(null)
/** 表格多选行 */
const selecmedRows = ref<AccountTitleRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selecmedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<AccountTitle> | null>(null)
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
  for (const key of ACCOUNTTITLE_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.balanceDirection !== undefined && form.balanceDirection !== null) {
    return true
  }
  if (form.accountTitleLevel !== undefined && form.accountTitleLevel !== null) {
    return true
  }
  if (form.isLeaf !== undefined && form.isLeaf !== null) {
    return true
  }
  if (form.isAuxiliary !== undefined && form.isAuxiliary !== null) {
    return true
  }
  if (form.isQuantity !== undefined && form.isQuantity !== null) {
    return true
  }
  if (form.isCurrency !== undefined && form.isCurrency !== null) {
    return true
  }
  if (form.isCash !== undefined && form.isCash !== null) {
    return true
  }
  if (form.isBank !== undefined && form.isBank !== null) {
    return true
  }
  if (form.accountTitleStatus !== undefined && form.accountTitleStatus !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(ACCOUNTTITLE_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ACCOUNTTITLE_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    balanceDirection: undefined as number | undefined,
    accountTitleLevel: undefined as number | undefined,
    isLeaf: undefined as number | undefined,
    isAuxiliary: undefined as number | undefined,
    isQuantity: undefined as number | undefined,
    isCurrency: undefined as number | undefined,
    isCash: undefined as number | undefined,
    isBank: undefined as number | undefined,
    accountTitleStatus: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  ACCOUNTTITLE_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
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
const entityIdName = 'accountTitleId'
/** 树节点标题字段名（左侧树 title） */
const treeTitleField = 'accountTitleName'

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matchesAccountTitleRightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (!String(record.accountTitleName ?? '').toLowerCase().includes(k) && !String(record.accountTitleCode ?? '').toLowerCase().includes(k)) return false
  }
  if (advancedQueryForm.value.cultureCode && !String(record.cultureCode ?? '').includes(String(advancedQueryForm.value.cultureCode))) return false
  if (advancedQueryForm.value.plantCode && !String(record.plantCode ?? '').includes(String(advancedQueryForm.value.plantCode))) return false
  if (advancedQueryForm.value.accountTitleCode && !String(record.accountTitleCode ?? '').includes(String(advancedQueryForm.value.accountTitleCode))) return false
  if (advancedQueryForm.value.accountTitleName && !String(record.accountTitleName ?? '').includes(String(advancedQueryForm.value.accountTitleName))) return false
  if (advancedQueryForm.value.parentId && !String(record.parentId ?? '').includes(String(advancedQueryForm.value.parentId))) return false
  if (advancedQueryForm.value.accountTitleType && !String(record.accountTitleType ?? '').includes(String(advancedQueryForm.value.accountTitleType))) return false
  if (advancedQueryForm.value.balanceDirection !== undefined && record.balanceDirection !== advancedQueryForm.value.balanceDirection) return false
  if (advancedQueryForm.value.accountTitleLevel !== undefined && record.accountTitleLevel !== advancedQueryForm.value.accountTitleLevel) return false
  if (advancedQueryForm.value.isLeaf !== undefined && record.isLeaf !== advancedQueryForm.value.isLeaf) return false
  if (advancedQueryForm.value.isAuxiliary !== undefined && record.isAuxiliary !== advancedQueryForm.value.isAuxiliary) return false
  if (advancedQueryForm.value.auxiliaryType && !String(record.auxiliaryType ?? '').includes(String(advancedQueryForm.value.auxiliaryType))) return false
  if (advancedQueryForm.value.isQuantity !== undefined && record.isQuantity !== advancedQueryForm.value.isQuantity) return false
  if (advancedQueryForm.value.isCurrency !== undefined && record.isCurrency !== advancedQueryForm.value.isCurrency) return false
  if (advancedQueryForm.value.isCash !== undefined && record.isCash !== advancedQueryForm.value.isCash) return false
  if (advancedQueryForm.value.isBank !== undefined && record.isBank !== advancedQueryForm.value.isBank) return false
  if (advancedQueryForm.value.validFromStart && !String(record.validFromStart ?? '').includes(String(advancedQueryForm.value.validFromStart))) return false
  if (advancedQueryForm.value.validFromEnd && !String(record.validFromEnd ?? '').includes(String(advancedQueryForm.value.validFromEnd))) return false
  if (advancedQueryForm.value.validToStart && !String(record.validToStart ?? '').includes(String(advancedQueryForm.value.validToStart))) return false
  if (advancedQueryForm.value.validToEnd && !String(record.validToEnd ?? '').includes(String(advancedQueryForm.value.validToEnd))) return false
  if (advancedQueryForm.value.accountTitleStatus !== undefined && record.accountTitleStatus !== advancedQueryForm.value.accountTitleStatus) return false
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
function mapAccountTitleLazyNodes(rows: AccountTitleTree[]): TaktLazyTreeNode[] {
  return mapLazyTreeNodes(rows, {
    getKey: (n) => String(n.accountTitleId ?? ''),
    getTitle: (n) => String(n.accountTitleName || n.accountTitleCode || n.accountTitleId || ''),
    isLeaf: (n) => taktIsLeafFlag((n as { isLeaf?: unknown }).isLeaf),
  })
}

/**
 * 将一层 DTO 映射为右侧树表节点（未加载子级用 _hasChildren 显示展开箭头）
 * @param rows 一层子节点
 */
function mapAccountTitleRightTreeNodes(rows: AccountTitleTree[]): Record<string, unknown>[] {
  return (rows ?? []).map((row) => {
    const rec = row as Record<string, unknown>
    const id = String(rec.accountTitleId ?? '')
    return {
      ...rec,
      key: id,
      _hasChildren: !taktIsLeafFlag(rec.isLeaf),
      children: undefined,
    }
  })
}

/**
 * 全量树映射为左侧 a-tree 节点（低于阈值、已带 children）
 * @param nodes 树节点
 */
function mapAccountTitleFullTreeToLeft(nodes: Record<string, unknown>[]): TaktLazyTreeNode[] {
  return (nodes ?? []).map((node) => {
    const rawChildren = Array.isArray(node.children) ? node.children as Record<string, unknown>[] : []
    const children = rawChildren.length > 0 ? mapAccountTitleFullTreeToLeft(rawChildren) : undefined
    return {
      ...node,
      key: String(node.accountTitleId ?? node.key ?? ''),
      title: String(node.accountTitleName || node.accountTitleCode || node.accountTitleId || ''),
      isLeaf: children == null || children.length === 0,
      children,
    } as TaktLazyTreeNode
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
 * 收集已加载且有子节点的 key（展开全部仅作用于已加载层）
 * @param nodes 树节点
 */
function collectTreeExpandableKeys(nodes: Array<Record<string, unknown>>): (string | number)[] {
  return collectTaktTreeTableExpandableKeys(nodes, (node) => taktTreeTableNodeKey(node, 'accountTitleId'))
}

/**
 * 当前右侧树的父级 ID（左侧选中；未选中则为根 0）
 * @returns {string} parentId
 */
function getRightListParentId(): string {
  const keys = selecmedTreeKeys.value
  if (keys.length > 0 && keys[keys.length - 1] != null) {
    return String(keys[keys.length - 1])
  }
  return '0'
}

/** 从树 API 响应中取出一层节点 */
function unwrapAccountTitleTree(res: unknown): AccountTitleTree[] {
  const resAny = res as { data?: AccountTitleTree[]; Data?: AccountTitleTree[] }
  if (Array.isArray(res)) return res as AccountTitleTree[]
  return resAny?.data ?? resAny?.Data ?? []
}

/** 从 list API 响应中取出行与总数 */
function unwrapAccountTitleList(res: unknown): { items: AccountTitle[]; total: number } {
  const r = res as { data?: AccountTitle[]; Data?: AccountTitle[]; items?: AccountTitle[]; total?: number; Total?: number }
  const items = Array.isArray((res as { data?: AccountTitle[] })?.data)
    ? (res as { data: AccountTitle[] }).data
    : Array.isArray(r?.Data)
      ? r.Data
      : Array.isArray(r?.items)
        ? r.items
        : []
  const total = Number(r?.total ?? r?.Total ?? items.length) || 0
  return { items, total }
}

/**
 * 加载右侧树（懒加载：当前父级直接子节点一层，可再展开）
 */
async function loadRightTree() {
  if (!useLazyTree.value) return
  listLoading.value = true
  try {
    const trees = unwrapAccountTitleTree(await getAccountTitleTree(getRightListParentId(), true))
    tableTreeData.value = mapAccountTitleRightTreeNodes(trees)
    tableExpandedRowKeys.value = []
    tableExpanded.value = false
  } catch (error: unknown) {
    logger.error('[AccountTitle] 加载右侧树失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    tableTreeData.value = []
  } finally {
    listLoading.value = false
  }
}

/**
 * 右侧树展开：再拉一层子节点
 * @param record 当前行
 */
async function handleRightTreeLoadChildren(record: Record<string, unknown>) {
  const id = getAccountTitleId(record)
  if (!id) return
  const trees = unwrapAccountTitleTree(await getAccountTitleTree(id, true))
  const children = mapAccountTitleRightTreeNodes(trees)
  tableTreeData.value = mergeLoadedChildren(
    tableTreeData.value as TaktLazyTreeNode[],
    id,
    children as TaktLazyTreeNode[],
    { keyField: 'accountTitleId' },
  )
}

/**
 * 低于阈值：list 全量后在前端组树，左右共用
 * @param total 记录总数
 */
async function loadFullTreeFromList(total: number) {
  const pageSize = Math.max(total, getTaktMaxPageSize())
  const { items } = unwrapAccountTitleList(await getAccountTitleList({
    pageIndex: 1,
    pageSize,
  }))
  const tree = buildTaktTreeFromFlat(items as Record<string, unknown>[], 'accountTitleId')
  tableTreeData.value = tree
  entityTreeData.value = mapAccountTitleFullTreeToLeft(tree)
}

/**
 * 加载左侧树根节点（parentId=0，一层）
 */
async function reloadLeftTreeRoots() {
  const trees = unwrapAccountTitleTree(await getAccountTitleTree('0', true))
  entityTreeData.value = mapAccountTitleLazyNodes(trees)
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
  const trees = unwrapAccountTitleTree(await getAccountTitleTree(parentKey, true))
  const children = mapAccountTitleLazyNodes(trees)
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    parentKey,
    children,
  ) as TaktLazyTreeNode[]
}

/**
 * 左侧树懒加载子节点
 * @param treeNode Ant Design Tree 节点
 */
async function handleLeftTreeLoadData(treeNode: Record<string, unknown>) {
  const dataRef = (treeNode.dataRef ?? treeNode) as Record<string, unknown>
  const key = dataRef.key ?? treeNode.key
  if (key == null) return
  if (Array.isArray(dataRef.children) && dataRef.children.length > 0) return
  const trees = unwrapAccountTitleTree(await getAccountTitleTree(String(key), true))
  const children = mapAccountTitleLazyNodes(trees)
  dataRef.children = children
  entityTreeData.value = mergeLoadedChildren(
    entityTreeData.value,
    String(key),
    children,
  ) as TaktLazyTreeNode[]
}

/** 右侧展示树：懒加载用已拉节点；全量树时按左侧选中取子树 */
const tableDisplayTree = computed(() => {
  if (useLazyTree.value) return tableTreeData.value
  const keys = selecmedTreeKeys.value
  if (keys.length > 0 && keys[keys.length - 1] != null) {
    const sub = findTaktTreeTableSubtree(tableTreeData.value, keys[keys.length - 1], 'accountTitleId')
    if (sub.length > 0) return sub
  }
  return tableTreeData.value
})

/** 右侧过滤后的树（保留 children，供组件按展开路径拍平） */
const tableFilteredTree = computed(() =>
  filterTaktTreeTableNodes(tableDisplayTree.value, matchesAccountTitleRightQuery)
)

/**
 * 同步右侧树表全部展开/收缩
 */
function applyAccountTitleTableExpandState() {
  tableExpandedRowKeys.value = tableExpanded.value
    ? collectTaktTreeTableExpandableKeys(tableFilteredTree.value, (node) =>
        taktTreeTableNodeKey(node, 'accountTitleId'),
      )
    : []
}

watch(tableExpanded, applyAccountTitleTableExpandState)
watch(tableFilteredTree, () => {
  if (tableExpanded.value) applyAccountTitleTableExpandState()
})

/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param accountTitle 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {AccountTitleUpdate} 更新载荷
 */
function buildAccountTitleUpdateDto(
  accountTitle: AccountTitle,
  overrides: Pick<AccountTitleUpdate, 'parentId'> & { sortOrder: number },
): AccountTitleUpdate {
  return {
    accountTitleId: String(accountTitle.accountTitleId),
    tenantCode: accountTitle.tenantCode,
    companyCode: accountTitle.companyCode,
    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
    plantCode: accountTitle.plantCode,
    accountTitleCode: accountTitle.accountTitleCode,
    accountTitleName: accountTitle.accountTitleName,
    parentId: overrides.parentId,
    accountTitleType: accountTitle.accountTitleType,
    balanceDirection: accountTitle.balanceDirection,
    accountTitleLevel: accountTitle.accountTitleLevel,
    isAuxiliary: accountTitle.isAuxiliary,
    auxiliaryType: accountTitle.auxiliaryType,
    isQuantity: accountTitle.isQuantity,
    isCurrency: accountTitle.isCurrency,
    isCash: accountTitle.isCash,
    isBank: accountTitle.isBank,
    validFrom: accountTitle.validFrom,
    validTo: accountTitle.validTo,
    accountTitleStatus: accountTitle.accountTitleStatus,
    extField: accountTitle.extField,
    remark: accountTitle.remark,
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
    const k = String(node?.key ?? node?.accountTitleId ?? '')
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
    const full = await getAccountTitleById(String(dragKey))
    await updateAccountTitle(String(dragKey), buildAccountTitleUpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
    await updateAccountTitleSort({ accountTitleId: String(dragKey), sortOrder: pos.sortOrder })
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await reloadLeftTreeRoots().catch(() => undefined)
  } finally {
    loading.value = false
  }
}

/** 左侧树关键字搜索（仅过滤已加载节点） */
const handleTreeQuerySearch = () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
}

/** 左侧展开/收缩：仅展开已加载节点 */
watch(treeExpanded, (expanded) => {
  if (expanded) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  } else {
    treeExpandedKeys.value = []
  }
})

/** 过滤后的左侧树变化且处于展开态时，同步 expandable keys */
watch(filteredTreeData, () => {
  if (treeExpanded.value) {
    treeExpandedKeys.value = collectTreeExpandableKeys(filteredTreeData.value)
  }
})

/** 左侧树选中：懒加载时换源拉右侧树；全量树时仅过滤子树 */
const handleTreeSelect = (selecmedKeys: (string | number)[]) => {
  selecmedTreeKeys.value = selecmedKeys
  if (useLazyTree.value) {
    void loadRightTree()
  }
}

/** 表格行记录（实体 DTO 或 ant-design-vue 模板 loose record） */
type AccountTitleRowRecord = AccountTitle | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const getAccountTitleId = (record: AccountTitleRowRecord): string => {
  if (record != null && 'accountTitleId' in record && (record as Record<string, unknown>).accountTitleId != null) {
    return String((record as Record<string, unknown>).accountTitleId)
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const getAccountTitleField = (record: AccountTitleRowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const getAccountTitleDictValue = (
  record: AccountTitleRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
/** 将行字段/字典值转为有限 number */
const toAccountTitleNumber = (value: string | number | undefined | null): number => {
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
    dataIndex: 'accountTitleId',
    key: 'accountTitleId',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      getAccountTitleField(record, 'accountTitleId') ?? getAccountTitleField(record, 'id') ?? '',
  },
  {
    title: pi.label('accountTitleCode'),
    dataIndex: 'accountTitleCode',
    key: 'accountTitleCode',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'accountTitleCode') ?? ''
  },
  {
    title: pi.label('accountTitleName'),
    dataIndex: 'accountTitleName',
    key: 'accountTitleName',
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
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'parentId') ?? ''
  },
  {
    title: pi.label('accountTitleType'),
    dataIndex: 'accountTitleType',
    key: 'accountTitleType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('balanceDirection'),
    dataIndex: 'balanceDirection',
    key: 'balanceDirection',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'balanceDirection') ?? ''
  },
  {
    title: pi.label('accountTitleLevel'),
    dataIndex: 'accountTitleLevel',
    key: 'accountTitleLevel',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'accountTitleLevel') ?? ''
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
    title: pi.label('isAuxiliary'),
    dataIndex: 'isAuxiliary',
    key: 'isAuxiliary',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('auxiliaryType'),
    dataIndex: 'auxiliaryType',
    key: 'auxiliaryType',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isQuantity'),
    dataIndex: 'isQuantity',
    key: 'isQuantity',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isCurrency'),
    dataIndex: 'isCurrency',
    key: 'isCurrency',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isCash'),
    dataIndex: 'isCash',
    key: 'isCash',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('isBank'),
    dataIndex: 'isBank',
    key: 'isBank',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  {
    title: pi.label('validFrom'),
    dataIndex: 'validFrom',
    key: 'validFrom',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'validFrom') ?? ''
  },
  {
    title: pi.label('validTo'),
    dataIndex: 'validTo',
    key: 'validTo',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: Record<string, unknown> }) => getAccountTitleField(record, 'validTo') ?? ''
  },
  {
    title: pi.label('accountTitleStatus'),
    dataIndex: 'accountTitleStatus',
    key: 'accountTitleStatus',
    width: 120,
    resizable: true,
    ellipsis: true,
  },
  CreateActionColumn<AccountTitle>({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'accounting:financial:account:title:update',
        onClick: (record: AccountTitleRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'accounting:financial:account:title:delete',
        onClick: (record: AccountTitleRowRecord) => handleDeleteOne(record)
      }
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: AccountTitle[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: AccountTitleRowRecord, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && getAccountTitleId(selecmedRow.value) === getAccountTitleId(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: AccountTitle[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? selecmedRowsData[0] : null
  },
}))

/** 加载左右树：一律 parentId 一层懒加载 */
async function loadData() {
  loading.value = true
  try {
    useLazyTree.value = true
    await reloadLeftTreeRoots()
    await loadRightTree()
  } catch (error: unknown) {
    logger.error('[AccountTitle] 加载树数据失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.load.data.failed')))
    useLazyTree.value = true
    entityTreeData.value = []
    tableTreeData.value = []
  } finally {
    loading.value = false
  }
}

/**
 * CRUD / 状态变更后刷新左右树
 */
async function refreshAfterMutation() {
  loading.value = true
  try {
    useLazyTree.value = true
    const parentKey = getRightListParentId()
    await reloadLeftTreeChildren(parentKey)
    await loadRightTree()
  } catch (error: unknown) {
    logger.error('[AccountTitle] 刷新失败', undefined, error)
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
async function handleAccountTitleStatusChange(record: AccountTitleRowRecord, checked: boolean) {
  const newVal = checked ? 1 : 0
  const oldVal = toAccountTitleNumber(getAccountTitleDictValue(record, 'accountTitleStatus'))
  const id = getAccountTitleId(record)
  try {
    await updateAccountTitleStatus({ accountTitleId: id, accountTitleStatus: newVal })
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
function handleEdit(record: AccountTitleRowRecord) {
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
      await updateAccountTitle(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createAccountTitle(payload as any)
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
async function handleDeleteOne(record: AccountTitleRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteAccountTitleById((record as any)[entityIdName])
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
      await deleteAccountTitleBatch(ids)
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
  const res = await getAccountTitleTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importAccountTitle(file, sheetName)
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
    const exportMeta = await exportAccountTitle({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
    logger.error('[AccountTitle] 导出失败', undefined, error)
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

/** 页面挂载：拉左右树（超阈值自动懒加载） */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  void loadData()
})
useTableRefresh(loadData)
</script>

<style scoped lang="css">
.accounting-financial-account-title {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.accounting-financial-account-title-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-financial-account-title-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.accounting-financial-account-title-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>

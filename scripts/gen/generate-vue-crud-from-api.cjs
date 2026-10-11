// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-crud-from-api.cjs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：标准单表 CRUD Vue（index.vue + *Form.vue），仅此一种模板；查询栏关键字=栏宽−查询/重置（TaktQueryBar flex）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { buildSingleIndexStateRefs, buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  VUE_TEMPLATE,
  runVueGeneratorCli,
  FORM_TAB_FIELDS_PER_TAB,
  buildMenuIndex,
  buildMasterDetailChildRegistry,
  loadVueModuleContext,
  isChildNavMasterDetailMaster,
  writeVueModuleOutputs,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderQueryFormItem,
  renderFormControl,
  renderFormItemOpening,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  buildQueryFieldMetaLine,
  computeFormTabCount,
  buildFormTabLabelAttr,
  buildFormContentClassExpr,
  buildAdvancedQueryFactoryBlock,
  buildListColumnsGeneratorBlock,
  resolveScopeFormFieldPresence,
  buildScopeContextFormScriptFragments,
  buildGeneratedFormTemplateBody,
  buildMasterDetailFormTypeImportLines,
  buildFormTabsScopedStyleBlock,
  hasScopeContextFormFields,
  pascalToCamel,
  fieldsUseDictSelect,
  buildDictDataStoreImportLine,
  buildDictDataStoreIndexSetup,
  buildGeneratedFormVueScriptFragments,
  buildResetPeriodListMapperScriptBlock,
  buildListDictTagValueExpr,
  resolveListSwitchAndDictColsForIndex,
  buildListBodyCellBlock,
  buildListSwitchHandlersBlock,
  INDEX_FORM_RESET_NEXT_TICK,
  buildServerPagedListQueryBlock,
  buildServerPagedLoadDataBody,
  buildServerPagedExportApiCall,
  buildServerPagedOnMountedBlock,
  buildServerPagedPaginationHandlersBlock,
  buildServerPagedIndexStyleBlock,
  buildFormResetScopeDefaultsBlock,
  buildVueImportResultUtilImportLine,
  TAKT_FORM_MODAL_WIDTH_IMPORT,
  TAKT_FORM_MODAL_WIDTH_ATTR,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
  entityRowRecordTypeName,
  buildEntityRowRecordTypeAlias,
  buildEntityDictValueHelper,
  buildEntityNumericCoerceHelper,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  toFrontendEntityFileCamel,
} = require('./generate-vue-common.cjs');

const EMPTY_MD_INDEX_PARTS = {
  state: '',
  columns: '',
  helpers: '',
  handlers: '',
  childApiImports: '',
  expandProps: '',
  expandTemplate: '',
};

/**
 * 生成 index.vue（单表 CRUD 或主子表，由 assemblyOptions 区分）
 * @param {object} ctx
 * @param {{ mdParts?: object, hasMasterDetail?: boolean, generatorScript?: string }} [assemblyOptions]
 */
function generateCrudIndexVue(ctx) {
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    permissionPrefix,
    caps,
    fields,
    comment,
  } = ctx;
  const generatorScript = 'generate-vue-crud-from-api.cjs';
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const rowRecordTypeAlias = buildEntityRowRecordTypeAlias(entityPascal);
  const mdParts = { state: '', columns: '', helpers: '', handlers: '', childApiImports: '', expandProps: '', expandTemplate: '' };
  const hasMasterDetail = false;
  const entityScope = fields.entityScope || 'company';
  const importApiNames = [
    caps.apiGetList,
    caps.apiGetById,
    caps.apiCreate,
    caps.apiUpdate,
    caps.apiDelete,
    caps.apiDeleteBatch,
    caps.apiGetTemplate,
    caps.apiImport,
    caps.apiExport,
    caps.apiUpdateStatus,
    caps.apiUpdateBuiltIn,
  ].filter(Boolean);
  const typeImports = [`${entityPascal}`, `${entityPascal}Query`]
    .filter((name, idx, arr) => arr.indexOf(name) === idx);
  const childTypeImportLines = hasMasterDetail
    ? (fields.masterDetailChildren || [])
      .map((child) => `import type { ${child.childType} } from '@/types/${modulePath}/${toFrontendEntityFileCamel(child.childKebab)}.types'`)
      .join('\n')
    : '';
  const listCols = fields.listFields.filter((f) => f.name !== caps.entityIdName);
  const { switchListCols, dictTagListCols } = resolveListSwitchAndDictColsForIndex(
    listCols.filter((f) => f.dictType || f.isListSwitch),
    caps,
  );
  const needsDictInIndex = fieldsUseDictSelect(fields.queryFields)
    || fieldsUseDictSelect(fields.formFields)
    || dictTagListCols.length > 0;
  const indexDictImport = needsDictInIndex ? buildDictDataStoreImportLine() : '';
  const indexDictSetup = needsDictInIndex ? buildDictDataStoreIndexSetup() : '';
  const indexDictOnMounted = needsDictInIndex ? '  void dictDataStore.loadAllDictDataAsync()\n' : '';
  const resetPeriodListMapperBlock = buildResetPeriodListMapperScriptBlock(dictTagListCols);
  const dictBodyCellBlock = buildListBodyCellBlock(dictTagListCols, switchListCols, entityPascal);
  const dictValueHelperBlock = [
    (dictTagListCols.length > 0 || switchListCols.length > 0)
      ? buildEntityDictValueHelper(entityPascal, rowRecordType)
      : '',
    switchListCols.length > 0 ? buildEntityNumericCoerceHelper(entityPascal) : '',
  ].filter(Boolean).join('\n');
  const listSwitchHandlersBlock = buildListSwitchHandlersBlock(switchListCols, entityPascal, caps);
  const queryItems = fields.queryFields.map((f) => renderQueryFormItem(f)).join('\n');
  const queryFactoryBlock = buildAdvancedQueryFactoryBlock(entityPascal, fields.queryFields);
  const queryInit = fields.queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}`;
  const listColumnsBlock = buildListColumnsGeneratorBlock(entityPascal, caps.entityIdName);
  const actionItems = [];
  if (caps.hasUpdate) {
    actionItems.push(`      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${permissionPrefix}:update',
        onClick: (record: ${rowRecordType}) => handleEdit(record)
      },`);
  }
  if (caps.hasDelete) {
    actionItems.push(`      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${permissionPrefix}:delete',
        onClick: (record: ${rowRecordType}) => handleDeleteOne(record)
      }`);
  }
  const formBlock = (caps.hasCreate || caps.hasUpdate) ? `
    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      ${TAKT_FORM_MODAL_WIDTH_ATTR}
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <${entityPascal}Form
        :key="formData?.${caps.entityIdName} ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>` : '';
  const importBlock = (caps.hasImport && caps.hasGetTemplate)
    ? buildImportModalVueBlock(entityPascal)
    : '';
  const formImports = (caps.hasCreate || caps.hasUpdate)
    ? `import ${entityPascal}Form from './components/${entityPascal}Form.vue'\n`
    : '';
  const iconImports = buildRemixIconImportLine({
    includeActionIcons: caps.hasUpdate || caps.hasDelete,
    queryFields: fields.queryFields,
  });
  const excelImport = (caps.hasImport || caps.hasExport)
    ? "import { taktExcelEntityNames } from '@/utils/naming'\n"
    : '';
  const exportImport = caps.hasExport
    ? "import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'\n"
    : '';
  const importResultImport = (caps.hasImport && caps.hasGetTemplate)
    ? buildVueImportResultUtilImportLine()
    : '';
  const excelConst = (caps.hasImport || caps.hasExport)
    ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${caps.entityClassName}')
`
    : '';
  const entityI18nIndexImport = buildEntityI18nIndexImportBlock(entityPascal, viewEntityKebab);
  const singleStateBlock = buildSingleIndexStateRefs(entityPascal, {
    hasForm: caps.hasCreate || caps.hasUpdate,
    hasImport: caps.hasImport && caps.hasGetTemplate,
    hasUpdate: caps.hasUpdate,
    hasDelete: caps.hasDelete || caps.hasDeleteBatch,
    queryInit,
    queryFactoryBlock,
    entityPascal,
    entityIdName: caps.entityIdName,
    excelConst,
  });
  const formStateBlock = '';
  const createHandler = caps.hasCreate ? `
/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true${INDEX_FORM_RESET_NEXT_TICK}
}` : '';
  const updateHandler = caps.hasUpdate ? (hasMasterDetail && caps.hasGetById ? `
/** 打开编辑弹窗（主子表：先拉详情含子表） */
async function handleEdit(record: ${rowRecordType}) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await load${entityPascal}Detail(record)
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}` : caps.hasGetById ? `
/** 打开编辑弹窗（拉取详情，避免列表列裁剪字段） */
async function handleEdit(record: ${rowRecordType}) {
  const id = get${entityPascal}Id(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await ${caps.apiGetById}(id)
    formData.value = detail ?? ({ ...record } as Partial<${entityPascal}>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}` : `
/** 打开编辑弹窗 */
function handleEdit(record: ${rowRecordType}) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formData.value = { ...record }
  formVisible.value = true
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}`) : '';
  const formSubmitHandler = (caps.hasCreate || caps.hasUpdate) ? `
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
${caps.hasUpdate ? `      await ${caps.apiUpdate}(id, payload as any)\n      message.success(t('common.feedback.updated', { target: pi.self() }))` : ''}
    } else {
${caps.hasCreate ? `      await ${caps.apiCreate}(payload as any)\n      message.success(t('common.feedback.created', { target: pi.self() }))` : ''}
    }
    formVisible.value = false
    formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
}` : '';
  const importHandlers = (caps.hasImport && caps.hasGetTemplate)
    ? buildImportHandlersScriptBlock({
      apiGetTemplate: caps.apiGetTemplate,
      apiImport: caps.apiImport,
    })
    : '';
  const exportHandler = caps.hasExport ? `
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
${buildServerPagedExportApiCall(caps.apiExport)}
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = \`\${excelNames.fileBase}_\${ts.getFullYear()}\${pad(ts.getMonth() + 1)}\${pad(ts.getDate())}\${pad(ts.getHours())}\${pad(ts.getMinutes())}\${pad(ts.getSeconds())}\`
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
  } catch (error: any) {
    logger.error('[${entityPascal}] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}` : '';
  const deleteOneHandler = caps.hasDelete ? `
/** 删除单行 */
async function handleDeleteOne(record: ${rowRecordType}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${caps.apiDelete}((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}` : '';
  const deleteBatchHandler = caps.hasDeleteBatch ? `
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await ${caps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}` : '';
  const loadDataBody = caps.hasGetList
    ? buildServerPagedLoadDataBody(caps.apiGetList)
    : `    dataSource.value = []
    total.value = 0`;
  const serverPagedScriptBlock = caps.hasGetList
    ? buildServerPagedListQueryBlock(entityPascal, fields.queryFields)
    : '';
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：${comment}管理页面，含查询、增删改，由 ${generatorScript} 根据 types/api 自动生成 -->
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
${caps.hasCreate ? `      create-permission="${permissionPrefix}:create"` : ''}
${caps.hasUpdate ? `      update-permission="${permissionPrefix}:update"` : ''}
${caps.hasDelete ? `      delete-permission="${permissionPrefix}:delete"` : ''}
${caps.hasImport ? `      import-permission="${permissionPrefix}:import"` : ''}
${caps.hasExport ? `      export-permission="${permissionPrefix}:export"` : ''}
      :show-create="${caps.hasCreate}"
      :show-update="${caps.hasUpdate}"
      :show-delete="${caps.hasDelete || caps.hasDeleteBatch}"
      :show-import="${caps.hasImport && caps.hasGetTemplate}"
      :show-export="${caps.hasExport}"
      :show-expand="${hasMasterDetail}"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
${caps.hasCreate ? '      :create-disabled="false"\n      :create-loading="loading"' : ''}
${caps.hasUpdate ? '      :update-disabled="updateDisabled"\n      :update-loading="loading"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      :delete-disabled="deleteDisabled"\n      :delete-loading="loading"' : ''}
      :refresh-loading="loading"
${caps.hasCreate ? '      @create="handleCreate"' : ''}
${caps.hasUpdate ? '      @update="handleUpdate"' : ''}
${caps.hasDelete || caps.hasDeleteBatch ? '      @delete="handleDelete"' : ''}
${caps.hasImport && caps.hasGetTemplate ? '      @import="handleImport"' : ''}
${caps.hasExport ? '      @export="handleExport"' : ''}
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="${entityScope}"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'${caps.entityIdName}'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="get${entityPascal}Id"
      :row-selection="rowSelection"
      :custom-row="onClickRow"
${mdParts.expandProps}
      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
${dictBodyCellBlock}${mdParts.expandTemplate}
    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />
${formBlock}
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'${queryFieldStorageKey}'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
${queryItems}
      </template>
    </TaktQueryDrawer>
${importBlock}
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'${caps.entityIdName}'"
      :action-column-key="'action'"
      entity-scope="${entityScope}"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}管理页 · 由 ${generatorScript} 根据 types/api 生成
 * @module views/${viewModulePath}
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
${(caps.hasCreate || caps.hasUpdate) ? TAKT_FORM_MODAL_WIDTH_IMPORT : ''}${formImports}import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.api'
${mdParts.childApiImports ? `${mdParts.childApiImports}\n` : ''}${childTypeImportLines ? `${childTypeImportLines}\n` : ''}import type { ${typeImports.join(', ')} } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'
${indexDictImport}${excelImport}${exportImport}${importResultImport}${iconImports}
${entityI18nIndexImport}${rowRecordTypeAlias}${singleStateBlock}
${indexDictSetup}${mdParts.state}
${serverPagedScriptBlock}${buildServerPagedOnMountedBlock(indexDictOnMounted)}

${listColumnsBlock}
    actions: [
${actionItems.join('\n')}
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const get${entityPascal}Id = (record: ${rowRecordType}): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
${dictValueHelperBlock}
${resetPeriodListMapperBlock}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${rowRecordType}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ${rowRecordType}, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && get${entityPascal}Id(selectedRow.value) === get${entityPascal}Id(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${rowRecordType}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: ${rowRecordType}) => ({
  onClick: () => {
    const key = get${entityPascal}Id(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(get${entityPascal}Id(item)))
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
${loadDataBody}
  } catch (error: any) {
    logger.error('[${entityPascal}] 加载数据失败', { error })
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
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}
${createHandler}${updateHandler}${formSubmitHandler}${importHandlers}${exportHandler}${deleteOneHandler}${deleteBatchHandler}${listSwitchHandlersBlock}
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

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
${caps.hasGetList ? buildServerPagedPaginationHandlersBlock() : ''}
</script>
`;
}

const EMPTY_MD_FORM_PARTS = { tabs: '', script: '', needsTaktSelect: false };

/**
 * 生成 *Form.vue（单表 CRUD 或主子表，由 assemblyOptions 区分）
 * @param {object} ctx
 * @param {{ mdFormParts?: object, hasMasterDetail?: boolean, generatorScript?: string }} [assemblyOptions]
 */
function generateCrudFormVue(ctx) {
  const { entityPascal, entityCamel, entityKebab, viewEntityKebab, modulePath, viewModulePath, fields, comment, caps } = ctx;
  const generatorScript = 'generate-vue-crud-from-api.cjs';
  const mdFormParts = { tabs: '', script: '', needsTaktSelect: false };
  const hasMasterDetail = false;
  const formFields = fields.formFields;
  const entityIdField = caps?.entityIdName ?? `${entityCamel}Id`;
  const formCodeControlOptions = { entityIdField };
  const formTemplate = buildGeneratedFormTemplateBody({
    formFields,
    formCodeControlOptions,
    hasMasterDetail,
    extraTabPanes: mdFormParts.tabs,
    entityKebab: viewEntityKebab,
  });
  const useFormTabs = formTemplate.useFormTabs;
  const formTabCount = computeFormTabCount(formFields);
  const formContentClassExpr = buildFormContentClassExpr(useFormTabs, formTabCount);
  const omitFormFieldsArray = useFormTabs && formTabCount > 1;
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasScopeContextFields = hasScopeContextFormFields(formFields, masterDetailChildren);
  const scopePresence = resolveScopeFormFieldPresence(formFields, masterDetailChildren);
  const scopeFragments = buildScopeContextFormScriptFragments(scopePresence, entityIdField, entityPascal);
  const scopeStoreImports = hasScopeContextFields ? scopeFragments.imports : '';
  const scopeStoreScript = hasScopeContextFields ? scopeFragments.script : '';
  const scopeContextWatch = hasScopeContextFields ? scopeFragments.watch : '';
  const { masterTypeImport } = buildMasterDetailFormTypeImportLines({
    entityPascal,
    entityKebab,
    modulePath,
  });
  const childFieldStrip = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `    delete (next as any).${c.fieldName}`).join('\n')
    : '';
  const watchSyncChild = hasMasterDetail ? '    syncChildRowsFromFormData(val)\n' : '';
  const resetChildRows = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `  child${c.childPascal}Rows.value = []`).join('\n')
    : '';
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields,
    entityIdField,
    childFieldStrip,
    hasScopeContextFields,
    watchSyncChild,
    useBuildSubmitPayload: hasMasterDetail,
  });
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(entityIdField, hasScopeContextFields);
  const getValuesBody = formScriptFragments.getValuesBody;
  // TaktSelect / Takt* 业务 SFC 由 unplugin-vue-components 自动注册（components.d.ts），禁止再写 import
  const extFieldIconImport = buildExtFieldIconImportLine(formFields);
  const formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(formFields.map((f) => f.name)),
    mdScript: mdFormParts.script,
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal,
    entityIdField,
    useFormTabs,
    omitFormFieldsArray,
  });
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${entityPascal}Form.vue -->
<!-- 功能描述：${comment}维护弹窗内嵌表单。由 ${generatorScript} 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
${formTemplate.body}
  </a-form>
</template>

<script setup lang="ts">
/**
 * ${comment}维护表单 · 由 ${generatorScript} 根据 types/api 生成
 * @module views/${viewModulePath}/components
 */
${formScriptFragments.vueImportLine}
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
${buildEntityI18nFormImportBlock(entityPascal, viewEntityKebab)}${masterTypeImport}
${extFieldIconImport}${formScriptFragments.dictImportLine}${formScriptFragments.fileUploadImportLine}${formScriptFragments.numberingImportLine || ''}${scopeStoreImports}
${formScriptState}
${formScriptFragments.defaultsBlock}
${formScriptFragments.normalizerBlock}
${formScriptFragments.dictBootstrap}
${formScriptFragments.fileUploadBootstrap}
${formScriptFragments.numberingBootstrap || ''}
${formScriptFragments.watchBlock}
${scopeContextWatch}
/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
${formScriptFragments.requiredRules}
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
${getValuesBody}
}

/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
${resetScopeDefaultsLine}${resetChildRows}
${useFormTabs ? '  activeTab.value = \'tab-0\'' : ''}
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
${buildFormTabsScopedStyleBlock(useFormTabs)}
`;
}
/**
 * 是否标准单表 CRUD（非树、非主子表主实体）
 * 特例：Employee / News 虽有 OneToMany 子表，主菜单页仍走单表 CRUD（子表菜单另生主子视图）
 * @param {object} bundle loadVueModuleContext 返回值
 */
function isCrudEntity(bundle) {
  if (bundle.isTreeEntity) {
    return false;
  }
  if (isChildNavMasterDetailMaster(bundle.entityShort)) {
    return Boolean(bundle.capsMerged.hasGetList || bundle.capsMerged.hasCreate || bundle.capsMerged.hasUpdate);
  }
  if (bundle.isMasterDetailEntity) {
    return false;
  }
  return Boolean(bundle.capsMerged.hasGetList || bundle.capsMerged.hasCreate || bundle.capsMerged.hasUpdate);
}

/**
 * 处理单表 CRUD API 模块
 */
function processCrudApiModule(apiFilePath, options, registry) {
  const bundle = loadVueModuleContext(apiFilePath, options, registry);
  if (bundle.skipped) {
    return bundle;
  }
  if (!isCrudEntity(bundle)) {
    if (bundle.isTreeEntity) {
      console.log(`⏭️  跳过（树表，请用 generate-vue-tree-from-api.cjs）: ${bundle.rel}`);
    } else if (bundle.isMasterDetailEntity) {
      console.log(`⏭️  跳过（主子表，请用 generate-vue-master-detail-from-api.cjs）: ${bundle.rel}`);
    } else {
      console.warn(`⚠️  非标准 CRUD API，跳过: ${bundle.rel}`);
    }
    return { skipped: true };
  }
  console.log(`  标准 CRUD: ${bundle.fullCtx.caps.apiGetList || bundle.fullCtx.caps.apiCreate}`);
  console.log(`  entityScope: ${bundle.fullCtx.fields.entityScope} ← Takt${bundle.entityShort}`);
  // Employee / News：表单不内嵌子表（子表由各菜单主子页维护）
  const crudCtx = isChildNavMasterDetailMaster(bundle.entityShort)
    ? {
        ...bundle.fullCtx,
        fields: { ...bundle.fullCtx.fields, masterDetailChildren: [] },
      }
    : bundle.fullCtx;
  if (isChildNavMasterDetailMaster(bundle.entityShort)) {
    console.log(`  ${bundle.entityShort}：主菜单单表 CRUD（不含主子内嵌；子表见各子菜单主子视图）`);
  }
  const indexContent = generateCrudIndexVue(crudCtx);
  const formContent = bundle.needsForm ? generateCrudFormVue(crudCtx) : '';
  const listCols = crudCtx.fields.listFields.filter((f) => f.name !== crudCtx.caps.entityIdName);
  const i18nComposableContent = buildEntityI18nComposableFile({
    entityPascal: crudCtx.entityPascal,
    entityI18nSlug: crudCtx.entityI18nSlug,
    entityKebab: crudCtx.entityKebab,
    viewModulePath: crudCtx.viewModulePath,
    viewEntityKebab: crudCtx.viewEntityKebab,
    modulePath: crudCtx.modulePath,
    listFields: listCols,
    formFields: crudCtx.fields.formFields,
    queryFields: crudCtx.fields.queryFields,
    comment: crudCtx.comment,
  });
  return writeVueModuleOutputs(bundle, indexContent, formContent, options, i18nComposableContent);
}

function printCrudUsage() {
  console.log(`
用法: node scripts/generate-vue-crud-from-api.cjs [参数]

模板: **标准单表 CRUD**（分页列表 + 弹窗表单）

参数:
  --<实体名>            如 --Plant、--Holiday
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体

示例:
  node scripts/generate-vue-crud-from-api.cjs --Plant
`);
}

if (require.main === module) {
  runVueGeneratorCli({
    banner: '🚀 标准单表 CRUD Vue（generate-vue-crud-from-api.cjs）...\n',
    printUsage: printCrudUsage,
    templateType: VUE_TEMPLATE.CRUD,
    buildRegistry: buildMasterDetailChildRegistry,
    onInit: buildMenuIndex,
    processModule: processCrudApiModule,
  });
}

module.exports = {
  processCrudApiModule,
  generateCrudIndexVue,
  generateCrudFormVue,
};

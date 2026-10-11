// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-master-detail-from-api.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：主子表 Vue（列表左主右从 + 弹窗上主下从级联保存）；查询栏关键字=左/右表栏宽−查询/重置（TaktQueryBar flex）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const path = require('path');
const { buildSingleIndexStateRefs, buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  VUE_TEMPLATE,
  CONFIG,
  runVueGeneratorCli,
  FORM_TAB_FIELDS_PER_TAB,
  buildMenuIndex,
  buildMasterDetailChildRegistry,
  loadVueModuleContext,
  writeVueModuleOutputs,
  resolveMasterDetailViewPlans,
  filterStandaloneMenuChildren,
  validateMasterDetailChildrenAlignment,
  cloneFieldMetaWithMasterDetailChildren,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderQueryFormItem,
  renderFormControl,
  renderFormItemOpening,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  computeFormTabCount,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  buildGeneratedFormTemplateBody,
  buildMasterDetailFormTypeImportLines,
  buildFormTabsScopedStyleBlock,
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
  buildFormResetScopeDefaultsBlock,
  buildVueImportResultUtilImportLine,
  TAKT_FORM_MODAL_WIDTH_IMPORT,
  TAKT_FORM_MODAL_WIDTH_ATTR,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  buildAdvancedQueryFactoryBlock,
  buildEntityDictValueHelper,
  buildEntityNumericCoerceHelper,
  entityRowRecordTypeName,
  entityI18nComposableFileName,
  hasScopeContextFormFields,
  resolveScopeFormFieldPresence,
  buildScopeContextFormScriptFragments,
  toFrontendEntityFileCamel,
} = require('./generate-vue-common.cjs');
const {
  generateMasterDetailLrIndexScript,
  generateMasterDetailEditableFormParts,
  writeMasterDetailLayoutOutputs,
  buildMasterDetailFormHalfPaneStyleBlock,
  buildMasterDetailFormHalfPaneScrollScript,
} = require('./generate-vue-master-detail-layout.cjs');
const {
  listAssociationsForChild,
  resolveApiFilePathForEntity,
  validateEntityMasterDetailAssociations,
} = require('./generate-master-detail-associations.cjs');

/**
 * 按 viewModulePath 克隆 bundle 输出路径（主表 form 文件名仍用 viewEntityKebab）
 * @param {object} baseBundle
 * @param {string} viewModulePath
 * @param {object} fields
 * @returns {object}
 */
function buildViewBundle(baseBundle, viewModulePath, fields) {
  const viewDir = path.join(CONFIG.frontendRoot, CONFIG.viewsDir, viewModulePath);
  const masterFormKebab = baseBundle.fullCtx.viewEntityKebab;
  const i18nComposablePath = path.join(
    viewDir,
    'composables',
    entityI18nComposableFileName(masterFormKebab),
  );
  return {
    ...baseBundle,
    fullCtx: {
      ...baseBundle.fullCtx,
      viewModulePath,
      cssRootClass: viewModulePath.replace(/\//g, '-'),
      fields,
    },
    indexPath: path.join(viewDir, 'index.vue'),
    formPath: path.join(viewDir, 'components', `${baseBundle.fullCtx.entityPascal}Form.vue`),
    i18nComposablePath,
  };
}

/**
 * 写入单个主子表视图（LR + panel + 主表 form 含单个子表 TaktEditableTable）
 * @param {object} bundle
 * @param {object} ctx
 * @param {object} options
 */
function writeSingleMasterDetailView(bundle, ctx, options) {
  writeMasterDetailLayoutOutputs(bundle, ctx, options);
  const indexContent = generateMasterDetailIndexVue(ctx);
  const formContent = bundle.needsForm ? generateMasterDetailFormVue(ctx) : '';
  const listCols = ctx.fields.listFields.filter((f) => f.name !== ctx.caps.entityIdName);
  const i18nComposableContent = buildEntityI18nComposableFile({
    entityPascal: ctx.entityPascal,
    entityI18nSlug: ctx.entityI18nSlug,
    entityKebab: ctx.entityKebab,
    viewModulePath: ctx.viewModulePath,
    viewEntityKebab: ctx.viewEntityKebab,
    modulePath: ctx.modulePath,
    listFields: listCols,
    formFields: ctx.fields.formFields,
    queryFields: ctx.fields.queryFields,
    comment: ctx.comment,
  });
  return writeVueModuleOutputs(bundle, indexContent, formContent, options, i18nComposableContent);
}

/**
 * 从实体 ManyToOne 反向生成主子视图（视图与主表平级，落在 module 目录下）
 * @param {string} childEntityShort
 * @param {object} options
 * @param {Map<string, object>} registry
 * @returns {{ skipped: boolean, created?: boolean }}
 */
function processManyToOneAssociationViews(childEntityShort, options, registry) {
  const assocs = listAssociationsForChild(childEntityShort);
  if (!assocs.length) {
    return { skipped: true };
  }
  let created = false;
  assocs.forEach((assoc) => {
    const masterApiPath = resolveApiFilePathForEntity(assoc.masterPascal);
    if (!masterApiPath) {
      console.warn(`⚠️  主表 API 未生成，跳过关联视图 Takt${assoc.masterPascal}/Takt${assoc.childPascal}`);
      return;
    }
    const masterBundle = loadVueModuleContext(
      masterApiPath,
      { ...options, entityPrefix: assoc.masterPascal, bypassChildRegistrySkip: true },
      registry,
    );
    if (masterBundle.skipped || !masterBundle.isMasterDetailEntity) {
      console.warn(`⚠️  主表 Takt${assoc.masterPascal} 非主子表，跳过关联视图`);
      return;
    }
    const masterChildren = masterBundle.fullCtx.fields.masterDetailChildren || [];
    const childMeta = masterChildren.find((c) => c.childPascal === assoc.childPascal);
    if (!childMeta) {
      console.warn(`⚠️  主表未解析子表 Takt${assoc.childPascal}，跳过关联视图`);
      return;
    }
    const plans = resolveMasterDetailViewPlans(
      masterBundle.fullCtx.viewModulePath,
      masterBundle.fullCtx.modulePath,
      masterChildren,
      assoc.masterPascal,
    );
    const plan = plans.find((item) => item.childMeta.childPascal === childMeta.childPascal);
    if (!plan) {
      console.log(`  ⏭️  跳过关联视图（无对应菜单导航）: Takt${assoc.masterPascal} / Takt${assoc.childPascal}`);
      return;
    }
    const childViewPath = plan.viewModulePath;
    const childFields = cloneFieldMetaWithMasterDetailChildren(masterBundle.fullCtx.fields, [childMeta]);
    const childCtx = {
      ...masterBundle.fullCtx,
      viewModulePath: childViewPath,
      cssRootClass: childViewPath.replace(/\//g, '-'),
      fields: childFields,
    };
    const childBundle = buildViewBundle(masterBundle, childViewPath, childFields);
    console.log(
      `  关联主子视图（OneToMany ↔ ManyToOne）: Takt${assoc.masterPascal} / Takt${assoc.childPascal} → ${childViewPath}`,
    );
    writeSingleMasterDetailView(childBundle, childCtx, options);
    created = true;
  });
  return created ? { skipped: false, created: true } : { skipped: true };
}

/**
 * 从实体同时是主表（≥2 个 OneToMany）：不生成自身 CRUD，仅在 module 目录下生成各子导航平级主子视图
 * @param {string} entityShort
 * @param {object} options
 * @param {Map<string, object>} registry
 * @returns {{ skipped: boolean, created?: boolean }}
 */
function processChildMasterMultiNavViews(entityShort, options, registry) {
  const masterApiPath = resolveApiFilePathForEntity(entityShort);
  if (!masterApiPath) {
    return { skipped: true };
  }
  const masterBundle = loadVueModuleContext(
    masterApiPath,
    { ...options, entityPrefix: entityShort, bypassChildRegistrySkip: true },
    registry,
  );
  if (masterBundle.skipped || !masterBundle.isMasterDetailEntity) {
    return { skipped: true };
  }
  const children = masterBundle.fullCtx.fields.masterDetailChildren || [];
  if (children.length <= 1) {
    return { skipped: true };
  }
  const plans = resolveMasterDetailViewPlans(
    masterBundle.fullCtx.viewModulePath,
    masterBundle.fullCtx.modulePath,
    children,
    entityShort,
  );
  if (!plans.length) {
    return { skipped: true };
  }
  console.log(
    `  布局: 从实体 Takt${entityShort} → ${plans.length} 个菜单导航主子视图（不写自身单表页）`,
  );
  plans.forEach((plan) => {
    const childFields = cloneFieldMetaWithMasterDetailChildren(
      masterBundle.fullCtx.fields,
      [plan.childMeta],
    );
    const childCtx = {
      ...masterBundle.fullCtx,
      viewModulePath: plan.viewModulePath,
      cssRootClass: plan.viewModulePath.replace(/\//g, '-'),
      fields: childFields,
    };
    const childBundle = buildViewBundle(masterBundle, plan.viewModulePath, childFields);
    console.log(`  ▶ 子导航 Takt${plan.childMeta.childPascal}: ${childBundle.indexPath}`);
    writeSingleMasterDetailView(childBundle, childCtx, options);
  });
  return { skipped: false, created: true };
}

/**
 * 生成 index.vue（主子表：TaktMasterDetailTableLr + 右侧明细面板）
 * @param {object} ctx
 */
function generateMasterDetailIndexVue(ctx) {
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
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  const mdParts = generateMasterDetailLrIndexScript(ctx);
  const hasMasterDetail = true;
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
  const rowRecordType = entityRowRecordTypeName(entityPascal);
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
  const columnBlocks = listCols.map((f) => {
    if (f.dictType) {
      return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
  },`;
    }
    return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${f.name}') ?? ''
  },`;
  }).join('\n');
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
    formData.value = null${INDEX_FORM_RESET_NEXT_TICK}${mdParts.formSubmitReload}
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
      successExtraBody: mdParts.formSubmitReload || '',
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
${mdParts.deleteClearSelection}
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
${mdParts.deleteClearSelection}
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
  <div class="p-4 flex flex-col min-h-0 h-full">
    <!-- 左主右从 -->
    <TaktMasterDetailTableLr
      v-model:master-current="currentPage"
      v-model:master-page-size="pageSize"
      v-model:selected-master-key="selectedMasterKey"
      class="min-h-0 flex-1"
      :master-columns="columns"
      :master-data-source="dataSource"
      :master-loading="loading"
      :master-row-key="get${entityPascal}Id"
      :master-row-selection="rowSelection"
      master-id-column-key="${caps.entityIdName}"
      :master-visible-column-keys="visibleColumnKeys"
      master-table-mode="masterDetailMaster"
      master-scroll-layout="masterDetailLr"
      :master-total="total"
      master-entity-scope="${entityScope}"
      @master-change="handleTableChange"
      @master-resize-column="handleResizeColumn"
      @master-pagination-change="handleMasterPaginationChange"
      @master-select="handleMasterSelect"
    >
      <template #master-toolbar>
        <TaktQueryBar
          v-model="queryKeyword"
          :placeholder="searchPlaceholder"
          :loading="loading"
          @search="handleSearch"
          @reset="handleReset"
        />
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
      :show-expand="false"
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
      </template>
${dictBodyCellBlock}${mdParts.detailSlot}
    </TaktMasterDetailTableLr>
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
      table-mode="masterDetailMaster"
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
${(caps.hasCreate || caps.hasUpdate) ? TAKT_FORM_MODAL_WIDTH_IMPORT : ''}${formImports}${mdParts.panelImports}
${mdParts.composableImport}
import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.api'
import type { ${typeImports.join(', ')} } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'
${indexDictImport}${excelImport}${exportImport}${importResultImport}${iconImports}
${entityI18nIndexImport}
${singleStateBlock}
${indexDictSetup}${mdParts.composableSetup}
${mdParts.panelRefs}
${serverPagedScriptBlock}${buildServerPagedOnMountedBlock(indexDictOnMounted)}
${mdParts.lrScript}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${caps.entityIdName}',
    key: '${caps.entityIdName}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: any }) => get${entityPascal}Field(record, '${caps.entityIdName}') ?? ''
  },
${columnBlocks}
  CreateActionColumn({
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
/**
 * 读取行字段值
 * @param record 行数据
 * @param field 字段名
 */
const get${entityPascal}Field = (record: any, field: string): any => record?.[field]
${dictValueHelperBlock}
${resetPeriodListMapperBlock}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${rowRecordType}[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
${mdParts.rowSelectionPatch}
  },
  onSelect: (record: ${rowRecordType}, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
      syncMasterSelection(record)
    } else if (selectedRow.value && get${entityPascal}Id(selectedRow.value) === get${entityPascal}Id(record)) {
      selectedRow.value = null
      syncMasterSelection(null)
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: ${rowRecordType}[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
    syncMasterSelection(selectedRow.value)
  }
}))

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
  advancedQueryForm.value = {
${queryInit}
  }
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
  advancedQueryForm.value = {
${queryInit}
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
</script>
`;
}

const EMPTY_MD_FORM_PARTS = { editableBlocks: '', script: '', tableRefs: '', validateLines: '', resetLines: '', needsTaktSelect: false };

/**
 * 生成 *Form.vue（单表 CRUD 或主子表，由 assemblyOptions 区分）
 * @param {object} ctx
 * @param {{ mdFormParts?: object, hasMasterDetail?: boolean, generatorScript?: string }} [assemblyOptions]
 */
function generateMasterDetailFormVue(ctx) {
  const { entityPascal, entityCamel, entityKebab, viewEntityKebab, modulePath, viewModulePath, fields, comment, caps } = ctx;
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  const mdFormParts = generateMasterDetailEditableFormParts(ctx);
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasMasterDetailChildren = masterDetailChildren.length > 0;
  const formFields = fields.formFields;
  const entityIdField = caps?.entityIdName ?? `${entityCamel}Id`;
  const formCodeControlOptions = { entityIdField };
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const formTemplate = buildGeneratedFormTemplateBody({
    formFields,
    formCodeControlOptions,
    hasMasterDetail: false,
    entityKebab: viewEntityKebab,
  });
  const useFormTabs = formTemplate.useFormTabs;
  const mainFormBody = useFormTabs
    ? formTemplate.body
    : `    <div :class="formContentClass">
${formTemplate.body}
    </div>`;
  const formTemplateBody = hasMasterDetailChildren
    ? `    <!-- 上：主表（弹窗视口约 1/2） -->
    <div class="${viewEntityKebab}-form__master min-h-0 flex-1 overflow-hidden">
${mainFormBody}
    </div>
    <!-- 下：子表（弹窗视口约 1/2） -->
    <div
      ref="detailHostRef"
      class="${viewEntityKebab}-form__detail flex min-h-0 flex-1 flex-col overflow-hidden"
    >
${mdFormParts.editableBlocks}
    </div>`
    : `${mainFormBody}
${mdFormParts.editableBlocks}`;
  const hasScopeContextFields = hasScopeContextFormFields(formFields, masterDetailChildren)
    || hasMasterDetailChildren;
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
  const childFieldStrip = hasMasterDetailChildren
    ? masterDetailChildren.map((c) => `    delete (next as any).${c.fieldName}`).join('\n')
    : '';
  const watchSyncChild = hasMasterDetailChildren ? '    syncChildRowsFromFormData(val)\n' : '';
  const resetChildRows = hasMasterDetailChildren
    ? masterDetailChildren.map((c) => `  child${c.childPascal}Rows.value = []`).join('\n')
    : '';
  const resetEditableTables = mdFormParts.resetLines || '';
  // TaktSelect 等业务 SFC：unplugin-vue-components 自动注册，禁止手写 import
  const extFieldIconImport = buildExtFieldIconImportLine(formFields);
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields,
    entityIdField,
    childFieldStrip,
    hasScopeContextFields,
    watchSyncChild,
    useBuildSubmitPayload: hasMasterDetailChildren,
  });
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(entityIdField, hasScopeContextFields);
  const getValuesBody = formScriptFragments.getValuesBody;
  const validateChildLines = mdFormParts.validateLines
    ? `\n${mdFormParts.validateLines}`
    : '';
  const formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(formFields.map((f) => f.name)),
    mdScript: mdFormParts.script,
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal,
    entityIdField,
    useFormTabs,
  });
  const halfPaneScrollScript = hasMasterDetailChildren ? buildMasterDetailFormHalfPaneScrollScript() : '';
  const halfPaneStyleBlock = hasMasterDetailChildren
    ? buildMasterDetailFormHalfPaneStyleBlock(viewEntityKebab)
    : buildFormTabsScopedStyleBlock(useFormTabs);
  // vue API 由 auto-import 注入；禁止再拼 import { … } from 'vue'
  const vueImportNormalized = '';
  const activeTabReset = useFormTabs ? "  activeTab.value = 'tab-0'\n" : '';
  const halfPaneFormDataWatchHook = hasMasterDetailChildren
    ? `
watch(
  () => props.formData,
  () => {
    void nextTick(() => {
      recalcDetailScrollYPx()
      bindDetailHostResizeObserver()
    })
  },
)
`
    : '';
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${entityPascal}Form.vue -->
<!-- 功能描述：${comment}维护弹窗内嵌表单（上主下从各占约 1/2 级联保存）。由 ${generatorScript} 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ${viewEntityKebab}-form flex h-full min-h-0 flex-col overflow-hidden"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
${formTemplateBody}
  </a-form>
</template>

<script setup lang="ts">
/**
 * ${comment}维护表单 · 由 ${generatorScript} 根据 types/api 生成
 * @module views/${viewModulePath}/components
 */
${vueImportNormalized}
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
${buildEntityI18nFormImportBlock(entityPascal, viewEntityKebab)}
${masterTypeImport}
${extFieldIconImport}${formScriptFragments.dictImportLine}${formScriptFragments.fileUploadImportLine}${formScriptFragments.numberingImportLine || ''}${scopeStoreImports}
${formScriptState}
${halfPaneScrollScript}
${formScriptFragments.defaultsBlock}
${formScriptFragments.normalizerBlock}
${formScriptFragments.dictBootstrap}
${formScriptFragments.fileUploadBootstrap}
${formScriptFragments.numberingBootstrap || ''}
${formScriptFragments.watchBlock}
${halfPaneFormDataWatchHook}
${scopeContextWatch}
/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
${formScriptFragments.requiredRules}
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()${validateChildLines}
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
${resetEditableTables}
${activeTabReset}  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
${halfPaneStyleBlock}
`;
}
/**
 * 处理主子表 API 模块
 */
function processMasterDetailApiModule(apiFilePath, options, registry) {
  if (options.entityPrefix) {
    validateEntityMasterDetailAssociations(options.entityPrefix);
  }
  const bundle = loadVueModuleContext(apiFilePath, options, registry);
  if (bundle.skipped) {
    if (options.entityPrefix) {
      const multiNavResult = processChildMasterMultiNavViews(options.entityPrefix, options, registry);
      if (!multiNavResult.skipped) {
        return multiNavResult;
      }
      const assocResult = processManyToOneAssociationViews(options.entityPrefix, options, registry);
      if (!assocResult.skipped) {
        return assocResult;
      }
    }
    return bundle;
  }
  if (!bundle.isMasterDetailEntity) {
    const assocResult = processManyToOneAssociationViews(bundle.entityShort, options, registry);
    if (!assocResult.skipped) {
      return assocResult;
    }
    console.log(`⏭️  跳过（非主子表主实体）: ${bundle.rel}`);
    return { skipped: true };
  }
  const allChildren = bundle.fullCtx.fields.masterDetailChildren || [];
  const viewChildren = filterStandaloneMenuChildren(
    allChildren,
    bundle.fullCtx.modulePath,
    bundle.entityShort,
  );
  if (!viewChildren.length) {
    console.log(`⏭️  跳过（子实体均有独立菜单，主实体走单表 CRUD）: ${bundle.rel}`);
    return { skipped: true };
  }
  validateMasterDetailChildrenAlignment(
    bundle.entityShort,
    bundle.fullCtx.modulePath,
    allChildren,
    bundle.ifaceMap,
  );
  const plans = resolveMasterDetailViewPlans(
    bundle.fullCtx.viewModulePath,
    bundle.fullCtx.modulePath,
    allChildren,
    bundle.entityShort,
  );
  if (!plans.length) {
    console.log(`⏭️  跳过（无可用菜单导航的主子视图）: ${bundle.rel}`);
    return { skipped: true };
  }
  console.log(`  主子表子实体（OneToMany）: ${allChildren.map((c) => c.childPascal).join(', ')}`);
  console.log(`  菜单导航主子视图: ${plans.length} 个 ← ${plans.map((p) => p.viewModulePath).join(', ')}`);
  console.log(`  entityScope: ${bundle.fullCtx.fields.entityScope} ← Takt${bundle.entityShort}`);
  plans.forEach((plan) => {
    const planChildren = [plan.childMeta];
    const childFields = cloneFieldMetaWithMasterDetailChildren(bundle.fullCtx.fields, planChildren);
    const childCtx = { ...bundle.fullCtx, fields: childFields };
    if (plan.viewModulePath === bundle.fullCtx.viewModulePath) {
      console.log(`  ▶ 主菜单主子: ${plan.viewModulePath}（Takt${plan.childMeta.childPascal}）`);
      writeSingleMasterDetailView(bundle, childCtx, options);
      return;
    }
    const childBundle = buildViewBundle(bundle, plan.viewModulePath, childFields);
    const navCtx = {
      ...childCtx,
      viewModulePath: plan.viewModulePath,
      cssRootClass: plan.viewModulePath.replace(/\//g, '-'),
    };
    console.log(`  ▶ 子导航主子: ${plan.viewModulePath}（Takt${plan.childMeta.childPascal}）`);
    writeSingleMasterDetailView(childBundle, navCtx, options);
  });
  return { skipped: false, created: true };
}

function printMasterDetailUsage() {
  console.log(`
用法: node scripts/generate-vue-master-detail-from-api.cjs [参数]

模板: **主子表 Master-Detail**（列表 TaktMasterDetailTableLr + 弹窗 TaktEditableTable 上主下从）

参数:
  --<实体名>            如 --DictType
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 实体仅 1 个 OneToMany：生成 1 个主子视图（参照 SerialInbound）
  - 视图目录数 = 菜单 ComponentPath 数（主菜单 + 与子表 viewChildKebab 路径一致的子导航菜单）
  - 主菜单绑定「尚无独立 ComponentPath 菜单」的首个 eligible 子表；子实体有独立实体菜单则不计入主表主子规划
  - 子实体在 shouldExcludeVueGeneration 排除列表者，虽有菜单仍视为主表子导航（非独立实体页）
  - 特例：人事 Employee / 日常 News — 主菜单单表 CRUD；各子表独立菜单页为「主表 + 该子表明细」主子视图（不改普通主子表规则）

示例:
  node scripts/generate-vue-master-detail-from-api.cjs --DictType
  node scripts/generate-vue-master-detail-from-api.cjs --Equipment
  node scripts/generate-vue-master-detail-from-api.cjs --MaintenanceNotification
`);
}

if (require.main === module) {
  runVueGeneratorCli({
    banner: '🚀 主子表 Vue（generate-vue-master-detail-from-api.cjs）...\n',
    printUsage: printMasterDetailUsage,
    templateType: VUE_TEMPLATE.MASTER_DETAIL,
    buildRegistry: buildMasterDetailChildRegistry,
    onInit: buildMenuIndex,
    processModule: processMasterDetailApiModule,
  });
}

module.exports = {
  processMasterDetailApiModule,
  processManyToOneAssociationViews,
  processChildMasterMultiNavViews,
  generateMasterDetailIndexVue,
  generateMasterDetailFormVue,
};

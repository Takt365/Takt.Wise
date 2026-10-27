// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-tree-from-api.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：树表 Vue 视图生成（左右均为树表：超 500 自动懒加载+virtual，右表无分页）；查询栏关键字=左/右表栏宽−查询/重置（组件 flex）；单表见 generate-vue-crud-from-api.cjs；主子表见 generate-vue-master-detail-from-api.cjs
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const { findDomainEntityFile } = require('./generate-script-common.cjs');
const { buildTreeIndexStateRefs, buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  FORM_TAB_FIELDS_PER_TAB,
  resolveFormFieldColSpan,
  buildFormRowMarkup,
  buildFormFieldColItems,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderQueryFormItem,
  renderFormControl,
  renderFormItemOpening,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  computeFormTabCount,
  buildFormTabPanesMarkup,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  hasScopeContextFormFields,
  resolveScopeFormFieldPresence,
  buildScopeContextFormScriptFragments,
  pascalToCamel,
  buildMenuIndex,
  buildMasterDetailChildRegistry,
  loadVueModuleContext,
  writeVueModuleOutputs,
  resolveFieldTranslationKey,
  fieldsUseDictSelect,
  buildDictDataStoreImportLine,
  buildDictDataStoreIndexSetup,
  buildGeneratedFormVueScriptFragments,
  buildResetPeriodListMapperScriptBlock,
  buildListDictTagValueExpr,
  resolveListSwitchAndDictColsForIndex,
  buildListSwitchBodyCellLine,
  buildListSwitchHandlersBlock,
  buildEntityNumericCoerceHelper,
  INDEX_FORM_RESET_NEXT_TICK,
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
  entityRowRecordTypeName,
  toFrontendEntityFileCamel,
} = require('./generate-vue-common.cjs');

/**
 * Domain 实体是否含 ParentId（树形）
 * @param {string} entityPascal
 * @param {string} backendRoot
 * @returns {boolean}
 */
function entityHasParentId(entityPascal, backendRoot) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (!entityFile) {
    return false;
  }
  const content = fs.readFileSync(entityFile, 'utf-8');
  return /public\s+long(?:\?)?\s+ParentId\s*\{/.test(content);
}

/**
 * 扩展 API 能力：树接口
 * @param {string} entityPascal
 * @param {Record<string, string>} methods
 */
function extendTreeApiCapabilities(entityPascal, methods) {
  const names = Object.keys(methods);
  const pick = (...candidates) => candidates.find((c) => names.includes(c)) || '';
  return {
    hasGetTree: Boolean(pick(`get${entityPascal}Tree`)),
    apiGetTree: pick(`get${entityPascal}Tree`),
    hasGetTreeOptions: Boolean(pick(`get${entityPascal}TreeOptions`)),
    apiGetTreeOptions: pick(`get${entityPascal}TreeOptions`),
    entityTreeType: `${entityPascal}Tree`,
  };
}

/**
 * 解析树节点标题字段与右侧关键字搜索字段
 * @param {Map<string, { properties: Array<{ name: string, type: string }> }>} interfaces
 * @param {string} entityPascal
 * @param {string} entityCamel
 * @param {object[]} listFields
 */
function resolveTreeFieldMeta(interfaces, entityPascal, entityCamel, listFields) {
  const props = interfaces.get(entityPascal)?.properties || [];
  const propNames = new Set(props.map((p) => p.name));
  const nameField = `${entityCamel}Name`;
  const codeField = `${entityCamel}Code`;
  const isStringProp = (p) => typeof p.type === 'string' && /\bstring\b/.test(p.type);
  const skipCodeNames = new Set([
    'tenantCode', 'companyCode', 'cultureCode', 'currencyCode', 'plantCode',
    'relatedPlant', 'postalCode', 'phoneCode', 'countryCode',
  ]);
  /** 优先：实体Name → 任意 *Name → 实体Code → 业务 *Code → 列表首个业务字符串 → Id */
  let titleField = nameField;
  if (!propNames.has(nameField)) {
    const nameLike = props.find((p) => isStringProp(p) && /Name$/i.test(p.name))?.name;
    if (nameLike) {
      titleField = nameLike;
    } else if (propNames.has(codeField)) {
      titleField = codeField;
    } else {
      const codeLike = props.find(
        (p) => isStringProp(p) && /Code$/i.test(p.name) && !skipCodeNames.has(p.name),
      )?.name;
      titleField = codeLike
        || listFields.find(
          (f) => f.type === 'string'
            && !['parentId', 'sortOrder', 'level', 'extField', 'remark', ...skipCodeNames].includes(f.name)
            && !/Id$/i.test(f.name),
        )?.name
        || `${entityCamel}Id`;
    }
  }
  const codeCompanion = /Name$/i.test(titleField)
    ? titleField.replace(/Name$/i, 'Code')
    : '';
  const searchFields = [titleField, codeCompanion, nameField, codeField]
    .filter((n, i, arr) => n && propNames.has(n) && arr.indexOf(n) === i);
  return {
    titleField,
    searchFields: searchFields.length > 0 ? searchFields : [titleField],
  };
}

/**
 * 生成树拖拽更新用的 buildXxxUpdateDto（详情 DTO → Update DTO，对齐 Create 字段）
 * @param {string} entityPascal
 * @param {string} entityCamel
 * @param {string} idField
 * @param {Array<{ name: string }>} createProperties CreateDto 属性（不含审计字段）
 */
function buildTreeUpdateDtoFunction(entityPascal, entityCamel, idField, createProperties) {
  if (!createProperties.length) {
    return '';
  }
  const hasSortOrderInUpdateDto = createProperties.some((p) => p.name === 'sortOrder');
  const overridesType = hasSortOrderInUpdateDto
    ? `Pick<${entityPascal}Update, 'parentId' | 'sortOrder'>`
    : `Pick<${entityPascal}Update, 'parentId'> & { sortOrder: number }`;
  const fieldLines = createProperties.map((p) => {
    if (p.name === 'cultureCode') {
      return `    cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',`;
    }
    if (p.name === 'parentId' || p.name === 'sortOrder') {
      return `    ${p.name}: overrides.${p.name},`;
    }
    return `    ${p.name}: ${entityCamel}.${p.name},`;
  });
  return `/**
 * 将详情 DTO 映射为更新载荷（树拖拽改 parentId/sortOrder 等场景）
 * @param ${entityCamel} 实体详情
 * @param overrides 需覆盖的 parentId、sortOrder
 * @returns {${entityPascal}Update} 更新载荷
 */
function build${entityPascal}UpdateDto(
  ${entityCamel}: ${entityPascal},
  overrides: ${overridesType},
): ${entityPascal}Update {
  return {
    ${idField}: String(${entityCamel}.${idField}),
${fieldLines.join('\n')}
  }
}`;
}

/**
 * 右侧树表客户端过滤（关键字 + 高级查询字段）
 * @param {string} entityPascal
 * @param {Array<{ name: string, type?: string }>} queryFields
 * @param {string[]} searchFields
 * @param {string} idField
 * @param {string} codeField
 * @param {string} titleField
 * @returns {string}
 */
function buildTreeMatchesRightQueryScript(entityPascal, queryFields, searchFields, idField, codeField, titleField) {
  const kwFields = (Array.isArray(searchFields) && searchFields.length
    ? searchFields
    : [codeField, titleField, idField]
  ).filter((f, i, arr) => f && arr.indexOf(f) === i);
  const kwCondition = kwFields
    .map((f) => `!String(record.${f} ?? '').toLowerCase().includes(k)`)
    .join(' && ');
  const advLines = (queryFields || []).map((f) => {
    if (f.type === 'number') {
      return `  if (advancedQueryForm.value.${f.name} !== undefined && record.${f.name} !== advancedQueryForm.value.${f.name}) return false`;
    }
    return `  if (advancedQueryForm.value.${f.name} && !String(record.${f.name} ?? '').includes(String(advancedQueryForm.value.${f.name}))) return false`;
  }).join('\n');
  return `/** 右侧查询条件过滤（仅影响表格展示，不重建左侧树） */
function matches${entityPascal}RightQuery(record: Record<string, unknown>): boolean {
  const kw = queryKeyword.value.trim()
  if (kw) {
    const k = kw.toLowerCase()
    if (${kwCondition || 'false'}) return false
  }
${advLines}
  return true
}`;
}

/**
 * 生成树表 index.vue（左树导航 + 右表树，无分页）
 * 左/右取数一律 GET …/tree?parentId=（禁止用 list 探测/组树）。
 * 左树：根层 parentId=0 + loadData；点击仅选中；三角手风琴；工具栏展开已加载层。
 * 右表：默认空；选中后 getById + tree 拉齐该节点全部子孙并展开。
 */
function generateTreeIndexVue(ctx, helpers) {
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    permissionPrefix,
    cssRootClass,
    apiBase,
    caps,
    fields,
    comment,
    treeMeta,
    updateDtoFields = [],
  } = ctx;
  const { fieldLabelTExpr, fieldPlaceholderTExpr } = helpers;
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const { titleField } = treeMeta;
  const entityScope = fields.entityScope || 'company';
  const importApiNames = [
    caps.apiGetTree,
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
    caps.apiUpdateSort,
  ].filter(Boolean);
  const codeField = `${entityCamel}Code`;
  const matchesRightQueryBlock = buildTreeMatchesRightQueryScript(
    entityPascal,
    fields.queryFields,
    treeMeta.searchFields || [],
    caps.entityIdName,
    codeField,
    titleField,
  );
  const listCols = fields.listFields.filter((f) => f.name !== caps.entityIdName && f.name !== 'children');
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
  const searchFieldLabelExprs = (treeMeta.searchFields || []).length
    ? (treeMeta.searchFields || []).map((f) => `pi.label('${f}')`).join(', ')
    : 'pi.self()';
  const queryItems = fields.queryFields.map((f) => helpers.renderQueryFormItem(f)).join('\n');
  const queryFactoryBlock = buildAdvancedQueryFactoryBlock(entityPascal, fields.queryFields);
  const queryInit = fields.queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const advancedQueryResetExpr = fields.queryFields.length
    ? 'createEmptyAdvancedQueryForm()'
    : `{\n${queryInit}\n  }`;
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}`;
  const columnBlocks = listCols.map((f) => {
    if (f.name === titleField) {
      return `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 160,
    resizable: true,
    ellipsis: true,
  },`;
    }
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
    customRender: ({ record }: { record: Record<string, unknown> }) => get${entityPascal}Field(record, '${f.name}') ?? ''
  },`;
  }).join('\n');
  const resetPeriodListMapperBlock = buildResetPeriodListMapperScriptBlock(dictTagListCols);
  const switchBodyCellExtra = switchListCols
    .filter((f) => f.name !== titleField)
    .map((f, i) => {
      const branch = titleField ? 'v-else-if' : (i === 0 ? 'v-if' : 'v-else-if');
      return buildListSwitchBodyCellLine(f, entityPascal, branch);
    })
    .join('\n');
  const dictBodyCellExtra = dictTagListCols.filter((f) => f.name !== titleField).map((f, i) => {
    const priorCount = (titleField ? 1 : 0) + switchListCols.filter((sf) => sf.name !== titleField).length;
    const branch = priorCount === 0 && i === 0 ? 'v-if' : 'v-else-if';
    const valueExpr = f.name === 'resetPeriod'
      ? `mapResetPeriodDictValue(get${entityPascal}DictValue(record, '${f.name}') as string | number | undefined)`
      : `get${entityPascal}DictValue(record, '${f.name}')`;
    return `          <template ${branch}="column.key === '${f.name}'">
            <TaktDictTag
              :value="${valueExpr}"
              dict-type="${f.dictType}"
            />
          </template>`;
  }).join('\n');
  const listSwitchHandlersBlock = buildListSwitchHandlersBlock(
    switchListCols,
    entityPascal,
    caps,
    { reloadAfterSuccess: true, recordType: `${entityPascal}RowRecord` },
  );
  const titleBodyCell = titleField ? `          <template v-if="column.key === '${titleField}'">
            <span>{{ get${entityPascal}Field(record, '${titleField}') }}</span>
          </template>` : '';
  const bodyCellBlock = (titleField || switchListCols.length || dictTagListCols.length)
    ? `        <!-- 自定义列渲染 -->
        <template #bodyCell="{ column, record }">
${titleBodyCell}
${switchBodyCellExtra}
${dictBodyCellExtra}
        </template>`
    : '';
  const idField = caps.entityIdName;
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
        :key="formData?.${idField} ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>` : '';
  const importBlock = (caps.hasImport && caps.hasGetTemplate)
    ? buildImportModalVueBlock(entityPascal)
    : '';
  const entityI18nIndexImport = buildEntityI18nIndexImportBlock(entityPascal, viewEntityKebab, './composables', {
    includeListFields: false,
  });
  const needsUserStoreForTreeDrop = caps.hasUpdate && updateDtoFields.some((p) => p.name === 'cultureCode');
  const hasSortOrderInUpdateDto = updateDtoFields.some((p) => p.name === 'sortOrder');
  const treeUpdateDtoBlock = caps.hasUpdate
    ? buildTreeUpdateDtoFunction(entityPascal, entityCamel, idField, updateDtoFields)
    : '';
  const treeStateBlock = buildTreeIndexStateRefs(entityPascal, {
    hasForm: caps.hasCreate || caps.hasUpdate,
    hasImport: caps.hasImport && caps.hasGetTemplate,
    idField,
    titleField,
    queryInit,
    queryFactoryBlock,
    searchFieldLabelExprs,
    needsUserStore: needsUserStoreForTreeDrop,
    needsExcelNames: caps.hasImport || caps.hasExport,
    entityClassName: caps.entityClassName,
    entityPascal,
  });
  const remixIconImport = buildRemixIconImportLine({
    includeActionIcons: caps.hasUpdate || caps.hasDelete,
    queryFields: fields.queryFields,
  });
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath} -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：${comment}树表管理页（仅 tree API；右表选中后展示子孙），由 generate-vue-tree-from-api.cjs 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="${cssRootClass}">
    <!-- 查询栏 -->
    <div class="${cssRootClass}-query-row">
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
    <div class="${cssRootClass}-toolbar-row">
      <TaktTreeLeftToolsBar
        v-model:expanded="treeExpanded"
        :loading="loading"
        @search="loadData"
      />
      <TaktTreeRightToolsBar
${caps.hasCreate ? `        create-permission="${permissionPrefix}:create"` : ''}
${caps.hasUpdate ? `        update-permission="${permissionPrefix}:update"` : ''}
${caps.hasDelete ? `        delete-permission="${permissionPrefix}:delete"` : ''}
${caps.hasImport ? `        import-permission="${permissionPrefix}:import"` : ''}
${caps.hasExport ? `        export-permission="${permissionPrefix}:export"` : ''}
        :show-create="${caps.hasCreate}"
        :show-update="${caps.hasUpdate}"
        :show-delete="${caps.hasDelete || caps.hasDeleteBatch}"
        :show-import="${caps.hasImport && caps.hasGetTemplate}"
        :show-export="${caps.hasExport}"
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
${caps.hasImport && caps.hasGetTemplate ? '        @import="handleImport"' : ''}
${caps.hasExport ? '        @export="handleExport"' : ''}
        @advanced-query="handleAdvancedQuery"
        @column-setting="handleColumnSetting"
        @refresh="handleRefresh"
        @update:expanded="(v: boolean) => (tableExpanded = v)"
      />
    </div>

    <!-- 左树右表：左导航树；右表仅在选中后展示该节点全部子孙树 -->
    <div class="${cssRootClass}-tree-table-wrap">
      <TaktTreeLeftTable
        v-model:expanded-keys="treeExpandedKeys"
        v-model:selecmed-keys="selecmedTreeKeys"
        :tree-data="filteredTreeData"
        :tree-field-names="{ title: 'title', key: 'key', children: 'children' }"
        :tree-width-ratio="0.2"
        :loading="loading"
        :virtual="true"
        :draggable="${caps.hasUpdate}"
        :accordion="false"
        :expand-action="false"
        :load-data="useLazyTree ? handleLeftTreeLoadData : undefined"
        @tree-select="handleTreeSelect"
        @tree-drop="handleTreeDrop"
      />
      <TaktTreeRightTable
        entity-scope="${entityScope}"
        :columns="columns"
        :visible-column-keys="visibleColumnKeys"
        :id-column-key="'${idField}'"
        :action-column-key="'action'"
        table-mode="tree"
        :data-source="tableFilteredTree"
        v-model:expanded-row-keys="tableExpandedRowKeys"
        :load-children="useLazyTree ? handleRightTreeLoadChildren : undefined"
        :loading="listLoading"
        :row-key="get${entityPascal}Id"
        :stripe="true"
        :row-selection="rowSelection"
        :virtual="true"
        @change="handleTableChange"
        @resize-column="handleResizeColumn"
      >
${bodyCellBlock}
      </TaktTreeRightTable>
    </div>
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
      entity-scope="${entityScope}"
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'${idField}'"
      :action-column-key="'action'"
      table-mode="tree"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}树表管理页 · 左树选中后右表展示该节点+直接子级（更深展开懒加载）；默认右表为空
 * @module views/${viewModulePath}
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
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
${(caps.hasCreate || caps.hasUpdate) ? TAKT_FORM_MODAL_WIDTH_IMPORT : ''}import {
  mapLazyTreeNodes,
  mergeLoadedChildren,
  taktIsLeafFlag,
  type TaktLazyTreeNode,
} from '@/composables/useLazyTree'
${(caps.hasCreate || caps.hasUpdate) ? `import ${entityPascal}Form from './components/${entityPascal}Form.vue'\n` : ''}import { ${importApiNames.join(', ')} } from '@/api/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.api'
import type { ${entityPascal}, ${caps.entityTreeType}, ${entityPascal}Update } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'
import type { TreeDropPayload } from '@/components/business/TaktTreeLeftTable/index.vue'
${indexDictImport}${(caps.hasImport || caps.hasExport) ? "import { taktExcelEntityNames } from '@/utils/naming'\n" : ''}${caps.hasExport ? "import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'\n" : ''}${(caps.hasImport && caps.hasGetTemplate) ? buildVueImportResultUtilImportLine() : ''}${remixIconImport}${needsUserStoreForTreeDrop ? "import { useUserStore } from '@/stores/identity/useUserStore'\n" : ''}
${entityI18nIndexImport}
${treeStateBlock}
${indexDictSetup}
${matchesRightQueryBlock}

/**
 * 将树 API 一层 DTO 映射为左侧懒加载节点
 * @param rows 一层子节点
 */
function map${entityPascal}LazyNodes(rows: ${caps.entityTreeType}[]): TaktLazyTreeNode[] {
  return mapLazyTreeNodes(rows, {
    getKey: (n) => String(n.${idField} ?? ''),
    getTitle: (n) => String(n.${titleField} || ${(titleField !== codeField) ? `n.${codeField} || ` : ''}n.${idField} || ''),
    isLeaf: (n) => taktIsLeafFlag((n as { isLeaf?: unknown }).isLeaf),
  })
}

/**
 * 将一层 DTO 映射为右侧树表节点（未加载子级用 _hasChildren 显示展开箭头）
 * @param rows 一层子节点
 */
function map${entityPascal}RightTreeNodes(rows: ${caps.entityTreeType}[]): Record<string, unknown>[] {
  return (rows ?? []).map((row) => {
    const rec = row as Record<string, unknown>
    const id = String(rec.${idField} ?? '')
    const rawChildren = Array.isArray(rec.children) ? rec.children as ${caps.entityTreeType}[] : []
    const children = rawChildren.length > 0 ? map${entityPascal}RightTreeNodes(rawChildren) : undefined
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
    (node) => taktTreeTableNodeKey(node, '${idField}'),
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
function unwrap${entityPascal}Tree(res: unknown): ${caps.entityTreeType}[] {
  const resAny = res as { data?: ${caps.entityTreeType}[]; Data?: ${caps.entityTreeType}[] }
  if (Array.isArray(res)) return res as ${caps.entityTreeType}[]
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
    const detail = await ${caps.apiGetById}(selecmedId) as Record<string, unknown>
    const childRows = unwrap${entityPascal}Tree(await ${caps.apiGetTree}(selecmedId, true))
    const mappedChildren = map${entityPascal}RightTreeNodes(childRows)
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
    logger.error('[${entityPascal}] 加载右侧树失败', undefined, error)
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
  const trees = unwrap${entityPascal}Tree(await ${caps.apiGetTree}(parentId, true))
  const children = map${entityPascal}RightTreeNodes(trees)
  tableTreeData.value = mergeLoadedChildren(
    tableTreeData.value as TaktLazyTreeNode[],
    parentId,
    children as TaktLazyTreeNode[],
    { keyField: '${idField}' },
  )
}

/**
 * 右侧树展开：再拉一层子节点（懒加载，并发受限）
 * @param record 当前行
 */
async function handleRightTreeLoadChildren(record: Record<string, unknown>) {
  const id = get${entityPascal}Id(record)
  if (!id) return
  await runWithTaktTreeLoadConcurrency(async () => {
    await loadRightChildrenByParentId(id)
  })
}

/**
 * 加载左侧树根节点（仅 GET tree?parentId=0，一层）
 */
async function reloadLeftTreeRoots() {
  const trees = unwrap${entityPascal}Tree(await ${caps.apiGetTree}('0', true))
  entityTreeData.value = map${entityPascal}LazyNodes(trees)
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
  const trees = unwrap${entityPascal}Tree(await ${caps.apiGetTree}(parentKey, true))
  const children = map${entityPascal}LazyNodes(trees)
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
  const trees = unwrap${entityPascal}Tree(await ${caps.apiGetTree}(parentId, true))
  const children = map${entityPascal}LazyNodes(trees)
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
  filterTaktTreeTableNodes(tableDisplayTree.value, matches${entityPascal}RightQuery)
)

/**
 * 同步右侧树表 expandable keys（展开态；含未加载非叶子）
 */
function apply${entityPascal}TableExpandState() {
  if (!tableExpanded.value) {
    tableExpandedRowKeys.value = []
    return
  }
  const next = collectTaktTreeTableExpandableKeys(
    tableFilteredTree.value,
    (node) => taktTreeTableNodeKey(node, '${idField}'),
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
    getKey: (node) => taktTreeTableNodeKey(node, '${idField}'),
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
    apply${entityPascal}TableExpandState()
  }
})

${treeUpdateDtoBlock ? `${treeUpdateDtoBlock}\n\n` : ''}/** 从树结构中查找节点 key 的父级 key 与在同级中的序号（用于 parentId / sortOrder） */
function findParentAndOrderNum(
  tree: Array<Record<string, unknown>>,
  targetKey: string | number,
  parentKey: string = '0',
): { parentId: string; sortOrder: number } | null {
  const keyStr = String(targetKey)
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i]
    const k = String(node?.key ?? node?.${idField} ?? '')
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

${caps.hasUpdate ? `/**
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
    const full = await ${caps.apiGetById}(String(dragKey))
    await ${caps.apiUpdate}(String(dragKey), build${entityPascal}UpdateDto(full, {
      parentId: pos.parentId,
      sortOrder: pos.sortOrder,
    }))
${!hasSortOrderInUpdateDto && caps.apiUpdateSort ? `    await ${caps.apiUpdateSort}({ ${idField}: String(dragKey), sortOrder: pos.sortOrder })` : ''}
    message.success(t('common.feedback.updated', { target: pi.self() }))
    await loadData()
  } catch (error: unknown) {
    message.error(getErrorMessage(error, t('common.feedback.update.failed', { target: pi.self() })))
    await reloadLeftTreeRoots().catch(() => undefined)
  } finally {
    loading.value = false
  }
}` : 'const handleTreeDrop = async () => {}'}

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
    getKey: (node) => taktTreeTableNodeKey(node, '${idField}'),
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
type ${entityPascal}RowRecord = ${entityPascal} | Record<string, unknown>

/** 表格 row-key（优先实体主键字段） */
const get${entityPascal}Id = (record: ${entityPascal}RowRecord): string => {
  if (record != null && '${idField}' in record && (record as Record<string, unknown>).${idField} != null) {
    return String((record as Record<string, unknown>).${idField})
  }
  if (record != null && 'id' in record && (record as Record<string, unknown>).id != null) {
    return String((record as Record<string, unknown>).id)
  }
  return ''
}
/** 读取行字段值 */
const get${entityPascal}Field = (record: ${entityPascal}RowRecord, field: string): unknown =>
  (record as Record<string, unknown>)?.[field]
/** 供 TaktDictTag 等组件使用的标量字典值 */
const get${entityPascal}DictValue = (
  record: ${entityPascal}RowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
${switchListCols.length > 0 ? buildEntityNumericCoerceHelper(entityPascal) : ''}
${resetPeriodListMapperBlock}

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
    dataIndex: '${idField}',
    key: '${idField}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: Record<string, unknown> }) =>
      get${entityPascal}Field(record, '${idField}') ?? get${entityPascal}Field(record, 'id') ?? '',
  },
${columnBlocks}
  CreateActionColumn<${entityPascal}>({
    actions: [
${actionItems.join('\n')}
    ],
  }),
  ]
})

/** 行选择配置 */
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${entityPascal}[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? rows[0] : null
  },
  onSelect: (record: ${rowRecordType}, selecmed: boolean) => {
    if (selecmed) selecmedRow.value = record
    else if (selecmedRow.value && get${entityPascal}Id(selecmedRow.value) === get${entityPascal}Id(record)) selecmedRow.value = null
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: ${entityPascal}[]) => {
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
    logger.error('[${entityPascal}] 加载树数据失败', undefined, error)
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
    logger.error('[${entityPascal}] 刷新失败', undefined, error)
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
  advancedQueryForm.value = ${advancedQueryResetExpr}
}

${listSwitchHandlersBlock}

${caps.hasCreate ? `/** 新增：默认 parentId 为当前左侧选中节点 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  const keys = selecmedTreeKeys.value
  formData.value = {
    parentId: keys.length > 0 ? String(keys[keys.length - 1]) : '0',
  }
  formVisible.value = true${INDEX_FORM_RESET_NEXT_TICK}
}` : ''}

${caps.hasUpdate ? `/** 打开编辑弹窗 */
function handleEdit(record: ${rowRecordType}) {
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
}` : ''}

${(caps.hasCreate || caps.hasUpdate) ? `/** 提交新增/编辑表单 */
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
${caps.hasUpdate ? `      await ${caps.apiUpdate}(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))` : ''}
    } else {
${caps.hasCreate ? `      await ${caps.apiCreate}(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))` : ''}
    }
    formVisible.value = false
    formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
    await refreshAfterMutation()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null${INDEX_FORM_RESET_NEXT_TICK}
}` : ''}

${caps.hasDelete ? `/** 删除单行 */
async function handleDeleteOne(record: ${rowRecordType}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${caps.apiDelete}((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}` : ''}

${caps.hasDeleteBatch ? `/** 批量删除选中行 */
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
      await ${caps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await refreshAfterMutation()
    }
  })
}` : ''}

${(caps.hasImport && caps.hasGetTemplate) ? buildImportHandlersScriptBlock({
  apiGetTemplate: caps.apiGetTemplate,
  apiImport: caps.apiImport,
  successBody: 'void refreshAfterMutation()',
}) : ''}

${caps.hasExport ? `/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    const exportMeta = await ${caps.apiExport}({ pageIndex: 1, pageSize: 100000 }, excelNames.sheet, excelNames.fileBase)
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
  } catch (error: unknown) {
    logger.error('[${entityPascal}] 导出失败', undefined, error)
    message.error(getErrorMessage(error, t('common.feedback.export.failed', { target: pi.self() })))
  } finally {
    loading.value = false
  }
}` : ''}

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
  advancedQueryForm.value = ${advancedQueryResetExpr}
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
${indexDictOnMounted}  void loadData()
})
useTableRefresh(loadData)
</script>

<style scoped lang="css">
.${cssRootClass} {
  padding: 16px;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.${cssRootClass}-query-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.${cssRootClass}-toolbar-row {
  display: flex;
  gap: 8px;
  margin-bottom: 8px;
}
.${cssRootClass}-tree-table-wrap {
  display: flex;
  flex: 1;
  min-height: 0;
  gap: 8px;
}
</style>
`;
}

function generateTreeFormBase(ctx) {
  const { entityPascal, entityCamel, entityKebab, viewEntityKebab, modulePath, viewModulePath, fields, comment } = ctx;
  const generatorScript = 'generate-vue-tree-from-api.cjs';
  const mdFormParts = { tabs: '', script: '', needsTaktSelect: false };
  const hasMasterDetail = false;
  const formFields = fields.formFields;
  const treeFormFields = formFields.filter((f) => f.name !== 'parentId');
  const entityIdField = `${entityCamel}Id`;
  const formCodeControlOptions = { entityIdField };
  const { tabsHtml } = buildFormTabPanesMarkup({
    formFields: treeFormFields,
    formCodeControlOptions,
    hasMasterDetail,
  });
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const masterDetailChildren = fields.masterDetailChildren || [];
  const hasScopeContextFields = hasScopeContextFormFields(treeFormFields, masterDetailChildren);
  const scopePresence = resolveScopeFormFieldPresence(treeFormFields, masterDetailChildren);
  const scopeFragments = buildScopeContextFormScriptFragments(scopePresence, entityIdField, entityPascal);
  const scopeStoreImports = hasScopeContextFields ? scopeFragments.imports : '';
  const scopeStoreScript = hasScopeContextFields ? scopeFragments.script : '';
  const scopeContextWatch = hasScopeContextFields ? scopeFragments.watch : '';
  const childTypeImports = hasMasterDetail
    ? [...new Set((fields.masterDetailChildren || []).flatMap((c) => [c.childCreateType, c.childType]))]
    : [];
  const typeImportLine = [`${entityPascal}Create`, ...childTypeImports]
    .filter((name, idx, arr) => arr.indexOf(name) === idx)
    .join(', ');
  const childFieldStrip = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `    delete (next as any).${c.fieldName}`).join('\n')
    : '';
  const watchSyncChild = hasMasterDetail ? '    syncChildRowsFromFormData(val)\n' : '';
  const resetChildRows = hasMasterDetail
    ? (fields.masterDetailChildren || []).map((c) => `  child${c.childPascal}Rows.value = []`).join('\n')
    : '';
  // TaktSelect 等业务 SFC：unplugin-vue-components 自动注册，禁止手写 import
  const extFieldIconImport = buildExtFieldIconImportLine(treeFormFields);
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields: treeFormFields,
    entityIdField,
    childFieldStrip,
    hasScopeContextFields,
    watchSyncChild,
    useBuildSubmitPayload: hasMasterDetail,
  });
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(entityIdField, hasScopeContextFields);
  const getValuesBody = formScriptFragments.getValuesBody;
  const formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(treeFormFields.map((f) => f.name)),
    mdScript: mdFormParts.script,
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal,
    entityIdField,
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
    <TaktFormTabs
      v-model:active-key="activeTab"
      class="${viewEntityKebab}-form-tabs"
    >
${tabsHtml}
${mdFormParts.tabs}
    </TaktFormTabs>
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
${buildEntityI18nFormImportBlock(entityPascal, viewEntityKebab)}
import type { ${typeImportLine} } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'
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

/** 重置表单与子表行 */
/** 重置表单与子表行（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
${resetScopeDefaultsLine}${resetChildRows}
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>
`;
}

/**
 * 树表表单 parentId 默认根节点 0（无树数据 / tree-options 为空时，参照 DeptForm.vue）
 * @param {string} content
 * @returns {string}
 */
function patchTreeFormParentIdHandling(content) {
  if (content.includes('function normalizeTreeParentId')) {
    let next = content;
    if (/const FORM_FIELD_DEFAULTS:/.test(next) && !/parentId:\s*'0'/.test(next)) {
      next = next.replace(
        /const FORM_FIELD_DEFAULTS: Record<string, string \| number> = \{\n/,
        "const FORM_FIELD_DEFAULTS: Record<string, string | number> = {\n  parentId: '0',\n",
      );
    }
    if (!/normalizeTreeParentId\(target\)/.test(next)) {
      next = next.replace(
        'function applyFormDefaults(target: Record<string, unknown>) {\n  Object.assign(target, FORM_FIELD_DEFAULTS)\n}',
        `function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
      );
    }
    return next;
  }
  const normalizeHelper = `
/** 树表 parentId：空值归一为根节点 0（string，与后端 ParentId=0 一致） */
function normalizeTreeParentId(target: Record<string, unknown>) {
  const raw = target.parentId
  target.parentId = raw === '' || raw === undefined || raw === null ? '0' : String(raw)
}
`;
  let next = content.replace(
    'const formState = reactive<Record<string, any>>({})',
    "const formState = reactive<Record<string, any>>({ parentId: '0' })",
  );
  if (/const FORM_FIELD_DEFAULTS:/.test(next) && !/parentId:\s*'0'/.test(next)) {
    next = next.replace(
      /const FORM_FIELD_DEFAULTS: Record<string, string \| number> = \{\n/,
      "const FORM_FIELD_DEFAULTS: Record<string, string | number> = {\n  parentId: '0',\n",
    );
  }
  if (/\/\*\* 表单字段默认值（无字典默认项） \*\//.test(next)) {
    next = next.replace(
      /\/\*\* 表单字段默认值（无字典默认项） \*\/\nfunction applyFormDefaults\(target: Record<string, unknown>\) \{\n  void target\n\}/,
      `/** 表单字段默认值（树表 parentId 根节点） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  parentId: '0',
}
${normalizeHelper}
/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
    );
  } else {
    if (!next.includes('function normalizeTreeParentId')) {
      next = next.replace(
        /\/\*\* 写入表单默认值/,
        `${normalizeHelper}/** 写入表单默认值`,
      );
    }
    next = next.replace(
      'function applyFormDefaults(target: Record<string, unknown>) {\n  Object.assign(target, FORM_FIELD_DEFAULTS)\n}',
      `function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
  normalizeTreeParentId(target)
}`,
    );
  }
  if (!next.includes('payload.parentId = parentId')) {
    next = next.replace(
      /  if \('sortOrder' in payload\) delete payload\.sortOrder\n  return payload/,
      `  const parentRaw = payload.parentId
  const parentId = parentRaw === '' || parentRaw === undefined || parentRaw === null ? '0' : String(parentRaw)
  payload.parentId = parentId
  if ('sortOrder' in payload) delete payload.sortOrder
  return payload`,
    );
  }
  next = next.replace(
    '      Object.assign(formState, next)\n      formRef.value?.clearValidate()',
    `      Object.assign(formState, next)
      normalizeTreeParentId(formState)
      formRef.value?.clearValidate()`,
  );
  return next;
}

/**
 * 生成树表 *Form.vue（含 TaktTreeSelect parentId，参照 DeptForm.vue）
 * @param {object} ctx
 * @param {object} helpers
 */
function generateTreeFormVue(ctx, helpers) {
  const base = generateTreeFormBase(ctx);
  const {
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    apiBase,
    caps,
    fields,
    comment,
  } = ctx;
  const { fieldLabelTExpr, fieldPlaceholderTExpr, renderFormControl, renderFormItemOpening, buildFormFieldColItems } = helpers;
  const formFields = fields.formFields.filter((f) => f.name !== 'parentId');
  const parentIdField = fields.formFields.find((f) => f.name === 'parentId')
    || { name: 'parentId', i18nKey: resolveFieldTranslationKey('parentId', entityI18nSlug), optional: false };
  const entityIdField = `${entityCamel}Id`;
  const treeOptionsUrl = `${apiBase}/tree-options`;
  const parentIdRow = `            <a-col :span="24">
              <a-form-item
                :label="${fieldLabelTExpr(parentIdField)}"
                name="parentId"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="${treeOptionsUrl}"
                  :lazy="true"
                  :placeholder="${fieldPlaceholderTExpr(parentIdField, 'common.page.form.placeholder.select')}"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>`;
  let content = base.replace(
    "import { useI18n } from 'vue-i18n'",
    "import { useI18n } from 'vue-i18n'",
  );
  content = content.replace(
    /功能描述：.*由 generate-vue-(?:crud|tree|master-detail)-from-api/,
    `功能描述：${comment}树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成`,
  );
  content = content.replace(
    /\*\s+\$\{comment\}维护表单/,
    `* ${comment}树表维护表单`,
  );
  const formCodeControlOptions = { entityIdField };
  const parentInsert = `          <a-row :gutter="24">
${parentIdRow}
          </a-row>`;
  const { tabsHtml } = helpers.buildFormTabPanesMarkup({
    formFields,
    formCodeControlOptions,
    beforeFirstTabExtra: parentInsert,
  });
  content = content.replace(
    /<(?:a-tabs|TaktFormTabs)[\s\S]*?<\/(?:a-tabs|TaktFormTabs)>/,
    `<TaktFormTabs
      v-model:active-key="activeTab"
      class="${viewEntityKebab}-form-tabs"
    >
${tabsHtml}
    </TaktFormTabs>`,
  );
  if (!formFields.some((f) => f.name === 'parentId')) {
    const rulesInsert = `  parentId: [
    {
      required: true,
      message: ${fieldPlaceholderTExpr(parentIdField, 'common.page.form.placeholder.select')},
      trigger: 'change'
    }
  ],`;
    content = content.replace(
      'const rules = computed<Record<string, Rule[]>>(() => ({',
      `const rules = computed<Record<string, Rule[]>>(() => ({\n${rulesInsert}`,
    );
  }
  return patchTreeFormParentIdHandling(content);
}

/**
 * 树表 API 模块处理
 * @param {string} apiFilePath
 * @param {object} options
 * @param {Map} registry
 */
function processTreeApiModule(apiFilePath, options, registry) {
  const bundle = loadVueModuleContext(apiFilePath, options, registry);
  if (bundle.skipped) {
    return bundle;
  }
  if (!bundle.isTreeEntity) {
    console.log(`⏭️  跳过（非树表：须 ParentId + getXxxTree）: ${bundle.rel}`);
    return { skipped: true };
  }
  if (!bundle.capsMerged.hasGetTree && !bundle.capsMerged.hasCreate && !bundle.capsMerged.hasUpdate) {
    console.warn(`⚠️  缺少树表 API，跳过: ${bundle.rel}`);
    return { skipped: true };
  }
  console.log(`  树表: ${bundle.fullCtx.caps.apiGetTree}（仅 tree API；右表选中后展示子孙）`);
  console.log(`  entityScope: ${bundle.fullCtx.fields.entityScope} ← Takt${bundle.entityShort}`);
  const treeFieldMetaRaw = resolveTreeFieldMeta(
    bundle.ifaceMap,
    bundle.entityShort,
    bundle.fullCtx.entityCamel,
    bundle.fullCtx.fields.listFields,
  );
  const treeMeta = {
    ...treeFieldMetaRaw,
  };
  const vueHelpers = {
    fieldLabelTExpr,
    fieldPlaceholderTExpr,
    renderQueryFormItem,
    renderFormControl,
    renderFormItemOpening,
    buildFormFieldColItems,
    buildFormTabPanesMarkup,
    computeFormTabCount,
    buildFormTabLabelAttr,
    FORM_TAB_FIELDS_PER_TAB,
  };
  const fullCtx = { ...bundle.fullCtx, treeMeta };
  const indexContent = generateTreeIndexVue(fullCtx, vueHelpers);
  const formContent = bundle.needsForm ? generateTreeFormVue(fullCtx, vueHelpers) : '';
  const listCols = fullCtx.fields.listFields.filter(
    (f) => f.name !== fullCtx.caps.entityIdName && f.name !== 'children',
  );
  const i18nComposableContent = buildEntityI18nComposableFile({
    entityPascal: fullCtx.entityPascal,
    entityI18nSlug: fullCtx.entityI18nSlug,
    entityKebab: fullCtx.entityKebab,
    viewModulePath: fullCtx.viewModulePath,
    viewEntityKebab: fullCtx.viewEntityKebab,
    modulePath: fullCtx.modulePath,
    listFields: listCols,
    formFields: fullCtx.fields.formFields,
    queryFields: fullCtx.fields.queryFields,
    comment: fullCtx.comment,
  });
  return writeVueModuleOutputs(bundle, indexContent, formContent, options, i18nComposableContent);
}

module.exports = {
  entityHasParentId,
  extendTreeApiCapabilities,
  resolveTreeFieldMeta,
  generateTreeIndexVue,
  generateTreeFormVue,
  patchTreeFormParentIdHandling,
  processTreeApiModule,
};

const { runVueGeneratorCli, VUE_TEMPLATE } = require('./generate-vue-common.cjs');


function printTreeUsage() {
  console.log(`
用法: node scripts/generate-vue-tree-from-api.cjs [参数]

模板: **树表 TREE**（仅 GET …/tree；左根懒加载+virtual；右表默认空，选中后展示该节点全部子孙；TaktTreeSelect :lazy）

参数:
  --<实体名>            如 --CostCenter、--Dept（不带 Takt 前缀；Dept 为手工页，仍会跳过排除列表）
  --view-path <路径>    覆盖 views 输出目录
  --dry-run             仅预览

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 单表 CRUD → generate-vue-crud-from-api.cjs
  - 主子表 → generate-vue-master-detail-from-api.cjs
  - 一键 → generate-vue-all-from-api.cjs

示例:
  node scripts/generate-vue-tree-from-api.cjs --CostCenter
`);
}

if (require.main === module) {
  runVueGeneratorCli({
    banner: '🚀 开始生成树表 Vue 视图（generate-vue-tree-from-api.cjs）...\n',
    printUsage: printTreeUsage,
    templateType: VUE_TEMPLATE.TREE,
    buildRegistry: buildMasterDetailChildRegistry,
    onInit: buildMenuIndex,
    processModule: processTreeApiModule,
  });
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-master-detail-layout.cjs
// 功能描述：主子表 Vue 布局：列表 TaktMasterDetailTableLr + 弹窗上主下从 TaktEditableTable；查询栏关键字=栏宽−查询/重置（TaktQueryBar flex）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

const path = require('path');
const fs = require('fs');
const { writeGeneratedFile } = require('./generate-script-common.cjs');
const { buildFormScriptStateBlock } = require('./generate-vue-script-docs.cjs');
const {
  CONFIG,
  FORM_TAB_FIELDS_PER_TAB,
  fieldLabelTExpr,
  fieldPlaceholderTExpr,
  renderFormControl,
  renderApiSelectParamsAttr,
  renderFormItemOpening,
  buildFormFieldColItems,
  buildFormRowMarkup,
  buildGeneratedFormTemplateBody,
  buildFormTabsScopedStyleBlock,
  buildFormContentClassComputedExpr,
  isFixedLongTextareaField,
  isExtFieldField,
  pascalToCamel,
  renderQueryFormItem,
  buildServerPagedListQueryBlock,
  buildServerPagedExportApiCall,
  detectApiCapabilities,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  buildGeneratedFormVueScriptFragments,
  buildFormResetScopeDefaultsBlock,
  buildMasterDetailFormTypeImportLines,
  SCOPE_FORM_FIELD_NAMES,
  hasScopeContextFormFields,
  resolveScopeFormFieldPresence,
  buildScopeContextFormScriptFragments,
  buildVueImportResultUtilImportLine,
  TAKT_FORM_MODAL_WIDTH_IMPORT,
  TAKT_FORM_MODAL_WIDTH_ATTR,
  buildFormModalWidthStateBlock,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  buildChildPanelEntityI18nImportBlock,
  buildChildPanelSummarySumFieldNames,
  entityI18nConstPrefix,
  buildAdvancedQueryFactoryBlock,
  entityI18nHookName,
  entityI18nComposableFileName,
  entityMasterContextComposableFileName,
  entityRowRecordTypeName,
  toFrontendEntityFileCamel,
} = require('./generate-vue-common.cjs');

/**
 * 子表 Create 字段 → TaktEditableTableColumn 配置片段
 * @param {object} field
 * @returns {string}
 */
function mapFormFieldToEditableColumn(field, piVar = 'pi') {
  const title = fieldLabelTExpr(field, 'form', piVar);
  const width = field.htmlType === 'textarea' || field.htmlType === 'richEditor' || field.htmlType === 'fileUpload' ? 180 : 140;
  if (field.readOnly || field.htmlType === 'fileNameReadonly') {
    return `  {
    key: '${field.name}',
    title: ${title},
    readonly: true,
    width: ${width},
  }`;
  }
  if (field.htmlType === 'select' || field.htmlType === 'apiSelect' || field.htmlType === 'switch') {
    return `  {
    key: '${field.name}',
    title: ${title},
    width: ${width},
  }`;
  }
  if (field.htmlType === 'textarea' || field.htmlType === 'richEditor') {
    const rows = isFixedLongTextareaField(field) ? 2 : 1;
    const placeholder = field.optional
      ? fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'form', piVar)
      : fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required', 'form', piVar);
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'textarea',
    rows: ${rows},
    placeholder: ${placeholder},
    width: ${width},
  }`;
  }
  if (field.htmlType === 'date') {
    const valueFormat = field.name.toLowerCase().includes('time') ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD';
    const showTime = field.name.toLowerCase().includes('time') ? ', showTime: true' : '';
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'datePicker',
    valueFormat: '${valueFormat}'${showTime},
    width: ${width},
  }`;
  }
  if (field.type === 'number') {
    const summary = field.name === 'lineNumber' ? ", summary: 'sum'" : '';
    return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'inputNumber',
    width: ${width}${summary},
  }`;
  }
  const optionalParts = [];
  if (field.optional) {
    optionalParts.push('allowClear: true');
    optionalParts.push(`placeholder: ${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'form', piVar)}`);
  } else if (field.name.endsWith('SerialNo') || field.name.endsWith('serialNo')) {
    optionalParts.push('required: true');
    optionalParts.push('unique: true');
  }
  const extra = optionalParts.length ? `, ${optionalParts.join(', ')}` : '';
  return `  {
    key: '${field.name}',
    title: ${title},
    editor: 'input',
    width: ${width}${extra},
  }`;
}

/**
 * 子表 TaktEditableTable 是否需自定义 #cell 插槽（字典 / 选项 API / 开关）
 * @param {{ htmlType?: string, dictType?: string, apiUrl?: string }} field
 * @returns {boolean}
 */
function isEditableTableSlotField(field) {
  if (field.htmlType === 'switch') {
    return true;
  }
  if (field.htmlType === 'select' && field.dictType) {
    return true;
  }
  if (field.htmlType === 'apiSelect' && field.apiUrl) {
    return true;
  }
  return false;
}

function childFormFieldHasLineNumber(child) {
  return (child.formFields || []).some((f) => f.name === 'lineNumber');
}

function childFormFieldHasIsObsolete(child) {
  return (child.formFields || []).some((f) => f.name === 'isObsolete');
}

/**
 * 子表行号脚本片段（IsObsolete 子表仅据表格行递增；其余同步后端 Max*LineNumber）
 * @param {object} child
 * @returns {object}
 */
function buildChildLineNumberScriptParts(child) {
  if (!childFormFieldHasLineNumber(child)) {
    return {
      importLine: '',
      refLine: '',
      helperBlock: '',
      syncLines: '',
      syncAfterRows: '',
      resetLine: '',
      defaultLineField: (field, childPascal) => buildEditableDefaultRowFieldLine(field, childPascal),
      submitMapExtra: '',
    };
  }
  const pascal = child.childPascal;
  const camel = child.childCamel;
  const idField = child.childIdField || `${camel}Id`;
  if (childFormFieldHasIsObsolete(child)) {
    return {
      importLine: "import { resolveNextDetailLineNumber } from '@/utils/taktSequence'",
      refLine: '',
      helperBlock: `/** 是否已持久化的子表行 */
function isPersisted${pascal}Row(row: Record<string, unknown>): boolean {
  const id = row.${idField}
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNext${pascal}LineNumber(): number {
  const rows = ${camel}TableRef.value?.getRows?.() ?? child${pascal}Rows.value
  return resolveNextDetailLineNumber(0, rows)
}`,
      syncLines: '',
      syncAfterRows: '',
      resetLine: '',
      defaultLineField: () => `    lineNumber: allocateNext${pascal}LineNumber(),`,
      submitMapExtra: `      if (isUpdate && isPersisted${pascal}Row(row)) {
        normalized.${idField} = row.${idField}
      } else {
        delete normalized.${idField}
      }`,
    };
  }
  const maxRef = `max${pascal}LineNumber`;
  return {
    importLine: "import { resolveMaxLineNumber, resolveNextDetailLineNumber } from '@/utils/taktSequence'",
    refLine: `/** 后端已占用子表最大行号（与 max${pascal}LineNumber 对齐） */\nconst ${maxRef} = ref(0)`,
    helperBlock: `/** 是否已持久化的子表行 */
function isPersisted${pascal}Row(row: Record<string, unknown>): boolean {
  const id = row.${idField}
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 同步已占用最大行号 */
function syncMax${pascal}LineNumber(rows?: readonly Record<string, unknown>[]) {
  const sourceRows = rows ?? (${camel}TableRef.value?.getRows?.() ?? child${pascal}Rows.value)
  const rowMax = resolveMaxLineNumber(sourceRows.map((row) => Number(row.lineNumber) || 0))
  ${maxRef}.value = Math.max(${maxRef}.value, rowMax)
}

/** 分配下一可用子表行号 */
function allocateNext${pascal}LineNumber(): number {
  const rows = ${camel}TableRef.value?.getRows?.() ?? child${pascal}Rows.value
  const next = resolveNextDetailLineNumber(${maxRef}.value, rows)
  ${maxRef}.value = next
  return next
}`,
    syncLines: `  ${maxRef}.value = Number((val as { ${maxRef}?: number })?.${maxRef}) || 0`,
    syncAfterRows: `  syncMax${pascal}LineNumber(rows)`,
    resetLine: `  ${maxRef}.value = 0`,
    defaultLineField: () => `    lineNumber: allocateNext${pascal}LineNumber(),`,
    submitMapExtra: `      if (isUpdate && isPersisted${pascal}Row(row)) {
        normalized.${idField} = row.${idField}
      } else {
        delete normalized.${idField}
      }`,
  };
}

/**
 * 子表可编辑行默认值片段
 * @param {{ name: string, type?: string, htmlType?: string }} field
 * @param {string} childPascal
 * @param {object} [child]
 * @returns {string}
 */
function buildEditableDefaultRowFieldLine(field, childPascal, child) {
  if (field.name === 'lineNumber' && child && childFormFieldHasLineNumber(child)) {
    return `    lineNumber: allocateNext${childPascal}LineNumber(),`;
  }
  if (field.name === 'lineNumber') {
    return `    lineNumber: (child${childPascal}Rows.value.length + 1) * 10,`;
  }
  if (field.htmlType === 'date') {
    return `    ${field.name}: '',`;
  }
  if (field.type === 'number' || field.htmlType === 'switch') {
    return `    ${field.name}: 0,`;
  }
  return `    ${field.name}: '',`;
}

/**
 * 生成 TaktEditableTable 字典/选项/开关列 #cell 插槽
 * @param {object} field
 * @param {string} childCamel
 * @returns {string}
 */
function buildEditableTableCellSlotMarkup(field, childCamel) {
  const piVar = `${childCamel}Pi`;
  if (field.htmlType === 'select' && field.dictType) {
    const placeholder = field.optional
      ? fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'form', piVar)
      : fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required', 'form', piVar);
    return `      <template #cell-${field.name}="{ record }">
        <TaktSelect
          v-model:value="record.${field.name}"
          dict-type="${field.dictType}"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="${placeholder}"
          :disabled="loading"
          allow-clear
        />
      </template>`;
  }
  if (field.htmlType === 'apiSelect' && field.apiUrl) {
    const placeholder = `${piVar}.queryPh('${field.name}', 'select')`;
    return `      <template #cell-${field.name}="{ record }">
        <TaktSelect
          v-model:value="record.${field.name}"
          api-url="${field.apiUrl}"${renderApiSelectParamsAttr(field, '          ')}
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="${placeholder}"
          :disabled="loading"
          allow-clear
        />
      </template>`;
  }
  if (field.htmlType === 'switch') {
    return `      <template #cell-${field.name}="{ record }">
        <a-switch
          v-model:checked="record.${field.name}"
          :disabled="loading"
        />
      </template>`;
  }
  return '';
}

/**
 * 生成 use{Entity}MasterContext.ts
 * @param {object} ctx
 * @returns {string}
 */
function generateMasterContextComposable(ctx) {
  const { entityPascal, entityKebab, viewEntityKebab, viewModulePath, comment, modulePath } = ctx;
  const entityCamel = pascalToCamel(entityPascal);
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const masterContextFileName = entityMasterContextComposableFileName(entityPascal);
  return `// ========================================
// 项目名称：节拍智控 · TaktWise (TDF)
// 命名空间：@/views/${viewModulePath}/composables
// 文件名称：${masterContextFileName}
// 功能描述：${comment}主表选中行上下文（供右侧明细面板读取）
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// ========================================

import type { InjectionKey, Ref } from 'vue'
import type { ${entityPascal} } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'

/** 表格行类型（与 index 列表行、TaktSingleTable slot record 一致） */
export type ${rowRecordType} = ${entityPascal} | Record<string, unknown>

/** 主表选中行上下文 */
export interface ${entityPascal}MasterContext {
  /** 当前选中的主表行（右侧明细依赖） */
  selecmedMasterRow: Ref<${rowRecordType} | null>
}

const ${entityCamel}MasterContextKey: InjectionKey<${entityPascal}MasterContext> = Symbol('${viewEntityKebab}MasterContext')

/**
 * 在主表页 provide 选中行上下文
 * @returns {${entityPascal}MasterContext} 主表上下文
 */
export function provide${entityPascal}MasterContext(): ${entityPascal}MasterContext {
  const selecmedMasterRow = ref<${rowRecordType} | null>(null)
  const ctx: ${entityPascal}MasterContext = { selecmedMasterRow }
  provide(${entityCamel}MasterContextKey, ctx)
  return ctx
}

/**
 * 在明细面板 inject 主表选中行
 * @returns {${entityPascal}MasterContext} 主表上下文
 */
export function use${entityPascal}MasterContext(): ${entityPascal}MasterContext {
  const ctx = inject(${entityCamel}MasterContextKey)
  if (!ctx) {
    throw new Error('use${entityPascal}MasterContext must be used within ${viewEntityKebab} index')
  }
  return ctx
}
`;
}

/**
 * 生成子表独立 CRUD 弹窗表单（右栏明细 / 面板 Modal）
 * @param {object} ctx
 * @param {object} child
 * @returns {string}
 */
function generateChildDetailFormVue(ctx, child) {
  const { modulePath, viewModulePath, comment } = ctx;
  const viewChildKebab = child.viewChildKebab || child.childKebab;
  const childPascal = child.childPascal;
  const childIdField = child.childIdField;
  const generatorScript = 'generate-vue-master-detail-from-api.cjs';
  // 隔离字段（租户/公司/语言/工厂）只读但仍须展示 + applyScopeDefaults；其它 readOnly 派生字段不入子表单
  const formFields = (child.formFields || []).filter(
    (f) => !f.readOnly || SCOPE_FORM_FIELD_NAMES.includes(f.name),
  );
  const formCodeControlOptions = {
    entityIdField: childIdField,
    colSpanFieldCount: FORM_TAB_FIELDS_PER_TAB,
  };
  const formContentClassExpr = buildFormContentClassComputedExpr();
  const formTemplate = buildGeneratedFormTemplateBody({
    formFields,
    formCodeControlOptions,
    hasMasterDetail: false,
    forceFormTabs: true,
    entityKebab: viewChildKebab,
  });
  const useFormTabs = formTemplate.useFormTabs;
  const formTemplateBody = useFormTabs
    ? formTemplate.body
    : `    <div :class="formContentClass">
${formTemplate.body}
    </div>`;
  const hasScopeContextFields = hasScopeContextFormFields(formFields, []);
  const scopePresence = resolveScopeFormFieldPresence(formFields, []);
  const scopeFragments = buildScopeContextFormScriptFragments(scopePresence, childIdField, childPascal);
  const scopeStoreImports = hasScopeContextFields ? scopeFragments.imports : '';
  const scopeStoreScript = hasScopeContextFields ? scopeFragments.script : '';
  const scopeContextWatch = hasScopeContextFields ? scopeFragments.watch : '';
  const masterTypeImport = `import type { ${childPascal}Create } from '@/types/${modulePath}/${toFrontendEntityFileCamel(child.childKebab)}.types'`;
  const formScriptFragments = buildGeneratedFormVueScriptFragments({
    formFields,
    entityIdField: childIdField,
    childFieldStrip: '',
    hasScopeContextFields,
    watchSyncChild: '',
    useBuildSubmitPayload: false,
  });
  const masterCamel = ctx.entityCamel;
  const masterPascal = ctx.entityPascal;
  const masterFkCamel = String(child.masterFkField || '');
  const fkPrefixCamel = masterFkCamel.replace(/Id$/i, '');
  const altFkPrefixBackfill =
    fkPrefixCamel && fkPrefixCamel !== masterCamel
      ? `    if (masterCode != null && masterCode !== '' && !payload.${fkPrefixCamel}Code) {
      payload.${fkPrefixCamel}Code = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.${fkPrefixCamel}Name) {
      payload.${fkPrefixCamel}Name = masterName
    }
`
      : '';
  const getValuesBody = `${formScriptFragments.getValuesBody.replace(
    '  return payload',
    `  payload.${child.masterFkField} = props.masterId
  // 主表冗余码/名：左侧选中行回填（后端 Stamp 仍按主表 FK 兜底；不限人事）
  const masterRow = props.masterRow as Record<string, unknown> | null | undefined
  if (masterRow) {
    const masterCode = masterRow.${masterCamel}Code ?? masterRow.${masterPascal}Code
    const masterName = masterRow.${masterCamel}Name ?? masterRow.${masterPascal}Name
    if (masterCode != null && masterCode !== '' && !payload.${masterCamel}Code) {
      payload.${masterCamel}Code = masterCode
    }
    if (masterName != null && masterName !== '' && !payload.${masterCamel}Name) {
      payload.${masterCamel}Name = masterName
    }
${altFkPrefixBackfill}    const masterPlant = masterRow.plantCode ?? masterRow.PlantCode
    if (masterPlant != null && masterPlant !== '' && !payload.plantCode) {
      payload.plantCode = masterPlant
    }
    const masterCulture = masterRow.cultureCode ?? masterRow.CultureCode
    if (masterCulture != null && masterCulture !== '' && !payload.cultureCode) {
      payload.cultureCode = masterCulture
    }
  }
  return payload`,
  )}`;
  const resetScopeDefaultsLine = buildFormResetScopeDefaultsBlock(childIdField, hasScopeContextFields);
  // TaktSelect：unplugin-vue-components 自动注册，禁止手写 import（needsTaktSelect 仍用于 popupContainer）
  const needsTaktSelect = formFields.some((f) => f.htmlType === 'select' && f.dictType);
  const extFieldIconImport = buildExtFieldIconImportLine(formFields);
  let formScriptState = buildFormScriptStateBlock({
    formContentClassExpr,
    formFieldsJson: JSON.stringify(formFields.map((f) => f.name)),
    mdScript: '',
    scopeStoreScript: hasScopeContextFields ? scopeStoreScript : '',
    entityPascal: childPascal,
    entityIdField: childIdField,
    useFormTabs,
  });
  formScriptState = formScriptState.replace(
    `  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}`,
    `  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
  /** 主表选中行快照（冗余 {主表}Code/Name、plantCode 等，供 Stamp 前前端回填） */
  masterRow?: Record<string, unknown> | null
}`,
  ).replace(
    `  loading: false,
})`,
    `  loading: false,
  masterId: '',
  masterRow: null,
})`,
  );
  const activeTabReset = useFormTabs ? "  activeTab.value = 'tab-0'\n" : '';
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${child.childPascal}Form.vue -->
<!-- 功能描述：${comment}子表 ${child.childCamel} 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 ${generatorScript} 生成，风格与主表 *Form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ${viewChildKebab}-form flex flex-col min-h-0"
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
 * ${comment}子表 ${child.childCamel} 维护表单 · 由 ${generatorScript} 生成
 * @module views/${viewModulePath}/components
 */
${formScriptFragments.vueImportLine}
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
${buildEntityI18nFormImportBlock(childPascal, viewChildKebab)}
${masterTypeImport}
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

/** 映射为 Create/Update DTO（含主表外键 ${child.masterFkField}） */
function getValues(): Record<string, any> {
${getValuesBody}
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
${resetScopeDefaultsLine}${activeTabReset}  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>
${buildFormTabsScopedStyleBlock(useFormTabs)}
`;
}

/**
 * 子表 buildListQuery（始终带主表外键）
 * @param {string} childPascal
 * @param {string} masterFkField
 * @param {string} entityPascal
 * @param {object[]} queryFields
 * @returns {string}
 */
function buildChildPanelListQueryBlock(childPascal, masterFkField, entityPascal, queryFields) {
  const block = buildServerPagedListQueryBlock(childPascal, queryFields);
  return block.replace(
    '    pageSize: pageSize.value,\n    ...overrides,',
    `    pageSize: pageSize.value,\n    ${masterFkField}: master${entityPascal}Id.value,\n    ...overrides,`,
  );
}

/**
 * 子表右栏面板：按 __table-wrap 实测 scroll.y（右栏含查询/工具栏时不可沿用主表共享高度）
 * @param {boolean} [reserveSummaryRow] 是否预留合计行高度
 * @returns {string}
 */
function buildChildPanelDetailScrollScript(reserveSummaryRow = false) {
  const measureArgs = reserveSummaryRow ? ', { reserveSummaryRow: true }' : '';
  const recalcComment = reserveSummaryRow
    ? '/** 按子表容器重算 scroll.y（扣除表头 + 汇总行，避免合计被裁切或双滚动条） */'
    : '/** 按子表容器重算 scroll.y */';
  return `
/** 子表滚动区容器（扣除查询/工具栏后剩余高度） */
const detailTableWrapRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（按 __table-wrap 实测，避免沿用主表共享高度导致双滚动条） */
const detailTableScrollY = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailTableScrollResizeObserver: ResizeObserver | null = null

${recalcComment}
function recalcDetailTableScrollY(): void {
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollY.value = measureMasterDetailLrTableScrollY(wrap${measureArgs})
}

/** 监听子表容器尺寸变化 */
function startDetailTableScrollObserve(): void {
  stopDetailTableScrollObserve()
  recalcDetailTableScrollY()
  const wrap = detailTableWrapRef.value
  if (!wrap) {
    return
  }
  detailTableScrollResizeObserver = new ResizeObserver(() => {
    recalcDetailTableScrollY()
  })
  detailTableScrollResizeObserver.observe(wrap)
}

/** 停止监听子表容器尺寸 */
function stopDetailTableScrollObserve(): void {
  detailTableScrollResizeObserver?.disconnect()
  detailTableScrollResizeObserver = null
}`;
}

/**
 * 子表右栏 panel 合计行模板与脚本（无数值合计字段时返回空块）
 * @param {object} child
 * @param {string} childI18nPrefix
 * @param {'tenant'|'company'|'approval'} entityScope
 * @returns {{ hasSummary: boolean, templateBlock: string, importLines: string, scriptBlock: string }}
 */
function buildChildPanelSummaryParts(child, childI18nPrefix, entityScope = 'company') {
  const summarySumFields = buildChildPanelSummarySumFieldNames(child.listFields || []);
  if (summarySumFields.length === 0) {
    return { hasSummary: false, templateBlock: '', importLines: '', scriptBlock: '' };
  }
  const idField = child.childIdField;
  const templateBlock = `
        <template #summary>
          <a-table-summary fixed>
            <a-table-summary-row>
              <a-table-summary-cell :index="0" />
              <a-table-summary-cell
                v-for="cell in summaryCells"
                :key="cell.key"
                :index="cell.index"
              >
                <span class="text-sm font-medium">{{ cell.text }}</span>
              </a-table-summary-cell>
            </a-table-summary-row>
          </a-table-summary>
        </template>`;
  const importLines = `import {
  filterMergedColumnsByDefaultVisible,
  filterTableColumnsByVisibleKeys,
  mergeDefaultColumns,
  normalizeUserTableColumns,
} from '@/utils/tableColumns'
import { formatSummaryValue } from '@/components/business/TaktEditableTable/editableTableUtils'`;
  const scriptBlock = `
/** 与 TaktSingleTable 展示列对齐（用于汇总行单元格） */
const resolvedSummaryColumns = computed(() => {
  const userCols = normalizeUserTableColumns(columns.value)
  const merged = mergeDefaultColumns(userCols, t, true, '${entityScope}')
  const keys = visibleColumnKeys.value
  if (keys.length > 0) {
    return filterTableColumnsByVisibleKeys(merged, keys, merged)
  }
  return filterMergedColumnsByDefaultVisible(merged, userCols, {
    idColumnKey: '${idField}',
    actionColumnKey: 'action',
    tableMode: 'masterDetailDetail',
    entityScope: '${entityScope}',
  })
})

const summarySumFieldSet = new Set<string>(${childI18nPrefix}_SUMMARY_SUM_FIELDS)

/** 汇总行首列文案 */
const summaryLabel = computed(() => t('components.business.page.editabletable.summarylabel'))

/** 汇总行单元格（index 与 a-table 列序一致：0=行选择，1..n=展示列） */
const summaryCells = computed(() => {
  const cells: Array<{ key: string; text: string; index: number }> = []
  resolvedSummaryColumns.value.forEach((col, columnIndex) => {
    const key = String(col.key ?? columnIndex)
    let text = ''
    if (columnIndex === 0) {
      text = summaryLabel.value
    } else if (isSummarySumField(key)) {
      text = formatSummaryFieldTotal(key)
    }
    cells.push({
      key,
      text,
      index: columnIndex + 1,
    })
  })
  return cells
})

/** 是否参与当前页合计 */
function isSummarySumField(field: string): boolean {
  return summarySumFieldSet.has(field)
}

/** 当前页 dataSource 各合计列求和 */
const summaryFieldTotals = computed(() => {
  const totals = Object.fromEntries(
    ${childI18nPrefix}_SUMMARY_SUM_FIELDS.map((field) => [field, 0]),
  ) as Record<(typeof ${childI18nPrefix}_SUMMARY_SUM_FIELDS)[number], number>
  for (const row of dataSource.value) {
    for (const field of ${childI18nPrefix}_SUMMARY_SUM_FIELDS) {
      const num = Number(get${child.childPascal}Field(row, field))
      if (Number.isFinite(num)) {
        totals[field] += num
      }
    }
  }
  return totals
})

/** 格式化合计单元格展示值 */
function formatSummaryFieldTotal(field: string): string {
  if (!isSummarySumField(field)) {
    return ''
  }
  return formatSummaryValue(summaryFieldTotals.value[field as keyof typeof summaryFieldTotals.value])
}`;
  return { hasSummary: true, templateBlock, importLines, scriptBlock };
}

/**
 * 生成子表右栏 CRUD 面板（对齐 logistics/serial/inbound inbound-item-panel）
 * @param {object} ctx
 * @param {object} child
 * @returns {string}
 */
function generateChildDetailPanelVue(ctx, child) {
  const {
    entityPascal,
    entityKebab,
    viewEntityKebab,
    modulePath,
    viewModulePath,
    comment,
    caps,
    permissionPrefix,
  } = ctx;
  const viewChildKebab = child.viewChildKebab || child.childKebab;
  const masterPerm = String(permissionPrefix || ctx.permissionPrefix || '').replace(/:list$/, '');
  const childCaps = child.childCaps || detectApiCapabilities(child.childPascal, {});
  const queryFields = child.queryFields || [];
  const queryInit = queryFields.map((f) => {
    const val = f.type === 'number' ? 'undefined as number | undefined' : "''";
    return `  ${f.name}: ${val},`;
  }).join('\n');
  const queryFactoryBlock = buildAdvancedQueryFactoryBlock(child.childPascal, queryFields);
  const childI18nPrefix = entityI18nConstPrefix(child.childPascal);
  const entityScope = ctx.fields?.entityScope || 'company';
  const summaryParts = buildChildPanelSummaryParts(child, childI18nPrefix, entityScope);
  const childEntityI18nImport = buildChildPanelEntityI18nImportBlock(
    child.childPascal,
    viewChildKebab,
    '../composables',
  );
  const queryItems = queryFields.map((f) => renderQueryFormItem(f)).join('\n');
  const queryFieldStorageKey = `takt-query-fields-${viewModulePath.replace(/\//g, '-')}-${viewChildKebab}`;
  const remixIconImport = buildRemixIconImportLine({ includeActionIcons: true, queryFields });
  const listCols = (child.listFields || []).map((f) => `  {
    title: ${fieldLabelTExpr(f)},
    dataIndex: '${f.name}',
    key: '${f.name}',
    width: 120,
    resizable: true,
    ellipsis: true,
    customRender: ({ record }: { record: ${child.childType} }) =>
      String(get${child.childPascal}Field(record, '${f.name}') ?? ''),
  },`).join('\n');
  const importApiImports = [
    childCaps.apiGetTemplate,
    childCaps.apiImport,
    childCaps.apiExport,
  ].filter(Boolean);
  const baseApiImports = [
    childCaps.apiGetList,
    childCaps.apiGetById,
    childCaps.apiCreate,
    childCaps.apiUpdate,
    childCaps.apiDelete,
    childCaps.apiDeleteBatch,
  ].filter(Boolean);
  const apiNames = [...new Set([...baseApiImports, ...importApiImports])];
  if (apiNames.length === 0) {
    console.warn(
      `⚠️  子表 ${child.childPascal} 未解析到 API 导出（请确认 ${modulePath}/${toFrontendEntityFileCamel(child.childKebab)}.api.ts）`,
    );
  }
  const apiImportStatement = apiNames.length
    ? `import {\n  ${apiNames.join(',\n  ')},\n} from '@/api/${modulePath}/${toFrontendEntityFileCamel(child.childKebab)}.api'`
    : '';
  const listQueryBlock = buildChildPanelListQueryBlock(
    child.childPascal,
    child.masterFkField,
    entityPascal,
    queryFields,
  );
  if (!child.masterFkField) {
    console.warn(
      `⚠️  子表 ${child.childPascal} 缺少 masterFkField，右栏 list 无法按主表 Id 过滤`,
    );
  }
  const importHandlers = (childCaps.hasImport && childCaps.hasGetTemplate)
    ? buildImportHandlersScriptBlock({
      apiGetTemplate: childCaps.apiGetTemplate,
      apiImport: childCaps.apiImport,
      openHandlerPrefix: `if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }`,
      successBody: 'void loadData()',
    })
    : '';
  const exportHandler = childCaps.hasExport ? `
async function handleExport() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  try {
    loading.value = true
${buildServerPagedExportApiCall(childCaps.apiExport)}
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = \`\${excelNames.fileBase}_\${ts.getFullYear()}\${pad(ts.getMonth() + 1)}\${pad(ts.getDate())}\${pad(ts.getHours())}\${pad(ts.getMinutes())}\${pad(ts.getSeconds())}\`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as { contentDisposition?: string | null }).contentDisposition ?? null,
      contentType: (exportMeta as { contentType?: string | null }).contentType ?? null,
      fallbackBase,
    })
    const blob = (exportMeta as { blob?: Blob }).blob ?? (exportMeta as Blob)
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
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}` : '';
  const importModalBlock = (childCaps.hasImport && childCaps.hasGetTemplate)
    ? buildImportModalVueBlock(child.childPascal)
    : '';
  const queryDrawerBlock = queryFields.length ? `
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      storage-key="${queryFieldStorageKey}"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
${queryItems}
      </template>
    </TaktQueryDrawer>` : '';
  const toolsBarImportExport = `
      :show-import="${childCaps.hasImport && childCaps.hasGetTemplate}"
      :show-export="${Boolean(childCaps.hasExport)}"
      :show-advanced-query="${queryFields.length > 0}"
      :show-column-setting="true"
      :show-fullscreen="true"
      :import-disabled="!hasMasterSelection"
      :export-disabled="!hasMasterSelection"
      :import-loading="loading"
      :export-loading="loading"
${childCaps.hasImport && childCaps.hasGetTemplate ? '      @import="handleImport"' : ''}
${childCaps.hasExport ? '      @export="handleExport"' : ''}
${queryFields.length ? `      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"` : '      @column-setting="handleColumnSetting"'}`;
  const excelImportLine = (childCaps.hasImport || childCaps.hasExport)
    ? [
      "import { taktExcelEntityNames } from '@/utils/naming'",
      ...(childCaps.hasExport ? ["import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'"] : []),
      ...(childCaps.hasImport && childCaps.hasGetTemplate
        ? ["import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'"]
        : []),
    ].join('\n')
    : '';
  const advancedQueryScript = queryFields.length ? `
const advancedQueryVisible = ref(false)
${queryFactoryBlock}
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
const visibleQueryFieldKeys = ref<string[]>([])

/** 高级查询字段元数据 */
const queryFieldsMeta = computed(() =>
  ${childI18nPrefix}_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)

function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}` : '';
  const columnSettingScript = `
const columnSettingVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([...${childI18nPrefix}_DEFAULT_VISIBLE_COLUMN_KEYS])

function handleColumnSetting() {
  columnSettingVisible.value = true
}

function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

function handleColumnSettingReset() {
  visibleColumnKeys.value = [...${childI18nPrefix}_DEFAULT_VISIBLE_COLUMN_KEYS]
}`;
  return `<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/${viewModulePath}/components -->
<!-- 文件名称：${child.childPascal}Panel.vue -->
<!-- 功能描述：${comment}主表实体右侧明细 ${child.childCamel} 独立 CRUD（按主表选中 ${child.masterFkField} 分页） -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <div class="${viewChildKebab}-panel flex h-full min-h-0 flex-col overflow-hidden">
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleQueryReset"
    />
    <TaktToolsBar
      create-permission="${masterPerm}:create"
      update-permission="${masterPerm}:update"
      delete-permission="${masterPerm}:delete"
${childCaps.hasImport ? `      import-permission="${masterPerm}:import"` : ''}
${childCaps.hasExport ? `      export-permission="${masterPerm}:export"` : ''}
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-expand="false"
      :show-refresh="true"
${toolsBarImportExport}
      :create-disabled="!hasMasterSelection"
      :update-disabled="updateDisabled"
      :delete-disabled="deleteDisabled"
      :create-loading="loading"
      :update-loading="loading"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @refresh="handleRefresh"
    />
    <div
      ref="detailTableWrapRef"
      class="${viewChildKebab}-panel__table-wrap min-h-0 flex-1 overflow-hidden"
    >
      <TaktSingleTable
        class="h-full min-h-0"
        :columns="columns"
        entity-scope="${ctx.fields?.entityScope || 'company'}"
        :data-source="dataSource"
        :loading="loading"
        :stripe="true"
        :virtual="true"
        :row-key="get${child.childPascal}Id"
        :row-selection="rowSelection"
        :custom-row="onClickRow"
        :visible-column-keys="visibleColumnKeys"
        id-column-key="${child.childIdField}"
        :show-pagination="true"
        v-model:current="currentPage"
        v-model:page-size="pageSize"
        :total="total"
        scroll-layout="masterDetailLr"
        table-mode="masterDetailDetail"
        :scroll="{ y: detailTableScrollY }"
        :show-row-selection="true"
        @change="handleTableChange"
        @pagination-change="handleMasterDetailPaginationChange"
        @resize-column="handleResizeColumn"
      >${summaryParts.templateBlock}
      </TaktSingleTable>
    </div>
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      ${TAKT_FORM_MODAL_WIDTH_ATTR}
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <${child.childPascal}Form
        ref="formRef"
        :form-data="formData"
        :master-id="master${entityPascal}Id"
        :master-row="selecmedMasterRow"
        :loading="formLoading"
      />
    </TaktModal>
${queryDrawerBlock}${importModalBlock}
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      id-column-key="${child.childIdField}"
      action-column-key="action"
      entity-scope="${ctx.fields?.entityScope || 'company'}"
      table-mode="masterDetailDetail"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * ${comment}子表 ${child.childCamel} 右栏面板
 * @module views/${viewModulePath}/components
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { measureMasterDetailLrTableScrollY } from '@/composables/useTaktMasterDetailLrScrollY'
${TAKT_FORM_MODAL_WIDTH_IMPORT}import { TAKT_TABLE_SCROLL_Y_MIN } from '@/utils/tableScroll'
import { getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
${excelImportLine ? `${excelImportLine}\n` : ''}${summaryParts.importLines ? `${summaryParts.importLines}\n` : ''}import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
${remixIconImport}import ${child.childPascal}Form from './${child.childPascal}Form.vue'
import { use${entityPascal}MasterContext } from '../composables/${entityMasterContextComposableFileName(entityPascal).replace(/\.ts$/, '')}'
${apiImportStatement}
import type { ${child.childType}, ${child.childPascal}Query } from '@/types/${modulePath}/${toFrontendEntityFileCamel(child.childKebab)}.types'

${childEntityI18nImport}
const { t } = useI18n()
const { selecmedMasterRow } = use${entityPascal}MasterContext()

${childCaps.hasImport || childCaps.hasExport ? `/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('${child.childEntityClassName || `Takt${child.childPascal}`}')
` : ''}/** 快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() }),
)

const loading = ref(false)
${buildChildPanelDetailScrollScript(summaryParts.hasSummary)}
const dataSource = ref<${child.childType}[]>([])
const currentPage = ref(getTaktDefaultPageIndex())
const pageSize = ref(getTaktDefaultPageSize())
const total = ref(0)
const queryKeyword = ref('')
const selecmedRow = ref<${child.childType} | null>(null)
const selecmedRows = ref<${child.childType}[]>([])
const selecmedRowKeys = ref<(string | number)[]>([])
const formVisible = ref(false)
const formTitle = ref('')
const formData = ref<Partial<${child.childType}>>({})
const formLoading = ref(false)
const formRef = ref()
${buildFormModalWidthStateBlock()}
${advancedQueryScript}${columnSettingScript}
${childCaps.hasImport && childCaps.hasGetTemplate ? 'const importVisible = ref(false)\n' : ''}
const entityIdName = '${child.childIdField}'
const master${entityPascal}Id = computed((): string => {
  const id = (selecmedMasterRow.value as Record<string, unknown> | null)?.['${caps.entityIdName}']
  return id != null ? String(id) : ''
})
const hasMasterSelection = computed(() => master${entityPascal}Id.value !== '')
const updateDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length !== 1)
const deleteDisabled = computed(() => !hasMasterSelection.value || selecmedRows.value.length === 0)

function get${child.childPascal}Id(record: ${child.childType} | Record<string, unknown>): string {
  return String((record as ${child.childType})?.[entityIdName] ?? '')
}

function get${child.childPascal}Field(record: ${child.childType} | Record<string, unknown>, field: string): unknown {
  return (record as ${child.childType})?.[field as keyof ${child.childType}]
}

const columns = computed<TableColumnsType>(() => [
  {
    title: t('common.page.entity.id'),
    dataIndex: '${child.childIdField}',
    key: '${child.childIdField}',
    width: 80,
    resizable: true,
    ellipsis: true,
    fixed: 'left',
    customRender: ({ record }: { record: ${child.childType} }) =>
      String(get${child.childPascal}Field(record, '${child.childIdField}') ?? ''),
  },
${listCols}
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: '${masterPerm}:update',
        onClick: (record: ${child.childType}) => void handleEdit(record),
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: '${masterPerm}:delete',
        onClick: (record: ${child.childType}) => void handleDeleteOne(record),
      },
    ],
  }),
])
${summaryParts.scriptBlock}
const rowSelection = computed(() => ({
  selecmedRowKeys: selecmedRowKeys.value,
  onChange: (keys: (string | number)[], rows: ${child.childType}[]) => {
    selecmedRowKeys.value = keys
    selecmedRows.value = rows
    selecmedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: ${child.childType}, selecmed: boolean) => {
    if (selecmed) {
      selecmedRow.value = record
    } else if (selecmedRow.value && get${child.childPascal}Id(selecmedRow.value) === get${child.childPascal}Id(record)) {
      selecmedRow.value = null
    }
  },
  onSelectAll: (selecmed: boolean, selecmedRowsData: ${child.childType}[]) => {
    selecmedRow.value = selecmed && selecmedRowsData.length === 1 ? (selecmedRowsData[0] ?? null) : null
  },
}))

/**
 * 行点击选中（与左主表 masterCustomRow 一致，联动 rowSelection）
 * @param record 行数据
 */
function onClickRow(record: ${child.childType}) {
  const key = get${child.childPascal}Id(record)
  return {
    onClick: () => {
      selecmedRowKeys.value = [key]
      selecmedRows.value = [record]
      selecmedRow.value = record
    },
    class: selecmedRowKeys.value.includes(key)
      ? 'takt-master-detail-table-row-selecmed cursor-pointer'
      : 'cursor-pointer',
  }
}
${listQueryBlock}
async function loadData() {
  if (!hasMasterSelection.value) {
    dataSource.value = []
    total.value = 0
    selecmedRowKeys.value = []
    selecmedRows.value = []
    selecmedRow.value = null
    return
  }
  loading.value = true
  try {
    const res = await ${childCaps.apiGetList}(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: unknown) {
    const err = error as { message?: string }
    message.error(err?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

function reload() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

/** 主表选中变更时自动加载子表 */
watch(master${entityPascal}Id, () => {
  reload()
})

/** 租户/公司切换时刷新子表 */
useTableRefresh(loadData)

onMounted(() => {
  startDetailTableScrollObserve()
})

onBeforeUnmount(() => {
  stopDetailTableScrollObserve()
})

watch(
  () => loading.value,
  (isLoading) => {
    if (!isLoading) {
      void nextTick(() => recalcDetailTableScrollY())
    }
  },
)

watch(
  () => [dataSource.value.length, visibleColumnKeys.value.join(',')],
  () => {
    void nextTick(() => recalcDetailTableScrollY())
  },
)

watch(hasMasterSelection, (selecmed) => {
  if (selecmed) {
    void nextTick(() => startDetailTableScrollObserve())
  }
})

function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleQueryReset() {
  queryKeyword.value = ''
  currentPage.value = getTaktDefaultPageIndex()
  void loadData()
}

function handleCreate() {
  if (!hasMasterSelection.value) {
    message.warning(t('common.status.empty'))
    return
  }
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = {}
  formVisible.value = true
}

async function handleEdit(record: ${child.childType}) {
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await ${childCaps.apiGetById}(get${child.childPascal}Id(record))
    formData.value = detail ? { ...detail } : { ...record }
    formVisible.value = true
  } finally {
    formLoading.value = false
  }
}

function handleUpdate() {
  if (selecmedRow.value) {
    void handleEdit(selecmedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.edit'),
      entity: pi.self(),
    }))
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
    const payload = refInst.getValues?.()
    const id = formData.value?.${child.childIdField}
    if (id) {
      await ${childCaps.apiUpdate}(id, payload)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await ${childCaps.apiCreate}(payload)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    await loadData()
  } finally {
    formLoading.value = false
  }
}

function handleFormCancel() {
  formVisible.value = false
}

async function handleDeleteOne(record: ${child.childType}) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', {
      entity: pi.self(),
      name: t('common.tip.this.target', { target: pi.self() }),
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await ${childCaps.apiDelete}(get${child.childPascal}Id(record))
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

async function handleDelete() {
  if (!hasMasterSelection.value || selecmedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', {
      action: t('common.page.button.delete'),
      entity: pi.self(),
    }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', {
      entity: pi.self(),
      count: selecmedRows.value.length,
    }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selecmedRows.value.map((r) => get${child.childPascal}Id(r)).filter(Boolean)
      await ${childCaps.apiDeleteBatch}(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      await loadData()
    },
  })
}

function handleRefresh() {
  void loadData()
}
${importHandlers}${exportHandler}
function handleTableChange() {}

function handleResizeColumn() {}

/**
 * 主子表内嵌分页变更
 * @param page 页码
 * @param size 每页条数
 */
function handleMasterDetailPaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  void loadData()
}

defineExpose({ reload, loadData })
</script>
`;
}

/**
 * index.vue 左右主子表 script 片段
 * @param {object} ctx
 * @returns {object}
 */
function generateMasterDetailLrIndexScript(ctx) {
  const { entityPascal, caps, fields } = ctx;
  const rowRecordType = entityRowRecordTypeName(entityPascal);
  const children = fields.masterDetailChildren || [];
  const panelRefs = children.map((c) => `const ${c.childCamel}PanelRef = ref<InstanceType<typeof ${c.childPascal}Panel> | null>(null)`).join('\n');
  const panelImports = children.map((c) => {
    const viewChildKebab = c.viewChildKebab || c.childKebab;
    return `import ${c.childPascal}Panel from './components/${c.childPascal}Panel.vue'`;
  }).join('\n');
  const reloadPanels = children.map((c) => `  ${c.childCamel}PanelRef.value?.reload?.()`).join('\n');
  const detailSlot = children.length === 1
    ? `      <template #detail>
        <${children[0].childPascal}Panel
          ref="${children[0].childCamel}PanelRef"
          class="h-full min-h-0 flex-1"
        />
      </template>`
    : `      <template #detail>
        <div class="flex min-h-0 flex-1 flex-col gap-3">
${children.map((c) => `          <${c.childPascal}Panel ref="${c.childCamel}PanelRef" class="h-full min-h-0 flex-1" />`).join('\n')}
        </div>
      </template>`;
  const detailFn = caps.hasGetById ? `
/** 加载主表详情并回填当前页 dataSource */
async function load${entityPascal}Detail(record: ${rowRecordType}): Promise<${entityPascal} | null> {
  const id = get${entityPascal}Id(record)
  if (!id) {
    return null
  }
  try {
    const detail = await ${caps.apiGetById}(id)
    const index = dataSource.value.findIndex((row) => get${entityPascal}Id(row) === id)
    if (index !== -1) {
      dataSource.value[index] = { ...dataSource.value[index], ...detail } as ${entityPascal}
    }
    return detail
  } catch (error: any) {
    message.error(error?.message || t('common.feedback.load.data.failed'))
    return null
  }
}` : '';
  return {
    panelImports,
    composableImport: `import { provide${entityPascal}MasterContext, type ${rowRecordType} } from './composables/${entityMasterContextComposableFileName(ctx.entityPascal).replace(/\.ts$/, '')}'`,
    composableSetup: `/** 主表选中行上下文（右侧明细面板读取） */
const { selecmedMasterRow } = provide${entityPascal}MasterContext()`,
    panelRefs,
    detailSlot,
    detailFn,
    lrScript: `
/** 主表行点击选中 key（左右主子表高亮） */
const selecmedMasterKey = ref('')

/** 同步主表选中行到右侧明细（子表由 *-panel watch 自动 reload） */
function syncMasterSelection(record: ${rowRecordType} | null) {
  selecmedMasterRow.value = record
  selecmedMasterKey.value = record ? get${entityPascal}Id(record) : ''
}

/**
 * 左右主子表：主表行选中
 * @param record 主表行
 */
function handleMasterSelect(record: Record<string, unknown>) {
  const row = record as unknown as ${rowRecordType}
  const key = get${entityPascal}Id(row)
  selecmedRowKeys.value = [key]
  selecmedRows.value = [row]
  selecmedRow.value = row
  syncMasterSelection(row)
}

/**
 * 主表分页变更（v-model 已同步页码与 pageSize）
 * @param _page 页码
 * @param _pageSize 每页条数
 */
function handleMasterPaginationChange(_page: number, _pageSize: number) {
  loadData()
}
${detailFn}`,
    rowSelectionPatch: `    if (rows.length === 1 && rows[0]) {
      syncMasterSelection(rows[0])
    } else if (rows.length === 0) {
      syncMasterSelection(null)
    }`,
    deleteClearSelection: `      selecmedRowKeys.value = []
      selecmedRows.value = []
      selecmedRow.value = null
      syncMasterSelection(null)`,
    formSubmitReload: children.length ? `
    if (selecmedMasterKey.value) {
${reloadPanels}
    }` : '',
  };
}

/**
 * 主子表弹窗表单：上主下从各占 body 约 1/2 的样式块
 * @param {string} viewEntityKebab 视图实体 kebab
 * @returns {string} scoped style
 */
function buildMasterDetailFormHalfPaneStyleBlock(viewEntityKebab) {
  const masterClass = `${viewEntityKebab}-form__master`;
  const tabsClass = `${viewEntityKebab}-form-tabs`;
  return `
<style scoped lang="css">
/* 上主下从各占弹窗 body 约 1/2；主表区内部滚动，子表用 scroll.y */
.${masterClass} {
  display: flex;
  flex-direction: column;
  min-height: 0;
}

/* 无 Tabs 时主表半区直接滚动 */
.${masterClass} > div {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.${masterClass} :deep(.${tabsClass}.ant-tabs) {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.${masterClass} :deep(.ant-tabs-nav) {
  flex-shrink: 0;
  margin-bottom: 8px;
}

.${masterClass} :deep(.ant-tabs-content-holder) {
  flex: 1 1 auto;
  min-height: 0;
  overflow: auto;
}

.${masterClass} :deep(.ant-tabs-content),
.${masterClass} :deep(.ant-tabs-tabpane) {
  height: 100%;
}
</style>`;
}

/**
 * 主子表弹窗表单：子表半区 scroll.y 测量脚本
 * @returns {string}
 */
function buildMasterDetailFormHalfPaneScrollScript() {
  return `
import {
  TAKT_TABLE_HEADER_FALLBACK_PX,
  TAKT_TABLE_SCROLL_Y_MIN,
  TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
} from '@/utils/tableScroll'

/** 子表半区宿主（弹窗视口约 1/2） */
const detailHostRef = ref<HTMLElement | null>(null)
/** 子表 scroll.y（半区内扣除标题/表头/汇总） */
const detailScrollYPx = ref(TAKT_TABLE_SCROLL_Y_MIN)
let detailHostResizeObserver: ResizeObserver | null = null

/**
 * 按子表半区实测 scroll.y
 */
function recalcDetailScrollYPx(): void {
  const host = detailHostRef.value
  if (host == null || host.clientHeight <= 0) {
    return
  }
  const tableRoot = host.querySelector('.takt-editable-table') as HTMLElement | null
  const toolbar = tableRoot?.querySelector(':scope > .mb-2') as HTMLElement | null
  const toolbarH = toolbar?.offsetHeight ?? 0
  const sectionPad = 12
  const next = Math.floor(
    host.clientHeight
      - toolbarH
      - sectionPad
      - TAKT_TABLE_HEADER_FALLBACK_PX
      - TAKT_TABLE_SUMMARY_ROW_HEIGHT_PX,
  )
  detailScrollYPx.value = Math.max(TAKT_TABLE_SCROLL_Y_MIN, next)
}

/** 监听弹窗/半区尺寸变化 */
function bindDetailHostResizeObserver(): void {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  const host = detailHostRef.value
  if (host == null || typeof ResizeObserver === 'undefined') {
    return
  }
  const target =
    (host.closest('.ant-modal-content') as HTMLElement | null)
    ?? (host.closest('.ant-modal-body') as HTMLElement | null)
    ?? host
  detailHostResizeObserver = new ResizeObserver(() => {
    recalcDetailScrollYPx()
  })
  detailHostResizeObserver.observe(target)
  detailHostResizeObserver.observe(host)
}

onMounted(() => {
  void nextTick(() => {
    recalcDetailScrollYPx()
    bindDetailHostResizeObserver()
  })
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', recalcDetailScrollYPx)
  }
})

onBeforeUnmount(() => {
  detailHostResizeObserver?.disconnect()
  detailHostResizeObserver = null
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', recalcDetailScrollYPx)
  }
})
`;
}

/**
 * *Form.vue 上主下从（TaktEditableTable）片段
 * @param {object} ctx
 * @returns {{ editableBlocks: string, script: string, tableRefs: string, validateLines: string, resetLines: string }}
 */
function generateMasterDetailEditableFormParts(ctx) {
  const { entityPascal, entityCamel, fields, caps } = ctx;
  const children = fields.masterDetailChildren || [];
  if (!children.length) {
    return { editableBlocks: '', script: '', tableRefs: '', validateLines: '', resetLines: '', needsTaktSelect: false };
  }
  const childI18nImportLines = children.map((child) => {
    const viewChildKebab = child.viewChildKebab || child.childKebab;
    const hookName = entityI18nHookName(child.childPascal);
    const composableStem = entityI18nComposableFileName(viewChildKebab).replace(/\.ts$/, '');
    return `import { ${hookName} } from '../composables/${composableStem}'`;
  }).join('\n');
  const childI18nSetupLines = children.map((child) => {
    const hookName = entityI18nHookName(child.childPascal);
    return `const ${child.childCamel}Pi = ${hookName}()`;
  }).join('\n');
  let needsTaktSelect = false;
  const editableBlocks = children.map((child) => {
    const editableFields = child.formFields.filter((f) => !f.readOnly || f.name === 'lineNumber');
    const slotMarkups = editableFields
      .filter((f) => isEditableTableSlotField(f))
      .map((f) => buildEditableTableCellSlotMarkup(f, child.childCamel))
      .filter(Boolean)
      .join('\n');
    if (slotMarkups) {
      needsTaktSelect = true;
    }
    const slotBlock = slotMarkups ? `\n${slotMarkups}\n` : '';
    return `    <TaktEditableTable
      ref="${child.childCamel}TableRef"
      v-model="child${child.childPascal}Rows"
      :columns="${child.childCamel}FormColumns"
      :title="${child.childCamel}Pi.self()"
      :add-button-entity="${child.childCamel}Pi.self()"
      id-field="${child.childIdField}"
      :default-row="createDefault${child.childPascal}Row"
      :disabled="loading"
      :enable-vertical-scroll="true"
      :virtual="false"
      :scroll="{ y: detailScrollYPx }"
      section-border
      class="w-full min-h-0 min-w-0 flex-1"
    >${slotBlock}    </TaktEditableTable>`;
  }).join('\n');
  const columnDefs = children.map((child) => {
    const cols = child.formFields
      .filter((f) => !f.readOnly || f.name === 'lineNumber')
      .map((f) => mapFormFieldToEditableColumn(f, `${child.childCamel}Pi`))
      .join(',\n');
    return `/** 子表 ${child.childCamel} 可编辑列 */
const ${child.childCamel}FormColumns = computed<TaktEditableTableColumn[]>(() => [
${cols},
])`;
  }).join('\n\n');
  const lineNumberParts = children.map((child) => buildChildLineNumberScriptParts(child));
  const lineNumberImportLines = [...new Set(lineNumberParts.map((p) => p.importLine).filter(Boolean))].join('\n');
  const lineNumberRefLines = lineNumberParts.map((p) => p.refLine).filter(Boolean).join('\n');
  const lineNumberHelperBlock = lineNumberParts.map((p) => p.helperBlock).filter(Boolean).join('\n\n');
  const rowRefs = children.map((child) => `const child${child.childPascal}Rows = ref<Record<string, unknown>[]>([])`).join('\n');
  const tableRefs = children.map((child) => `const ${child.childCamel}TableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)`).join('\n');
  const syncFromForm = children.map((child, index) => {
    const parts = lineNumberParts[index];
    const rowsVar = `rows_${child.childCamel}`;
    let block = `  const ${rowsVar} = ((val as any)?.${child.fieldName} ?? []) as Record<string, unknown>[]`;
    if (parts.syncLines) {
      block += `\n${parts.syncLines}`;
    }
    block += `\n  child${child.childPascal}Rows.value = ${rowsVar}`;
    if (parts.syncAfterRows) {
      block += `\n${parts.syncAfterRows.replace('rows)', `${rowsVar})`)}`;
    }
    return block;
  }).join('\n');
  const defaultRows = children.map((child) => {
    const defaults = child.formFields
      .filter((f) => !f.readOnly)
      .map((f) => buildEditableDefaultRowFieldLine(f, child.childPascal, child))
      .join('\n');
    return `function createDefault${child.childPascal}Row(): Record<string, unknown> {
  return {
${defaults}
  }
}`;
  }).join('\n\n');
  const getValuesMerge = children.map((child, index) => {
    const parts = lineNumberParts[index];
    const itemRowsExpr = `${child.childCamel}TableRef.value?.getRows?.() ?? child${child.childPascal}Rows.value`;
    if (parts.submitMapExtra) {
      return `    ${child.fieldName}: ${itemRowsExpr}.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
        plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
        // 新增态外键须为 0；空串会导致 long 绑定 ModelState 400
        ${child.masterFkField}: isUpdate ? masterId : 0,
      }
${parts.submitMapExtra}
      return normalized
    }),`;
    }
    return `    ${child.fieldName}: ${itemRowsExpr}.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      cultureCode: userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? '',
      plantCode: String(formState.plantCode ?? '').trim() || tenantStore.currentCompanyRelatedPlant || userStore.userInfo?.relatedPlant || '',
      ${child.masterFkField}: isUpdate ? masterId : 0,
    })),`;
  }).join('\n');
  const validateLines = children.map((child) => `  await ${child.childCamel}TableRef.value?.validate?.()`).join('\n');
  const resetLines = children.map((child, index) => {
    const parts = lineNumberParts[index];
    const lines = [`  ${child.childCamel}TableRef.value?.resetRows?.()`];
    if (parts.resetLine) {
      lines.push(parts.resetLine);
    }
    return lines.join('\n');
  }).join('\n');
  const popupContainerFn = needsTaktSelect ? `
/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}
` : '';
  const script = `
import type { TaktEditableTableColumn } from '@/components/business/TaktEditableTable/types'
${lineNumberImportLines ? `${lineNumberImportLines}\n` : ''}${childI18nImportLines}

${childI18nSetupLines}
${popupContainerFn}
${rowRefs}
${lineNumberRefLines ? `${lineNumberRefLines}\n` : ''}${tableRefs}

${lineNumberHelperBlock ? `${lineNumberHelperBlock}\n\n` : ''}${columnDefs}

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<${entityPascal}Create & { ${entityCamel}Id?: string }> | null | undefined) {
${syncFromForm}
}

${defaultRows}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.${caps.entityIdName} ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
${getValuesMerge}
  }
}`;
  return { editableBlocks, script, tableRefs, validateLines, resetLines, needsTaktSelect };
}

/**
 * 写入 composable / 子表面板 / 子表 form
 * @param {object} bundle
 * @param {object} ctx
 * @param {object} options
 */
function writeMasterDetailLayoutOutputs(bundle, ctx, options) {
  const viewDir = path.dirname(bundle.indexPath);
  const composableDir = path.join(viewDir, 'composables');
  const componentsDir = path.join(viewDir, 'components');
  const composablePath = path.join(composableDir, entityMasterContextComposableFileName(ctx.entityPascal));
  const composableContent = generateMasterContextComposable(ctx);
  if (options.dryRun) {
    console.log(`🔍 [dry-run] 将生成: ${composablePath}`);
  } else {
    writeGeneratedFile(composablePath, composableContent);
    console.log(`✅ 已生成: ${composablePath}`);
  }
  (ctx.fields.masterDetailChildren || []).forEach((child) => {
    const viewChildKebab = child.viewChildKebab || child.childKebab;
    const panelPath = path.join(componentsDir, `${child.childPascal}Panel.vue`);
    const childFormPath = path.join(componentsDir, `${child.childPascal}Form.vue`);
    const childI18nPath = path.join(composableDir, entityI18nComposableFileName(viewChildKebab));
    const childListCols = (child.listFields || []).filter((f) => f.name !== child.childIdField);
    const childI18nContent = buildEntityI18nComposableFile({
      entityPascal: child.childPascal,
      entityI18nSlug: child.childI18nSlug,
      entityKebab: child.childKebab,
      viewModulePath: ctx.viewModulePath,
      viewEntityKebab: viewChildKebab,
      modulePath: ctx.modulePath,
      listFields: childListCols,
      formFields: child.formFields || [],
      queryFields: child.queryFields || [],
      comment: child.childPascal,
      includeChildPanelTableKeys: true,
      entityIdField: child.childIdField,
      entityScope: ctx.fields?.entityScope || 'company',
    });
    const panelContent = generateChildDetailPanelVue(ctx, child);
    const childFormContent = generateChildDetailFormVue(ctx, child);
    if (options.dryRun) {
      console.log(`🔍 [dry-run] 将生成:\n  - ${childI18nPath}\n  - ${panelPath}\n  - ${childFormPath}`);
      return;
    }
    writeGeneratedFile(childI18nPath, childI18nContent);
    console.log(`✅ 已生成: ${childI18nPath}`);
    writeGeneratedFile(panelPath, panelContent);
    writeGeneratedFile(childFormPath, childFormContent);
    console.log(`✅ 已生成: ${panelPath}`);
    console.log(`✅ 已生成: ${childFormPath}`);
  });
}

function buildMasterDetailIndexStyleBlock() {
  return '';
}

module.exports = {
  generateMasterDetailLrIndexScript,
  generateMasterDetailEditableFormParts,
  writeMasterDetailLayoutOutputs,
  buildMasterDetailIndexStyleBlock,
  buildMasterDetailFormHalfPaneStyleBlock,
  buildMasterDetailFormHalfPaneScrollScript,
  buildChildPanelDetailScrollScript,
  generateChildDetailPanelVue,
  generateChildDetailFormVue,
};

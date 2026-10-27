// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-vue-common.cjs
// 创建时间：2026-06-08
// 创建人：Takt365(Cursor AI)
// 功能描述：Vue 三脚本共用基础设施（CLI + API/types/字段解析）；查询栏关键字 flex 约定见文件内注释；不含 index/form 模板
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

// @ts-nocheck

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  getControllerClassName,
  resolveEntityScope,
  findDomainEntityFile,
  logGeneratedFileWritePolicy,
  parseSingleEntityGenerateArgsFromArgv,
  entityClassToSlug,
  buildEntityI18nKey,
  stripModulePrefixFromEntityKebab,
  resolveFrontendModuleFileName,
  resolveViewModulePath,
  resolveFrontendOutputRelPath,
  isModuleLeafSameAsEntityKebab,
  isTaktNumberingAutoFormFieldSummary,
  toFrontendApiFileName,
  toFrontendTypesFileName,
  toFrontendApiImportPath,
  toFrontendTypesImportPath,
  toFrontendEntityFileCamel,
  parseFrontendTypesFileCamel,
  parseFrontendApiFileCamel,
  camelToKebab,
  kebabToCamel,
} = require('./generate-script-common.cjs');
const {
  shouldExcludeDtoSourceBase,
  shouldExcludeVueGeneration,
  isStandaloneChildVueEntity,
  assertNotManualDtoEntityCli,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
} = require('./generate-entity-exclusions.cjs');

/** Vue 生成模板类型 */
const VUE_TEMPLATE = {
  CRUD: 'crud',
  TREE: 'tree',
  MASTER_DETAIL: 'master-detail',
};

/**
 * 查询栏关键字宽度约定（组件 CSS，非生成属性）：
 * 输入框 flex:1 = 所在左/右表栏宽 −「查询」「重置」按钮（及 gap）；
 * 树左查询栏无按钮时占满左表栏宽。见 TaktQueryBar / TaktTree*QueryBar。
 */

const CONFIG = {
  frontendRoot: path.resolve(__dirname, '../../frontend'),
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  apiDir: 'src/api',
  typesDir: 'src/types',
  viewsDir: 'src/views',
};

/** 生成表单 a-input 默认最大长度（与 user-form 短文本字段一致） */
const DEFAULT_A_INPUT_MAX_LENGTH = 20;

/** 备注 / 扩展字段 JSON textarea 固定 UI（全模块一致） */
const REMARK_TEXTAREA_ROWS = 4;
const REMARK_TEXTAREA_MAX_LENGTH = 400;
/** extField（前端 camelCase，对应后端 ExtField）标签 tooltip 与输入占位 */
const EXT_FIELD_FORM_NAME = 'extField';
const EXT_FIELD_HINT_I18N_KEY = 'common.page.entity.extfieldhint';
const EXT_FIELD_PLACEHOLDER_I18N_KEY = 'common.page.form.placeholder.extfield';

/**
 * 模板类型中文标签
 * @param {string} templateType
 */
function templateTypeLabel(templateType) {
  if (templateType === VUE_TEMPLATE.TREE) {
    return '树表 TREE';
  }
  if (templateType === VUE_TEMPLATE.MASTER_DETAIL) {
    return '主子表 Master-Detail';
  }
  return '单表 CRUD';
}

/**
 * PascalCase → kebab-case
 * @param {string} str
 */
function pascalToKebab(str) {
  return str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * 统一模块目录路径（正斜杠 + 小写）
 * 仅用于目录段（如 foundation、routine/announcement）；❌ 禁止传入含实体文件主名的路径，
 * 否则 systemProperty 会被压成 systemproperty，Vue import 对不上 systemProperty.types.ts
 * @param {string} modulePath
 * @returns {string}
 */
function normalizeModulePath(modulePath) {
  if (!modulePath) {
    return '';
  }
  return modulePath.replace(/\\/g, '/').toLowerCase();
}

/**
 * @param {string|null} entityPrefix
 * @returns {string[]}
 */
function collectApiFiles(entityPrefix) {
  const root = path.join(CONFIG.frontendRoot, CONFIG.apiDir);
  if (entityPrefix) {
    const { resolveApiFilePathForEntity } = require('./generate-master-detail-associations.cjs');
    const resolved = resolveApiFilePathForEntity(entityPrefix);
    if (resolved) {
      return [resolved];
    }
  }
  /** @type {string[]} */
  const files = [];
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.endsWith('.api.ts')) {
        const relFromApi = path.relative(root, full).replace(/\\/g, '/');
        if (entityPrefix) {
          const modulePath = path.dirname(relFromApi);
          const rawKebab = pascalToKebab(entityPrefix);
          const shortKebab = resolveFrontendModuleFileName(rawKebab, modulePath);
          const baseName = entry.name;
          const wantRaw = toFrontendApiFileName(rawKebab);
          const wantShort = toFrontendApiFileName(shortKebab);
          const dir = path.dirname(full);
          const canonicalPath = path.join(dir, wantRaw);
          if (shortKebab !== rawKebab && baseName === wantShort && fs.existsSync(canonicalPath)) {
            continue;
          }
          if (baseName !== wantRaw && baseName !== wantShort) {
            continue;
          }
        }
        files.push(full);
      }
    }
  }
  scan(root);
  return files;
}

/**
 * kebab 文件名 → PascalCase（与 generate-vue-crud-from-api 一致）
 * @param {string} str
 */
function kebabToPascal(str) {
  return str
    .split('-')
    .filter(Boolean)
    .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
    .join('');
}

/**
 * 解析 CLI 参数（单实体，禁止 --all）
 * @param {() => void} printUsage
 * @returns {{ entityPrefix: string, force: boolean, dryRun: boolean, viewPath: string|null }}
 */
function parseVueCliArgs(printUsage) {
  return parseSingleEntityGenerateArgsFromArgv(process.argv.slice(2), printUsage, { allowViewPath: true });
}

/**
 * 运行 Vue 生成 CLI
 * @param {object} opts
 * @param {string} opts.banner 启动横幅
 * @param {() => void} opts.printUsage
 * @param {string} opts.templateType VUE_TEMPLATE.*
 * @param {(apiFilePath: string, options: object, registry: Map) => object} opts.processModule
 * @param {() => Map} opts.buildRegistry
 * @param {() => void} [opts.onInit]
 */
function runVueGeneratorCli(opts) {
  console.log(opts.banner);
  logGeneratedFileWritePolicy();
  if (opts.onInit) {
    opts.onInit();
  }
  const registry = opts.buildRegistry();
  const options = parseVueCliArgs(opts.printUsage);
  assertNotManualDtoEntityCli(options.entityPrefix);
  const apiFiles = collectApiFiles(options.entityPrefix);
  if (apiFiles.length === 0) {
    console.error('❌ 未找到匹配的 API 文件');
    process.exit(1);
  }
  let created = 0;
  let skipped = 0;
  apiFiles.forEach((file) => {
    const result = opts.processModule(file, options, registry);
    if (result.skipped) {
      skipped += 1;
    } else {
      created += 1;
    }
  });
  console.log(`\n✨ 完成（${templateTypeLabel(opts.templateType)}）：生成 ${created} 个模块，跳过 ${skipped} 个`);
}


const SKIP_LIST_FIELDS = new Set([
  'tenantCode',
  'companyCode',
  'cultureCode',
  'createdAt',
  'updatedAt',
  'createdBy',
  'updatedBy',
  'isDeleted',
  'extField',
  'orderNum',
  'sortOrder',
]);

const SKIP_FORM_FIELDS = new Set([
  'createdAt',
  'updatedAt',
  'createdBy',
  'updatedBy',
  'isDeleted',
  /** 排序号由后端 ITaktSortOrderGenerator 自动生成；专用 SortDto / 树拖拽改序，禁止入维护表单与子表行内编辑 */
  'sortOrder',
  /** RBAC 反向合并（rbac-parent-config RBAC_INVERSE_CREATE_FIELDS）；走 assign-* 弹窗，不入 CRUD 表单 */
  'roleIds',
  'userIds',
  'employeeIds',
]);

/**
 * CreateDto 上下文隔离字段（与 generate-dtos-from-entity 固定字段对齐）
 * 表单只读自动注入 + applyScopeDefaults（工厂=当前公司 RelatedPlant）
 * Tenant / Company / Plant 实体本身除外（见 SCOPE_FIELD_EDITABLE_ENTITIES）
 */
const SCOPE_FORM_FIELD_NAMES = [
  'tenantCode',
  'companyCode',
  'cultureCode',
  'plantCode',
  'relatedPlant',
];

/** 与 SCOPE_FORM_FIELD_NAMES 同集：用于检测是否注入 applyScopeDefaults */
const SCOPE_CONTEXT_DETECT_NAMES = SCOPE_FORM_FIELD_NAMES;

/** 租户/公司/工厂主档实体本身：表单中隔离字段可编辑 */
const SCOPE_FIELD_EDITABLE_ENTITIES = new Set(['Tenant', 'Company', 'Plant']);

const SKIP_QUERY_FIELDS = new Set([
  'pageIndex',
  'pageSize',
  'keyWords',
  'KeyWords',
  'tenantCode',
  'companyCode',
  'sortOrder',
  /** 工厂由仓储按当前公司 RelatedPlant / PlantCode 注入并过滤，查询栏不再重复 */
  'plantCode',
  'relatedPlant',
]);

/** 多行文本字段名片段（不含 content：xxx内容走富文本，见 isRichTextContentField） */
const TEXTAREA_NAME_HINTS = ['remark', 'extfield', 'quote', 'description', 'note', 'greeting', 'address', 'scope'];

/** 非正文 Content 字段：类型/长度/哈希、目录、流程定义 JSON 快照 */
const RICH_TEXT_CONTENT_NAME_EXCLUDE = /^(contentType|contentLength|contentHash|tableOfContents)$/i;

/**
 * 表单 Tabs 分页标准：每 Tab 最多字段数（满 Tab 时 2 列 × 5 行 = 10 项；不足 10 项时单列）
 * 必须与 frontend/src/utils/taktFormTabs.ts → TAKT_FORM_TAB_FIELDS_PER_TAB / TaktFormTabs 对齐
 */
const FORM_TAB_FIELDS_PER_TAB = 10;

/**
 * 始终置于第一个 Tab 开头的字段（顺序固定：工厂 → 区域文化，再接业务字段）
 * plantCode（公司/审批）与 relatedPlant（租户）通常二选一；cultureCode 紧随其后
 */
const FORM_TAB_LEADING_FIELD_NAMES = [
  'plantCode',
  'relatedPlant',
  'cultureCode',
];

const FORM_TAB_LEADING_FIELD_NAME_SET = new Set(FORM_TAB_LEADING_FIELD_NAMES);

/** 始终置于最后一个 Tab 的字段（顺序固定：租户/公司隔离 + 扩展 + 备注） */
const FORM_TAB_TRAILING_FIELD_NAMES = [
  'tenantCode',
  'companyCode',
  'extField',
  'remark',
];

const FORM_TAB_TRAILING_FIELD_NAME_SET = new Set(FORM_TAB_TRAILING_FIELD_NAMES);

/**
 * 解析表单字段 a-col span：Tab 内 < 10 项时整行单列；满 Tab（≥10）时 textarea 24、其余 12
 * @param {{ htmlType?: string }} field
 * @param {number} tabFieldCount 当前 Tab（或扁平表单）内字段数
 * @returns {number}
 */
function resolveFormFieldColSpan(field, tabFieldCount) {
  if (tabFieldCount < FORM_TAB_FIELDS_PER_TAB) {
    return 24;
  }
  if (isFullWidthFormControl(field)) {
    return 24;
  }
  return 12;
}

/** @type {Map<string, { componentPath: string, permissionPrefix: string }>} */
const MENU_INDEX = new Map();

/** 与 Domain 实体「导航属性区域」标记对齐 */
const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
 * PascalCase 转 camelCase
 * @param {string} str
 * @returns {string}
 */
function pascalToCamel(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

/** 通用实体字段 → common.page.entity.* 完整翻译键（与 TaktCommonI18nSeedData 对齐） */
const COMMON_ENTITY_FIELD_T_KEYS = {
  remark: 'common.page.entity.remark',
  extField: 'common.page.entity.extfield',
  tenantCode: 'common.page.entity.tenantcode',
  companyCode: 'common.page.entity.companycode',
  cultureCode: 'common.page.entity.culturecode',
  plantCode: 'common.page.entity.plantcode',
  relatedPlant: 'common.page.entity.relatedplant',
  createdAtStart: 'common.page.entity.createdatstart',
  createdAtEnd: 'common.page.entity.createdatend',
  approvalStatus: 'common.page.entity.approvalstatus',
  initiatorId: 'common.page.entity.initiatorid',
  initiatedAt: 'common.page.entity.initiatedat',
  approvalOpinion: 'common.page.entity.approvalopinion',
  approvedBy: 'common.page.entity.approvedby',
  approvedAt: 'common.page.entity.approvedat',
  flowInstanceId: 'common.page.entity.flowinstanceid',
};

/**
 * 解析字段完整 i18n 键（remark / extField 等走 common.page.entity.*，其余走 entity.{slug}.*）
 * 与 generate-entity-i18n-seed.cjs / TaktXxxI18nSeedData 键规则一致（slug 全小写 + 末段别名）
 * Culture / Translation 的 cultureCode 为业务字段 → entity.culture.code / entity.translation.culturecode（非 common）
 * @param {string} name 属性 camelCase
 * @param {string} entityI18nSlug 实体 slug（全小写，如 itasset）
 * @returns {string}
 */
function resolveFieldTranslationKey(name, entityI18nSlug) {
  const slug = String(entityI18nSlug || '').toLowerCase();
  if (name === 'cultureCode' && (slug === 'culture' || slug === 'translation')) {
    return buildEntityI18nKey(slug, name);
  }
  if (COMMON_ENTITY_FIELD_T_KEYS[name]) {
    return COMMON_ENTITY_FIELD_T_KEYS[name];
  }
  return buildEntityI18nKey(entityI18nSlug, name);
}

function fieldLabelTExpr(field, mode = 'form', piVar = 'pi') {
  if (mode === 'query') {
    return `${piVar}.queryLabel('${field.name}')`;
  }
  return `${piVar}.label('${field.name}')`;
}

/**
 * placeholderKey → EntityFieldPlaceholderKind
 * @param {string} placeholderKey
 * @returns {'required'|'select'|'optional'}
 */
function placeholderKeyToKind(placeholderKey) {
  if (placeholderKey.includes('select')) {
    return 'select';
  }
  if (placeholderKey.includes('optional')) {
    return 'optional';
  }
  return 'required';
}

/**
 * 生成字段 placeholder 表达式（表单 pi.ph；高级查询 pi.queryPh）
 * @param {{ name: string }} field
 * @param {string} placeholderKey common.page.form.placeholder.*
 * @param {'form'|'query'} [mode]
 * @returns {string}
 */
function fieldPlaceholderTExpr(field, placeholderKey, mode = 'form', piVar = 'pi') {
  if (isExtFieldField(field)) {
    return fieldExtFieldPlaceholderTExpr();
  }
  if (mode === 'query') {
    const kind = placeholderKeyToKind(placeholderKey);
    return `${piVar}.queryPh('${field.name}', '${kind}')`;
  }
  return `${piVar}.ph('${field.name}')`;
}

/**
 * 高级查询 queryFieldsMeta 已改为 index 内 PLANT_QUERY_FIELDS.map + pi.queryLabel（本函数保留供树表等过渡）
 * @param {{ name: string }} field
 * @returns {string}
 */
function buildQueryFieldMetaLine(field) {
  return `  { key: '${field.name}', label: pi.queryLabel('${field.name}') },`;
}

/**
 * 是否为 extField 扩展字段（前端字段名，对应后端 ExtField）
 * @param {{ name?: string }} field
 * @returns {boolean}
 */
function isExtFieldField(field) {
  return field?.name === EXT_FIELD_FORM_NAME;
}

/**
 * extField 输入框 placeholder t() 表达式
 * @returns {string}
 */
function fieldExtFieldPlaceholderTExpr() {
  return `t('${EXT_FIELD_PLACEHOLDER_I18N_KEY}')`;
}

/**
 * span=24 时表单项 label/wrapper（extField 图标+长文案自适应；其余 textarea 4/20）
 * @param {{ name?: string, htmlType?: string }} field
 * @param {string} indent
 * @param {number} colSpan
 * @returns {string}
 */
function renderFormItemLabelColAttrs(field, indent, colSpan, useUnifiedLabelCol = false) {
  if (useUnifiedLabelCol) {
    return isExtFieldField(field) ? `\n${indent}  class="takt-form-item-ext-field"` : '';
  }
  if (colSpan < 24) {
    return '';
  }
  if (isExtFieldField(field)) {
    return `\n${indent}  class="takt-form-item-ext-field"\n${indent}  :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"\n${indent}  :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"`;
  }
  if (isFullWidthFormControl(field)) {
    return `\n${indent}  :label-col="{ span: 4 }"\n${indent}  :wrapper-col="{ span: 20 }"`;
  }
  return '';
}

/**
 * 生成 a-form-item 开头（extField 带 RiQuestionLine 标签提示，图标在标签文字前）
 * @param {{ name: string, i18nKey: string, htmlType?: string }} field
 * @param {string} [indent]
 * @param {number} [colSpan]
 * @param {boolean} [useUnifiedLabelCol]
 * @returns {string}
 */
function renderFormItemOpening(field, indent = '              ', colSpan = 12, useUnifiedLabelCol = false) {
  const labelColAttrs = renderFormItemLabelColAttrs(field, indent, colSpan, useUnifiedLabelCol);
  if (field.htmlType === 'numberingRule') {
    return `${indent}<a-form-item
${indent}  :label="t('common.page.form.numberingRule')"
${indent}  name="${field.name}"${labelColAttrs}
${indent}>`;
  }
  if (isExtFieldField(field)) {
    return `${indent}<a-form-item
${indent}  name="${field.name}"${labelColAttrs}
${indent}>
${indent}  <template #label>
${indent}    <span class="takt-form-ext-field-label">
${indent}      <a-tooltip
${indent}        :title="t('${EXT_FIELD_HINT_I18N_KEY}')"
${indent}        placement="top"
${indent}      >
${indent}        <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
${indent}      </a-tooltip>
${indent}      <span>{{ ${fieldLabelTExpr(field)} }}</span>
${indent}    </span>
${indent}  </template>`;
  }
  return `${indent}<a-form-item
${indent}  :label="${fieldLabelTExpr(field)}"
${indent}  name="${field.name}"${labelColAttrs}
${indent}>`;
}

/**
 * 表单 / 高级查询含 extField 时追加 RiQuestionLine 导入
 * @param {...object[]} fieldLists 字段元数据数组（可传多个列表）
 * @returns {string}
 */
function buildExtFieldIconImportLine(...fieldLists) {
  const flat = fieldLists.flat();
  if (!flat.some(isExtFieldField)) {
    return '';
  }
  return "import { RiQuestionLine } from '@remixicon/vue'\n";
}

/**
 * 生成 index / 表单共用的 @remixicon/vue 导入行
 * @param {{ includeActionIcons?: boolean, formFields?: object[], queryFields?: object[] }} [options]
 * @returns {string}
 */
function buildRemixIconImportLine(options = {}) {
  const { includeActionIcons = false, formFields = [], queryFields = [] } = options;
  const icons = [];
  if (includeActionIcons) {
    icons.push('RiEditLine', 'RiDeleteBinLine');
  }
  if ([...formFields, ...queryFields].some(isExtFieldField)) {
    icons.push('RiQuestionLine');
  }
  if (!icons.length) {
    return '';
  }
  return `import { ${[...new Set(icons)].join(', ')} } from '@remixicon/vue'\n`;
}

/**
 * 隔离字段在表单中只读（Tenant / Company / Plant 主档实体本身可编辑）
 * @param {string} entityPascal
 * @param {string} fieldName
 * @returns {boolean}
 */
function isScopeFieldReadOnly(entityPascal, fieldName, entityScope) {
  if (!SCOPE_FORM_FIELD_NAMES.includes(fieldName)) {
    return false;
  }
  if (SCOPE_FIELD_EDITABLE_ENTITIES.has(entityPascal)) {
    return false;
  }
  if (fieldName === 'cultureCode' && entityScope === 'tenant-culture') {
    return false;
  }
  return true;
}

/**
 * 业务编码字段：camelCase 以 Code 结尾（编辑态 disabled，新增可编辑）
 * @param {string} fieldName
 * @returns {boolean}
 */
function isBusinessCodeFormFieldName(fieldName) {
  if (!fieldName || fieldName.length <= 4 || !fieldName.endsWith('Code')) {
    return false;
  }
  return !BUSINESS_CODE_EDIT_LOCK_SKIP_NAMES.has(fieldName);
}

/**
 * 表单 *Code 字段：编辑态 disabled，新增不禁用（已 readOnly 的隔离字段不重复处理）
 * @param {{ name: string, readOnly?: boolean }} field
 * @param {string} indent
 * @param {{ entityIdField?: string, formDataExpr?: string, rowIdField?: string, rowRecordExpr?: string }} [opts]
 * @returns {string}
 */
function renderFormCodeEditDisabledAttrs(field, indent, opts = {}) {
  if (field.readOnly || !isBusinessCodeFormFieldName(field.name)) {
    return '';
  }
  const formDataExpr = opts.formDataExpr ?? 'formData';
  if (opts.rowIdField && opts.rowRecordExpr) {
    return `\n${indent}  :disabled="!!${opts.rowRecordExpr}.${opts.rowIdField}"`;
  }
  if (opts.entityIdField) {
    return `\n${indent}  :disabled="!!${formDataExpr}?.${opts.entityIdField}"`;
  }
  return '';
}

/**
 * CreateDto 是否含上下文隔离字段（主表或子表；含 plantCode/relatedPlant）
 * @param {object[]} formFields
 * @param {object[]} [masterDetailChildren]
 * @returns {boolean}
 */
function hasScopeContextFormFields(formFields, masterDetailChildren = []) {
  const hasScope = (fields) => fields.some((f) => SCOPE_FORM_FIELD_NAMES.includes(f.name));
  return hasScope(formFields) || masterDetailChildren.some((c) => hasScope(c.formFields || []));
}

/**
 * 按 entityScope 决定表单隔离字段名（Culture 业务码 ≠ 隔离 CultureCode）
 * @param {'tenant'|'tenant-core'|'tenant-culture'|'tenant-plant'|'company'|'approval'} entityScope
 * @returns {string[]}
 */
function isolationFormFieldNamesForEntityScope(entityScope) {
  switch (entityScope) {
    case 'tenant-core':
      return ['tenantCode'];
    case 'tenant-culture':
      return ['tenantCode', 'cultureCode'];
    case 'tenant-plant':
      return ['tenantCode', 'relatedPlant'];
    case 'tenant':
      return ['tenantCode', 'cultureCode', 'relatedPlant'];
    case 'company':
    case 'approval':
      return ['tenantCode', 'companyCode', 'cultureCode', 'plantCode'];
    default:
      return SCOPE_FORM_FIELD_NAMES;
  }
}

/**
 * 从 Create 属性提取租户/公司/工厂/区域文化隔离字段（只读展示；主档实体可编辑）
 * @param {object[]} createProperties
 * @param {string} entityPascal
 * @returns {object[]}
 */
function buildScopeFormFields(createProperties, entityPascal) {
  const entityScope = resolveEntityScope(entityPascal, '', CONFIG.backendRoot);
  const isolationNames = isolationFormFieldNamesForEntityScope(entityScope);
  const propsByName = new Map((createProperties || []).map((p) => [p.name, p]));
  return isolationNames
    .filter((name) => propsByName.has(name))
    .filter((name) => {
      if (entityPascal === 'Plant' && name === 'plantCode') {
        return false;
      }
      if (entityPascal === 'Company' && name === 'relatedPlant') {
        return false;
      }
      return true;
    })
    .map((name) => ({
      ...propsByName.get(name),
      readOnly: isScopeFieldReadOnly(entityPascal, name, entityScope),
      optional: true,
    }));
}

/**
 * 表单控件 disabled 属性（隔离字段展示当前租户/公司/工厂/语言；禁止 HTML readonly，与 user-form 对齐）
 * @param {{ readOnly?: boolean, htmlType?: string }} field
 * @param {string} indent
 * @returns {string}
 */
function renderReadOnlyControlAttrs(field, indent) {
  if (!field.readOnly) {
    return '';
  }
  return `\n${indent}  disabled`;
}

/**
 * 按 Tab 规则拆分表单字段：
 * - 首 Tab：plantCode/relatedPlant → cultureCode → 业务字段（凑满 FORM_TAB_FIELDS_PER_TAB）
 * - 中间 Tab：其余业务字段分页
 * - 末 Tab：tenantCode / companyCode / extField / remark
 * @param {object[]} formFields
 * @returns {object[][]}
 */
function partitionFormFieldsForTabs(formFields) {
  if (!formFields?.length) {
    return [[]];
  }
  const leadingByName = new Map();
  const trailingByName = new Map();
  const main = [];
  for (const field of formFields) {
    if (FORM_TAB_LEADING_FIELD_NAME_SET.has(field.name)) {
      leadingByName.set(field.name, field);
    } else if (FORM_TAB_TRAILING_FIELD_NAME_SET.has(field.name)) {
      trailingByName.set(field.name, field);
    } else {
      main.push(field);
    }
  }
  const leading = FORM_TAB_LEADING_FIELD_NAMES
    .map((name) => leadingByName.get(name))
    .filter(Boolean);
  const trailing = FORM_TAB_TRAILING_FIELD_NAMES
    .map((name) => trailingByName.get(name))
    .filter(Boolean);
  if (main.length === 0) {
    if (leading.length && trailing.length) {
      return [leading, trailing];
    }
    if (leading.length) {
      return [leading];
    }
    if (trailing.length) {
      return [trailing];
    }
    return [[]];
  }
  const firstMainCapacity = Math.max(0, FORM_TAB_FIELDS_PER_TAB - leading.length);
  const firstMain = main.slice(0, firstMainCapacity);
  const restMain = main.slice(firstMainCapacity);
  const mainTabs = [[...leading, ...firstMain]];
  for (let i = 0; i < restMain.length; i += FORM_TAB_FIELDS_PER_TAB) {
    mainTabs.push(restMain.slice(i, i + FORM_TAB_FIELDS_PER_TAB));
  }
  if (trailing.length) {
    mainTabs.push(trailing);
  }
  return mainTabs;
}

/**
 * 表单 Tab 数量（业务字段分页 + 末 Tab trailing 字段）
 * @param {object[]|number} formFieldsOrCount 字段数组（推荐）或仅字段总数（无 trailing 语义）
 * @returns {number}
 */
function computeFormTabCount(formFieldsOrCount) {
  if (Array.isArray(formFieldsOrCount)) {
    return Math.max(1, partitionFormFieldsForTabs(formFieldsOrCount).length);
  }
  const fieldCount = Number(formFieldsOrCount) || 0;
  return Math.max(1, Math.ceil(fieldCount / FORM_TAB_FIELDS_PER_TAB));
}

/**
 * 表单 Tab 标签（多 Tab 时追加 当前/总数）
 * @param {number} tabIndex 1-based
 * @param {number} tabCount
 * @returns {string}
 */
function buildFormTabLabelAttr(tabIndex, tabCount) {
  if (tabCount <= 1) {
    return `:tab="t('common.page.form.tabs.basicinfo')"`;
  }
  return `:tab="t('common.page.form.tabs.basicinfo') + ' (${tabIndex}/${tabCount})'"`;
}

/**
 * 表单内容区高度 class（超过单 Tab 标准容量时用 10 行布局）
 * @returns {string} 生成到 *Form.vue 的 computed 表达式
 */
function buildFormContentClassComputedExpr() {
  return `computed(() => (formFields.length > ${FORM_TAB_FIELDS_PER_TAB} ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))`;
}

/**
 * 表单内容区 class 表达式（多 Tab 时固定 10 行，单 Tab 仍按 formFields 长度计算）
 * @param {boolean} useFormTabs
 * @param {number} tabCount
 * @returns {string}
 */
function buildFormContentClassExpr(useFormTabs, tabCount) {
  if (useFormTabs && tabCount > 1) {
    return "'takt-form-content-rows-10'";
  }
  return buildFormContentClassComputedExpr();
}

/**
 * 解析 CreateDto 隔离字段存在性（代码生成期烘焙，避免运行时 formFields.includes）
 * @param {object[]} formFields
 * @param {object[]} [masterDetailChildren]
 * @returns {{ hasTenant: boolean, hasCompany: boolean, hasCultureCode: boolean, hasPlantCode: boolean, hasRelatedPlant: boolean }}
 */
function resolveScopeFormFieldPresence(formFields, masterDetailChildren = []) {
  const names = new Set();
  for (const f of formFields || []) {
    names.add(f.name);
  }
  for (const child of masterDetailChildren || []) {
    for (const f of child.formFields || []) {
      names.add(f.name);
    }
  }
  return {
    hasTenant: names.has('tenantCode'),
    hasCompany: names.has('companyCode'),
    hasCultureCode: names.has('cultureCode'),
    hasPlantCode: names.has('plantCode'),
    hasRelatedPlant: names.has('relatedPlant'),
  };
}

/**
 * *Form.vue：租户/公司隔离 Pinia 与 applyScopeDefaults（按字段存在性 + entityScope 生成）
 * tenant-core：仅 tenantCode；tenant-culture：tenantCode + cultureCode 默认 mul（不回填公司语言）；
 * tenant：cultureCode 取公司主档语言；tenant-plant：tenantCode + relatedPlant。
 * @param {{ hasTenant: boolean, hasCompany: boolean, hasCultureCode: boolean, hasPlantCode: boolean, hasRelatedPlant: boolean }} presence
 * @param {string} entityIdField
 * @param {string} [entityPascal]
 * @returns {{ imports: string, script: string, watch: string }}
 */
function buildScopeContextFormScriptFragments(presence, entityIdField, entityPascal = '') {
  const {
    hasTenant,
    hasCompany,
    hasCultureCode,
    hasPlantCode = false,
    hasRelatedPlant = false,
  } = presence;
  const entityScope = entityPascal
    ? resolveEntityScope(entityPascal, '', CONFIG.backendRoot)
    : '';
  const injectCompanyCulture =
    hasCultureCode && (entityScope === 'tenant' || entityScope === 'company' || entityScope === 'approval');
  const injectMulCulture = hasCultureCode && entityScope === 'tenant-culture';
  if (!hasTenant && !hasCompany && !injectCompanyCulture && !injectMulCulture && !hasPlantCode && !hasRelatedPlant) {
    return { imports: '', script: '', watch: '' };
  }
  const imports = ["import { useTenantStore } from '@/stores/identity/useTenantStore'"];
  if (injectCompanyCulture) {
    imports.push("import { useUserStore } from '@/stores/identity/useUserStore'");
  }
  const storeLines = [];
  storeLines.push('/** Pinia：租户上下文 */');
  storeLines.push('const tenantStore = useTenantStore()');
  if (injectCompanyCulture) {
    storeLines.push('/** Pinia：用户上下文（当前公司 CultureCode 注入源） */');
    storeLines.push('const userStore = useUserStore()');
  }
  const applyLines = [];
  if (hasTenant) {
    applyLines.push(`  if (force || !target.tenantCode) {
    target.tenantCode = tenantStore.tenantCode
  }`);
  }
  if (hasCompany) {
    applyLines.push(`  if (force || !target.companyCode) {
    target.companyCode = tenantStore.companyCode
  }`);
  }
  if (injectMulCulture) {
    applyLines.push(`  if (force || !target.cultureCode) {
    target.cultureCode = 'mul'
  }`);
  } else if (injectCompanyCulture) {
    applyLines.push(`  if (force || !target.cultureCode) {
    target.cultureCode = userStore.userInfo?.companyDefaultCulture ?? userStore.userInfo?.cultureCode ?? ''
  }`);
  }
  if (hasPlantCode) {
    applyLines.push(`  if (force || !target.plantCode) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.plantCode = nextPlant
    }
  }`);
  }
  if (hasRelatedPlant && entityScope !== 'tenant-core' && entityScope !== 'tenant-culture') {
    applyLines.push(`  if (force || !target.relatedPlant) {
    const nextPlant = tenantStore.currentCompanyRelatedPlant || ''
    if (nextPlant) {
      target.relatedPlant = nextPlant
    }
  }`);
  }
  const scopeComment = injectMulCulture
    ? '租户 / CultureCode（默认 mul，不回填公司 UI 语言；无工厂）'
    : hasCompany
      ? '租户 / 公司 / CultureCode / PlantCode（登录或公司切换注入；工厂可选改）'
      : hasRelatedPlant && entityScope !== 'tenant-core' && entityScope !== 'tenant-culture'
        ? '租户 / RelatedPlant（登录或公司切换注入；关联工厂可选改）'
        : '租户级实体仅注入 tenantCode，表单只读';
  const script = `
${storeLines.join('\n')}

/**
 * 上下文隔离字段：${scopeComment}
 * @param target 表单数据
 * @param force 为 true 时强制覆盖（新增态或上下文切换）
 */
function applyScopeDefaults(target: Record<string, unknown>, force = false) {
${applyLines.join('\n')}
}
`;
  const watchSources = [];
  if (hasTenant) {
    watchSources.push('tenantStore.tenantCode');
  }
  if (hasCompany) {
    watchSources.push('tenantStore.companyCode');
  }
  if (injectCompanyCulture) {
    watchSources.push('userStore.userInfo?.companyDefaultCulture');
  }
  if (hasPlantCode || (hasRelatedPlant && entityScope !== 'tenant-core' && entityScope !== 'tenant-culture')) {
    watchSources.push('tenantStore.currentCompanyRelatedPlant');
  }
  const watchExpr = watchSources.length === 1
    ? `() => ${watchSources[0]}`
    : `() => [${watchSources.join(', ')}] as const`;
  const watchComment = hasCompany ? '公司/租户切换' : '租户切换';
  const watch = `
/** ${watchComment}时，新增态表单同步隔离字段 */
watch(
  ${watchExpr},
  () => {
    if (!props.formData?.${entityIdField}) {
      applyScopeDefaults(formState, true)
    }
  },
)
`;
  return {
    imports: `${imports.join('\n')}\n`,
    script,
    watch,
  };
}

/**
 * 实体 i18n 常量前缀（Plant → PLANT）
 * @param {string} entityPascal
 * @returns {string}
 */
function entityI18nConstPrefix(entityPascal) {
  return entityPascal.toUpperCase();
}

/**
 * 实体 i18n composable 导出名（Plant → usePlantI18n）
 * @param {string} entityPascal
 * @returns {string}
 */
function entityI18nHookName(entityPascal) {
  return `use${entityPascal}I18n`;
}

/**
 * composable 文件名（plant → usePlantI18n.ts）
 * @param {string} viewEntityKebab
 * @returns {string}
 */
function entityI18nComposableFileName(viewEntityKebab) {
  return `use${kebabToPascal(viewEntityKebab)}I18n.ts`;
}

/**
 * 主子表上下文 composable 文件名（Plant → usePlantMasterContext.ts）
 * @param {string} entityPascal
 * @returns {string}
 */
function entityMasterContextComposableFileName(entityPascal) {
  return `use${entityPascal}MasterContext.ts`;
}

/**
 * 表单占位类型（写入 {ENTITY}_PLACEHOLDER，不含 i18n 键）
 * @param {{ name?: string, optional?: boolean, htmlType?: string, readOnly?: boolean }} field
 * @returns {'required'|'select'|'optional'}
 */
function resolveFormPlaceholderKind(field) {
  if (field.readOnly && !isExtFieldField(field)) {
    return 'optional';
  }
  if (isExtFieldField(field) || field.optional || field.name === 'remark') {
    return 'optional';
  }
  if (field.htmlType === 'fileNameReadonly' || field.htmlType === 'fileUpload') {
    return 'optional';
  }
  if (field.htmlType === 'numberingRule') {
    return 'select';
  }
  if (field.htmlType === 'numberingCode') {
    return 'optional';
  }
  if (isFullWidthFormControl(field)) {
    return 'optional';
  }
  if (field.htmlType === 'select' || field.htmlType === 'apiSelect' || field.htmlType === 'date' || field.htmlType === 'switch') {
    return 'select';
  }
  return 'required';
}

/**
 * 生成 views/.../composables/use{Entity}I18n.ts（字段名清单 + useXxxI18n，文案由 takt-entity-i18n 推导）
 * @param {object} options
 * @returns {string}
 */
function buildEntityI18nComposableFile(options) {
  const {
    entityPascal,
    entityI18nSlug,
    viewModulePath,
    viewEntityKebab,
    listFields,
    formFields,
    queryFields,
    comment,
    includeChildPanelTableKeys = false,
    entityIdField,
    entityScope = 'company',
  } = options;
  const prefix = entityI18nConstPrefix(entityPascal);
  const hookName = entityI18nHookName(entityPascal);
  const queryTypeName = `${entityPascal}Query`;
  const queryStringFields = (queryFields || []).filter((f) => f.type !== 'number');
  const queryNumberFields = (queryFields || []).filter((f) => f.type === 'number');
  const listFieldLines = (listFields || []).map((f) => `  '${f.name}',`).join('\n');
  const queryStringLines = queryStringFields.map((f) => `  '${f.name}',`).join('\n');
  const numberTypeUnion = queryNumberFields.map((f) => `'${f.name}'`).join(' | ');
  const queryFieldTypeBlock = numberTypeUnion
    ? `export type ${entityPascal}QueryField =
  | (typeof ${prefix}_QUERY_STRING_FIELDS)[number]
  | ${numberTypeUnion}`
    : `export type ${entityPascal}QueryField = (typeof ${prefix}_QUERY_STRING_FIELDS)[number]`;
  const queryFieldsSpread = queryNumberFields.length > 0
    ? `export const ${prefix}_QUERY_FIELDS: readonly ${entityPascal}QueryField[] = [
  ...${prefix}_QUERY_STRING_FIELDS,
${queryNumberFields.map((f) => `  '${f.name}',`).join('\n')}
]`
    : `export const ${prefix}_QUERY_FIELDS: readonly ${entityPascal}QueryField[] = [...${prefix}_QUERY_STRING_FIELDS]`;
  const placeholderFields = formFields || [];
  const placeholderLines = placeholderFields
    .map((f) => `  ${f.name}: '${resolveFormPlaceholderKind(f)}',`)
    .join('\n');
  const childIdName = entityIdField || `${pascalToCamel(entityPascal)}Id`;
  const panelTableKeysBlock = includeChildPanelTableKeys
    ? (() => {
      const visibleKeys = buildChildPanelDefaultVisibleColumnKeyNames(listFields, childIdName, entityScope);
      const summaryFields = buildChildPanelSummarySumFieldNames(listFields);
      const visibleLines = visibleKeys.map((k) => `  '${k}',`).join('\n');
      const summaryLines = summaryFields.map((k) => `  '${k}',`).join('\n');
      let block = `
/** 明细右栏 panel 默认展示列（不含主键 id；含 action） */
export const ${prefix}_DEFAULT_VISIBLE_COLUMN_KEYS = [
${visibleLines}
] as const
`;
      if (summaryFields.length > 0) {
        block += `
/** 明细右栏 panel 合计列（当前页 dataSource 数值字段求和） */
export const ${prefix}_SUMMARY_SUM_FIELDS = [
${summaryLines}
] as const
`;
      } else {
        block += `
/** 明细右栏 panel 合计列（无可合计数值字段） */
export const ${prefix}_SUMMARY_SUM_FIELDS = [] as const
`;
      }
      return block;
    })()
    : '';
  return `// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：@/views/${viewModulePath}/composables
// 文件名称：${entityI18nComposableFileName(viewEntityKebab)}
// 功能描述：${comment}字段清单 + ${hookName}（字段名映射一次，文案由 entity.${entityI18nSlug}.* 种子动态解析）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { ${queryTypeName} } from '@/types/${options.modulePath}/${toFrontendEntityFileCamel(options.entityKebab)}.types'
import { buildEntitySelfI18nKey } from '@/utils/taktEntityI18n'
import { useEntityFieldI18n, type EntityFieldPlaceholderKind } from '@/composables/useEntityFieldI18n'

/** 与 Takt${entityPascal}I18nSeedData 一致的实体 slug */
export const ${prefix}_ENTITY_SLUG = '${entityI18nSlug}'

/** entity.${entityI18nSlug}._self 静态属性（导入组件 entity-i18n-key 等） */
export const ${prefix}_SELF_I18N_KEY = buildEntitySelfI18nKey(${prefix}_ENTITY_SLUG)

/** 列表业务列（不含主键） */
export const ${prefix}_LIST_FIELDS = [
${listFieldLines}
] as const
${panelTableKeysBlock}
/** 表单控件默认占位类型（仅 UI/校验语义，不含 i18n 键） */
export const ${prefix}_PLACEHOLDER = {
${placeholderLines}
} as const satisfies Record<string, EntityFieldPlaceholderKind>

/** 表单 ph() 可接受的字段（与 PLACEHOLDER 键一致，避免与 LIST_FIELDS 导航列混用） */
export type ${entityPascal}Field = keyof typeof ${prefix}_PLACEHOLDER

/** 高级查询可 trim 的字符串字段 */
export const ${prefix}_QUERY_STRING_FIELDS = [
${queryStringLines}
] as const satisfies readonly (keyof ${queryTypeName})[]

${queryFieldTypeBlock}

/** 高级查询抽屉全部字段（含数值） */
${queryFieldsSpread}

/**
 * ${comment}字段 i18n：index / ${entityPascal}Form 统一入口
 */
export function ${hookName}() {
  const ef = useEntityFieldI18n(${prefix}_ENTITY_SLUG)

  function ph(field: ${entityPascal}Field): string {
    return ef.placeholder(field, ${prefix}_PLACEHOLDER[field])
  }

  function queryPh(field: ${entityPascal}QueryField, kind: EntityFieldPlaceholderKind): string {
    return ef.queryPlaceholder(field, kind)
  }

  return {
    t: ef.t,
    label: ef.label,
    queryLabel: ef.queryLabel,
    queryPh,
    self: ef.self,
    ph,
  }
}
`;
}

/**
 * index.vue：composable 导入与 pi 实例
 * @param {string} entityPascal
 * @param {string} viewEntityKebab
 * @returns {string}
 */
function buildEntityI18nIndexImportBlock(entityPascal, viewEntityKebab, composableDir = './composables', options = {}) {
  const includeListFields = options.includeListFields !== false;
  const prefix = entityI18nConstPrefix(entityPascal);
  const hookName = entityI18nHookName(entityPascal);
  const composableStem = entityI18nComposableFileName(viewEntityKebab).replace(/\.ts$/, '');
  const listFieldsImportLine = includeListFields ? `  ${prefix}_LIST_FIELDS,\n` : '';
  return `import {
  ${hookName},
${listFieldsImportLine}  ${prefix}_QUERY_STRING_FIELDS,
  ${prefix}_QUERY_FIELDS,
  ${prefix}_SELF_I18N_KEY,
} from '${composableDir}/${composableStem}'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = ${hookName}()
`;
}

/**
 * *Form.vue：composable 导入与 pi 实例
 * @param {string} entityPascal
 * @param {string} viewEntityKebab
 * @returns {string}
 */
function buildEntityI18nFormImportBlock(entityPascal, viewEntityKebab) {
  const hookName = entityI18nHookName(entityPascal);
  const composableStem = entityI18nComposableFileName(viewEntityKebab).replace(/\.ts$/, '');
  return `import { ${hookName} } from '../composables/${composableStem}'

/** 实体字段 i18n */
const pi = ${hookName}()
`;
}

/**
 * 列表/导出：无业务查询条件时表格为空、不请求接口（与后端 HasAnyListQueryFilter 对齐）。
 * 有条件时正常分页查询（过滤生效，不是解锁全表）。
 */

/**
 * index.vue：高级查询 createEmptyAdvancedQueryForm + hasAnyListQueryFilter
 * @param {string} entityPascal
 * @param {object[]} queryFields
 * @returns {string}
 */
function buildAdvancedQueryFactoryBlock(entityPascal, queryFields = []) {
  const prefix = entityI18nConstPrefix(entityPascal);
  const numberFields = (queryFields || []).filter((f) => f.type === 'number');
  const numberInitLines = numberFields.map((f) => `    ${f.name}: undefined as number | undefined,`).join('\n');
  const numberCheckLines = numberFields.length > 0
    ? numberFields.map((f) => `  if (form.${f.name} !== undefined && form.${f.name} !== null) {
    return true
  }`).join('\n')
    : '';
  return `/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of ${prefix}_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
${numberCheckLines}
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(${prefix}_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof ${prefix}_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
${numberInitLines}  }
}`;
}

/**
 * index.vue：列表文本列配置 + buildXxxListColumn + columns computed 开头
 * @param {string} entityPascal
 * @param {string} entityIdName
 * @param {object[]} listCols 不含主键
 * @returns {string}
 */
function buildListColumnsGeneratorBlock(entityPascal, entityIdName) {
  const prefix = entityI18nConstPrefix(entityPascal);
  const helperName = `build${entityPascal}ListColumn`;
  return `/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function ${helperName}(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  ${helperName}('${entityIdName}', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...${prefix}_LIST_FIELDS.map((key) => ${helperName}(key, pi.label(key))),
  CreateActionColumn({`;
}

/**
 * 单表 CRUD 是否使用 TaktFormTabs 包裹（多 Tab 或主子表须保留）
 * @param {number} tabCount
 * @param {boolean} hasMasterDetail
 * @param {boolean} hasExtraTabPanes
 * @returns {boolean}
 */
function shouldWrapFormInTabs(tabCount, hasMasterDetail, hasExtraTabPanes, forceFormTabs = false) {
  if (forceFormTabs || hasMasterDetail || hasExtraTabPanes) {
    return true;
  }
  return tabCount > 1;
}

/** 单列 Tab（字段数 < 10）：整行单列布局 */
function shouldUseSingleColumnFormLayout(tabFieldCount) {
  return tabFieldCount < FORM_TAB_FIELDS_PER_TAB;
}

/**
 * 生成表单 a-row 块（各 Tab 样式由根 a-form.takt-generated-form 统一）
 * @param {string} itemsHtml
 * @param {string} [indent]
 * @returns {string}
 */
function buildFormRowMarkup(itemsHtml, indent = '    ') {
  const innerIndent = `${indent}  `;
  return `${innerIndent}<a-row :gutter="24">
${itemsHtml}
${innerIndent}</a-row>`;
}

/**
 * 生成表单字段 a-col 块
 * @param {object[]} tabFields
 * @param {object} formCodeControlOptions
 * @returns {string}
 */
function buildFormFieldColItems(tabFields, formCodeControlOptions = {}) {
  const tabFieldCount = formCodeControlOptions.colSpanFieldCount ?? tabFields.length;
  return tabFields.map((f) => {
    const colSpan = resolveFormFieldColSpan(f, tabFieldCount);
    const control = renderFormControl(f, 'formState.', '                ', formCodeControlOptions);
    return `            <a-col :span="${colSpan}">
${renderFormItemOpening(f, '              ', colSpan, true)}
${control}
              </a-form-item>
            </a-col>`;
  }).join('\n');
}

/**
 * 生成表单 a-tab-pane 块（包在 TaktFormTabs 内透传；分区与 taktFormTabs.ts 对齐）
 * @param {object} options
 * @param {object[]} options.formFields
 * @param {object} [options.formCodeControlOptions]
 * @param {boolean} [options.hasMasterDetail]
 * @param {string} [options.beforeFirstTabExtra] 首 Tab 字段行之前插入的模板（如树表 parentId）
 * @returns {{ tabCount: number, tabsHtml: string }}
 */
function buildFormTabPanesMarkup(options) {
  const {
    formFields,
    formCodeControlOptions = {},
    hasMasterDetail = false,
    beforeFirstTabExtra = '',
  } = options;
  const tabPartitions = partitionFormFieldsForTabs(formFields);
  const tabCount = Math.max(1, tabPartitions.length);
  const tabs = [];
  for (let i = 0; i < tabCount; i += 1) {
    const tabFields = tabPartitions[i];
    const tabIndex = i + 1;
    const tabLabel = buildFormTabLabelAttr(tabIndex, tabCount);
    const tabComment = tabIndex === 1 && hasMasterDetail ? '      <!-- 主表 -->\n' : '';
    const extraBefore = i === 0 && beforeFirstTabExtra ? `${beforeFirstTabExtra}\n` : '';
    const rowBlock = buildFormRowMarkup(buildFormFieldColItems(tabFields, formCodeControlOptions), '        ');
    tabs.push(`${tabComment}      <a-tab-pane
        key="tab-${i}"
        ${tabLabel}
        force-render
      >
        <div :class="formContentClass">
${extraBefore}${rowBlock}
        </div>
      </a-tab-pane>`);
  }
  return { tabCount, tabsHtml: tabs.join('\n') };
}

/**
 * 生成 *Form.vue 主表区模板（单 Tab 无主子表时扁平 a-row，与手工 CRUD 表单对齐）
 * @param {object} options
 * @returns {{ useFormTabs: boolean, body: string }}
 */
function buildGeneratedFormTemplateBody(options) {
  const {
    formFields,
    formCodeControlOptions,
    hasMasterDetail,
    extraTabPanes = '',
    entityKebab,
    forceFormTabs = false,
  } = options;
  const tabCount = computeFormTabCount(formFields);
  const useFormTabs = shouldWrapFormInTabs(tabCount, hasMasterDetail, Boolean(extraTabPanes), forceFormTabs);
  if (!useFormTabs) {
    return {
      useFormTabs: false,
      body: buildFormRowMarkup(buildFormFieldColItems(formFields, formCodeControlOptions)),
    };
  }
  const { tabsHtml } = buildFormTabPanesMarkup({
    formFields,
    formCodeControlOptions,
    hasMasterDetail,
  });
  const extraTabs = extraTabPanes ? `\n${extraTabPanes}` : '';
  return {
    useFormTabs: true,
    body: `    <TaktFormTabs
      v-model:active-key="activeTab"
      class="${entityKebab}-form-tabs"
    >
${tabsHtml}${extraTabs}
    </TaktFormTabs>`,
  };
}

/**
 * 表单使用 TaktFormTabs 时的 scoped 样式块
 * @param {boolean} useFormTabs
 * @returns {string}
 */
function buildFormTabsScopedStyleBlock(useFormTabs) {
  if (!useFormTabs) {
    return '';
  }
  return `
<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 50vh;
}

:deep(.ant-tabs-tabpane) {
  min-height: 50vh;
}
</style>`;
}

/**
 * 扫描菜单种子，建立 entityKebab → ComponentPath / Permission 索引
 */
function buildMenuIndex() {
  MENU_INDEX.clear();
  const seedsDir = path.join(CONFIG.backendRoot, 'Takt.Infrastructure', 'Data', 'Seeds', 'EntitySeedData');
  if (!fs.existsSync(seedsDir)) {
    return;
  }
  const files = fs.readdirSync(seedsDir).filter((f) => f.startsWith('TaktMenu') && f.endsWith('.cs'));
  const componentRe = /menu\.ComponentPath\s*=\s*"([^"]+)"/g;
  const permissionInBlockRe = /menu\.Permission\s*=\s*"([^"]+)"/g;
  files.forEach((file) => {
    const content = fs.readFileSync(path.join(seedsDir, file), 'utf-8');
    [...content.matchAll(componentRe)].forEach((match) => {
      const componentPath = match[1];
      if (!componentPath || !componentPath.endsWith('/index')) {
        return;
      }
      const blockStart = Math.max(0, match.index - 1200);
      const blockBefore = content.slice(blockStart, match.index);
      const permissionMatches = [...blockBefore.matchAll(permissionInBlockRe)];
      const perm = permissionMatches.length
        ? permissionMatches[permissionMatches.length - 1][1]
        : '';
      const segments = componentPath.split('/');
      const entityKebab = segments[segments.length - 1] === 'index'
        ? segments[segments.length - 2]
        : segments[segments.length - 1];
      if (!entityKebab) {
        return;
      }
      const permissionPrefix = perm.endsWith(':list') ? perm.replace(/:list$/, '') : '';
      const viewModulePath = componentPath.replace(/\/index$/, '');
      const entry = { componentPath: viewModulePath, permissionPrefix };
      MENU_INDEX.set(viewModulePath, entry);
      const existingByKebab = MENU_INDEX.get(entityKebab);
      if (!existingByKebab || existingByKebab.componentPath === viewModulePath) {
        MENU_INDEX.set(entityKebab, entry);
      }
    });
  });
}

/**
 * 从后端控制器解析权限前缀（取 list 权限去掉末段）
 * @param {string} entityShort
 * @returns {string}
 */
function resolvePermissionPrefixFromController(entityShort) {
  const controllerName = getControllerClassName(`Takt${entityShort}`);
  const controllersRoot = path.join(CONFIG.backendRoot, 'Takt.WebApi', 'Controllers');
  let found = '';
  function scan(dir) {
    if (found) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name === `${controllerName}.cs`) {
        const content = fs.readFileSync(full, 'utf-8');
        const match = content.match(/\[TaktPermission\("([^"]+):list"/);
        if (match) {
          found = match[1];
        }
        return;
      }
    }
  }
  if (fs.existsSync(controllersRoot)) {
    scan(controllersRoot);
  }
  return found;
}

/** 非主实体 interface 后缀（types 文件内） */
const MAIN_ENTITY_SKIP_SUFFIXES = [
  'Query', 'Create', 'Update', 'Template', 'Import', 'Export', 'Status', 'Sort', 'Tree',
  'Transposed', 'TransposedQuery', 'TransposedResult', 'TransposedBatch',
];

/**
 * 从 types 文件解析主实体 PascalCase 名（文件名去重后可能与类型名不一致）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @returns {string|null}
 */
function resolveMainEntityPascalFromTypes(interfaces) {
  for (const name of interfaces.keys()) {
    if (MAIN_ENTITY_SKIP_SUFFIXES.some((suffix) => name.endsWith(suffix))) {
      continue;
    }
    return name;
  }
  return null;
}

/**
 * 解析 .d.ts 中的 export interface
 * @param {string} content
 * @returns {Map<string, { name: string, properties: Array<{ name: string, type: string, optional: boolean, doc: string }> }>}
 */
function parseTypeInterfaces(content) {
  /** @type {Map<string, { name: string, properties: Array<{ name: string, type: string, optional: boolean, doc: string }> }>} */
  const ifaceMap = new Map();
  const chunks = content.split(/export interface /);
  chunks.slice(1).forEach((chunk) => {
    const nameMatch = chunk.match(/^(\w+)/);
    if (!nameMatch) {
      return;
    }
    const name = nameMatch[1];
    const bodyStart = chunk.indexOf('{');
    const bodyEnd = chunk.indexOf('\n}');
    if (bodyStart < 0 || bodyEnd < 0) {
      return;
    }
    const body = chunk.slice(bodyStart + 1, bodyEnd);
    const properties = [];
    const propRe = /\/\*\*([\s\S]*?)\*\/\s*(\w+)(\?)?:\s*([^;]+);/g;
    let m;
    while ((m = propRe.exec(body)) !== null) {
      properties.push({
        name: m[2],
        optional: Boolean(m[3]),
        type: m[4].trim(),
        doc: m[1].replace(/^\s*\*\s?/gm, '').replace(/\s+/g, ' ').trim(),
      });
    }
    ifaceMap.set(name, { name, properties });
  });
  return ifaceMap;
}

/**
 * 从 JSDoc 提取字典类型编码
 * @param {string} doc
 * @returns {string}
 */
function extractDictType(doc) {
  const match = doc.match(/字典\s+([a-z0-9_]+)/i);
  return match ? match[1] : '';
}

/**
 * 从 JSDoc 提取选项 API 路径（如「选项 TaktCompanies/options」）
 * @param {string} doc
 * @returns {string}
 */
function extractOptionsApiUrl(doc) {
  const match = String(doc || '').match(/选项\s+([A-Za-z][\w]*\/(?:tree-)?options)/);
  return match ? match[1] : '';
}

/**
 * 从 JSDoc 拼 TaktSelect :api-params（行政区划 Level、统驭科目 auxiliaryType）
 * @param {{ doc?: string, apiUrl?: string }} field
 * @returns {string} 如 `{ level: 2 }`，无则空串
 */
function resolveApiSelectParamsObjectLiteral(field) {
  const doc = String(field?.doc || '');
  const apiUrl = String(field?.apiUrl || '');
  const parts = [];
  if (apiUrl.includes('TaktAdminDivisions/options')) {
    const levelMatch = doc.match(/Level\s*=\s*(\d)/i);
    if (levelMatch) {
      parts.push(`level: ${levelMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktApsGroups/options')) {
    const categoryMatch = doc.match(/apsGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`apsGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktEcGroups/options')) {
    const categoryMatch = doc.match(/ecGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`ecGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktComplaintGroups/options')) {
    const categoryMatch = doc.match(/complaintGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`complaintGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktCostGroups/options')) {
    const categoryMatch = doc.match(/costGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`costGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktQualityGroups/options')) {
    const categoryMatch = doc.match(/qualityGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`qualityGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktBomGroups/options')) {
    const categoryMatch = doc.match(/bomGroupCategory\s*=\s*(\d+)/i);
    if (categoryMatch) {
      parts.push(`bomGroupCategory: ${categoryMatch[1]}`);
    }
  }
  if (apiUrl.includes('TaktAccountTitles/options')) {
    if (/reconciliationOnly\s*=\s*true/i.test(doc)) {
      parts.push('reconciliationOnly: true');
    }
    const auxMatch = doc.match(/auxiliaryType\s*=\s*([A-Za-z])/);
    if (auxMatch) {
      parts.push(`auxiliaryType: '${auxMatch[1]}'`);
    }
  }
  return parts.length ? `{ ${parts.join(', ')} }` : '';
}

/**
 * TaktSelect 的 :api-params 属性行（含前导换行）
 * @param {{ doc?: string, apiUrl?: string }} field
 * @param {string} indent
 * @returns {string}
 */
function renderApiSelectParamsAttr(field, indent) {
  const literal = resolveApiSelectParamsObjectLiteral(field);
  if (!literal) {
    return '';
  }
  return `\n${indent}  :api-params="${literal}"`;
}

/** 字段名 → 选项 API 回退（实体注释缺失时） */
const FIELD_OPTIONS_API_FALLBACK = {
  relatedPlant: 'TaktPlants/options',
  plantCode: 'TaktPlants/options',
  relatedCompany: 'TaktCompanies/options',
};

/**
 * 解析选项 API 路径：JSDoc「选项 xxx/options」优先，其次字段名回退
 * @param {{ name?: string, doc?: string }} field
 * @returns {string}
 */
function resolveOptionsApiUrl(field) {
  if (isEntityDerivedFormField(field.doc)) {
    return '';
  }
  const fromDoc = extractOptionsApiUrl(field.doc);
  if (fromDoc) {
    return fromDoc;
  }
  const name = field.name || '';
  return FIELD_OPTIONS_API_FALLBACK[name] || '';
}

/**
 * 表格行联合类型名（index.vue bodyCell slot 与 dataSource 兼容）
 * @param {string} entityPascal
 * @returns {string}
 */
function entityRowRecordTypeName(entityPascal) {
  return `${entityPascal}RowRecord`;
}

/**
 * 生成 index.vue 表格行类型别名
 * @param {string} entityPascal
 * @returns {string}
 */
function buildEntityRowRecordTypeAlias(entityPascal) {
  const rowType = entityRowRecordTypeName(entityPascal);
  return `/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type ${rowType} = ${entityPascal} | Record<string, unknown>
`;
}

/**
 * DTO/类型 JSDoc 是否为响应 DTO 填充字段（服务层 Fill，不参与列表/表单/查询）
 * 与 generate-dtos-from-entity 中「（填充字段）」注释对齐
 * @param {string} doc
 * @returns {boolean}
 */
function isDtoFillField(doc) {
  return /填充字段/.test(doc || '');
}

/** TS 标量类型（列表/表单可绑定） */
const TS_SCALAR_TYPES = new Set(['string', 'number', 'boolean', 'unknown', 'null', 'undefined']);

/**
 * DTO 属性是否为 ManyToOne/OneToMany 导航对象（非标量，不参与列表/表单列）
 * 与 generate-dtos-from-entity appendNavigationDtoProperties「（主表：」「（子表：」对齐
 * @param {{ doc?: string, type?: string }} field
 * @returns {boolean}
 */
function isDtoNavigationProperty(field) {
  const doc = field?.doc || '';
  if (/（主表：|（子表：/.test(doc)) {
    return true;
  }
  const rawType = String(field?.type || '').trim();
  if (!rawType) {
    return false;
  }
  const baseType = rawType.replace(/\?$/, '').replace(/\[\]$/, '').trim();
  if (TS_SCALAR_TYPES.has(baseType)) {
    return false;
  }
  if (/^(?:Array<|ReadonlyArray<)/.test(rawType) || /\[\]$/.test(rawType)) {
    return true;
  }
  if (/^Record<|^Map<|^Set</.test(baseType)) {
    return true;
  }
  return /^[A-Z]\w*$/.test(baseType);
}

/**
 * 实体 XML/DTO 注释标记为冗余联动、回填（仅排序/行号）、计算结果或固定值（表单须 disabled，禁止选项下拉）
 * 约定：…（冗余：…联动）、…（回填：…）、…（计算结果：…）或 …（固定值）
 * 口径：冗余联动=选 FK 后前端带出并落库；回填=仅排序号/行号等服务端生成
 * @param {string} doc
 * @returns {boolean}
 */
function isEntityDerivedFormField(doc) {
  return /（冗余|冗余：|（联动|联动：|（回填|回填：|计算结果|（计算结果|固定值|（固定值/.test(doc || '');
}

/** sys_normal_disable：注释含「1=启用…0=禁用」的通用状态字段 */
const COMMON_STATUS_DICT_TYPE = 'sys_normal_disable';
/** TaktApprovalEntityBase.ApprovalStatus 及镜像业务状态字段共用 */
const APPROVAL_STATUS_DICT_TYPE = 'sys_approval_status';
const CONVERT_STATUS_DICT_TYPE = 'sys_convert_status';
const APPROVAL_WORKFLOW_STATUS_FIELD_NAMES = new Set([
  'ApprovalStatus',
  'approvalStatus',
  'leaveStatus',
  'overtimeStatus',
  'expenseStatus',
  'countersignStatus',
]);
const CONVERT_STATUS_FIELD_NAMES = new Set([
  'ConvertedStatus',
  'convertedStatus',
]);

/** sys_yes_no：是否/内置等 */
const YES_NO_DICT_TYPE = 'sys_yes_no';

/** 编辑态不锁定：隔离字段 / 外键引用编码（非本实体业务主码） */
const BUSINESS_CODE_EDIT_LOCK_SKIP_NAMES = new Set([
  'tenantCode',
  'companyCode',
  'cultureCode',
  'plantCode',
  'relatedPlant',
  'deptCode',
  'parentCode',
]);

/**
 * 是否为启用/禁用通用状态（对齐 TaktCommonStatus）
 * @param {string} doc
 * @returns {boolean}
 */
function isCommonEnableDisableStatus(doc) {
  if (!doc) {
    return false;
  }
  return /1\s*=\s*启用/.test(doc) && /0\s*=\s*禁用/.test(doc);
}

/**
 * 是否为通用启用/禁用状态列（列表用 a-switch）
 * @param {{ name?: string, doc?: string, dictType?: string }} field
 * @returns {boolean}
 */
function isNormalDisableStatusField(field) {
  const dictType = field.dictType || resolveDictType(field);
  return dictType === COMMON_STATUS_DICT_TYPE || isCommonEnableDisableStatus(field.doc);
}

/**
 * 是否为内置 sys_yes_no 列（列表用 a-switch）
 * @param {{ name?: string, doc?: string, dictType?: string }} field
 * @returns {boolean}
 */
function isBuiltinYesNoField(field) {
  const name = field.name || '';
  const doc = field.doc || '';
  if (/^isBuiltIn$/i.test(name)) {
    return true;
  }
  const dictType = field.dictType || resolveDictType(field);
  if (dictType !== YES_NO_DICT_TYPE) {
    return false;
  }
  return /builtin|built_in|isbuiltin/i.test(name) || /内置/.test(doc);
}

/**
 * 列表列是否应渲染为 a-switch（状态 / 内置）；仅 index 表格，表单仍用 TaktSelect
 * @param {{ name?: string, doc?: string, dictType?: string }} field
 * @returns {boolean}
 */
function isListSwitchField(field) {
  return isNormalDisableStatusField(field) || isBuiltinYesNoField(field);
}

/**
 * 解析启用态对应字典值（默认 1=启用）
 * @param {{ doc?: string, dictType?: string }} field
 * @returns {number}
 */
function resolveEnableCheckedValue(field) {
  if (field.doc && /0\s*=\s*启用/.test(field.doc) && /1\s*=\s*禁用/.test(field.doc)) {
    return 0;
  }
  return 1;
}

/**
 * 附加列表开关列元数据
 * @param {{ name: string, doc?: string, dictType?: string, htmlType?: string }} field
 */
function attachListSwitchMeta(field) {
  if (!isListSwitchField(field)) {
    return field;
  }
  const isBuiltin = isBuiltinYesNoField(field);
  const checked = isBuiltin ? 1 : resolveEnableCheckedValue(field);
  return {
    ...field,
    isListSwitch: true,
    switchKind: isBuiltin ? 'builtin' : 'status',
    switchCheckedValue: checked,
    switchUncheckedValue: checked === 1 ? 0 : 1,
  };
}

/**
 * 字段名转处理器后缀（status → Status）
 * @param {string} name
 * @returns {string}
 */
function fieldNameToHandlerSuffix(name) {
  return name ? name.charAt(0).toUpperCase() + name.slice(1) : '';
}

/**
 * 解析字典类型：显式「字典 xxx」或 TaktCommonStatus 语义
 * @param {{ name: string, doc: string }} field
 * @returns {string}
 */
function resolveDictType(field) {
  if (APPROVAL_WORKFLOW_STATUS_FIELD_NAMES.has(field.name)) {
    return APPROVAL_STATUS_DICT_TYPE;
  }
  if (CONVERT_STATUS_FIELD_NAMES.has(field.name)) {
    return CONVERT_STATUS_DICT_TYPE;
  }
  const explicit = extractDictType(field.doc);
  if (explicit) {
    return explicit;
  }
  if (isCommonEnableDisableStatus(field.doc)) {
    return COMMON_STATUS_DICT_TYPE;
  }
  if (field.doc && /内置/.test(field.doc) && /1\s*=\s*是/.test(field.doc)) {
    return YES_NO_DICT_TYPE;
  }
  if (/^isBuiltIn$/i.test(field.name || '')) {
    return YES_NO_DICT_TYPE;
  }
  return '';
}

/**
 * 是否为 DateTime/DateOnly 业务字段（types 为 string，须结合实体列或命名/注释推断）
 * @param {{ name?: string, doc?: string, isDateTime?: boolean }} field
 * @returns {boolean}
 */
function isDateTimeField(field) {
  if (field.isDateTime) {
    return true;
  }
  const name = String(field.name || '');
  const lower = name.toLowerCase();
  const doc = String(field.doc || '');
  if (/^(validfrom|validto)$/.test(lower)) {
    return true;
  }
  if (/date/i.test(name)) {
    return true;
  }
  if (/Start$/.test(name) || /End$/.test(name)) {
    if (/date|time|validfrom|validto|createdat|updatedat/i.test(lower)) {
      return true;
    }
  }
  if (/^(created|updated|deleted|approved|published|submitted|completed|closed|opened)at$/i.test(name)) {
    return true;
  }
  if (/日期|datetime|dateonly|生效日期|失效日期|年月日/.test(doc.toLowerCase())) {
    return true;
  }
  return false;
}

/**
 * 表单/查询 DatePicker 是否带时分秒（审计时间等；业务生效/失效日期仅选日）
 * @param {{ name?: string }} field
 * @returns {boolean}
 */
function shouldDatePickerShowTime(field) {
  const name = String(field.name || '');
  const lower = name.toLowerCase();
  if (/createdat|updatedat|deletedat/.test(lower)) {
    return true;
  }
  return false;
}

/**
 * DatePicker value-format 与 show-time 属性片段
 * @param {{ name?: string }} field
 * @param {string} indent
 * @returns {{ valueFormat: string, showTimeAttr: string }}
 */
function resolveDatePickerTemplateAttrs(field, indent = '                ') {
  const showTime = shouldDatePickerShowTime(field);
  return {
    valueFormat: showTime ? 'YYYY-MM-DD HH:mm:ss' : 'YYYY-MM-DD',
    showTimeAttr: showTime ? `\n${indent}  show-time` : '',
  };
}

/**
 * 是否为「xxx内容」正文（表单须 takt-rich-editor）
 * 判定：字段名 content / XxxContent；或 XML 摘要标题以「内容」结尾；或注释含「富文本 HTML」。
 * 排除 contentType 等元数据、流程 ProcessContent / 快照、JSON 差异列。
 * @param {{ name?: string, doc?: string }} field
 * @returns {boolean}
 */
function isRichTextContentField(field) {
  const name = String(field?.name || '');
  if (!name || RICH_TEXT_CONTENT_NAME_EXCLUDE.test(name)) {
    return false;
  }
  if (/ProcessContent/i.test(name) || /ContentSnapshot$/i.test(name)) {
    return false;
  }
  if (/^content$/i.test(name) || /Content$/.test(name)) {
    return true;
  }
  const firstLine = String(field?.doc || '')
    .split(/\n/)
    .map((line) => line.replace(/^\/\/\/?\s*/, '').replace(/^\*\s*/, '').trim())
    .find(Boolean) || '';
  if (!firstLine || /JSON/i.test(firstLine)) {
    return false;
  }
  if (/富文本\s*HTML/.test(firstLine) || /富文本\s*HTML/.test(String(field?.doc || ''))) {
    return true;
  }
  const title = firstLine.replace(/[（(].*$/, '').trim();
  return /内容$/.test(title);
}

/**
 * 是否为 Foundation 文件目录实体（TaktFile → File）。该实体自身维护元数据，不生成业务上传控件。
 * @param {string} [entityPascal]
 * @returns {boolean}
 */
function isFileCatalogEntity(entityPascal) {
  return String(entityPascal || '') === 'File';
}

/**
 * 业务实体是否同时具备 fileName + accessUrl（须 takt-upload-file；TaktFile 除外）
 * @param {Array<{ name?: string }>} fields
 * @param {string} [entityPascal]
 * @returns {boolean}
 */
function hasBusinessFileUploadPair(fields, entityPascal) {
  if (isFileCatalogEntity(entityPascal)) {
    return false;
  }
  const names = new Set((fields || []).map((f) => f.name));
  return names.has('fileName') && names.has('accessUrl');
}

/**
 * 将 fileName 标为上传后只读回填，accessUrl 标为上传控件
 * @param {object[]} fields
 * @param {string} [entityPascal]
 * @returns {object[]}
 */
function applyBusinessFileUploadMeta(fields, entityPascal) {
  if (!hasBusinessFileUploadPair(fields, entityPascal)) {
    return fields;
  }
  return (fields || []).map((f) => {
    if (f.name === 'fileName') {
      return { ...f, htmlType: 'fileNameReadonly' };
    }
    if (f.name === 'accessUrl') {
      return { ...f, htmlType: 'fileUpload' };
    }
    return f;
  });
}

/**
 * 字段是否走「编码规则下拉 + 只读业务编码」（实体 XML 含自动通过 TaktNumbering，且非 MIME）
 * @param {{ name?: string, doc?: string, summary?: string }} field
 * @returns {boolean}
 */
function isTaktNumberingAutoFormField(field) {
  return isTaktNumberingAutoFormFieldSummary(field?.doc || field?.summary || '');
}

/**
 * 实体 PascalCase → 编码规则单据类型（TaktMenu.MenuName）
 * 与 TaktNumberingSeedData 关联菜单名称一致
 */
const NUMBERING_DOCUMENT_TYPE_BY_ENTITY = {
  Announcement: '公告通知',
  Meeting: '会议中心',
  Document: '文档管理',
  News: '新闻',
  Employee: '员工档案',
  FlowForm: '表单管理',
  Configurable: '定制报表',
};

/**
 * 从字段 XML 摘要解析单据类型菜单名（单据类型菜单：xxx / 单据类型：xxx）
 * @param {string} [summary]
 * @returns {string}
 */
function parseNumberingDocumentTypeFromSummary(summary) {
  const text = String(summary || '');
  const m = text.match(/单据类型(?:菜单)?[：:]\s*([^\s；;，,）)\]]+)/);
  return m ? String(m[1]).trim() : '';
}

/**
 * 解析表单编码规则下拉应过滤的 DocumentType（MenuName）
 * @param {object} field
 * @param {string} [entityPascal]
 * @returns {string}
 */
function resolveNumberingDocumentType(field, entityPascal) {
  const fromDoc = parseNumberingDocumentTypeFromSummary(field?.doc || field?.summary || '');
  if (fromDoc) {
    return fromDoc;
  }
  const key = String(entityPascal || '').replace(/^Takt/, '');
  return NUMBERING_DOCUMENT_TYPE_BY_ENTITY[key] || '';
}

/**
 * 表单插入 numberingRuleCode + 将业务编码标为 numberingCode（只读预览）
 * @description CreateDto 若已含 NumberingRuleCode，须跳过，避免与注入项重复（重复项会走 entity.*.numberingrulecode 无翻译）
 * @param {object[]} fields
 * @param {string} [entityPascal]
 * @returns {object[]}
 */
function applyTaktNumberingFormMeta(fields, entityPascal) {
  const list = fields || [];
  if (!list.some(isTaktNumberingAutoFormField)) {
    return list;
  }
  /** @type {object[]} */
  const out = [];
  for (const f of list) {
    // Create/Update Dto 上的 NumberingRuleCode：由下方注入的 numberingRule 控件统一渲染
    if (f.name === 'numberingRuleCode') {
      continue;
    }
    if (!isTaktNumberingAutoFormField(f)) {
      out.push(f);
      continue;
    }
    const documentType = resolveNumberingDocumentType(f, entityPascal);
    out.push({
      name: 'numberingRuleCode',
      type: 'string',
      optional: false,
      htmlType: 'numberingRule',
      businessCodeField: f.name,
      documentType,
      doc: '编码规则（表单选规则后预览业务编码；提交 NumberingRuleCode；不落库）',
      i18nKey: 'common.page.form.numberingRule',
      maxLength: 40,
    });
    out.push({
      ...f,
      htmlType: 'numberingCode',
      readOnly: true,
      optional: false,
    });
  }
  return out;
}

/**
 * 表单是否含编码规则取号控件
 * @param {Array<{ htmlType?: string }>} fields
 * @returns {boolean}
 */
function fieldsNeedTaktNumbering(fields) {
  return (fields || []).some((f) => f.htmlType === 'numberingRule' || f.htmlType === 'numberingCode');
}

/**
 * 表单 script：useTaktFormNumbering 接线
 * @param {Array<{ htmlType?: string, businessCodeField?: string, name?: string }>} fields
 * @param {string} entityIdField
 * @returns {string}
 */
function buildTaktNumberingFormScriptBlock(fields, entityIdField) {
  const ruleFields = (fields || []).filter((f) => f.htmlType === 'numberingRule');
  if (ruleFields.length === 0) {
    return '';
  }
  const wiring = ruleFields.map((f) => {
    const codeField = f.businessCodeField || 'businessCode';
    return `useTaktFormNumbering({
  formState,
  isEdit: isEditMode,
  businessCodeField: '${codeField}',
})`;
  }).join('\n\n');
  return `/** 是否编辑态（编码规则取号仅新增） */
const isEditMode = computed(() => !!props.formData?.${entityIdField})

${wiring}
`;
}

/**
 * 表单是否含业务文件上传控件
 * @param {Array<{ htmlType?: string }>} fields
 * @returns {boolean}
 */
function fieldsNeedFileUpload(fields) {
  return (fields || []).some((f) => f.htmlType === 'fileUpload');
}

/**
 * 表单是否整行控件（textarea / 富文本 / 文件上传）
 * @param {{ htmlType?: string }} field
 * @returns {boolean}
 */
function isFullWidthFormControl(field) {
  return field.htmlType === 'textarea' || field.htmlType === 'richEditor' || field.htmlType === 'fileUpload';
}

/**
 * 推断表单控件类型
 * @param {{ name: string, type: string, doc: string, isDateTime?: boolean }} field
 * @returns {'apiSelect'|'select'|'textarea'|'richEditor'|'date'|'switch'|'input'}
 */
function inferHtmlType(field) {
  if (field.htmlType === 'numberingRule' || field.htmlType === 'numberingCode') {
    return field.htmlType;
  }
  if (isTaktNumberingAutoFormField(field)) {
    return 'numberingCode';
  }
  const apiUrl = field.apiUrl || resolveOptionsApiUrl(field);
  if (apiUrl) {
    return 'apiSelect';
  }
  const dict = field.dictType || resolveDictType(field);
  if (dict) {
    return 'select';
  }
  if (field.type === 'boolean') {
    return 'switch';
  }
  if (isDateTimeField(field)) {
    return 'date';
  }
  if (/time$/i.test(field.name) && field.type === 'string' && !/Start$|End$/.test(field.name)) {
    return 'date';
  }
  if (isRichTextContentField(field)) {
    return 'richEditor';
  }
  const lower = field.name.toLowerCase();
  if (TEXTAREA_NAME_HINTS.some((hint) => lower.includes(hint.toLowerCase()))) {
    return 'textarea';
  }
  if (field.type === 'number') {
    return 'select';
  }
  return 'input';
}

/**
 * 解析 API 文件导出函数与 API_BASE
 * @param {string} content
 * @returns {{ apiBase: string, methods: Record<string, string> }}
 */
function parseApiFile(content) {
  const apiBaseMatch = content.match(/const\s+\w+_API_BASE\s*=\s*'([^']+)'/);
  const apiBase = apiBaseMatch ? apiBaseMatch[1] : '';
  /** @type {Record<string, string>} */
  const methods = {};
  const fnRe = /export function (\w+)\(/g;
  let m;
  while ((m = fnRe.exec(content)) !== null) {
    methods[m[1]] = m[1];
  }
  return { apiBase, methods };
}

/**
 * 根据 API 方法名推断能力开关
 * @param {string} entityPascal
 * @param {Record<string, string>} methods
 */
function detectApiCapabilities(entityPascal, methods) {
  const names = Object.keys(methods);
  const camel = pascalToCamel(entityPascal);
  const pick = (...candidates) => candidates.find((c) => names.includes(c)) || '';
  return {
    hasGetById: Boolean(pick(`get${entityPascal}ById`)),
    apiGetById: pick(`get${entityPascal}ById`),
    hasGetList: Boolean(pick(`get${entityPascal}List`)),
    hasGetOptions: Boolean(pick(`get${entityPascal}Options`)),
    hasCreate: Boolean(pick(`create${entityPascal}`)),
    hasUpdate: Boolean(pick(`update${entityPascal}`)),
    hasDelete: Boolean(pick(`delete${entityPascal}ById`)),
    hasDeleteBatch: Boolean(pick(`delete${entityPascal}Batch`)),
    hasGetTemplate: Boolean(pick(`get${entityPascal}Template`)),
    hasImport: Boolean(pick(`import${entityPascal}`, `import${entityPascal}Data`)),
    hasExport: Boolean(pick(`export${entityPascal}`, `export${entityPascal}Data`)),
    hasUpdateStatus: Boolean(pick(`update${entityPascal}Status`)),
    hasUpdateBuiltIn: Boolean(pick(`update${entityPascal}BuiltIn`)),
    hasUpdateSort: Boolean(pick(`update${entityPascal}Sort`)),
    apiGetList: pick(`get${entityPascal}List`),
    apiCreate: pick(`create${entityPascal}`),
    apiUpdate: pick(`update${entityPascal}`),
    apiDelete: pick(`delete${entityPascal}ById`),
    apiDeleteBatch: pick(`delete${entityPascal}Batch`),
    apiGetTemplate: pick(`get${entityPascal}Template`),
    apiImport: pick(`import${entityPascal}`, `import${entityPascal}Data`),
    apiExport: pick(`export${entityPascal}`, `export${entityPascal}Data`),
    apiUpdateStatus: pick(`update${entityPascal}Status`),
    apiUpdateBuiltIn: pick(`update${entityPascal}BuiltIn`),
    apiUpdateSort: pick(`update${entityPascal}Sort`),
    entityIdName: `${camel}Id`,
    entityClassName: `Takt${entityPascal}`,
  };
}

/**
 * 解析 Type[] 元素类型名
 * @param {string} typeStr
 * @returns {string|null}
 */
function parseArrayElementType(typeStr) {
  const match = typeStr.match(/^(\w+)\[\]$/);
  return match ? match[1] : null;
}

/**
 * 从 C# 实体文件提取 class 体
 * @param {string} content
 * @param {string} className
 * @returns {string}
 */
function extractClassBlock(content, className) {
  const startRegex = new RegExp(`public\\s+(?:sealed\\s+|partial\\s+)?class\\s+${className}\\b`);
  const startMatch = startRegex.exec(content);
  if (!startMatch) {
    return '';
  }
  const braceStart = content.indexOf('{', startMatch.index);
  if (braceStart < 0) {
    return '';
  }
  let depth = 0;
  for (let i = braceStart; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        return content.slice(braceStart + 1, i);
      }
    }
  }
  return '';
}

/**
 * 拆分实体 class 体：标量区 / 导航属性区
 * @param {string} classBody
 */
function splitClassBodyByNavigationRegion(classBody) {
  const lines = classBody.split('\n');
  let markerLineIdx = -1;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].includes(NAVIGATION_REGION_MARKER)) {
      markerLineIdx = i;
      break;
    }
  }
  if (markerLineIdx === -1) {
    return { scalarBody: classBody, navigationBody: '' };
  }
  let navStartLine = markerLineIdx;
  while (navStartLine > 0 && /^\s*\/\/\s*=+/.test(lines[navStartLine - 1])) {
    navStartLine -= 1;
  }
  return {
    scalarBody: lines.slice(0, navStartLine).join('\n'),
    navigationBody: lines.slice(navStartLine).join('\n'),
  };
}

/**
 * 解析 Domain 实体 OneToMany 导航（与 generate-services-from-dtos 对齐）
 * @param {string} entityFile
 * @returns {Array<{ navPropName: string, childEntity: string, childShort: string, foreignKeyOnChild: string }>}
 */
function parseOneToManyNavigations(entityFile) {
  const content = fs.readFileSync(entityFile, 'utf-8');
  const classMatch = content.match(/public\s+(?:sealed\s+)?class\s+(Takt\w+)\s*:\s*\w+/);
  if (!classMatch) {
    return [];
  }
  const classBody = extractClassBlock(content, classMatch[1]);
  if (!classBody) {
    return [];
  }
  const { navigationBody } = splitClassBodyByNavigationRegion(classBody);
  const body = navigationBody.trim() ? navigationBody : classBody;
  const navRegex =
    /\[Navigate\(\s*NavigateType\.OneToMany\s*,\s*nameof\(\w+\.(\w+)\)\s*\)\]([\s\S]*?)public\s+List<(Takt\w+)>\??\s+(\w+)\s*\{\s*get;\s*set;/g;
  /** @type {Array<{ navPropName: string, childEntity: string, childShort: string, foreignKeyOnChild: string }>} */
  const navigations = [];
  let match;
  while ((match = navRegex.exec(body)) !== null) {
    navigations.push({
      foreignKeyOnChild: match[1],
      childEntity: match[3],
      childShort: match[3].replace(/^Takt/, ''),
      navPropName: match[4],
    });
  }
  return navigations;
}

/**
 * 子表 types 文件路径（跨文件 Create/Entity 类型）
 * @param {string} modulePath
 * @param {string} childPascal
 */
function resolveChildTypesFilePath(modulePath, childPascal) {
  const childKebab = resolveFrontendModuleFileName(pascalToKebab(childPascal), modulePath);
  return path.join(CONFIG.frontendRoot, CONFIG.typesDir, modulePath, toFrontendTypesFileName(childKebab));
}

/**
 * 子表 types 是否存在于 frontend/types
 * @param {string} modulePath
 * @param {string} childPascal
 */
function childTypesFileExists(modulePath, childPascal) {
  return fs.existsSync(resolveChildTypesFilePath(modulePath, childPascal));
}

/**
 * 构建主子表子实体元数据（fieldName / childPascal / kebab 等）
 * @param {string} fieldName
 * @param {string} childPascal
 * @param {string} [childTypeOverride]
 */
function buildMasterDetailChildMeta(fieldName, childPascal, childTypeOverride, modulePath = '', foreignKeyOnChild = '') {
  const { resolveChildEntityFrontendKebab } = require('./generate-master-detail-associations.cjs');
  const childCamel = pascalToCamel(childPascal);
  const childType = childTypeOverride || childPascal;
  const childKebab = resolveChildEntityFrontendKebab(childPascal, normalizeModulePath(modulePath));
  return {
    fieldName,
    childPascal,
    childCreateType: `${childPascal}Create`,
    childType,
    childCamel,
    childI18nSlug: entityClassToSlug(`Takt${childPascal}`),
    childKebab,
    childIdField: `${childCamel}Id`,
    /** Domain OneToMany nameof 外键（PascalCase，如 OriginalEmployeeId）；空则由 types 推断 */
    foreignKeyOnChild: foreignKeyOnChild || '',
  };
}

/**
 * 检测 Create DTO 中的 OneToMany 子表（子 Create 类型可在独立 .d.ts 中）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @param {string} [modulePath]
 * @returns {Array<{ fieldName: string, childPascal: string, childCreateType: string, childType: string, childCamel: string, childKebab: string, childIdField: string }>}
 */
function detectMasterDetailChildren(interfaces, entityPascal, modulePath = '') {
  const createIface = interfaces.get(`${entityPascal}Create`);
  const entityIface = interfaces.get(entityPascal);
  if (!createIface) {
    return [];
  }
  /** @type {Array<{ fieldName: string, childPascal: string, childCreateType: string, childType: string, childCamel: string, childKebab: string, childIdField: string }>} */
  const children = [];
  createIface.properties.forEach((prop) => {
    const elementType = parseArrayElementType(prop.type);
    if (!elementType || !elementType.endsWith('Create')) {
      return;
    }
    const childPascal = elementType.slice(0, -'Create'.length);
    if (!childPascal || childPascal === entityPascal) {
      return;
    }
    if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(childPascal)) {
      return;
    }
    const hasCreateType = interfaces.has(elementType) || childTypesFileExists(modulePath, childPascal);
    if (!hasCreateType) {
      return;
    }
    const entityProp = entityIface?.properties.find((p) => p.name === prop.name);
    const entityElement = entityProp ? parseArrayElementType(entityProp.type) : null;
    const isChildEntity = Boolean(
      entityElement
      && entityElement !== entityPascal
      && (interfaces.has(entityElement) || childTypesFileExists(modulePath, childPascal)),
    );
    const docHint = /子表|一对多|级联|外键|关联|主子表/.test(`${prop.doc || ''}${entityProp?.doc || ''}`);
    if (!isChildEntity && !docHint) {
      return;
    }
    children.push(buildMasterDetailChildMeta(prop.name, childPascal, entityElement || childPascal, modulePath));
  });
  return children;
}

/**
 * 从 Domain 实体 OneToMany 解析主子表子实体（权威来源，对齐字典 DictType ↔ DictData）
 * @param {string} entityPascal
 * @param {string} [modulePath]
 * @param {ReturnType<typeof parseTypeInterfaces>} [interfaces]
 */
function detectMasterDetailChildrenFromEntity(entityPascal, modulePath = '', interfaces = new Map()) {
  const entityFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!entityFile) {
    return [];
  }
  const fromTypes = detectMasterDetailChildren(interfaces, entityPascal, modulePath);
  return parseOneToManyNavigations(entityFile)
    .filter((nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort))
    .map((nav) => {
      const typesChild = fromTypes.find((c) => c.childPascal === nav.childShort);
      const fieldName = typesChild?.fieldName || pascalToCamel(nav.navPropName);
      return buildMasterDetailChildMeta(
        fieldName,
        nav.childShort,
        typesChild?.childType,
        modulePath,
        nav.foreignKeyOnChild,
      );
    });
}

/**
 * 合并 Domain 实体与 types 的主子表子实体列表（实体 OneToMany 优先）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} entityPascal
 * @param {string} modulePath
 */
function resolveMasterDetailChildren(interfaces, entityPascal, modulePath) {
  const fromEntity = detectMasterDetailChildrenFromEntity(entityPascal, modulePath, interfaces);
  if (fromEntity.length) {
    return fromEntity;
  }
  return detectMasterDetailChildren(interfaces, entityPascal, modulePath);
}

/**
 * 主子表关联视图目录：与主表 view 平级，落在同一 module 目录下（如 logistics/maintenance/work-order）
 * @param {string} masterViewModulePath 主表 views 路径（如 logistics/maintenance/equipment）
 * @param {{ viewChildKebab?: string, childKebab?: string }} child
 * @param {string} [modulePath] API/types 模块目录（如 logistics/maintenance）
 * @returns {string}
 */
function resolveMasterDetailChildViewModulePath(masterViewModulePath, child, modulePath = '') {
  const segment = child.viewChildKebab || child.childKebab;
  if (!segment) {
    throw new Error('[generate-vue] 子表缺少 viewChildKebab / childKebab，无法拆分主子视图');
  }
  const parentDir = normalizeModulePath(modulePath)
    || normalizeModulePath(path.dirname(masterViewModulePath));
  if (!parentDir || parentDir === '.') {
    return segment;
  }
  return `${parentDir}/${segment}`;
}

/**
 * 子表 view 目录（module + viewChildKebab，与菜单 ComponentPath 末段对齐）
 * @param {object} child
 * @param {string} modulePath
 * @returns {string}
 */
function resolveChildMenuViewPath(child, modulePath) {
  const normalized = normalizeModulePath(modulePath);
  if (!normalized) {
    return '';
  }
  const childKebab = child.viewChildKebab || child.childKebab;
  if (!childKebab) {
    return '';
  }
  const viewEntityKebab = stripModulePrefixFromEntityKebab(childKebab, normalized);
  return `${normalized}/${viewEntityKebab}`;
}

/**
 * 主菜单为独立单表 CRUD、各子表菜单为「主表 + 该子表明细」子导航主子的主实体
 * @param {string} [masterPascal]
 * @returns {boolean}
 */
function isChildNavMasterDetailMaster(masterPascal) {
  return masterPascal === 'Employee' || masterPascal === 'News';
}

/**
 * 子实体是否已有独立实体菜单页（普通主子表：有菜单则不计入主表主子规划）
 * 特例：主表为人事 Employee / 日常 News 时，子表即使有菜单仍参与规划（生成「子导航主子」）
 * @param {object} child
 * @param {string} modulePath
 * @param {string} [masterPascal] 主表短名（如 Employee、News）
 * @returns {boolean}
 */
function childHasStandaloneMenu(child, modulePath, masterPascal) {
  // Employee / News：多子表 + 各子表独立菜单 → 仍由主表生成子导航主子视图（须先于 STANDALONE 判断）
  if (isChildNavMasterDetailMaster(masterPascal)) {
    return false;
  }
  if (isStandaloneChildVueEntity(child.childPascal)) {
    return true;
  }
  const childKebab = child.viewChildKebab || child.childKebab;
  const menuByKebab = childKebab ? MENU_INDEX.get(childKebab) : null;
  if (menuByKebab?.componentPath) {
    return !shouldExcludeVueGeneration('', child.childPascal);
  }
  const menuPath = resolveChildMenuViewPath(child, modulePath);
  if (!menuPath || !MENU_INDEX.has(menuPath)) {
    return false;
  }
  return !shouldExcludeVueGeneration('', child.childPascal);
}

/**
 * 过滤已有独立实体菜单的子表（不计入主表主子视图规划；Employee 主表除外）
 * @param {object[]} masterDetailChildren
 * @param {string} modulePath
 * @param {string} [masterPascal]
 * @returns {object[]}
 */
function filterStandaloneMenuChildren(masterDetailChildren, modulePath, masterPascal) {
  return (masterDetailChildren || []).filter(
    (child) => !childHasStandaloneMenu(child, modulePath, masterPascal),
  );
}

/**
 * 按菜单 ComponentPath 规划主子视图（导航数 = 视图目录数，无额外单表目录）
 * - 主菜单 viewModulePath：绑定「尚无独立 ComponentPath 菜单」的首个子表（OneToMany 顺序）
 * - 其余菜单：ComponentPath 与子表 viewChildKebab 路径一致则各生成 1 个主子视图
 * - 特例 Employee / News：有独立菜单的子表仍计入规划（仅子导航主子；主菜单页本身为单表 CRUD）
 * @param {string} masterViewModulePath
 * @param {string} modulePath
 * @param {object[]} masterDetailChildren
 * @param {string} [masterPascal]
 * @returns {Array<{ viewModulePath: string, childMeta: object }>}
 */
function resolveMasterDetailViewPlans(masterViewModulePath, modulePath, masterDetailChildren, masterPascal) {
  const eligibleChildren = filterStandaloneMenuChildren(masterDetailChildren, modulePath, masterPascal);
  if (!eligibleChildren.length) {
    return [];
  }
  /** @type {Array<{ viewModulePath: string, childMeta: object }>} */
  const plans = [];
  const reservedPaths = new Set();
  eligibleChildren.forEach((child) => {
    const childViewPath = resolveChildMenuViewPath(child, modulePath);
    if (!childViewPath || childViewPath === masterViewModulePath) {
      return;
    }
    if (MENU_INDEX.has(childViewPath)) {
      plans.push({ viewModulePath: childViewPath, childMeta: child });
      reservedPaths.add(childViewPath);
      return;
    }
    console.log(
      `  ⏭️  跳过子导航视图（无菜单 ComponentPath=${childViewPath}/index）: Takt${child.childPascal}`,
    );
  });
  const defaultChild = eligibleChildren.find(
    (child) => !MENU_INDEX.has(resolveChildMenuViewPath(child, modulePath)),
  ) || eligibleChildren[0];
  // Employee / News：主菜单页为独立单表 CRUD，不绑定主子；仅各子表菜单生成「子导航主子」
  if (isChildNavMasterDetailMaster(masterPascal)) {
    return plans.filter((plan, index, arr) => arr.findIndex(
      (item) => item.viewModulePath === plan.viewModulePath,
    ) === index);
  }
  if (masterViewModulePath) {
    plans.unshift({ viewModulePath: masterViewModulePath, childMeta: defaultChild });
  }
  return plans.filter((plan, index, arr) => arr.findIndex(
    (item) => item.viewModulePath === plan.viewModulePath,
  ) === index);
}

/**
 * 解析某一子实体应对应的 views 目录（与 resolveMasterDetailViewPlans 一致）
 * @param {string} masterViewModulePath
 * @param {string} modulePath
 * @param {object[]} masterDetailChildren
 * @param {object} childMeta
 * @returns {string}
 */
function resolveMasterDetailChildViewPath(masterViewModulePath, modulePath, masterDetailChildren, childMeta, masterPascal) {
  const plans = resolveMasterDetailViewPlans(
    masterViewModulePath,
    modulePath,
    masterDetailChildren,
    masterPascal,
  );
  const hit = plans.find((plan) => plan.childMeta.childPascal === childMeta.childPascal);
  if (hit) {
    return hit.viewModulePath;
  }
  return resolveMasterDetailChildViewModulePath(masterViewModulePath, childMeta, modulePath);
}

/**
 * 校验 Domain OneToMany 导航数量与已解析子表一致（以实体导航为权威）
 * @param {string} entityPascal
 * @param {string} modulePath
 * @param {object[]} children
 * @param {ReturnType<typeof parseTypeInterfaces>} [interfaces]
 */
function validateMasterDetailChildrenAlignment(entityPascal, modulePath, children, interfaces = new Map()) {
  const entityFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!entityFile) {
    return;
  }
  const navs = parseOneToManyNavigations(entityFile).filter(
    (nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort),
  );
  if (!navs.length) {
    return;
  }
  const navChildSet = new Set(navs.map((nav) => nav.childShort));
  const childSet = new Set((children || []).map((c) => c.childPascal));
  if (navs.length !== (children || []).length) {
    console.warn(
      `⚠️  实体 Takt${entityPascal} OneToMany 导航 ${navs.length} 个（${[...navChildSet].join(', ')}），` +
      `已解析子表 ${(children || []).length} 个（${[...childSet].join(', ') || '无'}）`,
    );
  }
  navs.forEach((nav) => {
    if (!childSet.has(nav.childShort)) {
      console.warn(`⚠️  缺少子表元数据: ${nav.navPropName} → Takt${nav.childShort}（types/Create DTO 未对齐）`);
    }
  });
  (children || []).forEach((child) => {
    if (!navChildSet.has(child.childPascal)) {
      console.warn(`⚠️  子表 Takt${child.childPascal} 无对应实体 OneToMany 导航（${child.fieldName}）`);
    }
  });
  const { validateMasterDetailChildrenManyToOnePairs } = require('./generate-master-detail-associations.cjs');
  validateMasterDetailChildrenManyToOnePairs(entityPascal, children);
}

/**
 * 复制 fields 并替换 masterDetailChildren
 * @param {object} fields
 * @param {object[]} masterDetailChildren
 * @returns {object}
 */
function cloneFieldMetaWithMasterDetailChildren(fields, masterDetailChildren) {
  return { ...fields, masterDetailChildren };
}

/**
 * 子表 Query 中指向主表的外键字段（默认 {masterCamel}Id）
 * 优先：Domain OneToMany 外键 → types「主子表关系」注释 → {master}Id/{master}Code
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} childPascal
 * @param {string} masterCamel
 * @param {string} [foreignKeyOnChildPascal] 实体 nameof 外键（PascalCase，如 OriginalEmployeeId）
 */
function resolveChildMasterFkField(interfaces, childPascal, masterCamel, foreignKeyOnChildPascal = '') {
  const create = interfaces.get(`${childPascal}Create`);
  const query = interfaces.get(`${childPascal}Query`);
  const hasProp = (name) =>
    Boolean(
      name
      && ((query?.properties || []).some((p) => p.name === name)
        || (create?.properties || []).some((p) => p.name === name)),
    );
  if (foreignKeyOnChildPascal) {
    const fromEntity = pascalToCamel(foreignKeyOnChildPascal);
    if (hasProp(fromEntity)) {
      return fromEntity;
    }
  }
  const pickMasterFkFromDoc = (properties) => {
    const hit = (properties || []).find((p) => /主子表关系/.test(p.doc || ''));
    return hit?.name || '';
  };
  const docFk = pickMasterFkFromDoc(create?.properties) || pickMasterFkFromDoc(query?.properties);
  if (docFk) {
    return docFk;
  }
  const fkId = `${masterCamel}Id`;
  if (hasProp(fkId)) {
    return fkId;
  }
  const fkCode = `${masterCamel}Code`;
  if (hasProp(fkCode)) {
    return fkCode;
  }
  return fkId;
}

/**
 * 解析前端 API 文件绝对路径（优先 camelCase.api.ts，兼容历史 *.ts）
 * @param {string} modulePath 相对 src/api 的目录
 * @param {string} fileKebabOrCamel 实体文件主名
 * @returns {string|null}
 */
function resolveFrontendApiAbsolutePath(modulePath, fileKebabOrCamel) {
  const apiRoot = path.join(CONFIG.frontendRoot, CONFIG.apiDir, modulePath || '');
  const preferred = path.join(apiRoot, toFrontendApiFileName(fileKebabOrCamel));
  if (fs.existsSync(preferred)) {
    return preferred;
  }
  const camelLegacy = path.join(apiRoot, `${toFrontendEntityFileCamel(fileKebabOrCamel)}.ts`);
  if (fs.existsSync(camelLegacy)) {
    return camelLegacy;
  }
  const rawLegacy = path.join(apiRoot, `${fileKebabOrCamel}.ts`);
  if (fs.existsSync(rawLegacy) && rawLegacy !== camelLegacy) {
    return rawLegacy;
  }
  return null;
}

/**
 * 解析子表 list API 方法名
 * @param {string} modulePath
 * @param {string} childKebab
 * @param {string} childPascal
 */
function resolveChildListApiMethod(modulePath, childKebab, childPascal) {
  const apiPath = resolveFrontendApiAbsolutePath(modulePath, childKebab);
  if (!apiPath) {
    return '';
  }
  const { methods } = parseApiFile(fs.readFileSync(apiPath, 'utf-8'));
  const expected = `get${childPascal}List`;
  return Object.prototype.hasOwnProperty.call(methods, expected) ? expected : '';
}

/**
 * 递归收集 types 目录下全部业务实体 .types.ts（不含 common / shim .d.ts）
 * @param {string} [rootDir]
 */
function collectAllTypesFiles(rootDir = path.join(CONFIG.frontendRoot, CONFIG.typesDir)) {
  /** @type {string[]} */
  const files = [];
  /** @param {string} dir */
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.endsWith('.types.ts') && entry.name !== 'common.types.ts') {
        files.push(full);
      }
    }
  }
  if (fs.existsSync(rootDir)) {
    scan(rootDir);
  }
  return files;
}

/**
 * 递归收集 Domain Entities 下全部 Takt*.cs
 * @param {string} [rootDir]
 */
function collectDomainEntityFiles(rootDir = path.join(CONFIG.backendRoot, 'Takt.Domain', 'Entities')) {
  /** @type {string[]} */
  const files = [];
  if (!fs.existsSync(rootDir)) {
    return files;
  }
  /** @param {string} dir */
  function scan(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        scan(full);
      } else if (entry.name.startsWith('Takt') && entry.name.endsWith('.cs')) {
        files.push(full);
      }
    }
  }
  scan(rootDir);
  return files;
}

/**
 * 扫描全部 types + Domain 实体，建立「从实体 → 主实体」映射（从实体默认不单独生成单表 Vue；关联主子视图落在 module 平级目录）
 * 特例：人事 Employee* 子表已由 Employee 认领后，禁止其它主表（如 TalentOffer→EmployeeOnboarding）覆盖
 * @returns {Map<string, { masterPascal: string, fieldName: string }>}
 */
function buildMasterDetailChildRegistry() {
  /** @type {Map<string, { masterPascal: string, fieldName: string }>} */
  const registry = new Map();
  /**
   * @param {string} childShort
   * @param {string} masterPascal
   * @param {string} fieldName
   */
  function claimChild(childShort, masterPascal, fieldName) {
    const existing = registry.get(childShort);
    // 仅人事：Employee 已认领的 Employee* 子表不被其它主表覆盖
    if (
      existing?.masterPascal === 'Employee' &&
      childShort.startsWith('Employee') &&
      masterPascal !== 'Employee'
    ) {
      return;
    }
    registry.set(childShort, { masterPascal, fieldName });
  }
  collectDomainEntityFiles().forEach((entityFile) => {
    const content = fs.readFileSync(entityFile, 'utf-8');
    const classMatch = content.match(/public\s+(?:sealed\s+)?class\s+Takt(\w+)\s*:/);
    if (!classMatch) {
      return;
    }
    const masterPascal = classMatch[1];
    parseOneToManyNavigations(entityFile).forEach((nav) => {
      if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort)) {
        return;
      }
      claimChild(nav.childShort, masterPascal, pascalToCamel(nav.navPropName));
    });
  });
  for (const typesFile of collectAllTypesFiles()) {
    const relTypes = path.relative(path.join(CONFIG.frontendRoot, CONFIG.typesDir), typesFile).replace(/\\/g, '/');
    const modulePath = path.dirname(relTypes);
    const entityCamel = parseFrontendTypesFileCamel(typesFile);
    const entityKebab = camelToKebab(entityCamel);
    const content = fs.readFileSync(typesFile, 'utf-8');
    const ifaceMap = parseTypeInterfaces(content);
    const entityPascal = resolveMainEntityPascalFromTypes(ifaceMap) || kebabToPascal(entityKebab);
    if (!ifaceMap.has(entityPascal)) {
      continue;
    }
    resolveMasterDetailChildren(ifaceMap, entityPascal, modulePath === '.' ? '' : modulePath).forEach((child) => {
      if (!registry.has(child.childPascal)) {
        registry.set(child.childPascal, {
          masterPascal: entityPascal,
          fieldName: child.fieldName,
        });
      }
    });
  }
  return registry;
}

/**
 * 子表 Create 表单字段（排除主表外键与审计字段）
 * @param {ReturnType<typeof parseTypeInterfaces>} interfaces
 * @param {string} childPascal
 * @param {string} masterCamel
 * @param {string} [foreignKeyOnChildPascal]
 */
function buildChildFormFieldProps(interfaces, childPascal, masterCamel, foreignKeyOnChildPascal = '') {
  const create = interfaces.get(`${childPascal}Create`);
  if (!create) {
    return [];
  }
  const masterFkField = resolveChildMasterFkField(
    interfaces,
    childPascal,
    masterCamel,
    foreignKeyOnChildPascal,
  );
  const masterFkNames = new Set([`${masterCamel}Id`, `${masterCamel}Code`, masterFkField]);
  const scopeFields = buildScopeFormFields(create.properties || [], childPascal);
  const scopeNames = new Set(scopeFields.map((f) => f.name));
  const rest = (create.properties || []).filter((p) => {
    if (scopeNames.has(p.name) || SKIP_FORM_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (p.name === `${pascalToCamel(childPascal)}Id`) {
      return false;
    }
    if (masterFkNames.has(p.name)) {
      return false;
    }
    if (FORM_TAB_TRAILING_FIELD_NAME_SET.has(p.name)) {
      return false;
    }
    return true;
  });
  return [...scopeFields, ...rest];
}

/**
 * 列表列跳过字段（Tenant/Company 管理页保留隔离字段列）
 * @param {string} entityPascal
 * @returns {Set<string>}
 */
function getSkipListFields(entityPascal) {
  const skip = new Set(SKIP_LIST_FIELDS);
  const entityScope = resolveEntityScope(entityPascal, '', CONFIG.backendRoot);
  if (entityScope === 'tenant-core' || entityScope === 'tenant-plant') {
    skip.delete('cultureCode');
  }
  if (entityPascal === 'Tenant') {
    skip.delete('tenantCode');
  }
  if (entityPascal === 'Company') {
    skip.delete('tenantCode');
    skip.delete('companyCode');
  }
  return skip;
}

/**
 * 构建实体字段元数据
 * @param {ReturnType<typeof parseTypeInterfaces>} ifaceMap
 * @param {string} entityPascal
 * @param {string} [typesFileContent]
 */
function buildFieldMeta(ifaceMap, entityPascal, typesFileContent = '', modulePath = '') {
  const normalizedModulePath = normalizeModulePath(modulePath);
  const skipListFields = getSkipListFields(entityPascal);
  const entityScope = resolveEntityScope(entityPascal, typesFileContent, CONFIG.backendRoot);
  const entity = ifaceMap.get(entityPascal);
  const create = ifaceMap.get(`${entityPascal}Create`);
  const query = ifaceMap.get(`${entityPascal}Query`);
  const masterDetailChildren = resolveMasterDetailChildren(ifaceMap, entityPascal, normalizedModulePath);
  const childFieldNames = new Set(masterDetailChildren.map((c) => c.fieldName));
  const entityCamel = pascalToCamel(entityPascal);
  const entityI18nSlug = entityClassToSlug(`Takt${entityPascal}`);
  const inputMaxLengths = loadEntityInputMaxLengths(entityPascal);
  const enrich = (fields, i18nSlug, metaEntityPascal = entityPascal) => {
    const dateTimeFields = loadEntityDateTimeFields(metaEntityPascal);
    return fields.map((f) => {
      const dictType = resolveDictType(f);
      const isDateTime = dateTimeFields.has(f.name);
      const apiUrl = resolveOptionsApiUrl(f);
      return attachListSwitchMeta({
        ...f,
        dictType,
        isDateTime,
        apiUrl,
        htmlType: inferHtmlType({ ...f, dictType, isDateTime, apiUrl }),
        i18nKey: resolveFieldTranslationKey(f.name, i18nSlug),
        maxLength: inputMaxLengths.get(f.name) ?? DEFAULT_A_INPUT_MAX_LENGTH,
      });
    });
  };
  const enrichFormFields = (fields, i18nSlug, metaEntityPascal = entityPascal) => {
    const dateTimeFields = loadEntityDateTimeFields(metaEntityPascal);
    const mapped = fields.map((f) => {
      const derived = isEntityDerivedFormField(f.doc);
      const dictType = derived ? '' : resolveDictType(f);
      const isDateTime = dateTimeFields.has(f.name);
      const apiUrl = resolveOptionsApiUrl(f);
      const base = {
        ...f,
        dictType,
        isDateTime,
        apiUrl,
        readOnly: Boolean(f.readOnly || derived),
      };
      return attachListSwitchMeta({
        ...base,
        htmlType: inferHtmlType(base),
        i18nKey: resolveFieldTranslationKey(f.name, i18nSlug),
        maxLength: inputMaxLengths.get(f.name) ?? DEFAULT_A_INPUT_MAX_LENGTH,
      });
    });
    return applyTaktNumberingFormMeta(applyBusinessFileUploadMeta(mapped, metaEntityPascal), metaEntityPascal);
  };
  const listFields = (entity?.properties || []).filter((p) => {
    if (skipListFields.has(p.name) || childFieldNames.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (isDtoNavigationProperty(p)) {
      return false;
    }
    return true;
  });
  const scopeFields = buildScopeFormFields(create?.properties || [], entityPascal);
  const scopeNames = new Set(scopeFields.map((f) => f.name));
  const formFieldsRaw = (create?.properties || []).filter((p) => {
    if (scopeNames.has(p.name) || SKIP_FORM_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (p.name === `${pascalToCamel(entityPascal)}Id`) {
      return false;
    }
    if (childFieldNames.has(p.name)) {
      return false;
    }
    return true;
  });
  const queryFieldsRaw = (query?.properties || []).filter((p) => {
    if (SKIP_QUERY_FIELDS.has(p.name)) {
      return false;
    }
    if (isDtoFillField(p.doc)) {
      return false;
    }
    if (childFieldNames.has(p.name)) {
      return false;
    }
    return true;
  });
  const queryFields = enrich(queryFieldsRaw, entityI18nSlug);
  const enrichedChildren = masterDetailChildren.map((child) => {
    const { resolveChildEntityFrontendKebab } = require('./generate-master-detail-associations.cjs');
    const childFileKebab = resolveChildEntityFrontendKebab(child.childPascal, normalizedModulePath);
    const childDtsFile = path.join(CONFIG.frontendRoot, CONFIG.typesDir, normalizedModulePath, toFrontendTypesFileName(childFileKebab));
    const childDtsText = fs.existsSync(childDtsFile) ? fs.readFileSync(childDtsFile, 'utf-8') : '';
    const childIfaceMap = childDtsText ? parseTypeInterfaces(childDtsText) : ifaceMap;
    const childFormRaw = buildChildFormFieldProps(
      childIfaceMap,
      child.childPascal,
      entityCamel,
      child.foreignKeyOnChild || '',
    );
    const childEntity = ifaceMap.get(child.childType) || childIfaceMap.get(child.childType);
    const childListRaw = (childEntity?.properties || []).filter((p) => {
      if (SKIP_LIST_FIELDS.has(p.name)) {
        return false;
      }
      if (isDtoFillField(p.doc)) {
        return false;
      }
      if (isDtoNavigationProperty(p)) {
        return false;
      }
      if (p.name === child.childIdField) {
        return false;
      }
      return true;
    });
    const childI18nSlug = child.childI18nSlug || entityClassToSlug(`Takt${child.childPascal}`);
    const masterFkField = resolveChildMasterFkField(
      childIfaceMap,
      child.childPascal,
      entityCamel,
      child.foreignKeyOnChild || '',
    );
    const masterFkNames = new Set([masterFkField, `${entityCamel}Id`]);
    const childQuery = childIfaceMap.get(`${child.childPascal}Query`);
    const childQueryRaw = (childQuery?.properties || []).filter((p) => {
      if (SKIP_QUERY_FIELDS.has(p.name)) {
        return false;
      }
      if (isDtoFillField(p.doc)) {
        return false;
      }
      if (masterFkNames.has(p.name)) {
        return false;
      }
      return true;
    });
    const childApiPath = resolveFrontendApiAbsolutePath(normalizedModulePath, childFileKebab);
    const childCaps = childApiPath
      ? detectApiCapabilities(child.childPascal, parseApiFile(fs.readFileSync(childApiPath, 'utf-8')).methods)
      : detectApiCapabilities(child.childPascal, {});
    return {
      ...child,
      childKebab: childFileKebab,
      viewChildKebab: childFileKebab,
      masterFkField,
      childI18nSlug,
      childCaps,
      childEntityClassName: `Takt${child.childPascal}`,
      apiGetList: resolveChildListApiMethod(normalizedModulePath, childFileKebab, child.childPascal),
      formFields: enrichFormFields(childFormRaw, childI18nSlug, child.childPascal),
      listFields: enrich(childListRaw, childI18nSlug, child.childPascal),
      queryFields: enrich(childQueryRaw, childI18nSlug, child.childPascal),
    };
  });
  return {
    listFields: enrich(listFields, entityI18nSlug),
    formFields: enrichFormFields([...scopeFields, ...formFieldsRaw], entityI18nSlug),
    queryFields: enrich(queryFields, entityI18nSlug),
    masterDetailChildren: enrichedChildren,
    entityScope,
  };
}

/**
 * 解析模块上下文
 * @param {string} apiFilePath
 * @param {string} entityShort
 * @param {{ viewPath?: string|null }} overrides
 */
function resolveModuleContext(apiFilePath, entityShort, overrides) {
  const relApi = path.relative(path.join(CONFIG.frontendRoot, CONFIG.apiDir), apiFilePath).replace(/\\/g, '/');
  // 先剥扩展名；文件主名必须保留 camelCase（勿对含文件名的路径调用 normalizeModulePath）
  const relStripped = relApi
    .replace(/\.api\.ts$/i, '')
    .replace(/\.types\.ts$/i, '')
    .replace(/\.ts$/i, '');
  const fileStem = path.basename(relStripped);
  const parentModulePath = normalizeModulePath(
    path.dirname(relStripped) === '.' ? '' : path.dirname(relStripped),
  );
  const entityPascal = entityShort;
  const entityCamel = pascalToCamel(entityPascal);
  // 与 generate-from-backend 一致：api/types 文件主名用实体 camel（systemProperty），不用 i18n slug
  const apiFileCamel = entityCamel || toFrontendEntityFileCamel(fileStem);
  const canonicalKebab = pascalToKebab(entityPascal);
  const viewEntityKebab = stripModulePrefixFromEntityKebab(canonicalKebab, parentModulePath);
  const moduleViewPathKey = resolveViewModulePath(parentModulePath, viewEntityKebab);
  const menu = MENU_INDEX.get(moduleViewPathKey)
    || MENU_INDEX.get(normalizeModulePath(relStripped))
    || MENU_INDEX.get(viewEntityKebab)
    || MENU_INDEX.get(canonicalKebab)
    || MENU_INDEX.get(camelToKebab(apiFileCamel));
  const viewModulePath = overrides.viewPath
    || (menu?.componentPath ? menu.componentPath.replace(/\/index$/, '') : '')
    || moduleViewPathKey;
  const outputRel = resolveFrontendOutputRelPath(parentModulePath, canonicalKebab);
  const modulePath = outputRel.importPath.includes('/')
    ? outputRel.importPath.split('/').slice(0, -1).join('/')
    : '';
  const resolvedFileCamel = toFrontendEntityFileCamel(outputRel.file) || apiFileCamel;
  let permissionPrefix = resolvePermissionPrefixFromController(entityShort) || menu?.permissionPrefix;
  if (!permissionPrefix) {
    permissionPrefix = `${modulePath.replace(/\//g, ':')}:${resolvedFileCamel}`;
  }
  const entityI18nSlug = entityClassToSlug(`Takt${entityPascal}`);
  return {
    modulePath,
    viewModulePath,
    entityKebab: resolvedFileCamel,
    viewEntityKebab,
    entityPascal,
    entityCamel,
    entityI18nSlug,
    entitySlug: entityCamel,
    permissionPrefix,
    cssRootClass: viewModulePath.replace(/\//g, '-'),
    apiImportRel: toFrontendApiImportPath(
      modulePath ? `${modulePath}/${resolvedFileCamel}` : resolvedFileCamel,
    ),
  };
}

/**
 * 从 SugarColumn 特性解析字符串最大长度
 * @param {string} blockBeforeProperty
 * @returns {number | null}
 */
function parseSugarColumnMaxLength(blockBeforeProperty) {
  const matches = [...blockBeforeProperty.matchAll(/\[SugarColumn\(([\s\S]*?)\)\]/g)];
  if (matches.length === 0) {
    return null;
  }
  const attrs = matches[matches.length - 1][1];
  const lengthMatch = attrs.match(/Length\s*=\s*(\d+)/);
  return lengthMatch ? parseInt(lengthMatch[1], 10) : null;
}

/**
 * 读取 Domain 实体 DateTime/DateOnly 列（camelCase 字段名集合）
 * @param {string} entityPascal
 * @returns {Set<string>}
 */
function loadEntityDateTimeFields(entityPascal) {
  /** @type {Set<string>} */
  const set = new Set();
  const entityFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!entityFile) {
    return set;
  }
  const content = fs.readFileSync(entityFile, 'utf-8');
  const classOpen = content.search(/public\s+class\s+Takt\w+/);
  if (classOpen < 0) {
    return set;
  }
  const braceStart = content.indexOf('{', classOpen);
  if (braceStart < 0) {
    return set;
  }
  let depth = 1;
  let i = braceStart + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
    }
    i += 1;
  }
  const classBody = content.slice(braceStart + 1, i - 1);
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>(?:[\s\S]*?)?([\w.?[\]]+)\s+(\w+)\s*\{[\s\S]*?get;\s*set;/g;
  let match;
  while ((match = propertyRegex.exec(classBody)) !== null) {
    if (/\[Navigate\s*\(/.test(match[0])) {
      continue;
    }
    const csharpType = match[2].trim().replace('?', '');
    if (csharpType !== 'DateTime' && csharpType !== 'DateOnly') {
      continue;
    }
    set.add(pascalToCamel(match[3]));
  }
  return set;
}

/**
 * 读取 Domain 实体字符串字段 maxLength（camelCase 字段名 → 长度）
 * @param {string} entityPascal
 * @returns {Map<string, number>}
 */
function loadEntityInputMaxLengths(entityPascal) {
  /** @type {Map<string, number>} */
  const map = new Map();
  const entityFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!entityFile) {
    return map;
  }
  const content = fs.readFileSync(entityFile, 'utf-8');
  const classOpen = content.search(/public\s+class\s+Takt\w+/);
  if (classOpen < 0) {
    return map;
  }
  const braceStart = content.indexOf('{', classOpen);
  if (braceStart < 0) {
    return map;
  }
  let depth = 1;
  let i = braceStart + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
    }
    i += 1;
  }
  const classBody = content.slice(braceStart + 1, i - 1);
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>(?:[\s\S]*?)?([\w.?[\]]+)\s+(\w+)\s*\{[\s\S]*?get;\s*set;/g;
  let match;
  while ((match = propertyRegex.exec(classBody)) !== null) {
    if (/\[Navigate\s*\(/.test(match[0])) {
      continue;
    }
    const csharpType = match[2].trim();
    if (!csharpType.replace('?', '').includes('string')) {
      continue;
    }
    const name = match[3];
    const publicDecl = `public ${csharpType} ${name}`;
    const publicIndex = classBody.indexOf(publicDecl, match.index);
    const blockEnd = publicIndex >= 0 ? publicIndex : match.index + match[0].length;
    const block = classBody.slice(Math.max(0, blockEnd - 600), blockEnd);
    const maxLength = parseSugarColumnMaxLength(block);
    if (maxLength != null && maxLength > 0) {
      map.set(pascalToCamel(name), maxLength);
    }
  }
  return map;
}

/**
 * 解析 a-input maxlength（实体列长优先，否则默认 20）
 * @param {{ maxLength?: number }} field
 * @returns {number}
 */
function resolveInputMaxLength(field) {
  if (field?.maxLength != null && field.maxLength > 0) {
    return field.maxLength;
  }
  return DEFAULT_A_INPUT_MAX_LENGTH;
}

/**
 * a-input 字数统计与 maxlength 属性
 * @param {{ maxLength?: number }} field
 * @param {string} indent
 * @returns {string}
 */
function renderAInputLimitAttrs(field, indent) {
  const maxLength = resolveInputMaxLength(field);
  return `\n${indent}  show-count\n${indent}  :maxlength="${maxLength}"`;
}

/**
 * 是否为 remark / extField 固定长文本字段
 * @param {{ name?: string }} field
 * @returns {boolean}
 */
function isFixedLongTextareaField(field) {
  return field?.name === 'remark' || isExtFieldField(field);
}

/** @deprecated 使用 isFixedLongTextareaField */
function isRemarkField(field) {
  return isFixedLongTextareaField(field);
}

/**
 * remark / extField 专用 a-textarea 固定属性（rows=4、show-count、maxlength=400、allow-clear）
 * @param {string} indent
 * @param {{ readOnly?: boolean }} [field]
 * @param {{ includeAllowClear?: boolean }} [options]
 * @returns {string}
 */
function renderFixedLongTextareaAttrs(indent, field = {}, options = {}) {
  const { includeAllowClear = true } = options;
  const readOnlyAttrs = renderReadOnlyControlAttrs(field, indent);
  const clearAttr = includeAllowClear && !field.readOnly ? `\n${indent}  allow-clear` : '';
  return `\n${indent}  :rows="${REMARK_TEXTAREA_ROWS}"
${indent}  show-count
${indent}  :maxlength="${REMARK_TEXTAREA_MAX_LENGTH}"${clearAttr}${readOnlyAttrs}`;
}

/** @deprecated 使用 renderFixedLongTextareaAttrs */
function renderRemarkTextareaAttrs(indent, field = {}, options = {}) {
  return renderFixedLongTextareaAttrs(indent, field, options);
}

/**
 * 生成表单控件模板片段（主子表子行 / 主表共用；禁止 size="small"，与手工 CRUD 表单默认尺寸对齐）
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, optional: boolean }} field
 * @param {string} modelPrefix 如 formState. / record.
 * @param {string} indent
 * @param {{ entityIdField?: string, formDataExpr?: string, rowIdField?: string, rowRecordExpr?: string }} [controlOptions]
 * @returns {string}
 */
function renderFormControl(field, modelPrefix, indent = '                ', controlOptions = {}) {
  const readOnlyAttrs = renderReadOnlyControlAttrs(field, indent);
  const codeEditDisabledAttrs = renderFormCodeEditDisabledAttrs(field, indent, controlOptions);
  const editLockAttrs = readOnlyAttrs || codeEditDisabledAttrs;
  const clearAttr = field.readOnly ? '' : `\n${indent}  allow-clear`;
  if (field.htmlType === 'numberingRule') {
    const docType = String(field.documentType || '').replace(/'/g, "\\'");
    const apiParamsAttr = docType
      ? `\n${indent}  :api-params="{ documentType: '${docType}' }"`
      : '';
    return `${indent}<TaktSelect
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  api-url="TaktNumberings/options"${apiParamsAttr}
${indent}  :placeholder="t('common.page.form.placeholder.selectonly')"
${indent}  :disabled="!!${controlOptions.formDataExpr || 'formData'}?.${controlOptions.entityIdField || 'id'} || loading"
${indent}/>`;
  }
  if (field.htmlType === 'numberingCode') {
    return `${indent}<a-input
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="t('common.page.form.numberingCodePreview')"
${indent}  disabled
${indent}/>`;
  }
  if (field.htmlType === 'apiSelect' && field.apiUrl) {
    const nameValueAttr = /DictValue\s*=\s*(ApsGroupName|ComplaintGroupName|CostGroupName)/i.test(field.doc || '')
      ? `\n${indent}  :field-names="{ label: 'dictLabel', value: 'extLabel' }"`
      : '';
    return `${indent}<TaktSelect
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  api-url="${field.apiUrl}"${renderApiSelectParamsAttr(field, indent)}${nameValueAttr}
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"${editLockAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'select' && field.dictType) {
    return `${indent}<TaktSelect
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  dict-type="${field.dictType}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"${editLockAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'textarea') {
    if (isFixedLongTextareaField(field)) {
      const placeholderExpr = isExtFieldField(field)
        ? fieldExtFieldPlaceholderTExpr()
        : fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional');
      const fixedAttrs = renderFixedLongTextareaAttrs(indent, field);
      return `${indent}<a-textarea
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${placeholderExpr}"${fixedAttrs}${codeEditDisabledAttrs}
${indent}/>`;
    }
    return `${indent}<a-textarea
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"
${indent}  :rows="2"${editLockAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'richEditor') {
    return `${indent}<TaktRichEditor
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"${editLockAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'fileNameReadonly') {
    return `${indent}<a-input
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"${renderAInputLimitAttrs(field, indent)}
${indent}  disabled
${indent}/>`;
  }
  if (field.htmlType === 'fileUpload') {
    return `${indent}<TaktUploadFile
${indent}  tabs-type="files"
${indent}  :files-auto-upload="true"
${indent}  :files-multiple="false"
${indent}  :files-max-count="1"
${indent}  :files-disabled="!!loading || fileUploading"
${indent}  :files-max-size="taktFileMaxSizeMb"
${indent}  :files-accept="taktFileAccept"
${indent}  :files-hint="t('foundation.file.page.upload.hint', { max: taktFileMaxSizeMb })"
${indent}  :files-custom-request="handleFilesCustomRequest"
${indent}  v-model:files-file-list="filesFileList"
${indent}  @files:remove="handleFileRemove"
${indent}/>
${indent}<a-input
${indent}  v-if="${modelPrefix}accessUrl"
${indent}  v-model:value="${modelPrefix}accessUrl"
${indent}  class="mt-2"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional')}"${renderAInputLimitAttrs(field, indent)}
${indent}  disabled
${indent}/>`;
  }
  if (field.htmlType === 'date') {
    const { valueFormat, showTimeAttr } = resolveDatePickerTemplateAttrs(field, indent);
    return `${indent}<a-date-picker
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select')}"
${indent}  value-format="${valueFormat}"${showTimeAttr}
${indent}  style="width: 100%"${editLockAttrs}
${indent}/>`;
  }
  if (field.htmlType === 'switch') {
    return `${indent}<a-switch v-model:checked="${modelPrefix}${field.name}"${editLockAttrs} />`;
  }
  if (field.type === 'number') {
    return `${indent}<a-input-number
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required')}"
${indent}  style="width: 100%"${editLockAttrs}
${indent}/>`;
  }
  return `${indent}<a-input
${indent}  v-model:value="${modelPrefix}${field.name}"
${indent}  :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required')}"${renderAInputLimitAttrs(field, indent)}${clearAttr}${editLockAttrs}
${indent}/>`;
}

/**
 * 生成高级查询抽屉表单项
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, type: string, optional?: boolean }} field
 * @returns {string}
 */
function renderQueryFormItem(field) {
  const body = renderQueryFormItemBody(field);
  return `      <div v-show="isFieldVisible('${field.name}')">
${body}
      </div>`;
}

/**
 * 生成高级查询抽屉表单项内容（不含显隐包裹）
 * @param {{ name: string, htmlType: string, dictType: string, i18nKey: string, type: string, optional?: boolean }} field
 * @returns {string}
 */
function renderQueryFormItemBody(field) {
  if (field.htmlType === 'select' && field.dictType) {
    return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <TaktSelect
          v-model:value="advancedQueryForm.${field.name}"
          dict-type="${field.dictType}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select', 'query')}"
          allow-clear
        />
      </a-form-item>`;
  }
  if (field.htmlType === 'apiSelect' && field.apiUrl) {
    const nameValueAttr = /DictValue\s*=\s*(ApsGroupName|ComplaintGroupName|CostGroupName)/i.test(field.doc || '')
      ? `\n          :field-names="{ label: 'dictLabel', value: 'extLabel' }"`
      : '';
    return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <TaktSelect
          v-model:value="advancedQueryForm.${field.name}"
          api-url="${field.apiUrl}"${renderApiSelectParamsAttr(field, '        ')}${nameValueAttr}
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select', 'query')}"
          allow-clear
        />
      </a-form-item>`;
  }
  if (field.htmlType === 'textarea' || field.htmlType === 'richEditor') {
    const placeholderExpr = isExtFieldField(field)
      ? fieldExtFieldPlaceholderTExpr()
      : fieldPlaceholderTExpr(field, 'common.page.form.placeholder.optional', 'query');
    const fixedLongAttrs = isFixedLongTextareaField(field)
      ? renderFixedLongTextareaAttrs('          ', field)
      : `\n          :rows="2"\n          allow-clear`;
    if (isExtFieldField(field)) {
      return `      <a-form-item
        name="${field.name}"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('${EXT_FIELD_HINT_I18N_KEY}')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ ${fieldLabelTExpr(field, 'query')} }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${placeholderExpr}"${fixedLongAttrs}
        />
      </a-form-item>`;
    }
    return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <a-textarea
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${placeholderExpr}"${fixedLongAttrs}
        />
      </a-form-item>`;
  }
  if (field.type === 'number') {
    return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <a-input-number
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.required', 'query')}"
          style="width: 100%"
        />
      </a-form-item>`;
  }
  if (field.htmlType === 'date') {
    const { valueFormat, showTimeAttr } = resolveDatePickerTemplateAttrs(field, '          ');
    return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <a-date-picker
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, 'common.page.form.placeholder.select', 'query')}"
          value-format="${valueFormat}"${showTimeAttr}
          style="width: 100%"
        />
      </a-form-item>`;
  }
  const placeholderKey = field.htmlType === 'date'
    ? 'common.page.form.placeholder.optional'
    : 'common.page.form.placeholder.required';
  return `      <a-form-item :label="${fieldLabelTExpr(field, 'query')}">
        <a-input
          v-model:value="advancedQueryForm.${field.name}"
          :placeholder="${fieldPlaceholderTExpr(field, placeholderKey, 'query')}"
          show-count
          :maxlength="${resolveInputMaxLength(field)}"
          allow-clear
        />
      </a-form-item>`;
}

/**
 * Domain 实体是否含 ParentId（树形判定，三脚本共用）
 * @param {string} entityPascal
 * @param {string} [backendRoot]
 */
function entityHasParentId(entityPascal, backendRoot = CONFIG.backendRoot) {
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

function writeVueModuleOutputs(bundle, indexContent, formContent, options, i18nComposableContent) {
  const composablePath = bundle.i18nComposablePath;
  if (options.dryRun) {
    const composableHint = i18nComposableContent && composablePath ? `\n  - ${composablePath}` : '';
    console.log(`🔍 [dry-run] 将生成:\n  - ${bundle.indexPath}${formContent ? `\n  - ${bundle.formPath}` : ''}${composableHint}`);
    return { skipped: false, dryRun: true };
  }
  if (i18nComposableContent && composablePath) {
    fs.mkdirSync(path.dirname(composablePath), { recursive: true });
    writeGeneratedFile(composablePath, i18nComposableContent);
    console.log(`✅ 已生成: ${composablePath}`);
  }
  writeGeneratedFile(bundle.indexPath, indexContent);
  console.log(`✅ 已生成: ${bundle.indexPath}`);
  if (formContent) {
    assertGeneratedFormNoSmallSize(formContent, bundle.formPath);
    writeGeneratedFile(bundle.formPath, formContent);
    console.log(`✅ 已生成: ${bundle.formPath}`);
  }
  return { skipped: false, created: true };
}

/**
 * 生成表单模板策略校验（禁止 size="small"、HTML readonly）
 * @param {string} formContent 表单 Vue 源码
 * @param {string} formPath 目标路径（报错用）
 */
function assertGeneratedFormNoSmallSize(formContent, formPath) {
  if (/\bsize\s*=\s*["']small["']/.test(formContent)) {
    throw new Error(
      `[generate-vue] 表单禁止 size="small"（须使用 Ant Design 默认尺寸）: ${formPath}`,
    );
  }
  if (/(^|\n)\s*readonly\s*(\n|$)/.test(formContent)) {
    throw new Error(
      `[generate-vue] 表单禁止 HTML readonly（隔离/只读字段须用 disabled，参照 UserForm.vue）: ${formPath}`,
    );
  }
}

/**
 * 由 API 相对路径加载 types 与 interfaces
 * @param {string} rel 相对 frontend/src/api 的路径（如 logistics/serial/inbound.api.ts）
 * @returns {{ content: string, ifaceMap: ReturnType<typeof parseTypeInterfaces> } | null}
 */
function loadTypesInterfacesForApiRel(rel) {
  const typesRel = toFrontendTypesImportPath(rel.replace(/\.api\.ts$/i, '').replace(/\.ts$/i, ''));
  const dtsFilePath = path.join(CONFIG.frontendRoot, CONFIG.typesDir, `${typesRel}.ts`);
  if (!fs.existsSync(dtsFilePath)) {
    console.warn(`⚠️  缺少类型文件，跳过: ${dtsFilePath}`);
    return null;
  }
  const content = fs.readFileSync(dtsFilePath, 'utf-8');
  return { content, ifaceMap: parseTypeInterfaces(content) };
}

/**
 * 加载 API 模块公共上下文（不含模板过滤）
 */
function loadVueModuleContext(apiFilePath, options, masterDetailChildRegistry) {
  const rel = path.relative(path.join(CONFIG.frontendRoot, CONFIG.apiDir), apiFilePath).replace(/\\/g, '/');
  const entityCamel = parseFrontendApiFileCamel(rel);
  const entityKebab = camelToKebab(entityCamel);
  const typesBundle = loadTypesInterfacesForApiRel(rel);
  if (!typesBundle) {
    return { skipped: true };
  }
  const modulePathFromRel = path.dirname(rel);
  const inferredShort = resolveMainEntityPascalFromTypes(typesBundle.ifaceMap) || kebabToPascal(entityKebab);
  let entityShort = inferredShort;
  if (options.entityPrefix) {
    const prefixKebab = pascalToKebab(options.entityPrefix);
    const shortKebab = resolveFrontendModuleFileName(
      prefixKebab,
      modulePathFromRel === '.' ? '' : modulePathFromRel,
    );
    if (
      inferredShort === options.entityPrefix
      || entityKebab === prefixKebab
      || entityKebab === shortKebab
    ) {
      entityShort = options.entityPrefix;
    }
  }
  const dtoSourceBase = `Takt${entityShort}Dtos`;
  if (shouldExcludeDtoSourceBase(dtoSourceBase)) {
    console.log(`⏭️  跳过手工/排除模块: ${rel}`);
    return { skipped: true };
  }
  if (shouldExcludeVueGeneration(rel, entityShort)) {
    console.log(`⏭️  跳过 Vue 生成（架构约束跳过）: ${rel}`);
    return { skipped: true };
  }
  const normalizedModule = normalizeModulePath(modulePathFromRel === '.' ? '' : modulePathFromRel);
  const canonicalKebab = pascalToKebab(entityShort);
  const viewEntityKebab = stripModulePrefixFromEntityKebab(canonicalKebab, normalizedModule);
  const ownMenuPath = resolveViewModulePath(normalizedModule, viewEntityKebab);
  const menuEntry = MENU_INDEX.get(ownMenuPath)
    || MENU_INDEX.get(viewEntityKebab)
    || MENU_INDEX.get(canonicalKebab)
    || MENU_INDEX.get(entityKebab);
  const hasOwnMenuPage = Boolean(menuEntry?.componentPath);
  const masterRef = masterDetailChildRegistry.get(entityShort);
  // 普通主子表：无独立菜单的从实体跳过（视图由主表承载）；有菜单则按主实体生成独立页
  // 特例 Employee / News 子表：即使有菜单（或 STANDALONE）也跳过单表页，由主表子导航主子写出
  const isChildNavMdChild = Boolean(
    masterRef && isChildNavMasterDetailMaster(masterRef.masterPascal),
  );
  if (
    masterRef &&
    !options.bypassChildRegistrySkip &&
    (isChildNavMdChild || !hasOwnMenuPage)
  ) {
    console.log(
      `⏭️  跳过主子表从实体: ${rel}（视图由主表 ${masterRef.masterPascal}.${masterRef.fieldName} 承载` +
      (isChildNavMdChild && hasOwnMenuPage ? `；${masterRef.masterPascal} 子导航主子` : '') +
      '）',
    );
    return { skipped: true };
  }
  if (masterRef && (isStandaloneChildVueEntity(entityShort) || hasOwnMenuPage) && !isChildNavMdChild) {
    console.log(
      `ℹ️  从实体 ${entityShort} 有独立菜单页（${ownMenuPath}），按主实体生成视图` +
      (masterRef ? `（主表 ${masterRef.masterPascal}.${masterRef.fieldName} 展开区可并存）` : ''),
    );
  }
  const apiContent = fs.readFileSync(apiFilePath, 'utf-8');
  const { methods, apiBase } = parseApiFile(apiContent);
  if (!typesBundle.ifaceMap.has(entityShort)) {
    console.warn(`⚠️  类型文件中未找到主实体 interface ${entityShort}，跳过: ${rel}`);
    return { skipped: true };
  }
  const caps = detectApiCapabilities(entityShort, methods);
  const treeCaps = extendTreeApiCapabilities(entityShort, methods);
  const capsMerged = { ...caps, ...treeCaps };
  const ctx = resolveModuleContext(apiFilePath, entityShort, options);
  const fields = buildFieldMeta(typesBundle.ifaceMap, entityShort, typesBundle.content, ctx.modulePath);
  const comment = parseEntityComment(typesBundle.content, entityShort);
  const create = typesBundle.ifaceMap.get(`${entityShort}Create`);
  const fullCtx = {
    ...ctx,
    caps: capsMerged,
    fields,
    comment,
    apiBase,
    treeMeta: null,
    updateDtoFields: (create?.properties || []).filter(
      (p) => !SKIP_FORM_FIELDS.has(p.name) && !isDtoFillField(p.doc) && p.name !== `${ctx.entitySlug}Id`,
    ),
  };
  const viewDir = path.join(CONFIG.frontendRoot, CONFIG.viewsDir, ctx.viewModulePath);
  const indexPath = path.join(viewDir, 'index.vue');
  const formPath = path.join(viewDir, 'components', `${ctx.entityPascal}Form.vue`);
  const i18nComposablePath = path.join(viewDir, 'composables', entityI18nComposableFileName(ctx.viewEntityKebab));
  const needsForm = capsMerged.hasCreate || capsMerged.hasUpdate;
  return {
    skipped: false,
    rel,
    entityShort,
    fullCtx,
    indexPath,
    formPath,
    i18nComposablePath,
    needsForm,
    ifaceMap: typesBundle.ifaceMap,
    capsMerged,
    isTreeEntity: entityHasParentId(entityShort, CONFIG.backendRoot) && capsMerged.hasGetTree,
    isMasterDetailEntity: filterStandaloneMenuChildren(
      fields.masterDetailChildren,
      ctx.modulePath,
      entityShort,
    ).length > 0,
  };
}

/**
 * 主表 *Form.vue types 导入（仅主表 Create；子表类型由 *ItemForm.vue 独立导入）
 * @param {object} options
 * @param {string} options.entityPascal
 * @param {string} options.entityKebab API/types 文件名（kebab）
 * @param {string} options.modulePath
 * @returns {{ masterTypeImport: string }}
 */
function buildMasterDetailFormTypeImportLines(options) {
  const { entityPascal, entityKebab, modulePath } = options;
  const masterTypeImport = `import type { ${entityPascal}Create } from '@/types/${modulePath}/${toFrontendEntityFileCamel(entityKebab)}.types'`;
  return { masterTypeImport };
}

/**
 * 从 types 文件提取主实体 interface 上方 JSDoc 首行
 * @param {string} content
 * @param {string} entityName
 * @returns {string}
 */
function parseEntityComment(content, entityName) {
  const re = new RegExp(`/\\*\\*([\\s\\S]*?)\\*/\\s*export interface ${entityName}\\b`);
  const match = content.match(re);
  if (!match) {
    return entityName;
  }
  const firstLine = match[1]
    .split(/\r?\n/)
    .map((line) => line.replace(/^\s*\*\s?/, '').trim())
    .find((line) => line && !line.startsWith('@'));
  if (!firstLine) {
    return entityName;
  }
  return firstLine.split(/[，,。；;（(]/)[0].trim() || entityName;
}

/** TaktDictDataSeedData 解析缓存：dictType → IsDefault=1 的 dictValue */
let dictTypeDefaultsCache = null;

/**
 * 从 TaktDictDataSeedData 读取各字典类型默认项（IsDefault=1 的 dictValue）
 * @returns {Record<string, string>}
 */
function getDictTypeDefaultsMap() {
  if (dictTypeDefaultsCache) {
    return dictTypeDefaultsCache;
  }
  /** @type {Record<string, string>} */
  const map = {};
  const seedPath = path.join(
    CONFIG.backendRoot,
    'Takt.Infrastructure',
    'Data',
    'Seeds',
    'EntitySeedData',
    'TaktDictDataSeedData.cs',
  );
  if (!fs.existsSync(seedPath)) {
    dictTypeDefaultsCache = map;
    return map;
  }
  const content = fs.readFileSync(seedPath, 'utf-8');
  const tupleRe = /\("([^"]+)","(?:[^"\\]|\\.)*","([^"]*)","(?:[^"\\]|\\.)*",\d+,\d+,(\d+),/g;
  let match = tupleRe.exec(content);
  while (match) {
    const [, dictType, dictValue, isDefault] = match;
    if (isDefault === '1') {
      map[dictType] = dictValue;
    }
    match = tupleRe.exec(content);
  }
  dictTypeDefaultsCache = map;
  return map;
}

/**
 * 推断表单字段初始默认值（字典 IsDefault + 通用状态/内置）
 * @param {{ name: string, type?: string, doc?: string, dictType?: string, readOnly?: boolean }} field
 * @param {Record<string, string>} dictDefaults
 * @returns {string|number|undefined}
 */
function resolveFormFieldDefaultValue(field, dictDefaults) {
  if (field.readOnly) {
    return undefined;
  }
  if (field.dictType && dictDefaults[field.dictType] !== undefined) {
    const raw = dictDefaults[field.dictType];
    if (field.type === 'number') {
      const num = Number(raw);
      return Number.isFinite(num) ? num : undefined;
    }
    return raw;
  }
  if (field.type === 'number' && isCommonEnableDisableStatus(field.doc)) {
    return 1;
  }
  if (/^isBuiltIn$/i.test(field.name || '')) {
    return 0;
  }
  if (field.dictType === 'sys_yes_no' && /builtin|built_in|isbuiltin/i.test(field.name)) {
    return 0;
  }
  return undefined;
}

/**
 * 生成 *Form.vue：FORM_FIELD_DEFAULTS + applyFormDefaults
 * @param {Array<{ name: string, type?: string, doc?: string, dictType?: string, readOnly?: boolean }>} formFields
 * @returns {string}
 */
function buildFormDefaultsScriptBlock(formFields) {
  const dictDefaults = getDictTypeDefaultsMap();
  const entries = (formFields || [])
    .map((f) => {
      const val = resolveFormFieldDefaultValue(f, dictDefaults);
      if (val === undefined) {
        return null;
      }
      return `  ${f.name}: ${JSON.stringify(val)}`;
    })
    .filter(Boolean);
  if (entries.length === 0) {
    return `/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}`;
  }
  return `/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
${entries.join(',\n')}
}

/** 写入表单默认值（新增 / resetFields / 弹窗再次打开时） */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}`;
}

/**
 * resetPeriod 列表/表单归一化脚本（sys_reset_period；None|Annually|Monthly|Daily）
 * @returns {string}
 */
function buildResetPeriodNormalizerScriptBlock() {
  return `/** resetPeriod：后端 legacy 与字典 dictValue 归一化（None|Annually|Monthly|Daily） */
const RESET_PERIOD_TO_DICT: Record<string, string> = {
  none: 'None',
  annually: 'Annually',
  year: 'Annually',
  yearly: 'Annually',
  monthly: 'Monthly',
  month: 'Monthly',
  daily: 'Daily',
  day: 'Daily',
  hour: 'Daily',
  hourly: 'Daily',
}

/** 编辑回填：归一化为 sys_reset_period dictValue */
function normalizeResetPeriodForForm(value: unknown): string {
  const fallback = String(FORM_FIELD_DEFAULTS.resetPeriod ?? 'None')
  const key = String(value ?? fallback).trim().toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? (typeof value === 'string' && value.trim() ? value.trim() : fallback)
}

/** 提交：与实体 reset_period、字典 sys_reset_period 一致 */
function normalizeResetPeriodForSubmit(value: unknown): string {
  const fallback = String(FORM_FIELD_DEFAULTS.resetPeriod ?? 'None')
  const key = String(value ?? '').trim().toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? (typeof value === 'string' && value.trim() ? value.trim() : fallback)
}
`;
}

/**
 * 生成 *Form.vue：提交前字段归一化（resetPeriod、number 字典项）
 * @param {Array<{ name: string, type?: string, readOnly?: boolean }>} formFields
 * @param {{ useBuildSubmitPayload?: boolean }} [options]
 * @returns {{ script: string, editNormalizeLines: string[], getValuesBody: string }}
 */
function buildFormSubmitNormalizerScriptBlock(formFields, options = {}) {
  const { useBuildSubmitPayload = false, entityIdField = '' } = options;
  const hasResetPeriod = (formFields || []).some((f) => f.name === 'resetPeriod');
  const numberFields = (formFields || []).filter((f) => f.type === 'number' && !f.readOnly);
  /** @type {string[]} */
  const editNormalizeLines = [];
  /** @type {string[]} */
  const coerceLines = [];
  let script = '';
  if (hasResetPeriod) {
    script += buildResetPeriodNormalizerScriptBlock();
    editNormalizeLines.push('      if (\'resetPeriod\' in next) next.resetPeriod = normalizeResetPeriodForForm((next as Record<string, unknown>).resetPeriod)');
    coerceLines.push('  if (\'resetPeriod\' in payload) payload.resetPeriod = normalizeResetPeriodForSubmit(payload.resetPeriod)');
  }
  numberFields.forEach((f) => {
    coerceLines.push(`  if ('${f.name}' in payload) {
    const raw${f.name} = payload.${f.name}
    if (raw${f.name} === undefined || raw${f.name} === null || raw${f.name} === '') {
      delete payload.${f.name}
    } else {
      const num${f.name} = typeof raw${f.name} === 'number' ? raw${f.name} : Number(raw${f.name})
      if (Number.isFinite(num${f.name})) payload.${f.name} = num${f.name}
      else delete payload.${f.name}
    }
  }`);
  });
  const payloadInit = useBuildSubmitPayload
    ? '  const payload = buildSubmitPayload() as Record<string, unknown>'
    : '  const payload = { ...formState }';
  const sortOrderCleanup = `  if ('sortOrder' in payload) delete payload.sortOrder`;
  const plantFallback = (formFields || []).some((f) => f.name === 'plantCode')
    ? `  if (!payload.plantCode) {
    // 只读工厂：未注入时勿提交空串触发 FluentValidation
    const scopedPlant = (typeof tenantStore !== 'undefined' && tenantStore.currentCompanyRelatedPlant) || ''
    if (scopedPlant) payload.plantCode = scopedPlant
  }`
    : '';
  const idEnsure = entityIdField
    ? `  if (props.formData?.${entityIdField}) {
    payload.${entityIdField} = props.formData.${entityIdField}
    delete payload.numberingRuleCode
  }`
    : '';
  const numberingStrip = (formFields || []).some((f) => f.htmlType === 'numberingRule') && !entityIdField
    ? `  if (payload.numberingRuleCode !== undefined && !String(payload.numberingRuleCode ?? '').trim()) {
    delete payload.numberingRuleCode
  }`
    : '';
  const getValuesBody = coerceLines.length > 0
    ? `${payloadInit}
${coerceLines.join('\n')}
${sortOrderCleanup}
${plantFallback}
${numberingStrip}
${idEnsure}
  return payload`
    : `${payloadInit}
${sortOrderCleanup}
${plantFallback}
${numberingStrip}
${idEnsure}
  return payload`;
  return { script, editNormalizeLines, getValuesBody };
}

/** 与 frontend table-columns.ts DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT 对齐 */
const DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT = {
  single: 8,
  tree: 4,
  masterDetailMaster: 2,
  masterDetailDetail: 4,
};

const TENANT_CORE_AUDIT_FIELDS = [
  'tenantCode', 'extField', 'remark', 'createdBy', 'createdAt', 'updatedBy', 'updatedAt',
  'isDeleted', 'deletedBy', 'deletedAt',
];

/** 与 table-columns.ts ENTITY_BASE_FIELDS 对齐（小写键，不含 id；有工厂时 plant 居首） */
const ENTITY_BASE_FIELDS_BY_SCOPE = {
  tenant: ['relatedPlant', 'cultureCode', ...TENANT_CORE_AUDIT_FIELDS],
  'tenant-core': [...TENANT_CORE_AUDIT_FIELDS],
  'tenant-culture': ['cultureCode', ...TENANT_CORE_AUDIT_FIELDS],
  'tenant-plant': ['relatedPlant', ...TENANT_CORE_AUDIT_FIELDS],
  company: [
    'plantCode', 'tenantCode', 'companyCode', 'cultureCode', 'extField', 'remark', 'createdBy', 'createdAt', 'updatedBy', 'updatedAt',
    'isDeleted', 'deletedBy', 'deletedAt',
  ],
  approval: [
    'plantCode', 'tenantCode', 'companyCode', 'cultureCode', 'extField', 'remark', 'approvalStatus', 'initiatorId', 'initiatedAt',
    'approvalOpinion', 'approvedBy', 'approvedAt', 'flowInstanceId', 'createdBy', 'createdAt', 'updatedBy',
    'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
};

/** 与 table-columns.ts ENTITY_SCOPE_PLANT_FIELD 对齐 */
const ENTITY_SCOPE_PLANT_FIELD = {
  tenant: 'relatedPlant',
  'tenant-core': undefined,
  'tenant-culture': undefined,
  'tenant-plant': 'relatedPlant',
  company: 'plantCode',
  approval: 'plantCode',
};

/**
 * 从列表字段元数据提取业务列 key（排除 id、plant、action、基类字段）
 * @param {Array<{ name: string }>} listFields
 * @param {string} entityIdName
 * @param {'tenant'|'company'|'approval'} [entityScope]
 * @returns {string[]}
 */
function extractBusinessListFieldNames(listFields, entityIdName, entityScope = 'company') {
  const baseKeys = new Set(ENTITY_BASE_FIELDS_BY_SCOPE[entityScope] || ENTITY_BASE_FIELDS_BY_SCOPE.company);
  const plantKey = ENTITY_SCOPE_PLANT_FIELD[entityScope];
  const keys = [];
  for (const field of listFields || []) {
    const name = field?.name;
    if (
      !name ||
      name === entityIdName ||
      name === 'action' ||
      (plantKey && name === plantKey) ||
      baseKeys.has(name)
    ) {
      continue;
    }
    keys.push(name);
  }
  return keys;
}

/**
 * 默认可见列 key（id + plant + 前 N 个业务列 + action）
 * @param {Array<{ name: string }>} listFields
 * @param {string} entityIdName
 * @param {'tenant'|'company'|'approval'} [entityScope]
 * @param {keyof typeof DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT} [tableMode]
 * @returns {string[]}
 */
function buildDefaultVisibleColumnKeys(listFields, entityIdName, entityScope = 'company', tableMode = 'single') {
  const count = DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT[tableMode] ?? DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT.single;
  const plantKey = ENTITY_SCOPE_PLANT_FIELD[entityScope];
  const businessKeys = extractBusinessListFieldNames(listFields, entityIdName, entityScope).slice(0, Math.max(0, count));
  const keys = [entityIdName];
  if (plantKey) {
    keys.push(plantKey);
  }
  keys.push(...businessKeys, 'action');
  return keys;
}

/**
 * 生成 Vue 可见列默认 key 数组字面量（供主子表 index / 子 panel 初始化 visibleColumnKeys）
 * @param {Array<{ name: string }>} listFields
 * @param {string} entityIdName
 * @param {'tenant'|'company'|'approval'} [entityScope]
 * @param {keyof typeof DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT} [tableMode]
 * @returns {string}
 */
function buildDefaultVisibleColumnKeysLiteral(listFields, entityIdName, entityScope = 'company', tableMode = 'single') {
  return JSON.stringify(buildDefaultVisibleColumnKeys(listFields, entityIdName, entityScope, tableMode));
}

/** 子表 panel 合计行跳过的数值字段（行号/排序等非累计列） */
const CHILD_PANEL_SUMMARY_SKIP_FIELDS = new Set(['lineNumber', 'sortOrder', 'seq', 'rowNo', 'rowIndex']);

/**
 * 子表右栏 panel 默认可见业务列（不含 id，含 action）
 * @param {Array<{ name: string }>} listFields
 * @param {string} entityIdName
 * @param {'tenant'|'company'|'approval'} [entityScope]
 * @returns {string[]}
 */
function buildChildPanelDefaultVisibleColumnKeyNames(listFields, entityIdName, entityScope = 'company') {
  const businessKeys = extractBusinessListFieldNames(listFields, entityIdName, entityScope);
  return [...businessKeys, 'action'];
}

/**
 * 子表右栏 panel 合计列（数值型 list 字段）
 * @param {Array<{ name: string, type?: string }>} listFields
 * @returns {string[]}
 */
function buildChildPanelSummarySumFieldNames(listFields) {
  return (listFields || [])
    .filter((f) => f?.name && f.type === 'number' && !CHILD_PANEL_SUMMARY_SKIP_FIELDS.has(f.name))
    .map((f) => f.name);
}

/**
 * 子表右栏 panel composable 导入（含 DEFAULT_VISIBLE / SUMMARY_SUM）
 * @param {string} entityPascal
 * @param {string} viewEntityKebab
 * @param {string} [composableDir]
 * @returns {string}
 */
function buildChildPanelEntityI18nImportBlock(entityPascal, viewEntityKebab, composableDir = '../composables') {
  const prefix = entityI18nConstPrefix(entityPascal);
  const hookName = entityI18nHookName(entityPascal);
  const composableStem = entityI18nComposableFileName(viewEntityKebab).replace(/\.ts$/, '');
  return `import {
  ${hookName},
  ${prefix}_DEFAULT_VISIBLE_COLUMN_KEYS,
  ${prefix}_SUMMARY_SUM_FIELDS,
  ${prefix}_QUERY_STRING_FIELDS,
  ${prefix}_QUERY_FIELDS,
  ${prefix}_SELF_I18N_KEY,
} from '${composableDir}/${composableStem}'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = ${hookName}()
`;
}

/**
 * index.vue：resetPeriod 列 TaktDictTag 展示归一化
 * @param {Array<{ name: string }>} listFields
 * @returns {string}
 */
function buildResetPeriodListMapperScriptBlock(listFields) {
  if (!(listFields || []).some((f) => f.name === 'resetPeriod')) {
    return '';
  }
  return `/** 列表 TaktDictTag：resetPeriod 归一化为 sys_reset_period dictValue */
const RESET_PERIOD_TO_DICT: Record<string, string> = {
  none: 'None',
  annually: 'Annually',
  year: 'Annually',
  yearly: 'Annually',
  monthly: 'Monthly',
  month: 'Monthly',
  daily: 'Daily',
  day: 'Daily',
  hour: 'Daily',
  hourly: 'Daily',
}

/** @param value 后端 resetPeriod */
function mapResetPeriodDictValue(value?: string | number | null): string {
  const raw = String(value ?? 'None').trim()
  const key = raw.toLowerCase()
  return RESET_PERIOD_TO_DICT[key] ?? raw
}
`;
}

/**
 * 列表字典列 :value 表达式（resetPeriod 需 legacy 归一化）
 * @param {{ name: string }} field
 * @param {string} entityPascal
 * @returns {string}
 */
function buildListDictTagValueExpr(field, entityPascal) {
  if (field.name === 'resetPeriod') {
    return `mapResetPeriodDictValue(get${entityPascal}DictValue(record, '${field.name}'))`;
  }
  return `get${entityPascal}DictValue(record, '${field.name}')`;
}

/**
 * 生成 index.vue 字典标量读取辅助函数（TaktDictTag :value 类型安全）
 * @param {string} entityPascal
 * @param {string} rowRecordType
 * @returns {string}
 */
function buildEntityDictValueHelper(entityPascal, rowRecordType) {
  return `/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const get${entityPascal}DictValue = (
  record: ${rowRecordType},
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}
`;
}

/**
 * 生成 index.vue 数值字段强制转换（开关回滚等，返回有限 number）
 * @param {string} entityPascal
 * @returns {string}
 */
function buildEntityNumericCoerceHelper(entityPascal) {
  return `/** 将行字段/字典值转为有限 number */
const to${entityPascal}Number = (value: string | number | undefined | null): number => {
  if (typeof value === 'number' && Number.isFinite(value)) return value
  const num = Number(value ?? 0)
  return Number.isFinite(num) ? num : 0
}
`;
}

/**
 * 拆分列表字典列：开关列 vs DictTag 列
 * @param {Array<{ dictType?: string, isListSwitch?: boolean }>} listCols
 */
function splitListDictColumns(listCols) {
  const switchListCols = (listCols || []).filter((f) => f.isListSwitch);
  const dictTagListCols = (listCols || []).filter((f) => f.dictType && !f.isListSwitch);
  return { switchListCols, dictTagListCols };
}

/**
 * 按 API 能力拆分列表列：有对应更新 API 的用 switch，否则回退 DictTag
 * @param {Array<object>} listColsWithDictOrSwitch
 * @param {{ apiUpdateStatus?: string, apiUpdateBuiltIn?: string }} caps
 * @returns {{ switchListCols: Array<object>, dictTagListCols: Array<object> }}
 */
function resolveListSwitchAndDictColsForIndex(listColsWithDictOrSwitch, caps) {
  const { switchListCols: rawSwitch, dictTagListCols: rawDict } = splitListDictColumns(listColsWithDictOrSwitch);
  const switchListCols = [];
  const dictTagListCols = [...rawDict];
  for (const field of rawSwitch) {
    const hasApi = field.switchKind === 'builtin'
      ? Boolean(caps.apiUpdateBuiltIn)
      : Boolean(caps.apiUpdateStatus);
    if (hasApi) {
      switchListCols.push(field);
    } else {
      const {
        isListSwitch: _isListSwitch,
        switchKind: _switchKind,
        switchCheckedValue: _switchCheckedValue,
        switchUncheckedValue: _switchUncheckedValue,
        ...dictField
      } = field;
      dictTagListCols.push(dictField);
    }
  }
  return { switchListCols, dictTagListCols };
}

/**
 * 生成列表开关列 bodyCell 片段
 * @param {{ name: string, switchKind?: string, switchCheckedValue?: number }} field
 * @param {string} entityPascal
 * @param {string} branch
 * @returns {string}
 */
function buildListSwitchBodyCellLine(field, entityPascal, branch) {
  const checkedExpr = `get${entityPascal}DictValue(record, '${field.name}') === ${field.switchCheckedValue}`;
  const childrenAttrs = field.switchKind === 'builtin'
    ? `:checked-children="t('dict.sys.yes.no.1')" :un-checked-children="t('dict.sys.yes.no.0')"`
    : `:checked-children="t('common.page.button.enable')" :un-checked-children="t('common.page.button.disable')"`;
  const handlerName = `handle${fieldNameToHandlerSuffix(field.name)}Change`;
  return `        <template ${branch}="column.key === '${field.name}'">
          <a-switch
            :checked="${checkedExpr}"
            ${childrenAttrs}
            @change="(checked: unknown) => ${handlerName}(record, Boolean(checked))"
          />
        </template>`;
}

/**
 * 生成列表 bodyCell（开关 + DictTag）
 * @param {Array<object>} dictTagListCols
 * @param {Array<object>} switchListCols
 * @param {string} entityPascal
 * @returns {string}
 */
function buildListBodyCellBlock(dictTagListCols, switchListCols, entityPascal) {
  if (!dictTagListCols.length && !switchListCols.length) {
    return '';
  }
  const lines = [];
  switchListCols.forEach((f, i) => {
    lines.push(buildListSwitchBodyCellLine(f, entityPascal, i === 0 ? 'v-if' : 'v-else-if'));
  });
  dictTagListCols.forEach((f, i) => {
    const branch = lines.length === 0 && i === 0 ? 'v-if' : 'v-else-if';
    lines.push(`        <template ${branch}="column.key === '${f.name}'">
          <TaktDictTag
            :value="${buildListDictTagValueExpr(f, entityPascal)}"
            dict-type="${f.dictType}"
          />
        </template>`);
  });
  return `      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
${lines.join('\n')}
      </template>
`;
}

/**
 * 生成列表开关列 change 处理器
 * @param {Array<object>} switchListCols
 * @param {string} entityPascal
 * @param {{ entityIdName: string, apiUpdateStatus?: string, apiUpdateBuiltIn?: string }} caps
 * @param {{ reloadAfterSuccess?: boolean, recordType?: string }} [options]
 * @returns {string}
 */
function buildListSwitchHandlersBlock(switchListCols, entityPascal, caps, options = {}) {
  const reloadAfterSuccess = options.reloadAfterSuccess === true;
  const recordType = options.recordType || entityRowRecordTypeName(entityPascal);
  return (switchListCols || []).map((field) => {
    const handlerName = `handle${fieldNameToHandlerSuffix(field.name)}Change`;
    const apiMethod = field.switchKind === 'builtin' ? caps.apiUpdateBuiltIn : caps.apiUpdateStatus;
    if (!apiMethod) {
      return '';
    }
    const idField = caps.entityIdName;
    if (field.switchKind === 'builtin') {
      return `
/**
 * 行内切换内置（sys_yes_no：1=是，0=否）
 * @param record 当前行
 * @param checked 开关是否选中
 */
async function ${handlerName}(record: ${recordType}, checked: boolean) {
  const id = get${entityPascal}Id(record)
  if (!id) {
    return
  }
  const newVal = checked ? ${field.switchCheckedValue} : ${field.switchUncheckedValue}
  const oldVal = to${entityPascal}Number(get${entityPascal}DictValue(record, '${field.name}'))
  ${reloadAfterSuccess ? '' : `const row = dataSource.value.find((item) => get${entityPascal}Id(item) === id)
  if (row) {
    row.${field.name} = newVal
  }
  `}try {
    await ${apiMethod}({ ${idField}: id, ${field.name}: newVal })
    message.success(t('common.feedback.updated'))
    ${reloadAfterSuccess ? 'await loadData()' : ''}
  } catch (error: unknown) {
    ${reloadAfterSuccess ? '' : `if (row) {
      row.${field.name} = oldVal
    }
    `}message.error(t('common.feedback.failed'))
  }
}`;
    }
    return `
/**
 * 行内状态切换
 * @param record 当前行
 * @param checked 是否启用
 */
async function ${handlerName}(record: ${recordType}, checked: boolean) {
  const newVal = checked ? ${field.switchCheckedValue} : ${field.switchUncheckedValue}
  const oldVal = to${entityPascal}Number(get${entityPascal}DictValue(record, '${field.name}'))
  const id = get${entityPascal}Id(record)
  ${reloadAfterSuccess ? '' : `const row = dataSource.value.find((item) => get${entityPascal}Id(item) === id)
  if (row) {
    row.${field.name} = newVal
  }
  `}try {
    await ${apiMethod}({ ${idField}: id, ${field.name}: newVal })
    message.success(t('common.feedback.updated'))
    ${reloadAfterSuccess ? 'await loadData()' : ''}
  } catch (error: unknown) {
    ${reloadAfterSuccess ? '' : `if (row) {
      row.${field.name} = oldVal
    }
    `}message.error(t('common.feedback.failed'))
  }
}`;
  }).filter(Boolean).join('\n');
}

/** index.vue：关闭/提交/新增后重置内嵌表单（弹窗未 destroy 时须 nextTick） */
const INDEX_FORM_RESET_NEXT_TICK = `
  nextTick(() => formRef.value?.resetFields())`;

/**
 * 字段列表是否含 dict-type 的 TaktSelect
 * @param {Array<{ htmlType?: string, dictType?: string }>} fields
 * @returns {boolean}
 */
function fieldsUseDictSelect(fields) {
  return (fields || []).some((f) => f.htmlType === 'select' && f.dictType);
}

/**
 * 生成表单必填校验规则（数值字典项用 validator，避免 0 被 required 判空）
 * @param {Array<{ name: string, htmlType: string, dictType?: string, type?: string, optional?: boolean, readOnly?: boolean }>} formFields
 * @returns {string}
 */
function buildFormRequiredRuleLines(formFields) {
  return formFields
    .filter((f) => {
      if (f.htmlType === 'numberingRule' || f.htmlType === 'numberingCode') {
        return true;
      }
      return !f.optional && f.name !== 'remark' && !isExtFieldField(f) && !f.readOnly;
    })
    .map((f) => {
      if (f.htmlType === 'numberingRule') {
        return `  numberingRuleCode: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingRuleRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],`;
      }
      if (f.htmlType === 'numberingCode') {
        return `  ${f.name}: [{
    validator: async (_rule, value) => {
      if (isEditMode.value) {
        return Promise.resolve()
      }
      if (!String(value ?? '').trim()) {
        return Promise.reject(t('common.page.form.numberingCodePreview'))
      }
      return Promise.resolve()
    },
    trigger: 'change',
  }],`;
      }
      const trigger = f.htmlType === 'select' || f.htmlType === 'apiSelect' || f.htmlType === 'date' || f.htmlType === 'switch' ? 'change' : 'blur';
      const placeholderKey = f.htmlType === 'select' || f.htmlType === 'apiSelect' || f.htmlType === 'date'
        ? 'common.page.form.placeholder.select'
        : 'common.page.form.placeholder.required';
      if (f.htmlType === 'select' && f.type === 'number') {
        return `  ${f.name}: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(${fieldPlaceholderTExpr(f, placeholderKey)})
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(${fieldPlaceholderTExpr(f, placeholderKey)})
      }
      return Promise.resolve()
    },
    trigger: '${trigger}'
  }],`;
      }
      return `  ${f.name}: [
    {
      required: true,
      message: ${fieldPlaceholderTExpr(f, placeholderKey)},
      trigger: '${trigger}'
    }
  ],`;
    })
    .join('\n');
}

/** @returns {string} useDictDataStore 导入行 */
function buildDictDataStoreImportLine() {
  return "import { useDictDataStore } from '@/stores/foundation/useDictDataStore'\n";
}

/**
 * 表单 script：字典预热（TaktSelect dict-type 打开弹窗前加载缓存）
 * @returns {string}
 */
function buildDictDataStoreFormBootstrap() {
  return `/** Pinia：字典缓存（TaktSelect dict-type 渲染前预热，避免选项空白） */
const dictDataStore = useDictDataStore()

/** 表单挂载时预加载全量字典 */
onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})
`;
}

/**
 * index.vue script：字典预热 setup 块
 * @returns {string}
 */
function buildDictDataStoreIndexSetup() {
  return `/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()
`;
}

/**
 * 生成 *Form.vue 的 formData watch（仅含主键时灌编辑态）
 * @param {object} options
 * @returns {string}
 */
function buildFormDataWatchBlock(options) {
  const {
    entityIdField,
    childFieldStrip = '',
    hasScopeContextFields = false,
    watchSyncChild = '',
    editNormalizeLines = [],
  } = options;
  const scopeOnCreate = hasScopeContextFields
    ? `      applyScopeDefaults(formState as Record<string, unknown>, true)
`
    : '';
  const scopeOnEdit = hasScopeContextFields
    ? `      applyScopeDefaults(next)
`
    : '';
  const editNormalizeBlock = editNormalizeLines.length > 0
    ? `${editNormalizeLines.join('\n')}\n`
    : '';
  return `/** 编辑态灌入 formData；新增态恢复默认值（须含 ${entityIdField} 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.${entityIdField}) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
${childFieldStrip}
${editNormalizeBlock}${scopeOnEdit}      Object.assign(formState, next)
${watchSyncChild}      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
${scopeOnCreate}      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)`;
}

/**
 * 业务文件上传：额外 import（message / Upload 类型 / TaktFile API 与策略）
 * @returns {string}
 */
function buildBusinessFileUploadFormImportBlock() {
  return `import { message } from 'ant-design-vue'
import type { UploadFile, UploadProps } from 'ant-design-vue'
import { getFileById } from '@/api/foundation/file.api'
import { uploadTaktFileSmart } from '@/utils/taktFileChunkUpload'
import {
  buildTaktFileAcceptAttribute,
  loadTaktFileUploadBasePolicy,
  resolveTaktFileMaxSizeMb,
} from '@/utils/taktFileUploadPolicy'
`;
}

/**
 * 业务文件上传：状态、回填 fileName/accessUrl、自定义上传与策略加载
 * @returns {string}
 */
function buildBusinessFileUploadFormScriptBlock() {
  return `/** 文件上传中 */
const fileUploading = ref(false)
/** takt-upload-file 文件列表 */
const filesFileList = ref<UploadFile[]>([])
/** 上传 accept */
const taktFileAccept = ref('')
/** 上传体积上限 MB */
const taktFileMaxSizeMb = ref(500)

/**
 * 按 fileName / accessUrl 同步上传列表展示
 */
function syncFilesFileListFromFormState() {
  const url = String(formState.accessUrl ?? '').trim()
  if (!url) {
    filesFileList.value = []
    return
  }
  filesFileList.value = [{
    uid: '-1',
    name: String(formState.fileName ?? url.split('/').pop() ?? 'file'),
    status: 'done',
    url,
  }]
}

/**
 * 将 TaktFile 上传结果回填至表单（文件名由上传结果回填，禁止手输）
 * @param file 本地文件
 * @param result 上传结果
 */
async function applyUploadResultToForm(file: globalThis.File, result: Awaited<ReturnType<typeof uploadTaktFileSmart>>) {
  let accessUrl = result.accessUrl?.trim() ?? ''
  if (!accessUrl && result.fileId) {
    const detail = await getFileById(result.fileId)
    accessUrl = detail.accessUrl?.trim() ?? ''
  }
  if (!accessUrl) {
    throw new Error('accessUrl empty')
  }
  formState.accessUrl = accessUrl
  formState.fileName = result.fileOriginalName?.trim()
    || result.fileName?.trim()
    || file.name
  syncFilesFileListFromFormState()
  formRef.value?.validateFields(['accessUrl', 'fileName']).catch(() => undefined)
}

/** takt-upload-file 自定义上传：落库 TaktFile 后回写 accessUrl / fileName */
const handleFilesCustomRequest: UploadProps['customRequest'] = (options) => {
  if (props.loading || fileUploading.value) {
    options.onError?.(new Error('upload disabled'))
    return
  }
  const originFile = options.file as globalThis.File
  fileUploading.value = true
  void (async () => {
    try {
      const result = await uploadTaktFileSmart(originFile)
      await applyUploadResultToForm(originFile, result)
      options.onSuccess?.(result)
    } catch (error: unknown) {
      const err = error instanceof Error ? error : new Error(String(error))
      message.error(t('common.feedback.failed'))
      options.onError?.(err)
    } finally {
      fileUploading.value = false
    }
  })()
}

/** 移除已上传文件 */
function handleFileRemove() {
  formState.accessUrl = ''
  formState.fileName = ''
  filesFileList.value = []
}

watch(
  () => [formState.fileName, formState.accessUrl],
  () => {
    syncFilesFileListFromFormState()
  },
)

/** 挂载后加载后端上传策略（accept / maxSize） */
onMounted(() => {
  void (async () => {
    try {
      const policy = await loadTaktFileUploadBasePolicy()
      taktFileAccept.value = buildTaktFileAcceptAttribute(policy.allowedExtensions ?? [])
      taktFileMaxSizeMb.value = resolveTaktFileMaxSizeMb(policy)
    } catch {
      // 回退默认值；实际上传校验仍由后端 API 返回
    }
  })()
})
`;
}

/**
 * 生成 *Form.vue script 段公共片段（三模板共用）
 * @param {object} options
 * @returns {{ vueImportLine: string, dictImportLine: string, dictBootstrap: string, fileUploadImportLine: string, fileUploadBootstrap: string, requiredRules: string, watchBlock: string }}
 */
function buildGeneratedFormVueScriptFragments(options) {
  const needsDictSelect = fieldsUseDictSelect(options.formFields);
  const needsFileUpload = fieldsNeedFileUpload(options.formFields);
  const needsNumbering = fieldsNeedTaktNumbering(options.formFields);
  const needsOnMounted = needsDictSelect || needsFileUpload;
  const fileUploadImportLine = needsFileUpload ? buildBusinessFileUploadFormImportBlock() : '';
  const fileUploadBootstrap = needsFileUpload ? buildBusinessFileUploadFormScriptBlock() : '';
  const numberingImportLine = needsNumbering
    ? "import { useTaktFormNumbering } from '@/composables/useTaktFormNumbering'\n"
    : '';
  const numberingBootstrap = needsNumbering
    ? buildTaktNumberingFormScriptBlock(options.formFields, options.entityIdField || 'id')
    : '';
  const normalizer = buildFormSubmitNormalizerScriptBlock(options.formFields, {
    useBuildSubmitPayload: !!options.useBuildSubmitPayload,
    entityIdField: options.entityIdField || '',
  });
  return {
    // vue / pinia 等由 unplugin-auto-import 注入（auto-imports.d.ts）；.vue 禁止再写 import { ref, … } from 'vue'
    vueImportLine: '',
    dictImportLine: needsDictSelect ? buildDictDataStoreImportLine() : '',
    dictBootstrap: needsDictSelect ? buildDictDataStoreFormBootstrap() : '',
    fileUploadImportLine,
    fileUploadBootstrap,
    numberingImportLine,
    numberingBootstrap,
    defaultsBlock: buildFormDefaultsScriptBlock(options.formFields),
    normalizerBlock: normalizer.script,
    getValuesBody: normalizer.getValuesBody,
    requiredRules: buildFormRequiredRuleLines(options.formFields),
    watchBlock: buildFormDataWatchBlock({
      ...options,
      editNormalizeLines: normalizer.editNormalizeLines,
    }),
  };
}

/**
 * index.vue：buildListQuery 函数（列表/导出共用；空查询项不下发，避免 DateTime? 绑定 400）
 * 无参不补默认条件；由 hasAnyListQueryFilter 决定是否请求
 * @param {string} entityPascal
 * @param {object[]} [queryFields]
 * @returns {string}
 */
function buildServerPagedListQueryBlock(entityPascal, queryFields = []) {
  const numberFields = (queryFields || []).filter((f) => f.type === 'number');
  const constName = `${entityI18nConstPrefix(entityPascal)}_QUERY_STRING_FIELDS`;
  const numberAssign = numberFields.map((f) => `  if (form.${f.name} !== undefined && form.${f.name} !== null) {
    query.${f.name} = form.${f.name}
  }`).join('\n');
  const numberAssignBlock = numberAssign ? `\n${numberAssign}` : '';
  return `
/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {${entityPascal}Query} 查询 DTO
 */
function buildListQuery(overrides?: Partial<${entityPascal}Query>): ${entityPascal}Query {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: ${entityPascal}Query = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof ${entityPascal}Query, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of ${constName}) {
    assignTrimmed(key, form[key])
  }${numberAssignBlock}
  return query
}
`;
}

/**
 * loadData 内请求列表片段
 * @param {string} apiGetList
 * @returns {string}
 */
function buildServerPagedLoadDataBody(apiGetList) {
  return `    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await ${apiGetList}(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0`;
}

/**
 * handleExport 内 export API 调用片段
 * @param {string} apiExport
 * @returns {string}
 */
function buildServerPagedExportApiCall(apiExport) {
  return `    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await ${apiExport}(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )`;
}

/**
 * index.vue：onMounted（先 ensure 分页配置，再 loadData / 字典预热等）
 * @param {string} [extraMountedBody] 额外挂载逻辑（如 dictDataStore.loadAllDictDataAsync）
 * @returns {string}
 */
function buildServerPagedOnMountedBlock(extraMountedBody = '') {
  const body = extraMountedBody.trimEnd();
  const mid = body ? `${body}\n` : '';
  return `/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
${mid}  loadData()
})
`;
}

/**
 * index.vue：外置 TaktPagination 事件处理
 * @returns {string}
 */
function buildServerPagedPaginationHandlersBlock() {
  return `/** 分页页码变更 */
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
}`;
}

/**
 * resetFields 内补全租户/公司/语言（父级 nextTick 调用时会清空 watch 已写入的隔离字段）
 * @param {string} entityIdField 主键字段名
 * @param {boolean} hasScopeContextFields
 * @returns {string}
 */
function buildFormResetScopeDefaultsBlock(entityIdField, hasScopeContextFields) {
  if (!hasScopeContextFields) {
    return '';
  }
  return `  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.${entityIdField})
`;
}

/**
 * index.vue：单表列表页不生成 scoped 样式（表格高度/滚动由 TaktSingleTable 统一处理）
 * @returns {string}
 */
function buildServerPagedIndexStyleBlock() {
  return '';
}

/**
 * index.vue 导入结果归一化工具 import 行
 * @returns {string}
 */
function buildVueImportResultUtilImportLine() {
  return "import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'\n";
}

/** 表单弹窗宽度 import（与 ec-te 来源导入/主表/附件弹窗一致） */
const TAKT_FORM_MODAL_WIDTH_IMPORT =
  "import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'\n";

/** TaktModal 表单弹窗 width 绑定 */
const TAKT_FORM_MODAL_WIDTH_ATTR = ':width="formModalWidthPx"';

/**
 * index/panel：表单弹窗宽度（视口 − 左侧菜单）× 80%
 * @returns {string}
 */
function buildFormModalWidthStateBlock() {
  return `/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()
`;
}

/**
 * 导入 Modal + TaktImportFile 模板（v-if 关闭时销毁组件，避免文件/结果残留）
 * @param {string} entityPascal 实体 PascalCase（用于 SELF_I18N_KEY 常量前缀）
 * @returns {string}
 */
function buildImportModalVueBlock(entityPascal) {
  const prefix = entityI18nConstPrefix(entityPascal);
  return `
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
        :entity-i18n-key="${prefix}_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>`;
}

/**
 * 导入 handler 脚本块（归一化后端 SuccessCount/successCount，与 TaktImportFile 提示一致）
 * @param {object} options
 * @param {string} options.apiGetTemplate getXxxTemplate API 函数名
 * @param {string} options.apiImport importXxxData API 函数名
 * @param {string} [options.openHandlerPrefix] handleImport 开头额外逻辑（如主子表须先选中主行）
 * @param {string} [options.successBody] handleImportSuccess 体内刷新语句，默认 loadData()
 * @param {string} [options.successExtraBody] handleImportSuccess 刷新后追加语句（如 reload 子表面板）
 * @returns {string}
 */
function buildImportHandlersScriptBlock(options) {
  const {
    apiGetTemplate,
    apiImport,
    openHandlerPrefix = '',
    successBody = 'loadData()',
    successExtraBody = '',
  } = options;
  const openPrefixLines = openHandlerPrefix
    ? openHandlerPrefix.split('\n').map((line) => (line ? `  ${line}` : line)).join('\n') + '\n'
    : '';
  const successExtra = successExtraBody
    ? successExtraBody.split('\n').map((line) => (line ? `  ${line}` : line)).join('\n') + '\n'
    : '';
  return `
/** 打开导入对话框 */
function handleImport() {
${openPrefixLines}  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await ${apiGetTemplate}(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await ${apiImport}(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  ${successBody}
${successExtra}  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}`;
}

module.exports = {
  VUE_TEMPLATE,
  CONFIG,
  templateTypeLabel,
  collectApiFiles,
  kebabToPascal,
  pascalToKebab,
  toFrontendEntityFileCamel,
  toFrontendApiFileName,
  toFrontendTypesFileName,
  parseFrontendApiFileCamel,
  parseFrontendTypesFileCamel,
  camelToKebab,
  kebabToCamel,
  parseVueCliArgs,
  runVueGeneratorCli,
  SKIP_LIST_FIELDS,
  SKIP_FORM_FIELDS,
  SKIP_QUERY_FIELDS,
  FORM_TAB_FIELDS_PER_TAB,
  FORM_TAB_LEADING_FIELD_NAMES,
  FORM_TAB_TRAILING_FIELD_NAMES,
  resolveFormFieldColSpan,
  MENU_INDEX,
  pascalToCamel,
  entityClassToSlug,
  buildEntityI18nKey,
  resolveFieldTranslationKey,
  entityI18nConstPrefix,
  entityI18nHookName,
  entityI18nComposableFileName,
  entityMasterContextComposableFileName,
  resolveFormPlaceholderKind,
  buildEntityI18nComposableFile,
  buildEntityI18nIndexImportBlock,
  buildEntityI18nFormImportBlock,
  buildChildPanelEntityI18nImportBlock,
  buildChildPanelSummarySumFieldNames,
  fieldLabelTExpr,
  buildQueryFieldMetaLine,
  fieldPlaceholderTExpr,
  isExtFieldField,
  fieldExtFieldPlaceholderTExpr,
  renderFormItemOpening,
  buildExtFieldIconImportLine,
  buildRemixIconImportLine,
  EXT_FIELD_FORM_NAME,
  EXT_FIELD_HINT_I18N_KEY,
  EXT_FIELD_PLACEHOLDER_I18N_KEY,
  SCOPE_FORM_FIELD_NAMES,
  hasScopeContextFormFields,
  buildScopeFormFields,
  renderReadOnlyControlAttrs,
  isBusinessCodeFormFieldName,
  renderFormCodeEditDisabledAttrs,
  computeFormTabCount,
  partitionFormFieldsForTabs,
  buildFormTabPanesMarkup,
  buildFormTabLabelAttr,
  buildFormContentClassComputedExpr,
  buildFormContentClassExpr,
  buildAdvancedQueryFactoryBlock,
  buildListColumnsGeneratorBlock,
  resolveScopeFormFieldPresence,
  buildScopeContextFormScriptFragments,
  shouldWrapFormInTabs,
  buildFormRowMarkup,
  buildFormFieldColItems,
  buildGeneratedFormTemplateBody,
  buildMasterDetailFormTypeImportLines,
  buildFormTabsScopedStyleBlock,
  buildMenuIndex,
  resolvePermissionPrefixFromController,
  parseApiFile,
  parseTypeInterfaces,
  isDtoFillField,
  isDtoNavigationProperty,
  isEntityDerivedFormField,
  detectApiCapabilities,
  entityHasParentId,
  extendTreeApiCapabilities,
  parseOneToManyNavigations,
  collectDomainEntityFiles,
  buildMasterDetailChildRegistry,
  resolveMasterDetailChildren,
  normalizeModulePath,
  resolveMasterDetailChildViewModulePath,
  resolveMasterDetailChildViewPath,
  resolveMasterDetailViewPlans,
  resolveChildMenuViewPath,
  filterStandaloneMenuChildren,
  childHasStandaloneMenu,
  isChildNavMasterDetailMaster,
  validateMasterDetailChildrenAlignment,
  cloneFieldMetaWithMasterDetailChildren,
  buildFieldMeta,
  resolveModuleContext,
  resolveMainEntityPascalFromTypes,
  stripModulePrefixFromEntityKebab,
  renderFormControl,
  renderAInputLimitAttrs,
  resolveInputMaxLength,
  DEFAULT_A_INPUT_MAX_LENGTH,
  REMARK_TEXTAREA_ROWS,
  REMARK_TEXTAREA_MAX_LENGTH,
  isFixedLongTextareaField,
  renderFixedLongTextareaAttrs,
  isRemarkField,
  renderRemarkTextareaAttrs,
  renderQueryFormItem,
  parseEntityComment,
  loadVueModuleContext,
  writeVueModuleOutputs,
  fieldsUseDictSelect,
  extractDictType,
  extractOptionsApiUrl,
  resolveOptionsApiUrl,
  renderApiSelectParamsAttr,
  entityRowRecordTypeName,
  buildEntityRowRecordTypeAlias,
  buildEntityDictValueHelper,
  buildEntityNumericCoerceHelper,
  inferHtmlType,
  isRichTextContentField,
  hasBusinessFileUploadPair,
  applyBusinessFileUploadMeta,
  fieldsNeedFileUpload,
  isTaktNumberingAutoFormField,
  applyTaktNumberingFormMeta,
  fieldsNeedTaktNumbering,
  buildTaktNumberingFormScriptBlock,
  buildFormRequiredRuleLines,
  buildDictDataStoreImportLine,
  buildDictDataStoreFormBootstrap,
  buildDictDataStoreIndexSetup,
  buildFormDataWatchBlock,
  buildGeneratedFormVueScriptFragments,
  getDictTypeDefaultsMap,
  resolveFormFieldDefaultValue,
  buildFormDefaultsScriptBlock,
  buildFormSubmitNormalizerScriptBlock,
  buildResetPeriodListMapperScriptBlock,
  DEFAULT_VISIBLE_BUSINESS_FIELD_COUNT,
  buildDefaultVisibleColumnKeys,
  buildDefaultVisibleColumnKeysLiteral,
  buildListDictTagValueExpr,
  splitListDictColumns,
  resolveListSwitchAndDictColsForIndex,
  buildListBodyCellBlock,
  buildListSwitchHandlersBlock,
  buildListSwitchBodyCellLine,
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
  buildFormModalWidthStateBlock,
  buildImportModalVueBlock,
  buildImportHandlersScriptBlock,
};

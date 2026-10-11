// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-entity-i18n-seed.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Domain 实体全量生成 Takt{Entity}I18nSeedData.cs（英/日/中三语翻译种子）
// TranslationText：基准文案 + 语言后缀（en-US/_us、ja-JP/_jp、zh-CN/_cn、zh-HK/_hk）
// 用法: node scripts/generate-entity-i18n-seed.cjs [--all|-all]
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  logGeneratedFileWritePolicy,
  sanitizeXmlDocPlainText,
  pascalToCamel,
  ENTITY_FIELD_I18N_SEGMENT,
  ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG,
  entityClassToSlug,
  buildEntityI18nKey,
  buildTranslationResourceGroup,
  TRANSLATION_RESOURCE_TYPE_FRONTEND,
  parseAllOnlyGenerateArgs,
  parseEntityClassHeaderFromCsContent,
  ENTITY_CLASS_HEADER_REGEX,
} = require('./generate-script-common.cjs');
const { isManualDtoEntity } = require('./generate-entity-exclusions.cjs');

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  frontendRoot: path.resolve(__dirname, '../../frontend'),
  entitiesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities'),
  i18nSeedRoot: path.join(
    path.resolve(__dirname, '../../backend/src'),
    'Takt.Infrastructure',
    'Data',
    'Seeds',
    'I18nSeedData'
  ),
  cultures: ['en-US', 'ja-JP', 'zh-CN', 'zh-HK'],
};

/** 各语言 TranslationText 后缀（与 zh-CN 基准文案拼接，便于区分多语言种子） */
const CULTURE_TRANSLATION_SUFFIX = {
  'en-US': '_us',
  'ja-JP': '_jp',
  'zh-CN': '',
  'zh-HK': '_hk',
};

/**
 * 为 TranslationText 追加语言后缀（如 状态 → 状态_us）
 * @param {string} text 基准文案
 * @param {string} culture BCP47
 * @returns {string}
 */
function withCultureTranslationSuffix(text, culture) {
  const base = (text || '').trim();
  if (!base) {
    return base;
  }
  const suffix = CULTURE_TRANSLATION_SUFFIX[culture];
  if (!suffix || base.endsWith(suffix)) {
    return base;
  }
  return `${base}${suffix}`;
}

/**
 * 按 CONFIG.cultures 创建空语言映射
 * @returns {Record<string, Record<string, string>>}
 */
function createEmptyCultureFieldMap() {
  return Object.fromEntries(CONFIG.cultures.map((culture) => [culture, {}]));
}

/** 实体基类隔离/审计字段（不生成 entity.{slug}.{field} 键；业务自声明的 CultureCode 除外） */
const ENTITY_BASE_FIELDS = new Set([
  'Id',
  'TenantCode',
  'CompanyCode',
  // CultureCode：仅基类隔离列不在实体 classBody 中；Culture/Translation 等业务 CultureCode 须生成 entity.*.culturecode
  'PlantCode',
  'RelatedPlant',
  'ExtField',
  'Remark',
  'CreatedBy',
  'CreatedAt',
  'UpdatedBy',
  'UpdatedAt',
  'IsDeleted',
  'DeletedBy',
  'DeletedAt',
  'ApprovalStatus',
  'InitiatorId',
  'InitiatedAt',
  'ApprovalOpinion',
  'ApprovedBy',
  'ApprovedAt',
]);

/** frontend locales field 键别名（与 ENTITY_FIELD_I18N_SEGMENT 对齐，供静态文案回填） */
const LOCALE_FIELD_ALIASES = {
  ...ENTITY_FIELD_I18N_SEGMENT,
  userStatus: 'isActive',
};

/** 命名空间首段 → TaktModule（与业务领域一致） */
const NAMESPACE_MODULE_MAP = {
  Identity: 'Identity',
  Routine: 'Routine',
  Accounting: 'Accounting',
  Logistics: 'Logistics',
  HumanResource: 'HumanResource',
  Workflow: 'Workflow',
  Code: 'Code',
  Foundation: 'Foundation',
  Statistics: 'Statistics',
};

/**
 * 根据实体命名空间解析 TaktModule
 * @param {string} entityNamespace
 */
function resolveTaktModule(entityNamespace) {
  const relative = entityNamespace.replace('Takt.Domain.Entities.', '');
  const top = relative.split('.')[0];
  return NAMESPACE_MODULE_MAP[top] || 'Foundation';
}

/** @type {Set<string> | null} */
let TAKT_ENUM_TYPE_NAMES = null;

/** @type {Set<string> | null} */
let TAKT_ENTITY_CLASS_NAMES = null;

/**
 * 扫描 Takt.Shared/Enums 下全部 public enum TaktXxx
 * @returns {Set<string>}
 */
function loadTaktEnumTypeNames() {
  if (TAKT_ENUM_TYPE_NAMES) {
    return TAKT_ENUM_TYPE_NAMES;
  }
  const set = new Set();
  const enumsRoot = path.join(CONFIG.backendRoot, 'Takt.Shared', 'Enums');
  function walk(dir) {
    fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(full);
        return;
      }
      if (!entry.name.endsWith('.cs')) {
        return;
      }
      const content = fs.readFileSync(full, 'utf-8');
      for (const m of content.matchAll(/public\s+enum\s+(Takt\w+)/g)) {
        set.add(m[1]);
      }
    });
  }
  walk(enumsRoot);
  TAKT_ENUM_TYPE_NAMES = set;
  return set;
}

/**
 * 扫描 Domain/Entities 下全部 public class TaktXxx
 * @returns {Set<string>}
 */
function loadTaktEntityClassNames() {
  if (TAKT_ENTITY_CLASS_NAMES) {
    return TAKT_ENTITY_CLASS_NAMES;
  }
  const set = new Set();
  function walk(dir) {
    fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(full);
        return;
      }
      if (!entry.name.startsWith('Takt') || !entry.name.endsWith('.cs')) {
        return;
      }
      const content = fs.readFileSync(full, 'utf-8');
      const classMatch = content.match(/public\s+class\s+(Takt\w+)/);
      if (classMatch) {
        set.add(classMatch[1]);
      }
    });
  }
  walk(CONFIG.entitiesRoot);
  TAKT_ENTITY_CLASS_NAMES = set;
  return set;
}

/**
 * 判断是否为 Shared 枚举类型（须生成 entity.* 字段键）
 * @param {string} bareType
 * @returns {boolean}
 */
function isTaktEnumType(bareType) {
  return loadTaktEnumTypeNames().has(bareType.replace(/\?$/, '').trim());
}

/**
 * 判断是否为 Domain 实体类型
 * @param {string} bareType
 * @returns {boolean}
 */
function isTaktDomainEntityType(bareType) {
  return loadTaktEntityClassNames().has(bareType.replace(/\?$/, '').trim());
}

/**
 * 解析集合元素类型（List/ICollection&lt;T&gt; 等）
 * @param {string} csharpType
 * @returns {string | null}
 */
function unwrapCollectionInnerType(csharpType) {
  const bare = csharpType.replace(/\?$/, '').trim();
  const match = bare.match(/^(?:List|IList|ICollection|IEnumerable|HashSet)<(.+)>$/);
  if (!match) {
    return null;
  }
  return match[1].replace(/\?$/, '').trim();
}

/**
 * 无 [Navigate] 的实体引用/集合引用应跳过（ORM 导航，非业务字段标签）
 * @param {string} bareType 去掉尾部 ? 的类型名
 * @returns {boolean}
 */
function isSkippedEntityNavigationReference(bareType) {
  const bare = bareType.replace(/\?$/, '').trim();
  if (!/^Takt[A-Z]/.test(bare)) {
    return false;
  }
  if (isTaktEnumType(bare)) {
    return false;
  }
  return isTaktDomainEntityType(bare);
}

/**
 * 判断是否为无 [Navigate] 的实体集合导航（应跳过）
 * @param {string} csharpType 属性 C# 类型（可含 ?）
 * @returns {boolean}
 */
function isEntityCollectionNavigationType(csharpType) {
  const inner = unwrapCollectionInnerType(csharpType);
  if (!inner) {
    return false;
  }
  return isSkippedEntityNavigationReference(inner);
}

// ========================================
// 工具
// ========================================

function pascalToKebab(str) {
  return str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

function csharpDocToXml(block) {
  if (!block) {
    return '';
  }
  return block
    .split('\n')
    .map((line) => line.replace(/^\s*\/\/\/?\s?/, '').trim())
    .filter(Boolean)
    .join('\n');
}

function extractSummary(xmlComment) {
  if (!xmlComment) {
    return '';
  }
  const match = xmlComment.match(/<summary>([\s\S]*?)<\/summary>/);
  if (!match) {
    return '';
  }
  return match[1].replace(/\s+/g, ' ').trim();
}

/**
 * 提取 XML summary 首行（用于 entity.*._self，避免多行说明拼成冗长文案）
 * @param {string} xmlComment
 * @returns {string}
 */
function extractSummaryFirstLine(xmlComment) {
  if (!xmlComment) {
    return '';
  }
  const match = xmlComment.match(/<summary>([\s\S]*?)<\/summary>/);
  if (!match) {
    return '';
  }
  const lines = match[1]
    .split(/\r?\n/)
    .map((line) => line.trim())
    .filter(Boolean);
  return lines[0] || '';
}

function normalizeSummaryForLabel(summary) {
  if (!summary) {
    return '';
  }
  let text = summary.split(/\r?\n/)[0].trim();
  text = text.split(/[，,。；;（(]/)[0].trim();
  text = text.replace(/实体$/, '').trim();
  text = text.replace(/^代表.+$/, '').trim();
  return text || summary;
}

/**
 * 由实体 summary 首行生成简洁中文 entity.*._self 文案（如「菜单实体」→「菜单信息」）
 * @param {string} firstLine summary 首行
 * @returns {string}
 */
function buildEntitySelfLabelZh(firstLine) {
  if (!firstLine) {
    return '';
  }
  let text = firstLine.trim();
  text = text.split(/[，,。；;（(]/)[0].trim();
  if (text.endsWith('实体')) {
    return `${text.slice(0, -2)}信息`;
  }
  if (text.endsWith('信息') || text.endsWith('日志')) {
    return text;
  }
  return `${text}信息`;
}

/**
 * 字段 TranslationText 简洁标签：去掉括号说明、逗号/说明性从句（完整说明写入 ContextNote）
 * @param {string} text 原始 summary 首行或 locales 字段文案
 * @returns {string}
 */
function normalizeEntityFieldLabel(text) {
  if (!text) {
    return '';
  }
  let label = text.split(/\r?\n/)[0].trim();
  label = label.replace(/[（(][^）)]*[）)]/g, '').trim();
  label = label.split(/[，,。；;]/)[0].trim();
  label = label.split(
    /\s+(?:使用|用于|代表|定义|支持|注意|例如|如|参照|参考|详见|即|指|存储|记录|标识)/,
  )[0].trim();
  return label || text.split(/\r?\n/)[0].trim();
}

/**
 * 由实体 slug 或 PascalCase 短名生成英文 entity.*._self 文案（如 AssyDefectDetail → Assy Defect Detail Information）
 * @param {string} shortPascal 去 Takt 前缀的 PascalCase
 * @returns {string}
 */
function slugToEnSelfLabel(shortPascal) {
  const spaced = shortPascal
    .replace(/([a-z])([A-Z])/g, '$1 $2')
    .replace(/([A-Z]+)([A-Z][a-z])/g, '$1 $2');
  const title = spaced.charAt(0).toUpperCase() + spaced.slice(1);
  return `${title} Information`;
}

/**
 * 按语言生成 entity.*._self 展示名（不读 frontend page.title，不拼接 summary 全文）
 * @param {string} firstLine 实体 class summary 首行
 * @param {string} shortPascal 去 Takt 前缀的 PascalCase 短名
 * @param {string} culture BCP47
 * @returns {string}
 */
function buildEntitySelfLabel(firstLine, shortPascal, culture) {
  const zh = buildEntitySelfLabelZh(firstLine);
  if (!zh) {
    return culture === 'en-US' ? slugToEnSelfLabel(shortPascal) : shortPascal.toLowerCase();
  }
  if (culture === 'en-US') {
    return slugToEnSelfLabel(shortPascal);
  }
  return zh;
}

function escapeCsharpString(value) {
  return (value || '').replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

/**
 * 实体类名 → 去 Takt 前缀的 PascalCase 短名（仅用于 en-US 展示文案，非 I18nKey）
 * @param {string} className Takt 实体类名
 * @returns {string}
 */
function entityClassToShortPascal(className) {
  return className.replace(/^Takt/, '');
}

function entityClassToSeedClassName(className) {
  const short = className.replace(/^Takt/, '');
  return `Takt${short}I18nSeedData`;
}

function entityClassToSeedFileName(className) {
  return `${entityClassToSeedClassName(className)}.cs`;
}

// ========================================
// 解析实体
// ========================================

/**
 * 从属性块文本解析 SugarColumn.ColumnDescription
 * @param {string} propSegment summary 至属性声明片段
 * @returns {string}
 */
function extractColumnDescription(propSegment) {
  const match = propSegment.match(/ColumnDescription\s*=\s*"([^"]+)"/);
  return match?.[1]?.trim() ?? '';
}

function parseEntityProperties(classBody) {
  const properties = [];
  /** 支持 List&lt;TaktXxx&gt;?、TaktYesNo、string? 等 */
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>[\s\S]*?\b([\w]+(?:<[^>]+>)?\??)\s+(\w+)\s*\{[\s\S]*?get;\s*set;/g;
  let match;

  while ((match = propertyRegex.exec(classBody)) !== null) {
    const csharpType = match[2].trim();
    const bare = csharpType.replace(/\?$/, '');
    const propSegment = match[0];
    const hasNavigate = /\[Navigate\s*\(/.test(propSegment);

    if (!hasNavigate && (isSkippedEntityNavigationReference(bare) || isEntityCollectionNavigationType(csharpType))) {
      continue;
    }

    const name = match[3];
    if (ENTITY_BASE_FIELDS.has(name)) {
      continue;
    }

    const summaryXml = csharpDocToXml(`/// <summary>${match[1]}</summary>`);
    properties.push({
      name,
      camelName: pascalToCamel(name),
      summary: extractSummary(summaryXml),
      summaryFirstLine: extractSummaryFirstLine(summaryXml),
      columnDescription: extractColumnDescription(propSegment),
      isNavigate: hasNavigate,
    });
  }

  return properties;
}

function extractClassBody(content, openBraceIndex) {
  let depth = 1;
  let i = openBraceIndex + 1;
  while (i < content.length && depth > 0) {
    const ch = content[i];
    if (ch === '{') {
      depth += 1;
    } else if (ch === '}') {
      depth -= 1;
    }
    i += 1;
  }
  return content.slice(openBraceIndex + 1, i - 1);
}

/**
 * 实体命名空间 → 与 Entities 下相同的子目录片段
 * @param {string} entityNamespace
 */
function entityNamespaceToDirParts(entityNamespace) {
  const suffix = entityNamespace.replace(/^Takt\.Domain\.Entities\.?/, '');
  return suffix ? suffix.split('.').filter(Boolean) : [];
}

function resolveSeedOutput(entity) {
  const seedDirParts = entityNamespaceToDirParts(entity.entityNamespace);
  const seedNamespaceSuffix = seedDirParts.length ? `.${seedDirParts.join('.')}` : '';
  const seedNamespace = `Takt.Infrastructure.Data.Seeds.I18nSeedData${seedNamespaceSuffix}`;
  const outDir = path.join(CONFIG.i18nSeedRoot, ...seedDirParts);
  return {
    seedDirParts,
    seedNamespace,
    outDir,
    outFile: path.join(outDir, entityClassToSeedFileName(entity.className)),
  };
}

function parseEntityFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const classHeader = parseEntityClassHeaderFromCsContent(content);
  if (!classHeader) {
    return null;
  }

  const namespaceMatch = content.match(/namespace\s+([\w.]+);/);
  const entityNamespace = namespaceMatch ? namespaceMatch[1] : '';
  const className = classHeader.className;
  const classHeaderMatch = content.match(ENTITY_CLASS_HEADER_REGEX);
  const openBraceIndex = classHeaderMatch.index + classHeaderMatch[0].length - 1;
  const classBody = extractClassBody(content, openBraceIndex);
  const beforeClass = content.slice(0, classHeaderMatch.index);
  const docBlocks = [...beforeClass.matchAll(/((?:\s*\/\/\/[^\n]*\n)+)/g)];
  const classDocBlock = docBlocks.length ? docBlocks[docBlocks.length - 1][1] : '';
  const classDocXml = csharpDocToXml(classDocBlock);
  const classSummaryFirstLine = extractSummaryFirstLine(classDocXml);
  const classSummary = normalizeSummaryForLabel(classSummaryFirstLine || extractSummary(classDocXml));

  const entity = {
    className,
    classSummary,
    classSummaryFirstLine,
    entityNamespace,
    slug: entityClassToSlug(className),
    shortPascal: entityClassToShortPascal(className),
    taktModule: resolveTaktModule(entityNamespace),
    properties: parseEntityProperties(classBody),
    filePath,
  };
  return { ...entity, ...resolveSeedOutput(entity) };
}

// ========================================
// 读取 frontend locales（若存在）
// ========================================

/**
 * 根据实体推断 locales 相对路径并尝试加载
 * @param {object} entity
 * @returns {Record<string, Record<string, string>>}
 */
function loadFrontendLocaleFields(entity) {
  const result = createEmptyCultureFieldMap();
  const relativeNs = entity.entityNamespace.replace('Takt.Domain.Entities.', '');
  const nsParts = relativeNs.split('.').map((p) => pascalToKebab(p));
  const slugKebab = pascalToKebab(entity.className.replace(/^Takt/, ''));

  const candidates = [
    path.join(CONFIG.frontendRoot, 'src', 'locales', ...nsParts, slugKebab),
    path.join(CONFIG.frontendRoot, 'src', 'locales', nsParts[0], slugKebab),
    path.join(CONFIG.frontendRoot, 'src', 'locales', nsParts[0], entity.slug),
  ];

  let localeDir = null;
  for (const dir of candidates) {
    if (fs.existsSync(path.join(dir, 'zh-CN.ts')) || fs.existsSync(path.join(dir, 'en-US.ts'))) {
      localeDir = dir;
      break;
    }
  }

  if (!localeDir) {
    return { localeDir: null, fields: result, title: {} };
  }

  const title = {};
  CONFIG.cultures.forEach((culture) => {
    const filePath = path.join(localeDir, `${culture}.ts`);
    if (!fs.existsSync(filePath)) {
      return;
    }
    const content = fs.readFileSync(filePath, 'utf-8');
    const titleMatch = content.match(/title:\s*['"]([^'"]+)['"]/);
    if (titleMatch) {
      title[culture] = titleMatch[1];
    }
    const fieldBlockMatch =
      content.match(/field:\s*\{([\s\S]*?)\n\s*\},\s*\n\s*placeholder:/) ||
      content.match(/field:\s*\{([\s\S]*?)\n\s*\}/);
    if (!fieldBlockMatch) {
      return;
    }
    const fieldBody = fieldBlockMatch[1];
    const propRegex = /(\w+):\s*['"]([^'"]+)['"]/g;
    let m;
    while ((m = propRegex.exec(fieldBody)) !== null) {
      result[culture][m[1]] = m[2];
    }
  });

  return { localeDir, fields: result, title };
}

/**
 * 为 entity.*._self 解析各语言简洁实体名
 * @param {object} entity
 * @returns {Record<string, string>}
 */
function resolveEntitySelfTranslations(entity) {
  const texts = {};
  CONFIG.cultures.forEach((culture) => {
    texts[culture] = buildEntitySelfLabel(entity.classSummaryFirstLine, entity.shortPascal, culture);
  });
  return texts;
}

/**
 * 为单条字段翻译解析各语言文案
 * TranslationText：SugarColumn.ColumnDescription（优先）→ frontend locales → 属性名
 * ContextNote 由调用方使用属性 XML summary 全文
 * @param {object} entity
 * @param {string} propCamel 属性 camelCase
 * @param {string} columnDescription SugarColumn.ColumnDescription
 * @param {object} localeData loadFrontendLocaleFields 结果
 */
function resolveFieldTranslations(entity, propCamel, columnDescription, summaryFirstLine, localeData) {
  const { fields } = localeData;
  const texts = {};

  const localeKey = LOCALE_FIELD_ALIASES[propCamel] || propCamel;
  const pickLocale = (cultureMap) => cultureMap?.[localeKey] || cultureMap?.[propCamel];
  const summaryLabel = normalizeEntityFieldLabel(summaryFirstLine);

  CONFIG.cultures.forEach((culture) => {
    texts[culture] =
      columnDescription ||
      pickLocale(fields[culture]) ||
      pickLocale(fields['zh-CN']) ||
      summaryLabel ||
      propCamel;
  });

  return texts;
}

/**
 * 构建翻译元组列表
 * @param {object} entity
 */
function buildTranslationTuples(entity) {
  const localeData = loadFrontendLocaleFields(entity);
  const tuples = [];
  let sortOrder = 1;

  const selfTexts = resolveEntitySelfTranslations(entity);
  if (selfTexts) {
    CONFIG.cultures.forEach((culture) => {
      tuples.push({
        i18nKey: buildEntityI18nKey(entity.slug, '_self'),
        culture,
        text: withCultureTranslationSuffix(selfTexts[culture], culture),
        contextNote: '实体名称',
        sortOrder: sortOrder,
      });
    });
    sortOrder += 1;
  }

  const seenFieldKeys = new Set();
  entity.properties.forEach((prop) => {
    const i18nKey = buildEntityI18nKey(entity.slug, prop.camelName);
    if (seenFieldKeys.has(i18nKey)) {
      return;
    }
    seenFieldKeys.add(i18nKey);
    const texts = resolveFieldTranslations(
      entity,
      prop.camelName,
      prop.columnDescription,
      prop.summaryFirstLine,
      localeData,
    );
    CONFIG.cultures.forEach((culture) => {
      tuples.push({
        i18nKey,
        culture,
        text: withCultureTranslationSuffix(texts[culture], culture),
        contextNote: sanitizeXmlDocPlainText(prop.summary || prop.name),
        sortOrder,
      });
    });
    sortOrder += 1;
  });

  return { tuples, localeDir: localeData.localeDir };
}

// ========================================
// 生成 C# 种子类
// ========================================

function generateSeedClassContent(entity, translationData) {
  const seedClass = entityClassToSeedClassName(entity.className);
  const today = new Date().toISOString().split('T')[0];
  const localeHint = translationData.localeDir
    ? `已对齐前端 locales：${path.relative(CONFIG.frontendRoot, translationData.localeDir).replace(/\\/g, '/')}`
    : '无对应 frontend locales；TranslationText 取自 ColumnDescription，ContextNote 取自属性 XML summary';

  const lines = [];
  lines.push('// ========================================');
  lines.push('// 项目名称：节拍智控·TaktWise');
  lines.push(`// 命名空间：${entity.seedNamespace}`);
  lines.push(`// 文件名称：${entityClassToSeedFileName(entity.className)}`);
  lines.push(`// 创建时间：${today}`);
  lines.push('// 创建人：Takt365(Auto Generated)');
  lines.push(`// 功能描述：${entity.className} 实体字段国际化种子（${localeHint}）`);
  lines.push('// ');
  lines.push('// 版权信息：Copyright (c) 2025 Takt  All rights reserved.');
  lines.push('// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。');
  lines.push('// ========================================');
  lines.push('');
  lines.push('using System.Linq;');
  lines.push('using Microsoft.Extensions.DependencyInjection;');
  lines.push('using Takt.Domain.Entities.Foundation;');
  lines.push('using Takt.Domain.Interfaces;');
  lines.push('using Takt.Domain.Repositories;');
  lines.push('using Takt.Shared.Helpers;');
  lines.push('');
  lines.push(`namespace ${entity.seedNamespace};`);
  const resourceGroup = buildTranslationResourceGroup(entity.seedDirParts);
  const resourceType = TRANSLATION_RESOURCE_TYPE_FRONTEND;
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// ${entity.className} 实体国际化翻译种子（键前缀 entity.${entity.slug}.*）`);
  lines.push('/// 幂等性：存在则更新，不存在则创建');
  lines.push('/// </summary>');
  lines.push(`public class ${seedClass} : ITaktSeedDataCoordinator`);
  lines.push('{');
  lines.push('    /// <summary>');
  lines.push('    /// 执行顺序（实体翻译种子，位于部门翻译之后）');
  lines.push('    /// </summary>');
  lines.push('    public int Order => 52;');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 初始化实体字段翻译种子');
  lines.push('    /// </summary>');
  lines.push('    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)');
  lines.push('    {');
  lines.push(`        TaktLogger.Information("开始初始化 ${entity.className} 实体国际化翻译种子...");`);
  lines.push('');
  lines.push('        if (string.IsNullOrEmpty(tenantCode))');
  lines.push('        {');
  lines.push('            TaktLogger.Warning("租户编码为空，跳过实体国际化翻译种子初始化");');
  lines.push('            return (0, 0);');
  lines.push('        }');
  lines.push('');
  lines.push('        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktTranslation>>();');
  lines.push('        var cultureRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCulture>>();');
  lines.push('        var cultureIdByCode = (await cultureRepository.GetListAsync(c => c.TenantCode == tenantCode))');
  lines.push('            .ToDictionary(c => c.CultureCode, c => c.Id);');
  lines.push('        int insertCount = 0;');
  lines.push('        int updateCount = 0;');
  lines.push('');
  lines.push(`        TaktLogger.Information("正在为租户 {TenantCode} 初始化 ${entity.slug} 实体翻译...", tenantCode);`);
  lines.push('');
  lines.push(`        foreach (var item in Get${entity.className.replace(/^Takt/, '')}Translations())`);
  lines.push('        {');
  lines.push('            if (!cultureIdByCode.TryGetValue(item.CultureCode, out var cultureId))');
  lines.push('            {');
  lines.push('                TaktLogger.Warning("未找到区域文化 {CultureCode}，跳过翻译 {I18nKey}", item.CultureCode, item.I18nKey);');
  lines.push('                continue;');
  lines.push('            }');
  lines.push('');
  lines.push('            var (translation, i, u) = await CreateOrUpdateTranslationAsync(');
  lines.push('                repository,');
  lines.push('                tenantCode,');
  lines.push('                cultureId,');
  lines.push('                item);');
  lines.push('            insertCount += i;');
  lines.push('            updateCount += u;');
  lines.push('        }');
  lines.push('');
  lines.push(`        TaktLogger.Information("${entity.className} 实体翻译种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);`);
  lines.push('        return (insertCount, updateCount);');
  lines.push('    }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push(`    /// ${entity.className} 实体翻译列表（${CONFIG.cultures.join(' / ')}）`);
  lines.push(`    /// I18nKey：entity.${entity.slug}._self / entity.${entity.slug}.{{field}}；ResourceGroup=${resourceGroup}；ResourceType=${resourceType}`);
  lines.push('    /// </summary>');
  lines.push(`    private static List<TranslationSeedItem> Get${entity.className.replace(/^Takt/, '')}Translations()`);
  lines.push('    {');
  lines.push('        return new List<TranslationSeedItem>');
  lines.push('        {');

  let lastKey = '';
  translationData.tuples.forEach((item) => {
    if (item.i18nKey !== lastKey && lastKey !== '') {
      lines.push('');
    }
    lastKey = item.i18nKey;
    const comment = item.i18nKey.includes('._self') ? '实体名称' : item.contextNote;
    lines.push(`            // ${item.i18nKey}`);
    lines.push(
      `            new TranslationSeedItem("${item.i18nKey}", "${item.culture}", "${escapeCsharpString(item.text)}", "${escapeCsharpString(comment)}"),`
    );
  });

  lines.push('        };');
  lines.push('    }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 填充 TaktTranslation 全部业务字段（含租户基类字段）');
  lines.push('    /// </summary>');
  lines.push('    private static void ApplyTranslationFields(');
  lines.push('        TaktTranslation translation,');
  lines.push('        string tenantCode,');
  lines.push('        long cultureId,');
  lines.push('        TranslationSeedItem item)');
  lines.push('    {');
  lines.push('        translation.TenantCode = tenantCode;');
  lines.push('        translation.CultureId = cultureId;');
  lines.push('        translation.CultureCode = item.CultureCode;');
  lines.push('        translation.I18nKey = item.I18nKey;');
  lines.push('        translation.TranslationText = item.TranslationText;');
  lines.push(`        translation.ResourceGroup = "${resourceGroup}";`);
  lines.push(`        translation.ResourceType = "${resourceType}";`);
  lines.push('        translation.ContextNote = item.ContextNote;');
  lines.push('        translation.ExtField = null;');
  lines.push('        translation.Remark = null;');
  lines.push('        translation.IsDeleted = 0;');
  lines.push('        translation.DeletedBy = null;');
  lines.push('        translation.DeletedAt = null;');
  lines.push('    }');
  lines.push('');
  lines.push('    private static async Task<(TaktTranslation Translation, int InsertCount, int UpdateCount)> CreateOrUpdateTranslationAsync(');
  lines.push('        ITaktTenantSeedRepository<TaktTranslation> repository,');
  lines.push('        string tenantCode,');
  lines.push('        long cultureId,');
  lines.push('        TranslationSeedItem item)');
  lines.push('    {');
  lines.push('        var translation = await repository.FirstAsync(t =>');
  lines.push('            t.TenantCode == tenantCode &&');
  lines.push('            t.I18nKey == item.I18nKey &&');
  lines.push('            t.CultureCode == item.CultureCode);');
  lines.push('');
  lines.push('        if (translation == null)');
  lines.push('        {');
  lines.push('            translation = new TaktTranslation();');
  lines.push('            ApplyTranslationFields(translation, tenantCode, cultureId, item);');
  lines.push('            translation = await repository.CreateAsync(translation);');
  lines.push('            return (translation, 1, 0);');
  lines.push('        }');
  lines.push('');
  lines.push('        ApplyTranslationFields(translation, tenantCode, cultureId, item);');
  lines.push('        await repository.UpdateAsync(translation);');
  lines.push('        return (translation, 0, 1);');
  lines.push('    }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 翻译种子项（对应 TaktTranslation 全部可写字段，CultureId 由 SeedAsync 解析）');
  lines.push('    /// </summary>');
  lines.push('    private sealed record TranslationSeedItem(');
  lines.push('        string I18nKey,');
  lines.push('        string CultureCode,');
  lines.push('        string TranslationText,');
  lines.push('        string? ContextNote);');
  lines.push('}');
  lines.push('');

  return lines.join('\n');
}

// ========================================
// 扫描与写入
// ========================================

function scanEntities() {
  const results = [];
  function walk(dir) {
    fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(full);
        return;
      }
      if (!entry.name.startsWith('Takt') || !entry.name.endsWith('.cs')) {
        return;
      }
      const entityShort = entry.name.replace(/^Takt/, '').replace(/\.cs$/, '');
      if (isManualDtoEntity(entityShort)) {
        console.log(`⏭️  跳过手工维护 i18n: Takt${entityShort}`);
        return;
      }
      const parsed = parseEntityFile(full);
      if (parsed) {
        results.push(parsed);
      }
    });
  }
  walk(CONFIG.entitiesRoot);
  return results;
}

function generateForEntity(entity) {
  const { outFile, outDir } = resolveSeedOutput(entity);
  const translationData = buildTranslationTuples(entity);
  const content = generateSeedClassContent(entity, translationData);
  const writeResult = writeGeneratedFile(outFile, content);
  const actionLabel = writeResult.created ? '已创建' : '已更新';
  console.log(
    `✅ ${actionLabel}: ${outFile}（${translationData.tuples.length / CONFIG.cultures.length} 个键 × ${CONFIG.cultures.length} 语言 = ${translationData.tuples.length} 条）`,
  );
  return {
    created: writeResult.created,
    updated: writeResult.updated,
    keyCount: translationData.tuples.length / CONFIG.cultures.length,
  };
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-entity-i18n-seed.cjs [--all|-all]

说明:
  - 本脚本固定全量扫描 Domain/Entities 下全部 Takt* 实体
  - 仅支持无参或 --all / -all，不支持其他参数

示例:
  node scripts/generate-entity-i18n-seed.cjs
  node scripts/generate-entity-i18n-seed.cjs --all
`);
}

/**
 * 解析 CLI：仅允许无参或 --all / -all
 */
function parseArgs() {
  parseAllOnlyGenerateArgs(printUsage);
}

// ========================================
// 主流程
// ========================================

console.log('🚀 从实体全量生成 Entity I18n 种子（--all）...\n');
logGeneratedFileWritePolicy();

try {
  loadTaktEnumTypeNames();
  loadTaktEntityClassNames();
  parseArgs();
  const entities = scanEntities();
  if (entities.length === 0) {
    console.error('❌ 未找到任何可解析实体');
    process.exit(1);
  }

  console.log(`📦 模式: 全量（--all）共 ${entities.length} 个实体\n`);

  let created = 0;
  let updated = 0;

  entities.forEach((entity) => {
    const r = generateForEntity(entity);
    if (r.updated) {
      updated += 1;
    } else {
      created += 1;
    }
  });

  console.log(`\n📊 已创建 ${created} 个种子类，已更新 ${updated} 个`);
  console.log('✨ 完成！类名由 Autofac 自动注册（*SeedData + ITaktSeedDataCoordinator）。');
} catch (err) {
  console.error('❌ 失败:', err);
  process.exit(1);
}

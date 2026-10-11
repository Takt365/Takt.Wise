// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-script-common.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成脚本公共工具（文件写入、Service 单数 / Controller 复数命名）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');

/**
 * REST 控制器复数特例（键为实体短名单数，与 ITaktXxxService 一致）
 */
const CONTROLLER_PLURAL_OVERRIDES = {
  User: 'Users',
  Auth: 'Auths',
  Company: 'Companies',
  Culture: 'Cultures',
  TranslationMessage: 'TranslationMessages',
  HolidayTheme: 'HolidayThemes',
  DataDictAll: 'DataDictAlls',
  Analysis: 'Analyses',
  PerfAnalysis: 'PerfAnalyses',
  News: 'News',
};

/**
 * 将实体短名复数化（用于 Takt{Plural}Controller 与 api/Takt{Plural} 路由）
 * @param {string} entityShort 如 Holiday、DictData、HolidayTheme
 * @returns {string} 如 Holidays、DictDatas、HolidayThemes
 */
function pluralizeEntityShort(entityShort) {
  if (!entityShort) {
    return entityShort;
  }
  if (CONTROLLER_PLURAL_OVERRIDES[entityShort]) {
    return CONTROLLER_PLURAL_OVERRIDES[entityShort];
  }
  if (entityShort.endsWith('y') && !/[aeiou]y$/i.test(entityShort)) {
    return `${entityShort.slice(0, -1)}ies`;
  }
  if (entityShort.endsWith('Company')) {
    return `${entityShort.slice(0, -7)}Companies`;
  }
  if (/(s|x|z|ch|sh)$/i.test(entityShort)) {
    return `${entityShort}es`;
  }
  return `${entityShort}s`;
}

/**
 * 从控制器路由段（无 Takt 前缀）还原实体短名单数
 * @param {string} segment 如 Holidays、Users、Companies
 * @returns {string} 如 Holiday、User、Company
 */
function singularizeControllerSegment(segment) {
  if (!segment) {
    return segment;
  }
  for (const [singular, plural] of Object.entries(CONTROLLER_PLURAL_OVERRIDES)) {
    if (segment === plural) {
      return singular;
    }
  }
  if (segment.endsWith('ies')) {
    return `${segment.slice(0, -3)}y`;
  }
  if (segment.endsWith('Companies')) {
    return `${segment.slice(0, -10)}Company`;
  }
  if (segment.endsWith('es')) {
    const base = segment.slice(0, -2);
    // 仅 -ss/-x/-z/-ch/-sh + es（如 Classes、Boxes）；勿用末尾单 s（Warehouses→Warehouse 走下方 -s）
    if (/(ss|x|z|ch|sh)$/i.test(base)) {
      return base;
    }
  }
  if (segment.endsWith('s') && segment.length > 1) {
    return segment.slice(0, -1);
  }
  return segment;
}

/**
 * 由实体完整类名生成控制器类名（复数 + Controller）
 * @param {string} entityName 如 TaktHoliday、TaktUser
 * @returns {string} 如 TaktHolidaysController、TaktUsersController
 */
function getControllerClassName(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}Controller`;
}

/**
 * 由实体完整类名生成 [controller] 路由段（含 Takt 前缀）
 * @param {string} entityName 如 TaktHoliday
 * @returns {string} 如 TaktHolidays
 */
function getControllerRouteSegment(entityName) {
  const entityShort = entityName.replace(/^Takt/, '');
  return `Takt${pluralizeEntityShort(entityShort)}`;
}

/**
 * 从控制器类名解析实体短名（单数，用于 DTO/权限/前端 types 文件名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @returns {string} 如 Holiday
 */
function entityShortFromControllerClassName(controllerClassName) {
  const segment = controllerClassName.replace(/^Takt/, '').replace(/Controller$/, '');
  return singularizeControllerSegment(segment);
}

/**
 * CLI 实体前缀是否匹配控制器类名（仅接受复数控制器名）
 * @param {string} controllerClassName 如 TaktHolidaysController
 * @param {string} entityPrefix 如 Holiday
 * @returns {boolean}
 */
function matchControllerForEntityPrefix(controllerClassName, entityPrefix) {
  return controllerClassName === getControllerClassName(`Takt${entityPrefix}`);
}

/**
 * 全栈 generate-*.cjs / generate-all.cjs 统一文件写入策略
 * @type {string}
 */
const GENERATED_FILE_WRITE_POLICY = '不存在则创建，已存在则覆盖更新';

/**
 * 启动时打印统一写入策略
 */
function logGeneratedFileWritePolicy() {
  console.log(`📝 写入策略：${GENERATED_FILE_WRITE_POLICY}\n`);
}

/**
 * 写入生成文件：目录不存在则创建；文件已存在则覆盖更新
 * @param {string} filePath 目标文件绝对或相对路径
 * @param {string} content 文件内容
 * @returns {{ created: boolean, updated: boolean }}
 */
function writeGeneratedFile(filePath, content) {
  const dir = path.dirname(filePath);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
  const existed = fs.existsSync(filePath);
  fs.writeFileSync(filePath, content, 'utf-8');
  return { created: !existed, updated: existed };
}

const SCRIPTS_GEN_DIR = __dirname;
const SCRIPTS_DIR = path.resolve(__dirname, '..');
const REPO_ROOT = path.resolve(__dirname, '../..');
const DEFAULT_BACKEND_ROOT = path.join(REPO_ROOT, 'backend', 'src');
const DEFAULT_FRONTEND_ROOT = path.join(REPO_ROOT, 'frontend');

/** 全量生成脚本允许的 CLI 参数（无参 / --all / -all） */
const ALL_ONLY_CLI_ALIASES = new Set(['--all', '--ALL', '-all', '-ALL']);

/**
 * 解析全量生成 CLI（无参或 --all / -all，禁止其他参数）
 * @param {string[]} args process.argv.slice(2)
 * @param {() => void} printUsage
 */
function parseAllOnlyGenerateArgsFromArgv(args, printUsage) {
  for (const arg of args) {
    if (ALL_ONLY_CLI_ALIASES.has(arg)) {
      continue;
    }
    console.error(`❌ 不支持参数: ${arg}（仅允许无参或 --all）`);
    printUsage();
    process.exit(1);
  }
}

/**
 * 从 process.argv 解析全量生成 CLI（无参或 --all / -all）
 * @param {() => void} printUsage
 */
function parseAllOnlyGenerateArgs(printUsage) {
  parseAllOnlyGenerateArgsFromArgv(process.argv.slice(2), printUsage);
}

/**
 * 从 process.argv 解析单实体代码生成 CLI（禁止 --all）
 * @param {string[]} args
 * @param {() => void} [printUsage]
 * @param {{ allowViewPath?: boolean }} [opts]
 * @returns {{ entityPrefix: string, force: boolean, dryRun: boolean, viewPath: string|null }}
 */
function parseSingleEntityGenerateArgsFromArgv(args, printUsage, opts = {}) {
  const options = { entityPrefix: null, force: false, dryRun: false, viewPath: null };
  for (let i = 0; i < args.length; i += 1) {
    const arg = args[i];
    if (arg === '--force') {
      options.force = true;
      continue;
    }
    if (arg === '--dry-run') {
      options.dryRun = true;
      continue;
    }
    if (opts.allowViewPath && arg === '--view-path') {
      options.viewPath = args[i + 1] || null;
      i += 1;
      continue;
    }
    if (!arg.startsWith('--')) {
      console.error(`❌ 未知参数: ${arg}`);
      process.exit(1);
    }
    const value = arg.slice(2);
    if (value.toLowerCase() === 'all') {
      console.error('❌ 已禁用 --all，请指定单个实体，例如 --CostCenter');
      process.exit(1);
    }
    if (value.startsWith('Takt')) {
      console.error('❌ 实体名不要带 Takt 前缀，例如 --CostCenter');
      process.exit(1);
    }
    if (options.entityPrefix) {
      console.error('❌ 只能指定一个实体名');
      process.exit(1);
    }
    options.entityPrefix = value;
  }
  if (!options.entityPrefix) {
    console.error('❌ 请指定 --<实体名>，例如 --CostCenter');
    if (printUsage) {
      printUsage();
    }
    process.exit(1);
  }
  return options;
}

/**
 * 解析单实体代码生成 CLI（禁止 --all）
 * @param {() => void} [printUsage]
 * @returns {{ entityPrefix: string, force: boolean, dryRun: boolean, viewPath: string|null }}
 */
function parseSingleEntityGenerateArgs(printUsage) {
  return parseSingleEntityGenerateArgsFromArgv(process.argv.slice(2), printUsage);
}

/**
 * 构建传递给子脚本的单实体 CLI 参数
 * @param {{ entityPrefix: string, force?: boolean, dryRun?: boolean, viewPath?: string|null }} options
 * @returns {string[]}
 */
function buildSingleEntityChildArgs(options) {
  const args = [`--${options.entityPrefix}`];
  if (options.force) {
    args.push('--force');
  }
  if (options.dryRun) {
    args.push('--dry-run');
  }
  if (options.viewPath) {
    args.push('--view-path', options.viewPath);
  }
  return args;
}

/**
 * 将 DtoBase / EntityBase 名称映射为表格 entityScope
 * @param {string} baseName
 * @returns {'tenant'|'tenant-core'|'tenant-culture'|'tenant-plant'|'company'|'approval'}
 */
function entityBaseNameToScope(baseName) {
  if (!baseName) {
    return 'company';
  }
  if (baseName.includes('Approval')) {
    return 'approval';
  }
  if (baseName.includes('Company')) {
    return 'company';
  }
  if (baseName.includes('TenantCore')) {
    return 'tenant-core';
  }
  if (baseName.includes('TenantCulture')) {
    return 'tenant-culture';
  }
  if (baseName.includes('TenantPlant')) {
    return 'tenant-plant';
  }
  if (baseName.includes('Tenant')) {
    return 'tenant';
  }
  return 'company';
}

/**
 * 三种实体/DTO 基类是否含公司隔离（CompanyCode）
 * - TaktTenantEntityBase / TaktTenantDtoBase → false（仅 TenantCode）
 * - TaktCompany* / TaktApproval* → true（TenantCode + CompanyCode）
 * @param {string|null|undefined} dtoOrEntityBase 如 TaktTenantDtoBase / TaktCompanyEntityBase
 * @returns {boolean}
 */
function dtoBaseHasCompanyIsolation(dtoOrEntityBase) {
  if (!dtoOrEntityBase || typeof dtoOrEntityBase !== 'string') {
    return false;
  }
  if (dtoOrEntityBase.includes('Tenant')) {
    return false;
  }
  return dtoOrEntityBase.includes('Company') || dtoOrEntityBase.includes('Approval');
}

/**
 * Options / QueryExpression 隔离用 DtoBase：以 Domain 实体基类为准（CompanyCode 是否存在以实体为准）
 * @param {string|null} dtoBaseFromDto 从 *Dto 声明解析的基类
 * @param {string|null|undefined} entityBaseFromFile 从实体文件解析的 EntityBase
 * @returns {string|null}
 */
function resolveIsolationDtoBase(dtoBaseFromDto, entityBaseFromFile) {
  if (entityBaseFromFile) {
    const fromEntity = resolveDtoBaseFromEntityBase(entityBaseFromFile);
    if (fromEntity) {
      return fromEntity;
    }
  }
  return dtoBaseFromDto || null;
}

/**
 * 查找 Domain 实体文件
 * @param {string} entityPascal 如 CostCenter
 * @param {string} [backendRoot]
 * @returns {string|null}
 */
function findDomainEntityFile(entityPascal, backendRoot = DEFAULT_BACKEND_ROOT) {
  const entityFileName = `Takt${entityPascal}.cs`;
  const entitiesRoot = path.join(backendRoot, 'Takt.Domain', 'Entities');
  if (!fs.existsSync(entitiesRoot)) {
    return null;
  }
  /** @param {string} dir */
  function search(dir) {
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const full = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        const found = search(full);
        if (found) {
          return found;
        }
      } else if (entry.name === entityFileName) {
        return full;
      }
    }
    return null;
  }
  return search(entitiesRoot);
}

/**
 * 实体类头正则（租户四组合 Core/Culture/Plant/默认 + Company/Approval；含 Increment / Guid）
 * 组合：4 Core / 2 Culture / 3 Plant / 1 TenantEntity（默认）
 */
const ENTITY_CLASS_HEADER_REGEX =
  /public\s+(?:sealed\s+|abstract\s+)?class\s+(Takt\w+)\s*:\s*(Takt(?:Tenant(?:Core|Culture|Plant)?|Company|Approval)Entity(?:Increment|Guid)?Base)\s*\{/;

/** EntityBase / EntityIncrementBase / EntityGuidBase → DTO 基类（对齐 TaktDtoBase 四组合） */
const ENTITY_BASE_TO_DTO_BASE = {
  TaktTenantCoreEntityBase: 'TaktTenantCoreDtoBase',
  TaktTenantCultureEntityBase: 'TaktTenantCultureDtoBase',
  TaktTenantPlantEntityBase: 'TaktTenantPlantDtoBase',
  TaktTenantEntityBase: 'TaktTenantDtoBase',
  TaktCompanyEntityBase: 'TaktCompanyDtoBase',
  TaktApprovalEntityBase: 'TaktApprovalDtoBase',
  TaktTenantCoreEntityIncrementBase: 'TaktTenantCoreDtoBase',
  TaktTenantCultureEntityIncrementBase: 'TaktTenantCultureDtoBase',
  TaktTenantPlantEntityIncrementBase: 'TaktTenantPlantDtoBase',
  TaktTenantEntityIncrementBase: 'TaktTenantDtoBase',
  TaktCompanyEntityIncrementBase: 'TaktCompanyDtoBase',
  TaktApprovalEntityIncrementBase: 'TaktApprovalDtoBase',
  TaktTenantCoreEntityGuidBase: 'TaktTenantCoreDtoBase',
  TaktTenantCultureEntityGuidBase: 'TaktTenantCultureDtoBase',
  TaktTenantPlantEntityGuidBase: 'TaktTenantPlantDtoBase',
  TaktTenantEntityGuidBase: 'TaktTenantDtoBase',
  TaktCompanyEntityGuidBase: 'TaktCompanyDtoBase',
  TaktApprovalEntityGuidBase: 'TaktApprovalDtoBase',
};

/**
 * 去掉 Increment / Guid 主键变体后缀，得到标准 EntityBase 名
 * @param {string|null|undefined} entityBase
 * @returns {string}
 */
function stripEntityBasePkVariant(entityBase) {
  if (typeof entityBase !== 'string' || !entityBase) {
    return '';
  }
  return entityBase.replace(/Increment|Guid/g, '');
}

/**
 * 租户基类是否含 RelatedPlant（组合 1 默认 / 组合 3 Plant）
 * @param {string|null|undefined} entityBase
 * @returns {boolean}
 */
function entityBaseHasRelatedPlant(entityBase) {
  const normalized = stripEntityBasePkVariant(entityBase);
  return (
    normalized === 'TaktTenantEntityBase' || normalized === 'TaktTenantPlantEntityBase'
  );
}

/**
 * 租户/公司/审批基类是否含 CultureCode（组合 1·2；公司/审批）
 * @param {string|null|undefined} entityBase
 * @returns {boolean}
 */
function entityBaseHasCultureCode(entityBase) {
  if (isCompanyOrApprovalEntityBase(entityBase)) {
    return true;
  }
  const normalized = stripEntityBasePkVariant(entityBase);
  return (
    normalized === 'TaktTenantEntityBase' || normalized === 'TaktTenantCultureEntityBase'
  );
}

/**
 * 实体基类可 Stamp 的隔离列（与 TaktEntityBase 四组合 + 公司/审批一致；不含审计列）
 * @param {string|null|undefined} entityBase
 * @returns {readonly string[]}
 */
function getIsolationStampFieldNamesForEntityBase(entityBase) {
  if (isCompanyOrApprovalEntityBase(entityBase)) {
    return ['TenantCode', 'CompanyCode', 'CultureCode', 'PlantCode'];
  }
  /** @type {string[]} */
  const fields = ['TenantCode'];
  if (entityBaseHasCultureCode(entityBase)) {
    fields.push('CultureCode');
  }
  if (entityBaseHasRelatedPlant(entityBase)) {
    fields.push('RelatedPlant');
  }
  return fields;
}

/**
 * 是否租户级实体基类（含四组合与 Increment / Guid）
 * @param {string|null|undefined} entityBase
 * @returns {boolean}
 */
function isTenantEntityBase(entityBase) {
  return typeof entityBase === 'string' && entityBase.includes('Tenant');
}

/**
 * 是否公司或审批级实体基类（含 Increment / Guid）
 * @param {string|null|undefined} entityBase
 * @returns {boolean}
 */
function isCompanyOrApprovalEntityBase(entityBase) {
  if (typeof entityBase !== 'string') {
    return false;
  }
  return entityBase.includes('Company') || entityBase.includes('Approval');
}

/**
 * 归一化为标准 EntityBase（去掉 Increment / Guid；保留租户四组合名）
 * @param {string|null|undefined} entityBase
 * @returns {string|null}
 */
function normalizeEntityBaseKind(entityBase) {
  const normalized = stripEntityBasePkVariant(entityBase);
  if (!normalized) {
    return null;
  }
  if (
    normalized === 'TaktTenantCoreEntityBase' ||
    normalized === 'TaktTenantCultureEntityBase' ||
    normalized === 'TaktTenantPlantEntityBase' ||
    normalized === 'TaktTenantEntityBase' ||
    normalized === 'TaktCompanyEntityBase' ||
    normalized === 'TaktApprovalEntityBase'
  ) {
    return normalized;
  }
  return null;
}

/** 领域实体基类本身（非业务实体；DTO/Validator/i18n 生成必须跳过） */
const ENTITY_BASE_CLASS_NAMES = new Set(Object.keys(ENTITY_BASE_TO_DTO_BASE));

/**
 * 从 C# 实体源码解析类名与基类
 * @param {string} content
 * @returns {{ className: string, entityBase: string }|null}
 */
function parseEntityClassHeaderFromCsContent(content) {
  const match = content.match(ENTITY_CLASS_HEADER_REGEX);
  if (!match) {
    return null;
  }
  const className = match[1];
  // TaktIncrementBase.cs 中 abstract 基类也匹配正则，须排除以免生成空 Validators/i18n
  if (ENTITY_BASE_CLASS_NAMES.has(className)) {
    return null;
  }
  return { className, entityBase: match[2] };
}

/**
 * EntityBase / EntityIncrementBase → Application DTO 基类
 * @param {string} entityBase
 * @returns {string}
 */
function resolveDtoBaseFromEntityBase(entityBase) {
  return ENTITY_BASE_TO_DTO_BASE[entityBase] || 'TaktTenantDtoBase';
}

/**
 * 从 C# 实体文件解析 EntityBase（保留租户四组合；去掉 Increment/Guid）
 * @param {string} entityFilePath
 * @returns {string}
 */
function parseEntityBaseFromCsFile(entityFilePath) {
  const content = fs.readFileSync(entityFilePath, 'utf-8');
  const header = parseEntityClassHeaderFromCsContent(content);
  if (header) {
    return (
      normalizeEntityBaseKind(header.entityBase) || stripEntityBasePkVariant(header.entityBase) || 'TaktCompanyEntityBase'
    );
  }
  // 回退：按专名优先（Culture/Plant/Core 须先于裸 TenantEntity）
  const fallbackOrdered = [
    'TaktTenantCultureEntityIncrementBase',
    'TaktTenantCultureEntityGuidBase',
    'TaktTenantCultureEntityBase',
    'TaktTenantPlantEntityIncrementBase',
    'TaktTenantPlantEntityGuidBase',
    'TaktTenantPlantEntityBase',
    'TaktTenantCoreEntityIncrementBase',
    'TaktTenantCoreEntityGuidBase',
    'TaktTenantCoreEntityBase',
    'TaktTenantEntityIncrementBase',
    'TaktTenantEntityGuidBase',
    'TaktTenantEntityBase',
    'TaktApprovalEntityIncrementBase',
    'TaktApprovalEntityGuidBase',
    'TaktApprovalEntityBase',
    'TaktCompanyEntityIncrementBase',
    'TaktCompanyEntityGuidBase',
    'TaktCompanyEntityBase',
  ];
  for (const baseName of fallbackOrdered) {
    if (content.includes(`: ${baseName}`)) {
      return normalizeEntityBaseKind(baseName) || baseName;
    }
  }
  return 'TaktCompanyEntityBase';
}

/**
 * 从 types 主实体 interface 块解析 entityScope
 * @param {string} typesContent
 * @param {string} entityPascal
 * @returns {'tenant'|'company'|'approval'|null}
 */
function resolveEntityScopeFromTypesInterface(typesContent, entityPascal) {
  const ifaceRe = new RegExp(
    `/\\*\\*[\\s\\S]*?\\*/\\s*export interface ${entityPascal}\\b(?:\\s+extends\\s+(\\w+))?`,
  );
  const match = typesContent.match(ifaceRe);
  if (!match) {
    return null;
  }
  const jsdocBase = match[0].match(/继承\s+Takt(\w+DtoBase)/);
  if (jsdocBase) {
    return entityBaseNameToScope(`Takt${jsdocBase[1]}`);
  }
  if (match[1]) {
    return entityBaseNameToScope(match[1]);
  }
  return null;
}

/**
 * 解析实体基类作用域（优先 Domain 实体，其次 types JSDoc / extends）
 * @param {string} entityPascal
 * @param {string} [typesContent]
 * @param {string} [backendRoot]
 * @returns {'tenant'|'company'|'approval'}
 */
function resolveEntityScope(entityPascal, typesContent = '', backendRoot = DEFAULT_BACKEND_ROOT) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (entityFile) {
    return entityBaseNameToScope(parseEntityBaseFromCsFile(entityFile));
  }
  const fromTypes = resolveEntityScopeFromTypesInterface(typesContent, entityPascal);
  if (fromTypes) {
    return fromTypes;
  }
  return 'company';
}

const { stripSeeCref } = require('./xml-cref-strip.cjs');

/**
 * 将 XML 文档中的 see cref 转为可读纯文本（避免 cref 落盘到 DTO/i18n/JSDoc）
 * @param {string} text
 * @returns {string}
 */
function sanitizeXmlDocPlainText(text) {
  if (!text) {
    return '';
  }
  let result = stripSeeCref(text);
  result = result.replace(
    /适配\s*TaktFlowInstance\.Id/gi,
    '对应 takt_workflow_instance 主键 Id',
  );
  return result.replace(/\s{2,}/g, ' ').trim();
}

/**
 * PascalCase → camelCase
 * @param {string} str
 * @returns {string}
 */
function pascalToCamel(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

/**
 * 全局：属性 camelCase → I18nKey 末段（与 generate-entity-i18n-seed.cjs 一致）
 */
const ENTITY_FIELD_I18N_SEGMENT = {
  passwordHash: 'password',
  employeeId: 'employeeid',
  dictCode: 'code',
  typeCode: 'code',
  themeCode: 'code',
};

/**
 * 按实体 slug 覆盖末段（slug 须全小写）
 */
const ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG = {
  menu: {
    i18nKey: 'l10nkey',
    componentPath: 'component',
    externalUrl: 'linkurl',
  },
};

/**
 * 去掉属性名中与实体 slug 重复的前缀（tenantName + tenant → name）
 * @param {string} camelName
 * @param {string} entitySlug 全小写 slug
 */
function stripEntitySlugPrefixFromCamel(camelName, entitySlug) {
  if (!entitySlug) {
    return camelName;
  }
  const prefix = entitySlug.toLowerCase();
  const lower = camelName.toLowerCase();
  if (!lower.startsWith(prefix) || camelName.length <= prefix.length) {
    return camelName;
  }
  const rest = camelName.slice(prefix.length);
  return rest.charAt(0).toLowerCase() + rest.slice(1);
}

/**
 * 将 C# 属性 camelCase 解析为 I18nKey 末段（全小写 a-z0-9）
 * @param {string} camelName _self 或属性 camelCase
 * @param {string} [entitySlug] 实体 slug（全小写）
 */
function resolveEntityFieldI18nSegment(camelName, entitySlug) {
  if (camelName === '_self') {
    return '_self';
  }
  const slugOverrides = entitySlug ? ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG[entitySlug] : null;
  let segment =
    slugOverrides?.[camelName] ??
    ENTITY_FIELD_I18N_SEGMENT[camelName] ??
    stripEntitySlugPrefixFromCamel(camelName, entitySlug);
  segment = String(segment).toLowerCase();
  if (!/^[a-z0-9]+$/.test(segment)) {
    throw new Error(`I18n 键末段非法（须全小写 a-z0-9）：${camelName} → ${segment}`);
  }
  return segment;
}

/**
 * 实体类名 → I18nKey 实体 slug（全小写 a-z0-9，如 TaktItAsset → itasset）
 * @param {string} className Takt 实体类名或短名（可带 Takt 前缀）
 */
function entityClassToSlug(className) {
  const short = className.replace(/^Takt/, '');
  const slug = pascalToCamel(short).toLowerCase();
  if (!/^[a-z0-9]+$/.test(slug)) {
    throw new Error(`I18n 实体 slug 非法（须全小写 a-z0-9）：${className} → ${slug}`);
  }
  return slug;
}

/**
 * 生成 entity.* 完整 I18nKey（与 TaktXxxI18nSeedData 一致）
 * @param {string} slug 实体 slug（全小写）
 * @param {string} segment _self 或属性 camelCase
 */
function buildEntityI18nKey(slug, segment) {
  const normalizedSlug = String(slug).toLowerCase();
  if (!/^[a-z0-9]+$/.test(normalizedSlug)) {
    throw new Error(`I18n 实体 slug 非法（须全小写 a-z0-9）：${slug}`);
  }
  return `entity.${normalizedSlug}.${resolveEntityFieldI18nSegment(segment, normalizedSlug)}`;
}

/**
 * entity.*._self 键
 * @param {string} slug 实体 slug（全小写）或 Takt 类名
 */
function buildEntitySelfI18nKey(slug) {
  const normalizedSlug = slug.startsWith('Takt') ? entityClassToSlug(slug) : String(slug).toLowerCase();
  return buildEntityI18nKey(normalizedSlug, '_self');
}

/** TaktTranslation.ResourceType 种子固定值（字典 sys_resource_type：frontend） */
const TRANSLATION_RESOURCE_TYPE_FRONTEND = 'frontend';

/**
 * 由 I18nSeedData 业务目录片段生成 ResourceGroup（取命名空间路径最后一段，如 Logging、Foundation）
 * @param {readonly string[]} seedDirParts 如 ['Statistics', 'Logging'] → Logging；根目录种子传空数组 → Common
 * @returns {string}
 */
function buildTranslationResourceGroup(seedDirParts) {
  if (!seedDirParts || seedDirParts.length === 0) {
    return 'Common';
  }
  return seedDirParts[seedDirParts.length - 1];
}

/**
 * TaktModule 数值（与 Takt.Shared.Enums.TaktModule 一致；ApiModule 等仍用 int）
 */
const TAKT_MODULE_INT = {
  Dashboard: 0,
  Identity: 1,
  Routine: 2,
  Accounting: 3,
  Logistics: 4,
  HumanResource: 5,
  Workflow: 6,
  Code: 7,
  Foundation: 8,
  Statistics: 9,
  Entity: 10,
};

/**
 * TaktAppSide 数值（与 Takt.Shared.Enums.TaktAppSide 一致）
 */
const TAKT_APP_SIDE_INT = {
  Frontend: 0,
  Backend: 1,
};

/** appsettings 分页节名（与 TaktPagedOptions.SectionName 一致） */
const TAKT_PAGED_OPTIONS_SECTION = 'Paged';

/**
 * vue-i18n 消息内空对象字面量（显示为 {}）
 * token：{'{'} + {'}'}
 */
const VUE_I18N_LITERAL_EMPTY_OBJECT = "{'{'}{'}'}";

/**
 * vue-i18n 消息内 JSON 示例（字面量块含开括号与内容，闭合 } 用 {'}'} token）
 * 显示为 {"customCode":"A001"}
 */
const VUE_I18N_JSON_EXAMPLE_SIMPLE = "{'{\"customCode\":\"A001\"'}{'}'}";

/**
 * vue-i18n 消息内 JSON 示例（含布尔字段）
 * 显示为 {"customCode":"A001","enabled":true}
 */
const VUE_I18N_JSON_EXAMPLE_WITH_ENABLED = "{'{\"customCode\":\"A001\",\"enabled\":true'}{'}'}";

/**
 * common.page.form.placeholder.extfield 种子译文（与 TaktCommonI18nSeedData 对齐）
 * @returns {Array<[string, string, string, string]>}
 */
function buildCommonExtFieldPlaceholderI18nTuples() {
  return [
    ['common.page.form.placeholder.extfield', 'zh-CN', `示例：${VUE_I18N_JSON_EXAMPLE_SIMPLE}（键与字符串值须英文双引号，选填）`, '通用表单'],
    ['common.page.form.placeholder.extfield', 'en-US', `e.g. ${VUE_I18N_JSON_EXAMPLE_SIMPLE} (use double quotes for keys/strings)`, 'Common Form'],
    ['common.page.form.placeholder.extfield', 'ja-JP', `例: ${VUE_I18N_JSON_EXAMPLE_SIMPLE}（キーと文字列値は二重引用符）`, '共通フォーム'],
    ['common.page.form.placeholder.extfield', 'zh-HK', `示例：${VUE_I18N_JSON_EXAMPLE_SIMPLE}（鍵與字串值須英文雙引號，選填）`, '通用表单'],
  ];
}

/**
 * common.page.entity.extfieldhint 种子译文（与 TaktCommonI18nSeedData 对齐）
 * @returns {Array<[string, string, string, string]>}
 */
function buildCommonExtFieldHintI18nTuples() {
  return [
    [
      'common.page.entity.extfieldhint',
      'zh-CN',
      `请输入合法 JSON 对象字符串：整体须为 ${VUE_I18N_LITERAL_EMPTY_OBJECT}；键名与字符串值必须使用英文双引号；数字、布尔、null 不加引号。示例：${VUE_I18N_JSON_EXAMPLE_WITH_ENABLED}。不支持数组 [] 或裸字符串。选填，最多 400 字符。`,
      '通用实体',
    ],
    [
      'common.page.entity.extfieldhint',
      'en-US',
      `Enter a valid JSON object: wrap in ${VUE_I18N_LITERAL_EMPTY_OBJECT}; use double quotes for keys and string values; numbers/booleans/null are unquoted. Example: ${VUE_I18N_JSON_EXAMPLE_WITH_ENABLED}. Arrays and plain strings are not supported. Optional, max 400 characters.`,
      'Common Entity',
    ],
    [
      'common.page.entity.extfieldhint',
      'ja-JP',
      `有効な JSON オブジェクトを入力してください：全体は ${VUE_I18N_LITERAL_EMPTY_OBJECT}；キーと文字列値は二重引用符必須；数値・真偽値・null は引用符なし。例: ${VUE_I18N_JSON_EXAMPLE_WITH_ENABLED}。配列 [] や裸の文字列は不可。任意、最大 400 文字。`,
      '共通エンティティ',
    ],
    [
      'common.page.entity.extfieldhint',
      'zh-HK',
      `請輸入合法 JSON 物件字串：整體須為 ${VUE_I18N_LITERAL_EMPTY_OBJECT}；鍵名與字串值必須使用英文雙引號；數字、布林、null 不加引號。示例：${VUE_I18N_JSON_EXAMPLE_WITH_ENABLED}。不支援陣列 [] 或裸字串。選填，最多 400 字元。`,
      '通用实体',
    ],
  ];
}

/**
 * 解析 TaktModule 名称为 int 字典码
 * @param {string} moduleName 如 Identity、Statistics
 * @returns {number}
 */
function resolveTaktModuleInt(moduleName) {
  if (!Object.prototype.hasOwnProperty.call(TAKT_MODULE_INT, moduleName)) {
    throw new Error(`未知 TaktModule：${moduleName}`);
  }
  return TAKT_MODULE_INT[moduleName];
}

/**
 * 三个实体基类字段（与 frontend/src/utils/tableColumns.ts ENTITY_BASE_FIELDS 保持同步，不含 id）
 */
const TENANT_CORE_AUDIT_FIELDS = [
  'tenantCode', 'ExtField', 'remark',
  'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
];

const ENTITY_BASE_FIELDS = {
  tenant: ['relatedPlant', 'cultureCode', ...TENANT_CORE_AUDIT_FIELDS],
  'tenant-core': [...TENANT_CORE_AUDIT_FIELDS],
  'tenant-culture': ['cultureCode', ...TENANT_CORE_AUDIT_FIELDS],
  'tenant-plant': ['relatedPlant', ...TENANT_CORE_AUDIT_FIELDS],
  company: [
    'tenantCode', 'companyCode', 'cultureCode', 'plantCode', 'ExtField', 'remark',
    'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
  approval: [
    'tenantCode', 'companyCode', 'cultureCode', 'plantCode', 'ExtField', 'remark',
    'approvalStatus', 'initiatorId', 'initiatedAt', 'approvalOpinion', 'approvedBy', 'approvedAt',
    'flowInstanceId',
    'createdBy', 'createdAt', 'updatedBy', 'updatedAt', 'isDeleted', 'deletedBy', 'deletedAt',
  ],
};

/**
 * 当 kebab 名与 modulePath 任一路径段重复时去掉前缀（api/types/views 共用）
 * 例：logistics/serial + serial-inbound → inbound
 * 例：logistics/quality/cost + quality-assurance → assurance
 * 例：logistics/maintenance + maintenance-work-order → work-order
 * @param {string} entityKebab
 * @param {string} modulePath 如 logistics/serial、logistics/quality/cost
 * @returns {string}
 */
function stripModulePrefixFromEntityKebab(entityKebab, modulePath) {
  if (!entityKebab || !modulePath) {
    return entityKebab;
  }
  let result = entityKebab;
  const segments = modulePath.split('/').filter(Boolean);
  for (const seg of segments) {
    const redundantPrefix = `${seg.toLowerCase()}-`;
    if (result.startsWith(redundantPrefix)) {
      result = result.slice(redundantPrefix.length);
    }
  }
  return result;
}

/**
 * 解析前端 api/types 文件名（去模块目录重复前缀）
 * @param {string} rawKebab 控制器或 DTO 推导的 kebab 名
 * @param {string} modulePath 模块目录，如 logistics/serial
 * @returns {string}
 */
function resolveFrontendModuleFileName(rawKebab, modulePath) {
  return stripModulePrefixFromEntityKebab(rawKebab, modulePath);
}

/**
 * 模块末级目录与实体 kebab 同名（如 routine/announcement + announcement）→ 不再追加一段
 * @param {string} entityKebab
 * @param {string} modulePath
 * @returns {boolean}
 */
function isModuleLeafSameAsEntityKebab(entityKebab, modulePath) {
  if (!entityKebab || !modulePath) {
    return false;
  }
  const segments = String(modulePath).split('/').filter(Boolean);
  return segments.length > 0 && segments[segments.length - 1] === entityKebab;
}

/**
 * 解析 views / ComponentPath 目录（禁止 routine/announcement/announcement）
 * @param {string} modulePath
 * @param {string} entityKebab
 * @returns {string}
 */
function resolveViewModulePath(modulePath, entityKebab) {
  if (!modulePath || modulePath === '.') {
    return entityKebab;
  }
  if (isModuleLeafSameAsEntityKebab(entityKebab, modulePath)) {
    return modulePath;
  }
  return `${modulePath}/${entityKebab}`;
}

/**
 * 解析 frontend api/types 落盘相对路径（禁止 routine/announcement/announcement.ts）
 * @param {string} modulePath DTO/控制器扫描目录，如 routine/announcement
 * @param {string} fileKebab 实体 kebab 文件名
 * @returns {{ relDir: string, file: string, importPath: string }}
 */
function resolveFrontendOutputRelPath(modulePath, fileKebab) {
  const file = resolveFrontendModuleFileName(fileKebab, modulePath);
  if (!modulePath) {
    return { relDir: '', file, importPath: file };
  }
  if (isModuleLeafSameAsEntityKebab(file, modulePath) || isModuleLeafSameAsEntityKebab(fileKebab, modulePath)) {
    const segments = modulePath.split('/').filter(Boolean);
    const parentDir = segments.slice(0, -1).join('/');
    return {
      relDir: parentDir,
      file,
      importPath: parentDir ? `${parentDir}/${file}` : file,
    };
  }
  return {
    relDir: modulePath,
    file,
    importPath: `${modulePath}/${file}`,
  };
}

/**
 * kebab-case → camelCase（api/types 文件主名）
 * @param {string} kebab
 * @returns {string}
 */
function kebabToCamel(kebab) {
  const parts = String(kebab || '')
    .split(/[-_/]/)
    .filter(Boolean);
  if (!parts.length) {
    return '';
  }
  return (
    parts[0].toLowerCase() +
    parts
      .slice(1)
      .map((s) => s.charAt(0).toUpperCase() + s.slice(1).toLowerCase())
      .join('')
  );
}

/**
 * camelCase → kebab-case
 * @param {string} camel
 * @returns {string}
 */
function camelToKebab(camel) {
  return String(camel || '')
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .replace(/_/g, '-')
    .toLowerCase();
}

/**
 * 规范化实体文件主名（kebab 或 camel → camelCase；去掉误加的 Api / .api / .types 后缀）
 * @param {string} stem
 * @returns {string}
 */
function toFrontendEntityFileCamel(stem) {
  let s = String(stem || '').replace(/\\/g, '/').split('/').pop() || '';
  // 先剥完整扩展名，再剥残留的 .api / .types（resolveModuleContext 曾只去 .ts 留下 announcement.api）
  s = s
    .replace(/\.d\.ts$/i, '')
    .replace(/\.types\.ts$/i, '')
    .replace(/\.api\.ts$/i, '')
    .replace(/\.ts$/i, '')
    .replace(/\.types$/i, '')
    .replace(/\.api$/i, '');
  // 兼容「announcement.api」被 Api$ 剥成「announcement.」的历史坏输入
  s = s.replace(/\.+$/g, '');
  s = s.replace(/Types$/i, '').replace(/Api$/i, '');
  s = s.replace(/\.+$/g, '').trim();
  if (!s) {
    return '';
  }
  if (/[-_]/.test(s)) {
    return kebabToCamel(s);
  }
  return s;
}

/**
 * 实体 kebab/camel → API 文件名（含扩展名）：`user.api.ts`（不以 Api 重复缀主名）
 * @param {string} fileKebabOrCamel
 * @returns {string}
 */
function toFrontendApiFileName(fileKebabOrCamel) {
  return `${toFrontendEntityFileCamel(fileKebabOrCamel)}.api.ts`;
}

/**
 * 实体 kebab/camel → types 文件名（含扩展名）：`user.types.ts`
 * @param {string} fileKebabOrCamel
 * @returns {string}
 */
function toFrontendTypesFileName(fileKebabOrCamel) {
  return `${toFrontendEntityFileCamel(fileKebabOrCamel)}.types.ts`;
}

/**
 * resolveFrontendOutputRelPath.importPath（kebab）→ API import 路径
 * @param {string} importPath 如 identity/user
 * @returns {string} identity/user.api
 */
function toFrontendApiImportPath(importPath) {
  const normalized = String(importPath || '').replace(/\\/g, '/').replace(/^\//, '');
  const parts = normalized.split('/').filter(Boolean);
  const file = parts.pop() || '';
  const camel = toFrontendEntityFileCamel(file);
  return [...parts, `${camel}.api`].join('/');
}

/**
 * resolveFrontendOutputRelPath.importPath（kebab）→ types import 路径
 * @param {string} importPath 如 identity/user
 * @returns {string} identity/user.types
 */
function toFrontendTypesImportPath(importPath) {
  const normalized = String(importPath || '').replace(/\\/g, '/').replace(/^\//, '');
  const parts = normalized.split('/').filter(Boolean);
  const file = parts.pop() || '';
  const camel = toFrontendEntityFileCamel(file);
  return [...parts, `${camel}.types`].join('/');
}

/**
 * 从 types 文件名解析实体 camel（user.types.ts → user）
 * @param {string} fileName
 * @returns {string}
 */
function parseFrontendTypesFileCamel(fileName) {
  return toFrontendEntityFileCamel(path.basename(String(fileName || '')));
}

/**
 * 从 API 文件名解析实体 camel（user.api.ts → user）
 * @param {string} fileName
 * @returns {string}
 */
function parseFrontendApiFileCamel(fileName) {
  return toFrontendEntityFileCamel(path.basename(String(fileName || '')));
}

/**
 * 实体短名（Pascal）→ API import 尾段
 * @param {string} entityPascal
 * @returns {string} user.api
 */
function entityPascalToApiImportTail(entityPascal) {
  return `${pascalToCamel(entityPascal)}.api`;
}

/**
 * 实体短名（Pascal）→ types import 尾段
 * @param {string} entityPascal
 * @returns {string} user.types
 */
function entityPascalToTypesImportTail(entityPascal) {
  return `${pascalToCamel(entityPascal)}.types`;
}

/**
 * 实体短名 → 权限末段（小写连写，如 SerialInbound→serialinbound）
 * @param {string} entityShort
 * @returns {string}
 */
function entityShortToPermissionSlug(entityShort) {
  return entityShort.replace(/([a-z0-9])([A-Z])/g, '$1$2').toLowerCase();
}

/**
 * 权限末段去重：仅全字匹配模块目录段（materials 与 material 视为不同词，禁止复数剥 s 假去重）
 * @param {string} entitySlug entityShortToPermissionSlug 结果
 * @param {string} moduleSegment 模块路径段（如 materials、conferencecenter）
 * @returns {string}
 */
function dedupePermissionEntitySlugAgainstModule(entitySlug, moduleSegment) {
  if (!entitySlug || !moduleSegment) {
    return entitySlug;
  }
  const slug = String(entitySlug).toLowerCase();
  const seg = String(moduleSegment).toLowerCase();
  if (slug === seg) {
    return '';
  }
  if (slug.startsWith(seg)) {
    return slug.slice(seg.length);
  }
  if (slug.endsWith(seg) && slug.length > seg.length) {
    return slug.slice(0, -seg.length);
  }
  return slug;
}

/** 服务路径目录段 → 权限目录段（小写连写，CustomerService→service） */
const PERMISSION_PATH_PART_ALIASES = {
  CustomerService: 'service',
};

/**
 * 由服务 pathParts 提取权限目录段（不含领域）
 * @param {string[]} pathParts 如 ['Routine','NewsCenter']
 * @returns {string[]}
 */
function buildPermissionModuleSegments(pathParts) {
  if (!pathParts || pathParts.length <= 1) {
    return [];
  }
  return pathParts.slice(1).map((part) => PERMISSION_PATH_PART_ALIASES[part] || part.toLowerCase());
}

/**
 * 按服务 pathParts 解析权限实体段（逐段与目录去重）
 * @param {string} entityShort
 * @param {string[]} pathParts
 * @returns {string}
 */
function resolvePermissionEntitySlugFromPathParts(entityShort, pathParts) {
  let slug = entityShortToPermissionSlug(entityShort);
  for (const seg of buildPermissionModuleSegments(pathParts)) {
    slug = dedupePermissionEntitySlugAgainstModule(slug, seg);
  }
  return slug;
}

/**
 * 组装权限前缀：领域[:目录…][:实体]；实体为空或与任一段目录相同则省略实体段
 * 格式：业务领域:业务目录:实体:操作（操作由调用方追加）
 * @param {string} domain
 * @param {string[]} subdirs
 * @param {string} entitySlug
 * @returns {string}
 */
function assemblePermissionBase(domain, subdirs, entitySlug) {
  const domainSeg = String(domain || '').trim().toLowerCase();
  const dirs = (subdirs || []).map((seg) => String(seg || '').trim().toLowerCase()).filter(Boolean);
  let slug = String(entitySlug || '').trim().toLowerCase();
  if (slug && dirs.some((seg) => seg === slug)) {
    slug = '';
  }
  if (!dirs.length) {
    return slug ? `${domainSeg}:${slug}` : domainSeg;
  }
  const subdirPath = dirs.join(':');
  if (!slug) {
    return `${domainSeg}:${subdirPath}`;
  }
  return `${domainSeg}:${subdirPath}:${slug}`;
}

/**
 * 按模块路径解析权限实体末段（逐段与目录名去重）
 * @param {string} entityShort PascalCase 实体短名
 * @param {string} modulePath 模块目录（不含领域），如 logistics/quality/cost
 * @returns {string}
 */
function resolvePermissionEntitySlugFromModulePath(entityShort, modulePath) {
  let slug = entityShortToPermissionSlug(entityShort);
  const segments = String(modulePath || '').split('/').filter(Boolean);
  for (const seg of segments) {
    slug = dedupePermissionEntitySlugAgainstModule(slug, seg);
  }
  return slug;
}

/**
 * 字段 XML 摘要是否声明「前端表单：规则下拉 + 只读编码」
 * 判定：含「自动通过 … TaktNumbering」；排除 MIME 回填（TaktFileUploadEngine）
 * @param {string|null|undefined} summaryOrDoc
 * @returns {boolean}
 */
function isTaktNumberingAutoFormFieldSummary(summaryOrDoc) {
  const text = String(summaryOrDoc || '');
  if (!/自动通过\s*TaktNumbering/.test(text)) {
    return false;
  }
  if (/\bMIME\b/i.test(text) || /MIME\s*类型/.test(text)) {
    return false;
  }
  return true;
}

/**
 * 字段 XML 摘要是否声明「上传引擎按 MIME 自动取号」（非表单选规则）
 * @param {string|null|undefined} summaryOrDoc
 * @returns {boolean}
 */
function isTaktNumberingMimeEngineFieldSummary(summaryOrDoc) {
  const text = String(summaryOrDoc || '');
  if (!/自动通过\s*TaktNumbering/.test(text)) {
    return false;
  }
  return /\bMIME\b/i.test(text) || /MIME\s*类型/.test(text);
}

module.exports = {
  GENERATED_FILE_WRITE_POLICY,
  logGeneratedFileWritePolicy,
  writeGeneratedFile,
  CONTROLLER_PLURAL_OVERRIDES,
  pluralizeEntityShort,
  singularizeControllerSegment,
  getControllerClassName,
  getControllerRouteSegment,
  entityShortFromControllerClassName,
  matchControllerForEntityPrefix,
  entityBaseNameToScope,
  dtoBaseHasCompanyIsolation,
  isTenantEntityBase,
  isCompanyOrApprovalEntityBase,
  entityBaseHasRelatedPlant,
  entityBaseHasCultureCode,
  getIsolationStampFieldNamesForEntityBase,
  stripEntityBasePkVariant,
  normalizeEntityBaseKind,
  resolveIsolationDtoBase,
  findDomainEntityFile,
  parseEntityBaseFromCsFile,
  parseEntityClassHeaderFromCsContent,
  resolveDtoBaseFromEntityBase,
  ENTITY_CLASS_HEADER_REGEX,
  ENTITY_BASE_TO_DTO_BASE,
  resolveEntityScopeFromTypesInterface,
  resolveEntityScope,
  ENTITY_BASE_FIELDS,
  DEFAULT_BACKEND_ROOT,
  DEFAULT_FRONTEND_ROOT,
  REPO_ROOT,
  SCRIPTS_DIR,
  SCRIPTS_GEN_DIR,
  isTaktNumberingAutoFormFieldSummary,
  isTaktNumberingMimeEngineFieldSummary,
  parseSingleEntityGenerateArgsFromArgv,
  parseSingleEntityGenerateArgs,
  parseAllOnlyGenerateArgsFromArgv,
  parseAllOnlyGenerateArgs,
  buildSingleEntityChildArgs,
  sanitizeXmlDocPlainText,
  pascalToCamel,
  ENTITY_FIELD_I18N_SEGMENT,
  ENTITY_PROPERTY_I18N_SEGMENT_BY_SLUG,
  stripEntitySlugPrefixFromCamel,
  resolveEntityFieldI18nSegment,
  entityClassToSlug,
  buildEntityI18nKey,
  buildEntitySelfI18nKey,
  TRANSLATION_RESOURCE_TYPE_FRONTEND,
  buildTranslationResourceGroup,
  TAKT_MODULE_INT,
  TAKT_APP_SIDE_INT,
  resolveTaktModuleInt,
  TAKT_PAGED_OPTIONS_SECTION,
  VUE_I18N_LITERAL_EMPTY_OBJECT,
  VUE_I18N_JSON_EXAMPLE_SIMPLE,
  VUE_I18N_JSON_EXAMPLE_WITH_ENABLED,
  buildCommonExtFieldPlaceholderI18nTuples,
  buildCommonExtFieldHintI18nTuples,
  stripModulePrefixFromEntityKebab,
  resolveFrontendModuleFileName,
  isModuleLeafSameAsEntityKebab,
  resolveViewModulePath,
  resolveFrontendOutputRelPath,
  kebabToCamel,
  camelToKebab,
  toFrontendEntityFileCamel,
  toFrontendApiFileName,
  toFrontendTypesFileName,
  toFrontendApiImportPath,
  toFrontendTypesImportPath,
  parseFrontendTypesFileCamel,
  parseFrontendApiFileCamel,
  entityPascalToApiImportTail,
  entityPascalToTypesImportTail,
  entityShortToPermissionSlug,
  dedupePermissionEntitySlugAgainstModule,
  PERMISSION_PATH_PART_ALIASES,
  buildPermissionModuleSegments,
  resolvePermissionEntitySlugFromPathParts,
  assemblePermissionBase,
  resolvePermissionEntitySlugFromModulePath,
};

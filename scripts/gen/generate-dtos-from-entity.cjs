// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-dtos-from-entity.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Takt.Domain 实体自动生成 Takt.Application/Dtos/*Dtos.cs；不存在则创建，已存在则覆盖更新
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  logGeneratedFileWritePolicy,
  parseSingleEntityGenerateArgs,
  sanitizeXmlDocPlainText,
  parseEntityClassHeaderFromCsContent,
  resolveDtoBaseFromEntityBase,
  isTenantEntityBase,
  isCompanyOrApprovalEntityBase,
  entityBaseHasRelatedPlant,
  entityBaseHasCultureCode,
  ENTITY_CLASS_HEADER_REGEX,
  isTaktNumberingAutoFormFieldSummary,
} = require('./generate-script-common.cjs');
const {
  isRbacJunctionEntity,
  isStandaloneChildVueEntity,
  assertNotRbacJunctionEntityCli,
  assertNotManualDtoEntityCli,
} = require('./generate-entity-exclusions.cjs');
const { isTransposableEntity, appendTransposedDtoBlock } = require('./generate-transposed-support.cjs');
const {
  resolveRbacCreateFieldFromNav,
  appendInverseRbacCreateFields,
  RBAC_JUNCTION_NAMESPACE,
} = require('./rbac-parent-config.cjs');
const { syncAllRbacParentEntityNavigations } = require('./generate-entity-rbac-navigations.cjs');
const {
  isSharedEnumType,
  entityUsesSharedEnumsFromProperties,
  findEntityStatusProperty,
} = require('./generate-enum-common.cjs');

// ========================================
// 配置
// ========================================

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  entitiesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities'),
  dtosRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Dtos'),
};

/** 实体基类字段（不进入业务 properties；Create/Import 由 appendTenantCompanyCreateImportProperties 固定追加） */
const ENTITY_BASE_FIELDS = new Set([
  'Id',
  'TenantCode',
  'CompanyCode',
  'CultureCode',
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
  'FlowInstanceId',
]);

/** CreateDto 排除的只读/统计类字段 */
const CREATE_EXCLUDE_NAME_PATTERNS = [
  /^LastLogin/i,
  /^LoginCount$/i,
  /^LoginFail/i,
  /^LockedUntil$/i,
  /^SessionDuration$/i,
  /^Level$/i,
  /^DeptPath$/i,
  /^IsLeaf$/i,
];

/** CreateDto / UpdateDto 排除：排序由服务端自动生成或走 TaktXxxSortDto 专用接口 */
const CREATE_EXCLUDE_PROPERTY_NAMES = new Set(['SortOrder']);

/**
 * 子表冗余码/名：存在对应 *Id 外键时由服务 Stamp 从主表回填，CreateDto 不加 [Required]
 * 全库主子表通用（不限人事）：XxxCode/XxxName ← XxxId；OriginalYyyCode ← OriginalYyyId
 * @param {string} propName
 * @param {Set<string>} propNames CreateDto 业务字段名集合
 * @returns {boolean}
 */
function isMasterStampFilledRedundantString(propName, propNames) {
  if (!propName || !propNames || propNames.size === 0) {
    return false;
  }
  if (propName.endsWith('Code')) {
    const idName = `${propName.slice(0, -4)}Id`;
    return propNames.has(idName);
  }
  if (propName.endsWith('Name')) {
    const idName = `${propName.slice(0, -4)}Id`;
    return propNames.has(idName);
  }
  return false;
}

/**
 * 聚合实体 DTO 类名（统一为 Takt{Entity}{Suffix}Dto，禁止 TaktCreate{Entity}Dto / TaktUpdate{Entity}Dto）
 * @param {string} entityShort 不含 Takt 前缀，如 Menu、Holiday
 * @returns {{ base: string, query: string, create: string, update: string, status: string, sort: string, template: string, import: string, export: string, tree: string }}
 */
function buildAggregateDtoClassNames(entityShort) {
  const prefix = `Takt${entityShort}`;
  return {
    base: `${prefix}Dto`,
    query: `${prefix}QueryDto`,
    create: `${prefix}CreateDto`,
    update: `${prefix}UpdateDto`,
    status: `${prefix}StatusDto`,
    sort: `${prefix}SortDto`,
    template: `${prefix}TemplateDto`,
    import: `${prefix}ImportDto`,
    export: `${prefix}ExportDto`,
    tree: `${prefix}TreeDto`,
  };
}

// ========================================
// 工具
// ========================================

/**
 * 规范化 XML 文档块
 * @param {string} block
 */
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

/**
 * 提取 summary
 * @param {string} xmlComment
 */
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
 * 清理注释文本
 * @param {string} text
 */
function normalizeDocText(text) {
  return sanitizeXmlDocPlainText(
    (text || '')
      .replace(/\/\/\/?/g, '')
      .split('\n')
      .map((l) => l.trim())
      .filter(Boolean)
      .join(' ')
      .trim(),
  );
}

/**
 * 实体名 → 主键 DTO 属性名（TaktUser → UserId）
 * @param {string} entityName
 */
function entityToIdPropertyName(entityName) {
  const shortName = entityName.replace(/^Takt/, '');
  return `${shortName}Id`;
}

/**
 * 是否仅生成极简关联 DTO（relationOnly）
 * RBAC 八表走 relationOnly 极简 DTO；业务主子表子实体一律走完整聚合 DTO（含 CRUD/Query/Import/Export）
 * @param {object} entity
 */
function isRelationEntity(entity) {
  const entityShort = entity.className.replace(/^Takt/, '');
  return isRbacJunctionEntity(entityShort);
}

/**
 * 实体短名是否以 Log 结尾（日志类实体，如 LoginLog、OperLog）
 * @param {object} entity
 */
function isLogSuffixEntity(entity) {
  const entityShort = entity.className.replace(/^Takt/, '');
  return entityShort.endsWith('Log');
}

/**
 * 聚合实体是否生成导出 DTO（关联表除外）
 * @param {object} entity
 */
function shouldGenerateImportExport(entity) {
  return !isRelationEntity(entity);
}

/**
 * 是否生成导入模板 / 导入 DTO（Log 后缀实体仅导出，不支持 Excel 导入）
 * @param {object} entity
 */
function shouldGenerateTemplateImport(entity) {
  return shouldGenerateImportExport(entity) && !isLogSuffixEntity(entity);
}

/**
 * 查找状态字段（*Status，如 DeptStatus、UserStatus）
 * @param {object} entity
 */
function findStatusProperty(entity) {
  return findEntityStatusProperty(entity.properties);
}

/**
 * 查找排序字段（SortOrder）
 * @param {object} entity
 */
function findSortOrderProperty(entity) {
  return entity.properties.find((p) => p.name === 'SortOrder');
}

/**
 * 导入/模板用业务字段（与 CreateDto 业务标量字段一致，不含基类隔离字段）
 * @param {object[]} createProps
 * @returns {object[]}
 */
function getTemplateImportProps(createProps) {
  return createProps;
}

/**
 * 向类体写入属性列表
 * @param {string[]} lines
 * @param {object[]} props
 * @param {object} emitOptions
 */
function appendEmittedProperties(lines, props, emitOptions = {}) {
  props.forEach((prop) => {
    emitProperty(prop, emitOptions).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('');
  });
}

/**
 * 写入实体主键属性（统一 [AdaptMember("Id")] + 长整型 JSON 序列化）
 * @param {string[]} lines
 * @param {string} idProp 如 DeptId、UserId
 * @param {object} [options]
 */
function appendEntityIdProperty(lines, idProp, options = {}) {
  const { required = false, summary = '主键ID' } = options;
  lines.push('    /// <summary>');
  lines.push(`    /// ${summary}`);
  lines.push('    /// </summary>');
  if (required) {
    lines.push('    [Required(ErrorMessage = "ID不能为空")]');
  }
  lines.push('    [AdaptMember("Id")]');
  lines.push('    [JsonConverter(typeof(ValueToStringConverter))]');
  lines.push(`    public long ${idProp} { get; set; }`);
  lines.push('');
}

/**
 * CreateDto / TemplateDto / ImportDto 租户与公司隔离字段（业务字段之前）
 * TenantCode / CompanyCode / CultureCode / PlantCode / RelatedPlant 由登录上下文或公司切换注入，不加 [Required]
 * @param {string[]} lines
 * @param {string} entityBase
 * @param {{ forImport?: boolean, entityShort?: string }} [options]
 *   forImport=true 时字段可空（Excel 导入）
 *   公司/审批：CompanyCode + CultureCode + PlantCode
 *   租户组合 1：RelatedPlant + CultureCode（TaktPlant 除外 RelatedPlant）
 *   组合 2 Culture：CultureCode（Query/Create/Template/Import 必须）
 *   组合 3 Plant：RelatedPlant（同上必须）
 *   组合 4 Core：仅 TenantCode
 */
function appendTenantCompanyCreateImportProperties(lines, entityBase, options = {}) {
  const { forImport = false, entityShort = '' } = options;
  const stringType = forImport ? 'string?' : 'string';
  lines.push('    /// <summary>');
  lines.push('    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）');
  lines.push('    /// </summary>');
  lines.push(`    public ${stringType} TenantCode { get; set; } = string.Empty;`);
  lines.push('');
  if (isCompanyOrApprovalEntityBase(entityBase)) {
    lines.push('    /// <summary>');
    lines.push('    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）');
    lines.push('    /// </summary>');
    lines.push(`    public ${stringType} CompanyCode { get; set; } = string.Empty;`);
    lines.push('');
    lines.push('    /// <summary>');
    lines.push('    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
    lines.push('    /// </summary>');
    lines.push(`    public ${stringType} CultureCode { get; set; } = string.Empty;`);
    lines.push('');
    lines.push('    /// <summary>');
    lines.push('    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode；空则仓储按公司 RelatedPlant 注入）');
    lines.push('    /// </summary>');
    lines.push(`    public ${stringType} PlantCode { get; set; } = string.Empty;`);
    lines.push('');
    return;
  }
  if (!isTenantEntityBase(entityBase)) {
    return;
  }
  // 组合 1 / 3：有工厂 → RelatedPlant（工厂主档 TaktPlant 自身除外）
  if (entityBaseHasRelatedPlant(entityBase) && entityShort !== 'Plant') {
    lines.push('    /// <summary>');
    lines.push('    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）');
    lines.push('    /// </summary>');
    lines.push(`    public ${stringType} RelatedPlant { get; set; } = string.Empty;`);
    lines.push('');
  }
  // 组合 1 / 2：有语言 → CultureCode（Query/Create/Template/Import 必须）
  if (entityBaseHasCultureCode(entityBase)) {
    lines.push('    /// <summary>');
    lines.push('    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
    lines.push('    /// </summary>');
    lines.push(`    public ${stringType} CultureCode { get; set; } = string.Empty;`);
    lines.push('');
  }
}

/**
 * 扩展字段与备注（与 CreateDto 一致，用于 Template / Import）
 * @param {string[]} lines
 */
function appendExtFieldAndRemark(lines) {
  lines.push('    /// <summary>');
  lines.push('    /// 扩展字段JSON');
  lines.push('    /// </summary>');
  lines.push('    public string? ExtField { get; set; }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 备注');
  lines.push('    /// </summary>');
  lines.push('    public string? Remark { get; set; }');
  lines.push('');
}

/**
 * 是否树形实体（含 ParentId）
 * @param {object} entity
 */
function isTreeEntity(entity) {
  return entity.properties.some((p) => p.name === 'ParentId');
}

/**
 * 实体是否使用 Takt.Shared.Enums 类型
 * @param {object} entity
 */
function entityUsesSharedEnums(entity) {
  return entityUsesSharedEnumsFromProperties(entity.properties);
}

/**
 * 是否为日期/时间类型属性（QueryDto 生成范围查询）
 * @param {object} prop
 */
function isDateTimeProperty(prop) {
  return prop.bareType === 'DateTime' || prop.bareType === 'DateOnly';
}

/**
 * 向 QueryDto 写入单个日期字段的起止范围查询属性
 * @param {string[]} lines
 * @param {object} prop
 */
function appendDateRangeQueryProperties(lines, prop) {
  const label = prop.summary || prop.name;
  lines.push('    /// <summary>');
  lines.push(`    /// ${label}（范围查询-开始）`);
  lines.push('    /// </summary>');
  lines.push(`    public DateTime? ${prop.name}Start { get; set; }`);
  lines.push('');
  lines.push('    /// <summary>');
  lines.push(`    /// ${label}（范围查询-结束）`);
  lines.push('    /// </summary>');
  lines.push(`    public DateTime? ${prop.name}End { get; set; }`);
  lines.push('');
}

/**
 * 写入创建时间范围查询（对应基类 CreatedAt；QueryDto 中始终生成）
 * @param {string[]} lines
 */
function appendCreatedAtRangeQueryProperties(lines) {
  lines.push('    /// <summary>');
  lines.push('    /// 创建时间（范围查询-开始）');
  lines.push('    /// </summary>');
  lines.push('    public DateTime? CreatedAtStart { get; set; }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 创建时间（范围查询-结束）');
  lines.push('    /// </summary>');
  lines.push('    public DateTime? CreatedAtEnd { get; set; }');
  lines.push('');
}

/**
 * QueryDto：可空字符串查询字段
 * @param {string[]} lines
 * @param {string} name
 * @param {string} summary
 */
function appendQueryStringProperty(lines, name, summary) {
  lines.push('    /// <summary>');
  lines.push(`    /// ${summary}`);
  lines.push('    /// </summary>');
  lines.push(`    public string? ${name} { get; set; } = string.Empty;`);
  lines.push('');
}

/**
 * QueryDto：可空整型精确查询
 * @param {string[]} lines
 * @param {string} name
 * @param {string} summary
 */
function appendQueryIntProperty(lines, name, summary) {
  lines.push('    /// <summary>');
  lines.push(`    /// ${summary}`);
  lines.push('    /// </summary>');
  lines.push(`    public int? ${name} { get; set; }`);
  lines.push('');
}

/**
 * QueryDto：可空枚举精确查询
 * @param {string[]} lines
 * @param {string} enumType
 * @param {string} name
 * @param {string} summary
 */
function appendQueryEnumProperty(lines, enumType, name, summary) {
  lines.push('    /// <summary>');
  lines.push(`    /// ${summary}`);
  lines.push('    /// </summary>');
  lines.push(`    public ${enumType}? ${name} { get; set; }`);
  lines.push('');
}

/**
 * QueryDto：可空 long 精确查询（含 JsonConverter）
 * @param {string[]} lines
 * @param {string} name
 * @param {string} summary
 */
function appendQueryLongProperty(lines, name, summary) {
  lines.push('    /// <summary>');
  lines.push(`    /// ${summary}`);
  lines.push('    /// </summary>');
  lines.push('    [JsonConverter(typeof(ValueToStringConverter))]');
  lines.push(`    public long? ${name} { get; set; }`);
  lines.push('');
}

/**
 * QueryDto 第 1～n 位：租户 / 公司 / 区域文化 / 工厂隔离字段（业务字段之前）
 * @param {string[]} lines
 * @param {string} entityBase
 * @param {string} [entityShort]
 */
function appendTenantCompanyQueryProperties(lines, entityBase, entityShort = '') {
  appendQueryStringProperty(lines, 'TenantCode', '租户编码');

  if (isCompanyOrApprovalEntityBase(entityBase)) {
    appendQueryStringProperty(lines, 'CompanyCode', '公司（选项 TaktCompanies/options；DictValue=CompanyCode）');
    appendQueryStringProperty(lines, 'CultureCode', '区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
    appendQueryStringProperty(
      lines,
      'PlantCode',
      '工厂代码（选项 TaktPlants/options；DictValue=PlantCode）',
    );
    return;
  }
  if (!isTenantEntityBase(entityBase)) {
    return;
  }
  if (entityBaseHasRelatedPlant(entityBase) && entityShort !== 'Plant') {
    appendQueryStringProperty(
      lines,
      'RelatedPlant',
      '关联工厂（选项 TaktPlants/options；DictValue=PlantCode）',
    );
  }
  if (entityBaseHasCultureCode(entityBase)) {
    appendQueryStringProperty(lines, 'CultureCode', '区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
  }
}

/**
 * QueryDto：审批基类字段（业务字段之后、CreatedAtStart 之前）
 * @param {string[]} lines
 * @param {string} entityBase
 */
function appendApprovalQueryProperties(lines, entityBase) {
  if (!entityBase || !entityBase.includes('Approval')) {
    return;
  }
  appendQueryEnumProperty(lines, 'TaktApprovalStatus', 'ApprovalStatus', '审批状态（字典 sys_approval_status；与 TaktApprovalEntityBase.ApprovalStatus 一致）');
  appendQueryLongProperty(lines, 'InitiatorId', '发起人ID');
  appendDateRangeQueryProperties(lines, { name: 'InitiatedAt', summary: '发起时间' });
  appendQueryLongProperty(lines, 'ApprovedBy', '最终审批人ID');
  appendDateRangeQueryProperties(lines, { name: 'ApprovedAt', summary: '最终审批时间' });
  appendQueryLongProperty(lines, 'FlowInstanceId', '流程实例 ID');
}

// ========================================
// 解析实体
// ========================================

/** 实体类体内「导航属性区域」标记（该标记之后为 SqlSugar 导航属性，不映射表列） */
const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
 * 将类体拆分为标量属性区与导航属性区
 * @param {string} classBody
 * @returns {{ scalarBody: string, navigationBody: string }}
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
 * 解析类体内的 public 属性
 * @param {string} classBody
 * @param {{ allowListTypes?: boolean }} [options] allowListTypes=true 时保留 List&lt;Takt*&gt;（用于导航区）
 */
function parseScalarProperties(classBody, options = {}) {
  const { allowListTypes = false } = options;
  const properties = [];
  /** 必须从 public 后捕获类型，避免误匹配 [SugarColumn(...)] 中的 ] */
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>[\s\S]*?public\s+((?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?)\s+(\w+)\s*\{\s*get;\s*set;/g;
  let match;

  while ((match = propertyRegex.exec(classBody)) !== null) {
    const summary = normalizeDocText(match[1]);
    const csharpType = match[2].trim();
    const name = match[3];

    if (!csharpType || csharpType.includes('Navigate')) {
      continue;
    }
    if (!allowListTypes && csharpType.startsWith('List<')) {
      continue;
    }

    if (!allowListTypes && /\[Navigate\s*\(/.test(match[0])) {
      continue;
    }
    if (summary.includes('SugarColumn') || summary.includes('public ')) {
      continue;
    }

    properties.push({
      name,
      csharpType,
      summary,
      isNullable: csharpType.endsWith('?'),
      bareType: csharpType.replace('?', ''),
    });
  }

  return properties;
}

/**
 * 从属性前文解析 NavigateType
 * @param {string} segment
 */
function parseNavigateTypeFromSegment(segment) {
  const matches = [...segment.matchAll(/NavigateType\.(OneToMany|ManyToOne)/g)];
  if (!matches.length) {
    return null;
  }
  return matches[matches.length - 1][1];
}

/**
 * 从 [Navigate(..., nameof(TaktChild.FkField))] 解析子表外键字段名
 * @param {string} segment
 */
function parseNavigateForeignKeyFromSegment(segment) {
  const matches = [...segment.matchAll(/nameof\((?:\w+\.)?(\w+)\)/g)];
  if (!matches.length) {
    return null;
  }
  return matches[matches.length - 1][1];
}

/**
 * 从导航属性 C# 类型解析关联实体类名
 * @param {string} csharpType
 * @param {string} navigateType
 */
function resolveRelatedEntityType(csharpType, navigateType) {
  if (navigateType === 'OneToMany' || csharpType.startsWith('List<')) {
    const listMatch = csharpType.match(/List<(Takt\w+)>/);
    return listMatch ? listMatch[1] : null;
  }
  const bare = csharpType.replace('?', '');
  return /^Takt\w+$/.test(bare) ? bare : null;
}

/**
 * 解析导航属性（[Navigate] 一对多 / 多对一）
 * @param {string} navigationBody
 */
function parseNavigationProperties(navigationBody) {
  if (!navigationBody.trim()) {
    return [];
  }

  const rawProps = parseScalarProperties(navigationBody, { allowListTypes: true });
  const navigations = [];

  rawProps.forEach((prop) => {
    const nameIdx = navigationBody.search(new RegExp(`\\b${prop.name}\\s*\\{`));
    const segment = nameIdx >= 0 ? navigationBody.slice(Math.max(0, nameIdx - 500), nameIdx) : '';
    const navigateType =
      parseNavigateTypeFromSegment(segment) ||
      (prop.csharpType.startsWith('List<') ? 'OneToMany' : 'ManyToOne');
    const relatedEntityType = resolveRelatedEntityType(prop.csharpType, navigateType);
    if (!relatedEntityType) {
      return;
    }
    const foreignKeyOnChild = parseNavigateForeignKeyFromSegment(segment);
    navigations.push({
      name: prop.name,
      summary: prop.summary,
      navigateType,
      isCollection: navigateType === 'OneToMany',
      relatedEntityType,
      relatedEntityShort: relatedEntityType.replace(/^Takt/, ''),
      foreignKeyOnChild,
    });
  });

  return navigations;
}

/**
 * 取 [Navigate] 前紧邻的 XML summary 文本（窗口内最后一个 summary 块）
 * @param {string} classBody
 * @param {number} navigateIndex
 */
function extractImmediateSummaryBeforeNavigate(classBody, navigateIndex) {
  const before = classBody.slice(Math.max(0, navigateIndex - 800), navigateIndex);
  const blocks = [...before.matchAll(/\/\/\/\s*<summary>([\s\S]*?)<\/summary>/g)];
  if (!blocks.length) {
    return '';
  }
  return normalizeDocText(blocks[blocks.length - 1][1]);
}

/**
 * 从类体中移除 [Navigate] 导航属性块（含紧邻其前的 summary 注释），供标量属性解析
 * @param {string} classBody
 */
function stripNavigatePropertyBlocks(classBody) {
  const navAnchorRegex =
    /\[Navigate\(\s*NavigateType\.(?:OneToMany|ManyToOne)[\s\S]*?public\s+(?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?\s+\w+\s*\{\s*get;\s*set;/g;
  const matches = [...classBody.matchAll(navAnchorRegex)];
  if (!matches.length) {
    return classBody;
  }
  let result = classBody;
  for (let i = matches.length - 1; i >= 0; i -= 1) {
    const m = matches[i];
    const navEnd = m.index + m[0].length;
    const beforeNav = result.slice(0, m.index);
    const tail = beforeNav.slice(-800);
    const blocks = [...tail.matchAll(/\s*\/\/\/\s*<summary>[\s\S]*?<\/summary>\s*/g)];
    const lastBlock = blocks.length ? blocks[blocks.length - 1][0] : '';
    const removeStart = lastBlock ? beforeNav.length - lastBlock.length : m.index;
    result = result.slice(0, removeStart) + result.slice(navEnd);
  }
  return result;
}

/**
 * 无「导航属性区域」时，从整段类体提取 [Navigate] 属性（兼容旧实体）
 * summary 仅取 [Navigate] 前紧邻的 XML 注释，避免跨属性误匹配
 * @param {string} classBody
 */
function parseNavigationPropertiesFallback(classBody) {
  const navigations = [];
  const navAnchorRegex =
    /\[Navigate\(\s*NavigateType\.(OneToMany|ManyToOne)\s*,\s*nameof\((?:\w+\.)?(\w+)\)(?:[^)]*)?\)\]([\s\S]*?)public\s+((?:List<)?(?:Takt\w+|[a-zA-Z][\w]*)(?:>)?(?:\?)?)\s+(\w+)\s*\{\s*get;\s*set;/g;
  let match;
  while ((match = navAnchorRegex.exec(classBody)) !== null) {
    const navigateType = match[1];
    const foreignKeyOnChild = match[2];
    const summary = extractImmediateSummaryBeforeNavigate(classBody, match.index);
    const csharpType = match[4].trim();
    const name = match[5];
    const relatedEntityType = resolveRelatedEntityType(csharpType, navigateType);
    if (!relatedEntityType) {
      continue;
    }
    if (summary.includes('SugarColumn') || summary.includes('public ')) {
      continue;
    }
    navigations.push({
      name,
      summary,
      navigateType,
      isCollection: navigateType === 'OneToMany',
      relatedEntityType,
      relatedEntityShort: relatedEntityType.replace(/^Takt/, ''),
      foreignKeyOnChild,
    });
  }
  return navigations;
}

/**
 * 子表明细实体（含 LineNumber 且 ManyToOne 挂主表）
 * @param {object} entity
 * @returns {boolean}
 */
function isDetailChildEntity(entity) {
  const hasLineNumber = entity.properties.some((p) => p.name === 'LineNumber');
  const hasManyToOne = (entity.navigationProperties || []).some(
    (nav) => nav.navigateType === 'ManyToOne' || (!nav.isCollection && nav.navigateType !== 'OneToMany'),
  );
  return hasLineNumber && hasManyToOne;
}

/**
 * 主表响应 DTO：子表最大行号（仅无 IsObsolete 子表；含已软删行，供前端递增分配）
 * @param {string[]} lines
 * @param {object[]} navigationProperties
 * @param {Map<string, object>} entityRegistry
 */
/**
 * 主表级联用的 OneToMany（排除 RBAC 关联表与独立菜单从实体）
 * @param {object[]} navigationProperties
 * @returns {object[]}
 */
function filterCascadingOneToManyNavigations(navigationProperties) {
  return (navigationProperties || []).filter((nav) => {
    if (!(nav.navigateType === 'OneToMany' || nav.isCollection)) {
      return false;
    }
    if (isRbacJunctionEntity(nav.relatedEntityShort)) {
      return false;
    }
    if (isStandaloneChildVueEntity(nav.relatedEntityShort)) {
      return false;
    }
    return true;
  });
}

function appendMasterDetailMaxLineNumberProperties(lines, navigationProperties, entityRegistry) {
  const oneToMany = filterCascadingOneToManyNavigations(navigationProperties);
  oneToMany.forEach((nav) => {
    const child = entityRegistry.get(nav.relatedEntityType);
    if (!child || !child.properties.some((p) => p.name === 'LineNumber')) {
      return;
    }
    const childHasIsObsolete = child.properties.some((p) => p.name === 'IsObsolete');
    if (childHasIsObsolete) {
      return;
    }
    lines.push('    /// <summary>');
    lines.push(`    /// ${nav.relatedEntityShort}最大行号（含已软删；前端分配新行号须在此基础上递增，禁止复用已占用行号）`);
    lines.push('    /// </summary>');
    lines.push(`    public int Max${nav.relatedEntityShort}LineNumber { get; set; }`);
    lines.push('');
  });
}

/**
 * Create/Update DTO 写入一对多子表集合（级联保存）
 * @param {string[]} lines
 * @param {object[]} navigationProperties
 * @param {string} entityShort
 * @param {{ cascadeChildUseUpdateDto?: boolean, useNewKeyword?: boolean }} [options] 主表 Create 用 CreateDto；Update 用 UpdateDto（new 隐藏基类集合）
 */
function appendMasterDetailCreateProperties(lines, navigationProperties, entityShort, options = {}) {
  const useChildUpdateDto = options.cascadeChildUseUpdateDto === true;
  const useNewKeyword = options.useNewKeyword === true;
  const oneToMany = (navigationProperties || []).filter(
    (nav) => nav.navigateType === 'OneToMany' || nav.isCollection,
  );
  if (!oneToMany.length) {
    return;
  }
  oneToMany.forEach((nav) => {
    if (isRbacJunctionEntity(nav.relatedEntityShort)) {
      // Update 继承 Create 的 *Ids，勿重复声明（否则 CS0108）
      if (useChildUpdateDto || useNewKeyword) {
        return;
      }
      const field = resolveRbacCreateFieldFromNav(entityShort, nav);
      if (!field) {
        return;
      }
      lines.push('    /// <summary>');
      lines.push(`    /// ${field.summary}`);
      lines.push('    /// </summary>');
      lines.push(`    public ${field.type} ${field.prop} { get; set; }`);
      lines.push('');
      return;
    }
    // 独立菜单明细：不写入主表 Create/Update 级联集合
    if (isStandaloneChildVueEntity(nav.relatedEntityShort)) {
      return;
    }
    const childDtoType = useChildUpdateDto
      ? `Takt${nav.relatedEntityShort}UpdateDto`
      : `Takt${nav.relatedEntityShort}CreateDto`;
    lines.push('    /// <summary>');
    lines.push(`    /// ${nav.summary || nav.name}（子表，级联保存）`);
    lines.push('    /// </summary>');
    const newPrefix = useNewKeyword ? 'new ' : '';
    lines.push(`    public ${newPrefix}List<${childDtoType}>? ${nav.name} { get; set; }`);
    lines.push('');
  });
}

/**
 * 收集导航关联 DTO 所需的跨命名空间 using
 * @param {object} entity
 * @param {object[]} navigationProperties
 * @param {Map<string, object>} entityRegistry
 */
function collectNavigationDtoUsings(entity, navigationProperties, entityRegistry) {
  const usings = new Set();
  navigationProperties.forEach((nav) => {
    if ((nav.navigateType === 'OneToMany' || nav.isCollection) && isStandaloneChildVueEntity(nav.relatedEntityShort)) {
      return;
    }
    const related = entityRegistry.get(nav.relatedEntityType);
    if (related && related.dtoNamespace !== entity.dtoNamespace) {
      usings.add(related.dtoNamespace);
      return;
    }
    // RBAC 关联实体不进 registry，按八表命名空间映射到 Application.Dtos
    if (isRbacJunctionEntity(nav.relatedEntityShort)) {
      const domainNs = RBAC_JUNCTION_NAMESPACE[nav.relatedEntityShort];
      if (!domainNs) {
        return;
      }
      const dtoNs = domainNs.replace(/^Takt\.Domain\.Entities/, 'Takt.Application.Dtos');
      if (dtoNs !== entity.dtoNamespace) {
        usings.add(dtoNs);
      }
    }
  });
  return [...usings].sort();
}

/**
 * 在响应 DTO 中写入导航属性（主子表：主表 List&lt;子Dto&gt;，子表 子Dto? 主表；RBAC 关联表亦写入 List 供 GetById 填充）
 * @param {string[]} lines
 * @param {object[]} navigationProperties
 */
function appendNavigationDtoProperties(lines, navigationProperties) {
  if (!navigationProperties.length) {
    return;
  }
  navigationProperties.forEach((nav) => {
    // 独立菜单明细：响应 DTO 不嵌套全量子表（走子表独立 list）
    if ((nav.navigateType === 'OneToMany' || nav.isCollection) && isStandaloneChildVueEntity(nav.relatedEntityShort)) {
      return;
    }
    const dtoTypeName = `Takt${nav.relatedEntityShort}Dto`;
    const csharpType = nav.isCollection ? `List<${dtoTypeName}>?` : `${dtoTypeName}?`;
    const roleLabel = isRbacJunctionEntity(nav.relatedEntityShort)
      ? 'RBAC'
      : (nav.isCollection ? '子表' : '主表');

    lines.push('    /// <summary>');
    lines.push(`    /// ${nav.summary || nav.name}`);
    lines.push(`    /// （${roleLabel}：${nav.relatedEntityType}）`);
    lines.push('    /// </summary>');
    lines.push(`    public ${csharpType} ${nav.name} { get; set; }`);
    lines.push('');
  });
}

/**
 * 按大括号深度提取类体（避免多行类被错误截断）
 * @param {string} content
 * @param {number} openBraceIndex
 */
function extractClassBody(content, openBraceIndex) {
  let depth = 1;
  let i = openBraceIndex + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
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

/**
 * 解析实体 .cs 文件
 * @param {string} filePath
 */
function parseEntityFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const classHeader = parseEntityClassHeaderFromCsContent(content);
  if (!classHeader) {
    return null;
  }

  const className = classHeader.className;
  const entityBase = classHeader.entityBase;
  const classHeaderMatch = content.match(ENTITY_CLASS_HEADER_REGEX);
  const openBraceIndex = classHeaderMatch.index + classHeaderMatch[0].length - 1;
  const classBody = extractClassBody(content, openBraceIndex);
  const beforeClass = content.slice(0, classHeaderMatch.index);
  const docBlocks = [...beforeClass.matchAll(/((?:\s*\/\/\/[^\n]*\n)+)/g)];
  const classDocBlock = docBlocks.length ? docBlocks[docBlocks.length - 1][1] : '';
  const classDoc = extractSummary(csharpDocToXml(classDocBlock));

  const namespaceMatch = content.match(/namespace\s+([\w.]+);/);
  const entityNamespace = namespaceMatch ? namespaceMatch[1] : '';
  const dtoDirParts = entityNamespaceToDirParts(entityNamespace);
  const dtoNamespaceSuffix = dtoDirParts.length ? `.${dtoDirParts.join('.')}` : '';
  const dtoNamespace = `Takt.Application.Dtos${dtoNamespaceSuffix}`;

  const { scalarBody, navigationBody } = splitClassBodyByNavigationRegion(classBody);
  let navigationProperties = parseNavigationProperties(navigationBody);
  if (navigationProperties.length === 0) {
    navigationProperties = parseNavigationPropertiesFallback(classBody);
  }

  const navNames = new Set(navigationProperties.map((n) => n.name));
  const scalarParseBody = navigationBody.trim()
    ? scalarBody
    : stripNavigatePropertyBlocks(classBody);
  const allScalar = parseScalarProperties(scalarParseBody);
  const properties = allScalar
    .filter((p) => !ENTITY_BASE_FIELDS.has(p.name))
    .filter((p) => !navNames.has(p.name));

  return {
    className,
    classDoc,
    entityBase,
    dtoBase: resolveDtoBaseFromEntityBase(entityBase),
    entityNamespace,
    dtoNamespace,
    dtoDirParts,
    properties,
    navigationProperties,
    filePath,
  };
}

// ========================================
// 生成 C# 代码
// ========================================

/**
 * long 字段 JsonConverter 特性
 * @param {string} csharpType
 */
function longJsonAttribute(csharpType) {
  const bare = csharpType.replace('?', '');
  if (bare === 'long') {
    return '    [JsonConverter(typeof(ValueToStringConverter))]';
  }
  return '';
}

/**
 * 生成属性块
 * @param {object} prop
 * @param {object} [options]
 */
function emitProperty(prop, options = {}) {
  const lines = [];
  const { forceNullable = false, required = false, indent = '    ' } = options;
  const bare = prop.bareType || prop.csharpType.replace('?', '');
  const nullableSuffix = forceNullable || prop.isNullable ? '?' : '';
  const typeStr = `${bare}${nullableSuffix}`;

  if (prop.summary) {
    lines.push(`${indent}/// <summary>`);
    lines.push(`${indent}/// ${prop.summary}`);
    lines.push(`${indent}/// </summary>`);
  }

  if (required) {
    lines.push(`${indent}[Required(ErrorMessage = "${prop.summary || prop.name}不能为空")]`);
  }

  const jsonAttr = longJsonAttribute(prop.csharpType);
  if (jsonAttr) {
    lines.push(`${indent}${jsonAttr.trim()}`);
  }

  const defaultValue = prop.csharpType.includes('string')
    ? ' = string.Empty;'
    : prop.csharpType.includes('int') && !nullableSuffix && !isSharedEnumType(bare)
      ? ' = 0;'
      : ';';
  lines.push(`${indent}public ${typeStr} ${prop.name} { get; set; }${defaultValue === ';' ? '' : defaultValue}`);
  if (defaultValue === ';') {
    lines[lines.length - 1] = `${indent}public ${typeStr} ${prop.name} { get; set; }`;
  }

  return lines;
}

/**
 * 文件头
 * @param {object} entity
 * @param {string} description
 */
function buildFileHeader(entity, description) {
  const today = new Date().toISOString().split('T')[0];
  return [
    '// ========================================',
    '// 项目名称：节拍智控·TaktWise',
    `// 命名空间：${entity.dtoNamespace}`,
    `// 文件名称：Takt${entity.className.replace(/^Takt/, '')}Dtos.cs`,
    `// 创建时间：${today}`,
    '// 创建人：Takt365(Auto Generated)',
    `// 功能描述：${description}（由 generate-dtos-from-entity.cjs 根据 ${entity.className} 生成，请按需审阅）`,
    '// ',
    '// 版权信息：Copyright (c) 2025 Takt  All rights reserved.',
    '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。',
    '// ========================================',
    '',
  ];
}

/**
 * 生成聚合根实体完整 Dtos 文件（关联表复用本函数并设 relationOnly: true）
 * @param {object} entity
 * @param {Map<string, object>} [entityRegistry]
 * @param {{ relationOnly?: boolean }} [options]
 */
function generateAggregateDtoFileContent(entity, entityRegistry, options = {}) {
  const { relationOnly = false } = options;
  const lines = [];
  const entityShort = entity.className.replace(/^Takt/, '');
  const idProp = entityToIdPropertyName(entity.className);
  const statusProp = findStatusProperty(entity);
  const sortOrderProp = findSortOrderProperty(entity);
  const treeEntity = !relationOnly && isTreeEntity(entity);
  const navigationProperties = relationOnly ? [] : entity.navigationProperties || [];

  const headerDesc = relationOnly
    ? `${entity.className} 关联 DTO`
    : `${entityShort} 模块 DTO`;
  lines.push(...buildFileHeader(entity, headerDesc));
  lines.push('using System.ComponentModel.DataAnnotations;');
  lines.push('using Mapster;');
  lines.push('using Takt.Shared.Helpers;');
  lines.push('using Takt.Shared.Models;');
  if (entityUsesSharedEnums(entity)) {
    lines.push('using Takt.Shared.Enums;');
  }
  if (!relationOnly && entityRegistry && navigationProperties.length) {
    collectNavigationDtoUsings(entity, navigationProperties, entityRegistry).forEach((ns) => {
      lines.push(`using ${ns};`);
    });
  }
  lines.push('');
  lines.push(`namespace ${entity.dtoNamespace};`);
  lines.push('');

  // Response DTO
  lines.push('// ========================================');
  lines.push(relationOnly ? '// 关联 DTO' : `// ${entityShort} 响应 DTO`);
  lines.push('// ========================================');
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// ${entity.classDoc || (relationOnly ? `${entityShort} 关联 DTO` : `${entityShort} 响应 DTO`)}`);
  if (!relationOnly) {
    lines.push(`/// 对应前端 Takt${entityShort}Dto`);
  } else {
    lines.push(`/// 对应实体 ${entity.className}`);
  }
  lines.push(`/// 继承 ${entity.dtoBase}`);
  lines.push('/// </summary>');
  lines.push(`public class Takt${entityShort}Dto : ${entity.dtoBase}`);
  lines.push('{');
  appendEntityIdProperty(lines, idProp, {
    summary: relationOnly
      ? `${entityShort}ID（适配实体 Id）`
      : `${entityShort}ID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）`,
  });
  const propertyNames = new Set(entity.properties.map((p) => p.name));
  entity.properties.forEach((prop) => {
    emitProperty(prop).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('');
    if (/Id$/.test(prop.name) && prop.name !== 'Id' && prop.name !== 'ParentId') {
      const fillName = prop.name.replace(/Id$/, 'Name');
      if (!propertyNames.has(fillName)) {
        lines.push('    /// <summary>');
        lines.push(`    /// ${prop.summary ? prop.summary.replace(/ID.*/, '名称（填充字段）') : `${fillName}（填充字段）`}`);
        lines.push('    /// </summary>');
        lines.push(`    public string? ${fillName} { get; set; }`);
        lines.push('');
      }
    }
  });
  if (!relationOnly && entityRegistry && navigationProperties.length) {
    appendMasterDetailMaxLineNumberProperties(lines, navigationProperties, entityRegistry);
  }
  appendNavigationDtoProperties(lines, navigationProperties);
  lines.push('}');
  lines.push('');

  if (treeEntity) {
    lines.push('// ========================================');
    lines.push(`// ${entityShort} 树形响应 DTO`);
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 树形列表/树选择 DTO（含子节点）`);
    lines.push(`/// 对应 Get${entityShort}TreeAsync 等接口`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}TreeDto : Takt${entityShort}Dto`);
    lines.push('{');
    lines.push('    /// <summary>');
    lines.push('    /// 子节点（懒加载树接口返回 null，表示尚未加载；勿用空 List 冒充已加载）');
    lines.push('    /// </summary>');
    lines.push(`    public List<Takt${entityShort}TreeDto>? Children { get; set; }`);
    lines.push('}');
    lines.push('');
  }

  // Query DTO
  lines.push('// ========================================');
  lines.push(`// ${entityShort} 查询 DTO`);
  lines.push('// ========================================');
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// ${entityShort} 分页查询 DTO`);
  lines.push('/// 继承 TaktPagedQuery');
  lines.push('/// </summary>');
  lines.push(`public class Takt${entityShort}QueryDto : TaktPagedQuery`);
  lines.push('{');
  appendTenantCompanyQueryProperties(lines, entity.entityBase, entityShort);
  entity.properties.forEach((prop) => {
    if (isDateTimeProperty(prop)) {
      appendDateRangeQueryProperties(lines, prop);
      return;
    }
    const queryProp = {
      ...prop,
      isNullable: true,
      csharpType: `${prop.bareType}?`,
    };
    emitProperty(queryProp, { forceNullable: true }).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('');
  });
  appendApprovalQueryProperties(lines, entity.entityBase);
  appendCreatedAtRangeQueryProperties(lines);
  lines.push('    /// <summary>');
  lines.push('    /// 扩展字段JSON');
  lines.push('    /// </summary>');
  lines.push('    public string? ExtField { get; set; }');
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 备注（模糊查询）');
  lines.push('    /// </summary>');
  lines.push('    public string? Remark { get; set; }');
  lines.push('}');
  lines.push('');

  // Create DTO
  const createProps = entity.properties.filter((p) => {
    if (CREATE_EXCLUDE_PROPERTY_NAMES.has(p.name)) {
      return false;
    }
    if (CREATE_EXCLUDE_NAME_PATTERNS.some((re) => re.test(p.name))) {
      return false;
    }
    return true;
  });

  lines.push('// ========================================');
  lines.push(`// 创建${entityShort} DTO`);
  lines.push('// ========================================');
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// 创建${entityShort} DTO`);
  lines.push('/// </summary>');
  const dtoNames = buildAggregateDtoClassNames(entityShort);
  lines.push(`public class ${dtoNames.create}`);
  lines.push('{');
  appendTenantCompanyCreateImportProperties(lines, entity.entityBase, { entityShort });
  const createPropNames = new Set(createProps.map((p) => p.name));
  createProps.forEach((prop) => {
    // 主表外键冗余码/名由服务 Stamp* 回填：勿加 [Required]，否则 [ApiController] 在 FluentValidation 前对空串直接 400
    const stampFilledRedundant = isMasterStampFilledRedundantString(prop.name, createPropNames);
    const required =
      prop.bareType === 'string'
      && !prop.isNullable
      && !prop.name.includes('Hash')
      && !stampFilledRedundant;
    emitProperty(prop, { required }).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('');
    if (isTaktNumberingAutoFormFieldSummary(prop.summary)) {
      lines.push('    /// <summary>');
      lines.push('    /// 编码规则编码（前端表单从 TaktNumberings/options 选择；对应 TaktNumbering.RuleCode；不落库）');
      lines.push('    /// </summary>');
      lines.push('    public string? NumberingRuleCode { get; set; }');
      lines.push('');
    }
  });
  appendMasterDetailCreateProperties(lines, navigationProperties, entityShort, {
    cascadeChildUseUpdateDto: false,
  });
  appendInverseRbacCreateFields(lines, entityShort);
  appendExtFieldAndRemark(lines);
  lines.push('}');
  lines.push('');

  // Update DTO
  lines.push('// ========================================');
  lines.push(`// 更新${entityShort} DTO`);
  lines.push('// ========================================');
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// 更新${entityShort} DTO`);
  lines.push(`/// 继承 ${dtoNames.create}，添加 ${idProp} 字段`);
  lines.push('/// </summary>');
  lines.push(`public class ${dtoNames.update} : ${dtoNames.create}`);
  lines.push('{');
  appendEntityIdProperty(lines, idProp, {
    required: true,
    summary: `${entityShort}ID（标识要更新的实体）`,
  });
  appendMasterDetailCreateProperties(lines, navigationProperties, entityShort, {
    cascadeChildUseUpdateDto: true,
    useNewKeyword: true,
  });
  lines.push('}');
  lines.push('');

  if (!relationOnly && statusProp) {
    lines.push('// ========================================');
    lines.push(`// ${entityShort} 状态 DTO`);
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 状态更新 DTO`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}StatusDto`);
    lines.push('{');
    appendEntityIdProperty(lines, idProp, {
      required: true,
      summary: `${entityShort}ID`,
    });
    emitProperty({ ...statusProp, isNullable: false }, { required: true }).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('}');
    lines.push('');
  }

  if (!relationOnly && sortOrderProp) {
    lines.push('// ========================================');
    lines.push(`// ${entityShort} 排序 DTO`);
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 排序更新 DTO`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}SortDto`);
    lines.push('{');
    appendEntityIdProperty(lines, idProp, {
      required: true,
      summary: `${entityShort}ID`,
    });
    emitProperty({ ...sortOrderProp, isNullable: false }, { required: true }).forEach((l) => lines.push(`    ${l.trimStart()}`));
    lines.push('}');
    lines.push('');
  }

  const hasIsBuiltIn = !relationOnly && entity.properties.some((p) => p.name === 'IsBuiltIn');
  if (hasIsBuiltIn) {
    lines.push('// ========================================');
    lines.push(`// ${entityShort} 内置 DTO`);
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 内置更新 DTO`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}BuiltInDto`);
    lines.push('{');
    appendEntityIdProperty(lines, idProp, {
      required: true,
      summary: `${entityShort}ID`,
    });
    lines.push('    /// <summary>');
    lines.push('    /// 内置（字典 sys_yes_no；1=是，0=否）');
    lines.push('    /// </summary>');
    lines.push('    [Required(ErrorMessage = "内置不能为空")]');
    lines.push('    public int IsBuiltIn { get; set; } = 0;');
    lines.push('}');
    lines.push('');
  }

  const hasIsObsolete = !relationOnly && entity.properties.some((p) => p.name === 'IsObsolete');
  if (hasIsObsolete) {
    lines.push('// ========================================');
    lines.push(`// ${entityShort} 作废 DTO`);
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 作废/撤销作废 DTO`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}ObsoleteDto`);
    lines.push('{');
    appendEntityIdProperty(lines, idProp, {
      required: true,
      summary: `${entityShort}ID`,
    });
    lines.push('    /// <summary>');
    lines.push('    /// 是否作废（字典 sys_yes_no，0=否 1=是；编辑移除子行时标记作废）');
    lines.push('    /// </summary>');
    lines.push('    public int IsObsolete { get; set; }');
    lines.push('}');
    lines.push('');
  }

  if (shouldGenerateTemplateImport(entity)) {
    lines.push('// ========================================');
    lines.push('// 导入 DTO');
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 导入模板行 DTO`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}TemplateDto`);
    lines.push('{');
    appendTenantCompanyCreateImportProperties(lines, entity.entityBase, { forImport: true, entityShort });
    const templateProps = getTemplateImportProps(createProps);
    appendEmittedProperties(
      lines,
      templateProps.map((p) => ({ ...p, isNullable: true })),
      { forceNullable: true }
    );
    appendMasterDetailCreateProperties(lines, navigationProperties, entityShort, {
      cascadeChildUseUpdateDto: false,
    });
    appendInverseRbacCreateFields(lines, entityShort);
    appendExtFieldAndRemark(lines);
    lines.push('}');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 导入 DTO（独立实现，不继承 TemplateDto）`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}ImportDto`);
    lines.push('{');
    appendTenantCompanyCreateImportProperties(lines, entity.entityBase, {
      forImport: true,
      entityShort,
    });
    appendEmittedProperties(
      lines,
      templateProps.map((p) => ({ ...p, isNullable: true })),
      { forceNullable: true }
    );
    appendMasterDetailCreateProperties(lines, navigationProperties, entityShort, {
      cascadeChildUseUpdateDto: false,
    });
    appendInverseRbacCreateFields(lines, entityShort);
    appendExtFieldAndRemark(lines);
    lines.push('}');
    lines.push('');
  }

  if (shouldGenerateImportExport(entity)) {
    lines.push('// ========================================');
    lines.push('// 导出 DTO');
    lines.push('// ========================================');
    lines.push('');
    lines.push('/// <summary>');
    lines.push(`/// ${entityShort} 导出 DTO（独立实现，不继承响应 Dto）`);
    lines.push('/// </summary>');
    lines.push(`public class Takt${entityShort}ExportDto`);
    lines.push('{');
    appendEntityIdProperty(lines, idProp, { summary: `${entityShort}ID` });
    if (isCompanyOrApprovalEntityBase(entity.entityBase)) {
      lines.push('    /// <summary>');
      lines.push('    /// 公司代码');
      lines.push('    /// </summary>');
      lines.push('    public string CompanyCode { get; set; } = string.Empty;');
      lines.push('');
      lines.push('    /// <summary>');
      lines.push('    /// 工厂代码（选项 TaktPlants/options；DictValue=PlantCode）');
      lines.push('    /// </summary>');
      lines.push('    public string PlantCode { get; set; } = string.Empty;');
      lines.push('');
      lines.push('    /// <summary>');
      lines.push('    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
      lines.push('    /// </summary>');
      lines.push('    public string CultureCode { get; set; } = string.Empty;');
      lines.push('');
    } else if (isTenantEntityBase(entity.entityBase)) {
      if (entityBaseHasRelatedPlant(entity.entityBase) && entityShort !== 'Plant') {
        lines.push('    /// <summary>');
        lines.push('    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）');
        lines.push('    /// </summary>');
        lines.push('    public string RelatedPlant { get; set; } = string.Empty;');
        lines.push('');
      }
      if (entityBaseHasCultureCode(entity.entityBase)) {
        lines.push('    /// <summary>');
        lines.push('    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）');
        lines.push('    /// </summary>');
        lines.push('    public string CultureCode { get; set; } = string.Empty;');
        lines.push('');
      }
    }
    appendEmittedProperties(lines, entity.properties);
    lines.push('    /// <summary>');
    lines.push('    /// 扩展字段JSON');
    lines.push('    /// </summary>');
    lines.push('    public string? ExtField { get; set; }');
    lines.push('');
    lines.push('    /// <summary>');
    lines.push('    /// 备注');
    lines.push('    /// </summary>');
    lines.push('    public string? Remark { get; set; }');
    lines.push('');
    lines.push('    /// <summary>');
    lines.push('    /// 创建时间');
    lines.push('    /// </summary>');
    lines.push('    public DateTime CreatedAt { get; set; }');
    lines.push('}');
    lines.push('');
  }

  if (!relationOnly && isTransposableEntity(entityShort)) {
    appendTransposedDtoBlock(lines, entityShort, entityShort);
  }

  return lines.join('\n');
}

/** *Dtos.cs 内手工商附加类型保留标记（覆盖生成时原样拼回文件末尾） */
const HAND_MAINTAINED_BEGIN = '// <takt:hand-maintained-begin>';
const HAND_MAINTAINED_END = '// <takt:hand-maintained-end>';

/**
 * 从已有 *Dtos.cs 提取手工商附加块（含 begin/end 标记行）
 * @param {string} existingContent
 * @returns {string}
 */
function extractHandMaintainedDtoBlock(existingContent) {
  if (!existingContent) {
    return '';
  }
  const start = existingContent.indexOf(HAND_MAINTAINED_BEGIN);
  const end = existingContent.indexOf(HAND_MAINTAINED_END);
  if (start < 0 || end < 0 || end <= start) {
    return '';
  }
  return existingContent.slice(start, end + HAND_MAINTAINED_END.length).trimEnd();
}

/**
 * 生成内容末尾拼回手工商块；推荐非 CRUD DTO 另建 TaktXxxExtraDtos.cs，勿写进本文件
 * @param {string} content
 * @param {string} outputFile
 * @returns {{ content: string, preserved: boolean }}
 */
function mergeHandMaintainedDtoBlock(content, outputFile) {
  if (!fs.existsSync(outputFile)) {
    return { content, preserved: false };
  }
  const block = extractHandMaintainedDtoBlock(fs.readFileSync(outputFile, 'utf8'));
  if (!block) {
    return { content, preserved: false };
  }
  return {
    content: `${content.trimEnd()}\n\n${block}\n`,
    preserved: true,
  };
}

/**
 * 生成单个实体的 Dtos 文件
 * @param {object} entity
 * @param {object} options
 */
function generateEntityDtos(entity, options, entityRegistry) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const outputDir = path.join(CONFIG.dtosRoot, ...entity.dtoDirParts);
  const outputFile = path.join(outputDir, `Takt${entityShort}Dtos.cs`);

  if (!entity.properties.length) {
    console.warn(
      `⚠️  跳过（未解析到标量字段，请检查实体注释与 [Navigate] 导航属性）: ${entity.className} → ${outputFile}`,
    );
    return { skipped: true, created: false, updated: false, path: outputFile, relation: false, tree: false };
  }

  const relation = isRelationEntity(entity);
  const tree = !relation && isTreeEntity(entity);
  let content = relation
    ? generateAggregateDtoFileContent(entity, entityRegistry, { relationOnly: true })
    : generateAggregateDtoFileContent(entity, entityRegistry);
  const merged = mergeHandMaintainedDtoBlock(content, outputFile);
  content = merged.content;

  const writeResult = writeGeneratedFile(outputFile, content);
  if (merged.preserved) {
    console.log(`  ℹ️  已保留手工商附加 DTO 块（${HAND_MAINTAINED_BEGIN}）`);
  }
  const extras = [];
  if (!relation) {
    if (tree) {
      extras.push('TreeDto');
    }
    if ((entity.navigationProperties || []).length) {
      extras.push(`导航×${entity.navigationProperties.length}`);
    }
    if (shouldGenerateTemplateImport(entity)) {
      extras.push('Import/Export');
    } else if (shouldGenerateImportExport(entity)) {
      extras.push('Export');
    }
    if (findStatusProperty(entity)) {
      extras.push('StatusDto');
    }
    if (findSortOrderProperty(entity)) {
      extras.push('SortDto');
    }
    if (entity.properties.some((p) => p.name === 'IsBuiltIn')) {
      extras.push('BuiltInDto');
    }
  }
  const actionLabel = writeResult.created ? '已创建' : '已更新';
  console.log(
    `✅ ${actionLabel}: ${outputFile}（${relation ? '关联' : '聚合'}，${entity.properties.length} 个标量字段${extras.length ? `，含 ${extras.join('、')}` : ''}）`,
  );
  return { skipped: false, created: writeResult.created, updated: writeResult.updated, path: outputFile, relation, tree };
}

// ========================================
// 扫描
// ========================================

/**
 * 扫描实体目录
 * @param {string|null} entityPrefix
 */
function scanEntities(entityPrefix = null) {
  const results = [];

  function walk(dir) {
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    entries.forEach((entry) => {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(fullPath);
        return;
      }
      if (!entry.name.startsWith('Takt') || !entry.name.endsWith('.cs')) {
        return;
      }
      if (entry.name === 'TaktCompanyEntityBase.cs') {
        return;
      }

      const entityShort = entry.name.replace(/^Takt/, '').replace(/\.cs$/, '');

      if (isRbacJunctionEntity(entityShort)) {
        if (!entityPrefix) {
          console.log(`⏭️  跳过特殊实体（手工维护 DTO）: Takt${entityShort}`);
        }
        return;
      }

      if (entityPrefix && entityShort !== entityPrefix) {
        return;
      }

      const parsed = parseEntityFile(fullPath);
      if (!parsed) {
        console.warn(`⚠️  跳过（无法解析实体类）: ${fullPath}`);
        return;
      }
      results.push(parsed);
    });
  }

  walk(CONFIG.entitiesRoot);
  return results;
}

// ========================================
// CLI
// ========================================

function printUsage() {
  console.log(`
用法: node scripts/generate-dtos-from-entity.cjs [参数]

参数:
  --<实体名>         仅生成指定实体，如 --Company、--Dept（不可用 --User、--Online、--UserRole、--Holiday）
  --force            已废弃（与默认行为相同，仅为兼容 generate-all.cjs 传参保留）
  --dry-run          仅打印将生成的文件，不写入磁盘

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 输出策略：目标 *Dtos.cs 不存在则创建，已存在则整文件覆盖更新（writeGeneratedFile，无需 --force）
  - 附加非 CRUD DTO：优先独立文件 TaktXxxExtraDtos.cs（本脚本不碰）；若须同文件则用
    // <takt:hand-maintained-begin> … // <takt:hand-maintained-end> 包裹，覆盖时会拼回末尾
  - 排除：User（密码等）、Online、Message；RBAC 八表（UserRole…EmployeePost）；Holiday（含 TaktHolidayThemeDto 手工 DTO）
  - 对应 TaktUserDtos.cs、TaktOnlineDtos.cs、TaktMessageDtos.cs、TaktHolidayDtos.cs 及八张关联 *Dtos.cs
  - 主子表：响应 TaktXxxDto 含 List<子Dto>；Create 级联 List<子CreateDto>?；Update 级联 new List<子UpdateDto>?（含主键，新增行主键为 0）；导入/模板仍为 List<子CreateDto>?
  - 子表明细 IsObsolete：各 DTO 变体均含 IsObsolete；另生成 TaktXxxObsoleteDto + UpdateXxxObsoleteAsync（作废/撤销）
  - 转置（仅 Translation）：TaktTranslationTransposedDto/Query/Result/Batch
  - 输出目录: backend/src/Takt.Application/Dtos/{与实体相同的模块路径}/
  - 仅扫描 Takt.Domain/Entities 下真实实体；无实体则不生成 DTO（禁止虚拟/手写规格表）
  - 聚合实体 → TaktXxxDto / TaktXxxQueryDto / TaktXxxCreateDto / TaktXxxUpdateDto
    / TaktXxxStatusDto（含 *Status 字段）/ TaktXxxSortDto（含 SortOrder 字段）
    / TaktXxxTemplateDto、TaktXxxImportDto、TaktXxxExportDto（Import/Export 独立类，不继承）
    / 实体短名以 Log 结尾：仅生成 ExportDto，不生成 TemplateDto / ImportDto
  - 禁止 TaktCreateXxxDto、TaktUpdateXxxDto 等动词前置命名
    含 ParentId 时另生成 TreeDto
  - QueryDto 字段顺序：
      1) TenantCode；（公司/审批级）CompanyCode、CultureCode、PlantCode；（租户级）RelatedPlant
      2) 业务字段
      3) （审批级）ApprovalStatus、InitiatorId、InitiatedAtStart/End、ApprovedBy、ApprovedAtStart/End
      4) CreatedAtStart/End、ExtField、Remark
  - CreateDto / TemplateDto / ImportDto 字段顺序：
      1) TenantCode；（公司/审批级）CompanyCode、CultureCode、PlantCode；（租户级）RelatedPlant
      2) 业务字段（TemplateDto / ImportDto 与 CreateDto 全量一致，导入列可空）
      3) 主子表 List<子CreateDto>?、RBAC 反向 *Ids/*Codes（与 CreateDto 一致）
      4) ExtField、Remark
    租户四组合（Query/Create/Template/Import 必须）：
    Core 仅 TenantCode；Culture 必含 CultureCode；Plant 必含 RelatedPlant；
    默认 TenantEntity 必含 RelatedPlant + CultureCode；
    公司/审批级含 TenantCode + CompanyCode + CultureCode + PlantCode；
    租户级实体若自身声明 CultureCode（User/Translation 等）作为业务字段生成；
    TenantCode / CompanyCode / CultureCode / PlantCode / RelatedPlant 由登录或公司切换注入，不加 [Required]
    ❌ 禁止再生成 CompanyDefaultCulture（与 CultureCode 重复）
    ❌ PlantCode（工厂代码，公司/审批行）≠ RelatedPlant（关联工厂，租户行）
    Entity→Dto：Core/Culture/Plant/TenantEntity → 对应 *DtoBase；Company/Approval 同理
  - QueryDto 日期：实体含 DateTime/DateOnly 业务字段 → 各字段 XxxStart/XxxEnd；
    且始终追加 CreatedAtStart/CreatedAtEnd（基类创建时间）
  - 生成前自动执行 generate-entity-rbac-navigations（rbac-parent-config → 实体导航属性区域）
  - 「导航属性区域」内 [Navigate]：响应 TaktXxxDto 为 List<Takt关联Dto>?
    RBAC 八表：Create/Update 为 *Ids/*Codes（非 List<CreateDto>）；反向合并字段见 RBAC_INVERSE_CREATE_FIELDS
    非 RBAC 子表仍为 List<Takt子CreateDto>?；ManyToOne → Takt主表Dto?；导航不入 Query/导入导出
  - 下拉/树选项统一使用 Takt.Shared.Options.TaktSelectOption / TaktTreeSelectOption，不生成 *OptionDto

示例:
  node scripts/generate-dtos-from-entity.cjs --Company
  node scripts/generate-dtos-from-entity.cjs --CostCenter
  node scripts/generate-dtos-from-entity.cjs --Company --force
`);
}

/**
 * 解析命令行
 */
function parseArgs() {
  const options = parseSingleEntityGenerateArgs(printUsage);
  assertNotRbacJunctionEntityCli(options.entityPrefix);
  assertNotManualDtoEntityCli(options.entityPrefix);
  return options;
}

// ========================================
// 主流程
// ========================================

console.log('🚀 从实体生成 Application Dtos...');
logGeneratedFileWritePolicy();

try {
  const options = parseArgs();
  if (!options.dryRun) {
    console.log('🔗 同步主实体 RBAC 导航（rbac-parent-config）...');
    const navSync = syncAllRbacParentEntityNavigations(CONFIG.entitiesRoot, {
      entityPrefix: options.entityPrefix,
    });
    navSync.forEach((r) => {
      if (r.status === 'updated' || r.status === 'created') {
        console.log(`   ✅ 实体导航 ${r.entityShort}`);
      } else if (r.status === 'failed') {
        console.log(`   ❌ 实体导航 ${r.entityShort}: ${r.reason}`);
      }
    });
    console.log('');
  }
  const entities = scanEntities(options.entityPrefix);

  if (entities.length === 0) {
    if (options.entityPrefix && isRbacJunctionEntity(options.entityPrefix)) {
      assertNotRbacJunctionEntityCli(options.entityPrefix);
    }
    console.error('❌ 未找到匹配的实体文件');
    process.exit(1);
  }

  console.log(`📦 匹配实体 ${entities.length} 个\n`);

  const entityRegistry = new Map(entities.map((e) => [e.className, e]));

  let created = 0;
  let updated = 0;
  let skipped = 0;

  entities.forEach((entity) => {
    if (options.dryRun) {
      const entityShort = entity.className.replace(/^Takt/, '');
      const out = path.join(CONFIG.dtosRoot, ...entity.dtoDirParts, `Takt${entityShort}Dtos.cs`);
      const navCount = (entity.navigationProperties || []).length;
      console.log(
        `📄 [dry-run] ${out} ← ${entity.className} (${isRelationEntity(entity) ? '关联' : '聚合'}${navCount ? `，导航×${navCount}` : ''})`,
      );
      return;
    }
    const result = generateEntityDtos(entity, options, entityRegistry);
    if (result.skipped) {
      skipped += 1;
    } else if (result.updated) {
      updated += 1;
    } else {
      created += 1;
    }
  });

  console.log(`\n📊 已创建 ${created} 个，已更新 ${updated} 个，跳过 ${skipped} 个`);
  console.log('✨ 完成！请人工审阅生成的 DTO 并补充校验特性、填充字段与 Mapster 配置。');
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}

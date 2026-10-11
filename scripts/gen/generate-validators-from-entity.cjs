// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-validators-from-entity.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Takt.Domain 实体全量自动生成 FluentValidation 验证器（Create/Update/Import）
// 用法: node scripts/generate-validators-from-entity.cjs [--all|-all]
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  logGeneratedFileWritePolicy,
  parseAllOnlyGenerateArgs,
  sanitizeXmlDocPlainText,
  parseEntityClassHeaderFromCsContent,
  isTenantEntityBase,
  isCompanyOrApprovalEntityBase,
  entityBaseHasRelatedPlant,
  entityBaseHasCultureCode,
  ENTITY_CLASS_HEADER_REGEX,
} = require('./generate-script-common.cjs');
const { isRbacJunctionEntity, isManualDtoEntity } = require('./generate-entity-exclusions.cjs');

// ========================================
// 配置（与 generate-dtos-from-entity.cjs 对齐）
// ========================================

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  entitiesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities'),
  dtosRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Dtos'),
  validatorsRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Validators'),
};

const ext_field_MAX = 4000;
const REMARK_MAX = 500;
const DEFAULT_PASSWORD_MIN = 8;

// ========================================
// 工具
// ========================================

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

function entityToIdPropertyName(entityName) {
  const shortName = entityName.replace(/^Takt/, '');
  return `${shortName}Id`;
}

function escapeCsharpString(value) {
  return (value || '').replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

function fieldLabel(prop) {
  const raw = prop.summary || prop.name;
  let label = raw.replace(/（[\s\S]*$/, '').replace(/\([\s\S]*$/, '').trim();
  label = label.replace(/<[^>]+>/g, '').replace(/\s+/g, ' ').trim();
  const stopIdx = label.search(/[{\[]/);
  if (stopIdx > 0) {
    label = label.slice(0, stopIdx).replace(/[，,：:\s]+$/, '').trim();
  }
  if (label.length > 40) {
    label = label.slice(0, 40).trim();
  }
  return label || prop.name;
}

function getCreateProps(entity) {
  return entity.properties;
}

/**
 * DTO 文件中是否存在指定类
 * @param {string|null} dtoContent
 * @param {string|null} className
 * @returns {boolean}
 */
function hasDtoClassInFile(dtoContent, className) {
  if (!dtoContent || !className) {
    return false;
  }
  return new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b`).test(dtoContent);
}

/**
 * 关联表 DTO 文件（仅 Takt{Entity}Dto，无 Create/Update；分配走 ITaktRbacService）
 * @param {string|null} dtoContent
 * @param {string} entityShort
 */
function isRelationDtoFile(dtoContent, entityShort) {
  if (!dtoContent) {
    return false;
  }
  const dtoName = `Takt${entityShort}Dto`;
  const hasDto = hasDtoClassInFile(dtoContent, dtoName);
  const hasCreate = hasDtoClassInFile(dtoContent, `Takt${entityShort}CreateDto`);
  const hasUpdate = hasDtoClassInFile(dtoContent, `Takt${entityShort}UpdateDto`);
  return hasDto && !hasCreate && !hasUpdate;
}

/**
 * 是否仅生成 Takt{Entity}DtoValidator（RBAC 八表强制；其余看 *Dtos.cs 是否无 Create/Update）
 * @param {string} entityShort
 * @param {string|null} dtoContent
 * @returns {boolean}
 */
function shouldUseRelationValidator(entityShort, dtoContent) {
  if (isRbacJunctionEntity(entityShort)) {
    return true;
  }
  return isRelationDtoFile(dtoContent, entityShort);
}

/**
 * 导入校验用业务字段（与 CreateDto 业务标量字段一致）
 * @param {object[]} createProps
 * @returns {object[]}
 */
function getTemplateImportProps(createProps) {
  return createProps;
}

function isCreateRequiredString(prop) {
  return prop.bareType === 'string' && !prop.isNullable && !/Hash$/i.test(prop.name);
}

function entityUsesSharedEnums(entity) {
  return entity.properties.some((p) => /^Takt[A-Z]/.test(p.bareType));
}

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

function parseSugarColumnMeta(blockBeforeProperty) {
  const matches = [...blockBeforeProperty.matchAll(/\[SugarColumn\(([\s\S]*?)\)\]/g)];
  if (matches.length === 0) {
    return {};
  }
  const attrs = matches[matches.length - 1][1];
  const lengthMatch = attrs.match(/Length\s*=\s*(\d+)/);
  const nullableMatch = attrs.match(/IsNullable\s*=\s*(true|false)/);
  return {
    maxLength: lengthMatch ? parseInt(lengthMatch[1], 10) : null,
    sugarNullable: nullableMatch ? nullableMatch[1] === 'true' : null,
  };
}

function parseEntityProperties(classBody) {
  const properties = [];
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>(?:[\s\S]*?)?([\w.?[\]]+)\s+(\w+)\s*\{[\s\S]*?get;\s*set;/g;
  let match;

  while ((match = propertyRegex.exec(classBody)) !== null) {
    const summary = normalizeDocText(match[1]);
    const csharpType = match[2].trim();
    const name = match[3];

    // Navigate 在 summary 之后、public 之前，须检查整段匹配内容（非 match.index 之前）
    if (/\[Navigate\s*\(/.test(match[0])) {
      continue;
    }

    const publicDecl = `public ${csharpType} ${name}`;
    const publicIndex = classBody.indexOf(publicDecl, match.index);
    const blockEnd = publicIndex >= 0 ? publicIndex : match.index + match[0].length;
    const block = classBody.slice(Math.max(0, blockEnd - 600), blockEnd);
    const sugar = parseSugarColumnMeta(block);

    properties.push({
      name,
      csharpType,
      summary,
      isNullable: csharpType.endsWith('?'),
      bareType: csharpType.replace('?', ''),
      maxLength: sugar.maxLength,
      sugarNullable: sugar.sugarNullable,
    });
  }

  return properties;
}

function entityNamespaceToDirParts(entityNamespace) {
  const suffix = entityNamespace.replace(/^Takt\.Domain\.Entities\.?/, '');
  return suffix ? suffix.split('.').filter(Boolean) : [];
}

function findDtoFile(entity) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const dtoPath = path.join(CONFIG.dtosRoot, ...entity.dirParts, `Takt${entityShort}Dtos.cs`);
  return fs.existsSync(dtoPath) ? dtoPath : null;
}

function extractClassBlock(content, className) {
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b`);
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
 * 解析 DTO 类声明中的基类名（TaktCreateUserDto : TaktUserCreateDto）
 * @param {string} content
 * @param {string} className
 * @returns {string|null}
 */
function extractDtoBaseClassName(content, className) {
  const headerRegex = new RegExp(
    `public\\s+(?:partial\\s+)?class\\s+${className}\\b([^\\{]*)\\{`,
    's',
  );
  const headerMatch = headerRegex.exec(content);
  if (!headerMatch) {
    return null;
  }
  const inheritMatch = headerMatch[1].match(/:\s*(Takt\w+)/);
  return inheritMatch ? inheritMatch[1] : null;
}

/**
 * 提取 DTO 类属性名（含基类继承字段）
 * @param {string} dtoContent
 * @param {string} className
 * @returns {Set<string>}
 */
function extractDtoPropertyNames(dtoContent, className) {
  const names = new Set();
  const baseClass = extractDtoBaseClassName(dtoContent, className);
  if (baseClass) {
    for (const name of extractDtoPropertyNames(dtoContent, baseClass)) {
      names.add(name);
    }
  }
  const block = extractClassBlock(dtoContent, className);
  const propRegex = /public\s+[\w?<>,\s\[\]]+\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * 从 *Dtos.cs 解析 Create/Update/Import 类名（仅 TaktXxxCreateDto / TaktCreateXxxDto 与 Update 对应体；禁止误匹配 TaktUpdateMyProfileResultDto 等）
 * @param {string} entityShort
 * @param {string|null} dtoContent
 */
function resolveDtoClassNames(entityShort, dtoContent) {
  const fallback = {
    create: `Takt${entityShort}CreateDto`,
    update: `Takt${entityShort}UpdateDto`,
    import: `Takt${entityShort}ImportDto`,
  };
  if (!dtoContent) {
    return fallback;
  }
  const found = { create: null, update: null, import: null };
  const aliasCreate = `TaktCreate${entityShort}Dto`;
  const aliasUpdate = `TaktUpdate${entityShort}Dto`;
  const classRegex = /public\s+(?:partial\s+)?class\s+(\w+)\s*(?::|\{)/g;
  let match;
  while ((match = classRegex.exec(dtoContent)) !== null) {
    const className = match[1];
    if (className === fallback.create) {
      found.create = className;
    } else if (className === aliasCreate && !found.create) {
      found.create = className;
    } else if (className === fallback.update) {
      found.update = className;
    } else if (className === aliasUpdate && !found.update) {
      found.update = className;
    } else if (className === fallback.import) {
      found.import = className;
    }
  }
  return {
    create: found.create || fallback.create,
    update: found.update || fallback.update,
    import: found.import || fallback.import,
  };
}

/**
 * 按 DTO 实际字段过滤实体属性（仅生成 DTO 上存在的校验）
 * @param {object[]} entityProps
 * @param {Set<string>|null} dtoPropNames
 */
function filterPropsForDto(entityProps, dtoPropNames) {
  if (dtoPropNames === null || dtoPropNames === undefined) {
    return entityProps;
  }
  if (dtoPropNames.size === 0) {
    return [];
  }
  return entityProps.filter((p) => dtoPropNames.has(p.name));
}

/**
 * 实体 string? 不参与 Create/Update 校验；非空 string 必须校验
 * @param {object} prop
 * @returns {boolean}
 */
function shouldValidateProperty(prop) {
  if (prop.bareType === 'string' && prop.isNullable) {
    return false;
  }
  return true;
}

function buildEntityPropMap(entity) {
  return new Map(entity.properties.map((p) => [p.name, p]));
}

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
  const namespaceMatch = content.match(/namespace\s+([\w.]+);/);
  const entityNamespace = namespaceMatch ? namespaceMatch[1] : '';
  const dirParts = entityNamespaceToDirParts(entityNamespace);
  const namespaceSuffix = dirParts.length ? `.${dirParts.join('.')}` : '';

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
  ]);

  const allProperties = parseEntityProperties(classBody);
  const properties = allProperties.filter((p) => !ENTITY_BASE_FIELDS.has(p.name));

  return {
    className,
    entityBase,
    entityNamespace,
    dirParts,
    validatorNamespace: `Takt.Application.Validators${namespaceSuffix}`,
    dtoNamespace: `Takt.Application.Dtos${namespaceSuffix}`,
    properties,
    filePath,
  };
}

// ========================================
// FluentValidation 规则生成
// ========================================

/**
 * 冗余 Code/Name 对应的主表外键（XxxCode→XxxId；仅当 XxxId 在实体/DTO 上为 long）
 * @param {string} propName
 * @param {Set<string>|null|undefined} dtoPropNames
 * @param {Map<string, object>|null|undefined} entityPropMap
 * @returns {string|null}
 */
function resolveRedundantFieldFkGuard(propName, dtoPropNames, entityPropMap) {
  if (!dtoPropNames || !propName) {
    return null;
  }
  for (const suffix of ['Code', 'Name']) {
    if (!propName.endsWith(suffix) || propName.length <= suffix.length) {
      continue;
    }
    const fk = `${propName.slice(0, -suffix.length)}Id`;
    if (fk === 'Id' || !dtoPropNames.has(fk)) {
      continue;
    }
    const entityProp = entityPropMap?.get(fk);
    if (!entityProp || entityProp.bareType !== 'long') {
      continue;
    }
    return fk;
  }
  return null;
}

/** 角色前缀外键：Plant/Culture Stamp 守卫优先选用无此前缀的主表 FK */
const STAMP_FK_ROLE_PREFIX = /^(Original|Proxy|Source|Target|Related|From|To)/;

/**
 * 子表 Stamp 可用的主表外键（配套 *Code/*Name 的 long *Id；无则回退主子关联 FK）
 * @param {Set<string>|null|undefined} dtoPropNames
 * @param {string} [entityShort]
 * @param {Map<string, object>|null|undefined} [entityPropMap]
 * @returns {string|null}
 */
function resolvePrimaryMasterFkForStamp(dtoPropNames, entityShort, entityPropMap) {
  if (!dtoPropNames || dtoPropNames.size === 0) {
    return null;
  }
  /** @type {string[]} */
  const candidates = [];
  for (const name of dtoPropNames) {
    if (!name.endsWith('Id') || name === 'Id' || name === 'ParentId') {
      continue;
    }
    const entityProp = entityPropMap?.get(name);
    if (!entityProp || entityProp.bareType !== 'long') {
      continue;
    }
    const prefix = name.slice(0, -2);
    if (dtoPropNames.has(`${prefix}Code`) || dtoPropNames.has(`${prefix}Name`)) {
      candidates.push(name);
    }
  }
  if (candidates.length > 0) {
    candidates.sort((a, b) => {
      const aRole = STAMP_FK_ROLE_PREFIX.test(a) ? 1 : 0;
      const bRole = STAMP_FK_ROLE_PREFIX.test(b) ? 1 : 0;
      if (aRole !== bRole) {
        return aRole - bRole;
      }
      return a.length - b.length;
    });
    return candidates[0];
  }
  if (entityShort) {
    try {
      const { listAssociationsForChild } = require('./generate-master-detail-associations.cjs');
      const assocs = listAssociationsForChild(entityShort);
      for (const assoc of assocs) {
        const fk = assoc.fkFieldOnChild;
        if (!fk || !fk.endsWith('Id') || !dtoPropNames.has(fk)) {
          continue;
        }
        const entityProp = entityPropMap?.get(fk);
        if (!entityProp || entityProp.bareType !== 'long') {
          continue;
        }
        return fk;
      }
    } catch {
      // 关联模块不可用时仅依赖配套 Code/Name
    }
  }
  return null;
}

/**
 * @param {object} prop
 * @param {object} options
 * @returns {string[]}
 */
function buildValidationRules(prop, options) {
  const { mode = 'create', required = false, stampFkGuard = null } = options;
  if (!shouldValidateProperty(prop)) {
    return [];
  }
  const label = escapeCsharpString(fieldLabel(prop));
  const rules = [];

  if (prop.bareType === 'string') {
    if (required) {
      if (stampFkGuard) {
        rules.push(
          `NotEmpty().WithMessage("${label}不能为空").When(x => x.${stampFkGuard} <= 0)`,
        );
      } else {
        rules.push(`NotEmpty().WithMessage("${label}不能为空")`);
      }
    }
    if (prop.maxLength) {
      if (mode === 'import' && !required) {
        rules.push(
          `MaximumLength(${prop.maxLength}).WithMessage("${label}长度不能超过${prop.maxLength}个字符").When(x => !string.IsNullOrWhiteSpace(x.${prop.name}))`
        );
      } else {
        rules.push(`MaximumLength(${prop.maxLength}).WithMessage("${label}长度不能超过${prop.maxLength}个字符")`);
      }
    }
    if (/Password/i.test(prop.name) && !/Hash$/i.test(prop.name)) {
      if (required) {
        rules.push(`MinimumLength(${DEFAULT_PASSWORD_MIN}).WithMessage("${label}长度不能少于${DEFAULT_PASSWORD_MIN}位")`);
      }
    }
    if (/Email/i.test(prop.name)) {
      rules.push(
        `EmailAddress().WithMessage("${label}格式不正确").When(x => !string.IsNullOrWhiteSpace(x.${prop.name}))`
      );
    }
  } else if (prop.bareType === 'long') {
    if (prop.name === 'ParentId' || prop.name.endsWith('Id')) {
      rules.push(`GreaterThanOrEqualTo(0).WithMessage("${label}不能为负数")`);
    }
  } else if (prop.bareType === 'int') {
    if (prop.name === 'SortOrder') {
      rules.push(`GreaterThanOrEqualTo(0).WithMessage("${label}不能为负数")`);
    }
  } else if (/^Takt[A-Z]/.test(prop.bareType)) {
    rules.push(`IsInEnum().WithMessage("${label}无效")`);
  }

  return rules;
}

/**
 * @param {object} prop
 * @param {object} options
 * @returns {string|null}
 */
function emitRuleFor(prop, options) {
  /** SortOrder 由后端 ITaktSortOrderGenerator 自动回填，不入 Create/Import 校验 */
  if (prop.name === 'SortOrder') {
    return null;
  }
  const rules = buildValidationRules(prop, options);
  if (rules.length === 0) {
    return null;
  }
  return `        RuleFor(x => x.${prop.name})\n            .${rules.join('\n            .')};`;
}

function emitExtFieldAndRemarkRules(mode) {
  const lines = [];
  const extWhen =
    mode === 'import'
      ? '.When(x => !string.IsNullOrWhiteSpace(x.ExtField))'
      : '';
  const remarkWhen =
    mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.Remark))' : '';

  lines.push(`        RuleFor(x => x.ExtField)`);
  lines.push(
    `            .MaximumLength(${ext_field_MAX}).WithMessage("扩展字段JSON长度不能超过${ext_field_MAX}个字符")${extWhen};`
  );
  lines.push(`        RuleFor(x => x.Remark)`);
  lines.push(
    `            .MaximumLength(${REMARK_MAX}).WithMessage("备注长度不能超过${REMARK_MAX}个字符")${remarkWhen};`
  );
  return lines;
}

/**
 * Create/Import DTO 租户与公司隔离字段校验（与 generate-dtos-from-entity 固定字段对齐）
 * @param {string} entityBase
 * @param {'create'|'import'} mode
 * @param {Set<string>|null} dtoPropNames
 * @returns {string[]}
 */
function emitTenantCompanyScopeRules(entity, entityBase, mode, dtoPropNames) {
  const lines = [];
  const entityShort = String(entity?.className || '').replace(/^Takt/, '');
  const entityPropMap = entity?.properties
    ? new Map(entity.properties.map((p) => [p.name, p]))
    : null;
  const tenantWhen =
    mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.TenantCode))' : '';
  const companyWhen =
    mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.CompanyCode))' : '';
  if (!dtoPropNames || dtoPropNames.has('TenantCode')) {
    lines.push('        RuleFor(x => x.TenantCode)');
    if (mode === 'create') {
      lines.push('            .NotEmpty().WithMessage("租户编码不能为空")');
    }
    lines.push(`            .MaximumLength(3).WithMessage("租户编码长度不能超过3个字符")${tenantWhen};`);
  }
  if (
    isCompanyOrApprovalEntityBase(entityBase) &&
    (!dtoPropNames || dtoPropNames.has('CompanyCode'))
  ) {
    lines.push('        RuleFor(x => x.CompanyCode)');
    if (mode === 'create') {
      lines.push('            .NotEmpty().WithMessage("公司代码不能为空")');
    }
    lines.push(`            .MaximumLength(4).WithMessage("公司代码长度不能超过4个字符")${companyWhen};`);
  }
  if (
    isCompanyOrApprovalEntityBase(entityBase) &&
    (!dtoPropNames || dtoPropNames.has('CultureCode'))
  ) {
    const stampFk = resolvePrimaryMasterFkForStamp(dtoPropNames, entityShort, entityPropMap);
    const cultureWhen =
      mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.CultureCode))' : '';
    lines.push('        RuleFor(x => x.CultureCode)');
    if (mode === 'create') {
      if (stampFk) {
        lines.push(
          `            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.${stampFk} <= 0)`,
        );
      } else {
        lines.push('            .NotEmpty().WithMessage("区域文化编码不能为空")');
      }
    }
    lines.push(`            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符")${cultureWhen};`);
  } else if (
    isTenantEntityBase(entityBase) &&
    entityBaseHasCultureCode(entityBase) &&
    (!dtoPropNames || dtoPropNames.has('CultureCode'))
  ) {
    const stampFk = resolvePrimaryMasterFkForStamp(dtoPropNames, entityShort, entityPropMap);
    const cultureWhen =
      mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.CultureCode))' : '';
    lines.push('        RuleFor(x => x.CultureCode)');
    if (mode === 'create') {
      if (stampFk) {
        lines.push(
          `            .NotEmpty().WithMessage("区域文化编码不能为空").When(x => x.${stampFk} <= 0)`,
        );
      } else {
        lines.push('            .NotEmpty().WithMessage("区域文化编码不能为空")');
      }
    }
    lines.push(`            .MaximumLength(5).WithMessage("区域文化编码长度不能超过5个字符")${cultureWhen};`);
  }
  if (
    isCompanyOrApprovalEntityBase(entityBase) &&
    (!dtoPropNames || dtoPropNames.has('PlantCode'))
  ) {
    const stampFk = resolvePrimaryMasterFkForStamp(dtoPropNames, entityShort, entityPropMap);
    const plantWhen =
      mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.PlantCode))' : '';
    lines.push('        RuleFor(x => x.PlantCode)');
    if (mode === 'create') {
      if (stampFk) {
        lines.push(
          `            .NotEmpty().WithMessage("工厂代码不能为空").When(x => x.${stampFk} <= 0)`,
        );
      } else {
        lines.push('            .NotEmpty().WithMessage("工厂代码不能为空")');
      }
    }
    lines.push(`            .MaximumLength(4).WithMessage("工厂代码长度不能超过4个字符")${plantWhen};`);
  }
  if (
    entityBaseHasRelatedPlant(entityBase) &&
    entity.className !== 'TaktPlant' &&
    (!dtoPropNames || dtoPropNames.has('RelatedPlant'))
  ) {
    const relatedWhen =
      mode === 'import' ? '.When(x => !string.IsNullOrWhiteSpace(x.RelatedPlant))' : '';
    lines.push('        RuleFor(x => x.RelatedPlant)');
    if (mode === 'create') {
      lines.push('            .NotEmpty().WithMessage("关联工厂不能为空")');
    }
    lines.push(`            .MaximumLength(4).WithMessage("关联工厂长度不能超过4个字符")${relatedWhen};`);
  }
  return lines;
}

function buildFileHeader(entity, description) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const today = new Date().toISOString().split('T')[0];
  return [
    '// ========================================',
    '// 项目名称：节拍智控·TaktWise',
    `// 命名空间：${entity.validatorNamespace}`,
    `// 文件名称：Takt${entityShort}Validators.cs`,
    `// 创建时间：${today}`,
    '// 创建人：Takt365(Auto Generated)',
    `// 功能描述：${description}（由 generate-validators-from-entity.cjs 根据 ${entity.className} 生成，请按需审阅）`,
    '// ',
    '// 版权信息：Copyright (c) 2025 Takt  All rights reserved.',
    '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。',
    '// ========================================',
    '',
  ];
}

function emitValidatorClass(lines, className, dtoName, entityShort, actionLabel, ruleLines) {
  lines.push('/// <summary>');
  lines.push(`/// ${actionLabel}${entityShort} DTO 验证器`);
  lines.push('/// </summary>');
  lines.push(`public class ${className} : AbstractValidator<${dtoName}>`);
  lines.push('{');
  lines.push(`    /// <summary>`);
  lines.push(`    /// 初始化 ${actionLabel}${entityShort} 校验规则`);
  lines.push(`    /// </summary>`);
  lines.push(`    public ${className}()`);
  lines.push('    {');
  ruleLines.forEach((l) => lines.push(l));
  lines.push('    }');
  lines.push('}');
  lines.push('');
}

function generateValidatorFileContent(entity) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const idProp = entityToIdPropertyName(entity.className);
  const entityCreateProps = getCreateProps(entity);
  const propMap = buildEntityPropMap(entity);

  const dtoFile = findDtoFile(entity);
  const dtoContent = dtoFile ? fs.readFileSync(dtoFile, 'utf-8') : null;
  const dtoNames = resolveDtoClassNames(entityShort, dtoContent);
  const hasCreateDto = hasDtoClassInFile(dtoContent, dtoNames.create);
  const hasUpdateDto = hasDtoClassInFile(dtoContent, dtoNames.update);
  const hasImportDtoClass = hasDtoClassInFile(dtoContent, dtoNames.import);

  const createDtoPropNames =
    hasCreateDto && dtoContent ? extractDtoPropertyNames(dtoContent, dtoNames.create) : null;
  const updateDtoPropNames =
    hasUpdateDto && dtoContent ? extractDtoPropertyNames(dtoContent, dtoNames.update) : null;
  const importDtoPropNames =
    hasImportDtoClass && dtoContent ? extractDtoPropertyNames(dtoContent, dtoNames.import) : null;

  const createProps = filterPropsForDto(entityCreateProps, createDtoPropNames);
  const importPropNames =
    hasImportDtoClass && importDtoPropNames && importDtoPropNames.size > 0
      ? importDtoPropNames
      : hasImportDtoClass
        ? new Set(getTemplateImportProps(entityCreateProps).map((p) => p.name))
        : new Set();
  const importProps = [...importPropNames]
    .map((name) => propMap.get(name))
    .filter(Boolean);

  const lines = [];
  lines.push(...buildFileHeader(entity, `${entityShort} 模块 FluentValidation 验证器`));
  lines.push('using FluentValidation;');
  lines.push(`using ${entity.dtoNamespace};`);
  if (entityUsesSharedEnums(entity)) {
    lines.push('using Takt.Shared.Enums;');
  }
  lines.push('');
  lines.push(`namespace ${entity.validatorNamespace};`);
  lines.push('');

  // Create
  const createRules = [];
  createRules.push(...emitTenantCompanyScopeRules(entity, entity.entityBase, 'create', createDtoPropNames));
  createProps.forEach((prop) => {
    if (!shouldValidateProperty(prop)) {
      return;
    }
    const required = isCreateRequiredString(prop);
    const stampFkGuard = resolveRedundantFieldFkGuard(prop.name, createDtoPropNames, propMap);
    const rule = emitRuleFor(prop, { mode: 'create', required, stampFkGuard });
    if (rule) {
      createRules.push(rule);
    }
  });
  if (!createDtoPropNames || createDtoPropNames.has('ExtField') || createDtoPropNames.has('Remark')) {
    createRules.push(
      ...emitExtFieldAndRemarkRules('create').filter((line) => {
        if (createDtoPropNames && line.includes('ExtField') && !createDtoPropNames.has('ExtField')) {
          return false;
        }
        if (createDtoPropNames && line.includes('Remark') && !createDtoPropNames.has('Remark')) {
          return false;
        }
        return true;
      })
    );
  }

  if (hasCreateDto) {
    lines.push('// ========================================');
    lines.push(`// 创建${entityShort} 验证器`);
    lines.push('// ========================================');
    lines.push('');
    emitValidatorClass(
      lines,
      `Takt${entityShort}CreateValidator`,
      dtoNames.create,
      entityShort,
      '创建',
      createRules,
    );
  }

  if (hasUpdateDto) {
    const updateEntityProps = filterPropsForDto(entityCreateProps, updateDtoPropNames);
    const updateFieldRules = [];
    updateEntityProps.forEach((prop) => {
      if (!shouldValidateProperty(prop)) {
        return;
      }
      const required = isCreateRequiredString(prop);
      const stampFkGuard = resolveRedundantFieldFkGuard(prop.name, updateDtoPropNames, propMap);
      const rule = emitRuleFor(prop, { mode: 'create', required, stampFkGuard });
      if (rule) {
        updateFieldRules.push(rule);
      }
    });
    const updateRules = [
      `        RuleFor(x => x.${idProp})`,
      `            .GreaterThan(0).WithMessage("${entityShort}ID无效");`,
      ...emitTenantCompanyScopeRules(entity, entity.entityBase, 'create', updateDtoPropNames),
      ...updateFieldRules,
    ];
    if (!updateDtoPropNames || updateDtoPropNames.has('ExtField') || updateDtoPropNames.has('Remark')) {
      const extRules = emitExtFieldAndRemarkRules('create');
      if (updateDtoPropNames) {
        updateRules.push(
          ...extRules.filter((line) => {
            if (line.includes('ExtField') && !updateDtoPropNames.has('ExtField')) {
              return false;
            }
            if (line.includes('Remark') && !updateDtoPropNames.has('Remark')) {
              return false;
            }
            return true;
          }),
        );
      } else {
        updateRules.push(...extRules);
      }
    }
    lines.push('// ========================================');
    lines.push(`// 更新${entityShort} 验证器`);
    lines.push('// ========================================');
    lines.push('');
    emitValidatorClass(
      lines,
      `Takt${entityShort}UpdateValidator`,
      dtoNames.update,
      entityShort,
      '更新',
      updateRules,
    );
  } else if (isRbacJunctionEntity(entityShort) && hasCreateDto) {
    lines.push('// RBAC 关联表无 UpdateDto，分配走 ITaktRbacService，不生成 UpdateValidator');
    lines.push('');
  }

  if (hasImportDtoClass) {
    const importRules = [];
    importRules.push(...emitTenantCompanyScopeRules(entity, entity.entityBase, 'import', importDtoPropNames));
    importProps.forEach((prop) => {
      if (!shouldValidateProperty(prop)) {
        return;
      }
      const required = isCreateRequiredString(prop);
      const rule = emitRuleFor(prop, { mode: 'import', required });
      if (rule) {
        importRules.push(rule);
      }
    });
    if (!importDtoPropNames || importDtoPropNames.has('ExtField') || importDtoPropNames.has('Remark')) {
      const extRules = emitExtFieldAndRemarkRules('import');
      importRules.push(
        ...extRules.filter((line) => {
          if (line.includes('ExtField') && importDtoPropNames && !importDtoPropNames.has('ExtField')) {
            return false;
          }
          if (line.includes('Remark') && importDtoPropNames && !importDtoPropNames.has('Remark')) {
            return false;
          }
          return true;
        })
      );
    }

    lines.push('// ========================================');
    lines.push(`// 导入${entityShort} 验证器`);
    lines.push('// ========================================');
    lines.push('');
    emitValidatorClass(
      lines,
      `Takt${entityShort}ImportValidator`,
      dtoNames.import,
      entityShort,
      '导入',
      importRules
    );
  }

  return lines.join('\n');
}

/**
 * 关联实体：单文件 Takt{Entity}DtoValidator（对应 Takt{Entity}Dto）
 * @param {object} entity
 * @param {string} dtoContent
 */
function generateRelationValidatorFileContent(entity, dtoContent) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const dtoName = `Takt${entityShort}Dto`;
  const dtoPropNames = extractDtoPropertyNames(dtoContent, dtoName);
  const entityPropSet = new Set(entity.properties.map((p) => p.name));

  const ruleLines = [];

  if (dtoPropNames.has('Id')) {
    ruleLines.push(
      `        RuleFor(x => x.Id)\n            .GreaterThan(0).WithMessage("${entityShort}ID无效");`,
    );
  }

  entity.properties.forEach((prop) => {
    if (!dtoPropNames.has(prop.name)) {
      return;
    }
    if (prop.bareType === 'long' && /Id$/.test(prop.name)) {
      ruleLines.push(
        `        RuleFor(x => x.${prop.name})\n            .GreaterThan(0).WithMessage("${fieldLabel(prop)}无效");`,
      );
      return;
    }
    const required = isCreateRequiredString(prop);
    const rule = emitRuleFor(prop, { mode: 'create', required });
    if (rule) {
      ruleLines.push(rule);
    }
  });

  [...dtoPropNames].forEach((name) => {
    if (entityPropSet.has(name) || name === 'Id') {
      return;
    }
    if (!name.endsWith('Name')) {
      return;
    }
    const rule = `        RuleFor(x => x.${name})\n            .MaximumLength(200).WithMessage("${name}长度不能超过200个字符").When(x => !string.IsNullOrWhiteSpace(x.${name}));`;
    ruleLines.push(rule);
  });

  const lines = [];
  lines.push(...buildFileHeader(entity, `${entityShort} 关联 DTO FluentValidation 验证器`));
  lines.push('using FluentValidation;');
  lines.push(`using ${entity.dtoNamespace};`);
  if (entityUsesSharedEnums(entity)) {
    lines.push('using Takt.Shared.Enums;');
  }
  lines.push('');
  lines.push(`namespace ${entity.validatorNamespace};`);
  lines.push('');
  lines.push('// ========================================');
  lines.push(`// ${entityShort} 关联 DTO 验证器`);
  lines.push('// ========================================');
  lines.push('');
  emitValidatorClass(lines, `Takt${entityShort}DtoValidator`, dtoName, entityShort, '关联 ', ruleLines);

  return lines.join('\n');
}

function scanEntities() {
  const results = [];

  function walk(dir) {
    fs.readdirSync(dir, { withFileTypes: true }).forEach((entry) => {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        walk(fullPath);
        return;
      }
      if (!entry.name.startsWith('Takt') || !entry.name.endsWith('.cs') || entry.name === 'TaktCompanyEntityBase.cs') {
        return;
      }
      const entityShort = entry.name.replace(/^Takt/, '').replace(/\.cs$/, '');
      if (isManualDtoEntity(entityShort)) {
        console.log(`⏭️  跳过手工维护验证器: Takt${entityShort}`);
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

function generateEntityValidators(entity) {
  const entityShort = entity.className.replace(/^Takt/, '');
  const outputDir = path.join(CONFIG.validatorsRoot, ...entity.dirParts);
  const outputFile = path.join(outputDir, `Takt${entityShort}Validators.cs`);

  const dtoFile = findDtoFile(entity);
  const dtoContent = dtoFile ? fs.readFileSync(dtoFile, 'utf-8') : null;
  const isRelation = shouldUseRelationValidator(entityShort, dtoContent);

  const content = isRelation
    ? generateRelationValidatorFileContent(entity, dtoContent)
    : generateValidatorFileContent(entity);
  const writeResult = writeGeneratedFile(outputFile, content);
  const actionLabel = writeResult.created ? '已创建' : '已更新';
  const kindLabel = isRelation
    ? isRbacJunctionEntity(entityShort)
      ? 'RBAC 关联 Dto（仅列表）'
      : '关联 Dto'
    : 'Create / Update / Import';
  console.log(`✅ ${actionLabel}: ${outputFile}（${kindLabel} 验证器）`);
  return { created: writeResult.created, updated: writeResult.updated, path: outputFile, relation: isRelation };
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-validators-from-entity.cjs [--all|-all]

说明:
  - 全量扫描 Domain/Entities 下全部 Takt* 实体并生成验证器
  - 仅支持无参或 --all / -all，不支持其他参数

输出: backend/src/Takt.Application/Validators/{与实体相同路径}/Takt{Entity}Validators.cs
  - 聚合实体：Takt{Entity}CreateValidator / UpdateValidator / ImportValidator（*Dtos.cs 中存在的类才生成）
  - 关联 / RBAC 八表：仅 Takt{Entity}DtoValidator（无 Create/Update/Import；分配走 ITaktRbacService）

说明:
  - RBAC 八表（UserRole…EmployeePost）强制关联模式，与 TaktRoleDeptValidators 一致
  - resolveDtoClassNames 不为缺失的 Update/Import 做 fallback，避免虚构 DTO 验证器

示例:
  node scripts/generate-validators-from-entity.cjs
  node scripts/generate-validators-from-entity.cjs --all
`);
}

function parseArgs() {
  parseAllOnlyGenerateArgs(printUsage);
}

console.log('🚀 从实体全量生成 FluentValidation 验证器（--all）...\n');
logGeneratedFileWritePolicy();

try {
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
    const result = generateEntityValidators(entity);
    if (result.updated) {
      updated += 1;
    } else {
      created += 1;
    }
  });

  console.log(`\n📊 已创建 ${created} 个，已更新 ${updated} 个`);
  console.log('✨ 完成！请确保 WebApi 已启用 FluentValidation 自动验证。');
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}

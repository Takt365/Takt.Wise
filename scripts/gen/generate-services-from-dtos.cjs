// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-services-from-dtos.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Takt.Application/Dtos/*Dtos.cs 自动生成服务接口与实现（独立脚本）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  logGeneratedFileWritePolicy,
  parseSingleEntityGenerateArgsFromArgv,
  parseEntityBaseFromCsFile,
  dtoBaseHasCompanyIsolation,
  resolveIsolationDtoBase,
  isCompanyOrApprovalEntityBase,
  getIsolationStampFieldNamesForEntityBase,
  isTaktNumberingAutoFormFieldSummary,
} = require('./generate-script-common.cjs');
const {
  isRbacJunctionEntity,
  isStandaloneChildVueEntity,
  assertNotRbacJunctionEntityCli,
  assertNotManualDtoEntityCli,
  shouldExcludeDtoFile: shouldExcludeRbacDtoFile,
  shouldExcludeStandaloneService,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
} = require('./generate-entity-exclusions.cjs');

const {
  hasRbacParentConfig,
  generateRbacParentDelegationExtras,
} = require('./rbac-parent-config.cjs');
const {
  isTransposableEntity,
  getTransposableConfig,
  generateTransposedInterfaceMethods,
  generateTransposedServiceImplementation,
} = require('./generate-transposed-support.cjs');
const {
  isSharedEnumType,
  extractPrimaryEnableStatusMeta,
  extractBuiltInDisableStatusMeta,
  optionsBlockUsesStaleIntStatusCompare,
  parseEntityScalarProperties,
} = require('./generate-enum-common.cjs');
const { extractPreservedExtraServiceMethods } = require('./service-method-preservation.cjs');

/** 扩展方法块可能引用的额外 using（按实体/内容推断） */
const EXTRA_SERVICE_USING_BY_ENTITY = {
  DictData: ['Takt.Shared.Models.Foundation'],
};

/**
 * 推断保留扩展方法所需的额外 using
 * @param {string} entityShort
 * @param {string} preservedBlocks
 * @returns {string[]}
 */
function collectPreservedExtraUsings(entityShort, preservedBlocks) {
  const usings = new Set(EXTRA_SERVICE_USING_BY_ENTITY[entityShort] || []);
  if (/TaktDictSnapshot|TaktDictStorageContext/.test(preservedBlocks || '')) {
    usings.add('Takt.Shared.Models.Foundation');
  }
  return [...usings];
}

// ========================================
// 配置
// ========================================

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  entitiesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities'),
  dtosRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Dtos'),
  servicesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Services'),
};

/** 禁止进入扫描列表的 *Dtos.cs（无对应实体 CRUD 聚合） */
const INFRASTRUCTURE_DTO_FILE_NAMES = new Set([
  'TaktLoginDtos.cs',
  'TaktCacheDtos.cs',
  'TaktServerMonitorDtos.cs',
]);

/** QueryDto 中继承自 TaktPagedQuery 的字段，不参与 QueryExpression */
const PAGED_QUERY_FIELDS = new Set(['PageIndex', 'PageSize', 'KeyWords']);

/**
 * 列表/导出：无业务查询条件时返回空结果（不默认当前月、不全表扫描）。
 * 有条件时走 QueryExpression + 分页/导出（正常过滤，不是「解锁全表」）。
 * 由 HasAnyListQueryFilter + GetXxxListAsync / ExportXxxAsync 入口守卫统一实现。
 */

/** DTO 基类（与 TaktDtoBase.cs 一致，驱动隔离过滤与仓储接口） */
const DTO_BASE_NAMES = [
  'TaktTenantCoreDtoBase',
  'TaktTenantCultureDtoBase',
  'TaktTenantPlantDtoBase',
  'TaktTenantDtoBase',
  'TaktCompanyDtoBase',
  'TaktApprovalDtoBase',
];

const DTO_BASE_TO_ENTITY_BASE = {
  TaktTenantCoreDtoBase: 'TaktTenantCoreEntityBase',
  TaktTenantCultureDtoBase: 'TaktTenantCultureEntityBase',
  TaktTenantPlantDtoBase: 'TaktTenantPlantEntityBase',
  TaktTenantDtoBase: 'TaktTenantEntityBase',
  TaktCompanyDtoBase: 'TaktCompanyEntityBase',
  TaktApprovalDtoBase: 'TaktApprovalEntityBase',
};

const DTO_BASE_TO_REPOSITORY = {
  TaktTenantCoreDtoBase: 'ITaktTenantRepository',
  TaktTenantCultureDtoBase: 'ITaktTenantRepository',
  TaktTenantPlantDtoBase: 'ITaktTenantRepository',
  TaktTenantDtoBase: 'ITaktTenantRepository',
  TaktCompanyDtoBase: 'ITaktCompanyRepository',
  TaktApprovalDtoBase: 'ITaktApprovalRepository',
};

/** 实体基类 → 仓储接口（子表注入） */
const ENTITY_BASE_TO_REPOSITORY = {
  TaktTenantCoreEntityBase: 'ITaktTenantRepository',
  TaktTenantCultureEntityBase: 'ITaktTenantRepository',
  TaktTenantPlantEntityBase: 'ITaktTenantRepository',
  TaktTenantEntityBase: 'ITaktTenantRepository',
  TaktCompanyEntityBase: 'ITaktCompanyRepository',
  TaktApprovalEntityBase: 'ITaktApprovalRepository',
};

const NAVIGATION_REGION_MARKER = '导航属性区域';

/** 唯一索引中不参与查重的系统/审计字段（TenantCode 亦由仓储隔离，见 getUniqueIndexScopeFields） */
const UNIQUE_INDEX_SKIP_FIELDS = new Set([
  'TenantCode',
  'Id',
  'IsDeleted',
  'CreatedAt',
  'UpdatedAt',
  'CreatedBy',
  'UpdatedBy',
  'DeletedBy',
  'DeletedAt',
  'ApprovalStatus',
  'InitiatorId',
  'InitiatedAt',
  'ApprovedBy',
  'ApprovedAt',
]);

// ========================================
// 工具
// ========================================

/** 已有手工服务（单实体生成时须 --force 才覆盖） */
const EXISTING_MANUAL_SERVICE_ENTITIES = new Set(['TaktAuth', 'TaktRbac', 'TaktFlowEngine']);

/**
 * 是否应跳过该 Dtos 文件
 * @param {string} dtoFile 绝对路径
 */
function shouldExcludeDtoFile(dtoFile) {
  const fileName = path.basename(dtoFile);
  if (INFRASTRUCTURE_DTO_FILE_NAMES.has(fileName)) {
    return true;
  }
  return shouldExcludeRbacDtoFile(dtoFile);
}

function isInEngineDirectory(filePath) {
  const normalizedPath = filePath.replace(/\\/g, '/');
  return /\/\w*[Ee]ngine($|\/)/i.test(normalizedPath);
}

function readUtf8(filePath) {
  return fs.readFileSync(filePath, 'utf-8');
}

function entityNameFromDtoFile(dtoFile) {
  const base = path.basename(dtoFile, '.cs');
  if (!base.endsWith('Dtos')) {
    return null;
  }
  return base.slice(0, -'Dtos'.length);
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
 * 列出文件中所有 Get*OptionsAsync 方法名（含 GetXxxTreeOptionsAsync）
 * @param {string} content
 * @returns {string[]}
 */
function listGetOptionsAsyncMethodNames(content) {
  const names = new Set();
  const re = /\b(Get\w+OptionsAsync)\s*\(/g;
  let match;
  while ((match = re.exec(content)) !== null) {
    names.add(match[1]);
  }
  return [...names];
}

/**
 * 从方法签名位置向前提取紧邻的 XML 文档注释
 * @param {string} content
 * @param {number} signatureIndex
 */
function extractLeadingXmlDocBeforeSignature(content, signatureIndex) {
  const before = content.slice(0, signatureIndex);
  const lines = before.split('\n');
  const docLines = [];
  for (let i = lines.length - 1; i >= 0; i -= 1) {
    const line = lines[i];
    const trimmed = line.trim();
    if (trimmed === '') {
      continue;
    }
    if (/^\/\/\//.test(trimmed)) {
      docLines.unshift(line);
      continue;
    }
    break;
  }
  return docLines.length > 0 ? `${docLines.join('\n')}\n` : '';
}

/**
 * 按大括号深度截取方法体（含首尾花括号）
 * @param {string} content
 * @param {number} openBraceIndex
 */
function sliceBalancedBraceBlock(content, openBraceIndex) {
  let depth = 0;
  for (let i = openBraceIndex; i < content.length; i += 1) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
      if (depth === 0) {
        return content.slice(openBraceIndex, i + 1);
      }
    }
  }
  return null;
}

/**
 * 提取单个 Get*OptionsAsync 方法块（含 XML 注释）
 * @param {string} content
 * @param {string} methodName
 * @param {'interface'|'implementation'} variant
 */
function extractGetOptionsAsyncMethodBlock(content, methodName, variant) {
  const anchorRe = new RegExp(`\\b${methodName}\\s*\\(`);
  const anchorMatch = anchorRe.exec(content);
  if (!anchorMatch) {
    return null;
  }

  const anchorIndex = anchorMatch.index;
  const lineStart = content.lastIndexOf('\n', anchorIndex) + 1;
  const lineEnd = content.indexOf('\n', anchorIndex);
  const line = content.slice(lineStart, lineEnd < 0 ? content.length : lineEnd);

  if (variant === 'interface') {
    if (!/\bTask\s*</.test(line) || !line.includes(';')) {
      return null;
    }
    const semi = content.indexOf(';', anchorIndex);
    if (semi < 0) {
      return null;
    }
    let sigStart = lineStart;
    const prevLineEnd = content.lastIndexOf('\n', lineStart - 2);
    const prevLine = content.slice(prevLineEnd + 1, lineStart - 1);
    if (/\bTask\s*</.test(prevLine) && !/^\s*\/\/\//.test(line.trim())) {
      sigStart = prevLineEnd + 1;
    }
    const doc = extractLeadingXmlDocBeforeSignature(content, sigStart);
    let block = doc + content.slice(sigStart, semi + 1);
    if (!block.endsWith('\n\n')) {
      block += block.endsWith('\n') ? '\n' : '\n\n';
    }
    return block;
  }

  if (!/^\s*public\b/.test(line) || !/\bTask\s*</.test(line)) {
    return null;
  }
  const doc = extractLeadingXmlDocBeforeSignature(content, lineStart);
  const openBrace = content.indexOf('{', anchorIndex);
  if (openBrace < 0) {
    return null;
  }
  const body = sliceBalancedBraceBlock(content, openBrace);
  if (!body) {
    return null;
  }
  let block = doc + content.slice(lineStart, openBrace) + body;
  if (!block.endsWith('\n\n')) {
    block += block.endsWith('\n') ? '\n' : '\n\n';
  }
  return block;
}

/**
 * 目标文件中是否已声明指定 Get*OptionsAsync 方法
 * @param {string|null|undefined} content
 * @param {string} methodName
 */
function hasGetOptionsAsyncMethod(content, methodName) {
  if (!content) {
    return false;
  }
  return new RegExp(`\\b${methodName}\\s*\\(`).test(content);
}

/**
 * Get*OptionsAsync / Get*TreeOptionsAsync 实现是否通过仓储查询（拒绝遗留的 GetTenantXxxListAsync、全量递归 Build*Tree）
 * @param {string|null} block 方法块全文
 * @param {string} repoField 如 _menuRepository
 * @param {{ statusField?: string, enabledValue?: number }|null} [statusMeta]
 * @param {Set<string>|null} [entityPropNames] 实体属性名；块内引用不存在的属性则视为失效并重生成
 * @param {Array<{ name: string, bareType: string }>|null} [entityScalarProps] 实体标量属性（含类型）；用于检出 string Status 上残留的 == 1
 * @param {string|null} [dtoBase] TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase（隔离以三基类为准）
 * @returns {boolean}
 */
/**
 * Options 方法是否已含标准 plantCode + keyword 可选参数
 * @param {string} block
 * @returns {boolean}
 */
function optionsBlockHasStandardPlantKeywordSignature(block) {
  if (!block || !block.trim()) {
    return false;
  }
  return (
    /string\?\s+plantCode\s*=\s*null/.test(block) && /string\?\s+keyword\s*=\s*null/.test(block)
  );
}

/**
 * Options XML 是否已含 plantCode / keyword 的 param 标记（避免 CS1573）
 * @param {string} block
 * @returns {boolean}
 */
function optionsBlockHasPlantKeywordXmlParams(block) {
  if (!block || !block.trim()) {
    return false;
  }
  return (
    /\/\/\/\s*<param\s+name="plantCode"\s*>/.test(block)
    && /\/\/\/\s*<param\s+name="keyword"\s*>/.test(block)
  );
}

function isValidOptionsImplementationBlock(
  block,
  repoField,
  statusMeta = null,
  entityPropNames = null,
  entityScalarProps = null,
  dtoBase = null,
) {
  if (!block || !block.trim()) {
    return false;
  }
  if (!block.includes(`${repoField}.GetListAsync`)) {
    return false;
  }
  if (/\bawait\s+Get(?:Tenant|Company)?\w+ListAsync\s*\(/.test(block)) {
    return false;
  }
  // 大数据树：拒绝内存递归 Build*Tree / Build*TreeOptions（须按 parentId 只查一层）
  if (/\bBuild\w+Tree(?:Options)?\s*\(/.test(block)) {
    return false;
  }
  // 标准签名：必须含 plantCode + keyword（与工单 Options 对齐）
  if (!optionsBlockHasStandardPlantKeywordSignature(block)) {
    return false;
  }
  // XML：签名有 plantCode/keyword 时须有对应 <param>（避免 CS1573）
  if (!optionsBlockHasPlantKeywordXmlParams(block)) {
    return false;
  }
  // 三基类隔离：Options 谓词必须与 Tenant / Company / Approval 一致
  if (dtoBase && !optionsBlockMatchesIsolationScope(block, dtoBase)) {
    return false;
  }
  if (optionsBlockUsesStaleIntStatusCompare(block, statusMeta, entityScalarProps)) {
    return false;
  }
  if (optionsBlockReferencesMissingEntityProps(block, entityPropNames, dtoBase)) {
    return false;
  }
  // 平铺 Options：禁止 DictValue/DictLabel 使用雪花 Id（须业务 *Code）
  if (optionsBlockUsesSnowflakeIdForSelect(block)) {
    return false;
  }
  return true;
}

/**
 * Options 块隔离谓词是否与三基类一致（双向校验，禁止胡来）
 * - TaktTenantDtoBase：禁止 CompanyCode / EnsureThreeLayerContext
 * - TaktCompanyDtoBase / TaktApprovalDtoBase：必须含 CompanyCode == CurrentCompanyCode
 * @param {string} block
 * @param {string} dtoBase
 * @returns {boolean} true=匹配隔离级别
 */
function optionsBlockMatchesIsolationScope(block, dtoBase) {
  if (!block || !dtoBase) {
    return true;
  }
  const hasCompanyPred = /\.CompanyCode\s*==\s*CurrentCompanyCode/.test(block);
  const hasEnsureThree = /\bEnsureThreeLayerContext\s*\(/.test(block);
  if (!dtoBaseHasCompanyIsolation(dtoBase)) {
    // 租户级：无公司列
    if (hasCompanyPred || /\.CompanyCode\b/.test(block) || hasEnsureThree) {
      return false;
    }
    return true;
  }
  // 公司级 / 审批级：必须过滤 CompanyCode（缺则跨公司串数据，强制重生成）
  if (!hasCompanyPred) {
    return false;
  }
  return true;
}

/**
 * 平铺 Get*OptionsAsync 是否仍用 e.Id / item.Id 作 DictValue 或 DictLabel
 * @param {string} block
 */
function optionsBlockUsesSnowflakeIdForSelect(block) {
  if (!block || !block.trim()) {
    return false;
  }
  // 树形 TreeOptions 的 DictValue=item.Id 仍服务于 ParentId，此处仅拦平铺 TaktSelectOption
  if (/\bTaktTreeSelectOption\b/.test(block)) {
    return false;
  }
  if (/DictValue\s*=\s*(?:e|item)\.Id\b/.test(block)) {
    return true;
  }
  if (/DictLabel\s*=\s*(?:e|item)\.Id(?:\.ToString\s*\(\s*\))?/.test(block)) {
    return true;
  }
  if (/DictLabel\s*=\s*.*\?\?\s*(?:e|item)\.Id(?:\.ToString\s*\(\s*\))?/.test(block)) {
    return true;
  }
  return false;
}

/**
 * Options 方法块是否引用了实体上已删除/不存在的属性（如旧 MaterialName）
 * @param {string} block
 * @param {Set<string>|null|undefined} entityPropNames
 * @param {string|null} [dtoBase] 仅公司/审批基类把 CompanyCode 列入白名单
 * @returns {boolean} true=存在缺失引用，应重生成
 */
function optionsBlockReferencesMissingEntityProps(block, entityPropNames, dtoBase = null) {
  if (!block || !entityPropNames || entityPropNames.size === 0) {
    return false;
  }
  const allowed = new Set([
    ...entityPropNames,
    'Id',
    'TenantCode',
    'CultureCode',
    'PlantCode',
    'RelatedPlant',
    'CreatedAt',
    'CreatedBy',
    'UpdatedAt',
    'UpdatedBy',
    'DeletedAt',
    'DeletedBy',
    'IsDeleted',
    'Remark',
    'ExtField',
    'SortOrder',
    'ApprovalStatus',
    'FlowInstanceId',
  ]);
  // CompanyCode 仅 TaktCompany* / TaktApproval*；禁止默认白名单导致租户级 Options 误保留
  if (dtoBaseHasCompanyIsolation(dtoBase)) {
    allowed.add('CompanyCode');
  }
  const re = /\b(?:e|x|item)\.([A-Z]\w*)\b/g;
  let m;
  while ((m = re.exec(block)) !== null) {
    if (!allowed.has(m[1])) {
      return true;
    }
  }
  return false;
}

/**
 * 解析 Options 方法实现：有效则保留，否则输出模板
 * @param {object} params
 * @param {string|null|undefined} params.existingContent
 * @param {string} params.methodName
 * @param {string} params.repoField
 * @param {string} params.freshTemplate
 * @param {boolean} [params.refreshOptions]
 * @param {object|null} [params.statusMeta]
 * @param {Set<string>|null} [params.entityPropNames]
 * @param {Array<{ name: string, bareType: string }>|null} [params.entityScalarProps]
 * @param {string|null} [params.dtoBase]
 * @returns {{ block: string, preserved: boolean, regenerated: boolean }}
 */
function resolveOptionsImplementationBlock({
  existingContent,
  methodName,
  repoField,
  freshTemplate,
  refreshOptions = false,
  statusMeta = null,
  entityPropNames = null,
  entityScalarProps = null,
  dtoBase = null,
}) {
  if (!hasGetOptionsAsyncMethod(existingContent, methodName)) {
    return { block: freshTemplate, preserved: false, regenerated: false };
  }
  if (refreshOptions) {
    return { block: freshTemplate, preserved: false, regenerated: true };
  }
  const preserved = extractGetOptionsAsyncMethodBlock(
    existingContent,
    methodName,
    'implementation',
  );
  if (
    preserved
    && isValidOptionsImplementationBlock(
      preserved,
      repoField,
      statusMeta,
      entityPropNames,
      entityScalarProps,
      dtoBase,
    )
  ) {
    return { block: preserved, preserved: true, regenerated: false };
  }
  return { block: freshTemplate, preserved: false, regenerated: true };
}

/**
 * 生成阶段：已有则原样拷贝，没有才输出模板（接口）
 * @returns {{ block: string, preserved: boolean, methodName: string }}
 */
/**
 * 从实体源码解析 Options 工厂过滤列（PlantCode 优先，否则 RelatedPlant）
 * @param {string} entityContent
 * @returns {string|null}
 */
function resolveOptionsPlantField(entityContent) {
  if (!entityContent) {
    return null;
  }
  if (/public\s+string\??\s+PlantCode\s*\{/.test(entityContent) || /nameof\(\s*PlantCode\s*\)/.test(entityContent)) {
    return 'PlantCode';
  }
  if (
    /public\s+string\??\s+RelatedPlant\s*\{/.test(entityContent)
    || /nameof\(\s*RelatedPlant\s*\)/.test(entityContent)
  ) {
    return 'RelatedPlant';
  }
  return null;
}

function buildGetOptionsAsyncInterfaceSection(entityShort, hasTree, dtoInfo, desc, existingContent) {
  const methodName =
    hasTree && dtoInfo.tree ? `Get${entityShort}TreeOptionsAsync` : `Get${entityShort}OptionsAsync`;
  if (hasGetOptionsAsyncMethod(existingContent, methodName)) {
    const preserved = extractGetOptionsAsyncMethodBlock(existingContent, methodName, 'interface');
    // 树形 TreeOptions：旧签名无 parentId 时强制换成懒加载一层接口
    // 平铺/树形：无 plantCode+keyword 时强制换成标准签名
    if (preserved) {
      const isLazyTreeOptions =
        !(hasTree && dtoInfo.tree) || /\blong\s+parentId\b/.test(preserved);
      const hasPlantKeyword = optionsBlockHasStandardPlantKeywordSignature(preserved);
      const hasPlantKeywordXml = optionsBlockHasPlantKeywordXmlParams(preserved);
      if (isLazyTreeOptions && hasPlantKeyword && hasPlantKeywordXml) {
        return { block: preserved, preserved: true, methodName };
      }
    }
  }
  let block = '';
  if (hasTree && dtoInfo.tree) {
    block += buildMethodXmlDoc({
      summary: `获取${desc}树形选项列表（懒加载：仅 parentId 直接子级一层）`,
      params: [
        { name: 'parentId', desc: '父级ID（0=根）' },
        { name: 'plantCode', desc: '工厂代码（可选，用于按工厂过滤）' },
        { name: 'keyword', desc: '搜索关键字（可选，模糊匹配）' },
      ],
      returns: '树形选项（一层）',
    });
    block += `    Task<List<TaktTreeSelectOption>> ${methodName}(long parentId = 0, string? plantCode = null, string? keyword = null);\n\n`;
  } else {
    block += buildMethodXmlDoc({
      summary: `获取${desc}选项列表`,
      params: [
        { name: 'plantCode', desc: '工厂代码（可选，用于按工厂过滤）' },
        { name: 'keyword', desc: '搜索关键字（可选，模糊匹配）' },
      ],
      returns: '下拉选项',
    });
    block += `    Task<List<TaktSelectOption>> ${methodName}(string? plantCode = null, string? keyword = null);\n\n`;
  }
  return { block, preserved: false, methodName };
}

/**
 * 非树形实体：GetXxxOptionsAsync 默认实现模板（DictValue/DictLabel 均禁止雪花 Id）
 * @param {string} entityName 实体类名（Expressionable&lt;T&gt;）
 * @param {string} nameField 展示字段（Name / Code / nvarchar / int）
 * @param {string} valueField 业务 Code，无则 Name，再无则首个业务 nvarchar/int
 * @param {boolean} [valueAsString] int 字段须 ToString 作为 DictValue/排序键
 * @param {string|null} [plantField] PlantCode / RelatedPlant；无则不按工厂过滤
 */
function buildFlatOptionsAsyncImplTemplate(
  entityShort,
  desc,
  repoField,
  ensureContextLine,
  optionsListPredicate,
  nameField,
  valueField,
  valueAsString = false,
  entityName = null,
  plantField = null,
) {
  if (!valueField || valueField === 'Id') {
    throw new Error(
      `Get${entityShort}OptionsAsync：valueField 须为 *Code / *Name / 业务 nvarchar/int，禁止雪花 Id`,
    );
  }
  const entityType = entityName || `Takt${entityShort}`;
  const labelField = nameField && nameField !== 'Id' ? nameField : valueField;
  const orderExpr = valueAsString
    ? `x => x.${labelField}.ToString()`
    : `x => x.${labelField} ?? string.Empty`;
  const valueExpr = valueAsString ? `e.${valueField}.ToString()` : `e.${valueField}`;
  const labelExpr =
    labelField === valueField
      ? valueExpr
      : valueAsString
        ? `e.${labelField}.ToString()`
        : `e.${labelField} ?? e.${valueField}`;
  let keywordPred;
  if (valueAsString) {
    keywordPred = `x.${valueField}.ToString().Contains(normalizedKeyword!)`;
    if (labelField !== valueField) {
      keywordPred += `\n                || x.${labelField}.ToString().Contains(normalizedKeyword!)`;
    }
  } else {
    keywordPred = `(x.${valueField} != null && x.${valueField}.Contains(normalizedKeyword!))`;
    if (labelField !== valueField) {
      keywordPred += `\n                || (x.${labelField} != null && x.${labelField}.Contains(normalizedKeyword!))`;
    }
  }
  let block = '';
  block += buildMethodXmlDoc({
    summary: `获取${desc}选项列表`,
    params: [
      { name: 'plantCode', desc: '工厂代码（可选，用于按工厂过滤）' },
      { name: 'keyword', desc: '搜索关键字（可选，模糊匹配）' },
    ],
    returns: '下拉选项',
  });
  block += `    public async Task<List<TaktSelectOption>> Get${entityShort}OptionsAsync(string? plantCode = null, string? keyword = null)\n`;
  block += '    {\n';
  block += ensureContextLine;
  block += '        var normalizedPlantCode = plantCode?.Trim();\n';
  block += '        var normalizedKeyword = keyword?.Trim();\n';
  block += `        var predicate = Expressionable.Create<${entityType}>()\n`;
  block += `            .And(${optionsListPredicate})\n`;
  if (plantField) {
    block += `            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.${plantField} == normalizedPlantCode)\n`;
  }
  block += `            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>\n`;
  block += `                ${keywordPred})\n`;
  block += '            .ToExpression();\n';
  block += `        var list = await ${repoField}.GetListAsync(\n`;
  block += '            predicate,\n';
  block += `            ${orderExpr},\n`;
  block += '            false);\n';
  block += '        return list.Select(e => new TaktSelectOption\n';
  block += '        {\n';
  block += `            DictValue = ${valueExpr},\n`;
  block += `            DictLabel = ${labelExpr},\n`;
  block += '        }).ToList();\n';
  block += '    }\n\n';
  return block;
}

/**
 * 从 *Dtos.cs 提取 DTO 元信息（标准：TaktXxxCreateDto / TaktXxxUpdateDto；兼容旧版 TaktCreateXxxDto）
 * @param {string} dtoFile
 */
function extractDtoInfo(dtoFile) {
  const content = readUtf8(dtoFile);
  const entityName = entityNameFromDtoFile(dtoFile);
  const dtos = {
    entityName,
    base: null,
    query: null,
    create: null,
    update: null,
    statuses: [],
    sort: null,
    builtIn: null,
    obsolete: null,
    tree: null,
    template: null,
    import: null,
    export: null,
    transposedQuery: null,
    transposedResult: null,
    transposedBatch: null,
  };

  const classRegex = /public\s+(?:partial\s+)?class\s+(\w+)\s*(?::|\{)/g;
  let match;
  while ((match = classRegex.exec(content)) !== null) {
    const className = match[1];
    if (!className.includes('Dto')) {
      continue;
    }

    if (className.endsWith('TransposedQueryDto')) {
      dtos.transposedQuery = className;
    } else if (className.endsWith('TransposedResultDto')) {
      dtos.transposedResult = className;
    } else if (className.endsWith('TransposedBatchDto')) {
      dtos.transposedBatch = className;
    } else if (className.endsWith('QueryDto')) {
      dtos.query = className;
    } else if (/^Takt\w+CreateDto$/.test(className)) {
      dtos.create = className;
    } else if (/^TaktCreate\w+Dto$/.test(className)) {
      dtos.create = className;
    } else if (/^Takt\w+UpdateDto$/.test(className)) {
      dtos.update = className;
    } else if (/^TaktUpdate\w+Dto$/.test(className)) {
      dtos.update = className;
    } else if (className.endsWith('StatusDto')) {
      dtos.statuses.push(className);
    } else if (className.endsWith('SortDto')) {
      dtos.sort = className;
    } else if (entityName && className === `${entityName}BuiltInDto`) {
      dtos.builtIn = className;
    } else if (entityName && className === `${entityName}ObsoleteDto`) {
      dtos.obsolete = className;
    } else if (className.endsWith('TreeDto')) {
      dtos.tree = className;
    } else if (className.endsWith('TemplateDto')) {
      dtos.template = className;
    } else if (className.endsWith('ImportDto')) {
      dtos.import = className;
    } else if (className.endsWith('ExportDto')) {
      dtos.export = className;
    } else if (entityName && className === `${entityName}Dto`) {
      dtos.base = className;
    }
  }

  return dtos;
}

function isAggregatable(dtoInfo) {
  return Boolean(dtoInfo.base && dtoInfo.query && dtoInfo.create && dtoInfo.update && dtoInfo.entityName);
}

function findEntityFile(entityName) {
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        const found = searchDir(fullPath);
        if (found) {
          return found;
        }
      } else if (entry.name === `${entityName}.cs`) {
        return fullPath;
      }
    }
    return null;
  }
  return searchDir(CONFIG.entitiesRoot);
}

function parseEntityBase(entityFile) {
  return parseEntityBaseFromCsFile(entityFile);
}

/**
 * 从主响应 DTO 类声明解析继承的 DtoBase（TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase）
 * @param {string} dtoFile
 * @param {{ base: string|null }} dtoInfo
 * @returns {string|null}
 */
function parseDtoBase(dtoFile, dtoInfo) {
  if (!dtoInfo.base) {
    return null;
  }
  const content = readUtf8(dtoFile);
  const declRegex = new RegExp(`public\\s+class\\s+${dtoInfo.base}\\s*:\\s*(\\w+)`);
  const match = content.match(declRegex);
  if (!match) {
    return null;
  }
  const base = match[1];
  return DTO_BASE_NAMES.includes(base) ? base : null;
}

/**
 * QueryExpression / Options 等 lambda 的数据隔离前缀（三基类）
 * - Tenant：仅 TenantCode
 * - Company / Approval：TenantCode + CompanyCode
 * @param {string} dtoBase
 * @param {string} varName 实体参数名（如 holiday）
 * @returns {string[]}
 */
function buildIsolationFilterLines(dtoBase, varName) {
  if (!dtoBaseHasCompanyIsolation(dtoBase)) {
    return [`        return ${varName} => ${varName}.TenantCode == CurrentTenantCode`];
  }
  return [
    `        return ${varName} => ${varName}.TenantCode == CurrentTenantCode`,
    `                    && ${varName}.CompanyCode == CurrentCompanyCode`,
  ];
}

/**
 * GetById 等详情校验：租户/公司不匹配则视为不存在（三基类）
 * @param {string} dtoBase
 */
function buildEntityScopeGuard(dtoBase) {
  if (!dtoBaseHasCompanyIsolation(dtoBase)) {
    return 'entity.TenantCode != CurrentTenantCode';
  }
  return 'entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode';
}

/**
 * 子表明细作废删除（标记 IsObsolete=1，非软删）
 * @param {string} repoField
 * @param {string} desc
 * @param {string} entityScopeGuard
 * @param {string} [builtInGuardLines]
 * @returns {string}
 */
function buildObsoleteMarkDeleteBody(repoField, desc, entityScopeGuard, builtInGuardLines = '') {
  return `        var entity = await ${repoField}.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("${desc}不存在或已删除");
        }
        if (${entityScopeGuard})
        {
            throw new TaktBusinessException("${desc}不存在或已删除");
        }
${builtInGuardLines}        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("${desc}已作废");
        }
        entity.IsObsolete = 1;
        await ${repoField}.UpdateAsync(entity);
`;
}

/**
 * Options 列表查询 predicate（三基类隔离 + 可选启用态）
 * @param {string} dtoBase TaktTenantDtoBase | TaktCompanyDtoBase | TaktApprovalDtoBase
 * @param {{ field?: string, kind?: string, enabledLiteral?: string, intEnabled?: number }|null} [statusMeta]
 */
function buildOptionsListPredicate(dtoBase, statusMeta = null, hasIsObsolete = false) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const obsoletePart = hasIsObsolete ? ' && x.IsObsolete == 0' : '';
  if (statusMeta?.kind === 'int') {
    const enabled = statusMeta.intEnabled ?? 1;
    const statusCmp = statusMeta.isEnum
      ? `(int)x.${statusMeta.field} == ${enabled}`
      : `x.${statusMeta.field} == ${enabled}`;
    return `x => ${scope} && ${statusCmp}${obsoletePart}`;
  }
  return `x => ${scope}${obsoletePart}`;
}

/**
 * 写入选项方法前的上下文校验（仅公司/审批级需要三层上下文）
 * @param {string} dtoBase
 */
function buildEnsureContextLine(dtoBase) {
  if (!dtoBaseHasCompanyIsolation(dtoBase)) {
    return '';
  }
  return '        EnsureThreeLayerContext();\n';
}

function getEntityNamespace(entityFile) {
  const content = readUtf8(entityFile);
  const nsMatch = content.match(/namespace\s+([\w.]+);/);
  return nsMatch ? nsMatch[1] : 'Takt.Domain.Entities';
}

function getModuleRelativePath(entityFile) {
  const relativePath = path.relative(CONFIG.entitiesRoot, entityFile);
  const parts = relativePath.split(path.sep);
  parts.pop();
  return parts;
}

function buildNamespace(prefix, moduleParts) {
  let ns = prefix;
  for (const part of moduleParts) {
    ns += `.${part}`;
  }
  return ns;
}

function extractEntityDescription(entityFile) {
  if (!entityFile) {
    return null;
  }
  const content = readUtf8(entityFile);
  const sugarTableMatch = content.match(/SugarTable\([^,]*,\s*"([^"]+)"/);
  if (sugarTableMatch) {
    return sugarTableMatch[1].replace(/表$/, '');
  }
  const xmlMatch = content.match(/\/\/\/\s*<summary>\s*\n\s*\/\/\/\s*(.+?)\s*\n\s*\/\/\/\s*<\/summary>/s);
  if (xmlMatch) {
    return xmlMatch[1].trim().replace(/实体$/, '');
  }
  return null;
}

/**
 * 实体是否含 IsObsolete（子表明细作废，编辑移除行时标记）
 * @param {string|null|undefined} entityFile
 * @returns {boolean}
 */
function entityFileHasIsObsolete(entityFile) {
  if (!entityFile) {
    return false;
  }
  return /\bpublic int IsObsolete\s*\{/.test(readUtf8(entityFile));
}

/**
 * 实体是否含「前端表单 TaktNumbering 自动编码」字段；返回业务编码属性名
 * @param {string|null|undefined} entityFile
 * @returns {string|null}
 */
function findTaktNumberingAutoFormCodeProperty(entityFile) {
  if (!entityFile) {
    return null;
  }
  const props = parseEntityScalarProperties(readUtf8(entityFile));
  const hit = props.find((p) => p.bareType === 'string' && isTaktNumberingAutoFormFieldSummary(p.summary));
  return hit ? hit.name : null;
}

function identifyCrudType(entityFile) {
  if (!entityFile) {
    return 'Single';
  }
  const cascadingChildren = parseOneToManyNavigations(entityFile).filter(
    (nav) => !isRbacJunctionEntity(nav.childShort) && !isStandaloneChildVueEntity(nav.childShort),
  );
  if (cascadingChildren.length > 0) {
    return 'MasterDetail';
  }
  const content = readUtf8(entityFile);
  if (/public\s+\w+\??\s+ParentId\s*\{/.test(content)) {
    return 'Tree';
  }
  return 'Single';
}

function parseNavigateForeignKeyFromSegment(segment) {
  const matches = [...segment.matchAll(/nameof\(\w+\.(\w+)\)/g)];
  if (!matches.length) {
    return null;
  }
  return matches[matches.length - 1][1];
}

function isEntityLongPropertyNullable(entityFile, propName) {
  const content = readUtf8(entityFile);
  return new RegExp(`public\\s+long\\?\\s+${propName}\\s*\\{`).test(content);
}

/**
 * DTO 类（含基类）上是否为可空 long 外键
 * @param {string} dtoContent
 * @param {string} className
 * @param {string} propName
 * @returns {boolean}
 */
function isDtoClassLongPropertyNullable(dtoContent, className, propName) {
  if (!dtoContent || !className) {
    return false;
  }
  const block = extractClassBlock(dtoContent, className);
  if (block && new RegExp(`public\\s+long\\?\\s+${propName}\\s*\\{`).test(block)) {
    return true;
  }
  const startRegex = new RegExp(
    `public\\s+(?:partial\\s+)?class\\s+${className}\\b[^\\{]*(?::\\s*(\\w+))?`,
  );
  const startMatch = startRegex.exec(dtoContent);
  if (startMatch?.[1]) {
    return isDtoClassLongPropertyNullable(dtoContent, startMatch[1], propName);
  }
  return false;
}

/**
 * ManyToOne Stamp 方法参数为 CreateDto，仅依据 CreateDto 判定外键是否 long?
 * @param {string} dtoContent
 * @param {object} dtoInfo
 * @param {string} propName
 * @returns {boolean}
 */
function isStampLongIdFieldNullable(dtoContent, dtoInfo, propName) {
  if (!dtoInfo?.create) {
    return false;
  }
  return isDtoClassLongPropertyNullable(dtoContent, dtoInfo.create, propName);
}

function wrapMasterIdExpr(entityFile, entityVar, fieldName) {
  const expr = `${entityVar}.${fieldName}`;
  if (isEntityLongPropertyNullable(entityFile, fieldName)) {
    return `${expr}.GetValueOrDefault()`;
  }
  return expr;
}

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

function parseOneToManyNavigations(entityFile) {
  const content = readUtf8(entityFile);
  const classMatch = content.match(/public\s+class\s+(Takt\w+)\s*:\s*\w+/);
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
  const navigations = [];
  let match;
  while ((match = navRegex.exec(body)) !== null) {
    const foreignKeyOnChild = match[1];
    const childEntity = match[3];
    const navPropName = match[4];
    const childShort = childEntity.replace(/^Takt/, '');
    navigations.push({
      navPropName,
      childEntity,
      childShort,
      foreignKeyOnChild,
    });
  }
  return navigations;
}

/**
 * 子表从主表回填的字段映射（隔离列 + 同名冗余码/名 + FK 前缀冗余）
 * 全库主子表通用（不限人事）：同名字符串属性 + `{FkPrefix}Code/Name` ← `{MasterShort}Code/Name`
 * 隔离列按子表实体基类写入（禁止给 TenantCulture 子表 Stamp CompanyCode/PlantCode）
 * @param {string} childEntityFile
 * @param {string} foreignKeyOnChild 如 DictTypeId / OriginalEmployeeId
 * @param {string|null} masterIdField
 * @param {string|null} masterEntityFile
 * @param {string|null} masterShort 如 DictType / Employee
 * @returns {Array<{ childField: string, masterField: string }>}
 */
function getChildStampFieldMappings(
  childEntityFile,
  foreignKeyOnChild,
  masterIdField,
  masterEntityFile = null,
  masterShort = null,
) {
  const childProps = extractEntityPropertyNames(childEntityFile);
  const childStringProps = extractEntityStringPropertyNames(childEntityFile);
  const childBase = parseEntityBase(childEntityFile);
  const masterProps = masterEntityFile
    ? extractEntityPropertyNames(masterEntityFile)
    : new Set();
  const masterStringProps = masterEntityFile
    ? extractEntityStringPropertyNames(masterEntityFile)
    : new Set();
  /** @type {Array<{ childField: string, masterField: string }>} */
  const mappings = [];
  const seenChild = new Set();
  const add = (childField, masterField, requireMasterProp = true, requireChildProp = true) => {
    if (!childField || !masterField || seenChild.has(childField)) {
      return;
    }
    if (requireChildProp && !childProps.has(childField) && !childStringProps.has(childField)) {
      // 基类列常不在 extract 结果中：隔离列用 requireChildProp=false
      if (requireChildProp) {
        return;
      }
    }
    if (requireChildProp && !childProps.has(childField) && !['TenantCode', 'CompanyCode', 'CultureCode', 'PlantCode'].includes(childField)) {
      return;
    }
    if (requireMasterProp && masterProps.size > 0 && !masterProps.has(masterField) && !masterStringProps.has(masterField)) {
      // 主表基类隔离列同样可能扫不到
      if (!['TenantCode', 'CompanyCode', 'CultureCode', 'PlantCode'].includes(masterField)) {
        return;
      }
    }
    if (childField === foreignKeyOnChild || childField === masterIdField) {
      return;
    }
    seenChild.add(childField);
    mappings.push({ childField, masterField });
  };
  // 隔离列：主子表基类交集（例：DictData 有 CultureCode，DictType 主表无 → 禁止 master.CultureCode）
  const masterBase = masterEntityFile ? parseEntityBase(masterEntityFile) : childBase;
  const childIsolation = new Set(getIsolationStampFieldNamesForEntityBase(childBase));
  const masterIsolation = new Set(getIsolationStampFieldNamesForEntityBase(masterBase));
  for (const field of childIsolation) {
    if (masterIsolation.has(field)) {
      add(field, field, false, false);
    }
  }
  const skipSame = new Set(
    [
      'Id',
      'TenantCode',
      'CompanyCode',
      'CultureCode',
      'PlantCode',
      'RelatedPlant',
      'Remark',
      'ExtField',
      'IsDeleted',
      'CreatedAt',
      'UpdatedAt',
      'CreatedBy',
      'UpdatedBy',
      foreignKeyOnChild,
      masterIdField,
    ].filter(Boolean),
  );
  for (const propName of childProps) {
    if (skipSame.has(propName) || /Id$/i.test(propName)) {
      continue;
    }
    // 仅字符串同名冗余：decimal/int/DateTime 不得走 string.IsNullOrEmpty Stamp
    if (!childStringProps.has(propName)) {
      continue;
    }
    if (masterProps.has(propName) || masterStringProps.has(propName)) {
      add(propName, propName);
    }
  }
  // OriginalEmployeeId → OriginalEmployeeCode/Name ← Employee.EmployeeCode/Name
  if (foreignKeyOnChild && /Id$/i.test(foreignKeyOnChild) && masterShort) {
    const prefix = foreignKeyOnChild.replace(/Id$/i, '');
    const masterCode = masterProps.has(`${masterShort}Code`) || masterStringProps.has(`${masterShort}Code`)
      ? `${masterShort}Code`
      : null;
    const masterName = masterProps.has(`${masterShort}Name`) || masterStringProps.has(`${masterShort}Name`)
      ? `${masterShort}Name`
      : null;
    if (masterCode) {
      add(`${prefix}Code`, masterCode);
    }
    if (masterName) {
      add(`${prefix}Name`, masterName);
    }
  }
  return mappings;
}

/**
 * 同名回填字段列表（OneToMany SaveChildren：childDto.X = entity.X）
 * @returns {string[]}
 */
function getChildStampFields(childEntityFile, foreignKeyOnChild, masterIdField, masterEntityFile, masterShort) {
  return getChildStampFieldMappings(
    childEntityFile,
    foreignKeyOnChild,
    masterIdField,
    masterEntityFile,
    masterShort,
  )
    .filter((m) => m.childField === m.masterField)
    .map((m) => m.childField);
}

/** 子表上的主表 Id 外键字段名（如 DictData.DictTypeId、SalesOrderItem.SalesOrderId） */
function resolveChildMasterIdField(childEntityFile, foreignKeyOnChild, entityShort) {
  const props = extractEntityPropertyNames(childEntityFile);
  if (foreignKeyOnChild?.endsWith('Code')) {
    const derived = foreignKeyOnChild.replace(/Code$/, 'Id');
    if (props.has(derived)) {
      return derived;
    }
  }
  if (foreignKeyOnChild && props.has(foreignKeyOnChild)) {
    return foreignKeyOnChild;
  }
  const byMasterShort = `${entityShort}Id`;
  if (props.has(byMasterShort)) {
    return byMasterShort;
  }
  return foreignKeyOnChild || null;
}

function buildChildLinkPredicate(childVar, entityVar, foreignKeyOnChild, childFkField) {
  const fk = childFkField || foreignKeyOnChild;
  if (!fk) {
    return `${childVar}.Id == ${entityVar}.Id`;
  }
  if (fk.endsWith('Code')) {
    return `${childVar}.${fk} == ${entityVar}.${fk}`;
  }
  return `${childVar}.${fk} == ${entityVar}.Id`;
}

/**
 * 子表保存时写入主表外键（*Id → entity.Id；*Code → entity.*Code）
 * @param {string} childVar
 * @param {string} entityVar
 * @param {string} fkField
 */
function buildChildForeignKeyAssignment(childVar, entityVar, fkField) {
  if (!fkField) {
    return null;
  }
  if (fkField.endsWith('Code')) {
    return `${childVar}.${fkField} = ${entityVar}.${fkField};`;
  }
  return `${childVar}.${fkField} = ${entityVar}.Id;`;
}

function parseManyToOneMasterRecord(entityFile, fkField, masterEntity) {
  const masterShort = masterEntity.replace(/^Takt/, '');
  const masterFile = findEntityFile(masterEntity);
  if (!masterFile) {
    return null;
  }
  const masterBase = parseEntityBase(masterFile);
  const masterRepoInterface = ENTITY_BASE_TO_REPOSITORY[masterBase];
  if (!masterRepoInterface) {
    return null;
  }
  const masterIdField = resolveChildMasterIdField(entityFile, fkField, masterShort);
  if (!masterIdField) {
    return null;
  }
  return {
    fkField,
    masterEntity,
    masterShort,
    masterIdField,
    masterRepoInterface,
    masterRepoField: repositoryFieldName(masterShort),
  };
}

/**
 * 解析实体上全部 ManyToOne 导航（按声明顺序）
 * @param {string} entityFile
 * @returns {Array<object>}
 */
function parseManyToOneMasters(entityFile) {
  const content = readUtf8(entityFile);
  const classMatch = content.match(/public\s+class\s+(Takt\w+)\s*:\s*\w+/);
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
    /\[Navigate\(\s*NavigateType\.ManyToOne\s*,\s*nameof\((\w+)\)\s*\)\][\s\S]*?public\s+(Takt\w+)\??\s+\w+\s*\{/g;
  const masters = [];
  const seenFk = new Set();
  let navMatch;
  while ((navMatch = navRegex.exec(body)) !== null) {
    const fkField = navMatch[1];
    if (seenFk.has(fkField)) {
      continue;
    }
    seenFk.add(fkField);
    const record = parseManyToOneMasterRecord(entityFile, fkField, navMatch[2]);
    if (record) {
      masters.push(record);
    }
  }
  return masters;
}

/** @deprecated 使用 parseManyToOneMasters；保留首条以兼容旧调用 */
function parseManyToOneMaster(entityFile) {
  const masters = parseManyToOneMasters(entityFile);
  return masters.length ? masters[0] : null;
}

function buildManyToOneStampMethodBody(master, childEntityFile, masterDesc, dtoContent, dtoInfo) {
  const masterEntityFile = findEntityFile(master.masterEntity);
  const stampMappings = getChildStampFieldMappings(
    childEntityFile,
    master.fkField,
    master.masterIdField,
    masterEntityFile,
    master.masterShort,
  );
  const stampSync = stampMappings
    .map(
      (m) => `        if (string.IsNullOrEmpty(entity.${m.childField}))
        {
            entity.${m.childField} = ${formatStampAssignmentRhs('master', m.masterField, masterEntityFile)};
        }`,
    )
    .join('\n');
  const fk = master.fkField;
  const dtoLookupField = master.masterIdField || fk;
  if (fk.endsWith('Id')) {
    const nullableLongId = isStampLongIdFieldNullable(dtoContent, dtoInfo, dtoLookupField);
    const idGuard = nullableLongId
      ? `if (dto.${dtoLookupField} is not > 0)`
      : `if (dto.${dtoLookupField} <= 0)`;
    const idArg = nullableLongId ? `dto.${dtoLookupField}.Value` : `dto.${dtoLookupField}`;
    return `        ${idGuard}
        {
            return;
        }
        var master = await ${master.masterRepoField}.GetByIdAsync(${idArg});
        if (master == null)
        {
            throw new TaktBusinessException("${masterDesc}不存在");
        }
        entity.${fk} = master.Id;
${stampSync ? `${stampSync}\n` : ''}`;
  }
  const idAssign =
    master.masterIdField && master.masterIdField !== fk
      ? `        entity.${master.masterIdField} = master.Id;\n`
      : '';
  return `        if (string.IsNullOrEmpty(dto.${fk}))
        {
            return;
        }
        var master = await ${master.masterRepoField}.FirstAsync(x => x.${fk} == dto.${fk} && x.TenantCode == CurrentTenantCode);
        if (master == null)
        {
            throw new TaktBusinessException("${masterDesc}不存在");
        }
        entity.${fk} = master.${fk};
${idAssign}${stampSync ? `${stampSync}\n` : ''}`;
}

function generateManyToOneMasterStampExtras(entityFile, entityName, entityShort, dtoInfo, desc, dtoContent) {
  const masters = parseManyToOneMasters(entityFile);
  if (!masters.length) {
    return null;
  }

  const seenRepoFields = new Set();
  const stampParts = [];
  const extraUsings = [];
  for (const master of masters) {
    if (seenRepoFields.has(master.masterRepoField)) {
      continue;
    }
    seenRepoFields.add(master.masterRepoField);
    const masterEntityFile = findEntityFile(master.masterEntity);
    const masterDesc = extractEntityDescription(masterEntityFile) || master.masterShort;
    const stampBody = buildManyToOneStampMethodBody(
      master,
      entityFile,
      masterDesc,
      dtoContent,
      dtoInfo,
    );
    const masterEntityNs = masterEntityFile ? getEntityNamespace(masterEntityFile) : null;
    if (masterEntityNs) {
      extraUsings.push(masterEntityNs);
    }
    const repoParam = `${master.masterShort.charAt(0).toLowerCase()}${master.masterShort.slice(1)}Repository`;
    stampParts.push({
      master,
      masterDesc,
      stampBody,
      repoParam,
    });
  }
  if (!stampParts.length) {
    return null;
  }

  return {
    masterEntity: stampParts[0].master.masterEntity,
    masterShort: stampParts[0].master.masterShort,
    masterEntities: stampParts.map((part) => part.master.masterEntity),
    extraUsings,
    ctorFields: stampParts
      .map(
        (part) =>
          `    private readonly ${part.master.masterRepoInterface}<${part.master.masterEntity}> ${part.master.masterRepoField};`,
      )
      .join('\n'),
    ctorParams: stampParts
      .map(
        (part) =>
          `        ${part.master.masterRepoInterface}<${part.master.masterEntity}> ${part.repoParam},`,
      )
      .join('\n'),
    ctorParamDocs: stampParts
      .map(
        (part) =>
          `    /// <param name="${part.repoParam}">${part.masterDesc}仓储</param>`,
      )
      .join('\n'),
    ctorAssigns: stampParts
      .map((part) => `        ${part.master.masterRepoField} = ${part.repoParam};`)
      .join('\n'),
    privateMethods: stampParts
      .map(
        (part) => `${buildMethodXmlDoc({
          summary: `同步${desc}主表外键（ManyToOne → ${part.masterDesc}）`,
          params: [
            { name: 'entity', desc: '当前实体' },
            { name: 'dto', desc: '创建 DTO' },
          ],
          returns: '任务',
        }).trimEnd()}
    private async Task Stamp${entityShort}${part.master.masterShort}Async(${entityName} entity, ${dtoInfo.create} dto)
    {
${part.stampBody}    }
`,
      )
      .join('\n'),
    createBeforeSave: stampParts
      .map((part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, dto);`)
      .join('\n        '),
    updateBeforeSave: stampParts
      .map((part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, dto);`)
      .join('\n        '),
    importBeforeSave: stampParts
      .map(
        (part) => `await Stamp${entityShort}${part.master.masterShort}Async(entity, importDto);`,
      )
      .join('\n                '),
  };
}

/**
 * 主子表 SaveChildren：按子表 Id 增量新增/更新；未提交行标记作废（含 IsObsolete 子表），否则软删
 * @param {object} c 子表元数据
 * @param {string} entityName 主表实体名
 * @param {string} entityShort 主表短名
 * @param {string} entityFile 主表实体文件
 * @param {string} dtoBase 主表 DTO 基类
 * @param {string} childDesc 子表描述
 * @param {{ isSingleChild?: boolean }} [options]
 * @returns {string[]}
 */
function buildMasterDetailChildUpsertBlock(c, entityName, entityShort, entityFile, dtoBase, childDesc, options = {}) {
  const { isSingleChild = false } = options;
  const childIdProp = `${c.childShort}Id`;
  const childUpdateDto = `Takt${c.childShort}UpdateDto`;
  const saveVar = `${c.navPropName.charAt(0).toLowerCase()}${c.navPropName.slice(1)}ForSave`;
  /** 多子表时各块须用不同模式变量名，避免 CS0128（updateDto 重复定义） */
  const updateDtoVar = `updateDtoFor${c.navPropName}`;
  const childUniqueIndexes = extractUniqueIndexes(c.childFile, c.childBase);
  const lineUniqueIndexes = childUniqueIndexes.filter((idx) => idx.fields.includes('LineNumber'));
  const indent = '        ';
  const lines = [];
  lines.push(`${indent}// ${childDesc}（${c.navPropName}）`);
  lines.push(`${indent}List<${childUpdateDto}>? ${saveVar};`);
  lines.push(`${indent}if (dto is Takt${entityShort}UpdateDto ${updateDtoVar} && ${updateDtoVar}.${c.navPropName} != null)`);
  lines.push(`${indent}{`);
  lines.push(`${indent}    ${saveVar} = ${updateDtoVar}.${c.navPropName};`);
  lines.push(`${indent}}`);
  lines.push(`${indent}else if (dto.${c.navPropName} != null)`);
  lines.push(`${indent}{`);
  lines.push(`${indent}    ${saveVar} = dto.${c.navPropName}.Adapt<List<${childUpdateDto}>>();`);
  lines.push(`${indent}}`);
  lines.push(`${indent}else`);
  lines.push(`${indent}{`);
  lines.push(`${indent}    ${saveVar} = null;`);
  lines.push(`${indent}}`);
  if (c.childHasIsObsolete) {
    lines.push(`${indent}if (${saveVar} is not { Count: > 0 })`);
    lines.push(`${indent}{`);
    lines.push(`${indent}    await Mark${c.childShort}sObsoleteAsync(entity.Id);`);
    if (isSingleChild) {
      lines.push(`${indent}    return;`);
    }
    lines.push(`${indent}}`);
  } else {
    lines.push(`${indent}if (${saveVar} is not { Count: > 0 })`);
    lines.push(`${indent}{`);
    lines.push(`${indent}    await ${c.childRepoField}.DeleteAsync(x => ${c.linkPredicate});`);
    lines.push(`${indent}}`);
  }
  lines.push(`${indent}else`);
  lines.push(`${indent}{`);
  lines.push(`${indent}    var existingList = await ${c.childRepoField}.GetListAsync(x => ${c.linkPredicate});`);
  lines.push(`${indent}    var existingById = existingList.ToDictionary(x => x.Id);`);
  lines.push(`${indent}    var submittedIds = new HashSet<long>();`);
  lines.push(`${indent}    var toCreate = new List<${c.childEntity}>();`);
  if (c.childSeq?.lineNumber) {
    lines.push(`${indent}    var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);`);
  }
  lines.push(`${indent}    for (var i = 0; i < ${saveVar}.Count; i++)`);
  lines.push(`${indent}    {`);
  lines.push(`${indent}        var childDto = ${saveVar}[i];`);
  if (c.masterIdField) {
    lines.push(`${indent}        childDto.${c.masterIdField} = entity.Id;`);
  }
  const stampMaps =
    c.stampMappings && c.stampMappings.length
      ? c.stampMappings
      : (c.stampFields || []).map((field) => ({ childField: field, masterField: field }));
  for (const m of stampMaps) {
    lines.push(`${indent}        childDto.${m.childField} = ${formatStampAssignmentRhs('entity', m.masterField, entityFile)};`);
  }
  if (c.childSeq?.lineNumber) {
    // 行号去重键：公司/审批级用 CompanyCode；租户级无公司列，用 TenantCode
    const lineScopeField = dtoBaseHasCompanyIsolation(dtoBase) ? 'CompanyCode' : 'TenantCode';
    lines.push(`${indent}        var lineKey = $"{entity.${lineScopeField}}|{entity.Id}|{childDto.LineNumber}";`);
    lines.push(`${indent}        if (!seenLineKeys.Add(lineKey))`);
    lines.push(`${indent}        {`);
    lines.push(`${indent}            throw new TaktBusinessException("${childDesc}第{i + 1}项与本次提交的其他项重复（${lineScopeField}、${c.masterIdField}、LineNumber）");`);
    lines.push(`${indent}        }`);
  }
  lines.push(`${indent}        if (childDto.${childIdProp} > 0)`);
  lines.push(`${indent}        {`);
  lines.push(`${indent}            if (!existingById.TryGetValue(childDto.${childIdProp}, out var target))`);
  lines.push(`${indent}            {`);
  lines.push(`${indent}                throw new TaktBusinessException("${childDesc}不存在（${childIdProp}={childDto.${childIdProp}}）");`);
  lines.push(`${indent}            }`);
  lines.push(`${indent}            if (target.${c.masterIdField} != entity.Id)`);
  lines.push(`${indent}            {`);
  lines.push(`${indent}                throw new TaktBusinessException("${childDesc}不属于当前主表（${childIdProp}={childDto.${childIdProp}}）");`);
  lines.push(`${indent}            }`);
  lines.push(`${indent}            submittedIds.Add(childDto.${childIdProp});`);
  if (lineUniqueIndexes.length) {
    for (const idx of lineUniqueIndexes) {
      const varSuffix = sanitizeUniqueIndexVarSuffix(idx.indexKey);
      const fieldLabel = idx.fields.join('、');
      const predicate = buildUniquePredicate(idx.fields, 'childDto', '                ');
      lines.push(`${indent}            var isUniqueUpdate_${varSuffix} = await _uniqueValidator.IsUniqueAsync(`);
      lines.push(`${indent}                ${c.childRepoField},`);
      lines.push(`${indent}                x => ${predicate.replace(/childDto\./g, 'x.')},`);
      lines.push(`${indent}                childDto.${childIdProp});`);
      lines.push(`${indent}            if (!isUniqueUpdate_${varSuffix})`);
      lines.push(`${indent}            {`);
      lines.push(`${indent}                throw new TaktBusinessException("${childDesc}的${fieldLabel}已存在");`);
      lines.push(`${indent}            }`);
    }
  }
  lines.push(`${indent}            childDto.Adapt(target);`);
  lines.push(`${indent}            target.Id = childDto.${childIdProp};`);
  lines.push(`${indent}            target.${c.masterIdField} = entity.Id;`);
  if (c.childHasIsObsolete) {
    lines.push(`${indent}            target.IsObsolete = 0;`);
  }
  lines.push(`${indent}            await ${c.childRepoField}.UpdateAsync(target);`);
  lines.push(`${indent}        }`);
  lines.push(`${indent}        else`);
  lines.push(`${indent}        {`);
  if (lineUniqueIndexes.length) {
    for (const idx of lineUniqueIndexes) {
      const varSuffix = sanitizeUniqueIndexVarSuffix(idx.indexKey);
      const fieldLabel = idx.fields.join('、');
      const predicate = buildUniquePredicate(idx.fields, 'childDto', '                ');
      lines.push(`${indent}            var isUniqueCreate_${varSuffix} = await _uniqueValidator.IsUniqueAsync(`);
      lines.push(`${indent}                ${c.childRepoField},`);
      lines.push(`${indent}                x => ${predicate.replace(/childDto\./g, 'x.').replace(/entity\./g, 'x.')});`);
      lines.push(`${indent}            if (!isUniqueCreate_${varSuffix})`);
      lines.push(`${indent}            {`);
      lines.push(`${indent}                throw new TaktBusinessException("${childDesc}的${fieldLabel}已存在");`);
      lines.push(`${indent}            }`);
    }
  }
  lines.push(`${indent}            var child = childDto.Adapt<${c.childEntity}>();`);
  lines.push(`${indent}            child.Id = 0;`);
  lines.push(`${indent}            child.${c.masterIdField} = entity.Id;`);
  if (c.childHasIsObsolete) {
    lines.push(`${indent}            child.IsObsolete = 0;`);
  }
  lines.push(`${indent}            toCreate.Add(child);`);
  lines.push(`${indent}        }`);
  lines.push(`${indent}    }`);
  if (c.childHasIsObsolete) {
    lines.push(`${indent}    var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();`);
    lines.push(`${indent}    foreach (var removed in toObsolete)`);
    lines.push(`${indent}    {`);
    lines.push(`${indent}        removed.IsObsolete = 1;`);
    lines.push(`${indent}        await ${c.childRepoField}.UpdateAsync(removed);`);
    lines.push(`${indent}    }`);
  } else {
    lines.push(`${indent}    foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))`);
    lines.push(`${indent}    {`);
    lines.push(`${indent}        await ${c.childRepoField}.DeleteAsync(removed.Id);`);
    lines.push(`${indent}    }`);
  }
  if (c.childSeq?.lineNumber) {
    const masterCodeField = resolvePrimaryBusinessCodeField(entityFile, entityShort);
    const scope = buildTenantCompanyScope(dtoBase, 'x');
    const maxPredicate = `${scope} && x.${c.masterIdField} == entity.Id`;
    const businessCodeFromMaster = masterCodeField
      ? `!string.IsNullOrWhiteSpace(entity.${masterCodeField}) ? entity.${masterCodeField} : entity.Id.ToString()`
      : 'entity.Id.ToString()';
    lines.push(`${indent}    if (toCreate.Count > 0)`);
    lines.push(`${indent}    {`);
    lines.push(`${indent}        var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();`);
    lines.push(`${indent}        if (needLine.Count > 0)`);
    lines.push(`${indent}        {`);
    lines.push(`${indent}            var businessCode = ${businessCodeFromMaster};`);
    if (c.childHasIsObsolete) {
      lines.push(`${indent}            var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;`);
    } else {
      lines.push(`${indent}            var maxLine = await ${c.childRepoField}.GetMaxIntAsync(`);
      lines.push(`${indent}                x => ${maxPredicate},`);
      lines.push(`${indent}                x => x.LineNumber,`);
      lines.push(`${indent}                includeSoftDeleted: true);`);
    }
    lines.push(`${indent}            var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();`);
    lines.push(`${indent}            var lineIdx = 0;`);
    lines.push(`${indent}            foreach (var child in toCreate)`);
    lines.push(`${indent}            {`);
    lines.push(`${indent}                if (child.LineNumber <= 0)`);
    lines.push(`${indent}                {`);
    lines.push(`${indent}                    child.LineNumber = lineSeq[lineIdx++];`);
    lines.push(`${indent}                }`);
    lines.push(`${indent}            }`);
    lines.push(`${indent}        }`);
    lines.push(`${indent}        await ${c.childRepoField}.CreateRangeAsync(toCreate);`);
    lines.push(`${indent}    }`);
  } else {
    lines.push(`${indent}    if (toCreate.Count > 0)`);
    lines.push(`${indent}    {`);
    lines.push(`${indent}        await ${c.childRepoField}.CreateRangeAsync(toCreate);`);
    lines.push(`${indent}    }`);
  }
  lines.push(`${indent}}`);
  return lines;
}

/**
 * 主子表（OneToMany）服务级联代码块
 * @param {string} entityFile
 * @param {string} entityName
 * @param {string} entityShort
 * @param {object} dtoInfo
 * @param {string} dtoBase
 * @param {string} masterRepoField
 */
function generateMasterDetailServiceExtras(
  entityFile,
  entityName,
  entityShort,
  dtoInfo,
  dtoBase,
  masterRepoField,
  desc,
) {
  const rawChildren = parseOneToManyNavigations(entityFile).filter(
    (nav) => !isRbacJunctionEntity(nav.childShort) && !isStandaloneChildVueEntity(nav.childShort),
  );
  if (!rawChildren.length) {
    return null;
  }

  const children = rawChildren
    .map((nav) => {
      const childFile = findEntityFile(nav.childEntity);
      if (!childFile || !nav.foreignKeyOnChild) {
        return null;
      }
      const childBase = parseEntityBase(childFile);
      const childRepoInterface = ENTITY_BASE_TO_REPOSITORY[childBase];
      if (!childRepoInterface) {
        return null;
      }
      const childModuleParts = getModuleRelativePath(childFile);
      const childDtoNs = buildNamespace('Takt.Application.Dtos', childModuleParts);
      const masterIdField = resolveChildMasterIdField(
        childFile,
        nav.foreignKeyOnChild,
        entityShort,
      );
      const stampMappings = getChildStampFieldMappings(
        childFile,
        nav.foreignKeyOnChild,
        masterIdField,
        entityFile,
        entityShort,
      );
      const childSeq = analyzeEntitySequenceFields(childFile, nav.childShort, 'Single', childBase);
      return {
        ...nav,
        childFile,
        childBase,
        childRepoInterface,
        childRepoField: repositoryFieldName(nav.childShort),
        childDtoNs,
        childResponseDto: `Takt${nav.childShort}Dto`,
        childUpdateDto: `Takt${nav.childShort}UpdateDto`,
        masterIdField,
        stampMappings,
        stampFields: stampMappings
          .filter((m) => m.childField === m.masterField)
          .map((m) => m.childField),
        childSeq,
        linkPredicate: buildChildLinkPredicate(
          'x',
          'entity',
          nav.foreignKeyOnChild,
          masterIdField,
        ),
        childHasIsObsolete: entityFileHasIsObsolete(childFile),
      };
    })
    .filter(Boolean);

  if (!children.length) {
    return null;
  }

  const extraUsings = [
    ...new Set(
      children
        .flatMap((c) => [c.childDtoNs, c.childFile ? getEntityNamespace(c.childFile) : null])
        .filter((ns) => ns),
    ),
  ];

  const ctorFields = children
    .map(
      (c) =>
        `    private readonly ${c.childRepoInterface}<${c.childEntity}> ${c.childRepoField};`,
    )
    .join('\n');

  const ctorParams = children
    .map((c) => {
      const paramName = `${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository`;
      return `        ${c.childRepoInterface}<${c.childEntity}> ${paramName},`;
    })
    .join('\n');

  const ctorParamDocs = children
    .map((c) => `    /// <param name="${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository">${c.childShort}仓储</param>`)
    .join('\n');

  const ctorAssigns = children
    .map((c) => {
      const paramName = `${c.childShort.charAt(0).toLowerCase()}${c.childShort.slice(1)}Repository`;
      return `        ${c.childRepoField} = ${paramName};`;
    })
    .join('\n');

  const childDescSummary = children
    .map((c) => extractEntityDescription(c.childFile) || c.navPropName)
    .join('、');

  const fillDoc = buildMethodXmlDoc({
    summary: `填充${desc}详情（加载 OneToMany 子表：${childDescSummary}）`,
    params: [
      { name: 'dto', desc: '响应 DTO' },
      { name: 'entity', desc: '主表实体' },
    ],
    returns: '任务',
  });

  const saveDoc = buildMethodXmlDoc({
    summary: `保存${desc}子表级联（${childDescSummary}；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）`,
    params: [
      { name: 'entity', desc: '主表实体' },
      { name: 'dto', desc: '创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）' },
    ],
    returns: '任务',
  });

  const markObsoleteMethodLines = [];
  for (const c of children) {
    if (!c.childHasIsObsolete) {
      continue;
    }
    const masterIdParam = `${c.masterIdField.charAt(0).toLowerCase()}${c.masterIdField.slice(1)}`;
    const childDesc = extractEntityDescription(c.childFile) || c.childEntity;
    markObsoleteMethodLines.push('    /// <summary>');
    markObsoleteMethodLines.push(`    /// 将指定主表下全部未作废${childDesc}标记为作废（编辑清空子表）`);
    markObsoleteMethodLines.push('    /// </summary>');
    markObsoleteMethodLines.push(`    /// <param name="${masterIdParam}">主表主键</param>`);
    markObsoleteMethodLines.push('    /// <returns>任务</returns>');
    markObsoleteMethodLines.push(`    private async Task Mark${c.childShort}sObsoleteAsync(long ${masterIdParam})`);
    markObsoleteMethodLines.push('    {');
    markObsoleteMethodLines.push(`        if (${masterIdParam} <= 0)`);
    markObsoleteMethodLines.push('        {');
    markObsoleteMethodLines.push('            return;');
    markObsoleteMethodLines.push('        }');
    markObsoleteMethodLines.push(`        var rows = await ${c.childRepoField}.GetListAsync(`);
    markObsoleteMethodLines.push(`            x => x.${c.masterIdField} == ${masterIdParam} && x.IsObsolete == 0);`);
    markObsoleteMethodLines.push('        if (rows.Count == 0)');
    markObsoleteMethodLines.push('        {');
    markObsoleteMethodLines.push('            return;');
    markObsoleteMethodLines.push('        }');
    markObsoleteMethodLines.push('        foreach (var row in rows)');
    markObsoleteMethodLines.push('        {');
    markObsoleteMethodLines.push('            row.IsObsolete = 1;');
    markObsoleteMethodLines.push('        }');
    markObsoleteMethodLines.push(`        await ${c.childRepoField}.UpdateRangeAsync(rows);`);
    markObsoleteMethodLines.push('    }');
    markObsoleteMethodLines.push('');
  }

  const fillMethodLines = [];
  fillMethodLines.push(fillDoc.trimEnd());
  fillMethodLines.push(`    private async Task Fill${entityShort}DetailsAsync(${dtoInfo.base} dto, ${entityName} entity)`);
  fillMethodLines.push('    {');
  fillMethodLines.push('        if (dto == null)');
  fillMethodLines.push('        {');
  fillMethodLines.push('            return;');
  fillMethodLines.push('        }');
  for (const c of children) {
    const childDesc = extractEntityDescription(c.childFile) || c.childEntity;
    const fillNote = c.childHasIsObsolete ? '（含作废行）' : '';
    fillMethodLines.push(`        // ${childDesc} → dto.${c.navPropName}${fillNote}`);
    const fillPredicate = c.linkPredicate;
    fillMethodLines.push(`        var ${c.navPropName.toLowerCase()} = await ${c.childRepoField}.GetListAsync(x => ${fillPredicate});`);
    fillMethodLines.push(`        dto.${c.navPropName} = ${c.navPropName.toLowerCase()}.Adapt<List<${c.childResponseDto}>>();`);
    if (c.childSeq?.lineNumber && !c.childHasIsObsolete) {
      const scope = buildTenantCompanyScope(dtoBase, 'x');
      const maxPredicate = `${scope} && x.${c.masterIdField} == entity.Id`;
      fillMethodLines.push(`        dto.Max${c.childShort}LineNumber = await ${c.childRepoField}.GetMaxIntAsync(`);
      fillMethodLines.push(`            x => ${maxPredicate},`);
      fillMethodLines.push('            x => x.LineNumber,');
      fillMethodLines.push('            includeSoftDeleted: true);');
    }
  }
  fillMethodLines.push('    }');
  fillMethodLines.push('');

  const saveMethodLines = [];
  saveMethodLines.push(saveDoc.trimEnd());
  saveMethodLines.push(`    private async Task Save${entityShort}ChildrenAsync(${entityName} entity, ${dtoInfo.create} dto)`);
  saveMethodLines.push('    {');
  for (const c of children) {
    const childDesc = extractEntityDescription(c.childFile) || c.childEntity;
    for (const line of buildMasterDetailChildUpsertBlock(
      c,
      entityName,
      entityShort,
      entityFile,
      dtoBase,
      childDesc,
      { isSingleChild: children.length === 1 },
    )) {
      saveMethodLines.push(line);
    }
  }
  saveMethodLines.push('    }');
  saveMethodLines.push('');

  const deletePrefixLines = [];
  deletePrefixLines.push(`        var entity = await ${masterRepoField}.GetByIdAsync(id);`);
  deletePrefixLines.push('        if (entity == null)');
  deletePrefixLines.push('        {');
  deletePrefixLines.push(`            throw new TaktBusinessException("${desc}不存在或已删除");`);
  deletePrefixLines.push('        }');
  if (entityHasIsBuiltIn(entityFile)) {
    deletePrefixLines.push(buildBuiltInDeleteGuardLines(desc).trimEnd());
  }
  for (const c of children) {
    deletePrefixLines.push(`        await ${c.childRepoField}.DeleteAsync(x => ${c.linkPredicate});`);
  }

  return {
    children,
    extraUsings,
    ctorFields,
    ctorParams,
    ctorParamDocs,
    ctorAssigns,
    privateMethods: [...markObsoleteMethodLines, ...fillMethodLines, ...saveMethodLines].join('\n'),
    getByIdReturn: `        var dto = entity.Adapt<${dtoInfo.base}>();\n        await Fill${entityShort}DetailsAsync(dto, entity);\n        return dto;`,
    createAfterSave: `        await Save${entityShort}ChildrenAsync(entity, dto);`,
    updateAfterSave: `        await Save${entityShort}ChildrenAsync(entity, dto);`,
    deletePrefix: deletePrefixLines.join('\n'),
    skipDirectDelete: true,
  };
}

/**
 * 实体业务 string 属性（排除租户/审计等非展示列）
 * @param {string} entityFile
 * @returns {string[]}
 */
function listBusinessStringPropertyNames(entityFile) {
  const content = readUtf8(entityFile);
  const stringRegex = /public\s+string\??\s+(\w+)\s*\{/g;
  const standard = new Set([
    'TenantCode',
    'CompanyCode',
    'CultureCode',
    'PlantCode',
    'RelatedPlant',
    'ExtField',
    'Remark',
    'CreatedBy',
    'UpdatedBy',
    'DeletedBy',
    'ApprovalOpinion',
  ]);
  const names = [];
  let m;
  while ((m = stringRegex.exec(content)) !== null) {
    if (!standard.has(m[1])) {
      names.push(m[1]);
    }
  }
  return names;
}

/**
 * 下拉 DictLabel：优先 {Entity}Name，再合理 *Name（不含 Code / nvarchar 回退）
 * @param {string} entityFile
 * @param {string} [entityShort] 不含 Takt 前缀
 * @returns {string|null}
 */
function getNameFieldName(entityFile, entityShort = '') {
  const names = listBusinessStringPropertyNames(entityFile);
  /** 非业务显示名（不可作 DictLabel；如 FormName） */
  const nameDenylist = new Set([
    'FormName',
    'FileName',
    'SheetName',
    'TableName',
    'ColumnName',
    'SchemaName',
    'AssemblyName',
  ]);
  const exactName = entityShort ? `${entityShort}Name` : '';
  if (exactName && names.includes(exactName)) {
    return exactName;
  }
  const nameField = names.find(
    (f) =>
      f.endsWith('Name') &&
      !nameDenylist.has(f) &&
      !f.endsWith('ByName') &&
      !f.endsWith('FileName'),
  );
  return nameField || null;
}

/**
 * 无 *Code / *Name 时：声明顺序上第一个业务 string（对应 nvarchar 列）
 * @param {string} entityFile
 * @param {string} [entityShort]
 * @returns {string|null}
 */
function getFirstBusinessNvarcharField(entityFile, entityShort = '') {
  const names = listBusinessStringPropertyNames(entityFile);
  const nameDenylist = new Set([
    'FormName',
    'FileName',
    'SheetName',
    'TableName',
    'ColumnName',
    'SchemaName',
    'AssemblyName',
  ]);
  const preferredName = getNameFieldName(entityFile, entityShort);
  const codeField = resolvePrimaryBusinessCodeField(entityFile, entityShort);
  const skip = new Set([preferredName, codeField].filter(Boolean));
  return (
    names.find((f) => !skip.has(f) && !nameDenylist.has(f)) ||
    names.find((f) => !nameDenylist.has(f)) ||
    null
  );
}

/**
 * 下拉 DictValue：*Code → *Name → 首个业务 nvarchar；禁止雪花 Id
 * @param {string} entityFile
 * @param {string} entityShort
 * @returns {string|null}
 */
function getOptionsValueFieldName(entityFile, entityShort) {
  return (
    resolvePrimaryBusinessCodeField(entityFile, entityShort) ||
    getNameFieldName(entityFile, entityShort) ||
    getFirstBusinessNvarcharField(entityFile, entityShort)
  );
}

/**
 * 解析 Options 的 label/value：无 Code 用 Name，再无则用首个 nvarchar；禁止 Id
 * @param {string} entityFile
 * @param {string} entityShort
 * @param {string} desc
 */
/**
 * 无业务 nvarchar 时：整数展示字段回退（如 StepNo）；Options 生成时 ToString
 * @param {string} entityFile
 * @returns {string|null}
 */
function getFirstBusinessIntDisplayField(entityFile) {
  const content = readUtf8(entityFile);
  const intRegex = /public\s+int\??\s+(\w+)\s*\{/g;
  const skip = new Set([
    'IsDeleted',
    'IsBuiltIn',
    'IsLeaf',
    'Level',
    'ApprovalStatus',
  ]);
  const names = [];
  let m;
  while ((m = intRegex.exec(content)) !== null) {
    if (skip.has(m[1])) continue;
    if (/^Is[A-Z]/.test(m[1])) continue;
    if (m[1].endsWith('Status')) continue;
    names.push(m[1]);
  }
  return (
    names.find((f) => f.endsWith('No')) ||
    names.find((f) => f === 'SortOrder') ||
    names[0] ||
    null
  );
}

function resolveOptionsDisplayFields(entityFile, entityShort, desc) {
  const codeField = resolvePrimaryBusinessCodeField(entityFile, entityShort);
  const nameOnly = getNameFieldName(entityFile, entityShort);
  const firstNvarchar = getFirstBusinessNvarcharField(entityFile, entityShort);
  const firstInt = getFirstBusinessIntDisplayField(entityFile);
  const valueField = codeField || nameOnly || firstNvarchar || firstInt;
  if (!valueField || valueField === 'Id') {
    throw new Error(
      `实体 Takt${entityShort}（${desc}）无可用 Options 字段：请提供 {Entity}Code，或 *Name，或至少一个业务 nvarchar/int 字段（禁止雪花 Id）`,
    );
  }
  const nameField = nameOnly || codeField || firstNvarchar || firstInt || valueField;
  const valueAsString = Boolean(firstInt && valueField === firstInt && !codeField && !nameOnly && !firstNvarchar);
  return { nameField, valueField, valueAsString };
}

function extractEntityPropertyNames(entityFile) {
  const content = readUtf8(entityFile);
  const propRegex = /public\s+[\w<>,\?\s]+\s+(\w+)\s*\{/g;
  const names = new Set();
  let m;
  while ((m = propRegex.exec(content)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * 实体上声明为 string/string? 的属性名（Stamp 仅回填字符串，禁止 decimal/int 走 IsNullOrEmpty）
 * @param {string} entityFile
 * @returns {Set<string>}
 */
function extractEntityStringPropertyNames(entityFile) {
  const content = readUtf8(entityFile);
  const propRegex = /public\s+(?:string\?|string)\s+(\w+)\s*\{/g;
  const names = new Set();
  let m;
  while ((m = propRegex.exec(content)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * 实体 string 属性是否可空（Stamp/SaveChildren 赋值时 nullable→non-nullable 须 ?? string.Empty）
 * @param {string} entityFile
 * @param {string} propName
 * @returns {boolean}
 */
function isEntityStringPropertyNullable(entityFile, propName) {
  const content = readUtf8(entityFile);
  return new RegExp(`public\\s+string\\?\\s+${propName}\\s*\\{`).test(content);
}

/**
 * Stamp 同源字段赋值右侧（可空源赋给非空目标时兜底空串，消除 CS8601）
 * @param {string} sourceVar
 * @param {string} sourceField
 * @param {string|null} sourceEntityFile
 * @returns {string}
 */
function formatStampAssignmentRhs(sourceVar, sourceField, sourceEntityFile) {
  const rhs = `${sourceVar}.${sourceField}`;
  if (sourceEntityFile && isEntityStringPropertyNullable(sourceEntityFile, sourceField)) {
    return `${rhs} ?? string.Empty`;
  }
  return rhs;
}

function entityHasIntProperty(entityFile, propName) {
  const content = readUtf8(entityFile);
  return new RegExp(`public\\s+int\\s+${propName}\\s*\\{`).test(content);
}

function entityHasParentId(entityFile) {
  const content = readUtf8(entityFile);
  return /public\s+long(?:\?)?\s+ParentId\s*\{/.test(content);
}

/**
 * 租户/公司隔离谓词（三基类；与 buildOptionsListPredicate 同源）
 * - TaktTenantDtoBase → 仅 TenantCode
 * - TaktCompanyDtoBase / TaktApprovalDtoBase → TenantCode + CompanyCode
 * @param {string} dtoBase
 * @param {string} varName
 */
function buildTenantCompanyScope(dtoBase, varName) {
  if (!dtoBaseHasCompanyIsolation(dtoBase)) {
    return `${varName}.TenantCode == CurrentTenantCode`;
  }
  return `${varName}.TenantCode == CurrentTenantCode && ${varName}.CompanyCode == CurrentCompanyCode`;
}

/**
 * 主表/当前实体上的业务编码字段（用于行号生成）
 * @param {string} entityFile
 * @param {string} entityShort 不含 Takt 前缀
 */
function resolvePrimaryBusinessCodeField(entityFile, entityShort) {
  const props = extractEntityPropertyNames(entityFile);
  const exact = `${entityShort}Code`;
  if (props.has(exact)) {
    return exact;
  }
  const skip = new Set(['TenantCode', 'CompanyCode', 'PlantCode', 'WarehouseCode', 'LocationCode']);
  const candidates = [...props].filter((p) => p.endsWith('Code') && !skip.has(p));
  const shortLower = entityShort.toLowerCase();
  const matched = candidates.find((p) => p.toLowerCase().includes(shortLower));
  return matched || candidates[0] || null;
}

/**
 * 解析 SortOrder 自动生成作用域
 * @param {string} entityFile
 * @param {string} crudType
 * @param {string} dtoBase
 */
function detectSortOrderConfig(entityFile, crudType, dtoBase) {
  if (!entityHasIntProperty(entityFile, 'SortOrder')) {
    return null;
  }
  if (crudType === 'Tree' || entityHasParentId(entityFile)) {
    return { mode: 'tree' };
  }
  const props = extractEntityPropertyNames(entityFile);
  if (props.has('GroupCode')) {
    const lineIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
      idx.fields.includes('SortOrder'),
    );
    const masterIdField = lineIndexes.length
      ? lineIndexes[0].fields.find((f) => f.endsWith('Id') && f !== 'Id')
      : null;
    if (masterIdField) {
      return { mode: 'group', masterIdField, groupCodeField: 'GroupCode' };
    }
  }
  const sortIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
    idx.fields.includes('SortOrder'),
  );
  let masterIdField = null;
  if (sortIndexes.length) {
    masterIdField = sortIndexes[0].fields.find((f) => f.endsWith('Id') && f !== 'Id') || null;
  }
  if (!masterIdField) {
    const content = readUtf8(entityFile);
    const idFields = [...content.matchAll(/public\s+long(?:\?)?\s+(\w+Id)\s*\{/g)]
      .map((m) => m[1])
      .filter((n) => n !== 'Id' && n !== 'ParentId' && props.has(n));
    masterIdField =
      idFields.find((n) => /TypeId$|CategoryId$|GroupId$|HeaderId$|MasterId$/.test(n)) ||
      idFields[0] ||
      null;
  }
  if (masterIdField) {
    return { mode: 'master', masterIdField };
  }
  return { mode: 'flat' };
}

/**
 * 解析 LineNumber 自动生成作用域
 * @param {string} entityFile
 * @param {string} dtoBase
 */
function detectLineNumberConfig(entityFile, dtoBase) {
  if (!entityHasIntProperty(entityFile, 'LineNumber')) {
    return null;
  }
  const props = extractEntityPropertyNames(entityFile);
  const lineIndexes = extractUniqueIndexes(entityFile, dtoBase).filter((idx) =>
    idx.fields.includes('LineNumber'),
  );
  let masterIdField = null;
  let groupCodeField = null;
  if (lineIndexes.length) {
    const fields = lineIndexes[0].fields;
    const linePos = fields.indexOf('LineNumber');
    masterIdField =
      fields.slice(0, linePos).find((f) => f.endsWith('Id') && f !== 'Id') || null;
    const between = fields.slice(0, linePos).filter((f) => f !== masterIdField && !f.endsWith('Id'));
    if (between.length === 1 && props.has(between[0]) && between[0] !== 'LineNumber') {
      groupCodeField = between[0];
    }
  }
  if (!masterIdField) {
    const content = readUtf8(entityFile);
    const idFields = [...content.matchAll(/public\s+long(?:\?)?\s+(\w+Id)\s*\{/g)]
      .map((m) => m[1])
      .filter((n) => n !== 'Id' && n !== 'ParentId' && props.has(n));
    masterIdField =
      idFields.find((n) => /ItemId$|OrderId$|OperationId$|StandardId$/.test(n)) || idFields[0] || null;
  }
  const prefix = masterIdField ? masterIdField.replace(/Id$/, '') : '';
  const businessCodeField = prefix && props.has(`${prefix}Code`) ? `${prefix}Code` : null;
  if (groupCodeField) {
    return {
      mode: 'group',
      masterIdField,
      businessCodeField,
      groupCodeField,
    };
  }
  return {
    mode: 'businessCode',
    masterIdField,
    businessCodeField,
  };
}

/**
 * @param {string} entityFile
 * @param {string} entityShort
 * @param {string} crudType
 * @param {string} dtoBase
 */
function analyzeEntitySequenceFields(entityFile, entityShort, crudType, dtoBase) {
  return {
    sortOrder: detectSortOrderConfig(entityFile, crudType, dtoBase),
    lineNumber: detectLineNumberConfig(entityFile, dtoBase),
  };
}

/**
 * @param {object} ctx
 */
function buildSequenceMetaForService(ctx) {
  const main = analyzeEntitySequenceFields(
    ctx.entityFile,
    ctx.entityShort,
    ctx.crudType,
    ctx.dtoBase,
  );
  const childAssignments = [];
  if (ctx.masterDetail?.children) {
    for (const c of ctx.masterDetail.children) {
      const childSeq = analyzeEntitySequenceFields(c.childFile, c.childShort, 'Single', c.childBase);
      if (childSeq.sortOrder || childSeq.lineNumber) {
        childAssignments.push({ child: c, seq: childSeq });
      }
    }
  }
  const needsSort = Boolean(main.sortOrder) || childAssignments.some((x) => x.seq.sortOrder);
  const needsLine = Boolean(main.lineNumber) || childAssignments.some((x) => x.seq.lineNumber);
  return { main, childAssignments, needsSort, needsLine };
}

/**
 * @param {object} sortConfig
 * @param {string} repoField
 * @param {string} dtoBase
 * @param {string} entityVar
 * @param {string} indent
 */
function buildAssignSortOrderBlock(sortConfig, repoField, dtoBase, entityVar, indent, entityFile) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  let maxPredicate = scope;
  let generateCall;
  if (sortConfig.mode === 'tree') {
    maxPredicate = `${scope} && x.ParentId == ${entityVar}.ParentId`;
    generateCall = `_sortOrderGenerator.GenerateNext(${entityVar}.ParentId, maxSort)`;
  } else if (sortConfig.mode === 'master') {
    maxPredicate = `${scope} && x.${sortConfig.masterIdField} == ${entityVar}.${sortConfig.masterIdField}`;
    generateCall = `_sortOrderGenerator.GenerateNextForMaster(${wrapMasterIdExpr(entityFile, entityVar, sortConfig.masterIdField)}, maxSort)`;
  } else if (sortConfig.mode === 'group') {
    maxPredicate = `${scope} && x.${sortConfig.masterIdField} == ${entityVar}.${sortConfig.masterIdField} && x.${sortConfig.groupCodeField} == ${entityVar}.${sortConfig.groupCodeField}`;
    generateCall = `_sortOrderGenerator.GenerateNextForGroup(${wrapMasterIdExpr(entityFile, entityVar, sortConfig.masterIdField)}, ${entityVar}.${sortConfig.groupCodeField}, maxSort)`;
  } else {
    generateCall = '_sortOrderGenerator.GenerateNext(maxSort)';
  }
  return `${indent}if (${entityVar}.SortOrder <= 0)
${indent}{
${indent}    var maxSort = await ${repoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.SortOrder);
${indent}    ${entityVar}.SortOrder = ${generateCall};
${indent}}
`;
}

/**
 * 导入（扁平 SortOrder）在循环外预取最大值，循环内递增
 */
function buildImportSortOrderDeclare(sortConfig, repoField, dtoBase) {
  if (!sortConfig || sortConfig.mode !== 'flat') {
    return '';
  }
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  return `        var importSortMax = await ${repoField}.GetMaxIntAsync(
            x => ${scope},
            x => x.SortOrder);
`;
}

function buildImportSortOrderAssign(sortConfig, entityVar, indent) {
  return `${indent}if (${entityVar}.SortOrder <= 0)
${indent}{
${indent}    ${entityVar}.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
${indent}    importSortMax = ${entityVar}.SortOrder;
${indent}}
`;
}

/**
 * @param {object} lineConfig
 * @param {string} repoField
 * @param {string} dtoBase
 * @param {string} entityVar
 * @param {string} indent
 */
function buildAssignLineNumberBlock(lineConfig, repoField, dtoBase, entityVar, indent) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  let maxPredicate = scope;
  if (lineConfig.masterIdField) {
    maxPredicate = `${scope} && x.${lineConfig.masterIdField} == ${entityVar}.${lineConfig.masterIdField}`;
  }
  let businessCodeExpr;
  if (lineConfig.businessCodeField) {
    businessCodeExpr = `!string.IsNullOrWhiteSpace(${entityVar}.${lineConfig.businessCodeField}) ? ${entityVar}.${lineConfig.businessCodeField} : ${entityVar}.${lineConfig.masterIdField}.ToString()`;
  } else if (lineConfig.masterIdField) {
    businessCodeExpr = `${entityVar}.${lineConfig.masterIdField}.ToString()`;
  } else {
    businessCodeExpr = `${entityVar}.Id.ToString()`;
  }
  if (lineConfig.mode === 'group' && lineConfig.groupCodeField) {
    maxPredicate = `${maxPredicate} && x.${lineConfig.groupCodeField} == ${entityVar}.${lineConfig.groupCodeField}`;
    return `${indent}if (${entityVar}.LineNumber <= 0)
${indent}{
${indent}  var maxLine = await ${repoField}.GetMaxIntAsync(
${indent}      x => ${maxPredicate},
${indent}      x => x.LineNumber);
${indent}  var businessCode = ${businessCodeExpr};
${indent}  ${entityVar}.LineNumber = _lineNumberGenerator.GenerateNextForGroup(businessCode, ${entityVar}.${lineConfig.groupCodeField}, maxLine);
${indent}}
`;
  }
  return `${indent}if (${entityVar}.LineNumber <= 0)
${indent}{
${indent}    var maxLine = await ${repoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.LineNumber);
${indent}    var businessCode = ${businessCodeExpr};
${indent}    ${entityVar}.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
${indent}}
`;
}

/**
 * 主子表 SaveChildren：批量分配子表 SortOrder
 */
function buildAssignChildSortOrdersInSave(c, masterVar, listVar, indent, dtoBase) {
  const sortConfig = c.childSeq?.sortOrder;
  if (!sortConfig) {
    return '';
  }
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const masterFk = c.masterIdField;
  let maxPredicate = `${scope} && x.${masterFk} == ${masterVar}.Id`;
  let sequenceCall;
  if (sortConfig.mode === 'master' && sortConfig.masterIdField === masterFk) {
    sequenceCall = `_sortOrderGenerator.GenerateSequenceForMaster(${masterVar}.Id, ${listVar}NeedSort.Count, maxSort)`;
  } else if (sortConfig.mode === 'tree') {
    maxPredicate = `${scope} && x.ParentId == ${masterVar}.Id`;
    sequenceCall = `_sortOrderGenerator.GenerateSequence(${masterVar}.Id, ${listVar}NeedSort.Count, maxSort)`;
  } else {
    sequenceCall = `_sortOrderGenerator.GenerateSequence(${listVar}NeedSort.Count, maxSort)`;
  }
  return `${indent}var ${listVar}NeedSort = ${listVar}.Where(c => c.SortOrder <= 0).ToList();
${indent}if (${listVar}NeedSort.Count > 0)
${indent}{
${indent}    var maxSort = await ${c.childRepoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.SortOrder);
${indent}    var sortSeq = ${sequenceCall}.ToList();
${indent}    var sortIdx = 0;
${indent}    foreach (var child in ${listVar})
${indent}    {
${indent}        if (child.SortOrder <= 0)
${indent}        {
${indent}            child.SortOrder = sortSeq[sortIdx++];
${indent}        }
${indent}    }
${indent}}
`;
}

/**
 * 主子表 SaveChildren：批量分配子表 LineNumber
 */
function buildAssignChildLineNumbersInSave(c, masterVar, listVar, indent, parentEntityFile, parentShort, dtoBase) {
  const lineConfig = c.childSeq?.lineNumber;
  if (!lineConfig) {
    return '';
  }
  const masterCodeField = resolvePrimaryBusinessCodeField(parentEntityFile, parentShort);
  const masterFk = c.masterIdField;
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const maxPredicate = `${scope} && x.${masterFk} == ${masterVar}.Id`;
  let businessCodeFromMaster;
  if (masterCodeField) {
    businessCodeFromMaster = `!string.IsNullOrWhiteSpace(${masterVar}.${masterCodeField}) ? ${masterVar}.${masterCodeField} : ${masterVar}.Id.ToString()`;
  } else {
    businessCodeFromMaster = `${masterVar}.Id.ToString()`;
  }
  const childCodeAssign =
    lineConfig.businessCodeField && lineConfig.businessCodeField !== masterCodeField
      ? `${indent}        if (string.IsNullOrWhiteSpace(child.${lineConfig.businessCodeField}))
${indent}        {
${indent}            child.${lineConfig.businessCodeField} = ${businessCodeFromMaster};
${indent}        }
`
      : '';
  const maxLineCall = c.childHasIsObsolete
    ? `${indent}    var maxLine = ${listVar}.Count > 0 ? ${listVar}.Max(x => x.LineNumber) : 0;`
    : `${indent}    var maxLine = await ${c.childRepoField}.GetMaxIntAsync(
${indent}        x => ${maxPredicate},
${indent}        x => x.LineNumber,
${indent}        includeSoftDeleted: true);`;
  return `${indent}var ${listVar}NeedLine = ${listVar}.Where(c => c.LineNumber <= 0).ToList();
${indent}if (${listVar}NeedLine.Count > 0)
${indent}{
${indent}    var businessCode = ${businessCodeFromMaster};
${maxLineCall}
${indent}    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, ${listVar}NeedLine.Count, maxLine).ToList();
${indent}    var lineIdx = 0;
${indent}    foreach (var child in ${listVar})
${indent}    {
${childCodeAssign}${indent}        if (child.LineNumber <= 0)
${indent}        {
${indent}            child.LineNumber = lineSeq[lineIdx++];
${indent}        }
${indent}    }
${indent}}
`;
}

/**
 * 提取 DTO 类中的属性名
 * @param {string} dtoContent Dtos 文件内容
 * @param {string} className DTO 类名
 * @returns {Set<string>}
 */
function extractDtoPropertyNames(dtoContent, className) {
  const names = new Set();
  const startRegex = new RegExp(`public\\s+(?:partial\\s+)?class\\s+${className}\\b[^\\{]*(?::\\s*(\\w+))?`);
  const startMatch = startRegex.exec(dtoContent);
  if (!startMatch) {
    return names;
  }
  const baseClass = startMatch[1];
  if (baseClass) {
    for (const name of extractDtoPropertyNames(dtoContent, baseClass)) {
      names.add(name);
    }
  }
  const block = extractClassBlock(dtoContent, className);
  const propRegex = /public\s+[\w?<>,\s]+\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    names.add(m[1]);
  }
  return names;
}

/**
 * 按 DTO/实体基类返回唯一索引中由仓储自动隔离的字段（不参与应用层查重条件）
 * @param {string} dtoBase TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase
 * @returns {Set<string>}
 */
function getUniqueIndexScopeFields(dtoBase) {
  const scopeFields = new Set(['TenantCode']);
  if (dtoBaseHasCompanyIsolation(dtoBase)) {
    scopeFields.add('CompanyCode');
  }
  return scopeFields;
}

/**
 * 从实体文件解析 SugarIndex 唯一索引（末位参数为 true，名称以 _unique 结尾）
 * 索引字段顺序约定：TenantCode → CompanyCode（公司/审批级）→ 业务字段；末位 true/false 为唯一约束
 * @param {string} entityFile 实体文件路径
 * @param {string} dtoBase DTO 基类（决定 TenantCode/CompanyCode 是否作为隔离字段跳过查重）
 * @returns {Array<{ indexKey: string, fields: string[] }>}
 */
function extractUniqueIndexes(entityFile, dtoBase) {
  const content = readUtf8(entityFile);
  const scopeFields = getUniqueIndexScopeFields(dtoBase);
  const results = [];
  for (const line of content.split('\n')) {
    const trimmed = line.trim();
    if (!trimmed.startsWith('[SugarIndex(') || !/,\s*true\s*\)\]$/.test(trimmed)) {
      continue;
    }
    const indexKeyMatch = trimmed.match(/^\[SugarIndex\("([^"]+)"/);
    if (!indexKeyMatch) {
      continue;
    }
    const indexKey = indexKeyMatch[1];
    const fields = [];
    const fieldRegex = /nameof\((\w+)\)/g;
    let fm;
    while ((fm = fieldRegex.exec(trimmed)) !== null) {
      fields.push(fm[1]);
    }
    const validationFields = fields.filter(
      (f) => !UNIQUE_INDEX_SKIP_FIELDS.has(f) && !scopeFields.has(f),
    );
    if (validationFields.length === 0) {
      continue;
    }
    results.push({ indexKey, fields: validationFields });
  }
  const seen = new Set();
  return results.filter((item) => {
    const key = item.fields.join('|');
    if (seen.has(key)) {
      return false;
    }
    seen.add(key);
    return true;
  });
}

/**
 * 唯一索引变量名后缀（与 ix_*_unique 索引名对齐）
 * @param {string} indexKey
 * @returns {string}
 */
function sanitizeUniqueIndexVarSuffix(indexKey) {
  return indexKey.replace(/[^a-zA-Z0-9_]/g, '_');
}

/**
 * 组合唯一条件表达式（x.Field == valueVar.Field && …）
 * @param {string[]} fields 实体唯一索引业务字段
 * @param {string} valueVar 取值变量（entity / child）
 * @param {string} [lineIndent] 换行续行缩进
 * @returns {string}
 */
function buildUniquePredicate(fields, valueVar, lineIndent = '') {
  if (!fields.length) {
    return 'true';
  }
  return fields
    .map((f, i) => (i === 0 ? `x.${f} == ${valueVar}.${f}` : `${lineIndent}&& x.${f} == ${valueVar}.${f}`))
    .join('\n');
}

/**
 * 生成 _uniqueValidator.IsUniqueAsync 组合查重语句（Create/Update/Import/子表级联）
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes 实体唯一索引
 * @param {string} repoField 仓储字段名
 * @param {string} desc 实体描述
 * @param {'create'|'update'|'import'|'child'} mode
 * @param {string} [valueVar='entity'] 取值变量名
 * @returns {string}
 */
function buildUniqueValidationBlock(uniqueIndexes, repoField, desc, mode, valueVar = 'entity') {
  if (!uniqueIndexes.length) {
    return '';
  }
  const indent =
    mode === 'import' ? '                ' : mode === 'child' ? '            ' : '        ';
  const throwIndent =
    mode === 'import' ? '                    ' : mode === 'child' ? '                ' : '            ';
  const callIndent = `${indent}    `;
  const predIndent = mode === 'import' || mode === 'child' ? `${callIndent}    ` : `${callIndent}    `;
  let block = '';
  for (const idx of uniqueIndexes) {
    const fields = idx.fields;
    if (!fields.length) {
      continue;
    }
    const varSuffix = sanitizeUniqueIndexVarSuffix(idx.indexKey);
    const fieldLabel = fields.join('、');
    const predicate = buildUniquePredicate(fields, valueVar, `${predIndent}`);
    block += `${indent}var isUnique_${varSuffix} = await _uniqueValidator.IsUniqueAsync(\n`;
    block += `${callIndent}${repoField},\n`;
    if (mode === 'update') {
      block += `${callIndent}x => ${predicate},\n`;
      block += `${callIndent}id);\n`;
    } else {
      block += `${callIndent}x => ${predicate});\n`;
    }
    block += `${indent}if (!isUnique_${varSuffix})\n`;
    block += `${indent}{\n`;
    block += `${throwIndent}throw new TaktBusinessException("${desc}的${fieldLabel}已存在");\n`;
    block += `${indent}}\n`;
  }
  return block;
}

/**
 * 导入 Excel 批次内组合键去重（与实体唯一索引业务字段对齐）
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes
 * @param {string} valueVar 取值变量名
 * @returns {{ declare: string, check: string }}
 */
function buildImportBatchDuplicateGuard(uniqueIndexes, valueVar = 'entity') {
  if (!uniqueIndexes.length) {
    return { declare: '', check: '' };
  }
  const idx = uniqueIndexes[0];
  if (!idx.fields.length) {
    return { declare: '', check: '' };
  }
  const fieldLabel = idx.fields.join('、');
  const keyInterpol = idx.fields.map((f) => `{${valueVar}.${f}}`).join('|');
  return {
    declare: '        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);\n',
    check:
      `                var importKey = $"${keyInterpol}";\n` +
      '                if (!importSeenKeys.Add(importKey))\n' +
      '                {\n' +
      `                    throw new TaktBusinessException("与Excel中其他行重复（${fieldLabel}）");\n` +
      '                }\n',
  };
}

/**
 * 主子表 SaveChildren 内子表批次组合键去重
 * @param {Array<{ indexKey: string, fields: string[] }>} uniqueIndexes
 * @param {string} listVar 子表列表变量名
 * @param {string} itemLabel 错误消息中的项名称
 * @returns {string[]}
 */
function buildChildBatchDuplicateGuardLines(uniqueIndexes, listVar, itemLabel) {
  if (!uniqueIndexes.length) {
    return [];
  }
  const idx = uniqueIndexes[0];
  if (!idx.fields.length) {
    return [];
  }
  const fieldLabel = idx.fields.join('、');
  const keyInterpol = idx.fields.map((f) => `{${listVar}[i].${f}}`).join('|');
  return [
    '            var seenKeys = new HashSet<string>(StringComparer.Ordinal);',
    `            for (var i = 0; i < ${listVar}.Count; i++)`,
    '            {',
    `                var key = $"${keyInterpol}";`,
    '                if (!seenKeys.Add(key))',
    '                {',
    `                    throw new TaktBusinessException($"${itemLabel}第{i + 1}项与本次提交的其他项重复（${fieldLabel}）");`,
    '                }',
    '            }',
  ];
}

function extractQueryDtoProperties(dtoContent, queryDtoName, entityPropertyNames) {
  const block = extractClassBlock(dtoContent, queryDtoName);
  const props = [];
  const propRegex = /public\s+([\w?<>,\s]+)\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    const rawType = m[1].trim();
    const name = m[2];
    if (PAGED_QUERY_FIELDS.has(name)) {
      continue;
    }
    if (name.endsWith('Start') || name.endsWith('End')) {
      continue;
    }
    if (
      !entityPropertyNames.has(name) &&
      name !== 'Remark' &&
      name !== 'ExtField' &&
      name !== 'CultureCode' &&
      name !== 'PlantCode' &&
      name !== 'RelatedPlant'
    ) {
      continue;
    }
    const bareType = rawType.replace('?', '').trim();
    const isSharedEnum = isSharedEnumType(bareType);
    props.push({
      name,
      rawType,
      bareType,
      isSharedEnum,
      isNullableEnum: isSharedEnum && rawType.endsWith('?'),
      isEnum: isSharedEnum,
      isString: rawType.startsWith('string'),
      isDateTime: rawType.includes('DateTime'),
      isBool: rawType.includes('bool'),
      isNumeric: /^(int|long|decimal)/.test(rawType) && !isSharedEnum,
    });
  }
  return props;
}

function extractDateRangeFields(dtoContent, queryDtoName, entityPropertyNames) {
  const block = extractClassBlock(dtoContent, queryDtoName);
  const ranges = [];
  const propRegex = /public\s+DateTime\?\s+(\w+)\s*\{/g;
  const startEndNames = [];
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    startEndNames.push(m[1]);
  }
  for (const fieldName of startEndNames) {
    if (fieldName.endsWith('Start')) {
      const baseName = fieldName.slice(0, -'Start'.length);
      const endField = `${baseName}End`;
      if (startEndNames.includes(endField) && (entityPropertyNames.has(baseName) || baseName === 'CreatedAt')) {
        ranges.push({ baseName, startField: fieldName, endField });
      }
    }
  }
  return ranges;
}

function parseStatusDtoFields(dtoContent, statusDtoName) {
  const block = extractClassBlock(dtoContent, statusDtoName);
  const idMatch = block.match(/public\s+long\s+(\w+Id)\s*\{/);
  const props = [];
  const propRegex = /public\s+([\w?<>,\s]+)\s+(\w+)\s*\{/g;
  let m;
  while ((m = propRegex.exec(block)) !== null) {
    if (m[2].endsWith('Id')) {
      continue;
    }
    props.push({ type: m[1].trim(), name: m[2] });
  }
  return {
    idProperty: idMatch ? idMatch[1] : null,
    valueProperties: props,
  };
}

function buildStatusMethodSuffix(statusDtoName, entityName) {
  let suffix = statusDtoName.replace(entityName, '').replace(/StatusDto$/, '');
  const entityShort = entityName.replace(/^Takt/, '');
  if (suffix.startsWith(entityShort)) {
    suffix = suffix.slice(entityShort.length);
  }
  if (!suffix) {
    suffix = 'Status';
  }
  return suffix;
}

function repositoryFieldName(entityShort) {
  return `_${entityShort.charAt(0).toLowerCase()}${entityShort.slice(1)}Repository`;
}

/**
 * 转置列头主表与 ManyToOne 主表相同时，避免重复注入同一仓储
 * @param {ReturnType<typeof generateManyToOneMasterStampExtras>|null} manyToOneMaster
 * @param {ReturnType<typeof generateTransposedServiceImplementation>|null} transposedGen
 * @param {string} entityShort
 */
function omitDuplicateTransposedCtorExtras(manyToOneMaster, transposedGen, entityShort) {
  if (!manyToOneMaster || !transposedGen) {
    return transposedGen;
  }
  const cfg = getTransposableConfig(entityShort);
  const stampMasterEntities =
    manyToOneMaster.masterEntities ??
    (manyToOneMaster.masterEntity ? [manyToOneMaster.masterEntity] : []);
  if (!cfg?.masterTable || !stampMasterEntities.includes(cfg.masterTable.entity)) {
    return transposedGen;
  }
  return {
    ...transposedGen,
    ctorFields: '',
    ctorParams: '',
    ctorAssigns: '',
    ctorParamDocs: '',
  };
}

/**
 * 实体是否含 IsBuiltIn（种子内置项，禁止删除/禁用）
 * @param {string} entityFile
 * @returns {boolean}
 */
function entityHasIsBuiltIn(entityFile) {
  return extractEntityPropertyNames(entityFile).has('IsBuiltIn');
}

/**
 * 创建时强制非内置
 * @returns {string}
 */
function buildBuiltInCreateAssignLine() {
  return '        entity.IsBuiltIn = 0;\n';
}

/**
 * 更新前保存 IsBuiltIn（及员工状态）快照
 * @param {{ field: string, kind: string }|null} builtInStatusMeta
 * @returns {string}
 */
function buildBuiltInUpdateBeforeAdaptLines(builtInStatusMeta) {
  let block = '        var originalIsBuiltIn = entity.IsBuiltIn;\n';
  if (builtInStatusMeta?.kind === 'employeeResigned') {
    block += '        var originalEmployeeStatus = entity.EmployeeStatus;\n';
  }
  return block;
}

/**
 * 更新后恢复 IsBuiltIn，并校验内置员工不可离职/退休
 * @param {string} desc
 * @param {{ field: string, kind: string }|null} builtInStatusMeta
 * @returns {string}
 */
function buildBuiltInUpdateAfterAdaptLines(desc, builtInStatusMeta) {
  let block = '        entity.IsBuiltIn = originalIsBuiltIn;\n';
  if (builtInStatusMeta?.kind === 'employeeResigned') {
    block += `        if (entity.IsBuiltIn == 1 && entity.EmployeeStatus != originalEmployeeStatus
            && (entity.EmployeeStatus == 3 || entity.EmployeeStatus == 4))
        {
            throw new TaktBusinessException("不允许将内置${desc}设为离职或退休");
        }
`;
  }
  return block;
}

/**
 * 删除内置项保护
 * @param {string} desc
 * @returns {string}
 */
function buildBuiltInDeleteGuardLines(desc) {
  return `        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置${desc}不允许删除");
        }
`;
}

/**
 * 批量删除前：所选 ID 中不得含内置项（整批拒绝，避免部分删除后失败）
 * @param {string} desc
 * @param {string} repoField
 * @returns {string}
 */
function buildBuiltInBatchDeleteGuardLines(desc, repoField) {
  return `        if (await ${repoField}.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置${desc}不允许删除");
        }
`;
}

/**
 * 状态更新：禁止将内置项设为禁用/非启用
 * @param {string} desc
 * @param {{ field: string, kind: string }} builtInStatusMeta
 * @param {string} dtoPropName StatusDto 中的状态属性名
 * @returns {string}
 */
function buildBuiltInStatusDisableGuardLines(desc, builtInStatusMeta, dtoPropName) {
  if (!builtInStatusMeta || builtInStatusMeta.field !== dtoPropName) {
    return '';
  }
  if (builtInStatusMeta.kind === 'intEnabled') {
    return `        if (entity.IsBuiltIn == 1 && dto.${dtoPropName} != 1)
        {
            throw new TaktBusinessException("不允许禁用内置${desc}");
        }
`;
  }
  return '';
}

/**
 * 树形选项/树列表：按 parentId 只查直接子级（含租户/公司/启用态）
 * @param {string} dtoBase
 * @param {ReturnType<typeof extractPrimaryEnableStatusMeta>} statusMeta
 * @param {boolean} [hasIsObsolete]
 * @param {{ forIncludeDisabled?: boolean }} [opts] forIncludeDisabled=true 时生成三元 Expression 用的启用谓词片段
 */
function buildLazyTreeChildPredicate(dtoBase, statusMeta, hasIsObsolete = false) {
  const scope = buildTenantCompanyScope(dtoBase, 'x');
  const obsoletePart = hasIsObsolete ? ' && x.IsObsolete == 0' : '';
  const base = `${scope} && x.ParentId == parentId${obsoletePart}`;
  if (statusMeta?.kind === 'int') {
    const enabled = statusMeta.intEnabled ?? 1;
    const statusCmp = statusMeta.isEnum
      ? `(int)x.${statusMeta.field} == ${enabled}`
      : `x.${statusMeta.field} == ${enabled}`;
    return {
      withStatus: `x => ${base} && ${statusCmp}`,
      withoutStatus: `x => ${base}`,
      statusField: statusMeta.field,
      enabledValue: enabled,
    };
  }
  return {
    withStatus: `x => ${base}`,
    withoutStatus: `x => ${base}`,
    statusField: null,
    enabledValue: 1,
  };
}

/**
 * 生成 GetXxxTreeOptionsAsync、GetXxxTreeAsync（懒加载一层；不生成递归 Build*Tree）
 * 参照 TaktAdminDivisionService + TaktLazyTreeHelper
 */
function generateTreeServiceMethods(
  entityName,
  entityShort,
  dtoInfo,
  dtoBase,
  repoField,
  entityFile,
  desc,
  nameField,
) {
  const treeDto = dtoInfo.tree;
  const entityContent = readUtf8(entityFile);
  const statusMeta = extractPrimaryEnableStatusMeta(entityContent, entityShort);
  const hasSortOrder = /public\s+int\s+SortOrder\s*\{/.test(entityContent);
  const hasIsLeaf = /public\s+int\s+IsLeaf\s*\{/.test(entityContent);
  const hasIsObsolete = entityFileHasIsObsolete(entityFile);
  const ensureLine = buildEnsureContextLine(dtoBase);
  const pred = buildLazyTreeChildPredicate(dtoBase, statusMeta, hasIsObsolete);
  const sortOrderAssign = hasSortOrder
    ? '                    SortOrder = item.SortOrder,\n'
    : '                    SortOrder = 0,\n';
  const isLeafAssign = hasIsLeaf
    ? `                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);\n`
    : '                var isLeaf = false;\n';

  let treeOptionsBlock = '';
  treeOptionsBlock += buildMethodXmlDoc({
    summary: `获取${desc}树形选项列表（懒加载：仅 parentId 直接子级一层）`,
    params: [
      { name: 'parentId', desc: '父级ID（0=根）' },
      { name: 'plantCode', desc: '工厂代码（可选，用于按工厂过滤）' },
      { name: 'keyword', desc: '搜索关键字（可选，模糊匹配）' },
    ],
    returns: '树形选项（一层）',
  });
  treeOptionsBlock += `    public async Task<List<TaktTreeSelectOption>> Get${entityShort}TreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)\n`;
  treeOptionsBlock += '    {\n';
  treeOptionsBlock += ensureLine;
  treeOptionsBlock += '        var normalizedPlantCode = plantCode?.Trim();\n';
  treeOptionsBlock += '        var normalizedKeyword = keyword?.Trim();\n';
  const plantField = resolveOptionsPlantField(entityContent);
  treeOptionsBlock += `        var predicate = Expressionable.Create<${entityName}>()\n`;
  treeOptionsBlock += `            .And(${pred.withStatus})\n`;
  if (plantField) {
    treeOptionsBlock += `            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.${plantField} == normalizedPlantCode)\n`;
  }
  treeOptionsBlock += `            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>\n`;
  treeOptionsBlock += `                (x.${nameField} != null && x.${nameField}.Contains(normalizedKeyword!)))\n`;
  treeOptionsBlock += '            .ToExpression();\n';
  treeOptionsBlock += `        var list = await ${repoField}.GetListAsync(predicate);\n`;
  treeOptionsBlock += '        return list\n';
  treeOptionsBlock += hasSortOrder
    ? '            .OrderBy(x => x.SortOrder)\n'
    : '            .OrderBy(x => x.Id)\n';
  treeOptionsBlock += '            .Select(item =>\n';
  treeOptionsBlock += '            {\n';
  treeOptionsBlock += isLeafAssign;
  treeOptionsBlock += '                return new TaktTreeSelectOption\n';
  treeOptionsBlock += '                {\n';
  treeOptionsBlock += '                    DictValue = item.Id.ToString(),\n';
  // DictLabel：禁止回退雪花 Id；无 *Name 时用业务 Code（与平铺 Options 一致）
  treeOptionsBlock += `                    DictLabel = item.${nameField},\n`;
  treeOptionsBlock += sortOrderAssign;
  treeOptionsBlock += '                    IsLeaf = isLeaf,\n';
  treeOptionsBlock += '                    Children = null,\n';
  treeOptionsBlock += '                };\n';
  treeOptionsBlock += '            })\n';
  treeOptionsBlock += '            .ToList();\n';
  treeOptionsBlock += '    }\n\n';

  let treeRemainderBlock = '';
  treeRemainderBlock += buildMethodXmlDoc({
    summary: `获取${desc}树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）`,
    params: [
      { name: 'parentId', desc: '父级ID（0=根）' },
      { name: 'includeDisabled', desc: '是否包含禁用项' },
    ],
    returns: '树形列表（一层）',
  });
  treeRemainderBlock += `    public async Task<List<${treeDto}>> Get${entityShort}TreeAsync(long parentId = 0, bool includeDisabled = false)\n`;
  treeRemainderBlock += '    {\n';
  treeRemainderBlock += ensureLine;
  if (pred.statusField) {
    treeRemainderBlock += `        Expression<Func<${entityName}, bool>> predicate = includeDisabled\n`;
    treeRemainderBlock += `            ? (${pred.withoutStatus})\n`;
    treeRemainderBlock += `            : (${pred.withStatus});\n`;
    treeRemainderBlock += `        var list = await ${repoField}.GetListAsync(predicate);\n`;
  } else {
    treeRemainderBlock += `        var list = await ${repoField}.GetListAsync(${pred.withoutStatus});\n`;
  }
  treeRemainderBlock += '        return list\n';
  treeRemainderBlock += hasSortOrder
    ? '            .OrderBy(x => x.SortOrder)\n'
    : '            .OrderBy(x => x.Id)\n';
  treeRemainderBlock += '            .Select(item =>\n';
  treeRemainderBlock += '            {\n';
  treeRemainderBlock += `                var treeDto = item.Adapt<${treeDto}>();\n`;
  treeRemainderBlock += `                treeDto.Children = null;\n`;
  treeRemainderBlock += '                return treeDto;\n';
  treeRemainderBlock += '            })\n';
  treeRemainderBlock += '            .ToList();\n';
  treeRemainderBlock += '    }\n\n';

  return {
    treeOptionsBlock,
    treeRemainderBlock,
    block: treeOptionsBlock + treeRemainderBlock,
    needsLazyTreeHelper: true,
    needsEnumsUsing: false,
  };
}

function todayFileHeaderDate() {
  return new Date().toISOString().split('T')[0];
}

// ========================================
// 接口 / 实现生成
// ========================================

/**
 * 生成公共方法 XML 注释（实现类与 TaktLoginLogService 一致，禁止 inheritdoc）
 * @param {{ summary: string, params?: Array<{ name: string, desc: string }>, returns?: string }} doc
 * @returns {string}
 */
function buildMethodXmlDoc(doc) {
  let block = '    /// <summary>\n';
  block += `    /// ${doc.summary}\n`;
  block += '    /// </summary>\n';
  if (doc.params) {
    for (const p of doc.params) {
      block += `    /// <param name="${p.name}">${p.desc}</param>\n`;
    }
  }
  if (doc.returns != null && doc.returns !== '') {
    block += `    /// <returns>${doc.returns}</returns>\n`;
  }
  return block;
}

function generateServiceInterface(
  entityName,
  dtoInfo,
  description,
  entityFile,
  crudType,
  existingContent = null,
) {
  const entityShort = entityName.replace(/^Takt/, '');
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceNs = buildNamespace('Takt.Application.Services', moduleParts);
  const dtoNs = buildNamespace('Takt.Application.Dtos', moduleParts);
  const desc = description || entityShort;
  const hasTree = crudType === 'Tree' || dtoInfo.tree !== null;
  const preservedOptionsMethods = [];

  let content = '';
  content += '// ========================================\n';
  content += '// 项目名称：节拍智控·TaktWise\n';
  content += `// 命名空间：${serviceNs}\n`;
  content += `// 文件名称：I${entityName}Service.cs\n`;
  content += `// 创建时间：${todayFileHeaderDate()}\n`;
  content += '// 创建人：Takt365(Cursor AI)\n';
  content += `// 功能描述：${desc}应用服务接口\n`;
  content += '// \n';
  content += `// 版权信息：Copyright (c) ${new Date().getFullYear()} Takt  All rights reserved.\n`;
  content += '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。\n';
  content += '// ========================================\n\n';
  content += `using ${dtoNs};\n`;
  content += 'using Takt.Shared.Models;\n';
  content += 'using Takt.Shared.Options;\n';
  const preservedExtraIfacePreview = extractPreservedExtraServiceMethods(
    existingContent,
    entityShort,
    dtoInfo,
    crudType,
    'interface',
  );
  for (const ns of collectPreservedExtraUsings(entityShort, preservedExtraIfacePreview.blocks)) {
    content += `using ${ns};\n`;
  }
  content += '\n';
  content += `namespace ${serviceNs};\n\n`;
  content += '/// <summary>\n';
  content += `/// ${desc}应用服务接口\n`;
  content += '/// </summary>\n';
  content += `public interface I${entityName}Service\n`;
  content += '{\n';

  content += buildMethodXmlDoc({
    summary: `获取${desc}列表（分页）`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '分页结果',
  });
  content += `    Task<TaktPagedResult<${dtoInfo.base}>> Get${entityShort}ListAsync(${dtoInfo.query} queryDto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `根据ID获取${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}?> Get${entityShort}ByIdAsync(long id);\n\n`;

  const optionsIface = buildGetOptionsAsyncInterfaceSection(
    entityShort,
    hasTree,
    dtoInfo,
    desc,
    existingContent,
  );
  content += optionsIface.block;
  if (optionsIface.preserved) {
    preservedOptionsMethods.push(optionsIface.methodName);
  }

  if (hasTree && dtoInfo.tree) {
    content += buildMethodXmlDoc({
      summary: `获取${desc}树形列表（懒加载：仅 parentId 直接子级一层）`,
      params: [
        { name: 'parentId', desc: '父级ID（0=根）' },
        { name: 'includeDisabled', desc: '是否包含禁用项' },
      ],
      returns: '树形列表（一层）',
    });
    content += `    Task<List<${dtoInfo.tree}>> Get${entityShort}TreeAsync(long parentId = 0, bool includeDisabled = false);\n\n`;
  }

  content += buildMethodXmlDoc({
    summary: `创建${desc}`,
    params: [{ name: 'dto', desc: '创建DTO' }],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}> Create${entityShort}Async(${dtoInfo.create} dto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `更新${desc}`,
    params: [
      { name: 'id', desc: `${desc}ID` },
      { name: 'dto', desc: '更新DTO' },
    ],
    returns: 'DTO',
  });
  content += `    Task<${dtoInfo.base}> Update${entityShort}Async(long id, ${dtoInfo.update} dto);\n\n`;

  content += buildMethodXmlDoc({
    summary: `删除${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: '任务',
  });
  content += `    Task Delete${entityShort}ByIdAsync(long id);\n\n`;

  content += buildMethodXmlDoc({
    summary: `批量删除${desc}`,
    params: [{ name: 'ids', desc: 'ID列表' }],
    returns: '任务',
  });
  content += `    Task Delete${entityShort}BatchAsync(IEnumerable<long> ids);\n\n`;

  for (const statusDto of dtoInfo.statuses) {
    const suffix = buildStatusMethodSuffix(statusDto, entityName);
    const statusLabel = suffix === 'Status' ? '状态' : suffix;
    content += buildMethodXmlDoc({
      summary: `更新${desc}${statusLabel}`,
      params: [{ name: 'dto', desc: '状态DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}${suffix}Async(${statusDto} dto);\n\n`;
  }

  if (dtoInfo.builtIn) {
    content += buildMethodXmlDoc({
      summary: `更新${desc}内置`,
      params: [{ name: 'dto', desc: '内置 DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}BuiltInAsync(${dtoInfo.builtIn} dto);\n\n`;
  }

  if (dtoInfo.sort) {
    content += buildMethodXmlDoc({
      summary: `更新${desc}排序`,
      params: [{ name: 'dto', desc: '排序DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}SortAsync(${dtoInfo.sort} dto);\n\n`;
  }

  if (dtoInfo.obsolete) {
    content += buildMethodXmlDoc({
      summary: `更新${desc}作废状态`,
      params: [{ name: 'dto', desc: '作废DTO' }],
      returns: 'DTO',
    });
    content += `    Task<${dtoInfo.base}> Update${entityShort}ObsoleteAsync(${dtoInfo.obsolete} dto);\n\n`;
  }

  if (dtoInfo.template) {
    content += buildMethodXmlDoc({
      summary: '获取导入模板',
      params: [
        { name: 'sheetName', desc: '工作表名称' },
        { name: 'fileName', desc: '文件名' },
      ],
      returns: 'Excel 文件',
    });
    content += `    Task<(string fileName, byte[] content)> Get${entityShort}TemplateAsync(string? sheetName = null, string? fileName = null);\n\n`;
  }

  if (dtoInfo.import) {
    content += buildMethodXmlDoc({
      summary: `导入${desc}`,
      params: [
        { name: 'fileStream', desc: 'Excel 文件流' },
        { name: 'sheetName', desc: '工作表名称' },
      ],
      returns: '导入结果',
    });
    content += `    Task<(int success, int fail, List<string> errors)> Import${entityShort}Async(Stream fileStream, string? sheetName = null);\n\n`;
  }

  content += buildMethodXmlDoc({
    summary: `导出${desc}`,
    params: [
      { name: 'query', desc: '查询条件' },
      { name: 'sheetName', desc: '工作表名称' },
      { name: 'fileName', desc: '文件名' },
    ],
    returns: 'Excel 文件',
  });
  content += `    Task<(string fileName, byte[] fileContent)> Export${entityShort}Async(${dtoInfo.query}? query = null, string? sheetName = null, string? fileName = null);\n\n`;

  if (isTransposableEntity(entityShort)) {
    content += generateTransposedInterfaceMethods(entityShort, desc);
  }

  const preservedExtraIface = extractPreservedExtraServiceMethods(
    existingContent,
    entityShort,
    dtoInfo,
    crudType,
    'interface',
  );
  if (preservedExtraIface.blocks) {
    content += '    // ========================================\n';
    content += '    // 扩展方法（保留）\n';
    content += '    // ========================================\n\n';
    content += preservedExtraIface.blocks;
  }

  content += '}\n';
  return { content, serviceNs, dtoNs, preservedOptionsMethods, preservedExtraMethods: preservedExtraIface.methodNames };
}

/**
 * 汇总 QueryDto 中参与 KeyWords 模糊匹配的实体字段（与独立查询条件字段一致）
 * @param {Array} queryProps 查询 DTO 属性（不含范围 Start/End）
 * @param {Array} dateRanges 日期范围字段
 * @returns {Array<{ name: string, isString: boolean }>}
 */
function collectKeyWordsSearchFields(queryProps, dateRanges) {
  const fields = [];
  const seen = new Set();
  // 仅字符串业务字段；禁止 SqlFunc.ToString(CreatedAt).Contains —— 短关键字会命中几乎全表
  for (const prop of queryProps) {
    if (!prop.isString || seen.has(prop.name)) {
      continue;
    }
    seen.add(prop.name);
    fields.push({ name: prop.name, isString: true });
  }
  void dateRanges;
  return fields;
}

/**
 * 生成 KeyWords 模糊匹配代码块（覆盖 QueryDto 全部可搜索项）
 * @param {Array} queryProps 查询 DTO 属性
 * @param {Array} dateRanges 日期范围字段
 * @returns {string[]}
 */
function buildKeyWordsExpressionLines(queryProps, dateRanges) {
  const keyWordFields = collectKeyWordsSearchFields(queryProps, dateRanges);
  if (keyWordFields.length === 0) {
    return [];
  }
  const lines = [];
  lines.push('        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))');
  lines.push('        {');
  lines.push('            var keywords = queryDto.KeyWords!.Trim();');
  lines.push('            exp = exp.And(x =>');
  keyWordFields.forEach((field, index) => {
    const prefix = index === 0 ? '                ' : '                || ';
    if (field.isString) {
      lines.push(`${prefix}(x.${field.name} != null && x.${field.name}.Contains(keywords))`);
      return;
    }
    lines.push(`${prefix}SqlFunc.ToString(x.${field.name}).Contains(keywords)`);
  });
  lines.push('            );');
  lines.push('        }');
  lines.push('');
  return lines;
}

/**
 * 生成「是否存在任一业务查询条件」方法（分页字段除外；无参时列表/导出返回空）
 * @param {string} queryDtoTypeName QueryDto 类型名
 * @param {Array} queryProps 查询属性
 * @param {Array} dateRanges 日期范围
 * @param {boolean} hasIsObsolete 是否含 IsObsolete（未传值不视为用户条件）
 * @returns {string}
 */
function buildHasAnyListQueryFilterMethod(
  queryDtoTypeName,
  queryProps,
  dateRanges,
  hasIsObsolete = false,
) {
  const lines = [];
  lines.push('    /// <summary>');
  lines.push('    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描');
  lines.push('    /// </summary>');
  lines.push('    /// <param name="queryDto">查询 DTO</param>');
  lines.push('    /// <returns>有条件为 true</returns>');
  lines.push(`    private static bool HasAnyListQueryFilter(${queryDtoTypeName}? queryDto)`);
  lines.push('    {');
  lines.push('        if (queryDto == null)');
  lines.push('        {');
  lines.push('            return false;');
  lines.push('        }');
  lines.push('        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))');
  lines.push('        {');
  lines.push('            return true;');
  lines.push('        }');
  const loopProps = hasIsObsolete ? queryProps.filter((p) => p.name !== 'IsObsolete') : queryProps;
  for (const prop of loopProps) {
    if (prop.isString) {
      lines.push(`        if (!string.IsNullOrWhiteSpace(queryDto.${prop.name}))`);
      lines.push('        {');
      lines.push('            return true;');
      lines.push('        }');
      continue;
    }
    if (prop.isDateTime || prop.isSharedEnum || prop.isNullableEnum || prop.isBool || prop.isNumeric) {
      lines.push(`        if (queryDto.${prop.name}.HasValue)`);
      lines.push('        {');
      lines.push('            return true;');
      lines.push('        }');
    }
  }
  if (hasIsObsolete) {
    lines.push('        if (queryDto.IsObsolete.HasValue)');
    lines.push('        {');
    lines.push('            return true;');
    lines.push('        }');
  }
  for (const range of dateRanges) {
    lines.push(`        if (queryDto.${range.startField}.HasValue || queryDto.${range.endField}.HasValue)`);
    lines.push('        {');
    lines.push('            return true;');
    lines.push('        }');
  }
  lines.push('        return false;');
  lines.push('    }');
  lines.push('');
  return lines.join('\n');
}

/**
 * 生成 QueryExpression 方法体（SqlSugar Expressionable，租户/公司隔离由仓储 Where 处理）
 * @param {string} entityName 实体类名
 * @param {Array} queryProps 查询 DTO 属性（不含范围 Start/End）
 * @param {Array} dateRanges 日期范围字段
 * @param {boolean} [hasIsObsolete=false]
 * @returns {string} 方法体 C# 代码
 */
function buildQueryExpressionBody(entityName, queryProps, dateRanges, hasIsObsolete = false) {
  const lines = [];
  lines.push(`        var exp = Expressionable.Create<${entityName}>();`);
  lines.push('');
  if (hasIsObsolete) {
    lines.push('        if (queryDto?.IsObsolete.HasValue == true)');
    lines.push('        {');
    lines.push('            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);');
    lines.push('        }');
    lines.push('        else');
    lines.push('        {');
    lines.push('            exp = exp.And(x => x.IsObsolete == 0);');
    lines.push('        }');
    lines.push('');
  }
  const loopProps = hasIsObsolete ? queryProps.filter((p) => p.name !== 'IsObsolete') : queryProps;
  lines.push(...buildKeyWordsExpressionLines(loopProps, dateRanges));

  for (const prop of loopProps) {
    if (prop.isString) {
      // 局部变量捕获：避免 SqlSugar 无法翻译 queryDto.Xxx 导致条件丢失（表现为有参却查出全表）
      const localName = prop.name.charAt(0).toLowerCase() + prop.name.slice(1);
      lines.push(`        if (!string.IsNullOrWhiteSpace(queryDto?.${prop.name}))`);
      lines.push('        {');
      lines.push(`            var ${localName} = queryDto.${prop.name};`);
      lines.push(
        `            exp = exp.And(x => x.${prop.name} != null && x.${prop.name}.Contains(${localName}));`,
      );
      lines.push('        }');
      lines.push('');
      continue;
    }

    if (prop.isDateTime) {
      // 必须用 .Value：SqlSugar 无法可靠翻译 long?/DateTime? 与实体非空列比较（表现为有参却查出全表）
      lines.push(`        if (queryDto?.${prop.name}.HasValue == true)`);
      lines.push('        {');
      lines.push(`            var ${prop.name.charAt(0).toLowerCase() + prop.name.slice(1)} = queryDto.${prop.name}.Value;`);
      lines.push(`            exp = exp.And(x => x.${prop.name} == ${prop.name.charAt(0).toLowerCase() + prop.name.slice(1)});`);
      lines.push('        }');
      lines.push('');
      continue;
    }

    if (prop.isSharedEnum || prop.isNullableEnum || prop.isBool || prop.isNumeric) {
      // 必须用 .Value：可空数值局部变量参与表达式时 SqlSugar 常丢条件 → 有外键却返回全表
      lines.push(`        if (queryDto?.${prop.name}.HasValue == true)`);
      lines.push('        {');
      const localName = prop.name.charAt(0).toLowerCase() + prop.name.slice(1);
      lines.push(`            var ${localName} = queryDto.${prop.name}.Value;`);
      lines.push(`            exp = exp.And(x => x.${prop.name} == ${localName});`);
      lines.push('        }');
      lines.push('');
    }
  }

  for (const range of dateRanges) {
    const startLocal = range.startField.charAt(0).toLowerCase() + range.startField.slice(1);
    const endLocal = range.endField.charAt(0).toLowerCase() + range.endField.slice(1);
    lines.push(`        if (queryDto?.${range.startField}.HasValue == true)`);
    lines.push('        {');
    lines.push(`            var ${startLocal} = queryDto.${range.startField}.Value;`);
    lines.push(`            exp = exp.And(x => x.${range.baseName} >= ${startLocal});`);
    lines.push('        }');
    lines.push('');
    lines.push(`        if (queryDto?.${range.endField}.HasValue == true)`);
    lines.push('        {');
    lines.push(`            var ${endLocal} = queryDto.${range.endField}.Value;`);
    lines.push(`            exp = exp.And(x => x.${range.baseName} <= ${endLocal});`);
    lines.push('        }');
    lines.push('');
  }

  lines.push('        return exp.ToExpression();');
  return lines.join('\n');
}

function generateServiceImplementation(
  entityName,
  dtoInfo,
  description,
  entityFile,
  crudType,
  dtoBase,
  existingContent = null,
  genOptions = {},
) {
  const entityShort = entityName.replace(/^Takt/, '');
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceNs = buildNamespace('Takt.Application.Services', moduleParts);
  const dtoNs = buildNamespace('Takt.Application.Dtos', moduleParts);
  const entityNs = getEntityNamespace(entityFile);
  const repoInterface = DTO_BASE_TO_REPOSITORY[dtoBase] || 'ITaktTenantRepository';
  const repoField = repositoryFieldName(entityShort);
  const desc = description || entityShort;
  const preservedOptionsMethods = [];
  const regeneratedOptionsMethods = [];
  const dtoContent = readUtf8(findDtoFileByEntity(entityName));
  const entityContent = readUtf8(entityFile);
  const entityProps = extractEntityPropertyNames(entityFile);
  const entityScalarProps = parseEntityScalarProperties(entityContent);
  const queryProps = extractQueryDtoProperties(dtoContent, dtoInfo.query, entityProps);
  const dateRanges = extractDateRangeFields(dtoContent, dtoInfo.query, entityProps);
  const enableStatusMeta = extractPrimaryEnableStatusMeta(entityContent, entityShort);
  const { nameField, valueField: optionsValueField, valueAsString: optionsValueAsString } =
    resolveOptionsDisplayFields(entityFile, entityShort, desc);
  const importDtoName = dtoInfo.import;
  const uniqueIndexes = extractUniqueIndexes(entityFile, dtoBase);
  const manyToOneMasterEarly =
    crudType === 'Single'
      ? generateManyToOneMasterStampExtras(
          entityFile,
          entityName,
          entityShort,
          dtoInfo,
          desc,
          dtoContent,
        )
      : null;
  const createUniqueBlock = buildUniqueValidationBlock(
    uniqueIndexes,
    repoField,
    desc,
    'create',
    'entity',
  );
  const updateUniqueBlock = buildUniqueValidationBlock(
    uniqueIndexes,
    repoField,
    desc,
    'update',
    'entity',
  );
  const importUniqueBlock = importDtoName
    ? buildUniqueValidationBlock(uniqueIndexes, repoField, desc, 'import', 'entity')
    : '';
  const importBatchGuard = importDtoName
    ? buildImportBatchDuplicateGuard(uniqueIndexes, 'entity')
    : { declare: '', check: '' };
  const hasTree = crudType === 'Tree' || dtoInfo.tree !== null;
  const treeGen = hasTree && dtoInfo.tree
    ? generateTreeServiceMethods(
        entityName,
        entityShort,
        dtoInfo,
        dtoBase,
        repoField,
        entityFile,
        desc,
        nameField,
      )
    : null;
  const rbacDelegation = hasRbacParentConfig(entityShort)
    ? generateRbacParentDelegationExtras(
        entityShort,
        dtoInfo,
        repoField,
        entityFile,
        desc,
        buildBuiltInDeleteGuardLines,
      )
    : null;
  const masterDetail =
    crudType === 'MasterDetail'
      ? generateMasterDetailServiceExtras(
          entityFile,
          entityName,
          entityShort,
          dtoInfo,
          dtoBase,
          repoField,
          desc,
        )
      : null;
  const sequenceMeta = buildSequenceMetaForService({
    entityFile,
    entityShort,
    crudType,
    dtoBase,
    masterDetail,
  });
  const manyToOneMaster = manyToOneMasterEarly;
  const transposedGenRaw = isTransposableEntity(entityShort)
    ? generateTransposedServiceImplementation(entityShort, desc, repoField, entityName, dtoBase)
    : null;
  const transposedGen = omitDuplicateTransposedCtorExtras(manyToOneMaster, transposedGenRaw, entityShort);
  const templateDto = dtoInfo.template;
  const importDto = dtoInfo.import;
  const exportDto = dtoInfo.export || dtoInfo.base;
  const hasIsObsolete = entityFileHasIsObsolete(entityFile);
  const queryExprBody = buildQueryExpressionBody(
    entityName,
    queryProps,
    dateRanges,
    hasIsObsolete,
  );

  const entityScopeGuard = buildEntityScopeGuard(dtoBase);
  const optionsListPredicate = buildOptionsListPredicate(dtoBase, enableStatusMeta, hasIsObsolete);
  const ensureContextLine = buildEnsureContextLine(dtoBase);
  const hasBuiltIn = entityHasIsBuiltIn(entityFile);
  const builtInStatusMeta = hasBuiltIn ? extractBuiltInDisableStatusMeta(entityContent) : null;
  const numberingCodeProp = findTaktNumberingAutoFormCodeProperty(entityFile);
  const needsNumberingGenerator = Boolean(numberingCodeProp);
  const needsSharedEnumsUsing = false;

  let content = '';
  content += '// ========================================\n';
  content += '// 项目名称：节拍智控·TaktWise\n';
  content += `// 命名空间：${serviceNs}\n`;
  content += `// 文件名称：${entityName}Service.cs\n`;
  content += `// 创建时间：${todayFileHeaderDate()}\n`;
  content += '// 创建人：Takt365(Cursor AI)\n';
  content += `// 功能描述：${desc}应用服务实现\n`;
  content += '// \n';
  content += `// 版权信息：Copyright (c) ${new Date().getFullYear()} Takt  All rights reserved.\n`;
  content += '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。\n';
  content += '// ========================================\n\n';
  content += 'using System.Linq.Expressions;\n';
  content += 'using Mapster;\n';
  content += 'using SqlSugar;\n';
  content += `using ${dtoNs};\n`;
  content += `using ${entityNs};\n`;
  content += 'using Takt.Domain.Interfaces;\n';
  content += 'using Takt.Domain.Repositories;\n';
  content += 'using Takt.Shared.Exceptions;\n';
  content += 'using Takt.Shared.Helpers;\n';
  content += 'using Takt.Shared.Models;\n';
  content += 'using Takt.Shared.Options;\n';
  const preservedExtraImplPreview = extractPreservedExtraServiceMethods(
    existingContent,
    entityShort,
    dtoInfo,
    crudType,
    'implementation',
  );
  for (const ns of collectPreservedExtraUsings(entityShort, preservedExtraImplPreview.blocks)) {
    content += `using ${ns};\n`;
  }
  if (needsSharedEnumsUsing) {
    content += 'using Takt.Shared.Enums;\n';
  }
  const extraUsingNs = new Set([
    ...(rbacDelegation?.extraUsings ?? []),
    ...(masterDetail?.extraUsings ?? []),
    ...(manyToOneMaster?.extraUsings ?? []),
  ].map((ns) => ns.replace(/^\s*using\s+/, '').replace(/;\s*$/, '').trim()).filter(Boolean));
  extraUsingNs.forEach((ns) => {
    if (ns !== dtoNs && ns !== serviceNs && ns !== entityNs) {
      content += `using ${ns};\n`;
    }
  });
  content += '\n';
  content += `namespace ${serviceNs};\n\n`;
  content += '/// <summary>\n';
  content += `/// ${desc}应用服务\n`;
  content += '/// </summary>\n';
  content += `public class ${entityName}Service : TaktServiceBase, I${entityName}Service\n`;
  content += '{\n';
  content += `    private readonly ${repoInterface}<${entityName}> ${repoField};\n`;
  if (rbacDelegation?.ctorFields) {
    content += `${rbacDelegation.ctorFields}\n`;
  }
  if (masterDetail?.ctorFields) {
    content += `${masterDetail.ctorFields}\n`;
  }
  if (manyToOneMaster?.ctorFields) {
    content += `${manyToOneMaster.ctorFields}\n`;
  }
  if (transposedGen?.ctorFields) {
    content += `${transposedGen.ctorFields}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '    private readonly ITaktSortOrderGenerator _sortOrderGenerator;\n';
  }
  if (sequenceMeta.needsLine) {
    content += '    private readonly ITaktLineNumberGenerator _lineNumberGenerator;\n';
  }
  if (needsNumberingGenerator) {
    content += '    private readonly ITaktNumberingGenerator _numberingGenerator;\n';
  }
  content += '    private readonly ITaktUniqueValidator _uniqueValidator;\n\n';
  content += '    /// <summary>\n';
  content += '    /// 构造函数\n';
  content += '    /// </summary>\n';
  content += `    /// <param name="${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository">${desc}仓储</param>\n`;
  if (rbacDelegation?.ctorParamDocs) {
    content += `${rbacDelegation.ctorParamDocs}\n`;
  }
  if (masterDetail?.ctorParamDocs) {
    content += `${masterDetail.ctorParamDocs}\n`;
  }
  if (manyToOneMaster?.ctorParamDocs) {
    content += `${manyToOneMaster.ctorParamDocs}\n`;
  }
  if (transposedGen?.ctorParamDocs) {
    content += `${transposedGen.ctorParamDocs}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '    /// <param name="sortOrderGenerator">排序号生成器</param>\n';
  }
  if (sequenceMeta.needsLine) {
    content += '    /// <param name="lineNumberGenerator">明细行号生成器</param>\n';
  }
  if (needsNumberingGenerator) {
    content += '    /// <param name="numberingGenerator">编码生成器</param>\n';
  }
  content += '    /// <param name="uniqueValidator">唯一性验证器</param>\n';
  content += '    /// <param name="userContext">用户上下文</param>\n';
  content += '    /// <param name="localizationService">本地化服务</param>\n';
  content += `    public ${entityName}Service(\n`;
  content += `        ${repoInterface}<${entityName}> ${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository,\n`;
  if (rbacDelegation?.ctorParams) {
    content += `${rbacDelegation.ctorParams}\n`;
  }
  if (masterDetail?.ctorParams) {
    content += `${masterDetail.ctorParams}\n`;
  }
  if (manyToOneMaster?.ctorParams) {
    content += `${manyToOneMaster.ctorParams}\n`;
  }
  if (transposedGen?.ctorParams) {
    content += `${transposedGen.ctorParams}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '        ITaktSortOrderGenerator sortOrderGenerator,\n';
  }
  if (sequenceMeta.needsLine) {
    content += '        ITaktLineNumberGenerator lineNumberGenerator,\n';
  }
  if (needsNumberingGenerator) {
    content += '        ITaktNumberingGenerator numberingGenerator,\n';
  }
  content += '        ITaktUniqueValidator uniqueValidator,\n';
  content += '        ITaktUserContext? userContext = null,\n';
  content += '        ITaktLocalizationService? localizationService = null)\n';
  content += '        : base(userContext, localizationService)\n';
  content += '    {\n';
  content += `        ${repoField} = ${entityShort.charAt(0).toLowerCase() + entityShort.slice(1)}Repository;\n`;
  if (rbacDelegation?.ctorAssigns) {
    content += `${rbacDelegation.ctorAssigns}\n`;
  }
  if (masterDetail?.ctorAssigns) {
    content += `${masterDetail.ctorAssigns}\n`;
  }
  if (manyToOneMaster?.ctorAssigns) {
    content += `${manyToOneMaster.ctorAssigns}\n`;
  }
  if (transposedGen?.ctorAssigns) {
    content += `${transposedGen.ctorAssigns}\n`;
  }
  if (sequenceMeta.needsSort) {
    content += '        _sortOrderGenerator = sortOrderGenerator;\n';
  }
  if (sequenceMeta.needsLine) {
    content += '        _lineNumberGenerator = lineNumberGenerator;\n';
  }
  if (needsNumberingGenerator) {
    content += '        _numberingGenerator = numberingGenerator;\n';
  }
  content += '        _uniqueValidator = uniqueValidator;\n';
  content += '    }\n\n';

  // List（无业务查询条件 → 空结果；有条件 → QueryExpression + 分页）
  content += buildMethodXmlDoc({
    summary: `获取${desc}列表（分页；无业务查询条件时返回空结果）`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '分页结果',
  });
  content += `    public async Task<TaktPagedResult<${dtoInfo.base}>> Get${entityShort}ListAsync(${dtoInfo.query} queryDto)\n`;
  content += '    {\n';
  content += '        if (!HasAnyListQueryFilter(queryDto))\n';
  content += '        {\n';
  content += `            return TaktPagedResult<${dtoInfo.base}>.Create(\n`;
  content += `                new List<${dtoInfo.base}>(),\n`;
  content += '                0,\n';
  content += '                queryDto.PageIndex,\n';
  content += '                queryDto.PageSize);\n';
  content += '        }\n';
  content += '        var predicate = QueryExpression(queryDto);\n';
  content += `        var (data, total) = await ${repoField}.GetPagedAsync(\n`;
  content += '            queryDto.PageIndex,\n';
  content += '            queryDto.PageSize,\n';
  content += '            predicate);\n';
  content += `        return TaktPagedResult<${dtoInfo.base}>.Create(\n`;
  content += `            data.Adapt<List<${dtoInfo.base}>>(),\n`;
  content += '            total,\n';
  content += '            queryDto.PageIndex,\n';
  content += '            queryDto.PageSize);\n';
  content += '    }\n\n';

  // GetById
  content += buildMethodXmlDoc({
    summary: `根据ID获取${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}?> Get${entityShort}ByIdAsync(long id)\n`;
  content += '    {\n';
  content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
  content += `        if (entity == null || ${entityScopeGuard})\n`;
  content += '        {\n';
  content += '            return null;\n';
  content += '        }\n';
  content +=
    rbacDelegation?.getByIdReturn
    || masterDetail?.getByIdReturn
    || `        return entity.Adapt<${dtoInfo.base}>();\n`;
  content += '    }\n\n';

  if (treeGen) {
    const treeOptionsMethodName = `Get${entityShort}TreeOptionsAsync`;
    const treeOptionsResolved = resolveOptionsImplementationBlock({
      existingContent,
      methodName: treeOptionsMethodName,
      repoField,
      freshTemplate: treeGen.treeOptionsBlock,
      refreshOptions: genOptions.refreshOptions === true,
      statusMeta: enableStatusMeta,
      entityPropNames: entityProps,
      entityScalarProps,
      dtoBase,
    });
    content += treeOptionsResolved.block;
    if (treeOptionsResolved.preserved) {
      preservedOptionsMethods.push(treeOptionsMethodName);
    } else if (treeOptionsResolved.regenerated) {
      regeneratedOptionsMethods.push(treeOptionsMethodName);
    }
    content += treeGen.treeRemainderBlock;
  } else {
    const flatOptionsMethodName = `Get${entityShort}OptionsAsync`;
    const entityContentForOptions = fs.existsSync(entityFile) ? readUtf8(entityFile) : '';
    const optionsPlantField = resolveOptionsPlantField(entityContentForOptions);
    const flatOptionsTemplate = buildFlatOptionsAsyncImplTemplate(
      entityShort,
      desc,
      repoField,
      ensureContextLine,
      optionsListPredicate,
      nameField,
      optionsValueField,
      optionsValueAsString,
      entityName,
      optionsPlantField,
    );
    const flatOptionsResolved = resolveOptionsImplementationBlock({
      existingContent,
      methodName: flatOptionsMethodName,
      repoField,
      freshTemplate: flatOptionsTemplate,
      refreshOptions: genOptions.refreshOptions === true,
      statusMeta: enableStatusMeta,
      entityPropNames: entityProps,
      entityScalarProps,
      dtoBase,
    });
    content += flatOptionsResolved.block;
    if (flatOptionsResolved.preserved) {
      preservedOptionsMethods.push(flatOptionsMethodName);
    } else if (flatOptionsResolved.regenerated) {
      regeneratedOptionsMethods.push(flatOptionsMethodName);
    }
  }

  // Create
  content += buildMethodXmlDoc({
    summary: `创建${desc}`,
    params: [{ name: 'dto', desc: '创建DTO' }],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}> Create${entityShort}Async(${dtoInfo.create} dto)\n`;
  content += '    {\n';
  content += `        var entity = dto.Adapt<${entityName}>();\n`;
  if (needsNumberingGenerator && numberingCodeProp) {
    content += `        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))\n`;
    content += '        {\n';
    content += '            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());\n';
    content += '            if (string.IsNullOrWhiteSpace(generated.BusinessCode))\n';
    content += '            {\n';
    content += '                throw new TaktBusinessException("业务编码生成失败");\n';
    content += '            }\n';
    content += `            entity.${numberingCodeProp} = generated.BusinessCode;\n`;
    content += '        }\n';
    content += `        else if (string.IsNullOrWhiteSpace(entity.${numberingCodeProp}))\n`;
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}编码不能为空");\n`;
    content += '        }\n';
  }
  if (hasIsObsolete) {
    content += '        entity.IsObsolete = 0;\n';
  }
  if (hasBuiltIn) {
    content += buildBuiltInCreateAssignLine();
  }
  if (manyToOneMaster?.createBeforeSave) {
    content += `        ${manyToOneMaster.createBeforeSave}\n`;
  }
  if (createUniqueBlock) {
    content += createUniqueBlock;
  }
  if (sequenceMeta.main.sortOrder) {
    content += buildAssignSortOrderBlock(
      sequenceMeta.main.sortOrder,
      repoField,
      dtoBase,
      'entity',
      '        ',
      entityFile,
    );
  }
  if (sequenceMeta.main.lineNumber) {
    content += buildAssignLineNumberBlock(
      sequenceMeta.main.lineNumber,
      repoField,
      dtoBase,
      'entity',
      '        ',
    );
  }
  content += `        entity = await ${repoField}.CreateAsync(entity);\n`;
  if (rbacDelegation?.createAfterSave) {
    content += `${rbacDelegation.createAfterSave}\n`;
  }
  if (masterDetail?.createAfterSave) {
    content += `        ${masterDetail.createAfterSave}\n`;
  }
  content += `        return await Get${entityShort}ByIdAsync(entity.Id) ?? entity.Adapt<${dtoInfo.base}>();\n`;
  content += '    }\n\n';

  // Update
  content += buildMethodXmlDoc({
    summary: `更新${desc}`,
    params: [
      { name: 'id', desc: `${desc}ID` },
      { name: 'dto', desc: '更新DTO' },
    ],
    returns: 'DTO',
  });
  content += `    public async Task<${dtoInfo.base}> Update${entityShort}Async(long id, ${dtoInfo.update} dto)\n`;
  content += '    {\n';
  content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
  content += '        if (entity == null)\n';
  content += '        {\n';
  content += `            throw new TaktBusinessException("${desc}不存在");\n`;
  content += '        }\n';
  if (hasBuiltIn) {
    content += buildBuiltInUpdateBeforeAdaptLines(builtInStatusMeta);
  }
  content += '        dto.Adapt(entity);\n';
  if (hasBuiltIn) {
    content += buildBuiltInUpdateAfterAdaptLines(desc, builtInStatusMeta);
  }
  if (manyToOneMaster?.updateBeforeSave) {
    content += `        ${manyToOneMaster.updateBeforeSave}\n`;
  }
  if (updateUniqueBlock) {
    content += updateUniqueBlock;
  }
  content += `        await ${repoField}.UpdateAsync(entity);\n`;
  if (rbacDelegation?.updateAfterSave) {
    content += `${rbacDelegation.updateAfterSave}\n`;
  }
  if (masterDetail?.updateAfterSave) {
    content += `        ${masterDetail.updateAfterSave}\n`;
  }
  content += `        return await Get${entityShort}ByIdAsync(id) ?? throw new TaktBusinessException("${desc}不存在");\n`;
  content += '    }\n\n';

  // Delete
  let treeDeleteGuard = '';
  if (crudType === 'Tree') {
    treeDeleteGuard = `
        var hasChildren = await ${repoField}.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
`;
  }
  content += buildMethodXmlDoc({
    summary: `删除${desc}`,
    params: [{ name: 'id', desc: `${desc}ID` }],
    returns: '任务',
  });
  content += `    public async Task Delete${entityShort}ByIdAsync(long id)\n`;
  content += '    {\n';
  if (hasBuiltIn && !masterDetail?.deletePrefix && !rbacDelegation?.deletePrefix && !hasIsObsolete) {
    content += `        var entity = await ${repoField}.GetByIdAsync(id);\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
    content += buildBuiltInDeleteGuardLines(desc);
  }
  content += treeDeleteGuard;
  if (rbacDelegation?.deletePrefix) {
    content += `${rbacDelegation.deletePrefix}\n`;
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else if (masterDetail?.deletePrefix) {
    content += `${masterDetail.deletePrefix}\n`;
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else if (hasIsObsolete) {
    content += buildObsoleteMarkDeleteBody(
      repoField,
      desc,
      entityScopeGuard,
      hasBuiltIn ? buildBuiltInDeleteGuardLines(desc) : '',
    );
  } else if (hasBuiltIn) {
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  } else {
    content += `        var deleted = await ${repoField}.DeleteAsync(id);\n`;
    content += '        if (!deleted)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在或已删除");\n`;
    content += '        }\n';
  }
  content += '    }\n\n';

  content += buildMethodXmlDoc({
    summary: `批量删除${desc}`,
    params: [{ name: 'ids', desc: 'ID列表' }],
    returns: '任务',
  });
  content += `    public async Task Delete${entityShort}BatchAsync(IEnumerable<long> ids)\n`;
  content += '    {\n';
  content += '        var idList = ids?.Distinct().ToList() ?? new List<long>();\n';
  content += '        if (idList.Count == 0)\n';
  content += '        {\n';
  content += '            return;\n';
  content += '        }\n';
  if (hasBuiltIn) {
    content += buildBuiltInBatchDeleteGuardLines(desc, repoField);
  }
  content += '        foreach (var id in idList)\n';
  content += '        {\n';
  content += `            await Delete${entityShort}ByIdAsync(id);\n`;
  content += '        }\n';
  content += '    }\n\n';

  // Status
  for (const statusDto of dtoInfo.statuses) {
    const suffix = buildStatusMethodSuffix(statusDto, entityName);
    const statusFields = parseStatusDtoFields(dtoContent, statusDto);
    const idProp = statusFields.idProperty || `${entityShort}Id`;
    const valueProp = statusFields.valueProperties[0];
    if (!valueProp) {
      continue;
    }
    const statusLabel = suffix === 'Status' ? '状态' : suffix;
    content += buildMethodXmlDoc({
      summary: `更新${desc}${statusLabel}`,
      params: [{ name: 'dto', desc: '状态DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}${suffix}Async(${statusDto} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${idProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    if (hasBuiltIn) {
      content += buildBuiltInStatusDisableGuardLines(desc, builtInStatusMeta, valueProp.name);
    }
    content += `        entity.${valueProp.name} = dto.${valueProp.name};\n`;
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${idProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  // Sort
  if (dtoInfo.sort) {
    const sortBlock = extractClassBlock(dtoContent, dtoInfo.sort);
    const sortIdMatch = sortBlock.match(/public\s+long\s+(\w+Id)\s*\{/);
    const sortIdProp = sortIdMatch ? sortIdMatch[1] : `${entityShort}Id`;
    content += buildMethodXmlDoc({
      summary: `更新${desc}排序`,
      params: [{ name: 'dto', desc: '排序DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}SortAsync(${dtoInfo.sort} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${sortIdProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    content += '        entity.SortOrder = dto.SortOrder;\n';
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${sortIdProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  if (dtoInfo.builtIn) {
    const builtInBlock = extractClassBlock(dtoContent, dtoInfo.builtIn);
    const builtInIdMatch = builtInBlock.match(/public\s+long\s+(\w+Id)\s*\{/);
    const builtInIdProp = builtInIdMatch ? builtInIdMatch[1] : `${entityShort}Id`;
    content += buildMethodXmlDoc({
      summary: `更新${desc}内置`,
      params: [{ name: 'dto', desc: '内置 DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}BuiltInAsync(${dtoInfo.builtIn} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${builtInIdProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    content += '        if (dto.IsBuiltIn is not 0 and not 1)\n';
    content += '        {\n';
    content += '            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");\n';
    content += '        }\n';
    content += '        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("不允许取消内置${desc}标识");\n`;
    content += '        }\n';
    content += '        entity.IsBuiltIn = dto.IsBuiltIn;\n';
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${builtInIdProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  if (dtoInfo.obsolete) {
    const obsoleteBlock = extractClassBlock(dtoContent, dtoInfo.obsolete);
    const obsoleteIdMatch = obsoleteBlock.match(/public\s+long\s+(\w+Id)\s*\{/);
    const obsoleteIdProp = obsoleteIdMatch ? obsoleteIdMatch[1] : `${entityShort}Id`;
    content += buildMethodXmlDoc({
      summary: `更新${desc}作废状态`,
      params: [{ name: 'dto', desc: '作废DTO' }],
      returns: 'DTO',
    });
    content += `    public async Task<${dtoInfo.base}> Update${entityShort}ObsoleteAsync(${dtoInfo.obsolete} dto)\n`;
    content += '    {\n';
    content += `        var entity = await ${repoField}.GetByIdAsync(dto.${obsoleteIdProp});\n`;
    content += '        if (entity == null)\n';
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    content += `        if (${entityScopeGuard})\n`;
    content += '        {\n';
    content += `            throw new TaktBusinessException("${desc}不存在");\n`;
    content += '        }\n';
    content += '        entity.IsObsolete = dto.IsObsolete;\n';
    content += `        await ${repoField}.UpdateAsync(entity);\n`;
    content += `        return await Get${entityShort}ByIdAsync(dto.${obsoleteIdProp}) ?? throw new TaktBusinessException("${desc}不存在");\n`;
    content += '    }\n\n';
  }

  if (templateDto) {
    content += buildMethodXmlDoc({
      summary: '获取导入模板',
      params: [
        { name: 'sheetName', desc: '工作表名称' },
        { name: 'fileName', desc: '文件名' },
      ],
      returns: 'Excel 文件',
    });
    content += `    public async Task<(string fileName, byte[] content)> Get${entityShort}TemplateAsync(string? sheetName = null, string? fileName = null)\n`;
    content += '    {\n';
    content += `        return await TaktExcelHelper.GenerateTemplateAsync<${templateDto}>(\n`;
    content += `            sheetName ?? "${desc}导入模板",\n`;
    content += `            fileName ?? "${desc}导入模板.xlsx");\n`;
    content += '    }\n\n';
  }

  if (importDto) {
    content += buildMethodXmlDoc({
      summary: `导入${desc}`,
      params: [
        { name: 'fileStream', desc: 'Excel 文件流' },
        { name: 'sheetName', desc: '工作表名称' },
      ],
      returns: '导入结果',
    });
    content += `    public async Task<(int success, int fail, List<string> errors)> Import${entityShort}Async(Stream fileStream, string? sheetName = null)\n`;
    content += '    {\n';
    content += '        var errors = new List<string>();\n';
    content += '        var success = 0;\n';
    content += '        var fail = 0;\n';
    content += `        var rows = await TaktExcelHelper.ImportAsync<${importDto}>(fileStream, sheetName ?? "${desc}导入模板");\n`;
    content += '        if (rows == null || rows.Count == 0)\n';
    content += '        {\n';
    content += '            errors.Add("Excel文件中没有数据");\n';
    content += '            return (0, 0, errors);\n';
    content += '        }\n';
    content += importBatchGuard.declare;
    if (sequenceMeta.main.sortOrder) {
      content += buildImportSortOrderDeclare(sequenceMeta.main.sortOrder, repoField, dtoBase);
    }
    content += '        for (var i = 0; i < rows.Count; i++)\n';
    content += '        {\n';
    content += '            try\n';
    content += '            {\n';
    content += `                var entity = rows[i].Adapt<${entityName}>();\n`;
    if (hasBuiltIn) {
      content += '                entity.IsBuiltIn = 0;\n';
    }
    if (manyToOneMaster) {
      content += `                var importDto = rows[i].Adapt<${dtoInfo.create}>();\n`;
      content += `                ${manyToOneMaster.importBeforeSave}\n`;
    }
    if (importBatchGuard.check) {
      content += importBatchGuard.check;
    }
    if (importUniqueBlock) {
      content += importUniqueBlock;
    }
    if (sequenceMeta.main.sortOrder) {
      if (sequenceMeta.main.sortOrder.mode === 'flat') {
        content += buildImportSortOrderAssign(sequenceMeta.main.sortOrder, 'entity', '                ');
      } else {
        content += buildAssignSortOrderBlock(
          sequenceMeta.main.sortOrder,
          repoField,
          dtoBase,
          'entity',
          '                ',
          entityFile,
        );
      }
    }
    if (sequenceMeta.main.lineNumber) {
      content += buildAssignLineNumberBlock(
        sequenceMeta.main.lineNumber,
        repoField,
        dtoBase,
        'entity',
        '                ',
      );
    }
    content += `                await ${repoField}.CreateAsync(entity);\n`;
    content += '                success += 1;\n';
    content += '            }\n';
    content += '            catch (Exception ex)\n';
    content += '            {\n';
    content += '                fail += 1;\n';
    content += '                errors.Add($"第{i + 2}行: {ex.Message}");\n';
    content += '            }\n';
    content += '        }\n';
    content += '        return (success, fail, errors);\n';
    content += '    }\n\n';
  }

  // Export
  content += buildMethodXmlDoc({
    summary: `导出${desc}`,
    params: [
      { name: 'query', desc: '查询条件' },
      { name: 'sheetName', desc: '工作表名称' },
      { name: 'fileName', desc: '文件名' },
    ],
    returns: 'Excel 文件',
  });
  content += `    public async Task<(string fileName, byte[] fileContent)> Export${entityShort}Async(${dtoInfo.query}? query = null, string? sheetName = null, string? fileName = null)\n`;
  content += '    {\n';
  content += `        var queryDto = query ?? new ${dtoInfo.query}();\n`;
  content += '        if (!HasAnyListQueryFilter(queryDto))\n';
  content += '        {\n';
  content += `            return await TaktExcelHelper.ExportAsync(\n`;
  content += `                new List<${exportDto}>(),\n`;
  content += `                sheetName ?? "${desc}数据",\n`;
  content += `                fileName ?? "${desc}导出.xlsx");\n`;
  content += '        }\n';
  content += '        var predicate = QueryExpression(queryDto);\n';
  content += `        var list = await ${repoField}.GetListAsync(predicate);\n`;
  content += '        if (list == null || list.Count == 0)\n';
  content += '        {\n';
  content += `            return await TaktExcelHelper.ExportAsync(\n`;
  content += `                new List<${exportDto}>(),\n`;
  content += `                sheetName ?? "${desc}数据",\n`;
  content += `                fileName ?? "${desc}导出.xlsx");\n`;
  content += '        }\n';
  content += `        var exportData = list.Adapt<List<${exportDto}>>();\n`;
  content += '        return await TaktExcelHelper.ExportAsync(\n';
  content += '            exportData,\n';
  content += `            sheetName ?? "${desc}数据",\n`;
  content += `            fileName ?? "${desc}导出.xlsx");\n`;
  content += '    }\n\n';

  const preservedExtraImpl = extractPreservedExtraServiceMethods(
    existingContent,
    entityShort,
    dtoInfo,
    crudType,
    'implementation',
  );
  if (preservedExtraImpl.blocks) {
    content += '    // ========================================\n';
    content += '    // 扩展方法（保留）\n';
    content += '    // ========================================\n\n';
    content += preservedExtraImpl.blocks;
  }

  if (masterDetail?.privateMethods) {
    content += '    // ========================================\n';
    content += '    // 主子表级联（OneToMany）\n';
    content += '    // ========================================\n\n';
    content += masterDetail.privateMethods;
  }

  if (manyToOneMaster?.privateMethods) {
    content += '    // ========================================\n';
    content += '    // 主表外键同步（ManyToOne）\n';
    content += '    // ========================================\n\n';
    content += manyToOneMaster.privateMethods;
  }

  if (transposedGen?.methods) {
    content += '    // ========================================\n';
    content += '    // 转置（多语言表格）\n';
    content += '    // ========================================\n\n';
    content += transposedGen.methods;
    content += '\n';
  }

  // QueryExpression
  content += '    // ========================================\n';
  content += '    // 查询表达式\n';
  content += '    // ========================================\n\n';
  content += buildMethodXmlDoc({
    summary: `构建${desc}查询表达式`,
    params: [{ name: 'queryDto', desc: '查询DTO' }],
    returns: '查询表达式',
  });
  content += `    private static Expression<Func<${entityName}, bool>> QueryExpression(${dtoInfo.query}? queryDto)\n`;
  content += '    {\n';
  content += `${queryExprBody}\n`;
  content += '    }\n\n';
  content += buildHasAnyListQueryFilterMethod(
    dtoInfo.query,
    queryProps,
    dateRanges,
    hasIsObsolete,
  );
  if (transposedGen?.transposedQueryExpr) {
    content += '\n';
    content += '    /// <summary>\n';
    content += `    /// 构建${desc}转置查询表达式\n`;
    content += '    /// </summary>\n';
    content += `    private Expression<Func<${entityName}, bool>> TransposedQueryExpression(${dtoInfo.transposedQuery} queryDto)\n`;
    content += '    {\n';
    content += `${transposedGen.transposedQueryExpr};\n`;
    content += '    }\n';
  }

  content += '}\n';
  return {
    content,
    preservedOptionsMethods,
    regeneratedOptionsMethods,
    preservedExtraMethods: preservedExtraImpl.methodNames,
    sequenceMeta,
  };
}

function findDtoFileByEntity(entityName) {
  const fileName = `${entityName}Dtos.cs`;
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        const found = searchDir(fullPath);
        if (found) {
          return found;
        }
      } else if (entry.name === fileName) {
        return fullPath;
      }
    }
    return null;
  }
  return searchDir(CONFIG.dtosRoot);
}

function scanDtoFiles(entityPrefix) {
  const results = [];
  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return;
    }
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        walk(fullPath);
      } else if (entry.name.endsWith('Dtos.cs')) {
        if (shouldExcludeDtoFile(fullPath)) {
          continue;
        }
        const entityName = entityNameFromDtoFile(fullPath);
        if (!entityName) {
          continue;
        }
        const entityShort = entityName.replace(/^Takt/, '');
        if (entityPrefix && entityShort !== entityPrefix) {
          continue;
        }
        results.push(fullPath);
      }
    }
  }
  walk(CONFIG.dtosRoot);
  return results.sort();
}

function getServiceOutputPaths(entityFile, entityName) {
  const moduleParts = getModuleRelativePath(entityFile);
  const serviceDir = path.join(CONFIG.servicesRoot, ...moduleParts);
  return {
    interfaceFile: path.join(serviceDir, `I${entityName}Service.cs`),
    implFile: path.join(serviceDir, `${entityName}Service.cs`),
    serviceDir,
  };
}

function processDtoFile(dtoFile, options) {
  if (shouldExcludeDtoFile(dtoFile)) {
    const entityName = entityNameFromDtoFile(dtoFile);
    const entityShort = entityName ? entityName.replace(/^Takt/, '') : path.basename(dtoFile);
    console.log(`\n⏭️  跳过特殊实体: ${entityShort}（${path.relative(CONFIG.dtosRoot, dtoFile)}）`);
    return { status: 'skipped' };
  }

  const entityName = entityNameFromDtoFile(dtoFile);
  const entityShort = entityName.replace(/^Takt/, '');
  const relDto = path.relative(CONFIG.dtosRoot, dtoFile);

  console.log(`\n${'='.repeat(60)}`);
  console.log(`📦 ${entityName}  ←  ${relDto}`);

  const dtoInfo = extractDtoInfo(dtoFile);
  if (!isAggregatable(dtoInfo)) {
    console.log('  ⏭️  跳过：非标准聚合 DTO（需同时具备 Dto / QueryDto / CreateDto / UpdateDto）');
    return { status: 'skipped' };
  }

  const entityFile = findEntityFile(entityName);
  if (!entityFile) {
    console.log(`  ❌ 未找到实体文件: ${entityName}.cs`);
    return { status: 'failed' };
  }

  const crudType = identifyCrudType(entityFile);
  if (crudType === 'MasterDetail') {
    const navChildren = parseOneToManyNavigations(entityFile);
    const rbacNavs = navChildren.filter((n) => isRbacJunctionEntity(n.childShort));
    const standaloneNavs = navChildren.filter((n) => isStandaloneChildVueEntity(n.childShort));
    const masterNavs = navChildren.filter(
      (n) => !isRbacJunctionEntity(n.childShort) && !isStandaloneChildVueEntity(n.childShort),
    );
    if (rbacNavs.length > 0 || hasRbacParentConfig(entityShort)) {
      console.log(
        `  ℹ️  RBAC 关联委托 ITaktRbacService（${hasRbacParentConfig(entityShort) ? 'rbac-parent-config' : rbacNavs.map((n) => n.childShort).join('、')}）`,
      );
    }
    if (standaloneNavs.length > 0) {
      console.log(
        `  ℹ️  独立菜单从实体（不级联）：${standaloneNavs.map((n) => n.childShort).join('、')}`,
      );
    }
    if (masterNavs.length > 0) {
      const obsoleteChildNavs = masterNavs.filter((n) => {
        const childFile = findEntityFile(n.childEntity);
        return childFile && entityFileHasIsObsolete(childFile);
      });
      console.log(
        `  ℹ️  主子表：Create/Update 含子表集合，服务级联查询/保存/删除（子表×${masterNavs.length}）`,
      );
      if (obsoleteChildNavs.length > 0) {
        console.log(
          `  ℹ️  主子表 IsObsolete：Fill 含作废行；Save 行号 existingList.Max；未提交行作废（子表×${obsoleteChildNavs.length}）`,
        );
      }
    }
  } else if (hasRbacParentConfig(entityShort)) {
    console.log('  ℹ️  RBAC 关联委托 ITaktRbacService（rbac-parent-config）');
  }
  if (crudType === 'Single') {
    const standaloneOnly = parseOneToManyNavigations(entityFile).filter((n) =>
      isStandaloneChildVueEntity(n.childShort),
    );
    if (standaloneOnly.length > 0) {
      console.log(
        `  ℹ️  实体导航含独立菜单从实体（单表生成，不级联）：${standaloneOnly.map((n) => n.childShort).join('、')}`,
      );
    }
  }

  if (!options.force && (EXISTING_MANUAL_SERVICE_ENTITIES.has(entityName) || shouldExcludeStandaloneService(entityName))) {
    console.log(`  ⏭️  跳过：已有手工服务（实体 ${entityName}），使用 --force 可覆盖`);
    return { status: 'skipped' };
  }

  const output = getServiceOutputPaths(entityFile, entityName);

  const description = extractEntityDescription(entityFile) || entityShort;
  const dtoBaseFromDto = parseDtoBase(dtoFile, dtoInfo);
  if (!dtoBaseFromDto) {
    console.log(
      `  ❌ 无法识别 DTO 基类：${dtoInfo.base} 须继承 TaktTenantDtoBase / TaktCompanyDtoBase / TaktApprovalDtoBase`,
    );
    return { status: 'failed' };
  }
  const entityBaseFromFile = parseEntityBase(entityFile);
  // 隔离/Options 以 Domain 实体三基类为准（CompanyCode 是否存在以实体为准）
  const dtoBase = resolveIsolationDtoBase(dtoBaseFromDto, entityBaseFromFile) || dtoBaseFromDto;
  const entityBaseExpected = DTO_BASE_TO_ENTITY_BASE[dtoBaseFromDto];
  if (entityBaseFromFile && entityBaseFromFile !== entityBaseExpected) {
    console.log(
      `  ⚠️  DTO 基类 ${dtoBaseFromDto}（→${entityBaseExpected}）与实体基类 ${entityBaseFromFile} 不一致，隔离/Options 以实体为准 → ${dtoBase}`,
    );
  }
  const isolationHint = dtoBaseHasCompanyIsolation(dtoBase)
    ? 'TenantCode + CompanyCode'
    : '仅 TenantCode';
  console.log(`  DtoBase: ${dtoBase}  →  ${DTO_BASE_TO_REPOSITORY[dtoBase]}（${isolationHint}）`);
  if (entityHasIsBuiltIn(entityFile)) {
    console.log('  ℹ️  实体含 IsBuiltIn：已生成创建/更新/单删/批删/状态更新内置保护');
  }

  const existingIfaceContent = fs.existsSync(output.interfaceFile) ? readUtf8(output.interfaceFile) : null;
  const existingImplContent = fs.existsSync(output.implFile) ? readUtf8(output.implFile) : null;

  const iface = generateServiceInterface(
    entityName,
    dtoInfo,
    description,
    entityFile,
    crudType,
    existingIfaceContent,
  );
  const implResult = generateServiceImplementation(
    entityName,
    dtoInfo,
    description,
    entityFile,
    crudType,
    dtoBase,
    existingImplContent,
    { refreshOptions: options.refreshOptions === true },
  );
  const ifaceContent = iface.content;
  const impl = implResult.content;

  const preservedOptions = [...new Set([...(implResult.preservedOptionsMethods || [])])];
  const ifaceOnlyPreserved = (iface.preservedOptionsMethods || []).filter(
    (m) => !(implResult.regeneratedOptionsMethods || []).includes(m),
  );
  if (preservedOptions.length > 0) {
    console.log(`  ℹ️  已保留已有 Get*OptionsAsync（未重新生成）: ${preservedOptions.join(', ')}`);
  } else if (ifaceOnlyPreserved.length > 0 && (implResult.regeneratedOptionsMethods || []).length > 0) {
    // 接口签名保留、实现因实体字段变更等已重生成 —— 只打重生成日志即可
  }
  if (implResult.regeneratedOptionsMethods?.length > 0) {
    console.log(
      `  ℹ️  已重新生成 Get*OptionsAsync（实体字段失效/遗留无效调用/--refresh-options）: ${implResult.regeneratedOptionsMethods.join(', ')}`,
    );
  }
  const preservedExtra = [...new Set([
    ...(iface.preservedExtraMethods || []),
    ...(implResult.preservedExtraMethods || []),
  ])];
  if (preservedExtra.length > 0) {
    console.log(`  ℹ️  已保留扩展方法: ${preservedExtra.join(', ')}`);
  }
  if (entityHasIsBuiltIn(entityFile) && dtoInfo.builtIn) {
    console.log(`  ℹ️  已生成 Update${entityShort}BuiltInAsync（${dtoInfo.builtIn}）`);
  }
  if (
    preservedOptions.length === 0 &&
    !(implResult.regeneratedOptionsMethods?.length > 0) &&
    (existingImplContent || existingIfaceContent)
  ) {
    const optionsMethodName =
      crudType === 'Tree' || dtoInfo.tree
        ? `Get${entityShort}TreeOptionsAsync`
        : `Get${entityShort}OptionsAsync`;
    if (!hasGetOptionsAsyncMethod(existingImplContent, optionsMethodName)) {
      console.log(`  ℹ️  已生成默认 ${optionsMethodName}`);
    }
  }

  if (options.dryRun) {
    console.log(`  🔍 [dry-run] ${path.relative(CONFIG.backendRoot, output.interfaceFile)}`);
    console.log(`  🔍 [dry-run] ${path.relative(CONFIG.backendRoot, output.implFile)}`);
    if (preservedOptions.length > 0) {
      console.log(`  🔍 [dry-run] 将保留 Options 方法: ${preservedOptions.join(', ')}`);
    }
    return { status: 'dry-run' };
  }

  const ifaceWrite = writeGeneratedFile(output.interfaceFile, ifaceContent);
  const implWrite = writeGeneratedFile(output.implFile, impl);
  const ifaceLabel = ifaceWrite.created ? '已创建' : '已更新';
  const implLabel = implWrite.created ? '已创建' : '已更新';
  console.log(`  ✅ ${ifaceLabel}: ${path.relative(CONFIG.backendRoot, output.interfaceFile)}`);
  console.log(`  ✅ ${implLabel}: ${path.relative(CONFIG.backendRoot, output.implFile)}`);
  if (crudType === 'Tree' || dtoInfo.tree) {
    console.log('  ℹ️  已生成 Get*TreeAsync / TreeOptions（懒加载一层 parentId，见 TaktLazyTreeHelper）');
  }
  if (crudType === 'MasterDetail') {
    console.log('  ℹ️  已生成 Fill*DetailsAsync / Save*ChildrenAsync 级联方法');
  }
  if (implResult.sequenceMeta?.needsSort || implResult.sequenceMeta?.needsLine) {
    const parts = [];
    if (implResult.sequenceMeta.needsSort) {
      parts.push('SortOrder');
    }
    if (implResult.sequenceMeta.needsLine) {
      parts.push('LineNumber');
    }
    console.log(`  ℹ️  已接入 ${parts.join('、')} 自动生成（Create/Import/子表级联）`);
  }
  if (isTransposableEntity(entityShort)) {
    console.log('  ℹ️  已生成 Get*TransposedListAsync / Save*TransposedBatchAsync 转置方法');
  }
  return {
    status: 'written',
    created: ifaceWrite.created || implWrite.created,
    updated: ifaceWrite.updated || implWrite.updated,
  };
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-services-from-dtos.cjs --Holiday
  node scripts/generate-services-from-dtos.cjs --Holiday --force
  node scripts/generate-services-from-dtos.cjs --Holiday --refresh-options
  node scripts/generate-services-from-dtos.cjs --Holiday --dry-run

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 扫描 Takt.Application/Dtos/**/*Dtos.cs
  - 仅处理同时具备 TaktXxxDto / QueryDto / CreateDto / UpdateDto 的聚合模块
  - 隔离与仓储由主 DTO / Domain 实体三基类决定（不一致时隔离以实体为准）：
      TaktTenant*EntityBase / TaktTenant*DtoBase（四组合 Core/Culture/Plant/默认）→ ITaktTenantRepository，仅 TenantCode；Options 禁止 CompanyCode / EnsureThreeLayerContext
      RelatedPlant 仅组合 1·3；CultureCode 注入 Create 仅组合 2（Culture）与公司/审批
      TaktCompanyEntityBase / TaktCompanyDtoBase → ITaktCompanyRepository，TenantCode + CompanyCode；Options 必须含 CompanyCode 过滤
      TaktApprovalEntityBase / TaktApprovalDtoBase → ITaktApprovalRepository，TenantCode + CompanyCode；Options 同公司级
  - 排除 User（与 generate-dtos-from-entity.cjs 一致，禁止生成/覆盖）
  - Translation：额外生成转置查询/批量保存（多语言表格）
  - 输出策略：文件不存在则创建，已存在则覆盖更新（无需 --force）
  - 主子表（OneToMany）：Create/Update 含子表 List，Fill*DetailsAsync / Save*ChildrenAsync 级联
  - 子表明细含 IsObsolete：Fill 含作废行、不生成 Max*LineNumber；Save 行号 existingList.Max；未提交行 IsObsolete=1
  - 无 IsObsolete 子表行号：Fill/Save 用 GetMaxIntAsync(includeSoftDeleted: true)（原含软删占号语义）
  - 独立子表明细含 IsObsolete：列表默认未作废；Delete 标记作废；Create 强制 IsObsolete=0；UpdateXxxObsoleteAsync 作废/撤销
  - RBAC 八表：主实体 Create/Update/Delete 委托 ITaktRbacService（见 scripts/gen/rbac-parent-config.cjs，User 除外）
  - Auth 等手工服务仅在不带 --force 时跳过
  - 树形实体（含 ParentId）：生成懒加载 GetXxxTreeOptionsAsync(parentId) + GetXxxTreeAsync(parentId)（仅一层，见 TaktLazyTreeHelper），不生成 GetXxxOptionsAsync / 递归 Build*Tree
  - Get*OptionsAsync：磁盘上无该方法 → 按三基类生成默认模板；已存在且隔离谓词/字段仍有效 → 原样保留
  - 已存在但隔离级别错误（租户级残留 CompanyCode，或公司/审批级缺少 CompanyCode 过滤）、遗留无效调用、全量递归 Build*Tree、或引用已删字段 → 自动改用模板重新生成
  - DictLabel：优先 {Entity}Name / *Name；否则与 DictValue 同字段
  - DictValue（平铺）：*Code → *Name → 首个业务 nvarchar；禁止雪花 Id；树形 TreeOptions 的 DictValue 仍为 Id 字符串（ParentId 外键）
  - --refresh-options：强制重新生成所有 Get*OptionsAsync / Get*TreeOptionsAsync 实现
  - 实体含 IsBuiltIn（int，字典 sys_yes_no）时：创建/导入强制 0；更新保留原值；单删/批删前校验；状态更新禁止将内置项设为非启用(1)
  - 实体含 SortOrder / LineNumber：Create、Import、主子表 Save*ChildrenAsync 在值 <= 0 时经 ITaktSortOrderGenerator / ITaktLineNumberGenerator 自动生成；SortOrder/独立子表用 GetMaxIntAsync；主子表 Save 用 IsObsolete 时用 GetMaxIntAsync(includeSoftDeleted: true)
`);
}

function parseArgs() {
  const rawArgs = process.argv.slice(2);
  const refreshOptions = rawArgs.includes('--refresh-options');
  const args = rawArgs.filter((arg) => arg !== '--refresh-options');
  const options = parseSingleEntityGenerateArgsFromArgv(args, printUsage);
  options.refreshOptions = refreshOptions;
  assertNotRbacJunctionEntityCli(options.entityPrefix);
  assertNotManualDtoEntityCli(options.entityPrefix);
  return options;
}

// ========================================
// 主流程
// ========================================

console.log('🚀 从 DTO 生成 Application 服务接口与实现...\n');
logGeneratedFileWritePolicy();
console.log(`⏭️  跳过 RBAC 关联表 DTO: ${[...RBAC_ASSOCIATION_ENTITY_SHORT_NAMES].join('、')}\n`);

try {
  const options = parseArgs();
  const dtoFiles = scanDtoFiles(options.entityPrefix);

  if (dtoFiles.length === 0) {
    console.error('❌ 未找到匹配的 DTO 文件');
    process.exit(1);
  }

  console.log(`📄 匹配 DTO 文件 ${dtoFiles.length} 个`);

  let created = 0;
  let updated = 0;
  let skipped = 0;
  let failed = 0;

  for (const dtoFile of dtoFiles) {
    const result = processDtoFile(dtoFile, options);
    if (result.status === 'dry-run') {
      created += 1;
    } else if (result.status === 'written') {
      if (result.updated && !result.created) {
        updated += 1;
      } else {
        created += 1;
      }
    } else if (result.status === 'failed') {
      failed += 1;
    } else {
      skipped += 1;
    }
  }

  const processed = created + updated + skipped + failed;
  console.log(`\n📊 已创建 ${created} 个，已更新 ${updated} 个，跳过 ${skipped} 个，失败 ${failed} 个（共处理 ${processed} 个 *Dtos.cs）`);
  if (skipped > 0) {
    console.log(
      '   跳过原因：① 关联表无完整 CRUD 聚合；② 手工服务实体 TaktAuth；TaktLoginDtos.cs 不进入扫描；',
    );
    console.log('   User/Online/Message、RBAC 八表等 Dtos 不会进入扫描列表。');
  }
  console.log('✨ 完成！请编译解决方案，并人工审阅 QueryExpression、导入校验与特殊树形业务逻辑。');
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-master-detail-associations.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：主子表关联（OneToMany ↔ ManyToOne）解析、校验与 API 路径定位
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  findDomainEntityFile,
  resolveFrontendModuleFileName,
  resolveFrontendOutputRelPath,
  toFrontendApiFileName,
  parseFrontendApiFileCamel,
  camelToKebab,
} = require('./generate-script-common.cjs');
const { RBAC_ASSOCIATION_ENTITY_SHORT_NAMES } = require('./generate-entity-exclusions.cjs');
const {
  CONFIG,
  parseOneToManyNavigations,
  collectDomainEntityFiles,
  pascalToKebab,
  pascalToCamel,
  resolvePermissionPrefixFromController,
} = require('./generate-vue-common.cjs');

const NAVIGATION_REGION_MARKER = '导航属性区域';

/**
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
 * 解析实体 ManyToOne 导航（与 generate-services-from-dtos 对齐）
 * @param {string} entityFile
 * @returns {Array<{ fkField: string, masterEntity: string, masterShort: string, navPropName: string }>}
 */
function parseManyToOneNavigations(entityFile) {
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
    /\[Navigate\(\s*NavigateType\.ManyToOne\s*,\s*nameof\((\w+)\)\s*\)\][\s\S]*?public\s+(Takt\w+)\??\s+(\w+)\s*\{\s*get;\s*set;/g;
  /** @type {Array<{ fkField: string, masterEntity: string, masterShort: string, navPropName: string }>} */
  const navigations = [];
  const seenFk = new Set();
  let match;
  while ((match = navRegex.exec(body)) !== null) {
    const fkField = match[1];
    if (seenFk.has(fkField)) {
      continue;
    }
    seenFk.add(fkField);
    const masterEntity = match[2];
    navigations.push({
      fkField,
      masterEntity,
      masterShort: masterEntity.replace(/^Takt/, ''),
      navPropName: match[3],
    });
  }
  return navigations;
}

/**
 * 实体 → frontend api/types 模块目录（如 logistics/maintenance）
 * @param {string} entityPascal
 * @param {string} [backendRoot]
 * @returns {string}
 */
function resolveEntityModuleDir(entityPascal, backendRoot = CONFIG.backendRoot) {
  const entityFile = findDomainEntityFile(entityPascal, backendRoot);
  if (!entityFile) {
    return '';
  }
  const entitiesRoot = path.join(backendRoot, 'Takt.Domain', 'Entities');
  const relDir = path.dirname(path.relative(entitiesRoot, entityFile)).replace(/\\/g, '/');
  if (relDir === '.') {
    return '';
  }
  return relDir.split('/').map((seg) => pascalToKebab(seg)).join('/');
}

/**
 * 子实体 frontend api/types 文件名（与 api 目录平齐，如 work-order、notification）
 * @param {string} childPascal
 * @param {string} [fallbackModuleDir]
 * @returns {string}
 */
function resolveChildEntityFrontendKebab(childPascal, fallbackModuleDir = '') {
  const apiPath = resolveApiFilePathForEntity(childPascal);
  if (apiPath) {
    return camelToKebab(parseFrontendApiFileCamel(apiPath));
  }
  const moduleDir = resolveEntityModuleDir(childPascal) || fallbackModuleDir.replace(/\\/g, '/').toLowerCase();
  const rawKebab = pascalToKebab(childPascal);
  return resolveFrontendModuleFileName(rawKebab, moduleDir);
}

/**
 * 定位实体对应 frontend api 文件
 * @param {string} entityPascal
 * @param {string} [backendRoot]
 * @param {string} [frontendRoot]
 * @returns {string|null}
 */
function resolveApiFilePathForEntity(entityPascal, backendRoot = CONFIG.backendRoot, frontendRoot = CONFIG.frontendRoot) {
  const moduleDir = resolveEntityModuleDir(entityPascal, backendRoot);
  const rawKebab = pascalToKebab(entityPascal);
  /** @type {string[]} */
  const candidates = [];
  const pushCandidate = (value) => {
    const normalized = String(value || '').trim();
    if (normalized && !candidates.includes(normalized)) {
      candidates.push(normalized);
    }
  };
  // 优先实体全名（如 procurement 模块下 PurchaseRequest → purchase-request，不与 procurement 混剥 purchase-）
  const outputRel = resolveFrontendOutputRelPath(moduleDir, rawKebab);
  pushCandidate(outputRel.importPath);
  pushCandidate(resolveFrontendModuleFileName(rawKebab, moduleDir));
  pushCandidate(rawKebab);
  const permPrefix = resolvePermissionPrefixFromController(entityPascal);
  if (permPrefix) {
    const segments = permPrefix.split(':');
    pushCandidate(segments[segments.length - 1]);
  }
  // 仅兼容历史短名 alias（request/order/price）；canonical 文件存在时不会命中
  if (/^purchase/i.test(entityPascal)) {
    pushCandidate(rawKebab.replace(/^purchase-/, ''));
    pushCandidate(resolveFrontendModuleFileName(rawKebab.replace(/^purchase-/, ''), moduleDir));
  }
  if (/^sales/i.test(entityPascal)) {
    pushCandidate(rawKebab.replace(/^sales-/, ''));
    pushCandidate(resolveFrontendModuleFileName(rawKebab.replace(/^sales-/, ''), moduleDir));
  }
  const apiBase = path.join(frontendRoot, CONFIG.apiDir);
  for (const fileKebab of candidates) {
    const candidatePath = path.join(apiBase, toFrontendApiFileName(fileKebab));
    if (fs.existsSync(candidatePath)) {
      return candidatePath;
    }
    // 兼容迁移前残留
    const legacyPath = path.join(apiBase, `${fileKebab}.ts`);
    if (fs.existsSync(legacyPath)) {
      return legacyPath;
    }
  }
  return null;
}

/**
 * 在子实体上查找指向主实体的 ManyToOne（外键一致）
 * @param {string} childPascal
 * @param {string} masterPascal
 * @param {string} fkFieldOnChild
 * @param {string} [backendRoot]
 * @returns {object|null}
 */
function findManyToOneOnChild(childPascal, masterPascal, fkFieldOnChild, backendRoot = CONFIG.backendRoot) {
  const childFile = findDomainEntityFile(childPascal, backendRoot);
  if (!childFile) {
    return null;
  }
  return parseManyToOneNavigations(childFile).find(
    (nav) => nav.masterShort === masterPascal && nav.fkField === fkFieldOnChild,
  ) || null;
}

/**
 * 在主实体上查找指向子实体的 OneToMany（外键一致）
 * @param {string} masterPascal
 * @param {string} childPascal
 * @param {string} fkFieldOnChild
 * @param {string} [backendRoot]
 * @returns {object|null}
 */
function findOneToManyOnMaster(masterPascal, childPascal, fkFieldOnChild, backendRoot = CONFIG.backendRoot) {
  const masterFile = findDomainEntityFile(masterPascal, backendRoot);
  if (!masterFile) {
    return null;
  }
  return parseOneToManyNavigations(masterFile).find(
    (nav) => nav.childShort === childPascal && nav.foreignKeyOnChild === fkFieldOnChild,
  ) || null;
}

/**
 * 扫描全库 OneToMany ↔ ManyToOne 成对关联
 * @param {string} [backendRoot]
 * @returns {object[]}
 */
function buildMasterDetailAssociations(backendRoot = CONFIG.backendRoot) {
  /** @type {object[]} */
  const associations = [];
  collectDomainEntityFiles().forEach((masterFile) => {
    const classMatch = fs.readFileSync(masterFile, 'utf-8').match(/public\s+(?:sealed\s+)?class\s+Takt(\w+)\s*:/);
    if (!classMatch) {
      return;
    }
    const masterPascal = classMatch[1];
    parseOneToManyNavigations(masterFile).forEach((o2m) => {
      if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(o2m.childShort)) {
        return;
      }
      const inverse = findManyToOneOnChild(o2m.childShort, masterPascal, o2m.foreignKeyOnChild, backendRoot);
      if (!inverse) {
        return;
      }
      associations.push({
        masterPascal,
        childPascal: o2m.childShort,
        masterNavProp: o2m.navPropName,
        childNavProp: inverse.navPropName,
        fkFieldOnChild: o2m.foreignKeyOnChild,
        fieldName: pascalToCamel(o2m.navPropName),
        moduleDir: resolveEntityModuleDir(masterPascal, backendRoot),
      });
    });
  });
  return associations;
}

/** @type {object[]|null} */
let associationCache = null;

/**
 * @param {string} [backendRoot]
 * @returns {object[]}
 */
function getMasterDetailAssociations(backendRoot = CONFIG.backendRoot) {
  if (!associationCache) {
    associationCache = buildMasterDetailAssociations(backendRoot);
  }
  return associationCache;
}

/**
 * 重置关联缓存（测试/长进程）
 */
function resetMasterDetailAssociationCache() {
  associationCache = null;
}

/**
 * @param {string} masterPascal
 * @returns {object[]}
 */
function listAssociationsForMaster(masterPascal) {
  return getMasterDetailAssociations().filter((a) => a.masterPascal === masterPascal);
}

/**
 * @param {string} childPascal
 * @returns {object[]}
 */
function listAssociationsForChild(childPascal) {
  return getMasterDetailAssociations().filter((a) => a.childPascal === childPascal);
}

/**
 * 主实体 OneToMany 子实体短名（级联 generate-all 用）
 * @param {string} masterPascal
 * @param {string} [backendRoot]
 * @returns {string[]}
 */
function getOneToManyChildShortNamesForEntity(masterPascal, backendRoot = CONFIG.backendRoot) {
  const masterFile = findDomainEntityFile(masterPascal, backendRoot);
  if (!masterFile) {
    return [];
  }
  return parseOneToManyNavigations(masterFile)
    .filter((nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort))
    .map((nav) => nav.childShort);
}

/**
 * 子表是否已与主表 ManyToOne 成对
 * @param {string} masterPascal
 * @param {string} childPascal
 * @param {string} fkFieldOnChild
 * @returns {boolean}
 */
function isPairedMasterDetailAssociation(masterPascal, childPascal, fkFieldOnChild) {
  return Boolean(findManyToOneOnChild(childPascal, masterPascal, fkFieldOnChild)
    && findOneToManyOnMaster(masterPascal, childPascal, fkFieldOnChild));
}

/**
 * 校验主表各 OneToMany 在子表均有 ManyToOne 反向导航
 * @param {string} entityPascal
 * @param {object[]} [children] buildFieldMeta 解析结果
 */
function validateMasterDetailChildrenManyToOnePairs(entityPascal, children = []) {
  const masterFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!masterFile) {
    return;
  }
  const navs = parseOneToManyNavigations(masterFile).filter(
    (nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort),
  );
  navs.forEach((nav) => {
    const inverse = findManyToOneOnChild(nav.childShort, entityPascal, nav.foreignKeyOnChild);
    if (!inverse) {
      console.warn(
        `⚠️  Takt${entityPascal}.${nav.navPropName} → Takt${nav.childShort} 缺少子表 ManyToOne（外键 ${nav.foreignKeyOnChild}）`,
      );
      return;
    }
    const childMeta = children.find((c) => c.childPascal === nav.childShort);
    if (childMeta && childMeta.masterFkField && childMeta.masterFkField !== nav.foreignKeyOnChild
      && childMeta.masterFkField.toLowerCase() !== nav.foreignKeyOnChild.toLowerCase()) {
      console.warn(
        `⚠️  外键不一致: 实体 ${nav.foreignKeyOnChild} vs types ${childMeta.masterFkField}（Takt${nav.childShort}）`,
      );
    }
  });
}

/**
 * 校验实体作为 master/child 的关联导航是否成对
 * @param {string} entityPascal
 */
function validateEntityMasterDetailAssociations(entityPascal) {
  const masterFile = findDomainEntityFile(entityPascal, CONFIG.backendRoot);
  if (!masterFile) {
    return;
  }
  parseOneToManyNavigations(masterFile)
    .filter((nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort))
    .forEach((nav) => {
      if (!findManyToOneOnChild(nav.childShort, entityPascal, nav.foreignKeyOnChild)) {
        console.warn(
          `⚠️  [关联未成对] 主 Takt${entityPascal}.${nav.navPropName} OneToMany → Takt${nav.childShort}，` +
          `子表无 ManyToOne(nameof(${nav.foreignKeyOnChild}))`,
        );
      }
    });
  const childFile = masterFile;
  parseManyToOneNavigations(childFile).forEach((m2o) => {
    if (RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(entityPascal)) {
      return;
    }
    const inverse = findOneToManyOnMaster(m2o.masterShort, entityPascal, m2o.fkField);
    if (!inverse) {
      console.warn(
        `⚠️  [关联未成对] 子 Takt${entityPascal}.${m2o.navPropName} ManyToOne → Takt${m2o.masterShort}，` +
        `主表无 OneToMany 反向（外键 ${m2o.fkField}；可空外键/非主子表场景可忽略）`,
      );
    }
  });
}

/**
 * 主实体 OneToMany 子实体列表（级联 generate-all 用；不要求子表 ManyToOne 反向导航）
 * @param {string} masterPascal
 * @param {string} [backendRoot]
 * @returns {object[]}
 */
function listOneToManyChildrenForMaster(masterPascal, backendRoot = CONFIG.backendRoot) {
  const masterFile = findDomainEntityFile(masterPascal, backendRoot);
  if (!masterFile) {
    return [];
  }
  return parseOneToManyNavigations(masterFile)
    .filter((nav) => !RBAC_ASSOCIATION_ENTITY_SHORT_NAMES.has(nav.childShort))
    .map((nav) => ({
      masterPascal,
      childPascal: nav.childShort,
      childEntity: nav.childEntity,
      masterNavProp: nav.navPropName,
      fkFieldOnChild: nav.foreignKeyOnChild,
      fieldName: pascalToCamel(nav.navPropName),
      moduleDir: resolveEntityModuleDir(masterPascal, backendRoot),
    }));
}

/**
 * 按主表 OneToMany 声明顺序深度优先，收集「全部子表 → 主表」实体短名序列
 * @param {string} rootMasterPascal 根主实体短名（如 PurchaseOrder）
 * @param {string} [backendRoot]
 * @returns {string[]} 如 [PurchaseOrderItem, PurchaseOrder]
 */
function collectOneToManyCascadeEntityOrder(rootMasterPascal, backendRoot = CONFIG.backendRoot) {
  /** @type {string[]} */
  const ordered = [];
  /** @type {Set<string>} */
  const visited = new Set();

  /**
   * @param {string} masterPascal
   */
  function walkMaster(masterPascal) {
    listOneToManyChildrenForMaster(masterPascal, backendRoot).forEach((assoc) => {
      if (visited.has(assoc.childPascal)) {
        return;
      }
      walkMaster(assoc.childPascal);
      visited.add(assoc.childPascal);
      ordered.push(assoc.childPascal);
    });
  }

  walkMaster(rootMasterPascal);
  ordered.push(rootMasterPascal);
  return ordered;
}

/**
 * 格式化 OneToMany 级联树（日志用）
 * @param {string} rootMasterPascal
 * @param {string} [backendRoot]
 * @returns {string[]}
 */
function formatOneToManyCascadeTreeLines(rootMasterPascal, backendRoot = CONFIG.backendRoot) {
  /** @type {string[]} */
  const lines = [];

  /**
   * @param {string} masterPascal
   * @param {number} depth
   */
  function walk(masterPascal, depth) {
    listOneToManyChildrenForMaster(masterPascal, backendRoot).forEach((assoc, index, arr) => {
      const prefix = depth === 0 ? `${index + 1}/${arr.length}` : `${index + 1}`;
      lines.push(
        `${'  '.repeat(depth)}${prefix}. Takt${masterPascal}.${assoc.masterNavProp} → ` +
        `Takt${assoc.childPascal}（外键 ${assoc.fkFieldOnChild}）`,
      );
      walk(assoc.childPascal, depth + 1);
    });
  }

  walk(rootMasterPascal, 0);
  return lines;
}

/**
 * 按主表 OneToMany 遍历子实体（generate-all 子表优先流水线）
 * @param {string} masterPascal
 * @param {(childPascal: string, assoc: object) => void} callback
 */
function forEachOneToManyChildAssociation(masterPascal, callback) {
  listOneToManyChildrenForMaster(masterPascal).forEach((assoc) => {
    callback(assoc.childPascal, assoc);
  });
}

/**
 * 按主表 OneToMany ↔ 子表 ManyToOne 成对遍历（仅已配对关联）
 * @param {string} masterPascal
 * @param {(childPascal: string, assoc: object) => void} callback
 */
function forEachPairedChildAssociation(masterPascal, callback) {
  listAssociationsForMaster(masterPascal).forEach((assoc) => {
    callback(assoc.childPascal, assoc);
  });
}

module.exports = {
  parseManyToOneNavigations,
  resolveEntityModuleDir,
  resolveApiFilePathForEntity,
  resolveChildEntityFrontendKebab,
  findManyToOneOnChild,
  findOneToManyOnMaster,
  buildMasterDetailAssociations,
  getMasterDetailAssociations,
  resetMasterDetailAssociationCache,
  listAssociationsForMaster,
  listAssociationsForChild,
  getOneToManyChildShortNamesForEntity,
  listOneToManyChildrenForMaster,
  collectOneToManyCascadeEntityOrder,
  formatOneToManyCascadeTreeLines,
  forEachOneToManyChildAssociation,
  isPairedMasterDetailAssociation,
  validateMasterDetailChildrenManyToOnePairs,
  validateEntityMasterDetailAssociations,
  forEachPairedChildAssociation,
};

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：permission-base.cjs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：权限前缀 buildPermissionBase；格式 领域:目录:实体词:…:操作（PascalCase 拆为冒号分段）；主子表继承主表
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const { isStandaloneChildVueEntity } = require('./generate-entity-exclusions.cjs');
const { buildMasterDetailChildRegistry } = require('./generate-vue-common.cjs');
const { PERMISSION_PATH_PART_ALIASES } = require('./generate-script-common.cjs');

/** @type {Map<string, { masterPascal: string, fieldName: string }>|undefined} */
let masterDetailChildRegistryCache;

/**
 * PascalCase → kebab-case
 * @param {string} value
 * @returns {string}
 */
function pascalToKebab(value) {
  return String(value || '').replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * 服务目录段 → 权限词数组（PascalCase 按词拆段，如 HumanResource→['human','resource']）
 * @param {string} part
 * @returns {string[]}
 */
function pathPartToPermissionWords(part) {
  if (!part) {
    return [];
  }
  if (PERMISSION_PATH_PART_ALIASES[part]) {
    return [PERMISSION_PATH_PART_ALIASES[part]];
  }
  return pascalToKebab(part).split('-').filter(Boolean);
}

/**
 * @deprecated 使用 pathPartToPermissionWords；保留兼容单段拼接场景
 * @param {string} part
 * @returns {string}
 */
function pathPartToPermissionSegment(part) {
  return pathPartToPermissionWords(part).join('');
}

/**
 * 实体短名 → 权限词数组（PascalCase 按词拆段，如 StandardWageRate→['standard','wage','rate']）
 * @param {string} entityShort
 * @returns {string[]}
 */
function entityToPermissionWords(entityShort) {
  return pascalToKebab(entityShort).split('-').filter(Boolean);
}

/**
 * 服务 pathParts → 权限路径段（含领域）
 * @param {string[]} pathParts
 * @returns {string[]}
 */
function buildPathPermissionSegments(pathParts) {
  if (!pathParts || !pathParts.length) {
    return [];
  }
  /** @type {string[]} */
  const segments = [];
  for (const part of pathParts) {
    segments.push(...pathPartToPermissionWords(part));
  }
  return segments;
}

/**
 * 主子表从实体注册表（懒加载）
 * @returns {Map<string, { masterPascal: string, fieldName: string }>}
 */
function getMasterDetailChildRegistry() {
  if (!masterDetailChildRegistryCache) {
    masterDetailChildRegistryCache = buildMasterDetailChildRegistry();
  }
  return masterDetailChildRegistryCache;
}

/**
 * 实体全名权限前缀（不含操作末段）：领域 + 目录 + 实体各词
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @returns {string}
 */
function buildPermissionBaseFull(pathParts, entityShort) {
  const segments = buildPathPermissionSegments(pathParts);
  const entityWords = entityToPermissionWords(entityShort);
  /** @type {string[]} */
  const result = [...segments];
  const pathWordSet = new Set(segments);
  // EngineeringChange 实体短名以 Ec 开头（EcPur、EcTe）时跳过词 ec，
  // 与菜单 logistics:manufacturing:engineering:change:{dept} 对齐；EcGroup 仍保留 :ec:group。
  const skipLeadingEc =
    segments.includes('engineering')
    && segments.includes('change')
    && entityShort.startsWith('Ec')
    && entityShort !== 'EcGroup';
  for (let i = 0; i < entityWords.length; i += 1) {
    const word = entityWords[i];
    if (pathWordSet.has(word)) {
      continue;
    }
    if (skipLeadingEc && i === 0 && word === 'ec') {
      continue;
    }
    result.push(word);
  }
  return result.join(':');
}

/**
 * 主子表从实体：主表权限前缀 + 子实体相对主表的多余词段
 * 例：Overtime + OvertimeItem → …:overtime:item；News + NewsAttachment → …:news:attachment
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @param {string} masterPascal
 * @param {Set<string>} seen
 * @returns {string|null}
 */
function buildMasterDetailChildPermissionBase(pathParts, entityShort, masterPascal, seen) {
  if (!entityShort.startsWith(masterPascal)) {
    return null;
  }
  const masterPerm = buildPermissionBase(pathParts, masterPascal, seen);
  const childWords = entityToPermissionWords(entityShort);
  const masterWords = entityToPermissionWords(masterPascal);
  if (childWords.length <= masterWords.length) {
    return masterPerm;
  }
  const masterPrefix = masterWords.join('-');
  const childPrefix = childWords.slice(0, masterWords.length).join('-');
  if (masterPrefix !== childPrefix) {
    return null;
  }
  const suffix = childWords.slice(masterWords.length);
  if (!suffix.length) {
    return masterPerm;
  }
  return `${masterPerm}:${suffix.join(':')}`;
}

/**
 * 权限前缀（不含操作末段 list/create/…）
 * 规则：领域:业务目录:实体各词；主子表（实体名以主表名为前缀）继承主表前缀
 * @param {string[]} pathParts 相对 Services 的目录段
 * @param {string} entityShort 实体短名
 * @param {Set<string>} [visited] 防环
 * @returns {string}
 */
function buildPermissionBase(pathParts, entityShort, visited) {
  const seen = visited || new Set();
  if (seen.has(entityShort)) {
    return buildPermissionBaseFull(pathParts, entityShort);
  }
  seen.add(entityShort);

  const masterRef = getMasterDetailChildRegistry().get(entityShort);
  if (masterRef && !isStandaloneChildVueEntity(entityShort)) {
    const childBase = buildMasterDetailChildPermissionBase(
      pathParts,
      entityShort,
      masterRef.masterPascal,
      seen,
    );
    if (childBase) {
      return childBase;
    }
  }

  return buildPermissionBaseFull(pathParts, entityShort);
}

/**
 * @deprecated 新格式无单独 special；保留兼容导出
 * @returns {null}
 */
function buildSpecialPermissionBase() {
  return null;
}

/**
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @returns {string}
 */
function buildPermissionBaseDefault(pathParts, entityShort) {
  return buildPermissionBaseFull(pathParts, entityShort);
}

const CRUD_ACTION_SUFFIXES = new Set([
  'list', 'query', 'create', 'update', 'delete', 'import', 'export',
  'tree', 'approve', 'addsign', 'reducesign', 'transfer', 'withdraw',
  'suspend', 'resume', 'terminate', 'design', 'publish', 'start', 'clone', 'preview',
]);

/**
 * 检测权限码是否与 buildPermissionBase 期望不一致（旧连写 slug 或主子表未继承）
 * @param {string} permissionCode 完整权限码
 * @param {string} [expectedPrefix] 若提供则直接比对
 * @returns {{ actualPrefix: string, suggestedPrefix: string }|null}
 */
function detectPermissionStemRedundancy(permissionCode, expectedPrefix) {
  const raw = String(permissionCode || '').trim().toLowerCase();
  if (!raw || raw.includes('.')) {
    return null;
  }
  const parts = raw.split(':').filter(Boolean);
  if (parts.length < 2) {
    return null;
  }
  const last = parts[parts.length - 1];
  if (!CRUD_ACTION_SUFFIXES.has(last)) {
    return null;
  }
  const actualPrefix = parts.slice(0, -1).join(':');
  const suggestedPrefix = String(expectedPrefix || '').trim().toLowerCase();
  if (!suggestedPrefix || actualPrefix === suggestedPrefix) {
    return null;
  }
  return { actualPrefix, suggestedPrefix };
}

/**
 * 沿主子表链追溯到根主表实体短名（控制器权限继承用）
 * @param {string} entityShort
 * @returns {string}
 */
function resolveRootMasterPascal(entityShort) {
  /** @type {Set<string>} */
  const seen = new Set();
  let current = entityShort;
  while (true) {
    if (seen.has(current)) {
      break;
    }
    seen.add(current);
    const masterRef = getMasterDetailChildRegistry().get(current);
    if (!masterRef || isStandaloneChildVueEntity(current)) {
      break;
    }
    current = masterRef.masterPascal;
  }
  return current;
}

/**
 * 控制器权限前缀（主子表从实体继承根主表前缀，不含子实体后缀）
 * @param {string[]} pathParts
 * @param {string} entityShort
 * @returns {string}
 */
function buildControllerPermissionBase(pathParts, entityShort) {
  const masterRef = getMasterDetailChildRegistry().get(entityShort);
  if (masterRef && !isStandaloneChildVueEntity(entityShort)) {
    const rootMaster = resolveRootMasterPascal(entityShort);
    return buildPermissionBase(pathParts, rootMaster);
  }

  // 非主子表从实体：使用自己的权限前缀
  return buildPermissionBase(pathParts, entityShort);
}

module.exports = {
  buildPermissionBase,
  buildControllerPermissionBase,
  buildSpecialPermissionBase,
  buildPermissionBaseDefault,
  buildPermissionBaseFull,
  buildPathPermissionSegments,
  entityToPermissionWords,
  pathPartToPermissionSegment,
  pascalToKebab,
  detectPermissionStemRedundancy,
};

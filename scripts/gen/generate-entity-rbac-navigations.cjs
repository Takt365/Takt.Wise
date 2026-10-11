// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-entity-rbac-navigations.cjs
// 创建时间：2026-06-01
// 创建人：Takt365(Cursor AI)
// 功能描述：按 rbac-parent-config 为主实体写入 RBAC OneToMany 导航属性区域
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile, logGeneratedFileWritePolicy, parseSingleEntityGenerateArgs } = require('./generate-script-common.cjs');
const { assertNotManualDtoEntityCli } = require('./generate-entity-exclusions.cjs');
const {
  NAVIGATION_REGION_MARKER,
  RBAC_PARENT_NAVIGATIONS,
  getRbacParentNavigations,
  collectRbacJunctionUsings,
} = require('./rbac-parent-config.cjs');

const ENTITIES_ROOT = path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities');

/**
 * @param {string} dir
 * @param {string} entityShort
 */
function findEntityFile(dir, entityShort) {
  const target = `Takt${entityShort}.cs`;
  const entries = fs.readdirSync(dir, { withFileTypes: true });
  for (const entry of entries) {
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      const found = findEntityFile(full, entityShort);
      if (found) {
        return found;
      }
      continue;
    }
    if (entry.name === target) {
      return full;
    }
  }
  return null;
}

/**
 * @param {string} content
 */
function parseEntityNamespace(content) {
  const m = content.match(/namespace\s+([\w.]+)\s*;/);
  return m ? m[1] : '';
}

/**
 * @param {string} content
 */
function mergeUsings(content, entityNamespace, entityShort) {
  const needed = collectRbacJunctionUsings(entityNamespace, entityShort);
  if (!needed.length) {
    return content;
  }
  const lines = content.split('\n');
  let insertAt = 0;
  for (let i = 0; i < lines.length; i += 1) {
    if (lines[i].startsWith('using ')) {
      insertAt = i + 1;
    } else if (lines[i].trim() === '' && insertAt > 0) {
      break;
    } else if (!lines[i].startsWith('//') && lines[i].trim() !== '' && insertAt > 0) {
      break;
    }
  }
  const existing = new Set(
    lines.filter((l) => l.startsWith('using ')).map((l) => l.trim()),
  );
  const toAdd = needed
    .map((ns) => `using ${ns};`)
    .filter((u) => !existing.has(u));
  if (!toAdd.length) {
    return content;
  }
  const out = [...lines.slice(0, insertAt), ...toAdd, ...lines.slice(insertAt)];
  return out.join('\n');
}

/**
 * 从导航区正文解析已有 [Navigate] 属性块（含紧邻 summary）
 * @param {string} navRegionBody 含「导航属性区域」标记之后的类体片段
 * @returns {{ navProp: string, block: string }[]}
 */
function extractExistingNavigateBlocks(navRegionBody) {
  const blocks = [];
  const re =
    /(\s*\/\/\/\s*<summary>[\s\S]*?\/\/\/\s*<\/summary>\s*)?\[Navigate\([\s\S]*?\)\]\s*public\s+List<(?:Takt\w+)>\??\s+(\w+)\s*\{\s*get;\s*set;\s*\}/g;
  let match;
  while ((match = re.exec(navRegionBody)) !== null) {
    blocks.push({
      navProp: match[2],
      block: match[0].replace(/^\n+/, '').replace(/\n+$/, ''),
    });
  }
  return blocks;
}

/**
 * @param {object[]} navSpecs RBAC 配置项
 * @param {{ navProp: string, block: string }[]} [preserveBlocks] 非 RBAC 业务导航（须保留）
 */
function buildNavigationRegionLines(navSpecs, preserveBlocks = []) {
  const lines = [];
  lines.push('    // ========================================');
  lines.push(`    // ${NAVIGATION_REGION_MARKER}`);
  lines.push('    // ========================================');
  lines.push('');
  navSpecs.forEach((spec) => {
    lines.push('    /// <summary>');
    lines.push(`    /// ${spec.summary}（RBAC，表 ${spec.table}）`);
    lines.push('    /// </summary>');
    lines.push(
      `    [Navigate(NavigateType.OneToMany, nameof(Takt${spec.junction}.${spec.fkOnChild}))]`,
    );
    lines.push(`    public List<Takt${spec.junction}>? ${spec.navProp} { get; set; }`);
    lines.push('');
  });
  // 保留手工/业务 OneToMany（如 EmployeeAddresses），避免整区覆盖时被删掉
  preserveBlocks.forEach((item) => {
    const normalized = item.block
      .split('\n')
      .map((line) => (line.startsWith('    ') ? line : `    ${line.trimStart()}`))
      .join('\n');
    lines.push(normalized);
    lines.push('');
  });
  return lines.join('\n');
}

/**
 * 替换「导航属性区域」：RBAC 项按配置重写，其余 [Navigate] 原样保留
 * @param {string} content
 * @param {object[]} navSpecs
 */
function replaceNavigationRegion(content, navSpecs) {
  const classMatch = content.match(/public\s+class\s+Takt\w+[\s\S]*?\{/);
  if (!classMatch) {
    return null;
  }
  const openIdx = classMatch.index + classMatch[0].length - 1;
  let depth = 1;
  let i = openIdx + 1;
  while (i < content.length && depth > 0) {
    if (content[i] === '{') {
      depth += 1;
    } else if (content[i] === '}') {
      depth -= 1;
    }
    i += 1;
  }
  const classEndIdx = i - 1;
  const classBody = content.slice(openIdx + 1, classEndIdx);
  const markerIdx = classBody.indexOf(NAVIGATION_REGION_MARKER);
  const rbacNavProps = new Set(navSpecs.map((s) => s.navProp));
  let preserveBlocks = [];
  let scalarBody;
  if (markerIdx >= 0) {
    let navStart = classBody.lastIndexOf('// ========================================', markerIdx);
    if (navStart < 0) {
      navStart = markerIdx;
    }
    const existingNavRegion = classBody.slice(navStart);
    preserveBlocks = extractExistingNavigateBlocks(existingNavRegion).filter(
      (b) => !rbacNavProps.has(b.navProp),
    );
    scalarBody = classBody.slice(0, navStart).replace(/\s+$/, '');
  } else {
    // 无标记时：从整段类体提取非 RBAC 导航，并从标量区剥离全部导航块
    const allBlocks = extractExistingNavigateBlocks(classBody);
    preserveBlocks = allBlocks.filter((b) => !rbacNavProps.has(b.navProp));
    let stripped = classBody;
    allBlocks.forEach((b) => {
      stripped = stripped.replace(b.block, '');
    });
    scalarBody = stripped.replace(/\s+$/, '');
  }
  const navigationBlock = buildNavigationRegionLines(navSpecs, preserveBlocks);
  const sep = scalarBody.length ? '\n\n' : '\n';
  const newBody = `${scalarBody}${sep}${navigationBlock}\n`;
  return content.slice(0, openIdx + 1) + newBody + content.slice(classEndIdx);
}

/**
 * @param {string} entityShort
 * @param {{ dryRun?: boolean }} [options]
 */
function syncEntityRbacNavigations(entityShort, options = {}) {
  const navSpecs = getRbacParentNavigations(entityShort);
  if (!navSpecs.length) {
    return { status: 'skipped', reason: 'no-config' };
  }
  const entityFile = findEntityFile(ENTITIES_ROOT, entityShort);
  if (!entityFile) {
    return { status: 'failed', reason: 'file-not-found' };
  }
  let content = fs.readFileSync(entityFile, 'utf-8');
  const entityNamespace = parseEntityNamespace(content);
  content = mergeUsings(content, entityNamespace, entityShort);
  const updated = replaceNavigationRegion(content, navSpecs);
  if (!updated) {
    return { status: 'failed', reason: 'parse-failed' };
  }
  if (options.dryRun) {
    return { status: 'dry-run', path: entityFile };
  }
  const writeResult = writeGeneratedFile(entityFile, updated);
  return {
    status: writeResult.created ? 'created' : 'updated',
    path: entityFile,
    created: writeResult.created,
    updated: writeResult.updated,
  };
}

/**
 * @param {string} entitiesRoot
 * @param {{ dryRun?: boolean, entityPrefix?: string|null }} [options]
 */
function syncAllRbacParentEntityNavigations(entitiesRoot, options = {}) {
  const root = entitiesRoot || ENTITIES_ROOT;
  const results = [];
  Object.keys(RBAC_PARENT_NAVIGATIONS).forEach((entityShort) => {
    if (options.entityPrefix && options.entityPrefix !== entityShort) {
      return;
    }
    const file = findEntityFile(root, entityShort);
    const r = syncEntityRbacNavigations(entityShort, { ...options, entityFile: file });
    results.push({ entityShort, ...r });
  });
  return results;
}

function printUsage() {
  console.log(`
用法: node scripts/generate-entity-rbac-navigations.cjs [参数]

参数:
  --<实体名>         仅同步指定实体，如 --Role
  --dry-run          仅打印，不写入

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 配置源：scripts/gen/rbac-parent-config.cjs → RBAC_PARENT_NAVIGATIONS
  - 仅重写配置中的 RBAC [Navigate]；「导航属性区域」内其它业务 OneToMany（如 EmployeeAddresses）原样保留
  - 须先于 generate-dtos-from-entity.cjs（DTO 从实体导航生成 List<关联Dto> 与 Create *Ids）
`);
}

function parseArgs() {
  const options = parseSingleEntityGenerateArgs(printUsage);
  assertNotManualDtoEntityCli(options.entityPrefix);
  return options;
}

if (require.main === module) {
  console.log('🚀 同步主实体 RBAC 导航属性...\n');
  logGeneratedFileWritePolicy();
  const options = parseArgs();
  const results = syncAllRbacParentEntityNavigations(ENTITIES_ROOT, options);
  results.forEach((r) => {
    if (r.status === 'skipped') {
      console.log(`  ⏭️  ${r.entityShort}: 无配置`);
    } else if (r.status === 'failed') {
      console.log(`  ❌ ${r.entityShort}: ${r.reason}`);
    } else if (r.status === 'dry-run') {
      console.log(`  📄 [dry-run] ${r.path}`);
    } else {
      console.log(`  ✅ ${r.entityShort}: ${r.path}`);
    }
  });
  console.log('\n✨ 完成');
}

module.exports = {
  syncEntityRbacNavigations,
  syncAllRbacParentEntityNavigations,
  findEntityFile,
};

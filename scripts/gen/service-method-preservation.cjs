// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts/gen
// 文件名称：service-method-preservation.cjs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：应用服务非标准 CRUD 方法保留（generate-services-from-dtos 合并用）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 构建代码生成器会产出的标准服务方法名集合
 * @param {string} entityShort 不含 Takt 前缀
 * @param {object} dtoInfo extractDtoInfo 结果
 * @param {string} crudType Tree | MasterDetail | 其他
 * @returns {Set<string>}
 */
function buildStandardServiceMethodNames(entityShort, dtoInfo, crudType) {
  const names = new Set([
    `Get${entityShort}ListAsync`,
    `Get${entityShort}ByIdAsync`,
    `Get${entityShort}OptionsAsync`,
    `Create${entityShort}Async`,
    `Update${entityShort}Async`,
    `Delete${entityShort}ByIdAsync`,
    `Delete${entityShort}BatchAsync`,
    `Export${entityShort}Async`,
  ]);
  const hasTree = crudType === 'Tree' || dtoInfo.tree;
  if (hasTree) {
    names.add(`Get${entityShort}TreeAsync`);
    names.add(`Get${entityShort}TreeOptionsAsync`);
  }
  if (dtoInfo.sort) {
    names.add(`Update${entityShort}SortAsync`);
  }
  if (dtoInfo.builtIn) {
    names.add(`Update${entityShort}BuiltInAsync`);
  }
  if (dtoInfo.obsolete) {
    names.add(`Update${entityShort}ObsoleteAsync`);
  }
  if (dtoInfo.template) {
    names.add(`Get${entityShort}TemplateAsync`);
  }
  if (dtoInfo.import) {
    names.add(`Import${entityShort}Async`);
  }
  const entityName = dtoInfo.entityName || `Takt${entityShort}`;
  for (const statusDto of dtoInfo.statuses || []) {
    let suffix = statusDto.replace(entityName, '').replace(/StatusDto$/, '');
    if (suffix.startsWith(entityShort)) {
      suffix = suffix.slice(entityShort.length);
    }
    if (!suffix) {
      suffix = 'Status';
    }
    names.add(`Update${entityShort}${suffix}Async`);
  }
  names.add(`Get${entityShort}TransposedListAsync`);
  names.add(`Save${entityShort}TransposedBatchAsync`);
  return names;
}

/**
 * 从接口或实现类文本中提取 Async 方法名
 * @param {string} content
 * @param {'interface'|'implementation'} variant
 * @returns {string[]}
 */
function listServiceMethodNames(content, variant) {
  if (!content) {
    return [];
  }
  const names = new Set();
  const patterns =
    variant === 'interface'
      ? [/\bTask(?:<[^>]+>)?\s+(\w+Async)\s*\(/g]
      : [/\bpublic\s+(?:async\s+)?Task(?:<[^>]+>)?\s+(\w+Async)\s*\(/g];
  for (const re of patterns) {
    let m;
    while ((m = re.exec(content)) !== null) {
      names.add(m[1]);
    }
  }
  return [...names];
}

/**
 * 提取方法前的 XML 文档
 * @param {string} content
 * @param {number} signatureStart
 * @returns {string}
 */
function extractLeadingXmlDocBeforeSignature(content, signatureStart) {
  const before = content.slice(0, signatureStart);
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
 * 平衡花括号块切片
 * @param {string} content
 * @param {number} openBraceIndex
 * @returns {string|null}
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
 * 提取单个服务方法块（含 XML 注释）
 * @param {string} content
 * @param {string} methodName
 * @param {'interface'|'implementation'} variant
 * @returns {string|null}
 */
function extractServiceMethodBlock(content, methodName, variant) {
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
    if (!/\bTask\b/.test(line) || !line.includes(';')) {
      return null;
    }
    const semi = content.indexOf(';', anchorIndex);
    if (semi < 0) {
      return null;
    }
    let sigStart = lineStart;
    const prevLineEnd = content.lastIndexOf('\n', lineStart - 2);
    const prevLine = content.slice(prevLineEnd + 1, lineStart - 1);
    if (/\bTask\b/.test(prevLine) && !/^\s*\/\/\//.test(line.trim())) {
      sigStart = prevLineEnd + 1;
    }
    const doc = extractLeadingXmlDocBeforeSignature(content, sigStart);
    let block = doc + content.slice(sigStart, semi + 1);
    if (!block.endsWith('\n\n')) {
      block += block.endsWith('\n') ? '\n' : '\n\n';
    }
    return block;
  }

  if (!/^\s*public\b/.test(line) || !/\bTask\b/.test(line)) {
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
 * 提取嵌套类型/私有类型块（如 ConfigurableRuntimeBundle）
 * @param {string} content
 * @param {string} typeName
 * @returns {string|null}
 */
function extractNestedTypeBlock(content, typeName) {
  const re = new RegExp(`\\b(?:private\\s+)?(?:sealed\\s+)?class\\s+${typeName}\\b`);
  const match = re.exec(content);
  if (!match) {
    return null;
  }
  const lineStart = content.lastIndexOf('\n', match.index) + 1;
  const openBrace = content.indexOf('{', match.index);
  if (openBrace < 0) {
    return null;
  }
  const body = sliceBalancedBraceBlock(content, openBrace);
  if (!body) {
    return null;
  }
  let block = content.slice(lineStart, openBrace) + body;
  if (!block.endsWith('\n\n')) {
    block += block.endsWith('\n') ? '\n' : '\n\n';
  }
  return block;
}

/**
 * 从已有服务文件中保留非标准方法
 * @param {string|null} content 已有接口或实现全文
 * @param {string} entityShort
 * @param {object} dtoInfo
 * @param {string} crudType
 * @param {'interface'|'implementation'} variant
 * @returns {{ blocks: string, methodNames: string[] }}
 */
function extractPreservedExtraServiceMethods(content, entityShort, dtoInfo, crudType, variant) {
  if (!content) {
    return { blocks: '', methodNames: [] };
  }
  const standard = buildStandardServiceMethodNames(entityShort, dtoInfo, crudType);
  const existing = listServiceMethodNames(content, variant);
  const extraNames = existing.filter((name) => !standard.has(name));
  const blocks = [];
  for (const name of extraNames) {
    const block = extractServiceMethodBlock(content, name, variant);
    if (block) {
      blocks.push(block);
    }
  }
  if (variant === 'implementation') {
    const nested = extractNestedTypeBlock(content, 'ConfigurableRuntimeBundle');
    if (nested && !blocks.some((b) => b.includes('ConfigurableRuntimeBundle'))) {
      blocks.unshift(nested);
    }
  }
  return {
    blocks: blocks.join(''),
    methodNames: extraNames,
  };
}

module.exports = {
  buildStandardServiceMethodNames,
  listServiceMethodNames,
  extractServiceMethodBlock,
  extractPreservedExtraServiceMethods,
};

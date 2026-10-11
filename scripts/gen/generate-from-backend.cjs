// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-from-backend.cjs
// 创建时间：2026-05-22
// 创建人：Takt365(Cursor AI)
// 功能描述：根据后端C#代码自动生成前端TypeScript类型定义和API方法（存在则覆盖更新，不存在则创建）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const {
  writeGeneratedFile,
  singularizeControllerSegment,
  entityShortFromControllerClassName,
  matchControllerForEntityPrefix,
  pluralizeEntityShort,
  logGeneratedFileWritePolicy,
  parseSingleEntityGenerateArgs,
  sanitizeXmlDocPlainText,
  resolveFrontendModuleFileName,
  resolveFrontendOutputRelPath,
  toFrontendApiFileName,
  toFrontendTypesFileName,
  toFrontendApiImportPath,
  toFrontendTypesImportPath,
} = require('./generate-script-common.cjs');
const {
  shouldExcludeDtoSourceBase,
  shouldExcludeController,
  assertNotManualDtoEntityCli,
} = require('./generate-entity-exclusions.cjs');

// ========================================
// 配置区
// ========================================

const CONFIG = {
  // 后端项目根目录
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  
  // 前端项目根目录
  frontendRoot: path.resolve(__dirname, '../../frontend'),
  
  // 输出目录
  output: {
    types: 'src/types',
    api: 'src/api',
  },
  
  // 控制器路由映射（仅特殊路由需要配置，默认自动生成）
  controllerRoutes: {
    'TaktAuthsController': '/api/TaktAuths',
  },
};

/**
 * DTO 类名 → 前端类型文件路径索引（processDtos 填充，供 API 多文件 import）
 * @type {Map<string, { moduleName: string, typeFile: string }>}
 */
const DTO_TYPE_INDEX = new Map();

// ========================================
// 工具函数
// ========================================

/**
 * 保留 Takt 前缀的框架/公共类型（非实体 *Dtos.cs 业务 DTO）
 */
const FRAMEWORK_TS_TYPE_NAMES = new Set([
  'TaktPagedQuery',
  'TaktPagedResult',
  'TaktSelectOption',
  'TaktTreeSelectOption',
  'TaktApiResult',
]);

/** 后端 DTO/Query 基类（映射为 frontend/src/types/common.types.ts 中的类型） */
const COMMON_TYPES_FROM_COMMON_MODULE = new Set([
  'TaktTenantCoreDtoBase',
  'TaktTenantCultureDtoBase',
  'TaktTenantPlantDtoBase',
  'TaktTenantDtoBase',
  'TaktCompanyDtoBase',
  'TaktApprovalDtoBase',
  'TaktPagedQuery',
]);

/**
 * 后端 DTO 类名 → 前端类型名（去 Takt 前缀、去末尾 Dto）
 * 例：TaktCompanyDto → Company，TaktCompanyQueryDto → CompanyQuery，TaktCompanyCreateDto → CompanyCreate
 * @param {string} className C# 类名
 * @returns {string} 前端 export 类型名
 */
function dtoClassToFrontendTypeName(className) {
  if (!className || !className.startsWith('Takt')) {
    return className;
  }
  if (FRAMEWORK_TS_TYPE_NAMES.has(className)) {
    return className;
  }
  let name = className.slice(4);
  if (name.endsWith('Dto')) {
    name = name.slice(0, -3);
  }
  return name;
}

/**
 * 将 TS 类型字符串中的 Takt*Dto 替换为前端类型名（含泛型，如 TaktPagedResult<TaktUserDto>）
 * @param {string} tsType
 * @returns {string}
 */
function mapDtoTypesInTsType(tsType) {
  if (!tsType || typeof tsType !== 'string') {
    return tsType;
  }
  if (tsType.startsWith('TaktPagedResult<')) {
    const match = tsType.match(/TaktPagedResult<(.+)>/);
    if (match) {
      return `TaktPagedResult<${mapDtoTypesInTsType(match[1].trim())}>`;
    }
  }
  return tsType.replace(/\bTakt[A-Z][A-Za-z0-9]*Dto\b/g, (match) => dtoClassToFrontendTypeName(match));
}

/**
 * PascalCase转camelCase
 */
function pascalToCamel(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

/**
 * PascalCase转kebab-case（用于目录名）
 */
function pascalToKebab(str) {
  return str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();
}

/**
 * 后端目录名转前端目录名
 * 例：HumanResource -> human-resource, Organization -> organization
 */
function backendDirToFrontendDir(dirName) {
  return pascalToKebab(dirName);
}

/**
 * 从DTO类名生成文件名
 * 例：TaktUserDto -> user, TaktUserCompanyDto -> user-company
 */
function dtoClassToFileName(className) {
  // 去掉 Takt 前缀和 Dto 后缀
  let name = className.replace(/^Takt/, '').replace(/Dto$/, '');
  return pascalToKebab(name);
}

/**
 * 从控制器类名生成文件名
 * 例：TaktUserController -> user, TaktUserCompanyController -> user-company
 */
/**
 * 从控制器类名生成前端 API 文件名（kebab-case，实体单数）
 * 例：TaktHolidaysController -> holiday，TaktUsersController -> user
 * @param {string} className 控制器类名
 * @returns {string}
 */
function controllerClassToFileName(className) {
  const segment = className.replace(/^Takt/, '').replace(/Controller$/, '');
  const singular = singularizeControllerSegment(segment);
  return pascalToKebab(singular);
}

/**
 * 从后端 *Dtos.cs 文件名生成前端类型文件名
 * 例：TaktUserDtos.cs → user，TaktUserRoleDtos.cs → user-role，TaktLoginDtos.cs → login
 * @param {string} dtosSourceBase 不含扩展名，如 TaktUserDtos
 */
function dtosSourceFileToOutputName(dtosSourceBase) {
  const name = dtosSourceBase.replace(/^Takt/, '').replace(/Dtos$/, '');
  return pascalToKebab(name);
}

/**
 * C#类型转TypeScript类型
 */
function csharpToTsType(csharpType, isNullable = false) {
  const typeMap = {
    'string': 'string',
    'int': 'number',
    'int?': 'number',
    'long': 'string', // 雪花ID，前端用string
    'long?': 'string',
    'float': 'number',
    'float?': 'number',
    'double': 'number',
    'double?': 'number',
    'decimal': 'number',
    'decimal?': 'number',
    'bool': 'boolean',
    'bool?': 'boolean',
    'DateTime': 'string',
    'DateTime?': 'string',
    'DateTimeOffset': 'string',
    'DateTimeOffset?': 'string',
    'Guid': 'string',
    'Guid?': 'string',
    'object': 'any',
    'void': 'void',
  };

  // Dictionary<K,V>
  if (csharpType.startsWith('Dictionary<')) {
    const dictMatch = csharpType.match(/Dictionary<([^,>]+),\s*([^>]+)>/);
    if (dictMatch) {
      return `Record<string, ${csharpToTsType(dictMatch[2].trim())}>`;
    }
  }

  // IReadOnlyList<string>
  if (csharpType.startsWith('IReadOnlyList<')) {
    const inner = csharpType.match(/IReadOnlyList<(.+)>/)?.[1];
    return inner ? `${csharpToTsType(inner)}[]` : 'string[]';
  }

  // 处理List<T>
  if (csharpType.startsWith('List<')) {
    const innerType = csharpType.match(/List<(.+)>/)?.[1];
    return innerType ? `${csharpToTsType(innerType)}[]` : 'unknown[]';
  }

  // 框架/公共类型（如 List<TaktSelectOption>）
  if (FRAMEWORK_TS_TYPE_NAMES.has(csharpType)) {
    return csharpType;
  }

  // DTO 类型名 → 前端短名（主子表 List<TaktXxxDto>、导航属性）
  if (/^Takt\w+Dto/.test(csharpType)) {
    return dtoClassToFrontendTypeName(csharpType);
  }

  // 处理枚举类型（Takt 前缀且非 Dto、非框架类型）
  if (csharpType.startsWith('Takt') && !typeMap[csharpType]) {
    return 'number';
  }

  return typeMap[csharpType] || 'any';
}

/**
 * 将 C# /// 文档块转为可解析的 XML 片段
 * @param {string} docBlock 含 /// 的注释块
 * @returns {string}
 */
function csharpDocToXml(docBlock) {
  if (!docBlock) return '';
  return docBlock
    .split(/\r?\n/)
    .map((line) => line.replace(/^\s*\/\/\/\s?/, ''))
    .join('\n');
}

/**
 * 清理 summary 内文（去除 /// 前缀、XML 标签行）
 * @param {string} text
 * @returns {string[]}
 */
function cleanSummaryLines(text) {
  if (!text) return [];
  const raw = text.includes('<summary>') ? extractSummary(text) : text;
  return raw
    .split(/\r?\n/)
    .map((line) => line.replace(/^\s*\/\/\/\s?/, '').trim())
    .map((line) => line.replace(/\s+/g, ' '))
    .filter((line) => line && !/^<\/?summary>$/.test(line));
}

/**
 * 规范化文档文本为单行（属性注释用）
 * @param {string} text 已是 summary 内文或完整 XML
 * @returns {string}
 */
function normalizeDocText(text) {
  const lines = cleanSummaryLines(text);
  return sanitizeXmlDocPlainText(lines.join(' ').trim());
}

/**
 * 将文档文本拆为多行（用于 JSDoc，保留后端换行语义）
 * @param {string} text
 * @returns {string[]}
 */
function formatSummaryLines(text) {
  return cleanSummaryLines(text);
}

/**
 * 从XML注释中提取summary（保留原始文本）
 */
function extractSummary(xmlComment) {
  if (!xmlComment) return '';
  const match = xmlComment.match(/<summary>([\s\S]*?)<\/summary>/);
  if (!match) return '';
  
  // 保留原始文本，只去除首尾空白
  return match[1].trim();
}

/**
 * 从XML注释中提取param
 */
function extractParams(xmlComment) {
  if (!xmlComment) return [];
  const params = [];
  const regex = /<param\s+name="([^"]+)">([\s\S]*?)<\/param>/g;
  let match;
  while ((match = regex.exec(xmlComment)) !== null) {
    params.push({
      name: match[1],
      description: match[2].trim().replace(/\s+/g, ' '),
    });
  }
  return params;
}

/**
 * 从XML注释中提取returns
 */
function extractReturns(xmlComment) {
  if (!xmlComment) return '';
  const match = xmlComment.match(/<returns>([\s\S]*?)<\/returns>/);
  return match ? match[1].trim().replace(/\s+/g, ' ') : '';
}

// ========================================
// 解析C# DTO文件
// ========================================

/**
 * 解析单个DTO类
 */
function parseDtoClass(content, className) {
  // 匹配类定义（含前置 /// 文档）
  const classRegex = new RegExp(
    `((?:\\s*///[^\\n]*\\n)+)?public class ${className}(\\s*:\\s*[\\w<>\\s,]+)?\\s*\\{([\\s\\S]*?)\\n\\}`,
    'g'
  );
  const classMatch = classRegex.exec(content);
  
  if (!classMatch) return null;

  const classDocBlock = classMatch[1] || '';
  const classSummary = extractSummary(csharpDocToXml(classDocBlock));
  const inheritance = classMatch[2] || '';
  const classBody = classMatch[3];

  // 提取属性（支持属性前面有多个特性标签）
  const properties = [];
  // 匹配模式：单个属性的summary注释 + 可选的特性标签 + 类型 + 属性名 + { get; set; }
  const propertyRegex =
    /\/\/\/\s*<summary>([\s\S]*?)<\/summary>[\s\S]*?public\s+(?:required\s+)?(.+?)\s+(\w+)\s*\{[\s\S]*?get;\s*set;/g;
  let propMatch;

  while ((propMatch = propertyRegex.exec(classBody)) !== null) {
    const summary = normalizeDocText(propMatch[1]);
    const type = propMatch[2].trim();
    const name = propMatch[3];
    const isNullable = type.includes('?');
    
    properties.push({
      name: pascalToCamel(name),
      type: csharpToTsType(type.replace('?', ''), isNullable),
      isNullable: isNullable || type.includes('?'),
      summary,
      csharpType: type,
    });
  }

  return {
    name: className,
    frontendName: dtoClassToFrontendTypeName(className),
    classSummary,
    inheritance,
    properties,
  };
}

/**
 * 解析DTO文件中的所有类
 */
function parseDtoFile(filePath) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const classes = [];
  
  // 匹配所有public class XxxDto（包含Dto在类名中的任意位置）
  const classRegex = /public class (Takt\w*Dto\w*)/g;
  let match;
  
  while ((match = classRegex.exec(content)) !== null) {
    const className = match[1];
    const dto = parseDtoClass(content, className);
    if (dto) {
      classes.push(dto);
    }
  }
  
  return classes;
}

// ========================================
// 解析C#控制器文件
// ========================================

/**
 * 解析控制器路由元信息（相对 request baseURL `/api`）
 */
function parseControllerMeta(content, controllerClassName) {
  const routeMatch = content.match(
    /\[Route\("([^"]+)"(?:\s*,\s*Name\s*=\s*"([^"]*)")?\)\]/,
  );
  const routeTemplate = routeMatch ? routeMatch[1] : 'api/[controller]';
  const routeDisplayName = routeMatch && routeMatch[2] ? routeMatch[2] : null;
  const controllerSegment = controllerClassName.replace(/Controller$/, '');
  let apiPath = routeTemplate.replace('[controller]', controllerSegment).replace(/^\//, '');
  // request baseURL 已是 /api，相对路径不再带 api/ 前缀
  apiPath = apiPath.replace(/^api\//, '');
  return {
    controllerSegment,
    apiPath,
    routeDisplayName,
  };
}

/**
 * 从特性块解析 HTTP 动词与路由模板
 */
function parseHttpRoute(attributes) {
  const verbs = [
    { key: 'HttpGet', method: 'get' },
    { key: 'HttpPost', method: 'post' },
    { key: 'HttpPut', method: 'put' },
    { key: 'HttpDelete', method: 'delete' },
  ];
  for (const verb of verbs) {
    const regex = new RegExp(`\\[${verb.key}(?:\\("([^"]*)"\\))?\\]`);
    const match = attributes.match(regex);
    if (match) {
      return { httpMethod: verb.method, routeTemplate: match[1] ?? '' };
    }
  }
  return { httpMethod: 'get', routeTemplate: '' };
}

/**
 * 解析 C# 方法参数列表
 */
function parseCsharpMethodParams(paramString) {
  if (!paramString || !paramString.trim()) return [];
  const params = [];
  const parts = paramString.split(',').map((p) => p.trim()).filter(Boolean);
  for (const part of parts) {
    const bindingMatch = part.match(/\[(FromQuery|FromBody|FromRoute|FromForm)\]/);
    let cleaned = part
      .replace(/\[(FromQuery|FromBody|FromRoute|FromForm)\]/g, '')
      .replace(/\[(Required|AllowAnonymous)([^\]]*)\]/gi, '')
      .replace(/\[TaktPermission[^\]]*\]/gi, '')
      .trim();
    cleaned = cleaned.replace(/\s*=\s*null\s*$/i, '').trim();
    cleaned = cleaned.replace(/\s*=\s*[^,]+$/, '').trim();
    const tokens = cleaned.split(/\s+/).filter(Boolean);
    const paramName = tokens[tokens.length - 1].replace(/\?$/, '');
    const csharpType = tokens.slice(0, -1).join(' ').replace(/\?$/, '').trim();
    params.push({
      name: pascalToCamel(paramName),
      csharpName: paramName,
      csharpType,
      binding: bindingMatch ? bindingMatch[1] : null,
      bindingExplicit: Boolean(bindingMatch),
      isOptional: part.includes('= null') || part.includes('?'),
    });
  }
  return params;
}

/**
 * 推断未标注参数的来源（路由 / 查询 / 正文）
 */
function inferParamBinding(param, routeTemplate, httpMethod) {
  if (param.bindingExplicit) return param.binding;
  if (param.csharpType === 'IFormFile') return 'FromForm';
  if (routeTemplate && routeTemplate.includes(`{${param.csharpName}}`)) return 'FromRoute';
  if (param.csharpType.startsWith('Takt') && param.csharpType.includes('Dto')) {
    return httpMethod === 'get' ? 'FromQuery' : 'FromBody';
  }
  if (param.csharpType === 'IEnumerable<long>' || param.csharpType === 'long[]') {
    return 'FromBody';
  }
  if (httpMethod === 'get' || httpMethod === 'delete') return 'FromQuery';
  return 'FromRoute';
}

/**
 * 控制器对应的前端 types 导入路径
 */
function getTypesImportPath(moduleName, controllerName, entityPrefix) {
  const entity = entityPrefix || entityShortFromControllerClassName(controllerName);
  const rawTypeFile = dtosSourceFileToOutputName(`Takt${entity}Dtos`);
  const { importPath } = resolveFrontendOutputRelPath(moduleName, rawTypeFile);
  return `@/types/${toFrontendTypesImportPath(importPath)}`;
}

/**
 * 根据 DTO 类名解析类型定义文件导入路径（依赖 processDtos 建立的 DTO_TYPE_INDEX）
 * @param {string} dtoClassName 如 TaktUserRoleDto
 * @param {string} moduleName 模块目录，如 identity
 */
function getTypeImportPathForDtoClass(dtoClassName, moduleName) {
  if (dtoClassName === 'TaktSelectOption' || dtoClassName === 'TaktTreeSelectOption') {
    return '@/types/common.types';
  }
  const backendName = dtoClassName.startsWith('Takt')
    ? dtoClassName
    : [...DTO_TYPE_INDEX.entries()].find(([, v]) => v.frontendName === dtoClassName)?.[0];
  const indexed = DTO_TYPE_INDEX.get(dtoClassName) || (backendName && DTO_TYPE_INDEX.get(backendName));
  if (indexed) {
    const importPath = indexed.importPath || `${indexed.moduleName}/${indexed.typeFile}`;
    return `@/types/${toFrontendTypesImportPath(importPath)}`;
  }
  const typeFile = dtoClassToFileName(backendName || dtoClassName);
  return `@/types/${toFrontendTypesImportPath(`${moduleName}/${typeFile}`)}`;
}

/**
 * 将 DTO 类型名按目标 types 文件分组，生成多条 import type
 * @param {Set<string>} dtoTypeNames
 * @param {string} moduleName
 * @param {string} [fallbackImportPath]
 */
/**
 * 从 API 返回类型字符串收集需 import 的 Takt 类型名
 * @param {string} returnType 如 TaktUserRoleDto[]、TaktPagedResult<TaktUserDto>
 * @returns {string[]}
 */
function collectTsTypesFromReturnType(returnType) {
  const types = [];
  if (!returnType || returnType === 'void' || returnType === 'unknown' || returnType === 'Blob') {
    return types;
  }
  if (returnType.startsWith('TaktPagedResult<')) {
    types.push('TaktPagedResult');
    const match = returnType.match(/TaktPagedResult<(.+)>/);
    if (match) {
      const inner = match[1].trim();
      if (inner && !FRAMEWORK_TS_TYPE_NAMES.has(inner)) {
        types.push(inner);
      }
    }
    return types;
  }
  if (returnType.endsWith('[]')) {
    types.push(returnType.slice(0, -2));
    return types;
  }
  if (returnType.startsWith('Takt') && !FRAMEWORK_TS_TYPE_NAMES.has(returnType)) {
    types.push(dtoClassToFrontendTypeName(returnType));
    return types;
  }
  if (/^[A-Z]/.test(returnType)) {
    types.push(returnType);
  }
  return types;
}

/**
 * 在 Services 目录查找控制器注入的应用服务接口文件
 * @param {string} controllerContent 控制器源码
 * @returns {string|null} 接口文件绝对路径
 */
function findServiceInterfaceFile(controllerContent) {
  const match = controllerContent.match(/I(Takt\w+Service)/);
  if (!match) {
    return null;
  }
  const interfaceFileName = `I${match[1]}.cs`;
  const servicesRoot = path.join(CONFIG.backendRoot, 'Takt.Application', 'Services');

  function walk(dir) {
    const entries = fs.readdirSync(dir, { withFileTypes: true });
    for (const entry of entries) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        const found = walk(fullPath);
        if (found) {
          return found;
        }
      } else if (entry.name === interfaceFileName) {
        return fullPath;
      }
    }
    return null;
  }

  if (!fs.existsSync(servicesRoot)) {
    return null;
  }
  return walk(servicesRoot);
}

/**
 * 解析 Task / Task&lt;T&gt; 片段中的返回类型与方法名
 * @param {string} fragment 以 Task 开头的片段
 */
function parseTaskReturnFragment(fragment) {
  let index = 4;
  while (index < fragment.length && fragment[index] === ' ') {
    index += 1;
  }
  if (fragment[index] !== '<') {
    const methodMatch = fragment.slice(index).match(/^(\w+Async)/);
    if (!methodMatch) {
      return null;
    }
    return { methodName: methodMatch[1], returnType: '' };
  }
  index += 1;
  let depth = 1;
  const typeStart = index;
  while (index < fragment.length && depth > 0) {
    const ch = fragment[index];
    if (ch === '<') {
      depth += 1;
    } else if (ch === '>') {
      depth -= 1;
    }
    index += 1;
  }
  const returnType = fragment.substring(typeStart, index - 1).trim();
  const methodMatch = fragment.slice(index).trim().match(/^(\w+Async)/);
  if (!methodMatch) {
    return null;
  }
  return { methodName: methodMatch[1], returnType };
}

/**
 * 解析应用服务接口中各 Async 方法的 C# 返回类型（Task 泛型内）
 * @param {string} interfaceFilePath ITaktXxxService.cs 路径
 * @returns {Map<string, string>} 方法名 → C# 返回类型（Task 无泛型则为空串表示 void）
 */
function parseServiceInterfaceReturns(interfaceFilePath) {
  const content = fs.readFileSync(interfaceFilePath, 'utf-8');
  const map = new Map();
  let searchFrom = 0;
  while (searchFrom < content.length) {
    const taskIndex = content.indexOf('Task', searchFrom);
    if (taskIndex === -1) {
      break;
    }
    const parsed = parseTaskReturnFragment(content.slice(taskIndex));
    if (parsed && parsed.methodName.endsWith('Async')) {
      map.set(parsed.methodName, parsed.returnType);
    }
    searchFrom = taskIndex + 4;
  }
  return map;
}

/**
 * 将应用服务 C# 返回类型转为前端 data 类型（对应 request 解包后）
 * @param {string} csharpReturn C# Task 泛型内的类型
 */
function csharpServiceReturnToTsType(csharpReturn) {
  if (!csharpReturn) {
    return 'void';
  }
  let type = csharpReturn.trim().replace(/\?$/, '');
  if (type.startsWith('(')) {
    return '{ success: number; fail: number; errors: string[] }';
  }
  if (type.startsWith('List<') && type.endsWith('>')) {
    const inner = type.slice(5, -1).trim();
    return `${csharpServiceReturnToTsType(inner)}[]`;
  }
  if (type.startsWith('IEnumerable<') && type.endsWith('>')) {
    const inner = type.slice(12, -1).trim();
    return `${csharpServiceReturnToTsType(inner)}[]`;
  }
  if (type === 'long' || type === 'int' || type === 'float' || type === 'double' || type === 'decimal') {
    return 'number';
  }
  if (type === 'bool') {
    return 'boolean';
  }
  if (type === 'string') {
    return 'string';
  }
  if (type.startsWith('TaktPagedResult<')) {
    const inner = type.match(/TaktPagedResult<(.+)>/);
    if (inner) {
      return mapDtoTypesInTsType(
        `TaktPagedResult<${csharpServiceReturnToTsType(inner[1].trim())}>`,
      );
    }
  }
  if (/^Takt\w+Dto/.test(type)) {
    return dtoClassToFrontendTypeName(type);
  }
  if (type.startsWith('Takt')) {
    return type;
  }
  return csharpToTsType(type);
}

function buildGroupedTypeImports(dtoTypeNames, moduleName, fallbackImportPath, aliasEntityFile = false) {
  const groups = new Map();
  dtoTypeNames.forEach((typeName) => {
    if (FRAMEWORK_TS_TYPE_NAMES.has(typeName)) {
      return;
    }
    const lookupName =
      DTO_TYPE_INDEX.get(typeName)?.backendName ||
      (typeName.startsWith('Takt') ? typeName : null);
    const indexed = lookupName ? DTO_TYPE_INDEX.get(lookupName) : DTO_TYPE_INDEX.get(typeName);
    const importPath =
      (indexed && `@/types/${toFrontendTypesImportPath(`${indexed.moduleName}/${indexed.typeFile}`)}`) ||
      getTypeImportPathForDtoClass(lookupName || typeName, moduleName) ||
      fallbackImportPath;
    if (!importPath) {
      return;
    }
    if (!groups.has(importPath)) {
      groups.set(importPath, new Set());
    }
    groups.get(importPath).add(typeName);
  });
  const lines = [];
  [...groups.keys()].sort().forEach((importPath) => {
    formatTypeImportLines([...groups.get(importPath)].sort(), importPath, aliasEntityFile).forEach((l) => lines.push(l));
  });
  return lines;
}

/**
 * C# 参数类型转 TS（含 DTO）
 */
function csharpParamToTsType(csharpType) {
  if (csharpType === 'IFormFile') return 'globalThis.File';
  if (csharpType === 'IEnumerable<long>' || csharpType === 'long[]') return 'string[]';
  if (csharpType.startsWith('Takt') && csharpType.includes('Dto')) {
    return dtoClassToFrontendTypeName(csharpType);
  }
  return csharpToTsType(csharpType);
}

/**
 * 后端方法名转前端导出函数名（camelCase + 导入导出约定）
 */
function methodNameToExportName(methodName) {
  const base = pascalToCamel(methodName);
  if (base === 'importUser') return 'importUserData';
  if (base === 'exportUser') return 'exportUserData';
  if (base === 'exportLoginLog') return 'exportLoginLogData';
  if (base === 'importLoginLog') return 'importLoginLogData';
  return base;
}

/**
 * 拼接 API 相对路径（不含 /api 前缀，与 request baseURL 配合）
 */
function buildApiRelativePath(apiPath, routeTemplate) {
  const base = apiPath.replace(/\/$/, '');
  if (!routeTemplate) return base;
  return `${base}/${routeTemplate}`.replace(/\/+/g, '/');
}

/**
 * 将路径模板 {id} 转为 TS 模板字符串片段
 */
function pathTemplateToTs(urlPath) {
  return urlPath.replace(/\{(\w+)\}/g, '${$1}');
}

/**
 * 解析控制器方法
 */
function parseControllerFile(filePath, controllerClassName) {
  const content = fs.readFileSync(filePath, 'utf-8');
  const meta = parseControllerMeta(content, controllerClassName);
  const methods = [];

  const serviceInterfacePath = findServiceInterfaceFile(content);
  const serviceReturnMap = serviceInterfacePath
    ? parseServiceInterfaceReturns(serviceInterfacePath)
    : new Map();

  const methodRegex = /((?:\s*\/\/\/[^\n]*\n)+)\s*((?:\[[^\]]*\]\s*)*)public\s+async\s+Task<[^>]+>\s+(\w+)Async\s*\(([^)]*)\)/g;
  let match;

  while ((match = methodRegex.exec(content)) !== null) {
    const xmlDoc = csharpDocToXml(match[1]);
    const summary = extractSummary(xmlDoc);
    const attributes = match[2] || '';
    const methodName = match[3];
    const paramString = match[4] || '';
    const { httpMethod, routeTemplate } = parseHttpRoute(attributes);

    // OAuth 根路径 /connect/* 由 frontend/src/config/oauth.ts 与 oauth-authorize.ts 处理，不走 /api/TaktAuths
    if (routeTemplate.startsWith('~/connect')) {
      continue;
    }

    let params = parseCsharpMethodParams(paramString);
    const docParams = extractParams(xmlDoc);
    const returns = extractReturns(xmlDoc);

    params = params.map((p) => ({
      ...p,
      binding: inferParamBinding(p, routeTemplate, httpMethod),
    }));

    const exportName = methodNameToExportName(methodName);
    params = normalizeControllerMethodParams({ exportName, params });

    params.forEach((p) => {
      const doc = docParams.find((d) => d.name === p.csharpName);
      if (doc) p.description = doc.description;
    });

    const routeSuffix = routeTemplate || '';
    const isBlob = /template|export|download/i.test(methodName) && httpMethod === 'get';
    const isFormData = params.some((p) => p.csharpType === 'IFormFile');

    const methodNameAsync = `${methodName}Async`;
    const serviceReturnType = serviceReturnMap.has(methodNameAsync)
      ? serviceReturnMap.get(methodNameAsync)
      : undefined;

    methods.push({
      exportName: methodNameToExportName(methodName),
      httpMethod,
      routeSuffix,
      summary,
      params,
      returns,
      isBlob,
      isFormData,
      methodNameAsync,
      serviceReturnType,
    });
  }

  return { meta, methods, serviceInterfacePath };
}

// ========================================
// 生成TypeScript类型定义
// ========================================

/**
 * 生成单个DTO的TypeScript接口
 */
function generateTsInterface(dto, moduleName) {
  const lines = [];
  
  const classSummaryLines = formatSummaryLines(dto.classSummary);
  lines.push(`/**`);
  if (classSummaryLines.length > 0) {
    classSummaryLines.forEach((line) => lines.push(` * ${line}`));
  } else {
    lines.push(` * ${dto.name}`);
  }
  lines.push(` * 对应前端 ${dto.frontendName}`);
  lines.push(` * @description 对应后端 ${dto.name}`);
  lines.push(` */`);
  
  // 处理继承关系
  let extendsClause = '';
  if (dto.inheritance) {
    // 提取继承的类名
    const inheritMatch = dto.inheritance.match(/:\s*(.+)/);
    if (inheritMatch) {
      const baseClass = inheritMatch[1].trim();
      const tsBaseClass = dtoClassToFrontendTypeName(baseClass);
      extendsClause = ` extends ${tsBaseClass}`;
    }
  }
  
  lines.push(`export interface ${dto.frontendName}${extendsClause} {`);
  
  dto.properties.forEach(prop => {
    const propSummaryLines = formatSummaryLines(prop.summary);
    lines.push(`  /**`);
    if (propSummaryLines.length > 0) {
      propSummaryLines.forEach((line) => lines.push(`   * ${line}`));
    } else {
      lines.push(`   * ${prop.name}`);
    }
    lines.push(`   */`);
    lines.push(`  ${prop.name}${prop.isNullable ? '?' : ''}: ${prop.type};`);
    lines.push('');
  });
  
  lines.push('}');
  lines.push('');
  
  return lines.join('\n');
}

/**
 * 解析 public class X : BaseA, BaseB 中的基类 C# 名列表
 * @param {string} inheritance 如 " : TaktCompanyDtoBase"
 * @returns {string[]}
 */
function parseInheritanceBaseNames(inheritance) {
  if (!inheritance) {
    return [];
  }
  const match = inheritance.match(/:\s*(.+)/);
  if (!match) {
    return [];
  }
  return match[1]
    .split(',')
    .map((part) => part.trim().replace(/<[\s\S]*$/, '').trim())
    .filter(Boolean);
}

/**
 * 收集 types 文件需从 @/types/common.types 导入的继承基类（Tenant/Company/Approval DtoBase、TaktPagedQuery）
 * @param {object[]} dtos parseDtoClass 结果
 * @returns {string[]} 前端类型名（已排序）
 */
function collectCommonBaseTypeImports(dtos) {
  const imports = new Set();
  dtos.forEach((dto) => {
    parseInheritanceBaseNames(dto.inheritance).forEach((baseCsName) => {
      if (!COMMON_TYPES_FROM_COMMON_MODULE.has(baseCsName)) {
        return;
      }
      const tsName = FRAMEWORK_TS_TYPE_NAMES.has(baseCsName)
        ? baseCsName
        : dtoClassToFrontendTypeName(baseCsName);
      imports.add(tsName);
    });
  });
  return [...imports].sort();
}

/**
 * 生成完整的类型定义文件
 */
/**
 * 收集当前 types 文件需从其他模块导入的 DTO 类型
 */
function collectCrossModuleTypeImports(dtos, currentModuleName) {
  const groups = new Map();
  /** 匹配前端短名（Company、UserRole）及尚未转换的 Takt*Dto */
  const typePattern = /\b(?:Takt[A-Z][A-Za-z0-9]*Dto|[A-Z][A-Za-z0-9]+)\b/g;

  dtos.forEach((dto) => {
    dto.properties.forEach((prop) => {
      const raw = `${prop.type}`;
      const candidates = new Set();
      raw.match(typePattern)?.forEach((token) => {
        if (token.startsWith('Takt')) {
          candidates.add(dtoClassToFrontendTypeName(token));
        } else if (!['string', 'number', 'boolean', 'any', 'void', 'Record'].includes(token)) {
          candidates.add(token);
        }
      });
      candidates.forEach((typeName) => {
        const indexed =
          DTO_TYPE_INDEX.get(typeName) ||
          [...DTO_TYPE_INDEX.entries()].find(([, v]) => v.frontendName === typeName)?.[1];
        if (!indexed || indexed.moduleName === currentModuleName) {
          return;
        }
        const key = `@/types/${toFrontendTypesImportPath(`${indexed.moduleName}/${indexed.typeFile}`)}`;
        if (!groups.has(key)) {
          groups.set(key, new Set());
        }
        groups.get(key).add(typeName);
      });
    });
  });

  const lines = [];
  [...groups.keys()].sort().forEach((importPath) => {
    formatTypeImportLines([...groups.get(importPath)].sort(), importPath).forEach((l) => lines.push(l));
  });
  if (lines.length) {
    lines.push('');
  }
  return lines;
}

function generateTypesFile(dtos, moduleName, typeFileName) {
  const lines = [];
  
  // 文件头
  lines.push(`// ========================================`);
  lines.push(`// 项目名称：节拍智控·TaktWise`);
  lines.push(`// 命名空间：frontend/src/types/${moduleName}`);
  lines.push(`// 文件名称：${toFrontendTypesFileName(typeFileName)}`);
  lines.push(`// 创建时间：${new Date().toISOString().split('T')[0]}`);
  lines.push(`// 创建人：Takt365(Auto Generated)`);
  lines.push(
    `// 功能描述：${moduleName} 模块类型定义（自动生成；类型名去 Takt 前缀与末尾 Dto，如 TaktCompanyDto → Company）`,
  );
  lines.push(`// `);
  lines.push(`// 版权信息：Copyright (c) 2025 Takt  All rights reserved.`);
  lines.push(`// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。`);
  lines.push(`// ========================================`);
  lines.push('');
  const commonBaseTypes = collectCommonBaseTypeImports(dtos);
  if (commonBaseTypes.length) {
    formatTypeImportLines(commonBaseTypes, '@/types/common.types').forEach((l) => lines.push(l));
  }
  const crossImports = collectCrossModuleTypeImports(dtos, moduleName);
  if (crossImports.length) {
    lines.push('');
    crossImports.forEach((l) => lines.push(l));
  } else {
    lines.push('');
  }
  
  // 生成每个DTO
  dtos.forEach(dto => {
    lines.push(generateTsInterface(dto, moduleName));
    lines.push('');
  });
  
  return lines.join('\n');
}

// ========================================
// 生成API方法文件
// ========================================

/**
 * 收集方法用到的 DTO 类型名
 */
function collectDtoTypes(methods) {
  const types = new Set();
  methods.forEach((method) => {
    method.params.forEach((p) => {
      if (p.csharpType.startsWith('Takt') && p.csharpType.includes('Dto')) {
        types.add(dtoClassToFrontendTypeName(p.csharpType));
      }
    });
  });
  return [...types].sort();
}

/**
 * 控制器对应 API 路径常量名（USER_API_BASE）
 */
function getApiBaseConstantName(controllerName) {
  const entity = entityShortFromControllerClassName(controllerName);
  const upper = entity.replace(/([a-z0-9])([A-Z])/g, '$1_$2').toUpperCase();
  return `${upper}_API_BASE`;
}

/**
 * 前端参数名（QueryDto -> queryDto）
 */
function toFrontendParamName(param) {
  if (param.csharpType.includes('QueryDto')) {
    return 'queryDto';
  }
  if (param.csharpType.includes('Dto') && param.name === 'dto') {
    return 'dto';
  }
  return param.name;
}

/**
 * 生成方法参数签名（多参数换行）
 */
function buildTsParamSignature(params) {
  if (params.length === 0) return '';
  const parts = params.map((p) => {
    const tsType = csharpParamToTsType(p.csharpType);
    const optional = p.isOptional ? '?' : '';
    const name = toFrontendParamName(p);
    return `${name}${optional}: ${tsType}`;
  });
  if (parts.length <= 2) {
    return parts.join(', ');
  }
  return parts.join(',\n  ');
}

/**
 * 生成 URL 模板表达式
 */
function buildUrlExpression(apiBaseVar, routeSuffix) {
  if (!routeSuffix) {
    return `\`\${${apiBaseVar}}\``;
  }
  const pathPart = pathTemplateToTs(routeSuffix);
  return `\`\${${apiBaseVar}}/${pathPart}\``;
}

/**
 * 是否为分页/导出 Query 参数（扁平 params 绑定 [FromQuery] DTO，禁止 params: { queryDto } 嵌套）
 * @param {object} param
 * @returns {boolean}
 */
function isFlatQueryParam(param) {
  if (!param) {
    return false;
  }
  const type = param.csharpType || '';
  const name = param.csharpName || param.name || '';
  return (
    type.includes('QueryDto')
    || type.endsWith('Query')
    || name === 'queryDto'
    || name === 'query'
  );
}

/**
 * 生成多行 request({ url, method, ... }) 调用
 */
function buildHttpCallLines(method, apiBaseVar) {
  const lines = [];
  const urlExpr = buildUrlExpression(apiBaseVar, method.routeSuffix);
  const queryParams = method.params.filter((p) => p.binding === 'FromQuery');
  const bodyParams = method.params.filter((p) => p.binding === 'FromBody');
  const fileParam = method.params.find((p) => p.csharpType === 'IFormFile');

  if (method.isFormData && fileParam) {
    lines.push('const formData = new FormData();');
    lines.push(`formData.append('file', ${toFrontendParamName(fileParam)});`);
    lines.push('');
  }

  lines.push('return request({');
  lines.push(`  url: ${urlExpr},`);
  lines.push(`  method: '${method.httpMethod}',`);

  if (method.isFormData && fileParam) {
    lines.push('  data: formData,');
    lines.push('  headers: {');
    lines.push("    'Content-Type': 'multipart/form-data',");
    lines.push('  },');
  } else if (bodyParams.length === 1) {
    lines.push(`  data: ${toFrontendParamName(bodyParams[0])},`);
  } else if (bodyParams.length > 1) {
    lines.push('  data: {');
    bodyParams.forEach((p, index) => {
      const comma = index < bodyParams.length - 1 ? ',' : '';
      lines.push(`    ${toFrontendParamName(p)}${comma}`);
    });
    lines.push('  },');
  }

  if (queryParams.length === 1 && isFlatQueryParam(queryParams[0])) {
    lines.push(`  params: ${toFrontendParamName(queryParams[0])},`);
  } else if (queryParams.length > 0) {
    lines.push('  params: {');
    queryParams.forEach((p, index) => {
      const comma = index < queryParams.length - 1 ? ',' : '';
      const name = toFrontendParamName(p);
      if (isFlatQueryParam(p)) {
        lines.push(`    ...${name}${comma}`);
      } else {
        lines.push(`    ${name}${comma}`);
      }
    });
    lines.push('  },');
  }

  if (method.isBlob) {
    lines.push("  responseType: 'blob',");
  }

  lines.push('});');
  return lines;
}

/**
 * 多行格式化 import type
 * @param {string[]} types
 * @param {string} fromPath
 * @param {boolean} [aliasEntityFile] 实体 File DTO 与 DOM File 冲突时 import 为 TaktFile
 */
function formatTypeImportLines(types, fromPath, aliasEntityFile = false) {
  if (types.length === 0) return [];
  const lines = ['import type {'];
  types.forEach((typeName, index) => {
    const suffix = index < types.length - 1 ? ',' : '';
    const entry = aliasEntityFile && typeName === 'File' ? 'File as TaktFile' : typeName;
    lines.push(`  ${entry}${suffix}`);
  });
  lines.push(`} from '${fromPath}';`);
  return lines;
}

/**
 * API 是否含 IFormFile 参数（浏览器 File）
 * @param {object[]} methods
 */
function apiMethodsUseDomFile(methods) {
  return methods.some((m) => m.params.some((p) => p.csharpType === 'IFormFile'));
}

/**
 * API 返回类型是否引用实体 File DTO
 * @param {object[]} methods
 * @param {string[]} dtoTypes
 */
function apiMethodsUseEntityFileType(methods, dtoTypes) {
  return methods.some((m) => {
    const ret = inferReturnTsType(m, dtoTypes);
    return ret === 'File' || ret.includes('<File>') || ret === 'File[]';
  });
}

/**
 * 实体 File DTO 在 API 层映射为 TaktFile，避免与 globalThis.File 冲突
 * @param {string} tsType
 * @param {boolean} aliasEntityFile
 */
function mapEntityFileTsType(tsType, aliasEntityFile) {
  if (!aliasEntityFile || !tsType) {
    return tsType;
  }
  if (tsType === 'File') {
    return 'TaktFile';
  }
  if (tsType === 'TaktPagedResult<File>') {
    return 'TaktPagedResult<TaktFile>';
  }
  if (tsType === 'File[]') {
    return 'TaktFile[]';
  }
  return tsType;
}

/**
 * API 方法分组标题
 */
/**
 * 按方法名修正 QueryDto / BatchDto 参数类型（避免 Transposed 与标准 Query 混淆）
 */
function normalizeControllerMethodParams(method) {
  const { exportName, params } = method;
  const transposedListMatch = exportName.match(/^get(\w+)TransposedList$/);
  const standardListMatch = exportName.match(/^get(\w+)List$/);
  const transposedBatchMatch = exportName.match(/^save(\w+)TransposedBatch$/);
  const exportMatch = exportName.match(/^export(\w+)$/);

  return params.map((p) => {
    const isQuery =
      p.csharpName === 'queryDto' ||
      p.csharpName === 'query' ||
      (p.csharpType && p.csharpType.includes('QueryDto'));
    if (isQuery) {
      if (transposedListMatch) {
        return {
          ...p,
          csharpType: dtoClassToFrontendTypeName(
            `Takt${transposedListMatch[1]}TransposedQueryDto`,
          ),
        };
      }
      if (standardListMatch) {
        return {
          ...p,
          csharpType: dtoClassToFrontendTypeName(`Takt${standardListMatch[1]}QueryDto`),
        };
      }
      if (exportMatch && !p.csharpType.includes('Transposed')) {
        return {
          ...p,
          csharpType: dtoClassToFrontendTypeName(`Takt${exportMatch[1]}QueryDto`),
        };
      }
    }
    if (transposedBatchMatch && (p.csharpName === 'dto' || p.csharpType.includes('BatchDto'))) {
      return {
        ...p,
        csharpType: dtoClassToFrontendTypeName(
          `Takt${transposedBatchMatch[1]}TransposedBatchDto`,
        ),
      };
    }
    return p;
  });
}

function getMethodSectionLabel(method) {
  const name = method.exportName;
  if (name.includes('Transposed')) {
    return '转置（多语言表格）';
  }
  if (name.includes('Template') || name.includes('import') || name.includes('export')) {
    return '导入导出';
  }
  if (/password|Password|unlock|forgot/i.test(name)) {
    return '密码与解锁';
  }
  if (/^assign|^getUser(Role|Dept|Post|Tenant)/i.test(name)) {
    return '关联分配';
  }
  if (name.endsWith('Count')) {
    return '统计';
  }
  if (name.includes('Options')) {
    return '选项';
  }
  return '基础 CRUD';
}

/**
 * 推断 API 方法返回类型（解包后的 data，对应 request 拦截器）
 */
function inferReturnTsType(method, dtoTypes) {
  if (method.isBlob) {
    return 'Blob';
  }
  if (method.serviceReturnType !== undefined) {
    return csharpServiceReturnToTsType(method.serviceReturnType);
  }
  const returnsText = method.returns || '';

  const transposedListMatch = method.exportName.match(/^get(\w+)TransposedList$/);
  if (transposedListMatch) {
    return dtoClassToFrontendTypeName(`Takt${transposedListMatch[1]}TransposedResultDto`);
  }

  const standardListMatch = method.exportName.match(/^get(\w+)List$/);
  if (standardListMatch || (method.exportName.endsWith('List') && returnsText.includes('分页'))) {
    if (standardListMatch) {
      return `TaktPagedResult<${dtoClassToFrontendTypeName(`Takt${standardListMatch[1]}Dto`)}>`;
    }
    const queryDto = method.params.find(
      (p) => p.csharpType.includes('Query') && !p.csharpType.includes('Transposed'),
    );
    if (queryDto) {
      const entityDto = queryDto.csharpType.replace(/Query$/, '');
      return `TaktPagedResult<${entityDto}>`;
    }
    return 'TaktPagedResult<unknown>';
  }
  const byIdMatch = method.exportName.match(/^get(\w+)ById$/);
  if (byIdMatch || returnsText.includes('详情')) {
    if (byIdMatch) {
      return dtoClassToFrontendTypeName(`Takt${byIdMatch[1]}Dto`);
    }
    const queryDto = method.params.find((p) => p.csharpType.includes('Query'));
    if (queryDto) {
      return queryDto.csharpType.replace(/Query$/, '');
    }
  }
  const bodyDto = method.params.find(
    (p) =>
      (p.csharpType.startsWith('Takt') && p.csharpType.endsWith('Dto')) ||
      /^[A-Z][a-zA-Z0-9]+$/.test(p.csharpType),
  );
  if (bodyDto && (returnsText.includes('DTO') || returnsText.includes('用户') || returnsText.includes('列表'))) {
    return bodyDto.csharpType.startsWith('Takt')
      ? dtoClassToFrontendTypeName(bodyDto.csharpType)
      : bodyDto.csharpType;
  }
  if (returnsText.includes('总数') || method.exportName.endsWith('Count')) {
    return 'number';
  }
  if (returnsText.includes('是否成功')) {
    return 'boolean';
  }
  if (returnsText.includes('任务')) {
    return 'void';
  }

  const optionsMatch = method.exportName.match(/^get(\w+)Options$/);
  if (optionsMatch) {
    return 'TaktSelectOption[]';
  }

  if (returnsText.includes('列表') && method.exportName.startsWith('get')) {
    const entityMatch = method.exportName.match(/^getUser(\w+)Ids$/);
    if (entityMatch) {
      return `${dtoClassToFrontendTypeName(`TaktUser${entityMatch[1]}Dto`)}[]`;
    }
  }

  return 'unknown';
}

/**
 * 生成API方法
 */
function generateApiMethod(method, dtoTypes, apiBaseVar, aliasEntityFile = false) {
  const lines = [];
  const summaryLines = formatSummaryLines(method.summary);
  const returnType = mapEntityFileTsType(inferReturnTsType(method, dtoTypes), aliasEntityFile);

  lines.push('/**');
  if (summaryLines.length > 0) {
    summaryLines.forEach((line) => lines.push(` * ${line}`));
  }
  method.params.forEach((param) => {
    const tsType = csharpParamToTsType(param.csharpType);
    const desc = param.description || toFrontendParamName(param);
    const name = toFrontendParamName(param);
    lines.push(` * @param {${tsType}} ${name} ${desc}`);
  });
  if (method.returns) {
    lines.push(` * @returns {Promise<${returnType}>} ${method.returns}`);
  }
  lines.push(' */');

  if (method.params.length === 0) {
    lines.push(`export function ${method.exportName}(): Promise<${returnType}> {`);
  } else if (method.params.length <= 2) {
    lines.push(
      `export function ${method.exportName}(${buildTsParamSignature(method.params)}): Promise<${returnType}> {`
    );
  } else {
    lines.push(`export function ${method.exportName}(`);
    lines.push(`  ${buildTsParamSignature(method.params)}`);
    lines.push(`): Promise<${returnType}> {`);
  }

  const httpLines = buildHttpCallLines(method, apiBaseVar);
  if (returnType !== 'void' && returnType !== 'unknown') {
    httpLines[0] = httpLines[0].replace('return request({', `return request<${returnType}>({`);
  }
  httpLines.forEach((line) => {
    lines.push(`  ${line}`);
  });

  lines.push('}');
  lines.push('');

  return lines.join('\n');
}

/**
 * 生成完整的API文件
 */
function generateApiFile(methods, moduleName, fileName, typesImportPath, meta, controllerName) {
  const lines = [];
  const dtoTypes = collectDtoTypes(methods);
  const apiBaseVar = getApiBaseConstantName(controllerName);
  const aliasEntityFile = apiMethodsUseDomFile(methods) && apiMethodsUseEntityFileType(methods, dtoTypes);

  lines.push('// ========================================');
  lines.push('// 项目名称：节拍智控·TaktWise');
  lines.push(`// 命名空间：frontend/src/api/${moduleName}`);
  lines.push(`// 文件名称：${toFrontendApiFileName(fileName)}`);
  lines.push(`// 创建时间：${new Date().toISOString().split('T')[0]}`);
  lines.push('// 创建人：Takt365(Auto Generated)');
  lines.push(`// 功能描述：${moduleName} 模块 API（自动生成，请勿手改路由常量）`);
  lines.push('// ');
  lines.push('// 版权信息：Copyright (c) 2025 Takt  All rights reserved.');
  lines.push('// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。');
  lines.push('// ========================================');
  lines.push('');
  lines.push("import request from '@/api/request';");

  const commonImports = new Set();
  const entityDtoTypes = new Set(dtoTypes);
  methods.forEach((m) => {
    const ret = inferReturnTsType(m, dtoTypes);
    collectTsTypesFromReturnType(ret).forEach((typeName) => {
      if (typeName === 'TaktPagedResult' || typeName === 'TaktSelectOption' || typeName === 'TaktTreeSelectOption') {
        commonImports.add(typeName);
      } else {
        entityDtoTypes.add(typeName);
      }
    });
  });

  formatTypeImportLines([...commonImports].sort(), '@/types/common.types').forEach((l) => lines.push(l));
  if (entityDtoTypes.size > 0) {
    buildGroupedTypeImports(entityDtoTypes, moduleName, typesImportPath, aliasEntityFile).forEach((l) => lines.push(l));
  }
  lines.push('');

  lines.push('/**');
  lines.push(` * API 路径前缀（相对 request baseURL，对应后端 [controller]）`);
  lines.push(` * @description ${meta.apiPath}`);
  lines.push(' */');
  lines.push(`const ${apiBaseVar} = '${meta.apiPath}';`);
  lines.push('');

  const sectionOrder = ['基础 CRUD', '选项', '密码与解锁', '关联分配', '统计', '导入导出'];
  const sortedMethods = [...methods].sort(
    (a, b) =>
      sectionOrder.indexOf(getMethodSectionLabel(a)) -
      sectionOrder.indexOf(getMethodSectionLabel(b))
  );

  let lastSection = '';
  sortedMethods.forEach((method) => {
    const section = getMethodSectionLabel(method);
    if (section !== lastSection) {
      lines.push('// ========================================');
      lines.push(`// ${section}`);
      lines.push('// ========================================');
      lines.push('');
      lastSection = section;
    }
    lines.push(generateApiMethod(method, dtoTypes, apiBaseVar, aliasEntityFile));
  });

  let content = lines.join('\n');
  if (moduleName === 'foundation' && fileName === 'file') {
    const { mergeFoundationFileUploadApi } = require('./api-fragments/foundation-file-upload-api.cjs');
    content = mergeFoundationFileUploadApi(content);
  }
  return content;
}

// ========================================
// 主函数
// ========================================

/**
 * 扫描并处理所有DTO文件
 */
function processDtos(entityPrefix = null) {
  console.log('🔍 开始扫描后端DTO文件...');

  DTO_TYPE_INDEX.clear();

  const dtosPath = path.join(CONFIG.backendRoot, 'Takt.Application', 'Dtos');
  /** @type {Array<{ moduleName: string, sourceFileBase: string, dtos: object[] }>} */
  const results = [];

  // 递归扫描DTO目录（每个 *Dtos.cs 单独生成一个 .types.ts，禁止按模块目录合并）
  function scanDir(dir, parentModule = '') {
    const files = fs.readdirSync(dir);

    files.forEach((file) => {
      const fullPath = path.join(dir, file);
      const stat = fs.statSync(fullPath);

      if (stat.isDirectory()) {
        const frontendDirName = backendDirToFrontendDir(file);
        const moduleName = parentModule ? `${parentModule}/${frontendDirName}` : frontendDirName;
        scanDir(fullPath, moduleName);
      } else if (file.endsWith('.cs') && file.includes('Dto')) {
        const sourceFileBase = file.replace('.cs', '');

        // 单实体模式：先按前缀过滤，避免把全库手工模块误打成「跳过」日志
        if (entityPrefix) {
          const expectedFileName = `Takt${entityPrefix}Dtos`;
          if (sourceFileBase !== expectedFileName) {
            return;
          }
        }

        if (shouldExcludeDtoSourceBase(sourceFileBase)) {
          console.log(`⏭️  跳过手工维护 DTO（前端）: ${file}`);
          return;
        }

        const dtos = parseDtoFile(fullPath);
        if (dtos.length === 0) {
          console.warn(`⚠️  跳过（未解析到 DTO 类）: ${fullPath}`);
          return;
        }

        const moduleName =
          parentModule || dtosSourceFileToOutputName(sourceFileBase);

        results.push({
          moduleName,
          sourceFileBase,
          dtos,
        });
      }
    });
  }

  scanDir(dtosPath);

  let created = 0;
  let updated = 0;

  // 第一遍：建立全局 DTO 索引（供主子表/转置跨模块 import）
  results.forEach(({ moduleName, sourceFileBase, dtos }) => {
    const rawTypeFileName = dtosSourceFileToOutputName(sourceFileBase);
    const outputRel = resolveFrontendOutputRelPath(moduleName, rawTypeFileName);
    dtos.forEach((dto) => {
      const entry = {
        moduleName,
        typeFile: outputRel.file,
        importPath: outputRel.importPath,
        frontendName: dto.frontendName,
        backendName: dto.name,
      };
      DTO_TYPE_INDEX.set(dto.name, entry);
      DTO_TYPE_INDEX.set(dto.frontendName, entry);
    });
  });

  results.forEach(({ moduleName, sourceFileBase, dtos }) => {
    const rawTypeFileName = dtosSourceFileToOutputName(sourceFileBase);
    const outputRel = resolveFrontendOutputRelPath(moduleName, rawTypeFileName);
    const outputPath = path.join(CONFIG.frontendRoot, CONFIG.output.types, outputRel.relDir);

    const content = generateTypesFile(dtos, moduleName, outputRel.file);
    const filePath = path.join(outputPath, toFrontendTypesFileName(outputRel.file));
    const writeResult = writeGeneratedFile(filePath, content);
    if (writeResult.created) {
      created += 1;
    } else if (writeResult.updated) {
      updated += 1;
    }
    const actionLabel = writeResult.created ? '已创建' : '已更新';

    console.log(`✅ ${actionLabel}: ${filePath}（${dtos.length} 个类型，来源 ${sourceFileBase}.cs）`);
  });

  return { results, created, updated };
}

/**
 * 扫描并处理所有控制器文件
 */
function processControllers(entityPrefix = null) {
  console.log('🔍 开始扫描后端控制器文件...');
  
  const controllersPath = path.join(CONFIG.backendRoot, 'Takt.WebApi', 'Controllers');
  const results = {};
  
  // 递归扫描控制器目录
  function scanDir(dir, parentModule = '') {
    const files = fs.readdirSync(dir);
    
    files.forEach(file => {
      const fullPath = path.join(dir, file);
      const stat = fs.statSync(fullPath);
      
      if (stat.isDirectory()) {
        // 子目录作为模块名，转换为前端命名规范（kebab-case）
        const frontendDirName = backendDirToFrontendDir(file);
        const moduleName = parentModule ? `${parentModule}/${frontendDirName}` : frontendDirName;
        scanDir(fullPath, moduleName);
      } else if (file.endsWith('Controller.cs')) {
        const controllerName = file.replace('.cs', ''); // 例：TaktUsersController

        // 单实体模式：先按前缀过滤，避免把全库手工模块（Holiday/Auth/…）误打成「跳过」日志
        if (entityPrefix && !matchControllerForEntityPrefix(controllerName, entityPrefix)) {
          return;
        }

        if (shouldExcludeController(controllerName)) {
          console.log(`⏭️  跳过手工维护控制器（前端）: ${file}`);
          return;
        }
        
        const parsed = parseControllerFile(fullPath, controllerName);
        if (parsed.methods.length > 0) {
          const moduleName = parentModule || backendDirToFrontendDir(controllerName.replace('Takt', '').replace('Controller', ''));
          results[controllerName] = { ...parsed, moduleName, controllerName };
          if (parsed.serviceInterfacePath) {
            console.log(`   ↳ 返回类型来源: ${path.basename(parsed.serviceInterfacePath)}`);
          }
        }
      }
    });
  }
  
  scanDir(controllersPath);

  let created = 0;
  let updated = 0;

  // 生成API文件
  Object.entries(results).forEach(([controllerName, { methods, moduleName, meta }]) => {
    const rawFileName = controllerClassToFileName(controllerName);
    const outputRel = resolveFrontendOutputRelPath(moduleName, rawFileName);
    const outputPath = path.join(CONFIG.frontendRoot, CONFIG.output.api, outputRel.relDir);

    const typesImportPath = getTypesImportPath(moduleName, controllerName, entityPrefix);
    const content = generateApiFile(methods, moduleName, outputRel.file, typesImportPath, meta, controllerName);
    const filePath = path.join(outputPath, toFrontendApiFileName(outputRel.file));
    const writeResult = writeGeneratedFile(filePath, content);
    if (writeResult.created) {
      created += 1;
    } else if (writeResult.updated) {
      updated += 1;
    }
    const actionLabel = writeResult.created ? '已创建' : '已更新';

    console.log(`✅ ${actionLabel}: ${filePath}`);
  });

  return { controllers: results, created, updated };
}

// ========================================
// 执行
// ========================================

/**
 * 打印使用说明
 */
function printUsage() {
  console.log(`
用法: node scripts/generate-from-backend.cjs [参数]

参数:
  --<实体名前缀>     生成指定实体（如: --User, --Calendar, --CostCenter）
                     匹配: Takt{实体}Dtos.cs、Takt{实体复数}Controller.cs（如 TaktCalendarsController）

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 跳过 types：Holiday（TaktHolidayThemeDto 手工 DTO，须手工维护 holiday.types.ts）

输出策略:
  - 目标 .types.ts / .api.ts 不存在则创建，已存在则覆盖更新（与 generate-script-common.cjs 一致）

示例:
  node scripts/generate-from-backend.cjs --CostCenter
  node scripts/generate-from-backend.cjs --Menu
  node scripts/generate-from-backend.cjs --User
  node scripts/generate-from-backend.cjs --Dept
  node scripts/generate-from-backend.cjs --Employee

生成范围:
  - 类型：*Dtos.cs 全部类（含主子表导航、转置 DTO）；前端类型名去 Takt 与末尾 Dto（TaktCompanyDto → Company）
  - 跳过前端：Online、Message（SignalR 手工维护）
  - 跳过独立模块 DTO：已合并至对应 Takt*Dtos.cs 的附属 DTO
  - API：控制器全部 Action；标准 list 用 XxxQueryDto，转置用 XxxTransposedQueryDto
  - 主子表字段：List<Takt子Dto> → 子Dto[]（跨模块自动 import）

  `
  );
}

/**
 * 解析命令行参数（单实体，禁止 --all）
 */
function parseArgs() {
  const options = parseSingleEntityGenerateArgs(printUsage);
  assertNotManualDtoEntityCli(options.entityPrefix);
  return {
    command: options.entityPrefix,
    generateTypes: true,
    generateApi: true,
    entityPrefix: options.entityPrefix,
    force: options.force,
    dryRun: options.dryRun,
  };
}

console.log('🚀 开始从后端生成前端代码...\n');
logGeneratedFileWritePolicy();

try {
  const options = parseArgs();
  
  console.log(`📦 实体前缀: ${options.entityPrefix}`);
  console.log(
    `   将匹配: Takt${options.entityPrefix}Dtos、Takt${pluralizeEntityShort(options.entityPrefix)}Controller\n`,
  );

  let typesCreated = 0;
  let typesUpdated = 0;
  let apiCreated = 0;
  let apiUpdated = 0;

  // 生成类型定义
  if (options.generateTypes) {
    const dtoResult = processDtos(options.entityPrefix);
    typesCreated = dtoResult.created;
    typesUpdated = dtoResult.updated;
    console.log(
      `\n📊 类型：已创建 ${typesCreated} 个，已更新 ${typesUpdated} 个（${dtoResult.results.length} 个文件，${DTO_TYPE_INDEX.size} 个 DTO 类）\n`,
    );
  }

  // 生成API接口
  if (options.generateApi) {
    const apiResult = processControllers(options.entityPrefix);
    apiCreated = apiResult.created;
    apiUpdated = apiResult.updated;
    const controllerCount = Object.keys(apiResult.controllers).length;
    console.log(
      `\n📊 API：已创建 ${apiCreated} 个，已更新 ${apiUpdated} 个（扫描到 ${controllerCount} 个控制器）\n`,
    );
  }

  console.log(
    `✨ 生成完成！合计：类型 ${typesCreated + typesUpdated} 个文件，API ${apiCreated + apiUpdated} 个文件。`,
  );
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}

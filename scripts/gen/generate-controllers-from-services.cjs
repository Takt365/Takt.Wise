// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-controllers-from-services.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：根据 Takt.Application/Services/**/ITaktXxxService.cs 自动生成 WebApi 控制器
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

const fs = require('fs');
const path = require('path');
const { writeGeneratedFile, getControllerClassName, logGeneratedFileWritePolicy, parseSingleEntityGenerateArgs } = require('./generate-script-common.cjs');
const {
  isRbacJunctionEntity,
  assertNotRbacJunctionEntityCli,
  assertNotManualDtoEntityCli,
  shouldExcludeStandaloneService,
  RBAC_ASSOCIATION_ENTITY_SHORT_NAMES,
} = require('./generate-entity-exclusions.cjs');
const { buildPermissionBase, buildControllerPermissionBase } = require('./permission-base.cjs');
const { transposedClassNames } = require('./generate-transposed-support.cjs');
const { isSharedEnumType } = require('./generate-enum-common.cjs');

// ========================================
// 配置
// ========================================

const CONFIG = {
  backendRoot: path.resolve(__dirname, '../../backend/src'),
  servicesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Services'),
  controllersRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.WebApi', 'Controllers'),
  entitiesRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Domain', 'Entities'),
  dtosRoot: path.join(path.resolve(__dirname, '../../backend/src'), 'Takt.Application', 'Dtos'),
};

/** 一级模块 → ApiModule int 与默认显示名（TaktModule 字典码，与 TaktTranslation.ResourceGroup 一致） */
const TOP_LEVEL_MODULE = {
  Identity: { moduleInt: 1, display: '身份认证' },
  HumanResource: { moduleInt: 5, display: '人力资源' },
  Statistics: { moduleInt: 9, display: '统计看板' },
  Routine: { moduleInt: 2, display: '日常事务' },
  Accounting: { moduleInt: 3, display: '财务核算' },
  Logistics: { moduleInt: 4, display: '后勤管理' },
  Foundation: { moduleInt: 8, display: '基础设置' },
  Workflow: { moduleInt: 6, display: '工作流' },
  Code: { moduleInt: 7, display: '代码管理' },
};

/** 二级目录 → ApiModule 子模块显示名（覆盖一级默认名） */
const SUBMODULE_DISPLAY = {
  Attendance: '考勤管理',
  Logging: '统计日志',
  Organization: '组织管理',
  Personnel: '人事管理',
  Controlling: '管控会计',
};

/** @deprecated 使用 generate-entity-exclusions 的 MANUAL_STANDALONE_SERVICE_ENTITY_NAMES */
const EXISTING_MANUAL_SERVICE_ENTITIES = new Set(['TaktAuth', 'TaktRbac', 'TaktFlowEngine']);

/**
 * 登录/注册等未鉴权场景需公开访问的 Options 接口（生成 [AllowAnonymous]，不使用 TaktPermission）
 */
const ANONYMOUS_OPTIONS_METHOD_NAMES = new Set([
  'GetTenantOptionsAsync',
  'GetCultureOptionsAsync',
  'GetCompanyOptionsAsync',
]);

/**
 * 是否为需匿名访问的 Options 方法
 * @param {string} methodName 服务方法名
 * @returns {boolean}
 */
function isAnonymousOptionsMethod(methodName) {
  return ANONYMOUS_OPTIONS_METHOD_NAMES.has(methodName);
}

function isInEngineDirectory(filePath) {
  const normalizedPath = filePath.replace(/\\/g, '/');
  return /\/\w*[Ee]ngine($|\/)/i.test(normalizedPath);
}

function readUtf8(filePath) {
  return fs.readFileSync(filePath, 'utf-8');
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
 * 解析 StatusDto 中除 Id 外的状态值属性（用于 XML 注释标注 Shared 枚举）
 * @param {string} dtoContent
 * @param {string} statusDtoName
 */
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

function findDtoFileByEntity(entityName) {
  const fileName = `${entityName}Dtos.cs`;
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
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

function todayFileHeaderDate() {
  return new Date().toISOString().split('T')[0];
}

function pascalToCamel(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

function buildNamespace(prefix, parts) {
  if (!parts.length) {
    return prefix;
  }
  return `${prefix}.${parts.join('.')}`;
}

function entityNameFromInterfaceFile(interfaceFile) {
  const base = path.basename(interfaceFile, '.cs');
  const match = base.match(/^I(Takt\w+)Service$/);
  return match ? match[1] : null;
}

function shouldExcludeInterface(interfaceFile, entityName) {
  if (!entityName) {
    return true;
  }
  const entityShort = entityName.replace(/^Takt/, '');
  if (isRbacJunctionEntity(entityShort)) {
    return true;
  }
  return false;
}

/**
 * 工作流运行时实体 CRUD 权限（与菜单种子、TaktFlowEngineController 对齐）
 * @param {string} entityShort
 * @param {string} action list|query|create|update|delete|import|export
 * @returns {string|null}
 */
function resolveWorkflowRuntimePermission(entityShort, action) {
  if (entityShort === 'FlowAddSign') {
    const map = {
      list: 'workflow:todo:list',
      query: 'workflow:todo:query',
      create: 'workflow:todo:addsign',
      update: 'workflow:todo:addsign',
      delete: 'workflow:todo:reducesign',
      import: 'workflow:todo:import',
      export: 'workflow:todo:export',
    };
    return map[action] ?? null;
  }
  if (entityShort === 'FlowTask') {
    const map = {
      list: 'workflow:todo:list',
      query: 'workflow:todo:query',
      create: 'workflow:todo:create',
      update: 'workflow:todo:update',
      delete: 'workflow:todo:delete',
      approve: 'workflow:todo:approve',
      import: 'workflow:todo:import',
      export: 'workflow:todo:export',
    };
    return map[action] ?? null;
  }
  if (entityShort === 'FlowTransition' || entityShort === 'FlowVariable') {
    const map = {
      list: 'workflow:instance:list',
      query: 'workflow:instance:query',
      create: 'workflow:instance:create',
      update: 'workflow:instance:update',
      delete: 'workflow:instance:delete',
      import: 'workflow:instance:import',
      export: 'workflow:instance:export',
    };
    return map[action] ?? null;
  }
  return null;
}

/**
 * @param {{ entityShort: string, permissionBase: string }} ctx
 * @param {string} action
 * @returns {string}
 */
function permissionCode(ctx, action) {
  return resolveWorkflowRuntimePermission(ctx.entityShort, action)
    ?? `${ctx.permissionBase}:${action}`;
}

function getApiModuleMeta(pathParts) {
  const top = pathParts[0];
  const topMeta = TOP_LEVEL_MODULE[top];
  if (!topMeta) {
    return { moduleInt: 8, display: top || '基础设置' };
  }
  if (pathParts.length <= 1) {
    return { moduleInt: topMeta.moduleInt, display: topMeta.display };
  }
  const sub = pathParts[pathParts.length - 1];
  const subDisplay = SUBMODULE_DISPLAY[sub];
  return { moduleInt: topMeta.moduleInt, display: subDisplay || topMeta.display };
}

function extractInterfaceDescription(content) {
  const match = content.match(/\/\/\/\s*<summary>\s*\r?\n\s*\/\/\/\s*(.+?)\s*\r?\n/s);
  if (!match) {
    return null;
  }
  return match[1].replace(/应用服务接口$/, '').trim();
}

/**
 * 按实体类名查找 Domain 实体文件（与 generate-services-from-dtos 一致）
 * @param {string} entityName 如 TaktHoliday
 * @returns {string|null}
 */
function findEntityFile(entityName) {
  function searchDir(dir) {
    if (!fs.existsSync(dir)) {
      return null;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        const found = searchDir(fullPath);
        if (found) {
          return found;
        }
        continue;
      }
      if (entry.name === `${entityName}.cs`) {
        return fullPath;
      }
    }
    return null;
  }
  return searchDir(CONFIG.entitiesRoot);
}

/**
 * 从实体 SugarTable 第二参数读取表描述，去掉末尾「表」（如「用户表」→「用户」）
 * @param {string|null} entityFile
 * @returns {string|null}
 */
function extractEntityTableDescription(entityFile) {
  if (!entityFile) {
    return null;
  }
  const content = readUtf8(entityFile);
  const sugarTableMatch = content.match(/\[SugarTable\([^,]*,\s*"([^"]+)"/);
  if (sugarTableMatch) {
    return sugarTableMatch[1].replace(/表$/, '').trim();
  }
  const xmlMatch = content.match(/\/\/\/\s*<summary>\s*\r?\n\s*\/\/\/\s*(.+?)\s*\r?\n\s*\/\/\/\s*<\/summary>/s);
  if (xmlMatch) {
    return xmlMatch[1].trim().replace(/实体$/, '');
  }
  return null;
}

/**
 * 控制器 Route Name（优先 SugarTable 描述）
 */
function resolveRouteDisplayName(entityName, interfaceContent) {
  const entityFile = findEntityFile(entityName);
  const fromEntity = extractEntityTableDescription(entityFile);
  if (fromEntity) {
    return fromEntity;
  }
  const fromInterface = extractInterfaceDescription(interfaceContent);
  if (fromInterface) {
    return fromInterface;
  }
  return entityName.replace(/^Takt/, '');
}

/** C# 字符串字面量转义 */
function escapeCSharpString(value) {
  return value.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
}

/**
 * 解析接口方法（含 XML 摘要）
 */
function parseInterfaceMethods(content) {
  const methods = [];
  const seen = new Set();
  const methodRegex = /Task(?:<([\s\S]*?)>)?\s+(\w+Async)\s*\(([^)]*)\)\s*;/g;
  let match;

  while ((match = methodRegex.exec(content)) !== null) {
    const methodName = match[2];
    if (seen.has(methodName)) {
      continue;
    }
    seen.add(methodName);

    const before = content.slice(0, match.index);
    const summaryBlocks = [...before.matchAll(/\/\/\/\s*<summary>\s*\r?\n\s*\/\/\/\s*(.+?)\s*\r?\n/g)];
    const summary = summaryBlocks.length
      ? summaryBlocks[summaryBlocks.length - 1][1].trim()
      : '';

    methods.push({
      order: methods.length,
      returnType: (match[1] || '').trim(),
      methodName,
      parameters: match[3].trim(),
      summary,
    });
  }

  return methods;
}

/**
 * 解析参数字符串为控制器参数列表
 */
function parseMethodParameters(paramString) {
  if (!paramString) {
    return [];
  }
  const parts = [];
  let depth = 0;
  let current = '';
  for (const ch of paramString) {
    if (ch === '<') {
      depth += 1;
    } else if (ch === '>') {
      depth -= 1;
    } else if (ch === ',' && depth === 0) {
      parts.push(current.trim());
      current = '';
      continue;
    }
    current += ch;
  }
  if (current.trim()) {
    parts.push(current.trim());
  }

  return parts.map((part) => {
    const nullable = part.includes('?');
    const defaultMatch = part.match(/=\s*([^,]+)$/);
    const defaultValue = defaultMatch ? defaultMatch[1].trim() : null;
    const decl = defaultMatch ? part.slice(0, part.indexOf('=')).trim() : part.trim();
    const tokens = decl.split(/\s+/);
    const type = tokens.slice(0, -1).join(' ');
    const name = tokens[tokens.length - 1].replace('?', '');
    return { type, name, nullable, defaultValue };
  });
}

function formatControllerParameters(params, binding) {
  return params
    .map((p) => {
      const typeWithNullable = p.nullable && !p.type.endsWith('?') ? `${p.type}?` : p.type;
      const defaultSuffix = p.defaultValue != null ? ` = ${p.defaultValue}` : '';
      if (binding === 'route' && p.type === 'long') {
        return `${typeWithNullable} ${p.name}${defaultSuffix}`;
      }
      if (binding === 'body') {
        return `[FromBody] ${typeWithNullable} ${p.name}${defaultSuffix}`;
      }
      if (binding === 'query') {
        return `[FromQuery] ${typeWithNullable} ${p.name}${defaultSuffix}`;
      }
      return `${typeWithNullable} ${p.name}${defaultSuffix}`;
    })
    .join(', ');
}

/**
 * 服务参数名 → 控制器查询参数名（避免与返回值 fileName 解构冲突，对齐 TaktUsersController）
 * @param {{ name: string, type: string, nullable?: boolean, defaultValue?: string }} param
 * @param {'template'|'export'} context
 */
function mapControllerQueryParam(param, context) {
  if (param.name !== 'fileName') {
    return param;
  }
  const alias = context === 'template' ? 'templateName' : 'exportName';
  return { ...param, name: alias };
}

function generateListEndpoint(ctx) {
  const queryParam = ctx.params.find((p) => p.type.endsWith('QueryDto'));
  if (!queryParam) {
    return { skipped: true, reason: '缺少 QueryDto 参数' };
  }
  const perm = permissionCode(ctx, 'list');
  const display = `${ctx.displayName}列表`;
  const code = `    /// <summary>
    /// ${ctx.summary || `获取${ctx.displayName}列表（分页）`}
    /// </summary>
    /// <param name="${queryParam.name}">查询DTO</param>
    /// <returns>分页结果</returns>
    [TaktPermission("${perm}", "${display}")]
    [HttpGet("list")]
    public async Task<IActionResult> ${ctx.methodName}([FromQuery] ${queryParam.type} ${queryParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${queryParam.name});
            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateGetByIdEndpoint(ctx) {
  const perm = permissionCode(ctx, 'query');
  const code = `    /// <summary>
    /// ${ctx.summary || `根据ID获取${ctx.displayName}`}
    /// </summary>
    /// <param name="id">${ctx.displayName}ID</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "${ctx.displayName}详情")]
    [HttpGet("{id:long}")]
    public async Task<IActionResult> ${ctx.methodName}(long id)
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(id);
            if (result == null)
            {
                return NotFound("${ctx.displayName}不存在");
            }
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateOptionsEndpoint(ctx) {
  const perm = permissionCode(ctx, 'query');
  const permissionAttr = isAnonymousOptionsMethod(ctx.methodName)
    ? '    [AllowAnonymous]'
    : `    [TaktPermission("${perm}", "${ctx.displayName}选项")]`;
  const paramDecl = formatControllerParameters(ctx.params, 'query');
  const callArgs = ctx.params.map((p) => p.name).join(', ');
  const plantParam = ctx.params.find((p) => p.name === 'plantCode');
  const keywordParam = ctx.params.find((p) => p.name === 'keyword');
  const plantDoc = plantParam
    ? `\n    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>`
    : '';
  const keywordDoc = keywordParam
    ? `\n    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>`
    : '';
  const extraParamDocs = ctx.params
    .filter((p) => p.name !== 'plantCode' && p.name !== 'keyword')
    .map((p) => `\n    /// <param name="${p.name}">${p.name}</param>`)
    .join('');
  const code = `    /// <summary>
    /// ${ctx.summary || `获取${ctx.displayName}选项列表`}
    /// </summary>${plantDoc}${keywordDoc}${extraParamDocs}
    /// <returns>下拉选项</returns>
${permissionAttr}
    [HttpGet("options")]
    public async Task<IActionResult> ${ctx.methodName}(${paramDecl})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateTreeOptionsEndpoint(ctx) {
  const paramDecl = formatControllerParameters(ctx.params, 'query');
  const callArgs = ctx.params.map((p) => p.name).join(', ');
  const perm = permissionCode(ctx, 'query');
  const parentIdParam = ctx.params.find((p) => p.name === 'parentId');
  const plantParam = ctx.params.find((p) => p.name === 'plantCode');
  const keywordParam = ctx.params.find((p) => p.name === 'keyword');
  const parentIdDoc = parentIdParam
    ? `\n    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>`
    : '';
  const plantDoc = plantParam
    ? `\n    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>`
    : '';
  const keywordDoc = keywordParam
    ? `\n    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>`
    : '';
  const code = `    /// <summary>
    /// ${ctx.summary || `获取${ctx.displayName}树形选项列表（懒加载一层）`}
    /// </summary>${parentIdDoc}${plantDoc}${keywordDoc}
    /// <returns>树形选项</returns>
    [TaktPermission("${perm}", "${ctx.displayName}树形选项")]
    [HttpGet("tree-options")]
    public async Task<IActionResult> ${ctx.methodName}(${paramDecl})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateTreeEndpoint(ctx) {
  const paramDecl = formatControllerParameters(ctx.params, 'query');
  const callArgs = ctx.params.map((p) => p.name).join(', ');
  const perm = permissionCode(ctx, 'query');
  const includeDisabledParam = ctx.params.find((p) => p.name === 'includeDisabled');
  const parentIdParam = ctx.params.find((p) => p.name === 'parentId');
  const includeDisabledDoc = includeDisabledParam
    ? `\n    /// <param name="includeDisabled">为 false 时过滤禁用项（按实体 *Status 字段）</param>`
    : '';
  const parentIdDoc = parentIdParam
    ? `\n    /// <param name="parentId">父级ID（0=根；懒加载仅返回直接子级一层）</param>`
    : '';
  const code = `    /// <summary>
    /// ${ctx.summary || `获取${ctx.displayName}树（懒加载一层）`}
    /// </summary>${parentIdDoc}${includeDisabledDoc}
    /// <returns>树形数据</returns>
    [TaktPermission("${perm}", "${ctx.displayName}树")]
    [HttpGet("tree")]
    public async Task<IActionResult> ${ctx.methodName}(${paramDecl})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateCreateEndpoint(ctx) {
  const dtoParam = ctx.params.find((p) => p.type.includes('Create') || p.name === 'dto');
  if (!dtoParam) {
    return { skipped: true, reason: '缺少创建 DTO 参数' };
  }
  const perm = permissionCode(ctx, 'create');
  const code = `    /// <summary>
    /// ${ctx.summary || `创建${ctx.displayName}`}
    /// </summary>
    /// <param name="${dtoParam.name}">创建DTO</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "创建${ctx.displayName}")]
    [HttpPost]
    public async Task<IActionResult> ${ctx.methodName}([FromBody] ${dtoParam.type} ${dtoParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${dtoParam.name});
            return Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateUpdateEndpoint(ctx) {
  const dtoParam = ctx.params.find((p) => p.type.includes('Update') || p.name === 'dto');
  if (!dtoParam) {
    return { skipped: true, reason: '缺少更新 DTO 参数' };
  }
  const perm = permissionCode(ctx, 'update');
  const code = `    /// <summary>
    /// ${ctx.summary || `更新${ctx.displayName}`}
    /// </summary>
    /// <param name="id">${ctx.displayName}ID</param>
    /// <param name="${dtoParam.name}">更新DTO</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "更新${ctx.displayName}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> ${ctx.methodName}(long id, [FromBody] ${dtoParam.type} ${dtoParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(id, ${dtoParam.name});
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateUpdateStatusEndpoint(ctx) {
  const dtoParam = ctx.params[0];
  if (!dtoParam) {
    return { skipped: true, reason: '缺少状态 DTO' };
  }
  const suffixMatch = ctx.methodName.match(
    new RegExp(`^Update${ctx.entityShort}(\\w*)StatusAsync$`),
  );
  const statusSuffix = suffixMatch && suffixMatch[1] ? suffixMatch[1] : '';
  const routeSuffix = statusSuffix ? `-${statusSuffix.charAt(0).toLowerCase()}${statusSuffix.slice(1)}` : '';
  const statusLabel = statusSuffix || '状态';
  const perm = resolveWorkflowRuntimePermission(ctx.entityShort, 'approve')
    ?? permissionCode(ctx, 'update');
  const statusValueProp = ctx.statusValueProperty;
  const enumHint =
    statusValueProp && isSharedEnumType(statusValueProp.type)
      ? `（${statusValueProp.type} 枚举）`
      : '';
  const code = `    /// <summary>
    /// ${ctx.summary || `更新${ctx.displayName}${statusLabel}`}
    /// </summary>
    /// <param name="${dtoParam.name}">状态 DTO${enumHint}</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "更新${ctx.displayName}${statusLabel}")]
    [HttpPut("status${routeSuffix}")]
    public async Task<IActionResult> ${ctx.methodName}([FromBody] ${dtoParam.type} ${dtoParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${dtoParam.name});
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateUpdateSortEndpoint(ctx) {
  const dtoParam = ctx.params[0];
  if (!dtoParam) {
    return { skipped: true, reason: '缺少排序 DTO' };
  }
  const perm = permissionCode(ctx, 'update');
  const code = `    /// <summary>
    /// ${ctx.summary || `更新${ctx.displayName}排序`}
    /// </summary>
    /// <param name="${dtoParam.name}">排序DTO</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "更新${ctx.displayName}排序")]
    [HttpPut("sort")]
    public async Task<IActionResult> ${ctx.methodName}([FromBody] ${dtoParam.type} ${dtoParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${dtoParam.name});
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateUpdateObsoleteEndpoint(ctx) {
  const dtoParam = ctx.params[0];
  if (!dtoParam) {
    return { skipped: true, reason: '缺少作废 DTO' };
  }
  const perm = permissionCode(ctx, 'update');
  const code = `    /// <summary>
    /// ${ctx.summary || `更新${ctx.displayName}作废状态`}
    /// </summary>
    /// <param name="${dtoParam.name}">作废 DTO</param>
    /// <returns>${ctx.displayName}DTO</returns>
    [TaktPermission("${perm}", "更新${ctx.displayName}作废状态")]
    [HttpPut("obsolete")]
    public async Task<IActionResult> ${ctx.methodName}([FromBody] ${dtoParam.type} ${dtoParam.name})
    {
        try
        {
            var result = await ${ctx.serviceField}.${ctx.methodName}(${dtoParam.name});
            return Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateDeleteByIdEndpoint(ctx) {
  const perm = permissionCode(ctx, 'delete');
  const code = `    /// <summary>
    /// ${ctx.summary || `删除${ctx.displayName}`}
    /// </summary>
    /// <param name="id">${ctx.displayName}ID</param>
    /// <returns>操作结果</returns>
    [TaktPermission("${perm}", "删除${ctx.displayName}")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> ${ctx.methodName}(long id)
    {
        try
        {
            await ${ctx.serviceField}.${ctx.methodName}(id);
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateDeleteBatchEndpoint(ctx) {
  const idsParam = ctx.params.find((p) => p.type.includes('IEnumerable') || p.type.includes('List'));
  const paramType = idsParam ? idsParam.type : 'IEnumerable<long>';
  const paramName = idsParam ? idsParam.name : 'ids';
  const perm = permissionCode(ctx, 'delete');
  const code = `    /// <summary>
    /// ${ctx.summary || `批量删除${ctx.displayName}`}
    /// </summary>
    /// <param name="${paramName}">ID列表</param>
    /// <returns>操作结果</returns>
    [TaktPermission("${perm}", "批量删除${ctx.displayName}")]
    [HttpDelete("batch")]
    public async Task<IActionResult> ${ctx.methodName}([FromBody] ${paramType} ${paramName})
    {
        try
        {
            await ${ctx.serviceField}.${ctx.methodName}(${paramName});
            return Success("删除成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateTemplateEndpoint(ctx) {
  const queryParams = ctx.params.filter((p) => p.type === 'string' || p.type === 'string?');
  const controllerParams = queryParams.map((p) => mapControllerQueryParam(p, 'template'));
  const paramDecl = formatControllerParameters(controllerParams, 'query');
  const callArgs = controllerParams.map((p) => p.name).join(', ');
  const perm = permissionCode(ctx, 'import');
  const code = `    /// <summary>
    /// ${ctx.summary || `获取${ctx.displayName}导入模板`}
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("${perm}", "获取${ctx.displayName}导入模板")]
    [HttpGet("template")]
    public async Task<IActionResult> ${ctx.methodName}(${paramDecl})
    {
        try
        {
            var (resultFileName, content) = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateImportEndpoint(ctx) {
  const sheetParam = ctx.params.find((p) => p.name === 'sheetName');
  const sheetQuery = sheetParam ? `[FromQuery] string? ${sheetParam.name} = null` : '';
  const perm = permissionCode(ctx, 'import');
  const callArgs = sheetParam ? `stream, ${sheetParam.name}` : 'stream';
  const code = `    /// <summary>
    /// ${ctx.summary || `导入${ctx.displayName}`}
    /// </summary>
    /// <param name="file">Excel文件</param>
    /// <returns>导入结果</returns>
    [TaktPermission("${perm}", "导入${ctx.displayName}")]
    [HttpPost("import")]
    public async Task<IActionResult> ${ctx.methodName}(IFormFile file${sheetQuery ? `, ${sheetQuery}` : ''})
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("请选择要导入的文件");
            }

            await using var stream = file.OpenReadStream();
            var (success, fail, errors) = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return Success(new
            {
                SuccessCount = success,
                FailCount = fail,
                Errors = errors
            }, $"导入完成：成功{success}条，失败{fail}条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateExportEndpoint(ctx) {
  const queryParams = ctx.params.filter((p) => p.type !== 'Stream');
  const controllerParams = queryParams.map((p) => mapControllerQueryParam(p, 'export'));
  const paramDecl = formatControllerParameters(controllerParams, 'query');
  const callArgs = controllerParams.map((p) => p.name).join(', ');
  const perm = permissionCode(ctx, 'export');
  const code = `    /// <summary>
    /// ${ctx.summary || `导出${ctx.displayName}`}
    /// </summary>
    /// <returns>Excel文件</returns>
    [TaktPermission("${perm}", "导出${ctx.displayName}")]
    [HttpGet("export")]
    public async Task<IActionResult> ${ctx.methodName}(${paramDecl})
    {
        try
        {
            var (resultFileName, fileContent) = await ${ctx.serviceField}.${ctx.methodName}(${callArgs});
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateEndpoint(method, ctx) {
  const { entityShort } = ctx;
  const methodName = method.methodName;
  const fullCtx = {
    ...ctx,
    params: parseMethodParameters(method.parameters),
    summary: method.summary,
    methodName,
  };

  if (methodName === `Get${entityShort}ListAsync`) {
    return generateListEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}ByIdAsync`) {
    return generateGetByIdEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}OptionsAsync`) {
    return generateOptionsEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}TreeOptionsAsync`) {
    return generateTreeOptionsEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}TreeAsync`) {
    return generateTreeEndpoint(fullCtx);
  }
  if (methodName === `Create${entityShort}Async`) {
    return generateCreateEndpoint(fullCtx);
  }
  if (methodName === `Update${entityShort}Async`) {
    return generateUpdateEndpoint(fullCtx);
  }
  if (new RegExp(`^Update${entityShort}\\w*StatusAsync$`).test(methodName)) {
    const statusDtoParam = fullCtx.params[0];
    let statusValueProperty = null;
    if (statusDtoParam && ctx.dtoFilePath && fs.existsSync(ctx.dtoFilePath)) {
      const statusFields = parseStatusDtoFields(readUtf8(ctx.dtoFilePath), statusDtoParam.type);
      statusValueProperty = statusFields.valueProperties[0] || null;
    }
    return generateUpdateStatusEndpoint({ ...fullCtx, statusValueProperty });
  }
  if (methodName === `Update${entityShort}SortAsync`) {
    return generateUpdateSortEndpoint(fullCtx);
  }
  if (methodName === `Update${entityShort}ObsoleteAsync`) {
    return generateUpdateObsoleteEndpoint(fullCtx);
  }
  if (methodName === `Delete${entityShort}ByIdAsync`) {
    return generateDeleteByIdEndpoint(fullCtx);
  }
  if (methodName === `Delete${entityShort}BatchAsync`) {
    return generateDeleteBatchEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}TemplateAsync`) {
    return generateTemplateEndpoint(fullCtx);
  }
  if (methodName === `Import${entityShort}Async`) {
    return generateImportEndpoint(fullCtx);
  }
  if (methodName === `Export${entityShort}Async`) {
    return generateExportEndpoint(fullCtx);
  }
  if (methodName === `Get${entityShort}TransposedListAsync`) {
    return generateTransposedListEndpoint(fullCtx);
  }
  if (methodName === `Save${entityShort}TransposedBatchAsync`) {
    return generateTransposedBatchEndpoint(fullCtx);
  }

  return { skipped: true, reason: '未识别的接口方法' };
}

function generateTransposedListEndpoint(ctx) {
  const names = transposedClassNames(ctx.entityShort);
  const perm = permissionCode(ctx, 'query');
  const code = `    /// <summary>
    /// 获取${ctx.displayName}转置列表（分页）
    /// </summary>
    [TaktPermission("${perm}", "查询${ctx.displayName}转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> Get${ctx.entityShort}TransposedListAsync([FromQuery] ${names.query} queryDto)
    {
        try
        {
            var result = await ${ctx.serviceField}.Get${ctx.entityShort}TransposedListAsync(queryDto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateTransposedBatchEndpoint(ctx) {
  const names = transposedClassNames(ctx.entityShort);
  const perm = `${ctx.permissionBase}:edit`;
  const code = `    /// <summary>
    /// 批量保存${ctx.displayName}转置数据
    /// </summary>
    [TaktPermission("${perm}", "保存${ctx.displayName}转置数据")]
    [HttpPost("transposed/batch")]
    public async Task<IActionResult> Save${ctx.entityShort}TransposedBatchAsync([FromBody] ${names.batch} dto)
    {
        try
        {
            var count = await ${ctx.serviceField}.Save${ctx.entityShort}TransposedBatchAsync(dto);
            return Success(count, $"已保存 {count} 条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;
  return { code };
}

function generateController(interfaceFile, entityName, methods, options) {
  const pathParts = path
    .relative(CONFIG.servicesRoot, path.dirname(interfaceFile))
    .split(path.sep)
    .filter(Boolean);
  const entityShort = entityName.replace(/^Takt/, '');
  const content = readUtf8(interfaceFile);
  const routeDisplayName = resolveRouteDisplayName(entityName, content);
  const displayName = routeDisplayName;
  const permissionBase = buildControllerPermissionBase(pathParts, entityShort);
  const apiModule = getApiModuleMeta(pathParts);
  const controllerClass = getControllerClassName(entityName);
  const serviceInterface = `I${entityName}Service`;
  const serviceField = `_${pascalToCamel(entityShort)}Service`;
  const serviceParam = `${pascalToCamel(entityShort)}Service`;
  const controllerNs = buildNamespace('Takt.WebApi.Controllers', pathParts);
  const dtoNs = buildNamespace('Takt.Application.Dtos', pathParts);
  const serviceNs = buildNamespace('Takt.Application.Services', pathParts);
  const outputFile = path.join(CONFIG.controllersRoot, ...pathParts, `${controllerClass}.cs`);
  const dtoFilePath = findDtoFileByEntity(entityName);

  const ctx = {
    entityName,
    entityShort,
    displayName,
    permissionBase,
    serviceField,
    serviceInterface,
    serviceParam,
    dtoFilePath,
  };

  const endpointBlocks = [];
  const warnings = [];
  let needsAllowAnonymous = false;

  // 严格按 ITaktXxxService 接口中的方法声明顺序生成 Action（不重排）
  for (const method of methods) {
    const result = generateEndpoint(method, ctx);
    if (result.skipped) {
      warnings.push(`  ⚠️  跳过方法 ${method.methodName}：${result.reason}`);
      continue;
    }
    if (isAnonymousOptionsMethod(method.methodName)) {
      needsAllowAnonymous = true;
    }
    endpointBlocks.push(result.code);
  }

  if (endpointBlocks.length === 0) {
    return { status: 'failed', message: '未生成任何端点' };
  }

  let file = '';
  file += '// ========================================\n';
  file += '// 项目名称：节拍智控·TaktWise\n';
  file += `// 命名空间：${controllerNs}\n`;
  file += `// 文件名称：${controllerClass}.cs\n`;
  file += `// 创建时间：${todayFileHeaderDate()}\n`;
  file += '// 创建人：Takt365(Cursor AI)\n';
  file += `// 功能描述：${displayName}控制器\n`;
  file += '// \n';
  file += `// 版权信息：Copyright (c) ${new Date().getFullYear()} Takt  All rights reserved.\n`;
  file += '// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。\n';
  file += '// ========================================\n\n';
  file += 'using Microsoft.AspNetCore.Mvc;\n';
  if (needsAllowAnonymous) {
    file += 'using Microsoft.AspNetCore.Authorization;\n';
  }
  file += `using ${dtoNs};\n`;
  file += `using ${serviceNs};\n`;
  file += 'using Takt.Shared.Constants;\n\n';
  file += `namespace ${controllerNs};\n\n`;
  file += '/// <summary>\n';
  file += `/// ${displayName}控制器\n`;
  file += `/// 提供${displayName}的 REST API\n`;
  file += '/// </summary>\n';
  file += `[ApiModule(${apiModule.moduleInt}, "${apiModule.display}")]\n`;
  file += `[Route("api/[controller]", Name = "${escapeCSharpString(routeDisplayName)}")]\n`;
  file += `public class ${controllerClass} : TaktControllerBase\n`;
  file += '{\n';
  file += `    private readonly ${serviceInterface} ${serviceField};\n\n`;
  file += '    /// <summary>\n';
  file += '    /// 构造函数\n';
  file += '    /// </summary>\n';
  file += `    /// <param name="${serviceParam}">${displayName}服务</param>\n`;
  file += `    public ${controllerClass}(${serviceInterface} ${serviceParam})\n`;
  file += '    {\n';
  file += `        ${serviceField} = ${serviceParam};\n`;
  file += '    }\n\n';
  file += endpointBlocks.join('\n');
  file += '}\n';

  if (options.dryRun) {
    warnings.forEach((w) => console.log(w));
    console.log(`  🔍 [dry-run] ${path.relative(CONFIG.backendRoot, outputFile)}`);
    return { status: 'dry-run', outputFile, warnings };
  }

  warnings.forEach((w) => console.log(w));
  const writeResult = writeGeneratedFile(outputFile, file);
  const label = writeResult.created ? '已创建' : '已更新';
  console.log(`  ✅ ${label}: ${path.relative(CONFIG.backendRoot, outputFile)}`);
  return { status: 'written', created: writeResult.created, updated: writeResult.updated, warnings };
}

function scanServiceInterfaces(entityPrefix) {
  const results = [];

  function walk(dir) {
    if (!fs.existsSync(dir)) {
      return;
    }
    for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
      const fullPath = path.join(dir, entry.name);
      if (entry.isDirectory()) {
        if (isInEngineDirectory(fullPath)) {
          continue;
        }
        walk(fullPath);
        continue;
      }
      if (!entry.name.startsWith('ITakt') || !entry.name.endsWith('Service.cs')) {
        continue;
      }
      if (isInEngineDirectory(fullPath)) {
        continue;
      }
      const entityName = entityNameFromInterfaceFile(fullPath);
      if (!entityName) {
        continue;
      }
      if (shouldExcludeInterface(fullPath, entityName)) {
        continue;
      }
      if (entityPrefix) {
        const short = entityName.replace(/^Takt/, '');
        if (short.toLowerCase() !== entityPrefix.toLowerCase()) {
          continue;
        }
      }
      results.push(fullPath);
    }
  }

  walk(CONFIG.servicesRoot);
  return results.sort();
}

function processInterfaceFile(interfaceFile, options) {
  const entityName = entityNameFromInterfaceFile(interfaceFile);
  const entityShort = entityName.replace(/^Takt/, '');
  const rel = path.relative(CONFIG.servicesRoot, interfaceFile);
  console.log(`\n📦 ${entityName} ← ${rel}`);

  if (!options.force && (EXISTING_MANUAL_SERVICE_ENTITIES.has(entityName) || shouldExcludeStandaloneService(entityName))) {
    console.log(`  ⏭️  跳过：已有手工服务（实体 ${entityName}），使用 --force 可覆盖`);
    return { status: 'skipped' };
  }

  const content = readUtf8(interfaceFile);
  const methods = parseInterfaceMethods(content);
  if (methods.length === 0) {
    console.log('  ❌ 未解析到接口方法');
    return { status: 'failed' };
  }

  const result = generateController(interfaceFile, entityName, methods, options);
  if (result.status === 'failed') {
    console.log(`  ❌ ${result.message}`);
  }
  return result;
}

function printUsage() {
  console.log(`
用法:
  node scripts/generate-controllers-from-services.cjs --Holiday
  node scripts/generate-controllers-from-services.cjs --CostCenter --dry-run
  node scripts/generate-controllers-from-services.cjs --Holiday --force

说明:
  - 已禁用 --all；每次必须指定一个实体
  - 扫描 Takt.Application/Services/**/ITaktXxxService.cs（仅接口文件）
  - 输出 Takt.WebApi/Controllers/**/TaktXxxsController.cs（控制器复数，如 TaktHolidaysController、TaktUsersController）
  - 应用服务保持单数 ITaktHolidayService / TaktHolidayService（与 DDD 一致）
  - 对齐 TaktLoginLogsController：try/catch、Success/HandleException、TaktPermission、ApiModule
  - 特殊权限前缀：Code/Generator→code:generator（如 :list/:query）；Code/Database→code:database:{实体}；
    Foundation Dict→foundation:dict；Culture/Translation→foundation:i18n（均无实体中段）
  - [Route("api/[controller]", Name = "…")]：Name 与 TaktPermission 的 displayName 同源（SugarTable 描述去「表」）
  - Action 顺序与 ITaktXxxService 接口方法声明顺序一致（不重排）
  - 排除 User；Auth 等手工控制器默认跳过，须 --force 才覆盖
  - 输出策略：文件不存在则创建，已存在则覆盖更新（无需 --force）
`);
}

function parseArgs() {
  const options = parseSingleEntityGenerateArgs(printUsage);
  assertNotRbacJunctionEntityCli(options.entityPrefix);
  assertNotManualDtoEntityCli(options.entityPrefix);
  return options;
}

// ========================================
// 主流程
// ========================================

console.log('🚀 从服务接口生成 WebApi 控制器...\n');
logGeneratedFileWritePolicy();
console.log(`⏭️  跳过 RBAC 关联表: ${[...RBAC_ASSOCIATION_ENTITY_SHORT_NAMES].join('、')}\n`);

try {
  const options = parseArgs();
  const interfaceFiles = scanServiceInterfaces(options.entityPrefix);

  if (interfaceFiles.length === 0) {
    const manualEntityName = options.entityPrefix ? `Takt${options.entityPrefix}` : '';
    if (manualEntityName && shouldExcludeStandaloneService(manualEntityName)) {
      console.log(`⏭️  跳过：已有手工服务（实体 ${manualEntityName}），使用 --force 可覆盖`);
      console.log('\n📊 已创建 0 个，已更新 0 个，跳过 1 个，失败 0 个');
      console.log('✨ 完成！请编译 Takt.WebApi 并核对权限码与路由。');
      process.exit(0);
    }
    console.error('❌ 未找到匹配的服务接口文件');
    process.exit(1);
  }

  console.log(`📄 匹配服务接口 ${interfaceFiles.length} 个`);

  let created = 0;
  let updated = 0;
  let skipped = 0;
  let failed = 0;

  for (const interfaceFile of interfaceFiles) {
    const result = processInterfaceFile(interfaceFile, options);
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

  console.log(`\n📊 已创建 ${created} 个，已更新 ${updated} 个，跳过 ${skipped} 个，失败 ${failed} 个`);
  console.log('✨ 完成！请编译 Takt.WebApi 并核对权限码与路由。');
} catch (error) {
  console.error('❌ 生成失败:', error);
  process.exit(1);
}

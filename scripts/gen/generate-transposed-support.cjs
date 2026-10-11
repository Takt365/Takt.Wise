// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：scripts
// 文件名称：generate-transposed-support.cjs
// 创建时间：2026-05-23
// 创建人：Takt365(Cursor AI)
// 功能描述：翻译转置 DTO / 服务 / 控制器代码生成（仅 TaktTranslation）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 支持转置表格编辑的实体（当前仅 Translation；DictData 走标准 CRUD，不生成转置）
 * @type {Record<string, object>}
 */
const TRANSPOSABLE_ENTITY_CONFIG = {
  Translation: {
    entityName: 'TaktTranslation',
    /** 转置列头来源：多对一主表 TaktCulture（区域文化） */
    masterTable: {
      entity: 'TaktCulture',
      short: 'Culture',
      sortField: 'SortOrder',
      codeField: 'CultureCode',
      idField: 'CultureId',
    },
    rowId: { name: 'TranslationId', adapt: 'Id', summary: '翻译ID（分组内首条记录 Id，新建为 0）' },
    groupKeyFields: ['I18nKey', 'ResourceGroup', 'ResourceType'],
    rowFields: [
      { name: 'I18nKey', type: 'string', summary: '翻译键（转置行键）', required: true },
      { name: 'ResourceGroup', type: 'string', summary: '资源分组（关联 TaktMenu.Id）' },
      { name: 'ResourceType', type: 'string', summary: '资源类别（字典 sys_resource_type）' },
      { name: 'ContextNote', type: 'string?', summary: '上下文注释' },
    ],
    queryFields: [
      { name: 'CultureId', type: 'long?', summary: '文化ID' },
      { name: 'CultureCode', type: 'string?', summary: '文化编码' },
      { name: 'I18nKey', type: 'string?', summary: '翻译键' },
      { name: 'TranslationText', type: 'string?', summary: '翻译文本' },
      { name: 'ResourceGroup', type: 'string?', summary: '资源分组' },
      { name: 'ResourceType', type: 'string?', summary: '资源类别' },
      { name: 'ContextNote', type: 'string?', summary: '上下文注释' },
    ],
    batchScopeFields: [],
    valueEntityField: 'TranslationText',
    orderBy: 'I18nKey',
    needsCultureRepo: true,
    needsTranslationRepo: false,
  },
};

function isTransposableEntity(entityShort) {
  return Boolean(TRANSPOSABLE_ENTITY_CONFIG[entityShort]);
}

function getTransposableConfig(entityShort) {
  return TRANSPOSABLE_ENTITY_CONFIG[entityShort] || null;
}

function transposedClassNames(entityShort) {
  return {
    row: `Takt${entityShort}TransposedDto`,
    query: `Takt${entityShort}TransposedQueryDto`,
    result: `Takt${entityShort}TransposedResultDto`,
    batch: `Takt${entityShort}TransposedBatchDto`,
  };
}

function emitLongJson(lines) {
  lines.push('    [JsonConverter(typeof(ValueToStringConverter))]');
}

function emitPropertyBlock(lines, field) {
  lines.push('    /// <summary>');
  lines.push(`    /// ${field.summary}`);
  lines.push('    /// </summary>');
  if (field.jsonLong) {
    emitLongJson(lines);
  }
  const required = field.required ? 'required ' : '';
  lines.push(`    public ${required}${field.type} ${field.name} { get; set; }${field.type === 'string' && !field.type.includes('?') ? ' = string.Empty;' : ''}`);
  lines.push('');
}

/**
 * 在聚合 Dto 文件末尾追加转置 DTO 区块
 * @param {string[]} lines
 * @param {string} entityShort
 * @param {string} entityShortDesc
 */
function appendTransposedDtoBlock(lines, entityShort, entityShortDesc) {
  const cfg = getTransposableConfig(entityShort);
  if (!cfg) {
    return;
  }
  const names = transposedClassNames(entityShort);

  lines.push('// ========================================');
  lines.push(`// ${entityShortDesc} 转置 DTO（多语言表格：行=业务键，列=各语言文本）`);
  lines.push('// ========================================');
  lines.push('');
  lines.push('/// <summary>');
  lines.push(`/// ${entityShortDesc}转置行 DTO`);
  lines.push('/// </summary>');
  lines.push(`public class ${names.row}`);
  lines.push('{');
  lines.push('    /// <summary>');
  lines.push(`    /// ${cfg.rowId.summary}`);
  lines.push('    /// </summary>');
  if (cfg.rowId.adapt) {
    lines.push(`    [AdaptMember("${cfg.rowId.adapt}")]`);
  }
  emitLongJson(lines);
  lines.push(`    public long ${cfg.rowId.name} { get; set; }`);
  lines.push('');
  (cfg.rowFields || []).forEach((f) => emitPropertyBlock(lines, f));
  lines.push('    /// <summary>');
  lines.push('    /// 各语言文本；键为 CultureCode（如 zh-CN、en-US），值对应该语言下的显示文本');
  lines.push('    /// </summary>');
  lines.push('    public Dictionary<string, string> Translations { get; set; } = new();');
  lines.push('}');
  lines.push('');

  lines.push('/// <summary>');
  lines.push(`/// ${entityShortDesc}转置分页查询 DTO`);
  lines.push('/// </summary>');
  lines.push(`public class ${names.query} : TaktPagedQuery`);
  lines.push('{');
  lines.push('    /// <summary>');
  lines.push('    /// 租户编码');
  lines.push('    /// </summary>');
  lines.push('    public string? TenantCode { get; set; } = string.Empty;');
  lines.push('');
  cfg.queryFields.forEach((f) => emitPropertyBlock(lines, f));
  lines.push('}');
  lines.push('');

  lines.push('/// <summary>');
  lines.push(`/// ${entityShortDesc}转置分页结果 DTO（含语言列顺序）`);
  lines.push('/// </summary>');
  lines.push(`public class ${names.result}`);
  lines.push('{');
  lines.push('    /// <summary>');
  lines.push('    /// 分页数据');
  lines.push('    /// </summary>');
  lines.push(`    public TaktPagedResult<${names.row}> Paged { get; set; } = null!;`);
  lines.push('');
  lines.push('    /// <summary>');
  lines.push('    /// 语言列顺序（表头从左到右），如 zh-CN、en-US 等');
  lines.push('    /// </summary>');
  lines.push('    public IReadOnlyList<string> CultureCodeOrder { get; set; } = Array.Empty<string>();');
  lines.push('}');
  lines.push('');

  lines.push('/// <summary>');
  lines.push(`/// ${entityShortDesc}转置批量保存 DTO`);
  lines.push('/// </summary>');
  lines.push(`public class ${names.batch}`);
  lines.push('{');
  cfg.batchScopeFields.forEach((f) => emitPropertyBlock(lines, { ...f, required: true }));
  lines.push('    /// <summary>');
  lines.push('    /// 转置行数据');
  lines.push('    /// </summary>');
  lines.push(`    public List<${names.row}> Rows { get; set; } = new();`);
  lines.push('}');
  lines.push('');
}

/**
 * 服务接口中的转置方法声明
 */
function generateTransposedInterfaceMethods(entityShort, desc) {
  const cfg = getTransposableConfig(entityShort);
  if (!cfg) {
    return '';
  }
  const names = transposedClassNames(entityShort);
  let block = '';
  block += '    /// <summary>\n';
  block += `    /// 获取${desc}转置列表（分页，行=业务键，列=各语言）\n`;
  block += '    /// </summary>\n';
  block += `    Task<${names.result}> Get${entityShort}TransposedListAsync(${names.query} queryDto);\n\n`;
  block += '    /// <summary>\n';
  block += `    /// 批量保存${desc}转置数据\n`;
  block += '    /// </summary>\n';
  block += `    Task<int> Save${entityShort}TransposedBatchAsync(${names.batch} dto);\n\n`;
  return block;
}

/**
 * 转置查询表达式（子表 + 租户隔离）
 */
function buildTransposedQueryExpressionBody(entityShort, dtoBase, cfg) {
  const v = entityShort.charAt(0).toLowerCase() + entityShort.slice(1);
  const lines = [`        return ${v} => ${v}.TenantCode == CurrentTenantCode`];
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.KeyWords)`);
  lines.push(`                        || (${v}.CultureCode != null && ${v}.CultureCode.Contains(queryDto.KeyWords))`);
  lines.push(`                        || (${v}.I18nKey != null && ${v}.I18nKey.Contains(queryDto.KeyWords))`);
  lines.push(`                        || (${v}.TranslationText != null && ${v}.TranslationText.Contains(queryDto.KeyWords))`);
  lines.push(`                        || (${v}.ContextNote != null && ${v}.ContextNote.Contains(queryDto.KeyWords)))`);
  lines.push(`                    && (!queryDto.CultureId.HasValue || ${v}.CultureId == queryDto.CultureId.Value)`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.CultureCode) || (${v}.CultureCode != null && ${v}.CultureCode.Contains(queryDto.CultureCode)))`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.I18nKey) || (${v}.I18nKey != null && ${v}.I18nKey.Contains(queryDto.I18nKey)))`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.TranslationText) || (${v}.TranslationText != null && ${v}.TranslationText.Contains(queryDto.TranslationText)))`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.ResourceGroup) || (${v}.ResourceGroup != null && ${v}.ResourceGroup.Contains(queryDto.ResourceGroup)))`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.ResourceType) || (${v}.ResourceType != null && ${v}.ResourceType.Contains(queryDto.ResourceType)))`);
  lines.push(`                    && (string.IsNullOrEmpty(queryDto.ContextNote) || (${v}.ContextNote != null && ${v}.ContextNote.Contains(queryDto.ContextNote)));`);
  return lines.join('\n');
}

/**
 * 服务实现中的转置方法（子表 Translation + 主表 Culture 列头）
 */
function generateTransposedServiceImplementation(entityShort, desc, repoField, entityName, dtoBase) {
  const cfg = getTransposableConfig(entityShort);
  if (!cfg) {
    return {
      ctorFields: '',
      ctorParams: '',
      ctorAssigns: '',
      ctorParamDocs: '',
      methods: '',
      transposedQueryExpr: '',
    };
  }
  const names = transposedClassNames(entityShort);
  const master = cfg.masterTable;
  const cultureRepoField = `_${master.short.charAt(0).toLowerCase()}${master.short.slice(1)}Repository`;
  const cultureEntity = master.entity;
  const masterDesc = master.short === 'Culture' ? '区域文化' : master.short;

  const ctorFields = `    private readonly ITaktTenantRepository<${cultureEntity}> ${cultureRepoField};\n`;
  const ctorParams = `        ITaktTenantRepository<${cultureEntity}> ${master.short.charAt(0).toLowerCase()}${master.short.slice(1)}Repository,\n`;
  const ctorAssigns = `        ${cultureRepoField} = ${master.short.charAt(0).toLowerCase()}${master.short.slice(1)}Repository;\n`;
  const ctorParamDocs = `    /// <param name="${master.short.charAt(0).toLowerCase()}${master.short.slice(1)}Repository">${masterDesc}仓储（转置列头主表）</param>\n`;

  const transposedQueryExpr = buildTransposedQueryExpressionBody(entityShort, dtoBase, cfg);

  const cultureListPredicate = 'x => x.TenantCode == CurrentTenantCode';

  const fillRowFromGroup = `            var first = g.First();
            var row = new ${names.row}
            {
                TranslationId = first.Id,
                I18nKey = first.I18nKey,
                ResourceGroup = first.ResourceGroup,
                ResourceType = first.ResourceType,
                ContextNote = first.ContextNote,
                Translations = new Dictionary<string, string>()
            };
            foreach (var code in cultureCodeOrder)
            {
                var item = g.FirstOrDefault(x => x.${cfg.masterTable.codeField} == code);
                row.Translations[code] = item?.${cfg.valueEntityField} ?? string.Empty;
            }`;

  const saveLoop = `        foreach (var row in dto.Rows)
        {
            foreach (var kvp in row.Translations)
            {
                var cultureCode = kvp.Key;
                var text = kvp.Value ?? string.Empty;
                if (!cultureMap.TryGetValue(cultureCode, out var culture))
                {
                    continue;
                }

                var existing = await ${repoField}.GetListAsync(x =>
                    x.I18nKey == row.I18nKey
                    && x.${cfg.masterTable.codeField} == cultureCode
                    && x.ResourceGroup == row.ResourceGroup
                    && x.ResourceType == row.ResourceType);
                var entity = existing.FirstOrDefault();
                if (entity != null)
                {
                    entity.TranslationText = text;
                    entity.ContextNote = row.ContextNote;
                    entity.${master.idField} = culture.Id;
                    entity.${master.codeField} = cultureCode;
                    await ${repoField}.UpdateAsync(entity);
                    affected += 1;
                }
                else if (!string.IsNullOrWhiteSpace(text))
                {
                    var created = new ${entityName}
                    {
                        ${master.idField} = culture.Id,
                        ${master.codeField} = cultureCode,
                        I18nKey = row.I18nKey,
                        TranslationText = text,
                        ResourceGroup = row.ResourceGroup,
                        ResourceType = row.ResourceType,
                        ContextNote = row.ContextNote
                    };
                    await ${repoField}.CreateAsync(created);
                    affected += 1;
                }
            }
        }`;

  const groupKeyExpr = cfg.groupKeyFields.map((f) => `x.${f}`).join(', ');

  const methods = `
    /// <summary>
    /// 获取转置列头主表（${masterDesc}，仅启用项）
    /// </summary>
    private async Task<List<${cultureEntity}>> GetTransposedMasterCulturesAsync()
    {
        return await ${cultureRepoField}.GetListAsync(
            ${cultureListPredicate},
            x => x.${master.sortField},
            false);
    }

    /// <summary>
    /// 获取${desc}转置列表（分页）
    /// </summary>
    public async Task<${names.result}> Get${entityShort}TransposedListAsync(${names.query} queryDto)
    {
        var cultures = await GetTransposedMasterCulturesAsync();
        var cultureCodeOrder = cultures.Select(c => c.${master.codeField}).ToList();

        var all = await ${repoField}.GetListAsync(TransposedQueryExpression(queryDto));
        var grouped = all
            .GroupBy(x => new { ${groupKeyExpr} })
            .OrderBy(g => g.First().${cfg.orderBy})
            .ToList();
        var total = grouped.Count;
        var pageGroups = grouped
            .Skip((queryDto.PageIndex - 1) * queryDto.PageSize)
            .Take(queryDto.PageSize)
            .ToList();

        var rows = new List<${names.row}>();
        foreach (var g in pageGroups)
        {
${fillRowFromGroup}
            rows.Add(row);
        }

        return new ${names.result}
        {
            Paged = TaktPagedResult<${names.row}>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize),
            CultureCodeOrder = cultureCodeOrder
        };
    }

    /// <summary>
    /// 批量保存${desc}转置数据
    /// </summary>
    public async Task<int> Save${entityShort}TransposedBatchAsync(${names.batch} dto)
    {
        if (dto.Rows == null || dto.Rows.Count == 0)
        {
            return 0;
        }
        var cultures = await GetTransposedMasterCulturesAsync();
        var cultureMap = cultures.ToDictionary(c => c.${master.codeField}, c => c);
        var affected = 0;
${saveLoop}
        return affected;
    }
`;

  return {
    ctorFields,
    ctorParams,
    ctorAssigns,
    ctorParamDocs,
    methods,
    transposedQueryExpr,
    needsEnumsUsing: false,
  };
}

/**
 * 控制器端点生成
 */
function generateTransposedControllerEndpoints(ctx) {
  const cfg = getTransposableConfig(ctx.entityShort);
  if (!cfg) {
    return null;
  }
  const names = transposedClassNames(ctx.entityShort);
  const listMethod = `Get${ctx.entityShort}TransposedListAsync`;
  const saveMethod = `Save${ctx.entityShort}TransposedBatchAsync`;
  const listPerm = `${ctx.permissionBase}:query`;
  const savePerm = `${ctx.permissionBase}:edit`;

  const listCode = `    /// <summary>
    /// 获取${ctx.displayName}转置列表（分页）
    /// </summary>
    [TaktPermission("${listPerm}", "查询${ctx.displayName}转置列表")]
    [HttpGet("transposed")]
    public async Task<IActionResult> ${listMethod}([FromQuery] ${names.query} queryDto)
    {
        try
        {
            var result = await ${ctx.serviceField}.${listMethod}(queryDto);
            return Success(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;

  const saveCode = `    /// <summary>
    /// 批量保存${ctx.displayName}转置数据
    /// </summary>
    [TaktPermission("${savePerm}", "保存${ctx.displayName}转置数据")]
    [HttpPost("transposed/batch")]
    public async Task<IActionResult> ${saveMethod}([FromBody] ${names.batch} dto)
    {
        try
        {
            var count = await ${ctx.serviceField}.${saveMethod}(dto);
            return Success(count, $"已保存 {count} 条");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
`;

  return { code: `${listCode}\n${saveCode}`, listMethod, saveMethod };
}

module.exports = {
  TRANSPOSABLE_ENTITY_CONFIG,
  isTransposableEntity,
  getTransposableConfig,
  transposedClassNames,
  appendTransposedDtoBlock,
  generateTransposedInterfaceMethods,
  generateTransposedServiceImplementation,
  generateTransposedControllerEndpoints,
};

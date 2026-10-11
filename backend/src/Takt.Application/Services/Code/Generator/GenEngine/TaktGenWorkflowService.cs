// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：TaktGenWorkflowService.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成工作流实现：有表/无表两条独立流程，均通过 ITaktGenEngine 生成后端/前端代码
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Takt.Application.Dtos.Code.Generator;
using Takt.Domain.Entities.Code.Generator;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Microsoft.Extensions.Options;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models.Code;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 代码生成工作流服务：有表（选库→选表→导入→生成代码）、无表（建表配置→建物理表→生成代码）两条独立实现，均使用通用代码生成引擎。
/// </summary>
public class TaktGenWorkflowService : ITaktGenWorkflowService
{
    private const string GenButtonCategoryDictTypeCode = "code_generator_button_category";

    /// <summary>
    /// 生成菜单/字段翻译 SQL 时使用的区域文化，与 <c>TaktCultureSeedData</c> 种子一致（4 种）。
    /// </summary>
    private static readonly string[] GenTranslationCultureCodes = { "en-US", "ja-JP", "zh-HK", "zh-CN" };

    private readonly ITaktDatabaseSchemaProvider _schemaProvider;
    private readonly ITaktTenantRepository<TaktGenTable> _genTableRepository;
    private readonly ITaktTenantRepository<TaktGenTableColumn> _genTableColumnRepository;
    private readonly ITaktTenantRepository<TaktDictData> _dictDataRepository;
    private readonly ITaktGenEngine _codeEngine;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly TaktGenEngineOptions _genEngineOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="schemaProvider">数据库元数据提供者（获取表、列、建表等）</param>
    /// <param name="genTableRepository">代码生成表配置仓储</param>
    /// <param name="genTableColumnRepository">代码生成字段配置仓储</param>
    /// <param name="dictDataRepository">字典数据仓储</param>
    /// <param name="codeEngine">通用代码生成引擎（Scriban 渲染）</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="genEngineOptions">代码生成引擎配置（ContentRootPath 定位 wwwroot/Generator）</param>
    public TaktGenWorkflowService(
        ITaktDatabaseSchemaProvider schemaProvider,
        ITaktTenantRepository<TaktGenTable> genTableRepository,
        ITaktTenantRepository<TaktGenTableColumn> genTableColumnRepository,
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        ITaktGenEngine codeEngine,
        ITaktUniqueValidator uniqueValidator,
        ITaktLineNumberGenerator lineNumberGenerator,
        IOptions<TaktGenEngineOptions> genEngineOptions)
    {
        _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
        _genTableRepository = genTableRepository ?? throw new ArgumentNullException(nameof(genTableRepository));
        _genTableColumnRepository = genTableColumnRepository ?? throw new ArgumentNullException(nameof(genTableColumnRepository));
        _dictDataRepository = dictDataRepository ?? throw new ArgumentNullException(nameof(dictDataRepository));
        _codeEngine = codeEngine ?? throw new ArgumentNullException(nameof(codeEngine));
        _uniqueValidator = uniqueValidator ?? throw new ArgumentNullException(nameof(uniqueValidator));
        _lineNumberGenerator = lineNumberGenerator ?? throw new ArgumentNullException(nameof(lineNumberGenerator));
        _genEngineOptions = genEngineOptions?.Value ?? new TaktGenEngineOptions();
    }

    /// <summary>
    /// 从数据库导入指定表：读取表及列元数据，写入 TaktGenTable、TaktGenTableColumn（用于“数据表存在”流程：导入）
    /// </summary>
    /// <remarks>有表流程第 2 步：从数据库导入指定表及列元数据，写入 TaktGenTable、TaktGenTableColumn。生成代码时走通用引擎。</remarks>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">要导入的数据表名</param>
    /// <param name="tableOverrides">表配置覆盖（可选，用于补充实体类名、业务名等）</param>
    /// <returns>导入后的表配置 DTO（含表 ID，可用于后续生成代码）</returns>
    public async Task<TaktGenTableDto> ImportTableFromDatabaseAsync(string tenantCode, string tableName, TaktGenTableCreateDto? tableOverrides = null)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("表名不能为空。", nameof(tableName));

        // 重复导入直接提示，避免唯一索引冲突
        TaktLogger.Debug("[CodeGenWorkflow] 开始从数据库导入表: TenantCode={TenantCode}, TableName={TableName}", tenantCode, tableName);
        var isUnique = await _uniqueValidator.IsUniqueAsync(_genTableRepository, t => t.TableName, tableName, null);
        if (!isUnique)
            throw new TaktBusinessException($"表 {tableName} 已导入，请勿重复导入");

        var columns = await _schemaProvider.GetColumnsAsync(tenantCode, tableName).ConfigureAwait(false);
        if (columns == null || columns.Count == 0)
            throw new InvalidOperationException($"未找到表 {tableName} 的列信息。");

        var tableComment = await _schemaProvider.GetTableCommentAsync(tenantCode, tableName).ConfigureAwait(false);
        var (namePrefix, genModuleName, genBusinessNameRaw) = ParseTableNameParts(tableName);
        // 实体类名与项目约定一致：Takt + 业务名帕斯卡，如 TaktCompany、TaktStandardOperationTime
        var businessPascal = ToPascalCase(genBusinessNameRaw);
        var entityClassName = tableOverrides?.EntityClassName ?? (string.IsNullOrEmpty(businessPascal) ? ToPascalCase(tableName) : "Takt" + businessPascal);
        var resolvedGenModuleName = tableOverrides?.GenModuleName ?? genModuleName;
        var overridePermsPrefix = string.IsNullOrWhiteSpace(tableOverrides?.PermsPrefix)
            ? null
            : tableOverrides!.PermsPrefix!.Trim();
        var resolvedPermsPrefix = TaktGenTableTemplateModel.ResolvePermsPrefixCanonical(
            overridePermsPrefix,
            resolvedGenModuleName,
            entityClassName);

        var databases = await _schemaProvider.GetDatabasesAsync().ConfigureAwait(false);
        var dbInfo = databases?.FirstOrDefault(d => string.Equals(d.TenantCode, tenantCode, StringComparison.OrdinalIgnoreCase));
        var dataSource = dbInfo != null ? $"{dbInfo.DisplayName}:{dbInfo.TenantCode}" : $":{tenantCode}";

        var defaultSortField = columns
            .Where(c => !IsEntityBaseColumn(c.DatabaseColumnName))
            .FirstOrDefault(c => c.IsPrimaryKey)?.DatabaseColumnName
            ?? columns
                .Where(c => !IsEntityBaseColumn(c.DatabaseColumnName))
                .Select(c => c.DatabaseColumnName)
                .FirstOrDefault(name => !string.IsNullOrWhiteSpace(name))
            ?? "id";
        var table = new TaktGenTable
        {
            DataSource = dataSource,
            TableName = tableName,
            TableComment = tableComment ?? tableOverrides?.TableComment,
            InDatabase = 0,
            GenTemplateCategory = tableOverrides?.GenTemplateCategory ?? "crud",
            EntityClassName = entityClassName,
            GenBusinessName = tableOverrides?.GenBusinessName ?? ToPascalCase(genBusinessNameRaw),
            GenFunctionName = tableOverrides?.GenFunctionName,
            GenAuthor = tableOverrides?.GenAuthor ?? "Takt365",
            GenMethod = tableOverrides?.GenMethod ?? 0,
            GenPath = tableOverrides?.GenPath ?? "/",
            PermsPrefix = resolvedPermsPrefix,
            MenuButtonGroup = tableOverrides?.MenuButtonGroup,
            SortType = tableOverrides?.SortType ?? "asc",
            SortField = string.IsNullOrWhiteSpace(tableOverrides?.SortField) ? defaultSortField : tableOverrides!.SortField!,
            NamePrefix = tableOverrides?.NamePrefix ?? namePrefix,
            GenModuleName = resolvedGenModuleName,
            EntityNamespace = tableOverrides?.EntityNamespace ?? BuildNamespace(namePrefix, resolvedGenModuleName, "Domain.Entities"),
            DtoNamespace = tableOverrides?.DtoNamespace ?? BuildNamespace(namePrefix, resolvedGenModuleName, "Application.Dtos"),
            DtoClassName = tableOverrides?.DtoClassName ?? entityClassName + "Dto",
            ServiceNamespace = tableOverrides?.ServiceNamespace ?? BuildNamespace(namePrefix, resolvedGenModuleName, "Application.Services"),
            IServiceClassName = tableOverrides?.IServiceClassName ?? "I" + entityClassName + "Service",
            ServiceClassName = tableOverrides?.ServiceClassName ?? entityClassName + "Service",
            ControllerNamespace = tableOverrides?.ControllerNamespace ?? BuildNamespace(namePrefix, resolvedGenModuleName, "WebApi.Controllers"),
            ControllerClassName = tableOverrides?.ControllerClassName ?? entityClassName + "Controller",
            RepositoryInterfaceNamespace = tableOverrides?.RepositoryInterfaceNamespace ?? "Takt.Domain.Repositories",
            IRepositoryClassName = tableOverrides?.IRepositoryClassName ?? "I" + entityClassName + "Repository",
            RepositoryNamespace = tableOverrides?.RepositoryNamespace ?? BuildNamespace(namePrefix, resolvedGenModuleName, "Infrastructure.Repositories"),
            RepositoryClassName = tableOverrides?.RepositoryClassName ?? entityClassName + "Repository"
        };
        if (tableOverrides != null)
        {
            if (tableOverrides.SubTableName != null) table.SubTableName = tableOverrides.SubTableName;
            if (tableOverrides.SubTableFkName != null) table.SubTableFkName = tableOverrides.SubTableFkName;
            if (tableOverrides.TreeCode != null) table.TreeCode = tableOverrides.TreeCode;
            if (tableOverrides.TreeParentCode != null) table.TreeParentCode = tableOverrides.TreeParentCode;
            if (tableOverrides.TreeName != null) table.TreeName = tableOverrides.TreeName;
            if (tableOverrides.GenFunctionName != null) table.GenFunctionName = tableOverrides.GenFunctionName;
            if (tableOverrides.GenFunction != null) table.GenFunction = tableOverrides.GenFunction;
            table.IsRepository = tableOverrides.IsRepository;
            table.ParentMenuId = tableOverrides.ParentMenuId;
            table.IsGenMenu = tableOverrides.IsGenMenu;
            table.IsGenTranslation = tableOverrides.IsGenTranslation;
            table.FrontUi = tableOverrides.FrontUi;
            table.FrontFormLayout = tableOverrides.FrontFormLayout;
            table.FrontBtnStyle = tableOverrides.FrontBtnStyle;
            table.IsGenCode = tableOverrides.IsGenCode;
            table.IsUseTabs = tableOverrides.IsUseTabs;
            table.TabsFieldCount = tableOverrides.TabsFieldCount;
            if (tableOverrides.OtherGenOptions != null) table.OtherGenOptions = tableOverrides.OtherGenOptions;
            if (tableOverrides.MenuButtonGroup != null) table.MenuButtonGroup = tableOverrides.MenuButtonGroup;
        }

        table = await _genTableRepository.CreateAsync(table).ConfigureAwait(false);
        var tableId = table.Id;

        var toCreate = new List<TaktGenTableColumn>();
        foreach (var col in columns)
        {
            var dbColName = col.DatabaseColumnName?.Trim() ?? string.Empty;
            if (IsEntityBaseColumn(dbColName))
            {
                continue;
            }
            toCreate.Add(BuildGenTableColumnFromSchema(col, tableId, dbColName));
        }
        if (toCreate.Count > 0)
        {
            await AssignGenTableColumnLineNumbersAsync(toCreate, table, tableId, importBatch: true).ConfigureAwait(false);
            await _genTableColumnRepository.CreateRangeAsync(toCreate).ConfigureAwait(false);
        }

        TaktLogger.Information("[CodeGenWorkflow] 导入表完成: TableName={TableName}, TableId={TableId}, 列数={ColumnCount}", tableName, tableId, toCreate.Count);
        return await LoadGenTableDtoWithColumnsAsync(table).ConfigureAwait(false);
    }

    /// <summary>
    /// 从数据库同步指定表的列元数据：已存在列更新库表字段属性，新增列插入，库表已删除列物理移除（保留用户生成配置）
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <returns>同步后的表配置 DTO（含列列表）</returns>
    public async Task<TaktGenTableDto> SyncTableColumnsFromDatabaseAsync(long tableId)
    {
        if (tableId <= 0)
        {
            throw new ArgumentException("表配置 ID 无效。", nameof(tableId));
        }
        TaktLogger.Debug("[CodeGenWorkflow] 开始从数据库同步列: TableId={TableId}", tableId);
        var table = await _genTableRepository.GetByIdAsync(tableId).ConfigureAwait(false);
        if (table == null)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在");
        }
        if (string.IsNullOrWhiteSpace(table.TableName))
        {
            throw new TaktBusinessException("表名为空，无法同步列");
        }
        var tenantCode = table.TenantCode;
        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            throw new TaktBusinessException("租户编码为空，无法同步列");
        }
        var schemaColumns = await _schemaProvider.GetColumnsAsync(tenantCode, table.TableName).ConfigureAwait(false);
        if (schemaColumns == null || schemaColumns.Count == 0)
        {
            throw new InvalidOperationException($"未找到表 {table.TableName} 的列信息。");
        }
        var existing = await _genTableColumnRepository.GetListAsync(x => x.GenTableId == tableId).ConfigureAwait(false);
        var existingByName = existing.ToDictionary(c => c.DatabaseColumnName, StringComparer.OrdinalIgnoreCase);
        var schemaNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var toCreate = new List<TaktGenTableColumn>();
        foreach (var col in schemaColumns)
        {
            var dbColName = col.DatabaseColumnName?.Trim() ?? string.Empty;
            if (IsEntityBaseColumn(dbColName))
            {
                continue;
            }
            schemaNames.Add(dbColName);
            if (existingByName.TryGetValue(dbColName, out var existingCol))
            {
                ApplySchemaMetadataToGenTableColumn(existingCol, col, dbColName);
                await _genTableColumnRepository.UpdateAsync(existingCol).ConfigureAwait(false);
            }
            else
            {
                toCreate.Add(BuildGenTableColumnFromSchema(col, tableId, dbColName));
            }
        }
        foreach (var removed in existing.Where(e => !schemaNames.Contains(e.DatabaseColumnName)))
        {
            await _genTableColumnRepository.DeleteAsync(removed.Id).ConfigureAwait(false);
        }
        if (toCreate.Count > 0)
        {
            await AssignGenTableColumnLineNumbersAsync(toCreate, table, tableId).ConfigureAwait(false);
            await _genTableColumnRepository.CreateRangeAsync(toCreate).ConfigureAwait(false);
        }
        TaktLogger.Information(
            "[CodeGenWorkflow] 同步列完成: TableName={TableName}, TableId={TableId}, 新增={Added}, 更新={Updated}, 删除={Removed}",
            table.TableName,
            tableId,
            toCreate.Count,
            schemaNames.Count - toCreate.Count,
            existing.Count(e => !schemaNames.Contains(e.DatabaseColumnName)));
        return await LoadGenTableDtoWithColumnsAsync(table).ConfigureAwait(false);
    }

    /// <summary>
    /// 按实体类型初始化数据表（无表流程：代码生成后，手动指定实体类型全名）。与项目内 TaktTableInitializer 一致，使用 SqlSugar CodeFirst.InitTables。
    /// </summary>
    /// <remarks>无表流程：代码生成后，手动指定实体类型全名，在指定库中按实体初始化表（与 TaktTableInitializer 一致：CodeFirst.InitTables）。</remarks>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="entityTypeFullName">实体类型全名（如 Takt.Domain.Entities.Code.Generator.TaktGenTable，对应生成的实体文件中的类）</param>
    /// <returns>任务</returns>
    public async Task InitializeTableFromEntityTypeAsync(string tenantCode, string entityTypeFullName)
    {
        TaktLogger.Debug("[CodeGenWorkflow] 按实体初始化数据表: TenantCode={TenantCode}, EntityType={EntityType}", tenantCode, entityTypeFullName);
        await _schemaProvider.InitializeTableFromEntityTypeAsync(tenantCode, entityTypeFullName).ConfigureAwait(false);
        TaktLogger.Information("[CodeGenWorkflow] 按实体初始化数据表完成: {EntityType}", entityTypeFullName);
    }

    /// <summary>
    /// 获取可用于“按实体初始化表”的实体类型全名列表（Domain 中 TaktTenant/Company/Approval 实体基类派生类型）。
    /// </summary>
    /// <returns>实体类型全名列表</returns>
    public async Task<IReadOnlyList<string>> GetAvailableEntityTypeFullNamesAsync()
    {
        return await _schemaProvider.GetAvailableEntityTypeFullNamesAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// 根据表配置与模板生成代码，并按 GenMethod 交付（0=zip，1=自定义路径，2=当前项目）。
    /// </summary>
    public async Task<TaktCodeGenGenerateResultDto> GenerateCodeAsync(
        long tableId,
        TaktGenerateCodeRequestDto request,
        string? sqlCreateBy = null)
    {
        ArgumentNullException.ThrowIfNull(request);
        var table = await _genTableRepository.GetByIdAsync(tableId).ConfigureAwait(false);
        if (table == null)
            throw new InvalidOperationException($"未找到表配置 TableId={tableId}。");

        var genMethod = request.GenMethod ?? table.GenMethod;
        var effectiveTemplates = ResolveEffectiveTemplates(request.Templates, table, _genEngineOptions.ContentRootPath);
        if (effectiveTemplates.Count == 0)
            throw new TaktBusinessException("模板字典不能为空。");

        TaktLogger.Debug("[CodeGenWorkflow] 开始生成代码: TableId={TableId}, GenMethod={GenMethod}, 模板数={TemplateCount}",
            tableId, genMethod, effectiveTemplates.Count);
        var results = await RenderTemplatesAsync(tableId, effectiveTemplates, sqlCreateBy, "生成", table).ConfigureAwait(false);
        if (results.Count == 0)
            throw new TaktBusinessException("未生成任何代码文件，请检查表配置、生成功能与模板。");

        var output = new TaktCodeGenGenerateResultDto { GenMethod = genMethod, FileCount = results.Count };
        if (genMethod == 0)
        {
            output.ZipFileName = BuildZipFileName(table.TableName, tableId);
            output.ZipBytes = CreateZipFromResults(results);
            TaktLogger.Information("[CodeGenWorkflow] zip 生成完成: TableId={TableId}, 文件数={Count}", tableId, results.Count);
            return output;
        }

        var basePath = ResolveGenerationBasePath(table, request.GenPath, genMethod);
        output.BasePath = basePath;
        output.WrittenFilePaths = WriteGeneratedFilesToDisk(basePath, results);
        output.FileCount = output.WrittenFilePaths.Count;
        TaktLogger.Information("[CodeGenWorkflow] 落盘完成: TableId={TableId}, BasePath={BasePath}, 文件数={Count}",
            tableId, basePath, output.FileCount);
        return output;
    }

    /// <summary>
    /// 根据表配置与模板映射渲染预览文件（目标相对路径 + 内容 + 是否已存在），仅用于模板正确性校验，不执行落盘生成。
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <param name="templates">模板键（如 "Backend/Crud/Csharp/Entity.cs"）→ Scriban 模板内容</param>
    /// <param name="resolveTargetRelativePath">根据模板键解析目标相对路径的函数</param>
    /// <param name="targetBasePath">目标根路径（可空；为空时不检查是否已存在）</param>
    /// <param name="sqlCreateBy">生成 SQL 时写入 create_by 的当前登录用户名（可空）</param>
    /// <param name="pathMappings">模板键→目标相对路径映射（可空；为空时使用内置默认映射）</param>
    /// <returns>预览渲染结果（成功文件 + 校验问题）</returns>
    public async Task<TaktCodeGenPreviewResultDto> GeneratePreviewFilesAsync(
        long tableId,
        IReadOnlyDictionary<string, string> templates,
        Func<TaktGenTableDto, string, string?>? resolveTargetRelativePath = null,
        string? targetBasePath = null,
        string? sqlCreateBy = null,
        IReadOnlyDictionary<string, string>? pathMappings = null)
    {
        var result = new TaktCodeGenPreviewResultDto();
        ArgumentNullException.ThrowIfNull(templates);

        var tableEntity = await _genTableRepository.GetByIdAsync(tableId).ConfigureAwait(false);
        if (tableEntity == null)
            throw new InvalidOperationException($"未找到表配置 TableId={tableId}。");
        var effectiveTemplates = ResolveEffectiveTemplates(templates, tableEntity, _genEngineOptions.ContentRootPath);
        if (effectiveTemplates.Count == 0)
        {
            result.ValidationIssues.Add(new TaktCodeGenPreviewValidationIssueDto
            {
                TemplateKey = "global",
                Message = "未找到可用模板，请检查 wwwroot/Generator 目录或表配置的生成模板类型。"
            });
            result.IsValid = false;
            return result;
        }
        TaktLogger.Debug("[CodeGenWorkflow] 预览模板已就绪: TableId={TableId}, 模板数={TemplateCount}", tableId, effectiveTemplates.Count);
        var table = tableEntity.Adapt<TaktGenTableDto>();

        var columns = await _genTableColumnRepository.GetListAsync(c => c.GenTableId == tableId).ConfigureAwait(false);
        if (columns == null || columns.Count == 0)
        {
            result.ValidationIssues.Add(new TaktCodeGenPreviewValidationIssueDto
            {
                TemplateKey = "global",
                Message = $"表 {tableEntity.TableName}（TableId={tableId}）未配置任何字段，无法进行预览校验。"
            });
            result.IsValid = false;
            return result;
        }
        var effectiveTargetBasePath = ResolvePreviewTargetBasePath(tableEntity, targetBasePath);
        var context = TaktGenTemplateContext.From(tableEntity, columns);
        context.Table.SqlCreateBy = !string.IsNullOrWhiteSpace(sqlCreateBy) ? sqlCreateBy.Trim() : (tableEntity.GenAuthor ?? "admin");
        if (context.Table.IsGenMenu == 1)
        {
            context.Table.SqlMenuId = SnowFlakeSingle.Instance.NextId();
            context.Table.SqlMenuButtonRows = await BuildSqlMenuButtonRowsAsync(context).ConfigureAwait(false);
        }
        if (context.Table.IsGenTranslation == 1)
            context.Table.SqlTranslationRows = BuildSqlTranslationRows(context);

        var previewFiles = new List<TaktCodeGenPreviewFileDto>(effectiveTemplates.Count);
        foreach (var kv in effectiveTemplates)
        {
            var templateKey = kv.Key?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(templateKey)) continue;
            var relativePath = ResolvePreviewTargetPath(tableEntity, table, templateKey, resolveTargetRelativePath, pathMappings)
                ?? templateKey;

            // InDatabase=0 表示是库表，实体已存在，不生成实体文件，避免重复
            if (tableEntity.InDatabase == 0 && IsBackendEntityTemplateKey(templateKey))
            {
                TaktLogger.Debug("[CodeGenWorkflow] 预览跳过实体模板（InDatabase=0 库表）: {TemplateKey}", templateKey);
                continue;
            }

            try
            {
                var content = await _codeEngine.RenderAsync(kv.Value, context).ConfigureAwait(false);
                var isExisting = !string.IsNullOrWhiteSpace(effectiveTargetBasePath)
                                 && System.IO.File.Exists(System.IO.Path.Combine(effectiveTargetBasePath, relativePath));
                previewFiles.Add(new TaktCodeGenPreviewFileDto
                {
                    Path = relativePath,
                    Content = content ?? string.Empty,
                    IsExisting = isExisting
                });
            }
            catch (Exception ex)
            {
                result.ValidationIssues.Add(new TaktCodeGenPreviewValidationIssueDto
                {
                    TemplateKey = templateKey,
                    TargetPath = relativePath,
                    Message = ex.Message
                });
            }
        }
        result.PreviewFiles = previewFiles;
        if (previewFiles.Count == 0 && result.ValidationIssues.Count == 0)
        {
            result.ValidationIssues.Add(new TaktCodeGenPreviewValidationIssueDto
            {
                TemplateKey = "global",
                Message = "模板已加载但未生成任何预览文件，请检查表配置（命名空间、生成模板类型）或 InDatabase 跳过规则。"
            });
        }
        result.IsValid = result.ValidationIssues.Count == 0;
        return result;
    }

    /// <summary>
    /// 渲染模板并返回文件内容（仅模板渲染，不落盘、不压缩）。可用于预览校验或正式生成前的中间结果。
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <param name="templates">模板键→模板内容</param>
    /// <param name="sqlCreateBy">SQL 创建人</param>
    /// <param name="scene">场景标识（如“预览”“生成”）</param>
    /// <param name="tableEntity">已加载的表配置实体（可空；为空时按 tableId 查询）</param>
    /// <returns>渲染结果列表</returns>
    private async Task<List<TaktCodeGenResultDto>> RenderTemplatesAsync(
        long tableId,
        IReadOnlyDictionary<string, string> templates,
        string? sqlCreateBy,
        string scene,
        TaktGenTable? tableEntity = null)
    {
        if (templates == null || templates.Count == 0)
            return new List<TaktCodeGenResultDto>();

        var table = tableEntity ?? await _genTableRepository.GetByIdAsync(tableId).ConfigureAwait(false);
        if (table == null)
        {
            TaktLogger.Warning("[CodeGenWorkflow] 未找到表配置: TableId={TableId}", tableId);
            throw new InvalidOperationException($"未找到表配置 TableId={tableId}。");
        }

        var columns = await _genTableColumnRepository.GetListAsync(c => c.GenTableId == tableId).ConfigureAwait(false);
        if (columns == null || columns.Count == 0)
        {
            TaktLogger.Warning("[CodeGenWorkflow] 字段配置为空，停止{Scene}: TableId={TableId}, TableName={TableName}", scene, tableId, table.TableName);
            throw new InvalidOperationException($"表 {table.TableName}（TableId={tableId}）未配置任何字段，已停止{scene}。请先在代码生成器中导入并保存字段配置后再继续。");
        }
        var context = TaktGenTemplateContext.From(table, columns);
        context.Table.SqlCreateBy = !string.IsNullOrWhiteSpace(sqlCreateBy) ? sqlCreateBy.Trim() : (table.GenAuthor ?? "admin");

        // 菜单/翻译 SQL 的雪花 ID：仅在“是否生成=是(1)”时赋值；直接调用 SnowFlakeSingle.Instance.NextId()（见 SqlSugar 文档 1.3 手动调用雪花ID）
        if (context.Table.IsGenMenu == 1)
        {
            context.Table.SqlMenuId = SnowFlakeSingle.Instance.NextId();
            context.Table.SqlMenuButtonRows = await BuildSqlMenuButtonRowsAsync(context).ConfigureAwait(false);
        }
        if (context.Table.IsGenTranslation == 1)
            context.Table.SqlTranslationRows = BuildSqlTranslationRows(context);

        TaktLogger.Debug("[CodeGenWorkflow] 表配置已加载: TableName={TableName}, 列数={ColumnCount}, Scene={Scene}", table.TableName, columns.Count, scene);

        var results = new List<TaktCodeGenResultDto>(templates.Count);
        foreach (var kv in templates)
        {
            var fileName = kv.Key;
            // InDatabase=0 表示是库表，实体已存在，不生成实体文件，避免重复
            if (table.InDatabase == 0 && IsBackendEntityTemplateKey(fileName))
            {
                TaktLogger.Debug("[CodeGenWorkflow] 跳过实体模板（InDatabase=0 库表）: {FileName}", fileName);
                continue;
            }
            TaktLogger.Debug("[CodeGenWorkflow] 渲染模板: {FileName}, Scene={Scene}", fileName, scene);
            var content = await _codeEngine.RenderAsync(kv.Value, context).ConfigureAwait(false);
            var outputPath = TaktGenTemplateLoader.ResolveOutputRelativePath(table, fileName) ?? fileName;
            results.Add(new TaktCodeGenResultDto { FileName = outputPath, Content = content });
        }

        return results;
    }

    /// <summary>
    /// 请求体未传模板时，从 wwwroot/Generator 按表配置的 GenTemplateCategory 加载。
    /// </summary>
    private static IReadOnlyDictionary<string, string> ResolveEffectiveTemplates(
        IReadOnlyDictionary<string, string> templates,
        TaktGenTable table,
        string? contentRootPath)
    {
        if (templates.Count > 0)
            return templates;
        return TaktGenTemplateLoader.LoadTemplates(table.GenTemplateCategory, contentRootPath);
    }

    /// <summary>
    /// 解析预览目标相对路径：PathMappings 优先，其次自定义解析器，最后内置规则。
    /// </summary>
    private static string? ResolvePreviewTargetPath(
        TaktGenTable tableEntity,
        TaktGenTableDto table,
        string templateKey,
        Func<TaktGenTableDto, string, string?>? resolveTargetRelativePath,
        IReadOnlyDictionary<string, string>? pathMappings)
    {
        if (pathMappings != null
            && pathMappings.TryGetValue(templateKey, out var mapped)
            && !string.IsNullOrWhiteSpace(mapped))
            return mapped.Trim();
        var custom = resolveTargetRelativePath?.Invoke(table, templateKey);
        if (!string.IsNullOrWhiteSpace(custom))
            return custom.Trim();
        return TaktGenTemplateLoader.ResolveOutputRelativePath(tableEntity, templateKey);
    }

    /// <summary>
    /// 解析预览/覆盖检测用的目标根路径：显式传入优先；GenMethod=2 解析仓库根；GenMethod=1 用表 GenPath。
    /// </summary>
    private string? ResolvePreviewTargetBasePath(TaktGenTable tableEntity, string? targetBasePath)
    {
        if (!string.IsNullOrWhiteSpace(targetBasePath))
        {
            var trimmed = targetBasePath.Trim();
            if (trimmed != "/")
                return ResolveGenPathDirectory(trimmed);
        }
        if (tableEntity.GenMethod == 2)
            return TaktFileHelper.GetSolutionRootPath(_genEngineOptions.ContentRootPath);
        if (tableEntity.GenMethod == 1
            && !string.IsNullOrWhiteSpace(tableEntity.GenPath)
            && tableEntity.GenPath.Trim() != "/")
            return ResolveGenPathDirectory(tableEntity.GenPath.Trim());
        return null;
    }

    /// <summary>
    /// 解析代码落盘根路径：GenMethod=2 仓库根；GenMethod=1 取请求或表 GenPath（字典 code_generator_path_type 的 DictValue）。
    /// </summary>
    private string ResolveGenerationBasePath(TaktGenTable table, string? genPathOverride, int genMethod)
    {
        var genPath = genPathOverride ?? table.GenPath;
        if (genMethod == 2)
            return TaktFileHelper.GetSolutionRootPath(_genEngineOptions.ContentRootPath);
        if (genMethod == 1)
        {
            if (string.IsNullOrWhiteSpace(genPath) || genPath.Trim() == "/")
                throw new TaktBusinessException("自定义路径不能为空，请选择有效的生成路径。");
            return ResolveGenPathDirectory(genPath.Trim());
        }
        throw new TaktBusinessException($"不支持的生成方式 GenMethod={genMethod}。");
    }

    /// <summary>
    /// 将 GenPath 字典值或目录路径解析为绝对路径；solution 令牌解析为 Solution 根目录。
    /// </summary>
    /// <param name="genPath">字典 code_generator_path_type 的 DictValue 或相对/绝对目录</param>
    /// <returns>绝对路径</returns>
    private string ResolveGenPathDirectory(string genPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(genPath);
        var trimmed = genPath.Trim();
        if (trimmed.Equals("solution", StringComparison.OrdinalIgnoreCase))
            return TaktFileHelper.GetSolutionRootPath(_genEngineOptions.ContentRootPath);
        return Path.GetFullPath(trimmed);
    }

    /// <summary>
    /// 将生成结果打包为 zip 字节数组（UTF-8 无 BOM）。
    /// </summary>
    private static byte[] CreateZipFromResults(IReadOnlyList<TaktCodeGenResultDto> results)
    {
        ArgumentNullException.ThrowIfNull(results);
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var item in results)
            {
                if (string.IsNullOrWhiteSpace(item.FileName))
                    continue;
                ValidateRelativeOutputPath(item.FileName);
                var entryName = item.FileName.Replace('\\', '/');
                var entry = zip.CreateEntry(entryName, CompressionLevel.Optimal);
                using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                writer.Write(item.Content ?? string.Empty);
            }
        }
        return ms.ToArray();
    }

    /// <summary>
    /// 将生成结果写入磁盘，返回已写入的相对路径列表。
    /// </summary>
    private static List<string> WriteGeneratedFilesToDisk(string basePath, IReadOnlyList<TaktCodeGenResultDto> results)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath);
        ArgumentNullException.ThrowIfNull(results);
        var written = new List<string>(results.Count);
        foreach (var item in results)
        {
            if (string.IsNullOrWhiteSpace(item.FileName))
                continue;
            ValidateRelativeOutputPath(item.FileName);
            var relative = item.FileName.Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(basePath, relative);
            var dir = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(fullPath, item.Content ?? string.Empty, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            written.Add(item.FileName.Replace('\\', '/'));
        }
        return written;
    }

    /// <summary>
    /// 校验相对输出路径，禁止目录穿越。
    /// </summary>
    private static void ValidateRelativeOutputPath(string relativePath)
    {
        if (relativePath.Contains("..", StringComparison.Ordinal))
            throw new InvalidOperationException($"非法输出路径: {relativePath}");
    }

    /// <summary>
    /// 构建 zip 下载文件名。
    /// </summary>
    private static string BuildZipFileName(string? tableName, long tableId)
    {
        var baseName = string.IsNullOrWhiteSpace(tableName) ? $"gen_{tableId}" : tableName.Trim();
        foreach (var c in Path.GetInvalidFileNameChars())
            baseName = baseName.Replace(c, '_');
        return $"{baseName}_{DateTime.UtcNow:yyyyMMddHHmmss}.zip";
    }

    /// <summary>
    /// 按 menu_and_translation.sql 模板顺序构建翻译行（每行 Id 用 SnowFlakeSingle.Instance.NextId() 生成）。翻译键与种子一致：菜单标题用 menu.xxx（同 menu_l10n_key），字段用 xxx.entities.fieldname（全小写）；文化代码仅 GenTranslationCultureCodes 四种。
    /// </summary>
    private static List<TaktSqlTranslationRowItem> BuildSqlTranslationRows(TaktGenTemplateContext context)
    {
        var rows = new List<TaktSqlTranslationRowItem>();
        var cultureCodes = GenTranslationCultureCodes;
        var t = context.Table;
        var moduleDot = (t.GenModuleName ?? "").ToLowerInvariant().Replace("_", ".");
        var entityLower = (t.EntityNameCamel ?? "").ToLowerInvariant();
        var menuL10nKey = "menu." + moduleDot + "." + entityLower;
        var pageKeyPrefix = string.IsNullOrEmpty(moduleDot) ? entityLower : (moduleDot + "." + entityLower);
        var comment = t.Comment ?? "";

        foreach (var culture in cultureCodes)
        {
            var titleValue = culture is "zh-CN" or "zh-HK" ? comment + "管理" : comment;
            rows.Add(new TaktSqlTranslationRowItem
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                Culture = culture,
                ResourceKey = menuL10nKey,
                TranslationValue = titleValue,
                ResourceGroup = "menu",
                SortOrder = 0
            });
        }
        foreach (var col in context.Columns.Where(c => c.IsList == 1))
        {
            var colKey = pageKeyPrefix + ".entities." + (col.TsColumnName ?? "").ToLowerInvariant();
            foreach (var culture in cultureCodes)
            {
                rows.Add(new TaktSqlTranslationRowItem
                {
                    Id = SnowFlakeSingle.Instance.NextId(),
                    Culture = culture,
                    ResourceKey = colKey,
                    TranslationValue = col.Comment ?? "",
                    ResourceGroup = "page",
                    SortOrder = col.LineNumber
                });
            }
        }
        foreach (var col in context.Columns.Where(c => c.IsQuery == 1 && c.IsList != 1))
        {
            var colKey = pageKeyPrefix + ".entities." + (col.TsColumnName ?? "").ToLowerInvariant();
            foreach (var culture in cultureCodes)
            {
                rows.Add(new TaktSqlTranslationRowItem
                {
                    Id = SnowFlakeSingle.Instance.NextId(),
                    Culture = culture,
                    ResourceKey = colKey,
                    TranslationValue = col.Comment ?? "",
                    ResourceGroup = "page",
                    SortOrder = col.LineNumber
                });
            }
        }
        return rows;
    }

    /// <summary>
    /// 构建菜单按钮行列表（仅依据 MenuButtonGroup 与 PermsPrefixCanonical，供 menu_and_translation.sql 模板）。
    /// 与 TaktMenuButtonSeedData 一致：权限为 <c>{PermsPrefixCanonical}:{suffix}</c>，I18nKey 为 <c>common.page.button.{suffix}</c>。
    /// </summary>
    /// <param name="context">代码生成模板上下文</param>
    /// <returns>菜单按钮行列表</returns>
    private async Task<List<TaktSqlMenuButtonRowItem>> BuildSqlMenuButtonRowsAsync(TaktGenTemplateContext context)
    {
        var t = context.Table;
        var rows = new List<TaktSqlMenuButtonRowItem>();
        var basePerm = (t.PermsPrefixCanonical ?? string.Empty).Trim();
        if (basePerm.Length == 0)
            return rows;

        var menuCodeUpper = ToMenuCodeUpperFromTableName(t.TableName);
        var (labelBySuffix, sortOrderBySuffix) = await LoadGenButtonCategoryMapsAsync().ConfigureAwait(false);

        var suffixes = ResolveMenuButtonSuffixes(t);
        var orderedSuffixes = suffixes
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(sfx => sortOrderBySuffix.TryGetValue(sfx, out var sortOrder) ? sortOrder : int.MaxValue)
            .ThenBy(sfx => sfx, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var order = 0;
        foreach (var sfx in orderedSuffixes)
        {
            var suffix = sfx.Trim().ToLowerInvariant();
            if (suffix.Length == 0)
                continue;
            order++;
            var menuName = labelBySuffix.TryGetValue(suffix, out var label) ? label : suffix;
            rows.Add(new TaktSqlMenuButtonRowItem
            {
                Id = SnowFlakeSingle.Instance.NextId(),
                MenuCode = BuildSqlMenuButtonCode(menuCodeUpper, suffix),
                MenuName = menuName,
                Permission = $"{basePerm}:{suffix}",
                MenuL10nKey = TaktCommonI18nKeys.MenuButton(suffix),
                SortOrder = order
            });
        }

        return rows;
    }

    /// <summary>
    /// 解析菜单按钮权限后缀：优先 MenuButtonGroup；为空时按 GenFunction 能力回退标准 CRUD 后缀。
    /// </summary>
    /// <param name="table">表级模板模型</param>
    /// <returns>权限后缀列表（小写）</returns>
    private static IReadOnlyList<string> ResolveMenuButtonSuffixes(TaktGenTableTemplateModel table)
    {
        var fromGroup = TaktGenButtonGroupParser.ParseSelectionSuffixes(table.MenuButtonGroup);
        if (fromGroup.Count > 0)
            return fromGroup;

        var fallback = new List<string>();
        void Add(string sfx)
        {
            var normalized = sfx.Trim().ToLowerInvariant();
            if (normalized.Length == 0)
                return;
            if (!fallback.Exists(x => string.Equals(x, normalized, StringComparison.OrdinalIgnoreCase)))
                fallback.Add(normalized);
        }

        if (table.IsQuery == 1)
            Add("query");
        if (table.IsCreate == 1)
            Add("create");
        if (table.IsUpdate == 1 || table.IsStatus == 1 || table.IsSort == 1)
            Add("update");
        if (table.IsDelete == 1)
            Add("delete");
        if (table.IsTemplate == 1 || table.IsImport == 1)
            Add("template");
        if (table.IsImport == 1)
            Add("import");
        if (table.IsExport == 1)
            Add("export");

        if (fallback.Count == 0)
        {
            Add("query");
            Add("create");
            Add("update");
            Add("delete");
        }

        return fallback;
    }

    /// <summary>
    /// 生成按钮 menu_code，与 TaktMenuButtonSeedData.BuildButtonCode 规则一致（后缀大写，超长截断+哈希）。
    /// </summary>
    /// <param name="menuCodeUpper">页面菜单编码（大写）</param>
    /// <param name="buttonSuffix">权限后缀（小写）</param>
    /// <returns>按钮 menu_code</returns>
    private static string BuildSqlMenuButtonCode(string menuCodeUpper, string buttonSuffix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(menuCodeUpper);
        ArgumentException.ThrowIfNullOrWhiteSpace(buttonSuffix);
        var code = $"{menuCodeUpper.Trim()}_{buttonSuffix.Trim().ToUpperInvariant()}";
        if (code.Length <= 50)
            return code;
        var hash = ComputeStableMenuButtonCodeHash(code);
        return code[..43] + "_" + hash;
    }

    /// <summary>
    /// 计算 menu_code 稳定哈希（8 位十六进制，与按钮菜单种子一致）。
    /// </summary>
    /// <param name="input">待哈希字符串</param>
    /// <returns>8 位十六进制哈希</returns>
    private static string ComputeStableMenuButtonCodeHash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes)[..8];
    }

    /// <summary>
    /// 加载 code_generator_button_category 字典：后缀 → 显示名、排序号。
    /// </summary>
    /// <returns>标签与排序映射</returns>
    private async Task<(Dictionary<string, string> Labels, Dictionary<string, int> SortOrders)> LoadGenButtonCategoryMapsAsync()
    {
        var rows = await _dictDataRepository
            .GetListAsync(d => d.DictTypeCode == GenButtonCategoryDictTypeCode && d.IsDeleted == 0)
            .ConfigureAwait(false);
        var ordered = rows
            .OrderBy(r => r.SortOrder)
            .ThenBy(r => r.Id);
        var labels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var sortOrders = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in ordered)
        {
            var sfx = (row.DictValue ?? string.Empty).Trim().ToLowerInvariant();
            if (sfx.Length == 0 || labels.ContainsKey(sfx))
                continue;
            var label = (row.DictLabel ?? string.Empty).Trim();
            labels[sfx] = label.Length > 0 ? label : sfx;
            sortOrders[sfx] = row.SortOrder;
        }
        return (labels, sortOrders);
    }

    /// <summary>
    /// 与菜单 SQL 模板中 <c>menu_code</c> 一致：去掉表名前缀 <c>takt_</c> 后转大写、横线改下划线。
    /// </summary>
    private static string ToMenuCodeUpperFromTableName(string? tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            return "ENTITY";
        var s = tableName.Trim().Replace('-', '_');
        if (s.StartsWith("takt_", StringComparison.OrdinalIgnoreCase))
            s = s[5..];
        return s.ToUpperInvariant();
    }

    /// <summary>
    /// 判断模板键是否为后端实体模板（Backend/Crud/Csharp/Entity.cs）。
    /// </summary>
    private static bool IsBackendEntityTemplateKey(string? templateKey)
    {
        if (string.IsNullOrWhiteSpace(templateKey)) return false;
        var key = templateKey.Replace('\\', '/').Trim();
        return key.EndsWith("/Entity.cs", StringComparison.OrdinalIgnoreCase)
            && key.Contains("/Csharp/", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 将 snake_case 模块名转为命名空间后缀（如 accounting_financial → Accounting.Financial）。
    /// </summary>
    private static string ToNamespaceSuffix(string? moduleSnakeCase)
    {
        if (string.IsNullOrWhiteSpace(moduleSnakeCase)) return string.Empty;
        var parts = moduleSnakeCase.Split(new[] { '_', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(".", parts.Select(p => p.Length > 0 ? char.ToUpperInvariant(p[0]) + p.Substring(1).ToLowerInvariant() : p));
    }

    /// <summary>
    /// 拼接命名空间：前缀.中间段.模块后缀，如 Takt + accounting_financial + Domain.Entities → Takt.Domain.Entities.Accounting.Financial。
    /// </summary>
    private static string BuildNamespace(string namePrefix, string genModuleName, string middleSegment)
    {
        var suffix = ToNamespaceSuffix(genModuleName);
        var prefix = string.IsNullOrEmpty(namePrefix) ? "Takt" : namePrefix;
        return string.IsNullOrEmpty(suffix) ? $"{prefix}.{middleSegment}" : $"{prefix}.{middleSegment}.{suffix}";
    }

    /// <summary>
    /// 将下划线/空格/横线命名字符串转为帕斯卡命名（如 user_name → UserName）。
    /// </summary>
    /// <param name="snakeCase">下划线、空格或横线分隔的字符串</param>
    /// <returns>帕斯卡命名字符串</returns>
    private static string ToPascalCase(string snakeCase)
    {
        if (string.IsNullOrWhiteSpace(snakeCase)) return snakeCase;
        var parts = snakeCase.Split(new[] { '_', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(parts.Select(p => p.Length > 0 ? char.ToUpperInvariant(p[0]) + p.Substring(1).ToLowerInvariant() : p));
    }

    /// <summary>
    /// 根据数据表名拆分为：命名空间前缀、模块名称、业务名。
    /// 约定：表名按下划线分段，第一段为前缀，最后一段或若干段为业务名，中间为模块名。
    /// 例：takt_accounting_financial_company → 前缀=takt，模块=accounting_financial，业务=company；
    /// takt_logistics_manufacturing_bom_standard_operation_time → 前缀=takt，模块=logistics_manufacturing_bom，业务=standard_operation_time。
    /// </summary>
    /// <param name="tableName">数据表名（下划线分隔）</param>
    /// <returns>(NamePrefix 帕斯卡, GenModuleName 下划线, GenBusinessName 原始 snake 段)</returns>
    private static (string NamePrefix, string GenModuleName, string GenBusinessNameRaw) ParseTableNameParts(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            return ("Takt", string.Empty, string.Empty);
        var parts = tableName.Split(new[] { '_', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return ("Takt", string.Empty, string.Empty);
        var first = parts[0];
        var namePrefix = first.Length > 0 ? char.ToUpperInvariant(first[0]) + first.Substring(1).ToLowerInvariant() : "Takt";
        if (parts.Length == 1)
            return (namePrefix, string.Empty, first.ToLowerInvariant());
        if (parts.Length == 2)
            return (namePrefix, parts[1].ToLowerInvariant(), parts[1].ToLowerInvariant());
        // 业务名：最后一段若为“复合业务”常见后缀（rate/time/date/code/name/type 等），取最后 3 段；否则取最后 1 段。至少保留 1 段给模块。
        var lastSegment = parts[^1].ToLowerInvariant();
        var multiSegmentSuffixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "rate", "rates", "time", "date", "code", "name", "type", "control" };
        var businessSegmentCount = 1;
        if (parts.Length >= 5 && multiSegmentSuffixes.Contains(lastSegment))
            businessSegmentCount = 3;
        var moduleCount = parts.Length - 1 - businessSegmentCount;
        if (moduleCount < 1)
        {
            businessSegmentCount = 1;
            moduleCount = parts.Length - 2;
        }
        var genModuleName = moduleCount > 0
            ? string.Join("_", parts.Skip(1).Take(moduleCount).Select(p => p.ToLowerInvariant()))
            : string.Empty;
        var genBusinessNameRaw = string.Join("_", parts.Skip(1 + moduleCount).Take(businessSegmentCount).Select(p => p.ToLowerInvariant()));
        return (namePrefix, genModuleName, genBusinessNameRaw);
    }

    /// <summary>
    /// 实体基类在数据库中的列名（与 TaktCompanyEntityBase.cs 三档基类 SugarColumn 一致），导入时排除；ext_field、remark 保留为业务列。
    /// </summary>
    private static readonly HashSet<string> EntityBaseColumnNames = TaktGenEntityBaseProfile.AllImportColumnNames
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 根据库表列元数据构建代码生成字段配置实体（不含行号，由 AssignGenTableColumnLineNumbersAsync 分配）
    /// </summary>
    /// <param name="schemaColumn">库表列元数据</param>
    /// <param name="tableId">代码生成表 Id</param>
    /// <param name="dbColName">数据库列名（snake_case）</param>
    /// <returns>字段配置实体</returns>
    private static TaktGenTableColumn BuildGenTableColumnFromSchema(
        TaktDatabaseTableColumnInfo schemaColumn,
        long tableId,
        string dbColName)
    {
        var dbType = string.IsNullOrWhiteSpace(schemaColumn.DatabaseDataType) ? "nvarchar" : schemaColumn.DatabaseDataType;
        return new TaktGenTableColumn
        {
            Id = 0,
            GenTableId = tableId,
            LineNumber = 0,
            DatabaseColumnName = dbColName,
            ColumnComment = schemaColumn.ColumnComment,
            DatabaseDataType = dbType,
            CsharpDataType = MapDbTypeToCsharp(dbType),
            CsharpColumnName = ToPascalCase(dbColName),
            Length = schemaColumn.Length,
            DecimalDigits = schemaColumn.DecimalDigits,
            IsPk = schemaColumn.IsPrimaryKey ? 1 : 0,
            IsIncrement = schemaColumn.IsIdentity ? 1 : 0,
            IsRequired = schemaColumn.IsNullable ? 0 : 1,
            IsCreate = 1,
            IsUpdate = 1,
            IsList = 1,
            IsUnique = 0,
            IsExport = 1,
            IsSort = 0,
            IsQuery = 0,
            QueryType = string.Empty,
            HtmlType = "input"
        };
    }

    /// <summary>
    /// 将库表列元数据合并到已有字段配置（仅更新库表相关属性，保留生成/UI 配置）
    /// </summary>
    /// <param name="target">已有字段配置</param>
    /// <param name="schemaColumn">库表列元数据</param>
    /// <param name="dbColName">数据库列名</param>
    private static void ApplySchemaMetadataToGenTableColumn(
        TaktGenTableColumn target,
        TaktDatabaseTableColumnInfo schemaColumn,
        string dbColName)
    {
        var dbType = string.IsNullOrWhiteSpace(schemaColumn.DatabaseDataType) ? "nvarchar" : schemaColumn.DatabaseDataType;
        target.DatabaseColumnName = dbColName;
        target.ColumnComment = schemaColumn.ColumnComment;
        target.DatabaseDataType = dbType;
        target.CsharpDataType = MapDbTypeToCsharp(dbType);
        target.CsharpColumnName = ToPascalCase(dbColName);
        target.Length = schemaColumn.Length;
        target.DecimalDigits = schemaColumn.DecimalDigits;
        target.IsPk = schemaColumn.IsPrimaryKey ? 1 : 0;
        target.IsIncrement = schemaColumn.IsIdentity ? 1 : 0;
        target.IsRequired = schemaColumn.IsNullable ? 0 : 1;
    }

    /// <summary>
    /// 为新增列分配行号（项号/序号，步长 10，首条 10；与 TaktSequenceDefaults 一致）
    /// </summary>
    /// <param name="columns">待插入列（保持库表列顺序）</param>
    /// <param name="table">主表实体</param>
    /// <param name="tableId">主表 Id</param>
    /// <param name="importBatch">是否为库表首次导入批次；为 true 时整批从 10 起按步长 10 连续分配，不查库内最大行号</param>
    /// <returns>任务</returns>
    private async Task AssignGenTableColumnLineNumbersAsync(
        List<TaktGenTableColumn> columns,
        TaktGenTable table,
        long tableId,
        bool importBatch = false)
    {
        if (columns.Count == 0)
        {
            return;
        }
        var columnsNeedLine = importBatch
            ? columns
            : columns.Where(c => c.Id <= 0).ToList();
        if (columnsNeedLine.Count == 0)
        {
            return;
        }
        var maxLine = importBatch
            ? 0
            : await _genTableColumnRepository.GetMaxIntAsync(
                x => x.TenantCode == table.TenantCode && x.GenTableId == tableId,
                x => x.LineNumber).ConfigureAwait(false);
        var businessCode = tableId.ToString();
        var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, columnsNeedLine.Count, maxLine).ToList();
        if (importBatch)
        {
            for (var i = 0; i < columnsNeedLine.Count; i++)
            {
                columnsNeedLine[i].LineNumber = lineSeq[i];
            }
            return;
        }
        var lineIdx = 0;
        foreach (var col in columns)
        {
            if (col.Id <= 0)
            {
                col.LineNumber = lineSeq[lineIdx++];
            }
        }
    }

    /// <summary>
    /// 加载表配置 DTO 并填充列列表（按 LineNumber 升序）
    /// </summary>
    /// <param name="table">主表实体</param>
    /// <returns>表配置 DTO</returns>
    private async Task<TaktGenTableDto> LoadGenTableDtoWithColumnsAsync(TaktGenTable table)
    {
        var dto = table.Adapt<TaktGenTableDto>();
        var columnEntities = await _genTableColumnRepository.GetListAsync(
            x => x.GenTableId == table.Id,
            x => x.LineNumber,
            false).ConfigureAwait(false);
        dto.Columns = columnEntities.Adapt<List<TaktGenTableColumnDto>>();
        return dto;
    }

    /// <summary>
    /// 判断数据库列名是否属于实体基类字段，若是则导入时排除。
    /// </summary>
    private static bool IsEntityBaseColumn(string? dbColumnName)
    {
        return !string.IsNullOrWhiteSpace(dbColumnName) && EntityBaseColumnNames.Contains(dbColumnName.Trim());
    }

    /// <summary>
    /// 将数据库数据类型映射为 C# 类型（如 nvarchar→string、bigint→long、bit→bool）。
    /// </summary>
    /// <param name="dbType">数据库类型名（如 nvarchar、bigint、datetime）</param>
    /// <returns>C# 类型名（如 string、long、DateTime）</returns>
    private static string MapDbTypeToCsharp(string dbType)
    {
        if (string.IsNullOrWhiteSpace(dbType)) return "string";
        var t = dbType.ToLowerInvariant();
        if (t.Contains("int") && !t.Contains("bigint")) return "int";
        if (t.Contains("bigint")) return "long";
        if (t.Contains("decimal") || t.Contains("numeric")) return "decimal";
        if (t.Contains("float") || t.Contains("double")) return "double";
        if (t.Contains("date") || t.Contains("time")) return "DateTime";
        if (t.Contains("bit") || t.Contains("bool")) return "bool";
        if (t.Contains("uniqueidentifier") || t.Contains("guid")) return "Guid";
        return "string";
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator
// 文件名称：TaktGenTableService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成数据表配置应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Code.Generator;
using Takt.Domain.Entities.Code.Generator;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Code.Generator;

/// <summary>
/// 代码生成数据表配置应用服务
/// </summary>
public class TaktGenTableService : TaktServiceBase, ITaktGenTableService
{
    private readonly ITaktTenantRepository<TaktGenTable> _genTableRepository;
    private readonly ITaktTenantRepository<TaktGenTableColumn> _genTableColumnRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="genTableRepository">代码生成数据表配置仓储</param>
    /// <param name="genTableColumnRepository">GenTableColumn仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGenTableService(
        ITaktTenantRepository<TaktGenTable> genTableRepository,
        ITaktTenantRepository<TaktGenTableColumn> genTableColumnRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _genTableRepository = genTableRepository;
        _genTableColumnRepository = genTableColumnRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取代码生成数据表配置列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGenTableDto>> GetGenTableListAsync(TaktGenTableQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktGenTableDto>.Create(
                new List<TaktGenTableDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _genTableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGenTableDto>.Create(
            data.Adapt<List<TaktGenTableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto?> GetGenTableByIdAsync(long id)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktGenTableDto>();
        await FillGenTableDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取代码生成数据表配置选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGenTableOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _genTableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SubTableName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TreeCode ?? string.Empty,
            DictLabel = e.SubTableName ?? e.TreeCode ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建代码生成数据表配置
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto> CreateGenTableAsync(TaktGenTableCreateDto dto)
    {
        var entity = dto.Adapt<TaktGenTable>();
        var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableRepository,
            x => x.DataSource == entity.DataSource
                && x.TableName == entity.TableName);
        if (!isUnique_ix_gen_table_datasource_table_unique)
        {
            throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
        }
        entity = await _genTableRepository.CreateAsync(entity);
                await SaveGenTableChildrenAsync(entity, dto);
        return await GetGenTableByIdAsync(entity.Id) ?? entity.Adapt<TaktGenTableDto>();
    }

    /// <summary>
    /// 更新代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGenTableDto> UpdateGenTableAsync(long id, TaktGenTableUpdateDto dto)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
            _genTableRepository,
            x => x.DataSource == entity.DataSource
                && x.TableName == entity.TableName,
            id);
        if (!isUnique_ix_gen_table_datasource_table_unique)
        {
            throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
        }
        await _genTableRepository.UpdateAsync(entity);
                await SaveGenTableChildrenAsync(entity, dto);
        return await GetGenTableByIdAsync(id) ?? throw new TaktBusinessException("代码生成数据表配置不存在");
    }

    /// <summary>
    /// 删除代码生成数据表配置
    /// </summary>
    /// <param name="id">代码生成数据表配置ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableByIdAsync(long id)
    {
        var entity = await _genTableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在或已删除");
        }
        await _genTableColumnRepository.DeleteAsync(x => x.GenTableId == entity.Id);
        var deleted = await _genTableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("代码生成数据表配置不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除代码生成数据表配置
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGenTableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGenTableByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGenTableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGenTableTemplateDto>(
            sheetName ?? "代码生成数据表配置导入模板",
            fileName ?? "代码生成数据表配置导入模板.xlsx");
    }

    /// <summary>
    /// 导入代码生成数据表配置
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGenTableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGenTableImportDto>(fileStream, sheetName ?? "代码生成数据表配置导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktGenTable>();
                var importKey = $"{entity.DataSource}|{entity.TableName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DataSource、TableName）");
                }
                var isUnique_ix_gen_table_datasource_table_unique = await _uniqueValidator.IsUniqueAsync(
                    _genTableRepository,
                    x => x.DataSource == entity.DataSource
                        && x.TableName == entity.TableName);
                if (!isUnique_ix_gen_table_datasource_table_unique)
                {
                    throw new TaktBusinessException("代码生成数据表配置的DataSource、TableName已存在");
                }
                await _genTableRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出代码生成数据表配置
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGenTableAsync(TaktGenTableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktGenTableQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableExportDto>(),
                sheetName ?? "代码生成数据表配置数据",
                fileName ?? "代码生成数据表配置导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _genTableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGenTableExportDto>(),
                sheetName ?? "代码生成数据表配置数据",
                fileName ?? "代码生成数据表配置导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGenTableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "代码生成数据表配置数据",
            fileName ?? "代码生成数据表配置导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充代码生成数据表配置详情（加载 OneToMany 子表：代码生成数据表列配置）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillGenTableDetailsAsync(TaktGenTableDto dto, TaktGenTable entity)
    {
        if (dto == null)
        {
            return;
        }
        // 代码生成数据表列配置 → dto.Columns
        var columns = await _genTableColumnRepository.GetListAsync(x => x.GenTableId == entity.Id);
        dto.Columns = columns.Adapt<List<TaktGenTableColumnDto>>();
        dto.MaxGenTableColumnLineNumber = await _genTableColumnRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.Id,
            x => x.LineNumber,
            includeSoftDeleted: true);
    }

    /// <summary>
    /// 保存代码生成数据表配置子表级联（代码生成数据表列配置；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveGenTableChildrenAsync(TaktGenTable entity, TaktGenTableCreateDto dto)
    {
        // 代码生成数据表列配置（Columns）
        List<TaktGenTableColumnUpdateDto>? columnsForSave;
        if (dto is TaktGenTableUpdateDto updateDtoForColumns && updateDtoForColumns.Columns != null)
        {
            columnsForSave = updateDtoForColumns.Columns;
        }
        else if (dto.Columns != null)
        {
            columnsForSave = dto.Columns.Adapt<List<TaktGenTableColumnUpdateDto>>();
        }
        else
        {
            columnsForSave = null;
        }
        if (columnsForSave is not { Count: > 0 })
        {
            await _genTableColumnRepository.DeleteAsync(x => x.GenTableId == entity.Id);
        }
        else
        {
            var existingList = await _genTableColumnRepository.GetListAsync(x => x.GenTableId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktGenTableColumn>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < columnsForSave.Count; i++)
            {
                var childDto = columnsForSave[i];
                childDto.GenTableId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                var lineKey = $"{entity.TenantCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("代码生成数据表列配置第{i + 1}项与本次提交的其他项重复（TenantCode、GenTableId、LineNumber）");
                }
                if (childDto.GenTableColumnId > 0)
                {
                    if (!existingById.TryGetValue(childDto.GenTableColumnId, out var target))
                    {
                        throw new TaktBusinessException("代码生成数据表列配置不存在（GenTableColumnId={childDto.GenTableColumnId}）");
                    }
                    if (target.GenTableId != entity.Id)
                    {
                        throw new TaktBusinessException("代码生成数据表列配置不属于当前主表（GenTableColumnId={childDto.GenTableColumnId}）");
                    }
                    submittedIds.Add(childDto.GenTableColumnId);
                    childDto.Adapt(target);
                    target.Id = childDto.GenTableColumnId;
                    target.GenTableId = entity.Id;
                    await _genTableColumnRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktGenTableColumn>();
                    child.Id = 0;
                    child.GenTableId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _genTableColumnRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.TreeCode) ? entity.TreeCode : entity.Id.ToString();
                    var maxLine = await _genTableColumnRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.GenTableId == entity.Id,
                        x => x.LineNumber,
                        includeSoftDeleted: true);
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _genTableColumnRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建代码生成数据表配置查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGenTable, bool>> QueryExpression(TaktGenTableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGenTable>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.DataSource != null && x.DataSource.Contains(keywords))
                || (x.TableName != null && x.TableName.Contains(keywords))
                || (x.TableComment != null && x.TableComment.Contains(keywords))
                || (x.SubTableName != null && x.SubTableName.Contains(keywords))
                || (x.SubTableFkName != null && x.SubTableFkName.Contains(keywords))
                || (x.TreeCode != null && x.TreeCode.Contains(keywords))
                || (x.TreeParentCode != null && x.TreeParentCode.Contains(keywords))
                || (x.TreeName != null && x.TreeName.Contains(keywords))
                || (x.GenTemplateCategory != null && x.GenTemplateCategory.Contains(keywords))
                || (x.GenModuleName != null && x.GenModuleName.Contains(keywords))
                || (x.GenBusinessName != null && x.GenBusinessName.Contains(keywords))
                || (x.GenFunctionName != null && x.GenFunctionName.Contains(keywords))
                || (x.PermsPrefix != null && x.PermsPrefix.Contains(keywords))
                || (x.MenuButtonGroup != null && x.MenuButtonGroup.Contains(keywords))
                || (x.NamePrefix != null && x.NamePrefix.Contains(keywords))
                || (x.EntityNamespace != null && x.EntityNamespace.Contains(keywords))
                || (x.EntityClassName != null && x.EntityClassName.Contains(keywords))
                || (x.DtoNamespace != null && x.DtoNamespace.Contains(keywords))
                || (x.DtoClassName != null && x.DtoClassName.Contains(keywords))
                || (x.ServiceNamespace != null && x.ServiceNamespace.Contains(keywords))
                || (x.IServiceClassName != null && x.IServiceClassName.Contains(keywords))
                || (x.ServiceClassName != null && x.ServiceClassName.Contains(keywords))
                || (x.ControllerNamespace != null && x.ControllerNamespace.Contains(keywords))
                || (x.ControllerClassName != null && x.ControllerClassName.Contains(keywords))
                || (x.RepositoryInterfaceNamespace != null && x.RepositoryInterfaceNamespace.Contains(keywords))
                || (x.IRepositoryClassName != null && x.IRepositoryClassName.Contains(keywords))
                || (x.RepositoryNamespace != null && x.RepositoryNamespace.Contains(keywords))
                || (x.RepositoryClassName != null && x.RepositoryClassName.Contains(keywords))
                || (x.GenFunction != null && x.GenFunction.Contains(keywords))
                || (x.GenPath != null && x.GenPath.Contains(keywords))
                || (x.SortField != null && x.SortField.Contains(keywords))
                || (x.SortType != null && x.SortType.Contains(keywords))
                || (x.GenAuthor != null && x.GenAuthor.Contains(keywords))
                || (x.OtherGenOptions != null && x.OtherGenOptions.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DataSource))
        {
            var dataSource = queryDto.DataSource;
            exp = exp.And(x => x.DataSource != null && x.DataSource.Contains(dataSource));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TableName))
        {
            var tableName = queryDto.TableName;
            exp = exp.And(x => x.TableName != null && x.TableName.Contains(tableName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TableComment))
        {
            var tableComment = queryDto.TableComment;
            exp = exp.And(x => x.TableComment != null && x.TableComment.Contains(tableComment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubTableName))
        {
            var subTableName = queryDto.SubTableName;
            exp = exp.And(x => x.SubTableName != null && x.SubTableName.Contains(subTableName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubTableFkName))
        {
            var subTableFkName = queryDto.SubTableFkName;
            exp = exp.And(x => x.SubTableFkName != null && x.SubTableFkName.Contains(subTableFkName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TreeCode))
        {
            var treeCode = queryDto.TreeCode;
            exp = exp.And(x => x.TreeCode != null && x.TreeCode.Contains(treeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TreeParentCode))
        {
            var treeParentCode = queryDto.TreeParentCode;
            exp = exp.And(x => x.TreeParentCode != null && x.TreeParentCode.Contains(treeParentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TreeName))
        {
            var treeName = queryDto.TreeName;
            exp = exp.And(x => x.TreeName != null && x.TreeName.Contains(treeName));
        }

        if (queryDto?.InDatabase.HasValue == true)
        {
            var inDatabase = queryDto.InDatabase.Value;
            exp = exp.And(x => x.InDatabase == inDatabase);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenTemplateCategory))
        {
            var genTemplateCategory = queryDto.GenTemplateCategory;
            exp = exp.And(x => x.GenTemplateCategory != null && x.GenTemplateCategory.Contains(genTemplateCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenModuleName))
        {
            var genModuleName = queryDto.GenModuleName;
            exp = exp.And(x => x.GenModuleName != null && x.GenModuleName.Contains(genModuleName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenBusinessName))
        {
            var genBusinessName = queryDto.GenBusinessName;
            exp = exp.And(x => x.GenBusinessName != null && x.GenBusinessName.Contains(genBusinessName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenFunctionName))
        {
            var genFunctionName = queryDto.GenFunctionName;
            exp = exp.And(x => x.GenFunctionName != null && x.GenFunctionName.Contains(genFunctionName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PermsPrefix))
        {
            var permsPrefix = queryDto.PermsPrefix;
            exp = exp.And(x => x.PermsPrefix != null && x.PermsPrefix.Contains(permsPrefix));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MenuButtonGroup))
        {
            var menuButtonGroup = queryDto.MenuButtonGroup;
            exp = exp.And(x => x.MenuButtonGroup != null && x.MenuButtonGroup.Contains(menuButtonGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NamePrefix))
        {
            var namePrefix = queryDto.NamePrefix;
            exp = exp.And(x => x.NamePrefix != null && x.NamePrefix.Contains(namePrefix));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EntityNamespace))
        {
            var entityNamespace = queryDto.EntityNamespace;
            exp = exp.And(x => x.EntityNamespace != null && x.EntityNamespace.Contains(entityNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EntityClassName))
        {
            var entityClassName = queryDto.EntityClassName;
            exp = exp.And(x => x.EntityClassName != null && x.EntityClassName.Contains(entityClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DtoNamespace))
        {
            var dtoNamespace = queryDto.DtoNamespace;
            exp = exp.And(x => x.DtoNamespace != null && x.DtoNamespace.Contains(dtoNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DtoClassName))
        {
            var dtoClassName = queryDto.DtoClassName;
            exp = exp.And(x => x.DtoClassName != null && x.DtoClassName.Contains(dtoClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceNamespace))
        {
            var serviceNamespace = queryDto.ServiceNamespace;
            exp = exp.And(x => x.ServiceNamespace != null && x.ServiceNamespace.Contains(serviceNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IServiceClassName))
        {
            var iServiceClassName = queryDto.IServiceClassName;
            exp = exp.And(x => x.IServiceClassName != null && x.IServiceClassName.Contains(iServiceClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ServiceClassName))
        {
            var serviceClassName = queryDto.ServiceClassName;
            exp = exp.And(x => x.ServiceClassName != null && x.ServiceClassName.Contains(serviceClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ControllerNamespace))
        {
            var controllerNamespace = queryDto.ControllerNamespace;
            exp = exp.And(x => x.ControllerNamespace != null && x.ControllerNamespace.Contains(controllerNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ControllerClassName))
        {
            var controllerClassName = queryDto.ControllerClassName;
            exp = exp.And(x => x.ControllerClassName != null && x.ControllerClassName.Contains(controllerClassName));
        }

        if (queryDto?.IsRepository.HasValue == true)
        {
            var isRepository = queryDto.IsRepository.Value;
            exp = exp.And(x => x.IsRepository == isRepository);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RepositoryInterfaceNamespace))
        {
            var repositoryInterfaceNamespace = queryDto.RepositoryInterfaceNamespace;
            exp = exp.And(x => x.RepositoryInterfaceNamespace != null && x.RepositoryInterfaceNamespace.Contains(repositoryInterfaceNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IRepositoryClassName))
        {
            var iRepositoryClassName = queryDto.IRepositoryClassName;
            exp = exp.And(x => x.IRepositoryClassName != null && x.IRepositoryClassName.Contains(iRepositoryClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RepositoryNamespace))
        {
            var repositoryNamespace = queryDto.RepositoryNamespace;
            exp = exp.And(x => x.RepositoryNamespace != null && x.RepositoryNamespace.Contains(repositoryNamespace));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RepositoryClassName))
        {
            var repositoryClassName = queryDto.RepositoryClassName;
            exp = exp.And(x => x.RepositoryClassName != null && x.RepositoryClassName.Contains(repositoryClassName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenFunction))
        {
            var genFunction = queryDto.GenFunction;
            exp = exp.And(x => x.GenFunction != null && x.GenFunction.Contains(genFunction));
        }

        if (queryDto?.GenMethod.HasValue == true)
        {
            var genMethod = queryDto.GenMethod.Value;
            exp = exp.And(x => x.GenMethod == genMethod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenPath))
        {
            var genPath = queryDto.GenPath;
            exp = exp.And(x => x.GenPath != null && x.GenPath.Contains(genPath));
        }

        if (queryDto?.IsGenMenu.HasValue == true)
        {
            var isGenMenu = queryDto.IsGenMenu.Value;
            exp = exp.And(x => x.IsGenMenu == isGenMenu);
        }

        if (queryDto?.ParentMenuId.HasValue == true)
        {
            var parentMenuId = queryDto.ParentMenuId.Value;
            exp = exp.And(x => x.ParentMenuId == parentMenuId);
        }

        if (queryDto?.IsGenTranslation.HasValue == true)
        {
            var isGenTranslation = queryDto.IsGenTranslation.Value;
            exp = exp.And(x => x.IsGenTranslation == isGenTranslation);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SortField))
        {
            var sortField = queryDto.SortField;
            exp = exp.And(x => x.SortField != null && x.SortField.Contains(sortField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SortType))
        {
            var sortType = queryDto.SortType;
            exp = exp.And(x => x.SortType != null && x.SortType.Contains(sortType));
        }

        if (queryDto?.FrontUi.HasValue == true)
        {
            var frontUi = queryDto.FrontUi.Value;
            exp = exp.And(x => x.FrontUi == frontUi);
        }

        if (queryDto?.FrontFormLayout.HasValue == true)
        {
            var frontFormLayout = queryDto.FrontFormLayout.Value;
            exp = exp.And(x => x.FrontFormLayout == frontFormLayout);
        }

        if (queryDto?.FrontBtnStyle.HasValue == true)
        {
            var frontBtnStyle = queryDto.FrontBtnStyle.Value;
            exp = exp.And(x => x.FrontBtnStyle == frontBtnStyle);
        }

        if (queryDto?.IsGenCode.HasValue == true)
        {
            var isGenCode = queryDto.IsGenCode.Value;
            exp = exp.And(x => x.IsGenCode == isGenCode);
        }

        if (queryDto?.GenCodeCount.HasValue == true)
        {
            var genCodeCount = queryDto.GenCodeCount.Value;
            exp = exp.And(x => x.GenCodeCount == genCodeCount);
        }

        if (queryDto?.IsUseTabs.HasValue == true)
        {
            var isUseTabs = queryDto.IsUseTabs.Value;
            exp = exp.And(x => x.IsUseTabs == isUseTabs);
        }

        if (queryDto?.TabsFieldCount.HasValue == true)
        {
            var tabsFieldCount = queryDto.TabsFieldCount.Value;
            exp = exp.And(x => x.TabsFieldCount == tabsFieldCount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenAuthor))
        {
            var genAuthor = queryDto.GenAuthor;
            exp = exp.And(x => x.GenAuthor != null && x.GenAuthor.Contains(genAuthor));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OtherGenOptions))
        {
            var otherGenOptions = queryDto.OtherGenOptions;
            exp = exp.And(x => x.OtherGenOptions != null && x.OtherGenOptions.Contains(otherGenOptions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktGenTableQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DataSource))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TableName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TableComment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubTableName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubTableFkName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TreeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TreeParentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TreeName))
        {
            return true;
        }
        if (queryDto.InDatabase.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenTemplateCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenModuleName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenBusinessName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenFunctionName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PermsPrefix))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MenuButtonGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NamePrefix))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EntityNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EntityClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DtoNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DtoClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IServiceClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ServiceClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ControllerNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ControllerClassName))
        {
            return true;
        }
        if (queryDto.IsRepository.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RepositoryInterfaceNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IRepositoryClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RepositoryNamespace))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RepositoryClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenFunction))
        {
            return true;
        }
        if (queryDto.GenMethod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenPath))
        {
            return true;
        }
        if (queryDto.IsGenMenu.HasValue)
        {
            return true;
        }
        if (queryDto.ParentMenuId.HasValue)
        {
            return true;
        }
        if (queryDto.IsGenTranslation.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SortField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SortType))
        {
            return true;
        }
        if (queryDto.FrontUi.HasValue)
        {
            return true;
        }
        if (queryDto.FrontFormLayout.HasValue)
        {
            return true;
        }
        if (queryDto.FrontBtnStyle.HasValue)
        {
            return true;
        }
        if (queryDto.IsGenCode.HasValue)
        {
            return true;
        }
        if (queryDto.GenCodeCount.HasValue)
        {
            return true;
        }
        if (queryDto.IsUseTabs.HasValue)
        {
            return true;
        }
        if (queryDto.TabsFieldCount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenAuthor))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OtherGenOptions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}

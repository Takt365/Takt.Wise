// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Statistics.QuickQuery
// 文件名称：TaktConfigurableService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：定制报表主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Statistics.QuickQuery;
using Takt.Domain.Entities.Statistics.QuickQuery;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Models.Statistics;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Statistics.QuickQuery;

/// <summary>
/// 定制报表主应用服务
/// </summary>
public class TaktConfigurableService : TaktServiceBase, ITaktConfigurableService
{
    private readonly ITaktCompanyRepository<TaktConfigurable> _configurableRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableSource> _configurableSourceRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableJoin> _configurableJoinRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableField> _configurableFieldRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableSelection> _configurableSelectionRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableGroupBy> _configurableGroupByRepository;
    private readonly ITaktCompanyRepository<TaktConfigurableOrderBy> _configurableOrderByRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktStatQueryExecutor _statQueryExecutor;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configurableRepository">定制报表主仓储</param>
    /// <param name="configurableSourceRepository">ConfigurableSource仓储</param>
    /// <param name="configurableJoinRepository">ConfigurableJoin仓储</param>
    /// <param name="configurableFieldRepository">ConfigurableField仓储</param>
    /// <param name="configurableSelectionRepository">ConfigurableSelection仓储</param>
    /// <param name="configurableGroupByRepository">ConfigurableGroupBy仓储</param>
    /// <param name="configurableOrderByRepository">ConfigurableOrderBy仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="statQueryExecutor">SqlSugar Queryable 定制报表查询执行器</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktConfigurableService(
        ITaktCompanyRepository<TaktConfigurable> configurableRepository,
        ITaktCompanyRepository<TaktConfigurableSource> configurableSourceRepository,
        ITaktCompanyRepository<TaktConfigurableJoin> configurableJoinRepository,
        ITaktCompanyRepository<TaktConfigurableField> configurableFieldRepository,
        ITaktCompanyRepository<TaktConfigurableSelection> configurableSelectionRepository,
        ITaktCompanyRepository<TaktConfigurableGroupBy> configurableGroupByRepository,
        ITaktCompanyRepository<TaktConfigurableOrderBy> configurableOrderByRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktStatQueryExecutor statQueryExecutor,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _configurableRepository = configurableRepository;
        _configurableSourceRepository = configurableSourceRepository;
        _configurableJoinRepository = configurableJoinRepository;
        _configurableFieldRepository = configurableFieldRepository;
        _configurableSelectionRepository = configurableSelectionRepository;
        _configurableGroupByRepository = configurableGroupByRepository;
        _configurableOrderByRepository = configurableOrderByRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _statQueryExecutor = statQueryExecutor;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取定制报表主列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktConfigurableDto>> GetConfigurableListAsync(TaktConfigurableQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _configurableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktConfigurableDto>.Create(
            data.Adapt<List<TaktConfigurableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取定制报表主
    /// </summary>
    /// <param name="id">定制报表主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto?> GetConfigurableByIdAsync(long id)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktConfigurableDto>();
        await FillConfigurableDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取定制报表下拉选项
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    public async Task<List<TaktSelectOption>> GetConfigurableOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var currentUserId = CurrentUserId ?? 0;
        var list = await _configurableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.CompanyCode == CurrentCompanyCode
                && x.ConfigurableStatus == 1
                && (x.IsPublic == 0 || x.CreatedBy == currentUserId),
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ConfigurableName ?? e.ConfigurableCode,
        }).ToList();
    }

    /// <summary>
    /// 创建定制报表主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> CreateConfigurableAsync(TaktConfigurableCreateDto dto)
    {
        var entity = dto.Adapt<TaktConfigurable>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.ConfigurableCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.ConfigurableCode))
        {
            throw new TaktBusinessException("定制报表编码不能为空");
        }
        var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
            _configurableRepository,
            x => x.ConfigurableCode == entity.ConfigurableCode);
        if (!isUnique_ix_configurable_code_unique)
        {
            throw new TaktBusinessException("定制报表编码已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _configurableRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity.MaxQueryRows = NormalizeConfigurableRowLimit(entity.MaxQueryRows);
        entity.MaxExportRows = NormalizeConfigurableRowLimit(entity.MaxExportRows);
        entity = await _configurableRepository.CreateAsync(entity);
                await SaveConfigurableChildrenAsync(entity, dto);
        return await GetConfigurableByIdAsync(entity.Id) ?? entity.Adapt<TaktConfigurableDto>();
    }

    /// <summary>
    /// 更新定制报表主
    /// </summary>
    /// <param name="id">定制报表主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableAsync(long id, TaktConfigurableUpdateDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表主不存在");
        }
        dto.Adapt(entity);
        entity.MaxQueryRows = NormalizeConfigurableRowLimit(entity.MaxQueryRows);
        entity.MaxExportRows = NormalizeConfigurableRowLimit(entity.MaxExportRows);
        var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
            _configurableRepository,
            x => x.ConfigurableCode == entity.ConfigurableCode,
            id);
        if (!isUnique_ix_configurable_code_unique)
        {
            throw new TaktBusinessException("定制报表编码已存在");
        }
        await _configurableRepository.UpdateAsync(entity);
                await SaveConfigurableChildrenAsync(entity, dto);
        return await GetConfigurableByIdAsync(id) ?? throw new TaktBusinessException("定制报表主不存在");
    }

    /// <summary>
    /// 删除定制报表主
    /// </summary>
    /// <param name="id">定制报表主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableByIdAsync(long id)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表主不存在或已删除");
        }
        await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        var deleted = await _configurableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("定制报表主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除定制报表主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteConfigurableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteConfigurableByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新定制报表主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableStatusAsync(TaktConfigurableStatusDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(dto.ConfigurableId);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表主不存在");
        }
        entity.ConfigurableStatus = dto.ConfigurableStatus;
        await _configurableRepository.UpdateAsync(entity);
        return await GetConfigurableByIdAsync(dto.ConfigurableId) ?? throw new TaktBusinessException("定制报表主不存在");
    }

    /// <summary>
    /// 更新定制报表主排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktConfigurableDto> UpdateConfigurableSortAsync(TaktConfigurableSortDto dto)
    {
        var entity = await _configurableRepository.GetByIdAsync(dto.ConfigurableId);
        if (entity == null)
        {
            throw new TaktBusinessException("定制报表主不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _configurableRepository.UpdateAsync(entity);
        return await GetConfigurableByIdAsync(dto.ConfigurableId) ?? throw new TaktBusinessException("定制报表主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetConfigurableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktConfigurableTemplateDto>(
            sheetName ?? "定制报表主导入模板",
            fileName ?? "定制报表主导入模板.xlsx");
    }

    /// <summary>
    /// 导入定制报表主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportConfigurableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktConfigurableImportDto>(fileStream, sheetName ?? "定制报表主导入模板");
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
                var entity = rows[i].Adapt<TaktConfigurable>();
                var importKey = $"{entity.ConfigurableCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与 Excel 中其他行重复（定制报表编码）");
                }
                var isUnique_ix_configurable_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _configurableRepository,
                    x => x.ConfigurableCode == entity.ConfigurableCode);
                if (!isUnique_ix_configurable_code_unique)
                {
                    throw new TaktBusinessException("定制报表编码已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _configurableRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
                }
                entity.MaxQueryRows = NormalizeConfigurableRowLimit(entity.MaxQueryRows);
                entity.MaxExportRows = NormalizeConfigurableRowLimit(entity.MaxExportRows);
                await _configurableRepository.CreateAsync(entity);
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
    /// 导出定制报表主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportConfigurableAsync(TaktConfigurableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktConfigurableQueryDto());
        var list = await _configurableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktConfigurableExportDto>(),
                sheetName ?? "定制报表主数据",
                fileName ?? "定制报表主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktConfigurableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "定制报表主数据",
            fileName ?? "定制报表主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充定制报表主详情（加载 OneToMany 子表：定制报表数据源、定制报表关联、定制报表输出字段、定制报表筛选、定制报表分组、定制报表排序）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillConfigurableDetailsAsync(TaktConfigurableDto dto, TaktConfigurable entity)
    {
        if (dto == null)
        {
            return;
        }
        // 定制报表数据源 → dto.Sources
        var sources = await _configurableSourceRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Sources = sources.Adapt<List<TaktConfigurableSourceDto>>();
        // 定制报表关联 → dto.Joins
        var joins = await _configurableJoinRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Joins = joins.Adapt<List<TaktConfigurableJoinDto>>();
        // 定制报表输出字段 → dto.Fields
        var fields = await _configurableFieldRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Fields = fields.Adapt<List<TaktConfigurableFieldDto>>();
        // 定制报表筛选 → dto.Selections
        var selections = await _configurableSelectionRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.Selections = selections.Adapt<List<TaktConfigurableSelectionDto>>();
        // 定制报表分组 → dto.GroupBys
        var groupbys = await _configurableGroupByRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.GroupBys = groupbys.Adapt<List<TaktConfigurableGroupByDto>>();
        // 定制报表排序 → dto.OrderBys
        var orderbys = await _configurableOrderByRepository.GetListAsync(x => x.ConfigurableId == entity.Id);
        dto.OrderBys = orderbys.Adapt<List<TaktConfigurableOrderByDto>>();
    }

    /// <summary>
    /// 保存定制报表主子表级联（定制报表数据源、定制报表关联、定制报表输出字段、定制报表筛选、定制报表分组、定制报表排序；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveConfigurableChildrenAsync(TaktConfigurable entity, TaktConfigurableCreateDto dto)
    {
        // 定制报表数据源（Sources）
        if (dto.Sources is not { Count: > 0 })
        {
            await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var sources = dto.Sources.Adapt<List<TaktConfigurableSource>>();
            foreach (var child in sources)
            {
                child.ConfigurableId = entity.Id;
            }
            var sourcesNeedSort = sources.Where(c => c.SortOrder <= 0).ToList();
            if (sourcesNeedSort.Count > 0)
            {
                var maxSort = await _configurableSourceRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, sourcesNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in sources)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableSourceRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in sources)
            {
            }
            await _configurableSourceRepository.CreateRangeAsync(sources);
        }
        // 定制报表关联（Joins）
        if (dto.Joins is not { Count: > 0 })
        {
            await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var joins = dto.Joins.Adapt<List<TaktConfigurableJoin>>();
            foreach (var child in joins)
            {
                child.ConfigurableId = entity.Id;
            }
            var joinsNeedSort = joins.Where(c => c.SortOrder <= 0).ToList();
            if (joinsNeedSort.Count > 0)
            {
                var maxSort = await _configurableJoinRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, joinsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in joins)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableJoinRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in joins)
            {
            }
            await _configurableJoinRepository.CreateRangeAsync(joins);
        }
        // 定制报表输出字段（Fields）
        if (dto.Fields is not { Count: > 0 })
        {
            await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var fields = dto.Fields.Adapt<List<TaktConfigurableField>>();
            foreach (var child in fields)
            {
                child.ConfigurableId = entity.Id;
            }
            var fieldsNeedSort = fields.Where(c => c.SortOrder <= 0).ToList();
            if (fieldsNeedSort.Count > 0)
            {
                var maxSort = await _configurableFieldRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, fieldsNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in fields)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableFieldRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in fields)
            {
            }
            await _configurableFieldRepository.CreateRangeAsync(fields);
        }
        // 定制报表筛选（Selections）
        if (dto.Selections is not { Count: > 0 })
        {
            await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var selections = dto.Selections.Adapt<List<TaktConfigurableSelection>>();
            foreach (var child in selections)
            {
                child.ConfigurableId = entity.Id;
            }
            EnsureUniqueSelectionSortOrders(selections);
            await _configurableSelectionRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in selections)
            {
            }
            await _configurableSelectionRepository.CreateRangeAsync(selections);
        }
        // 定制报表分组（GroupBys）
        if (dto.GroupBys is not { Count: > 0 })
        {
            await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var groupbys = dto.GroupBys.Adapt<List<TaktConfigurableGroupBy>>();
            foreach (var child in groupbys)
            {
                child.ConfigurableId = entity.Id;
            }
            var groupbysNeedSort = groupbys.Where(c => c.SortOrder <= 0).ToList();
            if (groupbysNeedSort.Count > 0)
            {
                var maxSort = await _configurableGroupByRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, groupbysNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in groupbys)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableGroupByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in groupbys)
            {
            }
            await _configurableGroupByRepository.CreateRangeAsync(groupbys);
        }
        // 定制报表排序（OrderBys）
        if (dto.OrderBys is not { Count: > 0 })
        {
            await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
        }
        else
        {
            var orderbys = dto.OrderBys.Adapt<List<TaktConfigurableOrderBy>>();
            foreach (var child in orderbys)
            {
                child.ConfigurableId = entity.Id;
            }
            var orderbysNeedSort = orderbys.Where(c => c.SortOrder <= 0).ToList();
            if (orderbysNeedSort.Count > 0)
            {
                var maxSort = await _configurableOrderByRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfigurableId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, orderbysNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in orderbys)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _configurableOrderByRepository.DeleteAsync(x => x.ConfigurableId == entity.Id);
            foreach (var child in orderbys)
            {
            }
            await _configurableOrderByRepository.CreateRangeAsync(orderbys);
        }
    }

    /// <summary>
    /// 获取 运行时筛选条件定义
    /// </summary>
    /// <param name="id">定制报表主键</param>
    /// <returns>运行时屏幕 DTO</returns>
    public async Task<TaktConfigurableRuntimeScreenDto> GetConfigurableRuntimeScreenAsync(long id)
    {
        var bundle = await LoadConfigurableRuntimeBundleAsync(id);
        return new TaktConfigurableRuntimeScreenDto
        {
            ConfigurableId = bundle.Entity.Id,
            ConfigurableCode = bundle.Entity.ConfigurableCode,
            ConfigurableName = bundle.Entity.ConfigurableName,
            MaxQueryRows = bundle.Entity.MaxQueryRows,
            MaxExportRows = bundle.Entity.MaxExportRows,
            Columns = BuildRuntimeColumns(bundle.Fields),
            Selections = bundle.Selections
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Id)
                .Select(x => new TaktConfigurableRuntimeSelectionDto
                {
                    ConfigurableSelectionId = x.Id,
                    SortOrder = x.SortOrder,
                    SourceAlias = x.SourceAlias,
                    ColumnName = x.ColumnName,
                    DisplayName = x.DisplayName,
                    FilterOperator = x.FilterOperator,
                    IsRequired = x.IsRequired,
                    DefaultValue = x.DefaultValue,
                    DefaultValueTo = x.DefaultValueTo,
                })
                .ToList(),
        };
    }

    /// <summary>
    /// 执行定制报表查询（分页）
    /// </summary>
    /// <param name="id">定制报表主键</param>
    /// <param name="dto">查询参数与筛选值</param>
    /// <returns>查询结果</returns>
    public async Task<TaktConfigurableQueryResultDto> ExecuteConfigurableQueryAsync(
        long id,
        TaktConfigurableExecuteQueryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var bundle = await LoadConfigurableRuntimeBundleAsync(id);
        return await ExecuteQueryFromBundleAsync(bundle, dto);
    }

    /// <summary>
    /// 设计态预览查询（未保存定制报表定义）
    /// </summary>
    /// <param name="dto">定制报表定义与分页参数</param>
    /// <returns>查询结果</returns>
    public async Task<TaktConfigurableQueryResultDto> PreviewConfigurableQueryAsync(
        TaktConfigurablePreviewQueryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var bundle = BuildPreviewRuntimeBundle(dto);
        return await ExecuteQueryFromBundleAsync(bundle, dto);
    }

    /// <summary>
    /// ExportConfigurableDataAsync
    /// </summary>
    public async Task<(string fileName, byte[] content)> ExportConfigurableDataAsync(
        long id,
        TaktConfigurableExportDataDto dto,
        string? sheetName = null,
        string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var bundle = await LoadConfigurableRuntimeBundleAsync(id);
        var runtimeValues = BuildRuntimeSelectionValues(bundle.Selections, dto.SelectionValues);
        var buildRequest = MapBuildRequest(bundle, runtimeValues);
        var maxRows = ResolveRuntimeRowLimit(dto.RowLimit, bundle.Entity.MaxExportRows);
        var queryResult = await ExecuteStatQueryTopAsync(buildRequest, maxRows);
        var dictRows = queryResult.Rows
            .Select(row => (IReadOnlyDictionary<string, object?>)row)
            .ToList();
        var resolvedSheet = string.IsNullOrWhiteSpace(sheetName)
            ? TaktNamingHelper.DefaultSheetNameEnglish("ConfigurableData")
            : sheetName;
        var resolvedFileName = string.IsNullOrWhiteSpace(fileName)
            ? $"{bundle.Entity.ConfigurableCode}_data"
            : fileName;
        return await TaktExcelHelper.ExportDictionaryRowsAsync(
            dictRows,
            queryResult.OutputKeys,
            queryResult.OutputLabels,
            resolvedSheet,
            resolvedFileName);
    }

    /// <summary>
    /// 加载报表运行时定义（校验启用状态与归属）
    /// </summary>
    /// <param name="id">定制报表主键</param>
    /// <returns>运行时定义包</returns>
    private async Task<ConfigurableRuntimeBundle> LoadConfigurableRuntimeBundleAsync(long id)
    {
        var entity = await _configurableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            ThrowBusinessException("定制报表不存在");
        }
        if (entity.ConfigurableStatus != 1)
        {
            ThrowBusinessException("报表已禁用，无法执行");
        }
        if (entity.IsPublic == 1 && CurrentUserId.HasValue && entity.CreatedBy != CurrentUserId.Value)
        {
            ThrowBusinessException("无权执行该私有报表");
        }
        var sources = await _configurableSourceRepository.GetListAsync(x => x.ConfigurableId == id);
        if (sources.Count == 0)
        {
            ThrowBusinessException("报表未配置数据源");
        }
        var fields = await _configurableFieldRepository.GetListAsync(x => x.ConfigurableId == id);
        if (fields.Count(x => x.IsVisible == 1) == 0)
        {
            ThrowBusinessException("报表未配置输出字段");
        }
        var joins = await _configurableJoinRepository.GetListAsync(x => x.ConfigurableId == id);
        var selections = await _configurableSelectionRepository.GetListAsync(x => x.ConfigurableId == id);
        EnsureUniqueSelectionSortOrders(selections);
        var groupBys = await _configurableGroupByRepository.GetListAsync(x => x.ConfigurableId == id);
        var orderBys = await _configurableOrderByRepository.GetListAsync(x => x.ConfigurableId == id);
        return new ConfigurableRuntimeBundle(
            entity,
            sources,
            joins,
            fields,
            selections,
            groupBys,
            orderBys);
    }

    /// <summary>
    /// 由设计态 DTO 构建运行时定义包（不落库）
    /// </summary>
    /// <param name="dto">预览查询 DTO</param>
    /// <returns>运行时定义包</returns>
    private ConfigurableRuntimeBundle BuildPreviewRuntimeBundle(TaktConfigurablePreviewQueryDto dto)
    {
        var sources = dto.Sources?.Adapt<List<TaktConfigurableSource>>() ?? new List<TaktConfigurableSource>();
        if (sources.Count == 0)
        {
            ThrowBusinessException("报表未配置数据源");
        }
        var fields = dto.Fields?.Adapt<List<TaktConfigurableField>>() ?? new List<TaktConfigurableField>();
        if (fields.Count(x => x.IsVisible == 1) == 0)
        {
            ThrowBusinessException("报表未配置输出字段");
        }
        var joins = dto.Joins?.Adapt<List<TaktConfigurableJoin>>() ?? new List<TaktConfigurableJoin>();
        var selections = dto.Selections?.Adapt<List<TaktConfigurableSelection>>() ?? new List<TaktConfigurableSelection>();
        EnsureUniqueSelectionSortOrders(selections);
        var groupBys = dto.GroupBys?.Adapt<List<TaktConfigurableGroupBy>>() ?? new List<TaktConfigurableGroupBy>();
        var orderBys = dto.OrderBys?.Adapt<List<TaktConfigurableOrderBy>>() ?? new List<TaktConfigurableOrderBy>();
        var maxQueryRows = NormalizeConfigurableRowLimit(dto.MaxQueryRows);
        var entity = new TaktConfigurable
        {
            DistinctRows = dto.DistinctRows,
            MaxQueryRows = maxQueryRows,
            ConfigurableStatus = 1,
        };
        return new ConfigurableRuntimeBundle(entity, sources, joins, fields, selections, groupBys, orderBys);
    }

    /// <summary>
    /// 按运行时定义包执行分页查询
    /// </summary>
    /// <param name="bundle">运行时定义包</param>
    /// <param name="dto">分页与筛选值</param>
    /// <returns>查询结果</returns>
    private async Task<TaktConfigurableQueryResultDto> ExecuteQueryFromBundleAsync(
        ConfigurableRuntimeBundle bundle,
        TaktConfigurableExecuteQueryDto dto)
    {
        var pageIndex = TaktPagedClamp.NormalizePageIndex(dto.PageIndex);
        var queryRowCap = ResolveRuntimeRowLimit(dto.RowLimit, bundle.Entity.MaxQueryRows);
        var maxPageSize = Math.Min(queryRowCap, TaktPagedClamp.DefaultMaxPageSize);
        var pageSize = TaktPagedClamp.NormalizePageSize(dto.PageSize, maxPageSize);
        var runtimeValues = BuildRuntimeSelectionValues(bundle.Selections, dto.SelectionValues);
        var buildRequest = MapBuildRequest(bundle, runtimeValues);
        var queryResult = await ExecuteStatQueryPagedAsync(
            buildRequest,
            pageIndex,
            pageSize,
            maxPageSize);
        var cappedTotal = Math.Min(queryResult.Total, queryRowCap);
        var skip = TaktPagedClamp.ComputeSkip(pageIndex, pageSize);
        var rows = skip >= cappedTotal
            ? new List<Dictionary<string, object?>>()
            : queryResult.Rows.ToList();
        var columns = queryResult.OutputKeys
            .Select((key, index) => new TaktConfigurableRuntimeColumnDto
            {
                Key = key,
                Label = queryResult.OutputLabels[index],
            })
            .ToList();
        return new TaktConfigurableQueryResultDto
        {
            Columns = columns,
            Rows = rows,
            Total = cappedTotal,
            PageIndex = pageIndex,
            PageSize = pageSize,
        };
    }

    /// <summary>
    /// 将运行时定义包映射为 SqlSugar Queryable 编译请求
    /// </summary>
    /// <param name="bundle">运行时定义包</param>
    /// <param name="runtimeValues">筛选值</param>
    /// <returns>编译请求</returns>
    private TaktStatQueryBuildRequest MapBuildRequest(
        ConfigurableRuntimeBundle bundle,
        IReadOnlyDictionary<long, TaktStatQuerySelectionValue> runtimeValues)
    {
        return TaktConfigurableQueryMapper.MapBuildRequest(
            bundle.Entity,
            CurrentTenantCode,
            CurrentCompanyCode,
            bundle.Sources,
            bundle.Joins,
            bundle.Fields,
            bundle.Selections,
            bundle.GroupBys,
            bundle.OrderBys,
            runtimeValues);
    }

    /// <summary>
    /// 执行 SqlSugar Queryable 分页查询
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页大小</param>
    /// <param name="maxPageSize">pageSize 上限</param>
    /// <returns>分页结果</returns>
    private async Task<TaktStatQueryPageResult> ExecuteStatQueryPagedAsync(
        TaktStatQueryBuildRequest request,
        int pageIndex,
        int pageSize,
        int maxPageSize)
    {
        try
        {
            return await _statQueryExecutor.ExecutePagedAsync(
                request,
                pageIndex,
                pageSize,
                maxPageSize);
        }
        catch (ArgumentException ex)
        {
            ThrowBusinessException(ex.Message);
            return null!;
        }
    }

    /// <summary>
    /// 执行 SqlSugar Queryable 导出行数上限查询
    /// </summary>
    /// <param name="request">编译请求</param>
    /// <param name="maxRows">最大行数</param>
    /// <returns>查询结果</returns>
    private async Task<TaktStatQueryPageResult> ExecuteStatQueryTopAsync(
        TaktStatQueryBuildRequest request,
        int maxRows)
    {
        try
        {
            return await _statQueryExecutor.ExecuteTopAsync(request, maxRows);
        }
        catch (ArgumentException ex)
        {
            ThrowBusinessException(ex.Message);
            return null!;
        }
    }

    /// <summary>
    /// 构建运行时输出列（不含 SQL 编译）
    /// </summary>
    /// <param name="fields">输出字段</param>
    /// <returns>列定义</returns>
    private static List<TaktConfigurableRuntimeColumnDto> BuildRuntimeColumns(IReadOnlyList<TaktConfigurableField> fields)
    {
        return fields
            .Where(x => x.IsVisible == 1)
            .OrderBy(x => x.SortOrder)
            .Select(field => new TaktConfigurableRuntimeColumnDto
            {
                Key = ResolveFieldOutputKey(field),
                Label = string.IsNullOrWhiteSpace(field.DisplayName) ? field.ColumnName : field.DisplayName,
            })
            .ToList();
    }

    /// <summary>
    /// 保证筛选项 SortOrder 唯一且从 1 递增（避免多条件共用同一输入键）
    /// </summary>
    /// <param name="selections">筛选定义</param>
    private static void EnsureUniqueSelectionSortOrders(List<TaktConfigurableSelection> selections)
    {
        if (selections.Count == 0)
        {
            return;
        }
        var ordered = selections
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Id)
            .ToList();
        for (var i = 0; i < ordered.Count; i++)
        {
            ordered[i].SortOrder = i + 1;
        }
        selections.Clear();
        selections.AddRange(ordered);
    }

    /// <summary>
    /// 合并用户提交的筛选值（未提交或空值视为不限制，查询全部；不使用库内 DefaultValue 回填）
    /// </summary>
    /// <param name="selections">筛选定义</param>
    /// <param name="inputValues">用户输入</param>
    /// <returns>运行时筛选字典</returns>
    private static Dictionary<long, TaktStatQuerySelectionValue> BuildRuntimeSelectionValues(
        IReadOnlyList<TaktConfigurableSelection> selections,
        IReadOnlyList<TaktConfigurableRuntimeSelectionValueDto>? inputValues)
    {
        var inputById = new Dictionary<long, TaktConfigurableRuntimeSelectionValueDto>();
        var inputBySort = new Dictionary<int, TaktConfigurableRuntimeSelectionValueDto>();
        if (inputValues != null)
        {
            foreach (var item in inputValues)
            {
                if (item.ConfigurableSelectionId > 0 && !inputById.ContainsKey(item.ConfigurableSelectionId))
                {
                    inputById[item.ConfigurableSelectionId] = item;
                }
                if (!inputBySort.ContainsKey(item.SortOrder))
                {
                    inputBySort[item.SortOrder] = item;
                }
            }
        }
        var result = new Dictionary<long, TaktStatQuerySelectionValue>();
        foreach (var selection in selections)
        {
            TaktConfigurableRuntimeSelectionValueDto? input = null;
            var hasInput = false;
            if (selection.Id > 0 && inputById.TryGetValue(selection.Id, out var byId))
            {
                input = byId;
                hasInput = true;
            }
            else if (inputBySort.TryGetValue(selection.SortOrder, out var bySort))
            {
                input = bySort;
                hasInput = true;
            }
            if (!hasInput)
            {
                continue;
            }
            var value = input?.Value?.Trim();
            var valueTo = input?.ValueTo?.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }
            var runtimeKey = ResolveSelectionRuntimeKey(selection.Id, selection.SortOrder);
            result[runtimeKey] = new TaktStatQuerySelectionValue
            {
                Value = value,
                ValueTo = valueTo,
                FilterOperator = input?.FilterOperator ?? 0,
            };
        }
        return result;
    }

    /// <summary>
    /// 运行时筛选值字典键（持久化行用 Id；预览无 Id 时用 -SortOrder）
    /// </summary>
    /// <param name="selectionId">筛选项主键</param>
    /// <param name="sortOrder">排序号</param>
    /// <returns>运行时键</returns>
    private static long ResolveSelectionRuntimeKey(long selectionId, int sortOrder) =>
        selectionId > 0 ? selectionId : -sortOrder;

    /// <summary>
    /// 解析输出字段键名
    /// </summary>
    /// <param name="field">输出字段</param>
    /// <returns>输出键</returns>
    private static string ResolveFieldOutputKey(TaktConfigurableField field)
    {
        if (!string.IsNullOrWhiteSpace(field.OutputAlias))
        {
            return field.OutputAlias;
        }
        return $"{field.SourceAlias}_{field.ColumnName}";
    }

    /// <summary>
    /// 报表运行时定义包
    /// </summary>
    private sealed class ConfigurableRuntimeBundle
    {
        /// <summary>
        /// 初始化运行时定义包
        /// </summary>
        public ConfigurableRuntimeBundle(
            TaktConfigurable entity,
            List<TaktConfigurableSource> sources,
            List<TaktConfigurableJoin> joins,
            List<TaktConfigurableField> fields,
            List<TaktConfigurableSelection> selections,
            List<TaktConfigurableGroupBy> groupBys,
            List<TaktConfigurableOrderBy> orderBys)
        {
            Entity = entity;
            Sources = sources;
            Joins = joins;
            Fields = fields;
            Selections = selections;
            GroupBys = groupBys;
            OrderBys = orderBys;
        }

        /// <summary>
        /// 定制报表主表
        /// </summary>
        public TaktConfigurable Entity { get; }

        /// <summary>
        /// 数据源
        /// </summary>
        public List<TaktConfigurableSource> Sources { get; }

        /// <summary>
        /// 关联
        /// </summary>
        public List<TaktConfigurableJoin> Joins { get; }

        /// <summary>
        /// 输出字段
        /// </summary>
        public List<TaktConfigurableField> Fields { get; }

        /// <summary>
        /// 筛选
        /// </summary>
        public List<TaktConfigurableSelection> Selections { get; }

        /// <summary>
        /// 分组
        /// </summary>
        public List<TaktConfigurableGroupBy> GroupBys { get; }

        /// <summary>
        /// 排序
        /// </summary>
        public List<TaktConfigurableOrderBy> OrderBys { get; }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 规范化查询/导出行数（未配置用默认 500，上限 50000）
    /// </summary>
    /// <param name="value">原始行数</param>
    /// <returns>合法行数</returns>
    private static int NormalizeConfigurableRowLimit(int value)
    {
        if (value <= 0)
        {
            return TaktConfigurableConstants.DefaultRowLimit;
        }
        return Math.Min(value, TaktConfigurableConstants.MaxRowLimit);
    }

    /// <summary>
    /// 解析运行时本次查询/导出行数（可选，默认 500，全局最大 50000，且不超过报表配置）
    /// </summary>
    /// <param name="dtoRowLimit">请求中的行数（0 表示默认）</param>
    /// <param name="entityConfiguredLimit">报表配置的行数上限</param>
    /// <returns>实际行数上限</returns>
    private static int ResolveRuntimeRowLimit(int dtoRowLimit, int entityConfiguredLimit)
    {
        var requested = dtoRowLimit > 0 ? dtoRowLimit : TaktConfigurableConstants.DefaultRowLimit;
        requested = Math.Min(requested, TaktConfigurableConstants.MaxRowLimit);
        var entityCap = NormalizeConfigurableRowLimit(entityConfiguredLimit);
        return Math.Min(requested, entityCap);
    }

    /// <summary>
    /// 构建定制报表主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private Expression<Func<TaktConfigurable, bool>> QueryExpression(TaktConfigurableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktConfigurable>();
        var currentUserId = CurrentUserId ?? 0;
        exp = exp.And(x => x.IsPublic == 0 || x.CreatedBy == currentUserId);

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ConfigurableCode != null && x.ConfigurableCode.Contains(keywords))
                || (x.ConfigurableName != null && x.ConfigurableName.Contains(keywords))
                || SqlFunc.ToString(x.ConfigurableDomain).Contains(keywords)
                || (x.ConfigurableSubCategory != null && x.ConfigurableSubCategory.Contains(keywords))
                || SqlFunc.ToString(x.DistinctRows).Contains(keywords)
                || SqlFunc.ToString(x.MaxExportRows).Contains(keywords)
                || SqlFunc.ToString(x.MaxQueryRows).Contains(keywords)
                || SqlFunc.ToString(x.IsPublic).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ConfigurableStatus).Contains(keywords)
                || (x.ConfigurableDescription != null && x.ConfigurableDescription.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurableCode))
        {
            exp = exp.And(x => x.ConfigurableCode != null && x.ConfigurableCode.Contains(queryDto.ConfigurableCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurableName))
        {
            exp = exp.And(x => x.ConfigurableName != null && x.ConfigurableName.Contains(queryDto.ConfigurableName));
        }

        if (queryDto?.ConfigurableDomain.HasValue == true)
        {
            exp = exp.And(x => x.ConfigurableDomain == queryDto.ConfigurableDomain);
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurableSubCategory))
        {
            exp = exp.And(x => x.ConfigurableSubCategory != null && x.ConfigurableSubCategory.Contains(queryDto.ConfigurableSubCategory));
        }

        if (queryDto?.DistinctRows.HasValue == true)
        {
            exp = exp.And(x => x.DistinctRows == queryDto.DistinctRows);
        }

        if (queryDto?.MaxExportRows.HasValue == true)
        {
            exp = exp.And(x => x.MaxExportRows == queryDto.MaxExportRows);
        }

        if (queryDto?.MaxQueryRows.HasValue == true)
        {
            exp = exp.And(x => x.MaxQueryRows == queryDto.MaxQueryRows);
        }

        if (queryDto?.IsPublic.HasValue == true)
        {
            exp = exp.And(x => x.IsPublic == queryDto.IsPublic);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ConfigurableStatus.HasValue == true)
        {
            exp = exp.And(x => x.ConfigurableStatus == queryDto.ConfigurableStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ConfigurableDescription))
        {
            exp = exp.And(x => x.ConfigurableDescription != null && x.ConfigurableDescription.Contains(queryDto.ConfigurableDescription));
        }


        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}

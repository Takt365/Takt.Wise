// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Bom;

/// <summary>
/// 工艺路线明细应用服务
/// </summary>
public class TaktRoutingItemService : TaktServiceBase, ITaktRoutingItemService
{
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItemArgument> _routingItemArgumentRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingItemRepository">工艺路线明细仓储</param>
    /// <param name="routingItemArgumentRepository">RoutingItemArgument仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingItemService(
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktCompanyRepository<TaktRoutingItemArgument> routingItemArgumentRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingItemRepository = routingItemRepository;
        _routingItemArgumentRepository = routingItemArgumentRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingItemDto>> GetRoutingItemListAsync(TaktRoutingItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktRoutingItemDto>.Create(
                new List<TaktRoutingItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingItemDto>.Create(
            data.Adapt<List<TaktRoutingItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto?> GetRoutingItemByIdAsync(long id)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktRoutingItemDto>();
        await FillRoutingItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工艺路线明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _routingItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.RoutingCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.RoutingCode,
            DictLabel = e.RoutingCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> CreateRoutingItemAsync(TaktRoutingItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktRoutingItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
            _routingItemRepository,
            x => x.RoutingId == entity.RoutingId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
        {
            throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _routingItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingId, maxSort);
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _routingItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.RoutingId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _routingItemRepository.CreateAsync(entity);
                await SaveRoutingItemChildrenAsync(entity, dto);
        return await GetRoutingItemByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingItemDto>();
    }

    /// <summary>
    /// 更新工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> UpdateRoutingItemAsync(long id, TaktRoutingItemUpdateDto dto)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
            _routingItemRepository,
            x => x.RoutingId == entity.RoutingId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
        {
            throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
        }
        await _routingItemRepository.UpdateAsync(entity);
                await SaveRoutingItemChildrenAsync(entity, dto);
        return await GetRoutingItemByIdAsync(id) ?? throw new TaktBusinessException("工艺路线明细不存在");
    }

    /// <summary>
    /// 删除工艺路线明细
    /// </summary>
    /// <param name="id">工艺路线明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemByIdAsync(long id)
    {
        var entity = await _routingItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在或已删除");
        }
        await _routingItemArgumentRepository.DeleteAsync(x => x.RoutingItemId == entity.Id);
        var deleted = await _routingItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工艺路线明细排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> UpdateRoutingItemSortAsync(TaktRoutingItemSortDto dto)
    {
        var entity = await _routingItemRepository.GetByIdAsync(dto.RoutingItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _routingItemRepository.UpdateAsync(entity);
        return await GetRoutingItemByIdAsync(dto.RoutingItemId) ?? throw new TaktBusinessException("工艺路线明细不存在");
    }

    /// <summary>
    /// 更新工艺路线明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingItemDto> UpdateRoutingItemObsoleteAsync(TaktRoutingItemObsoleteDto dto)
    {
        var entity = await _routingItemRepository.GetByIdAsync(dto.RoutingItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("工艺路线明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _routingItemRepository.UpdateAsync(entity);
        return await GetRoutingItemByIdAsync(dto.RoutingItemId) ?? throw new TaktBusinessException("工艺路线明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoutingItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoutingItemTemplateDto>(
            sheetName ?? "工艺路线明细导入模板",
            fileName ?? "工艺路线明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入工艺路线明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoutingItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoutingItemImportDto>(fileStream, sheetName ?? "工艺路线明细导入模板");
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
                var entity = rows[i].Adapt<TaktRoutingItem>();
                var importKey = $"{entity.RoutingId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（RoutingId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _routingItemRepository,
                    x => x.RoutingId == entity.RoutingId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
                {
                    throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _routingItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.RoutingId, maxSort);
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _routingItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingId == entity.RoutingId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.RoutingId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _routingItemRepository.CreateAsync(entity);
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
    /// 导出工艺路线明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingItemAsync(TaktRoutingItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktRoutingItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingItemExportDto>(),
                sheetName ?? "工艺路线明细数据",
                fileName ?? "工艺路线明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _routingItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingItemExportDto>(),
                sheetName ?? "工艺路线明细数据",
                fileName ?? "工艺路线明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线明细数据",
            fileName ?? "工艺路线明细导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工艺路线明细详情（加载 OneToMany 子表：工艺路线工序参数）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillRoutingItemDetailsAsync(TaktRoutingItemDto dto, TaktRoutingItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工艺路线工序参数 → dto.Arguments
        var arguments = await _routingItemArgumentRepository.GetListAsync(x => x.RoutingItemId == entity.Id);
        dto.Arguments = arguments.Adapt<List<TaktRoutingItemArgumentDto>>();
    }

    /// <summary>
    /// 保存工艺路线明细子表级联（工艺路线工序参数；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveRoutingItemChildrenAsync(TaktRoutingItem entity, TaktRoutingItemCreateDto dto)
    {
        // 工艺路线工序参数（Arguments）
        List<TaktRoutingItemArgumentUpdateDto>? argumentsForSave;
        if (dto is TaktRoutingItemUpdateDto updateDtoForArguments && updateDtoForArguments.Arguments != null)
        {
            argumentsForSave = updateDtoForArguments.Arguments;
        }
        else if (dto.Arguments != null)
        {
            argumentsForSave = dto.Arguments.Adapt<List<TaktRoutingItemArgumentUpdateDto>>();
        }
        else
        {
            argumentsForSave = null;
        }
        if (argumentsForSave is not { Count: > 0 })
        {
            await _routingItemArgumentRepository.DeleteAsync(x => x.RoutingItemId == entity.Id);
        }
        else
        {
            var existingList = await _routingItemArgumentRepository.GetListAsync(x => x.RoutingItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktRoutingItemArgument>();
            for (var i = 0; i < argumentsForSave.Count; i++)
            {
                var childDto = argumentsForSave[i];
                childDto.RoutingItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.RoutingItemArgumentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.RoutingItemArgumentId, out var target))
                    {
                        throw new TaktBusinessException("工艺路线工序参数不存在（RoutingItemArgumentId={childDto.RoutingItemArgumentId}）");
                    }
                    if (target.RoutingItemId != entity.Id)
                    {
                        throw new TaktBusinessException("工艺路线工序参数不属于当前主表（RoutingItemArgumentId={childDto.RoutingItemArgumentId}）");
                    }
                    submittedIds.Add(childDto.RoutingItemArgumentId);
                    childDto.Adapt(target);
                    target.Id = childDto.RoutingItemArgumentId;
                    target.RoutingItemId = entity.Id;
                    await _routingItemArgumentRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktRoutingItemArgument>();
                    child.Id = 0;
                    child.RoutingItemId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _routingItemArgumentRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _routingItemArgumentRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRoutingItem, bool>> QueryExpression(TaktRoutingItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRoutingItem>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.TimeUnit != null && x.TimeUnit.Contains(keywords))
                || (x.PointsUnit != null && x.PointsUnit.Contains(keywords))
                || (x.PointsToMinutesRate != null && x.PointsToMinutesRate.Contains(keywords))
                || (x.ProcessDescription != null && x.ProcessDescription.Contains(keywords))
                || (x.ExtJson != null && x.ExtJson.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (queryDto?.RoutingId.HasValue == true)
        {
            var routingId = queryDto.RoutingId.Value;
            exp = exp.And(x => x.RoutingId == routingId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingCode))
        {
            var routingCode = queryDto.RoutingCode;
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(routingCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BaseUnit))
        {
            var baseUnit = queryDto.BaseUnit;
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(baseUnit));
        }

        if (queryDto?.BaseQuantity.HasValue == true)
        {
            var baseQuantity = queryDto.BaseQuantity.Value;
            exp = exp.And(x => x.BaseQuantity == baseQuantity);
        }

        if (queryDto?.StandardMinutes.HasValue == true)
        {
            var standardMinutes = queryDto.StandardMinutes.Value;
            exp = exp.And(x => x.StandardMinutes == standardMinutes);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TimeUnit))
        {
            var timeUnit = queryDto.TimeUnit;
            exp = exp.And(x => x.TimeUnit != null && x.TimeUnit.Contains(timeUnit));
        }

        if (queryDto?.StandardShorts.HasValue == true)
        {
            var standardShorts = queryDto.StandardShorts.Value;
            exp = exp.And(x => x.StandardShorts == standardShorts);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PointsUnit))
        {
            var pointsUnit = queryDto.PointsUnit;
            exp = exp.And(x => x.PointsUnit != null && x.PointsUnit.Contains(pointsUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PointsToMinutesRate))
        {
            var pointsToMinutesRate = queryDto.PointsToMinutesRate;
            exp = exp.And(x => x.PointsToMinutesRate != null && x.PointsToMinutesRate.Contains(pointsToMinutesRate));
        }

        if (queryDto?.ConvertedMinutes.HasValue == true)
        {
            var convertedMinutes = queryDto.ConvertedMinutes.Value;
            exp = exp.And(x => x.ConvertedMinutes == convertedMinutes);
        }

        if (queryDto?.SetupMinutes.HasValue == true)
        {
            var setupMinutes = queryDto.SetupMinutes.Value;
            exp = exp.And(x => x.SetupMinutes == setupMinutes);
        }

        if (queryDto?.TeardownMinutes.HasValue == true)
        {
            var teardownMinutes = queryDto.TeardownMinutes.Value;
            exp = exp.And(x => x.TeardownMinutes == teardownMinutes);
        }

        if (queryDto?.IsInspection.HasValue == true)
        {
            var isInspection = queryDto.IsInspection.Value;
            exp = exp.And(x => x.IsInspection == isInspection);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProcessDescription))
        {
            var processDescription = queryDto.ProcessDescription;
            exp = exp.And(x => x.ProcessDescription != null && x.ProcessDescription.Contains(processDescription));
        }

        if (queryDto?.ProcessSegmentType.HasValue == true)
        {
            var processSegmentType = queryDto.ProcessSegmentType.Value;
            exp = exp.And(x => x.ProcessSegmentType == processSegmentType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtJson))
        {
            var extJson = queryDto.ExtJson;
            exp = exp.And(x => x.ExtJson != null && x.ExtJson.Contains(extJson));
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
    private static bool HasAnyListQueryFilter(TaktRoutingItemQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (queryDto.RoutingId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (queryDto.BaseQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.StandardMinutes.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TimeUnit))
        {
            return true;
        }
        if (queryDto.StandardShorts.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PointsUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PointsToMinutesRate))
        {
            return true;
        }
        if (queryDto.ConvertedMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.SetupMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.TeardownMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.IsInspection.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProcessDescription))
        {
            return true;
        }
        if (queryDto.ProcessSegmentType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtJson))
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
        if (queryDto.IsObsolete.HasValue)
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Bom
// 文件名称：TaktRoutingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工艺路线主应用服务实现
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
/// 工艺路线主应用服务
/// </summary>
public class TaktRoutingService : TaktServiceBase, ITaktRoutingService
{
    private readonly ITaktApprovalRepository<TaktRouting> _routingRepository;
    private readonly ITaktCompanyRepository<TaktRoutingItem> _routingItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="routingRepository">工艺路线主仓储</param>
    /// <param name="routingItemRepository">RoutingItem仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktRoutingService(
        ITaktApprovalRepository<TaktRouting> routingRepository,
        ITaktCompanyRepository<TaktRoutingItem> routingItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _routingRepository = routingRepository;
        _routingItemRepository = routingItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工艺路线主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktRoutingDto>> GetRoutingListAsync(TaktRoutingQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktRoutingDto>.Create(
                new List<TaktRoutingDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _routingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktRoutingDto>.Create(
            data.Adapt<List<TaktRoutingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto?> GetRoutingByIdAsync(long id)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktRoutingDto>();
        await FillRoutingDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工艺路线主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetRoutingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _routingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RoutingStatus == 1,
            x => x.RoutingName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.RoutingCode,
            DictLabel = e.RoutingName ?? e.RoutingCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工艺路线主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> CreateRoutingAsync(TaktRoutingCreateDto dto)
    {
        var entity = dto.Adapt<TaktRouting>();
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
            _routingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.RoutingCode == entity.RoutingCode);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
        {
            throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
        }
        entity = await _routingRepository.CreateAsync(entity);
                await SaveRoutingChildrenAsync(entity, dto);
        return await GetRoutingByIdAsync(entity.Id) ?? entity.Adapt<TaktRoutingDto>();
    }

    /// <summary>
    /// 更新工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> UpdateRoutingAsync(long id, TaktRoutingUpdateDto dto)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
            _routingRepository,
            x => x.PlantCode == entity.PlantCode
                && x.RoutingCode == entity.RoutingCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
        {
            throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
        }
        await _routingRepository.UpdateAsync(entity);
                await SaveRoutingChildrenAsync(entity, dto);
        return await GetRoutingByIdAsync(id) ?? throw new TaktBusinessException("工艺路线主不存在");
    }

    /// <summary>
    /// 删除工艺路线主
    /// </summary>
    /// <param name="id">工艺路线主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingByIdAsync(long id)
    {
        var entity = await _routingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在或已删除");
        }
        await _routingItemRepository.DeleteAsync(x => x.RoutingId == entity.Id);
        var deleted = await _routingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工艺路线主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工艺路线主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteRoutingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteRoutingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工艺路线主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktRoutingDto> UpdateRoutingStatusAsync(TaktRoutingStatusDto dto)
    {
        var entity = await _routingRepository.GetByIdAsync(dto.RoutingId);
        if (entity == null)
        {
            throw new TaktBusinessException("工艺路线主不存在");
        }
        entity.RoutingStatus = dto.RoutingStatus;
        await _routingRepository.UpdateAsync(entity);
        return await GetRoutingByIdAsync(dto.RoutingId) ?? throw new TaktBusinessException("工艺路线主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetRoutingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktRoutingTemplateDto>(
            sheetName ?? "工艺路线主导入模板",
            fileName ?? "工艺路线主导入模板.xlsx");
    }

    /// <summary>
    /// 导入工艺路线主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportRoutingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktRoutingImportDto>(fileStream, sheetName ?? "工艺路线主导入模板");
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
                var entity = rows[i].Adapt<TaktRouting>();
                var importKey = $"{entity.PlantCode}|{entity.RoutingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、RoutingCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique = await _uniqueValidator.IsUniqueAsync(
                    _routingRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.RoutingCode == entity.RoutingCode);
                if (!isUnique_ix_takt_logistics_manufacturing_bom_routing_plant_code_version_unique)
                {
                    throw new TaktBusinessException("工艺路线主的PlantCode、RoutingCode已存在");
                }
                await _routingRepository.CreateAsync(entity);
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
    /// 导出工艺路线主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportRoutingAsync(TaktRoutingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktRoutingQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingExportDto>(),
                sheetName ?? "工艺路线主数据",
                fileName ?? "工艺路线主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _routingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktRoutingExportDto>(),
                sheetName ?? "工艺路线主数据",
                fileName ?? "工艺路线主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktRoutingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工艺路线主数据",
            fileName ?? "工艺路线主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废工艺路线明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="routingId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkRoutingItemsObsoleteAsync(long routingId)
    {
        if (routingId <= 0)
        {
            return;
        }
        var rows = await _routingItemRepository.GetListAsync(
            x => x.RoutingId == routingId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _routingItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充工艺路线主详情（加载 OneToMany 子表：工艺路线明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillRoutingDetailsAsync(TaktRoutingDto dto, TaktRouting entity)
    {
        if (dto == null)
        {
            return;
        }
        // 工艺路线明细 → dto.Items（含作废行）
        var items = await _routingItemRepository.GetListAsync(x => x.RoutingId == entity.Id);
        dto.Items = items.Adapt<List<TaktRoutingItemDto>>();
    }

    /// <summary>
    /// 保存工艺路线主子表级联（工艺路线明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveRoutingChildrenAsync(TaktRouting entity, TaktRoutingCreateDto dto)
    {
        // 工艺路线明细（Items）
        List<TaktRoutingItemUpdateDto>? itemsForSave;
        if (dto is TaktRoutingUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktRoutingItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkRoutingItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _routingItemRepository.GetListAsync(x => x.RoutingId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktRoutingItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.RoutingId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.RoutingCode = entity.RoutingCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("工艺路线明细第{i + 1}项与本次提交的其他项重复（CompanyCode、RoutingId、LineNumber）");
                }
                if (childDto.RoutingItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.RoutingItemId, out var target))
                    {
                        throw new TaktBusinessException("工艺路线明细不存在（RoutingItemId={childDto.RoutingItemId}）");
                    }
                    if (target.RoutingId != entity.Id)
                    {
                        throw new TaktBusinessException("工艺路线明细不属于当前主表（RoutingItemId={childDto.RoutingItemId}）");
                    }
                    submittedIds.Add(childDto.RoutingItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _routingItemRepository,
                        x => x.RoutingId == x.RoutingId
                && x.LineNumber == x.LineNumber,
                        childDto.RoutingItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
                    {
                        throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.RoutingItemId;
                    target.RoutingId = entity.Id;
                    target.IsObsolete = 0;
                    await _routingItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _routingItemRepository,
                        x => x.RoutingId == x.RoutingId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_bom_routing_item_routing_line_unique)
                    {
                        throw new TaktBusinessException("工艺路线明细的RoutingId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktRoutingItem>();
                    child.Id = 0;
                    child.RoutingId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _routingItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.RoutingCode) ? entity.RoutingCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
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
                await _routingItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工艺路线主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktRouting, bool>> QueryExpression(TaktRoutingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktRouting>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkCenter != null && x.WorkCenter.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
                || (x.RoutingGroupCode != null && x.RoutingGroupCode.Contains(keywords))
                || (x.RoutingGroupName != null && x.RoutingGroupName.Contains(keywords))
                || (x.RoutingName != null && x.RoutingName.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.Version != null && x.Version.Contains(keywords))
                || (x.RoutingDescription != null && x.RoutingDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenter))
        {
            var workCenter = queryDto.WorkCenter;
            exp = exp.And(x => x.WorkCenter != null && x.WorkCenter.Contains(workCenter));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingCode))
        {
            var routingCode = queryDto.RoutingCode;
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(routingCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingGroupCode))
        {
            var routingGroupCode = queryDto.RoutingGroupCode;
            exp = exp.And(x => x.RoutingGroupCode == routingGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingGroupName))
        {
            var routingGroupName = queryDto.RoutingGroupName;
            exp = exp.And(x => x.RoutingGroupName != null && x.RoutingGroupName.Contains(routingGroupName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingName))
        {
            var routingName = queryDto.RoutingName;
            exp = exp.And(x => x.RoutingName != null && x.RoutingName.Contains(routingName));
        }

        if (queryDto?.Purpose.HasValue == true)
        {
            var purpose = queryDto.Purpose.Value;
            exp = exp.And(x => x.Purpose == purpose);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Version))
        {
            var version = queryDto.Version;
            exp = exp.And(x => x.Version != null && x.Version.Contains(version));
        }

        if (queryDto?.RoutingStatus.HasValue == true)
        {
            var routingStatus = queryDto.RoutingStatus.Value;
            exp = exp.And(x => x.RoutingStatus == routingStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingDescription))
        {
            var routingDescription = queryDto.RoutingDescription;
            exp = exp.And(x => x.RoutingDescription != null && x.RoutingDescription.Contains(routingDescription));
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

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            var effectiveDateStart = queryDto.EffectiveDateStart.Value;
            exp = exp.And(x => x.EffectiveDate >= effectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            var effectiveDateEnd = queryDto.EffectiveDateEnd.Value;
            exp = exp.And(x => x.EffectiveDate <= effectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            var expiryDateStart = queryDto.ExpiryDateStart.Value;
            exp = exp.And(x => x.ExpiryDate >= expiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            var expiryDateEnd = queryDto.ExpiryDateEnd.Value;
            exp = exp.And(x => x.ExpiryDate <= expiryDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktRoutingQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenter))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingGroupName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingName))
        {
            return true;
        }
        if (queryDto.Purpose.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Version))
        {
            return true;
        }
        if (queryDto.RoutingStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingDescription))
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
        if (queryDto.EffectiveDateStart.HasValue || queryDto.EffectiveDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpiryDateStart.HasValue || queryDto.ExpiryDateEnd.HasValue)
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

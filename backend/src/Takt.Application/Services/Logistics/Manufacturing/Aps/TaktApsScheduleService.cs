// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程主应用服务
/// </summary>
public class TaktApsScheduleService : TaktServiceBase, ITaktApsScheduleService
{
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktCompanyRepository<TaktApsOrder> _apsOrderRepository;
    private readonly ITaktCompanyRepository<TaktApsScheduleItem> _apsScheduleItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleRepository">APS排程主仓储</param>
    /// <param name="apsOrderRepository">ApsOrder仓储</param>
    /// <param name="apsScheduleItemRepository">ApsScheduleItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsScheduleService(
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktCompanyRepository<TaktApsOrder> apsOrderRepository,
        ITaktCompanyRepository<TaktApsScheduleItem> apsScheduleItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsScheduleRepository = apsScheduleRepository;
        _apsOrderRepository = apsOrderRepository;
        _apsScheduleItemRepository = apsScheduleItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsScheduleDto>> GetApsScheduleListAsync(TaktApsScheduleQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktApsScheduleDto>.Create(
                new List<TaktApsScheduleDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsScheduleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsScheduleDto>.Create(
            data.Adapt<List<TaktApsScheduleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto?> GetApsScheduleByIdAsync(long id)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktApsScheduleDto>();
        await FillApsScheduleDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取APS排程主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsScheduleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _apsScheduleRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ScheduleStatus == 1,
            x => x.ScheduleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialRequirementsPlanningCode ?? string.Empty,
            DictLabel = e.ScheduleName ?? e.MaterialRequirementsPlanningCode ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> CreateApsScheduleAsync(TaktApsScheduleCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsSchedule>();
        var isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ScheduleCode == entity.ScheduleCode);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique)
        {
            throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
        }
        entity = await _apsScheduleRepository.CreateAsync(entity);
                await SaveApsScheduleChildrenAsync(entity, dto);
        return await GetApsScheduleByIdAsync(entity.Id) ?? entity.Adapt<TaktApsScheduleDto>();
    }

    /// <summary>
    /// 更新APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> UpdateApsScheduleAsync(long id, TaktApsScheduleUpdateDto dto)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ScheduleCode == entity.ScheduleCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique)
        {
            throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
        }
        await _apsScheduleRepository.UpdateAsync(entity);
                await SaveApsScheduleChildrenAsync(entity, dto);
        return await GetApsScheduleByIdAsync(id) ?? throw new TaktBusinessException("APS排程主不存在");
    }

    /// <summary>
    /// 删除APS排程主
    /// </summary>
    /// <param name="id">APS排程主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleByIdAsync(long id)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在或已删除");
        }
        await _apsOrderRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        await _apsScheduleItemRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        var deleted = await _apsScheduleRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsScheduleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleDto> UpdateApsScheduleStatusAsync(TaktApsScheduleStatusDto dto)
    {
        var entity = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        entity.ScheduleStatus = dto.ScheduleStatus;
        await _apsScheduleRepository.UpdateAsync(entity);
        return await GetApsScheduleByIdAsync(dto.ApsScheduleId) ?? throw new TaktBusinessException("APS排程主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsScheduleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsScheduleTemplateDto>(
            sheetName ?? "APS排程主导入模板",
            fileName ?? "APS排程主导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsScheduleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsScheduleImportDto>(fileStream, sheetName ?? "APS排程主导入模板");
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
                var entity = rows[i].Adapt<TaktApsSchedule>();
                var importKey = $"{entity.PlantCode}|{entity.ScheduleCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ScheduleCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsScheduleRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ScheduleCode == entity.ScheduleCode);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_plant_code_unique)
                {
                    throw new TaktBusinessException("APS排程主的PlantCode、ScheduleCode已存在");
                }
                await _apsScheduleRepository.CreateAsync(entity);
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
    /// 导出APS排程主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsScheduleAsync(TaktApsScheduleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktApsScheduleQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleExportDto>(),
                sheetName ?? "APS排程主数据",
                fileName ?? "APS排程主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _apsScheduleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleExportDto>(),
                sheetName ?? "APS排程主数据",
                fileName ?? "APS排程主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsScheduleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程主数据",
            fileName ?? "APS排程主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废APS排程明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="apsScheduleId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkApsScheduleItemsObsoleteAsync(long apsScheduleId)
    {
        if (apsScheduleId <= 0)
        {
            return;
        }
        var rows = await _apsScheduleItemRepository.GetListAsync(
            x => x.ApsScheduleId == apsScheduleId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _apsScheduleItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充APS排程主详情（加载 OneToMany 子表：APS排程订单、APS排程明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillApsScheduleDetailsAsync(TaktApsScheduleDto dto, TaktApsSchedule entity)
    {
        if (dto == null)
        {
            return;
        }
        // APS排程订单 → dto.Orders
        var orders = await _apsOrderRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
        dto.Orders = orders.Adapt<List<TaktApsOrderDto>>();
        // APS排程明细 → dto.Items（含作废行）
        var items = await _apsScheduleItemRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
        dto.Items = items.Adapt<List<TaktApsScheduleItemDto>>();
    }

    /// <summary>
    /// 保存APS排程主子表级联（APS排程订单、APS排程明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveApsScheduleChildrenAsync(TaktApsSchedule entity, TaktApsScheduleCreateDto dto)
    {
        // APS排程订单（Orders）
        List<TaktApsOrderUpdateDto>? ordersForSave;
        if (dto is TaktApsScheduleUpdateDto updateDtoForOrders && updateDtoForOrders.Orders != null)
        {
            ordersForSave = updateDtoForOrders.Orders;
        }
        else if (dto.Orders != null)
        {
            ordersForSave = dto.Orders.Adapt<List<TaktApsOrderUpdateDto>>();
        }
        else
        {
            ordersForSave = null;
        }
        if (ordersForSave is not { Count: > 0 })
        {
            await _apsOrderRepository.DeleteAsync(x => x.ApsScheduleId == entity.Id);
        }
        else
        {
            var existingList = await _apsOrderRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktApsOrder>();
            for (var i = 0; i < ordersForSave.Count; i++)
            {
                var childDto = ordersForSave[i];
                childDto.ApsScheduleId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.ApsOrderId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ApsOrderId, out var target))
                    {
                        throw new TaktBusinessException("APS排程订单不存在（ApsOrderId={childDto.ApsOrderId}）");
                    }
                    if (target.ApsScheduleId != entity.Id)
                    {
                        throw new TaktBusinessException("APS排程订单不属于当前主表（ApsOrderId={childDto.ApsOrderId}）");
                    }
                    submittedIds.Add(childDto.ApsOrderId);
                    childDto.Adapt(target);
                    target.Id = childDto.ApsOrderId;
                    target.ApsScheduleId = entity.Id;
                    await _apsOrderRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktApsOrder>();
                    child.Id = 0;
                    child.ApsScheduleId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _apsOrderRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _apsOrderRepository.CreateRangeAsync(toCreate);
            }
        }
        // APS排程明细（Items）
        List<TaktApsScheduleItemUpdateDto>? itemsForSave;
        if (dto is TaktApsScheduleUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktApsScheduleItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkApsScheduleItemsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _apsScheduleItemRepository.GetListAsync(x => x.ApsScheduleId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktApsScheduleItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.ApsScheduleId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("APS排程明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ApsScheduleId、LineNumber）");
                }
                if (childDto.ApsScheduleItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ApsScheduleItemId, out var target))
                    {
                        throw new TaktBusinessException("APS排程明细不存在（ApsScheduleItemId={childDto.ApsScheduleItemId}）");
                    }
                    if (target.ApsScheduleId != entity.Id)
                    {
                        throw new TaktBusinessException("APS排程明细不属于当前主表（ApsScheduleItemId={childDto.ApsScheduleItemId}）");
                    }
                    submittedIds.Add(childDto.ApsScheduleItemId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _apsScheduleItemRepository,
                        x => x.ApsScheduleId == x.ApsScheduleId
                && x.LineNumber == x.LineNumber,
                        childDto.ApsScheduleItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique)
                    {
                        throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.ApsScheduleItemId;
                    target.ApsScheduleId = entity.Id;
                    target.IsObsolete = 0;
                    await _apsScheduleItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _apsScheduleItemRepository,
                        x => x.ApsScheduleId == x.ApsScheduleId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique)
                    {
                        throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktApsScheduleItem>();
                    child.Id = 0;
                    child.ApsScheduleId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _apsScheduleItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MaterialRequirementsPlanningCode) ? entity.MaterialRequirementsPlanningCode : entity.Id.ToString();
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
                await _apsScheduleItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsSchedule, bool>> QueryExpression(TaktApsScheduleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsSchedule>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(keywords))
                || (x.ScheduleCode != null && x.ScheduleCode.Contains(keywords))
                || (x.ScheduleName != null && x.ScheduleName.Contains(keywords))
                || (x.WorkshopCode != null && x.WorkshopCode.Contains(keywords))
                || (x.WorkshopName != null && x.WorkshopName.Contains(keywords))
                || (x.ProductionLineCode != null && x.ProductionLineCode.Contains(keywords))
                || (x.ProductionLineName != null && x.ProductionLineName.Contains(keywords))
                || (x.PlannerGroupName != null && x.PlannerGroupName.Contains(keywords))
                || (x.PublishUserName != null && x.PublishUserName.Contains(keywords))
                || (x.ScheduleDescription != null && x.ScheduleDescription.Contains(keywords))
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

        if (queryDto?.MaterialRequirementsPlanningId.HasValue == true)
        {
            var materialRequirementsPlanningId = queryDto.MaterialRequirementsPlanningId.Value;
            exp = exp.And(x => x.MaterialRequirementsPlanningId == materialRequirementsPlanningId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialRequirementsPlanningCode))
        {
            var materialRequirementsPlanningCode = queryDto.MaterialRequirementsPlanningCode;
            exp = exp.And(x => x.MaterialRequirementsPlanningCode != null && x.MaterialRequirementsPlanningCode.Contains(materialRequirementsPlanningCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScheduleCode))
        {
            var scheduleCode = queryDto.ScheduleCode;
            exp = exp.And(x => x.ScheduleCode != null && x.ScheduleCode.Contains(scheduleCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScheduleName))
        {
            var scheduleName = queryDto.ScheduleName;
            exp = exp.And(x => x.ScheduleName != null && x.ScheduleName.Contains(scheduleName));
        }

        if (queryDto?.ScheduleType.HasValue == true)
        {
            var scheduleType = queryDto.ScheduleType.Value;
            exp = exp.And(x => x.ScheduleType == scheduleType);
        }

        if (queryDto?.PlanCycle.HasValue == true)
        {
            var planCycle = queryDto.PlanCycle.Value;
            exp = exp.And(x => x.PlanCycle == planCycle);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkshopCode))
        {
            var workshopCode = queryDto.WorkshopCode;
            exp = exp.And(x => x.WorkshopCode != null && x.WorkshopCode.Contains(workshopCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkshopName))
        {
            var workshopName = queryDto.WorkshopName;
            exp = exp.And(x => x.WorkshopName != null && x.WorkshopName.Contains(workshopName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionLineCode))
        {
            var productionLineCode = queryDto.ProductionLineCode;
            exp = exp.And(x => x.ProductionLineCode != null && x.ProductionLineCode.Contains(productionLineCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionLineName))
        {
            var productionLineName = queryDto.ProductionLineName;
            exp = exp.And(x => x.ProductionLineName != null && x.ProductionLineName.Contains(productionLineName));
        }

        if (queryDto?.ScheduleStrategy.HasValue == true)
        {
            var scheduleStrategy = queryDto.ScheduleStrategy.Value;
            exp = exp.And(x => x.ScheduleStrategy == scheduleStrategy);
        }

        if (queryDto?.ScheduleAlgorithm.HasValue == true)
        {
            var scheduleAlgorithm = queryDto.ScheduleAlgorithm.Value;
            exp = exp.And(x => x.ScheduleAlgorithm == scheduleAlgorithm);
        }

        if (queryDto?.OptimizationObjective.HasValue == true)
        {
            var optimizationObjective = queryDto.OptimizationObjective.Value;
            exp = exp.And(x => x.OptimizationObjective == optimizationObjective);
        }

        if (queryDto?.ScheduleStatus.HasValue == true)
        {
            var scheduleStatus = queryDto.ScheduleStatus.Value;
            exp = exp.And(x => x.ScheduleStatus == scheduleStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupCode))
        {
            var plannerGroupCode = queryDto.PlannerGroupCode;
            exp = exp.And(x => x.PlannerGroupCode == plannerGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupName))
        {
            var plannerGroupName = queryDto.PlannerGroupName;
            exp = exp.And(x => x.PlannerGroupName != null && x.PlannerGroupName.Contains(plannerGroupName));
        }

        if (queryDto?.PublishUserId.HasValue == true)
        {
            var publishUserId = queryDto.PublishUserId.Value;
            exp = exp.And(x => x.PublishUserId == publishUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PublishUserName))
        {
            var publishUserName = queryDto.PublishUserName;
            exp = exp.And(x => x.PublishUserName != null && x.PublishUserName.Contains(publishUserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScheduleDescription))
        {
            var scheduleDescription = queryDto.ScheduleDescription;
            exp = exp.And(x => x.ScheduleDescription != null && x.ScheduleDescription.Contains(scheduleDescription));
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

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            var planDateStart = queryDto.PlanDateStart.Value;
            exp = exp.And(x => x.PlanDate >= planDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            var planDateEnd = queryDto.PlanDateEnd.Value;
            exp = exp.And(x => x.PlanDate <= planDateEnd);
        }

        if (queryDto?.PlanStartTimeStart.HasValue == true)
        {
            var planStartTimeStart = queryDto.PlanStartTimeStart.Value;
            exp = exp.And(x => x.PlanStartTime >= planStartTimeStart);
        }

        if (queryDto?.PlanStartTimeEnd.HasValue == true)
        {
            var planStartTimeEnd = queryDto.PlanStartTimeEnd.Value;
            exp = exp.And(x => x.PlanStartTime <= planStartTimeEnd);
        }

        if (queryDto?.PlanEndTimeStart.HasValue == true)
        {
            var planEndTimeStart = queryDto.PlanEndTimeStart.Value;
            exp = exp.And(x => x.PlanEndTime >= planEndTimeStart);
        }

        if (queryDto?.PlanEndTimeEnd.HasValue == true)
        {
            var planEndTimeEnd = queryDto.PlanEndTimeEnd.Value;
            exp = exp.And(x => x.PlanEndTime <= planEndTimeEnd);
        }

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            var publishTimeStart = queryDto.PublishTimeStart.Value;
            exp = exp.And(x => x.PublishTime >= publishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            var publishTimeEnd = queryDto.PublishTimeEnd.Value;
            exp = exp.And(x => x.PublishTime <= publishTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktApsScheduleQueryDto? queryDto)
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
        if (queryDto.MaterialRequirementsPlanningId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialRequirementsPlanningCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScheduleCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScheduleName))
        {
            return true;
        }
        if (queryDto.ScheduleType.HasValue)
        {
            return true;
        }
        if (queryDto.PlanCycle.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkshopCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkshopName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionLineCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionLineName))
        {
            return true;
        }
        if (queryDto.ScheduleStrategy.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduleAlgorithm.HasValue)
        {
            return true;
        }
        if (queryDto.OptimizationObjective.HasValue)
        {
            return true;
        }
        if (queryDto.ScheduleStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupName))
        {
            return true;
        }
        if (queryDto.PublishUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PublishUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScheduleDescription))
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
        if (queryDto.PlanDateStart.HasValue || queryDto.PlanDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanStartTimeStart.HasValue || queryDto.PlanStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanEndTimeStart.HasValue || queryDto.PlanEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PublishTimeStart.HasValue || queryDto.PublishTimeEnd.HasValue)
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

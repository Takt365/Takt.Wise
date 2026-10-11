// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktEquipmentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂设备应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Maintenance;
using Takt.Domain.Entities.Logistics.Maintenance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Maintenance;

/// <summary>
/// 工厂设备应用服务
/// </summary>
public class TaktEquipmentService : TaktServiceBase, ITaktEquipmentService
{
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceNotification> _maintenanceNotificationRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktCompanyRepository<TaktMaintenanceHistory> _maintenanceHistoryRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceNotificationRepository">MaintenanceNotification仓储</param>
    /// <param name="maintenanceWorkOrderRepository">MaintenanceWorkOrder仓储</param>
    /// <param name="maintenanceHistoryRepository">MaintenanceHistory仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEquipmentService(
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceNotification> maintenanceNotificationRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktCompanyRepository<TaktMaintenanceHistory> maintenanceHistoryRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _equipmentRepository = equipmentRepository;
        _maintenanceNotificationRepository = maintenanceNotificationRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _maintenanceHistoryRepository = maintenanceHistoryRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂设备列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEquipmentDto>> GetEquipmentListAsync(TaktEquipmentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEquipmentDto>.Create(
                new List<TaktEquipmentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _equipmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEquipmentDto>.Create(
            data.Adapt<List<TaktEquipmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto?> GetEquipmentByIdAsync(long id)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktEquipmentDto>();
        await FillEquipmentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取工厂设备选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEquipmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _equipmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WarrantyStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EquipCode,
            DictLabel = e.EquipmentName ?? e.EquipCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工厂设备
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> CreateEquipmentAsync(TaktEquipmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktEquipment>();
        var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipCode == entity.EquipCode);
        if (!isUnique_ix_equipment_code_unique)
        {
            throw new TaktBusinessException("工厂设备的PlantCode、EquipCode已存在");
        }
        entity = await _equipmentRepository.CreateAsync(entity);
                await SaveEquipmentChildrenAsync(entity, dto);
        return await GetEquipmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEquipmentDto>();
    }

    /// <summary>
    /// 更新工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> UpdateEquipmentAsync(long id, TaktEquipmentUpdateDto dto)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipCode == entity.EquipCode,
            id);
        if (!isUnique_ix_equipment_code_unique)
        {
            throw new TaktBusinessException("工厂设备的PlantCode、EquipCode已存在");
        }
        await _equipmentRepository.UpdateAsync(entity);
                await SaveEquipmentChildrenAsync(entity, dto);
        return await GetEquipmentByIdAsync(id) ?? throw new TaktBusinessException("工厂设备不存在");
    }

    /// <summary>
    /// 删除工厂设备
    /// </summary>
    /// <param name="id">工厂设备ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentByIdAsync(long id)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在或已删除");
        }
        await _maintenanceNotificationRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        await _maintenanceWorkOrderRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        await _maintenanceHistoryRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        var deleted = await _equipmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂设备不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂设备
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEquipmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工厂设备状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentDto> UpdateEquipmentStatusAsync(TaktEquipmentStatusDto dto)
    {
        var entity = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        entity.WarrantyStatus = dto.WarrantyStatus;
        await _equipmentRepository.UpdateAsync(entity);
        return await GetEquipmentByIdAsync(dto.EquipmentId) ?? throw new TaktBusinessException("工厂设备不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEquipmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEquipmentTemplateDto>(
            sheetName ?? "工厂设备导入模板",
            fileName ?? "工厂设备导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂设备
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEquipmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEquipmentImportDto>(fileStream, sheetName ?? "工厂设备导入模板");
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
                var entity = rows[i].Adapt<TaktEquipment>();
                var importKey = $"{entity.PlantCode}|{entity.EquipCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EquipCode）");
                }
                var isUnique_ix_equipment_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _equipmentRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EquipCode == entity.EquipCode);
                if (!isUnique_ix_equipment_code_unique)
                {
                    throw new TaktBusinessException("工厂设备的PlantCode、EquipCode已存在");
                }
                await _equipmentRepository.CreateAsync(entity);
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
    /// 导出工厂设备
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEquipmentAsync(TaktEquipmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEquipmentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEquipmentExportDto>(),
                sheetName ?? "工厂设备数据",
                fileName ?? "工厂设备导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _equipmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEquipmentExportDto>(),
                sheetName ?? "工厂设备数据",
                fileName ?? "工厂设备导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEquipmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂设备数据",
            fileName ?? "工厂设备导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取设备统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设备统计</returns>
    public async Task<TaktEquipmentStatDto> GetEquipmentStatAsync(TaktEquipmentStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktEquipment, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        var monthEquipmentCount = await _equipmentRepository.CountAsync(predicate);
        return new TaktEquipmentStatDto
        {
            StatMonth = statMonth,
            MonthEquipmentCount = monthEquipmentCount,
        };
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充工厂设备详情（加载 OneToMany 子表：维护通知单、维护工单、设备维护履历）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillEquipmentDetailsAsync(TaktEquipmentDto dto, TaktEquipment entity)
    {
        if (dto == null)
        {
            return;
        }
        // 维护通知单 → dto.MaintenanceNotifications
        var maintenancenotifications = await _maintenanceNotificationRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceNotifications = maintenancenotifications.Adapt<List<TaktMaintenanceNotificationDto>>();
        // 维护工单 → dto.MaintenanceWorkOrders
        var maintenanceworkorders = await _maintenanceWorkOrderRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceWorkOrders = maintenanceworkorders.Adapt<List<TaktMaintenanceWorkOrderDto>>();
        // 设备维护履历 → dto.MaintenanceHistories
        var maintenancehistories = await _maintenanceHistoryRepository.GetListAsync(x => x.EquipmentId == entity.Id);
        dto.MaintenanceHistories = maintenancehistories.Adapt<List<TaktMaintenanceHistoryDto>>();
    }

    /// <summary>
    /// 保存工厂设备子表级联（维护通知单、维护工单、设备维护履历；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveEquipmentChildrenAsync(TaktEquipment entity, TaktEquipmentCreateDto dto)
    {
        // 维护通知单（MaintenanceNotifications）
        List<TaktMaintenanceNotificationUpdateDto>? maintenanceNotificationsForSave;
        if (dto is TaktEquipmentUpdateDto updateDtoForMaintenanceNotifications && updateDtoForMaintenanceNotifications.MaintenanceNotifications != null)
        {
            maintenanceNotificationsForSave = updateDtoForMaintenanceNotifications.MaintenanceNotifications;
        }
        else if (dto.MaintenanceNotifications != null)
        {
            maintenanceNotificationsForSave = dto.MaintenanceNotifications.Adapt<List<TaktMaintenanceNotificationUpdateDto>>();
        }
        else
        {
            maintenanceNotificationsForSave = null;
        }
        if (maintenanceNotificationsForSave is not { Count: > 0 })
        {
            await _maintenanceNotificationRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var existingList = await _maintenanceNotificationRepository.GetListAsync(x => x.EquipmentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaintenanceNotification>();
            for (var i = 0; i < maintenanceNotificationsForSave.Count; i++)
            {
                var childDto = maintenanceNotificationsForSave[i];
                childDto.EquipmentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EquipCode = entity.EquipCode;
                childDto.EquipmentName = entity.EquipmentName;
                if (childDto.MaintenanceNotificationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaintenanceNotificationId, out var target))
                    {
                        throw new TaktBusinessException("维护通知单不存在（MaintenanceNotificationId={childDto.MaintenanceNotificationId}）");
                    }
                    if (target.EquipmentId != entity.Id)
                    {
                        throw new TaktBusinessException("维护通知单不属于当前主表（MaintenanceNotificationId={childDto.MaintenanceNotificationId}）");
                    }
                    submittedIds.Add(childDto.MaintenanceNotificationId);
                    childDto.Adapt(target);
                    target.Id = childDto.MaintenanceNotificationId;
                    target.EquipmentId = entity.Id;
                    await _maintenanceNotificationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktMaintenanceNotification>();
                    child.Id = 0;
                    child.EquipmentId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _maintenanceNotificationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _maintenanceNotificationRepository.CreateRangeAsync(toCreate);
            }
        }
        // 维护工单（MaintenanceWorkOrders）
        List<TaktMaintenanceWorkOrderUpdateDto>? maintenanceWorkOrdersForSave;
        if (dto is TaktEquipmentUpdateDto updateDtoForMaintenanceWorkOrders && updateDtoForMaintenanceWorkOrders.MaintenanceWorkOrders != null)
        {
            maintenanceWorkOrdersForSave = updateDtoForMaintenanceWorkOrders.MaintenanceWorkOrders;
        }
        else if (dto.MaintenanceWorkOrders != null)
        {
            maintenanceWorkOrdersForSave = dto.MaintenanceWorkOrders.Adapt<List<TaktMaintenanceWorkOrderUpdateDto>>();
        }
        else
        {
            maintenanceWorkOrdersForSave = null;
        }
        if (maintenanceWorkOrdersForSave is not { Count: > 0 })
        {
            await _maintenanceWorkOrderRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var existingList = await _maintenanceWorkOrderRepository.GetListAsync(x => x.EquipmentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaintenanceWorkOrder>();
            for (var i = 0; i < maintenanceWorkOrdersForSave.Count; i++)
            {
                var childDto = maintenanceWorkOrdersForSave[i];
                childDto.EquipmentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EquipCode = entity.EquipCode;
                childDto.EquipmentName = entity.EquipmentName;
                if (childDto.MaintenanceWorkOrderId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaintenanceWorkOrderId, out var target))
                    {
                        throw new TaktBusinessException("维护工单不存在（MaintenanceWorkOrderId={childDto.MaintenanceWorkOrderId}）");
                    }
                    if (target.EquipmentId != entity.Id)
                    {
                        throw new TaktBusinessException("维护工单不属于当前主表（MaintenanceWorkOrderId={childDto.MaintenanceWorkOrderId}）");
                    }
                    submittedIds.Add(childDto.MaintenanceWorkOrderId);
                    childDto.Adapt(target);
                    target.Id = childDto.MaintenanceWorkOrderId;
                    target.EquipmentId = entity.Id;
                    await _maintenanceWorkOrderRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktMaintenanceWorkOrder>();
                    child.Id = 0;
                    child.EquipmentId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _maintenanceWorkOrderRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _maintenanceWorkOrderRepository.CreateRangeAsync(toCreate);
            }
        }
        // 设备维护履历（MaintenanceHistories）
        List<TaktMaintenanceHistoryUpdateDto>? maintenanceHistoriesForSave;
        if (dto is TaktEquipmentUpdateDto updateDtoForMaintenanceHistories && updateDtoForMaintenanceHistories.MaintenanceHistories != null)
        {
            maintenanceHistoriesForSave = updateDtoForMaintenanceHistories.MaintenanceHistories;
        }
        else if (dto.MaintenanceHistories != null)
        {
            maintenanceHistoriesForSave = dto.MaintenanceHistories.Adapt<List<TaktMaintenanceHistoryUpdateDto>>();
        }
        else
        {
            maintenanceHistoriesForSave = null;
        }
        if (maintenanceHistoriesForSave is not { Count: > 0 })
        {
            await _maintenanceHistoryRepository.DeleteAsync(x => x.EquipmentId == entity.Id);
        }
        else
        {
            var existingList = await _maintenanceHistoryRepository.GetListAsync(x => x.EquipmentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMaintenanceHistory>();
            for (var i = 0; i < maintenanceHistoriesForSave.Count; i++)
            {
                var childDto = maintenanceHistoriesForSave[i];
                childDto.EquipmentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.EquipCode = entity.EquipCode;
                if (childDto.MaintenanceHistoryId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MaintenanceHistoryId, out var target))
                    {
                        throw new TaktBusinessException("设备维护履历不存在（MaintenanceHistoryId={childDto.MaintenanceHistoryId}）");
                    }
                    if (target.EquipmentId != entity.Id)
                    {
                        throw new TaktBusinessException("设备维护履历不属于当前主表（MaintenanceHistoryId={childDto.MaintenanceHistoryId}）");
                    }
                    submittedIds.Add(childDto.MaintenanceHistoryId);
                    childDto.Adapt(target);
                    target.Id = childDto.MaintenanceHistoryId;
                    target.EquipmentId = entity.Id;
                    await _maintenanceHistoryRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktMaintenanceHistory>();
                    child.Id = 0;
                    child.EquipmentId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _maintenanceHistoryRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _maintenanceHistoryRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂设备查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEquipment, bool>> QueryExpression(TaktEquipmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEquipment>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.EquipCode != null && x.EquipCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || (x.EquipmentModel != null && x.EquipmentModel.Contains(keywords))
                || (x.EquipSpecification != null && x.EquipSpecification.Contains(keywords))
                || (x.EquipBrand != null && x.EquipBrand.Contains(keywords))
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.DealerBy != null && x.DealerBy.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.WorkshopBy != null && x.WorkshopBy.Contains(keywords))
                || (x.ProductionLineBy != null && x.ProductionLineBy.Contains(keywords))
                || (x.WorkstationBy != null && x.WorkstationBy.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.EquipmentLocation != null && x.EquipmentLocation.Contains(keywords))
                || (x.ResponsibleUserName != null && x.ResponsibleUserName.Contains(keywords))
                || (x.OperatorEmployeeName != null && x.OperatorEmployeeName.Contains(keywords))
                || (x.TechnicalParameters != null && x.TechnicalParameters.Contains(keywords))
                || (x.EquipmentImages != null && x.EquipmentImages.Contains(keywords))
                || (x.EquipmentDocuments != null && x.EquipmentDocuments.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipCode))
        {
            var equipCode = queryDto.EquipCode;
            exp = exp.And(x => x.EquipCode != null && x.EquipCode.Contains(equipCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentName))
        {
            var equipmentName = queryDto.EquipmentName;
            exp = exp.And(x => x.EquipmentName != null && x.EquipmentName.Contains(equipmentName));
        }

        if (queryDto?.EquipmentType.HasValue == true)
        {
            var equipmentType = queryDto.EquipmentType.Value;
            exp = exp.And(x => x.EquipmentType == equipmentType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentModel))
        {
            var equipmentModel = queryDto.EquipmentModel;
            exp = exp.And(x => x.EquipmentModel != null && x.EquipmentModel.Contains(equipmentModel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipSpecification))
        {
            var equipSpecification = queryDto.EquipSpecification;
            exp = exp.And(x => x.EquipSpecification != null && x.EquipSpecification.Contains(equipSpecification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipBrand))
        {
            var equipBrand = queryDto.EquipBrand;
            exp = exp.And(x => x.EquipBrand != null && x.EquipBrand.Contains(equipBrand));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Manufacturer))
        {
            var manufacturer = queryDto.Manufacturer;
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(manufacturer));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DealerBy))
        {
            var dealerBy = queryDto.DealerBy;
            exp = exp.And(x => x.DealerBy != null && x.DealerBy.Contains(dealerBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialNumber))
        {
            var serialNumber = queryDto.SerialNumber;
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(serialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkshopBy))
        {
            var workshopBy = queryDto.WorkshopBy;
            exp = exp.And(x => x.WorkshopBy != null && x.WorkshopBy.Contains(workshopBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionLineBy))
        {
            var productionLineBy = queryDto.ProductionLineBy;
            exp = exp.And(x => x.ProductionLineBy != null && x.ProductionLineBy.Contains(productionLineBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkstationBy))
        {
            var workstationBy = queryDto.WorkstationBy;
            exp = exp.And(x => x.WorkstationBy != null && x.WorkstationBy.Contains(workstationBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptBy = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentLocation))
        {
            var equipmentLocation = queryDto.EquipmentLocation;
            exp = exp.And(x => x.EquipmentLocation != null && x.EquipmentLocation.Contains(equipmentLocation));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponsibleUserName))
        {
            var responsibleUserBy = queryDto.ResponsibleUserName;
            exp = exp.And(x => x.ResponsibleUserName != null && x.ResponsibleUserName.Contains(responsibleUserBy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OperatorEmployeeName))
        {
            var operatorBy = queryDto.OperatorEmployeeName;
            exp = exp.And(x => x.OperatorEmployeeName != null && x.OperatorEmployeeName.Contains(operatorBy));
        }

        if (queryDto?.EquipmentOriginalValue.HasValue == true)
        {
            var equipmentOriginalValue = queryDto.EquipmentOriginalValue.Value;
            exp = exp.And(x => x.EquipmentOriginalValue == equipmentOriginalValue);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TechnicalParameters))
        {
            var technicalParameters = queryDto.TechnicalParameters;
            exp = exp.And(x => x.TechnicalParameters != null && x.TechnicalParameters.Contains(technicalParameters));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentImages))
        {
            var equipmentImages = queryDto.EquipmentImages;
            exp = exp.And(x => x.EquipmentImages != null && x.EquipmentImages.Contains(equipmentImages));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EquipmentDocuments))
        {
            var equipmentDocuments = queryDto.EquipmentDocuments;
            exp = exp.And(x => x.EquipmentDocuments != null && x.EquipmentDocuments.Contains(equipmentDocuments));
        }

        if (queryDto?.IsCritical.HasValue == true)
        {
            var isCritical = queryDto.IsCritical.Value;
            exp = exp.And(x => x.IsCritical == isCritical);
        }

        if (queryDto?.WarrantyStatus.HasValue == true)
        {
            var warrantyStatus = queryDto.WarrantyStatus.Value;
            exp = exp.And(x => x.WarrantyStatus == warrantyStatus);
        }

        if (queryDto?.EquipmentStatus.HasValue == true)
        {
            var equipmentStatus = queryDto.EquipmentStatus.Value;
            exp = exp.And(x => x.EquipmentStatus == equipmentStatus);
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

        if (queryDto?.PurchaseDateStart.HasValue == true)
        {
            var purchaseDateStart = queryDto.PurchaseDateStart.Value;
            exp = exp.And(x => x.PurchaseDate >= purchaseDateStart);
        }

        if (queryDto?.PurchaseDateEnd.HasValue == true)
        {
            var purchaseDateEnd = queryDto.PurchaseDateEnd.Value;
            exp = exp.And(x => x.PurchaseDate <= purchaseDateEnd);
        }

        if (queryDto?.InstallationDateStart.HasValue == true)
        {
            var installationDateStart = queryDto.InstallationDateStart.Value;
            exp = exp.And(x => x.InstallationDate >= installationDateStart);
        }

        if (queryDto?.InstallationDateEnd.HasValue == true)
        {
            var installationDateEnd = queryDto.InstallationDateEnd.Value;
            exp = exp.And(x => x.InstallationDate <= installationDateEnd);
        }

        if (queryDto?.StartDateStart.HasValue == true)
        {
            var startDateStart = queryDto.StartDateStart.Value;
            exp = exp.And(x => x.StartDate >= startDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            var startDateEnd = queryDto.StartDateEnd.Value;
            exp = exp.And(x => x.StartDate <= startDateEnd);
        }

        if (queryDto?.WarrantyStartDateStart.HasValue == true)
        {
            var warrantyStartDateStart = queryDto.WarrantyStartDateStart.Value;
            exp = exp.And(x => x.WarrantyStartDate >= warrantyStartDateStart);
        }

        if (queryDto?.WarrantyStartDateEnd.HasValue == true)
        {
            var warrantyStartDateEnd = queryDto.WarrantyStartDateEnd.Value;
            exp = exp.And(x => x.WarrantyStartDate <= warrantyStartDateEnd);
        }

        if (queryDto?.WarrantyEndDateStart.HasValue == true)
        {
            var warrantyEndDateStart = queryDto.WarrantyEndDateStart.Value;
            exp = exp.And(x => x.WarrantyEndDate >= warrantyEndDateStart);
        }

        if (queryDto?.WarrantyEndDateEnd.HasValue == true)
        {
            var warrantyEndDateEnd = queryDto.WarrantyEndDateEnd.Value;
            exp = exp.And(x => x.WarrantyEndDate <= warrantyEndDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktEquipmentQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.EquipCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentName))
        {
            return true;
        }
        if (queryDto.EquipmentType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentModel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipBrand))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Manufacturer))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DealerBy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkshopBy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionLineBy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkstationBy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentLocation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResponsibleUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OperatorEmployeeName))
        {
            return true;
        }
        if (queryDto.EquipmentOriginalValue.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TechnicalParameters))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentImages))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EquipmentDocuments))
        {
            return true;
        }
        if (queryDto.IsCritical.HasValue)
        {
            return true;
        }
        if (queryDto.WarrantyStatus.HasValue)
        {
            return true;
        }
        if (queryDto.EquipmentStatus.HasValue)
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
        if (queryDto.PurchaseDateStart.HasValue || queryDto.PurchaseDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.InstallationDateStart.HasValue || queryDto.InstallationDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.StartDateStart.HasValue || queryDto.StartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.WarrantyStartDateStart.HasValue || queryDto.WarrantyStartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.WarrantyEndDateStart.HasValue || queryDto.WarrantyEndDateEnd.HasValue)
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

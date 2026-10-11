// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceNotificationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：维护通知单应用服务实现
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
/// 维护通知单应用服务
/// </summary>
public class TaktMaintenanceNotificationService : TaktServiceBase, ITaktMaintenanceNotificationService
{
    private readonly ITaktApprovalRepository<TaktMaintenanceNotification> _maintenanceNotificationRepository;
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceNotificationRepository">维护通知单仓储</param>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceNotificationService(
        ITaktApprovalRepository<TaktMaintenanceNotification> maintenanceNotificationRepository,
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceNotificationRepository = maintenanceNotificationRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取维护通知单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceNotificationDto>> GetMaintenanceNotificationListAsync(TaktMaintenanceNotificationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaintenanceNotificationDto>.Create(
                new List<TaktMaintenanceNotificationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceNotificationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceNotificationDto>.Create(
            data.Adapt<List<TaktMaintenanceNotificationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto?> GetMaintenanceNotificationByIdAsync(long id)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceNotificationDto>();
    }

    /// <summary>
    /// 获取维护通知单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceNotificationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceNotificationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NotificationStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.NotificationCode,
            DictLabel = e.EquipmentName ?? e.NotificationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建维护通知单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> CreateMaintenanceNotificationAsync(TaktMaintenanceNotificationCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceNotification>();
        await StampMaintenanceNotificationEquipmentAsync(entity, dto);
        await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceNotificationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.NotificationCode == entity.NotificationCode);
        if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
        {
            throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
        }
        entity = await _maintenanceNotificationRepository.CreateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceNotificationDto>();
    }

    /// <summary>
    /// 更新维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationAsync(long id, TaktMaintenanceNotificationUpdateDto dto)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护通知单不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceNotificationEquipmentAsync(entity, dto);
        await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceNotificationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.NotificationCode == entity.NotificationCode,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
        {
            throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
        }
        await _maintenanceNotificationRepository.UpdateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(id) ?? throw new TaktBusinessException("维护通知单不存在");
    }

    /// <summary>
    /// 删除维护通知单
    /// </summary>
    /// <param name="id">维护通知单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceNotificationByIdAsync(long id)
    {
        var deleted = await _maintenanceNotificationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("维护通知单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除维护通知单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceNotificationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceNotificationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新维护通知单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceNotificationDto> UpdateMaintenanceNotificationStatusAsync(TaktMaintenanceNotificationStatusDto dto)
    {
        var entity = await _maintenanceNotificationRepository.GetByIdAsync(dto.MaintenanceNotificationId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护通知单不存在");
        }
        entity.NotificationStatus = dto.NotificationStatus;
        await _maintenanceNotificationRepository.UpdateAsync(entity);
        return await GetMaintenanceNotificationByIdAsync(dto.MaintenanceNotificationId) ?? throw new TaktBusinessException("维护通知单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceNotificationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceNotificationTemplateDto>(
            sheetName ?? "维护通知单导入模板",
            fileName ?? "维护通知单导入模板.xlsx");
    }

    /// <summary>
    /// 导入维护通知单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceNotificationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceNotificationImportDto>(fileStream, sheetName ?? "维护通知单导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceNotification>();
                var importDto = rows[i].Adapt<TaktMaintenanceNotificationCreateDto>();
                await StampMaintenanceNotificationEquipmentAsync(entity, importDto);
                await StampMaintenanceNotificationMaintenanceWorkOrderAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.NotificationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、NotificationCode）");
                }
                var isUnique_ix_takt_logistics_maintenance_notification_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceNotificationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.NotificationCode == entity.NotificationCode);
                if (!isUnique_ix_takt_logistics_maintenance_notification_code_unique)
                {
                    throw new TaktBusinessException("维护通知单的PlantCode、NotificationCode已存在");
                }
                await _maintenanceNotificationRepository.CreateAsync(entity);
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
    /// 导出维护通知单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceNotificationAsync(TaktMaintenanceNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaintenanceNotificationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceNotificationExportDto>(),
                sheetName ?? "维护通知单数据",
                fileName ?? "维护通知单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _maintenanceNotificationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceNotificationExportDto>(),
                sheetName ?? "维护通知单数据",
                fileName ?? "维护通知单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceNotificationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "维护通知单数据",
            fileName ?? "维护通知单导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取维护通知单统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护通知单统计</returns>
    public async Task<TaktMaintenanceNotificationStatDto> GetMaintenanceNotificationStatAsync(TaktMaintenanceNotificationStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.DiscoveredAtStart,
            queryDto.DiscoveredAtEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktMaintenanceNotification, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.DiscoveredAt >= start
            && x.DiscoveredAt <= end;
        var monthNotificationCount = await _maintenanceNotificationRepository.CountAsync(predicate);
        return new TaktMaintenanceNotificationStatDto
        {
            StatMonth = statMonth,
            MonthNotificationCount = monthNotificationCount,
        };
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步维护通知单主表外键（ManyToOne → 工厂设备）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceNotificationEquipmentAsync(TaktMaintenanceNotification entity, TaktMaintenanceNotificationCreateDto dto)
    {
        if (dto.EquipmentId <= 0)
        {
            return;
        }
        var master = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
        if (master == null)
        {
            throw new TaktBusinessException("工厂设备不存在");
        }
        entity.EquipmentId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.EquipCode))
        {
            entity.EquipCode = master.EquipCode;
        }
        if (string.IsNullOrEmpty(entity.EquipmentName))
        {
            entity.EquipmentName = master.EquipmentName;
        }
    }

    /// <summary>
    /// 同步维护通知单主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceNotificationMaintenanceWorkOrderAsync(TaktMaintenanceNotification entity, TaktMaintenanceNotificationCreateDto dto)
    {
        if (dto.MaintenanceWorkOrderId is not > 0)
        {
            return;
        }
        var master = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId.Value);
        if (master == null)
        {
            throw new TaktBusinessException("维护工单不存在");
        }
        entity.MaintenanceWorkOrderId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.NotificationCode))
        {
            entity.NotificationCode = master.NotificationCode ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.EquipCode))
        {
            entity.EquipCode = master.EquipCode;
        }
        if (string.IsNullOrEmpty(entity.EquipmentName))
        {
            entity.EquipmentName = master.EquipmentName;
        }
        if (string.IsNullOrEmpty(entity.FaultDescription))
        {
            entity.FaultDescription = master.FaultDescription ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.CostCenterCode))
        {
            entity.CostCenterCode = master.CostCenterCode ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建维护通知单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceNotification, bool>> QueryExpression(TaktMaintenanceNotificationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceNotification>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.NotificationCode != null && x.NotificationCode.Contains(keywords))
                || (x.EquipCode != null && x.EquipCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.ReportedByEmployeeName != null && x.ReportedByEmployeeName.Contains(keywords))
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.MaintenanceWorkOrderCode != null && x.MaintenanceWorkOrderCode.Contains(keywords))
                || (x.NotificationImages != null && x.NotificationImages.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.NotificationCode))
        {
            var notificationCode = queryDto.NotificationCode;
            exp = exp.And(x => x.NotificationCode != null && x.NotificationCode.Contains(notificationCode));
        }

        if (queryDto?.EquipmentId.HasValue == true)
        {
            var equipmentId = queryDto.EquipmentId.Value;
            exp = exp.And(x => x.EquipmentId == equipmentId);
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

        if (queryDto?.MaintenanceCategory.HasValue == true)
        {
            var maintenanceCategory = queryDto.MaintenanceCategory.Value;
            exp = exp.And(x => x.MaintenanceCategory == maintenanceCategory);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
        }

        if (queryDto?.NotificationStatus.HasValue == true)
        {
            var notificationStatus = queryDto.NotificationStatus.Value;
            exp = exp.And(x => x.NotificationStatus == notificationStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FaultDescription))
        {
            var faultDescription = queryDto.FaultDescription;
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(faultDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReportedByEmployeeName))
        {
            var reportedBy = queryDto.ReportedByEmployeeName;
            exp = exp.And(x => x.ReportedByEmployeeName != null && x.ReportedByEmployeeName.Contains(reportedBy));
        }

        if (queryDto?.CostCenterId.HasValue == true)
        {
            var costCenterId = queryDto.CostCenterId.Value;
            exp = exp.And(x => x.CostCenterId == costCenterId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterCode))
        {
            var costCenterCode = queryDto.CostCenterCode;
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(costCenterCode));
        }

        if (queryDto?.MaintenanceWorkOrderId.HasValue == true)
        {
            var maintenanceWorkOrderId = queryDto.MaintenanceWorkOrderId.Value;
            exp = exp.And(x => x.MaintenanceWorkOrderId == maintenanceWorkOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceWorkOrderCode))
        {
            var maintenanceWorkOrderCode = queryDto.MaintenanceWorkOrderCode;
            exp = exp.And(x => x.MaintenanceWorkOrderCode != null && x.MaintenanceWorkOrderCode.Contains(maintenanceWorkOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NotificationImages))
        {
            var notificationImages = queryDto.NotificationImages;
            exp = exp.And(x => x.NotificationImages != null && x.NotificationImages.Contains(notificationImages));
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

        if (queryDto?.DiscoveredAtStart.HasValue == true)
        {
            var discoveredAtStart = queryDto.DiscoveredAtStart.Value;
            exp = exp.And(x => x.DiscoveredAt >= discoveredAtStart);
        }

        if (queryDto?.DiscoveredAtEnd.HasValue == true)
        {
            var discoveredAtEnd = queryDto.DiscoveredAtEnd.Value;
            exp = exp.And(x => x.DiscoveredAt <= discoveredAtEnd);
        }

        if (queryDto?.BreakdownStartTimeStart.HasValue == true)
        {
            var breakdownStartTimeStart = queryDto.BreakdownStartTimeStart.Value;
            exp = exp.And(x => x.BreakdownStartTime >= breakdownStartTimeStart);
        }

        if (queryDto?.BreakdownStartTimeEnd.HasValue == true)
        {
            var breakdownStartTimeEnd = queryDto.BreakdownStartTimeEnd.Value;
            exp = exp.And(x => x.BreakdownStartTime <= breakdownStartTimeEnd);
        }

        if (queryDto?.BreakdownEndTimeStart.HasValue == true)
        {
            var breakdownEndTimeStart = queryDto.BreakdownEndTimeStart.Value;
            exp = exp.And(x => x.BreakdownEndTime >= breakdownEndTimeStart);
        }

        if (queryDto?.BreakdownEndTimeEnd.HasValue == true)
        {
            var breakdownEndTimeEnd = queryDto.BreakdownEndTimeEnd.Value;
            exp = exp.And(x => x.BreakdownEndTime <= breakdownEndTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaintenanceNotificationQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.NotificationCode))
        {
            return true;
        }
        if (queryDto.EquipmentId.HasValue)
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
        if (queryDto.MaintenanceCategory.HasValue)
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
        {
            return true;
        }
        if (queryDto.NotificationStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FaultDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReportedByEmployeeName))
        {
            return true;
        }
        if (queryDto.CostCenterId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterCode))
        {
            return true;
        }
        if (queryDto.MaintenanceWorkOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceWorkOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NotificationImages))
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
        if (queryDto.DiscoveredAtStart.HasValue || queryDto.DiscoveredAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.BreakdownStartTimeStart.HasValue || queryDto.BreakdownStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.BreakdownEndTimeStart.HasValue || queryDto.BreakdownEndTimeEnd.HasValue)
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

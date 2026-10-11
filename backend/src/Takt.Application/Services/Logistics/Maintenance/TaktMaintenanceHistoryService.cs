// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceHistoryService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设备维护履历应用服务实现
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
/// 设备维护履历应用服务
/// </summary>
public class TaktMaintenanceHistoryService : TaktServiceBase, ITaktMaintenanceHistoryService
{
    private readonly ITaktCompanyRepository<TaktMaintenanceHistory> _maintenanceHistoryRepository;
    private readonly ITaktCompanyRepository<TaktEquipment> _equipmentRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceHistoryRepository">设备维护履历仓储</param>
    /// <param name="equipmentRepository">工厂设备仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceHistoryService(
        ITaktCompanyRepository<TaktMaintenanceHistory> maintenanceHistoryRepository,
        ITaktCompanyRepository<TaktEquipment> equipmentRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceHistoryRepository = maintenanceHistoryRepository;
        _equipmentRepository = equipmentRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设备维护履历列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceHistoryDto>> GetMaintenanceHistoryListAsync(TaktMaintenanceHistoryQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaintenanceHistoryDto>.Create(
                new List<TaktMaintenanceHistoryDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceHistoryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceHistoryDto>.Create(
            data.Adapt<List<TaktMaintenanceHistoryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto?> GetMaintenanceHistoryByIdAsync(long id)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceHistoryDto>();
    }

    /// <summary>
    /// 获取设备维护履历选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceHistoryOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceHistoryRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceStatus == 1,
            x => x.WorkOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkOrderCode,
            DictLabel = e.WorkOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建设备维护履历
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> CreateMaintenanceHistoryAsync(TaktMaintenanceHistoryCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceHistory>();
        await StampMaintenanceHistoryEquipmentAsync(entity, dto);
        await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceHistoryRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId);
        if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
        {
            throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
        }
        entity = await _maintenanceHistoryRepository.CreateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceHistoryDto>();
    }

    /// <summary>
    /// 更新设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryAsync(long id, TaktMaintenanceHistoryUpdateDto dto)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护履历不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceHistoryEquipmentAsync(entity, dto);
        await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceHistoryRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
        {
            throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
        }
        await _maintenanceHistoryRepository.UpdateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(id) ?? throw new TaktBusinessException("设备维护履历不存在");
    }

    /// <summary>
    /// 删除设备维护履历
    /// </summary>
    /// <param name="id">设备维护履历ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceHistoryByIdAsync(long id)
    {
        var deleted = await _maintenanceHistoryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设备维护履历不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设备维护履历
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceHistoryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceHistoryByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设备维护履历状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceHistoryDto> UpdateMaintenanceHistoryStatusAsync(TaktMaintenanceHistoryStatusDto dto)
    {
        var entity = await _maintenanceHistoryRepository.GetByIdAsync(dto.MaintenanceHistoryId);
        if (entity == null)
        {
            throw new TaktBusinessException("设备维护履历不存在");
        }
        entity.MaintenanceStatus = dto.MaintenanceStatus;
        await _maintenanceHistoryRepository.UpdateAsync(entity);
        return await GetMaintenanceHistoryByIdAsync(dto.MaintenanceHistoryId) ?? throw new TaktBusinessException("设备维护履历不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceHistoryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceHistoryTemplateDto>(
            sheetName ?? "设备维护履历导入模板",
            fileName ?? "设备维护履历导入模板.xlsx");
    }

    /// <summary>
    /// 导入设备维护履历
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceHistoryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceHistoryImportDto>(fileStream, sheetName ?? "设备维护履历导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceHistory>();
                var importDto = rows[i].Adapt<TaktMaintenanceHistoryCreateDto>();
                await StampMaintenanceHistoryEquipmentAsync(entity, importDto);
                await StampMaintenanceHistoryMaintenanceWorkOrderAsync(entity, importDto);
                var importKey = $"{entity.MaintenanceWorkOrderId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaintenanceWorkOrderId）");
                }
                var isUnique_ix_takt_logistics_maintenance_history_work_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceHistoryRepository,
                    x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId);
                if (!isUnique_ix_takt_logistics_maintenance_history_work_order_unique)
                {
                    throw new TaktBusinessException("设备维护履历的MaintenanceWorkOrderId已存在");
                }
                await _maintenanceHistoryRepository.CreateAsync(entity);
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
    /// 导出设备维护履历
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceHistoryAsync(TaktMaintenanceHistoryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaintenanceHistoryQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceHistoryExportDto>(),
                sheetName ?? "设备维护履历数据",
                fileName ?? "设备维护履历导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _maintenanceHistoryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceHistoryExportDto>(),
                sheetName ?? "设备维护履历数据",
                fileName ?? "设备维护履历导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceHistoryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设备维护履历数据",
            fileName ?? "设备维护履历导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取维护履历统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>维护履历统计</returns>
    public async Task<TaktMaintenanceHistoryStatDto> GetMaintenanceHistoryStatAsync(TaktMaintenanceHistoryStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.MaintenanceDateStart,
            queryDto.MaintenanceDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktMaintenanceHistory, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.MaintenanceDate >= start
            && x.MaintenanceDate <= end;
        var monthHistoryCount = await _maintenanceHistoryRepository.CountAsync(predicate);
        return new TaktMaintenanceHistoryStatDto
        {
            StatMonth = statMonth,
            MonthHistoryCount = monthHistoryCount,
        };
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步设备维护履历主表外键（ManyToOne → 工厂设备）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceHistoryEquipmentAsync(TaktMaintenanceHistory entity, TaktMaintenanceHistoryCreateDto dto)
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
    }

    /// <summary>
    /// 同步设备维护履历主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceHistoryMaintenanceWorkOrderAsync(TaktMaintenanceHistory entity, TaktMaintenanceHistoryCreateDto dto)
    {
        if (dto.MaintenanceWorkOrderId <= 0)
        {
            return;
        }
        var master = await _maintenanceWorkOrderRepository.GetByIdAsync(dto.MaintenanceWorkOrderId);
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
        if (string.IsNullOrEmpty(entity.WorkOrderCode))
        {
            entity.WorkOrderCode = master.WorkOrderCode;
        }
        if (string.IsNullOrEmpty(entity.EquipCode))
        {
            entity.EquipCode = master.EquipCode;
        }
        if (string.IsNullOrEmpty(entity.MaintenanceCompany))
        {
            entity.MaintenanceCompany = master.MaintenanceCompany ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.MaintenanceContent))
        {
            entity.MaintenanceContent = master.MaintenanceContent ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.FaultDescription))
        {
            entity.FaultDescription = master.FaultDescription ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.Solution))
        {
            entity.Solution = master.Solution ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.MaintenanceDocuments))
        {
            entity.MaintenanceDocuments = master.MaintenanceDocuments ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.MaintenanceImages))
        {
            entity.MaintenanceImages = master.MaintenanceImages ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.AcceptedSummary))
        {
            entity.AcceptedSummary = master.AcceptedSummary ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.AcceptedByEmployeeName))
        {
            entity.AcceptedByEmployeeName = master.AcceptedByEmployeeName ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设备维护履历查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceHistory, bool>> QueryExpression(TaktMaintenanceHistoryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceHistory>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.EquipCode != null && x.EquipCode.Contains(keywords))
                || (x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(keywords))
                || (x.MaintenanceTechnician != null && x.MaintenanceTechnician.Contains(keywords))
                || (x.MaintenanceContent != null && x.MaintenanceContent.Contains(keywords))
                || (x.FaultDescription != null && x.FaultDescription.Contains(keywords))
                || (x.Solution != null && x.Solution.Contains(keywords))
                || (x.UsedParts != null && x.UsedParts.Contains(keywords))
                || (x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(keywords))
                || (x.MaintenanceImages != null && x.MaintenanceImages.Contains(keywords))
                || (x.AcceptedSummary != null && x.AcceptedSummary.Contains(keywords))
                || (x.AcceptedByEmployeeName != null && x.AcceptedByEmployeeName.Contains(keywords))
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

        if (queryDto?.MaintenanceWorkOrderId.HasValue == true)
        {
            var maintenanceWorkOrderId = queryDto.MaintenanceWorkOrderId.Value;
            exp = exp.And(x => x.MaintenanceWorkOrderId == maintenanceWorkOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkOrderCode))
        {
            var workOrderCode = queryDto.WorkOrderCode;
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(workOrderCode));
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

        if (queryDto?.MaintenanceType.HasValue == true)
        {
            var maintenanceType = queryDto.MaintenanceType.Value;
            exp = exp.And(x => x.MaintenanceType == maintenanceType);
        }

        if (queryDto?.MaintenanceCategory.HasValue == true)
        {
            var maintenanceCategory = queryDto.MaintenanceCategory.Value;
            exp = exp.And(x => x.MaintenanceCategory == maintenanceCategory);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceCompany))
        {
            var maintenanceCompany = queryDto.MaintenanceCompany;
            exp = exp.And(x => x.MaintenanceCompany != null && x.MaintenanceCompany.Contains(maintenanceCompany));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceTechnician))
        {
            var maintenanceTechnician = queryDto.MaintenanceTechnician;
            exp = exp.And(x => x.MaintenanceTechnician != null && x.MaintenanceTechnician.Contains(maintenanceTechnician));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceContent))
        {
            var maintenanceContent = queryDto.MaintenanceContent;
            exp = exp.And(x => x.MaintenanceContent != null && x.MaintenanceContent.Contains(maintenanceContent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FaultDescription))
        {
            var faultDescription = queryDto.FaultDescription;
            exp = exp.And(x => x.FaultDescription != null && x.FaultDescription.Contains(faultDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Solution))
        {
            var solution = queryDto.Solution;
            exp = exp.And(x => x.Solution != null && x.Solution.Contains(solution));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UsedParts))
        {
            var usedParts = queryDto.UsedParts;
            exp = exp.And(x => x.UsedParts != null && x.UsedParts.Contains(usedParts));
        }

        if (queryDto?.MaintenanceCost.HasValue == true)
        {
            var maintenanceCost = queryDto.MaintenanceCost.Value;
            exp = exp.And(x => x.MaintenanceCost == maintenanceCost);
        }

        if (queryDto?.MaintenanceResult.HasValue == true)
        {
            var maintenanceResult = queryDto.MaintenanceResult.Value;
            exp = exp.And(x => x.MaintenanceResult == maintenanceResult);
        }

        if (queryDto?.MaintenanceStatus.HasValue == true)
        {
            var maintenanceStatus = queryDto.MaintenanceStatus.Value;
            exp = exp.And(x => x.MaintenanceStatus == maintenanceStatus);
        }

        if (queryDto?.MaintenanceCycleDays.HasValue == true)
        {
            var maintenanceCycleDays = queryDto.MaintenanceCycleDays.Value;
            exp = exp.And(x => x.MaintenanceCycleDays == maintenanceCycleDays);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceDocuments))
        {
            var maintenanceDocuments = queryDto.MaintenanceDocuments;
            exp = exp.And(x => x.MaintenanceDocuments != null && x.MaintenanceDocuments.Contains(maintenanceDocuments));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceImages))
        {
            var maintenanceImages = queryDto.MaintenanceImages;
            exp = exp.And(x => x.MaintenanceImages != null && x.MaintenanceImages.Contains(maintenanceImages));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AcceptedSummary))
        {
            var acceptedSummary = queryDto.AcceptedSummary;
            exp = exp.And(x => x.AcceptedSummary != null && x.AcceptedSummary.Contains(acceptedSummary));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AcceptedByEmployeeName))
        {
            var acceptedBy = queryDto.AcceptedByEmployeeName;
            exp = exp.And(x => x.AcceptedByEmployeeName != null && x.AcceptedByEmployeeName.Contains(acceptedBy));
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

        if (queryDto?.MaintenanceDateStart.HasValue == true)
        {
            var maintenanceDateStart = queryDto.MaintenanceDateStart.Value;
            exp = exp.And(x => x.MaintenanceDate >= maintenanceDateStart);
        }

        if (queryDto?.MaintenanceDateEnd.HasValue == true)
        {
            var maintenanceDateEnd = queryDto.MaintenanceDateEnd.Value;
            exp = exp.And(x => x.MaintenanceDate <= maintenanceDateEnd);
        }

        if (queryDto?.MaintenanceStartTimeStart.HasValue == true)
        {
            var maintenanceStartTimeStart = queryDto.MaintenanceStartTimeStart.Value;
            exp = exp.And(x => x.MaintenanceStartTime >= maintenanceStartTimeStart);
        }

        if (queryDto?.MaintenanceStartTimeEnd.HasValue == true)
        {
            var maintenanceStartTimeEnd = queryDto.MaintenanceStartTimeEnd.Value;
            exp = exp.And(x => x.MaintenanceStartTime <= maintenanceStartTimeEnd);
        }

        if (queryDto?.MaintenanceEndTimeStart.HasValue == true)
        {
            var maintenanceEndTimeStart = queryDto.MaintenanceEndTimeStart.Value;
            exp = exp.And(x => x.MaintenanceEndTime >= maintenanceEndTimeStart);
        }

        if (queryDto?.MaintenanceEndTimeEnd.HasValue == true)
        {
            var maintenanceEndTimeEnd = queryDto.MaintenanceEndTimeEnd.Value;
            exp = exp.And(x => x.MaintenanceEndTime <= maintenanceEndTimeEnd);
        }

        if (queryDto?.NextMaintenanceDateStart.HasValue == true)
        {
            var nextMaintenanceDateStart = queryDto.NextMaintenanceDateStart.Value;
            exp = exp.And(x => x.NextMaintenanceDate >= nextMaintenanceDateStart);
        }

        if (queryDto?.NextMaintenanceDateEnd.HasValue == true)
        {
            var nextMaintenanceDateEnd = queryDto.NextMaintenanceDateEnd.Value;
            exp = exp.And(x => x.NextMaintenanceDate <= nextMaintenanceDateEnd);
        }

        if (queryDto?.AcceptedAtStart.HasValue == true)
        {
            var acceptedAtStart = queryDto.AcceptedAtStart.Value;
            exp = exp.And(x => x.AcceptedAt >= acceptedAtStart);
        }

        if (queryDto?.AcceptedAtEnd.HasValue == true)
        {
            var acceptedAtEnd = queryDto.AcceptedAtEnd.Value;
            exp = exp.And(x => x.AcceptedAt <= acceptedAtEnd);
        }

        if (queryDto?.ArchivedAtStart.HasValue == true)
        {
            var archivedAtStart = queryDto.ArchivedAtStart.Value;
            exp = exp.And(x => x.ArchivedAt >= archivedAtStart);
        }

        if (queryDto?.ArchivedAtEnd.HasValue == true)
        {
            var archivedAtEnd = queryDto.ArchivedAtEnd.Value;
            exp = exp.And(x => x.ArchivedAt <= archivedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaintenanceHistoryQueryDto? queryDto)
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
        if (queryDto.MaintenanceWorkOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkOrderCode))
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
        if (queryDto.MaintenanceType.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceCategory.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceCompany))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceTechnician))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceContent))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FaultDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Solution))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UsedParts))
        {
            return true;
        }
        if (queryDto.MaintenanceCost.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceResult.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceStatus.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceCycleDays.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceDocuments))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceImages))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptedSummary))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptedByEmployeeName))
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
        if (queryDto.MaintenanceDateStart.HasValue || queryDto.MaintenanceDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceStartTimeStart.HasValue || queryDto.MaintenanceStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.MaintenanceEndTimeStart.HasValue || queryDto.MaintenanceEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.NextMaintenanceDateStart.HasValue || queryDto.NextMaintenanceDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.AcceptedAtStart.HasValue || queryDto.AcceptedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ArchivedAtStart.HasValue || queryDto.ArchivedAtEnd.HasValue)
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Maintenance
// 文件名称：TaktMaintenanceWorkOrderLaborService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：维护工单报工应用服务实现
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
/// 维护工单报工应用服务
/// </summary>
public class TaktMaintenanceWorkOrderLaborService : TaktServiceBase, ITaktMaintenanceWorkOrderLaborService
{
    private readonly ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> _maintenanceWorkOrderLaborRepository;
    private readonly ITaktApprovalRepository<TaktMaintenanceWorkOrder> _maintenanceWorkOrderRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maintenanceWorkOrderLaborRepository">维护工单报工仓储</param>
    /// <param name="maintenanceWorkOrderRepository">维护工单仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaintenanceWorkOrderLaborService(
        ITaktCompanyRepository<TaktMaintenanceWorkOrderLabor> maintenanceWorkOrderLaborRepository,
        ITaktApprovalRepository<TaktMaintenanceWorkOrder> maintenanceWorkOrderRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _maintenanceWorkOrderLaborRepository = maintenanceWorkOrderLaborRepository;
        _maintenanceWorkOrderRepository = maintenanceWorkOrderRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取维护工单报工列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaintenanceWorkOrderLaborDto>> GetMaintenanceWorkOrderLaborListAsync(TaktMaintenanceWorkOrderLaborQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMaintenanceWorkOrderLaborDto>.Create(
                new List<TaktMaintenanceWorkOrderLaborDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _maintenanceWorkOrderLaborRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaintenanceWorkOrderLaborDto>.Create(
            data.Adapt<List<TaktMaintenanceWorkOrderLaborDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderLaborDto?> GetMaintenanceWorkOrderLaborByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderLaborRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaintenanceWorkOrderLaborDto>();
    }

    /// <summary>
    /// 获取维护工单报工选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaintenanceWorkOrderLaborOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _maintenanceWorkOrderLaborRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ConfirmationStatus == 1 && x.IsObsolete == 0,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WorkOrderCode,
            DictLabel = e.EmployeeName ?? e.WorkOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建维护工单报工
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderLaborDto> CreateMaintenanceWorkOrderLaborAsync(TaktMaintenanceWorkOrderLaborCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaintenanceWorkOrderLabor>();
        entity.IsObsolete = 0;
        await StampMaintenanceWorkOrderLaborMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderLaborRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                && x.LineNumber == entity.LineNumber
                && x.EmployeeCode == entity.EmployeeCode);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique)
        {
            throw new TaktBusinessException("维护工单报工的MaintenanceWorkOrderId、LineNumber、EmployeeCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _maintenanceWorkOrderLaborRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
                x => x.LineNumber);
            var businessCode = entity.MaintenanceWorkOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _maintenanceWorkOrderLaborRepository.CreateAsync(entity);
        return await GetMaintenanceWorkOrderLaborByIdAsync(entity.Id) ?? entity.Adapt<TaktMaintenanceWorkOrderLaborDto>();
    }

    /// <summary>
    /// 更新维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborAsync(long id, TaktMaintenanceWorkOrderLaborUpdateDto dto)
    {
        var entity = await _maintenanceWorkOrderLaborRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单报工不存在");
        }
        dto.Adapt(entity);
        await StampMaintenanceWorkOrderLaborMaintenanceWorkOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _maintenanceWorkOrderLaborRepository,
            x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                && x.LineNumber == entity.LineNumber
                && x.EmployeeCode == entity.EmployeeCode,
            id);
        if (!isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique)
        {
            throw new TaktBusinessException("维护工单报工的MaintenanceWorkOrderId、LineNumber、EmployeeCode已存在");
        }
        await _maintenanceWorkOrderLaborRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderLaborByIdAsync(id) ?? throw new TaktBusinessException("维护工单报工不存在");
    }

    /// <summary>
    /// 删除维护工单报工
    /// </summary>
    /// <param name="id">维护工单报工ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderLaborByIdAsync(long id)
    {
        var entity = await _maintenanceWorkOrderLaborRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单报工不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("维护工单报工不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("维护工单报工已作废");
        }
        entity.IsObsolete = 1;
        await _maintenanceWorkOrderLaborRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除维护工单报工
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaintenanceWorkOrderLaborBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaintenanceWorkOrderLaborByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新维护工单报工状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborStatusAsync(TaktMaintenanceWorkOrderLaborStatusDto dto)
    {
        var entity = await _maintenanceWorkOrderLaborRepository.GetByIdAsync(dto.MaintenanceWorkOrderLaborId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单报工不存在");
        }
        entity.ConfirmationStatus = dto.ConfirmationStatus;
        await _maintenanceWorkOrderLaborRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderLaborByIdAsync(dto.MaintenanceWorkOrderLaborId) ?? throw new TaktBusinessException("维护工单报工不存在");
    }

    /// <summary>
    /// 更新维护工单报工作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaintenanceWorkOrderLaborDto> UpdateMaintenanceWorkOrderLaborObsoleteAsync(TaktMaintenanceWorkOrderLaborObsoleteDto dto)
    {
        var entity = await _maintenanceWorkOrderLaborRepository.GetByIdAsync(dto.MaintenanceWorkOrderLaborId);
        if (entity == null)
        {
            throw new TaktBusinessException("维护工单报工不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("维护工单报工不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _maintenanceWorkOrderLaborRepository.UpdateAsync(entity);
        return await GetMaintenanceWorkOrderLaborByIdAsync(dto.MaintenanceWorkOrderLaborId) ?? throw new TaktBusinessException("维护工单报工不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaintenanceWorkOrderLaborTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaintenanceWorkOrderLaborTemplateDto>(
            sheetName ?? "维护工单报工导入模板",
            fileName ?? "维护工单报工导入模板.xlsx");
    }

    /// <summary>
    /// 导入维护工单报工
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaintenanceWorkOrderLaborAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaintenanceWorkOrderLaborImportDto>(fileStream, sheetName ?? "维护工单报工导入模板");
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
                var entity = rows[i].Adapt<TaktMaintenanceWorkOrderLabor>();
                var importDto = rows[i].Adapt<TaktMaintenanceWorkOrderLaborCreateDto>();
                await StampMaintenanceWorkOrderLaborMaintenanceWorkOrderAsync(entity, importDto);
                var importKey = $"{entity.MaintenanceWorkOrderId}|{entity.LineNumber}|{entity.EmployeeCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaintenanceWorkOrderId、LineNumber、EmployeeCode）");
                }
                var isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _maintenanceWorkOrderLaborRepository,
                    x => x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId
                        && x.LineNumber == entity.LineNumber
                        && x.EmployeeCode == entity.EmployeeCode);
                if (!isUnique_ix_takt_logistics_maintenance_work_order_labor_order_line_unique)
                {
                    throw new TaktBusinessException("维护工单报工的MaintenanceWorkOrderId、LineNumber、EmployeeCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _maintenanceWorkOrderLaborRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaintenanceWorkOrderId == entity.MaintenanceWorkOrderId,
                        x => x.LineNumber);
                    var businessCode = entity.MaintenanceWorkOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _maintenanceWorkOrderLaborRepository.CreateAsync(entity);
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
    /// 导出维护工单报工
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaintenanceWorkOrderLaborAsync(TaktMaintenanceWorkOrderLaborQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMaintenanceWorkOrderLaborQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceWorkOrderLaborExportDto>(),
                sheetName ?? "维护工单报工数据",
                fileName ?? "维护工单报工导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _maintenanceWorkOrderLaborRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaintenanceWorkOrderLaborExportDto>(),
                sheetName ?? "维护工单报工数据",
                fileName ?? "维护工单报工导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaintenanceWorkOrderLaborExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "维护工单报工数据",
            fileName ?? "维护工单报工导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步维护工单报工主表外键（ManyToOne → 维护工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMaintenanceWorkOrderLaborMaintenanceWorkOrderAsync(TaktMaintenanceWorkOrderLabor entity, TaktMaintenanceWorkOrderLaborCreateDto dto)
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
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建维护工单报工查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaintenanceWorkOrderLabor, bool>> QueryExpression(TaktMaintenanceWorkOrderLaborQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaintenanceWorkOrderLabor>();

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
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.EmployeeCode != null && x.EmployeeCode.Contains(keywords))
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.OperationDescription != null && x.OperationDescription.Contains(keywords))
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

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            var employeeId = queryDto.EmployeeId.Value;
            exp = exp.And(x => x.EmployeeId == employeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeCode))
        {
            var employeeCode = queryDto.EmployeeCode;
            exp = exp.And(x => x.EmployeeCode != null && x.EmployeeCode.Contains(employeeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmployeeName))
        {
            var employeeName = queryDto.EmployeeName;
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(employeeName));
        }

        if (queryDto?.WorkHours.HasValue == true)
        {
            var workHours = queryDto.WorkHours.Value;
            exp = exp.And(x => x.WorkHours == workHours);
        }

        if (queryDto?.HourlyRate.HasValue == true)
        {
            var hourlyRate = queryDto.HourlyRate.Value;
            exp = exp.And(x => x.HourlyRate == hourlyRate);
        }

        if (queryDto?.LaborCost.HasValue == true)
        {
            var laborCost = queryDto.LaborCost.Value;
            exp = exp.And(x => x.LaborCost == laborCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OperationDescription))
        {
            var operationDescription = queryDto.OperationDescription;
            exp = exp.And(x => x.OperationDescription != null && x.OperationDescription.Contains(operationDescription));
        }

        if (queryDto?.ConfirmationStatus.HasValue == true)
        {
            var confirmationStatus = queryDto.ConfirmationStatus.Value;
            exp = exp.And(x => x.ConfirmationStatus == confirmationStatus);
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

        if (queryDto?.WorkDateStart.HasValue == true)
        {
            var workDateStart = queryDto.WorkDateStart.Value;
            exp = exp.And(x => x.WorkDate >= workDateStart);
        }

        if (queryDto?.WorkDateEnd.HasValue == true)
        {
            var workDateEnd = queryDto.WorkDateEnd.Value;
            exp = exp.And(x => x.WorkDate <= workDateEnd);
        }

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            var startTimeStart = queryDto.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime >= startTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            var startTimeEnd = queryDto.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime <= startTimeEnd);
        }

        if (queryDto?.EndTimeStart.HasValue == true)
        {
            var endTimeStart = queryDto.EndTimeStart.Value;
            exp = exp.And(x => x.EndTime >= endTimeStart);
        }

        if (queryDto?.EndTimeEnd.HasValue == true)
        {
            var endTimeEnd = queryDto.EndTimeEnd.Value;
            exp = exp.And(x => x.EndTime <= endTimeEnd);
        }

        if (queryDto?.ConfirmedAtStart.HasValue == true)
        {
            var confirmedAtStart = queryDto.ConfirmedAtStart.Value;
            exp = exp.And(x => x.ConfirmedAt >= confirmedAtStart);
        }

        if (queryDto?.ConfirmedAtEnd.HasValue == true)
        {
            var confirmedAtEnd = queryDto.ConfirmedAtEnd.Value;
            exp = exp.And(x => x.ConfirmedAt <= confirmedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktMaintenanceWorkOrderLaborQueryDto? queryDto)
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
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.EmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmployeeName))
        {
            return true;
        }
        if (queryDto.WorkHours.HasValue)
        {
            return true;
        }
        if (queryDto.HourlyRate.HasValue)
        {
            return true;
        }
        if (queryDto.LaborCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OperationDescription))
        {
            return true;
        }
        if (queryDto.ConfirmationStatus.HasValue)
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
        if (queryDto.WorkDateStart.HasValue || queryDto.WorkDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.StartTimeStart.HasValue || queryDto.StartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndTimeStart.HasValue || queryDto.EndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ConfirmedAtStart.HasValue || queryDto.ConfirmedAtEnd.HasValue)
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

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktEquipmentOperationRateService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：机器稼动率应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 机器稼动率应用服务
/// </summary>
public class TaktEquipmentOperationRateService : TaktServiceBase, ITaktEquipmentOperationRateService
{
    private readonly ITaktCompanyRepository<TaktEquipmentOperationRate> _equipmentOperationRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="equipmentOperationRateRepository">机器稼动率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEquipmentOperationRateService(
        ITaktCompanyRepository<TaktEquipmentOperationRate> equipmentOperationRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _equipmentOperationRateRepository = equipmentOperationRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取机器稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEquipmentOperationRateDto>> GetEquipmentOperationRateListAsync(TaktEquipmentOperationRateQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _equipmentOperationRateRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEquipmentOperationRateDto>.Create(
            data.Adapt<List<TaktEquipmentOperationRateDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentOperationRateDto?> GetEquipmentOperationRateByIdAsync(long id)
    {
        var entity = await _equipmentOperationRateRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEquipmentOperationRateDto>();
    }

    /// <summary>
    /// 获取机器稼动率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEquipmentOperationRateOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _equipmentOperationRateRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RateStatus == 1,
            x => x.EquipmentName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EquipmentName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建机器稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentOperationRateDto> CreateEquipmentOperationRateAsync(TaktEquipmentOperationRateCreateDto dto)
    {
        var entity = dto.Adapt<TaktEquipmentOperationRate>();
        var isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipCode == entity.EquipCode
                && x.TimeCategory == entity.TimeCategory
                && x.StartDate == entity.StartDate
                && x.ShiftNo == entity.ShiftNo);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique)
        {
            throw new TaktBusinessException("机器稼动率的PlantCode、EquipCode、TimeCategory、StartDate、ShiftNo已存在");
        }
        entity = await _equipmentOperationRateRepository.CreateAsync(entity);
        return await GetEquipmentOperationRateByIdAsync(entity.Id) ?? entity.Adapt<TaktEquipmentOperationRateDto>();
    }

    /// <summary>
    /// 更新机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentOperationRateDto> UpdateEquipmentOperationRateAsync(long id, TaktEquipmentOperationRateUpdateDto dto)
    {
        var entity = await _equipmentOperationRateRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("机器稼动率不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique = await _uniqueValidator.IsUniqueAsync(
            _equipmentOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.EquipCode == entity.EquipCode
                && x.TimeCategory == entity.TimeCategory
                && x.StartDate == entity.StartDate
                && x.ShiftNo == entity.ShiftNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique)
        {
            throw new TaktBusinessException("机器稼动率的PlantCode、EquipCode、TimeCategory、StartDate、ShiftNo已存在");
        }
        await _equipmentOperationRateRepository.UpdateAsync(entity);
        return await GetEquipmentOperationRateByIdAsync(id) ?? throw new TaktBusinessException("机器稼动率不存在");
    }

    /// <summary>
    /// 删除机器稼动率
    /// </summary>
    /// <param name="id">机器稼动率ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentOperationRateByIdAsync(long id)
    {
        var deleted = await _equipmentOperationRateRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("机器稼动率不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除机器稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEquipmentOperationRateBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEquipmentOperationRateByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新机器稼动率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEquipmentOperationRateDto> UpdateEquipmentOperationRateStatusAsync(TaktEquipmentOperationRateStatusDto dto)
    {
        var entity = await _equipmentOperationRateRepository.GetByIdAsync(dto.EquipmentOperationRateId);
        if (entity == null)
        {
            throw new TaktBusinessException("机器稼动率不存在");
        }
        entity.RateStatus = dto.RateStatus;
        await _equipmentOperationRateRepository.UpdateAsync(entity);
        return await GetEquipmentOperationRateByIdAsync(dto.EquipmentOperationRateId) ?? throw new TaktBusinessException("机器稼动率不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEquipmentOperationRateTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEquipmentOperationRateTemplateDto>(
            sheetName ?? "机器稼动率导入模板",
            fileName ?? "机器稼动率导入模板.xlsx");
    }

    /// <summary>
    /// 导入机器稼动率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEquipmentOperationRateAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEquipmentOperationRateImportDto>(fileStream, sheetName ?? "机器稼动率导入模板");
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
                var entity = rows[i].Adapt<TaktEquipmentOperationRate>();
                var importKey = $"{entity.PlantCode}|{entity.EquipCode}|{entity.TimeCategory}|{entity.StartDate}|{entity.ShiftNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、EquipCode、TimeCategory、StartDate、ShiftNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique = await _uniqueValidator.IsUniqueAsync(
                    _equipmentOperationRateRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.EquipCode == entity.EquipCode
                        && x.TimeCategory == entity.TimeCategory
                        && x.StartDate == entity.StartDate
                        && x.ShiftNo == entity.ShiftNo);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_equipment_operation_rate_eor_unique)
                {
                    throw new TaktBusinessException("机器稼动率的PlantCode、EquipCode、TimeCategory、StartDate、ShiftNo已存在");
                }
                await _equipmentOperationRateRepository.CreateAsync(entity);
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
    /// 导出机器稼动率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEquipmentOperationRateAsync(TaktEquipmentOperationRateQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktEquipmentOperationRateQueryDto());
        var list = await _equipmentOperationRateRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEquipmentOperationRateExportDto>(),
                sheetName ?? "机器稼动率数据",
                fileName ?? "机器稼动率导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEquipmentOperationRateExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "机器稼动率数据",
            fileName ?? "机器稼动率导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建机器稼动率查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEquipmentOperationRate, bool>> QueryExpression(TaktEquipmentOperationRateQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEquipmentOperationRate>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.TimeCategory).Contains(keywords)
                || SqlFunc.ToString(x.WeekNumber).Contains(keywords)
                || SqlFunc.ToString(x.MonthNumber).Contains(keywords)
                || (x.EquipCode != null && x.EquipCode.Contains(keywords))
                || (x.EquipmentName != null && x.EquipmentName.Contains(keywords))
                || SqlFunc.ToString(x.EquipmentType).Contains(keywords)
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || SqlFunc.ToString(x.PlannedRuntime).Contains(keywords)
                || SqlFunc.ToString(x.ActualRuntime).Contains(keywords)
                || SqlFunc.ToString(x.Downtime).Contains(keywords)
                || SqlFunc.ToString(x.EquipmentOperationRate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedOutput).Contains(keywords)
                || SqlFunc.ToString(x.ActualOutput).Contains(keywords)
                || SqlFunc.ToString(x.QualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.DefectiveQuantity).Contains(keywords)
                || SqlFunc.ToString(x.YieldRate).Contains(keywords)
                || SqlFunc.ToString(x.DowntimeReasonType).Contains(keywords)
                || (x.DowntimeReason != null && x.DowntimeReason.Contains(keywords))
                || (x.EquipmentOperator != null && x.EquipmentOperator.Contains(keywords))
                || (x.EquipmentMaintainer != null && x.EquipmentMaintainer.Contains(keywords))
                || (x.TeamLeader != null && x.TeamLeader.Contains(keywords))
                || SqlFunc.ToString(x.RateStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.EndDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.TimeCategory.HasValue == true)
        {
            exp = exp.And(x => x.TimeCategory == queryDto.TimeCategory);
        }

        if (queryDto?.WeekNumber.HasValue == true)
        {
            exp = exp.And(x => x.WeekNumber == queryDto.WeekNumber);
        }

        if (queryDto?.MonthNumber.HasValue == true)
        {
            exp = exp.And(x => x.MonthNumber == queryDto.MonthNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipCode))
        {
            exp = exp.And(x => x.EquipCode != null && x.EquipCode.Contains(queryDto.EquipCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentName))
        {
            exp = exp.And(x => x.EquipmentName != null && x.EquipmentName.Contains(queryDto.EquipmentName));
        }

        if (queryDto?.EquipmentType.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentType == queryDto.EquipmentType);
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (queryDto?.PlannedRuntime.HasValue == true)
        {
            exp = exp.And(x => x.PlannedRuntime == queryDto.PlannedRuntime);
        }

        if (queryDto?.ActualRuntime.HasValue == true)
        {
            exp = exp.And(x => x.ActualRuntime == queryDto.ActualRuntime);
        }

        if (queryDto?.Downtime.HasValue == true)
        {
            exp = exp.And(x => x.Downtime == queryDto.Downtime);
        }

        if (queryDto?.EquipmentOperationRate.HasValue == true)
        {
            exp = exp.And(x => x.EquipmentOperationRate == queryDto.EquipmentOperationRate);
        }

        if (queryDto?.PlannedOutput.HasValue == true)
        {
            exp = exp.And(x => x.PlannedOutput == queryDto.PlannedOutput);
        }

        if (queryDto?.ActualOutput.HasValue == true)
        {
            exp = exp.And(x => x.ActualOutput == queryDto.ActualOutput);
        }

        if (queryDto?.QualifiedQuantity.HasValue == true)
        {
            exp = exp.And(x => x.QualifiedQuantity == queryDto.QualifiedQuantity);
        }

        if (queryDto?.DefectiveQuantity.HasValue == true)
        {
            exp = exp.And(x => x.DefectiveQuantity == queryDto.DefectiveQuantity);
        }

        if (queryDto?.YieldRate.HasValue == true)
        {
            exp = exp.And(x => x.YieldRate == queryDto.YieldRate);
        }

        if (queryDto?.DowntimeReasonType.HasValue == true)
        {
            exp = exp.And(x => x.DowntimeReasonType == queryDto.DowntimeReasonType);
        }

        if (!string.IsNullOrEmpty(queryDto?.DowntimeReason))
        {
            exp = exp.And(x => x.DowntimeReason != null && x.DowntimeReason.Contains(queryDto.DowntimeReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentOperator))
        {
            exp = exp.And(x => x.EquipmentOperator != null && x.EquipmentOperator.Contains(queryDto.EquipmentOperator));
        }

        if (!string.IsNullOrEmpty(queryDto?.EquipmentMaintainer))
        {
            exp = exp.And(x => x.EquipmentMaintainer != null && x.EquipmentMaintainer.Contains(queryDto.EquipmentMaintainer));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamLeader))
        {
            exp = exp.And(x => x.TeamLeader != null && x.TeamLeader.Contains(queryDto.TeamLeader));
        }

        if (queryDto?.RateStatus.HasValue == true)
        {
            exp = exp.And(x => x.RateStatus == queryDto.RateStatus);
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

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.EndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EndDate >= queryDto.EndDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndDate <= queryDto.EndDateEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}

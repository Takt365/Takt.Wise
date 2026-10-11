// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktPersonnelOperationRateService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：人员稼动率应用服务实现
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
/// 人员稼动率应用服务
/// </summary>
public class TaktPersonnelOperationRateService : TaktServiceBase, ITaktPersonnelOperationRateService
{
    private readonly ITaktCompanyRepository<TaktPersonnelOperationRate> _personnelOperationRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="personnelOperationRateRepository">人员稼动率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPersonnelOperationRateService(
        ITaktCompanyRepository<TaktPersonnelOperationRate> personnelOperationRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _personnelOperationRateRepository = personnelOperationRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取人员稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPersonnelOperationRateDto>> GetPersonnelOperationRateListAsync(TaktPersonnelOperationRateQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _personnelOperationRateRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPersonnelOperationRateDto>.Create(
            data.Adapt<List<TaktPersonnelOperationRateDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateDto?> GetPersonnelOperationRateByIdAsync(long id)
    {
        var entity = await _personnelOperationRateRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPersonnelOperationRateDto>();
    }

    /// <summary>
    /// 获取人员稼动率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPersonnelOperationRateOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _personnelOperationRateRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RateStatus == 1,
            x => x.TeamCodeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TeamCodeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建人员稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateDto> CreatePersonnelOperationRateAsync(TaktPersonnelOperationRateCreateDto dto)
    {
        var entity = dto.Adapt<TaktPersonnelOperationRate>();
        var isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique = await _uniqueValidator.IsUniqueAsync(
            _personnelOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TimeCategory == entity.TimeCategory
                && x.StartDate == entity.StartDate
                && x.ShiftNo == entity.ShiftNo);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique)
        {
            throw new TaktBusinessException("人员稼动率的PlantCode、TeamCode、TimeCategory、StartDate、ShiftNo已存在");
        }
        entity = await _personnelOperationRateRepository.CreateAsync(entity);
        return await GetPersonnelOperationRateByIdAsync(entity.Id) ?? entity.Adapt<TaktPersonnelOperationRateDto>();
    }

    /// <summary>
    /// 更新人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateDto> UpdatePersonnelOperationRateAsync(long id, TaktPersonnelOperationRateUpdateDto dto)
    {
        var entity = await _personnelOperationRateRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("人员稼动率不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique = await _uniqueValidator.IsUniqueAsync(
            _personnelOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.TeamCode == entity.TeamCode
                && x.TimeCategory == entity.TimeCategory
                && x.StartDate == entity.StartDate
                && x.ShiftNo == entity.ShiftNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique)
        {
            throw new TaktBusinessException("人员稼动率的PlantCode、TeamCode、TimeCategory、StartDate、ShiftNo已存在");
        }
        await _personnelOperationRateRepository.UpdateAsync(entity);
        return await GetPersonnelOperationRateByIdAsync(id) ?? throw new TaktBusinessException("人员稼动率不存在");
    }

    /// <summary>
    /// 删除人员稼动率
    /// </summary>
    /// <param name="id">人员稼动率ID</param>
    /// <returns>任务</returns>
    public async Task DeletePersonnelOperationRateByIdAsync(long id)
    {
        var deleted = await _personnelOperationRateRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("人员稼动率不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除人员稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePersonnelOperationRateBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePersonnelOperationRateByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新人员稼动率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPersonnelOperationRateDto> UpdatePersonnelOperationRateStatusAsync(TaktPersonnelOperationRateStatusDto dto)
    {
        var entity = await _personnelOperationRateRepository.GetByIdAsync(dto.PersonnelOperationRateId);
        if (entity == null)
        {
            throw new TaktBusinessException("人员稼动率不存在");
        }
        entity.RateStatus = dto.RateStatus;
        await _personnelOperationRateRepository.UpdateAsync(entity);
        return await GetPersonnelOperationRateByIdAsync(dto.PersonnelOperationRateId) ?? throw new TaktBusinessException("人员稼动率不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPersonnelOperationRateTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPersonnelOperationRateTemplateDto>(
            sheetName ?? "人员稼动率导入模板",
            fileName ?? "人员稼动率导入模板.xlsx");
    }

    /// <summary>
    /// 导入人员稼动率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPersonnelOperationRateAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPersonnelOperationRateImportDto>(fileStream, sheetName ?? "人员稼动率导入模板");
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
                var entity = rows[i].Adapt<TaktPersonnelOperationRate>();
                var importKey = $"{entity.PlantCode}|{entity.TeamCode}|{entity.TimeCategory}|{entity.StartDate}|{entity.ShiftNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、TeamCode、TimeCategory、StartDate、ShiftNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique = await _uniqueValidator.IsUniqueAsync(
                    _personnelOperationRateRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.TeamCode == entity.TeamCode
                        && x.TimeCategory == entity.TimeCategory
                        && x.StartDate == entity.StartDate
                        && x.ShiftNo == entity.ShiftNo);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_personnel_operation_rate_por_unique)
                {
                    throw new TaktBusinessException("人员稼动率的PlantCode、TeamCode、TimeCategory、StartDate、ShiftNo已存在");
                }
                await _personnelOperationRateRepository.CreateAsync(entity);
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
    /// 导出人员稼动率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPersonnelOperationRateAsync(TaktPersonnelOperationRateQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPersonnelOperationRateQueryDto());
        var list = await _personnelOperationRateRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPersonnelOperationRateExportDto>(),
                sheetName ?? "人员稼动率数据",
                fileName ?? "人员稼动率导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPersonnelOperationRateExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "人员稼动率数据",
            fileName ?? "人员稼动率导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建人员稼动率查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPersonnelOperationRate, bool>> QueryExpression(TaktPersonnelOperationRateQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPersonnelOperationRate>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.TimeCategory).Contains(keywords)
                || SqlFunc.ToString(x.WeekNumber).Contains(keywords)
                || SqlFunc.ToString(x.MonthNumber).Contains(keywords)
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.TeamCodeName != null && x.TeamCodeName.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || SqlFunc.ToString(x.PlannedDirectPersonnelCount).Contains(keywords)
                || SqlFunc.ToString(x.ActualDirectPersonnelCount).Contains(keywords)
                || SqlFunc.ToString(x.PlannedIndirectPersonnelCount).Contains(keywords)
                || SqlFunc.ToString(x.ActualIndirectPersonnelCount).Contains(keywords)
                || SqlFunc.ToString(x.PlannedWorkTime).Contains(keywords)
                || SqlFunc.ToString(x.ActualWorkTime).Contains(keywords)
                || SqlFunc.ToString(x.BreakTime).Contains(keywords)
                || SqlFunc.ToString(x.IdleTime).Contains(keywords)
                || SqlFunc.ToString(x.PersonnelOperationRate).Contains(keywords)
                || SqlFunc.ToString(x.PlannedOutput).Contains(keywords)
                || SqlFunc.ToString(x.ActualOutput).Contains(keywords)
                || SqlFunc.ToString(x.QualifiedQuantity).Contains(keywords)
                || SqlFunc.ToString(x.DefectiveQuantity).Contains(keywords)
                || SqlFunc.ToString(x.YieldRate).Contains(keywords)
                || SqlFunc.ToString(x.WorkEfficiency).Contains(keywords)
                || SqlFunc.ToString(x.IdleReasonType).Contains(keywords)
                || (x.IdleReason != null && x.IdleReason.Contains(keywords))
                || SqlFunc.ToString(x.OvertimeHours).Contains(keywords)
                || (x.TeamLeader != null && x.TeamLeader.Contains(keywords))
                || (x.Supervisor != null && x.Supervisor.Contains(keywords))
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

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCodeName))
        {
            exp = exp.And(x => x.TeamCodeName != null && x.TeamCodeName.Contains(queryDto.TeamCodeName));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (queryDto?.PlannedDirectPersonnelCount.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDirectPersonnelCount == queryDto.PlannedDirectPersonnelCount);
        }

        if (queryDto?.ActualDirectPersonnelCount.HasValue == true)
        {
            exp = exp.And(x => x.ActualDirectPersonnelCount == queryDto.ActualDirectPersonnelCount);
        }

        if (queryDto?.PlannedIndirectPersonnelCount.HasValue == true)
        {
            exp = exp.And(x => x.PlannedIndirectPersonnelCount == queryDto.PlannedIndirectPersonnelCount);
        }

        if (queryDto?.ActualIndirectPersonnelCount.HasValue == true)
        {
            exp = exp.And(x => x.ActualIndirectPersonnelCount == queryDto.ActualIndirectPersonnelCount);
        }

        if (queryDto?.PlannedWorkTime.HasValue == true)
        {
            exp = exp.And(x => x.PlannedWorkTime == queryDto.PlannedWorkTime);
        }

        if (queryDto?.ActualWorkTime.HasValue == true)
        {
            exp = exp.And(x => x.ActualWorkTime == queryDto.ActualWorkTime);
        }

        if (queryDto?.BreakTime.HasValue == true)
        {
            exp = exp.And(x => x.BreakTime == queryDto.BreakTime);
        }

        if (queryDto?.IdleTime.HasValue == true)
        {
            exp = exp.And(x => x.IdleTime == queryDto.IdleTime);
        }

        if (queryDto?.PersonnelOperationRate.HasValue == true)
        {
            exp = exp.And(x => x.PersonnelOperationRate == queryDto.PersonnelOperationRate);
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

        if (queryDto?.WorkEfficiency.HasValue == true)
        {
            exp = exp.And(x => x.WorkEfficiency == queryDto.WorkEfficiency);
        }

        if (queryDto?.IdleReasonType.HasValue == true)
        {
            exp = exp.And(x => x.IdleReasonType == queryDto.IdleReasonType);
        }

        if (!string.IsNullOrEmpty(queryDto?.IdleReason))
        {
            exp = exp.And(x => x.IdleReason != null && x.IdleReason.Contains(queryDto.IdleReason));
        }

        if (queryDto?.OvertimeHours.HasValue == true)
        {
            exp = exp.And(x => x.OvertimeHours == queryDto.OvertimeHours);
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamLeader))
        {
            exp = exp.And(x => x.TeamLeader != null && x.TeamLeader.Contains(queryDto.TeamLeader));
        }

        if (!string.IsNullOrEmpty(queryDto?.Supervisor))
        {
            exp = exp.And(x => x.Supervisor != null && x.Supervisor.Contains(queryDto.Supervisor));
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

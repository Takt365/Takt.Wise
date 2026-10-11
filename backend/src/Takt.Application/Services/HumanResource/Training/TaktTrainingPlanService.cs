// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Training
// 文件名称：TaktTrainingPlanService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：培训计划应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Training;
using Takt.Domain.Entities.HumanResource.Training;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Training;

/// <summary>
/// 培训计划应用服务
/// </summary>
public class TaktTrainingPlanService : TaktServiceBase, ITaktTrainingPlanService
{
    private readonly ITaktApprovalRepository<TaktTrainingPlan> _trainingPlanRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingPlanRepository">培训计划仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTrainingPlanService(
        ITaktApprovalRepository<TaktTrainingPlan> trainingPlanRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _trainingPlanRepository = trainingPlanRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取培训计划列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTrainingPlanDto>> GetTrainingPlanListAsync(TaktTrainingPlanQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTrainingPlanDto>.Create(
                new List<TaktTrainingPlanDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _trainingPlanRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTrainingPlanDto>.Create(
            data.Adapt<List<TaktTrainingPlanDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingPlanDto?> GetTrainingPlanByIdAsync(long id)
    {
        var entity = await _trainingPlanRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTrainingPlanDto>();
    }

    /// <summary>
    /// 获取培训计划选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTrainingPlanOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _trainingPlanRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TrainingPlanStatus == 1,
            x => x.PlanName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PlanCode,
            DictLabel = e.PlanName ?? e.PlanCode,
        }).ToList();
    }

    /// <summary>
    /// 创建培训计划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingPlanDto> CreateTrainingPlanAsync(TaktTrainingPlanCreateDto dto)
    {
        var entity = dto.Adapt<TaktTrainingPlan>();
        var isUnique_ix_training_plan_code_unique = await _uniqueValidator.IsUniqueAsync(
            _trainingPlanRepository,
            x => x.PlanCode == entity.PlanCode);
        if (!isUnique_ix_training_plan_code_unique)
        {
            throw new TaktBusinessException("培训计划的PlanCode已存在");
        }
        entity = await _trainingPlanRepository.CreateAsync(entity);
        return await GetTrainingPlanByIdAsync(entity.Id) ?? entity.Adapt<TaktTrainingPlanDto>();
    }

    /// <summary>
    /// 更新培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingPlanDto> UpdateTrainingPlanAsync(long id, TaktTrainingPlanUpdateDto dto)
    {
        var entity = await _trainingPlanRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("培训计划不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_training_plan_code_unique = await _uniqueValidator.IsUniqueAsync(
            _trainingPlanRepository,
            x => x.PlanCode == entity.PlanCode,
            id);
        if (!isUnique_ix_training_plan_code_unique)
        {
            throw new TaktBusinessException("培训计划的PlanCode已存在");
        }
        await _trainingPlanRepository.UpdateAsync(entity);
        return await GetTrainingPlanByIdAsync(id) ?? throw new TaktBusinessException("培训计划不存在");
    }

    /// <summary>
    /// 删除培训计划
    /// </summary>
    /// <param name="id">培训计划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingPlanByIdAsync(long id)
    {
        var deleted = await _trainingPlanRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("培训计划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除培训计划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingPlanBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTrainingPlanByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新培训计划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingPlanDto> UpdateTrainingPlanStatusAsync(TaktTrainingPlanStatusDto dto)
    {
        var entity = await _trainingPlanRepository.GetByIdAsync(dto.TrainingPlanId);
        if (entity == null)
        {
            throw new TaktBusinessException("培训计划不存在");
        }
        entity.TrainingPlanStatus = dto.TrainingPlanStatus;
        await _trainingPlanRepository.UpdateAsync(entity);
        return await GetTrainingPlanByIdAsync(dto.TrainingPlanId) ?? throw new TaktBusinessException("培训计划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTrainingPlanTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTrainingPlanTemplateDto>(
            sheetName ?? "培训计划导入模板",
            fileName ?? "培训计划导入模板.xlsx");
    }

    /// <summary>
    /// 导入培训计划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTrainingPlanAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTrainingPlanImportDto>(fileStream, sheetName ?? "培训计划导入模板");
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
                var entity = rows[i].Adapt<TaktTrainingPlan>();
                var importKey = $"{entity.PlanCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlanCode）");
                }
                var isUnique_ix_training_plan_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _trainingPlanRepository,
                    x => x.PlanCode == entity.PlanCode);
                if (!isUnique_ix_training_plan_code_unique)
                {
                    throw new TaktBusinessException("培训计划的PlanCode已存在");
                }
                await _trainingPlanRepository.CreateAsync(entity);
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
    /// 导出培训计划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTrainingPlanAsync(TaktTrainingPlanQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTrainingPlanQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrainingPlanExportDto>(),
                sheetName ?? "培训计划数据",
                fileName ?? "培训计划导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _trainingPlanRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrainingPlanExportDto>(),
                sheetName ?? "培训计划数据",
                fileName ?? "培训计划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTrainingPlanExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "培训计划数据",
            fileName ?? "培训计划导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建培训计划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTrainingPlan, bool>> QueryExpression(TaktTrainingPlanQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTrainingPlan>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PlanCode != null && x.PlanCode.Contains(keywords))
                || (x.PlanName != null && x.PlanName.Contains(keywords))
                || (x.PlanType != null && x.PlanType.Contains(keywords))
                || (x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(keywords))
                || (x.TrainingObjectives != null && x.TrainingObjectives.Contains(keywords))
                || (x.TrainingPlanDescription != null && x.TrainingPlanDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanCode))
        {
            var planCode = queryDto.PlanCode;
            exp = exp.And(x => x.PlanCode != null && x.PlanCode.Contains(planCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanName))
        {
            var planName = queryDto.PlanName;
            exp = exp.And(x => x.PlanName != null && x.PlanName.Contains(planName));
        }

        if (queryDto?.PlanYear.HasValue == true)
        {
            var planYear = queryDto.PlanYear.Value;
            exp = exp.And(x => x.PlanYear == planYear);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanType))
        {
            var planType = queryDto.PlanType;
            exp = exp.And(x => x.PlanType != null && x.PlanType.Contains(planType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicableDepartment))
        {
            var applicableDepartment = queryDto.ApplicableDepartment;
            exp = exp.And(x => x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(applicableDepartment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TrainingObjectives))
        {
            var trainingObjectives = queryDto.TrainingObjectives;
            exp = exp.And(x => x.TrainingObjectives != null && x.TrainingObjectives.Contains(trainingObjectives));
        }

        if (queryDto?.PlannedHeadcount.HasValue == true)
        {
            var plannedHeadcount = queryDto.PlannedHeadcount.Value;
            exp = exp.And(x => x.PlannedHeadcount == plannedHeadcount);
        }

        if (queryDto?.TrainingBudget.HasValue == true)
        {
            var trainingBudget = queryDto.TrainingBudget.Value;
            exp = exp.And(x => x.TrainingBudget == trainingBudget);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TrainingPlanDescription))
        {
            var trainingPlanDescription = queryDto.TrainingPlanDescription;
            exp = exp.And(x => x.TrainingPlanDescription != null && x.TrainingPlanDescription.Contains(trainingPlanDescription));
        }

        if (queryDto?.TrainingPlanStatus.HasValue == true)
        {
            var trainingPlanStatus = queryDto.TrainingPlanStatus.Value;
            exp = exp.And(x => x.TrainingPlanStatus == trainingPlanStatus);
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

        if (queryDto?.EndDateStart.HasValue == true)
        {
            var endDateStart = queryDto.EndDateStart.Value;
            exp = exp.And(x => x.EndDate >= endDateStart);
        }

        if (queryDto?.EndDateEnd.HasValue == true)
        {
            var endDateEnd = queryDto.EndDateEnd.Value;
            exp = exp.And(x => x.EndDate <= endDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktTrainingPlanQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PlanCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanName))
        {
            return true;
        }
        if (queryDto.PlanYear.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicableDepartment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TrainingObjectives))
        {
            return true;
        }
        if (queryDto.PlannedHeadcount.HasValue)
        {
            return true;
        }
        if (queryDto.TrainingBudget.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TrainingPlanDescription))
        {
            return true;
        }
        if (queryDto.TrainingPlanStatus.HasValue)
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
        if (queryDto.StartDateStart.HasValue || queryDto.StartDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndDateStart.HasValue || queryDto.EndDateEnd.HasValue)
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

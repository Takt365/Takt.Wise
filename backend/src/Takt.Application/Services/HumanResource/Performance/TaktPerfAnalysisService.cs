// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktPerfAnalysisService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：分析改进应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Performance;
using Takt.Domain.Entities.HumanResource.Performance;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Performance;

/// <summary>
/// 分析改进应用服务
/// </summary>
public class TaktPerfAnalysisService : TaktServiceBase, ITaktPerfAnalysisService
{
    private readonly ITaktApprovalRepository<TaktPerfAnalysis> _perfAnalysisRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfAnalysisRepository">分析改进仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPerfAnalysisService(
        ITaktApprovalRepository<TaktPerfAnalysis> perfAnalysisRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _perfAnalysisRepository = perfAnalysisRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取分析改进列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPerfAnalysisDto>> GetPerfAnalysisListAsync(TaktPerfAnalysisQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _perfAnalysisRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPerfAnalysisDto>.Create(
            data.Adapt<List<TaktPerfAnalysisDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAnalysisDto?> GetPerfAnalysisByIdAsync(long id)
    {
        var entity = await _perfAnalysisRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPerfAnalysisDto>();
    }

    /// <summary>
    /// 获取分析改进选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPerfAnalysisOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _perfAnalysisRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ImprovementStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建分析改进
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAnalysisDto> CreatePerfAnalysisAsync(TaktPerfAnalysisCreateDto dto)
    {
        var entity = dto.Adapt<TaktPerfAnalysis>();
        entity = await _perfAnalysisRepository.CreateAsync(entity);
        return await GetPerfAnalysisByIdAsync(entity.Id) ?? entity.Adapt<TaktPerfAnalysisDto>();
    }

    /// <summary>
    /// 更新分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAnalysisDto> UpdatePerfAnalysisAsync(long id, TaktPerfAnalysisUpdateDto dto)
    {
        var entity = await _perfAnalysisRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("分析改进不存在");
        }
        dto.Adapt(entity);
        await _perfAnalysisRepository.UpdateAsync(entity);
        return await GetPerfAnalysisByIdAsync(id) ?? throw new TaktBusinessException("分析改进不存在");
    }

    /// <summary>
    /// 删除分析改进
    /// </summary>
    /// <param name="id">分析改进ID</param>
    /// <returns>任务</returns>
    public async Task DeletePerfAnalysisByIdAsync(long id)
    {
        var deleted = await _perfAnalysisRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("分析改进不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除分析改进
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePerfAnalysisBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePerfAnalysisByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新分析改进状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAnalysisDto> UpdatePerfAnalysisStatusAsync(TaktPerfAnalysisStatusDto dto)
    {
        var entity = await _perfAnalysisRepository.GetByIdAsync(dto.PerfAnalysisId);
        if (entity == null)
        {
            throw new TaktBusinessException("分析改进不存在");
        }
        entity.ImprovementStatus = dto.ImprovementStatus;
        await _perfAnalysisRepository.UpdateAsync(entity);
        return await GetPerfAnalysisByIdAsync(dto.PerfAnalysisId) ?? throw new TaktBusinessException("分析改进不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPerfAnalysisTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPerfAnalysisTemplateDto>(
            sheetName ?? "分析改进导入模板",
            fileName ?? "分析改进导入模板.xlsx");
    }

    /// <summary>
    /// 导入分析改进
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPerfAnalysisAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPerfAnalysisImportDto>(fileStream, sheetName ?? "分析改进导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPerfAnalysis>();
                await _perfAnalysisRepository.CreateAsync(entity);
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
    /// 导出分析改进
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPerfAnalysisAsync(TaktPerfAnalysisQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPerfAnalysisQueryDto());
        var list = await _perfAnalysisRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPerfAnalysisExportDto>(),
                sheetName ?? "分析改进数据",
                fileName ?? "分析改进导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPerfAnalysisExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "分析改进数据",
            fileName ?? "分析改进导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建分析改进查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPerfAnalysis, bool>> QueryExpression(TaktPerfAnalysisQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPerfAnalysis>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentId).Contains(keywords)
                || (x.PlanTitle != null && x.PlanTitle.Contains(keywords))
                || (x.ImprovementArea != null && x.ImprovementArea.Contains(keywords))
                || (x.CurrentSituation != null && x.CurrentSituation.Contains(keywords))
                || (x.ImprovementGoal != null && x.ImprovementGoal.Contains(keywords))
                || (x.ImprovementActions != null && x.ImprovementActions.Contains(keywords))
                || SqlFunc.ToString(x.ProgressPercentage).Contains(keywords)
                || (x.ResultDescription != null && x.ResultDescription.Contains(keywords))
                || SqlFunc.ToString(x.MentorId).Contains(keywords)
                || SqlFunc.ToString(x.ImprovementStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.PlanDate).Contains(keywords)
                || SqlFunc.ToString(x.TargetCompletionDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.EmployeeId.HasValue == true)
        {
            exp = exp.And(x => x.EmployeeId == queryDto.EmployeeId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EmployeeName))
        {
            exp = exp.And(x => x.EmployeeName != null && x.EmployeeName.Contains(queryDto.EmployeeName));
        }

        if (queryDto?.AssessmentId.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentId == queryDto.AssessmentId);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlanTitle))
        {
            exp = exp.And(x => x.PlanTitle != null && x.PlanTitle.Contains(queryDto.PlanTitle));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementArea))
        {
            exp = exp.And(x => x.ImprovementArea != null && x.ImprovementArea.Contains(queryDto.ImprovementArea));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrentSituation))
        {
            exp = exp.And(x => x.CurrentSituation != null && x.CurrentSituation.Contains(queryDto.CurrentSituation));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementGoal))
        {
            exp = exp.And(x => x.ImprovementGoal != null && x.ImprovementGoal.Contains(queryDto.ImprovementGoal));
        }

        if (!string.IsNullOrEmpty(queryDto?.ImprovementActions))
        {
            exp = exp.And(x => x.ImprovementActions != null && x.ImprovementActions.Contains(queryDto.ImprovementActions));
        }

        if (queryDto?.ProgressPercentage.HasValue == true)
        {
            exp = exp.And(x => x.ProgressPercentage == queryDto.ProgressPercentage);
        }

        if (!string.IsNullOrEmpty(queryDto?.ResultDescription))
        {
            exp = exp.And(x => x.ResultDescription != null && x.ResultDescription.Contains(queryDto.ResultDescription));
        }

        if (queryDto?.MentorId.HasValue == true)
        {
            exp = exp.And(x => x.MentorId == queryDto.MentorId);
        }

        if (queryDto?.ImprovementStatus.HasValue == true)
        {
            exp = exp.And(x => x.ImprovementStatus == queryDto.ImprovementStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate >= queryDto.PlanDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.PlanDate <= queryDto.PlanDateEnd);
        }

        if (queryDto?.TargetCompletionDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TargetCompletionDate >= queryDto.TargetCompletionDateStart);
        }

        if (queryDto?.TargetCompletionDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TargetCompletionDate <= queryDto.TargetCompletionDateEnd);
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

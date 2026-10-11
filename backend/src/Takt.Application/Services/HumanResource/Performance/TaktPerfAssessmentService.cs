// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktPerfAssessmentService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效考核应用服务实现
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
/// 绩效考核应用服务
/// </summary>
public class TaktPerfAssessmentService : TaktServiceBase, ITaktPerfAssessmentService
{
    private readonly ITaktCompanyRepository<TaktPerfAssessment> _perfAssessmentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfAssessmentRepository">绩效考核仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPerfAssessmentService(
        ITaktCompanyRepository<TaktPerfAssessment> perfAssessmentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _perfAssessmentRepository = perfAssessmentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效考核列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPerfAssessmentDto>> GetPerfAssessmentListAsync(TaktPerfAssessmentQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _perfAssessmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPerfAssessmentDto>.Create(
            data.Adapt<List<TaktPerfAssessmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAssessmentDto?> GetPerfAssessmentByIdAsync(long id)
    {
        var entity = await _perfAssessmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPerfAssessmentDto>();
    }

    /// <summary>
    /// 获取绩效考核选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPerfAssessmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _perfAssessmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AssessmentStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效考核
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAssessmentDto> CreatePerfAssessmentAsync(TaktPerfAssessmentCreateDto dto)
    {
        var entity = dto.Adapt<TaktPerfAssessment>();
        entity = await _perfAssessmentRepository.CreateAsync(entity);
        return await GetPerfAssessmentByIdAsync(entity.Id) ?? entity.Adapt<TaktPerfAssessmentDto>();
    }

    /// <summary>
    /// 更新绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAssessmentDto> UpdatePerfAssessmentAsync(long id, TaktPerfAssessmentUpdateDto dto)
    {
        var entity = await _perfAssessmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效考核不存在");
        }
        dto.Adapt(entity);
        await _perfAssessmentRepository.UpdateAsync(entity);
        return await GetPerfAssessmentByIdAsync(id) ?? throw new TaktBusinessException("绩效考核不存在");
    }

    /// <summary>
    /// 删除绩效考核
    /// </summary>
    /// <param name="id">绩效考核ID</param>
    /// <returns>任务</returns>
    public async Task DeletePerfAssessmentByIdAsync(long id)
    {
        var deleted = await _perfAssessmentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效考核不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效考核
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePerfAssessmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePerfAssessmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效考核状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfAssessmentDto> UpdatePerfAssessmentStatusAsync(TaktPerfAssessmentStatusDto dto)
    {
        var entity = await _perfAssessmentRepository.GetByIdAsync(dto.PerfAssessmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效考核不存在");
        }
        entity.AssessmentStatus = dto.AssessmentStatus;
        await _perfAssessmentRepository.UpdateAsync(entity);
        return await GetPerfAssessmentByIdAsync(dto.PerfAssessmentId) ?? throw new TaktBusinessException("绩效考核不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPerfAssessmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPerfAssessmentTemplateDto>(
            sheetName ?? "绩效考核导入模板",
            fileName ?? "绩效考核导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效考核
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPerfAssessmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPerfAssessmentImportDto>(fileStream, sheetName ?? "绩效考核导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPerfAssessment>();
                await _perfAssessmentRepository.CreateAsync(entity);
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
    /// 导出绩效考核
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPerfAssessmentAsync(TaktPerfAssessmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPerfAssessmentQueryDto());
        var list = await _perfAssessmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPerfAssessmentExportDto>(),
                sheetName ?? "绩效考核数据",
                fileName ?? "绩效考核导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPerfAssessmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效考核数据",
            fileName ?? "绩效考核导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效考核查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPerfAssessment, bool>> QueryExpression(TaktPerfAssessmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPerfAssessment>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || (x.AssessmentPeriod != null && x.AssessmentPeriod.Contains(keywords))
                || SqlFunc.ToString(x.SchemeMetricId).Contains(keywords)
                || SqlFunc.ToString(x.SelfScore).Contains(keywords)
                || (x.SelfEvaluationNotes != null && x.SelfEvaluationNotes.Contains(keywords))
                || SqlFunc.ToString(x.SupervisorScore).Contains(keywords)
                || (x.SupervisorComments != null && x.SupervisorComments.Contains(keywords))
                || SqlFunc.ToString(x.FinalScore).Contains(keywords)
                || (x.PerformanceGrade != null && x.PerformanceGrade.Contains(keywords))
                || SqlFunc.ToString(x.ReviewerId).Contains(keywords)
                || (x.InterviewNotes != null && x.InterviewNotes.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.AssessmentDate).Contains(keywords)
                || SqlFunc.ToString(x.InterviewDate).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.AssessmentPeriod))
        {
            exp = exp.And(x => x.AssessmentPeriod != null && x.AssessmentPeriod.Contains(queryDto.AssessmentPeriod));
        }

        if (queryDto?.SchemeMetricId.HasValue == true)
        {
            exp = exp.And(x => x.SchemeMetricId == queryDto.SchemeMetricId);
        }

        if (queryDto?.SelfScore.HasValue == true)
        {
            exp = exp.And(x => x.SelfScore == queryDto.SelfScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.SelfEvaluationNotes))
        {
            exp = exp.And(x => x.SelfEvaluationNotes != null && x.SelfEvaluationNotes.Contains(queryDto.SelfEvaluationNotes));
        }

        if (queryDto?.SupervisorScore.HasValue == true)
        {
            exp = exp.And(x => x.SupervisorScore == queryDto.SupervisorScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.SupervisorComments))
        {
            exp = exp.And(x => x.SupervisorComments != null && x.SupervisorComments.Contains(queryDto.SupervisorComments));
        }

        if (queryDto?.FinalScore.HasValue == true)
        {
            exp = exp.And(x => x.FinalScore == queryDto.FinalScore);
        }

        if (!string.IsNullOrEmpty(queryDto?.PerformanceGrade))
        {
            exp = exp.And(x => x.PerformanceGrade != null && x.PerformanceGrade.Contains(queryDto.PerformanceGrade));
        }

        if (queryDto?.ReviewerId.HasValue == true)
        {
            exp = exp.And(x => x.ReviewerId == queryDto.ReviewerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.InterviewNotes))
        {
            exp = exp.And(x => x.InterviewNotes != null && x.InterviewNotes.Contains(queryDto.InterviewNotes));
        }

        if (queryDto?.AssessmentStatus.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentStatus == queryDto.AssessmentStatus);
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

        if (queryDto?.AssessmentDateStart.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentDate >= queryDto.AssessmentDateStart);
        }

        if (queryDto?.AssessmentDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.AssessmentDate <= queryDto.AssessmentDateEnd);
        }

        if (queryDto?.InterviewDateStart.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDate >= queryDto.InterviewDateStart);
        }

        if (queryDto?.InterviewDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.InterviewDate <= queryDto.InterviewDateEnd);
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

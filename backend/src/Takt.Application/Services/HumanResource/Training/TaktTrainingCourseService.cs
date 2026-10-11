// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Training
// 文件名称：TaktTrainingCourseService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：培训课程应用服务实现
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
/// 培训课程应用服务
/// </summary>
public class TaktTrainingCourseService : TaktServiceBase, ITaktTrainingCourseService
{
    private readonly ITaktCompanyRepository<TaktTrainingCourse> _trainingCourseRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingCourseRepository">培训课程仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTrainingCourseService(
        ITaktCompanyRepository<TaktTrainingCourse> trainingCourseRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _trainingCourseRepository = trainingCourseRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取培训课程列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTrainingCourseDto>> GetTrainingCourseListAsync(TaktTrainingCourseQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _trainingCourseRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTrainingCourseDto>.Create(
            data.Adapt<List<TaktTrainingCourseDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingCourseDto?> GetTrainingCourseByIdAsync(long id)
    {
        var entity = await _trainingCourseRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTrainingCourseDto>();
    }

    /// <summary>
    /// 获取培训课程选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTrainingCourseOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _trainingCourseRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TrainingCourseStatus == 1,
            x => x.CourseName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.CourseName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建培训课程
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingCourseDto> CreateTrainingCourseAsync(TaktTrainingCourseCreateDto dto)
    {
        var entity = dto.Adapt<TaktTrainingCourse>();
        var isUnique_ix_training_course_code_unique = await _uniqueValidator.IsUniqueAsync(
            _trainingCourseRepository,
            x => x.CourseCode == entity.CourseCode);
        if (!isUnique_ix_training_course_code_unique)
        {
            throw new TaktBusinessException("培训课程的CourseCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _trainingCourseRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _trainingCourseRepository.CreateAsync(entity);
        return await GetTrainingCourseByIdAsync(entity.Id) ?? entity.Adapt<TaktTrainingCourseDto>();
    }

    /// <summary>
    /// 更新培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingCourseDto> UpdateTrainingCourseAsync(long id, TaktTrainingCourseUpdateDto dto)
    {
        var entity = await _trainingCourseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("培训课程不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_training_course_code_unique = await _uniqueValidator.IsUniqueAsync(
            _trainingCourseRepository,
            x => x.CourseCode == entity.CourseCode,
            id);
        if (!isUnique_ix_training_course_code_unique)
        {
            throw new TaktBusinessException("培训课程的CourseCode已存在");
        }
        await _trainingCourseRepository.UpdateAsync(entity);
        return await GetTrainingCourseByIdAsync(id) ?? throw new TaktBusinessException("培训课程不存在");
    }

    /// <summary>
    /// 删除培训课程
    /// </summary>
    /// <param name="id">培训课程ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingCourseByIdAsync(long id)
    {
        var deleted = await _trainingCourseRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("培训课程不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除培训课程
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingCourseBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTrainingCourseByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新培训课程状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingCourseDto> UpdateTrainingCourseStatusAsync(TaktTrainingCourseStatusDto dto)
    {
        var entity = await _trainingCourseRepository.GetByIdAsync(dto.TrainingCourseId);
        if (entity == null)
        {
            throw new TaktBusinessException("培训课程不存在");
        }
        entity.TrainingCourseStatus = dto.TrainingCourseStatus;
        await _trainingCourseRepository.UpdateAsync(entity);
        return await GetTrainingCourseByIdAsync(dto.TrainingCourseId) ?? throw new TaktBusinessException("培训课程不存在");
    }

    /// <summary>
    /// 更新培训课程排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingCourseDto> UpdateTrainingCourseSortAsync(TaktTrainingCourseSortDto dto)
    {
        var entity = await _trainingCourseRepository.GetByIdAsync(dto.TrainingCourseId);
        if (entity == null)
        {
            throw new TaktBusinessException("培训课程不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _trainingCourseRepository.UpdateAsync(entity);
        return await GetTrainingCourseByIdAsync(dto.TrainingCourseId) ?? throw new TaktBusinessException("培训课程不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTrainingCourseTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTrainingCourseTemplateDto>(
            sheetName ?? "培训课程导入模板",
            fileName ?? "培训课程导入模板.xlsx");
    }

    /// <summary>
    /// 导入培训课程
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTrainingCourseAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTrainingCourseImportDto>(fileStream, sheetName ?? "培训课程导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _trainingCourseRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTrainingCourse>();
                var importKey = $"{entity.CourseCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CourseCode）");
                }
                var isUnique_ix_training_course_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _trainingCourseRepository,
                    x => x.CourseCode == entity.CourseCode);
                if (!isUnique_ix_training_course_code_unique)
                {
                    throw new TaktBusinessException("培训课程的CourseCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _trainingCourseRepository.CreateAsync(entity);
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
    /// 导出培训课程
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTrainingCourseAsync(TaktTrainingCourseQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTrainingCourseQueryDto());
        var list = await _trainingCourseRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrainingCourseExportDto>(),
                sheetName ?? "培训课程数据",
                fileName ?? "培训课程导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTrainingCourseExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "培训课程数据",
            fileName ?? "培训课程导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建培训课程查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTrainingCourse, bool>> QueryExpression(TaktTrainingCourseQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTrainingCourse>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CourseCode != null && x.CourseCode.Contains(keywords))
                || (x.CourseName != null && x.CourseName.Contains(keywords))
                || (x.CourseType != null && x.CourseType.Contains(keywords))
                || (x.CourseLevel != null && x.CourseLevel.Contains(keywords))
                || (x.CourseDescription != null && x.CourseDescription.Contains(keywords))
                || (x.CourseObjectives != null && x.CourseObjectives.Contains(keywords))
                || SqlFunc.ToString(x.TrainingHours).Contains(keywords)
                || (x.MainInstructor != null && x.MainInstructor.Contains(keywords))
                || (x.TrainingMethod != null && x.TrainingMethod.Contains(keywords))
                || (x.AssessmentMethod != null && x.AssessmentMethod.Contains(keywords))
                || SqlFunc.ToString(x.PassingScore).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.TrainingCourseStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseCode))
        {
            exp = exp.And(x => x.CourseCode != null && x.CourseCode.Contains(queryDto.CourseCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseName))
        {
            exp = exp.And(x => x.CourseName != null && x.CourseName.Contains(queryDto.CourseName));
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseType))
        {
            exp = exp.And(x => x.CourseType != null && x.CourseType.Contains(queryDto.CourseType));
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseLevel))
        {
            exp = exp.And(x => x.CourseLevel != null && x.CourseLevel.Contains(queryDto.CourseLevel));
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseDescription))
        {
            exp = exp.And(x => x.CourseDescription != null && x.CourseDescription.Contains(queryDto.CourseDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseObjectives))
        {
            exp = exp.And(x => x.CourseObjectives != null && x.CourseObjectives.Contains(queryDto.CourseObjectives));
        }

        if (queryDto?.TrainingHours.HasValue == true)
        {
            exp = exp.And(x => x.TrainingHours == queryDto.TrainingHours);
        }

        if (!string.IsNullOrEmpty(queryDto?.MainInstructor))
        {
            exp = exp.And(x => x.MainInstructor != null && x.MainInstructor.Contains(queryDto.MainInstructor));
        }

        if (!string.IsNullOrEmpty(queryDto?.TrainingMethod))
        {
            exp = exp.And(x => x.TrainingMethod != null && x.TrainingMethod.Contains(queryDto.TrainingMethod));
        }

        if (!string.IsNullOrEmpty(queryDto?.AssessmentMethod))
        {
            exp = exp.And(x => x.AssessmentMethod != null && x.AssessmentMethod.Contains(queryDto.AssessmentMethod));
        }

        if (queryDto?.PassingScore.HasValue == true)
        {
            exp = exp.And(x => x.PassingScore == queryDto.PassingScore);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.TrainingCourseStatus.HasValue == true)
        {
            exp = exp.And(x => x.TrainingCourseStatus == queryDto.TrainingCourseStatus);
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

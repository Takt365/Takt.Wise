// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Training
// 文件名称：TaktTrainingAttendeeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：培训参训记录应用服务实现
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
/// 培训参训记录应用服务
/// </summary>
public class TaktTrainingAttendeeService : TaktServiceBase, ITaktTrainingAttendeeService
{
    private readonly ITaktCompanyRepository<TaktTrainingAttendee> _trainingAttendeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="trainingAttendeeRepository">培训参训记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTrainingAttendeeService(
        ITaktCompanyRepository<TaktTrainingAttendee> trainingAttendeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _trainingAttendeeRepository = trainingAttendeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取培训参训记录列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTrainingAttendeeDto>> GetTrainingAttendeeListAsync(TaktTrainingAttendeeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _trainingAttendeeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTrainingAttendeeDto>.Create(
            data.Adapt<List<TaktTrainingAttendeeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingAttendeeDto?> GetTrainingAttendeeByIdAsync(long id)
    {
        var entity = await _trainingAttendeeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTrainingAttendeeDto>();
    }

    /// <summary>
    /// 获取培训结果选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTrainingAttendeeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _trainingAttendeeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TrainingResultStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建培训参训记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingAttendeeDto> CreateTrainingAttendeeAsync(TaktTrainingAttendeeCreateDto dto)
    {
        var entity = dto.Adapt<TaktTrainingAttendee>();
        entity = await _trainingAttendeeRepository.CreateAsync(entity);
        return await GetTrainingAttendeeByIdAsync(entity.Id) ?? entity.Adapt<TaktTrainingAttendeeDto>();
    }

    /// <summary>
    /// 更新培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingAttendeeDto> UpdateTrainingAttendeeAsync(long id, TaktTrainingAttendeeUpdateDto dto)
    {
        var entity = await _trainingAttendeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("培训参训记录不存在");
        }
        dto.Adapt(entity);
        await _trainingAttendeeRepository.UpdateAsync(entity);
        return await GetTrainingAttendeeByIdAsync(id) ?? throw new TaktBusinessException("培训参训记录不存在");
    }

    /// <summary>
    /// 删除培训参训记录
    /// </summary>
    /// <param name="id">培训参训记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingAttendeeByIdAsync(long id)
    {
        var deleted = await _trainingAttendeeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("培训参训记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除培训参训记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTrainingAttendeeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTrainingAttendeeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新培训参训记录状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTrainingAttendeeDto> UpdateTrainingAttendeeStatusAsync(TaktTrainingAttendeeStatusDto dto)
    {
        var entity = await _trainingAttendeeRepository.GetByIdAsync(dto.TrainingAttendeeId);
        if (entity == null)
        {
            throw new TaktBusinessException("培训参训记录不存在");
        }
        entity.TrainingResultStatus = dto.TrainingResultStatus;
        await _trainingAttendeeRepository.UpdateAsync(entity);
        return await GetTrainingAttendeeByIdAsync(dto.TrainingAttendeeId) ?? throw new TaktBusinessException("培训参训记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTrainingAttendeeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTrainingAttendeeTemplateDto>(
            sheetName ?? "培训参训记录导入模板",
            fileName ?? "培训参训记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入培训参训记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTrainingAttendeeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTrainingAttendeeImportDto>(fileStream, sheetName ?? "培训参训记录导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTrainingAttendee>();
                await _trainingAttendeeRepository.CreateAsync(entity);
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
    /// 导出培训参训记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTrainingAttendeeAsync(TaktTrainingAttendeeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTrainingAttendeeQueryDto());
        var list = await _trainingAttendeeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTrainingAttendeeExportDto>(),
                sheetName ?? "培训参训记录数据",
                fileName ?? "培训参训记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTrainingAttendeeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "培训参训记录数据",
            fileName ?? "培训参训记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建培训参训记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTrainingAttendee, bool>> QueryExpression(TaktTrainingAttendeeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTrainingAttendee>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.TrainingCourseId).Contains(keywords)
                || (x.CourseName != null && x.CourseName.Contains(keywords))
                || (x.TrainingType != null && x.TrainingType.Contains(keywords))
                || (x.Instructor != null && x.Instructor.Contains(keywords))
                || SqlFunc.ToString(x.TrainingHours).Contains(keywords)
                || SqlFunc.ToString(x.TrainingScore).Contains(keywords)
                || SqlFunc.ToString(x.IsPassed).Contains(keywords)
                || (x.CertificateCode != null && x.CertificateCode.Contains(keywords))
                || (x.TrainingEvaluation != null && x.TrainingEvaluation.Contains(keywords))
                || SqlFunc.ToString(x.TrainingResultStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.TrainingStartDate).Contains(keywords)
                || SqlFunc.ToString(x.TrainingEndDate).Contains(keywords)
                || SqlFunc.ToString(x.TrainingDate).Contains(keywords)
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

        if (queryDto?.TrainingCourseId.HasValue == true)
        {
            exp = exp.And(x => x.TrainingCourseId == queryDto.TrainingCourseId);
        }

        if (!string.IsNullOrEmpty(queryDto?.CourseName))
        {
            exp = exp.And(x => x.CourseName != null && x.CourseName.Contains(queryDto.CourseName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TrainingType))
        {
            exp = exp.And(x => x.TrainingType != null && x.TrainingType.Contains(queryDto.TrainingType));
        }

        if (!string.IsNullOrEmpty(queryDto?.Instructor))
        {
            exp = exp.And(x => x.Instructor != null && x.Instructor.Contains(queryDto.Instructor));
        }

        if (queryDto?.TrainingHours.HasValue == true)
        {
            exp = exp.And(x => x.TrainingHours == queryDto.TrainingHours);
        }

        if (queryDto?.TrainingScore.HasValue == true)
        {
            exp = exp.And(x => x.TrainingScore == queryDto.TrainingScore);
        }

        if (queryDto?.IsPassed.HasValue == true)
        {
            exp = exp.And(x => x.IsPassed == queryDto.IsPassed);
        }

        if (!string.IsNullOrEmpty(queryDto?.CertificateCode))
        {
            exp = exp.And(x => x.CertificateCode != null && x.CertificateCode.Contains(queryDto.CertificateCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.TrainingEvaluation))
        {
            exp = exp.And(x => x.TrainingEvaluation != null && x.TrainingEvaluation.Contains(queryDto.TrainingEvaluation));
        }

        if (queryDto?.TrainingResultStatus.HasValue == true)
        {
            exp = exp.And(x => x.TrainingResultStatus == queryDto.TrainingResultStatus);
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

        if (queryDto?.TrainingStartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingStartDate >= queryDto.TrainingStartDateStart);
        }

        if (queryDto?.TrainingStartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingStartDate <= queryDto.TrainingStartDateEnd);
        }

        if (queryDto?.TrainingEndDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingEndDate >= queryDto.TrainingEndDateStart);
        }

        if (queryDto?.TrainingEndDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingEndDate <= queryDto.TrainingEndDateEnd);
        }

        if (queryDto?.TrainingDateStart.HasValue == true)
        {
            exp = exp.And(x => x.TrainingDate >= queryDto.TrainingDateStart);
        }

        if (queryDto?.TrainingDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.TrainingDate <= queryDto.TrainingDateEnd);
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

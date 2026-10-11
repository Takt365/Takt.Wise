// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktPerfObjectiveService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效目标应用服务实现
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
/// 绩效目标应用服务
/// </summary>
public class TaktPerfObjectiveService : TaktServiceBase, ITaktPerfObjectiveService
{
    private readonly ITaktApprovalRepository<TaktPerfObjective> _perfObjectiveRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfObjectiveRepository">绩效目标仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPerfObjectiveService(
        ITaktApprovalRepository<TaktPerfObjective> perfObjectiveRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _perfObjectiveRepository = perfObjectiveRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效目标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPerfObjectiveDto>> GetPerfObjectiveListAsync(TaktPerfObjectiveQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _perfObjectiveRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPerfObjectiveDto>.Create(
            data.Adapt<List<TaktPerfObjectiveDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfObjectiveDto?> GetPerfObjectiveByIdAsync(long id)
    {
        var entity = await _perfObjectiveRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPerfObjectiveDto>();
    }

    /// <summary>
    /// 获取绩效目标选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPerfObjectiveOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _perfObjectiveRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ObjectiveStatus == 1,
            x => x.EmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EmployeeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效目标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfObjectiveDto> CreatePerfObjectiveAsync(TaktPerfObjectiveCreateDto dto)
    {
        var entity = dto.Adapt<TaktPerfObjective>();
        entity = await _perfObjectiveRepository.CreateAsync(entity);
        return await GetPerfObjectiveByIdAsync(entity.Id) ?? entity.Adapt<TaktPerfObjectiveDto>();
    }

    /// <summary>
    /// 更新绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfObjectiveDto> UpdatePerfObjectiveAsync(long id, TaktPerfObjectiveUpdateDto dto)
    {
        var entity = await _perfObjectiveRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效目标不存在");
        }
        dto.Adapt(entity);
        await _perfObjectiveRepository.UpdateAsync(entity);
        return await GetPerfObjectiveByIdAsync(id) ?? throw new TaktBusinessException("绩效目标不存在");
    }

    /// <summary>
    /// 删除绩效目标
    /// </summary>
    /// <param name="id">绩效目标ID</param>
    /// <returns>任务</returns>
    public async Task DeletePerfObjectiveByIdAsync(long id)
    {
        var deleted = await _perfObjectiveRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效目标不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效目标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePerfObjectiveBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePerfObjectiveByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效目标状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfObjectiveDto> UpdatePerfObjectiveStatusAsync(TaktPerfObjectiveStatusDto dto)
    {
        var entity = await _perfObjectiveRepository.GetByIdAsync(dto.PerfObjectiveId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效目标不存在");
        }
        entity.ObjectiveStatus = dto.ObjectiveStatus;
        await _perfObjectiveRepository.UpdateAsync(entity);
        return await GetPerfObjectiveByIdAsync(dto.PerfObjectiveId) ?? throw new TaktBusinessException("绩效目标不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPerfObjectiveTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPerfObjectiveTemplateDto>(
            sheetName ?? "绩效目标导入模板",
            fileName ?? "绩效目标导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效目标
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPerfObjectiveAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPerfObjectiveImportDto>(fileStream, sheetName ?? "绩效目标导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPerfObjective>();
                await _perfObjectiveRepository.CreateAsync(entity);
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
    /// 导出绩效目标
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPerfObjectiveAsync(TaktPerfObjectiveQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPerfObjectiveQueryDto());
        var list = await _perfObjectiveRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPerfObjectiveExportDto>(),
                sheetName ?? "绩效目标数据",
                fileName ?? "绩效目标导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPerfObjectiveExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效目标数据",
            fileName ?? "绩效目标导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效目标查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPerfObjective, bool>> QueryExpression(TaktPerfObjectiveQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPerfObjective>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.EmployeeId).Contains(keywords)
                || (x.EmployeeName != null && x.EmployeeName.Contains(keywords))
                || SqlFunc.ToString(x.SchemeMetricId).Contains(keywords)
                || (x.ObjectivePeriod != null && x.ObjectivePeriod.Contains(keywords))
                || (x.ObjectiveDescription != null && x.ObjectiveDescription.Contains(keywords))
                || SqlFunc.ToString(x.TargetValue).Contains(keywords)
                || SqlFunc.ToString(x.ActualValue).Contains(keywords)
                || SqlFunc.ToString(x.CompletionPercentage).Contains(keywords)
                || SqlFunc.ToString(x.ObjectiveWeight).Contains(keywords)
                || (x.AchievementNotes != null && x.AchievementNotes.Contains(keywords))
                || SqlFunc.ToString(x.ObjectiveStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartDate).Contains(keywords)
                || SqlFunc.ToString(x.DueDate).Contains(keywords)
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

        if (queryDto?.SchemeMetricId.HasValue == true)
        {
            exp = exp.And(x => x.SchemeMetricId == queryDto.SchemeMetricId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ObjectivePeriod))
        {
            exp = exp.And(x => x.ObjectivePeriod != null && x.ObjectivePeriod.Contains(queryDto.ObjectivePeriod));
        }

        if (!string.IsNullOrEmpty(queryDto?.ObjectiveDescription))
        {
            exp = exp.And(x => x.ObjectiveDescription != null && x.ObjectiveDescription.Contains(queryDto.ObjectiveDescription));
        }

        if (queryDto?.TargetValue.HasValue == true)
        {
            exp = exp.And(x => x.TargetValue == queryDto.TargetValue);
        }

        if (queryDto?.ActualValue.HasValue == true)
        {
            exp = exp.And(x => x.ActualValue == queryDto.ActualValue);
        }

        if (queryDto?.CompletionPercentage.HasValue == true)
        {
            exp = exp.And(x => x.CompletionPercentage == queryDto.CompletionPercentage);
        }

        if (queryDto?.ObjectiveWeight.HasValue == true)
        {
            exp = exp.And(x => x.ObjectiveWeight == queryDto.ObjectiveWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.AchievementNotes))
        {
            exp = exp.And(x => x.AchievementNotes != null && x.AchievementNotes.Contains(queryDto.AchievementNotes));
        }

        if (queryDto?.ObjectiveStatus.HasValue == true)
        {
            exp = exp.And(x => x.ObjectiveStatus == queryDto.ObjectiveStatus);
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

        if (queryDto?.StartDateStart.HasValue == true)
        {
            exp = exp.And(x => x.StartDate >= queryDto.StartDateStart);
        }

        if (queryDto?.StartDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartDate <= queryDto.StartDateEnd);
        }

        if (queryDto?.DueDateStart.HasValue == true)
        {
            exp = exp.And(x => x.DueDate >= queryDto.DueDateStart);
        }

        if (queryDto?.DueDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.DueDate <= queryDto.DueDateEnd);
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

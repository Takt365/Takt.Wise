// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Performance
// 文件名称：TaktPerfSchemeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：绩效方案指标应用服务实现
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
/// 绩效方案指标应用服务
/// </summary>
public class TaktPerfSchemeService : TaktServiceBase, ITaktPerfSchemeService
{
    private readonly ITaktCompanyRepository<TaktPerfScheme> _perfSchemeRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="perfSchemeRepository">绩效方案指标仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPerfSchemeService(
        ITaktCompanyRepository<TaktPerfScheme> perfSchemeRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _perfSchemeRepository = perfSchemeRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取绩效方案指标列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPerfSchemeDto>> GetPerfSchemeListAsync(TaktPerfSchemeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _perfSchemeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPerfSchemeDto>.Create(
            data.Adapt<List<TaktPerfSchemeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfSchemeDto?> GetPerfSchemeByIdAsync(long id)
    {
        var entity = await _perfSchemeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPerfSchemeDto>();
    }

    /// <summary>
    /// 获取绩效方案指标选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPerfSchemeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _perfSchemeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SchemeMetricStatus == 1,
            x => x.SchemeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SchemeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建绩效方案指标
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfSchemeDto> CreatePerfSchemeAsync(TaktPerfSchemeCreateDto dto)
    {
        var entity = dto.Adapt<TaktPerfScheme>();
        var isUnique_ix_perf_scheme_code_unique = await _uniqueValidator.IsUniqueAsync(
            _perfSchemeRepository,
            x => x.SchemeCode == entity.SchemeCode
                && x.MetricCode == entity.MetricCode);
        if (!isUnique_ix_perf_scheme_code_unique)
        {
            throw new TaktBusinessException("绩效方案指标的SchemeCode、MetricCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _perfSchemeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _perfSchemeRepository.CreateAsync(entity);
        return await GetPerfSchemeByIdAsync(entity.Id) ?? entity.Adapt<TaktPerfSchemeDto>();
    }

    /// <summary>
    /// 更新绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfSchemeDto> UpdatePerfSchemeAsync(long id, TaktPerfSchemeUpdateDto dto)
    {
        var entity = await _perfSchemeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效方案指标不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_perf_scheme_code_unique = await _uniqueValidator.IsUniqueAsync(
            _perfSchemeRepository,
            x => x.SchemeCode == entity.SchemeCode
                && x.MetricCode == entity.MetricCode,
            id);
        if (!isUnique_ix_perf_scheme_code_unique)
        {
            throw new TaktBusinessException("绩效方案指标的SchemeCode、MetricCode已存在");
        }
        await _perfSchemeRepository.UpdateAsync(entity);
        return await GetPerfSchemeByIdAsync(id) ?? throw new TaktBusinessException("绩效方案指标不存在");
    }

    /// <summary>
    /// 删除绩效方案指标
    /// </summary>
    /// <param name="id">绩效方案指标ID</param>
    /// <returns>任务</returns>
    public async Task DeletePerfSchemeByIdAsync(long id)
    {
        var deleted = await _perfSchemeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("绩效方案指标不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除绩效方案指标
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePerfSchemeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePerfSchemeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新绩效方案指标状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfSchemeDto> UpdatePerfSchemeStatusAsync(TaktPerfSchemeStatusDto dto)
    {
        var entity = await _perfSchemeRepository.GetByIdAsync(dto.PerfSchemeId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效方案指标不存在");
        }
        entity.SchemeMetricStatus = dto.SchemeMetricStatus;
        await _perfSchemeRepository.UpdateAsync(entity);
        return await GetPerfSchemeByIdAsync(dto.PerfSchemeId) ?? throw new TaktBusinessException("绩效方案指标不存在");
    }

    /// <summary>
    /// 更新绩效方案指标排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPerfSchemeDto> UpdatePerfSchemeSortAsync(TaktPerfSchemeSortDto dto)
    {
        var entity = await _perfSchemeRepository.GetByIdAsync(dto.PerfSchemeId);
        if (entity == null)
        {
            throw new TaktBusinessException("绩效方案指标不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _perfSchemeRepository.UpdateAsync(entity);
        return await GetPerfSchemeByIdAsync(dto.PerfSchemeId) ?? throw new TaktBusinessException("绩效方案指标不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPerfSchemeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPerfSchemeTemplateDto>(
            sheetName ?? "绩效方案指标导入模板",
            fileName ?? "绩效方案指标导入模板.xlsx");
    }

    /// <summary>
    /// 导入绩效方案指标
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPerfSchemeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPerfSchemeImportDto>(fileStream, sheetName ?? "绩效方案指标导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _perfSchemeRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPerfScheme>();
                var importKey = $"{entity.SchemeCode}|{entity.MetricCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SchemeCode、MetricCode）");
                }
                var isUnique_ix_perf_scheme_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _perfSchemeRepository,
                    x => x.SchemeCode == entity.SchemeCode
                        && x.MetricCode == entity.MetricCode);
                if (!isUnique_ix_perf_scheme_code_unique)
                {
                    throw new TaktBusinessException("绩效方案指标的SchemeCode、MetricCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _perfSchemeRepository.CreateAsync(entity);
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
    /// 导出绩效方案指标
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPerfSchemeAsync(TaktPerfSchemeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPerfSchemeQueryDto());
        var list = await _perfSchemeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPerfSchemeExportDto>(),
                sheetName ?? "绩效方案指标数据",
                fileName ?? "绩效方案指标导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPerfSchemeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "绩效方案指标数据",
            fileName ?? "绩效方案指标导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建绩效方案指标查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPerfScheme, bool>> QueryExpression(TaktPerfSchemeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPerfScheme>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.SchemeCode != null && x.SchemeCode.Contains(keywords))
                || (x.SchemeName != null && x.SchemeName.Contains(keywords))
                || (x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(keywords))
                || (x.CycleType != null && x.CycleType.Contains(keywords))
                || (x.ScoringStandard != null && x.ScoringStandard.Contains(keywords))
                || SqlFunc.ToString(x.SelfEvaluationWeight).Contains(keywords)
                || SqlFunc.ToString(x.SupervisorWeight).Contains(keywords)
                || (x.MetricCode != null && x.MetricCode.Contains(keywords))
                || (x.MetricName != null && x.MetricName.Contains(keywords))
                || (x.Category != null && x.Category.Contains(keywords))
                || (x.MetricType != null && x.MetricType.Contains(keywords))
                || (x.ScoringCriteria != null && x.ScoringCriteria.Contains(keywords))
                || SqlFunc.ToString(x.StandardWeight).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.SchemeMetricStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.SchemeCode))
        {
            exp = exp.And(x => x.SchemeCode != null && x.SchemeCode.Contains(queryDto.SchemeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SchemeName))
        {
            exp = exp.And(x => x.SchemeName != null && x.SchemeName.Contains(queryDto.SchemeName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ApplicableDepartment))
        {
            exp = exp.And(x => x.ApplicableDepartment != null && x.ApplicableDepartment.Contains(queryDto.ApplicableDepartment));
        }

        if (!string.IsNullOrEmpty(queryDto?.CycleType))
        {
            exp = exp.And(x => x.CycleType != null && x.CycleType.Contains(queryDto.CycleType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScoringStandard))
        {
            exp = exp.And(x => x.ScoringStandard != null && x.ScoringStandard.Contains(queryDto.ScoringStandard));
        }

        if (queryDto?.SelfEvaluationWeight.HasValue == true)
        {
            exp = exp.And(x => x.SelfEvaluationWeight == queryDto.SelfEvaluationWeight);
        }

        if (queryDto?.SupervisorWeight.HasValue == true)
        {
            exp = exp.And(x => x.SupervisorWeight == queryDto.SupervisorWeight);
        }

        if (!string.IsNullOrEmpty(queryDto?.MetricCode))
        {
            exp = exp.And(x => x.MetricCode != null && x.MetricCode.Contains(queryDto.MetricCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MetricName))
        {
            exp = exp.And(x => x.MetricName != null && x.MetricName.Contains(queryDto.MetricName));
        }

        if (!string.IsNullOrEmpty(queryDto?.Category))
        {
            exp = exp.And(x => x.Category != null && x.Category.Contains(queryDto.Category));
        }

        if (!string.IsNullOrEmpty(queryDto?.MetricType))
        {
            exp = exp.And(x => x.MetricType != null && x.MetricType.Contains(queryDto.MetricType));
        }

        if (!string.IsNullOrEmpty(queryDto?.ScoringCriteria))
        {
            exp = exp.And(x => x.ScoringCriteria != null && x.ScoringCriteria.Contains(queryDto.ScoringCriteria));
        }

        if (queryDto?.StandardWeight.HasValue == true)
        {
            exp = exp.And(x => x.StandardWeight == queryDto.StandardWeight);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.SchemeMetricStatus.HasValue == true)
        {
            exp = exp.And(x => x.SchemeMetricStatus == queryDto.SchemeMetricStatus);
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

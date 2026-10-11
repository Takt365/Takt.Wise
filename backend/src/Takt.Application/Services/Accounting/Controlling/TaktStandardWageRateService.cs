// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktStandardWageRateService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：标准工资率应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Domain.Entities.Accounting.Controlling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 标准工资率应用服务
/// </summary>
public class TaktStandardWageRateService : TaktServiceBase, ITaktStandardWageRateService
{
    private readonly ITaktCompanyRepository<TaktStandardWageRate> _standardWageRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardWageRateRepository">标准工资率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStandardWageRateService(
        ITaktCompanyRepository<TaktStandardWageRate> standardWageRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _standardWageRateRepository = standardWageRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取标准工资率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStandardWageRateDto>> GetStandardWageRateListAsync(TaktStandardWageRateQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _standardWageRateRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStandardWageRateDto>.Create(
            data.Adapt<List<TaktStandardWageRateDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardWageRateDto?> GetStandardWageRateByIdAsync(long id)
    {
        var entity = await _standardWageRateRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktStandardWageRateDto>();
    }

    /// <summary>
    /// 获取标准工资率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStandardWageRateOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _standardWageRateRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.YearMonth ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.YearMonth ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建标准工资率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardWageRateDto> CreateStandardWageRateAsync(TaktStandardWageRateCreateDto dto)
    {
        var entity = dto.Adapt<TaktStandardWageRate>();
        var isUnique_ix_standard_wage_rate_unique = await _uniqueValidator.IsUniqueAsync(
            _standardWageRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.YearMonth == entity.YearMonth);
        if (!isUnique_ix_standard_wage_rate_unique)
        {
            throw new TaktBusinessException("标准工资率的PlantCode、YearMonth已存在");
        }
        entity = await _standardWageRateRepository.CreateAsync(entity);
        return await GetStandardWageRateByIdAsync(entity.Id) ?? entity.Adapt<TaktStandardWageRateDto>();
    }

    /// <summary>
    /// 更新标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardWageRateDto> UpdateStandardWageRateAsync(long id, TaktStandardWageRateUpdateDto dto)
    {
        var entity = await _standardWageRateRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准工资率不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_standard_wage_rate_unique = await _uniqueValidator.IsUniqueAsync(
            _standardWageRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.YearMonth == entity.YearMonth,
            id);
        if (!isUnique_ix_standard_wage_rate_unique)
        {
            throw new TaktBusinessException("标准工资率的PlantCode、YearMonth已存在");
        }
        await _standardWageRateRepository.UpdateAsync(entity);
        return await GetStandardWageRateByIdAsync(id) ?? throw new TaktBusinessException("标准工资率不存在");
    }

    /// <summary>
    /// 删除标准工资率
    /// </summary>
    /// <param name="id">标准工资率ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardWageRateByIdAsync(long id)
    {
        var deleted = await _standardWageRateRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("标准工资率不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除标准工资率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardWageRateBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteStandardWageRateByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetStandardWageRateTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktStandardWageRateTemplateDto>(
            sheetName ?? "标准工资率导入模板",
            fileName ?? "标准工资率导入模板.xlsx");
    }

    /// <summary>
    /// 导入标准工资率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportStandardWageRateAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktStandardWageRateImportDto>(fileStream, sheetName ?? "标准工资率导入模板");
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
                var entity = rows[i].Adapt<TaktStandardWageRate>();
                var importKey = $"{entity.PlantCode}|{entity.YearMonth}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、YearMonth）");
                }
                var isUnique_ix_standard_wage_rate_unique = await _uniqueValidator.IsUniqueAsync(
                    _standardWageRateRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.YearMonth == entity.YearMonth);
                if (!isUnique_ix_standard_wage_rate_unique)
                {
                    throw new TaktBusinessException("标准工资率的PlantCode、YearMonth已存在");
                }
                await _standardWageRateRepository.CreateAsync(entity);
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
    /// 导出标准工资率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStandardWageRateAsync(TaktStandardWageRateQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktStandardWageRateQueryDto());
        var list = await _standardWageRateRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStandardWageRateExportDto>(),
                sheetName ?? "标准工资率数据",
                fileName ?? "标准工资率导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStandardWageRateExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "标准工资率数据",
            fileName ?? "标准工资率导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建标准工资率查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStandardWageRate, bool>> QueryExpression(TaktStandardWageRateQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStandardWageRate>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.YearMonth != null && x.YearMonth.Contains(keywords))
                || SqlFunc.ToString(x.WorkingDays).Contains(keywords)
                || SqlFunc.ToString(x.SalesAmount).Contains(keywords)
                || SqlFunc.ToString(x.DirectLaborCount).Contains(keywords)
                || SqlFunc.ToString(x.DirectLaborWage).Contains(keywords)
                || SqlFunc.ToString(x.DirectOvertimeHours).Contains(keywords)
                || SqlFunc.ToString(x.DirectOvertimeTotal).Contains(keywords)
                || SqlFunc.ToString(x.DirectWageRate).Contains(keywords)
                || SqlFunc.ToString(x.IndirectLaborCount).Contains(keywords)
                || SqlFunc.ToString(x.IndirectLaborWage).Contains(keywords)
                || SqlFunc.ToString(x.IndirectOvertimeHours).Contains(keywords)
                || SqlFunc.ToString(x.IndirectOvertimeTotal).Contains(keywords)
                || SqlFunc.ToString(x.IndirectWageRate).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.YearMonth))
        {
            exp = exp.And(x => x.YearMonth != null && x.YearMonth.Contains(queryDto.YearMonth));
        }

        if (queryDto?.WorkingDays.HasValue == true)
        {
            exp = exp.And(x => x.WorkingDays == queryDto.WorkingDays);
        }

        if (queryDto?.SalesAmount.HasValue == true)
        {
            exp = exp.And(x => x.SalesAmount == queryDto.SalesAmount);
        }

        if (queryDto?.DirectLaborCount.HasValue == true)
        {
            exp = exp.And(x => x.DirectLaborCount == queryDto.DirectLaborCount);
        }

        if (queryDto?.DirectLaborWage.HasValue == true)
        {
            exp = exp.And(x => x.DirectLaborWage == queryDto.DirectLaborWage);
        }

        if (queryDto?.DirectOvertimeHours.HasValue == true)
        {
            exp = exp.And(x => x.DirectOvertimeHours == queryDto.DirectOvertimeHours);
        }

        if (queryDto?.DirectOvertimeTotal.HasValue == true)
        {
            exp = exp.And(x => x.DirectOvertimeTotal == queryDto.DirectOvertimeTotal);
        }

        if (queryDto?.DirectWageRate.HasValue == true)
        {
            exp = exp.And(x => x.DirectWageRate == queryDto.DirectWageRate);
        }

        if (queryDto?.IndirectLaborCount.HasValue == true)
        {
            exp = exp.And(x => x.IndirectLaborCount == queryDto.IndirectLaborCount);
        }

        if (queryDto?.IndirectLaborWage.HasValue == true)
        {
            exp = exp.And(x => x.IndirectLaborWage == queryDto.IndirectLaborWage);
        }

        if (queryDto?.IndirectOvertimeHours.HasValue == true)
        {
            exp = exp.And(x => x.IndirectOvertimeHours == queryDto.IndirectOvertimeHours);
        }

        if (queryDto?.IndirectOvertimeTotal.HasValue == true)
        {
            exp = exp.And(x => x.IndirectOvertimeTotal == queryDto.IndirectOvertimeTotal);
        }

        if (queryDto?.IndirectWageRate.HasValue == true)
        {
            exp = exp.And(x => x.IndirectWageRate == queryDto.IndirectWageRate);
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

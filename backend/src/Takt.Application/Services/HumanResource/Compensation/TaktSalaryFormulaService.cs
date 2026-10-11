// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：TaktSalaryFormulaService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资计算公式应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Domain.Entities.HumanResource.Compensation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 薪资计算公式应用服务
/// </summary>
public class TaktSalaryFormulaService : TaktServiceBase, ITaktSalaryFormulaService
{
    private readonly ITaktCompanyRepository<TaktSalaryFormula> _salaryFormulaRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryFormulaRepository">薪资计算公式仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalaryFormulaService(
        ITaktCompanyRepository<TaktSalaryFormula> salaryFormulaRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salaryFormulaRepository = salaryFormulaRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取薪资计算公式列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalaryFormulaDto>> GetSalaryFormulaListAsync(TaktSalaryFormulaQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salaryFormulaRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalaryFormulaDto>.Create(
            data.Adapt<List<TaktSalaryFormulaDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryFormulaDto?> GetSalaryFormulaByIdAsync(long id)
    {
        var entity = await _salaryFormulaRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalaryFormulaDto>();
    }

    /// <summary>
    /// 获取薪资计算公式选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalaryFormulaOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salaryFormulaRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FormulaStatus == 1,
            x => x.SetName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SetName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建薪资计算公式
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryFormulaDto> CreateSalaryFormulaAsync(TaktSalaryFormulaCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalaryFormula>();
        var isUnique_ix_salary_formula_set_step_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryFormulaRepository,
            x => x.SetCode == entity.SetCode
                && x.SortOrder == entity.SortOrder);
        if (!isUnique_ix_salary_formula_set_step_unique)
        {
            throw new TaktBusinessException("薪资计算公式的SetCode、SortOrder已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _salaryFormulaRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PayrollId == entity.PayrollId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PayrollId.GetValueOrDefault(), maxSort);
        }
        entity = await _salaryFormulaRepository.CreateAsync(entity);
        return await GetSalaryFormulaByIdAsync(entity.Id) ?? entity.Adapt<TaktSalaryFormulaDto>();
    }

    /// <summary>
    /// 更新薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryFormulaDto> UpdateSalaryFormulaAsync(long id, TaktSalaryFormulaUpdateDto dto)
    {
        var entity = await _salaryFormulaRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资计算公式不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_salary_formula_set_step_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryFormulaRepository,
            x => x.SetCode == entity.SetCode
                && x.SortOrder == entity.SortOrder,
            id);
        if (!isUnique_ix_salary_formula_set_step_unique)
        {
            throw new TaktBusinessException("薪资计算公式的SetCode、SortOrder已存在");
        }
        await _salaryFormulaRepository.UpdateAsync(entity);
        return await GetSalaryFormulaByIdAsync(id) ?? throw new TaktBusinessException("薪资计算公式不存在");
    }

    /// <summary>
    /// 删除薪资计算公式
    /// </summary>
    /// <param name="id">薪资计算公式ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryFormulaByIdAsync(long id)
    {
        var deleted = await _salaryFormulaRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("薪资计算公式不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除薪资计算公式
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryFormulaBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalaryFormulaByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新薪资计算公式状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryFormulaDto> UpdateSalaryFormulaStatusAsync(TaktSalaryFormulaStatusDto dto)
    {
        var entity = await _salaryFormulaRepository.GetByIdAsync(dto.SalaryFormulaId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资计算公式不存在");
        }
        entity.FormulaStatus = dto.FormulaStatus;
        await _salaryFormulaRepository.UpdateAsync(entity);
        return await GetSalaryFormulaByIdAsync(dto.SalaryFormulaId) ?? throw new TaktBusinessException("薪资计算公式不存在");
    }

    /// <summary>
    /// 更新薪资计算公式排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryFormulaDto> UpdateSalaryFormulaSortAsync(TaktSalaryFormulaSortDto dto)
    {
        var entity = await _salaryFormulaRepository.GetByIdAsync(dto.SalaryFormulaId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资计算公式不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _salaryFormulaRepository.UpdateAsync(entity);
        return await GetSalaryFormulaByIdAsync(dto.SalaryFormulaId) ?? throw new TaktBusinessException("薪资计算公式不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalaryFormulaTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalaryFormulaTemplateDto>(
            sheetName ?? "薪资计算公式导入模板",
            fileName ?? "薪资计算公式导入模板.xlsx");
    }

    /// <summary>
    /// 导入薪资计算公式
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalaryFormulaAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalaryFormulaImportDto>(fileStream, sheetName ?? "薪资计算公式导入模板");
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
                var entity = rows[i].Adapt<TaktSalaryFormula>();
                var importKey = $"{entity.SetCode}|{entity.SortOrder}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SetCode、SortOrder）");
                }
                var isUnique_ix_salary_formula_set_step_unique = await _uniqueValidator.IsUniqueAsync(
                    _salaryFormulaRepository,
                    x => x.SetCode == entity.SetCode
                        && x.SortOrder == entity.SortOrder);
                if (!isUnique_ix_salary_formula_set_step_unique)
                {
                    throw new TaktBusinessException("薪资计算公式的SetCode、SortOrder已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _salaryFormulaRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PayrollId == entity.PayrollId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PayrollId.GetValueOrDefault(), maxSort);
                }
                await _salaryFormulaRepository.CreateAsync(entity);
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
    /// 导出薪资计算公式
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalaryFormulaAsync(TaktSalaryFormulaQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalaryFormulaQueryDto());
        var list = await _salaryFormulaRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalaryFormulaExportDto>(),
                sheetName ?? "薪资计算公式数据",
                fileName ?? "薪资计算公式导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalaryFormulaExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "薪资计算公式数据",
            fileName ?? "薪资计算公式导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建薪资计算公式查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalaryFormula, bool>> QueryExpression(TaktSalaryFormulaQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalaryFormula>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.SetCode != null && x.SetCode.Contains(keywords))
                || (x.SetName != null && x.SetName.Contains(keywords))
                || SqlFunc.ToString(x.PayrollId).Contains(keywords)
                || (x.FormulaCode != null && x.FormulaCode.Contains(keywords))
                || (x.FormulaName != null && x.FormulaName.Contains(keywords))
                || SqlFunc.ToString(x.FormulaStep).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.TargetField != null && x.TargetField.Contains(keywords))
                || (x.FormulaExpression != null && x.FormulaExpression.Contains(keywords))
                || (x.StepDescription != null && x.StepDescription.Contains(keywords))
                || SqlFunc.ToString(x.FormulaStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.SetCode))
        {
            exp = exp.And(x => x.SetCode != null && x.SetCode.Contains(queryDto.SetCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SetName))
        {
            exp = exp.And(x => x.SetName != null && x.SetName.Contains(queryDto.SetName));
        }

        if (queryDto?.PayrollId.HasValue == true)
        {
            exp = exp.And(x => x.PayrollId == queryDto.PayrollId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormulaCode))
        {
            exp = exp.And(x => x.FormulaCode != null && x.FormulaCode.Contains(queryDto.FormulaCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormulaName))
        {
            exp = exp.And(x => x.FormulaName != null && x.FormulaName.Contains(queryDto.FormulaName));
        }

        if (queryDto?.FormulaStep.HasValue == true)
        {
            exp = exp.And(x => x.FormulaStep == queryDto.FormulaStep);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (!string.IsNullOrEmpty(queryDto?.TargetField))
        {
            exp = exp.And(x => x.TargetField != null && x.TargetField.Contains(queryDto.TargetField));
        }

        if (!string.IsNullOrEmpty(queryDto?.FormulaExpression))
        {
            exp = exp.And(x => x.FormulaExpression != null && x.FormulaExpression.Contains(queryDto.FormulaExpression));
        }

        if (!string.IsNullOrEmpty(queryDto?.StepDescription))
        {
            exp = exp.And(x => x.StepDescription != null && x.StepDescription.Contains(queryDto.StepDescription));
        }

        if (queryDto?.FormulaStatus.HasValue == true)
        {
            exp = exp.And(x => x.FormulaStatus == queryDto.FormulaStatus);
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

        if (queryDto?.EffectiveDateStart.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate >= queryDto.EffectiveDateStart);
        }

        if (queryDto?.EffectiveDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.EffectiveDate <= queryDto.EffectiveDateEnd);
        }

        if (queryDto?.ExpiryDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate >= queryDto.ExpiryDateStart);
        }

        if (queryDto?.ExpiryDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ExpiryDate <= queryDto.ExpiryDateEnd);
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

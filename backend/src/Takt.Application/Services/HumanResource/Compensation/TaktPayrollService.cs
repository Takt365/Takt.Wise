// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：TaktPayrollService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪酬体系应用服务实现
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
/// 薪酬体系应用服务
/// </summary>
public class TaktPayrollService : TaktServiceBase, ITaktPayrollService
{
    private readonly ITaktCompanyRepository<TaktPayroll> _payrollRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="payrollRepository">薪酬体系仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPayrollService(
        ITaktCompanyRepository<TaktPayroll> payrollRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _payrollRepository = payrollRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取薪酬体系列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPayrollDto>> GetPayrollListAsync(TaktPayrollQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _payrollRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPayrollDto>.Create(
            data.Adapt<List<TaktPayrollDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayrollDto?> GetPayrollByIdAsync(long id)
    {
        var entity = await _payrollRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPayrollDto>();
    }

    /// <summary>
    /// 获取薪酬体系选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPayrollOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _payrollRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PayrollStatus == 1,
            x => x.PayrollName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PayrollName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建薪酬体系
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayrollDto> CreatePayrollAsync(TaktPayrollCreateDto dto)
    {
        var entity = dto.Adapt<TaktPayroll>();
        var isUnique_ix_payroll_code_unique = await _uniqueValidator.IsUniqueAsync(
            _payrollRepository,
            x => x.PayrollCode == entity.PayrollCode);
        if (!isUnique_ix_payroll_code_unique)
        {
            throw new TaktBusinessException("薪酬体系的PayrollCode已存在");
        }
        entity = await _payrollRepository.CreateAsync(entity);
        return await GetPayrollByIdAsync(entity.Id) ?? entity.Adapt<TaktPayrollDto>();
    }

    /// <summary>
    /// 更新薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayrollDto> UpdatePayrollAsync(long id, TaktPayrollUpdateDto dto)
    {
        var entity = await _payrollRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("薪酬体系不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_payroll_code_unique = await _uniqueValidator.IsUniqueAsync(
            _payrollRepository,
            x => x.PayrollCode == entity.PayrollCode,
            id);
        if (!isUnique_ix_payroll_code_unique)
        {
            throw new TaktBusinessException("薪酬体系的PayrollCode已存在");
        }
        await _payrollRepository.UpdateAsync(entity);
        return await GetPayrollByIdAsync(id) ?? throw new TaktBusinessException("薪酬体系不存在");
    }

    /// <summary>
    /// 删除薪酬体系
    /// </summary>
    /// <param name="id">薪酬体系ID</param>
    /// <returns>任务</returns>
    public async Task DeletePayrollByIdAsync(long id)
    {
        var deleted = await _payrollRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("薪酬体系不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除薪酬体系
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePayrollBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePayrollByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新薪酬体系状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPayrollDto> UpdatePayrollStatusAsync(TaktPayrollStatusDto dto)
    {
        var entity = await _payrollRepository.GetByIdAsync(dto.PayrollId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪酬体系不存在");
        }
        entity.PayrollStatus = dto.PayrollStatus;
        await _payrollRepository.UpdateAsync(entity);
        return await GetPayrollByIdAsync(dto.PayrollId) ?? throw new TaktBusinessException("薪酬体系不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPayrollTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPayrollTemplateDto>(
            sheetName ?? "薪酬体系导入模板",
            fileName ?? "薪酬体系导入模板.xlsx");
    }

    /// <summary>
    /// 导入薪酬体系
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPayrollAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPayrollImportDto>(fileStream, sheetName ?? "薪酬体系导入模板");
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
                var entity = rows[i].Adapt<TaktPayroll>();
                var importKey = $"{entity.PayrollCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PayrollCode）");
                }
                var isUnique_ix_payroll_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _payrollRepository,
                    x => x.PayrollCode == entity.PayrollCode);
                if (!isUnique_ix_payroll_code_unique)
                {
                    throw new TaktBusinessException("薪酬体系的PayrollCode已存在");
                }
                await _payrollRepository.CreateAsync(entity);
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
    /// 导出薪酬体系
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPayrollAsync(TaktPayrollQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPayrollQueryDto());
        var list = await _payrollRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPayrollExportDto>(),
                sheetName ?? "薪酬体系数据",
                fileName ?? "薪酬体系导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPayrollExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "薪酬体系数据",
            fileName ?? "薪酬体系导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建薪酬体系查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPayroll, bool>> QueryExpression(TaktPayrollQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPayroll>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PayrollCode != null && x.PayrollCode.Contains(keywords))
                || (x.PayrollName != null && x.PayrollName.Contains(keywords))
                || SqlFunc.ToString(x.PayScaleId).Contains(keywords)
                || (x.FormulaSetCode != null && x.FormulaSetCode.Contains(keywords))
                || SqlFunc.ToString(x.PayrollStatus).Contains(keywords)
                || (x.PayrollDescription != null && x.PayrollDescription.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PayrollCode))
        {
            exp = exp.And(x => x.PayrollCode != null && x.PayrollCode.Contains(queryDto.PayrollCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PayrollName))
        {
            exp = exp.And(x => x.PayrollName != null && x.PayrollName.Contains(queryDto.PayrollName));
        }

        if (queryDto?.PayScaleId.HasValue == true)
        {
            exp = exp.And(x => x.PayScaleId == queryDto.PayScaleId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormulaSetCode))
        {
            exp = exp.And(x => x.FormulaSetCode != null && x.FormulaSetCode.Contains(queryDto.FormulaSetCode));
        }

        if (queryDto?.PayrollStatus.HasValue == true)
        {
            exp = exp.And(x => x.PayrollStatus == queryDto.PayrollStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PayrollDescription))
        {
            exp = exp.And(x => x.PayrollDescription != null && x.PayrollDescription.Contains(queryDto.PayrollDescription));
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

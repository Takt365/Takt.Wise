// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktFinancialPeriodService.cs
// 创建时间：2026-07-20
// 创建人：Takt365(Cursor AI)
// 功能描述：财务期间应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 财务期间应用服务
/// </summary>
public class TaktFinancialPeriodService : TaktServiceBase, ITaktFinancialPeriodService
{
    private readonly ITaktTenantRepository<TaktFinancialPeriod> _financialPeriodRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="financialPeriodRepository">财务期间仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFinancialPeriodService(
        ITaktTenantRepository<TaktFinancialPeriod> financialPeriodRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _financialPeriodRepository = financialPeriodRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取财务期间列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFinancialPeriodDto>> GetFinancialPeriodListAsync(TaktFinancialPeriodQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _financialPeriodRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFinancialPeriodDto>.Create(
            data.Adapt<List<TaktFinancialPeriodDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFinancialPeriodDto?> GetFinancialPeriodByIdAsync(long id)
    {
        var entity = await _financialPeriodRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktFinancialPeriodDto>();
    }

    /// <summary>
    /// 获取财务期间选项列表（按 FinancialYearCode 去重；DictValue=FinancialYearCode，供业务表 FiscalYear/FinancialYear 下拉）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFinancialPeriodOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _financialPeriodRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode
                && x.FinancialYearCode != null
                && x.FinancialYearCode != string.Empty,
            x => x.FinancialYearCode ?? string.Empty,
            false);
        return list
            .GroupBy(e => e.FinancialYearCode!, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Key, StringComparer.OrdinalIgnoreCase)
            .Select(g => new TaktSelectOption
            {
                DictValue = g.Key,
                DictLabel = g.Key,
            })
            .ToList();
    }

    /// <summary>
    /// 创建财务期间
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFinancialPeriodDto> CreateFinancialPeriodAsync(TaktFinancialPeriodCreateDto dto)
    {
        var entity = dto.Adapt<TaktFinancialPeriod>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_accounting_financial_period_period_unique = await _uniqueValidator.IsUniqueAsync(
            _financialPeriodRepository,
            x => x.CountryCode == entity.CountryCode
                && x.PeriodCode == entity.PeriodCode);
        if (!isUnique_ix_takt_accounting_financial_period_period_unique)
        {
            throw new TaktBusinessException("财务期间的CountryCode、PeriodCode已存在");
        }
        entity = await _financialPeriodRepository.CreateAsync(entity);
        return await GetFinancialPeriodByIdAsync(entity.Id) ?? entity.Adapt<TaktFinancialPeriodDto>();
    }

    /// <summary>
    /// 更新财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFinancialPeriodDto> UpdateFinancialPeriodAsync(long id, TaktFinancialPeriodUpdateDto dto)
    {
        var entity = await _financialPeriodRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("财务期间不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_accounting_financial_period_period_unique = await _uniqueValidator.IsUniqueAsync(
            _financialPeriodRepository,
            x => x.CountryCode == entity.CountryCode
                && x.PeriodCode == entity.PeriodCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_period_period_unique)
        {
            throw new TaktBusinessException("财务期间的CountryCode、PeriodCode已存在");
        }
        await _financialPeriodRepository.UpdateAsync(entity);
        return await GetFinancialPeriodByIdAsync(id) ?? throw new TaktBusinessException("财务期间不存在");
    }

    /// <summary>
    /// 删除财务期间
    /// </summary>
    /// <param name="id">财务期间ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFinancialPeriodByIdAsync(long id)
    {
        var entity = await _financialPeriodRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("财务期间不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置财务期间不允许删除");
        }
        var deleted = await _financialPeriodRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("财务期间不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除财务期间
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFinancialPeriodBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _financialPeriodRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置财务期间不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteFinancialPeriodByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFinancialPeriodTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFinancialPeriodTemplateDto>(
            sheetName ?? "财务期间导入模板",
            fileName ?? "财务期间导入模板.xlsx");
    }

    /// <summary>
    /// 导入财务期间
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFinancialPeriodAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFinancialPeriodImportDto>(fileStream, sheetName ?? "财务期间导入模板");
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
                var entity = rows[i].Adapt<TaktFinancialPeriod>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.CountryCode}|{entity.PeriodCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountryCode、PeriodCode）");
                }
                var isUnique_ix_takt_accounting_financial_period_period_unique = await _uniqueValidator.IsUniqueAsync(
                    _financialPeriodRepository,
                    x => x.CountryCode == entity.CountryCode
                        && x.PeriodCode == entity.PeriodCode);
                if (!isUnique_ix_takt_accounting_financial_period_period_unique)
                {
                    throw new TaktBusinessException("财务期间的CountryCode、PeriodCode已存在");
                }
                await _financialPeriodRepository.CreateAsync(entity);
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
    /// 导出财务期间
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFinancialPeriodAsync(TaktFinancialPeriodQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFinancialPeriodQueryDto());
        var list = await _financialPeriodRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFinancialPeriodExportDto>(),
                sheetName ?? "财务期间数据",
                fileName ?? "财务期间导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFinancialPeriodExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "财务期间数据",
            fileName ?? "财务期间导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建财务期间查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFinancialPeriod, bool>> QueryExpression(TaktFinancialPeriodQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFinancialPeriod>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.FinancialYearCode != null && x.FinancialYearCode.Contains(keywords))
                || (x.PeriodCode != null && x.PeriodCode.Contains(keywords))
                || SqlFunc.ToString(x.CalendarYear).Contains(keywords)
                || SqlFunc.ToString(x.CalendarMonth).Contains(keywords)
                || (x.FinancialQuarterCode != null && x.FinancialQuarterCode.Contains(keywords))
                || SqlFunc.ToString(x.IsBuiltIn).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.CountryCode))
        {
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(queryDto.CountryCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FinancialYearCode))
        {
            exp = exp.And(x => x.FinancialYearCode != null && x.FinancialYearCode.Contains(queryDto.FinancialYearCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PeriodCode))
        {
            exp = exp.And(x => x.PeriodCode != null && x.PeriodCode.Contains(queryDto.PeriodCode));
        }

        if (queryDto?.CalendarYear.HasValue == true)
        {
            exp = exp.And(x => x.CalendarYear == queryDto.CalendarYear);
        }

        if (queryDto?.CalendarMonth.HasValue == true)
        {
            exp = exp.And(x => x.CalendarMonth == queryDto.CalendarMonth);
        }

        if (!string.IsNullOrEmpty(queryDto?.FinancialQuarterCode))
        {
            exp = exp.And(x => x.FinancialQuarterCode != null && x.FinancialQuarterCode.Contains(queryDto.FinancialQuarterCode));
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            exp = exp.And(x => x.IsBuiltIn == queryDto.IsBuiltIn);
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

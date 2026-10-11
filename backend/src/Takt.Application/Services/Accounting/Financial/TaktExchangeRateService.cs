// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktExchangeRateService.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：汇率应用服务实现
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
/// 汇率应用服务
/// </summary>
public class TaktExchangeRateService : TaktServiceBase, ITaktExchangeRateService
{
    private readonly ITaktTenantRepository<TaktExchangeRate> _exchangeRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="exchangeRateRepository">汇率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktExchangeRateService(
        ITaktTenantRepository<TaktExchangeRate> exchangeRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取汇率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktExchangeRateDto>> GetExchangeRateListAsync(TaktExchangeRateQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _exchangeRateRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktExchangeRateDto>.Create(
            data.Adapt<List<TaktExchangeRateDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktExchangeRateDto?> GetExchangeRateByIdAsync(long id)
    {
        var entity = await _exchangeRateRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktExchangeRateDto>();
    }

    /// <summary>
    /// 获取汇率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetExchangeRateOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _exchangeRateRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.ExchangeRateStatus == 1,
            x => x.FromCurrencyCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.FromCurrencyCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建汇率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExchangeRateDto> CreateExchangeRateAsync(TaktExchangeRateCreateDto dto)
    {
        var entity = dto.Adapt<TaktExchangeRate>();
        var isUnique_ix_takt_accounting_financial_exchange_rate_unique = await _uniqueValidator.IsUniqueAsync(
            _exchangeRateRepository,
            x => x.FromCurrencyCode == entity.FromCurrencyCode
                && x.ToCurrencyCode == entity.ToCurrencyCode
                && x.ExchangeRateType == entity.ExchangeRateType
                && x.ValidFrom == entity.ValidFrom);
        if (!isUnique_ix_takt_accounting_financial_exchange_rate_unique)
        {
            throw new TaktBusinessException("汇率的FromCurrencyCode、ToCurrencyCode、ExchangeRateType、ValidFrom已存在");
        }
        entity = await _exchangeRateRepository.CreateAsync(entity);
        return await GetExchangeRateByIdAsync(entity.Id) ?? entity.Adapt<TaktExchangeRateDto>();
    }

    /// <summary>
    /// 更新汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExchangeRateDto> UpdateExchangeRateAsync(long id, TaktExchangeRateUpdateDto dto)
    {
        var entity = await _exchangeRateRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("汇率不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_exchange_rate_unique = await _uniqueValidator.IsUniqueAsync(
            _exchangeRateRepository,
            x => x.FromCurrencyCode == entity.FromCurrencyCode
                && x.ToCurrencyCode == entity.ToCurrencyCode
                && x.ExchangeRateType == entity.ExchangeRateType
                && x.ValidFrom == entity.ValidFrom,
            id);
        if (!isUnique_ix_takt_accounting_financial_exchange_rate_unique)
        {
            throw new TaktBusinessException("汇率的FromCurrencyCode、ToCurrencyCode、ExchangeRateType、ValidFrom已存在");
        }
        await _exchangeRateRepository.UpdateAsync(entity);
        return await GetExchangeRateByIdAsync(id) ?? throw new TaktBusinessException("汇率不存在");
    }

    /// <summary>
    /// 删除汇率
    /// </summary>
    /// <param name="id">汇率ID</param>
    /// <returns>任务</returns>
    public async Task DeleteExchangeRateByIdAsync(long id)
    {
        var deleted = await _exchangeRateRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("汇率不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除汇率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteExchangeRateBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteExchangeRateByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新汇率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExchangeRateDto> UpdateExchangeRateStatusAsync(TaktExchangeRateStatusDto dto)
    {
        var entity = await _exchangeRateRepository.GetByIdAsync(dto.ExchangeRateId);
        if (entity == null)
        {
            throw new TaktBusinessException("汇率不存在");
        }
        entity.ExchangeRateStatus = dto.ExchangeRateStatus;
        await _exchangeRateRepository.UpdateAsync(entity);
        return await GetExchangeRateByIdAsync(dto.ExchangeRateId) ?? throw new TaktBusinessException("汇率不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetExchangeRateTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktExchangeRateTemplateDto>(
            sheetName ?? "汇率导入模板",
            fileName ?? "汇率导入模板.xlsx");
    }

    /// <summary>
    /// 导入汇率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportExchangeRateAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktExchangeRateImportDto>(fileStream, sheetName ?? "汇率导入模板");
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
                var entity = rows[i].Adapt<TaktExchangeRate>();
                var importKey = $"{entity.FromCurrencyCode}|{entity.ToCurrencyCode}|{entity.ExchangeRateType}|{entity.ValidFrom}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FromCurrencyCode、ToCurrencyCode、ExchangeRateType、ValidFrom）");
                }
                var isUnique_ix_takt_accounting_financial_exchange_rate_unique = await _uniqueValidator.IsUniqueAsync(
                    _exchangeRateRepository,
                    x => x.FromCurrencyCode == entity.FromCurrencyCode
                        && x.ToCurrencyCode == entity.ToCurrencyCode
                        && x.ExchangeRateType == entity.ExchangeRateType
                        && x.ValidFrom == entity.ValidFrom);
                if (!isUnique_ix_takt_accounting_financial_exchange_rate_unique)
                {
                    throw new TaktBusinessException("汇率的FromCurrencyCode、ToCurrencyCode、ExchangeRateType、ValidFrom已存在");
                }
                await _exchangeRateRepository.CreateAsync(entity);
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
    /// 导出汇率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportExchangeRateAsync(TaktExchangeRateQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktExchangeRateQueryDto());
        var list = await _exchangeRateRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExchangeRateExportDto>(),
                sheetName ?? "汇率数据",
                fileName ?? "汇率导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktExchangeRateExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "汇率数据",
            fileName ?? "汇率导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建汇率查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktExchangeRate, bool>> QueryExpression(TaktExchangeRateQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktExchangeRate>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.FromCurrencyCode != null && x.FromCurrencyCode.Contains(keywords))
                || (x.ToCurrencyCode != null && x.ToCurrencyCode.Contains(keywords))
                || (x.ExchangeRateType != null && x.ExchangeRateType.Contains(keywords))
                || SqlFunc.ToString(x.ExchangeRate).Contains(keywords)
                || SqlFunc.ToString(x.RatioFrom).Contains(keywords)
                || SqlFunc.ToString(x.RatioTo).Contains(keywords)
                || SqlFunc.ToString(x.ExchangeRateStatus).Contains(keywords)
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.FromCurrencyCode))
        {
            exp = exp.And(x => x.FromCurrencyCode != null && x.FromCurrencyCode.Contains(queryDto.FromCurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ToCurrencyCode))
        {
            exp = exp.And(x => x.ToCurrencyCode != null && x.ToCurrencyCode.Contains(queryDto.ToCurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExchangeRateType))
        {
            exp = exp.And(x => x.ExchangeRateType != null && x.ExchangeRateType.Contains(queryDto.ExchangeRateType));
        }

        if (queryDto?.ExchangeRate.HasValue == true)
        {
            exp = exp.And(x => x.ExchangeRate == queryDto.ExchangeRate);
        }

        if (queryDto?.RatioFrom.HasValue == true)
        {
            exp = exp.And(x => x.RatioFrom == queryDto.RatioFrom);
        }

        if (queryDto?.RatioTo.HasValue == true)
        {
            exp = exp.And(x => x.RatioTo == queryDto.RatioTo);
        }

        if (queryDto?.ExchangeRateStatus.HasValue == true)
        {
            exp = exp.And(x => x.ExchangeRateStatus == queryDto.ExchangeRateStatus);
        }
        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
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

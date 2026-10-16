// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktTaxConditionService.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税务条件应用服务实现
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
/// 税务条件应用服务
/// </summary>
public class TaktTaxConditionService : TaktServiceBase, ITaktTaxConditionService
{
    private readonly ITaktCompanyRepository<TaktTaxCondition> _taxConditionRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxConditionRepository">税务条件仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTaxConditionService(
        ITaktCompanyRepository<TaktTaxCondition> taxConditionRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _taxConditionRepository = taxConditionRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取税务条件列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTaxConditionDto>> GetTaxConditionListAsync(TaktTaxConditionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTaxConditionDto>.Create(
                new List<TaktTaxConditionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _taxConditionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTaxConditionDto>.Create(
            data.Adapt<List<TaktTaxConditionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxConditionDto?> GetTaxConditionByIdAsync(long id)
    {
        var entity = await _taxConditionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTaxConditionDto>();
    }

    /// <summary>
    /// 获取税务条件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTaxConditionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktTaxCondition>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TaxConditionStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.ApplicationCode != null && x.ApplicationCode.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _taxConditionRepository.GetListAsync(
            predicate,
            x => x.ApplicationCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ApplicationCode,
            DictLabel = e.ApplicationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建税务条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxConditionDto> CreateTaxConditionAsync(TaktTaxConditionCreateDto dto)
    {
        var entity = dto.Adapt<TaktTaxCondition>();
        var isUnique_ix_takt_accounting_financial_tax_condition_unique = await _uniqueValidator.IsUniqueAsync(
            _taxConditionRepository,
            x => x.ApplicationCode == entity.ApplicationCode
                && x.CountryCode == entity.CountryCode
                && x.TaxCode == entity.TaxCode
                && x.ConditionRecordCode == entity.ConditionRecordCode);
        if (!isUnique_ix_takt_accounting_financial_tax_condition_unique)
        {
            throw new TaktBusinessException("税务条件的ApplicationCode、CountryCode、TaxCode、ConditionRecordCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _taxConditionRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _taxConditionRepository.CreateAsync(entity);
        return await GetTaxConditionByIdAsync(entity.Id) ?? entity.Adapt<TaktTaxConditionDto>();
    }

    /// <summary>
    /// 更新税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxConditionDto> UpdateTaxConditionAsync(long id, TaktTaxConditionUpdateDto dto)
    {
        var entity = await _taxConditionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("税务条件不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_tax_condition_unique = await _uniqueValidator.IsUniqueAsync(
            _taxConditionRepository,
            x => x.ApplicationCode == entity.ApplicationCode
                && x.CountryCode == entity.CountryCode
                && x.TaxCode == entity.TaxCode
                && x.ConditionRecordCode == entity.ConditionRecordCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_tax_condition_unique)
        {
            throw new TaktBusinessException("税务条件的ApplicationCode、CountryCode、TaxCode、ConditionRecordCode已存在");
        }
        await _taxConditionRepository.UpdateAsync(entity);
        return await GetTaxConditionByIdAsync(id) ?? throw new TaktBusinessException("税务条件不存在");
    }

    /// <summary>
    /// 删除税务条件
    /// </summary>
    /// <param name="id">税务条件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxConditionByIdAsync(long id)
    {
        var deleted = await _taxConditionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("税务条件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除税务条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxConditionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTaxConditionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新税务条件状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxConditionDto> UpdateTaxConditionStatusAsync(TaktTaxConditionStatusDto dto)
    {
        var entity = await _taxConditionRepository.GetByIdAsync(dto.TaxConditionId);
        if (entity == null)
        {
            throw new TaktBusinessException("税务条件不存在");
        }
        entity.TaxConditionStatus = dto.TaxConditionStatus;
        await _taxConditionRepository.UpdateAsync(entity);
        return await GetTaxConditionByIdAsync(dto.TaxConditionId) ?? throw new TaktBusinessException("税务条件不存在");
    }

    /// <summary>
    /// 更新税务条件排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxConditionDto> UpdateTaxConditionSortAsync(TaktTaxConditionSortDto dto)
    {
        var entity = await _taxConditionRepository.GetByIdAsync(dto.TaxConditionId);
        if (entity == null)
        {
            throw new TaktBusinessException("税务条件不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _taxConditionRepository.UpdateAsync(entity);
        return await GetTaxConditionByIdAsync(dto.TaxConditionId) ?? throw new TaktBusinessException("税务条件不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTaxConditionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTaxConditionTemplateDto>(
            sheetName ?? "税务条件导入模板",
            fileName ?? "税务条件导入模板.xlsx");
    }

    /// <summary>
    /// 导入税务条件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTaxConditionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTaxConditionImportDto>(fileStream, sheetName ?? "税务条件导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _taxConditionRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTaxCondition>();
                var importKey = $"{entity.ApplicationCode}|{entity.CountryCode}|{entity.TaxCode}|{entity.ConditionRecordCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ApplicationCode、CountryCode、TaxCode、ConditionRecordCode）");
                }
                var isUnique_ix_takt_accounting_financial_tax_condition_unique = await _uniqueValidator.IsUniqueAsync(
                    _taxConditionRepository,
                    x => x.ApplicationCode == entity.ApplicationCode
                        && x.CountryCode == entity.CountryCode
                        && x.TaxCode == entity.TaxCode
                        && x.ConditionRecordCode == entity.ConditionRecordCode);
                if (!isUnique_ix_takt_accounting_financial_tax_condition_unique)
                {
                    throw new TaktBusinessException("税务条件的ApplicationCode、CountryCode、TaxCode、ConditionRecordCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _taxConditionRepository.CreateAsync(entity);
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
    /// 导出税务条件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTaxConditionAsync(TaktTaxConditionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTaxConditionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTaxConditionExportDto>(),
                sheetName ?? "税务条件数据",
                fileName ?? "税务条件导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _taxConditionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTaxConditionExportDto>(),
                sheetName ?? "税务条件数据",
                fileName ?? "税务条件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTaxConditionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "税务条件数据",
            fileName ?? "税务条件导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建税务条件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTaxCondition, bool>> QueryExpression(TaktTaxConditionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTaxCondition>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ApplicationCode != null && x.ApplicationCode.Contains(keywords))
                || (x.ConditionTypeCode != null && x.ConditionTypeCode.Contains(keywords))
                || (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.ConditionRecordCode != null && x.ConditionRecordCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicationCode))
        {
            var applicationCode = queryDto.ApplicationCode;
            exp = exp.And(x => x.ApplicationCode != null && x.ApplicationCode.Contains(applicationCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionTypeCode))
        {
            var conditionTypeCode = queryDto.ConditionTypeCode;
            exp = exp.And(x => x.ConditionTypeCode != null && x.ConditionTypeCode.Contains(conditionTypeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryCode))
        {
            var countryCode = queryDto.CountryCode;
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(countryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionRecordCode))
        {
            var conditionRecordCode = queryDto.ConditionRecordCode;
            exp = exp.And(x => x.ConditionRecordCode != null && x.ConditionRecordCode.Contains(conditionRecordCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.TaxConditionStatus.HasValue == true)
        {
            var taxConditionStatus = queryDto.TaxConditionStatus.Value;
            exp = exp.And(x => x.TaxConditionStatus == taxConditionStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktTaxConditionQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicationCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionTypeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionRecordCode))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.TaxConditionStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}

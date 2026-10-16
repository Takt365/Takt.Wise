// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktTaxCodeService.cs
// 创建时间：2026-10-14
// 创建人：Takt365(Cursor AI)
// 功能描述：税收代码应用服务实现
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
/// 税收代码应用服务
/// </summary>
public class TaktTaxCodeService : TaktServiceBase, ITaktTaxCodeService
{
    private readonly ITaktCompanyRepository<TaktTaxCode> _taxCodeRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="taxCodeRepository">税收代码仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTaxCodeService(
        ITaktCompanyRepository<TaktTaxCode> taxCodeRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _taxCodeRepository = taxCodeRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取税收代码列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTaxCodeDto>> GetTaxCodeListAsync(TaktTaxCodeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTaxCodeDto>.Create(
                new List<TaktTaxCodeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _taxCodeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTaxCodeDto>.Create(
            data.Adapt<List<TaktTaxCodeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCodeDto?> GetTaxCodeByIdAsync(long id)
    {
        var entity = await _taxCodeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTaxCodeDto>();
    }

    /// <summary>
    /// 获取税收代码选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTaxCodeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktTaxCode>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TaxCodeStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.TaxCode != null && x.TaxCode.Contains(normalizedKeyword!))
                || (x.Description != null && x.Description.Contains(normalizedKeyword!))
                || (x.TaxProcedureCode != null && x.TaxProcedureCode.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _taxCodeRepository.GetListAsync(
            predicate,
            x => x.TaxCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TaxCode,
            DictLabel = string.IsNullOrWhiteSpace(e.Description) ? e.TaxCode : $"{e.TaxCode} {e.Description}",
        }).ToList();
    }

    /// <summary>
    /// 创建税收代码
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCodeDto> CreateTaxCodeAsync(TaktTaxCodeCreateDto dto)
    {
        var entity = dto.Adapt<TaktTaxCode>();
        var isUnique_ix_takt_accounting_financial_tax_code_unique = await _uniqueValidator.IsUniqueAsync(
            _taxCodeRepository,
            x => x.TaxProcedureCode == entity.TaxProcedureCode
                && x.TaxCode == entity.TaxCode
                && x.TaxCategory == entity.TaxCategory);
        if (!isUnique_ix_takt_accounting_financial_tax_code_unique)
        {
            throw new TaktBusinessException("税收代码的TaxProcedureCode、TaxCode、TaxCategory已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _taxCodeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _taxCodeRepository.CreateAsync(entity);
        return await GetTaxCodeByIdAsync(entity.Id) ?? entity.Adapt<TaktTaxCodeDto>();
    }

    /// <summary>
    /// 更新税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCodeDto> UpdateTaxCodeAsync(long id, TaktTaxCodeUpdateDto dto)
    {
        var entity = await _taxCodeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("税收代码不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_tax_code_unique = await _uniqueValidator.IsUniqueAsync(
            _taxCodeRepository,
            x => x.TaxProcedureCode == entity.TaxProcedureCode
                && x.TaxCode == entity.TaxCode
                && x.TaxCategory == entity.TaxCategory,
            id);
        if (!isUnique_ix_takt_accounting_financial_tax_code_unique)
        {
            throw new TaktBusinessException("税收代码的TaxProcedureCode、TaxCode、TaxCategory已存在");
        }
        await _taxCodeRepository.UpdateAsync(entity);
        return await GetTaxCodeByIdAsync(id) ?? throw new TaktBusinessException("税收代码不存在");
    }

    /// <summary>
    /// 删除税收代码
    /// </summary>
    /// <param name="id">税收代码ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxCodeByIdAsync(long id)
    {
        var deleted = await _taxCodeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("税收代码不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除税收代码
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTaxCodeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTaxCodeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新税收代码状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCodeDto> UpdateTaxCodeStatusAsync(TaktTaxCodeStatusDto dto)
    {
        var entity = await _taxCodeRepository.GetByIdAsync(dto.TaxCodeId);
        if (entity == null)
        {
            throw new TaktBusinessException("税收代码不存在");
        }
        entity.TaxCodeStatus = dto.TaxCodeStatus;
        await _taxCodeRepository.UpdateAsync(entity);
        return await GetTaxCodeByIdAsync(dto.TaxCodeId) ?? throw new TaktBusinessException("税收代码不存在");
    }

    /// <summary>
    /// 更新税收代码排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTaxCodeDto> UpdateTaxCodeSortAsync(TaktTaxCodeSortDto dto)
    {
        var entity = await _taxCodeRepository.GetByIdAsync(dto.TaxCodeId);
        if (entity == null)
        {
            throw new TaktBusinessException("税收代码不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _taxCodeRepository.UpdateAsync(entity);
        return await GetTaxCodeByIdAsync(dto.TaxCodeId) ?? throw new TaktBusinessException("税收代码不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTaxCodeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTaxCodeTemplateDto>(
            sheetName ?? "税收代码导入模板",
            fileName ?? "税收代码导入模板.xlsx");
    }

    /// <summary>
    /// 导入税收代码
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTaxCodeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTaxCodeImportDto>(fileStream, sheetName ?? "税收代码导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _taxCodeRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTaxCode>();
                var importKey = $"{entity.TaxProcedureCode}|{entity.TaxCode}|{entity.TaxCategory}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TaxProcedureCode、TaxCode、TaxCategory）");
                }
                var isUnique_ix_takt_accounting_financial_tax_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _taxCodeRepository,
                    x => x.TaxProcedureCode == entity.TaxProcedureCode
                        && x.TaxCode == entity.TaxCode
                        && x.TaxCategory == entity.TaxCategory);
                if (!isUnique_ix_takt_accounting_financial_tax_code_unique)
                {
                    throw new TaktBusinessException("税收代码的TaxProcedureCode、TaxCode、TaxCategory已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _taxCodeRepository.CreateAsync(entity);
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
    /// 导出税收代码
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTaxCodeAsync(TaktTaxCodeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTaxCodeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTaxCodeExportDto>(),
                sheetName ?? "税收代码数据",
                fileName ?? "税收代码导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _taxCodeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTaxCodeExportDto>(),
                sheetName ?? "税收代码数据",
                fileName ?? "税收代码导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTaxCodeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "税收代码数据",
            fileName ?? "税收代码导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建税收代码查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTaxCode, bool>> QueryExpression(TaktTaxCodeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTaxCode>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TaxProcedureCode != null && x.TaxProcedureCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.TaxCategory != null && x.TaxCategory.Contains(keywords))
                || (x.Description != null && x.Description.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxProcedureCode))
        {
            var taxProcedureCode = queryDto.TaxProcedureCode;
            exp = exp.And(x => x.TaxProcedureCode != null && x.TaxProcedureCode.Contains(taxProcedureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCategory))
        {
            var taxCategory = queryDto.TaxCategory;
            exp = exp.And(x => x.TaxCategory != null && x.TaxCategory.Contains(taxCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Description))
        {
            var description = queryDto.Description;
            exp = exp.And(x => x.Description != null && x.Description.Contains(description));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.TaxCodeStatus.HasValue == true)
        {
            var taxCodeStatus = queryDto.TaxCodeStatus.Value;
            exp = exp.And(x => x.TaxCodeStatus == taxCodeStatus);
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
    private static bool HasAnyListQueryFilter(TaktTaxCodeQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TaxProcedureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Description))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.TaxCodeStatus.HasValue)
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

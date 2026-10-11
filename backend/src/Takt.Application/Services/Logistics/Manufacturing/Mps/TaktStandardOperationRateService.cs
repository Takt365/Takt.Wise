// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mps
// 文件名称：TaktStandardOperationRateService.cs
// 创建时间：2026-07-13
// 创建人：Takt365(Cursor AI)
// 功能描述：标准生产稼动率应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mps;

/// <summary>
/// 标准生产稼动率应用服务
/// </summary>
public class TaktStandardOperationRateService : TaktServiceBase, ITaktStandardOperationRateService
{
    private readonly ITaktCompanyRepository<TaktStandardOperationRate> _standardOperationRateRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStandardOperationRateService(
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _standardOperationRateRepository = standardOperationRateRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取标准生产稼动率列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStandardOperationRateDto>> GetStandardOperationRateListAsync(TaktStandardOperationRateQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _standardOperationRateRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStandardOperationRateDto>.Create(
            data.Adapt<List<TaktStandardOperationRateDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateDto?> GetStandardOperationRateByIdAsync(long id)
    {
        var entity = await _standardOperationRateRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktStandardOperationRateDto>();
    }

    /// <summary>
    /// 获取标准生产稼动率选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStandardOperationRateOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _standardOperationRateRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RateStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.PlantCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 按生产日期解析有效标准生产稼动率（%）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="operationType">稼动率类型（默认 1=人员）</param>
    /// <returns>稼动率(%)</returns>
    public async Task<decimal> GetEffectiveStandardOperationRatePercentAsync(string plantCode, DateTime prodDate, int operationType = 1)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        return await TaktStandardOperationRateResolver.ResolveEffectiveOperationRatePercentAsync(
            _standardOperationRateRepository,
            CurrentTenantCode,
            CurrentCompanyCode,
            plantCode.Trim(),
            prodDate,
            operationType);
    }

    /// <summary>
    /// 创建标准生产稼动率
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateDto> CreateStandardOperationRateAsync(TaktStandardOperationRateCreateDto dto)
    {
        var entity = dto.Adapt<TaktStandardOperationRate>();
        var isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique = await _uniqueValidator.IsUniqueAsync(
            _standardOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.FinancialYear == entity.FinancialYear
                && x.OperationType == entity.OperationType);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique)
        {
            throw new TaktBusinessException("标准生产稼动率的PlantCode、FinancialYear、OperationType已存在");
        }
        entity = await _standardOperationRateRepository.CreateAsync(entity);
        return await GetStandardOperationRateByIdAsync(entity.Id) ?? entity.Adapt<TaktStandardOperationRateDto>();
    }

    /// <summary>
    /// 更新标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateDto> UpdateStandardOperationRateAsync(long id, TaktStandardOperationRateUpdateDto dto)
    {
        var entity = await _standardOperationRateRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("标准生产稼动率不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique = await _uniqueValidator.IsUniqueAsync(
            _standardOperationRateRepository,
            x => x.PlantCode == entity.PlantCode
                && x.FinancialYear == entity.FinancialYear
                && x.OperationType == entity.OperationType,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique)
        {
            throw new TaktBusinessException("标准生产稼动率的PlantCode、FinancialYear、OperationType已存在");
        }
        await _standardOperationRateRepository.UpdateAsync(entity);
        return await GetStandardOperationRateByIdAsync(id) ?? throw new TaktBusinessException("标准生产稼动率不存在");
    }

    /// <summary>
    /// 删除标准生产稼动率
    /// </summary>
    /// <param name="id">标准生产稼动率ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationRateByIdAsync(long id)
    {
        var deleted = await _standardOperationRateRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("标准生产稼动率不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除标准生产稼动率
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStandardOperationRateBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteStandardOperationRateByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新标准生产稼动率状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStandardOperationRateDto> UpdateStandardOperationRateStatusAsync(TaktStandardOperationRateStatusDto dto)
    {
        var entity = await _standardOperationRateRepository.GetByIdAsync(dto.StandardOperationRateId);
        if (entity == null)
        {
            throw new TaktBusinessException("标准生产稼动率不存在");
        }
        entity.RateStatus = dto.RateStatus;
        await _standardOperationRateRepository.UpdateAsync(entity);
        return await GetStandardOperationRateByIdAsync(dto.StandardOperationRateId) ?? throw new TaktBusinessException("标准生产稼动率不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetStandardOperationRateTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktStandardOperationRateTemplateDto>(
            sheetName ?? "标准生产稼动率导入模板",
            fileName ?? "标准生产稼动率导入模板.xlsx");
    }

    /// <summary>
    /// 导入标准生产稼动率
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportStandardOperationRateAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktStandardOperationRateImportDto>(fileStream, sheetName ?? "标准生产稼动率导入模板");
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
                var entity = rows[i].Adapt<TaktStandardOperationRate>();
                var importKey = $"{entity.PlantCode}|{entity.FinancialYear}|{entity.OperationType}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、FinancialYear、OperationType）");
                }
                var isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique = await _uniqueValidator.IsUniqueAsync(
                    _standardOperationRateRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.FinancialYear == entity.FinancialYear
                        && x.OperationType == entity.OperationType);
                if (!isUnique_ix_takt_logistics_manufacturing_planning_standard_operation_rate_sor_unique)
                {
                    throw new TaktBusinessException("标准生产稼动率的PlantCode、FinancialYear、OperationType已存在");
                }
                await _standardOperationRateRepository.CreateAsync(entity);
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
    /// 导出标准生产稼动率
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStandardOperationRateAsync(TaktStandardOperationRateQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktStandardOperationRateQueryDto());
        var list = await _standardOperationRateRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStandardOperationRateExportDto>(),
                sheetName ?? "标准生产稼动率数据",
                fileName ?? "标准生产稼动率导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStandardOperationRateExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "标准生产稼动率数据",
            fileName ?? "标准生产稼动率导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建标准生产稼动率查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStandardOperationRate, bool>> QueryExpression(TaktStandardOperationRateQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStandardOperationRate>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.FinancialYear != null && x.FinancialYear.Contains(keywords))
                || SqlFunc.ToString(x.OperationType).Contains(keywords)
                || SqlFunc.ToString(x.OperationRate).Contains(keywords)
                || SqlFunc.ToString(x.RateStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EffectiveDate).Contains(keywords)
                || SqlFunc.ToString(x.ExpiryDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.FinancialYear))
        {
            exp = exp.And(x => x.FinancialYear != null && x.FinancialYear.Contains(queryDto.FinancialYear));
        }

        if (queryDto?.OperationType.HasValue == true)
        {
            exp = exp.And(x => x.OperationType == queryDto.OperationType);
        }

        if (queryDto?.OperationRate.HasValue == true)
        {
            exp = exp.And(x => x.OperationRate == queryDto.OperationRate);
        }

        if (queryDto?.RateStatus.HasValue == true)
        {
            exp = exp.And(x => x.RateStatus == queryDto.RateStatus);
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

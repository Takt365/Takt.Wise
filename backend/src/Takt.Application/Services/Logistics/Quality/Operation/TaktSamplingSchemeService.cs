// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Operation
// 文件名称：TaktSamplingSchemeService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：抽样方案应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Operation;
using Takt.Domain.Entities.Logistics.Quality.Operation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Operation;

/// <summary>
/// 抽样方案应用服务
/// </summary>
public class TaktSamplingSchemeService : TaktServiceBase, ITaktSamplingSchemeService
{
    private readonly ITaktCompanyRepository<TaktSamplingScheme> _samplingSchemeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="samplingSchemeRepository">抽样方案仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSamplingSchemeService(
        ITaktCompanyRepository<TaktSamplingScheme> samplingSchemeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _samplingSchemeRepository = samplingSchemeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取抽样方案列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSamplingSchemeDto>> GetSamplingSchemeListAsync(TaktSamplingSchemeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _samplingSchemeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSamplingSchemeDto>.Create(
            data.Adapt<List<TaktSamplingSchemeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSamplingSchemeDto?> GetSamplingSchemeByIdAsync(long id)
    {
        var entity = await _samplingSchemeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSamplingSchemeDto>();
    }

    /// <summary>
    /// 获取抽样方案选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSamplingSchemeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _samplingSchemeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SamplingSchemeStatus == 1,
            x => x.SamplingSchemeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.SamplingSchemeName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建抽样方案
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSamplingSchemeDto> CreateSamplingSchemeAsync(TaktSamplingSchemeCreateDto dto)
    {
        var entity = dto.Adapt<TaktSamplingScheme>();
        var isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique = await _uniqueValidator.IsUniqueAsync(
            _samplingSchemeRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SamplingSchemeCode == entity.SamplingSchemeCode);
        if (!isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique)
        {
            throw new TaktBusinessException("抽样方案的PlantCode、SamplingSchemeCode已存在");
        }
        entity = await _samplingSchemeRepository.CreateAsync(entity);
        return await GetSamplingSchemeByIdAsync(entity.Id) ?? entity.Adapt<TaktSamplingSchemeDto>();
    }

    /// <summary>
    /// 更新抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSamplingSchemeDto> UpdateSamplingSchemeAsync(long id, TaktSamplingSchemeUpdateDto dto)
    {
        var entity = await _samplingSchemeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("抽样方案不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique = await _uniqueValidator.IsUniqueAsync(
            _samplingSchemeRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SamplingSchemeCode == entity.SamplingSchemeCode,
            id);
        if (!isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique)
        {
            throw new TaktBusinessException("抽样方案的PlantCode、SamplingSchemeCode已存在");
        }
        await _samplingSchemeRepository.UpdateAsync(entity);
        return await GetSamplingSchemeByIdAsync(id) ?? throw new TaktBusinessException("抽样方案不存在");
    }

    /// <summary>
    /// 删除抽样方案
    /// </summary>
    /// <param name="id">抽样方案ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSamplingSchemeByIdAsync(long id)
    {
        var deleted = await _samplingSchemeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("抽样方案不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除抽样方案
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSamplingSchemeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSamplingSchemeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新抽样方案状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSamplingSchemeDto> UpdateSamplingSchemeStatusAsync(TaktSamplingSchemeStatusDto dto)
    {
        var entity = await _samplingSchemeRepository.GetByIdAsync(dto.SamplingSchemeId);
        if (entity == null)
        {
            throw new TaktBusinessException("抽样方案不存在");
        }
        entity.SamplingSchemeStatus = dto.SamplingSchemeStatus;
        await _samplingSchemeRepository.UpdateAsync(entity);
        return await GetSamplingSchemeByIdAsync(dto.SamplingSchemeId) ?? throw new TaktBusinessException("抽样方案不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSamplingSchemeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSamplingSchemeTemplateDto>(
            sheetName ?? "抽样方案导入模板",
            fileName ?? "抽样方案导入模板.xlsx");
    }

    /// <summary>
    /// 导入抽样方案
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSamplingSchemeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSamplingSchemeImportDto>(fileStream, sheetName ?? "抽样方案导入模板");
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
                var entity = rows[i].Adapt<TaktSamplingScheme>();
                var importKey = $"{entity.PlantCode}|{entity.SamplingSchemeCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SamplingSchemeCode）");
                }
                var isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique = await _uniqueValidator.IsUniqueAsync(
                    _samplingSchemeRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SamplingSchemeCode == entity.SamplingSchemeCode);
                if (!isUnique_ix_takt_logistics_quality_sampling_scheme_ss_unique)
                {
                    throw new TaktBusinessException("抽样方案的PlantCode、SamplingSchemeCode已存在");
                }
                await _samplingSchemeRepository.CreateAsync(entity);
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
    /// 导出抽样方案
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSamplingSchemeAsync(TaktSamplingSchemeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSamplingSchemeQueryDto());
        var list = await _samplingSchemeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSamplingSchemeExportDto>(),
                sheetName ?? "抽样方案数据",
                fileName ?? "抽样方案导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSamplingSchemeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "抽样方案数据",
            fileName ?? "抽样方案导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建抽样方案查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSamplingScheme, bool>> QueryExpression(TaktSamplingSchemeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSamplingScheme>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(keywords))
                || (x.SamplingSchemeName != null && x.SamplingSchemeName.Contains(keywords))
                || SqlFunc.ToString(x.SamplingSchemeType).Contains(keywords)
                || SqlFunc.ToString(x.SamplingStandard).Contains(keywords)
                || SqlFunc.ToString(x.InspectionLevel).Contains(keywords)
                || SqlFunc.ToString(x.AqlValue).Contains(keywords)
                || SqlFunc.ToString(x.LotSizeMin).Contains(keywords)
                || SqlFunc.ToString(x.LotSizeMax).Contains(keywords)
                || SqlFunc.ToString(x.SampleSize).Contains(keywords)
                || SqlFunc.ToString(x.AcceptanceNumber).Contains(keywords)
                || SqlFunc.ToString(x.RejectionNumber).Contains(keywords)
                || SqlFunc.ToString(x.InspectionStrictness).Contains(keywords)
                || SqlFunc.ToString(x.IsTransferRuleEnabled).Contains(keywords)
                || (x.TransferRuleConfig != null && x.TransferRuleConfig.Contains(keywords))
                || SqlFunc.ToString(x.SamplingSchemeStatus).Contains(keywords)
                || (x.SchemeDescription != null && x.SchemeDescription.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SamplingSchemeCode))
        {
            exp = exp.And(x => x.SamplingSchemeCode != null && x.SamplingSchemeCode.Contains(queryDto.SamplingSchemeCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.SamplingSchemeName))
        {
            exp = exp.And(x => x.SamplingSchemeName != null && x.SamplingSchemeName.Contains(queryDto.SamplingSchemeName));
        }

        if (queryDto?.SamplingSchemeType.HasValue == true)
        {
            exp = exp.And(x => x.SamplingSchemeType == queryDto.SamplingSchemeType);
        }

        if (queryDto?.SamplingStandard.HasValue == true)
        {
            exp = exp.And(x => x.SamplingStandard == queryDto.SamplingStandard);
        }

        if (queryDto?.InspectionLevel.HasValue == true)
        {
            exp = exp.And(x => x.InspectionLevel == queryDto.InspectionLevel);
        }

        if (queryDto?.AqlValue.HasValue == true)
        {
            exp = exp.And(x => x.AqlValue == queryDto.AqlValue);
        }

        if (queryDto?.LotSizeMin.HasValue == true)
        {
            exp = exp.And(x => x.LotSizeMin == queryDto.LotSizeMin);
        }

        if (queryDto?.LotSizeMax.HasValue == true)
        {
            exp = exp.And(x => x.LotSizeMax == queryDto.LotSizeMax);
        }

        if (queryDto?.SampleSize.HasValue == true)
        {
            exp = exp.And(x => x.SampleSize == queryDto.SampleSize);
        }

        if (queryDto?.AcceptanceNumber.HasValue == true)
        {
            exp = exp.And(x => x.AcceptanceNumber == queryDto.AcceptanceNumber);
        }

        if (queryDto?.RejectionNumber.HasValue == true)
        {
            exp = exp.And(x => x.RejectionNumber == queryDto.RejectionNumber);
        }

        if (queryDto?.InspectionStrictness.HasValue == true)
        {
            exp = exp.And(x => x.InspectionStrictness == queryDto.InspectionStrictness);
        }

        if (queryDto?.IsTransferRuleEnabled.HasValue == true)
        {
            exp = exp.And(x => x.IsTransferRuleEnabled == queryDto.IsTransferRuleEnabled);
        }

        if (!string.IsNullOrEmpty(queryDto?.TransferRuleConfig))
        {
            exp = exp.And(x => x.TransferRuleConfig != null && x.TransferRuleConfig.Contains(queryDto.TransferRuleConfig));
        }

        if (queryDto?.SamplingSchemeStatus.HasValue == true)
        {
            exp = exp.And(x => x.SamplingSchemeStatus == queryDto.SamplingSchemeStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.SchemeDescription))
        {
            exp = exp.And(x => x.SchemeDescription != null && x.SchemeDescription.Contains(queryDto.SchemeDescription));
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

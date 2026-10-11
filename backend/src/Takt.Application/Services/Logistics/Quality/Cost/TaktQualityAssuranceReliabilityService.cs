// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceReliabilityService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务信赖性评价ORT费用明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Domain.Entities.Logistics.Quality.Cost;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Quality.Cost;

/// <summary>
/// 品质业务信赖性评价ORT费用明细应用服务
/// </summary>
public class TaktQualityAssuranceReliabilityService : TaktServiceBase, ITaktQualityAssuranceReliabilityService
{
    private readonly ITaktCompanyRepository<TaktQualityAssuranceReliability> _qualityAssuranceReliabilityRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssurance> _qualityAssuranceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityAssuranceReliabilityRepository">品质业务信赖性评价ORT费用明细仓储</param>
    /// <param name="qualityAssuranceRepository">品质业务主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityAssuranceReliabilityService(
        ITaktCompanyRepository<TaktQualityAssuranceReliability> qualityAssuranceReliabilityRepository,
        ITaktCompanyRepository<TaktQualityAssurance> qualityAssuranceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityAssuranceReliabilityRepository = qualityAssuranceReliabilityRepository;
        _qualityAssuranceRepository = qualityAssuranceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质业务信赖性评价ORT费用明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityAssuranceReliabilityDto>> GetQualityAssuranceReliabilityListAsync(TaktQualityAssuranceReliabilityQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityAssuranceReliabilityDto>.Create(
                new List<TaktQualityAssuranceReliabilityDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityAssuranceReliabilityRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityAssuranceReliabilityDto>.Create(
            data.Adapt<List<TaktQualityAssuranceReliabilityDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="id">品质业务信赖性评价ORT费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceReliabilityDto?> GetQualityAssuranceReliabilityByIdAsync(long id)
    {
        var entity = await _qualityAssuranceReliabilityRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityAssuranceReliabilityDto>();
    }

    /// <summary>
    /// 获取品质业务信赖性评价ORT费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityAssuranceReliabilityOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityAssuranceReliabilityRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.QualityAssuranceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityAssuranceCode,
            DictLabel = e.QualityAssuranceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceReliabilityDto> CreateQualityAssuranceReliabilityAsync(TaktQualityAssuranceReliabilityCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityAssuranceReliability>();
        entity.IsObsolete = 0;
        await StampQualityAssuranceReliabilityQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceReliabilityRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的QualityAssuranceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityAssuranceReliabilityRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityAssuranceReliabilityRepository.CreateAsync(entity);
        return await GetQualityAssuranceReliabilityByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityAssuranceReliabilityDto>();
    }

    /// <summary>
    /// 更新品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="id">品质业务信赖性评价ORT费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceReliabilityDto> UpdateQualityAssuranceReliabilityAsync(long id, TaktQualityAssuranceReliabilityUpdateDto dto)
    {
        var entity = await _qualityAssuranceReliabilityRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityAssuranceReliabilityQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceReliabilityRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的QualityAssuranceId、LineNumber已存在");
        }
        await _qualityAssuranceReliabilityRepository.UpdateAsync(entity);
        return await GetQualityAssuranceReliabilityByIdAsync(id) ?? throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在");
    }

    /// <summary>
    /// 删除品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="id">品质业务信赖性评价ORT费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceReliabilityByIdAsync(long id)
    {
        var entity = await _qualityAssuranceReliabilityRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityAssuranceReliabilityRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceReliabilityBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityAssuranceReliabilityByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新品质业务信赖性评价ORT费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceReliabilityDto> UpdateQualityAssuranceReliabilityObsoleteAsync(TaktQualityAssuranceReliabilityObsoleteDto dto)
    {
        var entity = await _qualityAssuranceReliabilityRepository.GetByIdAsync(dto.QualityAssuranceReliabilityId);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityAssuranceReliabilityRepository.UpdateAsync(entity);
        return await GetQualityAssuranceReliabilityByIdAsync(dto.QualityAssuranceReliabilityId) ?? throw new TaktBusinessException("品质业务信赖性评价ORT费用明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityAssuranceReliabilityTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityAssuranceReliabilityTemplateDto>(
            sheetName ?? "品质业务信赖性评价ORT费用明细导入模板",
            fileName ?? "品质业务信赖性评价ORT费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityAssuranceReliabilityAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityAssuranceReliabilityImportDto>(fileStream, sheetName ?? "品质业务信赖性评价ORT费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityAssuranceReliability>();
                var importDto = rows[i].Adapt<TaktQualityAssuranceReliabilityCreateDto>();
                await StampQualityAssuranceReliabilityQualityAssuranceAsync(entity, importDto);
                var importKey = $"{entity.QualityAssuranceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityAssuranceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityAssuranceReliabilityRepository,
                    x => x.QualityAssuranceId == entity.QualityAssuranceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_assurance_reliability_line_unique)
                {
                    throw new TaktBusinessException("品质业务信赖性评价ORT费用明细的QualityAssuranceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityAssuranceReliabilityRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityAssuranceReliabilityRepository.CreateAsync(entity);
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
    /// 导出品质业务信赖性评价ORT费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityAssuranceReliabilityAsync(TaktQualityAssuranceReliabilityQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityAssuranceReliabilityQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceReliabilityExportDto>(),
                sheetName ?? "品质业务信赖性评价ORT费用明细数据",
                fileName ?? "品质业务信赖性评价ORT费用明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityAssuranceReliabilityRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceReliabilityExportDto>(),
                sheetName ?? "品质业务信赖性评价ORT费用明细数据",
                fileName ?? "品质业务信赖性评价ORT费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityAssuranceReliabilityExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质业务信赖性评价ORT费用明细数据",
            fileName ?? "品质业务信赖性评价ORT费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步品质业务信赖性评价ORT费用明细主表外键（ManyToOne → 品质业务主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityAssuranceReliabilityQualityAssuranceAsync(TaktQualityAssuranceReliability entity, TaktQualityAssuranceReliabilityCreateDto dto)
    {
        if (dto.QualityAssuranceId <= 0)
        {
            return;
        }
        var master = await _qualityAssuranceRepository.GetByIdAsync(dto.QualityAssuranceId);
        if (master == null)
        {
            throw new TaktBusinessException("品质业务主不存在");
        }
        entity.QualityAssuranceId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.QualityAssuranceCode))
        {
            entity.QualityAssuranceCode = master.QualityAssuranceCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质业务信赖性评价ORT费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityAssuranceReliability, bool>> QueryExpression(TaktQualityAssuranceReliabilityQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityAssuranceReliability>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(keywords))
                || (x.ReliabilityNote != null && x.ReliabilityNote.Contains(keywords))
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

        if (queryDto?.QualityAssuranceId.HasValue == true)
        {
            var qualityAssuranceId = queryDto.QualityAssuranceId.Value;
            exp = exp.And(x => x.QualityAssuranceId == qualityAssuranceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityAssuranceCode))
        {
            var qualityAssuranceCode = queryDto.QualityAssuranceCode;
            exp = exp.And(x => x.QualityAssuranceCode != null && x.QualityAssuranceCode.Contains(qualityAssuranceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.TestCost.HasValue == true)
        {
            var testCost = queryDto.TestCost.Value;
            exp = exp.And(x => x.TestCost == testCost);
        }

        if (queryDto?.WorkTimeMinutes.HasValue == true)
        {
            var workTimeMinutes = queryDto.WorkTimeMinutes.Value;
            exp = exp.And(x => x.WorkTimeMinutes == workTimeMinutes);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            var otherExpenses = queryDto.OtherExpenses.Value;
            exp = exp.And(x => x.OtherExpenses == otherExpenses);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReliabilityNote))
        {
            var reliabilityNote = queryDto.ReliabilityNote;
            exp = exp.And(x => x.ReliabilityNote != null && x.ReliabilityNote.Contains(reliabilityNote));
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
    private static bool HasAnyListQueryFilter(TaktQualityAssuranceReliabilityQueryDto? queryDto)
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
        if (queryDto.QualityAssuranceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityAssuranceCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.TestCost.HasValue)
        {
            return true;
        }
        if (queryDto.WorkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.OtherExpenses.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReliabilityNote))
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
        if (queryDto.IsObsolete.HasValue)
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

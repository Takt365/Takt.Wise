// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityAssuranceFirstArticleService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质业务初期定期检定费用明细应用服务实现
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
/// 品质业务初期定期检定费用明细应用服务
/// </summary>
public class TaktQualityAssuranceFirstArticleService : TaktServiceBase, ITaktQualityAssuranceFirstArticleService
{
    private readonly ITaktCompanyRepository<TaktQualityAssuranceFirstArticle> _qualityAssuranceFirstArticleRepository;
    private readonly ITaktCompanyRepository<TaktQualityAssurance> _qualityAssuranceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityAssuranceFirstArticleRepository">品质业务初期定期检定费用明细仓储</param>
    /// <param name="qualityAssuranceRepository">品质业务主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityAssuranceFirstArticleService(
        ITaktCompanyRepository<TaktQualityAssuranceFirstArticle> qualityAssuranceFirstArticleRepository,
        ITaktCompanyRepository<TaktQualityAssurance> qualityAssuranceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityAssuranceFirstArticleRepository = qualityAssuranceFirstArticleRepository;
        _qualityAssuranceRepository = qualityAssuranceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质业务初期定期检定费用明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityAssuranceFirstArticleDto>> GetQualityAssuranceFirstArticleListAsync(TaktQualityAssuranceFirstArticleQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityAssuranceFirstArticleDto>.Create(
                new List<TaktQualityAssuranceFirstArticleDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityAssuranceFirstArticleRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityAssuranceFirstArticleDto>.Create(
            data.Adapt<List<TaktQualityAssuranceFirstArticleDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="id">品质业务初期定期检定费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceFirstArticleDto?> GetQualityAssuranceFirstArticleByIdAsync(long id)
    {
        var entity = await _qualityAssuranceFirstArticleRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityAssuranceFirstArticleDto>();
    }

    /// <summary>
    /// 获取品质业务初期定期检定费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityAssuranceFirstArticleOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityAssuranceFirstArticleRepository.GetListAsync(
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
    /// 创建品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceFirstArticleDto> CreateQualityAssuranceFirstArticleAsync(TaktQualityAssuranceFirstArticleCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityAssuranceFirstArticle>();
        entity.IsObsolete = 0;
        await StampQualityAssuranceFirstArticleQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceFirstArticleRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细的QualityAssuranceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityAssuranceFirstArticleRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityAssuranceFirstArticleRepository.CreateAsync(entity);
        return await GetQualityAssuranceFirstArticleByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityAssuranceFirstArticleDto>();
    }

    /// <summary>
    /// 更新品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="id">品质业务初期定期检定费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceFirstArticleDto> UpdateQualityAssuranceFirstArticleAsync(long id, TaktQualityAssuranceFirstArticleUpdateDto dto)
    {
        var entity = await _qualityAssuranceFirstArticleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityAssuranceFirstArticleQualityAssuranceAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityAssuranceFirstArticleRepository,
            x => x.QualityAssuranceId == entity.QualityAssuranceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细的QualityAssuranceId、LineNumber已存在");
        }
        await _qualityAssuranceFirstArticleRepository.UpdateAsync(entity);
        return await GetQualityAssuranceFirstArticleByIdAsync(id) ?? throw new TaktBusinessException("品质业务初期定期检定费用明细不存在");
    }

    /// <summary>
    /// 删除品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="id">品质业务初期定期检定费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceFirstArticleByIdAsync(long id)
    {
        var entity = await _qualityAssuranceFirstArticleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityAssuranceFirstArticleRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityAssuranceFirstArticleBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityAssuranceFirstArticleByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新品质业务初期定期检定费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityAssuranceFirstArticleDto> UpdateQualityAssuranceFirstArticleObsoleteAsync(TaktQualityAssuranceFirstArticleObsoleteDto dto)
    {
        var entity = await _qualityAssuranceFirstArticleRepository.GetByIdAsync(dto.QualityAssuranceFirstArticleId);
        if (entity == null)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("品质业务初期定期检定费用明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityAssuranceFirstArticleRepository.UpdateAsync(entity);
        return await GetQualityAssuranceFirstArticleByIdAsync(dto.QualityAssuranceFirstArticleId) ?? throw new TaktBusinessException("品质业务初期定期检定费用明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityAssuranceFirstArticleTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityAssuranceFirstArticleTemplateDto>(
            sheetName ?? "品质业务初期定期检定费用明细导入模板",
            fileName ?? "品质业务初期定期检定费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityAssuranceFirstArticleAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityAssuranceFirstArticleImportDto>(fileStream, sheetName ?? "品质业务初期定期检定费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityAssuranceFirstArticle>();
                var importDto = rows[i].Adapt<TaktQualityAssuranceFirstArticleCreateDto>();
                await StampQualityAssuranceFirstArticleQualityAssuranceAsync(entity, importDto);
                var importKey = $"{entity.QualityAssuranceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityAssuranceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityAssuranceFirstArticleRepository,
                    x => x.QualityAssuranceId == entity.QualityAssuranceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_assurance_first_article_line_unique)
                {
                    throw new TaktBusinessException("品质业务初期定期检定费用明细的QualityAssuranceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityAssuranceFirstArticleRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityAssuranceId == entity.QualityAssuranceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityAssuranceCode) ? entity.QualityAssuranceCode : entity.QualityAssuranceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityAssuranceFirstArticleRepository.CreateAsync(entity);
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
    /// 导出品质业务初期定期检定费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityAssuranceFirstArticleAsync(TaktQualityAssuranceFirstArticleQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityAssuranceFirstArticleQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceFirstArticleExportDto>(),
                sheetName ?? "品质业务初期定期检定费用明细数据",
                fileName ?? "品质业务初期定期检定费用明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityAssuranceFirstArticleRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityAssuranceFirstArticleExportDto>(),
                sheetName ?? "品质业务初期定期检定费用明细数据",
                fileName ?? "品质业务初期定期检定费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityAssuranceFirstArticleExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质业务初期定期检定费用明细数据",
            fileName ?? "品质业务初期定期检定费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步品质业务初期定期检定费用明细主表外键（ManyToOne → 品质业务主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityAssuranceFirstArticleQualityAssuranceAsync(TaktQualityAssuranceFirstArticle entity, TaktQualityAssuranceFirstArticleCreateDto dto)
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
    /// 构建品质业务初期定期检定费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityAssuranceFirstArticle, bool>> QueryExpression(TaktQualityAssuranceFirstArticleQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityAssuranceFirstArticle>();

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
                || (x.QualificationNote != null && x.QualificationNote.Contains(keywords))
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

        if (queryDto?.QualificationCost.HasValue == true)
        {
            var qualificationCost = queryDto.QualificationCost.Value;
            exp = exp.And(x => x.QualificationCost == qualificationCost);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.QualificationNote))
        {
            var qualificationNote = queryDto.QualificationNote;
            exp = exp.And(x => x.QualificationNote != null && x.QualificationNote.Contains(qualificationNote));
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
    private static bool HasAnyListQueryFilter(TaktQualityAssuranceFirstArticleQueryDto? queryDto)
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
        if (queryDto.QualificationCost.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.QualificationNote))
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

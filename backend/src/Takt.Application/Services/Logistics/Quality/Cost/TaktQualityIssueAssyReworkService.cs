// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueAssyReworkService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题组装不良改修费用明细应用服务实现
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
/// 质量问题组装不良改修费用明细应用服务
/// </summary>
public class TaktQualityIssueAssyReworkService : TaktServiceBase, ITaktQualityIssueAssyReworkService
{
    private readonly ITaktCompanyRepository<TaktQualityIssueAssyRework> _qualityIssueAssyReworkRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueAssyReworkRepository">质量问题组装不良改修费用明细仓储</param>
    /// <param name="qualityIssueRepository">品质问题应对主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIssueAssyReworkService(
        ITaktCompanyRepository<TaktQualityIssueAssyRework> qualityIssueAssyReworkRepository,
        ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIssueAssyReworkRepository = qualityIssueAssyReworkRepository;
        _qualityIssueRepository = qualityIssueRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量问题组装不良改修费用明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIssueAssyReworkDto>> GetQualityIssueAssyReworkListAsync(TaktQualityIssueAssyReworkQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIssueAssyReworkDto>.Create(
                new List<TaktQualityIssueAssyReworkDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIssueAssyReworkRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIssueAssyReworkDto>.Create(
            data.Adapt<List<TaktQualityIssueAssyReworkDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto?> GetQualityIssueAssyReworkByIdAsync(long id)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIssueAssyReworkDto>();
    }

    /// <summary>
    /// 获取质量问题组装不良改修费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIssueAssyReworkOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIssueAssyReworkRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.QualityIssueCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityIssueCode,
            DictLabel = e.QualityIssueCode,
        }).ToList();
    }

    /// <summary>
    /// 创建质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto> CreateQualityIssueAssyReworkAsync(TaktQualityIssueAssyReworkCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIssueAssyRework>();
        entity.IsObsolete = 0;
        await StampQualityIssueAssyReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueAssyReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIssueAssyReworkRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIssueAssyReworkRepository.CreateAsync(entity);
        return await GetQualityIssueAssyReworkByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIssueAssyReworkDto>();
    }

    /// <summary>
    /// 更新质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto> UpdateQualityIssueAssyReworkAsync(long id, TaktQualityIssueAssyReworkUpdateDto dto)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityIssueAssyReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueAssyReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        await _qualityIssueAssyReworkRepository.UpdateAsync(entity);
        return await GetQualityIssueAssyReworkByIdAsync(id) ?? throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
    }

    /// <summary>
    /// 删除质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题组装不良改修费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueAssyReworkByIdAsync(long id)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityIssueAssyReworkRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueAssyReworkBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIssueAssyReworkByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新质量问题组装不良改修费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueAssyReworkDto> UpdateQualityIssueAssyReworkObsoleteAsync(TaktQualityIssueAssyReworkObsoleteDto dto)
    {
        var entity = await _qualityIssueAssyReworkRepository.GetByIdAsync(dto.QualityIssueAssyReworkId);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityIssueAssyReworkRepository.UpdateAsync(entity);
        return await GetQualityIssueAssyReworkByIdAsync(dto.QualityIssueAssyReworkId) ?? throw new TaktBusinessException("质量问题组装不良改修费用明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIssueAssyReworkTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIssueAssyReworkTemplateDto>(
            sheetName ?? "质量问题组装不良改修费用明细导入模板",
            fileName ?? "质量问题组装不良改修费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIssueAssyReworkAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIssueAssyReworkImportDto>(fileStream, sheetName ?? "质量问题组装不良改修费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIssueAssyRework>();
                var importDto = rows[i].Adapt<TaktQualityIssueAssyReworkCreateDto>();
                await StampQualityIssueAssyReworkQualityIssueAsync(entity, importDto);
                var importKey = $"{entity.QualityIssueId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIssueId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIssueAssyReworkRepository,
                    x => x.QualityIssueId == entity.QualityIssueId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_issue_assy_rework_line_unique)
                {
                    throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIssueAssyReworkRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIssueAssyReworkRepository.CreateAsync(entity);
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
    /// 导出质量问题组装不良改修费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIssueAssyReworkAsync(TaktQualityIssueAssyReworkQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIssueAssyReworkQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueAssyReworkExportDto>(),
                sheetName ?? "质量问题组装不良改修费用明细数据",
                fileName ?? "质量问题组装不良改修费用明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIssueAssyReworkRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueAssyReworkExportDto>(),
                sheetName ?? "质量问题组装不良改修费用明细数据",
                fileName ?? "质量问题组装不良改修费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIssueAssyReworkExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量问题组装不良改修费用明细数据",
            fileName ?? "质量问题组装不良改修费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步质量问题组装不良改修费用明细主表外键（ManyToOne → 品质问题应对主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIssueAssyReworkQualityIssueAsync(TaktQualityIssueAssyRework entity, TaktQualityIssueAssyReworkCreateDto dto)
    {
        if (dto.QualityIssueId <= 0)
        {
            return;
        }
        var master = await _qualityIssueRepository.GetByIdAsync(dto.QualityIssueId);
        if (master == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在");
        }
        entity.QualityIssueId = master.Id;
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
        if (string.IsNullOrEmpty(entity.QualityIssueCode))
        {
            entity.QualityIssueCode = master.QualityIssueCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建质量问题组装不良改修费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIssueAssyRework, bool>> QueryExpression(TaktQualityIssueAssyReworkQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIssueAssyRework>();

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
                || (x.QualityIssueCode != null && x.QualityIssueCode.Contains(keywords))
                || (x.AssyDefectParts != null && x.AssyDefectParts.Contains(keywords))
                || (x.AssyReworkNote != null && x.AssyReworkNote.Contains(keywords))
                || (x.AssyCustomerName1 != null && x.AssyCustomerName1.Contains(keywords))
                || (x.AssyDebitNoteCode != null && x.AssyDebitNoteCode.Contains(keywords))
                || (x.AssyNote != null && x.AssyNote.Contains(keywords))
                || (x.ResponseGroupName != null && x.ResponseGroupName.Contains(keywords))
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

        if (queryDto?.QualityIssueId.HasValue == true)
        {
            var qualityIssueId = queryDto.QualityIssueId.Value;
            exp = exp.And(x => x.QualityIssueId == qualityIssueId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityIssueCode))
        {
            var qualityIssueCode = queryDto.QualityIssueCode;
            exp = exp.And(x => x.QualityIssueCode != null && x.QualityIssueCode.Contains(qualityIssueCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssyDefectParts))
        {
            var assyDefectParts = queryDto.AssyDefectParts;
            exp = exp.And(x => x.AssyDefectParts != null && x.AssyDefectParts.Contains(assyDefectParts));
        }

        if (queryDto?.AssyReworkCost.HasValue == true)
        {
            var assyReworkCost = queryDto.AssyReworkCost.Value;
            exp = exp.And(x => x.AssyReworkCost == assyReworkCost);
        }

        if (queryDto?.AssyReworkTimeMinutes.HasValue == true)
        {
            var assyReworkTimeMinutes = queryDto.AssyReworkTimeMinutes.Value;
            exp = exp.And(x => x.AssyReworkTimeMinutes == assyReworkTimeMinutes);
        }

        if (queryDto?.AssyReinspectionTimeMinutes.HasValue == true)
        {
            var assyReinspectionTimeMinutes = queryDto.AssyReinspectionTimeMinutes.Value;
            exp = exp.And(x => x.AssyReinspectionTimeMinutes == assyReinspectionTimeMinutes);
        }

        if (queryDto?.AssyTravelCost.HasValue == true)
        {
            var assyTravelCost = queryDto.AssyTravelCost.Value;
            exp = exp.And(x => x.AssyTravelCost == assyTravelCost);
        }

        if (queryDto?.AssyWarehouseCost.HasValue == true)
        {
            var assyWarehouseCost = queryDto.AssyWarehouseCost.Value;
            exp = exp.And(x => x.AssyWarehouseCost == assyWarehouseCost);
        }

        if (queryDto?.AssyOtherExpenses.HasValue == true)
        {
            var assyOtherExpenses = queryDto.AssyOtherExpenses.Value;
            exp = exp.And(x => x.AssyOtherExpenses == assyOtherExpenses);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssyReworkNote))
        {
            var assyReworkNote = queryDto.AssyReworkNote;
            exp = exp.And(x => x.AssyReworkNote != null && x.AssyReworkNote.Contains(assyReworkNote));
        }

        if (queryDto?.AssyScrapCost.HasValue == true)
        {
            var assyScrapCost = queryDto.AssyScrapCost.Value;
            exp = exp.And(x => x.AssyScrapCost == assyScrapCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssyCustomerName1))
        {
            var assyCustomerName1 = queryDto.AssyCustomerName1;
            exp = exp.And(x => x.AssyCustomerName1 != null && x.AssyCustomerName1.Contains(assyCustomerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssyDebitNoteCode))
        {
            var assyDebitNoteCode = queryDto.AssyDebitNoteCode;
            exp = exp.And(x => x.AssyDebitNoteCode != null && x.AssyDebitNoteCode.Contains(assyDebitNoteCode));
        }

        if (queryDto?.AssyOtherExpenses2.HasValue == true)
        {
            var assyOtherExpenses2 = queryDto.AssyOtherExpenses2.Value;
            exp = exp.And(x => x.AssyOtherExpenses2 == assyOtherExpenses2);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssyNote))
        {
            var assyNote = queryDto.AssyNote;
            exp = exp.And(x => x.AssyNote != null && x.AssyNote.Contains(assyNote));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponseGroupName))
        {
            var assyRecorder = queryDto.ResponseGroupName;
            exp = exp.And(x => x.ResponseGroupName != null && x.ResponseGroupName.Contains(assyRecorder));
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
    private static bool HasAnyListQueryFilter(TaktQualityIssueAssyReworkQueryDto? queryDto)
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
        if (queryDto.QualityIssueId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityIssueCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssyDefectParts))
        {
            return true;
        }
        if (queryDto.AssyReworkCost.HasValue)
        {
            return true;
        }
        if (queryDto.AssyReworkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.AssyReinspectionTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.AssyTravelCost.HasValue)
        {
            return true;
        }
        if (queryDto.AssyWarehouseCost.HasValue)
        {
            return true;
        }
        if (queryDto.AssyOtherExpenses.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssyReworkNote))
        {
            return true;
        }
        if (queryDto.AssyScrapCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssyCustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssyDebitNoteCode))
        {
            return true;
        }
        if (queryDto.AssyOtherExpenses2.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssyNote))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ResponseGroupName))
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

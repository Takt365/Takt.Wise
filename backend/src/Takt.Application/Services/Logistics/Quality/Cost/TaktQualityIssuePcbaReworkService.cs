// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIssuePcbaReworkService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题PCBA不良改修费用明细应用服务实现
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
/// 质量问题PCBA不良改修费用明细应用服务
/// </summary>
public class TaktQualityIssuePcbaReworkService : TaktServiceBase, ITaktQualityIssuePcbaReworkService
{
    private readonly ITaktCompanyRepository<TaktQualityIssuePcbaRework> _qualityIssuePcbaReworkRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssuePcbaReworkRepository">质量问题PCBA不良改修费用明细仓储</param>
    /// <param name="qualityIssueRepository">品质问题应对主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIssuePcbaReworkService(
        ITaktCompanyRepository<TaktQualityIssuePcbaRework> qualityIssuePcbaReworkRepository,
        ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIssuePcbaReworkRepository = qualityIssuePcbaReworkRepository;
        _qualityIssueRepository = qualityIssueRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIssuePcbaReworkDto>> GetQualityIssuePcbaReworkListAsync(TaktQualityIssuePcbaReworkQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIssuePcbaReworkDto>.Create(
                new List<TaktQualityIssuePcbaReworkDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIssuePcbaReworkRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIssuePcbaReworkDto>.Create(
            data.Adapt<List<TaktQualityIssuePcbaReworkDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssuePcbaReworkDto?> GetQualityIssuePcbaReworkByIdAsync(long id)
    {
        var entity = await _qualityIssuePcbaReworkRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIssuePcbaReworkDto>();
    }

    /// <summary>
    /// 获取质量问题PCBA不良改修费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIssuePcbaReworkOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIssuePcbaReworkRepository.GetListAsync(
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
    /// 创建质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssuePcbaReworkDto> CreateQualityIssuePcbaReworkAsync(TaktQualityIssuePcbaReworkCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIssuePcbaRework>();
        entity.IsObsolete = 0;
        await StampQualityIssuePcbaReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssuePcbaReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIssuePcbaReworkRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIssuePcbaReworkRepository.CreateAsync(entity);
        return await GetQualityIssuePcbaReworkByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIssuePcbaReworkDto>();
    }

    /// <summary>
    /// 更新质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssuePcbaReworkDto> UpdateQualityIssuePcbaReworkAsync(long id, TaktQualityIssuePcbaReworkUpdateDto dto)
    {
        var entity = await _qualityIssuePcbaReworkRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityIssuePcbaReworkQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssuePcbaReworkRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细的QualityIssueId、LineNumber已存在");
        }
        await _qualityIssuePcbaReworkRepository.UpdateAsync(entity);
        return await GetQualityIssuePcbaReworkByIdAsync(id) ?? throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在");
    }

    /// <summary>
    /// 删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="id">质量问题PCBA不良改修费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssuePcbaReworkByIdAsync(long id)
    {
        var entity = await _qualityIssuePcbaReworkRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityIssuePcbaReworkRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssuePcbaReworkBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIssuePcbaReworkByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新质量问题PCBA不良改修费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssuePcbaReworkDto> UpdateQualityIssuePcbaReworkObsoleteAsync(TaktQualityIssuePcbaReworkObsoleteDto dto)
    {
        var entity = await _qualityIssuePcbaReworkRepository.GetByIdAsync(dto.QualityIssuePcbaReworkId);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityIssuePcbaReworkRepository.UpdateAsync(entity);
        return await GetQualityIssuePcbaReworkByIdAsync(dto.QualityIssuePcbaReworkId) ?? throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIssuePcbaReworkTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIssuePcbaReworkTemplateDto>(
            sheetName ?? "质量问题PCBA不良改修费用明细导入模板",
            fileName ?? "质量问题PCBA不良改修费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIssuePcbaReworkAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIssuePcbaReworkImportDto>(fileStream, sheetName ?? "质量问题PCBA不良改修费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIssuePcbaRework>();
                var importDto = rows[i].Adapt<TaktQualityIssuePcbaReworkCreateDto>();
                await StampQualityIssuePcbaReworkQualityIssueAsync(entity, importDto);
                var importKey = $"{entity.QualityIssueId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIssueId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIssuePcbaReworkRepository,
                    x => x.QualityIssueId == entity.QualityIssueId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_issue_pcba_rework_line_unique)
                {
                    throw new TaktBusinessException("质量问题PCBA不良改修费用明细的QualityIssueId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIssuePcbaReworkRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIssuePcbaReworkRepository.CreateAsync(entity);
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
    /// 导出质量问题PCBA不良改修费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIssuePcbaReworkAsync(TaktQualityIssuePcbaReworkQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIssuePcbaReworkQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssuePcbaReworkExportDto>(),
                sheetName ?? "质量问题PCBA不良改修费用明细数据",
                fileName ?? "质量问题PCBA不良改修费用明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIssuePcbaReworkRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssuePcbaReworkExportDto>(),
                sheetName ?? "质量问题PCBA不良改修费用明细数据",
                fileName ?? "质量问题PCBA不良改修费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIssuePcbaReworkExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量问题PCBA不良改修费用明细数据",
            fileName ?? "质量问题PCBA不良改修费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步质量问题PCBA不良改修费用明细主表外键（ManyToOne → 品质问题应对主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIssuePcbaReworkQualityIssueAsync(TaktQualityIssuePcbaRework entity, TaktQualityIssuePcbaReworkCreateDto dto)
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
    /// 构建质量问题PCBA不良改修费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIssuePcbaRework, bool>> QueryExpression(TaktQualityIssuePcbaReworkQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIssuePcbaRework>();

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
                || (x.PcbaDefectParts != null && x.PcbaDefectParts.Contains(keywords))
                || (x.PcbaReworkNote != null && x.PcbaReworkNote.Contains(keywords))
                || (x.PcbaCustomerName1 != null && x.PcbaCustomerName1.Contains(keywords))
                || (x.PcbaDebitNoteCode != null && x.PcbaDebitNoteCode.Contains(keywords))
                || (x.PcbaNote != null && x.PcbaNote.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaDefectParts))
        {
            var pcbaDefectParts = queryDto.PcbaDefectParts;
            exp = exp.And(x => x.PcbaDefectParts != null && x.PcbaDefectParts.Contains(pcbaDefectParts));
        }

        if (queryDto?.PcbaReworkCost.HasValue == true)
        {
            var pcbaReworkCost = queryDto.PcbaReworkCost.Value;
            exp = exp.And(x => x.PcbaReworkCost == pcbaReworkCost);
        }

        if (queryDto?.PcbaReworkTimeMinutes.HasValue == true)
        {
            var pcbaReworkTimeMinutes = queryDto.PcbaReworkTimeMinutes.Value;
            exp = exp.And(x => x.PcbaReworkTimeMinutes == pcbaReworkTimeMinutes);
        }

        if (queryDto?.PcbaReinspectionTimeMinutes.HasValue == true)
        {
            var pcbaReinspectionTimeMinutes = queryDto.PcbaReinspectionTimeMinutes.Value;
            exp = exp.And(x => x.PcbaReinspectionTimeMinutes == pcbaReinspectionTimeMinutes);
        }

        if (queryDto?.PcbaTravelCost.HasValue == true)
        {
            var pcbaTravelCost = queryDto.PcbaTravelCost.Value;
            exp = exp.And(x => x.PcbaTravelCost == pcbaTravelCost);
        }

        if (queryDto?.PcbaWarehouseCost.HasValue == true)
        {
            var pcbaWarehouseCost = queryDto.PcbaWarehouseCost.Value;
            exp = exp.And(x => x.PcbaWarehouseCost == pcbaWarehouseCost);
        }

        if (queryDto?.PcbaOtherExpenses.HasValue == true)
        {
            var pcbaOtherExpenses = queryDto.PcbaOtherExpenses.Value;
            exp = exp.And(x => x.PcbaOtherExpenses == pcbaOtherExpenses);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaReworkNote))
        {
            var pcbaReworkNote = queryDto.PcbaReworkNote;
            exp = exp.And(x => x.PcbaReworkNote != null && x.PcbaReworkNote.Contains(pcbaReworkNote));
        }

        if (queryDto?.PcbaScrapCost.HasValue == true)
        {
            var pcbaScrapCost = queryDto.PcbaScrapCost.Value;
            exp = exp.And(x => x.PcbaScrapCost == pcbaScrapCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaCustomerName1))
        {
            var pcbaCustomerName1 = queryDto.PcbaCustomerName1;
            exp = exp.And(x => x.PcbaCustomerName1 != null && x.PcbaCustomerName1.Contains(pcbaCustomerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaDebitNoteCode))
        {
            var pcbaDebitNoteCode = queryDto.PcbaDebitNoteCode;
            exp = exp.And(x => x.PcbaDebitNoteCode != null && x.PcbaDebitNoteCode.Contains(pcbaDebitNoteCode));
        }

        if (queryDto?.PcbaOtherExpenses2.HasValue == true)
        {
            var pcbaOtherExpenses2 = queryDto.PcbaOtherExpenses2.Value;
            exp = exp.And(x => x.PcbaOtherExpenses2 == pcbaOtherExpenses2);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaNote))
        {
            var pcbaNote = queryDto.PcbaNote;
            exp = exp.And(x => x.PcbaNote != null && x.PcbaNote.Contains(pcbaNote));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponseGroupName))
        {
            var pcbaRecorder = queryDto.ResponseGroupName;
            exp = exp.And(x => x.ResponseGroupName != null && x.ResponseGroupName.Contains(pcbaRecorder));
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
    private static bool HasAnyListQueryFilter(TaktQualityIssuePcbaReworkQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaDefectParts))
        {
            return true;
        }
        if (queryDto.PcbaReworkCost.HasValue)
        {
            return true;
        }
        if (queryDto.PcbaReworkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.PcbaReinspectionTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.PcbaTravelCost.HasValue)
        {
            return true;
        }
        if (queryDto.PcbaWarehouseCost.HasValue)
        {
            return true;
        }
        if (queryDto.PcbaOtherExpenses.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaReworkNote))
        {
            return true;
        }
        if (queryDto.PcbaScrapCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaCustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaDebitNoteCode))
        {
            return true;
        }
        if (queryDto.PcbaOtherExpenses2.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaNote))
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

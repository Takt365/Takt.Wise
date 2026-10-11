// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueMeetingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：质量问题会议调查试验费用明细应用服务实现
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
/// 质量问题会议调查试验费用明细应用服务
/// </summary>
public class TaktQualityIssueMeetingService : TaktServiceBase, ITaktQualityIssueMeetingService
{
    private readonly ITaktCompanyRepository<TaktQualityIssueMeeting> _qualityIssueMeetingRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueMeetingRepository">质量问题会议调查试验费用明细仓储</param>
    /// <param name="qualityIssueRepository">品质问题应对主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIssueMeetingService(
        ITaktCompanyRepository<TaktQualityIssueMeeting> qualityIssueMeetingRepository,
        ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIssueMeetingRepository = qualityIssueMeetingRepository;
        _qualityIssueRepository = qualityIssueRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIssueMeetingDto>> GetQualityIssueMeetingListAsync(TaktQualityIssueMeetingQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIssueMeetingDto>.Create(
                new List<TaktQualityIssueMeetingDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIssueMeetingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIssueMeetingDto>.Create(
            data.Adapt<List<TaktQualityIssueMeetingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueMeetingDto?> GetQualityIssueMeetingByIdAsync(long id)
    {
        var entity = await _qualityIssueMeetingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQualityIssueMeetingDto>();
    }

    /// <summary>
    /// 获取质量问题会议调查试验费用明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIssueMeetingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIssueMeetingRepository.GetListAsync(
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
    /// 创建质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueMeetingDto> CreateQualityIssueMeetingAsync(TaktQualityIssueMeetingCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIssueMeeting>();
        entity.IsObsolete = 0;
        await StampQualityIssueMeetingQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueMeetingRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_quality_issue_meeting_line_unique)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityIssueId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _qualityIssueMeetingRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _qualityIssueMeetingRepository.CreateAsync(entity);
        return await GetQualityIssueMeetingByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIssueMeetingDto>();
    }

    /// <summary>
    /// 更新质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueMeetingDto> UpdateQualityIssueMeetingAsync(long id, TaktQualityIssueMeetingUpdateDto dto)
    {
        var entity = await _qualityIssueMeetingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
        }
        dto.Adapt(entity);
        await StampQualityIssueMeetingQualityIssueAsync(entity, dto);
        var isUnique_ix_takt_logistics_quality_issue_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueMeetingRepository,
            x => x.QualityIssueId == entity.QualityIssueId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_quality_issue_meeting_line_unique)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityIssueId、LineNumber已存在");
        }
        await _qualityIssueMeetingRepository.UpdateAsync(entity);
        return await GetQualityIssueMeetingByIdAsync(id) ?? throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
    }

    /// <summary>
    /// 删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="id">质量问题会议调查试验费用明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueMeetingByIdAsync(long id)
    {
        var entity = await _qualityIssueMeetingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细已作废");
        }
        entity.IsObsolete = 1;
        await _qualityIssueMeetingRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueMeetingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIssueMeetingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新质量问题会议调查试验费用明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueMeetingDto> UpdateQualityIssueMeetingObsoleteAsync(TaktQualityIssueMeetingObsoleteDto dto)
    {
        var entity = await _qualityIssueMeetingRepository.GetByIdAsync(dto.QualityIssueMeetingId);
        if (entity == null)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _qualityIssueMeetingRepository.UpdateAsync(entity);
        return await GetQualityIssueMeetingByIdAsync(dto.QualityIssueMeetingId) ?? throw new TaktBusinessException("质量问题会议调查试验费用明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIssueMeetingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIssueMeetingTemplateDto>(
            sheetName ?? "质量问题会议调查试验费用明细导入模板",
            fileName ?? "质量问题会议调查试验费用明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIssueMeetingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIssueMeetingImportDto>(fileStream, sheetName ?? "质量问题会议调查试验费用明细导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIssueMeeting>();
                var importDto = rows[i].Adapt<TaktQualityIssueMeetingCreateDto>();
                await StampQualityIssueMeetingQualityIssueAsync(entity, importDto);
                var importKey = $"{entity.QualityIssueId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（QualityIssueId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_quality_issue_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIssueMeetingRepository,
                    x => x.QualityIssueId == entity.QualityIssueId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_quality_issue_meeting_line_unique)
                {
                    throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityIssueId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _qualityIssueMeetingRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QualityIssueId == entity.QualityIssueId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.QualityIssueId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _qualityIssueMeetingRepository.CreateAsync(entity);
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
    /// 导出质量问题会议调查试验费用明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIssueMeetingAsync(TaktQualityIssueMeetingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIssueMeetingQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueMeetingExportDto>(),
                sheetName ?? "质量问题会议调查试验费用明细数据",
                fileName ?? "质量问题会议调查试验费用明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIssueMeetingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueMeetingExportDto>(),
                sheetName ?? "质量问题会议调查试验费用明细数据",
                fileName ?? "质量问题会议调查试验费用明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIssueMeetingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "质量问题会议调查试验费用明细数据",
            fileName ?? "质量问题会议调查试验费用明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步质量问题会议调查试验费用明细主表外键（ManyToOne → 品质问题应对主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampQualityIssueMeetingQualityIssueAsync(TaktQualityIssueMeeting entity, TaktQualityIssueMeetingCreateDto dto)
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
    /// 构建质量问题会议调查试验费用明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIssueMeeting, bool>> QueryExpression(TaktQualityIssueMeetingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIssueMeeting>();

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
                || (x.MeetingInvestigationContent != null && x.MeetingInvestigationContent.Contains(keywords))
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

        if (queryDto?.DirectManpowerCostPerMinute.HasValue == true)
        {
            var directManpowerCostPerMinute = queryDto.DirectManpowerCostPerMinute.Value;
            exp = exp.And(x => x.DirectManpowerCostPerMinute == directManpowerCostPerMinute);
        }

        if (queryDto?.IndirectManpowerCostPerMinute.HasValue == true)
        {
            var indirectManpowerCostPerMinute = queryDto.IndirectManpowerCostPerMinute.Value;
            exp = exp.And(x => x.IndirectManpowerCostPerMinute == indirectManpowerCostPerMinute);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingInvestigationContent))
        {
            var meetingInvestigationContent = queryDto.MeetingInvestigationContent;
            exp = exp.And(x => x.MeetingInvestigationContent != null && x.MeetingInvestigationContent.Contains(meetingInvestigationContent));
        }

        if (queryDto?.MeetingInvestigationCost.HasValue == true)
        {
            var meetingInvestigationCost = queryDto.MeetingInvestigationCost.Value;
            exp = exp.And(x => x.MeetingInvestigationCost == meetingInvestigationCost);
        }

        if (queryDto?.MeetingTimeMinutes.HasValue == true)
        {
            var meetingTimeMinutes = queryDto.MeetingTimeMinutes.Value;
            exp = exp.And(x => x.MeetingTimeMinutes == meetingTimeMinutes);
        }

        if (queryDto?.DirectParticipantCount.HasValue == true)
        {
            var directParticipantCount = queryDto.DirectParticipantCount.Value;
            exp = exp.And(x => x.DirectParticipantCount == directParticipantCount);
        }

        if (queryDto?.IndirectParticipantCount.HasValue == true)
        {
            var indirectParticipantCount = queryDto.IndirectParticipantCount.Value;
            exp = exp.And(x => x.IndirectParticipantCount == indirectParticipantCount);
        }

        if (queryDto?.InvestigationWorkTimeMinutes.HasValue == true)
        {
            var investigationWorkTimeMinutes = queryDto.InvestigationWorkTimeMinutes.Value;
            exp = exp.And(x => x.InvestigationWorkTimeMinutes == investigationWorkTimeMinutes);
        }

        if (queryDto?.TravelCost.HasValue == true)
        {
            var travelCost = queryDto.TravelCost.Value;
            exp = exp.And(x => x.TravelCost == travelCost);
        }

        if (queryDto?.OtherExpenses.HasValue == true)
        {
            var otherExpenses = queryDto.OtherExpenses.Value;
            exp = exp.And(x => x.OtherExpenses == otherExpenses);
        }

        if (queryDto?.OtherWorkTimeMinutes.HasValue == true)
        {
            var otherWorkTimeMinutes = queryDto.OtherWorkTimeMinutes.Value;
            exp = exp.And(x => x.OtherWorkTimeMinutes == otherWorkTimeMinutes);
        }

        if (queryDto?.OtherApparatusCost.HasValue == true)
        {
            var otherApparatusCost = queryDto.OtherApparatusCost.Value;
            exp = exp.And(x => x.OtherApparatusCost == otherApparatusCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ResponseGroupName))
        {
            var meetingRecorder = queryDto.ResponseGroupName;
            exp = exp.And(x => x.ResponseGroupName != null && x.ResponseGroupName.Contains(meetingRecorder));
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
    private static bool HasAnyListQueryFilter(TaktQualityIssueMeetingQueryDto? queryDto)
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
        if (queryDto.DirectManpowerCostPerMinute.HasValue)
        {
            return true;
        }
        if (queryDto.IndirectManpowerCostPerMinute.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingInvestigationContent))
        {
            return true;
        }
        if (queryDto.MeetingInvestigationCost.HasValue)
        {
            return true;
        }
        if (queryDto.MeetingTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.DirectParticipantCount.HasValue)
        {
            return true;
        }
        if (queryDto.IndirectParticipantCount.HasValue)
        {
            return true;
        }
        if (queryDto.InvestigationWorkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.TravelCost.HasValue)
        {
            return true;
        }
        if (queryDto.OtherExpenses.HasValue)
        {
            return true;
        }
        if (queryDto.OtherWorkTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.OtherApparatusCost.HasValue)
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

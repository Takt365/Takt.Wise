// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Quality.Cost
// 文件名称：TaktQualityIssueService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：品质问题应对主应用服务实现
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
/// 品质问题应对主应用服务
/// </summary>
public class TaktQualityIssueService : TaktServiceBase, ITaktQualityIssueService
{
    private readonly ITaktCompanyRepository<TaktQualityIssue> _qualityIssueRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssueMeeting> _qualityIssueMeetingRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssueAssyRework> _qualityIssueAssyReworkRepository;
    private readonly ITaktCompanyRepository<TaktQualityIssuePcbaRework> _qualityIssuePcbaReworkRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityIssueRepository">品质问题应对主仓储</param>
    /// <param name="qualityIssueMeetingRepository">QualityIssueMeeting仓储</param>
    /// <param name="qualityIssueAssyReworkRepository">QualityIssueAssyRework仓储</param>
    /// <param name="qualityIssuePcbaReworkRepository">QualityIssuePcbaRework仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQualityIssueService(
        ITaktCompanyRepository<TaktQualityIssue> qualityIssueRepository,
        ITaktCompanyRepository<TaktQualityIssueMeeting> qualityIssueMeetingRepository,
        ITaktCompanyRepository<TaktQualityIssueAssyRework> qualityIssueAssyReworkRepository,
        ITaktCompanyRepository<TaktQualityIssuePcbaRework> qualityIssuePcbaReworkRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _qualityIssueRepository = qualityIssueRepository;
        _qualityIssueMeetingRepository = qualityIssueMeetingRepository;
        _qualityIssueAssyReworkRepository = qualityIssueAssyReworkRepository;
        _qualityIssuePcbaReworkRepository = qualityIssuePcbaReworkRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取品质问题应对主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQualityIssueDto>> GetQualityIssueListAsync(TaktQualityIssueQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQualityIssueDto>.Create(
                new List<TaktQualityIssueDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _qualityIssueRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQualityIssueDto>.Create(
            data.Adapt<List<TaktQualityIssueDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueDto?> GetQualityIssueByIdAsync(long id)
    {
        var entity = await _qualityIssueRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktQualityIssueDto>();
        await FillQualityIssueDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取品质问题应对主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQualityIssueOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _qualityIssueRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.QualityIssueCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.QualityIssueCode,
            DictLabel = e.QualityIssueCode,
        }).ToList();
    }

    /// <summary>
    /// 创建品质问题应对主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueDto> CreateQualityIssueAsync(TaktQualityIssueCreateDto dto)
    {
        var entity = dto.Adapt<TaktQualityIssue>();
        var isUnique_ix_takt_logistics_quality_issue_qf_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIssueCode == entity.QualityIssueCode
                && x.IssueDate == entity.IssueDate);
        if (!isUnique_ix_takt_logistics_quality_issue_qf_unique)
        {
            throw new TaktBusinessException("品质问题应对主的PlantCode、QualityIssueCode、IssueDate已存在");
        }
        entity = await _qualityIssueRepository.CreateAsync(entity);
                await SaveQualityIssueChildrenAsync(entity, dto);
        return await GetQualityIssueByIdAsync(entity.Id) ?? entity.Adapt<TaktQualityIssueDto>();
    }

    /// <summary>
    /// 更新品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQualityIssueDto> UpdateQualityIssueAsync(long id, TaktQualityIssueUpdateDto dto)
    {
        var entity = await _qualityIssueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_quality_issue_qf_unique = await _uniqueValidator.IsUniqueAsync(
            _qualityIssueRepository,
            x => x.PlantCode == entity.PlantCode
                && x.QualityIssueCode == entity.QualityIssueCode
                && x.IssueDate == entity.IssueDate,
            id);
        if (!isUnique_ix_takt_logistics_quality_issue_qf_unique)
        {
            throw new TaktBusinessException("品质问题应对主的PlantCode、QualityIssueCode、IssueDate已存在");
        }
        await _qualityIssueRepository.UpdateAsync(entity);
                await SaveQualityIssueChildrenAsync(entity, dto);
        return await GetQualityIssueByIdAsync(id) ?? throw new TaktBusinessException("品质问题应对主不存在");
    }

    /// <summary>
    /// 删除品质问题应对主
    /// </summary>
    /// <param name="id">品质问题应对主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueByIdAsync(long id)
    {
        var entity = await _qualityIssueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("品质问题应对主不存在或已删除");
        }
        await _qualityIssueMeetingRepository.DeleteAsync(x => x.QualityIssueId == entity.Id);
        await _qualityIssueAssyReworkRepository.DeleteAsync(x => x.QualityIssueId == entity.Id);
        await _qualityIssuePcbaReworkRepository.DeleteAsync(x => x.QualityIssueId == entity.Id);
        var deleted = await _qualityIssueRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("品质问题应对主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除品质问题应对主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQualityIssueBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQualityIssueByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQualityIssueTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQualityIssueTemplateDto>(
            sheetName ?? "品质问题应对主导入模板",
            fileName ?? "品质问题应对主导入模板.xlsx");
    }

    /// <summary>
    /// 导入品质问题应对主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQualityIssueAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQualityIssueImportDto>(fileStream, sheetName ?? "品质问题应对主导入模板");
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
                var entity = rows[i].Adapt<TaktQualityIssue>();
                var importKey = $"{entity.PlantCode}|{entity.QualityIssueCode}|{entity.IssueDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、QualityIssueCode、IssueDate）");
                }
                var isUnique_ix_takt_logistics_quality_issue_qf_unique = await _uniqueValidator.IsUniqueAsync(
                    _qualityIssueRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.QualityIssueCode == entity.QualityIssueCode
                        && x.IssueDate == entity.IssueDate);
                if (!isUnique_ix_takt_logistics_quality_issue_qf_unique)
                {
                    throw new TaktBusinessException("品质问题应对主的PlantCode、QualityIssueCode、IssueDate已存在");
                }
                await _qualityIssueRepository.CreateAsync(entity);
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
    /// 导出品质问题应对主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQualityIssueAsync(TaktQualityIssueQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQualityIssueQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueExportDto>(),
                sheetName ?? "品质问题应对主数据",
                fileName ?? "品质问题应对主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _qualityIssueRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQualityIssueExportDto>(),
                sheetName ?? "品质问题应对主数据",
                fileName ?? "品质问题应对主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQualityIssueExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "品质问题应对主数据",
            fileName ?? "品质问题应对主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废质量问题会议调查试验费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityIssueId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityIssueMeetingsObsoleteAsync(long qualityIssueId)
    {
        if (qualityIssueId <= 0)
        {
            return;
        }
        var rows = await _qualityIssueMeetingRepository.GetListAsync(
            x => x.QualityIssueId == qualityIssueId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityIssueMeetingRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废质量问题组装不良改修费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityIssueId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityIssueAssyReworksObsoleteAsync(long qualityIssueId)
    {
        if (qualityIssueId <= 0)
        {
            return;
        }
        var rows = await _qualityIssueAssyReworkRepository.GetListAsync(
            x => x.QualityIssueId == qualityIssueId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityIssueAssyReworkRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废质量问题PCBA不良改修费用明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="qualityIssueId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkQualityIssuePcbaReworksObsoleteAsync(long qualityIssueId)
    {
        if (qualityIssueId <= 0)
        {
            return;
        }
        var rows = await _qualityIssuePcbaReworkRepository.GetListAsync(
            x => x.QualityIssueId == qualityIssueId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _qualityIssuePcbaReworkRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充品质问题应对主详情（加载 OneToMany 子表：质量问题会议调查试验费用明细、质量问题组装不良改修费用明细、质量问题PCBA不良改修费用明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillQualityIssueDetailsAsync(TaktQualityIssueDto dto, TaktQualityIssue entity)
    {
        if (dto == null)
        {
            return;
        }
        // 质量问题会议调查试验费用明细 → dto.MeetingItems（含作废行）
        var meetingitems = await _qualityIssueMeetingRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
        dto.MeetingItems = meetingitems.Adapt<List<TaktQualityIssueMeetingDto>>();
        // 质量问题组装不良改修费用明细 → dto.AssyReworkItems（含作废行）
        var assyreworkitems = await _qualityIssueAssyReworkRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
        dto.AssyReworkItems = assyreworkitems.Adapt<List<TaktQualityIssueAssyReworkDto>>();
        // 质量问题PCBA不良改修费用明细 → dto.PcbaReworkItems（含作废行）
        var pcbareworkitems = await _qualityIssuePcbaReworkRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
        dto.PcbaReworkItems = pcbareworkitems.Adapt<List<TaktQualityIssuePcbaReworkDto>>();
    }

    /// <summary>
    /// 保存品质问题应对主子表级联（质量问题会议调查试验费用明细、质量问题组装不良改修费用明细、质量问题PCBA不良改修费用明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveQualityIssueChildrenAsync(TaktQualityIssue entity, TaktQualityIssueCreateDto dto)
    {
        // 质量问题会议调查试验费用明细（MeetingItems）
        List<TaktQualityIssueMeetingUpdateDto>? meetingItemsForSave;
        if (dto is TaktQualityIssueUpdateDto updateDtoForMeetingItems && updateDtoForMeetingItems.MeetingItems != null)
        {
            meetingItemsForSave = updateDtoForMeetingItems.MeetingItems;
        }
        else if (dto.MeetingItems != null)
        {
            meetingItemsForSave = dto.MeetingItems.Adapt<List<TaktQualityIssueMeetingUpdateDto>>();
        }
        else
        {
            meetingItemsForSave = null;
        }
        if (meetingItemsForSave is not { Count: > 0 })
        {
            await MarkQualityIssueMeetingsObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityIssueMeetingRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityIssueMeeting>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < meetingItemsForSave.Count; i++)
            {
                var childDto = meetingItemsForSave[i];
                childDto.QualityIssueId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityIssueCode = entity.QualityIssueCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("质量问题会议调查试验费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityIssueId、LineNumber）");
                }
                if (childDto.QualityIssueMeetingId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityIssueMeetingId, out var target))
                    {
                        throw new TaktBusinessException("质量问题会议调查试验费用明细不存在（QualityIssueMeetingId={childDto.QualityIssueMeetingId}）");
                    }
                    if (target.QualityIssueId != entity.Id)
                    {
                        throw new TaktBusinessException("质量问题会议调查试验费用明细不属于当前主表（QualityIssueMeetingId={childDto.QualityIssueMeetingId}）");
                    }
                    submittedIds.Add(childDto.QualityIssueMeetingId);
                    var isUniqueUpdate_ix_takt_logistics_quality_issue_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssueMeetingRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityIssueMeetingId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_issue_meeting_line_unique)
                    {
                        throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityIssueId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityIssueMeetingId;
                    target.QualityIssueId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityIssueMeetingRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_issue_meeting_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssueMeetingRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_issue_meeting_line_unique)
                    {
                        throw new TaktBusinessException("质量问题会议调查试验费用明细的QualityIssueId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityIssueMeeting>();
                    child.Id = 0;
                    child.QualityIssueId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityIssueMeetingRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _qualityIssueMeetingRepository.CreateRangeAsync(toCreate);
            }
        }
        // 质量问题组装不良改修费用明细（AssyReworkItems）
        List<TaktQualityIssueAssyReworkUpdateDto>? assyReworkItemsForSave;
        if (dto is TaktQualityIssueUpdateDto updateDtoForAssyReworkItems && updateDtoForAssyReworkItems.AssyReworkItems != null)
        {
            assyReworkItemsForSave = updateDtoForAssyReworkItems.AssyReworkItems;
        }
        else if (dto.AssyReworkItems != null)
        {
            assyReworkItemsForSave = dto.AssyReworkItems.Adapt<List<TaktQualityIssueAssyReworkUpdateDto>>();
        }
        else
        {
            assyReworkItemsForSave = null;
        }
        if (assyReworkItemsForSave is not { Count: > 0 })
        {
            await MarkQualityIssueAssyReworksObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityIssueAssyReworkRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityIssueAssyRework>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < assyReworkItemsForSave.Count; i++)
            {
                var childDto = assyReworkItemsForSave[i];
                childDto.QualityIssueId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityIssueCode = entity.QualityIssueCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("质量问题组装不良改修费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityIssueId、LineNumber）");
                }
                if (childDto.QualityIssueAssyReworkId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityIssueAssyReworkId, out var target))
                    {
                        throw new TaktBusinessException("质量问题组装不良改修费用明细不存在（QualityIssueAssyReworkId={childDto.QualityIssueAssyReworkId}）");
                    }
                    if (target.QualityIssueId != entity.Id)
                    {
                        throw new TaktBusinessException("质量问题组装不良改修费用明细不属于当前主表（QualityIssueAssyReworkId={childDto.QualityIssueAssyReworkId}）");
                    }
                    submittedIds.Add(childDto.QualityIssueAssyReworkId);
                    var isUniqueUpdate_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssueAssyReworkRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityIssueAssyReworkId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_issue_assy_rework_line_unique)
                    {
                        throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityIssueAssyReworkId;
                    target.QualityIssueId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityIssueAssyReworkRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_issue_assy_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssueAssyReworkRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_issue_assy_rework_line_unique)
                    {
                        throw new TaktBusinessException("质量问题组装不良改修费用明细的QualityIssueId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityIssueAssyRework>();
                    child.Id = 0;
                    child.QualityIssueId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityIssueAssyReworkRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _qualityIssueAssyReworkRepository.CreateRangeAsync(toCreate);
            }
        }
        // 质量问题PCBA不良改修费用明细（PcbaReworkItems）
        List<TaktQualityIssuePcbaReworkUpdateDto>? pcbaReworkItemsForSave;
        if (dto is TaktQualityIssueUpdateDto updateDtoForPcbaReworkItems && updateDtoForPcbaReworkItems.PcbaReworkItems != null)
        {
            pcbaReworkItemsForSave = updateDtoForPcbaReworkItems.PcbaReworkItems;
        }
        else if (dto.PcbaReworkItems != null)
        {
            pcbaReworkItemsForSave = dto.PcbaReworkItems.Adapt<List<TaktQualityIssuePcbaReworkUpdateDto>>();
        }
        else
        {
            pcbaReworkItemsForSave = null;
        }
        if (pcbaReworkItemsForSave is not { Count: > 0 })
        {
            await MarkQualityIssuePcbaReworksObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _qualityIssuePcbaReworkRepository.GetListAsync(x => x.QualityIssueId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktQualityIssuePcbaRework>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < pcbaReworkItemsForSave.Count; i++)
            {
                var childDto = pcbaReworkItemsForSave[i];
                childDto.QualityIssueId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.QualityIssueCode = entity.QualityIssueCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("质量问题PCBA不良改修费用明细第{i + 1}项与本次提交的其他项重复（CompanyCode、QualityIssueId、LineNumber）");
                }
                if (childDto.QualityIssuePcbaReworkId > 0)
                {
                    if (!existingById.TryGetValue(childDto.QualityIssuePcbaReworkId, out var target))
                    {
                        throw new TaktBusinessException("质量问题PCBA不良改修费用明细不存在（QualityIssuePcbaReworkId={childDto.QualityIssuePcbaReworkId}）");
                    }
                    if (target.QualityIssueId != entity.Id)
                    {
                        throw new TaktBusinessException("质量问题PCBA不良改修费用明细不属于当前主表（QualityIssuePcbaReworkId={childDto.QualityIssuePcbaReworkId}）");
                    }
                    submittedIds.Add(childDto.QualityIssuePcbaReworkId);
                    var isUniqueUpdate_ix_takt_logistics_quality_issue_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssuePcbaReworkRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber,
                        childDto.QualityIssuePcbaReworkId);
                    if (!isUniqueUpdate_ix_takt_logistics_quality_issue_pcba_rework_line_unique)
                    {
                        throw new TaktBusinessException("质量问题PCBA不良改修费用明细的QualityIssueId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.QualityIssuePcbaReworkId;
                    target.QualityIssueId = entity.Id;
                    target.IsObsolete = 0;
                    await _qualityIssuePcbaReworkRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_quality_issue_pcba_rework_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _qualityIssuePcbaReworkRepository,
                        x => x.QualityIssueId == x.QualityIssueId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_quality_issue_pcba_rework_line_unique)
                    {
                        throw new TaktBusinessException("质量问题PCBA不良改修费用明细的QualityIssueId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktQualityIssuePcbaRework>();
                    child.Id = 0;
                    child.QualityIssueId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _qualityIssuePcbaReworkRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.QualityIssueCode) ? entity.QualityIssueCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _qualityIssuePcbaReworkRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建品质问题应对主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQualityIssue, bool>> QueryExpression(TaktQualityIssueQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQualityIssue>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.QualityIssueCode != null && x.QualityIssueCode.Contains(keywords))
                || (x.Model != null && x.Model.Contains(keywords))
                || (x.Lot != null && x.Lot.Contains(keywords))
                || (x.QualityProblemsResponse != null && x.QualityProblemsResponse.Contains(keywords))
                || (x.ReworkDueToDefects != null && x.ReworkDueToDefects.Contains(keywords))
                || (x.NeedRework != null && x.NeedRework.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityIssueCode))
        {
            var qualityIssueCode = queryDto.QualityIssueCode;
            exp = exp.And(x => x.QualityIssueCode != null && x.QualityIssueCode.Contains(qualityIssueCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Model))
        {
            var model = queryDto.Model;
            exp = exp.And(x => x.Model != null && x.Model.Contains(model));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Lot))
        {
            var lot = queryDto.Lot;
            exp = exp.And(x => x.Lot != null && x.Lot.Contains(lot));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityProblemsResponse))
        {
            var qualityProblemsResponse = queryDto.QualityProblemsResponse;
            exp = exp.And(x => x.QualityProblemsResponse != null && x.QualityProblemsResponse.Contains(qualityProblemsResponse));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReworkDueToDefects))
        {
            var reworkDueToDefects = queryDto.ReworkDueToDefects;
            exp = exp.And(x => x.ReworkDueToDefects != null && x.ReworkDueToDefects.Contains(reworkDueToDefects));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NeedRework))
        {
            var needRework = queryDto.NeedRework;
            exp = exp.And(x => x.NeedRework != null && x.NeedRework.Contains(needRework));
        }

        if (queryDto?.TotalTimeMinutes.HasValue == true)
        {
            var totalTimeMinutes = queryDto.TotalTimeMinutes.Value;
            exp = exp.And(x => x.TotalTimeMinutes == totalTimeMinutes);
        }

        if (queryDto?.TotalCost.HasValue == true)
        {
            var totalCost = queryDto.TotalCost.Value;
            exp = exp.And(x => x.TotalCost == totalCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
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

        if (queryDto?.IssueDateStart.HasValue == true)
        {
            var issueDateStart = queryDto.IssueDateStart.Value;
            exp = exp.And(x => x.IssueDate >= issueDateStart);
        }

        if (queryDto?.IssueDateEnd.HasValue == true)
        {
            var issueDateEnd = queryDto.IssueDateEnd.Value;
            exp = exp.And(x => x.IssueDate <= issueDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktQualityIssueQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.QualityIssueCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Model))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Lot))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityProblemsResponse))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReworkDueToDefects))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NeedRework))
        {
            return true;
        }
        if (queryDto.TotalTimeMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.TotalCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
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
        if (queryDto.IssueDateStart.HasValue || queryDto.IssueDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取品质应对金额统计（数据看板；按 IssueDate 汇总 TotalCost）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>品质应对金额统计</returns>
    public async Task<TaktQualityCostStatDto> GetQualityIssueCostStatAsync(TaktQualityCostStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.DateStart,
            queryDto.DateEnd,
            queryDto.StatMonth);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktQualityIssue, bool>> predicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IssueDate >= start
            && x.IssueDate <= end;
        var monthRowCount = await _qualityIssueRepository.CountAsync(predicate);
        var monthTotalAmount = await _qualityIssueRepository.SumAsync(x => x.TotalCost, predicate);
        return new TaktQualityCostStatDto
        {
            StatMonth = statMonth,
            MonthTotalAmount = monthTotalAmount,
            MonthRowCount = monthRowCount,
        };
    }
}

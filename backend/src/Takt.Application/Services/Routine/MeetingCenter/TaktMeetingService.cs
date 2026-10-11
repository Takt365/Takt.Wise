// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会议中心应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Domain.Entities.Routine.MeetingCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会议中心应用服务
/// </summary>
public class TaktMeetingService : TaktServiceBase, ITaktMeetingService
{
    private readonly ITaktApprovalRepository<TaktMeeting> _meetingRepository;
    private readonly ITaktCompanyRepository<TaktMeetingAttendee> _meetingAttendeeRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;
    private readonly ITaktMeetingNotificationDispatchService _meetingNotificationDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingRepository">会议中心仓储</param>
    /// <param name="meetingAttendeeRepository">MeetingAttendee仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">编码生成器</param>
    /// <param name="meetingNotificationDispatchService">会议通知派发服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingService(
        ITaktApprovalRepository<TaktMeeting> meetingRepository,
        ITaktCompanyRepository<TaktMeetingAttendee> meetingAttendeeRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktMeetingNotificationDispatchService meetingNotificationDispatchService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingRepository = meetingRepository;
        _meetingAttendeeRepository = meetingAttendeeRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
        _meetingNotificationDispatchService = meetingNotificationDispatchService;
    }

    /// <summary>
    /// 获取会议中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMeetingDto>> GetMeetingListAsync(TaktMeetingQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMeetingDto>.Create(
                new List<TaktMeetingDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _meetingRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMeetingDto>.Create(
            data.Adapt<List<TaktMeetingDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingDto?> GetMeetingByIdAsync(long id)
    {
        var entity = await _meetingRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktMeetingDto>();
        await FillMeetingDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取会议中心选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMeetingOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _meetingRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MeetingStatus == 1,
            x => x.OrganizerName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MeetingCode,
            DictLabel = e.OrganizerName ?? e.MeetingCode,
        }).ToList();
    }

    /// <summary>
    /// 创建会议中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingDto> CreateMeetingAsync(TaktMeetingCreateDto dto)
    {
        var entity = dto.Adapt<TaktMeeting>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.MeetingCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.MeetingCode))
        {
            throw new TaktBusinessException("会议编码不能为空");
        }
        var isUnique_ix_meeting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingRepository,
            x => x.MeetingCode == entity.MeetingCode);
        if (!isUnique_ix_meeting_code_unique)
        {
            throw new TaktBusinessException("会议中心的MeetingCode已存在");
        }
        entity = await _meetingRepository.CreateAsync(entity);
                await SaveMeetingChildrenAsync(entity, dto);
        await TryNotifyMeetingAttendeesAsync(entity.Id, entity.MeetingStatus, TaktMeetingNotificationKind.Invitation);
        return await GetMeetingByIdAsync(entity.Id) ?? entity.Adapt<TaktMeetingDto>();
    }

    /// <summary>
    /// 更新会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingDto> UpdateMeetingAsync(long id, TaktMeetingUpdateDto dto)
    {
        var entity = await _meetingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_meeting_code_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingRepository,
            x => x.MeetingCode == entity.MeetingCode,
            id);
        if (!isUnique_ix_meeting_code_unique)
        {
            throw new TaktBusinessException("会议中心的MeetingCode已存在");
        }
        await _meetingRepository.UpdateAsync(entity);
                await SaveMeetingChildrenAsync(entity, dto);
        await TryNotifyMeetingAttendeesAsync(entity.Id, entity.MeetingStatus, TaktMeetingNotificationKind.Update);
        return await GetMeetingByIdAsync(id) ?? throw new TaktBusinessException("会议中心不存在");
    }

    /// <summary>
    /// 删除会议中心
    /// </summary>
    /// <param name="id">会议中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingByIdAsync(long id)
    {
        var entity = await _meetingRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在或已删除");
        }
        await _meetingAttendeeRepository.DeleteAsync(x => x.MeetingId == entity.Id);
        var deleted = await _meetingRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会议中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会议中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMeetingByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingDto> UpdateMeetingStatusAsync(TaktMeetingStatusDto dto)
    {
        var entity = await _meetingRepository.GetByIdAsync(dto.MeetingId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        var previousStatus = entity.MeetingStatus;
        entity.MeetingStatus = dto.MeetingStatus;
        await _meetingRepository.UpdateAsync(entity);
        var kind = ResolveNotificationKind(previousStatus, dto.MeetingStatus);
        if (kind.HasValue)
        {
            await TryNotifyMeetingAttendeesAsync(entity.Id, dto.MeetingStatus, kind.Value);
        }
        return await GetMeetingByIdAsync(dto.MeetingId) ?? throw new TaktBusinessException("会议中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMeetingTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMeetingTemplateDto>(
            sheetName ?? "会议中心导入模板",
            fileName ?? "会议中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMeetingAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMeetingImportDto>(fileStream, sheetName ?? "会议中心导入模板");
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
                var entity = rows[i].Adapt<TaktMeeting>();
                var importKey = $"{entity.MeetingCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MeetingCode）");
                }
                var isUnique_ix_meeting_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _meetingRepository,
                    x => x.MeetingCode == entity.MeetingCode);
                if (!isUnique_ix_meeting_code_unique)
                {
                    throw new TaktBusinessException("会议中心的MeetingCode已存在");
                }
                await _meetingRepository.CreateAsync(entity);
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
    /// 导出会议中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMeetingAsync(TaktMeetingQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMeetingQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingExportDto>(),
                sheetName ?? "会议中心数据",
                fileName ?? "会议中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _meetingRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingExportDto>(),
                sheetName ?? "会议中心数据",
                fileName ?? "会议中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMeetingExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议中心数据",
            fileName ?? "会议中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充会议中心详情（加载 OneToMany 子表：参会人员）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillMeetingDetailsAsync(TaktMeetingDto dto, TaktMeeting entity)
    {
        if (dto == null)
        {
            return;
        }
        // 参会人员 → dto.Attendees
        var attendees = await _meetingAttendeeRepository.GetListAsync(x => x.MeetingId == entity.Id);
        dto.Attendees = attendees.Adapt<List<TaktMeetingAttendeeDto>>();
    }

    /// <summary>
    /// 保存会议中心子表级联（参会人员；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveMeetingChildrenAsync(TaktMeeting entity, TaktMeetingCreateDto dto)
    {
        // 会议出席人（Attendees）
        List<TaktMeetingAttendeeUpdateDto>? attendeesForSave;
        if (dto is TaktMeetingUpdateDto updateDtoForAttendees && updateDtoForAttendees.Attendees != null)
        {
            attendeesForSave = updateDtoForAttendees.Attendees;
        }
        else if (dto.Attendees != null)
        {
            attendeesForSave = dto.Attendees.Adapt<List<TaktMeetingAttendeeUpdateDto>>();
        }
        else
        {
            attendeesForSave = null;
        }
        if (attendeesForSave is not { Count: > 0 })
        {
            await MarkMeetingAttendeesObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _meetingAttendeeRepository.GetListAsync(x => x.MeetingId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktMeetingAttendee>();
            for (var i = 0; i < attendeesForSave.Count; i++)
            {
                var childDto = attendeesForSave[i];
                childDto.MeetingId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.MeetingAttendeeId > 0)
                {
                    if (!existingById.TryGetValue(childDto.MeetingAttendeeId, out var target))
                    {
                        throw new TaktBusinessException("参会人员不存在（MeetingAttendeeId={childDto.MeetingAttendeeId}）");
                    }
                    if (target.MeetingId != entity.Id)
                    {
                        throw new TaktBusinessException("参会人员不属于当前主表（MeetingAttendeeId={childDto.MeetingAttendeeId}）");
                    }
                    submittedIds.Add(childDto.MeetingAttendeeId);
                    childDto.Adapt(target);
                    target.Id = childDto.MeetingAttendeeId;
                    target.MeetingId = entity.Id;
                    target.MeetingTitle = entity.MeetingTitle;
                    target.IsObsolete = 0;
                    await _meetingAttendeeRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktMeetingAttendee>();
                    child.Id = 0;
                    child.MeetingId = entity.Id;
                    child.MeetingTitle = entity.MeetingTitle;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _meetingAttendeeRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.MeetingCode) ? entity.MeetingCode : entity.Id.ToString();
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
                await _meetingAttendeeRepository.CreateRangeAsync(toCreate);
            }
        }
    }

    /// <summary>
    /// 将参会人员子表未作废行全部标记作废
    /// </summary>
    /// <param name="meetingId">会议 ID</param>
    /// <returns>任务</returns>
    private async Task MarkMeetingAttendeesObsoleteAsync(long meetingId)
    {
        if (meetingId <= 0)
        {
            return;
        }
        var rows = await _meetingAttendeeRepository.GetListAsync(
            x => x.MeetingId == meetingId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _meetingAttendeeRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 向参会人发送会议邮件通知（失败仅记日志，不影响主业务提交）
    /// </summary>
    /// <param name="meetingId">会议 ID</param>
    /// <param name="meetingStatus">当前会议状态</param>
    /// <param name="kind">通知类型</param>
    /// <returns>任务</returns>
    private async Task TryNotifyMeetingAttendeesAsync(
        long meetingId,
        int meetingStatus,
        TaktMeetingNotificationKind kind)
    {
        try
        {
            await _meetingNotificationDispatchService.NotifyMeetingAttendeesAsync(meetingId, kind);
        }
        catch (Exception ex)
        {
            LogWarning(
                $"会议邮件通知失败: MeetingId={meetingId}, Status={meetingStatus}, Kind={kind}, Error={ex.Message}");
        }
    }

    /// <summary>
    /// 根据状态变更解析通知类型
    /// </summary>
    /// <param name="previousStatus">变更前状态</param>
    /// <param name="newStatus">变更后状态</param>
    /// <returns>通知类型；无需通知时返回 null</returns>
    private static TaktMeetingNotificationKind? ResolveNotificationKind(int previousStatus, int newStatus)
    {
        if (newStatus == TaktMeetingConstants.StatusCancelled)
        {
            return TaktMeetingNotificationKind.Cancellation;
        }
        if (newStatus == TaktMeetingConstants.StatusScheduled || newStatus == TaktMeetingConstants.StatusInProgress)
        {
            return previousStatus == TaktMeetingConstants.StatusDraft
                ? TaktMeetingNotificationKind.Invitation
                : TaktMeetingNotificationKind.Update;
        }
        return null;
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMeeting, bool>> QueryExpression(TaktMeetingQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMeeting>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MeetingCode != null && x.MeetingCode.Contains(keywords))
                || (x.MeetingTitle != null && x.MeetingTitle.Contains(keywords))
                || (x.Location != null && x.Location.Contains(keywords))
                || (x.MeetingLink != null && x.MeetingLink.Contains(keywords))
                || (x.MeetingAgenda != null && x.MeetingAgenda.Contains(keywords))
                || (x.MeetingTags != null && x.MeetingTags.Contains(keywords))
                || (x.OrganizerName != null && x.OrganizerName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || (x.MeetingRoomName != null && x.MeetingRoomName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingCode))
        {
            var meetingCode = queryDto.MeetingCode;
            exp = exp.And(x => x.MeetingCode != null && x.MeetingCode.Contains(meetingCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingTitle))
        {
            var meetingTitle = queryDto.MeetingTitle;
            exp = exp.And(x => x.MeetingTitle != null && x.MeetingTitle.Contains(meetingTitle));
        }

        if (queryDto?.MeetingType.HasValue == true)
        {
            var meetingType = queryDto.MeetingType.Value;
            exp = exp.And(x => x.MeetingType == meetingType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Location))
        {
            var location = queryDto.Location;
            exp = exp.And(x => x.Location != null && x.Location.Contains(location));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingLink))
        {
            var meetingLink = queryDto.MeetingLink;
            exp = exp.And(x => x.MeetingLink != null && x.MeetingLink.Contains(meetingLink));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingAgenda))
        {
            var meetingAgenda = queryDto.MeetingAgenda;
            exp = exp.And(x => x.MeetingAgenda != null && x.MeetingAgenda.Contains(meetingAgenda));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingTags))
        {
            var meetingTags = queryDto.MeetingTags;
            exp = exp.And(x => x.MeetingTags != null && x.MeetingTags.Contains(meetingTags));
        }

        if (queryDto?.OrganizerId.HasValue == true)
        {
            var organizerId = queryDto.OrganizerId.Value;
            exp = exp.And(x => x.OrganizerId == organizerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrganizerName))
        {
            var organizerName = queryDto.OrganizerName;
            exp = exp.And(x => x.OrganizerName != null && x.OrganizerName.Contains(organizerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.MaxAttendees.HasValue == true)
        {
            var maxAttendees = queryDto.MaxAttendees.Value;
            exp = exp.And(x => x.MaxAttendees == maxAttendees);
        }

        if (queryDto?.ReminderMinutes.HasValue == true)
        {
            var reminderMinutes = queryDto.ReminderMinutes.Value;
            exp = exp.And(x => x.ReminderMinutes == reminderMinutes);
        }

        if (queryDto?.MeetingRoomId.HasValue == true)
        {
            var meetingRoomId = queryDto.MeetingRoomId.Value;
            exp = exp.And(x => x.MeetingRoomId == meetingRoomId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingRoomName))
        {
            var meetingRoomName = queryDto.MeetingRoomName;
            exp = exp.And(x => x.MeetingRoomName != null && x.MeetingRoomName.Contains(meetingRoomName));
        }

        if (queryDto?.MeetingStatus.HasValue == true)
        {
            var meetingStatus = queryDto.MeetingStatus.Value;
            exp = exp.And(x => x.MeetingStatus == meetingStatus);
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

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            var startTimeStart = queryDto.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime >= startTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            var startTimeEnd = queryDto.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime <= startTimeEnd);
        }

        if (queryDto?.EndTimeStart.HasValue == true)
        {
            var endTimeStart = queryDto.EndTimeStart.Value;
            exp = exp.And(x => x.EndTime >= endTimeStart);
        }

        if (queryDto?.EndTimeEnd.HasValue == true)
        {
            var endTimeEnd = queryDto.EndTimeEnd.Value;
            exp = exp.And(x => x.EndTime <= endTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktMeetingQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingTitle))
        {
            return true;
        }
        if (queryDto.MeetingType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Location))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingLink))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingAgenda))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingTags))
        {
            return true;
        }
        if (queryDto.OrganizerId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrganizerName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.MaxAttendees.HasValue)
        {
            return true;
        }
        if (queryDto.ReminderMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.MeetingRoomId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingRoomName))
        {
            return true;
        }
        if (queryDto.MeetingStatus.HasValue)
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
        if (queryDto.StartTimeStart.HasValue || queryDto.StartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndTimeStart.HasValue || queryDto.EndTimeEnd.HasValue)
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
    /// 获取会议件数统计（数据看板；按 StartTime 与 MeetingStatus）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>会议件数统计</returns>
    public async Task<TaktMeetingStatDto> GetMeetingStatAsync(TaktMeetingStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.StartTimeStart,
            queryDto.StartTimeEnd,
            queryDto.StatMonth,
            defaultMonthsAgo: 0);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        // 字典 routine_meeting_center_status：0草稿 1已排期 2进行中 3已结束 4已取消
        const int statusScheduled = 1;
        const int statusEnded = 3;
        Expression<Func<TaktMeeting, bool>> notStarted = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.StartTime >= start
            && x.StartTime <= end
            && x.MeetingStatus == statusScheduled;
        Expression<Func<TaktMeeting, bool>> completed = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.StartTime >= start
            && x.StartTime <= end
            && x.MeetingStatus == statusEnded;
        var monthNotStartedCount = await _meetingRepository.CountAsync(notStarted);
        var monthCompletedCount = await _meetingRepository.CountAsync(completed);
        return new TaktMeetingStatDto
        {
            StatMonth = statMonth,
            MonthNotStartedCount = monthNotStartedCount,
            MonthCompletedCount = monthCompletedCount,
        };
    }
}

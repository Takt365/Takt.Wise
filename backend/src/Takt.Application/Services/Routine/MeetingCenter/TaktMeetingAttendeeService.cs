// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingAttendeeService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：参会人员应用服务实现
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
/// 参会人员应用服务
/// </summary>
public class TaktMeetingAttendeeService : TaktServiceBase, ITaktMeetingAttendeeService
{
    private readonly ITaktCompanyRepository<TaktMeetingAttendee> _meetingAttendeeRepository;
    private readonly ITaktApprovalRepository<TaktMeeting> _meetingRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktMeetingNotificationDispatchService _meetingNotificationDispatchService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingAttendeeRepository">参会人员仓储</param>
    /// <param name="meetingRepository">会议中心仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="meetingNotificationDispatchService">会议通知派发服务</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingAttendeeService(
        ITaktCompanyRepository<TaktMeetingAttendee> meetingAttendeeRepository,
        ITaktApprovalRepository<TaktMeeting> meetingRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktMeetingNotificationDispatchService meetingNotificationDispatchService,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingAttendeeRepository = meetingAttendeeRepository;
        _meetingRepository = meetingRepository;
        _uniqueValidator = uniqueValidator;
        _meetingNotificationDispatchService = meetingNotificationDispatchService;
    }

    /// <summary>
    /// 获取参会人员列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMeetingAttendeeDto>> GetMeetingAttendeeListAsync(TaktMeetingAttendeeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMeetingAttendeeDto>.Create(
                new List<TaktMeetingAttendeeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _meetingAttendeeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMeetingAttendeeDto>.Create(
            data.Adapt<List<TaktMeetingAttendeeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingAttendeeDto?> GetMeetingAttendeeByIdAsync(long id)
    {
        var entity = await _meetingAttendeeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMeetingAttendeeDto>();
    }

    /// <summary>
    /// 获取参会人员选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMeetingAttendeeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _meetingAttendeeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AttendanceStatus == 1,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.UserName,
            DictLabel = e.UserName,
        }).ToList();
    }

    /// <summary>
    /// 创建参会人员
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingAttendeeDto> CreateMeetingAttendeeAsync(TaktMeetingAttendeeCreateDto dto)
    {
        var entity = dto.Adapt<TaktMeetingAttendee>();
        await StampMeetingAttendeeMeetingAsync(entity, dto);
        var isUnique_ix_meeting_attendee_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingAttendeeRepository,
            x => x.MeetingId == entity.MeetingId
                && x.UserId == entity.UserId);
        if (!isUnique_ix_meeting_attendee_unique)
        {
            throw new TaktBusinessException("参会人员的MeetingId、UserId已存在");
        }
        entity = await _meetingAttendeeRepository.CreateAsync(entity);
        await TryNotifyParentMeetingAttendeesAsync(entity.MeetingId);
        return await GetMeetingAttendeeByIdAsync(entity.Id) ?? entity.Adapt<TaktMeetingAttendeeDto>();
    }

    /// <summary>
    /// 更新参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingAttendeeDto> UpdateMeetingAttendeeAsync(long id, TaktMeetingAttendeeUpdateDto dto)
    {
        var entity = await _meetingAttendeeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("参会人员不存在");
        }
        dto.Adapt(entity);
        await StampMeetingAttendeeMeetingAsync(entity, dto);
        var isUnique_ix_meeting_attendee_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingAttendeeRepository,
            x => x.MeetingId == entity.MeetingId
                && x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_meeting_attendee_unique)
        {
            throw new TaktBusinessException("参会人员的MeetingId、UserId已存在");
        }
        await _meetingAttendeeRepository.UpdateAsync(entity);
        await TryNotifyParentMeetingAttendeesAsync(entity.MeetingId);
        return await GetMeetingAttendeeByIdAsync(id) ?? throw new TaktBusinessException("参会人员不存在");
    }

    /// <summary>
    /// 删除参会人员
    /// </summary>
    /// <param name="id">参会人员ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingAttendeeByIdAsync(long id)
    {
        var deleted = await _meetingAttendeeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("参会人员不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除参会人员
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingAttendeeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMeetingAttendeeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新参会人员状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingAttendeeDto> UpdateMeetingAttendeeStatusAsync(TaktMeetingAttendeeStatusDto dto)
    {
        var entity = await _meetingAttendeeRepository.GetByIdAsync(dto.MeetingAttendeeId);
        if (entity == null)
        {
            throw new TaktBusinessException("参会人员不存在");
        }
        entity.AttendanceStatus = dto.AttendanceStatus;
        await _meetingAttendeeRepository.UpdateAsync(entity);
        return await GetMeetingAttendeeByIdAsync(dto.MeetingAttendeeId) ?? throw new TaktBusinessException("参会人员不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMeetingAttendeeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMeetingAttendeeTemplateDto>(
            sheetName ?? "参会人员导入模板",
            fileName ?? "参会人员导入模板.xlsx");
    }

    /// <summary>
    /// 导入参会人员
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMeetingAttendeeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMeetingAttendeeImportDto>(fileStream, sheetName ?? "参会人员导入模板");
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
                var entity = rows[i].Adapt<TaktMeetingAttendee>();
                var importDto = rows[i].Adapt<TaktMeetingAttendeeCreateDto>();
                await StampMeetingAttendeeMeetingAsync(entity, importDto);
                var importKey = $"{entity.MeetingId}|{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MeetingId、UserId）");
                }
                var isUnique_ix_meeting_attendee_unique = await _uniqueValidator.IsUniqueAsync(
                    _meetingAttendeeRepository,
                    x => x.MeetingId == entity.MeetingId
                        && x.UserId == entity.UserId);
                if (!isUnique_ix_meeting_attendee_unique)
                {
                    throw new TaktBusinessException("参会人员的MeetingId、UserId已存在");
                }
                await _meetingAttendeeRepository.CreateAsync(entity);
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
    /// 导出参会人员
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMeetingAttendeeAsync(TaktMeetingAttendeeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMeetingAttendeeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingAttendeeExportDto>(),
                sheetName ?? "参会人员数据",
                fileName ?? "参会人员导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _meetingAttendeeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingAttendeeExportDto>(),
                sheetName ?? "参会人员数据",
                fileName ?? "参会人员导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMeetingAttendeeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "参会人员数据",
            fileName ?? "参会人员导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步参会人员主表外键（ManyToOne → 会议中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMeetingAttendeeMeetingAsync(TaktMeetingAttendee entity, TaktMeetingAttendeeCreateDto dto)
    {
        if (dto.MeetingId <= 0)
        {
            return;
        }
        var master = await _meetingRepository.GetByIdAsync(dto.MeetingId);
        if (master == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        entity.MeetingId = master.Id;
        entity.MeetingTitle = master.MeetingTitle;
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
    }

    /// <summary>
    /// 参会人变更后通知主会议参会人（仅已排期/进行中会议）
    /// </summary>
    /// <param name="meetingId">会议 ID</param>
    /// <returns>任务</returns>
    private async Task TryNotifyParentMeetingAttendeesAsync(long meetingId)
    {
        if (meetingId <= 0)
        {
            return;
        }
        try
        {
            var meeting = await _meetingRepository.GetByIdAsync(meetingId);
            if (meeting == null)
            {
                return;
            }
            if (meeting.MeetingStatus != TaktMeetingConstants.StatusScheduled
                && meeting.MeetingStatus != TaktMeetingConstants.StatusInProgress)
            {
                return;
            }
            await _meetingNotificationDispatchService.NotifyMeetingAttendeesAsync(
                meetingId,
                TaktMeetingNotificationKind.Update);
        }
        catch (Exception ex)
        {
            LogWarning($"参会人变更后会议通知失败: MeetingId={meetingId}, Error={ex.Message}");
        }
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建参会人员查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMeetingAttendee, bool>> QueryExpression(TaktMeetingAttendeeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMeetingAttendee>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
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

        if (queryDto?.MeetingId.HasValue == true)
        {
            var meetingId = queryDto.MeetingId.Value;
            exp = exp.And(x => x.MeetingId == meetingId);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var UserName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(UserName));
        }

        if (queryDto?.AttendeeRole.HasValue == true)
        {
            var attendeeRole = queryDto.AttendeeRole.Value;
            exp = exp.And(x => x.AttendeeRole == attendeeRole);
        }

        if (queryDto?.CheckInMethod.HasValue == true)
        {
            var checkInMethod = queryDto.CheckInMethod.Value;
            exp = exp.And(x => x.CheckInMethod == checkInMethod);
        }

        if (queryDto?.AttendanceStatus.HasValue == true)
        {
            var attendanceStatus = queryDto.AttendanceStatus.Value;
            exp = exp.And(x => x.AttendanceStatus == attendanceStatus);
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

        if (queryDto?.CheckInTimeStart.HasValue == true)
        {
            var checkInTimeStart = queryDto.CheckInTimeStart.Value;
            exp = exp.And(x => x.CheckInTime >= checkInTimeStart);
        }

        if (queryDto?.CheckInTimeEnd.HasValue == true)
        {
            var checkInTimeEnd = queryDto.CheckInTimeEnd.Value;
            exp = exp.And(x => x.CheckInTime <= checkInTimeEnd);
        }

        if (queryDto?.CheckOutTimeStart.HasValue == true)
        {
            var checkOutTimeStart = queryDto.CheckOutTimeStart.Value;
            exp = exp.And(x => x.CheckOutTime >= checkOutTimeStart);
        }

        if (queryDto?.CheckOutTimeEnd.HasValue == true)
        {
            var checkOutTimeEnd = queryDto.CheckOutTimeEnd.Value;
            exp = exp.And(x => x.CheckOutTime <= checkOutTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktMeetingAttendeeQueryDto? queryDto)
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
        if (queryDto.MeetingId.HasValue)
        {
            return true;
        }
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserName))
        {
            return true;
        }
        if (queryDto.AttendeeRole.HasValue)
        {
            return true;
        }
        if (queryDto.CheckInMethod.HasValue)
        {
            return true;
        }
        if (queryDto.AttendanceStatus.HasValue)
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
        if (queryDto.CheckInTimeStart.HasValue || queryDto.CheckInTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CheckOutTimeStart.HasValue || queryDto.CheckOutTimeEnd.HasValue)
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

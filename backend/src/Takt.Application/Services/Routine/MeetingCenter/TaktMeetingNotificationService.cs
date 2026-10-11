// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingNotificationService.cs
// 创建时间：2026-08-26
// 创建人：Takt365(Cursor AI)
// 功能描述：会议通知应用服务实现
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
/// 会议通知应用服务
/// </summary>
public class TaktMeetingNotificationService : TaktServiceBase, ITaktMeetingNotificationService
{
    private readonly ITaktCompanyRepository<TaktMeetingNotification> _meetingNotificationRepository;
    private readonly ITaktApprovalRepository<TaktMeeting> _meetingRepository;
    private readonly ITaktCompanyRepository<TaktMeetingAttendee> _meetingAttendeeRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingNotificationRepository">会议通知仓储</param>
    /// <param name="meetingRepository">会议中心仓储</param>
    /// <param name="meetingAttendeeRepository">参会人员仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingNotificationService(
        ITaktCompanyRepository<TaktMeetingNotification> meetingNotificationRepository,
        ITaktApprovalRepository<TaktMeeting> meetingRepository,
        ITaktCompanyRepository<TaktMeetingAttendee> meetingAttendeeRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingNotificationRepository = meetingNotificationRepository;
        _meetingRepository = meetingRepository;
        _meetingAttendeeRepository = meetingAttendeeRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会议通知列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMeetingNotificationDto>> GetMeetingNotificationListAsync(TaktMeetingNotificationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktMeetingNotificationDto>.Create(
                new List<TaktMeetingNotificationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _meetingNotificationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMeetingNotificationDto>.Create(
            data.Adapt<List<TaktMeetingNotificationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingNotificationDto?> GetMeetingNotificationByIdAsync(long id)
    {
        var entity = await _meetingNotificationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMeetingNotificationDto>();
    }

    /// <summary>
    /// 获取会议通知选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMeetingNotificationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _meetingNotificationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeliveryStatus == 1,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MeetingCode,
            DictLabel = e.UserName ?? e.MeetingCode,
        }).ToList();
    }

    /// <summary>
    /// 创建会议通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public Task<TaktMeetingNotificationDto> CreateMeetingNotificationAsync(TaktMeetingNotificationCreateDto dto)
    {
        throw new TaktBusinessException("会议通知由系统在派发邮件时自动创建，不支持手工新增");
    }

    /// <summary>
    /// 更新会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingNotificationDto> UpdateMeetingNotificationAsync(long id, TaktMeetingNotificationUpdateDto dto)
    {
        var entity = await _meetingNotificationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会议通知不存在");
        }
        dto.Adapt(entity);
        await StampMeetingNotificationMeetingAsync(entity, dto);
        await StampMeetingNotificationMeetingAttendeeAsync(entity, dto);
        var isUnique_ix_meeting_notification_confirm_token_unique = await _uniqueValidator.IsUniqueAsync(
            _meetingNotificationRepository,
            x => x.ConfirmReceiptToken == entity.ConfirmReceiptToken,
            id);
        if (!isUnique_ix_meeting_notification_confirm_token_unique)
        {
            throw new TaktBusinessException("会议通知的ConfirmReceiptToken已存在");
        }
        await _meetingNotificationRepository.UpdateAsync(entity);
        return await GetMeetingNotificationByIdAsync(id) ?? throw new TaktBusinessException("会议通知不存在");
    }

    /// <summary>
    /// 删除会议通知
    /// </summary>
    /// <param name="id">会议通知ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingNotificationByIdAsync(long id)
    {
        var deleted = await _meetingNotificationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会议通知不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会议通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingNotificationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMeetingNotificationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会议通知状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingNotificationDto> UpdateMeetingNotificationStatusAsync(TaktMeetingNotificationStatusDto dto)
    {
        var entity = await _meetingNotificationRepository.GetByIdAsync(dto.MeetingNotificationId);
        if (entity == null)
        {
            throw new TaktBusinessException("会议通知不存在");
        }
        entity.DeliveryStatus = dto.DeliveryStatus;
        await _meetingNotificationRepository.UpdateAsync(entity);
        return await GetMeetingNotificationByIdAsync(dto.MeetingNotificationId) ?? throw new TaktBusinessException("会议通知不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMeetingNotificationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMeetingNotificationTemplateDto>(
            sheetName ?? "会议通知导入模板",
            fileName ?? "会议通知导入模板.xlsx");
    }

    /// <summary>
    /// 导入会议通知
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMeetingNotificationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMeetingNotificationImportDto>(fileStream, sheetName ?? "会议通知导入模板");
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
                var entity = rows[i].Adapt<TaktMeetingNotification>();
                var importDto = rows[i].Adapt<TaktMeetingNotificationCreateDto>();
                await StampMeetingNotificationMeetingAsync(entity, importDto);
                await StampMeetingNotificationMeetingAttendeeAsync(entity, importDto);
                var importKey = $"{entity.ConfirmReceiptToken}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ConfirmReceiptToken）");
                }
                var isUnique_ix_meeting_notification_confirm_token_unique = await _uniqueValidator.IsUniqueAsync(
                    _meetingNotificationRepository,
                    x => x.ConfirmReceiptToken == entity.ConfirmReceiptToken);
                if (!isUnique_ix_meeting_notification_confirm_token_unique)
                {
                    throw new TaktBusinessException("会议通知的ConfirmReceiptToken已存在");
                }
                await _meetingNotificationRepository.CreateAsync(entity);
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
    /// 导出会议通知
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMeetingNotificationAsync(TaktMeetingNotificationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktMeetingNotificationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingNotificationExportDto>(),
                sheetName ?? "会议通知数据",
                fileName ?? "会议通知导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _meetingNotificationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingNotificationExportDto>(),
                sheetName ?? "会议通知数据",
                fileName ?? "会议通知导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMeetingNotificationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会议通知数据",
            fileName ?? "会议通知导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步会议通知主表外键（ManyToOne → 会议中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMeetingNotificationMeetingAsync(TaktMeetingNotification entity, TaktMeetingNotificationCreateDto dto)
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
        if (string.IsNullOrEmpty(entity.MeetingTitle))
        {
            entity.MeetingTitle = master.MeetingTitle;
        }
        if (string.IsNullOrEmpty(entity.MeetingCode))
        {
            entity.MeetingCode = master.MeetingCode;
        }
    }

    /// <summary>
    /// 同步会议通知主表外键（ManyToOne → 参会人员）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMeetingNotificationMeetingAttendeeAsync(TaktMeetingNotification entity, TaktMeetingNotificationCreateDto dto)
    {
        if (dto.MeetingAttendeeId <= 0)
        {
            return;
        }
        var master = await _meetingAttendeeRepository.GetByIdAsync(dto.MeetingAttendeeId);
        if (master == null)
        {
            throw new TaktBusinessException("参会人员不存在");
        }
        entity.MeetingAttendeeId = master.Id;
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
        if (string.IsNullOrEmpty(entity.MeetingTitle))
        {
            entity.MeetingTitle = master.MeetingTitle;
        }
        if (string.IsNullOrEmpty(entity.UserName))
        {
            entity.UserName = master.UserName;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会议通知查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMeetingNotification, bool>> QueryExpression(TaktMeetingNotificationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMeetingNotification>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MeetingTitle != null && x.MeetingTitle.Contains(keywords))
                || (x.MeetingCode != null && x.MeetingCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
                || (x.RecipientEmail != null && x.RecipientEmail.Contains(keywords))
                || (x.NotificationSubject != null && x.NotificationSubject.Contains(keywords))
                || (x.ConfirmReceiptToken != null && x.ConfirmReceiptToken.Contains(keywords))
                || (x.ConfirmedByUserName != null && x.ConfirmedByUserName.Contains(keywords))
                || (x.SendErrorMessage != null && x.SendErrorMessage.Contains(keywords))
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

        if (queryDto?.MeetingAttendeeId.HasValue == true)
        {
            var meetingAttendeeId = queryDto.MeetingAttendeeId.Value;
            exp = exp.And(x => x.MeetingAttendeeId == meetingAttendeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingTitle))
        {
            var meetingTitle = queryDto.MeetingTitle;
            exp = exp.And(x => x.MeetingTitle != null && x.MeetingTitle.Contains(meetingTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MeetingCode))
        {
            var meetingCode = queryDto.MeetingCode;
            exp = exp.And(x => x.MeetingCode != null && x.MeetingCode.Contains(meetingCode));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var userName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(userName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RecipientEmail))
        {
            var recipientEmail = queryDto.RecipientEmail;
            exp = exp.And(x => x.RecipientEmail != null && x.RecipientEmail.Contains(recipientEmail));
        }

        if (queryDto?.NotificationType.HasValue == true)
        {
            var notificationType = queryDto.NotificationType.Value;
            exp = exp.And(x => x.NotificationType == notificationType);
        }

        if (queryDto?.NotificationChannel.HasValue == true)
        {
            var notificationChannel = queryDto.NotificationChannel.Value;
            exp = exp.And(x => x.NotificationChannel == notificationChannel);
        }

        if (queryDto?.DeliveryStatus.HasValue == true)
        {
            var deliveryStatus = queryDto.DeliveryStatus.Value;
            exp = exp.And(x => x.DeliveryStatus == deliveryStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NotificationSubject))
        {
            var notificationSubject = queryDto.NotificationSubject;
            exp = exp.And(x => x.NotificationSubject != null && x.NotificationSubject.Contains(notificationSubject));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConfirmReceiptToken))
        {
            var confirmReceiptToken = queryDto.ConfirmReceiptToken;
            exp = exp.And(x => x.ConfirmReceiptToken != null && x.ConfirmReceiptToken.Contains(confirmReceiptToken));
        }

        if (queryDto?.ConfirmedByUserId.HasValue == true)
        {
            var confirmedByUserId = queryDto.ConfirmedByUserId.Value;
            exp = exp.And(x => x.ConfirmedByUserId == confirmedByUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConfirmedByUserName))
        {
            var confirmedByUserName = queryDto.ConfirmedByUserName;
            exp = exp.And(x => x.ConfirmedByUserName != null && x.ConfirmedByUserName.Contains(confirmedByUserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SendErrorMessage))
        {
            var sendErrorMessage = queryDto.SendErrorMessage;
            exp = exp.And(x => x.SendErrorMessage != null && x.SendErrorMessage.Contains(sendErrorMessage));
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

        if (queryDto?.SentAtStart.HasValue == true)
        {
            var sentAtStart = queryDto.SentAtStart.Value;
            exp = exp.And(x => x.SentAt >= sentAtStart);
        }

        if (queryDto?.SentAtEnd.HasValue == true)
        {
            var sentAtEnd = queryDto.SentAtEnd.Value;
            exp = exp.And(x => x.SentAt <= sentAtEnd);
        }

        if (queryDto?.ConfirmedAtStart.HasValue == true)
        {
            var confirmedAtStart = queryDto.ConfirmedAtStart.Value;
            exp = exp.And(x => x.ConfirmedAt >= confirmedAtStart);
        }

        if (queryDto?.ConfirmedAtEnd.HasValue == true)
        {
            var confirmedAtEnd = queryDto.ConfirmedAtEnd.Value;
            exp = exp.And(x => x.ConfirmedAt <= confirmedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktMeetingNotificationQueryDto? queryDto)
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
        if (queryDto.MeetingAttendeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MeetingCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.RecipientEmail))
        {
            return true;
        }
        if (queryDto.NotificationType.HasValue)
        {
            return true;
        }
        if (queryDto.NotificationChannel.HasValue)
        {
            return true;
        }
        if (queryDto.DeliveryStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NotificationSubject))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfirmReceiptToken))
        {
            return true;
        }
        if (queryDto.ConfirmedByUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfirmedByUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SendErrorMessage))
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
        if (queryDto.SentAtStart.HasValue || queryDto.SentAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ConfirmedAtStart.HasValue || queryDto.ConfirmedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 按邮件令牌确认收到会议通知（匿名；凭令牌定位记录）
    /// </summary>
    /// <param name="dto">令牌 DTO</param>
    /// <returns>确认结果</returns>
    public async Task<TaktMeetingNotificationConfirmReceiptResultDto> ConfirmMeetingNotificationReceiptByTokenAsync(
        TaktMeetingNotificationConfirmReceiptByTokenDto dto)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.ConfirmReceiptToken);
        var token = dto.ConfirmReceiptToken.Trim();
        var entity = await _meetingNotificationRepository.FirstAsync(
            x => x.ConfirmReceiptToken == token && x.IsDeleted == 0);
        if (entity == null)
        {
            throw new TaktBusinessException("回执确认链接无效或已过期");
        }
        return await ApplyConfirmReceiptAsync(entity);
    }

    /// <summary>
    /// 当前登录用户确认收到会议通知（须为通知收件人）
    /// </summary>
    /// <param name="id">会议通知 ID</param>
    /// <returns>确认结果</returns>
    public async Task<TaktMeetingNotificationConfirmReceiptResultDto> ConfirmMeetingNotificationReceiptAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await _meetingNotificationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会议通知不存在");
        }
        if (!IsAuthenticated || CurrentUserId == null)
        {
            throw new TaktBusinessException("请先登录后再确认回执");
        }
        if (entity.UserId != CurrentUserId.Value)
        {
            throw new TaktBusinessException("仅通知收件人本人可确认回执");
        }
        return await ApplyConfirmReceiptAsync(entity);
    }

    /// <summary>
    /// 写入回执确认状态（已确认则幂等返回）
    /// </summary>
    /// <param name="entity">通知实体</param>
    /// <returns>确认结果</returns>
    private async Task<TaktMeetingNotificationConfirmReceiptResultDto> ApplyConfirmReceiptAsync(TaktMeetingNotification entity)
    {
        if (entity.DeliveryStatus == TaktMeetingConstants.NotificationStatusConfirmed)
        {
            return new TaktMeetingNotificationConfirmReceiptResultDto
            {
                MeetingNotificationId = entity.Id,
                MeetingTitle = entity.MeetingTitle ?? string.Empty,
                AlreadyConfirmed = true,
                ConfirmedAt = entity.ConfirmedAt,
            };
        }
        if (entity.DeliveryStatus != TaktMeetingConstants.NotificationStatusSent)
        {
            throw new TaktBusinessException("当前通知尚未成功发送，无法确认回执");
        }
        var now = DateTime.UtcNow;
        entity.DeliveryStatus = TaktMeetingConstants.NotificationStatusConfirmed;
        entity.ConfirmedAt = now;
        entity.ConfirmedByUserId = entity.UserId;
        entity.ConfirmedByUserName = entity.UserName;
        await _meetingNotificationRepository.UpdateAsync(entity);
        return new TaktMeetingNotificationConfirmReceiptResultDto
        {
            MeetingNotificationId = entity.Id,
            MeetingTitle = entity.MeetingTitle ?? string.Empty,
            AlreadyConfirmed = false,
            ConfirmedAt = now,
        };
    }
}

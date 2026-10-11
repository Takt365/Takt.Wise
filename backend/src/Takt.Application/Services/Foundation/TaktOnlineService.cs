// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktOnlineService.cs
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：在线用户应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Statistics.Logging;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Enums;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 在线用户应用服务
/// </summary>
public class TaktOnlineService : TaktServiceBase, ITaktOnlineService
{
    private readonly ITaktCompanyRepository<TaktOnline> _onlineRepository;
    private readonly ITaktCompanyRepository<TaktDurationLog> _durationLogRepository;
    private readonly ITaktCompanyRepository<TaktVisitLog> _visitLogRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="onlineRepository">在线用户仓储</param>
    /// <param name="durationLogRepository">在线时长日汇总仓储</param>
    /// <param name="visitLogRepository">访问量日汇总仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktOnlineService(
        ITaktCompanyRepository<TaktOnline> onlineRepository,
        ITaktCompanyRepository<TaktDurationLog> durationLogRepository,
        ITaktCompanyRepository<TaktVisitLog> visitLogRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _onlineRepository = onlineRepository;
        _durationLogRepository = durationLogRepository;
        _visitLogRepository = visitLogRepository;
    }

    /// <summary>
    /// 获取在线用户列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktOnlineDto>> GetOnlineListAsync(TaktOnlineQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (items, total) = await _onlineRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            orderBy: x => x.CreatedAt,
            isDesc: true);
        var dtos = items.Adapt<List<TaktOnlineDto>>();
        foreach (var dto in dtos)
        {
            EnrichOnlineDto(dto);
        }
        return TaktPagedResult<TaktOnlineDto>.Create(dtos, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktOnlineDto?> GetOnlineByIdAsync(long id)
    {
        var entity = await _onlineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktOnlineDto>();
        EnrichOnlineDto(dto);
        return dto;
    }

    /// <summary>
    /// 获取在线用户选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetOnlineOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _onlineRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrEmpty(e.UserName) ? e.Id.ToString() : e.UserName,
        }).ToList();
    }

    /// <summary>
    /// 注册 SignalR 在线会话（租户+公司+UserId 唯一一行：存在则更新，不存在则插入）
    /// </summary>
    /// <param name="dto">连接信息</param>
    /// <returns>在线用户 DTO</returns>
    public async Task<TaktOnlineDto> RegisterOnlineSessionAsync(TaktOnlineCreateDto dto)
    {
        EnsureThreeLayerContext();
        if (dto.UserId <= 0)
        {
            throw new TaktBusinessException("无法解析在线用户 ID");
        }

        if (string.IsNullOrWhiteSpace(dto.ConnectionId))
        {
            throw new TaktBusinessException("ConnectionId 不能为空");
        }

        var userId = dto.UserId;
        var connectTime = dto.ConnectTime;
        var connectLocation = TaktHttpAuditHelper.ResolveLocationFromIp(dto.ConnectIp, dto.ConnectLocation);
        var clientProfile = TaktUserAgentHelper.Resolve(
            dto.UserAgent,
            dto.BrowserType,
            dto.OperatingSystem,
            dto.DeviceType);
        var entity = await ResolveOnlineByUserIdAsync(userId);
        if (entity == null)
        {
            entity = dto.Adapt<TaktOnline>();
            entity.ConnectionId = dto.ConnectionId.Trim();
            entity.UserName = dto.UserName.Trim();
            entity.ConnectIp = dto.ConnectIp ?? string.Empty;
            entity.ConnectLocation = connectLocation ?? string.Empty;
            entity.UserAgent = dto.UserAgent ?? string.Empty;
            entity.DeviceType = clientProfile.DeviceType;
            entity.BrowserType = clientProfile.Browser;
            entity.OperatingSystem = clientProfile.OperatingSystem;
            entity.ConnectionDuration = 0;
            entity.LastActiveTime = connectTime;
            entity.DisconnectTime = null;
            entity.OnlineStatus = 0;
            await CaptureSessionDurationBaselinesAsync(entity, connectTime);
            entity = await _onlineRepository.CreateAsync(entity);
        }
        else
        {
            if (entity.OnlineStatus == 0)
            {
                await FlushOnlineSessionDurationToDailyLogAsync(entity, connectTime);
            }

            entity.ConnectionId = dto.ConnectionId.Trim();
            entity.UserName = dto.UserName.Trim();
            entity.ConnectIp = dto.ConnectIp ?? string.Empty;
            entity.ConnectLocation = connectLocation ?? string.Empty;
            entity.UserAgent = dto.UserAgent ?? string.Empty;
            entity.DeviceType = clientProfile.DeviceType;
            entity.BrowserType = clientProfile.Browser;
            entity.OperatingSystem = clientProfile.OperatingSystem;
            entity.ConnectTime = connectTime;
            entity.LastActiveTime = connectTime;
            entity.DisconnectTime = null;
            entity.OnlineStatus = 0;
            entity.ConnectionDuration = 0;
            entity.UpdatedAt = connectTime;
            await CaptureSessionDurationBaselinesAsync(entity, connectTime);
            await _onlineRepository.UpdateAsync(entity);
        }

        return await GetOnlineByIdAsync(entity.Id) ?? entity.Adapt<TaktOnlineDto>();
    }

    /// <summary>
    /// SignalR Heartbeat 累计 ConnectionDuration（每次 +ReportingIntervalSeconds）
    /// </summary>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <param name="activeAt">活跃时刻</param>
    /// <returns>是否成功累计</returns>
    public async Task<bool> RefreshOnlineConnectionDurationAsync(string connectionId, DateTime activeAt)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);

        var online = await _onlineRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ConnectionId == connectionId.Trim()
            && x.OnlineStatus == 0
            && x.IsDeleted == 0);
        if (online == null)
        {
            return false;
        }

        if (!TryApplyHeartbeatIncrement(online, activeAt))
        {
            return false;
        }

        await SyncTodayDailyDurationFromActiveSessionAsync(online, activeAt);
        online.UpdatedAt = activeAt;
        await _onlineRepository.UpdateAsync(online);
        return true;
    }

    /// <summary>
    /// 按 ConnectionId 关闭在线会话（仅写 DisconnectTime/OnlineStatus，保留已累计 ConnectionDuration）
    /// </summary>
    /// <param name="connectionId">SignalR 连接 ID</param>
    /// <param name="disconnectTime">断开时间</param>
    /// <param name="onlineStatus">离线状态（默认 1=离线；强退可传 2=离开）</param>
    /// <returns>是否更新到记录</returns>
    public async Task<bool> CloseOnlineSessionByConnectionIdAsync(
        string connectionId,
        DateTime disconnectTime,
        int onlineStatus = 1)
    {
        EnsureThreeLayerContext();
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionId);

        var online = await _onlineRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ConnectionId == connectionId.Trim()
            && x.OnlineStatus == 0
            && x.IsDeleted == 0);
        if (online == null)
        {
            return false;
        }

        await FlushOnlineSessionDurationToDailyLogAsync(online, disconnectTime);
        ApplyOfflineSessionState(online, disconnectTime, onlineStatus);
        await _onlineRepository.UpdateAsync(online);
        return true;
    }

    /// <summary>
    /// 按用户 ID 关闭当前租户+公司下所有在线会话（HTTP 登出时 SignalR 可能尚未断开）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="disconnectTime">断开/登出时刻</param>
    /// <param name="onlineStatus">离线状态（默认 1=离线）</param>
    /// <returns>关闭的会话数</returns>
    public async Task<int> CloseOnlineSessionsByUserIdAsync(
        long userId,
        DateTime disconnectTime,
        int onlineStatus = 1)
    {
        EnsureThreeLayerContext();
        if (userId <= 0)
        {
            return 0;
        }

        var onlineSessions = await _onlineRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.UserId == userId
            && x.OnlineStatus == 0
            && x.IsDeleted == 0);
        if (onlineSessions.Count == 0)
        {
            return 0;
        }

        var closedCount = 0;
        foreach (var online in onlineSessions)
        {
            await FlushOnlineSessionDurationToDailyLogAsync(online, disconnectTime);
            ApplyOfflineSessionState(online, disconnectTime, onlineStatus);
            await _onlineRepository.UpdateAsync(online);
            closedCount++;
        }

        return closedCount;
    }

    /// <summary>
    /// 删除在线用户
    /// </summary>
    /// <param name="id">在线用户ID</param>
    /// <returns>任务</returns>
    public async Task DeleteOnlineByIdAsync(long id)
    {
        EnsureThreeLayerContext();
        var entity = await _onlineRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("在线用户不存在或已删除");
        }

        var disconnectTime = DateTime.Now;
        if (entity.OnlineStatus == 0)
        {
            await FlushOnlineSessionDurationToDailyLogAsync(entity, disconnectTime);
        }

        ApplyOfflineSessionState(entity, disconnectTime);
        await _onlineRepository.UpdateAsync(entity);

        var deleted = await _onlineRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("在线用户不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除在线用户
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteOnlineBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteOnlineByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新在线用户状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktOnlineDto> UpdateOnlineStatusAsync(TaktOnlineStatusDto dto)
    {
        EnsureThreeLayerContext();
        var entity = await _onlineRepository.GetByIdAsync(dto.OnlineId);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("在线用户不存在");
        }

        if (entity.OnlineStatus == 0 && dto.OnlineStatus != 0)
        {
            var disconnectTime = DateTime.Now;
            await FlushOnlineSessionDurationToDailyLogAsync(entity, disconnectTime);
            ApplyOfflineSessionState(entity, disconnectTime, dto.OnlineStatus);
        }
        else
        {
            entity.OnlineStatus = dto.OnlineStatus;
        }

        await _onlineRepository.UpdateAsync(entity);
        return await GetOnlineByIdAsync(dto.OnlineId) ?? throw new TaktBusinessException("在线用户不存在");
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportOnlineAsync(TaktOnlineQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktOnlineQueryDto());
        var list = await _onlineRepository.GetListForExportAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktOnlineExportDto>(),
                sheetName ?? "在线用户数据",
                fileName ?? "在线用户导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktOnlineExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "在线用户数据",
            fileName ?? "在线用户导出.xlsx");
    }

    /// <summary>
    /// 获取在线时长统计（唯一入口：当前/当天/本周日均/本月日均）
    /// </summary>
    /// <param name="queryDto">查询 DTO（可选 UserName，为空时取当前登录用户）</param>
    /// <returns>统计 DTO</returns>
    public async Task<TaktOnlineStatisticsDto> GetOnlineStatisticsAsync(TaktOnlineStatisticsQueryDto? queryDto = null)
    {
        EnsureThreeLayerContext();
        var normalizedUserName = !string.IsNullOrWhiteSpace(queryDto?.UserName)
            ? queryDto.UserName.Trim()
            : RequireCurrentUserName();

        var now = DateTime.Now;
        var todayStart = DateTime.Today;
        var weekStart = GetWeekStartMonday(todayStart);
        var monthStart = new DateTime(now.Year, now.Month, 1);

        var onlineRecord = await _onlineRepository.FirstAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.UserName == normalizedUserName
            && online.IsDeleted == 0);

        if (onlineRecord == null)
        {
            return new TaktOnlineStatisticsDto
            {
                UserName = normalizedUserName,
                UserId = CurrentUserId,
            };
        }

        var onlineCount = onlineRecord.OnlineStatus == 0 ? 1 : 0;
        var currentDurationSeconds = onlineRecord.OnlineStatus == 0
            ? GetEffectiveDurationSeconds(onlineRecord, now)
            : 0;
        var todayDuration = GetDurationInRange(onlineRecord, todayStart, now);
        var weekTotal = GetDurationInRange(onlineRecord, weekStart, now);
        var monthTotal = GetDurationInRange(onlineRecord, monthStart, now);
        var weekElapsedDays = (todayStart - weekStart).Days + 1;
        var monthElapsedDays = (todayStart - monthStart).Days + 1;

        return new TaktOnlineStatisticsDto
        {
            UserName = normalizedUserName,
            UserId = onlineRecord.UserId,
            OnlineCount = onlineCount,
            CurrentDurationSeconds = currentDurationSeconds,
            TodayDurationSeconds = todayDuration,
            WeekTotalDurationSeconds = weekTotal,
            WeekAverageDurationSeconds = weekElapsedDays > 0 ? weekTotal / weekElapsedDays : 0,
            MonthDurationSeconds = monthTotal,
            MonthAverageDurationSeconds = monthElapsedDays > 0 ? monthTotal / monthElapsedDays : 0,
        };
    }

    /// <summary>
    /// 获取在线看板统计（公司维度：在线人数、当日总访问量）
    /// </summary>
    /// <returns>看板统计 DTO</returns>
    public async Task<TaktOnlineDashboardStatisticsDto> GetOnlineDashboardStatisticsAsync()
    {
        EnsureThreeLayerContext();
        var today = DateTime.Today;
        var onlineUserCount = await _onlineRepository.CountAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.OnlineStatus == 0
            && online.IsDeleted == 0);
        var visitRows = await _visitLogRepository.GetListAsync(log =>
            log.TenantCode == CurrentTenantCode
            && log.CompanyCode == CurrentCompanyCode
            && log.StatDate == today
            && log.IsDeleted == 0);
        var todayVisitCount = visitRows.Sum(row => row.VisitCount);
        return new TaktOnlineDashboardStatisticsDto
        {
            OnlineUserCount = onlineUserCount,
            TodayVisitCount = todayVisitCount,
        };
    }

    /// <summary>
    /// 解析并校验当前登录用户名
    /// </summary>
    /// <returns>用户名</returns>
    private string RequireCurrentUserName()
    {
        if (string.IsNullOrWhiteSpace(CurrentUserName))
        {
            throw new TaktBusinessException("无法解析当前登录用户");
        }

        return CurrentUserName.Trim();
    }

    /// <summary>
    /// 按 UserId 解析在线用户唯一行（合并历史重复行，保留 Id 最小的一条）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <returns>在线实体；不存在时 null</returns>
    private async Task<TaktOnline?> ResolveOnlineByUserIdAsync(long userId)
    {
        var records = await _onlineRepository.GetListAsync(online =>
            online.TenantCode == CurrentTenantCode
            && online.CompanyCode == CurrentCompanyCode
            && online.UserId == userId
            && online.IsDeleted == 0);
        if (records.Count == 0)
        {
            return null;
        }

        var primary = records.OrderBy(online => online.Id).First();
        foreach (var duplicate in records.Where(online => online.Id != primary.Id))
        {
            await _onlineRepository.DeleteAsync(duplicate.Id);
        }

        return primary;
    }

    /// <summary>
    /// 将会话标记为离线（仅写 DisconnectTime/OnlineStatus，不修改 ConnectionDuration）
    /// </summary>
    /// <param name="online">在线用户实体</param>
    /// <param name="disconnectTime">断开时间</param>
    /// <param name="onlineStatus">离线状态（1=离线 2=离开）</param>
    private static void ApplyOfflineSessionState(TaktOnline online, DateTime disconnectTime, int onlineStatus = 1)
    {
        if (online.OnlineStatus != 0)
        {
            return;
        }

        online.DisconnectTime = disconnectTime;
        online.LastActiveTime = disconnectTime;
        online.OnlineStatus = onlineStatus;
    }

    // ========================================
    // ConnectionDuration 与在线时长统计（唯一实现）
    // ========================================

    /// <summary>
    /// 将会话有效时长按自然日写入 TaktDurationLog（租户+公司+用户+StatDate 唯一）
    /// </summary>
    /// <param name="online">在线记录（须为当前会话快照，调用前勿清零 ConnectionDuration）</param>
    /// <param name="sessionEnd">会话结束时刻</param>
    private async Task FlushOnlineSessionDurationToDailyLogAsync(TaktOnline online, DateTime sessionEnd)
    {
        if (sessionEnd <= online.ConnectTime)
        {
            return;
        }

        var startDate = online.ConnectTime.Date;
        var endDate = sessionEnd.Date;
        for (var statDate = startDate; statDate <= endDate; statDate = statDate.AddDays(1))
        {
            var dayStart = statDate;
            var dayEnd = statDate.AddDays(1).AddTicks(-1);
            if (dayEnd > sessionEnd)
            {
                dayEnd = sessionEnd;
            }

            var sessionDaySeconds = GetDurationInRange(online, dayStart, dayEnd, sessionEnd);
            if (sessionDaySeconds <= 0)
            {
                continue;
            }

            var baseline = await ResolveSessionDayBaselineAsync(online, statDate);
            var totalSeconds = checked(baseline + sessionDaySeconds);
            await SetDailyDurationAsync(
                online.UserId,
                online.UserName,
                statDate,
                (int)Math.Min(totalSeconds, int.MaxValue));
        }
    }

    /// <summary>
    /// Heartbeat 后同步当日 TaktDurationLog（基线 + 当前会话当日有效时长）
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="activeAt">活跃时刻</param>
    private async Task SyncTodayDailyDurationFromActiveSessionAsync(TaktOnline online, DateTime activeAt)
    {
        await RefreshDailyDurationBaselineIfDayChangedAsync(online, activeAt);
        var today = activeAt.Date;
        var liveSeconds = GetDurationInRange(online, today, activeAt, activeAt);
        if (liveSeconds <= 0)
        {
            return;
        }

        var baseline = today == online.ConnectTime.Date
            ? online.SessionDurationBaselineSeconds
            : online.DailyDurationBaselineSeconds;
        var totalSeconds = checked(baseline + liveSeconds);
        await SetDailyDurationAsync(
            online.UserId,
            online.UserName,
            today,
            (int)Math.Min(totalSeconds, int.MaxValue));
    }

    /// <summary>
    /// 会话开始时捕获当日 TaktDurationLog 基线
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="connectTime">连接时刻</param>
    private async Task CaptureSessionDurationBaselinesAsync(TaktOnline online, DateTime connectTime)
    {
        var todaySeconds = await GetTodayDurationSecondsAsync(online.UserId, connectTime.Date);
        online.SessionDurationBaselineSeconds = todaySeconds;
        online.DailyDurationBaselineSeconds = todaySeconds;
        online.DailyDurationBaselineDate = connectTime.Date;
    }

    /// <summary>
    /// 跨自然日会话时刷新日汇总基线
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="activeAt">活跃时刻</param>
    private async Task RefreshDailyDurationBaselineIfDayChangedAsync(TaktOnline online, DateTime activeAt)
    {
        var today = activeAt.Date;
        if (online.DailyDurationBaselineDate.Date == today)
        {
            return;
        }

        online.DailyDurationBaselineSeconds = await GetTodayDurationSecondsAsync(online.UserId, today);
        online.DailyDurationBaselineDate = today;
    }

    /// <summary>
    /// 读取用户指定自然日已落库的累计秒数
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="statDate">自然日</param>
    /// <returns>累计秒数；无记录时 0</returns>
    private async Task<int> GetTodayDurationSecondsAsync(long userId, DateTime statDate)
    {
        var normalizedStatDate = statDate.Date;
        var existing = await _durationLogRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.UserId == userId
            && x.StatDate == normalizedStatDate
            && x.IsDeleted == 0);
        return existing?.DurationSeconds ?? 0;
    }

    /// <summary>
    /// 解析会话在指定自然日的 TaktDurationLog 基线秒数
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="statDate">自然日</param>
    /// <returns>基线秒数</returns>
    private async Task<int> ResolveSessionDayBaselineAsync(TaktOnline online, DateTime statDate)
    {
        var normalizedStatDate = statDate.Date;
        if (normalizedStatDate == online.ConnectTime.Date)
        {
            return online.SessionDurationBaselineSeconds;
        }

        if (normalizedStatDate == online.DailyDurationBaselineDate.Date)
        {
            return online.DailyDurationBaselineSeconds;
        }

        return await GetTodayDurationSecondsAsync(online.UserId, normalizedStatDate);
    }

    /// <summary>
    /// 设置指定自然日累计时长（覆盖写入，避免重复累加）
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="UserName">用户名</param>
    /// <param name="statDate">自然日</param>
    /// <param name="durationSeconds">当日累计秒数</param>
    private async Task SetDailyDurationAsync(long userId, string UserName, DateTime statDate, int durationSeconds)
    {
        if (durationSeconds < 0)
        {
            return;
        }

        var normalizedStatDate = statDate.Date;
        var existing = await _durationLogRepository.FirstAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.UserId == userId
            && x.StatDate == normalizedStatDate
            && x.IsDeleted == 0);
        if (existing == null)
        {
            if (durationSeconds == 0)
            {
                return;
            }

            await _durationLogRepository.CreateAsync(new TaktDurationLog
            {
                TenantCode = CurrentTenantCode,
                CompanyCode = CurrentCompanyCode,
                UserId = userId,
                UserName = UserName.Trim(),
                StatDate = normalizedStatDate,
                DurationSeconds = durationSeconds,
            });
            return;
        }

        if (existing.DurationSeconds == durationSeconds)
        {
            return;
        }

        existing.UserName = UserName.Trim();
        existing.DurationSeconds = durationSeconds;
        existing.UpdatedAt = DateTime.Now;
        await _durationLogRepository.UpdateAsync(existing);
    }

    /// <summary>
    /// Heartbeat 累计 ConnectionDuration（唯一写入路径）
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="activeAt">活跃时刻</param>
    /// <returns>是否成功累计</returns>
    private static bool TryApplyHeartbeatIncrement(TaktOnline online, DateTime activeAt)
    {
        var lastActiveAt = online.LastActiveTime;
        if ((activeAt - lastActiveAt).TotalSeconds < TaktOnlineConstants.MinReportingGapSeconds)
        {
            return false;
        }

        online.ConnectionDuration = checked(online.ConnectionDuration + TaktOnlineConstants.ReportingIntervalSeconds);
        online.LastActiveTime = activeAt;
        return true;
    }

    /// <summary>
    /// 读取会话在指定时刻的有效累计时长（不修改 ConnectionDuration 列）
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="asOf">统计截止时刻</param>
    /// <returns>有效累计秒数</returns>
    private static long GetEffectiveDurationSeconds(TaktOnline online, DateTime asOf)
    {
        var storedDuration = online.ConnectionDuration;
        if (online.OnlineStatus != 0)
        {
            return storedDuration;
        }

        var lastActiveAt = online.LastActiveTime;
        var pendingSeconds = (long)(asOf - lastActiveAt).TotalSeconds;
        if (pendingSeconds <= 0)
        {
            return storedDuration;
        }

        pendingSeconds = Math.Min(pendingSeconds, TaktOnlineConstants.ReportingIntervalSeconds);
        return checked(storedDuration + pendingSeconds);
    }

    /// <summary>
    /// 计算单条会话在统计区间内的有效时长（秒）
    /// </summary>
    /// <param name="online">在线记录</param>
    /// <param name="rangeStart">区间起点（含）</param>
    /// <param name="rangeEnd">区间终点（含）</param>
    /// <param name="sessionEndOverride">会话结束时刻覆盖值；为空时在线取 rangeEnd，离线取 DisconnectTime</param>
    /// <returns>区间内有效秒数</returns>
    private static long GetDurationInRange(
        TaktOnline online,
        DateTime rangeStart,
        DateTime rangeEnd,
        DateTime? sessionEndOverride = null)
    {
        if (rangeEnd < rangeStart)
        {
            throw new ArgumentOutOfRangeException(nameof(rangeEnd), "统计区间终点不能早于起点");
        }

        var sessionEnd = sessionEndOverride
            ?? (online.OnlineStatus == 0 ? rangeEnd : online.DisconnectTime ?? rangeEnd);
        if (sessionEnd <= rangeStart)
        {
            return 0;
        }

        var effectiveStart = online.ConnectTime > rangeStart ? online.ConnectTime : rangeStart;
        var clippedWallSeconds = (long)(sessionEnd - effectiveStart).TotalSeconds;
        if (clippedWallSeconds <= 0)
        {
            return 0;
        }

        var totalDuration = GetEffectiveDurationSeconds(online, sessionEnd);
        if (totalDuration <= 0)
        {
            return 0;
        }

        var fullWallSeconds = (long)(sessionEnd - online.ConnectTime).TotalSeconds;
        if (fullWallSeconds <= 0)
        {
            return totalDuration;
        }

        return (long)(totalDuration * ((double)clippedWallSeconds / fullWallSeconds));
    }

    /// <summary>
    /// 获取以周一为起点的自然周开始日期
    /// </summary>
    /// <param name="date">参考日期</param>
    /// <returns>当周周一（含）</returns>
    private static DateTime GetWeekStartMonday(DateTime date)
    {
        var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return date.Date.AddDays(-diff);
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建在线用户查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktOnline, bool>> QueryExpression(TaktOnlineQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktOnline>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                x.ConnectionId.Contains(keywords)
                || x.UserName.Contains(keywords)
                || SqlFunc.ToString(x.UserId).Contains(keywords)
                || SqlFunc.ToString(x.OnlineStatus).Contains(keywords)
                || x.ConnectIp.Contains(keywords)
                || (x.ConnectLocation != null && x.ConnectLocation.Contains(keywords))
                || (x.UserAgent != null && x.UserAgent.Contains(keywords))
                || x.DeviceType.Contains(keywords)
                || x.BrowserType.Contains(keywords)
                || x.OperatingSystem.Contains(keywords)
                || SqlFunc.ToString(x.ConnectionDuration).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ConnectTime).Contains(keywords)
                || SqlFunc.ToString(x.LastActiveTime).Contains(keywords)
                || SqlFunc.ToString(x.DisconnectTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectionId))
        {
            exp = exp.And(x => x.ConnectionId.Contains(queryDto.ConnectionId));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserName))
        {
            exp = exp.And(x => x.UserName.Contains(queryDto.UserName));
        }

        if (queryDto?.UserId.HasValue == true)
        {
            exp = exp.And(x => x.UserId == queryDto.UserId);
        }

        if (queryDto?.OnlineStatus.HasValue == true)
        {
            exp = exp.And(x => x.OnlineStatus == queryDto.OnlineStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectIp))
        {
            exp = exp.And(x => x.ConnectIp.Contains(queryDto.ConnectIp));
        }

        if (!string.IsNullOrEmpty(queryDto?.ConnectLocation))
        {
            exp = exp.And(x => x.ConnectLocation != null && x.ConnectLocation.Contains(queryDto.ConnectLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.UserAgent))
        {
            exp = exp.And(x => x.UserAgent != null && x.UserAgent.Contains(queryDto.UserAgent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeviceType))
        {
            var deviceType = queryDto.DeviceType.Trim();
            exp = exp.And(x => x.DeviceType == deviceType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BrowserType))
        {
            var browserType = queryDto.BrowserType.Trim();
            exp = exp.And(x => x.BrowserType == browserType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OperatingSystem))
        {
            var operatingSystem = queryDto.OperatingSystem.Trim();
            exp = exp.And(x => x.OperatingSystem == operatingSystem);
        }

        if (queryDto?.ConnectionDuration.HasValue == true)
        {
            exp = exp.And(x => x.ConnectionDuration == queryDto.ConnectionDuration);
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

        if (queryDto?.ConnectTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.ConnectTime >= queryDto.ConnectTimeStart);
        }

        if (queryDto?.ConnectTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.ConnectTime <= queryDto.ConnectTimeEnd);
        }

        if (queryDto?.LastActiveTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.LastActiveTime >= queryDto.LastActiveTimeStart);
        }

        if (queryDto?.LastActiveTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.LastActiveTime <= queryDto.LastActiveTimeEnd);
        }

        if (queryDto?.DisconnectTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.DisconnectTime >= queryDto.DisconnectTimeStart);
        }

        if (queryDto?.DisconnectTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.DisconnectTime <= queryDto.DisconnectTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }

    /// <summary>
    /// 列表/详情展示时根据 User-Agent 回填 BrowserType/OperatingSystem/DeviceType
    /// </summary>
    /// <param name="dto">在线用户 DTO</param>
    private static void EnrichOnlineDto(TaktOnlineDto dto)
    {
        dto.ConnectLocation = TaktHttpAuditHelper.ResolveLocationFromIp(dto.ConnectIp, dto.ConnectLocation);
        var profile = TaktUserAgentHelper.Resolve(
            dto.UserAgent,
            dto.BrowserType ?? TaktConstants.BrowserType.Unknown,
            dto.OperatingSystem ?? TaktConstants.OperatingSystem.Unknown,
            dto.DeviceType ?? TaktConstants.DeviceType.Unknown);
        dto.BrowserType = profile.Browser;
        dto.OperatingSystem = profile.OperatingSystem;
        dto.DeviceType = profile.DeviceType;
    }
}
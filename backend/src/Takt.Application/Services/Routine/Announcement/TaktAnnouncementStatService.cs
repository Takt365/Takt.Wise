// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.Announcement
// 文件名称：TaktAnnouncementStatService.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知看板统计服务（与 Announcement CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Domain.Entities.Routine.Announcement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Routine.Announcement;

/// <summary>
/// 公告通知看板统计服务（读公告表；与 TaktAnnouncementService CRUD 分离）
/// </summary>
public class TaktAnnouncementStatService : TaktServiceBase, ITaktAnnouncementStatService
{
    private readonly ITaktApprovalRepository<TaktAnnouncement> _announcementRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="announcementRepository">公告仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAnnouncementStatService(
        ITaktApprovalRepository<TaktAnnouncement> announcementRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _announcementRepository = announcementRepository;
    }

    /// <inheritdoc />
    public async Task<TaktAnnouncementStatDto> GetAnnouncementStatAsync(TaktAnnouncementStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.PublishTimeStart,
            queryDto.PublishTimeEnd,
            queryDto.StatMonth,
            defaultMonthsAgo: 0);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktAnnouncement, bool>> inMonth = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PublishTime != null
            && x.PublishTime >= start
            && x.PublishTime <= end;
        var monthAnnouncementCount = await _announcementRepository.CountAsync(inMonth);
        return new TaktAnnouncementStatDto
        {
            StatMonth = statMonth,
            MonthAnnouncementCount = monthAnnouncementCount,
        };
    }
}

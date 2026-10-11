// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.Announcement
// 文件名称：ITaktAnnouncementStatService.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知看板统计服务接口（与 Announcement CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Routine.Announcement;

namespace Takt.Application.Services.Routine.Announcement;

/// <summary>
/// 公告通知看板统计服务（读 TaktAnnouncement；与 CRUD 服务分离）
/// </summary>
public interface ITaktAnnouncementStatService
{
    /// <summary>
    /// 获取公告通知件数统计（数据看板；按 PublishTime）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>公告通知件数统计</returns>
    Task<TaktAnnouncementStatDto> GetAnnouncementStatAsync(TaktAnnouncementStatQueryDto queryDto);
}

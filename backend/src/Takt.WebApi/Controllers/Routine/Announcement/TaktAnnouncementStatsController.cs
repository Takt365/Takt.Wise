// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Routine.Announcement
// 文件名称：TaktAnnouncementStatsController.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知看板统计控制器（与 TaktAnnouncements CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Application.Services.Routine.Announcement;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Routine.Announcement;

/// <summary>
/// 公告通知看板统计控制器（与 CRUD 控制器分离）
/// </summary>
[ApiModule(2, "日常事务")]
[Route("api/[controller]", Name = "公告通知统计")]
public class TaktAnnouncementStatsController : TaktControllerBase
{
    private readonly ITaktAnnouncementStatService _announcementStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="announcementStatService">公告统计服务</param>
    public TaktAnnouncementStatsController(ITaktAnnouncementStatService announcementStatService)
    {
        _announcementStatService = announcementStatService;
    }

    /// <summary>
    /// 公告通知件数统计（数据看板；按 PublishTime）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>公告通知件数统计</returns>
    [TaktPermission("routine:announcement:list", "公告通知件数统计")]
    [HttpGet("announcement-stat")]
    public async Task<IActionResult> GetAnnouncementStatAsync([FromQuery] TaktAnnouncementStatQueryDto queryDto)
    {
        try
        {
            var result = await _announcementStatService.GetAnnouncementStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

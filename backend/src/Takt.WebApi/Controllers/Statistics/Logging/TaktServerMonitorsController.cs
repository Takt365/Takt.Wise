// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Statistics.Logging
// 文件名称：TaktServerMonitorsController.cs
// 创建时间：2026-06-05
// 创建人：Takt365(Cursor AI)
// 功能描述：服务器监控控制器，提供硬件信息、应用状态查询与缓存刷新 API
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Statistics.Logging;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Statistics.Logging;

/// <summary>
/// 服务器监控控制器
/// 提供服务器硬件与应用运行状态查询 REST API
/// </summary>
[ApiModule(9, "统计日志")]
[Route("api/[controller]", Name = "服务器监控")]
public class TaktServerMonitorsController : TaktControllerBase
{
    private readonly ITaktServerMonitorService _serverMonitorService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serverMonitorService">服务器监控服务</param>
    public TaktServerMonitorsController(ITaktServerMonitorService serverMonitorService)
    {
        _serverMonitorService = serverMonitorService;
    }

    /// <summary>
    /// 获取服务器硬件信息
    /// </summary>
    /// <returns>服务器硬件信息 DTO</returns>
    [TaktPermission("statistics:logging:server:monitor:list", "服务器硬件信息")]
    [HttpGet("hardware")]
    public async Task<IActionResult> GetServerHardwareAsync()
    {
        try
        {
            var result = await _serverMonitorService.GetServerHardwareAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取应用运行状态
    /// </summary>
    /// <returns>应用运行状态 DTO</returns>
    [TaktPermission("statistics:logging:server:monitor:list", "应用运行状态")]
    [HttpGet("app-status")]
    public async Task<IActionResult> GetAppStatusAsync()
    {
        try
        {
            var result = await _serverMonitorService.GetAppStatusAsync();
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 刷新硬件信息缓存
    /// </summary>
    /// <returns>操作结果</returns>
    [TaktPermission("statistics:logging:server:monitor:refresh", "刷新硬件信息缓存")]
    [HttpPost("refresh-cache")]
    public IActionResult RefreshHardwareCache()
    {
        try
        {
            _serverMonitorService.RefreshHardwareCache();
            return Success("刷新成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

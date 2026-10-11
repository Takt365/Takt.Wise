// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Foundation
// 文件名称：TaktIpGeolocationsController.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询控制器（ip2region / TaktLocationHelper）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Foundation;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Foundation;

/// <summary>
/// IP 归属查询控制器
/// </summary>
[ApiModule(8, "基础设置")]
[Route("api/[controller]", Name = "IP归属")]
public class TaktIpGeolocationsController : TaktControllerBase
{
    private readonly ITaktIpGeolocationService _ipGeolocationService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ipGeolocationService">IP 归属查询服务</param>
    public TaktIpGeolocationsController(ITaktIpGeolocationService ipGeolocationService)
    {
        _ipGeolocationService = ipGeolocationService;
    }

    /// <summary>
    /// 按 IP 查询归属地
    /// </summary>
    /// <param name="ip">IPv4 或 IPv6</param>
    /// <returns>归属结果 DTO</returns>
    [TaktPermission("foundation:ip:geolocation:list", "IP归属")]
    [HttpGet("search")]
    public async Task<IActionResult> SearchIpGeolocationAsync([FromQuery] string ip)
    {
        try
        {
            var result = await _ipGeolocationService.SearchIpGeolocationAsync(ip);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 查询当前请求客户端 IP 归属地
    /// </summary>
    /// <returns>归属结果 DTO</returns>
    [TaktPermission("foundation:ip:geolocation:list", "客户端IP归属")]
    [HttpGet("client")]
    public async Task<IActionResult> SearchClientIpGeolocationAsync()
    {
        try
        {
            var clientIp = TaktLocationHelper.ResolveClientIp(HttpContext);
            if (string.IsNullOrWhiteSpace(clientIp))
            {
                return BadRequest("无法解析客户端 IP");
            }

            var result = await _ipGeolocationService.SearchIpGeolocationAsync(clientIp);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

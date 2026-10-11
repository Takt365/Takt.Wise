// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Output
// 文件名称：TaktAssyOutputStatsController.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：组立产出看板统计控制器（与 TaktAssyOutputs CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Application.Services.Logistics.Manufacturing.Output;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Output;

/// <summary>
/// 组立产出看板统计控制器（与 CRUD 控制器分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "组立生产统计")]
public class TaktAssyOutputStatsController : TaktControllerBase
{
    private readonly ITaktAssyOutputStatService _assyOutputStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assyOutputStatService">组立产出统计服务</param>
    public TaktAssyOutputStatsController(ITaktAssyOutputStatService assyOutputStatService)
    {
        _assyOutputStatService = assyOutputStatService;
    }

    /// <summary>
    /// 获取组立生产统计（数据看板 production-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>组立生产统计</returns>
    [TaktPermission("logistics:manufacturing:output:assy:list", "组立生产统计")]
    [HttpGet("production-stat")]
    public async Task<IActionResult> GetAssyOutputProductionStatAsync([FromQuery] TaktOutputProductionStatQueryDto queryDto)
    {
        try
        {
            var result = await _assyOutputStatService.GetAssyOutputProductionStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairStatsController.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 改修不良看板统计控制器（与 TaktPcbaRepairs CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Application.Services.Logistics.Manufacturing.Defect;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA 改修不良看板统计控制器（与 CRUD 控制器分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA改修不良统计")]
public class TaktPcbaRepairStatsController : TaktControllerBase
{
    private readonly ITaktPcbaRepairStatService _pcbaRepairStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairStatService">PCBA 改修统计服务</param>
    public TaktPcbaRepairStatsController(ITaktPcbaRepairStatService pcbaRepairStatService)
    {
        _pcbaRepairStatService = pcbaRepairStatService;
    }

    /// <summary>
    /// 获取 PCBA 改修不良统计（数据看板 defect-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 改修统计</returns>
    [TaktPermission("logistics:manufacturing:defect:pcba:repair:list", "PCBA改修不良统计")]
    [HttpGet("defect-stat")]
    public async Task<IActionResult> GetPcbaRepairStatAsync([FromQuery] TaktDefectStatQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaRepairStatService.GetPcbaRepairStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

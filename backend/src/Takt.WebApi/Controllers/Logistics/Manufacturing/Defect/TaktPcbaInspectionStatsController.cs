// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionStatsController.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 检查不良看板统计控制器（与 TaktPcbaInspections CRUD 分离）
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
/// PCBA 检查不良看板统计控制器（与 CRUD 控制器分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "PCBA检查不良统计")]
public class TaktPcbaInspectionStatsController : TaktControllerBase
{
    private readonly ITaktPcbaInspectionStatService _pcbaInspectionStatService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionStatService">PCBA 检查统计服务</param>
    public TaktPcbaInspectionStatsController(ITaktPcbaInspectionStatService pcbaInspectionStatService)
    {
        _pcbaInspectionStatService = pcbaInspectionStatService;
    }

    /// <summary>
    /// 获取 PCBA 检查不良统计（数据看板 defect-stat）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 检查统计</returns>
    [TaktPermission("logistics:manufacturing:defect:mfg2:inspection:list", "PCBA检查不良统计")]
    [HttpGet("defect-stat")]
    public async Task<IActionResult> GetPcbaInspectionStatAsync([FromQuery] TaktDefectStatQueryDto queryDto)
    {
        try
        {
            var result = await _pcbaInspectionStatService.GetPcbaInspectionStatAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

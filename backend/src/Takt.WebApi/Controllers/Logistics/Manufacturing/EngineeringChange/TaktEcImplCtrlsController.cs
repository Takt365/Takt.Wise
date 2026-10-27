// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcImplCtrlsController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：实施管控控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 实施管控控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "实施管控")]
public class TaktEcImplCtrlsController : TaktControllerBase
{
    private readonly ITaktEcImplCtrlService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcImplCtrlsController(ITaktEcImplCtrlService service) => _service = service;

    /// <summary>
    /// 获取实施管控列表（分页）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:implctrl:list", "实施管控列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcImplCtrlListAsync([FromQuery] TaktEcImplCtrlQueryDto queryDto)
    {
        try { var result = await _service.GetEcImplCtrlListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取实施管控详情
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:implctrl:query", "实施管控详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcImplCtrlByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcImplCtrlByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("实施管控不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 更新实施管控
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:implctrl:update", "更新实施管控")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcImplCtrlAsync(long ecDetailId, [FromBody] TaktEcImplCtrlUpdateDto dto)
    {
        try { var result = await _service.UpdateEcImplCtrlAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 导出实施管控
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:implctrl:export", "导出实施管控")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcImplCtrlAsync([FromQuery] TaktEcImplCtrlQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcImplCtrlAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}

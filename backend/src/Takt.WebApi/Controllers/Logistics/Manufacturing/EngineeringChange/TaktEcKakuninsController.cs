// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKakuninsController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：物料确认控制器
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
/// 物料确认控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "物料确认")]
public class TaktEcKakuninsController : TaktControllerBase
{
    private readonly ITaktEcKakuninService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcKakuninsController(ITaktEcKakuninService service) => _service = service;

    /// <summary>
    /// 获取物料确认列表（分页）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kakunin:list", "物料确认列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcKakuninListAsync([FromQuery] TaktEcKakuninQueryDto queryDto)
    {
        try { var result = await _service.GetEcKakuninListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取物料确认详情
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kakunin:query", "物料确认详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcKakuninByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcKakuninByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("物料确认不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 更新物料确认
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kakunin:update", "更新物料确认")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcKakuninAsync(long ecDetailId, [FromBody] TaktEcKakuninUpdateDto dto)
    {
        try { var result = await _service.UpdateEcKakuninAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 导出物料确认
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:kakunin:export", "导出物料确认")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcKakuninAsync([FromQuery] TaktEcKakuninQueryDto? query)
    {
        try { var (fileName, fileContent) = await _service.ExportEcKakuninAsync(query); return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); }
        catch (Exception ex) { return HandleException(ex); }
    }
}

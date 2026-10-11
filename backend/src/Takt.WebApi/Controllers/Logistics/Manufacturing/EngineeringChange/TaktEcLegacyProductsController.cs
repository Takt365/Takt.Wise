// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcLegacyProductsController.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：旧品管制控制器
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
/// 旧品管制控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "旧品管制")]
public class TaktEcLegacyProductsController : TaktControllerBase
{
    private readonly ITaktEcLegacyProductService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktEcLegacyProductsController(ITaktEcLegacyProductService service) => _service = service;

    /// <summary>
    /// 获取旧品管制列表（分页）
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:legacy:product:list", "旧品管制列表")]
    [HttpGet("list")]
    public async Task<IActionResult> GetEcLegacyProductListAsync([FromQuery] TaktEcLegacyProductQueryDto queryDto)
    {
        try { var result = await _service.GetEcLegacyProductListAsync(queryDto); return Success(result.Data, result.Total, result.PageIndex, result.PageSize, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 获取旧品管制详情
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:legacy:product:query", "旧品管制详情")]
    [HttpGet("detail/{ecDetailId}")]
    public async Task<IActionResult> GetEcLegacyProductByEcDetailIdAsync(long ecDetailId)
    {
        try { var result = await _service.GetEcLegacyProductByEcDetailIdAsync(ecDetailId); if (result == null) return NotFound("旧品管制不存在"); return Success(result, "查询成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 更新旧品管制
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:legacy:product:update", "更新旧品管制")]
    [HttpPut("detail/{ecDetailId}")]
    public async Task<IActionResult> UpdateEcLegacyProductAsync(long ecDetailId, [FromBody] TaktEcLegacyProductUpdateDto dto)
    {
        try { var result = await _service.UpdateEcLegacyProductAsync(ecDetailId, dto); return Success(result, "更新成功"); }
        catch (Exception ex) { return HandleException(ex); }
    }

    /// <summary>
    /// 导出旧品管制
    /// </summary>
    [TaktPermission("logistics:manufacturing:engineering:change:legacy:product:export", "导出旧品管制")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportEcLegacyProductAsync(
        [FromQuery] TaktEcLegacyProductQueryDto? query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, fileContent) = await _service.ExportEcLegacyProductAsync(query, sheetName, exportName);
            return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

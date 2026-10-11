// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomPriceDeltaTrendsController.cs
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：成本差异推移控制器（独立菜单 list/export）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Manufacturing.Bom;
using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Manufacturing.Bom;

/// <summary>
/// 成本差异推移控制器
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "成本差异推移")]
public class TaktBomPriceDeltaTrendsController : TaktControllerBase
{
    private readonly ITaktBomPriceDeltaTrendService _service;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="service">成本差异推移服务</param>
    public TaktBomPriceDeltaTrendsController(ITaktBomPriceDeltaTrendService service)
    {
        _service = service;
    }

    /// <summary>
    /// 成本差异推移列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页结果</returns>
    [TaktPermission("logistics:manufacturing:bom:pricedelta:trend:list", "成本差异推移")]
    [HttpGet("list")]
    public async Task<IActionResult> GetBomPriceDeltaTrendListAsync(
        [FromQuery] TaktBomPriceDeltaTrendQueryDto queryDto)
    {
        try
        {
            var result = await _service.GetBomPriceDeltaTrendListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出成本差异推移
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:manufacturing:bom:pricedelta:trend:export", "导出成本差异推移")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportBomPriceDeltaTrendAsync(
        [FromQuery] TaktBomPriceDeltaTrendQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _service.ExportBomPriceDeltaTrendAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Manufacturing.Bom
// 文件名称：TaktBomMaterialUsagesController.cs
// 创建时间：2026-10-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 通用物料 / 专用物料控制器
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
/// BOM 通用物料与专用物料（与成本 CRUD 分离）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "BOM通用物料与专用物料")]
public class TaktBomMaterialUsagesController : TaktControllerBase
{
    private readonly ITaktBomMaterialUsageService _bomMaterialUsageService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="bomMaterialUsageService">通用物料 / 专用物料服务</param>
    public TaktBomMaterialUsagesController(ITaktBomMaterialUsageService bomMaterialUsageService)
    {
        _bomMaterialUsageService = bomMaterialUsageService;
    }

    /// <summary>
    /// 通用物料分页
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:manufacturing:bom:common:material:list", "通用物料")]
    [HttpGet("common-material")]
    public async Task<IActionResult> GetBomCommonMaterialListAsync(
        [FromQuery] TaktBomMaterialUsageQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialUsageService.GetBomCommonMaterialListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 专用物料分页
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:manufacturing:bom:dedicated:material:list", "专用物料")]
    [HttpGet("dedicated-material")]
    public async Task<IActionResult> GetBomDedicatedMaterialListAsync(
        [FromQuery] TaktBomMaterialUsageQueryDto queryDto)
    {
        try
        {
            var result = await _bomMaterialUsageService.GetBomDedicatedMaterialListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 通用物料页的物料下拉（明细 ComponentCode）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算年月 yyyy-MM</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:bom:common:material:list", "通用物料")]
    [HttpGet("common-material/component-options")]
    public async Task<IActionResult> GetBomCommonMaterialComponentOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? costingPeriod = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomMaterialUsageService.GetBomMaterialUsageComponentOptionsAsync(
                plantCode, costingPeriod, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 专用物料页的物料下拉（明细 ComponentCode）
    /// </summary>
    /// <param name="plantCode">工厂</param>
    /// <param name="costingPeriod">核算年月 yyyy-MM</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:manufacturing:bom:dedicated:material:list", "专用物料")]
    [HttpGet("dedicated-material/component-options")]
    public async Task<IActionResult> GetBomDedicatedMaterialComponentOptionsAsync(
        [FromQuery] string? plantCode = null,
        [FromQuery] string? costingPeriod = null,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _bomMaterialUsageService.GetBomMaterialUsageComponentOptionsAsync(
                plantCode, costingPeriod, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出通用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:common:material:export", "导出通用物料")]
    [HttpGet("common-material/export")]
    public async Task<IActionResult> ExportBomCommonMaterialAsync(
        [FromQuery] TaktBomMaterialUsageQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialUsageService.ExportBomCommonMaterialAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出专用物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="exportName">导出文件名</param>
    /// <returns>Excel 文件</returns>
    [TaktPermission("logistics:manufacturing:bom:dedicated:material:export", "导出专用物料")]
    [HttpGet("dedicated-material/export")]
    public async Task<IActionResult> ExportBomDedicatedMaterialAsync(
        [FromQuery] TaktBomMaterialUsageQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (resultFileName, fileContent) = await _bomMaterialUsageService.ExportBomDedicatedMaterialAsync(
                query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, resultFileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}

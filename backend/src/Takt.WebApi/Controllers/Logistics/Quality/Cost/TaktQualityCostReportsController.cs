// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Logistics.Quality.Cost
// 文件名称：TaktQualityCostReportsController.cs
// 创建时间：2026-10-23
// 创建人：Takt365(Cursor AI)
// 功能描述：品质成本报告控制器
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Dtos.Logistics.Quality.Cost;
using Takt.Application.Services.Logistics.Quality.Cost;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;

namespace Takt.WebApi.Controllers.Logistics.Quality.Cost;

/// <summary>
/// 品质成本报告（品质保证、品质问题、品质事故）
/// </summary>
[ApiModule(4, "后勤管理")]
[Route("api/[controller]", Name = "品质成本报告")]
public class TaktQualityCostReportsController : TaktControllerBase
{
    private readonly ITaktQualityCostReportService _qualityCostReportService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="qualityCostReportService">品质成本报告服务</param>
    public TaktQualityCostReportsController(ITaktQualityCostReportService qualityCostReportService)
    {
        _qualityCostReportService = qualityCostReportService;
    }

    /// <summary>
    /// 工厂选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:quality:cost:report:list", "品质成本报告工厂选项")]
    [HttpGet("plant-options")]
    public async Task<IActionResult> GetQualityCostReportPlantOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityCostReportService.GetQualityCostReportPlantOptionsAsync(statTab, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 维度选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:quality:cost:report:list", "品质成本报告维度选项")]
    [HttpGet("dimension-options")]
    public async Task<IActionResult> GetQualityCostReportDimensionOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityCostReportService.GetQualityCostReportDimensionOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 币种选项
    /// </summary>
    /// <param name="statTab">页签</param>
    /// <param name="plantCode">工厂</param>
    /// <param name="keyword">关键字</param>
    /// <returns>选项</returns>
    [TaktPermission("logistics:quality:cost:report:list", "品质成本报告币种选项")]
    [HttpGet("currency-options")]
    public async Task<IActionResult> GetQualityCostReportCurrencyOptionsAsync([FromQuery] string? statTab = null, [FromQuery] string? plantCode = null, [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _qualityCostReportService.GetQualityCostReportCurrencyOptionsAsync(statTab, plantCode, keyword);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 品质成本报告列表
    /// </summary>
    /// <param name="queryDto">查询</param>
    /// <returns>分页行</returns>
    [TaktPermission("logistics:quality:cost:report:list", "品质成本报告")]
    [HttpGet("list")]
    public async Task<IActionResult> GetQualityCostReportListAsync([FromQuery] TaktQualityCostReportQueryDto queryDto)
    {
        try
        {
            var result = await _qualityCostReportService.GetQualityCostReportListAsync(queryDto);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 导出品质成本报告
    /// </summary>
    /// <param name="query">查询</param>
    /// <param name="sheetName">工作表</param>
    /// <param name="exportName">文件名</param>
    /// <returns>Excel</returns>
    [TaktPermission("logistics:quality:cost:report:export", "清单导出品质成本报告")]
    [HttpGet("export")]
    public async Task<IActionResult> ExportQualityCostReportAsync(
        [FromQuery] TaktQualityCostReportQueryDto query,
        [FromQuery] string? sheetName = null,
        [FromQuery] string? exportName = null)
    {
        try
        {
            var (fileName, fileContent) = await _qualityCostReportService.ExportQualityCostReportAsync(query, sheetName, exportName);
            return File(fileContent, TaktExcelHelper.ExcelContentType, fileName);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}
